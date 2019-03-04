using ImageMagick;
using NUnit.Framework;
using StreetlightVision.Extensions;
using StreetlightVision.Models;
using StreetlightVision.Pages;
using StreetlightVision.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace StreetlightVision.Tests.Coverage.Widgets
{
    [TestFixture]
    [NonParallelizable]
    public class GeozoneFailuresMonitorWidgetTests : TestBase
    {
        #region Variables
        
        #endregion //Variables

        #region Contructors

        #endregion //Contructors        

        #region Test Cases       

        [Test, DynamicRetry]
        [Description("GFM_01 System calculates the number and percent of devices having Critical Faults in a specific geozone")]
        [NonParallelizable]
        public void GFM_01()
        {
            var testData = GetTestDataOfTestGFM_01();

            var geozone = testData["Geozone"];
            var geozoneName = geozone.GetChildName();
            var streetlight = testData["Streetlight"];
            var controllerId = testData["ControllerId"];
            var controllerName = testData["ControllerName"];

            Step("**** Precondition ****");
            Step(" - Login to the system successfully");
            Step(" - Install the widget Geozone Failures Monitor");
            Step(" - Create a specific geozone and containing a streetlight");
            Step(" - Simulate the Critical Faults fot the streetlight using 'LampFailure' parameter and value=true");
            Step("**** Precondition ****\n");

            var request = SetValueToDevice(controllerId, streetlight, "LampFailure", "true", Settings.GetServerTime());
            VerifyEqual("-> Verify the request is sent successfully (attribute: LampFailure, value: true)", true, request);

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.EquipmentInventory);
            desktopPage.DeleteWidgets(Widget.GeozoneFailuresMonitor);
            desktopPage.InstallWidgets(Widget.GeozoneFailuresMonitor);

            Step("1. Navigate to the Geozone Failures Monitor widget");
            Step("2. Verify");
            Step(" o All 3 options 'Properly functional', 'Faulty devices', 'Critical faults' are selected");
            Step(" o The 'Failures' displays at the bottom of the widget");
            Step(" o There is a Config button on the bottom right of the widget");
            var expectedOptions = new List<string> { "Properly functional", "Faulty devices", "Critical faults" };
            var actualOptions = desktopPage.GetGeozoneFailuresMonitorPieChartOptions().Select(p => p.Name).ToList();
            var info = desktopPage.GetGeozoneFailuresMonitorInfo();
            VerifyEqual("2. Verify All 3 options 'Properly functional', 'Faulty devices', 'Critical faults' are selected", expectedOptions, actualOptions);
            VerifyEqual("2. Verify The Root Geozone displays at the bottom of the widget", Settings.RootGeozoneName, info.GeoZoneName);
            VerifyEqual("2. Verify The 'Failures' displays at the bottom of the widget", "Failures", desktopPage.GetGeozoneFailuresMonitorCaption());
            VerifyEqual("2. Verify There is a Config button on the bottom right of the widget", true, desktopPage.IsGeozoneFailuresMonitorSettingButtonDisplayed());

            Step("3. Press Config button and select the testing geozone");
            desktopPage.ClickGeozoneFailuresMonitorSetting();
            desktopPage.WaitForPreviousActionComplete();
            desktopPage.WaitForFailuresMonitorSettingDisplayed();
            desktopPage.GeozoneTreeWidgetPanel.SelectNode(geozone);
            desktopPage.WaitForFailuresMonitorSettingDisappeared();

            Step("4. Verify");
            Step(" o The testing geozone's name + 'Failures' displays at the bottom of the widget");
            Step(" o There is a RED circle surrounding a WHITE circle");
            Step(" o In the WHITE circle there is the number and percent of devices having Critical faults. They are '100%' and '1 Devices'");
            Step(" o The text in WHILE circle is RED");
            var options = desktopPage.GetGeozoneFailuresMonitorPieChartOptions();
            info = desktopPage.GetGeozoneFailuresMonitorInfo();
            var criticalFaultsOption = options.FirstOrDefault(p => p.Name.Equals("Critical faults"));
            var totalPercent = string.Format("{0}%", Math.Round(options.Sum(p => p.Percentage)) * 100);
            var totalDevicesCount = options.Sum(p => p.Count);
            VerifyEqual("4. Verify The testing geozone's name displays at the bottom of the widget", geozoneName, info.GeoZoneName);
            VerifyEqual("4. Verify The 'Failures' displays at the bottom of the widget", "Failures", desktopPage.GetGeozoneFailuresMonitorCaption());
            VerifyEqual("[#1429535] 4. Verify There is a RED circle surrounding a WHITE circle", true, totalDevicesCount == 1 && criticalFaultsOption.Count == 1 && criticalFaultsOption.ColorHex.Equals("#FF0000"));
            VerifyEqual("4. Verify In the WHITE circle there is the number and percent of devices having Critical Faults, Percent is '100%'", "100%", totalPercent);
            VerifyEqual("4. Verify In the WHITE circle there is the number and percent of devices having Critical Faults, Devices count is '1'", 1, totalDevicesCount);
            
            Step("5. Deselect the option 'Critical Faults'");
            desktopPage.ClickGeozoneFailuresMonitorOption("Critical faults");
            
            Step("6. Verify The system calculates the number and percent again. They are now 0% and '0 Devices'");
            var options1 = desktopPage.GetGeozoneFailuresMonitorPieChartOptions();
            var otherOptionsPercent = string.Format("{0}%", Math.Round(options1.Where(p => !p.Name.Equals("Critical faults")).Sum(p => p.Percentage)) * 100);
            var otherOptionsDeviceCount = options1.Where(p => !p.Name.Equals("Critical faults")).Sum(p => p.Count);
            VerifyEqual("6. Verify The system calculates the number and percent again, Percent is '0%'", "0%", otherOptionsPercent);
            VerifyEqual("6. Verify The system calculates the number and percent again, Devices count is '0'", 0, otherOptionsDeviceCount);            
        }

        [Test, DynamicRetry]
        [Description("GFM_02 System calculates the number and percent of devices having Warning in a specific geozone")]
        [NonParallelizable]
        public void GFM_02()
        {
            var testData = GetTestDataOfTestGFM_02();

            var geozone = testData["Geozone"];
            var geozoneName = geozone.GetChildName();
            var streetlight = testData["Streetlight"];
            var controllerId = testData["ControllerId"];
            var controllerName = testData["ControllerName"];

            Step("**** Precondition ****");
            Step(" - Login to the system successfully");
            Step(" - Install the widget Geozone Failures Monitor");
            Step(" - Create a specific geozone and containing a streetlight");
            Step(" - Simulate the Warnings for the streetlight using 'HighVoltage' parameter and value=true");
            Step("**** Precondition ****\n");

            var request = SetValueToDevice(controllerId, streetlight, "HighVoltage", "true", Settings.GetServerTime());
            VerifyEqual("-> Verify the request is sent successfully (attribute: HighVoltage, value: true)", true, request);

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.EquipmentInventory);
            desktopPage.DeleteWidgets(Widget.GeozoneFailuresMonitor);
            desktopPage.InstallWidgets(Widget.GeozoneFailuresMonitor);

            Step("1. Navigate to the Geozone Failures Monitor widget");
            Step("2. Press Config button and select the testing geozone");
            desktopPage.ClickGeozoneFailuresMonitorSetting();
            desktopPage.WaitForPreviousActionComplete();
            desktopPage.WaitForFailuresMonitorSettingDisplayed();
            desktopPage.GeozoneTreeWidgetPanel.SelectNode(geozone);
            desktopPage.WaitForFailuresMonitorSettingDisappeared();

            Step("3. Verify");
            Step(" o The testing geozone's name + 'Failures' displays at the bottom of the widget");
            Step(" o There is a ORANGE circle surrounding a WHITE circle");
            Step(" o In the WHITE circle there is the number and percent of devices having Faulty devices. They are '100%' and '1 Devices'");
            Step(" o The text in WHITE circle is ORANGE");
            var options = desktopPage.GetGeozoneFailuresMonitorPieChartOptions();
            var info = desktopPage.GetGeozoneFailuresMonitorInfo();
            var faultyDevicesOption = options.FirstOrDefault(p => p.Name.Equals("Faulty devices"));
            var totalPercent = string.Format("{0}%", Math.Round(options.Sum(p => p.Percentage)) * 100);
            var totalDevicesCount = options.Sum(p => p.Count);
            VerifyEqual("3. Verify The testing geozone's name displays at the bottom of the widget", geozoneName, info.GeoZoneName);
            VerifyEqual("3. Verify The 'Failures' displays at the bottom of the widget", "Failures", desktopPage.GetGeozoneFailuresMonitorCaption());
            VerifyEqual("3. Verify There is a ORANGE circle surrounding a WHITE circle", true, totalDevicesCount == 1 && faultyDevicesOption.Count == 1 && faultyDevicesOption.ColorHex.Equals("#FF8800"));
            VerifyEqual("3. Verify In the WHITE circle there is the number and percent of devices having Faulty devices, Percent is '100%'", "100%", totalPercent);
            VerifyEqual("3. Verify In the WHITE circle there is the number and percent of devices having Faulty devices, Devices count is '1'", 1, totalDevicesCount);

            Step("4. Deselect the option 'Faulty devices'");
            desktopPage.ClickGeozoneFailuresMonitorOption("Faulty devices");

            Step("5. Verify The system calculates the number and percent again. They are now 0% and '0 Devices'");
            var options1 = desktopPage.GetGeozoneFailuresMonitorPieChartOptions();
            var otherOptionsPercent = string.Format("{0}%", Math.Round(options1.Where(p => !p.Name.Equals("Faulty devices")).Sum(p => p.Percentage)) * 100);
            var otherOptionsDeviceCount = options1.Where(p => !p.Name.Equals("Faulty devices")).Sum(p => p.Count);
            VerifyEqual("5. Verify The system calculates the number and percent again, Percent is '0%'", "0%", otherOptionsPercent);
            VerifyEqual("5. Verify The system calculates the number and percent again, Devices count is '0'", 0, otherOptionsDeviceCount);
        }

        [Test, DynamicRetry]
        [Description("GFM_03 System calculates the number and percent of devices having Properly Functional in a specific geozone")]
        [NonParallelizable]
        public void GFM_03()
        {
            var testData = GetTestDataOfTestGFM_03();
            var geozone1 = testData["Geozone1"];
            var geozone2 = testData["Geozone2"];
            var controllerId = testData["ControllerId"];
            var controllerName = testData["ControllerName"];
            var geozoneName1 = geozone1.GetChildName();
            var geozone1DevicesCount = 3;

            Step("**** Precondition ****");
            Step(" - Login to the system successfully");
            Step(" - Create a new geozone with 3 streetlights");
            Step(" - Install the widget Geozone Failures Monitor");            
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.EquipmentInventory);
            desktopPage.DeleteWidgets(Widget.GeozoneFailuresMonitor);
            desktopPage.InstallWidgets(Widget.GeozoneFailuresMonitor);

            Step("1. Navigate to the Geozone Failures Monitor widget");
            Step("2. Press Config button and select the testing geozone");
            desktopPage.ClickGeozoneFailuresMonitorSetting();
            desktopPage.WaitForPreviousActionComplete();
            desktopPage.WaitForFailuresMonitorSettingDisplayed();
            desktopPage.GeozoneTreeWidgetPanel.SelectNode(geozone1);
            desktopPage.WaitForFailuresMonitorSettingDisappeared();

            Step("3. Verify");
            Step(" o The testing geozone's name + 'Failures' displays at the bottom of the widget");
            Step(" o There is a Green circle surrounding a WHITE circle");
            Step(" o In the WHITE circle there is the number and percent of devices having Properly functional. They are '100%' and '{the number of streetlight in testing geozone} Devices'");
            var options = desktopPage.GetGeozoneFailuresMonitorPieChartOptions();
            var info = desktopPage.GetGeozoneFailuresMonitorInfo();
            var properlyFunctionalOption = options.FirstOrDefault(p => p.Name.Equals("Properly functional"));
            var totalPercent = string.Format("{0}%", Math.Round(options.Sum(p => p.Percentage)) * 100);
            var totalDevicesCount = options.Sum(p => p.Count);
            VerifyEqual("3. Verify The testing geozone's name displays at the bottom of the widget", geozoneName1, info.GeoZoneName);
            VerifyEqual("3. Verify The 'Failures' displays at the bottom of the widget", "Failures", desktopPage.GetGeozoneFailuresMonitorCaption());
            VerifyEqual("3. Verify There is a Green circle surrounding a WHITE circle", true, totalDevicesCount == geozone1DevicesCount && properlyFunctionalOption.Count == geozone1DevicesCount && properlyFunctionalOption.ColorHex.Equals("#00CC00"));
            VerifyEqual("3. Verify In the WHITE circle there is the number and percent of devices having Properly functional, Percent is '100%'", "100%", totalPercent);
            VerifyEqual("3. Verify In the WHITE circle there is the number and percent of devices having Properly functional, Devices count is '{the number of streetlight in testing geozone} Devices'", geozone1DevicesCount, totalDevicesCount);

            Step("4. Deselect the option 'Properly functional'");
            desktopPage.ClickGeozoneFailuresMonitorOption("Properly functional");

            Step("5. Verify The system calculates the number and percent again. They are now 0% and '0 Devices'");
            options = desktopPage.GetGeozoneFailuresMonitorPieChartOptions();
            var otherOptionsPercent = string.Format("{0}%", Math.Round(options.Where(p => !p.Name.Equals("Properly functional")).Sum(p => p.Percentage)) * 100);
            var otherOptionsDeviceCount = options.Where(p => !p.Name.Equals("Properly functional")).Sum(p => p.Count);
            VerifyEqual("5. Verify The system calculates the number and percent again, Percent is '0%'", "0%", otherOptionsPercent);
            VerifyEqual("5. Verify The system calculates the number and percent again, Devices count is '0'", 0, otherOptionsDeviceCount);

            Step("6. Select the option 'Properly Functional' again");
            desktopPage.ClickGeozoneFailuresMonitorOption("Properly functional");

            Step("7. Verify");
            Step(" o The testing geozone's name + 'Failures' displays at the bottom of the widget");
            Step(" o There is a Green circle surrounding a WHITE circle");
            Step(" o In the WHITE circle there is the number and percent of devices having Properly functional. They are '100%' and '{the number of streetlight in testing geozone} Devices'");
            options = desktopPage.GetGeozoneFailuresMonitorPieChartOptions();
            info = desktopPage.GetGeozoneFailuresMonitorInfo();
            properlyFunctionalOption = options.FirstOrDefault(p => p.Name.Equals("Properly functional"));
            totalPercent = string.Format("{0}%", Math.Round(options.Sum(p => p.Percentage)) * 100);
            totalDevicesCount = options.Sum(p => p.Count);
            VerifyEqual("7. Verify The testing geozone's name displays at the bottom of the widget", geozoneName1, info.GeoZoneName);
            VerifyEqual("7. Verify The 'Failures' displays at the bottom of the widget", "Failures", desktopPage.GetGeozoneFailuresMonitorCaption());
            VerifyEqual("7. Verify There is a Green circle surrounding a WHITE circle", true, totalDevicesCount == geozone1DevicesCount && properlyFunctionalOption.Count == geozone1DevicesCount && properlyFunctionalOption.ColorHex.Equals("#00CC00"));
            VerifyEqual("7. Verify In the WHITE circle there is the number and percent of devices having Properly functional, Percent is '100%'", "100%", totalPercent);
            VerifyEqual("7. Verify In the WHITE circle there is the number and percent of devices having Properly functional, Devices count is '{the number of streetlight in testing geozone} Devices'", geozone1DevicesCount, totalDevicesCount);

            Step("8. Select Config button and select another geozone, then select back the testing geozone geozone");
            desktopPage.ClickGeozoneFailuresMonitorSetting();
            desktopPage.WaitForPreviousActionComplete();
            desktopPage.WaitForFailuresMonitorSettingDisplayed();
            desktopPage.GeozoneTreeWidgetPanel.SelectNode(geozone2);
            desktopPage.WaitForFailuresMonitorSettingDisappeared();

            desktopPage.ClickGeozoneFailuresMonitorSetting();
            desktopPage.WaitForPreviousActionComplete();
            desktopPage.WaitForFailuresMonitorSettingDisplayed();
            desktopPage.GeozoneTreeWidgetPanel.SelectNode(geozone1);
            desktopPage.WaitForFailuresMonitorSettingDisappeared();

            Step("9. Verify");
            Step(" o The testing geozone's name + 'Failures' displays at the bottom of the widget");
            Step(" o There is a Green circle surrounding a WHITE circle");
            Step(" o In the WHITE circle there is the number and percent of devices having Properly functional. They are '100%' and '{the number of streetlight in testing geozone} Devices'");
            options = desktopPage.GetGeozoneFailuresMonitorPieChartOptions();
            info = desktopPage.GetGeozoneFailuresMonitorInfo();
            properlyFunctionalOption = options.FirstOrDefault(p => p.Name.Equals("Properly functional"));
            totalPercent = string.Format("{0}%", Math.Round(options.Sum(p => p.Percentage)) * 100);
            totalDevicesCount = options.Sum(p => p.Count);
            VerifyEqual("9. Verify The testing geozone's name displays at the bottom of the widget", geozoneName1, info.GeoZoneName);
            VerifyEqual("9. Verify The 'Failures' displays at the bottom of the widget", "Failures", desktopPage.GetGeozoneFailuresMonitorCaption());
            VerifyEqual("9. Verify There is a Green circle surrounding a WHITE circle", true, totalDevicesCount == geozone1DevicesCount && properlyFunctionalOption.Count == geozone1DevicesCount && properlyFunctionalOption.ColorHex.Equals("#00CC00"));
            VerifyEqual("9. Verify In the WHITE circle there is the number and percent of devices having Properly functional, Percent is '100%'", "100%", totalPercent);
            VerifyEqual("9. Verify In the WHITE circle there is the number and percent of devices having Properly functional, Devices count is '{the number of streetlight in testing geozone} Devices'", geozone1DevicesCount, totalDevicesCount);            
        }        

        [Test, DynamicRetry]
        [Description("GFM_04 System calculates the number and percent of devices with different types in a specific geozone")]
        [NonParallelizable]
        public void GFM_04()
        {
            var testData = GetTestDataOfTestGFM_04();
            var geozone1 = testData["Geozone1"];
            var geozone2 = testData["Geozone2"];
            var warningStreetlight = testData["WarningStreetlight"];
            var errorStreetlight = testData["ErrorStreetlight"];
            var controllerId = testData["ControllerId"];
            var controllerName = testData["ControllerName"];
            var geozoneName1 = geozone1.GetChildName();
            var geozone1DevicesCount = 3;

            Step("**** Precondition ****");
            Step(" - Login to the system successfully");          
            Step(" - Install the widget Geozone Failures Monitor");
            Step(" - Create a new geozone with 3 streetlights");
            Step("  + one is working OK (use the Technology Type: SSN Cimcon Dim Photocell[Lamp #0])");
            Step("  + one is having warning (High Voltage)");
            Step("  + one is having critical failure (Lamp Failure)");
            Step("**** Precondition ****\n");

            var request = SetValueToDevice(controllerId, warningStreetlight, "HighVoltage", true, Settings.GetServerTime());
            VerifyEqual(string.Format("-> Verify the request is sent successfully (attribute: {0}, value: {1})", "HighVoltage", true), true, request);
            request = SetValueToDevice(controllerId, errorStreetlight, "LampFailure", true, Settings.GetServerTime());
            VerifyEqual(string.Format("-> Verify the request is sent successfully (attribute: {0}, value: {1})", "LampFailure", true), true, request);

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.EquipmentInventory);
            desktopPage.DeleteWidgets(Widget.GeozoneFailuresMonitor);
            desktopPage.InstallWidgets(Widget.GeozoneFailuresMonitor);            
           
            Step("1. Navigate to the Geozone Failures Monitor widget");
            Step("2. Press Config button and select the testing geozone");
            desktopPage.ClickGeozoneFailuresMonitorSetting();
            desktopPage.WaitForPreviousActionComplete();
            desktopPage.WaitForFailuresMonitorSettingDisplayed();
            desktopPage.GeozoneTreeWidgetPanel.SelectNode(geozone1);
            desktopPage.WaitForFailuresMonitorSettingDisappeared();

            Step("3. Verify");
            Step(" o The testing geozone's name + 'Failures' displays at the bottom of the widget");
            Step(" o There is a circle containing 3 equal parts with 3 colors: Green, Orange, Red and surrounding a WHITE circle");            
            var options = desktopPage.GetGeozoneFailuresMonitorPieChartOptions();
            var info = desktopPage.GetGeozoneFailuresMonitorInfo();
            var properlyFunctionalOption = options.FirstOrDefault(p => p.Name.Equals("Properly functional"));
            var faultyDeviceslOption = options.FirstOrDefault(p => p.Name.Equals("Faulty devices"));
            var criticalFaultsOption = options.FirstOrDefault(p => p.Name.Equals("Critical faults"));
            var totalDevicesCount = options.Sum(p => p.Count);
            VerifyEqual("3. Verify The testing geozone's name displays at the bottom of the widget", geozoneName1, info.GeoZoneName);
            VerifyEqual("3. Verify The 'Failures' displays at the bottom of the widget", "Failures", desktopPage.GetGeozoneFailuresMonitorCaption());
            VerifyEqual("3. Verify There is a circle containing 3 equal parts with color: Green and surrounding a WHITE circle", true, totalDevicesCount == geozone1DevicesCount && properlyFunctionalOption.Count == 1 && properlyFunctionalOption.ColorHex.Equals("#00CC00"));
            VerifyEqual("3. Verify There is a circle containing 3 equal parts with color: Orange and surrounding a WHITE circle", true, totalDevicesCount == geozone1DevicesCount && faultyDeviceslOption.Count == 1 && faultyDeviceslOption.ColorHex.Equals("#FF8800"));
            VerifyEqual("[#1429535] 3. Verify There is a circle containing 3 equal parts with color: Red and surrounding a WHITE circle", true, totalDevicesCount == geozone1DevicesCount && criticalFaultsOption.Count == 1 && criticalFaultsOption.ColorHex.Equals("#FF0000"));

            Step("4. Mouse over Red part");
            desktopPage.HoverGeozoneFailuresMonitor(70, 120);            

            Step("5. Verify");
            Step(" o There are the number and percent of devices having Critical Failure in the WHITE circle. They are '33%' and '1 devices'");
            Step(" o The text is RED");
            var expectedBytes = desktopPage.GetGeozoneFailuresMonitorScreenshot("GFM04", "RED_33");
            var actualBytes = desktopPage.TakeGeozoneFailuresMonitorScreenshot();
            Verify2ChartsAsBytes(expectedBytes, actualBytes, "Hover RED - 33%");

            Step("6. Mouse over Orange part");
            desktopPage.HoverGeozoneFailuresMonitor(135, 210);               

            Step("7. Verify");
            Step(" o There are the number and percent of devices having Warnings in the WHITE circle. They are '33%' and '1 devices'");
            Step(" o The text is Orange");
            var expectedBytes1 = desktopPage.GetGeozoneFailuresMonitorScreenshot("GFM04", "ORANGE_33");
            var actualBytes1 = desktopPage.TakeGeozoneFailuresMonitorScreenshot();
            Verify2ChartsAsBytes(expectedBytes1, actualBytes1, "Hover ORANGE - 33%");

            Step("8. Mouse over Green part");
            desktopPage.HoverGeozoneFailuresMonitor(190, 120);            

            Step("9. Verify");
            Step(" o There are the number and percent of devices working OK in the WHITE circle. They are '33%' and '1 devices'");
            Step(" o The text is GREEN");
            var expectedBytes2 = desktopPage.GetGeozoneFailuresMonitorScreenshot("GFM04", "GREEN_33");
            var actualBytes2 = desktopPage.TakeGeozoneFailuresMonitorScreenshot();
            Verify2ChartsAsBytes(expectedBytes2, actualBytes2, "Hover GREEN - 33%");

            Step("10. Deselect the option 'Properly Functional'");
            desktopPage.ClickGeozoneFailuresMonitorOption("Properly functional");

            Step("11. Verify The system calculates the number and percent again.");
            Step(" o There is a circle containing 2 equal parts with 2 colors: Orange, Red and surrounding a WHITE circle");
            options = desktopPage.GetGeozoneFailuresMonitorPieChartOptions();
            properlyFunctionalOption = options.FirstOrDefault(p => p.Name.Equals("Properly functional"));
            var otherOptionsPercent = string.Format("{0}%", Math.Round(options.Where(p => !p.Name.Equals("Properly functional")).Sum(p => p.Percentage)) * 100);
            var otherOptionsDeviceCount = options.Where(p => !p.Name.Equals("Properly functional")).Sum(p => p.Count);
            VerifyEqual("11. There is a circle containing 2 equal parts with 2 colors: Orange, Red and surrounding a WHITE circle", true, properlyFunctionalOption.Percentage == 0);

            Step("12. Mouse over Red part");
            desktopPage.HoverGeozoneFailuresMonitor(70, 120);

            Step("13. Verify there are the number and percent of devices having Critical Failure in the WHITE circle. They are '50%' and '1 devices'");
            var expectedBytes3 = desktopPage.GetGeozoneFailuresMonitorScreenshot("GFM04", "RED_50");
            var actualBytes3 = desktopPage.TakeGeozoneFailuresMonitorScreenshot();
            Verify2ChartsAsBytes(expectedBytes3, actualBytes3, "Not Properly functional - Hover RED - 50%");

            Step("14. Mouse over Orange part");
            desktopPage.HoverGeozoneFailuresMonitor(190, 120);

            Step("15. Verify there are the number and percent of devices having Warnings in the WHITE circle. They are '50%' and '1 devices'");
            var expectedBytes4 = desktopPage.GetGeozoneFailuresMonitorScreenshot("GFM04", "ORANGE_50");
            var actualBytes4 = desktopPage.TakeGeozoneFailuresMonitorScreenshot();
            Verify2ChartsAsBytes(expectedBytes4, actualBytes4, "Not Properly functional - Hover ORANGE - 50%");

            Step("16. Deselect also the option 'Faulty devices'");
            desktopPage.ClickGeozoneFailuresMonitorOption("Faulty devices");

            Step("17. Verify");
            Step(" o There is a RED circle surrounding a WHITE circle");
            Step(" o In the WHITE circle there is the number and percent of devices having Critical Failure. They are '100%' and '1 Devices'");
            Step(" o The text in WHITE circle is RED");
            desktopPage.HoverGeozoneFailuresMonitor(70, 120);
            var expectedBytes5 = desktopPage.GetGeozoneFailuresMonitorScreenshot("GFM04", "RED_100");
            var actualBytes5 = desktopPage.TakeGeozoneFailuresMonitorScreenshot();
            Verify2ChartsAsBytes(expectedBytes5, actualBytes5, "Only Critical faults selected - RED - 100%");

            Step("18. Select options 'Faulty devices' & 'Properly Functional' again");
            desktopPage.ClickGeozoneFailuresMonitorOption("Faulty devices");
            desktopPage.ClickGeozoneFailuresMonitorOption("Properly functional");

            Step("19. Select Config button and select another geozone, then select back the testing geozone again");
            desktopPage.ClickGeozoneFailuresMonitorSetting();
            desktopPage.WaitForPreviousActionComplete();
            desktopPage.WaitForFailuresMonitorSettingDisplayed();
            desktopPage.GeozoneTreeWidgetPanel.SelectNode(geozone2);
            desktopPage.WaitForFailuresMonitorSettingDisappeared();

            desktopPage.ClickGeozoneFailuresMonitorSetting();
            desktopPage.WaitForPreviousActionComplete();
            desktopPage.WaitForFailuresMonitorSettingDisplayed();
            desktopPage.GeozoneTreeWidgetPanel.SelectNode(geozone1);
            desktopPage.WaitForFailuresMonitorSettingDisappeared();

            Step("20. Verify");
            Step(" o There is a circle containing 3 equal parts with 3 colors: Green, Orange, Red and surrounding a WHITE circle");
            options = desktopPage.GetGeozoneFailuresMonitorPieChartOptions();         
            properlyFunctionalOption = options.FirstOrDefault(p => p.Name.Equals("Properly functional"));
            faultyDeviceslOption = options.FirstOrDefault(p => p.Name.Equals("Faulty devices"));
            criticalFaultsOption = options.FirstOrDefault(p => p.Name.Equals("Critical faults"));
            totalDevicesCount = options.Sum(p => p.Count);
            VerifyEqual("20. Verify There is a circle containing 3 equal parts with color: Green and surrounding a WHITE circle", true, totalDevicesCount == geozone1DevicesCount && properlyFunctionalOption.Count == 1 && properlyFunctionalOption.ColorHex.Equals("#00CC00"));
            VerifyEqual("20. Verify There is a circle containing 3 equal parts with color: Orange and surrounding a WHITE circle", true, totalDevicesCount == geozone1DevicesCount && faultyDeviceslOption.Count == 1 && faultyDeviceslOption.ColorHex.Equals("#FF8800"));
            VerifyEqual("[#1429535] 20. Verify There is a circle containing 3 equal parts with color: Red and surrounding a WHITE circle", true, totalDevicesCount == geozone1DevicesCount && criticalFaultsOption.Count == 1 && criticalFaultsOption.ColorHex.Equals("#FF0000"));
            
            Step("21. Mouse over each part");
            Step("22. Verify The number and percent of devices changes color following each part and they are '33%' and '1 devices'");
            desktopPage.HoverGeozoneFailuresMonitor(70, 120);
            var expectedBytes6 = desktopPage.GetGeozoneFailuresMonitorScreenshot("GFM04", "RED_33");
            var actualBytes6 = desktopPage.TakeGeozoneFailuresMonitorScreenshot();
            Verify2ChartsAsBytes(expectedBytes6, actualBytes6, "All options selected - Hover RED - 33%");

            desktopPage.HoverGeozoneFailuresMonitor(135, 210);
            expectedBytes6 = desktopPage.GetGeozoneFailuresMonitorScreenshot("GFM04", "ORANGE_33");
            actualBytes6 = desktopPage.TakeGeozoneFailuresMonitorScreenshot();
            Verify2ChartsAsBytes(expectedBytes6, actualBytes6, "All options selected - Hover ORANGE - 33%");

            desktopPage.HoverGeozoneFailuresMonitor(190, 120);
            expectedBytes6 = desktopPage.GetGeozoneFailuresMonitorScreenshot("GFM04", "GREEN_33");
            actualBytes6 = desktopPage.TakeGeozoneFailuresMonitorScreenshot();
            Verify2ChartsAsBytes(expectedBytes6, actualBytes6, "All options selected - Hover GREEN - 33%");            
        }

        [Test, DynamicRetry]
        [Description("GFM_05 System calculates the number and percent of devices with different types in parent and sub geozone")]
        [NonParallelizable]
        public void GFM_05()
        {
            var testData = GetTestDataOfTestGFM_05();
            var controllerId = testData["ControllerId"].ToString();
            var controllerName = testData["ControllerName"].ToString();
            var geozoneParent = testData["GeozoneParent"].ToString();
            var geozoneSub = testData["GeozoneSub"].ToString();
            var warningStreetlights = testData["WarningStreetlights"] as List<string>;
            var errorStreetlights = testData["ErrorStreetlights"] as List<string>;
            var geozoneParentName = geozoneParent.GetChildName();
            var geozoneSubName = geozoneSub.GetChildName();
            var geozoneParentDevicesCount = 6;
            var geozoneSubDevicesCount = 3;

            Step("**** Precondition ****");
            Step(" - Login to the system successfully");
            Step(" - Install the widget Geozone Failures Monitor");
            Step(" - Create a new geozone with 3 streetlights");
            Step("  + one is working OK (use the Technology Type: SSN Cimcon Dim Photocell[Lamp #0])");
            Step("  + one is having warning (High Voltage)");
            Step("  + one is having critical failure (Lamp Failure)");
            Step(" - Create a sub geozone with 3 streetlights as above");
            Step("**** Precondition ****\n");

            foreach (var streetlight in warningStreetlights)
            {
                var request = SetValueToDevice(controllerId, streetlight, "HighVoltage", true, Settings.GetServerTime());
                VerifyEqual(string.Format("-> Verify the request is sent successfully (attribute: {0}, value: {1})", "HighVoltage", true), true, request);
            }
            foreach (var streetlight in errorStreetlights)
            {
                var request = SetValueToDevice(controllerId, streetlight, "LampFailure", true, Settings.GetServerTime());
                VerifyEqual(string.Format("-> Verify the request is sent successfully (attribute: {0}, value: {1})", "LampFailure", true), true, request);
            }

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.EquipmentInventory);
            desktopPage.DeleteWidgets(Widget.GeozoneFailuresMonitor);
            desktopPage.InstallWidgets(Widget.GeozoneFailuresMonitor);

            Step("1. Navigate to the Geozone Failures Monitor widget");
            Step("2. Press Config button and select the testing parent geozone");
            desktopPage.ClickGeozoneFailuresMonitorSetting();
            desktopPage.WaitForPreviousActionComplete();
            desktopPage.WaitForFailuresMonitorSettingDisplayed();
            desktopPage.GeozoneTreeWidgetPanel.SelectNode(geozoneParent);
            desktopPage.WaitForFailuresMonitorSettingDisappeared();

            Step("3. Verify");
            Step(" o The testing parent geozone's name + 'Failures' displays at the bottom of the widget");
            Step(" o There is a circle containing 3 equal parts with 3 colors: Green, Orange, Red and surrounding a WHITE circle");
            var options = desktopPage.GetGeozoneFailuresMonitorPieChartOptions();
            var info = desktopPage.GetGeozoneFailuresMonitorInfo();
            var properlyFunctionalOption = options.FirstOrDefault(p => p.Name.Equals("Properly functional"));
            var faultyDeviceslOption = options.FirstOrDefault(p => p.Name.Equals("Faulty devices"));
            var criticalFaultsOption = options.FirstOrDefault(p => p.Name.Equals("Critical faults"));
            var totalDevicesCount = options.Sum(p => p.Count);
            VerifyEqual("3. Verify The testing parent geozone's name displays at the bottom of the widget", geozoneParentName, info.GeoZoneName);
            VerifyEqual("3. Verify The 'Failures' displays at the bottom of the widget", "Failures", desktopPage.GetGeozoneFailuresMonitorCaption());
            VerifyEqual("3. Verify There is a circle containing 3 equal parts with color: Green and surrounding a WHITE circle", true, totalDevicesCount == geozoneParentDevicesCount && properlyFunctionalOption.Count == 2 && properlyFunctionalOption.ColorHex.Equals("#00CC00"));
            VerifyEqual("3. Verify There is a circle containing 3 equal parts with color: Orange and surrounding a WHITE circle", true, totalDevicesCount == geozoneParentDevicesCount && faultyDeviceslOption.Count == 2 && faultyDeviceslOption.ColorHex.Equals("#FF8800"));
            VerifyEqual("[#1429535] 3. Verify There is a circle containing 3 equal parts with color: Red and surrounding a WHITE circle", true, totalDevicesCount == geozoneParentDevicesCount && criticalFaultsOption.Count == 2 && criticalFaultsOption.ColorHex.Equals("#FF0000"));

            Step("4. Mouse over Red part");
            desktopPage.HoverGeozoneFailuresMonitor(70, 120);

            Step("5. Verify");
            Step(" o There are the number and percent of devices having Critical Failure in the WHITE circle. They are '33%' and '2 devices'");
            Step(" o The text is RED");
            var expectedBytes = desktopPage.GetGeozoneFailuresMonitorScreenshot("GFM05", "RED_33_2");
            var actualBytes = desktopPage.TakeGeozoneFailuresMonitorScreenshot();
            Verify2ChartsAsBytes(expectedBytes, actualBytes, "Hover RED - 33%");

            Step("6. Mouse over Orange part");
            desktopPage.HoverGeozoneFailuresMonitor(135, 210);

            Step("7. Verify");
            Step(" o There are the number and percent of devices having Warnings in the WHITE circle. They are '33%' and '2 devices'");
            Step(" o The text is Orange");
            var expectedBytes1 = desktopPage.GetGeozoneFailuresMonitorScreenshot("GFM05", "ORANGE_33_2");
            var actualBytes1 = desktopPage.TakeGeozoneFailuresMonitorScreenshot();
            Verify2ChartsAsBytes(expectedBytes1, actualBytes1, "Hover ORANGE - 33%");

            Step("8. Mouse over Green part");
            desktopPage.HoverGeozoneFailuresMonitor(190, 120);

            Step("9. Verify");
            Step(" o There are the number and percent of devices working OK in the WHITE circle. They are '33%' and '2 devices'");
            Step(" o The text is GREEN");
            var expectedBytes2 = desktopPage.GetGeozoneFailuresMonitorScreenshot("GFM05", "GREEN_33_2");
            var actualBytes2 = desktopPage.TakeGeozoneFailuresMonitorScreenshot();
            Verify2ChartsAsBytes(expectedBytes2, actualBytes2, "Hover GREEN - 33%");

            Step("10. Deselect the option 'Properly Functional'");
            desktopPage.ClickGeozoneFailuresMonitorOption("Properly functional");

            Step("11. Verify The system calculates the number and percent again.");
            Step(" o There is a circle containing 2 equal parts with 2 colors: Orange, Red and surrounding a WHITE circle");
            options = desktopPage.GetGeozoneFailuresMonitorPieChartOptions();
            properlyFunctionalOption = options.FirstOrDefault(p => p.Name.Equals("Properly functional"));
            var otherOptionsPercent = string.Format("{0}%", Math.Round(options.Where(p => !p.Name.Equals("Properly functional")).Sum(p => p.Percentage)) * 100);
            var otherOptionsDeviceCount = options.Where(p => !p.Name.Equals("Properly functional")).Sum(p => p.Count);
            VerifyEqual("11. There is a circle containing 2 equal parts with 2 colors: Orange, Red and surrounding a WHITE circle", true, properlyFunctionalOption.Percentage == 0);

            Step("12. Mouse over Red part");
            desktopPage.HoverGeozoneFailuresMonitor(70, 120);

            Step("13. Verify there are the number and percent of devices having Critical Failure in the WHITE circle. They are '50%' and '2 devices'");
            var expectedBytes3 = desktopPage.GetGeozoneFailuresMonitorScreenshot("GFM05", "RED_50_2");
            var actualBytes3 = desktopPage.TakeGeozoneFailuresMonitorScreenshot();
            Verify2ChartsAsBytes(expectedBytes3, actualBytes3, "Not Properly functional - Hover RED - 50%");

            Step("14. Mouse over Orange part");
            desktopPage.HoverGeozoneFailuresMonitor(190, 120);

            Step("15. Verify there are the number and percent of devices having Warnings in the WHITE circle. They are '50%' and '2 devices'");
            var expectedBytes4 = desktopPage.GetGeozoneFailuresMonitorScreenshot("GFM05", "ORANGE_50_2");
            var actualBytes4 = desktopPage.TakeGeozoneFailuresMonitorScreenshot();
            Verify2ChartsAsBytes(expectedBytes4, actualBytes4, "Not Properly functional - Hover ORANGE - 50%");

            Step("16. Deselect also the option 'Faulty devices'");
            desktopPage.ClickGeozoneFailuresMonitorOption("Faulty devices");

            Step("17. Verify");
            Step(" o There is a RED circle surrounding a WHITE circle");
            Step(" o In the WHITE circle there is the number and percent of devices having Critical Failure. They are '100%' and '2 Devices'");
            Step(" o The text in WHITE circle is RED");
            desktopPage.HoverGeozoneFailuresMonitor(70, 120);
            var expectedBytes5 = desktopPage.GetGeozoneFailuresMonitorScreenshot("GFM05", "RED_100_2");
            var actualBytes5 = desktopPage.TakeGeozoneFailuresMonitorScreenshot();
            Verify2ChartsAsBytes(expectedBytes5, actualBytes5, "Only Critical faults selected - RED - 100%");

            Step("18. Select options 'Faulty devices' & 'Properly Functional' again");
            desktopPage.ClickGeozoneFailuresMonitorOption("Faulty devices");
            desktopPage.ClickGeozoneFailuresMonitorOption("Properly functional");

            Step("19. Select Config button and select the sub geozone");
            desktopPage.ClickGeozoneFailuresMonitorSetting();
            desktopPage.WaitForPreviousActionComplete();
            desktopPage.WaitForFailuresMonitorSettingDisplayed();
            desktopPage.GeozoneTreeWidgetPanel.SelectNode(geozoneSub);
            desktopPage.WaitForFailuresMonitorSettingDisappeared();
            
            Step("20. Verify");
            Step(" o The testing sub geozone's name + 'Failures' displays at the bottom of the widget");
            Step(" o There is a circle containing 3 equal parts with 3 colors: Green, Orange, Red and surrounding a WHITE circle");
            options = desktopPage.GetGeozoneFailuresMonitorPieChartOptions();
            info = desktopPage.GetGeozoneFailuresMonitorInfo();
            properlyFunctionalOption = options.FirstOrDefault(p => p.Name.Equals("Properly functional"));
            faultyDeviceslOption = options.FirstOrDefault(p => p.Name.Equals("Faulty devices"));
            criticalFaultsOption = options.FirstOrDefault(p => p.Name.Equals("Critical faults"));
            totalDevicesCount = options.Sum(p => p.Count);
            VerifyEqual("20. Verify The testing sub geozone's name displays at the bottom of the widget", geozoneSubName, info.GeoZoneName);
            VerifyEqual("20. Verify The 'Failures' displays at the bottom of the widget", "Failures", desktopPage.GetGeozoneFailuresMonitorCaption());
            VerifyEqual("20. Verify There is a circle containing 3 equal parts with color: Green and surrounding a WHITE circle", true, totalDevicesCount == geozoneSubDevicesCount && properlyFunctionalOption.Count == 1 && properlyFunctionalOption.ColorHex.Equals("#00CC00"));
            VerifyEqual("20. Verify There is a circle containing 3 equal parts with color: Orange and surrounding a WHITE circle", true, totalDevicesCount == geozoneSubDevicesCount && faultyDeviceslOption.Count == 1 && faultyDeviceslOption.ColorHex.Equals("#FF8800"));
            VerifyEqual("[#1429535] 20. Verify There is a circle containing 3 equal parts with color: Red and surrounding a WHITE circle", true, totalDevicesCount == geozoneSubDevicesCount && criticalFaultsOption.Count == 1 && criticalFaultsOption.ColorHex.Equals("#FF0000"));

            Step("21. Mouse over each part");
            Step("22. Verify The number and percent of devices changes color following each part and they are '33%' and '1 devices'");
            desktopPage.HoverGeozoneFailuresMonitor(70, 120);
            var expectedBytes6 = desktopPage.GetGeozoneFailuresMonitorScreenshot("GFM05", "RED_33_1");
            var actualBytes6 = desktopPage.TakeGeozoneFailuresMonitorScreenshot();
            Verify2ChartsAsBytes(expectedBytes6, actualBytes6, "All options selected - Hover RED - 33%");

            desktopPage.HoverGeozoneFailuresMonitor(135, 210);
            expectedBytes6 = desktopPage.GetGeozoneFailuresMonitorScreenshot("GFM05", "ORANGE_33_1");
            actualBytes6 = desktopPage.TakeGeozoneFailuresMonitorScreenshot();
            Verify2ChartsAsBytes(expectedBytes6, actualBytes6, "All options selected - Hover ORANGE - 33%");

            desktopPage.HoverGeozoneFailuresMonitor(190, 120);
            expectedBytes6 = desktopPage.GetGeozoneFailuresMonitorScreenshot("GFM05", "GREEN_33_1");
            actualBytes6 = desktopPage.TakeGeozoneFailuresMonitorScreenshot();
            Verify2ChartsAsBytes(expectedBytes6, actualBytes6, "All options selected - Hover GREEN - 33%");

            Step("23. Select Config button and select the parent geozone again");
            desktopPage.ClickGeozoneFailuresMonitorSetting();
            desktopPage.WaitForPreviousActionComplete();
            desktopPage.WaitForFailuresMonitorSettingDisplayed();
            desktopPage.GeozoneTreeWidgetPanel.SelectNode(geozoneParent);
            desktopPage.WaitForFailuresMonitorSettingDisappeared();

            Step("24. Verify");
            Step(" o The testing parent geozone's name + 'Failures' displays at the bottom of the widget");
            Step(" o There is a circle containing 3 equal parts with 3 colors: Green, Orange, Red and surrounding a WHITE circle");
            options = desktopPage.GetGeozoneFailuresMonitorPieChartOptions();
            info = desktopPage.GetGeozoneFailuresMonitorInfo();
            properlyFunctionalOption = options.FirstOrDefault(p => p.Name.Equals("Properly functional"));
            faultyDeviceslOption = options.FirstOrDefault(p => p.Name.Equals("Faulty devices"));
            criticalFaultsOption = options.FirstOrDefault(p => p.Name.Equals("Critical faults"));
            totalDevicesCount = options.Sum(p => p.Count);
            VerifyEqual("24. Verify The testing parent geozone's name displays at the bottom of the widget", geozoneParentName, info.GeoZoneName);
            VerifyEqual("24. Verify The 'Failures' displays at the bottom of the widget", "Failures", desktopPage.GetGeozoneFailuresMonitorCaption());
            VerifyEqual("24. Verify There is a circle containing 3 equal parts with color: Green and surrounding a WHITE circle", true, totalDevicesCount == geozoneParentDevicesCount && properlyFunctionalOption.Count == 2 && properlyFunctionalOption.ColorHex.Equals("#00CC00"));
            VerifyEqual("24. Verify There is a circle containing 3 equal parts with color: Orange and surrounding a WHITE circle", true, totalDevicesCount == geozoneParentDevicesCount && faultyDeviceslOption.Count == 2 && faultyDeviceslOption.ColorHex.Equals("#FF8800"));
            VerifyEqual("24. Verify There is a circle containing 3 equal parts with color: Red and surrounding a WHITE circle", true, totalDevicesCount == geozoneParentDevicesCount && criticalFaultsOption.Count == 2 && criticalFaultsOption.ColorHex.Equals("#FF0000"));

            Step("25. Mouse over each part");
            Step("26. Verify The number and percent of devices changes color following each part and they are '33%' and '2 devices'");
            desktopPage.HoverGeozoneFailuresMonitor(70, 120);
            var expectedBytes7 = desktopPage.GetGeozoneFailuresMonitorScreenshot("GFM05", "RED_33_2");
            var actualBytes7 = desktopPage.TakeGeozoneFailuresMonitorScreenshot();
            Verify2ChartsAsBytes(expectedBytes7, actualBytes7, "All options selected - Hover RED - 33%");

            desktopPage.HoverGeozoneFailuresMonitor(135, 210);
            expectedBytes7 = desktopPage.GetGeozoneFailuresMonitorScreenshot("GFM05", "ORANGE_33_2");
            actualBytes7 = desktopPage.TakeGeozoneFailuresMonitorScreenshot();
            Verify2ChartsAsBytes(expectedBytes7, actualBytes7, "All options selected - Hover ORANGE - 33%");

            desktopPage.HoverGeozoneFailuresMonitor(190, 120);
            expectedBytes7 = desktopPage.GetGeozoneFailuresMonitorScreenshot("GFM05", "GREEN_33_2");
            actualBytes7 = desktopPage.TakeGeozoneFailuresMonitorScreenshot();
            Verify2ChartsAsBytes(expectedBytes7, actualBytes7, "All options selected - Hover GREEN - 33%");
        }

        #endregion //Test Cases

        #region Private methods

        private static void Verify2ChartsAsBytes(byte[] image1AsBytes, byte[] image2AsBytes, string message)
        {
            var graph1Image = new MagickImage(image1AsBytes);
            var graph2Image = new MagickImage(image2AsBytes);
            var result = graph1Image.Compare(graph2Image, ErrorMetric.MeanAbsolute);
            VerifyEqual(string.Format("Verify chart changed as expected ({0})", message), true, Math.Round(result) == 0);
        }

        private Dictionary<string, string> GetTestDataOfTestGFM_01()
        {
            var testCaseName = "GFM01";
            var xmlUtility = new XmlUtility(Settings.GFM_TEST_DATA_FILE_PATH);
            var testData = new Dictionary<string, string>();

            testData.Add("Geozone", xmlUtility.GetSingleNodeText(string.Format(Settings.GFM_XPATH_PREFIX, testCaseName, "Geozone")));
            testData.Add("Streetlight", xmlUtility.GetSingleNodeText(string.Format(Settings.GFM_XPATH_PREFIX, testCaseName, "Streetlight")));
            var controllerInfo = xmlUtility.GetSingleNode(string.Format(Settings.GFM_XPATH_PREFIX, testCaseName, "Controller"));
            testData.Add("ControllerId", controllerInfo.GetAttrVal("id"));
            testData.Add("ControllerName", controllerInfo.GetAttrVal("name"));

            return testData;
        }

        private Dictionary<string, string> GetTestDataOfTestGFM_02()
        {
            var testCaseName = "GFM02";
            var xmlUtility = new XmlUtility(Settings.GFM_TEST_DATA_FILE_PATH);
            var testData = new Dictionary<string, string>();

            testData.Add("Geozone", xmlUtility.GetSingleNodeText(string.Format(Settings.GFM_XPATH_PREFIX, testCaseName, "Geozone")));
            testData.Add("Streetlight", xmlUtility.GetSingleNodeText(string.Format(Settings.GFM_XPATH_PREFIX, testCaseName, "Streetlight")));
            var controllerInfo = xmlUtility.GetSingleNode(string.Format(Settings.GFM_XPATH_PREFIX, testCaseName, "Controller"));
            testData.Add("ControllerId", controllerInfo.GetAttrVal("id"));
            testData.Add("ControllerName", controllerInfo.GetAttrVal("name"));

            return testData;
        }

        private Dictionary<string, string> GetTestDataOfTestGFM_03()
        {
            var testCaseName = "GFM03";
            var xmlUtility = new XmlUtility(Settings.GFM_TEST_DATA_FILE_PATH);
            var testData = new Dictionary<string, string>();
            testData.Add("Geozone1", xmlUtility.GetSingleNodeText(string.Format(Settings.GFM_XPATH_PREFIX, testCaseName, "Geozone1")));
            testData.Add("Geozone2", xmlUtility.GetSingleNodeText(string.Format(Settings.GFM_XPATH_PREFIX, testCaseName, "Geozone2")));
            var controllerInfo = xmlUtility.GetSingleNode(string.Format(Settings.GFM_XPATH_PREFIX, testCaseName, "Controller"));
            testData.Add("ControllerId", controllerInfo.GetAttrVal("id"));
            testData.Add("ControllerName", controllerInfo.GetAttrVal("name"));
            return testData;
        }

        private Dictionary<string, string> GetTestDataOfTestGFM_04()
        {
            var testCaseName = "GFM04";
            var xmlUtility = new XmlUtility(Settings.GFM_TEST_DATA_FILE_PATH);
            var testData = new Dictionary<string, string>();
            testData.Add("Geozone1", xmlUtility.GetSingleNodeText(string.Format(Settings.GFM_XPATH_PREFIX, testCaseName, "Geozone1")));
            testData.Add("Geozone2", xmlUtility.GetSingleNodeText(string.Format(Settings.GFM_XPATH_PREFIX, testCaseName, "Geozone2")));
            testData.Add("WarningStreetlight", xmlUtility.GetSingleNodeText(string.Format(Settings.GFM_XPATH_PREFIX, testCaseName, "WarningStreetlight")));
            testData.Add("ErrorStreetlight", xmlUtility.GetSingleNodeText(string.Format(Settings.GFM_XPATH_PREFIX, testCaseName, "ErrorStreetlight")));
            var controllerInfo = xmlUtility.GetSingleNode(string.Format(Settings.GFM_XPATH_PREFIX, testCaseName, "Controller"));
            testData.Add("ControllerId", controllerInfo.GetAttrVal("id"));
            testData.Add("ControllerName", controllerInfo.GetAttrVal("name"));
            return testData;
        }

        private Dictionary<string, object> GetTestDataOfTestGFM_05()
        {
            var testCaseName = "GFM05";
            var xmlUtility = new XmlUtility(Settings.GFM_TEST_DATA_FILE_PATH);
            var testData = new Dictionary<string, object>();
            var controllerInfo = xmlUtility.GetSingleNode(string.Format(Settings.GFM_XPATH_PREFIX, testCaseName, "Controller"));
            testData.Add("GeozoneParent", xmlUtility.GetSingleNodeText(string.Format(Settings.GFM_XPATH_PREFIX, testCaseName, "GeozoneParent")));
            testData.Add("GeozoneSub", xmlUtility.GetSingleNodeText(string.Format(Settings.GFM_XPATH_PREFIX, testCaseName, "GeozoneSub")));
            testData.Add("WarningStreetlights", xmlUtility.GetChildNodesText(string.Format(Settings.GFM_XPATH_PREFIX, testCaseName, "WarningStreetlights")));
            testData.Add("ErrorStreetlights", xmlUtility.GetChildNodesText(string.Format(Settings.GFM_XPATH_PREFIX, testCaseName, "ErrorStreetlights")));
            testData.Add("ControllerId", controllerInfo.GetAttrVal("id"));
            testData.Add("ControllerName", controllerInfo.GetAttrVal("name"));
            return testData;
        }

        #endregion //Private methods
    }
}
