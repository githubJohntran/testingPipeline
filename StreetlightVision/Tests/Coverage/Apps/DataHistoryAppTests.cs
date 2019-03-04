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
using System.Text;
using System.Text.RegularExpressions;

namespace StreetlightVision.Tests.Coverage.Apps
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    public class DataHistoryAppTests : TestBase
    {
        #region Variables
        
        #endregion //Variables
        
        #region Test Cases

        [Test, DynamicRetry]
        [Description("DA_01 - Select a device from the geozone with diferent types of devices")]
        public void DA_01()
        {
            var testData = GetTestDataOfDA_01();
            var xmlGeozonePath = testData["Geozone"].ToString();
            var geozoneName = xmlGeozonePath.GetChildName();
            var expectedStreetlightMeterings = testData["StreetlightMeterings"] as List<string>;
            var expectedStreetlightFailures = testData["StreetlightFailures"] as List<string>;
            var expectedControllerMeterings = testData["ControllerMeterings"] as List<string>;
            var expectedControllerFailuress = testData["ControllerFailures"] as List<string>;
            var expectedMeterMeterings = testData["MeterMeterings"] as List<string>;

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - Create a new geozone containing the following devices: Streetlight, Controller, Meter");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.DataHistory);

            Step("1. Go to Data History app");            
            var dataHistoryPage = desktopPage.GoToApp(App.DataHistory) as DataHistoryPage;

            Step("2. Verify Data History page is routed and loaded successfully");
            Step(" o The title of the main panel is the name of the root. Ex: 'GeoZones'");
            VerifyEqual("Verify The title of the main panel is the name of the root. Ex: 'GeoZones'", Settings.RootGeozoneName, dataHistoryPage.GridPanel.GetPanelTitleText());
            
            Step("3. Go to the geozone from the precondition");
            dataHistoryPage.GeozoneTreeMainPanel.SelectNode(xmlGeozonePath);
            dataHistoryPage.GridPanel.WaitForGridContentAvailable();
            var streetlightName = dataHistoryPage.GeozoneTreeMainPanel.GetChildNodeNamesOfSelectedNode(NodeType.Streetlight).FirstOrDefault();
            var controllerName = dataHistoryPage.GeozoneTreeMainPanel.GetChildNodeNamesOfSelectedNode(NodeType.Controller).FirstOrDefault();
            var meterName = dataHistoryPage.GeozoneTreeMainPanel.GetChildNodeNamesOfSelectedNode(NodeType.ElectricalCounter).FirstOrDefault();

            Step("4. Verify The title of the main panel is the name of the selected geozone. Ex: 'GeoZones'");
            VerifyEqual("4. Verify The title of the main panel is the name of the selected geozone. Ex: 'GeoZones'", geozoneName, dataHistoryPage.GridPanel.GetPanelTitleText());

            Step("5. Click on the streetlight");
            dataHistoryPage.GeozoneTreeMainPanel.SelectNode(streetlightName);
            dataHistoryPage.WaitForLastValuePanelDisplayed();

            Step("6. Verify The panel appears with 2 tabs: Meterings & Failures containing all attributes of streetlight.");
            Step(" o Meterings: there are 14 attributes");
            Step("  + Active energy (KWh)");
            Step("  + Lamp burning hours");
            Step("  + Lamp command mode");
            Step("  + Lamp current");
            Step("  + Lamp level command");
            Step("  + Lamp level feedback");
            Step("  + Lamp switch command");
            Step("  + Lamp switch feedback");
            Step("  + Lamp voltage (V)");
            Step("  + Mains current");
            Step("  + Mains voltage (V)");
            Step("  + Metered power (W)");
            Step("  + Sum power factor");
            Step("  + Temperature");
            Step(" o Failures: there are 6 attributes");
            Step("  + Communication failure");
            Step("  + Driver temperature too high");
            Step("  + External comms failure");
            Step("  + High voltage");
            Step("  + Lamp failure");
            Step("  + Lamp restart count");
            VerifyEqual("6. Verify The panel appears with 2 tabs: Meterings & Failures", new List<string> { "Meterings", "Failures" }, dataHistoryPage.LastValuesPanel.GetListOfTabs());
            dataHistoryPage.LastValuesPanel.SelectTab("Meterings");
            VerifyEqual("[SC-1214] 6. Verify Meterings: there are 14 attributes as expected", expectedStreetlightMeterings, dataHistoryPage.LastValuesPanel.GetMeteringsNameList(), false);
            dataHistoryPage.LastValuesPanel.SelectTab("Failures");
            VerifyEqual("6. Verify Failures: there are 6 attributes as expected", expectedStreetlightFailures, dataHistoryPage.LastValuesPanel.GetFailuresNameList(), false);

            Step("7. Close the panel");
            dataHistoryPage.LastValuesPanel.ClickBackToolbarButton();
            dataHistoryPage.WaitForLastValuePanelDisappeared();

            Step("8. Expected The panel is closed");
            VerifyEqual("8. Verify The panel is closed", false, dataHistoryPage.IsLastValuePanelDisplayed());

            Step("9. Click on the Meter");
            dataHistoryPage.GeozoneTreeMainPanel.SelectNode(meterName);
            dataHistoryPage.WaitForLastValuePanelDisplayed();

            Step("10. Verify The panel appears with 1 tab: Meterings containing all attributes of meter");
            Step(" o Meterings: there are 27 attributes");
            Step("  + Active energy (KWh)");
            Step("  + Active Energy-L1(kWh)");
            Step("  + Active Energy-L2(kWh)");
            Step("  + Active Energy-L3(kWh)");
            Step("  + Active power (W)");
            Step("  + Apparent Power-L1(VA)");
            Step("  + Apparent Power-L2(VA)");
            Step("  + Apparent Power-L3(VA)");
            Step("  + Current - L1 (A)");
            Step("  + Current - L2 (A)");
            Step("  + Current - L3 (A)");
            Step("  + Frequency (Hz)");
            Step("  + Power Factor - L1");
            Step("  + Power factor - L2");
            Step("  + Power factor - L3");
            Step("  + Reactive Power-L1(VAR)");
            Step("  + Reactive Power-L2(VAR)");
            Step("  + Reactive Power-L3(VAR)");
            Step("  + Sum apparent power (VA)");
            Step("  + Sum reactive power (VAR)");
            Step("  + Total reactive energy (kVARh)");
            Step("  + Voltage - L1 (V)");
            Step("  + Voltage - L2 (V)");
            Step("  + Voltage - L3 (V)");
            Step("  + Voltage L1-L2 (V)");
            Step("  + Voltage L2-L3 (V)");
            Step("  + Voltage L3-L1 (V)");
            VerifyEqual("10. Verify The panel appears with 1 tab: Meterings", new List<string> { "Meterings" }, dataHistoryPage.LastValuesPanel.GetListOfTabs());
            dataHistoryPage.LastValuesPanel.SelectTab("Meterings");
            VerifyEqual("10. Verify Meterings: there are 27 attributes as expected", expectedMeterMeterings, dataHistoryPage.LastValuesPanel.GetMeteringsNameList(), false);

            Step("11. Close the panel");
            dataHistoryPage.LastValuesPanel.ClickBackToolbarButton();
            dataHistoryPage.WaitForLastValuePanelDisappeared();

            Step("12. Expected The panel is closed");
            VerifyEqual("12. Verify The panel is closed", false, dataHistoryPage.IsLastValuePanelDisplayed());

            Step("13. Click on the Controller");
            dataHistoryPage.GeozoneTreeMainPanel.SelectNode(controllerName);
            dataHistoryPage.WaitForLastValuePanelDisplayed();

            Step("14. Verify The panel appears with 2 tabs: Meterings and Failures containing all attributes of meter");
            Step(" o Meterings: there are 19 attributes");
            Step("  + 1-Modbus input 1");
            Step("  + 2-Modbus input 2");
            Step("  + 3-Modbus input 3");
            Step("  + 4-Modbus input 4");
            Step("  + 5-Modbus input 5");
            Step("  + 6-Modbus input 6");
            Step("  + 7-Modbus input 7");
            Step("  + 8-Modbus input 8");
            Step("  + 9-Modbus input 9");
            Step("  + 10-Modbus input 10");
            Step("  + 11-Modbus input 11");
            Step("  + 12-Modbus input 12");
            Step("  + Analog input 1");
            Step("  + Analog input 2");
            Step("  + Analog input 3");
            Step("  + Digital Input 1");
            Step("  + Digital Input 2");
            Step("  + Digital Output 1");
            Step("  + Digital Output 2");
            Step(" o Failures: there are 14 attributes");
            Step("  + Manual mode Output1");
            Step("  + Manual mode Output2");
            Step("  + Segment 1 - Failure");
            Step("  + Segment 2 - Failure");
            Step("  + Segment 3 - Failure");
            Step("  + Segment 4 - Failure");
            Step("  + Segment 5 - Failure");
            Step("  + Segment 6 - Failure");
            Step("  + Segment 7 - Failure");
            Step("  + Segment 8 - Failure");
            Step("  + Segment 9 - Failure");
            Step("  + Segment 10 - Failure");
            Step("  + Segment 11 - Failure");
            Step("  + Segment 12 - Failure");
            VerifyEqual("14. Verify The panel appears with 2 tabs: Meterings & Failures", new List<string> { "Meterings", "Failures" }, dataHistoryPage.LastValuesPanel.GetListOfTabs());
            dataHistoryPage.LastValuesPanel.SelectTab("Meterings");
            VerifyEqual("14. Verify Meterings: there are 19 attributes as expected", expectedControllerMeterings, dataHistoryPage.LastValuesPanel.GetMeteringsNameList(), false);
            dataHistoryPage.LastValuesPanel.SelectTab("Failures");
            VerifyEqual("14. Verify Failures: there are 14 attributes as expected", expectedControllerFailuress, dataHistoryPage.LastValuesPanel.GetFailuresNameList(), false);
            
            Step("15. Close the panel");
            dataHistoryPage.LastValuesPanel.ClickBackToolbarButton();
            dataHistoryPage.WaitForLastValuePanelDisappeared();

            Step("16. Expected The panel is closed");
            VerifyEqual("12. Verify The panel is closed", false, dataHistoryPage.IsLastValuePanelDisplayed());
        }

        [Test, DynamicRetry]
        [Description("DA_02 - The data and the time displayed for Metering attributes")]
        public void DA_02()
        {
            var testData = GetTestDataOfDA_02();
            var olsonTimeZoneId = testData["OlsonTimeZoneId"];
            var geozone = SLVHelper.GenerateUniqueName("GZNDA02");
            var controller = SLVHelper.GenerateUniqueName("CTRL");
            var streetlight = SLVHelper.GenerateUniqueName("STL");
            var meter = SLVHelper.GenerateUniqueName("MTR");

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - Create new controller with specific timezone");
            Step(" - Create following devices: Streetlight, Meter using above controller");
            Step(" - Prepare the command to simulate data for attributes of Meterings. Refer to the below table for Attribute Name and Parrameter.");
            Step("  • Lamp level command: LampCommandLevel");
            Step("  • Lamp command mode: LampCommandMode");
            Step("  • Mains current: Current");
            Step("  • Mains voltage (V): MainVoltage");
            Step("  • Metered power (W): MeteredPower");
            Step("  • Temperature: Temperature");
            Step("  • Active energy (KWh): Energy");
            Step("  • Lamp burning hours: RunningHoursLamp");
            Step("  • Lamp current: LampCurrent");
            Step("  • Lamp level feedback: LampLevel");
            Step("  • Lamp switch command: LampCommandSwitch");
            Step("  • Lamp switch feedback: LampSwitch: ON/OFF");
            Step("  • Lamp voltage (V): LampVoltage");
            Step("  • Sum power factor: PowerFactor");
            Step("**** Precondition ****\n");

            Step("-> Create data for testing");
            DeleteGeozones("GZNDA02*");
            CreateNewGeozone(geozone);
            CreateNewController(controller, geozone);
            SetValueToController(controller, "TimeZoneId", olsonTimeZoneId, Settings.GetServerTime());
            CreateNewDevice(DeviceType.Streetlight, streetlight, controller, geozone, "ABEL-Vigilon A[Dimmable ballast]");
            CreateNewDevice(DeviceType.ElectricalCounter, meter, controller, geozone);
            
            var attributes = new List<string>() { "LampCommandLevel", "LampCommandMode", "Current", "MainVoltage", "MeteredPower", "Temperature", "Energy", "RunningHoursLamp", "LampCurrent", "LampLevel", "LampCommandSwitch", "LampSwitch", "LampVoltage", "PowerFactor" };
            var dicDataCommand = new Dictionary<string, string>();
            foreach (var attribute in attributes)
            {
                var randomValue = SLVHelper.GenerateInteger(999).ToString("N2");
                if (attribute.Equals("LampSwitch")) randomValue = new List<string> { "ON", "OFF" }.PickRandom();
                dicDataCommand.Add(attribute, randomValue);
            }
            
            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.DataHistory);

            Step("1. Go to Data History app");
            var dataHistoryPage = desktopPage.GoToApp(App.DataHistory) as DataHistoryPage;

            Step("2. Go to the geozone from the precondition");
            dataHistoryPage.GeozoneTreeMainPanel.SelectNode(geozone);
            dataHistoryPage.GridPanel.WaitForGridContentAvailable();
            
            Step("3. Click on the streetlight");
            dataHistoryPage.GeozoneTreeMainPanel.SelectNode(streetlight);
            dataHistoryPage.WaitForLastValuePanelDisplayed();

            Step("4. Send 5 commands to simulate the data for 5 random attributes of Metering with the following eventTime");
            Step(" o eventTime=The current date time of controller");
            Step(" o eventTime=The current date time of controller - 10 minutes");
            Step(" o eventTime=The current date time of controller - 1 hour");
            Step(" o eventTime=The current date time of controller - 24 hours");
            Step(" o eventTime=The current date time of controller - 32 days");
            Step("5. Verify All 5 commands sent OK");
            var eventTime = Settings.GetCurrentControlerDateTime(controller);
            var data = dicDataCommand.PickRandom();
            var attribute1 = data.Key;
            var value1 = data.Value;
            var request1 = SetValueToDevice(controller, streetlight, attribute1, value1, eventTime);
            VerifyEqual(string.Format("5. Verify the request 1 is sent successfully (attribute: {0}, value: {1})", attribute1, value1), true, request1);

            dicDataCommand.Remove(attribute1);
            data = dicDataCommand.PickRandom();
            var attribute2 = data.Key;
            var value2 = data.Value;
            var request2 = SetValueToDevice(controller, streetlight, attribute2, value2, eventTime.AddMinutes(-10));
            VerifyEqual(string.Format("5. Verify the request 2 is sent successfully (attribute: {0}, value: {1})", attribute2, value2), true, request2);

            dicDataCommand.Remove(attribute2);
            data = dicDataCommand.PickRandom();
            var attribute3 = data.Key;
            var value3 = data.Value;
            var request3 = SetValueToDevice(controller, streetlight, attribute3, value3, eventTime.AddHours(-1));
            VerifyEqual(string.Format("5. Verify the request 3 is sent successfully (attribute: {0}, value: {1})", attribute3, value3), true, request3);

            dicDataCommand.Remove(attribute3);
            data = dicDataCommand.PickRandom();
            var attribute4 = data.Key;
            var value4 = data.Value;
            var request4 = SetValueToDevice(controller, streetlight, attribute4, value4, eventTime.AddHours(-24));
            VerifyEqual(string.Format("5. Verify the request 4 is sent successfully (attribute: {0}, value: {1})", attribute4, value4), true, request4);

            dicDataCommand.Remove(attribute4);
            data = dicDataCommand.PickRandom();
            var attribute5 = data.Key;
            var value5 = data.Value;
            var request5 = SetValueToDevice(controller, streetlight, attribute5, value5, eventTime.AddDays(-32));
            VerifyEqual(string.Format("5. Verify the request 5 is sent successfully (attribute: {0}, value: {1})", attribute5, value5), true, request5);
            
            Step("6. Press Back to the geozone, select another device, press back, then select the testing streetlight again to refresh data");
            dataHistoryPage.LastValuesPanel.ClickBackToolbarButton();
            dataHistoryPage.WaitForLastValuePanelDisappeared();
            dataHistoryPage.GeozoneTreeMainPanel.SelectNode(meter);
            dataHistoryPage.WaitForLastValuePanelDisplayed();
            dataHistoryPage.LastValuesPanel.ClickBackToolbarButton();
            dataHistoryPage.WaitForLastValuePanelDisappeared();
            dataHistoryPage.GeozoneTreeMainPanel.SelectNode(streetlight);
            dataHistoryPage.WaitForLastValuePanelDisplayed();
            
            Step("7. Verify For 5 testing attributes the data and time are displayed as 2 following columns");
            Step(" o Data = testing data from the command (ex: 100.00), Time = 1 s");
            Step(" o Data = testing data from the command, Time = 10 mn (current time - 10 minutes)");
            Step(" o Data = testing data from the command, Time = 1 h (current time - 1 hour)");
            Step(" o Data = testing data from the command, Time = 1 d (current time - 24 hours)");
            Step(" o Data = testing data from the command, Time = 32 d (current time - 32 days)");
            VerifyMeteringAttributeBetween(dataHistoryPage, attribute1, value1, 0, 5);
            VerifyMeteringAttributeBetween(dataHistoryPage, attribute2, value2, 5, 15);
            VerifyMeteringAttribute(dataHistoryPage, attribute3, value3, "1 h");            
            VerifyMeteringAttribute(dataHistoryPage, attribute4, value4, "1 d");
            VerifyMeteringAttribute(dataHistoryPage, attribute5, value5, "32 d");

            try
            {
                DeleteGeozone(geozone);
            }
            catch { }
        }       

        [Test, DynamicRetry]
        [Description("DA_03 - The data and the time displayed for Failures attributes")]
        public void DA_03()
        {
            var testData = GetTestDataOfDA_03();
            var olsonTimeZoneId = testData["OlsonTimeZoneId"];
            var geozone = SLVHelper.GenerateUniqueName("GZNDA03");
            var controller = SLVHelper.GenerateUniqueName("CTRL");
            var streetlight = SLVHelper.GenerateUniqueName("STL");
            var meter = SLVHelper.GenerateUniqueName("MTR");

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - Create a new controler with specific timezone");
            Step(" - Create a new streetlight using above controller");
            Step(" - Prepare the command to simulate data for attributes of Failures");
            Step(" - Refer to the below table for Attribute Name and Parrameter.");
            Step("  • Communication failure: DefaultLostNode");
            Step("  • Driver temperature too high: HighBallastTemperature");
            Step("  • External comms failure: ExternalComFailure");
            Step("  • High voltage: HighVoltage");
            Step("  • Lamp failure: LampFailure");
            Step("  • Lamp restart count: LampRestartCount");
            Step("**** Precondition ****\n");

            Step("-> Create data for testing");
            DeleteGeozones("GZNDA03*");
            CreateNewGeozone(geozone);
            CreateNewController(controller, geozone);
            SetValueToController(controller, "TimeZoneId", olsonTimeZoneId, Settings.GetServerTime());
            CreateNewDevice(DeviceType.Streetlight, streetlight, controller, geozone, "ABEL-Vigilon A[Dimmable ballast]");
            CreateNewDevice(DeviceType.ElectricalCounter, meter, controller, geozone);

            var attributes = new List<string>() { "Driver temperature too high:HighBallastTemperature", "External comms failure:ExternalComFailure", "High voltage:HighVoltage", "Lamp failure:LampFailure", "Lamp restart count:LampRestartCount" };
            var randomDataCommands = new Dictionary<string, string>();
            foreach (var attribute in attributes)
            {
                var randomValue = SLVHelper.GenerateInteger(999).ToString("N2");
                randomDataCommands.Add(attribute, randomValue);
            }

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.DataHistory);

            Step("1. Go to Data History app");
            var dataHistoryPage = desktopPage.GoToApp(App.DataHistory) as DataHistoryPage;

            Step("2. Go to the geozone from the precondition");
            dataHistoryPage.GeozoneTreeMainPanel.SelectNode(geozone);
            dataHistoryPage.GridPanel.WaitForGridContentAvailable();

            Step("3. Click on the streetlight");
            dataHistoryPage.GeozoneTreeMainPanel.SelectNode(streetlight);
            dataHistoryPage.WaitForLastValuePanelDisplayed();
            dataHistoryPage.LastValuesPanel.SelectTab("Failures");

            Step("4. Send 5 commands to simulate the data for 5 random attributes of Failures with the following eventTime");
            Step(" o eventTime=The current date time of controller");
            Step(" o eventTime=The current date time of controller - 10 minutes");
            Step(" o eventTime=The current date time of controller - 1 hour");
            Step(" o eventTime=The current date time of controller - 24 hours");
            Step(" o eventTime=The current date time of controller - 32 days");
            Step("5. Verify All 5 commands sent OK");
            var attributesData = new Dictionary<string, DateTime>();
            var eventTime = Settings.GetCurrentControlerDateTime(controller);
            var data = randomDataCommands.PickRandom();
            var attributeName1 = data.Key.SplitAndGetAt(new char[] { ':' }, 0);
            var attributeKey1 = data.Key.SplitAndGetAt(new char[] { ':' }, 1);
            var value1 = data.Value;
            var sentTime = eventTime;
            attributesData.Add(attributeName1, sentTime);
            var request1 = SetValueToDevice(controller, streetlight, attributeKey1, value1, sentTime);
            VerifyEqual(string.Format("5. Verify the request 1 is sent successfully (attribute: {0}, value: {1})", attributeName1, value1), true, request1);

            randomDataCommands.Remove(data.Key);
            data = randomDataCommands.PickRandom();
            var attributeName2 = data.Key.SplitAndGetAt(new char[] { ':' }, 0);
            var attributeKey2 = data.Key.SplitAndGetAt(new char[] { ':' }, 1);
            var value2 = data.Value;
            sentTime = eventTime.AddMinutes(-10);
            attributesData.Add(attributeName2, sentTime);
            var request2 = SetValueToDevice(controller, streetlight, attributeKey2, value2, sentTime);
            VerifyEqual(string.Format("5. Verify the request 2 is sent successfully (attribute: {0}, value: {1})", attributeName2, value2), true, request2);

            randomDataCommands.Remove(data.Key);
            data = randomDataCommands.PickRandom();
            var attributeName3 = data.Key.SplitAndGetAt(new char[] { ':' }, 0);
            var attributeKey3 = data.Key.SplitAndGetAt(new char[] { ':' }, 1);
            var value3 = data.Value;
            sentTime = eventTime.AddHours(-1);
            attributesData.Add(attributeName3, sentTime);
            var request3 = SetValueToDevice(controller, streetlight, attributeKey3, value3, sentTime);
            VerifyEqual(string.Format("5. Verify the request 3 is sent successfully (attribute: {0}, value: {1})", attributeName3, value3), true, request3);

            randomDataCommands.Remove(data.Key);
            data = randomDataCommands.PickRandom();
            var attributeName4 = data.Key.SplitAndGetAt(new char[] { ':' }, 0);
            var attributeKey4 = data.Key.SplitAndGetAt(new char[] { ':' }, 1);
            var value4 = data.Value;
            sentTime = eventTime.AddHours(-24);
            attributesData.Add(attributeName4, sentTime);
            var request4 = SetValueToDevice(controller, streetlight, attributeKey4, value4, sentTime);
            VerifyEqual(string.Format("5. Verify the request 4 is sent successfully (attribute: {0}, value: {1})", attributeName4, value4), true, request4);

            randomDataCommands.Remove(data.Key);
            data = randomDataCommands.PickRandom();
            var attributeName5 = data.Key.SplitAndGetAt(new char[] { ':' }, 0);
            var attributeKey5 = data.Key.SplitAndGetAt(new char[] { ':' }, 1);
            var value5 = data.Value;
            sentTime = eventTime.AddDays(-32);
            attributesData.Add(attributeName5, sentTime);
            var request5 = SetValueToDevice(controller, streetlight, attributeKey5, value5, sentTime);
            VerifyEqual(string.Format("5. Verify the request 5 is sent successfully (attribute: {0}, value: {1})", attributeName5, value5), true, request5);

            Step("6. Press Back to the geozone, select another device, press back, then select the testing streetlight again to refresh data");
            dataHistoryPage.LastValuesPanel.ClickBackToolbarButton();
            dataHistoryPage.WaitForLastValuePanelDisappeared();
            dataHistoryPage.GeozoneTreeMainPanel.SelectNode(meter);
            dataHistoryPage.WaitForLastValuePanelDisplayed();
            dataHistoryPage.LastValuesPanel.ClickBackToolbarButton();
            dataHistoryPage.WaitForLastValuePanelDisappeared();
            dataHistoryPage.GeozoneTreeMainPanel.SelectNode(streetlight);
            dataHistoryPage.WaitForLastValuePanelDisplayed();
            dataHistoryPage.LastValuesPanel.SelectTab("Failures");

            Step("7. Verify For 5 testing attributes the data and time are displayed as 2 following columns");
            Step(" o Warning icon, Time = 1 s");
            Step(" o Warning icon, Time = 10 mn (current time - 10 minutes)");
            Step(" o Warning icon, Time = 1 h (current time - 1 hour)");
            Step(" o Warning icon, Time = 1 d (current time - 24 hours)");
            Step(" o Warning icon, Time = 32 d (current time - 32 days)");
            Step("* Note: except for 'Lamp restart count' instead of Warning icon, the Error icon is displayed");          
            VerifyFailureAttributeWarningBetween(dataHistoryPage, attributeKey1, 0, 5);
            VerifyFailureAttributeWarningBetween(dataHistoryPage, attributeKey2, 5, 15);
            VerifyFailureAttributeWarning(dataHistoryPage, attributeKey3, "1 h");
            VerifyFailureAttributeWarning(dataHistoryPage, attributeKey4, "1 d");
            VerifyFailureAttributeWarning(dataHistoryPage, attributeKey5, "32 d");

            Step("8. Hover an attribute");
            var attributeData = attributesData.PickRandom();
            var expectedValue = attributeData.Value.ToString("M/d/yyyy H:mm tt");
            dataHistoryPage.LastValuesPanel.MoveHoverFailuresAttribute(attributeData.Key);
            var tooltipValue = DateTime.Parse(dataHistoryPage.LastValuesPanel.GetFailuresTooltipAttribute(attributeData.Key)).ToString("M/d/yyyy H:mm tt");

            Step("9. Verify A tooltip displays the date and time data is simulated. Ex: 12/27/2017 4:40 AM");
            VerifyEqual("9. Verify A tooltip displays the date and time data is simulated. Ex: 12/27/2017 4:40 AM", expectedValue, tooltipValue);

            Step("10. Send 5 commands to simulate the data for 5 random attributes of Failures with the following eventTime, and value=false");
            Step(" o eventTime=The current date time of controller");
            Step(" o eventTime=The current date time of controller - 10 minutes");
            Step(" o eventTime=The current date time of controller - 1 hour");
            Step(" o eventTime=The current date time of controller - 24 hours");
            Step(" o eventTime=The current date time of controller - 32 days");
            Step("11. Verify All 5 commands sent OK");
            attributesData = new Dictionary<string, DateTime>();
            eventTime = Settings.GetCurrentControlerDateTime(controller);
            sentTime = eventTime;
            attributesData.Add(attributeName1, sentTime);
            request1 = SetValueToDevice(controller, streetlight, attributeKey1, false, sentTime);
            VerifyEqual(string.Format("11. Verify the request 1 is sent successfully (attribute: {0}, value: {1})", attributeName1, "false"), true, request1);
     
            sentTime = eventTime.AddMinutes(-10);
            attributesData.Add(attributeName2, sentTime);
            request2 = SetValueToDevice(controller, streetlight, attributeKey2, false, sentTime);
            VerifyEqual(string.Format("11. Verify the request 2 is sent successfully (attribute: {0}, value: {1})", attributeName2, "false"), true, request2);
            
            sentTime = eventTime.AddHours(-1);
            attributesData.Add(attributeName3, sentTime);
            request3 = SetValueToDevice(controller, streetlight, attributeKey3, false, sentTime);
            VerifyEqual(string.Format("11. Verify the request 3 is sent successfully (attribute: {0}, value: {1})", attributeName3, "false"), true, request3);
            
            sentTime = eventTime.AddHours(-24);
            attributesData.Add(attributeName4, sentTime);
            request4 = SetValueToDevice(controller, streetlight, attributeKey4, false, sentTime);
            VerifyEqual(string.Format("11. Verify the request 4 is sent successfully (attribute: {0}, value: {1})", attributeName4, "false"), true, request4);
            
            sentTime = eventTime.AddDays(-32);
            attributesData.Add(attributeName5, sentTime);
            request5 = SetValueToDevice(controller, streetlight, attributeKey5, false, sentTime);
            VerifyEqual(string.Format("11. Verify the request 5 is sent successfully (attribute: {0}, value: {1})", attributeName5, "false"), true, request5);

            Step("12. Press Back to the geozone, select another device, press back, then select the testing streetlight again to refresh data");
            dataHistoryPage.LastValuesPanel.ClickBackToolbarButton();
            dataHistoryPage.WaitForLastValuePanelDisappeared();
            dataHistoryPage.GeozoneTreeMainPanel.SelectNode(meter);
            dataHistoryPage.WaitForLastValuePanelDisplayed();
            dataHistoryPage.LastValuesPanel.ClickBackToolbarButton();
            dataHistoryPage.WaitForLastValuePanelDisappeared();
            dataHistoryPage.GeozoneTreeMainPanel.SelectNode(streetlight);
            dataHistoryPage.WaitForLastValuePanelDisplayed();
            dataHistoryPage.LastValuesPanel.SelectTab("Failures");

            Step("13. Verify For 5 testing attributes the data and time are displayed as 2 following columns");
            Step(" o Checked icon, Time = 1 s");
            Step(" o Checked icon, Time = 10 mn (current time - 10 minutes)");
            Step(" o Checked icon, Time = 1 h (current time - 1 hour)");
            Step(" o Checked icon, Time = 1 d (current time - 24 hours)");
            Step(" o Checked icon, Time = 32 d (current time - 32 days)");
            VerifyFailureAttributeOkBetween(dataHistoryPage, attributeKey1, 0, 5);
            VerifyFailureAttributeOkBetween(dataHistoryPage, attributeKey2, 5, 15);
            VerifyFailureAttributeOk(dataHistoryPage, attributeKey3, "1 h");
            VerifyFailureAttributeOk(dataHistoryPage, attributeKey4, "1 d");
            VerifyFailureAttributeOk(dataHistoryPage, attributeKey5, "32 d");

            Step("14. Hover an attribute");
            attributeData = attributesData.PickRandom();
            expectedValue = attributeData.Value.ToString("M/d/yyyy H:mm tt");
            dataHistoryPage.LastValuesPanel.MoveHoverFailuresAttribute(attributeData.Key);
            tooltipValue = DateTime.Parse(dataHistoryPage.LastValuesPanel.GetFailuresTooltipAttribute(attributeData.Key)).ToString("M/d/yyyy H:mm tt");
            Step("15. Verify A tooltip displays the date and time data is simulated. Ex: 12/27/2017 6:00 AM");
            VerifyEqual("15. Verify A tooltip displays the date and time data is simulated. Ex: 12/27/2017 6:00 AM", expectedValue, tooltipValue);

            try
            {
                DeleteGeozone(geozone);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("DA_04 Search Fields pop-up UI in Data History")]
        public void DA_04()
        {
            var testData = GetTestDataOfDA_04();                        
            var expectedSearchFields = testData["SearchFields"] as List<string>;            

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.DataHistory);

            Step("1. Go to Data History app");
            Step("2. Verify Data History page is routed and loaded successfully");
            var dataHistoryPage = desktopPage.GoToApp(App.DataHistory) as DataHistoryPage;
            
            Step("3. Click Search Field icon");
            dataHistoryPage.GridPanel.ClickSearchToolbarButton();
            dataHistoryPage.GridPanel.WaitForAdvancedSearchPanelDisplayed();

            Step("4. Verify");
            Step(" o The attribute 'Device' will always appear first");
            Step(" o Attributes are listed in alphabetical order");
            var listSearchFields = dataHistoryPage.GridPanel.GetListOfAdvancedSearchFields();
            var firstField = listSearchFields.First();
            listSearchFields.RemoveAt(0);
            var firstCharList = listSearchFields.Select(p => p[0].ToString()).ToList();
            VerifyEqual("4. Verify The attribute 'Device' will always appear first", "Device", firstField);
            VerifyEqual("4. Verify Attributes are listed in alphabetical order", true, firstCharList.IsIncreasing());

            Step("5. Verify the Search Fields pop-up displays with");
            Step(" o The following 37 fields");
            Step("  + Device");
            Step("  + Dimming group");
            Step("  + Lamp level command");
            Step("  + Lamp command mode");
            Step("  + Mains current");
            Step("  + Mains voltage (V)");
            Step("  + Metered power (W)");
            Step("  + Lux level (Lux)");
            Step("  + Temperature");
            Step("  + Power Supply Failure");
            Step("  + Lamp failure");
            Step("  + Relay failure");
            Step("  + Calendar changed");
            Step("  + Active energy (KWh)");
            Step("  + Ballast failure");
            Step("  + Cycle count");
            Step("  + Cycling lamp");
            Step("  + Driver temperature (°C)");
            Step("  + Flicker count");
            Step("  + High current");
            Step("  + High OLC temperature");
            Step("  + High voltage");
            Step("  + Lamp burning hours");
            Step("  + Lamp current");
            Step("  + Lamp level feedback");
            Step("  + Lamp switch command");
            Step("  + Lamp switch feedback");
            Step("  + Lamp Type");
            Step("  + Lamp voltage (V)");
            Step("  + Lamp wattage (W)");
            Step("  + Low current");
            Step("  + Low power factor");
            Step("  + Low voltage");
            Step("  + Sum power factor");
            Step("  + TalqAddress");
            Step("  + Type of equipment");
            Step("  + Unique address");
            Step(" o 2 button icons: Reset, Search");

            VerifyEqual("[SC-1214] 5. Verify 37 fields are displayed as expected", expectedSearchFields, dataHistoryPage.GridPanel.GetListOfAdvancedSearchFields(), false);
            VerifyEqual("5. Verify Reset button is displayed", true, dataHistoryPage.GridPanel.IsAdvancedSearchesResetButtonDisplayed());
            VerifyEqual("5. Verify Search button is displayed", true, dataHistoryPage.GridPanel.IsAdvancedSearchesSearchButtonDisplayed());
           
            Step("6. Select the drop down of Device, TalqAddress, Unique address fields");
            var testingFields = new List<string> { "Device", "TalqAddress", "Unique address" };

            Step("7. Verify The operators are listed as: is, contains, begins, ends");
            Step(" o There is Only 1 textbox for this field");
            var expectedOperatorList = new List<string> { "is", "contains", "begins", "ends" };
            foreach (var field in testingFields)
            {
                var actualOperatorList = dataHistoryPage.GridPanel.GetSearchCriteriaOperatorItems(field);
                var actualInputCount = dataHistoryPage.GridPanel.GetSearchCriteriaInputsCount(field);               
                VerifyEqual(string.Format("[{0}] 7. Verify The operators are listed as: is, contains, begins, ends", field), expectedOperatorList, actualOperatorList);
                VerifyEqual(string.Format("[{0}] 7. Verify There is Only 1 textbox for this field", field), 1, actualInputCount);
            }

            Step("8. Select the drop down of Dimming group, Lamp Type, Type of equipment fields");
            testingFields = new List<string> { "Dimming group", "Lamp Type", "Type of equipment" };

            Step("9. Verify The operators are listed as: in, not in");
            Step(" o There is Only 1 textbox for this field");
            expectedOperatorList = new List<string> { "in", "not in"};
            foreach (var field in testingFields)
            {
                var actualOperatorList = dataHistoryPage.GridPanel.GetSearchCriteriaOperatorItems(field);
                var actualInputCount = dataHistoryPage.GridPanel.GetSearchCriteriaInputsCount(field);
                VerifyEqual(string.Format("[{0}] 9. Verify The operators are listed as: in, not in", field), expectedOperatorList, actualOperatorList);
                VerifyEqual(string.Format("[{0}] 9. Verify There is Only 1 textbox for this field", field), 1, actualInputCount);
            }

            testingFields = new List<string> { "Lamp level command", "Lamp command mode", "Mains current", "Mains voltage (V)", "Metered power (W)", "Lux level (Lux)", "Temperature", "Cycle count", "Driver temperature (°C)", "Flicker count", "Lamp burning hours", "Lamp current", "Active energy (KWh)", "Lamp level feedback", "Lamp switch command", "Lamp voltage (V)", "Lamp wattage (W)", "Sum power factor" };            
            Step("10. Select the drop down of the following fields");
            Step(" o Lamp level command");
            Step(" o Lamp command mode");
            Step(" o Mains current");
            Step(" o Mains voltage (V)");
            Step(" o Metered power (W)");
            Step(" o Lux level (Lux)");
            Step(" o Temperature");
            Step(" o Active energy (KWh)");
            Step(" o Cycle count");
            Step(" o Driver temperature (°C)");
            Step(" o Flicker count");
            Step(" o Lamp burning hours");
            Step(" o Lamp current");
            Step(" o Lamp level feedback");
            Step(" o Lamp switch command");
            Step(" o Lamp voltage (V)");
            Step(" o Lamp wattage (W)");
            Step(" o Sum power factor");
            Step("11. Verify The operators are listed as: equal, not equal, less than or equal to, greater than or equal to, between.");
            Step(" o Between is set as default");
            Step(" o There are 2 textboxes for Between operator");
            Step("12. Select randomly one of these operators: equal, not equal, less than or equal to, greater than or equal to");
            Step("13. Verify There is ONLY 1 textbox for these operator");
            expectedOperatorList = new List<string> { "=", "≠", "≤", "≥", "between" };
            foreach (var field in testingFields)
            {
                var actualOperatorValue = dataHistoryPage.GridPanel.GetSearchCriteriaOperatorValue(field);
                var actualOperatorList = dataHistoryPage.GridPanel.GetSearchCriteriaOperatorItems(field);
                var actualInputCount = dataHistoryPage.GridPanel.GetSearchCriteriaInputsCount(field);
                VerifyEqual(string.Format("[{0}] 11. Verify The operators are listed as: equal, not equal, less than or equal to, greater than or equal to, between", field), expectedOperatorList, actualOperatorList);
                VerifyEqual(string.Format("[{0}] 11. Verify Between is set as default", field), "between", actualOperatorValue);
                VerifyEqual(string.Format("[{0}] 11. Verify There are 2 textboxes for Between operator", field), 2, actualInputCount);
                               
                var randomOperator = expectedOperatorList.Where(p => p != "between").PickRandom();
                Step(string.Format("--> Select operator '{0}' for {1}", randomOperator, field));
                dataHistoryPage.GridPanel.SelectSearchCriteriaForOperatorDropdownField(field, randomOperator);
                actualInputCount = dataHistoryPage.GridPanel.GetSearchCriteriaInputsCount(field);
                VerifyEqual(string.Format("[{0}] 13. Verify There is ONLY 1 textbox for these operator", field), 1, actualInputCount);
            }

            testingFields = new List<string> { "Power Supply Failure", "Lamp failure", "Relay failure", "Calendar changed", "Ballast failure", "Cycling lamp", "High current", "High OLC temperature", "High voltage", "Lamp switch feedback", "Low current", "Low power factor", "Low voltage" };
            Step("14. Select the drop down of the following fields");
            Step(" o Power Supply Failure");
            Step(" o Lamp failure");
            Step(" o Relay failure");
            Step(" o Calendar changed");
            Step(" o Ballast failure");
            Step(" o Cycling lamp");
            Step(" o High current");
            Step(" o High OLC temperature");
            Step(" o High voltage");
            Step(" o Lamp switch feedback");
            Step(" o Low current");
            Step(" o Low power factor");
            Step(" o Low voltage");
            Step("15. Verify The operator is ONLY: is");
            Step(" o There is a drop down with 2 values: True, False");
            expectedOperatorList = new List<string> { "is" };
            var expectedDropDownValueItems = new List<string> { "True", "False" };
            foreach (var field in testingFields)
            {
                var actualOperatorList = dataHistoryPage.GridPanel.GetSearchCriteriaOperatorItems(field);
                var actualDropDownValueItems = dataHistoryPage.GridPanel.GetSearchCriteriaDropDownValueItems(field);
                actualDropDownValueItems.Remove("--");
                VerifyEqual(string.Format("[{0}] 15. Verify The operator is ONLY: is", field), expectedOperatorList, actualOperatorList);
                VerifyEqual(string.Format("[{0}] 15. There is a drop down with 2 values: True, False", field), expectedDropDownValueItems, actualDropDownValueItems);
            }
        }

        [Test, DynamicRetry]
        [Description("DA_05 - Search data history of metering attributes using operators: between")]
        public void DA_05()
        {
            var geozone = SLVHelper.GenerateUniqueName("GZNDA05");
            var controller = SLVHelper.GenerateUniqueName("CTRL");
            var streetlight = SLVHelper.GenerateUniqueName("STL");
            var meter = SLVHelper.GenerateUniqueName("MTR");

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - Create following devices: Streetlight, Meter");
            Step(" - Refer to the below table for Attribute Name and Parrameter.");
            Step(" • Lamp level command: LampCommandLevel");
            Step(" • Lamp command mode: LampCommandMode");
            Step(" • Mains current: Current");
            Step(" • Mains voltage (V): MainVoltage");
            Step(" • Metered power (W): MeteredPower");
            Step(" • Temperature: Temperature");
            Step(" • Active energy (KWh): Energy");
            Step(" • Lamp burning hours: RunningHoursLamp");
            Step(" • Lamp current: LampCurrent");
            Step(" • Lamp level feedback: LampLevel");
            Step(" • Lamp switch command: LampCommandSwitch");
            Step(" • Driver temperature (°C): BallastTemp");
            Step(" • Lamp voltage (V): LampVoltage");
            Step(" • Sum power factor: PowerFactor");
            Step("**** Precondition ****\n");

            Step("-> Create data for testing");
            DeleteGeozones("GZNDA05*");
            CreateNewGeozone(geozone);
            CreateNewController(controller, geozone);
            CreateNewDevice(DeviceType.Streetlight, streetlight, controller, geozone);
            CreateNewDevice(DeviceType.ElectricalCounter, meter, controller, geozone);

            var attributes = new List<string>() { "Lamp level command:LampCommandLevel", "Lamp command mode:LampCommandMode", "Mains current:Current", "Mains voltage (V):MainVoltage", "Metered power (W):MeteredPower", "Temperature:Temperature", "Active energy (KWh):Energy", "Lamp burning hours:RunningHoursLamp", "Lamp current:LampCurrent", "Lamp level feedback:LampLevel", "Lamp switch command:LampCommandSwitch", "Driver temperature (°C):BallastTemp", "Lamp voltage (V):LampVoltage", "Sum power factor:PowerFactor" };
            var dicDataCommand = new Dictionary<string, string>();
            foreach (var attribute in attributes)
            {
                var randomValue = SLVHelper.GenerateInteger(10, 9999).ToString();
                dicDataCommand.Add(attribute, randomValue);
            }

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.DataHistory);

            Step("1. Go to Data History app");
            var dataHistoryPage = desktopPage.GoToApp(App.DataHistory) as DataHistoryPage;

            Step("2. Go to the geozone from the precondition");
            dataHistoryPage.GeozoneTreeMainPanel.SelectNode(geozone);
            dataHistoryPage.GridPanel.WaitForGridContentAvailable();
          
            Step("3. Send 5 commands to simulate the data for 5 random attributes of Metering");
            Step("4. Verify All 5 commands sent OK");
            var eventTime = Settings.GetServerTime();
            var randomData = dicDataCommand.PickRandom(5);
            for (int i = 0; i < randomData.Count; i++)
            {
                var attributeKey = randomData[i].Key.SplitAndGetAt(new char[] { ':' }, 1);
                var value = randomData[i].Value;
                var request = SetValueToDevice(controller, streetlight, attributeKey, value, eventTime);
                VerifyEqual(string.Format("4. Verify the request {0} is sent successfully (attribute: {1}, value: {2})", i + 1, attributeKey, value), true, request);
            }

            Step("5. Select Search Fields icon on the main panel");
            dataHistoryPage.GridPanel.ClickSearchToolbarButton();
            dataHistoryPage.GridPanel.WaitForAdvancedSearchPanelDisplayed();

            Step("6. Fill the textbox Device with testing Streetlight's name and select operation IS");
            var deviceOperatior = "is";
            dataHistoryPage.GridPanel.SelectSearchCriteriaForOperatorDropdownField("Device", deviceOperatior);
            dataHistoryPage.GridPanel.EnterSearchCriteriaFor1stValueInputField("Device", streetlight);

            Step("7. Choose the operator Between for 5 attributes for which we just sent simulated commands");
            Step("8. Fill in the 1st textbox with 0 value and the 2nd with value in the simulated commands");            
            for (int i = 0; i < randomData.Count; i++)
            {
                var attributeName = randomData[i].Key.SplitAndGetAt(new char[] { ':' }, 0);
                var value = randomData[i].Value;
                dataHistoryPage.GridPanel.SelectSearchCriteriaForOperatorDropdownField(attributeName, "between");
                dataHistoryPage.GridPanel.EnterSearchCriteriaFor1stValueInputField(attributeName, "0");
                dataHistoryPage.GridPanel.EnterSearchCriteriaFor2ndValueInputField(attributeName, value);
            }

            Step("9. Press Search button");
            dataHistoryPage.GridPanel.ClickAdvancedSearchSearchButton();
            dataHistoryPage.GridPanel.WaitForAdvancedSearchPanelDisappeared();
            dataHistoryPage.GridPanel.WaitForLeftFooterTextDisplayed();

            Step("10. Verify The search result display ONLY the testing streetlight in the list");
            var devicesList = dataHistoryPage.GridPanel.GetListOfColumnData("Device");
            VerifyEqual("10. Verify The search result display ONLY the testing streetlight in the list", new List<string> { streetlight }, devicesList);

            Step("11. Select Search Fields icon again");
            dataHistoryPage.GridPanel.ClickSearchToolbarButton();
            dataHistoryPage.GridPanel.WaitForAdvancedSearchPanelDisplayed();

            Step("12. Verify The Search Fields screen displays with");
            Step(" o Text: Current search criteria: Device is {Streetlight's name},{Attribute 1's name} 0,{value of attribute 1},{Attribute 2's name} 0,{value of attribute 2}, {Attribute 3's name} 0,{value of attribute 3},{Attribute 4's name} 0,{value of attribute 4} and {Attribute 5's name} 0,{value of attribute 5}");
            Step(" o Ex: Device is DataHistorySL02,Lamp command mode 0,102");
            var expectedSearchCriteriaList = new List<string>();
            expectedSearchCriteriaList.Add(string.Format("Device is {0}", streetlight));           
            for (int i = 0; i < randomData.Count; i++)
            {
                var attributeName = randomData[i].Key.SplitAndGetAt(new char[] { ':' }, 0);
                var value = randomData[i].Value;
                expectedSearchCriteriaList.Add(string.Format("{0} 0,{1}", attributeName, value));
            }
            var actualSearchCriteriaList = dataHistoryPage.GridPanel.GetListOfSearchCriteria();
            VerifyEqual("12. Verify The Search Title displays as expected", expectedSearchCriteriaList, actualSearchCriteriaList, false);
            Warning(string.Format("SC-1098: Data History - Some Current Search Criteria operators are not visible ({0})", string.Join(" , ", expectedSearchCriteriaList)));

            Step("13. Set randomly the 2nd value of an attribute to a value less than the attribute's value");         
            var rdmKey = randomData.PickRandom().Key;
            for (int i = 0; i < randomData.Count; i++)
            {
                var attributeName = randomData[i].Key.SplitAndGetAt(new char[] { ':' }, 0);
                var value = randomData[i].Value;
                if (randomData[i].Key == rdmKey) value = (int.Parse(value) - 1).ToString();
                dataHistoryPage.GridPanel.SelectSearchCriteriaForOperatorDropdownField(attributeName, "between");
                dataHistoryPage.GridPanel.EnterSearchCriteriaFor1stValueInputField(attributeName, "0");
                dataHistoryPage.GridPanel.EnterSearchCriteriaFor2ndValueInputField(attributeName, value);
            }

            Step("14. Press Search button");
            dataHistoryPage.GridPanel.ClickAdvancedSearchSearchButton();
            dataHistoryPage.GridPanel.WaitForAdvancedSearchPanelDisappeared();
            dataHistoryPage.GridPanel.WaitForLeftFooterTextDisplayed();

            Step("15. Verify The search result is empty");
            devicesList = dataHistoryPage.GridPanel.GetListOfColumnData("Device");
            VerifyEqual("15. Verify The search result is empty", 0, devicesList.Count);

            Step("16. Select Search Fields icon again");
            dataHistoryPage.GridPanel.ClickSearchToolbarButton();
            dataHistoryPage.GridPanel.WaitForAdvancedSearchPanelDisplayed();

            Step("17. Verify the text of the Search Fields screen is updated the value of the attribute above");
            Step(" o Ex: Device is DataHistorySL02,Lamp command mode 0,101");
            expectedSearchCriteriaList.Clear();
            expectedSearchCriteriaList.Add(string.Format("Device is {0}", streetlight));
            for (int i = 0; i < randomData.Count; i++)
            {
                var attributeName = randomData[i].Key.SplitAndGetAt(new char[] { ':' }, 0);
                var value = randomData[i].Value;
                if (randomData[i].Key == rdmKey) value = (int.Parse(value) - 1).ToString();
                expectedSearchCriteriaList.Add(string.Format("{0} 0,{1}", attributeName, value));
            }
            actualSearchCriteriaList = dataHistoryPage.GridPanel.GetListOfSearchCriteria();
            VerifyEqual("17. Verify the text of the Search Fields screen is updated the value of the attribute above", expectedSearchCriteriaList, actualSearchCriteriaList, false);
            Warning(string.Format("SC-1098: Data History - Some Current Search Criteria operators are not visible ({0})", string.Join(" , ", expectedSearchCriteriaList)));

            Step("18. Set the 1st value = the old value -1 and 2nd value= the old value, then press Search button");
            for (int i = 0; i < randomData.Count; i++)
            {
                var attributeName = randomData[i].Key.SplitAndGetAt(new char[] { ':' }, 0);
                var valueInput2 = randomData[i].Value;
                var valueInput1 = (int.Parse(randomData[i].Value) - 1).ToString();
                dataHistoryPage.GridPanel.SelectSearchCriteriaForOperatorDropdownField(attributeName, "between");
                dataHistoryPage.GridPanel.EnterSearchCriteriaFor1stValueInputField(attributeName, valueInput1);
                dataHistoryPage.GridPanel.EnterSearchCriteriaFor2ndValueInputField(attributeName, valueInput2);
            }
            dataHistoryPage.GridPanel.ClickAdvancedSearchSearchButton();
            dataHistoryPage.GridPanel.WaitForAdvancedSearchPanelDisappeared();
            dataHistoryPage.GridPanel.WaitForLeftFooterTextDisplayed();

            Step("19. Verify The search result display ONLY the testing streetlight in the list");
            devicesList = dataHistoryPage.GridPanel.GetListOfColumnData("Device");
            VerifyEqual("19. Verify The search result display ONLY the testing streetlight in the list", new List<string> { streetlight }, devicesList);
            
            try
            {
                DeleteGeozone(geozone);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("DA_06 - Search data history of metering attributes using operators: equal to")]
        public void DA_06()
        {
            var geozone = SLVHelper.GenerateUniqueName("GZNDA06");
            var controller = SLVHelper.GenerateUniqueName("CTRL");
            var streetlight = SLVHelper.GenerateUniqueName("STL");
            var meter = SLVHelper.GenerateUniqueName("MTR");

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - Create following devices: Streetlight, Meter");
            Step(" - Refer to the below table for Attribute Name and Parrameter.");
            Step(" • Lamp level command: LampCommandLevel");
            Step(" • Lamp command mode: LampCommandMode");
            Step(" • Mains current: Current");
            Step(" • Mains voltage (V): MainVoltage");
            Step(" • Metered power (W): MeteredPower");
            Step(" • Temperature: Temperature");
            Step(" • Active energy (KWh): Energy");
            Step(" • Lamp burning hours: RunningHoursLamp");
            Step(" • Lamp current: LampCurrent");
            Step(" • Lamp level feedback: LampLevel");
            Step(" • Lamp switch command: LampCommandSwitch");
            Step(" • Driver temperature (°C): BallastTemp");
            Step(" • Lamp voltage (V): LampVoltage");
            Step(" • Sum power factor: PowerFactor");
            Step("**** Precondition ****\n");

            Step("-> Create data for testing");
            DeleteGeozones("GZNDA06*");
            CreateNewGeozone(geozone);
            CreateNewController(controller, geozone);
            CreateNewDevice(DeviceType.Streetlight, streetlight, controller, geozone);
            CreateNewDevice(DeviceType.ElectricalCounter, meter, controller, geozone);

            var attributes = new List<string>() { "Lamp level command:LampCommandLevel", "Lamp command mode:LampCommandMode", "Mains current:Current", "Mains voltage (V):MainVoltage", "Metered power (W):MeteredPower", "Temperature:Temperature", "Active energy (KWh):Energy", "Lamp burning hours:RunningHoursLamp", "Lamp current:LampCurrent", "Lamp level feedback:LampLevel", "Lamp switch command:LampCommandSwitch", "Driver temperature (°C):BallastTemp", "Lamp voltage (V):LampVoltage", "Sum power factor:PowerFactor" };
            var dicDataCommand = new Dictionary<string, string>();
            foreach (var attribute in attributes)
            {
                var randomValue = SLVHelper.GenerateInteger(10, 9999).ToString();
                dicDataCommand.Add(attribute, randomValue);
            }

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.DataHistory);

            Step("1. Go to Data History app");
            var dataHistoryPage = desktopPage.GoToApp(App.DataHistory) as DataHistoryPage;

            Step("2. Go to the geozone from the precondition");
            dataHistoryPage.GeozoneTreeMainPanel.SelectNode(geozone);
            dataHistoryPage.GridPanel.WaitForGridContentAvailable();

            Step("3. Send 5 commands to simulate the data for 5 random attributes of Metering");
            Step("4. Verify All 5 commands sent OK");
            var eventTime = Settings.GetServerTime();
            var randomData = dicDataCommand.PickRandom(5);
            for (int i = 0; i < randomData.Count; i++)
            {
                var attributeKey = randomData[i].Key.SplitAndGetAt(new char[] { ':' }, 1);
                var value = randomData[i].Value;
                var request = SetValueToDevice(controller, streetlight, attributeKey, value, eventTime);
                VerifyEqual(string.Format("4. Verify the request {0} is sent successfully (attribute: {1}, value: {2})", i + 1, attributeKey, value), true, request);
            }

            Step("5. Select Search Fields icon on the main panel");
            dataHistoryPage.GridPanel.ClickSearchToolbarButton();
            dataHistoryPage.GridPanel.WaitForAdvancedSearchPanelDisplayed();

            Step("6. Fill the textbox Device with testing Streetlight's name and select operation IS");
            var deviceOperatior = "is";
            dataHistoryPage.GridPanel.SelectSearchCriteriaForOperatorDropdownField("Device", deviceOperatior);
            dataHistoryPage.GridPanel.EnterSearchCriteriaFor1stValueInputField("Device", streetlight);

            Step("7. Choose the operator Between for 5 attributes for which we just sent simulated commands");
            Step("8. Fill in the textbox with value in the simulated commands");
            for (int i = 0; i < randomData.Count; i++)
            {
                var attributeName = randomData[i].Key.SplitAndGetAt(new char[] { ':' }, 0);
                var value = randomData[i].Value;
                dataHistoryPage.GridPanel.SelectSearchCriteriaForOperatorDropdownField(attributeName, "=");
                dataHistoryPage.GridPanel.EnterSearchCriteriaFor1stValueInputField(attributeName, value);
            }

            Step("9. Press Search button");
            dataHistoryPage.GridPanel.ClickAdvancedSearchSearchButton();
            dataHistoryPage.GridPanel.WaitForAdvancedSearchPanelDisappeared();
            dataHistoryPage.GridPanel.WaitForLeftFooterTextDisplayed();

            Step("10. Verify The search result display ONLY the testing streetlight in the list");
            var devicesList = dataHistoryPage.GridPanel.GetListOfColumnData("Device");
            VerifyEqual("10. Verify The search result display ONLY the testing streetlight in the list", new List<string> { streetlight }, devicesList);

            Step("11. Select Search Fields icon again");
            dataHistoryPage.GridPanel.ClickSearchToolbarButton();
            dataHistoryPage.GridPanel.WaitForAdvancedSearchPanelDisplayed();

            Step("12. Verify The Search Fields screen displays with");
            Step(" o Text: Current search criteria: Device is {Streetlight's name},{Attribute 1's name} {value of attribute 1},{Attribute 2's name} {value of attribute 2}, {Attribute 3's name} {value of attribute 3},{Attribute 4's name} {value of attribute 4} and {Attribute 5's name} {value of attribute 5}");
            Step(" o Ex: Device is DataHistorySL02,Lamp command mode 102");
            var expectedSearchCriteriaList = new List<string>();
            expectedSearchCriteriaList.Add(string.Format("Device is {0}", streetlight));
            for (int i = 0; i < randomData.Count; i++)
            {
                var attributeName = randomData[i].Key.SplitAndGetAt(new char[] { ':' }, 0);
                var value = randomData[i].Value;
                expectedSearchCriteriaList.Add(string.Format("{0} {1}", attributeName, value));
            }
            var actualSearchCriteriaList = dataHistoryPage.GridPanel.GetListOfSearchCriteria();
            VerifyEqual("12. Verify The Search Title displays as expected", expectedSearchCriteriaList, actualSearchCriteriaList, false);
            Warning(string.Format("SC-1098: Data History - Some Current Search Criteria operators are not visible ({0})", string.Join(" , ", expectedSearchCriteriaList)));

            Step("13. Set randomly the value of an attribute to a value different to the attribute's value");
            var rdmKey = randomData.PickRandom().Key;
            var rdmValue = SLVHelper.GenerateInteger(1, 9999).ToString();
            for (int i = 0; i < randomData.Count; i++)
            {
                var attributeName = randomData[i].Key.SplitAndGetAt(new char[] { ':' }, 0);
                var value = randomData[i].Value;
                if (randomData[i].Key == rdmKey) value = rdmValue;
                dataHistoryPage.GridPanel.SelectSearchCriteriaForOperatorDropdownField(attributeName, "=");                
                dataHistoryPage.GridPanel.EnterSearchCriteriaFor1stValueInputField(attributeName, value);
            }

            Step("14. Press Search button");
            dataHistoryPage.GridPanel.ClickAdvancedSearchSearchButton();
            dataHistoryPage.GridPanel.WaitForAdvancedSearchPanelDisappeared();
            dataHistoryPage.GridPanel.WaitForLeftFooterTextDisplayed();

            Step("15. Verify The search result is empty");
            devicesList = dataHistoryPage.GridPanel.GetListOfColumnData("Device");
            VerifyEqual("15. Verify The search result is empty", 0, devicesList.Count);

            Step("16. Select Search Fields icon again");
            dataHistoryPage.GridPanel.ClickSearchToolbarButton();
            dataHistoryPage.GridPanel.WaitForAdvancedSearchPanelDisplayed();

            Step("17. Verify the text of the Search Fields screen is updated the value of the attribute above");
            Step(" o Ex: Device is DataHistorySL02,Lamp command mode 101");
            expectedSearchCriteriaList.Clear();
            expectedSearchCriteriaList.Add(string.Format("Device is {0}", streetlight));
            for (int i = 0; i < randomData.Count; i++)
            {
                var attributeName = randomData[i].Key.SplitAndGetAt(new char[] { ':' }, 0);
                var value = randomData[i].Value;
                if (randomData[i].Key == rdmKey) value = rdmValue;
                expectedSearchCriteriaList.Add(string.Format("{0} {1}", attributeName, value));
            }
            actualSearchCriteriaList = dataHistoryPage.GridPanel.GetListOfSearchCriteria();
            VerifyEqual("17. Verify the text of the Search Fields screen is updated the value of the attribute above", expectedSearchCriteriaList, actualSearchCriteriaList, false);
            Warning(string.Format("SC-1098: Data History - Some Current Search Criteria operators are not visible ({0})", string.Join(" , ", expectedSearchCriteriaList)));

            Step("18. Set back the old value then press Search button");
            for (int i = 0; i < randomData.Count; i++)
            {
                var attributeName = randomData[i].Key.SplitAndGetAt(new char[] { ':' }, 0);
                var value = randomData[i].Value;
                dataHistoryPage.GridPanel.SelectSearchCriteriaForOperatorDropdownField(attributeName, "=");
                dataHistoryPage.GridPanel.EnterSearchCriteriaFor1stValueInputField(attributeName, value);
            }
            dataHistoryPage.GridPanel.ClickAdvancedSearchSearchButton();
            dataHistoryPage.GridPanel.WaitForAdvancedSearchPanelDisappeared();
            dataHistoryPage.GridPanel.WaitForLeftFooterTextDisplayed();

            Step("19. Verify The search result display ONLY the testing streetlight in the list");
            devicesList = dataHistoryPage.GridPanel.GetListOfColumnData("Device");
            VerifyEqual("19. Verify The search result display ONLY the testing streetlight in the list", new List<string> { streetlight }, devicesList);

            try
            {
                DeleteGeozone(geozone);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("DA_07- Search data history of metering attributes using operators: less than or equal to")]
        [NonParallelizable]
        public void DA_07()
        {
            var geozone = SLVHelper.GenerateUniqueName("GZNDA07");
            var controller = SLVHelper.GenerateUniqueName("CTRL");
            var streetlight = SLVHelper.GenerateUniqueName("STL");
            var meter = SLVHelper.GenerateUniqueName("MTR");

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - Create following devices: Streetlight, Meter");
            Step(" - Refer to the below table for Attribute Name and Parrameter.");
            Step(" • Lamp level command: LampCommandLevel");
            Step(" • Lamp command mode: LampCommandMode");
            Step(" • Mains current: Current");
            Step(" • Mains voltage (V): MainVoltage");
            Step(" • Metered power (W): MeteredPower");
            Step(" • Temperature: Temperature");
            Step(" • Active energy (KWh): Energy");
            Step(" • Lamp burning hours: RunningHoursLamp");
            Step(" • Lamp current: LampCurrent");
            Step(" • Lamp level feedback: LampLevel");
            Step(" • Lamp switch command: LampCommandSwitch");
            Step(" • Driver temperature (°C): BallastTemp");
            Step(" • Lamp voltage (V): LampVoltage");
            Step(" • Sum power factor: PowerFactor");
            Step("**** Precondition ****\n");

            Step("-> Create data for testing");
            DeleteGeozones("GZNDA07*");
            CreateNewGeozone(geozone);
            CreateNewController(controller, geozone);
            CreateNewDevice(DeviceType.Streetlight, streetlight, controller, geozone);
            CreateNewDevice(DeviceType.ElectricalCounter, meter, controller, geozone);

            var attributes = new List<string>() { "Lamp level command:LampCommandLevel", "Lamp command mode:LampCommandMode", "Mains current:Current", "Mains voltage (V):MainVoltage", "Metered power (W):MeteredPower", "Temperature:Temperature", "Active energy (KWh):Energy", "Lamp burning hours:RunningHoursLamp", "Lamp current:LampCurrent", "Lamp level feedback:LampLevel", "Lamp switch command:LampCommandSwitch", "Driver temperature (°C):BallastTemp", "Lamp voltage (V):LampVoltage", "Sum power factor:PowerFactor" };
            var dicDataCommand = new Dictionary<string, string>();
            foreach (var attribute in attributes)
            {
                var randomValue = SLVHelper.GenerateInteger(10, 9999).ToString();
                dicDataCommand.Add(attribute, randomValue);
            }

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.DataHistory);

            Step("1. Go to Data History app");
            var dataHistoryPage = desktopPage.GoToApp(App.DataHistory) as DataHistoryPage;

            Step("2. Go to the geozone from the precondition");
            dataHistoryPage.GeozoneTreeMainPanel.SelectNode(geozone);
            dataHistoryPage.GridPanel.WaitForGridContentAvailable();

            Step("3. Send 5 commands to simulate the data for 5 random attributes of Metering");
            Step("4. Verify All 5 commands sent OK");
            var eventTime = Settings.GetServerTime();
            var randomData = dicDataCommand.PickRandom(5);
            for (int i = 0; i < randomData.Count; i++)
            {
                var attributeKey = randomData[i].Key.SplitAndGetAt(new char[] { ':' }, 1);
                var value = randomData[i].Value;
                var request = SetValueToDevice(controller, streetlight, attributeKey, value, eventTime);
                VerifyEqual(string.Format("4. Verify the request {0} is sent successfully (attribute: {1}, value: {2})", i + 1, attributeKey, value), true, request);
            }

            Step("5. Select Search Fields icon on the main panel");
            dataHistoryPage.GridPanel.ClickSearchToolbarButton();
            dataHistoryPage.GridPanel.WaitForAdvancedSearchPanelDisplayed();

            Step("6. Fill the textbox Device with testing Streetlight's name and select operation IS");
            var deviceOperatior = "is";
            dataHistoryPage.GridPanel.SelectSearchCriteriaForOperatorDropdownField("Device", deviceOperatior);
            dataHistoryPage.GridPanel.EnterSearchCriteriaFor1stValueInputField("Device", streetlight);

            Step("7. Choose the operator 'less than or equal to' for 5 attributes for which we just sent simulated commands");
            Step("8. Fill in the textbox with value in the simulated commands");
            for (int i = 0; i < randomData.Count; i++)
            {
                var attributeName = randomData[i].Key.SplitAndGetAt(new char[] { ':' }, 0);
                var value = randomData[i].Value;
                dataHistoryPage.GridPanel.SelectSearchCriteriaForOperatorDropdownField(attributeName, "≤");
                dataHistoryPage.GridPanel.EnterSearchCriteriaFor1stValueInputField(attributeName, value);
            }

            Step("9. Press Search button");
            dataHistoryPage.GridPanel.ClickAdvancedSearchSearchButton();
            dataHistoryPage.GridPanel.WaitForAdvancedSearchPanelDisappeared();
            dataHistoryPage.GridPanel.WaitForLeftFooterTextDisplayed();

            Step("10. Verify The search result display ONLY the testing streetlight in the list");
            var devicesList = dataHistoryPage.GridPanel.GetListOfColumnData("Device");
            VerifyEqual("10. Verify The search result display ONLY the testing streetlight in the list", new List<string> { streetlight }, devicesList);

            Step("11. Select Search Fields icon again");
            dataHistoryPage.GridPanel.ClickSearchToolbarButton();
            dataHistoryPage.GridPanel.WaitForAdvancedSearchPanelDisplayed();

            Step("12. Verify The Search Fields screen displays with");
            Step(" o Text: Current search criteria: Device is {Streetlight's name},{Attribute 1's name} {value of attribute 1},{Attribute 2's name} {value of attribute 2}, {Attribute 3's name} {value of attribute 3},{Attribute 4's name} {value of attribute 4} and {Attribute 5's name} {value of attribute 5}");
            Step(" o Ex: Device is DataHistorySL02,Lamp command mode 102");
            var expectedSearchCriteriaList = new List<string>();
            expectedSearchCriteriaList.Add(string.Format("Device is {0}", streetlight));
            for (int i = 0; i < randomData.Count; i++)
            {
                var attributeName = randomData[i].Key.SplitAndGetAt(new char[] { ':' }, 0);
                var value = randomData[i].Value;
                expectedSearchCriteriaList.Add(string.Format("{0} {1}", attributeName, value));
            }
            var actualSearchCriteriaList = dataHistoryPage.GridPanel.GetListOfSearchCriteria();
            VerifyEqual("12. Verify The Search Title displays as expected", expectedSearchCriteriaList, actualSearchCriteriaList, false);
            Warning(string.Format("SC-1098: Data History - Some Current Search Criteria operators are not visible ({0})", string.Join(" , ", expectedSearchCriteriaList)));

            Step("13. Set randomly the value of an attribute to a value less than the attribute's value");
            var rdmKey = randomData.PickRandom().Key;
            for (int i = 0; i < randomData.Count; i++)
            {
                var attributeName = randomData[i].Key.SplitAndGetAt(new char[] { ':' }, 0);
                var value = randomData[i].Value;
                if (randomData[i].Key == rdmKey) value = (int.Parse(value) - 1).ToString();
                dataHistoryPage.GridPanel.SelectSearchCriteriaForOperatorDropdownField(attributeName, "≤");
                dataHistoryPage.GridPanel.EnterSearchCriteriaFor1stValueInputField(attributeName, value);
            }

            Step("14. Press Search button");
            dataHistoryPage.GridPanel.ClickAdvancedSearchSearchButton();
            dataHistoryPage.GridPanel.WaitForAdvancedSearchPanelDisappeared();
            dataHistoryPage.GridPanel.WaitForLeftFooterTextDisplayed();

            Step("15. Verify The search result is empty");
            devicesList = dataHistoryPage.GridPanel.GetListOfColumnData("Device");
            VerifyEqual("15. Verify The search result is empty", 0, devicesList.Count);

            Step("16. Select Search Fields icon again");
            dataHistoryPage.GridPanel.ClickSearchToolbarButton();
            dataHistoryPage.GridPanel.WaitForAdvancedSearchPanelDisplayed();

            Step("17. Verify the text of the Search Fields screen is updated the value of the attribute above");
            Step(" o Ex: Device is DataHistorySL02,Lamp command mode 101");
            expectedSearchCriteriaList.Clear();
            expectedSearchCriteriaList.Add(string.Format("Device is {0}", streetlight));
            for (int i = 0; i < randomData.Count; i++)
            {
                var attributeName = randomData[i].Key.SplitAndGetAt(new char[] { ':' }, 0);
                var value = randomData[i].Value;
                if (randomData[i].Key == rdmKey) value = (int.Parse(value) - 1).ToString();
                expectedSearchCriteriaList.Add(string.Format("{0} {1}", attributeName, value));
            }
            actualSearchCriteriaList = dataHistoryPage.GridPanel.GetListOfSearchCriteria();
            VerifyEqual("17. Verify the text of the Search Fields screen is updated the value of the attribute above", expectedSearchCriteriaList, actualSearchCriteriaList, false);
            Warning(string.Format("SC-1098: Data History - Some Current Search Criteria operators are not visible ({0})", string.Join(" , ", expectedSearchCriteriaList)));

            Step("18. Set back the old value then press Search button");
            for (int i = 0; i < randomData.Count; i++)
            {
                var attributeName = randomData[i].Key.SplitAndGetAt(new char[] { ':' }, 0);
                var value = randomData[i].Value;
                dataHistoryPage.GridPanel.SelectSearchCriteriaForOperatorDropdownField(attributeName, "≤");
                dataHistoryPage.GridPanel.EnterSearchCriteriaFor1stValueInputField(attributeName, value);
            }
            dataHistoryPage.GridPanel.ClickAdvancedSearchSearchButton();
            dataHistoryPage.GridPanel.WaitForAdvancedSearchPanelDisappeared();
            dataHistoryPage.GridPanel.WaitForLeftFooterTextDisplayed();

            Step("19. Verify The search result display ONLY the testing streetlight in the list");
            devicesList = dataHistoryPage.GridPanel.GetListOfColumnData("Device");
            VerifyEqual("19. Verify The search result display ONLY the testing streetlight in the list", new List<string> { streetlight }, devicesList);

            try
            {
                DeleteGeozone(geozone);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("DA_08- Search data history of metering attributes using operators: greater than or equal to")]
        public void DA_08()
        {
            var geozone = SLVHelper.GenerateUniqueName("GZNDA08");
            var controller = SLVHelper.GenerateUniqueName("CTRL");
            var streetlight = SLVHelper.GenerateUniqueName("STL");
            var meter = SLVHelper.GenerateUniqueName("MTR");

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - Create following devices: Streetlight, Meter");
            Step(" - Refer to the below table for Attribute Name and Parrameter.");
            Step(" • Lamp level command: LampCommandLevel");
            Step(" • Lamp command mode: LampCommandMode");
            Step(" • Mains current: Current");
            Step(" • Mains voltage (V): MainVoltage");
            Step(" • Metered power (W): MeteredPower");
            Step(" • Temperature: Temperature");
            Step(" • Active energy (KWh): Energy");
            Step(" • Lamp burning hours: RunningHoursLamp");
            Step(" • Lamp current: LampCurrent");
            Step(" • Lamp level feedback: LampLevel");
            Step(" • Lamp switch command: LampCommandSwitch");
            Step(" • Driver temperature (°C): BallastTemp");
            Step(" • Lamp voltage (V): LampVoltage");
            Step(" • Sum power factor: PowerFactor");
            Step("**** Precondition ****\n");

            Step("-> Create data for testing");
            DeleteGeozones("GZNDA08*");
            CreateNewGeozone(geozone);
            CreateNewController(controller, geozone);
            CreateNewDevice(DeviceType.Streetlight, streetlight, controller, geozone);
            CreateNewDevice(DeviceType.ElectricalCounter, meter, controller, geozone);

            var attributes = new List<string>() { "Lamp level command:LampCommandLevel", "Lamp command mode:LampCommandMode", "Mains current:Current", "Mains voltage (V):MainVoltage", "Metered power (W):MeteredPower", "Temperature:Temperature", "Active energy (KWh):Energy", "Lamp burning hours:RunningHoursLamp", "Lamp current:LampCurrent", "Lamp level feedback:LampLevel", "Lamp switch command:LampCommandSwitch", "Driver temperature (°C):BallastTemp", "Lamp voltage (V):LampVoltage", "Sum power factor:PowerFactor" };
            var dicDataCommand = new Dictionary<string, string>();
            foreach (var attribute in attributes)
            {
                var randomValue = SLVHelper.GenerateInteger(10, 9999).ToString();
                dicDataCommand.Add(attribute, randomValue);
            }

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.DataHistory);

            Step("1. Go to Data History app");
            var dataHistoryPage = desktopPage.GoToApp(App.DataHistory) as DataHistoryPage;

            Step("2. Go to the geozone from the precondition");
            dataHistoryPage.GeozoneTreeMainPanel.SelectNode(geozone);
            dataHistoryPage.GridPanel.WaitForGridContentAvailable();

            Step("3. Send 3 commands to simulate the data for 3 random attributes of Metering");
            Step("4. Verify All 3 commands sent OK");
            var eventTime = Settings.GetServerTime();
            var randomData = dicDataCommand.PickRandom(3);
            for (int i = 0; i < randomData.Count; i++)
            {
                var attributeKey = randomData[i].Key.SplitAndGetAt(new char[] { ':' }, 1);
                var value = randomData[i].Value;
                var request = SetValueToDevice(controller, streetlight, attributeKey, value, eventTime);
                VerifyEqual(string.Format("4. Verify the request {0} is sent successfully (attribute: {1}, value: {2})", i + 1, attributeKey, value), true, request);
            }

            Step("5. Select Search Fields icon on the main panel");
            dataHistoryPage.GridPanel.ClickSearchToolbarButton();
            dataHistoryPage.GridPanel.WaitForAdvancedSearchPanelDisplayed();

            Step("6. Fill the textbox Device with testing Streetlight's name and select operation IS");
            var deviceOperatior = "is";
            dataHistoryPage.GridPanel.SelectSearchCriteriaForOperatorDropdownField("Device", deviceOperatior);
            dataHistoryPage.GridPanel.EnterSearchCriteriaFor1stValueInputField("Device", streetlight);

            Step("7. Choose the operator 'greater than or equal to' for 3 attributes for which we just sent simulated commands");
            Step("8. Fill in the textbox with value in the simulated commands");
            for (int i = 0; i < randomData.Count; i++)
            {
                var attributeName = randomData[i].Key.SplitAndGetAt(new char[] { ':' }, 0);
                var value = randomData[i].Value;
                dataHistoryPage.GridPanel.SelectSearchCriteriaForOperatorDropdownField(attributeName, "≥");
                dataHistoryPage.GridPanel.EnterSearchCriteriaFor1stValueInputField(attributeName, value);
            }

            Step("9. Press Search button");
            dataHistoryPage.GridPanel.ClickAdvancedSearchSearchButton();
            dataHistoryPage.GridPanel.WaitForAdvancedSearchPanelDisappeared();
            dataHistoryPage.GridPanel.WaitForLeftFooterTextDisplayed();

            Step("10. Verify The search result display ONLY the testing streetlight in the list");
            var devicesList = dataHistoryPage.GridPanel.GetListOfColumnData("Device");
            VerifyEqual("10. Verify The search result display ONLY the testing streetlight in the list", new List<string> { streetlight }, devicesList);

            Step("11. Select Search Fields icon again");
            dataHistoryPage.GridPanel.ClickSearchToolbarButton();
            dataHistoryPage.GridPanel.WaitForAdvancedSearchPanelDisplayed();

            Step("12. Verify The Search Fields screen displays with");
            Step(" o Text: Current search criteria: Device is {Streetlight's name},{Attribute 1's name} {value of attribute 1},{Attribute 2's name} {value of attribute 2} and {Attribute 3's name} >= {value of attribute 3}");
            Step(" o Ex: Device is DataHistorySL02,Lamp command mode 102");
            var expectedSearchCriteriaList = new List<string>();
            expectedSearchCriteriaList.Add(string.Format("Device is {0}", streetlight));
            for (int i = 0; i < randomData.Count; i++)
            {
                var attributeName = randomData[i].Key.SplitAndGetAt(new char[] { ':' }, 0);
                var value = randomData[i].Value;
                expectedSearchCriteriaList.Add(string.Format("{0} {1}", attributeName, value));
            }
            var actualSearchCriteriaList = dataHistoryPage.GridPanel.GetListOfSearchCriteria();
            VerifyEqual("12. Verify The Search Title displays as expected", expectedSearchCriteriaList, actualSearchCriteriaList, false);
            Warning(string.Format("SC-1098: Data History - Some Current Search Criteria operators are not visible ({0})", string.Join(" , ", expectedSearchCriteriaList)));

            Step("13. Set randomly the value of an attribute to a value greater than the attribute's value");
            var rdmKey = randomData.PickRandom().Key;
            for (int i = 0; i < randomData.Count; i++)
            {
                var attributeName = randomData[i].Key.SplitAndGetAt(new char[] { ':' }, 0);
                var value = randomData[i].Value;
                if (randomData[i].Key == rdmKey) value = (int.Parse(value) + 1).ToString();
                dataHistoryPage.GridPanel.SelectSearchCriteriaForOperatorDropdownField(attributeName, "≥");
                dataHistoryPage.GridPanel.EnterSearchCriteriaFor1stValueInputField(attributeName, value);
            }

            Step("14. Press Search button");
            dataHistoryPage.GridPanel.ClickAdvancedSearchSearchButton();
            dataHistoryPage.GridPanel.WaitForAdvancedSearchPanelDisappeared();
            dataHistoryPage.GridPanel.WaitForLeftFooterTextDisplayed();

            Step("15. Verify The search result is empty");
            devicesList = dataHistoryPage.GridPanel.GetListOfColumnData("Device");
            VerifyEqual("15. Verify The search result is empty", 0, devicesList.Count);

            Step("16. Select Search Fields icon again");
            dataHistoryPage.GridPanel.ClickSearchToolbarButton();
            dataHistoryPage.GridPanel.WaitForAdvancedSearchPanelDisplayed();

            Step("17. Verify the text of the Search Fields screen is updated the value of the attribute above");
            Step(" o Ex: Device is DataHistorySL02,Lamp command mode 103");
            expectedSearchCriteriaList.Clear();
            expectedSearchCriteriaList.Add(string.Format("Device is {0}", streetlight));
            for (int i = 0; i < randomData.Count; i++)
            {
                var attributeName = randomData[i].Key.SplitAndGetAt(new char[] { ':' }, 0);
                var value = randomData[i].Value;
                if (randomData[i].Key == rdmKey) value = (int.Parse(value) + 1).ToString();
                expectedSearchCriteriaList.Add(string.Format("{0} {1}", attributeName, value));
            }
            actualSearchCriteriaList = dataHistoryPage.GridPanel.GetListOfSearchCriteria();
            VerifyEqual("17. Verify the text of the Search Fields screen is updated the value of the attribute above", expectedSearchCriteriaList, actualSearchCriteriaList, false);
            Warning(string.Format("SC-1098: Data History - Some Current Search Criteria operators are not visible ({0})", string.Join(" , ", expectedSearchCriteriaList)));

            Step("18. Set back the value less than the attribute's value then press Search button. Ex: the attribute's value =102, set the value =101");
            for (int i = 0; i < randomData.Count; i++)
            {
                var attributeName = randomData[i].Key.SplitAndGetAt(new char[] { ':' }, 0);
                var value = randomData[i].Value;
                dataHistoryPage.GridPanel.SelectSearchCriteriaForOperatorDropdownField(attributeName, "≥");
                dataHistoryPage.GridPanel.EnterSearchCriteriaFor1stValueInputField(attributeName, value);
            }
            dataHistoryPage.GridPanel.ClickAdvancedSearchSearchButton();
            dataHistoryPage.GridPanel.WaitForAdvancedSearchPanelDisappeared();
            dataHistoryPage.GridPanel.WaitForLeftFooterTextDisplayed();

            Step("19. Verify The search result display ONLY the testing streetlight in the list");
            devicesList = dataHistoryPage.GridPanel.GetListOfColumnData("Device");
            VerifyEqual("19. Verify The search result display ONLY the testing streetlight in the list", new List<string> { streetlight }, devicesList);

            try
            {
                DeleteGeozone(geozone);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("DA_09- Search data history of metering attributes using operators: different")]
        public void DA_09()
        {
            var geozone = SLVHelper.GenerateUniqueName("GZNDA09");
            var controller = SLVHelper.GenerateUniqueName("CTRL");
            var streetlight = SLVHelper.GenerateUniqueName("STL");
            var meter = SLVHelper.GenerateUniqueName("MTR");

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - Create following devices: Streetlight, Meter");
            Step(" - Refer to the below table for Attribute Name and Parrameter.");
            Step(" • Lamp level command: LampCommandLevel");
            Step(" • Lamp command mode: LampCommandMode");
            Step(" • Mains current: Current");
            Step(" • Mains voltage (V): MainVoltage");
            Step(" • Metered power (W): MeteredPower");
            Step(" • Temperature: Temperature");
            Step(" • Active energy (KWh): Energy");
            Step(" • Lamp burning hours: RunningHoursLamp");
            Step(" • Lamp current: LampCurrent");
            Step(" • Lamp level feedback: LampLevel");
            Step(" • Lamp switch command: LampCommandSwitch");
            Step(" • Driver temperature (°C): BallastTemp");
            Step(" • Lamp voltage (V): LampVoltage");
            Step(" • Sum power factor: PowerFactor");
            Step("**** Precondition ****\n");

            Step("-> Create data for testing");
            DeleteGeozones("GZNDA09*");
            CreateNewGeozone(geozone);
            CreateNewController(controller, geozone);
            CreateNewDevice(DeviceType.Streetlight, streetlight, controller, geozone);
            CreateNewDevice(DeviceType.ElectricalCounter, meter, controller, geozone);

            var attributes = new List<string>() { "Lamp level command:LampCommandLevel", "Lamp command mode:LampCommandMode", "Mains current:Current", "Mains voltage (V):MainVoltage", "Metered power (W):MeteredPower", "Temperature:Temperature", "Active energy (KWh):Energy", "Lamp burning hours:RunningHoursLamp", "Lamp current:LampCurrent", "Lamp level feedback:LampLevel", "Lamp switch command:LampCommandSwitch", "Driver temperature (°C):BallastTemp", "Lamp voltage (V):LampVoltage", "Sum power factor:PowerFactor" };
            var dicDataCommand = new Dictionary<string, int>();
            foreach (var attribute in attributes)
            {
                var randomValue = SLVHelper.GenerateInteger(10, 9999);
                dicDataCommand.Add(attribute, randomValue);
            }

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.DataHistory);

            Step("1. Go to Data History app");
            var dataHistoryPage = desktopPage.GoToApp(App.DataHistory) as DataHistoryPage;

            Step("2. Go to the geozone from the precondition");
            dataHistoryPage.GeozoneTreeMainPanel.SelectNode(geozone);
            dataHistoryPage.GridPanel.WaitForGridContentAvailable();

            Step("3. Send 3 commands to simulate the data for 3 random attributes of Metering");
            Step("4. Verify All 3 commands sent OK");
            var eventTime = Settings.GetServerTime();
            var randomData = dicDataCommand.PickRandom(3);
            for (int i = 0; i < randomData.Count; i++)
            {
                var attributeKey = randomData[i].Key.SplitAndGetAt(new char[] { ':' }, 1);
                var value = randomData[i].Value;
                var request = SetValueToDevice(controller, streetlight, attributeKey, value.ToString(), eventTime);
                VerifyEqual(string.Format("4. Verify the request {0} is sent successfully (attribute: {1}, value: {2})", i + 1, attributeKey, value), true, request);
            }

            Step("5. Select Search Fields icon on the main panel");
            dataHistoryPage.GridPanel.ClickSearchToolbarButton();
            dataHistoryPage.GridPanel.WaitForAdvancedSearchPanelDisplayed();

            Step("6. Fill the textbox Device with testing Streetlight's name and select operation IS");
            var deviceOperatior = "is";
            dataHistoryPage.GridPanel.SelectSearchCriteriaForOperatorDropdownField("Device", deviceOperatior);
            dataHistoryPage.GridPanel.EnterSearchCriteriaFor1stValueInputField("Device", streetlight);

            Step("7. Choose the operator 'different' for 3 attributes for which we just sent simulated commands");
            Step("8. Fill in the textbox with values different from the values in the simulated commands");
            for (int i = 0; i < randomData.Count; i++)
            {
                var attributeName = randomData[i].Key.SplitAndGetAt(new char[] { ':' }, 0);
                var value = randomData[i].Value + 1;
                dataHistoryPage.GridPanel.SelectSearchCriteriaForOperatorDropdownField(attributeName, "≠");
                dataHistoryPage.GridPanel.EnterSearchCriteriaFor1stValueInputField(attributeName, value.ToString());
            }

            Step("9. Press Search button");
            dataHistoryPage.GridPanel.ClickAdvancedSearchSearchButton();
            dataHistoryPage.GridPanel.WaitForAdvancedSearchPanelDisappeared();
            dataHistoryPage.GridPanel.WaitForLeftFooterTextDisplayed();

            Step("10. Verify The search result display ONLY the testing streetlight in the list");
            var devicesList = dataHistoryPage.GridPanel.GetListOfColumnData("Device");
            VerifyEqual("10. Verify The search result display ONLY the testing streetlight in the list", new List<string> { streetlight }, devicesList);

            Step("11. Select Search Fields icon again");
            dataHistoryPage.GridPanel.ClickSearchToolbarButton();
            dataHistoryPage.GridPanel.WaitForAdvancedSearchPanelDisplayed();

            Step("12. Verify The Search Fields screen displays with");
            Step(" o Text: Current search criteria: Device is {Streetlight's name},{Attribute 1's name} {value of attribute 1},{Attribute 2's name} {value of attribute 2} and {Attribute 3's name} >= {value of attribute 3}");
            Step(" o Ex: Device is DataHistorySL02,Lamp command mode 102");
            var expectedSearchCriteriaList = new List<string>();
            expectedSearchCriteriaList.Add(string.Format("Device is {0}", streetlight));
            for (int i = 0; i < randomData.Count; i++)
            {
                var attributeName = randomData[i].Key.SplitAndGetAt(new char[] { ':' }, 0);
                var value = randomData[i].Value + 1;
                expectedSearchCriteriaList.Add(string.Format("{0} {1}", attributeName, value));
            }
            var actualSearchCriteriaList = dataHistoryPage.GridPanel.GetListOfSearchCriteria();
            VerifyEqual("12. Verify The Search Title displays as expected", expectedSearchCriteriaList, actualSearchCriteriaList, false);
            Warning(string.Format("SC-1098: Data History - Some Current Search Criteria operators are not visible ({0})", string.Join(" , ", expectedSearchCriteriaList)));

            Step("13. Set randomly the value of an attribute to a value equal to the attribute's value");
            var rdmKey = randomData.PickRandom().Key;
            for (int i = 0; i < randomData.Count; i++)
            {
                var attributeName = randomData[i].Key.SplitAndGetAt(new char[] { ':' }, 0);
                var value = randomData[i].Value + 1;
                if (randomData[i].Key == rdmKey) value = randomData[i].Value;
                dataHistoryPage.GridPanel.SelectSearchCriteriaForOperatorDropdownField(attributeName, "≠");
                dataHistoryPage.GridPanel.EnterSearchCriteriaFor1stValueInputField(attributeName, value.ToString());
            }

            Step("14. Press Search button");
            dataHistoryPage.GridPanel.ClickAdvancedSearchSearchButton();
            dataHistoryPage.GridPanel.WaitForAdvancedSearchPanelDisappeared();
            dataHistoryPage.GridPanel.WaitForLeftFooterTextDisplayed();

            Step("15. Verify The search result is empty");
            devicesList = dataHistoryPage.GridPanel.GetListOfColumnData("Device");
            VerifyEqual("15. Verify The search result is empty", 0, devicesList.Count);

            Step("16. Select Search Fields icon again");
            dataHistoryPage.GridPanel.ClickSearchToolbarButton();
            dataHistoryPage.GridPanel.WaitForAdvancedSearchPanelDisplayed();

            Step("17. Verify the text of the Search Fields screen is updated the value of the attribute above");
            Step(" o Ex: Device is DataHistorySL02,Lamp command mode 103");
            expectedSearchCriteriaList.Clear();
            expectedSearchCriteriaList.Add(string.Format("Device is {0}", streetlight));
            for (int i = 0; i < randomData.Count; i++)
            {
                var attributeName = randomData[i].Key.SplitAndGetAt(new char[] { ':' }, 0);
                var value = randomData[i].Value + 1;
                if (randomData[i].Key == rdmKey) value = randomData[i].Value;
                expectedSearchCriteriaList.Add(string.Format("{0} {1}", attributeName, value));
            }
            actualSearchCriteriaList = dataHistoryPage.GridPanel.GetListOfSearchCriteria();
            VerifyEqual("17. Verify the text of the Search Fields screen is updated the value of the attribute above", expectedSearchCriteriaList, actualSearchCriteriaList, false);
            Warning(string.Format("SC-1098: Data History - Some Current Search Criteria operators are not visible ({0})", string.Join(" , ", expectedSearchCriteriaList)));

            Step("18. Set back the value different from the attribute's value, then press Search button.");
            for (int i = 0; i < randomData.Count; i++)
            {
                var attributeName = randomData[i].Key.SplitAndGetAt(new char[] { ':' }, 0);
                var value = randomData[i].Value + 1;
                dataHistoryPage.GridPanel.SelectSearchCriteriaForOperatorDropdownField(attributeName, "≠");
                dataHistoryPage.GridPanel.EnterSearchCriteriaFor1stValueInputField(attributeName, value.ToString());
            }
            dataHistoryPage.GridPanel.ClickAdvancedSearchSearchButton();
            dataHistoryPage.GridPanel.WaitForAdvancedSearchPanelDisappeared();
            dataHistoryPage.GridPanel.WaitForLeftFooterTextDisplayed();

            Step("19. Verify The search result display ONLY the testing streetlight in the list");
            devicesList = dataHistoryPage.GridPanel.GetListOfColumnData("Device");
            VerifyEqual("19. Verify The search result display ONLY the testing streetlight in the list", new List<string> { streetlight }, devicesList);

            try
            {
                DeleteGeozone(geozone);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("DA_10- Search data history of metering attributes using operators: IS")]
        public void DA_10()
        {
            var geozone = SLVHelper.GenerateUniqueName("GZNDA10");
            var controller = SLVHelper.GenerateUniqueName("CTRL");
            var streetlight = SLVHelper.GenerateUniqueName("STL");
            var meter = SLVHelper.GenerateUniqueName("MTR");

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - Create following devices: Streetlight, Meter");
            Step(" - Refer to the below table for Attribute Name and Parrameter.");
            Step(" • Lamp switch feedback: LampSwitch: ON/OFF");
            Step(" • High voltage: HighVoltage	true/false");
            Step(" • Lamp failure: LampFailure true/false");
            Step("**** Precondition ****\n");

            Step("-> Create data for testing");
            DeleteGeozones("GZNDA10*");
            CreateNewGeozone(geozone);
            CreateNewController(controller, geozone);
            CreateNewDevice(DeviceType.Streetlight, streetlight, controller, geozone);
            CreateNewDevice(DeviceType.ElectricalCounter, meter, controller, geozone);

            var attributes = new List<string>() { "Lamp switch feedback:LampSwitch", "High voltage:HighVoltage", "Lamp failure:LampFailure" };
            var dicDataCommand = new Dictionary<string, string>();
            foreach (var attribute in attributes)
            {
                dicDataCommand.Add(attribute, "True");
            }

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.DataHistory);

            Step("1. Go to Data History app");
            var dataHistoryPage = desktopPage.GoToApp(App.DataHistory) as DataHistoryPage;

            Step("2. Go to the geozone from the precondition");
            dataHistoryPage.GeozoneTreeMainPanel.SelectNode(geozone);
            dataHistoryPage.GridPanel.WaitForGridContentAvailable();

            Step("3. Send 1 command to simulate the 'TRUE' data for 1 random attribute of Metering");
            Step("4. Verify The command sent OK");
            var randomData = dicDataCommand.PickRandom();
            var attributeName = randomData.Key.SplitAndGetAt(new char[] { ':' }, 0);
            var attributeKey = randomData.Key.SplitAndGetAt(new char[] { ':' }, 1);
            var value = randomData.Value;
            if (attributeKey.Equals("LampSwitch")) value = "ON";
            var request = SetValueToDevice(controller, streetlight, attributeKey, value, Settings.GetServerTime());
            VerifyEqual(string.Format("4. Verify the request is sent successfully (attribute: {0}, value: {1})", attributeKey, value), true, request);

            Step("5. Select Search Fields icon on the main panel");
            dataHistoryPage.GridPanel.ClickSearchToolbarButton();
            dataHistoryPage.GridPanel.WaitForAdvancedSearchPanelDisplayed();

            Step("6. Fill the textbox Device with testing Streetlight's name and select operation IS");
            dataHistoryPage.GridPanel.SelectSearchCriteriaForOperatorDropdownField("Device", "is");
            dataHistoryPage.GridPanel.EnterSearchCriteriaFor1stValueInputField("Device", streetlight);

            Step("7. Choose the operator 'Is' for the attribute for which we just sent the simulated command");
            dataHistoryPage.GridPanel.SelectSearchCriteriaForOperatorDropdownField(attributeName, "is");

            Step("8. Select the option 'True'");
            dataHistoryPage.GridPanel.SelectSearchCriteriaForValueDropdownField(attributeName, "True");

            Step("9. Press Search button");
            dataHistoryPage.GridPanel.ClickAdvancedSearchSearchButton();
            dataHistoryPage.GridPanel.WaitForAdvancedSearchPanelDisappeared();
            dataHistoryPage.GridPanel.WaitForLeftFooterTextDisplayed();

            Step("10. Verify The search result display ONLY the testing streetlight in the list");
            var devicesList = dataHistoryPage.GridPanel.GetListOfColumnData("Device");
            VerifyEqual("10. Verify The search result display ONLY the testing streetlight in the list", new List<string> { streetlight }, devicesList);

            Step("11. Select Search Fields icon again");
            dataHistoryPage.GridPanel.ClickSearchToolbarButton();
            dataHistoryPage.GridPanel.WaitForAdvancedSearchPanelDisplayed();

            Step("12. Verify The Search Fields screen displays with");
            Step(" o Text: Current search criteria:Device is {Streetlight's name} and {Attribute 1's name} is true");
            Step(" o Ex: Device is DataHistorySL02 and Lamp failure is true");
            var expectedSearchCriteria = string.Format("Current search criteria:Device is {0} and {1} is true", streetlight, attributeName);
            var actualSearchCriteria = dataHistoryPage.GridPanel.GetSearchCriteria();
            VerifyEqual("12. Verify The Search Title displays as expected", expectedSearchCriteria, actualSearchCriteria);

            Step("13. Change the option to 'False' and press Search button");
            dataHistoryPage.GridPanel.SelectSearchCriteriaForValueDropdownField(attributeName, "False");
            dataHistoryPage.GridPanel.ClickAdvancedSearchSearchButton();
            dataHistoryPage.GridPanel.WaitForAdvancedSearchPanelDisappeared();
            dataHistoryPage.GridPanel.WaitForLeftFooterTextDisplayed();

            Step("14. Verify The search result is empty");
            devicesList = dataHistoryPage.GridPanel.GetListOfColumnData("Device");
            VerifyEqual("14. Verify The search result is empty", 0, devicesList.Count);

            Step("15. Select Search Fields icon again");
            dataHistoryPage.GridPanel.ClickSearchToolbarButton();
            dataHistoryPage.GridPanel.WaitForAdvancedSearchPanelDisplayed();

            Step("16. Verify the text of the Search Fields screen is updated the value of the attribute above");
            Step(" o Text: Current search criteria:Device is {Streetlight's name} and {Attribute 1's name} is false");
            expectedSearchCriteria = string.Format("Current search criteria:Device is {0} and {1} is false", streetlight, attributeName);
            actualSearchCriteria = dataHistoryPage.GridPanel.GetSearchCriteria();
            VerifyEqual("16. Verify The Search Title displays as expected", expectedSearchCriteria, actualSearchCriteria);

            Step("17. Click Reset search");
            dataHistoryPage.GridPanel.ClickAdvancedSearchResetButton();
            dataHistoryPage.GridPanel.WaitForAdvancedSearchPanelDisappeared();

            Step("18. Send 1 command to simulate the 'False' data for the testing attribute of Metering");
            Step("19. Verify The command sent OK");
            value = "False";
            if (attributeKey.Equals("LampSwitch")) value = "OFF";
            request = SetValueToDevice(controller, streetlight, attributeKey, value, Settings.GetServerTime());
            VerifyEqual(string.Format("19. Verify the request is sent successfully (attribute: {0}, value: {1})", attributeKey, value), true, request);

            Step("20. Select Search Fields icon on the main panel");
            dataHistoryPage.GridPanel.ClickSearchToolbarButton();
            dataHistoryPage.GridPanel.WaitForAdvancedSearchPanelDisplayed();

            Step("21. Fill the textbox Device with testing Streetlight's name and select operation IS");
            dataHistoryPage.GridPanel.SelectSearchCriteriaForOperatorDropdownField("Device", "is");
            dataHistoryPage.GridPanel.EnterSearchCriteriaFor1stValueInputField("Device", streetlight);

            Step("22. Choose the operator 'Is' for the attribute and select the option 'False'");
            dataHistoryPage.GridPanel.SelectSearchCriteriaForOperatorDropdownField(attributeName, "is");
            dataHistoryPage.GridPanel.SelectSearchCriteriaForValueDropdownField(attributeName, "False");

            Step("23. Press Search button");
            dataHistoryPage.GridPanel.ClickAdvancedSearchSearchButton();
            dataHistoryPage.GridPanel.WaitForAdvancedSearchPanelDisappeared();
            dataHistoryPage.GridPanel.WaitForLeftFooterTextDisplayed();

            Step("24. Verify The search result display ONLY the testing streetlight in the list");
            devicesList = dataHistoryPage.GridPanel.GetListOfColumnData("Device");
            VerifyEqual("24. Verify The search result display ONLY the testing streetlight in the list", new List<string> { streetlight }, devicesList);

            Step("25. Select Search Fields icon again");
            dataHistoryPage.GridPanel.ClickSearchToolbarButton();
            dataHistoryPage.GridPanel.WaitForAdvancedSearchPanelDisplayed();

            Step("26. Verify The Search Fields screen displays with");
            Step(" o Text: Current search criteria:Device is {Streetlight's name} and {Attribute 1's name} is false");
            Step(" o Ex: Device is DataHistorySL02 and Lamp failure is false");
            expectedSearchCriteria = string.Format("Current search criteria:Device is {0} and {1} is false", streetlight, attributeName);
            actualSearchCriteria = dataHistoryPage.GridPanel.GetSearchCriteria();
            VerifyEqual("26. Verify The Search Title displays as expected", expectedSearchCriteria, actualSearchCriteria);

            Step("27. Change the option to 'True' and press Search button");
            dataHistoryPage.GridPanel.SelectSearchCriteriaForValueDropdownField(attributeName, "True");
            dataHistoryPage.GridPanel.ClickAdvancedSearchSearchButton();
            dataHistoryPage.GridPanel.WaitForAdvancedSearchPanelDisappeared();
            dataHistoryPage.GridPanel.WaitForLeftFooterTextDisplayed();

            Step("28. Verify The search result is empty");
            devicesList = dataHistoryPage.GridPanel.GetListOfColumnData("Device");
            VerifyEqual("28. Verify The search result is empty", 0, devicesList.Count);

            Step("29. Select Search Fields icon again");
            dataHistoryPage.GridPanel.ClickSearchToolbarButton();
            dataHistoryPage.GridPanel.WaitForAdvancedSearchPanelDisplayed();

            Step("29. Verify the text of the Search Fields screen is updated the value of the attribute above");
            Step(" o Text: Current search criteria:Device is {Streetlight's name} and {Attribute 1's name} is true");
            expectedSearchCriteria = string.Format("Current search criteria:Device is {0} and {1} is true", streetlight, attributeName);
            actualSearchCriteria = dataHistoryPage.GridPanel.GetSearchCriteria();
            VerifyEqual("29. Verify The Search Title displays as expected", expectedSearchCriteria, actualSearchCriteria);

            try
            {
                DeleteGeozone(geozone);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("DA_11- Search data history of metering attributes using operators: IN")]
        public void DA_11()
        {
            var testData = GetTestDataOfDA_11();           
            var equipmentTypes = testData["EquipmentTypes"] as Dictionary<string, string>;
            var lampTypes = testData["LampTypes"] as Dictionary<string, string>;
            var dimmingGroups = testData["DimmingGroups"] as List<string>;
            var geozone = SLVHelper.GenerateUniqueName("GZNDA11");
            var controller = SLVHelper.GenerateUniqueName("CTRL");
            var streetlight = SLVHelper.GenerateUniqueName("STL");
            var meter = SLVHelper.GenerateUniqueName("MTR");
            var equipmentType = equipmentTypes.PickRandom();
            var equipmentType1 = equipmentTypes.Where(p => p.Key != equipmentType.Key).PickRandom();
            var lampType = lampTypes.PickRandom();
            var lampType1 = lampTypes.Where(p => p.Key != lampType.Key).PickRandom();
            var dimmingGroup = dimmingGroups.PickRandom();
            var dimmingGroup1 = dimmingGroups.Where(p => p != dimmingGroup).PickRandom();

            var dicAttributesDataCorrect = new Dictionary<string, string>();
            dicAttributesDataCorrect.Add("Dimming group", dimmingGroup);
            dicAttributesDataCorrect.Add("Lamp Type", lampType.Value);
            dicAttributesDataCorrect.Add("Type of equipment", equipmentType.Value);
            var dicAttributesDataWrong = new Dictionary<string, string>();
            dicAttributesDataWrong.Add("Dimming group", dimmingGroup1);
            dicAttributesDataWrong.Add("Lamp Type", lampType1.Value);
            dicAttributesDataWrong.Add("Type of equipment", equipmentType1.Value);
            var attributes = new List<string>() { "Dimming group", "Lamp Type", "Type of equipment" };

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - Create following devices: Streetlight, Meter");
            Step(" - Set values for all below attributes of the testing streetlight");
            Step(" • Dimming group");
            Step(" • Lamp Type");
            Step(" • Type of equipment");
            Step("**** Precondition ****\n");

            Step("-> Create data for testing");
            DeleteGeozones("GZNDA11*");
            CreateNewGeozone(geozone);
            CreateNewController(controller, geozone);
            CreateNewDevice(DeviceType.Streetlight, streetlight, controller, geozone, equipmentType.Value);
            CreateNewDevice(DeviceType.ElectricalCounter, meter, controller, geozone);            
            SetValueToDevice(controller, streetlight, "DimmingGroupName", dimmingGroup, Settings.GetServerTime());            
            SetValueToDevice(controller, streetlight, "brandId", lampType.Key, Settings.GetServerTime());            

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.DataHistory, App.EquipmentInventory);           

            Step("1. Go to Data History app");           
            var dataHistoryPage = desktopPage.GoToApp(App.DataHistory) as DataHistoryPage;

            Step("2. Go to the geozone from the precondition");
            dataHistoryPage.GeozoneTreeMainPanel.SelectNode(geozone);
            dataHistoryPage.GridPanel.WaitForGridContentAvailable();
            
            Step("3. Select Search Fields icon on the main panel");
            dataHistoryPage.GridPanel.ClickSearchToolbarButton();
            dataHistoryPage.GridPanel.WaitForAdvancedSearchPanelDisplayed();

            Step("4. Fill the textbox Device with testing Streetlight's name and select operator IS");            
            dataHistoryPage.GridPanel.SelectSearchCriteriaForOperatorDropdownField("Device", "is");
            dataHistoryPage.GridPanel.EnterSearchCriteriaFor1stValueInputField("Device", streetlight);

            Step("5. Choose the operator 'In' for the 3 attributes");
            Step("6. Click the textbox next to the operator of each field and enter the attribute's value");
            var operatorSearch = "in";
            foreach (var attribute in attributes)
            {
                dataHistoryPage.GridPanel.SelectSearchCriteriaForOperatorDropdownField(attribute, operatorSearch);
                dataHistoryPage.GridPanel.SelectSearchCriteriaForValueMultipleDropdownField(attribute, dicAttributesDataCorrect[attribute]);
            }

            Step("7. Press Search button");
            dataHistoryPage.GridPanel.ClickAdvancedSearchSearchButton();
            dataHistoryPage.GridPanel.WaitForAdvancedSearchPanelDisappeared();
            dataHistoryPage.GridPanel.WaitForLeftFooterTextDisplayed();

            Step("8. Verify The search result display ONLY the testing streetlight in the list -Note: there is a bug, failed to search for Type Of Equipment field, wait for JM confirmed");
            var devicesList = dataHistoryPage.GridPanel.GetListOfColumnData("Device");
            VerifyEqual("[SC-1143] 8. Verify The search result display ONLY the testing streetlight in the list", new List<string> { streetlight }, devicesList);

            Step("9. Select Search Fields icon again");
            dataHistoryPage.GridPanel.ClickSearchToolbarButton();
            dataHistoryPage.GridPanel.WaitForAdvancedSearchPanelDisplayed();

            Step("10. Verify The Search Fields screen displays with");
            Step(" o Text: Current search criteria: Device is {Streetlight's name} and {Attribute 1's name} in {Attribute 1's value} and {Attribute 2's name} in {Attribute 2's value} and {Attribute 3's name} in {Attribute 3's value}");
            Step(" o Ex: Device is DataHistorySL02 and Lamp Type in HPS 100W Ferro");
            var expectedSearchCriteriaList = new List<string>();
            expectedSearchCriteriaList.Add(string.Format("Device is {0}", streetlight));
            foreach (var attribute in attributes)
            {
                expectedSearchCriteriaList.Add(string.Format("{0} in {1}", attribute, dicAttributesDataCorrect[attribute]));
            }

            var actualSearchCriteriaList = dataHistoryPage.GridPanel.GetListOfSearchCriteria();
            VerifyEqual("[SC-1151] 10. Verify The Search Title displays as expected", expectedSearchCriteriaList, actualSearchCriteriaList, false);

            Step("11. Add another value for each field and press Search button");
            foreach (var attribute in attributes)
            {
                dataHistoryPage.GridPanel.SelectSearchCriteriaForOperatorDropdownField(attribute, operatorSearch);
                dataHistoryPage.GridPanel.SelectSearchCriteriaForValueMultipleDropdownField(attribute, dicAttributesDataWrong[attribute]);
            }
            dataHistoryPage.GridPanel.ClickAdvancedSearchSearchButton();
            dataHistoryPage.GridPanel.WaitForAdvancedSearchPanelDisappeared();
            dataHistoryPage.GridPanel.WaitForLeftFooterTextDisplayed();

            Step("12. Verify The search result display ONLY the testing streetlight in the list -Note: there is a bug, failed to search multiple options for Lamp Type and Type Of Equipment field, wait for JM confirmed");
            devicesList = dataHistoryPage.GridPanel.GetListOfColumnData("Device");
            VerifyEqual("[SC-1143] 12. Verify The search result display ONLY the testing streetlight in the list", new List<string> { streetlight }, devicesList);

            Step("13. Select Search Fields icon again");
            dataHistoryPage.GridPanel.ClickSearchToolbarButton();
            dataHistoryPage.GridPanel.WaitForAdvancedSearchPanelDisplayed();

            Step("14. Verify the text of the Search Fields screen is updated the value of the attribute above");
            Step("o Text: Current search criteria: Device is {Streetlight's name}, {Attribute 1's name} in {Attribute 1's value 1; Attribute 2's value 2} and {Attribute 2's name} in {Attribute 2's value 1; Attribute 2's value 2} and {Attribute 3's name} in {Attribute 3's value 1; Attribute 3's value 2}");
            Step("o Note: bug in the text, SC-1098");
            expectedSearchCriteriaList = new List<string>();
            expectedSearchCriteriaList.Add(string.Format("Device is {0}", streetlight));
            foreach (var attribute in attributes)
            {
                expectedSearchCriteriaList.Add(string.Format("{0} in {1},{2}", attribute, dicAttributesDataCorrect[attribute], dicAttributesDataWrong[attribute]));
            }
            actualSearchCriteriaList = dataHistoryPage.GridPanel.GetListOfSearchCriteria();
            VerifyEqual("[SC-1151] 14. Verify The Search Title displays as expected", expectedSearchCriteriaList, actualSearchCriteriaList, false);

            Step("15. Remove randomly a value of a search field that match an attribute's value and search again");
            var randomData = dicAttributesDataCorrect.PickRandom();
            dataHistoryPage.GridPanel.RemoveSearchCriteriaForValueDropdownField(randomData.Key, randomData.Value);
            dataHistoryPage.GridPanel.ClickAdvancedSearchSearchButton();
            dataHistoryPage.GridPanel.WaitForAdvancedSearchPanelDisappeared();
            dataHistoryPage.GridPanel.WaitForLeftFooterTextDisplayed();

            Step("16. Verify The search result is empty");
            devicesList = dataHistoryPage.GridPanel.GetListOfColumnData("Device");
            VerifyEqual("16. Verify The search result is empty", 0, devicesList.Count);

            Step("17. Select Search Fields icon again");
            dataHistoryPage.GridPanel.ClickSearchToolbarButton();
            dataHistoryPage.GridPanel.WaitForAdvancedSearchPanelDisplayed();

            Step("18. Verify the text of the Search Fields screen is updated the text");
            expectedSearchCriteriaList = new List<string>();
            expectedSearchCriteriaList.Add(string.Format("Device is {0}", streetlight));
            foreach (var attribute in attributes)
            {
                if(attribute == randomData.Key)
                    expectedSearchCriteriaList.Add(string.Format("{0} in {1}", attribute, dicAttributesDataWrong[attribute]));
                else
                    expectedSearchCriteriaList.Add(string.Format("{0} in {1},{2}", attribute, dicAttributesDataCorrect[attribute], dicAttributesDataWrong[attribute]));
            }

            actualSearchCriteriaList = dataHistoryPage.GridPanel.GetListOfSearchCriteria();
            VerifyEqual("[SC-1151] 18. Verify The Search Title displays as expected", expectedSearchCriteriaList, actualSearchCriteriaList, false);

            Step("19. Add the removed value again and press Search");
            dataHistoryPage.GridPanel.SelectSearchCriteriaForValueMultipleDropdownField(randomData.Key, randomData.Value);
            dataHistoryPage.GridPanel.ClickAdvancedSearchSearchButton();
            dataHistoryPage.GridPanel.WaitForAdvancedSearchPanelDisappeared();
            dataHistoryPage.GridPanel.WaitForLeftFooterTextDisplayed();

            Step("20. Verify The search result display ONLY the testing streetlight in the list");
            devicesList = dataHistoryPage.GridPanel.GetListOfColumnData("Device");
            VerifyEqual("20. Verify The search result display ONLY the testing streetlight in the list", new List<string> { streetlight }, devicesList);           

            try
            {
                DeleteGeozone(geozone);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("DA_12- Search data history of metering attributes using operators- NOT IN")]
        [NonParallelizable]
        public void DA_12()
        {
            var testData = GetTestDataOfDA_12();
            var equipmentTypes = testData["EquipmentTypes"] as Dictionary<string, string>;
            var lampTypes = testData["LampTypes"] as Dictionary<string, string>;
            var dimmingGroups = testData["DimmingGroups"] as List<string>;
            var geozone = SLVHelper.GenerateUniqueName("GZNDA12");
            var controller = SLVHelper.GenerateUniqueName("CTRL");
            var streetlight = SLVHelper.GenerateUniqueName("STL");
            var meter = SLVHelper.GenerateUniqueName("MTR");
            var equipmentType = equipmentTypes.PickRandom();
            var equipmentType1 = equipmentTypes.Where(p => p.Key != equipmentType.Key).PickRandom(2);
            var lampType = lampTypes.PickRandom();
            var lampType1 = lampTypes.Where(p => p.Key != lampType.Key).PickRandom(2);
            var dimmingGroup = dimmingGroups.PickRandom();
            var dimmingGroup1 = dimmingGroups.Where(p => p != dimmingGroup).PickRandom(2);

            var dicAttributesDataCorrect = new Dictionary<string, string>();
            dicAttributesDataCorrect.Add("Dimming group", dimmingGroup);
            dicAttributesDataCorrect.Add("Lamp Type", lampType.Value);
            dicAttributesDataCorrect.Add("Type of equipment", equipmentType.Value);
            var dicAttributesDataWrong = new Dictionary<string, List<string>>();
            dicAttributesDataWrong.Add("Dimming group", dimmingGroup1);
            dicAttributesDataWrong.Add("Lamp Type", lampType1.Select(p => p.Value).ToList());
            dicAttributesDataWrong.Add("Type of equipment", equipmentType1.Select(p => p.Value).ToList());
            var attributes = new List<string>() { "Dimming group", "Lamp Type", "Type of equipment" };

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - Create following devices: Streetlight, Meter");
            Step(" - Set values for all below attributes of the testing streetlight");
            Step(" • Dimming group");
            Step(" • Lamp Type");
            Step(" • Type of equipment");
            Step("**** Precondition ****\n");

            Step("-> Create data for testing");
            DeleteGeozones("GZNDA12*");
            CreateNewGeozone(geozone);
            CreateNewController(controller, geozone);
            CreateNewDevice(DeviceType.Streetlight, streetlight, controller, geozone, equipmentType.Value);
            CreateNewDevice(DeviceType.ElectricalCounter, meter, controller, geozone);
            SetValueToDevice(controller, streetlight, "DimmingGroupName", dimmingGroup, Settings.GetServerTime());
            SetValueToDevice(controller, streetlight, "brandId", lampType.Key, Settings.GetServerTime());            
            
            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.DataHistory);

            Step("1. Go to Data History app");
            var dataHistoryPage = desktopPage.GoToApp(App.DataHistory) as DataHistoryPage;

            Step("2. Go to the geozone from the precondition");
            dataHistoryPage.GeozoneTreeMainPanel.SelectNode(geozone);
            dataHistoryPage.GridPanel.WaitForGridContentAvailable();

            Step("3. Select Search Fields icon on the main panel");
            dataHistoryPage.GridPanel.ClickSearchToolbarButton();
            dataHistoryPage.GridPanel.WaitForAdvancedSearchPanelDisplayed();

            Step("4. Fill the textbox Device with testing Streetlight's name and select operator IS");
            dataHistoryPage.GridPanel.SelectSearchCriteriaForOperatorDropdownField("Device", "is");
            dataHistoryPage.GridPanel.EnterSearchCriteriaFor1stValueInputField("Device", streetlight);

            Step("5. Choose the operator 'Not In' for the 3 attributes");
            Step("6. Click the textbox next to the operator of each field and enter the attribute's value");
            var operatorSearch = "not in";
            foreach (var attribute in attributes)
            {
                dataHistoryPage.GridPanel.SelectSearchCriteriaForOperatorDropdownField(attribute, operatorSearch);
                dataHistoryPage.GridPanel.SelectSearchCriteriaForValueMultipleDropdownField(attribute, dicAttributesDataWrong[attribute][0]);
            }

            Step("7. Press Search button");
            dataHistoryPage.GridPanel.ClickAdvancedSearchSearchButton();
            dataHistoryPage.GridPanel.WaitForAdvancedSearchPanelDisappeared();
            dataHistoryPage.GridPanel.WaitForLeftFooterTextDisplayed();

            Step("8. Verify The search result display ONLY the testing streetlight in the list -Note: there is a bug, failed to search for Type Of Equipment field- SC-1143");
            var devicesList = dataHistoryPage.GridPanel.GetListOfColumnData("Device");
            VerifyEqual("[SC-1143] 8. Verify The search result display ONLY the testing streetlight in the list", new List<string> { streetlight }, devicesList);

            Step("9. Select Search Fields icon again");
            dataHistoryPage.GridPanel.ClickSearchToolbarButton();
            dataHistoryPage.GridPanel.WaitForAdvancedSearchPanelDisplayed();

            Step("10. Verify The Search Fields screen displays with");
            Step(" o Text: Current search criteria: Device is {Streetlight's name}, {Attribute 1's name} not in {value1} and {Attribute 2's name} not in {value2} and {Attribute 3's name} not in {value3}");
            Step(" o Ex: Device is DataHistorySL02, Lamp Type not in HPS 100W Ferro");
            Step(" o Note: bug in the text, SC-1151");
            var expectedSearchCriteriaList = new List<string>();
            expectedSearchCriteriaList.Add(string.Format("Device is {0}", streetlight));
            foreach (var attribute in attributes)
            {
                expectedSearchCriteriaList.Add(string.Format("{0} not in {1}", attribute, dicAttributesDataWrong[attribute][0]));
            }

            var actualSearchCriteriaList = dataHistoryPage.GridPanel.GetListOfSearchCriteria();
            VerifyEqual("[SC-1151] 10. Verify The Search Title displays as expected", expectedSearchCriteriaList, actualSearchCriteriaList, false);

            Step("11. Add another value which is not the real attribute's value for each field and press Search buttonn");
            foreach (var attribute in attributes)
            {
                dataHistoryPage.GridPanel.SelectSearchCriteriaForOperatorDropdownField(attribute, operatorSearch);
                dataHistoryPage.GridPanel.SelectSearchCriteriaForValueMultipleDropdownField(attribute, dicAttributesDataWrong[attribute][1]);
            }
            dataHistoryPage.GridPanel.ClickAdvancedSearchSearchButton();
            dataHistoryPage.GridPanel.WaitForAdvancedSearchPanelDisappeared();
            dataHistoryPage.GridPanel.WaitForLeftFooterTextDisplayed();

            Step("12. Verify The search result display ONLY the testing streetlight in the list -Note: there is a bug, failed to search multiple options for Lamp Type and Type Of Equipment field- SC-1153");
            devicesList = dataHistoryPage.GridPanel.GetListOfColumnData("Device");
            VerifyEqual("[SC-1143] 12. Verify The search result display ONLY the testing streetlight in the list", new List<string> { streetlight }, devicesList);

            Step("13. Select Search Fields icon again");
            dataHistoryPage.GridPanel.ClickSearchToolbarButton();
            dataHistoryPage.GridPanel.WaitForAdvancedSearchPanelDisplayed();

            Step("14. Verify the text of the Search Fields screen is updated the value of the attribute above");
            Step("o Text: Current search criteria: Device is {Streetlight's name}, {Attribute 1's name} not in {value1,value4} and {Attribute 2's name} not in {value2; value5} and {Attribute 3's name} not in {value3,value6}");
            Step("o Note: bug in the text, SC-1151");
            expectedSearchCriteriaList = new List<string>();
            expectedSearchCriteriaList.Add(string.Format("Device is {0}", streetlight));
            foreach (var attribute in attributes)
            {
                expectedSearchCriteriaList.Add(string.Format("{0} not in {1},{2}", attribute, dicAttributesDataWrong[attribute][0], dicAttributesDataWrong[attribute][1]));
            }
            actualSearchCriteriaList = dataHistoryPage.GridPanel.GetListOfSearchCriteria();
            VerifyEqual("[SC-1151] 14. Verify The Search Title displays as expected", expectedSearchCriteriaList, actualSearchCriteriaList, false);

            Step("15. Add randomly a value of a search field that match an attribute's value and search again");
            var randomData = dicAttributesDataCorrect.PickRandom();
            dataHistoryPage.GridPanel.SelectSearchCriteriaForValueMultipleDropdownField(randomData.Key, randomData.Value);
            dataHistoryPage.GridPanel.ClickAdvancedSearchSearchButton();
            dataHistoryPage.GridPanel.WaitForAdvancedSearchPanelDisappeared();
            dataHistoryPage.GridPanel.WaitForLeftFooterTextDisplayed();

            Step("16. Verify The search result is empty");
            devicesList = dataHistoryPage.GridPanel.GetListOfColumnData("Device");
            VerifyEqual("16. Verify The search result is empty", 0, devicesList.Count);           

            try
            {
                DeleteGeozone(geozone);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("DA_13- Search data history of metering attributes using operators: IS")]
        public void DA_13()
        {
            var geozone = SLVHelper.GenerateUniqueName("GZNDA13");
            var controller = SLVHelper.GenerateUniqueName("CTRL");
            var streetlight = SLVHelper.GenerateUniqueName("STL");
            var meter = SLVHelper.GenerateUniqueName("MTR");
            var uniqueAddress = SLVHelper.GenerateMACAddress();            

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - Create following devices: Streetlight, Meter");
            Step(" - Set values for all below attributes of the testing streetlight");
            Step(" • Device");
            Step(" • Unique address");
            Step("**** Precondition ****\n");

            Step("-> Create data for testing");
            DeleteGeozones("GZNDA13*");
            CreateNewGeozone(geozone);
            CreateNewController(controller, geozone);
            CreateNewDevice(DeviceType.Streetlight, streetlight, controller, geozone);
            CreateNewDevice(DeviceType.ElectricalCounter, meter, controller, geozone);
            SetValueToDevice(controller, streetlight, "MacAddress", uniqueAddress, Settings.GetServerTime());            

            var attributes = new List<string>() {"Device", "Unique address"};
            var dicAttributesData = new Dictionary<string, string>();
            dicAttributesData.Add("Device", streetlight);
            dicAttributesData.Add("Unique address", uniqueAddress);

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.DataHistory);

            Step("1. Go to Data History app");
            var dataHistoryPage = desktopPage.GoToApp(App.DataHistory) as DataHistoryPage;

            Step("2. Go to the geozone from the precondition");
            dataHistoryPage.GeozoneTreeMainPanel.SelectNode(geozone);
            dataHistoryPage.GridPanel.WaitForGridContentAvailable();

            Step("3. Select Search Fields icon on the main panel");
            dataHistoryPage.GridPanel.ClickSearchToolbarButton();
            dataHistoryPage.GridPanel.WaitForAdvancedSearchPanelDisplayed();

            Step("4. Fill the textbox Device with testing Streetlight's name and select operator IS");
            dataHistoryPage.GridPanel.SelectSearchCriteriaForOperatorDropdownField("Device", "is");
            dataHistoryPage.GridPanel.EnterSearchCriteriaFor1stValueInputField("Device", streetlight);

            Step("5. Fill the textbox Unique address with the testing Streetlight's unique address and select the operator IS");
            dataHistoryPage.GridPanel.SelectSearchCriteriaForOperatorDropdownField("Unique address", "is");
            dataHistoryPage.GridPanel.EnterSearchCriteriaFor1stValueInputField("Unique address", uniqueAddress);

            Step("6. Press Search button");
            dataHistoryPage.GridPanel.ClickAdvancedSearchSearchButton();
            dataHistoryPage.GridPanel.WaitForAdvancedSearchPanelDisappeared();
            dataHistoryPage.GridPanel.WaitForLeftFooterTextDisplayed();

            Step("7. Verify The search result display ONLY the testing streetlight in the list");
            var devicesList = dataHistoryPage.GridPanel.GetListOfColumnData("Device");
            VerifyEqual("7. Verify The search result display ONLY the testing streetlight in the list", new List<string> { streetlight }, devicesList);

            Step("8. Select Search Fields icon again");
            dataHistoryPage.GridPanel.ClickSearchToolbarButton();
            dataHistoryPage.GridPanel.WaitForAdvancedSearchPanelDisplayed();

            Step("9. Verify The Search Fields screen displays with");
            Step(" o Text: Current search criteria: Device is {Streetlight's name} and Unique address is 'unique address value'");
            Step(" o Ex: Device is DataHistorySL02 and Unique address is AF0D30FD5F4B");
            var expectedSearchCriteria = string.Format("Current search criteria:Device is {0} and Unique address is {1}", streetlight, uniqueAddress);   
            var actualSearchCriteria = dataHistoryPage.GridPanel.GetSearchCriteria();
            VerifyEqual("9. Verify The Search Title displays as expected", expectedSearchCriteria, actualSearchCriteria);

            Step("10. Update randomly a field with the invalid value and press Search button");
            var randomData = dicAttributesData.PickRandom();
            var newValue = SLVHelper.GenerateString(10);
            dataHistoryPage.GridPanel.EnterSearchCriteriaFor1stValueInputField(randomData.Key, newValue);
            dataHistoryPage.GridPanel.ClickAdvancedSearchSearchButton();
            dataHistoryPage.GridPanel.WaitForAdvancedSearchPanelDisappeared();
            dataHistoryPage.GridPanel.WaitForLeftFooterTextDisplayed();
            
            Step("11. Verify The search result is empty");
            devicesList = dataHistoryPage.GridPanel.GetListOfColumnData("Device");
            VerifyEqual("11. Verify The search result is empty", 0, devicesList.Count);

            Step("12. Select Search Fields icon again");
            dataHistoryPage.GridPanel.ClickSearchToolbarButton();
            dataHistoryPage.GridPanel.WaitForAdvancedSearchPanelDisplayed();
            
            Step("13. Verify the text of the Search Fields screen is updated the text");
            if(randomData.Key.Equals("Device"))
                expectedSearchCriteria = string.Format("Current search criteria:Device is {0} and Unique address is {1}", newValue, uniqueAddress);
            else
                expectedSearchCriteria = string.Format("Current search criteria:Device is {0} and Unique address is {1}", streetlight, newValue);
            actualSearchCriteria = dataHistoryPage.GridPanel.GetSearchCriteria();
            VerifyEqual("13. Verify The Search Title displays as expected", expectedSearchCriteria, actualSearchCriteria);

            Step("14. Update to the exact value and press Search button");
            dataHistoryPage.GridPanel.EnterSearchCriteriaFor1stValueInputField(randomData.Key, randomData.Value);
            dataHistoryPage.GridPanel.ClickAdvancedSearchSearchButton();
            dataHistoryPage.GridPanel.WaitForAdvancedSearchPanelDisappeared();
            dataHistoryPage.GridPanel.WaitForLeftFooterTextDisplayed();

            Step("15. Verify The search result display ONLY the testing streetlight in the list");
            devicesList = dataHistoryPage.GridPanel.GetListOfColumnData("Device");
            VerifyEqual("15. Verify The search result display ONLY the testing streetlight in the list", new List<string> { streetlight }, devicesList);

            try
            {
                DeleteGeozone(geozone);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("DA_14- Search data history of metering attributes using operators: CONTAINS")]
        public void DA_14()
        {
            var geozone = SLVHelper.GenerateUniqueName("GZNDA14");
            var controller = SLVHelper.GenerateUniqueName("CTRL");
            var streetlight = SLVHelper.GenerateUniqueName("STL");
            var meter = SLVHelper.GenerateUniqueName("MTR");
            var uniqueAddress = SLVHelper.GenerateMACAddress();

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - Create following devices: Streetlight, Meter");
            Step(" - Set values for all below attributes of the testing streetlight");
            Step(" • Device");
            Step(" • Unique address");
            Step("**** Precondition ****\n");

            Step("-> Create data for testing");
            DeleteGeozones("GZNDA14*");
            CreateNewGeozone(geozone);
            CreateNewController(controller, geozone);
            CreateNewDevice(DeviceType.Streetlight, streetlight, controller, geozone);
            CreateNewDevice(DeviceType.ElectricalCounter, meter, controller, geozone);
            SetValueToDevice(controller, streetlight, "MacAddress", uniqueAddress, Settings.GetServerTime());            
            
            var attributes = new List<string>() { "Device", "Unique address" };
            var dicAttributesData = new Dictionary<string, string>();
            dicAttributesData.Add("Device", streetlight);
            dicAttributesData.Add("Unique address", uniqueAddress);
            var partOfStreelight = streetlight.Substring(0, streetlight.Length / 2);
            var partOfUniqueAddress = uniqueAddress.Substring(0, uniqueAddress.Length / 2);

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.DataHistory);          
            
            Step("1. Go to Data History app");
            var dataHistoryPage = desktopPage.GoToApp(App.DataHistory) as DataHistoryPage;

            Step("2. Go to the geozone from the precondition");
            dataHistoryPage.GeozoneTreeMainPanel.SelectNode(geozone);
            dataHistoryPage.GridPanel.WaitForGridContentAvailable();

            Step("3. Select Search Fields icon on the main panel");
            dataHistoryPage.GridPanel.ClickSearchToolbarButton();
            dataHistoryPage.GridPanel.WaitForAdvancedSearchPanelDisplayed();

            Step("4. Fill the textbox Device with a part of testing Streetlight's name and select operator CONTAINS");
            dataHistoryPage.GridPanel.SelectSearchCriteriaForOperatorDropdownField("Device", "CONTAINS");
            dataHistoryPage.GridPanel.EnterSearchCriteriaFor1stValueInputField("Device", partOfStreelight);

            Step("5. Fill the textbox Unique address with the testing Streetlight's unique address and select the operator CONTAINS");
            dataHistoryPage.GridPanel.SelectSearchCriteriaForOperatorDropdownField("Unique address", "CONTAINS");
            dataHistoryPage.GridPanel.EnterSearchCriteriaFor1stValueInputField("Unique address", partOfUniqueAddress);

            Step("6. Press Search button");
            dataHistoryPage.GridPanel.ClickAdvancedSearchSearchButton();
            dataHistoryPage.GridPanel.WaitForAdvancedSearchPanelDisappeared();
            dataHistoryPage.GridPanel.WaitForLeftFooterTextDisplayed();

            Step("7. Verify The search result display ONLY the testing streetlight in the list");
            var devicesList = dataHistoryPage.GridPanel.GetListOfColumnData("Device");
            VerifyEqual("7. Verify The search result display ONLY the testing streetlight in the list", new List<string> { streetlight }, devicesList);

            Step("8. Select Search Fields icon again");
            dataHistoryPage.GridPanel.ClickSearchToolbarButton();
            dataHistoryPage.GridPanel.WaitForAdvancedSearchPanelDisplayed();

            Step("9. Verify The Search Fields screen displays with");
            Step(" o Text: Current search criteria: Device contains {a part of Streetlight's name} and Unique address contains 'a part of unique address value'");
            Step(" o Ex: Ex: Device contains SL02 and Unique address contains F4B");
            var expectedSearchCriteria = string.Format("Current search criteria:Device contains {0} and Unique address contains {1}", partOfStreelight, partOfUniqueAddress);
            var actualSearchCriteria = dataHistoryPage.GridPanel.GetSearchCriteria();
            VerifyEqual("9. Verify The Search Title displays as expected", expectedSearchCriteria, actualSearchCriteria);

            Step("10. Update randomly a field with the invalid value and press Search button");
            var randomData = dicAttributesData.PickRandom();
            var newValue = SLVHelper.GenerateString(10);
            dataHistoryPage.GridPanel.EnterSearchCriteriaFor1stValueInputField(randomData.Key, newValue);
            dataHistoryPage.GridPanel.ClickAdvancedSearchSearchButton();
            dataHistoryPage.GridPanel.WaitForAdvancedSearchPanelDisappeared();
            dataHistoryPage.GridPanel.WaitForLeftFooterTextDisplayed();

            Step("11. Verify The search result is empty");
            devicesList = dataHistoryPage.GridPanel.GetListOfColumnData("Device");
            VerifyEqual("11. Verify The search result is empty", 0, devicesList.Count);

            Step("12. Select Search Fields icon again");
            dataHistoryPage.GridPanel.ClickSearchToolbarButton();
            dataHistoryPage.GridPanel.WaitForAdvancedSearchPanelDisplayed();

            Step("13. Verify the text of the Search Fields screen is updated the text");
            if (randomData.Key.Equals("Device"))
                expectedSearchCriteria = string.Format("Current search criteria:Device contains {0} and Unique address contains {1}", newValue, partOfUniqueAddress);
            else
                expectedSearchCriteria = string.Format("Current search criteria:Device contains {0} and Unique address contains {1}", partOfStreelight, newValue);
            actualSearchCriteria = dataHistoryPage.GridPanel.GetSearchCriteria();
            VerifyEqual("13. Verify The Search Title displays as expected", expectedSearchCriteria, actualSearchCriteria);

            Step("14. Update to the exact value and press Search button");
            dataHistoryPage.GridPanel.EnterSearchCriteriaFor1stValueInputField(randomData.Key, randomData.Value);
            dataHistoryPage.GridPanel.ClickAdvancedSearchSearchButton();
            dataHistoryPage.GridPanel.WaitForAdvancedSearchPanelDisappeared();
            dataHistoryPage.GridPanel.WaitForLeftFooterTextDisplayed();

            Step("15. Verify The search result display ONLY the testing streetlight in the list");
            devicesList = dataHistoryPage.GridPanel.GetListOfColumnData("Device");
            VerifyEqual("15. Verify The search result display ONLY the testing streetlight in the list", new List<string> { streetlight }, devicesList);

            try
            {
                DeleteGeozone(geozone);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("DA_15- Search data history of metering attributes using operators- BEGINS")]
        public void DA_15()
        {
            var geozone = SLVHelper.GenerateUniqueName("GZNDA15");
            var controller = SLVHelper.GenerateUniqueName("CTRL");
            var streetlight = SLVHelper.GenerateUniqueName("STL");
            var meter = SLVHelper.GenerateUniqueName("MTR");
            var uniqueAddress = SLVHelper.GenerateMACAddress();

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - Create following devices: Streetlight, Meter");
            Step(" - Set values for all below attributes of the testing streetlight");
            Step(" • Device");
            Step(" • Unique address");
            Step("**** Precondition ****\n");

            Step("-> Create data for testing");
            DeleteGeozones("GZNDA15*");
            CreateNewGeozone(geozone);
            CreateNewController(controller, geozone);
            CreateNewDevice(DeviceType.Streetlight, streetlight, controller, geozone);
            CreateNewDevice(DeviceType.ElectricalCounter, meter, controller, geozone);
            SetValueToDevice(controller, streetlight, "MacAddress", uniqueAddress, Settings.GetServerTime());            

            var attributes = new List<string>() { "Device", "Unique address" };
            var dicAttributesData = new Dictionary<string, string>();
            dicAttributesData.Add("Device", streetlight);
            dicAttributesData.Add("Unique address", uniqueAddress);
            var firstLettersOfStreelight = streetlight.Substring(0, streetlight.Length / 2);
            var firstLettersOfUniqueAddress = uniqueAddress.Substring(0, uniqueAddress.Length / 2);

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.DataHistory);            

            Step("1. Go to Data History app");
            var dataHistoryPage = desktopPage.GoToApp(App.DataHistory) as DataHistoryPage;

            Step("2. Go to the geozone from the precondition");
            dataHistoryPage.GeozoneTreeMainPanel.SelectNode(geozone);
            dataHistoryPage.GridPanel.WaitForGridContentAvailable();

            Step("3. Select Search Fields icon on the main panel");
            dataHistoryPage.GridPanel.ClickSearchToolbarButton();
            dataHistoryPage.GridPanel.WaitForAdvancedSearchPanelDisplayed();

            Step("4. Fill the textbox Device with some first letters of testing Streetlight's name and select operator BEGINS");
            dataHistoryPage.GridPanel.SelectSearchCriteriaForOperatorDropdownField("Device", "BEGINS");
            dataHistoryPage.GridPanel.EnterSearchCriteriaFor1stValueInputField("Device", firstLettersOfStreelight);

            Step("5. Fill the textbox Unique address with some first letters of the testing Streetlight's unique address and select the operator BEGINS");
            dataHistoryPage.GridPanel.SelectSearchCriteriaForOperatorDropdownField("Unique address", "BEGINS");
            dataHistoryPage.GridPanel.EnterSearchCriteriaFor1stValueInputField("Unique address", firstLettersOfUniqueAddress);

            Step("6. Press Search button");
            dataHistoryPage.GridPanel.ClickAdvancedSearchSearchButton();
            dataHistoryPage.GridPanel.WaitForAdvancedSearchPanelDisappeared();
            dataHistoryPage.GridPanel.WaitForLeftFooterTextDisplayed();

            Step("7. Verify The search result display ONLY the testing streetlight in the list");
            var devicesList = dataHistoryPage.GridPanel.GetListOfColumnData("Device");
            VerifyEqual("7. Verify The search result display ONLY the testing streetlight in the list", new List<string> { streetlight }, devicesList);

            Step("8. Select Search Fields icon again");
            dataHistoryPage.GridPanel.ClickSearchToolbarButton();
            dataHistoryPage.GridPanel.WaitForAdvancedSearchPanelDisplayed();

            Step("9. Verify The Search Fields screen displays with");
            Step(" o Text: Current search criteria: Device begins with {some first letters of Streetlight's name} and Unique address begins with 'some first letters of unique address value'");
            Step(" o Ex: Device begins with Data and Unique address begins with AF");
            var expectedSearchCriteria = string.Format("Current search criteria:Device begins with {0} and Unique address begins with {1}", firstLettersOfStreelight, firstLettersOfUniqueAddress);
            var actualSearchCriteria = dataHistoryPage.GridPanel.GetSearchCriteria();
            VerifyEqual("9. Verify The Search Title displays as expected", expectedSearchCriteria, actualSearchCriteria);

            Step("10. Update randomly a field with the invalid value and press Search button");
            var randomData = dicAttributesData.PickRandom();
            var newValue = SLVHelper.GenerateString(10);
            dataHistoryPage.GridPanel.EnterSearchCriteriaFor1stValueInputField(randomData.Key, newValue);
            dataHistoryPage.GridPanel.ClickAdvancedSearchSearchButton();
            dataHistoryPage.GridPanel.WaitForAdvancedSearchPanelDisappeared();
            dataHistoryPage.GridPanel.WaitForLeftFooterTextDisplayed();

            Step("11. Verify The search result is empty");
            devicesList = dataHistoryPage.GridPanel.GetListOfColumnData("Device");
            VerifyEqual("11. Verify The search result is empty", 0, devicesList.Count);

            Step("12. Select Search Fields icon again");
            dataHistoryPage.GridPanel.ClickSearchToolbarButton();
            dataHistoryPage.GridPanel.WaitForAdvancedSearchPanelDisplayed();

            Step("13. Verify the text of the Search Fields screen is updated the text");
            if (randomData.Key.Equals("Device"))
                expectedSearchCriteria = string.Format("Current search criteria:Device begins with {0} and Unique address begins with {1}", newValue, firstLettersOfUniqueAddress);
            else
                expectedSearchCriteria = string.Format("Current search criteria:Device begins with {0} and Unique address begins with {1}", firstLettersOfStreelight, newValue);
            actualSearchCriteria = dataHistoryPage.GridPanel.GetSearchCriteria();
            VerifyEqual("13. Verify The Search Title displays as expected", expectedSearchCriteria, actualSearchCriteria);

            Step("14. Update to the exact value and press Search button");
            dataHistoryPage.GridPanel.EnterSearchCriteriaFor1stValueInputField(randomData.Key, randomData.Value);
            dataHistoryPage.GridPanel.ClickAdvancedSearchSearchButton();
            dataHistoryPage.GridPanel.WaitForAdvancedSearchPanelDisappeared();
            dataHistoryPage.GridPanel.WaitForLeftFooterTextDisplayed();

            Step("15. Verify The search result display ONLY the testing streetlight in the list");
            devicesList = dataHistoryPage.GridPanel.GetListOfColumnData("Device");
            VerifyEqual("15. Verify The search result display ONLY the testing streetlight in the list", new List<string> { streetlight }, devicesList);

            try
            {
                DeleteGeozone(geozone);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("DA_16- Search data history of metering attributes using operators- ENDS")]
        public void DA_16()
        {
            var geozone = SLVHelper.GenerateUniqueName("GZNDA16");
            var controller = SLVHelper.GenerateUniqueName("CTRL");
            var streetlight = SLVHelper.GenerateUniqueName("STL");
            var meter = SLVHelper.GenerateUniqueName("MTR");
            var uniqueAddress = SLVHelper.GenerateMACAddress();

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - Create following devices: Streetlight, Meter");
            Step(" - Set values for all below attributes of the testing streetlight");
            Step(" • Device");
            Step(" • Unique address");
            Step("**** Precondition ****\n");

            Step("-> Create data for testing");
            DeleteGeozones("GZNDA16*");
            CreateNewGeozone(geozone);
            CreateNewController(controller, geozone);
            CreateNewDevice(DeviceType.Streetlight, streetlight, controller, geozone);
            CreateNewDevice(DeviceType.ElectricalCounter, meter, controller, geozone);
            SetValueToDevice(controller, streetlight, "MacAddress", uniqueAddress, Settings.GetServerTime());            

            var attributes = new List<string>() { "Device", "Unique address" };
            var dicAttributesData = new Dictionary<string, string>();
            dicAttributesData.Add("Device", streetlight);
            dicAttributesData.Add("Unique address", uniqueAddress);
            var lastLettersOfStreelight = streetlight.Substring(streetlight.Length / 2);
            var lastLettersOfUniqueAddress = uniqueAddress.Substring(uniqueAddress.Length / 2);

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.DataHistory);      

            Step("1. Go to Data History app");
            var dataHistoryPage = desktopPage.GoToApp(App.DataHistory) as DataHistoryPage;

            Step("2. Go to the geozone from the precondition");
            dataHistoryPage.GeozoneTreeMainPanel.SelectNode(geozone);
            dataHistoryPage.GridPanel.WaitForGridContentAvailable();

            Step("3. Select Search Fields icon on the main panel");
            dataHistoryPage.GridPanel.ClickSearchToolbarButton();
            dataHistoryPage.GridPanel.WaitForAdvancedSearchPanelDisplayed();

            Step("4. Fill the textbox Device with some last letters of testing Streetlight's name and select operator ENDS");
            dataHistoryPage.GridPanel.SelectSearchCriteriaForOperatorDropdownField("Device", "ENDS");
            dataHistoryPage.GridPanel.EnterSearchCriteriaFor1stValueInputField("Device", lastLettersOfStreelight);

            Step("5. Fill the textbox Unique address with some last letters of the testing Streetlight's unique address and select the operator ENDS");
            dataHistoryPage.GridPanel.SelectSearchCriteriaForOperatorDropdownField("Unique address", "ENDS");
            dataHistoryPage.GridPanel.EnterSearchCriteriaFor1stValueInputField("Unique address", lastLettersOfUniqueAddress);

            Step("6. Press Search button");
            dataHistoryPage.GridPanel.ClickAdvancedSearchSearchButton();
            dataHistoryPage.GridPanel.WaitForAdvancedSearchPanelDisappeared();
            dataHistoryPage.GridPanel.WaitForLeftFooterTextDisplayed();

            Step("7. Verify The search result display ONLY the testing streetlight in the list");
            var devicesList = dataHistoryPage.GridPanel.GetListOfColumnData("Device");
            VerifyEqual("7. Verify The search result display ONLY the testing streetlight in the list", new List<string> { streetlight }, devicesList);

            Step("8. Select Search Fields icon again");
            dataHistoryPage.GridPanel.ClickSearchToolbarButton();
            dataHistoryPage.GridPanel.WaitForAdvancedSearchPanelDisplayed();

            Step("9. Verify The Search Fields screen displays with");
            Step(" o Text: Current search criteria: Device ends with {some last letters of Streetlight's name} and Unique address begins with 'some last letters of unique address value'");
            Step(" o Ex: Device ends with 02 and Unique address ends with 4B");
            var expectedSearchCriteria = string.Format("Current search criteria:Device ends with {0} and Unique address ends with {1}", lastLettersOfStreelight, lastLettersOfUniqueAddress);
            var actualSearchCriteria = dataHistoryPage.GridPanel.GetSearchCriteria();
            VerifyEqual("9. Verify The Search Title displays as expected", expectedSearchCriteria, actualSearchCriteria);

            Step("10. Update randomly a field with the invalid value and press Search button");
            var randomData = dicAttributesData.PickRandom();
            var newValue = SLVHelper.GenerateString(10);
            dataHistoryPage.GridPanel.EnterSearchCriteriaFor1stValueInputField(randomData.Key, newValue);
            dataHistoryPage.GridPanel.ClickAdvancedSearchSearchButton();
            dataHistoryPage.GridPanel.WaitForAdvancedSearchPanelDisappeared();
            dataHistoryPage.GridPanel.WaitForLeftFooterTextDisplayed();

            Step("11. Verify The search result is empty");
            devicesList = dataHistoryPage.GridPanel.GetListOfColumnData("Device");
            VerifyEqual("11. Verify The search result is empty", 0, devicesList.Count);

            Step("12. Select Search Fields icon again");
            dataHistoryPage.GridPanel.ClickSearchToolbarButton();
            dataHistoryPage.GridPanel.WaitForAdvancedSearchPanelDisplayed();

            Step("13. Verify the text of the Search Fields screen is updated the text");
            if (randomData.Key.Equals("Device"))
                expectedSearchCriteria = string.Format("Current search criteria:Device ends with {0} and Unique address ends with {1}", newValue, lastLettersOfUniqueAddress);
            else
                expectedSearchCriteria = string.Format("Current search criteria:Device ends with {0} and Unique address ends with {1}", lastLettersOfStreelight, newValue);
            actualSearchCriteria = dataHistoryPage.GridPanel.GetSearchCriteria();
            VerifyEqual("13. Verify The Search Title displays as expected", expectedSearchCriteria, actualSearchCriteria);

            Step("14. Update to the exact value and press Search button");
            dataHistoryPage.GridPanel.EnterSearchCriteriaFor1stValueInputField(randomData.Key, randomData.Value);
            dataHistoryPage.GridPanel.ClickAdvancedSearchSearchButton();
            dataHistoryPage.GridPanel.WaitForAdvancedSearchPanelDisappeared();
            dataHistoryPage.GridPanel.WaitForLeftFooterTextDisplayed();

            Step("15. Verify The search result display ONLY the testing streetlight in the list");
            devicesList = dataHistoryPage.GridPanel.GetListOfColumnData("Device");
            VerifyEqual("15. Verify The search result display ONLY the testing streetlight in the list", new List<string> { streetlight }, devicesList);

            try
            {
                DeleteGeozone(geozone);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("SC-1214- All metering and failures provided by SSN devices are available in the Search screen of Data History and Advanced Search")]
        public void DA_17()
        {
            var testData = GetTestDataOfDA_17();            
            var equipmentTypes = testData["EquipmentTypes"] as Dictionary<string, string>;
            var equipmentType = equipmentTypes.PickRandom();
            var geozone = SLVHelper.GenerateUniqueName("GZNDA17");
            var controller = SLVHelper.GenerateUniqueName("CTRL");
            var streetlight = SLVHelper.GenerateUniqueName("STL");

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - Create a testing streetlight which belongs to one of the types of SSN Devices below.");
            Step(" - There are 10 types of SSN device");
            Step("  • SSN Cimcon Dim Photocell[Lamp #0]");
            Step("  • SSN Enlight SCD220 Photocell[Lamp /#0]");
            Step("  • SSN Enlight SCD230 Node[Lamp #0]");
            Step("  • SSN LRL Integrated LED[Lamp /#0]");
            Step("  • SSN Pulsadis On/Off[Lamp #0]");
            Step("  • SSN Selc Control Node[Lamp #0]");
            Step("  • SSN Selc Dim Photocell[Lamp #0]");
            Step("  • SSN Selc Photocell On/Off[onofflamp0]");
            Step("  • SSN Sun-Tech Dim Photocell[Lamp #0]");
            Step("  • SSN Sun-Tech Photocell On/Off[onofflamp0]");
            Step("**** Precondition ****\n");

            Step("-> Create data for testing");
            DeleteGeozones("GZNDA17*");
            CreateNewGeozone(geozone);
            CreateNewController(controller, geozone);
            CreateNewDevice(DeviceType.Streetlight, streetlight, controller, geozone);

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.DataHistory, App.AdvancedSearch);

            Step("1. Go to Data History app");
            var dataHistoryPage = desktopPage.GoToApp(App.DataHistory) as DataHistoryPage;

            Step("2. Select the testing streetlight");
            dataHistoryPage.GeozoneTreeMainPanel.SelectNode(geozone + @"\" + streetlight);

            Step("3. Select Meterings tab and take note all the metering fields");
            dataHistoryPage.LastValuesPanel.SelectTab("Meterings");
            var listMeterings = dataHistoryPage.LastValuesPanel.GetMeteringsNameList();

            Step("4. Select Failures tab and take note all the failure fields");
            dataHistoryPage.LastValuesPanel.SelectTab("Failures");
            var listFailures = dataHistoryPage.LastValuesPanel.GetFailuresNameList();

            Step("5. Press Search Fields icon on the main panel");
            dataHistoryPage.GridPanel.ClickSearchToolbarButton();
            dataHistoryPage.GridPanel.WaitForAdvancedSearchPanelDisplayed();            
            var listSearchFields = dataHistoryPage.GridPanel.GetListOfAdvancedSearchFields();

            Step("6. Verify all metering fields and failure fields of the testing streetlight are displayed in the Search screen");
            VerifyTrue("6. Verify all metering fields are displayed in the Search screen", listSearchFields.CheckIfIncluded(listMeterings), string.Join(", ", listSearchFields), string.Join(", ", listMeterings));
            VerifyTrue("6. Verify all failure fields are displayed in the Search screen", listSearchFields.CheckIfIncluded(listFailures), string.Join(", ", listSearchFields), string.Join(", ", listFailures));

            Step("7. Switch to Advanced Search app");
            var advancedSearchPage = dataHistoryPage.AppBar.SwitchTo(App.AdvancedSearch) as AdvancedSearchPage;
            advancedSearchPage.WaitForSearchWizardPopupPanelDisplayed();

            Step("8. Close the 'My advanced searches' pop-up if it's opened");            
            advancedSearchPage.SearchWizardPopupPanel.ClickCloseButton();
            advancedSearchPage.WaitForSearchWizardPopupPanelDisappeared();

            Step("9. Press Search Fields icon on the main panel");
            advancedSearchPage.GridPanel.ClickSearchToolbarButton();
            advancedSearchPage.GridPanel.WaitForAdvancedSearchPanelDisplayed();
            listSearchFields = advancedSearchPage.GridPanel.GetListOfAdvancedSearchFields();

            Step("10. Verify all metering fields and failure fields of the testing streetlight are displayed in the Search screen");
            VerifyTrue("10. Verify all metering fields are displayed in the Search screen", listSearchFields.CheckIfIncluded(listMeterings), string.Join(", ", listSearchFields), string.Join(", ", listMeterings));
            VerifyTrue("10. Verify all failure fields are displayed in the Search screen", listSearchFields.CheckIfIncluded(listFailures), string.Join(", ", listSearchFields), string.Join(", ", listFailures));

            try
            {
                DeleteGeozone(geozone);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("DA_1061884_01 - Power Supply Failure is cleared if measured power is greater than 4W at least twice in the past 24 hours")]
        public void DA_1061884_01()
        {
            var geozone = SLVHelper.GenerateUniqueName("GZNDA106188401");
            var controller = SLVHelper.GenerateUniqueName("CTRL");
            var streetlight = SLVHelper.GenerateUniqueName("STL");

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - Create a testing geozone containing a streetlight(TALQ Streetlight[lightNodeFunction6]) connected to a controller using UTC time");
            Step("**** Precondition ****\n");

            Step("-> Create data for testing");
            DeleteGeozones("GZNDA106188401*");
            CreateNewGeozone(geozone);
            CreateNewController(controller, geozone);
            SetValueToController(controller, "TimeZoneId", "UTC", Settings.GetServerTime());
            CreateNewDevice(DeviceType.Streetlight, streetlight, controller, geozone, "TALQ Streetlight[lightNodeFunction6]");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.DataHistory, App.DeviceHistory);
            
            Step("1. Simulate the Power Supply Failure for the streetlight");
            Step(" o valueName=PowerSupplyFailure");
            Step(" o value=true");
            Step(" o eventTime= the current UTC datetime");
            var failure = "PowerSupplyFailure";
            var eventTime = Settings.GetServerTime();
            SetValueToDevice(controller, streetlight, failure, true, eventTime);

            Step("2. Go to Data History app and select the testing streetlight, then select the Failure tab");
            var dataHistoryPage = desktopPage.GoToApp(App.DataHistory) as DataHistoryPage;

            Step("3. Verify Power Supply Failure is recorded with the red error icon");
            dataHistoryPage.GeozoneTreeMainPanel.SelectNode(geozone + @"\" + streetlight);
            dataHistoryPage.WaitForLastValuePanelDisplayed();
            dataHistoryPage.LastValuesPanel.SelectTab("Failures");
            var actualIcon = dataHistoryPage.LastValuesPanel.GetFailureIcon(failure);
            VerifyEqual("3. Verify Power Supply Failure is recorded with the red error icon", FailureIcon.Error, actualIcon);

            Step("4. Simulate the data for measured power by sending the 2 commands, each command is about 30s after the other");
            Step(" o valueName=MeteredPower");
            Step(" o value= randomly but greater than 4");
            Step(" o eventTime= the current UTC datetime");
            var meterKey = "MeteredPower";
            var meterName = "Metered power (W)";
            var value = SLVHelper.GenerateStringInteger(5, 999);
            eventTime = Settings.GetServerTime();
            SetValueToDevice(controller, streetlight, meterKey, value, eventTime);
            value = SLVHelper.GenerateStringInteger(5, 999);
            eventTime = Settings.GetServerTime().AddSeconds(30);
            SetValueToDevice(controller, streetlight, meterKey, value, eventTime);

            Step("5. Choose another device and select the testing streetlight again, then select the Meterings tab and Metered Powers meter");
            dataHistoryPage.LastValuesPanel.ClickBackToolbarButton();
            dataHistoryPage.WaitForLastValuePanelDisappeared();
            dataHistoryPage.GeozoneTreeMainPanel.SelectNode(controller);
            dataHistoryPage.WaitForLastValuePanelDisplayed();
            dataHistoryPage.LastValuesPanel.ClickBackToolbarButton();
            dataHistoryPage.WaitForLastValuePanelDisappeared();
            Wait.ForMinutes(1); // Wait for failure changes status
            dataHistoryPage.GeozoneTreeMainPanel.SelectNode(streetlight);
            dataHistoryPage.WaitForLastValuePanelDisplayed();
            dataHistoryPage.LastValuesPanel.SelectTab("Meterings");
            dataHistoryPage.LastValuesPanel.SelectMeteringsAttribute(meterName);
            dataHistoryPage.WaitForPreviousActionComplete();

            Step("6. Verify Metered Powers are recorded with 2 points on the graph");
            VerifyEqual("6. Verify Metered Powers are recorded with 2 points on the graph", 2, dataHistoryPage.GraphPanel.GetChartGraphPointsCount(streetlight));
           
            Step("7. Select Failure tab");
            dataHistoryPage.LastValuesPanel.SelectTab("Failures");

            Step("8. Verify Power Supply Failure is cleared with the green checked icon.");
            actualIcon = dataHistoryPage.LastValuesPanel.GetFailureIcon(failure);
            VerifyEqual("8. Verify Power Supply Failure is cleared with the green checked icon.", FailureIcon.OK, actualIcon);            

            try
            {
                DeleteGeozone(geozone);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("DA_1061884_02 - Open Circuit failure is cleared if measured power is greater than 4W at least twice in the past 24 hours")]
        public void DA_1061884_02()
        {
            var geozone = SLVHelper.GenerateUniqueName("GZNDA106188402");
            var controller = SLVHelper.GenerateUniqueName("CTRL");
            var streetlight = SLVHelper.GenerateUniqueName("STL");

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - Create a testing geozone containing a streetlight(TALQ Streetlight[lightNodeFunction6]) connected to a controller using UTC time");
            Step("**** Precondition ****\n");

            Step("-> Create data for testing");
            DeleteGeozones("GZNDA106188402*");
            CreateNewGeozone(geozone);
            CreateNewController(controller, geozone);
            SetValueToController(controller, "TimeZoneId", "UTC", Settings.GetServerTime());
            CreateNewDevice(DeviceType.Streetlight, streetlight, controller, geozone, "TALQ Streetlight[lightNodeFunction6]");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.DataHistory, App.DeviceHistory);

            Step("1. Simulate the Open Circuit failure for the streetlight");
            Step(" o valueName=OpenCircuit");
            Step(" o value=true");
            Step(" o eventTime= the current UTC datetime");
            var failure = "OpenCircuit";
            var eventTime = Settings.GetServerTime();
            SetValueToDevice(controller, streetlight, failure, true, eventTime);

            Step("2. Go to Data History app and select the testing streetlight, then select the Failure tab");
            var dataHistoryPage = desktopPage.GoToApp(App.DataHistory) as DataHistoryPage;

            Step("3. Verify Open Circuit failure is recorded with the red error icon");
            dataHistoryPage.GeozoneTreeMainPanel.SelectNode(geozone + @"\" + streetlight);
            dataHistoryPage.WaitForLastValuePanelDisplayed();
            dataHistoryPage.LastValuesPanel.SelectTab("Failures");
            var actualIcon = dataHistoryPage.LastValuesPanel.GetFailureIcon(failure);
            VerifyEqual("3. Verify Open Circuit failure is recorded with the red error icon", FailureIcon.Error, actualIcon);

            Step("4. Simulate the data for measured power by sending the 2 commands, each command is about 30s after the other");
            Step(" o valueName=MeteredPower");
            Step(" o value= randomly but greater than 4");
            Step(" o eventTime= the current UTC datetime");
            var meterKey = "MeteredPower";
            var meterName = "Metered power (W)";
            var value = SLVHelper.GenerateStringInteger(5, 999);
            eventTime = Settings.GetServerTime();
            SetValueToDevice(controller, streetlight, meterKey, value, eventTime);
            value = SLVHelper.GenerateStringInteger(5, 999);
            eventTime = Settings.GetServerTime().AddSeconds(30);
            SetValueToDevice(controller, streetlight, meterKey, value, eventTime);

            Step("5. Choose another device and select the testing streetlight again, then select the Meterings tab and Metered Powers meter");
            dataHistoryPage.LastValuesPanel.ClickBackToolbarButton();
            dataHistoryPage.WaitForLastValuePanelDisappeared();
            dataHistoryPage.GeozoneTreeMainPanel.SelectNode(controller);
            dataHistoryPage.WaitForLastValuePanelDisplayed();
            dataHistoryPage.LastValuesPanel.ClickBackToolbarButton();
            dataHistoryPage.WaitForLastValuePanelDisappeared();
            Wait.ForMinutes(1); // Wait for failure changes status
            dataHistoryPage.GeozoneTreeMainPanel.SelectNode(streetlight);
            dataHistoryPage.WaitForLastValuePanelDisplayed();
            dataHistoryPage.LastValuesPanel.SelectTab("Meterings");
            dataHistoryPage.LastValuesPanel.SelectMeteringsAttribute(meterName);
            dataHistoryPage.WaitForPreviousActionComplete();

            Step("6. Verify Metered Powers are recorded with 2 points on the graph");
            VerifyEqual("6. Verify Metered Powers are recorded with 2 points on the graph", 2, dataHistoryPage.GraphPanel.GetChartGraphPointsCount(streetlight));

            Step("7. Select Failure tab");
            dataHistoryPage.LastValuesPanel.SelectTab("Failures");

            Step("8. Verify Open Circuit failure is cleared with the green checked icon.");
            actualIcon = dataHistoryPage.LastValuesPanel.GetFailureIcon(failure);
            VerifyEqual("8. Verify Open Circuit failure is cleared with the green checked icon.", FailureIcon.OK, actualIcon);            

            try
            {
                DeleteGeozone(geozone);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("DA_1061884_03 - Lamp failure is cleared if measured power is greater than 4W at least twice in the past 24 hours")]
        public void DA_1061884_03()
        {
            var geozone = SLVHelper.GenerateUniqueName("GZNDA106188403");
            var controller = SLVHelper.GenerateUniqueName("CTRL");
            var streetlight = SLVHelper.GenerateUniqueName("STL");

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - Create a testing geozone containing a streetlight(TALQ Streetlight[lightNodeFunction6]) connected to a controller using UTC time");
            Step("**** Precondition ****\n");

            Step("-> Create data for testing");
            DeleteGeozones("GZNDA106188403*");
            CreateNewGeozone(geozone);
            CreateNewController(controller, geozone);
            SetValueToController(controller, "TimeZoneId", "UTC", Settings.GetServerTime());
            CreateNewDevice(DeviceType.Streetlight, streetlight, controller, geozone, "TALQ Streetlight[lightNodeFunction6]");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.DataHistory, App.DeviceHistory);

            Step("1. Simulate the Lamp Failure for the streetlight");
            Step(" o valueName=LampFailure");
            Step(" o value=true");
            Step(" o eventTime= the current UTC datetime");
            var failure = "LampFailure";
            var eventTime = Settings.GetServerTime();
            SetValueToDevice(controller, streetlight, failure, true, eventTime);

            Step("2. Go to Data History app and select the testing streetlight, then select the Failure tab");
            var dataHistoryPage = desktopPage.GoToApp(App.DataHistory) as DataHistoryPage;

            Step("3. Verify Lamp Failure is recorded with the red error icon");
            dataHistoryPage.GeozoneTreeMainPanel.SelectNode(geozone + @"\" + streetlight);
            dataHistoryPage.WaitForLastValuePanelDisplayed();
            dataHistoryPage.LastValuesPanel.SelectTab("Failures");
            var actualIcon = dataHistoryPage.LastValuesPanel.GetFailureIcon(failure);
            VerifyEqual("3. Verify Lamp failure is recorded with the red error icon", FailureIcon.Error, actualIcon);

            Step("4. Simulate the data for measured power by sending the 2 commands, each command is about 30s after the other");
            Step(" o valueName=MeteredPower");
            Step(" o value= randomly but greater than 4");
            Step(" o eventTime= the current UTC datetime");
            var meterKey = "MeteredPower";
            var meterName = "Metered power (W)";
            var value = SLVHelper.GenerateStringInteger(5, 999);
            eventTime = Settings.GetServerTime();
            SetValueToDevice(controller, streetlight, meterKey, value, eventTime);
            value = SLVHelper.GenerateStringInteger(5, 999);
            eventTime = Settings.GetServerTime().AddSeconds(30);
            SetValueToDevice(controller, streetlight, meterKey, value, eventTime);

            Step("5. Choose another device and select the testing streetlight again, then select the Meterings tab and Metered Powers meter");
            dataHistoryPage.LastValuesPanel.ClickBackToolbarButton();
            dataHistoryPage.WaitForLastValuePanelDisappeared();
            dataHistoryPage.GeozoneTreeMainPanel.SelectNode(controller);
            dataHistoryPage.WaitForLastValuePanelDisplayed();
            dataHistoryPage.LastValuesPanel.ClickBackToolbarButton();
            dataHistoryPage.WaitForLastValuePanelDisappeared();
            Wait.ForMinutes(1); // Wait for failure changes status
            dataHistoryPage.GeozoneTreeMainPanel.SelectNode(streetlight);
            dataHistoryPage.WaitForLastValuePanelDisplayed();
            dataHistoryPage.LastValuesPanel.SelectTab("Meterings");
            dataHistoryPage.LastValuesPanel.SelectMeteringsAttribute(meterName);
            dataHistoryPage.WaitForPreviousActionComplete();

            Step("6. Verify Metered Powers are recorded with 2 points on the graph");
            VerifyEqual("6. Verify Metered Powers are recorded with 2 points on the graph", 2, dataHistoryPage.GraphPanel.GetChartGraphPointsCount(streetlight));

            Step("7. Select Failure tab");
            dataHistoryPage.LastValuesPanel.SelectTab("Failures");

            Step("8. Verify Lamp Failure is cleared with the green checked icon.");
            actualIcon = dataHistoryPage.LastValuesPanel.GetFailureIcon(failure);
            VerifyEqual("8. Verify Lamp failure is cleared with the green checked icon.", FailureIcon.OK, actualIcon);            

            try
            {
                DeleteGeozone(geozone);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("DA_1061884_04 - Communication Failure is cleared if SLV receives some data from it within the past 6 hours")]
        public void DA_1061884_04()
        {
            var geozone = SLVHelper.GenerateUniqueName("GZNDA106188404");
            var controller = SLVHelper.GenerateUniqueName("CTRL");
            var streetlight = SLVHelper.GenerateUniqueName("STL");

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - Create a testing geozone containing a streetlight(TALQ Streetlight[lightNodeFunction6]) connected to a controller using UTC time");
            Step("**** Precondition ****\n");

            Step("-> Create data for testing");
            DeleteGeozones("GZNDA106188404*");
            CreateNewGeozone(geozone);
            CreateNewController(controller, geozone);
            SetValueToController(controller, "TimeZoneId", "UTC", Settings.GetServerTime());
            CreateNewDevice(DeviceType.Streetlight, streetlight, controller, geozone, "TALQ Streetlight[lightNodeFunction6]");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.DataHistory, App.DeviceHistory);

            Step("1. Simulate the Communication Failure for the streetlight");
            Step(" o valueName=CommunicationFailure");
            Step(" o value=true");
            Step(" o eventTime= the current UTC datetime");
            var failure = "CommunicationFailure";
            var eventTime = Settings.GetServerTime();
            SetValueToDevice(controller, streetlight, failure, true, eventTime);

            Step("2. Go to Data History app and select the testing streetlight, then select the Failure tab");
            var dataHistoryPage = desktopPage.GoToApp(App.DataHistory) as DataHistoryPage;

            Step("3. Verify Communication Failure is recorded with the red error icon");
            dataHistoryPage.GeozoneTreeMainPanel.SelectNode(geozone + @"\" + streetlight);
            dataHistoryPage.WaitForLastValuePanelDisplayed();
            dataHistoryPage.LastValuesPanel.SelectTab("Failures");
            var actualIcon = dataHistoryPage.LastValuesPanel.GetFailureIcon(failure);
            VerifyEqual("3. Verify Communication Failure is recorded with the red error icon", FailureIcon.Error, actualIcon);

            Step("4. Simulate data for the streetlight by sending a command");
            Step(" o valueName=LampCommandLevel");
            Step(" o value= 100");
            Step(" o eventTime= the current UTC datetime");
            var meterKey = "LampCommandLevel";
            var meterName = "Lamp Level Command";
            var value = "100.00";
            eventTime = Settings.GetServerTime();
            SetValueToDevice(controller, streetlight, meterKey, value, eventTime);

            Step("5. Choose another device and select the testing streetlight again, then select the Meterings tab");
            dataHistoryPage.LastValuesPanel.ClickBackToolbarButton();
            dataHistoryPage.WaitForLastValuePanelDisappeared();
            dataHistoryPage.GeozoneTreeMainPanel.SelectNode(controller);
            dataHistoryPage.WaitForLastValuePanelDisplayed();
            dataHistoryPage.LastValuesPanel.ClickBackToolbarButton();
            dataHistoryPage.WaitForLastValuePanelDisappeared();
            Wait.ForMinutes(1); // Wait for failure changes status
            dataHistoryPage.GeozoneTreeMainPanel.SelectNode(streetlight);
            dataHistoryPage.WaitForLastValuePanelDisplayed();
            dataHistoryPage.LastValuesPanel.SelectTab("Meterings");

            Step("6. Verify Lamp Level Command is recorded in Data History");
            var expectedValue = value;
            var actualValue = dataHistoryPage.LastValuesPanel.GetMeteringValue(meterKey);
            VerifyEqual("6. Verify Lamp Level Command is recorded in Data History", expectedValue, actualValue);

            Step("7. Select Failure tab");
            dataHistoryPage.LastValuesPanel.SelectTab("Failures");

            Step("8. Verify Communication Failure is cleared with the green checked icon.");
            actualIcon = dataHistoryPage.LastValuesPanel.GetFailureIcon(failure);
            VerifyEqual("8. Verify Communication Failure is cleared with the green checked icon.", FailureIcon.OK, actualIcon);            

            Step("9. Repeat the test with simulating data for the streetlight by sending a command");
            Step(" o valueName=LampLevel");
            Step(" o value= 100");
            Step(" o eventTime= the current UTC datetime");

            Step("9.1. Simulate the Communication Failure for the streetlight");
            eventTime = Settings.GetServerTime();
            SetValueToDevice(controller, streetlight, failure, true, eventTime);           

            Step("9.2. Go to Data History app and select the testing streetlight, then select the Failure tab");
            desktopPage = Browser.RefreshLoggedInCMS();
            dataHistoryPage = desktopPage.GoToApp(App.DataHistory) as DataHistoryPage;

            Step("9.3. Verify Communication Failure is recorded with the red error icon");
            dataHistoryPage.GeozoneTreeMainPanel.SelectNode(geozone + @"\" + streetlight);
            dataHistoryPage.WaitForLastValuePanelDisplayed();
            dataHistoryPage.LastValuesPanel.SelectTab("Failures");
            actualIcon = dataHistoryPage.LastValuesPanel.GetFailureIcon(failure);
            VerifyEqual("9.3. Verify Communication Failure is recorded with the red error icon", FailureIcon.Error, actualIcon);

            Step("9.4. Simulate data for the streetlight by sending a command");            
            meterKey = "LampLevel";
            meterName = "Lamp Level";
            value = "100.00";
            eventTime = Settings.GetServerTime();
            SetValueToDevice(controller, streetlight, meterKey, value, eventTime);

            Step("9.5. Choose another device and select the testing streetlight again, then select the Meterings tab");
            dataHistoryPage.LastValuesPanel.ClickBackToolbarButton();
            dataHistoryPage.WaitForLastValuePanelDisappeared();
            dataHistoryPage.GeozoneTreeMainPanel.SelectNode(controller);
            dataHistoryPage.WaitForLastValuePanelDisplayed();
            dataHistoryPage.LastValuesPanel.ClickBackToolbarButton();
            dataHistoryPage.WaitForLastValuePanelDisappeared();
            Wait.ForMinutes(1); // Wait for failure changes status
            dataHistoryPage.GeozoneTreeMainPanel.SelectNode(streetlight);
            dataHistoryPage.WaitForLastValuePanelDisplayed();
            dataHistoryPage.LastValuesPanel.SelectTab("Meterings");

            Step("9.6. Verify Lamp Level Command is recorded in Data History");
            expectedValue = value;
            actualValue = dataHistoryPage.LastValuesPanel.GetMeteringValue(meterKey);
            VerifyEqual("9.6. Verify Lamp Level Command is recorded in Data History", expectedValue, actualValue);

            Step("9.7. Select Failure tab");
            dataHistoryPage.LastValuesPanel.SelectTab("Failures");

            Step("9.8. Verify Communication Failure is cleared with the green checked icon.");
            actualIcon = dataHistoryPage.LastValuesPanel.GetFailureIcon(failure);
            VerifyEqual("9.8. Verify Communication Failure is cleared with the green checked icon.", FailureIcon.OK, actualIcon);            

            try
            {
                DeleteGeozone(geozone);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("DA_1061884_08 - Communication Failure of Cabinet Controller is cleared if SLV receives some data from it within the past 6 hours")]
        public void DA_1061884_08()
        {
            var geozone = SLVHelper.GenerateUniqueName("GZNDA106188408");
            var controller = SLVHelper.GenerateUniqueName("CTRL");
            var cabinetController = SLVHelper.GenerateUniqueName("CBN");

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - Create a testing geozone containing a cabinet controller connected to a controller using UTC time");
            Step("**** Precondition ****\n");

            Step("-> Create data for testing");
            DeleteGeozones("GZNDA106188408*");
            CreateNewGeozone(geozone);
            CreateNewController(controller, geozone);
            SetValueToController(controller, "TimeZoneId", "UTC", Settings.GetServerTime());
            CreateNewDevice(DeviceType.CabinetController, cabinetController, controller, geozone);

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.DataHistory, App.DeviceHistory);

            Step("1. Simulate the Communication Failure for the cabinet controller");
            Step(" o valueName=CommunicationFailure");
            Step(" o value=true");
            Step(" o eventTime= the current UTC datetime - a few minutes");
            var failure = "CommunicationFailure";
            var eventTime = Settings.GetServerTime().AddMinutes(-5);
            SetValueToDevice(controller, cabinetController, failure, true, eventTime);

            Step("2.  Go to Data History app and select the testing cabinet controller, then select the Failure tab");
            var dataHistoryPage = desktopPage.GoToApp(App.DataHistory) as DataHistoryPage;

            Step("3. Verify Communication Failure is recorded with the red error icon");
            dataHistoryPage.GeozoneTreeMainPanel.SelectNode(geozone + @"\" + cabinetController);
            dataHistoryPage.WaitForLastValuePanelDisplayed();
            dataHistoryPage.LastValuesPanel.SelectTab("Failures");
            var actualIcon = dataHistoryPage.LastValuesPanel.GetFailureIcon(failure);
            VerifyEqual("3. Verify Communication Failure is recorded with the red error icon", FailureIcon.Error, actualIcon);

            Step("4. Simulate data for the cabinet controller by sending a command");
            Step(" o valueName=Relay1Level");
            Step(" o value= 100");
            Step(" o eventTime= the current UTC datetime");
            var meterKey = "Relay1Level";
            var meterName = "Relay1Level";
            var value = "100.00";
            eventTime = Settings.GetServerTime();
            SetValueToDevice(controller, cabinetController, meterKey, value, eventTime);

            Step("5. Choose another device and select the testing cabinet controller again, then select the Meterings tab");
            dataHistoryPage.LastValuesPanel.ClickBackToolbarButton();
            dataHistoryPage.WaitForLastValuePanelDisappeared();
            dataHistoryPage.GeozoneTreeMainPanel.SelectNode(controller);
            dataHistoryPage.WaitForLastValuePanelDisplayed();
            dataHistoryPage.LastValuesPanel.ClickBackToolbarButton();
            dataHistoryPage.WaitForLastValuePanelDisappeared();
            Wait.ForMinutes(1); // Wait for failure changes status
            dataHistoryPage.GeozoneTreeMainPanel.SelectNode(cabinetController);
            dataHistoryPage.WaitForLastValuePanelDisplayed();
            dataHistoryPage.LastValuesPanel.SelectTab("Meterings");

            Step("6. Verify Relay1Level is recorded in Data History");
            var expectedValue = value;
            var actualValue = dataHistoryPage.LastValuesPanel.GetMeteringValue(meterKey);
            VerifyEqual("6. Verify Relay1Level is recorded in Data History", expectedValue, actualValue);

            Step("7. Select Failure tab");
            dataHistoryPage.LastValuesPanel.SelectTab("Failures");

            Step("8. Verify Communication Failure is cleared with the green checked icon.");
            actualIcon = dataHistoryPage.LastValuesPanel.GetFailureIcon(failure);
            VerifyEqual("8. Verify Communication Failure is cleared with the green checked icon.", FailureIcon.OK, actualIcon);
            
            Step("9. Repeat the test with simulating data for the cabinet controller by sending a command");
            Step(" o valueName=Relay1State");
            Step(" o value= 100");
            Step(" o eventTime= the current UTC datetime");

            Step("9.1. Simulate the Communication Failure for the cabinet controller");
            eventTime = Settings.GetServerTime().AddMinutes(-1);
            SetValueToDevice(controller, cabinetController, failure, true, eventTime);

            Step("9.2. Go to Data History app and select the testing cabinet controller, then select the Failure tab");
            desktopPage = Browser.RefreshLoggedInCMS();
            dataHistoryPage = desktopPage.GoToApp(App.DataHistory) as DataHistoryPage;

            Step("9.3. Verify Communication Failure is recorded with the red error icon");
            dataHistoryPage.GeozoneTreeMainPanel.SelectNode(geozone + @"\" + cabinetController);
            dataHistoryPage.WaitForLastValuePanelDisplayed();
            dataHistoryPage.LastValuesPanel.SelectTab("Failures");
            actualIcon = dataHistoryPage.LastValuesPanel.GetFailureIcon(failure);
            VerifyEqual("9.3. Verify Communication Failure is recorded with the red error icon", FailureIcon.Error, actualIcon);

            Step("9.4. Simulate data for the cabinet controller by sending a command");
            meterKey = "Relay1State";
            meterName = "Relay1State";
            value = "100.00";
            eventTime = Settings.GetServerTime();
            SetValueToDevice(controller, cabinetController, meterKey, value, eventTime);

            Step("9.5. Choose another device and select the testing cabinet controller again, then select the Meterings tab");
            dataHistoryPage.LastValuesPanel.ClickBackToolbarButton();
            dataHistoryPage.WaitForLastValuePanelDisappeared();
            dataHistoryPage.GeozoneTreeMainPanel.SelectNode(controller);
            dataHistoryPage.WaitForLastValuePanelDisplayed();
            dataHistoryPage.LastValuesPanel.ClickBackToolbarButton();
            dataHistoryPage.WaitForLastValuePanelDisappeared();
            Wait.ForMinutes(1); // Wait for failure changes status
            dataHistoryPage.GeozoneTreeMainPanel.SelectNode(cabinetController);
            dataHistoryPage.WaitForLastValuePanelDisplayed();
            dataHistoryPage.LastValuesPanel.SelectTab("Meterings");

            Step("9.6. Verify Lamp Level Command is recorded in Data History");
            expectedValue = value;
            actualValue = dataHistoryPage.LastValuesPanel.GetMeteringValue(meterKey);
            VerifyEqual("9.6. Verify Lamp Level Command is recorded in Data History", expectedValue, actualValue);

            Step("9.7. Select Failure tab");
            dataHistoryPage.LastValuesPanel.SelectTab("Failures");

            Step("9.8. Verify Communication Failure is cleared with the green checked icon.");
            actualIcon = dataHistoryPage.LastValuesPanel.GetFailureIcon(failure);
            VerifyEqual("9.8. Verify Communication Failure is cleared with the green checked icon.", FailureIcon.OK, actualIcon);

            try
            {
                DeleteGeozone(geozone);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("DA_18 - Cabinet Controller in Data History app")]
        public void DA_18()
        {
            var testData = GetTestDataOfDA_18();
            var expectedMeterings = testData["Meterings"] as List<string>;
            var expectedFailures = testData["Failures"] as List<string>;
            var geozone = SLVHelper.GenerateUniqueName("GZNDA18");
            var controller = SLVHelper.GenerateUniqueName("CTRL");
            var cabinetController = SLVHelper.GenerateUniqueName("CBN");

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - Create a new geozone containing the Cabinet Controller");
            Step("**** Precondition ****\n");

            Step("-> Create data for testing");
            DeleteGeozones("GZNDA18*");
            CreateNewGeozone(geozone);
            CreateNewController(controller, geozone);
            CreateNewDevice(DeviceType.CabinetController, cabinetController, controller, geozone);

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.DataHistory, App.DeviceHistory);
            
            Step("1. Go to Data History app and select the testing streetlight, then select the Failure tab");
            var dataHistoryPage = desktopPage.GoToApp(App.DataHistory) as DataHistoryPage;
            
            Step("2. Click on the Cabinet Controller");
            dataHistoryPage.GeozoneTreeMainPanel.SelectNode(geozone + @"\" + cabinetController);
            dataHistoryPage.WaitForLastValuePanelDisplayed();

            Step("3. Verify The panel appears with 2 tabs: Meterings & Failures containing all attributes of Cabinet Controller.");
            Step(" - Meterings: there are 18 attributes");
            Step("  + Active energy (KWh)");
            Step("  + Active power (W)");
            Step("  + Calendar changed");
            Step("  + Current - L1 (A)");
            Step("  + Current - L2 (A)");
            Step("  + Current - L3 (A)");
            Step("  + Mains current");
            Step("  + Power - L1 (W)");
            Step("  + Power - L2 (W)");
            Step("  + Power - L3 (W)");
            Step("  + Program changed");
            Step("  + Relay1CommandMode");
            Step("  + Relay1Level");
            Step("  + Relay1State");
            Step("  + Voltage - L1 (V)");
            Step("  + Voltage - L2 (V)");
            Step("  + Voltage - L3 (V)");
            Step("  + Voltage average (V)");
            Step(" - Failures: there are 6 attributes");
            Step("  + Calendar commission failure");
            Step("  + Communication failure");
            Step("  + Door open");
            Step("  + Invalid calendar");
            Step("  + Invalid program");
            Step("  + Power Supply Failure");         
            dataHistoryPage.LastValuesPanel.SelectTab("Meterings");
            var actualMeterings = dataHistoryPage.LastValuesPanel.GetMeteringsNameList();
            VerifyEqual("3. Verify Meterings: there are 18 attributes as expected", expectedMeterings, actualMeterings, false);

            dataHistoryPage.LastValuesPanel.SelectTab("Failures");
            var actualFailures = dataHistoryPage.LastValuesPanel.GetFailuresNameList();
            VerifyEqual("3. Verify Failures: there are 6 attributes as expected", expectedFailures, actualFailures, false);

            Step("4. Close the panel");
            dataHistoryPage.LastValuesPanel.ClickBackToolbarButton();
            dataHistoryPage.WaitForLastValuePanelDisappeared();

            Step("5. Expected The panel is closed");
            VerifyEqual("5. Verify The panel is closed", true, !dataHistoryPage.IsLastValuePanelDisplayed());

            try
            {
                DeleteGeozone(geozone);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("DA_19 - The data and the time displayed for Metering attributes of Cabinet Controller")]
        public void DA_19()
        {
            var testData = GetTestDataOfDA_19();
            var olsonTimeZoneId = testData["OlsonTimeZoneId"];
            var geozone = SLVHelper.GenerateUniqueName("GZNDA19");
            var controller = SLVHelper.GenerateUniqueName("CTRL");
            var cabinetControler = SLVHelper.GenerateUniqueName("CBN");
            var streetlight = SLVHelper.GenerateUniqueName("STL");

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - Create new controller with specific timezone");
            Step(" - Create a cabinet controller using above controller");
            Step(" - Prepare the command to simulate data for attributes of Meterings. Refer to the below table for Attribute Name and Parrameter.");
            Step(" + Active energy (KWh): TotalKWHPositive");
            Step(" + Active power (W): SummedPower");
            Step(" + Calendar changed: CalendarChanged");
            Step(" + Current - L1 (A): Phase1Current");
            Step(" + Current - L2 (A): Phase2Current");
            Step(" + Current - L3 (A): Phase3Current");
            Step(" + Mains current: Current");
            Step(" + Power - L1 (W): Phase1Power");
            Step(" + Power - L2 (W): Phase2Power");
            Step(" + Power - L3 (W): Phase3Power");
            Step(" + Program changed: ProgramChanged");
            Step(" + Relay1CommandMode: Relay1CommandMode");
            Step(" + Relay1Level: Relay1Level");
            Step(" + Relay1State: Relay1State");
            Step(" + Voltage - L1 (V): Phase1Voltage");
            Step(" + Voltage - L2 (V): Phase2Voltage");
            Step(" + Voltage - L3 (V): Phase3Voltage");
            Step(" + Voltage average (V): VoltageAverage");
            Step("**** Precondition ****\n");

            Step("-> Create data for testing");
            DeleteGeozones("GZNDA19*");
            CreateNewGeozone(geozone);
            CreateNewController(controller, geozone);
            SetValueToController(controller, "TimeZoneId", olsonTimeZoneId, Settings.GetServerTime());
            CreateNewDevice(DeviceType.CabinetController, cabinetControler, controller, geozone);
            CreateNewDevice(DeviceType.Streetlight, streetlight, controller, geozone);

            var attributes = new List<string>() { "TotalKWHPositive", "SummedPower", "CalendarChanged", "Phase1Current", "Phase2Current", "Phase3Current", "Current", "Phase1Power", "Phase2Power", "Phase3Power", "ProgramChanged", "Relay1CommandMode", "Relay1Level", "Relay1State", "Phase1Voltage", "Phase2Voltage", "Phase3Voltage", "VoltageAverage" };            
            var dicDataCommand = new Dictionary<string, string>();
            foreach (var attribute in attributes)
            {
                var randomValue = SLVHelper.GenerateInteger(999).ToString("N2");
                if (attribute.Equals("CalendarChanged") || attribute.Equals("ProgramChanged")) randomValue = new List<string> { "ON", "OFF" }.PickRandom();
                dicDataCommand.Add(attribute, randomValue);
            }

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.DataHistory);

            Step("1. Go to Data History app and go to the geozone from the precondition");
            var dataHistoryPage = desktopPage.GoToApp(App.DataHistory) as DataHistoryPage;

            Step("2. Click on the cabinet controller");
            dataHistoryPage.GeozoneTreeMainPanel.SelectNode(geozone + @"\" + cabinetControler);
            dataHistoryPage.WaitForLastValuePanelDisplayed();

            Step("3. Send 5 commands to simulate the data for 5 random attributes of Metering with the following eventTime");
            Step(" o eventTime=The current date time of controller");
            Step(" o eventTime=The current date time of controller - 10 minutes");
            Step(" o eventTime=The current date time of controller - 1 hour");
            Step(" o eventTime=The current date time of controller - 24 hours");
            Step(" o eventTime=The current date time of controller - 32 days");
            Step("4. Verify All 5 commands sent OK");
            var eventTime = Settings.GetCurrentControlerDateTime(controller);
            var eventTimeList = new List<DateTime> { eventTime, eventTime.AddMinutes(-10), eventTime.AddHours(-1), eventTime.AddHours(-24), eventTime.AddDays(-32) };
            var dicData = new List<KeyValuePair<string, string>>();
            foreach (var time in eventTimeList)
            {
                var data = dicDataCommand.PickRandom();
                dicData.Add(data);
                var request = SetValueToDevice(controller, cabinetControler, data.Key, data.Value, time);
                VerifyEqual(string.Format("4. Verify the request is sent successfully (attribute: {0}, value: {1})", data.Key, data.Value), true, request);
                dicDataCommand.Remove(data.Key);
            }

            Step("5. Press Back to the geozone, select another device, press back, then select the testing cabinet controller again to refresh data");
            dataHistoryPage.LastValuesPanel.ClickBackToolbarButton();
            dataHistoryPage.WaitForLastValuePanelDisappeared();
            dataHistoryPage.GeozoneTreeMainPanel.SelectNode(streetlight);
            dataHistoryPage.WaitForLastValuePanelDisplayed();
            dataHistoryPage.LastValuesPanel.ClickBackToolbarButton();
            dataHistoryPage.WaitForLastValuePanelDisappeared();
            dataHistoryPage.GeozoneTreeMainPanel.SelectNode(cabinetControler);
            dataHistoryPage.WaitForLastValuePanelDisplayed();

            Step("6. Verify For 5 testing attributes the data and time are displayed as 2 following columns");
            Step(" o Data = testing data from the command (ex: 100.00), Time = 1 s");
            Step(" o Data = testing data from the command, Time = 10 mn (current time - 10 minutes)");
            Step(" o Data = testing data from the command, Time = 1 h (current time - 1 hour)");
            Step(" o Data = testing data from the command, Time = 1 d (current time - 24 hours)");
            Step(" o Data = testing data from the command, Time = 32 d (current time - 32 days)");
            VerifyMeteringAttributeBetween(dataHistoryPage, dicData[0].Key, dicData[0].Value, 0, 5);
            VerifyMeteringAttributeBetween(dataHistoryPage, dicData[1].Key, dicData[1].Value, 5, 15);
            VerifyMeteringAttribute(dataHistoryPage, dicData[2].Key, dicData[2].Value, "1 h");
            VerifyMeteringAttribute(dataHistoryPage, dicData[3].Key, dicData[3].Value, "1 d");
            VerifyMeteringAttribute(dataHistoryPage, dicData[4].Key, dicData[4].Value, "32 d");

            try
            {
                DeleteGeozone(geozone);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("DA_20 - The data and the time displayed for Failures attributes of Cabinet Controller")]
        public void DA_20()
        {
            var testData = GetTestDataOfDA_20();
            var olsonTimeZoneId = testData["OlsonTimeZoneId"];
            var geozone = SLVHelper.GenerateUniqueName("GZNDA20");
            var controller = SLVHelper.GenerateUniqueName("CTRL");
            var cabinetControler = SLVHelper.GenerateUniqueName("CBN");
            var streetlight = SLVHelper.GenerateUniqueName("STL");

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - Create new controller with specific timezone");
            Step(" - Create a cabinet controller using above controller");
            Step(" - Prepare the command to simulate data for attributes of Meterings. Refer to the below table for Attribute Name and Parrameter.");
            Step(" + Calendar commission failure: calendarCommissionFailure");
            Step(" + Communication failure: CommunicationFailure");
            Step(" + Door open: DoorOpen");
            Step(" + Invalid calendar: CalendarFailure");
            Step(" + Invalid program: ControlProgramFailure");
            Step(" + Power Supply Failure: PowerSupplyFailure");
       
            Step("**** Precondition ****\n");

            Step("-> Create data for testing");
            DeleteGeozones("GZNDA20*");
            CreateNewGeozone(geozone);
            CreateNewController(controller, geozone);
            SetValueToController(controller, "TimeZoneId", olsonTimeZoneId, Settings.GetServerTime());
            CreateNewDevice(DeviceType.CabinetController, cabinetControler, controller, geozone);
            CreateNewDevice(DeviceType.Streetlight, streetlight, controller, geozone);

            var failures = new List<string>() { "Calendar commission failure:calendarCommissionFailure", "Communication failure:CommunicationFailure", "Door open:DoorOpen", "Invalid calendar:CalendarFailure", "Invalid program:ControlProgramFailure", "Power Supply Failure:PowerSupplyFailure" };
            var rndFailures = failures.PickRandom(5);

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.DataHistory);

            Step("1. Go to Data History app and go to the geozone from the precondition");
            var dataHistoryPage = desktopPage.GoToApp(App.DataHistory) as DataHistoryPage;

            Step("2. Click on the cabinet controller");
            dataHistoryPage.GeozoneTreeMainPanel.SelectNode(geozone + @"\" + cabinetControler);
            dataHistoryPage.WaitForLastValuePanelDisplayed();

            Step("3. Send 5 commands to simulate the data for 5 random attributes of Failures with the following eventTime, and value=true");
            Step(" o eventTime=The current date time of controller");
            Step(" o eventTime=The current date time of controller - 10 minutes");
            Step(" o eventTime=The current date time of controller - 1 hour");
            Step(" o eventTime=The current date time of controller - 24 hours");
            Step(" o eventTime=The current date time of controller - 32 days");
            Step("4. Verify All 5 commands sent OK");
            var eventTime = Settings.GetCurrentControlerDateTime(controller);
            var eventTimeList = new List<DateTime> { eventTime, eventTime.AddMinutes(-10), eventTime.AddHours(-1), eventTime.AddHours(-24), eventTime.AddDays(-32) };
            var failuresKey = new List<string>();
            var dicFailuresTime = new Dictionary<string, DateTime>();
            for (int i = 0; i < eventTimeList.Count; i++)
            {
                var name = rndFailures[i].SplitAndGetAt(new char[] { ':' }, 0);
                var key = rndFailures[i].SplitAndGetAt(new char[] { ':' }, 1);
                var time = eventTimeList[i];
                dicFailuresTime.Add(name, time);
                failuresKey.Add(key);
                var request = SetValueToDevice(controller, cabinetControler, key, true, time);
                VerifyEqual(string.Format("4. Verify the request is sent successfully (attribute: {0}, value: {1})", key, "true"), true, request);
            }

            Step("5. Press Back to the geozone, select another device, press back, then select the testing cabinet controller again to refresh data");
            dataHistoryPage.LastValuesPanel.ClickBackToolbarButton();
            dataHistoryPage.WaitForLastValuePanelDisappeared();
            dataHistoryPage.GeozoneTreeMainPanel.SelectNode(streetlight);
            dataHistoryPage.WaitForLastValuePanelDisplayed();
            dataHistoryPage.LastValuesPanel.ClickBackToolbarButton();
            dataHistoryPage.WaitForLastValuePanelDisappeared();
            dataHistoryPage.GeozoneTreeMainPanel.SelectNode(cabinetControler);
            dataHistoryPage.WaitForLastValuePanelDisplayed();
            dataHistoryPage.LastValuesPanel.SelectTab("Failures");

            Step("6. Verify For 5 testing attributes the data and time are displayed as 2 following columns");
            Step(" o Data = testing data from the command (ex: 100.00), Time = 1 s");
            Step(" o Data = testing data from the command, Time = 10 mn (current time - 10 minutes)");
            Step(" o Data = testing data from the command, Time = 1 h (current time - 1 hour)");
            Step(" o Data = testing data from the command, Time = 1 d (current time - 24 hours)");
            Step(" o Data = testing data from the command, Time = 32 d (current time - 32 days)");
            VerifyFailureAttributeWarningBetween(dataHistoryPage, failuresKey[0], 0, 5);
            VerifyFailureAttributeWarningBetween(dataHistoryPage, failuresKey[1], 5, 15);
            VerifyFailureAttributeWarning(dataHistoryPage, failuresKey[2], "1 h");
            VerifyFailureAttributeWarning(dataHistoryPage, failuresKey[3], "1 d");
            VerifyFailureAttributeWarning(dataHistoryPage, failuresKey[4], "32 d");

            Step("7. Hover an attribute");
            var failureData = dicFailuresTime.PickRandom();
            var expectedValue = failureData.Value.ToString("M/d/yyyy H:mm tt");
            dataHistoryPage.LastValuesPanel.MoveHoverFailuresAttribute(failureData.Key);
            var tooltipValue = DateTime.Parse(dataHistoryPage.LastValuesPanel.GetFailuresTooltipAttribute(failureData.Key)).ToString("M/d/yyyy H:mm tt");

            Step("8. Verify A tooltip displays the date and time data is simulated. Ex: 12/27/2017 4:40 AM");
            VerifyEqual("8. Verify A tooltip displays the date and time data is simulated. Ex: 12/27/2017 4:40 AM", expectedValue, tooltipValue);

            Step("9. Send 5 commands to simulate the data for 5 random attributes of Failures with the following eventTime, and value=false");
            Step(" o eventTime=The current date time of controller");
            Step(" o eventTime=The current date time of controller - 10 minutes");
            Step(" o eventTime=The current date time of controller - 1 hour");
            Step(" o eventTime=The current date time of controller - 24 hours");
            Step(" o eventTime=The current date time of controller - 32 days");
            Step("10. Verify All 5 commands sent OK");
            eventTime = Settings.GetCurrentControlerDateTime(controller);
            eventTimeList = new List<DateTime> { eventTime, eventTime.AddMinutes(-10), eventTime.AddHours(-1), eventTime.AddHours(-24), eventTime.AddDays(-32) };
            dicFailuresTime = new Dictionary<string, DateTime>();
            for (int i = 0; i < eventTimeList.Count; i++)
            {
                var name = rndFailures[i].SplitAndGetAt(new char[] { ':' }, 0);
                var key = rndFailures[i].SplitAndGetAt(new char[] { ':' }, 1);
                var time = eventTimeList[i];
                dicFailuresTime.Add(name, time);
                var request = SetValueToDevice(controller, cabinetControler, key, false, time);
                VerifyEqual(string.Format("10. Verify the request is sent successfully (attribute: {0}, value: {1})", key, "false"), true, request);
            }

            Step("11. Press Back to the geozone, select another device, press back, then select the testing streetlight again to refresh data");
            dataHistoryPage.LastValuesPanel.ClickBackToolbarButton();
            dataHistoryPage.WaitForLastValuePanelDisappeared();
            dataHistoryPage.GeozoneTreeMainPanel.SelectNode(streetlight);
            dataHistoryPage.WaitForLastValuePanelDisplayed();
            dataHistoryPage.LastValuesPanel.ClickBackToolbarButton();
            dataHistoryPage.WaitForLastValuePanelDisappeared();
            dataHistoryPage.GeozoneTreeMainPanel.SelectNode(cabinetControler);
            dataHistoryPage.WaitForLastValuePanelDisplayed();
            dataHistoryPage.LastValuesPanel.SelectTab("Failures");

            Step("12. Verify For 5 testing attributes the data and time are displayed as 2 following columns");
            Step(" o Checked icon, Time = 1 s");
            Step(" o Checked icon, Time = 10 mn (current time - 10 minutes)");
            Step(" o Checked icon, Time = 1 h (current time - 1 hour)");
            Step(" o Checked icon, Time = 1 d (current time - 24 hours)");
            Step(" o Checked icon, Time = 32 d (current time - 32 days)");
            VerifyFailureAttributeOkBetween(dataHistoryPage, failuresKey[0], 0, 5);
            VerifyFailureAttributeOkBetween(dataHistoryPage, failuresKey[1], 5, 15);
            VerifyFailureAttributeOk(dataHistoryPage, failuresKey[2], "1 h");
            VerifyFailureAttributeOk(dataHistoryPage, failuresKey[3], "1 d");
            VerifyFailureAttributeOk(dataHistoryPage, failuresKey[4], "32 d");

            Step("13. Hover an attribute");
            failureData = dicFailuresTime.PickRandom();
            expectedValue = failureData.Value.ToString("M/d/yyyy H:mm tt");
            dataHistoryPage.LastValuesPanel.MoveHoverFailuresAttribute(failureData.Key);
            tooltipValue = DateTime.Parse(dataHistoryPage.LastValuesPanel.GetFailuresTooltipAttribute(failureData.Key)).ToString("M/d/yyyy H:mm tt");

            Step("14. Verify A tooltip displays the date and time data is simulated. Ex: 12/27/2017 6:00 AM");
            VerifyEqual("14. Verify A tooltip displays the date and time data is simulated. Ex: 12/27/2017 6:00 AM", expectedValue, tooltipValue);

            try
            {
                DeleteGeozone(geozone);
            }
            catch { }
        }

        #endregion //Test Cases

        #region Private methods  


        /// <summary>
        /// Get minute of time string
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        private int GetTimeValueInMinute(string time)
        {

            int valueInMinute = 0;

            if (time.Contains("s"))
            {
                var value = int.Parse(time.Replace("s", string.Empty).Trim());
                valueInMinute = value / 60;
            }
            else if (time.Contains("m"))
            {
                valueInMinute = int.Parse(time.Replace("m", string.Empty).Trim());
            }

            return valueInMinute;
        }

        /// <summary>
        ///  Verify an attribute of Metering
        /// </summary>
        /// <param name="page"></param>
        /// <param name="attribute"></param>
        /// <param name="expectedValue"></param>
        /// <param name="expectedTime"></param>
        private void VerifyMeteringAttribute(DataHistoryPage page, string attribute, string expectedValue, string expectedTime)
        {
            var actualValue = page.LastValuesPanel.GetMeteringValue(attribute);
            var actualTime = page.LastValuesPanel.GetMeteringTime(attribute);

            VerifyEqual(string.Format("Verify Attribute: {0} with Data = {1} Time = {2}", attribute, expectedValue, expectedTime), expectedValue, actualValue);
            VerifyEqual(string.Format("Verify Attribute: {0} with Data = {1} Time = {2}", attribute, expectedValue, expectedTime), expectedTime, actualTime);
        }        

        /// <summary>
        /// Verify an attribute of Metering between 'from minute' and 'to minute'
        /// </summary>
        /// <param name="page"></param>
        /// <param name="attribute"></param>
        /// <param name="expectedValue"></param>
        /// <param name="expectedTimeFrom"></param>
        /// <param name="expectedTimeTo"></param>
        private void VerifyMeteringAttributeBetween(DataHistoryPage page, string attribute, string expectedValue, int expectedMinuteFrom, int expectedMinuteTo)
        {
            var actualValue = page.LastValuesPanel.GetMeteringValue(attribute);
            var actualTime = page.LastValuesPanel.GetMeteringTime(attribute);
            var actualValueInMinute = GetTimeValueInMinute(actualTime);

            VerifyEqual(string.Format("Verify Attribute: {0} with Data = {1} Time between {2} mn and {3} mn (Actual value: {4})", attribute, expectedValue, expectedMinuteFrom, expectedMinuteTo, actualValue), expectedValue, actualValue);
            VerifyEqual(string.Format("Verify Attribute: {0} with Data = {1} Time between {2} mn and {3} mn (Actual time: {4})", attribute, expectedValue, expectedMinuteFrom, expectedMinuteTo, actualValueInMinute), true, expectedMinuteFrom <= actualValueInMinute  && actualValueInMinute <= expectedMinuteTo);
        }

        /// <summary>
        /// Verify an attribute of Failures is Warning (except for LampRestartCount, LampFailure, CommunicationFailure, PowerSupplyFailure)
        /// </summary>
        /// <param name="page"></param>
        /// <param name="attribute"></param>
        /// <param name="expectedTime"></param>
        private void VerifyFailureAttributeWarning(DataHistoryPage page, string attribute, string expectedTime)
        {
            var actualIcon = page.LastValuesPanel.GetFailureIcon(attribute);
            var actualTime = page.LastValuesPanel.GetFailureTime(attribute);

            if (attribute.Equals("LampRestartCount") || attribute.Equals("LampFailure") || attribute.Equals("CommunicationFailure") || attribute.Equals("PowerSupplyFailure"))
                VerifyEqual(string.Format("Verify Attribute: {0} with Error icon", attribute), FailureIcon.Error, actualIcon);
            else
                VerifyEqual(string.Format("Verify Attribute: {0} with Warning icon", attribute), FailureIcon.Warning, actualIcon);
            VerifyEqual(string.Format("Verify Attribute: {0} with Time = {1}", attribute, expectedTime), expectedTime, actualTime);
        }

        /// <summary>
        ///  Verify an attribute of Failures is Warning (except for LampRestartCount, LampFailure, CommunicationFailure, PowerSupplyFailure)
        /// </summary>
        /// <param name="page"></param>
        /// <param name="attribute"></param>
        /// <param name="expectedMinuteFrom"></param>
        /// <param name="expectedMinuteTo"></param>
        private void VerifyFailureAttributeWarningBetween(DataHistoryPage page, string attribute, int expectedMinuteFrom, int expectedMinuteTo)
        {
            var actualIcon = page.LastValuesPanel.GetFailureIcon(attribute);
            var actualTime = page.LastValuesPanel.GetFailureTime(attribute);
            var actualValueInMinute = GetTimeValueInMinute(actualTime);

            if (attribute.Equals("LampRestartCount") || attribute.Equals("LampFailure") || attribute.Equals("CommunicationFailure") || attribute.Equals("PowerSupplyFailure"))
                VerifyEqual(string.Format("Verify Attribute: {0} with Error icon", attribute), FailureIcon.Error, actualIcon);
            else
                VerifyEqual(string.Format("Verify Attribute: {0} with Warning icon", attribute), FailureIcon.Warning, actualIcon);
            VerifyEqual(string.Format("Verify Attribute: {0} with Time between {1} mn and {2} mn", attribute, expectedMinuteFrom, expectedMinuteTo), true, expectedMinuteFrom <= actualValueInMinute && actualValueInMinute <= expectedMinuteTo);
        }

        /// <summary>
        ///  Verify an attribute of Failures is OK
        /// </summary>
        /// <param name="page"></param>
        /// <param name="attribute"></param>
        /// <param name="expectedTime"></param>
        private void VerifyFailureAttributeOk(DataHistoryPage page, string attribute, string expectedTime)
        {
            var actualIcon = page.LastValuesPanel.GetFailureIcon(attribute);
            var actualTime = page.LastValuesPanel.GetFailureTime(attribute);
            
            VerifyEqual(string.Format("Verify Attribute: {0} with OK icon", attribute), FailureIcon.OK, actualIcon);
            VerifyEqual(string.Format("Verify Attribute: {0} with Time = {1}", attribute, expectedTime), expectedTime, actualTime);
        }

        private void VerifyFailureAttributeOkBetween(DataHistoryPage page, string attribute, int expectedMinuteFrom, int expectedMinuteTo)
        {
            var actualIcon = page.LastValuesPanel.GetFailureIcon(attribute);
            var actualTime = page.LastValuesPanel.GetFailureTime(attribute);
            var actualValueInMinute = GetTimeValueInMinute(actualTime);

            VerifyEqual(string.Format("Verify Attribute: {0} with OK icon", attribute), FailureIcon.OK, actualIcon);
            VerifyEqual(string.Format("Verify Attribute: {0} with Time between {1} mn and {2} mn", attribute, expectedMinuteFrom, expectedMinuteTo), true, expectedMinuteFrom <= actualValueInMinute && actualValueInMinute <= expectedMinuteTo);
        }

        #region Input XML data

        /// <summary>
        /// Get common data test
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, object> GetCommonTestData()
        {
            var realtimeGeozone = Settings.CommonTestData[0];
            var testData = new Dictionary<string, object>();
            testData.Add("Geozone", realtimeGeozone.Path);
            var streetlights = realtimeGeozone.Devices.Where(p => p.Type == DeviceType.Streetlight && p.Status == DeviceStatus.Working).ToList();
            testData.Add("Streetlights", streetlights);

            return testData;
        }

        private Dictionary<string, object> GetTestDataOfDA_01()
        {
            var testCaseName = "DA_01";
            var xmlUtility = new XmlUtility(Settings.DATA_HISTORY_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, object>();
            testData.Add("Geozone", xmlUtility.GetSingleNodeText(string.Format(Settings.DATA_HISTORY_XPATH_PREFIX, testCaseName, "Geozone")));
            testData.Add("StreetlightMeterings", xmlUtility.GetChildNodesText(string.Format(Settings.DATA_HISTORY_XPATH_PREFIX, testCaseName, "StreetlightMeterings")));
            testData.Add("StreetlightFailures", xmlUtility.GetChildNodesText(string.Format(Settings.DATA_HISTORY_XPATH_PREFIX, testCaseName, "StreetlightFailures")));
            testData.Add("ControllerMeterings", xmlUtility.GetChildNodesText(string.Format(Settings.DATA_HISTORY_XPATH_PREFIX, testCaseName, "ControllerMeterings")));
            testData.Add("ControllerFailures", xmlUtility.GetChildNodesText(string.Format(Settings.DATA_HISTORY_XPATH_PREFIX, testCaseName, "ControllerFailures")));
            testData.Add("MeterMeterings", xmlUtility.GetChildNodesText(string.Format(Settings.DATA_HISTORY_XPATH_PREFIX, testCaseName, "MeterMeterings")));            

            return testData;
        }

        private Dictionary<string, string> GetTestDataOfDA_02()
        {
            var testCaseName = "DA_02";
            var xmlUtility = new XmlUtility(Settings.DATA_HISTORY_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, string>();
            testData.Add("OlsonTimeZoneId", xmlUtility.GetSingleNodeText(string.Format(Settings.DATA_HISTORY_XPATH_PREFIX, testCaseName, "OlsonTimeZoneId")));

            return testData;
        }

        private Dictionary<string, string> GetTestDataOfDA_03()
        {
            var testCaseName = "DA_03";
            var xmlUtility = new XmlUtility(Settings.DATA_HISTORY_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, string>();
            testData.Add("OlsonTimeZoneId", xmlUtility.GetSingleNodeText(string.Format(Settings.DATA_HISTORY_XPATH_PREFIX, testCaseName, "OlsonTimeZoneId")));

            return testData;
        }

        private Dictionary<string, object> GetTestDataOfDA_04()
        {
            var testCaseName = "DA_04";
            var xmlUtility = new XmlUtility(Settings.DATA_HISTORY_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, object>();
            testData.Add("SearchFields", xmlUtility.GetChildNodesText(string.Format(Settings.DATA_HISTORY_XPATH_PREFIX, testCaseName, "SearchFields")));

            return testData;
        }

        private Dictionary<string, object> GetTestDataOfDA_11()
        {
            var testCaseName = "DA_11";
            var xmlUtility = new XmlUtility(Settings.DATA_HISTORY_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, object>();
            var equipmentTypes = new Dictionary<string, string>();
            var equipmentTypesNodes = xmlUtility.GetChildNodes(string.Format(Settings.DATA_HISTORY_XPATH_PREFIX, testCaseName, "EquipmentTypes"));
            foreach (var node in equipmentTypesNodes)
            {
                equipmentTypes.Add(node.GetAttrVal("id"), node.GetAttrVal("name"));
            }
            testData.Add("EquipmentTypes", equipmentTypes);
            var lampTypes = new Dictionary<string, string>();
            var lampTypesNodes = xmlUtility.GetChildNodes(string.Format(Settings.DATA_HISTORY_XPATH_PREFIX, testCaseName, "LampTypes"));
            foreach (var node in lampTypesNodes)
            {
                lampTypes.Add(node.GetAttrVal("id"), node.GetAttrVal("name"));
            }
            testData.Add("LampTypes", lampTypes);
            testData.Add("DimmingGroups", xmlUtility.GetChildNodesText(string.Format(Settings.DATA_HISTORY_XPATH_PREFIX, testCaseName, "DimmingGroups")));

            return testData;
        }

        private Dictionary<string, object> GetTestDataOfDA_12()
        {
            var testCaseName = "DA_12";
            var xmlUtility = new XmlUtility(Settings.DATA_HISTORY_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, object>();
            var equipmentTypes = new Dictionary<string, string>();
            var equipmentTypesNodes = xmlUtility.GetChildNodes(string.Format(Settings.DATA_HISTORY_XPATH_PREFIX, testCaseName, "EquipmentTypes"));
            foreach (var node in equipmentTypesNodes)
            {
                equipmentTypes.Add(node.GetAttrVal("id"), node.GetAttrVal("name"));
            }
            testData.Add("EquipmentTypes", equipmentTypes);
            var lampTypes = new Dictionary<string, string>();
            var lampTypesNodes = xmlUtility.GetChildNodes(string.Format(Settings.DATA_HISTORY_XPATH_PREFIX, testCaseName, "LampTypes"));
            foreach (var node in lampTypesNodes)
            {
                lampTypes.Add(node.GetAttrVal("id"), node.GetAttrVal("name"));
            }
            testData.Add("LampTypes", lampTypes);
            testData.Add("DimmingGroups", xmlUtility.GetChildNodesText(string.Format(Settings.DATA_HISTORY_XPATH_PREFIX, testCaseName, "DimmingGroups")));

            return testData;
        }

        private Dictionary<string, object> GetTestDataOfDA_17()
        {
            var testCaseName = "DA_17";
            var xmlUtility = new XmlUtility(Settings.DATA_HISTORY_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, object>();
            var equipmentTypes = new Dictionary<string, string>();
            var equipmentTypesNodes = xmlUtility.GetChildNodes(string.Format(Settings.DATA_HISTORY_XPATH_PREFIX, testCaseName, "EquipmentTypes"));
            foreach (var node in equipmentTypesNodes)
            {
                equipmentTypes.Add(node.GetAttrVal("id"), node.GetAttrVal("name"));
            }
            testData.Add("EquipmentTypes", equipmentTypes);

            return testData;
        }

        private Dictionary<string, object> GetTestDataOfDA_18()
        {
            var testCaseName = "DA_18";
            var xmlUtility = new XmlUtility(Settings.DATA_HISTORY_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, object>();
            var meterings = xmlUtility.GetChildNodesText(string.Format(Settings.DATA_HISTORY_XPATH_PREFIX, testCaseName, "Meterings"));
            var failures = xmlUtility.GetChildNodesText(string.Format(Settings.DATA_HISTORY_XPATH_PREFIX, testCaseName, "Failures"));
            testData.Add("Meterings", meterings);
            testData.Add("Failures", failures);

            return testData;
        }

        private Dictionary<string, string> GetTestDataOfDA_19()
        {
            var testCaseName = "DA_19";
            var xmlUtility = new XmlUtility(Settings.DATA_HISTORY_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, string>();
            testData.Add("OlsonTimeZoneId", xmlUtility.GetSingleNodeText(string.Format(Settings.DATA_HISTORY_XPATH_PREFIX, testCaseName, "OlsonTimeZoneId")));

            return testData;
        }

        private Dictionary<string, string> GetTestDataOfDA_20()
        {
            var testCaseName = "DA_20";
            var xmlUtility = new XmlUtility(Settings.DATA_HISTORY_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, string>();
            testData.Add("OlsonTimeZoneId", xmlUtility.GetSingleNodeText(string.Format(Settings.DATA_HISTORY_XPATH_PREFIX, testCaseName, "OlsonTimeZoneId")));

            return testData;
        }

        #endregion //Input XML data

        #endregion //Private methods
    }
}