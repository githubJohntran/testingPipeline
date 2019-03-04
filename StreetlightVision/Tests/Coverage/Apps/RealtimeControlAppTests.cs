using NUnit.Framework;
using OpenQA.Selenium;
using StreetlightVision.Extensions;
using StreetlightVision.Models;
using StreetlightVision.Pages;
using StreetlightVision.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace StreetlightVision.Tests.Coverage.Apps
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    public class RealtimeControlAppTests : TestBase
    {
        #region Variables

        #endregion //Variables

        #region Contructors

        #endregion //Contructors

        #region Test Cases

        [Test, DynamicRetry]
        [Description("RTC_01 Control Widget - Controller - UI")]
        public void RTC_01()
        {
            var testData = GetTestDataOfRTC_01();
            var xmlController = testData["Controller"].ToString();
            var xmlCommands = testData["Commands"] as List<string>;
            var controllerName = xmlController.GetChildName();
            var geozone = xmlController.GetParentName();

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.RealTimeControl);

            Step("1. Go to Real-time Control app");
            Step("2. Expected Real-time Control page is routed and loaded successfully");
            var realtimeControlPage = desktopPage.GoToApp(App.RealTimeControl) as RealTimeControlPage;

            Step("3. Select a controller from geozone tree");
            realtimeControlPage.GeozoneTreeMainPanel.SelectNode(xmlController);

            Step("4. Verify Control widget for controller appears");
            realtimeControlPage.WaitForControllerWidgetDisplayed(controllerName);
            VerifyEqual("4. Verify Control widget for controller appears", true, realtimeControlPage.IsControllerWidgetDisplayed());

            Step("5. Verify Controller widget for controller has:");
            Step(" - Header bar:");
            Step("   + Refresh button");
            Step("   + INFORMATION button");
            Step("   + Close button");
            VerifyEqual("5. Verify Refresh button is visible", true, realtimeControlPage.ControllerWidgetPanel.IsRefreshButtonVisible());
            VerifyEqual("5. Verify INFORMATION button is visible", true, realtimeControlPage.ControllerWidgetPanel.IsInformationButtonVisible());
            VerifyEqual("5. Verify Close button is visible", true, realtimeControlPage.ControllerWidgetPanel.IsCloseButtonVisible());

            Step(" - Dimming levels bar:");
            Step("   + ON");
            Step("   + 40%, 50%, 60%, 70%, 80% (These levels are applied only to controller whose control technology is 'Open South Bound XML Web API')");
            Step("   + OFF");
            Step("   + Indicator (black triangle)");
            var listOfCommandsText = realtimeControlPage.ControllerWidgetPanel.GetListOfCommandsText();
            VerifyEqual("5. Verify Dimming levels as expected", xmlCommands, listOfCommandsText, false);
            VerifyEqual("5. Verify Indicator (black triangle) is visible", true, realtimeControlPage.ControllerWidgetPanel.IsIndicatorCursorVisible());

            Step(" - 'Feedback': '0%'");
            VerifyEqual("5. Verify label is 'Feedback'", "Feedback", realtimeControlPage.ControllerWidgetPanel.GetRealtimeFeedbackText());
            VerifyEqual("5. Verify value is '-%'", "-%", realtimeControlPage.ControllerWidgetPanel.GetRealtimeFeedbackValueText());

            Step(" - 'Command': '0%'");
            VerifyEqual("5. Verify label is 'Command'", "Command", realtimeControlPage.ControllerWidgetPanel.GetRealtimeCommandText());
            VerifyEqual("5. Verify value is '-%'", "-%", realtimeControlPage.ControllerWidgetPanel.GetRealtimeCommandValueText());

            Step(" - 'Date/Time': format 'M/dd/yyyy HH:mm:ss'");
            var systemTime = realtimeControlPage.ControllerWidgetPanel.GetSystemTimeValueText();
            VerifyEqual("5. Verify label is 'Date/Time'", "Date/Time", realtimeControlPage.ControllerWidgetPanel.GetSystemTimeText());
            VerifyTrue("5. Verify 'Date/Time' value format is 'M/dd/yyyy HH:mm:ss'", Regex.IsMatch(systemTime, @"\d{1,2}/\d{1,2}/\d{4} \d{2}:\d{2}:\d{2}"), "M/dd/yyyy HH:mm:ss", systemTime);

            Step(" - 'GPS Position'");
            double value;
            Step("   + Latitude: number");
            VerifyEqual("5. Verify label is 'Latitude'", "Latitude", realtimeControlPage.ControllerWidgetPanel.GetLatitubeText());
            VerifyEqual("5. Verify Latitude is number", true, double.TryParse(realtimeControlPage.ControllerWidgetPanel.GetLatitubeValue(), out value));
            Step("   + Longitude: number");
            VerifyEqual("5. Verify label is 'Longitude'", "Longitude", realtimeControlPage.ControllerWidgetPanel.GetLongitubeText());
            VerifyEqual("5. Verify Longitude is number", true, double.TryParse(realtimeControlPage.ControllerWidgetPanel.GetLongitubeValue(), out value));

            Step(" - 'Last time SC sent data': format 'M/dd/yyyy hh:mm:ss'");
            var lastTimeSC = realtimeControlPage.ControllerWidgetPanel.GetGpsLastDataSentValueText();
            VerifyEqual("5. Verify label is 'Last time SC sent data'", "Last time SC sent data", realtimeControlPage.ControllerWidgetPanel.GetGpsLastDataSentText());
            VerifyTrue("5. Verify 'Last time SC sent data' value format is 'M/dd/yyyy hh:mm:ss'", Regex.IsMatch(lastTimeSC, @"\d{1,2}/\d{1,2}/\d{4} \d{2}:\d{2}:\d{2}"), "M/dd/yyyy hh:mm:ss", lastTimeSC);

            Step(" -'Whole segment'");
            VerifyEqual("5. Verify label is 'Whole segment'", "Whole segment", realtimeControlPage.ControllerWidgetPanel.GetWholeSegmentText());
            Step(" - 'AUTOMATIC' button with clock icon");
            VerifyEqual("5. Verify AUTOMATIC' button is visible", true, realtimeControlPage.ControllerWidgetPanel.IsAutomationButtonVisible());

            Step(" - Controller icon and name at bottom left");
            VerifyEqual("5. Verify Controller icon at bottom left", true, realtimeControlPage.ControllerWidgetPanel.CheckIfDeviceIcon(DeviceType.Controller));
            VerifyEqual("5. Verify name at bottom left", controllerName, realtimeControlPage.ControllerWidgetPanel.GetDeviceNameText());

            Step(" - Local time at bottom right: value: format 'HH:mm:ss'");
            var localTime = realtimeControlPage.ControllerWidgetPanel.GetLastUpdateTimeText();
            VerifyTrue("5. Verify 'Local time' value format is 'HH:mm:ss'", Regex.IsMatch(localTime, @"\d{2}:\d{2}:\d{2}"), "HH:mm:ss", localTime);
        }

        [Test, DynamicRetry]
        [Description("RTC_02 Control Widget - Controller - Refresh")]
        public void RTC_02()
        {
            var testData = GetTestDataOfRTC_02();
            var xmlController = testData["Controller"].ToString();
            var controllerName = xmlController.GetChildName();

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.RealTimeControl);

            Step("1. Go to Real-time Control app");
            Step("2. Expected Real-time Control page is routed and loaded successfully");
            var realtimeControlPage = desktopPage.GoToApp(App.RealTimeControl) as RealTimeControlPage;

            Step("3. Select a controller from geozone tree");
            realtimeControlPage.GeozoneTreeMainPanel.SelectNode(xmlController);

            Step("4. Verify Control widget for controller appears");
            realtimeControlPage.WaitForControllerWidgetDisplayed(controllerName);
            VerifyEqual("4. Verify Control widget for controller appears", true, realtimeControlPage.IsControllerWidgetDisplayed());

            Step("5. Click Refresh button");
            var localTime = realtimeControlPage.ControllerWidgetPanel.GetLastUpdateTimeText();
            var serverTime = realtimeControlPage.ControllerWidgetPanel.GetSystemTimeValueText();
            var indicatorPosition = realtimeControlPage.ControllerWidgetPanel.GetIndicatorCursorPositionValue();
            var feedback = realtimeControlPage.ControllerWidgetPanel.GetRealtimeFeedbackValueText();
            var command = realtimeControlPage.ControllerWidgetPanel.GetRealtimeCommandValueText();
            var latitude = realtimeControlPage.ControllerWidgetPanel.GetLatitubeValue();
            var longitude = realtimeControlPage.ControllerWidgetPanel.GetLongitubeValue();
            var lastTimeDataSent = realtimeControlPage.ControllerWidgetPanel.GetGpsLastDataSentValueText();

            realtimeControlPage.ControllerWidgetPanel.ClickRefreshButton();
            realtimeControlPage.ControllerWidgetPanel.WaitForCommandCompleted();

            Step("6. Verify Values of Controller's time and local time change");
            var updatedLocalTime = realtimeControlPage.ControllerWidgetPanel.GetLastUpdateTimeText();
            var updatedServerTime = realtimeControlPage.ControllerWidgetPanel.GetSystemTimeValueText();
            VerifyTrue("6. Verify Values of Controller's time changed", serverTime != updatedServerTime, serverTime, updatedServerTime);
            VerifyTrue("6. Verify Values of local time changed", localTime != updatedLocalTime, localTime, updatedLocalTime);

            Step("7. Verify Values of other fields remain unchanged: Indicator position, Feedback, Command, Latitude, Longitude, last time data sent");
            var updatedIndicatorPosition = realtimeControlPage.ControllerWidgetPanel.GetIndicatorCursorPositionValue();
            var updatedFeedback = realtimeControlPage.ControllerWidgetPanel.GetRealtimeFeedbackValueText();
            var updatedCommand = realtimeControlPage.ControllerWidgetPanel.GetRealtimeCommandValueText();
            var updatedLatitude = realtimeControlPage.ControllerWidgetPanel.GetLatitubeValue();
            var updatedLongitude = realtimeControlPage.ControllerWidgetPanel.GetLongitubeValue();
            var updatedLastTimeDataSent = realtimeControlPage.ControllerWidgetPanel.GetGpsLastDataSentValueText();
            VerifyTrue("7. Verify Values of Indicator position unchanged", indicatorPosition == updatedIndicatorPosition, indicatorPosition, updatedIndicatorPosition);
            VerifyTrue("7. Verify Values of Feedback unchanged", feedback == updatedFeedback, feedback, updatedFeedback);
            VerifyTrue("7. Verify Values of Command unchanged", command == updatedCommand, command, updatedCommand);
            VerifyTrue("7. Verify Values of Latitude unchanged", latitude == updatedLatitude, latitude, updatedLatitude);
            VerifyTrue("7. Verify Values of Longitude unchanged", longitude == updatedLongitude, longitude, updatedLongitude);
            VerifyTrue("7. Verify Values of last time data sent unchanged", lastTimeDataSent == updatedLastTimeDataSent, lastTimeDataSent, updatedLastTimeDataSent);            

            Step("8. Click Refresh button");
            realtimeControlPage.ControllerWidgetPanel.ClickRefreshButton();
            realtimeControlPage.WaitForPreviousActionComplete();

            Step("9. Verify Values of Controller's time and local time change");
            var lastRefeshLocalTime = realtimeControlPage.ControllerWidgetPanel.GetLastUpdateTimeText();
            var lastRefeshServerTime = realtimeControlPage.ControllerWidgetPanel.GetSystemTimeValueText();
            VerifyTrue("9. Verify Values of Controller's time changed", lastRefeshLocalTime != updatedServerTime, updatedServerTime, lastRefeshLocalTime);
            VerifyTrue("9. Verify Values of local time changed", lastRefeshServerTime != updatedLocalTime, updatedLocalTime, lastRefeshServerTime);

            Step("10. Verify Values of other fields remains unchanged: Indicator position, Feedback, Command, Latitude, Longitude, Last time SC sent data");
            var lastRefeshIndicatorPosition = realtimeControlPage.ControllerWidgetPanel.GetIndicatorCursorPositionValue();
            var lastRefeshFeedback = realtimeControlPage.ControllerWidgetPanel.GetRealtimeFeedbackValueText();
            var lastRefeshCommand = realtimeControlPage.ControllerWidgetPanel.GetRealtimeCommandValueText();
            var lastRefeshLatitude = realtimeControlPage.ControllerWidgetPanel.GetLatitubeValue();
            var lastRefeshLongitude = realtimeControlPage.ControllerWidgetPanel.GetLongitubeValue();
            var lastRefeshLastTimeDataSent = realtimeControlPage.ControllerWidgetPanel.GetGpsLastDataSentValueText();
            VerifyTrue("10. Verify Values of Indicator position unchanged", updatedIndicatorPosition == lastRefeshIndicatorPosition, updatedIndicatorPosition, lastRefeshIndicatorPosition);
            VerifyTrue("10. Verify Values of Feedback unchanged", updatedFeedback == lastRefeshFeedback, updatedFeedback, lastRefeshFeedback);
            VerifyTrue("10. Verify Values of Command unchanged", updatedCommand == lastRefeshCommand, updatedCommand, lastRefeshCommand);
            VerifyTrue("10. Verify Values of Latitude unchanged", lastRefeshLatitude == updatedLatitude, updatedLatitude, lastRefeshLatitude);
            VerifyTrue("10. Verify Values of Longitude unchanged", lastRefeshLongitude == updatedLongitude, updatedLongitude, lastRefeshLongitude);
            VerifyTrue("10. Verify Values of last time data sent unchanged", lastRefeshLastTimeDataSent == updatedLastTimeDataSent, updatedLastTimeDataSent, lastRefeshLastTimeDataSent);
        }

        [Test, DynamicRetry]
        [Description("RTC_03 Control Widget - Controller - Close")]
        public void RTC_03()
        {
            var testData = GetTestDataOfRTC_03();
            var xmlController = testData["Controller"].ToString();
            var controllerName = xmlController.GetChildName();

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.RealTimeControl);

            Step("1. Go to Real-time Control app");
            Step("2. Expected Real-time Control page is routed and loaded successfully");
            var realtimeControlPage = desktopPage.GoToApp(App.RealTimeControl) as RealTimeControlPage;

            Step("3. Select a controller from geozone tree");
            realtimeControlPage.GeozoneTreeMainPanel.SelectNode(xmlController);

            Step("4. Verify Control widget for controller appears");
            realtimeControlPage.WaitForControllerWidgetDisplayed(controllerName);
            VerifyEqual("4. Verify Control widget for controller appears", true, realtimeControlPage.IsControllerWidgetDisplayed());

            Step("5. Click Close button");
            realtimeControlPage.ControllerWidgetPanel.ClickCloseButton();
            realtimeControlPage.WaitForControllerWidgetDisappeared();

            Step("6. Verify The widget disappears");
            VerifyEqual("6. Verify Control widget for controller disappears", false, realtimeControlPage.IsControllerWidgetDisplayed());

            Step("7. Select the controller from geozone tree again");
            realtimeControlPage.GeozoneTreeMainPanel.SelectNode(controllerName);

            Step("8. Verify Control widget for controller appears back");
            realtimeControlPage.WaitForControllerWidgetDisplayed(controllerName);
            VerifyEqual("8. Verify Control widget for controller appears back", true, realtimeControlPage.IsControllerWidgetDisplayed());
        }

        [Test, DynamicRetry]
        [Description("RTC_04 Control Widget - Controller - INFORMATION - UI")]
        public void RTC_04()
        {
            var testData = GetTestDataOfRTC_04();
            var xmlController = testData["Controller"].ToString();
            var controllerName = xmlController.GetChildName();
            var expectedDimmingGroups = testData["DimmingGroups"] as List<string>;
            expectedDimmingGroups.Sort();

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.RealTimeControl);

            Step("1. Go to Real-time Control app");
            Step("2. Expected Real-time Control page is routed and loaded successfully");
            var realtimeControlPage = desktopPage.GoToApp(App.RealTimeControl) as RealTimeControlPage;

            Step("3. Select a controller from geozone tree");
            realtimeControlPage.GeozoneTreeMainPanel.SelectNode(xmlController);

            Step("4. Verify Control widget for controller appears");
            realtimeControlPage.WaitForControllerWidgetDisplayed(controllerName);
            VerifyEqual("4. Verify Control widget for controller appears", true, realtimeControlPage.IsControllerWidgetDisplayed());

            Step("5. Click INFORMATION (right arrow) button");
            var expectedCommands = realtimeControlPage.ControllerWidgetPanel.GetListOfCommandsText();
            expectedCommands.Remove("ON");
            expectedCommands.Remove("OFF");
            realtimeControlPage.ControllerWidgetPanel.ClickInformationButton();
            realtimeControlPage.ControllerWidgetPanel.WaitForInformationWidgetPanelDisplayed();

            Step("6. Verify The widget turns into INFORMATION view:");
            Step(" - Controller's gateway host name at top left");
            Step(" - 'Group Control' label");
            Step(" - 'Select a group' label");
            Step(" - Dimming group dropdown list: contains dimming groups of all devices whose controller is the selected one");
            Step(" - 'Select a command' label");
            Step(" - Dimming command dropdown list: equals dimming levels of the first view and ones: 'Dim to 90%', 'Switch OFF', 'Switch ON'");
            Step(" - Execute button");
            Step(" - Controller icon and name at bottom left");

            VerifyEqual("6. Verify Controller's gateway host is visible", true, realtimeControlPage.ControllerWidgetPanel.InformationWidgetPanel.IsHostNameDisplayed());
            VerifyEqual("6. Verify label is 'Group Control'", "Group Control", realtimeControlPage.ControllerWidgetPanel.InformationWidgetPanel.GetGroupControlText());
            VerifyEqual("6. Verify label is 'Select a group'", "Select a group", realtimeControlPage.ControllerWidgetPanel.InformationWidgetPanel.GetGroupControlSelectAGroupText());
            var actualDimmingGroups = realtimeControlPage.ControllerWidgetPanel.InformationWidgetPanel.GetDimmingGroupItems();
            actualDimmingGroups.Sort();
            VerifyTrue("6. Verify Dimming group dropdown list contains dimming groups of all devices whose controller is the selected one", actualDimmingGroups.CheckIfIncluded(expectedDimmingGroups), expectedDimmingGroups, actualDimmingGroups);
            VerifyEqual("6. Verify label is 'Select a command'", "Select a command", realtimeControlPage.ControllerWidgetPanel.InformationWidgetPanel.GetGroupControlSelectACommandText());
            var actualCommands = realtimeControlPage.ControllerWidgetPanel.InformationWidgetPanel.GetCommandItems();
            VerifyEqual("6. Verify Dimming command dropdown list equals dimming levels of the first view", true, actualCommands.CheckIfIncluded(expectedCommands));
            VerifyEqual("6. Verify Dimming command dropdown list has '90%', 'Switch OFF', 'Switch ON'", true, actualCommands.Contains("90%") && actualCommands.Contains("Switch OFF") && actualCommands.Contains("Switch ON"));
            VerifyEqual("6. Verify Execute is visible", true, realtimeControlPage.ControllerWidgetPanel.InformationWidgetPanel.IsExecuteButtonDisplayed());
            VerifyEqual("6. Verify Controller icon at bottom left", true, realtimeControlPage.ControllerWidgetPanel.InformationWidgetPanel.CheckIfDeviceIcon(DeviceType.Controller));
            VerifyEqual("6. Verify name at bottom left", controllerName, realtimeControlPage.ControllerWidgetPanel.InformationWidgetPanel.GetDeviceNameText());

            Step("7. Click on empty area of the widget");
            realtimeControlPage.ControllerWidgetPanel.InformationWidgetPanel.ClickDeviceIcon();
            realtimeControlPage.ControllerWidgetPanel.WaitInformationWidgetPanelDisappeared();

            Step("8. Verify The first view is returned");
            VerifyEqual("8. Verify INFORMATION button is visible", true, realtimeControlPage.ControllerWidgetPanel.IsInformationButtonVisible());
            VerifyEqual("8. Verify Controller's gateway host is invisible", false, realtimeControlPage.ControllerWidgetPanel.InformationWidgetPanel.IsHostNameDisplayed());
        }        

        [Test, DynamicRetry]
        [Description("RTC_05 Control Widget - Controller - Execute global dimming commands")]
        [NonParallelizable]
        public void RTC_05()
        {
            var testData = GetTestDataOfRTC_05();
            var xmlController = testData["Controller"].ToString();
            var controllerName = xmlController.GetChildName();
            var parentGeozone = xmlController.GetParentName();
            var streetlights = testData["Streetlights"] as List<DeviceModel>;
            streetlights = streetlights.Where(p => p.Controller.Equals(controllerName)).ToList();

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.RealTimeControl);

            Step("1. Go to Real-time Control app");
            Step("2. Expected Real-time Control page is routed and loaded successfully");
            var realtimeControlPage = desktopPage.GoToApp(App.RealTimeControl) as RealTimeControlPage;

            Step("3. Select a controller from geozone tree");
            realtimeControlPage.GeozoneTreeMainPanel.SelectNode(xmlController);

            Step("4. Verify Control widget for controller appears");
            realtimeControlPage.WaitForControllerWidgetDisplayed(controllerName);
            VerifyEqual("4. Verify Control widget for controller appears", true, realtimeControlPage.IsControllerWidgetDisplayed());
            var listOfCommandsText = realtimeControlPage.ControllerWidgetPanel.GetListOfCommandsText();
            listOfCommandsText.Remove("OFF");

            Step("5. Click OFF dimming command");
            var command = RealtimeCommand.DimOff;
            realtimeControlPage.ControllerWidgetPanel.ExecuteCommand(command);
            realtimeControlPage.ControllerWidgetPanel.WaitForCommandCompleted();

            Step("6. Select the parent geozone from geozone tree");
            realtimeControlPage.GeozoneTreeMainPanel.SelectNode(parentGeozone);
            realtimeControlPage.WaitForControllerWidgetDisappeared();

            Step("7. Click \"Refresh all devices displayed on the map\" button");
            realtimeControlPage.Map.MoveToGlobalEarthIcon();
            realtimeControlPage.Map.WaitForRealtimeRefreshPanelDisplayed();
            realtimeControlPage.Map.ClickRealtimeRefreshButton();
            realtimeControlPage.Map.WaitForProgressGLCompleted();

            Step("8. Hover mouse onto each streetlight controlled by the controller");
            Step("9. Verify Tooltip of each streetlight displays:");
            Step(" - Streetlight's name");
            Step(" - 'Status': 'OK'");
            Step(" - 'Mode': 'Manual'");
            Step(" - 'Level': '0%'");
            foreach (var streetlight in streetlights)
            {
                realtimeControlPage.Map.MoveToDeviceGL(streetlight.Longitude, streetlight.Latitude);
                var name = realtimeControlPage.Map.GetDeviceNameGL();
                var status = realtimeControlPage.Map.GetDeviceStatusGL();
                var mode = realtimeControlPage.Map.GetDeviceModeGL();
                var level = realtimeControlPage.Map.GetDeviceLevelGL();

                VerifyEqual(string.Format("9. Verify Streetlight's name is {0}", streetlight.Name), streetlight.Name, name);
                VerifyEqual("9. Verify Status is OK", "OK", status);
                VerifyEqual("9. Verify Mode is Manual", "Manual", mode);
                VerifyEqual("9. Verify Level is 0%", "0%", level);
            }

            Step("10. Repeat steps from #3 to #11 with other dimming levels and with using Triangle indicator");
            Step("11. Verify After \"refresh all devices displayed on the map\", tooltip of each streetlight displays:");
            Step(" - Streetlight's name");
            Step(" - 'Status': 'OK'");
            Step(" - 'Mode': 'Manual'");
            Step(" - 'Level': '{dimming-level-value}%' ('100%' in case 'ON' level is clicked)");
            foreach (var commandText in listOfCommandsText)
            {
                realtimeControlPage.GeozoneTreeMainPanel.SelectNode(controllerName);
                realtimeControlPage.WaitForControllerWidgetDisplayed(controllerName);
                command = realtimeControlPage.ControllerWidgetPanel.GetCommandByText(commandText);
                var commandData = realtimeControlPage.ControllerWidgetPanel.CommandsDict[command];
                realtimeControlPage.ControllerWidgetPanel.ExecuteCommand(command);
                realtimeControlPage.ControllerWidgetPanel.WaitForCommandCompleted();

                realtimeControlPage.GeozoneTreeMainPanel.SelectNode(parentGeozone);
                realtimeControlPage.WaitForControllerWidgetDisappeared();

                realtimeControlPage.Map.MoveToGlobalEarthIcon();
                realtimeControlPage.Map.WaitForRealtimeRefreshPanelDisplayed();
                realtimeControlPage.Map.ClickRealtimeRefreshButton();
                realtimeControlPage.Map.WaitForProgressGLCompleted();

                foreach (var streetlight in streetlights)
                {
                    realtimeControlPage.Map.MoveToDeviceGL(streetlight.Longitude, streetlight.Latitude);
                    var name = realtimeControlPage.Map.GetDeviceNameGL();
                    var status = realtimeControlPage.Map.GetDeviceStatusGL();
                    var mode = realtimeControlPage.Map.GetDeviceModeGL();
                    var level = realtimeControlPage.Map.GetDeviceLevelGL();

                    VerifyEqual(string.Format("[0] 11. Verify Streetlight's name is {1}", commandText, streetlight.Name), streetlight.Name, name);
                    VerifyEqual(string.Format("[0] 11. Verify Status is OK", commandText), "OK", status);
                    VerifyEqual(string.Format("[0] 11. Verify Mode is Manual", commandText), "Manual", mode);
                    VerifyEqual(string.Format("[0] 11. Verify Level is {1}", commandText, commandData.ValueText), commandData.ValueText, level);
                }
            }
        }

        [Test, DynamicRetry]
        [Description("RTC_06 Control Widget - Controller - Execute group dimming commands")]
        [NonParallelizable]
        public void RTC_06()
        {
            var testData = GetTestDataOfRTC_06();
            var xmlController = testData["Controller"].ToString();
            var controllerName = xmlController.GetChildName();
            var parentGeozone = xmlController.GetParentName();
            var streetlights = testData["Streetlights"] as List<DeviceModel>;
            streetlights = streetlights.Where(p => p.Controller.Equals(controllerName)).ToList();
            var dimmingGroups = streetlights.Select(p => p.DimmingGroup).Distinct().ToList();

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.RealTimeControl);

            Step("1. Go to Real-time Control app");
            Step("2. Expected Real-time Control page is routed and loaded successfully");
            var realtimeControlPage = desktopPage.GoToApp(App.RealTimeControl) as RealTimeControlPage;

            Step("3. Select a controller from geozone tree");
            realtimeControlPage.GeozoneTreeMainPanel.SelectNode(xmlController);

            Step("4. Verify Control widget for controller appears");
            realtimeControlPage.WaitForControllerWidgetDisplayed(controllerName);
            VerifyEqual("4. Verify Control widget for controller appears", true, realtimeControlPage.IsControllerWidgetDisplayed());
            var listOfCommandsText = realtimeControlPage.ControllerWidgetPanel.GetListOfCommandsText();
            listOfCommandsText.Remove("OFF");

            Step("5. Click AUTOMATIC button");
            realtimeControlPage.ControllerWidgetPanel.ClickWholeSegmentAutomaticButton();
            realtimeControlPage.ControllerWidgetPanel.WaitForCommandCompleted();

            Step("6. Select the parent geozone from geozone tree");
            realtimeControlPage.GeozoneTreeMainPanel.SelectNode(parentGeozone);
            realtimeControlPage.WaitForControllerWidgetDisappeared();

            Step("7. Click \"Refresh all devices displayed on the map\" button");
            realtimeControlPage.Map.MoveToGlobalEarthIcon();
            realtimeControlPage.Map.WaitForRealtimeRefreshPanelDisplayed();
            realtimeControlPage.Map.ClickRealtimeRefreshButton();
            realtimeControlPage.Map.WaitForProgressGLCompleted();

            Step("8. Hover mouse onto each streetlight controlled by the controller");
            Step("9. Verify Tooltip of each streetlight displays:");
            Step(" - Streetlight's name");
            Step(" - 'Status': 'OK'");
            Step(" - 'Mode': 'Automatic'");
            Step(" - 'Level': '100%'");
            foreach (var streetlight in streetlights)
            {
                realtimeControlPage.Map.MoveToDeviceGL(streetlight.Longitude, streetlight.Latitude);
                var name = realtimeControlPage.Map.GetDeviceNameGL();
                var status = realtimeControlPage.Map.GetDeviceStatusGL();
                var mode = realtimeControlPage.Map.GetDeviceModeGL();
                var level = realtimeControlPage.Map.GetDeviceLevelGL();
                VerifyEqual(string.Format("9. Verify Streetlight's name is {0}", streetlight.Name), streetlight.Name, name);
                VerifyEqual("9. Verify Status is OK", "OK", status);
                VerifyEqual("9. Verify Mode is Automatic", "Automatic", mode);
                VerifyEqual("9. Verify Level is 100%", "100%", level);
            }

            Step("10. Select the controller from geozone tree again");
            realtimeControlPage.GeozoneTreeMainPanel.SelectNode(controllerName);

            Step("11. Verify Control widget for controller appears back");
            realtimeControlPage.WaitForControllerWidgetDisplayed(controllerName);
            VerifyEqual("12. Verify Control widget for controller appears", true, realtimeControlPage.IsControllerWidgetDisplayed());

            Step("12. Click INFORMATION button");
            realtimeControlPage.ControllerWidgetPanel.ClickInformationButton();
            realtimeControlPage.ControllerWidgetPanel.WaitForInformationWidgetPanelDisplayed();

            Step("13. Verify INFORMATION view is turned to");
            VerifyEqual("14. Verify INFORMATION button is invisible", false, realtimeControlPage.ControllerWidgetPanel.IsInformationButtonVisible());

            Step("14. Select a dimming group and a random dimming command then click Execute button");
            var dimmingLevels = realtimeControlPage.ControllerWidgetPanel.InformationWidgetPanel.GetCommandItems();
            dimmingLevels.Remove("Reset back to automatic mode");
            var dimmingGroup = dimmingGroups.PickRandom();
            dimmingGroups.Remove(dimmingGroup);
            realtimeControlPage.ControllerWidgetPanel.InformationWidgetPanel.SelectSelectGroupDropDown(dimmingGroup);
            var rndLevel = dimmingLevels.PickRandom();
            var command = GetCommandByText(realtimeControlPage, rndLevel);
            var commandData = realtimeControlPage.ControllerWidgetPanel.CommandsDict[command];          
            realtimeControlPage.ControllerWidgetPanel.InformationWidgetPanel.SelectSelecCommandDropDown(rndLevel);
            realtimeControlPage.ControllerWidgetPanel.InformationWidgetPanel.ClickExecuteCommandButton();
            realtimeControlPage.ControllerWidgetPanel.WaitForCommandCompleted();
            
            Step("15. Select the parent geozone from geozone tree");
            realtimeControlPage.GeozoneTreeMainPanel.SelectNode(parentGeozone);
            realtimeControlPage.WaitForControllerWidgetDisappeared();

            Step("16. Click \"Refresh all devices displayed on the map\" button");
            realtimeControlPage.Map.MoveToGlobalEarthIcon();
            realtimeControlPage.Map.WaitForRealtimeRefreshPanelDisplayed();
            realtimeControlPage.Map.ClickRealtimeRefreshButton();
            realtimeControlPage.Map.WaitForProgressGLCompleted();

            Step("17. Hover mouse onto each streetlight controlled by the controller and having dimming group which is the selected one from INFORMATION view");
            Step("18. Tooltip of these streetlights displays:");
            Step(" - Streetlight's name");
            Step(" - 'Status': 'OK'");
            Step(" - 'Mode': 'Manual'");
            Step(" - 'Level': '{selected-dimming-level-value}%'");

            var streetlightsWithSelectedGroup = streetlights.Where(p => p.DimmingGroup.Equals(dimmingGroup));
            foreach (var streetlight in streetlightsWithSelectedGroup)
            {
                realtimeControlPage.Map.MoveToDeviceGL(streetlight.Longitude, streetlight.Latitude);
                var name = realtimeControlPage.Map.GetDeviceNameGL();
                var status = realtimeControlPage.Map.GetDeviceStatusGL();
                var mode = realtimeControlPage.Map.GetDeviceModeGL();
                var level = realtimeControlPage.Map.GetDeviceLevelGL();

                VerifyEqual(string.Format("18. Verify Streetlight's name is {0}", streetlight.Name), streetlight.Name, name);
                VerifyEqual(string.Format("[{0}] 18. Verify Status is Ok", streetlight.Name), "OK", status);
                VerifyEqual(string.Format("[{0}] 18. Verify Mode is Manual", streetlight.Name), "Manual", mode);
                VerifyEqual(string.Format("18. Verify Level is {0}", commandData.ValueText), commandData.ValueText, level);
            }

            Step("19. Tooltip of other streetlights displays:");
            Step(" - Streetlight's name");
            Step(" - 'Status': 'OK'");
            Step(" - 'Mode': 'Automatic'");
            Step(" - 'Level': '100%'");
            var streetlightsWithNotSelectedGroup = streetlights.Where(p => !p.DimmingGroup.Equals(dimmingGroup));
            foreach (var streetlight in streetlightsWithNotSelectedGroup)
            {
                realtimeControlPage.Map.MoveToDeviceGL(streetlight.Longitude, streetlight.Latitude);
                var name = realtimeControlPage.Map.GetDeviceNameGL();
                var status = realtimeControlPage.Map.GetDeviceStatusGL();
                var mode = realtimeControlPage.Map.GetDeviceModeGL();
                var level = realtimeControlPage.Map.GetDeviceLevelGL();

                VerifyEqual(string.Format("19. Verify Streetlight's name is {0}", streetlight.Name), streetlight.Name, name);
                VerifyEqual("19. Verify Status is OK", "OK", status);
                VerifyEqual("19. Verify Mode is Automatic", "Automatic", mode);
                VerifyEqual("19. Verify Level is 100%", "100%", level);
            }

            Step("20. Repeat from step #3 to #19. This time, choose another dimming group at step #14");
            for (int i = 0; i < dimmingGroups.Count; i++)
            {
                dimmingGroup = dimmingGroups[i];

                Step("20.3. Select a controller from geozone tree");
                realtimeControlPage.GeozoneTreeMainPanel.SelectNode(controllerName);
                Step("20.4. Verify Control widget for controller appears");
                realtimeControlPage.WaitForControllerWidgetDisplayed(controllerName);
                VerifyEqual("Verify Control widget for controller appears", true, realtimeControlPage.IsControllerWidgetDisplayed());

                Step("20.5. Click AUTOMATIC button");
                realtimeControlPage.ControllerWidgetPanel.ClickWholeSegmentAutomaticButton();
                realtimeControlPage.ControllerWidgetPanel.WaitForCommandCompleted();

                Step("20.6. Select the parent geozone from geozone tree");
                realtimeControlPage.GeozoneTreeMainPanel.SelectNode(parentGeozone);
                realtimeControlPage.WaitForControllerWidgetDisappeared();

                Step("20.7. Click \"Refresh all devices displayed on the map\" button");
                realtimeControlPage.Map.MoveToGlobalEarthIcon();
                realtimeControlPage.Map.WaitForRealtimeRefreshPanelDisplayed();
                realtimeControlPage.Map.ClickRealtimeRefreshButton();
                realtimeControlPage.Map.WaitForProgressGLCompleted();

                Step("20.8. Hover mouse onto each streetlight controlled by the controller");
                Step("20.9. Verify Tooltip of each streetlight displays:");
                Step(" - Streetlight's name");
                Step(" - 'Status': 'OK'");
                Step(" - 'Mode': 'Automatic'");
                Step(" - 'Level': '100%'");
                foreach (var streetlight in streetlights)
                {
                    realtimeControlPage.Map.MoveToDeviceGL(streetlight.Longitude, streetlight.Latitude);
                    var name = realtimeControlPage.Map.GetDeviceNameGL();
                    var status = realtimeControlPage.Map.GetDeviceStatusGL();
                    var mode = realtimeControlPage.Map.GetDeviceModeGL();
                    var level = realtimeControlPage.Map.GetDeviceLevelGL();

                    VerifyEqual(string.Format("20.9. Verify Streetlight's name is {0}", streetlight.Name), streetlight.Name, name);
                    VerifyEqual("20.9. Verify Status is OK", "OK", status);
                    VerifyEqual("20.9. Verify Mode is Automatic", "Automatic", mode);
                    VerifyEqual("20.9. Verify Level is 100%", "100%", level);
                }

                Step("20.10. Select the controller from geozone tree again");
                realtimeControlPage.GeozoneTreeMainPanel.SelectNode(controllerName);

                Step("20.11. Verify Control widget for controller appears back");
                realtimeControlPage.WaitForControllerWidgetDisplayed(controllerName);
                VerifyEqual("20.11. Verify Control widget for controller appears", true, realtimeControlPage.IsControllerWidgetDisplayed());

                Step("20.12. Click INFORMATION button");
                realtimeControlPage.ControllerWidgetPanel.ClickInformationButton();
                realtimeControlPage.ControllerWidgetPanel.WaitForInformationWidgetPanelDisplayed();

                Step("20.13. Verify INFORMATION view is turned to");
                VerifyEqual("20.13. Verify INFORMATION button is invisible", false, realtimeControlPage.ControllerWidgetPanel.IsInformationButtonVisible());

                Step("20.14. Select a dimming group and a random dimming command then click Execute button");
                realtimeControlPage.ControllerWidgetPanel.InformationWidgetPanel.SelectSelectGroupDropDown(dimmingGroup);
                rndLevel = dimmingLevels.PickRandom();
                command = GetCommandByText(realtimeControlPage, rndLevel);
                commandData = realtimeControlPage.ControllerWidgetPanel.CommandsDict[command];
                realtimeControlPage.ControllerWidgetPanel.InformationWidgetPanel.SelectSelecCommandDropDown(rndLevel);
                realtimeControlPage.ControllerWidgetPanel.InformationWidgetPanel.ClickExecuteCommandButton();
                realtimeControlPage.ControllerWidgetPanel.WaitForCommandCompleted();
                
                Step("20.15. Select the parent geozone from geozone tree");
                realtimeControlPage.GeozoneTreeMainPanel.SelectNode(parentGeozone);
                realtimeControlPage.WaitForControllerWidgetDisappeared();

                Step("20.16. Click \"Refresh all devices displayed on the map\" button");
                realtimeControlPage.Map.MoveToGlobalEarthIcon();
                realtimeControlPage.Map.WaitForRealtimeRefreshPanelDisplayed();
                realtimeControlPage.Map.ClickRealtimeRefreshButton();
                realtimeControlPage.Map.WaitForProgressGLCompleted();

                Step("20.17. Hover mouse onto each streetlight controlled by the controller and having dimming group which is the selected one from INFORMATION view");
                Step("20.18. Tooltip of these streetlights displays:");
                Step(" - Streetlight's name");
                Step(" - 'Status': 'OK'");
                Step(" - 'Mode': 'Manual'");
                Step(" - 'Level': '{selected-dimming-level-value}%'");

                streetlightsWithSelectedGroup = streetlights.Where(p => p.DimmingGroup.Equals(dimmingGroup));
                foreach (var streetlight in streetlightsWithSelectedGroup)
                {
                    realtimeControlPage.Map.MoveToDeviceGL(streetlight.Longitude, streetlight.Latitude);
                    var name = realtimeControlPage.Map.GetDeviceNameGL();
                    var status = realtimeControlPage.Map.GetDeviceStatusGL();
                    var mode = realtimeControlPage.Map.GetDeviceModeGL();
                    var level = realtimeControlPage.Map.GetDeviceLevelGL();

                    VerifyEqual(string.Format("20.18. Verify Streetlight's name is {0}", streetlight.Name), streetlight.Name, name);
                    VerifyEqual(string.Format("[{0}] 20.18. Verify Status is Ok", streetlight.Name), "OK", status);
                    VerifyEqual(string.Format("[{0}] 20.18. Verify Mode is Manual", streetlight.Name), "Manual", mode);
                    VerifyEqual(string.Format("20.18. Verify Level is {0}", commandData.ValueText), commandData.ValueText, level);
                }

                Step("20.19. Tooltip of other streetlights displays:");
                Step(" - Streetlight's name");
                Step(" - 'Status': 'OK'");
                Step(" - 'Mode': 'Automatic'");
                Step(" - 'Level': '100%'");
                streetlightsWithNotSelectedGroup = streetlights.Where(p => !p.DimmingGroup.Equals(dimmingGroup));
                foreach (var streetlight in streetlightsWithNotSelectedGroup)
                {
                    realtimeControlPage.Map.MoveToDeviceGL(streetlight.Longitude, streetlight.Latitude);
                    var name = realtimeControlPage.Map.GetDeviceNameGL();
                    var status = realtimeControlPage.Map.GetDeviceStatusGL();
                    var mode = realtimeControlPage.Map.GetDeviceModeGL();
                    var level = realtimeControlPage.Map.GetDeviceLevelGL();

                    VerifyEqual(string.Format("20.19. Verify Streetlight's name is {0}", streetlight.Name), streetlight.Name, name);
                    VerifyEqual("20.19. Verify Status is OK", "OK", status);
                    VerifyEqual("20.19. Verify Mode is Automatic", "Automatic", mode);
                    VerifyEqual("20.19. Verify Level is 100%", "100%", level);
                }
            }
        }

        [Test, DynamicRetry]
        [Description("RTC_07 Control Widget - Controller - Exit manual mode")]
        [NonParallelizable]
        public void RTC_07()
        {
            var testData = GetTestDataOfRTC_07();
            var xmlController = testData["Controller"].ToString();
            var controllerName = xmlController.GetChildName();
            var parentGeozone = xmlController.GetParentName();
            var streetlights = testData["Streetlights"] as List<DeviceModel>;
            streetlights = streetlights.Where(p => p.Controller.Equals(controllerName)).ToList();
            var dimmingGroups = streetlights.Select(p => p.DimmingGroup).Distinct().ToList();

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.RealTimeControl);

            Step("1. Go to Real-time Control app");
            Step("2. Expected Real-time Control page is routed and loaded successfully");
            var realtimeControlPage = desktopPage.GoToApp(App.RealTimeControl) as RealTimeControlPage;

            Step("3. Select a controller from geozone tree");
            realtimeControlPage.GeozoneTreeMainPanel.SelectNode(xmlController);

            Step("4. Verify Control widget for controller appears");
            realtimeControlPage.WaitForControllerWidgetDisplayed(controllerName);
            VerifyEqual("4. Verify Control widget for controller appears", true, realtimeControlPage.IsControllerWidgetDisplayed());
            var listOfCommandsText = realtimeControlPage.ControllerWidgetPanel.GetListOfCommandsText();

            Step("5. Execute a dimming command");
            var commandText = listOfCommandsText.PickRandom();
            var command = GetCommandByText(realtimeControlPage, commandText);
            var commandData = realtimeControlPage.ControllerWidgetPanel.CommandsDict[command];
            realtimeControlPage.ControllerWidgetPanel.ExecuteCommand(command);
            realtimeControlPage.ControllerWidgetPanel.WaitForCommandCompleted();

            Step("6. Select the parent geozone from geozone tree");
            realtimeControlPage.GeozoneTreeMainPanel.SelectNode(parentGeozone);
            realtimeControlPage.WaitForControllerWidgetDisappeared();

            Step("7. Click \"Refresh all devices displayed on the map\" button");
            realtimeControlPage.Map.MoveToGlobalEarthIcon();
            realtimeControlPage.Map.WaitForRealtimeRefreshPanelDisplayed();
            realtimeControlPage.Map.ClickRealtimeRefreshButton();
            realtimeControlPage.Map.WaitForProgressGLCompleted();

            Step("8. Hover mouse onto each streetlight controlled by the controller");
            Step("9. Verify Tooltip of each streetlight displays:");
            Step(" - Streetlight's name");
            Step(" - 'Status': 'OK'");
            Step(" - 'Mode': 'Manual'");
            Step(" - 'Level': '{dimming-level} %'");
            foreach (var streetlight in streetlights)
            {
                realtimeControlPage.Map.MoveToDeviceGL(streetlight.Longitude, streetlight.Latitude);
                var name = realtimeControlPage.Map.GetDeviceNameGL();
                var status = realtimeControlPage.Map.GetDeviceStatusGL();
                var mode = realtimeControlPage.Map.GetDeviceModeGL();
                var level = realtimeControlPage.Map.GetDeviceLevelGL();
                VerifyEqual(string.Format("9. Verify Streetlight's name is {0}", streetlight.Name), streetlight.Name, name);
                VerifyEqual("9. Verify Status is OK", "OK", status);
                VerifyEqual("9. Verify Mode is Manual", "Manual", mode);
                VerifyEqual(string.Format("9. Verify Level is {0}", commandData.ValueText), commandData.ValueText, level);
            }

            Step("10. Select the controller again");
            realtimeControlPage.GeozoneTreeMainPanel.SelectNode(controllerName);

            Step("11. Verify Control widget for controller appears back");
            realtimeControlPage.WaitForControllerWidgetDisplayed(controllerName);
            VerifyEqual("11. Verify Control widget for controller appears", true, realtimeControlPage.IsControllerWidgetDisplayed());

            Step("12. Click AUTOMATIC button");           
            realtimeControlPage.ControllerWidgetPanel.ClickWholeSegmentAutomaticButton();
            realtimeControlPage.ControllerWidgetPanel.WaitForCommandCompleted();

            Step("13. Select the parent geozone from geozone tree");
            realtimeControlPage.GeozoneTreeMainPanel.SelectNode(parentGeozone);
            realtimeControlPage.WaitForControllerWidgetDisappeared();

            Step("14. Click \"Refresh all devices displayed on the map\" button");
            realtimeControlPage.Map.MoveToGlobalEarthIcon();
            realtimeControlPage.Map.WaitForRealtimeRefreshPanelDisplayed();
            realtimeControlPage.Map.ClickRealtimeRefreshButton();
            realtimeControlPage.Map.WaitForProgressGLCompleted();

            Step("15. Hover mouse onto each streetlight controlled by the controller");
            Step("16. Tooltip of these streetlights displays:");
            Step(" - Streetlight's name");
            Step(" - 'Mode': 'Automatic'");
            Step(" - 'Level': '100%'");
            foreach (var streetlight in streetlights)
            {
                realtimeControlPage.Map.MoveToDeviceGL(streetlight.Longitude, streetlight.Latitude);
                var name = realtimeControlPage.Map.GetDeviceNameGL();
                var status = realtimeControlPage.Map.GetDeviceStatusGL();
                var mode = realtimeControlPage.Map.GetDeviceModeGL();
                var level = realtimeControlPage.Map.GetDeviceLevelGL();

                VerifyEqual(string.Format("16. Verify Streetlight's name is {0}", streetlight.Name), streetlight.Name, name);
                VerifyEqual("16. Verify Status is OK", "OK", status);
                VerifyEqual("16. Verify Mode is Automatic", "Automatic", mode);
                VerifyEqual("16. Verify Level is 100%", "100%", level);
            }
        }

        [Test, DynamicRetry]
        [Description("RTC_08 Control Widget - Streetlight - UI")]
        public void RTC_08()
        {
            var testData = GetTestDataOfRTC_08();
            var geozone = testData["Geozone"].ToString();
            var expectedCommands = testData["Commands"] as List<string>;
            var streetlights = testData["Streetlights"] as List<DeviceModel>;
            var streetlight = streetlights.PickRandom();
            var streetlightName = streetlight.Name;

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.RealTimeControl);

            Step("1. Go to Real-time Control app");
            Step("2. Expected Real-time Control page is routed and loaded successfully");
            var realtimeControlPage = desktopPage.GoToApp(App.RealTimeControl) as RealTimeControlPage;

            Step("3. Select a working streetlight from geozone tree");
            realtimeControlPage.GeozoneTreeMainPanel.SelectNode(string.Format(@"{0}\{1}", geozone, streetlightName));

            Step("4. Verify Control widget for streetlight appears");
            realtimeControlPage.WaitForStreetlightWidgetDisplayed(streetlightName);
            VerifyEqual("4. Verify Control widget for streetlight appears", true, realtimeControlPage.IsStreetlightWidgetDisplayed());

            Step("5. Verify Control widget for streetlight has:");
            Step(" - Header bar:");
            Step("   + Refresh button");
            Step("   + 'AUTOMATIC' or 'MANUAL' button with clock icon");
            Step("   + 'Status': icon: question or ok");
            Step("   + Sunrise/Sunset times button");
            Step("   + INFORMATION button");
            Step("   + Close button");
            VerifyEqual("5. Verify Refresh button is visible", true, realtimeControlPage.StreetlightWidgetPanel.IsRefreshButtonVisible());
            var clockText = realtimeControlPage.StreetlightWidgetPanel.GetClockText();
            VerifyEqual("5. Verify Clock button is visible", true, realtimeControlPage.StreetlightWidgetPanel.IsClockButtonVisible());
            VerifyTrue("5. Verify 'AUTOMATIC' or 'MANUAL' button with clock icon", clockText.Equals("AUTOMATIC") || clockText.Equals("MANUAL"), "AUTOMATIC / MANUAL", clockText);
            var statusIconUrl = realtimeControlPage.StreetlightWidgetPanel.GetStatusIconValue();
            VerifyEqual("5. Verify Status label is 'Status'", "Status", realtimeControlPage.StreetlightWidgetPanel.GetStatusText());
            VerifyTrue("5. Verify Status icon is Question or Ok", statusIconUrl.Contains("status-question.png") || statusIconUrl.Contains("status-ok.png"), "status-question.png / status-ok.png", statusIconUrl);
            VerifyEqual("5. Verify Sunrise/Sunset times button is visible", true, realtimeControlPage.StreetlightWidgetPanel.IsSunriseSunsetButtonVisible());
            VerifyEqual("5. Verify INFORMATION button is visible", true, realtimeControlPage.StreetlightWidgetPanel.IsInformationButtonVisible());
            VerifyEqual("5. Verify Close button is visible", true, realtimeControlPage.StreetlightWidgetPanel.IsCloseButtonVisible());

            Step(" - Dimming levels bar:");
            Step("   + ON");
            Step("   + 40%, 50%, 60%, 70%, 80%, 90% (These levels are applied only to controller whose equipment type is 'Telematics LCU')");
            Step("   + OFF");
            Step("   + Indicator (black triangle)");
            var listOfCommandsText = realtimeControlPage.StreetlightWidgetPanel.GetListOfCommandsText();
            VerifyEqual("5. Verify Dimming levels as expected", expectedCommands, listOfCommandsText, false);
            VerifyEqual("5. Verify Indicator (black triangle) is visible", true, realtimeControlPage.StreetlightWidgetPanel.IsIndicatorCursorVisible());

            Step(" - 'Feedback': number + '%'");
            var feedbackValueText = realtimeControlPage.StreetlightWidgetPanel.GetRealtimeFeedbackValueText();
            VerifyEqual("5. Verify label is 'Feedback'", "Feedback", realtimeControlPage.StreetlightWidgetPanel.GetRealtimeFeedbackText());
            VerifyEqual("5. Verify value is matching 'number + %'", true, Regex.IsMatch(feedbackValueText, @"\d{1,}%"));

            Step(" - 'Command': number + '%'");
            var commandValueText = realtimeControlPage.StreetlightWidgetPanel.GetRealtimeCommandValueText();
            VerifyEqual("5. Verify label is 'Command'", "Command", realtimeControlPage.StreetlightWidgetPanel.GetRealtimeCommandText());
            VerifyEqual("5. Verify value is matching 'number + %'", true, Regex.IsMatch(commandValueText, @"\d{1,}%"));

            Step(" - 'Metering':");
            Step("   + 'Lamp burning hours': number + 'h'");
            Step("   + 'Lamp energy': number");
            Step("   + 'Lamp level command': number");
            Step("   + 'Lamp level feedback': number + '%'");
            Step("   + 'Lamp power': number");
            Step("   + 'Lamp switch feedback': number");
            Step("   + 'Mains current': number");
            Step("   + 'Mains voltage (V)': number + 'V'");
            Step("   + 'Node failure message': number");
            Step("   + 'Power factor': number");
            Step("   + 'Temperature': number");
            VerifyWorkingStreetlightMeterings(realtimeControlPage);

            Step(" - Streetlight icon and name at bottom left");
            VerifyEqual("5. Verify Streetlight icon and name at bottom left", true, realtimeControlPage.StreetlightWidgetPanel.CheckIfDeviceIcon(DeviceType.Streetlight));
            VerifyEqual("5. Verify name at bottom left", streetlightName, realtimeControlPage.StreetlightWidgetPanel.GetDeviceNameText());

            Step(" - Local time at bottom right: value: format 'hh:mm:ss'");
            VerifyEqual("5. Verify value format is 'hh:mm:ss'", true, Regex.IsMatch(realtimeControlPage.StreetlightWidgetPanel.GetLastUpdateTimeText(), @"\d{2}:\d{2}:\d{2}"));
        }

        [Test, DynamicRetry]
        [Description("RTC_09 Control Widget - Streetlight - Refresh")]
        public void RTC_09()
        {
            var testData = GetTestDataOfRTC_09();
            var geozone = testData["Geozone"].ToString();
            var streetlights = testData["Streetlights"] as List<DeviceModel>;
            var streetlight = streetlights.PickRandom();
            var streetlightName = streetlight.Name;

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.RealTimeControl);

            Step("1. Go to Real-time Control app");
            Step("2. Expected Real-time Control page is routed and loaded successfully");
            var realtimeControlPage = desktopPage.GoToApp(App.RealTimeControl) as RealTimeControlPage;

            Step("3. Select a working streetlight from geozone tree");
            realtimeControlPage.GeozoneTreeMainPanel.SelectNode(string.Format(@"{0}\{1}", geozone, streetlightName));

            Step("4. Verify Control widget for streetlight appears");
            realtimeControlPage.WaitForStreetlightWidgetDisplayed(streetlightName);
            VerifyEqual("4. Verify Control widget for streetlight appears", true, realtimeControlPage.IsStreetlightWidgetDisplayed());

            var listOfCommandsText = realtimeControlPage.StreetlightWidgetPanel.GetListOfCommandsText();
            var indicatorCursorPosition = realtimeControlPage.StreetlightWidgetPanel.GetIndicatorCursorPositionValue();
            var feedbackValueText = realtimeControlPage.StreetlightWidgetPanel.GetRealtimeFeedbackValueText();
            var commandValueText = realtimeControlPage.StreetlightWidgetPanel.GetRealtimeCommandValueText();
            var lampBurningHours = realtimeControlPage.StreetlightWidgetPanel.GetLampBurningHoursValueText();
            var lampEnergy = realtimeControlPage.StreetlightWidgetPanel.GetLampEnergyValueText();
            var lampLevelCommand = realtimeControlPage.StreetlightWidgetPanel.GetLampLevelCommandValueText();
            var lampLevelFeedback = realtimeControlPage.StreetlightWidgetPanel.GetLampLevelFeedbackValueText();
            var lampPower = realtimeControlPage.StreetlightWidgetPanel.GetLampPowerValueText();
            var lampSwitchFeedback = realtimeControlPage.StreetlightWidgetPanel.GetLampSwitchFeedbackValueText();
            var mainsCurrent = realtimeControlPage.StreetlightWidgetPanel.GetMainsCurrentValueText();
            var mainsVoltage = realtimeControlPage.StreetlightWidgetPanel.GetMainsVoltageValueText();
            var nodeFailureMessage = realtimeControlPage.StreetlightWidgetPanel.GetNodeFailureMessageValueText();
            var powerFactor = realtimeControlPage.StreetlightWidgetPanel.GetPowerFactorValueText();
            var temperature = realtimeControlPage.StreetlightWidgetPanel.GetTemperatureValueText();
            var localTime = realtimeControlPage.StreetlightWidgetPanel.GetLastUpdateTimeText();
            var isStreetlightIcon = realtimeControlPage.StreetlightWidgetPanel.CheckIfDeviceIcon(DeviceType.Streetlight);
            var deviceName = realtimeControlPage.StreetlightWidgetPanel.GetDeviceNameText();

            Step("5. Click Refresh button");
            realtimeControlPage.StreetlightWidgetPanel.ClickRefreshButton();
            realtimeControlPage.StreetlightWidgetPanel.WaitForCommandCompleted();

            Step("6. Verify Values of following values remain unchanged:");
            Step(" - Indicator position");
            Step(" - Feedback");
            Step(" - Command");
            Step(" - Lamp burning hours");
            Step(" - Lamp energy");
            Step(" - Lamp level command");
            Step(" - Lamp level feedback");
            Step(" - Lamp power");
            Step(" - Lamp switch feedback");
            Step(" - Node failure message");
            Step(" - Streetlight icon and name");

            Step("7. Verify Values of following values change");
            Step(" - Mains current (> 0 when dimming level is not '0%', = 0 when dimming level = '0%')");
            Step(" - Mains voltage(V)");
            Step(" - Power factor");
            Step(" - Temperature");
            Step(" - Local time");
            VerifyStreetlightMeteringsAfterRefreshed(realtimeControlPage, indicatorCursorPosition, feedbackValueText, commandValueText
                , lampBurningHours, lampEnergy, lampLevelCommand, lampLevelFeedback, lampPower, lampSwitchFeedback
                , nodeFailureMessage, isStreetlightIcon, deviceName, mainsCurrent, mainsVoltage, powerFactor, temperature, localTime);

            listOfCommandsText = realtimeControlPage.StreetlightWidgetPanel.GetListOfCommandsText();
            indicatorCursorPosition = realtimeControlPage.StreetlightWidgetPanel.GetIndicatorCursorPositionValue();
            feedbackValueText = realtimeControlPage.StreetlightWidgetPanel.GetRealtimeFeedbackValueText();
            commandValueText = realtimeControlPage.StreetlightWidgetPanel.GetRealtimeCommandValueText();
            lampBurningHours = realtimeControlPage.StreetlightWidgetPanel.GetLampBurningHoursValueText();
            lampEnergy = realtimeControlPage.StreetlightWidgetPanel.GetLampEnergyValueText();
            lampLevelCommand = realtimeControlPage.StreetlightWidgetPanel.GetLampLevelCommandValueText();
            lampLevelFeedback = realtimeControlPage.StreetlightWidgetPanel.GetLampLevelFeedbackValueText();
            lampPower = realtimeControlPage.StreetlightWidgetPanel.GetLampPowerValueText();
            lampSwitchFeedback = realtimeControlPage.StreetlightWidgetPanel.GetLampSwitchFeedbackValueText();
            mainsCurrent = realtimeControlPage.StreetlightWidgetPanel.GetMainsCurrentValueText();
            mainsVoltage = realtimeControlPage.StreetlightWidgetPanel.GetMainsVoltageValueText();
            nodeFailureMessage = realtimeControlPage.StreetlightWidgetPanel.GetNodeFailureMessageValueText();
            powerFactor = realtimeControlPage.StreetlightWidgetPanel.GetPowerFactorValueText();
            temperature = realtimeControlPage.StreetlightWidgetPanel.GetTemperatureValueText();
            localTime = realtimeControlPage.StreetlightWidgetPanel.GetLastUpdateTimeText();
            isStreetlightIcon = realtimeControlPage.StreetlightWidgetPanel.CheckIfDeviceIcon(DeviceType.Streetlight);
            deviceName = realtimeControlPage.StreetlightWidgetPanel.GetDeviceNameText();

            Step("8. Select a dimming level other than the current level");
            realtimeControlPage.StreetlightWidgetPanel.ExecuteRandomDimmingByCursor();
            realtimeControlPage.StreetlightWidgetPanel.WaitForCommandCompleted();

            Step("9. Verify Values of following values remain unchanged:");
            Step(" - Lamp burning hours");
            Step(" - Lamp energy");
            Step(" - Node failure message");
            Step(" - Streetlight icon and name");

            Step("10. Verify Values of following values change (remember to apply these changes)");
            Step(" - Indicator position");
            Step(" - Feedback");
            Step(" - Command");
            Step(" - Lamp level command");
            Step(" - Lamp level feedback");
            Step(" - Lamp power");
            Step(" - Lamp switch feedback (> 0 when dimming level is not '0%', = 0 when dimming level = '0%')");
            Step(" - Mains current (> 0 when dimming level is not '0%', = 0 when dimming level = '0%')");
            Step(" - Mains voltage(V)");
            Step(" - Power factor");
            Step(" - Temperature");
            Step(" - Local time");
            VerifyStreetlightMeteringsAfterExecutedCommand(realtimeControlPage, indicatorCursorPosition, feedbackValueText, commandValueText
               , lampBurningHours, lampEnergy, lampLevelCommand, lampLevelFeedback, lampPower, lampSwitchFeedback
               , nodeFailureMessage, isStreetlightIcon, deviceName, mainsCurrent, mainsVoltage, powerFactor, temperature, localTime);

            listOfCommandsText = realtimeControlPage.StreetlightWidgetPanel.GetListOfCommandsText();
            indicatorCursorPosition = realtimeControlPage.StreetlightWidgetPanel.GetIndicatorCursorPositionValue();
            feedbackValueText = realtimeControlPage.StreetlightWidgetPanel.GetRealtimeFeedbackValueText();
            commandValueText = realtimeControlPage.StreetlightWidgetPanel.GetRealtimeCommandValueText();
            lampBurningHours = realtimeControlPage.StreetlightWidgetPanel.GetLampBurningHoursValueText();
            lampEnergy = realtimeControlPage.StreetlightWidgetPanel.GetLampEnergyValueText();
            lampLevelCommand = realtimeControlPage.StreetlightWidgetPanel.GetLampLevelCommandValueText();
            lampLevelFeedback = realtimeControlPage.StreetlightWidgetPanel.GetLampLevelFeedbackValueText();
            lampPower = realtimeControlPage.StreetlightWidgetPanel.GetLampPowerValueText();
            lampSwitchFeedback = realtimeControlPage.StreetlightWidgetPanel.GetLampSwitchFeedbackValueText();
            mainsCurrent = realtimeControlPage.StreetlightWidgetPanel.GetMainsCurrentValueText();
            mainsVoltage = realtimeControlPage.StreetlightWidgetPanel.GetMainsVoltageValueText();
            nodeFailureMessage = realtimeControlPage.StreetlightWidgetPanel.GetNodeFailureMessageValueText();
            powerFactor = realtimeControlPage.StreetlightWidgetPanel.GetPowerFactorValueText();
            temperature = realtimeControlPage.StreetlightWidgetPanel.GetTemperatureValueText();
            localTime = realtimeControlPage.StreetlightWidgetPanel.GetLastUpdateTimeText();
            isStreetlightIcon = realtimeControlPage.StreetlightWidgetPanel.CheckIfDeviceIcon(DeviceType.Streetlight);
            deviceName = realtimeControlPage.StreetlightWidgetPanel.GetDeviceNameText();

            Step("11. Click Refresh button again");
            realtimeControlPage.StreetlightWidgetPanel.ClickRefreshButton();
            realtimeControlPage.StreetlightWidgetPanel.WaitForCommandCompleted();

            Step("12. Verify Values of following values remain unchanged:");
            Step(" - Indicator position");
            Step(" - Feedback");
            Step(" - Command");
            Step(" - Lamp burning hours");
            Step(" - Lamp energy");
            Step(" - Lamp level command");
            Step(" - Lamp level feedback");
            Step(" - Lamp power");
            Step(" - Lamp switch feedback");
            Step(" - Node failure message");
            Step(" - Streetlight icon and name");

            Step("13. Verify Values of following values change");
            Step(" - Mains current (> 0 when dimming level is not '0%', = 0 when dimming level = '0%')");
            Step(" - Mains voltage(V)");
            Step(" - Power factor");
            Step(" - Temperature");
            Step(" - Local time");
            VerifyStreetlightMeteringsAfterRefreshed(realtimeControlPage, indicatorCursorPosition, feedbackValueText, commandValueText
              , lampBurningHours, lampEnergy, lampLevelCommand, lampLevelFeedback, lampPower, lampSwitchFeedback
              , nodeFailureMessage, isStreetlightIcon, deviceName, mainsCurrent, mainsVoltage, powerFactor, temperature, localTime);

        }

        [Test, DynamicRetry]
        [Description("RTC_10 Control Widget - Streetlight - Close")]
        public void RTC_10()
        {
            var testData = GetTestDataOfRTC_10();
            var geozone = testData["Geozone"].ToString();
            var streetlights = testData["Streetlights"] as List<DeviceModel>;
            var streetlight = streetlights.PickRandom();
            var streetlightName = streetlight.Name;

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.RealTimeControl);

            Step("1. Go to Real-time Control app");
            Step("2. Expected Real-time Control page is routed and loaded successfully");
            var realtimeControlPage = desktopPage.GoToApp(App.RealTimeControl) as RealTimeControlPage;

            Step("3. Select a streetlight from geozone tree");
            realtimeControlPage.GeozoneTreeMainPanel.SelectNode(string.Format(@"{0}\{1}", geozone, streetlightName));

            Step("4. Verify Control widget for streetlight appears");
            realtimeControlPage.WaitForStreetlightWidgetDisplayed(streetlightName);
            VerifyEqual("4. Verify Control widget for streetlight appears", true, realtimeControlPage.IsStreetlightWidgetDisplayed());

            Step("5. Click Close button");
            realtimeControlPage.StreetlightWidgetPanel.ClickCloseButton();
            realtimeControlPage.WaitForStreetlightWidgetDisappeared();

            Step("6. Verify The widget disappears");
            VerifyEqual("6. Verify Control widget for streetlight disappears", false, realtimeControlPage.IsStreetlightWidgetDisplayed());

            Step("7. Select the streetlight from geozone tree again");
            realtimeControlPage.GeozoneTreeMainPanel.SelectNode(streetlightName);

            Step("8. Verify Control widget for streetlight appears back");
            realtimeControlPage.WaitForStreetlightWidgetDisplayed(streetlightName);
            VerifyEqual("8. Verify Control widget for streetlight appears back", true, realtimeControlPage.IsStreetlightWidgetDisplayed());
        }

        [Test, DynamicRetry]
        [Description("RTC_11 Control Widget - Streetlight - INFORMATION - UI")]
        public void RTC_11()
        {
            var testData = GetTestDataOfRTC_11();
            var geozone = testData["Geozone"].ToString();
            var controller = testData["Controller"] as DeviceModel;
            var streetlights = testData["Streetlights"] as List<DeviceModel>;
            var streetlight = streetlights.PickRandom();
            var streetlightName = streetlight.Name;

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - Use API calls to set 'power' (Lamp wattage) for testing streetlight");
            Step("**** Precondition ****\n");

            var value = SLVHelper.GenerateStringInteger(999);
            var request = SetValueToDevice(controller.Id, streetlight.Id, "power", value, Settings.GetServerTime());
            VerifyEqual(string.Format("-> Verify the request is sent successfully (attribute: {0}, value: {1})", "power", value), true, request);

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.RealTimeControl);

            Step("1. Go to Real-time Control app");
            Step("2. Expected Real-time Control page is routed and loaded successfully");
            var realtimeControlPage = desktopPage.GoToApp(App.RealTimeControl) as RealTimeControlPage;

            Step("3. Select a streetlight from geozone tree");
            realtimeControlPage.GeozoneTreeMainPanel.SelectNode(string.Format(@"{0}\{1}", geozone, streetlightName));

            Step("4. Verify Control widget for streetlight appears");
            realtimeControlPage.WaitForStreetlightWidgetDisplayed(streetlightName);
            VerifyEqual("4. Verify Control widget for streetlight appears", true, realtimeControlPage.IsStreetlightWidgetDisplayed());

            Step("5. Click INFORMATION (right arrow) button");
            realtimeControlPage.StreetlightWidgetPanel.ClickInformationButton();
            realtimeControlPage.StreetlightWidgetPanel.WaitForInformationWidgetPanelDisplayed();

            Step("6. Verify The widget turns into INFORMATION view:");
            Step(" - 'Identifier:': streetlight's identifier");
            Step(" - 'Model': streetlight's equipment type");
            Step(" - 'Unique Address:' streetlight's unique address");
            Step(" - 'Dimming group:' streetlight's dimming group");
            Step(" - 'Power:' number + ' W'");
            Step(" - 'Lamp install date: nullable");
            Step(" - 'Address:' nullable");
            Step(" - Streetlight icon and name at bottom left");

            VerifyEqual("6. Verify Identifier is 'Identifier:'", "Identifier:", realtimeControlPage.StreetlightWidgetPanel.InformationWidgetPanel.GetIdentifierText());
            VerifyEqual(string.Format("6. Verify Identifier is {0}", streetlight.Id), streetlight.Id, realtimeControlPage.StreetlightWidgetPanel.InformationWidgetPanel.GetIdentifierValueText());
            VerifyEqual("6. Verify Model is 'Model:'", "Model:", realtimeControlPage.StreetlightWidgetPanel.InformationWidgetPanel.GetModelText());
            VerifyEqual(string.Format("6. Verify Model is {0}", streetlight.TypeOfEquipment), streetlight.TypeOfEquipment, realtimeControlPage.StreetlightWidgetPanel.InformationWidgetPanel.GetModelValueText());
            VerifyEqual("6. Verify Unique Address is 'Unique Address:'", "Unique Address:", realtimeControlPage.StreetlightWidgetPanel.InformationWidgetPanel.GetUniqueAddressText());
            VerifyEqual(string.Format("6. Verify Unique Address is {0}", streetlight.UniqueAddress), streetlight.UniqueAddress, realtimeControlPage.StreetlightWidgetPanel.InformationWidgetPanel.GetUniqueAddressValueText());
            VerifyEqual("6. Verify Dimming group label is 'Dimming group:'", "Dimming group:", realtimeControlPage.StreetlightWidgetPanel.InformationWidgetPanel.GetDimmingGroupText());
            VerifyEqual(string.Format("6. Verify Dimming group is {0}", streetlight.DimmingGroup), streetlight.DimmingGroup, realtimeControlPage.StreetlightWidgetPanel.InformationWidgetPanel.GetDimmingGroupValueText());

            VerifyEqual("6. Verify Power label is 'Power:'", "Power:", realtimeControlPage.StreetlightWidgetPanel.InformationWidgetPanel.GetPowerText());
            VerifyEqual("6. Verify Power is matching 'number + ' W'", true, Regex.IsMatch(realtimeControlPage.StreetlightWidgetPanel.InformationWidgetPanel.GetPowerValueText(), @"\d{1,} W"));

            var lampInstallDateValue = realtimeControlPage.StreetlightWidgetPanel.InformationWidgetPanel.GetLampInstallDateValueText();
            VerifyEqual("6. Verify Lamp install date label is 'Lamp install date:'", "Lamp install date:", realtimeControlPage.StreetlightWidgetPanel.InformationWidgetPanel.GetLampInstallDateText());
            VerifyTrue("6. Verify Lamp install date is nullable", lampInstallDateValue != null, "not null", lampInstallDateValue);
            var addressValue = realtimeControlPage.StreetlightWidgetPanel.InformationWidgetPanel.GetAddressValueText();
            VerifyEqual("6. Verify Address label is 'Address:'", "Address:", realtimeControlPage.StreetlightWidgetPanel.InformationWidgetPanel.GetAddressText());
            VerifyTrue("6. Verify Address is nullable", addressValue != null, "not null", addressValue);

            VerifyEqual("6. Verify Streetlight icon at bottom left", true, realtimeControlPage.StreetlightWidgetPanel.InformationWidgetPanel.CheckIfDeviceIcon(DeviceType.Streetlight));
            VerifyEqual("6. Verify name at bottom left", streetlightName, realtimeControlPage.StreetlightWidgetPanel.InformationWidgetPanel.GetDeviceNameText());

            Step("7. Click on empty area of the widget");
            realtimeControlPage.StreetlightWidgetPanel.InformationWidgetPanel.ClickDeviceIcon();
            realtimeControlPage.StreetlightWidgetPanel.WaitInformationWidgetPanelDisappeared();

            Step("8. Verify The first view is returned");
            VerifyEqual("8. Verify INFORMATION button is visible", true, realtimeControlPage.StreetlightWidgetPanel.IsInformationButtonVisible());
            VerifyEqual("8. Verify Controller's gateway host is invisible", false, realtimeControlPage.StreetlightWidgetPanel.InformationWidgetPanel.IsHostNameDisplayed());
        }

        [Test, DynamicRetry]
        [Description("RTC_12 Control Widget - Streetlight - Sunrise-Sunset times")]
        [NonParallelizable]
        public void RTC_12()
        {
            var testData = GetTestDataOfRTC_12();
            var geozone = testData["Geozone"].ToString();
            var controller = testData["Controller"] as DeviceModel;
            var streetlights = testData["Streetlights"] as List<DeviceModel>;
            var streetlight = streetlights.PickRandom();
            var streetlightName = streetlight.Name;            

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.RealTimeControl);

            Step("1. Go to Real-time Control app");
            Step("2. Expected Real-time Control page is routed and loaded successfully");
            var realtimeControlPage = desktopPage.GoToApp(App.RealTimeControl) as RealTimeControlPage;

            Step("3. Select a streetlight from geozone tree");
            realtimeControlPage.GeozoneTreeMainPanel.SelectNode(string.Format(@"{0}\{1}", geozone, streetlightName));

            Step("4. Verify Control widget for streetlight appears");
            realtimeControlPage.WaitForStreetlightWidgetDisplayed(streetlightName);
            VerifyEqual("4. Verify Control widget for streetlight appears", true, realtimeControlPage.IsStreetlightWidgetDisplayed());

            Step("5. Click Sunrise/Sunset times button");
            realtimeControlPage.StreetlightWidgetPanel.ClickSunriseSunsetButton();
            realtimeControlPage.WaitForPopupDialogDisplayed();

            Step("6. Verify Sunrise/Sunset times dialog appears");
            VerifyEqual("6. Verify Sunrise/Sunset times dialog appears", true, realtimeControlPage.IsSunriseSunsetPopupDialogDisplayed());

            Step("7. Verify Dialog title is 'Sunrise/Sunset times'");
            VerifyEqual("7. Verify Dialog title is 'Sunrise/Sunset times'", "Sunrise/Sunset times", realtimeControlPage.SunriseSunsetTimesPopupPanel.GetPanelTitleText());

            if(realtimeControlPage.SunriseSunsetTimesPopupPanel.IsLoaderDisplayed())
            {
                Warning("Loading is not completed");
                return;
            }

            Step("8. Verify Current selected year (top right) is the current year of the local timezone of the browser's timezone and Date is the current date of the local timezone of the browser's timezone");
            var dateStr = realtimeControlPage.SunriseSunsetTimesPopupPanel.GetHeaderTitleText();
            if (string.IsNullOrEmpty(dateStr))
            {
                Warning("[SC-1063] Sunrise sunset times button does not work");
            }
            else
            {
                var year = realtimeControlPage.SunriseSunsetTimesPopupPanel.GetCalendarYearText();
                var date = DateTime.Parse(dateStr).Date;
                var currentLocalDate = Settings.GetLocalTime().Date;
                var sunrise = realtimeControlPage.SunriseSunsetTimesPopupPanel.GetSunriseTimeText();
                var sunset = realtimeControlPage.SunriseSunsetTimesPopupPanel.GetSunsetTimeText();
                var day = realtimeControlPage.SunriseSunsetTimesPopupPanel.GetDayTimeText();
                var night = realtimeControlPage.SunriseSunsetTimesPopupPanel.GetNightTimeText();

                VerifyEqual("8. Verify Current selected year (top right) is the current year", currentLocalDate.Year.ToString(), year);
                VerifyEqual("8. Verify Date is the current date of local timezone of the browser's timezone", currentLocalDate, date);

                Step("9. Mouse hover inside chart area");
                realtimeControlPage.SunriseSunsetTimesPopupPanel.MoveToRandomInsideChart();

                Step("10. Verify Values of following fields change when mouse-hovering inside chart area: Date, Sunrise, Sunset, Night, Day");
                var randomDateStr = realtimeControlPage.SunriseSunsetTimesPopupPanel.GetHeaderTitleText();
                var randomDate = DateTime.Parse(randomDateStr).Date;
                var randomSunrise = realtimeControlPage.SunriseSunsetTimesPopupPanel.GetSunriseTimeText();
                var randomSunset = realtimeControlPage.SunriseSunsetTimesPopupPanel.GetSunsetTimeText();
                var randomDay = realtimeControlPage.SunriseSunsetTimesPopupPanel.GetDayTimeText();
                var randomNight = realtimeControlPage.SunriseSunsetTimesPopupPanel.GetNightTimeText();
                VerifyTrue(string.Format("10. Verify Date is changed {0}->{1}", date, randomDate), date != randomDate, "<>" + date, randomDate);
                VerifyTrue(string.Format("10. Verify Sunrise is changed {0}->{1}", sunrise, randomSunrise), sunrise != randomSunrise, "<>" + sunrise, randomSunrise);
                VerifyTrue(string.Format("10. Verify Sunset is changed {0}->{1}", sunset, randomSunset), sunset != randomSunset, "<>" + sunset, randomSunset);
                VerifyTrue(string.Format("10. Verify Night is changed {0}->{1}", night, randomNight), night != randomNight, "<>" + night, randomNight);
                VerifyTrue(string.Format("10. Verify Day is changed {0}->{1}", day, randomDay), day != randomDay, "<>" + day, randomDay);

                Step("11. Verify Date has format 'full_day_name dd full_month_name yyyy'");
                VerifyEqual("11. Verify Date format is 'full_day_name dd full_month_name yyyy", true, Regex.IsMatch(dateStr, @".* \d{2} .* \d{4}"));

                Step("12. Verify Sunrise, Sunset, Night, Day have format 'hh:mm'");
                VerifyEqual("12. Verify Sunrise format is 'hh:mm''", true, Regex.IsMatch(sunrise, @"\d{2}:\d{2}"));
                VerifyEqual("12. Verify Sunset format is 'hh:mm''", true, Regex.IsMatch(sunset, @"\d{2}:\d{2}"));
                VerifyEqual("12. Verify Night format is 'hh:mm''", true, Regex.IsMatch(night, @"\d{2}:\d{2}"));
                VerifyEqual("12. Verify Day format is 'hh:mm''", true, Regex.IsMatch(day, @"\d{2}:\d{2}"));

                Step("13. Click Export");
                SLVHelper.DeleteAllFilesByPattern("sunrise-sunset-times*.csv");
                realtimeControlPage.SunriseSunsetTimesPopupPanel.ClickExportButton();
                SLVHelper.SaveDownloads();

                Step("14. Verify A csv file is downloaded with name format 'sunrise-sunset-times-{controller-id}-yyyy.csv' where {controller-id} is the id of the controller what the streetlight is controlled by and 'yyyy' is the currently selected year");
                var expectedFileCsvName = string.Format("sunrise-sunset-times-{0}-{1}.csv", streetlight.Controller.ToLower(), year);
                VerifyEqual("14. Verify A csv file is downloaded with name format 'sunrise-sunset-times-{controller-id}-yyyy.csv'", true, SLVHelper.CheckFileExists(expectedFileCsvName));

                Step("15. Click Close");
                realtimeControlPage.SunriseSunsetTimesPopupPanel.ClickCloseButton();
                realtimeControlPage.WaitForPopupDialogDisappeared();

                Step("16. Verify The dialog disappears");
                VerifyEqual("16. Verify Sunrise/Sunset times dialog disappears", false, realtimeControlPage.IsSunriseSunsetPopupDialogDisplayed());

                Step("17. Click Sunrise/Sunset times button again from control widget");
                realtimeControlPage.StreetlightWidgetPanel.ClickSunriseSunsetButton();
                realtimeControlPage.WaitForPopupDialogDisplayed();

                Step("18. Verify Sunrise/Sunset times dialog appears back");
                VerifyEqual("18. Verify Sunrise/Sunset times dialog appears back", true, realtimeControlPage.IsSunriseSunsetPopupDialogDisplayed());

                Step("19. Click Close");
                realtimeControlPage.SunriseSunsetTimesPopupPanel.ClickCloseButton();
                realtimeControlPage.WaitForPopupDialogDisappeared();

                Step("20. Verify The dialog disappears");
                VerifyEqual("20. Verify Sunrise/Sunset times dialog disappears", false, realtimeControlPage.IsSunriseSunsetPopupDialogDisplayed());
            }
        }

        [Test, DynamicRetry]
        [Description("RTC_13 Control Widget - Streetlight - Execute dimming commands")]
        [NonParallelizable]
        public void RTC_13()
        {
            var testData = GetTestDataOfRTC_13();
            var geozone = testData["Geozone"].ToString();
            var streetlights = testData["Streetlights"] as List<DeviceModel>;
            var streetlight = streetlights.PickRandom();
            var streetlightName = streetlight.Name;

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.RealTimeControl);

            Step("1. Go to Real-time Control app");
            Step("2. Expected Real-time Control page is routed and loaded successfully");
            var realtimeControlPage = desktopPage.GoToApp(App.RealTimeControl) as RealTimeControlPage;

            Step("3. Select a streetlight from geozone tree");
            realtimeControlPage.GeozoneTreeMainPanel.SelectNode(string.Format(@"{0}\{1}", geozone, streetlightName));

            Step("4. Verify Control widget for streetlight appears");
            realtimeControlPage.WaitForStreetlightWidgetDisplayed(streetlightName);
            VerifyEqual("4. Verify Control widget for streetlight appears", true, realtimeControlPage.IsStreetlightWidgetDisplayed());
            var indicatorCursorPosition = realtimeControlPage.StreetlightWidgetPanel.GetIndicatorCursorPositionValue();
            var feedbackValueText = realtimeControlPage.StreetlightWidgetPanel.GetRealtimeFeedbackValueText();
            var commandValueText = realtimeControlPage.StreetlightWidgetPanel.GetRealtimeCommandValueText();
            var lampBurningHours = realtimeControlPage.StreetlightWidgetPanel.GetLampBurningHoursValueText();
            var lampEnergy = realtimeControlPage.StreetlightWidgetPanel.GetLampEnergyValueText();
            var lampLevelCommand = realtimeControlPage.StreetlightWidgetPanel.GetLampLevelCommandValueText();
            var lampLevelFeedback = realtimeControlPage.StreetlightWidgetPanel.GetLampLevelFeedbackValueText();
            var lampPower = realtimeControlPage.StreetlightWidgetPanel.GetLampPowerValueText();
            var lampSwitchFeedback = realtimeControlPage.StreetlightWidgetPanel.GetLampSwitchFeedbackValueText();
            var mainsCurrent = realtimeControlPage.StreetlightWidgetPanel.GetMainsCurrentValueText();
            var mainsVoltage = realtimeControlPage.StreetlightWidgetPanel.GetMainsVoltageValueText();
            var nodeFailureMessage = realtimeControlPage.StreetlightWidgetPanel.GetNodeFailureMessageValueText();
            var powerFactor = realtimeControlPage.StreetlightWidgetPanel.GetPowerFactorValueText();
            var temperature = realtimeControlPage.StreetlightWidgetPanel.GetTemperatureValueText();
            var localTime = realtimeControlPage.StreetlightWidgetPanel.GetLastUpdateTimeText();
            var isStreetlightIcon = realtimeControlPage.StreetlightWidgetPanel.CheckIfDeviceIcon(DeviceType.Streetlight);
            var deviceName = realtimeControlPage.StreetlightWidgetPanel.GetDeviceNameText();

            var listOfCommandsText = realtimeControlPage.ControllerWidgetPanel.GetListOfCommandsText();
            listOfCommandsText.Remove("OFF");

            Step("5. Click OFF dimming command");
            realtimeControlPage.StreetlightWidgetPanel.ClickCommandOffButton();
            realtimeControlPage.StreetlightWidgetPanel.WaitForCommandCompleted();

            Step("6. Verify Feedback = 0%, Command = 0%, Triangle indicator's position is moved properly");
            var commandData = realtimeControlPage.StreetlightWidgetPanel.CommandsDict[RealtimeCommand.DimOff];
            VerifyEqual("6. Verify Feedback = 0%", "0%", realtimeControlPage.StreetlightWidgetPanel.GetRealtimeFeedbackValueText());
            VerifyEqual("6. Verify Command = 0%", "0%", realtimeControlPage.StreetlightWidgetPanel.GetRealtimeCommandValueText());
            VerifyEqual("6. Verify Triangle indicator's position is moved properly", commandData.TriangleValue, realtimeControlPage.StreetlightWidgetPanel.GetIndicatorCursorPositionValue());

            Step("7. Select the parent geozone from geozone tree");
            realtimeControlPage.GeozoneTreeMainPanel.SelectNode(geozone);
            realtimeControlPage.WaitForControllerWidgetDisappeared();

            Step("8. Click \"Refresh all devices displayed on the map\" button");
            realtimeControlPage.Map.MoveToGlobalEarthIcon();
            realtimeControlPage.Map.WaitForRealtimeRefreshPanelDisplayed();
            realtimeControlPage.Map.ClickRealtimeRefreshButton();
            realtimeControlPage.Map.WaitForProgressGLCompleted();

            Step("9. Hover mouse on the selected streetlight");
            realtimeControlPage.Map.MoveToDeviceGL(streetlight.Longitude, streetlight.Latitude);

            Step("10. Verify Tooltip of streetlight displays:");
            Step(" - Streetlight's name");
            Step(" - 'Status': 'OK'");
            Step(" - 'Mode': 'Manual'");
            Step(" - 'Level': '0%'");
            var tooltipName = realtimeControlPage.Map.GetDeviceNameGL();
            var tooltipStatus = realtimeControlPage.Map.GetDeviceStatusGL();
            var tooltipmode = realtimeControlPage.Map.GetDeviceModeGL();
            var tooltipLevel = realtimeControlPage.Map.GetDeviceLevelGL();
            VerifyEqual(string.Format("10. Verify Streetlight's name is {0}", streetlightName), streetlightName, tooltipName);
            VerifyEqual("10. Verify Status is OK", "OK", tooltipStatus);
            VerifyEqual("10. Verify Mode is Manual", "Manual", tooltipmode);
            VerifyEqual("10. Verify Level is 0%", "0%", tooltipLevel);

            Step("11. Select the streetlight again");
            realtimeControlPage.GeozoneTreeMainPanel.SelectNode(streetlightName);

            Step("12. Verify Control widget for streetlight appears back");
            realtimeControlPage.WaitForStreetlightWidgetDisplayed(streetlightName);
            VerifyEqual("13. Verify Control widget for streetlight appears back", true, realtimeControlPage.IsStreetlightWidgetDisplayed());

            Step("13. Verify Values of following values remain unchanged:");
            Step(" - Lamp burning hours");
            Step(" - Lamp energy");
            Step(" - Node failure message");
            Step(" - Streetlight icon and name");

            Step("14. Verify Values of following values change");
            Step(" - Indicator position");
            Step(" - Feedback");
            Step(" - Command");
            Step(" - Lamp level command");
            Step(" - Lamp level feedback");
            Step(" - Lamp power");
            Step(" - Lamp switch feedback (> 0 when dimming level is not 0%, = 0 when dimming level = '0%')");
            Step(" - Mains current (> 0 when dimming level is not 0%, = 0 when dimming level = '0%')");
            Step(" - Mains voltage(V)");
            Step(" - Power factor");
            Step(" - Temperature");
            Step(" - Local time");
            VerifyStreetlightMeteringsAfterExecutedCommand(realtimeControlPage, indicatorCursorPosition, feedbackValueText, commandValueText
                , lampBurningHours, lampEnergy, lampLevelCommand, lampLevelFeedback, lampPower, lampSwitchFeedback
                , nodeFailureMessage, isStreetlightIcon, deviceName, mainsCurrent, mainsVoltage, powerFactor, temperature, localTime);

            Step("15. Repeat steps from #5 to #14 with other dimming levels and with using Triangle indicator");
            Step("16. Verify Feedback = '{dimming-level-value}%', Feedback = '{dimming-level-value}%'");
            Step("17. Verify After \"refresh all devices displayed on the map\", tooltip of the selected streetlight displays:");
            Step(" - Streetlight's name");
            Step(" - 'Status': 'OK'");
            Step(" - 'Mode': 'Manual'");
            Step(" - 'Level': '{dimming-level-value} %' ('100 %' in case 'ON' level is clicked)");
            realtimeControlPage.AppBar.ClickHeaderBartop();

            foreach (var commandText in listOfCommandsText)
            {
                indicatorCursorPosition = realtimeControlPage.StreetlightWidgetPanel.GetIndicatorCursorPositionValue();
                feedbackValueText = realtimeControlPage.StreetlightWidgetPanel.GetRealtimeFeedbackValueText();
                commandValueText = realtimeControlPage.StreetlightWidgetPanel.GetRealtimeCommandValueText();
                lampBurningHours = realtimeControlPage.StreetlightWidgetPanel.GetLampBurningHoursValueText();
                lampEnergy = realtimeControlPage.StreetlightWidgetPanel.GetLampEnergyValueText();
                lampLevelCommand = realtimeControlPage.StreetlightWidgetPanel.GetLampLevelCommandValueText();
                lampLevelFeedback = realtimeControlPage.StreetlightWidgetPanel.GetLampLevelFeedbackValueText();
                lampPower = realtimeControlPage.StreetlightWidgetPanel.GetLampPowerValueText();
                lampSwitchFeedback = realtimeControlPage.StreetlightWidgetPanel.GetLampSwitchFeedbackValueText();
                mainsCurrent = realtimeControlPage.StreetlightWidgetPanel.GetMainsCurrentValueText();
                mainsVoltage = realtimeControlPage.StreetlightWidgetPanel.GetMainsVoltageValueText();
                nodeFailureMessage = realtimeControlPage.StreetlightWidgetPanel.GetNodeFailureMessageValueText();
                powerFactor = realtimeControlPage.StreetlightWidgetPanel.GetPowerFactorValueText();
                temperature = realtimeControlPage.StreetlightWidgetPanel.GetTemperatureValueText();
                localTime = realtimeControlPage.StreetlightWidgetPanel.GetLastUpdateTimeText();
                isStreetlightIcon = realtimeControlPage.StreetlightWidgetPanel.CheckIfDeviceIcon(DeviceType.Streetlight);
                deviceName = realtimeControlPage.StreetlightWidgetPanel.GetDeviceNameText();

                var command = realtimeControlPage.StreetlightWidgetPanel.GetCommandByText(commandText);
                commandData = realtimeControlPage.StreetlightWidgetPanel.CommandsDict[command];
                realtimeControlPage.StreetlightWidgetPanel.ExecuteCommand(command);
                realtimeControlPage.StreetlightWidgetPanel.WaitForCommandCompleted();
              
                VerifyEqual(string.Format("17. Verify Feedback = {0}", commandData.ValueText), commandData.ValueText, realtimeControlPage.StreetlightWidgetPanel.GetRealtimeFeedbackValueText());
                VerifyEqual(string.Format("17. Verify Command = {0}", commandData.ValueText), commandData.ValueText, realtimeControlPage.StreetlightWidgetPanel.GetRealtimeCommandValueText());
                VerifyEqual("17. Verify Triangle indicator's position is moved properly", commandData.TriangleValue, realtimeControlPage.StreetlightWidgetPanel.GetIndicatorCursorPositionValue());

                Step("-> Select the parent geozone from geozone tree");
                realtimeControlPage.GeozoneTreeMainPanel.SelectNode(geozone);
                realtimeControlPage.WaitForStreetlightWidgetDisappeared();
                Step("-> Click \"Refresh all devices displayed on the map\" button");
                realtimeControlPage.Map.MoveToGlobalEarthIcon();
                realtimeControlPage.Map.WaitForRealtimeRefreshPanelDisplayed();
                realtimeControlPage.Map.ClickRealtimeRefreshButton();
                realtimeControlPage.Map.WaitForProgressGLCompleted();

                Step("-> Hover mouse on the selected streetlight");
                realtimeControlPage.Map.MoveToDeviceGL(streetlight.Longitude, streetlight.Latitude);

                tooltipName = realtimeControlPage.Map.GetDeviceNameGL();
                tooltipStatus = realtimeControlPage.Map.GetDeviceStatusGL();
                tooltipmode = realtimeControlPage.Map.GetDeviceModeGL();
                tooltipLevel = realtimeControlPage.Map.GetDeviceLevelGL();
                VerifyEqual(string.Format("17. Verify Streetlight's name is {0}", streetlightName), streetlightName, tooltipName);
                VerifyEqual("17. Verify Status is OK", "OK", tooltipStatus);
                VerifyEqual("17. Verify Mode is Manual", "Manual", tooltipmode);
                VerifyEqual(string.Format("Verify Level is {0}", commandData.ValueText), commandData.ValueText, tooltipLevel);
                realtimeControlPage.AppBar.ClickHeaderBartop();

                Step("-> Select the streetlight again");
                realtimeControlPage.GeozoneTreeMainPanel.SelectNode(streetlightName);
                Step("-> Verify Control widget for streetlight appears back");
                realtimeControlPage.WaitForStreetlightWidgetDisplayed(streetlightName);
                VerifyEqual("17. Verify Control widget for streetlight appears back", true, realtimeControlPage.IsStreetlightWidgetDisplayed());

                VerifyStreetlightMeteringsAfterExecutedCommand(realtimeControlPage, indicatorCursorPosition, feedbackValueText, commandValueText
               , lampBurningHours, lampEnergy, lampLevelCommand, lampLevelFeedback, lampPower, lampSwitchFeedback
               , nodeFailureMessage, isStreetlightIcon, deviceName, mainsCurrent, mainsVoltage, powerFactor, temperature, localTime);
            }
        }

        [Test, DynamicRetry]
        [Description("RTC_14 Control Widget - Streetlight - Exit manual mode")]
        [NonParallelizable]
        public void RTC_14()
        {
            var testData = GetTestDataOfRTC_14();
            var geozone = testData["Geozone"].ToString();
            var streetlights = testData["Streetlights"] as List<DeviceModel>;
            var streetlight = streetlights.PickRandom();
            var streetlightName = streetlight.Name;

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.RealTimeControl);

            Step("1. Go to Real-time Control app");
            Step("2. Expected Real-time Control page is routed and loaded successfully");
            var realtimeControlPage = desktopPage.GoToApp(App.RealTimeControl) as RealTimeControlPage;

            Step("3. Select a streetlight from geozone tree");
            realtimeControlPage.GeozoneTreeMainPanel.SelectNode(string.Format(@"{0}\{1}", geozone, streetlightName));

            Step("4. Verify Control widget for streetlight appears");
            realtimeControlPage.WaitForStreetlightWidgetDisplayed(streetlightName);
            VerifyEqual("4. Verify Control widget for streetlight appears", true, realtimeControlPage.IsStreetlightWidgetDisplayed());
            var indicatorCursorPosition = realtimeControlPage.StreetlightWidgetPanel.GetIndicatorCursorPositionValue();
            var feedbackValueText = realtimeControlPage.StreetlightWidgetPanel.GetRealtimeFeedbackValueText();
            var commandValueText = realtimeControlPage.StreetlightWidgetPanel.GetRealtimeCommandValueText();
            var lampBurningHours = realtimeControlPage.StreetlightWidgetPanel.GetLampBurningHoursValueText();
            var lampEnergy = realtimeControlPage.StreetlightWidgetPanel.GetLampEnergyValueText();
            var lampLevelCommand = realtimeControlPage.StreetlightWidgetPanel.GetLampLevelCommandValueText();
            var lampLevelFeedback = realtimeControlPage.StreetlightWidgetPanel.GetLampLevelFeedbackValueText();
            var lampPower = realtimeControlPage.StreetlightWidgetPanel.GetLampPowerValueText();
            var lampSwitchFeedback = realtimeControlPage.StreetlightWidgetPanel.GetLampSwitchFeedbackValueText();
            var mainsCurrent = realtimeControlPage.StreetlightWidgetPanel.GetMainsCurrentValueText();
            var mainsVoltage = realtimeControlPage.StreetlightWidgetPanel.GetMainsVoltageValueText();
            var nodeFailureMessage = realtimeControlPage.StreetlightWidgetPanel.GetNodeFailureMessageValueText();
            var powerFactor = realtimeControlPage.StreetlightWidgetPanel.GetPowerFactorValueText();
            var temperature = realtimeControlPage.StreetlightWidgetPanel.GetTemperatureValueText();
            var localTime = realtimeControlPage.StreetlightWidgetPanel.GetLastUpdateTimeText();
            var isStreetlightIcon = realtimeControlPage.StreetlightWidgetPanel.CheckIfDeviceIcon(DeviceType.Streetlight);
            var deviceName = realtimeControlPage.StreetlightWidgetPanel.GetDeviceNameText();

            Step("5. Execute a dimming command (except ON)");
            var command = realtimeControlPage.StreetlightWidgetPanel.ExecuteRandomDimming(RealtimeCommand.DimOn);
            var commandData = realtimeControlPage.StreetlightWidgetPanel.CommandsDict[command];
            realtimeControlPage.StreetlightWidgetPanel.WaitForCommandCompleted();

            Step("6. Verify Control widget displays 'MANUAL' for dimming mode");
            VerifyEqual("6. Verify Control widget displays 'MANUAL' for dimming mode", "MANUAL", realtimeControlPage.StreetlightWidgetPanel.GetClockText());

            Step("7. Select the parent geozone from geozone tree");
            realtimeControlPage.GeozoneTreeMainPanel.SelectNode(geozone);
            realtimeControlPage.WaitForControllerWidgetDisappeared();

            Step("8. Click \"Refresh all devices displayed on the map\" button");
            realtimeControlPage.Map.MoveToGlobalEarthIcon();
            realtimeControlPage.Map.WaitForRealtimeRefreshPanelDisplayed();
            realtimeControlPage.Map.ClickRealtimeRefreshButton();
            realtimeControlPage.Map.WaitForProgressGLCompleted();

            Step("9. Hover mouse on the selected streetlight");
            realtimeControlPage.Map.MoveToDeviceGL(streetlight.Longitude, streetlight.Latitude);

            Step("10. Verify Tooltip of streetlight displays:");
            Step(" - Streetlight's name");
            Step(" - 'Status': 'OK'");
            Step(" - 'Mode': 'Manual'");
            Step(" - 'Level': '{dimming-level}%'");
            var tooltipName = realtimeControlPage.Map.GetDeviceNameGL();
            var tooltipStatus = realtimeControlPage.Map.GetDeviceStatusGL();
            var tooltipmode = realtimeControlPage.Map.GetDeviceModeGL();
            var tooltipLevel = realtimeControlPage.Map.GetDeviceLevelGL();
            VerifyEqual(string.Format("10. Verify Streetlight's name is {0}", streetlightName), streetlightName, tooltipName);
            VerifyEqual("10. Verify Status is OK", "OK", tooltipStatus);
            VerifyEqual("10. Verify Mode is Manual", "Manual", tooltipmode);
            VerifyEqual(string.Format("10. Verify Level is {0}", commandData.ValueText), commandData.ValueText, tooltipLevel);
            realtimeControlPage.AppBar.ClickHeaderBartop();

            Step("11. Select the streetlight again");
            realtimeControlPage.GeozoneTreeMainPanel.SelectNode(streetlightName);

            Step("12. Verify Control widget for streetlight appears back");
            realtimeControlPage.WaitForStreetlightWidgetDisplayed(streetlightName);
            VerifyEqual("12. Verify Control widget for streetlight appears back", true, realtimeControlPage.IsStreetlightWidgetDisplayed());

            Step("13. Click AUTOMATIC button on the control widget");
            realtimeControlPage.StreetlightWidgetPanel.ClickClockButton();
            realtimeControlPage.StreetlightWidgetPanel.WaitForCommandCompleted();

            Step("14. Verify Control widget displays 'AUTOMATIC' for dimming mode");
            VerifyEqual("14. Verify Control widget displays 'AUTOMATIC' for dimming mode", "AUTOMATIC", realtimeControlPage.StreetlightWidgetPanel.GetClockText());

            Step("15. Verify Values of following values remain unchanged:");
            Step(" - Lamp burning hours");
            Step(" - Lamp energy");
            Step(" - Node failure message");
            Step(" - Streetlight icon and name");

            Step("16. Verify Values of following values change");
            Step(" - Indicator position");
            Step(" - Feedback");
            Step(" - Command");
            Step(" - Lamp level command");
            Step(" - Lamp level feedback");
            Step(" - Lamp power");
            Step(" - Lamp switch feedback (> 0 when dimming level is not 0%, = 0 when dimming level = '0%')");
            Step(" - Mains current (> 0 when dimming level is not 0%, = 0 when dimming level = '0%')");
            Step(" - Mains voltage(V)");
            Step(" - Power factor");
            Step(" - Temperature");
            Step(" - Local time");
            VerifyStreetlightMeteringsAfterExecutedCommand(realtimeControlPage, indicatorCursorPosition, feedbackValueText, commandValueText
                , lampBurningHours, lampEnergy, lampLevelCommand, lampLevelFeedback, lampPower, lampSwitchFeedback
                , nodeFailureMessage, isStreetlightIcon, deviceName, mainsCurrent, mainsVoltage, powerFactor, temperature, localTime);

            Step("17. Select the parent geozone from geozone tree");
            realtimeControlPage.GeozoneTreeMainPanel.SelectNode(geozone);
            realtimeControlPage.WaitForControllerWidgetDisappeared();

            Step("18. Click \"Refresh all devices displayed on the map\" button");
            realtimeControlPage.Map.MoveToGlobalEarthIcon();
            realtimeControlPage.Map.WaitForRealtimeRefreshPanelDisplayed();
            realtimeControlPage.Map.ClickRealtimeRefreshButton();
            realtimeControlPage.Map.WaitForProgressGLCompleted();

            Step("19. Hover mouse on the selected streetlight");
            realtimeControlPage.Map.MoveToDeviceGL(streetlight.Longitude, streetlight.Latitude);

            Step("20. Verify Tooltip of streetlight displays:");
            Step(" - Streetlight's name");
            Step(" - 'Status': 'OK'");
            Step(" - 'Mode': 'Automatic'");
            Step(" - 'Level': '100 %'");
            tooltipName = realtimeControlPage.Map.GetDeviceNameGL();
            tooltipStatus = realtimeControlPage.Map.GetDeviceStatusGL();
            tooltipmode = realtimeControlPage.Map.GetDeviceModeGL();
            tooltipLevel = realtimeControlPage.Map.GetDeviceLevelGL();
            VerifyEqual(string.Format("20. Verify Streetlight's name is {0}", streetlightName), streetlightName, tooltipName);
            VerifyEqual("20. Verify Status is OK", "OK", tooltipStatus);
            VerifyEqual("20. Verify Mode is Automatic", "Automatic", tooltipmode);
            VerifyEqual("20. Verify Level is 100%", "100%", tooltipLevel);
            realtimeControlPage.AppBar.ClickHeaderBartop();

            Step("21. Select the streetlight again");
            realtimeControlPage.GeozoneTreeMainPanel.SelectNode(streetlightName);

            Step("21. Verify Control widget for streetlight appears back");
            realtimeControlPage.WaitForStreetlightWidgetDisplayed(streetlightName);
            VerifyEqual("22. Verify Control widget for streetlight appears back", true, realtimeControlPage.IsStreetlightWidgetDisplayed());

            indicatorCursorPosition = realtimeControlPage.StreetlightWidgetPanel.GetIndicatorCursorPositionValue();
            feedbackValueText = realtimeControlPage.StreetlightWidgetPanel.GetRealtimeFeedbackValueText();
            commandValueText = realtimeControlPage.StreetlightWidgetPanel.GetRealtimeCommandValueText();
            lampBurningHours = realtimeControlPage.StreetlightWidgetPanel.GetLampBurningHoursValueText();
            lampEnergy = realtimeControlPage.StreetlightWidgetPanel.GetLampEnergyValueText();
            lampLevelCommand = realtimeControlPage.StreetlightWidgetPanel.GetLampLevelCommandValueText();
            lampLevelFeedback = realtimeControlPage.StreetlightWidgetPanel.GetLampLevelFeedbackValueText();
            lampPower = realtimeControlPage.StreetlightWidgetPanel.GetLampPowerValueText();
            lampSwitchFeedback = realtimeControlPage.StreetlightWidgetPanel.GetLampSwitchFeedbackValueText();
            mainsCurrent = realtimeControlPage.StreetlightWidgetPanel.GetMainsCurrentValueText();
            mainsVoltage = realtimeControlPage.StreetlightWidgetPanel.GetMainsVoltageValueText();
            nodeFailureMessage = realtimeControlPage.StreetlightWidgetPanel.GetNodeFailureMessageValueText();
            powerFactor = realtimeControlPage.StreetlightWidgetPanel.GetPowerFactorValueText();
            temperature = realtimeControlPage.StreetlightWidgetPanel.GetTemperatureValueText();
            localTime = realtimeControlPage.StreetlightWidgetPanel.GetLastUpdateTimeText();
            isStreetlightIcon = realtimeControlPage.StreetlightWidgetPanel.CheckIfDeviceIcon(DeviceType.Streetlight);
            deviceName = realtimeControlPage.StreetlightWidgetPanel.GetDeviceNameText();

            Step("23. Click AUTOMATIC button on the control widget");
            realtimeControlPage.StreetlightWidgetPanel.ClickClockButton();
            realtimeControlPage.StreetlightWidgetPanel.WaitForCommandCompleted();

            Step("24. Verify Control widget displays 'AUTOMATIC' for dimming mode");
            VerifyEqual("24. Verify Control widget displays 'AUTOMATIC' for dimming mode", "AUTOMATIC", realtimeControlPage.StreetlightWidgetPanel.GetClockText());

            Step("25. Verify All values in control widget remain unchanged");
            VerifyStreetlightMeteringsUnchanged(realtimeControlPage, indicatorCursorPosition, feedbackValueText, commandValueText
              , lampBurningHours, lampEnergy, lampLevelCommand, lampLevelFeedback, lampPower, lampSwitchFeedback
              , nodeFailureMessage, isStreetlightIcon, deviceName, mainsCurrent, mainsVoltage, powerFactor, temperature, localTime);
        }

        [Test, DynamicRetry]
        [Description("RTC_15 Control Widget - Streetlight - Status")]
        public void RTC_15()
        {
            var testData = GetTestDataOfRTC_15();
            var geozone = testData["Geozone"].ToString();
            var workingStreetlights = testData["WorkingStreetlights"] as List<DeviceModel>;
            var nonWorkingStreetlights = testData["NonWorkingStreetlights"] as List<DeviceModel>;
            var workingStreetlight = workingStreetlights.PickRandom();
            var workingStreetlightName = workingStreetlight.Name;
            var nonWorkingStreetlight = nonWorkingStreetlights.PickRandom();
            var nonWorkingStreetlightName = nonWorkingStreetlight.Name;

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.RealTimeControl);

            Step("1. Go to Real-time Control app");
            Step("2. Expected Real-time Control page is routed and loaded successfully");
            var realtimeControlPage = desktopPage.GoToApp(App.RealTimeControl) as RealTimeControlPage;

            Step("3. Select a non-working streetlight from geozone tree");
            realtimeControlPage.GeozoneTreeMainPanel.SelectNode(string.Format(@"{0}\{1}", geozone, nonWorkingStreetlightName));

            Step("4. Verify Control widget for streetlight appears");
            realtimeControlPage.WaitForStreetlightWidgetDisplayed(nonWorkingStreetlightName);
            VerifyEqual("Verify Control widget for streetlight appears", true, realtimeControlPage.IsStreetlightWidgetDisplayed());

            Step("5. Verify When a non-working streetlight:");
            Step(" - AUTOMATIC button with clock icon and label '...'");
            Step(" - Status button: icon is the question icon");
            Step(" - Metering values is '...' (Lamp buring hours, Lamp energy , Lamp level command, Lamp level feedback, Lamp power, Lamp switch feedback, Mains current, Mains voltage (V), Node failure message, Power factor, Temperature)");
            VerifyEqual("5. Verify clock icon is visible", true, realtimeControlPage.StreetlightWidgetPanel.IsClockButtonVisible());
            VerifyEqual("5. Verify AUTOMATIC button with clock icon and label is '...'", "...", realtimeControlPage.StreetlightWidgetPanel.GetClockText());
            var statusIconUrl = realtimeControlPage.StreetlightWidgetPanel.GetStatusIconValue();
            VerifyTrue("5. Verify Status icon is Question icon", statusIconUrl.Contains("status-question.png"), "status-question.png", statusIconUrl);
            VerifyNonWorkingStreetlightMeterings(realtimeControlPage);

            Step("6. Click Status button");
            realtimeControlPage.StreetlightWidgetPanel.ClickStatusButton();

            Step("7. Verify Status view is turned to");
            realtimeControlPage.StreetlightWidgetPanel.WaitForStatusPanelDisplayed();
            VerifyEqual("7. Verify Status view is shown", true, realtimeControlPage.StreetlightWidgetPanel.IsStatusPanelShown());

            Step("8. Verify 4 attributes with labels 'Lamp failure', 'Node failure', 'Lost communication', 'Unknown identifier' and its label has question icon");
            VerifyStreetlightStatusPanel(realtimeControlPage, DeviceStatus.NonWorking);

            Step("9. Click Status button");
            realtimeControlPage.StreetlightWidgetPanel.ClickStatusButton();

            Step("10. Verify The first view is returned");
            realtimeControlPage.StreetlightWidgetPanel.WaitForStatusPanelDisappeared();
            VerifyEqual("10. Verify Information button is visible", true, realtimeControlPage.StreetlightWidgetPanel.IsInformationButtonVisible());
            VerifyEqual("10. Verify Status view is hidden", false, realtimeControlPage.StreetlightWidgetPanel.IsStatusPanelShown());

            Step("11. Select a working streetlight from geozone tree");
            realtimeControlPage.GeozoneTreeMainPanel.SelectNode(string.Format(@"{0}\{1}", geozone, workingStreetlightName));

            Step("12. Verify Control widget for streetlight appears");
            realtimeControlPage.WaitForStreetlightWidgetDisplayed(workingStreetlightName);
            VerifyEqual("12. Verify Control widget for streetlight appears", true, realtimeControlPage.IsStreetlightWidgetDisplayed());

            Step("13. Verify When a working streetlight:");
            Step(" - Clock icon with label 'AUTOMATIC' or 'MANUAL'");
            Step(" - Status button: icon is the Ok icon");
            Step(" - Metering values: covered in 'Streetlight - UI' test");
            var lockText = realtimeControlPage.StreetlightWidgetPanel.GetClockText();
            VerifyEqual("13. Verify clock icon is visible", true, realtimeControlPage.StreetlightWidgetPanel.IsClockButtonVisible());
            VerifyTrue("13. Verify Clock icon with label 'AUTOMATIC' or 'MANUAL'", lockText == "AUTOMATIC" || lockText == "MANUAL", "AUTOMATIC/MANUAL", lockText);
            statusIconUrl = realtimeControlPage.StreetlightWidgetPanel.GetStatusIconValue();
            VerifyTrue("13. Verify Status icon is Ok icon", statusIconUrl.Contains("status-ok.png"), "status-ok.png", statusIconUrl);
            VerifyWorkingStreetlightMeterings(realtimeControlPage);

            Step("14. Click Status button");
            realtimeControlPage.StreetlightWidgetPanel.ClickStatusButton();

            Step("15. Verify Status view is turned to");
            realtimeControlPage.StreetlightWidgetPanel.WaitForStatusPanelDisplayed();
            VerifyEqual("15. VVerify Status view is shown", true, realtimeControlPage.StreetlightWidgetPanel.IsStatusPanelShown());

            Step("16. Verify 4 attributes with labels 'Lamp failure', 'Node failure', 'Lost communication', 'Unknown identifier' and its label has ok icon");
            VerifyStreetlightStatusPanel(realtimeControlPage, DeviceStatus.Working);

            Step("17. Click Status button");
            realtimeControlPage.StreetlightWidgetPanel.ClickStatusButton();

            Step("18. Verify The first view is returned");
            realtimeControlPage.StreetlightWidgetPanel.WaitForStatusPanelDisappeared();
            VerifyEqual("18. Verify Information button is visible", true, realtimeControlPage.StreetlightWidgetPanel.IsInformationButtonVisible());
            VerifyEqual("18. Verify Status view is hidden", false, realtimeControlPage.StreetlightWidgetPanel.IsStatusPanelShown());
        }

        [Test, DynamicRetry]
        [Description("RTC_22 Select controller from geozone tree-map-search results panel")]
        public void RTC_22()
        {
            var testData = GetTestDataOfRTC_22();
            var geozone = testData["Geozone"].ToString();
            var controller = testData["Controller"] as DeviceModel;
            var controllerName = controller.Name;

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.RealTimeControl);

            Step("1. Go to Real-time Control app");
            Step("2. Expected Real-time Control page is routed and loaded successfully");
            var realtimeControlPage = desktopPage.GoToApp(App.RealTimeControl) as RealTimeControlPage;

            Step("3. Verify No any control widget appears");
            VerifyEqual("3. Verify No any control widget appears", false, realtimeControlPage.IsWidgetDisplayed());

            Step("4. Select a controller from geozone tree");
            realtimeControlPage.GeozoneTreeMainPanel.SelectNode(string.Format(@"{0}\{1}", geozone, controllerName));

            Step("5. Verify Control widget for controller appears");
            realtimeControlPage.WaitForControllerWidgetDisplayed(controllerName);
            VerifyEqual("5. Verify Control widget for controller appears", true, realtimeControlPage.IsControllerWidgetDisplayed());

            Step("6. Verify The controller is being selected in geozone tree and map");
            VerifyEqual("6. Verify The controller is being selected in geozone tree", controllerName, realtimeControlPage.GeozoneTreeMainPanel.GetSelectedNodeName());
            realtimeControlPage.Map.MoveToSelectedDeviceGL();
            VerifyEqual("6. Verify The controller is being selected in map", controllerName, realtimeControlPage.Map.GetDeviceNameGL());
            realtimeControlPage.AppBar.ClickHeaderBartop();

            Step("7. Select it's parent geozone");
            realtimeControlPage.GeozoneTreeMainPanel.SelectNode(geozone);
            realtimeControlPage.WaitForControllerWidgetDisappeared();

            Step("8. Verify Control widget for controller disappears. No other widgets appear either.");
            VerifyEqual("8. Verify Control widget for controller disappears", false, realtimeControlPage.IsControllerWidgetDisplayed());
            VerifyEqual("8. Verify No other widgets appear either", false, realtimeControlPage.IsWidgetDisplayed());

            Step("9. Verify The parent geozone is being selected in geozone tree and no selection in map");
            VerifyEqual("9. Verify The parent geozone is being selected in geozone tree", geozone, realtimeControlPage.GeozoneTreeMainPanel.GetSelectedNodeName());
            VerifyTrue("[SC-691] 9. Verify no selection in map", !realtimeControlPage.Map.HasSelectedDevicesInMapGL(), "No devices selected", "Have devices selected");

            Step("10. Select the controller again but from map");
            realtimeControlPage.Map.SelectDeviceGL(controller.Longitude, controller.Latitude);

            Step("11. Verify Control widget for controller appears");
            realtimeControlPage.WaitForControllerWidgetDisplayed(controllerName);
            VerifyEqual("Verify Control widget for controller appears", true, realtimeControlPage.IsControllerWidgetDisplayed());

            Step("12. Verify The controller is being selected in geozone tree and map");
            VerifyEqual("12. Verify The controller is being selected in geozone tree", controllerName, realtimeControlPage.GeozoneTreeMainPanel.GetSelectedNodeName());
            realtimeControlPage.Map.MoveToSelectedDeviceGL();
            VerifyEqual("12. Verify The controller is being selected in map", controllerName, realtimeControlPage.Map.GetDeviceNameGL());
            realtimeControlPage.AppBar.ClickHeaderBartop();

            Step("13. Select it's parent geozone");
            realtimeControlPage.GeozoneTreeMainPanel.SelectNode(geozone);
            realtimeControlPage.WaitForControllerWidgetDisappeared();

            Step("14. Verify Control widget for controller disappears. No other widgets appear either.");
            VerifyEqual("14. Verify Control widget for controller disappears", false, realtimeControlPage.IsControllerWidgetDisplayed());
            VerifyEqual("14. Verify No other widgets appear either", false, realtimeControlPage.IsWidgetDisplayed());

            Step("15. Verify The parent geozone is being selected in geozone tree and no selection in map");
            VerifyEqual("15. Verify The parent geozone is being selected in geozone tree", geozone, realtimeControlPage.GeozoneTreeMainPanel.GetSelectedNodeName());
            VerifyTrue("[SC-691] 15. Verify no selection in map", !realtimeControlPage.Map.HasSelectedDevicesInMapGL(), "No devices selected", "Have devices selected");

            Step("16. Search with name of the controller using Search bar in geozone tree");
            realtimeControlPage.GeozoneTreeMainPanel.ChangeSearchAttribute("Name", "Contains");
            realtimeControlPage.GeozoneTreeMainPanel.EnterSearchTextInput(controllerName);
            realtimeControlPage.GeozoneTreeMainPanel.ClickSearchButton();
            realtimeControlPage.WaitForPreviousActionComplete();

            Step("17. Verify Search results panel appears with the controller found");
            var searchResult = realtimeControlPage.GeozoneTreeMainPanel.SearchResultsGeozonePanel.GetListOfSearchResult("Equipment");
            VerifyEqual("17. Verify Search results panel appears with the controller found", true, searchResult.Exists(s => s.Equals(controllerName)));

            Step("18. Select the controller from Search results panel");
            realtimeControlPage.GeozoneTreeMainPanel.SearchResultsGeozonePanel.SelectFoundDevice(controllerName);
            realtimeControlPage.WaitForPreviousActionComplete();

            Step("19. Verify Control widget for controller appears");
            realtimeControlPage.WaitForControllerWidgetDisplayed(controllerName);
            VerifyEqual("19. Verify Control widget for controller appears", true, realtimeControlPage.IsControllerWidgetDisplayed());

            Step("20. Verify The controller is being selected in found grid and map");
            VerifyEqual("20. Verify The controller is being selected in found grid", controllerName, realtimeControlPage.GeozoneTreeMainPanel.SearchResultsGeozonePanel.GetSelectedEquipmentName());
            realtimeControlPage.Map.MoveToSelectedDeviceGL();
            VerifyEqual("20. Verify The controller is being selected in map", controllerName, realtimeControlPage.Map.GetDeviceNameGL());
            realtimeControlPage.AppBar.ClickHeaderBartop();

            Step("21. Close the search results panel");
            realtimeControlPage.GeozoneTreeMainPanel.SearchResultsGeozonePanel.ClickBackButton();

            Step("22. Verify Search results panel disappears");
            realtimeControlPage.GeozoneTreeMainPanel.WaitForSearchResultPanelDisappeared();
            VerifyEqual("22. Verify Search results panel disappears", false, realtimeControlPage.GeozoneTreeMainPanel.IsSearchResultPanelDisplayed());

            Step("23. Verify The controller is being selected in geozone tree");
            VerifyEqual("23. Verify The controller is being selected in geozone tree", controllerName, realtimeControlPage.GeozoneTreeMainPanel.GetSelectedNodeName());

            Step("24. Verify The controller is still being selected in map");
            realtimeControlPage.Map.MoveToSelectedDeviceGL();
            VerifyEqual("24. Verify The controller is being selected in map", controllerName, realtimeControlPage.Map.GetDeviceNameGL());
            realtimeControlPage.AppBar.ClickHeaderBartop();

            Step("25. Verify Control widget for controller still shows");
            VerifyEqual("25. Verify Control widget for controller still shows", true, realtimeControlPage.IsControllerWidgetDisplayed());
        }

        [Test, DynamicRetry]
        [Description("RTC_23 Select streetlight from geozone tree-map-search results panel")]
        public void RTC_23()
        {
            var testData = GetTestDataOfRTC_23();
            var geozone = testData["Geozone"].ToString();
            var streetlights = testData["Streetlights"] as List<DeviceModel>;
            var streetlight = streetlights.PickRandom();
            var streetlightName = streetlight.Name;

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.RealTimeControl);

            Step("1. Go to Real-time Control app");
            Step("2. Expected Real-time Control page is routed and loaded successfully");
            var realtimeControlPage = desktopPage.GoToApp(App.RealTimeControl) as RealTimeControlPage;

            Step("3. Verify No any control widget appears");
            VerifyEqual("3. Verify No any control widget appears", false, realtimeControlPage.IsWidgetDisplayed());

            Step("4. Select a streetlight from geozone tree");
            realtimeControlPage.GeozoneTreeMainPanel.SelectNode(string.Format(@"{0}\{1}", geozone, streetlightName));

            Step("5. Verify Control widget for streetlight appears");
            realtimeControlPage.WaitForStreetlightWidgetDisplayed(streetlightName);
            VerifyEqual("5. Verify Control widget for streetlight appears", true, realtimeControlPage.IsStreetlightWidgetDisplayed());

            Step("6. Verify The streetlight is being selected in geozone tree and map");
            VerifyEqual("6. Verify The streetlight is being selected in geozone tree", streetlightName, realtimeControlPage.GeozoneTreeMainPanel.GetSelectedNodeName());
            realtimeControlPage.Map.MoveToSelectedDeviceGL();
            VerifyEqual("6. Verify The streetlight is being selected in map", streetlightName, realtimeControlPage.Map.GetDeviceNameGL());
            realtimeControlPage.AppBar.ClickHeaderBartop();

            Step("7. Select it's parent geozone");
            realtimeControlPage.GeozoneTreeMainPanel.SelectNode(geozone);
            realtimeControlPage.WaitForStreetlightWidgetDisappeared();

            Step("8. Verify Control widget for streetlight disappears. No other widgets appear either.");
            VerifyEqual("8. Verify Control widget for streetlight disappears", false, realtimeControlPage.IsStreetlightWidgetDisplayed());
            VerifyEqual("8. Verify No other widgets appear either", false, realtimeControlPage.IsWidgetDisplayed());

            Step("9. Verify The parent geozone is being selected in geozone tree and no selection in map");
            VerifyEqual("9. Verify The parent geozone is being selected in geozone tree", geozone, realtimeControlPage.GeozoneTreeMainPanel.GetSelectedNodeName());
            VerifyTrue("[SC-691] 9. Verify no selection in map", !realtimeControlPage.Map.HasSelectedDevicesInMapGL(), "No devices selected", "Have devices selected");

            Step("10. Select the controller again but from map");
            realtimeControlPage.Map.SelectDeviceGL(streetlight.Longitude, streetlight.Latitude);

            Step("11. Verify Control widget for controller appears");
            realtimeControlPage.WaitForStreetlightWidgetDisplayed(streetlightName);
            VerifyEqual("11. Verify Control widget for controller appears", true, realtimeControlPage.IsStreetlightWidgetDisplayed());

            Step("12. Verify The controller is being selected in geozone tree and map");
            VerifyEqual("12. Verify The controller is being selected in geozone tree", streetlightName, realtimeControlPage.GeozoneTreeMainPanel.GetSelectedNodeName());
            realtimeControlPage.Map.MoveToSelectedDeviceGL();
            VerifyEqual("12. Verify The controller is being selected in map", streetlightName, realtimeControlPage.Map.GetDeviceNameGL());
            realtimeControlPage.AppBar.ClickHeaderBartop();

            Step("13. Select it's parent geozone");
            realtimeControlPage.GeozoneTreeMainPanel.SelectNode(geozone);
            realtimeControlPage.WaitForStreetlightWidgetDisappeared();

            Step("14. Verify Control widget for streetlight disappears. No other widgets appear either.");
            VerifyEqual("14. Verify Control widget for streetlight disappears", false, realtimeControlPage.IsStreetlightWidgetDisplayed());
            VerifyEqual("14. Verify No other widgets appear either", false, realtimeControlPage.IsWidgetDisplayed());

            Step("15. Verify The parent geozone is being selected in geozone tree and no selection in map");
            VerifyEqual("15. Verify The parent geozone is being selected in geozone tree", geozone, realtimeControlPage.GeozoneTreeMainPanel.GetSelectedNodeName());
            VerifyTrue("[SC-691] 15. Verify no selection in map", !realtimeControlPage.Map.HasSelectedDevicesInMapGL(), "No devices selected", "Have devices selected");

            Step("16. Search with name of the streetlight using Search bar in geozone tree");
            realtimeControlPage.GeozoneTreeMainPanel.ChangeSearchAttribute("Name", "Contains");
            realtimeControlPage.GeozoneTreeMainPanel.EnterSearchTextInput(streetlightName);
            realtimeControlPage.GeozoneTreeMainPanel.ClickSearchButton();
            realtimeControlPage.WaitForPreviousActionComplete();

            Step("17. Verify Search results panel appears with the streetlight found");
            var searchResult = realtimeControlPage.GeozoneTreeMainPanel.SearchResultsGeozonePanel.GetListOfSearchResult("Equipment");
            VerifyEqual("17. Verify Search results panel appears with the streetlight found", true, searchResult.Exists(s => s.Equals(streetlightName)));

            Step("18. Select the streetlight from Search results panel");
            realtimeControlPage.GeozoneTreeMainPanel.SearchResultsGeozonePanel.SelectFoundDevice(streetlightName);
            realtimeControlPage.WaitForPreviousActionComplete();

            Step("19. Verify Control widget for streetlight appears");
            realtimeControlPage.WaitForStreetlightWidgetDisplayed(streetlightName);
            VerifyEqual("19. Verify Control widget for streetlight appears", true, realtimeControlPage.IsStreetlightWidgetDisplayed());

            Step("20. Verify The streetlight is being selected in found grid and map");
            VerifyEqual("20. Verify The streetlight is being selected in found grid", streetlightName, realtimeControlPage.GeozoneTreeMainPanel.SearchResultsGeozonePanel.GetSelectedEquipmentName());
            realtimeControlPage.Map.MoveToSelectedDeviceGL();
            VerifyEqual("20. Verify The streetlight is being selected in map", streetlightName, realtimeControlPage.Map.GetDeviceNameGL());
            realtimeControlPage.AppBar.ClickHeaderBartop();

            Step("21. Close the search results panel");
            realtimeControlPage.GeozoneTreeMainPanel.SearchResultsGeozonePanel.ClickBackButton();

            Step("22. Verify Search results panel disappears");
            realtimeControlPage.GeozoneTreeMainPanel.WaitForSearchResultPanelDisappeared();
            VerifyEqual("22. Verify Search results panel disappears", false, realtimeControlPage.GeozoneTreeMainPanel.IsSearchResultPanelDisplayed());

            Step("23. Verify The streetlight is being selected in geozone tree");
            VerifyEqual("23. Verify The streetlight is being selected in geozone tree", streetlightName, realtimeControlPage.GeozoneTreeMainPanel.GetSelectedNodeName());

            Step("24. Verify The streetlight is still being selected in map");
            realtimeControlPage.Map.MoveToSelectedDeviceGL();
            VerifyEqual("24. Verify The streetlight is being selected in map", streetlightName, realtimeControlPage.Map.GetDeviceNameGL());
            realtimeControlPage.AppBar.ClickHeaderBartop();

            Step("25. Verify Control widget for streetlight still shows");
            VerifyEqual("25. Verify Control widget for streetlight still shows", true, realtimeControlPage.IsStreetlightWidgetDisplayed());
        }

        [Test, DynamicRetry]
        [Description("RTC_24 Refresh devices on map manually")]
        [NonParallelizable]
        public void RTC_24()
        {
            var testData = GetTestDataOfRTC_24();
            var geozone = testData["Geozone"].ToString();
            var controller = testData["Controller"] as DeviceModel;
            var controllerName = controller.Name;
            var streetlights = testData["Streetlights"] as List<DeviceModel>;
            var tesingStreetlight = streetlights.PickRandom(3);
            var streetlight1 = tesingStreetlight[0];
            var streetlight2 = tesingStreetlight[1];
            var streetlight3 = tesingStreetlight[2];

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - Testing on 4 devices 1 Controller, 2 Streetlights in Real Time Control area");
            Step("  + Streetlight 01: is ON (Feedback and Command=100%)");
            Step("  + Streetlight 02: is OFF (Feedback and Command=0%)");
            Step("  + Streetlight 03: is partial on (Feedback and Command=50%)");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.RealTimeControl);
            var realtimeControlPage = desktopPage.GoToApp(App.RealTimeControl) as RealTimeControlPage;
            realtimeControlPage.GeozoneTreeMainPanel.SelectNode(geozone);
            Step("->  Set Streetlight 01: is ON (Feedback and Command=100%)");
            realtimeControlPage.GeozoneTreeMainPanel.SelectNode(streetlight1.Name);
            realtimeControlPage.StreetlightWidgetPanel.ExecuteCommand(RealtimeCommand.DimOn);
            realtimeControlPage.StreetlightWidgetPanel.WaitForCommandCompleted();
            Step("->  Set Streetlight 02: is OFF (Feedback and Command=0%)");
            realtimeControlPage.GeozoneTreeMainPanel.SelectNode(streetlight2.Name);
            realtimeControlPage.StreetlightWidgetPanel.ExecuteCommand(RealtimeCommand.DimOff);
            realtimeControlPage.StreetlightWidgetPanel.WaitForCommandCompleted();
            Step("->  Set Streetlight 03: is partial on (Feedback and Command=50%)");
            realtimeControlPage.GeozoneTreeMainPanel.SelectNode(streetlight3.Name);
            realtimeControlPage.StreetlightWidgetPanel.ExecuteCommand(RealtimeCommand.Dim50);
            realtimeControlPage.StreetlightWidgetPanel.WaitForCommandCompleted();

            Step("1. Go to Real-time Control app");
            Step("2. Expected Real-time Control page is routed and loaded successfully");
            desktopPage = Browser.RefreshLoggedInCMS();
            realtimeControlPage = desktopPage.GoToApp(App.RealTimeControl) as RealTimeControlPage;

            Step("3. Verify No any control widget appears");
            VerifyEqual("3. Verify No any control widget appears", false, realtimeControlPage.IsWidgetDisplayed());

            Step("4. Select a geozone from geozone tree");
            realtimeControlPage.GeozoneTreeMainPanel.SelectNode(geozone);

            Step("5. Hover mouse onto Earch icon in the map");
            realtimeControlPage.Map.MoveToGlobalEarthIcon();
            realtimeControlPage.Map.WaitForRealtimeRefreshPanelDisplayed();

            Step("6. Click Refresh");            
            realtimeControlPage.Map.ClickRealtimeRefreshButton();
            realtimeControlPage.Map.WaitForProgressGLCompleted();          

            Step("7. Verify Testing streetlights color is changed");
            Step(" - Streetlight 01: Yellow");
            Step(" - Streetlight 02: White");
            Step(" - Streetlight 03: 50% White and 50% Yellow");
            realtimeControlPage.Map.ZoomOutToGLLevel(ZoomGLLevel.m50);
            VerifyStreetlightDimmingLevel(realtimeControlPage, streetlight1.Longitude, streetlight1.Latitude, RealtimeCommand.DimOn);
            VerifyStreetlightDimmingLevel(realtimeControlPage, streetlight2.Longitude, streetlight2.Latitude, RealtimeCommand.DimOff);
            VerifyStreetlightDimmingLevel(realtimeControlPage, streetlight3.Longitude, streetlight3.Latitude, RealtimeCommand.Dim50);

            Step("8. Hover each testing streetlight");
            Step("9. Verify Check the level value of them");
            Step(" - Streetlight 01: 100%");
            Step(" - Streetlight 02: 0%");
            Step(" - Streetlight 03: 50%");
            realtimeControlPage.Map.MoveToDeviceGL(streetlight1.Longitude, streetlight1.Latitude);
            VerifyEqual("9. Verify the dimming level of Streetlight 01: 100%", "100%", realtimeControlPage.Map.GetDeviceLevelGL());
            realtimeControlPage.Map.MoveToDeviceGL(streetlight2.Longitude, streetlight2.Latitude);
            VerifyEqual("9. Verify the dimming level of Streetlight 02: 0%", "0%", realtimeControlPage.Map.GetDeviceLevelGL());
            realtimeControlPage.Map.MoveToDeviceGL(streetlight3.Longitude, streetlight3.Latitude);
            VerifyEqual("9. Verify the dimming level of Streetlight 03: 50%", "50%", realtimeControlPage.Map.GetDeviceLevelGL());
        }

        [Test, DynamicRetry]
        [Description("RTC_25 Refresh devices on map automatically")]
        [NonParallelizable]
        public void RTC_25()
        {
            var testData = GetTestDataOfRTC_25();
            var geozone = testData["Geozone"].ToString();
            var refreshMinute = int.Parse(testData["RefreshMinute"].ToString());
            var streetlights = testData["Streetlights"] as List<DeviceModel>;
            var testingStreetlight = streetlights.PickRandom(3);
            var streetlight1 = testingStreetlight[0];
            var streetlight2 = testingStreetlight[1];
            var streetlight3 = testingStreetlight[2];

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - Testing with 3 streetlights in Real Time Control area");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.RealTimeControl);

            Step("1. Go to Real-time Control app");
            Step("2. Expected Real-time Control page is routed and loaded successfully");
            var realtimeControlPage = desktopPage.GoToApp(App.RealTimeControl) as RealTimeControlPage;

            Step("3. Verify No any control widget appears");
            VerifyEqual("Verify No any control widget appears", false, realtimeControlPage.IsWidgetDisplayed());

            Step("4. Select a geozone from geozone tree");
            realtimeControlPage.GeozoneTreeMainPanel.SelectNode(geozone);

            Step("5. Hover mouse onto Earch icon in the map");
            realtimeControlPage.Map.MoveToGlobalEarthIcon();
            realtimeControlPage.Map.WaitForRealtimeRefreshPanelDisplayed();

            Step("6. Enter randomly {number of minutes: should be in range 1-3} for refreshing period then click Execute button");
            realtimeControlPage.Map.EnterRealtimeRefreshRateNumericInput(refreshMinute.ToString());
            realtimeControlPage.Map.ClickRealtimeExecuteAutoRefreshButton();
            realtimeControlPage.Map.WaitForProgressGLCompleted();

            Step("7. Open another tab and access the Real-time Control app and set the 3 testing streetlights as below");
            Step(" - Streetlight 01: ON (100% Feedback and Command)");
            Step(" - Streetlight 02: OFF (0% Feedback and Command)");
            Step(" - Streetlight 03: 50% Feedback and Command");
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var mainTabInstance = Browser.GetCurrentWindowHandles();
            var newTabInstance = Browser.OpenNewTab();
            var newDesktopPage = Browser.NavigateToLoggedInCMS();
            var newRealtimeControlPage = newDesktopPage.GoToApp(App.RealTimeControl) as RealTimeControlPage;
            newRealtimeControlPage.GeozoneTreeMainPanel.SelectNode(geozone);
            Step("->  Set Streetlight 01: is ON (Feedback and Command=100%)");
            newRealtimeControlPage.GeozoneTreeMainPanel.SelectNode(streetlight1.Name);
            newRealtimeControlPage.StreetlightWidgetPanel.ExecuteCommand(RealtimeCommand.DimOn);
            newRealtimeControlPage.StreetlightWidgetPanel.WaitForCommandCompleted();
            Step("->  Set Streetlight 02: is OFF (Feedback and Command=0%)");
            newRealtimeControlPage.GeozoneTreeMainPanel.SelectNode(streetlight2.Name);
            newRealtimeControlPage.StreetlightWidgetPanel.ExecuteCommand(RealtimeCommand.DimOff);
            newRealtimeControlPage.StreetlightWidgetPanel.WaitForCommandCompleted();
            Step("->  Set Streetlight 03: is partial on (Feedback and Command=50%)");
            newRealtimeControlPage.GeozoneTreeMainPanel.SelectNode(streetlight3.Name);
            newRealtimeControlPage.StreetlightWidgetPanel.ExecuteCommand(RealtimeCommand.Dim50);
            newRealtimeControlPage.StreetlightWidgetPanel.WaitForCommandCompleted();
            stopWatch.Stop();

            Step("8. After entered period, come back to the original tab and check the color of testing streetlight");
            Browser.SwitchTo(mainTabInstance);
            Wait.ForSeconds(refreshMinute * 60 + stopWatch.Elapsed.Seconds);

            Step("9. Verify Testing streetlights color is changed");
            Step(" - Streetlight 01: Yellow");
            Step(" - Streetlight 02: White");
            Step(" - Streetlight 03: 50% White and 50% Yellow");
            realtimeControlPage.Map.ZoomOutToGLLevel(ZoomGLLevel.m30);
            VerifyStreetlightDimmingLevel(realtimeControlPage, streetlight1.Longitude, streetlight1.Latitude, RealtimeCommand.DimOn);
            VerifyStreetlightDimmingLevel(realtimeControlPage, streetlight2.Longitude, streetlight2.Latitude, RealtimeCommand.DimOff);
            VerifyStreetlightDimmingLevel(realtimeControlPage, streetlight3.Longitude, streetlight3.Latitude, RealtimeCommand.Dim50);

            Step("10. Hover each testing streetlight");
            Step("11. Verify Check the level value of them");
            Step(" - Streetlight 01: 100%");
            Step(" - Streetlight 02: 0%");
            Step(" - Streetlight 03: 50%");
            realtimeControlPage.Map.MoveToDeviceGL(streetlight1.Longitude, streetlight1.Latitude);
            VerifyEqual("11. Verify the dimming level of Streetlight 01: 100%", "100%", realtimeControlPage.Map.GetDeviceLevelGL());
            realtimeControlPage.Map.MoveToDeviceGL(streetlight2.Longitude, streetlight2.Latitude);
            VerifyEqual("11. Verify the dimming level of Streetlight 02: 0%", "0%", realtimeControlPage.Map.GetDeviceLevelGL());
            realtimeControlPage.Map.MoveToDeviceGL(streetlight3.Longitude, streetlight3.Latitude);
            VerifyEqual("11. Verify the dimming level of Streetlight 03: 50%", "50%", realtimeControlPage.Map.GetDeviceLevelGL());

            Step("12. Go to the other tab and access the Real-time Control app and set the 3 testing streetlights as below");
            Step(" - Streetlight 01: OFF (0% Feedback and Command)");
            Step(" - Streetlight 02: ON (100% Feedback and Command)");
            Step(" - Streetlight 03: 30% Feedback and Command");
            stopWatch.Reset();
            Browser.SwitchTo(newTabInstance);
            Step("->  Set Streetlight 01: is OFF (Feedback and Command=0%)");
            newRealtimeControlPage.GeozoneTreeMainPanel.SelectNode(streetlight1.Name);
            newRealtimeControlPage.StreetlightWidgetPanel.ExecuteCommand(RealtimeCommand.DimOff);
            newRealtimeControlPage.StreetlightWidgetPanel.WaitForCommandCompleted();
            Step("->  Set Streetlight 02: is ON (Feedback and Command=100%)");
            newRealtimeControlPage.GeozoneTreeMainPanel.SelectNode(streetlight2.Name);
            newRealtimeControlPage.StreetlightWidgetPanel.ExecuteCommand(RealtimeCommand.DimOn);
            newRealtimeControlPage.StreetlightWidgetPanel.WaitForCommandCompleted();
            Step("->  Set Streetlight 03: is partial on (Feedback and Command=80%)");
            newRealtimeControlPage.GeozoneTreeMainPanel.SelectNode(streetlight3.Name);
            newRealtimeControlPage.StreetlightWidgetPanel.ExecuteCommand(RealtimeCommand.Dim80);
            newRealtimeControlPage.StreetlightWidgetPanel.WaitForCommandCompleted();
            stopWatch.Stop();

            Step("13. After entered period, come back to the original tab and check the color of testing streetlight");
            Browser.SwitchTo(mainTabInstance);
            Wait.ForSeconds(refreshMinute * 60 + stopWatch.Elapsed.Seconds);
            realtimeControlPage.Map.WaitForProgressGLCompleted();

            Step("14. Verify Testing streetlights color is changed");
            Step(" - Streetlight 01: White");
            Step(" - Streetlight 02: Yellow");
            Step(" - Streetlight 03: 20% White and 80% Yellow");
            VerifyStreetlightDimmingLevel(realtimeControlPage, streetlight1.Longitude, streetlight1.Latitude, RealtimeCommand.DimOff);
            VerifyStreetlightDimmingLevel(realtimeControlPage, streetlight2.Longitude, streetlight2.Latitude, RealtimeCommand.DimOn);
            VerifyStreetlightDimmingLevel(realtimeControlPage, streetlight3.Longitude, streetlight3.Latitude, RealtimeCommand.Dim80);

            Step("15. Hover each testing streetlight");
            Step("16. Verify Check the level value of them");
            Step(" - Streetlight 01: 0%");
            Step(" - Streetlight 02: 100%");
            Step(" - Streetlight 03: 80%");
            realtimeControlPage.Map.MoveToDeviceGL(streetlight1.Longitude, streetlight1.Latitude);
            VerifyEqual("16. Verify the dimming level of Streetlight 01: 0%", "0%", realtimeControlPage.Map.GetDeviceLevelGL());
            realtimeControlPage.Map.MoveToDeviceGL(streetlight2.Longitude, streetlight2.Latitude);
            VerifyEqual("16. Verify the dimming level of Streetlight 02: 100%", "100%", realtimeControlPage.Map.GetDeviceLevelGL());
            realtimeControlPage.Map.MoveToDeviceGL(streetlight3.Longitude, streetlight3.Latitude);
            VerifyEqual("16. Verify the dimming level of Streetlight 03: 80%", "80%", realtimeControlPage.Map.GetDeviceLevelGL());

            Step("17. Click Stop button");
            realtimeControlPage.Map.ZoomInToGLLevel(ZoomGLLevel.m20);
            realtimeControlPage.Map.MoveToGlobalEarthIcon();
            realtimeControlPage.Map.WaitForRealtimeRefreshPanelDisplayed();
            realtimeControlPage.Map.ClickRealtimeStopAutoRefreshButtonButton();

            Step("18. Go to the other tab and access the Real-time Control app and set the 3 testing streetlights as below");
            Step(" - Streetlight 01: ON (100% Feedback and Command)");
            Step(" - Streetlight 02: OFF (0% Feedback and Command)");
            Step(" - Streetlight 03: 50% Feedback and Command");
            stopWatch.Reset();
            Browser.SwitchTo(newTabInstance);
            Step("->  Set Streetlight 01: is ON (Feedback and Command=100%)");
            newRealtimeControlPage.GeozoneTreeMainPanel.SelectNode(streetlight1.Name);
            newRealtimeControlPage.StreetlightWidgetPanel.ExecuteCommand(RealtimeCommand.DimOn);
            newRealtimeControlPage.StreetlightWidgetPanel.WaitForCommandCompleted();
            Step("->  Set Streetlight 02: is OFF (Feedback and Command=0%)");
            newRealtimeControlPage.GeozoneTreeMainPanel.SelectNode(streetlight2.Name);
            newRealtimeControlPage.StreetlightWidgetPanel.ExecuteCommand(RealtimeCommand.DimOff);
            newRealtimeControlPage.StreetlightWidgetPanel.WaitForCommandCompleted();
            Step("->  Set Streetlight 03: is partial on (Feedback and Command=50%)");
            newRealtimeControlPage.GeozoneTreeMainPanel.SelectNode(streetlight3.Name);
            newRealtimeControlPage.StreetlightWidgetPanel.ExecuteCommand(RealtimeCommand.Dim50);
            newRealtimeControlPage.StreetlightWidgetPanel.WaitForCommandCompleted();
            stopWatch.Stop();

            Step("19. After entered period, come back to the original tab and check the color of testing streetlight");
            Browser.SwitchTo(mainTabInstance);
            Wait.ForSeconds(refreshMinute * 60 + stopWatch.Elapsed.Seconds);
            realtimeControlPage.Map.WaitForProgressGLCompleted();

            Step("20. Verify Testing streetlights color is not changed");
            Step(" - Streetlight 01: White");
            Step(" - Streetlight 02: Yellow");
            Step(" - Streetlight 03: 20% White and 80% Yellow");
            realtimeControlPage.Map.ZoomOutToGLLevel(ZoomGLLevel.m30);
            VerifyStreetlightDimmingLevel(realtimeControlPage, streetlight1.Longitude, streetlight1.Latitude, RealtimeCommand.DimOff);
            VerifyStreetlightDimmingLevel(realtimeControlPage, streetlight2.Longitude, streetlight2.Latitude, RealtimeCommand.DimOn);
            VerifyStreetlightDimmingLevel(realtimeControlPage, streetlight3.Longitude, streetlight3.Latitude, RealtimeCommand.Dim80);

            Step("21. Hover each testing streetlight");
            Step("22. Verify Check the level value of them");
            Step(" - Streetlight 01: 0%");
            Step(" - Streetlight 02: 100%");
            Step(" - Streetlight 03: 80%");
            realtimeControlPage.Map.MoveToDeviceGL(streetlight1.Longitude, streetlight1.Latitude);
            VerifyEqual("22. Verify the dimming level of Streetlight 01: 0%", "0%", realtimeControlPage.Map.GetDeviceLevelGL());
            realtimeControlPage.Map.MoveToDeviceGL(streetlight2.Longitude, streetlight2.Latitude);
            VerifyEqual("22. Verify the dimming level of Streetlight 02: 100%", "100%", realtimeControlPage.Map.GetDeviceLevelGL());
            realtimeControlPage.Map.MoveToDeviceGL(streetlight3.Longitude, streetlight3.Latitude);
            VerifyEqual("22. Verify the dimming level of Streetlight 03: 80%", "80%", realtimeControlPage.Map.GetDeviceLevelGL());
        }

        [Test, DynamicRetry]
        [Description("RTC_26 Support for 'Back to AUTO' at the dimming group level for OSCP gateways")]
        [NonParallelizable]
        public void RTC_26()
        {
            var testData = GetTestDataOfRTC_26();
            var geozone = testData["Geozone"].ToString();
            var controller = testData["Controller"] as DeviceModel;
            var controllerName = controller.Name;
            var streetlights = testData["Streetlights"] as List<DeviceModel>;
            streetlights = streetlights.Where(p => p.Controller.Equals(controllerName)).ToList();
            var dimmingGroups = streetlights.Select(p => p.DimmingGroup).Distinct().ToList();
            var dimmingGroup = dimmingGroups.PickRandom();
            var streetlightName = streetlights.Where(p => p.DimmingGroup.Equals(dimmingGroup)).PickRandom().Name;

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.RealTimeControl);

            Step("1. Go to Real-Time Control app and select the geozone: Real Time Control Area");
            var realtimeControlPage = desktopPage.GoToApp(App.RealTimeControl) as RealTimeControlPage;

            Step("2. Select the controller Smartsims");
            realtimeControlPage.GeozoneTreeMainPanel.SelectNode(geozone + @"\" + controllerName);

            Step("3. Press Information button to go to the back of the widget of Smartsims");
            realtimeControlPage.ControllerWidgetPanel.ClickInformationButton();
            realtimeControlPage.ControllerWidgetPanel.WaitForInformationWidgetPanelDisplayed();

            Step("4. Select randomly a group in \"Select a group\" dropdown list");
            realtimeControlPage.ControllerWidgetPanel.InformationWidgetPanel.SelectSelectGroupDropDown(dimmingGroup);

            Step("5. Select \"Reset back to automatic mode\" in \"Select a command\" dropdown list");
            realtimeControlPage.ControllerWidgetPanel.InformationWidgetPanel.SelectSelecCommandDropDown("Reset back to automatic mode");

            Step("6. Press Execute button and wait a few seconds to complete");
            realtimeControlPage.ControllerWidgetPanel.InformationWidgetPanel.ClickExecuteCommandButton();
            realtimeControlPage.ControllerWidgetPanel.WaitForCommandCompleted();

            Step("7. Select one of a streetlight using the testing dimming group");
            realtimeControlPage.GeozoneTreeMainPanel.SelectNode(streetlightName);
            realtimeControlPage.WaitForStreetlightWidgetDisplayed(streetlightName);

            Step("8. Verify The widget fo the streetlight displays with the status \"Automatic\", Feedback: 100%, Command: 100% and the light indicator is at ON");
            VerifyEqual("8. Verify Status text is AUTOMATIC", "AUTOMATIC", realtimeControlPage.StreetlightWidgetPanel.GetClockText());
            VerifyEqual("8. Verify Feedback is 100%", "100%", realtimeControlPage.StreetlightWidgetPanel.GetRealtimeFeedbackValueText());
            VerifyEqual("8. Verify Command is 100%", "100%", realtimeControlPage.StreetlightWidgetPanel.GetRealtimeCommandValueText());
            var commandData = realtimeControlPage.StreetlightWidgetPanel.CommandsDict[RealtimeCommand.DimOn];
            VerifyEqual("8. Verify Light indicator is at ON", commandData.TriangleValue, realtimeControlPage.StreetlightWidgetPanel.GetIndicatorCursorPositionValue());

            Step("9. Select the controller Smartsims and go to the back of the widget of Smartsims");
            realtimeControlPage.GeozoneTreeMainPanel.SelectNode(controllerName);
            var listOfCommandsText = realtimeControlPage.ControllerWidgetPanel.GetListOfCommandsText();
            listOfCommandsText.Remove("OFF");
            listOfCommandsText.Remove("ON");
            var randomCommand = listOfCommandsText.PickRandom();

            Step("10. Select the testing dimming group and select a percent number for a command");
            realtimeControlPage.ControllerWidgetPanel.ClickInformationButton();
            realtimeControlPage.ControllerWidgetPanel.WaitForInformationWidgetPanelDisplayed();
            realtimeControlPage.ControllerWidgetPanel.InformationWidgetPanel.SelectSelectGroupDropDown(dimmingGroup);
            realtimeControlPage.ControllerWidgetPanel.InformationWidgetPanel.SelectSelecCommandDropDown(randomCommand);

            Step("11. Press Execute button and wait a few seconds to complete");
            realtimeControlPage.ControllerWidgetPanel.InformationWidgetPanel.ClickExecuteCommandButton();
            realtimeControlPage.ControllerWidgetPanel.WaitForCommandCompleted();

            Step("12. Select one of a streetlight using the testing dimming group");
            realtimeControlPage.GeozoneTreeMainPanel.SelectNode(streetlightName);
            realtimeControlPage.WaitForStreetlightWidgetDisplayed(streetlightName);

            Step("13. Verify The widget of the streetlight displays with the status \"Manual\", Feedback: {the percent number}, Command: {the percent number} and the light indicator is at {the percent number}");
            var command = realtimeControlPage.StreetlightWidgetPanel.GetCommandByText(randomCommand);
            commandData = realtimeControlPage.StreetlightWidgetPanel.CommandsDict[command];
            VerifyEqual("13. Verify Status text is MANUAL", "MANUAL", realtimeControlPage.StreetlightWidgetPanel.GetClockText());
            VerifyEqual(string.Format("13. Verify Feedback is {0}", commandData.ValueText), commandData.ValueText, realtimeControlPage.StreetlightWidgetPanel.GetRealtimeFeedbackValueText());
            VerifyEqual(string.Format("13. Verify Command is {0}", commandData.ValueText), commandData.ValueText, realtimeControlPage.StreetlightWidgetPanel.GetRealtimeCommandValueText());
            VerifyEqual(string.Format("13. Verify Light indicator is at {0}", commandData.ValueText), commandData.TriangleValue, realtimeControlPage.StreetlightWidgetPanel.GetIndicatorCursorPositionValue());
        }

        [Test, DynamicRetry]
        [Description("RTC-27 - SC-782 - Cluster behaviors in Real Time Control")]
        [NonParallelizable]
        [Category("RunAlone")]
        public void RTC_27()
        {
            var testData = GetTestDataOfRTC_27();
            var geozone = testData["Geozone"].ToString();
            var controller = testData["Controller"] as DeviceModel;
            var controllerId = controller.Id;
            var controllerName = controller.Name;
            var streetlights = testData["ClusterStreetlights"] as List<DeviceModel>;
            var cluster1Streetlights = streetlights.Where(p => p.Cluster == "1").ToList();
            var cluster2Streetlights = streetlights.Where(p => p.Cluster == "2").ToList();

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - Go to Equipment Inventory app and make");
            Step("  + A cluster 01 of Telematics 01 & Telematics 02");
            Step("  + A cluster 02 of Telematics 03 & Telematics 04");
            Step(" - Go to Real Time Control app and set");
            Step("  + Telematics 01 & 02 mode: Automatic and ON (100% Feedback & Command)");
            Step("  + Telematics 03 & 04 mode: Manual and OFF (0% Feedback & Command)");
            Step("**** Precondition ****\n");

            //Delete temp devices are using Smartsims controller
            var devicesWithController = GetDevicesByControllerId(controllerId);
            var deviceInRTCA = GetDevicesByGeozone(geozone);
            var deletedDevices = devicesWithController.Where(p => !deviceInRTCA.ContainsValue(p.Value)).Select(p => p.Key);
            foreach (var device in deletedDevices)
            {
               DeleteDevice(controllerId, device);
            }

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.RealTimeControl);

            var realtimeControlPage = desktopPage.GoToApp(App.RealTimeControl) as RealTimeControlPage;
            realtimeControlPage.GeozoneTreeMainPanel.SelectNode(geozone);
            foreach (var streetlight in cluster1Streetlights)
            {
                var name = streetlight.Name;
                realtimeControlPage.GeozoneTreeMainPanel.SelectNode(streetlight.Name);
                realtimeControlPage.WaitForStreetlightWidgetDisplayed(name);
                realtimeControlPage.StreetlightWidgetPanel.ExecuteCommand(RealtimeCommand.DimOn);
                realtimeControlPage.StreetlightWidgetPanel.WaitForCommandCompleted();
            }
            foreach (var streetlight in cluster2Streetlights)
            {
                var name = streetlight.Name;
                realtimeControlPage.GeozoneTreeMainPanel.SelectNode(streetlight.Name);
                realtimeControlPage.WaitForStreetlightWidgetDisplayed(name);
                realtimeControlPage.StreetlightWidgetPanel.ExecuteCommand(RealtimeCommand.DimOff);
                realtimeControlPage.StreetlightWidgetPanel.WaitForCommandCompleted();
            }

            Step("1. Refresh the page and go to Real-Time Control app and select the geozone: Real Time Control Area");
            desktopPage = Browser.RefreshLoggedInCMS();
            realtimeControlPage = desktopPage.GoToApp(App.RealTimeControl) as RealTimeControlPage;
            realtimeControlPage.GeozoneTreeMainPanel.SelectNode(geozone);

            Step("2. Hover the Global icon and press Refresh button");
            realtimeControlPage.Map.MoveToGlobalEarthIcon();
            realtimeControlPage.Map.WaitForRealtimeRefreshPanelDisplayed();
            realtimeControlPage.Map.ClickRealtimeRefreshButton();
            realtimeControlPage.Map.WaitForProgressGLCompleted();

            Step("3. Verify The clusters are updated");
            Step(" - Cluster 01: YELLOW");
            Step(" - Cluster 02: WHITE");
            var cluster1Long = cluster1Streetlights.First().Longitude;
            var cluster1Lat = cluster1Streetlights.First().Latitude;
            realtimeControlPage.Map.MoveToDeviceGL(cluster1Long, cluster1Lat);
            var cluster1 = realtimeControlPage.Map.GetClusterSprite(cluster1Long, cluster1Lat);
            VerifyEqual("3. Verify Cluster 01: YELLOW", "100", cluster1.Status);

            var cluster2Long = cluster2Streetlights.First().Longitude;
            var cluster2Lat = cluster2Streetlights.First().Latitude;
            realtimeControlPage.Map.MoveToDeviceGL(cluster2Long, cluster2Lat);
            var cluster2 = realtimeControlPage.Map.GetClusterSprite(cluster2Long, cluster2Lat);
            VerifyEqual("3. Verify Cluster 02: WHITE", "0", cluster2.Status);

            Step("4. Press Cluster 01");
            realtimeControlPage.Map.SelectDeviceGL(cluster1Long, cluster1Lat);
            realtimeControlPage.Map.WaitForDeviceClusterPopupPanelDisplayed();

            Step("5. Verify The streetlight icons display in the grid with YELLOW color");
            var iconList = realtimeControlPage.Map.DeviceClusterPopupPanel.GetListOfIconType();
            VerifyEqual("5. Verify The streetlight icons display in the grid with YELLOW color", true, iconList.All(p => p.Contains("streetlight-ready-100.png")));

            Step("6. Select Telematics 01 and set the value 40% for Feedback & Command");
            var streetlight1 = cluster1Streetlights.First();
            realtimeControlPage.GeozoneTreeMainPanel.SelectNode(streetlight1.Name);
            realtimeControlPage.WaitForStreetlightWidgetDisplayed(streetlight1.Name);
            realtimeControlPage.StreetlightWidgetPanel.ExecuteCommand(RealtimeCommand.Dim40);
            realtimeControlPage.StreetlightWidgetPanel.WaitForCommandCompleted();

            Step("7. Verify");
            Step(" - The values of Feedback and Command are updated");
            Step(" - The streetlight icons in the grid is updated to Partial Yellow color");
            VerifyEqual("7. Verify The values of Feedback is updated", "40%", realtimeControlPage.StreetlightWidgetPanel.GetRealtimeFeedbackValueText());
            VerifyEqual("7. Verify The values of Command is updated", "40%", realtimeControlPage.StreetlightWidgetPanel.GetRealtimeCommandValueText());
            var icon = realtimeControlPage.Map.DeviceClusterPopupPanel.GetIconType(streetlight1.Name);
            VerifyEqual("7. Verify The streetlight icons in the grid is updated to Partial Yellow color", true, icon.Contains("streetlight-ready-40.png"));

            Step("8. Close the right panel and the cluster grid");
            realtimeControlPage.StreetlightWidgetPanel.ClickCloseButton();
            realtimeControlPage.WaitForStreetlightWidgetDisappeared();
            realtimeControlPage.Map.DeviceClusterPopupPanel.ClickCloseButton();
            realtimeControlPage.Map.WaitForDeviceClusterPopupPanelDisappeared();

            Step("9. Verify The color of cluster 01 is updated");
            Step(" - A half of White and a half of Yellow");
            cluster1 = realtimeControlPage.Map.GetClusterSprite(cluster1Long, cluster1Lat);
            VerifyEqual("3. Verify Cluster 01: A half of White and a half of Yellow", "mixed", cluster1.Status);

            Step("10. Press Cluster 02");
            realtimeControlPage.Map.SelectDeviceGL(cluster2Long, cluster2Lat);
            realtimeControlPage.Map.WaitForDeviceClusterPopupPanelDisplayed();

            Step("11. Verify The streetlight icons display WHITE color");
            iconList = realtimeControlPage.Map.DeviceClusterPopupPanel.GetListOfIconType();
            VerifyEqual("11. Verify The streetlight icons display WHITE color", true, iconList.All(p => p.Contains("streetlight-ready.png")));

            Step("12. Press Telematics 03 and press the Clock icon to change from MANUAL to AUTOMATIC");
            var streetlight3 = cluster2Streetlights.First();
            realtimeControlPage.GeozoneTreeMainPanel.SelectNode(streetlight3.Name);
            realtimeControlPage.WaitForStreetlightWidgetDisplayed(streetlight3.Name);
            realtimeControlPage.StreetlightWidgetPanel.ClickClockButton();
            realtimeControlPage.StreetlightWidgetPanel.WaitForCommandCompleted();

            Step("13. Verify");
            Step(" - The values of Feedback and Command are updated");
            Step(" - The streetlight icons in the grid is updated to Yellow color");
            VerifyEqual("13. Verify The values of Feedback is updated", "100%", realtimeControlPage.StreetlightWidgetPanel.GetRealtimeFeedbackValueText());
            VerifyEqual("13. Verify The values of Command is updated", "100%", realtimeControlPage.StreetlightWidgetPanel.GetRealtimeCommandValueText());
            icon = realtimeControlPage.Map.DeviceClusterPopupPanel.GetIconType(streetlight3.Name);
            VerifyEqual("13. Verify The streetlight icons in the grid is updated to Yellow color", true, icon.Contains("streetlight-ready-100.png"));

            Step("14. Close the right panel and the cluster grid");
            realtimeControlPage.StreetlightWidgetPanel.ClickCloseButton();
            realtimeControlPage.WaitForStreetlightWidgetDisappeared();
            realtimeControlPage.Map.DeviceClusterPopupPanel.ClickCloseButton();
            realtimeControlPage.Map.WaitForDeviceClusterPopupPanelDisappeared();

            Step("15. Verify The color of cluster 02 is updated");
            Step(" - A half of White and a half of Yellow");
            cluster2 = realtimeControlPage.Map.GetClusterSprite(cluster2Long, cluster2Lat);
            VerifyEqual("3. Verify Cluster 02: A half of White and a half of Yellow", "mixed", cluster2.Status);

            Step("16. Select the Smartsims controller and press ON, then close the panel and press Refresh button on the Global icon");
            realtimeControlPage.GeozoneTreeMainPanel.SelectNode(controllerName);
            realtimeControlPage.WaitForControllerWidgetDisplayed(controllerName);
            realtimeControlPage.ControllerWidgetPanel.ExecuteCommand(RealtimeCommand.DimOn);
            realtimeControlPage.ControllerWidgetPanel.WaitForCommandCompleted();
            realtimeControlPage.ControllerWidgetPanel.ClickCloseButton();
            realtimeControlPage.WaitForStreetlightWidgetDisappeared();
            realtimeControlPage.Map.MoveToGlobalEarthIcon();
            realtimeControlPage.Map.WaitForRealtimeRefreshPanelDisplayed();
            realtimeControlPage.Map.ClickRealtimeRefreshButton();
            realtimeControlPage.Map.WaitForProgressGLCompleted();

            Step("17. Verify All 2 clusters are update to Yellow color.");
            cluster1 = realtimeControlPage.Map.GetClusterSprite(cluster1Long, cluster1Lat);
            VerifyEqual("17. Verify Cluster 01: Yellow color", "100", cluster1.Status);
            cluster2 = realtimeControlPage.Map.GetClusterSprite(cluster2Long, cluster2Lat);
            VerifyEqual("17. Verify Cluster 02: Yellow color", "100", cluster2.Status);

            Step("18. Select the Smartsims controller and press OFF, then close the panel and press Refresh button on the Global icon");
            realtimeControlPage.GeozoneTreeMainPanel.SelectNode(controllerName);
            realtimeControlPage.WaitForControllerWidgetDisplayed(controllerName);
            realtimeControlPage.ControllerWidgetPanel.ExecuteCommand(RealtimeCommand.DimOff);
            realtimeControlPage.ControllerWidgetPanel.WaitForCommandCompleted();
            realtimeControlPage.ControllerWidgetPanel.ClickCloseButton();
            realtimeControlPage.WaitForStreetlightWidgetDisappeared();
            realtimeControlPage.Map.MoveToGlobalEarthIcon();
            realtimeControlPage.Map.WaitForRealtimeRefreshPanelDisplayed();
            realtimeControlPage.Map.ClickRealtimeRefreshButton();
            realtimeControlPage.Map.WaitForProgressGLCompleted();

            Step("19. Verify All 2 clusters are update to WHITE color.");
            cluster1 = realtimeControlPage.Map.GetClusterSprite(cluster1Long, cluster1Lat);
            VerifyEqual("19. Verify Cluster 01: WHITE color", "0", cluster1.Status);
            cluster2 = realtimeControlPage.Map.GetClusterSprite(cluster2Long, cluster2Lat);
            VerifyEqual("19. Verify Cluster 02: WHITE color", "0", cluster2.Status);

            Step("20. Select the Smartsims controller and set a value for Feedback and Command, then close the panel and press Refresh button on the Global icon");
            realtimeControlPage.GeozoneTreeMainPanel.SelectNode(controllerName);
            realtimeControlPage.WaitForControllerWidgetDisplayed(controllerName);
            var dimming = realtimeControlPage.ControllerWidgetPanel.ExecuteRandomDimming();
            realtimeControlPage.ControllerWidgetPanel.WaitForCommandCompleted();
            realtimeControlPage.ControllerWidgetPanel.ClickCloseButton();
            realtimeControlPage.WaitForStreetlightWidgetDisappeared();
            realtimeControlPage.Map.MoveToGlobalEarthIcon();
            realtimeControlPage.Map.WaitForRealtimeRefreshPanelDisplayed();
            realtimeControlPage.Map.ClickRealtimeRefreshButton();
            realtimeControlPage.Map.WaitForProgressGLCompleted();

            Step("21. Verify All 2 clusters are update to a half of White and a half of Yellow.");
            cluster1 = realtimeControlPage.Map.GetClusterSprite(cluster1Long, cluster1Lat);
            VerifyEqual("[SC-2021] 21. Verify Cluster 01: A half of White and a half of Yellow", "mixed", cluster1.Status);
            cluster2 = realtimeControlPage.Map.GetClusterSprite(cluster2Long, cluster2Lat);
            VerifyEqual("[SC-2021] 21. Verify Cluster 02: A half of White and a half of Yellow", "mixed", cluster2.Status);

            Step("22. Press Cluster 02");
            realtimeControlPage.Map.SelectDeviceGL(cluster2Long, cluster2Lat);
            realtimeControlPage.Map.WaitForDeviceClusterPopupPanelDisplayed();
            
            Step("23. Verify - The streetlight icons in the grid is displayed as Partial Yellow color");           
            iconList = realtimeControlPage.Map.DeviceClusterPopupPanel.GetListOfIconType();
            VerifyEqual("23. The streetlight icons in the grid is displayed as Partial Yellow color", true, iconList.All(p => Regex.IsMatch(p, "streetlight-ready-([0-9]*).png")));
        }

        [Test, DynamicRetry]
        [Description("RTC_28 - SC-782 - Multi-selection in Real Time Control")]
        [NonParallelizable]
        public void RTC_28()
        {
            var testData = GetTestDataOfRTC_28();
            var geozone = testData["Geozone"].ToString();
            var controller = testData["Controller"] as DeviceModel;
            var controllerName = controller.Name;
            var streetlights = testData["Streetlights"] as List<DeviceModel>;
            var clusterStreetlights = testData["ClusterStreetlights"] as List<DeviceModel>;
            var cluster1Streetlights = clusterStreetlights.Where(p => p.Cluster == "1").ToList();
            var separeatedStreetlight = streetlights.First();

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - Go to Equipment Inventory app and make");
            Step("  + A cluster 01 of Telematics 01 & Telematics 02");
            Step(" - Go to Real Time Control app and set");
            Step("  + Telematics 01 & 02 mode: Automatic and ON (100% Feedback & Command)");
            Step("  + Telematics 03 mode: Manual and OFF (0% Feedback & Command)");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.RealTimeControl);
            var realtimeControlPage = desktopPage.GoToApp(App.RealTimeControl) as RealTimeControlPage;
            realtimeControlPage.GeozoneTreeMainPanel.SelectNode(geozone);
            foreach (var streetlight in cluster1Streetlights)
            {
                var name = streetlight.Name;
                realtimeControlPage.GeozoneTreeMainPanel.SelectNode(name);
                realtimeControlPage.WaitForStreetlightWidgetDisplayed(name);
                realtimeControlPage.StreetlightWidgetPanel.ClickClockButton();
                realtimeControlPage.StreetlightWidgetPanel.WaitForCommandCompleted();
            }

            realtimeControlPage.GeozoneTreeMainPanel.SelectNode(separeatedStreetlight.Name);
            realtimeControlPage.WaitForStreetlightWidgetDisplayed(separeatedStreetlight.Name);
            realtimeControlPage.StreetlightWidgetPanel.ExecuteCommand(RealtimeCommand.DimOff);
            realtimeControlPage.StreetlightWidgetPanel.WaitForCommandCompleted();            

            Step("1. Refresh the page and go to Real-Time Control app and select the geozone: Real Time Control Area");
            desktopPage = Browser.RefreshLoggedInCMS();
            realtimeControlPage = desktopPage.GoToApp(App.RealTimeControl) as RealTimeControlPage;
            realtimeControlPage.GeozoneTreeMainPanel.SelectNode(geozone);

            Step("2. Press Shift key and select a cluster 01 and Telematics 03");
            var cluster1Long = cluster1Streetlights.First().Longitude;
            var cluster1Lat = cluster1Streetlights.First().Latitude;
            realtimeControlPage.Map.SelectDeviceGL(cluster1Long, cluster1Lat);
            realtimeControlPage.Map.WaitForDeviceClusterPopupPanelDisplayed();
            realtimeControlPage.Map.DragAndDropDeviceClusterPopupPanel(200, 200);
            realtimeControlPage.Map.SelectDevicesGL(separeatedStreetlight.Longitude, separeatedStreetlight.Latitude);
            realtimeControlPage.Map.WaitForDeviceClusterPopupPanelDisappeared();
            
            Step("3. Verify Control widget for streetlights appears");
            var isMultipleWidgetDisplayed = realtimeControlPage.IsMultipleStreetlightsWidgetDisplayed();
            VerifyEqual("[SC-1887] 3. Verify Control widget for streetlights appears", true, isMultipleWidgetDisplayed);
            if (isMultipleWidgetDisplayed)
            {
                Step("4. Verify Control widget for streetlights has:");
                Step(" • Header bar:");
                Step("  o Refresh button");
                Step("  o 'Checked Status': icon");
                Step("  o 'MANUAL's button with clock icon");
                Step("  o Close button");
                var clockText = realtimeControlPage.MultipleStreetlightsWidgetPanel.GetClockText();
                VerifyEqual("4. Verify Refresh button is visible", true, realtimeControlPage.MultipleStreetlightsWidgetPanel.IsRefreshButtonVisible());                
                VerifyEqual("4. Verify Status icon is Ok", true, realtimeControlPage.MultipleStreetlightsWidgetPanel.IsStatusIconOkDisplayed());
                VerifyEqual("4. Verify Clock button is visible", true, realtimeControlPage.MultipleStreetlightsWidgetPanel.IsClockButtonVisible());
                VerifyEqual("4. Verify 'MANUAL' button with clock icon", "MANUAL", clockText);
                VerifyEqual("4. Verify Close button is visible", true, realtimeControlPage.MultipleStreetlightsWidgetPanel.IsCloseButtonVisible());
                
                Step(" • Dimming levels bar:");
                Step("   o ON");
                Step("   o OFF");
                Step("   o No other dimming command buttons");
                Step("   o Indicator (black triangle)");
                var expectedCommands = new List<string> { "ON", "OFF" };
                var actualCommands = realtimeControlPage.MultipleStreetlightsWidgetPanel.GetListOfCommandsText();
                VerifyEqual("4. Verify Dimming levels as expected", expectedCommands, actualCommands, false);
                VerifyEqual("4. Verify Indicator (black triangle) is visible", true, realtimeControlPage.MultipleStreetlightsWidgetPanel.IsIndicatorCursorVisible());

                Step(" • 'Feedback': (total values of Feedback of all streetlights)/total number of streetlights + '%'");
                VerifyEqual("4. Verify label is 'Feedback'", "Feedback", realtimeControlPage.MultipleStreetlightsWidgetPanel.GetRealtimeFeedbackText());
                VerifyEqual("4. Verify value is (total values of Feedback of all streetlights)/total number of streetlights + '%'", "50%", realtimeControlPage.MultipleStreetlightsWidgetPanel.GetRealtimeFeedbackValueText());

                Step(" • 'Command': (total values of Command of all streetlights)/total number of streetlights + '%'");
                VerifyEqual("4. Verify label is 'Command'", "Command", realtimeControlPage.MultipleStreetlightsWidgetPanel.GetRealtimeCommandText());
                VerifyEqual("4. Verify value is (total values of Command of all streetlights)/total number of streetlights + '%'", "50%", realtimeControlPage.MultipleStreetlightsWidgetPanel.GetRealtimeCommandValueText());

                Step(" • Streetlight icon and name at bottom left");
                VerifyEqual("4. Verify Streetlight icon and name at bottom left", true, realtimeControlPage.MultipleStreetlightsWidgetPanel.CheckIfDeviceIcon(DeviceType.Streetlight));

                Step(" • Local time at bottom right: value: format 'hh:mm:ss'");
                VerifyEqual("4. Verify Local time at bottom right: value: format 'hh:mm:ss'", true, Regex.IsMatch(realtimeControlPage.MultipleStreetlightsWidgetPanel.GetLastUpdateTimeText(), @"\d{2}:\d{2}:\d{2}"));
            }
        }

        [Test, DynamicRetry]
        [Description("SC-1968 - Real-Time Control- Some streetlamp icons vanish when clicked on")]
        public void SC_1968()
        {
            var testData = GetTestDataOfSC_1968();
            var controllerId = testData["ControllerId"];
            var controllerName = testData["ControllerName"];
            var controllerGeozone = testData["ControllerGeozone"];
            var geozone = SLVHelper.GenerateUniqueName("GZNSC1968");
            var failureStreetlight = string.Format("STL{0}-lampfailure", SLVHelper.GenerateStringInteger(999999));
            var streetlight = SLVHelper.GenerateUniqueName("STL");
            var controlProgramName = SLVHelper.GenerateUniqueName("CPSC1968");
            var calendarName = SLVHelper.GenerateUniqueName("CSC1968");
            var failureStreetlightLat = SLVHelper.GenerateCoordinate("11.94117", "11.94176"); 
            var failureStreetlightLong = SLVHelper.GenerateCoordinate("106.64352", "106.64405"); 
            var streetlightLat = SLVHelper.GenerateCoordinate("11.94122", "11.94178");
            var streetlightLong = SLVHelper.GenerateCoordinate("106.64428", "106.64474");
            var typeOfEquipment = "Telematics LCU[Lamp]";

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - Create a testing geozone with a new streetlight using Smartsims commission");
            Step("  + Identifier: random id and ends with '-lampfailure'");
            Step("  + Type of Equipment: Telematics LCU[Lamp]");
            Step("  + Controller: Smartsims commission");
            Step("  + Dimming group: select the testing calendar");
            Step(" - Create a testing control program with");
            Step("  + Template: Advanced Mode");
            Step("  + Remove all triangle icons on the graph by pressing and holding it 3s");
            Step("  + Remove 1 white circle icon on the graph");
            Step("  + Drag remained white circle icon to (0%, 12:00 AM)");
            Step("**** Precondition ****\n");

            Step("-> Create data for testing");
            DeleteGeozones("GZNSC1968*");
            CreateNewControlProgram(controlProgramName, "T_12_12", "Automated control program for SC-1968", new List<string>(), new List<string> { "0#0:0:0" });
            CreateNewCalendar(calendarName, "Automated calendar for SC-1968");
            CreateNewGeozone(geozone, latMin: "11.94057", latMax: "11.94219", lngMin: "106.64154", lngMax: "106.64669");
            CreateNewDevice(DeviceType.Streetlight, streetlight, controllerId, geozone, typeOfEquipment, lat: streetlightLat, lng: streetlightLong);
            CreateNewDevice(DeviceType.Streetlight, failureStreetlight, controllerId, geozone, typeOfEquipment, lat: failureStreetlightLat, lng: failureStreetlightLong);
            SetValueToDevice(controllerId, failureStreetlight, "DimmingGroupName", calendarName, Settings.GetCurrentControlerDateTime(controllerId).AddMinutes(10));

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.RealTimeControl, App.EquipmentInventory, App.SchedulingManager);

            Step("1. Got to Scheduling Manager, select Calendar tab and choose the testing calendar");           
            var schedulingManagerPage = desktopPage.GoToApp(App.SchedulingManager) as SchedulingManagerPage;
            schedulingManagerPage.SchedulingManagerPanel.SelectTab("Calendar");
            schedulingManagerPage.WaitForPreviousActionComplete();
            schedulingManagerPage.SchedulingManagerPanel.SelectCalendar(calendarName);
            schedulingManagerPage.WaitForPreviousActionComplete();            

            Step("2. In the calendar editor, select the current date on a calendar (make sure to know the timezone of testing controller to select the correct current date), and in Control Program pop-up, select the testing control program. Save change on the pop-up, and save change on the calendar editor");
            var currentCtrlDateTime = Settings.GetCurrentControlerDateTime(controllerId);
            var currentDate = currentCtrlDateTime.ToString("M/d/yyyy");
            schedulingManagerPage.CalendarEditorPanel.ClickCalendarDate(currentDate);
            schedulingManagerPage.WaitForPopupDialogDisplayed();
            schedulingManagerPage.CalendarControlProgramsPopupPanel.SelectItem(controlProgramName);
            schedulingManagerPage.CalendarControlProgramsPopupPanel.ClickSaveButton();
            schedulingManagerPage.WaitForPopupDialogDisappeared();

            Step("3. Switch to Equipment Inventory > testing geozone > select Smartsim controller, press Commission button on the Controller panel with all options are checked in Commission panel, press Launch button");
            var equipmentInventoryPage = schedulingManagerPage.AppBar.SwitchTo(App.EquipmentInventory) as EquipmentInventoryPage;
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(controllerGeozone + @"\"+ controllerName);
            equipmentInventoryPage.WaitForDeviceEditorPanelDisplayed();
            equipmentInventoryPage.ControllerEditorPanel.ClickCommissionButton();
            equipmentInventoryPage.ControllerEditorPanel.WaitForCommissionPanelDisplayed();
            equipmentInventoryPage.WaitForPreviousActionComplete();
            equipmentInventoryPage.ControllerEditorPanel.CommissionPanel.CheckAllSettings();
            equipmentInventoryPage.ControllerEditorPanel.CommissionPanel.ClickLaunchButton();
            equipmentInventoryPage.WaitForPreviousActionComplete();
            equipmentInventoryPage.ControllerEditorPanel.CommissionPanel.WaitForLaunchButtonDisappeared();

            Step("4. Switch to Real Time Control app, then select the testing streetlight");
            var realTimeControlPage = schedulingManagerPage.AppBar.SwitchTo(App.RealTimeControl) as RealTimeControlPage;
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(geozone);

            Step("5. Verify The streetlight icon still displays on the map");
            var streetlightInMap = realTimeControlPage.Map.MoveAndGetDeviceNameGL(failureStreetlightLong, failureStreetlightLat);
            VerifyEqual("5. Verify The streetlight icon still displays on the map", failureStreetlight, streetlightInMap);

            Step("6. Select another streetlight on the map");
            realTimeControlPage.GeozoneTreeMainPanel.SelectNode(streetlight);
            equipmentInventoryPage.Map.SelectDeviceGL(streetlightLong, streetlightLat);
            realTimeControlPage.WaitForStreetlightWidgetDisplayed(streetlight);

            Step("7. Verify The selected streetlight icon still displays on the map");
            streetlightInMap = equipmentInventoryPage.Map.GetDeviceNameGL();
            VerifyEqual("7. Verify The selected streetlight icon still displays on the map", streetlight, streetlightInMap);

            Step("8. Select again the testing streetlight icon on the map");
            realTimeControlPage.GeozoneTreeMainPanel.SelectNode(failureStreetlight);
            realTimeControlPage.WaitForStreetlightWidgetDisplayed(failureStreetlight);

            Step("9. Verify The streetlight icon still displays on the map");
            streetlightInMap = realTimeControlPage.Map.MoveAndGetDeviceNameGL(failureStreetlightLong, failureStreetlightLat);
            VerifyEqual("9. Verify The streetlight icon still displays on the map", failureStreetlight, streetlightInMap);

            try
            {
                DeleteGeozone(geozone);
                DeleteControlProgram(controlProgramName);
                DeleteCalendar(calendarName);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("RTC_29 1061890 Location Search - in Real Time Control")]
        public void RTC_29()
        {
            var testData = GetTestDataOfRTC_29();
            var latMin = testData["LatMin"];
            var latMax = testData["LatMax"];
            var lngMin = testData["LngMin"];
            var lngMax = testData["LngMax"];
            var partialAddress = testData["PartialAddress"];
            var fullAddress = testData["FullAddress"];
            var geozone = SLVHelper.GenerateUniqueName("GZNRTC29");
            var controller = SLVHelper.GenerateUniqueName("CLRTC29");
            var streetlight = SLVHelper.GenerateUniqueName("SLRTC29");

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - Create a testing geozone containing a streetlight which is located at a well-known location. Ex: Champ de Mars, 5 Avenue Anatole France, 75007 Paris, France");
            Step("**** Precondition ****\n");

            Step("-> Create data for testing");
            DeleteGeozones("GZNRTC29*");
            var streetlightLat = SLVHelper.GenerateCoordinate("48.26468", "48.26670");
            var streetlightLng = SLVHelper.GenerateCoordinate("2.69184", "2.69438");
            CreateNewGeozone(geozone, latMin: latMin, latMax: latMax, lngMin: lngMin, lngMax: lngMax);
            CreateNewController(controller, geozone, lat: SLVHelper.GenerateCoordinate(latMin, latMax), lng: SLVHelper.GenerateCoordinate(lngMin, lngMax));
            CreateNewDevice(DeviceType.Streetlight, streetlight, controller, geozone, lat: streetlightLat, lng: streetlightLng);

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.RealTimeControl);

            Step("1. Go to Real Time Control app");
            var realTimeControlPage = desktopPage.GoToApp(App.RealTimeControl) as RealTimeControlPage;

            Step("2. Verify There is a button with icon: Globe icon on the top-right corner of the GeoZone tree");
            VerifyEqual("2. Verify There is a button with icon: Globe icon on the top-right corner of the GeoZone tree", true, realTimeControlPage.GeozoneTreeMainPanel.IsMapFilterButtonVisible());

            Step("3. Hover the button");
            realTimeControlPage.GeozoneTreeMainPanel.HoverMapSearchButton();

            Step("4. Verify The text 'Map Search' displays");
            VerifyEqual("4. Verify The text 'Map Search' displays", "Map Search", realTimeControlPage.GeozoneTreeMainPanel.GetMapSearchButtonTooltip());

            Step("5. Click the button");
            realTimeControlPage.GeozoneTreeMainPanel.ClickMapSearchButton();
            realTimeControlPage.GeozoneTreeMainPanel.WaitForMapSearchPanelDisplayed();

            Step("6. Verify A panel displays with");
            Step(" o Title: Map Search");
            Step(" o Text: Search by Location");
            Step(" o Textbox with a Magnifying Glass icon and the text 'Search in map'");
            Step(" o Button: Back");
            VerifyEqual("6. Verify A panel displays: Title: Map Search", "Map Search", realTimeControlPage.GeozoneTreeMainPanel.MapSearchPanel.GetPanelTitleText());
            VerifyEqual("6. Verify A panel displays: Text: Search by Location", "Search by Location", realTimeControlPage.GeozoneTreeMainPanel.MapSearchPanel.GetContentText());
            VerifyEqual("6. Verify A panel displays: Textbox", true, realTimeControlPage.GeozoneTreeMainPanel.MapSearchPanel.IsSearchInputDisplayed());
            VerifyEqual("6. Verify A panel displays: Textbox with a Magnifying Glass icon", true, realTimeControlPage.GeozoneTreeMainPanel.MapSearchPanel.IsSearchInputHasMagnifyingGlass());
            VerifyEqual("6. Verify A panel displays: Textbox with the text 'Search in map'", "Search in map", realTimeControlPage.GeozoneTreeMainPanel.MapSearchPanel.GetSearchPlaceholder());
            VerifyEqual("6. Verify A panel displays: Button: Back", true, realTimeControlPage.GeozoneTreeMainPanel.MapSearchPanel.IsBackButtonDisplayed());

            Step("7. Enter a partial of the testing address into the input");
            realTimeControlPage.GeozoneTreeMainPanel.MapSearchPanel.EnterSearchInput(partialAddress);
            realTimeControlPage.GeozoneTreeMainPanel.MapSearchPanel.WaitForSuggestionsDisplayed();

            Step("8. Verify The search results appear as user types. The matched words are bold");
            var searchSuggestionsBoldTextList = realTimeControlPage.GeozoneTreeMainPanel.MapSearchPanel.GetSearchSuggestionsBoldText();
            VerifyTrue("8. Verify The search results appear as user types. The matched words are bold", searchSuggestionsBoldTextList.All(p => p.Equals(partialAddress)), "all matched words are bold", string.Join(", ", searchSuggestionsBoldTextList));

            Step("9. Input the full value of the testing address, then click on the 1st result in the list");
            realTimeControlPage.GeozoneTreeMainPanel.MapSearchPanel.ClickClearSearchButton();
            realTimeControlPage.GeozoneTreeMainPanel.MapSearchPanel.WaitForClearSearchButtonDisappeared();
            realTimeControlPage.GeozoneTreeMainPanel.MapSearchPanel.EnterSearchInput(fullAddress);
            realTimeControlPage.GeozoneTreeMainPanel.MapSearchPanel.WaitForSuggestionsDisplayed();
            realTimeControlPage.GeozoneTreeMainPanel.MapSearchPanel.SelectSearchSuggestion();

            Step("10. Verify The map is centered on the selected location and zoomed to level 15(50 m)");
            Wait.ForGLMapStopFlying();
            VerifyEqual("10. Verify Map Search panel remains visible", "50 m", realTimeControlPage.Map.GetMapGLScaleText());

            Step("11. Verify There is an Orange location icon on the center of the map");
            VerifyEqual("11. Verify There is location icon on the center of the map", true, realTimeControlPage.Map.IsLocationSearchMarkerDisplayed());
            VerifyEqual("11. Verify There is Orange icon", true, realTimeControlPage.Map.GetLocationSearchMarkerImageSrc().Contains("marker-generic.svg"));

            Step("12. Select the streetlight on the map");
            realTimeControlPage.Map.SelectDeviceGL(streetlightLng, streetlightLat);
            realTimeControlPage.WaitForStreetlightWidgetDisplayed(streetlight);

            Step("13. Verify The device is selected and the Map Search panel remains visible");
            VerifyEqual("13. Verify The device is selected", true, realTimeControlPage.Map.HasSelectedDevicesInMapGL());
            VerifyEqual("13. Verify Map Search panel remains visible", true, realTimeControlPage.GeozoneTreeMainPanel.IsMapSearchPanelDisplayed());

            Step("14. Verify The Light Point Controller widget displays on the right");
            VerifyEqual("14. Verify The Light Point Controller widget displays on the right", true, realTimeControlPage.IsStreetlightWidgetDisplayed());

            Step("15. Press X icon on the Search box");
            realTimeControlPage.GeozoneTreeMainPanel.MapSearchPanel.ClickClearSearchButton();
            realTimeControlPage.GeozoneTreeMainPanel.MapSearchPanel.WaitForClearSearchButtonDisappeared();

            Step("16. Verify The input is cleared");
            VerifyEqual("16. Verify The input is cleared", "", realTimeControlPage.GeozoneTreeMainPanel.MapSearchPanel.GetSearchValue());

            Step("17. Press Back button");
            realTimeControlPage.GeozoneTreeMainPanel.MapSearchPanel.ClickBackToolbarButton();
            realTimeControlPage.GeozoneTreeMainPanel.WaitForMapSearchPanelDisappeared();

            Step("18. Verify Geozone tree panel displays again");
            Step(" o The streetlight is selected in the geozone tree");
            Step(" o The Light Point Controller widget remains visible");
            VerifyEqual("18. Verify The streetlight is selected in the geozone tree", streetlight, realTimeControlPage.GeozoneTreeMainPanel.GetSelectedNodeName());
            VerifyEqual("18. Verify The Light Point Controller widget remains visible", true, realTimeControlPage.IsStreetlightWidgetDisplayed());

            try
            {
                DeleteGeozone(geozone);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("RTC - 1306572 - Bug Id 1306564 - Icon missing for streetlight device with a failure and turned off in a cluster")]
        public void RTC_1306572()
        {            
            var testData = GetTestDataOfRTC_1306572();
            var controllerId = testData["ControllerId"];
            var controllerName = testData["ControllerName"];
            var geozone = SLVHelper.GenerateUniqueName("GZNRTC1306572");
            var normalStreetlight = SLVHelper.GenerateUniqueName("STL");
            var failureStreetlight = SLVHelper.GenerateUniqueName("STL") + "-lampfailure";
            var calendar = SLVHelper.GenerateUniqueName("CRTC1306572");
            var controlProgram = SLVHelper.GenerateUniqueName("CPRTC1306572");
            var typeOfEquipment = "Telematics LCU[Lamp]";

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - Create a testing geozone containing a cluster having 2 streetlights connected to Smartsims controller and each streetlight has a valid Unique Mac Address");
            Step("  + 1st streetlight works normally");
            Step("  + 2nd streetlight has a critical failure (Lamp Failure). Note: set the suffix '-lampfailure' for the streetlight's identifier to make a critical failure");
            Step(" - 2 streetlights are in Automation mode (ON with 100% Feedback and Command)");
            Step("**** Precondition ****\n");

            Step("-> Create data for testing");
            DeleteGeozones("GZNRTC1306572*");
            var calendarId = CreateNewCalendar(calendar);
            var controlProgramId = CreateNewControlProgram(controlProgram, "T_00_24", SLVHelper.GenerateHexColor(), "", "always-on");
            AddEventControlProgram(controlProgramId, 100, 0, 0, 0);
            AssociateControlProgramToCalendarYearly(controlProgramId, calendarId, 1, 31, 12, 31);
            
            var latCluster = SLVHelper.GenerateCoordinate("36.44743", "36.44829");
            var lngCluster = SLVHelper.GenerateCoordinate("113.84951", "113.85062");
            CreateNewGeozone(geozone, latMin: "36.44630", latMax: "36.44918", lngMin: "113.84636", lngMax: "113.85389");
            CreateNewDevice(DeviceType.Streetlight, normalStreetlight, controllerId, geozone, typeOfEquipment, lat: latCluster, lng: lngCluster);
            CreateNewDevice(DeviceType.Streetlight, failureStreetlight, controllerId, geozone, typeOfEquipment, lat: latCluster, lng: lngCluster);
            SetValueToDevice(controllerId, normalStreetlight, "MacAddress", SLVHelper.GenerateMACAddress(), Settings.GetServerTime());
            SetValueToDevice(controllerId, failureStreetlight, "MacAddress", SLVHelper.GenerateMACAddress(), Settings.GetServerTime());
            SetValueToDevice(controllerId, normalStreetlight, "DimmingGroupName", calendar, Settings.GetServerTime());
            SetValueToDevice(controllerId, failureStreetlight, "DimmingGroupName", calendar, Settings.GetServerTime());
            CommissionController(controllerId);
            ExitManualMode(controllerId, normalStreetlight);
            ExitManualMode(controllerId, failureStreetlight);

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.RealTimeControl);

            Step("1. Go to Real-Time Control app and select the testing geozone");
            var realTimeControlPage = desktopPage.GoToApp(App.RealTimeControl) as RealTimeControlPage;
            realTimeControlPage.GeozoneTreeMainPanel.SelectNode(geozone);
            
            Step("2. Select the cluster on the map");
            realTimeControlPage.Map.SelectDeviceGL(lngCluster, latCluster);
            realTimeControlPage.Map.WaitForDeviceClusterPopupPanelDisplayed();

            Step("3. Check the checkbox of the streetlight with a critical failure on the cluster table");
            realTimeControlPage.Map.DeviceClusterPopupPanel.TickGridColumn(failureStreetlight, true);
            realTimeControlPage.WaitForPreviousActionComplete();
            realTimeControlPage.WaitForStreetlightWidgetDisplayed(failureStreetlight);

            Step("4. Verify In the Type column of selected streetlight in the cluster table");
            Step(" o An icon of streetlight has Yellow color surrounding a Orange circle containing a White Bulb icon (it means the streetlight is ON and has a critical failure)");           
            var icon = realTimeControlPage.Map.DeviceClusterPopupPanel.GetIconType(failureStreetlight);
            VerifyTrue("4. Verify An icon of streetlight has Yellow color surrounding a Orange circle containing a White Bulb icon (it means the streetlight is ON and has a critical failure)", icon.Contains("streetlight-warning-100.png"), "streetlight-warning-100.png", icon);

            Step("5. Verify The Light Control Panel of selected streetlight displays");
            VerifyEqual("5. Verify The Light Control Panel of selected streetlight displays", true, realTimeControlPage.IsStreetlightWidgetDisplayed());
            VerifyEqual("5. Verify The Light Control Panel of selected streetlight displays", failureStreetlight, realTimeControlPage.StreetlightWidgetPanel.GetDeviceNameText());

            Step("6. Press OFF button on the Light Control Panel to turn the streetlight off");
            realTimeControlPage.StreetlightWidgetPanel.ClickCommandOffButton();
            realTimeControlPage.StreetlightWidgetPanel.WaitForCommandCompleted();

            Step("7. Verify In the Type column of selected streetlight in the cluster table");
            Step(" o An icon of streetlight has Gray color surrounding a Orange circle containing a White Bulb icon (it means the streetlight is OFF and has a critical failure)");
            icon = realTimeControlPage.Map.DeviceClusterPopupPanel.GetIconType(failureStreetlight);
            VerifyTrue("7. Verify An icon of streetlight has Gray color surrounding a Orange circle containing a White Bulb icon (it means the streetlight is OFF and has a critical failure)", icon.Contains("streetlight-warning.png"), "streetlight-warning.png", icon);

            Step("8. Turn the streetlight ON again");
            realTimeControlPage.StreetlightWidgetPanel.ClickCommandOnButton();
            realTimeControlPage.StreetlightWidgetPanel.WaitForCommandCompleted();

            Step("9. Verify In the Type column of selected streetlight in the cluster table");
            Step(" o An icon of streetlight has Yellow color surrounding a Orange circle containing a White Bulb icon (it means the streetlight is ON and has a critical failure)");
            icon = realTimeControlPage.Map.DeviceClusterPopupPanel.GetIconType(failureStreetlight);
            VerifyTrue("9. Verify An icon of streetlight has Yellow color surrounding a Orange circle containing a White Bulb icon (it means the streetlight is ON and has a critical failure)", icon.Contains("streetlight-warning-100.png"), "streetlight-warning-100.png", icon);

            Step("10. Press the number 100% of Command on Light Control Panel, input 0 and press 'Enter' to make the change");
            realTimeControlPage.StreetlightWidgetPanel.ClickToEditCommandValue();
            realTimeControlPage.StreetlightWidgetPanel.EnterRealtimeCommandInput("0", true);
            realTimeControlPage.StreetlightWidgetPanel.WaitForCommandCompleted();

            Step("11. Verify In the Type column of selected streetlight in the cluster table");
            Step(" o An icon of streetlight has Gray color surrounding a Orange circle containing a White Bulb icon (it means the streetlight is OFF and has a critical failure)");
            icon = realTimeControlPage.Map.DeviceClusterPopupPanel.GetIconType(failureStreetlight);
            VerifyTrue("11. Verify An icon of streetlight has Gray color surrounding a Orange circle containing a White Bulb icon (it means the streetlight is OFF and has a critical failure)", icon.Contains("streetlight-warning.png"), "streetlight-warning.png", icon);

            try
            {
                DeleteGeozone(geozone);
                DeleteCalendar(calendarId);
                DeleteControlProgram(controlProgramId);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("RTC - 1429505 - Bug Id 1354748 - Real Time Control - Disable multi selection")]
        [NonParallelizable]
        public void RTC_1429505()
        {
            var testData = GetTestDataOfRTC_1429505();
            var geozone = testData["Geozone"].ToString();
            var streetlights = testData["Streetlights"] as List<DeviceModel>;
            var streetlight1 = streetlights[0];
            var streetlight2 = streetlights[1];
            var cluster1 = streetlights.Where(p => p.Cluster == "1").ToList();

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.RealTimeControl);

            Step("1. Go to Real-Time Control app and select the Real Time Control Area geozone");
            var realtimeControlPage = desktopPage.GoToApp(App.RealTimeControl) as RealTimeControlPage;
            realtimeControlPage.GeozoneTreeMainPanel.SelectNode(geozone);

            Step("2. Select a streetlight. Ex: Telematics 01");
            realtimeControlPage.GeozoneTreeMainPanel.SelectNode(streetlight1.Name);
            realtimeControlPage.WaitForStreetlightWidgetDisplayed(streetlight1.Name);

            Step("3. Verify Its Light Control panel appears");
            VerifyEqual("3. Verify Its Light Control panel appears", true, realtimeControlPage.IsStreetlightWidgetDisplayed());
            VerifyEqual("3. Verify Its Light Control panel appears", streetlight1.Name, realtimeControlPage.StreetlightWidgetPanel.GetDeviceNameText());

            Step("4. Hold Ctrl key and click another streetlight. Ex: Telematics 02");
            realtimeControlPage.Map.SelectDevicesGLWithCtrlKey(streetlight2.Longitude, streetlight2.Latitude);
            realtimeControlPage.WaitForStreetlightWidgetDisplayed(streetlight2.Name);

            Step("5. Verify Telematics 02's Light Control panel appears");
            VerifyEqual("5. Verify Telematics 02's Light Control panel appears", streetlight2.Name, realtimeControlPage.StreetlightWidgetPanel.GetDeviceNameText());

            Step("6. Hold Shift key and click another streetlight. Ex: Telematics 01");
            realtimeControlPage.Map.SelectDevicesGL(streetlight1.Longitude, streetlight1.Latitude);

            Step("7. Verify Telematics 02's Light Control panel still appears and it is still selected");
            VerifyEqual("7. Verify Telematics 02's Light Control panel still appears and it is still selected", streetlight2.Name, realtimeControlPage.StreetlightWidgetPanel.GetDeviceNameText());
            VerifyEqual("7. Verify Telematics 02 is still selected", streetlight2.Name, realtimeControlPage.GeozoneTreeMainPanel.GetSelectedNodeName());

            Step("8. Hold S key and click another streetlight. Ex: Telematics 01");
            realtimeControlPage.Map.SelectDevicesGLWithSKey(streetlight1.Longitude, streetlight1.Latitude);
            realtimeControlPage.WaitForStreetlightWidgetDisplayed(streetlight1.Name);

            Step("9. Verify Telematics 01's Light Control panel appears");
            VerifyEqual("9. Verify Telematics 01's Light Control panel appears", streetlight1.Name, realtimeControlPage.StreetlightWidgetPanel.GetDeviceNameText());

            Step("10. Hold Shift key and drag the mouse on the map");
            var notedPoint = realtimeControlPage.Map.GetDevicePosition(streetlight1.Longitude, streetlight1.Latitude);
            realtimeControlPage.Map.ClickHoldAndMoveToWithShiftKey();

            Step("11. Verify The map is moved");
            var mapPoint1 = realtimeControlPage.Map.GetDevicePosition(streetlight1.Longitude, streetlight1.Latitude);
            VerifyEqual("11. Verify The map is moved", true, notedPoint != mapPoint1);

            Step("12. Hold S key and drag the mouse on the map");
            realtimeControlPage.Map.ClickHoldAndMoveToWithSKey();

            Step("13. Verify The map is moved");
            var mapPoint2 = realtimeControlPage.Map.GetDevicePosition(streetlight1.Longitude, streetlight1.Latitude);
            VerifyEqual("13. Verify The map is moved", true, mapPoint1 != mapPoint2);

            Step("14. Select a cluster. Ex: cluster containing Telematics 06 & 07");
            realtimeControlPage.AppBar.ClickHeaderBartop();
            realtimeControlPage.Map.SelectDeviceGL(cluster1[0].Longitude, cluster1[0].Latitude);            
            realtimeControlPage.Map.WaitForDeviceClusterPopupPanelDisplayed();

            Step("15. Verify The cluster's table displays");
            VerifyEqual("15. Verify The cluster's table displays", true, realtimeControlPage.Map.IsDeviceClusterPopupPanelDisplayed());

            Step("16. Select a row on the table. Ex: Telematics 07");
            realtimeControlPage.Map.DeviceClusterPopupPanel.TickGridColumn(cluster1[1].Name, true);
            realtimeControlPage.WaitForStreetlightWidgetDisplayed(cluster1[1].Name);
            realtimeControlPage.WaitForPreviousActionComplete();

            Step("17. Verify Telematics 07's Light Control panel appears");
            VerifyEqual("17. Verify Telematics 07's Light Control panel appears", cluster1[1].Name, realtimeControlPage.StreetlightWidgetPanel.GetDeviceNameText());

            Step("18. Hold Shift key and select Telematics 06's row");
            realtimeControlPage.Map.DeviceClusterPopupPanel.SelectRowWithShiftKey(cluster1[0].Name);
            realtimeControlPage.WaitForStreetlightWidgetDisplayed(cluster1[0].Name);
            realtimeControlPage.WaitForPreviousActionComplete();

            Step("19. Verify");
            Step(" o Only the last selected row is checked");
            Step(" o Telematics 06's Light Control panel appears");
            VerifyEqual("19. Verify Telematics 06 row is checked", true, realtimeControlPage.Map.DeviceClusterPopupPanel.GetCheckBoxGridColumnValue(cluster1[0].Name));
            VerifyEqual("19. Verify Telematics 07 row is not checked", false, realtimeControlPage.Map.DeviceClusterPopupPanel.GetCheckBoxGridColumnValue(cluster1[1].Name));
            VerifyEqual("19. Verify Telematics 06's Light Control panel appears", cluster1[0].Name, realtimeControlPage.StreetlightWidgetPanel.GetDeviceNameText());

            Step("20. Check the checkbox of Telematics 07's row");
            realtimeControlPage.Map.DeviceClusterPopupPanel.TickGridColumn(cluster1[1].Name, true);
            realtimeControlPage.WaitForStreetlightWidgetDisplayed(cluster1[1].Name);
            realtimeControlPage.WaitForPreviousActionComplete();

            Step("21. Verify");
            Step(" o Only the last selected row is checked");
            Step(" o Telematics 07's Light Control panel appears");
            VerifyEqual("21. Verify Telematics 07 row is checked", true, realtimeControlPage.Map.DeviceClusterPopupPanel.GetCheckBoxGridColumnValue(cluster1[1].Name));
            VerifyEqual("21. Verify Telematics 06 row is not checked", false, realtimeControlPage.Map.DeviceClusterPopupPanel.GetCheckBoxGridColumnValue(cluster1[0].Name));
            VerifyEqual("21. Verify Telematics 07's Light Control panel appears", cluster1[1].Name, realtimeControlPage.StreetlightWidgetPanel.GetDeviceNameText());

            Step("22. Hold Ctrl key and select Telematics 06's row");
            realtimeControlPage.Map.DeviceClusterPopupPanel.SelectRowWithCtrlKey(cluster1[0].Name);
            realtimeControlPage.WaitForStreetlightWidgetDisplayed(cluster1[0].Name);
            realtimeControlPage.WaitForPreviousActionComplete();

            Step("23. Verify");
            Step(" o Only the last selected row is checked");
            Step(" o Telematics 06's Light Control panel appears");
            VerifyEqual("23. Verify Telematics 06 row is checked", true, realtimeControlPage.Map.DeviceClusterPopupPanel.GetCheckBoxGridColumnValue(cluster1[0].Name));
            VerifyEqual("23. Verify Telematics 07 row is not checked", false, realtimeControlPage.Map.DeviceClusterPopupPanel.GetCheckBoxGridColumnValue(cluster1[1].Name));
            VerifyEqual("23. Verify Telematics 06's Light Control panel appears", cluster1[0].Name, realtimeControlPage.StreetlightWidgetPanel.GetDeviceNameText());

            Step("24. Check the checkbox on the header of the table");
            realtimeControlPage.Map.DeviceClusterPopupPanel.TickAllRowsCheckbox(true);
            realtimeControlPage.WaitForPreviousActionComplete();

            Step("25. Verify The checkbox on the header is checked but only Telematics 06's checkbox is checked in the grid");
            VerifyEqual("25. Verify The checkbox on the header is checked", true, realtimeControlPage.Map.DeviceClusterPopupPanel.GetAllRowsCheckbox());
            VerifyEqual("25. Verify Telematics 06 row is checked", true, realtimeControlPage.Map.DeviceClusterPopupPanel.GetCheckBoxGridColumnValue(cluster1[0].Name));
            VerifyEqual("25. Verify Telematics 07 row is not checked", false, realtimeControlPage.Map.DeviceClusterPopupPanel.GetCheckBoxGridColumnValue(cluster1[1].Name));
        }

        #endregion //Test Cases

        #region Private methods

        public RealtimeCommand GetCommandByText(RealTimeControlPage page, string text)
        {
            switch (text)
            {
                case "Switch ON":
                case "100%":
                    return RealtimeCommand.DimOn;
                case "Switch OFF":
                case "0%":
                    return RealtimeCommand.DimOff;
                default:
                    return page.ControllerWidgetPanel.GetCommandByText(text);
            }
        }

        #region Verify methods

        public void VerifyStreetlightMeteringsAfterRefreshed(RealTimeControlPage page, int indicatorCursorPosition, string feedbackValueText
            , string commandValueText, string lampBurningHours, string lampEnergy, string lampLevelCommand, string lampLevelFeedback
            , string lampPower, string lampSwitchFeedback, string nodeFailureMessage, bool isStreetlightIcon, string deviceName
            , string mainsCurrent, string mainsVoltage, string powerFactor, string temperature, string localTime)
        {
            var updatedIndicatorCursorPosition = page.StreetlightWidgetPanel.GetIndicatorCursorPositionValue();
            var updatedFeedbackValueText = page.StreetlightWidgetPanel.GetRealtimeFeedbackValueText();
            var updatedCommandValueText = page.StreetlightWidgetPanel.GetRealtimeCommandValueText();
            var updatedLampBurningHours = page.StreetlightWidgetPanel.GetLampBurningHoursValueText();
            var updatedLampEnergy = page.StreetlightWidgetPanel.GetLampEnergyValueText();
            var updatedLampLevelCommand = page.StreetlightWidgetPanel.GetLampLevelCommandValueText();
            var updatedLampLevelFeedback = page.StreetlightWidgetPanel.GetLampLevelFeedbackValueText();
            var updatedLampPower = page.StreetlightWidgetPanel.GetLampPowerValueText();
            var updatedLampSwitchFeedback = page.StreetlightWidgetPanel.GetLampSwitchFeedbackValueText();
            var updatedNodeFailureMessage = page.StreetlightWidgetPanel.GetNodeFailureMessageValueText();
            var updatedIsStreetlightIcon = page.StreetlightWidgetPanel.CheckIfDeviceIcon(DeviceType.Streetlight);
            var updatedName = page.StreetlightWidgetPanel.GetDeviceNameText();
            var updatedMainsCurrent = page.StreetlightWidgetPanel.GetMainsCurrentValueText();
            var updatedMainsVoltage = page.StreetlightWidgetPanel.GetMainsVoltageValueText();
            var updatedPowerFactor = page.StreetlightWidgetPanel.GetPowerFactorValueText();
            var updatedTemperature = page.StreetlightWidgetPanel.GetTemperatureValueText();
            var updatedLocalTime = page.StreetlightWidgetPanel.GetLastUpdateTimeText();

            Step("* Verify Values of following values unchanged:");
            Step(" - Indicator position");
            Step(" - Feedback");
            Step(" - Command");
            Step(" - Lamp burning hours");
            Step(" - Lamp energy");
            Step(" - Lamp level command");
            Step(" - Lamp level feedback");
            Step(" - Lamp power");
            Step(" - Lamp switch feedback");
            Step(" - Node failure message");
            Step(" - Streetlight icon and name");
            VerifyEqual("Verify Indicator position unchanged", indicatorCursorPosition, updatedIndicatorCursorPosition);
            VerifyEqual("Verify Feedback unchanged", feedbackValueText, updatedFeedbackValueText);
            VerifyEqual("Verify Command unchanged", commandValueText, updatedCommandValueText);
            VerifyEqual("Verify Lamp burning hours unchanged", lampBurningHours, updatedLampBurningHours);
            VerifyEqual("Verify Lamp energy unchanged", lampEnergy, updatedLampEnergy);
            VerifyEqual("Verify Lamp level command unchanged", lampLevelCommand, updatedLampLevelCommand);
            VerifyEqual("Verify Lamp level feedback unchanged", lampLevelFeedback, updatedLampLevelFeedback);
            VerifyEqual("Verify Lamp power unchanged", lampPower, updatedLampPower);
            VerifyEqual("Verify Lamp switch feedback unchanged", lampSwitchFeedback, updatedLampSwitchFeedback);
            VerifyEqual("Verify Node failure message unchanged", nodeFailureMessage, updatedNodeFailureMessage);
            VerifyEqual("Verify Streetlight icon unchanged", isStreetlightIcon, updatedIsStreetlightIcon);
            VerifyEqual("Verify name unchanged", deviceName, updatedName);

            Step("* Verify Values of following values changed:");
            Step(" - Mains current (> 0A when dimming level is not '0%', = 0A when dimming level = '0%')");
            Step(" - Mains voltage(V)");
            Step(" - Power factor");
            Step(" - Temperature");
            Step(" - Local time");
            if (updatedCommandValueText.Equals("0%"))
                VerifyTrue("Verify Mains current is 0A (> 0 when dimming level is not '0%', = 0A when dimming level = '0%')", mainsCurrent == "0A" && updatedMainsCurrent == "0A", mainsCurrent, updatedMainsCurrent);
            else
                VerifyTrue(string.Format("Verify Mains current changed '{0}' (> 0 when dimming level is not '0%', = 0 when dimming level = '0%')", updatedMainsCurrent), mainsCurrent != updatedMainsCurrent && double.Parse(updatedMainsCurrent.Replace("A", string.Empty)) > 0, "<>" + mainsCurrent, updatedMainsCurrent);
            VerifyTrue(string.Format("Verify Mains voltage(V) changed {0}->{1}", mainsVoltage, updatedMainsVoltage), mainsVoltage != updatedMainsVoltage, "<>" + mainsVoltage, updatedMainsVoltage);
            VerifyTrue(string.Format("Verify Power factor changed {0}->{1}", powerFactor, updatedPowerFactor), powerFactor != updatedPowerFactor, "<>" + powerFactor, updatedPowerFactor);
            VerifyTrue(string.Format("Verify Temperature changed {0}->{1}", temperature, updatedTemperature), temperature != updatedTemperature, "<>" + temperature, updatedTemperature);
            VerifyTrue(string.Format("Verify Local time changed {0}->{1}", localTime, updatedLocalTime), localTime != updatedLocalTime, "<>" + localTime, updatedLocalTime);
        }

        public void VerifyStreetlightMeteringsAfterExecutedCommand(RealTimeControlPage page, int indicatorCursorPosition, string feedbackValueText
            , string commandValueText, string lampBurningHours, string lampEnergy, string lampLevelCommand, string lampLevelFeedback
            , string lampPower, string lampSwitchFeedback, string nodeFailureMessage, bool isStreetlightIcon, string deviceName
            , string mainsCurrent, string mainsVoltage, string powerFactor, string temperature, string localTime)
        {
            var updatedIndicatorCursorPosition = page.StreetlightWidgetPanel.GetIndicatorCursorPositionValue();
            var updatedFeedbackValueText = page.StreetlightWidgetPanel.GetRealtimeFeedbackValueText();
            var updatedCommandValueText = page.StreetlightWidgetPanel.GetRealtimeCommandValueText();
            var updatedLampBurningHours = page.StreetlightWidgetPanel.GetLampBurningHoursValueText();
            var updatedLampEnergy = page.StreetlightWidgetPanel.GetLampEnergyValueText();
            var updatedLampLevelCommand = page.StreetlightWidgetPanel.GetLampLevelCommandValueText();
            var updatedLampLevelFeedback = page.StreetlightWidgetPanel.GetLampLevelFeedbackValueText();
            var updatedLampPower = page.StreetlightWidgetPanel.GetLampPowerValueText();
            var updatedLampSwitchFeedback = page.StreetlightWidgetPanel.GetLampSwitchFeedbackValueText();
            var updatedNodeFailureMessage = page.StreetlightWidgetPanel.GetNodeFailureMessageValueText();
            var updatedIsStreetlightIcon = page.StreetlightWidgetPanel.CheckIfDeviceIcon(DeviceType.Streetlight);
            var updatedName = page.StreetlightWidgetPanel.GetDeviceNameText();
            var updatedMainsCurrent = page.StreetlightWidgetPanel.GetMainsCurrentValueText();
            var updatedMainsVoltage = page.StreetlightWidgetPanel.GetMainsVoltageValueText();
            var updatedPowerFactor = page.StreetlightWidgetPanel.GetPowerFactorValueText();
            var updatedTemperature = page.StreetlightWidgetPanel.GetTemperatureValueText();
            var updatedLocalTime = page.StreetlightWidgetPanel.GetLastUpdateTimeText();

            Step("*** Verify Values unchanged:");
            if (commandValueText == updatedCommandValueText)
            {
                Step(" - Indicator position");
                Step(" - Feedback");
                Step(" - Command");
                Step(" - Lamp level command");
                Step(" - Lamp level feedback");
                Step(" - Lamp power");
            }
            Step(" - Lamp burning hours");
            Step(" - Lamp energy");
            Step(" - Node failure message");
            Step(" - Streetlight icon and name");
            if (commandValueText == updatedCommandValueText)
            {
                VerifyEqual("Verify Indicator position unchanged", indicatorCursorPosition, updatedIndicatorCursorPosition);
                VerifyEqual("Verify Feedback unchanged", feedbackValueText, updatedFeedbackValueText);
                VerifyEqual("Verify Command unchanged", commandValueText, updatedCommandValueText);
                VerifyEqual("Verify Lamp level command unchanged", lampLevelCommand, updatedLampLevelCommand);
                VerifyEqual("Verify Lamp level feedback unchanged", lampLevelFeedback, updatedLampLevelFeedback);
                VerifyEqual("Verify Lamp power unchanged", lampPower, updatedLampPower);
            }
            VerifyEqual("Verify Lamp burning hours unchanged", lampBurningHours, updatedLampBurningHours);
            VerifyEqual("Verify Lamp energy unchanged", lampEnergy, updatedLampEnergy);
            VerifyEqual("Verify Node failure message unchanged", nodeFailureMessage, updatedNodeFailureMessage);
            VerifyEqual("Verify Streetlight icon unchanged", isStreetlightIcon, updatedIsStreetlightIcon);
            VerifyEqual("Verify name unchanged", deviceName, updatedName);

            Step("*** Verify Values changed (if executing another command):");
            if (commandValueText != updatedCommandValueText)
            {
                Step(" - Indicator position");
                Step(" - Feedback");
                Step(" - Command");
                Step(" - Lamp level command");
                Step(" - Lamp level feedback");
                Step(" - Lamp power");
            }
            Step(" - Lamp switch feedback (> 0 when dimming level is not '0%', = 0 when dimming level = '0%')");
            Step(" - Mains current (> 0 when dimming level is not '0%', = 0 when dimming level = '0%')");
            Step(" - Mains voltage(V)");
            Step(" - Power factor");
            Step(" - Temperature");
            Step(" - Local time");
            if (commandValueText != updatedCommandValueText)
            {
                VerifyTrue(string.Format("Verify Indicator position changed {0}->{1}", indicatorCursorPosition, updatedIndicatorCursorPosition), indicatorCursorPosition != updatedIndicatorCursorPosition, "<>" + indicatorCursorPosition, updatedIndicatorCursorPosition);
                VerifyTrue(string.Format("[SC-810] Verify Feedback changed {0}->{1}", feedbackValueText, updatedFeedbackValueText), feedbackValueText != updatedFeedbackValueText, "<>" + feedbackValueText, updatedFeedbackValueText);
                VerifyTrue(string.Format("Verify Command changed {0}->{1}", commandValueText, updatedCommandValueText), commandValueText != updatedCommandValueText, "<>" + commandValueText, updatedCommandValueText);
                VerifyTrue(string.Format("Verify Lamp level command changed {0}->{1}", lampLevelCommand, updatedLampLevelCommand), lampLevelCommand != updatedLampLevelCommand, "<>" + lampLevelCommand, updatedLampLevelCommand);
                VerifyTrue(string.Format("Verify Lamp level feedback changed {0}->{1}", lampLevelFeedback, updatedLampLevelFeedback), lampLevelFeedback != updatedLampLevelFeedback, "<>" + lampLevelFeedback, updatedLampLevelFeedback);
                VerifyTrue(string.Format("Verify Lamp power changed {0}->{1}", lampPower, updatedLampPower), lampPower != updatedLampPower, "<>" + lampPower, updatedLampPower);
            }

            if (updatedCommandValueText.Equals("0%"))
            {
                VerifyTrue(string.Format("Verify Lamp switch feedback '{0}' (> 0 when dimming level is not 0, = '0' when dimming level = '0%')", updatedLampSwitchFeedback), updatedLampSwitchFeedback == "0", "0", updatedLampSwitchFeedback);
                VerifyTrue(string.Format("Verify Mains current {0} (> 0A when dimming level is not 0, = '0A' when dimming level = '0%')", updatedMainsCurrent), updatedMainsCurrent == "0A", "0A", updatedMainsCurrent);
            }
            else
            {
                VerifyTrue(string.Format("Verify Lamp switch feedback '{0}' (> 0 when dimming level is not '0%', = 0 when dimming level = '0%')", updatedLampSwitchFeedback), double.Parse(updatedLampSwitchFeedback) > 0, "> 0", updatedLampSwitchFeedback);
                VerifyTrue(string.Format("Verify Mains current '{0}' (> 0A when dimming level is not '0%', = 0A when dimming level = '0%')", updatedMainsCurrent), mainsCurrent != updatedMainsCurrent && double.Parse(updatedMainsCurrent.Replace("A", string.Empty)) > 0, "<>" + mainsCurrent, updatedMainsCurrent);
            }

            VerifyTrue(string.Format("Verify Mains voltage(V) changed {0}->{1}", mainsVoltage, updatedMainsVoltage), mainsVoltage != updatedMainsVoltage, "<>" + mainsVoltage, updatedMainsVoltage);
            VerifyTrue(string.Format("Verify Power factor changed {0}->{1}", powerFactor, updatedPowerFactor), powerFactor != updatedPowerFactor, "<>" + powerFactor, updatedPowerFactor);
            VerifyTrue(string.Format("Verify Temperature changed {0}->{1}", temperature, updatedTemperature), temperature != updatedTemperature, "<>" + temperature, updatedTemperature);
            VerifyTrue(string.Format("Verify Local time changed {0}->{1}", localTime, updatedLocalTime), localTime != updatedLocalTime, "<>" + localTime, updatedLocalTime);
        }

        public void VerifyStreetlightMeteringsUnchanged(RealTimeControlPage page, int indicatorCursorPosition, string feedbackValueText
            , string commandValueText, string lampBurningHours, string lampEnergy, string lampLevelCommand, string lampLevelFeedback
            , string lampPower, string lampSwitchFeedback, string nodeFailureMessage, bool isStreetlightIcon, string deviceName
            , string mainsCurrent, string mainsVoltage, string powerFactor, string temperature, string localTime)
        {
            var updatedIndicatorCursorPosition = page.StreetlightWidgetPanel.GetIndicatorCursorPositionValue();
            var updatedFeedbackValueText = page.StreetlightWidgetPanel.GetRealtimeFeedbackValueText();
            var updatedCommandValueText = page.StreetlightWidgetPanel.GetRealtimeCommandValueText();
            var updatedLampBurningHours = page.StreetlightWidgetPanel.GetLampBurningHoursValueText();
            var updatedLampEnergy = page.StreetlightWidgetPanel.GetLampEnergyValueText();
            var updatedLampLevelCommand = page.StreetlightWidgetPanel.GetLampLevelCommandValueText();
            var updatedLampLevelFeedback = page.StreetlightWidgetPanel.GetLampLevelFeedbackValueText();
            var updatedLampPower = page.StreetlightWidgetPanel.GetLampPowerValueText();
            var updatedLampSwitchFeedback = page.StreetlightWidgetPanel.GetLampSwitchFeedbackValueText();
            var updatedNodeFailureMessage = page.StreetlightWidgetPanel.GetNodeFailureMessageValueText();
            var updatedIsStreetlightIcon = page.StreetlightWidgetPanel.CheckIfDeviceIcon(DeviceType.Streetlight);
            var updatedName = page.StreetlightWidgetPanel.GetDeviceNameText();
            var updatedMainsCurrent = page.StreetlightWidgetPanel.GetMainsCurrentValueText();
            var updatedMainsVoltage = page.StreetlightWidgetPanel.GetMainsVoltageValueText();
            var updatedPowerFactor = page.StreetlightWidgetPanel.GetPowerFactorValueText();
            var updatedTemperature = page.StreetlightWidgetPanel.GetTemperatureValueText();
            var updatedLocalTime = page.StreetlightWidgetPanel.GetLastUpdateTimeText();

            VerifyEqual("Verify Indicator position unchanged", indicatorCursorPosition, updatedIndicatorCursorPosition);
            VerifyEqual("Verify Feedback unchanged", feedbackValueText, updatedFeedbackValueText);
            VerifyEqual("Verify Command unchanged", commandValueText, updatedCommandValueText);
            VerifyEqual("Verify Lamp Burning Hours unchanged", lampBurningHours, updatedLampBurningHours);
            VerifyEqual("Verify Lamp Energy unchanged", lampEnergy, updatedLampEnergy);
            VerifyEqual("Verify Lamp level command unchanged", lampLevelCommand, updatedLampLevelCommand);
            VerifyEqual("Verify Lamp level feedback unchanged", lampLevelFeedback, updatedLampLevelFeedback);
            VerifyEqual("Verify Lamp power unchanged", lampPower, updatedLampPower);
            VerifyEqual("Verify Lamp Switch Feedback unchanged", lampSwitchFeedback, updatedLampSwitchFeedback);
            VerifyEqual("Verify Mains Current unchanged", mainsCurrent, updatedMainsCurrent);
            VerifyEqual("Verify Mains Voltage unchanged", mainsVoltage, updatedMainsVoltage);
            VerifyEqual("Verify Node failure message unchanged", nodeFailureMessage, updatedNodeFailureMessage);
            VerifyEqual("Verify Power Factor unchanged", powerFactor, updatedPowerFactor);
            VerifyEqual("Verify Temperature unchanged", temperature, updatedTemperature);
            VerifyEqual("Verify Local Time unchanged", localTime, updatedLocalTime);
            VerifyEqual("Verify Streetlight icon unchanged", isStreetlightIcon, updatedIsStreetlightIcon);
            VerifyEqual("Verify name unchanged", deviceName, updatedName);
        }

        public void VerifyNonWorkingStreetlightMeterings(RealTimeControlPage page)
        {
            var lampBurningHours = page.StreetlightWidgetPanel.GetLampBurningHoursValueText();
            var lampEnergy = page.StreetlightWidgetPanel.GetLampEnergyValueText();
            var lampLevelCommand = page.StreetlightWidgetPanel.GetLampLevelCommandValueText();
            var lampLevelFeedback = page.StreetlightWidgetPanel.GetLampLevelFeedbackValueText();
            var lampPower = page.StreetlightWidgetPanel.GetLampPowerValueText();
            var lampSwitchFeedback = page.StreetlightWidgetPanel.GetLampSwitchFeedbackValueText();
            var nodeFailureMessage = page.StreetlightWidgetPanel.GetNodeFailureMessageValueText();
            var mainsCurrent = page.StreetlightWidgetPanel.GetMainsCurrentValueText();
            var mainsVoltage = page.StreetlightWidgetPanel.GetMainsVoltageValueText();
            var powerFactor = page.StreetlightWidgetPanel.GetPowerFactorValueText();
            var temperature = page.StreetlightWidgetPanel.GetTemperatureValueText();

            VerifyEqual("Verify Lamp Burning Hours is ...", "...", lampBurningHours);
            VerifyEqual("Verify Lamp Energy is ...", "...", lampEnergy);
            VerifyEqual("Verify Lamp level command is ...", "...", lampLevelCommand);
            VerifyEqual("Verify Lamp level feedback is ...", "...", lampLevelFeedback);
            VerifyEqual("Verify Lamp power is ...", "...", lampPower);
            VerifyEqual("Verify Lamp Switch Feedback is ...", "...", lampSwitchFeedback);
            VerifyEqual("Verify Mains Current is ...", "...", mainsCurrent);
            VerifyEqual("Verify Mains Voltage is ...", "...", mainsVoltage);
            VerifyEqual("Verify Node failure message is ...", "...", nodeFailureMessage);
            VerifyEqual("Verify Power Factor is ...", "...", powerFactor);
            VerifyEqual("Verify Temperature is ...", "...", temperature);
        }

        public void VerifyWorkingStreetlightMeterings(RealTimeControlPage page)
        {
            double value;
            VerifyEqual("Verify 'Lamp burning hours': number + 'h'", true, Regex.IsMatch(page.StreetlightWidgetPanel.GetLampBurningHoursValueText(), @"\d{1,}h"));
            VerifyEqual("Verify 'Lamp energy': number", true, double.TryParse(page.StreetlightWidgetPanel.GetLampEnergyValueText(), out value));
            VerifyEqual("Verify 'Lamp level command': number", true, double.TryParse(page.StreetlightWidgetPanel.GetLampLevelCommandValueText(), out value));
            VerifyEqual("Verify 'Lamp level feedback': number + 'h'", true, Regex.IsMatch(page.StreetlightWidgetPanel.GetLampLevelFeedbackValueText(), @"\d{1,}%"));
            VerifyEqual("Verify 'Lamp power': number", true, double.TryParse(page.StreetlightWidgetPanel.GetLampPowerValueText(), out value));
            VerifyEqual("Verify 'Lamp switch feedback': number", true, double.TryParse(page.StreetlightWidgetPanel.GetLampSwitchFeedbackValueText(), out value));
            VerifyEqual("Verify 'Mains current': number + 'A'", true, Regex.IsMatch(page.StreetlightWidgetPanel.GetMainsCurrentValueText(), @"\d{1,}A"));
            VerifyEqual("Verify 'Mains voltage (V)': number + 'V'", true, Regex.IsMatch(page.StreetlightWidgetPanel.GetMainsVoltageValueText(), @"\d{1,}V"));
            VerifyEqual("Verify 'Node failure message': number", true, double.TryParse(page.StreetlightWidgetPanel.GetNodeFailureMessageValueText(), out value));
            VerifyEqual("Verify 'Power factor': number", true, double.TryParse(page.StreetlightWidgetPanel.GetPowerFactorValueText(), out value));
            VerifyEqual("Verify 'Temperature': number", true, double.TryParse(page.StreetlightWidgetPanel.GetTemperatureValueText(), out value));
        }

        public void VerifyStreetlightStatusPanel(RealTimeControlPage page, DeviceStatus status)
        {
            var questionIcon = "status-question.png";
            var okIcon = "status-ok.png";
            var lampFailureText = page.StreetlightWidgetPanel.GetStatusLampFailureText();
            var lampFailureIconUrl = page.StreetlightWidgetPanel.GetStatusLampFailureIconValue();
            var lostCommunicationText = page.StreetlightWidgetPanel.GetStatusLostCommunicationText();
            var lostCommunicationIconUrl = page.StreetlightWidgetPanel.GetStatusLostCommunicationIconValue();
            var nodeFailureText = page.StreetlightWidgetPanel.GetStatusNodeFailureText();
            var nodeFailureIconUrl = page.StreetlightWidgetPanel.GetStatusNodeFailureIconValue();
            var unknownIdentifierText = page.StreetlightWidgetPanel.GetStatusUnknownIdentifierText();
            var unknownIdentifierIconUrl = page.StreetlightWidgetPanel.GetStatusUnknownIdentifierIconValue();

            VerifyEqual("Verify Lamp Failure label is 'Lamp failure'", "Lamp failure", lampFailureText);
            VerifyEqual("Verify Lost Communication label is 'Lost communication'", "Lost communication", lostCommunicationText);
            VerifyEqual("Verify Node Failure is label 'Node Failure'", "Node Failure", nodeFailureText);
            VerifyEqual("Verify Unknown Identifier label is 'Unknown identifier'", "Unknown identifier", unknownIdentifierText);

            switch (status)
            {
                case DeviceStatus.Working:
                    VerifyTrue("Verify Lamp Failure icon is Ok icon", lampFailureIconUrl.Contains(okIcon), okIcon, lampFailureIconUrl);
                    VerifyTrue("Verify Lost Communication icon is Ok icon", lostCommunicationIconUrl.Contains(okIcon), okIcon, lostCommunicationIconUrl);
                    VerifyTrue("Verify Node Failure icon is Ok icon", nodeFailureIconUrl.Contains(okIcon), okIcon, nodeFailureIconUrl);
                    VerifyTrue("Verify Unknown Identifier icon is Ok icon", unknownIdentifierIconUrl.Contains(okIcon), okIcon, unknownIdentifierIconUrl);
                    break;
                case DeviceStatus.NonWorking:
                    VerifyTrue("Verify Lamp Failure icon is Question icon", lampFailureIconUrl.Contains(questionIcon), questionIcon, lampFailureIconUrl);
                    VerifyTrue("Verify Lost Communication icon is Question icon", lostCommunicationIconUrl.Contains(questionIcon), questionIcon, lostCommunicationIconUrl);
                    VerifyTrue("Verify Node Failure icon is Question icon", nodeFailureIconUrl.Contains(questionIcon), questionIcon, nodeFailureIconUrl);
                    VerifyTrue("Verify Unknown Identifier icon is Question icon", unknownIdentifierIconUrl.Contains(questionIcon), questionIcon, unknownIdentifierIconUrl);
                    break;
            }
        }

        private void VerifyStreetlightDimmingLevel(RealTimeControlPage page, string longitude, string latitude, RealtimeCommand command)
        {
            page.Map.MoveToDeviceGL(longitude, latitude);
            var deviceSprite = page.Map.GetDeviceSprite(longitude, latitude);
            var actualDimmingLevel = deviceSprite.DimmingLevel;
            var commandData = page.StreetlightWidgetPanel.CommandsDict[command];
            var expectedDimingLevel = "0";
            if (1 <= commandData.Value && commandData.Value <= 30) expectedDimingLevel = "20";
            else if (31 <= commandData.Value && commandData.Value <= 50) expectedDimingLevel = "40";
            else if (51 <= commandData.Value && commandData.Value <= 70) expectedDimingLevel = "60";
            else if (71 <= commandData.Value && commandData.Value <= 90) expectedDimingLevel = "80";
            else if (91 <= commandData.Value && commandData.Value <= 100) expectedDimingLevel = "100";
            VerifyEqual("Verify Dimming Level of Device is changed correctly", expectedDimingLevel, actualDimmingLevel);
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

        private Dictionary<string, object> GetTestDataOfRTC_01()
        {
            var testCaseName = "RTC_01";
            var xmlUtility = new XmlUtility(Settings.RTC_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, object>();
            testData.Add("Controller", xmlUtility.GetSingleNodeText(string.Format(Settings.RTC_XPATH_PREFIX, testCaseName, "Controller")));
            testData.Add("Commands", xmlUtility.GetChildNodesText(string.Format(Settings.RTC_XPATH_PREFIX, testCaseName, "Commands")));

            return testData;
        }

        private Dictionary<string, string> GetTestDataOfRTC_02()
        {
            var testCaseName = "RTC_02";
            var xmlUtility = new XmlUtility(Settings.RTC_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, string>();
            testData.Add("Controller", xmlUtility.GetSingleNodeText(string.Format(Settings.RTC_XPATH_PREFIX, testCaseName, "Controller")));

            return testData;
        }

        private Dictionary<string, string> GetTestDataOfRTC_03()
        {
            var testCaseName = "RTC_03";
            var xmlUtility = new XmlUtility(Settings.RTC_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, string>();
            testData.Add("Controller", xmlUtility.GetSingleNodeText(string.Format(Settings.RTC_XPATH_PREFIX, testCaseName, "Controller")));

            return testData;
        }

        private Dictionary<string, object> GetTestDataOfRTC_04()
        {
            var testCaseName = "RTC_04";
            var xmlUtility = new XmlUtility(Settings.RTC_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, object>();
            testData.Add("Controller", xmlUtility.GetSingleNodeText(string.Format(Settings.RTC_XPATH_PREFIX, testCaseName, "Controller")));
            var expectedDimmingGroups = Settings.CommonTestData[0].Devices.Where(p => p.Type == DeviceType.Streetlight && p.Status == DeviceStatus.Working && string.IsNullOrEmpty(p.Cluster)).Select(p => p.DimmingGroup).Distinct().ToList();
            testData.Add("DimmingGroups", expectedDimmingGroups);

            return testData;
        }

        private Dictionary<string, object> GetTestDataOfRTC_05()
        {
            var testCaseName = "RTC_05";
            var xmlUtility = new XmlUtility(Settings.RTC_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, object>();
            testData.Add("Controller", xmlUtility.GetSingleNodeText(string.Format(Settings.RTC_XPATH_PREFIX, testCaseName, "Controller")));
            var streetlights = Settings.CommonTestData[0].Devices.Where(p => p.Type == DeviceType.Streetlight && p.Status == DeviceStatus.Working && string.IsNullOrEmpty(p.Cluster)).ToList();
            testData.Add("Streetlights", streetlights);

            return testData;
        }

        private Dictionary<string, object> GetTestDataOfRTC_06()
        {
            var testCaseName = "RTC_06";
            var xmlUtility = new XmlUtility(Settings.RTC_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, object>();
            testData.Add("Controller", xmlUtility.GetSingleNodeText(string.Format(Settings.RTC_XPATH_PREFIX, testCaseName, "Controller")));
            var streetlights = Settings.CommonTestData[0].Devices.Where(p => p.Type == DeviceType.Streetlight && p.Status == DeviceStatus.Working && string.IsNullOrEmpty(p.Cluster)).ToList();
            testData.Add("Streetlights", streetlights);

            return testData;
        }

        private Dictionary<string, object> GetTestDataOfRTC_07()
        {
            var testCaseName = "RTC_07";
            var xmlUtility = new XmlUtility(Settings.RTC_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, object>();
            testData.Add("Controller", xmlUtility.GetSingleNodeText(string.Format(Settings.RTC_XPATH_PREFIX, testCaseName, "Controller")));
            var streetlights = Settings.CommonTestData[0].Devices.Where(p => p.Type == DeviceType.Streetlight && p.Status == DeviceStatus.Working && string.IsNullOrEmpty(p.Cluster)).ToList();
            testData.Add("Streetlights", streetlights);

            return testData;
        }

        private Dictionary<string, object> GetTestDataOfRTC_08()
        {
            var testCaseName = "RTC_08";
            var xmlUtility = new XmlUtility(Settings.RTC_TEST_DATA_FILE_PATH);

            var testData = GetCommonTestData();
            testData.Add("Commands", xmlUtility.GetChildNodesText(string.Format(Settings.RTC_XPATH_PREFIX, testCaseName, "Commands")));

            return testData;
        }

        private Dictionary<string, object> GetTestDataOfRTC_09()
        {
            return GetCommonTestData();
        }

        private Dictionary<string, object> GetTestDataOfRTC_10()
        {
            return GetCommonTestData();
        }

        private Dictionary<string, object> GetTestDataOfRTC_11()
        {
            return GetCommonTestData();
        }

        private Dictionary<string, object> GetTestDataOfRTC_12()
        {
            return GetCommonTestData();
        }

        private Dictionary<string, object> GetTestDataOfRTC_13()
        {
            return GetCommonTestData();
        }

        private Dictionary<string, object> GetTestDataOfRTC_14()
        {
            return GetCommonTestData();
        }

        private Dictionary<string, object> GetTestDataOfRTC_15()
        {
            var realtimeGeozone = Settings.CommonTestData[0];
            var testData = new Dictionary<string, object>();
            testData.Add("Geozone", realtimeGeozone.Path);
            var workingStreetlights = realtimeGeozone.Devices.Where(p => p.Type == DeviceType.Streetlight && p.Status == DeviceStatus.Working && string.IsNullOrEmpty(p.Cluster)).ToList();
            testData.Add("WorkingStreetlights", workingStreetlights);
            var nonWorkingStreetlights = realtimeGeozone.Devices.Where(p => p.Type == DeviceType.Streetlight && p.Status == DeviceStatus.NonWorking).ToList();
            testData.Add("NonWorkingStreetlights", nonWorkingStreetlights);

            return testData;
        }

        private Dictionary<string, object> GetTestDataOfRTC_16()
        {
            return GetCommonTestData();
        }

        private Dictionary<string, object> GetTestDataOfRTC_17()
        {
            return GetCommonTestData();
        }

        private Dictionary<string, object> GetTestDataOfRTC_18()
        {
            return GetCommonTestData();
        }

        private Dictionary<string, object> GetTestDataOfRTC_19()
        {
            return GetCommonTestData();
        }

        private Dictionary<string, object> GetTestDataOfRTC_20()
        {
            return GetCommonTestData();
        }

        private Dictionary<string, object> GetTestDataOfRTC_21()
        {
            var realtimeGeozone = Settings.CommonTestData[0];
            var testData = new Dictionary<string, object>();
            testData.Add("Geozone", realtimeGeozone.Path);
            var workingStreetlights = realtimeGeozone.Devices.Where(p => p.Type == DeviceType.Streetlight && p.Status == DeviceStatus.Working && string.IsNullOrEmpty(p.Cluster)).ToList();
            testData.Add("WorkingStreetlights", workingStreetlights);
            var nonWorkingStreetlights = realtimeGeozone.Devices.Where(p => p.Type == DeviceType.Streetlight && p.Status == DeviceStatus.NonWorking).ToList();
            testData.Add("NonWorkingStreetlights", nonWorkingStreetlights);

            return testData;
        }

        private Dictionary<string, object> GetTestDataOfRTC_22()
        {
            var realtimeGeozone = Settings.CommonTestData[0];
            var testData = new Dictionary<string, object>();
            testData.Add("Geozone", realtimeGeozone.Path);
            var controller = realtimeGeozone.Devices.Where(p => p.Type == DeviceType.Controller && p.Status == DeviceStatus.Working).FirstOrDefault();
            testData.Add("Controller", controller);

            return testData;
        }

        private Dictionary<string, object> GetTestDataOfRTC_23()
        {
            return GetCommonTestData();
        }

        private Dictionary<string, object> GetTestDataOfRTC_24()
        {
            var realtimeGeozone = Settings.CommonTestData[0];
            var testData = new Dictionary<string, object>();
            testData.Add("Geozone", realtimeGeozone.Path);
            var streetlights = realtimeGeozone.Devices.Where(p => p.Type == DeviceType.Streetlight && p.Status == DeviceStatus.Working && string.IsNullOrEmpty(p.Cluster)).ToList();
            testData.Add("Streetlights", streetlights);
            var controller = realtimeGeozone.Devices.Where(p => p.Type == DeviceType.Controller && p.Status == DeviceStatus.Working).FirstOrDefault();
            testData.Add("Controller", controller);

            return testData;
        }

        private Dictionary<string, object> GetTestDataOfRTC_25()
        {
            var testCaseName = "RTC_25";
            var realtimeGeozone = Settings.CommonTestData[0];
            var xmlUtility = new XmlUtility(Settings.RTC_TEST_DATA_FILE_PATH);
            var testData = new Dictionary<string, object>();
            testData.Add("Geozone", realtimeGeozone.Path);
            var streetlights = realtimeGeozone.Devices.Where(p => p.Type == DeviceType.Streetlight && p.Status == DeviceStatus.Working && string.IsNullOrEmpty(p.Cluster)).ToList();
            testData.Add("Streetlights", streetlights);
            testData.Add("RefreshMinute", xmlUtility.GetSingleNodeText(string.Format(Settings.RTC_XPATH_PREFIX, testCaseName, "RefreshMinute")));

            return testData;
        }

        /// <summary>
        /// Read test data for RTC_26
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, object> GetTestDataOfRTC_26()
        {
            return GetCommonTestData();
        }

        /// <summary>
        /// Read test data for RTC_27
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, object> GetTestDataOfRTC_27()
        {
            var realtimeGeozone = Settings.CommonTestData[0];
            var testData = new Dictionary<string, object>();
            testData.Add("Geozone", realtimeGeozone.Path);
            var controller = realtimeGeozone.Devices.Where(p => p.Type == DeviceType.Controller && p.Status == DeviceStatus.Working).FirstOrDefault();
            testData.Add("Controller", controller);
            var streetlights = realtimeGeozone.Devices.Where(p => p.Type == DeviceType.Streetlight && p.Status == DeviceStatus.Working && !string.IsNullOrEmpty(p.Cluster)).ToList();
            testData.Add("ClusterStreetlights", streetlights);

            return testData;
        }

        /// <summary>
        /// Read test data for RTC_28
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, object> GetTestDataOfRTC_28()
        {
            var realtimeGeozone = Settings.CommonTestData[0];
            var testData = new Dictionary<string, object>();
            testData.Add("Geozone", realtimeGeozone.Path);
            var controller = realtimeGeozone.Devices.Where(p => p.Type == DeviceType.Controller && p.Status == DeviceStatus.Working).FirstOrDefault();
            testData.Add("Controller", controller);
            var streetlights = realtimeGeozone.Devices.Where(p => p.Type == DeviceType.Streetlight && p.Status == DeviceStatus.Working && string.IsNullOrEmpty(p.Cluster)).ToList();
            testData.Add("Streetlights", streetlights);
            var clusterStreetlights = realtimeGeozone.Devices.Where(p => p.Type == DeviceType.Streetlight && p.Status == DeviceStatus.Working && !string.IsNullOrEmpty(p.Cluster)).ToList();
            testData.Add("ClusterStreetlights", clusterStreetlights);

            return testData;
        }
        /// <summary>
        /// Read test data for SC_1968
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfSC_1968()
        {
            var testCaseName = "SC_1968";
            var xmlUtility = new XmlUtility(Settings.RTC_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, string>();
            var controllerInfo = xmlUtility.GetSingleNode(string.Format(Settings.RTC_XPATH_PREFIX, testCaseName, "Controller"));
            testData.Add("ControllerId", controllerInfo.GetAttrVal("id"));
            testData.Add("ControllerName", controllerInfo.GetAttrVal("name"));
            testData.Add("ControllerGeozone", controllerInfo.GetAttrVal("geozone"));

            return testData;
        }

        private Dictionary<string, string> GetTestDataOfRTC_29()
        {
            var testCaseName = "RTC_29";
            var xmlUtility = new XmlUtility(Settings.RTC_TEST_DATA_FILE_PATH);
            var testData = new Dictionary<string, string>();

            var geozoneInfo = xmlUtility.GetSingleNode(string.Format(Settings.RTC_XPATH_PREFIX, testCaseName, "Geozone"));
            testData.Add("LatMin", geozoneInfo.GetAttrVal("latMin"));
            testData.Add("LatMax", geozoneInfo.GetAttrVal("latMax"));
            testData.Add("LngMin", geozoneInfo.GetAttrVal("lngMin"));
            testData.Add("LngMax", geozoneInfo.GetAttrVal("lngMax"));

            testData.Add("FullAddress", geozoneInfo.GetAttrVal("fullAddress"));
            testData.Add("PartialAddress", geozoneInfo.GetAttrVal("partialAddress"));

            return testData;
        }

        private Dictionary<string, string> GetTestDataOfRTC_1306572()
        {
            var testCaseName = "RTC_1306572";
            var xmlUtility = new XmlUtility(Settings.RTC_TEST_DATA_FILE_PATH);
            var testData = new Dictionary<string, string>();

            var controllerInfo = xmlUtility.GetSingleNode(string.Format(Settings.RTC_XPATH_PREFIX, testCaseName, "Controller"));
            testData.Add("ControllerId", controllerInfo.GetAttrVal("id"));
            testData.Add("ControllerName", controllerInfo.GetAttrVal("name"));

            return testData;
        }

        private Dictionary<string, object> GetTestDataOfRTC_1429505()
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

        #endregion //Input XML data

        #endregion //Private methods
    }
}
