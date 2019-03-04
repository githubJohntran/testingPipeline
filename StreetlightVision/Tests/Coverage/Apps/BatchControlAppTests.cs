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

namespace StreetlightVision.Tests.Coverage.Apps
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    public class BatchControlAppTests : TestBase
    {
        #region Variables

        #endregion //Variables

        #region Contructors

        #endregion //Contructors       

        #region Test Cases
        
        [Test, DynamicRetry]
        [Description("BC_02 - The UI of Batch Control for each type of devices")]
        public void BC_02()
        {
            var geozone = SLVHelper.GenerateUniqueName("GZNBC02");
            var controllerLat = SLVHelper.GenerateCoordinate("15.12085", "15.12496");
            var controllerLng = SLVHelper.GenerateCoordinate("106.53723", "106.54280");
            var controller = new DeviceModel() { Type = DeviceType.Controller, Name = SLVHelper.GenerateUniqueName("CTRL"), Latitude = controllerLat, Longitude = controllerLng };
            var streetlightLat = SLVHelper.GenerateCoordinate("15.12085", "15.12496");
            var streetlightLng = SLVHelper.GenerateCoordinate("106.53723", "106.54280");
            var streetlight = new DeviceModel() { Type = DeviceType.Streetlight, Name = SLVHelper.GenerateUniqueName("STL"), Latitude = streetlightLat, Longitude = streetlightLng };
            var switchDeviceLat = SLVHelper.GenerateCoordinate("15.12085", "15.12496");
            var switchDeviceLng = SLVHelper.GenerateCoordinate("106.53723", "106.54280");
            var switchDevice = new DeviceModel() { Type = DeviceType.Switch, Name = SLVHelper.GenerateUniqueName("SWH"), Latitude = switchDeviceLat, Longitude = switchDeviceLng };
            var meterLat = SLVHelper.GenerateCoordinate("15.12085", "15.12496");
            var meterLng = SLVHelper.GenerateCoordinate("106.53723", "106.54280");
            var meter = new DeviceModel() { Type = DeviceType.ElectricalCounter, Name = SLVHelper.GenerateUniqueName("MTR"), Latitude = meterLat, Longitude = meterLng };
            var devices = new List<DeviceModel>() { controller, streetlight, switchDevice, meter };
            var devicesLatLng = devices.Select(p => string.Format("{0};{1}", p.Longitude, p.Latitude)).ToList();

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - Create a testing geozone with a controller, a streetlight, an electrical counter, and a switch");
            Step("**** Precondition ****\n");

            Step("-> Create data for testing");
            DeleteGeozones("GZNBC02*");
            CreateNewGeozone(geozone, latMin: "15.11431", latMax: "15.12992", lngMin: "106.52220", lngMax: "106.55619");
            CreateNewController(controller.Name, geozone, lat: controller.Latitude, lng: controller.Longitude);
            CreateNewDevice(DeviceType.Streetlight, streetlight.Name, controller.Name, geozone, lat: streetlight.Latitude, lng: streetlight.Longitude);
            CreateNewDevice(DeviceType.Switch, switchDevice.Name, controller.Name, geozone, lat: switchDevice.Latitude, lng: switchDevice.Longitude);
            CreateNewDevice(DeviceType.ElectricalCounter, meter.Name, controller.Name, geozone, lat: meter.Latitude, lng: meter.Longitude);

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.BatchControl);

            Step("1. Go to Batch Control app and select the testing geozone");
            var batchControlPage = desktopPage.GoToApp(App.BatchControl) as BatchControlPage;

            Step("2. Verify Batch Control widget displays on the map with");
            Step(" o A text 'Run a custom search in the Selected Devices panel below to show devices' displayed on the top of the screen. Updated for SC-1293");
            Step(" o Title: Selected Devices (0 devices)");
            Step(" o Maximize icon on the top right corner of the widget");
            Step(" o Auto-Zoom checkbox: checked (as default)");
            Step(" o Custom search with 2 dropdownlist(s), 1 input and Search button");
            Step(" o a text: 'Search for devices using the controls above'. Updated for SC-1293");
            Step(" o Hide/Show column button");
            Step(" o All Fields Search textbox");
            Step(" o Search Fields button");
            Step(" o Export Data button");
            Step(" o a grid containing 2 columns: Name, Type");
            VerifyEqual("[SC-1293] 2. Verify A text 'Run a custom search in the Selected Devices panel below to show devices' displayed on the top of the screen.", "Run a custom search in the Selected Devices panel below to show devices", batchControlPage.GetRealtimeBatchWarningMessageTopOfScreen());
            VerifyEqual("2. Verify Title: Selected Devices (0 devices)", "Selected Devices (0 devices)", batchControlPage.RealTimeBatchPanel.GetPanelTitleText());
            VerifyEqual("2. Verify Maximize icon on the top right corner of the widget is displayed", true, batchControlPage.RealTimeBatchPanel.IsMaximizeIconDisplayed());
            VerifyEqual("2. Verify Auto-Zoom checkbox: checked (as default)", true, batchControlPage.RealTimeBatchPanel.GetAutoZoomValue());
            VerifyEqual("2. Verify Custom search with Attribute dropdown is displayed", true, batchControlPage.RealTimeBatchPanel.IsAttributeDropDownDisplayed());
            VerifyEqual("2. Verify Custom search with Operater dropdown is displayed", true, batchControlPage.RealTimeBatchPanel.IsOperaterDropDownDisplayed());
            VerifyEqual("2. Verify Custom search with Search Name input is displayed", true, batchControlPage.RealTimeBatchPanel.IsSearchNameInputDisplayed());
            VerifyEqual("2. Verify Custom search with Search button is displayed", true, batchControlPage.RealTimeBatchPanel.IsSearchButtonDisplayed());
            VerifyEqual("[SC-1293] 2. Verify Custom search with a text: 'Search for devices using the controls above'", "Search for devices using the controls above", batchControlPage.RealTimeBatchPanel.GetInstructionsMessage());

            Step("3. Press Maximize icon");
            batchControlPage.RealTimeBatchPanel.ClickShowButton();

            Step("4. Verify The widget is maximized");
            VerifyEqual("4. Verify The widget is maximized", true, batchControlPage.RealTimeBatchPanel.IsWidgetMaximized());

            Step("5. Press Minimize icon");
            batchControlPage.RealTimeBatchPanel.ClickHideButton();

            Step("6. Verify The widget is minimized");
            VerifyEqual("6. Verify The widget is minimized", true, batchControlPage.RealTimeBatchPanel.IsWidgetMinimized());
            if (!batchControlPage.RealTimeBatchPanel.IsMinimizeIconDisplayed()) //For cannot minimize panel
            {
                Warning("[SC-2022] Batch Control: On the Batch Control widget, the minimize icon is not there.");
                batchControlPage.RealTimeBatchPanel.MinimizePanelByDragDrop();
            }

            Step("7. Select the Controller on the map");
            batchControlPage.GeozoneTreeMainPanel.SelectNode(geozone);
            batchControlPage.Map.SelectDeviceGL(controller.Longitude, controller.Latitude);

            Step("8. Verify The widget is changed with");
            Step(" o Title: Selected Devices (1 devices)");
            Step(" o There is a 'Controller Device' tab");
            Step(" o Some new things (button, dropdownlist) like: Refresh, Send data logs, Synchronyze system time, Off, On, Back to auto, and a dropdownlist containing: Output 1, Output 2");
            VerifyEqual("8. Verify Title: Selected Devices (1 devices)", "Selected Devices (1 devices)", batchControlPage.RealTimeBatchPanel.GetPanelTitleText());
            VerifyEqual("8. Verify There is a 'Controller Device' tab", "Controller Device", batchControlPage.RealTimeBatchPanel.GetListOfTabsName().FirstOrDefault());
            VerifyEqual("8. Verify Refresh button is displayed", true, batchControlPage.RealTimeBatchPanel.IsControllerRefreshButtonDisplayed());
            VerifyEqual("8. Verify Send data logs button is displayed", true, batchControlPage.RealTimeBatchPanel.IsControllerSendDataLogsButtonDisplayed());
            VerifyEqual("8. Verify Synchronyze system time button is displayed", true, batchControlPage.RealTimeBatchPanel.IsControllerSyncSystemTimeButtonDisplayed());
            VerifyEqual("8. Verify Off button is displayed", true, batchControlPage.RealTimeBatchPanel.IsControllerOffButtonDisplayed());
            VerifyEqual("8. Verify On button is displayed", true, batchControlPage.RealTimeBatchPanel.IsControllerOnButtonDisplayed());
            VerifyEqual("8. Verify Back to auto button is displayed", true, batchControlPage.RealTimeBatchPanel.IsControllerBackToAutoButtonDisplayed());
            VerifyEqual("8. Verify Output Dropdownlist is displayed", true, batchControlPage.RealTimeBatchPanel.IsControllerOutputDropDownDisplayed());
            VerifyEqual("8. Verify Output Dropdownlist containing: Output 1, Output 2", new List<string> { "Output 1", "Output 2" }, batchControlPage.RealTimeBatchPanel.GetListOfControllerOutputItems());

            Step("9. Verify the grid of widget displays the controller info with these columns");
            Step(" o Name");
            Step(" o Geozone");
            Step(" o Progress");
            Step(" o Local Time");
            Step(" o Digital Output 1");
            Step(" o Digital Output 2");
            Step(" o ModeOutput1");
            Step(" o ModeOutput2");
            Step(" o Digital Input 1");
            Step(" o Cabinet wattage");
            var expectedColumns = new List<string> { "Name", "Geozone", "Progress", "Digital Output 1", "Digital Output 2", "ModeOutput1", "ModeOutput2", "Digital Input 1", "Cabinet wattage" };
            VerifyEqual("9. Verify the grid of widget displays the controller info with these columns", expectedColumns, batchControlPage.RealTimeBatchPanel.GetListOfControllerColumnHeaders());

            Step("10. Select the streelight on the map");
            batchControlPage.Map.SelectDeviceGL(streetlight.Longitude, streetlight.Latitude);

            Step("11. Verify The widget is changed with");
            Step(" o The new tab was added and named 'StreetLight'");
            Step(" o Some new things added: Refresh, Off, Dimming Bar (100% as default), On, Back to Auto");
            VerifyEqual("11. Verify The new tab was added and named 'StreetLight'", "StreetLight", batchControlPage.RealTimeBatchPanel.GetActivedTabName());
            VerifyEqual("11. Verify Refresh button is displayed", true, batchControlPage.RealTimeBatchPanel.IsStreetlightRefreshButtonDisplayed());
            VerifyEqual("11. Verify Off button is displayed", true, batchControlPage.RealTimeBatchPanel.IsStreetlightDimmingOffButtonDisplayed());
            VerifyEqual("11. Verify On button is displayed", true, batchControlPage.RealTimeBatchPanel.IsStreetlightDimmingOnButtonDisplayed());
            VerifyEqual("11. Verify Back to auto button is displayed", true, batchControlPage.RealTimeBatchPanel.IsStreetlightBackToAutoButtonDisplayed());
            VerifyEqual("11. Verify Streetlight Dimming Bar is displayed", true, batchControlPage.RealTimeBatchPanel.IsStreetlightDimmingBarDisplayed());
            VerifyEqual("11. Verify Streetlight Dimming Bar is 100% as default", "100%", batchControlPage.RealTimeBatchPanel.GetStreetlightDimmingRangeValueText());

            Step("12. Verify the grid of widget displays the streetlight info with these columns");
            Step(" o Name");
            Step(" o Geozone");
            Step(" o Progress");
            Step(" o Local Time");
            Step(" o Lamp wattage (W)");
            Step(" o Mains current");
            Step(" o Mains voltage (V)");
            Step(" o Sum power factor");
            Step(" o Metered power (W)");
            expectedColumns = new List<string> { "Name", "Geozone", "Progress", "Lamp wattage (W)", "Mains current", "Mains voltage (V)", "Sum power factor", "Metered power (W)" };
            VerifyEqual("12. Verify the grid of widget displays the streetlight info with these columns", expectedColumns, batchControlPage.RealTimeBatchPanel.GetListOfStreetlightColumnHeaders());

            Step("13. Select the old tab 'Controller Device'");
            batchControlPage.RealTimeBatchPanel.SelectTab("Controller Device");
            var controllers = batchControlPage.RealTimeBatchPanel.GetListOfControllerColumnData("Name");

            Step("14. Verify The grid is empty now.");
            VerifyEqual("14. Verify The grid is empty now", true, !controllers.Any());

            Step("15. Select the switch on the map");
            batchControlPage.Map.SelectDeviceGL(switchDevice.Longitude, switchDevice.Latitude);

            Step("16. Verify The widget is changed with");
            Step(" o The new tab was added and named 'Switch Device'");
            Step(" o Some new things added: Refresh, Off, On, Back to Auto");
            VerifyEqual("16. Verify The new tab was added and named 'Switch Device'", "Switch Device", batchControlPage.RealTimeBatchPanel.GetActivedTabName());
            VerifyEqual("16. Verify Refresh button is displayed", true, batchControlPage.RealTimeBatchPanel.IsSwitchRefreshButtonDisplayed());
            VerifyEqual("16. Verify Off button is displayed", true, batchControlPage.RealTimeBatchPanel.IsSwitchOffButtonDisplayed());
            VerifyEqual("16. Verify On button is displayed", true, batchControlPage.RealTimeBatchPanel.IsSwitchOnButtonDisplayed());
            VerifyEqual("16. Verify Back to auto button is displayed", true, batchControlPage.RealTimeBatchPanel.IsSwitchBackToAutoButtonDisplayed());

            Step("17. Verify the grid of widget displays the switch info with these columns");
            Step(" o Name");
            Step(" o Geozone");
            Step(" o Progress");
            Step(" o Local Time");
            Step(" o Lamp wattage (W)");
            Step(" o Mains current");
            Step(" o Mains voltage (V)");
            Step(" o Sum power factor");
            Step(" o Metered power (W)");
            expectedColumns = new List<string> { "Name", "Geozone", "Progress", "Lamp wattage (W)", "Mains current", "Mains voltage (V)", "Sum power factor", "Metered power (W)" };
            VerifyEqual("17. Verify the grid of widget displays the switch info with these columns", expectedColumns, batchControlPage.RealTimeBatchPanel.GetListOfSwitchColumnHeaders());

            Step("18. Select the old tabs 'Controller Device' & 'Streetlight'");
            batchControlPage.RealTimeBatchPanel.SelectTab("Controller Device");
            controllers = batchControlPage.RealTimeBatchPanel.GetListOfControllerColumnData("Name");
            batchControlPage.RealTimeBatchPanel.SelectTab("StreetLight");
            var streetlights = batchControlPage.RealTimeBatchPanel.GetListOfStreetlightColumnData("Name");

            Step("19. Verify The grids are empty now.");
            VerifyEqual("19. Verify The Controller grid is empty now", true, !controllers.Any());
            VerifyEqual("19. Verify The Streetlight grid is empty now", true, !streetlights.Any());

            Step("20. Select the Electrical Counter on the map");
            batchControlPage.Map.SelectDeviceGL(meter.Longitude, meter.Latitude);

            Step("21. Verify The widget is changed with");
            Step(" o The new tab was added and named 'Electrical Counter'");
            Step(" o Some new things added: Refresh");
            VerifyEqual("21. Verify The new tab was added and named 'Electrical Counter'", "Electrical Counter", batchControlPage.RealTimeBatchPanel.GetActivedTabName());
            VerifyEqual("21. Verify Refresh button is displayed", true, batchControlPage.RealTimeBatchPanel.IsElectricalCounterRefreshButtonDisplayed());

            Step("22. Verify the grid of widget displays the electrical counter info with these columns");
            Step(" o Name");
            Step(" o Geozone");
            Step(" o Progress");
            Step(" o Local Time");
            Step(" o Active power (W)");
            Step(" o Sum apparent power (VA)");
            Step(" o Sum reactive power (VAR)");
            Step(" o Demand power (W)");
            Step(" o Active energy (KWh)");
            Step(" o Total reactive energy (kVARh)");
            Step(" o Apparent energy (kVAh)");
            Step(" o Frequency (Hz)");
            Step(" o Sum power factor");
            expectedColumns = new List<string> { "Name", "Geozone", "Progress", "Active power (W)", "Sum apparent power (VA)", "Sum reactive power (VAR)", "Demand power (W)", "Active energy (KWh)", "Total reactive energy (kVARh)", "Apparent energy (kVAh)", "Frequency (Hz)", "Sum power factor" };
            VerifyEqual("22. Verify the grid of widget displays the switch info with these columns", expectedColumns, batchControlPage.RealTimeBatchPanel.GetListOfElectricalCounterColumnHeaders());

            Step("23. Select the old tabs 'Controller Device', 'Streetlight' & 'Switch Device'");
            batchControlPage.RealTimeBatchPanel.SelectTab("Controller Device");
            controllers = batchControlPage.RealTimeBatchPanel.GetListOfControllerColumnData("Name");
            batchControlPage.RealTimeBatchPanel.SelectTab("StreetLight");
            streetlights = batchControlPage.RealTimeBatchPanel.GetListOfStreetlightColumnData("Name");
            batchControlPage.RealTimeBatchPanel.SelectTab("Switch Device");
            var switchDevices = batchControlPage.RealTimeBatchPanel.GetListOfSwitchColumnData("Name");

            Step("24. Verify The grids are empty now.");
            VerifyEqual("24. Verify The Controller grid is empty now", true, !controllers.Any());
            VerifyEqual("24. Verify The Streetlight grid is empty now", true, !streetlights.Any());
            VerifyEqual("24. Verify The Switch grid is empty now", true, !switchDevices.Any());

            Step("25. Press Shift and drag to select all 4 devices");
            batchControlPage.Map.SelectDevicesGL(devicesLatLng);

            Step("26. Verify The widget is changed with");
            Step(" o Title: Selected Devices (4 devices)");
            VerifyEqual("26. Verify Title: Selected Devices (4 devices)", "Selected Devices (4 devices)", batchControlPage.RealTimeBatchPanel.GetPanelTitleText());

            Step("27. Select random a device on the map again");
            var rndDevice = devices.PickRandom();
            devices.Remove(rndDevice);
            batchControlPage.Map.SelectDeviceGL(rndDevice.Longitude, rndDevice.Latitude);

            Step("28. Verify The widget is changed with");
            Step(" o The tab of selected device is selected in the widget");
            Step(" o Title: Selected Devices (1 devices)");
            var expectedTabName = batchControlPage.RealTimeBatchPanel.GetTabName(rndDevice.Type);
            VerifyEqual("28. Verify The tab of selected device is selected in the widget", expectedTabName, batchControlPage.RealTimeBatchPanel.GetActivedTabName());
            VerifyEqual("28. Verify Title: Selected Devices (1 devices)", "Selected Devices (1 devices)", batchControlPage.RealTimeBatchPanel.GetPanelTitleText());

            try
            {
                DeleteGeozone(geozone);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("BC_09 Custom Search via devices attributes with operator Equal")]
        public void BC_09()
        {
            var geozone = SLVHelper.GenerateUniqueName("GZNBC09");
            var controller = SLVHelper.GenerateUniqueName("CTRL");
            var streetlight1 = SLVHelper.GenerateUniqueName("STL01");
            var streetlight2 = SLVHelper.GenerateUniqueName("STL02");
            var streetlight3 = SLVHelper.GenerateUniqueName("STL03");
            var streetlight4 = SLVHelper.GenerateUniqueName("STL04");
            var streetlight5 = SLVHelper.GenerateUniqueName("STL05");
            var streetlight6 = SLVHelper.GenerateUniqueName("STL06");

            Step("**** Precondition ****");
            Step(" - User has logged in successful");
            Step(" - Batch Control is installed successfully");
            Step(" - Covered the following attributes:");
            Step(" o Controller ID");
            Step(" o Unique address");
            Step(" o Configuration status");
            Step(" o Address 2");
            Step(" o Customer name");
            Step(" o Luminaire model");
            Step(" o Software version");
            Step(" o Name");
            Step(" o Address 1");
            Step(" o Identifier");
            Step(" o City");
            Step(" o Zip code");
            Step(" - Pick up randomly 3 attributes and go to Equipment Inventory to set the testing values for a streetlight. There is one exception that Software version belongs to Controller not Streetlight");
            Step("**** Precondition ****\n");

            Step("-> Pick up random 3 attributes");
            var attributes = new List<string>
            {
                "Configuration status",
                "Address 2",
                "Customer name",
                "Customer number",
                "Luminaire model",
                "Address 1",
                "Identifier",
                "City",
                "Zip code"
            };

            var dictionary = new Dictionary<string, string>()
            {
                {"Controller ID", "controllerStrId" },
                {"Unique address", "MacAddress"},
                {"Configuration status", "ConfigStatus"},
                {"Address 2", "location.streetdescription"},
                {"Customer name", "client.name"},
                {"Customer number", "client.number" },
                {"Luminaire model", "luminaire.model"},
                {"Software version", "SoftwareVersion"},
                {"Name", "name"},
                {"Address 1", "address"},
                {"Identifier", "idOnController"},
                {"City", "location.city"},
                {"Zip code", "location.zipcode"}
            };

            var softwareVersion = "Software version";
            var firstAttribute = attributes.PickRandom();
            attributes.Remove(firstAttribute);
            var secondAttribute = attributes.PickRandom();
            attributes.Remove(secondAttribute);
            var thirdAttribute = attributes.PickRandom();
            var controllerID = "Controller ID";
            var uniqueAddress = "Unique address";
            var randomUniqueAddress = SLVHelper.GenerateMACAddress();
            var name = "Name";

            Step("-> Creating data for testing");
            DeleteGeozones("GZNBC09*");
            CreateNewGeozone(geozone);
            CreateNewController(controller, geozone);
            CreateNewDevice(DeviceType.Streetlight, streetlight1, controller, geozone);
            CreateNewDevice(DeviceType.Streetlight, streetlight2, controller, geozone);
            CreateNewDevice(DeviceType.Streetlight, streetlight3, controller, geozone);
            CreateNewDevice(DeviceType.Streetlight, streetlight4, controller, geozone);
            CreateNewDevice(DeviceType.Streetlight, streetlight5, controller, geozone);
            CreateNewDevice(DeviceType.Streetlight, streetlight6, controller, geozone);

            var requestToSetSoftwareVersion = SetValueToController(controller, dictionary[softwareVersion], softwareVersion, Settings.GetServerTime());
            var requestToSetFirstAttribute = SetValueToDevice(controller, streetlight1, dictionary[firstAttribute], firstAttribute, Settings.GetServerTime());
            var requestToSetSecondAttribute = SetValueToDevice(controller, streetlight2, dictionary[secondAttribute], secondAttribute, Settings.GetServerTime());
            var requestToSetThirdAttribute = SetValueToDevice(controller, streetlight3, dictionary[thirdAttribute], thirdAttribute, Settings.GetServerTime());
            var requestToSetControllerID = SetValueToDevice(controller, streetlight4, dictionary[controllerID], controller, Settings.GetServerTime());
            var requestToSetUniqueAddress = SetValueToDevice(controller, streetlight5, dictionary[uniqueAddress], randomUniqueAddress, Settings.GetServerTime());
            var requestToSetName = SetValueToDevice(controller, streetlight6, dictionary[name], name, Settings.GetServerTime());

            VerifyEqual(string.Format("-> Verify the requestToSetSoftwareVersion is sent successfully (attribute: {0}, value: {1})", softwareVersion, softwareVersion), true, requestToSetSoftwareVersion);
            VerifyEqual(string.Format("-> Verify the requestToSetFirstAttribute is sent successfully (attribute: {0}, value: {1})", firstAttribute, firstAttribute), true, requestToSetFirstAttribute);
            VerifyEqual(string.Format("-> Verify requestToSetSecondAttribute is sent successfully (attribute: {0}, value: {1})", secondAttribute, secondAttribute), true, requestToSetSecondAttribute);
            VerifyEqual(string.Format("-> Verify requestToSetThirdAttribute is sent successfully (attribute: {0}, value: {1})", thirdAttribute, thirdAttribute), true, requestToSetThirdAttribute);
            VerifyEqual(string.Format("-> Verify requestToSetControllerID is sent successfully (attribute: {0}, value: {1})", controllerID, controllerID), true, requestToSetControllerID);
            VerifyEqual(string.Format("-> Verify requestToSetUniqueAddress is sent successfully (attribute: {0}, value: {1})", uniqueAddress, randomUniqueAddress), true, requestToSetUniqueAddress);
            VerifyEqual(string.Format("-> Verify requestToSetName is sent successfully (attribute: {0}, value: {1})", name, name), true, requestToSetName);

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.BatchControl);

            Step("1. Go to Batch Control app and select the Geozones > testing geozone");
            var batchControlPage = desktopPage.GoToApp(App.BatchControl) as BatchControlPage;
            batchControlPage.GeozoneTreeMainPanel.SelectNode(geozone);

            var firstCombobox = batchControlPage.RealTimeBatchPanel.GetAttributeValue();
            var secondCombobox = batchControlPage.RealTimeBatchPanel.GetOperatorValue();
            var thirdCombobox = batchControlPage.RealTimeBatchPanel.GetSearchNameInputDefaultValue();
            var searchIconDisplayed = batchControlPage.RealTimeBatchPanel.IsSearchButtonDisplayed();
            Step("1. Verify There is a section name: Custom search with");
            Step(" o Verify the 1st attributes combobox with Software version selecting as default");
            Step(" o Verify the 2nd operator combobox with Contains selecting as default");
            Step(" o Verify the 3rd box with text 'Software version'");
            Step(" o A Search icon");
            if (firstCombobox.Equals("Name") && secondCombobox.Equals("Contains") && thirdCombobox.Equals("Name"))
            {
                VerifyEqual("Verify the 1st attributes combobox with Controller ID selecting as default", "Name", firstCombobox);
                VerifyEqual("Verify the 2nd operator combobox with Equal selecting as default", "Contains", secondCombobox);
                VerifyEqual("Verify the 3rd box with text 'Controller ID'", "Name", thirdCombobox);
                VerifyEqual("Verify a Search icon", true, searchIconDisplayed);
            }
            else
            {
                Info("Custom search depends on custom config.");
            }

            Step("2. Select a testing attribute {0}", softwareVersion);
            batchControlPage.RealTimeBatchPanel.SelectAttributeDropDown(softwareVersion);
            var valueOfAttributeDropDown_SoftwareVersion = batchControlPage.RealTimeBatchPanel.GetAttributeValue();
            var valueOfSearchNameInput_SoftwareVersion = batchControlPage.RealTimeBatchPanel.GetSearchNameInputDefaultValue();
            VerifyEqual("2. Verify the testing attribute is selected", softwareVersion, valueOfAttributeDropDown_SoftwareVersion);
            VerifyEqual("2. Verify the text in the 3rd box is updated to the selected attribute", softwareVersion, valueOfSearchNameInput_SoftwareVersion);

            Step("3. Select the operator 'Equal'");
            batchControlPage.RealTimeBatchPanel.SelectOperatorDropDown("Equal");
            var valueOfOperatorDropDown_SoftwareVersion = batchControlPage.RealTimeBatchPanel.GetOperatorValue();
            VerifyEqual("3. Verify The operator Equal is selected", "Equal", valueOfOperatorDropDown_SoftwareVersion);

            Step("4. Input the value of that attribute into the search box and press Search icon");
            batchControlPage.RealTimeBatchPanel.EnterSearchNameInput(softwareVersion);
            batchControlPage.RealTimeBatchPanel.ClickSearchButton();
            batchControlPage.WaitForPreviousActionComplete();
            var tabOfControllDevice_SoftwareVersion = batchControlPage.RealTimeBatchPanel.GetListOfTabsName().FirstOrDefault();
            var isControllerFound_SoftwareVersion = batchControlPage.RealTimeBatchPanel.GetListOfControllerColumnData("Name").Contains(controller);
            VerifyEqual("4. Verify there is a new tab name 'Controller Device'", "Controller Device", tabOfControllDevice_SoftwareVersion);
            VerifyEqual("4. Verify the tab contains the name of testing controller", true, isControllerFound_SoftwareVersion);

            Step("5. Update the value in the search box to invalid value and press Search icon");
            batchControlPage.RealTimeBatchPanel.EnterSearchNameInput(SLVHelper.GenerateString(5));
            batchControlPage.RealTimeBatchPanel.ClickSearchButton();
            batchControlPage.WaitForPreviousActionComplete();
            var numberOfTabs_SoftwareVersion = batchControlPage.RealTimeBatchPanel.GetListOfTabsName().Count;
            VerifyEqual("5. Verify The Controller Device tab disappeared", 0, numberOfTabs_SoftwareVersion);

            Step("6. Set the testing value into the searchbox and press Search icon again");
            batchControlPage.RealTimeBatchPanel.EnterSearchNameInput(softwareVersion);
            batchControlPage.RealTimeBatchPanel.ClickSearchButton();
            batchControlPage.WaitForPreviousActionComplete();
            var tabOfControllDevice_SoftwareVersion2 = batchControlPage.RealTimeBatchPanel.GetListOfTabsName().FirstOrDefault();
            var isControllerFound_SoftwareVersion2 = batchControlPage.RealTimeBatchPanel.GetListOfControllerColumnData("Name").Contains(controller);
            VerifyEqual("6. Verify there is a new tab name 'Controller Device'", "Controller Device", tabOfControllDevice_SoftwareVersion2);
            VerifyEqual("6. Verify the tab contains the name of testing controller", true, isControllerFound_SoftwareVersion2);

            Step("7. Select a testing attribute {0}", firstAttribute);
            batchControlPage.RealTimeBatchPanel.SelectAttributeDropDown(firstAttribute);
            var valueOfAttributeDropDown_FirstAttribute = batchControlPage.RealTimeBatchPanel.GetAttributeValue();
            var valueOfSearchNameInput_FirstAttribute = batchControlPage.RealTimeBatchPanel.GetSearchNameInputDefaultValue();
            VerifyEqual("7. Verify the testing attribute is selected", firstAttribute, valueOfAttributeDropDown_FirstAttribute);
            VerifyEqual("7. Verify the text in the 3rd box is updated to the selected attribute", firstAttribute, valueOfSearchNameInput_FirstAttribute);

            Step("8. Select the operator 'Equal'");
            batchControlPage.RealTimeBatchPanel.SelectOperatorDropDown("Equal");
            var valueOfOperatorDropDown_FirstAttribute = batchControlPage.RealTimeBatchPanel.GetOperatorValue();
            VerifyEqual("8. Verify The operator Equal is selected", "Equal", valueOfOperatorDropDown_FirstAttribute);

            Step("9. Input the value of that attribute into the search box and press Search icon");
            batchControlPage.RealTimeBatchPanel.EnterSearchNameInput(firstAttribute);
            batchControlPage.RealTimeBatchPanel.ClickSearchButton();
            batchControlPage.WaitForPreviousActionComplete();
            var tabOfStreetlight_FirstAttribute = batchControlPage.RealTimeBatchPanel.GetListOfTabsName().FirstOrDefault();
            var isStreetlightFound_FirstAttribute = batchControlPage.RealTimeBatchPanel.GetListOfStreetlightColumnData("Name").Contains(streetlight1);
            VerifyEqual("9. Verify there is a new tab name 'StreetLight'", "StreetLight", tabOfStreetlight_FirstAttribute);
            VerifyEqual("9. Verify the tab contains the name of testing streetlight", true, isStreetlightFound_FirstAttribute);

            Step("10. Update the value in the search box to invalid value and press Search icon");
            batchControlPage.RealTimeBatchPanel.EnterSearchNameInput(SLVHelper.GenerateString(5));
            batchControlPage.RealTimeBatchPanel.ClickSearchButton();
            batchControlPage.WaitForPreviousActionComplete();
            var numberOfTabs_FirstAttribute = batchControlPage.RealTimeBatchPanel.GetListOfTabsName().Count;
            VerifyEqual("10. Verify The Streetlight tab disappeared", 0, numberOfTabs_FirstAttribute);

            Step("11. Set the testing value into the searchbox and press Search icon again");
            batchControlPage.RealTimeBatchPanel.EnterSearchNameInput(firstAttribute);
            batchControlPage.RealTimeBatchPanel.ClickSearchButton();
            batchControlPage.WaitForPreviousActionComplete();
            var tabOfStreetlight_FirstAttribute2 = batchControlPage.RealTimeBatchPanel.GetListOfTabsName().FirstOrDefault();
            var isStreetlightFound_FirstAttribute2 = batchControlPage.RealTimeBatchPanel.GetListOfStreetlightColumnData("Name").Contains(streetlight1);
            VerifyEqual("11. Verify there is a new tab name 'StreetLight'", "StreetLight", tabOfStreetlight_FirstAttribute2);
            VerifyEqual("11. Verify the tab contains the name of testing streetlight", true, isStreetlightFound_FirstAttribute2);

            Step("12. Select a testing attribute {0}", secondAttribute);
            batchControlPage.RealTimeBatchPanel.SelectAttributeDropDown(secondAttribute);
            var valueOfAttributeDropDown_SecondAttribute = batchControlPage.RealTimeBatchPanel.GetAttributeValue();
            var valueOfSearchNameInput_SecondAttribute = batchControlPage.RealTimeBatchPanel.GetSearchNameInputDefaultValue();
            VerifyEqual("12. Verify the testing attribute is selected", secondAttribute, valueOfAttributeDropDown_SecondAttribute);
            VerifyEqual("12. Verify the text in the 3rd box is updated to the selected attribute", secondAttribute, valueOfSearchNameInput_SecondAttribute);

            Step("13. Select the operator 'Equal'");
            batchControlPage.RealTimeBatchPanel.SelectOperatorDropDown("Equal");
            var valueOfOperatorDropDown_SecondAttribute = batchControlPage.RealTimeBatchPanel.GetOperatorValue();
            VerifyEqual("13. Verify The operator Equal is selected", "Equal", valueOfOperatorDropDown_SecondAttribute);

            Step("14. Input the value of that attribute into the search box and press Search icon");
            batchControlPage.RealTimeBatchPanel.EnterSearchNameInput(secondAttribute);
            batchControlPage.RealTimeBatchPanel.ClickSearchButton();
            batchControlPage.WaitForPreviousActionComplete();
            var tabOfStreetlight_SecondAttribute = batchControlPage.RealTimeBatchPanel.GetListOfTabsName().FirstOrDefault();
            var isStreetlightFound_SecondAttribute = batchControlPage.RealTimeBatchPanel.GetListOfStreetlightColumnData("Name").Contains(streetlight2);
            VerifyEqual("14. Verify there is a new tab name 'StreetLight'", "StreetLight", tabOfStreetlight_SecondAttribute);
            VerifyEqual("14. Verify the tab contains the name of testing streetlight", true, isStreetlightFound_SecondAttribute);

            Step("15. Update the value in the search box to invalid value and press Search icon");
            batchControlPage.RealTimeBatchPanel.EnterSearchNameInput(SLVHelper.GenerateString(5));
            batchControlPage.RealTimeBatchPanel.ClickSearchButton();
            batchControlPage.WaitForPreviousActionComplete();
            var numberOfTabs_SecondAttribute = batchControlPage.RealTimeBatchPanel.GetListOfTabsName().Count;
            VerifyEqual("15. Verify The Streetlight tab disappeared", 0, numberOfTabs_SecondAttribute);

            Step("16. Set the testing value into the searchbox and press Search icon again");
            batchControlPage.RealTimeBatchPanel.EnterSearchNameInput(secondAttribute);
            batchControlPage.RealTimeBatchPanel.ClickSearchButton();
            batchControlPage.WaitForPreviousActionComplete();
            var tabOfStreetlight_SecondAttribute2 = batchControlPage.RealTimeBatchPanel.GetListOfTabsName().FirstOrDefault();
            var isStreetlightFound_SecondAttribute2 = batchControlPage.RealTimeBatchPanel.GetListOfStreetlightColumnData("Name").Contains(streetlight2);
            VerifyEqual("16. Verify there is a new tab name 'StreetLight'", "StreetLight", tabOfStreetlight_SecondAttribute2);
            VerifyEqual("16. Verify the tab contains the name of testing streetlight", true, isStreetlightFound_SecondAttribute2);

            Step("17. Select a testing attribute {0}", thirdAttribute);
            batchControlPage.RealTimeBatchPanel.SelectAttributeDropDown(thirdAttribute);
            var valueOfAttributeDropDown_ThirdAttribute = batchControlPage.RealTimeBatchPanel.GetAttributeValue();
            var valueOfSearchNameInput_ThirdAttribute = batchControlPage.RealTimeBatchPanel.GetSearchNameInputDefaultValue();
            VerifyEqual("17. Verify the testing attribute is selected", thirdAttribute, valueOfAttributeDropDown_ThirdAttribute);
            VerifyEqual("17. Verify the text in the 3rd box is updated to the selected attribute", thirdAttribute, valueOfSearchNameInput_ThirdAttribute);

            Step("18. Select the operator 'Equal'");
            batchControlPage.RealTimeBatchPanel.SelectOperatorDropDown("Equal");
            var valueOfOperatorDropDown_ThirdAttribute = batchControlPage.RealTimeBatchPanel.GetOperatorValue();
            VerifyEqual("18. Verify The operator Equal is selected", "Equal", valueOfOperatorDropDown_ThirdAttribute);

            Step("19. Input the value of that attribute into the search box and press Search icon");
            batchControlPage.RealTimeBatchPanel.EnterSearchNameInput(thirdAttribute);
            batchControlPage.RealTimeBatchPanel.ClickSearchButton();
            batchControlPage.WaitForPreviousActionComplete();
            var tabOfStreetlight_ThirdAttribute = batchControlPage.RealTimeBatchPanel.GetListOfTabsName().FirstOrDefault();
            var isStreetlightFound_ThirdAttribute = batchControlPage.RealTimeBatchPanel.GetListOfStreetlightColumnData("Name").Contains(streetlight3);
            VerifyEqual("19. Verify there is a new tab name 'StreetLight'", "StreetLight", tabOfStreetlight_ThirdAttribute);
            VerifyEqual("19. Verify the tab contains the name of testing streetlight", true, isStreetlightFound_ThirdAttribute);

            Step("20. Update the value in the search box to invalid value and press Search icon");
            batchControlPage.RealTimeBatchPanel.EnterSearchNameInput(SLVHelper.GenerateString(5));
            batchControlPage.RealTimeBatchPanel.ClickSearchButton();
            batchControlPage.WaitForPreviousActionComplete();
            var numberOfTabs_ThirdAttribute = batchControlPage.RealTimeBatchPanel.GetListOfTabsName().Count;
            VerifyEqual("20. Verify The Streetlight tab disappeared", 0, numberOfTabs_ThirdAttribute);

            Step("21. Set the testing value into the searchbox and press Search icon again");
            batchControlPage.RealTimeBatchPanel.EnterSearchNameInput(thirdAttribute);
            batchControlPage.RealTimeBatchPanel.ClickSearchButton();
            batchControlPage.WaitForPreviousActionComplete();
            var tabOfStreetlight_ThirdAttribute2 = batchControlPage.RealTimeBatchPanel.GetListOfTabsName().FirstOrDefault();
            var isStreetlightFound_ThirdAttribute2 = batchControlPage.RealTimeBatchPanel.GetListOfStreetlightColumnData("Name").Contains(streetlight3);
            VerifyEqual("21. Verify there is a new tab name 'StreetLight'", "StreetLight", tabOfStreetlight_ThirdAttribute2);
            VerifyEqual("21. Verify the tab contains the name of testing streetlight", true, isStreetlightFound_ThirdAttribute2);

            Step("22. Select a testing attribute {0}", controllerID);
            batchControlPage.RealTimeBatchPanel.SelectAttributeDropDown(controllerID);
            var valueOfAttributeDropDown_ControllerID = batchControlPage.RealTimeBatchPanel.GetAttributeValue();
            var valueOfSearchNameInput_ControllerID = batchControlPage.RealTimeBatchPanel.GetDropDownMenuDefaultValue();
            VerifyEqual("22. Verify the testing attribute is selected", controllerID, valueOfAttributeDropDown_ControllerID);
            VerifyEqual("22. Verify the text in the 3rd box is updated to the selected attribute", controllerID, valueOfSearchNameInput_ControllerID);

            Step("23. Select the operator 'Equal'");
            batchControlPage.RealTimeBatchPanel.SelectOperatorDropDown("Equal");
            var valueOfOperatorDropDown_ControllerID = batchControlPage.RealTimeBatchPanel.GetOperatorValue();
            VerifyEqual("23. Verify The operator Equal is selected", "Equal", valueOfOperatorDropDown_ControllerID);

            Step("24. Input the value of that attribute into the search box and press Search icon");
            batchControlPage.RealTimeBatchPanel.SelectValueDropDownMenu(controller);
            batchControlPage.RealTimeBatchPanel.ClickSearchButton();
            batchControlPage.WaitForPreviousActionComplete();
            var tabOfStreetlight_ControllerID = batchControlPage.RealTimeBatchPanel.GetListOfTabsName().ElementAtOrDefault(1);
            var isStreetlightFound_ControllerID = batchControlPage.RealTimeBatchPanel.GetListOfStreetlightColumnData("Name").Contains(streetlight4);
            VerifyEqual("24. Verify there is a new tab name 'StreetLight'", "StreetLight", tabOfStreetlight_ControllerID);
            VerifyEqual("24. Verify the tab contains the name of testing streetlight", true, isStreetlightFound_ControllerID);

            Step("25. Update the value in the search box to invalid value and press Search icon");
            batchControlPage.RealTimeBatchPanel.EnterSearchNameInput(SLVHelper.GenerateString(5));
            var isNoMatchesFoundDisplayed_ControllerID = batchControlPage.RealTimeBatchPanel.NoMatchesFoundIsDisplayedInDropDownMenu();
            VerifyEqual("25. Verify The Streetlight tab disappeared", true, isNoMatchesFoundDisplayed_ControllerID);

            Step("26. Set the testing value into the searchbox and press Search icon again");
            batchControlPage.RealTimeBatchPanel.SelectValueDropDownMenu(controller);
            batchControlPage.RealTimeBatchPanel.ClickSearchButton();
            batchControlPage.WaitForPreviousActionComplete();
            var tabOfStreetlight_ControllerID2 = batchControlPage.RealTimeBatchPanel.GetListOfTabsName().ElementAtOrDefault(1);
            var isStreetlightFound_ControllerID2 = batchControlPage.RealTimeBatchPanel.GetListOfStreetlightColumnData("Name").Contains(streetlight4);
            VerifyEqual("26. Verify there is a new tab name 'StreetLight'", "StreetLight", tabOfStreetlight_ControllerID2);
            VerifyEqual("26. Verify the tab contains the name of testing streetlight", true, isStreetlightFound_ControllerID2);

            Step("27. Select a testing attribute {0}", uniqueAddress);
            batchControlPage.RealTimeBatchPanel.SelectAttributeDropDown(uniqueAddress);
            var valueOfAttributeDropDown_UniqueAddress = batchControlPage.RealTimeBatchPanel.GetAttributeValue();
            var valueOfSearchNameInput_UniqueAddress = batchControlPage.RealTimeBatchPanel.GetSearchNameInputDefaultValue();
            VerifyEqual("27. Verify the testing attribute is selected", uniqueAddress, valueOfAttributeDropDown_UniqueAddress);
            VerifyEqual("27. Verify the text in the 3rd box is updated to the selected attribute", uniqueAddress, valueOfSearchNameInput_UniqueAddress);

            Step("29. Select the operator 'Equal'");
            batchControlPage.RealTimeBatchPanel.SelectOperatorDropDown("Equal");
            var valueOfOperatorDropDown_UniqueAddress = batchControlPage.RealTimeBatchPanel.GetOperatorValue();
            VerifyEqual("29. Verify The operator Equal is selected", "Equal", valueOfOperatorDropDown_UniqueAddress);

            Step("30. Input the value of that attribute into the search box and press Search icon");
            batchControlPage.RealTimeBatchPanel.EnterSearchNameInput(randomUniqueAddress);
            batchControlPage.RealTimeBatchPanel.ClickSearchButton();
            batchControlPage.WaitForPreviousActionComplete();
            var tabOfStreetlight_UniqueAddress = batchControlPage.RealTimeBatchPanel.GetListOfTabsName().FirstOrDefault();
            var isStreetlightFound_UniqueAddress = batchControlPage.RealTimeBatchPanel.GetListOfStreetlightColumnData("Name").Contains(streetlight5);
            VerifyEqual("30. Verify there is a new tab name 'StreetLight'", "StreetLight", tabOfStreetlight_UniqueAddress);
            VerifyEqual("30. Verify the tab contains the name of testing streetlight", true, isStreetlightFound_UniqueAddress);

            Step("31. Update the value in the search box to invalid value and press Search icon");
            batchControlPage.RealTimeBatchPanel.EnterSearchNameInput(SLVHelper.GenerateString(5));
            batchControlPage.RealTimeBatchPanel.ClickSearchButton();
            batchControlPage.WaitForPreviousActionComplete();
            var numberOfTabs_UniqueAddress = batchControlPage.RealTimeBatchPanel.GetListOfTabsName().Count;
            VerifyEqual("31. Verify The Streetlight tab disappeared", 0, numberOfTabs_UniqueAddress);

            Step("32. Set the testing value into the searchbox and press Search icon again");
            batchControlPage.RealTimeBatchPanel.EnterSearchNameInput(randomUniqueAddress);
            batchControlPage.RealTimeBatchPanel.ClickSearchButton();
            batchControlPage.WaitForPreviousActionComplete();
            var tabOfStreetlight_UniqueAddress2 = batchControlPage.RealTimeBatchPanel.GetListOfTabsName().FirstOrDefault();
            var isStreetlightFound_UniqueAddress2 = batchControlPage.RealTimeBatchPanel.GetListOfStreetlightColumnData("Name").Contains(streetlight5);
            VerifyEqual("32. Verify there is a new tab name 'StreetLight'", "StreetLight", tabOfStreetlight_UniqueAddress2);
            VerifyEqual("32. Verify the tab contains the name of testing streetlight", true, isStreetlightFound_UniqueAddress2);

            Step("33. Select a testing attribute {0}", name);
            batchControlPage.RealTimeBatchPanel.SelectAttributeDropDown(name);
            var valueOfAttributeDropDown_Name = batchControlPage.RealTimeBatchPanel.GetAttributeValue();
            var valueOfSearchNameInput_Name = batchControlPage.RealTimeBatchPanel.GetSearchNameInputDefaultValue();
            VerifyEqual("33. Verify the testing attribute is selected", name, valueOfAttributeDropDown_Name);
            VerifyEqual("33. Verify the text in the 3rd box is updated to the selected attribute", name, valueOfSearchNameInput_Name);

            Step("34. Select the operator 'Equal'");
            batchControlPage.RealTimeBatchPanel.SelectOperatorDropDown("Equal");
            var valueOfOperatorDropDown_Name = batchControlPage.RealTimeBatchPanel.GetOperatorValue();
            VerifyEqual("34. Verify The operator Equal is selected", "Equal", valueOfOperatorDropDown_Name);

            Step("35. Input the value of that attribute into the search box and press Search icon");
            batchControlPage.RealTimeBatchPanel.EnterSearchNameInput(name);
            batchControlPage.RealTimeBatchPanel.ClickSearchButton();
            batchControlPage.WaitForPreviousActionComplete();
            var tabOfStreetlight_Name = batchControlPage.RealTimeBatchPanel.GetListOfTabsName().FirstOrDefault();
            var isStreetlightFound_Name = batchControlPage.RealTimeBatchPanel.GetListOfStreetlightColumnData("Name").Contains(name);
            VerifyEqual("35. Verify there is a new tab name 'StreetLight'", "StreetLight", tabOfStreetlight_Name);
            VerifyEqual("35. Verify the tab contains the name of testing streetlight", true, isStreetlightFound_Name);

            Step("36. Update the value in the search box to invalid value and press Search icon");
            batchControlPage.RealTimeBatchPanel.EnterSearchNameInput(SLVHelper.GenerateString(5));
            batchControlPage.RealTimeBatchPanel.ClickSearchButton();
            batchControlPage.WaitForPreviousActionComplete();
            var numberOfTabs_Name = batchControlPage.RealTimeBatchPanel.GetListOfTabsName().Count;
            VerifyEqual("36. Verify The Streetlight tab disappeared", 0, numberOfTabs_Name);

            Step("37. Set the testing value into the searchbox and press Search icon again");
            batchControlPage.RealTimeBatchPanel.EnterSearchNameInput(name);
            batchControlPage.RealTimeBatchPanel.ClickSearchButton();
            batchControlPage.WaitForPreviousActionComplete();
            var tabOfStreetlight_Name2 = batchControlPage.RealTimeBatchPanel.GetListOfTabsName().FirstOrDefault();
            var isStreetlightFound_Name2 = batchControlPage.RealTimeBatchPanel.GetListOfStreetlightColumnData("Name").Contains(name);
            VerifyEqual("37. Verify there is a new tab name 'StreetLight'", "StreetLight", tabOfStreetlight_Name2);
            VerifyEqual("37. Verify the tab contains the name of testing streetlight", true, isStreetlightFound_Name2);

            try
            {
                DeleteGeozone(geozone);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("BC_10 Custom Search via devices attributes with operator Contains")]
        public void BC_10()
        {
            var geozone = SLVHelper.GenerateUniqueName("GZNBC10");
            var controller = SLVHelper.GenerateUniqueName("CTRL");
            var streetlight1 = SLVHelper.GenerateUniqueName("STL01");
            var streetlight2 = SLVHelper.GenerateUniqueName("STL02");
            var streetlight3 = SLVHelper.GenerateUniqueName("STL03");
            var streetlight4 = SLVHelper.GenerateUniqueName("STL04");
            var streetlight5 = SLVHelper.GenerateUniqueName("STL05");

            Step("**** Precondition ****");
            Step(" - User has logged in successful");
            Step(" - Batch Control is installed successfully");
            Step(" - Covered the following attributes:");
            Step(" o Unique address");
            Step(" o Configuration status");
            Step(" o Address 2");
            Step(" o Customer name");
            Step(" o Customer number");
            Step(" o Luminaire model");
            Step(" o Software version");
            Step(" o Name");
            Step(" o Address 1");
            Step(" o Identifier");
            Step(" o City");
            Step(" o Zip code");
            Step(" - Pick up randomly 3 attributes and go to Equipment Inventory to set the testing values for a streetlight.");
            Step("**** Precondition ****\n");

            Step("-> Pick up random 3 attributes");
            var attributes = new List<string>
            {
                "Configuration status",
                "Address 2",
                "Customer name",
                "Customer number",
                "Luminaire model",
                "Address 1",
                "Identifier",
                "City",
                "Zip code"
            };

            var dictionary = new Dictionary<string, string>()
            {
                {"Unique address", "MacAddress"},
                {"Configuration status", "ConfigStatus"},
                {"Address 2", "location.streetdescription"},
                {"Customer name", "client.name"},
                {"Customer number", "client.number" },
                {"Luminaire model", "luminaire.model"},
                {"Software version", "SoftwareVersion"},
                {"Name", "name"},
                {"Address 1", "address"},
                {"Identifier", "idOnController"},
                {"City", "location.city"},
                {"Zip code", "location.zipcode"}
            };

            var softwareVersion = "Software version";
            var firstAttribute = attributes.PickRandom();
            attributes.Remove(firstAttribute);
            var secondAttribute = attributes.PickRandom();
            attributes.Remove(secondAttribute);
            var thirdAttribute = attributes.PickRandom();
            var uniqueAddress = "Unique address";
            var randomUniqueAddress = SLVHelper.GenerateMACAddress();
            var name = "Name";

            var downLimit = 1;
            var uplimit = 4;
            var softwareVersionPart = softwareVersion.Substring(SLVHelper.GenerateInteger(downLimit, uplimit));
            var firstAttributePart = firstAttribute.Substring(SLVHelper.GenerateInteger(downLimit, uplimit));
            var secondAttributePart = secondAttribute.Substring(SLVHelper.GenerateInteger(downLimit, uplimit));
            var thirdAttributePart = thirdAttribute.Substring(SLVHelper.GenerateInteger(downLimit, uplimit));
            var randomUniqueAddressPart = randomUniqueAddress.Substring(SLVHelper.GenerateInteger(downLimit, uplimit));
            var namePart = name.Substring(SLVHelper.GenerateInteger(name.Length));

            Step("-> Creating data for testing");
            DeleteGeozones("GZNBC10*");
            CreateNewGeozone(geozone);
            CreateNewController(controller, geozone);
            CreateNewDevice(DeviceType.Streetlight, streetlight1, controller, geozone);
            CreateNewDevice(DeviceType.Streetlight, streetlight2, controller, geozone);
            CreateNewDevice(DeviceType.Streetlight, streetlight3, controller, geozone);
            CreateNewDevice(DeviceType.Streetlight, streetlight4, controller, geozone);
            CreateNewDevice(DeviceType.Streetlight, streetlight5, controller, geozone);

            var requestToSetSoftwareVersion = SetValueToController(controller, dictionary[softwareVersion], softwareVersion, Settings.GetServerTime());
            var requestToSetFirstAttribute = SetValueToDevice(controller, streetlight1, dictionary[firstAttribute], firstAttribute, Settings.GetServerTime());
            var requestToSetSecondAttribute = SetValueToDevice(controller, streetlight2, dictionary[secondAttribute], secondAttribute, Settings.GetServerTime());
            var requestToSetThirdAttribute = SetValueToDevice(controller, streetlight3, dictionary[thirdAttribute], thirdAttribute, Settings.GetServerTime());
            var requestToSetUniqueAddress = SetValueToDevice(controller, streetlight4, dictionary[uniqueAddress], randomUniqueAddress, Settings.GetServerTime());
            var requestToSetName = SetValueToDevice(controller, streetlight5, dictionary[name], name, Settings.GetServerTime());

            VerifyEqual(string.Format("-> Verify the requestToSetSoftwareVersion is sent successfully (attribute: {0}, value: {1})", softwareVersion, softwareVersion), true, requestToSetSoftwareVersion);
            VerifyEqual(string.Format("-> Verify the requestToSetFirstAttribute is sent successfully (attribute: {0}, value: {1})", firstAttribute, firstAttribute), true, requestToSetFirstAttribute);
            VerifyEqual(string.Format("-> Verify requestToSetSecondAttribute is sent successfully (attribute: {0}, value: {1})", secondAttribute, secondAttribute), true, requestToSetSecondAttribute);
            VerifyEqual(string.Format("-> Verify requestToSetThirdAttribute is sent successfully (attribute: {0}, value: {1})", thirdAttribute, thirdAttribute), true, requestToSetThirdAttribute);
            VerifyEqual(string.Format("-> Verify requestToSetUniqueAddress is sent successfully (attribute: {0}, value: {1})", uniqueAddress, randomUniqueAddress), true, requestToSetUniqueAddress);
            VerifyEqual(string.Format("-> Verify requestToSetName is sent successfully (attribute: {0}, value: {1})", name, name), true, requestToSetName);

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.BatchControl);

            Step("1. Go to Batch Control app and select the Geozones > testing geozone");
            var batchControlPage = desktopPage.GoToApp(App.BatchControl) as BatchControlPage;
            batchControlPage.GeozoneTreeMainPanel.SelectNode(geozone);

            var firstCombobox = batchControlPage.RealTimeBatchPanel.GetAttributeValue();
            var secondCombobox = batchControlPage.RealTimeBatchPanel.GetOperatorValue();
            var thirdCombobox = batchControlPage.RealTimeBatchPanel.GetSearchNameInputDefaultValue();
            var searchIconDisplayed = batchControlPage.RealTimeBatchPanel.IsSearchButtonDisplayed();
            Step("1. Verify There is a section name: Custom search with");
            Step(" o Verify the 1st attributes combobox with Software version selecting as default");
            Step(" o Verify the 2nd operator combobox with Contains selecting as default");
            Step(" o Verify the 3rd box with text 'Software version'");
            Step(" o A Search icon");
            if (firstCombobox.Equals("Name") && secondCombobox.Equals("Contains") && thirdCombobox.Equals("Name"))
            {
                VerifyEqual("Verify the 1st attributes combobox with Controller ID selecting as default", "Name", firstCombobox);
                VerifyEqual("Verify the 2nd operator combobox with Equal selecting as default", "Contains", secondCombobox);
                VerifyEqual("Verify the 3rd box with text 'Controller ID'", "Name", thirdCombobox);
                VerifyEqual("Verify a Search icon", true, searchIconDisplayed);
            }
            else
            {
                Info("Custom search depends on custom config.");
            }

            Step("2. Select a testing attribute {0}", softwareVersion);
            batchControlPage.RealTimeBatchPanel.SelectAttributeDropDown(softwareVersion);
            var valueOfAttributeDropDown_SoftwareVersion = batchControlPage.RealTimeBatchPanel.GetAttributeValue();
            var valueOfSearchNameInput_SoftwareVersion = batchControlPage.RealTimeBatchPanel.GetSearchNameInputDefaultValue();
            VerifyEqual("2. Verify the testing attribute is selected", softwareVersion, valueOfAttributeDropDown_SoftwareVersion);
            VerifyEqual("2. Verify the text in the 3rd box is updated to the selected attribute", softwareVersion, valueOfSearchNameInput_SoftwareVersion);

            Step("3. Select the operator 'Contains'");
            batchControlPage.RealTimeBatchPanel.SelectOperatorDropDown("Contains");
            var valueOfOperatorDropDown_SoftwareVersion = batchControlPage.RealTimeBatchPanel.GetOperatorValue();
            VerifyEqual("3. Verify The operator Contains is selected", "Contains", valueOfOperatorDropDown_SoftwareVersion);

            Step("4. Input a part of value of that attribute into the search box and press Search icon");
            batchControlPage.RealTimeBatchPanel.EnterSearchNameInput(softwareVersionPart);
            batchControlPage.RealTimeBatchPanel.ClickSearchButton();
            batchControlPage.WaitForPreviousActionComplete();
            var tabOfControllDevice_SoftwareVersion = batchControlPage.RealTimeBatchPanel.GetListOfTabsName().FirstOrDefault();
            var isControllerFound_SoftwareVersion = batchControlPage.RealTimeBatchPanel.GetListOfControllerColumnData("Name").Contains(controller);
            VerifyEqual("4. Verify there is a new tab name 'Controller Device'", "Controller Device", tabOfControllDevice_SoftwareVersion);
            VerifyEqual("4. Verify the tab contains the name of testing controller", true, isControllerFound_SoftwareVersion);

            Step("5. Update the value in the search box to invalid value and press Search icon");
            batchControlPage.RealTimeBatchPanel.EnterSearchNameInput(SLVHelper.GenerateString(5));
            batchControlPage.RealTimeBatchPanel.ClickSearchButton();
            batchControlPage.WaitForPreviousActionComplete();
            var numberOfTabs_SoftwareVersion = batchControlPage.RealTimeBatchPanel.GetListOfTabsName().Count;
            VerifyEqual("5. Verify The Controller Device tab disappeared", 0, numberOfTabs_SoftwareVersion);

            Step("6. Set a part of testing value into the searchbox and press Search icon again");
            batchControlPage.RealTimeBatchPanel.EnterSearchNameInput(softwareVersionPart);
            batchControlPage.RealTimeBatchPanel.ClickSearchButton();
            batchControlPage.WaitForPreviousActionComplete();
            var tabOfControllDevice_SoftwareVersion2 = batchControlPage.RealTimeBatchPanel.GetListOfTabsName().FirstOrDefault();
            var isControllerFound_SoftwareVersion2 = batchControlPage.RealTimeBatchPanel.GetListOfControllerColumnData("Name").Contains(controller);
            VerifyEqual("6. Verify there is a new tab name 'Controller Device'", "Controller Device", tabOfControllDevice_SoftwareVersion2);
            VerifyEqual("6. Verify the tab contains the name of testing controller", true, isControllerFound_SoftwareVersion2);

            Step("7. Select a testing attribute {0}", firstAttribute);
            batchControlPage.RealTimeBatchPanel.SelectAttributeDropDown(firstAttribute);
            var valueOfAttributeDropDown_FirstAttribute = batchControlPage.RealTimeBatchPanel.GetAttributeValue();
            var valueOfSearchNameInput_FirstAttribute = batchControlPage.RealTimeBatchPanel.GetSearchNameInputDefaultValue();
            VerifyEqual("7. Verify the testing attribute is selected", firstAttribute, valueOfAttributeDropDown_FirstAttribute);
            VerifyEqual("7. Verify the text in the 3rd box is updated to the selected attribute", firstAttribute, valueOfSearchNameInput_FirstAttribute);

            Step("8. Select the operator 'Contains'");
            batchControlPage.RealTimeBatchPanel.SelectOperatorDropDown("Contains");
            var valueOfOperatorDropDown_FirstAttribute = batchControlPage.RealTimeBatchPanel.GetOperatorValue();
            VerifyEqual("8. Verify The operator Equal is selected", "Contains", valueOfOperatorDropDown_FirstAttribute);

            Step("9. Input a part of value of that attribute into the search box and press Search icon");
            batchControlPage.RealTimeBatchPanel.EnterSearchNameInput(firstAttributePart);
            batchControlPage.RealTimeBatchPanel.ClickSearchButton();
            batchControlPage.WaitForPreviousActionComplete();
            var tabOfStreetlight_FirstAttribute = batchControlPage.RealTimeBatchPanel.GetListOfTabsName().FirstOrDefault();
            var isStreetlightFound_FirstAttribute = batchControlPage.RealTimeBatchPanel.GetListOfStreetlightColumnData("Name").Contains(streetlight1);
            VerifyEqual("9. Verify there is a new tab name 'StreetLight'", "StreetLight", tabOfStreetlight_FirstAttribute);
            VerifyEqual("9. Verify the tab contains the name of testing streetlight", true, isStreetlightFound_FirstAttribute);

            Step("10. Update the value in the search box to invalid value and press Search icon");
            batchControlPage.RealTimeBatchPanel.EnterSearchNameInput(SLVHelper.GenerateString(5));
            batchControlPage.RealTimeBatchPanel.ClickSearchButton();
            batchControlPage.WaitForPreviousActionComplete();
            var numberOfTabs_FirstAttribute = batchControlPage.RealTimeBatchPanel.GetListOfTabsName().Count;
            VerifyEqual("10. Verify The Streetlight tab disappeared", 0, numberOfTabs_FirstAttribute);

            Step("11. Set a part of testing value into the searchbox and press Search icon again");
            batchControlPage.RealTimeBatchPanel.EnterSearchNameInput(firstAttributePart);
            batchControlPage.RealTimeBatchPanel.ClickSearchButton();
            batchControlPage.WaitForPreviousActionComplete();
            var tabOfStreetlight_FirstAttribute2 = batchControlPage.RealTimeBatchPanel.GetListOfTabsName().FirstOrDefault();
            var isStreetlightFound_FirstAttribute2 = batchControlPage.RealTimeBatchPanel.GetListOfStreetlightColumnData("Name").Contains(streetlight1);
            VerifyEqual("11. Verify there is a new tab name 'StreetLight'", "StreetLight", tabOfStreetlight_FirstAttribute2);
            VerifyEqual("11. Verify the tab contains the name of testing streetlight", true, isStreetlightFound_FirstAttribute2);

            Step("12. Select a testing attribute {0}", secondAttribute);
            batchControlPage.RealTimeBatchPanel.SelectAttributeDropDown(secondAttribute);
            var valueOfAttributeDropDown_SecondAttribute = batchControlPage.RealTimeBatchPanel.GetAttributeValue();
            var valueOfSearchNameInput_SecondAttribute = batchControlPage.RealTimeBatchPanel.GetSearchNameInputDefaultValue();
            VerifyEqual("12. Verify the testing attribute is selected", secondAttribute, valueOfAttributeDropDown_SecondAttribute);
            VerifyEqual("12. Verify the text in the 3rd box is updated to the selected attribute", secondAttribute, valueOfSearchNameInput_SecondAttribute);

            Step("13. Select the operator 'Contains'");
            batchControlPage.RealTimeBatchPanel.SelectOperatorDropDown("Contains");
            var valueOfOperatorDropDown_SecondAttribute = batchControlPage.RealTimeBatchPanel.GetOperatorValue();
            VerifyEqual("13. Verify The operator Equal is selected", "Contains", valueOfOperatorDropDown_SecondAttribute);

            Step("14. Input a part of value of that attribute into the search box and press Search icon");
            batchControlPage.RealTimeBatchPanel.EnterSearchNameInput(secondAttributePart);
            batchControlPage.RealTimeBatchPanel.ClickSearchButton();
            batchControlPage.WaitForPreviousActionComplete();
            var tabOfStreetlight_SecondAttribute = batchControlPage.RealTimeBatchPanel.GetListOfTabsName().FirstOrDefault();
            var isStreetlightFound_SecondAttribute = batchControlPage.RealTimeBatchPanel.GetListOfStreetlightColumnData("Name").Contains(streetlight2);
            VerifyEqual("14. Verify there is a new tab name 'StreetLight'", "StreetLight", tabOfStreetlight_SecondAttribute);
            VerifyEqual("14. Verify the tab contains the name of testing streetlight", true, isStreetlightFound_SecondAttribute);

            Step("15. Update the value in the search box to invalid value and press Search icon");
            batchControlPage.RealTimeBatchPanel.EnterSearchNameInput(SLVHelper.GenerateString(5));
            batchControlPage.RealTimeBatchPanel.ClickSearchButton();
            batchControlPage.WaitForPreviousActionComplete();
            var numberOfTabs_SecondAttribute = batchControlPage.RealTimeBatchPanel.GetListOfTabsName().Count;
            VerifyEqual("15. Verify The Streetlight tab disappeared", 0, numberOfTabs_SecondAttribute);

            Step("16. Set a part of testing value into the searchbox and press Search icon again");
            batchControlPage.RealTimeBatchPanel.EnterSearchNameInput(secondAttributePart);
            batchControlPage.RealTimeBatchPanel.ClickSearchButton();
            batchControlPage.WaitForPreviousActionComplete();
            var tabOfStreetlight_SecondAttribute2 = batchControlPage.RealTimeBatchPanel.GetListOfTabsName().FirstOrDefault();
            var isStreetlightFound_SecondAttribute2 = batchControlPage.RealTimeBatchPanel.GetListOfStreetlightColumnData("Name").Contains(streetlight2);
            VerifyEqual("16. Verify there is a new tab name 'StreetLight'", "StreetLight", tabOfStreetlight_SecondAttribute2);
            VerifyEqual("16. Verify the tab contains the name of testing streetlight", true, isStreetlightFound_SecondAttribute2);

            Step("17. Select a testing attribute {0}", thirdAttribute);
            batchControlPage.RealTimeBatchPanel.SelectAttributeDropDown(thirdAttribute);
            var valueOfAttributeDropDown_ThirdAttribute = batchControlPage.RealTimeBatchPanel.GetAttributeValue();
            var valueOfSearchNameInput_ThirdAttribute = batchControlPage.RealTimeBatchPanel.GetSearchNameInputDefaultValue();
            VerifyEqual("17. Verify the testing attribute is selected", thirdAttribute, valueOfAttributeDropDown_ThirdAttribute);
            VerifyEqual("17. Verify the text in the 3rd box is updated to the selected attribute", thirdAttribute, valueOfSearchNameInput_ThirdAttribute);

            Step("18. Select the operator 'Contains'");
            batchControlPage.RealTimeBatchPanel.SelectOperatorDropDown("Contains");
            var valueOfOperatorDropDown_ThirdAttribute = batchControlPage.RealTimeBatchPanel.GetOperatorValue();
            VerifyEqual("18. Verify The operator Equal is selected", "Contains", valueOfOperatorDropDown_ThirdAttribute);

            Step("19. Input a part of value of that attribute into the search box and press Search icon");
            batchControlPage.RealTimeBatchPanel.EnterSearchNameInput(thirdAttributePart);
            batchControlPage.RealTimeBatchPanel.ClickSearchButton();
            batchControlPage.WaitForPreviousActionComplete();
            var tabOfStreetlight_ThirdAttribute = batchControlPage.RealTimeBatchPanel.GetListOfTabsName().FirstOrDefault();
            var isStreetlightFound_ThirdAttribute = batchControlPage.RealTimeBatchPanel.GetListOfStreetlightColumnData("Name").Contains(streetlight3);
            VerifyEqual("19. Verify there is a new tab name 'StreetLight'", "StreetLight", tabOfStreetlight_ThirdAttribute);
            VerifyEqual("19. Verify the tab contains the name of testing streetlight", true, isStreetlightFound_ThirdAttribute);

            Step("20. Update the value in the search box to invalid value and press Search icon");
            batchControlPage.RealTimeBatchPanel.EnterSearchNameInput(SLVHelper.GenerateString(5));
            batchControlPage.RealTimeBatchPanel.ClickSearchButton();
            batchControlPage.WaitForPreviousActionComplete();
            var numberOfTabs_ThirdAttribute = batchControlPage.RealTimeBatchPanel.GetListOfTabsName().Count;
            VerifyEqual("20. Verify The Streetlight tab disappeared", 0, numberOfTabs_ThirdAttribute);

            Step("21. Set a part of testing value into the searchbox and press Search icon again");
            batchControlPage.RealTimeBatchPanel.EnterSearchNameInput(thirdAttributePart);
            batchControlPage.RealTimeBatchPanel.ClickSearchButton();
            batchControlPage.WaitForPreviousActionComplete();
            var tabOfStreetlight_ThirdAttribute2 = batchControlPage.RealTimeBatchPanel.GetListOfTabsName().FirstOrDefault();
            var isStreetlightFound_ThirdAttribute2 = batchControlPage.RealTimeBatchPanel.GetListOfStreetlightColumnData("Name").Contains(streetlight3);
            VerifyEqual("21. Verify there is a new tab name 'StreetLight'", "StreetLight", tabOfStreetlight_ThirdAttribute2);
            VerifyEqual("21. Verify the tab contains the name of testing streetlight", true, isStreetlightFound_ThirdAttribute2);

            Step("22. Select a testing attribute {0}", uniqueAddress);
            batchControlPage.RealTimeBatchPanel.SelectAttributeDropDown(uniqueAddress);
            var valueOfAttributeDropDown_UniqueAddress = batchControlPage.RealTimeBatchPanel.GetAttributeValue();
            var valueOfSearchNameInput_UniqueAddress = batchControlPage.RealTimeBatchPanel.GetSearchNameInputDefaultValue();
            VerifyEqual("22. Verify the testing attribute is selected", uniqueAddress, valueOfAttributeDropDown_UniqueAddress);
            VerifyEqual("22. Verify the text in the 3rd box is updated to the selected attribute", uniqueAddress, valueOfSearchNameInput_UniqueAddress);

            Step("23. Select the operator 'Contains'");
            batchControlPage.RealTimeBatchPanel.SelectOperatorDropDown("Contains");
            var valueOfOperatorDropDown_UniqueAddress = batchControlPage.RealTimeBatchPanel.GetOperatorValue();
            VerifyEqual("23. Verify The operator Contains is selected", "Contains", valueOfOperatorDropDown_UniqueAddress);

            Step("24. Input a part of value of that attribute into the search box and press Search icon");
            batchControlPage.RealTimeBatchPanel.EnterSearchNameInput(randomUniqueAddressPart);
            batchControlPage.RealTimeBatchPanel.ClickSearchButton();
            batchControlPage.WaitForPreviousActionComplete();
            var tabOfStreetlight_UniqueAddress = batchControlPage.RealTimeBatchPanel.GetListOfTabsName().FirstOrDefault();
            var isStreetlightFound_UniqueAddress = batchControlPage.RealTimeBatchPanel.GetListOfStreetlightColumnData("Name").Contains(streetlight4);
            VerifyEqual("24. Verify there is a new tab name 'StreetLight'", "StreetLight", tabOfStreetlight_UniqueAddress);
            VerifyEqual("24. Verify the tab contains the name of testing streetlight", true, isStreetlightFound_UniqueAddress);

            Step("25. Update the value in the search box to invalid value and press Search icon");
            batchControlPage.RealTimeBatchPanel.EnterSearchNameInput(SLVHelper.GenerateString(5));
            batchControlPage.RealTimeBatchPanel.ClickSearchButton();
            batchControlPage.WaitForPreviousActionComplete();
            var numberOfTabs_UniqueAddress = batchControlPage.RealTimeBatchPanel.GetListOfTabsName().Count;
            VerifyEqual("25. Verify The Streetlight tab disappeared", 0, numberOfTabs_UniqueAddress);

            Step("26. Set a part of testing value into the searchbox and press Search icon again");
            batchControlPage.RealTimeBatchPanel.EnterSearchNameInput(randomUniqueAddressPart);
            batchControlPage.RealTimeBatchPanel.ClickSearchButton();
            batchControlPage.WaitForPreviousActionComplete();
            var tabOfStreetlight_UniqueAddress2 = batchControlPage.RealTimeBatchPanel.GetListOfTabsName().FirstOrDefault();
            var isStreetlightFound_UniqueAddress2 = batchControlPage.RealTimeBatchPanel.GetListOfStreetlightColumnData("Name").Contains(streetlight4);
            VerifyEqual("26. Verify there is a new tab name 'StreetLight'", "StreetLight", tabOfStreetlight_UniqueAddress2);
            VerifyEqual("26. Verify the tab contains the name of testing streetlight", true, isStreetlightFound_UniqueAddress2);

            Step("27. Select a testing attribute {0}", name);
            batchControlPage.RealTimeBatchPanel.SelectAttributeDropDown(name);
            var valueOfAttributeDropDown_Name = batchControlPage.RealTimeBatchPanel.GetAttributeValue();
            var valueOfSearchNameInput_Name = batchControlPage.RealTimeBatchPanel.GetSearchNameInputDefaultValue();
            VerifyEqual("27. Verify the testing attribute is selected", name, valueOfAttributeDropDown_Name);
            VerifyEqual("27. Verify the text in the 3rd box is updated to the selected attribute", name, valueOfSearchNameInput_Name);

            Step("28. Select the operator 'Contains'");
            batchControlPage.RealTimeBatchPanel.SelectOperatorDropDown("Contains");
            var valueOfOperatorDropDown_Name = batchControlPage.RealTimeBatchPanel.GetOperatorValue();
            VerifyEqual("28. Verify The operator Contains is selected", "Contains", valueOfOperatorDropDown_Name);

            Step("29. Input a part of value of that attribute into the search box and press Search icon");
            batchControlPage.RealTimeBatchPanel.EnterSearchNameInput(namePart);
            batchControlPage.RealTimeBatchPanel.ClickSearchButton();
            batchControlPage.WaitForPreviousActionComplete();
            var tabOfStreetlight_Name = batchControlPage.RealTimeBatchPanel.GetListOfTabsName().FirstOrDefault();
            var isStreetlightFound_Name = batchControlPage.RealTimeBatchPanel.GetListOfStreetlightColumnData("Name").Contains(name);
            VerifyEqual("29. Verify there is a new tab name 'StreetLight'", "StreetLight", tabOfStreetlight_Name);
            VerifyEqual("29. Verify the tab contains the name of testing streetlight", true, isStreetlightFound_Name);

            Step("30. Update the value in the search box to invalid value and press Search icon");
            batchControlPage.RealTimeBatchPanel.EnterSearchNameInput(SLVHelper.GenerateString(5));
            batchControlPage.RealTimeBatchPanel.ClickSearchButton();
            batchControlPage.WaitForPreviousActionComplete();
            var numberOfTabs_Name = batchControlPage.RealTimeBatchPanel.GetListOfTabsName().Count;
            VerifyEqual("30. Verify The Streetlight tab disappeared", 0, numberOfTabs_Name);

            Step("31. Set a part of testing value into the searchbox and press Search icon again");
            batchControlPage.RealTimeBatchPanel.EnterSearchNameInput(namePart);
            batchControlPage.RealTimeBatchPanel.ClickSearchButton();
            batchControlPage.WaitForPreviousActionComplete();
            var tabOfStreetlight_Name2 = batchControlPage.RealTimeBatchPanel.GetListOfTabsName().FirstOrDefault();
            var isStreetlightFound_Name2 = batchControlPage.RealTimeBatchPanel.GetListOfStreetlightColumnData("Name").Contains(name);
            VerifyEqual("31. Verify there is a new tab name 'StreetLight'", "StreetLight", tabOfStreetlight_Name2);
            VerifyEqual("31. Verify the tab contains the name of testing streetlight", true, isStreetlightFound_Name2);

            try
            {
                DeleteGeozone(geozone);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("BC_11 Custom Search via Lamp wattage (W) with operator Equal")]
        public void BC_11()
        {
            var geozone = SLVHelper.GenerateUniqueName("GZNBC11");
            var controller = SLVHelper.GenerateUniqueName("CTRL");
            var streetlight = SLVHelper.GenerateUniqueName("STL");
            var lampWattage = SLVHelper.GenerateStringInteger(5, 96);

            Step("**** Precondition ****");
            Step(" - User has logged in successful");
            Step(" - Batch Control is installed successfully");
            Step(" - Covered the following attributes: Lamp wattage (W)");
            Step(" - Go to Equipment Inventory > take note the Lamp Wattage (W) value for a streetlight at Inventory tab > Lamp section");
            Step("**** Precondition ****\n");

            Step("-> Create data for testing");
            DeleteGeozones("GZNBC11*");
            CreateNewGeozone(geozone);
            CreateNewController(controller, geozone);
            CreateNewDevice(DeviceType.Streetlight, streetlight, controller, geozone);

            var requestToSetLampWattage = SetValueToDevice(controller, streetlight, "power", lampWattage, Settings.GetServerTime());
            VerifyEqual(string.Format("-> Verify the requestToSetLampWattage is sent successfully (attribute: {0}, value: {1})", "Lamp wattage (W)", lampWattage), true, requestToSetLampWattage);

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.BatchControl);

            Step("1. Go to Batch Control app and select the Geozones > testing geozone");
            var batchControlPage = desktopPage.GoToApp(App.BatchControl) as BatchControlPage;

            Step("2. Select Lamp wattage (W) attribute");
            batchControlPage.RealTimeBatchPanel.SelectAttributeDropDown("Lamp wattage (W)");
            var firstBox = batchControlPage.RealTimeBatchPanel.GetAttributeValue();
            var thirdBox = batchControlPage.RealTimeBatchPanel.GetSearchNameInputDefaultValue();
            VerifyEqual("2. Verify the testing attribute is selected", "Lamp wattage (W)", firstBox);
            VerifyEqual("2. Verify the text in the 3rd box is updated to the selected attribute", "Lamp wattage (W)", thirdBox);

            Step("3. Press randomly a number of times on the Up Arrow icon in the 3rd box to set value for Lamp wattage");
            var numberUpArrow = SLVHelper.GenerateInteger(1, 101);
            batchControlPage.RealTimeBatchPanel.ClickLampWattageUpArrow(numberUpArrow);
            var valueUpArrow = batchControlPage.RealTimeBatchPanel.GetSearchNameValue();
            VerifyEqual("3. Verify The 3rd box is set the number of pressing", numberUpArrow, Convert.ToInt32(valueUpArrow));

            Step("4. Press randomly a number of times on the Down Arrow icon in the 3rd box to set value for Lamp wattage");
            var numberDownArrow = SLVHelper.GenerateInteger(1, numberUpArrow + 1);
            batchControlPage.RealTimeBatchPanel.ClickLampWattageDownArrow(numberDownArrow);
            var valueDownArrow = batchControlPage.RealTimeBatchPanel.GetSearchNameValue();
            var different = Convert.ToString(numberUpArrow - numberDownArrow);
            VerifyEqual("4. Verify The 3rd box is set the number = (current number - number of pressing). The minimum value is 0", different, valueDownArrow);

            Step("5. Select the operator 'Equal'");
            batchControlPage.RealTimeBatchPanel.SelectOperatorDropDown("Equal");
            var operatorValue = batchControlPage.RealTimeBatchPanel.GetOperatorValue();
            VerifyEqual("5. Verify The operator Equal is selected", "Equal", operatorValue);

            Step("6. Input the value of Lamp wattage of the testing streetlight into the search box and press Search icon");
            batchControlPage.RealTimeBatchPanel.EnterSearchNameInput(lampWattage);
            batchControlPage.RealTimeBatchPanel.ClickSearchButton();
            batchControlPage.WaitForPreviousActionComplete();
            var tabOfStreetlightValid = batchControlPage.RealTimeBatchPanel.GetListOfTabsName().FirstOrDefault();
            var isStreetlightFoundValid = batchControlPage.RealTimeBatchPanel.GetListOfStreetlightColumnData("Name").Contains(streetlight);
            VerifyEqual("6. Verify there is a new tab name 'StreetLight'", "StreetLight", tabOfStreetlightValid);
            VerifyEqual("6. Verify the tab contains the name of testing streetlight", true, isStreetlightFoundValid);

            Step("7. Update the value in the search box to the value different from testing value and press Search icon");
            var invalidLampWattage = Convert.ToString(Convert.ToInt32(lampWattage) - 5);
            batchControlPage.RealTimeBatchPanel.EnterSearchNameInput(invalidLampWattage);
            batchControlPage.RealTimeBatchPanel.ClickSearchButton();
            batchControlPage.WaitForPreviousActionComplete();
            var tabOfStreetlightInvalid = batchControlPage.RealTimeBatchPanel.GetListOfTabsName().FirstOrDefault();
            var isStreetlightFoundInvalid = batchControlPage.RealTimeBatchPanel.GetListOfStreetlightColumnData("Name").Contains(streetlight);
            VerifyEqual("7. Verify The tab Streetlight disappeared", false, isStreetlightFoundInvalid);

            Step("8. Set the testing value into the searchbox and press Search icon again");
            batchControlPage.RealTimeBatchPanel.EnterSearchNameInput(lampWattage);
            batchControlPage.RealTimeBatchPanel.ClickSearchButton();
            batchControlPage.WaitForPreviousActionComplete();
            var tabOfStreetlightValid2 = batchControlPage.RealTimeBatchPanel.GetListOfTabsName().FirstOrDefault();
            var isStreetlightFoundValid2 = batchControlPage.RealTimeBatchPanel.GetListOfStreetlightColumnData("Name").Contains(streetlight);
            VerifyEqual("6. Verify there is a new tab name 'StreetLight'", "StreetLight", tabOfStreetlightValid2);
            VerifyEqual("6. Verify the tab contains the name of testing streetlight", true, isStreetlightFoundValid2);

            try
            {
                DeleteGeozone(geozone);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("BC_12 Custom Search via Lamp wattage (W) with operator Not Equal")]
        public void BC_12()
        {
            var geozone = SLVHelper.GenerateUniqueName("GZNBC12");
            var controller = SLVHelper.GenerateUniqueName("CTRL");
            var streetlight = SLVHelper.GenerateUniqueName("STL");
            var lampWattage = SLVHelper.GenerateStringInteger(5, 96);

            Step("**** Precondition ****");
            Step(" - User has logged in successful");
            Step(" - Batch Control is installed successfully");
            Step(" - Covered the following attributes: Lamp wattage (W)");
            Step(" - Go to Equipment Inventory > take note the Lamp Wattage (W) value for a streetlight at Inventory tab > Lamp section");
            Step("**** Precondition ****\n");

            Step("-> Create data for testing");
            DeleteGeozones("GZNBC12*");
            CreateNewGeozone(geozone);
            CreateNewController(controller, geozone);
            CreateNewDevice(DeviceType.Streetlight, streetlight, controller, geozone);

            var requestToSetLampWattage = SetValueToDevice(controller, streetlight, "power", lampWattage, Settings.GetServerTime());
            VerifyEqual(string.Format("-> Verify the requestToSetLampWattage is sent successfully (attribute: {0}, value: {1})", "Lamp wattage (W)", lampWattage), true, requestToSetLampWattage);

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.BatchControl);

            Step("1. Go to Batch Control app and select the Geozones > testing geozone");
            var batchControlPage = desktopPage.GoToApp(App.BatchControl) as BatchControlPage;
            batchControlPage.GeozoneTreeMainPanel.SelectNode(geozone);

            Step("2. Select Lamp wattage (W) attribute");
            batchControlPage.RealTimeBatchPanel.SelectAttributeDropDown("Lamp wattage (W)");
            var firstBox = batchControlPage.RealTimeBatchPanel.GetAttributeValue();
            var thirdBox = batchControlPage.RealTimeBatchPanel.GetSearchNameInputDefaultValue();
            VerifyEqual("2. Verify the testing attribute is selected", "Lamp wattage (W)", firstBox);
            VerifyEqual("2. Verify the text in the 3rd box is updated to the selected attribute", "Lamp wattage (W)", thirdBox);

            Step("3. Select the operator 'Not equal'");
            batchControlPage.RealTimeBatchPanel.SelectOperatorDropDown("Not equal");
            var IsNotEqualSelected = batchControlPage.RealTimeBatchPanel.GetOperatorValue().Equals("Not equal");
            VerifyEqual("3. Verify the operator Not equal is selected", true, IsNotEqualSelected);

            Step("4. Input the value of Lamp wattage of the testing streetlight into the search box and press Search icon");
            batchControlPage.RealTimeBatchPanel.EnterSearchNameInput(lampWattage);
            batchControlPage.RealTimeBatchPanel.ClickSearchButton();
            batchControlPage.WaitForPreviousActionComplete();
            var numberOfTabs = batchControlPage.RealTimeBatchPanel.GetListOfTabsName().Count;
            VerifyEqual("4. Verify the tab StreetLight is not displayed", 0, numberOfTabs);

            Step("5. Press Up Arrow button to update the value in the search box to the value different from testing value and press Search icon");
            batchControlPage.RealTimeBatchPanel.ClickLampWattageUpArrow(SLVHelper.GenerateInteger(1, 6));
            batchControlPage.RealTimeBatchPanel.ClickSearchButton();
            batchControlPage.WaitForPreviousActionComplete();
            var tabContainStreetlight = batchControlPage.RealTimeBatchPanel.GetListOfStreetlightColumnData("Name").Contains(streetlight);
            VerifyEqual("5. Verify the 'StreetLight' tab contains the name of testing streetlight", true, tabContainStreetlight);

            try
            {
                DeleteGeozone(geozone);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("BC_13 Custom Search via Lamp wattage (W) with operator Greater Than")]
        public void BC_13()
        {
            var geozone = SLVHelper.GenerateUniqueName("GZNBC13");
            var controller = SLVHelper.GenerateUniqueName("CTRL");
            var streetlight = SLVHelper.GenerateUniqueName("STL");
            var lampWattage = SLVHelper.GenerateStringInteger(5, 96);

            Step("**** Precondition ****");
            Step(" - User has logged in successful");
            Step(" - Batch Control is installed successfully");
            Step(" - Covered the following attributes: Lamp wattage (W)");
            Step(" - Go to Equipment Inventory > take note the Lamp Wattage (W) value for a streetlight at Inventory tab > Lamp section");
            Step("**** Precondition ****\n");

            Step("-> Create data for testing");
            DeleteGeozones("GZNBC13*");
            CreateNewGeozone(geozone);
            CreateNewController(controller, geozone);
            CreateNewDevice(DeviceType.Streetlight, streetlight, controller, geozone);

            var requestToSetLampWattage = SetValueToDevice(controller, streetlight, "power", lampWattage, Settings.GetServerTime());
            VerifyEqual(string.Format("-> Verify the requestToSetLampWattage is sent successfully (attribute: {0}, value: {1})", "Lamp wattage (W)", lampWattage), true, requestToSetLampWattage);

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.BatchControl);

            Step("1. Go to Batch Control app and select the Geozones > testing geozone");
            var batchControlPage = desktopPage.GoToApp(App.BatchControl) as BatchControlPage;
            batchControlPage.GeozoneTreeMainPanel.SelectNode(geozone);

            Step("2. Select Lamp wattage (W) attribute");
            batchControlPage.RealTimeBatchPanel.SelectAttributeDropDown("Lamp wattage (W)");
            var firstBox = batchControlPage.RealTimeBatchPanel.GetAttributeValue();
            var thirdBox = batchControlPage.RealTimeBatchPanel.GetSearchNameInputDefaultValue();
            VerifyEqual("2. Verify the testing attribute is selected", "Lamp wattage (W)", firstBox);
            VerifyEqual("2. Verify the text in the 3rd box is updated to the selected attribute", "Lamp wattage (W)", thirdBox);

            Step("3. Select the operator 'Greater than'");
            batchControlPage.RealTimeBatchPanel.SelectOperatorDropDown("Greater than");
            var IsGreaterThanSelected = batchControlPage.RealTimeBatchPanel.GetOperatorValue().Equals("Greater than");
            VerifyEqual("3. Verify the operator 'Greater than' is selected", true, IsGreaterThanSelected);

            Step("4. Input the value of Lamp wattage of the testing streetlight into the search box and press Search icon");
            batchControlPage.RealTimeBatchPanel.EnterSearchNameInput(lampWattage);
            batchControlPage.RealTimeBatchPanel.ClickSearchButton();
            batchControlPage.WaitForPreviousActionComplete();
            var numberOfTabs = batchControlPage.RealTimeBatchPanel.GetListOfTabsName().Count;
            VerifyEqual("4. Verify the tab StreetLight is not displayed", 0, numberOfTabs);

            Step("5. Press Down Arrow button once to descrease the search value and press Search icon");
            batchControlPage.RealTimeBatchPanel.ClickLampWattageDownArrow(SLVHelper.GenerateInteger(1, 6));
            batchControlPage.RealTimeBatchPanel.ClickSearchButton();
            batchControlPage.WaitForPreviousActionComplete();
            var tabContainStreetlight = batchControlPage.RealTimeBatchPanel.GetListOfStreetlightColumnData("Name").Contains(streetlight);
            if (tabContainStreetlight)
                VerifyEqual("5. Verify the 'StreetLight' tab contains the name of testing streetlight", true, tabContainStreetlight);
            else
                Warning("[SC-1340] Batch Control - Search using greater than operator does not work");

            try
            {
                DeleteGeozone(geozone);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("BC_14 Custom Search via Lamp wattage (W) with operator Greater Or Equal")]
        public void BC_14()
        {
            var geozone = SLVHelper.GenerateUniqueName("GZNBC14");
            var controller = SLVHelper.GenerateUniqueName("CTRL");
            var streetlight = SLVHelper.GenerateUniqueName("STL");
            var lampWattage = SLVHelper.GenerateStringInteger(5, 96);

            Step("**** Precondition ****");
            Step(" - User has logged in successful");
            Step(" - Batch Control is installed successfully");
            Step(" - Covered the following attributes: Lamp wattage (W)");
            Step(" - Go to Equipment Inventory > take note the Lamp wattage (W) value for a streetlight at Inventory tab > Lamp section");
            Step("**** Precondition ****\n");

            Step("-> Create data for testing");
            DeleteGeozones("GZNBC14*");
            CreateNewGeozone(geozone);
            CreateNewController(controller, geozone);
            CreateNewDevice(DeviceType.Streetlight, streetlight, controller, geozone);

            var requestToSetLampWattage = SetValueToDevice(controller, streetlight, "power", lampWattage, Settings.GetServerTime());
            VerifyEqual(string.Format("-> Verify the requestToSetLampWattage is sent successfully (attribute: {0}, value: {1})", "Lamp wattage (W)", lampWattage), true, requestToSetLampWattage);

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.BatchControl);

            Step("1. Go to Batch Control app and select the Geozones > testing geozone");
            var batchControlPage = desktopPage.GoToApp(App.BatchControl) as BatchControlPage;
            batchControlPage.GeozoneTreeMainPanel.SelectNode(geozone);

            Step("2. Select Lamp wattage (W) attribute");
            batchControlPage.RealTimeBatchPanel.SelectAttributeDropDown("Lamp wattage (W)");
            var firstBox = batchControlPage.RealTimeBatchPanel.GetAttributeValue();
            var thirdBox = batchControlPage.RealTimeBatchPanel.GetSearchNameInputDefaultValue();
            VerifyEqual("2. Verify the testing attribute is selected", "Lamp wattage (W)", firstBox);
            VerifyEqual("2. Verify the text in the 3rd box is updated to the selected attribute", "Lamp wattage (W)", thirdBox);

            Step("3. Select the operator 'Greater or equal'");
            batchControlPage.RealTimeBatchPanel.SelectOperatorDropDown("Greater or equal");
            var IsGreaterOrEqualSelected = batchControlPage.RealTimeBatchPanel.GetOperatorValue().Equals("Greater or equal");
            VerifyEqual("3. Verify the operator 'Greater or equal' is selected", true, IsGreaterOrEqualSelected);

            Step("4. Input the value of Lamp wattage of the testing streetlight into the search box and press Search icon");
            batchControlPage.RealTimeBatchPanel.EnterSearchNameInput(lampWattage);
            batchControlPage.RealTimeBatchPanel.ClickSearchButton();
            batchControlPage.WaitForPreviousActionComplete();
            var tabContainStreetlight1 = batchControlPage.RealTimeBatchPanel.GetListOfStreetlightColumnData("Name").Contains(streetlight);
            if (tabContainStreetlight1)
                VerifyEqual("4. There is a new tab name 'Streetlight' and it contains the name of testing streetlight", true, tabContainStreetlight1);
            else
                Warning("[SC-1340] Batch Control - Search using greater or equal operator does not work");

            Step("5. Press Up Arrow button once to increase the search value and press Search icon");
            batchControlPage.RealTimeBatchPanel.ClickLampWattageUpArrow(1);
            batchControlPage.RealTimeBatchPanel.ClickSearchButton();
            batchControlPage.WaitForPreviousActionComplete();
            var tabContainStreetlight2 = batchControlPage.RealTimeBatchPanel.GetListOfStreetlightColumnData("Name").Contains(streetlight);
            if (tabContainStreetlight2)
                VerifyEqual("5. There is a new tab name 'Streetlight' and it contains the name of testing streetlight", true, tabContainStreetlight2);
            else
                Warning("[SC-1340] Batch Control - Search using greater or equal operator does not work");

            Step("6. Input the value less than the value of Lamp wattage of the testing streetlight into the search box and press Search icon");
            var lampWattageMinusTwo = (Convert.ToInt32(lampWattage) - 2).ToString();
            batchControlPage.RealTimeBatchPanel.EnterSearchNameInput(lampWattageMinusTwo);
            batchControlPage.RealTimeBatchPanel.ClickSearchButton();
            batchControlPage.WaitForPreviousActionComplete();
            var numberOfTabs = batchControlPage.RealTimeBatchPanel.GetListOfTabsName().Count;
            VerifyEqual("6. Verify the tab StreetLight is not displayed", 0, numberOfTabs);

            try
            {
                DeleteGeozone(geozone);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("BC_15 Custom Search via Lamp wattage (W) with operator Less Than")]
        public void BC_15()
        {
            var geozone = SLVHelper.GenerateUniqueName("GZNBC15");
            var controller = SLVHelper.GenerateUniqueName("CTRL");
            var streetlight = SLVHelper.GenerateUniqueName("STL");
            var lampWattage = SLVHelper.GenerateStringInteger(5, 96);

            Step("**** Precondition ****");
            Step(" - User has logged in successful");
            Step(" - Batch Control is installed successfully");
            Step(" - Covered the following attributes: Lamp wattage (W)");
            Step(" - Go to Equipment Inventory > take note the Lamp wattage (W) value for a streetlight at Inventory tab > Lamp section");
            Step("**** Precondition ****\n");

            Step("-> Create data for testing");
            DeleteGeozones("GZNBC15*");
            CreateNewGeozone(geozone);
            CreateNewController(controller, geozone);
            CreateNewDevice(DeviceType.Streetlight, streetlight, controller, geozone);

            var requestToSetLampWattage = SetValueToDevice(controller, streetlight, "power", lampWattage, Settings.GetServerTime());
            VerifyEqual(string.Format("-> Verify the requestToSetLampWattage is sent successfully (attribute: {0}, value: {1})", "Lamp wattage (W)", lampWattage), true, requestToSetLampWattage);

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.BatchControl);

            Step("1. Go to Batch Control app and select the Geozones > testing geozone");
            var batchControlPage = desktopPage.GoToApp(App.BatchControl) as BatchControlPage;
            batchControlPage.GeozoneTreeMainPanel.SelectNode(geozone);

            Step("2. Select Lamp wattage (W) attribute");
            batchControlPage.RealTimeBatchPanel.SelectAttributeDropDown("Lamp wattage (W)");
            var firstBox = batchControlPage.RealTimeBatchPanel.GetAttributeValue();
            var thirdBox = batchControlPage.RealTimeBatchPanel.GetSearchNameInputDefaultValue();
            VerifyEqual("2. Verify the testing attribute is selected", "Lamp wattage (W)", firstBox);
            VerifyEqual("2. Verify the text in the 3rd box is updated to the selected attribute", "Lamp wattage (W)", thirdBox);

            Step("3. Select the operator 'Less than'");
            batchControlPage.RealTimeBatchPanel.SelectOperatorDropDown("Less than");
            var IsLessThanSelected = batchControlPage.RealTimeBatchPanel.GetOperatorValue().Equals("Less than");
            VerifyEqual("3. Verify the operator 'Less than' is selected", true, IsLessThanSelected);

            Step("4. Input the value of Lamp wattage of the testing streetlight into the search box and press Search icon");
            batchControlPage.RealTimeBatchPanel.EnterSearchNameInput(lampWattage);
            batchControlPage.RealTimeBatchPanel.ClickSearchButton();
            batchControlPage.WaitForPreviousActionComplete();
            var numberOfTabs = batchControlPage.RealTimeBatchPanel.GetListOfTabsName().Count;
            VerifyEqual("4. Verify the tab StreetLight is not displayed", 0, numberOfTabs);

            Step("5. Press Up Arrow button once to inscrease the search value and press Search icon");
            batchControlPage.RealTimeBatchPanel.ClickLampWattageUpArrow(SLVHelper.GenerateInteger(1, 6));
            batchControlPage.RealTimeBatchPanel.ClickSearchButton();
            batchControlPage.WaitForPreviousActionComplete();
            var tabContainStreetlight = batchControlPage.RealTimeBatchPanel.GetListOfStreetlightColumnData("Name").Contains(streetlight);
            if (tabContainStreetlight)
                VerifyEqual("5. Verify the 'StreetLight' tab contains the name of testing streetlight", true, tabContainStreetlight);
            else
                Warning("[SC-1340] Batch Control - Search using less than operator does not work");

            try
            {
                DeleteGeozone(geozone);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("BC_16 Custom Search via Lamp wattage (W) with operator Less Or Equal")]
        public void BC_16()
        {
            var geozone = SLVHelper.GenerateUniqueName("GZNBC16");
            var controller = SLVHelper.GenerateUniqueName("CTRL");
            var streetlight = SLVHelper.GenerateUniqueName("STL");
            var lampWattage = SLVHelper.GenerateStringInteger(5, 96);

            Step("**** Precondition ****");
            Step(" - User has logged in successful");
            Step(" - Batch Control is installed successfully");
            Step(" - Covered the following attributes: Lamp wattage (W)");
            Step(" - Go to Equipment Inventory > take note the Lamp wattage (W) value for a streetlight at Inventory tab > Lamp section");
            Step("**** Precondition ****\n");

            Step("-> Create data for testing");
            DeleteGeozones("GZNBC16*");
            CreateNewGeozone(geozone);
            CreateNewController(controller, geozone);
            CreateNewDevice(DeviceType.Streetlight, streetlight, controller, geozone);

            var requestToSetLampWattage = SetValueToDevice(controller, streetlight, "power", lampWattage, Settings.GetServerTime());
            VerifyEqual(string.Format("-> Verify the requestToSetLampWattage is sent successfully (attribute: {0}, value: {1})", "Lamp wattage (W)", lampWattage), true, requestToSetLampWattage);

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.BatchControl);

            Step("1. Go to Batch Control app and select the Geozones > testing geozone");
            var batchControlPage = desktopPage.GoToApp(App.BatchControl) as BatchControlPage;
            batchControlPage.GeozoneTreeMainPanel.SelectNode(geozone);

            Step("2. Select Lamp wattage (W) attribute");
            batchControlPage.RealTimeBatchPanel.SelectAttributeDropDown("Lamp wattage (W)");
            var firstBox = batchControlPage.RealTimeBatchPanel.GetAttributeValue();
            var thirdBox = batchControlPage.RealTimeBatchPanel.GetSearchNameInputDefaultValue();
            VerifyEqual("2. Verify the testing attribute is selected", "Lamp wattage (W)", firstBox);
            VerifyEqual("2. Verify the text in the 3rd box is updated to the selected attribute", "Lamp wattage (W)", thirdBox);

            Step("3. Select the operator 'Less or equal'");
            batchControlPage.RealTimeBatchPanel.SelectOperatorDropDown("Less or equal");
            var IsLessOrEqualSelected = batchControlPage.RealTimeBatchPanel.GetOperatorValue().Equals("Less or equal");
            VerifyEqual("3. Verify the operator 'Less or equal' is selected", true, IsLessOrEqualSelected);

            Step("4. Input the value of Lamp wattage of the testing streetlight into the search box and press Search icon");
            batchControlPage.RealTimeBatchPanel.EnterSearchNameInput(lampWattage);
            batchControlPage.RealTimeBatchPanel.ClickSearchButton();
            batchControlPage.WaitForPreviousActionComplete();
            var tabContainStreetlight1 = batchControlPage.RealTimeBatchPanel.GetListOfStreetlightColumnData("Name").Contains(streetlight);
            if (tabContainStreetlight1)
                VerifyEqual("4. Verify the 'StreetLight' tab contains the name of testing streetlight", true, tabContainStreetlight1);
            else
                Warning("[SC-1340] Batch Control - Search using less or equal operator does not work");

            Step("5. Press Down Arrow button once to descrease the search value and press Search icon");
            batchControlPage.RealTimeBatchPanel.ClickLampWattageDownArrow(1);
            batchControlPage.RealTimeBatchPanel.ClickSearchButton();
            batchControlPage.WaitForPreviousActionComplete();
            var tabContainStreetlight2 = batchControlPage.RealTimeBatchPanel.GetListOfStreetlightColumnData("Name").Contains(streetlight);
            if (tabContainStreetlight1)
                VerifyEqual("5. Verify the 'StreetLight' tab contains the name of testing streetlight", true, tabContainStreetlight2);
            else
                Warning("[SC-1340] Batch Control - Search using less or equal operator does not work");

            Step("6. Input the value greater than the value of Lamp wattage of the testing streetlight into the search box and press Search icon");
            var lampWattagePlusTwo = (Convert.ToInt32(lampWattage) + 2).ToString();
            batchControlPage.RealTimeBatchPanel.EnterSearchNameInput(lampWattagePlusTwo);
            batchControlPage.RealTimeBatchPanel.ClickSearchButton();
            batchControlPage.WaitForPreviousActionComplete();
            var numberOfTabs = batchControlPage.RealTimeBatchPanel.GetListOfTabsName().Count;
            VerifyEqual("6. Verify the tab StreetLight is not displayed", 0, numberOfTabs);

            try
            {
                DeleteGeozone(geozone);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("BC_17 Custom Search via Category ")]
        public void BC_17()
        {
            var geozone = SLVHelper.GenerateUniqueName("GZNBC17");
            var controller = SLVHelper.GenerateUniqueName("CTRL");
            var streetlightName = SLVHelper.GenerateUniqueName("STL");
            var switchDevice = SLVHelper.GenerateUniqueName("SWH");
            var meterName = SLVHelper.GenerateUniqueName("MTR");

            Step("**** Precondition ****");
            Step(" - User has logged in successful");
            Step(" - Batch Control is installed successfully");
            Step(" - Create a new geozone and add 4 types of deviecs: 1 Streetlight, 1 Controller, 1 Electrical Meter, 1 Switch");
            Step("**** Precondition ****\n");

            Step("-> Create data for testing");
            DeleteGeozones("GZNBC17*");
            CreateNewGeozone(geozone);
            CreateNewController(controller, geozone);
            CreateNewDevice(DeviceType.Streetlight, streetlightName, controller, geozone);
            CreateNewDevice(DeviceType.Switch, switchDevice, controller, geozone);
            CreateNewDevice(DeviceType.ElectricalCounter, meterName, controller, geozone);

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.BatchControl);

            Step("1. Go to Batch Control app and select the Geozones > {0}", geozone);
            var batchControlPage = desktopPage.GoToApp(App.BatchControl) as BatchControlPage;
            batchControlPage.GeozoneTreeMainPanel.SelectNode(geozone);

            Step("2. Select Category for Custome Search");
            batchControlPage.RealTimeBatchPanel.SelectAttributeDropDown("Category");
            var attributeDropDownValue = batchControlPage.RealTimeBatchPanel.GetAttributeValue();
            var searchNameInputText = batchControlPage.RealTimeBatchPanel.GetDropDownInputFieldText();
            VerifyEqual("2. Verify The testing attribute is selected", "Category", attributeDropDownValue);
            VerifyEqual("2. Verify The text in the 3rd box is updated to the selected attribute", "Category", searchNameInputText);

            Step("3. Select the operator 'Equal'");
            batchControlPage.RealTimeBatchPanel.SelectOperatorDropDown("Equal");
            var numberOfOperators = batchControlPage.RealTimeBatchPanel.GetNumberOfOperators();
            var operatorDropDownValue = batchControlPage.RealTimeBatchPanel.GetOperatorValue();
            VerifyEqual("3. Verify There is only one operator 'Equal' in the combobox", 1, numberOfOperators);
            VerifyEqual("3. Verify The 'Equal' operator is selected", "Equal", operatorDropDownValue);
            batchControlPage.WaitForPreviousActionComplete();

            Step("4. Select the 3rd combobox");
            var actualTypes = batchControlPage.RealTimeBatchPanel.GetListOfAllListedItemsFromDropDownMenu();
            List<string> expectedTypes = new List<string> { "StreetLight", "Switch Device", "Controller Device", "Electrical Counter", "Cabinet Controller" };
            VerifyEqual("4. Verify There is 5 types of devices listed: StreetLight, Switch Device, Controller Device, Electrical Counter, Cabinet Controller", expectedTypes, actualTypes);

            Step("5. Select 'StreetLight' and press Search icon");
            batchControlPage.RealTimeBatchPanel.SelectValueDropDownMenu("StreetLight");
            batchControlPage.RealTimeBatchPanel.ClickSearchButton();
            batchControlPage.WaitForPreviousActionComplete();
            var streetlightTab = batchControlPage.RealTimeBatchPanel.GetListOfTabsName().FirstOrDefault();
            var nameColumnOfStreetlight = batchControlPage.RealTimeBatchPanel.GetListOfStreetlightColumnData("Name");
            var streetlightIsFound = nameColumnOfStreetlight.Contains(streetlightName);
            VerifyEqual("5. Verify There is a new tab name 'StreetLight'", "StreetLight", streetlightTab);
            VerifyEqual("5. Verify The new tab contains the name of testing streetlight", true, streetlightIsFound);
            batchControlPage.WaitForPreviousActionComplete();

            Step("6. Select 'Switch Device' and press Search icon");
            batchControlPage.RealTimeBatchPanel.SelectValueDropDownMenu("Switch Device");
            batchControlPage.RealTimeBatchPanel.ClickSearchButton();
            batchControlPage.WaitForPreviousActionComplete();
            var switchDeviceTab = batchControlPage.RealTimeBatchPanel.GetListOfTabsName().FirstOrDefault();
            var nameColumnOfSwitchDevice = batchControlPage.RealTimeBatchPanel.GetListOfSwitchColumnData("Name");
            var switchDevicesIsFound = nameColumnOfSwitchDevice.Contains(switchDevice);
            VerifyEqual("6. Verify There is a new tab name 'Switch Device'", "Switch Device", switchDeviceTab);
            VerifyEqual("6. Verify The new tab contains the name of testing streetlight", true, switchDevicesIsFound);
            batchControlPage.WaitForPreviousActionComplete();

            Step("7. Select 'Controller Device' and press Search icon");
            batchControlPage.RealTimeBatchPanel.SelectValueDropDownMenu("Controller Device");
            batchControlPage.RealTimeBatchPanel.ClickSearchButton();
            batchControlPage.WaitForPreviousActionComplete();
            var controllerDeviceTab = batchControlPage.RealTimeBatchPanel.GetListOfTabsName().FirstOrDefault();
            var nameColumnOfControllerDevice = batchControlPage.RealTimeBatchPanel.GetListOfControllerColumnData("Name");
            var controllerDevicesIsFound = nameColumnOfControllerDevice.Contains(controller);
            VerifyEqual("7. Verify There is a new tab name 'Controller Device'", "Controller Device", controllerDeviceTab);
            VerifyEqual("7. Verify The new tab contains the name of testing streetlight", true, controllerDevicesIsFound);
            batchControlPage.WaitForPreviousActionComplete();

            Step("8. Select 'Electrical Counter' and press Search icon");
            batchControlPage.RealTimeBatchPanel.SelectValueDropDownMenu("Electrical Counter");
            batchControlPage.RealTimeBatchPanel.ClickSearchButton();
            batchControlPage.WaitForPreviousActionComplete();
            var electricalCounterTab = batchControlPage.RealTimeBatchPanel.GetListOfTabsName().FirstOrDefault();
            var nameColumnOfElectricalCounter = batchControlPage.RealTimeBatchPanel.GetListOfElectricalCounterColumnData("Name");
            var electricalCounterIsFound = nameColumnOfElectricalCounter.Contains(meterName);
            VerifyEqual("8. Verify There is a new tab name 'Electrical Counter'", "Electrical Counter", electricalCounterTab);
            VerifyEqual("8. Verify The new tab contains the name of testing streetlight", true, electricalCounterIsFound);
            batchControlPage.WaitForPreviousActionComplete();

            try
            {
                DeleteGeozone(geozone);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("BC_18 Custom Search for streetlight via Dimming Group ")]
        public void BC_18()
        {
            var geozone = SLVHelper.GenerateUniqueName("GZNBC18");
            var controller = SLVHelper.GenerateUniqueName("CTRL");
            var streetlight = SLVHelper.GenerateUniqueName("STL");
            var dimmingGroup = SLVHelper.GenerateString();

            Step("-> Create data for testing");
            DeleteGeozones("GZNBC18*");
            CreateNewGeozone(geozone);
            CreateNewController(controller, geozone);
            CreateNewDevice(DeviceType.Streetlight, streetlight, controller, geozone);

            var requestToSetDimmingGroup = SetValueToDevice(controller, streetlight, "DimmingGroupName", dimmingGroup, Settings.GetServerTime());
            VerifyEqual(string.Format("-> Verify the requestToSetDimmingGroup is sent successfully (attribute: {0}, value: {1})", "Dimming group", dimmingGroup), true, requestToSetDimmingGroup);

            Step("**** Precondition ****");
            Step(" - User has logged in successful");
            Step(" -Batch Control is installed successfully");
            Step(" -Take note a dimming group name of a streetlight in Real Time Control Area");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.BatchControl);

            Step("1. Go to Batch Control app and select the Geozones > Real Time Control Area");
            var batchControlPage = desktopPage.GoToApp(App.BatchControl) as BatchControlPage;
            batchControlPage.GeozoneTreeMainPanel.SelectNode(geozone);

            Step("2. Select 'Dimming group' for the 1st combobox of Custome Search");
            batchControlPage.RealTimeBatchPanel.SelectAttributeDropDown("Dimming group");

            Step("3. Verify The testing attribute is selected and the text in the 3rd box is updated to the selected attribute");
            var attributeDropDownValue = batchControlPage.RealTimeBatchPanel.GetAttributeValue();
            var searchNameInputValue = batchControlPage.RealTimeBatchPanel.GetDropDownInputFieldText();
            VerifyEqual("3. Verify The testing attribute is selected", "Dimming group", attributeDropDownValue);
            VerifyEqual("3. Verify The value of third box", "Dimming group", searchNameInputValue);

            Step("4. Select the operator 'Equal'");
            batchControlPage.RealTimeBatchPanel.SelectOperatorDropDown("Equal");

            Step("4. Verify There is only one operator 'Equal' in the combobox and is selected");
            var operatorDropDownValue = batchControlPage.RealTimeBatchPanel.GetOperatorValue();
            var numberOfOperators = batchControlPage.RealTimeBatchPanel.GetNumberOfOperators();
            VerifyEqual("4. Verify The Equal operator is the only one", 1, numberOfOperators);
            VerifyEqual("4. Verify The Equal operator is selected", "Equal", operatorDropDownValue);

            Step("5. Select the 3rd combobox and enter the invalid name of dimming group");
            batchControlPage.RealTimeBatchPanel.EnterSearchNameInput(SLVHelper.GenerateString(8));
            var noMatchesFoundDisplayed = batchControlPage.RealTimeBatchPanel.NoMatchesFoundIsDisplayedInOperatorDropDown();
            VerifyEqual("5. Verify The text 'No matches found' displays on the 3rd box", true, noMatchesFoundDisplayed);

            Step("6. Enter the name of testing dimming group and press Enter");
            batchControlPage.RealTimeBatchPanel.EnterSearchNameInput(dimmingGroup);
            if (batchControlPage.RealTimeBatchPanel.NoMatchesFoundIsDisplayedInDropDownMenu())
            {
                Warning("'[#1321516] The test has to change the Custom search to City and back to Dimming group'");
                batchControlPage.RealTimeBatchPanel.SelectAttributeDropDown("City");
                batchControlPage.RealTimeBatchPanel.SelectAttributeDropDown("Dimming Group");
            }

            batchControlPage.RealTimeBatchPanel.ClickInsideDropDownMenu();
            batchControlPage.RealTimeBatchPanel.EnterSearchValueDropDownInputWithClear(dimmingGroup);
            var actualValueOfBox = batchControlPage.RealTimeBatchPanel.GetDropDownInputFieldText();
            VerifyEqual("6. Verify The name of testing dimming group displays on the box", dimmingGroup, actualValueOfBox);

            Step("7. Press Search icon");
            batchControlPage.RealTimeBatchPanel.ClickSearchButton();
            batchControlPage.WaitForPreviousActionComplete();
            var listOfTabs = batchControlPage.RealTimeBatchPanel.GetListOfTabsName();
            var firstTab = listOfTabs.FirstOrDefault();
            VerifyEqual("7. Verify There is a new tab name 'StreetLight' and it contains the name of testing streetlight", "StreetLight", firstTab);
            batchControlPage.WaitForPreviousActionComplete();
            var results = batchControlPage.RealTimeBatchPanel.GetListOfStreetlightColumnData("Name");
            var found = results.Contains(streetlight);
            VerifyEqual("7. The streetligh was found", true, found);

            try
            {
                DeleteGeozone(geozone);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("BC_19 Custom Search for streetlight via Install status")]
        public void BC_19()
        {
            var geozone = SLVHelper.GenerateUniqueName("GZNBC19");
            var controller = SLVHelper.GenerateUniqueName("CTRL");
            var streetlight = SLVHelper.GenerateUniqueName("STL");

            List<string> expectedStatuses = new List<string>
            {
                "-",
                "To be verified",
                "Verified",
                "New",
                "Does not exist",
                "To be installed",
                "Installed",
                "Removed"
            };
            var installStatus = expectedStatuses.PickRandom();

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - Batch Control is installed successfully");
            Step(" - Take note a Install status of a streetlight in Real Time Control Area");
            Step("**** Precondition ****\n");

            Step("-> Create data for testing");
            DeleteGeozones("GZNBC19*");
            CreateNewGeozone(geozone);
            CreateNewController(controller, geozone);
            CreateNewDevice(DeviceType.Streetlight, streetlight, controller, geozone);

            var requestToSetInstallStatus = SetValueToDevice(controller, streetlight, "installStatus", installStatus, Settings.GetServerTime());
            VerifyEqual(string.Format("-> Verify the requestToSetInstallStatus is sent successfully (attribute: {0}, value: {1})", "Install status", installStatus), true, requestToSetInstallStatus);

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.BatchControl);

            Step("1. Go to Batch Control app and select the Geozones > testing geozone");
            var batchControlPage = desktopPage.GoToApp(App.BatchControl) as BatchControlPage;
            batchControlPage.GeozoneTreeMainPanel.SelectNode(geozone);

            Step("2. Select 'Install status' for the 1st combobox of Custome Search");
            batchControlPage.RealTimeBatchPanel.SelectAttributeDropDown("Install status");
            var attributeDropDownValue = batchControlPage.RealTimeBatchPanel.GetAttributeValue();
            var searchNameInputText = batchControlPage.RealTimeBatchPanel.GetDropDownInputFieldText();
            VerifyEqual("2. Verify The testing attribute is selected", "Install status", attributeDropDownValue);
            VerifyEqual("2. Verify The text in the 3rd box is updated to the selected attribute", "Install status", searchNameInputText);

            Step("3. Select the operator 'Equal'");
            batchControlPage.RealTimeBatchPanel.SelectOperatorDropDown("Equal");
            var numberOfOperators = batchControlPage.RealTimeBatchPanel.GetNumberOfOperators();
            var operatorValue = batchControlPage.RealTimeBatchPanel.GetOperatorValue();
            VerifyEqual("3. Verify There is only one operator 'Equal' in the combobox", 1, numberOfOperators);
            VerifyEqual("3.Verify The 'Equal' operator is selected", "Equal", operatorValue);

            Step("4. Select the 3rd combobox");
            batchControlPage.RealTimeBatchPanel.ClickInsideDropDownMenu();
            var actualStatuses = batchControlPage.RealTimeBatchPanel.GetListOfAllListedItemsFromDropDownMenu();
            Step("4. Verify The combobox contains the following status:" +
                " '-' " +
                " 'Verified'" +
                " 'New' " +
                " 'Does not exist' " +
                " 'To be installed' " +
                " 'Installed' " +
                " 'Removed' ");
            VerifyEqual("4. Verify There are 7 types of statuses listed: '-', 'To be verified', 'Verified', 'New', 'Does not exist', 'To be installed', 'Installed', 'Removed'", expectedStatuses, actualStatuses);

            Step("5. Enter the invalid Install status ");
            batchControlPage.RealTimeBatchPanel.EnterSearchNameInput(SLVHelper.GenerateString());
            var noMatchesFoundIsDisplayed = batchControlPage.RealTimeBatchPanel.NoMatchesFoundIsDisplayedInDropDownMenu();
            VerifyEqual("5. Verify The text 'No matches found' displays on the 3rd box", true, noMatchesFoundIsDisplayed);

            Step("6. Enter the testing Install status and press Enter");
            batchControlPage.RealTimeBatchPanel.SelectValueDropDownMenu(installStatus);
            var actualValueOfBox = batchControlPage.RealTimeBatchPanel.GetDropDownInputFieldText();
            VerifyEqual("6. Verify The testing Install status displays on the box", installStatus, actualValueOfBox);

            Step("7. Press Search icon");
            batchControlPage.RealTimeBatchPanel.ClickSearchButton();
            batchControlPage.WaitForPreviousActionComplete();
            var streetlightDeviceTab = batchControlPage.RealTimeBatchPanel.GetListOfTabsName().FirstOrDefault();
            var nameColumnOfStreetlightDevice = batchControlPage.RealTimeBatchPanel.GetListOfStreetlightColumnData("Name");
            var streetlightDevicesIsFound = nameColumnOfStreetlightDevice.Contains(streetlight);
            VerifyEqual("7. Verify There is a new tab name 'StreetLight' and it contains the name of testing streetlight. ", "StreetLight", streetlightDeviceTab);
            VerifyEqual("7. Verify The new tab contains the name of testing streetlight", true, streetlightDevicesIsFound);

            try
            {
                DeleteGeozone(geozone);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("BC_25 Send data logs of controllers in Batch Control")]
        public void BC_25()
        {
            var testData = GetTestDataOfBC_25();
            var geozone = testData["Geozone"].ToString();
            var controller = testData["Controller"] as DeviceModel;
            var controllerName = controller.Name;

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - Batch Control is installed successfully");
            Step(" - This test is applied of iLON controller but Smartsim is not, so just verify the command, parameters, values sent to server");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.BatchControl);

            Step("1. Go to Batch Control app and select the Geozones > Real Time Control Area");
            var batchControlPage = desktopPage.GoToApp(App.BatchControl) as BatchControlPage;
            batchControlPage.GeozoneTreeMainPanel.SelectNode(geozone);

            Step("2. Select 'Category' for the 1st combobox of Custome Search");
            batchControlPage.RealTimeBatchPanel.SelectAttributeDropDown("Category");

            Step("3. Select the operator 'Equal' and select 'Controller' for the 3rd combobox");
            batchControlPage.RealTimeBatchPanel.SelectOperatorDropDown("Equal");
            batchControlPage.RealTimeBatchPanel.SelectValueDropDownMenu("Controller Device");
  
            Step("4. Press Search icon");
            batchControlPage.RealTimeBatchPanel.ClickSearchButton();
            batchControlPage.WaitForPreviousActionComplete();

            Step("5. Select controller 'Smartsims' on the list");
            batchControlPage.RealTimeBatchPanel.ClickControllerGridRecord(controllerName);

            Step("6. Press 'Send data logs' button");
            batchControlPage.RealTimeBatchPanel.ClickControllerSendDataLogsButton();
            batchControlPage.WaitForPreviousActionComplete();
            var popUpTitle = batchControlPage.Dialog.GetDialogTitleText();
            var currentPopUpDescription = batchControlPage.Dialog.GetDialogDescriptionText();
            var expectedPopUpDescription = "You are about to ask 1 controller(s) to send their data logs.To confirm your action, type your password:";
            var passwordTextBoxDisplayed = batchControlPage.Dialog.IsRealTimeBatchPasswordInputDisplayed();
            var confirmText = batchControlPage.Dialog.GetConfirmationSubmitButtonText();
            Step("6. Verify a confirmation pop-up displays with");
            Step(" o Title: 'Confirmation' ");
            Step(" o Description: 'You are about to ask {#} controller(s) to send their data logs. To confirm your action, type your password:' ");
            Step(" o A textbox to type password");
            Step(" o A text 'Confirm'");
            VerifyEqual("6. Verify a confirmation pop-up displays with Title: 'Confirmation'", "Confirmation", popUpTitle);
            VerifyEqual("6. Verify Description: 'You are about to ask 1 controller(s) to send their data logs. To confirm your action, type your password:", expectedPopUpDescription, currentPopUpDescription);
            VerifyEqual("6. Verify A textbox to type password", true, passwordTextBoxDisplayed);
            VerifyEqual("6. Verify A text 'Confirm'", "Confirm", confirmText);            

            Step("7. Press X icon to close the pop-up");
            batchControlPage.Dialog.ClickCloseButton();
            batchControlPage.WaitForPreviousActionComplete();
            var popUpIsDisplayed = batchControlPage.Dialog.IsDialogVisible();
            VerifyEqual("7. Verify The pop-up is closed", false, popUpIsDisplayed);

            Step("8. Press 'Send data logs' button and press 'Confirm'");
            batchControlPage.RealTimeBatchPanel.ClickControllerSendDataLogsButton();
            batchControlPage.WaitForPreviousActionComplete();
            batchControlPage.Dialog.ClickConfirmationSubmitButton();
            batchControlPage.WaitForPreviousActionComplete();
            var passwordRejectedFirst = batchControlPage.GetRealtimeBatchWarningMessageTopOfScreen();
            VerifyEqual("8. Verify the message 'Password rejected' displays on the top of the screen", "Password rejected", passwordRejectedFirst);
            batchControlPage.WaitForWarningMessageTopOfScreenDisappeared();

            Step("9. Press 'Send data logs' button and input the wrong password then press 'Confirm' text");
            batchControlPage.RealTimeBatchPanel.ClickControllerSendDataLogsButton();
            batchControlPage.WaitForPreviousActionComplete();
            batchControlPage.Dialog.EnterRealTimeBatchPasswordInputInput("wRonG PasSworD");
            batchControlPage.Dialog.ClickConfirmationSubmitButton();
            batchControlPage.WaitForPreviousActionComplete();
            var passwordRejectedSecond = batchControlPage.GetRealtimeBatchWarningMessageTopOfScreen();
            VerifyEqual("9. Verify the message 'Password rejected' displays on the top of the screen", "Password rejected", passwordRejectedSecond);
            batchControlPage.WaitForWarningMessageTopOfScreenDisappeared();

            Step("10. Press 'Send data logs' button and input the correct password then press 'Confirm' text");
            batchControlPage.RealTimeBatchPanel.ClickControllerSendDataLogsButton();
            batchControlPage.WaitForPreviousActionComplete();
            batchControlPage.Dialog.EnterRealTimeBatchPasswordInputInput(Settings.Users["DefaultTest"].Password);
            batchControlPage.Dialog.ClickConfirmationSubmitButton();
            batchControlPage.WaitForPreviousActionComplete();
            var commandSent = batchControlPage.GetRealtimeBatchWarningMessageTopOfScreen();
            VerifyEqual("10. The message 'Command sent' displays on the top of the screen", "Command sent", commandSent);
        }

        [Test, DynamicRetry]
        [Description("BC_26 Synchronize system time of controllers in Batch Control")]
        public void BC_26()
        {
            var testData = GetTestDataOfBC_26();
            var geozone = testData["Geozone"].ToString();
            var controller = testData["Controller"] as DeviceModel;
            var controllerName = controller.Name;

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - Batch Control is installed successfully");
            Step(" - This test is applied of iLON controller but Smartsim is not, so just verify the command, parameters, values sent to server");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.BatchControl);

            Step("1. Go to Batch Control app and select the Geozones > Real Time Control Area");
            var batchControlPage = desktopPage.GoToApp(App.BatchControl) as BatchControlPage;
            batchControlPage.GeozoneTreeMainPanel.SelectNode(geozone);

            Step("2. Select 'Category' for the 1st combobox of Custome Search");
            batchControlPage.RealTimeBatchPanel.SelectAttributeDropDown("Category");

            Step("3.Select the operator 'Equal' and select 'Controller Device' for the 3rd combobox");
            batchControlPage.RealTimeBatchPanel.SelectOperatorDropDown("Equal");
            batchControlPage.RealTimeBatchPanel.SelectValueDropDownMenu("Controller Device");

            Step("4. Press Search icon");
            batchControlPage.RealTimeBatchPanel.ClickSearchButton();
            batchControlPage.WaitForPreviousActionComplete();

            Step("5. Select controller 'Smartsims' on the list");
            batchControlPage.RealTimeBatchPanel.ClickControllerGridRecord(controllerName);

            Step("6. Press 'Synchronize System Time' button");
            batchControlPage.RealTimeBatchPanel.ClickControllerSyncSystemTimeButton();
            batchControlPage.WaitForPreviousActionComplete();
            var popUpTitle = batchControlPage.Dialog.GetDialogTitleText();
            var currentPopUpDescription = batchControlPage.Dialog.GetDialogDescriptionText();
            var expectedPopUpDescription = "You are about to ask 1 controller(s) to synchronize their system time with your current local time.To confirm your action, type your password:";
            var passwordTextBoxDisplayed = batchControlPage.Dialog.IsRealTimeBatchPasswordInputDisplayed();
            var confirmText = batchControlPage.Dialog.GetConfirmationSubmitButtonText();
            Step("6. Verify a confirmation pop-up displays with");
            Step(" o Title: 'Confirmation' ");
            Step(" o Description: 'You are about to ask {#} controller(s) to synchronize their system time with your current local time' ");
            Step(" o A textbox to type password");
            Step(" o A text 'Confirm'");
            VerifyEqual("6. Verify a confirmation pop-up displays with Title: 'Confirmation'", "Confirmation", popUpTitle);
            VerifyEqual("6. Verify Description: 'You are about to ask 1 controller(s) to send their data logs. To confirm your action, type your password:", expectedPopUpDescription, currentPopUpDescription);
            VerifyEqual("6. Verify A textbox to type password", true, passwordTextBoxDisplayed);
            VerifyEqual("6. Verify A text 'Confirm'", "Confirm", confirmText);

            Step("7. Press X icon to close the pop-up");
            batchControlPage.Dialog.ClickCloseButton();
            batchControlPage.WaitForPreviousActionComplete();
            var popUpIsDisplayed = batchControlPage.Dialog.IsDialogVisible();
            VerifyEqual("7. Verify The pop-up is closed", false, popUpIsDisplayed);

            Step("8. Press 'Synchronize System Time' button and press 'Confirm'");
            batchControlPage.RealTimeBatchPanel.ClickControllerSyncSystemTimeButton();
            batchControlPage.WaitForPreviousActionComplete();
            batchControlPage.Dialog.ClickConfirmationSubmitButton();
            batchControlPage.WaitForPreviousActionComplete();
            var passwordRejectedFirst = batchControlPage.GetRealtimeBatchWarningMessageTopOfScreen();
            VerifyEqual("8. Verify the message 'Password rejected' displays on the top of the screen", "Password rejected", passwordRejectedFirst);
            batchControlPage.WaitForWarningMessageTopOfScreenDisappeared();

            Step("9. Press 'Synchronize System Time' button and input the wrong password then press 'Confirm' text");
            batchControlPage.RealTimeBatchPanel.ClickControllerSyncSystemTimeButton();
            batchControlPage.WaitForPreviousActionComplete();
            batchControlPage.Dialog.EnterRealTimeBatchPasswordInputInput(SLVHelper.GenerateString(8));
            batchControlPage.Dialog.ClickConfirmationSubmitButton();
            batchControlPage.WaitForPreviousActionComplete();
            var passwordRejectedSecond = batchControlPage.GetRealtimeBatchWarningMessageTopOfScreen();
            VerifyEqual("9. Verify the message 'Password rejected' displays on the top of the screen", "Password rejected", passwordRejectedSecond);
            batchControlPage.WaitForWarningMessageTopOfScreenDisappeared();

            Step("10. Press 'Synchronize System Time' button and input the correct password then press 'Confirm' text");
            batchControlPage.RealTimeBatchPanel.ClickControllerSyncSystemTimeButton();
            batchControlPage.WaitForPreviousActionComplete();
            batchControlPage.Dialog.EnterRealTimeBatchPasswordInputInput(Settings.Users["DefaultTest"].Password);
            batchControlPage.Dialog.ClickConfirmationSubmitButton();
            batchControlPage.WaitForPreviousActionComplete();
            var commandSent = batchControlPage.GetRealtimeBatchWarningMessageTopOfScreen();
            VerifyEqual("10. The message 'Command sent' displays on the top of the screen", "Command sent", commandSent);
        }


        [Test, DynamicRetry]
        [Description("BC_27 - SC-782 - Multi-selection in Batch Control")]
        public void BC_27()
        {
            var geozone = SLVHelper.GenerateUniqueName("GZNBC27");
            var controller = SLVHelper.GenerateUniqueName("CTRL");
            var streetlight1 = SLVHelper.GenerateUniqueName("STL01");
            var streetlight2 = SLVHelper.GenerateUniqueName("STL02");
            var streetlight3 = SLVHelper.GenerateUniqueName("STL03");
            var clusterLat = SLVHelper.GenerateCoordinate("12.48677", "12.48739"); 
            var clusterLng = SLVHelper.GenerateCoordinate("74.98945", "74.99020"); 
            var streetlight3Lat = SLVHelper.GenerateCoordinate("12.48677", "12.48739");
            var streetlight3Lng = SLVHelper.GenerateCoordinate("74.98945", "74.99020");

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - Go to Equipment Inventory and create a new geozone and add a cluster containing 2 streetlights");
            Step(" - Add another streetlight 03");
            Step("**** Precondition ****\n");

            Step("-> Create data for testing");
            DeleteGeozones("GZNBC27*");
            CreateNewGeozone(geozone, latMin: "12.48563", latMax: "12.48804", lngMin: "74.98720", lngMax: "74.99235");
            CreateNewController(controller, geozone);
            CreateNewDevice(DeviceType.Streetlight, streetlight1, controller, geozone, lat: clusterLat, lng: clusterLng);
            CreateNewDevice(DeviceType.Streetlight, streetlight2, controller, geozone, lat: clusterLat, lng: clusterLng);
            CreateNewDevice(DeviceType.Streetlight, streetlight3, controller, geozone, lat: streetlight3Lat, lng: streetlight3Lng);

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.BatchControl);

            Step("1. Go to Batch Control app");           
            var batchControlPage = desktopPage.GoToApp(App.BatchControl) as BatchControlPage;

            Step("2. Verify The Batch Control on geozone tree displays the correct number of devices.");
            batchControlPage.GeozoneTreeMainPanel.SelectNode(geozone);
            var devicesCount = batchControlPage.GeozoneTreeMainPanel.GetSelectedNodeDevicesCount();
            VerifyEqual("2. Verify The Batch Control on geozone tree displays the correct number of devices", 4, devicesCount);

            Step("3. Hover the cluster");
            batchControlPage.Map.MoveToDeviceGL(clusterLng, clusterLat);          

            Step("4. Verify the tooltip displays with 2 rows");
            Step(" o 'Device Cluster'");
            Step(" o '2 devices'");
            var clusterName = batchControlPage.Map.GetDeviceNameGL();
            var deviceCount = batchControlPage.Map.GetTooltipDevicesCountGL();
            VerifyEqual("4. Verify A tooltip displays: Device Cluster", "Device Cluster", clusterName);
            VerifyEqual("4. Verify A tooltip displays: 2 devices", "2 devices", deviceCount);

            Step("5. Press the cluster on the map");
            batchControlPage.Map.SelectDeviceGL(clusterLng, clusterLat);
            batchControlPage.RealTimeBatchPanel.WaitForDataGridLoaded();

            Step("6. Verify There is a new tab name 'Streetlight' in the bellow panel");
            Step(" o 'Selected Devices' (2 devices)");
            Step(" o The 2 streetlights of cluster are added into the grid");
            var streetlights = batchControlPage.RealTimeBatchPanel.GetListOfStreetlightColumnData("Name");
            VerifyEqual("6. Verify Title: Selected Devices (2 devices)", "Selected Devices (2 devices)", batchControlPage.RealTimeBatchPanel.GetPanelTitleText());
            VerifyEqual("6. Verify The 2 streetlights of cluster are added into the grid", new List<string> { streetlight1, streetlight2 }, streetlights, false);

            Step("7. Verify The cluster icon on the map is updated to the selected icon (has a dark border)");
            var clusterSprite = batchControlPage.Map.GetClusterSprite(clusterLng, clusterLat);
            VerifyEqual("7. Verify The cluster icon on the map is updated to the selected icon (has a dark border)", true, clusterSprite.IsSelected);

            Step("8. Press the streetlight 03 on the map");
            batchControlPage.Map.SelectDeviceGL(streetlight3Lng, streetlight3Lat);
            batchControlPage.RealTimeBatchPanel.WaitForDataGridLoaded();

            Step("9. Verify The tab name 'Streetlight' is updated");
            Step(" o 'Selected Devices' (1 devices)");
            Step(" o The streetlight 03 is added into the grid");
            streetlights = batchControlPage.RealTimeBatchPanel.GetListOfStreetlightColumnData("Name");
            VerifyEqual("9. Verify Title: Selected Devices (1 devices)", "Selected Devices (1 devices)", batchControlPage.RealTimeBatchPanel.GetPanelTitleText());
            VerifyTrue("9. Verify The streetlight 03 is added into the grid", streetlights.Count == 1 && streetlight3 == streetlights.FirstOrDefault(), streetlight3, string.Join(",", streetlights));

            Step("10. Verify the cluster icon on the map is updated to the unselected icon (has no dark border)");
            clusterSprite = batchControlPage.Map.GetClusterSprite(clusterLng, clusterLat);
            VerifyEqual("10. Verify the cluster icon on the map is updated to the unselected icon (has no dark border)", true, !clusterSprite.IsSelected);

            Step("11. Press Shift and select cluster and streetlight 03");
            batchControlPage.Map.SelectDevicesGL(clusterLng, clusterLat);
            batchControlPage.RealTimeBatchPanel.WaitForDataGridLoaded();

            Step("12. Verify The tab name 'Streetlight' is updated");
            Step(" o 'Selected Devices' (3 devices)");
            Step(" o The 3 streetlights are added into the grid");
            streetlights = batchControlPage.RealTimeBatchPanel.GetListOfStreetlightColumnData("Name");
            VerifyEqual("12. Verify Title: Selected Devices (3 devices)", "Selected Devices (3 devices)", batchControlPage.RealTimeBatchPanel.GetPanelTitleText());
            VerifyEqual("12. Verify The 3 streetlights of cluster are added into the grid", new List<string> { streetlight1, streetlight2, streetlight3 }, streetlights, false);

            Step("13. Verify Both cluster and streetlight 03 icons on the map is updated to the selected icon");
            clusterSprite = batchControlPage.Map.GetClusterSprite(clusterLng, clusterLat);
            var streetlight3Sprite = batchControlPage.Map.GetDeviceSprite(streetlight3Lng, streetlight3Lat);
            VerifyEqual("13. Verify The cluster icon on the map is updated to the selected icon (has a dark border)", true, clusterSprite.IsSelected);
            VerifyEqual("13. Verify The streetlight 03 icon on the map is updated to the selected icon (has a dark border)", true, streetlight3Sprite.IsSelected);

            Step("14. Click the line of streetlight 03 on the grid");
            batchControlPage.RealTimeBatchPanel.ClickStreetlightGridRecord(streetlight3);
            batchControlPage.RealTimeBatchPanel.WaitForDataGridLoaded();

            Step("15. Verify");
            Step(" o The cluster icon on the map is updated to the unselected icon");
            Step(" o The streetlight icon on the map is updated to the selected icon");
            clusterSprite = batchControlPage.Map.GetClusterSprite(clusterLng, clusterLat);
            streetlight3Sprite = batchControlPage.Map.GetDeviceSprite(streetlight3Lng, streetlight3Lat);
            VerifyEqual("15. Verify the cluster icon on the map is updated to the unselected icon (has no dark border)", true, !clusterSprite.IsSelected);
            VerifyEqual("15. Verify The streetlight 03 icon on the map is updated to the selected icon (has a dark border)", true, streetlight3Sprite.IsSelected);

            Step("16. Click randomly a line of streetlights in the cluster");
            var rndStreetlight = new List<string> { streetlight1, streetlight2 }.PickRandom();
            batchControlPage.RealTimeBatchPanel.ClickStreetlightGridRecord(rndStreetlight);
            batchControlPage.RealTimeBatchPanel.WaitForDataGridLoaded();

            Step("17. Verify");
            Step(" o The cluster icon on the map is updated to the selected icon");
            Step(" o The streetlight icon on the map is updated to the unselected icon");
            clusterSprite = batchControlPage.Map.GetClusterSprite(clusterLng, clusterLat);
            streetlight3Sprite = batchControlPage.Map.GetDeviceSprite(streetlight3Lng, streetlight3Lat);
            VerifyEqual("17. Verify The cluster icon on the map is updated to the selected icon (has a dark border)", true, clusterSprite.IsSelected);
            VerifyEqual("17. Verify The streetlight 03 icon on the map is updated to the unselected icon (has no dark border)", true, !streetlight3Sprite.IsSelected);

            try
            {
                DeleteGeozone(geozone);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("SC-1475 - Batch Control- Search filter not working on numeric attributes in certain cases")]
        public void BC_28()
        {
            var geozone = SLVHelper.GenerateUniqueName("GZNBC28");
            var controller = SLVHelper.GenerateUniqueName("CTRL");
            var streetlight = SLVHelper.GenerateUniqueName("STL");

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - Create a testing geozone with a streetlight and set the Lamp Wattage in Inventory tab > Lamp section to the value > 1000");
            Step("**** Precondition ****\n");

            Step("-> Create data for testing");
            DeleteGeozones("GZNBC28*");
            CreateNewGeozone(geozone);
            CreateNewController(controller, geozone);
            CreateNewDevice(DeviceType.Streetlight, streetlight, controller, geozone);

            var value = SLVHelper.GenerateStringInteger(1001, 9999);
            var request = SetValueToDevice(controller, streetlight, "power", value, Settings.GetServerTime());
            VerifyEqual(string.Format("-> Verify the request is sent successfully (attribute: {0}, value: {1})", "power", value), true, request);

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.BatchControl);           

            Step("1. Go to Batch Control app");
            Step("2. Verify Batch Control page is routed and loaded successfully");
            var batchControlPage = desktopPage.GoToApp(App.BatchControl) as BatchControlPage;

            Step("3. Select the testing geozone");
            batchControlPage.GeozoneTreeMainPanel.SelectNode(geozone);

            Step("4. Select Lamp Wattage(W) in the Custom Search and the operator is Equal");
            batchControlPage.RealTimeBatchPanel.SelectAttributeDropDown("Lamp Wattage (W)");
            batchControlPage.RealTimeBatchPanel.SelectOperatorDropDown("Equal");

            Step("5. Input the value of Lamp Wattage and press Search button");
            batchControlPage.RealTimeBatchPanel.EnterSearchNameInput(value);
            batchControlPage.RealTimeBatchPanel.ClickSearchButton();
            batchControlPage.WaitForPreviousActionComplete();

            Step("6. Verify The testing streetlight displays in the list");
            var resultStreetlights = batchControlPage.RealTimeBatchPanel.GetListOfStreetlightColumnData("Name");
            VerifyEqual("6. Verify The testing streetlight displays in the list", true, resultStreetlights.Contains(streetlight));

            Step("7. Change the operator randomly to one of the following: 'Not equal', 'Greater than', 'Less than', then press Search button");
            var operatorList1 = new List<string> { "Not equal", "Greater than", "Less than" };
            batchControlPage.RealTimeBatchPanel.SelectOperatorDropDown(operatorList1.PickRandom());
            batchControlPage.RealTimeBatchPanel.ClickSearchButton();
            batchControlPage.WaitForPreviousActionComplete();
            
            Step("8. Verify Grid view is empty");
            resultStreetlights = batchControlPage.RealTimeBatchPanel.GetListOfStreetlightColumnData("Name");
            VerifyEqual("8. Verify Grid view is empty", true, !resultStreetlights.Any());

            Step("9. Change the operator to 'Greater or equal' or 'Less or equal', then press Search button");
            var operatorList2 = new List<string> { "Greater or equal", "Less or equal" };
            batchControlPage.RealTimeBatchPanel.SelectOperatorDropDown(operatorList2.PickRandom());
            batchControlPage.RealTimeBatchPanel.ClickSearchButton();
            batchControlPage.WaitForPreviousActionComplete();

            Step("10. Verify The testing streetlight displays in the list");
            resultStreetlights = batchControlPage.RealTimeBatchPanel.GetListOfStreetlightColumnData("Name");
            VerifyEqual("[#1282096] 10. Verify The testing streetlight displays in the list", true, resultStreetlights.Contains(streetlight));

            try
            {
                DeleteGeozone(geozone);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("BC_29 1061890 Location Search - in Batch Control")]
        public void BC_29()
        {
            var testData = GetTestDataOfBC_29();
            var latMin = testData["LatMin"];
            var latMax = testData["LatMax"];
            var lngMin = testData["LngMin"];
            var lngMax = testData["LngMax"];
            var partialAddress = testData["PartialAddress"];
            var fullAddress = testData["FullAddress"];
            var geozone = SLVHelper.GenerateUniqueName("GZNBC29");
            var controller = SLVHelper.GenerateUniqueName("CTRL");
            var streetlight = SLVHelper.GenerateUniqueName("STL");

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - Create a testing geozone containing a streetlight which is located at a well-known location. Ex: Champ de Mars, 5 Avenue Anatole France, 75007 Paris, France");
            Step("**** Precondition ****\n");

            Step("-> Create data for testing");
            DeleteGeozones("GZNBC29*");
            var streetlightLat = SLVHelper.GenerateCoordinate("48.26468", "48.26670");
            var streetlightLng = SLVHelper.GenerateCoordinate("2.69184", "2.69438");
            CreateNewGeozone(geozone, latMin: latMin, latMax: latMax, lngMin: lngMin, lngMax: lngMax);
            CreateNewController(controller, geozone, lat: SLVHelper.GenerateCoordinate(latMin, latMax), lng: SLVHelper.GenerateCoordinate(lngMin, lngMax));
            CreateNewDevice(DeviceType.Streetlight, streetlight, controller, geozone, lat: streetlightLat, lng: streetlightLng);

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.BatchControl);

            Step("1. Go to Batch Control app");
            var batchControlPage = desktopPage.GoToApp(App.BatchControl) as BatchControlPage;

            Step("2. Verify There is a button with icon: Globe icon on the top-right corner of the GeoZone tree");
            VerifyEqual("2. Verify There is a button with icon: Globe icon on the top-right corner of the GeoZone tree", true, batchControlPage.GeozoneTreeMainPanel.IsMapFilterButtonVisible());

            Step("3. Minimize the Batch Control widget and hover the Global button on the geozone tree");
            batchControlPage.RealTimeBatchPanel.MinimizePanelByDragDrop();
            batchControlPage.GeozoneTreeMainPanel.HoverMapSearchButton();

            Step("4. Verify The text 'Map Search' displays");
            VerifyEqual("4. Verify The text 'Map Search' displays", "Map Search", batchControlPage.GeozoneTreeMainPanel.GetMapSearchButtonTooltip());

            Step("5. Click the button");
            batchControlPage.GeozoneTreeMainPanel.ClickMapSearchButton();
            batchControlPage.GeozoneTreeMainPanel.WaitForMapSearchPanelDisplayed();

            Step("6. Verify A panel displays with");
            Step(" o Title: Map Search");
            Step(" o Text: Search by Location");
            Step(" o Textbox with a Magnifying Glass icon and the text 'Search in map'");
            Step(" o Button: Back");
            VerifyEqual("6. Verify A panel displays: Title: Map Search", "Map Search", batchControlPage.GeozoneTreeMainPanel.MapSearchPanel.GetPanelTitleText());
            VerifyEqual("6. Verify A panel displays: Text: Search by Location", "Search by Location", batchControlPage.GeozoneTreeMainPanel.MapSearchPanel.GetContentText());
            VerifyEqual("6. Verify A panel displays: Textbox", true, batchControlPage.GeozoneTreeMainPanel.MapSearchPanel.IsSearchInputDisplayed());
            VerifyEqual("6. Verify A panel displays: Textbox with a Magnifying Glass icon", true, batchControlPage.GeozoneTreeMainPanel.MapSearchPanel.IsSearchInputHasMagnifyingGlass());
            VerifyEqual("6. Verify A panel displays: Textbox with the text 'Search in map'", "Search in map", batchControlPage.GeozoneTreeMainPanel.MapSearchPanel.GetSearchPlaceholder());
            VerifyEqual("6. Verify A panel displays: Button: Back", true, batchControlPage.GeozoneTreeMainPanel.MapSearchPanel.IsBackButtonDisplayed());

            Step("7. Enter a partial of the testing address into the input");
            batchControlPage.GeozoneTreeMainPanel.MapSearchPanel.EnterSearchInput(partialAddress);
            batchControlPage.GeozoneTreeMainPanel.MapSearchPanel.WaitForSuggestionsDisplayed();

            Step("8. Verify The search results appear as user types. The matched words are bold");
            var searchSuggestionsBoldTextList = batchControlPage.GeozoneTreeMainPanel.MapSearchPanel.GetSearchSuggestionsBoldText();
            VerifyTrue("8. Verify The search results appear as user types. The matched words are bold", searchSuggestionsBoldTextList.All(p => p.Equals(partialAddress)), "all matched words are bold", string.Join(", ", searchSuggestionsBoldTextList));

            Step("9. Input the full value of the testing address, then click on the 1 result in the list");
            batchControlPage.GeozoneTreeMainPanel.MapSearchPanel.ClickClearSearchButton();
            batchControlPage.GeozoneTreeMainPanel.MapSearchPanel.WaitForClearSearchButtonDisappeared();
            batchControlPage.GeozoneTreeMainPanel.MapSearchPanel.EnterSearchInput(fullAddress);
            batchControlPage.GeozoneTreeMainPanel.MapSearchPanel.WaitForSuggestionsDisplayed();
            batchControlPage.GeozoneTreeMainPanel.MapSearchPanel.SelectSearchSuggestion();

            Step("10. Verify The map is centered on the selected location and zoomed to level 15(50 m)");
            Wait.ForGLMapStopFlying();
            VerifyEqual("10. Verify The map is centered on the selected location and zoomed to level 15(50 m)", "50 m", batchControlPage.Map.GetMapGLScaleText());

            Step("11. Verify There is an Orange location icon on the center of the map");
            VerifyEqual("11. Verify There is location icon on the center of the map", true, batchControlPage.Map.IsLocationSearchMarkerDisplayed());
            VerifyEqual("11. Verify There is Orange icon", true, batchControlPage.Map.GetLocationSearchMarkerImageSrc().Contains("marker-generic.svg"));

            Step("12. Select the streetlight on the map");
            batchControlPage.Map.SelectDeviceGL(streetlightLng, streetlightLat);

            Step("13. Verify The device is selected and the Map Search panel remains visible");
            VerifyEqual("13. Verify The device is selected", true, batchControlPage.Map.HasSelectedDevicesInMapGL());
            VerifyEqual("13. Verify Map Search panel remains visible", true, batchControlPage.GeozoneTreeMainPanel.IsMapSearchPanelDisplayed());

            Step("14. Verify The Batch Control panel displays with the information of the selected streetlight");
            batchControlPage.RealTimeBatchPanel.SelectTab("StreetLight");
            var streetlights = batchControlPage.RealTimeBatchPanel.GetListOfStreetlightColumnData("Name");
            VerifyEqual("14. The Batch Control panel displays with the information of the selected streetlight", streetlight, streetlights.FirstOrDefault());

            Step("15. Press X icon on the Search box");
            batchControlPage.GeozoneTreeMainPanel.MapSearchPanel.ClickClearSearchButton();
            batchControlPage.GeozoneTreeMainPanel.MapSearchPanel.WaitForClearSearchButtonDisappeared();

            Step("16. Verify The input is cleared");
            VerifyEqual("16. Verify The input is cleared", "", batchControlPage.GeozoneTreeMainPanel.MapSearchPanel.GetSearchValue());

            Step("17. Press Back button");
            batchControlPage.GeozoneTreeMainPanel.MapSearchPanel.ClickBackToolbarButton();
            batchControlPage.GeozoneTreeMainPanel.WaitForMapSearchPanelDisappeared();

            Step("18. Verify Geozone tree panel displays again");
            Step(" o The Batch Control panel remains visible");
            VerifyEqual("18. Verify The Batch Control panel remains visible", true, batchControlPage.IsRealTimeBatchPanelDisplayed());

            try
            {
                DeleteGeozone(geozone);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("BC_1377638 - Bug Id 1328646 - Id is displaying while searching with Custom search in Batch Control")]
        public void BC_1377638()
        {
            var attributesList = new List<string> { "Controller ID", "Category", "Dimming group" };
            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.BatchControl);

            Step("1. Go to Batch Control app");
            var batchControlPage = desktopPage.GoToApp(App.BatchControl) as BatchControlPage;

            Step("2. Verify Batch Control widget displays on the map");
            VerifyEqual("2. Verify Batch Control widget displays on the map", true, batchControlPage.IsRealTimeBatchPanelDisplayed());

            Step("3. Select 'Install status' at Custom Search in the Batch Control widget");
            batchControlPage.RealTimeBatchPanel.SelectAttributeDropDown("Install status");

            Step("4. Select randomly an option in the third commbobox, then press Search button");
            var value = batchControlPage.RealTimeBatchPanel.SelectRandomValueFieldDropDown();
            batchControlPage.RealTimeBatchPanel.ClickSearchButton();

            Step("5. Wait for the search finishes");
            batchControlPage.WaitForPreviousActionComplete();

            Step("6. Verify The text is updated to: Results for '[the selected option's text]'");
            var expectedTitleCaption = string.Format("Results for '{0}'", value);
            var actualTitleCaption = batchControlPage.RealTimeBatchPanel.GetPanelTitleText();
            VerifyEqual("6. Verify The text is updated to: Results for '[the selected option's text]'", expectedTitleCaption, actualTitleCaption);

            Step("7. Repeat the test with other Custom Search bellow: Controller Id; Category; Dimming Group");
            foreach (var attribute in attributesList)
            {
                Step(string.Format("-> Select '{0}' at Custom Search in the Batch Control widget", attribute));
                batchControlPage.RealTimeBatchPanel.SelectAttributeDropDown(attribute);

                Step("-> Select randomly an option in the third commbobox, then press Search button");
                value = batchControlPage.RealTimeBatchPanel.SelectRandomValueFieldDropDown();
                batchControlPage.RealTimeBatchPanel.ClickSearchButton();

                Step("-> Wait for the search finishes");
                batchControlPage.WaitForPreviousActionComplete();

                Step("-> Verify The text is updated to: Results for '[the selected option's text]'");
                expectedTitleCaption = string.Format("Results for '{0}'", value);
                actualTitleCaption = batchControlPage.RealTimeBatchPanel.GetPanelTitleText();
                VerifyEqual(string.Format("[{0}] Verify The text is updated to: Results for '[the selected option's text]'", attribute), expectedTitleCaption, actualTitleCaption);
            }
        }

        #endregion //Test Cases

        #region Private methods

        #region Input XML data

        private Dictionary<string, object> GetCommonTestData()
        {
            var realtimeGeozone = Settings.CommonTestData[0];
            var testData = new Dictionary<string, object>();
            testData.Add("Geozone", realtimeGeozone.Path);
            var controller = realtimeGeozone.Devices.Where(p => p.Type == DeviceType.Controller && p.Status == DeviceStatus.Working).FirstOrDefault();
            testData.Add("Controller", controller);
            var streetlights = realtimeGeozone.Devices.Where(p => p.Type == DeviceType.Streetlight && p.Status == DeviceStatus.Working).ToList();
            testData.Add("Streetlights", streetlights);

            return testData;
        }

        private Dictionary<string, string> GetTestDataOfBC_01()
        {
            var testCaseName = "BC_01";
            var xmlUtility = new XmlUtility(Settings.BC_TEST_DATA_FILE_PATH);
            var testData = new Dictionary<string, string>();

            testData.Add("Geozone", xmlUtility.GetSingleNodeText(string.Format(Settings.BC_XPATH_PREFIX, testCaseName, "Geozone")));            
            var streetlightInfo = xmlUtility.GetSingleNode(string.Format(Settings.BC_XPATH_PREFIX, testCaseName, "Streetlight"));
            testData.Add("StreetlightLat", streetlightInfo.GetAttrVal("lat"));
            testData.Add("StreetlightLng", streetlightInfo.GetAttrVal("lng"));
            var controllerInfo = xmlUtility.GetSingleNode(string.Format(Settings.BC_XPATH_PREFIX, testCaseName, "Controller"));
            testData.Add("ControllerId", controllerInfo.GetAttrVal("id"));
            testData.Add("ControllerName", controllerInfo.GetAttrVal("name"));

            return testData;
        }

        private Dictionary<string, object> GetTestDataOfBC_25()
        {
            return GetCommonTestData();
        }

        private Dictionary<string, object> GetTestDataOfBC_26()
        {
            return GetCommonTestData();
        }

        private Dictionary<string, string> GetTestDataOfBC_29()
        {
            var testCaseName = "BC_29";
            var xmlUtility = new XmlUtility(Settings.BC_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, string>();
            var geozoneInfo = xmlUtility.GetSingleNode(string.Format(Settings.BC_XPATH_PREFIX, testCaseName, "Geozone"));
            testData.Add("LatMin", geozoneInfo.GetAttrVal("latMin"));
            testData.Add("LatMax", geozoneInfo.GetAttrVal("latMax"));
            testData.Add("LngMin", geozoneInfo.GetAttrVal("lngMin"));
            testData.Add("LngMax", geozoneInfo.GetAttrVal("lngMax"));

            testData.Add("FullAddress", geozoneInfo.GetAttrVal("fullAddress"));
            testData.Add("PartialAddress", geozoneInfo.GetAttrVal("partialAddress"));

            return testData;
        }

        #endregion //Input XML data

        #endregion //Private methods
    }
}
