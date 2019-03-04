using NUnit.Framework;
using StreetlightVision.Extensions;
using StreetlightVision.Models;
using StreetlightVision.Pages;
using StreetlightVision.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace StreetlightVision.Tests.Coverage.Apps
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    public class AlarmTests : TestBase
    {
        #region Variables

        #endregion //Variables

        #region Contructors

        #endregion //Contructors

        #region Test Cases

        [Test, DynamicRetry]
        [Description("Alarm-01: Alarm & Alarm Manager - Device alarm: multiple failures on multiple devices - Manual acknowledge")]
        public void Alarm_01()
        {
            var testData = GetTestDataOfAlarm01();
            var geozone = SLVHelper.GenerateUniqueName("GZNA01");
            var controller = SLVHelper.GenerateUniqueName("CTRL");
            var streetlight1 = SLVHelper.GenerateUniqueName("STL01");
            var streetlight2 = SLVHelper.GenerateUniqueName("STL02");
            var streetlights = new List<string> { streetlight1, streetlight2 };

            //Basic info
            var alarmType = testData["Alarm.type"].ToString();
            var alarmAction = testData["Alarm.action"].ToString();
            var alarmName = SLVHelper.GenerateUniqueName(string.Format("[{0}][{1}][{2}] {3}", Browser.Name, Settings.Users["DefaultTest"].Username, TestContext.CurrentContext.Test.MethodName, alarmType));

            // General
            var autoAckowledge = Convert.ToBoolean(testData["Alarm.General.auto-acknowledge"]);
            var refreshRate = testData["Alarm.General.refresh-rate"].ToString();
            var refreshRateRegex = Regex.Match(refreshRate, @"(\d*) (.*)");
            var refreshRateUnit = refreshRateRegex.Groups[2].Value;

            //Trigger
            var message = testData["Alarm.Trigger.message"].ToString();
            var failure1 = testData["Alarm.Trigger.failure1"].ToString();
            var failure2 = testData["Alarm.Trigger.failure2"].ToString();
            var failureInfoRegex1 = Regex.Match(failure1, @"(.*)#(.*)");
            var failure1Name = failureInfoRegex1.Groups[1].Value;
            var failure1Id = failureInfoRegex1.Groups[2].Value;
            var failureInfoRegex2 = Regex.Match(failure2, @"(.*)#(.*)");
            var failure2Name = failureInfoRegex2.Groups[1].Value;
            var failure2Id = failureInfoRegex2.Groups[2].Value;
            var failures = new List<string> { failure1Name, failure2Name };

            //Actions
            var mailFrom = testData["Alarm.Actions.mail-from"].ToString();
            var mailTo = testData["Alarm.Actions.mail-to"].ToString();
            var mailSubject = alarmName;
            var mailContent = testData["Alarm.Actions.mail-content"].ToString();

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - Create a geozone and 2 streetlights");
            Step(" - Simulate 2 different failures for 2 streetlights");
            Step("  + Communication failure");
            Step("  + Lamp failure");
            Step("**** Precondition ****\n");

            Step("-> Create data for testing");
            DeleteGeozones("GZNA01*");
            CreateNewGeozone(geozone);
            CreateNewController(controller, geozone);
            SetValueToController(controller, "TimeZoneId", "UTC", Settings.GetServerTime());
            CreateNewDevice(DeviceType.Streetlight, streetlight1, controller, geozone);
            CreateNewDevice(DeviceType.Streetlight, streetlight2, controller, geozone);
            var curCtrlDateTime = Settings.GetCurrentControlerDateTime(controller);
            var value1 = SLVHelper.GenerateInteger(999).ToString("N2");
            var value2 = SLVHelper.GenerateInteger(999).ToString("N2");
            var request = SetValueToDevice(controller, streetlight1, failure1Id, value1, curCtrlDateTime);
            VerifyEqual(string.Format("-> Verify the request is sent successfully (attribute: {0}, value: {1}, time: {2})", failure1Name, value1, curCtrlDateTime), true, request);
            request = SetValueToDevice(controller, streetlight2, failure2Id, value2, curCtrlDateTime);
            VerifyEqual(string.Format("-> Verify the request is sent successfully (attribute: {0}, value: {1}, time: {2})", failure2Name, value2, curCtrlDateTime), true, request);

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.AlarmManager, App.Alarms);

            Step("1. Go to Alarm Manager app");
            Step("2. Expected Alarm Manager page is routed and loaded successfully");
            var alarmManagerPage = desktopPage.GoToApp(App.AlarmManager) as AlarmManagerPage;

            Step("3. Select a geozone");
            alarmManagerPage.GeozoneTreeMainPanel.SelectNode(geozone);

            Step("4. Add an alarm");
            alarmManagerPage.GridPanel.ClickAddAlarmToolbarButton();
            alarmManagerPage.WaitForPreviousActionComplete();

            Step("5. Verify Add Alarm widget appears");
            VerifyEqual("5. Verify Alarm Details Panel appears", true, alarmManagerPage.AlarmEditorPanel.IsPanelVisible());

            Step("6. Specify report parameters");
            Step(" - Name: Any {{date time span}}");
            Step(" - Type: Device alarm: data analysis versus previous day");
            Step(" - Action: Notify by eMail");
            Step(" - General tab:");
            Step("  + Auto-acknowledge: unchecked");
            Step("  + Refresh rate: 30 seconds");
            Step(" - Trigger condition tab:");
            Step("  + Message: Any");
            Step("  + Failures: selected from the list e.g. Lamp failure (*should be configurable*)");
            Step("  + Devices: selected from the list (*should be configurable*)");
            Step(" - Actions tab:");
            Step("  + From: qa@streetlightmonitoring.com");
            Step("  + To: Any valid mailbox");
            Step("  + Subject: Testcase name");
            Step("  + Message: add 3 rows");
            Step("    + Time: ${ET} ");
            Step("    + Devices: ${FD}");
            Step("    + Failures: ${FMS}");

            //Basic info
            alarmManagerPage.AlarmEditorPanel.EnterNameInput(alarmName);
            alarmManagerPage.AlarmEditorPanel.SelectTypeDropDown(alarmType);
            alarmManagerPage.WaitForPreviousActionComplete();
            alarmManagerPage.AlarmEditorPanel.SelectActionDropDown(alarmAction);

            //General tab
            //General tab is active by default
            alarmManagerPage.AlarmEditorPanel.SelectTab("General");
            alarmManagerPage.AlarmEditorPanel.TickAutoAcknowledgeCheckbox(autoAckowledge);
            alarmManagerPage.AlarmEditorPanel.SelectRefreshRateDropDown(refreshRate);

            //Trigger tab
            alarmManagerPage.AlarmEditorPanel.SelectTab("Trigger condition");
            alarmManagerPage.AlarmEditorPanel.EnterMessageInput(message);
            alarmManagerPage.AlarmEditorPanel.SelectFailuresListDropDown(failure1Name);
            alarmManagerPage.AlarmEditorPanel.SelectFailuresListDropDown(failure2Name);
            alarmManagerPage.AlarmEditorPanel.SelectDevicesListDropDown(string.Format("{0} [@{1}]", streetlight1, controller));
            alarmManagerPage.AlarmEditorPanel.SelectDevicesListDropDown(string.Format("{0} [@{1}]", streetlight2, controller));

            //Actions tab
            alarmManagerPage.AlarmEditorPanel.SelectTab("Actions");
            alarmManagerPage.AlarmEditorPanel.EnterFromInput(mailFrom);
            alarmManagerPage.AlarmEditorPanel.SelectToListDropDown(mailTo);
            alarmManagerPage.AlarmEditorPanel.EnterSubjectInput(mailSubject);
            alarmManagerPage.AlarmEditorPanel.EnterEmailMessageInput(mailContent);

            Step("7. Click Save");
            alarmManagerPage.AlarmEditorPanel.ClickSaveButton();
            alarmManagerPage.WaitForPreviousActionComplete();
            var alarmCreated = Settings.GetServerTime().ToString("G");

            Step("8. After 30 seconds or a bit longer, go to Alarms page and select the testing geozone");
            Wait.ForAlarmTrigger();
            var alarmsPage = alarmManagerPage.AppBar.SwitchTo(App.Alarms) as AlarmsPage;
            alarmsPage.GridPanel.ClickReloadDataToolbarButton();
            alarmsPage.WaitForPreviousActionComplete();

            Step("9. Verify 2 rows of alarms are added to the grid of Alarm panel and each row displays with:");
            Step(" - Name: Alarm name from creating the alarm in Alarm Manager app");
            Step(" - Geozone: testing geozone");
            Step(" - Priority: 0");
            Step(" - Generator: Alarm name '-' random numbers");
            Step(" - Creation Date: the time the alarm is triggered with format {M/d/yyyy hh:mm:ss tt}");
            Step(" - State: X red icon (active status)");
            Step(" - Last Change: equal to Creation Date");
            Step(" - User: -");
            Step(" - Info: message in 'Trigger condition' tab of Alarm Manager app when alarm is created");
            Step(" - Trigger Time: empty");
            Step(" - Trigger Info: empty");

            var iconList = alarmsPage.GridPanel.GetListOfAlarmGridIconColumn(alarmName);
            VerifyEqual("9. Verify 2 rows of alarms are added to the grid of Alarm panel", 2, iconList.Count);
            VerifyEqual("9. Verify State: X red icon (active status)", true, iconList.All(p => p.Contains("status-error.png")));

            var dtGridView = alarmsPage.GridPanel.BuildAlarmDataTable();
            var rows = dtGridView.Select(string.Format("Name = '{0}'", alarmName));
            if (rows.Count() == 2)
            {
                foreach (var dr in rows)
                {
                    var creationDate = dr["Creation Date"].ToString();
                    VerifyEqual("9. Verify Geozone is " + geozone, geozone, dr["Geozone"].ToString());
                    VerifyEqual("9. Verify Priority: 0", "0", dr["Priority"].ToString());
                    VerifyTrue("9. Verify Generator: Alarm name '-' random numbers", dr["Generator"].ToString().Contains(alarmName), alarmName, dr["Generator"].ToString());
                    VerifyTrue("9. Verify Creation Date: the time the alarm is triggered with format {M/d/yyyy h:mm:ss tt}", Settings.CheckDateTimeMatchFormats(creationDate, "M/d/yyyy h:mm:ss tt"), "Format: M/d/yyyy h:mm:ss tt", creationDate);
                    VerifyEqual("9. Verify Last Change: equal to Creation Date", dr["Creation Date"].ToString(), dr["Last Change"].ToString());
                    VerifyEqual("9. Verify User: -", "-", dr["User"].ToString());
                    VerifyEqual("9. Verify Info: message in 'Trigger condition' tab of Alarm Manager app when alarm is created", message, dr["Info"].ToString());
                    VerifyEqual("9. Verify Trigger Time: empty", "", dr["Trigger Time"].ToString());
                    VerifyEqual("9. Verify Trigger Info: empty", "", dr["Trigger Info"].ToString());
                }                
            }

            Step("10. Check the email");
            Step("11. Verify There are 2 emails sent. The subject and the contain is displayed with");
            Step(" - Subject: subject set up in Actions tab of Alarm Manager");
            Step(" - Contains:");
            Step("  + Time: the time the alarm is trigger with format yyyy-MM-dd HH:mm:ss");
            Step("  + Devices: Streetlight{#}'s name");
            Step("  + Failures: failure's name");
            var foundEmails = EmailUtility.GetNewEmails(alarmName);
            var isEmailsSent = foundEmails != null;
            var emailCheckingDateTime = Settings.GetServerTime().ToString("G");
            VerifyEqual(string.Format("11. Verify a sent alarm email is found in mailbox ({0}, Alarm created: {1}, Email checking: {2})", alarmName, alarmCreated, emailCheckingDateTime), true, isEmailsSent);
            if (isEmailsSent)
            {
                if (foundEmails.Count == 2)
                {
                    var bodyList = foundEmails.Select(p => p.Body);
                    var emailTimes = bodyList.Select(p => p.SplitAndGetAt("|", 0).Trim());
                    var emailStreetlights = bodyList.Select(p => p.SplitAndGetAt("|", 1).Trim());
                    var emailFailures = bodyList.Select(p => p.SplitAndGetAt("|", 2).Trim());
                    VerifyTrue("11. Verify Email Time: datetime when the simulated command sent with format: yyyy-MM-dd HH:mm:ss", emailTimes.All(p => Settings.CheckDateTimeMatchFormats(p, "yyyy-MM-dd HH:mm:ss")), "Format: yyyy-MM-dd HH:mm:ss", string.Join(", ", emailTimes));
                    VerifyTrue("11. Verify Email Streetlight Name: testing Streetlight's name", emailStreetlights.All(p => streetlights.IndexOf(p) >= 0), string.Join(", ", streetlights), string.Join(", ", emailStreetlights));
                    VerifyTrue("11. Verify Email Failure Name: failure's name", emailFailures.All(p => failures.Any(x => x.IndexOf(p) >= 0)), string.Join(", ", failures), string.Join(", ", emailFailures));
                    EmailUtility.CleanInbox(alarmName);
                }
                else
                    Warning(string.Format("11. Acutal emails sent is {0} (Expected: 2 emails)", foundEmails.Count));
            }
            else
                Warning("11. There is no emails sent");

            Step("12. Send the 2 simulated commands to solve the failures");
            curCtrlDateTime = Settings.GetCurrentControlerDateTime(controller);
            request = SetValueToDevice(controller, streetlight1, failure1Id, "false", curCtrlDateTime);
            VerifyEqual(string.Format("-> Verify the request is sent successfully (attribute: {0}, value: {1}, time: {2})", failure1Name, "false", curCtrlDateTime), true, request);
            request = SetValueToDevice(controller, streetlight2, failure2Id, "false", curCtrlDateTime);
            VerifyEqual(string.Format("-> Verify the request is sent successfully (attribute: {0}, value: {1}, time: {2})", failure2Name, "false", curCtrlDateTime), true, request);
            
            Step("13. Refresh page, then go to Alarm app and the testing geozone");
            desktopPage = Browser.RefreshLoggedInCMS();
            alarmsPage = desktopPage.GoToApp(App.Alarms) as AlarmsPage;
            alarmManagerPage.GeozoneTreeMainPanel.SelectNode(geozone);

            Step("14. Verify The 'Acknowledge' button on the top right corner of the Alarm panel is disabled");
            VerifyEqual("14. Verify The 'Acknowledge' button on the top right corner of the Alarm panel is disabled", true, !alarmsPage.GridPanel.IsAcknowledgeToolbarButtonEnabled());

            Step("15. Select an alarm on the grid");
            alarmsPage.GridPanel.ClickAlarmGridRecordHasErrorIcon(alarmName);

            Step("16. Press the 'Acknowledge' button on the top right corner of the Alarm panel");
            alarmsPage.GridPanel.ClickAcknowledgeToolbarButton();
            alarmsPage.WaitForPopupDialogDisplayed();

            Step("17. Verify the acknowledge pop-up displays with");
            Step(" - Title: 'Acknowledge Alarms'");
            Step(" - Message label and a textbox");
            VerifyEqual("17. Verify the acknowledge pop-up displays - Title: 'Acknowledge Alarms", "Acknowledge Alarms", alarmsPage.Dialog.GetDialogTitleText());            
            VerifyEqual("17. Verify the acknowledge pop-up displays - Message label and a textbox", true, alarmsPage.Dialog.IsAcknowledgeMessageInputDisplayed());
            
            Step("18. Press X button on the pop-up");
            alarmsPage.Dialog.ClickCloseButton();
            alarmsPage.WaitForPopupDialogDisappeared();

            Step("19. Verify The pop-up is closed");
            VerifyEqual("19. Verify The pop-up is closed", false, alarmsPage.IsPopupDialogDisplayed());

            Step("20. Press the 'Acknowledge' button on the top right corner of the Alarm panel");
            alarmsPage.GridPanel.ClickAcknowledgeToolbarButton();
            alarmsPage.WaitForPopupDialogDisplayed();

            Step("21. Input a message into the textbox, and press Send button. Ex: Alarm_01-Streetlight-Acknowledged");
            var msgAcknowledge = SLVHelper.GenerateUniqueName("Alarm-Acknowledged");
            alarmsPage.Dialog.EnterAcknowledgeMessageInput(msgAcknowledge);
            alarmsPage.Dialog.ClickSendButton();
            alarmsPage.Dialog.WaitForPopupMessageDisplayed();
            var acknowledgeTime = Settings.GetCurrentControlerDateTime(controller);

            Step("22. Verify The pop-up displays 'Acknowledgement sent!'");
            VerifyEqual("22. Verify The pop-up displays 'Acknowledgement sent!'", "Acknowledgement sent!", alarmsPage.Dialog.GetMessageText());

            Step("23. Press OK button");
            alarmsPage.Dialog.ClickOkButton();
            alarmsPage.WaitForPopupDialogDisappeared();
            
            Step("24. Verify All pop-ups are closed and the selected row on the grid is updated with");
            Step(" - State: Green Checked icon");
            Step(" - Info: is filled with the message inputted. Ex: Alarm_01-Streetlight-Acknowledged");
            var icon = alarmsPage.GridPanel.GetListOfAlarmGridIconColumn(alarmName).FirstOrDefault();
            var info = alarmsPage.GridPanel.GetListOfColumnDataAlarm("Info").FirstOrDefault(); 
            VerifyEqual("24. Verify The Status is changed to Acknowledged (Green Check icon)", true, icon.Contains("status-ok.png"));
            VerifyEqual("24. Verify Info: is filled with the message inputted. Ex: Alarm_01-Streetlight-Acknowledged", msgAcknowledge, info);
            
            Step("25. Press Refresh button on the grid and press the Red Bell icon");
            alarmsPage.GridPanel.ClickReloadDataToolbarButton();
            alarmsPage.WaitForPreviousActionComplete();
            alarmsPage.GridPanel.ClickShowAllAlarmsToolbarOption();
            alarmsPage.WaitForPreviousActionComplete();

            Step("26. Verify The testing row is updated with");
            Step(" - Last Change: time when acknowledged with format: M/d/yyyy h:mm:ss tt");
            Step(" - User: the user who acknowledged the alarm");
            Step(" - Trigger Time: the time the alarm is trigger with format: M/d/yyyy h:mm:ss tt");
            Step(" - Trigger Info: Alarm message");
            dtGridView = alarmsPage.GridPanel.BuildAlarmDataTable();
            var row = dtGridView.Select(string.Format("Name = '{0}'", alarmName)).FirstOrDefault();
            if (row != null)
            {
                var lastChangeDate = row["Last Change"].ToString();
                var triggerTime = row["Trigger Time"].ToString();
                VerifyTrue("26. Verify Last Change: time when acknowledged with format: M/d/yyyy h:mm:ss tt", Settings.CheckDateTimeMatchFormats(lastChangeDate, "M/d/yyyy h:mm:ss tt"), "Format: M/d/yyyy h:mm:ss tt", lastChangeDate);
                VerifyEqual("26. Verify User: the user who acknowledged the alarm", Settings.Users["DefaultTest"].Username, row["User"].ToString());
                VerifyTrue("26. Verify Trigger Time: time the alarm is trigger with format: M/d/yyyy h:mm:ss tt", Settings.CheckDateTimeMatchFormats(triggerTime, "M/d/yyyy h:mm:ss tt"), "Format: M/d/yyyy h:mm:ss tt", triggerTime);
                VerifyEqual("26. Verify Trigger Info: Alarm message", message, row["Trigger Info"].ToString());
            }
            else
            {
                Warning(string.Format("26. There is no row with alarm '{0}'", alarmName));
            }

            try
            {
                DeleteAlarm(alarmName);
                DeleteGeozone(geozone);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("Alarm - 1426882 - Cover ticket 1396449 - 1418405 - Change alarm import to allow empty id")]
        [Category("RunAlone")]
        public void Alarm_1426882()
        {
            var importedFileName = Settings.GetFullPath(Settings.CSV_FILE_PATH + "Alarm1426882-New.csv");
            var importedFileNameUpdated = Settings.GetFullPath(Settings.CSV_FILE_PATH + "Alarm1426882-Update.csv");
            var alarmName = "Alarm-1426882";
            var alarmNameUpdated = alarmName + "-updated";

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - Create an alarm of any types. Ex: 'Controller alarm: no data received', then export this alarm to csv file for testing.");
            Step(" - Remove Id column and its value in csv file.");
            Step(" - The name's value and the alarmName's value of csv file should be not existing in the system.");            
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.AlarmManager, App.Alarms);

            Step("1. Go to Alarm Manager app");
            Step("2. Verify Alarm Manager page is routed and loaded successfully");
            var alarmManagerPage = desktopPage.GoToApp(App.AlarmManager) as AlarmManagerPage;
            if (alarmManagerPage.GridPanel.IsAlarmManagerGridHasTextPresent("Name", alarmName)) alarmManagerPage.DeleteAlarm(alarmName);
            if (alarmManagerPage.GridPanel.IsAlarmManagerGridHasTextPresent("Name", alarmNameUpdated)) alarmManagerPage.DeleteAlarm(alarmNameUpdated);

            Step("3. Click Import button and browse to the csv file then click OK");           
            alarmManagerPage.GridPanel.ClickImportToolbarButton();
            SLVHelper.OpenFileFromFileDialog(importedFileName);
            alarmManagerPage.WaitUntilOpenFileDialogDisappears();
            alarmManagerPage.WaitForHeaderMessageDisplayed();
            alarmManagerPage.WaitForPreviousActionComplete();                     

            Step("4. Verify");
            Step(" - Preloader appears");
            Step(" - Success message is shown with content '{{n1}}/{{n2}} alarms have been successfully imported'");
            Step(" - Preloader disappears");
            var expectedImportMessage = string.Format("{0} / {1} alarms have been successfully imported", 1, 1);
            VerifyEqual("4. Verify success message after importing alarm", expectedImportMessage, alarmManagerPage.GetHeaderMessage());
            Wait.ForProgressCompleted();
            Wait.ForLoadingIconDisappeared();
            alarmManagerPage.WaitForHeaderMessageDisappeared();          

            Step("5. Verify There is a new row of the imported alarm added in the grid with the name of the imported alarm");
            VerifyEqual("5. Verify There is a new row of the imported alarm added in the grid with the name of the imported alarm", true, alarmManagerPage.GridPanel.IsAlarmManagerGridHasTextPresent("Name", alarmName));

            Step("6. In csv file update the following columns' values");
            Step(" - triggerCondition.hoursDelay: 1");
            Step(" - newAlarmWhenAcknowledged: true");
            Step(" - triggerCondition.fullFilledMessageTemplate: Trigger condition message updated");
            Step(" - alarmName: 'Alarm name'-updated");
            Step(" - action[0].subject: Action subject updated");
            Step(" - autoAcknowledge: false");
            Step(" - action[0].message: Action message updated");
            Step("7. Click Import button and browse to the csv file then click OK");
            alarmManagerPage.GridPanel.ClickImportToolbarButton();
            SLVHelper.OpenFileFromFileDialog(importedFileNameUpdated);
            alarmManagerPage.WaitUntilOpenFileDialogDisappears();
            alarmManagerPage.WaitForHeaderMessageDisplayed();
            alarmManagerPage.WaitForPreviousActionComplete();            

            Step("8. Verify Success message is shown with content '{{n1}}/{{n2}} alarms have been successfully imported'");
            VerifyEqual("8. Verify success message after importing alarm", expectedImportMessage, alarmManagerPage.GetHeaderMessage());
            Wait.ForProgressCompleted();
            Wait.ForLoadingIconDisappeared();
            alarmManagerPage.WaitForHeaderMessageDisappeared();

            Step("9. Verify The row of the updated alarm is updated with the new name");
            VerifyEqual("9. Verify The row of the updated alarm is updated with the new name", true, alarmManagerPage.GridPanel.IsAlarmManagerGridHasTextPresent("Name", alarmNameUpdated));
            VerifyEqual("9. Verify old name does not exist", true, !alarmManagerPage.GridPanel.IsAlarmManagerGridHasTextPresent("Name", alarmName));

            Step("10. Click the row");
            alarmManagerPage.GridPanel.ClickGridRecord(alarmNameUpdated);
            alarmManagerPage.WaitForPreviousActionComplete();

            Step("11. Verify The following values are updated correctly with the values in csv file");
            Step(" - Name: 'Alarm name'-updated");
            Step(" - General tab");
            Step("  + New alarm if re-triggerd: checked");
            Step("  + Auto-acknowledge: unchecked");            
            Step(" - Trigger Condition tab");
            Step("  + Message: Trigger condition message updated");
            Step("  + Delay (hours): 1");            
            Step(" - Action tab");
            Step("  + Subject: Action subject updated");
            Step("  + Message: Action message updated");
            VerifyEqual("11. Verify Name: 'Alarm name'-updated", alarmNameUpdated, alarmManagerPage.AlarmEditorPanel.GetNameValue());
            alarmManagerPage.AlarmEditorPanel.SelectTab("General");
            VerifyEqual("11. Verify New alarm if re-triggerd: checked", true, alarmManagerPage.AlarmEditorPanel.GetNewAlarmIfRetriggerValue());
            VerifyEqual("11. Verify Auto-acknowledge: unchecked", false, alarmManagerPage.AlarmEditorPanel.GetAutoAcknowledgeValue());
            alarmManagerPage.AlarmEditorPanel.SelectTab("Trigger condition");
            VerifyEqual("11. Verify Message: trigger condition message updated", "Trigger condition message updated", alarmManagerPage.AlarmEditorPanel.GetMessageValue());
            VerifyEqual("11. Verify Delay (hours): 1", "1", alarmManagerPage.AlarmEditorPanel.GetHoursDelayNumbericValue());
            alarmManagerPage.AlarmEditorPanel.SelectTab("Actions");
            VerifyEqual("11. Verify Subject: Action subject updated", "Action subject updated", alarmManagerPage.AlarmEditorPanel.GetSubjectValue());
            VerifyEqual("11. Verify Message: Action message updated", "Action message updated", alarmManagerPage.AlarmEditorPanel.GetEmailMessageValue());            

            Step("12. Refresh browser then go to Alarm Manager app again");
            desktopPage = Browser.RefreshLoggedInCMS();
            alarmManagerPage = desktopPage.GoToApp(App.AlarmManager) as AlarmManagerPage;

            Step("13. Verify the imported alarm is still present in the grid");
            VerifyEqual("13. Verify the imported alarm is still present in the grid", true, alarmManagerPage.GridPanel.IsAlarmManagerGridHasTextPresent("Name", alarmNameUpdated));

            try
            {
                DeleteAlarm(alarmNameUpdated);
            }
            catch { }
        }

        #endregion //Test Cases

        #region Private methods   

        /// <summary>
        /// Wait for alarm trigger
        /// </summary>
        private void WaitForAlarmTrigger(int seconds)
        {
            Wait.ForSeconds(seconds);
        }

        #region Verify methods        

        #endregion //Verify methods

        #region Input XML data 

        private Dictionary<string, string> GetTestDataOfAlarm01()
        {
            var testCaseName = "Alarm01";
            var xmlUtility = new XmlUtility(Settings.ALARM_TEST_DATA_FILE_PATH);
            var testData = new Dictionary<string, string>();

            var nodeAlarmInfo = xmlUtility.GetSingleNode(string.Format(Settings.ALARM_XPATH_PREFIX, testCaseName, "AlarmInfo"));
            testData.Add("Alarm.type", nodeAlarmInfo.GetAttrVal("type"));
            testData.Add("Alarm.action", nodeAlarmInfo.GetAttrVal("action"));

            var nodeGeneral = nodeAlarmInfo.GetChildNode("General");
            testData.Add("Alarm.General.auto-acknowledge", nodeGeneral.GetAttrVal("auto-acknowledge"));
            testData.Add("Alarm.General.refresh-rate", nodeGeneral.GetAttrVal("refresh-rate"));

            var nodeTrigger = nodeAlarmInfo.GetChildNode("Trigger");
            testData.Add("Alarm.Trigger.message", nodeTrigger.GetAttrVal("message"));
            testData.Add("Alarm.Trigger.failure1", nodeTrigger.GetAttrVal("failure1"));
            testData.Add("Alarm.Trigger.failure2", nodeTrigger.GetAttrVal("failure2"));

            var nodeActions = nodeAlarmInfo.GetChildNode("Actions");
            testData.Add("Alarm.Actions.mail-from", nodeActions.GetAttrVal("mail-from"));
            testData.Add("Alarm.Actions.mail-to", nodeActions.GetAttrVal("mail-to"));
            testData.Add("Alarm.Actions.mail-subject", nodeActions.GetAttrVal("mail-subject"));
            testData.Add("Alarm.Actions.mail-content", nodeActions.GetAttrVal("mail-content"));

            return testData;
        }

        #endregion //Input XML data       

        #endregion //Private methods
    }
}