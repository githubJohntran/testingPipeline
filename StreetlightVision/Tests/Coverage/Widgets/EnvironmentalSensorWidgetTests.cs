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
    public class EnvironmentalSensorWidgetTests : TestBase
    {
        #region Variables
        
        #endregion //Variables

        #region Contructors

        #endregion //Contructors        

        #region Test Cases       

        [Test, DynamicRetry]
        [Description("EnvSensor_01 SC-378 Real Time Control - Add a new widget for Environmental Sensors")]
        [NonParallelizable]
        public void EnvSensor_01()
        {
            var testData = GetTestDataOfEnvSensor_01();

            var device = testData["Device"];
            var deviceName = device.GetChildName();

            Step("**** Precondition ****");
            Step(" - Login to the system successfully");
            Step(" - Create a testing geozone containing an Environmental Sensor device that uses Smartsims controller");
            Step("**** Precondition ****\n");          

            Step("1. Go to Desktop and install an Environmental Sensor widget");
            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.DeleteWidgets(Widget.EnvironmentalSensor);
            desktopPage.InstallWidgets(Widget.EnvironmentalSensor);

            Step("2. Verify A widget is added to the Desktop with");           
            Step(" - Setting button on the top right corner");
            Step(" - The text 'Select an environmental sensor to configure your widget' in the middle");
            Step(" - A sensor icon and the text 'Environmental Sensor' at the bottom left corner");
            var bottomIconUrl = desktopPage.GetEnvSensorBottomIcon();
            VerifyEqual("2. Verify Setting button on the top right corner", true, desktopPage.IsEnvSensorSettingButtonDisplayed());
            VerifyEqual("2. Verify The text 'Select an environmental sensor to configure your widget' in the middle", "Select an environmental sensor to configure your widget", desktopPage.GetEnvSensorHelpButtonText());
            VerifyEqual("2. Verify A sensor icon  at the bottom left corner", true, bottomIconUrl.Contains("environmental-sensor.png") && SLVHelper.IsServerFileExists(bottomIconUrl));
            VerifyEqual("2. Verify the text 'Environmental Sensor' at the bottom left corner", "Environmental Sensor", desktopPage.GetEnvSensorBottomTitle());
            
            Step("3. Press on the Setting button");
            desktopPage.ClickEnvSensorSettingButton();
            desktopPage.WaitForPreviousActionComplete();
            desktopPage.WaitForEnvSensorSettingDisplayed();

            Step("4. Verify The Environmental Sensor panel displays with");
            Step(" - Title: Environmental Sensor");
            Step(" - The geozone tree");
            VerifyEqual("4. Verify The Environmental Sensor panel displays with Title: Environmental Sensor", "Environmental Sensor", desktopPage.GetEnvSensorSettingHeaderText());
            VerifyEqual("4. Verify The Environmental Sensor panel displays with The geozone tree", true, desktopPage.IsEnvSensorSettingGeozonesDisplayed());

            Step("5. Press an empty place on the Desktop");
            desktopPage.AppBar.ClickHeaderBartop();
            desktopPage.WaitForEnvSensorSettingDisappeared();

            Step("6. Verify The Environmental Sensor panel is hidden");
            VerifyEqual("4. Verify The Environmental Sensor panel is hidden", true, !desktopPage.IsEnvSensorSettingDisplayed());

            Step("7. Press the text 'Select an environmental sensor to configure your widget'");
            desktopPage.ClickEnvSensorHelpButton();
            desktopPage.WaitForPreviousActionComplete();
            desktopPage.WaitForEnvSensorSettingDisplayed();

            Step("8. Verify The Environmental Sensor panel displays again");
            VerifyEqual("8. Verify The Environmental Sensor panel displays again", true, desktopPage.IsEnvSensorSettingDisplayed());

            Step("9. Select the testing Environmental Sensor device");
            desktopPage.GeozoneTreeWidgetPanel.SelectNode(device);
            desktopPage.WaitForPreviousActionComplete();
            desktopPage.WaitForEnvSensorSettingDisappeared();

            Step("10. Verify The widget is configured with the testing sensor");
            Step(" - Status:");
            Step("  + Green Check icon");
            Step("  + Text: Status");
            var statusIcon = desktopPage.GetEnvSensorStatusIcon();
            VerifyEqual("10. Verify Status with Green Check icon", true, statusIcon.Contains("status-ok.png") && SLVHelper.IsServerFileExists(statusIcon));
            VerifyEqual("10. Verify Status with Text: Status", "Status", desktopPage.GetEnvSensorStatusText());

            Step(" - Metering:");
            Step("  + Battery level: ##");
            Step("  + CO sensor: ##");
            Step("  + Lux level (Lux): ##");
            Step("  + Motion detection: Off");
            Step("  + NO2 sensor: ##");
            Step("  + Noise sensor: ##");
            Step("  + O3 sensor: ##");
            Step("  + PM10 sensor: ##");
            Step("  + Parking status: ##");
            Step("  + SO2 sensor: ##");
            Step("  + Tank filling %: ##");
            double value;
            var meterings = desktopPage.GetDictionaryOfEnvSensorMeterings();
            VerifyEqual("10. Verify Metering with Battery level: ##", true, double.TryParse(meterings["Battery level"], out value));
            VerifyEqual("10. Verify Metering with CO sensor: ##", true, double.TryParse(meterings["CO sensor"], out value));
            VerifyEqual("10. Verify Metering with Lux level (Lux): ##", true, double.TryParse(meterings["Lux level (Lux)"], out value));
            VerifyTrue("10. Verify Metering with Motion detection: On/Off", meterings["Motion detection"].Equals("On") || meterings["Motion detection"].Equals("Off"), "On/Off", meterings["Motion detection"]);
            VerifyEqual("10. Verify Metering with NO2 sensor: ##", true, double.TryParse(meterings["NO2 sensor"], out value));
            VerifyEqual("10. Verify Metering with Noise sensor: ##", true, double.TryParse(meterings["Noise sensor"], out value));
            VerifyEqual("10. Verify Metering with O3 sensor: ##", true, double.TryParse(meterings["O3 sensor"], out value));
            VerifyEqual("10. Verify Metering with PM10 sensor: ##", true, double.TryParse(meterings["PM10 sensor"], out value));
            VerifyEqual("10. Verify Metering with Parking status: ##", true, double.TryParse(meterings["Parking status"], out value));
            VerifyEqual("10. Verify Metering with SO2 sensor: ##", true, double.TryParse(meterings["SO2 sensor"], out value));
            VerifyEqual("10. Verify Metering with Tank filling %: ##", true, double.TryParse(meterings["Tank filling %"], out value));

            Step("11. Press Refresh button");
            desktopPage.ClickEnvSensorRefreshButton();
            var updatedMeterings = desktopPage.GetDictionaryOfEnvSensorMeterings();
            
            Step("12. Verify The values in the widget is updated");
            Step(" - Check some values: Battery level, CO sensor");
            VerifyTrue("12. Verify Metering - Battery level is updated", meterings["Battery level"] != updatedMeterings["Battery level"], meterings["Battery level"], updatedMeterings["Battery level"]);
            VerifyTrue("12. Verify Metering - CO sensor is updated", meterings["CO sensor"] != updatedMeterings["CO sensor"], meterings["CO sensor"], updatedMeterings["CO sensor"]);

            Step("13. Press Status tab");
            desktopPage.ClickEnvSensorStatusTab();
            desktopPage.WaitForEnvSensorStatusPanelDisplayed();

            Step("14. Verify A section displays");
            Step(" - Green Check icon + 'Communication failure'");
            Step(" - Green Check icon + 'Low battery'");
            var failures = desktopPage.GetDictionaryOfEnvSensorStatusFailures();
            var communicationFailureIcon = failures["Communication failure"];
            var lowBatteryIcon = failures["Low battery"];
            VerifyEqual("14. Verify Green Check icon + 'Communication failure'", true, communicationFailureIcon.Contains("status-ok.png") && SLVHelper.IsServerFileExists(communicationFailureIcon));
            VerifyEqual("14. Verify Green Check icon + 'Low battery'", true, lowBatteryIcon.Contains("status-ok.png") && SLVHelper.IsServerFileExists(lowBatteryIcon));

            Step("15. Press Status tab again");
            desktopPage.ClickEnvSensorStatusTab();
            desktopPage.WaitForEnvSensorStatusPanelDisappeared();

            Step("16. Verify The Status panel is closed");
            VerifyEqual("16. Verify Status panel is closed", true, !desktopPage.IsEnvSensorStatusPanelDisplayed());
        }

        #endregion //Test Cases

        #region Private methods

        private Dictionary<string, string> GetTestDataOfEnvSensor_01()
        {
            var testCaseName = "EnvSensor_01";
            var xmlUtility = new XmlUtility(Settings.ENV_SENSOR_TEST_DATA_FILE_PATH);
            var testData = new Dictionary<string, string>();            
            testData.Add("Device", xmlUtility.GetSingleNodeText(string.Format(Settings.ENV_SENSOR_XPATH_PREFIX, testCaseName, "Device")));

            return testData;
        }
        
        #endregion //Private methods
    }
}
