using NUnit.Framework;
using StreetlightVision.Extensions;
using StreetlightVision.Models;
using StreetlightVision.Pages;
using StreetlightVision.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Xml;

namespace StreetlightVision.Tests.Acceptance
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    public class TC4Tests : TestBase
    {
        #region Variables

        private readonly string _failureExportedFilePattern = "*-Export*.csv";
        
        #endregion //Variables

        #region Contructors

        #endregion //Contructors        

        #region Test Cases        

        [Test, DynamicRetry]
        [Description("TS 4.1.1 Number of failures on Failure Tracking and Failure Analysis icons")]
        public void TS4_01_01()
        {
            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.FailureAnalysis, App.FailureTracking);

            Step("1. Note numbers in Failures Analysis and Failures Tracking tiles in Desktop page");
            var warningCount = desktopPage.GetFailureTrackingWarningsCount();
            var wariningPercent = desktopPage.GetFailureTrackingWarningsPercent();
            var outageCount = desktopPage.GetFailureTrackingOutagesCount();
            var outagePercent = desktopPage.GetFailureTrackingOutagesPercent();
            var failureCount = desktopPage.GetSchedulingManagerFailuresCount();

            Step("2. Go to Energy app");
            Step("3. **Expected** Energy page is routed and loaded successfully");
            var failureAnalysisPage = desktopPage.GoToApp(App.FailureAnalysis) as FailureAnalysisPage;

            Step("4. Select the root geozone from geozone tree");
            failureAnalysisPage.GeozoneTreeMainPanel.SelectNode(Settings.RootGeozoneName);

            Step("5. Expected The grid in main panel displays aggregated statistics");
            Step("6. Note total of faulty count, faulty ratio & critical count, critical ratio in Total row");
            // Call this because of caching on IE causing all error unchecked by tests before
            if (Settings.Browser == "IE")
            {
                failureAnalysisPage.GridPanel.CheckAllColumnsInShowHideColumnsMenu();
            }

            var tblGrid = failureAnalysisPage.GridPanel.BuildDataTableFromTotalRow();
            var faultyCount = tblGrid.Rows[0]["Faulty count"];
            var faultyRatio = tblGrid.Rows[0]["Faulty ratio"];
            var criticalCount = tblGrid.Rows[0]["Critical count"];
            var criticalRatio = tblGrid.Rows[0]["Critical ratio"];

            Info("Verifyings of steps 7 to 9 can't be performed due to Jean-Marc's email:");
            Info(@"
                Hi Chung,

                I believe this is because of all the devices that are directly under ""GeoZones"". If you remove them or move them to a sub-geozone, the percentages should be identical.

                Cheers,
                Jean - Marc
            ");

            Step("7. Verify number of failures shown in Failure Analysis app's tile");
            Step("8. Expected In case critical count at step #5 is greater than 0, this number is shown in Failure Analysis app's tile ");
            Step("9. Verify number of failures shown in Failure Tracking app's tile");
            Step(". Expected");
            Step("\t - In case faulty count at step #5 is greater than 0, number and percentage of warnings is shown in Failure Tracking app's tile and the values are the same with values noted at step #5");
            Step("\t- In case critical count at step #5 is greater than 0, number and percentage of outages shown in Failure Tracking app's tile and the values are the same with values noted at step #5");
        }

        [Test, DynamicRetry]
        [Description("TS 4.2.1 Failure Analysis - Go to app")]
        public void TS4_02_01()
        {
            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);

            desktopPage.InstallAppsIfNotExist(App.FailureAnalysis);

            Step("1. Go to Failure Analysis app");
            Step("2. **Expected** Failure Analysis page is routed and loaded successfully");
            var startLogTime = DateTime.Now;
            var failureAnalysisPage = desktopPage.GoToApp(App.FailureAnalysis) as FailureAnalysisPage;

            Step("3. Log the time to load Failure Analysis page in running result");
            var endLogTime = DateTime.Now;

            Info(string.Format("Time to go to and load Failure Analysis is {0} seconds", (endLogTime - startLogTime).TotalSeconds));
        }

        [Test, DynamicRetry]
        [Description("TS 4.3.1 Failure Analysis - Export aggregated failure report")]
        [NonParallelizable]
        public void TS4_03_01()
        {
            var testData = GetTestDataOfTestTS4_03_01();

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);

            Step("1. Go to Failure Analysis app");
            Step("2. **Expected** Failure Analysis page is routed and loaded successfully");
            desktopPage.InstallAppsIfNotExist(App.FailureAnalysis);
            var failureAnalysisPage = desktopPage.GoToApp(App.FailureAnalysis) as FailureAnalysisPage;

            Step("3. Select the root geozone");
            foreach (var geozone in testData.Values)
            {
                failureAnalysisPage.GeozoneTreeMainPanel.SelectNode(geozone as string);

                var listOfCheckedColumns = failureAnalysisPage.GridPanel.GetAllCheckedColumnsInShowHideColumnsMenu();
                var listOfDisplayedColumnsInGrid = GetListOfColumnsOfDataTable(failureAnalysisPage.GridPanel.BuildDataTableFromGrid());

                Step("4. Expected The aggregated failure report is displayed. Each row is a child geozone of the root geozone");
                var geozoneListInGrid = failureAnalysisPage.GridPanel.GetListOfGeozones();
                var geozoneListOfSelectedGeozone = failureAnalysisPage.GeozoneTreeMainPanel.GetChildNodeNamesOfSelectedNode(NodeType.GeoZone);

                if (listOfDisplayedColumnsInGrid.Contains("Geozone"))//Preventing fail from Geozone column is unchecked
                {
                    VerifyEqual("4. Verify geozones displayed in selected geozone & grid", geozoneListInGrid, geozoneListOfSelectedGeozone, false);
                }

                // Check checked vs displayed columns
                VerifyEqual("4. Verify checked columns match displayed columns", listOfCheckedColumns, listOfDisplayedColumnsInGrid);

                Step("5. Click Export");
                Step("6. Expected A CSV file is download. Its content reflects what is being shown in the grid");
                Step("7. Click Show / hide columns menu and un/ check columns");
                Step("8. Expected Un / checked columns are hidden/ shown on the grid");
                Step("9. Click Export");
                Step("10. Expected A CSV file is download. Its content reflects what is being shown in the grid");

                /*
                 Check all columns
                 */
                failureAnalysisPage.GridPanel.CheckAllColumnsInShowHideColumnsMenu();

                // Check checked vs displayed columns
                listOfCheckedColumns = failureAnalysisPage.GridPanel.GetAllCheckedColumnsInShowHideColumnsMenu();
                listOfDisplayedColumnsInGrid = GetListOfColumnsOfDataTable(failureAnalysisPage.GridPanel.BuildDataTableFromGrid());

                VerifyEqual("[SC-736] 8. Verify checked columns match displayed columns", listOfCheckedColumns, listOfDisplayedColumnsInGrid);

                SLVHelper.DeleteAllFilesByPattern(_failureExportedFilePattern);

                failureAnalysisPage.GridPanel.ClickExportToolbarButton();
                SLVHelper.SaveDownloads();

                var tblGrid = failureAnalysisPage.GridPanel.BuildDataTableFromGrid();
                var tblCSV = SLVHelper.BuildDataTableFromLastDownloadedCSV(_failureExportedFilePattern);
                var tblGridFormatted = tblGrid.Copy();
                var tblCSVFormatted = tblCSV.Copy();

                FormatDataTablesBeforeVerifying(ref tblGridFormatted, ref tblCSVFormatted);

                VerifyEqual("10. Verify exported CSV file reflects what is being shown in the grid", tblGridFormatted, tblCSVFormatted);

                // Uncheck all columns
                failureAnalysisPage.GridPanel.UncheckAllColumnsInShowHideColumnsMenu();

                // Check checked vs displayed columns
                listOfCheckedColumns = failureAnalysisPage.GridPanel.GetAllCheckedColumnsInShowHideColumnsMenu();
                listOfDisplayedColumnsInGrid = GetListOfColumnsOfDataTable(failureAnalysisPage.GridPanel.BuildDataTableFromGrid());

                VerifyEqual("[SC-736] 10. Verify checked columns match displayed columns", listOfCheckedColumns, listOfDisplayedColumnsInGrid);

                SLVHelper.DeleteAllFilesByPattern(_failureExportedFilePattern);

                failureAnalysisPage.GridPanel.ClickExportToolbarButton();
                SLVHelper.SaveDownloads();

                tblGrid = failureAnalysisPage.GridPanel.BuildDataTableFromGrid();
                tblCSV = SLVHelper.BuildDataTableFromLastDownloadedCSV(_failureExportedFilePattern);
                tblGridFormatted = tblGrid.Copy();
                tblCSVFormatted = tblCSV.Copy();

                FormatDataTablesBeforeVerifying(ref tblGridFormatted, ref tblCSVFormatted);

                VerifyEqual("10. Verify exported CSV file reflects what is being shown in the grid", tblGridFormatted, tblCSVFormatted);

                Info("Steps from 11 to 18 are performed in foreach loop");
            }
        }

        [Test, DynamicRetry]
        [Description("TS 4.4.1 Failure Analysis - Display failures by type")]
        [NonParallelizable]
        public void TS4_04_01()
        {
            var testData = GetTestDataOfTestTS4_04_01();
            
            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);

            Info("TS 4.4.1");
            Step("1. Go to Failure Analysis app");
            Step("2. Expected Failure Analysis page is routed and loaded successfully");
            desktopPage.InstallAppsIfNotExist(App.FailureAnalysis);
            var failureAnalysisPage = desktopPage.GoToApp(App.FailureAnalysis) as FailureAnalysisPage;

            Step("3. Select a geozone which contains sub - geozones(not the root geozone)");
            var geozone = testData["Geozone1"] as string;
            failureAnalysisPage.GeozoneTreeMainPanel.SelectNode(geozone);

            Step("4. Expected The aggredated failure report is displayed in the main panel");
            VerifyEqual("4. Verify Display Failures By Type Toolbar Button visible", true, failureAnalysisPage.GridPanel.IsDisplayFailuresByTypeToolbarButtonVisible());
            VerifyEqual("4. Verify Display Warning Failures Toolbar Button visible", true, failureAnalysisPage.GridPanel.IsDisplayWarningFailuresToolbarButtonVisible());
            VerifyEqual("4. Verify Display Critical Failures Toolbar Button visible", true, failureAnalysisPage.GridPanel.IsDisplayCriticalFailuresToolbarButtonVisible());

            Step("5. Toggle OFF all displaying \"Display failures by type\"(White triangle icon), \"Display warning failures\"(Yellow triangle icon) and \"Display critical failures\"(Red triangle icon)");
            if (failureAnalysisPage.GridPanel.IsDisplayFailuresByTypeToolbarButtonToggleOn()) failureAnalysisPage.GridPanel.ClickDisplayFailuresByTypeToolbarButton();
            if (failureAnalysisPage.GridPanel.IsDisplayWarningFailuresToolbarButtoToggleOn()) failureAnalysisPage.GridPanel.ClickDisplayWarningFailuresToolbarButton();
            if (failureAnalysisPage.GridPanel.IsDisplayCriticalFailuresToolbarButtonToggleOn()) failureAnalysisPage.GridPanel.ClickDisplayCriticalFailuresToolbarButton();
            
            Step("6. Expected The report displays columns: Geozone, Devices, Faulty count, Faulty ratio, Critical count, Critical ratio");
            var expectedGridHeaders = new List<string> { "Geozone", "Devices", "Faulty count", "Faulty ratio", "Critical count", "Critical ratio" };
            var actualGridHeaders = failureAnalysisPage.GridPanel.GetListOfColumnsHeader();
            VerifyEqual("4. Verify The report displays columns as expected", expectedGridHeaders, actualGridHeaders);
            
            Step("7. Click Export");
            SLVHelper.DeleteAllFilesByPattern(_failureExportedFilePattern);
            failureAnalysisPage.GridPanel.ClickExportToolbarButton();
            SLVHelper.SaveDownloads();

            Step("8. Expected A CSV file is download.Its content reflects what is being shown in the grid");                  
            var tblGrid = failureAnalysisPage.GridPanel.BuildDataTableFromGrid();
            var tblCSV = SLVHelper.BuildDataTableFromLastDownloadedCSV(_failureExportedFilePattern);
            var tblGridFormatted = tblGrid.Copy();
            var tblCSVFormatted = tblCSV.Copy();
            FormatDataTablesBeforeVerifying(ref tblGridFormatted, ref tblCSVFormatted);
            VerifyEqual("8. Verify exported CSV file reflects what is being shown in the grid", tblGridFormatted, tblCSVFormatted);

            Step("9. Toggle ON \"Display failures by type\"(White triangle icon)");
            failureAnalysisPage.GridPanel.ClickDisplayFailuresByTypeToolbarButton();
            failureAnalysisPage.WaitForPreviousActionComplete();

            Step("10. Expected The report displays columns: Geozone;Calendar commission failure;Communication failure;Cycling lamp;Day burner;External comms failure;High current;High lamp voltage;High OLC temperature;High power;Invalid calendar;Invalid program;Lamp failure;Low current;Low lamp voltage;Low power;Low power factor;Open circuit;Power Supply Failure;Relay failure");
            expectedGridHeaders = new List<string> { "Geozone", "Calendar commission failure", "Communication failure", "Cycling lamp", "Day burner", "External comms failure", "High current", "High lamp voltage", "High OLC temperature", "High power", "Invalid calendar", "Invalid program", "Lamp failure", "Low current", "Low lamp voltage", "Low power", "Low power factor", "Open circuit", "Power Supply Failure", "Relay failure" };
            actualGridHeaders = failureAnalysisPage.GridPanel.GetListOfColumnsHeader();
            VerifyEqual("10. Verify The report displays columns as expected", expectedGridHeaders, actualGridHeaders);

            Step("11. Click on Show/Hide columns button");
            failureAnalysisPage.GridPanel.CheckAllColumnsInShowHideColumnsMenu();

            Step("12. Expected All the following columns are checked: Geozone;Calendar commission failure;Communication failure;Cycling lamp;Day burner;External comms failure;High current;High lamp voltage;High OLC temperature;High power;Invalid calendar;Invalid program;Lamp failure;Low current;Low lamp voltage;Low power;Low power factor;Open circuit;Power Supply Failure;Relay failure");
            var listOfCheckedColumns = failureAnalysisPage.GridPanel.GetAllCheckedColumnsInShowHideColumnsMenu();
            VerifyEqual("12. Verify All the following columns are checked", expectedGridHeaders, listOfCheckedColumns);
            VerifyEqual("[SC-736] 12. Verify checked columns match displayed columns", listOfCheckedColumns, actualGridHeaders);

            Step("13. Click Export");
            SLVHelper.DeleteAllFilesByPattern(_failureExportedFilePattern);
            failureAnalysisPage.GridPanel.ClickExportToolbarButton();
            SLVHelper.SaveDownloads(false);

            Step("14. Expected A CSV file is download.Its content reflects what is being shown in the grid");    
            tblGrid = failureAnalysisPage.GridPanel.BuildDataTableFromGrid();
            tblCSV = SLVHelper.BuildDataTableFromLastDownloadedCSV(_failureExportedFilePattern);
            tblGridFormatted = tblGrid.Copy();
            tblCSVFormatted = tblCSV.Copy();
            FormatDataTablesBeforeVerifying(ref tblGridFormatted, ref tblCSVFormatted);
            VerifyEqual("14. Verify exported CSV file reflects what is being shown in the grid", tblGridFormatted, tblCSVFormatted);

            Step("15. Toggle ON \"Display warning failures\"(Yellow triangle icon)");
            failureAnalysisPage.GridPanel.ClickDisplayWarningFailuresToolbarButton();
            failureAnalysisPage.WaitForPreviousActionComplete();

            Step("16. Expected The report displays columns: Device, GeoZone, Warnings count, Warnings");
            expectedGridHeaders = new List<string> { "Device", "GeoZone", "Warnings count", "Warnings"};
            actualGridHeaders = failureAnalysisPage.GridPanel.GetListOfColumnsHeader();
            VerifyEqual("14. Verify The report displays columns as expected", expectedGridHeaders, actualGridHeaders);

            Step("17. Click Export");
            SLVHelper.DeleteAllFilesByPattern(_failureExportedFilePattern);
            failureAnalysisPage.GridPanel.ClickExportToolbarButton();
            SLVHelper.SaveDownloads(false);

            Step("18. Expected A CSV file is download.Its content reflects what is being shown in the grid");
            tblGrid = failureAnalysisPage.GridPanel.BuildDataTableFromGrid();
            tblCSV = SLVHelper.BuildDataTableFromLastDownloadedCSV(_failureExportedFilePattern);
            tblGridFormatted = tblGrid.Copy();
            tblCSVFormatted = tblCSV.Copy();
            FormatDataTablesBeforeVerifying(ref tblGridFormatted, ref tblCSVFormatted);
            VerifyEqual("16. Verify exported CSV file reflects what is being shown in the grid", tblGridFormatted, tblCSVFormatted);

            Step("19. Toggle ON \"Display critical failures\"(Red triangle icon)");
            failureAnalysisPage.GridPanel.ClickDisplayCriticalFailuresToolbarButton();
            failureAnalysisPage.WaitForPreviousActionComplete();

            Step("20. Expected The report displays columns: Device, GeoZone, Errors count, Critical errors");
            expectedGridHeaders = new List<string> { "Device", "GeoZone", "Errors count", "Critical errors" };
            actualGridHeaders = failureAnalysisPage.GridPanel.GetListOfColumnsHeader();
            VerifyEqual("20. Verify The report displays columns as expected", expectedGridHeaders, actualGridHeaders);

            Step("21. Click Export");
            SLVHelper.DeleteAllFilesByPattern(_failureExportedFilePattern);
            failureAnalysisPage.GridPanel.ClickExportToolbarButton();
            SLVHelper.SaveDownloads(false);

            Step("22. Expected A CSV file is download.Its content reflects what is being shown in the grid");            
            tblGrid = failureAnalysisPage.GridPanel.BuildDataTableFromGrid();
            tblCSV = SLVHelper.BuildDataTableFromLastDownloadedCSV(_failureExportedFilePattern);
            tblGridFormatted = tblGrid.Copy();
            tblCSVFormatted = tblCSV.Copy();
            FormatDataTablesBeforeVerifying(ref tblGridFormatted, ref tblCSVFormatted);
            VerifyEqual("22. Verify exported CSV file reflects what is being shown in the grid", tblGridFormatted, tblCSVFormatted);
            
            Info("TS 4.5.1");
            Step("[TS 4.5.1] 1. In the report, select a geozone which contains no sub-geozones and has faulty and critical failures (e.g. \"Energy Data Area\")");
            Step("[TS 4.5.1] 2. Expected The selected geozone is active on geozone tree, the grid now displays detailed failure report for the selected geozone");
            Step("[TS 4.5.1] 3. Verify detailed report content");
            Step("[TS 4.5.1] 4. Expected");
            Step(" • In case either Warning or Outages has NOT - OK statuses(not green - checked icon), Failures cells should be not empty");
            Step(" • In case Warning is NOT - OK statuses, Last Update should be filled with format \"d /M/yyyy hh:mm:ss T\"");
            Step(" • In case Outages is NOT - OK statuses, Failed since should be filled with format \"{ {n}h{{n}}m}\", e.g 19h31m");
            geozone = testData["Geozone2"] as string;
            failureAnalysisPage.GeozoneTreeMainPanel.SelectNode(geozone);           
            failureAnalysisPage.GridPanel.CheckAllColumnsInShowHideColumnsMenu();
            listOfCheckedColumns = failureAnalysisPage.GridPanel.GetAllCheckedColumnsInShowHideColumnsMenu();
            var listOfDisplayedColumnsInGrid = GetListOfColumnsOfDataTable(failureAnalysisPage.GridPanel.BuildDataTableFromGrid());
            VerifyEqual("[TS 4.5.1] [SC-736] 4. Verify checked columns match displayed columns", listOfCheckedColumns, listOfDisplayedColumnsInGrid);

            SLVHelper.DeleteAllFilesByPattern(_failureExportedFilePattern);
            failureAnalysisPage.GridPanel.ClickExportToolbarButton();
            SLVHelper.SaveDownloads(false);
            tblGrid = failureAnalysisPage.GridPanel.BuildDataTableFromGrid();
            tblCSV = SLVHelper.BuildDataTableFromLastDownloadedCSV(_failureExportedFilePattern);
            tblGridFormatted = tblGrid.Copy();
            tblCSVFormatted = tblCSV.Copy();

            FormatDataTablesBeforeVerifying(ref tblGridFormatted, ref tblCSVFormatted);
            VerifyEqual("[TS 4.5.1] 4. Verify exported CSV file reflects what is being shown in the grid", tblGridFormatted, tblCSVFormatted);

            Info("TS 4.7.1");
            Step("1.Repeat from step #2 (select a device from the grid) of TS 3.4.1 Data History - Last value panel");
            Step("2.Expected The same expectation with TS 3.4.1");
        }

        [Test, DynamicRetry]
        [Description("TS 4.5.1 Failure Analysis - Export detailed failure report")]
        public void TS4_05_01()
        {
            Assert.Pass("This test is included in TS 4.4.1. See TS 4.4.1 for details");
        }
        
        [Test, DynamicRetry]
        [Description("TS 4.7.1 Failure Analysis - Last value panel")]
        public void TS4_07_01()
        {
            Assert.Pass("This test is included in TS 4.4.1. See TS 4.4.1 for details");
        }

        [Test, DynamicRetry]
        [Description("TS 4.8.1 Failure Tracking - Go to app")]
        public void TS4_08_01()
        {
            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.FailureTracking);

            Step("1. Go to Failure Tracking app");
            Step("2. **Expected** Failure Tracking page is routed and loaded successfully");

            var startLogTime = DateTime.Now;
            var failureTrackingPage = desktopPage.GoToApp(App.FailureTracking) as FailureTrackingPage;

            Step("3. Log the time to load Failure Tracking page in running result");
            var endLogTime = DateTime.Now;

            Info(string.Format("Time to go to and load Failure Tracking is {0} seconds", (endLogTime - startLogTime).TotalSeconds));
        }

        [Test, DynamicRetry]
        [Description("TS 4.9.1 Failure Tracking - View failures from map")]
        public void TS4_09_01()
        {
            Assert.Pass("Covered by FT_02");
        }

        [Test, DynamicRetry]
        [Description("Failure Tracking - View failures from geozone")]
        public void TS4_10_01()
        {
            Assert.Pass("Covered by FT_01");
        }

        [Test, DynamicRetry]
        [Description("TS 4.11.1 Failure Tracking - Search with Unique address")]
        public void TS4_11_01()
        {
            var testData = GetTestDataOfTestTS4_11_01();

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.FailureTracking);

            Step("1. Go to Failure Tracking app");
            Step("2. Expected Failure Tracking page is routed and loaded successfully");
            var failureTrackingPage = desktopPage.GoToApp(App.FailureTracking) as FailureTrackingPage;

            Step("3. Select the root geozone");
            failureTrackingPage.GeozoneTreeMainPanel.SelectNode(Settings.RootGeozoneName);

            Step("4. Expand Search field below geozone tree and select \"Unique address\" attribute");
            var searchAttribute = testData["SearchAttribute"];
            var searchOperator = testData["SearchOperator"];
            var inexistingUniqueAddress = testData["InexistingUniqueAddress"];
            var existingUniqueAddress = testData["ExistingUniqueAddress"];

            failureTrackingPage.GeozoneTreeMainPanel.ClickExpandSearchButton();
            failureTrackingPage.GeozoneTreeMainPanel.SelectAttributeDropDown(searchAttribute);
            failureTrackingPage.GeozoneTreeMainPanel.SelectOperatorDropDown(searchOperator);

            Step("5. Enter an inexisting unique address into Search text field then click Search icon");
            failureTrackingPage.GeozoneTreeMainPanel.EnterSearchTextInput(inexistingUniqueAddress);
            failureTrackingPage.GeozoneTreeMainPanel.ClickSearchButton();
            failureTrackingPage.WaitForPreviousActionComplete();

            Step("6. Expected Search result widget shows \"No result\".There is not any device found");
            VerifyEqual("6. Verify no device found", true, failureTrackingPage.GeozoneTreeMainPanel.SearchResultsGeozonePanel.IsSearchResultMessageDisplayed());

            Step("7.Enter an existing unique address into Search text field then click Search icon");
            failureTrackingPage.GeozoneTreeMainPanel.EnterSearchTextInput(existingUniqueAddress);
            failureTrackingPage.GeozoneTreeMainPanel.ClickSearchButton();
            failureTrackingPage.WaitForPreviousActionComplete();

            Step("8. Expected Search results widget appears with only 1 device found");
            VerifyEqual("8. Verify 1 or many devices found", true, failureTrackingPage.GeozoneTreeMainPanel.SearchResultsGeozonePanel.IsSearchResultContentFoundDisplayed());

            Step("9. Select the found device");
            failureTrackingPage.GeozoneTreeMainPanel.SearchResultsGeozonePanel.SelectRandomFoundDevice().FirstOrDefault();
            failureTrackingPage.WaitForPreviousActionComplete();

            Step("10. Expected");
            Step(" • The selected dot is surrounded by a larger circle (Skipped for now, unable to check this on GL Map)");
            Step(" • Left widget appears with title \"Failure Tracking\" as the picture below. Verify device name, geozone path, address");
            VerifyEqual("10. Verify Failure Tracking Details panel is displayed", true, failureTrackingPage.FailureTrackingDetailsPanel.IsPanelVisible());
        }

        #region Comment out alarms because created all-in-one test case
        
        //[Test, DynamicRetry]
        //[Description("TS 4.13.1 Alarm Manager - Device alarm: multiple failures on multiple devices")]
        //[NonParallelizable]
        //public void TS4_13_01()
        //{
        //    var testData = GetTestDataOfTestTS4_13_01();

        //    /* Basic info */
        //    var alarmName = testData["Alarm.name"] as string;
        //    var alarmType = testData["Alarm.type"] as string;
        //    var alarmAction = testData["Alarm.action"] as string;

        //    alarmName = SLVHelper.GenerateUniqueName(string.Format("[{0}][{1}][{2}] {3}", Browser.Name, Settings.Users["DefaultTest"].Username, TestContext.CurrentContext.Test.MethodName, alarmName));

        //    /* General tab */
        //    var autoAckowledge = Convert.ToBoolean(testData["Alarm.General.auto-acknowledge"]);
        //    var refreshRate = testData["Alarm.General.refresh-rate"] as string;
        //    var refreshRateRegex = Regex.Match(refreshRate, @"(\d*) (.*)");
        //    var refreshRateUnit = refreshRateRegex.Groups[2].Value;

        //    /* Trigger tab */
        //    var message = testData["Alarm.Trigger.message"] as string;
        //    var failures = testData["Alarm.Trigger.failures"] as string;
        //    var devices = testData["Alarm.Trigger.devices"] as string;

        //    var deviceInfoRegex = Regex.Match(devices, @"(.*)#(.*)@(.*)");
        //    var deviceName = deviceInfoRegex.Groups[1].Value;
        //    var deviceId = deviceInfoRegex.Groups[2].Value;
        //    var controllerId = deviceInfoRegex.Groups[3].Value;
        //    var deviceNameWithControllerId = string.Format("{0} [@{1}]", deviceName, controllerId);

        //    var failureInfoRegex = Regex.Match(failures, @"(.*)#(.*)");
        //    var failureName = failureInfoRegex.Groups[1].Value;
        //    var failureId = failureInfoRegex.Groups[2].Value;

        //    /* Actions tab */
        //    var mailFrom = testData["Alarm.Actions.mail-from"] as string;
        //    var mailTo = testData["Alarm.Actions.mail-to"] as string;
        //    var mailSubject = alarmName;
        //    var mailContent = testData["Alarm.Actions.mail-content"] as string;

        //    var loginPage = Browser.OpenCMS();
        //    var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
        //    desktopPage.InstallAppsIfNotExist(App.AlarmManager, App.Alarms);

        //    /* Send failure request to get an email sent when alarm conditions is met */
        //    SetValueToDevice(controllerId, deviceId, failureId, true, Settings.GetServerTime().AddSeconds(1));

        //    Info("TS 4.13.1");
        //    Step("1. Go to Alarm Manager app");
        //    Step("2. Expected Alarm Manager page is routed and loaded successfully");
        //    var alarmManagerPage = desktopPage.GoToApp(App.AlarmManager) as AlarmManagerPage;

        //    Step("3. Select a geozone (should be configurable)");
        //    var geozone = testData["Geozone"] as string;
        //    alarmManagerPage.GeozoneTreeMainPanel.SelectNode(geozone);

        //    Step("4. Add an alarm");
        //    alarmManagerPage.GridPanel.ClickAddAlarmToolbarButton();
        //    alarmManagerPage.WaitForPreviousActionComplete();

        //    Step("5. Expected Add Alarm widget appears");
        //    VerifyEqual("5. Verify Alarm Details Panel appears", true, alarmManagerPage.AlarmEditorPanel.IsPanelVisible());

        //    Step("6. Specify report parameters");
        //    /* Basic info */
        //    alarmManagerPage.AlarmEditorPanel.EnterNameInput(alarmName);
        //    alarmManagerPage.AlarmEditorPanel.SelectTypeDropDown(alarmType);
        //    alarmManagerPage.WaitForPreviousActionComplete();
        //    alarmManagerPage.AlarmEditorPanel.SelectActionDropDown(alarmAction);

        //    /* General tab */
        //    alarmManagerPage.AlarmEditorPanel.SelectTab("General");
        //    alarmManagerPage.AlarmEditorPanel.TickAutoAcknowledgeCheckbox(autoAckowledge);
        //    alarmManagerPage.AlarmEditorPanel.SelectRefreshRateDropDown(refreshRate);

        //    /* Trigger tab */
        //    alarmManagerPage.AlarmEditorPanel.SelectTab("Trigger condition");
        //    alarmManagerPage.AlarmEditorPanel.EnterMessageInput(message);
        //    alarmManagerPage.AlarmEditorPanel.SelectFailuresListDropDown(failureName);
        //    alarmManagerPage.AlarmEditorPanel.SelectDevicesListDropDown(deviceNameWithControllerId);

        //    /* Actions tab */
        //    alarmManagerPage.AlarmEditorPanel.SelectTab("Actions");
        //    alarmManagerPage.AlarmEditorPanel.EnterFromInput(mailFrom);
        //    alarmManagerPage.AlarmEditorPanel.SelectToListDropDown(mailTo);
        //    alarmManagerPage.AlarmEditorPanel.EnterSubjectInput(mailSubject);
        //    alarmManagerPage.AlarmEditorPanel.EnterEmailMessageInput(mailContent);

        //    Step("7. Click Save");
        //    alarmManagerPage.AlarmEditorPanel.ClickSaveButton();
        //    alarmManagerPage.WaitForPreviousActionComplete();
        //    VerifyEqual(string.Format("7. Verify the newly created alarm is present in the grid ({0})", alarmName), true, alarmManagerPage.GridPanel.IsAlarmManagerGridHasTextPresent("Name", alarmName));
        //    var alarmCreated = Settings.GetServerTime().ToString("G");

        //    Step("8. After 30 seconds or a bit longer, check alarms Alarms page and the email's inbox");
        //    Step("9. Expected");
        //    Step(" • In Alarms page and in the mailbox, alarms with name specified at step #6 are present in the grid");
        //    // Verify email is sent and found
        //    var foundEmail = GetEmailForAlarm(alarmName);            
        //    var emailCheckingDateTime = Settings.GetServerTime().ToString("G");
        //    VerifyEqual(string.Format("9. Verify a sent alarm email is found in mailbox ({0}, Alarm created: {1}, Email checking: {2})", alarmName, alarmCreated, emailCheckingDateTime), true, foundEmail != null);
        //    EmailUtility.CleanInbox(alarmName);

        //    // Verify alarm in Alarms page
        //    desktopPage = Browser.RefreshLoggedInCMS();
        //    var alarmsPage = desktopPage.GoToApp(App.Alarms) as AlarmsPage;
        //    VerifyEqual(string.Format("9. Verify alarm is present in grid ({0})", alarmName), true, alarmsPage.GridPanel.IsAlarmGridHasTextPresent("Name", alarmName));

        //    Info("TS 4.14.1");
        //    Step("10. Send this request /api/loggingmanagement/setDeviceValues?controllerStrId={{controller id}}&idOnController={{device id}}&valueName={{failure property}}&value=false&doLog=true&eventTime={{current date time stamp}} to accept the failure as well as to auto-acknowledge the alarm");
        //    Step("11. Expected The request is sent successfully, the response returns ok");
        //    var isRequestOk = SetValueToDevice(controllerId, deviceId, failureId, false, Settings.GetServerTime().AddSeconds(1));
        //    VerifyEqual("11. Verify The request is sent successfully, the response returns ok", true, isRequestOk);
        //    Wait.ForAlarmTrigger();

        //    Step("12. Reload grid");
        //    alarmsPage.GridPanel.ClickReloadDataToolbarButton();
        //    alarmsPage.WaitForPreviousActionComplete();

        //    Step("13. Expected The alarm is not present in the grid any more because it was auto-acknowledged");
        //    alarmsPage.GridPanel.WaitForGridContentAvailable();
        //    var isAlarmExisting = alarmsPage.GridPanel.IsAlarmGridHasTextPresent("Name", alarmName);
        //    VerifyEqual(string.Format("13. Verify alarm '{0}' is not present in grid any longer because it was auto-acknowledged", alarmName), false, isAlarmExisting);

        //    try
        //    {
        //        if (!isAlarmExisting)
        //        {
        //            alarmManagerPage = alarmsPage.AppBar.SwitchTo(App.AlarmManager) as AlarmManagerPage;
        //            alarmManagerPage.DeleteAlarm(alarmName, true);
        //        }
        //    }
        //    catch { }
        //}

        //[Test, DynamicRetry]
        //[Description("TS 4.14.1 Alarm Manager - Device alarm - multiple failures on multiple devices - Auto acknowledged")]
        //public void TS4_14_01()
        //{
        //    Assert.Pass("This test is included in TS 4.13.1. See TS 4.13.1 for details");
        //}        

        //[Test, DynamicRetry]
        //[Description("TS 4.15.1 Alarm Manager - Device alarm: too many failures in an area - Auto acknowledged")]
        //public void TS4_15_01()
        //{
        //    var testData = GetTestDataOfTestTS4_15_01();

        //    /* Basic info */
        //    var alarmName = testData["Alarm.name"] as string;
        //    var alarmType = testData["Alarm.type"] as string;
        //    var alarmAction = testData["Alarm.action"] as string;

        //    alarmName = SLVHelper.GenerateUniqueName(string.Format("[{0}][{1}][{2}] {3}", Browser.Name, Settings.Users["DefaultTest"].Username, TestContext.CurrentContext.Test.MethodName, alarmName));

        //    /* General tab */
        //    var autoAckowledge = Convert.ToBoolean(testData["Alarm.General.auto-acknowledge"]);
        //    var refreshRate = testData["Alarm.General.refresh-rate"] as string;
        //    var refreshRateRegex = Regex.Match(refreshRate, @"(\d*) (.*)");
        //    var refreshRateUnit = refreshRateRegex.Groups[2].Value;

        //    /* Trigger tab */
        //    var message = testData["Alarm.Trigger.message"] as string;
        //    var failures = testData["Alarm.Trigger.failures"] as string;
        //    var radius = testData["Alarm.Trigger.radius"] as string;
        //    var threshold = testData["Alarm.Trigger.threshold"] as string;
        //    var devices = testData["Alarm.Trigger.devices"] as string;

        //    var deviceInfoRegex = Regex.Match(devices, @"(.*)#(.*)@(.*)");
        //    var deviceName = deviceInfoRegex.Groups[1].Value;
        //    var deviceId = deviceInfoRegex.Groups[2].Value;
        //    var controllerId = deviceInfoRegex.Groups[3].Value;
        //    var deviceNameWithControllerId = string.Format("{0} [@{1}]", deviceName, controllerId);

        //    var failureInfoRegex = Regex.Match(failures, @"(.*)#(.*)");
        //    var failureName = failureInfoRegex.Groups[1].Value;
        //    var failureId = failureInfoRegex.Groups[2].Value;

        //    /* Actions tab */
        //    var mailFrom = testData["Alarm.Actions.mail-from"] as string;
        //    var mailTo = testData["Alarm.Actions.mail-to"] as string;
        //    var mailSubject = alarmName;
        //    var mailContent = testData["Alarm.Actions.mail-content"] as string;

        //    var loginPage = Browser.OpenCMS();
        //    var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
        //    desktopPage.InstallAppsIfNotExist(App.AlarmManager, App.Alarms);

        //    /* Send failure request to get an email sent when alarm conditions is met */
        //    var thresholdAsNumber = Convert.ToInt32(threshold);
        //    for (var i = 0; i < (thresholdAsNumber <= 0 ? 1 : thresholdAsNumber); i++)
        //    {
        //        SetValueToDevice(controllerId, deviceId, failureId, true, Settings.GetServerTime().AddSeconds(1));
        //    }

        //    Step("1. Go to Alarm Manager app");
        //    Step("2. Expected Alarm Manager page is routed and loaded successfully");
        //    var alarmManagerPage = desktopPage.GoToApp(App.AlarmManager) as AlarmManagerPage;

        //    Step("3. Select a geozone (should be configurable)");
        //    var geozone = testData["Geozone"] as string;
        //    alarmManagerPage.GeozoneTreeMainPanel.SelectNode(geozone);

        //    Step("4. Add an alarm");
        //    alarmManagerPage.GridPanel.ClickAddAlarmToolbarButton();
        //    alarmManagerPage.WaitForPreviousActionComplete();

        //    Step("5. Expected Add Alarm widget appears");
        //    VerifyEqual("5. Verify Alarm Details Panel appears", true, alarmManagerPage.AlarmEditorPanel.IsPanelVisible());

        //    Step("6. Specify report parameters");
        //    /* Basic info */
        //    alarmManagerPage.AlarmEditorPanel.EnterNameInput(alarmName);
        //    alarmManagerPage.AlarmEditorPanel.SelectTypeDropDown(alarmType);
        //    alarmManagerPage.WaitForPreviousActionComplete();
        //    alarmManagerPage.AlarmEditorPanel.SelectActionDropDown(alarmAction);

        //    /* General tab */
        //    // General tab is active by default
        //    alarmManagerPage.AlarmEditorPanel.SelectTab("General");
        //    alarmManagerPage.AlarmEditorPanel.TickAutoAcknowledgeCheckbox(autoAckowledge);
        //    alarmManagerPage.AlarmEditorPanel.SelectRefreshRateDropDown(refreshRate);

        //    /* Trigger tab */
        //    alarmManagerPage.AlarmEditorPanel.SelectTab("Trigger condition");
        //    alarmManagerPage.AlarmEditorPanel.EnterMessageInput(message);
        //    alarmManagerPage.AlarmEditorPanel.SelectFailuresListDropDown(failureName);
        //    alarmManagerPage.AlarmEditorPanel.EnterRadiusNumericInput(radius);
        //    alarmManagerPage.AlarmEditorPanel.EnterThresholdNumericInput(threshold);

        //    /* Actions tab */
        //    alarmManagerPage.AlarmEditorPanel.SelectTab("Actions");
        //    alarmManagerPage.AlarmEditorPanel.EnterFromInput(mailFrom);
        //    alarmManagerPage.AlarmEditorPanel.SelectToListDropDown(mailTo);
        //    alarmManagerPage.AlarmEditorPanel.EnterSubjectInput(mailSubject);
        //    alarmManagerPage.AlarmEditorPanel.EnterEmailMessageInput(mailContent);

        //    Step("7. Click Save");
        //    alarmManagerPage.AlarmEditorPanel.ClickSaveButton();
        //    alarmManagerPage.WaitForPreviousActionComplete();
        //    VerifyEqual(string.Format("7. Verify the newly created alarm is present in the grid ({0})", alarmName), true, alarmManagerPage.GridPanel.IsAlarmManagerGridHasTextPresent("Name", alarmName));
        //    var alarmCreated = Settings.GetServerTime().ToString("G");

        //    Step("8. After 30 seconds or a bit longer, check alarms Alarms page and the email's inbox");
        //    Step("9. Expected");
        //    Step(" • In Alarms page and in the mailbox, alarms with name specified at step #6 are present in the grid");
        //    // Verify email is sent and found
        //    var foundEmail = GetEmailForAlarm(alarmName);
        //    var emailCheckingDateTime = Settings.GetServerTime().ToString("G");
        //    VerifyEqual(string.Format("9. Verify a sent alarm email is found in mailbox ({0}, Alarm created: {1}, Email checking: {2})", alarmName, alarmCreated, emailCheckingDateTime), true, foundEmail != null);
        //    EmailUtility.CleanInbox(alarmName);

        //    // Verify alarm in Alarms page
        //    desktopPage = Browser.RefreshLoggedInCMS();
        //    var alarmsPage = desktopPage.GoToApp(App.Alarms) as AlarmsPage;
        //    VerifyEqual(string.Format("9. Verify alarm is present in grid ({0})", alarmName), true, alarmsPage.GridPanel.IsAlarmGridHasTextPresent("Name", alarmName));

        //    Step("10. Send this request /api/loggingmanagement/setDeviceValues?controllerStrId={{controller id}}&idOnController={{device id}}&valueName={{failure property}}&value=false&doLog=true&eventTime={{current date time stamp}} to accept the failure as well as to auto-acknowledge the alarm");
        //    Step("11. Expected The request is sent successfully, the response returns ok");           
        //    var isRequestOk = SetValueToDevice(controllerId, deviceId, failureId, false, Settings.GetServerTime().AddSeconds(1));
        //    VerifyEqual("11. Verify The request is sent successfully, the response returns ok", true, isRequestOk);            
        //    Wait.ForAlarmTrigger();

        //    Step("12. Reload grid");
        //    alarmsPage.GridPanel.ClickReloadDataToolbarButton();
        //    alarmsPage.WaitForPreviousActionComplete();

        //    Step("13. Expected The alarm is not present in the grid any more because it was auto-acknowledged");
        //    alarmsPage.GridPanel.WaitForGridContentAvailable();
        //    var isAlarmExisting = alarmsPage.GridPanel.IsAlarmGridHasTextPresent("Name", alarmName);
        //    VerifyEqual(string.Format("13. Verify alarm '{0}' is not present in grid any longer because it was auto-acknowledged", alarmName), false, isAlarmExisting);

        //    try
        //    {
        //        if (!isAlarmExisting)
        //        {
        //            alarmManagerPage = alarmsPage.AppBar.SwitchTo(App.AlarmManager) as AlarmManagerPage;
        //            alarmManagerPage.DeleteAlarm(alarmName, true);
        //        }
        //    }
        //    catch { }
        //}

        //[Test, DynamicRetry]
        //[Description("TS 4.16.1 Device alarm: failure ration in a group - Auto acknowledged")]
        //public void TS4_16_01()
        //{
        //    var testData = GetTestDataOfTestTS4_16_01();

        //    /* Basic info */
        //    var alarmName = testData["Alarm.name"] as string;
        //    var alarmType = testData["Alarm.type"] as string;
        //    var alarmAction = testData["Alarm.action"] as string;

        //    alarmName = SLVHelper.GenerateUniqueName(string.Format("[{0}][{1}][{2}] {3}", Browser.Name, Settings.Users["DefaultTest"].Username, TestContext.CurrentContext.Test.MethodName, alarmName));

        //    /* General tab */
        //    var autoAckowledge = Convert.ToBoolean(testData["Alarm.General.auto-acknowledge"]);
        //    var refreshRate = testData["Alarm.General.refresh-rate"] as string;
        //    var refreshRateRegex = Regex.Match(refreshRate, @"(\d*) (.*)");
        //    var refreshRateUnit = refreshRateRegex.Groups[2].Value;

        //    /* Trigger tab */
        //    var message = testData["Alarm.Trigger.message"] as string;
        //    var failures = testData["Alarm.Trigger.failures"] as string;
        //    var devices = testData["Alarm.Trigger.devices"] as string;
        //    var criticalFailureRatio = testData["Alarm.Trigger.critical-failure-ratio"] as string;

        //    var deviceInfoRegex = Regex.Match(devices, @"(.*)#(.*)@(.*)");
        //    var deviceName = deviceInfoRegex.Groups[1].Value;
        //    var deviceId = deviceInfoRegex.Groups[2].Value;
        //    var controllerId = deviceInfoRegex.Groups[3].Value;
        //    var deviceNameWithControllerId = string.Format("{0} [@{1}]", deviceName, controllerId);

        //    var failureInfoRegex = Regex.Match(failures, @"(.*)#(.*)");
        //    var failureName = failureInfoRegex.Groups[1].Value;
        //    var failureId = failureInfoRegex.Groups[2].Value;

        //    /* Actions tab */
        //    var mailFrom = testData["Alarm.Actions.mail-from"] as string;
        //    var mailTo = testData["Alarm.Actions.mail-to"] as string;
        //    var mailSubject = alarmName;
        //    var mailContent = testData["Alarm.Actions.mail-content"] as string;

        //    var loginPage = Browser.OpenCMS();
        //    var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
        //    desktopPage.InstallAppsIfNotExist(App.AlarmManager, App.Alarms);

        //    /* Send failure request to get an email sent when alarm conditions is met */
        //    SetValueToDevice(controllerId, deviceId, failureId, true, Settings.GetServerTime().AddSeconds(1));

        //    Step("1. Go to Alarm Manager app");
        //    Step("2. Expected Alarm Manager page is routed and loaded successfully");
        //    var alarmManagerPage = desktopPage.GoToApp(App.AlarmManager) as AlarmManagerPage;

        //    Step("3. Select a geozone (should be configurable)");
        //    var geozone = testData["Geozone"] as string;
        //    alarmManagerPage.GeozoneTreeMainPanel.SelectNode(geozone);

        //    Step("4. Add an alarm");
        //    alarmManagerPage.GridPanel.ClickAddAlarmToolbarButton();
        //    alarmManagerPage.WaitForPreviousActionComplete();

        //    Step("5. Expected Add Alarm widget appears");
        //    VerifyEqual("5. Verify Alarm Details Panel appears", true, alarmManagerPage.AlarmEditorPanel.IsPanelVisible());

        //    Step("6. Specify report parameters");
        //    /* Basic info */
        //    alarmManagerPage.AlarmEditorPanel.EnterNameInput(alarmName);
        //    alarmManagerPage.AlarmEditorPanel.SelectTypeDropDown(alarmType);
        //    alarmManagerPage.WaitForPreviousActionComplete();
        //    alarmManagerPage.AlarmEditorPanel.SelectActionDropDown(alarmAction);

        //    /* General tab */
        //    // General tab is active by default
        //    alarmManagerPage.AlarmEditorPanel.SelectTab("General");
        //    alarmManagerPage.AlarmEditorPanel.TickAutoAcknowledgeCheckbox(autoAckowledge);
        //    alarmManagerPage.AlarmEditorPanel.SelectRefreshRateDropDown(refreshRate);

        //    /* Trigger tab */
        //    alarmManagerPage.AlarmEditorPanel.SelectTab("Trigger condition");
        //    alarmManagerPage.AlarmEditorPanel.EnterMessageInput(message);
        //    alarmManagerPage.AlarmEditorPanel.EnterCriticalFailureRatioNumericInput(criticalFailureRatio);
        //    alarmManagerPage.AlarmEditorPanel.SelectFailuresListDropDown(failureName);

        //    /* Actions tab */
        //    alarmManagerPage.AlarmEditorPanel.SelectTab("Actions");
        //    alarmManagerPage.AlarmEditorPanel.EnterFromInput(mailFrom);
        //    alarmManagerPage.AlarmEditorPanel.SelectToListDropDown(mailTo);
        //    alarmManagerPage.AlarmEditorPanel.EnterSubjectInput(mailSubject);
        //    alarmManagerPage.AlarmEditorPanel.EnterEmailMessageInput(mailContent);

        //    Step("7. Click Save");
        //    alarmManagerPage.AlarmEditorPanel.ClickSaveButton();
        //    alarmManagerPage.WaitForPreviousActionComplete();
        //    VerifyEqual(string.Format("7. Verify the newly created alarm is present in the grid ({0})", alarmName), true, alarmManagerPage.GridPanel.IsAlarmManagerGridHasTextPresent("Name", alarmName));
        //    var alarmCreated = Settings.GetServerTime().ToString("G"); 

        //    Step("8. After 30 seconds or a bit longer, check alarms Alarms page and the email's inbox");
        //    Step("9. Expected");
        //    Step(" • In Alarms page and in the mailbox, alarms with name specified at step #6 are present in the grid");
        //    // Verify email is sent and found
        //    var foundEmail = GetEmailForAlarm(alarmName);
        //    var emailCheckingDateTime = Settings.GetServerTime().ToString("G");
        //    VerifyEqual(string.Format("[SC-749] 9. Verify a sent alarm email is found in mailbox ({0}, Alarm created: {1}, Email checking: {2})", alarmName, alarmCreated, emailCheckingDateTime), true, foundEmail != null);
        //    EmailUtility.CleanInbox(alarmName);

        //    // Verify alarm in Alarms page
        //    desktopPage = Browser.RefreshLoggedInCMS();
        //    var alarmsPage = desktopPage.GoToApp(App.Alarms) as AlarmsPage;
        //    VerifyEqual(string.Format("9. Verify alarm is present in grid ({0})", alarmName), true, alarmsPage.GridPanel.IsAlarmGridHasTextPresent("Name", alarmName));

        //    Step("10. Send this request /api/loggingmanagement/setDeviceValues?controllerStrId={{controller id}}&idOnController={{device id}}&valueName={{failure property}}&value=false&doLog=true&eventTime={{current date time stamp}} to accept the failure as well as to auto-acknowledge the alarm");
        //    Step("11. Expected The request is sent successfully, the response returns ok");            
        //    var isRequestOk = SetValueToDevice(controllerId, deviceId, failureId, false, Settings.GetServerTime().AddSeconds(1));
        //    VerifyEqual("11. Verify The request is sent successfully, the response returns ok", true, isRequestOk);
        //    Wait.ForAlarmTrigger();

        //    Step("12. Reload grid");
        //    alarmsPage.GridPanel.ClickReloadDataToolbarButton();
        //    alarmsPage.WaitForPreviousActionComplete();

        //    Step("13. Expected The alarm is not present in the grid any more because it was auto-acknowledged");
        //    alarmsPage.GridPanel.WaitForGridContentAvailable();
        //    var isAlarmExisting = alarmsPage.GridPanel.IsAlarmGridHasTextPresent("Name", alarmName);
        //    VerifyEqual(string.Format("13. Verify alarm '{0}' is not present in grid any longer because it was auto-acknowledged", alarmName), false, isAlarmExisting);
        //    try
        //    {
        //        if (!isAlarmExisting)
        //        {
        //            alarmManagerPage = alarmsPage.AppBar.SwitchTo(App.AlarmManager) as AlarmManagerPage;
        //            alarmManagerPage.DeleteAlarm(alarmName, true);
        //        }
        //    }
        //    catch { }    
        //}

        //[Test, DynamicRetry]
        //[Description("TS 4.17.1 Alarm Manager - Device alarm: data analysis versus previous day - Analytic mode - Average")]
        //public void TS4_17_01()
        //{
        //    var testData = GetTestDataOfTestTS4_17_01();
        //    var geozone = SLVHelper.GenerateUniqueName("GZNTS41701");
        //    var controller = SLVHelper.GenerateUniqueName("CTRL");
        //    var device = SLVHelper.GenerateUniqueName("STL");

        //    //Basic info
        //    var alarmType = testData["Alarm.type"].ToString();
        //    var alarmAction = testData["Alarm.action"].ToString();
        //    var alarmName = SLVHelper.GenerateUniqueName(string.Format("[{0}][{1}][{2}] {3}", Browser.Name, Settings.Users["DefaultTest"].Username, TestContext.CurrentContext.Test.MethodName, alarmType));
            
        //    // General
        //    var autoAckowledge = Convert.ToBoolean(testData["Alarm.General.auto-acknowledge"]);
        //    var refreshRate = testData["Alarm.General.refresh-rate"].ToString();
        //    var refreshRateRegex = Regex.Match(refreshRate, @"(\d*) (.*)");
        //    var refreshRateUnit = refreshRateRegex.Groups[2].Value;

        //    //Trigger
        //    var message = testData["Alarm.Trigger.message"].ToString();
        //    var metering = testData["Alarm.Trigger.metering"].ToString();
        //    var ignoreOperator = testData["Alarm.Trigger.ignore-operator"].ToString();
        //    var ignoreValue = testData["Alarm.Trigger.ignore-value"].ToString();
        //    var analysisPeriod = testData["Alarm.Trigger.analysis-period"].ToString();
        //    var analyticMode = testData["Alarm.Trigger.analytic-mode"].ToString();
        //    var percentageDifferenceTrigger = testData["Alarm.Trigger.percentage-difference-trigger"].ToString();    

        //    var meteringInfoRegex = Regex.Match(metering, @"(.*)#(.*)");
        //    var meteringName = meteringInfoRegex.Groups[1].Value;
        //    var meteringId = meteringInfoRegex.Groups[2].Value;

        //    //Actions
        //    var mailFrom = testData["Alarm.Actions.mail-from"].ToString();
        //    var mailTo = testData["Alarm.Actions.mail-to"].ToString();
        //    var mailSubject = alarmName;
        //    var mailContent = SLVHelper.GenerateUniqueName("Content of TS41701");           
            
        //    Step("**** Precondition ****");
        //    Step(" - User has logged in successfully");
        //    Step(" - Create a geozone containing a controller using UTC time and a streetlight");
        //    Step(" - Simulate the previous day data by sending 2 commands for the property Mains Current with");
        //    Step("  + valueName: Current");
        //    Step("  + Value: 50");
        //    Step("  + For 1st Command:");
        //    Step("    * evenTime: {Previous day} {current UTC time - 10 minute}");
        //    Step("  + For 2nd Command:");
        //    Step("    * evenTime: {Previous day} {current UTC time} Ex:");
        //    Step("    * Current UTC time: 06:50:00 > {current UTC time - 10 minute} = 06:40:00");
        //    Step("    * Today: 2018-05-08 > Previous day: 2018-05-07");
        //    Step("**** Precondition ****\n");

        //    Step("-> Create data for testing");
        //    DeleteGeozones("GZNTS41701*");
        //    CreateNewGeozone(geozone);
        //    CreateNewController(controller, geozone);
        //    SetValueToController(controller, "TimeZoneId", "UTC", Settings.GetServerTime());
        //    CreateNewDevice(DeviceType.Streetlight, device, controller, geozone);
        //    var curCtrlDateTime = Settings.GetCurrentControlerDateTime(controller);
        //    var previousBefore10m = curCtrlDateTime.AddDays(-1).AddMinutes(-10);
        //    var previousDay = curCtrlDateTime.AddDays(-1);
        //    var value = "50";
        //    var request = SetValueToDevice(controller, device, meteringId, value, previousBefore10m);
        //    VerifyEqual(string.Format("-> Verify the request is sent successfully (attribute: {0}, value: {1}, time: {2})", meteringName, value, previousBefore10m), true, request);
        //    request = SetValueToDevice(controller, device, meteringId, value, previousDay);
        //    VerifyEqual(string.Format("-> Verify the request is sent successfully (attribute: {0}, value: {1}, time: {2})", meteringName, value, previousDay), true, request);

        //    var loginPage = Browser.OpenCMS();
        //    var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
        //    desktopPage.InstallAppsIfNotExist(App.AlarmManager, App.Alarms);
           
        //    Step("1. Go to Alarm Manager app");
        //    Step("2. Expected Alarm Manager page is routed and loaded successfully");
        //    var alarmManagerPage = desktopPage.GoToApp(App.AlarmManager) as AlarmManagerPage;

        //    Step("3. Select a geozone");          
        //    alarmManagerPage.GeozoneTreeMainPanel.SelectNode(geozone);

        //    Step("4. Add an alarm");
        //    alarmManagerPage.GridPanel.ClickAddAlarmToolbarButton();
        //    alarmManagerPage.WaitForPreviousActionComplete();

        //    Step("5. Verify Add Alarm widget appears");
        //    VerifyEqual("5. Verify Alarm Details Panel appears", true, alarmManagerPage.AlarmEditorPanel.IsPanelVisible());

        //    Step("6. Specify report parameters");
        //    Step(" - Name: Any {{date time span}}");
        //    Step(" - Type: Device alarm: data analysis versus previous day");
        //    Step(" - Action: Notify by eMail");
        //    Step(" - General tab:");
        //    Step("  + Auto-acknowledge: checked");
        //    Step("  + Refresh rate: 30 seconds");
        //    Step(" - Trigger condition tab:");
        //    Step("  + Message: Any");
        //    Step("  + Devices: selected from the testing streetlight");
        //    Step("  + Metering: Mains current");
        //    Step("  + Ignore operator: Lower");
        //    Step("  + Ignore value: 30");
        //    Step("  + Analysis period: 30 minutes");
        //    Step("  + Analytic mode: Average");
        //    Step("  + Percentage difference trigger: 10");
        //    Step(" - Actions tab:");
        //    Step("  + From: qa@streetlightmonitoring.com");
        //    Step("  + To: Any valid mailbox");
        //    Step("  + Subject: Any");
        //    Step("  + Message: Any");
        //    //Basic info
        //    alarmManagerPage.AlarmEditorPanel.EnterNameInput(alarmName);
        //    alarmManagerPage.AlarmEditorPanel.SelectTypeDropDown(alarmType);
        //    alarmManagerPage.WaitForPreviousActionComplete();
        //    alarmManagerPage.AlarmEditorPanel.SelectActionDropDown(alarmAction);

        //    //General tab
        //    //General tab is active by default
        //    alarmManagerPage.AlarmEditorPanel.SelectTab("General");
        //    alarmManagerPage.AlarmEditorPanel.TickAutoAcknowledgeCheckbox(autoAckowledge);
        //    alarmManagerPage.AlarmEditorPanel.SelectRefreshRateDropDown(refreshRate);

        //    //Trigger tab
        //    alarmManagerPage.AlarmEditorPanel.SelectTab("Trigger condition");
        //    alarmManagerPage.AlarmEditorPanel.EnterMessageInput(message);
        //    alarmManagerPage.AlarmEditorPanel.SelectDevicesListDropDown(device);
        //    alarmManagerPage.AlarmEditorPanel.SelectMeteringDropDown(meteringName);
        //    alarmManagerPage.AlarmEditorPanel.SelectIgnoreOperatorDropDown(ignoreOperator);
        //    alarmManagerPage.AlarmEditorPanel.EnterIgnoreValueNumericInput(ignoreValue);
        //    alarmManagerPage.AlarmEditorPanel.SelectAnalysisPeriodDropDown(analysisPeriod);
        //    alarmManagerPage.AlarmEditorPanel.SelectAnalyticModeDropDown(analyticMode);
        //    alarmManagerPage.AlarmEditorPanel.EnterPercentageDifferenceTriggerNumericInput(percentageDifferenceTrigger);

        //    //Actions tab
        //    alarmManagerPage.AlarmEditorPanel.SelectTab("Actions");
        //    alarmManagerPage.AlarmEditorPanel.EnterFromInput(mailFrom);
        //    alarmManagerPage.AlarmEditorPanel.SelectToListDropDown(mailTo);
        //    alarmManagerPage.AlarmEditorPanel.EnterSubjectInput(mailSubject);
        //    alarmManagerPage.AlarmEditorPanel.EnterEmailMessageInput(mailContent);

        //    Step("7. Click Save");
        //    alarmManagerPage.AlarmEditorPanel.ClickSaveButton();
        //    alarmManagerPage.WaitForPreviousActionComplete();
        //    var alarmCreated = Settings.GetServerTime().ToString("G");

        //    Step("8. Verify The new alarm is added into the grid of Alarm Manager");
        //    VerifyEqual(string.Format("8. Verify The new alarm is added into the grid of Alarm Manager ({0})", alarmName), true, alarmManagerPage.GridPanel.IsAlarmManagerGridHasTextPresent("Name", alarmName));
                       
        //    Step("9. Simulate the today data by sending 2 commands for the property Mains Current with");
        //    Step(" - valueName: Current");
        //    Step(" - Value: 60");
        //    Step(" - For 1st Command:");
        //    Step("  + evenTime: {current UTC datetime - 10 minute}");
        //    Step(" - For 2nd Command:");
        //    Step("  + evenTime: {current UTC datetime}");
        //    curCtrlDateTime = Settings.GetCurrentControlerDateTime(controller);
        //    var currentDateTimeBefore10m = curCtrlDateTime.AddMinutes(-10);
        //    value = "60";
        //    request = SetValueToDevice(controller, device, meteringId, value, currentDateTimeBefore10m);
        //    VerifyEqual(string.Format("-> Verify the request is sent successfully (attribute: {0}, value: {1}, time: {2})", meteringName, value, currentDateTimeBefore10m), true, request);            
        //    request = SetValueToDevice(controller, device, meteringId, value, curCtrlDateTime);
        //    VerifyEqual(string.Format("-> Verify the request is sent successfully (attribute: {0}, value: {1}, time: {2})", meteringName, value, curCtrlDateTime), true, request);
            
        //    Step("10. Wait for 30s or a bit longer, then go to the testing geozone in Alarm app and press Refresh button");
        //    Wait.ForAlarmTrigger();
        //    var alarmsPage = alarmManagerPage.AppBar.SwitchTo(App.Alarms) as AlarmsPage;
        //    alarmsPage.GridPanel.ClickReloadDataToolbarButton();
        //    alarmsPage.WaitForPreviousActionComplete();

        //    Step("11. Verify In Alarms page, an alarm is present in the grid with");
        //    Step(" - Name: Alarm name from creating the alarm in Alarm Manager app");
        //    Step(" - Geozone: testing geozone");
        //    Step(" - Priority: 0");
        //    Step(" - Generator: Alarm name '-' random numbers");
        //    Step(" - Creation Date: the time the alarm is triggered with format {mm/dd/yyyy hh:MM:ss AM/PM}, usually about 2 minutes after sending command.");
        //    Step(" - State: X red icon (active status)");
        //    Step(" - Last Change: equal to Creation Date");
        //    Step(" - User: -");
        //    Step(" - Info: message in 'Trigger condition' tab of Alarm Manager app when alarm is created");
        //    Step(" - Trigger Time: empty");
        //    Step(" - Trigger Info: empty");
        //    var dtGridView = alarmsPage.GridPanel.BuildAlarmDataTable();
        //    var row = dtGridView.Select(string.Format("Name = '{0}'", alarmName)).FirstOrDefault();
        //    if (row != null)
        //    {
        //        var creationDate = DateTime.Parse(row["Creation Date"].ToString());
        //        var timeSpan = curCtrlDateTime.Subtract(creationDate);
        //        var iconList = alarmsPage.GridPanel.GetListOfAlarmGridIconColumn(alarmName);
        //        VerifyEqual("11. Verify Geozone is " + geozone, geozone, row["Geozone"].ToString());
        //        VerifyEqual("11. Verify Priority: 0", "0", row["Priority"].ToString());
        //        VerifyTrue("11. Verify Generator: Alarm name '-' random numbers", row["Generator"].ToString().Contains(alarmName), alarmName, row["Generator"].ToString());
        //        VerifyTrue("11. Verify Creation Date: the time the alarm is triggered with format {M/d/yyyy hh:mm:ss AM/PM}, usually about 2 minutes after sending command.", Math.Abs(timeSpan.TotalMinutes) <= 2, curCtrlDateTime, creationDate);
        //        VerifyEqual("11. Verify State: X red icon (active status)", true, iconList.Count == 1 && iconList.Any(p => p.Contains("status-error.png")));
        //        VerifyEqual("11. Verify Last Change: equal to Creation Date", row["Creation Date"].ToString(), row["Last Change"].ToString());
        //        VerifyEqual("11. Verify User: -", "-", row["User"].ToString());
        //        VerifyEqual("11. Verify Info: message in 'Trigger condition' tab of Alarm Manager app when alarm is created", message, row["Info"].ToString());
        //        VerifyEqual("11. Verify Trigger Time: empty", "", row["Trigger Time"].ToString());
        //        VerifyEqual("11. Verify Trigger Info: empty", "", row["Trigger Info"].ToString());
        //    }
        //    else
        //    {
        //        Warning(string.Format("11. There is no row with alarm '{0}'", alarmName));
        //    }

        //    Step("12. Check the email");   
        //    Step("13. Verify an email is sent with");
        //    Step(" - Subject: subject set up in Actions tab of Alarm Manager");
        //    Step(" - Contains: message set up in Actions tab of Alarm Manager Note: No email send. Should ask JM to create a new ticket for this");
        //    var foundEmail = EmailUtility.GetNewEmail(alarmName);
        //    var isEmailSent = foundEmail != null;
        //    var emailCheckingDateTime = Settings.GetServerTime().ToString("G");
        //    VerifyEqual(string.Format("13. Verify a sent alarm email is found in mailbox ({0}, Alarm created: {1}, Email checking: {2})", alarmName, alarmCreated, emailCheckingDateTime), true, isEmailSent);
        //    if (isEmailSent)
        //    {
        //        VerifyEqual("13. Verify Subject: subject set up in Actions tab of Alarm Manager", mailSubject, foundEmail.Subject);
        //        VerifyEqual("13. Verify Contains: message set up in Actions tab of Alarm Manager", true, foundEmail.Body.IndexOf(mailContent) >= 0);
        //        EmailUtility.CleanInbox(alarmName);
        //    }

        //    Step("14. Simulate the today data by sending 1 command for the property Mains Current with");
        //    Step(" - valueName: Current");
        //    Step(" - Value: 40");
        //    Step(" - evenTime: {current UTC datetime}");
        //    curCtrlDateTime = Settings.GetCurrentControlerDateTime(controller);
        //    value = "40";
        //    request = SetValueToDevice(controller, device, meteringId, value, curCtrlDateTime);
        //    VerifyEqual(string.Format("-> Verify the request is sent successfully (attribute: {0}, value: {1}, time: {2})", meteringName, value, curCtrlDateTime), true, request);
            
        //    Step("15. Wait for 30s or a bit longer, then go to the testing geozone in Alarm app and press Refresh button");
        //    Wait.ForAlarmTrigger();
        //    alarmsPage.GridPanel.ClickReloadDataToolbarButton();
        //    alarmsPage.WaitForPreviousActionComplete();

        //    Step("16. Verify The alarm is not present in the grid any more because it was auto-acknowledged");
        //    VerifyEqual("16. Verify The alarm is not present in the grid any more because it was auto-acknowledged", true, !alarmsPage.GridPanel.IsAlarmGridHasTextPresent("Name", alarmName));

        //    Step("17. Go to Alarm Manager app and edit the testing alarm. Check 'New alarm if re-triggerd' and save changes");
        //    alarmManagerPage = alarmsPage.AppBar.SwitchTo(App.AlarmManager) as AlarmManagerPage;
        //    alarmManagerPage.GridPanel.ClickGridRecord(alarmName);
        //    alarmManagerPage.WaitForPreviousActionComplete();
        //    alarmManagerPage.AlarmEditorPanel.SelectTab("General");
        //    alarmManagerPage.AlarmEditorPanel.TickNewAlarmIfRetriggerCheckbox(true);
        //    alarmManagerPage.AlarmEditorPanel.ClickSaveButton();
        //    alarmManagerPage.WaitForPreviousActionComplete();
        //    var alarmUpdated = Settings.GetServerTime().ToString("G");

        //    Step("18. Simulate the today data by sending 1 command for the property Mains Current with");
        //    Step(" - valueName: Current");
        //    Step(" - Value: 70");
        //    Step(" - evenTime: {current UTC datetime}");
        //    curCtrlDateTime = Settings.GetCurrentControlerDateTime(controller);
        //    value = "70";
        //    request = SetValueToDevice(controller, device, meteringId, value, curCtrlDateTime);
        //    VerifyEqual(string.Format("-> Verify the request is sent successfully (attribute: {0}, value: {1}, time: {2})", meteringName, value, curCtrlDateTime), true, request);

        //    Step("19. Wait for 30s or a bit longer, then go to the testing geozone in Alarm app and press Refresh button");
        //    Wait.ForAlarmTrigger();
        //    alarmsPage = alarmManagerPage.AppBar.SwitchTo(App.Alarms) as AlarmsPage;
        //    alarmsPage.GridPanel.ClickReloadDataToolbarButton();
        //    alarmsPage.WaitForPreviousActionComplete();            

        //    Step("20. Verify");
        //    Step(" - A new email sent");
        //    Step(" - An new alarm added to Alarm panel in Alarm app");
        //    VerifyEqual("20. Verify An new alarm added to Alarm panel in Alarm app", true, alarmsPage.GridPanel.IsAlarmGridHasTextPresent("Name", alarmName));
        //    foundEmail = EmailUtility.GetNewEmail(alarmName);
        //    isEmailSent = foundEmail != null;
        //    emailCheckingDateTime = Settings.GetServerTime().ToString("G");
        //    VerifyEqual(string.Format("20. Verify A new email sent in mailbox ({0}, Alarm created: {1}, Email checking: {2})", alarmName, alarmUpdated, emailCheckingDateTime), true, isEmailSent);
        //    if (isEmailSent) EmailUtility.CleanInbox(alarmName);

        //    Step("21. Press Red Bell icon");
        //    alarmsPage.GridPanel.ClickShowAllAlarmsToolbarOption();
        //    alarmsPage.WaitForPreviousActionComplete();

        //    Step("22. Verify 2 rows of the alarm in the Alarm panel");
        //    Step(" - 1st row with status: Active (Red X icon)");
        //    Step(" - 2nd row with status: Acknowledged (Green Check icon)");
        //    var iconList1 = alarmsPage.GridPanel.GetListOfAlarmGridIconColumn(alarmName);
        //    VerifyEqual("22. Verify 2 rows of the alarm in the Alarm panel", 2, iconList1.Count);
        //    VerifyEqual("22. Verify 1st row with status: Active (Red X icon)", true, iconList1.Any(p => p.Contains("status-error.png")));
        //    VerifyEqual("22. Verify 2nd row with status: Acknowledged (Green Check icon)", true, iconList1.Any(p => p.Contains("status-ok.png")));

        //    Step("23. Select the row with status Active (Red X icon) and press Acknowledge button and enter the message to Acknowledge Alarms pop-up then press Send button and press OK");
        //    alarmsPage.GridPanel.ClickAlarmGridRecordHasErrorIcon(alarmName);
        //    alarmsPage.GridPanel.ClickAcknowledgeToolbarButton();
        //    alarmsPage.WaitForPopupDialogDisplayed();
        //    alarmsPage.Dialog.ClickSendButton();
        //    alarmsPage.Dialog.WaitForPopupMessageDisplayed();
        //    alarmsPage.Dialog.ClickOkButton();
        //    alarmsPage.WaitForPopupDialogDisappeared();
        //    var acknowledgeTime = Settings.GetServerTime();

        //    Step("24. Verify The Status is changed to Acknowledged (Green Check icon)");
        //    iconList1 = alarmsPage.GridPanel.GetListOfAlarmGridIconColumn(alarmName);
        //    VerifyEqual("24. Verify The Status is changed to Acknowledged (Green Check icon)", true, iconList1.All(p => p.Contains("status-ok.png")));

        //    Step("25. Press Refresh button");
        //    alarmsPage.GridPanel.ClickReloadDataToolbarButton();
        //    alarmsPage.WaitForPreviousActionComplete();

        //    Step("26. Verify The row is updated with");
        //    Step(" - Last Change: the datetime when alarm is acknowledged");
        //    Step(" - User: the user who acknowledged the alarm");
        //    Step(" - Trigger Time: Creation Date");
        //    Step(" - Trigger Info: Alarm message");
        //    dtGridView = alarmsPage.GridPanel.BuildAlarmDataTable();
        //    row = dtGridView.Select(string.Format("Name = '{0}'", alarmName)).FirstOrDefault();
        //    if (row != null)
        //    {
        //        var lastChangeDate = DateTime.Parse(row["Last Change"].ToString());
        //        var timeSpan = acknowledgeTime.Subtract(lastChangeDate);

        //        var iconList = alarmsPage.GridPanel.GetListOfAlarmGridIconColumn(alarmName);                
        //        VerifyTrue("26. Verify Last Change: the datetime when alarm is acknowledged", Math.Abs(timeSpan.TotalMinutes) <= 2, acknowledgeTime, lastChangeDate);
        //        VerifyEqual("26. Verify User: the user who acknowledged the alarm", Settings.Users["DefaultTest"].Username, row["User"].ToString());                
        //        VerifyEqual("26. Verify Trigger Time: Creation Date", row["Creation Date"].ToString(), row["Trigger Time"].ToString());
        //        VerifyEqual("26. Verify Trigger Info: Alarm message", message, row["Trigger Info"].ToString());
        //    }
        //    else
        //    {
        //        Warning(string.Format("26. There is no row with alarm '{0}'", alarmName));
        //    }

        //    try
        //    {
        //        alarmManagerPage = alarmsPage.AppBar.SwitchTo(App.AlarmManager) as AlarmManagerPage;
        //        alarmManagerPage.DeleteAlarm(alarmName);
        //        DeleteGeozone(geozone);
        //    }
        //    catch { }
        //}

        //[Test, DynamicRetry]
        //[Ignore("SLV-3598")]
        //[Description("TS 4.18.1 Alarm Manager - Device alarm: no data received - Auto acknowledged")]
        //public void TS4_18_01()
        //{
        //    var testData = GetTestDataOfTestTS4_18_01();

        //    /* Basic info */
        //    var alarmName = testData["Alarm.name"] as string;
        //    var alarmType = testData["Alarm.type"] as string;
        //    var alarmAction = testData["Alarm.action"] as string;

        //    alarmName = SLVHelper.GenerateUniqueName(string.Format("[{0}][{1}][{2}] {3}", Browser.Name, Settings.Users["DefaultTest"].Username, TestContext.CurrentContext.Test.MethodName, alarmName));

        //    /* General tab */
        //    var autoAckowledge = Convert.ToBoolean(testData["Alarm.General.auto-acknowledge"]);
        //    var refreshRate = testData["Alarm.General.refresh-rate"] as string;
        //    var refreshRateRegex = Regex.Match(refreshRate, @"(\d*) (.*)");
        //    var refreshRateUnit = refreshRateRegex.Groups[2].Value;

        //    /* Trigger tab */
        //    var message = testData["Alarm.Trigger.message"] as string;
        //    var devices = testData["Alarm.Trigger.devices"] as string;
        //    var variablesType = testData["Alarm.Trigger.variables-type"] as string;
        //    var criticalDelay = testData["Alarm.Trigger.critical-delay"] as string;
        //    var timestampMode = testData["Alarm.Trigger.timestamp-mode"] as string;
        //    var criticalRatio = testData["Alarm.Trigger.critical-ratio"] as string;

        //    var deviceInfoRegex = Regex.Match(devices, @"(.*)#(.*)@(.*)");
        //    var deviceName = deviceInfoRegex.Groups[1].Value;
        //    var deviceId = deviceInfoRegex.Groups[2].Value;
        //    var controllerId = deviceInfoRegex.Groups[3].Value;
        //    var deviceNameWithControllerId = string.Format("{0} [@{1}]", deviceName, controllerId);

        //    /* Actions tab */
        //    var mailFrom = testData["Alarm.Actions.mail-from"] as string;
        //    var mailTo = testData["Alarm.Actions.mail-to"] as string;
        //    var mailSubject = alarmName;
        //    var mailContent = testData["Alarm.Actions.mail-content"] as string;

        //    var loginPage = Browser.OpenCMS();
        //    var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
        //    desktopPage.InstallAppsIfNotExist(App.AlarmManager, App.Alarms);

        //    Step("1. Go to Alarm Manager app");
        //    Step("2. Expected Alarm Manager page is routed and loaded successfully");
        //    var alarmManagerPage = desktopPage.GoToApp(App.AlarmManager) as AlarmManagerPage;

        //    Step("3. Select a geozone (should be configurable)");
        //    var geozone = testData["Geozone"] as string;
        //    alarmManagerPage.GeozoneTreeMainPanel.SelectNode(geozone);

        //    Step("4. Add an alarm");
        //    alarmManagerPage.GridPanel.ClickAddAlarmToolbarButton();
        //    alarmManagerPage.WaitForPreviousActionComplete();

        //    Step("5. Expected Add Alarm widget appears");
        //    VerifyEqual("5. Verify Alarm Details Panel appears", true, alarmManagerPage.AlarmEditorPanel.IsPanelVisible());

        //    Step("6. Specify report parameters");
        //    /* Basic info */
        //    alarmManagerPage.AlarmEditorPanel.EnterNameInput(alarmName);
        //    alarmManagerPage.AlarmEditorPanel.SelectTypeDropDown(alarmType);
        //    alarmManagerPage.WaitForPreviousActionComplete();
        //    alarmManagerPage.AlarmEditorPanel.SelectActionDropDown(alarmAction);

        //    /* General tab */
        //    // General tab is active by default
        //    alarmManagerPage.AlarmEditorPanel.SelectTab("General");
        //    alarmManagerPage.AlarmEditorPanel.TickAutoAcknowledgeCheckbox(autoAckowledge);
        //    alarmManagerPage.AlarmEditorPanel.SelectRefreshRateDropDown(refreshRate);

        //    /* Trigger tab */
        //    alarmManagerPage.AlarmEditorPanel.SelectTab("Trigger condition");
        //    alarmManagerPage.AlarmEditorPanel.EnterMessageInput(message);
        //    alarmManagerPage.AlarmEditorPanel.SelectDevicesListDropDown(deviceNameWithControllerId);
        //    alarmManagerPage.AlarmEditorPanel.SelectVariablesTypeListDropDown(variablesType);
        //    alarmManagerPage.AlarmEditorPanel.SelectCriticalDelayDropDown(criticalDelay);
        //    alarmManagerPage.AlarmEditorPanel.SelectTimestampModeDropDown(timestampMode);
        //    alarmManagerPage.AlarmEditorPanel.EnterCriticalRatioNumericInput(criticalRatio);

        //    /* Actions tab */
        //    alarmManagerPage.AlarmEditorPanel.SelectTab("Actions");
        //    alarmManagerPage.AlarmEditorPanel.EnterFromInput(mailFrom);
        //    alarmManagerPage.AlarmEditorPanel.SelectToListDropDown(mailTo);
        //    alarmManagerPage.AlarmEditorPanel.EnterSubjectInput(mailSubject);
        //    alarmManagerPage.AlarmEditorPanel.EnterEmailMessageInput(mailContent);

        //    Step("7. Click Save");
        //    alarmManagerPage.AlarmEditorPanel.ClickSaveButton();
        //    alarmManagerPage.WaitForPreviousActionComplete();
        //    VerifyEqual(string.Format("7. Verify the newly created alarm is present in the grid ({0})", alarmName), true, alarmManagerPage.GridPanel.IsAlarmManagerGridHasTextPresent("Name", alarmName));
        //    var alarmCreated = Settings.GetServerTime().ToString("G");

        //    Step("8. After 30 seconds or a bit longer, check alarms Alarms page and the email's inbox");
        //    Step("9. Expected");
        //    Step(" • In Alarms page and in the mailbox, alarms with name specified at step #6 are present in the grid");
        //    var foundEmail = GetEmailForAlarm(alarmName);
        //    var emailCheckingDateTime = Settings.GetServerTime().ToString("G");
        //    VerifyEqual(string.Format("9. Verify a sent alarm email is found in mailbox ({0}, Alarm created: {1}, Email checking: {2})", alarmName, alarmCreated, emailCheckingDateTime), true, foundEmail != null);
        //    EmailUtility.CleanInbox(alarmName);            
        //    desktopPage = Browser.RefreshLoggedInCMS();
        //    var alarmsPage = desktopPage.GoToApp(App.Alarms) as AlarmsPage;
        //    VerifyEqual(string.Format("9. Verify alarm is present in grid ({0})", alarmName), true, alarmsPage.GridPanel.IsAlarmGridHasTextPresent("Name", alarmName));

        //    Step("10. Go to Real-time Control browse to a device selected in step #6");
        //    desktopPage = Browser.RefreshLoggedInCMS();
        //    var realtimeControlPage = desktopPage.GoToApp(App.RealTimeControl) as RealTimeControlPage;

        //    Step("11. Click On on Luminair widget");
        //    realtimeControlPage.GeozoneTreeMainPanel.SelectNode(geozone + @"\" + deviceName);
        //    realtimeControlPage.StreetlightWidgetPanel.ClickCommandDim90Button();
        //    realtimeControlPage.StreetlightWidgetPanel.WaitForCommandCompleted();
        //    Wait.ForAlarmTrigger();

        //    Step("12. Reload grid");
        //    alarmsPage = realtimeControlPage.AppBar.SwitchTo(App.Alarms) as AlarmsPage;
        //    alarmsPage.GridPanel.ClickReloadDataToolbarButton();
        //    alarmsPage.WaitForPreviousActionComplete();

        //    Step("13. Expected The alarm is not present in the grid any more because it was auto-acknowledged");
        //    var isAlarmExisting = alarmsPage.GridPanel.IsAlarmGridHasTextPresent("Name", alarmName);
        //    VerifyEqual(string.Format("13. Verify alarm '{0}' is not present in grid any longer because it was auto-acknowledged", alarmName), false, isAlarmExisting);

        //    try
        //    {
        //        if (!isAlarmExisting)
        //        {
        //            alarmManagerPage = alarmsPage.AppBar.SwitchTo(App.AlarmManager) as AlarmManagerPage;
        //            alarmManagerPage.DeleteAlarm(alarmName, true);
        //        }
        //    }
        //    catch { }
        //}

        //[Test, DynamicRetry]
        //[Description("TS 4.19.1 Alarm Manager - Controller alarm: no data received - Auto acknowledged")]
        //public void TS4_19_01()
        //{          
        //    var testData = GetTestDataOfTestTS4_19_01();
        //    var geozone = testData["Geozone"] as string;
        //    var geozoneName = geozone.GetChildName();

        //    /* Basic info */
        //    var alarmName = testData["Alarm.name"] as string;
        //    var alarmType = testData["Alarm.type"] as string;
        //    var alarmAction = testData["Alarm.action"] as string;
        //    alarmName = SLVHelper.GenerateUniqueName(string.Format("[{0}][{1}][{2}] {3}", Browser.Name, Settings.Users["DefaultTest"].Username, TestContext.CurrentContext.Test.MethodName, alarmName));

        //    /* General tab */
        //    var autoAckowledge = Convert.ToBoolean(testData["Alarm.General.auto-acknowledge"]);
        //    var refreshRate = testData["Alarm.General.refresh-rate"] as string;
        //    var refreshRateRegex = Regex.Match(refreshRate, @"(\d*) (.*)");
        //    var refreshRateUnit = refreshRateRegex.Groups[2].Value;
        //    /* Trigger tab */
        //    var message = testData["Alarm.Trigger.message"] as string;
        //    var delay = testData["Alarm.Trigger.delay"] as string;
        //    var delayAsNumber = Convert.ToInt32(testData["Alarm.Trigger.delay"]);
        //    var controllers = testData["Alarm.Trigger.controllers"] as string;
        //    var controllerRegex = Regex.Match(controllers, @"(.*)#(.*)");
        //    var controllerName = controllerRegex.Groups[1].Value;
        //    var controllerId = controllerRegex.Groups[2].Value;
        //    /* Actions tab */
        //    var mailFrom = testData["Alarm.Actions.mail-from"] as string;
        //    var mailTo = testData["Alarm.Actions.mail-to"] as string;
        //    var mailSubject = alarmName;
        //    var mailContent = testData["Alarm.Actions.mail-content"] as string;

        //    var loginPage = Browser.OpenCMS();
        //    var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
        //    desktopPage.InstallAppsIfNotExist(App.AlarmManager, App.Alarms);

        //    Step("1. Go to Alarm Manager app");
        //    Step("2. Expected Alarm Manager page is routed and loaded successfully");
        //    var alarmManagerPage = desktopPage.GoToApp(App.AlarmManager) as AlarmManagerPage;

        //    Step("3. Select a geozone (should be configurable)");          
        //    alarmManagerPage.GeozoneTreeMainPanel.SelectNode(geozone);

        //    Step("4. Add an alarm");
        //    alarmManagerPage.GridPanel.ClickAddAlarmToolbarButton();
        //    alarmManagerPage.WaitForPreviousActionComplete();

        //    Step("5. Expected Add Alarm widget appears");
        //    VerifyEqual("5. Verify Alarm Details Panel appears", true, alarmManagerPage.AlarmEditorPanel.IsPanelVisible());

        //    Step("6. Specify report parameters");
        //    Step(" o Name: Any {{date time span}}");
        //    Step(" o Type: Controller alarm: no data received");
        //    Step(" o Action: Notify by eMail");
        //    Step(" o General tab:");
        //    Step("  + Auto-acknowledge: checked");
        //    Step("  + Refresh rate: 30 seconds (should be configurable)");
        //    Step(" o Trigger condition tab:");
        //    Step("  + Message: Any");
        //    Step("  + Delay hours: 0 (should be configurable)");
        //    Step("  + Controllers: selected from the list (should be configurable)");
        //    Step(" o Actions tab:");
        //    Step("  + From: qa@streetlightmonitoring.com");
        //    Step("  + To: Any valid mailbox");
        //    Step("  + Subject: Any");
        //    Step("  + Message: Any + ${ET} ${MN}");
        //    Step("    * Time: ${ET}");
        //    Step("    * Controller Name: ${MN}");
        //    Step("    * Duration of the absence of data (in hours): ${HD}");            
        //    alarmManagerPage.AlarmEditorPanel.EnterNameInput(alarmName);
        //    alarmManagerPage.AlarmEditorPanel.SelectTypeDropDown(alarmType);
        //    alarmManagerPage.WaitForPreviousActionComplete();
        //    alarmManagerPage.AlarmEditorPanel.SelectActionDropDown(alarmAction);
        //    alarmManagerPage.AlarmEditorPanel.SelectTab("General");
        //    alarmManagerPage.AlarmEditorPanel.TickAutoAcknowledgeCheckbox(autoAckowledge);
        //    alarmManagerPage.AlarmEditorPanel.SelectRefreshRateDropDown(refreshRate);
        //    alarmManagerPage.AlarmEditorPanel.SelectTab("Trigger condition");
        //    alarmManagerPage.AlarmEditorPanel.EnterMessageInput(message);
        //    alarmManagerPage.AlarmEditorPanel.EnterHoursDelayNumbericInput(delay);
        //    alarmManagerPage.AlarmEditorPanel.SelectControllersListDropDown(controllerName);
        //    alarmManagerPage.AlarmEditorPanel.SelectTab("Actions");
        //    alarmManagerPage.AlarmEditorPanel.EnterFromInput(mailFrom);
        //    alarmManagerPage.AlarmEditorPanel.SelectToListDropDown(mailTo);
        //    alarmManagerPage.AlarmEditorPanel.EnterSubjectInput(mailSubject);
        //    alarmManagerPage.AlarmEditorPanel.EnterEmailMessageInput(mailContent);

        //    Step("7. Click Save");
        //    alarmManagerPage.AlarmEditorPanel.ClickSaveButton();
        //    alarmManagerPage.WaitForPreviousActionComplete();
        //    VerifyEqual(string.Format("7. Verify the newly created alarm is present in the grid ({0})", alarmName), true, alarmManagerPage.GridPanel.IsAlarmManagerGridHasTextPresent("Name", alarmName));
        //    var curCtrlDateTime = Settings.GetServerTime();
        //    var alarmCreated = curCtrlDateTime.ToString("G");          

        //    Step("8. After 30 seconds or a bit longer, check alarms Alarms page and the email's inbox");
        //    Wait.ForAlarmTrigger();
        //    var alarmsPage = alarmManagerPage.AppBar.SwitchTo(App.Alarms) as AlarmsPage;
        //    alarmsPage.GridPanel.ClickReloadDataToolbarButton();
        //    alarmsPage.WaitForPreviousActionComplete();       
            
        //    Step("9. Expected");
        //    Step(" • In Alarms page and in the mailbox, alarms with name specified at step #6 are present in the grid");
        //    Step("  - Name: Alarm name from creating the alarm in Alarm Manager app");
        //    Step("  - Geozone: testing geozone");
        //    Step("  - Priority: 0");
        //    Step("  - Generator: Alarm name '-' random numbers");
        //    Step("  - Creation Date: the time the alarm is triggered with format {M/d/yyyy hh:mm:ss tt}, usually about 2 minutes after sending command.");
        //    Step("  - State: X red icon (active status)");
        //    Step("  - Last Change: equal to Creation Date");
        //    Step("  - User: -");
        //    Step("  - Info: message in 'Trigger condition' tab of Alarm Manager app when alarm is created");
        //    Step("  - Trigger Time: empty");
        //    Step("  - Trigger Info: empty");
        //    VerifyEqual(string.Format("9. Verify alarm is present in grid ({0})", alarmName), true, alarmsPage.GridPanel.IsAlarmGridHasTextPresent("Name", alarmName));
        //    var dtGridView = alarmManagerPage.GridPanel.BuildAlarmDataTable();
        //    var row = dtGridView.Select(string.Format("Name = '{0}'", alarmName)).FirstOrDefault();
        //    if (row != null)
        //    {
        //        var creationDate = DateTime.Parse(row["Creation Date"].ToString());
        //        var timeSpan = curCtrlDateTime.Subtract(creationDate);
        //        var iconList = alarmManagerPage.GridPanel.GetListOfAlarmGridIconColumn(alarmName);
        //        VerifyEqual("9. Verify Geozone is " + geozoneName, geozoneName, row["Geozone"].ToString());
        //        VerifyEqual("9. Verify Priority: 0", "0", row["Priority"].ToString());
        //        VerifyTrue("9. Verify Generator: Alarm name '-' random numbers", row["Generator"].ToString().Contains(alarmName), alarmName, row["Generator"].ToString());
        //        VerifyTrue("9. Verify Creation Date: the time the alarm is triggered with format {M/d/yyyy hh:mm:ss tt}, usually about 2 minutes after sending command.", Math.Abs(timeSpan.TotalMinutes) <= 2, curCtrlDateTime, creationDate);
        //        VerifyEqual("9. Verify State: X red icon (active status)", true, iconList.Count == 1 && iconList.Any(p => p.Contains("status-error.png")));
        //        VerifyEqual("9. Verify Last Change: equal to Creation Date", row["Creation Date"].ToString(), row["Last Change"].ToString());
        //        VerifyEqual("9. Verify User: -", "-", row["User"].ToString());
        //        VerifyEqual("9. Verify Info: message in 'Trigger condition' tab of Alarm Manager app when alarm is created", message, row["Info"].ToString());
        //        VerifyEqual("9. Verify Trigger Time: empty", "", row["Trigger Time"].ToString());
        //        VerifyEqual("9. Verify Trigger Info: empty", "", row["Trigger Info"].ToString());
        //    }
        //    else
        //    {
        //        Warning(string.Format("9. There is no row with alarm '{0}'", alarmName));
        //    }

        //    Step("10. Check the email");
        //    Step("11. Verify an email is sent with");
        //    Step(" - Subject: subject set up in Actions tab of Alarm Manager");
        //    Step(" - Contains: message set up in Actions tab of Alarm Manager");
        //    Step("   + Time: Create Date with format yyyy-MM-dd HH:mm:ss. Ex: 2018-05-17 06:51:42");
        //    Step("   + Controller Name: testing controller name");
        //    Step("   + Duration of the absence of data (in hours): number (> 0)");
        //    var foundEmail = GetEmailForAlarm(alarmName);
        //    var hasEmail = foundEmail != null;
        //    var emailCheckingDateTime = Settings.GetServerTime().ToString("G");
        //    VerifyEqual(string.Format("9. Verify a sent alarm email is found in mailbox ({0}, Alarm created: {1}, Email checking: {2})", alarmName, alarmCreated, emailCheckingDateTime), true, hasEmail);
        //    if (hasEmail)
        //    {
        //        var arrBody = foundEmail.Body.SplitEx("|");
        //        var emailTime = arrBody[0].Trim();
        //        var emailController = arrBody[1].Trim();
        //        var emailDelay = arrBody[2].Trim();

        //        var timeSpan = curCtrlDateTime.Subtract(DateTime.Parse(emailTime));
        //        VerifyTrue("11. Verify Time: Create Date with format yyyy-MM-dd HH:mm:ss. Ex: 2018-05-17 06:51:42 usually about 2 minutes after sending command.", Math.Abs(timeSpan.TotalMinutes) <= 2, curCtrlDateTime, emailTime);
        //        VerifyEqual("11. Verify Controller Name: testing controller name", controllerName, emailController);
        //        VerifyTrue(string.Format("11. Verify Duration of the absence of data (in hours) is number (> {0})", delay), int.Parse(emailDelay) >= delayAsNumber, delay, emailDelay);
        //        EmailUtility.CleanInbox(alarmName);
        //    }

        //    Step("12. Send this request /api/loggingmanagement/setDeviceValues?controllerStrId={{controller id}}&idOnController=controllerdevice&valueName=DigitalInput1&value=**ON**&doLog=true&eventTime={{current date time stamp}}");
        //    Step("13. Expected The request is sent successfully, the response returns ok");
        //    var isRequestOk = SetValueToController(controllerId, "DigitalInput1", "ON", Settings.GetServerTime(), true);
        //    VerifyEqual("13. Verify The request is sent successfully, the response returns ok", true, isRequestOk);

        //    Step("14. After 30 seconds or a bit longer, reload grid, press the RED BELL icon");
        //    Wait.ForAlarmTrigger();           
        //    alarmsPage.GridPanel.ClickReloadDataToolbarButton();
        //    alarmsPage.WaitForPreviousActionComplete();
        //    alarmsPage.GridPanel.ClickShowAllAlarmsToolbarOption();
        //    alarmsPage.WaitForPreviousActionComplete();
        //    alarmsPage.GridPanel.WaitForGridContentAvailable();
        //    var acknowledgeTime = Settings.GetServerTime();

        //    Step("15. Verify The alarm is auto-acknowledged and updated with");
        //    Step(" - The Status is changed to Acknowledged (Green Check icon)");
        //    Step(" - Last Change: the datetime when alarm is acknowledged");
        //    Step(" - User: 'auto'");
        //    Step(" - Trigger Time: Creation Date");
        //    Step(" - Trigger Info: Alarm message");
        //    var iconList1 = alarmsPage.GridPanel.GetListOfAlarmGridIconColumn(alarmName);
        //    VerifyEqual("15. Verify The Status is changed to Acknowledged (Green Check icon)", true, iconList1.All(p => p.Contains("status-ok.png")));            
        //    dtGridView = alarmsPage.GridPanel.BuildAlarmDataTable();
        //    row = dtGridView.Select(string.Format("Name = '{0}'", alarmName)).FirstOrDefault();
        //    if (row != null)
        //    {
        //        var lastChangeDate = DateTime.Parse(row["Last Change"].ToString());
        //        var timeSpan = acknowledgeTime.Subtract(lastChangeDate);
        //        var iconList = alarmsPage.GridPanel.GetListOfAlarmGridIconColumn(alarmName);
        //        VerifyTrue("15. Verify Last Change: the datetime when alarm is acknowledged", Math.Abs(timeSpan.TotalMinutes) <= 2, acknowledgeTime, lastChangeDate);
        //        VerifyEqual("15. Verify User: the user who acknowledged the alarm", "auto", row["User"].ToString());
        //        VerifyEqual("15. Verify Trigger Time: Creation Date", row["Creation Date"].ToString(), row["Trigger Time"].ToString());
        //        VerifyEqual("15. Verify Trigger Info: Alarm message", message, row["Trigger Info"].ToString());
        //    }
        //    else
        //    {
        //        Warning(string.Format("15. There is no row with alarm '{0}'", alarmName));
        //    }

        //    try
        //    {
        //        alarmManagerPage = alarmsPage.AppBar.SwitchTo(App.AlarmManager) as AlarmManagerPage;
        //        alarmManagerPage.DeleteAlarm(alarmName, true);
        //    }
        //    catch { }
        //}

        //[Test, DynamicRetry]
        //[Description("TS 4.20.1 Alarm Manager - Controller alarm: ON/OFF at dawn/dusk - Auto acknowledged")]
        //[Ignore("The test is hard to be automated. Considered later")]
        //public void TS4_20_01()
        //{
        //}

        //[Test, DynamicRetry]
        //[Description("TS 4.20.2 Alarm Manager - Controller alarm: ON/OFF times vs previous day - Auto acknowledged")]
        //[NonParallelizable]
        //public void TS4_20_02()
        //{
        //    var testData = GetTestDataOfTestTS4_20_02();
        //    var geozone = SLVHelper.GenerateUniqueName("GZNTS42002");
        //    var controller = SLVHelper.GenerateUniqueName("CTRL");

        //    /* Basic info */
        //    var alarmName = testData["Alarm.name"] as string;
        //    var alarmType = testData["Alarm.type"] as string;
        //    var alarmAction = testData["Alarm.action"] as string;
        //    alarmName = SLVHelper.GenerateUniqueName(string.Format("[{0}][{1}][{2}] {3}", Browser.Name, Settings.Users["DefaultTest"].Username, TestContext.CurrentContext.Test.MethodName, alarmName));

        //    /* General tab */
        //    var autoAckowledge = Convert.ToBoolean(testData["Alarm.General.auto-acknowledge"]);
        //    var refreshRate = testData["Alarm.General.refresh-rate"] as string;
        //    var refreshRateRegex = Regex.Match(refreshRate, @"(\d*) (.*)");
        //    var refreshRateUnit = refreshRateRegex.Groups[2].Value;

        //    /* Trigger tab */
        //    var message = testData["Alarm.Trigger.message"] as string;
        //    var io = testData["Alarm.Trigger.io"] as string;
        //    var delay = testData["Alarm.Trigger.delay"] as string;
        //    var delayAsNumber = Convert.ToInt32(testData["Alarm.Trigger.delay"]);
        //    var ioNameReg = Regex.Match(io, @"(.*)#(.*)");
        //    var ioNameDisplayed = ioNameReg.Groups[1].Value;
        //    var ioId = ioNameReg.Groups[2].Value;

        //    /* Actions tab */
        //    var mailFrom = testData["Alarm.Actions.mail-from"] as string;
        //    var mailTo = testData["Alarm.Actions.mail-to"] as string;
        //    var mailSubject = alarmName;
        //    var mailContent = testData["Alarm.Actions.mail-content"] as string;

        //    Step("-> Create data for testing");
        //    DeleteGeozones("GZNTS42002*");
        //    CreateNewGeozone(geozone);
        //    CreateNewController(controller, geozone);

        //    var loginPage = Browser.OpenCMS();
        //    var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
        //    desktopPage.InstallAppsIfNotExist(App.AlarmManager, App.Alarms);
          
        //    Step("1. Go to Alarm Manager app");
        //    Step("2. Expected Alarm Manager page is routed and loaded successfully");
        //    var alarmManagerPage = desktopPage.GoToApp(App.AlarmManager) as AlarmManagerPage;

        //    Step("3. Select a geozone (should be configurable)");            
        //    alarmManagerPage.GeozoneTreeMainPanel.SelectNode(geozone);

        //    Step("4. Add an alarm");
        //    alarmManagerPage.GridPanel.ClickAddAlarmToolbarButton();
        //    alarmManagerPage.WaitForPreviousActionComplete();

        //    Step("5. Expected Add Alarm widget appears");
        //    VerifyEqual("5. Verify Alarm Details Panel appears", true, alarmManagerPage.AlarmEditorPanel.IsPanelVisible());

        //    Step("6. Specify report parameters");
        //    Step(" o Name: Any {{date time span}}");
        //    Step(" o Type: Controller alarm: ON/OFF times vs previous day");
        //    Step(" o Action: Notify by eMail");
        //    Step(" o General tab:");
        //    Step("  + Auto-acknowledge: checked");
        //    Step("  + Refresh rate: 30 seconds (should be configurable)");
        //    Step(" o Trigger condition tab:");
        //    Step("  + Message: Any");
        //    Step("  + Controllers: selected from the list (should be configurable)");
        //    Step("  + IO: Digital Output 1(should be configurable)");
        //    Step("  + Delay: 10 (should be configurable)");
        //    Step(" o Actions tab:");
        //    Step("  + From: qa@streetlightmonitoring.com");
        //    Step("  + To: Any valid mailbox");
        //    Step("  + Subject: Any");
        //    Step("  + Message:");
        //    Step("    * Time: ${ET}");
        //    Step("    * Controller's name: ${CN}");            
        //    alarmManagerPage.AlarmEditorPanel.EnterNameInput(alarmName);
        //    alarmManagerPage.AlarmEditorPanel.SelectTypeDropDown(alarmType);
        //    alarmManagerPage.WaitForPreviousActionComplete();
        //    alarmManagerPage.AlarmEditorPanel.SelectActionDropDown(alarmAction);            
        //    alarmManagerPage.AlarmEditorPanel.SelectTab("General");
        //    alarmManagerPage.AlarmEditorPanel.TickAutoAcknowledgeCheckbox(autoAckowledge);
        //    alarmManagerPage.AlarmEditorPanel.SelectRefreshRateDropDown(refreshRate);
        //    alarmManagerPage.AlarmEditorPanel.SelectTab("Trigger condition");
        //    alarmManagerPage.AlarmEditorPanel.EnterMessageInput(message);
        //    alarmManagerPage.AlarmEditorPanel.SelectControllersListDropDown(controller);
        //    alarmManagerPage.AlarmEditorPanel.SelectInputOutputDropDown(ioNameDisplayed);
        //    alarmManagerPage.AlarmEditorPanel.EnterDelayNumbericInput(delay);
        //    alarmManagerPage.AlarmEditorPanel.SelectTab("Actions");
        //    alarmManagerPage.AlarmEditorPanel.EnterFromInput(mailFrom);
        //    alarmManagerPage.AlarmEditorPanel.SelectToListDropDown(mailTo);
        //    alarmManagerPage.AlarmEditorPanel.EnterSubjectInput(mailSubject);
        //    alarmManagerPage.AlarmEditorPanel.EnterEmailMessageInput(mailContent);

        //    Step("7. Click Save");
        //    alarmManagerPage.AlarmEditorPanel.ClickSaveButton();
        //    alarmManagerPage.WaitForPreviousActionComplete();
        //    var alarmCreated = Settings.GetServerTime().ToString("G");
        //    VerifyEqual(string.Format("7. Verify the newly created alarm is present in the grid ({0})", alarmName), true, alarmManagerPage.GridPanel.IsAlarmManagerGridHasTextPresent("Name", alarmName));

        //    Step("8. Send the 2 commmands to simulate the data for previous day and today");
        //    Step(" - Previous Day");
        //    Step("  + valueName=IO's value");
        //    Step("  + value=ON");
        //    Step("  + eventTime=[previous day] [current time-Delay]");
        //    Step(" - Today");
        //    Step("  + valueName=IO's value");
        //    Step("  + value=OFF");
        //    Step("  + eventTime=[today] [current time]");
        //    var curCtrlDateTime = Settings.GetServerTime();            
        //    var curPrevDateTime = curCtrlDateTime.AddDays(-1).AddMinutes(0 - (delayAsNumber / 2));
        //    var isRequestOk = SetValueToController(controller, ioId, "ON", curPrevDateTime);
        //    VerifyEqual("8. Verify The request Previous Day is sent successfully, the response returns ok", true, isRequestOk);
        //    isRequestOk = SetValueToController(controller, ioId, "OFF", curCtrlDateTime);
        //    VerifyEqual("8. Verify The request Today is sent successfully, the response returns ok", true, isRequestOk);

        //    Step("9. After 30 seconds or a bit longer, check alarms Alarms page and the email's inbox");
        //    Wait.ForAlarmTrigger();
        //    var alarmsPage = alarmManagerPage.AppBar.SwitchTo(App.Alarms) as AlarmsPage;
        //    alarmsPage.GridPanel.ClickReloadDataToolbarButton();
        //    alarmsPage.WaitForPreviousActionComplete();            
        //    VerifyEqual(string.Format("9. Verify alarm is present in grid ({0})", alarmName), true, alarmsPage.GridPanel.IsAlarmGridHasTextPresent("Name", alarmName));

        //    Step("10. Verify In Alarms page, an alarm is present in the grid with");
        //    Step(" - Name: Alarm name from creating the alarm in Alarm Manager app");
        //    Step(" - Geozone: testing geozone");
        //    Step(" - Priority: 0");
        //    Step(" - Generator: Alarm name '-' random numbers");
        //    Step(" - Creation Date: the time the alarm is triggered with format {M/d/yyyy hh:mm:ss tt}, usually about 2 minutes after sending command.");
        //    Step(" - State: X red icon (active status)");
        //    Step(" - Last Change: equal to Creation Date");
        //    Step(" - User: -");
        //    Step(" - Info: message in 'Trigger condition' tab of Alarm Manager app when alarm is created");
        //    Step(" - Trigger Time: empty");
        //    Step(" - Trigger Info: empty");
        //    VerifyEqual(string.Format("10. Verify alarm is present in grid ({0})", alarmName), true, alarmsPage.GridPanel.IsAlarmGridHasTextPresent("Name", alarmName));
        //    var dtGridView = alarmManagerPage.GridPanel.BuildAlarmDataTable();
        //    var row = dtGridView.Select(string.Format("Name = '{0}'", alarmName)).FirstOrDefault();
        //    if (row != null)
        //    {
        //        var creationDate = DateTime.Parse(row["Creation Date"].ToString());
        //        var timeSpan = curCtrlDateTime.Subtract(creationDate);
        //        var iconList = alarmManagerPage.GridPanel.GetListOfAlarmGridIconColumn(alarmName);
        //        VerifyEqual("10. Verify Geozone is " + geozone, geozone, row["Geozone"].ToString());
        //        VerifyEqual("10. Verify Priority: 0", "0", row["Priority"].ToString());
        //        VerifyTrue("10. Verify Generator: Alarm name '-' random numbers", row["Generator"].ToString().Contains(alarmName), alarmName, row["Generator"].ToString());
        //        VerifyTrue("10. Verify Creation Date: the time the alarm is triggered with format {M/d/yyyy hh:mm:ss tt}, usually about 2 minutes after sending command.", Math.Abs(timeSpan.TotalMinutes) <= 2, curCtrlDateTime, creationDate);
        //        VerifyEqual("10. Verify State: X red icon (active status)", true, iconList.Count == 1 && iconList.Any(p => p.Contains("status-error.png")));
        //        VerifyEqual("10. Verify Last Change: equal to Creation Date", row["Creation Date"].ToString(), row["Last Change"].ToString());
        //        VerifyEqual("10. Verify User: -", "-", row["User"].ToString());
        //        VerifyEqual("10. Verify Info: message in 'Trigger condition' tab of Alarm Manager app when alarm is created", message, row["Info"].ToString());
        //        VerifyEqual("10. Verify Trigger Time: empty", "", row["Trigger Time"].ToString());
        //        VerifyEqual("10. Verify Trigger Info: empty", "", row["Trigger Info"].ToString());
        //    }
        //    else
        //    {
        //        Warning(string.Format("10. There is no row with alarm '{0}'", alarmName));
        //    }

        //    Step("11. Check the email");
        //    Step("12. Verify an email is sent with");
        //    Step(" - Subject: subject set up in Actions tab of Alarm Manager");
        //    Step(" - Contains: message set up in Actions tab of Alarm Manager");
        //    Step("   + Time: Create Date with format yyyy-MM-dd HH:mm:ss. Ex: 2018-05-17 06:51:42");
        //    Step("   + Controller Name: testing controller name");
        //    var foundEmail = GetEmailForAlarm(alarmName);
        //    var hasEmail = foundEmail != null;
        //    var emailCheckingDateTime = Settings.GetServerTime().ToString("G");
        //    VerifyEqual(string.Format("12. Verify a sent alarm email is found in mailbox ({0}, Alarm created: {1}, Email checking: {2})", alarmName, alarmCreated, emailCheckingDateTime), true, hasEmail);
        //    if (hasEmail)
        //    {
        //        var arrBody = foundEmail.Body.SplitEx("|");
        //        var emailTime = arrBody[0].Trim();
        //        var emailController = arrBody[1].Trim();

        //        VerifyTrue(string.Format("12. Verify Time ({0}): datetime when the simulated command sent with format: yyyy-MM-dd HH:mm:ss", emailTime), Settings.CheckDateTimeMatchFormats(emailTime, "yyyy-MM-dd HH:mm:ss"), "Time format: yyyy-MM-dd HH:mm:ss", emailTime);
        //        VerifyEqual("12. Verify Controller Name: testing controller name", controller, emailController);
        //        EmailUtility.CleanInbox(alarmName);
        //    }            

        //    Step("13. Go to Alarm Manager page and update the Delay: 0 (minute), then save changes. After 90 seconds or a bit longer, check alarms Alarms page");
        //    alarmManagerPage = alarmsPage.AppBar.SwitchTo(App.AlarmManager) as AlarmManagerPage;
        //    alarmManagerPage.GridPanel.ClickGridRecord(alarmName);
        //    alarmManagerPage.WaitForPreviousActionComplete();
        //    alarmManagerPage.AlarmEditorPanel.SelectTab("Trigger condition");
        //    alarmManagerPage.AlarmEditorPanel.EnterDelayNumbericInput("0");
        //    alarmManagerPage.AlarmEditorPanel.ClickSaveButton();
        //    alarmManagerPage.WaitForPreviousActionComplete();           
        //    Wait.ForAlarmTrigger();
        //    alarmsPage = alarmManagerPage.AppBar.SwitchTo(App.Alarms) as AlarmsPage;
        //    alarmsPage.GridPanel.ClickReloadDataToolbarButton();
        //    alarmsPage.WaitForPreviousActionComplete();
        //    curCtrlDateTime = Settings.GetServerTime();            

        //    Step("14. Verify alarm is not presented in grid any longer because it has been auto-acknowledged");            
        //    VerifyEqual(string.Format("14. Verify alarm '{0}' is not present in grid any longer because it was auto-acknowledged", alarmName), false, alarmsPage.GridPanel.IsAlarmGridHasTextPresent("Name", alarmName));

        //    Step("15. Press the Red Bell icon");
        //    alarmsPage.GridPanel.ClickShowAllAlarmsToolbarOption();
        //    alarmsPage.WaitForPreviousActionComplete();
        //    alarmsPage.GridPanel.WaitForGridContentAvailable();

        //    Step("16. Verify The testing alarm is updated");
        //    Step(" - State: Green Checked icon");
        //    Step(" - Info: 'Auto acknowledged'");
        //    Step(" - Last Change: time when acknowledged with format {M/d/yyyy hh:mm:ss tt}");
        //    Step(" - User: 'auto'");
        //    Step(" - Trigger Time: the time the alarm is trigger with format {M/d/yyyy h:mm:ss tt}");
        //    Step(" - Trigger Info: message in 'Trigger condition' tab of Alarm Manager app when alarm is created");
        //    var iconList1 = alarmsPage.GridPanel.GetListOfAlarmGridIconColumn(alarmName);
        //    VerifyEqual("16. Verify The Status is changed to Acknowledged (Green Check icon)", true, iconList1.All(p => p.Contains("status-ok.png")));
        //    dtGridView = alarmsPage.GridPanel.BuildAlarmDataTable();
        //    row = dtGridView.Select(string.Format("Name = '{0}'", alarmName)).FirstOrDefault();
        //    if (row != null)
        //    {
        //        var lastChangeDate = row["Last Change"].ToString();
        //        var triggerTime = row["Trigger Time"].ToString();
        //        var iconList = alarmsPage.GridPanel.GetListOfAlarmGridIconColumn(alarmName);
        //        VerifyEqual("16. Verify Info: 'Auto acknowledged'", "Auto acknowledged", row["Info"].ToString());
        //        VerifyEqual(string.Format("16. Verify Last Change ({0}): time when acknowledged with format: M/d/yyyy h:mm:ss tt", lastChangeDate), true, Settings.CheckDateTimeMatchFormats(lastChangeDate, "M/d/yyyy h:mm:ss tt"));
        //        VerifyEqual("16. Verify User: the user who acknowledged the alarm", "auto", row["User"].ToString());
        //        VerifyEqual(string.Format("16. Verify Trigger Time ({0}): the time the alarm is trigger with format: M/d/yyyy h:mm:ss tt", triggerTime), true, Settings.CheckDateTimeMatchFormats(lastChangeDate, "M/d/yyyy h:mm:ss tt"));
        //        VerifyEqual("16. Verify Trigger Info: Alarm message", message, row["Trigger Info"].ToString());
        //    }
        //    else
        //    {
        //        Warning(string.Format("16. There is no row with alarm '{0}'", alarmName));
        //    }

        //    if (!alarmsPage.GridPanel.IsShowAllAlarmsOptionToggledOn())
        //    {
        //        alarmsPage.GridPanel.ClickShowAllAlarmsToolbarOption();
        //        alarmsPage.WaitForPreviousActionComplete();
        //    }

        //    Step("17. Go to Alarm Manager app and update the Delay: 30 (minutes); check 'New alarm if re-triggerd' and save changes");
        //    alarmManagerPage = alarmsPage.AppBar.SwitchTo(App.AlarmManager) as AlarmManagerPage;
        //    alarmManagerPage.GridPanel.ClickGridRecord(alarmName);
        //    alarmManagerPage.WaitForPreviousActionComplete();
        //    alarmManagerPage.AlarmEditorPanel.SelectTab("General");
        //    alarmManagerPage.AlarmEditorPanel.TickNewAlarmIfRetriggerCheckbox(true);
        //    alarmManagerPage.AlarmEditorPanel.SelectTab("Trigger condition");
        //    alarmManagerPage.AlarmEditorPanel.EnterDelayNumbericInput("30");
        //    alarmManagerPage.AlarmEditorPanel.ClickSaveButton();
        //    alarmManagerPage.WaitForPreviousActionComplete();
        //    var alarmUpdated = Settings.GetServerTime().ToString("G");

        //    Step("18. Wait for 90s or a bit longer, then go to the testing geozone in Alarm app and press Refresh button");
        //    Wait.ForAlarmTrigger();
        //    alarmsPage = alarmManagerPage.AppBar.SwitchTo(App.Alarms) as AlarmsPage;
        //    alarmsPage.GridPanel.ClickReloadDataToolbarButton();
        //    alarmsPage.WaitForPreviousActionComplete();

        //    Step("19. Verify");
        //    Step(" - A new email sent");
        //    Step(" - An new alarm added to Alarm panel in Alarm app");
        //    VerifyEqual(string.Format("19. Verify new alarm '{0}' is added", alarmName), true, alarmsPage.GridPanel.IsAlarmGridHasTextPresent("Name", alarmName));
        //    foundEmail = GetEmailForAlarm(alarmName);
        //    hasEmail = foundEmail != null;
        //    emailCheckingDateTime = Settings.GetServerTime().ToString("G");
        //    VerifyEqual(string.Format("19. Verify a sent alarm email is found in mailbox ({0}, Alarm created: {1}, Email checking: {2})", alarmName, alarmUpdated, emailCheckingDateTime), true, hasEmail);
        //    if (hasEmail) EmailUtility.CleanInbox(alarmName);

        //    Step("20. Press Red Bell icon to show all alarms");
        //    alarmsPage.GridPanel.ClickShowAllAlarmsToolbarOption();
        //    alarmsPage.WaitForPreviousActionComplete();
        //    alarmsPage.GridPanel.WaitForGridContentAvailable();

        //    Step("21. Verify 2 rows of the alarm in the Alarm panel");
        //    Step(" - 1st row with status: Active (Red X icon)");
        //    Step(" - 2nd row with status: Acknowledged (Green Check icon)");
        //    iconList1 = alarmsPage.GridPanel.GetListOfAlarmGridIconColumn(alarmName);
        //    VerifyEqual("21. Verify 2 rows of the alarm in the Alarm panel", 2, iconList1.Count);
        //    VerifyEqual("21. Verify 1st row with status: Active (Red X icon)", true, iconList1.Any(p => p.Contains("status-error.png")));
        //    VerifyEqual("21. Verify 2nd row with status: Acknowledged (Green Check icon)", true, iconList1.Any(p => p.Contains("status-ok.png")));

        //    Step("22. Select the row with status Active (Red X icon) and press Acknowledge button and enter the message to Acknowledge Alarms pop-up then press Send button and press OK");
        //    alarmsPage.GridPanel.ClickAlarmGridRecordHasErrorIcon(alarmName);
        //    alarmsPage.GridPanel.ClickAcknowledgeToolbarButton();
        //    alarmsPage.WaitForPopupDialogDisplayed();
        //    alarmsPage.Dialog.ClickSendButton();
        //    alarmsPage.Dialog.WaitForPopupMessageDisplayed();
        //    alarmsPage.Dialog.ClickOkButton();
        //    alarmsPage.WaitForPopupDialogDisappeared();
        //    var acknowledgeTime = Settings.GetServerTime();

        //    Step("23. Verify The Status is changed to Acknowledged (Green Check icon)");
        //    iconList1 = alarmsPage.GridPanel.GetListOfAlarmGridIconColumn(alarmName);
        //    VerifyEqual("23. Verify The Status is changed to Acknowledged (Green Check icon)", true, iconList1.All(p => p.Contains("status-ok.png")));

        //    Step("24. Press Refresh button");
        //    alarmsPage.GridPanel.ClickReloadDataToolbarButton();
        //    alarmsPage.WaitForPreviousActionComplete();

        //    Step("25. Verify The row is updated with");
        //    Step(" - Last Change: the datetime when alarm is acknowledged");
        //    Step(" - User: the user who acknowledged the alarm");
        //    Step(" - Trigger Time: Creation Date");
        //    Step(" - Trigger Info: Alarm name");          
        //    dtGridView = alarmsPage.GridPanel.BuildAlarmDataTable();
        //    row = dtGridView.Select(string.Format("Name = '{0}'", alarmName)).FirstOrDefault();
        //    if (row != null)
        //    {
        //        var lastChangeDate = DateTime.Parse(row["Last Change"].ToString());
        //        var timeSpan = acknowledgeTime.Subtract(lastChangeDate);

        //        var iconList = alarmsPage.GridPanel.GetListOfAlarmGridIconColumn(alarmName);
        //        VerifyTrue("25. Verify Last Change: the datetime when alarm is acknowledged", Math.Abs(timeSpan.TotalMinutes) <= 2, acknowledgeTime, lastChangeDate);
        //        VerifyEqual("25. Verify User: the user who acknowledged the alarm", Settings.Users["DefaultTest"].Username, row["User"].ToString());
        //        VerifyEqual("25. Verify Trigger Time: Creation Date", row["Creation Date"].ToString(), row["Trigger Time"].ToString());
        //        VerifyEqual("25. Verify Trigger Info: Alarm message", message, row["Trigger Info"].ToString());
        //    }
        //    else
        //    {
        //        Warning(string.Format("25. There is no row with alarm '{0}'", alarmName));
        //    }

        //    try
        //    {               
        //        alarmManagerPage = alarmsPage.AppBar.SwitchTo(App.AlarmManager) as AlarmManagerPage;
        //        alarmManagerPage.DeleteAlarm(alarmName, true);
        //        DeleteGeozone(geozone);
        //    }
        //    catch { }
        //}

        //[Test, DynamicRetry]
        //[Description("TS 4.21.1 Alarm Manager - Controller alarm: last known state of an I/O - Auto acknowledged")]
        //[NonParallelizable]
        //public void TS4_21_01()
        //{
        //    var testData = GetTestDataOfTestTS4_21_01();
        //    var geozone = SLVHelper.GenerateUniqueName("GZNTS42101");
        //    var controller = SLVHelper.GenerateUniqueName("CTRL");

        //    /* Basic info */
        //    var alarmName = testData["Alarm.name"] as string;
        //    var alarmType = testData["Alarm.type"] as string;
        //    var alarmAction = testData["Alarm.action"] as string;
        //    alarmName = SLVHelper.GenerateUniqueName(string.Format("[{0}][{1}][{2}] {3}", Browser.Name, Settings.Users["DefaultTest"].Username, TestContext.CurrentContext.Test.MethodName, alarmName));

        //    /* General tab */
        //    var autoAckowledge = Convert.ToBoolean(testData["Alarm.General.auto-acknowledge"]);
        //    var refreshRate = testData["Alarm.General.refresh-rate"] as string;
        //    var refreshRateRegex = Regex.Match(refreshRate, @"(\d*) (.*)");
        //    var refreshRateUnit = refreshRateRegex.Groups[2].Value;

        //    /* Trigger tab */
        //    var message = testData["Alarm.Trigger.message"] as string;
        //    var inputName = testData["Alarm.Trigger.input-name"] as string;
        //    var inputValue = testData["Alarm.Trigger.input-value"] as string;
        //    var inputNameReg = Regex.Match(inputName, @"(.*)#(.*)");
        //    var inputNameDisplayed = inputNameReg.Groups[1].Value;
        //    var inputNameId = inputNameReg.Groups[2].Value;

        //    /* Actions tab */
        //    var mailFrom = testData["Alarm.Actions.mail-from"] as string;
        //    var mailTo = testData["Alarm.Actions.mail-to"] as string;
        //    var mailSubject = alarmName;
        //    var mailContent = testData["Alarm.Actions.mail-content"] as string;

        //    Step("-> Create data for testing");
        //    DeleteGeozones("GZNTS42101*");
        //    CreateNewGeozone(geozone);
        //    CreateNewController(controller, geozone);
        //    // Send this request to make sure alarm is triggered
        //    SetValueToController(controller, inputNameId, inputValue, Settings.GetServerTime().AddHours(-1));

        //    var loginPage = Browser.OpenCMS();
        //    var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
        //    desktopPage.InstallAppsIfNotExist(App.AlarmManager, App.Alarms);     

        //    Step("1. Go to Alarm Manager app");
        //    Step("2. Expected Alarm Manager page is routed and loaded successfully");
        //    var alarmManagerPage = desktopPage.GoToApp(App.AlarmManager) as AlarmManagerPage;

        //    Step("3. Select a geozone (should be configurable)");
        //    alarmManagerPage.GeozoneTreeMainPanel.SelectNode(geozone);

        //    Step("4. Add an alarm");
        //    alarmManagerPage.GridPanel.ClickAddAlarmToolbarButton();
        //    alarmManagerPage.WaitForPreviousActionComplete();

        //    Step("5. Expected Add Alarm widget appears");
        //    VerifyEqual("5. Verify Alarm Details Panel appears", true, alarmManagerPage.AlarmEditorPanel.IsPanelVisible());

        //    Step("6. Specify report parameters");
        //    Step(" o Name: Any {{date time span}}");
        //    Step(" o Type: Controller alarm: last known state of an I/O");
        //    Step(" o Action: Notify by eMail");
        //    Step(" o General tab:");
        //    Step("  + Auto-acknowledge: checked");
        //    Step("  + Refresh rate: 30 seconds (should be configurable)");
        //    Step(" o Trigger condition tab:");
        //    Step("  + Message: Any");    
        //    Step("  + Controllers: selected from the list (should be configurable)");
        //    Step("  + Input Name: Digital Output 1 or selected from the list (should be configurable)");
        //    Step("  + Input Value: On or selected from the list (should be configurable)");
        //    Step(" o Actions tab:");
        //    Step("  + From: qa@streetlightmonitoring.com");
        //    Step("  + To: Any valid mailbox");
        //    Step("  + Subject: Any");
        //    Step("  + Message:");
        //    Step("    * Time: ${ET}");
        //    Step("    * Controller's name: ${CN}");
        //    Step("    * Date and time of the I/O event triggering this alarm: ${LT}");
        //    Step("    * Current server time: ${ST}");
        //    Step("    * Geozone: ${GZ}");
        //    Step("    * Controller ID: ${CI}");
        //    Step("    * ${IOIVV} should be the same ${IO1IVV} and belong to Input Name defined when creating an alarm. Note: IOIVV and IO1IVV are two different names but are actually the same thing. They represent the label of the I/O that the user has chosen in the alarm definition, so they can be any of:");
        //    Step("      - Input 1 - Label");
        //    Step("      - Output 1 - Label");
        //    Step("      - Input 2 - Label");
        //    Step("      - Output 2 - Label");
        //    Step("      - For instance, when creating an alarm, Input Name in Trigger Condition tab is set: Digital Input 1, the value of IOIVV & IO1IVV is Input 1 - Label in Input and Output tab of the testing controller.");            
        //    alarmManagerPage.AlarmEditorPanel.EnterNameInput(alarmName);
        //    alarmManagerPage.AlarmEditorPanel.SelectTypeDropDown(alarmType);
        //    alarmManagerPage.WaitForPreviousActionComplete();
        //    alarmManagerPage.AlarmEditorPanel.SelectActionDropDown(alarmAction);
        //    alarmManagerPage.AlarmEditorPanel.SelectTab("General");
        //    alarmManagerPage.AlarmEditorPanel.TickAutoAcknowledgeCheckbox(autoAckowledge);
        //    alarmManagerPage.AlarmEditorPanel.SelectRefreshRateDropDown(refreshRate);
        //    alarmManagerPage.AlarmEditorPanel.SelectTab("Trigger condition");
        //    alarmManagerPage.AlarmEditorPanel.EnterMessageInput(message);
        //    alarmManagerPage.AlarmEditorPanel.SelectControllersListDropDown(controller);
        //    alarmManagerPage.AlarmEditorPanel.SelectInputNameDropDown(inputNameDisplayed);
        //    alarmManagerPage.AlarmEditorPanel.SelectInputValueDropDown(inputValue);
        //    alarmManagerPage.AlarmEditorPanel.SelectTab("Actions");
        //    alarmManagerPage.AlarmEditorPanel.EnterFromInput(mailFrom);
        //    alarmManagerPage.AlarmEditorPanel.SelectToListDropDown(mailTo);
        //    alarmManagerPage.AlarmEditorPanel.EnterSubjectInput(mailSubject);
        //    alarmManagerPage.AlarmEditorPanel.EnterEmailMessageInput(mailContent);

        //    Step("7. Click Save");
        //    alarmManagerPage.AlarmEditorPanel.ClickSaveButton();
        //    alarmManagerPage.WaitForPreviousActionComplete();
        //    VerifyEqual(string.Format("7. Verify the newly created alarm is present in the grid ({0})", alarmName), true, alarmManagerPage.GridPanel.IsAlarmManagerGridHasTextPresent("Name", alarmName));                      

        //    Step("8. After 30 seconds or a bit longer, check alarms Alarms page and the email's inbox");
        //    Wait.ForAlarmTrigger();
        //    var alarmsPage = alarmManagerPage.AppBar.SwitchTo(App.Alarms) as AlarmsPage;
        //    alarmsPage.GridPanel.ClickReloadDataToolbarButton();
        //    alarmsPage.WaitForPreviousActionComplete();

        //    Step("9. Expected");
        //    Step(" - In Alarms page alarms with name specified at step #6 are present in the grid");
        //    VerifyEqual(string.Format("9. Verify alarm is present in grid ({0})", alarmName), true, alarmsPage.GridPanel.IsAlarmGridHasTextPresent("Name", alarmName));

        //    Step("10. Check the email");
        //    Step("11. Verify an email is sent with");
        //    Step(" - Subject: subject set up in Actions tab of Alarm Manager");
        //    Step(" - Contains: message set up in Actions tab of Alarm Manager");
        //    Step("   + Time: datetime when the simulated command sent with format: yyyy-MM-dd HH:mm:ss");
        //    Step("   + Controller's name: the name of controller");
        //    Step("   + Date and time of the I/O event triggering this alarm: datetime when the simulated command sent with format: yyyy-MM-dd HH:mm:ss");
        //    var foundEmail = GetEmailForAlarm(alarmName);
        //    var hasEmail = foundEmail != null;
        //    var curCtrlDateTime = Settings.GetServerTime();
        //    var alarmCreated = curCtrlDateTime.ToString("G");
        //    var emailCheckingDateTime = curCtrlDateTime.ToString("G");
        //    VerifyEqual(string.Format("11. Verify a sent alarm email is found in mailbox ({0}, Alarm created: {1}, Email checking: {2})", alarmName, alarmCreated, emailCheckingDateTime), true, hasEmail);
        //    if (hasEmail)
        //    {
        //        var arrBody = foundEmail.Body.SplitEx("|");
        //        var emailTimeET = arrBody[0].Trim();
        //        var emailController = arrBody[1].Trim();
        //        var emailTimeLT = arrBody[2].Trim();

        //        VerifyEqual(string.Format("11. Verify Time ({0}): datetime when the simulated command sent with format: yyyy-MM-dd HH:mm:ss", emailTimeET), true, Settings.CheckDateTimeMatchFormats(emailTimeET, "yyyy-MM-dd HH:mm:ss"));                
        //        VerifyEqual("11. Verify Controller Name: testing controller name", controller, emailController);
        //        VerifyEqual(string.Format("11. Verify Date and time of the I/O event triggering this alarm ({0}): datetime when the simulated command sent with format: yyyy-MM-dd HH:mm:ss", emailTimeLT), true, Settings.CheckDateTimeMatchFormats(emailTimeLT, "yyyy-MM-dd HH:mm:ss"));
        //        EmailUtility.CleanInbox(alarmName);
        //    }

        //    Step("12. Send the simulated command with valueName=DigitalOutput1 and value=OFF");
        //    Step("13. Expected The request is sent successfully, the response returns ok");                        
        //    var isRequestOk = SetValueToController(controller, inputNameId, "OFF", Settings.GetServerTime());
        //    VerifyEqual("13. Verify The request is sent successfully, the response returns ok", true, isRequestOk);

        //    Step("14. After 30 seconds or a bit longer, Reload grid");
        //    Wait.ForAlarmTrigger();
        //    alarmsPage.GridPanel.ClickReloadDataToolbarButton();
        //    alarmsPage.WaitForPreviousActionComplete();

        //    Step("15. Verify alarm is not presented in grid any longer because it has been auto-acknowledged");
        //    var isAlarmExisting = alarmsPage.GridPanel.IsAlarmGridHasTextPresent("Name", alarmName);
        //    VerifyEqual(string.Format("15. Verify alarm '{0}' is not present in grid any longer because it was auto-acknowledged", alarmName), false, isAlarmExisting);

        //    Step("16. Press the Red Bell icon");
        //    alarmsPage.GridPanel.ClickShowAllAlarmsToolbarOption();
        //    alarmsPage.WaitForPreviousActionComplete();
        //    alarmsPage.GridPanel.WaitForGridContentAvailable();            

        //    Step("17. Verify The testing alarm is updated");           
        //    Step(" - State: Green Checked icon");
        //    Step(" - Info: 'Auto Acknowledged'");
        //    Step(" - Last Change: time when acknowledged with format {M/d/yyyy h:mm:ss tt}");
        //    Step(" - User: 'auto'");
        //    Step(" - Trigger time: the time the alarm is trigger with format yyyy-MM-dd hh:mm:dd");
        //    Step(" - Trigger info: message in 'Trigger condition' tab of Alarm Manager app when alarm is created");
        //    var iconList1 = alarmsPage.GridPanel.GetListOfAlarmGridIconColumn(alarmName);
        //    VerifyEqual("17. Verify The Status is changed to Acknowledged (Green Check icon)", true, iconList1.All(p => p.Contains("status-ok.png")));
        //    var dtGridView = alarmsPage.GridPanel.BuildAlarmDataTable();
        //    var row = dtGridView.Select(string.Format("Name = '{0}'", alarmName)).FirstOrDefault();
        //    if (row != null)
        //    {
        //        var lastChangeDate = row["Last Change"].ToString();
        //        var triggerTime = row["Trigger Time"].ToString();
        //        VerifyEqual("17. Verify Info: 'Auto acknowledged'", "Auto acknowledged", row["Info"].ToString());                
        //        VerifyEqual(string.Format("17. Verify Last Change ({0}): time when acknowledged with format: M/d/yyyy h:mm:ss tt", lastChangeDate), true, Settings.CheckDateTimeMatchFormats(lastChangeDate, "M/d/yyyy h:mm:ss tt"));
        //        VerifyEqual("17. Verify User: the user who acknowledged the alarm", "auto", row["User"].ToString());
        //        VerifyEqual(string.Format("17. Verify Trigger Time ({0}): the time the alarm is trigger with format: M/d/yyyy h:mm:ss tt", triggerTime), true, Settings.CheckDateTimeMatchFormats(lastChangeDate, "M/d/yyyy h:mm:ss tt"));
        //        VerifyEqual("17. Verify Trigger Info: Alarm message", message, row["Trigger Info"].ToString());
        //    }
        //    else
        //    {
        //        Warning(string.Format("17. There is no row with alarm '{0}'", alarmName));
        //    }

        //    if (!alarmsPage.GridPanel.IsShowAllAlarmsOptionToggledOn())
        //    {
        //        alarmsPage.GridPanel.ClickShowAllAlarmsToolbarOption();
        //        alarmsPage.WaitForPreviousActionComplete();
        //    }

        //    Step("18. Go to Alarm Manager app and select the testing alarm, at Trigger Condition tab, update the Input value= 'OFF' save changes");
        //    alarmManagerPage = alarmsPage.AppBar.SwitchTo(App.AlarmManager) as AlarmManagerPage;
        //    alarmManagerPage.GridPanel.ClickGridRecord(alarmName);
        //    alarmManagerPage.WaitForPreviousActionComplete();
        //    alarmManagerPage.AlarmEditorPanel.SelectTab("Trigger condition");
        //    alarmManagerPage.AlarmEditorPanel.SelectInputValueDropDown("OFF");
        //    alarmManagerPage.AlarmEditorPanel.ClickSaveButton();
        //    alarmManagerPage.WaitForPreviousActionComplete();

        //    Step("19. After 30 seconds or a bit longer, go to Alarm page and reload the page");
        //    Wait.ForAlarmTrigger();
        //    alarmsPage = alarmManagerPage.AppBar.SwitchTo(App.Alarms) as AlarmsPage;
        //    alarmsPage.GridPanel.ClickReloadDataToolbarButton();
        //    alarmsPage.WaitForPreviousActionComplete();
        //    curCtrlDateTime = Settings.GetServerTime();

        //    Step("20 Expected");
        //    Step(" - In Alarms page alarms with name specified at step #6 are present in the grid");
        //    VerifyEqual(string.Format("20. Verify alarm is present in grid ({0})", alarmName), true, alarmsPage.GridPanel.IsAlarmGridHasTextPresent("Name", alarmName));

        //    Step("21. Check the email");
        //    Step("22. Verify an email is sent with");
        //    Step(" - Subject: subject set up in Actions tab of Alarm Manager");
        //    Step(" - Contains: message set up in Actions tab of Alarm Manager");
        //    Step("   + Time: datetime when the simulated command sent with format: yyyy-MM-dd HH:mm:ss");
        //    Step("   + Controller's name: the name of controller");
        //    Step("   + Date and time of the I/O event triggering this alarm: datetime when the simulated command sent with format: yyyy-MM-dd HH:mm:ss");
        //    foundEmail = GetEmailForAlarm(alarmName);
        //    hasEmail = foundEmail != null;
        //    emailCheckingDateTime = Settings.GetServerTime().ToString("G");
        //    VerifyEqual(string.Format("22. Verify a sent alarm email is found in mailbox ({0}, Alarm created: {1}, Email checking: {2})", alarmName, alarmCreated, emailCheckingDateTime), true, hasEmail);
        //    if (hasEmail)
        //    {
        //        var arrBody = foundEmail.Body.SplitEx("|");
        //        var emailTimeET = arrBody[0].Trim();
        //        var emailController = arrBody[1].Trim();
        //        var emailTimeLT = arrBody[2].Trim();

        //        var timeSpanET = curCtrlDateTime.Subtract(DateTime.Parse(emailTimeET));
        //        var timeSpanLT = curCtrlDateTime.Subtract(DateTime.Parse(emailTimeLT));
        //        VerifyEqual(string.Format("22. Verify Time ({0}): datetime when the simulated command sent with format: yyyy-MM-dd HH:mm:ss", emailTimeET), true, Settings.CheckDateTimeMatchFormats(emailTimeET, "yyyy-MM-dd HH:mm:ss"));                
        //        VerifyEqual("22. Verify Controller Name: testing controller name", controller, emailController);
        //        VerifyEqual(string.Format("22. Verify Date and time of the I/O event triggering this alarm ({0}): datetime when the simulated command sent with format: yyyy-MM-dd HH:mm:ss", emailTimeLT), true, Settings.CheckDateTimeMatchFormats(emailTimeLT, "yyyy-MM-dd HH:mm:ss"));
        //        EmailUtility.CleanInbox(alarmName);
        //    }

        //    Step("23. Send the simulated command with valueName=DigitalOutput1 and value=OFF");            
        //    isRequestOk = SetValueToController(controller, inputNameId, "ON", Settings.GetServerTime());
        //    VerifyEqual("23. Verify The request is sent successfully, the response returns ok", true, isRequestOk);

        //    Step("24. After 30 seconds or a bit longer, Reload grid");
        //    Wait.ForAlarmTrigger();
        //    alarmsPage.GridPanel.ClickReloadDataToolbarButton();
        //    alarmsPage.WaitForPreviousActionComplete();            

        //    Step("25. Verify alarm is not presented in grid any longer because it has been auto-acknowledged");
        //    isAlarmExisting = alarmsPage.GridPanel.IsAlarmGridHasTextPresent("Name", alarmName);
        //    VerifyEqual(string.Format("25. Verify alarm '{0}' is not present in grid any longer because it was auto-acknowledged", alarmName), false, isAlarmExisting);

        //    Step("26. Press the Red Bell icon");
        //    alarmsPage.GridPanel.ClickShowAllAlarmsToolbarOption();
        //    alarmsPage.WaitForPreviousActionComplete();
        //    alarmsPage.GridPanel.WaitForGridContentAvailable();

        //    Step("27. Verify The testing alarm is updated");
        //    Step(" - State: Green Checked icon");
        //    Step(" - Info: 'Auto acknowledged'");
        //    Step(" - Last Change: time when acknowledged with format {M/d/yyyy h:mm:ss tt}");
        //    Step(" - User: 'auto'");
        //    Step(" - Trigger Time: the time the alarm is trigger with format {M/d/yyyy h:mm:ss tt}");
        //    Step(" - Trigger Info: message in 'Trigger condition' tab of Alarm Manager app when alarm is created");
        //    iconList1 = alarmsPage.GridPanel.GetListOfAlarmGridIconColumn(alarmName);
        //    VerifyEqual("27. Verify The Status is changed to Acknowledged (Green Check icon)", true, iconList1.All(p => p.Contains("status-ok.png")));
        //    dtGridView = alarmsPage.GridPanel.BuildAlarmDataTable();
        //    row = dtGridView.Select(string.Format("Name = '{0}'", alarmName)).FirstOrDefault();
        //    if (row != null)
        //    {
        //        var lastChangeDate = row["Last Change"].ToString();
        //        var triggerTime = row["Trigger Time"].ToString();
        //        VerifyEqual("27. Verify Info: 'Auto acknowledged'", "Auto acknowledged", row["Info"].ToString());
        //        VerifyEqual(string.Format("27. Verify Last Change ({0}): time when acknowledged with format: M/d/yyyy h:mm:ss tt", lastChangeDate), true, Settings.CheckDateTimeMatchFormats(lastChangeDate, "M/d/yyyy h:mm:ss tt"));
        //        VerifyEqual("27. Verify User: the user who acknowledged the alarm", "auto", row["User"].ToString());
        //        VerifyEqual(string.Format("27. Verify Trigger Time ({0}): the time the alarm is trigger with format: M/d/yyyy h:mm:ss tt", triggerTime), true, Settings.CheckDateTimeMatchFormats(lastChangeDate, "M/d/yyyy h:mm:ss tt"));
        //        VerifyEqual("27. Verify Trigger Info: Alarm message", message, row["Trigger Info"].ToString());
        //    }
        //    else
        //    {
        //        Warning(string.Format("27. There is no row with alarm '{0}'", alarmName));
        //    }

        //    try
        //    {               
        //        alarmManagerPage = alarmsPage.AppBar.SwitchTo(App.AlarmManager) as AlarmManagerPage;
        //        alarmManagerPage.DeleteAlarm(alarmName, true);
        //        DeleteGeozone(geozone);
        //    }
        //    catch { }
        //}

        //[Test, DynamicRetry]
        //[Description("TS 4.22.1 - Alarm Manager - Controller alarm: comparison between two I/Os - Auto acknowledged")]
        //[NonParallelizable]
        //public void TS4_22_01()
        //{
        //    var testData = GetTestDataOfTestTS4_22_01();
        //    var geozone = SLVHelper.GenerateUniqueName("GZNTS42201");
        //    var controller = SLVHelper.GenerateUniqueName("CTRL");

        //    /* Basic info */
        //    var alarmName = testData["Alarm.name"] as string;
        //    var alarmType = testData["Alarm.type"] as string;
        //    var alarmAction = testData["Alarm.action"] as string;

        //    alarmName = SLVHelper.GenerateUniqueName(string.Format("[{0}][{1}][{2}] {3}", Browser.Name, Settings.Users["DefaultTest"].Username, TestContext.CurrentContext.Test.MethodName, alarmName));

        //    /* General tab */
        //    var autoAckowledge = Convert.ToBoolean(testData["Alarm.General.auto-acknowledge"]);
        //    var refreshRate = testData["Alarm.General.refresh-rate"] as string;
        //    var refreshRateRegex = Regex.Match(refreshRate, @"(\d*) (.*)");
        //    var refreshRateUnit = refreshRateRegex.Groups[2].Value;

        //    /* Trigger tab */
        //    var message = testData["Alarm.Trigger.message"] as string;
        //    var firstIO = testData["Alarm.Trigger.first-io"] as string;
        //    var secondIO = testData["Alarm.Trigger.second-io"] as string;
        //    var operatorToCompare = testData["Alarm.Trigger.operator"] as string;
        //    var ioValue = testData["Alarm.Trigger.io-value"] as string;
        //    var firstIORegex = Regex.Match(firstIO, @"(.*)#(.*)");
        //    var firstIOName = firstIORegex.Groups[1].Value;
        //    var firstIOId = firstIORegex.Groups[2].Value;
        //    var secondIORegex = Regex.Match(secondIO, @"(.*)#(.*)");
        //    var secondIOName = secondIORegex.Groups[1].Value;
        //    var secondIOId = secondIORegex.Groups[2].Value;

        //    /* Actions tab */
        //    var mailFrom = testData["Alarm.Actions.mail-from"] as string;
        //    var mailTo = testData["Alarm.Actions.mail-to"] as string;
        //    var mailSubject = alarmName;
        //    var mailContent = testData["Alarm.Actions.mail-content"] as string;

        //    Step("-> Create data for testing");
        //    DeleteGeozones("GZNTS42201*");
        //    CreateNewGeozone(geozone);
        //    CreateNewController(controller, geozone);

        //    var loginPage = Browser.OpenCMS();
        //    var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
        //    desktopPage.InstallAppsIfNotExist(App.AlarmManager, App.Alarms);

        //    Step("1. Go to Alarm Manager app");
        //    Step("2. Expected Alarm Manager page is routed and loaded successfully");
        //    var alarmManagerPage = desktopPage.GoToApp(App.AlarmManager) as AlarmManagerPage;

        //    Step("3. Select a geozone (should be configurable)");           
        //    alarmManagerPage.GeozoneTreeMainPanel.SelectNode(geozone);

        //    Step("4. Add an alarm");
        //    alarmManagerPage.GridPanel.ClickAddAlarmToolbarButton();
        //    alarmManagerPage.WaitForPreviousActionComplete();

        //    Step("5. Expected Add Alarm widget appears");
        //    VerifyEqual("5. Verify Alarm Details Panel appears", true, alarmManagerPage.AlarmEditorPanel.IsPanelVisible());

        //    Step("6. Specify report parameters");
        //    Step(" o Name: Any {{date time span}}");
        //    Step(" o Type: Controller alarm: comparison between two I/Os");
        //    Step(" o Action: Notify by eMail");
        //    Step(" o General tab:");
        //    Step("  + Auto-acknowledge: checked");
        //    Step("  + Refresh rate: 30 seconds (should be configurable)");
        //    Step(" o Trigger condition tab:");
        //    Step("  + Message: Any");
        //    Step("  + Controllers: selected from the list (should be configurable)");
        //    Step("  + First IO: Digital Output 1 or selected from the list (should be configurable)");
        //    Step("  + Second IO: Digital Output 2 or selected from the list (should be configurable)");
        //    Step("  + Operator: equal to");
        //    Step(" o Actions tab:");
        //    Step("  + From: qa@streetlightmonitoring.com");
        //    Step("  + To: Any valid mailbox");
        //    Step("  + Subject: Any");
        //    Step("  + Message:");
        //    Step("    * Time: ${ET}");
        //    Step("    * Date and time of the I/O event triggering this alarm: ${LT}");
        //    Step("    * Geozone where the alarm has been created in: ${GZ}");
        //    Step("    * Current server time: ${ST}");
        //    Step("    * Controller's name: ${CN}");            
        //    Step("    * Controller ID: ${CI}");
        //    Step("    * First IO: ${IO1IVV}");
        //    Step("    * Second IO: ${IO2IVV}");
        //    Step("    * Note: IO1IVV represents for First IO. IO2IVV represents for Second IO. They represent the label of the I/O that the user has chosen in the alarm definition, so they can be any of:");
        //    Step("      - Input 1 - Label");
        //    Step("      - Output 1 - Label");
        //    Step("      - Input 2 - Label");
        //    Step("      - Output 2 - Label");
        //    Step("      - For instance, when creating an alarm, First IO in Trigger Condition tab is set: Digital Input 1, Second IO in Trigger Condition tab is set: Digital Input 2. The values of IO1IVV & IO2IVV are Input 1 - Label and Input 2 - Label in Input and Output tab of the testing controller.");
        //    /* Basic info */
        //    alarmManagerPage.AlarmEditorPanel.EnterNameInput(alarmName);
        //    alarmManagerPage.AlarmEditorPanel.SelectTypeDropDown(alarmType);
        //    alarmManagerPage.WaitForPreviousActionComplete();
        //    alarmManagerPage.AlarmEditorPanel.SelectActionDropDown(alarmAction);

        //    /* General tab */
        //    // General tab is active by default
        //    alarmManagerPage.AlarmEditorPanel.SelectTab("General");
        //    alarmManagerPage.AlarmEditorPanel.TickAutoAcknowledgeCheckbox(autoAckowledge);
        //    alarmManagerPage.AlarmEditorPanel.SelectRefreshRateDropDown(refreshRate);

        //    /* Trigger tab */
        //    alarmManagerPage.AlarmEditorPanel.SelectTab("Trigger condition");
        //    alarmManagerPage.AlarmEditorPanel.EnterMessageInput(message);
        //    alarmManagerPage.AlarmEditorPanel.SelectControllersListDropDown(controller);
        //    alarmManagerPage.AlarmEditorPanel.SelectFirstIODropDown(firstIOName);
        //    alarmManagerPage.AlarmEditorPanel.SelectSecondIODropDown(secondIOName);
        //    alarmManagerPage.AlarmEditorPanel.SelectOperatorDropDown(operatorToCompare);

        //    /* Actions tab */
        //    alarmManagerPage.AlarmEditorPanel.SelectTab("Actions");
        //    alarmManagerPage.AlarmEditorPanel.EnterFromInput(mailFrom);
        //    alarmManagerPage.AlarmEditorPanel.SelectToListDropDown(mailTo);
        //    alarmManagerPage.AlarmEditorPanel.EnterSubjectInput(mailSubject);
        //    alarmManagerPage.AlarmEditorPanel.EnterEmailMessageInput(mailContent);

        //    Step("7. Click Save");
        //    alarmManagerPage.AlarmEditorPanel.ClickSaveButton();
        //    alarmManagerPage.WaitForPreviousActionComplete();
        //    VerifyEqual(string.Format("7. Verify the newly created alarm is present in the grid ({0})", alarmName), true, alarmManagerPage.GridPanel.IsAlarmManagerGridHasTextPresent("Name", alarmName));
        //    var curCtrlDateTime = Settings.GetServerTime();
        //    var alarmCreated = curCtrlDateTime.ToString("G");
            
        //    Info(@"8. Send these 2 requests
        //               - /api/loggingmanagement/setDeviceValues?controllerStrId={{controller id}}&idOnController={{device id}}&valueName=DigitalOutput1&value=**ON**&doLog=true&eventTime={{current date time stamp}}
        //               - /api/loggingmanagement/setDeviceValues?controllerStrId={{controller id}}&idOnController={{device id}}&valueName=DigitalOutput2&value=**ON**&doLog=true&eventTime={{current date time stamp}}");            
        //    SetValueToController(controller, firstIOId, ioValue, curCtrlDateTime);
        //    SetValueToController(controller, secondIOId, ioValue, curCtrlDateTime);

        //    Step("9. After 30 seconds or a bit longer, check alarms Alarms page");
        //    Wait.ForAlarmTrigger();
        //    var alarmsPage = alarmManagerPage.AppBar.SwitchTo(App.Alarms) as AlarmsPage;
        //    alarmsPage.GridPanel.ClickReloadDataToolbarButton();
        //    alarmsPage.WaitForPreviousActionComplete();

        //    Step("10. Verify In Alarms page, an alarm is present in the grid with");
        //    Step(" - Name: Alarm name from creating the alarm in Alarm Manager app");
        //    Step(" - Geozone: testing geozone");
        //    Step(" - Priority: 0");
        //    Step(" - Generator: Alarm name + Controller's ID");
        //    Step(" - Create Date: the time the alarm is trigger with format {M/dd/yyyy hh:mm:ss tt}");
        //    Step(" - State: X red icon (active status)");
        //    Step(" - Last Change: equal to Creation Date");
        //    Step(" - User: -");
        //    Step(" - Info: message in 'Trigger condition' tab of Alarm Manager app when alarm is created");
        //    Step(" - Trigger Time: empty");
        //    Step(" - Trigger Info: empty");
        //    VerifyEqual(string.Format("10. Verify alarm is present in grid ({0})", alarmName), true, alarmsPage.GridPanel.IsAlarmGridHasTextPresent("Name", alarmName));
        //    var dtGridView = alarmManagerPage.GridPanel.BuildAlarmDataTable();
        //    var row = dtGridView.Select(string.Format("Name = '{0}'", alarmName)).FirstOrDefault();
        //    var lastChangeDate = "";
        //    if (row != null)
        //    {
        //        lastChangeDate = row["Last Change"].ToString();
        //        var generator = row["Generator"].ToString();
        //        var iconList = alarmManagerPage.GridPanel.GetListOfAlarmGridIconColumn(alarmName);
        //        VerifyEqual("10. Verify Geozone is " + geozone, geozone, row["Geozone"].ToString());
        //        VerifyEqual("10. Verify Priority: 0", "0", row["Priority"].ToString());                
        //        VerifyTrue("10. Verify Generator: Alarm name + Controller's ID", generator.Contains(alarmName) && generator.Contains(controller), "Alarm name + Controller's ID", generator);
        //        VerifyEqual(string.Format("10. Verify Last Change ({0}): time when acknowledged with format: M/d/yyyy h:mm:ss tt", lastChangeDate), true, Settings.CheckDateTimeMatchFormats(lastChangeDate, "M/d/yyyy h:mm:ss tt"));
        //        VerifyEqual("10. Verify State: X red icon (active status)", true, iconList.Count == 1 && iconList.Any(p => p.Contains("status-error.png")));
        //        VerifyEqual("10. Verify Last Change: equal to Creation Date", row["Creation Date"].ToString(), row["Last Change"].ToString());
        //        VerifyEqual("10. Verify User: -", "-", row["User"].ToString());
        //        VerifyEqual("10. Verify Info: message in 'Trigger condition' tab of Alarm Manager app when alarm is created", message, row["Info"].ToString());
        //        VerifyEqual("10. Verify Trigger Time: empty", "", row["Trigger Time"].ToString());
        //        VerifyEqual("10. Verify Trigger Info: empty", "", row["Trigger Info"].ToString());
        //    }
        //    else
        //    {
        //        Warning(string.Format("10. There is no row with alarm '{0}'", alarmName));
        //    }

        //    Step("11. Check the email");
        //    Step("12. Verify an email is sent with");
        //    Step(" - Subject: subject set up in Actions tab of Alarm Manager");
        //    Step(" - Contains: message set up in Actions tab of Alarm Manager");
        //    Step("   + Time: datetime with format yyyy-MM-dd HH:mm:ss}. Ex: 2018-04-19 02:48:00");
        //    Step("   + Date and time of the I/O event triggering this alarm: datetime with format {yyyy-MM-dd HH:mm:ss}. Note: it displays '?', maybe a bug ? !? ");
        //    Step("   + Geozone where the alarm has been created in: testing geozone");
        //    Step("   + Current Server Time: datetime with format {yyyy-MM-dd HH:mm:ss}. Ex: 2018-04-19 02:50:24");
        //    Step("   + Controller's name: testing controller's name");
        //    var foundEmail = GetEmailForAlarm(alarmName);
        //    var hasEmail = foundEmail != null;
        //    curCtrlDateTime = Settings.GetServerTime();
        //    alarmCreated = curCtrlDateTime.ToString("G");
        //    var emailCheckingDateTime = curCtrlDateTime.ToString("G");
        //    VerifyEqual(string.Format("12. Verify a sent alarm email is found in mailbox ({0}, Alarm created: {1}, Email checking: {2})", alarmName, alarmCreated, emailCheckingDateTime), true, hasEmail);           
        //    if (hasEmail)
        //    {
        //        var arrBody = foundEmail.Body.SplitEx("|");
        //        var emailTimeET = arrBody[0].Trim();               
        //        var emailTimeLT = arrBody[1].Trim();
        //        var emailGeozone = arrBody[2].Trim();
        //        var emailTimeST = arrBody[3].Trim();
        //        var emailController = arrBody[4].Trim();

        //        VerifyEqual(string.Format("12. Verify Time ({0}): datetime when the simulated command sent with format: yyyy-MM-dd HH:mm:ss", emailTimeET), true, Settings.CheckDateTimeMatchFormats(emailTimeET, "yyyy-MM-dd HH:mm:ss"));
        //        VerifyTrue(string.Format("12. Verify Date and time of the I/O event triggering this alarm ({0}): datetime with format yyyy-MM-dd HH:mm:ss", emailTimeLT), Settings.CheckDateTimeMatchFormats(emailTimeLT, "yyyy-MM-dd HH:mm:ss"), "datetime with format yyyy-MM-dd HH:mm:ss", emailTimeLT);
        //        VerifyEqual("12. Verify Geozone where the alarm has been created in: testing geozone", geozone, emailGeozone);
        //        VerifyEqual(string.Format("12. Verify Current Server Time ({0}): datetime with format yyyy-MM-dd HH:mm:ss. Ex: 2018-04-19 02:50:24", emailTimeST), true, Settings.CheckDateTimeMatchFormats(emailTimeST, "yyyy-MM-dd HH:mm:ss"));                
        //        VerifyEqual("12. Verify Controller Name: testing controller name", controller, emailController);
        //        EmailUtility.CleanInbox(alarmName);
        //    }

        //    Step(@"13. Send 1 request
        //               - /api/loggingmanagement/setDeviceValues?controllerStrId={{controller id}}&idOnController={{device id}}&valueName=DigitalOutput1&value=OFF&doLog=true&eventTime={{current date time stamp}}");            
        //    Step("14. Expected The request is sent successfully, the response returns ok");            
        //    var isRequestOk = SetValueToController(controller, firstIOId, "OFF", Settings.GetServerTime());
        //    VerifyEqual("14. Verify The request is sent successfully, the response returns ok", true, isRequestOk);

        //    Step("15. After 30 seconds or a bit longer, Reload grid");
        //    Wait.ForAlarmTrigger();
        //    alarmsPage.GridPanel.ClickReloadDataToolbarButton();
        //    alarmsPage.WaitForPreviousActionComplete();

        //    Step("16. Verify alarm is not present in grid any longer because it has been auto-acknowledged");      
        //    VerifyEqual(string.Format("16. Verify alarm '{0}' is not present in grid any longer because it was auto-acknowledged", alarmName), false, alarmsPage.GridPanel.IsAlarmGridHasTextPresent("Name", alarmName));

        //    Step("17. Go to Alarm manager and update the testing alarm with Operator: 'different from' and save changes, wait for 30 seconds or a bit longer, then go to Alarm page and reload grid.");
        //    operatorToCompare = "different from";
        //    alarmManagerPage = alarmsPage.AppBar.SwitchTo(App.AlarmManager) as AlarmManagerPage;
        //    alarmManagerPage.GridPanel.ClickGridRecord(alarmName);
        //    alarmManagerPage.WaitForPreviousActionComplete();
        //    alarmManagerPage.AlarmEditorPanel.SelectTab("Trigger condition");
        //    alarmManagerPage.AlarmEditorPanel.SelectOperatorDropDown(operatorToCompare);
        //    alarmManagerPage.AlarmEditorPanel.ClickSaveButton();
        //    alarmManagerPage.WaitForPreviousActionComplete();            
        //    Wait.ForAlarmTrigger();
        //    alarmsPage = alarmManagerPage.AppBar.SwitchTo(App.Alarms) as AlarmsPage;
        //    alarmsPage.GridPanel.ClickReloadDataToolbarButton();
        //    alarmsPage.WaitForPreviousActionComplete();
        //    curCtrlDateTime = Settings.GetServerTime();

        //    Step("18. Verify In Alarms page, an alarm is present in the grid with");
        //    Step(" - Name: Alarm name from creating the alarm in Alarm Manager app");
        //    Step(" - Geozone: testing geozone");
        //    Step(" - Priority: 0");
        //    Step(" - Generator: Alarm name + Controller's ID");
        //    Step(" - Create Date: Not changed");
        //    Step(" - State: X red icon (active status)");
        //    Step(" - User: -");
        //    Step(" - Info: message in 'Trigger condition' tab of Alarm Manager app when alarm is created");
        //    Step(" - Trigger Time: empty");
        //    Step(" - Trigger Info: empty");
        //    VerifyEqual(string.Format("18. Verify alarm is present in grid ({0})", alarmName), true, alarmsPage.GridPanel.IsAlarmGridHasTextPresent("Name", alarmName));
        //    dtGridView = alarmManagerPage.GridPanel.BuildAlarmDataTable();
        //    row = dtGridView.Select(string.Format("Name = '{0}'", alarmName)).FirstOrDefault();
        //    if (row != null)
        //    {               
        //        lastChangeDate = row["Last Change"].ToString();
        //        var generator = row["Generator"].ToString();
        //        var iconList = alarmManagerPage.GridPanel.GetListOfAlarmGridIconColumn(alarmName);
        //        VerifyEqual("18. Verify Geozone is " + geozone, geozone, row["Geozone"].ToString());
        //        VerifyEqual("18. Verify Priority: 0", "0", row["Priority"].ToString());                
        //        VerifyTrue("18. Verify Generator: Alarm name + Controller's ID", generator.Contains(alarmName) && generator.Contains(controller), "Alarm name + Controller's ID", generator);
        //        VerifyEqual(string.Format("18. Verify Last Change ({0}): time when acknowledged with format: M/d/yyyy h:mm:ss tt", lastChangeDate), true, Settings.CheckDateTimeMatchFormats(lastChangeDate, "M/d/yyyy h:mm:ss tt"));                
        //        VerifyEqual("18. Verify State: X red icon (active status)", true, iconList.Count == 1 && iconList.Any(p => p.Contains("status-error.png")));                
        //        VerifyEqual("18. Verify User: -", "-", row["User"].ToString());
        //        VerifyEqual("18. Verify Info: message in 'Trigger condition' tab of Alarm Manager app when alarm is created", message, row["Info"].ToString());
        //        VerifyEqual("18. Verify Trigger Time: empty", "", row["Trigger Time"].ToString());
        //        VerifyEqual("18. Verify Trigger Info: empty", "", row["Trigger Info"].ToString());             
        //    }
        //    else
        //    {
        //        Warning(string.Format("18. There is no row with alarm '{0}'", alarmName));
        //    }

        //    Step("19. Check the email");
        //    Step("20. Verify an email is sent with");
        //    Step(" - Subject: subject set up in Actions tab of Alarm Manager");
        //    Step(" - Contains: message set up in Actions tab of Alarm Manager");
        //    Step("   + Time: datetime with format yyyy-MM-dd HH:mm:ss}. Ex: 2018-04-19 02:48:00");
        //    Step("   + Date and time of the I/O event triggering this alarm: datetime with format {yyyy-MM-dd HH:mm:ss}. Note: it displays '?', maybe a bug ? !? ");
        //    Step("   + Geozone where the alarm has been created in: testing geozone");
        //    Step("   + Current Server Time: datetime with format {yyyy-MM-dd HH:mm:ss}. Ex: 2018-04-19 02:50:24, equal Last Change column value");
        //    Step("   + Controller's name: testing controller's name");
        //    foundEmail = GetEmailForAlarm(alarmName);
        //    hasEmail = foundEmail != null;
        //    curCtrlDateTime = Settings.GetServerTime();
        //    alarmCreated = curCtrlDateTime.ToString("G");
        //    emailCheckingDateTime = curCtrlDateTime.ToString("G");
        //    VerifyEqual(string.Format("20. Verify a sent alarm email is found in mailbox ({0}, Alarm created: {1}, Email checking: {2})", alarmName, alarmCreated, emailCheckingDateTime), true, hasEmail);
        //    if (hasEmail)
        //    {
        //        var arrBody = foundEmail.Body.SplitEx("|");
        //        var emailTimeET = arrBody[0].Trim();
        //        var emailTimeLT = arrBody[1].Trim();
        //        var emailGeozone = arrBody[2].Trim();
        //        var emailTimeST = arrBody[3].Trim();
        //        var emailController = arrBody[4].Trim();
        //        var serverDateTime = DateTime.Parse(emailTimeST);
        //        var lastChangeDateTime = DateTime.Parse(lastChangeDate);
        //        var timeSpanLastChange = serverDateTime.Subtract(lastChangeDateTime);

        //        VerifyEqual(string.Format("20. Verify Time ({0}): datetime when the simulated command sent with format: yyyy-MM-dd HH:mm:ss", emailTimeET), true, Settings.CheckDateTimeMatchFormats(emailTimeET, "yyyy-MM-dd HH:mm:ss"));
        //        VerifyTrue(string.Format("20. Verify Date and time of the I/O event triggering this alarm ({0}): datetime with format yyyy-MM-dd HH:mm:ss", emailTimeLT), Settings.CheckDateTimeMatchFormats(emailTimeLT, "yyyy-MM-dd HH:mm:ss"), "datetime with format yyyy-MM-dd HH:mm:ss", emailTimeLT);
        //        VerifyEqual("20. Verify Geozone where the alarm has been created in: testing geozone", geozone, emailGeozone);
        //        VerifyTrue(string.Format("20. Verify Current Server Time ({0}): datetime with format yyyy-MM-dd HH:mm:ss. Ex: 2018-04-19 02:50:24", emailTimeST), Settings.CheckDateTimeMatchFormats(emailTimeST, "yyyy-MM-dd HH:mm:ss") && Math.Abs(timeSpanLastChange.TotalMinutes) <= 2, lastChangeDate, emailTimeST);                
        //        VerifyEqual("20. Verify Controller Name: testing controller name", controller, emailController);
        //        EmailUtility.CleanInbox(alarmName);
        //    }

        //    Step(@"21. Send 1 request
        //               - /api/loggingmanagement/setDeviceValues?controllerStrId={{controller id}}&idOnController={{device id}}&valueName=DigitalOutput1&value=ON&doLog=true&eventTime={{current date time stamp}}");
        //    Step("22. Expected The request is sent successfully, the response returns ok");            
        //    isRequestOk = SetValueToController(controller, firstIOId, "ON", Settings.GetServerTime());
        //    VerifyEqual("22. Verify The request is sent successfully, the response returns ok", true, isRequestOk);

        //    Step("23. After 30 seconds or a bit longer, Reload grid");
        //    Wait.ForAlarmTrigger();
        //    alarmsPage.GridPanel.ClickReloadDataToolbarButton();
        //    alarmsPage.WaitForPreviousActionComplete();

        //    Step("24. Verify alarm is not present in grid any longer because it has been auto-acknowledged");        
        //    VerifyEqual(string.Format("24. Verify alarm '{0}' is not present in grid any longer because it was auto-acknowledged", alarmName), false, alarmsPage.GridPanel.IsAlarmGridHasTextPresent("Name", alarmName));

        //    Step("25. Deselect the Red Bell icon");
        //    alarmsPage.GridPanel.ClickShowAllAlarmsToolbarOption();
        //    alarmsPage.WaitForPreviousActionComplete();
        //    alarmsPage.GridPanel.WaitForGridContentAvailable();

        //    Step("26. Verify The testing alarm is updated");
        //    Step(" - State: Green Checked icon");
        //    Step(" - Info: 'Auto acknowledged'");
        //    Step(" - Last Change: time when acknowledged with format {M/d/yyyy h:mm:ss tt}");
        //    Step(" - User: 'auto'");
        //    Step(" - Trigger Time: the time the alarm is trigger with format {M/d/yyyy h:mm:ss tt}");
        //    Step(" - Trigger Info: message in 'Trigger condition' tab of Alarm Manager app when alarm is created");
        //    var iconList1 = alarmsPage.GridPanel.GetListOfAlarmGridIconColumn(alarmName);
        //    VerifyEqual("26. Verify The Status is changed to Acknowledged (Green Check icon)", true, iconList1.All(p => p.Contains("status-ok.png")));
        //    dtGridView = alarmsPage.GridPanel.BuildAlarmDataTable();
        //    row = dtGridView.Select(string.Format("Name = '{0}'", alarmName)).FirstOrDefault();
        //    if (row != null)
        //    {
        //        lastChangeDate = row["Last Change"].ToString();
        //        var triggerTime = row["Trigger Time"].ToString();
        //        var iconList = alarmsPage.GridPanel.GetListOfAlarmGridIconColumn(alarmName);
        //        VerifyEqual("26. Verify Info: 'Auto acknowledged'", "Auto acknowledged", row["Info"].ToString());
        //        VerifyEqual(string.Format("26. Verify Last Change ({0}): time when acknowledged with format: M/d/yyyy h:mm:ss tt", lastChangeDate), true, Settings.CheckDateTimeMatchFormats(lastChangeDate, "M/d/yyyy h:mm:ss tt"));
        //        VerifyEqual("26. Verify User: the user who acknowledged the alarm", "auto", row["User"].ToString());
        //        VerifyEqual(string.Format("26. Verify Trigger Time ({0}): the time the alarm is trigger with format: M/d/yyyy h:mm:ss tt", triggerTime), true, Settings.CheckDateTimeMatchFormats(lastChangeDate, "M/d/yyyy h:mm:ss tt"));
        //        VerifyEqual("26. Verify Trigger Info: Alarm message", message, row["Trigger Info"].ToString());
        //    }
        //    else
        //    {
        //        Warning(string.Format("26. There is no row with alarm '{0}'", alarmName));
        //    }

        //    try
        //    {                
        //        alarmManagerPage = alarmsPage.AppBar.SwitchTo(App.AlarmManager) as AlarmManagerPage;
        //        alarmManagerPage.DeleteAlarm(alarmName, true);
        //        DeleteGeozone(geozone);
        //    }
        //    catch { }
        //}        

        //[Test, DynamicRetry]
        //[Description("TS 4.23.1 - Alarm Manager - Controller alarm: state of the I/Os in the last hours - Auto acknowledged")]
        //[Ignore("Can't figure out why alarm is not raised")]
        //public void TS4_23_01()
        //{
        //    var testData = GetTestDataOfTestTS4_23_01();

        //    /* Basic info */
        //    var alarmName = testData["Alarm.name"] as string;
        //    var alarmType = testData["Alarm.type"] as string;
        //    var alarmAction = testData["Alarm.action"] as string;

        //    alarmName = SLVHelper.GenerateUniqueName(string.Format("[{0}][{1}][{2}] {3}", Browser.Name, Settings.Users["DefaultTest"].Username, TestContext.CurrentContext.Test.MethodName, alarmName));

        //    /* General tab */
        //    var autoAckowledge = Convert.ToBoolean(testData["Alarm.General.auto-acknowledge"]);
        //    var refreshRate = testData["Alarm.General.refresh-rate"] as string;
        //    var refreshRateRegex = Regex.Match(refreshRate, @"(\d*) (.*)");
        //    var refreshRateUnit = refreshRateRegex.Groups[2].Value;

        //    /* Trigger tab */
        //    var message = testData["Alarm.Trigger.message"] as string;
        //    var controllers = testData["Alarm.Trigger.controllers"] as string;
        //    var inputName = testData["Alarm.Trigger.input-name"] as string;
        //    var inputValue = testData["Alarm.Trigger.input-value"] as string;
        //    var checkHoursInterval = testData["Alarm.Trigger.check-hours-interval"] as string;

        //    var controllerRegex = Regex.Match(controllers, @"(.*)#(.*)");
        //    var controllerName = controllerRegex.Groups[1].Value;
        //    var controllerId = controllerRegex.Groups[2].Value;

        //    var inputNameReg = Regex.Match(inputName, @"(.*)#(.*)");
        //    var inputNameDisplayed = inputNameReg.Groups[1].Value;
        //    var inputNameId = inputNameReg.Groups[2].Value;

        //    var checkHoursIntervalRegex = Regex.Match(checkHoursInterval, @"(\d*)");
        //    var checkHoursIntervalAsNumber = Convert.ToInt32(checkHoursIntervalRegex.Groups[1].Value);

        //    /* Actions tab */
        //    var mailFrom = testData["Alarm.Actions.mail-from"] as string;
        //    var mailTo = testData["Alarm.Actions.mail-to"] as string;
        //    var mailSubject = alarmName;
        //    var mailContent = testData["Alarm.Actions.mail-content"] as string;
        //    var loginPage = Browser.OpenCMS();
        //    var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
        //    desktopPage.InstallAppsIfNotExist(App.AlarmManager, App.Alarms);

        //    Step("1. Go to Alarm Manager app");
        //    Step("2. Expected Alarm Manager page is routed and loaded successfully");
        //    var alarmManagerPage = desktopPage.GoToApp(App.AlarmManager) as AlarmManagerPage;

        //    Step("3. Select a geozone (should be configurable)");
        //    var geozone = testData["Geozone"] as string;
        //    alarmManagerPage.GeozoneTreeMainPanel.SelectNode(geozone);

        //    Step("4. Add an alarm");
        //    alarmManagerPage.GridPanel.ClickAddAlarmToolbarButton();
        //    alarmManagerPage.WaitForPreviousActionComplete();

        //    Step("5. Expected Add Alarm widget appears");
        //    VerifyEqual("Verify Alarm Details Panel appears", true, alarmManagerPage.AlarmEditorPanel.IsPanelVisible());

        //    Step("6. Specify report parameters");
        //    /* Basic info */
        //    alarmManagerPage.AlarmEditorPanel.EnterNameInput(alarmName);
        //    alarmManagerPage.AlarmEditorPanel.SelectTypeDropDown(alarmType);
        //    alarmManagerPage.WaitForPreviousActionComplete();
        //    alarmManagerPage.AlarmEditorPanel.SelectActionDropDown(alarmAction);

        //    /* General tab */
        //    // General tab is active by default
        //    alarmManagerPage.AlarmEditorPanel.SelectTab("General");
        //    alarmManagerPage.AlarmEditorPanel.TickAutoAcknowledgeCheckbox(autoAckowledge);
        //    alarmManagerPage.AlarmEditorPanel.SelectRefreshRateDropDown(refreshRate);

        //    /* Trigger tab */
        //    alarmManagerPage.AlarmEditorPanel.SelectTab("Trigger condition");
        //    alarmManagerPage.AlarmEditorPanel.EnterMessageInput(message);
        //    alarmManagerPage.AlarmEditorPanel.SelectControllersListDropDown(controllerName);
        //    alarmManagerPage.AlarmEditorPanel.SelectInputNameDropDown(inputNameDisplayed);
        //    alarmManagerPage.AlarmEditorPanel.SelectInputValueDropDown(inputValue);
        //    alarmManagerPage.AlarmEditorPanel.SelectCheckHoursIntervalDropDown(checkHoursInterval);

        //    /* Actions tab */
        //    alarmManagerPage.AlarmEditorPanel.SelectTab("Actions");
        //    alarmManagerPage.AlarmEditorPanel.EnterFromInput(mailFrom);
        //    alarmManagerPage.AlarmEditorPanel.SelectToListDropDown(mailTo);
        //    alarmManagerPage.AlarmEditorPanel.EnterSubjectInput(mailSubject);
        //    alarmManagerPage.AlarmEditorPanel.EnterEmailMessageInput(mailContent);

        //    Step("7. Click Save");
        //    alarmManagerPage.AlarmEditorPanel.ClickSaveButton();
        //    alarmManagerPage.WaitForPreviousActionComplete();
        //    VerifyEqual(string.Format("7. Verify the newly created alarm is present in the grid ({0})", alarmName), true, alarmManagerPage.GridPanel.IsAlarmManagerGridHasTextPresent("Name", alarmName));

        //    Step("8. Send this request /api/loggingmanagement/setDeviceValues?controllerStrId={{controller id}}&idOnController={{device id}}&valueName=DigitalOutput1&value=**ON**&doLog=true&eventTime={{current date time stamp - check hours interval + 1 minutes}}");
        //    Step("9. Expected The request is sent successfully, the response returns ok");
        //    // Send this request to make sure alarm is triggered
        //    var minutesToEscapeTimeFrame = 1;            
        //    var isRequestOk = SetValueToController(controllerId, inputNameId, inputValue, Settings.GetServerTime().AddHours(-checkHoursIntervalAsNumber).AddMinutes(minutesToEscapeTimeFrame));
        //    VerifyEqual("9. Verify The request is sent successfully, the response returns ok", true, isRequestOk);

        //    Step("10. After 30 seconds or a bit longer, check alarms Alarms page and the email's inbox");
        //    Step("11. Expected");
        //    Step(" • In Alarms page and in the mailbox, alarms with name specified at step #6 are present in the grid");

        //    // Verify email is sent and found
        //    var foundEmail = GetEmailForAlarm(alarmName);
        //    VerifyEqual(string.Format("11. Verify a sent alarm email is found in mailbox ({0})", alarmName), true, foundEmail != null);
        //    EmailUtility.CleanInbox(alarmName);

        //    // Verify alarm in Alarms page
        //    desktopPage = Browser.RefreshLoggedInCMS();
        //    var alarmsPage = desktopPage.GoToApp(App.Alarms) as AlarmsPage;
        //    VerifyEqual(string.Format("11. Verify alarm is present in grid ({0})", alarmName), true, alarmsPage.GridPanel.IsAlarmGridHasTextPresent("Name", alarmName));            
        //    Wait.ForMinutes(minutesToEscapeTimeFrame + 1);
        //    Wait.ForAlarmTrigger();

        //    Step("12. Reload grid");
        //    alarmsPage.GridPanel.ClickReloadDataToolbarButton();
        //    alarmsPage.WaitForPreviousActionComplete();

        //    Step("13. Expected The alarm is not present in the grid any more because it was auto-acknowledged");
        //    alarmsPage.GridPanel.WaitForGridContentAvailable();
        //    var isAlarmExisting = alarmsPage.GridPanel.IsAlarmGridHasTextPresent("Name", alarmName);
        //    VerifyEqual(string.Format("13. Verify alarm '{0}' is not present in grid any longer because it was auto-acknowledged", alarmName), false, isAlarmExisting);
        //    try
        //    {
        //        if (!isAlarmExisting)
        //        {
        //            alarmManagerPage = alarmsPage.AppBar.SwitchTo(App.AlarmManager) as AlarmManagerPage;
        //            alarmManagerPage.DeleteAlarm(alarmName, true);
        //        }
        //    }
        //    catch { }
        //}

        //[Test, DynamicRetry]
        //[Description("TS 4.24.1	Alarm Manager - Meter alarm: comparison to a trigger - Auto acknowledged")]
        //[NonParallelizable]
        //public void TS4_24_01()
        //{
        //    var testData = GetTestDataOfTestTS4_24_01();
        //    var geozone = SLVHelper.GenerateUniqueName("GZNTS42401");
        //    var controller = SLVHelper.GenerateUniqueName("CTRL");
        //    var meter = SLVHelper.GenerateUniqueName("MTR");

        //    /* Basic info */
        //    var alarmType = testData["Alarm.type"].ToString();
        //    var alarmAction = testData["Alarm.action"].ToString();
        //    var alarmName = SLVHelper.GenerateUniqueName(string.Format("[{0}][{1}][{2}] {3}", Browser.Name, Settings.Users["DefaultTest"].Username, TestContext.CurrentContext.Test.MethodName, alarmType));

        //    /* General tab */
        //    var autoAckowledge = Convert.ToBoolean(testData["Alarm.General.auto-acknowledge"]);
        //    var refreshRate = testData["Alarm.General.refresh-rate"].ToString();
        //    var refreshRateRegex = Regex.Match(refreshRate, @"(\d*) (.*)");
        //    var refreshRateUnit = refreshRateRegex.Groups[2].Value;

        //    /* Trigger tab */
        //    var message = testData["Alarm.Trigger.message"].ToString();
        //    var metering = testData["Alarm.Trigger.metering"].ToString();
        //    var ignoreOperator = testData["Alarm.Trigger.ignore-operator"].ToString();
        //    var ignoreValue = testData["Alarm.Trigger.ignore-value"].ToString();
        //    var triggeringOperator = testData["Alarm.Trigger.triggering-operator"].ToString();
        //    var triggeringValue = testData["Alarm.Trigger.triggering-value"].ToString();
        //    var t1 = testData["Alarm.Trigger.t1"].ToString();
        //    var t2 = testData["Alarm.Trigger.t2"].ToString();
        //    var meteringInfoRegex = Regex.Match(metering, @"(.*)#(.*)");
        //    var meteringName = meteringInfoRegex.Groups[1].Value;
        //    var meteringId = meteringInfoRegex.Groups[2].Value;

        //    /* Actions tab */
        //    var mailFrom = testData["Alarm.Actions.mail-from"].ToString();
        //    var mailTo = testData["Alarm.Actions.mail-to"].ToString();
        //    var mailSubject = alarmName;
        //    var mailContent = testData["Alarm.Actions.mail-content"].ToString();
            
        //    Step("-> Create data for testing");
        //    DeleteGeozones("GZNTS42401*");
        //    CreateNewGeozone(geozone);
        //    CreateNewController(controller, geozone);            
        //    SetValueToController(controller, "TimeZoneId", "UTC", Settings.GetServerTime());
        //    CreateNewDevice(DeviceType.ElectricalCounter, meter, controller, geozone);
           
        //    var loginPage = Browser.OpenCMS();
        //    var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
        //    desktopPage.InstallAppsIfNotExist(App.AlarmManager, App.Alarms);

        //    Step("1. Go to Alarm Manager app");
        //    Step("2. Expected Alarm Manager page is routed and loaded successfully");
        //    var alarmManagerPage = desktopPage.GoToApp(App.AlarmManager) as AlarmManagerPage;

        //    Step("3. Select a geozone (should be configurable)");  
        //    alarmManagerPage.GeozoneTreeMainPanel.SelectNode(geozone);

        //    Step("4. Add an alarm");
        //    alarmManagerPage.GridPanel.ClickAddAlarmToolbarButton();
        //    alarmManagerPage.WaitForPreviousActionComplete();

        //    Step("5. Expected Add Alarm widget appears");
        //    VerifyEqual("5. Verify Alarm Details Panel appears", true, alarmManagerPage.AlarmEditorPanel.IsPanelVisible());

        //    Step("6. Specify report parameters:");
        //    Step(" o Name: Any {{date time span}}");
        //    Step(" o Type: Meter alarm: comparison to a trigger");
        //    Step(" o Action: Notify by eMail");
        //    Step(" o General tab:");
        //    Step("  + Auto-acknowledge: checked");
        //    Step("  + Refresh rate: 30 seconds (should be configurable)");
        //    Step(" o Trigger condition tab:");
        //    Step("  + Message: Any");
        //    Step("  + Meters: selected from the list (should be configurable)");
        //    Step("  + Metering: Active power (W)");
        //    Step("  + Ignore operator: Lower");
        //    Step("  + Triggering operator: Greater or selected from the list (should be configurable)");
        //    Step("  + Triggering value: 21000 (should be configurable)");
        //    Step("  + Analysis time T1: 1 hour");
        //    Step("  + Alarm Time T2: 15 minutes");
        //    Step(" o Actions tab:");
        //    Step("  + From: qa@streetlightmonitoring.com");
        //    Step("  + To: Any valid mailbox");
        //    Step("  + Subject: Any");
        //    Step("  + Message:");
        //    Step("    + Time: ${ET}");
        //    Step("    + Meter: ${MN}");
        //    /* Basic info */
        //    alarmManagerPage.AlarmEditorPanel.EnterNameInput(alarmName);
        //    alarmManagerPage.AlarmEditorPanel.SelectTypeDropDown(alarmType);
        //    alarmManagerPage.WaitForPreviousActionComplete();
        //    alarmManagerPage.AlarmEditorPanel.SelectActionDropDown(alarmAction);

        //    /* General tab */
        //    // General tab is active by default
        //    alarmManagerPage.AlarmEditorPanel.SelectTab("General");
        //    alarmManagerPage.AlarmEditorPanel.TickAutoAcknowledgeCheckbox(autoAckowledge);
        //    alarmManagerPage.AlarmEditorPanel.SelectRefreshRateDropDown(refreshRate);

        //    /* Trigger tab */
        //    alarmManagerPage.AlarmEditorPanel.SelectTab("Trigger condition");
        //    alarmManagerPage.AlarmEditorPanel.EnterMessageInput(message);
        //    alarmManagerPage.AlarmEditorPanel.SelectMetersListDropDown(meter);
        //    alarmManagerPage.AlarmEditorPanel.SelectMeteringDropDown(meteringName);
        //    alarmManagerPage.AlarmEditorPanel.SelectIgnoreOperatorDropDown(ignoreOperator);
        //    alarmManagerPage.AlarmEditorPanel.EnterIgnoreValueNumericInput(ignoreValue);
        //    alarmManagerPage.AlarmEditorPanel.SelectTriggeringOperatorDropDown(triggeringOperator);
        //    alarmManagerPage.AlarmEditorPanel.EnterTriggeringValueNumericInput(triggeringValue);
        //    alarmManagerPage.AlarmEditorPanel.SelectAnalysisTimeT1DropDown(t1);
        //    alarmManagerPage.AlarmEditorPanel.SelectAlarmTimeT1DropDown(t2);

        //    /* Actions tab */
        //    alarmManagerPage.AlarmEditorPanel.SelectTab("Actions");
        //    alarmManagerPage.AlarmEditorPanel.EnterFromInput(mailFrom);
        //    alarmManagerPage.AlarmEditorPanel.SelectToListDropDown(mailTo);
        //    alarmManagerPage.AlarmEditorPanel.EnterSubjectInput(mailSubject);
        //    alarmManagerPage.AlarmEditorPanel.EnterEmailMessageInput(mailContent);

        //    Step("7. Click Save");
        //    alarmManagerPage.AlarmEditorPanel.ClickSaveButton();
        //    alarmManagerPage.WaitForPreviousActionComplete();
        //    VerifyEqual(string.Format("7. Verify the newly created alarm is present in the grid ({0})", alarmName), true, alarmManagerPage.GridPanel.IsAlarmManagerGridHasTextPresent("Name", alarmName));
        //    var alarmCreated = Settings.GetServerTime().ToString("G");
        //    var curCtrlDateTime = Settings.GetCurrentControlerDateTime(controller);

        //    // Send this request to make sure alarm is triggered
        //    Step("8. Simulate the data of testing Metering (Ex: Active energy (KWh)) by sending 2 commands");
        //    Step(" o 1st Cmd: valueName= TotalKWHPositive; value= Ignore value; eventTime= Controller's current timezone's datetime - 30 minutes.");
        //    Step(" o 2nd Cmd: valueName= TotalKWHPositive; value= Ignore value; eventTime= Controller's current timezone's datetime - 36 minutes.");
        //    var request1 = SetValueToDevice(controller, meter, meteringId, triggeringValue, curCtrlDateTime.AddMinutes(30)); 
        //    var request2 = SetValueToDevice(controller, meter, meteringId, triggeringValue, curCtrlDateTime.AddMinutes(36));

        //    Step("9. Wait for 30s or a bit longer, then go to the testing geozone in Alarm app and press Refresh button");
        //    Wait.ForAlarmTrigger();
        //    var alarmsPage = alarmManagerPage.AppBar.SwitchTo(App.Alarms) as AlarmsPage;
        //    alarmsPage.GridPanel.ClickReloadDataToolbarButton();
        //    alarmsPage.WaitForPreviousActionComplete();

        //    Step("10. Verify In Alarms page, an alarm is present in the grid with");
        //    Step(" - Name: Alarm name from creating the alarm in Alarm Manager app");
        //    Step(" - Geozone: testing geozone");
        //    Step(" - Priority: 0");
        //    Step(" - Generator: Alarm name '-' random numbers");
        //    Step(" - Creation Date: the time the alarm is triggered with format {M/d/yyyy hh:mm:ss tt}, usually about 2 minutes after sending command.");
        //    Step(" - State: X red icon (active status)");
        //    Step(" - Last Change: equal to Creation Date");
        //    Step(" - User: -");
        //    Step(" - Info: message in 'Trigger condition' tab of Alarm Manager app when alarm is created");
        //    Step(" - Trigger Time: empty");
        //    Step(" - Trigger Info: empty");
        //    var dtGridView = alarmsPage.GridPanel.BuildAlarmDataTable();
        //    var row = dtGridView.Select(string.Format("Name = '{0}'", alarmName)).FirstOrDefault();
        //    if (row != null)
        //    {
        //        var creationDate = DateTime.Parse(row["Creation Date"].ToString());
        //        var timeSpan = curCtrlDateTime.Subtract(creationDate);
        //        var iconList = alarmsPage.GridPanel.GetListOfAlarmGridIconColumn(alarmName);
        //        VerifyEqual("10. Verify Geozone is " + geozone, geozone, row["Geozone"].ToString());
        //        VerifyEqual("10. Verify Priority: 0", "0", row["Priority"].ToString());
        //        VerifyTrue("10. Verify Generator: Alarm name '-' random numbers", row["Generator"].ToString().Contains(alarmName), alarmName, row["Generator"].ToString());
        //        VerifyTrue("10. Verify Creation Date: the time the alarm is triggered with format {M/d/yyyy hh:mm:ss tt}, usually about 2 minutes after sending command.", Math.Abs(timeSpan.TotalMinutes) <= 2, curCtrlDateTime, creationDate);
        //        VerifyEqual("10. Verify State: X red icon (active status)", true, iconList.Count == 1 && iconList.Any(p => p.Contains("status-error.png")));
        //        VerifyEqual("10. Verify Last Change: equal to Creation Date", row["Creation Date"].ToString(), row["Last Change"].ToString());
        //        VerifyEqual("10. Verify User: -", "-", row["User"].ToString());
        //        VerifyEqual("10. Verify Info: message in 'Trigger condition' tab of Alarm Manager app when alarm is created", message, row["Info"].ToString());
        //        VerifyEqual("10. Verify Trigger Time: empty", "", row["Trigger Time"].ToString());
        //        VerifyEqual("10. Verify Trigger Info: empty", "", row["Trigger Info"].ToString());
        //    }
        //    else
        //    {
        //        Warning(string.Format("10. There is no row with alarm '{0}'", alarmName));
        //    }

        //    Step("11. Check the email");
        //    Step("12. Verify an email is sent with");
        //    Step(" - Subject: subject set up in Actions tab of Alarm Manager");
        //    Step(" - Contains: message set up in Actions tab of Alarm Manager");
        //    Step("   + Time: Create Date with format yyyy-MM-dd HH:mm:ss. Ex: 2018-05-17 06:51:42");
        //    Step("   + Meter: testing Meter name. Ex: Meter01");
        //    var foundEmail = EmailUtility.GetNewEmail(alarmName);
        //    var isEmailSent = foundEmail != null;
        //    var emailCheckingDateTime = Settings.GetServerTime().ToString("G");
        //    VerifyEqual(string.Format("12. Verify a sent alarm email is found in mailbox ({0}, Alarm created: {1}, Email checking: {2})", alarmName, alarmCreated, emailCheckingDateTime), true, isEmailSent);
        //    if (isEmailSent)
        //    {
        //        var arrBody = foundEmail.Body.SplitEx("|");
        //        var emailTimeET = arrBody[0].Trim();
        //        var emailMeter = arrBody[1].Trim();

        //        VerifyEqual(string.Format("12. Verify Time ({0}): datetime when the simulated command sent with format: yyyy-MM-dd HH:mm:ss", emailTimeET), true, Settings.CheckDateTimeMatchFormats(emailTimeET, "yyyy-MM-dd HH:mm:ss"));
        //        VerifyEqual("12. Verify Meter Name: testing Meter name", meter, emailMeter);
        //        EmailUtility.CleanInbox(alarmName);
        //    }

        //    Step("13. Go to Alarm Manager page and update the Alarm Time T2: 5 minutes, then save changes. After 30 seconds or a bit longer, check alarms Alarms page");
        //    alarmManagerPage = alarmsPage.AppBar.SwitchTo(App.AlarmManager) as AlarmManagerPage;
        //    alarmManagerPage.GridPanel.ClickGridRecord(alarmName);
        //    alarmManagerPage.WaitForPreviousActionComplete();
        //    alarmManagerPage.AlarmEditorPanel.SelectTab("Trigger condition");
        //    alarmManagerPage.AlarmEditorPanel.SelectAlarmTimeT1DropDown("5 minutes");
        //    alarmManagerPage.AlarmEditorPanel.ClickSaveButton();
        //    alarmManagerPage.WaitForPreviousActionComplete();
        //    Wait.ForAlarmTrigger();
        //    alarmsPage = alarmManagerPage.AppBar.SwitchTo(App.Alarms) as AlarmsPage;
        //    alarmsPage.GridPanel.ClickReloadDataToolbarButton();
        //    alarmsPage.WaitForPreviousActionComplete();
        //    var acknowledgeTime = Settings.GetServerTime();

        //    Step("14. Verify alarm is not presented in grid any longer because it has been auto-acknowledged");
        //    VerifyEqual(string.Format("14. Verify alarm '{0}' is not present in grid any longer because it was auto-acknowledged", alarmName), false, alarmsPage.GridPanel.IsAlarmGridHasTextPresent("Name", alarmName));

        //    Step("15. Press the Red Bell icon");
        //    alarmsPage.GridPanel.ClickShowAllAlarmsToolbarOption();
        //    alarmsPage.WaitForPreviousActionComplete();
        //    alarmsPage.GridPanel.WaitForGridContentAvailable();

        //    Step("16. Verify The testing alarm is updated");
        //    Step(" - State: Green Checked icon");
        //    Step(" - Info: 'Auto acknowledged'");
        //    Step(" - Last Change: the datetime when alarm is acknowledged");
        //    Step(" - User: 'auto'");
        //    Step(" - Trigger Time: Creation Date");
        //    Step(" - Trigger Info: message in 'Trigger condition' tab of Alarm Manager app when alarm is created");
        //    var iconList1 = alarmsPage.GridPanel.GetListOfAlarmGridIconColumn(alarmName);
        //    VerifyEqual("16. Verify The Status is changed to Acknowledged (Green Check icon)", true, iconList1.All(p => p.Contains("status-ok.png")));
        //    dtGridView = alarmsPage.GridPanel.BuildAlarmDataTable();
        //    row = dtGridView.Select(string.Format("Name = '{0}'", alarmName)).FirstOrDefault();
        //    if (row != null)
        //    {
        //        var lastChangeDate = DateTime.Parse(row["Last Change"].ToString());
        //        var triggerTime = row["Trigger Time"].ToString();
        //        var timeSpan = acknowledgeTime.Subtract(lastChangeDate);
        //        var iconList = alarmsPage.GridPanel.GetListOfAlarmGridIconColumn(alarmName);
        //        VerifyEqual("16. Verify Info: 'Auto acknowledged'", "Auto acknowledged", row["Info"].ToString());
        //        VerifyTrue("16. Verify Last Change: the datetime when alarm is acknowledged", Math.Abs(timeSpan.TotalMinutes) <= 2, acknowledgeTime, lastChangeDate);
        //        VerifyEqual("16. Verify User: the user who acknowledged the alarm", "auto", row["User"].ToString());
        //        VerifyEqual("16. Verify Trigger Time: Creation Date", row["Creation Date"].ToString(), row["Trigger Time"].ToString());
        //        VerifyEqual("16. Verify Trigger Info: Alarm message", message, row["Trigger Info"].ToString());
        //    }
        //    else
        //    {
        //        Warning(string.Format("16. There is no row with alarm '{0}'", alarmName));
        //    }

        //    if (!alarmsPage.GridPanel.IsShowAllAlarmsOptionToggledOn())
        //    {
        //        alarmsPage.GridPanel.ClickShowAllAlarmsToolbarOption();
        //        alarmsPage.WaitForPreviousActionComplete();
        //    }

        //    Step("17. Go to Alarm Manager app and update the Alarm Time T2: 15 minutes; check 'New alarm if re-triggerd' and save changes");
        //    alarmManagerPage = alarmsPage.AppBar.SwitchTo(App.AlarmManager) as AlarmManagerPage;
        //    alarmManagerPage.GridPanel.ClickGridRecord(alarmName);
        //    alarmManagerPage.WaitForPreviousActionComplete();
        //    alarmManagerPage.AlarmEditorPanel.SelectTab("General");
        //    alarmManagerPage.AlarmEditorPanel.TickNewAlarmIfRetriggerCheckbox(true);
        //    alarmManagerPage.AlarmEditorPanel.SelectTab("Trigger condition");
        //    alarmManagerPage.AlarmEditorPanel.SelectAlarmTimeT1DropDown(t2);
        //    alarmManagerPage.AlarmEditorPanel.ClickSaveButton();
        //    alarmManagerPage.WaitForPreviousActionComplete();
        //    var alarmUpdated = Settings.GetServerTime().ToString("G");

        //    Step("18. Wait for 30s or a bit longer, then go to the testing geozone in Alarm app and press Refresh button");
        //    Wait.ForAlarmTrigger();
        //    alarmsPage = alarmManagerPage.AppBar.SwitchTo(App.Alarms) as AlarmsPage;
        //    alarmsPage.GridPanel.ClickReloadDataToolbarButton();
        //    alarmsPage.WaitForPreviousActionComplete();

        //    Step("19. Verify");
        //    Step(" - A new email sent");
        //    Step(" - An new alarm added to Alarm panel in Alarm app");
        //    VerifyEqual(string.Format("19. Verify new alarm '{0}' is added", alarmName), true, alarmsPage.GridPanel.IsAlarmGridHasTextPresent("Name", alarmName));
        //    foundEmail = GetEmailForAlarm(alarmName);
        //    isEmailSent = foundEmail != null;
        //    emailCheckingDateTime = Settings.GetServerTime().ToString("G");
        //    VerifyEqual(string.Format("19. Verify a sent alarm email is found in mailbox ({0}, Alarm created: {1}, Email checking: {2})", alarmName, alarmUpdated, emailCheckingDateTime), true, isEmailSent);
        //    if (isEmailSent) EmailUtility.CleanInbox(alarmName);

        //    Step("20. Press Red Bell icon to show all alarms");
        //    alarmsPage.GridPanel.ClickShowAllAlarmsToolbarOption();
        //    alarmsPage.WaitForPreviousActionComplete();
        //    alarmsPage.GridPanel.WaitForGridContentAvailable();

        //    Step("21. Verify 2 rows of the alarm in the Alarm panel");
        //    Step(" - 1st row with status: Active (Red X icon)");
        //    Step(" - 2nd row with status: Acknowledged (Green Check icon)");
        //    iconList1 = alarmsPage.GridPanel.GetListOfAlarmGridIconColumn(alarmName);
        //    VerifyEqual("21. Verify 2 rows of the alarm in the Alarm panel", 2, iconList1.Count);
        //    VerifyEqual("21. Verify 1st row with status: Active (Red X icon)", true, iconList1.Any(p => p.Contains("status-error.png")));
        //    VerifyEqual("21. Verify 2nd row with status: Acknowledged (Green Check icon)", true, iconList1.Any(p => p.Contains("status-ok.png")));

        //    Step("22. Select the row with status Active (Red X icon) and press Acknowledge button and enter the message to Acknowledge Alarms pop-up then press Send button and press OK");
        //    alarmsPage.GridPanel.ClickAlarmGridRecordHasErrorIcon(alarmName);
        //    alarmsPage.GridPanel.ClickAcknowledgeToolbarButton();
        //    alarmsPage.WaitForPopupDialogDisplayed();
        //    alarmsPage.Dialog.ClickSendButton();
        //    alarmsPage.Dialog.WaitForPopupMessageDisplayed();
        //    alarmsPage.Dialog.ClickOkButton();
        //    alarmsPage.WaitForPopupDialogDisappeared();
        //    acknowledgeTime = Settings.GetServerTime();

        //    Step("23. Verify The Status is changed to Acknowledged (Green Check icon)");
        //    iconList1 = alarmsPage.GridPanel.GetListOfAlarmGridIconColumn(alarmName);
        //    VerifyEqual("23. Verify The Status is changed to Acknowledged (Green Check icon)", true, iconList1.All(p => p.Contains("status-ok.png")));

        //    Step("24. Press Refresh button");
        //    alarmsPage.GridPanel.ClickReloadDataToolbarButton();
        //    alarmsPage.WaitForPreviousActionComplete();

        //    Step("25. Verify The row is updated with");
        //    Step(" - Last Change: the datetime when alarm is acknowledged");
        //    Step(" - User: the user who acknowledged the alarm");
        //    Step(" - Trigger Time: Creation Date");
        //    Step(" - Trigger Info: Alarm message");
        //    dtGridView = alarmsPage.GridPanel.BuildAlarmDataTable();
        //    row = dtGridView.Select(string.Format("Name = '{0}'", alarmName)).FirstOrDefault();
        //    if (row != null)
        //    {
        //        var lastChangeDate = DateTime.Parse(row["Last Change"].ToString());
        //        var timeSpan = acknowledgeTime.Subtract(lastChangeDate);
        //        var iconList = alarmsPage.GridPanel.GetListOfAlarmGridIconColumn(alarmName);
        //        VerifyTrue("25. Verify Last Change: the datetime when alarm is acknowledged", Math.Abs(timeSpan.TotalMinutes) <= 2, acknowledgeTime, lastChangeDate);
        //        VerifyEqual("25. Verify User: the user who acknowledged the alarm", Settings.Users["DefaultTest"].Username, row["User"].ToString());
        //        VerifyEqual("25. Verify Trigger Time: Creation Date", row["Creation Date"].ToString(), row["Trigger Time"].ToString());
        //        VerifyEqual("25. Verify Trigger Info: Alarm message", message, row["Trigger Info"].ToString());
        //    }
        //    else
        //    {
        //        Warning(string.Format("25. There is no row with alarm '{0}'", alarmName));
        //    }

        //    try
        //    {                
        //        alarmManagerPage = alarmsPage.AppBar.SwitchTo(App.AlarmManager) as AlarmManagerPage;
        //        alarmManagerPage.DeleteAlarm(alarmName);
        //        DeleteGeozone(geozone);
        //    }
        //    catch { }
        //}

        //[Test, DynamicRetry]
        //[Description("TS 4.25.1 Alarm Manager - Meter alarm- data analysis versus previous day - Auto acknowledged - Analytic mode - Average")]
        //[NonParallelizable]
        //public void TS4_25_01()
        //{
        //    var testData = GetTestDataOfTestTS4_25_01();
        //    var geozone = SLVHelper.GenerateUniqueName("GZNTS42501");
        //    var controller = SLVHelper.GenerateUniqueName("CTRL");
        //    var meter = SLVHelper.GenerateUniqueName("MTR");

        //    /* Basic info */
        //    var alarmType = testData["Alarm.type"].ToString();
        //    var alarmAction = testData["Alarm.action"].ToString();
        //    var alarmName = SLVHelper.GenerateUniqueName(string.Format("[{0}][{1}][{2}] {3}", Browser.Name, Settings.Users["DefaultTest"].Username, TestContext.CurrentContext.Test.MethodName, alarmType));

        //    /* General tab */
        //    var autoAckowledge = Convert.ToBoolean(testData["Alarm.General.auto-acknowledge"]);
        //    var refreshRate = testData["Alarm.General.refresh-rate"].ToString();
        //    var refreshRateRegex = Regex.Match(refreshRate, @"(\d*) (.*)");
        //    var refreshRateUnit = refreshRateRegex.Groups[2].Value;

        //    /* Trigger tab */
        //    var message = testData["Alarm.Trigger.message"].ToString();
        //    var metering = testData["Alarm.Trigger.metering"].ToString();
        //    var ignoreOperator = testData["Alarm.Trigger.ignore-operator"].ToString();
        //    var ignoreValue = testData["Alarm.Trigger.ignore-value"].ToString();
        //    var analysisPeriod = testData["Alarm.Trigger.analysis-period"].ToString();
        //    var analyticMode = testData["Alarm.Trigger.analytic-mode"].ToString();
        //    var percentageDifferenceTrigger = testData["Alarm.Trigger.percentage-difference-trigger"] as string;

        //    var meteringInfoRegex = Regex.Match(metering, @"(.*)#(.*)");
        //    var meteringName = meteringInfoRegex.Groups[1].Value;
        //    var meteringId = meteringInfoRegex.Groups[2].Value;

        //    /* Actions tab */
        //    var mailFrom = testData["Alarm.Actions.mail-from"].ToString();
        //    var mailTo = testData["Alarm.Actions.mail-to"].ToString();
        //    var mailSubject = alarmName;
        //    var mailContent = testData["Alarm.Actions.mail-content"].ToString();

        //    Step(" - User has logged in successfully");
        //    Step(" - Create a geozone containing a meter. This meter belongs to a controller which is has a certain timezone");
        //    Step(" - Simulate the previous day data by sending 2 commands for the property Frequency (Hz) with");
        //    Step("  + valueName: Frequency");
        //    Step("  + Value: 60");
        //    Step("  + For 1st Command:");         
        //    Step("    * evenTime: {Previous day} {current Controller datetime - 10 minute}");
        //    Step("  + For 2nd Command:");
        //    Step("    * evenTime: {Previous day} {current Controller datetime} Ex:");
        //    Step("    * Current controller time: 06:50:00 > {current controller time - 10 minute} = 06:40:00");
        //    Step("    * Today: 2018-05-08 > Previous day: 2018-05-07");
        //    Step("**** Precondition ****\n");

        //    Step("-> Create data for testing");
        //    DeleteGeozones("GZNTS42501*");
        //    CreateNewGeozone(geozone);
        //    CreateNewController(controller, geozone);
        //    SetValueToController(controller, "TimeZoneId", "UTC", Settings.GetServerTime());
        //    CreateNewDevice(DeviceType.ElectricalCounter, meter, controller, geozone);
            
        //    var curCtrlDateTime = Settings.GetCurrentControlerDateTime(controller);
        //    var previousBefore10m = curCtrlDateTime.AddDays(-1).AddMinutes(-10);
        //    var previousDay = curCtrlDateTime.AddDays(-1);
        //    var value = "60";
        //    var request = SetValueToDevice(controller, meter, meteringId, value, previousBefore10m);
        //    VerifyEqual(string.Format("-> Verify the request is sent successfully (attribute: {0}, value: {1}, time: {2})", meteringName, value, previousBefore10m), true, request);
        //    request = SetValueToDevice(controller, meter, meteringId, value, previousDay);
        //    VerifyEqual(string.Format("-> Verify the request is sent successfully (attribute: {0}, value: {1}, time: {2})", meteringName, value, previousDay), true, request);
            
        //    var loginPage = Browser.OpenCMS();
        //    var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
        //    desktopPage.InstallAppsIfNotExist(App.AlarmManager, App.Alarms);

        //    Step("1. Go to Alarm Manager app");
        //    Step("2. Expected Alarm Manager page is routed and loaded successfully");
        //    var alarmManagerPage = desktopPage.GoToApp(App.AlarmManager) as AlarmManagerPage;

        //    Step("3. Select a geozone (should be configurable)");
        //    alarmManagerPage.GeozoneTreeMainPanel.SelectNode(geozone);

        //    Step("4. Add an alarm");
        //    alarmManagerPage.GridPanel.ClickAddAlarmToolbarButton();
        //    alarmManagerPage.WaitForPreviousActionComplete();

        //    Step("5. Expected Add Alarm widget appears");
        //    VerifyEqual("5. Verify Alarm Details Panel appears", true, alarmManagerPage.AlarmEditorPanel.IsPanelVisible());

        //    Step("6. Specify report parameters:");
        //    Step(" - Name: Any {{date time span}}");
        //    Step(" - Type: Meter alarm: data analysis vs previous day (at fixed time)");
        //    Step(" - Action: Notify by eMail");
        //    Step(" - General tab:");
        //    Step("  + Auto-acknowledge: checked");
        //    Step("  + Refresh rate: 30 seconds");
        //    Step(" - Trigger condition tab:");
        //    Step("  + Message: Any");
        //    Step("  + Devices: selected from the testing streetlight");
        //    Step("  + Metering: Frequency (Hz)");
        //    Step("  + Ignore operator: Lower");
        //    Step("  + Ignore value: 50");
        //    Step("  + Analysis period: 1 hour");
        //    Step("  + Analytic mode: Average");
        //    Step("  + Percentage difference trigger: 10");
        //    Step(" - Actions tab:");
        //    Step("  + From: qa@streetlightmonitoring.com");
        //    Step("  + To: Any valid mailbox");
        //    Step("  + Subject: Any");
        //    Step("  + Message: Any");
        //    Step("  + Message:");
        //    Step("    * Time: ${ET}");
        //    Step("    * Meter Name: ${MN}");
        //    //Basic info
        //    alarmManagerPage.AlarmEditorPanel.EnterNameInput(alarmName);
        //    alarmManagerPage.AlarmEditorPanel.SelectTypeDropDown(alarmType);
        //    alarmManagerPage.WaitForPreviousActionComplete();
        //    alarmManagerPage.AlarmEditorPanel.SelectActionDropDown(alarmAction);

        //    //General tab
        //    //General tab is active by default
        //    alarmManagerPage.AlarmEditorPanel.SelectTab("General");
        //    alarmManagerPage.AlarmEditorPanel.TickAutoAcknowledgeCheckbox(autoAckowledge);
        //    alarmManagerPage.AlarmEditorPanel.SelectRefreshRateDropDown(refreshRate);

        //    //Trigger tab
        //    alarmManagerPage.AlarmEditorPanel.SelectTab("Trigger condition");
        //    alarmManagerPage.AlarmEditorPanel.EnterMessageInput(message);
        //    alarmManagerPage.AlarmEditorPanel.SelectMetersListDropDown(meter);
        //    alarmManagerPage.AlarmEditorPanel.SelectMeteringDropDown(meteringName);
        //    alarmManagerPage.AlarmEditorPanel.SelectIgnoreOperatorDropDown(ignoreOperator);
        //    alarmManagerPage.AlarmEditorPanel.EnterIgnoreValueNumericInput(ignoreValue);
        //    alarmManagerPage.AlarmEditorPanel.SelectAnalysisPeriodDropDown(analysisPeriod);
        //    alarmManagerPage.AlarmEditorPanel.SelectAnalyticModeDropDown(analyticMode);
        //    alarmManagerPage.AlarmEditorPanel.EnterPercentageDifferenceTriggerNumericInput(percentageDifferenceTrigger);

        //    //Actions tab
        //    alarmManagerPage.AlarmEditorPanel.SelectTab("Actions");
        //    alarmManagerPage.AlarmEditorPanel.EnterFromInput(mailFrom);
        //    alarmManagerPage.AlarmEditorPanel.SelectToListDropDown(mailTo);
        //    alarmManagerPage.AlarmEditorPanel.EnterSubjectInput(mailSubject);
        //    alarmManagerPage.AlarmEditorPanel.EnterEmailMessageInput(mailContent);

        //    Step("7. Click Save");
        //    alarmManagerPage.AlarmEditorPanel.ClickSaveButton();
        //    alarmManagerPage.WaitForPreviousActionComplete();
        //    var alarmCreated = Settings.GetServerTime().ToString("G");

        //    Step("8. Verify The new alarm is added into the grid of Alarm Manager");
        //    VerifyEqual(string.Format("8. Verify The new alarm is added into the grid of Alarm Manager ({0})", alarmName), true, alarmManagerPage.GridPanel.IsAlarmManagerGridHasTextPresent("Name", alarmName));

        //    Step("9. Simulate the today data by sending 2 commands for the property Frequency (Hz) with");
        //    Step(" - valueName: Frequency");
        //    Step(" - For 1st Command:");
        //    Step("  + evenTime: {current Controller datetime - 35 minute}");
        //    Step("  + Value: 50");
        //    Step(" - For 2nd Command:");
        //    Step("  + Value: 100");
        //    Step("  + evenTime: {current Controller datetime}");
        //    curCtrlDateTime = Settings.GetCurrentControlerDateTime(controller);
        //    var currentDateTimeBefore35m = curCtrlDateTime.AddMinutes(-35);
        //    var value1 = "50";
        //    var value2 = "100";
        //    request = SetValueToDevice(controller, meter, meteringId, value1, currentDateTimeBefore35m);
        //    VerifyEqual(string.Format("-> Verify the request is sent successfully (attribute: {0}, value: {1}, time: {2})", meteringName, value1, currentDateTimeBefore35m), true, request);
        //    request = SetValueToDevice(controller, meter, meteringId, value2, curCtrlDateTime);
        //    VerifyEqual(string.Format("-> Verify the request is sent successfully (attribute: {0}, value: {1}, time: {2})", meteringName, value2, curCtrlDateTime), true, request);

        //    Step("10. Wait for 30s or a bit longer, then go to the testing geozone in Alarm app and press Refresh button");
        //    Wait.ForAlarmTrigger();
        //    var alarmsPage = alarmManagerPage.AppBar.SwitchTo(App.Alarms) as AlarmsPage;
        //    alarmsPage.GridPanel.ClickReloadDataToolbarButton();
        //    alarmsPage.WaitForPreviousActionComplete();

        //    Step("11. Verify In Alarms page, an alarm is present in the grid with");
        //    Step(" - Name: Alarm name from creating the alarm in Alarm Manager app");
        //    Step(" - Geozone: testing geozone");
        //    Step(" - Priority: 0");
        //    Step(" - Generator: Alarm name '-' random numbers");
        //    Step(" - Creation Date: the time the alarm is triggered with format {M/d/yyyy hh:mm:ss tt}, usually about 2 minutes after sending command.");
        //    Step(" - State: X red icon (active status)");
        //    Step(" - Last Change: equal to Creation Date");
        //    Step(" - User: -");
        //    Step(" - Info: message in 'Trigger condition' tab of Alarm Manager app when alarm is created");
        //    Step(" - Trigger Time: empty");
        //    Step(" - Trigger Info: empty");
        //    var dtGridView = alarmsPage.GridPanel.BuildAlarmDataTable();
        //    var row = dtGridView.Select(string.Format("Name = '{0}'", alarmName)).FirstOrDefault();
        //    if (row != null)
        //    {
        //        var creationDate = DateTime.Parse(row["Creation Date"].ToString());
        //        var timeSpan = curCtrlDateTime.Subtract(creationDate);
        //        var iconList = alarmsPage.GridPanel.GetListOfAlarmGridIconColumn(alarmName);
        //        VerifyEqual("11. Verify Geozone is " + geozone, geozone, row["Geozone"].ToString());
        //        VerifyEqual("11. Verify Priority: 0", "0", row["Priority"].ToString());
        //        VerifyTrue("11. Verify Generator: Alarm name '-' random numbers", row["Generator"].ToString().Contains(alarmName), alarmName, row["Generator"].ToString());
        //        VerifyTrue("11. Verify Creation Date: the time the alarm is triggered with format {M/d/yyyy hh:mm:ss tt}, usually about 2 minutes after sending command.", Math.Abs(timeSpan.TotalMinutes) <= 2, curCtrlDateTime, creationDate);
        //        VerifyEqual("11. Verify State: X red icon (active status)", true, iconList.Count == 1 && iconList.Any(p => p.Contains("status-error.png")));
        //        VerifyEqual("11. Verify Last Change: equal to Creation Date", row["Creation Date"].ToString(), row["Last Change"].ToString());
        //        VerifyEqual("11. Verify User: -", "-", row["User"].ToString());
        //        VerifyEqual("11. Verify Info: message in 'Trigger condition' tab of Alarm Manager app when alarm is created", message, row["Info"].ToString());
        //        VerifyEqual("11. Verify Trigger Time: empty", "", row["Trigger Time"].ToString());
        //        VerifyEqual("11. Verify Trigger Info: empty", "", row["Trigger Info"].ToString());
        //    }
        //    else
        //    {
        //        Warning(string.Format("11. There is no row with alarm '{0}'", alarmName));
        //    }

        //    Step("12. Check the email");
        //    Step("13. Verify an email is sent with");
        //    Step(" - Subject: subject set up in Actions tab of Alarm Manager");
        //    Step(" - Contains: message set up in Actions tab of Alarm Manager");
        //    Step("   + Time: Create Date with format yyyy-MM-dd HH:mm:ss. Ex: 2018-05-17 06:51:42");
        //    Step("   + Meter: testing Meter name. Ex: Meter01");
        //    var foundEmail = EmailUtility.GetNewEmail(alarmName);
        //    var isEmailSent = foundEmail != null;
        //    var emailCheckingDateTime = Settings.GetServerTime().ToString("G");
        //    VerifyEqual(string.Format("13. Verify a sent alarm email is found in mailbox ({0}, Alarm created: {1}, Email checking: {2})", alarmName, alarmCreated, emailCheckingDateTime), true, isEmailSent);
        //    if (isEmailSent)
        //    {
        //        var arrBody = foundEmail.Body.SplitEx("|");
        //        var emailTimeET = arrBody[0].Trim();
        //        var emailMeter = arrBody[1].Trim();

        //        VerifyEqual(string.Format("13. Verify Time ({0}): datetime when the simulated command sent with format: yyyy-MM-dd HH:mm:ss", emailTimeET), true, Settings.CheckDateTimeMatchFormats(emailTimeET, "yyyy-MM-dd HH:mm:ss"));
        //        VerifyEqual("13. Verify Meter Name: testing Meter name", meter, emailMeter);
        //        EmailUtility.CleanInbox(alarmName);
        //    }

        //    Step("14. Simulate the today data by sending 2 commands for the property Frequency (Hz) with");
        //    Step(" - valueName: Frequency");
        //    Step(" - For 1st Command:");
        //    Step("  + evenTime: {current Controller datetime - 10 minute}");
        //    Step("  + Value: 50");
        //    Step(" - For 2nd Command:");
        //    Step("  + Value: 55");
        //    Step("  + evenTime: {current Controller datetime}");
        //    curCtrlDateTime = Settings.GetCurrentControlerDateTime(controller);
        //    var currentDateTimeBefore10m = curCtrlDateTime.AddMinutes(-10);
        //    value1 = "50";
        //    value2 = "55";
        //    request = SetValueToDevice(controller, meter, meteringId, value1, currentDateTimeBefore10m);
        //    VerifyEqual(string.Format("-> Verify the request is sent successfully (attribute: {0}, value: {1}, time: {2})", meteringName, value1, currentDateTimeBefore10m), true, request);
        //    request = SetValueToDevice(controller, meter, meteringId, value2, curCtrlDateTime);
        //    VerifyEqual(string.Format("-> Verify the request is sent successfully (attribute: {0}, value: {1}, time: {2})", meteringName, value2, curCtrlDateTime), true, request);

        //    Step("15. After 30 seconds or a bit longer, refresh Alarms page");
        //    Wait.ForAlarmTrigger();
        //    alarmsPage.GridPanel.ClickReloadDataToolbarButton();
        //    alarmsPage.WaitForPreviousActionComplete();
        //    var acknowledgeTime = Settings.GetServerTime();

        //    Step("16. Verify alarm is not presented in grid any longer because it has been auto-acknowledged");
        //    VerifyEqual(string.Format("16. Verify alarm '{0}' is not present in grid any longer because it was auto-acknowledged", alarmName), false, alarmsPage.GridPanel.IsAlarmGridHasTextPresent("Name", alarmName));

        //    Step("17. Press the Red Bell icon");
        //    alarmsPage.GridPanel.ClickShowAllAlarmsToolbarOption();
        //    alarmsPage.WaitForPreviousActionComplete();
        //    alarmsPage.GridPanel.WaitForGridContentAvailable();

        //    Step("18. Verify The testing alarm is updated");
        //    Step(" - State: Green Checked icon");
        //    Step(" - Info: 'Auto acknowledged'");
        //    Step(" - Last Change: the datetime when alarm is acknowledged");
        //    Step(" - User: 'auto'");
        //    Step(" - Trigger Time: Creation Date");
        //    Step(" - Trigger Info: message in 'Trigger condition' tab of Alarm Manager app when alarm is created");
        //    var iconList1 = alarmsPage.GridPanel.GetListOfAlarmGridIconColumn(alarmName);
        //    VerifyEqual("18. Verify The Status is changed to Acknowledged (Green Check icon)", true, iconList1.All(p => p.Contains("status-ok.png")));
        //    dtGridView = alarmsPage.GridPanel.BuildAlarmDataTable();
        //    row = dtGridView.Select(string.Format("Name = '{0}'", alarmName)).FirstOrDefault();
        //    if (row != null)
        //    {
        //        var lastChangeDate = DateTime.Parse(row["Last Change"].ToString());
        //        var triggerTime = row["Trigger Time"].ToString();
        //        var timeSpan = acknowledgeTime.Subtract(lastChangeDate);
        //        var iconList = alarmsPage.GridPanel.GetListOfAlarmGridIconColumn(alarmName);
        //        VerifyEqual("18. Verify Info: 'Auto acknowledged'", "Auto acknowledged", row["Info"].ToString());
        //        VerifyTrue("18. Verify Last Change: the datetime when alarm is acknowledged", Math.Abs(timeSpan.TotalMinutes) <= 2, acknowledgeTime, lastChangeDate);
        //        VerifyEqual("18. Verify User: the user who acknowledged the alarm", "auto", row["User"].ToString());
        //        VerifyEqual("18. Verify Trigger Time: Creation Date", row["Creation Date"].ToString(), row["Trigger Time"].ToString());
        //        VerifyEqual("18. Verify Trigger Info: Alarm message", message, row["Trigger Info"].ToString());
        //    }
        //    else
        //    {
        //        Warning(string.Format("18. There is no row with alarm '{0}'", alarmName));
        //    }

        //    if (!alarmsPage.GridPanel.IsShowAllAlarmsOptionToggledOn())
        //    {
        //        alarmsPage.GridPanel.ClickShowAllAlarmsToolbarOption();
        //        alarmsPage.WaitForPreviousActionComplete();
        //    }

        //    Step("19. Go to Alarm Manager app and check 'New alarm if re-triggerd' and save changes");
        //    alarmManagerPage = alarmsPage.AppBar.SwitchTo(App.AlarmManager) as AlarmManagerPage;
        //    alarmManagerPage.GridPanel.ClickGridRecord(alarmName);
        //    alarmManagerPage.WaitForPreviousActionComplete();
        //    alarmManagerPage.AlarmEditorPanel.SelectTab("General");
        //    alarmManagerPage.AlarmEditorPanel.TickNewAlarmIfRetriggerCheckbox(true);
        //    alarmManagerPage.AlarmEditorPanel.ClickSaveButton();
        //    alarmManagerPage.WaitForPreviousActionComplete();             
        //    curCtrlDateTime = Settings.GetCurrentControlerDateTime(controller);
        //    currentDateTimeBefore10m = curCtrlDateTime.AddMinutes(-10);
        //    value1 = "100";
        //    value2 = "110";
        //    request = SetValueToDevice(controller, meter, meteringId, value1, currentDateTimeBefore10m);
        //    VerifyEqual(string.Format("-> Verify the request is sent successfully (attribute: {0}, value: {1}, time: {2})", meteringName, value1, currentDateTimeBefore10m), true, request);
        //    request = SetValueToDevice(controller, meter, meteringId, value2, curCtrlDateTime);
        //    VerifyEqual(string.Format("-> Verify the request is sent successfully (attribute: {0}, value: {1}, time: {2})", meteringName, value2, curCtrlDateTime), true, request);
            
        //    Step("20. Wait for 30s or a bit longer, then go to the testing geozone in Alarm app and press Refresh button");
        //    Wait.ForAlarmTrigger();
        //    alarmsPage = alarmManagerPage.AppBar.SwitchTo(App.Alarms) as AlarmsPage;
        //    alarmsPage.GridPanel.ClickReloadDataToolbarButton();
        //    alarmsPage.WaitForPreviousActionComplete();

        //    Step("21. Verify");
        //    Step(" - A new email sent");
        //    Step(" - An new alarm added to Alarm panel in Alarm app");
        //    VerifyEqual(string.Format("21. Verify new alarm '{0}' is added", alarmName), true, alarmsPage.GridPanel.IsAlarmGridHasTextPresent("Name", alarmName));
        //    foundEmail = GetEmailForAlarm(alarmName);
        //    isEmailSent = foundEmail != null;
        //    emailCheckingDateTime = Settings.GetServerTime().ToString("G");
        //    VerifyEqual(string.Format("21. Verify a sent alarm email is found in mailbox ({0}, Alarm created: {1}, Email checking: {2})", alarmName, curCtrlDateTime.ToString("G"), emailCheckingDateTime), true, isEmailSent);
        //    if (isEmailSent) EmailUtility.CleanInbox(alarmName);

        //    Step("22. Press Red Bell icon to show all alarms");
        //    alarmsPage.GridPanel.ClickShowAllAlarmsToolbarOption();
        //    alarmsPage.WaitForPreviousActionComplete();
        //    alarmsPage.GridPanel.WaitForGridContentAvailable();

        //    Step("23. Verify 2 rows of the alarm in the Alarm panel");
        //    Step(" - 1st row with status: Active (Red X icon)");
        //    Step(" - 2nd row with status: Acknowledged (Green Check icon)");
        //    iconList1 = alarmsPage.GridPanel.GetListOfAlarmGridIconColumn(alarmName);
        //    VerifyEqual("23. Verify 2 rows of the alarm in the Alarm panel", 2, iconList1.Count);
        //    VerifyEqual("23. Verify 1st row with status: Active (Red X icon)", true, iconList1.Any(p => p.Contains("status-error.png")));
        //    VerifyEqual("23. Verify 2nd row with status: Acknowledged (Green Check icon)", true, iconList1.Any(p => p.Contains("status-ok.png")));

        //    Step("24. Select the row with status Active (Red X icon) and press Acknowledge button and enter the message to Acknowledge Alarms pop-up then press Send button and press OK");
        //    alarmsPage.GridPanel.ClickAlarmGridRecordHasErrorIcon(alarmName);
        //    alarmsPage.GridPanel.ClickAcknowledgeToolbarButton();
        //    alarmsPage.WaitForPopupDialogDisplayed();
        //    alarmsPage.Dialog.ClickSendButton();
        //    alarmsPage.Dialog.WaitForPopupMessageDisplayed();
        //    alarmsPage.Dialog.ClickOkButton();
        //    alarmsPage.WaitForPopupDialogDisappeared();
        //    acknowledgeTime = Settings.GetCurrentControlerDateTime(controller);

        //    Step("25. Verify The Status is changed to Acknowledged (Green Check icon)");
        //    iconList1 = alarmsPage.GridPanel.GetListOfAlarmGridIconColumn(alarmName);
        //    VerifyEqual("25. Verify The Status is changed to Acknowledged (Green Check icon)", true, iconList1.All(p => p.Contains("status-ok.png")));

        //    Step("26. Press Refresh button");
        //    alarmsPage.GridPanel.ClickReloadDataToolbarButton();
        //    alarmsPage.WaitForPreviousActionComplete();
        //    alarmsPage.GridPanel.WaitForGridContentAvailable();

        //    Step("27. Verify The row is updated with");
        //    Step(" - Last Change: the datetime when alarm is acknowledged");
        //    Step(" - User: the user who acknowledged the alarm");
        //    Step(" - Trigger Time: Creation Date");
        //    Step(" - Trigger Info: Alarm message");
        //    dtGridView = alarmsPage.GridPanel.BuildAlarmDataTable();
        //    row = dtGridView.Select(string.Format("Name = '{0}'", alarmName)).FirstOrDefault();
        //    if (row != null)
        //    {
        //        var lastChangeDate = DateTime.Parse(row["Last Change"].ToString());
        //        var timeSpan = acknowledgeTime.Subtract(lastChangeDate);
        //        var iconList = alarmsPage.GridPanel.GetListOfAlarmGridIconColumn(alarmName);
        //        VerifyTrue("27. Verify Last Change: the datetime when alarm is acknowledged", Math.Abs(timeSpan.TotalMinutes) <= 2, acknowledgeTime, lastChangeDate);
        //        VerifyEqual("27. Verify User: the user who acknowledged the alarm", Settings.Users["DefaultTest"].Username, row["User"].ToString());
        //        VerifyEqual("27. Verify Trigger Time: Creation Date", row["Creation Date"].ToString(), row["Trigger Time"].ToString());
        //        VerifyEqual("27. Verify Trigger Info: Alarm message", message, row["Trigger Info"].ToString());
        //    }
        //    else
        //    {
        //        Warning(string.Format("27. There is no row with alarm '{0}'", alarmName));
        //    }

        //    try
        //    {                
        //        alarmManagerPage = alarmsPage.AppBar.SwitchTo(App.AlarmManager) as AlarmManagerPage;
        //        alarmManagerPage.DeleteAlarm(alarmName);
        //        DeleteGeozone(geozone);
        //    }
        //    catch { }
        //}

        //[Test, DynamicRetry]
        //[Description("TS 4.25.2 Alarm Manager - Meter alarm- data analysis versus previous day (fixed time) - Auto acknowledged - Analytic mode - Average")]
        //public void TS4_25_02()
        //{
        //    var testData = GetTestDataOfTestTS4_25_02();
        //    var geozone = SLVHelper.GenerateUniqueName("GZNTS42502");
        //    var controller = SLVHelper.GenerateUniqueName("CTRL");
        //    var meter = SLVHelper.GenerateUniqueName("MTR");

        //    //Basic info
        //    var alarmType = testData["Alarm.type"].ToString();
        //    var alarmAction = testData["Alarm.action"].ToString();
        //    var alarmName = SLVHelper.GenerateUniqueName(string.Format("[{0}][{1}][{2}] {3}", Browser.Name, Settings.Users["DefaultTest"].Username, TestContext.CurrentContext.Test.MethodName, alarmType));

        //    // General
        //    var autoAckowledge = Convert.ToBoolean(testData["Alarm.General.auto-acknowledge"]);
        //    var refreshRate = testData["Alarm.General.refresh-rate"].ToString();
        //    var refreshRateRegex = Regex.Match(refreshRate, @"(\d*) (.*)");
        //    var refreshRateUnit = refreshRateRegex.Groups[2].Value;

        //    //Trigger
        //    var message = testData["Alarm.Trigger.message"].ToString();
        //    var metering = testData["Alarm.Trigger.metering"].ToString();
        //    var ignoreOperator = testData["Alarm.Trigger.ignore-operator"].ToString();
        //    var ignoreValue = testData["Alarm.Trigger.ignore-value"].ToString();
        //    var analyticMode = testData["Alarm.Trigger.analytic-mode"].ToString();
        //    var percentageDifferenceTrigger = testData["Alarm.Trigger.percentage-difference-trigger"].ToString();

        //    var meteringInfoRegex = Regex.Match(metering, @"(.*)#(.*)");
        //    var meteringName = meteringInfoRegex.Groups[1].Value;
        //    var meteringId = meteringInfoRegex.Groups[2].Value;

        //    //Actions
        //    var mailFrom = testData["Alarm.Actions.mail-from"].ToString();
        //    var mailTo = testData["Alarm.Actions.mail-to"].ToString();
        //    var mailSubject = alarmName;
        //    var mailContent = testData["Alarm.Actions.mail-content"].ToString();

        //    Step("**** Precondition ****");
        //    Step(" - User has logged in successfully");
        //    Step(" - Create a geozone containing a meter. This meter belongs to a controller which is has a certain timezone");
        //    Step(" - Simulate the previous day data by sending 2 commands for the property Active energy (KWh) with");
        //    Step("  + valueName: TotalKWHPositive");            
        //    Step("  + For 1st Command:");
        //    Step("    * Value: 50");
        //    Step("    * evenTime: {Previous day} {current controller time - 10 minute}");            
        //    Step("  + For 2nd Command:");
        //    Step("    * Value: 51");
        //    Step("    * evenTime: {Previous day} {current controller time} Ex:");
        //    Step("    * Current controller time: 06:50:00 > {current controller time - 10 minute} = 06:40:00");
        //    Step("    * Today: 2018-05-08 > Previous day: 2018-05-07");
        //    Step("**** Precondition ****\n");

        //    Step("-> Create data for testing");
        //    DeleteGeozones("GZNTS42502*");
        //    CreateNewGeozone(geozone);
        //    CreateNewController(controller, geozone);
        //    SetValueToController(controller, "TimeZoneId", "UTC", Settings.GetServerTime());
        //    CreateNewDevice(DeviceType.ElectricalCounter, meter, controller, geozone);

        //    var curCtrlDateTime = Settings.GetCurrentControlerDateTime(controller);
        //    var previousBefore10m = curCtrlDateTime.AddDays(-1).AddMinutes(-10);
        //    var previousDay = curCtrlDateTime.AddDays(-1);
        //    var value1 = "50";
        //    var value2 = "51";
        //    var request = SetValueToDevice(controller, meter, meteringId, value1, previousBefore10m);
        //    VerifyEqual(string.Format("-> Verify the request is sent successfully (attribute: {0}, value: {1}, time: {2})", meteringName, value1, previousBefore10m), true, request);
        //    request = SetValueToDevice(controller, meter, meteringId, value2, previousDay);
        //    VerifyEqual(string.Format("-> Verify the request is sent successfully (attribute: {0}, value: {1}, time: {2})", meteringName, value2, previousDay), true, request);

        //    var loginPage = Browser.OpenCMS();
        //    var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
        //    desktopPage.InstallAppsIfNotExist(App.AlarmManager, App.Alarms);

        //    Step("1. Go to Alarm Manager app");
        //    Step("2. Expected Alarm Manager page is routed and loaded successfully");
        //    var alarmManagerPage = desktopPage.GoToApp(App.AlarmManager) as AlarmManagerPage;

        //    Step("3. Select a geozone");
        //    alarmManagerPage.GeozoneTreeMainPanel.SelectNode(geozone);

        //    Step("4. Add an alarm");
        //    alarmManagerPage.GridPanel.ClickAddAlarmToolbarButton();
        //    alarmManagerPage.WaitForPreviousActionComplete();

        //    Step("5. Verify Add Alarm widget appears");
        //    VerifyEqual("5. Verify Alarm Details Panel appears", true, alarmManagerPage.AlarmEditorPanel.IsPanelVisible());

        //    Step("6. Specify report parameters");
        //    Step(" - Name: Any {{date time span}}");
        //    Step(" - Type: Meter alarm: data analysis vs previous day (at fixed time)");
        //    Step(" - Action: Notify by eMail");
        //    Step(" - General tab:");
        //    Step("  + Auto-acknowledge: checked");
        //    Step("  + Refresh rate: 30 seconds");
        //    Step(" - Trigger condition tab:");
        //    Step("  + Message: Any");
        //    Step("  + Devices: selected from the testing streetlight");
        //    Step("  + Metering: Active energy (KWh)");
        //    Step("  + Ignore operator: Lower");
        //    Step("  + Ignore value: 50");
        //    Step("  + Analysis period: 30 minutes");
        //    Step("  + Analytic mode: Average");
        //    Step("  + Percentage difference trigger: 10");
        //    Step("  + From: The current time - 2 hours");
        //    Step("  +  To: 23:59");           
        //    Step(" - Actions tab:");
        //    Step("  + From: qa@streetlightmonitoring.com");
        //    Step("  + To: Any valid mailbox");
        //    Step("  + Subject: Any");
        //    Step("  + Message: Any");
        //    Step("  + Message:");
        //    Step("    * Time: ${ET}");
        //    Step("    * Meter Name: ${MN}");
        //    //Basic info
        //    alarmManagerPage.AlarmEditorPanel.EnterNameInput(alarmName);
        //    alarmManagerPage.AlarmEditorPanel.SelectTypeDropDown(alarmType);
        //    alarmManagerPage.WaitForPreviousActionComplete();
        //    alarmManagerPage.AlarmEditorPanel.SelectActionDropDown(alarmAction);

        //    //General tab
        //    //General tab is active by default
        //    alarmManagerPage.AlarmEditorPanel.SelectTab("General");
        //    alarmManagerPage.AlarmEditorPanel.TickAutoAcknowledgeCheckbox(autoAckowledge);
        //    alarmManagerPage.AlarmEditorPanel.SelectRefreshRateDropDown(refreshRate);

        //    //Trigger tab
        //    alarmManagerPage.AlarmEditorPanel.SelectTab("Trigger condition");
        //    alarmManagerPage.AlarmEditorPanel.EnterMessageInput(message);
        //    alarmManagerPage.AlarmEditorPanel.SelectMetersListDropDown(meter);
        //    alarmManagerPage.AlarmEditorPanel.SelectMeteringDropDown(meteringName);
        //    alarmManagerPage.AlarmEditorPanel.SelectIgnoreOperatorDropDown(ignoreOperator);
        //    alarmManagerPage.AlarmEditorPanel.EnterIgnoreValueNumericInput(ignoreValue);
        //    alarmManagerPage.AlarmEditorPanel.SelectAnalyticModeDropDown(analyticMode);
        //    alarmManagerPage.AlarmEditorPanel.EnterPercentageDifferenceTriggerNumericInput(percentageDifferenceTrigger);
        //    curCtrlDateTime = Settings.GetCurrentControlerDateTime(controller);
        //    var fromTime = "00:00";
        //    if (curCtrlDateTime.Hour > 2) fromTime = curCtrlDateTime.AddHours(-2).ToString("HH:mm");
        //    alarmManagerPage.AlarmEditorPanel.EnterFromDaytimeInput(fromTime);
        //    alarmManagerPage.AlarmEditorPanel.EnterToDaytimeInput("23:59");

        //    //Actions tab
        //    alarmManagerPage.AlarmEditorPanel.SelectTab("Actions");
        //    alarmManagerPage.AlarmEditorPanel.EnterFromInput(mailFrom);
        //    alarmManagerPage.AlarmEditorPanel.SelectToListDropDown(mailTo);
        //    alarmManagerPage.AlarmEditorPanel.EnterSubjectInput(mailSubject);
        //    alarmManagerPage.AlarmEditorPanel.EnterEmailMessageInput(mailContent);

        //    Step("7. Click Save");
        //    alarmManagerPage.AlarmEditorPanel.ClickSaveButton();
        //    alarmManagerPage.WaitForPreviousActionComplete();
        //    var alarmCreated = Settings.GetServerTime().ToString("G");

        //    Step("8. Verify The new alarm is added into the grid of Alarm Manager");
        //    VerifyEqual(string.Format("8. Verify The new alarm is added into the grid of Alarm Manager ({0})", alarmName), true, alarmManagerPage.GridPanel.IsAlarmManagerGridHasTextPresent("Name", alarmName));

        //    Step("9. Simulate the today data by sending 2 commands for the property Active energy (KWh) with");
        //    Step(" - valueName: TotalKWHPositive");           
        //    Step(" - For 1st Command:");
        //    Step("  + evenTime: {current UTC datetime - 10 minute}");
        //    Step("  + Value: 60");
        //    Step(" - For 2nd Command:");
        //    Step("  + Value: 61");
        //    Step("  + evenTime: {current UTC datetime}");
        //    curCtrlDateTime = Settings.GetCurrentControlerDateTime(controller);
        //    var currentDateTimeBefore10m = curCtrlDateTime.AddMinutes(-10);
        //    value1 = "60";
        //    value2 = "61";
        //    request = SetValueToDevice(controller, meter, meteringId, value1, currentDateTimeBefore10m);
        //    VerifyEqual(string.Format("-> Verify the request is sent successfully (attribute: {0}, value: {1}, time: {2})", meteringName, value1, currentDateTimeBefore10m), true, request);
        //    request = SetValueToDevice(controller, meter, meteringId, value2, curCtrlDateTime);
        //    VerifyEqual(string.Format("-> Verify the request is sent successfully (attribute: {0}, value: {1}, time: {2})", meteringName, value2, curCtrlDateTime), true, request);

        //    Step("10. Wait for 30s or a bit longer, then go to the testing geozone in Alarm app and press Refresh button");
        //    Wait.ForAlarmTrigger();
        //    var alarmsPage = alarmManagerPage.AppBar.SwitchTo(App.Alarms) as AlarmsPage;
        //    alarmsPage.GridPanel.ClickReloadDataToolbarButton();
        //    alarmsPage.WaitForPreviousActionComplete();

        //    Step("11. Verify In Alarms page, an alarm is present in the grid with");
        //    Step(" - Name: Alarm name from creating the alarm in Alarm Manager app");
        //    Step(" - Geozone: testing geozone");
        //    Step(" - Priority: 0");
        //    Step(" - Generator: Alarm name '-' random numbers");
        //    Step(" - Creation Date: the time the alarm is triggered with format {M/d/yyyy hh:mm:ss tt}, usually about 2 minutes after sending command.");
        //    Step(" - State: X red icon (active status)");
        //    Step(" - Last Change: equal to Creation Date");
        //    Step(" - User: -");
        //    Step(" - Info: message in 'Trigger condition' tab of Alarm Manager app when alarm is created");
        //    Step(" - Trigger Time: empty");
        //    Step(" - Trigger Info: empty");
        //    var dtGridView = alarmsPage.GridPanel.BuildAlarmDataTable();
        //    var row = dtGridView.Select(string.Format("Name = '{0}'", alarmName)).FirstOrDefault();
        //    if (row != null)
        //    {
        //        var creationDate = DateTime.Parse(row["Creation Date"].ToString());
        //        var timeSpan = curCtrlDateTime.Subtract(creationDate);
        //        var iconList = alarmsPage.GridPanel.GetListOfAlarmGridIconColumn(alarmName);
        //        VerifyEqual("11. Verify Geozone is " + geozone, geozone, row["Geozone"].ToString());
        //        VerifyEqual("11. Verify Priority: 0", "0", row["Priority"].ToString());
        //        VerifyTrue("11. Verify Generator: Alarm name '-' random numbers", row["Generator"].ToString().Contains(alarmName), alarmName, row["Generator"].ToString());
        //        VerifyTrue("11. Verify Creation Date: the time the alarm is triggered with format {M/d/yyyy hh:mm:ss tt}, usually about 2 minutes after sending command.", Math.Abs(timeSpan.TotalMinutes) <= 2, curCtrlDateTime, creationDate);
        //        VerifyEqual("11. Verify State: X red icon (active status)", true, iconList.Count == 1 && iconList.Any(p => p.Contains("status-error.png")));
        //        VerifyEqual("11. Verify Last Change: equal to Creation Date", row["Creation Date"].ToString(), row["Last Change"].ToString());
        //        VerifyEqual("11. Verify User: -", "-", row["User"].ToString());
        //        VerifyEqual("11. Verify Info: message in 'Trigger condition' tab of Alarm Manager app when alarm is created", message, row["Info"].ToString());
        //        VerifyEqual("11. Verify Trigger Time: empty", "", row["Trigger Time"].ToString());
        //        VerifyEqual("11. Verify Trigger Info: empty", "", row["Trigger Info"].ToString());
        //    }
        //    else
        //    {
        //        Warning(string.Format("11. There is no row with alarm '{0}'", alarmName));
        //    }

        //    Step("12. Check the email");
        //    Step("13. Verify an email is sent with");
        //    Step(" - Subject: subject set up in Actions tab of Alarm Manager");
        //    Step(" - Contains: message set up in Actions tab of Alarm Manager");
        //    Step("   + Time: Create Date with format yyyy-MM-dd HH:mm:ss. Ex: 2018-05-17 06:51:42");
        //    Step("   + Meter: testing Meter name. Ex: Meter01");
        //    var foundEmail = EmailUtility.GetNewEmail(alarmName);
        //    var isEmailSent = foundEmail != null;
        //    var emailCheckingDateTime = Settings.GetServerTime().ToString("G");
        //    VerifyEqual(string.Format("13. Verify a sent alarm email is found in mailbox ({0}, Alarm created: {1}, Email checking: {2})", alarmName, alarmCreated, emailCheckingDateTime), true, isEmailSent);
        //    if (isEmailSent)
        //    {
        //        var arrBody = foundEmail.Body.SplitEx("|");
        //        var emailTimeET = arrBody[0].Trim();
        //        var emailMeter = arrBody[1].Trim();

        //        VerifyEqual(string.Format("13. Verify Time ({0}): datetime when the simulated command sent with format: yyyy-MM-dd HH:mm:ss", emailTimeET), true, Settings.CheckDateTimeMatchFormats(emailTimeET, "yyyy-MM-dd HH:mm:ss"));                
        //        VerifyEqual("13. Verify Meter Name: testing Meter name", meter, emailMeter);
        //        EmailUtility.CleanInbox(alarmName);
        //    }

        //    Step("14. Go to Alarm Manager page and update the Percentage difference trigger: 100 (%), then save changes. After 30 seconds or a bit longer, check alarms Alarms page");
        //    alarmManagerPage = alarmsPage.AppBar.SwitchTo(App.AlarmManager) as AlarmManagerPage;
        //    alarmManagerPage.GridPanel.ClickGridRecord(alarmName);
        //    alarmManagerPage.WaitForPreviousActionComplete();
        //    alarmManagerPage.AlarmEditorPanel.SelectTab("Trigger condition");
        //    alarmManagerPage.AlarmEditorPanel.EnterPercentageDifferenceTriggerNumericInput("100");
        //    alarmManagerPage.AlarmEditorPanel.ClickSaveButton();
        //    alarmManagerPage.WaitForPreviousActionComplete();
        //    Wait.ForAlarmTrigger();
        //    alarmsPage = alarmManagerPage.AppBar.SwitchTo(App.Alarms) as AlarmsPage;
        //    alarmsPage.GridPanel.ClickReloadDataToolbarButton();
        //    alarmsPage.WaitForPreviousActionComplete();
        //    var acknowledgeTime = Settings.GetServerTime();

        //    Step("15. Verify alarm is not presented in grid any longer because it has been auto-acknowledged");
        //    VerifyEqual(string.Format("15. Verify alarm '{0}' is not present in grid any longer because it was auto-acknowledged", alarmName), false, alarmsPage.GridPanel.IsAlarmGridHasTextPresent("Name", alarmName));

        //    Step("16. Press the Red Bell icon");
        //    alarmsPage.GridPanel.ClickShowAllAlarmsToolbarOption();
        //    alarmsPage.WaitForPreviousActionComplete();
        //    alarmsPage.GridPanel.WaitForGridContentAvailable();

        //    Step("17. Verify The testing alarm is updated");
        //    Step(" - State: Green Checked icon");
        //    Step(" - Info: 'Auto acknowledged'");
        //    Step(" - Last Change: the datetime when alarm is acknowledged");
        //    Step(" - User: 'auto'");
        //    Step(" - Trigger Time: Creation Date");
        //    Step(" - Trigger Info: message in 'Trigger condition' tab of Alarm Manager app when alarm is created");
        //    var iconList1 = alarmsPage.GridPanel.GetListOfAlarmGridIconColumn(alarmName);
        //    VerifyEqual("17. Verify The Status is changed to Acknowledged (Green Check icon)", true, iconList1.All(p => p.Contains("status-ok.png")));
        //    dtGridView = alarmsPage.GridPanel.BuildAlarmDataTable();
        //    row = dtGridView.Select(string.Format("Name = '{0}'", alarmName)).FirstOrDefault();
        //    if (row != null)
        //    {
        //        var lastChangeDate = DateTime.Parse(row["Last Change"].ToString());
        //        var triggerTime = row["Trigger Time"].ToString();
        //        var timeSpan = acknowledgeTime.Subtract(lastChangeDate);
        //        var iconList = alarmsPage.GridPanel.GetListOfAlarmGridIconColumn(alarmName);
        //        VerifyEqual("17. Verify Info: 'Auto acknowledged'", "Auto acknowledged", row["Info"].ToString());
        //        VerifyTrue("17. Verify Last Change: the datetime when alarm is acknowledged", Math.Abs(timeSpan.TotalMinutes) <= 2, acknowledgeTime, lastChangeDate);
        //        VerifyEqual("17. Verify User: the user who acknowledged the alarm", "auto", row["User"].ToString());
        //        VerifyEqual("17. Verify Trigger Time: Creation Date", row["Creation Date"].ToString(), row["Trigger Time"].ToString());
        //        VerifyEqual("17. Verify Trigger Info: Alarm message", message, row["Trigger Info"].ToString());                 
        //    }
        //    else
        //    {
        //        Warning(string.Format("17. There is no row with alarm '{0}'", alarmName));
        //    }

        //    if (!alarmsPage.GridPanel.IsShowAllAlarmsOptionToggledOn())
        //    {
        //        alarmsPage.GridPanel.ClickShowAllAlarmsToolbarOption();
        //        alarmsPage.WaitForPreviousActionComplete();
        //    }

        //    Step("18. Go to Alarm Manager app and update the Percentage difference trigger: 10 (%); check 'New alarm if re-triggerd' and save changes");
        //    alarmManagerPage = alarmsPage.AppBar.SwitchTo(App.AlarmManager) as AlarmManagerPage;
        //    alarmManagerPage.GridPanel.ClickGridRecord(alarmName);
        //    alarmManagerPage.WaitForPreviousActionComplete();
        //    alarmManagerPage.AlarmEditorPanel.SelectTab("General");
        //    alarmManagerPage.AlarmEditorPanel.TickNewAlarmIfRetriggerCheckbox(true);
        //    alarmManagerPage.AlarmEditorPanel.SelectTab("Trigger condition");
        //    alarmManagerPage.AlarmEditorPanel.EnterPercentageDifferenceTriggerNumericInput("10");
        //    alarmManagerPage.AlarmEditorPanel.ClickSaveButton();
        //    alarmManagerPage.WaitForPreviousActionComplete();
        //    var alarmUpdated = Settings.GetServerTime().ToString("G");

        //    Step("19. Wait for 30s or a bit longer, then go to the testing geozone in Alarm app and press Refresh button");
        //    Wait.ForAlarmTrigger();
        //    alarmsPage = alarmManagerPage.AppBar.SwitchTo(App.Alarms) as AlarmsPage;
        //    alarmsPage.GridPanel.ClickReloadDataToolbarButton();
        //    alarmsPage.WaitForPreviousActionComplete();
        //    alarmsPage.GridPanel.WaitForGridContentAvailable();

        //    Step("20. Verify");
        //    Step(" - A new email sent");
        //    Step(" - An new alarm added to Alarm panel in Alarm app");
        //    VerifyEqual(string.Format("20. Verify new alarm '{0}' is added", alarmName), true, alarmsPage.GridPanel.IsAlarmGridHasTextPresent("Name", alarmName));
        //    foundEmail = GetEmailForAlarm(alarmName);
        //    isEmailSent = foundEmail != null;
        //    emailCheckingDateTime = Settings.GetServerTime().ToString("G");
        //    VerifyEqual(string.Format("20. Verify a sent alarm email is found in mailbox ({0}, Alarm created: {1}, Email checking: {2})", alarmName, alarmUpdated, emailCheckingDateTime), true, isEmailSent);
        //    if (isEmailSent) EmailUtility.CleanInbox(alarmName);

        //    Step("21. Press Red Bell icon to show all alarms");
        //    alarmsPage.GridPanel.ClickShowAllAlarmsToolbarOption();
        //    alarmsPage.WaitForPreviousActionComplete();
        //    alarmsPage.GridPanel.WaitForGridContentAvailable();

        //    Step("22. Verify 2 rows of the alarm in the Alarm panel");
        //    Step(" - 1st row with status: Active (Red X icon)");
        //    Step(" - 2nd row with status: Acknowledged (Green Check icon)");
        //    iconList1 = alarmsPage.GridPanel.GetListOfAlarmGridIconColumn(alarmName);
        //    VerifyEqual("22. Verify 2 rows of the alarm in the Alarm panel", 2, iconList1.Count);
        //    VerifyEqual("22. Verify 1st row with status: Active (Red X icon)", true, iconList1.Any(p => p.Contains("status-error.png")));
        //    VerifyEqual("22. Verify 2nd row with status: Acknowledged (Green Check icon)", true, iconList1.Any(p => p.Contains("status-ok.png")));

        //    Step("23. Select the row with status Active (Red X icon) and press Acknowledge button and enter the message to Acknowledge Alarms pop-up then press Send button and press OK");
        //    alarmsPage.GridPanel.ClickAlarmGridRecordHasErrorIcon(alarmName);
        //    alarmsPage.GridPanel.ClickAcknowledgeToolbarButton();
        //    alarmsPage.WaitForPopupDialogDisplayed();
        //    alarmsPage.Dialog.ClickSendButton();
        //    alarmsPage.Dialog.WaitForPopupMessageDisplayed();
        //    alarmsPage.Dialog.ClickOkButton();
        //    alarmsPage.WaitForPopupDialogDisappeared();
        //    acknowledgeTime = Settings.GetServerTime();

        //    Step("24. Verify The Status is changed to Acknowledged (Green Check icon)");
        //    iconList1 = alarmsPage.GridPanel.GetListOfAlarmGridIconColumn(alarmName);
        //    VerifyEqual("24. Verify The Status is changed to Acknowledged (Green Check icon)", true, iconList1.All(p => p.Contains("status-ok.png")));

        //    Step("25. Press Refresh button");
        //    alarmsPage.GridPanel.ClickReloadDataToolbarButton();
        //    alarmsPage.WaitForPreviousActionComplete();

        //    Step("26. Verify The row is updated with");
        //    Step(" - Last Change: the datetime when alarm is acknowledged");
        //    Step(" - User: the user who acknowledged the alarm");
        //    Step(" - Trigger Time: Creation Date");
        //    Step(" - Trigger Info: Alarm message");
        //    dtGridView = alarmsPage.GridPanel.BuildAlarmDataTable();
        //    row = dtGridView.Select(string.Format("Name = '{0}'", alarmName)).FirstOrDefault();
        //    if (row != null)
        //    {
        //        var lastChangeDate = DateTime.Parse(row["Last Change"].ToString());
        //        var timeSpan = acknowledgeTime.Subtract(lastChangeDate);
        //        var iconList = alarmsPage.GridPanel.GetListOfAlarmGridIconColumn(alarmName);
        //        VerifyTrue("26. Verify Last Change: the datetime when alarm is acknowledged", Math.Abs(timeSpan.TotalMinutes) <= 2, acknowledgeTime, lastChangeDate);
        //        VerifyEqual("26. Verify User: the user who acknowledged the alarm", Settings.Users["DefaultTest"].Username, row["User"].ToString());
        //        VerifyEqual("26. Verify Trigger Time: Creation Date", row["Creation Date"].ToString(), row["Trigger Time"].ToString());
        //        VerifyEqual("26. Verify Trigger Info: Alarm message", message, row["Trigger Info"].ToString());
        //    }
        //    else
        //    {
        //        Warning(string.Format("26. There is no row with alarm '{0}'", alarmName));
        //    }

        //    try
        //    {                
        //        alarmManagerPage = alarmsPage.AppBar.SwitchTo(App.AlarmManager) as AlarmManagerPage;
        //        alarmManagerPage.DeleteAlarm(alarmName);
        //        DeleteGeozone(geozone);
        //    }
        //    catch { }
        //}

       #endregion //Comment out alarms because created all-in-one test case

       [Test, DynamicRetry]
       [Description("SLV-1275 - Alarms - Keep informations when alarms is acknowledged")]
       public void SLV_1275()
       {
           var testData = GetTestDataOfTestSLV_1275();
           var alarm = testData["Alarm"] as XmlNode;
           var alarmName = SLVHelper.GenerateUniqueName("SLV1275");

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

           Step("4. Select an alarm and note its Info value");
           Step("5. Expected Acknowledged button is enabled");
           VerifyEqual("5. Verify Acknowledge button is disabled before an alarm is selected", false, alarmsPage.GridPanel.IsAcknowledgeToolbarButtonEnabled());
           alarmsPage.GridPanel.ClickGridRecord(alarmName);
           VerifyEqual("5. Verify Acknowledge button is enabled after an alarm is selected", true, alarmsPage.GridPanel.IsAcknowledgeToolbarButtonEnabled());

           Step("6. Click Acknowledged button");
           alarmsPage.GridPanel.ClickAcknowledgeToolbarButton();

           Step("7. Expected \"Acknowledge Alarms\" dialog appears");
           VerifyEqual("7. Verify Acknowlement dialog appears", true, alarmsPage.Dialog.IsDialogVisible());

           Step("8. Enter reason of acknowledgement into Message field then click Send");
           var message = string.Format("Acknowledged on {0}", DateTime.Now.ToUniversalTime());
           alarmsPage.Dialog.EnterAcknowledgeMessageInput(message);
           alarmsPage.Dialog.ClickSendButton();
           alarmsPage.Dialog.WaitForPopupMessageDisplayed();

           Step("9. Expected A popup with message \"Acknowledgement sent!\" appears");
           VerifyEqual(string.Format("9. Verify Popup with message {0} appears", alarmsPage.Dialog.GetMessageText()), "Acknowledgement sent!", alarmsPage.Dialog.GetMessageText());

           Step("10. Click OK on the popup");
           alarmsPage.Dialog.ClickOkButton();
           alarmsPage.Dialog.WaitForDialogDisappeared();

           Step("11. Expected The popup disappears.Info of the selected alarm now is the value entered at step #8");
           VerifyEqual("11. Verify Dialogs disappear", false, alarmsPage.Dialog.IsDialogVisible());

           var tblGrid = alarmsPage.GridPanel.BuildDataTableFromGrid();
           var tblView = tblGrid.DefaultView;
           tblView.RowFilter = string.Format("Name = '{0}' AND Info = '{1}'", alarmName, message);
           VerifyEqual(string.Format("11. Verify info column is value of entered message ({0})", message), true, tblView.Count > 0);

           try
           {
               DeleteAlarm(alarmName);
           }
           catch { }
       }

       [Test, DynamicRetry]
       [Description("SLV-1687 - Alarm Manager - Infinite loader after importing alarms")]
       [NonParallelizable]
       [Category("RunAlone")]
       public void SLV_1687()
       {
           var testData = GetTestDataOfTestSLV_1687();
           var importedAlarmName = testData["ImportedAlarmName"];
           var importedFileName = testData["ImportedFileName"];

           Step("**** Precondition ****");
           Step(" - User has logged in successfully");
           Step("**** Precondition ****\n");

           var loginPage = Browser.OpenCMS();
           var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
           desktopPage.InstallAppsIfNotExist(App.AlarmManager);

           Step("1. Go to Alarm Manager app");
           Step("2. Expected Alarm Manager page is routed and loaded successfully");
           var alarmManagerPage = desktopPage.GoToApp(App.AlarmManager) as AlarmManagerPage;
           // Remove the alarm with its name before importing it again
           if (alarmManagerPage.GridPanel.IsAlarmManagerGridHasTextPresent("Name", importedAlarmName))
           {
               alarmManagerPage.DeleteAlarm(importedAlarmName, true);
               alarmManagerPage.WaitForPreviousActionComplete();
               VerifyEqual("2. Verify the alarm has been removed in the grid", false, alarmManagerPage.GridPanel.IsAlarmManagerGridHasTextPresent("Name", importedAlarmName));
           }

           Step("3. Click Import button and browse to SLV-1687.csv file then click OK");
           Step("4. Expected");
           Step(" • Preloader appears");
           Step(" • Success message is shown with content \"{ {n1}}/{{n2}} alarms have been successfully imported\"");
           Step(" • Preloader disappears (According to the defect, the preloader still shows up after importing finishes)");
           var fullPathOfImportedFileName = Settings.GetFullPath(Settings.CSV_FILE_PATH + importedFileName);         
           alarmManagerPage.GridPanel.ClickImportToolbarButton();
           SLVHelper.OpenFileFromFileDialog(fullPathOfImportedFileName);
           alarmManagerPage.WaitUntilOpenFileDialogDisappears();
           alarmManagerPage.WaitForPreviousActionComplete();
           alarmManagerPage.WaitForHeaderMessageDisplayed();
           VerifyEqual("4. Verify success message after importing alarm", string.Format("{0} / {1} alarms have been successfully imported", 1, 1), alarmManagerPage.GetHeaderMessage());
           Wait.ForProgressCompleted();
           Wait.ForLoadingIconDisappeared();
           alarmManagerPage.WaitForHeaderMessageDisappeared();
           VerifyEqual("4. Verify the imported alarm is present in the grid", true, alarmManagerPage.GridPanel.IsAlarmManagerGridHasTextPresent("Name", importedAlarmName));

           Step("5. Refresh browser then go to Alarm Manager app again");
           desktopPage = Browser.RefreshLoggedInCMS();
           alarmManagerPage = desktopPage.GoToApp(App.AlarmManager) as AlarmManagerPage;

           Step("6. Expected Verify the imported alarm is still present in the grid");
           VerifyEqual("6. Verify the imported alarm is still present in the grid", true, alarmManagerPage.GridPanel.IsAlarmManagerGridHasTextPresent("Name", importedAlarmName));
       }

       [Test, DynamicRetry]
       [Description("SLV-1807 - FPL Requirement for customer accoungot view in Failure Tracking (FT)")]
       public void SLV_1807()
       {
           Assert.Pass("Covered by 4 tests: FT_05, FT_06, FT_07, FT_08");
       }

       [Test, DynamicRetry]
       [Description("Test all alarms at once")]
       [NonParallelizable]
       public void AlarmsTest()
       {
           var alarmTests = new List<dynamic>();
           var testData = GetTestDataOfAlarms();
           var mail = testData["Mail"] as XmlNode;

           Step("**** Precondition ****");
           Step(" - User has logged in successfully");
           Step("**** Precondition ****\n");

           DeleteGeozones("GZNALTS4*");
           DeleteAlarms("TS4*");
           var dataTS41301 = CreateTestDataTS41301(testData["TS4_13_01"] as XmlNode, mail);
           var dataTS41501 = CreateTestDataTS41501(testData["TS4_15_01"] as XmlNode, mail);
           var dataTS41601 = CreateTestDataTS41601(testData["TS4_16_01"] as XmlNode, mail);
           var dataTS41701 = CreateTestDataTS41701(testData["TS4_17_01"] as XmlNode, mail);
           var dataTS41801 = CreateTestDataTS41801(testData["TS4_18_01"] as XmlNode, mail);
           var dataTS41901 = CreateTestDataTS41901(testData["TS4_19_01"] as XmlNode, mail);
           var dataTS42002 = CreateTestDataTS42002(testData["TS4_20_02"] as XmlNode, mail);
           var dataTS42101 = CreateTestDataTS42101(testData["TS4_21_01"] as XmlNode, mail);
           var dataTS42201 = CreateTestDataTS42201(testData["TS4_22_01"] as XmlNode, mail);
           var dataTS42401 = CreateTestDataTS42401(testData["TS4_24_01"] as XmlNode, mail);
           var dataTS42501 = CreateTestDataTS42501(testData["TS4_25_01"] as XmlNode, mail);
           var dataTS42502 = CreateTestDataTS42502(testData["TS4_25_02"] as XmlNode, mail);

           var loginPage = Browser.OpenCMS();
           var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
           desktopPage.InstallAppsIfNotExist(App.AlarmManager, App.Alarms);

           Step("1. Go to Alarm Manager app");
           var alarmManagerPage = desktopPage.GoToApp(App.AlarmManager) as AlarmManagerPage;
           CreateAlarmTS41301(alarmManagerPage, ref dataTS41301);
           alarmTests.Add(dataTS41301);
           CreateAlarmTS41501(alarmManagerPage, ref dataTS41501);
           alarmTests.Add(dataTS41501);
           CreateAlarmTS41601(alarmManagerPage, ref dataTS41601);
           alarmTests.Add(dataTS41601);
           CreateAlarmTS41701(alarmManagerPage, ref dataTS41701);
           alarmTests.Add(dataTS41701);
           CreateAlarmTS41801(alarmManagerPage, ref dataTS41801);
           alarmTests.Add(dataTS41801);
           CreateAlarmTS41901(alarmManagerPage, ref dataTS41901);
           alarmTests.Add(dataTS41901);
           CreateAlarmTS42002(alarmManagerPage, ref dataTS42002);
           alarmTests.Add(dataTS42002);
           CreateAlarmTS42101(alarmManagerPage, ref dataTS42101);
           alarmTests.Add(dataTS42101);
           CreateAlarmTS42201(alarmManagerPage, ref dataTS42201);
           alarmTests.Add(dataTS42201);
           CreateAlarmTS42401(alarmManagerPage, ref dataTS42401);
           alarmTests.Add(dataTS42401);
           CreateAlarmTS42501(alarmManagerPage, ref dataTS42501);
           alarmTests.Add(dataTS42501);
           CreateAlarmTS42502(alarmManagerPage, ref dataTS42502);
           alarmTests.Add(dataTS42502);

           Step("2. Expected all alarms triggered");
           alarmManagerPage.GeozoneTreeMainPanel.SelectNode(Settings.RootGeozoneName);
           Wait.ForAlarmTrigger();
           var alarmsPage = alarmManagerPage.AppBar.SwitchTo(App.Alarms) as AlarmsPage;
           foreach (var test in alarmTests)
           {
               VerifyEqual(string.Format("2. Verify alarm '{0}' is present in grid", test.AlarmName), true, alarmsPage.GridPanel.IsAlarmGridHasTextPresent("Name", test.AlarmName));                
           }

           Step("3. Expected Emails sent for all alarm");
           foreach (var test in alarmTests)
           {
               var foundEmail = GetEmailForAlarm(test.AlarmName);
               var emailCheckingDateTime = Settings.GetServerTime().ToString("G");
               VerifyEqual(string.Format("3. Verify a sent alarm email is found in mailbox ({0}, Alarm created: {1}, Email checking: {2})", test.AlarmName, test.AlarmCreated, emailCheckingDateTime), true, foundEmail != null);
           }

           Step("4. Send request to make alarms auto acknowledged.");
           SetValueToDevice(dataTS41301.Controller, dataTS41301.Streetlight, dataTS41301.FailureId, false, Settings.GetCurrentControlerDateTime(dataTS41301.Controller));
           SetValueToDevice(dataTS41501.Controller, dataTS41501.Streetlight, dataTS41501.FailureId, false, Settings.GetCurrentControlerDateTime(dataTS41501.Controller));
           SetValueToDevice(dataTS41601.Controller, dataTS41601.Streetlight, dataTS41601.FailureId, false, Settings.GetCurrentControlerDateTime(dataTS41601.Controller));
           SetValueToDevice(dataTS41701.Controller, dataTS41701.Streetlight, dataTS41701.MeteringId, "40", Settings.GetCurrentControlerDateTime(dataTS41701.Controller));
           SetValueToDevice(dataTS41801.Controller, dataTS41801.Streetlight, dataTS41801.MeteringId, SLVHelper.GenerateStringInteger(20, 99), Settings.GetCurrentControlerDateTime(dataTS41801.Controller));
           SetValueToController(dataTS41901.Controller, dataTS41901.MeteringId, "ON", Settings.GetCurrentControlerDateTime(dataTS41901.Controller), true);
           UpdateAlarm(AlarmType.ControllerAlarmOnOffTimesVsPreviousDay.DefinitionId, dataTS42002.AlarmName, new string[] { "triggerCondition.delay|0" });
           SetValueToController(dataTS42101.Controller, dataTS42101.IoId, "OFF", Settings.GetCurrentControlerDateTime(dataTS42101.Controller));
           SetValueToController(dataTS42201.Controller, dataTS42201.IoId1, "OFF", Settings.GetCurrentControlerDateTime(dataTS42201.Controller));
           UpdateAlarm(AlarmType.MeterAlarmComparisonToATrigger.DefinitionId, dataTS42401.AlarmName, new string[] { "triggerCondition.delay2|300000" });
           UpdateAlarm(AlarmType.MeterAlarmDataAnalysisVsPreviousDay.DefinitionId, dataTS42501.AlarmName, new string[] { "triggerCondition.percentageDifferenceTriggerValue|100" });
           UpdateAlarm(AlarmType.MeterAlarmDataAnalysisVsPreviousDayAtFixedTime.DefinitionId, dataTS42502.AlarmName, new string[] { "triggerCondition.percentageDifferenceTriggerValue|100" });

           Step("5. Reload alarm grid and verify all alarms are auto acknowledged.");
           Wait.ForAlarmTrigger();
           alarmsPage.GridPanel.ClickReloadDataToolbarButton();
           alarmsPage.WaitForPreviousActionComplete();
           foreach (var test in alarmTests)
           {
               VerifyEqual(string.Format("5. Verify alarm '{0}' is not present in grid any longer because it was auto-acknowledged", test.AlarmName), false, alarmsPage.GridPanel.IsAlarmGridHasTextPresent("Name", test.AlarmName));
           }

           try
           {
                DeleteAlarms("TS4*");
                DeleteGeozones("GZNALTS4*");
           }
           catch { }
       }

       #endregion //Test Cases

       #region Private methods

       #region Init alarm data and create

       private dynamic CreateTestDataTS41301(XmlNode data, XmlNode mail)
       {
           dynamic testData = new ExpandoObject();
           testData.Data = data;
           testData.Mail = mail;
           testData.AlarmName = SLVHelper.GenerateUniqueName(string.Format("[{0}][{1}][{2}] {3}", Browser.Name, Settings.Users["DefaultTest"].Username, data.GetAttrVal("name"), data.GetAttrVal("type")));
           testData.Geozone = SLVHelper.GenerateUniqueName("GZNALTS41301");
           testData.Controller = SLVHelper.GenerateUniqueName("CTRL");
           testData.Streetlight = SLVHelper.GenerateUniqueName("STL");
           testData.DeviceNameWithControllerId = string.Format("{0} [@{1}]", testData.Streetlight, testData.Controller);
           var failureInfoRegex = Regex.Match(data.GetAttrVal("failure"), @"(.*)#(.*)");
           testData.FailureName = failureInfoRegex.Groups[1].Value;
           testData.FailureId = failureInfoRegex.Groups[2].Value;
           CreateNewGeozone(testData.Geozone);
           CreateNewController(testData.Controller, testData.Geozone);
           SetValueToController(testData.Controller, "TimeZoneId", "UTC", Settings.GetServerTime());
           CreateNewDevice(DeviceType.Streetlight, testData.Streetlight, testData.Controller, testData.Geozone);
           SetValueToDevice(testData.Controller, testData.Streetlight, testData.FailureId, true, Settings.GetServerTime().AddSeconds(1));

           return testData;
       }

       private void CreateAlarmTS41301(AlarmManagerPage page, ref dynamic testData)
       {
           var data = testData.Data as XmlNode;
           var mail = testData.Mail as XmlNode;

           page.GeozoneTreeMainPanel.SelectNode(testData.Geozone);
           page.GridPanel.ClickAddAlarmToolbarButton();
           page.WaitForPreviousActionComplete();
           page.AlarmEditorPanel.EnterNameInput(testData.AlarmName);
           page.AlarmEditorPanel.SelectTypeDropDown(data.GetAttrVal("type"));
           page.WaitForPreviousActionComplete();
           page.AlarmEditorPanel.SelectActionDropDown(data.GetAttrVal("action"));
           page.AlarmEditorPanel.SelectTab("General");
           page.AlarmEditorPanel.TickAutoAcknowledgeCheckbox(bool.Parse(data.GetAttrVal("auto-acknowledge")));
           page.AlarmEditorPanel.SelectRefreshRateDropDown(data.GetAttrVal("refresh-rate"));
           page.AlarmEditorPanel.SelectTab("Trigger condition");
           page.AlarmEditorPanel.SelectFailuresListDropDown(testData.FailureName);
           page.AlarmEditorPanel.SelectDevicesListDropDown(testData.DeviceNameWithControllerId);
           page.AlarmEditorPanel.SelectTab("Actions");
           page.AlarmEditorPanel.EnterFromInput(mail.GetAttrVal("from"));
           page.AlarmEditorPanel.SelectToListDropDown(mail.GetAttrVal("to"));
           page.AlarmEditorPanel.EnterSubjectInput(testData.AlarmName);
           page.AlarmEditorPanel.EnterEmailMessageInput(data.GetAttrVal("mail-content"));
           page.AlarmEditorPanel.ClickSaveButton();
           page.WaitForPreviousActionComplete();
           testData.AlarmCreated = Settings.GetServerTime();
       }

       private dynamic CreateTestDataTS41501(XmlNode data, XmlNode mail)
       {
           dynamic testData = new ExpandoObject();
           testData.Data = data;
           testData.Mail = mail;
           testData.AlarmName = SLVHelper.GenerateUniqueName(string.Format("[{0}][{1}][{2}] {3}", Browser.Name, Settings.Users["DefaultTest"].Username, data.GetAttrVal("name"), data.GetAttrVal("type")));
           testData.Geozone = SLVHelper.GenerateUniqueName("GZNALTS41501");
           testData.Controller = SLVHelper.GenerateUniqueName("CTRL");
           testData.Streetlight = SLVHelper.GenerateUniqueName("STL");
           var failureInfoRegex = Regex.Match(data.GetAttrVal("failure"), @"(.*)#(.*)");
           testData.FailureName = failureInfoRegex.Groups[1].Value;
           testData.FailureId = failureInfoRegex.Groups[2].Value;
           CreateNewGeozone(testData.Geozone);
           CreateNewController(testData.Controller, testData.Geozone);
           SetValueToController(testData.Controller, "TimeZoneId", "UTC", Settings.GetServerTime());
           CreateNewDevice(DeviceType.Streetlight, testData.Streetlight, testData.Controller, testData.Geozone);
           var thresholdNo = int.Parse(data.GetAttrVal("threshold"));
           for (var i = 0; i < (thresholdNo <= 0 ? 1 : thresholdNo); i++)
           {
               SetValueToDevice(testData.Controller, testData.Streetlight, testData.FailureId, true, Settings.GetCurrentControlerDateTime(testData.Controller));
           }

           return testData;
       }

       private void CreateAlarmTS41501(AlarmManagerPage page, ref dynamic testData)
       {
           var data = testData.Data as XmlNode;
           var mail = testData.Mail as XmlNode;

           page.GeozoneTreeMainPanel.SelectNode(testData.Geozone);
           page.GridPanel.ClickAddAlarmToolbarButton();
           page.WaitForPreviousActionComplete();
           page.AlarmEditorPanel.EnterNameInput(testData.AlarmName);
           page.AlarmEditorPanel.SelectTypeDropDown(data.GetAttrVal("type"));
           page.WaitForPreviousActionComplete();
           page.AlarmEditorPanel.SelectActionDropDown(data.GetAttrVal("action"));
           page.AlarmEditorPanel.SelectTab("General");
           page.AlarmEditorPanel.TickAutoAcknowledgeCheckbox(bool.Parse(data.GetAttrVal("auto-acknowledge")));
           page.AlarmEditorPanel.SelectRefreshRateDropDown(data.GetAttrVal("refresh-rate"));
           page.AlarmEditorPanel.SelectTab("Trigger condition");
           page.AlarmEditorPanel.SelectFailuresListDropDown(testData.FailureName);
           page.AlarmEditorPanel.EnterRadiusNumericInput(data.GetAttrVal("radius"));
           page.AlarmEditorPanel.EnterThresholdNumericInput(data.GetAttrVal("threshold"));
           page.AlarmEditorPanel.SelectTab("Actions");
           page.AlarmEditorPanel.EnterFromInput(mail.GetAttrVal("from"));
           page.AlarmEditorPanel.SelectToListDropDown(mail.GetAttrVal("to"));
           page.AlarmEditorPanel.EnterSubjectInput(testData.AlarmName);
           page.AlarmEditorPanel.EnterEmailMessageInput(data.GetAttrVal("mail-content"));
           page.AlarmEditorPanel.ClickSaveButton();
           page.WaitForPreviousActionComplete();
           testData.AlarmCreated = Settings.GetServerTime();
       }

       private dynamic CreateTestDataTS41601(XmlNode data, XmlNode mail)
       {
           dynamic testData = new ExpandoObject();
           testData.Data = data;
           testData.Mail = mail;
           testData.AlarmName = SLVHelper.GenerateUniqueName(string.Format("[{0}][{1}][{2}] {3}", Browser.Name, Settings.Users["DefaultTest"].Username, data.GetAttrVal("name"), data.GetAttrVal("type")));
           testData.Geozone = SLVHelper.GenerateUniqueName("GZNALTS41601");
           testData.Controller = SLVHelper.GenerateUniqueName("CTRL");
           testData.Streetlight = SLVHelper.GenerateUniqueName("STL");
           var failureInfoRegex = Regex.Match(data.GetAttrVal("failure"), @"(.*)#(.*)");
           testData.FailureName = failureInfoRegex.Groups[1].Value;
           testData.FailureId = failureInfoRegex.Groups[2].Value;
           CreateNewGeozone(testData.Geozone);
           CreateNewController(testData.Controller, testData.Geozone);
           SetValueToController(testData.Controller, "TimeZoneId", "UTC", Settings.GetServerTime());
           CreateNewDevice(DeviceType.Streetlight, testData.Streetlight, testData.Controller, testData.Geozone);
           SetValueToDevice(testData.Controller, testData.Streetlight, testData.FailureId, true, Settings.GetCurrentControlerDateTime(testData.Controller));

           return testData;
       }

       private void CreateAlarmTS41601(AlarmManagerPage page, ref dynamic testData)
       {
           var data = testData.Data as XmlNode;
           var mail = testData.Mail as XmlNode;

           page.GeozoneTreeMainPanel.SelectNode(testData.Geozone);
           page.GridPanel.ClickAddAlarmToolbarButton();
           page.WaitForPreviousActionComplete();
           page.AlarmEditorPanel.EnterNameInput(testData.AlarmName);
           page.AlarmEditorPanel.SelectTypeDropDown(data.GetAttrVal("type"));
           page.WaitForPreviousActionComplete();
           page.AlarmEditorPanel.SelectActionDropDown(data.GetAttrVal("action"));
           page.AlarmEditorPanel.SelectTab("General");
           page.AlarmEditorPanel.TickAutoAcknowledgeCheckbox(bool.Parse(data.GetAttrVal("auto-acknowledge")));
           page.AlarmEditorPanel.SelectRefreshRateDropDown(data.GetAttrVal("refresh-rate"));
           page.AlarmEditorPanel.SelectTab("Trigger condition");
           page.AlarmEditorPanel.SelectFailuresListDropDown(testData.FailureName);
           page.AlarmEditorPanel.EnterCriticalFailureRatioNumericInput(data.GetAttrVal("critical-failure-ratio"));
           page.AlarmEditorPanel.SelectTab("Actions");
           page.AlarmEditorPanel.EnterFromInput(mail.GetAttrVal("from"));
           page.AlarmEditorPanel.SelectToListDropDown(mail.GetAttrVal("to"));
           page.AlarmEditorPanel.EnterSubjectInput(testData.AlarmName);
           page.AlarmEditorPanel.EnterEmailMessageInput(data.GetAttrVal("mail-content"));
           page.AlarmEditorPanel.ClickSaveButton();
           page.WaitForPreviousActionComplete();
           testData.AlarmCreated = Settings.GetServerTime();
       }

       private dynamic CreateTestDataTS41701(XmlNode data, XmlNode mail)
       {
           dynamic testData = new ExpandoObject();
           testData.Data = data;
           testData.Mail = mail;
           testData.AlarmName = SLVHelper.GenerateUniqueName(string.Format("[{0}][{1}][{2}] {3}", Browser.Name, Settings.Users["DefaultTest"].Username, data.GetAttrVal("name"), data.GetAttrVal("type")));
           testData.Geozone = SLVHelper.GenerateUniqueName("GZNALTS41701");
           testData.Controller = SLVHelper.GenerateUniqueName("CTRL");
           testData.Streetlight = SLVHelper.GenerateUniqueName("STL");
           var meteringInfoRegex = Regex.Match(data.GetAttrVal("metering"), @"(.*)#(.*)");
           testData.MeteringName = meteringInfoRegex.Groups[1].Value;
           testData.MeteringId = meteringInfoRegex.Groups[2].Value;
           CreateNewGeozone(testData.Geozone);
           CreateNewController(testData.Controller, testData.Geozone);
           SetValueToController(testData.Controller, "TimeZoneId", "UTC", Settings.GetServerTime());
           CreateNewDevice(DeviceType.Streetlight, testData.Streetlight, testData.Controller, testData.Geozone);
           var curCtrlDateTime = Settings.GetCurrentControlerDateTime(testData.Controller);
           var previousBefore10m = curCtrlDateTime.AddDays(-1).AddMinutes(-10);
           var previousDay = curCtrlDateTime.AddDays(-1);
           var value = "50";
           SetValueToDevice(testData.Controller, testData.Streetlight, testData.MeteringId, value, previousBefore10m);
           SetValueToDevice(testData.Controller, testData.Streetlight, testData.MeteringId, value, previousDay);

           curCtrlDateTime = Settings.GetCurrentControlerDateTime(testData.Controller);
           var currentDateTimeBefore10m = curCtrlDateTime.AddMinutes(-10);
           value = "60";
           SetValueToDevice(testData.Controller, testData.Streetlight, testData.MeteringId, value, currentDateTimeBefore10m);
           SetValueToDevice(testData.Controller, testData.Streetlight, testData.MeteringId, value, curCtrlDateTime);

           return testData;
       }

       private void CreateAlarmTS41701(AlarmManagerPage page, ref dynamic testData)
       {
           var data = testData.Data as XmlNode;
           var mail = testData.Mail as XmlNode;

           page.GeozoneTreeMainPanel.SelectNode(testData.Geozone);
           page.GridPanel.ClickAddAlarmToolbarButton();
           page.WaitForPreviousActionComplete();
           page.AlarmEditorPanel.EnterNameInput(testData.AlarmName);
           page.AlarmEditorPanel.SelectTypeDropDown(data.GetAttrVal("type"));
           page.WaitForPreviousActionComplete();
           page.AlarmEditorPanel.SelectActionDropDown(data.GetAttrVal("action"));
           page.AlarmEditorPanel.SelectTab("General");
           page.AlarmEditorPanel.TickAutoAcknowledgeCheckbox(bool.Parse(data.GetAttrVal("auto-acknowledge")));
           page.AlarmEditorPanel.SelectRefreshRateDropDown(data.GetAttrVal("refresh-rate"));
           page.AlarmEditorPanel.SelectTab("Trigger condition");
           page.AlarmEditorPanel.SelectDevicesListDropDown(testData.Streetlight);
           page.AlarmEditorPanel.SelectMeteringDropDown(testData.MeteringName);
           page.AlarmEditorPanel.SelectIgnoreOperatorDropDown(data.GetAttrVal("ignore-operator"));
           page.AlarmEditorPanel.EnterIgnoreValueNumericInput(data.GetAttrVal("ignore-value"));
           page.AlarmEditorPanel.SelectAnalysisPeriodDropDown(data.GetAttrVal("analysis-period"));
           page.AlarmEditorPanel.SelectAnalyticModeDropDown(data.GetAttrVal("analytic-mode"));
           page.AlarmEditorPanel.EnterPercentageDifferenceTriggerNumericInput(data.GetAttrVal("percentage-difference-trigger"));
           page.AlarmEditorPanel.SelectTab("Actions");
           page.AlarmEditorPanel.EnterFromInput(mail.GetAttrVal("from"));
           page.AlarmEditorPanel.SelectToListDropDown(mail.GetAttrVal("to"));
           page.AlarmEditorPanel.EnterSubjectInput(testData.AlarmName);
           page.AlarmEditorPanel.EnterEmailMessageInput(data.GetAttrVal("mail-content"));
           page.AlarmEditorPanel.ClickSaveButton();
           page.WaitForPreviousActionComplete();
           testData.AlarmCreated = Settings.GetServerTime();
       }

       private dynamic CreateTestDataTS41801(XmlNode data, XmlNode mail)
       {
           dynamic testData = new ExpandoObject();
           testData.Data = data;
           testData.Mail = mail;
           testData.AlarmName = SLVHelper.GenerateUniqueName(string.Format("[{0}][{1}][{2}] {3}", Browser.Name, Settings.Users["DefaultTest"].Username, data.GetAttrVal("name"), data.GetAttrVal("type")));
           testData.Geozone = SLVHelper.GenerateUniqueName("GZNALTS41801");
           testData.Controller = SLVHelper.GenerateUniqueName("CTRL");
           testData.Streetlight = SLVHelper.GenerateUniqueName("STL");
           testData.DeviceNameWithControllerId = string.Format("{0} [@{1}]", testData.Streetlight, testData.Controller);
           testData.MeteringId = data.GetAttrVal("metering-id");
           CreateNewGeozone(testData.Geozone);
           CreateNewController(testData.Controller, testData.Geozone);
           SetValueToController(testData.Controller, "TimeZoneId", "UTC", Settings.GetServerTime());
           CreateNewDevice(DeviceType.Streetlight, testData.Streetlight, testData.Controller, testData.Geozone);
           SetValueToDevice(testData.Controller, testData.Streetlight, testData.MeteringId, SLVHelper.GenerateStringInteger(20, 99), Settings.GetCurrentControlerDateTime(testData.Controller).AddDays(-1));

           return testData;
       }

       private void CreateAlarmTS41801(AlarmManagerPage page, ref dynamic testData)
       {
           var data = testData.Data as XmlNode;
           var mail = testData.Mail as XmlNode;

           page.GeozoneTreeMainPanel.SelectNode(testData.Geozone);
           page.GridPanel.ClickAddAlarmToolbarButton();
           page.WaitForPreviousActionComplete();
           page.AlarmEditorPanel.EnterNameInput(testData.AlarmName);
           page.AlarmEditorPanel.SelectTypeDropDown(data.GetAttrVal("type"));
           page.WaitForPreviousActionComplete();
           page.AlarmEditorPanel.SelectActionDropDown(data.GetAttrVal("action"));
           page.AlarmEditorPanel.SelectTab("General");
           page.AlarmEditorPanel.TickAutoAcknowledgeCheckbox(bool.Parse(data.GetAttrVal("auto-acknowledge")));
           page.AlarmEditorPanel.SelectRefreshRateDropDown(data.GetAttrVal("refresh-rate"));
           page.AlarmEditorPanel.SelectTab("Trigger condition");
           page.AlarmEditorPanel.SelectDevicesListDropDown(testData.DeviceNameWithControllerId);
           page.AlarmEditorPanel.SelectVariablesTypeListDropDown(data.GetAttrVal("variables-type"));
           page.AlarmEditorPanel.SelectCriticalDelayDropDown(data.GetAttrVal("critical-delay"));
           page.AlarmEditorPanel.SelectTimestampModeDropDown(data.GetAttrVal("timestamp-mode"));
           page.AlarmEditorPanel.EnterCriticalRatioNumericInput(data.GetAttrVal("critical-ratio"));
           page.AlarmEditorPanel.SelectTab("Actions");
           page.AlarmEditorPanel.EnterFromInput(mail.GetAttrVal("from"));
           page.AlarmEditorPanel.SelectToListDropDown(mail.GetAttrVal("to"));
           page.AlarmEditorPanel.EnterSubjectInput(testData.AlarmName);
           page.AlarmEditorPanel.EnterEmailMessageInput(data.GetAttrVal("mail-content"));
           page.AlarmEditorPanel.ClickSaveButton();
           page.WaitForPreviousActionComplete();
           testData.AlarmCreated = Settings.GetServerTime();
       }

       private dynamic CreateTestDataTS41901(XmlNode data, XmlNode mail)
       {
           dynamic testData = new ExpandoObject();
           testData.Data = data;
           testData.Mail = mail;
           testData.AlarmName = SLVHelper.GenerateUniqueName(string.Format("[{0}][{1}][{2}] {3}", Browser.Name, Settings.Users["DefaultTest"].Username, data.GetAttrVal("name"), data.GetAttrVal("type")));
           testData.Geozone = SLVHelper.GenerateUniqueName("GZNALTS41901");
           testData.Controller = SLVHelper.GenerateUniqueName("CTRL");
           testData.MeteringId = data.GetAttrVal("metering-id");
           CreateNewGeozone(testData.Geozone);
           CreateNewController(testData.Controller, testData.Geozone);
           SetValueToController(testData.Controller, "TimeZoneId", "UTC", Settings.GetServerTime());
           SetValueToController(testData.Controller, testData.MeteringId, "ON", Settings.GetCurrentControlerDateTime(testData.Controller).AddDays(-1));

           return testData;
       }

       private void CreateAlarmTS41901(AlarmManagerPage page, ref dynamic testData)
       {
           var data = testData.Data as XmlNode;
           var mail = testData.Mail as XmlNode;

           page.GeozoneTreeMainPanel.SelectNode(testData.Geozone);
           page.GridPanel.ClickAddAlarmToolbarButton();
           page.WaitForPreviousActionComplete();
           page.AlarmEditorPanel.EnterNameInput(testData.AlarmName);
           page.AlarmEditorPanel.SelectTypeDropDown(data.GetAttrVal("type"));
           page.WaitForPreviousActionComplete();
           page.AlarmEditorPanel.SelectActionDropDown(data.GetAttrVal("action"));
           page.AlarmEditorPanel.SelectTab("General");
           page.AlarmEditorPanel.TickAutoAcknowledgeCheckbox(bool.Parse(data.GetAttrVal("auto-acknowledge")));
           page.AlarmEditorPanel.SelectRefreshRateDropDown(data.GetAttrVal("refresh-rate"));
           page.AlarmEditorPanel.SelectTab("Trigger condition");
           page.AlarmEditorPanel.EnterHoursDelayNumbericInput(data.GetAttrVal("delay"));
           page.AlarmEditorPanel.SelectControllersListDropDown(testData.Controller);
           page.AlarmEditorPanel.SelectTab("Actions");
           page.AlarmEditorPanel.EnterFromInput(mail.GetAttrVal("from"));
           page.AlarmEditorPanel.SelectToListDropDown(mail.GetAttrVal("to"));
           page.AlarmEditorPanel.EnterSubjectInput(testData.AlarmName);
           page.AlarmEditorPanel.EnterEmailMessageInput(data.GetAttrVal("mail-content"));
           page.AlarmEditorPanel.ClickSaveButton();
           page.WaitForPreviousActionComplete();
           testData.AlarmCreated = Settings.GetServerTime();
       }

       private dynamic CreateTestDataTS42002(XmlNode data, XmlNode mail)
       {
           dynamic testData = new ExpandoObject();
           testData.Data = data;
           testData.Mail = mail;
           testData.AlarmName = SLVHelper.GenerateUniqueName(string.Format("[{0}][{1}][{2}] {3}", Browser.Name, Settings.Users["DefaultTest"].Username, data.GetAttrVal("name"), data.GetAttrVal("type")));
           testData.Geozone = SLVHelper.GenerateUniqueName("GZNALTS42002");
           testData.Controller = SLVHelper.GenerateUniqueName("CTRL");
           var ioInfoRegex = Regex.Match(data.GetAttrVal("io"), @"(.*)#(.*)");
           testData.IoName = ioInfoRegex.Groups[1].Value;
           testData.IoId = ioInfoRegex.Groups[2].Value;
           CreateNewGeozone(testData.Geozone);
           CreateNewController(testData.Controller, testData.Geozone);
           SetValueToController(testData.Controller, "TimeZoneId", "UTC", Settings.GetServerTime());
           var delayNo = int.Parse(data.GetAttrVal("delay"));
           var curCtrlDateTime = Settings.GetCurrentControlerDateTime(testData.Controller);
           var curPrevDateTime = curCtrlDateTime.AddDays(-1).AddMinutes(0 - (delayNo / 2));
           SetValueToController(testData.Controller, testData.IoId, "ON", curPrevDateTime);
           SetValueToController(testData.Controller, testData.IoId, "OFF", curCtrlDateTime);

           return testData;
       }

       private void CreateAlarmTS42002(AlarmManagerPage page, ref dynamic testData)
       {
           var data = testData.Data as XmlNode;
           var mail = testData.Mail as XmlNode;

           page.GeozoneTreeMainPanel.SelectNode(testData.Geozone);
           page.GridPanel.ClickAddAlarmToolbarButton();
           page.WaitForPreviousActionComplete();
           page.AlarmEditorPanel.EnterNameInput(testData.AlarmName);
           page.AlarmEditorPanel.SelectTypeDropDown(data.GetAttrVal("type"));
           page.WaitForPreviousActionComplete();
           page.AlarmEditorPanel.SelectActionDropDown(data.GetAttrVal("action"));
           page.AlarmEditorPanel.SelectTab("General");
           page.AlarmEditorPanel.TickAutoAcknowledgeCheckbox(bool.Parse(data.GetAttrVal("auto-acknowledge")));
           page.AlarmEditorPanel.SelectRefreshRateDropDown(data.GetAttrVal("refresh-rate"));
           page.AlarmEditorPanel.SelectTab("Trigger condition");
           page.AlarmEditorPanel.SelectControllersListDropDown(testData.Controller);
           page.AlarmEditorPanel.SelectInputOutputDropDown(testData.IoName);
           page.AlarmEditorPanel.EnterDelayNumbericInput(data.GetAttrVal("delay"));
           page.AlarmEditorPanel.SelectTab("Actions");
           page.AlarmEditorPanel.EnterFromInput(mail.GetAttrVal("from"));
           page.AlarmEditorPanel.SelectToListDropDown(mail.GetAttrVal("to"));
           page.AlarmEditorPanel.EnterSubjectInput(testData.AlarmName);
           page.AlarmEditorPanel.EnterEmailMessageInput(data.GetAttrVal("mail-content"));
           page.AlarmEditorPanel.ClickSaveButton();
           page.WaitForPreviousActionComplete();
           testData.AlarmCreated = Settings.GetServerTime();
       }

       private dynamic CreateTestDataTS42101(XmlNode data, XmlNode mail)
       {
           dynamic testData = new ExpandoObject();
           testData.Data = data;
           testData.Mail = mail;
           testData.AlarmName = SLVHelper.GenerateUniqueName(string.Format("[{0}][{1}][{2}] {3}", Browser.Name, Settings.Users["DefaultTest"].Username, data.GetAttrVal("name"), data.GetAttrVal("type")));
           testData.Geozone = SLVHelper.GenerateUniqueName("GZNALTS42101");
           testData.Controller = SLVHelper.GenerateUniqueName("CTRL");
           var ioInfoRegex = Regex.Match(data.GetAttrVal("io"), @"(.*)#(.*)");
           testData.IoName = ioInfoRegex.Groups[1].Value;
           testData.IoId = ioInfoRegex.Groups[2].Value;
           CreateNewGeozone(testData.Geozone);
           CreateNewController(testData.Controller, testData.Geozone);
           SetValueToController(testData.Controller, "TimeZoneId", "UTC", Settings.GetServerTime());
           SetValueToController(testData.Controller, testData.IoId, data.GetAttrVal("value"), Settings.GetCurrentControlerDateTime(testData.Controller).AddHours(-1));

           return testData;
       }

       private void CreateAlarmTS42101(AlarmManagerPage page, ref dynamic testData)
       {
           var data = testData.Data as XmlNode;
           var mail = testData.Mail as XmlNode;

           page.GeozoneTreeMainPanel.SelectNode(testData.Geozone);
           page.GridPanel.ClickAddAlarmToolbarButton();
           page.WaitForPreviousActionComplete();
           page.AlarmEditorPanel.EnterNameInput(testData.AlarmName);
           page.AlarmEditorPanel.SelectTypeDropDown(data.GetAttrVal("type"));
           page.WaitForPreviousActionComplete();
           page.AlarmEditorPanel.SelectActionDropDown(data.GetAttrVal("action"));
           page.AlarmEditorPanel.SelectTab("General");
           page.AlarmEditorPanel.TickAutoAcknowledgeCheckbox(bool.Parse(data.GetAttrVal("auto-acknowledge")));
           page.AlarmEditorPanel.SelectRefreshRateDropDown(data.GetAttrVal("refresh-rate"));
           page.AlarmEditorPanel.SelectTab("Trigger condition");
           page.AlarmEditorPanel.SelectControllersListDropDown(testData.Controller);
           page.AlarmEditorPanel.SelectInputNameDropDown(testData.IoName);
           page.AlarmEditorPanel.SelectInputValueDropDown(data.GetAttrVal("value"));
           page.AlarmEditorPanel.SelectTab("Actions");
           page.AlarmEditorPanel.EnterFromInput(mail.GetAttrVal("from"));
           page.AlarmEditorPanel.SelectToListDropDown(mail.GetAttrVal("to"));
           page.AlarmEditorPanel.EnterSubjectInput(testData.AlarmName);
           page.AlarmEditorPanel.EnterEmailMessageInput(data.GetAttrVal("mail-content"));
           page.AlarmEditorPanel.ClickSaveButton();
           page.WaitForPreviousActionComplete();
           testData.AlarmCreated = Settings.GetServerTime();
       }

       private dynamic CreateTestDataTS42201(XmlNode data, XmlNode mail)
       {
           dynamic testData = new ExpandoObject();
           testData.Data = data;
           testData.Mail = mail;
           testData.AlarmName = SLVHelper.GenerateUniqueName(string.Format("[{0}][{1}][{2}] {3}", Browser.Name, Settings.Users["DefaultTest"].Username, data.GetAttrVal("name"), data.GetAttrVal("type")));
           testData.Geozone = SLVHelper.GenerateUniqueName("GZNALTS42201");
           testData.Controller = SLVHelper.GenerateUniqueName("CTRL");
           var io1InfoRegex = Regex.Match(data.GetAttrVal("first-io"), @"(.*)#(.*)");
           testData.IoName1 = io1InfoRegex.Groups[1].Value;
           testData.IoId1 = io1InfoRegex.Groups[2].Value;
           var io2InfoRegex = Regex.Match(data.GetAttrVal("second-io"), @"(.*)#(.*)");
           testData.IoName2 = io2InfoRegex.Groups[1].Value;
           testData.IoId2 = io2InfoRegex.Groups[2].Value;
           testData.IoValue = data.GetAttrVal("io-value");
           CreateNewGeozone(testData.Geozone);
           SetValueToController(testData.Controller, "TimeZoneId", "UTC", Settings.GetServerTime());
           CreateNewController(testData.Controller, testData.Geozone);            
           var curCtrlDateTime = Settings.GetCurrentControlerDateTime(testData.Controller);            
           SetValueToController(testData.Controller, testData.IoId1, testData.IoValue, curCtrlDateTime);
           SetValueToController(testData.Controller, testData.IoId2, testData.IoValue, curCtrlDateTime);

           return testData;
       }

       private void CreateAlarmTS42201(AlarmManagerPage page, ref dynamic testData)
       {
           var data = testData.Data as XmlNode;
           var mail = testData.Mail as XmlNode;

           page.GeozoneTreeMainPanel.SelectNode(testData.Geozone);
           page.GridPanel.ClickAddAlarmToolbarButton();
           page.WaitForPreviousActionComplete();
           page.AlarmEditorPanel.EnterNameInput(testData.AlarmName);
           page.AlarmEditorPanel.SelectTypeDropDown(data.GetAttrVal("type"));
           page.WaitForPreviousActionComplete();
           page.AlarmEditorPanel.SelectActionDropDown(data.GetAttrVal("action"));
           page.AlarmEditorPanel.SelectTab("General");
           page.AlarmEditorPanel.TickAutoAcknowledgeCheckbox(bool.Parse(data.GetAttrVal("auto-acknowledge")));
           page.AlarmEditorPanel.SelectRefreshRateDropDown(data.GetAttrVal("refresh-rate"));
           page.AlarmEditorPanel.SelectTab("Trigger condition");
           page.AlarmEditorPanel.SelectControllersListDropDown(testData.Controller);
           page.AlarmEditorPanel.SelectFirstIODropDown(testData.IoName1);
           page.AlarmEditorPanel.SelectSecondIODropDown(testData.IoName2);
           page.AlarmEditorPanel.SelectOperatorDropDown(data.GetAttrVal("operator"));
           page.AlarmEditorPanel.SelectTab("Actions");
           page.AlarmEditorPanel.EnterFromInput(mail.GetAttrVal("from"));
           page.AlarmEditorPanel.SelectToListDropDown(mail.GetAttrVal("to"));
           page.AlarmEditorPanel.EnterSubjectInput(testData.AlarmName);
           page.AlarmEditorPanel.EnterEmailMessageInput(data.GetAttrVal("mail-content"));
           page.AlarmEditorPanel.ClickSaveButton();
           page.WaitForPreviousActionComplete();
           testData.AlarmCreated = Settings.GetServerTime();
       }

       private dynamic CreateTestDataTS42401(XmlNode data, XmlNode mail)
       {
           dynamic testData = new ExpandoObject();
           testData.Data = data;
           testData.Mail = mail;
           testData.AlarmName = SLVHelper.GenerateUniqueName(string.Format("[{0}][{1}][{2}] {3}", Browser.Name, Settings.Users["DefaultTest"].Username, data.GetAttrVal("name"), data.GetAttrVal("type")));
           testData.Geozone = SLVHelper.GenerateUniqueName("GZNALTS42401");
           testData.Controller = SLVHelper.GenerateUniqueName("CTRL");
           testData.Meter = SLVHelper.GenerateUniqueName("MTR");
           var meteringInfoRegex = Regex.Match(data.GetAttrVal("metering"), @"(.*)#(.*)");
           testData.MeteringName = meteringInfoRegex.Groups[1].Value;
           testData.MeteringId = meteringInfoRegex.Groups[2].Value;
           testData.IgnoreValue = data.GetAttrVal("ignore-value");
           testData.TriggeringValue = data.GetAttrVal("triggering-value");
           CreateNewGeozone(testData.Geozone);
           CreateNewController(testData.Controller, testData.Geozone);
           SetValueToController(testData.Controller, "TimeZoneId", "UTC", Settings.GetServerTime());
           CreateNewDevice(DeviceType.ElectricalCounter, testData.Meter, testData.Controller, testData.Geozone);
           var curCtrlDateTime = Settings.GetCurrentControlerDateTime(testData.Controller);
           SetValueToDevice(testData.Controller, testData.Meter, testData.MeteringId, testData.TriggeringValue, curCtrlDateTime.AddMinutes(-36));
           SetValueToDevice(testData.Controller, testData.Meter, testData.MeteringId, testData.TriggeringValue, curCtrlDateTime.AddMinutes(-30));

           return testData;
       }

       private void CreateAlarmTS42401(AlarmManagerPage page, ref dynamic testData)
       {
           var data = testData.Data as XmlNode;
           var mail = testData.Mail as XmlNode;

           page.GeozoneTreeMainPanel.SelectNode(testData.Geozone);
           page.GridPanel.ClickAddAlarmToolbarButton();
           page.WaitForPreviousActionComplete();
           page.AlarmEditorPanel.EnterNameInput(testData.AlarmName);
           page.AlarmEditorPanel.SelectTypeDropDown(data.GetAttrVal("type"));
           page.WaitForPreviousActionComplete();
           page.AlarmEditorPanel.SelectActionDropDown(data.GetAttrVal("action"));
           page.AlarmEditorPanel.SelectTab("General");
           page.AlarmEditorPanel.TickAutoAcknowledgeCheckbox(bool.Parse(data.GetAttrVal("auto-acknowledge")));
           page.AlarmEditorPanel.SelectRefreshRateDropDown(data.GetAttrVal("refresh-rate"));
           page.AlarmEditorPanel.SelectTab("Trigger condition");
           page.AlarmEditorPanel.SelectMetersListDropDown(testData.Meter);
           page.AlarmEditorPanel.SelectMeteringDropDown(testData.MeteringName);
           page.AlarmEditorPanel.SelectIgnoreOperatorDropDown(data.GetAttrVal("ignore-operator"));
           page.AlarmEditorPanel.EnterIgnoreValueNumericInput(testData.IgnoreValue);
           page.AlarmEditorPanel.SelectTriggeringOperatorDropDown(data.GetAttrVal("triggering-operator"));
           page.AlarmEditorPanel.EnterTriggeringValueNumericInput(testData.TriggeringValue);
           page.AlarmEditorPanel.SelectAnalysisTimeT1DropDown(data.GetAttrVal("t1"));
           page.AlarmEditorPanel.SelectAlarmTimeT1DropDown(data.GetAttrVal("t2"));
           page.AlarmEditorPanel.SelectTab("Actions");
           page.AlarmEditorPanel.EnterFromInput(mail.GetAttrVal("from"));
           page.AlarmEditorPanel.SelectToListDropDown(mail.GetAttrVal("to"));
           page.AlarmEditorPanel.EnterSubjectInput(testData.AlarmName);
           page.AlarmEditorPanel.EnterEmailMessageInput(data.GetAttrVal("mail-content"));
           page.AlarmEditorPanel.ClickSaveButton();
           page.WaitForPreviousActionComplete();
           testData.AlarmCreated = Settings.GetServerTime();
       }

       private dynamic CreateTestDataTS42501(XmlNode data, XmlNode mail)
       {
           dynamic testData = new ExpandoObject();
           testData.Data = data;
           testData.Mail = mail;
           testData.AlarmName = SLVHelper.GenerateUniqueName(string.Format("[{0}][{1}][{2}] {3}", Browser.Name, Settings.Users["DefaultTest"].Username, data.GetAttrVal("name"), data.GetAttrVal("type")));
           testData.Geozone = SLVHelper.GenerateUniqueName("GZNALTS42501");
           testData.Controller = SLVHelper.GenerateUniqueName("CTRL");
           testData.Meter = SLVHelper.GenerateUniqueName("MTR");
           var meteringInfoRegex = Regex.Match(data.GetAttrVal("metering"), @"(.*)#(.*)");
           testData.MeteringName = meteringInfoRegex.Groups[1].Value;
           testData.MeteringId = meteringInfoRegex.Groups[2].Value;
           testData.IgnoreValue = data.GetAttrVal("ignore-value");
           CreateNewGeozone(testData.Geozone);
           CreateNewController(testData.Controller, testData.Geozone);
           SetValueToController(testData.Controller, "TimeZoneId", "UTC", Settings.GetServerTime());
           CreateNewDevice(DeviceType.ElectricalCounter, testData.Meter, testData.Controller, testData.Geozone);
           var curCtrlDateTime = Settings.GetCurrentControlerDateTime(testData.Controller);            
           var previousBefore10m = curCtrlDateTime.AddDays(-1).AddMinutes(-10);
           var previousDay = curCtrlDateTime.AddDays(-1);
           var currentDateTimeBefore35m = curCtrlDateTime.AddMinutes(-35);
           SetValueToDevice(testData.Controller, testData.Meter, testData.MeteringId, "60", previousBefore10m);            
           SetValueToDevice(testData.Controller, testData.Meter, testData.MeteringId, "60", previousDay);
           SetValueToDevice(testData.Controller, testData.Meter, testData.MeteringId, "50", currentDateTimeBefore35m);            
           SetValueToDevice(testData.Controller, testData.Meter, testData.MeteringId, "100", curCtrlDateTime);

           return testData;
       }

       private void CreateAlarmTS42501(AlarmManagerPage page, ref dynamic testData)
       {
           var data = testData.Data as XmlNode;
           var mail = testData.Mail as XmlNode;

           page.GeozoneTreeMainPanel.SelectNode(testData.Geozone);
           page.GridPanel.ClickAddAlarmToolbarButton();
           page.WaitForPreviousActionComplete();
           page.AlarmEditorPanel.EnterNameInput(testData.AlarmName);
           page.AlarmEditorPanel.SelectTypeDropDown(data.GetAttrVal("type"));
           page.WaitForPreviousActionComplete();
           page.AlarmEditorPanel.SelectActionDropDown(data.GetAttrVal("action"));
           page.AlarmEditorPanel.SelectTab("General");
           page.AlarmEditorPanel.TickAutoAcknowledgeCheckbox(bool.Parse(data.GetAttrVal("auto-acknowledge")));
           page.AlarmEditorPanel.SelectRefreshRateDropDown(data.GetAttrVal("refresh-rate"));
           page.AlarmEditorPanel.SelectTab("Trigger condition");
           page.AlarmEditorPanel.SelectMetersListDropDown(testData.Meter);
           page.AlarmEditorPanel.SelectMeteringDropDown(testData.MeteringName);
           page.AlarmEditorPanel.SelectIgnoreOperatorDropDown(data.GetAttrVal("ignore-operator"));
           page.AlarmEditorPanel.EnterIgnoreValueNumericInput(data.GetAttrVal("ignore-value"));
           page.AlarmEditorPanel.SelectAnalysisPeriodDropDown(data.GetAttrVal("analysis-period"));
           page.AlarmEditorPanel.SelectAnalyticModeDropDown(data.GetAttrVal("analytic-mode"));
           page.AlarmEditorPanel.EnterPercentageDifferenceTriggerNumericInput(data.GetAttrVal("percentage-difference-trigger"));
           page.AlarmEditorPanel.SelectTab("Actions");
           page.AlarmEditorPanel.EnterFromInput(mail.GetAttrVal("from"));
           page.AlarmEditorPanel.SelectToListDropDown(mail.GetAttrVal("to"));
           page.AlarmEditorPanel.EnterSubjectInput(testData.AlarmName);
           page.AlarmEditorPanel.EnterEmailMessageInput(data.GetAttrVal("mail-content"));
           page.AlarmEditorPanel.ClickSaveButton();
           page.WaitForPreviousActionComplete();
           testData.AlarmCreated = Settings.GetServerTime();
       }

       private dynamic CreateTestDataTS42502(XmlNode data, XmlNode mail)
       {
           dynamic testData = new ExpandoObject();
           testData.Data = data;
           testData.Mail = mail;
           testData.AlarmName = SLVHelper.GenerateUniqueName(string.Format("[{0}][{1}][{2}] {3}", Browser.Name, Settings.Users["DefaultTest"].Username, data.GetAttrVal("name"), data.GetAttrVal("type")));
           testData.Geozone = SLVHelper.GenerateUniqueName("GZNALTS42502");
           testData.Controller = SLVHelper.GenerateUniqueName("CTRL");
           testData.Meter = SLVHelper.GenerateUniqueName("MTR");
           var meteringInfoRegex = Regex.Match(data.GetAttrVal("metering"), @"(.*)#(.*)");
           testData.MeteringName = meteringInfoRegex.Groups[1].Value;
           testData.MeteringId = meteringInfoRegex.Groups[2].Value;
           testData.IgnoreValue = data.GetAttrVal("ignore-value");
           CreateNewGeozone(testData.Geozone);
           CreateNewController(testData.Controller, testData.Geozone);
           SetValueToController(testData.Controller, "TimeZoneId", "UTC", Settings.GetServerTime());
           CreateNewDevice(DeviceType.ElectricalCounter, testData.Meter, testData.Controller, testData.Geozone);
           var curCtrlDateTime = Settings.GetCurrentControlerDateTime(testData.Controller);
           var previousBefore10m = curCtrlDateTime.AddDays(-1).AddMinutes(-10);
           var previousDay = curCtrlDateTime.AddDays(-1);
           var currentDateTimeBefore10m = curCtrlDateTime.AddMinutes(-10);
           SetValueToDevice(testData.Controller, testData.Meter, testData.MeteringId, "50", previousBefore10m);
           SetValueToDevice(testData.Controller, testData.Meter, testData.MeteringId, "51", previousDay);
           SetValueToDevice(testData.Controller, testData.Meter, testData.MeteringId, "60", currentDateTimeBefore10m);
           SetValueToDevice(testData.Controller, testData.Meter, testData.MeteringId, "61", curCtrlDateTime);

           return testData;
       }

       private void CreateAlarmTS42502(AlarmManagerPage page, ref dynamic testData)
       {
           var data = testData.Data as XmlNode;
           var mail = testData.Mail as XmlNode;

           page.GeozoneTreeMainPanel.SelectNode(testData.Geozone);
           page.GridPanel.ClickAddAlarmToolbarButton();
           page.WaitForPreviousActionComplete();
           page.AlarmEditorPanel.EnterNameInput(testData.AlarmName);
           page.AlarmEditorPanel.SelectTypeDropDown(data.GetAttrVal("type"));
           page.WaitForPreviousActionComplete();
           page.AlarmEditorPanel.SelectActionDropDown(data.GetAttrVal("action"));
           page.AlarmEditorPanel.SelectTab("General");
           page.AlarmEditorPanel.TickAutoAcknowledgeCheckbox(bool.Parse(data.GetAttrVal("auto-acknowledge")));
           page.AlarmEditorPanel.SelectRefreshRateDropDown(data.GetAttrVal("refresh-rate"));
           page.AlarmEditorPanel.SelectTab("Trigger condition");
           page.AlarmEditorPanel.SelectMetersListDropDown(testData.Meter);
           page.AlarmEditorPanel.SelectMeteringDropDown(testData.MeteringName);
           page.AlarmEditorPanel.SelectIgnoreOperatorDropDown(data.GetAttrVal("ignore-operator"));
           page.AlarmEditorPanel.EnterIgnoreValueNumericInput(data.GetAttrVal("ignore-value"));
           page.AlarmEditorPanel.SelectAnalyticModeDropDown(data.GetAttrVal("analytic-mode"));
           page.AlarmEditorPanel.EnterPercentageDifferenceTriggerNumericInput(data.GetAttrVal("percentage-difference-trigger"));
           var curCtrlDateTime = Settings.GetCurrentControlerDateTime(testData.Controller);
           var fromTime = "00:00";
           if (curCtrlDateTime.Hour > 2) fromTime = curCtrlDateTime.AddHours(-2).ToString("HH:mm");
           page.AlarmEditorPanel.EnterFromDaytimeInput(fromTime);
           page.AlarmEditorPanel.EnterToDaytimeInput("23:59");
           page.AlarmEditorPanel.SelectTab("Actions");
           page.AlarmEditorPanel.EnterFromInput(mail.GetAttrVal("from"));
           page.AlarmEditorPanel.SelectToListDropDown(mail.GetAttrVal("to"));
           page.AlarmEditorPanel.EnterSubjectInput(testData.AlarmName);
           page.AlarmEditorPanel.EnterEmailMessageInput(data.GetAttrVal("mail-content"));
           page.AlarmEditorPanel.ClickSaveButton();
           page.WaitForPreviousActionComplete();
           testData.AlarmCreated = Settings.GetServerTime();
       }

       #endregion //Init alarm data and create

       private Dictionary<string, string> GetTestDataOfTestTS4_03_01()
       {
           var testCaseName = "TS4_03_01";
           var xmlUtility = new XmlUtility(Settings.TC4_TEST_DATA_FILE_PATH);
           var testData = new Dictionary<string, string>();

           testData.Add("Geozone1", xmlUtility.GetSingleNodeText(string.Format(Settings.TC4_XPATH_PREFIX, testCaseName, "Geozone1")));
           testData.Add("Geozone2", xmlUtility.GetSingleNodeText(string.Format(Settings.TC4_XPATH_PREFIX, testCaseName, "Geozone2")));

           return testData;
       }

       private Dictionary<string, string> GetTestDataOfTestTS4_04_01()
       {
           var testCaseName = "TS4_04_01";
           var xmlUtility = new XmlUtility(Settings.TC4_TEST_DATA_FILE_PATH);
           var testData = new Dictionary<string, string>();

           testData.Add("Geozone1", xmlUtility.GetSingleNodeText(string.Format(Settings.TC4_XPATH_PREFIX, testCaseName, "Geozone1")));
           testData.Add("Geozone2", xmlUtility.GetSingleNodeText(string.Format(Settings.TC4_XPATH_PREFIX, testCaseName, "Geozone2")));

           return testData;
       }

       private Dictionary<string, string> GetTestDataOfTestTS4_11_01()
       {
           var testCaseName = "TS4_11_01";
           var xmlUtility = new XmlUtility(Settings.TC4_TEST_DATA_FILE_PATH);
           var testData = new Dictionary<string, string>();

           testData.Add("SearchAttribute", xmlUtility.GetSingleNodeText(string.Format(Settings.TC4_XPATH_PREFIX, testCaseName, "SearchAttribute")));
           testData.Add("SearchOperator", xmlUtility.GetSingleNodeText(string.Format(Settings.TC4_XPATH_PREFIX, testCaseName, "SearchOperator")));
           testData.Add("InexistingUniqueAddress", xmlUtility.GetSingleNodeText(string.Format(Settings.TC4_XPATH_PREFIX, testCaseName, "InexistingUniqueAddress")));
           testData.Add("ExistingUniqueAddress", xmlUtility.GetSingleNodeText(string.Format(Settings.TC4_XPATH_PREFIX, testCaseName, "ExistingUniqueAddress")));

           return testData;

       }

       #region Comment out alarms because created all-in-one test case

       //private Dictionary<string, object> GetTestDataOfTestTS4_13_01()
       //{
       //    var testCaseName = "TS4_13_01";
       //    var xmlUtility = new XmlUtility(Settings.TC4_TEST_DATA_FILE_PATH);
       //    var testData = new Dictionary<string, object>();

       //    testData.Add("Geozone", xmlUtility.GetSingleNodeText(string.Format(Settings.TC4_XPATH_PREFIX, testCaseName, "Geozone")));

       //    var nodeAlarmInfo = xmlUtility.GetSingleNode(string.Format(Settings.TC4_XPATH_PREFIX, testCaseName, "AlarmInfo"));
       //    testData.Add("Alarm.name", nodeAlarmInfo.GetAttrVal("name"));
       //    testData.Add("Alarm.type", nodeAlarmInfo.GetAttrVal("type"));
       //    testData.Add("Alarm.action", nodeAlarmInfo.GetAttrVal("action"));

       //    var nodeGeneral = nodeAlarmInfo.GetChildNode("General");
       //    testData.Add("Alarm.General.auto-acknowledge", nodeGeneral.GetAttrVal("auto-acknowledge"));
       //    testData.Add("Alarm.General.refresh-rate", nodeGeneral.GetAttrVal("refresh-rate"));

       //    var nodeTrigger = nodeAlarmInfo.GetChildNode("Trigger");
       //    testData.Add("Alarm.Trigger.message", nodeTrigger.GetAttrVal("message"));
       //    testData.Add("Alarm.Trigger.failures", nodeTrigger.GetAttrVal("failures"));
       //    testData.Add("Alarm.Trigger.devices", nodeTrigger.GetAttrVal("devices"));

       //    var nodeActions = nodeAlarmInfo.GetChildNode("Actions");
       //    testData.Add("Alarm.Actions.mail-from", nodeActions.GetAttrVal("mail-from"));
       //    testData.Add("Alarm.Actions.mail-to", nodeActions.GetAttrVal("mail-to"));
       //    testData.Add("Alarm.Actions.mail-subject", nodeActions.GetAttrVal("mail-subject"));
       //    testData.Add("Alarm.Actions.mail-content", nodeActions.GetAttrVal("mail-content"));

       //    return testData;
       //}

       //private Dictionary<string, object> GetTestDataOfTestTS4_15_01()
       //{
       //    var testCaseName = "TS4_15_01";
       //    var xmlUtility = new XmlUtility(Settings.TC4_TEST_DATA_FILE_PATH);
       //    var testData = new Dictionary<string, object>();

       //    testData.Add("Geozone", xmlUtility.GetSingleNodeText(string.Format(Settings.TC4_XPATH_PREFIX, testCaseName, "Geozone")));

       //    var nodeAlarmInfo = xmlUtility.GetSingleNode(string.Format(Settings.TC4_XPATH_PREFIX, testCaseName, "AlarmInfo"));
       //    testData.Add("Alarm.name", nodeAlarmInfo.GetAttrVal("name"));
       //    testData.Add("Alarm.type", nodeAlarmInfo.GetAttrVal("type"));
       //    testData.Add("Alarm.action", nodeAlarmInfo.GetAttrVal("action"));

       //    var nodeGeneral = nodeAlarmInfo.GetChildNode("General");
       //    testData.Add("Alarm.General.auto-acknowledge", nodeGeneral.GetAttrVal("auto-acknowledge"));
       //    testData.Add("Alarm.General.refresh-rate", nodeGeneral.GetAttrVal("refresh-rate"));

       //    var nodeTrigger = nodeAlarmInfo.GetChildNode("Trigger");
       //    testData.Add("Alarm.Trigger.message", nodeTrigger.GetAttrVal("message"));
       //    testData.Add("Alarm.Trigger.failures", nodeTrigger.GetAttrVal("failures"));
       //    testData.Add("Alarm.Trigger.devices", nodeTrigger.GetAttrVal("devices"));
       //    testData.Add("Alarm.Trigger.radius", nodeTrigger.GetAttrVal("radius"));
       //    testData.Add("Alarm.Trigger.threshold", nodeTrigger.GetAttrVal("threshold"));

       //    var nodeActions = nodeAlarmInfo.GetChildNode("Actions");
       //    testData.Add("Alarm.Actions.mail-from", nodeActions.GetAttrVal("mail-from"));
       //    testData.Add("Alarm.Actions.mail-to", nodeActions.GetAttrVal("mail-to"));
       //    testData.Add("Alarm.Actions.mail-subject", nodeActions.GetAttrVal("mail-subject"));
       //    testData.Add("Alarm.Actions.mail-content", nodeActions.GetAttrVal("mail-content"));

       //    return testData;
       //}

       //private Dictionary<string, object> GetTestDataOfTestTS4_16_01()
       //{
       //    var testCaseName = "TS4_16_01";
       //    var xmlUtility = new XmlUtility(Settings.TC4_TEST_DATA_FILE_PATH);
       //    var testData = new Dictionary<string, object>();

       //    testData.Add("Geozone", xmlUtility.GetSingleNodeText(string.Format(Settings.TC4_XPATH_PREFIX, testCaseName, "Geozone")));

       //    var nodeAlarmInfo = xmlUtility.GetSingleNode(string.Format(Settings.TC4_XPATH_PREFIX, testCaseName, "AlarmInfo"));
       //    testData.Add("Alarm.name", nodeAlarmInfo.GetAttrVal("name"));
       //    testData.Add("Alarm.type", nodeAlarmInfo.GetAttrVal("type"));
       //    testData.Add("Alarm.action", nodeAlarmInfo.GetAttrVal("action"));

       //    var nodeGeneral = nodeAlarmInfo.GetChildNode("General");
       //    testData.Add("Alarm.General.auto-acknowledge", nodeGeneral.GetAttrVal("auto-acknowledge"));
       //    testData.Add("Alarm.General.refresh-rate", nodeGeneral.GetAttrVal("refresh-rate"));

       //    var nodeTrigger = nodeAlarmInfo.GetChildNode("Trigger");
       //    testData.Add("Alarm.Trigger.message", nodeTrigger.GetAttrVal("message"));
       //    testData.Add("Alarm.Trigger.failures", nodeTrigger.GetAttrVal("failures"));
       //    testData.Add("Alarm.Trigger.devices", nodeTrigger.GetAttrVal("devices"));
       //    testData.Add("Alarm.Trigger.critical-failure-ratio", nodeTrigger.GetAttrVal("critical-failure-ratio"));

       //    var nodeActions = nodeAlarmInfo.GetChildNode("Actions");
       //    testData.Add("Alarm.Actions.mail-from", nodeActions.GetAttrVal("mail-from"));
       //    testData.Add("Alarm.Actions.mail-to", nodeActions.GetAttrVal("mail-to"));
       //    testData.Add("Alarm.Actions.mail-subject", nodeActions.GetAttrVal("mail-subject"));
       //    testData.Add("Alarm.Actions.mail-content", nodeActions.GetAttrVal("mail-content"));

       //    return testData;
       //}

       //private Dictionary<string, object> GetTestDataOfTestTS4_17_01()
       //{
       //    var testCaseName = "TS4_17_01";
       //    var xmlUtility = new XmlUtility(Settings.TC4_TEST_DATA_FILE_PATH);
       //    var testData = new Dictionary<string, object>();

       //    var nodeAlarmInfo = xmlUtility.GetSingleNode(string.Format(Settings.TC4_XPATH_PREFIX, testCaseName, "AlarmInfo"));
       //    testData.Add("Alarm.type", nodeAlarmInfo.GetAttrVal("type"));
       //    testData.Add("Alarm.action", nodeAlarmInfo.GetAttrVal("action"));

       //    var nodeGeneral = nodeAlarmInfo.GetChildNode("General");
       //    testData.Add("Alarm.General.auto-acknowledge", nodeGeneral.GetAttrVal("auto-acknowledge"));
       //    testData.Add("Alarm.General.refresh-rate", nodeGeneral.GetAttrVal("refresh-rate"));

       //    var nodeTrigger = nodeAlarmInfo.GetChildNode("Trigger");
       //    testData.Add("Alarm.Trigger.message", nodeTrigger.GetAttrVal("message"));
       //    testData.Add("Alarm.Trigger.metering", nodeTrigger.GetAttrVal("metering"));
       //    testData.Add("Alarm.Trigger.ignore-operator", nodeTrigger.GetAttrVal("ignore-operator"));
       //    testData.Add("Alarm.Trigger.ignore-value", nodeTrigger.GetAttrVal("ignore-value"));
       //    testData.Add("Alarm.Trigger.analysis-period", nodeTrigger.GetAttrVal("analysis-period"));
       //    testData.Add("Alarm.Trigger.analytic-mode", nodeTrigger.GetAttrVal("analytic-mode"));
       //    testData.Add("Alarm.Trigger.percentage-difference-trigger", nodeTrigger.GetAttrVal("percentage-difference-trigger"));

       //    var nodeActions = nodeAlarmInfo.GetChildNode("Actions");
       //    testData.Add("Alarm.Actions.mail-from", nodeActions.GetAttrVal("mail-from"));
       //    testData.Add("Alarm.Actions.mail-to", nodeActions.GetAttrVal("mail-to"));
       //    testData.Add("Alarm.Actions.mail-subject", nodeActions.GetAttrVal("mail-subject"));                    

       //    return testData;
       //}

       //private Dictionary<string, object> GetTestDataOfTestTS4_18_01()
       //{
       //    var testCaseName = "TS4_18_01";
       //    var xmlUtility = new XmlUtility(Settings.TC4_TEST_DATA_FILE_PATH);
       //    var testData = new Dictionary<string, object>();

       //    testData.Add("Geozone", xmlUtility.GetSingleNodeText(string.Format(Settings.TC4_XPATH_PREFIX, testCaseName, "Geozone")));

       //    var nodeAlarmInfo = xmlUtility.GetSingleNode(string.Format(Settings.TC4_XPATH_PREFIX, testCaseName, "AlarmInfo"));
       //    testData.Add("Alarm.name", nodeAlarmInfo.GetAttrVal("name"));
       //    testData.Add("Alarm.type", nodeAlarmInfo.GetAttrVal("type"));
       //    testData.Add("Alarm.action", nodeAlarmInfo.GetAttrVal("action"));

       //    var nodeGeneral = nodeAlarmInfo.GetChildNode("General");
       //    testData.Add("Alarm.General.auto-acknowledge", nodeGeneral.GetAttrVal("auto-acknowledge"));
       //    testData.Add("Alarm.General.refresh-rate", nodeGeneral.GetAttrVal("refresh-rate"));

       //    var nodeTrigger = nodeAlarmInfo.GetChildNode("Trigger");
       //    testData.Add("Alarm.Trigger.message", nodeTrigger.GetAttrVal("message"));
       //    testData.Add("Alarm.Trigger.devices", nodeTrigger.GetAttrVal("devices"));
       //    testData.Add("Alarm.Trigger.variables-type", nodeTrigger.GetAttrVal("variables-type"));
       //    testData.Add("Alarm.Trigger.critical-delay", nodeTrigger.GetAttrVal("critical-delay"));
       //    testData.Add("Alarm.Trigger.timestamp-mode", nodeTrigger.GetAttrVal("timestamp-mode"));
       //    testData.Add("Alarm.Trigger.critical-ratio", nodeTrigger.GetAttrVal("critical-ratio"));

       //    var nodeActions = nodeAlarmInfo.GetChildNode("Actions");
       //    testData.Add("Alarm.Actions.mail-from", nodeActions.GetAttrVal("mail-from"));
       //    testData.Add("Alarm.Actions.mail-to", nodeActions.GetAttrVal("mail-to"));
       //    testData.Add("Alarm.Actions.mail-subject", nodeActions.GetAttrVal("mail-subject"));
       //    testData.Add("Alarm.Actions.mail-content", nodeActions.GetAttrVal("mail-content"));

       //    return testData;
       //}

       //private Dictionary<string, object> GetTestDataOfTestTS4_19_01()
       //{
       //    var testCaseName = "TS4_19_01";
       //    var xmlUtility = new XmlUtility(Settings.TC4_TEST_DATA_FILE_PATH);
       //    var testData = new Dictionary<string, object>();

       //    testData.Add("Geozone", xmlUtility.GetSingleNodeText(string.Format(Settings.TC4_XPATH_PREFIX, testCaseName, "Geozone")));

       //    var nodeAlarmInfo = xmlUtility.GetSingleNode(string.Format(Settings.TC4_XPATH_PREFIX, testCaseName, "AlarmInfo"));
       //    testData.Add("Alarm.name", nodeAlarmInfo.GetAttrVal("name"));
       //    testData.Add("Alarm.type", nodeAlarmInfo.GetAttrVal("type"));
       //    testData.Add("Alarm.action", nodeAlarmInfo.GetAttrVal("action"));

       //    var nodeGeneral = nodeAlarmInfo.GetChildNode("General");
       //    testData.Add("Alarm.General.auto-acknowledge", nodeGeneral.GetAttrVal("auto-acknowledge"));
       //    testData.Add("Alarm.General.refresh-rate", nodeGeneral.GetAttrVal("refresh-rate"));

       //    var nodeTrigger = nodeAlarmInfo.GetChildNode("Trigger");
       //    testData.Add("Alarm.Trigger.message", nodeTrigger.GetAttrVal("message"));
       //    testData.Add("Alarm.Trigger.delay", nodeTrigger.GetAttrVal("delay"));
       //    testData.Add("Alarm.Trigger.controllers", nodeTrigger.GetAttrVal("controllers"));

       //    var nodeActions = nodeAlarmInfo.GetChildNode("Actions");
       //    testData.Add("Alarm.Actions.mail-from", nodeActions.GetAttrVal("mail-from"));
       //    testData.Add("Alarm.Actions.mail-to", nodeActions.GetAttrVal("mail-to"));
       //    testData.Add("Alarm.Actions.mail-subject", nodeActions.GetAttrVal("mail-subject"));
       //    testData.Add("Alarm.Actions.mail-content", nodeActions.GetAttrVal("mail-content"));

       //    return testData;
       //}

       //private Dictionary<string, object> GetTestDataOfTestTS4_20_02()
       //{
       //    var testCaseName = "TS4_20_02";
       //    var xmlUtility = new XmlUtility(Settings.TC4_TEST_DATA_FILE_PATH);
       //    var testData = new Dictionary<string, object>();

       //    var nodeAlarmInfo = xmlUtility.GetSingleNode(string.Format(Settings.TC4_XPATH_PREFIX, testCaseName, "AlarmInfo"));
       //    testData.Add("Alarm.name", nodeAlarmInfo.GetAttrVal("name"));
       //    testData.Add("Alarm.type", nodeAlarmInfo.GetAttrVal("type"));
       //    testData.Add("Alarm.action", nodeAlarmInfo.GetAttrVal("action"));

       //    var nodeGeneral = nodeAlarmInfo.GetChildNode("General");
       //    testData.Add("Alarm.General.auto-acknowledge", nodeGeneral.GetAttrVal("auto-acknowledge"));
       //    testData.Add("Alarm.General.refresh-rate", nodeGeneral.GetAttrVal("refresh-rate"));

       //    var nodeTrigger = nodeAlarmInfo.GetChildNode("Trigger");
       //    testData.Add("Alarm.Trigger.message", nodeTrigger.GetAttrVal("message"));
       //    testData.Add("Alarm.Trigger.io", nodeTrigger.GetAttrVal("io"));
       //    testData.Add("Alarm.Trigger.delay", nodeTrigger.GetAttrVal("delay"));

       //    var nodeActions = nodeAlarmInfo.GetChildNode("Actions");
       //    testData.Add("Alarm.Actions.mail-from", nodeActions.GetAttrVal("mail-from"));
       //    testData.Add("Alarm.Actions.mail-to", nodeActions.GetAttrVal("mail-to"));
       //    testData.Add("Alarm.Actions.mail-subject", nodeActions.GetAttrVal("mail-subject"));
       //    testData.Add("Alarm.Actions.mail-content", nodeActions.GetAttrVal("mail-content"));

       //    return testData;
       //}

       //private Dictionary<string, object> GetTestDataOfTestTS4_21_01()
       //{
       //    var testCaseName = "TS4_21_01";
       //    var xmlUtility = new XmlUtility(Settings.TC4_TEST_DATA_FILE_PATH);
       //    var testData = new Dictionary<string, object>();

       //    var nodeAlarmInfo = xmlUtility.GetSingleNode(string.Format(Settings.TC4_XPATH_PREFIX, testCaseName, "AlarmInfo"));
       //    testData.Add("Alarm.name", nodeAlarmInfo.GetAttrVal("name"));
       //    testData.Add("Alarm.type", nodeAlarmInfo.GetAttrVal("type"));
       //    testData.Add("Alarm.action", nodeAlarmInfo.GetAttrVal("action"));

       //    var nodeGeneral = nodeAlarmInfo.GetChildNode("General");
       //    testData.Add("Alarm.General.auto-acknowledge", nodeGeneral.GetAttrVal("auto-acknowledge"));
       //    testData.Add("Alarm.General.refresh-rate", nodeGeneral.GetAttrVal("refresh-rate"));

       //    var nodeTrigger = nodeAlarmInfo.GetChildNode("Trigger");
       //    testData.Add("Alarm.Trigger.message", nodeTrigger.GetAttrVal("message"));
       //    testData.Add("Alarm.Trigger.input-name", nodeTrigger.GetAttrVal("input-name"));
       //    testData.Add("Alarm.Trigger.input-value", nodeTrigger.GetAttrVal("input-value"));

       //    var nodeActions = nodeAlarmInfo.GetChildNode("Actions");
       //    testData.Add("Alarm.Actions.mail-from", nodeActions.GetAttrVal("mail-from"));
       //    testData.Add("Alarm.Actions.mail-to", nodeActions.GetAttrVal("mail-to"));
       //    testData.Add("Alarm.Actions.mail-subject", nodeActions.GetAttrVal("mail-subject"));
       //    testData.Add("Alarm.Actions.mail-content", nodeActions.GetAttrVal("mail-content"));

       //    return testData;
       //}

       //private Dictionary<string, object> GetTestDataOfTestTS4_22_01()
       //{
       //    var testCaseName = "TS4_22_01";
       //    var xmlUtility = new XmlUtility(Settings.TC4_TEST_DATA_FILE_PATH);
       //    var testData = new Dictionary<string, object>();

       //    var nodeAlarmInfo = xmlUtility.GetSingleNode(string.Format(Settings.TC4_XPATH_PREFIX, testCaseName, "AlarmInfo"));
       //    testData.Add("Alarm.name", nodeAlarmInfo.GetAttrVal("name"));
       //    testData.Add("Alarm.type", nodeAlarmInfo.GetAttrVal("type"));
       //    testData.Add("Alarm.action", nodeAlarmInfo.GetAttrVal("action"));

       //    var nodeGeneral = nodeAlarmInfo.GetChildNode("General");
       //    testData.Add("Alarm.General.auto-acknowledge", nodeGeneral.GetAttrVal("auto-acknowledge"));
       //    testData.Add("Alarm.General.refresh-rate", nodeGeneral.GetAttrVal("refresh-rate"));

       //    var nodeTrigger = nodeAlarmInfo.GetChildNode("Trigger");
       //    testData.Add("Alarm.Trigger.message", nodeTrigger.GetAttrVal("message"));
       //    testData.Add("Alarm.Trigger.first-io", nodeTrigger.GetAttrVal("first-io"));
       //    testData.Add("Alarm.Trigger.second-io", nodeTrigger.GetAttrVal("second-io"));
       //    testData.Add("Alarm.Trigger.operator", nodeTrigger.GetAttrVal("operator"));
       //    testData.Add("Alarm.Trigger.io-value", nodeTrigger.GetAttrVal("io-value"));

       //    var nodeActions = nodeAlarmInfo.GetChildNode("Actions");
       //    testData.Add("Alarm.Actions.mail-from", nodeActions.GetAttrVal("mail-from"));
       //    testData.Add("Alarm.Actions.mail-to", nodeActions.GetAttrVal("mail-to"));
       //    testData.Add("Alarm.Actions.mail-subject", nodeActions.GetAttrVal("mail-subject"));
       //    testData.Add("Alarm.Actions.mail-content", nodeActions.GetAttrVal("mail-content"));

       //    return testData;
       //}

       //private Dictionary<string, object> GetTestDataOfTestTS4_23_01()
       //{
       //    var testCaseName = "TS4_23_01";
       //    var xmlUtility = new XmlUtility(Settings.TC4_TEST_DATA_FILE_PATH);
       //    var testData = new Dictionary<string, object>();

       //    testData.Add("Geozone", xmlUtility.GetSingleNodeText(string.Format(Settings.TC4_XPATH_PREFIX, testCaseName, "Geozone")));

       //    var nodeAlarmInfo = xmlUtility.GetSingleNode(string.Format(Settings.TC4_XPATH_PREFIX, testCaseName, "AlarmInfo"));
       //    testData.Add("Alarm.name", nodeAlarmInfo.GetAttrVal("name"));
       //    testData.Add("Alarm.type", nodeAlarmInfo.GetAttrVal("type"));
       //    testData.Add("Alarm.action", nodeAlarmInfo.GetAttrVal("action"));

       //    var nodeGeneral = nodeAlarmInfo.GetChildNode("General");
       //    testData.Add("Alarm.General.auto-acknowledge", nodeGeneral.GetAttrVal("auto-acknowledge"));
       //    testData.Add("Alarm.General.refresh-rate", nodeGeneral.GetAttrVal("refresh-rate"));

       //    var nodeTrigger = nodeAlarmInfo.GetChildNode("Trigger");
       //    testData.Add("Alarm.Trigger.message", nodeTrigger.GetAttrVal("message"));
       //    testData.Add("Alarm.Trigger.controllers", nodeTrigger.GetAttrVal("controllers"));
       //    testData.Add("Alarm.Trigger.input-name", nodeTrigger.GetAttrVal("input-name"));
       //    testData.Add("Alarm.Trigger.input-value", nodeTrigger.GetAttrVal("input-value"));
       //    testData.Add("Alarm.Trigger.check-hours-interval", nodeTrigger.GetAttrVal("check-hours-interval"));

       //    var nodeActions = nodeAlarmInfo.GetChildNode("Actions");
       //    testData.Add("Alarm.Actions.mail-from", nodeActions.GetAttrVal("mail-from"));
       //    testData.Add("Alarm.Actions.mail-to", nodeActions.GetAttrVal("mail-to"));
       //    testData.Add("Alarm.Actions.mail-subject", nodeActions.GetAttrVal("mail-subject"));
       //    testData.Add("Alarm.Actions.mail-content", nodeActions.GetAttrVal("mail-content"));

       //    return testData;
       //}

       //private Dictionary<string, object> GetTestDataOfTestTS4_24_01()
       //{
       //    var testCaseName = "TS4_24_01";
       //    var xmlUtility = new XmlUtility(Settings.TC4_TEST_DATA_FILE_PATH);
       //    var testData = new Dictionary<string, object>();

       //    var nodeAlarmInfo = xmlUtility.GetSingleNode(string.Format(Settings.TC4_XPATH_PREFIX, testCaseName, "AlarmInfo"));
       //    testData.Add("Alarm.type", nodeAlarmInfo.GetAttrVal("type"));
       //    testData.Add("Alarm.action", nodeAlarmInfo.GetAttrVal("action"));

       //    var nodeGeneral = nodeAlarmInfo.GetChildNode("General");
       //    testData.Add("Alarm.General.auto-acknowledge", nodeGeneral.GetAttrVal("auto-acknowledge"));
       //    testData.Add("Alarm.General.refresh-rate", nodeGeneral.GetAttrVal("refresh-rate"));

       //    var nodeTrigger = nodeAlarmInfo.GetChildNode("Trigger");
       //    testData.Add("Alarm.Trigger.message", nodeTrigger.GetAttrVal("message"));
       //    testData.Add("Alarm.Trigger.metering", nodeTrigger.GetAttrVal("metering"));
       //    testData.Add("Alarm.Trigger.ignore-operator", nodeTrigger.GetAttrVal("ignore-operator"));
       //    testData.Add("Alarm.Trigger.ignore-value", nodeTrigger.GetAttrVal("ignore-value"));
       //    testData.Add("Alarm.Trigger.triggering-operator", nodeTrigger.GetAttrVal("triggering-operator"));
       //    testData.Add("Alarm.Trigger.triggering-value", nodeTrigger.GetAttrVal("triggering-value"));
       //    testData.Add("Alarm.Trigger.t1", nodeTrigger.GetAttrVal("t1"));
       //    testData.Add("Alarm.Trigger.t2", nodeTrigger.GetAttrVal("t2"));

       //    var nodeActions = nodeAlarmInfo.GetChildNode("Actions");
       //    testData.Add("Alarm.Actions.mail-from", nodeActions.GetAttrVal("mail-from"));
       //    testData.Add("Alarm.Actions.mail-to", nodeActions.GetAttrVal("mail-to"));
       //    testData.Add("Alarm.Actions.mail-subject", nodeActions.GetAttrVal("mail-subject"));
       //    testData.Add("Alarm.Actions.mail-content", nodeActions.GetAttrVal("mail-content"));

       //    return testData;
       //}

       //private Dictionary<string, object> GetTestDataOfTestTS4_25_01()
       //{
       //    var testCaseName = "TS4_25_01";
       //    var xmlUtility = new XmlUtility(Settings.TC4_TEST_DATA_FILE_PATH);
       //    var testData = new Dictionary<string, object>();

       //    var nodeAlarmInfo = xmlUtility.GetSingleNode(string.Format(Settings.TC4_XPATH_PREFIX, testCaseName, "AlarmInfo"));
       //    testData.Add("Alarm.type", nodeAlarmInfo.GetAttrVal("type"));
       //    testData.Add("Alarm.action", nodeAlarmInfo.GetAttrVal("action"));

       //    var nodeGeneral = nodeAlarmInfo.GetChildNode("General");
       //    testData.Add("Alarm.General.auto-acknowledge", nodeGeneral.GetAttrVal("auto-acknowledge"));
       //    testData.Add("Alarm.General.refresh-rate", nodeGeneral.GetAttrVal("refresh-rate"));

       //    var nodeTrigger = nodeAlarmInfo.GetChildNode("Trigger");
       //    testData.Add("Alarm.Trigger.message", nodeTrigger.GetAttrVal("message"));
       //    testData.Add("Alarm.Trigger.metering", nodeTrigger.GetAttrVal("metering"));
       //    testData.Add("Alarm.Trigger.ignore-operator", nodeTrigger.GetAttrVal("ignore-operator"));
       //    testData.Add("Alarm.Trigger.ignore-value", nodeTrigger.GetAttrVal("ignore-value"));
       //    testData.Add("Alarm.Trigger.analysis-period", nodeTrigger.GetAttrVal("analysis-period"));
       //    testData.Add("Alarm.Trigger.analytic-mode", nodeTrigger.GetAttrVal("analytic-mode"));
       //    testData.Add("Alarm.Trigger.percentage-difference-trigger", nodeTrigger.GetAttrVal("percentage-difference-trigger"));

       //    var nodeActions = nodeAlarmInfo.GetChildNode("Actions");
       //    testData.Add("Alarm.Actions.mail-from", nodeActions.GetAttrVal("mail-from"));
       //    testData.Add("Alarm.Actions.mail-to", nodeActions.GetAttrVal("mail-to"));
       //    testData.Add("Alarm.Actions.mail-subject", nodeActions.GetAttrVal("mail-subject"));
       //    testData.Add("Alarm.Actions.mail-content", nodeActions.GetAttrVal("mail-content"));

       //    return testData;
       //}

       //private Dictionary<string, object> GetTestDataOfTestTS4_25_02()
       //{
       //    var testCaseName = "TS4_25_02";
       //    var xmlUtility = new XmlUtility(Settings.TC4_TEST_DATA_FILE_PATH);
       //    var testData = new Dictionary<string, object>();

       //    var nodeAlarmInfo = xmlUtility.GetSingleNode(string.Format(Settings.TC4_XPATH_PREFIX, testCaseName, "AlarmInfo"));
       //    testData.Add("Alarm.type", nodeAlarmInfo.GetAttrVal("type"));
       //    testData.Add("Alarm.action", nodeAlarmInfo.GetAttrVal("action"));

       //    var nodeGeneral = nodeAlarmInfo.GetChildNode("General");
       //    testData.Add("Alarm.General.auto-acknowledge", nodeGeneral.GetAttrVal("auto-acknowledge"));
       //    testData.Add("Alarm.General.refresh-rate", nodeGeneral.GetAttrVal("refresh-rate"));

       //    var nodeTrigger = nodeAlarmInfo.GetChildNode("Trigger");
       //    testData.Add("Alarm.Trigger.message", nodeTrigger.GetAttrVal("message"));
       //    testData.Add("Alarm.Trigger.metering", nodeTrigger.GetAttrVal("metering"));
       //    testData.Add("Alarm.Trigger.ignore-operator", nodeTrigger.GetAttrVal("ignore-operator"));
       //    testData.Add("Alarm.Trigger.ignore-value", nodeTrigger.GetAttrVal("ignore-value"));
       //    testData.Add("Alarm.Trigger.analytic-mode", nodeTrigger.GetAttrVal("analytic-mode"));
       //    testData.Add("Alarm.Trigger.percentage-difference-trigger", nodeTrigger.GetAttrVal("percentage-difference-trigger"));

       //    var nodeActions = nodeAlarmInfo.GetChildNode("Actions");
       //    testData.Add("Alarm.Actions.mail-from", nodeActions.GetAttrVal("mail-from"));
       //    testData.Add("Alarm.Actions.mail-to", nodeActions.GetAttrVal("mail-to"));
       //    testData.Add("Alarm.Actions.mail-subject", nodeActions.GetAttrVal("mail-subject"));
       //    testData.Add("Alarm.Actions.mail-content", nodeActions.GetAttrVal("mail-content"));

       //    return testData;
       //}

       #endregion //Comment out alarms because created all-in-one test case

       private Dictionary<string, object> GetTestDataOfTestSLV_1275()
       {
           var testCaseName = "SLV_1275";
           var xmlUtility = new XmlUtility(Settings.TC4_TEST_DATA_FILE_PATH);

           var testData = new Dictionary<string, object>();
           var alarmNode = xmlUtility.GetSingleNode(string.Format(Settings.TC4_XPATH_PREFIX, testCaseName, "Alarm"));
           testData.Add("Alarm", alarmNode);

           return testData;
       }

       private Dictionary<string, string> GetTestDataOfTestSLV_1687()
       {
           var testCaseName = "SLV_1687";
           var xmlUtility = new XmlUtility(Settings.TC4_TEST_DATA_FILE_PATH);
           var testData = new Dictionary<string, string>();

           testData.Add("ImportedAlarmName", xmlUtility.GetSingleNodeText(string.Format(Settings.TC4_XPATH_PREFIX, testCaseName, "ImportedAlarmName")));
           testData.Add("ImportedFileName", xmlUtility.GetSingleNodeText(string.Format(Settings.TC4_XPATH_PREFIX, testCaseName, "ImportedFileName")));

           return testData;
       }

       private Dictionary<string, string> GetTestDataOfTestSLV_1807()
       {
           var testCaseName = "SLV_1807";
           var xmlUtility = new XmlUtility(Settings.TC4_TEST_DATA_FILE_PATH);
           var testData = new Dictionary<string, string>();

           testData.Add("GeozoneA", xmlUtility.GetSingleNodeText(string.Format(Settings.TC4_XPATH_PREFIX, testCaseName, "GeozoneA")));
           testData.Add("GeozoneB", xmlUtility.GetSingleNodeText(string.Format(Settings.TC4_XPATH_PREFIX, testCaseName, "GeozoneB")));
           testData.Add("GeozoneC", xmlUtility.GetSingleNodeText(string.Format(Settings.TC4_XPATH_PREFIX, testCaseName, "GeozoneC")));
           testData.Add("GeozoneBImagePath", xmlUtility.GetSingleNodeText(string.Format(Settings.TC4_XPATH_PREFIX, testCaseName, "GeozoneBImagePath")));
           testData.Add("GeozoneCImagePath", xmlUtility.GetSingleNodeText(string.Format(Settings.TC4_XPATH_PREFIX, testCaseName, "GeozoneCImagePath")));

           return testData;
       }        

       private Dictionary<string, object> GetTestDataOfAlarms()
       {
           var testCaseName = "Alarms";
           var xmlUtility = new XmlUtility(Settings.TC4_TEST_DATA_FILE_PATH);

           var testData = new Dictionary<string, object>();
           testData.Add("TS4_13_01", xmlUtility.GetSingleNode(string.Format(Settings.TC4_XPATH_PREFIX, testCaseName, "TS4_13_01")));
           testData.Add("TS4_15_01", xmlUtility.GetSingleNode(string.Format(Settings.TC4_XPATH_PREFIX, testCaseName, "TS4_15_01")));
           testData.Add("TS4_16_01", xmlUtility.GetSingleNode(string.Format(Settings.TC4_XPATH_PREFIX, testCaseName, "TS4_16_01")));
           testData.Add("TS4_17_01", xmlUtility.GetSingleNode(string.Format(Settings.TC4_XPATH_PREFIX, testCaseName, "TS4_17_01")));
           testData.Add("TS4_18_01", xmlUtility.GetSingleNode(string.Format(Settings.TC4_XPATH_PREFIX, testCaseName, "TS4_18_01")));
           testData.Add("TS4_19_01", xmlUtility.GetSingleNode(string.Format(Settings.TC4_XPATH_PREFIX, testCaseName, "TS4_19_01")));
           testData.Add("TS4_20_02", xmlUtility.GetSingleNode(string.Format(Settings.TC4_XPATH_PREFIX, testCaseName, "TS4_20_02")));
           testData.Add("TS4_21_01", xmlUtility.GetSingleNode(string.Format(Settings.TC4_XPATH_PREFIX, testCaseName, "TS4_21_01")));
           testData.Add("TS4_22_01", xmlUtility.GetSingleNode(string.Format(Settings.TC4_XPATH_PREFIX, testCaseName, "TS4_22_01")));
           testData.Add("TS4_24_01", xmlUtility.GetSingleNode(string.Format(Settings.TC4_XPATH_PREFIX, testCaseName, "TS4_24_01")));
           testData.Add("TS4_25_01", xmlUtility.GetSingleNode(string.Format(Settings.TC4_XPATH_PREFIX, testCaseName, "TS4_25_01")));
           testData.Add("TS4_25_02", xmlUtility.GetSingleNode(string.Format(Settings.TC4_XPATH_PREFIX, testCaseName, "TS4_25_02")));

           testData.Add("Mail", xmlUtility.GetSingleNode(string.Format(Settings.TC4_XPATH_PREFIX, testCaseName, "Mail")));

           return testData;
       }

       /// <summary>
       /// Make sure 2 compared tables proper formatted before compared
       /// </summary>
       /// <param name="gridDataTable"></param>
       /// <param name="csvDataTable"></param>
       private void FormatDataTablesBeforeVerifying(ref DataTable gridDataTable, ref DataTable csvDataTable)
       {
           if (gridDataTable.Columns.Contains("Line #"))
           {
               gridDataTable.Columns.Remove("Line #");
           }

           foreach (DataRow row in gridDataTable.Rows)
           {
               if (gridDataTable.Columns.Contains("Faulty ratio"))
               {
                   var value = row["Faulty ratio"].ToString();
                   if (!string.IsNullOrEmpty(value)) row["Faulty ratio"] = Math.Round(decimal.Parse(value), 2);
               }
               if (gridDataTable.Columns.Contains("Critical ratio"))
               {
                   var value = row["Critical ratio"].ToString();
                   if (!string.IsNullOrEmpty(value)) row["Critical ratio"] = Math.Round(decimal.Parse(value), 2);
               }
               if (gridDataTable.Columns.Contains("Lifetime %"))
               {
                   var value = row["Lifetime %"].ToString();
                   if(!string.IsNullOrEmpty(value)) row["Lifetime %"] = Math.Round(decimal.Parse(value), 2);
               }
               if (gridDataTable.Columns.Contains("Last update"))
               {
                   var cellData = row["Last update"].ToString();
                   DateTime formattedCellData;

                   DateTime.TryParse(cellData, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out formattedCellData);

                   row["Last update"] = formattedCellData;
               }
           }

           foreach (DataRow row in csvDataTable.Rows)
           {
               if (csvDataTable.Columns.Contains("Faulty ratio"))
               {
                   var value = row["Faulty ratio"].ToString();
                   if (!string.IsNullOrEmpty(value)) row["Faulty ratio"] = string.Format("{0:N2}", Math.Round(decimal.Parse(value), 2));
               }
               if (csvDataTable.Columns.Contains("Critical ratio"))
               {
                   var value = row["Critical ratio"].ToString();
                   if (!string.IsNullOrEmpty(value)) row["Critical ratio"] = string.Format("{0:N2}", Math.Round(decimal.Parse(value), 2));
               }
               if (csvDataTable.Columns.Contains("Lifetime %"))
               {
                   var value = row["Lifetime %"].ToString();
                   if (!string.IsNullOrEmpty(value)) row["Lifetime %"] = string.Format("{0:N2}", Math.Round(decimal.Parse(value), 2));
               }
               if (csvDataTable.Columns.Contains("Last update"))
               {
                   var cellData = row["Last update"].ToString();

                   DateTime formattedCellData;
                   DateTime.TryParse(cellData.Contains("GMT") ? cellData.Substring(0, 24) : cellData, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out formattedCellData);

                   row["Last update"] = formattedCellData;
               }
           }
       }

       /// <summary>
       /// Get list of columns of a data table
       /// </summary>
       /// <param name="table"></param>
       /// <returns></returns>
       private List<string> GetListOfColumnsOfDataTable(DataTable table)
       {
           var columnList = new List<string>();

           foreach (DataColumn column in table.Columns)
           {
               columnList.Add(column.ColumnName);
           }

           return columnList;
       }

       /// <summary>
       /// Get email for alarm via email's subject
       /// </summary>
       /// <param name="emailSubject"></param>
       /// <returns>MailMessage</returns>
       private MailMessage GetEmailForAlarm(string emailSubject)
       {
           var mail = EmailUtility.GetNewEmail(emailSubject);

           return mail;
       }        

       #endregion
   }
}
