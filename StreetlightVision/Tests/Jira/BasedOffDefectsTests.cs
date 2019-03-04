using ImageMagick;
using NUnit.Framework;
using StreetlightVision.Extensions;
using StreetlightVision.Models;
using StreetlightVision.Pages;
using StreetlightVision.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace StreetlightVision.Tests.Jira
{
    [TestFixture]
    [NonParallelizable]
    public class BasedOffDefectsTests : TestBase
    {
        #region Variables

        private readonly string _alarmManagerExportedFilePattern = "*Alarm-Manager-Export*.csv";
        private readonly string _dataHistoryGridExportedFilePattern = "Data_History*.csv";
        private readonly string _monthlyEnergySavingsExportedFilePattern = "data*.csv";

        #endregion //Variables

        #region Contructors

        #endregion //Contructors        

        #region Test Cases

        [Test, DynamicRetry]
        [Description("SLV-1619 Equipment Inventory - Cannot select a device after it has been affected to a different")]
        public void SLV_1619()
        {
            var testData = GetTestDataOfSLV_1619();
            var controllerId1 = testData["ControllerId1"];
            var controllerName1 = testData["ControllerName1"];
            var controllerId2 = testData["ControllerId2"];
            var controllerName2 = testData["ControllerName2"];
            var geozone = SLVHelper.GenerateUniqueName("GZNSLV1619");
            var streetlight = SLVHelper.GenerateUniqueName("STL");
            var streetlightPath = geozone + @"\" + streetlight;

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            Step("-> Create data for testing");
            DeleteGeozones("GZNSLV1619*");
            CreateNewGeozone(geozone);
            CreateNewDevice(DeviceType.Streetlight, streetlight, controllerId1, geozone);

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.EquipmentInventory);

            Step("1. Go to 'Equipement Inventory' page");
            Step("2. Expected Equipment Inventory page is routed and loaded successfully");
            var equipmentInventoryPage = desktopPage.GoToApp(App.EquipmentInventory) as EquipmentInventoryPage;

            Step("3. Select a streetlight in a geozone");
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(streetlightPath);

            Step("4. Expected Luminaire Control widget appears from the right side");
            equipmentInventoryPage.WaitForEditorPanelDisplayed();

            Step("5. Change its Controller to a different one then save");
            equipmentInventoryPage.StreetlightEditorPanel.SelectControllerIdDropDown(controllerName2);
            equipmentInventoryPage.StreetlightEditorPanel.ClickSaveButton();
            equipmentInventoryPage.WaitForPreviousActionComplete();

            Step("6. Expected Its widget closes");
            equipmentInventoryPage.WaitForEditorPanelDisappeared();

            Step("7. Select the same streetlight");
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(streetlight);

            Step("8. Expected No error message is displayed (According to the defect, message with text 'Device 'XXX' not found on controller 'YYY'!' is dislayed");
            var isPopupDialogDisplayed = equipmentInventoryPage.IsPopupDialogDisplayed();
            VerifyEqual("8. Verify No error message is displayed", false, isPopupDialogDisplayed);

            try
            {
                DeleteGeozone(geozone);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("SLV-1680 Equipment Inventory - Virtual Energy Consumption is hidden in the geozone widget after reopening the widget")]
        public void SLV_1680()
        {
            var testData = GetTestDataOfSLV_1680();
            var geozone = testData["GeoZone"];

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.EquipmentInventory);

            Step("1. Go to Equipment Inventory app");
            Step("2. Expected Equipment Inventory page is routed and loaded successfully");
            var equipmentInventoryPage = desktopPage.GoToApp(App.EquipmentInventory) as EquipmentInventoryPage;

            Step("3. Select a geozone");
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(geozone);

            Step("4. Expected Its widget appears and displays Virtual Energy Consumption data for each month");
            equipmentInventoryPage.WaitForEditorPanelDisplayed();
            VerifyEqual("4. Verify widget appears", true, equipmentInventoryPage.GeozoneEditorPanel.IsDisplayed());
            VerifyEqual("4. Verify Virtual Energy Consumption - JAN is correct", true, !string.IsNullOrEmpty(equipmentInventoryPage.GeozoneEditorPanel.GetJanuaryValue()));
            VerifyEqual("4. Verify Virtual Energy Consumption - FEB is correct", true, !string.IsNullOrEmpty(equipmentInventoryPage.GeozoneEditorPanel.GetFebruaryValue()));
            VerifyEqual("4. Verify Virtual Energy Consumption - MAR is correct", true, !string.IsNullOrEmpty(equipmentInventoryPage.GeozoneEditorPanel.GetMarchValue()));
            VerifyEqual("4. Verify Virtual Energy Consumption - APR is correct", true, !string.IsNullOrEmpty(equipmentInventoryPage.GeozoneEditorPanel.GetAprilValue()));
            VerifyEqual("4. Verify Virtual Energy Consumption - MAY is correct", true, !string.IsNullOrEmpty(equipmentInventoryPage.GeozoneEditorPanel.GetMayValue()));
            VerifyEqual("4. Verify Virtual Energy Consumption - JUN is correct", true, !string.IsNullOrEmpty(equipmentInventoryPage.GeozoneEditorPanel.GetJuneValue()));
            VerifyEqual("4. Verify Virtual Energy Consumption - JUL is correct", true, !string.IsNullOrEmpty(equipmentInventoryPage.GeozoneEditorPanel.GetJulyValue()));
            VerifyEqual("4. Verify Virtual Energy Consumption - AUG is correct", true, !string.IsNullOrEmpty(equipmentInventoryPage.GeozoneEditorPanel.GetAugustValue()));
            VerifyEqual("4. Verify Virtual Energy Consumption - SEP is correct", true, !string.IsNullOrEmpty(equipmentInventoryPage.GeozoneEditorPanel.GetSeptemberValue()));
            VerifyEqual("4. Verify Virtual Energy Consumption - OCT is correct", true, !string.IsNullOrEmpty(equipmentInventoryPage.GeozoneEditorPanel.GetOctoberValue()));
            VerifyEqual("4. Verify Virtual Energy Consumption - NOV is correct", true, !string.IsNullOrEmpty(equipmentInventoryPage.GeozoneEditorPanel.GetNovemberValue()));
            VerifyEqual("4. Verify Virtual Energy Consumption - DEC is correct", true, !string.IsNullOrEmpty(equipmentInventoryPage.GeozoneEditorPanel.GetDecemberValue()));

            Step("5. Click Right Arrow icon on the widget to close the widget");
            equipmentInventoryPage.GeozoneEditorPanel.ClickBackButton();
            equipmentInventoryPage.WaitForEditorPanelDisappeared();

            Step("6. Expected Its widget closes");
            VerifyEqual("6. Verify widget is closed", false, equipmentInventoryPage.GeozoneEditorPanel.IsDisplayed());

            Step("7. Select that geozone again");
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(geozone);

            Step("8. Expected Its widget appears again and the Virtual Energy Consumption data is visible (According to the defect, Virtual Energy Consumption section is invisible)");
            equipmentInventoryPage.WaitForEditorPanelDisplayed();
            VerifyEqual("8. Verify widget appears", true, equipmentInventoryPage.GeozoneEditorPanel.IsDisplayed());
            VerifyEqual("8. Verify Virtual Energy Consumption data is visible", true, equipmentInventoryPage.GeozoneEditorPanel.IsVirtualEnergyConsumptionVisible());
        }

        [Test, DynamicRetry]
        [Description("SLV-1799 Real Time Control - Metering values are not associated with the correct labels")]
        public void SLV_1799()
        {
            var testData = GetTestDataOfSLV_1799();
            var geozone = testData["Geozone"].ToString();
            var streetlights = testData["Streetlights"] as List<DeviceModel>;
            var streetlightName = streetlights.PickRandom().Name;
            var streetlightPath = geozone + @"\" + streetlightName;

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");
            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.RealTimeControl);

            Step("1. Go to Real-time Control app");
            Step("2. Expected Real-time Control page is routed and loaded successfully");
            var realtimeControlPage = desktopPage.GoToApp(App.RealTimeControl) as RealTimeControlPage;

            Step("3. Select a streetlight in a geozone (e.g. 'Telematics 01' in 'Real Time Control Area' geozone)");
            realtimeControlPage.GeozoneTreeMainPanel.SelectNode(streetlightPath);

            Step("4. Expected Luminaire Control widget appears from the right side");
            realtimeControlPage.WaitForStreetlightWidgetDisplayed(streetlightName);
            realtimeControlPage.StreetlightWidgetPanel.WaitForLastUpdatedTimeText();

            Step("5.Verify Metering section in Luminaire Control widget");
            Step("6.Expected Values after loaded are displayed in correct order correspondingly to Attributes. Because it's hard to device which value belongs to which attribute, just need to verify value of 'Lamp burning hours' ends with 'h' and of 'Mains voltage (V)' ends with 'V'");
            VerifyLampBurningHoursAndMainsVoltage(realtimeControlPage);

            Step("7. Click Refresh button in Luminaire Control widget and wait for values are reloaded");
            realtimeControlPage.StreetlightWidgetPanel.ClickRefreshButton();
            realtimeControlPage.WaitForPreviousActionComplete();
            VerifyLampBurningHoursAndMainsVoltage(realtimeControlPage);
        }

        [Test, DynamicRetry]
        [Description("SLV-1800 Real Time Control - Tooltip never updated")]
        public void SLV_1800()
        {
            var testData = GetTestDataOfSLV_1800();
            var geoZonePath = testData["GeoZone"].ToString();
            var streetlights = testData["Streetlights"] as List<DeviceModel>;
            var streetlightsLatLong = streetlights.Select(p => string.Format("{0};{1}", p.Longitude, p.Latitude)).ToArray();

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.RealTimeControl);

            Step("1. Go to Real-time Control app");
            Step("2. Expected Real-time Control page is routed and loaded successfully");
            var realtimeControlPage = desktopPage.GoToApp(App.RealTimeControl) as RealTimeControlPage;

            Step("3. Select a geozone (whose devices are alive and controllable e.g. Real Time Control Area) and zoom in it until devices can be seen on the map");
            realtimeControlPage.GeozoneTreeMainPanel.SelectNode(geoZonePath);

            Step("4. Mouse-hover onto all streetlights and note content of each popup when they appear");
            var dicStatus = realtimeControlPage.Map.GetStreetlightsStatusGL(streetlightsLatLong);

            Step("5. Select a streetlight");
            var randomStreetlightName = dicStatus.Keys.ToList().PickRandom();
            var randomStreetlightStatus = dicStatus[randomStreetlightName];
            realtimeControlPage.GeozoneTreeMainPanel.SelectNode(randomStreetlightName);

            Step("6. Expected Luminare Control widget appears");
            realtimeControlPage.WaitForStreetlightWidgetDisplayed(randomStreetlightName);

            Step("7. Mouse-hover onto the selected streetlight to see its popup");
            realtimeControlPage.Map.MoveToSelectedDeviceGL();

            Step("8. Expected Popup's content has been updated in comparision with step #4 (e.g Status is not ' ? ' any more)");
            var selectedStreetlightStatus = realtimeControlPage.Map.GetDeviceStatusGL();
            VerifyTrue(string.Format("Verify {0}'s Status is not '?' any more (Status: '{1}')", randomStreetlightName, selectedStreetlightStatus), selectedStreetlightStatus != randomStreetlightStatus && selectedStreetlightStatus != "?", selectedStreetlightStatus, randomStreetlightStatus);

            Step("9. Close Luminare Control widget if any");
            realtimeControlPage.StreetlightWidgetPanel.ClickCloseButton();
            realtimeControlPage.WaitForStreetlightWidgetDisappeared();
            realtimeControlPage.AppBar.ClickHeaderBartop();
            realtimeControlPage.GeozoneTreeMainPanel.SelectNode(geoZonePath);

            Step("10. Mouse-hover onto Real-time icon of top right corner and click Refresh icon in it");
            realtimeControlPage.Map.MoveToGlobalEarthIcon();
            realtimeControlPage.Map.WaitForRealtimeRefreshPanelDisplayed();
            realtimeControlPage.Map.ClickRealtimeRefreshButton();

            Step("11. Wait until refreshing finishes");
            realtimeControlPage.Map.WaitForProgressGLCompleted();

            Step("12. Mouse-hover onto the rest of streetlights to see each popup");
            Step("13. Expected Popup's content has been updated in comparision with step #4 (e.g Status is not ' Unknown ' any more)");
            var dicRefreshStatus = realtimeControlPage.Map.GetStreetlightsStatusGL(streetlightsLatLong);

            var notUpdateStreetlights = new List<string>();
            foreach (var streetlight in dicRefreshStatus.Keys)
            {
                var currentStatus = dicRefreshStatus[streetlight];
                var beforeStatus = dicStatus[streetlight];

                if (currentStatus != beforeStatus && currentStatus != "Unknown")
                {
                    VerifyTrue(string.Format("Verify {0}'s Status is not 'Unknown' any more (Status: '{1}')", streetlight, currentStatus), true, currentStatus, beforeStatus);
                }
                else
                {
                    notUpdateStreetlights.Add(streetlight);
                }
            }

            Step("14. If after this step, a streetlight's popup hasn't been updated, select that device and click Refresh button in its Luminare Control widget");
            Step("15. Expected Popup's content has been updated in comparision with step #4 (e.g Status is not ' Unknown ' any more)");
            foreach (var streetlight in notUpdateStreetlights)
            {
                realtimeControlPage.GeozoneTreeMainPanel.SelectNode(randomStreetlightName);
                realtimeControlPage.WaitForStreetlightWidgetDisplayed(streetlight);
                realtimeControlPage.StreetlightWidgetPanel.ClickRefreshButton();
                realtimeControlPage.WaitForPreviousActionComplete();

                realtimeControlPage.Map.MoveToSelectedDeviceGL();
                var status = realtimeControlPage.Map.GetDeviceStatusGL();
                VerifyTrue(string.Format("Verify {0}'s Status is not 'Unknown' any more (Status: '{1}')", streetlight, status), status != "Unknown", "OK", status);
            }
        }

        [Test, DynamicRetry]
        [Description("SLV-2111 Report Manager - Report Details 'auto' and Filtering Mode 'no filter' not shown")]
        public void SLV_2111()
        {
            var testData = GetTestDataOfSLV_2111();

            var expectedGeoZone = testData["GeoZone"].ToString();
            var reportPrefixInput = testData["ReportPrefix"].ToString();
            var reportTypeInput = testData["ReportType"].ToString();
            var propertiesTabListInput = testData["PropertiesTab"] as List<string>;
            var schedulerTabListInput = testData["SchedulerTab"] as List<string>;
            var mailTabListInput = testData["MailTab"] as List<string>;

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.ReportManager);

            Step("1. Go to Report Manager page from Desktop page or App Switch");
            Step("2. Expected Report Manager page is routed and loaded successfully");
            var reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;

            Step("3. Create and save new report with parameters:");
            reportManagerPage.GeozoneTreeMainPanel.SelectNode(expectedGeoZone);
            reportManagerPage.GridPanel.ClickAddReportToolbarButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.ReportEditorPanel.WaitForPanelLoaded();

            Step(" Name: Failures HTML report { { current time stamp} });");
            Step(" Type: Failure HTML report");
            Step(" Properties tab:");
            Step("  + Description: Any");
            Step("  + Report details: Auto");
            Step("  + Filtering mode: No filter");
            Step(" Scheduler tab:");
            Step("  + Periodicity: Everyday");
            Step("  + Hour and minute: Current time + 2 minute");
            Step(" Mail tab:");
            Step("  + Subject: Any");
            Step("  + From: qa@streetlightmonitoring.com");
            Step("  + Contacts: testing_contact selected from the list");
            Step("  + Configuration: HTML format: unchecked");
            var reportName = SLVHelper.GenerateUniqueName(string.Format("[{0}] {1}", Settings.Users["DefaultTest"].Username, Settings.CurrentTestWebDriverKeyName));

            reportManagerPage.ReportEditorPanel.EnterNameInput(reportName);
            reportManagerPage.ReportEditorPanel.SelectTypeDropDown(reportTypeInput);
            reportManagerPage.ReportEditorPanel.WaitForPreviousActionComplete();

            // Properties tab
            var expectedDescription = propertiesTabListInput.ElementAt(0);
            var expectedReportDetails = propertiesTabListInput.ElementAt(1);
            var expectedFilteringMode = propertiesTabListInput.ElementAt(2);
            reportManagerPage.ReportEditorPanel.EnterDescriptionInput(expectedDescription);
            reportManagerPage.ReportEditorPanel.SelectReportDetailsDropDown(expectedReportDetails);
            reportManagerPage.ReportEditorPanel.SelectFilteringModeDropDown(expectedFilteringMode);

            // Scheduler tab
            var currentDate = Settings.GetLocalTime();
            reportManagerPage.ReportEditorPanel.SelectTab("Scheduler");
            reportManagerPage.ReportEditorPanel.SelectPeriodicityDropDown(schedulerTabListInput.ElementAt(0));
            reportManagerPage.ReportEditorPanel.SelectHourDropDown(string.Format("{0:D2}", currentDate.Hour));
            reportManagerPage.ReportEditorPanel.SelectMinuteDropDown(string.Format("{0:D2}", currentDate.Minute + 3));

            // Export tab
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            var expectedSubjectInput = string.Format("[{0}][{1}] {2}", Settings.Users["DefaultTest"].Username, Settings.CurrentTestWebDriverKeyName, mailTabListInput.ElementAt(0));
            var expectedSenderInput = mailTabListInput.ElementAt(1);
            reportManagerPage.ReportEditorPanel.EnterSubjectInput(expectedSubjectInput);
            reportManagerPage.ReportEditorPanel.EnterFromInput(expectedSenderInput);
            reportManagerPage.ReportEditorPanel.SelectContactsListDropDown(mailTabListInput.ElementAt(2));
            reportManagerPage.ReportEditorPanel.TickHtmlFormatCheckbox(false);

            reportManagerPage.ReportEditorPanel.ClickSaveButton();
            reportManagerPage.WaitForPreviousActionComplete();

            Step("4. Refresh browser then go to Report Manager again");
            desktopPage = Browser.RefreshLoggedInCMS();
            reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.GeozoneTreeMainPanel.SelectNode(expectedGeoZone);

            Step("5. Select the report created at step #3");
            reportManagerPage.GridPanel.ClickGridRecord(reportName);

            Step("6. Expected Verify in Properties tab, saved values (Report details & Filtering mode) are kept instead of being empty:");
            Step(" + Report details: Auto");
            Step(" + Filtering mode: No filter");
            var actualReportDetail = reportManagerPage.ReportEditorPanel.GetReportDetailsValue();
            var actualFilteringMode = reportManagerPage.ReportEditorPanel.GetFilteringModeValue();
            VerifyTrue("Verify Saved values (Report details & Filtering mode) are kept instead of being empty", expectedReportDetails.Equals(actualReportDetail) && expectedFilteringMode.Equals(actualFilteringMode), string.Format("Report details: '{0}' - Filtering mode: '{1}'", expectedReportDetails, expectedFilteringMode), string.Format("Report details: '{0}' - Filtering mode: '{1}'", actualReportDetail, actualFilteringMode));

            //Delete report
            reportManagerPage.ReportEditorPanel.ClickDeleteButton();
            reportManagerPage.Dialog.WaitForPanelLoaded();
            reportManagerPage.Dialog.ClickYesButton();
            reportManagerPage.WaitForPreviousActionComplete();
        }

        [Test, DynamicRetry]
        [Description("SLV-2227 Context lost when opening Data History")]
        public void SLV_2227()
        {
            var testData = GetTestDataOfSLV_2227();
            var searchName = testData["SearchName"];

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.DataHistory, App.EquipmentInventory);

            Step("1. Go to Data History app");
            Step("2. Expected Data History page is routed and loaded successfully");
            var dataHistoryPage = desktopPage.GoToApp(App.DataHistory) as DataHistoryPage;

            Step("3. Open Data History, search for a device (e.g enter 'A' and click Search button)");
            Step("4. Expected Found device(s) is displayed in search result list (grid) (e.g list of found devices starting with A')");
            dataHistoryPage.GeozoneTreeMainPanel.ChangeSearchAttribute("Name", "Contains");
            dataHistoryPage.GeozoneTreeMainPanel.EnterSearchTextInput(searchName);
            dataHistoryPage.GeozoneTreeMainPanel.ClickSearchButton();

            Step("5. Click on one device in the search result list (e.g Auto-ABC)");
            Step("6. Wait for the device table to be displayed");
            var record = dataHistoryPage.GeozoneTreeMainPanel.SearchResultsGeozonePanel.SelectFirstFoundDevice();
            dataHistoryPage.WaitForPreviousActionComplete();
            var expectedDeviceName = record[0];
            var expectedGeozoneName = record[1];

            dataHistoryPage.GridPanel.WaitForGridContentAvailable();
            var table = dataHistoryPage.GridPanel.BuildDataTableFromGrid();
            VerifyTrue("Verify the device table has records", table.Rows.Count > 0, "Have records", "No records");

            Step("7. Directly go to Equipement Inventory (from App Switch)");
            var equipmentInventorypage = dataHistoryPage.AppBar.SwitchTo(App.EquipmentInventory) as EquipmentInventoryPage;

            Step("8. Expected The clicked device at step #5 (e.g Auto-ABC) should be selected in the geozone tree");
            Step("and Node details panel should displayed info (Name, Parent) of it (According to the defect,");
            Step("the device widget does not show up, the device mother geozone is displayed instead)");
            var actualSeletedNodeName = equipmentInventorypage.GeozoneTreeMainPanel.GetSelectedNodeText();
            VerifyEqual(string.Format("Verify The clicked device at step #5 ({0}) should be selected in the geozone tree", expectedDeviceName), expectedDeviceName, actualSeletedNodeName);

            var actualDevice = equipmentInventorypage.StreetlightEditorPanel.GetNameValue();
            var actualGeoZone = equipmentInventorypage.StreetlightEditorPanel.GetGeozoneValue();
            VerifyEqual(string.Format("Verify Name {0} is correct", expectedDeviceName), expectedDeviceName, actualDevice);
            VerifyEqual(string.Format("Verify Parent {0} is correct", expectedGeozoneName), expectedGeozoneName, actualGeoZone);
        }

        [Test, DynamicRetry]
        [Description("SLV-1260 Multi selection clears the Dimming Group when the user has not changed it")]
        [NonParallelizable]
        public void SLV_1260()
        {
            var testData = GetTestDataOfSLV_1260();
            var device1Node = (XmlNode)testData["Device1"];
            var device2Node = (XmlNode)testData["Device2"];
            var devicesLongLat = new List<string>();
            var geozone = SLVHelper.GenerateUniqueName("GZNSC1260");
            var name1 = SLVHelper.GenerateUniqueName("STL01");
            var controllerId1 = device1Node.GetAttrVal("controller-id");
            var dimmingGroup1 = device1Node.GetAttrVal("dimming-group");
            var latitude1 = SLVHelper.GenerateCoordinate("10.65971", "10.66324");
            var longitude1 = SLVHelper.GenerateCoordinate("106.49301", "106.49680");
            devicesLongLat.Add(string.Format("{0};{1}", longitude1, latitude1));
            var name2 = SLVHelper.GenerateUniqueName("STL02");
            var controllerId2 = device2Node.GetAttrVal("controller-id");
            var dimmingGroup2 = device2Node.GetAttrVal("dimming-group");
            var latitude2 = SLVHelper.GenerateCoordinate("10.65971", "10.66324");
            var longitude2 = SLVHelper.GenerateCoordinate("106.49301", "106.49680");
            devicesLongLat.Add(string.Format("{0};{1}", longitude2, latitude2));

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            Step("-> Create data for testing");
            DeleteGeozones("GZNSC1260*");
            CreateNewGeozone(geozone, latMin: "10.65545", latMax: "10.66851", lngMin: "106.48009", lngMax: "106.50802");
            CreateNewDevice(DeviceType.Streetlight, name1, controllerId1, geozone, lat: latitude1, lng: longitude1);
            CreateNewDevice(DeviceType.Streetlight, name2, controllerId2, geozone, lat: latitude2, lng: longitude2);

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.EquipmentInventory);

            Step("1. Go to Equipment Inventory app");
            Step("2. Expected Equipment Inventory page is routed and loaded successfully");
            var equipmentInventoryPage = desktopPage.GoToApp(App.EquipmentInventory) as EquipmentInventoryPage;
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(geozone);

            Step("3. Set 2 different dimming groups for 2 Streetlights is going to be selected in the next step");
            // First device
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(name1);
            equipmentInventoryPage.StreetlightEditorPanel.SelectDimmingGroupDropDown(dimmingGroup1);
            equipmentInventoryPage.StreetlightEditorPanel.ClickSaveButton();
            equipmentInventoryPage.WaitForPreviousActionComplete();

            // Second device
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(name2);           
            equipmentInventoryPage.StreetlightEditorPanel.SelectDimmingGroupDropDown(dimmingGroup2);
            equipmentInventoryPage.StreetlightEditorPanel.ClickSaveButton();
            equipmentInventoryPage.WaitForPreviousActionComplete();

            Step("4. Select 2 Streetlights at step #3");
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(geozone);
            equipmentInventoryPage.Map.SelectDevicesGL(devicesLongLat);
            equipmentInventoryPage.WaitForMultipleDevicesEditorDisplayed();

            Step("5. Expected Multi-selection editor (Multi-selection Streetlight panel) appears");
            VerifyEqual("5. Verify multi-selection editor appears", true, equipmentInventoryPage.IsMultipleDevicesEditorPanelDisplayed());

            Step("6. Change a meaning different from the Dimming Group, e.g. Energy Provider");
            equipmentInventoryPage.MultipleDevicesEditorPanel.SelectTab("Electricity network");
            equipmentInventoryPage.MultipleDevicesEditorPanel.SelectEnergySupplierDropDown(equipmentInventoryPage.MultipleDevicesEditorPanel.GetListOfEnergySuppliers().PickRandom());

            Step("7. Click on the Save button");
            Step("8. Expected Saving is successful");
            equipmentInventoryPage.MultipleDevicesEditorPanel.ClickSaveButton();
            equipmentInventoryPage.WaitForPreviousActionComplete();

            Step("9. Select in turn each of selected streetlight that has been updated");
            Step("10. Expected Dimming group value of each one is still remained unchanged (the value set at step #3) because they has been not updated at step #6 (According to the defect, dimming group of each streetlight is set to blank)");
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(geozone);

            // First device
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(name1);
            equipmentInventoryPage.WaitForPreviousActionComplete();
            VerifyEqual("10. Verify dimming group of device 1 is remained unchanged", dimmingGroup1, equipmentInventoryPage.StreetlightEditorPanel.GetDimmingGroupValue());

            // Second device
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(name2);
            VerifyEqual("10. Verify dimming group of device 2 is remained unchanged", dimmingGroup2, equipmentInventoryPage.StreetlightEditorPanel.GetDimmingGroupValue());

            try
            {
                DeleteGeozone(geozone);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("SLV-408 Elapsed time since last value are calculated using client time instead of Controller time")]
        public void SLV_408()
        {
            var testData = GetTestDataOfSLV_408();
            var timeZone1Id = testData["TimeZone1Id"];
            var timeZone1Name = testData["TimeZone1Name"];
            var timeZone2Id = testData["TimeZone2Id"];
            var timeZone2Name = testData["TimeZone2Name"];            
            var geozone = SLVHelper.GenerateUniqueName("GZNSLV408");
            var controller = SLVHelper.GenerateUniqueName("CTRL");
            var streetlight = SLVHelper.GenerateUniqueName("STL");

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - Create a testing geozone containing a controller using a specific timezone. Ex: Central European Time [+01:00] [Europe/Paris]");
            Step(" - Create a streetlight using that controller and send a simulate command for it with:");
            Step("  + valueName=Current (Main Current)");
            Step("  + value=100");
            Step("  + eventTime= current datetime of testing timezone");
            Step("**** Precondition ****\n");

            Step("-> Create data for testing");
            DeleteGeozones("GZNSLV408*");
            CreateNewGeozone(geozone);
            CreateNewController(controller, geozone);
            SetValueToController(controller, "TimeZoneId", timeZone1Id, Settings.GetServerTime());
            CreateNewDevice(DeviceType.Streetlight, streetlight, controller, geozone);

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.EquipmentInventory, App.DataHistory);
            var currentTimeZone1DateTime = Settings.GetCurrentControlerDateTime(controller);
            var request = SetValueToDevice(controller, streetlight, "Current", "100", currentTimeZone1DateTime);
            VerifyEqual("-> Verify simulated request is sent successfully (attribute: Current, value: 100)", true, request);

            Step("1. Go to Data History app");
            Step("2. Expected Data History page is routed and loaded successfully");
            var dataHistoryPage = desktopPage.GoToApp(App.DataHistory) as DataHistoryPage;
           
            Step("3. Select the testing streetlight");
            dataHistoryPage.GeozoneTreeMainPanel.SelectNode(geozone + @"\" + streetlight);

            Step("4. Expected Last value panel of the selected device appears and it is selected in grid panel");
            dataHistoryPage.WaitForLastValuePanelDisplayed();
            dataHistoryPage.WaitForPreviousActionComplete();

            Step("5. Take note the elapsed time of 'Mains current' in Meterings tab");
            var attr = "Mains current";
            var date = dataHistoryPage.LastValuesPanel.GetMeteringsTooltipAttribute(attr);
            var time = dataHistoryPage.LastValuesPanel.GetMeteringsTimeAttribute(attr);

            Step("6. Expected The value is '# s'");
            VerifyTrue("6. Verify The value is '# s'", Regex.IsMatch(time, @"([0-9]*) s"), "# s", time);

            Step("7. Go to Equipment Inventory, select the testing controller, and change its timezone to Eastern European Time [+02:00] [Asia/Beirut], then save changes");
            var equipmentInventoryPage = dataHistoryPage.AppBar.SwitchTo(App.EquipmentInventory) as EquipmentInventoryPage;
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(controller);
            equipmentInventoryPage.ControllerEditorPanel.SelectTab("Time");
            equipmentInventoryPage.ControllerEditorPanel.SelectTimezoneDropDown(timeZone2Name);
            equipmentInventoryPage.ControllerEditorPanel.ClickSaveButton();
            equipmentInventoryPage.WaitForDeviceEditorPanelDisappeared();
            equipmentInventoryPage.WaitForPreviousActionComplete();

            Step("8. Go back to Data History and check elapsed time of 'Mains current' of the testing streetlight in Meterings tab");
            dataHistoryPage = equipmentInventoryPage.AppBar.SwitchTo(App.DataHistory) as DataHistoryPage;
            dataHistoryPage.LastValuesPanel.ClickBackToolbarButton();
            dataHistoryPage.WaitForLastValuePanelDisappeared();
            dataHistoryPage.GeozoneTreeMainPanel.SelectNode(streetlight);
            dataHistoryPage.WaitForLastValuePanelDisplayed();

            Step("9. Expected The values is updated to '1 h'");
            time = dataHistoryPage.LastValuesPanel.GetMeteringsTimeAttribute(attr);
            VerifyEqual("9. Verify The values is updated to '1 h'", "1 h", time);

            try
            {
                DeleteGeozone(geozone);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("SLV-1814 Advanced Search - Lamp type is not displayed correctly")]
        [NonParallelizable]
        public void SLV_1814()
        {
            var testData = GetTestDataOfSLV_1814();
            var xmlGeoZone = testData["GeoZone"];
            var xmlSearchNamePrefix = testData["SearchName"].ToString();
            var xmlExportFilePattern = testData["ExportFilePattern"].ToString();

            Step("**** PreconditiF1959on ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.AdvancedSearch);

            Step("1. Go to AdvancedSearch app");
            Step("2. Expected AdvancedSearch page is routed and loaded successfully");
            var advancedSearchPage = desktopPage.GoToApp(App.AdvancedSearch) as AdvancedSearchPage;
            advancedSearchPage.WaitForSearchWizardPopupPanelDisplayed();

            var geoZoneName = xmlGeoZone.GetChildName();
            var searchName = string.Format("[{0}] {1}{2}", geoZoneName, xmlSearchNamePrefix, DateTime.Now.Timestamp());

            Step("3. Configure an advanced search with Lamp Type field is checked");
            advancedSearchPage.SearchWizardPopupPanel.ClickNewAdvancedSearchButton();
            advancedSearchPage.SearchWizardPopupPanel.WaitForNewSearchNameInputVisible();
            advancedSearchPage.SearchWizardPopupPanel.EnterNewSearchNameInput(searchName);
            advancedSearchPage.SearchWizardPopupPanel.ClickNextButton();
            advancedSearchPage.SearchWizardPopupPanel.WaitForGeozoneFormDisplayed();

            advancedSearchPage.SearchWizardPopupPanel.GeozoneTreePopupPanel.SelectNode(xmlGeoZone);
            var expectedDevices = advancedSearchPage.SearchWizardPopupPanel.GeozoneTreePopupPanel.GetSelectedNodeDevicesCount();
            advancedSearchPage.SearchWizardPopupPanel.ClickNextButton();
            advancedSearchPage.SearchWizardPopupPanel.WaitForAttributeFormDisplayed();

            var checkedList = new List<string>() { "Lamp Type" };
            advancedSearchPage.SearchWizardPopupPanel.CheckAttributeList(checkedList.ToArray());
            advancedSearchPage.SearchWizardPopupPanel.ClickNextButton();
            advancedSearchPage.SearchWizardPopupPanel.WaitForFilterFormDisplayed();

            advancedSearchPage.SearchWizardPopupPanel.ClickNextButton();
            advancedSearchPage.SearchWizardPopupPanel.WaitForFinishFormDisplayed();
            advancedSearchPage.SearchWizardPopupPanel.WaitForDeviceSearchCompleted();

            advancedSearchPage.SearchWizardPopupPanel.ClickFinishButton();
            advancedSearchPage.WaitForSearchWizardPopupPanelDisappeared();
            advancedSearchPage.WaitForPreviousActionComplete();
            advancedSearchPage.GridPanel.WaitForPanelLoaded();

            Step("4. Export the search");
            SLVHelper.DeleteAllFilesByPattern(xmlExportFilePattern);
            advancedSearchPage.GridPanel.ClickGenerateCSVToolbarButton();
            advancedSearchPage.WaitForPreviousActionComplete();
            advancedSearchPage.GridPanel.ClickDownloadToolbarButton();
            SLVHelper.SaveDownloads();

            Step("-> Checking for SLV-2492: Server may randomly refuse to send generated CSV files");
            VerifyEqual("Verify The requested resource is not available.", false, advancedSearchPage.IsHttp404Existing());

            var tblGrid = advancedSearchPage.GridPanel.BuildDataTableFromGrid();
            int recordsOfPage = 200;
            if (expectedDevices > recordsOfPage)
            {
                var page = expectedDevices / recordsOfPage;
                for (int j = 0; j < page; j++)
                {
                    advancedSearchPage.GridPanel.ClickFooterPageNextButton();
                    advancedSearchPage.GridPanel.WaitForLeftFooterTextDisplayed();
                    var pageTable = advancedSearchPage.GridPanel.BuildDataTableFromGrid();
                    tblGrid.Merge(pageTable);
                }
            }

            Step("5. Expected In export CSV file, Lamp Type should be Lamp Type name instead of numeric brand id");
            var tblCSV = SLVHelper.BuildDataTableFromLastDownloadedCSV(xmlExportFilePattern);
            var tblGridFormatted = tblGrid.Copy();
            var tblCSVFormatted = tblCSV.Copy();
            VerifyEqual("Verify Lamp Type exists in search grid", true, tblGrid.Columns.Contains("Lamp Type"));
            VerifyEqual("[SC-562] Verify brandId does not exist in csv file", false, tblCSV.Columns.Contains("brandId"));

            FormatAdvancedSearchDataTables(ref tblGridFormatted, ref tblCSVFormatted);
            Step("-> Checking for SC-562: Advanced search - Lamp type is not exported properly");
            VerifyEqual("[SC-562] Verify Lamp Type should be Lamp Type name instead of numeric brand id", tblGridFormatted, tblCSVFormatted);

            //Remove search
            advancedSearchPage.WaitForSearchWizardPopupPanelDisappeared();
            advancedSearchPage.GridPanel.DeleleRequest(searchName);
        }

        [Test, DynamicRetry]
        [Description("SLV-1954 Advanced Search - It is possible to create two searches with the same name")]
        public void SLV_1954()
        {
            var testData = GetTestDataOfSLV_1954();
            var xmlGeoZone = testData["GeoZone"];
            var xmlSearchNamePrefix = testData["SearchName"].ToString();

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.AdvancedSearch);

            Step("1. Go to AdvancedSearch app");
            Step("2. Expected AdvancedSearch page is routed and loaded successfully");
            var advancedSearchPage = desktopPage.GoToApp(App.AdvancedSearch) as AdvancedSearchPage;
            advancedSearchPage.WaitForSearchWizardPopupPanelDisplayed();

            var geoZoneName = xmlGeoZone.GetChildName();
            var searchName = string.Format("{0}{1}", xmlSearchNamePrefix, DateTime.Now.Timestamp());

            Step("3. Configure a new advanced search with name \"ABC\" (free to name)");
            advancedSearchPage.SearchWizardPopupPanel.ClickNewAdvancedSearchButton();
            advancedSearchPage.SearchWizardPopupPanel.WaitForNewSearchNameInputVisible();
            advancedSearchPage.SearchWizardPopupPanel.EnterNewSearchNameInput(searchName);
            advancedSearchPage.SearchWizardPopupPanel.ClickNextButton();
            advancedSearchPage.SearchWizardPopupPanel.WaitForGeozoneFormDisplayed();

            advancedSearchPage.SearchWizardPopupPanel.GeozoneTreePopupPanel.SelectNode(xmlGeoZone);
            var expectedDevices = advancedSearchPage.SearchWizardPopupPanel.GeozoneTreePopupPanel.GetSelectedNodeDevicesCount();
            advancedSearchPage.SearchWizardPopupPanel.ClickNextButton();
            advancedSearchPage.SearchWizardPopupPanel.WaitForAttributeFormDisplayed();

            advancedSearchPage.SearchWizardPopupPanel.ClickNextButton();
            advancedSearchPage.SearchWizardPopupPanel.WaitForFilterFormDisplayed();

            advancedSearchPage.SearchWizardPopupPanel.ClickNextButton();
            advancedSearchPage.SearchWizardPopupPanel.WaitForFinishFormDisplayed();
            advancedSearchPage.SearchWizardPopupPanel.WaitForDeviceSearchCompleted();

            advancedSearchPage.SearchWizardPopupPanel.ClickFinishButton();
            advancedSearchPage.WaitForSearchWizardPopupPanelDisappeared();
            advancedSearchPage.WaitForPreviousActionComplete();
            advancedSearchPage.GridPanel.WaitForPanelLoaded();

            Step("4. Click Edit in grid toolbar");
            advancedSearchPage.GridPanel.ClickEditButton();

            Step("5. Expected My advanced searches dialog appears");
            advancedSearchPage.WaitForSearchWizardPopupPanelDisplayed();

            Step("6. Click New advanced search button");
            advancedSearchPage.SearchWizardPopupPanel.ClickNewAdvancedSearchButton();

            Step("7. Expected Search name text field displays");
            advancedSearchPage.SearchWizardPopupPanel.WaitForNewSearchNameInputVisible();

            Step("8. Enter into the text field the name at step #3");
            advancedSearchPage.SearchWizardPopupPanel.EnterNewSearchNameInput(searchName);

            Step("9. Expected");
            Step(" - A message with text \"This name already exists.\" appears at the top of the screen");
            Step(" - Next button in the dialog is not visible");
            advancedSearchPage.WaitForHeaderMessageDisplayed();
            var headerText = advancedSearchPage.GetHeaderMessage();
            VerifyEqual("Verify Message with text", "This name already exists.", headerText);
            VerifyEqual("Verify Next button is not visible", false, advancedSearchPage.SearchWizardPopupPanel.IsNextButtonVisible());
            advancedSearchPage.WaitForHeaderMessageDisappeared();

            Step("10. Enter a different name");
            var newSearchName = string.Format("{0}{1}", xmlSearchNamePrefix, DateTime.Now.Timestamp());
            advancedSearchPage.SearchWizardPopupPanel.EnterNewSearchNameInput(newSearchName);

            Step("11. Expected");
            Step(" - No message appears at the top of the screen");
            Step(" - Next button in the dialog is available");
            VerifyEqual("No message appears at the top of the screen", false, advancedSearchPage.IsHeaderMessageDisplayed());
            VerifyEqual("Verify Next button is available", true, advancedSearchPage.SearchWizardPopupPanel.IsNextButtonVisible());

            Step("12. Enter into the text field the name at step #3 again");
            advancedSearchPage.SearchWizardPopupPanel.EnterNewSearchNameInput(searchName);

            Step("13. Expected");
            Step(" - A message with text \"This name already exists.\" appears at the top of the screen");
            Step(" - Next button in the dialog is not visible");
            advancedSearchPage.WaitForHeaderMessageDisplayed();
            headerText = advancedSearchPage.GetHeaderMessage();
            VerifyEqual("Verify Message with text", "This name already exists.", headerText);
            VerifyEqual("Verify Next button is not visible", false, advancedSearchPage.SearchWizardPopupPanel.IsNextButtonVisible());
            advancedSearchPage.WaitForHeaderMessageDisappeared();

            //Remove search
            advancedSearchPage.SearchWizardPopupPanel.ClickCloseButton();
            advancedSearchPage.WaitForSearchWizardPopupPanelDisappeared();
            Wait.ForSeconds(1);
            advancedSearchPage.GridPanel.DeleleSelectedRequest();
        }

        [Test, DynamicRetry]
        [Description("SLV-1740 Equipement Inventory - Custom Report, clicking on a line is blocking the workflow")]
        public void SLV_1740()
        {
            var testData = GetTestDataOfSLV_1740();
            var geozone = testData["Geozone"];

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.EquipmentInventory);

            Step("1. Go to Equipment Inventory app");
            Step("2. Expected Equipment Inventory page is routed and loaded successfully");
            var equipmentInventoryPage = desktopPage.GoToApp(App.EquipmentInventory) as EquipmentInventoryPage;

            for (var i = 0; i < 2; i++)
            {
                Step("3. Select a geozone");
                equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(geozone);

                Step("4. Expected Geozone Details panel appears");
                Step("5. Click Custom report at the bottom of the panel");
                equipmentInventoryPage.GeozoneEditorPanel.ClickCustomReportButton();
                equipmentInventoryPage.WaitForPreviousActionComplete();

                Step("6. Expected Grid panel appears:");
                Step("• Grid title is the selected geozone's name"); // Verified in other tests with the same scenario
                Step("• Lines are all devices of the selected geozone"); // Verified in other tests with the same scenario

                Step("7. Click a line in grid");
                equipmentInventoryPage.GridPanel.WaitForGridContentAvailable();                
                var clickedDevice = equipmentInventoryPage.GridPanel.GetListOfDevices().PickRandom();
                if (i == 0)
                {
                    equipmentInventoryPage.GridPanel.ClickGridRecord(clickedDevice);
                }
                else
                {
                    equipmentInventoryPage.GridPanel.DoubleClickGridRecord(clickedDevice);
                }

                Step("8. Expected");
                Step(" • The Custom report grid panel disappears");
                Step(" • Device Details panel appears (depends on device type, Controller or Streetlight or other Details panel will appear accordingly)");
                Step(" • Device in geozone is selected");
                equipmentInventoryPage.WaitForEditorPanelDisplayed();
                VerifyEqual("Verify selected note text", clickedDevice, equipmentInventoryPage.GeozoneTreeMainPanel.GetSelectedNodeText());

                Step("9. Repeat the test, just one difference: double-click at step #7. The expectation in cases of click and double-click are equivalent");
            }
        }

        [Test, DynamicRetry]
        [Description("SLV-1959 Infinite loader or error messages after deleting a device")]
        public void SLV_1959()
        {
            var testData = GetTestDataOfSLV_1959();
            var deviceNode = (XmlNode)testData["Device"];
            var geozone = SLVHelper.GenerateUniqueName("GZNSLV1959");
            var streetlight = SLVHelper.GenerateUniqueName("STL");
            var controllerId = deviceNode.GetAttrVal("controller-id");        

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            Step("-> Create data for testing");
            DeleteGeozones("GZNSLV1959*");
            CreateNewGeozone(geozone);            
            CreateNewDevice(DeviceType.Streetlight, streetlight, controllerId, geozone);

            var listOfSwitchedApp = new List<string>() { App.EquipmentInventory, App.DataHistory, App.RealTimeControl, App.Energy, App.DeviceLifetime, App.FailureTracking };
            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(listOfSwitchedApp.ToArray());
            listOfSwitchedApp.Remove(App.EquipmentInventory);

            foreach (var app in listOfSwitchedApp)
            {
                Step("1. Go to Equipment Inventory app");
                Step("2. Expected Equipment Inventory page is routed and loaded successfully");
                var equipmentInventoryPage = desktopPage.GoToApp(App.EquipmentInventory) as EquipmentInventoryPage;

                Step("3. Select a device and delete it (create one if not existing)");
                equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(geozone + @"\" + streetlight);
                equipmentInventoryPage.WaitForDeviceEditorPanelDisplayed();
                equipmentInventoryPage.DeleteCurrentDevice();
                equipmentInventoryPage.WaitForHeaderMessageDisappeared();

                Step("4. Expected The device is deleted successfully");
                equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(geozone);
                var deviceNotFoundInParent = !equipmentInventoryPage.GeozoneTreeMainPanel.GetChildNodeNamesOfSelectedNode(NodeType.Streetlight).Contains(streetlight);
                VerifyEqual("4. Verify the streetlight is no longer found in parent geozone", true, deviceNotFoundInParent);

                Step("5. Switch to " + app + " app");
                var page = equipmentInventoryPage.AppBar.SwitchTo(app);

                Step("6. Expected No either infinite loader or error message appears");
                // Verify this by calling these 2 methods. If failed at this step, it means either infinite loader or error message appears
                page.WaitForHeaderMessageDisappeared();
                page.WaitForPreviousActionComplete();

                Step("7. Repeat the test - replacing app at step #5 by apps that invoke geozone tree (Realtime Control, Energy, Device Lifetime, Failure Tracking, etc.)");
                streetlight = SLVHelper.GenerateUniqueName("STL");
                CreateNewDevice(DeviceType.Streetlight, streetlight, controllerId, geozone);
                desktopPage = Browser.RefreshLoggedInCMS();
            }

            try
            {
                DeleteGeozone(geozone);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("SLV-1986 Failure Tracking - Enabling the geozone filter does not work right away")]
        public void SLV_1986()
        {
            Assert.Pass("Covered by 4 tests: FT_05, FT_06, FT_07, FT_08");
        }

        [Test, DynamicRetry]
        [Description("SLV-1987 Equipment Inventory - Two Identity tabs for TALQ Bridge")]
        public void SLV_1987()
        {            
            var geozone = SLVHelper.GenerateUniqueName("GZNSLV1987");
            var controller = SLVHelper.GenerateUniqueName("CTRL");

            Step("1. Go to Equipment Inventory app");
            Step("2. Expected Equipment Inventory page is routed and loaded successfully");

            Step("-> Create data for testing");
            DeleteGeozones("GZNSLV1987*");
            CreateNewGeozone(geozone);
            CreateNewController(controller, geozone);

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.EquipmentInventory);
            var equipmentInventoryPage = desktopPage.GoToApp(App.EquipmentInventory) as EquipmentInventoryPage;

            Step("3. Select a controller (create one if not existing)");
            Step("4. Expected Controller Details panel appears");
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(geozone + @"\" + controller);

            Step("5. Set Control technology field to \"TALQ Bridge\" then save");
            Step("6. Expected Saving is successful");
            equipmentInventoryPage.ControllerEditorPanel.SelectControlTechnologyDropDown("TALQ Bridge");
            equipmentInventoryPage.ControllerEditorPanel.ClickSaveButton();
            equipmentInventoryPage.WaitForPreviousActionComplete();

            Step("7. Select the controller again");
            Step("8. Expected Controller Details panel appears");
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(controller);

            Step("9. Expected Verify there is only one Identity tab instead of 2");
            VerifyEqual("9. Verify only 1 Identity tab", 1, equipmentInventoryPage.ControllerEditorPanel.GetListOfTabsName().Count(tab => tab == "Identity"));

            try
            {
                DeleteGeozone(geozone);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("SLV-1071 Alarm is shown as enabled after being disabled")]
        [NonParallelizable]
        public void SLV_1071()
        {
            var testData = GetTestDataOfSLV_1071();
            var geozone = testData["Geozone"];
            var alarmName = SLVHelper.GenerateUniqueName(string.Format("[{0}] {1}", Settings.Users["DefaultTest"].Username, Settings.CurrentTestWebDriverKeyName));

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.AlarmManager);

            Step("1. Go to Alarm Manager app");
            Step("2. Expected Alarm Manager page is routed and loaded successfully");
            var alarmManagerPage = desktopPage.GoToApp(App.AlarmManager) as AlarmManagerPage;

            Step("3. Create any alarm in any geozone with Enabled option checked");
            alarmManagerPage.GeozoneTreeMainPanel.SelectNode(geozone);
            alarmManagerPage.GridPanel.ClickAddAlarmToolbarButton();
            alarmManagerPage.WaitForPreviousActionComplete();

            /* Basic info */
            alarmManagerPage.AlarmEditorPanel.EnterNameInput(alarmName);

            /* General tab */
            alarmManagerPage.AlarmEditorPanel.TickDisabledCheckbox(false);
            alarmManagerPage.AlarmEditorPanel.ClickSaveButton();
            alarmManagerPage.WaitForPreviousActionComplete();

            Step("4. Expected The created alarm is present in grid panel");
            VerifyEqual("Verify the newly created alarm is present in the grid", true, alarmManagerPage.GridPanel.IsColumnHasTextPresent("Name", alarmName));

            Step("5. Disable the created alarm from grid panel by unchecked the option in Enabled column");
            alarmManagerPage.GridPanel.TickGridColumn(alarmName, false);
            alarmManagerPage.WaitForPanelRightDisappeared();
            alarmManagerPage.WaitForPreviousActionComplete();

            Step("6. Expected The option is unchecked in grid panel row now");
            VerifyEqual("[SC-541] 6. Verify The option is unchecked in grid panel row now", false, alarmManagerPage.GridPanel.GetCheckBoxGridColumnValue(alarmName));

            Step("7. Select another geozone then go back to the original geozone");
            alarmManagerPage.GeozoneTreeMainPanel.SelectNode(Settings.RootGeozoneName);

            Step("8. Expected The alarm should be still unchecked");
            VerifyEqual("8. Verify The alarm should be still unchecked", false, alarmManagerPage.GridPanel.GetCheckBoxGridColumnValue(alarmName));

            Step("9. Select the alarm");
            alarmManagerPage.GridPanel.ClickGridRecord(alarmName);
            alarmManagerPage.WaitForPreviousActionComplete();

            Step("10. Expected Alarm Details panel appears, Disabled option is checked");
            alarmManagerPage.AlarmEditorPanel.WaitForPanelLoaded();
            VerifyEqual("10. Verify Alarm Details Panel appears", true, alarmManagerPage.AlarmEditorPanel.IsPanelVisible());

            Step("11. Reload browser then go to Alarm Manager again");
            desktopPage = Browser.RefreshLoggedInCMS();
            alarmManagerPage = desktopPage.GoToApp(App.AlarmManager) as AlarmManagerPage;
            alarmManagerPage.GeozoneTreeMainPanel.SelectNode(geozone);

            Step("12. Expected The alarm should be still unchecked");
            VerifyEqual("Verify The alarm should be still unchecked", false, alarmManagerPage.GridPanel.GetCheckBoxGridColumnValue(alarmName));

            Step("13. Select the alarm");
            alarmManagerPage.GridPanel.ClickGridRecord(alarmName);
            alarmManagerPage.WaitForPreviousActionComplete();

            Step("14. Expected Alarm Details panel appears, Disabled option is checked");
            alarmManagerPage.AlarmEditorPanel.WaitForPanelLoaded();
            var disableValue = alarmManagerPage.AlarmEditorPanel.GetDisabledValue();
            VerifyEqual("14. Verify Disabled option is checked", true, disableValue);

            try
            {
                DeleteAlarm(alarmName);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("SLV-1681 Alarm Manager - Alarms are not exported in the correct format")]
        [NonParallelizable]
        public void SLV_1681()
        {
            var testData = GetTestDataOfSLV_1681();
            var expectedExportHeader = testData["ExpectedExportHeader"];

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.AlarmManager);

            Step("1. Go to Alarm Manager app");
            Step("2. Expected Alarm Manager page is routed and loaded successfully");
            var alarmManagerPage = desktopPage.GoToApp(App.AlarmManager) as AlarmManagerPage;

            Step("3. Click Export button from grid panel");
            SLVHelper.DeleteAllFilesByPattern(_alarmManagerExportedFilePattern);
            alarmManagerPage.GridPanel.ClickExportToolbarButton();
            SLVHelper.SaveDownloads();

            Step("4. Expected A CSV is downloaded:");
            Step(" - The CSV first line (column headers) should contain \"id;geoZoneId;triggerConditionImplClassName;alarmStateChangeActionImplClassNames;newAlarmWhenAcknowledged;alarmPriority;autoAcknowledge;refreshRate\"");
            var actualExportHeader = SLVHelper.GetHeaderLineFromDownloadedCSV(_alarmManagerExportedFilePattern);
            var expectedExportHeaderList = expectedExportHeader.Split(';').OrderBy(p => p).ToList();
            var actualExportHeaderList = actualExportHeader.Split(';').OrderBy(p => p).ToList();
            VerifyTrue("4. Verify The CSV first line (column headers) as expected", actualExportHeaderList.CheckIfIncluded(expectedExportHeaderList), string.Join(";", expectedExportHeaderList), string.Join(";", actualExportHeaderList));            
        }

        [Test, DynamicRetry]
        [Description("SLV-1378 Alarms - Not able to see an alarm on the list if the alarm is created on a very child geozone")]
        public void SLV_1378()
        {
            var alarmName = SLVHelper.GenerateUniqueName(string.Format("[{0}][{1}] {2}", Browser.Name, Settings.Users["DefaultTest"].Username, Settings.CurrentTestWebDriverKeyName));           
            var geozone = SLVHelper.GenerateUniqueName("GZNSLV1378");
            var geozone1 = SLVHelper.GenerateUniqueName("GZNSLV137801");
            var geozone2 = SLVHelper.GenerateUniqueName("GZNSLV137802");
            var geozone3 = SLVHelper.GenerateUniqueName("GZNSLV137803");
            var geozone4 = SLVHelper.GenerateUniqueName("GZNSLV137804");
            var geozone5 = SLVHelper.GenerateUniqueName("GZNSLV137805");
            var geozonePath = string.Format(@"{0}\{1}\{2}\{3}\{4}\{5}", geozone, geozone1, geozone2, geozone3, geozone4, geozone5);
            var geozones = geozonePath.SplitEx(new string[] { @"\" });
            geozones.Insert(0, Settings.RootGeozoneName);

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            Step("-> Create data for testing");
            DeleteGeozones("GZNSLV1378*");
            CreateNewGeozone(geozone);
            CreateNewGeozone(geozone1, geozone);
            CreateNewGeozone(geozone2, geozone1);
            CreateNewGeozone(geozone3, geozone2);
            CreateNewGeozone(geozone4, geozone3);
            CreateNewGeozone(geozone5, geozone4);

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.AlarmManager);

            Step("1. Go to Alarm Manager app");
            Step("2. Expected Alarm Manager page is routed and loaded successfully");
            var alarmManagerPage = desktopPage.GoToApp(App.AlarmManager) as AlarmManagerPage;

            Step("3. Create a alarm in a geozone E (GeoZones\\A\\B\\C\\D\\E)");
            alarmManagerPage.GeozoneTreeMainPanel.SelectNode(geozonePath);
            alarmManagerPage.GridPanel.ClickAddAlarmToolbarButton();
            alarmManagerPage.WaitForPreviousActionComplete();
            
            alarmManagerPage.AlarmEditorPanel.EnterNameInput(alarmName);
            alarmManagerPage.AlarmEditorPanel.ClickSaveButton();
            alarmManagerPage.WaitForPreviousActionComplete();

            Step("4. Select in turn all geozones GeoZones, A, B, C, D and E");
            foreach (var g in geozones)
            {
                alarmManagerPage.GeozoneTreeMainPanel.SelectNode(g);
                VerifyEqual(string.Format("4. Verify the newly created alarm is present in the grid of '{0}'", g), true, alarmManagerPage.GridPanel.IsAlarmManagerGridHasTextPresent("Name", alarmName));
            }

            try
            {                
                DeleteAlarm(alarmName);
                DeleteGeozone(geozone);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("SLV-402 'criticalFailureRatio' is not translated in 'Device alarm - failure ratio in a group'")]
        public void SLV_402()
        {
            var type = "Device alarm: failure ratio in a group";
            var expectedLabel = "Critical ratio";
            var alarmName = SLVHelper.GenerateUniqueName(string.Format("[{0}] {1}", Settings.Users["DefaultTest"].Username, Settings.CurrentTestWebDriverKeyName));

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.AlarmManager);

            Step("1. Go to Alarm Manager app");
            Step("2. Expected Alarm Manager page is routed and loaded successfully");
            var alarmManagerPage = desktopPage.GoToApp(App.AlarmManager) as AlarmManagerPage;

            Step("3. Click Add New Alarm on grid panel tool bar");
            alarmManagerPage.GridPanel.ClickAddAlarmToolbarButton();
            alarmManagerPage.WaitForPreviousActionComplete();

            Step("4. Expected Alarm Details panel appears");
            alarmManagerPage.AlarmEditorPanel.WaitForPanelLoaded();
            VerifyEqual("4. Verify Alarm Details Panel appears", true, alarmManagerPage.AlarmEditorPanel.IsPanelVisible());

            Step("5. Select alarm type (Type field) = \"Device alarm: failure ratio in a group\"");
            alarmManagerPage.AlarmEditorPanel.EnterNameInput(alarmName);
            alarmManagerPage.AlarmEditorPanel.SelectTypeDropDown(type);
            alarmManagerPage.WaitForPreviousActionComplete();

            Step("6. Select Trigger condition tab");
            alarmManagerPage.AlarmEditorPanel.SelectTab("Trigger condition");

            Step("7. Expected There is a label \"Critical ratio\" instead of \"criticalFailureRatio\"");
            VerifyEqual("7. Verify There is a label \"Critical ratio\"", expectedLabel, alarmManagerPage.AlarmEditorPanel.GetCriticalFailureRatioText());
        }

        [Test, DynamicRetry]
        [Description("SLV-403 The refresh rate '1 minute' appears twice in alarm definitions")]
        public void SLV_403()
        {
            var testData = GetTestDataOfSLV_403();
            var xmlRefreshRates = testData["RefreshRates"] as List<string>;
            var alarmName = SLVHelper.GenerateUniqueName(string.Format("[{0}] {1}", Settings.Users["DefaultTest"].Username, Settings.CurrentTestWebDriverKeyName));

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.AlarmManager);

            Step("1. Go to Alarm Manager app");
            Step("2. Expected Alarm Manager page is routed and loaded successfully");
            var alarmManagerPage = desktopPage.GoToApp(App.AlarmManager) as AlarmManagerPage;

            Step("3. Click Add New Alarm from grid panel toolbar");
            alarmManagerPage.GridPanel.ClickAddAlarmToolbarButton();
            alarmManagerPage.WaitForPreviousActionComplete();
            alarmManagerPage.AlarmEditorPanel.WaitForPanelLoaded();

            Step("4. Expected Alarm Details panel appears:");
            Step("In General tab, Refresh rate dropdown list should be items:");
            Step(" - Default value in the server");
            Step(" - 10 secondes");
            Step(" - 30 secondes");
            Step(" - 1 minute");
            Step(" - 5 minutes");
            Step(" - 10 minutes");
            Step(" - 30 minutes");
            Step(" - 1 hour");
            Step(" - 6 hours");
            Step(" - 12 hours");

            var allRefreshRateItems = alarmManagerPage.AlarmEditorPanel.GetRefreshRateItems();
            VerifyEqual("4. Verify All checked columns are displayed in grid", xmlRefreshRates, allRefreshRateItems, false);
        }

        [Test, DynamicRetry]
        [Description("SLV-404 'criticalCount' is not translated in 'Device alarm - single failure on multiple devices'")]
        public void SLV_404()
        {
            var type = "Device alarm: single failure on multiple devices";
            var expectedLabel = "On error devices critical count";
            var alarmName = SLVHelper.GenerateUniqueName(string.Format("[{0}] {1}", Settings.Users["DefaultTest"].Username, Settings.CurrentTestWebDriverKeyName));

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.AlarmManager);

            Step("1. Go to Alarm Manager app");
            Step("2. Expected Alarm Manager page is routed and loaded successfully");
            var alarmManagerPage = desktopPage.GoToApp(App.AlarmManager) as AlarmManagerPage;

            Step("3. Click Add New Alarm on grid panel tool bar");
            alarmManagerPage.GridPanel.ClickAddAlarmToolbarButton();
            alarmManagerPage.WaitForPreviousActionComplete();

            Step("4. Expected Alarm Details panel appears");
            alarmManagerPage.AlarmEditorPanel.WaitForPanelLoaded();
            VerifyEqual("4. Verify Alarm Details Panel appears", true, alarmManagerPage.AlarmEditorPanel.IsPanelVisible());

            Step("5. Select alarm type (Type field) = \"Device alarm: single failure on multiple devices\"");
            alarmManagerPage.AlarmEditorPanel.EnterNameInput(alarmName);
            alarmManagerPage.AlarmEditorPanel.SelectTypeDropDown(type);
            alarmManagerPage.WaitForPreviousActionComplete();

            Step("6. Select Trigger condition tab");
            alarmManagerPage.AlarmEditorPanel.SelectTab("Trigger condition");

            Step("7. There is a label \"On error devices critical count\" instead of \"criticalCount\"");
            VerifyEqual("7. Verify There is a label \"On error devices critical count\"", expectedLabel, alarmManagerPage.AlarmEditorPanel.GetCriticalCountText());
        }

        [Test, DynamicRetry]
        [Description("SLV-318 support % in alarm message")]
        public void SLV_318()
        {
            var message = "Test % message % with %";
            var alarmName = SLVHelper.GenerateUniqueName(string.Format("[{0}] {1}", Settings.Users["DefaultTest"].Username, Settings.CurrentTestWebDriverKeyName));

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.AlarmManager);

            Step("1. Go to Alarm Manager app");
            Step("2. Expected Alarm Manager page is routed and loaded successfully");
            var alarmManagerPage = desktopPage.GoToApp(App.AlarmManager) as AlarmManagerPage;

            Step("3. Create any alarm with Message field in Trigger condition tab entered with value containing '%' character");
            alarmManagerPage.GridPanel.ClickAddAlarmToolbarButton();
            alarmManagerPage.WaitForPreviousActionComplete();
            alarmManagerPage.AlarmEditorPanel.WaitForPanelLoaded();

            /* Basic info */
            alarmManagerPage.AlarmEditorPanel.EnterNameInput(alarmName);
            alarmManagerPage.WaitForPreviousActionComplete();
            /* Trigger tab */
            alarmManagerPage.AlarmEditorPanel.SelectTab("Trigger condition");
            alarmManagerPage.AlarmEditorPanel.EnterMessageInput(message);
            alarmManagerPage.AlarmEditorPanel.ClickSaveButton();
            alarmManagerPage.WaitForPreviousActionComplete();

            Step("4. Expected The alarm is created successfully: no error message, the alarm is present in grid panel");
            VerifyEqual("4. Verify The alarm is created successfully", true, alarmManagerPage.GridPanel.IsColumnHasTextPresent("Name", alarmName));

            try
            {
                DeleteAlarm(alarmName);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("SLV-1659 Alarm - The Refresh button does nothing")]
        public void SLV_1659()
        {
            var testData = GetTestDataOfSLV_1659();
            var alarm = testData["Alarm"] as XmlNode;
            var alarmName = SLVHelper.GenerateUniqueName("SLV1659");

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.Alarms, App.AlarmManager);

            var alarmManagerPage = desktopPage.GoToApp(App.AlarmManager) as AlarmManagerPage;
            alarmManagerPage.CreateControllerAlarmNoDataReceived(alarmName, alarm.GetAttrVal("controller"), alarm.GetAttrVal("refreshRate"), alarm.GetAttrVal("toEmail"));
            Wait.ForAlarmTrigger();

            Step("1. Go to Alarms app");
            Step("2. Expected Alarms page is routed and loaded successfully");
            desktopPage = Browser.RefreshLoggedInCMS();
            var alarmsPage = desktopPage.GoToApp(App.Alarms) as AlarmsPage;

            Step("3. Toggle on displaying only unacknowledged alarms");
            alarmsPage.GridPanel.ToggleShowAllAlarmsOption(true);
            VerifyEqual("3. Verify \"Show all alarms option is toggled on\"", true, alarmsPage.GridPanel.IsShowAllAlarmsOptionToggledOn());

            Step("4. Acknowledge an alarm (should be SLV-1275)");
            alarmsPage.GridPanel.ClickGridRecord(alarmName);
            alarmsPage.GridPanel.WaitForAcknowledgeButtonDisplayed();
            alarmsPage.GridPanel.ClickAcknowledgeToolbarButton();
            VerifyEqual("4. Verify Acknowlement dialog appears", true, alarmsPage.Dialog.IsDialogVisible());
            var message = string.Format("Acknowledged '{0}' on {1}", alarmName, DateTime.Now.ToUniversalTime());
            alarmsPage.Dialog.EnterAcknowledgeMessageInput(message);
            alarmsPage.Dialog.ClickSendButton();
            alarmsPage.Dialog.WaitForPopupMessageDisplayed();
            VerifyEqual(string.Format("4. Verify Popup with message '{0}' appears", alarmsPage.Dialog.GetMessageText()), "Acknowledgement sent!", alarmsPage.Dialog.GetMessageText());
            alarmsPage.Dialog.ClickOkButton();
            alarmsPage.Dialog.WaitForDialogDisappeared();

            Step("5. Click Refresh button");
            alarmsPage.GridPanel.ClickReloadDataToolbarButton();
            alarmsPage.WaitForPreviousActionComplete();

            Step("6. Expected The acknowledged alarm is no longer present in grid panel");
            VerifyEqual("6. Verify The acknowledged alarm is no longer present in grid panel", false, alarmsPage.GridPanel.IsAlarmGridHasTextPresent("Name", alarmName));

            Step("7. Wait for longer than 30 seconds, click Fresh button again");
            Wait.ForAlarmTrigger();
            alarmsPage.GridPanel.ClickReloadDataToolbarButton();
            alarmsPage.WaitForPreviousActionComplete();

            Step("8. Expected Alarm 'SLV-1275' should be present in grid panel");
            VerifyEqual(string.Format("8. Verify Alarm '{0}' should be present in grid panel", alarmName), true, alarmsPage.GridPanel.IsAlarmGridHasTextPresent("Name", alarmName));

            try
            {
                DeleteAlarm(alarmName);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("SLV-975 Sort alarms by date in the Alarms application")]
        public void SLV_975()
        {
            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.Alarms);

            Step("1. Go to Alarms app");
            Step("2. Expected Alarms page is routed and loaded successfully");
            var alarmsPage = desktopPage.GoToApp(App.Alarms) as AlarmsPage;
            if (alarmsPage.GridPanel.IsShowAllAlarmsOptionToggledOn())
            {
                alarmsPage.GridPanel.ToggleShowAllAlarmsOption(false);
                alarmsPage.GridPanel.WaitForPreviousActionComplete();
            }

            Step("3. Expected Last Change column is ascendingly sorted by default:");
            Step(" - Sort indicator is in Last Change column header with Down direction");
            Step(" - Dates in rows are sorted correctly");
            var listSortedDown = alarmsPage.GridPanel.GetListOfColumnSortedDown();
            var strDateList = alarmsPage.GridPanel.GetListOfColumnData("Last Change");
            var dateList = strDateList.ToDateList("dd/MM/yyyy HH:mm:ss");
            var sortedColumn = listSortedDown.FirstOrDefault();
            var isDecreasing = dateList.IsDecreasing();
            VerifyTrue("3. Verify Sort indicator is in Last Change column header with Down direction", listSortedDown.Count == 1 && sortedColumn.Equals("Last Change"), "Last Change", sortedColumn);
            VerifyEqual("3. Verify Dates in rows are sorted correctly", true, isDecreasing);

            Step("4. Toggle on displaying only unacknowledged alarms");
            alarmsPage.GridPanel.ToggleShowAllAlarmsOption(true);
            alarmsPage.GridPanel.WaitForPreviousActionComplete();

            Step("5. Expected Dates in rows are still sorted correctly with all rows displayed");
            strDateList = alarmsPage.GridPanel.GetListOfColumnData("Last Change");
            dateList = strDateList.ToDateList("dd/MM/yyyy HH:mm:ss");
            isDecreasing = dateList.IsDecreasing();
            VerifyEqual("5. Verify Dates in rows are sorted correctly", true, isDecreasing);
        }

        [Test, DynamicRetry]
        [Description("SLV-1000 The Refresh button resets the table sort mode in the Alarms application")]
        public void SLV_1000()
        {
            var testData = GetTestDataOfSLV_1000();
            var alarms = testData["Alarms"] as List<XmlNode>;

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.AlarmManager, App.Alarms);

            var alarmManagerPage = desktopPage.GoToApp(App.AlarmManager) as AlarmManagerPage;
            for (int i = 0; i < alarms.Count; i++)
            {
                var alarm = alarms[i];
                alarmManagerPage.CreateDeviceAlarmNoDataReceived(SLVHelper.GenerateUniqueName(string.Format("SLV1000{0}", i + 1)), alarm.GetAttrVal("device"), alarm.GetAttrVal("refreshRate"));
            }
            Wait.ForAlarmTrigger();

            Step("1. Go to Alarms app");
            Step("2. Expected Alarms page is routed and loaded successfully");
            desktopPage = Browser.RefreshLoggedInCMS();
            var alarmsPage = desktopPage.GoToApp(App.Alarms) as AlarmsPage;

            Step("3. Click Show hide columns button on grid panel's toolbar and check all columns except 'Line #' column");
            var allColumns = alarmsPage.GridPanel.GetAllColumnsInShowHideColumnsMenu();
            allColumns.Remove("Line #");
            alarmsPage.GridPanel.UncheckAllColumnsInShowHideColumnsMenu();
            alarmsPage.GridPanel.CheckColumnsInShowHideColumnsMenu(allColumns.ToArray());
            alarmsPage.AppBar.ClickHeaderBartop();

            Step("4. Expected All checked columns are displayed in grid");
            var allGridColumns = alarmsPage.GridPanel.GetListOfColumnsHeader();
            VerifyEqual("4. Verify All checked columns are displayed in grid", allColumns, allGridColumns, false);
            
            Step("5. Click header to a column");
            Step("6. Expected Rows are sorted by clicked column:");
            Step(" - Sort indicator appears next to the clicked column header");
            Step(" - Rows are sorted correctly");
            Step("7. Click Refresh button");
            Step("8. Expected The sorting in step #6 is still remained");
            Step("9. Repeat steps from 5 to 7 with remained columns");

            foreach (var column in allGridColumns)
            {
                Step(string.Format("-> Click on column '{0}'", column));
                alarmsPage.GridPanel.ClickGridColumnHeader(column);
                VerifyAlarmGridSorted(alarmsPage, column);
                Step("-> Click Refresh button");
                alarmsPage.GridPanel.ClickReloadDataToolbarButton();
                alarmsPage.WaitForPreviousActionComplete();
                VerifyAlarmGridSorted(alarmsPage, column);
            }

            try
            {
                DeleteAlarms("SLV1000*");
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("SLV-2435 Internal server error when deleting non-persisted alarm")]
        public void SLV_2435()
        {
            var type = "Device alarm: failure ratio in a group";
            var alarmName = SLVHelper.GenerateUniqueName(string.Format("[{0}] {1}", Settings.Users["DefaultTest"].Username, Settings.CurrentTestWebDriverKeyName));

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.AlarmManager);

            Step("1. Go to Alarm Manager app");
            Step("2. Expected Alarm Manager page is routed and loaded successfully");
            var alarmManagerPage = desktopPage.GoToApp(App.AlarmManager) as AlarmManagerPage;

            Step("3. Click Add New Alarm on grid panel tool bar");
            alarmManagerPage.GridPanel.ClickAddAlarmToolbarButton();
            alarmManagerPage.WaitForPreviousActionComplete();

            Step("4. Expected Alarm Details panel appears");
            alarmManagerPage.AlarmEditorPanel.WaitForPanelLoaded();
            VerifyEqual("4. Verify Alarm Details Panel appears", true, alarmManagerPage.AlarmEditorPanel.IsPanelVisible());

            Step("5. Note the auto-generated alarm name");
            var autoAlarmName = alarmManagerPage.AlarmEditorPanel.GetNameValue();

            Step("6. Change alarm name, select alarm type");
            alarmManagerPage.AlarmEditorPanel.EnterNameInput(alarmName);
            alarmManagerPage.AlarmEditorPanel.SelectTypeDropDown(type);
            alarmManagerPage.WaitForPreviousActionComplete();

            Step("7. Click Delete button");
            alarmManagerPage.AlarmEditorPanel.ClickDeleteButton();
            alarmManagerPage.Dialog.WaitForPanelLoaded();

            Step("8. Expected Confirmation dialog appears with message 'Would you like to delete '{{Alarm Name}}' alarm ?'");
            var expectedMessage = string.Format("Would you like to delete '{0}' alarm ?", autoAlarmName);
            var actualMessage = alarmManagerPage.Dialog.GetMessageText();
            VerifyEqual("8. Verify Confirmation dialog appears with message", expectedMessage, actualMessage);

            Step("9. Click Yes");
            alarmManagerPage.Dialog.ClickYesButton();
            alarmManagerPage.Dialog.WaitForDialogDisappeared();

            Step("10. Expected Because the alarm has NOT been saved, there should not be an alarm with either auto-generated or changed name in the alarm grid. No error message or extra dialog as well");
            VerifyEqual("10. Verify There is not an alarm with either auto-generated", false, alarmManagerPage.GridPanel.IsColumnHasTextPresent("Name", autoAlarmName) || alarmManagerPage.GridPanel.IsColumnHasTextPresent("Name", alarmName));
        }

        [Test, DynamicRetry]
        [Description("SLV-1989 Alarm Manager - No 10min refresh rate")]
        public void SLV_1989()
        {
            Assert.Ignore("Duplicated with SLV-403 The refresh rate '1 minute' appears twice in alarm definitions");
        }

        [Test, DynamicRetry]
        [Description("SLV-1689 Fix SunsetSunrise widget")]
        public void SLV_1689()
        {
            var testData = GetTestDataOfSLV_1689();
            var xmlGeozones = testData["Geozones"] as List<string>;

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - Sunrise Sunset Times widget has been installed successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallWidgetsIfNotExist(Widget.SunriseSunsetTimes);

            Step("1. Note data of Sunrise Sunset Times tile in Desktop page");
            Step(" - Sunrise time (hh:mm:ss)");
            Step(" - SUNRISE label");
            Step(" - Sunset time (hh:mm:ss)");
            Step(" - SUNSET label");
            Step(" - Geozone name");
            var srTime = desktopPage.GetFirstSunriseSunsetWidgetSunriseTime();
            var srLabel = desktopPage.GetFirstSunriseSunsetWidgetSunriseLabel();
            var ssTime = desktopPage.GetFirstSunriseSunsetWidgetSunsetTime();
            var ssLabel = desktopPage.GetFirstSunriseSunsetWidgetSunsetLabel();
            var geozoneName = desktopPage.GetFirstSunriseSunsetWidgetGeozoneName();

            //If SunriseSunsetWidget has no geozone, select root geozone
            if (string.IsNullOrEmpty(geozoneName))
            {
                desktopPage.OpenWidget(Widget.SunriseSunsetTimes);
                desktopPage.GeozoneTreeWidgetPanel.SelectNode(Settings.RootGeozoneName);
                desktopPage.GeozoneTreeWidgetPanel.ClickBackButton();
                desktopPage.WaitForSunriseSunsetGeozoneTreeDisappeared();

                srTime = desktopPage.GetFirstSunriseSunsetWidgetSunriseTime();
                srLabel = desktopPage.GetFirstSunriseSunsetWidgetSunriseLabel();
                ssTime = desktopPage.GetFirstSunriseSunsetWidgetSunsetTime();
                ssLabel = desktopPage.GetFirstSunriseSunsetWidgetSunsetLabel();
                geozoneName = desktopPage.GetFirstSunriseSunsetWidgetGeozoneName();
            }

            Step("2. Click Sunrise Sunset Times widget");
            desktopPage.OpenWidget(Widget.SunriseSunsetTimes);

            Step("3. Expected Geozone Tree panel appears:");
            Step(" - The panel has Back button");
            Step(" - The title of the panel is \"Sunrise Sunset Times\"");
            Step(" - A dropdown list for timezone selection");
            Step(" - Geozone tree");
            Step(" - The active geozone is the geozone name noted in step #1");
            desktopPage.WaitForSunriseSunsetGeozoneTreeDisplayed();
            var currentTimezone = desktopPage.GeozoneTreeWidgetPanel.GetTimezonesValue();
            VerifyEqual("3. Verify The panel has Back button", true, desktopPage.GeozoneTreeWidgetPanel.IsBackButtonDisplayed());
            VerifyEqual("3. Verify The title of the panel is \"Sunrise Sunset Times\"", "Sunrise Sunset Times", desktopPage.GeozoneTreeWidgetPanel.GetPanelTitleText());
            VerifyEqual("3. Verify A dropdown list for timezone selection", true, desktopPage.GeozoneTreeWidgetPanel.IsTimezonesDropDownDisplayed());
            VerifyEqual("3. Verify Geozone tree displayed", true, desktopPage.GeozoneTreeWidgetPanel.IsGeozoneTreeDisplayed());
            VerifyEqual("3. Verify The active geozone is the geozone name noted in step #1", geozoneName, desktopPage.GeozoneTreeWidgetPanel.GetSelectedNodeName());

            Step("4. Click Back button");
            desktopPage.GeozoneTreeWidgetPanel.ClickBackButton();

            Step("5. Expected Geozone Tree panel disappears");
            desktopPage.WaitForSunriseSunsetGeozoneTreeDisappeared();

            Step("6. Click Sunrise Sunset Times widget again");
            desktopPage.OpenWidget(Widget.SunriseSunsetTimes);

            Step("7. Expected Geozone Tree panel appears back");
            desktopPage.WaitForSunriseSunsetGeozoneTreeDisplayed();

            Step("8. Select a timezone other than the current value");
            desktopPage.GeozoneTreeWidgetPanel.SelectRandomTimezoneDropDown();

            Step("9. Expected Verify data of Sunrise Sunset Times tile in Desktop page");
            Step(" - Sunrise time has changed (hh:mm:ss)");
            Step(" - SUNRISE label is remained");
            Step(" - Sunset time has changed (hh:mm:ss)");
            Step(" - SUNSET label is remained");
            Step(" - Geozone name is remained");
            var newSrTime = desktopPage.GetFirstSunriseSunsetWidgetSunriseTime();
            var newSrLabel = desktopPage.GetFirstSunriseSunsetWidgetSunriseLabel();
            var newSsTime = desktopPage.GetFirstSunriseSunsetWidgetSunsetTime();
            var newSsLabel = desktopPage.GetFirstSunriseSunsetWidgetSunsetLabel();
            var newGeozoneName = desktopPage.GetFirstSunriseSunsetWidgetGeozoneName();
            VerifyTrue("9. Verify Sunrise time has changed", srTime != newSrTime, srTime, newSrTime);
            VerifyTrue("9. Verify SUNRISE label is remained", srLabel == newSrLabel, srLabel, newSrLabel);
            VerifyTrue("9. Verify Sunset time has changed", ssTime != newSsTime, ssTime, newSsTime);
            VerifyTrue("9. Verify SUNSET label is remained", ssLabel == newSsLabel, ssLabel, newSsLabel);
            VerifyTrue("9. Verify Geozone name is remained", geozoneName == newGeozoneName, geozoneName, newGeozoneName);

            Step("10. Select a geozone from geozone tree other than the current active geozone");            
            desktopPage.GeozoneTreeWidgetPanel.SelectNode(xmlGeozones.Where(p => p != geozoneName).PickRandom());

            Step("11. Expected Verify data of Sunrise Sunset Times tile in Desktop page");
            Step(" - Sunrise time has changed (hh:mm:ss)");
            Step(" - SUNRISE label is remained");
            Step(" - Sunset time has changed (hh:mm:ss)");
            Step(" - SUNSET label is remained");
            Step(" - Geozone name has changed to the selected geozone");
            newSrTime = desktopPage.GetFirstSunriseSunsetWidgetSunriseTime();
            newSrLabel = desktopPage.GetFirstSunriseSunsetWidgetSunriseLabel();
            newSsTime = desktopPage.GetFirstSunriseSunsetWidgetSunsetTime();
            newSsLabel = desktopPage.GetFirstSunriseSunsetWidgetSunsetLabel();
            newGeozoneName = desktopPage.GetFirstSunriseSunsetWidgetGeozoneName();
            VerifyTrue("11. Verify Sunrise time has changed", srTime != newSrTime, srTime, newSrTime);
            VerifyTrue("11. Verify SUNRISE label is remained", srLabel == newSrLabel, srLabel, newSrLabel);
            VerifyTrue("11. Verify Sunset time has changed", ssTime != newSsTime, ssTime, newSsTime);
            VerifyTrue("11. Verify SUNSET label is remained", ssLabel == newSsLabel, ssLabel, newSsLabel);
            VerifyTrue("11. Verify Geozone name has changed to the selected geozone", geozoneName != newGeozoneName, geozoneName, newGeozoneName);
            desktopPage.GeozoneTreeWidgetPanel.ClickBackButton();
            desktopPage.WaitForSunriseSunsetGeozoneTreeDisappeared();

            Step("12. Refresh browser");
            desktopPage = Browser.RefreshLoggedInCMS();

            Step("13. Click Sunrise Sunset Times widget");
            desktopPage.OpenWidget(Widget.SunriseSunsetTimes);

            Step("14. Expected Geozone Tree panel appears:");
            Step(" - The panel has Back button");
            Step(" - The title of the panel is \"Sunrise Sunset Times\"");
            Step(" - A dropdown list for timezone selection");
            Step(" - Geozone tree");
            Step(" - The active geozone is the geozone name of the last selected one");
            desktopPage.WaitForSunriseSunsetGeozoneTreeDisplayed();
            VerifyEqual("14. Verify The panel has Back button", true, desktopPage.GeozoneTreeWidgetPanel.IsBackButtonDisplayed());
            VerifyEqual("14. Verify The title of the panel is \"Sunrise Sunset Times\"", "Sunrise Sunset Times", desktopPage.GeozoneTreeWidgetPanel.GetPanelTitleText());
            VerifyEqual("14. Verify A dropdown list for timezone selection", true, desktopPage.GeozoneTreeWidgetPanel.IsTimezonesDropDownDisplayed());
            VerifyEqual("14. Verify Geozone tree displayed", true, desktopPage.GeozoneTreeWidgetPanel.IsGeozoneTreeDisplayed());
            VerifyEqual("14. Verify The active geozone is the geozone name of the last selected one", newGeozoneName, desktopPage.GeozoneTreeWidgetPanel.GetSelectedNodeName());            
        }

        [Test, DynamicRetry]
        [Description("SLV-2405 Data History - Clicking on the Wheel button after selecting a saved report does not allow users to immediately edit the saved report")]
        public void SLV_2405()
        {
            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.DataHistory);

            Step("1. Go to Data History app");
            Step("2. Expected Data History page is routed and loaded successfully");
            var dataHistoryPage = desktopPage.GoToApp(App.DataHistory) as DataHistoryPage;

            Step("3. Select an item from the report list in grid toolbar");
            dataHistoryPage.WaitForGridPanelDisplayed();
            var allSearches = dataHistoryPage.GridPanel.GetListOfSearchDropDownItems();
            var searchName = SLVHelper.GenerateUniqueName("SLV2405");
            var isNewSearch = false;
            if (allSearches.Any())
            {
                searchName = allSearches.PickRandom();
                dataHistoryPage.GridPanel.SelectSelectOrAddSearchDropDown(searchName);
                dataHistoryPage.WaitForPreviousActionComplete();
            }
            else
            {
                dataHistoryPage.CreateNewReport(searchName);
                isNewSearch = true;
            }
            Step("4. Expected The selected item is the current item in the list");
            var actualSearchName = dataHistoryPage.GridPanel.GetSelectOrAddSearchValue();
            VerifyEqual("4. Verify The selected item is the current item in the list", searchName, actualSearchName);

            Step("5. Click Edit button");
            dataHistoryPage.GridPanel.ClickEditButton();
            dataHistoryPage.WaitForSearchWizardPopupPanelDisplayed();

            Step("6. Expected 'My advanced searches' dialog appears. The selected report is already selected in the list of existing reports");
            var searchNameText = dataHistoryPage.SearchWizardPopupPanel.GetSearchNameValue();
            VerifyEqual("6. Verify 'My advanced searches' dialog appears", "My advanced searches", dataHistoryPage.SearchWizardPopupPanel.GetPanelTitleText());
            VerifyEqual("6. Verify The selected report is already selected in the list of existing reports", searchName, searchNameText);

            try
            {
                if (isNewSearch)
                {
                    dataHistoryPage.SearchWizardPopupPanel.ClickCloseButton();
                    dataHistoryPage.WaitForSearchWizardPopupPanelDisappeared();
                    dataHistoryPage.GridPanel.DeleleSelectedRequest();
                }
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("SLV-2375 Searching Device in Data History App is not working as expected, it requires for users to be on the root geozone always to search for devices")]
        public void SLV_2375()
        {
            var testData = GetTestDataOfSLV_2375();
            var xmlGeozone = testData["Geozone"];
            var xmlSearchName = testData["SearchName"];

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.DataHistory);

            Step("1. Go to Data History app");
            Step("2. Expected Data History page is routed and loaded successfully");
            var dataHistoryPage = desktopPage.GoToApp(App.DataHistory) as DataHistoryPage;

            Step("3. Search by name for a device which belongs to geozone A (A is not the root geozone)");
            dataHistoryPage.GeozoneTreeMainPanel.SelectNode(xmlGeozone);
            dataHistoryPage.GeozoneTreeMainPanel.ChangeSearchAttribute("Name", "Contains");
            dataHistoryPage.GeozoneTreeMainPanel.EnterSearchTextInput(xmlSearchName);
            dataHistoryPage.GeozoneTreeMainPanel.ClickSearchButton();
            dataHistoryPage.WaitForPreviousActionComplete();

            Step("4. Expected The found device is listed in found device listed");
            var devicesFoundList = dataHistoryPage.GeozoneTreeMainPanel.SearchResultsGeozonePanel.GetListOfSearchResult("Equipment");
            VerifyEqual("4. Verify The found device is listed in found device listed", true, devicesFoundList.Any() && devicesFoundList.TrueForAll(p => p.Contains(xmlSearchName)));

            Step("5. Select the found device from the found list");
            var record = dataHistoryPage.GeozoneTreeMainPanel.SearchResultsGeozonePanel.SelectFirstFoundDevice();
            dataHistoryPage.WaitForPreviousActionComplete();
            var expectedDeviceName = record[0];
            dataHistoryPage.GridPanel.WaitForGridContentAvailable();

            Step("6. Expected Last values panel appears");
            dataHistoryPage.WaitForLastValuePanelDisplayed();

            Step("7. Close Last values panel");
            dataHistoryPage.LastValuesPanel.ClickBackToolbarButton();

            Step("8. Expected Last values panel is closed. Search results panel shows up");
            dataHistoryPage.WaitForLastValuePanelDisappeared();
            VerifyEqual("8. Verify Search results panel shows up", true, dataHistoryPage.GeozoneTreeMainPanel.IsSearchResultPanelDisplayed());

            Step("9. Close Search results panel");
            dataHistoryPage.GeozoneTreeMainPanel.SearchResultsGeozonePanel.ClickBackButton();

            Step("10. Expected Search results panel is closed. Geozone tree panel shows up and is highlighting the selected device");
            dataHistoryPage.GeozoneTreeMainPanel.SearchResultsGeozonePanel.WaitForPanelClosed();
            var selectedDevice = dataHistoryPage.GeozoneTreeMainPanel.GetSelectedNodeText();
            VerifyEqual(string.Format("10. Verify Geozone tree panel shows up and is highlighting the selected device '{0}'", expectedDeviceName), expectedDeviceName, selectedDevice);

            Step("11. Search by name for a device which does NOT belongs to geozone A (not to the root geozone as well)");
            var newDevice = "DUPLICATED";
            dataHistoryPage.GeozoneTreeMainPanel.SelectNode(xmlGeozone);
            dataHistoryPage.GeozoneTreeMainPanel.ChangeSearchAttribute("Name", "Contains");
            dataHistoryPage.GeozoneTreeMainPanel.EnterSearchTextInput(newDevice);
            dataHistoryPage.GeozoneTreeMainPanel.ClickSearchButton();
            dataHistoryPage.WaitForPreviousActionComplete();

            Step("12. Expected The new found device is listed in found device listed");
            dataHistoryPage.GeozoneTreeMainPanel.SearchResultsGeozonePanel.WaitForPanelLoaded();
            devicesFoundList = dataHistoryPage.GeozoneTreeMainPanel.SearchResultsGeozonePanel.GetListOfSearchResult("Equipment");
            VerifyEqual("12. Verify The found device is listed in found device listed", true, devicesFoundList.TrueForAll(p => p.Contains(newDevice)));
        }

        [Test, DynamicRetry]
        [Description("SLV-847 Unable to select a Controller in Equipment Inventory after selecting it in Real Time Control")]
        public void SLV_847()
        {
            var testData = GetTestDataOfSLV_847();
            var geozone = testData["Geozone"].ToString();
            var xmlController = testData["Controller"] as DeviceModel;
            var xmlStreetlights = testData["Streetlights"] as List<DeviceModel>;          
            var controllerName = xmlController.Name;
            var streetlightName = xmlStreetlights.PickRandom().Name;
            var controllerPath = geozone + @"\" + controllerName;

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.RealTimeControl, App.EquipmentInventory);

            Step("1. Go to Real-time Control app");
            Step("2. Expected Real-time Control page is routed and loaded successfully");
            var realtimeControlPage = desktopPage.GoToApp(App.RealTimeControl) as RealTimeControlPage;

            Step("3. Browse to a controller in a geozone");
            realtimeControlPage.GeozoneTreeMainPanel.SelectNode(controllerPath);

            Step("4. Expected Controller Realtime panel appears");
            realtimeControlPage.WaitForControllerWidgetDisplayed(controllerName);
            realtimeControlPage.ControllerWidgetPanel.WaitForNameText(controllerName);

            Step("5. Switch to Equipment Inventory app");
            var equipmentInventoryPage = realtimeControlPage.AppBar.SwitchTo(App.EquipmentInventory) as EquipmentInventoryPage;

            Step("6. Expected Equipment Inventory page is switch to. The selected controller is being active in geozone tree and its Controller Details panel appears");
            equipmentInventoryPage.WaitForEditorPanelDisplayed();
            var actualControllerName = equipmentInventoryPage.ControllerEditorPanel.GetNameValue();
            var activeGeozoneDevice = equipmentInventoryPage.GeozoneTreeMainPanel.GetSelectedNodeText();
            VerifyEqual("6. Verify The selected controller is being active in geozone tree", controllerName, activeGeozoneDevice);
            VerifyEqual("6. Verify Controller Details panel appears", controllerName, actualControllerName);

            Step("7. Reload browser");
            desktopPage = Browser.RefreshLoggedInCMS();

            Step("8. Go to Equipment Inventory app");
            equipmentInventoryPage = desktopPage.GoToApp(App.EquipmentInventory) as EquipmentInventoryPage;

            Step("9. Expected Equipment Inventory page is routed and loaded successfully");
            equipmentInventoryPage.WaitForEditorPanelDisplayed();

            Step("10. Select the controller again");
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(controllerPath);

            Step("11. Expected Its Controller Details panel appears");
            actualControllerName = equipmentInventoryPage.ControllerEditorPanel.GetNameValue();
            VerifyEqual("11. Verify Controller Details panel appears", controllerName, actualControllerName);

            Step("12. Notes its detailed values");
            var notedLatValue = equipmentInventoryPage.ControllerEditorPanel.GetLatitudeValue();
            var notedLongValue = equipmentInventoryPage.ControllerEditorPanel.GetLongitudeValue();
            var notedControllerId = equipmentInventoryPage.ControllerEditorPanel.GetControllerIdValue();
            Step(string.Format(" -> Latitude: {0}", notedLatValue));
            Step(string.Format(" -> Longitude: {0}", notedLongValue));
            Step(string.Format(" -> ControllerId: {0}", notedControllerId));

            Step("13. Switch to Realtime Control app");
            realtimeControlPage = equipmentInventoryPage.AppBar.SwitchTo(App.RealTimeControl) as RealTimeControlPage;

            Step("14. Expected Realtime Control page is switch to. The selected controller is being active in geozone tree and its Controller Realtime panel appears");
            realtimeControlPage.WaitForControllerWidgetDisplayed(controllerName);
            actualControllerName = realtimeControlPage.ControllerWidgetPanel.GetDeviceNameText();
            activeGeozoneDevice = realtimeControlPage.GeozoneTreeMainPanel.GetSelectedNodeText();
            VerifyEqual("14. Verify The selected controller is being active in geozone tree", controllerName, activeGeozoneDevice);
            VerifyEqual("14. Verify Controller Details panel appears", controllerName, actualControllerName);

            Step("15. Select a streetlight");
            realtimeControlPage.GeozoneTreeMainPanel.SelectNode(streetlightName);

            Step("16. Expected Streetlight Realtime panel appears");
            realtimeControlPage.WaitForStreetlightWidgetDisplayed(streetlightName);
            var actualStreetlightName = realtimeControlPage.StreetlightWidgetPanel.GetDeviceNameText();
            VerifyEqual("16. Verify Streetlight Realtime panel appears", streetlightName, actualStreetlightName);

            Step("17. Switch back to Equipment Inventory app");
            equipmentInventoryPage = realtimeControlPage.AppBar.SwitchTo(App.EquipmentInventory) as EquipmentInventoryPage;

            Step("18. Expected Equipment Inventory page is switch to. The selected streetlight is being active in geozone tree and its Streetlight Details panel appears");
            equipmentInventoryPage.WaitForEditorPanelDisplayed();
            actualStreetlightName = equipmentInventoryPage.StreetlightEditorPanel.GetNameValue();
            activeGeozoneDevice = equipmentInventoryPage.GeozoneTreeMainPanel.GetSelectedNodeText();
            VerifyEqual("18. Verify The selected streetlight is being active in geozone tree", streetlightName, activeGeozoneDevice);
            VerifyEqual("18. Verify Streetlight Details panel appears", streetlightName, actualStreetlightName);

            Step("19. Select the controller");
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(controllerName);

            Step("20. Expected Equipment Inventory page is switch to. The selected controller is being active in geozone tree and its Controller Details panel appears. Detailed values of the controller of now are the same with the noted values");
            actualControllerName = equipmentInventoryPage.ControllerEditorPanel.GetNameValue();
            activeGeozoneDevice = equipmentInventoryPage.GeozoneTreeMainPanel.GetSelectedNodeText();
            VerifyEqual("20. Verify The selected controller is being active in geozone tree", controllerName, activeGeozoneDevice);
            VerifyEqual("20. Verify Controller Details panel appears", controllerName, actualControllerName);

            var actualLatValue = equipmentInventoryPage.ControllerEditorPanel.GetLatitudeValue();
            var actualLongValue = equipmentInventoryPage.ControllerEditorPanel.GetLongitudeValue();
            var actualControllerId = equipmentInventoryPage.ControllerEditorPanel.GetControllerIdValue();
            VerifyEqual(string.Format("20. Verify Latitude value '{0}' is the same with the noted values", actualLatValue), notedLatValue, actualLatValue);
            VerifyEqual(string.Format("20. Verify Longitude value '{0}' is the same with the noted values", actualLongValue), notedLongValue, actualLongValue);
            VerifyEqual(string.Format("20. Verify ControllerId '{0}' is the same with the noted values", actualControllerId), notedControllerId, actualControllerId);
        }

        [Test, DynamicRetry]
        [Description("SLV-2343 Cannot download csv file from Data History")]
        [NonParallelizable]
        public void SLV_2343()
        {
            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var reportName = SLVHelper.GenerateUniqueName(string.Format("[{0}] {1}", Settings.Users["DefaultTest"].Username, Settings.CurrentTestWebDriverKeyName));
            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.DataHistory);

            Step("1. Go to Data History app");
            Step("2. Expected Data History page is routed and loaded successfully");
            var dataHistoryPage = desktopPage.GoToApp(App.DataHistory) as DataHistoryPage;

            Step("3. Load a report (create one if not existing) then click Export button");
            dataHistoryPage.WaitForGridPanelDisplayed();
            var allSearches = dataHistoryPage.GridPanel.GetListOfSearchDropDownItems();
            var isNewReport = false;
            if (allSearches.Any())
            {
                reportName = allSearches.PickRandom();
                dataHistoryPage.GridPanel.SelectSelectOrAddSearchDropDown(reportName);
                dataHistoryPage.WaitForPreviousActionComplete();
            }
            else //Create new report
            {
                dataHistoryPage.CreateNewReport(reportName, "Real Time Control Area");
                isNewReport = true;
            }                     

            Step("4. Expected A CSV is downloaded. Its name pattern is 'Data_history*.csv'");
            SLVHelper.DeleteAllFilesByPattern(_dataHistoryGridExportedFilePattern);
            dataHistoryPage.GridPanel.ClickGenerateCSVToolbarButton();
            dataHistoryPage.WaitForPreviousActionComplete();
            dataHistoryPage.GridPanel.ClickDownloadToolbarButton();
            SLVHelper.SaveDownloads();
            VerifyEqual("4. Verify A CSV with pattern 'Data_History*.csv' is downloaded", true, SLVHelper.CheckFileExists(_dataHistoryGridExportedFilePattern));

            if (isNewReport)
            {
                dataHistoryPage.GridPanel.DeleleSelectedRequest();
            }
        }

        [Test, DynamicRetry]
        [Description("SLV-2193 Search bar is not displayed in Failure Analysis despite the manifest")]
        public void SLV_2193()
        {
            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.FailureAnalysis);

            Step("1. Go to Failure Analysis app");
            Step("2. Expected Failure Analysis page is routed and loaded successfully");
            var failureAnalysisPage = desktopPage.GoToApp(App.FailureAnalysis) as FailureAnalysisPage;

            Step("3. Expected Verify Search bar is displayed under geozone tree in geozone tree panel");
            failureAnalysisPage.WaitForGridPanelDisplayed();
            VerifyEqual("3. Verify Search bar is displayed under geozone tree in geozone tree panel", true, failureAnalysisPage.GeozoneTreeMainPanel.IsSearchBarVisible());
        }

        [Test, DynamicRetry]
        [Description("SLV-1793 Data History - The geozone name is not updated upon selecting a saved report")]
        public void SLV_1793()
        {
            var testData = GetTestDataOfSLV_1793();
            var xmlGeozoneA = testData["GeozoneA"];
            var xmlGeozoneB = testData["GeozoneB"];

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var reportNameA = SLVHelper.GenerateUniqueName(string.Format("[{0}] {1}", xmlGeozoneA, Settings.CurrentTestWebDriverKeyName));
            var reportNameB = SLVHelper.GenerateUniqueName(string.Format("[{0}] {1}", xmlGeozoneB, Settings.CurrentTestWebDriverKeyName));
            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.DataHistory);

            Step("1. Go to Data History app");
            Step("2. Expected Data History page is routed and loaded successfully");
            var dataHistoryPage = desktopPage.GoToApp(App.DataHistory) as DataHistoryPage;
            dataHistoryPage.WaitForGridPanelDisplayed();

            Step("3. Create report A with devices searched in geozone A");
            dataHistoryPage.CreateNewReport(reportNameA, xmlGeozoneA);

            Step("4. Create report B with devices searched in geozone B");
            dataHistoryPage.CreateNewReport(reportNameB, xmlGeozoneB);

            Step("5. Select A in report list");
            dataHistoryPage.GridPanel.SelectSelectOrAddSearchDropDown(reportNameA);
            dataHistoryPage.WaitForPreviousActionComplete();

            Step("6. Expected The geozone A is the selected node in geozone tree panel. Grid panel header is geozone A");
            var gridPanelText = dataHistoryPage.GridPanel.GetPanelTitleText();
            var selectedGeozone = dataHistoryPage.GeozoneTreeMainPanel.GetSelectedNodeName();
            VerifyEqual("Verify The geozone A is the selected node in geozone tree panel", xmlGeozoneA, selectedGeozone);
            VerifyEqual("Verify Grid panel header is geozone A", xmlGeozoneA, gridPanelText);

            Step("7. Select B in report list");
            dataHistoryPage.GridPanel.SelectSelectOrAddSearchDropDown(reportNameB);
            dataHistoryPage.WaitForPreviousActionComplete();

            Step("8. Expected The geozone B is the selected node in geozone tree panel. Grid panel header is geozone B");
            gridPanelText = dataHistoryPage.GridPanel.GetPanelTitleText();
            selectedGeozone = dataHistoryPage.GeozoneTreeMainPanel.GetSelectedNodeName();
            VerifyEqual("Verify The geozone B is the selected node in geozone tree panel", xmlGeozoneB, selectedGeozone);
            VerifyEqual("Verify Grid panel header is geozone B", xmlGeozoneB, gridPanelText);

            //Remove new reports created
            dataHistoryPage.GridPanel.DeleleRequest(reportNameA);
            dataHistoryPage.GridPanel.DeleleRequest(reportNameB);
        }

        [Test, DynamicRetry]
        [Description("SLV-1336 The Save button in the control program table editor should save the control program on the server")]
        public void SLV_1336()
        {
            var controlProgramName = SLVHelper.GenerateUniqueName("CP1336");
            CreateNewControlProgramAdvancedMode(controlProgramName, SLVHelper.GenerateUniqueName("Any description"));

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.SchedulingManager);

            Step("1. Go to Scheduling Manager app");
            Step("2. Expected Scheduling Manager page is routed and loaded successfully");
            var schedulingManagerPage = desktopPage.GoToApp(App.SchedulingManager) as SchedulingManagerPage;

            Step("3. Select a control program");            
            schedulingManagerPage.SchedulingManagerPanel.SelectControlProgram(controlProgramName);
            schedulingManagerPage.WaitForPreviousActionComplete();
            schedulingManagerPage.WaitForControlProgramDisplayed();
            schedulingManagerPage.ControlProgramEditorPanel.WaitForControlProgramNameDisplayed(controlProgramName);

            Step("4. Click Control Program Items icon in Control Program editor panel");
            schedulingManagerPage.ControlProgramEditorPanel.ClickControlProgramItemsButton();
            schedulingManagerPage.WaitForPopupDialogDisplayed();

            Step("5. Expected Control Program Table panel appears");
            var dialogTitle = schedulingManagerPage.Dialog.GetDialogTitleText();
            VerifyEqual("5. Verify Control Program Table panel appears", "Control program items", dialogTitle);

            Step("6. Add new item then click Save button inside Control Program Table panel [SC-567: Save button doesn't work]");
            var currentProgramItemsCount = schedulingManagerPage.ControlProgramItemsPopupPanel.GetItemsCount();
            schedulingManagerPage.ControlProgramItemsPopupPanel.ClickAddNewButton();
            var addedProgramItemsCount = schedulingManagerPage.ControlProgramItemsPopupPanel.GetItemsCount();
            VerifyEqual("6. Verify The new item is added in the table", currentProgramItemsCount + 1, addedProgramItemsCount);

            schedulingManagerPage.ControlProgramItemsPopupPanel.EnterNewItemHourInput(SLVHelper.GenerateStringInteger(23));
            schedulingManagerPage.ControlProgramItemsPopupPanel.EnterNewItemMinuteInput(SLVHelper.GenerateStringInteger(59));
            schedulingManagerPage.ControlProgramItemsPopupPanel.EnterNewItemSecondInput(SLVHelper.GenerateStringInteger(59));
            schedulingManagerPage.ControlProgramItemsPopupPanel.EnterNewItemLevelInput(SLVHelper.GenerateStringInteger(99));
            schedulingManagerPage.ControlProgramItemsPopupPanel.ClickSaveButton();
            schedulingManagerPage.WaitForPopupDialogDisappeared();
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("7. Reload browser then go to Scheduling Manager app again");
            desktopPage = Browser.RefreshLoggedInCMS();
            schedulingManagerPage = desktopPage.GoToApp(App.SchedulingManager) as SchedulingManagerPage;

            Step("8. Select the control program then click Control Program Items icon");
            schedulingManagerPage.SchedulingManagerPanel.SelectControlProgram(controlProgramName);
            schedulingManagerPage.WaitForPreviousActionComplete();
            schedulingManagerPage.WaitForControlProgramDisplayed();
            schedulingManagerPage.ControlProgramEditorPanel.WaitForControlProgramNameDisplayed(controlProgramName);

            schedulingManagerPage.ControlProgramEditorPanel.ClickControlProgramItemsButton();
            schedulingManagerPage.WaitForPopupDialogDisplayed();

            Step("9. Expected The new item is still present in the table");
            var refreshProgramItemsCount = schedulingManagerPage.ControlProgramItemsPopupPanel.GetItemsCount();
            VerifyEqual("9. Verify The new item is still present in the table", addedProgramItemsCount, refreshProgramItemsCount);

            try
            {
                //Remove new control programm
                DeleteControlProgram(controlProgramName);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("SLV-933 Unable to add absolute events using the control program table")]
        public void SLV_933()
        {
            var controlProgramName = SLVHelper.GenerateUniqueName("CP933");
            CreateNewControlProgramAdvancedMode(controlProgramName, SLVHelper.GenerateUniqueName("Any description"));

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);

            Step("1. Go to Scheduling Manager app");
            Step("2. Expected Scheduling Manager page is routed and loaded successfully");
            var schedulingManagerPage = desktopPage.GoToApp(App.SchedulingManager) as SchedulingManagerPage;

            Step("3. Select a control program");
            schedulingManagerPage.SchedulingManagerPanel.SelectTab("Control program");
            schedulingManagerPage.SchedulingManagerPanel.SelectControlProgram(controlProgramName);
            schedulingManagerPage.WaitForPreviousActionComplete();
            schedulingManagerPage.WaitForControlProgramDisplayed();
            schedulingManagerPage.ControlProgramEditorPanel.WaitForControlProgramNameDisplayed(controlProgramName);

            Step("4. Click Control Program Items icon in Control Program editor panel");
            var chartDots = schedulingManagerPage.ControlProgramEditorPanel.GetChartDotsCount();
            var itemIndex = new Random().Next(1, chartDots - 1);
            var expectedDiameter = schedulingManagerPage.ControlProgramEditorPanel.GetChartDotDiameter(itemIndex);

            schedulingManagerPage.ControlProgramEditorPanel.ClickControlProgramItemsButton();
            schedulingManagerPage.WaitForPopupDialogDisplayed();

            Step("5. Expected Control Program Table panel appears");
            var dialogTitle = schedulingManagerPage.Dialog.GetDialogTitleText();
            VerifyEqual("5. Verify Control Program Table panel appears", "Control program items", dialogTitle);

            Step("6. Change the diamond commands to circles or otherwise by double-clicking on them");
            schedulingManagerPage.ControlProgramItemsPopupPanel.DoubleClickItem(itemIndex);

            Step("7. Click on the Save button [SC-567: Save button doesn't work]");
            schedulingManagerPage.ControlProgramItemsPopupPanel.ClickSaveButton();
            schedulingManagerPage.WaitForPopupDialogDisappeared();
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("8. Expected Verify changes are reflected on graph");
            var actualDiameter = schedulingManagerPage.ControlProgramEditorPanel.GetChartDotDiameter(itemIndex);
            VerifyTrue("8. Verify changes are reflected on graph (diameter changed)", !expectedDiameter.Equals(actualDiameter), expectedDiameter, actualDiameter);

            try
            {
                //Remove new control program
                DeleteControlProgram(controlProgramName);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("SLV-2430 Monthly Energy Saving - Chrome and IE 11 exports don't work")]
        [NonParallelizable]
        public void SLV_2430()
        {
            var testData = GetTestDataOfSLV_2430();
            var xmlGeozone = testData["Geozone"];

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.MonthlyEnergySaving);

            Step("1. Go to Monthly Energy Savings app");
            Step("2. Expected Monthly Energy Saving page is routed and loaded successfully");
            var monthlyEnergySavingsPage = desktopPage.GoToApp(App.MonthlyEnergySaving) as MonthlyEnergySavingsPage;

            Step("3. Execute a saving report then click Export button");
            monthlyEnergySavingsPage.GeozoneTreeMainPanel.SelectNode(xmlGeozone);
            monthlyEnergySavingsPage.WaitForChartsCompletelyDrawn();

            var now = DateTime.Now;
            monthlyEnergySavingsPage.EnterDateFrom(now.AddYears(-1));
            monthlyEnergySavingsPage.EnterDateTo(now);
            monthlyEnergySavingsPage.ClickExecuteButton();
            monthlyEnergySavingsPage.WaitForPreviousActionComplete();

            Step("4. Expected A CSV is downloaded. Its name pattern is 'data*.csv'");
            SLVHelper.DeleteAllFilesByPattern(_monthlyEnergySavingsExportedFilePattern);
            monthlyEnergySavingsPage.ClickExportIcon();
            SLVHelper.SaveDownloads();
            VerifyEqual("[SC-1090] 4. Verify A CSV with pattern 'data*.csv' is downloaded", true, SLVHelper.CheckFileExists(_monthlyEnergySavingsExportedFilePattern));
        }

        [Test, DynamicRetry]
        [Description("SLV-964 Equipment Inventory - A newly created dimming group is not added to the list of dimming groups until the page is refreshed")]
        public void SLV_964()
        {
            var dimmingGroup1 = SLVHelper.GenerateUniqueName("CSLV96401");
            var dimmingGroup2 = SLVHelper.GenerateUniqueName("CSLV96402");
            var newDimmingGroup = SLVHelper.GenerateUniqueName("CSLV96403");
            var geozone = SLVHelper.GenerateUniqueName("GZNSLV964");
            var controller = SLVHelper.GenerateUniqueName("CTRL");
            var streetlight1 = SLVHelper.GenerateUniqueName("STL01");
            var streetlight2 = SLVHelper.GenerateUniqueName("STL02");
            var streetlight3 = SLVHelper.GenerateUniqueName("STL03");

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            Step("-> Create data for testing");
            DeleteGeozones("GZNSLV964*");
            CreateNewCalendar(dimmingGroup1);
            CreateNewCalendar(dimmingGroup2);
            CreateNewGeozone(geozone);
            CreateNewController(controller, geozone);
            CreateNewDevice(DeviceType.Streetlight, streetlight1, controller, geozone);
            CreateNewDevice(DeviceType.Streetlight, streetlight2, controller, geozone);
            CreateNewDevice(DeviceType.Streetlight, streetlight3, controller, geozone);
            SetValueToDevice(controller, streetlight1, "DimmingGroupName", dimmingGroup1, Settings.GetServerTime());
            SetValueToDevice(controller, streetlight2, "DimmingGroupName", dimmingGroup1, Settings.GetServerTime());
            SetValueToDevice(controller, streetlight3, "DimmingGroupName", dimmingGroup2, Settings.GetServerTime());

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.EquipmentInventory);

            Step("1. Go to Equipment Inventory app");
            Step("2. Expected Equipment Inventory page is routed and loaded successfully");
            var equipmentInventoryPage = desktopPage.GoToApp(App.EquipmentInventory) as EquipmentInventoryPage;

            Step("3. Select a streetlight");
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(geozone);
            var streetlights = equipmentInventoryPage.GeozoneTreeMainPanel.GetChildNodeNamesOfSelectedNode(NodeType.Streetlight);
            var rndStreetlights = streetlights.PickRandom(2);
            var sl1 = rndStreetlights[0];
            var sl2 = rndStreetlights[1];
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(sl1);

            Step("4. Expected Its Streetlight Details panel appears");
            equipmentInventoryPage.WaitForEditorPanelDisplayed();

            Step("5. Enter a new dimming group value then save");
            var curDimmingGroup = equipmentInventoryPage.StreetlightEditorPanel.GetDimmingGroupValue();
            equipmentInventoryPage.StreetlightEditorPanel.SelectDimmingGroupDropDown(newDimmingGroup);

            Step("6. Expected Saving is successful");
            equipmentInventoryPage.StreetlightEditorPanel.ClickSaveButton();
            equipmentInventoryPage.WaitForPreviousActionComplete();
            equipmentInventoryPage.WaitForEditorPanelDisappeared();

            Step("7. Select another streetlight");
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(sl2);

            Step("8. Expected Verify new dimming group entered in previous streetlight is present in dimming group dropdown of the second device");
            var allDimmingGroups = equipmentInventoryPage.StreetlightEditorPanel.GetListOfDimmingGroups();
            VerifyEqual("8. Verify new dimming group is present in dimming group dropdown of the second device", true, allDimmingGroups.Contains(newDimmingGroup));

            try
            {
                DeleteGeozone(geozone);
                DeleteCalendar(dimmingGroup1);
                DeleteCalendar(dimmingGroup2);
                DeleteCalendar(newDimmingGroup);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("SLV-965 The list of dimming groups is not refreshed in Equipment Inventory after a deletion-addition in the Scheduling Manager")]
        public void SLV_965()
        {
            var testData = GetTestDataOfSLV_965();
            var geozone = testData["Geozone"];
            var calendarName = SLVHelper.GenerateUniqueName(Settings.CurrentTestWebDriverKeyName);

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.SchedulingManager, App.EquipmentInventory);

            Step("1. Go to Scheduling Manager app");
            Step("2. Expected Scheduling Manager page is routed and loaded successfully");
            var schedulingManagerPage = desktopPage.GoToApp(App.SchedulingManager) as SchedulingManagerPage;

            Step("3. Create a new calendar");
            Step("4. Expected The new calendar is present in Scheduling Manager grid");
            schedulingManagerPage.SchedulingManagerPanel.SelectTab("Calendar");
            schedulingManagerPage.WaitForPreviousActionComplete();
            schedulingManagerPage.SchedulingManagerPanel.ClickAddCalendarButton();
            schedulingManagerPage.CalendarEditorPanel.EnterNameInput(calendarName);
            schedulingManagerPage.CalendarEditorPanel.ClickSaveButton();
            schedulingManagerPage.WaitForPreviousActionComplete();
            var calendarList = schedulingManagerPage.SchedulingManagerPanel.GetListOfCalendarName();
            VerifyEqual("4. Verify The new calendar is present in Scheduling Manager grid", true, calendarList.Contains(calendarName));

            Step("5. Switch to Equipment Inventory app");
            Step("6. Expected Equipment Inventory page is routed and loaded successfully");
            var equipmentInventoryPage = schedulingManagerPage.AppBar.SwitchTo(App.EquipmentInventory) as EquipmentInventoryPage;

            Step("7. Select a streetlight");
            Step("8. Expected Streetlight Details panel appears");
            Step("9. Verify name of the newly-created calendar is present in dimming group dropdown list");
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(geozone);
            var pickedDevice = equipmentInventoryPage.GeozoneTreeMainPanel.GetChildNodeNamesOfSelectedNode(NodeType.Streetlight).PickRandom();
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(pickedDevice);
            equipmentInventoryPage.WaitForEditorPanelDisplayed();
            VerifyEqual("9. Verify Name of the newly-created calendar is present in dimming group dropdown list (SLV-965)", true, equipmentInventoryPage.StreetlightEditorPanel.GetListOfDimmingGroups().Contains(calendarName));

            Step("10. Switch back to Scheduling Manager");
            schedulingManagerPage = equipmentInventoryPage.AppBar.SwitchTo(App.SchedulingManager) as SchedulingManagerPage;

            Step("11. Delete a calendar");
            Step("12. Expected The new calendar is no longer present in Scheduling Manager grid");
            schedulingManagerPage.SchedulingManagerPanel.SelectTab("Calendar");
            schedulingManagerPage.WaitForPreviousActionComplete();
            schedulingManagerPage.DeleteCalendar(calendarName);
            calendarList = schedulingManagerPage.SchedulingManagerPanel.GetListOfCalendarName();
            VerifyEqual("12. Verify The new calendar is no longer present in Scheduling Manager grid", false, calendarList.Contains(calendarName));
            
            Step("13. Switch to Equipment Inventory app");
            Step("14. Expected Equipment Inventory page is routed and loaded successfully");
            Step("15. Select a streetlight");
            Step("16. Expected Streetlight Details panel appears");
            Step("17. Verify name of the newly-deleted calendar is no longer present in dimming group dropdown list");
            equipmentInventoryPage = schedulingManagerPage.AppBar.SwitchTo(App.EquipmentInventory) as EquipmentInventoryPage;
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(geozone);
            pickedDevice = equipmentInventoryPage.GeozoneTreeMainPanel.GetChildNodeNamesOfSelectedNode(NodeType.Streetlight).PickRandom();
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(pickedDevice);
            equipmentInventoryPage.WaitForEditorPanelDisplayed();
            var listOfDimmingGroups = equipmentInventoryPage.StreetlightEditorPanel.GetListOfDimmingGroups();
            VerifyEqual("[SC-542] 17. Verify Name of the newly-deleted calendar is no longer present in dimming group dropdown list", false, listOfDimmingGroups.Contains(calendarName));
        }

        [Test, DynamicRetry]
        [Description("SLV-2140 A newly created dimming group in Equipment Inventory does not appear in the Scheduling Manager")]
        public void SLV_2140()
        {
            var geozone = SLVHelper.GenerateUniqueName("GZNSLV2140");
            var controller = SLVHelper.GenerateUniqueName("CTRL");
            var newDimmingGroup = SLVHelper.GenerateUniqueName("CSLV2140");

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            Step("-> Create data for testing");
            DeleteGeozones("GZNSLV2140*");
            var eventTime = Settings.GetServerTime();
            CreateNewGeozone(geozone);
            CreateNewController(controller, geozone);
            CreateNewDevice(DeviceType.Streetlight, SLVHelper.GenerateUniqueName("STL01"), controller, geozone);
            CreateNewDevice(DeviceType.Streetlight, SLVHelper.GenerateUniqueName("STL02"), controller, geozone);

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.SchedulingManager, App.EquipmentInventory);

            Step("1. Go to Scheduling Manager app");
            Step("2. Expected Scheduling Manager page is routed and loaded successfully");
            var schedulingManagerPage = desktopPage.GoToApp(App.SchedulingManager) as SchedulingManagerPage;

            Step("3. Select Calendar tab");
            schedulingManagerPage.SchedulingManagerPanel.SelectTab("Calendar");
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("4. Switch to Equipment Inventory");
            Step("5. Expected Equipment Inventory page is routed and loaded successfully");
            var equipmentInventoryPage = schedulingManagerPage.AppBar.SwitchTo(App.EquipmentInventory) as EquipmentInventoryPage;

            Step("6. Browse to and select a streetlight");
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(geozone);
            var streetlights = equipmentInventoryPage.GeozoneTreeMainPanel.GetChildNodeNamesOfSelectedNode(NodeType.Streetlight);
            var streetlight = streetlights.PickRandom();
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(streetlight);

            Step("7. Expected Streetlight Details panel appears");
            equipmentInventoryPage.WaitForEditorPanelDisplayed();

            Step("8. Enter a new (inexisting) value into dimming group field then save the streetlight");
            var curDimmingGroup = equipmentInventoryPage.StreetlightEditorPanel.GetDimmingGroupValue();
            equipmentInventoryPage.StreetlightEditorPanel.SelectDimmingGroupDropDown(newDimmingGroup);

            Step("9. Expected Saving is a success");
            equipmentInventoryPage.StreetlightEditorPanel.ClickSaveButton();
            equipmentInventoryPage.WaitForPreviousActionComplete();
            equipmentInventoryPage.WaitForEditorPanelDisappeared();

            Step("10. Switch back to Scheduling Manager and select Calendar tab");
            schedulingManagerPage = equipmentInventoryPage.AppBar.SwitchTo(App.SchedulingManager) as SchedulingManagerPage;
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("11. Expected The newly-entered dimming group is present in Dimming group grid (SC-602)");
            var dimmingGroupList = schedulingManagerPage.SchedulingManagerPanel.GetListOfCalendarName();
            var isNewDimmingGroupExisting = dimmingGroupList.Contains(newDimmingGroup);
            VerifyTrue("[SC-602] 11. Verify The newly-entered dimming group is present in Dimming group grid", isNewDimmingGroupExisting, string.Format("{0} exists", newDimmingGroup), string.Format("{0} does not exist", newDimmingGroup));

            Step("--> SC-616: With the latest version of CMS, when we comeback to Equipment Inventory page from Scheduling manager the page is struck.");
            equipmentInventoryPage = schedulingManagerPage.AppBar.SwitchTo(App.EquipmentInventory) as EquipmentInventoryPage;
            equipmentInventoryPage.WaitForPreviousActionComplete();
            equipmentInventoryPage.StreetlightEditorPanel.SelectDimmingGroupDropDown(curDimmingGroup);
            equipmentInventoryPage.StreetlightEditorPanel.ClickSaveButton();
            equipmentInventoryPage.WaitForPreviousActionComplete();
            equipmentInventoryPage.WaitForEditorPanelDisappeared();

            try
            {
                DeleteGeozone(geozone);
                DeleteCalendar(newDimmingGroup);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("SLV-2141 The list of geozones is not updated in the Users app after adding a new geozone")]
        public void SLV_2141()
        {
            var testData = GetTestDataOfSLV_2141();
            var xmlParentGeozone = testData["ParentGeozone"];
            var newGeozoneName = SLVHelper.GenerateUniqueName("GZNSLV2141");

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.EquipmentInventory);

            Step("1. Go to Equipment Inventory app");
            Step("2. Expected Equipment Inventory page is routed and loaded successfully");
            var equipmentInventoryPage = desktopPage.GoToApp(App.EquipmentInventory) as EquipmentInventoryPage;

            Step("3. Create a new geozone");
            equipmentInventoryPage.CreateGeozone(newGeozoneName, xmlParentGeozone, ZoomGLLevel.km1);

            Step("4. Expected Verify newly-created geozone is present in geozone tree");
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(xmlParentGeozone);

            var geozones = equipmentInventoryPage.GeozoneTreeMainPanel.GetChildNodeNamesOfSelectedNode(NodeType.GeoZone);
            VerifyEqual(string.Format("4. Verify newly-created geozone '{0}' is present in geozone tree", newGeozoneName), true, geozones.Contains(newGeozoneName));

            Step("5. Switch to Users app");
            Step("6. Expected Users page is routed and loaded successfully");
            var usersPage = equipmentInventoryPage.AppBar.SwitchTo(App.Users) as UsersPage;

            Step("7. Select a profile");
            usersPage.UserProfileListPanel.SelectProfile(Settings.Users["DefaultTest"].Profile);

            Step("8. Enter into Geozone field 2 start letters of name of the created geozone");
            var twoStartLetters = newGeozoneName.Substring(0, 2);
            usersPage.UserProfileDetailsPanel.EnterGeozoneInput(twoStartLetters);
            var allAutoCompleteText = usersPage.UserProfileDetailsPanel.GetAllAutoCompleteGeozoneItems();

            Step("9. Expected Geozone list appears and contains name of the created geozone");
            VerifyEqual(string.Format("9. Verify Geozone list appears and contains name of the created geozone '{0}'", newGeozoneName), true, allAutoCompleteText.Exists(p => p.Contains(newGeozoneName)));
            
            try
            {
                Info(string.Format("Delete new geozone '{0}'", newGeozoneName));
                DeleteGeozone(newGeozoneName);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("SLV-1970 Device Lifetime - Wrong device lifetime intervals displayed")]
        public void SLV_1970()
        {
            Assert.Pass("Covered by TS 5.8.1");
        }

        [Test, DynamicRetry]
        [Description("SLV-1952 Alarm Manager - Alarms are not exported properly")]
        public void SLV_1952()
        {
            Assert.Pass("Covered by SLV-1681");
        }

        [Test, DynamicRetry]
        [Description("SLV-1664 Equipment Inventory - A newly created geozone is not listed in the Parent list")]
        public void SLV_1664()
        {
            var testData = GetTestDataOfSLV_1664();
            var xmlParentGeozone = testData["ParentGeozone"];
            var xmlOtherGeozone = testData["OtherGeozone"];
            var newGeozoneName = SLVHelper.GenerateUniqueName("GZNSLV1664");

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.EquipmentInventory);

            Step("1. Go to Equipment Inventory app");
            Step("2. Expected Equipment Inventory page is routed and loaded successfully");
            var equipmentInventoryPage = desktopPage.GoToApp(App.EquipmentInventory) as EquipmentInventoryPage;

            Step("3. Create a new geozone");
            equipmentInventoryPage.CreateGeozone(newGeozoneName, xmlParentGeozone, ZoomGLLevel.km1);

            Step("4. Select another geozone then select the parent geozone of the newly-created geozone");
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(xmlOtherGeozone);
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(xmlParentGeozone);

            Step("5. Expected Verify the newly-created geozone is present in the list of its parent");
            var geozones = equipmentInventoryPage.GeozoneTreeMainPanel.GetChildNodeNamesOfSelectedNode(NodeType.GeoZone);
            VerifyEqual(string.Format("5. Verify newly-created geozone '{0}' is present in the list of its parent", newGeozoneName), true, geozones.Contains(newGeozoneName));           

            try
            {
                Info(string.Format("Delete new geozone '{0}'", newGeozoneName));
                DeleteGeozone(newGeozoneName);
            }
            catch { }
        }        

        [Test, DynamicRetry]
        [Description("SLV-854 The geozone tree displays 'no device' for all geozones after importing a CSV file")]
        [NonParallelizable]
        [Category("RunAlone")]
        public void SLV_854()
        {
            var testData = GetTestDataOfSLV_854();
            var controllerId = testData["ControllerId"];
            var geozone = SLVHelper.GenerateUniqueName("GZNSLV854");
            var fullGeozonePath = Settings.RootGeozoneName + @"/" + geozone;
            var fullPathOfImportedFileName = Settings.GetFullPath(Settings.CSV_FILE_PATH + "SLV854.csv");
            int importedDeviceCount = 5;
            var namePrefix = "SLV854";

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            Step("-> Create data for testing");
            DeleteGeozones("GZNSLV854*");
            CreateNewGeozone(geozone);
            for (int i = 1; i <= importedDeviceCount; i++)
            {
                CreateNewDevice(DeviceType.Streetlight, string.Format("{0}{1:D2}", namePrefix, i), controllerId, geozone);
            }
            CreateCsvDevices(importedDeviceCount, DeviceType.Streetlight, fullPathOfImportedFileName, fullGeozonePath, controllerId, namePrefix);

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.EquipmentInventory);

            Step("1. Go to Equipment Inventory app");
            Step("2. Expected Equipment Inventory page is routed and loaded successfully");
            var equipmentInventoryPage = desktopPage.GoToApp(App.EquipmentInventory) as EquipmentInventoryPage;

            Step("3. Note number of devices of testing geozone");
            var dicDevicesOfGeozones = equipmentInventoryPage.GeozoneTreeMainPanel.GetDevicesCountTextOfGeozones();
            var notedDevices = dicDevicesOfGeozones[geozone];

            Step("4. Import from SLV-854.csv file (it will update data for streetlights in testing geozone)");
            equipmentInventoryPage.Import(fullPathOfImportedFileName);
            equipmentInventoryPage.GeozoneEditorPanel.ImportPanel.ClickBackButton();
            equipmentInventoryPage.GeozoneEditorPanel.WaitForImportPanelDisappeared();
            equipmentInventoryPage.WaitForPreviousActionComplete();

            Step("5. Number of devices of noted geozones  is not \"no device\", it is the same with step #3");
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(geozone);
            var actualDevices = equipmentInventoryPage.GeozoneTreeMainPanel.GetSelectedNodeDevicesCountText();
            
            VerifyEqual("5. Verify Number of devices of noted geozones is not \"no device\"", true, !actualDevices.Equals("no device"));
            VerifyEqual("5. Verify it is the same with step #3", notedDevices, actualDevices);

            try
            {
                DeleteGeozone(geozone);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("SLV-733 Device Widget hidden behind the Custom Report Table")]
        public void SLV_733()
        {
            var testData = GetTestDataOfSLV_733();
            var xmlGeozone = testData["Geozone"];

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.EquipmentInventory);

            Step("1. Go to Equipment Inventory app");
            Step("2. Expected Equipment Inventory page is routed and loaded successfully");
            var equipmentInventoryPage = desktopPage.GoToApp(App.EquipmentInventory) as EquipmentInventoryPage;

            Step("3. Select a geozone");
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(xmlGeozone);

            Step("4. Expected Geozone Details panel appears");
            equipmentInventoryPage.WaitForEditorPanelDisplayed();
            VerifyEqual("4. Verify Geozone Details panel appears", true, equipmentInventoryPage.GeozoneEditorPanel.IsPanelVisible());

            Step("5. Click Custom Report at the bottom of the panel");
            equipmentInventoryPage.GeozoneEditorPanel.ClickCustomReportButton();
            equipmentInventoryPage.WaitForPreviousActionComplete();

            Step("6. Expected Geozone Details panel disappears. Custom Report Grid panel appears");
            equipmentInventoryPage.WaitForEditorPanelDisappeared();
            equipmentInventoryPage.WaitForCustomReportDisplayed();
            VerifyEqual("6. Verify Geozone Details panel disappears", false, equipmentInventoryPage.GeozoneEditorPanel.IsPanelVisible());
            VerifyEqual("6. Verify Custom Report Grid panel appears", true, equipmentInventoryPage.GridPanel.IsPanelVisible());

            Step("7. Search for device from Search bar under geozone tree in Geozone Tree panel");
            equipmentInventoryPage.GeozoneTreeMainPanel.ChangeSearchAttribute("Name", "Contains");
            equipmentInventoryPage.GeozoneTreeMainPanel.EnterSearchTextInput("Telematics");
            equipmentInventoryPage.GeozoneTreeMainPanel.ClickSearchButton();
            equipmentInventoryPage.WaitForPreviousActionComplete();

            Step("8. Select a found device from Search results panel");
            equipmentInventoryPage.GeozoneTreeMainPanel.SearchResultsGeozonePanel.SelectRandomFoundDevice();
            equipmentInventoryPage.WaitForPreviousActionComplete();

            Step("9. Expected Grid panel disappears. Device Details panel appears");
            equipmentInventoryPage.WaitForEditorPanelDisplayed();
            equipmentInventoryPage.WaitForCustomReportDisappeared();
            VerifyEqual("9. Verify Grid panel disappears", false, equipmentInventoryPage.IsCustomReportPanelDisplayed());
            VerifyEqual("9. Verify Device Details panel appears", true, equipmentInventoryPage.IsDeviceEditorPanelDisplayed());
        }

        [Test, DynamicRetry]
        [Description("SLV-356 The 'Message' input field used to enter email messages in alarm definitions is too small")]
        public void SLV_356()
        {
            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.AlarmManager);

            Step("1. Go to Alarm Manager app");
            Step("2. Expected Alarm Manager page is routed and loaded successfully");
            var alarmManagerPage = desktopPage.GoToApp(App.AlarmManager) as AlarmManagerPage;

            Step("3. Click Add New Alarm button");
            Step("4. Expected Alarm Details panel appears");
            alarmManagerPage.GridPanel.ClickAddAlarmToolbarButton();
            alarmManagerPage.WaitForPreviousActionComplete();

            Step("5. Select Actions tab");
            alarmManagerPage.AlarmEditorPanel.SelectTab("Actions");

            Step("6. Expected Height of input container of the tab is >= 270px and height of message field >= 140px");
            var containerHeight = alarmManagerPage.AlarmEditorPanel.GetEmailContainerHeight();
            var messageHeight = alarmManagerPage.AlarmEditorPanel.GetEmailMessageHeight();
            VerifyTrue("6. Verify height of input container of the tab is >= 270px", containerHeight >= 270, ">= 270", containerHeight);
            VerifyTrue("6. Verify height of message field >= 140px", messageHeight >= 140, ">= 140", containerHeight);
        }

        [Test, DynamicRetry]
        [Description("SLV-507 The result popup displayed after a Replace Lamp operation is not displayed properly")]
        public void SLV_507()
        {
            var geozone = SLVHelper.GenerateUniqueName("GZNSLV507");
            var controller = SLVHelper.GenerateUniqueName("CTRL");
            var streetlight = SLVHelper.GenerateUniqueName("STL");

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            Step("-> Create data for testing");
            DeleteGeozones("GZNSLV507*");
            CreateNewGeozone(geozone);
            CreateNewController(controller, geozone);
            CreateNewDevice(DeviceType.Streetlight, streetlight, controller, geozone);

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.EquipmentInventory);

            Step("1. Go to Equipment Inventory app");
            Step("2. Expected Equipment Inventory page is routed and loaded successfully");
            var equipmentInventoryPage = desktopPage.GoToApp(App.EquipmentInventory) as EquipmentInventoryPage;

            Step("3. Select a streetlight");
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(geozone + @"\" + streetlight);

            Step("4. Expected Streetlight Details panel appears");
            equipmentInventoryPage.WaitForEditorPanelDisplayed();
            VerifyEqual("4. Verify Streetlight Details panel appears", true, equipmentInventoryPage.StreetlightEditorPanel.IsPanelVisible());

            Step("5. Click Replace Lamp button");
            equipmentInventoryPage.StreetlightEditorPanel.ClickReplaceLampButton();
            equipmentInventoryPage.WaitForPopupDialogDisplayed();

            Step("6. Expected A confirmation dialog appears with caption 'Confirmation' and message 'Would you like to replace the lamp of '{{Device Name}}' ?'");
            var expectedMessage = string.Format("Would you like to replace the lamp of '{0}' ?", streetlight);
            VerifyEqual("6. Verify A confirmation dialog appears with caption 'Confirmation'", expectedMessage, equipmentInventoryPage.Dialog.GetMessageText());

            Step("7. Click Yes");
            equipmentInventoryPage.Dialog.ClickYesButton();
            equipmentInventoryPage.WaitForPreviousActionComplete();

            Step("8. Expected A dialog appears with message 'The lamp '{{Device Name}}' is successfully replaced.' displayed fully");
            expectedMessage = string.Format("The lamp '{0}' is successfully replaced.", streetlight);
            VerifyEqual(string.Format("8. Verify A dialog appears with message '{0}' displayed fully", expectedMessage), expectedMessage, equipmentInventoryPage.Dialog.GetMessageText());

            try
            {
                DeleteGeozone(geozone);
            }
            catch { }
        }       

        [Test, DynamicRetry]
        [Description("SLV-1932 Keep the search of devices in the context of application (SLV-3692 - Ignored due to not a bug but improment)")]
        public void SLV_1932()
        {
            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.EquipmentInventory, App.RealTimeControl, App.DataHistory, App.FailureTracking);

            Step("1. Go to Equipment Inventory app");
            Step("2. Expected Equipment Inventory page is routed and loaded successfully");
            var equipmentInventoryPage = desktopPage.GoToApp(App.EquipmentInventory) as EquipmentInventoryPage;

            Step("3. Perform a search in geozone tree");
            Step("4. Expected Only found devices are displayed in search results grid");
            equipmentInventoryPage.GeozoneTreeMainPanel.ChangeSearchAttribute("Name", "Contains");
            var searchText = "LP";
            equipmentInventoryPage.GeozoneTreeMainPanel.EnterSearchTextInput(searchText);
            equipmentInventoryPage.GeozoneTreeMainPanel.ClickSearchButton();
            equipmentInventoryPage.WaitForPreviousActionComplete();
            var searchResult = equipmentInventoryPage.GeozoneTreeMainPanel.SearchResultsGeozonePanel.GetListOfSearchResult("Equipment");
            VerifyEqual("4. Verify Only found devices are displayed in search results grid", searchResult, searchResult.FindAll(s => s.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) != -1));

            Step("5. Switch in turn to RealTimeControl, DataHistory and Failure Tracking");
            Step("6. Expected The switched app is switch to successfully. Search result from the previous app is remained displayed with the same search results");
            var realtimeControlPage = equipmentInventoryPage.AppBar.SwitchTo(App.RealTimeControl) as RealTimeControlPage;
            VerifyEqual("[SC-561][Real-time Control] 6. Verify Search results panel still displays", true, realtimeControlPage.GeozoneTreeMainPanel.IsSearchResultPanelDisplayed());
            var isSearchResultsDisplayed = realtimeControlPage.GeozoneTreeMainPanel.IsSearchResultPanelDisplayed();
            VerifyEqual("6. Verify Search result from the previous app is remained displayed", true, isSearchResultsDisplayed);
            if (isSearchResultsDisplayed)
            {
                var switchedSearchResult = realtimeControlPage.GeozoneTreeMainPanel.SearchResultsGeozonePanel.GetListOfSearchResult("Equipment");
                VerifyEqual("6. Verify The same search results", searchResult, switchedSearchResult);
            }

            var dataHistoryPage = realtimeControlPage.AppBar.SwitchTo(App.DataHistory) as DataHistoryPage;
            isSearchResultsDisplayed = realtimeControlPage.GeozoneTreeMainPanel.IsSearchResultPanelDisplayed();
            VerifyEqual("[Data History] 6. Verify Search result from the previous app is remained displayed", true, isSearchResultsDisplayed);
            if (isSearchResultsDisplayed)
            {
                var switchedSearchResult = dataHistoryPage.GeozoneTreeMainPanel.SearchResultsGeozonePanel.GetListOfSearchResult("Equipment");
                VerifyEqual("6. Verify The same search results", searchResult, switchedSearchResult);
            }

            var failureTrackingPage = dataHistoryPage.AppBar.SwitchTo(App.FailureTracking) as FailureTrackingPage;
            isSearchResultsDisplayed = realtimeControlPage.GeozoneTreeMainPanel.IsSearchResultPanelDisplayed();
            VerifyEqual("[Failure Tracking] 6. Verify Search result from the previous app is remained displayed", true, isSearchResultsDisplayed);
            if (isSearchResultsDisplayed)
            {
                var switchedSearchResult = failureTrackingPage.GeozoneTreeMainPanel.SearchResultsGeozonePanel.GetListOfSearchResult("Equipment");
                VerifyEqual("6. Verify The same search results", searchResult, switchedSearchResult);
            }
        }

        [Test, DynamicRetry]
        [Description("SLV-1606 Display the current search conditions in Advanced Search")]
        public void SLV_1606()
        {
            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);

            Step("1. Go to Advanced Search app");
            Step("2. Expected Advanced Search page is routed and loaded successfully");
            desktopPage.InstallAppsIfNotExist(App.AdvancedSearch);
            var advancedSearchPage = desktopPage.GoToApp(App.AdvancedSearch) as AdvancedSearchPage;
            advancedSearchPage.WaitForSearchWizardPopupPanelDisplayed();

            Step("3. Click Search icon button");
            advancedSearchPage.WaitForSearchWizardPopupPanelDisplayed();
            advancedSearchPage.SearchWizardPopupPanel.ClickCloseButton();
            advancedSearchPage.WaitForSearchWizardPopupPanelDisappeared();
            advancedSearchPage.GridPanel.ClickSearchToolbarButton();
            advancedSearchPage.GridPanel.WaitForAdvancedSearchPanelDisplayed();

            Step(@"4. **Expected** Advanced search popup appears
                      - Verify there is no section 'Current search criteria' and 'search conditions' displayed");
            VerifyEqual("4. Verify there is no section 'Current search criteria' and 'search conditions' displayed", false, advancedSearchPage.GridPanel.IsSearchCriteriaSectionDisplayed());

            // Temporarily enter device name only
            Step("5. Enter all edit fields then hit Manifier icon at the bottom");
            Step("6. **Expected** The advanced search panel disappears");
            var searchValue = "STL";
            advancedSearchPage.GridPanel.EnterSearchCriteriaFor1stValueInputField("Device", searchValue);
            advancedSearchPage.GridPanel.ClickAdvancedSearchSearchButton();
            VerifyEqual("6. Verify Advanced search popup appears", false, advancedSearchPage.GridPanel.IsAdvancedSearchPopupDisplayed());

            Step("7. Click Search (Manifier icon) button again");
            advancedSearchPage.GridPanel.WaitForSearchToolbarButtonEnabled();
            advancedSearchPage.GridPanel.ClickSearchToolbarButton();
            advancedSearchPage.GridPanel.WaitForAdvancedSearchPanelDisplayed();

            Step(@"8. **Expected** Section 'Current search criteria' and 'search conditions' is displayed: search condition text looks like '{ Field name 1} {operator 1} { value 1},...,{ Field name n} {operator n} { value n}'");
            VerifyEqual("8. Verify Section 'Current search criteria' and 'search conditions' is displayed", true, advancedSearchPage.GridPanel.IsSearchCriteriaSectionDisplayed());
            VerifyEqual("8. Verify Search criteria text", string.Format("Current search criteria:Device contains {0}", searchValue), advancedSearchPage.GridPanel.GetSearchCriteria());

            Step("9. Click Reset icon in advanced search panel");
            Step("10. **Expected** The advanced search panel disappears");
            advancedSearchPage.GridPanel.ClickAdvancedSearchResetButton();
            VerifyEqual("10. Verify Advanced search popup appears", false, advancedSearchPage.GridPanel.IsAdvancedSearchPopupDisplayed());

            Step("11. Click Search (Manifier icon) button again");
            Step("12. **Expected** Advanced search panel appears. There is no section 'Current search criteria' and 'search conditions' displayed");
            advancedSearchPage.GridPanel.WaitForSearchToolbarButtonEnabled();
            advancedSearchPage.GridPanel.ClickSearchToolbarButton();
            advancedSearchPage.GridPanel.WaitForAdvancedSearchPanelDisplayed();
            VerifyEqual("12. Verify there is no section 'Current search criteria' and 'search conditions' displayed", false, advancedSearchPage.GridPanel.IsSearchCriteriaSectionDisplayed());
        }

        [Test, DynamicRetry]
        [Description("SLV-2344 Failure Tracking - only display logvalues")]
        public void SLV_2344()
        {
            var testData = GetTestDataOfSLV_2344();
            var deviceName = testData["Device.name"];
            var deviceId = testData["Device.id"];
            var controllerId = testData["Device.controller-id"];
            var geozone = testData["Device.geozone"];
            var failureName = testData["Device.failure-name"];
            var failureId = testData["Device.failure-id"];
            var time1 = Settings.GetServerTime();
            var time2 = time1.AddHours(1);

            Step("1. Send at least 2 requests: http://5.196.91.118:8080/qa/api/loggingmanagement/setDeviceValues?controllerStrId={{controller id}}&idOnController={{device id}}&valueName={{failure property}}&value=**true**&doLog=true&eventTime={{date time stamp}} (different time stamps)");
            var requestSuccess = SetValueToDevice(controllerId, deviceId, failureId, "ON", time1);
            if (!requestSuccess) Warning(string.Format("Cannot send request to {0}-{1}-{2}-{3}", controllerId, deviceId, failureId, "ON", time1));
            requestSuccess = SetValueToDevice(controllerId, deviceId, failureId, "ON", time2);
            if (!requestSuccess) Warning(string.Format("Cannot send request to {0}-{1}-{2}-{3}", controllerId, deviceId, failureId, "ON", time2));

            Step("2. Go to Failure Tracking and browse to the device specified in step #1");
            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.FailureTracking);
            var failureTrackingPage = desktopPage.GoToApp(App.FailureTracking) as FailureTrackingPage;
            failureTrackingPage.GeozoneTreeMainPanel.SelectNode(geozone + @"\" + deviceName);
            failureTrackingPage.WaitForPreviousActionComplete();

            Step("3. **Expected** Failure Tracking panel appears from the right. Verify failures list contains only 1 {{failure property}}");
            failureTrackingPage.WaitForDetailsPanelDisplayed();
            var failureList = failureTrackingPage.FailureTrackingDetailsPanel.GetListOfFailureName();
            // Verify the failure sent first to make sure the sent request is present
            VerifyEqual("[SC-1048] 3. Verify failures list contains only 1 failure", 1, failureList.Count(i => i.Equals(failureName)));
            // Verify failure list to make sure each failure if any only appears once
            foreach (var failure in failureList)
            {
                VerifyEqual("[SC-1048] 3.1. Verify failures list contains only 1 failure", 1, failureList.Count(i => i.Equals(failure)));
            }
        }

        [Test, DynamicRetry]
        [Description("SLV-2118 Review Geozone Tool bar")]
        public void SLV_2118()
        {
            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.EquipmentInventory);

            Step("1. Go to Equipment Inventory app");
            Step("2. Expected Equipment Inventory page is routed and loaded successfully");
            var equipmentInventoryPage = desktopPage.GoToApp(App.EquipmentInventory) as EquipmentInventoryPage;

            Step("3. Select a geozone");
            Step("4. **Expected** Geozone editor panel appears. Verify 'More...' menu is displayed in panel's header");
            Step("5. Dropdown 'More...' menu");
            Step("6. **Expected** Dropdown menu with 3 items: Import, Export, Replace Nodes");
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(Settings.RootGeozoneName);
            var expectedMoreMenuItems = new List<string>();
            expectedMoreMenuItems.Add("Import");
            expectedMoreMenuItems.Add("Export");
            expectedMoreMenuItems.Add("Replace Nodes");
            var actualMoreMenuItems = equipmentInventoryPage.GeozoneEditorPanel.GetListOfMoreMenuItems();
            VerifyEqual("6. Verify Dropdown menu with 3 items: Import, Export, Replace Nodes", expectedMoreMenuItems, actualMoreMenuItems);
        }

        [Test, DynamicRetry]
        [Description("SLV-1925 Display devices on the map based on search results")]
        public void SLV_1925()
        {
            var testData = GetTestDataOfSLV_1925();
            var mapBounds = (XmlNode)testData["MapBounds"];
            var deviceName = testData["Device"].ToString();
            var lngMin = mapBounds.GetAttrVal("lngMin");
            var lngMax = mapBounds.GetAttrVal("lngMax");
            var latMin = mapBounds.GetAttrVal("latMin");
            var latMax = mapBounds.GetAttrVal("latMax");
            var expectedDevicesCount = int.Parse(mapBounds.GetAttrVal("devicesCount"));

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");            

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.EquipmentInventory, App.RealTimeControl);

            Step("1. Go to Equipment Inventory app");
            Step("2. Expected Equipment Inventory page is routed and loaded successfully");
            var equipmentInventoryPage = desktopPage.GoToApp(App.EquipmentInventory) as EquipmentInventoryPage;

            Step("3. Search for device with exact name (This device should have some devices around it which can be seen when the map is zoomed out to >= 18. e.g 'ToE')");
            Step("4. **Expected** Only device with exact name is displayed in search result grid");
            var searchText = deviceName;
            equipmentInventoryPage.GeozoneTreeMainPanel.ChangeSearchAttribute("Name", "Contains");           
            equipmentInventoryPage.GeozoneTreeMainPanel.EnterSearchTextInput(searchText);
            equipmentInventoryPage.GeozoneTreeMainPanel.ClickSearchButton();
            equipmentInventoryPage.WaitForPreviousActionComplete();
            var searchResult = equipmentInventoryPage.GeozoneTreeMainPanel.SearchResultsGeozonePanel.GetListOfSearchResult("Equipment");
            VerifyEqual("4. Verify Only found devices are displayed in search results grid", searchResult, searchResult.FindAll(s => s.Contains(searchText)));

            Step("5. Zoom the map out to >= 18");
            Step("6. Select the found device in the search result grid");
            Step("7. **Expected** The selected device and ones around it can be seen on the map");
            equipmentInventoryPage.GeozoneTreeMainPanel.SearchResultsGeozonePanel.SelectFirstFoundDevice();
            equipmentInventoryPage.WaitForPreviousActionComplete();
            Wait.ForGLMapStopFlying();
            var devicesCount = equipmentInventoryPage.Map.GetDeviceCount(lngMin, lngMax, latMin, latMax);
            if (equipmentInventoryPage.GeozoneTreeMainPanel.SearchResultsGeozonePanel.IsFilterResultsOnMapTurnedOn())
            {
                VerifyEqual("7. Verify only searched devices are being displayed", 1, devicesCount);
            }
            else
            {
                VerifyEqual("7. Verify all devices are being displayed", expectedDevicesCount, devicesCount);
            }

            Step("8. Toggle on the button 'Filter result on the map'");
            Step("9. **Expected** The button turns from black into orange. Only found device is displayed in the map now");
            Step("10. Toggle off the button 'Filter result on the map'");
            Step("11. **Expected** The button turns from orange into black. Now all devices (the found device and ones around it) are displayed in the map");
            // Toggle first time
            equipmentInventoryPage.GeozoneTreeMainPanel.SearchResultsGeozonePanel.ClickMapFilterButton();
            devicesCount = equipmentInventoryPage.Map.GetDeviceCount(lngMin, lngMax, latMin, latMax);
            if (equipmentInventoryPage.GeozoneTreeMainPanel.SearchResultsGeozonePanel.IsFilterResultsOnMapTurnedOn())
            {
                VerifyEqual("11. Verify only searched devices are being displayed", 1, devicesCount);
            }
            else
            {
                VerifyEqual("11. Verify all devices are being displayed", expectedDevicesCount, devicesCount);
            }
            // Toggle second time
            equipmentInventoryPage.GeozoneTreeMainPanel.SearchResultsGeozonePanel.ClickMapFilterButton();
            devicesCount = equipmentInventoryPage.Map.GetDeviceCount(lngMin, lngMax, latMin, latMax);
            if (equipmentInventoryPage.GeozoneTreeMainPanel.SearchResultsGeozonePanel.IsFilterResultsOnMapTurnedOn())
            {
                VerifyEqual("11. Verify only searched devices are being displayed", 1, devicesCount);
            }
            else
            {
                VerifyEqual("11. Verify all devices are being displayed", expectedDevicesCount, devicesCount);
            }

            Step("12. Repeat the test with apps where geozone tree and map are available (Realtime Control)");
            desktopPage = Browser.RefreshLoggedInCMS();
            Step("12.1. Go to Realtime Control app");
            Step("12.2. Expected Realtime Control  page is routed and loaded successfully");
            var realtimeControlPage = desktopPage.GoToApp(App.RealTimeControl) as RealTimeControlPage;

            Step("12.3. Search for device with exact name (This device should have some devices around it which can be seen when the map is zoomed out to >= 18. e.g 'ToE')");
            Step("12.4. **Expected** Only device with exact name is displayed in search result grid");
            equipmentInventoryPage.GeozoneTreeMainPanel.ChangeSearchAttribute("Name", "Contains");
            searchText = deviceName;
            realtimeControlPage.GeozoneTreeMainPanel.EnterSearchTextInput(searchText);
            realtimeControlPage.GeozoneTreeMainPanel.ClickSearchButton();
            realtimeControlPage.WaitForPreviousActionComplete();
            searchResult = realtimeControlPage.GeozoneTreeMainPanel.SearchResultsGeozonePanel.GetListOfSearchResult("Equipment");
            VerifyEqual("12.4 Only found devices are displayed in search results grid", searchResult, searchResult.FindAll(s => s.Contains(searchText)));

            Step("12.5. Zoom the map out to >= 18");
            Step("12.6. Select the found device in the search result grid");
            Step("12.7. **Expected** The selected device and ones around it can be seen on the map");
            realtimeControlPage.GeozoneTreeMainPanel.SearchResultsGeozonePanel.SelectFirstFoundDevice();
            realtimeControlPage.WaitForPreviousActionComplete();
            Wait.ForGLMapStopFlying();
            devicesCount = realtimeControlPage.Map.GetDeviceCount(lngMin, lngMax, latMin, latMax);
            if (realtimeControlPage.GeozoneTreeMainPanel.SearchResultsGeozonePanel.IsFilterResultsOnMapTurnedOn())
            {
                VerifyEqual("12.7 Verify only searched devices are being displayed", 1, devicesCount);
            }
            else
            {
                VerifyEqual("12.7 Verify all devices are being displayed", expectedDevicesCount, devicesCount);
            }

            Step("12.8. Toggle on the button 'Filter result on the map'");
            Step("12.9. **Expected** The button turns from black into orange. Only found device is displayed in the map now");
            Step("12.10. Toggle off the button 'Filter result on the map'");
            Step("12.11. **Expected** The button turns from orange into black. Now all devices (the found device and ones around it) are displayed in the map");
            // Toggle first time
            realtimeControlPage.GeozoneTreeMainPanel.SearchResultsGeozonePanel.ClickMapFilterButton();
            devicesCount = realtimeControlPage.Map.GetDeviceCount(lngMin, lngMax, latMin, latMax);
            if (realtimeControlPage.GeozoneTreeMainPanel.SearchResultsGeozonePanel.IsFilterResultsOnMapTurnedOn())
            {
                VerifyEqual("12.11 Verify only searched devices are being displayed", 1, devicesCount);
            }
            else
            {
                VerifyEqual("12.11 Verify all devices are being displayed", expectedDevicesCount, devicesCount);
            }
            // Toggle second time
            realtimeControlPage.GeozoneTreeMainPanel.SearchResultsGeozonePanel.ClickMapFilterButton();
            devicesCount = realtimeControlPage.Map.GetDeviceCount(lngMin, lngMax, latMin, latMax);
            if (realtimeControlPage.GeozoneTreeMainPanel.SearchResultsGeozonePanel.IsFilterResultsOnMapTurnedOn())
            {
                VerifyEqual("12.11 Verify only searched devices are being displayed", 1, devicesCount);
            }
            else
            {
                VerifyEqual("12.11 Verify all devices are being displayed", expectedDevicesCount, devicesCount);
            }
        }

        [Test, DynamicRetry]
        [Description("SLV-3885 Equipment Inventory - Adding a device into a specific geozone results in duplicated geozones")]
        public void SLV_3885()
        {
            Step("1. Go to Equipment Inventory app");
            Step("2. Expected Equipment Inventory page is routed and loaded successfully");
            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.EquipmentInventory);
            var equipmentInventoryPage = desktopPage.GoToApp(App.EquipmentInventory) as EquipmentInventoryPage;

            Step("3. Under 'GeoZones', create a new geozone 'A'");
            var newGeozoneNameA = SLVHelper.GenerateUniqueName("GZN3885A");
            equipmentInventoryPage.CreateGeozone(newGeozoneNameA);

            Step("4. Verify geozone 'A' is present under geozone 'GeoZones'");
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(Settings.RootGeozoneName);
            var subNodesList = equipmentInventoryPage.GeozoneTreeMainPanel.GetChildNodeNamesOfSelectedNode(NodeType.GeoZone);
            VerifyEqual(string.Format("4. Verify geozone '{0}' is present under geozone 'GeoZones'", newGeozoneNameA), true, subNodesList.Exists(p => p.Equals(newGeozoneNameA)));

            Step("5. Under 'A', create a new geozone 'B'");
            var newGeozoneNameB = SLVHelper.GenerateUniqueName("GZN3885B");
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(newGeozoneNameA);
            equipmentInventoryPage.WaitForGeozoneEditorPanelDisplayed();

            Step("6. Verify geozone 'B' is present under geozone 'A'");
            equipmentInventoryPage.CreateGeozone(newGeozoneNameB);
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(newGeozoneNameA);
            equipmentInventoryPage.WaitForGeozoneEditorPanelDisplayed();
            subNodesList = equipmentInventoryPage.GeozoneTreeMainPanel.GetChildNodeNamesOfSelectedNode(NodeType.GeoZone);
            VerifyEqual(string.Format("6. Verify geozone '{0}' is present under geozone '{0}'", newGeozoneNameB, newGeozoneNameA), true, subNodesList.Exists(p => p.Equals(newGeozoneNameB)));

            Step("7. Still under 'A', create a new geozone 'C'");
            var newGeozoneNameC = SLVHelper.GenerateUniqueName("GZN3885C");
            equipmentInventoryPage.CreateGeozone(newGeozoneNameC, zoomLevel: ZoomGLLevel.km5);

            Step("8. Verify geozone 'C' is present under geozone 'A'");
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(newGeozoneNameA);
            equipmentInventoryPage.WaitForGeozoneEditorPanelDisplayed();
            subNodesList = equipmentInventoryPage.GeozoneTreeMainPanel.GetChildNodeNamesOfSelectedNode(NodeType.GeoZone);
            VerifyEqual(string.Format("8. Verify geozone '{0}' is present under geozone '{0}'", newGeozoneNameC, newGeozoneNameA), true, subNodesList.Exists(p => p.Equals(newGeozoneNameC)));

            Step("9. Move 'B' into 'C', i.e. geozone 'B' path should be 'GeoZones/A/C/B'");
            equipmentInventoryPage.MoveNodeToGeozoneAndClickYesConfirmed(newGeozoneNameB, newGeozoneNameC);

            Step("10. Verify geozone 'C' is still present under geozone 'A'");
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(newGeozoneNameA);
            equipmentInventoryPage.WaitForGeozoneEditorPanelDisplayed();
            subNodesList = equipmentInventoryPage.GeozoneTreeMainPanel.GetChildNodeNamesOfSelectedNode(NodeType.GeoZone);
            VerifyEqual(string.Format("Verify geozone '{0}' is present under geozone '{0}'", newGeozoneNameC, newGeozoneNameA), true, subNodesList.Exists(p => p.Equals(newGeozoneNameC)));

            Step("11. Verify geozone 'B' is no longer present under geozone 'A' and is present under geozone 'C'");
            subNodesList = equipmentInventoryPage.GeozoneTreeMainPanel.GetChildNodeNamesOfSelectedNode(NodeType.GeoZone);
            VerifyEqual(string.Format("11. Verify geozone '{0}' is no longer present under geozone '{0}'", newGeozoneNameB, newGeozoneNameA), false, subNodesList.Exists(p => p.Equals(newGeozoneNameB)));
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(newGeozoneNameC);
            equipmentInventoryPage.WaitForGeozoneEditorPanelDisplayed();
            subNodesList = equipmentInventoryPage.GeozoneTreeMainPanel.GetChildNodeNamesOfSelectedNode(NodeType.GeoZone);
            VerifyEqual(string.Format("11. Verify geozone '{0}' is present under geozone '{0}'", newGeozoneNameB, newGeozoneNameC), true, subNodesList.Exists(p => p.Equals(newGeozoneNameB)));

            Step("12. Refresh browser");
            desktopPage = Browser.RefreshLoggedInCMS();

            Step("13. Go to Equipment Inventory");
            equipmentInventoryPage = desktopPage.GoToApp(App.EquipmentInventory) as EquipmentInventoryPage;

            Step("14. Create a new streetlight inside GeoZones/A/C/B");
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(string.Format(@"{0}\{1}\{2}", newGeozoneNameA, newGeozoneNameC, newGeozoneNameB));
            equipmentInventoryPage.WaitForGeozoneEditorPanelDisplayed();
            var newDeviceName = SLVHelper.GenerateUniqueName("STL");
            equipmentInventoryPage.CreateDevice(DeviceType.Streetlight, newDeviceName, "Vietnam Controller", newDeviceName, "SSN Cimcon Dim Photocell[Lamp #0]");

            Step("15. Verify The newly-created streetlight is present under geozone 'B'");
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(newGeozoneNameB);
            equipmentInventoryPage.WaitForGeozoneEditorPanelDisplayed();
            subNodesList = equipmentInventoryPage.GeozoneTreeMainPanel.GetChildNodeNamesOfSelectedNode(NodeType.Streetlight);
            VerifyEqual(string.Format("15. Verify streetlight '{0}' is present under geozone '{0}'", newDeviceName, newGeozoneNameB), true, subNodesList.Exists(p => p.Equals(newDeviceName)));

            Step("16. Refresh browser");
            desktopPage = Browser.RefreshLoggedInCMS();

            Step("17. Go to Equipment Inventory");
            equipmentInventoryPage = desktopPage.GoToApp(App.EquipmentInventory) as EquipmentInventoryPage;

            Step("18. Verify geozone 'C' is not present under geozone 'GeoZones'");
            subNodesList = equipmentInventoryPage.GeozoneTreeMainPanel.GetChildNodeNamesOfSelectedNode(NodeType.GeoZone);
            VerifyTrue(string.Format("18. Verify geozone '{0}' is not present under geozone 'GeoZones'", newGeozoneNameC), subNodesList.Exists(p => p.Equals(newGeozoneNameC)) == false, "Not present", "Present");

            Step("19. Verify geozone 'B' is still present under geozone 'C'");
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(string.Format(@"{0}\{1}", newGeozoneNameA, newGeozoneNameC));
            equipmentInventoryPage.WaitForGeozoneEditorPanelDisplayed();
            subNodesList = equipmentInventoryPage.GeozoneTreeMainPanel.GetChildNodeNamesOfSelectedNode(NodeType.GeoZone);
            VerifyTrue(string.Format("19. Verify geozone '{0}' is present under geozone '{0}'", newGeozoneNameB, newGeozoneNameC), subNodesList.Exists(p => p.Equals(newGeozoneNameB)), "Present", "Not present");

            Step("20. Verify The created streetlight is still present under geozone 'B'");
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(newGeozoneNameB);
            equipmentInventoryPage.WaitForGeozoneEditorPanelDisplayed();
            subNodesList = equipmentInventoryPage.GeozoneTreeMainPanel.GetChildNodeNamesOfSelectedNode(NodeType.Streetlight);
            VerifyTrue(string.Format("20. Verify streetlight '{0}' is still present under geozone '{0}'", newDeviceName, newGeozoneNameB), subNodesList.Exists(p => p.Equals(newDeviceName)), "Present", "Not present");
            
            try
            {
                //Remove new geozone
                DeleteGeozone(newGeozoneNameA);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("SC-32 Equipment Inventory - Moving a geozone into a different geozone then saving. It moves it back to the original parent")]
        public void SC_32()
        {
            var oldGeozone = SLVHelper.GenerateUniqueName("GZNSC32-Old");
            var oldSub = SLVHelper.GenerateUniqueName("GZNSC32-Sub");
            var newGeozone = SLVHelper.GenerateUniqueName("GZNSC32-New");

            Step("**** Precondition ****");
            Step(" - Create a new GeoZone named Old Parent");
            Step(" - Create a new GeoZone named SubGZ belonging to Old Parent.");
            Step(" - Create a new GeoZone named New Parent");
            Step(" - Go to Back Office and enable 'Enable geozone parent' in Editor Configuration section of Equipment Inventory");
            Step("**** Precondition ****\n");

            Step("-> Create data for testing");
            DeleteGeozones("GZNSC32*");
            CreateNewGeozone(oldGeozone);
            CreateNewGeozone(newGeozone);
            CreateNewGeozone(oldSub, oldGeozone);

            var loginPage = Browser.OpenBackOfficeApp();
            var backOfficePage = loginPage.LoginAsValidUserToBackOffice(Settings.Users["admin"].Username, Settings.Users["admin"].Password);
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.EquipmentInventory);
            var currentParentGeozoneCheckBoxValue = backOfficePage.BackOfficeDetailsPanel.GetEquipmentParentGeozoneValue();
            backOfficePage.BackOfficeDetailsPanel.TickEquipmentParentGeozoneCheckbox(true);
            backOfficePage.BackOfficeDetailsPanel.ClickSaveButton();
            backOfficePage.WaitForPreviousActionComplete();

            Step("1. Go to Equipment Inventory app");
            Step("2. Expected Equipment Inventory page is routed and loaded successfully");
            var desktopPage = Browser.NavigateToLoggedInCMS();
            var equipmentInventoryPage = desktopPage.GoToApp(App.EquipmentInventory) as EquipmentInventoryPage;

            Step("3. Select the GeoZone Old Parent");
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(oldGeozone);

            Step("4. Expected There is a GeoZone named SubGZ");
            var geozones = equipmentInventoryPage.GeozoneTreeMainPanel.GetChildNodeNamesOfSelectedNode(NodeType.GeoZone);
            VerifyEqual(string.Format("4. Verify There is a GeoZone named {0}", oldSub), true, geozones.Contains(oldSub));

            Step("5. Drag the GeoZone SubGZ and drop it on the GeoZone New Parent");
            equipmentInventoryPage.GeozoneTreeMainPanel.MoveNodeToGeozone(oldSub, newGeozone);
            equipmentInventoryPage.WaitForPopupDialogDisplayed();

            Step("6. Expected A confirmation pop-up displays");
            Step(" - Message: Would you like to move SubGZ geozone and all sub geoZones and equipments?");
            Step(" - Button: Yes, No");
            var expectedMessage = string.Format("Would you like to move {0} geozone and all sub geoZones and equipments ?", oldSub);
            VerifyEqual("6. Verify A confirmation pop-up displays", expectedMessage, equipmentInventoryPage.Dialog.GetMessageText());
            VerifyEqual("6. Verify Button: Yes, No displays", true, equipmentInventoryPage.Dialog.IsYesButtonDisplayed() && equipmentInventoryPage.Dialog.IsNoButtonDisplayed());

            Step("7. Select No button on the pop-up");
            equipmentInventoryPage.Dialog.ClickNoButton();
            equipmentInventoryPage.WaitForPopupDialogDisappeared();

            Step("8. Expected SubGZ is still in Old Parent geozone");
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(oldGeozone);
            geozones = equipmentInventoryPage.GeozoneTreeMainPanel.GetChildNodeNamesOfSelectedNode(NodeType.GeoZone);
            VerifyEqual(string.Format("8. Verify {0} is still in Old Parent geozone", oldSub), true, geozones.Contains(oldSub));

            Step("9. Drag the GeoZone SubGZ and drop it on the GeoZone New Parent");
            equipmentInventoryPage.GeozoneTreeMainPanel.MoveNodeToGeozone(oldSub, newGeozone);
            equipmentInventoryPage.WaitForPopupDialogDisplayed();
           
            Step("10. Select Yes button on the pop-up");
            equipmentInventoryPage.Dialog.ClickYesButton();
            equipmentInventoryPage.WaitForPreviousActionComplete();

            Step("11. Expected The pop-up disappears");
            equipmentInventoryPage.WaitForPopupDialogDisappeared();
            VerifyEqual("11. Verify The pop-up disappears", false, equipmentInventoryPage.IsPopupDialogDisplayed());
            
            Step("12. Expected SubGZ is moved out of Old Parent and exists in New Parent geozone");
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(oldGeozone);
            geozones = equipmentInventoryPage.GeozoneTreeMainPanel.GetChildNodeNamesOfSelectedNode(NodeType.GeoZone);
            VerifyEqual(string.Format("12. Verify {0} is moved out of Old Parent geozone", oldSub), false, geozones.Contains(oldSub));                        
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(newGeozone);
            geozones = equipmentInventoryPage.GeozoneTreeMainPanel.GetChildNodeNamesOfSelectedNode(NodeType.GeoZone);
            VerifyEqual(string.Format("12. Verify {0} exists in New Parent geozone", oldSub), true, geozones.Contains(oldSub));                       
           
            try
            {
                Step("13. Delete test data after testcase is done");
                DeleteGeozone(newGeozone);
                DeleteGeozone(oldGeozone);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("SC-384 Users - Unable to prevent users from accessing WebGL maps")]
        public void SC_384()
        {            
            var expectedUncheckApps = new List<string> { "Equipment Inventory", "Failure Tracking", "Real-Time Control", "Users" };

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - Create a profile 'SLV-384' with a user 'SLV-384' and all applications");
            Step("**** Precondition ****\n");

            var userModel = CreateNewProfileAndUser();
            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.Users);           

            Step("1. In Desktop page, click Users tile");
            Step("2. Expected Users page is routed and loaded successfully");
            var usersPage = desktopPage.GoToApp(App.Users) as UsersPage;            
           
            Step("3. Select the profile 'SLV-384' and scroll to Applications section");
            usersPage.UserProfileListPanel.SelectProfile(userModel.Profile);

            Step("4. Deselect the following applications:");
            Step(" - Equipment Inventory");
            Step(" - Failure Tracking");
            Step(" - Real-time Control");
            Step(" - Users");            
            usersPage.UserProfileDetailsPanel.UncheckApps(expectedUncheckApps.ToArray());
            
            Step("5. Expected Applications are unchecked from the list");
            var actualUncheckApps = usersPage.UserProfileDetailsPanel.GetDisabledAppsName();
            expectedUncheckApps.Sort();
            actualUncheckApps.Sort();
            VerifyEqual("5. Verify applications are unchecked from the list", expectedUncheckApps, actualUncheckApps);

            Step("6. Press Save button");
            usersPage.UserProfileDetailsPanel.ClickSaveButton();
            usersPage.WaitForPreviousActionComplete();
            usersPage.WaitForHeaderMessageDisappeared();

            Step("7. Log Admin out and login by the user 'SLV-384'");
            desktopPage = SLVHelper.LogoutAndLogin(usersPage, userModel.Username, userModel.Password);

            Step("8. Expected Login sucessfully and the applications are not visible from Desktop");
            var installedAppsList = desktopPage.GetInstalledAppsName();
            var hasExisted = installedAppsList.Intersect(expectedUncheckApps).Any();
            VerifyTrue("8. Verify The unchecked apps are not visible in Desktop page", !hasExisted, string.Format("Un-checked Apps: {0}", string.Join(",", expectedUncheckApps)), string.Format("Installed in Desktop: {0}", string.Join(",", installedAppsList)));
            
            Step("9. Press Setting icon");
            desktopPage.AppBar.ClickSettingsButton();
            if (desktopPage.IsSettingsPanelDisplayed())
            {
                desktopPage.SettingsPanel.ClickStoreLink();
                var listStoreDisableApps = desktopPage.SettingsPanel.StorePanel.GetDisabledAppsName();

                Step("10. Expected Setting widget displays, the applications are greyed out in Application tab");
                var listIntersect = listStoreDisableApps.Intersect(expectedUncheckApps).ToList();
                var expectedUncheckedAppCount = expectedUncheckApps.Count;
                var actualIntersectCount = listIntersect.Count;
                VerifyTrue("10. Verify The unchecked apps are greyed out in Applications tab", expectedUncheckedAppCount == actualIntersectCount, string.Format("Un-checked Apps: {0}", string.Join(",", expectedUncheckApps)), string.Format("Disabled apps intersected: {0}", string.Join(",", listIntersect)));
                desktopPage.AppBar.ClickHeaderBartop();

                Step("11. Access randomly an application on the desktop");
                Step("12. Expected The application is launched");
                var randomPage = desktopPage.GoToRandomApp();

                Step("13. Get Applications Switcher (from API: plugin.userContext.desktop.layout.header.headerInstance.appList)");
                var listSwitcherApps = SLVHelper.GetListOfSwitcherApps();

                Step("14. Expected Applications are not visible in the Application Switcher");
                hasExisted = listSwitcherApps.Intersect(expectedUncheckApps).Any();
                VerifyTrue("14. Verify Applications are not visible in the Application Switcher", !hasExisted, string.Format("Un-checked Apps: {0}", string.Join(",", expectedUncheckApps)), string.Format("Applications Switcher: {0}", string.Join(",", installedAppsList)));
                                
                try
                {
                    Step("15. Delete the testing profile and user after testing");
                    DeleteUserAndProfile(userModel);
                }
                catch { }
            }
            else
            {
                Warning("SC-1127: The Security widget breaks the Desktop");
            }           
        }

        [Test, DynamicRetry]
        [Description("SC-592 Scheduling Manager - Unable to delete a Calendar although it is not associated with any device")]
        public void SC_592()
        {
            var testData = GetTestDataOfSC_592();
            var controllerId = testData["ControllerId"];
            var controllerName = testData["ControllerName"];
            var geozone = SLVHelper.GenerateUniqueName("GZNSC592");
            var streetlight = SLVHelper.GenerateUniqueName("STL");
            var streetlightPath = string.Format(@"{0}\{1}", geozone, streetlight);
            var newDimmingGroup = SLVHelper.GenerateUniqueName("CSC592");

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");
            
            Step("-> Create data for testing");
            DeleteGeozones("GZNSC592*");
            CreateNewCalendar(newDimmingGroup);
            CreateNewGeozone(geozone);
            CreateNewDevice(DeviceType.Streetlight, streetlight, controllerId, geozone);

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.EquipmentInventory);

            Step("1. Go to Equipment Inventory app");
            Step("2. Expected Equipment Inventory page is routed and loaded successfully");
            var equipmentInventoryPage = desktopPage.GoToApp(App.EquipmentInventory) as EquipmentInventoryPage;

            Step("3. Select a GeoZone that contains Streetlights");
            Step("4. Select a Streetlight");
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(streetlightPath);
            equipmentInventoryPage.WaitForDeviceEditorPanelDisplayed();

            Step("5. Expected The streetlight Editor panel displays");
            VerifyEqual("5. Verify Streetlight Editor panel appears", true, equipmentInventoryPage.IsDeviceEditorPanelDisplayed());     
           
            Step("6. Press on the Dimming Group and enter the new Dimming Group name to create it, then press Save button");
            equipmentInventoryPage.StreetlightEditorPanel.SelectDimmingGroupDropDown(newDimmingGroup);
            equipmentInventoryPage.StreetlightEditorPanel.ClickSaveButton();
            equipmentInventoryPage.WaitForPreviousActionComplete();
            equipmentInventoryPage.WaitForEditorPanelDisappeared();

            Step("7. Expected The new Dimming Group name is set");
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(streetlight);
            equipmentInventoryPage.WaitForDeviceEditorPanelDisplayed();
            var acutalDimmingGroup = equipmentInventoryPage.StreetlightEditorPanel.GetDimmingGroupValue();
            VerifyEqual("7. Verify The new Dimming Group name is set", newDimmingGroup, acutalDimmingGroup);
            
            Step("8. Select Application Switcher and press on Scheduling Mananger app");
            Step("9. Expected Scheduling Mananger page is routed and loaded successfully");
            var schedulingManagerPage = equipmentInventoryPage.AppBar.SwitchTo(App.SchedulingManager) as SchedulingManagerPage;
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("10. Select Calendar tab");
            schedulingManagerPage.SchedulingManagerPanel.SelectTab("Calendar");
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("11. Expected The new Dimming Group is listed in the list");
            var calendarList = schedulingManagerPage.SchedulingManagerPanel.GetListOfCalendarName();
            var isNewDimmingGroupExisting = calendarList.Contains(newDimmingGroup);
            VerifyEqual("11. Verify The new Dimming Group is listed in the list", true, isNewDimmingGroupExisting);

            Step("12. Select Application Switcher and press on Equipment Inventory app");
            Step("13. Expected Equipment Inventory page is routed and loaded successfully");
            equipmentInventoryPage = schedulingManagerPage.AppBar.SwitchTo(App.EquipmentInventory) as EquipmentInventoryPage;

            Step("14. Select the GeoZone that contains the streetlights in step 3");
            Step("15. Select the streetlight and delete the current Dimming Group, then press Save button");
            equipmentInventoryPage.StreetlightEditorPanel.ClearDimmingGroupDropDown();
            equipmentInventoryPage.StreetlightEditorPanel.ClickSaveButton();
            equipmentInventoryPage.WaitForPreviousActionComplete();
            equipmentInventoryPage.WaitForEditorPanelDisappeared();

            Step("16. Expected The new Dimming Group name is cleared");
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(streetlight);
            equipmentInventoryPage.WaitForDeviceEditorPanelDisplayed();
            acutalDimmingGroup = equipmentInventoryPage.StreetlightEditorPanel.GetDimmingGroupValue();
            VerifyEqual("16. Verify The new Dimming Group name is cleared", "Select or enter a value", acutalDimmingGroup);

            Step("17. Select Application Switcher and press on Scheduling Mananger app");
            Step("18. Expected Scheduling Mananger page is routed and loaded successfully");
            schedulingManagerPage = equipmentInventoryPage.AppBar.SwitchTo(App.SchedulingManager) as SchedulingManagerPage;
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("19. Select Calendar tab");
            schedulingManagerPage.SchedulingManagerPanel.SelectTab("Calendar");
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("20. Expected The Dimming Group is still listed in the list");
            calendarList = schedulingManagerPage.SchedulingManagerPanel.GetListOfCalendarName();
            isNewDimmingGroupExisting = calendarList.Contains(newDimmingGroup);
            VerifyEqual("20. Verify The new Dimming Group is still listed in the list", true, isNewDimmingGroupExisting);
            
            Step("21. Select the Dimming Group");
            schedulingManagerPage.SchedulingManagerPanel.SelectCalendar(newDimmingGroup);
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("22. Expected The Calendar Editor of the Dimming Group is opened");
            var selectedCalendarName = schedulingManagerPage.CalendarEditorPanel.GetNameValue();
            VerifyEqual("22. Verify The Calendar Editor of the Dimming Group is opened", newDimmingGroup, selectedCalendarName);

            Step("23. Press Delete icon");
            schedulingManagerPage.SchedulingManagerPanel.ClickDeleteCalendarButton();
            schedulingManagerPage.WaitForPopupDialogDisplayed();

            Step("24. Expected The confirmation pop-up displays");  
            Step(" • Message: Do you want to delete the calendar '<Dimming Group's Name>'?");
            Step(" • Button: Yes, No");
            var expectedMessage = string.Format("Do you want to delete the calendar '{0}' ?", newDimmingGroup);
            var isMessageAsExpected = expectedMessage.Equals(equipmentInventoryPage.Dialog.GetMessageText());
            if (isMessageAsExpected)
            {
                VerifyEqual("24. Verify A confirmation pop-up displays as expected", true, isMessageAsExpected);
                VerifyEqual("24. Verify Button: Yes, No displays", true, equipmentInventoryPage.Dialog.IsYesButtonDisplayed() && equipmentInventoryPage.Dialog.IsNoButtonDisplayed());

                Step("25. Select No option");
                equipmentInventoryPage.Dialog.ClickNoButton();
                equipmentInventoryPage.WaitForPopupDialogDisappeared();

                Step("26. Expected The pop-up disappeared and the Dimming Group is still listed in the list");
                calendarList = schedulingManagerPage.SchedulingManagerPanel.GetListOfCalendarName();
                isNewDimmingGroupExisting = calendarList.Contains(newDimmingGroup);
                VerifyEqual("26. Verify The pop-up disappeared", false, equipmentInventoryPage.IsPopupDialogDisplayed());
                VerifyEqual("26. Verify The new Dimming Group is still listed in the list", true, isNewDimmingGroupExisting);

                Step("27. Select the Dimming Group");
                schedulingManagerPage.SchedulingManagerPanel.SelectCalendar(newDimmingGroup);
                schedulingManagerPage.WaitForPreviousActionComplete();

                Step("28. Press Delete icon");
                schedulingManagerPage.SchedulingManagerPanel.ClickDeleteCalendarButton();
                schedulingManagerPage.WaitForPopupDialogDisplayed();

                Step("29. Select Yes option");
                equipmentInventoryPage.Dialog.ClickYesButton();
                equipmentInventoryPage.WaitForPopupDialogDisappeared();

                Step("30. Expected The pop-up disappeared and the Dimming Group is removed from the list");
                calendarList = schedulingManagerPage.SchedulingManagerPanel.GetListOfCalendarName();
                isNewDimmingGroupExisting = calendarList.Contains(newDimmingGroup);
                VerifyEqual("30. Verify The pop-up disappeared", false, equipmentInventoryPage.IsPopupDialogDisplayed());
                VerifyEqual("30. Verify The new Dimming Group is removed from the list", false, isNewDimmingGroupExisting);
            }
            else
            {
                Warning("This bug is still happened. Skip steps: 25 -> 30");
            }

            Step("31. Select Application Switcher and press on Equipment Inventory app");
            Step("32. Expected Equipment Inventory page is routed and loaded successfully");
            equipmentInventoryPage = schedulingManagerPage.AppBar.SwitchTo(App.EquipmentInventory) as EquipmentInventoryPage;

            try
            {
                DeleteGeozone(geozone);
            }
            catch { }            
        }

        [Test, DynamicRetry]
        [Description("SC-595 Failure Tracking - Infinite loader when switching from Advanced Search")]
        public void SC_595()
        {
            var testData = GetTestDataOfSC_595();
            var xmlGeoZone = testData["Geozone"];
            var searchName = SLVHelper.GenerateUniqueName(TestContext.CurrentContext.Test.MethodName);
            var expectedAppsSwitcher = new List<string>() { App.EquipmentInventory, App.RealTimeControl, App.DataHistory, App.FailureTracking };

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - Create a search having at least 2 results and save it.");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.AdvancedSearch);
            var advancedSearchPage = desktopPage.GoToApp(App.AdvancedSearch) as AdvancedSearchPage;
            advancedSearchPage.WaitForSearchWizardPopupPanelDisplayed();
            advancedSearchPage.SearchWizardPopupPanel.ClickCloseButton();
            advancedSearchPage.WaitForSearchWizardPopupPanelDisappeared();
            advancedSearchPage.CreateNewSearch(searchName, xmlGeoZone);

            Step("1. Go to Advanced Search app");
            Step("2. Expected Advanced Search page is routed and loaded successfully");
            desktopPage = Browser.RefreshLoggedInCMS();
            advancedSearchPage = desktopPage.GoToApp(App.AdvancedSearch) as AdvancedSearchPage;

            Step("3. Select a saved search created from the precondition.");
            advancedSearchPage.SearchWizardPopupPanel.ClickCloseButton();
            advancedSearchPage.WaitForSearchWizardPopupPanelDisappeared();
            advancedSearchPage.GridPanel.SelectSelectOrAddSearchDropDown(searchName);
            advancedSearchPage.WaitForPreviousActionComplete();
            advancedSearchPage.GridPanel.WaitForGridContentAvailable();

            Step("4. Expected Search Result screen displays with total records > 2");
            var searchDeviceList = advancedSearchPage.GridPanel.GetListOfColumnData("Device");
            searchDeviceList = searchDeviceList.Where(p => p.Contains("Telematics")).ToList();
            VerifyEqual("4. Verify Data of selected search is loaded into grid", true, searchDeviceList.Count > 2);

            Step("5. Double click on the first device");
            var deviceName = searchDeviceList[0];
            advancedSearchPage.GridPanel.DoubleClickGridRecord(deviceName);
            advancedSearchPage.WaitForSwitcherOverlayPanelDisplayed();

            Step("6. Expected Quick Switcher appears and contains the following apps: Equipment Inventory, Real-Time Control, Data History, Failure Tracking.");            
            var actualAppsSwitcher = advancedSearchPage.SwitcherOverlayPanel.GetListOfAppsToSwitch();
            VerifyEqual("6. Verify 4 entries to apps are displayed as expected", expectedAppsSwitcher, actualAppsSwitcher);

            Step("7. Select Failure Tracking");
            var failureTrackingPage = advancedSearchPage.SwitcherOverlayPanel.SwitchToFailureTrackingApp();
            failureTrackingPage.WaitForDetailsPanelDisplayed();

            Step("8. Expected result Failure Tracking page is routed and loaded successfully.");
            VerifyEqual("8. Verify Display data of selected device", deviceName, failureTrackingPage.FailureTrackingDetailsPanel.GetDeviceNameValueText());
            
            Step("9. Go back to Advanced Search using Application Switcher");
            Step("10. Expected Advanced Search page is routed and loaded successfully");
            advancedSearchPage = failureTrackingPage.AppBar.SwitchTo(App.AdvancedSearch) as AdvancedSearchPage;          

            Step("11. Double click on the 2nd device");
            deviceName = searchDeviceList[1];
            advancedSearchPage.GridPanel.DoubleClickGridRecord(deviceName);
            advancedSearchPage.WaitForSwitcherOverlayPanelDisplayed();

            Step("12. Expected Quick Switcher appears and contains the following apps: Equipment Inventory, Real-Time Control, Data History, Failure Tracking.");
            actualAppsSwitcher = advancedSearchPage.SwitcherOverlayPanel.GetListOfAppsToSwitch();
            VerifyEqual("12. Verify 4 entries to apps are displayed as expected", expectedAppsSwitcher, actualAppsSwitcher);

            Step("13. Select Failure Tracking");
            failureTrackingPage = advancedSearchPage.SwitcherOverlayPanel.SwitchToFailureTrackingApp();
            failureTrackingPage.WaitForDetailsPanelDisplayed();

            Step("14. Expected result Failure Tracking page is routed and loaded successfully.");
            VerifyEqual("14. Verify Display data of selected device", deviceName, failureTrackingPage.FailureTrackingDetailsPanel.GetDeviceNameValueText());

            Step("15. Go to Advanced Search app");
            Step("16. Expected Advanced Search page is routed and loaded successfully");
            advancedSearchPage = failureTrackingPage.AppBar.SwitchTo(App.AdvancedSearch) as AdvancedSearchPage;

            Step("17. Double click on the first device");
            deviceName = searchDeviceList[0];
            advancedSearchPage.GridPanel.DoubleClickGridRecord(deviceName);
            advancedSearchPage.WaitForSwitcherOverlayPanelDisplayed();

            Step("18. Expected Quick Switcher appears and contains the following apps: Equipment Inventory, Real-Time Control, Data History, Failure Tracking.");
            actualAppsSwitcher = advancedSearchPage.SwitcherOverlayPanel.GetListOfAppsToSwitch();
            VerifyEqual("18. Verify 4 entries to apps are displayed as expected", expectedAppsSwitcher, actualAppsSwitcher);

            Step("19. Select Data History");
            var dataHistoryPage = advancedSearchPage.SwitcherOverlayPanel.SwitchToDataHistoryApp();
            dataHistoryPage.WaitForGridPanelDisplayed();
            
            Step("20. Expected result Data History page is routed and loaded successfully.");
            VerifyEqual("20. Verify Display data of selected device", deviceName, dataHistoryPage.LastValuesPanel.GetSelectedDeviceText());

            Step("21. Go back to Advanced Search using Application Switcher");
            Step("22. Expected Advanced Search page is routed and loaded successfully");
            advancedSearchPage = dataHistoryPage.AppBar.SwitchTo(App.AdvancedSearch) as AdvancedSearchPage;

            Step("23. Double click on the 2nd device");
            deviceName = searchDeviceList[1];
            advancedSearchPage.GridPanel.DoubleClickGridRecord(deviceName);
            advancedSearchPage.WaitForSwitcherOverlayPanelDisplayed();

            Step("24. Expected Quick Switcher appears and contains the following apps: Equipment Inventory, Real-Time Control, Data History, Failure Tracking.");
            actualAppsSwitcher = advancedSearchPage.SwitcherOverlayPanel.GetListOfAppsToSwitch();
            VerifyEqual("24. Verify 4 entries to apps are displayed as expected", expectedAppsSwitcher, actualAppsSwitcher);

            Step("25. Select Data History");
            dataHistoryPage = advancedSearchPage.SwitcherOverlayPanel.SwitchToDataHistoryApp();
            dataHistoryPage.WaitForGridPanelDisplayed();

            Step("26. Expected result Data History page is routed and loaded successfully.");
            VerifyEqual("26. Verify Display data of selected device", deviceName, dataHistoryPage.LastValuesPanel.GetSelectedDeviceText());

            Step("27. Go to Advanced Search app");
            Step("28. Expected Advanced Search page is routed and loaded successfully");
            advancedSearchPage = dataHistoryPage.AppBar.SwitchTo(App.AdvancedSearch) as AdvancedSearchPage;

            Step("29. Double click on the first device");
            deviceName = searchDeviceList[0];
            advancedSearchPage.GridPanel.DoubleClickGridRecord(deviceName);
            advancedSearchPage.WaitForSwitcherOverlayPanelDisplayed();

            Step("30. Expected Quick Switcher appears and contains the following apps: Equipment Inventory, Real-Time Control, Data History, Failure Tracking.");
            actualAppsSwitcher = advancedSearchPage.SwitcherOverlayPanel.GetListOfAppsToSwitch();
            VerifyEqual("30. Verify 4 entries to apps are displayed as expected", expectedAppsSwitcher, actualAppsSwitcher);

            Step("31. Select Real-time Control");
            var realtimeControlPage = advancedSearchPage.SwitcherOverlayPanel.SwitchToRealtimeControlApp();
            realtimeControlPage.WaitForStreetlightWidgetDisplayed(deviceName);
            
            Step("32. Expected result Real-time Control page is routed and loaded successfully.");
            VerifyEqual("32. Verify Display data of selected device", deviceName, realtimeControlPage.StreetlightWidgetPanel.GetDeviceNameText());

            Step("33. Go back to Advanced Search using Application Switcher");
            Step("34. Expected Advanced Search page is routed and loaded successfully");
            advancedSearchPage = realtimeControlPage.AppBar.SwitchTo(App.AdvancedSearch) as AdvancedSearchPage;

            Step("35. Double click on the 2nd device");
            deviceName = searchDeviceList[1];
            advancedSearchPage.GridPanel.DoubleClickGridRecord(deviceName);
            advancedSearchPage.WaitForSwitcherOverlayPanelDisplayed();

            Step("36. Expected Quick Switcher appears and contains the following apps: Equipment Inventory, Real-Time Control, Data History, Failure Tracking.");
            actualAppsSwitcher = advancedSearchPage.SwitcherOverlayPanel.GetListOfAppsToSwitch();
            VerifyEqual("36. Verify 4 entries to apps are displayed as expected", expectedAppsSwitcher, actualAppsSwitcher);

            Step("37. Select Real-time Control");
            realtimeControlPage = advancedSearchPage.SwitcherOverlayPanel.SwitchToRealtimeControlApp();
            realtimeControlPage.WaitForStreetlightWidgetDisplayed(deviceName);

            Step("38. Expected result Real-time Control page is routed and loaded successfully.");
            VerifyEqual("38. Verify Display data of selected device", deviceName, realtimeControlPage.StreetlightWidgetPanel.GetDeviceNameText());

            Step("39. Go to Advanced Search app");
            Step("40. Expected Advanced Search page is routed and loaded successfully");
            advancedSearchPage = realtimeControlPage.AppBar.SwitchTo(App.AdvancedSearch) as AdvancedSearchPage;

            Step("41. Double click on the first device");
            deviceName = searchDeviceList[0];
            advancedSearchPage.GridPanel.DoubleClickGridRecord(deviceName);
            advancedSearchPage.WaitForSwitcherOverlayPanelDisplayed();

            Step("42. Expected Quick Switcher appears and contains the following apps: Equipment Inventory, Real-Time Control, Data History, Failure Tracking.");
            actualAppsSwitcher = advancedSearchPage.SwitcherOverlayPanel.GetListOfAppsToSwitch();
            VerifyEqual("42. Verify 4 entries to apps are displayed as expected", expectedAppsSwitcher, actualAppsSwitcher);

            Step("43. Select Equipment Inventory");
            var equipmentInventoryPage = advancedSearchPage.SwitcherOverlayPanel.SwitchToEquipmentInventoryApp();
            equipmentInventoryPage.WaitForDeviceEditorPanelDisplayed();
                        
            Step("44. Expected result Equipment Inventory page is routed and loaded successfully.");
            VerifyEqual("44. Verify Display data of selected device", deviceName, equipmentInventoryPage.DeviceEditorPanel.GetNameValue());

            Step("45. Go back to Advanced Search using Application Switcher");
            Step("46. Expected Advanced Search page is routed and loaded successfully");
            advancedSearchPage = equipmentInventoryPage.AppBar.SwitchTo(App.AdvancedSearch) as AdvancedSearchPage;

            Step("47. Double click on the 2nd device");
            deviceName = searchDeviceList[1];
            advancedSearchPage.GridPanel.DoubleClickGridRecord(deviceName);
            advancedSearchPage.WaitForSwitcherOverlayPanelDisplayed();

            Step("48. Expected Quick Switcher appears and contains the following apps: Equipment Inventory, Real-Time Control, Data History, Failure Tracking.");
            actualAppsSwitcher = advancedSearchPage.SwitcherOverlayPanel.GetListOfAppsToSwitch();
            VerifyEqual("48. Verify 4 entries to apps are displayed as expected", expectedAppsSwitcher, actualAppsSwitcher);

            Step("49. Select Equipment Inventory");
            equipmentInventoryPage = advancedSearchPage.SwitcherOverlayPanel.SwitchToEquipmentInventoryApp();
            equipmentInventoryPage.WaitForDeviceEditorPanelDisplayed();

            Step("50. Expected result Equipment Inventory page is routed and loaded successfully.");
            VerifyEqual("50. Verify Display data of selected device", deviceName, equipmentInventoryPage.DeviceEditorPanel.GetNameValue());

            try
            {
                //Delete new search
                advancedSearchPage = equipmentInventoryPage.AppBar.SwitchTo(App.AdvancedSearch) as AdvancedSearchPage;
                advancedSearchPage.GridPanel.DeleleSelectedRequest();
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("SC-382 Scheduling Manager - Commissioning errors are not displayed when a single controller is commisioned")]
        public void SC_382()
        {
            var testData = GetTestDataOfSC_382();
            var controllerId = testData["ControllerId"];
            var controllerName = testData["ControllerName"];
            var streetlight1 = SLVHelper.GenerateUniqueName("STL01");
            var streetlight2 = SLVHelper.GenerateUniqueName("STL02");            
            var calendarName = SLVHelper.GenerateUniqueName("CSC382");
            var geozone = SLVHelper.GenerateUniqueName("GZNSC382");

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - Create 2 streetlights 'SC-382-01', 'SC-382-02', 1 Calendar 'Calendar-SC-382'");
            Step("**** Precondition ****\n");

            Step("-> Create data for testing");
            DeleteGeozones("GZNSC382*");
            CreateNewCalendar(calendarName);
            CreateNewGeozone(geozone);
            CreateNewDevice(DeviceType.Streetlight, streetlight1, controllerId, geozone);
            CreateNewDevice(DeviceType.Streetlight, streetlight2, controllerId, geozone);

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.EquipmentInventory, App.SchedulingManager);

            Step("1. Go to Equipment Inventory app");
            Step("2. Expected Equipment Inventory page is routed and loaded successfully");
            var equipmentInventoryPage = desktopPage.GoToApp(App.EquipmentInventory) as EquipmentInventoryPage;
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(geozone);
            
            Step("3. Set the dimming group of 'SC-382-01', 'SC-382-02' to 'Calendar-SC-382' and press Save button");
            Step(" -> Set the dimming group for {0}", streetlight1);
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(streetlight1);
            equipmentInventoryPage.WaitForDeviceEditorPanelDisplayed();
            equipmentInventoryPage.StreetlightEditorPanel.SelectDimmingGroupDropDown(calendarName);
            equipmentInventoryPage.StreetlightEditorPanel.ClickSaveButton();
            equipmentInventoryPage.WaitForPreviousActionComplete();
            equipmentInventoryPage.WaitForDeviceEditorPanelDisappeared();
            Step(" -> Set the dimming group for {0}", streetlight2);
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(streetlight2);
            equipmentInventoryPage.WaitForDeviceEditorPanelDisplayed();
            equipmentInventoryPage.StreetlightEditorPanel.SelectDimmingGroupDropDown(calendarName);
            equipmentInventoryPage.StreetlightEditorPanel.ClickSaveButton();
            equipmentInventoryPage.WaitForPreviousActionComplete();
            equipmentInventoryPage.WaitForDeviceEditorPanelDisappeared();

            Step("4. Expected The dimming group is updated.");
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(streetlight1);
            equipmentInventoryPage.WaitForDeviceEditorPanelDisplayed();
            VerifyEqual(string.Format("4. Verify The dimming group of '{0}' is updated", streetlight1), calendarName, equipmentInventoryPage.StreetlightEditorPanel.GetDimmingGroupValue());
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(streetlight2);
            equipmentInventoryPage.WaitForDeviceEditorPanelDisplayed();
            VerifyEqual(string.Format("4. Verify The dimming group of '{0}' is updated", streetlight2), calendarName, equipmentInventoryPage.StreetlightEditorPanel.GetDimmingGroupValue());

            Step("5. Go to Scheduling Manager app");
            Step("6. Expected Scheduling Manager page is routed and loaded successfully");
            var schedulingManagerPage = equipmentInventoryPage.AppBar.SwitchTo(App.SchedulingManager) as SchedulingManagerPage;

            Step("7. Select Calendar tab and choose 'Calendar-SC-382'");
            schedulingManagerPage.SchedulingManagerPanel.SelectTab("Calendar");
            schedulingManagerPage.WaitForPreviousActionComplete();
            schedulingManagerPage.SchedulingManagerPanel.SelectCalendar(calendarName);
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("8. Press Commission icon");
            schedulingManagerPage.SchedulingManagerPanel.ClickCommissioningCalendarButton();
            schedulingManagerPage.WaitForPopupDialogDisplayed();
            
            Step("9. Expected The Commissioning pop-up displays with: Controller's Name");
            schedulingManagerPage.CommissionPopupPanel.WaitForGridContentDisplayed();
            var controllerList = schedulingManagerPage.CommissionPopupPanel.GetListOfControllers();
            VerifyTrue("9. Verify The Commissioning pop-up displays with: Controller's Name", controllerList.Contains(controllerName), controllerName, string.Join(",", controllerList));

            Step("10. Press Commission button");
            schedulingManagerPage.CommissionPopupPanel.ClickCommissionButton();
            schedulingManagerPage.Dialog.WaitForPopupMessageDisplayed();
            
            Step("11. Expected The error pop-up displays with:");
            Step(" o Message: 'Errors have been encountered while commissioning control programs.'");
            Step(" o Button: OK");
            VerifyEqual("11. Verify message 'Errors have been encountered while commissioning control programs.' is shown", "Errors have been encountered while commissioning control programs.", schedulingManagerPage.Dialog.GetMessageText());            
            VerifyEqual("11. Button: OK displays", true, schedulingManagerPage.Dialog.IsOkButtonDisplayed());

            Step("12. Press OK button.");
            schedulingManagerPage.Dialog.ClickOkButton();
            schedulingManagerPage.Dialog.WaitForPopupMessageDisappeared();            

            Step("13. Expected The error pop-up dissappears. The Commissioning pop-up displays with:");
            Step(" o The 'Expand' icon");
            Step(" o Controller's Name");
            VerifyEqual("13. Verify The error pop-up dissappears", false, schedulingManagerPage.Dialog.IsPopupMessageDisplayed());
            VerifyEqual("13. Verify The 'Expand' icon is next Controller's Name", true, schedulingManagerPage.CommissionPopupPanel.IsExpandIconNextDevice(controllerName));

            Step("14. Press 'Expand' icon");
            schedulingManagerPage.CommissionPopupPanel.ClickExpandIconNextDevice(controllerName);

            Step("15. Expected The error details displays with:");
            Step(" o The 'Expand' icon is changed to the ' Collapse ' icon");
            Step(" o Description: has error message text");
            VerifyEqual("15. Verify The 'Expand' icon is changed to the 'Collapse' icon", true, schedulingManagerPage.CommissionPopupPanel.IsCollapseIconNextDevice(controllerName));
            VerifyEqual("15. Verify Description: has error message text", true, schedulingManagerPage.CommissionPopupPanel.GetListOfExpandInfomationIcon().Any(p => p.Equals("Error")));

            Step("16. Press the 'Collapse' icon");
            schedulingManagerPage.CommissionPopupPanel.ClickExpandIconNextDevice(controllerName);

            Step("17. Expected The error details is collapsed with:");
            Step(" o The 'Expand' icon");
            Step(" o Controller's Name");
            VerifyEqual("17. The 'Expand' icon is next Controller's Name", true, schedulingManagerPage.CommissionPopupPanel.IsExpandIconNextDevice(controllerName));
            schedulingManagerPage.CommissionPopupPanel.ClickCancelButton();
            schedulingManagerPage.WaitForPopupDialogDisappeared();

            Step("18. Select the Calendar: SlvDemoGroup2");            
            schedulingManagerPage.SchedulingManagerPanel.SelectCalendar("SlvDemoGroup2");
            schedulingManagerPage.WaitForPreviousActionComplete();
           
            Step("19. Press Commission icon");
            schedulingManagerPage.SchedulingManagerPanel.ClickCommissioningCalendarButton();
            schedulingManagerPage.WaitForPopupDialogDisplayed();

            Step("20. Expected The Commissioning pop-up displays with Controller's Name: 'Smartsims' (should use this calendar to test comissioning successfully)");            
            schedulingManagerPage.CommissionPopupPanel.WaitForGridContentDisplayed();
            controllerList = schedulingManagerPage.CommissionPopupPanel.GetListOfControllers();
            VerifyTrue("20. Verify The Commissioning pop-up displays with: Controller's Name", controllerList.Contains("Smartsims"), "Smartsims", string.Join(",", controllerList));

            Step("21. Press Commission button and wait for the process completes.");
            schedulingManagerPage.CommissionPopupPanel.ClickCommissionButton();
            schedulingManagerPage.Dialog.WaitForPopupMessageDisplayed();

            Step("22. Expected The Successful pop-up displays with:");
            Step(" o Message: 'Calendar commissioning has been initiated. This process can take several minutes. You may check for calendar commissioning failures in the Failures tab.'");
            Step(" o Button: OK");
            VerifyEqual("22. Verify message is shown as expected", "Calendar commissioning has been initiated. This process can take several minutes. You may check for calendar commissioning failures in the Failures tab.", schedulingManagerPage.Dialog.GetMessageText());
            VerifyEqual("22. Button: OK displays", true, schedulingManagerPage.Dialog.IsOkButtonDisplayed());

            Step("23. Press OK button.");
            schedulingManagerPage.Dialog.ClickOkButton();
            schedulingManagerPage.Dialog.WaitForPopupMessageDisappeared();

            Step("24. Expected The pop-up dissappears..");
            VerifyEqual("24. Verify The pop-up dissappears", false, schedulingManagerPage.Dialog.IsPopupMessageDisplayed());
            
            try
            {
                Step("25. Delete the testing data after test is done.");
                DeleteGeozone(geozone);
                DeleteCalendar(calendarName);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("SC-615 Scheduling Manager - 12PM bug in control program editor with French locale")]
        public void SC_615()
        {
            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - User with language 'France'");
            Step("**** Precondition ****\n");

            var userModel = CreateNewProfileAndUser(language: "fr_FR");
            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(userModel.Username, userModel.Password);
            desktopPage.InstallAppsIfNotExist(SLVHelper.ConvertAppName(App.SchedulingManager, "French"));

            Step("1. Go to Programmations horaires (Scheduling Manager) app");
            Step("2. Expected Programmations horaires (Scheduling Manager) page is routed and loaded successfully");
            var schedulingManagerPage = desktopPage.GoToApp(App.SchedulingManager) as SchedulingManagerPage;
            
            Step("3. Press the '+' icon to add a new control program");
            schedulingManagerPage.SchedulingManagerPanel.ClickAddControlProgramButton();
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("4. Enter the Nom, Description, and select Type: 'Commutation astronomique avec évènements à heures fixes'");
            var newName = SLVHelper.GenerateUniqueName("CPSC6151");
            schedulingManagerPage.ControlProgramEditorPanel.EnterNameInput(newName);
            schedulingManagerPage.ControlProgramEditorPanel.EnterDescriptionInput(SLVHelper.GenerateUniqueName("Any description"));
            schedulingManagerPage.ControlProgramEditorPanel.SelectTemplateDropDown("Commutation astronomique avec évènements à heures fixes");

            Step("5. Expected The 3 sections: 'Allumage', 'Variations', 'Extinction' are displayed");
            VerifyEqual("5. Verify Allumage is displayed", true, schedulingManagerPage.ControlProgramEditorPanel.IsSwitchOnGroupVisible() && schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOnText() == "Allumage");
            VerifyEqual("5. Verify Variations is displayed", true, schedulingManagerPage.ControlProgramEditorPanel.IsVariationsGroupVisible() && schedulingManagerPage.ControlProgramEditorPanel.GetVariationsText() == "Variations");
            VerifyEqual("5. Verify Extinction is displayed", true, schedulingManagerPage.ControlProgramEditorPanel.IsSwitchOffGroupVisible() && schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOffText() == "Extinction");

            Step("6. Set the time of the first textbox of 'Variations' to 12:00");
            schedulingManagerPage.ControlProgramEditorPanel.EnterVariationTimeInput(0, "12:00");
            schedulingManagerPage.AppBar.ClickHeaderBartop();

            Step("7. Expected The tooltip displays 12:00:00");
            schedulingManagerPage.ControlProgramEditorPanel.MoveToVariationDot(0);
            VerifyEqual("7. Verify The tooltip displays 12:00:00", "12:00:00", schedulingManagerPage.ControlProgramEditorPanel.GetDotTooltipTime());
            
            Step("8. Press Save button.");           
            schedulingManagerPage.ControlProgramEditorPanel.ClickSaveButton();
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("9. Expected The new program control is saved successfully.");
            VerifyEqual("9. Verify The new program control is saved successfully", newName, schedulingManagerPage.SchedulingManagerPanel.GetSelectedControlProgramName());

            Step("10. Update the first textbox to 11:00. Set the second textbox to 12:00 and hover its Diamond icon on the graph");
            schedulingManagerPage.ControlProgramEditorPanel.EnterVariationTimeInput(0, "11:00");
            schedulingManagerPage.ControlProgramEditorPanel.EnterVariationTimeInput(1, "12:00");
            schedulingManagerPage.AppBar.ClickHeaderBartop();

            Step("11. Expected The tooltip displays 12:00:00");
            schedulingManagerPage.ControlProgramEditorPanel.MoveToVariationDot(0);
            VerifyEqual("11. Verify The tooltip displays 12:00:00", "12:00:00", schedulingManagerPage.ControlProgramEditorPanel.GetDotTooltipTime());

            Step("12. Press the '+' icon to add a new control program");
            schedulingManagerPage.SchedulingManagerPanel.ClickAddControlProgramButton();
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("13. Enter the Nom, Description, and select Type: 'Toujours allumé'");
            newName = SLVHelper.GenerateUniqueName("CPSC6152");
            schedulingManagerPage.ControlProgramEditorPanel.EnterNameInput(newName);
            schedulingManagerPage.ControlProgramEditorPanel.EnterDescriptionInput(SLVHelper.GenerateUniqueName("Any description"));
            schedulingManagerPage.ControlProgramEditorPanel.SelectTemplateDropDown("Toujours allumé");

            Step("14. Expected The sections: 'Allumage' is displayed");
            VerifyEqual("14. Verify Allumage is displayed", true, schedulingManagerPage.ControlProgramEditorPanel.IsSwitchOnGroupVisible() && schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOnText() == "Allumage");

            Step("15. Set the time of the textbox of 'Allumage' to 12:00");
            schedulingManagerPage.ControlProgramEditorPanel.EnterSwitchOnTimeInput("12:00");
            schedulingManagerPage.AppBar.ClickHeaderBartop();

            Step("16. Expected The tooltip displays 12:00:00");
            schedulingManagerPage.ControlProgramEditorPanel.MoveToVariationDot(0);
            VerifyEqual("16. Verify The tooltip displays 12:00:00", "12:00:00", schedulingManagerPage.ControlProgramEditorPanel.GetDotTooltipTime());

            Step("17. Press Save button.");
            schedulingManagerPage.ControlProgramEditorPanel.ClickSaveButton();
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("18. Expected The new program control is saved successfully.");
            VerifyEqual("18. Verify The new program control is saved successfully", newName, schedulingManagerPage.SchedulingManagerPanel.GetSelectedControlProgramName());

            Step("19. Press the '+' icon to add a new control program");
            schedulingManagerPage.SchedulingManagerPanel.ClickAddControlProgramButton();
            schedulingManagerPage.WaitForPreviousActionComplete();            

            Step("20. Enter the Nom, Description, and select Type: 'Evènements à heures fixes de jour'");
            newName = SLVHelper.GenerateUniqueName("CPSC6153");
            schedulingManagerPage.ControlProgramEditorPanel.EnterNameInput(newName);
            schedulingManagerPage.ControlProgramEditorPanel.EnterDescriptionInput(SLVHelper.GenerateUniqueName("Any description"));
            schedulingManagerPage.ControlProgramEditorPanel.SelectTemplateDropDown("Evènements à heures fixes de jour");

            Step("21. Expected The sections: 'Variations' is displayed");
            VerifyEqual("21. Verify Variations is displayed", true, schedulingManagerPage.ControlProgramEditorPanel.IsVariationsGroupVisible() && schedulingManagerPage.ControlProgramEditorPanel.GetVariationsText() == "Variations");

            Step("22. Set the time of the first textbox of 'Variations' to 12:00 and hover its Diamond icon on the graph");
            schedulingManagerPage.ControlProgramEditorPanel.EnterVariationTimeInput(0, "12:00");
            schedulingManagerPage.AppBar.ClickHeaderBartop();

            Step("23. Expected The tooltip displays 12:00:00");
            schedulingManagerPage.ControlProgramEditorPanel.MoveToVariationDot(1);
            VerifyEqual("23. Verify The tooltip displays 12:00:00", "12:00:00", schedulingManagerPage.ControlProgramEditorPanel.GetDotTooltipTime());

            Step("24. Press Save button.");
            schedulingManagerPage.ControlProgramEditorPanel.ClickSaveButton();
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("25. Expected The new program control is saved successfully.");
            VerifyEqual("25. Verify The new program control is saved successfully", newName, schedulingManagerPage.SchedulingManagerPanel.GetSelectedControlProgramName());

            Step("26. Update the first textbox to 11:00. Set the second textbox to 12:00");
            schedulingManagerPage.ControlProgramEditorPanel.EnterVariationTimeInput(0, "11:00");
            schedulingManagerPage.ControlProgramEditorPanel.EnterVariationTimeInput(1, "12:00");
            schedulingManagerPage.AppBar.ClickHeaderBartop();

            Step("27. Expected The tooltip displays 12:00:00");
            schedulingManagerPage.ControlProgramEditorPanel.MoveToVariationDot(1);
            VerifyEqual("27. Verify The tooltip displays 12:00:00", "12:00:00", schedulingManagerPage.ControlProgramEditorPanel.GetDotTooltipTime());
           
            try
            {
                Step("28. Delete the testing data after test is done.");
                DeleteControlPrograms("SC_615.*");
                DeleteUserAndProfile(userModel);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("SC-322 Device History not showing recent changes when the user timezone is behind the server timzone")]
        public void SC_322()
        {
            var testData = GetTestDataOfSC_322();
            var controllerId = testData["ControllerId"];
            var controllerName = testData["ControllerName"];
            var geozone = SLVHelper.GenerateUniqueName("GZNSC322");
            var streetlight = SLVHelper.GenerateUniqueName("STL");
            var listAttributes = new List<string>() { "Unique address", "Dimming group", "Configuration status", "Install status", "Config status msg" };
            var listInstallStatus = new List<string>() { "To be verified", "Verified", "New", "Does not exist", "To be installed", "Installed", "Removed" };
            var notedDimmingGroupName = SLVHelper.GenerateString();
            var notedInstallStatus = listInstallStatus.PickRandom();
            var notedMacAddress = SLVHelper.GenerateMACAddress();
            var notedConfigStatus = SLVHelper.GenerateString();
            var notedConfigStatusMessage = SLVHelper.GenerateString(9);       

            Step("**** Precondition ****");            
            Step(" - Create new streetlight");           
            Step("**** Precondition ****\n");

            Step("-> Create data for testing");
            DeleteGeozones("GZNSC322*");
            var userModel = CreateNewProfileAndUser();
            CreateNewCalendar(notedDimmingGroupName, string.Format("Automated calendar for {0}", streetlight));
            CreateNewGeozone(geozone);
            CreateNewDevice(DeviceType.Streetlight, streetlight, controllerId, geozone);

            var loginPage = Browser.OpenBackOfficeApp();
            var backOfficePage = loginPage.LoginAsValidUserToBackOffice(Settings.Users["admin"].Username, Settings.Users["admin"].Password);
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.DeviceHistory);
            if (backOfficePage.BackOfficeDetailsPanel.GetDeviceHistoryEventTimeVisibleValue())
            {
                backOfficePage.BackOfficeDetailsPanel.TickDeviceHistoryEventTimeVisibleCheckbox(false);
                backOfficePage.BackOfficeDetailsPanel.ClickSaveButton();
                backOfficePage.WaitForPreviousActionComplete();
            }
            var desktopPage = Browser.NavigateToLoggedInCMS();
            desktopPage = SLVHelper.LogoutAndLogin(desktopPage, userModel.Username, userModel.Password);
            desktopPage.InstallAppsIfNotExist(App.DeviceHistory);

            Step("1. Prepare the commands to update the values for 5 following attributes of new streetlight with eventTime in the future:");
            Step(" o Dimming group - valueName=DimmingGroupName");
            Step(" o Install status - valueName=installStatus");
            Step(" o Unique address - valueName=MacAddress");
            Step(" o Configuration status - valueName=ConfigStatus");
            Step(" o Config status msg - valueName=ConfigStatusMessage");
            Step("2. Expected All commands are run successfully");           
            var eventDateTime = Settings.GetServerTime().AddDays(2).Date;           
            var requestSuccess = SetValueToDevice(controllerId, streetlight, "DimmingGroupName", notedDimmingGroupName, eventDateTime, "", userModel.Username, userModel.Password);
            if (!requestSuccess) Warning(string.Format("2. Verify sent request to {0}-{1}-{2}-{3}", controllerId, streetlight, "DimmingGroupName", notedDimmingGroupName, eventDateTime));
            requestSuccess = SetValueToDevice(controllerId, streetlight, "installStatus", notedInstallStatus, eventDateTime, "", userModel.Username, userModel.Password);
            if (!requestSuccess) Warning(string.Format("2. Verify sent send request to {0}-{1}-{2}-{3}", controllerId, streetlight, "installStatus", notedInstallStatus, eventDateTime));
            requestSuccess = SetValueToDevice(controllerId, streetlight, "MacAddress", notedMacAddress, eventDateTime, "", userModel.Username, userModel.Password);
            if (!requestSuccess) Warning(string.Format("2. Verify sent send request to {0}-{1}-{2}-{3}", controllerId, streetlight, "MacAddress", notedMacAddress, eventDateTime));
            requestSuccess = SetValueToDevice(controllerId, streetlight, "ConfigStatus ", notedConfigStatus, eventDateTime, "", userModel.Username, userModel.Password);
            if (!requestSuccess) Warning(string.Format("2. Verify sent send request to {0}-{1}-{2}-{3}", controllerId, streetlight, "ConfigStatus", notedConfigStatus, eventDateTime));
            requestSuccess = SetValueToDevice(controllerId, streetlight, "ConfigStatusMessage", notedConfigStatusMessage, eventDateTime, "", userModel.Username, userModel.Password);
            if (!requestSuccess) Warning(string.Format("2. Verify sent send request to {0}-{1}-{2}-{3}", controllerId, streetlight, "ConfigStatusMessage", notedConfigStatusMessage, eventDateTime));

            Step("3. Go to Device History");
            Step("4. Expected Device History app is routed and loaded successfully");
            var deviceHistoryPage = desktopPage.GoToApp(App.DeviceHistory) as DeviceHistoryPage;            

            Step("5. Select new streetlight");
            deviceHistoryPage.GeozoneTreeMainPanel.SelectNode(geozone + @"\" + streetlight);

            Step("6. Select Show and Hide Column button and select 5 attributes: Unique address, Dimming group, Configuration status, Install status, Config status msg");
            deviceHistoryPage.GridPanel.CheckColumnsInShowHideColumnsMenu(listAttributes.ToArray());            

            Step("7. Press Show and Hide Column again, and press Refresh button");
            deviceHistoryPage.AppBar.ClickHeaderBartop();
            deviceHistoryPage.GridPanel.ClickReloadDataToolbarButton();
            deviceHistoryPage.GridPanel.WaitForLeftFooterTextDisplayed();

            Step("8. Select new streetlight in the grid.");
            deviceHistoryPage.GridPanel.ClickGridRecord(streetlight);
            deviceHistoryPage.WaitForPreviousActionComplete();

            Step("9. Expected Verify the values of 5 attributes: Unique address, Dimming group, Configuration status, Install status, Config status msg are equal to values set up in step 1.");
            var dtGridView = deviceHistoryPage.GridPanel.BuildDataTableFromGrid();
            var row = dtGridView.Select(string.Format("Device = '{0}'", streetlight)).FirstOrDefault();
            VerifyEqual("7. Verify Unique address is " + notedMacAddress, notedMacAddress, row["Unique address"].ToString());
            VerifyEqual("7. Verify Dimming group is " + notedDimmingGroupName, notedDimmingGroupName, row["Dimming group"].ToString());
            VerifyEqual("7. Verify Configuration status is " + notedConfigStatus, notedConfigStatus, row["Configuration status"].ToString());
            VerifyEqual("7. Verify Install status is " + notedInstallStatus, notedInstallStatus, row["Install status"].ToString());
            VerifyEqual("7. Verify Config status msg is " + notedConfigStatusMessage, notedConfigStatusMessage, row["Config status msg"].ToString());

            Step("10. Press Attributes filter button on Device History Detail panel of new streetlight");
            deviceHistoryPage.DeviceHistoryPanel.ClickFilterToolbarButton();
            deviceHistoryPage.DeviceHistoryPanel.WaitForAttributesFilterPanelDisplayed();

            Step("11. Select 5 attributes: Unique address, Dimming group, Configuration status, Install status, Config status msg, Latitude, Longitude");
            deviceHistoryPage.DeviceHistoryPanel.CheckFilterAttributes(listAttributes.ToArray());

            Step("12. Press Attributes filter button again and press Refresh button");
            deviceHistoryPage.DeviceHistoryPanel.ClickFilterToolbarButton();
            deviceHistoryPage.DeviceHistoryPanel.WaitForAttributesFilterPanelDisappeared();
            deviceHistoryPage.DeviceHistoryPanel.ClickRefreshToolbarButton();
            deviceHistoryPage.DeviceHistoryPanel.WaitForLoaderSpinDisappeared();

            Step("13. Get the first 5 records of the grid and verify the following information");
            var dt = deviceHistoryPage.DeviceHistoryPanel.BuildDataTableFromGrid();
            var rows = dt.Select(string.Format("User = '{0}'", userModel.Username));

            Step("14. Expected Verify the values of 5 attributes: Unique address, Dimming group, Configuration status, Install status, Config status msg are equal to values set up in step 1.");
            VerifyEqual(string.Format("14. Verify There are 5 rows with user '{0}'", userModel.Username), 5, rows.Count());
            var uniqueAddressRow = rows.FirstOrDefault(p => p["Name"].ToString() == "Unique address");
            if (uniqueAddressRow != null)
            {
                VerifyEqual("14. Verify Time of Unique address is " + eventDateTime, eventDateTime, DateTime.Parse(uniqueAddressRow["Time"].ToString()).Date);
                VerifyEqual("14. Verify Value of Unique address is " + notedMacAddress, notedMacAddress, uniqueAddressRow["Value"].ToString());
            }
            else
                Warning("14. There is no row with Unique address attribute");

            var dimmingGroupRow = rows.FirstOrDefault(p => p["Name"].ToString() == "Dimming group");
            if (dimmingGroupRow != null)
            {
                VerifyEqual("14. Verify Time of Dimming group is " + eventDateTime, eventDateTime, DateTime.Parse(dimmingGroupRow["Time"].ToString()).Date);
                VerifyEqual("14. Verify Value of Dimming group is " + notedDimmingGroupName, notedDimmingGroupName, dimmingGroupRow["Value"].ToString());
            }
            else
                Warning("14. There is no row with Dimming group attribute");

            var configStatusRow = rows.FirstOrDefault(p => p["Name"].ToString() == "Configuration status");
            if (configStatusRow != null)
            {
                VerifyEqual("14. Verify Time of Configuration status is " + eventDateTime, eventDateTime, DateTime.Parse(configStatusRow["Time"].ToString()).Date);
                VerifyEqual("14. Verify Value of Configuration status is " + notedConfigStatus, notedConfigStatus, configStatusRow["Value"].ToString());
            }
            else
                Warning("14. There is no row with Configuration status attribute");

            var installstatusRow = rows.FirstOrDefault(p => p["Name"].ToString() == "Install status");
            if (installstatusRow != null)
            {
                VerifyEqual("14. Verify Time of Install status is " + eventDateTime, eventDateTime, DateTime.Parse(installstatusRow["Time"].ToString()).Date);
                VerifyEqual("14. Verify Value of Install status is " + notedInstallStatus, notedInstallStatus, installstatusRow["Value"].ToString());
            }
            else
                Warning("14. There is no row with Install status attribute");

            var configStatusMsgRow = rows.FirstOrDefault(p => p["Name"].ToString() == "Config status msg");
            if (configStatusMsgRow != null)
            {
                VerifyEqual("14. Verify Time of Config status msg is " + eventDateTime, eventDateTime, DateTime.Parse(configStatusMsgRow["Time"].ToString()).Date);
                VerifyEqual("14. Verify Value of Config status msg is " + notedConfigStatusMessage, notedConfigStatusMessage, configStatusMsgRow["Value"].ToString());
            }
            else
                Warning("14. There is no row with Config status msg attribute");
           
            try
            {
                Step("15. Delete the test data after testcase is done.");
                DeleteGeozone(geozone);
                DeleteUserAndProfile(userModel);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("SC-463 Data History - No API call when clicking on the variable")]
        public void SC_463()
        {
            var testData = GetTestDataOfSC_463();      
            var controllerId = testData["ControllerId"];
            var controllerName = testData["ControllerName"];            
            var geozone = SLVHelper.GenerateUniqueName("GZNSC463");
            var counter = SLVHelper.GenerateUniqueName("MTR");

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - Create an electrical meter");
            Step("**** Precondition ****\n");

            Step("-> Create data for testing");
            DeleteGeozones("GZNSC463*");
            CreateNewGeozone(geozone);
            CreateNewDevice(DeviceType.ElectricalCounter, counter, controllerId, geozone);

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.DataHistory);            

            Step("1. Run the following commands to simulate data for the following Meterings of Electrical Meter:");
            var currentDate = Settings.GetCurrentControlerDateTime(controllerId);
            Step(" - Active power (W)");
            var value = SLVHelper.GenerateStringInteger(999);
            var requestSuccess = SetValueToDevice(controllerId, counter, "SummedPower", value, currentDate);
            if (!requestSuccess) Warning(string.Format("Cannot send request to {0}-{1}-{2}-{3}", controllerId, counter, "SummedPower", value));

            Step(" - Frequency (Hz)");
            value = SLVHelper.GenerateStringInteger(999);
            requestSuccess = SetValueToDevice(controllerId, counter, "Frequency", value, currentDate);
            if (!requestSuccess) Warning(string.Format("Cannot send request to {0}-{1}-{2}-{3}", controllerId, counter, "Frequency", value));

            Step(" - Current - L1 (A)");
            value = SLVHelper.GenerateStringInteger(999);
            requestSuccess = SetValueToDevice(controllerId, counter, "Phase1Current", value, currentDate);
            if (!requestSuccess) Warning(string.Format("Cannot send request to {0}-{1}-{2}-{3}", controllerId, counter, "Phase1Current", value));

            Step(" - Voltage - L1 (V)");
            value = SLVHelper.GenerateStringInteger(999);
            requestSuccess = SetValueToDevice(controllerId, counter, "Phase1Voltage", value, currentDate);
            if (!requestSuccess) Warning(string.Format("Cannot send request to {0}-{1}-{2}-{3}", controllerId, counter, "Phase1Voltage", value));

            Step("2. Go to Data History app");
            Step("3. Expected Data History page is routed and loaded successfully");
            var dataHistoryPage = desktopPage.GoToApp(App.DataHistory) as DataHistoryPage;

            Step("4. Select Electrical Counter");
            dataHistoryPage.GeozoneTreeMainPanel.SelectNode(geozone + @"\" + counter);
            dataHistoryPage.WaitForLastValuePanelDisplayed();

            Step("5. Expected Electrical Meter panel displays with Meterings tab");
            VerifyEqual("4. Verify Electrical Meter panel displays as " + counter, counter, dataHistoryPage.LastValuesPanel.GetSelectedDeviceText());
            VerifyEqual("4. Verify Electrical Meter panel displays with Meterings tab", "Meterings", dataHistoryPage.LastValuesPanel.GetActiveTabText());

            Step("6. Select the Meterings: Active power(W)");
            var metering = "Active power (W)";
            dataHistoryPage.LastValuesPanel.SelectMeteringsAttribute(metering);
            dataHistoryPage.WaitForPreviousActionComplete();

            Step("7. Set Date and Time of From and To the Datetime before the current date, then press Execute button");
            dataHistoryPage.GridPanel.EnterFromDateInput(currentDate.AddDays(-10));
            dataHistoryPage.GridPanel.EnterToDateInput(currentDate.AddDays(-5));
            dataHistoryPage.GridPanel.ClickExecuteButton();
            dataHistoryPage.WaitForPreviousActionComplete();

            Step("8. Expected No data on the graph");
            VerifyEqual("8. Verify No data on the graph", false, dataHistoryPage.GraphPanel.IsChartHasData(counter));

            Step("9. Set Date and Time of From and To the Datetime containing the current date, then press Execute button");
            dataHistoryPage.GridPanel.EnterFromDateInput(currentDate.AddDays(-1));
            dataHistoryPage.GridPanel.EnterToDateInput(currentDate.AddDays(1));
            dataHistoryPage.GridPanel.ClickExecuteButton();
            dataHistoryPage.WaitForPreviousActionComplete();

            Step("10. Expected");
            Step(" o A graph is displayed in main content with data displayed.");
            Step(" o In graph header, selected Metering: Active power(W)");
            VerifyEqual("10. Verify A graph is displayed in main content with data displayed", true, dataHistoryPage.GraphPanel.IsChartHasData(counter));
            VerifyEqual("10. Verify In graph header, selected Metering: Active power(W)", metering, dataHistoryPage.GraphPanel.GetSelectedValues(counter).FirstOrDefault());

            Step("11. Close Active power (w) on the graph, and select the Meterings: Frequency (Hz)");
            dataHistoryPage.GraphPanel.CloseValueFromGraph(counter, metering);
            metering = "Frequency (Hz)";
            dataHistoryPage.LastValuesPanel.SelectMeteringsAttribute(metering);
            dataHistoryPage.WaitForPreviousActionComplete();

            Step("12. Set Date and Time of From and To the Datetime before the current date, then press Execute button");
            dataHistoryPage.GridPanel.EnterFromDateInput(currentDate.AddDays(-10));
            dataHistoryPage.GridPanel.EnterToDateInput(currentDate.AddDays(-5));
            dataHistoryPage.GridPanel.ClickExecuteButton();
            dataHistoryPage.WaitForPreviousActionComplete();

            Step("13. Expected No data on the graph.");
            VerifyEqual("13. Verify No data on the graph", false, dataHistoryPage.GraphPanel.IsChartHasData(counter));

            Step("14. Set Date and Time of From and To the Datetime containing the current date, then press Execute button");
            dataHistoryPage.GridPanel.EnterFromDateInput(currentDate.AddDays(-1));
            dataHistoryPage.GridPanel.EnterToDateInput(currentDate.AddDays(1));
            dataHistoryPage.GridPanel.ClickExecuteButton();
            dataHistoryPage.WaitForPreviousActionComplete();

            Step("15. Expected");
            Step(" o A graph is displayed in main content with data displayed.");
            Step(" o In graph header, selected Metering: Frequency (Hz)");
            VerifyEqual("15. Verify A graph is displayed in main content with data displayed", true, dataHistoryPage.GraphPanel.IsChartHasData(counter));
            VerifyEqual("15. Verify In graph header, selected Metering: Frequency (Hz)", metering, dataHistoryPage.GraphPanel.GetSelectedValues(counter).FirstOrDefault());

            Step("16. Close Frequency (Hz) on the graph, and select the Meterings: Current - L1 (A)");
            dataHistoryPage.GraphPanel.CloseValueFromGraph(counter, metering);
            metering = "Current - L1 (A)";
            dataHistoryPage.LastValuesPanel.SelectMeteringsAttribute(metering);
            dataHistoryPage.WaitForPreviousActionComplete();

            Step("17. Set Date and Time of From and To the Datetime before the current date, then press Execute button");
            dataHistoryPage.GridPanel.EnterFromDateInput(currentDate.AddDays(-10));
            dataHistoryPage.GridPanel.EnterToDateInput(currentDate.AddDays(-5));
            dataHistoryPage.GridPanel.ClickExecuteButton();
            dataHistoryPage.WaitForPreviousActionComplete();

            Step("18. Expected No data on the graph.");
            VerifyEqual("18. Verify No data on the graph", false, dataHistoryPage.GraphPanel.IsChartHasData(counter));

            Step("19. Set Date and Time of From and To the Datetime containing the current date, then press Execute button");
            dataHistoryPage.GridPanel.EnterFromDateInput(currentDate.AddDays(-1));
            dataHistoryPage.GridPanel.EnterToDateInput(currentDate.AddDays(1));
            dataHistoryPage.GridPanel.ClickExecuteButton();
            dataHistoryPage.WaitForPreviousActionComplete();

            Step("20. Expected");
            Step(" o A graph is displayed in main content with data displayed.");
            Step(" o In graph header, selected Metering: Current - L1 (A)");
            VerifyEqual("20. Verify A graph is displayed in main content with data displayed", true, dataHistoryPage.GraphPanel.IsChartHasData(counter));
            VerifyEqual("20. Verify In graph header, selected Metering: Current - L1 (A)", metering, dataHistoryPage.GraphPanel.GetSelectedValues(counter).FirstOrDefault());
            
            Step("21. Close Current - L1 (A) on the graph, and select the Meterings: Voltage - L1 (V)");
            dataHistoryPage.GraphPanel.CloseValueFromGraph(counter, metering);
            metering = "Voltage - L1 (V)";
            dataHistoryPage.LastValuesPanel.SelectMeteringsAttribute(metering);
            dataHistoryPage.WaitForPreviousActionComplete();

            Step("22. Set Date and Time of From and To the Datetime before the current date, then press Execute button");
            dataHistoryPage.GridPanel.EnterFromDateInput(currentDate.AddDays(-10));
            dataHistoryPage.GridPanel.EnterToDateInput(currentDate.AddDays(-5));
            dataHistoryPage.GridPanel.ClickExecuteButton();
            dataHistoryPage.WaitForPreviousActionComplete();

            Step("23. Expected No data on the graph.");
            VerifyEqual("23. Verify No data on the graph", false, dataHistoryPage.GraphPanel.IsChartHasData(counter));

            Step("24. Set Date and Time of From and To the Datetime containing the current date, then press Execute button");
            dataHistoryPage.GridPanel.EnterFromDateInput(currentDate.AddDays(-1));
            dataHistoryPage.GridPanel.EnterToDateInput(currentDate.AddDays(1));
            dataHistoryPage.GridPanel.ClickExecuteButton();
            dataHistoryPage.WaitForPreviousActionComplete();

            Step("25. Expected");
            Step(" o A graph is displayed in main content with data displayed.");
            Step(" o In graph header, selected Metering: Voltage - L1 (V)");
            VerifyEqual("25. Verify A graph is displayed in main content with data displayed", true, dataHistoryPage.GraphPanel.IsChartHasData(counter));
            VerifyEqual("25. Verify In graph header, selected Metering: Voltage - L1 (V)", metering, dataHistoryPage.GraphPanel.GetSelectedValues(counter).FirstOrDefault());
                        
            try
            {
                Step("26. Clear the data after test is done.");
                DeleteGeozone(geozone);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("SC-10-01 Advance Search - Display Calendar Commission Failures in CMS - Success Case")]
        public void SC_10_01()
        {
            var testData = GetTestDataOfSC_10_01();
            var controllerName = testData["ControllerName"];
            var controllerId = testData["ControllerId"];
            var geozone = SLVHelper.GenerateUniqueName("GZNSC1001");
            var dimmingGroup = SLVHelper.GenerateUniqueName("CSC1001");            
            var streetlight = SLVHelper.GenerateUniqueName("STL");            

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - Create new streetlight with controller = Smartsims commision");
            Step("**** Precondition ****\n");

            Step("-> Create data for testing");
            DeleteGeozones("GZNSC1001*");
            CreateNewGeozone(geozone);
            CreateNewDevice(DeviceType.Streetlight, streetlight, controllerId, geozone);
            SetValueToDevice(controllerId, streetlight, "DimmingGroupName", dimmingGroup, Settings.GetCurrentControlerDateTime(controllerId).AddMinutes(10));

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.SchedulingManager, App.AdvancedSearch, App.EquipmentInventory);

            Step("1. Go to Scheduling Manager app");
            Step("2. Expected Scheduling Manager page is routed and loaded successfully");
            var schedulingManagerPage = desktopPage.GoToApp(App.SchedulingManager) as SchedulingManagerPage;

            Step("3. Select 'Calendar' tab, then select the calendar named of new streetlight");
            schedulingManagerPage.SchedulingManagerPanel.SelectTab("Calendar");
            schedulingManagerPage.WaitForPreviousActionComplete();
            schedulingManagerPage.SchedulingManagerPanel.SelectCalendar(dimmingGroup);
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("4. Press Commission icon");
            schedulingManagerPage.SchedulingManagerPanel.ClickCommissioningCalendarButton();
            schedulingManagerPage.WaitForPopupDialogDisplayed();

            Step("5. Expected Commissioning pop-up displays containing the controller of new streetlight");
            schedulingManagerPage.CommissionPopupPanel.WaitForGridContentDisplayed();
            var controllerList = schedulingManagerPage.CommissionPopupPanel.GetListOfControllers();
            VerifyTrue("5. Verify Commissioning pop-up displays containing the controller of new streetlight", controllerList.Contains(controllerName), controllerName, string.Join(",", controllerList));

            Step("6. Press Commission icon");
            schedulingManagerPage.CommissionPopupPanel.ClickCommissionButton();
            schedulingManagerPage.Dialog.WaitForPopupMessageDisplayed();

            Step("7. Expected The message displays");
            Step(" o Message: Calendar commissioning has been initiated. This process can take several minutes. You may check for calendar commissioning failures in the Failures tab.");
            VerifyEqual("7. Verify message is shown as expected", "Calendar commissioning has been initiated. This process can take several minutes. You may check for calendar commissioning failures in the Failures tab.", schedulingManagerPage.Dialog.GetMessageText());

            Step("8. Send a comment to simulate the Canlendar Commission success with");
            Step(" o valueName=calendarCommissionFailure");
            Step(" o value=false");
            Step(" o eventTime= the current time of controller");
            var eventTime = Settings.GetCurrentControlerDateTime(controllerId);
            var status = SetValueToDevice(controllerId, streetlight, "calendarCommissionFailure", false, eventTime);
            VerifyEqual(string.Format("8. Verify send request for streetlight {0} success", streetlight), true, status);

            Step("9. Press Ok button and close Commissioning pop-up");
            schedulingManagerPage.Dialog.ClickOkButton();
            schedulingManagerPage.Dialog.WaitForPopupMessageDisappeared();
            schedulingManagerPage.CommissionPopupPanel.ClickCancelButton();
            schedulingManagerPage.WaitForPopupDialogDisappeared();

            Step("10. Press Application Switcher and select Advanced Search");
            Step("11. Expected Advance Search app is routed and loaded successfully");
            var advancedSearchPage = desktopPage.AppBar.SwitchTo(App.AdvancedSearch) as AdvancedSearchPage;

            Step("12. Select 'New Advanced Search', then enter the name and press Next icon");
            var searchName = SLVHelper.GenerateUniqueName(TestContext.CurrentContext.Test.MethodName);
            advancedSearchPage.SearchWizardPopupPanel.ClickNewAdvancedSearchButton();
            advancedSearchPage.SearchWizardPopupPanel.WaitForNewSearchNameInputVisible();
            advancedSearchPage.SearchWizardPopupPanel.EnterNewSearchNameInput(searchName);
            advancedSearchPage.SearchWizardPopupPanel.ClickNextButton();
            advancedSearchPage.SearchWizardPopupPanel.WaitForGeozoneFormDisplayed();

            Step("13. Select the geozone of new streetlight and press Next");
            advancedSearchPage.SearchWizardPopupPanel.GeozoneTreePopupPanel.SelectNode(geozone);
            advancedSearchPage.SearchWizardPopupPanel.ClickNextButton();
            advancedSearchPage.SearchWizardPopupPanel.WaitForAttributeFormDisplayed();

            Step("14. Check the attribute 'Calendar Commission Failure', then press Next");
            advancedSearchPage.SearchWizardPopupPanel.UnCheckAllAttributeList();

            var checkedList = new List<string>() { "Calendar Commission Failure" };
            advancedSearchPage.SearchWizardPopupPanel.CheckAttributeList(checkedList.ToArray());
            advancedSearchPage.SearchWizardPopupPanel.ClickNextButton();
            advancedSearchPage.SearchWizardPopupPanel.WaitForFilterFormDisplayed();

            Step("15. Press Next and Finish the search.");
            advancedSearchPage.SearchWizardPopupPanel.ClickNextButton();
            advancedSearchPage.SearchWizardPopupPanel.WaitForFinishFormDisplayed();
            advancedSearchPage.SearchWizardPopupPanel.WaitForDeviceSearchCompleted();

            advancedSearchPage.SearchWizardPopupPanel.ClickFinishButton();
            advancedSearchPage.WaitForSearchWizardPopupPanelDisappeared();
            advancedSearchPage.WaitForPreviousActionComplete();
            advancedSearchPage.GridPanel.WaitForPanelLoaded();

            Step("16. Expected The search result displays with the device listed");
            VerifyEqual("15. Verify The search result displays with the device listed", true, advancedSearchPage.GridPanel.GetListOfDevices().Any());

            Step("17. Check 'Timestamp' option and horizontal scroll to 'Calendar Commission Failure'. Press Refresh button");
            if (!advancedSearchPage.GridPanel.IsTimeStampButtonChecked()) advancedSearchPage.GridPanel.ClickTimeStampToolbarButton();
            advancedSearchPage.GridPanel.ClickReloadDataToolbarButton();
            advancedSearchPage.WaitForPreviousActionComplete();

            Step("18. Expected Value: false. Timestamp: the time when the 'Calendar Commission Failure' occurred");
            advancedSearchPage.GridPanel.ClickGridRecord(streetlight);
            Wait.ForSeconds(1);
            var selectedRecord = advancedSearchPage.GridPanel.GetSelectedGridRecordData();
            var value = selectedRecord[1];
            DateTime timestampDate;
            var parseSuccess = DateTime.TryParse(selectedRecord[2], out timestampDate) == true;
            VerifyEqual(string.Format("17. Verify Value is false", streetlight), "false", value);
            if (parseSuccess)
                VerifyEqual(string.Format("17. Verify Timestamp is the time when the 'Calendar Commission Failure", streetlight), eventTime.Date, timestampDate.Date);
            else
                Warning(string.Format("17. Parse timestamp fail ({0})", selectedRecord[2]));
           
            try
            {
                Step("19. Delete test data after testcase is done");
                DeleteGeozone(geozone);
                DeleteCalendar(dimmingGroup);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("SC-10-02 Advance Search - Display Calendar Commission Failures in CMS - Failure Case")]
        public void SC_10_02()
        {
            var testData = GetTestDataOfSC_10_02();
            var controllerName = testData["ControllerName"];
            var controllerId = testData["ControllerId"];
            var dimmingGroup = SLVHelper.GenerateUniqueName("CSC1002");
            var streetlight = SLVHelper.GenerateUniqueName("STL");
            var geozone = SLVHelper.GenerateUniqueName("GZNSC1002");

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - Create a new streetlight with no commission controller and a new dimming group");
            Step("**** Precondition ****\n");

            Step("-> Create data for testing");
            DeleteGeozones("GZNSC1002*");
            CreateNewCalendar(dimmingGroup);
            CreateNewGeozone(geozone);
            CreateNewDevice(DeviceType.Streetlight, streetlight, controllerId, geozone);
            SetValueToDevice(controllerId, streetlight, "DimmingGroupName", dimmingGroup, Settings.GetCurrentControlerDateTime(controllerId));
            
            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.SchedulingManager, App.AdvancedSearch);

            Step("1. Go to Scheduling Manager app");
            Step("2. Expected Scheduling Manager page is routed and loaded successfully");
            var schedulingManagerPage = desktopPage.GoToApp(App.SchedulingManager) as SchedulingManagerPage;

            Step("3. Select 'Calendar' tab, then select the calendar named of new streetlight");
            schedulingManagerPage.SchedulingManagerPanel.SelectTab("Calendar");
            schedulingManagerPage.WaitForPreviousActionComplete();
            schedulingManagerPage.SchedulingManagerPanel.SelectCalendar(dimmingGroup);
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("4. Press Commission icon");
            schedulingManagerPage.SchedulingManagerPanel.ClickCommissioningCalendarButton();
            schedulingManagerPage.WaitForPopupDialogDisplayed();

            Step("5. Expected Commissioning pop-up displays containing the controller of new streetlight");
            schedulingManagerPage.CommissionPopupPanel.WaitForGridContentDisplayed();
            var controllerList = schedulingManagerPage.CommissionPopupPanel.GetListOfControllers();
            VerifyTrue("5. Verify Commissioning pop-up displays containing the controller of new streetlight", controllerList.Contains(controllerName), controllerName, string.Join(",", controllerList));

            Step("6. Press Commission icon");
            schedulingManagerPage.CommissionPopupPanel.ClickCommissionButton();
            schedulingManagerPage.Dialog.WaitForPopupMessageDisplayed();

            Step("7. Expected The message displays");
            Step(" o Message: Errors have been encountered while commissioning control programs.");            
            VerifyEqual("7. Verify message 'Errors have been encountered while commissioning control programs.' is shown", "Errors have been encountered while commissioning control programs.", schedulingManagerPage.Dialog.GetMessageText());

            Step("8. Send a comment to simulate the Canlendar Commission success with");
            Step(" o valueName=calendarCommissionFailure");
            Step(" o value=true");
            Step(" o eventTime= the current local time");
            var eventTime = Settings.GetCurrentControlerDateTime(controllerId);
            var status = SetValueToDevice(controllerId, streetlight, "calendarCommissionFailure", true, eventTime);
            VerifyEqual(string.Format("8. Verify send request for streetlight {0} success", streetlight), true, status);

            Step("9. Press Ok button and close Commissioning pop-up");
            schedulingManagerPage.Dialog.ClickOkButton();
            schedulingManagerPage.Dialog.WaitForPopupMessageDisappeared();
            schedulingManagerPage.CommissionPopupPanel.ClickCancelButton();
            schedulingManagerPage.WaitForPopupDialogDisappeared();

            Step("10. Press Application Switcher and select Advanced Search");
            Step("11. Expected Advance Search app is routed and loaded successfully");
            var advancedSearchPage = desktopPage.AppBar.SwitchTo(App.AdvancedSearch) as AdvancedSearchPage;

            Step("12. Select 'New Advanced Search', then enter the name and press Next icon");
            var searchName = SLVHelper.GenerateUniqueName(TestContext.CurrentContext.Test.MethodName);
            advancedSearchPage.SearchWizardPopupPanel.ClickNewAdvancedSearchButton();
            advancedSearchPage.SearchWizardPopupPanel.WaitForNewSearchNameInputVisible();
            advancedSearchPage.SearchWizardPopupPanel.EnterNewSearchNameInput(searchName);
            advancedSearchPage.SearchWizardPopupPanel.ClickNextButton();
            advancedSearchPage.SearchWizardPopupPanel.WaitForGeozoneFormDisplayed();

            Step("13. Select the geozone of new streetlight and press Next");
            advancedSearchPage.SearchWizardPopupPanel.GeozoneTreePopupPanel.SelectNode(geozone);
            advancedSearchPage.SearchWizardPopupPanel.ClickNextButton();
            advancedSearchPage.SearchWizardPopupPanel.WaitForAttributeFormDisplayed();

            Step("14. Check the attribute 'Calendar Commission Failure', then press Next");
            advancedSearchPage.SearchWizardPopupPanel.UnCheckAllAttributeList();

            var checkedList = new List<string>() { "Calendar Commission Failure" };
            advancedSearchPage.SearchWizardPopupPanel.CheckAttributeList(checkedList.ToArray());
            advancedSearchPage.SearchWizardPopupPanel.ClickNextButton();
            advancedSearchPage.SearchWizardPopupPanel.WaitForFilterFormDisplayed();

            Step("15. Press Next and Finish the search.");
            advancedSearchPage.SearchWizardPopupPanel.ClickNextButton();
            advancedSearchPage.SearchWizardPopupPanel.WaitForFinishFormDisplayed();
            advancedSearchPage.SearchWizardPopupPanel.WaitForDeviceSearchCompleted();

            advancedSearchPage.SearchWizardPopupPanel.ClickFinishButton();
            advancedSearchPage.WaitForSearchWizardPopupPanelDisappeared();
            advancedSearchPage.WaitForPreviousActionComplete();
            advancedSearchPage.GridPanel.WaitForPanelLoaded();

            Step("16. Expected The search result displays with the device listed");
            VerifyEqual("16. Verify The search result displays with the device listed", true, advancedSearchPage.GridPanel.GetListOfDevices().Any());

            Step("17. Check 'Timestamp' option and horizontal scroll to 'Calendar Commission Failure'. Press Refresh button");
            if (!advancedSearchPage.GridPanel.IsTimeStampButtonChecked()) advancedSearchPage.GridPanel.ClickTimeStampToolbarButton();
            advancedSearchPage.GridPanel.ClickReloadDataToolbarButton();
            advancedSearchPage.WaitForPreviousActionComplete();

            Step("18. Expected Value: true. Timestamp: the time when the 'Calendar Commission Failure' occurred");           
            advancedSearchPage.GridPanel.ClickGridRecord(streetlight);
            Wait.ForSeconds(1);
            var selectedRecord = advancedSearchPage.GridPanel.GetSelectedGridRecordData();
            var value = selectedRecord[1];
            DateTime timestampDate;
            var parseSuccess = DateTime.TryParse(selectedRecord[2], out timestampDate) == true;
            VerifyEqual(string.Format("18. Verify Value is true", streetlight), "true", value);
            if(parseSuccess)
                VerifyEqual(string.Format("18. Verify Timestamp is the time when the 'Calendar Commission Failure", streetlight), eventTime.Date, timestampDate.Date);
            else
                Warning(string.Format("18. Parse timestamp fail ({0})", selectedRecord[2]));
            
            try
            {
                Step("19. Delete test data after testcase is done");
                DeleteGeozone(geozone);
                DeleteCalendar(dimmingGroup);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("SC-34 Equipment Inventory - Remove the duplicate button for Controller devices")]
        public void SC_34()
        {
            var controller = SLVHelper.GenerateUniqueName("CTRL");
            var geozone = SLVHelper.GenerateUniqueName("GZNSC34");

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - Create a new Controller");
            Step("**** Precondition ****\n");

            Step("-> Create data for testing");
            DeleteGeozones("GZNSC34*");
            CreateNewGeozone(geozone);
            CreateNewController(controller, geozone);

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.EquipmentInventory, App.RealTimeControl);

            Step("1. Go to Equipment Inventory app");
            Step("2. Expected Equipment Inventory page is routed and loaded successfully");
            var equipmentInventoryPage = desktopPage.GoToApp(App.EquipmentInventory) as EquipmentInventoryPage;

            Step("3. Select the new controller");
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(geozone + @"\" + controller);            
           
            Step("4. Expected Controller Editor panel. There is no Duplicate button on the Controller Device Editor panel.");
            VerifyEqual("4. Verify Controller Device Editor panel is displayed", true, equipmentInventoryPage.IsDeviceEditorPanelDisplayed());
            VerifyEqual("4. Verify Duplicate button is not visible", false, equipmentInventoryPage.ControllerEditorPanel.IsDuplicateButtonDisplayed());

            Step("5. Select Application Switcher, then choose 'Real Time Control'");
            var realtimeControlPage = equipmentInventoryPage.AppBar.SwitchTo(App.RealTimeControl) as RealTimeControlPage;

            Step("6. Expected Real Time Control is routed and loaded successfully.");
            if(realtimeControlPage.IsHeaderMessageDisplayed()) realtimeControlPage.WaitForHeaderMessageDisappeared();
            realtimeControlPage.WaitForControllerWidgetDisplayed(controller);

            Step("7. Select Application Switcher, then choose 'Equipment Inventory'");
            equipmentInventoryPage = realtimeControlPage.AppBar.SwitchTo(App.EquipmentInventory) as EquipmentInventoryPage;

            Step("8. Expected");
            Step(" o Equipment Inventory app is routed and loaded successfully.");
            Step(" o Controller Device Editor panel of controller is displayed without Duplicate button on it");
            equipmentInventoryPage.WaitForDeviceEditorPanelDisplayed();
            VerifyEqual("8. Verify Controller Device Editor panel is displayed", true, equipmentInventoryPage.IsDeviceEditorPanelDisplayed());
            VerifyEqual("8. Verify Duplicate button is not visible", false, equipmentInventoryPage.ControllerEditorPanel.IsDuplicateButtonDisplayed());

            try
            {
                Step("9. Delete test data after testcase is done");
                DeleteGeozone(geozone);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("SC-89 After commissioning, MAC Address field becomes read-only in the UI")]
        [NonParallelizable]
        [Category("RunAlone")]
        public void SC_89()
        {
            var testData = GetTestDataOfSC_89();
            var notCommissionControllerName = testData["NotCommisionControllerName"];
            var notCommissionControllerId = testData["NotCommisionControllerId"];
            var commissionControllerName = testData["CommisionControllerName"];
            var commissionControllerId = testData["CommisionControllerId"];           
            var commissionCalendar = testData["Calendar"];
            var validMacAddressInCSV1 = SLVHelper.GenerateMACAddress();
            var notCommissionCalendar = SLVHelper.GenerateUniqueName("CSC89");
            var geozone = SLVHelper.GenerateUniqueName("GZNSC89");
            var streetlight = SLVHelper.GenerateUniqueName("STL");
            var streelightPath = geozone + @"\" + streetlight;
            var fullGeozonePath = Settings.RootGeozoneName + @"/" + geozone;
            var typeOfEquipment = "ABEL-Vigilon A[Dimmable ballast]";
            var csv1 = Settings.GetFullPath(Settings.CSV_FILE_PATH + "SC-89-01.csv");
            var csv2 = Settings.GetFullPath(Settings.CSV_FILE_PATH + "SC-89-02.csv");

            Step("**** Precondition ****");
            Step(" - Create a new calendar named Calendar-SC-89 and a new Streetlight named SL-SC-89-01 in geozone");
            Step(" - Prepare a csv file named SC-89-01 to update Unique Mac for SL-SC-89-01");
            Step(" - Prepare a csv file named SC-89-02 to update Unique Mac for SL-SC-89-01 to another Mac Address.");
            Step("**** Precondition ****\n");

            Step("-> Create data for testing");
            DeleteGeozones("GZNSC89*");
            var longitue = SLVHelper.GenerateLongitude();
            var latitude = SLVHelper.GenerateLatitude();
           
            CreateCsv(DeviceType.Streetlight, csv1, fullGeozonePath, notCommissionControllerId, streetlight, typeOfEquipment, latitude, longitue, validMacAddressInCSV1);
            CreateCsv(DeviceType.Streetlight, csv2, fullGeozonePath, commissionControllerId, streetlight, typeOfEquipment, latitude, longitue);            
            CreateNewCalendar(notCommissionCalendar);
            CreateNewGeozone(geozone);
            CreateNewDevice(DeviceType.Streetlight, streetlight, notCommissionControllerId, geozone, lat: latitude, lng: longitue);
            SetValueToDevice(notCommissionControllerId, streetlight, "DimmingGroupName", notCommissionCalendar, Settings.GetCurrentControlerDateTime(notCommissionControllerId).AddMinutes(10));            

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.EquipmentInventory, App.SchedulingManager);

            Step("1. Go to Equipment Inventory app");
            var equipmentInventoryPage = desktopPage.GoToApp(App.EquipmentInventory) as EquipmentInventoryPage;            

            Step("2. Select streetlight SL-SC-89-01");
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(streelightPath);

            Step("3. Select Commission button");
            equipmentInventoryPage.StreetlightEditorPanel.ClickCommissionButton();
            equipmentInventoryPage.StreetlightEditorPanel.WaitForCommissionPanelDisplayed();
            equipmentInventoryPage.WaitForPreviousActionComplete();

            Step("4. Expected Commission panel displays with the error message");
            Step(" o Message: Connection error Connect to localhost...");
            VerifyEqual("4. Verify Commission panel appears", true, equipmentInventoryPage.StreetlightEditorPanel.IsCommissionPanelDisplayed());
            VerifyEqual("4. Verify Commission panel has the error message: Connection error Connect to localhost...", true, equipmentInventoryPage.StreetlightEditorPanel.CommissionPanel.GetListOfMessagesText().Exists(p => Regex.IsMatch(p, "Connection error Connect to localhost*")));
            
            Step("5. Close the Commission panel");
            equipmentInventoryPage.StreetlightEditorPanel.CommissionPanel.ClickBackButton();
            equipmentInventoryPage.StreetlightEditorPanel.WaitForCommissionPanelDisappeared();

            Step("6. Expected Unique address is still enabled.");
            VerifyEqual("6. Verify Unique address is still enabled", true, !equipmentInventoryPage.StreetlightEditorPanel.IsUniqueAddressReadOnly());

            Step("7. Using API to import the csv SC-89-01 file, then refresh the page and check streetlight SL-SC-89-01");
            ImportFile(csv1);
            desktopPage = Browser.RefreshLoggedInCMS();
            equipmentInventoryPage = desktopPage.GoToApp(App.EquipmentInventory) as EquipmentInventoryPage;
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(streelightPath);
            equipmentInventoryPage.WaitForDeviceEditorPanelDisplayed();

            Step("8. Expected Streetlight Editor panel displays with unique address updated the new value in csv file.");            
            VerifyEqual("8. Verify 'Unique address' is matched the value in csv file.", validMacAddressInCSV1, equipmentInventoryPage.StreetlightEditorPanel.GetUniqueAddressValue());

            Step("9. Change the Controller ID to 'Smartsims commission', save changes then select SL-SC-89-01");
            equipmentInventoryPage.StreetlightEditorPanel.SelectControllerIdDropDown(commissionControllerName);
            equipmentInventoryPage.StreetlightEditorPanel.ClickSaveButton();
            equipmentInventoryPage.WaitForPreviousActionComplete();
            equipmentInventoryPage.WaitForDeviceEditorPanelDisappeared();

            Step("10. Select SL-SC-89-01, then select Commission button");
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(streetlight);
            equipmentInventoryPage.WaitForDeviceEditorPanelDisplayed();
            equipmentInventoryPage.StreetlightEditorPanel.ClickCommissionButton();
            equipmentInventoryPage.StreetlightEditorPanel.WaitForCommissionPanelDisplayed();
            equipmentInventoryPage.WaitForPreviousActionComplete();

            Step("11. Expected Commission panel displays and contains the following message");
            Step(" o Message: All messages and configuration data have been sent and accepted by the controller");
            var message = "All messages and configuration data have been sent and accepted by the controller";
            VerifyEqual("11. Verify Commission panel appears", true, equipmentInventoryPage.StreetlightEditorPanel.IsCommissionPanelDisplayed());
            VerifyEqual("11. Verify Commission panel contains the following message " + message, true, equipmentInventoryPage.StreetlightEditorPanel.CommissionPanel.GetListOfMessagesText().Contains(message));

            Step("12. Close the Commission panel");
            equipmentInventoryPage.StreetlightEditorPanel.CommissionPanel.ClickBackButton();
            equipmentInventoryPage.StreetlightEditorPanel.WaitForCommissionPanelDisappeared();

            Step("13. Expected Unique address is readonly, cannot change its value.");   
            VerifyEqual("13. Verify Unique address is readonly", true, equipmentInventoryPage.StreetlightEditorPanel.IsUniqueAddressReadOnly());
            
            Step("14. Select the parent geozone, import file csv SC-89-02");
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(geozone);
            equipmentInventoryPage.WaitForGeozoneEditorPanelDisplayed();
            equipmentInventoryPage.Import(csv2);

            Step("15. Expected Import panel displays with the error -Message detail: Could not set variable 'MacAddress' on device");
            VerifyEqual("15. Verify The Import panel appears", true, equipmentInventoryPage.GeozoneEditorPanel.IsImportPanelDisplayed());
            VerifyEqual("15. Verify Message detail: Could not set variable 'MacAddress' on device...variable is marked as read-only!", true, equipmentInventoryPage.GeozoneEditorPanel.ImportPanel.GetListOfErrors().Exists(p => Regex.IsMatch(p, "Could not set variable 'MacAddress' on device*")));
            Warning("[SC-49] Error details message is cut off");

            Step("16. Close Import panel and select SL-SC-89-01");
            equipmentInventoryPage.GeozoneEditorPanel.ImportPanel.ClickBackButton();
            equipmentInventoryPage.GeozoneEditorPanel.WaitForImportPanelDisappeared();
            equipmentInventoryPage.WaitForPreviousActionComplete();
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(streetlight);
            equipmentInventoryPage.WaitForDeviceEditorPanelDisplayed();

            Step("17. Expected Unique address is not updated (value is not changed) and still readonly");
            VerifyEqual("17. Verify Unique address is still readonly", true, equipmentInventoryPage.StreetlightEditorPanel.IsUniqueAddressReadOnly());
            VerifyEqual("17. Verify Unique address is not updated", validMacAddressInCSV1, equipmentInventoryPage.StreetlightEditorPanel.GetUniqueAddressValue());

            Step("18. Update Controller ID to 'Vietnam Controller'. Save and select streetlight again.");
            equipmentInventoryPage.StreetlightEditorPanel.SelectControllerIdDropDown(notCommissionControllerName);
            equipmentInventoryPage.StreetlightEditorPanel.ClickSaveButton();
            equipmentInventoryPage.WaitForPreviousActionComplete();
            equipmentInventoryPage.WaitForDeviceEditorPanelDisappeared();
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(streetlight);
            equipmentInventoryPage.WaitForDeviceEditorPanelDisplayed();

            Step("19. Select Commission button");
            equipmentInventoryPage.StreetlightEditorPanel.ClickCommissionButton();
            equipmentInventoryPage.StreetlightEditorPanel.WaitForCommissionPanelDisplayed();
            equipmentInventoryPage.WaitForPreviousActionComplete();

            Step("20. Expected Commission panel displays with the error message");
            Step(" o Message: Device list commissionin not supported!");
            VerifyEqual("20. Verify Commission panel appears", true, equipmentInventoryPage.StreetlightEditorPanel.IsCommissionPanelDisplayed());
            VerifyEqual("20. Verify Commission panel has the error message: Connection error Connect to localhost...", true, equipmentInventoryPage.StreetlightEditorPanel.CommissionPanel.GetListOfMessagesText().Exists(p => Regex.IsMatch(p, "Connection error Connect to localhost*")));

            Step("21. Close the Commission panel, and input some data into Unique address in Streetlight Editor panel");
            equipmentInventoryPage.StreetlightEditorPanel.CommissionPanel.ClickBackButton();
            equipmentInventoryPage.StreetlightEditorPanel.WaitForCommissionPanelDisappeared();

            Step("22. Expected Unique address is readonly");
            VerifyEqual("22. Verify Unique address is readonly", true, equipmentInventoryPage.StreetlightEditorPanel.IsUniqueAddressReadOnly());

            Step("23. Press Replace Node button");
            equipmentInventoryPage.StreetlightEditorPanel.ClickReplaceNodeButton();
            equipmentInventoryPage.StreetlightEditorPanel.WaitForReplaceNodePanelDisplayed();
     
            Step("24. Expected Replace Node panel appears with Unique address editable.");
            VerifyEqual("24. Replace Node panel appears with Unique address editable", true, !equipmentInventoryPage.StreetlightEditorPanel.ReplaceNodePanel.IsUniqueAddressReadOnly());
           
            Step("25. Close Replace Node panel and select Application Switcher and choose Scheduling Manager app");
            Step("26. Expected Scheduling Manager app is routed and loaded successfully");
            equipmentInventoryPage.StreetlightEditorPanel.ReplaceNodePanel.ClickBackButton();
            equipmentInventoryPage.StreetlightEditorPanel.WaitForReplaceNodePanelDisappeared();
            var schedulingManagerPage = equipmentInventoryPage.AppBar.SwitchTo(App.SchedulingManager) as SchedulingManagerPage;

            Step("27. Select Calendar tab and choose 'Calendar-SC-89', then press Commission button");
            schedulingManagerPage.SchedulingManagerPanel.SelectTab("Calendar");
            schedulingManagerPage.WaitForPreviousActionComplete();
            schedulingManagerPage.SchedulingManagerPanel.SelectCalendar(notCommissionCalendar);
            schedulingManagerPage.WaitForPreviousActionComplete();
            schedulingManagerPage.SchedulingManagerPanel.ClickCommissioningCalendarButton();
            schedulingManagerPage.WaitForPopupDialogDisplayed();
            
            Step("28. Expected Commissioning pop-up displays");
            schedulingManagerPage.CommissionPopupPanel.WaitForGridContentDisplayed();            
            VerifyEqual("28. Verify Commissioning pop-up displays", true, schedulingManagerPage.IsPopupDialogDisplayed());
            
            Step("29. Press Commissioning button on the pop-up");
            schedulingManagerPage.CommissionPopupPanel.ClickCommissionButton();
            schedulingManagerPage.Dialog.WaitForPopupMessageDisplayed();
                 
            Step("30. Expected The error message displays");
            VerifyEqual("30. Verify message 'Errors have been encountered while commissioning control programs.' is shown", "Errors have been encountered while commissioning control programs.", schedulingManagerPage.Dialog.GetMessageText());
            
            Step("31. Close the error message and Commissioning pop-up, select Application Switcher, then choose Equipment Inventory app.");
            schedulingManagerPage.Dialog.ClickOkButton();
            schedulingManagerPage.Dialog.WaitForPopupMessageDisappeared();
            schedulingManagerPage.CommissionPopupPanel.ClickCancelButton();
            schedulingManagerPage.WaitForPopupDialogDisappeared();
            equipmentInventoryPage = schedulingManagerPage.AppBar.SwitchTo(App.EquipmentInventory) as EquipmentInventoryPage;

            Step("32. Expected Streetlight Editor panel of SL-SC-89-01 appears with Unique address readonly");
            equipmentInventoryPage.WaitForDeviceEditorPanelDisplayed();
            VerifyEqual("32. Verify Unique address is readonly", true, equipmentInventoryPage.StreetlightEditorPanel.IsUniqueAddressReadOnly());

            Step("33. Update the Controller ID: 'Smartsims commission' and Dimming group: SlvDemoGroup2");            
            equipmentInventoryPage.StreetlightEditorPanel.SelectControllerIdDropDown(commissionControllerName);
            equipmentInventoryPage.StreetlightEditorPanel.SelectDimmingGroupDropDown(commissionCalendar);
            equipmentInventoryPage.StreetlightEditorPanel.ClickSaveButton();
            equipmentInventoryPage.WaitForPreviousActionComplete();
            equipmentInventoryPage.WaitForDeviceEditorPanelDisappeared();

            Step("34. Select Application Switcher and Scheduling Manager app");
            schedulingManagerPage = equipmentInventoryPage.AppBar.SwitchTo(App.SchedulingManager) as SchedulingManagerPage;

            Step("35. Select calendar: SlvDemoGroup2, then press Commission button");
            schedulingManagerPage.SchedulingManagerPanel.SelectTab("Calendar");
            schedulingManagerPage.WaitForPreviousActionComplete();
            schedulingManagerPage.SchedulingManagerPanel.SelectCalendar(commissionCalendar);
            schedulingManagerPage.WaitForPreviousActionComplete();
            schedulingManagerPage.SchedulingManagerPanel.ClickCommissioningCalendarButton();
            schedulingManagerPage.WaitForPopupDialogDisplayed();

            Step("36. Expected Commissioning pop-up displays");
            VerifyEqual("36. Verify Commissioning pop-up displays", true, schedulingManagerPage.IsPopupDialogDisplayed());

            Step("37. Press Commission button on the pop-up.");
            schedulingManagerPage.CommissionPopupPanel.ClickCommissionButton();
            schedulingManagerPage.Dialog.WaitForPopupMessageDisplayed();

            Step("38. Expected The message displays 'Calendar commissioning has been initiated. This process can take several minutes. You may check for calendar commissioning failures in the Failures tab.'");
            VerifyEqual("38. Verify message is shown as expected", "Calendar commissioning has been initiated. This process can take several minutes. You may check for calendar commissioning failures in the Failures tab.", schedulingManagerPage.Dialog.GetMessageText());

            Step("39. Close the message and Commissioning pop-up, select Application Switcher, then choose Equipment Inventory app.");
            schedulingManagerPage.Dialog.ClickOkButton();
            schedulingManagerPage.Dialog.WaitForPopupMessageDisappeared();
            schedulingManagerPage.CommissionPopupPanel.ClickCancelButton();
            schedulingManagerPage.WaitForPopupDialogDisappeared();
            equipmentInventoryPage = schedulingManagerPage.AppBar.SwitchTo(App.EquipmentInventory) as EquipmentInventoryPage;

            Step("40. Expected Streetlight Editor panel of SL-SC-89-01 appears with Unique address readonly");
            equipmentInventoryPage.WaitForDeviceEditorPanelDisplayed();
            VerifyEqual("40. Verify Unique address is readonly", true, equipmentInventoryPage.StreetlightEditorPanel.IsUniqueAddressReadOnly());

            Step("41. Press Replace Node button");
            equipmentInventoryPage.StreetlightEditorPanel.ClickReplaceNodeButton();
            equipmentInventoryPage.StreetlightEditorPanel.WaitForReplaceNodePanelDisplayed();

            Step("42. Expected Replace Node panel appears with Unique address editable.");
            VerifyEqual("42. Replace Node panel appears with Unique address editable", true, !equipmentInventoryPage.StreetlightEditorPanel.ReplaceNodePanel.IsUniqueAddressReadOnly());            
            
            try
            {
                Step("43. Delete the test data after testcase is done.");
                DeleteGeozone(geozone);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("SC-90 Equipment Inventory - Validate MAC Address format and related error handling")]
        [NonParallelizable]
        [Category("RunAlone")]
        public void SC_90()
        {
            var testData = GetTestDataOfSC_90();
            var controllerName = testData["ControllerName"];
            var controllerId = testData["ControllerId"];      
            var typeOfEquipment = "ABEL-Vigilon A[Dimmable ballast]";
            var geozone = SLVHelper.GenerateUniqueName("GZNSC90");
            var streetlight1 = SLVHelper.GenerateUniqueName("STL01");
            var streetlight2 = SLVHelper.GenerateUniqueName("STL02");
            var streetlight3 = SLVHelper.GenerateUniqueName("STL03");
            var fullGeozonePath = Settings.RootGeozoneName + @"/" + geozone;
            var invalidMacAddressInCSV = SLVHelper.GenerateStringMixedNumber(12).ToUpper();
            var validMacAddressInCSV = SLVHelper.GenerateMACAddress();
            var invalidMacAddressCsvPath = Settings.GetFullPath(Settings.CSV_FILE_PATH + "SC-90-InvalidMacAddress.csv");
            var validMacAddressCsvPath = Settings.GetFullPath(Settings.CSV_FILE_PATH + "SC-90-ValidMacAddress.csv");

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            Step("-> Create data for testing");
            DeleteGeozones("GZNSC90*");
            var longitue1 = SLVHelper.GenerateLongitude();
            var latitude1 = SLVHelper.GenerateLatitude();
            var longitue2 = SLVHelper.GenerateLongitude();
            var latitude2 = SLVHelper.GenerateLatitude();            
            CreateCsv(DeviceType.Streetlight, invalidMacAddressCsvPath, fullGeozonePath, controllerId, streetlight1, typeOfEquipment, SLVHelper.GenerateLatitude(), SLVHelper.GenerateLongitude(), invalidMacAddressInCSV);           
            CreateCsv(DeviceType.Streetlight, validMacAddressCsvPath, fullGeozonePath, controllerId, streetlight2, typeOfEquipment, SLVHelper.GenerateLatitude(), SLVHelper.GenerateLongitude(), validMacAddressInCSV);
            CreateNewGeozone(geozone);
            CreateNewDevice(DeviceType.Streetlight, streetlight1, controllerId, geozone, typeOfEquipment);

            Step("1. Go to Equipment Inventory app");
            Step("2. Expected Equipment Inventory page is routed and loaded successfully");
            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.EquipmentInventory, App.Installation);
            var equipmentInventoryPage = desktopPage.GoToApp(App.EquipmentInventory) as EquipmentInventoryPage;

            Step("3. Select the geozone");
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(geozone);

            Step("4. Expected Geozone Editor panel displays");
            VerifyEqual("4. Verify Geozone Editor panel displays", true, equipmentInventoryPage.IsGeozoneEditorPanelDisplayed());

            Step("5. Add a new streetlight named ST-SC-90-01");
            Step("6. Expected Streetlight Editor panel displays");
            equipmentInventoryPage.CreateDevice(DeviceType.Streetlight, streetlight1, controllerName, streetlight1, typeOfEquipment);

            Step("7. Input the invalid MAC for 'Unique address' field, then press Save button");
            var invalidMacAddress = SLVHelper.GenerateStringMixedNumber(12).ToUpper();
            equipmentInventoryPage.StreetlightEditorPanel.EnterUniqueAddressInput(invalidMacAddress);
            equipmentInventoryPage.StreetlightEditorPanel.ClickSaveButton();
            equipmentInventoryPage.WaitForPopupDialogDisplayed();

            Step("8. Expected The error message displays");
            Step(" o Message: Invalid value '<invalid value>' for 'MacAddress'!");
            var errorMessage = string.Format("Invalid value '{0}' for 'MacAddress'!", invalidMacAddress);
            VerifyEqual("8. Verify The error message displays", true, equipmentInventoryPage.IsPopupDialogDisplayed());
            VerifyEqual("8. Verify message displays as expected", errorMessage, equipmentInventoryPage.Dialog.GetMessageText());            

            Step("9. Close the error message and input the valid Mac, then press Save button");
            equipmentInventoryPage.Dialog.ClickOkButton();
            equipmentInventoryPage.WaitForPopupDialogDisappeared();
            var validMacAddess = SLVHelper.GenerateMACAddress();
            equipmentInventoryPage.StreetlightEditorPanel.EnterUniqueAddressInput(validMacAddess);
            equipmentInventoryPage.StreetlightEditorPanel.ClickSaveButton();
            equipmentInventoryPage.WaitForPreviousActionComplete();

            Step("10. Select ST-SC-90-01 again");
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(streetlight1);
            equipmentInventoryPage.WaitForEditorPanelDisplayed();

            Step("11. Expected Mac Address is saved successfully");            
            VerifyEqual("10. Verify Mac Address is saved successfully", validMacAddess, equipmentInventoryPage.StreetlightEditorPanel.GetUniqueAddressValue());

            Step("12. Update 'Unique address' with invalid value and press Save button");
            invalidMacAddress = SLVHelper.GenerateStringMixedNumber(12).ToUpper();
            equipmentInventoryPage.StreetlightEditorPanel.EnterUniqueAddressInput(invalidMacAddress);
            equipmentInventoryPage.StreetlightEditorPanel.ClickSaveButton();
            equipmentInventoryPage.WaitForPopupDialogDisplayed();

            Step("13. Expected Expectation at step 8");
            errorMessage = string.Format("Invalid value '{0}' for 'MacAddress'!", invalidMacAddress);
            VerifyEqual("13. Verify The error message displays", true, equipmentInventoryPage.IsPopupDialogDisplayed());
            VerifyEqual("13. Verify message displays as expected", errorMessage, equipmentInventoryPage.Dialog.GetMessageText());

            Step("14. Close the error message and close the Streetlight Editor panel, then select 'ST-SC-90-01' again");
            equipmentInventoryPage.Dialog.ClickOkButton();
            equipmentInventoryPage.WaitForPopupDialogDisappeared();
            equipmentInventoryPage.StreetlightEditorPanel.ClickBackButton();
            equipmentInventoryPage.WaitForDeviceEditorPanelDisappeared();
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(streetlight1);
            equipmentInventoryPage.WaitForEditorPanelDisplayed();

            Step("15. Expected 'Unique address' is not changed.");
            VerifyEqual("15. Verify Unique address' is not changed", validMacAddess, equipmentInventoryPage.StreetlightEditorPanel.GetUniqueAddressValue());
            
            Step("16. Press 'Replace Node' button");
            Step("17. Expected Replace Node panel displays with 'Unique address'");
            equipmentInventoryPage.StreetlightEditorPanel.ClickReplaceNodeButton();
            equipmentInventoryPage.StreetlightEditorPanel.WaitForReplaceNodePanelDisplayed();             

            Step("18. Input the invalid Mac, then press Save button");
            invalidMacAddress = SLVHelper.GenerateStringMixedNumber(12).ToUpper();
            equipmentInventoryPage.StreetlightEditorPanel.ReplaceNodePanel.EnterUniqueAddressInput(invalidMacAddress);            
            equipmentInventoryPage.StreetlightEditorPanel.ReplaceNodePanel.ClickSaveButton();
            equipmentInventoryPage.WaitForPopupDialogDisplayed();

            Step("19. Expected The error message displays");
            Step(" o Message: ERROR:'<Streetlight id>'-Invalid MacAddress format '<invalid value>'!");
            errorMessage = string.Format("ERROR:{0}-Invalid MacAddress format '{1}'!", streetlight1, invalidMacAddress);
            VerifyEqual("19. Verify The error message displays", true, equipmentInventoryPage.IsPopupDialogDisplayed());
            VerifyEqual("19. Verify message displays as expected", errorMessage, equipmentInventoryPage.Dialog.GetMessageText());

            Step("20. Close the error message and close Replace Node panel, then close Streetlight Editor panel");
            equipmentInventoryPage.Dialog.ClickOkButton();
            equipmentInventoryPage.WaitForPopupDialogDisappeared();
            equipmentInventoryPage.StreetlightEditorPanel.ReplaceNodePanel.ClickBackButton();
            equipmentInventoryPage.StreetlightEditorPanel.WaitForReplaceNodePanelDisappeared();
            equipmentInventoryPage.StreetlightEditorPanel.ClickBackButton();
            equipmentInventoryPage.WaitForDeviceEditorPanelDisappeared();

            Step("21. Expected All panels are closed.");
            VerifyEqual("21. Verify All panels are closed", false, equipmentInventoryPage.IsDeviceEditorPanelDisplayed());

            Step("22. Select the geozone SC-90, then press More button and select Import option. Select the file csv with the invalid Mac Address in the precondition, and press Open.");
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(geozone);
            equipmentInventoryPage.WaitForGeozoneEditorPanelDisplayed();
            equipmentInventoryPage.Import(invalidMacAddressCsvPath);

            Step("23. Verify The Import panel appears with the error.");
            Step(" o Message: One device could not be imported.");
            Step(" o Details:");
            Step("   - Error setting property 'MacAddress' with value '<Invalid value in csv>':Invalid value '<Invalid value in csv>' for 'MacAddress'!");
            Step("   - Invalid value '<invalid value in csv>' for 'MacAddress'!");
            Step(" o Note: SC-49 makes the message is cut off.");
            VerifyEqual("23. Verify The Import panel appears", true, equipmentInventoryPage.GeozoneEditorPanel.IsImportPanelDisplayed());
            VerifyEqual("23. Verify Message: One device could not be imported.", "One device could not be imported.", equipmentInventoryPage.GeozoneEditorPanel.ImportPanel.GetMessageCaptionText());            
            var errorsDetail = equipmentInventoryPage.GeozoneEditorPanel.ImportPanel.GetListOfErrors();
            if (errorsDetail.Count == 2)
            {
                var error1 = string.Format("Error setting property 'MacAddress' with value '{0}':Invalid value '{0}' for 'MacAddress'!", invalidMacAddressInCSV);
                VerifyEqual(string.Format("23. Verify Details: {0}", error1), error1, errorsDetail[0]);
                var error2 = string.Format("Invalid value '{0}' for 'MacAddress'!", invalidMacAddressInCSV);
                VerifyEqual(string.Format("23. Verify Details: {0}", error2), error2, errorsDetail[1]);
                Warning("[SC-49] Error details message is cut off");
            }
            else
            {
                Warning("Error Details are not as expected");
            }

            equipmentInventoryPage.GeozoneEditorPanel.ImportPanel.ClickBackButton();
            equipmentInventoryPage.GeozoneEditorPanel.WaitForImportPanelDisappeared();
            equipmentInventoryPage.WaitForPreviousActionComplete();
           
            Step("24. Close Import panel, and select More button, then select Import option again. Select the file csv with the valid Mac Address in the precondition, and press Open.");
            Step(" o Note: SC-669 related to device is still created although Mac Address is invalid.");
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(Settings.RootGeozoneName + @"\" + geozone);
            equipmentInventoryPage.WaitForGeozoneEditorPanelDisplayed();
            equipmentInventoryPage.Import(validMacAddressCsvPath);

            Step("25. Verify The Import panel appears with the message");
            Step(" o One device has been updated.");
            VerifyEqual("25. Verify The Import panel appears", true, equipmentInventoryPage.GeozoneEditorPanel.IsImportPanelDisplayed());
            VerifyEqual("25. Verify Message: One device has been updated.", "One device has been updated.", equipmentInventoryPage.GeozoneEditorPanel.ImportPanel.GetMessageCaptionText());

            Step("26. Close Import panel and select the newly created streetlight 'ST-SC-90-02'");
            Step(" o Note: SC-751 Device not displayed on the map after importing a CSV");
            equipmentInventoryPage.GeozoneEditorPanel.ImportPanel.ClickBackButton();
            equipmentInventoryPage.GeozoneEditorPanel.WaitForImportPanelDisappeared();
            equipmentInventoryPage.WaitForPreviousActionComplete();

            Step("27. Verify The Streetlight Editor panel is displayed and 'Unique address' is matched the value in csv file.");
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(streetlight2);
            equipmentInventoryPage.WaitForDeviceEditorPanelDisplayed();
            VerifyEqual("27. Verify 'Unique address' is matched the value in csv file.", validMacAddressInCSV, equipmentInventoryPage.StreetlightEditorPanel.GetUniqueAddressValue());           

            Step("28. Go to Installation app");            
            Step("29. Verify Installation app is routed and loaded successfully");
            var installationPage = equipmentInventoryPage.AppBar.SwitchTo(App.Installation) as InstallationPage;

            Step("30. Press Cancel on the pop-up 'ST-SC-90-02', Select geozone: SC-90 then select 'Add Streetlight' button on the top right corner and set the position for it.");
            installationPage.WaitForPopupDialogDisplayed();
            installationPage.InstallationPopupPanel.ClickCancelButton();
            installationPage.WaitForPopupDialogDisappeared();
            installationPage.Map.ClickAddStreetlightButton();
            installationPage.Map.WaitForRecorderDisplayed();
            installationPage.Map.PositionNewDevice();
            installationPage.Map.WaitForRecorderDisappeared();            
            installationPage.WaitForPreviousActionComplete();
            installationPage.WaitForPopupDialogDisplayed();

            Step("31. Verify The pop-up 'ST-SC-90-02' is closed. The pop-up 'New Streetlight' displays");
            VerifyEqual("31. Verify The pop-up 'New Streetlight' displays.", true, installationPage.IsPopupDialogDisplayed());
            VerifyEqual("31. Verify The pop-up 'New Streetlight' displays.", "New Streetlight", installationPage.InstallationPopupPanel.GetPanelTitleText());
            
            Step("32. Input Type of equipment, controller, and Name: ST-SC-90-03, then press Next");
            installationPage.InstallationPopupPanel.SelectTypeOfEquipmentDropDown(typeOfEquipment);
            installationPage.InstallationPopupPanel.SelectControllerIdDropDown(controllerName);            
            installationPage.InstallationPopupPanel.EnterNameInput(streetlight3);
            installationPage.InstallationPopupPanel.ClickNextButton();

            Step("33. Verify 'Unique address' and 'NIC serial number' fields appears");
            installationPage.InstallationPopupPanel.WaitForScanQRCodeFormDisplayed();

            Step("34. Input the invalid Mac Address and press Next until finishing.");   
            Step(" o Note: SC-669 related to device is still created although Mac Address is invalid.");
            invalidMacAddress = SLVHelper.GenerateStringMixedNumber(12).ToUpper();
            installationPage.InstallationPopupPanel.EnterUniqueAddressInput(invalidMacAddress);
            installationPage.InstallationPopupPanel.ClickNextButton();
            installationPage.InstallationPopupPanel.WaitForLightComeOnFormDisplayed();
            installationPage.InstallationPopupPanel.ClickNextButton();
            installationPage.InstallationPopupPanel.WaitForCommentFormDisplayed();
            installationPage.InstallationPopupPanel.ClickNextButton();
            installationPage.InstallationPopupPanel.WaitForFinishFormDisplayed();
            installationPage.InstallationPopupPanel.ClickFinishButton();
            installationPage.WaitForPreviousActionComplete();
            installationPage.Dialog.WaitForPopupMessageDisplayed();

            Step("35. Verify The error message displays");
            Step(" o Message: Invalid value 'the invalid value of Mac Address' for 'MacAddress'!");
            errorMessage = string.Format("Invalid value '{0}' for 'MacAddress'!", invalidMacAddress);
            VerifyEqual("35. Verify The error message displays", true, installationPage.Dialog.IsPopupMessageDisplayed());
            VerifyEqual("35. Verify message displays as expected", errorMessage, equipmentInventoryPage.Dialog.GetMessageText());

            Step("36. Press Cancel on the pop-up and select an existing streetlight 'ST-SC-90-01' in the list");
            equipmentInventoryPage.Dialog.ClickOkButton();
            installationPage.Dialog.WaitForPopupMessageDisappeared();
            installationPage.InstallationPopupPanel.ClickCancelButton();
            installationPage.WaitForPopupDialogDisappeared();
            installationPage.GeozoneTreeMainPanel.SelectNode(streetlight1);
            installationPage.WaitForPopupDialogDisplayed();

            Step("37. Verify the pop-up 'ST-SC-90-01' displays.");
            VerifyEqual("37. Verify The pop-up 'ST-SC-90-01' displays.", true, installationPage.IsPopupDialogDisplayed());
            VerifyEqual("37. Verify The pop-up 'ST-SC-90-01' displays.", streetlight1, installationPage.InstallationPopupPanel.GetPanelTitleText());

            Step("38. Press Next button, and input the invalid value for 'Unique address', then press Next until finishing");
            installationPage.InstallationPopupPanel.ClickNextButton();
            invalidMacAddress = SLVHelper.GenerateStringMixedNumber(12).ToUpper();

            installationPage.InstallationPopupPanel.WaitForScanQRCodeFormDisplayed();
            installationPage.InstallationPopupPanel.EnterUniqueAddressInput(invalidMacAddress);
            installationPage.InstallationPopupPanel.ClickNextButton();
            installationPage.InstallationPopupPanel.WaitForLightComeOnFormDisplayed();
            installationPage.InstallationPopupPanel.ClickNextButton();
            installationPage.InstallationPopupPanel.WaitForCommentFormDisplayed();
            installationPage.InstallationPopupPanel.ClickNextButton();
            installationPage.InstallationPopupPanel.WaitForFinishFormDisplayed();
            installationPage.InstallationPopupPanel.ClickFinishButton();
            installationPage.WaitForPreviousActionComplete();
            installationPage.Dialog.WaitForPopupMessageDisplayed();

            Step("39. Verify The error message displays");
            Step(" o Message: Invalid value 'the invalid value of Mac Address' for 'MacAddress'!");
            errorMessage = string.Format("Invalid value '{0}' for 'MacAddress'!", invalidMacAddress);
            VerifyEqual("39. Verify The error message displays", true, installationPage.Dialog.IsPopupMessageDisplayed());
            VerifyEqual("39. Verify message displays as expected", errorMessage, equipmentInventoryPage.Dialog.GetMessageText());

            try
            {
                Step("40. Delete test data after testcase is done.");
                DeleteGeozone(geozone);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("SC-509 Scheduling Manager - Better refreshing of Scheduling Manager Failures page")]
        public void SC_509()
        {
            var testData = GetTestDataOfSC_509();
            var controllerId = testData["ControllerId"];
            var controllerName = testData["ControllerName"];
            var geozone = SLVHelper.GenerateUniqueName("GZNSC509");
            var streetlight1 = SLVHelper.GenerateUniqueName("STL01");
            var streetlight2 = SLVHelper.GenerateUniqueName("STL02");
            var streetlight3 = SLVHelper.GenerateUniqueName("STL03");
            var streetlight4 = SLVHelper.GenerateUniqueName("STL04");
            var calendarName = SLVHelper.GenerateUniqueName("CSC509");
            var eventDateTime = Settings.GetServerTime().AddDays(-2).Date;

            Step("**** Precondition ****");
            Step(" - 1 Calendar named Calendar-SC-509 and 4 new Streetlights named: SL-SC-509-01, SL-SC-509-02, SL-SC-509-03, SL-SC-509-04");
            Step(" - Simulate the calendar comission failures for 3 streetlights [01, 02, 03] by the following commands");
            Step("  + valueName=calendarCommissionFailure");
            Step("  + value=true");
            Step("  + eventTime=<2 days before the current local date>");
            Step("**** Precondition ****\n");

            Step("-> Create data for testing");
            DeleteGeozones("GZNSC509*");
            CreateNewCalendar(calendarName);
            CreateNewGeozone(geozone);
            CreateNewDevice(DeviceType.Streetlight, streetlight1, controllerId, geozone);
            CreateNewDevice(DeviceType.Streetlight, streetlight2, controllerId, geozone);
            CreateNewDevice(DeviceType.Streetlight, streetlight3, controllerId, geozone);
            CreateNewDevice(DeviceType.Streetlight, streetlight4, controllerId, geozone);
            SetValueToDevice(controllerId, streetlight1, "DimmingGroupName", calendarName, Settings.GetCurrentControlerDateTime(controllerId).AddMinutes(10));
            SetValueToDevice(controllerId, streetlight2, "DimmingGroupName", calendarName, Settings.GetCurrentControlerDateTime(controllerId).AddMinutes(10));
            SetValueToDevice(controllerId, streetlight3, "DimmingGroupName", calendarName, Settings.GetCurrentControlerDateTime(controllerId).AddMinutes(10));
            SetValueToDevice(controllerId, streetlight4, "DimmingGroupName", calendarName, Settings.GetCurrentControlerDateTime(controllerId).AddMinutes(10));

            var status = SetValueToDevice(controllerId, streetlight1, "calendarCommissionFailure", true, eventDateTime);
            VerifyEqual(string.Format("Verify send request for streetlight {0} success", streetlight1), true, status);
            status = SetValueToDevice(controllerId, streetlight2, "calendarCommissionFailure", true, eventDateTime);
            VerifyEqual(string.Format("Verify send request for streetlight {0} success", streetlight2), true, status);
            status = SetValueToDevice(controllerId, streetlight3, "calendarCommissionFailure", true, eventDateTime);
            VerifyEqual(string.Format("Verify send request for streetlight {0} success", streetlight3), true, status);

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.SchedulingManager);            

            Step("1. Go to Scheduling Manager app");
            Step("2. Expected Scheduling Manager app is routed and loaded successfully");
            var schedulingManagerPage = desktopPage.GoToApp(App.SchedulingManager) as SchedulingManagerPage;

            Step("3. Select tab Failures, select the calendar name created in precondition.");
            schedulingManagerPage.SchedulingManagerPanel.SelectTab("Failures");
            schedulingManagerPage.WaitForPreviousActionComplete();
            schedulingManagerPage.SchedulingManagerPanel.TickAllCalendarFailuresCheckbox(false);
            schedulingManagerPage.GridPanel.WaitForDataReloaded();
            schedulingManagerPage.SchedulingManagerPanel.TickFailuresCalendarGridRecordCheckbox(calendarName, true);
            schedulingManagerPage.GridPanel.WaitForDataReloaded();
            
            Step("4. Expected 3 failures of streetlights simulated in precondition is listing in the list with");
            Step(" o Identifier: Streetlight name");
            Step(" o Calendar Name: Calendar-SC-509");
            Step(" o Failure Message: {\"user\":\"<username >\"}");
            Step(" o Failure Event Time: Datetime set in the parameter 'eventTime' of the command");
            Step(" o Commission In Progress: false");
            var dtGrid = schedulingManagerPage.GridPanel.BuildFailuresDataTable();
            VerifyEqual("4. Verify 3 failures of streetlights simulated in precondition is listing in the list", 3, dtGrid.Rows.Count);
            var failureMessageFormat = "{\"user\":\"" + Settings.Users["DefaultTest"].Username + "\"}";
            Step("** Verify streetlight {0}", streetlight1);
            var row = dtGrid.Select(string.Format("Identifier = '{0}'", streetlight1)).FirstOrDefault();
            if (row != null)
            {
                VerifyEqual(string.Format("[{0}] 4. Verify Identifier is {0}", streetlight1), streetlight1, row["Identifier"].ToString());
                VerifyEqual(string.Format("[{0}] 4. Verify Calendar Name is {1}", streetlight1, calendarName), calendarName, row["Calendar Name"].ToString());
                VerifyEqual(string.Format("[{0}] 4. Verify Failure Message is {1}", streetlight1, failureMessageFormat), failureMessageFormat, row["Failure Message"].ToString());
                VerifyEqual(string.Format("[{0}] 4. Verify Failure Event Time is {1}", streetlight1, eventDateTime), eventDateTime, DateTime.Parse(row["Failure Event Time"].ToString()).Date);
                VerifyEqual(string.Format("[{0}] 4. Verify Commission In Progress is false", streetlight1), "false", row["Commission In Progress"].ToString());
            }
            else
            {
                Warning(string.Format("There is no failure for {0}", streetlight1));
            }
            
            Step("** Verify streetlight {0}", streetlight2);
            row = dtGrid.Select(string.Format("Identifier = '{0}'", streetlight2)).FirstOrDefault();
            if (row != null)
            {
                VerifyEqual(string.Format("[{0}] 4. Verify Identifier is {0}", streetlight2), streetlight2, row["Identifier"].ToString());
                VerifyEqual(string.Format("[{0}] 4. Verify Calendar Name is {1}", streetlight2, calendarName), calendarName, row["Calendar Name"].ToString());
                VerifyEqual(string.Format("[{0}] 4. Verify Failure Message is {1}", streetlight2, failureMessageFormat), failureMessageFormat, row["Failure Message"].ToString());
                VerifyEqual(string.Format("[{0}] 4. Verify Failure Event Time is {1}", streetlight2, eventDateTime), eventDateTime, DateTime.Parse(row["Failure Event Time"].ToString()).Date);
                VerifyEqual(string.Format("[{0}] 4. Verify Commission In Progress is false", streetlight2), "false", row["Commission In Progress"].ToString());
            }
            else
            {
                Warning(string.Format("There is no failure for {0}", streetlight2));
            }

            Step("** Verify streetlight {0}", streetlight3);
            row = dtGrid.Select(string.Format("Identifier = '{0}'", streetlight3)).FirstOrDefault();
            if (row != null)
            {
                VerifyEqual(string.Format("[{0}] 4. Verify Identifier is {0}", streetlight3), streetlight3, row["Identifier"].ToString());
                VerifyEqual(string.Format("[{0}] 4. Verify Calendar Name is {1}", streetlight3, calendarName), calendarName, row["Calendar Name"].ToString());
                VerifyEqual(string.Format("[{0}] 4. Verify Failure Message is {1}", streetlight3, failureMessageFormat), failureMessageFormat, row["Failure Message"].ToString());
                VerifyEqual(string.Format("[{0}] 4. Verify Failure Event Time is {1}", streetlight3, eventDateTime), eventDateTime, DateTime.Parse(row["Failure Event Time"].ToString()).Date);
                VerifyEqual(string.Format("[{0}] 4. Verify Commission In Progress is false", streetlight3), "false", row["Commission In Progress"].ToString());
            }
            else
            {
                Warning(string.Format("There is no failure for {0}", streetlight3));
            }
            Step("5. Send a command to simulate the resolve failure of Streetlight SL-SC-509-01 with");
            Step(" o calendarCommissionFailure&value=false");
            Step(" o eventTime= the current local date");
            SetValueToDevice(controllerId, streetlight1, "calendarCommissionFailure", false, Settings.GetServerTime().Date);

            Step("6. Press Refresh button on the grid.");
            schedulingManagerPage.GridPanel.ClickReloadDataToolbarButton();
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("7. Expected result: The failure of SL-SC-509-01 is removed out of the list.");
            var listIdentifier = schedulingManagerPage.GridPanel.GetListOfColumnDataFailures("Identifier");
            VerifyEqual(string.Format("7. The failure of {0} is removed out of the list", streetlight1), false, listIdentifier.Contains(streetlight1));

            Step("8. Send a command to simulate the resolve failure of Streetlight SL-SC-509-02 with");
            Step(" o calendarCommissionFailure&value=false");
            Step(" o eventTime= the current local date");
            SetValueToDevice(controllerId, streetlight2, "calendarCommissionFailure", false, Settings.GetServerTime().Date);

            Step("9. Press Refresh button on the grid.");
            schedulingManagerPage.GridPanel.ClickReloadDataToolbarButton();
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("10. Expected result: The failure of SL-SC-509-02 is removed out of the list.");
            listIdentifier = schedulingManagerPage.GridPanel.GetListOfColumnDataFailures("Identifier");
            VerifyEqual(string.Format("10. The failure of {0} is removed out of the list", streetlight2), false, listIdentifier.Contains(streetlight2));

            Step("11. Send a command to simulate the failure of Streetlight SL-SC-509-04 with");
            Step(" o calendarCommissionFailure&value=true");
            Step(" o eventTime= 2 days before the current date");
            SetValueToDevice(controllerId, streetlight4, "calendarCommissionFailure", true, eventDateTime);

            Step("12. Press Refresh button on the grid");
            schedulingManagerPage.GridPanel.ClickReloadDataToolbarButton();
            schedulingManagerPage.WaitForPreviousActionComplete();
            dtGrid = schedulingManagerPage.GridPanel.BuildFailuresDataTable();      
            Step("13. Expected A new line of failure of Streetlight SL-SC-509-04 is added in the list with");
            Step(" o Identifier: Streetlight name");
            Step(" o Calendar Name: Calendar-SC-509");
            Step(" o Failure Message: {\"user\":\"<username >\"}");
            Step(" o Failure Event Time: Datetime set in the parameter 'eventTime' of the command");
            Step(" o Commission In Progress: false");
            row = dtGrid.Select(string.Format("Identifier = '{0}'", streetlight4)).FirstOrDefault();
            if(row != null)
            {
                VerifyEqual(string.Format("[{0}] 13. Verify Identifier is {0}", streetlight4), streetlight4, row["Identifier"].ToString());
                VerifyEqual(string.Format("[{0}] 13. Verify Calendar Name is {1}", streetlight4, calendarName), calendarName, row["Calendar Name"].ToString());
                VerifyEqual(string.Format("[{0}] 13. Verify Failure Message is {1}", streetlight4, failureMessageFormat), failureMessageFormat, row["Failure Message"].ToString());
                VerifyEqual(string.Format("[{0}] 13. Verify Failure Event Time is {1}", streetlight4, eventDateTime), eventDateTime, DateTime.Parse(row["Failure Event Time"].ToString()).Date);
                VerifyEqual(string.Format("[{0}] 13. Verify Commission In Progress is false", streetlight4), "false", row["Commission In Progress"].ToString());
            }
            else
            {
                Warning(string.Format("There is no failure for {0}", streetlight4));
            }
            var failuresDevicesCount = schedulingManagerPage.SchedulingManagerPanel.GetFailuresDevicesCount();
            
            Step("14. Send a command to simulate the resolve of failure for SL-SC-509-03");
            Step(" o calendarCommissionFailure&value=false");
            Step(" o eventTime= the current date");
            SetValueToDevice(controllerId, streetlight3, "calendarCommissionFailure", false, Settings.GetServerTime().Date);
                
            Step("15. Select Setting icon on the top right connect and choose Log Out option");    
            Step("16. Log in CMS again.");
            desktopPage = SLVHelper.LogoutAndLogin(schedulingManagerPage, Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);

            Step("17. Expected Login successfully and in the Scheduling Manager, the number of failure is diplayed");
            Step(" o The number of failures - 1");
            var failuresAppcount = desktopPage.GetSchedulingManagerFailuresCount();
            VerifyEqual("18. Verify The number of the failures is dislayed be equal to total failures of calendars in Failure tab.", failuresDevicesCount - 1, failuresAppcount);

            Step("19. Go to Scheduling Manager app, and select Failures tab");
            schedulingManagerPage = desktopPage.GoToApp(App.SchedulingManager) as SchedulingManagerPage;
            schedulingManagerPage.SchedulingManagerPanel.SelectTab("Failures");
            schedulingManagerPage.WaitForPreviousActionComplete();
            schedulingManagerPage.SchedulingManagerPanel.TickAllCalendarFailuresCheckbox(false);
            schedulingManagerPage.GridPanel.WaitForDataReloaded();
            schedulingManagerPage.SchedulingManagerPanel.TickFailuresCalendarGridRecordCheckbox(calendarName, true);
            schedulingManagerPage.GridPanel.WaitForDataReloaded();

            Step("20. Expected Scheduling Manager app is routed and loaded successfully");
            Step(" o The failure of SL-SC-509-03 is removed out of the list");
            listIdentifier = schedulingManagerPage.GridPanel.GetListOfColumnDataFailures("Identifier");
            VerifyEqual(string.Format("21. The failure of {0} is removed out of the list", streetlight3), false, listIdentifier.Contains(streetlight2));

            Step("21. Send a command to simulate the resolve of failure for the last Streetlight SL-SC-509-04");
            Step(" o calendarCommissionFailure&value=false");
            Step(" o eventTime= the current local date");
            SetValueToDevice(controllerId, streetlight4, "calendarCommissionFailure", false, Settings.GetServerTime().Date);

            Step("22. Refresh page and Go to Scheduling Manager app, and select Failures tab");
            desktopPage = Browser.RefreshLoggedInCMS();
            schedulingManagerPage = desktopPage.GoToApp(App.SchedulingManager) as SchedulingManagerPage;
            schedulingManagerPage.SchedulingManagerPanel.SelectTab("Failures");
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("23. Expected The calendar is removed out of the list");  
            var listFailuresCalendarName = schedulingManagerPage.SchedulingManagerPanel.GetListOfFailuresCalendarName();
            VerifyEqual(string.Format("23. The calendar of {0} is removed out of the list", calendarName), false, listFailuresCalendarName.Contains(calendarName));
            
            try
            {
                Step("24. Delete test data after testcase is done.");
                DeleteGeozone(geozone);
                DeleteCalendar(calendarName);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("SC-98 Persist the geozone filter option in Failure Tracking across logins and application changes")]
        public void SC_98()
        {
            var testData = GetTestDataOfSC_98();
            var geozone = testData["Geozone"].ToString();
            var streetlights = testData["Streetlights"] as List<DeviceModel>;
            var streetlight = streetlights.Select(p => p.Name).PickRandom();

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.FailureTracking);

            Step("1. Go to Failure Tracking app");
            Step("2. Expected Failure Tracking page is routed and loaded successfully");
            var failureTrackingPage = desktopPage.GoToApp(App.FailureTracking) as FailureTrackingPage;
            
            Step("3. Hover the Global icon on the top right corner of the map");
            Step("4. Verify There is the Global icon.");
            VerifyEqual("4.  Verify There is the Global icon", true, failureTrackingPage.Map.IsGlobalEarthIconDisplayed());

            Step("5. Check 'Filter on the activated geozone on the map' option");
            failureTrackingPage.Map.TickFilterGeozoneCheckbox();           

            Step("6. Verify The option is selected");
            VerifyEqual("6. Verify Filtering option is selected", true, failureTrackingPage.Map.IsFilterGeozoneCheckboxChecked());

            Step("7. Log out and log in again");
            desktopPage = SLVHelper.LogoutAndLogin(failureTrackingPage, Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);

            Step("8. Go to Failure Tracking app");
            Step("9. Verify Failure Tracking page is routed and loaded successfully");
            failureTrackingPage = desktopPage.GoToApp(App.FailureTracking) as FailureTrackingPage;

            Step("10. Hover the Global icon on the top right corner of the map");
            failureTrackingPage.Map.MoveToGlobalEarthIcon();

            Step("11. Verify 'Filter on the activated geozone on the map' option is still checked.");
            VerifyEqual("11. Verify 'Filter on the activated geozone on the map' option is still checked.", true, failureTrackingPage.Map.IsFilterGeozoneCheckboxChecked());

            Step("12. Select Application Switcher and choose randomly an application.");
            Step("13. Verify The application is routed and loaded successfully");
            var userPage = failureTrackingPage.AppBar.SwitchTo(App.Users) as UsersPage;            

            Step("14. Go back to Failure Tracking app");
            Step("15. Verify Failure Tracking page is routed and loaded successfully");
            Warning("[SC-830] Failure Tracking - Loading bar never does not disappear in certain cases");
            failureTrackingPage = userPage.AppBar.SwitchTo(App.FailureTracking) as FailureTrackingPage;
            
            Step("16. Hover the Global icon on the top right corner of the map");
            failureTrackingPage.Map.MoveToGlobalEarthIcon();

            Step("17. Verify 'Filter on the activated geozone on the map' option is still checked.");
            VerifyEqual("17. Verify 'Filter on the activated geozone on the map' option is still checked.", true, failureTrackingPage.Map.IsFilterGeozoneCheckboxChecked());

            Step("18. Select Geozone: Real Time Control Area, then select a device");
            failureTrackingPage.GeozoneTreeMainPanel.SelectNode(geozone + @"\" + streetlight);
            failureTrackingPage.WaitForDetailsPanelDisplayed();

            Step("19. Verify Failure Tracking panel of the device is displayed with");
            Step(" o Tittle of Panel: Failure Tracking");
            Step(" o Detail: Name of device");
            VerifyEqual("19. Verify Tittle of Panel: Failure Tracking", "Failure Tracking", failureTrackingPage.FailureTrackingDetailsPanel.GetPanelTitleText());
            VerifyEqual("19. Verify Detail: Name of device", streetlight, failureTrackingPage.FailureTrackingDetailsPanel.GetDeviceNameValueText());

            Step("20. Close the Failure Tracking panel");
            failureTrackingPage.FailureTrackingDetailsPanel.ClickCloseButton();
            failureTrackingPage.WaitForDetailsPanelDisappeared();

            Step("21. Hover the Global icon on the top right corner of the map");
            failureTrackingPage.Map.MoveToGlobalEarthIcon();

            Step("22. Verify 'Filter on the activated geozone on the map' option is still checked.");
            VerifyEqual("22. Verify 'Filter on the activated geozone on the map' option is still checked.", true, failureTrackingPage.Map.IsFilterGeozoneCheckboxChecked());

            Step("23. Deselect 'Filter on the activated geozone on the map' option");
            failureTrackingPage.Map.TickFilterGeozoneCheckbox(false);

            Step("24. Verify 'Filter on the activated geozone on the map' option is unchecked");
            VerifyEqual("24. Verify 'Filter on the activated geozone on the map' option is unchecked", false, failureTrackingPage.Map.IsFilterGeozoneCheckboxChecked());

            Step("25. Press Dashboard button, then select randomly an application");
            failureTrackingPage.AppBar.ClickApplicationsButton();

            Step("26. Verify The application is routed and loaded successfully");
            var randomPage = desktopPage.GoToRandomApp();

            Step("27. Press Dashboard button, then select Failure Tracking app");
            randomPage.AppBar.ClickApplicationsButton();
            failureTrackingPage = desktopPage.GoToApp(App.FailureTracking) as FailureTrackingPage;

            Step("28. Hover the Global icon on the top right corner of the map");
            failureTrackingPage.Map.MoveToGlobalEarthIcon();

            Step("29. Verify 'Filter on the activated geozone on the map' option is still unchecked.");
            VerifyEqual("29. Verify 'Filter on the activated geozone on the map' option is still unchecked", false, failureTrackingPage.Map.IsFilterGeozoneCheckboxChecked());

            Step("30. Log out and log in again");
            desktopPage = SLVHelper.LogoutAndLogin(failureTrackingPage, Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);

            Step("31. Go to Failure Tracking app");
            Step("32. Verify Failure Tracking page is routed and loaded successfully");
            failureTrackingPage = desktopPage.GoToApp(App.FailureTracking) as FailureTrackingPage;

            Step("33. Hover the Global icon on the top right corner of the map");
            failureTrackingPage.Map.MoveToGlobalEarthIcon();

            Step("34. Verify 'Filter on the activated geozone on the map' option is still unchecked.");
            VerifyEqual("34. Verify 'Filter on the activated geozone on the map' option is still unchecked", false, failureTrackingPage.Map.IsFilterGeozoneCheckboxChecked());
        }

        [Test, DynamicRetry]
        [Description("SC-116 Know if and when a device has moved to a new location")]
        [Category("RunAlone")]
        public void SC_116()
        {
            var testData = GetTestDataOfSC_116();
            var controllerId = testData["ControllerId"];
            var controllerName = testData["ControllerName"];
            var geozone = SLVHelper.GenerateUniqueName("GZNSC116");
            var streetlight = SLVHelper.GenerateUniqueName("STL");            
            var fullGeozonePath = Settings.RootGeozoneName + @"/" + geozone;
            var csvFilePath = Settings.GetFullPath(Settings.CSV_FILE_PATH + "SC-116-UpdateLocation.csv");
            var typeOfEquipment = "ABEL-Vigilon A[Dimmable ballast]";

            var latitude = SLVHelper.GenerateLatitude();
            var longitude = SLVHelper.GenerateLongitude();
            var originalLatitude = decimal.Parse(latitude);
            var originalLongitude = decimal.Parse(longitude);
            var notedLatitude = Math.Round(originalLatitude, 3);
            var notedLongitude = Math.Round(originalLongitude, 3);

            var newLatitude = decimal.Parse(SLVHelper.GenerateLatitude());
            var newLongitude = decimal.Parse(SLVHelper.GenerateLongitude());
            var newLatitudeRound = Math.Round(newLatitude, 3);
            var newLongitudeRound = Math.Round(newLongitude, 3);            
          
            Step("**** Precondition ****");
            Step(" - Create a new streetlight");
            Step(" - Prepare a csv file to change location of a device.");
            Step("**** Precondition ****\n");

            Step("-> Create data for testing");
            DeleteGeozones("GZNSC116*");
            CreateCsv(DeviceType.Streetlight, csvFilePath, fullGeozonePath, controllerId, streetlight, typeOfEquipment, newLatitude.ToString(), newLongitude.ToString());
            CreateNewGeozone(geozone);
            CreateNewDevice(DeviceType.Streetlight, streetlight, controllerId, geozone, lat: latitude, lng: longitude);            

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.EquipmentInventory, App.DeviceHistory);
            
            Step("1. Go to Device History app");
            Step("2. Expected Device History app is routed and loaded successfully");
            var deviceHistoryPage = desktopPage.GoToApp(App.DeviceHistory) as DeviceHistoryPage;
            deviceHistoryPage.GeozoneTreeMainPanel.SelectNode(geozone);

            Step("3. Press Show and Hide Column button and make sure Latitude and Longitude are selected");
            var listAttributes = new string[] { "Latitude", "Longitude" };
            deviceHistoryPage.GridPanel.UncheckAllColumnsInShowHideColumnsMenu();
            deviceHistoryPage.GridPanel.CheckColumnsInShowHideColumnsMenu(listAttributes);

            Step("4. Press Show and Hide Column button again and press Refresh button");
            deviceHistoryPage.GridPanel.ClickReloadDataToolbarButton();
            deviceHistoryPage.WaitForPreviousActionComplete();
            deviceHistoryPage.GridPanel.WaitForDataReloaded();

            Step("5. Select the new streetlight in the grid");
            deviceHistoryPage.GridPanel.ClickGridRecord(streetlight);
            deviceHistoryPage.WaitForPreviousActionComplete();
            deviceHistoryPage.WaitForDeviceHistoryPanelDisplayed();

            Step("6. Expected Latitude and Longitude values equal to their values when creating a streetlight (rounded with 3 numbers behind the dot)");
            var row = deviceHistoryPage.GridPanel.GetGridRecordDataRowEquals("Device", streetlight);
            if (row != null)
            {
                var actualLongitude = decimal.Parse(row["Longitude"].ToString());
                var actualLatitude = decimal.Parse(row["Latitude"].ToString());
                VerifyEqual(string.Format("6. Verify Longitude value equal to their values when creating a streetlight (original: {0})", originalLongitude), notedLongitude, actualLongitude);
                VerifyEqual(string.Format("6. Verify Latitude value equal to their values when creating a streetlight(original: {0})", originalLatitude), notedLatitude, actualLatitude);
            }
            else
            {
                Warning(string.Format("6. There is no row with streetlight {0}", streetlight));
            }

            Step("7. Press Attributes filter button and make sure Latitude and Longitude are selected. Press Refresh button on the Device History panel of the streetlight");            
            deviceHistoryPage.DeviceHistoryPanel.ClickFilterToolbarButton();
            deviceHistoryPage.DeviceHistoryPanel.WaitForAttributesFilterPanelDisplayed();            
            deviceHistoryPage.DeviceHistoryPanel.CheckFilterAttributes(listAttributes);            
            deviceHistoryPage.DeviceHistoryPanel.ClickFilterToolbarButton();
            deviceHistoryPage.DeviceHistoryPanel.WaitForAttributesFilterPanelDisappeared();
            deviceHistoryPage.DeviceHistoryPanel.ClickRefreshToolbarButton();
            deviceHistoryPage.DeviceHistoryPanel.WaitForLoaderSpinDisappeared();   

            Step("8. Expected 2 records of Latitude and Longitude are displayed with values equal to their values when creating a streetlight (rounded with 3 numbers behind the dot)");
            Step("o Note: Failed by SC-776-Issue related to rounded value of Latitude");
            var dt = deviceHistoryPage.DeviceHistoryPanel.BuildDataTableFromGrid();
            var longitudeRow = dt.Select("Name = 'Longitude'").FirstOrDefault();
            if (longitudeRow != null)
            {
                var actualLongitude = decimal.Parse(longitudeRow["Value"].ToString());
                VerifyEqual("[SC-776] 8. Verify Longitude value equal to their values when creating a streetlight", notedLongitude, actualLongitude);
            }
            else
            {
                Warning("8. There is no row with Longitude attribute");
            }
            var latitudeRow = dt.Select("Name = 'Latitude'").FirstOrDefault();
            if (latitudeRow != null)
            {
                var actualLatitude = decimal.Parse(latitudeRow["Value"].ToString());
                VerifyEqual("[SC-776] 8. Verify Latitude value equal to their values when creating a streetlight", notedLatitude, actualLatitude);
            }
            else
            {
                Warning("8. There is no row with Latitude attribute");
            }
            
            Step("9. Using API to import the csv file in precondition, then refresh the page and go to Device History, select the testing geozone > streetlight and press Refresh button in the grid");
            ImportFile(csvFilePath);
            desktopPage = Browser.RefreshLoggedInCMS();
            deviceHistoryPage = desktopPage.GoToApp(App.DeviceHistory) as DeviceHistoryPage;
            deviceHistoryPage.GeozoneTreeMainPanel.SelectNode(geozone + @"\" + streetlight);
            deviceHistoryPage.WaitForDeviceHistoryPanelDisplayed();
            deviceHistoryPage.DeviceHistoryPanel.ClickFilterToolbarButton();
            deviceHistoryPage.DeviceHistoryPanel.WaitForAttributesFilterPanelDisplayed();
            deviceHistoryPage.DeviceHistoryPanel.CheckFilterAttributes(listAttributes);
            deviceHistoryPage.DeviceHistoryPanel.ClickFilterToolbarButton();
            deviceHistoryPage.DeviceHistoryPanel.WaitForAttributesFilterPanelDisappeared();
            deviceHistoryPage.DeviceHistoryPanel.ClickRefreshToolbarButton();
            deviceHistoryPage.DeviceHistoryPanel.WaitForLoaderSpinDisappeared();

            Step("10. Expected 2 new records of Latitude and Longitude are displayed with values equal to their newly inputted values.");
            dt = deviceHistoryPage.DeviceHistoryPanel.BuildDataTableFromGrid();
            longitudeRow = dt.Select("Name = 'Longitude'").FirstOrDefault();
            if (longitudeRow != null)
            {
                var actualLongitude = decimal.Parse(longitudeRow["Value"].ToString());
                VerifyEqual("[SC-776] 10. Verify Longitude value equal to their values when creating a streetlight", newLongitudeRound, actualLongitude);
            }
            else
            {
                Warning("10. There is no row with Longitude attribute");
            }
            latitudeRow = dt.Select("Name = 'Latitude'").FirstOrDefault();
            if (latitudeRow != null)
            {
                var actualLatitude = decimal.Parse(latitudeRow["Value"].ToString());
                VerifyEqual("[SC-776] 10. Verify Latitude value equal to their values when creating a streetlight", newLatitude, actualLatitude);
            }
            else
            {
                Warning("10. There is no row with Latitude attribute");
            }            
           
            try
            {
                Step("11. Clear the data after test is done");
                DeleteGeozone(geozone);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("SC-20-01 Scheduling Manager - Failures tab - Ability to retry commissioning a calendar to a specific device or set of devices")]
        public void SC_20_01()
        {
            var testData = GetTestDataOfSC_20_01();           
            var controllerId = testData["ControllerId"];
            var controllerName = testData["ControllerName"];
            var geozone = SLVHelper.GenerateUniqueName("GZNSC2001");
            var streetlight1 = SLVHelper.GenerateUniqueName("STL01");
            var streetlight2 = SLVHelper.GenerateUniqueName("STL02");
            var streetlight3 = SLVHelper.GenerateUniqueName("STL03");
            var calendarName = SLVHelper.GenerateUniqueName("CSC2001");
            var eventDateTime = Settings.GetServerTime().Date;
            var comment1 = SLVHelper.GenerateString();
            var comment2 = SLVHelper.GenerateString();
            var comment3 = SLVHelper.GenerateString();
            var streetlights = new List<string> { streetlight1, streetlight2, streetlight3 };

            Step("**** Precondition ****");
            Step(" - Create 3 new streetlights and send the commands to simulate the Calendar Commissioning Failure for them.");
            Step("  + valueName=calendarCommissionFailure&value=true");
            Step("  + comment=the random comment");
            Step("  + eventTime= the current datetime");
            Step("**** Precondition ****\n");

            Step("-> Create data for testing");
            DeleteGeozones("GZNSC2001*");
            CreateNewCalendar(calendarName);
            CreateNewGeozone(geozone);
            CreateNewDevice(DeviceType.Streetlight, streetlight1, controllerId, geozone);
            CreateNewDevice(DeviceType.Streetlight, streetlight2, controllerId, geozone);
            CreateNewDevice(DeviceType.Streetlight, streetlight3, controllerId, geozone);
            SetValueToDevice(controllerId, streetlight1, "DimmingGroupName", calendarName, Settings.GetCurrentControlerDateTime(controllerId).AddMinutes(10));
            SetValueToDevice(controllerId, streetlight2, "DimmingGroupName", calendarName, Settings.GetCurrentControlerDateTime(controllerId).AddMinutes(10));
            SetValueToDevice(controllerId, streetlight3, "DimmingGroupName", calendarName, Settings.GetCurrentControlerDateTime(controllerId).AddMinutes(10));

            var status = SetValueToDevice(controllerId, streetlight1, "calendarCommissionFailure", true, eventDateTime, comment1);
            VerifyEqual(string.Format("Verify send request for streetlight {0} success", streetlight1), true, status);
            status = SetValueToDevice(controllerId, streetlight2, "calendarCommissionFailure", true, eventDateTime, comment2);
            VerifyEqual(string.Format("Verify send request for streetlight {0} success", streetlight2), true, status);
            status = SetValueToDevice(controllerId, streetlight3, "calendarCommissionFailure", true, eventDateTime, comment3);
            VerifyEqual(string.Format("Verify send request for streetlight {0} success", streetlight3), true, status);

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.SchedulingManager);

            Step("1. Go to Scheduling Manager app");
            var schedulingManagerPage = desktopPage.GoToApp(App.SchedulingManager) as SchedulingManagerPage;           

            Step("2. Select the Failure tab");
            schedulingManagerPage.SchedulingManagerPanel.SelectTab("Failures");
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("3. Check the checkbox next to Identifier of a streetlight, and press Commission button");
            schedulingManagerPage.SchedulingManagerPanel.TickAllCalendarFailuresCheckbox(false);
            schedulingManagerPage.GridPanel.WaitForLeftFooterTextDisplayed();
            schedulingManagerPage.SchedulingManagerPanel.TickFailuresCalendarGridRecordCheckbox(calendarName, true);
            schedulingManagerPage.GridPanel.WaitForLeftFooterTextDisplayed();
            schedulingManagerPage.GridPanel.TickFailuresGridRecordCheckbox(streetlight2, true);
            schedulingManagerPage.GridPanel.ClickCommissionToolbarButton();
            schedulingManagerPage.WaitForPopupDialogDisplayed();

            Step("4. Verify A pop-up displays with 3 columns:");
            Step(" - Device Name: the name of device");
            Step(" - Calendar Name: the name of calendar");
            Step(" - Progress: emtpy");
            Step(" - 2 buttons: Close and Commission");
            var commisioningStreetlights = schedulingManagerPage.CommissionPopupPanel.GetListOfColumnData("Device Name");
            var commisioningCalendar = schedulingManagerPage.CommissionPopupPanel.GetListOfColumnData("Calendar Name");
            var commisioningProgress = schedulingManagerPage.CommissionPopupPanel.GetListOfProgressIcon();
            VerifyTrue("4. Verify Device Name: the name of device", commisioningStreetlights.Count == 1 && commisioningStreetlights.FirstOrDefault().Equals(streetlight2), streetlight2, commisioningStreetlights.FirstOrDefault().Equals(streetlight2));
            VerifyTrue("4. Verify Calendar Name: the name of calendar", commisioningCalendar.Count == 1 && commisioningCalendar.FirstOrDefault().Equals(calendarName), calendarName, commisioningCalendar.FirstOrDefault().Equals(calendarName));
            VerifyTrue("4. Verify Progress: emtpy", commisioningProgress.Count == 1 && string.IsNullOrEmpty(commisioningProgress.FirstOrDefault()), "", commisioningProgress.FirstOrDefault());
            VerifyEqual("4. Verify Close is displayed", true, schedulingManagerPage.CommissionPopupPanel.IsCancelButtonDisplayed());
            VerifyEqual("4. Verify Commission is displayed", true, schedulingManagerPage.CommissionPopupPanel.IsCommissionButtonDisplayed());

            Step("5. Press Close button");
            schedulingManagerPage.CommissionPopupPanel.ClickCancelButton();
            schedulingManagerPage.WaitForPopupDialogDisappeared();

            Step("6. Verify The pop-up is closed");
            VerifyEqual("6. Verify The pop-up is closed", false, schedulingManagerPage.IsPopupDialogDisplayed());

            Step("7. Press Commission button again, then press Commission on the pop-up");
            schedulingManagerPage.GridPanel.ClickCommissionToolbarButton();
            schedulingManagerPage.WaitForPopupDialogDisplayed();
            schedulingManagerPage.CommissionPopupPanel.ClickCommissionButton();
            schedulingManagerPage.Dialog.WaitForPopupMessageDisplayed();

            Step("8. Verify A message pop-up displays 'Devices successfully submitted for commissioning.'");
            VerifyEqual("8. Verify message 'Devices successfully submitted for commissioning.' is shown", "Devices successfully submitted for commissioning.", schedulingManagerPage.Dialog.GetMessageText());

            Step("9. Press OK button");
            schedulingManagerPage.Dialog.ClickOkButton();
            schedulingManagerPage.Dialog.WaitForPopupMessageDisappeared();

            Step("10. Verify The message pop-up is closed and the Commission pop-up is updated");
            Step(" - Progress: Plane icon");
            var iconUrl = schedulingManagerPage.CommissionPopupPanel.GetProgressIcon(streetlight2);
            VerifyEqual("10. Verify Progress: Plane icon", "Plane", iconUrl);

            Step("11. Press '>' icon");
            schedulingManagerPage.CommissionPopupPanel.ClickExpandIconNextDevice(streetlight2);

            Step("12. Verify The messages displays in 4 messages");
            Step(" - Inventory has been checked in the database and is consistent");
            Step(" - Neither controller nor devices checked. No device pushed.");
            Step(" - All messages and configuration data have been sent and accepted by the controller");
            Step(" - Commissioning process finished.");
            var expectedMessageList = new List<string>
            {
                "Inventory has been checked in the database and is consistent",
                "Neither controller nor devices checked. No device pushed.",
                "All messages and configuration data have been sent and accepted by the controller",
                "Commissioning process finished.",
            };            
            var actualMessageList = schedulingManagerPage.CommissionPopupPanel.GetListOfExpandInfomation();
            VerifyEqual("12. Verify The messages displays in 4 messages as expected", expectedMessageList, actualMessageList);
            
            Step("13. Verify Each message has a GREEN 'I' icon");
            var actualMessageIconList = schedulingManagerPage.CommissionPopupPanel.GetListOfExpandInfomationIcon();
            VerifyEqual("13. Verify Each message has a GREEN 'I' icon", true, actualMessageIconList.All(p => p.Equals("Info")));

            Step("14. Press Close button");
            schedulingManagerPage.CommissionPopupPanel.ClickCancelButton();
            schedulingManagerPage.WaitForPopupDialogDisappeared();

            Step("15. Verify The Commission pop-up is closed");
            VerifyEqual("15. Verify The pop-up is closed", false, schedulingManagerPage.IsPopupDialogDisplayed());

            Step("16. Check the checkbox next to Identifier of the 2 other streetlights, and press Commission button");
            schedulingManagerPage.GridPanel.TickFailuresGridRecordCheckbox(streetlight1, true);
            schedulingManagerPage.GridPanel.TickFailuresGridRecordCheckbox(streetlight3, true);
            schedulingManagerPage.GridPanel.ClickCommissionToolbarButton();
            schedulingManagerPage.WaitForPopupDialogDisplayed();

            Step("17. Verify A pop-up displays information of 2 streetlights with 3 columns:");
            Step(" - Device Name: the name of device");
            Step(" - Calendar Name: the name of calendar");
            Step(" - Process: emtpy");
            Step(" - 2 buttons: Close and Commission");
            commisioningStreetlights = schedulingManagerPage.CommissionPopupPanel.GetListOfColumnData("Device Name");
            commisioningCalendar = schedulingManagerPage.CommissionPopupPanel.GetListOfColumnData("Calendar Name");
            commisioningProgress = schedulingManagerPage.CommissionPopupPanel.GetListOfProgressIcon();
            VerifyEqual("17. Verify Device Name: the name of device", new List<string> { streetlight1, streetlight3 }, commisioningStreetlights, false);
            VerifyTrue("17. Verify Calendar Name: the name of calendar", commisioningCalendar.All(p => p.Equals(calendarName)), calendarName, string.Join(",", commisioningCalendar));
            VerifyTrue("17. Verify Progress: emtpy", commisioningProgress.All(p => string.IsNullOrEmpty(p)), "", string.Join(",", commisioningProgress));
            VerifyEqual("17. Verify Close is displayed", true, schedulingManagerPage.CommissionPopupPanel.IsCancelButtonDisplayed());
            VerifyEqual("17. Verify Commission is displayed", true, schedulingManagerPage.CommissionPopupPanel.IsCommissionButtonDisplayed());
            
            Step("18. Press Commission on the pop-up");
            schedulingManagerPage.CommissionPopupPanel.ClickCommissionButton();
            schedulingManagerPage.Dialog.WaitForPopupMessageDisplayed();

            Step("19. Verify A message pop-up displays 'Devices successfully submitted for commissioning.'");
            VerifyEqual("19. Verify message 'Devices successfully submitted for commissioning.' is shown", "Devices successfully submitted for commissioning.", schedulingManagerPage.Dialog.GetMessageText());

            Step("20. Press OK button");
            schedulingManagerPage.Dialog.ClickOkButton();
            schedulingManagerPage.Dialog.WaitForPopupMessageDisappeared();
            
            Step("21. Verify The message pop-up is closed and 2 rows of the Commission pop-up are updated");
            Step(" - Progress: Plane icon");
            var progressIcon = schedulingManagerPage.CommissionPopupPanel.GetListOfProgressIcon();
            VerifyEqual("21. Verify Progress: Plane icon", true, progressIcon.All(p => p.Equals("Plane")));

            Step("22. Press '>' icon of one of a row");
            schedulingManagerPage.CommissionPopupPanel.ClickExpandIconNextDevice(streetlight3);

            Step("23. Verify The messages displays in 4 messages");
            Step(" - Inventory has been checked in the database and is consistent");
            Step(" - Neither controller nor devices checked. No device pushed.");
            Step(" - All messages and configuration data have been sent and accepted by the controller");
            Step(" - Commissioning process finished.");
            actualMessageList = schedulingManagerPage.CommissionPopupPanel.GetListOfExpandInfomation();
            VerifyEqual("23. Verify The messages displays in 4 messages as expected", expectedMessageList, actualMessageList);
            
            Step("24. Verify Each message has a GREEN 'I' icon");
            actualMessageIconList = schedulingManagerPage.CommissionPopupPanel.GetListOfExpandInfomationIcon();
            VerifyEqual("24. Verify Each message has a GREEN 'I' icon", true, actualMessageIconList.All(p => p.Equals("Info")));

            Step("25. Press Close button");
            schedulingManagerPage.CommissionPopupPanel.ClickCancelButton();
            schedulingManagerPage.WaitForPopupDialogDisappeared();

            Step("26. Verify The Commission pop-up is closed");
            VerifyEqual("26. Verify The pop-up is closed", false, schedulingManagerPage.IsPopupDialogDisplayed());
            
            try
            {
                Step("27. Delete test data");
                DeleteGeozone(geozone);
                DeleteCalendar(calendarName);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("SC-20-02 Scheduling Manager - Failures tab - The UI")]
        public void SC_20_02()
        {
            var testData = GetTestDataOfSC_20_02();          
            var controllerId = testData["ControllerId"];
            var controllerName = testData["ControllerName"];
            var geozone = SLVHelper.GenerateUniqueName("GZNSC2002");
            var streetlight1 = SLVHelper.GenerateUniqueName("STL01");
            var streetlight2 = SLVHelper.GenerateUniqueName("STL02");
            var calendarName = SLVHelper.GenerateUniqueName("CSC2002");
            var eventDateTime = Settings.GetServerTime().Date;
            var comment1 = SLVHelper.GenerateString();
            var comment2 = SLVHelper.GenerateString();

            Step("**** Precondition ****");
            Step(" - Create 2 new streetlights and send the commands to simulate the Calendar Commissioning Failure for them.");
            Step("  + valueName=calendarCommissionFailure&value=true");
            Step("  + comment=the random comment");
            Step("  + eventTime= the current datetime");
            Step("**** Precondition ****\n");

            Step("-> Create data for testing");
            DeleteGeozones("GZNSC2002*");
            CreateNewCalendar(calendarName);
            CreateNewGeozone(geozone);
            CreateNewDevice(DeviceType.Streetlight, streetlight1, controllerId, geozone);
            CreateNewDevice(DeviceType.Streetlight, streetlight2, controllerId, geozone);     
            SetValueToDevice(controllerId, streetlight1, "DimmingGroupName", calendarName, Settings.GetCurrentControlerDateTime(controllerId).AddMinutes(10));
            SetValueToDevice(controllerId, streetlight2, "DimmingGroupName", calendarName, Settings.GetCurrentControlerDateTime(controllerId).AddMinutes(10));

            var status = SetValueToDevice(controllerId, streetlight1, "calendarCommissionFailure", true, eventDateTime, comment1);
            VerifyEqual(string.Format("Verify send request for streetlight {0} success", streetlight1), true, status);
            status = SetValueToDevice(controllerId, streetlight2, "calendarCommissionFailure", true, eventDateTime, comment2);
            VerifyEqual(string.Format("Verify send request for streetlight {0} success", streetlight2), true, status);
            
            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.SchedulingManager);            

            Step("1. Go to Scheduling Manager app");
            var schedulingManagerPage = desktopPage.GoToApp(App.SchedulingManager) as SchedulingManagerPage;

            Step("2. Select the Failure tab");
            schedulingManagerPage.SchedulingManagerPanel.SelectTab("Failures");
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("3. Verify The Failure displays with All Field search textbox, Search Icon and the 4 columns: 1 column containing Checkbox, Calendar Name, Devices, Geozone.");
            Step(" o Calendar Name: dimming group of streetlight");
            Step(" o Devices: number of devices using this calendar");
            Step(" o Geozone: Root geozone of the current user");
            var expectedColumns= new List < string > { "Calendar Name", "Devices", "GeoZone" };
            VerifyEqual("3. The Failure displays All Field search textbox", true, schedulingManagerPage.SchedulingManagerPanel.IsFailuresAllFieldSearchTextboxDisplayed());
            VerifyEqual("3. The Failure displays Search Icon button", true, schedulingManagerPage.SchedulingManagerPanel.IsFailuresSearchButtonDisplayed());
            VerifyEqual("3. The Failure displays 1 column containing Checkbox", true, schedulingManagerPage.SchedulingManagerPanel.IsFailuresGridHasColumnCheckbox());
            VerifyEqual("3. The Failure displays 3 columns: Calendar Name, Devices, Geozone", expectedColumns, schedulingManagerPage.SchedulingManagerPanel.GetListOfColumnsHeaderFailures());            
            var dtGrid = schedulingManagerPage.SchedulingManagerPanel.BuildDataTableFromFailuresGrid();
            var row = dtGrid.Select(string.Format("[Calendar Name] = '{0}'", calendarName)).FirstOrDefault();
            if (row != null)
            {
                var devices = string.IsNullOrEmpty(row["Devices"].ToString()) ? 0 : int.Parse(row["Devices"].ToString());
                VerifyEqual(string.Format("3. Verify Calendar Name is {0}", calendarName), calendarName, row["Calendar Name"].ToString());
                VerifyEqual("[SC-777] 3. Verify Devices is 2", 2, devices);
                VerifyEqual(string.Format("3. Verify Geozone is {0}", Settings.RootGeozoneName), Settings.RootGeozoneName, row["Geozone"].ToString());
            }
            else
            {
                Warning(string.Format("3. There is no failure for {0}", calendarName));
            }

            Step("4. Verify The Calendar Commissioning Failure Detail displays with");
            Step(" o Refresh button");
            Step(" o Commission button");
            Step(" o Reporting Information button");
            VerifyEqual("4. Verify The Calendar Commissioning Failure Detail displays Refresh button", true, schedulingManagerPage.GridPanel.IsFailuresRefreshToolbarButtonDisplayed());
            VerifyEqual("4. Verify The Calendar Commissioning Failure Detail displays Commission button", true, schedulingManagerPage.GridPanel.IsFailuresCommissionToolbarButtonDisplayed());
            VerifyEqual("4. Verify The Calendar Commissioning Failure Detail displays Reporting Information button", true, schedulingManagerPage.GridPanel.IsFailuresReportingInformationToolbarButtonDisplayed());
            
            Step("5. Verify The Calendar Commissioning Failure Detail contains a grid with columns");
            Step(" o Identifier");
            Step(" o Calendar Name");
            Step(" o Failure Message");
            Step(" o Failure Event Time");
            Step(" o Commission In Progress");
            expectedColumns = new List<string> { "Identifier", "Calendar Name", "Failure Message", "Failure Event Time", "Commission In Progress" };
            VerifyEqual("5. The Failure displays 3 columns: Calendar Name, Devices, Geozone", expectedColumns , schedulingManagerPage.GridPanel.GetListOfColumnsHeaderFailures());

            Step("6. Uncheck a checkbox next to Calendar Name");
            schedulingManagerPage.SchedulingManagerPanel.TickAllCalendarFailuresCheckbox(false);
            schedulingManagerPage.GridPanel.WaitForDataReloaded();

            Step("7. Verify All calendar names are unchecked and the grid is empty");
            VerifyEqual("7. Verify All calendar names are unchecked", true, schedulingManagerPage.SchedulingManagerPanel.AreFailuresCalendarGridRecordsUnchecked());
            VerifyEqual("7. Verify Failures details grid is empty", false, schedulingManagerPage.GridPanel.GetListOfColumnDataFailures("Identifier").Any());

            Step("8. Check a Calendar Name");
            schedulingManagerPage.SchedulingManagerPanel.TickFailuresCalendarGridRecordCheckbox(calendarName, true);
            schedulingManagerPage.GridPanel.WaitForDataReloaded();

            Step("9. Verify The grid is displayed with the following information");
            Step(" o Identifier: streetlight id");
            Step(" o Calendar Name: calendar Name of streetlight");
            Step(" o Failure Message: {\"comment\":\"value of comment sent in the precondition\",\"user\":\"the current user\"}");
            Step(" o Failure Event Time: the current datetime");
            Step(" o Commission In Progress: false");           
            dtGrid = schedulingManagerPage.GridPanel.BuildFailuresDataTable();            
            row = dtGrid.Select(string.Format("Identifier = '{0}'", streetlight1)).FirstOrDefault();
            if (row != null)
            {
                var failureMessageFormat = "{\"comment\":\"" + comment1 + "\",\"user\":\"" + Settings.Users["DefaultTest"].Username + "\"}";
                VerifyEqual(string.Format("[{0}] 9. Verify Identifier is {0}", streetlight1), streetlight1, row["Identifier"].ToString());
                VerifyEqual(string.Format("[{0}] 9. Verify Calendar Name is {1}", streetlight1, calendarName), calendarName, row["Calendar Name"].ToString());
                VerifyEqual(string.Format("[{0}] 9. Verify Failure Message is {1}", streetlight1, failureMessageFormat), failureMessageFormat, row["Failure Message"].ToString());
                VerifyEqual(string.Format("[{0}] 9. Verify Failure Event Time is {1}", streetlight1, eventDateTime), eventDateTime, DateTime.Parse(row["Failure Event Time"].ToString()).Date);
                VerifyEqual(string.Format("[{0}] 9. Verify Commission In Progress is false", streetlight1), "false", row["Commission In Progress"].ToString());
            }
            else
            {
                Warning(string.Format("9. There is no failure for {0}", streetlight1));
            }
            row = dtGrid.Select(string.Format("Identifier = '{0}'", streetlight2)).FirstOrDefault();
            if (row != null)
            {
                var failureMessageFormat = "{\"comment\":\"" + comment2 + "\",\"user\":\"" + Settings.Users["DefaultTest"].Username + "\"}";
                VerifyEqual(string.Format("[{0}] 9. Verify Identifier is {0}", streetlight2), streetlight2, row["Identifier"].ToString());
                VerifyEqual(string.Format("[{0}] 9. Verify Calendar Name is {1}", streetlight2, calendarName), calendarName, row["Calendar Name"].ToString());
                VerifyEqual(string.Format("[{0}] 9. Verify Failure Message is {1}", streetlight2, failureMessageFormat), failureMessageFormat, row["Failure Message"].ToString());
                VerifyEqual(string.Format("[{0}] 9. Verify Failure Event Time is {1}", streetlight2, eventDateTime), eventDateTime, DateTime.Parse(row["Failure Event Time"].ToString()).Date);
                VerifyEqual(string.Format("[{0}] 9. Verify Commission In Progress is false", streetlight2), "false", row["Commission In Progress"].ToString());
            }
            else
            {
                Warning(string.Format("9. There is no failure for {0}", streetlight2));
            }

            Step("10. Press Refresh button");
            schedulingManagerPage.GridPanel.ClickReloadDataToolbarButton();
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("11. Verify");
            Step(" o Only Calendar Name is checked");
            Step(" o The grid is reloaded and displayed information of all streetlights");           
            VerifyEqual(string.Format("11. Verify Calendar '{0}' is checked", calendarName), true, schedulingManagerPage.SchedulingManagerPanel.IsFailuresGridRecordHasCheckbox(calendarName));
            VerifyEqual("11. Verify The grid is reloaded and displayed information of all streetlights", true, schedulingManagerPage.GridPanel.GetListOfColumnDataFailures("Identifier").Count >= 2);

            Step("12. Input a text into the All Fields Search textbox of the tab Failures, and press Search Icon"); 
            schedulingManagerPage.SchedulingManagerPanel.EnterSearchAllInput(calendarName);
            schedulingManagerPage.SchedulingManagerPanel.ClickSearchFailuresButton();

            Step("13. Verify Only calendar names match the inputted text are displayed in the list");
            var listCalendars = schedulingManagerPage.SchedulingManagerPanel.GetListOfFailuresCalendarName();
            VerifyEqual(string.Format("13. Verify Grid records has calendar {0}", streetlight2), true, listCalendars.Count == 1 && listCalendars.Contains(calendarName));            

            Step("14. Clear the inputted text");
            schedulingManagerPage.SchedulingManagerPanel.ClickClearSearchFailuresButton();
            schedulingManagerPage.SchedulingManagerPanel.WaitForClearSeachButtonDisappered();
            schedulingManagerPage.GridPanel.WaitForDataReloaded();

            Step("15. Verify All calendars names displays in the list");
            VerifyEqual("15. Verify All calendars names displays in the list", true, listCalendars.Count >= 1);

            Step("16. Check a checkbox next to Calendar Name, check the checkbox next to Identifier of a streetlight");
            schedulingManagerPage.SchedulingManagerPanel.TickFailuresCalendarGridRecordCheckbox(calendarName, true);
            schedulingManagerPage.GridPanel.WaitForDataReloaded();
            schedulingManagerPage.GridPanel.TickFailuresGridRecordCheckbox(streetlight2, true);
            
            Step("17. Verify The checkbox is checked");
            VerifyEqual(string.Format("17. Verify The checkbox value of {0} is checked", streetlight2), true, schedulingManagerPage.GridPanel.GetFailuresGridRecordCheckboxValue(streetlight2));

            Step("18. Press Reporting Information button");
            schedulingManagerPage.GridPanel.ClickReportInformationToolbarButton();
            schedulingManagerPage.WaitForPopupDialogDisplayed();

            Step("19. Verify a pop-up displays with");
            Step(" o Title: Scheduling Manager Failures");
            Step(" o Details:");
            Step("  + This failure reporting applies only to TALQ-compliant devices");
            Step("  + To resolve failures, select devices in the list and click recommission icon in toolbar");
            Step("  + In addition, calendar commissioning failures last triggered more than 7 days ago are not included here");
            var expectedTitle = "Scheduling Manager Failures";
            var expectedLine1 = "This failure reporting applies only to TALQ-compliant devices";
            var expectedLine2 = "To resolve failures, select devices in list and click recommission icon in toolbar";
            var expectedLine3 = "In addition, calendar commissioning failures last triggered more than 7 days ago are not included here";
            var informationList = schedulingManagerPage.Dialog.GetListOfFailuresInformation();
            VerifyEqual("19. Verify a pop-up displays", true, schedulingManagerPage.IsPopupDialogDisplayed());
            VerifyEqual("19. Verify Title is " + expectedTitle, expectedTitle, schedulingManagerPage.Dialog.GetDialogTitleText());
            VerifyEqual("19. Verify Line 1 is " + expectedLine1, expectedLine1, informationList[0]);
            VerifyEqual("19. Verify Line 2 is " + expectedLine2, expectedLine2, informationList[1]);
            VerifyEqual("19. Verify Line 3 is " + expectedLine3, expectedLine3, informationList[2]);

            Step("20. Close the pop-up");
            schedulingManagerPage.Dialog.ClickCloseButton();
            schedulingManagerPage.WaitForPopupDialogDisappeared();

            try
            {
                Step("21. Delete test data after testcase is done.");
                DeleteGeozone(geozone);
                DeleteCalendar(calendarName);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("SC-548 Scheduling Manager - Make calendar commission failure locking time out after 24 hours")]
        public void SC_548()
        {
            var testData = GetTestDataOfSC_548();          
            var controllerId = testData["ControllerId"];
            var controllerName = testData["ControllerName"];
            var geozone = SLVHelper.GenerateUniqueName("GZNSC548");
            var streetlight1 = SLVHelper.GenerateUniqueName("STL01");
            var streetlight2 = SLVHelper.GenerateUniqueName("STL02");
            var calendarName1 = SLVHelper.GenerateUniqueName("CSC54801");
            var calendarName2 = SLVHelper.GenerateUniqueName("CSC54802");
            var calendarName3 = SLVHelper.GenerateUniqueName("CSC54803");
            var eventDateTime = Settings.GetServerTime().AddDays(-1).Date;

            Step("**** Precondition ****");
            Step(" - Change the lockout time in CMS server to 10 minutes (600000 milliseconds): Update the CMS configuration file /usr/ssn/slv-cms/etc/slvserver.cfg");
            Step(" - Create a new streetlight named SL-SC-548-01 with Dimming group: Calendar-SC-548-01");
            Step(" - Create a new streetlight named SL-SC-548-02 with Dimming group: Calendar-SC-548-02 and Controller is Smartsims (for commissioning successfully)");
            Step(" - Simulate the calendar commission failure by sending the cmd.");
            Step("   o Device: SL-SC-548-01");
            Step("   o valueName=calendarCommissionFailure");
            Step("   o value=true");
            Step("   o eventTime=<1 day before the current date>");
            Step("**** Precondition ****\n");

            Step("-> Create data for testing");
            DeleteGeozones("GZNSC548*");
            CreateNewCalendar(calendarName1);
            CreateNewCalendar(calendarName2);
            CreateNewCalendar(calendarName3);
            CreateNewGeozone(geozone);
            CreateNewDevice(DeviceType.Streetlight, streetlight1, controllerId, geozone);
            CreateNewDevice(DeviceType.Streetlight, streetlight2, controllerId, geozone);
            SetValueToDevice(controllerId, streetlight1, "DimmingGroupName", calendarName1, Settings.GetCurrentControlerDateTime(controllerId));
            SetValueToDevice(controllerId, streetlight2, "DimmingGroupName", calendarName2, Settings.GetCurrentControlerDateTime(controllerId));

            var status = SetValueToDevice(controllerId, streetlight1, "calendarCommissionFailure", true, eventDateTime);
            VerifyEqual(string.Format("3. Verify send request for streetlight {0} success", streetlight1), true, status);

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.EquipmentInventory, App.SchedulingManager);    

            Step("1. Go to Scheduling Manager app, select Failure tab, check the testing calendar: Calendar-SC-548-01");
            var schedulingManagerPage = desktopPage.GoToApp(App.SchedulingManager) as SchedulingManagerPage;
            schedulingManagerPage.SchedulingManagerPanel.SelectTab("Failures");
            schedulingManagerPage.WaitForPreviousActionComplete();
            schedulingManagerPage.SchedulingManagerPanel.TickAllCalendarFailuresCheckbox(false);
            schedulingManagerPage.GridPanel.WaitForDataReloaded();
            schedulingManagerPage.SchedulingManagerPanel.TickFailuresCalendarGridRecordCheckbox(calendarName1, true);
            schedulingManagerPage.GridPanel.WaitForDataReloaded();

            Step("2. Expected A new line of failure of Streetlight SL-SC-548-01 is added in the list with");
            Step(" o Identifier: Streetlight name");
            Step(" o Calendar Name: Calendar-SC-548-01");
            Step(" o Failure Message: {\"user\":\"username\"}");
            Step(" o Failure Event Time: Datetime set in the parameter 'eventTime' of the command");
            Step(" o Commission In Progress: false");
            var dtGrid = schedulingManagerPage.GridPanel.BuildFailuresDataTable();                      
            var failureMessageFormat = "{\"user\":\"" + Settings.Users["DefaultTest"].Username + "\"}";           
            var row = dtGrid.Select(string.Format("Identifier = '{0}'", streetlight1)).FirstOrDefault();
            if (row != null)
            {
                VerifyEqual(string.Format("[{0}] 2. Verify Identifier is {0}", streetlight1), streetlight1, row["Identifier"].ToString());
                VerifyEqual(string.Format("[{0}] 2. Verify Calendar Name is {1}", streetlight1, calendarName1), calendarName1, row["Calendar Name"].ToString());
                VerifyEqual(string.Format("[{0}] 2. Verify Failure Message is {1}", streetlight1, failureMessageFormat), failureMessageFormat, row["Failure Message"].ToString());
                VerifyEqual(string.Format("[{0}] 2. Verify Failure Event Time is {1}", streetlight1, eventDateTime), eventDateTime, DateTime.Parse(row["Failure Event Time"].ToString()).Date);
                VerifyEqual(string.Format("[{0}] 2. Verify Commission In Progress is false", streetlight1), "false", row["Commission In Progress"].ToString());
            }
            else
            {
                Warning(string.Format("2. There is no failure for {0}", streetlight1));
            }

            Step("3. Select Application Switcher and choose Equipment Inventory");
            var equipmentInventoryPage = schedulingManagerPage.AppBar.SwitchTo(App.EquipmentInventory) as EquipmentInventoryPage;

            Step("4. Change the Dimming group of SL-SC-548-01 to 'Calendar-SC-548-03'");
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(geozone + @"\" + streetlight1);
            equipmentInventoryPage.WaitForDeviceEditorPanelDisplayed();
            equipmentInventoryPage.StreetlightEditorPanel.SelectDimmingGroupDropDown(calendarName3);
            equipmentInventoryPage.StreetlightEditorPanel.ClickSaveButton();
            equipmentInventoryPage.WaitForPreviousActionComplete();
            equipmentInventoryPage.WaitForDeviceEditorPanelDisappeared();

            Step("5. Refresh the page and go to Scheduling Manager app, select Failure tab, check the testing calendar: Calendar-SC-548-03");
            desktopPage = Browser.RefreshLoggedInCMS();
            schedulingManagerPage = desktopPage.GoToApp(App.SchedulingManager) as SchedulingManagerPage;
            schedulingManagerPage.SchedulingManagerPanel.SelectTab("Failures");
            schedulingManagerPage.WaitForPreviousActionComplete();
            schedulingManagerPage.SchedulingManagerPanel.TickAllCalendarFailuresCheckbox(false);
            schedulingManagerPage.GridPanel.WaitForDataReloaded();
            schedulingManagerPage.SchedulingManagerPanel.TickFailuresCalendarGridRecordCheckbox(calendarName3, true);
            schedulingManagerPage.GridPanel.WaitForDataReloaded();

            Step("6. Expected the record of failure of Streetlight SL-SC-548-01 is updated in the list with");
            Step(" o Identifier: Streetlight name");
            Step(" o Calendar Name: Calendar-SC-548-03");
            Step(" o Failure Message: {\"user\":\"username\"}");
            Step(" o Failure Event Time: Datetime set in the parameter 'eventTime' of the command");
            Step(" o Commission In Progress: true");
            Step(" o The record is readonly");
            dtGrid = schedulingManagerPage.GridPanel.BuildFailuresDataTable();
            row = dtGrid.Select(string.Format("Identifier = '{0}'", streetlight1)).FirstOrDefault();
            if (row != null)
            {
                VerifyEqual(string.Format("[{0}] 6. Verify Identifier is {0}", streetlight1), streetlight1, row["Identifier"].ToString());
                VerifyEqual(string.Format("[{0}] 6. Verify Calendar Name is {1}", streetlight1, calendarName3), calendarName3, row["Calendar Name"].ToString());
                VerifyEqual(string.Format("[{0}] 6. Verify Failure Message is {1}", streetlight1, failureMessageFormat), failureMessageFormat, row["Failure Message"].ToString());
                VerifyEqual(string.Format("[{0}] 6. Verify Failure Event Time is {1}", streetlight1, eventDateTime), eventDateTime, DateTime.Parse(row["Failure Event Time"].ToString()).Date);
                VerifyEqual(string.Format("[{0}] 6. Verify Commission In Progress is true", streetlight1), "true", row["Commission In Progress"].ToString());
                VerifyEqual(string.Format("[{0}] 6. The record is readonly", streetlight1), true, schedulingManagerPage.GridPanel.IsFailuresGridRecordReadOnly(streetlight1));
            }
            else
            {
                Warning(string.Format("6. There is no failure for {0}", streetlight1));
            }

            Step("7. Simulate the calendar commission failure by sending the cmd.");
            Step(" o Device: SL-SC-548-02");
            Step(" o valueName=calendarCommissionFailure");
            Step(" o value=true");
            Step(" o eventTime=<1 day before the current date>");
            status = SetValueToDevice(controllerId, streetlight2, "calendarCommissionFailure", true, eventDateTime);
            VerifyEqual(string.Format("Verify send request for streetlight {0} success", streetlight2), true, status);

            Step("8. Refresh the page and go to Scheduling Manager app, select Failure tab, check the testing calendar: Calendar-SC-548-02");
            desktopPage = Browser.RefreshLoggedInCMS();
            schedulingManagerPage = desktopPage.GoToApp(App.SchedulingManager) as SchedulingManagerPage;
            schedulingManagerPage.SchedulingManagerPanel.SelectTab("Failures");
            schedulingManagerPage.WaitForPreviousActionComplete();
            schedulingManagerPage.SchedulingManagerPanel.TickAllCalendarFailuresCheckbox(false);
            schedulingManagerPage.GridPanel.WaitForDataReloaded();
            schedulingManagerPage.SchedulingManagerPanel.TickFailuresCalendarGridRecordCheckbox(calendarName2, true);
            schedulingManagerPage.GridPanel.WaitForDataReloaded();

            Step("9. Expected A new line of failure of Streetlight SL-SC-548-02 is added in the list with");
            Step(" o Identifier: Streetlight name");
            Step(" o Calendar Name: Calendar-SC-548-02");
            Step(" o Failure Message: {\"user\":\"username\"}");
            Step(" o Failure Event Time: Datetime set in the parameter 'eventTime' of the command");
            Step(" o Commission In Progress: false");
            dtGrid = schedulingManagerPage.GridPanel.BuildFailuresDataTable();
            row = dtGrid.Select(string.Format("Identifier = '{0}'", streetlight2)).FirstOrDefault();
            if (row != null)
            {
                VerifyEqual(string.Format("[{0}] 9. Verify Identifier is {0}", streetlight2), streetlight2, row["Identifier"].ToString());
                VerifyEqual(string.Format("[{0}] 9. Verify Calendar Name is {1}", streetlight2, calendarName2), calendarName2, row["Calendar Name"].ToString());
                VerifyEqual(string.Format("[{0}] 9. Verify Failure Message is {1}", streetlight2, failureMessageFormat), failureMessageFormat, row["Failure Message"].ToString());
                VerifyEqual(string.Format("[{0}] 9. Verify Failure Event Time is {1}", streetlight2, eventDateTime), eventDateTime, DateTime.Parse(row["Failure Event Time"].ToString()).Date);
                VerifyEqual(string.Format("[{0}] 9. Verify Commission In Progress is false", streetlight2), "false", row["Commission In Progress"].ToString());                
            }
            else
            {
                Warning(string.Format("9. There is no failure for {0}", streetlight2));
            }

            Step("10. Check the checkbox of the failure of SL-SC-548-02 and press Commission button on the grid");
            schedulingManagerPage.GridPanel.TickFailuresGridRecordCheckbox(streetlight2, true);
            schedulingManagerPage.GridPanel.ClickCommissionToolbarButton();
            schedulingManagerPage.WaitForPopupDialogDisplayed();

            Step("11. Press Commission button on the Commissioning pop-up");
            schedulingManagerPage.CommissionPopupPanel.ClickCommissionButton();
            schedulingManagerPage.Dialog.WaitForPopupMessageDisplayed();
            
            Step("12. Verify A message pop-up displays 'Devices successfully submitted for commissioning.'");
            VerifyEqual("12. Verify message 'Devices successfully submitted for commissioning.' is shown", "Devices successfully submitted for commissioning.", schedulingManagerPage.Dialog.GetMessageText());

            Step("13. Press OK button");
            schedulingManagerPage.Dialog.ClickOkButton();
            schedulingManagerPage.Dialog.WaitForPopupMessageDisappeared();

            Step("14. Verify The message pop-up is closed and the Commission pop-up is updated");
            Step(" - Progress: Plane icon");
            var iconUrl = schedulingManagerPage.CommissionPopupPanel.GetProgressIcon(streetlight2);
            VerifyEqual("14. Verify Progress: Plane icon", "Plane", iconUrl);

            Step("15. Press Close button");
            schedulingManagerPage.CommissionPopupPanel.ClickCancelButton();
            schedulingManagerPage.WaitForPopupDialogDisappeared();
            
            Step("16. Verify The Commission pop-up is closed");
            VerifyEqual("16. The Commission pop-up is closed", false, schedulingManagerPage.IsPopupDialogDisplayed());

            Step("17. Wait for 1 minutes");
            Wait.ForMinutes(1);

            Step("18. Refresh the page and go to Scheduling Manager, then select Failures tab, check testing calendars: Calendar-SC-548-01, Calendar-SC-548-03");
            desktopPage = Browser.RefreshLoggedInCMS();
            schedulingManagerPage = desktopPage.GoToApp(App.SchedulingManager) as SchedulingManagerPage;

            Step("19. Expected The 2 records of failure of Streetlight SL-SC-548-01, SL-SC-548-02 are updated with");
            Step(" o Commission In Progress: false");
            schedulingManagerPage.SchedulingManagerPanel.SelectTab("Failures");
            schedulingManagerPage.WaitForPreviousActionComplete();
            schedulingManagerPage.SchedulingManagerPanel.TickAllCalendarFailuresCheckbox(false);
            schedulingManagerPage.GridPanel.WaitForDataReloaded();
            schedulingManagerPage.SchedulingManagerPanel.TickFailuresCalendarGridRecordCheckbox(calendarName2, true);
            schedulingManagerPage.GridPanel.WaitForDataReloaded();
            schedulingManagerPage.SchedulingManagerPanel.TickFailuresCalendarGridRecordCheckbox(calendarName3, true);
            schedulingManagerPage.GridPanel.WaitForDataReloaded();

            dtGrid = schedulingManagerPage.GridPanel.BuildFailuresDataTable();
            row = dtGrid.Select(string.Format("Identifier = '{0}'", streetlight1)).FirstOrDefault();
            if (row != null)
            {
                VerifyEqual(string.Format("[{0}] 19. Verify Commission In Progress is false", streetlight1), "false", row["Commission In Progress"].ToString());                
            }
            else
            {
                Warning(string.Format("19. There is no failure for {0}", streetlight1));
            }
            row = dtGrid.Select(string.Format("Identifier = '{0}'", streetlight2)).FirstOrDefault();
            if (row != null)
            {
                VerifyEqual(string.Format("[{0}] 19. Verify Commission In Progress is false", streetlight2), "false", row["Commission In Progress"].ToString());
            }
            else
            {
                Warning(string.Format("19. There is no failure for {0}", streetlight2));
            }

            Step("20. Check the 2 checkboxes in the list.");
            schedulingManagerPage.GridPanel.TickFailuresGridRecordCheckbox(streetlight1, true);
            schedulingManagerPage.GridPanel.TickFailuresGridRecordCheckbox(streetlight2, true);

            Step("21. Expected The 2 records are selectable.");
            VerifyEqual(string.Format("21. Verify The record {0} is selectable", streetlight1), true, !schedulingManagerPage.GridPanel.IsFailuresGridRecordReadOnly(streetlight1));
            VerifyEqual(string.Format("21. Verify The record {0} is selectable", streetlight2), true, !schedulingManagerPage.GridPanel.IsFailuresGridRecordReadOnly(streetlight2));

            try
            {
                Step("22. Delete the test data after testcase is done.");
                DeleteGeozone(geozone);
                DeleteCalendar(calendarName1);
                DeleteCalendar(calendarName2);
                DeleteCalendar(calendarName3);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("SC-586 Documentation on Calendar Commissioning Failure Tab")]
        public void SC_586()
        { 
            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - User with language 'France'");
            Step("**** Precondition ****\n");

            var userModel = CreateNewProfileAndUser(language: "fr_FR");
            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.SchedulingManager);                    

            Step("1. Go to Scheduling Manager app");
            var schedulingManagerPage = desktopPage.GoToApp(App.SchedulingManager) as SchedulingManagerPage;

            Step("2. Select the Failure tab");
            schedulingManagerPage.SchedulingManagerPanel.SelectTab("Failures");
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("3. Press Reporting Information button");
            schedulingManagerPage.GridPanel.ClickReportInformationToolbarButton();
            schedulingManagerPage.WaitForPopupDialogDisplayed();

            Step("4. Verify a pop-up displays with");
            Step(" o Title: Scheduling Manager Failures");
            Step(" o Details:");
            Step("  + This failure reporting applies only to TALQ-compliant devices");
            Step("  + To resolve failures, select devices in list and click recommission icon in toolbar");
            Step("  + In addition, calendar commissioning failures last triggered more than 7 days ago are not included here");
            var expectedTitle = "Scheduling Manager Failures";
            var expectedLine1 = "This failure reporting applies only to TALQ-compliant devices";
            var expectedLine2 = "To resolve failures, select devices in list and click recommission icon in toolbar";
            var expectedLine3 = "In addition, calendar commissioning failures last triggered more than 7 days ago are not included here";
            var informationList = schedulingManagerPage.Dialog.GetListOfFailuresInformation();            
            VerifyEqual("4. Verify a pop-up displays", true, schedulingManagerPage.IsPopupDialogDisplayed());
            VerifyEqual("4. Verify Title is " + expectedTitle, expectedTitle, schedulingManagerPage.Dialog.GetDialogTitleText());
            VerifyEqual("4. Verify Line 1 is " + expectedLine1, expectedLine1, informationList[0]);
            VerifyEqual("4. Verify Line 2 is " + expectedLine2, expectedLine2, informationList[1]);
            VerifyEqual("4. Verify Line 3 is " + expectedLine3, expectedLine3, informationList[2]);

            Step("5. Close the pop-up");
            schedulingManagerPage.Dialog.ClickCloseButton();
            schedulingManagerPage.WaitForPopupDialogDisappeared();

            Step("6. Log out and login with user using France language");
            desktopPage = SLVHelper.LogoutAndLogin(desktopPage, userModel.Username, userModel.Password);
            desktopPage.InstallAppsIfNotExist(SLVHelper.ConvertAppName(App.SchedulingManager, "French"));

            Step("7. Go to Programmations horaires (Scheduling Manager) app");
            schedulingManagerPage = desktopPage.GoToApp(App.SchedulingManager) as SchedulingManagerPage;

            Step("8. Select the Échecs (Failure) tab");
            schedulingManagerPage.SchedulingManagerPanel.SelectTab("Échecs");
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("9. Press Reporting Information button");
            schedulingManagerPage.GridPanel.ClickReportInformationToolbarButton();
            schedulingManagerPage.WaitForPopupDialogDisplayed();

            Step("10. Verify a pop-up displays with");
            Step(" o Title: Programmations horaires - Incidents de commissionnement");
            Step(" o Details:");
            Step("  + Ce rapport d'incidents ne s'applique qu'aux équipements Talq");
            Step("  + Pour résoudre ces incidents, veuillez sélectionner les équipements désirés dans la liste et cliquer sur le bouton de recommissionnement dans la barre d'outils");
            Step("  + Les incidents qui se sont produits ou répétés il y a plus de 7 jours ne sont pas affichés dans cette liste");
            expectedTitle = "Programmations horaires - Incidents de commissionnement";
            expectedLine1 = "Ce rapport d'incidents ne s'applique qu'aux équipements Talq";
            expectedLine2 = "Pour résoudre ces incidents, veuillez sélectionner les équipements désirés dans la liste et cliquer sur le bouton de recommissionnement dans la barre d'outils";
            expectedLine3 = "Les incidents qui se sont produits ou répétés il y a plus de 7 jours ne sont pas affichés dans cette liste";
            informationList = schedulingManagerPage.Dialog.GetListOfFailuresInformation();
            VerifyEqual("4. Verify a pop-up displays", true, schedulingManagerPage.IsPopupDialogDisplayed());
            VerifyEqual("4. Verify Title is " + expectedTitle, expectedTitle, schedulingManagerPage.Dialog.GetDialogTitleText());
            VerifyEqual("4. Verify Line 1 is " + expectedLine1, expectedLine1, informationList[0]);
            VerifyEqual("4. Verify Line 2 is " + expectedLine2, expectedLine2, informationList[1]);
            VerifyEqual("4. Verify Line 3 is " + expectedLine3, expectedLine3, informationList[2]);

            Step("11. Close the pop-up");
            schedulingManagerPage.Dialog.ClickCloseButton();
            schedulingManagerPage.WaitForPopupDialogDisappeared();

            try
            {
                Step("12. Delete the testing data after test is done.");
                DeleteUserAndProfile(userModel);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("SC-665 Apps that use maps don't display tooltips properly on DARK themes")]
        public void SC_665()
        {
            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - There is a user belonging to a profile using theme 'streetlight' Test User Roles");
            Step("**** Precondition ****\n");

            var testData = GetTestDataOfSC_665();
            var geozone = testData["Geozone"].ToString();
            var streetlights = testData["Streetlights"] as List<DeviceModel>;
            var streetlight = streetlights.PickRandom();

            var userModel = CreateNewProfileAndUser(skin: "streetlight");
            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(userModel.Username, userModel.Password);
            desktopPage.InstallAppsIfNotExist(App.EquipmentInventory);

            Step("1. Go to Equipment Inventory app");
            var equipmentInventoryPage = desktopPage.GoToApp(App.EquipmentInventory) as EquipmentInventoryPage;
                        
            Step("2. Select a geozone and hover the mouse on a device");
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(geozone + @"\" + streetlight.Name);
            equipmentInventoryPage.Map.MoveToDeviceGL(streetlight.Longitude, streetlight.Latitude);
            var actualBackgroundColor = equipmentInventoryPage.Map.GetTooltipBackgroundColor();
            var actualTextColor = equipmentInventoryPage.Map.GetTooltipTextColor();

            Step("3. Verify The tooltip displays with");
            Step(" o Background color: #fafafa");
            Step(" o Text color: #333333");
            VerifyEqual("3. Verify Background color: #fafafa", "#fafafa", actualBackgroundColor.ToLower());
            VerifyEqual("3. Verify Text color: #333333", "#333333", actualTextColor.ToLower());

            Step("4. Change the profile of user to user theme 'streetdark'");
            var usersPage = equipmentInventoryPage.AppBar.SwitchTo(App.Users) as UsersPage;
            usersPage.UserProfileListPanel.SelectProfile(userModel.Profile);
            usersPage.UserProfileDetailsPanel.SelectSkinDropDown("streetdark");
            usersPage.UserProfileDetailsPanel.ClickSaveButton();
            usersPage.WaitForPreviousActionComplete();
            usersPage.WaitForHeaderMessageDisappeared();

            Step("5. Refressh the page. Go to Equipment Inventory app");
            desktopPage = Browser.RefreshLoggedInCMS();
            equipmentInventoryPage = desktopPage.GoToApp(App.EquipmentInventory) as EquipmentInventoryPage;

            Step("6. Select a geozone and hover the mouse on a device");
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(geozone + @"\" + streetlight.Name);
            equipmentInventoryPage.Map.MoveToDeviceGL(streetlight.Longitude, streetlight.Latitude);
            actualBackgroundColor = equipmentInventoryPage.Map.GetTooltipBackgroundColor();
            actualTextColor = equipmentInventoryPage.Map.GetTooltipTextColor();

            Step("7. Verify The tooltip displays with");
            Step(" o Background color: #323948");
            Step(" o Text color: #fafafa");
            VerifyEqual("7. Verify Background color: #323948", "#323948", actualBackgroundColor.ToLower());
            VerifyEqual("7. Verify Text color: #fafafa", "#fafafa", actualTextColor.ToLower());

            try
            {
                Step("8. Delete test data after testcase is done");
                DeleteUserAndProfile(userModel);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("SC-61 - Equipment Inventory - Unable to create or select devices belonging to certain categories")]
        public void SC_61()
        {
            var newDevicesList = new List<string>();
            var geozone = SLVHelper.GenerateUniqueName("GZNSC61");

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            Step("-> Create data for testing");
            DeleteGeozones("GZNSC61*");
            CreateNewGeozone(geozone);

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.EquipmentInventory);           

            Step("1. Go to Equipment Inventory app");
            Step("2. Select a geozone and press Add Device");
            var equipmentInventoryPage = desktopPage.GoToApp(App.EquipmentInventory) as EquipmentInventoryPage;
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(geozone);

            Step("3. Select Camera IP and input all required fields, then position the device");
            var deviceName = SLVHelper.GenerateUniqueName("Camera");
            newDevicesList.Add(deviceName);
            equipmentInventoryPage.CreateDevice(DeviceType.CameraIp, deviceName: deviceName, identifier: deviceName);

            Step("4. Verify The device is added successfully and displays on the map and the geozone");
            equipmentInventoryPage.Map.MoveToSelectedDeviceGL();
            var mapName = equipmentInventoryPage.Map.GetDeviceNameGL();
            equipmentInventoryPage.AppBar.ClickHeaderBartop();
            VerifyEqual(string.Format("4. Verify {0} is added successfully and displays on the map", deviceName), deviceName, mapName);
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(geozone);
            equipmentInventoryPage.WaitForGeozoneEditorPanelDisplayed();
            var devices = equipmentInventoryPage.GeozoneTreeMainPanel.GetChildNodeNamesOfSelectedNode(NodeType.CameraIp);
            VerifyEqual(string.Format("4. Verify {0} is added successfully and displays on the geozone", deviceName), true, devices.Contains(deviceName));

            Step("5. Select parent geozone and press Add Device");            
            Step("6. Select Audio Player and input all required fields, then position the device");
            deviceName = SLVHelper.GenerateUniqueName("Audio");
            newDevicesList.Add(deviceName);
            equipmentInventoryPage.CreateDevice(DeviceType.AudioPlayer, deviceName: deviceName, identifier: deviceName, gatewayHostName: "localhost");

            Step("7. Verify The device is added successfully and displays on the map and the geozone");
            equipmentInventoryPage.Map.MoveToSelectedDeviceGL();
            mapName = equipmentInventoryPage.Map.GetDeviceNameGL();
            equipmentInventoryPage.AppBar.ClickHeaderBartop();
            VerifyEqual(string.Format("7. Verify {0} is added successfully and displays on the map", deviceName), deviceName, mapName);
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(geozone);
            equipmentInventoryPage.WaitForGeozoneEditorPanelDisplayed();
            devices = equipmentInventoryPage.GeozoneTreeMainPanel.GetChildNodeNamesOfSelectedNode(NodeType.AudioPlayer);
            VerifyEqual(string.Format("7. Verify {0} is added successfully and displays on the geozone", deviceName), true, devices.Contains(deviceName));

            Step("8. Select parent geozone and press Add Device");
            Step("9. Select Building and input all required fields, then position the device");
            deviceName = SLVHelper.GenerateUniqueName("Building");
            newDevicesList.Add(deviceName);
            equipmentInventoryPage.CreateDevice(DeviceType.Building, deviceName: deviceName, controllerID: "Vietnam Controller", identifier: deviceName);

            Step("10. Verify The device is added successfully and displays on the map and the geozone");
            equipmentInventoryPage.Map.MoveToSelectedDeviceGL();
            mapName = equipmentInventoryPage.Map.GetDeviceNameGL();
            equipmentInventoryPage.AppBar.ClickHeaderBartop();
            VerifyEqual(string.Format("10. Verify {0} is added successfully and displays on the map", deviceName), deviceName, mapName);
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(geozone);
            equipmentInventoryPage.WaitForGeozoneEditorPanelDisplayed();
            devices = equipmentInventoryPage.GeozoneTreeMainPanel.GetChildNodeNamesOfSelectedNode(NodeType.Building);
            VerifyEqual(string.Format("10. Verify {0} is added successfully and displays on the geozone", deviceName), true, devices.Contains(deviceName));
            
            Step("11. Select parent geozone and press Add Device");
            Step("12. Select Parking Place and input all required fields, then position the device");
            deviceName = SLVHelper.GenerateUniqueName("ParkingPlace");
            newDevicesList.Add(deviceName);
            equipmentInventoryPage.CreateDevice(DeviceType.ParkingPlace, deviceName: deviceName, identifier: deviceName);

            Step("13. Verify The device is added successfully and displays on the map and the geozone");
            equipmentInventoryPage.Map.MoveToSelectedDeviceGL();
            mapName = equipmentInventoryPage.Map.GetDeviceNameGL();
            equipmentInventoryPage.AppBar.ClickHeaderBartop();
            VerifyEqual(string.Format("13. Verify {0} is added successfully and displays on the map", deviceName), deviceName, mapName);
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(geozone);
            equipmentInventoryPage.WaitForGeozoneEditorPanelDisplayed();
            devices = equipmentInventoryPage.GeozoneTreeMainPanel.GetChildNodeNamesOfSelectedNode(NodeType.ParkingPlace);
            VerifyEqual(string.Format("13. Verify {0} is added successfully and displays on the geozone", deviceName), true, devices.Contains(deviceName));

            Step("14. Select parent geozone and press Add Device");
            Step("15. Select Tank and input all required fields, then position the device");
            deviceName = SLVHelper.GenerateUniqueName("Tank");
            newDevicesList.Add(deviceName);
            equipmentInventoryPage.CreateDevice(DeviceType.Tank, deviceName: deviceName, identifier: deviceName);

            Step("16. Verify The device is added successfully and displays on the map and the geozone");
            equipmentInventoryPage.Map.MoveToSelectedDeviceGL();
            mapName = equipmentInventoryPage.Map.GetDeviceNameGL();
            equipmentInventoryPage.AppBar.ClickHeaderBartop();
            VerifyEqual(string.Format("16. Verify {0} is added successfully and displays on the map", deviceName), deviceName, mapName);
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(geozone);
            equipmentInventoryPage.WaitForGeozoneEditorPanelDisplayed();
            devices = equipmentInventoryPage.GeozoneTreeMainPanel.GetChildNodeNamesOfSelectedNode(NodeType.Tank);
            VerifyEqual(string.Format("16. Verify {0} is added successfully and displays on the geozone", deviceName), true, devices.Contains(deviceName));

            Step("17. Select parent geozone and press Add Device");
            Step("18. Select Waste Container and input all required fields, then position the device");
            deviceName = SLVHelper.GenerateUniqueName("Waste");
            newDevicesList.Add(deviceName);
            equipmentInventoryPage.CreateDevice(DeviceType.WasteContainer, deviceName: deviceName, identifier: deviceName);

            Step("19. Verify The device is added successfully and displays on the map and the geozone");
            equipmentInventoryPage.Map.MoveToSelectedDeviceGL();
            mapName = equipmentInventoryPage.Map.GetDeviceNameGL();
            equipmentInventoryPage.AppBar.ClickHeaderBartop();
            VerifyEqual(string.Format("19. Verify {0} is added successfully and displays on the map", deviceName), deviceName, mapName);
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(geozone);
            equipmentInventoryPage.WaitForGeozoneEditorPanelDisplayed();
            devices = equipmentInventoryPage.GeozoneTreeMainPanel.GetChildNodeNamesOfSelectedNode(NodeType.WasteContainer);
            VerifyEqual(string.Format("19. Verify {0} is added successfully and displays on the geozone", deviceName), true, devices.Contains(deviceName));

            Step("20. Select parent geozone and press Add Device");
            Step("21. Select Weather Station and input all required fields, then position the device");
            deviceName = SLVHelper.GenerateUniqueName("Weather");
            newDevicesList.Add(deviceName);
            equipmentInventoryPage.CreateDevice(DeviceType.WeatherStation, deviceName: deviceName, identifier: deviceName);

            Step("22. Verify The device is added successfully and displays on the map and the geozone");
            equipmentInventoryPage.Map.MoveToSelectedDeviceGL();
            mapName = equipmentInventoryPage.Map.GetDeviceNameGL();
            equipmentInventoryPage.AppBar.ClickHeaderBartop();
            VerifyEqual(string.Format("22. Verify {0} is added successfully and displays on the map", deviceName), deviceName, mapName);
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(geozone);
            equipmentInventoryPage.WaitForGeozoneEditorPanelDisplayed();
            devices = equipmentInventoryPage.GeozoneTreeMainPanel.GetChildNodeNamesOfSelectedNode(NodeType.WeatherStation);
            VerifyEqual(string.Format("22. Verify {0} is added successfully and displays on the geozone", deviceName), true, devices.Contains(deviceName));

            Step("23. Refresh page and come back to Equipment Inventory > geozone");
            desktopPage = Browser.RefreshLoggedInCMS();
            equipmentInventoryPage = desktopPage.GoToApp(App.EquipmentInventory) as EquipmentInventoryPage;
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(geozone);
            equipmentInventoryPage.WaitForGeozoneEditorPanelDisplayed();

            Step("24. Verify All added devices are listed correctly");
            var geozoneDevices = equipmentInventoryPage.GeozoneTreeMainPanel.GetChildNodeNamesOfSelectedNode();
            VerifyTrue("24. Verify All added devices are listed correctly", geozoneDevices.CheckIfIncluded(newDevicesList), string.Join(",", newDevicesList), string.Join(",", geozoneDevices));

            Step("25. Loop through each device in the list");
            Step("26. Verify Device Editor panel of each device is loaded successfully.");
            foreach (var device in newDevicesList)
            {
                equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(device);
                equipmentInventoryPage.WaitForDeviceEditorPanelDisplayed();
                VerifyEqual(string.Format("26. Verify Device Editor panel of {0} is loaded successfully", deviceName), device, equipmentInventoryPage.DeviceEditorPanel.GetNameValue());
            }

            try
            {
                Step("27. Delete test data after testcase is done");
                DeleteGeozone(geozone);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("SC-825 -Schedule Manager - Make control program template names more explicit")]
        public void SC_825()
        {
            var testData = GetTestDataOfSC_825();
            var expectedTemplates = testData["ExpectedTemplates"] as List<string>;

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - User with language 'France'");
            Step("**** Precondition ****\n");

            var userModel = CreateNewProfileAndUser(language: "fr_FR");
            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(userModel.Username, userModel.Password);
            desktopPage.InstallAppsIfNotExist(SLVHelper.ConvertAppName(App.SchedulingManager, "French"));
            
            Step("1. Go to Programmations horaires app");
            var schedulingManagerPage = desktopPage.GoToApp(App.SchedulingManager) as SchedulingManagerPage;

            Step("2. Select Add button");
            schedulingManagerPage.SchedulingManagerPanel.ClickAddControlProgramButton();
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("3. Select dropdowlist Type");
            var actualTemplatesList = schedulingManagerPage.ControlProgramEditorPanel.GetListOfTemplateItems();
            
            Step("4. Verify Type contains the following");
            Step(" - Commutation astronomique");
            Step(" - Commutation astronomique avec évènements à heures fixes");
            Step(" - Toujours allumé");
            Step(" - Toujours éteint");
            Step(" - Evènements à heures fixes de jour");
            Step(" - Mode avancé");
            VerifyEqual("Verify Type contains as expected", expectedTemplates, actualTemplatesList);

            try
            {
                Info("Delete the testing data after test is done.");
                DeleteUserAndProfile(userModel);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("SC-906 -Equipment Inventory - Importing a CSV file that creates a new geozone generates a geozone with all bounds set to 0")]
        [NonParallelizable]
        public void SC_906()
        {
            var csvFilePath = Settings.GetFullPath(Settings.CSV_FILE_PATH + "SC906.csv");
            var geozone = SLVHelper.GenerateUniqueName("GZNSC906");
            var streetlight1 = SLVHelper.GenerateUniqueName("STL01");
            var streetlightLat1 = SLVHelper.GenerateLatitude();
            var streetlightLong1 = SLVHelper.GenerateLongitude();
            var streetlight2 = SLVHelper.GenerateUniqueName("STL02");
            var streetlightLat2 = SLVHelper.GenerateLatitude();
            var streetlightLong2 = SLVHelper.GenerateLongitude();
            var geozoneFullPath = Settings.RootGeozoneName + @"/" + geozone;
            var controllerId = "vietnamcontroller";
            var typeOfEquipment = "ABEL-Vigilon A[Dimmable ballast]";

            var latMin = streetlightLat1;
            var latMax = streetlightLat1;
            var longMin = streetlightLong1;           
            var longMax = streetlightLong1;

            if (decimal.Parse(streetlightLat1) > decimal.Parse(streetlightLat2))
            {
                latMin = streetlightLat2;
            }
            else
            {
                latMax = streetlightLat2;
            }

            if (decimal.Parse(streetlightLong1) > decimal.Parse(streetlightLong2))
            {
                longMin = streetlightLong2;
            }
            else
            {
                longMax = streetlightLong2;
            }

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - Prepare a csv to import streelights");
            Step("**** Precondition ****\n");

            Step("-> Create data for testing");
            DeleteGeozones("GZNSC906*");
            CreateCsvDevices(csvFilePath, geozoneFullPath, new List<DeviceModel>
            {
                new DeviceModel{ Type = DeviceType.Streetlight, Id = streetlight1, Name = streetlight1, Controller = controllerId, TypeOfEquipment = typeOfEquipment, Latitude = streetlightLat1, Longitude = streetlightLong1, UniqueAddress = SLVHelper.GenerateMACAddress() },
                new DeviceModel{ Type = DeviceType.Streetlight, Id = streetlight2, Name = streetlight2, Controller = controllerId, TypeOfEquipment = typeOfEquipment, Latitude = streetlightLat2, Longitude = streetlightLong2, UniqueAddress = SLVHelper.GenerateMACAddress() }
            });            
            ImportFile(csvFilePath);

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.EquipmentInventory);
            
            Step("1. Using API to import the csv file, then refresh the page and go to the Equipment Inventory app > testing geozone");
            var equipmentInventoryPage = desktopPage.GoToApp(App.EquipmentInventory) as EquipmentInventoryPage;           
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(geozone);

            Step("2. Verify The Latitude Minimum and Longitude Minimum are set by smaller than the minimum values of Latitude and Longitude in csv file");
            var latMinValue = decimal.Parse(latMin);
            var longMinValue = decimal.Parse(longMin);
            var actualLatMinValue = decimal.Parse(equipmentInventoryPage.GeozoneEditorPanel.GetLatitudeMinimumValue().Replace(" °", ""));
            var actualLongMinValue = decimal.Parse(equipmentInventoryPage.GeozoneEditorPanel.GetLongitudeMinimumValue().Replace(" °", ""));
            VerifyTrue("2. Verify Latitude Minimum is smaller than the minimum values of Latitude in csv file", actualLatMinValue < latMinValue, latMinValue, actualLatMinValue);
            VerifyTrue("2. Verify Longitude Minimum is smaller than the minimum values of Longitude in csv file", actualLongMinValue < longMinValue, longMinValue, actualLongMinValue);

            Step("3. Verify The Latitude Maximum and Longitude Maximum are set by greater than the maximum values of Latitude and Longitude in csv file");
            var latMaxValue = decimal.Parse(latMax);
            var longMaxValue = decimal.Parse(longMax);
            var actualLatMaxValue = decimal.Parse(equipmentInventoryPage.GeozoneEditorPanel.GetLatitudeMaximumValue().Replace(" °", ""));
            var actualLongMaxValue = decimal.Parse(equipmentInventoryPage.GeozoneEditorPanel.GetLongitudeMaximumValue().Replace(" °", ""));
            VerifyTrue("3. Verify Latitude Maximum is greater than the maximum values of Latitude in csv file", latMaxValue < actualLatMaxValue, latMaxValue, actualLatMaxValue);
            VerifyTrue("3. Verify Longitude Maximum is greater than the maximum values of Longitude in csv file", longMaxValue < actualLongMaxValue, longMaxValue, actualLongMaxValue);

            Step("4. Select the 1st imported device");
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(streetlight1);
            equipmentInventoryPage.WaitForEditorPanelDisplayed();

            Step("5. Verify The device displays on the map with correct Latitude and Longitude");
            var streetlightLat1Value = streetlightLat1 + " °";
            var streetlightLong1Value = streetlightLong1 + " °";
            VerifyEqual("5. Verify Latitude is correct in CSV", streetlightLat1Value, equipmentInventoryPage.StreetlightEditorPanel.GetLatitudeValue());
            VerifyEqual("5. Verify Longitude is correct in CSV", streetlightLong1Value, equipmentInventoryPage.StreetlightEditorPanel.GetLongitudeValue());

            Step("6. Select the 2st imported device");
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(streetlight2);
            equipmentInventoryPage.WaitForEditorPanelDisplayed();

            Step("7. Verify The device displays on the map with correct Latitude and Longitude");
            var streetlightLat2Value = streetlightLat2 + " °";
            var streetlightLong2Value = streetlightLong2 + " °";
            VerifyEqual("7. Verify Latitude is correct in CSV", streetlightLat2Value, equipmentInventoryPage.StreetlightEditorPanel.GetLatitudeValue());
            VerifyEqual("7. Verify Longitude is correct in CSV", streetlightLong2Value, equipmentInventoryPage.StreetlightEditorPanel.GetLongitudeValue());
            
            try
            {
                Step("8. Delete test data after testcase is done");
                DeleteGeozone(geozone);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("SC-912 Equipment Inventory - Devices aren't removed from the map when a geozone is removed")]
        public void SC_912()
        {
            var geozone = SLVHelper.GenerateUniqueName("GZNSC912");
            var newGeozone = SLVHelper.GenerateUniqueName("GZNSC91201");
            var controller = SLVHelper.GenerateUniqueName("CLSC912");
            var streetlight = SLVHelper.GenerateUniqueName("SLSC912");

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            Step("-> Create data for testing");
            DeleteGeozones("GZNSC912*");
            CreateNewGeozone(geozone);
            CreateNewController(controller, geozone);

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.EquipmentInventory);

            Step("1. Go to Equipment Inventory app");           
            var equipmentInventoryPage = desktopPage.GoToApp(App.EquipmentInventory) as EquipmentInventoryPage;

            Step("2. Select Add Geozone and complete add a geozone");
            equipmentInventoryPage.CreateGeozone(newGeozone, geozone, ZoomGLLevel.m500);

            Step("3. Select the newly added geozone");
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(newGeozone);
            equipmentInventoryPage.WaitForGeozoneEditorPanelDisplayed();

            Step("4. Select Add Device and add a new streetlight to the geozone");
            equipmentInventoryPage.CreateDevice(DeviceType.Streetlight, streetlight, controller, streetlight, "ABEL-Vigilon A[Dimmable ballast]");

            Step("5. Verify The streetlight is displayed on the map");
            VerifyEqual("5. Verify The streetlight is displayed on the map", true, equipmentInventoryPage.Map.HasSelectedDevicesInMapGL());

            Step("6. Select the geozone again");
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(newGeozone);
            equipmentInventoryPage.WaitForGeozoneEditorPanelDisplayed();

            Step("7. Press Delete button to delete geozone");
            equipmentInventoryPage.GeozoneEditorPanel.ClickDeleteButton();
            equipmentInventoryPage.WaitForPopupDialogDisplayed();

            Step("8. Verify A confirmation pop-up is displayed.");
            Step(" - Would you like to delete SC-912 geozone and all sub geoZones and equipments ?");
            var expectedMessage = string.Format("Would you like to delete {0} geozone and all sub geoZones and equipments ?", newGeozone);
            VerifyEqual("8. Verify A confirmation pop-up is displayed", expectedMessage, equipmentInventoryPage.Dialog.GetMessageText());

            Step("9. Select No");
            equipmentInventoryPage.Dialog.ClickNoButton();
            equipmentInventoryPage.WaitForPopupDialogDisappeared();

            Step("10. Verify the streetlight is still displayed on the map");
            VerifyEqual("10. Verify The streetlight is displayed on the map", true, equipmentInventoryPage.Map.HasSelectedDevicesInMapGL());

            Step("11. Press Delete button again");
            equipmentInventoryPage.GeozoneEditorPanel.ClickDeleteButton();
            equipmentInventoryPage.WaitForPopupDialogDisplayed();

            Step("12. Select Yes");
            equipmentInventoryPage.Dialog.ClickYesButton();
            equipmentInventoryPage.WaitForPreviousActionComplete();            
            equipmentInventoryPage.WaitForPopupDialogDisappeared();
            equipmentInventoryPage.WaitForHeaderMessageDisappeared();

            Step("13. Verify the streetlight was deleted and was not displayed on the map.");
            VerifyEqual("13. Verify The streetlight is displayed on the map", false, equipmentInventoryPage.Map.HasSelectedDevicesInMapGL());

            try
            {
                DeleteGeozone(geozone);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("SC-917 WebGL - Some skins are missing map layers")]
        public void SC_917()
        {
            var requiredApps = new string[] { App.EquipmentInventory, App.RealTimeControl, App.FailureTracking };

            Step("**** Precondition ****");            
            Step(" - Log in with User using Skin: 'silverspring' and having all WebGL applications: Equipment Inventory, Failure Tracking, Real Time Control");
            Step("**** Precondition ****\n");

            var userModel = CreateNewProfileAndUser(skin: "silverspring");
            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.Users);
            var usersPage = desktopPage.GoToApp(App.Users) as UsersPage;
            usersPage.UserProfileListPanel.SelectProfile(userModel.Profile);
            usersPage.UserProfileDetailsPanel.UncheckAllApps();
            usersPage.UserProfileDetailsPanel.CheckApps(requiredApps);
            usersPage.UserProfileDetailsPanel.ClickSaveButton();
            usersPage.WaitForPreviousActionComplete();
            usersPage.WaitForHeaderMessageDisappeared();
            
            //Login with new user
            desktopPage = SLVHelper.LogoutAndLogin(usersPage, userModel.Username, userModel.Password);
            var isAppInstalled = desktopPage.InstallAppsIfNotExist(requiredApps);
            if (isAppInstalled) desktopPage = Browser.RefreshLoggedInCMS();

            Step("1. Select a following application in the list:");
            Step(" - Equipment Inventory");
            Step(" - Failure Tracking");
            Step(" - Real-time Control");
            Step("2. Expected The WebGL is loaded successfully without any crashes");
            Step("3. Repeat the test with all WebGL applications");
            var equipmentInventoryPage = desktopPage.GoToApp(App.EquipmentInventory) as EquipmentInventoryPage;
            Wait.ForSeconds(4);
            VerifyEqual("3. Verify The WebGL is loaded successfully without any crashes(Spinning wheel is disappeared)", false, equipmentInventoryPage.IsLoaderSpinDisplayed());

            var realtimeControlPage = equipmentInventoryPage.AppBar.SwitchTo(App.RealTimeControl) as RealTimeControlPage;
            Wait.ForSeconds(4);
            VerifyEqual("3. Verify The WebGL is loaded successfully without any crashes (Spinning wheel is disappeared)",false, realtimeControlPage.IsLoaderSpinDisplayed());

            var failureTrackingPage = realtimeControlPage.AppBar.SwitchTo(App.FailureTracking) as FailureTrackingPage;
            Wait.ForSeconds(4);
            VerifyEqual("3. Verify The WebGL is loaded successfully without any crashes (Spinning wheel is disappeared)", false, failureTrackingPage.IsLoaderSpinDisplayed());

            try
            {
                Step("** Delete the testing profile and user after testing **");
                DeleteUserAndProfile(userModel);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("SC-735 - Device History & Data History - Table views - 'Server Response' should be replaced by 'Generated in * seconds'")]
        public void SC_735()
        {
            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.DeviceHistory, App.DataHistory);

            var expectedPattern = "Generated in .* seconds";

            Step("1. Go to Device History app");
            var deviceHistoryPage = desktopPage.GoToApp(App.DeviceHistory) as DeviceHistoryPage;

            Step("2. Verify The text below the table displays 'Generated in <#> seconds'");
            deviceHistoryPage.GridPanel.WaitForLeftFooterTextDisplayed();
            var gridDataList1 = deviceHistoryPage.GridPanel.GetListOfColumnData("Device");
            var leftFooterText1 = deviceHistoryPage.GridPanel.GetFooterLeftText();
            VerifyEqual("2. Verify The text below the table displays 'Generated in <#> seconds'", true, Regex.IsMatch(leftFooterText1, expectedPattern));
            
            Step("3. Select a subgeozone of GeoZones");
            deviceHistoryPage.GeozoneTreeMainPanel.SelectNode("Real Time Control Area");

            Step("4. Verify The table is refreshed and the text displays 'Generated in <#> seconds'");
            deviceHistoryPage.GridPanel.WaitForLeftFooterTextDisplayed();
            var gridDataList2 = deviceHistoryPage.GridPanel.GetListOfColumnData("Device");
            var leftFooterText2 = deviceHistoryPage.GridPanel.GetFooterLeftText();
            VerifyEqual("4. Verify The table is refreshed", true, gridDataList1.Count != gridDataList2.Count);
            VerifyEqual("4. Verify The text displays 'Generated in <#> seconds'", true, Regex.IsMatch(leftFooterText2, expectedPattern));

            Step("5. Press Application Switcher in the top left corner of the screen");
            Step("6. Select Data History app");
            var dataHistoryPage = deviceHistoryPage.AppBar.SwitchTo(App.DataHistory) as DataHistoryPage;

            Step("7. Verify The text below the table displays 'Generated in <#> seconds'");
            dataHistoryPage.GridPanel.WaitForLeftFooterTextDisplayed();
            var gridDataList3 = dataHistoryPage.GridPanel.GetListOfColumnData("Device");
            var leftFooterText3 = dataHistoryPage.GridPanel.GetFooterLeftText();
            VerifyEqual("7. Verify The text below the table displays 'Generated in <#> seconds'", true, Regex.IsMatch(leftFooterText3, expectedPattern));

            Step("8. Select the root geozone: GeoZones");
            deviceHistoryPage.GeozoneTreeMainPanel.SelectNode(Settings.RootGeozoneName);

            Step("9. Verify The table is refreshed and the text displays 'Generated in <#> seconds'");
            dataHistoryPage.GridPanel.WaitForLeftFooterTextDisplayed();
            var gridDataList4 = dataHistoryPage.GridPanel.GetListOfColumnData("Device");
            var leftFooterText4 = dataHistoryPage.GridPanel.GetFooterLeftText();
            VerifyEqual("9. Verify The table is refreshed", true, gridDataList3.Count != gridDataList4.Count);
            VerifyEqual("9. Verify The text below the table displays 'Generated in <#> seconds'", true, Regex.IsMatch(leftFooterText4, expectedPattern));
        }

        [Test, DynamicRetry]
        [Description("SC-939 - GL apps have incorrect names in French")]
        public void SC_939()
        {
            string language = "French";
            var equipmentInventoryFrench = SLVHelper.ConvertAppName(App.EquipmentInventory, language);
            var realtimeControlFrench = SLVHelper.ConvertAppName(App.RealTimeControl, language);
            var failureTrackingFrench = SLVHelper.ConvertAppName(App.FailureTracking, language);
            var requiredApps = new string[] { equipmentInventoryFrench, realtimeControlFrench, failureTrackingFrench};            

            Step("**** Precondition ****");
            Step(" - Login with user using France language");
            Step(" - Make sure all the following apps are installed before testing: 'Equipment Inventory', 'Real Time Control', 'Failure Tracking'");
            Step("**** Precondition ****\n");

            var userModel = CreateNewProfileAndUser(language: "fr_FR");
            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.Users);
            var usersPage = desktopPage.GoToApp(App.Users) as UsersPage;
            usersPage.UserProfileListPanel.SelectProfile(userModel.Profile);
            usersPage.UserProfileDetailsPanel.UncheckAllApps();
            usersPage.UserProfileDetailsPanel.CheckApps(App.EquipmentInventory, App.RealTimeControl, App.FailureTracking, App.Users);
            usersPage.UserProfileDetailsPanel.ClickSaveButton();
            usersPage.WaitForPreviousActionComplete();
            usersPage.WaitForHeaderMessageDisappeared();

            //Login with new user
            desktopPage = SLVHelper.LogoutAndLogin(usersPage, userModel.Username, userModel.Password);
            desktopPage.InstallAppsIfNotExist(requiredApps);

            Step("1. Go to 'Équipements' app");
            var equipmentInventoryPage = desktopPage.GoToApp(App.EquipmentInventory) as EquipmentInventoryPage;
          
            Step("2. Verify The text 'Équipements' displays on the top left corner of the screen and on the title of Geozone tree panel");
            VerifyEqual("2. Verify The text 'Équipements' displays on on the title of Geozone tree panel", equipmentInventoryFrench, equipmentInventoryPage.GeozoneTreeMainPanel.GetPanelTitleText());
            VerifyEqual("2. Verify The text 'Équipements' displays on the top left corner of the screen", equipmentInventoryFrench, equipmentInventoryPage.AppBar.GetCurrentAppSwitcher());            
            
            Step("3. Go to 'Contrôle Temps-Réel' app");           
            var realtimeControlPage = equipmentInventoryPage.AppBar.SwitchTo(App.RealTimeControl, language) as RealTimeControlPage;            

            Step("4. Verify The text 'Contrôle Temps-Réel' displays on the top left corner of the screen and on the title of Geozone tree panel");
            VerifyEqual("4. Verify The text 'Contrôle Temps-Réel' displays on on the title of Geozone tree panel", realtimeControlFrench, realtimeControlPage.GeozoneTreeMainPanel.GetPanelTitleText());
            VerifyEqual("4. Verify The text 'Contrôle Temps-Réel' displays on the top left corner of the screen", realtimeControlFrench, realtimeControlPage.AppBar.GetCurrentAppSwitcher());
                        
            Step("5. Go to 'Suivi de Panne' app");           
            var failureTrackingPage = realtimeControlPage.AppBar.SwitchTo(App.FailureTracking, language) as FailureTrackingPage;

            Step("6. Verify The text 'Suivi de Panne' displays on the top left corner of the screen and on the title of Geozone tree panel");
            VerifyEqual("6. Verify The text 'Suivi de Panne' displays on on the title of Geozone tree panel", failureTrackingFrench, failureTrackingPage.GeozoneTreeMainPanel.GetPanelTitleText());
            VerifyEqual("6. Verify The text 'Suivi de Panne' displays on the top left corner of the screen", failureTrackingFrench, realtimeControlPage.AppBar.GetCurrentAppSwitcher());
                       
            Step("7. Go to 'Utilisateurs' and select the profile of the current user");
            usersPage = failureTrackingPage.AppBar.SwitchTo(App.Users, language) as UsersPage;
            usersPage.UserProfileListPanel.SelectProfile(userModel.Profile);

            Step("8. Verify The texts 'Équipements', 'Contrôle Temps-Réel', 'Suivi de Panne' displays for apps.");
            var allApps = usersPage.UserProfileDetailsPanel.GetAllAppsName();            
            VerifyTrue("8. Verify The texts 'Équipements', 'Contrôle Temps-Réel', 'Suivi de Panne' displays for apps.", allApps.CheckIfIncluded(requiredApps), string.Join(", ", requiredApps), string.Join(", ", allApps));            
            
            try
            {
                Step("** Delete the testing profile and user after testing **");
                DeleteUserAndProfile(userModel);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("SC-218 - Report Manager - Editor of scheduler in reports application should be done in one TimeZone properly")]
        public void SC_218()
        {
            var testData = GetTestDataOfSC_218();
            var reportTypes = testData["ReportTypes"] as List<string>;

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.ReportManager);
            
            Step("1. Go to Report Manager app");
            var reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;

            foreach (var reportType in reportTypes)
            {
                var reportName = SLVHelper.GenerateUniqueName(string.Format("[{0}][{1}][{2}] {3}", Browser.Name, Settings.Users["DefaultTest"].Username, Settings.CurrentTestWebDriverKeyName, reportType));
                Step("2. Press Add button to create a new report with a type in the list.");
                reportManagerPage.GridPanel.ClickAddReportToolbarButton();
                reportManagerPage.WaitForPreviousActionComplete();
                reportManagerPage.ReportEditorPanel.WaitForPanelLoaded();

                Step("3. Input all required fields and change TimeZone value to a random value");
                reportManagerPage.ReportEditorPanel.EnterNameInput(reportName);
                reportManagerPage.ReportEditorPanel.SelectTypeDropDown(reportType);
                reportManagerPage.ReportEditorPanel.WaitForPreviousActionComplete();

                reportManagerPage.ReportEditorPanel.SelectTab("Scheduler");               
                reportManagerPage.ReportEditorPanel.SelectRandomTimeZoneDropDown();
                var notedTimezone = reportManagerPage.ReportEditorPanel.GetTimezoneValue();

                Step("4. Press Save button and reload the report");
                reportManagerPage.ReportEditorPanel.ClickSaveButton();
                reportManagerPage.WaitForPreviousActionComplete();
                reportManagerPage.WaitForReportDetailsDisappeared();
                reportManagerPage.GridPanel.ClickGridRecord(reportName);
                reportManagerPage.WaitForPreviousActionComplete();
                reportManagerPage.WaitForReportDetailsDisplayed();

                Step("5. Verify The timezone is saved and reloaded correctly.");
                reportManagerPage.ReportEditorPanel.SelectTab("Scheduler");
                VerifyEqual(string.Format("[{0}] 5. Verify The timezone is saved and reloaded correctly.", reportType), notedTimezone, reportManagerPage.ReportEditorPanel.GetTimezoneValue());

                Step("6. Update the timezone value to another value, then save and reload the report");
                reportManagerPage.ReportEditorPanel.SelectRandomTimeZoneDropDown();
                notedTimezone = reportManagerPage.ReportEditorPanel.GetTimezoneValue();
                reportManagerPage.ReportEditorPanel.ClickSaveButton();
                reportManagerPage.WaitForPreviousActionComplete();
                reportManagerPage.WaitForReportDetailsDisappeared();
                reportManagerPage.GridPanel.ClickGridRecord(reportName);
                reportManagerPage.WaitForPreviousActionComplete();
                reportManagerPage.WaitForReportDetailsDisplayed();

                Step("7. Verify The timezone is updated correctly.");
                reportManagerPage.ReportEditorPanel.SelectTab("Scheduler");
                VerifyEqual(string.Format("[{0}] 7. Verify The timezone is updated correctly.", reportType), notedTimezone, reportManagerPage.ReportEditorPanel.GetTimezoneValue());

                Step("8. Delete the report after test is done.");
                reportManagerPage.DeleteCurrentReport();

                Step("9. Do the test with another types in the precondition list until test is done.");
            }
        }

        [Test, DynamicRetry]
        [Description("SC-733 - Equipment Inventory - Creating a controller device in equipment inventory while zoomed out results in tons of console errors and fails")]
        public void SC_733()
        {
            var geozone = SLVHelper.GenerateUniqueName("GZNSC733");
            var newGeozone = SLVHelper.GenerateUniqueName("GZNSC73301");
            var newController = SLVHelper.GenerateUniqueName("CTRL");

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            Step("-> Create data for testing");
            DeleteGeozones("GZNSC733*");
            CreateNewGeozone(geozone);

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.EquipmentInventory);

            Step("1. Go to Equipment Inventory app");
            var equipmentInventoryPage = desktopPage.GoToApp(App.EquipmentInventory) as EquipmentInventoryPage;

            Step("2. Create new geozone and set the zoom level of the map all the way out.");
            equipmentInventoryPage.CreateGeozone(newGeozone, geozone, ZoomGLLevel.km3000);

            Step("3. Select the new created geozone and press Add Device button");
            Step("4. Select Controller Device and input Name and other required fields, then position it on anywhere in the map.");
            Step("5. Press Save button on Controller Editor panel");
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(newGeozone);
            equipmentInventoryPage.WaitForGeozoneEditorPanelDisplayed();
            equipmentInventoryPage.CreateDevice(DeviceType.Controller, newController, newController, gatewayHostName: "localhost");
            
            Step("6. Verify The device is added successfully and displays in the geozone tree");
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(newGeozone);
            equipmentInventoryPage.WaitForGeozoneEditorPanelDisplayed();

            var devices = equipmentInventoryPage.GeozoneTreeMainPanel.GetChildNodeNamesOfSelectedNode(NodeType.Controller);
            VerifyEqual("6. Verify The device is added successfully and displays in the geozone tree", true, devices.Contains(newController));

            Step("7. Refresh the page and go to Equipment Inventory and select the newly created geozone");
            desktopPage = Browser.RefreshLoggedInCMS();
            equipmentInventoryPage = desktopPage.GoToApp(App.EquipmentInventory) as EquipmentInventoryPage;

            Step("8. Verify The Geozone Editor panel is displayed without any errors");
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(geozone + @"\" + newGeozone);
            Wait.ForSeconds(4);
            VerifyEqual("8. Verify The Geozone Editor panel is displayed without any errors", true, equipmentInventoryPage.IsGeozoneEditorPanelDisplayed());
            VerifyEqual("8. Verify The Geozone Editor panel is displayed without any errors", newGeozone, equipmentInventoryPage.GeozoneEditorPanel.GetNameValue());

            Step("9. Select the newly created device");
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(newController);
            Wait.ForSeconds(4);

            Step("10. Verify The Controller Editor panel is displayed without any errors");
            VerifyEqual("10. Verify The Controller Editor panel is displayed without any errors", true, equipmentInventoryPage.IsDeviceEditorPanelDisplayed());
            VerifyEqual("10. Verify The Controller Editor panel is displayed without any errors", newController, equipmentInventoryPage.ControllerEditorPanel.GetNameValue());
            
            try
            {
                DeleteGeozone(newGeozone);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("Errors displaying when multiple dimming groups of same name in Scheduling Manager")]
        public void SC_957()
        {
            var requiredApps = new string[] { App.Users, App.EquipmentInventory, App.SchedulingManager };
            var controlProgramName = SLVHelper.GenerateUniqueName("CPSC957");
            var newGeozoneName = SLVHelper.GenerateUniqueName("GZNSC957");

            Step("**** Precondition ****");
            Step(" - There is an existing dimming group (Control Program) named ABC belonging to GeoZones");
            Step(" - Login by a user belonging to a subgeozone of GeoZones (need to create subgeozone first, create user profile and user for that subgeozone)");
            Step("**** Precondition ****\n");

            Step("-> Create data for testing");
            DeleteGeozones("GZNSC957*");
            CreateNewControlProgram(controlProgramName, "T_12_12", SLVHelper.GenerateHexColor(), string.Format("Created for '{0}' geozone", Settings.RootGeozoneName));
            CreateNewGeozone(newGeozoneName);
            var userModel = CreateNewProfileAndUser(geozone: newGeozoneName);

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(userModel.Username, userModel.Password);
            desktopPage.InstallAppsIfNotExist(requiredApps);

            Step("1. Go to Scheduling Manager app");
            var schedulingManagerPage = desktopPage.GoToApp(App.SchedulingManager) as SchedulingManagerPage;

            Step("2. Create a new Control Program with the same name of the existing control program");
            var description = string.Format("Created for '{0}' geozone", newGeozoneName);
            schedulingManagerPage.SchedulingManagerPanel.SelectTab("Control program");
            schedulingManagerPage.SchedulingManagerPanel.ClickAddControlProgramButton();
            schedulingManagerPage.WaitForPreviousActionComplete();
            schedulingManagerPage.ControlProgramEditorPanel.EnterNameInput(controlProgramName);
            schedulingManagerPage.ControlProgramEditorPanel.EnterDescriptionInput(description);
            schedulingManagerPage.ControlProgramEditorPanel.ClickSaveButton();
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("3. Verify The control program is created successfully");
            VerifyEqual("3. Verify The control program is created successfully", controlProgramName, schedulingManagerPage.SchedulingManagerPanel.GetSelectedControlProgramName());

            Step("4. Refresh the system and go back Scheduling Manager and select the newly created Control Program");
            desktopPage = Browser.RefreshLoggedInCMS();
            schedulingManagerPage = desktopPage.GoToApp(App.SchedulingManager) as SchedulingManagerPage;
            schedulingManagerPage.SchedulingManagerPanel.SelectTab("Control program");
            schedulingManagerPage.SchedulingManagerPanel.SelectControlProgram(controlProgramName, newGeozoneName);
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("5. Verify The Control Program Editor displays successfully");
            VerifyEqual("3. Verify The Control Program Editor displays successfull", controlProgramName, schedulingManagerPage.ControlProgramEditorPanel.GetNameValue());
            VerifyEqual("3. Verify The Control Program Editor displays successfull", description, schedulingManagerPage.ControlProgramEditorPanel.GetDescriptionValue());
            
            Step("6. Select the Control Program belonging to GeoZones (the same name with the created Control Program)");
            schedulingManagerPage.SchedulingManagerPanel.SelectControlProgram(controlProgramName, Settings.RootGeozoneName);
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("7. Verify The Control Program Editor displays");
            Step(" o Name and Description are read only");
            Step(" o No Save button");
            VerifyEqual("7. Verify Name is read only", true, schedulingManagerPage.ControlProgramEditorPanel.IsNameInputReadOnly());
            VerifyEqual("7. Verify Description is read only", true, schedulingManagerPage.ControlProgramEditorPanel.IsDescriptionInputReadOnly());
            VerifyEqual("7. Verify No Save button", false, schedulingManagerPage.ControlProgramEditorPanel.IsSaveButtonVisible());
            
            Step("8. Select back the created Control Program");
            schedulingManagerPage.SchedulingManagerPanel.SelectControlProgram(controlProgramName, newGeozoneName);
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("9. Update Description and save the change.");
            description += " updated";
            schedulingManagerPage.ControlProgramEditorPanel.EnterDescriptionInput(description);
            schedulingManagerPage.ControlProgramEditorPanel.ClickSaveButton();
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("10. Press Application Switcher and select Equipment Inventory app");
            var equipmentInventoryPage = schedulingManagerPage.AppBar.SwitchTo(App.EquipmentInventory) as EquipmentInventoryPage;

            Step("11. Press Application Switcher and select Scheduling Manager app"); 
            schedulingManagerPage = equipmentInventoryPage.AppBar.SwitchTo(App.SchedulingManager) as SchedulingManagerPage;

            Step("12. Verify The Control Program Editor displays successfully and Description is updated.");
            VerifyEqual("12. Verify The Control Program Editor displays successfully and Description is updated.", description, schedulingManagerPage.ControlProgramEditorPanel.GetDescriptionValue());                                             
            
            try
            {
                Step("13. Delete test data after testcase is done");
                DeleteControlProgram(controlProgramName);
                DeleteUserAndProfile(userModel);
                DeleteGeozone(newGeozoneName);
            }
            catch { }
        }     

        #endregion //Test Cases

        #region Private methods        

        /// <summary>
        /// Make sure 2 compared tables proper formatted before compared
        /// </summary>
        /// <param name="gridDataTable"></param>
        /// <param name="csvDataTable"></param>
        private void FormatAdvancedSearchDataTables(ref DataTable gridDataTable, ref DataTable csvDataTable)
        {
            foreach (var col in gridDataTable.Columns.Cast<DataColumn>().ToList())
            {
                if (col.ColumnName.Equals("Lamp Type"))
                    continue;
                gridDataTable.Columns.Remove(col.ColumnName);
            }

            foreach (var col in csvDataTable.Columns.Cast<DataColumn>().ToList())
            {
                if (col.ColumnName.Equals("brandId") || col.ColumnName.Equals("LampType") || col.ColumnName.Equals("Lamp Type"))
                    continue;
                csvDataTable.Columns.Remove(col.ColumnName);
            }

            foreach (DataColumn col in gridDataTable.Columns)
            {
                foreach (DataRow row in gridDataTable.Rows)
                {
                    if (row[col.ColumnName].ToString() == "---")
                        row[col.ColumnName] = string.Empty;
                }
            }
        }

        #region Verify methods

        private void VerifyLampBurningHoursAndMainsVoltage(RealTimeControlPage page)
        {
            var burningHoursValue = page.StreetlightWidgetPanel.GetLampBurningHoursValueText();
            var lastBurningHoursChar = burningHoursValue[burningHoursValue.Length - 1];
            VerifyEqual("Verify Lamp burning hours ends with 'h'", "h", lastBurningHoursChar.ToString());
            var mainsVoltageValue = page.StreetlightWidgetPanel.GetMainsVoltageValueText();
            var lastMainsVoltageChar = mainsVoltageValue[mainsVoltageValue.Length - 1];
            VerifyEqual("Verify Mains Voltage (V) ends with 'V'", "V", lastMainsVoltageChar.ToString());
        }

        private void VerifyAlarmGridSorted(AlarmsPage page, string columnName)
        {
            var listSorted = page.GridPanel.GetListOfColumnSorted();
            var strDataList = page.GridPanel.GetListOfColumnData(columnName);
            strDataList = strDataList.ConvertAll(p => p.ToLower());
            var sortedColumn = listSorted.FirstOrDefault();
            VerifyTrue("[SC-722] Verify Sort indicator appears next to the clicked column header", listSorted.Count == 1 && sortedColumn.Equals(columnName), columnName, sortedColumn);
            var isSorted = strDataList.IsIncreasing() || strDataList.IsDecreasing();
            if (columnName == "Creation Date" || columnName == "Last Change")
            {
                var dateList = strDataList.ToDateList("dd/MM/yyyy HH:mm:ss");
                isSorted = dateList.IsIncreasing();
            }
            VerifyEqual(string.Format("[SC-722] Verify Data rows of '{0}' are sorted correctly", columnName), true, isSorted);
        }

        #endregion //Verify methods

        #region Input XML data

        private Dictionary<string, object> GetCommonTestData()
        {
            var realtimeGeozone = Settings.CommonTestData[0];
            var testData = new Dictionary<string, object>();
            testData.Add("Geozone", realtimeGeozone.Path);
            var controller = realtimeGeozone.Devices.Where(p => p.Type == DeviceType.Controller && p.Status == DeviceStatus.Working).FirstOrDefault();
            testData.Add("Controller", controller);
            var streetlights = realtimeGeozone.Devices.Where(p => p.Type == DeviceType.Streetlight && p.Status == DeviceStatus.Working && string.IsNullOrEmpty(p.Cluster)).ToList();
            testData.Add("Streetlights", streetlights);

            return testData;
        }

        /// <summary>
        /// Read test data for SLV-1619
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfSLV_1619()
        {
            var testCaseName = "SLV_1619";
            var testData = new Dictionary<string, string>();
            var xmlUtility = new XmlUtility(Settings.JIRA_COVERAGE_TEST_DATA_FILE_PATH);
            var controller1Info = xmlUtility.GetSingleNode(string.Format(Settings.JIRA_COVERAGE_XPATH_PREFIX, testCaseName, "Controller1"));
            testData.Add("ControllerName1", controller1Info.GetAttrVal("name"));
            testData.Add("ControllerId1", controller1Info.GetAttrVal("id"));
            var controller2Info = xmlUtility.GetSingleNode(string.Format(Settings.JIRA_COVERAGE_XPATH_PREFIX, testCaseName, "Controller2"));
            testData.Add("ControllerName2", controller2Info.GetAttrVal("name"));
            testData.Add("ControllerId2", controller2Info.GetAttrVal("id"));

            return testData;
        }

        /// <summary>
        /// Read test data for SLV-1680
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfSLV_1680()
        {
            var testCaseName = "SLV_1680";
            var xmlUtility = new XmlUtility(Settings.JIRA_COVERAGE_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, string>();
            testData.Add("GeoZone", xmlUtility.GetSingleNodeText(string.Format(Settings.JIRA_COVERAGE_XPATH_PREFIX, testCaseName, "GeoZone")));

            return testData;
        }

        /// <summary>
        /// Read test data for SLV-1799
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, object> GetTestDataOfSLV_1799()
        {
            return GetCommonTestData();
        }

        /// <summary>
        /// Read test data for SLV-1800
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, object> GetTestDataOfSLV_1800()
        {
            var realtimeGeozone = Settings.CommonTestData[0];
            var testData = new Dictionary<string, object>();
            testData.Add("GeoZone", realtimeGeozone.Path);
            var streetlights = realtimeGeozone.Devices.Where(p => p.Type == DeviceType.Streetlight && p.Status == DeviceStatus.Working && string.IsNullOrEmpty(p.Cluster)).ToList();
            testData.Add("Streetlights", streetlights);

            return testData;
        }

        /// <summary>
        /// Read test data for SLV-2111
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, object> GetTestDataOfSLV_2111()
        {
            var testCaseName = "SLV_2111";
            var xmlUtility = new XmlUtility(Settings.JIRA_COVERAGE_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, object>();
            testData.Add("GeoZone", xmlUtility.GetSingleNodeText(string.Format(Settings.JIRA_COVERAGE_XPATH_PREFIX, testCaseName, "GeoZone")));
            testData.Add("ReportPrefix", xmlUtility.GetSingleNodeText(string.Format(Settings.JIRA_COVERAGE_XPATH_PREFIX, testCaseName, "ReportPrefix")));
            testData.Add("ReportType", xmlUtility.GetSingleNodeText(string.Format(Settings.JIRA_COVERAGE_XPATH_PREFIX, testCaseName, "ReportType")));
            testData.Add("PropertiesTab", xmlUtility.GetChildNodesText(string.Format(Settings.JIRA_COVERAGE_XPATH_PREFIX, testCaseName, "PropertiesTab")));
            testData.Add("SchedulerTab", xmlUtility.GetChildNodesText(string.Format(Settings.JIRA_COVERAGE_XPATH_PREFIX, testCaseName, "SchedulerTab")));
            testData.Add("MailTab", xmlUtility.GetChildNodesText(string.Format(Settings.JIRA_COVERAGE_XPATH_PREFIX, testCaseName, "MailTab")));

            return testData;
        }

        /// <summary>
        /// Read test data for SLV-2227
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfSLV_2227()
        {
            var testCaseName = "SLV_2227";
            var xmlUtility = new XmlUtility(Settings.JIRA_COVERAGE_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, string>();
            testData.Add("SearchName", xmlUtility.GetSingleNodeText(string.Format(Settings.JIRA_COVERAGE_XPATH_PREFIX, testCaseName, "SearchName")));

            return testData;
        }

        /// <summary>
        /// Read test data for SLV-1260
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, object> GetTestDataOfSLV_1260()
        {
            var testCaseName = "SLV_1260";
            var xmlUtility = new XmlUtility(Settings.JIRA_COVERAGE_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, object>();
            testData.Add("Device1", xmlUtility.GetSingleNode(string.Format(Settings.JIRA_COVERAGE_XPATH_PREFIX, testCaseName, "Device1")));
            testData.Add("Device2", xmlUtility.GetSingleNode(string.Format(Settings.JIRA_COVERAGE_XPATH_PREFIX, testCaseName, "Device2")));

            return testData;
        }

        /// <summary>
        /// Read test data for SLV-408
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfSLV_408()
        {
            var testCaseName = "SLV_408";
            var xmlUtility = new XmlUtility(Settings.JIRA_COVERAGE_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, string>();
            var timeZone1Info = xmlUtility.GetSingleNode(string.Format(Settings.JIRA_COVERAGE_XPATH_PREFIX, testCaseName, "TimeZone1"));
            testData.Add("TimeZone1Name", timeZone1Info.GetAttrVal("name"));
            testData.Add("TimeZone1Id", timeZone1Info.GetAttrVal("id"));
            var timeZone2Info = xmlUtility.GetSingleNode(string.Format(Settings.JIRA_COVERAGE_XPATH_PREFIX, testCaseName, "TimeZone2"));
            testData.Add("TimeZone2Name", timeZone2Info.GetAttrVal("name"));
            testData.Add("TimeZone2Id", timeZone2Info.GetAttrVal("id"));

            return testData;
        }

        /// <summary>
        /// Read test data for SLV-1814
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfSLV_1814()
        {
            var testCaseName = "SLV_1814";
            var xmlUtility = new XmlUtility(Settings.JIRA_COVERAGE_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, string>();
            testData.Add("SearchName", xmlUtility.GetSingleNodeText(string.Format(Settings.JIRA_COVERAGE_XPATH_PREFIX, testCaseName, "SearchName")));
            testData.Add("GeoZone", xmlUtility.GetSingleNodeText(string.Format(Settings.JIRA_COVERAGE_XPATH_PREFIX, testCaseName, "GeoZone")));
            testData.Add("ExportFilePattern", xmlUtility.GetSingleNodeText(string.Format(Settings.JIRA_COVERAGE_XPATH_PREFIX, testCaseName, "ExportFilePattern")));

            return testData;
        }

        /// <summary>
        /// Read test data for SLV-1740
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfSLV_1740()
        {
            var testCaseName = "SLV_1740";
            var xmlUtility = new XmlUtility(Settings.JIRA_COVERAGE_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, string>();
            testData.Add("Geozone", xmlUtility.GetSingleNodeText(string.Format(Settings.JIRA_COVERAGE_XPATH_PREFIX, testCaseName, "Geozone")));

            return testData;
        }

        // <summary>
        /// Read test data for SLV-1925
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, object> GetTestDataOfSLV_1925()
        {
            var testCaseName = "SLV_1925";
            var xmlUtility = new XmlUtility(Settings.JIRA_COVERAGE_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, object>();
            testData.Add("MapBounds", xmlUtility.GetSingleNode(string.Format(Settings.JIRA_COVERAGE_XPATH_PREFIX, testCaseName, "MapBounds")));
            testData.Add("Device", xmlUtility.GetSingleNodeText(string.Format(Settings.JIRA_COVERAGE_XPATH_PREFIX, testCaseName, "Device")));

            return testData;
        }

        /// <summary>
        /// Read test data for SLV-1954
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfSLV_1954()
        {
            var testCaseName = "SLV_1954";
            var xmlUtility = new XmlUtility(Settings.JIRA_COVERAGE_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, string>();
            testData.Add("SearchName", xmlUtility.GetSingleNodeText(string.Format(Settings.JIRA_COVERAGE_XPATH_PREFIX, testCaseName, "SearchName")));
            testData.Add("GeoZone", xmlUtility.GetSingleNodeText(string.Format(Settings.JIRA_COVERAGE_XPATH_PREFIX, testCaseName, "GeoZone")));

            return testData;
        }

        /// <summary>
        /// Read test data for SLV-1959
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, object> GetTestDataOfSLV_1959()
        {
            var testCaseName = "SLV_1959";
            var xmlUtility = new XmlUtility(Settings.JIRA_COVERAGE_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, object>();
            testData.Add("Device", xmlUtility.GetSingleNode(string.Format(Settings.JIRA_COVERAGE_XPATH_PREFIX, testCaseName, "Device")));

            return testData;
        }

        /// <summary>
        /// Read test data for SLV-1986
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfSLV_1986()
        {
            var testCaseName = "SLV_1986";
            var xmlUtility = new XmlUtility(Settings.JIRA_COVERAGE_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, string>();
            testData.Add("Geozone", xmlUtility.GetSingleNodeText(string.Format(Settings.JIRA_COVERAGE_XPATH_PREFIX, testCaseName, "Geozone")));
            testData.Add("GeozoneImagePathFilteringOn", xmlUtility.GetSingleNodeText(string.Format(Settings.JIRA_COVERAGE_XPATH_PREFIX, testCaseName, "GeozoneImagePathFilteringOn")));
            testData.Add("GeozoneImagePathFilteringOff", xmlUtility.GetSingleNodeText(string.Format(Settings.JIRA_COVERAGE_XPATH_PREFIX, testCaseName, "GeozoneImagePathFilteringOff")));

            return testData;
        }

        /// <summary>
        /// Read test data for SLV-1071
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfSLV_1071()
        {
            var testCaseName = "SLV_1071";
            var xmlUtility = new XmlUtility(Settings.JIRA_COVERAGE_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, string>();
            testData.Add("Geozone", xmlUtility.GetSingleNodeText(string.Format(Settings.JIRA_COVERAGE_XPATH_PREFIX, testCaseName, "Geozone")));

            return testData;
        }

        /// <summary>
        /// Read test data for SLV-1071
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfSLV_1681()
        {
            var testCaseName = "SLV_1681";
            var xmlUtility = new XmlUtility(Settings.JIRA_COVERAGE_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, string>();
            testData.Add("ExpectedExportHeader", xmlUtility.GetSingleNodeText(string.Format(Settings.JIRA_COVERAGE_XPATH_PREFIX, testCaseName, "ExpectedExportHeader")));

            return testData;
        }
        
        /// <summary>
        /// Read test data for SLV-1659
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, object> GetTestDataOfSLV_1659()
        {
            var testCaseName = "SLV_1659";
            var xmlUtility = new XmlUtility(Settings.JIRA_COVERAGE_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, object>();
            var alarmNode = xmlUtility.GetSingleNode(string.Format(Settings.JIRA_COVERAGE_XPATH_PREFIX, testCaseName, "Alarm"));
            testData.Add("Alarm", alarmNode);

            return testData;
        }

        /// <summary>
        /// Read test data for SLV_403
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, object> GetTestDataOfSLV_403()
        {
            var testCaseName = "SLV_403";
            var xmlUtility = new XmlUtility(Settings.JIRA_COVERAGE_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, object>();
            testData.Add("RefreshRates", xmlUtility.GetChildNodesText(string.Format(Settings.JIRA_COVERAGE_XPATH_PREFIX, testCaseName, "RefreshRates")));

            return testData;
        }

        /// <summary>
        /// Read test data for SLV-1689
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, object> GetTestDataOfSLV_1689()
        {
            var testCaseName = "SLV_1689";
            var xmlUtility = new XmlUtility(Settings.JIRA_COVERAGE_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, object>();
            testData.Add("Geozones", xmlUtility.GetChildNodesText(string.Format(Settings.JIRA_COVERAGE_XPATH_PREFIX, testCaseName, "Geozones")));

            return testData;
        }

        /// <summary>
        /// Read test data for SLV-2375
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfSLV_2375()
        {
            var testCaseName = "SLV_2375";
            var xmlUtility = new XmlUtility(Settings.JIRA_COVERAGE_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, string>();
            testData.Add("Geozone", xmlUtility.GetSingleNodeText(string.Format(Settings.JIRA_COVERAGE_XPATH_PREFIX, testCaseName, "Geozone")));
            testData.Add("SearchName", xmlUtility.GetSingleNodeText(string.Format(Settings.JIRA_COVERAGE_XPATH_PREFIX, testCaseName, "SearchName")));

            return testData;
        }        

        /// <summary>
        /// Read test data for SLV-847
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, object> GetTestDataOfSLV_847()
        {
            return GetCommonTestData();
        }

        /// <summary>
        /// Read test data for SLV-1793
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfSLV_1793()
        {
            var testCaseName = "SLV_1793";
            var xmlUtility = new XmlUtility(Settings.JIRA_COVERAGE_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, string>();
            testData.Add("GeozoneA", xmlUtility.GetSingleNodeText(string.Format(Settings.JIRA_COVERAGE_XPATH_PREFIX, testCaseName, "GeozoneA")));
            testData.Add("GeozoneB", xmlUtility.GetSingleNodeText(string.Format(Settings.JIRA_COVERAGE_XPATH_PREFIX, testCaseName, "GeozoneB")));

            return testData;
        }

        /// <summary>
        /// Read test data for SLV-2430
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfSLV_2430()
        {
            var testCaseName = "SLV_2430";
            var xmlUtility = new XmlUtility(Settings.JIRA_COVERAGE_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, string>();
            testData.Add("Geozone", xmlUtility.GetSingleNodeText(string.Format(Settings.JIRA_COVERAGE_XPATH_PREFIX, testCaseName, "Geozone")));

            return testData;
        }

        /// <summary>
        /// Read test data for SLV-965
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfSLV_965()
        {
            var testCaseName = "SLV_965";
            var xmlUtility = new XmlUtility(Settings.JIRA_COVERAGE_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, string>();
            testData.Add("Geozone", xmlUtility.GetSingleNodeText(string.Format(Settings.JIRA_COVERAGE_XPATH_PREFIX, testCaseName, "Geozone")));

            return testData;
        }        

        /// <summary>
        /// Read test data for SLV-2141
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfSLV_2141()
        {
            var testCaseName = "SLV_2141";
            var xmlUtility = new XmlUtility(Settings.JIRA_COVERAGE_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, string>();
            testData.Add("ParentGeozone", xmlUtility.GetSingleNodeText(string.Format(Settings.JIRA_COVERAGE_XPATH_PREFIX, testCaseName, "ParentGeozone")));

            return testData;
        }

        /// <summary>
        /// Read test data for SLV-1664
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfSLV_1664()
        {
            var testCaseName = "SLV_1664";
            var xmlUtility = new XmlUtility(Settings.JIRA_COVERAGE_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, string>();
            testData.Add("ParentGeozone", xmlUtility.GetSingleNodeText(string.Format(Settings.JIRA_COVERAGE_XPATH_PREFIX, testCaseName, "ParentGeozone")));
            testData.Add("OtherGeozone", xmlUtility.GetSingleNodeText(string.Format(Settings.JIRA_COVERAGE_XPATH_PREFIX, testCaseName, "OtherGeozone")));

            return testData;
        }

        /// <summary>
        /// Read test data for SLV-1524
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, object> GetTestDataOfSLV_1524()
        {
            var testCaseName = "SLV_1524";
            var xmlUtility = new XmlUtility(Settings.JIRA_COVERAGE_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, object>();
            testData.Add("Geozones", xmlUtility.GetChildNodesText(string.Format(Settings.JIRA_COVERAGE_XPATH_PREFIX, testCaseName, "Geozones")));

            return testData;
        }

        /// <summary>
        /// Read test data for SLV-854
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfSLV_854()
        {
            var testCaseName = "SLV_854";
            var xmlUtility = new XmlUtility(Settings.JIRA_COVERAGE_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, string>();
            var controllerInfo = xmlUtility.GetSingleNode(string.Format(Settings.JIRA_COVERAGE_XPATH_PREFIX, testCaseName, "Controller"));
            testData.Add("ControllerId", controllerInfo.GetAttrVal("id"));

            return testData;
        }

        /// <summary>
        /// Read test data for SLV-733
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfSLV_733()
        {
            var testCaseName = "SLV_733";
            var xmlUtility = new XmlUtility(Settings.JIRA_COVERAGE_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, string>();
            testData.Add("Geozone", xmlUtility.GetSingleNodeText(string.Format(Settings.JIRA_COVERAGE_XPATH_PREFIX, testCaseName, "Geozone")));

            return testData;
        }

        /// <summary>
        /// Read test data for SLV-2344
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfSLV_2344()
        {
            var testCaseName = "SLV_2344";
            var xmlUtility = new XmlUtility(Settings.JIRA_COVERAGE_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, string>();
            var deviceNode = xmlUtility.GetSingleNode(string.Format(Settings.JIRA_COVERAGE_XPATH_PREFIX, testCaseName, "Device"));

            testData.Add("Device.name", deviceNode.Attributes["name"].Value);
            testData.Add("Device.id", deviceNode.Attributes["id"].Value);
            testData.Add("Device.controller-id", deviceNode.Attributes["controller-id"].Value);
            testData.Add("Device.geozone", deviceNode.Attributes["geozone"].Value);
            testData.Add("Device.failure-name", deviceNode.Attributes["failure-name"].Value);
            testData.Add("Device.failure-id", deviceNode.Attributes["failure-id"].Value);

            return testData;
        }

        /// <summary>
        /// Read test data for SLV-2344
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, object> GetTestDataOfSLV_1000()
        {
            var testCaseName = "SLV_1000";
            var xmlUtility = new XmlUtility(Settings.JIRA_COVERAGE_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, object>();
            var alarmNodes = xmlUtility.GetChildNodes(string.Format(Settings.JIRA_COVERAGE_XPATH_PREFIX, testCaseName, "Alarms"));
            testData.Add("Alarms", alarmNodes);
            return testData;
        }       

        /// <summary>
        /// Read test data for SC-592
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfSC_592()
        {
            var testCaseName = "SC_592";
            var xmlUtility = new XmlUtility(Settings.JIRA_COVERAGE_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, string>();
            var controllerInfo = xmlUtility.GetSingleNode(string.Format(Settings.JIRA_COVERAGE_XPATH_PREFIX, testCaseName, "Controller"));
            testData.Add("ControllerName", controllerInfo.GetAttrVal("name"));
            testData.Add("ControllerId", controllerInfo.GetAttrVal("id"));

            return testData;
        }

        /// <summary>
        /// Read test data for SC-595
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfSC_595()
        {
            var testCaseName = "SC_595";
            var xmlUtility = new XmlUtility(Settings.JIRA_COVERAGE_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, string>();
            testData.Add("Geozone", xmlUtility.GetSingleNodeText(string.Format(Settings.JIRA_COVERAGE_XPATH_PREFIX, testCaseName, "Geozone")));

            return testData;
        }

        /// <summary>
        /// Read test data for SC-382
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfSC_382()
        {
            var testCaseName = "SC_382";
            var xmlUtility = new XmlUtility(Settings.JIRA_COVERAGE_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, string>();
            var controllerInfo = xmlUtility.GetSingleNode(string.Format(Settings.JIRA_COVERAGE_XPATH_PREFIX, testCaseName, "Controller"));
            testData.Add("ControllerName", controllerInfo.GetAttrVal("name"));
            testData.Add("ControllerId", controllerInfo.GetAttrVal("id"));

            return testData;
        }

        /// <summary>
        /// Read test data for SC-463
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfSC_463()
        {
            var testCaseName = "SC_463";
            var xmlUtility = new XmlUtility(Settings.JIRA_COVERAGE_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, string>();
            var controllerInfo = xmlUtility.GetSingleNode(string.Format(Settings.JIRA_COVERAGE_XPATH_PREFIX, testCaseName, "Controller"));
            testData.Add("ControllerName", controllerInfo.GetAttrVal("name"));
            testData.Add("ControllerId", controllerInfo.GetAttrVal("id"));

            return testData;
        }

        /// <summary>
        /// Read test data for SC-322
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfSC_322()
        {
            var testCaseName = "SC_322";
            var xmlUtility = new XmlUtility(Settings.JIRA_COVERAGE_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, string>();
            var controllerInfo = xmlUtility.GetSingleNode(string.Format(Settings.JIRA_COVERAGE_XPATH_PREFIX, testCaseName, "Controller"));
            testData.Add("ControllerName", controllerInfo.GetAttrVal("name"));
            testData.Add("ControllerId", controllerInfo.GetAttrVal("id"));

            return testData;
        }

        /// <summary>
        /// Read test data for SC-10-01
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfSC_10_01()
        {
            var testCaseName = "SC_10_01";
            var xmlUtility = new XmlUtility(Settings.JIRA_COVERAGE_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, string>();
            var controllerInfo = xmlUtility.GetSingleNode(string.Format(Settings.JIRA_COVERAGE_XPATH_PREFIX, testCaseName, "Controller"));
            testData.Add("ControllerName", controllerInfo.GetAttrVal("name"));
            testData.Add("ControllerId", controllerInfo.GetAttrVal("id"));

            return testData;
        }

        /// <summary>
        /// Read test data for SC-10-01
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfSC_10_02()
        {
            var testCaseName = "SC_10_02";
            var xmlUtility = new XmlUtility(Settings.JIRA_COVERAGE_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, string>();
            var controllerInfo = xmlUtility.GetSingleNode(string.Format(Settings.JIRA_COVERAGE_XPATH_PREFIX, testCaseName, "Controller"));
            testData.Add("ControllerName", controllerInfo.GetAttrVal("name"));
            testData.Add("ControllerId", controllerInfo.GetAttrVal("id"));

            return testData;
        }

        /// <summary>
        /// Read test data for SC-89
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfSC_89()
        {
            var testCaseName = "SC_89";
            var xmlUtility = new XmlUtility(Settings.JIRA_COVERAGE_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, string>();        
            testData.Add("Calendar", xmlUtility.GetSingleNodeText(string.Format(Settings.JIRA_COVERAGE_XPATH_PREFIX, testCaseName, "Calendar")));
            var notCommControllerInfo = xmlUtility.GetSingleNode(string.Format(Settings.JIRA_COVERAGE_XPATH_PREFIX, testCaseName, "NotCommisionController"));
            testData.Add("NotCommisionControllerName", notCommControllerInfo.GetAttrVal("name"));
            testData.Add("NotCommisionControllerId", notCommControllerInfo.GetAttrVal("id"));
            var commControllerInfo = xmlUtility.GetSingleNode(string.Format(Settings.JIRA_COVERAGE_XPATH_PREFIX, testCaseName, "CommisionController"));
            testData.Add("CommisionControllerName", commControllerInfo.GetAttrVal("name"));
            testData.Add("CommisionControllerId", commControllerInfo.GetAttrVal("id"));

            return testData;
        }

        /// <summary>
        /// Read test data for SC-90
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfSC_90()
        {
            var testCaseName = "SC_90";
            var xmlUtility = new XmlUtility(Settings.JIRA_COVERAGE_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, string>();
            var controllerInfo = xmlUtility.GetSingleNode(string.Format(Settings.JIRA_COVERAGE_XPATH_PREFIX, testCaseName, "Controller"));
            testData.Add("ControllerName", controllerInfo.GetAttrVal("name"));
            testData.Add("ControllerId", controllerInfo.GetAttrVal("id"));

            return testData;
        }

        /// <summary>
        /// Read test data for SC-509
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfSC_509()
        {
            var testCaseName = "SC_509";
            var xmlUtility = new XmlUtility(Settings.JIRA_COVERAGE_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, string>();
            var controllerInfo = xmlUtility.GetSingleNode(string.Format(Settings.JIRA_COVERAGE_XPATH_PREFIX, testCaseName, "Controller"));
            testData.Add("ControllerName", controllerInfo.GetAttrVal("name"));
            testData.Add("ControllerId", controllerInfo.GetAttrVal("id"));

            return testData;
        }

        /// <summary>
        /// Read test data for SC-98
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, object> GetTestDataOfSC_98()
        {
            return GetCommonTestData();
        }

        /// <summary>
        /// Read test data for SC-116
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfSC_116()
        {
            var testCaseName = "SC_116";
            var xmlUtility = new XmlUtility(Settings.JIRA_COVERAGE_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, string>();
            var controllerInfo = xmlUtility.GetSingleNode(string.Format(Settings.JIRA_COVERAGE_XPATH_PREFIX, testCaseName, "Controller"));
            testData.Add("ControllerName", controllerInfo.GetAttrVal("name"));
            testData.Add("ControllerId", controllerInfo.GetAttrVal("id"));

            return testData;
        }

        /// <summary>
        /// Read test data for SC-20-01
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfSC_20_01()
        {
            var testCaseName = "SC_20_01";
            var xmlUtility = new XmlUtility(Settings.JIRA_COVERAGE_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, string>();
            var controllerInfo = xmlUtility.GetSingleNode(string.Format(Settings.JIRA_COVERAGE_XPATH_PREFIX, testCaseName, "Controller"));
            testData.Add("ControllerName", controllerInfo.GetAttrVal("name"));
            testData.Add("ControllerId", controllerInfo.GetAttrVal("id"));

            return testData;
        }

        /// <summary>
        /// Read test data for SC-20-02
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfSC_20_02()
        {
            var testCaseName = "SC_20_02";
            var xmlUtility = new XmlUtility(Settings.JIRA_COVERAGE_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, string>();
            var controllerInfo = xmlUtility.GetSingleNode(string.Format(Settings.JIRA_COVERAGE_XPATH_PREFIX, testCaseName, "Controller"));
            testData.Add("ControllerName", controllerInfo.GetAttrVal("name"));
            testData.Add("ControllerId", controllerInfo.GetAttrVal("id"));

            return testData;
        }

        /// <summary>
        /// Read test data for SC-548
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfSC_548()
        {
            var testCaseName = "SC_548";
            var xmlUtility = new XmlUtility(Settings.JIRA_COVERAGE_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, string>();
            var controllerInfo = xmlUtility.GetSingleNode(string.Format(Settings.JIRA_COVERAGE_XPATH_PREFIX, testCaseName, "Controller"));
            testData.Add("ControllerName", controllerInfo.GetAttrVal("name"));
            testData.Add("ControllerId", controllerInfo.GetAttrVal("id"));

            return testData;
        }

        /// <summary>
        /// Read test data for SC-665
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, object> GetTestDataOfSC_665()
        {
            return GetCommonTestData();
        }

        /// <summary>
        /// Read test data for SC-825
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, object> GetTestDataOfSC_825()
        {
            var testCaseName = "SC_825";
            var xmlUtility = new XmlUtility(Settings.JIRA_COVERAGE_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, object>();
            testData.Add("ExpectedTemplates", xmlUtility.GetChildNodesText(string.Format(Settings.JIRA_COVERAGE_XPATH_PREFIX, testCaseName, "ExpectedTemplates")));

            return testData;
        }   

        /// <summary>
        /// Read test data for SLV-218
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, object> GetTestDataOfSC_218()
        {
            var testCaseName = "SC_218";
            var xmlUtility = new XmlUtility(Settings.JIRA_COVERAGE_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, object>();
            testData.Add("ReportTypes", xmlUtility.GetChildNodesText(string.Format(Settings.JIRA_COVERAGE_XPATH_PREFIX, testCaseName, "ReportTypes")));

            return testData;
        }

        #endregion //Input XML data

        #endregion //Private methods
    }
}