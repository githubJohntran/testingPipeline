using ImageMagick;
using NUnit.Framework;
using StreetlightVision.Extensions;
using StreetlightVision.Models;
using StreetlightVision.Pages;
using StreetlightVision.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;

namespace StreetlightVision.Tests.Smoke
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    public class SmokeTests : TestBase
    {
        #region Variables

        private const int REPORT_WAIT_MINUTES = 5;

        #endregion //Variables

        #region Contructors

        #endregion //Contructors

        #region Test Cases

        [Test, DynamicRetry]
        [Description("TS 0.1 Login")]
        public void TS_01_Login()
        {
            Step("1. Open SLV CMS");
            Step("2. Enter correct username and password");
            Step("3. Expected Login success");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.Users);

            Step("4. Verify Users app");
            Step("5. Expected Username and Group are displayed correctly");
            VerifyUserAppInfo(desktopPage, Settings.Users["DefaultTest"].FullName, Settings.Users["DefaultTest"].Profile);

            Step("6. (SLV-1623) Refesh browser");
            desktopPage = Browser.RefreshLoggedInCMS();

            Step("7. Expected The session is still alive - Desktop page is displayed in stead of login screen");
            VerifyUserAppInfo(desktopPage, Settings.Users["DefaultTest"].FullName, Settings.Users["DefaultTest"].Profile);
        }        

        [Test, DynamicRetry]
        [Description("TS 0.2 Data History")]
        public void TS_02_DataHistory()
        {
            var testData = GetTestDataOfTestTS02();

            var geozone = testData["Geozone"].ToString();
            var streetlights = testData["Streetlights"] as List<DeviceModel>;

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.DataHistory);

            Step("1. Go to Data History page from Desktop page or App Switch");
            Step("2. **Expected** Data History page is routed");
            var dataHistoryPage = desktopPage.GoToApp(App.DataHistory) as DataHistoryPage;

            Step("3. Select a geozone");
            dataHistoryPage.GeozoneTreeMainPanel.SelectNode(geozone);

            Step("4. **Expected** all devices of selected geozone are displayed in grid");
            var selectedNodeText = dataHistoryPage.GeozoneTreeMainPanel.GetSelectedNodeText();
            var footerRightText = dataHistoryPage.GridPanel.GetFooterRightText();
            var devicesTotalRegex = Regex.Match(footerRightText, @"of ([0-9]*) Records").Groups[1].Value;

            var selectedNodeTextArr = selectedNodeText.SplitEx();
            var actualNofDevices = int.Parse(selectedNodeTextArr[1].SplitAndGetAt(new string[] { "devices" }, 0));
            var expectedNofDevices = int.Parse(devicesTotalRegex);
            VerifyEqual("4. Verify all devices of selected geozone are displayed in grid", expectedNofDevices, actualNofDevices);

            Step("5. Select a streetlight device in grid");
            var streetlightName = streetlights.PickRandom().Name;
            dataHistoryPage.GridPanel.ClickGridRecord(streetlightName);

            Step("6. **Expected** Device widget on the left appears which 2 tabs: Meterings and Failures as in pictures");
            var actualTabsListText = dataHistoryPage.LastValuesPanel.GetListOfTabs();
            var expectedTabsListText = new List<string>() { "Meterings", "Failures" };
            VerifyEqual("6. Verify device widget on the left appears which 2 tabs: Meterings and Failures as in pictures", expectedTabsListText, actualTabsListText);

            Step("7. Select an attribute on Left panel");
            Step("8. **Expected** A graph is displayed in main content with data displayed. In graph header, selected attribtue is displayed");
            List<string> currentSelectedValues = null;

            var meteringsAttributeList = dataHistoryPage.LastValuesPanel.GetMeteringsNameList();
            meteringsAttributeList.Remove("Node failure message");
            meteringsAttributeList = meteringsAttributeList.PickRandom(2);

            var firstAttribute = meteringsAttributeList.FirstOrDefault();
            dataHistoryPage.LastValuesPanel.SelectMeteringsAttribute(firstAttribute);
            Wait.ForSeconds(4);
            if (dataHistoryPage.IsLoaderSpinDisplayed())
            { 
                Warning("[SC-1015] 8. Unable to display graphs in Data History (Spinning wheel is disappeared)");
                return;
            }

            currentSelectedValues = dataHistoryPage.GraphPanel.GetSelectedValues(streetlightName);
            VerifyTrue(string.Format("8. Verify in graph header, selected attribtue {0} is displayed", firstAttribute), currentSelectedValues.Contains(firstAttribute), "True", "False");
            var graph1ImageAsBytes = dataHistoryPage.GraphPanel.SaveChartsAsBytes();

            meteringsAttributeList.Remove(firstAttribute);
            foreach (var attribute in meteringsAttributeList)
            {
                dataHistoryPage.LastValuesPanel.SelectMeteringsAttribute(attribute);
                dataHistoryPage.WaitForPreviousActionComplete();
                currentSelectedValues = dataHistoryPage.GraphPanel.GetSelectedValues(streetlightName);
                VerifyTrue(string.Format("8. Verify in graph header, selected attribtue {0} is displayed", attribute), currentSelectedValues.Contains(attribute), "True", "False");
                var graph2ImageAsBytes = dataHistoryPage.GraphPanel.SaveChartsAsBytes();
                Verify2GraphsAsBytes(graph1ImageAsBytes, graph2ImageAsBytes, string.Format("select attribtue {0}", attribute));
                graph1ImageAsBytes = graph2ImageAsBytes;
            }           

            Step("9. Select one or some attributes else on Left panel (in both tabs)");
            Step("10. **Expected** A graph is displayed in main content with data displayed. In graph header, selected attribtue is displayed");
            dataHistoryPage.LastValuesPanel.SelectTab("Failures");
            var failuresAttributeList = dataHistoryPage.LastValuesPanel.GetFailuresNameList().PickRandom(2);
            foreach (var attribute in failuresAttributeList)
            {
                graph1ImageAsBytes = dataHistoryPage.GraphPanel.SaveChartsAsBytes();
                dataHistoryPage.LastValuesPanel.SelectFailuresAttribute(attribute);
                dataHistoryPage.WaitForPreviousActionComplete();
                currentSelectedValues = dataHistoryPage.GraphPanel.GetSelectedValues(streetlightName);
                var graph2ImageAsBytes = dataHistoryPage.GraphPanel.SaveChartsAsBytes();
                VerifyTrue(string.Format("10. Verify in graph header, selected attribtue {0} is displayed", attribute), currentSelectedValues.Contains(attribute), "True", "False");
                Verify2GraphsAsBytes(graph1ImageAsBytes, graph2ImageAsBytes, string.Format("select attribtue {0}", attribute));
            }

            Step("11. Click Close icon in a graph");
            dataHistoryPage.GraphPanel.ClickCloseGraph(streetlightName);
            dataHistoryPage.WaitForPreviousActionComplete();

            Step("12. **Expected** The graph is close");
            var currentGraph = dataHistoryPage.GraphPanel.GetGraph(streetlightName);
            VerifyEqual("12. Verify the graph is close", null, currentGraph);
        }

        [Test, DynamicRetry]
        [Description("TS 0.3 Schedulers")]
        public void TS_03_Schedulers()
        {
            var testData = GetTestDataOfTestTS03();
            var geozone = testData["Geozone"].ToString();
            var streetlights = testData["Streetlights"] as List<DeviceModel>;
            var streetlight = streetlights.PickRandom();
            var streetlightName = streetlight.Name;
            var dimmingGroup = streetlight.DimmingGroup;

            Step("-> Login successfully");
            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.EquipmentInventory, App.SchedulingManager);

            Step("1. Go to Equipment Inventory app");
            Step("2. Expected Equipment Inventory page is routed and loaded successfully");
            var equipmentInventoryPage = desktopPage.GoToApp(App.EquipmentInventory) as EquipmentInventoryPage;

            Step("3. Select a street light device and set its Dimming Group to 'Calendar A' then save");
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(string.Format(@"{0}\{1}", geozone, streetlightName));

            var currentDimmingGroup = equipmentInventoryPage.StreetlightEditorPanel.GetDimmingGroupValue();
            var controller = equipmentInventoryPage.StreetlightEditorPanel.GetControllerIdValue();

            if (string.IsNullOrEmpty(currentDimmingGroup) || !dimmingGroup.Equals(currentDimmingGroup))
            {
                equipmentInventoryPage.StreetlightEditorPanel.SelectDimmingGroupDropDown(dimmingGroup);
                equipmentInventoryPage.StreetlightEditorPanel.ClickSaveButton();
                equipmentInventoryPage.WaitForPreviousActionComplete();
            }

            Step("4. Go to Scheduling Manager app");
            Step("5. Expected Scheduling Manager page is routed and loaded successfully");
            var schedulingManagerPage = equipmentInventoryPage.AppBar.SwitchTo(App.SchedulingManager) as SchedulingManagerPage;

            Step("6. Click Calendar tab");
            schedulingManagerPage.SchedulingManagerPanel.SelectTab("Calendar");
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("7. Expected Calendar tab is active");
            VerifyEqual("7. Verify Calendar is active", "Calendar", schedulingManagerPage.SchedulingManagerPanel.GetActiveTabText());

            Step("8. Select 'Calendar A' in the grid of calendars then click Commission icon");
            schedulingManagerPage.SchedulingManagerPanel.SelectCalendar(dimmingGroup);
            schedulingManagerPage.WaitForPreviousActionComplete();
            schedulingManagerPage.SchedulingManagerPanel.ClickCommissioningCalendarButton();
            schedulingManagerPage.WaitForPopupDialogDisplayed();
            schedulingManagerPage.CommissionPopupPanel.WaitForGridContentDisplayed();

            Step("9. Expected Commissioning dialog appears. In grid is the controller of the selected device at step #3");
            VerifyCommissionController(schedulingManagerPage, controller);

            Step("10. Click Close icon");
            Step("11. Expected The dialog is closed. No commissioning request is sent to server");
            schedulingManagerPage.CommissionPopupPanel.ClickCancelButton();
            schedulingManagerPage.WaitForPopupDialogDisappeared();

            Step("12. Click Commission icon again");
            Step("13. Commissioning dialog re-appears");
            schedulingManagerPage.SchedulingManagerPanel.ClickCommissioningCalendarButton();
            schedulingManagerPage.WaitForPopupDialogDisplayed();
            schedulingManagerPage.CommissionPopupPanel.WaitForGridContentDisplayed();

            Step("14. Click Commission icon");
            Step("15. Expected Commissioning request is sent to server. After successful commission, message 'Calendar commissioning has been initiated. This process can take several minutes. You may check for calendar commissioning failures in the Failures tab.' is shown");
            schedulingManagerPage.CommissionPopupPanel.ClickCommissionButton();
            schedulingManagerPage.Dialog.WaitForPopupMessageDisplayed();
            VerifyEqual("15. Verify message is shown as expected", "Calendar commissioning has been initiated. This process can take several minutes. You may check for calendar commissioning failures in the Failures tab.", schedulingManagerPage.Dialog.GetMessageText());

            Step("16. Click OK button");
            Step("17. Expected The message is closed");
            schedulingManagerPage.Dialog.ClickOkButton();
            schedulingManagerPage.Dialog.WaitForPopupMessageDisappeared();

            Step("18. Click Cross icon");
            Step("19. Expected Commissioning dialog is closed");
            schedulingManagerPage.CommissionPopupPanel.ClickCancelButton();
            schedulingManagerPage.WaitForPopupDialogDisappeared();
        }

        [Test, DynamicRetry]
        [Description("TS 0.4 Real Time Control")]
        public void TS_04_RealtimeControl()
        {
            var testData = GetTestDataOfTestTS04();
            var geozone = testData["Geozone"].ToString();
            var streetlights = testData["Streetlights"] as List<DeviceModel>;
            var streetlightName = streetlights.PickRandom().Name;

            Step("-> Login successfully");
            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.RealTimeControl);

            Step("1. Go to Real-time Control app");
            Step("2. **Expected** Real-time Control page is routed and loaded successfully");
            var realtimeControlPage = desktopPage.GoToApp(App.RealTimeControl) as RealTimeControlPage;

            Step("3. Select a deepest sub-geozone");
            Step("4. **Expected** Map displays location of sub-geozone with its devices");
            realtimeControlPage.GeozoneTreeMainPanel.SelectNode(geozone);
            var childNodesCount = realtimeControlPage.GeozoneTreeMainPanel.CountNodesOfSelectedGeozone();
            VerifyTrue(string.Format("4. Verify '{0}' displays sub-geozone with its devices", geozone), childNodesCount > 0, string.Format("'{0}' must have devices", geozone), string.Format("'{0}' have no devices", geozone));

            Step("5. Select a street light device");
            realtimeControlPage.GeozoneTreeMainPanel.SelectNode(streetlightName);

            Step("6. **Expected**");
            Step(" - The device is active on the map");
            Step(" - Luminaire Controller widget appears");

            var deviceModel = realtimeControlPage.Map.GetFirstSelectedDevice();
            if (deviceModel == null)
                Warning("SLV-3675: Active device selection not synced between geozone tree and map");
            else
            {
                var actualStreetlightOnMap = realtimeControlPage.Map.MoveAndGetDeviceNameGL(deviceModel.Longitude, deviceModel.Latitude);
                VerifyTrue(string.Format("6. Verify '{0}' is active on the map", streetlightName), streetlightName.Equals(actualStreetlightOnMap), streetlightName, actualStreetlightOnMap);

                var actualStreetlightOnWidget = realtimeControlPage.StreetlightWidgetPanel.GetDeviceNameText();
                VerifyTrue(string.Format("6. Verify '{0}' is correct on Luminaire Controller widget", streetlightName), streetlightName.Equals(actualStreetlightOnWidget), streetlightName, actualStreetlightOnWidget);

                Step("7. Click OFF on Luminaire Controller widget");
                Step("8. **Expected**");
                Step(" - Reload icon rotates");
                Step(" - Metering details are updated");
                Step(" - Last update time are updated");
                Step(" - Feedback and Command value are updated");
                Step(" - Highlight of dimming bar has updated according to set level");
                Step(" - The device on the map is updated its yellow part");
                Step(" - The position of triangle is updated");
                Step("** Dimming levels: OFF");
                var lastUpdateTime = realtimeControlPage.StreetlightWidgetPanel.GetLastUpdateTimeText();
                realtimeControlPage.StreetlightWidgetPanel.ExecuteCommand(RealtimeCommand.DimOff);
                realtimeControlPage.StreetlightWidgetPanel.WaitForCommandCompleted();
                VerifyLuminaireControllerInfo(realtimeControlPage, RealtimeCommand.DimOff, lastUpdateTime);
                Step("9. Repeat step #7 with predefined dimming levels: 40%, 50%, 60%, 70%, 80%, 90%, ON");
                Step("10. **Expected** The same with step #8");
                Step("** Dimming levels: 40%");
                lastUpdateTime = realtimeControlPage.StreetlightWidgetPanel.GetLastUpdateTimeText();
                realtimeControlPage.StreetlightWidgetPanel.ExecuteCommand(RealtimeCommand.Dim40);
                realtimeControlPage.StreetlightWidgetPanel.WaitForCommandCompleted();
                VerifyLuminaireControllerInfo(realtimeControlPage, RealtimeCommand.Dim40, lastUpdateTime);
                Step("** Dimming levels: 50%");
                lastUpdateTime = realtimeControlPage.StreetlightWidgetPanel.GetLastUpdateTimeText();
                realtimeControlPage.StreetlightWidgetPanel.ExecuteCommand(RealtimeCommand.Dim50);
                realtimeControlPage.StreetlightWidgetPanel.WaitForCommandCompleted();
                VerifyLuminaireControllerInfo(realtimeControlPage, RealtimeCommand.Dim50, lastUpdateTime);
                Step("** Dimming levels: 60%");
                lastUpdateTime = realtimeControlPage.StreetlightWidgetPanel.GetLastUpdateTimeText();
                realtimeControlPage.StreetlightWidgetPanel.ExecuteCommand(RealtimeCommand.Dim60);
                realtimeControlPage.StreetlightWidgetPanel.WaitForCommandCompleted();
                VerifyLuminaireControllerInfo(realtimeControlPage, RealtimeCommand.Dim60, lastUpdateTime);
                Step("** Dimming levels: 70%");
                lastUpdateTime = realtimeControlPage.StreetlightWidgetPanel.GetLastUpdateTimeText();
                realtimeControlPage.StreetlightWidgetPanel.ExecuteCommand(RealtimeCommand.Dim70);
                realtimeControlPage.StreetlightWidgetPanel.WaitForCommandCompleted();
                VerifyLuminaireControllerInfo(realtimeControlPage, RealtimeCommand.Dim70, lastUpdateTime);
                Step("** Dimming levels: 80%");
                lastUpdateTime = realtimeControlPage.StreetlightWidgetPanel.GetLastUpdateTimeText();
                realtimeControlPage.StreetlightWidgetPanel.ExecuteCommand(RealtimeCommand.Dim80);
                realtimeControlPage.StreetlightWidgetPanel.WaitForCommandCompleted();
                VerifyLuminaireControllerInfo(realtimeControlPage, RealtimeCommand.Dim80, lastUpdateTime);
                Step("** Dimming levels: 90%");
                lastUpdateTime = realtimeControlPage.StreetlightWidgetPanel.GetLastUpdateTimeText();
                realtimeControlPage.StreetlightWidgetPanel.ExecuteCommand(RealtimeCommand.Dim90);
                realtimeControlPage.StreetlightWidgetPanel.WaitForCommandCompleted();
                VerifyLuminaireControllerInfo(realtimeControlPage, RealtimeCommand.Dim90, lastUpdateTime);
                Step("** Dimming levels: ON");
                lastUpdateTime = realtimeControlPage.StreetlightWidgetPanel.GetLastUpdateTimeText();
                realtimeControlPage.StreetlightWidgetPanel.ExecuteCommand(RealtimeCommand.DimOn);
                realtimeControlPage.StreetlightWidgetPanel.WaitForCommandCompleted();
                VerifyLuminaireControllerInfo(realtimeControlPage, RealtimeCommand.DimOn, lastUpdateTime);

                Step("11. Set dimming level to a random level by using Triangle icon");
                Step("12. **Expected** The same with step #8");
                lastUpdateTime = realtimeControlPage.StreetlightWidgetPanel.GetLastUpdateTimeText();
                var command = realtimeControlPage.StreetlightWidgetPanel.ExecuteRandomDimming10To90ByCursor();
                realtimeControlPage.StreetlightWidgetPanel.WaitForCommandCompleted();
                VerifyLuminaireControllerInfo(realtimeControlPage, command, lastUpdateTime);

                #region Back step 13 & 14 when issue#SLV-1688 fixed

                Info("Ignore step 13 & 14: Back when issue#SLV-1688 fixed");
                ////13. Set dimming level to a random level by using Command field (focus, enter value then hit Enter)
                ////14. **Expected** The same with step #8

                #endregion
            }
        }        

        [Test, DynamicRetry]
        [Description("TS 0.5 Scheduled Report")]
        public void TS_05_ScheduledReport()
        {
            var testData = GetTestDataOfTestTS05();

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
            Step("2. **Expected** Report Manager page is routed");
            var reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;

            Step("3. Select a geozone");
            reportManagerPage.GeozoneTreeMainPanel.SelectNode(expectedGeoZone);

            Step("4. Click Add Report icon");
            Step("5. **Expected** Report wizard widget is opened");
            reportManagerPage.GridPanel.ClickAddReportToolbarButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.ReportEditorPanel.WaitForPanelLoaded();

            Step("6. Fill values into report fields:");

            /*     Name: "Failures HTML report {{current time stamp}}" 
                   Type: "Failure HTML report" 
                   Properties tab: 
                       + Description: Any 
                       + Report details: All devices 
                       + Filtering mode: No filter 
                   Scheduler tab: 
                       + Periodicity: Everyday 
                       + Hour and minute: Current time + 1 minute  
                   Mail tab: 
                       + Subject: Any 
                       + From: qa@streetlightmonitoring.com  
                       + Contacts: testing_contact selected from the list  
                       + Configuration: HTML format: checked */
            var reportName = SLVHelper.GenerateUniqueName(string.Format("[{0}][{1}] {2}", Settings.Users["DefaultTest"].Username, Settings.CurrentTestWebDriverKeyName, reportPrefixInput));

            var reportDetailsPanel = reportManagerPage.ReportEditorPanel;
            reportDetailsPanel.EnterNameInput(reportName);
            reportDetailsPanel.SelectTypeDropDown(reportTypeInput);
            reportManagerPage.WaitForPreviousActionComplete();

            // Properties tab
            reportDetailsPanel.EnterDescriptionInput(propertiesTabListInput.ElementAt(0));
            reportDetailsPanel.SelectReportDetailsDropDown(propertiesTabListInput.ElementAt(1));
            reportDetailsPanel.SelectFilteringModeDropDown(propertiesTabListInput.ElementAt(2));

            // Scheduler tab
            var reportRunDate = Settings.GetServerTime().AddMinutes(REPORT_WAIT_MINUTES);
            var mailHour = reportRunDate.Hour;
            var mailMinute = reportRunDate.Minute;

            reportDetailsPanel.SelectTab("Scheduler");            
            reportDetailsPanel.SelectHourDropDown(string.Format("{0:D2}", mailHour));
            reportDetailsPanel.SelectMinuteDropDown(string.Format("{0:D2}", mailMinute));
            reportDetailsPanel.SelectPeriodicityDropDown(schedulerTabListInput.ElementAt(0));
            reportDetailsPanel.SelectTimezoneDropDown(Settings.DEFAULT_TIMEZONE);

            // Export tab
            reportDetailsPanel.SelectTab("Export");
            var expectedSubjectInput = string.Format("[{0}][{1}] {2}", Settings.Users["DefaultTest"].Username, Settings.CurrentTestWebDriverKeyName, mailTabListInput.ElementAt(0));
            var expectedSenderInput = mailTabListInput.ElementAt(1);
            reportDetailsPanel.EnterSubjectInput(expectedSubjectInput);
            reportDetailsPanel.EnterFromInput(expectedSenderInput);
            reportDetailsPanel.SelectContactsListDropDown(mailTabListInput.ElementAt(2));
            reportDetailsPanel.TickHtmlFormatCheckbox(true);                     

            Step("7. Click Save icon");
            reportDetailsPanel.ClickSaveButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisappeared();
            
            Step("8. **Expected** The new report is saved as a new row in grid of reports");
            var reportsList = reportManagerPage.GridPanel.GetListOfColumnData("Name");
            var newReport = reportsList.FirstOrDefault(item => string.Equals(item.SplitAndGetAt(0), reportName));
            VerifyTrue(string.Format("8. Verify new report {0} is saved as a new row in grid of reports", reportName), newReport != null, "SUCCESS", "FAILED");

            Step("9. Wait until time set at step #6 comes, check mailbox of the contact set at Contacts in Mail tab");
            EmailUtility.CleanInbox(expectedSubjectInput);
            var newMail = EmailUtility.GetNewEmail(expectedSubjectInput);
            var hasNewMail = newMail != null;
            VerifyTrue(string.Format("9. Verify there is an email sent from {0}", expectedSenderInput), hasNewMail, "Email sent", "No email sent");
            if (hasNewMail)
            {
                var mailSubject = newMail.Subject;
                var mailSender = newMail.From;

                Step("10. **Expected** There is a mail sent with Subject, From and Content (in html format) are corresponding to what set in step #6");
                VerifyTrue(string.Format("Verify mail subject {0} is as expected", expectedSubjectInput), mailSubject.Contains(expectedSubjectInput), expectedSubjectInput, mailSubject);
                VerifyTrue(string.Format("Verify mail sender {0} is as expected", expectedSenderInput), mailSender.Equals(expectedSenderInput), expectedSenderInput, mailSender);
                VerifyTrue("10. Verify mail HTML format is enabled", newMail.IsBodyHtml, "Html format", "No Html format");

                //Delete report
                reportManagerPage.DeleteReport(newReport);
            }
        }

        [Test, DynamicRetry]
        [Description("TS 0.6 Energy Report")]
        public void TS_06_EnergyReport()
        {
            var testData = GetTestDataOfTestTS06();

            var geozonePath = testData["Geozone"].ToString();
            var deepestGeoZone = testData["DeepestGeozone"].ToString();
            var expectedGeozoneColumnsList = testData["ExpectedGeozoneColumnsList"] as List<string>;
            var expectedDeviceColumnsList = testData["ExpectedDeviceColumnsList"] as List<string>;
            var geozoneName = geozonePath.GetChildName();

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.Energy);

            Step("1. Go to Energy page from Desktop page or App Switch");
            Step("2. **Expected** Energy page is routed");
            var energyPage = desktopPage.GoToApp(App.Energy) as EnergyPage;

            Step("3. Verify main panel when the root geozone is active");
            Step("•	Title is the name of selected geozone");
            var actualTitle = energyPage.GridPanel.GetPanelTitleText();
            VerifyEqual("3. Verify title is the name of selected geozone {0}", Settings.RootGeozoneName, actualTitle);

            Step("•	Grid has columns: Geozone, Measured (kwh), Before (kwh), Energy savings (%), Pollution savings (tons of CO2)");
            var actualColumnsList = energyPage.GridPanel.GetListOfColumnsHeader();
            VerifyEqual("Verify grid has expected columns", expectedGeozoneColumnsList, actualColumnsList);

            Step("•	Each row is a level 1 geozone. Geozone column is for geozone name, others are aggregated values of that geozone");
            var actualSubGeoZoneList = energyPage.GeozoneTreeMainPanel.GetChildNodeNamesOfSelectedNode(NodeType.GeoZone).Select(e => e.SplitAndGetAt(0)).ToList();
            var expectedSubGeoZoneList = energyPage.GridPanel.GetListOfGeozones();
            VerifyEqual("[#1281964] 3. Verify each row is a level 1 geozone", expectedSubGeoZoneList, actualSubGeoZoneList, false);

            Step("4. Select a sub-geozone which is a root of other geozone(s)");
            energyPage.GeozoneTreeMainPanel.SelectNode(geozonePath);
            energyPage.WaitForPreviousActionComplete();
            energyPage.GridPanel.WaitForPanelLoaded();

            Step("5. **Expected** The same expectation with when the root geozone is selected but only level 1 children of the selected sub-geozone are listed and the title of main panel is the name of selected sub-geozone");
            actualTitle = energyPage.GridPanel.GetPanelTitleText();
            VerifyEqual("5. Verify title is the name of selected geozone {0}", geozoneName, actualTitle);

            actualColumnsList = energyPage.GridPanel.GetListOfColumnsHeader();
            VerifyEqual("5. Verify grid has expected columns", expectedGeozoneColumnsList, actualColumnsList);

            actualSubGeoZoneList = energyPage.GeozoneTreeMainPanel.GetChildNodeNamesOfSelectedNode(NodeType.GeoZone).Select(e => e.SplitAndGetAt(0)).ToList();
            expectedSubGeoZoneList = energyPage.GridPanel.GetListOfGeozones();
            VerifyEqual("5. Verify each row is a level 1 geozone", expectedSubGeoZoneList, actualSubGeoZoneList);

            Step("6. Select the deepest sub-geozone");
            energyPage.GeozoneTreeMainPanel.SelectNode(deepestGeoZone);
            energyPage.GridPanel.WaitForPanelLoaded();

            Step("7. **Expected**");
            Step("•	Grid has columns: Device, Measured (kwh), Before (kwh), Burning hours, Average (W), Energy savings (%), Pollution savings (tons of CO2)");
            var actualDeviceColumnsList = energyPage.GridPanel.GetListOfColumnsHeader();
            VerifyEqual("7. Verify grid has expected columns", expectedDeviceColumnsList, actualDeviceColumnsList);

            Step("•	Each row is a device of selected geozone. Device column is for device name, others are aggregated values of that device");
            var actualSubDeviceList = energyPage.GeozoneTreeMainPanel.GetChildDeviceNamesOfSelectedNode();
            var expectedSubDeviceList = energyPage.GridPanel.GetListOfDevices();
            VerifyEqual("7. Verify each row is a level 1 geozone", expectedSubDeviceList, actualSubDeviceList);
        }

        #endregion //Test Cases

        #region Private methods

        #region XML Input data

        private Dictionary<string, object> GetCommonTestData()
        {
            var realtimeGeozone = Settings.CommonTestData[0];
            var testData = new Dictionary<string, object>();
            testData.Add("Geozone", realtimeGeozone.Path);
            var streetlights = realtimeGeozone.Devices.Where(p => p.Type == DeviceType.Streetlight && p.Status == DeviceStatus.Working && string.IsNullOrEmpty(p.Cluster)).ToList();
            testData.Add("Streetlights", streetlights);

            return testData;
        }

        /// <summary>
        /// Read test data for test case TS02
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, object> GetTestDataOfTestTS02()
        {
            return GetCommonTestData();
        }

        /// <summary>
        /// Read test data for test case TS03
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, object> GetTestDataOfTestTS03()
        {
            return GetCommonTestData();
        }

        /// <summary>
        /// Read test data for test case TS04
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, object> GetTestDataOfTestTS04()
        {
            return GetCommonTestData();
        }

        /// <summary>
        /// Read test data for test case TS05
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, object> GetTestDataOfTestTS05()
        {
            var testCaseName = "TS05";
            var xmlUtility = new XmlUtility(Settings.SMOKE_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, object>();

            testData.Add("GeoZone", xmlUtility.GetSingleNodeText(string.Format(Settings.SMOKE_XPATH_PREFIX, testCaseName, "GeoZone")));
            testData.Add("ReportPrefix", xmlUtility.GetSingleNodeText(string.Format(Settings.SMOKE_XPATH_PREFIX, testCaseName, "ReportPrefix")));
            testData.Add("ReportType", xmlUtility.GetSingleNodeText(string.Format(Settings.SMOKE_XPATH_PREFIX, testCaseName, "ReportType")));
            testData.Add("PropertiesTab", xmlUtility.GetChildNodesText(string.Format(Settings.SMOKE_XPATH_PREFIX, testCaseName, "PropertiesTab")));
            testData.Add("SchedulerTab", xmlUtility.GetChildNodesText(string.Format(Settings.SMOKE_XPATH_PREFIX, testCaseName, "SchedulerTab")));
            testData.Add("MailTab", xmlUtility.GetChildNodesText(string.Format(Settings.SMOKE_XPATH_PREFIX, testCaseName, "MailTab")));

            return testData;

        }

        /// <summary>
        /// Read test data for test case TS06
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, object> GetTestDataOfTestTS06()
        {
            var testCaseName = "TS06";
            var xmlUtility = new XmlUtility(Settings.SMOKE_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, object>();
            testData.Add("Geozone", xmlUtility.GetSingleNodeText(string.Format(Settings.SMOKE_XPATH_PREFIX, testCaseName, "Geozone")));
            testData.Add("DeepestGeozone", xmlUtility.GetSingleNodeText(string.Format(Settings.SMOKE_XPATH_PREFIX, testCaseName, "DeepestGeozone")));
            testData.Add("ExpectedGeozoneColumnsList", xmlUtility.GetChildNodesText(string.Format(Settings.SMOKE_XPATH_PREFIX, testCaseName, "ExpectedGeozoneColumnsList")));
            testData.Add("ExpectedDeviceColumnsList", xmlUtility.GetChildNodesText(string.Format(Settings.SMOKE_XPATH_PREFIX, testCaseName, "ExpectedDeviceColumnsList")));

            return testData;
        }

        #endregion //XML Input data

        #region Verify methods

        private static void VerifyCommissionController(SchedulingManagerPage page, string expectedController)
        {
            page.CommissionPopupPanel.WaitForGridContentDisplayed();            
            var controllerList = page.CommissionPopupPanel.GetListOfControllers();

            VerifyTrue("Verify the controller of the selected device at step #3", controllerList.Contains(expectedController), expectedController, string.Join(",", controllerList));
        }

        private static void VerifyUserAppInfo(DesktopPage page, string expectedUserFullName, string expectedUserProfileName)
        {
            var actualUserFullName = page.GetUserFullNameText();
            var actualUserProfileName = page.GetUserProfileNameText();

            VerifyTrue("Verify Username is displayed correctly", expectedUserFullName.Equals(actualUserFullName), expectedUserFullName, actualUserFullName);
            VerifyTrue("Verify User Group is displayed correctly", expectedUserProfileName.Equals(actualUserProfileName), expectedUserProfileName, actualUserProfileName);
        }

        private static void Verify2GraphsAsBytes(byte[] graph1ImageAsBytes, byte[] graph2ImageAsBytes, string message)
        {
            var graph1Image = new MagickImage(graph1ImageAsBytes);
            var graph2Image = new MagickImage(graph2ImageAsBytes);
            var result = graph1Image.Compare(graph2Image, ErrorMetric.Absolute);
            VerifyTrue(string.Format("Verify data in charts change accordingly {0}", message), result > 0, true, result > 0);
        }

        #region Realtime Control
        /// <summary>
        /// Verify Luminaire Controller Information
        /// </summary>
        /// <param name="page"></param>
        /// <param name="command"></param>
        /// <param name="lastpdateTime"></param>
        private void VerifyLuminaireControllerInfo(RealTimeControlPage page, RealtimeCommand command, string lastpdateTime)
        {
            VerifyMeterings(page, command);
            VerifyFeedbackAndCommandValue(page, command);
            VerifyTriangleCursorPostion(page, command);
            VerifyLastUpdateTime(page, lastpdateTime);
        }

        /// <summary>
        /// Verify Meterings
        /// </summary>
        /// <param name="page"></param>
        /// <param name="command"></param>
        private void VerifyMeterings(RealTimeControlPage page, RealtimeCommand command)
        {
            var commandData = page.StreetlightWidgetPanel.CommandsDict[command];
            var expectedLampLevelCommand = commandData.Value.ToString();
            var expectedLampLevelFeedback = commandData.ValueText;

            var actualLampBurningHours = page.StreetlightWidgetPanel.GetLampBurningHoursValueText();
            var actualLampEnergy = page.StreetlightWidgetPanel.GetLampEnergyValueText();
            var actualLampLevelCommand = page.StreetlightWidgetPanel.GetLampLevelCommandValueText();
            var actualLampLevelFeedback = page.StreetlightWidgetPanel.GetLampLevelFeedbackValueText();
            var actualLampPower = page.StreetlightWidgetPanel.GetLampPowerValueText();
            var actualLampSwitchFeedback = page.StreetlightWidgetPanel.GetLampSwitchFeedbackValueText();
            var actualMainsCurrent = page.StreetlightWidgetPanel.GetMainsCurrentValueText();
            var actualMainsVoltage = page.StreetlightWidgetPanel.GetMainsVoltageValueText();
            var actualNodeFailureMessage = page.StreetlightWidgetPanel.GetNodeFailureMessageValueText();
            var actualPowerFactor = page.StreetlightWidgetPanel.GetPowerFactorValueText();
            var actualTemperature = page.StreetlightWidgetPanel.GetTemperatureValueText();

            VerifyTrue(string.Format("Verify Meterings - Lamp Burning Hours '{0}' is correct", actualLampBurningHours), !string.IsNullOrEmpty(actualLampBurningHours) && !actualLampBurningHours.Equals("..."), "Must have value", actualLampBurningHours);
            VerifyTrue(string.Format("Verify Meterings - Lamp Energy '{0}' is correct", actualLampEnergy), !string.IsNullOrEmpty(actualLampEnergy) && !actualLampEnergy.Equals("..."), "Must have value", actualLampEnergy);
            VerifyTrue(string.Format("Verify Meterings - Lamp Level command '{0}' is correct", expectedLampLevelCommand), expectedLampLevelCommand.Equals(actualLampLevelCommand), expectedLampLevelCommand, actualLampLevelCommand);
            VerifyTrue(string.Format("Verify Meterings - Lamp Level Feedback '{0}' is correct", expectedLampLevelFeedback), expectedLampLevelFeedback.Equals(actualLampLevelFeedback), expectedLampLevelFeedback, actualLampLevelFeedback);
            VerifyTrue(string.Format("Verify Meterings - Lamp Power '{0}' is correct", actualLampPower), !string.IsNullOrEmpty(actualLampPower) && !actualLampPower.Equals("..."), "Must have value", actualLampPower);
            VerifyTrue(string.Format("Verify Meterings - Switch Feedback '{0}' is correct", actualLampSwitchFeedback), !string.IsNullOrEmpty(actualLampSwitchFeedback) && !actualLampSwitchFeedback.Equals("..."), "Must have value", actualLampSwitchFeedback);
            VerifyTrue(string.Format("Verify Meterings - Mains Current '{0}' is correct", actualMainsCurrent), !string.IsNullOrEmpty(actualMainsCurrent) && !actualMainsCurrent.Equals("..."), "Must have value", actualMainsCurrent);
            VerifyTrue(string.Format("Verify Meterings - Mains Voltage '{0}' is correct", actualMainsVoltage), !string.IsNullOrEmpty(actualMainsVoltage) && !actualMainsVoltage.Equals("..."), "Must have value", actualMainsVoltage);
            VerifyTrue(string.Format("Verify Meterings - Node Failure Message '{0}' is correct", actualNodeFailureMessage), !string.IsNullOrEmpty(actualNodeFailureMessage) && !actualNodeFailureMessage.Equals("..."), "Must have value", actualNodeFailureMessage);
            VerifyTrue(string.Format("Verify Meterings - Power Factor '{0}' is correct", actualPowerFactor), !string.IsNullOrEmpty(actualPowerFactor) && !actualPowerFactor.Equals("..."), "Must have value", actualPowerFactor);
            VerifyTrue(string.Format("Verify Meterings - Temperature '{0}' is correct", actualTemperature), !string.IsNullOrEmpty(actualTemperature) && !actualTemperature.Equals("..."), "Must have value", actualTemperature);
        }

        /// <summary>
        /// Verify Feedback And Command Value
        /// </summary>
        /// <param name="page"></param>
        /// <param name="command"></param>
        private void VerifyFeedbackAndCommandValue(RealTimeControlPage page, RealtimeCommand command)
        {
            var expectedValue = page.StreetlightWidgetPanel.CommandsDict[command].ValueText;
            var actualFeedbackValue = page.StreetlightWidgetPanel.GetRealtimeFeedbackValueText();
            var actualCommandValue = page.StreetlightWidgetPanel.GetRealtimeCommandValueText();

            VerifyTrue(string.Format("[SC-1793] Verify Feedback Value of '{0}' is correct", expectedValue), expectedValue.Equals(actualFeedbackValue), expectedValue, actualFeedbackValue);
            VerifyTrue(string.Format("[SC-1793] Verify Command Value of '{0}' is correct", expectedValue), expectedValue.Equals(actualCommandValue), expectedValue, actualCommandValue);
        }

        /// <summary>
        /// Verify Last Update Time
        /// </summary>
        /// <param name="page"></param>
        /// <param name="oldUpdateTime"></param>
        private void VerifyLastUpdateTime(RealTimeControlPage page, string oldUpdateTime)
        {
            var expectedValue = oldUpdateTime;
            Wait.ForMilliseconds(500);
            var actualValue = page.StreetlightWidgetPanel.GetLastUpdateTimeText();

            VerifyTrue(string.Format("Verify Last Update Time was changed", expectedValue), !expectedValue.Equals(actualValue), expectedValue, actualValue);
        }

        /// <summary>
        /// Verify Triangle Cursor Postion
        /// </summary>
        /// <param name="page"></param>
        /// <param name="command"></param>
        private void VerifyTriangleCursorPostion(RealTimeControlPage page, RealtimeCommand command)
        {
            var commandData = page.StreetlightWidgetPanel.CommandsDict[command];
            var commandValue = commandData.ValueText;
            var expectedValue = commandData.TriangleValue.ToString();
            var actualValue = page.StreetlightWidgetPanel.GetJaugeCursorStyleTopValue();

            VerifyTrue(string.Format("Verify Triangle of Command '{0}' is correct", commandValue), expectedValue.Equals(actualValue), expectedValue, actualValue);
        }
        #endregion //Realtime Control

        #endregion //Verify methods

        #region Common methods

        /// <summary>
        /// Get DateTime String - yyyyMMdd HH:mm:ss.FF
        /// </summary>
        /// <returns></returns>
        private string GetDateTimeString(DateTime dt)
        {
            return dt.ToString("yyyyMMdd HH:mm:ss.FF");
        }

        #endregion //Common methods

        #endregion //Private methods
    }
}
