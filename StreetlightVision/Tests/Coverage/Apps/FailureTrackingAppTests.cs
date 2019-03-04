using NUnit.Framework;
using StreetlightVision.Extensions;
using StreetlightVision.Models;
using StreetlightVision.Pages;
using StreetlightVision.Utilities;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.Xml;

namespace StreetlightVision.Tests.Coverage.Apps
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    public class FailureTrackingAppTests : TestBase
    {
        #region Variables

        #endregion //Variables

        #region Contructors

        #endregion //Contructors

        #region Test Cases
        
        [Test, DynamicRetry]
        [Description("FT_01 View device failures from geozone tree")]
        public void FT_01()
        {
            var testData = GetTestDataOfTestFT_01();
            var geozone = testData["Geozone"].ToString();
            var streetlights = testData["Streetlights"] as List<DeviceModel>;
            var streetlightName = streetlights.PickRandom().Name;

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.FailureTracking);

            Step("1. Go to Failure Tracking app");
            Step("2. Expected Failure Tracking page is routed and loaded successfully");
            var failureTrackingPage = desktopPage.GoToApp(App.FailureTracking) as FailureTrackingPage;

            Step("3. Select a device from geozone");
            failureTrackingPage.GeozoneTreeMainPanel.SelectNode(geozone + @"\" + streetlightName);

            Step(@"4. **Verify**
                       - The device is selected in geozone tree
                       - The device is selected in map (SLV-3675)
                       - Failure Tracking panel appears");
            var selectedDeviceFromGeozoneTree = failureTrackingPage.GeozoneTreeMainPanel.GetSelectedNodeText();
            var deviceModel = failureTrackingPage.Map.GetFirstSelectedDevice();
            var selectedDeviceFromMap = failureTrackingPage.Map.MoveAndGetDeviceNameGL(deviceModel.Longitude, deviceModel.Latitude);
            VerifyEqual("4. Verify The device is selected in geozone tree", streetlightName, selectedDeviceFromGeozoneTree);
            VerifyEqual("4. Verify The device is selected in map", streetlightName, selectedDeviceFromMap);
            VerifyEqual("4. Verify Failure Tracking panel appears", true, failureTrackingPage.FailureTrackingDetailsPanel.IsPanelVisible());
        }
        
        [Test, DynamicRetry]
        [Description("FT_02 View device failures from map")]
        public void FT_02()
        {
            var testData = GetTestDataOfTestFT_02();
            var geozone = testData["Geozone"].ToString();
            var streetlights = testData["Streetlights"] as List<DeviceModel>;
            var streetlight = streetlights.PickRandom();

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.FailureTracking);

            Step("1. Go to Failure Tracking app");
            Step("2. Expected Failure Tracking page is routed and loaded successfully");
            var failureTrackingPage = desktopPage.GoToApp(App.FailureTracking) as FailureTrackingPage;

            Step("3. Select a device from map");
            failureTrackingPage.GeozoneTreeMainPanel.SelectNode(geozone);
            failureTrackingPage.Map.SelectDeviceGL(streetlight.Longitude, streetlight.Latitude);

            Step(@"4. **Verify**
                       - The device is selected in geozone tree (SLV-3675)
                       - The device is selected in map
                       - Failure Tracking panel appears");
            var selectedDeviceFromGeozoneTree = failureTrackingPage.GeozoneTreeMainPanel.GetSelectedNodeText();
            var selectedDeviceFromMap = failureTrackingPage.Map.MoveAndGetDeviceNameGL(streetlight.Longitude, streetlight.Latitude);
            VerifyEqual("4. Verify The device is selected in geozone tree", streetlight.Name, selectedDeviceFromGeozoneTree);
            VerifyEqual("4. Verify The device is selected in map", streetlight.Name, selectedDeviceFromMap);
            VerifyEqual("4. Verify Failure Tracking panel appears", true, failureTrackingPage.FailureTrackingDetailsPanel.IsPanelVisible());
        }
        
        [Test, DynamicRetry]
        [Description("FT_03 Failure Tracking panel")]
        public void FT_03()
        {
            var testData = GetTestDataOfTestFT_03();
            var geozone1 = testData["DeviceWithFailure.geozone"];
            var device1 = testData["DeviceWithFailure.name"];
            var geozone2 = testData["DeviceWithNoFailure.geozone"];
            var device2 = testData["DeviceWithNoFailure.name"];

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.FailureTracking, App.EquipmentInventory);

            Step("1. Go to Equipment Inventory and device 1 from geozone which has failure data");
            var equipmentInventoryPage = desktopPage.GoToApp(App.EquipmentInventory) as EquipmentInventoryPage;
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(geozone1 + @"\" + device1);

            Step("2. Noted its values (update if value is empty):");
            Step(" - Geozone path");
            Step(" - Unique address");
            Step(" - Address 1");
            Step(" - City");
            var isUpdated = false;
            var notedGeozonePath = Settings.RootGeozoneName + "/" + geozone1.Replace("\\", "/");
            var notedUniqueAddress = equipmentInventoryPage.StreetlightEditorPanel.GetUniqueAddressValue();
            if(string.IsNullOrEmpty(notedUniqueAddress))
            {
                notedUniqueAddress = SLVHelper.GenerateMACAddress();
                equipmentInventoryPage.StreetlightEditorPanel.EnterUniqueAddressInput(notedUniqueAddress);
                isUpdated = true;
            }
            equipmentInventoryPage.StreetlightEditorPanel.SelectTab("Inventory");
            equipmentInventoryPage.StreetlightEditorPanel.ExpandGroupsActiveTab();
            var notedAddress1 = equipmentInventoryPage.StreetlightEditorPanel.GetAddress1Value();
            if (string.IsNullOrEmpty(notedAddress1))
            {
                notedAddress1 = SLVHelper.GenerateString(6);
                equipmentInventoryPage.StreetlightEditorPanel.EnterAddress1Input(notedAddress1);
                isUpdated = true;
            }
            var notedCity = equipmentInventoryPage.StreetlightEditorPanel.GetCityValue();
            if (string.IsNullOrEmpty(notedCity))
            {
                notedCity = SLVHelper.GenerateString(6);
                equipmentInventoryPage.StreetlightEditorPanel.EnterCityInput(notedCity);
                isUpdated = true;
            }
            if (isUpdated)
            {
                equipmentInventoryPage.StreetlightEditorPanel.ClickSaveButton();
                equipmentInventoryPage.WaitForPreviousActionComplete();
                equipmentInventoryPage.WaitForEditorPanelDisappeared();
            }

            Step("3. Switch to Failure Tracking app and select device 1");
            var failureTrackingPage = equipmentInventoryPage.AppBar.SwitchTo(App.FailureTracking) as FailureTrackingPage;
            failureTrackingPage.WaitForPreviousActionComplete();
            failureTrackingPage.WaitForDetailsPanelDisplayed();
            
            Step(@"4. **Verify**
                       - Failure Tracking panel appears:
                           - Title: Failure Tracking
                           - Device icon - Device name
                           - Geozone path
                           - Unique address
                           - Address 1
                           - City
                           - Failure list: failure entries.Each entry has: failure level icon, failure name, failure time");

            VerifyEqual("4. Verify Panel title", "Failure Tracking", failureTrackingPage.FailureTrackingDetailsPanel.GetPanelTitleText());
            VerifyEqual("4. Verify Device icon", true, failureTrackingPage.FailureTrackingDetailsPanel.GetDeviceIconImageUrl().Contains(".png"));
            VerifyEqual("4. Verify Device name", device1, failureTrackingPage.FailureTrackingDetailsPanel.GetDeviceNameValueText());
            VerifyEqual("4. Verify Geozone path", notedGeozonePath, failureTrackingPage.FailureTrackingDetailsPanel.GetGeozonePathValueText());
            VerifyEqual("4. Verify Unique address", notedUniqueAddress, failureTrackingPage.FailureTrackingDetailsPanel.GetUniqueAdressValueText());
            VerifyEqual("4. Verify Address 1", notedAddress1, failureTrackingPage.FailureTrackingDetailsPanel.GetAdress1ValueText());
            VerifyEqual("4. Verify City", notedCity, failureTrackingPage.FailureTrackingDetailsPanel.GetCityValueText());

            var failureList = failureTrackingPage.FailureTrackingDetailsPanel.GetListOfFailures();
            foreach (var failure in failureList)
            {
                VerifyEqual("4. Verify Failure list: Icon", true, failure.Name != string.Empty);
                VerifyEqual("4. Verify Failure list: Name", true, failure.Name != string.Empty);
                VerifyEqual("4. Verify Failure list: Time", true, failure.Time != string.Empty);
            }

            Step("5. Switch to Equipment Inventory and device 2 from geozone which has no failure data");
            equipmentInventoryPage = failureTrackingPage.AppBar.SwitchTo(App.EquipmentInventory) as EquipmentInventoryPage;
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(geozone2 + @"\" + device2);

            Step("6. Noted its values (update if value is empty):");
            Step(" - Geozone path");
            Step(" - Unique address");
            Step(" - Address 1");
            Step(" - City");
            isUpdated = false;
            notedGeozonePath = Settings.RootGeozoneName + "/" + geozone2.Replace("\\", "/");
            notedUniqueAddress = equipmentInventoryPage.StreetlightEditorPanel.GetUniqueAddressValue();
            if (string.IsNullOrEmpty(notedUniqueAddress))
            {
                notedUniqueAddress = SLVHelper.GenerateMACAddress();
                equipmentInventoryPage.StreetlightEditorPanel.EnterUniqueAddressInput(notedUniqueAddress);
                isUpdated = true;
            }
            equipmentInventoryPage.StreetlightEditorPanel.SelectTab("Inventory");
            equipmentInventoryPage.StreetlightEditorPanel.ExpandGroupsActiveTab();
            notedAddress1 = equipmentInventoryPage.StreetlightEditorPanel.GetAddress1Value();
            if (string.IsNullOrEmpty(notedAddress1))
            {
                notedAddress1 = SLVHelper.GenerateString(6);
                equipmentInventoryPage.StreetlightEditorPanel.EnterAddress1Input(notedAddress1);
                isUpdated = true;
            }
            notedCity = equipmentInventoryPage.StreetlightEditorPanel.GetCityValue();
            if (string.IsNullOrEmpty(notedCity))
            {
                notedCity = SLVHelper.GenerateString(6);
                equipmentInventoryPage.StreetlightEditorPanel.EnterCityInput(notedCity);
                isUpdated = true;
            }
            if (isUpdated)
            {
                equipmentInventoryPage.StreetlightEditorPanel.ClickSaveButton();
                equipmentInventoryPage.WaitForPreviousActionComplete();
                equipmentInventoryPage.WaitForEditorPanelDisappeared();
            }

            Step("7. Switch to Failure Tracking app and select device 2");
            failureTrackingPage = equipmentInventoryPage.AppBar.SwitchTo(App.FailureTracking) as FailureTrackingPage;
            failureTrackingPage.WaitForPreviousActionComplete();
            failureTrackingPage.WaitForDetailsPanelDisplayed();

            Step(@"8. **Verify**
                       - Failure Tracking panel appears:
                           - Title: Failure Tracking
                           - Device icon - Device name
                           - Geozone path
                           - Unique address
                           - Address 1
                           - City
                           - Failure list: 'No data'");
            VerifyEqual("8. Verify Panel title", "Failure Tracking", failureTrackingPage.FailureTrackingDetailsPanel.GetPanelTitleText());
            VerifyEqual("8. Verify Device icon", true, failureTrackingPage.FailureTrackingDetailsPanel.GetDeviceIconImageUrl().Contains(".png"));
            VerifyEqual("8. Verify Device name", device2, failureTrackingPage.FailureTrackingDetailsPanel.GetDeviceNameValueText());
            VerifyEqual("8. Verify Geozone path", notedGeozonePath, failureTrackingPage.FailureTrackingDetailsPanel.GetGeozonePathValueText());
            VerifyEqual("8. Verify Unique address", notedUniqueAddress, failureTrackingPage.FailureTrackingDetailsPanel.GetUniqueAdressValueText());
            VerifyEqual("8. Verify Address 1", notedAddress1, failureTrackingPage.FailureTrackingDetailsPanel.GetAdress1ValueText());
            VerifyEqual("8. Verify City", notedCity, failureTrackingPage.FailureTrackingDetailsPanel.GetCityValueText());
            VerifyEqual("8. Verify Failure list", "No data", failureTrackingPage.FailureTrackingDetailsPanel.GetHistoryFailuresMessageText());

            Step("9. Click Back button");
            failureTrackingPage.FailureTrackingDetailsPanel.ClickCloseButton();
            failureTrackingPage.WaitForDetailsPanelDisappeared();

            Step("10. Verify Failure Tracking panel goes away");
            VerifyEqual("10. Verify Failure Tracking panel goes away", true, !failureTrackingPage.IsDetailsPanelDisplayed());
        }        

        [Test, DynamicRetry]
        [Description("FT_04 Failure displaying on map and tracking panel")]
        public void FT_04()
        {
            var geozone = SLVHelper.GenerateUniqueName("GZNFT04");
            var controller = SLVHelper.GenerateUniqueName("CTRL");
            var streetlight1 = new DeviceModel() { Type = DeviceType.Streetlight, Name = SLVHelper.GenerateUniqueName("STL01"), Latitude = SLVHelper.GenerateCoordinate("10.82890", "10.83042"), Longitude = SLVHelper.GenerateCoordinate("106.72190", "106.72349"), UniqueAddress = SLVHelper.GenerateMACAddress() };
            var streetlight2 = new DeviceModel() { Type = DeviceType.Streetlight, Name = SLVHelper.GenerateUniqueName("STL02"), Latitude = SLVHelper.GenerateCoordinate("10.82890", "10.83042"), Longitude = SLVHelper.GenerateCoordinate("106.72190", "106.72349"), UniqueAddress = SLVHelper.GenerateMACAddress() };
            var streetlight3 = new DeviceModel() { Type = DeviceType.Streetlight, Name = SLVHelper.GenerateUniqueName("STL03"), Latitude = SLVHelper.GenerateCoordinate("10.82890", "10.83042"), Longitude = SLVHelper.GenerateCoordinate("106.72190", "106.72349"), UniqueAddress = SLVHelper.GenerateMACAddress() };

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - Send a simulated failure command for 'Failures Active' streetlight");
            Step("**** Precondition ****\n");

            Step("-> Create data for testing");
            DeleteGeozones("GZNFT04*");
            var eventTime = Settings.GetServerTime();
            CreateNewGeozone(geozone, latMin: "10.82745", latMax: "10.83145", lngMin: "106.71857", lngMax: "106.72716");
            CreateNewController(controller, geozone);
            CreateNewDevice(DeviceType.Streetlight, streetlight1.Name, controller, geozone, lat: streetlight1.Latitude, lng: streetlight1.Longitude);
            CreateNewDevice(DeviceType.Streetlight, streetlight2.Name, controller, geozone, lat: streetlight2.Latitude, lng: streetlight2.Longitude);
            CreateNewDevice(DeviceType.Streetlight, streetlight3.Name, controller, geozone, lat: streetlight3.Latitude, lng: streetlight3.Longitude);
            SetValueToDevice(controller, streetlight1.Name, "MacAddress", streetlight1.UniqueAddress, eventTime);
            SetValueToDevice(controller, streetlight2.Name, "MacAddress", streetlight2.UniqueAddress, eventTime);
            SetValueToDevice(controller, streetlight3.Name, "MacAddress", streetlight3.UniqueAddress, eventTime);
            var requestStatus = SetValueToDevice(controller, streetlight1.Name, "LampFailure", true, eventTime);
            VerifyEqual("-> Verify the request is sent successfully (attribute: LampFailure, value: true)", true, requestStatus);            

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.FailureTracking);

            Step("1. Go to Failure Tracking app");
            Step("2. Expected Failure Tracking page is routed and loaded successfully");
            var failureTrackingPage = desktopPage.GoToApp(App.FailureTracking) as FailureTrackingPage;

            Step("3. Select a device from geozone or map which has activate failure data");
            failureTrackingPage.GeozoneTreeMainPanel.SelectNode(geozone);
            failureTrackingPage.Map.SelectDeviceGL(streetlight1.Longitude, streetlight1.Latitude);
            failureTrackingPage.WaitForDetailsPanelDisplayed();

            Step("4. Verify");
            Step("    o The device in map is displayed in red or orange");
            Step("    o Failure Tracking panel appears with failure list: failure entries. Each entry has: failure level icon, failure name, failure time. Active failures are displayed in green; Inactive failures are displayed in black");
            var selectedDeviceSpriteStatus1 = failureTrackingPage.Map.GetDeviceSpriteStatus(streetlight1.Longitude, streetlight1.Latitude);
            VerifyEqual("4. Verify The device in map is displayed in red or orange", true, IsErrorColor(selectedDeviceSpriteStatus1) || IsWarningColor(selectedDeviceSpriteStatus1));

            var failureList = failureTrackingPage.FailureTrackingDetailsPanel.GetListOfFailures();
            foreach (var failure in failureList)
            {
                VerifyEqual("4. Verify Failure list: Icon", true, failure.Icon != string.Empty);
                VerifyEqual("4. Verify Failure list: Name", true, failure.Name != string.Empty);
                VerifyEqual("4. Verify Failure list: Time", true, failure.Time != string.Empty);
            }

            Step("5. Mouse hover over the device on the map");
            Step("6. Verify a tooltip is displayed with device name");
            var deviceNameInTooltip = failureTrackingPage.Map.MoveAndGetDeviceNameGL(streetlight1.Longitude, streetlight1.Latitude);
            VerifyEqual("6. Verify a tooltip is displayed with device name", streetlight1.Name, deviceNameInTooltip);

            Step("7. Select another device from geozone or map which has no inactive failure data");
            failureTrackingPage.Map.SelectDeviceGL(streetlight2.Longitude, streetlight2.Latitude);
            failureTrackingPage.WaitForDetailsPanelDisplayed();

            Step("8. Verify");
            Step("    o The device in map is dispayed in green");
            Step("    o Failure Tracking panel appears with failure list: failure entries. Each entry has: failure level icon, failure name, failure time. All failures are displayed in black (because they are all inactive)");           
            var selectedDeviceSpriteStatus2 = failureTrackingPage.Map.GetDeviceSpriteStatus(streetlight2.Longitude, streetlight2.Latitude);
            VerifyEqual("8. Verify The device in map is displayed in green", true, IsOkColor(selectedDeviceSpriteStatus2));

            failureList = failureTrackingPage.FailureTrackingDetailsPanel.GetListOfFailures();
            foreach (var failure in failureList)
            {
                VerifyEqual("8. Verify Failure list: Icon", true, failure.Icon != string.Empty);
                VerifyEqual("8. Verify Failure list: Name", true, failure.Name != string.Empty);
                VerifyEqual("8. Verify Failure list: Time", true, failure.Time != string.Empty);
            }

            Step("9. Mouse hover over the device on the map");
            Step("10. Verify a tooltip is displayed with device name");
            deviceNameInTooltip = failureTrackingPage.Map.MoveAndGetDeviceNameGL(streetlight2.Longitude, streetlight2.Latitude);
            VerifyEqual("10. Verify a tooltip is displayed with device name", streetlight2.Name, deviceNameInTooltip);

            Step("11. Select another device from geozone or map which has no failure data");
            failureTrackingPage.Map.SelectDeviceGL(streetlight3.Longitude, streetlight3.Latitude);
            failureTrackingPage.WaitForDetailsPanelDisplayed();

            Step("12. Verify");
            Step("    o The device in map is dispayed in green");
            Step("    o Failure Tracking panel appears with failure list: 'No data'");
            var selectedDeviceSpriteStatus3 = failureTrackingPage.Map.GetDeviceSpriteStatus(streetlight3.Longitude, streetlight3.Latitude);
            VerifyEqual("12. The device in map is dispayed in green", true, IsOkColor(selectedDeviceSpriteStatus3));

            var noDataMsg = failureTrackingPage.FailureTrackingDetailsPanel.GetHistoryFailuresMessageText();
            var isNoDataMsgDisplayed = failureTrackingPage.FailureTrackingDetailsPanel.IsHistoryFailuresMessageDisplayed();
            VerifyEqual("12. Verify No data message", "No data", noDataMsg);
            VerifyEqual("12. Verify No data message displayed", true, isNoDataMsgDisplayed);

            Step("13. Mouse hover over the device on the map");
            Step("14. Verify a tooltip is displayed with device name");
            deviceNameInTooltip = failureTrackingPage.Map.MoveAndGetDeviceNameGL(streetlight3.Longitude, streetlight3.Latitude);
            VerifyEqual("14. Verify a tooltip is displayed with device name", streetlight3.Name, deviceNameInTooltip);

            try
            {
                DeleteGeozone(geozone);
            }
            catch { }
        }
        
        [Test, DynamicRetry]
        [Description("FT_05 Filter on the activated geozone on the map - Uncheck at parent geozone")]
        public void FT_05()
        {
            var testData = GetTestDataOfTestFT_05();
            var geozoneA = testData["GeozoneA"];
            var geozoneB = testData["GeozoneB"];
            var geozoneC = testData["GeozoneC"];

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.FailureTracking);

            Step("1. Go to Failure Tracking app");
            Step("2. Expected Failure Tracking page is routed and loaded successfully");
            var failureTrackingPage = desktopPage.GoToApp(App.FailureTracking) as FailureTrackingPage;

            Step("3. Select geozone A");
            failureTrackingPage.GeozoneTreeMainPanel.SelectNode(geozoneA.Name);

            Step("4. **Verify** All devices of B and C are displayed in map. They are in any color but not GREY");
            failureTrackingPage.Map.MoveToDeviceGL(geozoneB.Devices[0].Longitude, geozoneB.Devices[0].Latitude); //For queryCoords API timeout issue.
            foreach (var device in geozoneB.Devices)
            {
                var spriteStatus = failureTrackingPage.Map.GetDeviceSpriteStatus(device.Longitude, device.Latitude);
                VerifyTrue(string.Format("4. Device {0} of Geozone {1} is not grey(ready)", geozoneB.Name, device.Name), !spriteStatus.Equals("ready"), "!= ready", spriteStatus);
            }
            
            foreach (var device in geozoneC.Devices)
            {
                var spriteStatus = failureTrackingPage.Map.GetDeviceSpriteStatus(device.Longitude, device.Latitude);
                VerifyTrue(string.Format("4. Device {0} of Geozone {1} is not grey(ready)", geozoneC.Name, device.Name), !spriteStatus.Equals("ready"), "!= ready", spriteStatus);
            }

            Step("5. Uncheck 'Filter on the activated geozone on the map' option");
            failureTrackingPage.Map.TickFilterGeozoneCheckbox(false);

            Step("6. **Verify** The same with step #4. The option is remained unchecked");
            foreach (var device in geozoneB.Devices)
            {
                var spriteStatus = failureTrackingPage.Map.GetDeviceSpriteStatus(device.Longitude, device.Latitude);
                VerifyTrue(string.Format("6. Device {0} of Geozone {1} is not grey(ready)", geozoneB.Name, device.Name), !spriteStatus.Equals("ready"), "!= ready", spriteStatus);
            }

            foreach (var device in geozoneC.Devices)
            {
                var spriteStatus = failureTrackingPage.Map.GetDeviceSpriteStatus(device.Longitude, device.Latitude);
                VerifyTrue(string.Format("6. Device {0} of Geozone {1} is not grey(ready)", geozoneC.Name, device.Name), !spriteStatus.Equals("ready"), "!= ready", spriteStatus);
            }

            VerifyEqual("Filtering option is unchecked", false, failureTrackingPage.Map.IsFilterGeozoneCheckboxChecked());

            Step("7. Select geozone B");
            failureTrackingPage.GeozoneTreeMainPanel.SelectNode(geozoneB.Name);

            Step("8. **Verify** The same with step #4. The option is remained unchecked");
            Console.WriteLine("-------------------------");
            foreach (var device in geozoneB.Devices)
            {
                var spriteStatus = failureTrackingPage.Map.GetDeviceSpriteStatus(device.Longitude, device.Latitude);
                VerifyTrue(string.Format("8. Device {0} of Geozone {1} is not grey(ready)", geozoneB.Name, device.Name), !spriteStatus.Equals("ready"), "!= ready", spriteStatus);
            }

            foreach (var device in geozoneC.Devices)
            {
                var spriteStatus = failureTrackingPage.Map.GetDeviceSpriteStatus(device.Longitude, device.Latitude);
                VerifyTrue(string.Format("8. Device {0} of Geozone {1} is not grey(ready)", geozoneC.Name, device.Name), !spriteStatus.Equals("ready"), "!= ready", spriteStatus);
            }

            VerifyEqual("8. Verify Filtering option is unchecked", false, failureTrackingPage.Map.IsFilterGeozoneCheckboxChecked());

            Step("9. Select geozone C");
            failureTrackingPage.GeozoneTreeMainPanel.SelectNode(geozoneC.Name);

            Step("10. **Verify** The same with step #4. The option is remained unchecked");
            foreach (var device in geozoneB.Devices)
            {
                var spriteStatus = failureTrackingPage.Map.GetDeviceSpriteStatus(device.Longitude, device.Latitude);
                VerifyTrue(string.Format("10. Verify Device {0} of Geozone {1} is not grey(ready)", geozoneB.Name, device.Name), !spriteStatus.Equals("ready"), "!= ready", spriteStatus);
            }

            foreach (var device in geozoneC.Devices)
            {
                var spriteStatus = failureTrackingPage.Map.GetDeviceSpriteStatus(device.Longitude, device.Latitude);
                VerifyTrue(string.Format("10. Verify Device {0} of Geozone {1} is not grey(ready)", geozoneC.Name, device.Name), !spriteStatus.Equals("ready"), "!= ready", spriteStatus);
            }

            VerifyEqual("10. Verify Filtering option is unchecked", false, failureTrackingPage.Map.IsFilterGeozoneCheckboxChecked());
        }        
        
        [Test, DynamicRetry]
        [Description("FT_06 Filter on the activated geozone on the map - Check at parent geozone")]
        public void FT_06()
        {
            var testData = GetTestDataOfTestFT_05();
            var geozoneA = testData["GeozoneA"];
            var geozoneB = testData["GeozoneB"];
            var geozoneC = testData["GeozoneC"];

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.FailureTracking);

            Step("1. Go to Failure Tracking app");
            Step("2. Expected Failure Tracking page is routed and loaded successfully");
            var failureTrackingPage = desktopPage.GoToApp(App.FailureTracking) as FailureTrackingPage;

            Step("3. Select geozone A");
            failureTrackingPage.GeozoneTreeMainPanel.SelectNode(geozoneA.Name);

            Step("4. **Verify** All devices of B and C are displayed in map. They are in any color but not GREY");
            failureTrackingPage.Map.MoveToDeviceGL(geozoneB.Devices[0].Longitude, geozoneB.Devices[0].Latitude); //For queryCoords API timeout issue.
            foreach (var device in geozoneB.Devices)
            {
                var spriteStatus = failureTrackingPage.Map.GetDeviceSpriteStatus(device.Longitude, device.Latitude);
                VerifyTrue(string.Format("4. Verify Device {0} of Geozone {1} is not grey(ready)", geozoneB.Name, device.Name), !spriteStatus.Equals("ready"), "!= ready", spriteStatus);
            }

            foreach (var device in geozoneC.Devices)
            {
                var spriteStatus = failureTrackingPage.Map.GetDeviceSpriteStatus(device.Longitude, device.Latitude);
                VerifyTrue(string.Format("4. Verify Device {0} of Geozone {1} is not grey(ready)", geozoneC.Name, device.Name), !spriteStatus.Equals("ready"), "!= ready", spriteStatus);
            }

            Step("5. Check 'Filter on the activated geozone on the map' option");
            failureTrackingPage.Map.TickFilterGeozoneCheckbox();

            Step("6. **Verify** The same with step #4. The option is remained checked");
            foreach (var device in geozoneB.Devices)
            {
                var spriteStatus = failureTrackingPage.Map.GetDeviceSpriteStatus(device.Longitude, device.Latitude);
                VerifyTrue(string.Format("6. Verify Device {0} of Geozone {1} is not grey(ready)", geozoneB.Name, device.Name), !spriteStatus.Equals("ready"), "!= ready", spriteStatus);
            }

            foreach (var device in geozoneC.Devices)
            {
                var spriteStatus = failureTrackingPage.Map.GetDeviceSpriteStatus(device.Longitude, device.Latitude);
                VerifyTrue(string.Format("6. Verify Device {0} of Geozone {1} is not grey(ready)", geozoneC.Name, device.Name), !spriteStatus.Equals("ready"), "!= ready", spriteStatus);
            }

            VerifyEqual("Filtering option is checked", true, failureTrackingPage.Map.IsFilterGeozoneCheckboxChecked());

            Step("7. Select geozone B");
            failureTrackingPage.GeozoneTreeMainPanel.SelectNode(geozoneB.Name);

            Step("8. **Verify** All B's devices are in any color but not GREY, C's are in GREY. The option's icon is remained checked. The option is still checked");
            foreach (var device in geozoneB.Devices)
            {
                var spriteStatus = failureTrackingPage.Map.GetDeviceSpriteStatus(device.Longitude, device.Latitude);
                VerifyTrue(string.Format("8. Verify Device {0} of Geozone {1} is not grey(ready)", geozoneB.Name, device.Name), !spriteStatus.Equals("ready"), "!= ready", spriteStatus);
            }

            foreach (var device in geozoneC.Devices)
            {
                var spriteStatus = failureTrackingPage.Map.GetDeviceSpriteStatus(device.Longitude, device.Latitude);
                VerifyTrue(string.Format("8. Verify Device {0} of Geozone {1} is grey(ready)", geozoneC.Name, device.Name), spriteStatus.Equals("ready"), "ready", spriteStatus);
            }

            VerifyEqual("Filtering option is checked", true, failureTrackingPage.Map.IsFilterGeozoneCheckboxChecked());

            Step("9. Select geozone C");
            failureTrackingPage.GeozoneTreeMainPanel.SelectNode(geozoneC.Name);

            Step("10. **Verify** All C's devices are in any color but not GREY, B's are in GREY. The option's icon is remained checked. The option is still checked");
            foreach (var device in geozoneB.Devices)
            {
                var spriteStatus = failureTrackingPage.Map.GetDeviceSpriteStatus(device.Longitude, device.Latitude);
                VerifyTrue(string.Format("10. Verify Device {0} of Geozone {1} is grey(ready)", geozoneB.Name, device.Name), spriteStatus.Equals("ready"), "!= ready", spriteStatus);
            }

            foreach (var device in geozoneC.Devices)
            {
                var spriteStatus = failureTrackingPage.Map.GetDeviceSpriteStatus(device.Longitude, device.Latitude);
                VerifyTrue(string.Format("10. Verify Device {0} of Geozone {1} is not grey(ready)", geozoneC.Name, device.Name), !spriteStatus.Equals("ready"), "!= ready", spriteStatus);
            }

            VerifyEqual("10. Verify Filtering option is checked", true, failureTrackingPage.Map.IsFilterGeozoneCheckboxChecked());
        }
        
        [Test, DynamicRetry]
        [Description("FT_07 Filter on the activated geozone on the map - Uncheck at child geozone")]
        public void FT_07()
        {
            var testData = GetTestDataOfTestFT_05();
            var geozoneA = testData["GeozoneA"];
            var geozoneB = testData["GeozoneB"];
            var geozoneC = testData["GeozoneC"];

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.FailureTracking);

            Step("1. Go to Failure Tracking app");
            Step("2. Expected Failure Tracking page is routed and loaded successfully");
            var failureTrackingPage = desktopPage.GoToApp(App.FailureTracking) as FailureTrackingPage;
            failureTrackingPage.Map.TickFilterGeozoneCheckbox(false);

            Step("3. Select geozone B");
            failureTrackingPage.GeozoneTreeMainPanel.SelectNode(geozoneA.Name + "\\" + geozoneB.Name);            

            Step("4. Verify 'Filter on the activated geozone on the map' option is unchecked. All devices of B and C are displayed in map. They are in any color but not GREY");            
            failureTrackingPage.Map.MoveToDeviceGL(geozoneB.Devices[0].Longitude, geozoneB.Devices[0].Latitude); //For queryCoords API timeout issue.
            foreach (var device in geozoneB.Devices)
            {
                var spriteStatus = failureTrackingPage.Map.GetDeviceSpriteStatus(device.Longitude, device.Latitude);
                VerifyTrue(string.Format("4. Verify Device {0} of Geozone {1} is not grey(ready)", geozoneB.Name, device.Name), !spriteStatus.Equals("ready"), "!= ready", spriteStatus);
            }

            foreach (var device in geozoneC.Devices)
            {
                var spriteStatus = failureTrackingPage.Map.GetDeviceSpriteStatus(device.Longitude, device.Latitude);
                VerifyTrue(string.Format("4. Verify Device {0} of Geozone {1} is not grey(ready)", geozoneC.Name, device.Name), !spriteStatus.Equals("ready"), "!= ready", spriteStatus);
            }

            VerifyEqual("Filtering option is unchecked", false, failureTrackingPage.Map.IsFilterGeozoneCheckboxChecked());

            Step("5. Check 'Filter on the activated geozone on the map' option");
            failureTrackingPage.Map.TickFilterGeozoneCheckbox();

            Step("6. Verify All B's devices are in any color but not GREY, C's are in GREY. The option's icon is remained checked");
            foreach (var device in geozoneB.Devices)
            {
                var spriteStatus = failureTrackingPage.Map.GetDeviceSpriteStatus(device.Longitude, device.Latitude);
                VerifyTrue(string.Format("6. Verify Device '{0}' of Geozone '{1}' is not grey(ready)", device.Name, geozoneB.Name), !IsGreyColor(spriteStatus), "!= ready", spriteStatus);
            }

            foreach (var device in geozoneC.Devices)
            { 
                var spriteStatus = failureTrackingPage.Map.GetDeviceSpriteStatus(device.Longitude, device.Latitude);
                VerifyTrue(string.Format("6. Verify Device '{0}' of Geozone '{1}' is grey(ready)", device.Name, geozoneC.Name), IsGreyColor(spriteStatus), "ready", spriteStatus);
            }

            VerifyEqual("Filtering option is checked", true, failureTrackingPage.Map.IsFilterGeozoneCheckboxChecked());

            Step("7. Uncheck the option");
            failureTrackingPage.Map.TickFilterGeozoneCheckbox(false);

            Step("8. Verify All devices of B and C are displayed in map. They are in any color but not GREY. The option is remained unchecked");
            foreach (var device in geozoneB.Devices)
            {
                var spriteStatus = failureTrackingPage.Map.GetDeviceSpriteStatus(device.Longitude, device.Latitude);
                VerifyTrue(string.Format("8. Verify Device '{0}' of Geozone '{1} is not grey(ready)", geozoneB.Name, device.Name), !IsGreyColor(spriteStatus), "!= ready", spriteStatus);
            }

            foreach (var device in geozoneC.Devices)
            {
                var spriteStatus = failureTrackingPage.Map.GetDeviceSpriteStatus(device.Longitude, device.Latitude);
                VerifyTrue(string.Format("8. Verify Device '{0}' of Geozone '{1}' is not grey(ready)", device.Name, geozoneC.Name), !IsGreyColor(spriteStatus), "!= ready", spriteStatus);
            }

            VerifyEqual("Filtering option is unchecked", false, failureTrackingPage.Map.IsFilterGeozoneCheckboxChecked());

            Step("9. Select geozone C");
            failureTrackingPage.GeozoneTreeMainPanel.SelectNode(geozoneC.Name);

            Step("10. Verify All devices of B and C are displayed in map. They are in any color but not GREY. The option is remained unchecked"); 
            foreach (var device in geozoneB.Devices)
            {
                var spriteStatus = failureTrackingPage.Map.GetDeviceSpriteStatus(device.Longitude, device.Latitude);
                VerifyTrue(string.Format("10. Verify Device '{0}' of Geozone '{1}' is not grey(ready)", device.Name, geozoneB.Name), !IsGreyColor(spriteStatus), "!= ready", spriteStatus);
            }

            foreach (var device in geozoneC.Devices)
            {
                var spriteStatus = failureTrackingPage.Map.GetDeviceSpriteStatus(device.Longitude, device.Latitude);
                VerifyTrue(string.Format("10. Verify Device '{0}' of Geozone '{1}' is not grey(ready)", device.Name, geozoneC.Name), !IsGreyColor(spriteStatus), "!= ready", spriteStatus);
            }

            VerifyEqual("Filtering option is unchecked", false, failureTrackingPage.Map.IsFilterGeozoneCheckboxChecked());

            Step("11. Select geozone A");
            failureTrackingPage.GeozoneTreeMainPanel.SelectNode(geozoneA.Name.GetChildName());

            Step("12. Verify All devices of B and C are displayed in map. They are in any color but not GREY. The option is remained unchecked");
            foreach (var device in geozoneB.Devices)
            {
                var spriteStatus = failureTrackingPage.Map.GetDeviceSpriteStatus(device.Longitude, device.Latitude);
                VerifyTrue(string.Format("12. Verify Device '{0}' of Geozone '{1}' is not grey(ready)", device.Name, geozoneB.Name), !IsGreyColor(spriteStatus), "!= ready", spriteStatus);
            }

            foreach (var device in geozoneC.Devices)
            {
                var spriteStatus = failureTrackingPage.Map.GetDeviceSpriteStatus(device.Longitude, device.Latitude);
                VerifyTrue(string.Format("12. Verify Device '{0}' of Geozone '{1}' is not grey(ready)", device.Name, geozoneC.Name), !IsGreyColor(spriteStatus), "!= ready", spriteStatus);
            }

            VerifyEqual("12. Verify Filtering option is unchecked", false, failureTrackingPage.Map.IsFilterGeozoneCheckboxChecked());
        }
        
        [Test, DynamicRetry]
        [Description("FT_08 Filter on the activated geozone on the map - Check at child geozone")]
        public void FT_08()
        {
            var testData = GetTestDataOfTestFT_05();
            var geozoneA = testData["GeozoneA"];
            var geozoneB = testData["GeozoneB"];
            var geozoneC = testData["GeozoneC"];

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.FailureTracking);

            Step("1. Go to Failure Tracking app");
            Step("2. Expected Failure Tracking page is routed and loaded successfully");
            var failureTrackingPage = desktopPage.GoToApp(App.FailureTracking) as FailureTrackingPage;
            failureTrackingPage.Map.TickFilterGeozoneCheckbox(false);

            Step("3. Select geozone B");
            failureTrackingPage.GeozoneTreeMainPanel.SelectNode(geozoneA.Name + "\\" + geozoneB.Name);            

            Step("4. Verify 'Filter on the activated geozone on the map' option is unchecked. All devices of B and C are displayed in map. They are in any color but not GREY");
            failureTrackingPage.Map.MoveToDeviceGL(geozoneB.Devices[0].Longitude, geozoneB.Devices[0].Latitude); //For queryCoords API timeout issue.
            foreach (var device in geozoneB.Devices)
            {
                var spriteStatus = failureTrackingPage.Map.GetDeviceSpriteStatus(device.Longitude, device.Latitude);
                VerifyTrue(string.Format("4. Verify Device '{0}' of Geozone '{1}' is not grey(ready)", device.Name, geozoneB.Name), !IsGreyColor(spriteStatus), "!= ready", spriteStatus);
            }

            foreach (var device in geozoneC.Devices)
            {
                var spriteStatus = failureTrackingPage.Map.GetDeviceSpriteStatus(device.Longitude, device.Latitude);
                VerifyTrue(string.Format("4. Verify Device '{0}' of Geozone '{1}' is not grey(ready)", device.Name, geozoneC.Name), !IsGreyColor(spriteStatus), "!= ready", spriteStatus);
            }

            VerifyEqual("Filtering option is unchecked", false, failureTrackingPage.Map.IsFilterGeozoneCheckboxChecked());

            Step("5. Check 'Filter on the activated geozone on the map' option");
            failureTrackingPage.Map.TickFilterGeozoneCheckbox();

            Step("6. Verify All B's devices are in any color but not GREY, C's are in GREY. The option's icon is remained checked");
            foreach (var device in geozoneB.Devices)
            {
                var spriteStatus = failureTrackingPage.Map.GetDeviceSpriteStatus(device.Longitude, device.Latitude);
                VerifyTrue(string.Format("6. Verify Device '{0}' of Geozone '{1}' is not grey(ready)", device.Name, geozoneB.Name), !IsGreyColor(spriteStatus), "!= ready", spriteStatus);
            }

            foreach (var device in geozoneC.Devices)
            {
                var spriteStatus = failureTrackingPage.Map.GetDeviceSpriteStatus(device.Longitude, device.Latitude);
                VerifyTrue(string.Format("6. Verify Device '{0}' of Geozone '{1}' is grey(ready)", device.Name, geozoneC.Name), IsGreyColor(spriteStatus), "!= ready", spriteStatus);
            }
            VerifyEqual("6. Verify Filtering option is checked", true, failureTrackingPage.Map.IsFilterGeozoneCheckboxChecked());

            Step("7. Check the option");
            failureTrackingPage.Map.TickFilterGeozoneCheckbox();

            Step("8. **Verify** All B's devices are in any color but not GREY, C's are in GREY. The option's icon is remained checked. The option is still checked");
            foreach (var device in geozoneB.Devices)
            {
                var spriteStatus = failureTrackingPage.Map.GetDeviceSpriteStatus(device.Longitude, device.Latitude);
                VerifyTrue(string.Format("8. Verify Device '{0}' of Geozone '{1}' is not grey(ready)", device.Name, geozoneB.Name), !IsGreyColor(spriteStatus), "!= ready", spriteStatus);
            }

            foreach (var device in geozoneC.Devices)
            {
                var spriteStatus = failureTrackingPage.Map.GetDeviceSpriteStatus(device.Longitude, device.Latitude);
                VerifyTrue(string.Format("8. Verify Device '{0}' of Geozone '{1}' is grey(ready)", device.Name, geozoneC.Name), IsGreyColor(spriteStatus), "ready", spriteStatus);
            }

            VerifyEqual("8. Verify Filtering option is checked", true, failureTrackingPage.Map.IsFilterGeozoneCheckboxChecked());

            Step("9. Select geozone C");
            failureTrackingPage.GeozoneTreeMainPanel.SelectNode(geozoneC.Name);

            Step("10. **Verify** All C's devices are in any color but not GREY, B's are in GREY. The option's icon is remained checked. The option is still checked");
            foreach (var device in geozoneB.Devices)
            {
                var spriteStatus = failureTrackingPage.Map.GetDeviceSpriteStatus(device.Longitude, device.Latitude);
                VerifyTrue(string.Format("12. Verify Device '{0}' of Geozone '{1}' is grey(ready)", device.Name, geozoneB.Name), IsGreyColor(spriteStatus), "ready", spriteStatus);
            }

            foreach (var device in geozoneC.Devices)
            {
                var spriteStatus = failureTrackingPage.Map.GetDeviceSpriteStatus(device.Longitude, device.Latitude);
                VerifyTrue(string.Format("12. Verify Device '{0}' of Geozone '{1}' is not grey(ready)", device.Name, geozoneC.Name), !IsGreyColor(spriteStatus), "!= ready", spriteStatus);
            }

            VerifyEqual("12. Verify Filtering option is checked", true, failureTrackingPage.Map.IsFilterGeozoneCheckboxChecked());

            Step("11. Select geozone A");
            failureTrackingPage.GeozoneTreeMainPanel.SelectNode(geozoneA.Name.GetChildName());

            Step("12. **Verify** All devices of B and C are displayed in map. They are in any color but not GREY. The option's icon is remained checked. The option is still checked");
            foreach (var device in geozoneB.Devices)
            {
                var spriteStatus = failureTrackingPage.Map.GetDeviceSpriteStatus(device.Longitude, device.Latitude);
                VerifyTrue(string.Format("12. Verify Device '{0}' of Geozone '{1}' is not grey(ready)", device.Name, geozoneB.Name), !IsGreyColor(spriteStatus), "!= ready", spriteStatus);
            }

            foreach (var device in geozoneC.Devices)
            {
                var spriteStatus = failureTrackingPage.Map.GetDeviceSpriteStatus(device.Longitude, device.Latitude);
                VerifyTrue(string.Format("12. Verify Device '{0}' of Geozone '{1}' is not grey(ready)", device.Name, geozoneC.Name), !IsGreyColor(spriteStatus), "!= ready", spriteStatus);
            }

            VerifyEqual("12. Verify Filtering option is checked", true, failureTrackingPage.Map.IsFilterGeozoneCheckboxChecked());

        }

        [Test, DynamicRetry]
        [Description("FT_09 Communication Failure should be a critical failure")]
        [NonParallelizable]
        public void FT_09()
        {
            var testData = GetTestDataOfTestFT_09();
            var olsonTimeZoneId = testData["OlsonTimeZoneId"];
            var geozone = SLVHelper.GenerateUniqueName("GZNFT09");
            var controller = SLVHelper.GenerateUniqueName("CLFT09");
            var streetlight = SLVHelper.GenerateUniqueName("SLFT09");
            var latitude = SLVHelper.GenerateCoordinate("13.48624", "13.50138");
            var longitude = SLVHelper.GenerateCoordinate("106.33936", "106.35051");

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");         
            Step("**** Precondition ****\n");

            Step("1. Create a new streetlight");
            DeleteGeozones("GZNFT09*");
            CreateNewGeozone(geozone, latMin: "13.46837", latMax: "13.50885", lngMin: "106.30129", lngMax: "106.38884");
            CreateNewController(controller, geozone);
            SetValueToController(controller, "TimeZoneId", olsonTimeZoneId, Settings.GetServerTime());
            CreateNewDevice(DeviceType.Streetlight, streetlight, controller, geozone, lat: latitude, lng: longitude);
            SetValueToDevice(controller, streetlight, "MacAddress", SLVHelper.GenerateMACAddress(), Settings.GetServerTime());

            Step("2. Simulate the Communication Failure for the testing streetlight by sending the command with the parameter 'DefaultLostNode'");
            Step("3. Verify The command sent successfully");
            var currentDateTime = Settings.GetCurrentControlerDateTime(controller).AddHours(10);
            var request = SetValueToDevice(controller, streetlight, "DefaultLostNode", "1", currentDateTime);
            VerifyEqual(string.Format("3. Verify the request is sent successfully (attribute: {0}, value: {1})", "DefaultLostNode", "1"), true, request);

            Step("4. Go to Failure Tracking app, and select the testing streetlight");
            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.FailureTracking);
            var failureTrackingPage = desktopPage.GoToApp(App.FailureTracking) as FailureTrackingPage;            
            failureTrackingPage.GeozoneTreeMainPanel.SelectNode(geozone + @"\"+ streetlight);
            failureTrackingPage.WaitForDetailsPanelDisplayed();

            Step("5. Verify The device shows in the map in RED");
            failureTrackingPage.Map.MoveToDeviceGL(longitude, latitude); //For queryCoords API timeout issue.
            var selectedDeviceSpriteStatus = failureTrackingPage.Map.GetDeviceSpriteStatus(longitude, latitude);
            VerifyTrue("[#1429535] 5. Verify The device shows in the map in RED", IsErrorColor(selectedDeviceSpriteStatus), "error", selectedDeviceSpriteStatus);

            Step("6. Verify On the Failure Tracking panel, the Communication Failure displays");
            Step(" - Error icon");
            Step(" - 'Communication Failure'");
            Step(" - Date and time the failure appears (the sending time of command): m/d/yyyy HH:mm");

            var failure = failureTrackingPage.FailureTrackingDetailsPanel.GetListOfFailures().FirstOrDefault();
            if (failure != null)
            {
                VerifyEqual("6. Verify Communication Failure displays: Error icon", "icon-error", failure.Icon);
                VerifyEqual("6. Verify Communication Failure displays: Communication failure", "Communication failure", failure.Name);
                VerifyEqual("6. Verify Communication Failure displays: Date and time the failure appears (the sending time of command): m/d/yyyy HH:mm", currentDateTime.ToString("M/d/yyyy HH:mm"), failure.Time);
            }
            else
            {
                Warning("6. This is no failure found!");
            }

            try
            {
                DeleteGeozone(geozone);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("SC-782 In Failure Tracking, clusters that don't have devices from this geozone should be represented in white when enabling the filter on a geozone")]
        public void FT_10()
        {
            var geozone = SLVHelper.GenerateUniqueName("GZNFT10");
            var geozone1 = SLVHelper.GenerateUniqueName("GZNFT1001");
            var geozone2 = SLVHelper.GenerateUniqueName("GZNFT1002");
            var controller = SLVHelper.GenerateUniqueName("CTRL");
            var cluster1Lat = SLVHelper.GenerateCoordinate("9.36353", "9.37373");
            var cluster1Lng = SLVHelper.GenerateCoordinate("104.84192", "104.85412");
            var cluster1Streetlight1 = new DeviceModel() { Type = DeviceType.Streetlight, Name = SLVHelper.GenerateUniqueName("STL0101"), Latitude = cluster1Lat, Longitude = cluster1Lng, UniqueAddress = SLVHelper.GenerateMACAddress() };
            var cluster1Streetlight2 = new DeviceModel() { Type = DeviceType.Streetlight, Name = SLVHelper.GenerateUniqueName("STL0102"), Latitude = cluster1Lat, Longitude = cluster1Lng, UniqueAddress = SLVHelper.GenerateMACAddress() };
            var cluster1Devices = new List<DeviceModel> { cluster1Streetlight1, cluster1Streetlight2 };
            var cluster2Lat = SLVHelper.GenerateCoordinate("9.36353", "9.37373");
            var cluster2Lng = SLVHelper.GenerateCoordinate("104.84192", "104.85412");
            var cluster2Streetlight1 = new DeviceModel() { Type = DeviceType.Streetlight, Name = SLVHelper.GenerateUniqueName("STL0201"), Latitude = cluster2Lat, Longitude = cluster2Lng, UniqueAddress = SLVHelper.GenerateMACAddress() };
            var cluster2Streetlight2 = new DeviceModel() { Type = DeviceType.Streetlight, Name = SLVHelper.GenerateUniqueName("STL0202"), Latitude = cluster2Lat, Longitude = cluster2Lng, UniqueAddress = SLVHelper.GenerateMACAddress() };          
            var cluster2Devices = new List<DeviceModel> { cluster2Streetlight1, cluster2Streetlight2 };

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - Create 2 geozone covering the same zone on the map and each geozone contains a clusters of 2 streetlights sharing the same location.");            
            Step("**** Precondition ****\n");

            Step("-> Create data for testing");
            DeleteGeozones("GZNFT10*");
            var eventTime = Settings.GetServerTime();
            CreateNewGeozone(geozone, latMin: "9.27278", latMax: "9.42925", lngMin: "104.64561", lngMax: "104.97511");
            CreateNewGeozone(geozone1, geozone, latMin: "9.35260", latMax: "9.38101", lngMin: "104.81235", lngMax: "104.87219");
            CreateNewGeozone(geozone2, geozone, latMin: "9.35260", latMax: "9.38101", lngMin: "104.81235", lngMax: "104.87219");
            CreateNewController(controller, geozone);
            CreateNewDevice(DeviceType.Streetlight, cluster1Streetlight1.Name, controller, geozone1, lat: cluster1Streetlight1.Latitude, lng: cluster1Streetlight1.Longitude);
            CreateNewDevice(DeviceType.Streetlight, cluster1Streetlight2.Name, controller, geozone1, lat: cluster1Streetlight2.Latitude, lng: cluster1Streetlight2.Longitude);
            CreateNewDevice(DeviceType.Streetlight, cluster2Streetlight1.Name, controller, geozone2, lat: cluster2Streetlight1.Latitude, lng: cluster2Streetlight1.Longitude);
            CreateNewDevice(DeviceType.Streetlight, cluster2Streetlight2.Name, controller, geozone2, lat: cluster2Streetlight2.Latitude, lng: cluster2Streetlight2.Longitude);
            SetValueToDevice(controller, cluster1Streetlight1.Name, "MacAddress", cluster1Streetlight1.UniqueAddress, eventTime);
            SetValueToDevice(controller, cluster1Streetlight2.Name, "MacAddress", cluster1Streetlight2.UniqueAddress, eventTime);
            SetValueToDevice(controller, cluster2Streetlight1.Name, "MacAddress", cluster2Streetlight1.UniqueAddress, eventTime);
            SetValueToDevice(controller, cluster2Streetlight2.Name, "MacAddress", cluster2Streetlight2.UniqueAddress, eventTime);

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.FailureTracking);

            Step("1. Go to Failure Tracking App and select the one of a testing geozone");
            var failureTrackingPage = desktopPage.GoToApp(App.FailureTracking) as FailureTrackingPage;
            failureTrackingPage.GeozoneTreeMainPanel.SelectNode(geozone + @"\" + geozone1);

            Step("2. Hover the Global icon on the top right corner of the screen and deselect the checkbox if it's checked");
            failureTrackingPage.Map.TickFilterGeozoneCheckbox(false);            

            Step("3. Verify 2 testing clusters of devices are GREEN on the map");
            Step("4. Hover a cluster");
            failureTrackingPage.Map.MoveToDeviceGL(cluster1Devices[0].Longitude, cluster1Devices[0].Latitude); //For queryCoords API timeout issue.
            foreach (var device in cluster1Devices)
            {
                var cluster = failureTrackingPage.Map.GetClusterSprite(device.Longitude, device.Latitude);
                VerifyTrue(string.Format("3. Device {0} of Geozone {1} is green(ok)", geozone1, device.Name), IsOkColor(cluster.Status), "ok", cluster.Status);
            }

            foreach (var device in cluster2Devices)
            {
                var cluster = failureTrackingPage.Map.GetClusterSprite(device.Longitude, device.Latitude);
                VerifyTrue(string.Format("3. Device {0} of Geozone {1} is green(ok)", geozone2, device.Name), IsOkColor(cluster.Status), "ok", cluster.Status);
            }

            Step("5. Verify The tooltip displays with 2 rows");
            Step(" + 'Device Cluster'");
            Step(" + '2 devices'");
            var name = failureTrackingPage.Map.GetDeviceNameGL();
            var devicesCount = failureTrackingPage.Map.GetTooltipDevicesCountGL();
            VerifyEqual("5. Verify The tooltip displays 'Device Cluster'", "Device Cluster", name);
            VerifyEqual("5. Verify The tooltip displays '2 devices'", "2 devices", devicesCount);

            Step("6. Select a device in the geozone");
            var rndDevice = cluster1Devices.PickRandom();
            failureTrackingPage.GeozoneTreeMainPanel.SelectNode(rndDevice.Name);

            Step("7. Verify The cluster containing the selected device has the back circle. The other cluster has no back circle.");
            var cluster1 = failureTrackingPage.Map.GetClusterSprite(rndDevice.Longitude, rndDevice.Latitude);
            var cluster2 = failureTrackingPage.Map.GetClusterSprite(cluster2Devices[0].Longitude, cluster2Devices[0].Latitude);

            VerifyEqual("7. Verify The cluster containing the selected device has the back circle", true, cluster1.IsSelected);
            VerifyEqual("7. Verify The other cluster has no back circle", false, cluster2.IsSelected);

            Step("8. Hover the Global icon on the top right corner of the screen and check the checkbox");
            failureTrackingPage.GeozoneTreeMainPanel.SelectNode(geozone1);
            failureTrackingPage.Map.TickFilterGeozoneCheckbox(true);

            Step("9. Verify The cluster belonging to the selected geozone is still GREEN, the other is GRAY with black numberic text");
            foreach (var device in cluster1Devices)
            {
                var cluster = failureTrackingPage.Map.GetClusterSprite(device.Longitude, device.Latitude);
                VerifyTrue(string.Format("9. Device {0} of Geozone {1} is GREEN(ok)", geozone1, device.Name), IsOkColor(cluster.Status), "ok", cluster.Status);
            }

            foreach (var device in cluster2Devices)
            {
                var cluster = failureTrackingPage.Map.GetClusterSprite(device.Longitude, device.Latitude);
                VerifyTrue(string.Format("9. Device {0} of Geozone {1} is GRAY(ready)", geozone2, device.Name), IsGreyColor(cluster.Status), "ready", cluster.Status);
            }

            Step("10. Select another geozone");
            failureTrackingPage.GeozoneTreeMainPanel.SelectNode(geozone2);

            Step("11. Verify The cluster belonging to the selected geozone is still GREEN, the other is GRAY with black numberic text");
            foreach (var device in cluster1Devices)
            {
                var cluster = failureTrackingPage.Map.GetClusterSprite(device.Longitude, device.Latitude);
                VerifyTrue(string.Format("11. Device {0} of Geozone {1} is GRAY(ready)", geozone1, device.Name), IsGreyColor(cluster.Status), "ready", cluster.Status);
            }

            foreach (var device in cluster2Devices)
            {
                var cluster = failureTrackingPage.Map.GetClusterSprite(device.Longitude, device.Latitude);
                VerifyTrue(string.Format("11. Device {0} of Geozone {1} is GREEN(ok)", geozone2, device.Name), IsOkColor(cluster.Status), "ok", cluster.Status);
            }

            try
            {
                DeleteGeozone(geozone);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("FT_11 SC-782 the marker should use the warning colour when going to Failure Tracking having at least one device having warning but no failure")]
        public void FT_11()
        {
            var csvFilePath = Settings.GetFullPath(Settings.CSV_FILE_PATH + "FT11.csv");
            var geozone = SLVHelper.GenerateUniqueName("GZNFT11");
            var controller = SLVHelper.GenerateUniqueName("CTRL");
            var streetlight1 = SLVHelper.GenerateUniqueName("STL01");
            var streetlight2 = SLVHelper.GenerateUniqueName("STL02");
            var fullGeozonePath = Settings.RootGeozoneName + @"/" + geozone;            
            var clusterLatitude = SLVHelper.GenerateCoordinate("8.62532", "8.62608"); 
            var clusterLongitude = SLVHelper.GenerateCoordinate("104.71272", "104.71364");
            var typeOfEquipment = "ABEL-Vigilon A[Dimmable ballast]";

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - Prepare 1 csv files to import 2 streetlights sharing the same location in a geozone");
            Step("**** Precondition ****\n");

            Step("-> Create data for testing");
            DeleteGeozones("GZNFT11*");
            CreateNewGeozone(geozone, latMin: "8.62439", latMax: "8.62684", lngMin: "104.71052", lngMax: "104.71567");
            CreateNewController(controller, geozone);
            CreateCsvDevices(csvFilePath, fullGeozonePath, new List<DeviceModel>
            {
                new DeviceModel{ Type = DeviceType.Streetlight, Id = streetlight1, Name = streetlight1, Controller = controller, TypeOfEquipment = typeOfEquipment, Latitude = clusterLatitude, Longitude = clusterLongitude, UniqueAddress = SLVHelper.GenerateMACAddress() },
                new DeviceModel{ Type = DeviceType.Streetlight, Id = streetlight2, Name = streetlight2, Controller = controller, TypeOfEquipment = typeOfEquipment, Latitude = clusterLatitude, Longitude = clusterLongitude, UniqueAddress = SLVHelper.GenerateMACAddress() }
            });
            ImportFile(csvFilePath);

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.FailureTracking);

            Step("1. Go to Failure Tracking App and select the testing geozone");
            var failureTrackingPage = desktopPage.GoToApp(App.FailureTracking) as FailureTrackingPage;
            failureTrackingPage.GeozoneTreeMainPanel.SelectNode(geozone);
            
            Step("2. Verify The is only a marker displays with number: '2'");
            Step(" - The backround color is set the OK color Green and the numberic text's color is White");
            failureTrackingPage.Map.MoveToDeviceGL(clusterLongitude, clusterLatitude);
            var cluster = failureTrackingPage.Map.GetClusterSprite(clusterLongitude, clusterLatitude);
            VerifyEqual("2. Verify The is only a marker displays with number: '2'", "2", cluster.DeviceCount);
            VerifyEqual("2. Verify The backround color is set the OK color Green and the numberic text's color is White", true, IsOkColor(cluster.Status));

            Step("3. Send the command to simulate a warning of HighVoltage for streetlight 01 (valueName=HighVoltage &value=true)");
            Step("4. Verify The command sent successfully");
            var currentDateTime = Settings.GetServerTime();
            var request = SetValueToDevice(controller, streetlight1, "HighVoltage", true, currentDateTime);
            VerifyEqual("4. Verify The command sent successfully", true, request);

            Step("5. Refresh the page and go back to Failure Tracking App and select the testing geozone");
            desktopPage = Browser.RefreshLoggedInCMS();
            failureTrackingPage = desktopPage.GoToApp(App.FailureTracking) as FailureTrackingPage;
            failureTrackingPage.GeozoneTreeMainPanel.SelectNode(geozone);

            Step("6. Verify The is only a marker displays with number: '2'");
            Step(" - The background color is set the warning color Yellow and the numberic text's color is White");
            failureTrackingPage.Map.MoveToDeviceGL(clusterLongitude, clusterLatitude);
            cluster = failureTrackingPage.Map.GetClusterSprite(clusterLongitude, clusterLatitude);
            VerifyEqual("6. Verify The is only a marker displays with number: '2'", "2", cluster.DeviceCount);
            VerifyEqual("6. Verify The background color is set the warning color Yellow and the numberic text's color is White", true, IsWarningColor(cluster.Status));
            
            Step("7. Hover the maker");
            failureTrackingPage.Map.MoveToDeviceGL(clusterLongitude, clusterLatitude);

            Step("8. Verify The tooltip displays with 2 rows");
            Step(" - 'Device Cluster'");
            Step(" - '{#} devices'. Ex: 2 devices");
            var clusterName = failureTrackingPage.Map.GetDeviceNameGL();
            var deviceCount = failureTrackingPage.Map.GetTooltipDevicesCountGL();
            VerifyEqual("8. Verify A tooltip displays: Device Cluster", "Device Cluster", clusterName);
            VerifyEqual("8. Verify A tooltip displays: {#} devices", "2 devices", deviceCount);

            Step("9. Click on the marker");
            failureTrackingPage.Map.SelectDeviceGL(clusterLongitude, clusterLatitude);
            failureTrackingPage.Map.WaitForDeviceClusterPopupPanelDisplayed();

            Step("10. Verify The table pop-up displays with");
            Step(" - Tittle: Cluster with {#} devices");
            Step(" - 'Show/Hide columns' button");
            VerifyEqual("10. Verify Title: 'Cluster with {#} devices", "Cluster with 2 devices", failureTrackingPage.Map.DeviceClusterPopupPanel.GetPanelTitleText());
            VerifyEqual("10. Verify 'Show/Hide columns' button displayed", true, failureTrackingPage.Map.DeviceClusterPopupPanel.IsShowHideColumnsButtonDisplayed());

            Step("11. Click 'Show/Hide columns' button");
            Step("12. Verify The pop-up displays with the list of device's properties:");
            Step(" - Type");
            Step(" - Name");
            Step(" - Identifier");
            Step(" - Unique Address");
            Step(" - Wattage");
            Step(" - Address1");
            Step(" - Address2");
            Step(" - Luminaire Type");            
            var expectedAllDevicePropertiesList = new List<string>() { "Type", "Name", "Identifier", "Unique Address", "Wattage", "Address1", "Address2", "Luminaire Type" };
            var actualAllDevicePropertiesList = failureTrackingPage.Map.DeviceClusterPopupPanel.GetAllColumnsInShowHideColumnsMenu();
            VerifyEqual("12. Verify The pop-up displays with the list of device's properties as expected", expectedAllDevicePropertiesList, actualAllDevicePropertiesList);
            
            Step("13. Check on the checkboxes to select all properties");
            failureTrackingPage.Map.DeviceClusterPopupPanel.CheckAllColumnsInShowHideColumnsMenu();

            Step("14. Verify All checkboxes are checked");
            var actualCheckedDevicePropertiesList = failureTrackingPage.Map.DeviceClusterPopupPanel.GetAllCheckedColumnsInShowHideColumnsMenu();
            VerifyEqual("14. Verify All checkboxes are checked", expectedAllDevicePropertiesList, actualCheckedDevicePropertiesList);

            Step("15. Click 'Show/Hide columns' again");
            Step("16. Scroll the horizontal scrollbar");
            Step("17. Verify Selected properties are added as columns in the grid with the Checkbox column at the 1st position");
            var listOfColumnsHeader = failureTrackingPage.Map.DeviceClusterPopupPanel.GetListOfColumnsHeader();
            VerifyEqual("17. Verify Selected properties are added as columns in the grid with the Checkbox column at the 1st position", expectedAllDevicePropertiesList, listOfColumnsHeader);
            failureTrackingPage.Map.DeviceClusterPopupPanel.HideShowHideColumnsMenu();

            Step("18. Click a row of the streetlight having warning on the grid");
            failureTrackingPage.Map.DeviceClusterPopupPanel.TickGridColumn(streetlight1, true);
            failureTrackingPage.WaitForPreviousActionComplete();

            Step("19. Verify its checkbox is checked");
            Step(" - A Failure Tracking panel displays on the left screen with");
            Step("  + The name of the testing streetlight");
            Step("  + The warning message with yellow Caution icon; 'High voltage'; datetime formatted M/d/yyyy HH:mm (the time sending the simulated command)");
            VerifyEqual("19. Verify its checkbox is checked", true, failureTrackingPage.Map.DeviceClusterPopupPanel.GetCheckBoxGridColumnValue(streetlight1));
            VerifyEqual("19. Verify A Failure Tracking panel displays on the left screen", true, failureTrackingPage.FailureTrackingDetailsPanel.IsPanelVisible());
            VerifyEqual("19. Verify The name of the testing streetlight", streetlight1, failureTrackingPage.FailureTrackingDetailsPanel.GetDeviceNameValueText());
            var failure = failureTrackingPage.FailureTrackingDetailsPanel.GetListOfFailures().FirstOrDefault();
            if (failure != null)
            {
                VerifyEqual("19. Verify Communication Failure displays: Warning icon", "icon-warning", failure.Icon);
                VerifyEqual("19. Verify Communication Failure displays: High voltage", "High voltage", failure.Name);
                VerifyEqual("19. Verify Communication Failure displays: datetime formatted M/d/yyyy HH:mm (the time sending the simulated command)", currentDateTime.ToString("M/d/yyyy HH:mm"), failure.Time);
            }
            else
            {
                Warning("19. This is no failure found!");
            }

            Step("20. Click that row again");
            failureTrackingPage.Map.DeviceClusterPopupPanel.TickGridColumn(streetlight1, false);
            failureTrackingPage.WaitForPreviousActionComplete();

            Step("21. Verify its checkbox is unchecked");
            Step(" - The Failure Tracking panel is closed");
            VerifyEqual("21. Verify its checkbox is unchecked", false, failureTrackingPage.Map.DeviceClusterPopupPanel.GetCheckBoxGridColumnValue(streetlight1));
            VerifyEqual("21. Verify The Failure Tracking panel is closed", false, failureTrackingPage.FailureTrackingDetailsPanel.IsPanelVisible());

            Step("22. Check the color of device icon in Type column");
            Step("23. Verify");
            Step(" - The streetlight has no warning displays with a Green circle containing a White bulb icon");
            Step(" - The streetlight has a warning displays with a Orange circle containing a White bulb icon");
            var listOfIconType = failureTrackingPage.Map.DeviceClusterPopupPanel.GetListOfIconType();
            VerifyEqual("23. Verify The streetlight has no warning displays with a Green circle containing a White bulb icon", true, listOfIconType.Exists(p => p.Contains("streetlight-ok.png")));
            VerifyEqual("23. The streetlight has a warning displays with a Orange circle containing a White bulb icon", true, listOfIconType.Exists(p => p.Contains("streetlight-warning.png")));

            Step("24. Press 'X' button");
            failureTrackingPage.Map.DeviceClusterPopupPanel.ClickCloseButton();

            Step("25. Verify The pop-up is closed");
            VerifyEqual("25. Verify The pop-up is closed", false, failureTrackingPage.Map.IsDeviceClusterPopupPanelDisplayed());

            try
            {
                DeleteGeozone(geozone);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("FT_12 SC-782 the marker should use the failure colour when going to Failure Tracking having at least one device having a failure")]
        public void FT_12()
        {
            var csvFilePath = Settings.GetFullPath(Settings.CSV_FILE_PATH + "FT12.csv");
            var geozone = SLVHelper.GenerateUniqueName("GZNFT12");
            var controller = SLVHelper.GenerateUniqueName("CTRL");
            var streetlight1 = SLVHelper.GenerateUniqueName("STL01");
            var streetlight2 = SLVHelper.GenerateUniqueName("STL02");
            var fullGeozonePath = Settings.RootGeozoneName + @"/" + geozone;
            var clusterLatitude = SLVHelper.GenerateCoordinate("8.72452", "8.72584");
            var clusterLongitude = SLVHelper.GenerateCoordinate("105.17559", "105.17707");
            var typeOfEquipment = "ABEL-Vigilon A[Dimmable ballast]";

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - Prepare 1 csv files to import 2 streetlights sharing the same location in a geozone");
            Step("**** Precondition ****\n");

            Step("-> Create data for testing");
            DeleteGeozones("GZNFT12*");
            CreateNewGeozone(geozone, latMin: "8.72378", latMax: "8.72623", lngMin: "105.17394", lngMax: "105.17909");
            CreateNewController(controller, geozone);
            CreateCsvDevices(csvFilePath, fullGeozonePath, new List<DeviceModel>
            {
                new DeviceModel{ Type = DeviceType.Streetlight, Id = streetlight1, Name = streetlight1, Controller = controller, TypeOfEquipment = typeOfEquipment, Latitude = clusterLatitude, Longitude = clusterLongitude, UniqueAddress = SLVHelper.GenerateMACAddress() },
                new DeviceModel{ Type = DeviceType.Streetlight, Id = streetlight2, Name = streetlight2, Controller = controller, TypeOfEquipment = typeOfEquipment, Latitude = clusterLatitude, Longitude = clusterLongitude, UniqueAddress = SLVHelper.GenerateMACAddress() }
            });
            ImportFile(csvFilePath);

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.FailureTracking);           

            Step("1. Go to Failure Tracking App and select the testing geozone");
            var failureTrackingPage = desktopPage.GoToApp(App.FailureTracking) as FailureTrackingPage;
            failureTrackingPage.GeozoneTreeMainPanel.SelectNode(geozone);

            Step("2. Verify The is only a marker displays with number: '2'");
            Step(" - The backround color is set the OK color Green and the numberic text's color is White");
            failureTrackingPage.Map.MoveToDeviceGL(clusterLongitude, clusterLatitude);
            var cluster = failureTrackingPage.Map.GetClusterSprite(clusterLongitude, clusterLatitude);
            VerifyEqual("2. Verify The is only a marker displays with number: '2'", "2", cluster.DeviceCount);
            VerifyEqual("2. Verify The backround color is set the OK color Green and the numberic text's color is White", true, IsOkColor(cluster.Status));

            Step("3. Send a command to simulate a warning of HighVoltage for streetlight 01 (valueName=HighVoltage &value=true) and a command to simulate a failure of Lamp Failure for streetlight 02 (valueName=LampFailure & value =true)");
            Step("4. Verify The commands sent successfully");
            var currentDateTime = Settings.GetServerTime();
            var request1 = SetValueToDevice(controller, streetlight1, "HighVoltage", true, currentDateTime);
            VerifyEqual(string.Format("4. Verify The command sent to '{0} successfully", streetlight1), true, request1);
            var request2 = SetValueToDevice(controller, streetlight2, "LampFailure", true, currentDateTime);
            VerifyEqual(string.Format("4. Verify The command sent to '{0} successfully", streetlight2), true, request2);

            Step("5. Refresh the page and go back to Failure Tracking App and select the testing geozone");
            desktopPage = Browser.RefreshLoggedInCMS();
            failureTrackingPage = desktopPage.GoToApp(App.FailureTracking) as FailureTrackingPage;
            failureTrackingPage.GeozoneTreeMainPanel.SelectNode(geozone);

            Step("6. Verify The is only a marker displays with number: '2'");
            Step(" - The background color is set the failure color RED and the numberic text's color is White");
            failureTrackingPage.Map.MoveToDeviceGL(clusterLongitude, clusterLatitude);
            cluster = failureTrackingPage.Map.GetClusterSprite(clusterLongitude, clusterLatitude);
            VerifyEqual("6. Verify The is only a marker displays with number: '2'", "2", cluster.DeviceCount);
            VerifyTrue("[#1429535] 6. Verify The background color is set the failure color RED and the numberic text's color is White", IsErrorColor(cluster.Status), "error", cluster.Status);

            Step("7. Hover the maker");
            failureTrackingPage.Map.MoveToDeviceGL(clusterLongitude, clusterLatitude);

            Step("8. Verify The tooltip displays with 2 rows");
            Step(" - 'Device Cluster'");
            Step(" - '{#} devices'. Ex: 2 devices");
            var clusterName = failureTrackingPage.Map.GetDeviceNameGL();
            var deviceCount = failureTrackingPage.Map.GetTooltipDevicesCountGL();
            VerifyEqual("8. Verify A tooltip displays: Device Cluster", "Device Cluster", clusterName);
            VerifyEqual("8. Verify A tooltip displays: {#} devices", "2 devices", deviceCount);

            Step("9. Click on the marker");
            failureTrackingPage.Map.SelectDeviceGL(clusterLongitude, clusterLatitude);
            failureTrackingPage.Map.WaitForDeviceClusterPopupPanelDisplayed();

            Step("10. Verify The table pop-up displays with");
            Step(" - Tittle: Cluster with {#} devices");
            Step(" - 'Show/Hide columns' button");
            VerifyEqual("10. Verify Title: 'Cluster with {#} devices", "Cluster with 2 devices", failureTrackingPage.Map.DeviceClusterPopupPanel.GetPanelTitleText());
            VerifyEqual("10. Verify 'Show/Hide columns' button displayed", true, failureTrackingPage.Map.DeviceClusterPopupPanel.IsShowHideColumnsButtonDisplayed());
            
            Step("11. Click a row of the streetlight having failure on the grid");
            failureTrackingPage.Map.DeviceClusterPopupPanel.TickGridColumn(streetlight2, true);
            failureTrackingPage.WaitForPreviousActionComplete();

            Step("12. Verify its checkbox is checked");
            Step(" - A Failure Tracking panel displays on the left screen with");
            Step("  + The name of the testing streetlight");
            Step("  + The warning message with Error icon; 'Lamp failure'; datetime formatted M/d/yyyy hh:mn (the time sending the simulated command)");
            VerifyEqual("12. Verify its checkbox is checked", true, failureTrackingPage.Map.DeviceClusterPopupPanel.GetCheckBoxGridColumnValue(streetlight2));
            VerifyEqual("12. Verify A Failure Tracking panel displays on the left screen", true, failureTrackingPage.FailureTrackingDetailsPanel.IsPanelVisible());
            VerifyEqual("12. Verify The name of the testing streetlight", streetlight2, failureTrackingPage.FailureTrackingDetailsPanel.GetDeviceNameValueText());
            var failure = failureTrackingPage.FailureTrackingDetailsPanel.GetListOfFailures().FirstOrDefault();
            if (failure != null)
            {
                VerifyEqual("12. Verify Communication Failure displays: Error icon", "icon-error", failure.Icon);
                VerifyEqual("12. Verify Communication Failure displays: Lamp failure", "Lamp failure", failure.Name);
                VerifyEqual("12. Verify Communication Failure displays: datetime formatted M/d/yyyy HH:mn (the time sending the simulated command)", currentDateTime.ToString("M/d/yyyy HH:mm"), failure.Time);
            }
            else
            {
                Warning("12. This is no failure found!");
            }

            Step("13. Click that row again");
            failureTrackingPage.Map.DeviceClusterPopupPanel.TickGridColumn(streetlight2, false);
            failureTrackingPage.WaitForPreviousActionComplete();

            Step("14. Verify its checkbox is unchecked");
            Step(" - The Failure Tracking panel is closed");
            VerifyEqual("14. Verify its checkbox is unchecked", false, failureTrackingPage.Map.DeviceClusterPopupPanel.GetCheckBoxGridColumnValue(streetlight2));
            VerifyEqual("14. Verify The Failure Tracking panel is closed", false, failureTrackingPage.FailureTrackingDetailsPanel.IsPanelVisible());

            Step("15. Check the color of device icon in Type column");
            Step("16. Verify");
            Step(" - The streetlight having a failure displays with a Red circle containing a White bulb icon");
            Step(" - The streetlight having a warning displays with a Orange circle containing a White bulb icon");
            var listOfIconType = failureTrackingPage.Map.DeviceClusterPopupPanel.GetListOfIconType();
            VerifyEqual("[#1429535] 16. Verify The streetlight has no warning displays with a Red light icon", true, listOfIconType.Exists(p => p.Contains("streetlight-error.png")));
            VerifyEqual("16. The streetlight having a warning displays with a Orange circle containing a White bulb icon", true, listOfIconType.Exists(p => p.Contains("streetlight-warning.png")));

            Step("17. Press 'X' button");
            failureTrackingPage.Map.DeviceClusterPopupPanel.ClickCloseButton();

            Step("18. Verify The pop-up is closed");
            VerifyEqual("18. Verify The pop-up is closed", false, failureTrackingPage.Map.IsDeviceClusterPopupPanelDisplayed());

            try
            {
                DeleteGeozone(geozone);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("FT_13 1061890 Location Search - in Failure Tracking")]
        public void FT_13()
        {
            var testData = GetTestDataOfTestFT_13();
            var latMin = testData["LatMin"];
            var latMax = testData["LatMax"];
            var lngMin = testData["LngMin"];
            var lngMax = testData["LngMax"];
            var partialAddress = testData["PartialAddress"];
            var fullAddress = testData["FullAddress"];
            var geozone = SLVHelper.GenerateUniqueName("GZNFT13");
            var controller = SLVHelper.GenerateUniqueName("CLFT13");
            var streetlight = SLVHelper.GenerateUniqueName("SLFT13");

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - Create a testing geozone containing a streetlight which is located at a well-known location. Ex: Champ de Mars, 5 Avenue Anatole France, 75007 Paris, France");
            Step("**** Precondition ****\n");

            Step("-> Create data for testing");
            DeleteGeozones("GZNFT13*");
            var streetlightLat = SLVHelper.GenerateCoordinate("48.26468", "48.26670");
            var streetlightLng = SLVHelper.GenerateCoordinate("2.69184", "2.69438");
            CreateNewGeozone(geozone, latMin: latMin, latMax: latMax, lngMin: lngMin, lngMax: lngMax);
            CreateNewController(controller, geozone, lat: SLVHelper.GenerateCoordinate(latMin, latMax), lng: SLVHelper.GenerateCoordinate(lngMin, lngMax));
            CreateNewDevice(DeviceType.Streetlight, streetlight, controller, geozone, lat: streetlightLat, lng: streetlightLng);

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.FailureTracking);

            Step("1. Go to Failure Tracking app");
            var failureTrackingPage = desktopPage.GoToApp(App.FailureTracking) as FailureTrackingPage;

            Step("2. Verify There is a button with icon: Globe icon on the top-right corner of the GeoZone tree");
            VerifyEqual("2. Verify There is a button with icon: Globe icon on the top-right corner of the GeoZone tree", true, failureTrackingPage.GeozoneTreeMainPanel.IsMapFilterButtonVisible());

            Step("3. Hover the button");
            failureTrackingPage.GeozoneTreeMainPanel.HoverMapSearchButton();

            Step("4. Verify The text 'Map Search' displays");
            VerifyEqual("4. Verify The text 'Map Search' displays", "Map Search", failureTrackingPage.GeozoneTreeMainPanel.GetMapSearchButtonTooltip());

            Step("5. Click the button");
            failureTrackingPage.GeozoneTreeMainPanel.ClickMapSearchButton();
            failureTrackingPage.GeozoneTreeMainPanel.WaitForMapSearchPanelDisplayed();

            Step("6. Verify A panel displays with");
            Step(" o Title: Map Search");
            Step(" o Text: Search by Location");
            Step(" o Textbox with a Magnifying Glass icon and the text 'Search in map'");
            Step(" o Button: Back");
            VerifyEqual("6. Verify A panel displays: Title: Map Search", "Map Search", failureTrackingPage.GeozoneTreeMainPanel.MapSearchPanel.GetPanelTitleText());
            VerifyEqual("6. Verify A panel displays: Text: Search by Location", "Search by Location", failureTrackingPage.GeozoneTreeMainPanel.MapSearchPanel.GetContentText());
            VerifyEqual("6. Verify A panel displays: Textbox", true, failureTrackingPage.GeozoneTreeMainPanel.MapSearchPanel.IsSearchInputDisplayed());
            VerifyEqual("6. Verify A panel displays: Textbox with a Magnifying Glass icon", true, failureTrackingPage.GeozoneTreeMainPanel.MapSearchPanel.IsSearchInputHasMagnifyingGlass());
            VerifyEqual("6. Verify A panel displays: Textbox with the text 'Search in map'", "Search in map", failureTrackingPage.GeozoneTreeMainPanel.MapSearchPanel.GetSearchPlaceholder());
            VerifyEqual("6. Verify A panel displays: Button: Back", true, failureTrackingPage.GeozoneTreeMainPanel.MapSearchPanel.IsBackButtonDisplayed());

            Step("7. Enter a partial of the testing address into the input");
            failureTrackingPage.GeozoneTreeMainPanel.MapSearchPanel.EnterSearchInput(partialAddress);
            failureTrackingPage.GeozoneTreeMainPanel.MapSearchPanel.WaitForSuggestionsDisplayed();

            Step("8. Verify The search results appear as user types. The matched words are bold");
            var searchSuggestionsBoldTextList = failureTrackingPage.GeozoneTreeMainPanel.MapSearchPanel.GetSearchSuggestionsBoldText();
            VerifyTrue("8. Verify The search results appear as user types. The matched words are bold", searchSuggestionsBoldTextList.All(p => p.Equals(partialAddress)), "all matched words are bold", string.Join(", ", searchSuggestionsBoldTextList));

            Step("9. Input the full value of the testing address, then click on the 1 result in the list");
            failureTrackingPage.GeozoneTreeMainPanel.MapSearchPanel.ClickClearSearchButton();
            failureTrackingPage.GeozoneTreeMainPanel.MapSearchPanel.WaitForClearSearchButtonDisappeared();
            failureTrackingPage.GeozoneTreeMainPanel.MapSearchPanel.EnterSearchInput(fullAddress);
            failureTrackingPage.GeozoneTreeMainPanel.MapSearchPanel.WaitForSuggestionsDisplayed();
            failureTrackingPage.GeozoneTreeMainPanel.MapSearchPanel.SelectSearchSuggestion();

            Step("10. Verify The map is centered on the selected location and zoomed to level 15(50 m)");
            Wait.ForGLMapStopFlying();
            VerifyEqual("10. Verify The map is centered on the selected location and zoomed to level 15(50 m)", "50 m", failureTrackingPage.Map.GetMapGLScaleText());

            Step("11. Verify There is an Orange location icon on the center of the map");
            VerifyEqual("11. Verify There is location icon on the center of the map", true, failureTrackingPage.Map.IsLocationSearchMarkerDisplayed());
            VerifyEqual("11. Verify There is Orange icon", true, failureTrackingPage.Map.GetLocationSearchMarkerImageSrc().Contains("marker-generic.svg"));

            Step("12. Select the streetlight on the map");
            failureTrackingPage.Map.SelectDeviceGL(streetlightLng, streetlightLat);
            failureTrackingPage.WaitForDetailsPanelDisplayed();

            Step("13. Verify The device is selected and the Map Search panel remains visible");
            VerifyEqual("13. Verify The device is selected", true, failureTrackingPage.Map.HasSelectedDevicesInMapGL());
            VerifyEqual("13. Verify Map Search panel remains visible", true, failureTrackingPage.GeozoneTreeMainPanel.IsMapSearchPanelDisplayed());

            Step("14. Verify The Failure Tracking panel displays on the right");
            VerifyEqual("14. Verify The Failure Tracking panel displays on the right", true, failureTrackingPage.IsDetailsPanelDisplayed());

            Step("15. Press X icon on the Search box");
            failureTrackingPage.GeozoneTreeMainPanel.MapSearchPanel.ClickClearSearchButton();
            failureTrackingPage.GeozoneTreeMainPanel.MapSearchPanel.WaitForClearSearchButtonDisappeared();

            Step("16. Verify The input is cleared");
            VerifyEqual("16. Verify The input is cleared", "", failureTrackingPage.GeozoneTreeMainPanel.MapSearchPanel.GetSearchValue());

            Step("17. Press Back button");
            failureTrackingPage.GeozoneTreeMainPanel.MapSearchPanel.ClickBackToolbarButton();
            failureTrackingPage.GeozoneTreeMainPanel.WaitForMapSearchPanelDisappeared();

            Step("18. Verify Geozone tree panel displays again");
            Step(" o The streetlight is selected in the geozone tree");
            Step(" o The Failure Tracking panel remains visible");
            VerifyEqual("18. Verify The streetlight is selected in the geozone tree", streetlight, failureTrackingPage.GeozoneTreeMainPanel.GetSelectedNodeName());
            VerifyEqual("18. Verify The Failure Tracking panel remains visible", true, failureTrackingPage.IsDetailsPanelDisplayed());

            try
            {
                DeleteGeozone(geozone);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("FT_1377754 Failure Tracking - Display devices with no unique address in grey on the map (bug 1319077)")]
        public void FT_1377754()
        {
            var testData = GetTestDataOfTestFT_1377754();
            var geozoneRTC = testData["Geozone"].ToString();
            var controllerRTC = testData["Controller"] as DeviceModel;
            var streetlights = testData["Streetlights"] as List<DeviceModel>;
            var streetlightRTC = streetlights.PickRandom();
            var geozone = SLVHelper.GenerateUniqueName("GZNFT1377754");
            var streetlight = SLVHelper.GenerateUniqueName("STL01");
            var streetlight2 = SLVHelper.GenerateUniqueName("STL02");
            var uniqueAddress = SLVHelper.GenerateMACAddress();
            var streetlightLat = SLVHelper.GenerateCoordinate("29.62085", "29.62247");
            var streetlightLng = SLVHelper.GenerateCoordinate("116.66788", "116.66998");
            var typeOfEquipment = "Telematics LCU[Lamp]";

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - Create a testing geozone containing a streetlight connected to the controller: Smartsims and having a valid unique Mac Address");
            Step("**** Precondition ****\n");

            Step("-> Create data for testing");
            DeleteGeozones("GZNFT1377754*");
            CreateNewGeozone(geozone, latMin: "29.61894", latMax: "29.62364", lngMin: "116.66346", lngMax: "116.67447");
            CreateNewDevice(DeviceType.Streetlight, streetlight, controllerRTC.Id, geozone, typeOfEquipment, lat: streetlightLat, lng: streetlightLng);
            SetValueToDevice(controllerRTC.Id, streetlight, "MacAddress", uniqueAddress, Settings.GetServerTime());

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.EquipmentInventory, App.FailureTracking);

            Step("1. Go to Failure Tracking app, and select the testing geozone");
            var failureTrackingPage = desktopPage.GoToApp(App.FailureTracking) as FailureTrackingPage;
            
            Step("2. Select the testing streetlight");
            failureTrackingPage.GeozoneTreeMainPanel.SelectNode(geozone + @"\" + streetlight);

            Step("3. Verify The streetlight is displayed with colored marker on the map");
            Step(" o Green circle containing a white bulb icon");
            failureTrackingPage.Map.MoveToDeviceGL(streetlightLng, streetlightLat);
            var spriteStatus = failureTrackingPage.Map.GetDeviceSpriteStatus(streetlightLng, streetlightLat);
            VerifyTrue("3. Verify The streetlight is displayed with colored marker on the map (Green circle containing a white bulb icon)", IsOkColor(spriteStatus), "ok", spriteStatus);
            
            Step("4. Go to Equipment Inventory, and select the testing streetlight, then clear its Mac Address, and save changes");
            var equipmentInventoryPage = failureTrackingPage.AppBar.SwitchTo(App.EquipmentInventory) as EquipmentInventoryPage;
            equipmentInventoryPage.WaitForDeviceEditorPanelDisplayed();
            equipmentInventoryPage.StreetlightEditorPanel.ClearUniqueAddressInput();
            equipmentInventoryPage.StreetlightEditorPanel.ClickSaveButton();
            equipmentInventoryPage.WaitForPreviousActionComplete();
            equipmentInventoryPage.WaitForDeviceEditorPanelDisappeared();

            Step("5. Switch back to Failure Tracking app");
            failureTrackingPage = equipmentInventoryPage.AppBar.SwitchTo(App.FailureTracking) as FailureTrackingPage;
            failureTrackingPage.WaitForDetailsPanelDisplayed();

            Step("6. Verify The streetlight is displayed with the grey marker on the map");
            Step(" o Black circle containing a white bulb icon");
            failureTrackingPage.Map.MoveToDeviceGL(streetlightLng, streetlightLat);
            spriteStatus = failureTrackingPage.Map.GetDeviceSpriteStatus(streetlightLng, streetlightLat);
            VerifyTrue("6. Verify The streetlight is displayed with the grey marker on the map (Black circle containing a white bulb icon)", spriteStatus.Equals("selected") || spriteStatus.Equals("ready"), "ready", spriteStatus);

            Step("7. Select a streetlight in another geozone and then go back to the testing geozone and select the testing streetlight");
            failureTrackingPage.GeozoneTreeMainPanel.SelectNode(geozoneRTC + @"\" + streetlightRTC.Name);
            failureTrackingPage.GeozoneTreeMainPanel.SelectNode(streetlight);

            Step("8. Verify The streetlight is displayed with the grey marker on the map");
            Step(" o Black circle containing a white bulb icon");
            failureTrackingPage.Map.MoveToDeviceGL(streetlightLng, streetlightLat);
            spriteStatus = failureTrackingPage.Map.GetDeviceSpriteStatus(streetlightLng, streetlightLat);
            VerifyTrue("8. Verify The streetlight is displayed with the grey marker on the map (Black circle containing a white bulb icon)", spriteStatus.Equals("selected") || spriteStatus.Equals("ready"), "ready", spriteStatus);

            Step("9. Go to Equipment Inventory, and select the testing geozone and add a new streetlight without Mac Address, then save changes.");
            equipmentInventoryPage = failureTrackingPage.AppBar.SwitchTo(App.EquipmentInventory) as EquipmentInventoryPage;
            equipmentInventoryPage.WaitForDeviceEditorPanelDisplayed();
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(geozone);          
            equipmentInventoryPage.CreateDevice(DeviceType.Streetlight, streetlight2, controllerRTC.Name, streetlight2, typeOfEquipment);
            var streetlight2Lat = equipmentInventoryPage.StreetlightEditorPanel.GetLatitudeValue().Replace(" °", string.Empty).Trim();
            var streetlight2Lng = equipmentInventoryPage.StreetlightEditorPanel.GetLongitudeValue().Replace(" °", string.Empty).Trim();

            Step("10. Switch back to Failure Tracking app");
            failureTrackingPage = equipmentInventoryPage.AppBar.SwitchTo(App.FailureTracking) as FailureTrackingPage;

            Step("11. Verify The streetlight is displayed with the grey marker on the map");
            Step(" o Black circle containing a white bulb icon");
            failureTrackingPage.Map.MoveToDeviceGL(streetlight2Lng, streetlight2Lat);
            spriteStatus = failureTrackingPage.Map.GetDeviceSpriteStatus(streetlight2Lng, streetlight2Lat);
            VerifyTrue("11. Verify The streetlight is displayed with the grey marker on the map (Black circle containing a white bulb icon)", spriteStatus.Equals("selected") || spriteStatus.Equals("ready"), "ready", spriteStatus);

            try
            {
                DeleteGeozone(geozone);
            }
            catch { }
        }

        #endregion //Test Cases

        #region Private methods        

        #region Verify methods

        #endregion //Verify methods

        #region Input XML data

        private Dictionary<string, object> GetCommonTestData()
        {
            var realtimeGeozone = Settings.CommonTestData[0];
            var testData = new Dictionary<string, object>();
            testData.Add("Geozone", realtimeGeozone.Path);
            var controller = realtimeGeozone.Devices.FirstOrDefault(p => p.Type == DeviceType.Controller && p.Status == DeviceStatus.Working);
            testData.Add("Controller", controller);
            var streetlights = realtimeGeozone.Devices.Where(p => p.Type == DeviceType.Streetlight && p.Status == DeviceStatus.Working && string.IsNullOrEmpty(p.Cluster)).ToList();
            testData.Add("Streetlights", streetlights);

            return testData;
        }
        
        private Dictionary<string, object> GetTestDataOfTestFT_01()
        {
            return GetCommonTestData();
        }
        
        private Dictionary<string, object> GetTestDataOfTestFT_02()
        {
            return GetCommonTestData();
        }
        
        private Dictionary<string, string> GetTestDataOfTestFT_03()
        {
            var testCaseName = "FT_03";
            var xmlUtility = new XmlUtility(Settings.FT_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, string>();

            testData.Add("DeviceWithFailure.geozone", xmlUtility.GetSingleNodeAttrVal(string.Format(Settings.FT_XPATH_PREFIX, testCaseName, "DeviceWithFailure"), "geozone"));
            testData.Add("DeviceWithFailure.name", xmlUtility.GetSingleNodeAttrVal(string.Format(Settings.FT_XPATH_PREFIX, testCaseName, "DeviceWithFailure"), "name"));

            testData.Add("DeviceWithNoFailure.geozone", xmlUtility.GetSingleNodeAttrVal(string.Format(Settings.FT_XPATH_PREFIX, testCaseName, "DeviceWithNoFailure"), "geozone"));
            testData.Add("DeviceWithNoFailure.name", xmlUtility.GetSingleNodeAttrVal(string.Format(Settings.FT_XPATH_PREFIX, testCaseName, "DeviceWithNoFailure"), "name"));

            return testData;

        }
        
        private Dictionary<string, GeozoneModel> GetTestDataOfTestFT_05()
        {
            var testCaseName = "FT_05";
            var xmlUtility = new XmlUtility(Settings.FT_TEST_DATA_FILE_PATH);
            var nodeGeozoneA = xmlUtility.GetSingleNode(string.Format(Settings.FT_XPATH_PREFIX, testCaseName, "GeozoneA"));

            var geozoneA = new GeozoneModel();
            geozoneA.Name = nodeGeozoneA.GetAttrVal("name");

            var geozoneB = new GeozoneModel();

            var nodeGeozoneB = xmlUtility.GetSingleNode(string.Format(Settings.FT_XPATH_PREFIX, testCaseName, "GeozoneB"));
            geozoneB.Name = nodeGeozoneB.GetAttrVal("name");
            foreach (XmlNode nodeStreetlight in nodeGeozoneB.ChildNodes)
            {
                geozoneB.Devices.Add(new DeviceModel
                {
                    Name = nodeStreetlight.GetAttrVal("name"),
                    Id = nodeStreetlight.GetAttrVal("id"),
                    Latitude = nodeStreetlight.GetAttrVal("latitude"),
                    Longitude = nodeStreetlight.GetAttrVal("longitude")
                });
            }

            var nodeGeozoneC = xmlUtility.GetSingleNode(string.Format(Settings.FT_XPATH_PREFIX, testCaseName, "GeozoneC"));
            var geozoneC = new GeozoneModel();
            geozoneC.Name = nodeGeozoneC.GetAttrVal("name");

            foreach (XmlNode nodeStreetlight in nodeGeozoneC.ChildNodes)
            {
                geozoneC.Devices.Add(new DeviceModel
                {
                    Name = nodeStreetlight.GetAttrVal("name"),
                    Id = nodeStreetlight.GetAttrVal("id"),
                    Latitude = nodeStreetlight.GetAttrVal("latitude"),
                    Longitude = nodeStreetlight.GetAttrVal("longitude")
                });
            }

            var testData = new Dictionary<string, GeozoneModel>();

            testData.Add("GeozoneA", geozoneA);
            testData.Add("GeozoneB", geozoneB);
            testData.Add("GeozoneC", geozoneC);

            return testData;
        }

        private Dictionary<string, string> GetTestDataOfTestFT_09()
        {
            var testCaseName = "FT_09";
            var xmlUtility = new XmlUtility(Settings.FT_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, string>();
            testData.Add("OlsonTimeZoneId", xmlUtility.GetSingleNodeText(string.Format(Settings.FT_XPATH_PREFIX, testCaseName, "OlsonTimeZoneId")));

            return testData;
        }       

        private Dictionary<string, string> GetTestDataOfTestFT_13()
        {
            var testCaseName = "FT_13";
            var xmlUtility = new XmlUtility(Settings.FT_TEST_DATA_FILE_PATH);
            
            var testData = new Dictionary<string, string>();
            var geozoneInfo = xmlUtility.GetSingleNode(string.Format(Settings.FT_XPATH_PREFIX, testCaseName, "Geozone"));
            testData.Add("LatMin", geozoneInfo.GetAttrVal("latMin"));
            testData.Add("LatMax", geozoneInfo.GetAttrVal("latMax"));
            testData.Add("LngMin", geozoneInfo.GetAttrVal("lngMin"));
            testData.Add("LngMax", geozoneInfo.GetAttrVal("lngMax"));

            testData.Add("FullAddress", geozoneInfo.GetAttrVal("fullAddress"));
            testData.Add("PartialAddress", geozoneInfo.GetAttrVal("partialAddress"));

            return testData;
        }

        private Dictionary<string, object> GetTestDataOfTestFT_1377754()
        {
            return GetCommonTestData();
        }

        #endregion //Input XML data        

        private bool IsGreyColor(string spriteStatus)
        {
            return spriteStatus.Equals("ready");
        }

        private bool IsOkColor(string spriteStatus)
        {
            return spriteStatus.Equals("ok");
        }

        private bool IsWarningColor(string spriteStatus)
        {
            return spriteStatus.Equals("warning");
        }

        private bool IsErrorColor(string spriteStatus)
        {
            return spriteStatus.Equals("error");
        }

        #endregion //Private methods
    }
}