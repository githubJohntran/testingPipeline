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

namespace StreetlightVision.Tests.Acceptance
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    public class TC5Tests : TestBase
    {
        #region Variables

        #endregion //Variables

        #region Contructors

        #endregion //Contructors        

        #region Test Cases
        
        [Test, DynamicRetry]
        [Description("TS 5.1.1 Energy - Detailed energy consumption")]
        public void TS5_01_01()
        {
            var testData = GetTestDataOfTestTS5_01_01();

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.Energy);

            Step("1. Go to Energy app");
            Step("2. **Expected** Energy page is routed and loaded successfully");
            var energyPage = desktopPage.GoToApp(App.Energy) as EnergyPage;

            Step("3. Select a geozone which has no sub-geozone (e.g. Energy Data Area geozone) and note its devices");
            var selectedGeozone = testData["GeozoneWithDevicesOnly"] as string;
            energyPage.GeozoneTreeMainPanel.SelectNode(selectedGeozone);

            Step("4. **Expected** The grid displays detailed energy report");
            Step("- Grid title is name of the selected geozone");
            VerifyEqual("4. Verify geozone's name displayed in panel title", selectedGeozone, energyPage.GridPanel.GetPanelTitleText());

            Step("- From & To field are present");
            VerifyEqual("4. Verify From input field is visible", true, energyPage.GridPanel.IsFromDateInputVisible());
            VerifyEqual("4. Verify To input field is visible", true, energyPage.GridPanel.IsToDateInputVisible());

            Step("- Columns: Device, Category, Measured (kwh), Before (kwh), Burning hours, Average (W), Energy savings (%), Pollution savings (tons of CO2)");
            var gridColumnList = energyPage.GridPanel.GetListOfColumnsHeader();
            VerifyEqual("4. Verify columns displayed in grid", (List<string>)testData["ExpectedGridColumnHeaders"], gridColumnList);

            Step("- Devices of the selected geozone are present on the grid by device name (Device column)");
            var deviceListInGrid = energyPage.GridPanel.GetListOfDevices();
            var deviceListOfSelectedGeozone = energyPage.GeozoneTreeMainPanel.GetChildNodeNamesOfSelectedNode();
            VerifyEqual("4. Verify devices displayed in selected geozone & grid", deviceListInGrid, deviceListOfSelectedGeozone);

            Step("5. Set From = Today + 1 & To = Today then click Execute");
            var now = DateTime.Now;
            energyPage.GridPanel.EnterFromDateInput(now.AddDays(1));
            energyPage.GridPanel.EnterToDateInput(now);
            energyPage.GridPanel.ClickExecuteButton();
            energyPage.WaitForPreviousActionComplete();

            Step("6. **Expected** Cells are filled with 0 or empty including the total row");
            var tblGrid = energyPage.GridPanel.BuildDataTableFromGrid();
            tblGrid.Columns.Remove("Device");
            tblGrid.Columns.Remove("Category");
            foreach (DataRow row in tblGrid.Rows)
            {
                for (var columnIndex = 0; columnIndex < tblGrid.Columns.Count; columnIndex++)
                {
                    VerifyEqual(string.Format("6. Verified cell {0} is empty", tblGrid.Columns[columnIndex].ColumnName), "", row[columnIndex]);
                }
            }

            Step("7. Set From = last year(*Should be configurable *) & To = Today then click Execute");
            energyPage.GridPanel.EnterFromDateInput(new DateTime(2016, 1, 1));
            energyPage.GridPanel.EnterToDateInput(now);
            energyPage.GridPanel.ClickExecuteButton();
            energyPage.WaitForPreviousActionComplete();
            DataTable tblBodyGrid = energyPage.GridPanel.BuildDataTableFromGrid();
            DataTable tblTotalRowGrid = energyPage.GridPanel.BuildDataTableFromTotalRow();

            Step("8. Expected");
            Step(" - Cells are filled with values which could be 0 or empty");
            Step(" - Measured, Before, Burning hours, Pollution savings are aggregated in the total row");
            Step(" - Energy savings in the total row is an averaged value");          
            VerifyEnergyGridOfGeozoneHavingNoSubGeozone(energyPage, tblBodyGrid, tblTotalRowGrid);

            Info("TS 5.2.1 is being marked ignored");

            Info("TS 5.3.1 is being marked ignored");
        }

        [Test, DynamicRetry]
        [Description("TS 5.2.1 Energy - Detailed energy savings")]
        [Ignore("Ignored - too complicated to automate, considered later")]
        public void TS5_02_01()
        {
        }

        [Test, DynamicRetry]
        [Description("TS 5.3.1 Energy - Detailed CO2 savings")]
        [Ignore("Ignored - too complicated to automate, considered later")]
        public void TS5_03_01()
        {
        }
        
        [Test, DynamicRetry]
        [Description("TS 5.5.1 Energy - Aggregated values")]
        public void TS5_05_01()
        {
            var testData = GetTestDataOfTestTS5_05_01();
            var selectedGeozone = testData["GeoZoneWithSubGeozones"] as string;

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.Energy);

            Step("1. Go to Energy app");
            Step("2. **Expected** Energy page is routed and loaded successfully");
            var energyPage = desktopPage.GoToApp(App.Energy) as EnergyPage;

            Step("3. Select a geozone which has sub-geozones (e.g. the root geozone) and note its sub-geozones");
            energyPage.GeozoneTreeMainPanel.SelectNode(selectedGeozone);

            Step("4. **Expected** The grid displays aggregated energy report");
            Step("- Grid title is name of the selected geozone");
            VerifyEqual("4. Verify geozone's name displayed in panel title", testData["GeoZoneWithSubGeozones"], energyPage.GridPanel.GetPanelTitleText());

            Step("- From & To field are present");
            VerifyEqual("4. Verify From input field is visible", true, energyPage.GridPanel.IsFromDateInputVisible());
            VerifyEqual("4. Verify To input field is visible", true, energyPage.GridPanel.IsToDateInputVisible());

            Step("- Columns: GeoZone, Measured (kwh), Before (kwh), Energy savings (%), Pollution savings (tons of CO2)");
            var gridColumnList = energyPage.GridPanel.GetListOfColumnsHeader();
            VerifyEqual("4. Verify columns displayed in grid", (List<string>)testData["ExpectedGridColumnHeaders"], gridColumnList);

            Step("- Sub-geozones of the selected geozone are present on the grid by geozone name (GeoZone column)");
            var geozoneListInGrid = energyPage.GridPanel.GetListOfGeozones();
            var geozoneListOfSelectedGeozone = energyPage.GeozoneTreeMainPanel.GetChildNodeNamesOfSelectedNode(NodeType.GeoZone);
            VerifyEqual("[#1281964] 4. Verify devices displayed in selected geozone & grid", geozoneListInGrid, geozoneListOfSelectedGeozone, false);

            Step("5. Set From = Today + 1 & To = Today then click Execute");
            var now = DateTime.Now;
            energyPage.GridPanel.EnterFromDateInput(now.AddDays(1));
            energyPage.GridPanel.EnterToDateInput(now);
            energyPage.GridPanel.ClickExecuteButton();
            energyPage.WaitForPreviousActionComplete();
            energyPage.GridPanel.WaitForGridContentAvailable();

            Step("6. **Expected** Cells are filled with 0 or empty including the total row");
            DataTable tblGrid = energyPage.GridPanel.BuildDataTableFromGrid();
            foreach (DataRow row in tblGrid.Rows)
            {
                for (var columnIndex = 1; columnIndex < tblGrid.Columns.Count; columnIndex++)
                {
                    VerifyEqual(string.Format("Verified cell {0} is empty", tblGrid.Columns[columnIndex].ColumnName), "", row[columnIndex]);
                }
            }

            Step("7. Set From = last year(*Should be configurable *) & To = Today then click Execute");
            energyPage.GridPanel.EnterFromDateInput(now.AddYears(-1));
            energyPage.GridPanel.EnterToDateInput(now);
            energyPage.GridPanel.ClickExecuteButton();
            energyPage.WaitForPreviousActionComplete();
            energyPage.GridPanel.WaitForGridContentAvailable();
            DataTable tblBodyGrid = energyPage.GridPanel.BuildDataTableFromGrid();
            DataTable tblTotalRowGrid = energyPage.GridPanel.BuildDataTableFromTotalRow();

            Step("8. **Expected** Cells are filled with values which could be 0 or empty");
            VerifyEnergyGridOfGeozoneWithSubGeozones(energyPage, tblBodyGrid, tblTotalRowGrid);
        }

        [Test, DynamicRetry]
        [Description("TS 5.6.1 Dashboard - Show data for a geozone")]
        public void TS5_06_01()
        {
            var testData = GetTestDataOfTestTS5_06_01();

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.Dashboard);

            Step("1. Go to Energy app");
            Step("2. **Expected** Energy page is routed and loaded successfully");
            var dashboardPage = desktopPage.GoToApp(App.Dashboard) as DashboardPage;
            dashboardPage.WaitForChartsCompletelyDrawn();

            Step("- Left panel is geozone tree widget");
            VerifyEqual("2. Left panel is geozone tree widget", true, dashboardPage.MainGeozoneTreePanel.IsPanelVisible());

            Step("-Main panel has 3 graph widgets:");
            Step("-Energy Savings: Line chart");
            VerifyEqual("2. Verify Energy Savings chart is visible", true, dashboardPage.IsEnergySavingsChartVisible());
            VerifyEqual("2. Verify Energy Savings chart's title", "Energy Savings", dashboardPage.GetEnergySavingsChartTitleText());

            Step("-Failures: Pie chart");
            VerifyEqual("2. Verify Failures chart is visible", true, dashboardPage.IsFailuresChartVisible());
            VerifyEqual("2. Verify Failures chart chart's title", "Failures", dashboardPage.GetFailuresChartTitleText());

            Step("- Devices Lifetime: Pie chart");
            VerifyEqual("2. Verify Devices Lifetime chart is visible", true, dashboardPage.IsDeviceLifetimeChartVisible());
            VerifyEqual("2. Verify Devices Lifetime chart's title", "Devices Lifetime", dashboardPage.GetDeviceLifetimeChartTitleText());

            /**
             * Take screenshort of currently displayed charts
             */
            var currentChartImageAsBytes = dashboardPage.SaveChartsAsBytes();

            Step("3. Select root geozone then other geozones");
            var geozone1 = testData["Geozone1"] as string;
            dashboardPage.MainGeozoneTreePanel.SelectNode(geozone1);
            dashboardPage.WaitForChartsCompletelyDrawn();
            var geozone1ChartImageAsBytes = dashboardPage.SaveChartsAsBytes();
            var geozone2 = testData["Geozone2"] as string;
            dashboardPage.MainGeozoneTreePanel.SelectNode(geozone2);
            dashboardPage.WaitForChartsCompletelyDrawn();
            var geozone2ChartImageAsBytes = dashboardPage.SaveChartsAsBytes();

            Step("4. **Expected** Data in charts change accordingly to the selected geozone");
            MagickImage compareImage1 = new MagickImage(currentChartImageAsBytes);
            MagickImage compareImage2 = new MagickImage(geozone1ChartImageAsBytes);
            MagickImage compareImage3 = new MagickImage(geozone2ChartImageAsBytes);

            var verifyingResult1 = compareImage1.Compare(compareImage2, ErrorMetric.MeanAbsolute);
            var verifyingResult2 = compareImage2.Compare(compareImage3, ErrorMetric.MeanAbsolute);
            VerifyEqual(string.Format("4. Verify data in charts change accordingly to the selected geozone {0}", geozone1), true, verifyingResult1 > 0);
            VerifyEqual(string.Format("4. Verify data in charts change accordingly to the selected geozone {0}", geozone2), true, verifyingResult2 > 0);
        }       

        [Test, DynamicRetry]
        [Description("TS 5.8.1 Device Lifetime - Show data for a geozone")]
        public void TS5_08_01()
        {
            var testData = GetTestDataOfTestTS5_08_01();

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);

            desktopPage.InstallAppsIfNotExist(App.DeviceLifetime);

            Step("1. Go to Device Lifetime app");
            Step("2. **Expected** Device Lifetime page is routed and loaded successfully");
            var deviceLifetimePage = desktopPage.GoToApp(App.DeviceLifetime) as DeviceLifetimePage;

            Step("- Left panel is geozone");
            VerifyEqual("2. Verify geozone tree is visible", true, deviceLifetimePage.GeozoneTreeMainPanel.IsPanelVisible());

            Step("- Middle panel is a colorful stack of range of lifetime. Each range shows number of devices whose lifetime are in that range. E.g. 95 % -100 % (10)");
            VerifyEqual("2. Verify lifetime range chart is visible", true, deviceLifetimePage.IsLifetimeRangeChartVisible());

            var expectedRanges = new List<string> { "0% - 5%", "5% - 10%", "10% - 20%", "20% - 30%", "30% - 50%", "50% - 70%", "70% - 80%", "80% - 90%", "90% - 95%", "95% - 100%", "> 100%", "Unkown" };
            var actualRanges = deviceLifetimePage.GetLifetimeTextRanges();
            var actualRangesWithoutNumberOfDevices = actualRanges.Select(range => range.SplitAndGetAt(new string[] { "(" }, 0).TrimEnd()).ToList();
            VerifyEqual("2. Verify ranges", expectedRanges, actualRangesWithoutNumberOfDevices);

            Step("- Main panel is to display burning hours and lifetime of devices");
            VerifyEqual("2. Verify grid panel is visible", true, deviceLifetimePage.GridPanel.IsPanelVisible());

            foreach (var geozone in testData.Values)
            {
                deviceLifetimePage.GeozoneTreeMainPanel.SelectNode(geozone as string);
                // Reload number of devices in ranges
                actualRanges = deviceLifetimePage.GetLifetimeTextRanges();
                foreach (var selectedRange in actualRanges)
                {
                    var rangeWithoutNumberOfDevices = selectedRange.SplitAndGetAt(new string[] { "(" }, 0);

                    deviceLifetimePage.SelectLifetimeRange(rangeWithoutNumberOfDevices);
                    deviceLifetimePage.WaitForPreviousActionComplete();

                    var numberOfDevicesOfSelectedRange = Convert.ToInt32(selectedRange.SplitAndGetAt(new string[] { "(" }, 1).Replace(")", string.Empty));
                    var numberOfDevicesInGrid = deviceLifetimePage.GridPanel.BuildDataTableFromGrid().Rows.Count;      

                    VerifyTrue(string.Format("[SC-572] 2. Verify number of devices in {0} range and grid are equal", selectedRange), numberOfDevicesOfSelectedRange == numberOfDevicesInGrid, numberOfDevicesOfSelectedRange, numberOfDevicesInGrid);
                }
            }
        }

        #endregion //Test Cases

        #region Private methods

        /// <summary>
        /// Read test data for test case TS5_01_01
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, object> GetTestDataOfTestTS5_01_01()
        {
            var testCaseName = "TS5_1_1";
            var xmlUtility = new XmlUtility(Settings.TC5_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, object>();

            testData.Add("GeozoneWithDevicesOnly", xmlUtility.GetSingleNodeText(string.Format(Settings.TC5_XPATH_PREFIX, testCaseName, "GeozoneWithDevicesOnly")));
            testData.Add("ExpectedGridColumnHeaders", xmlUtility.GetChildNodesText(string.Format(Settings.TC5_XPATH_PREFIX, testCaseName, "ExpectedGridColumnHeaders")));

            return testData;

        }

        /// <summary>
        /// Read test data for test case TS5_05_01
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, object> GetTestDataOfTestTS5_05_01()
        {
            var testCaseName = "TS5_5_1";
            var xmlUtility = new XmlUtility(Settings.TC5_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, object>();

            testData.Add("GeoZoneWithSubGeozones", xmlUtility.GetSingleNodeText(string.Format(Settings.TC5_XPATH_PREFIX, testCaseName, "GeoZoneWithSubGeozones")));
            testData.Add("ExpectedGridColumnHeaders", xmlUtility.GetChildNodesText(string.Format(Settings.TC5_XPATH_PREFIX, testCaseName, "ExpectedGridColumnHeaders")));

            return testData;

        }

        /// <summary>
        /// Read test data for test case TS5_06_01
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, object> GetTestDataOfTestTS5_06_01()
        {
            var testCaseName = "TS5_6_1";
            var xmlUtility = new XmlUtility(Settings.TC5_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, object>();

            testData.Add("Geozone1", xmlUtility.GetSingleNodeText(string.Format(Settings.TC5_XPATH_PREFIX, testCaseName, "Geozone1")));
            testData.Add("Geozone2", xmlUtility.GetSingleNodeText(string.Format(Settings.TC5_XPATH_PREFIX, testCaseName, "Geozone2")));

            return testData;

        }

        /// <summary>
        /// Read test data for test case TS5_07_01
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, object> GetTestDataOfTestTS5_07_01()
        {
            var testCaseName = "TS5_7_1";
            var xmlUtility = new XmlUtility(Settings.TC5_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, object>();

            testData.Add("Geozone1", xmlUtility.GetSingleNodeText(string.Format(Settings.TC5_XPATH_PREFIX, testCaseName, "Geozone1")));
            testData.Add("Geozone2", xmlUtility.GetSingleNodeText(string.Format(Settings.TC5_XPATH_PREFIX, testCaseName, "Geozone2")));

            return testData;

        }

        /// <summary>
        /// Read test data for test case TS5_08_01
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, object> GetTestDataOfTestTS5_08_01()
        {
            var testCaseName = "TS5_8_1";
            var xmlUtility = new XmlUtility(Settings.TC5_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, object>();

            testData.Add("Geozone1", xmlUtility.GetSingleNodeText(string.Format(Settings.TC5_XPATH_PREFIX, testCaseName, "Geozone1")));
            testData.Add("Geozone2", xmlUtility.GetSingleNodeText(string.Format(Settings.TC5_XPATH_PREFIX, testCaseName, "Geozone2")));

            return testData;

        }

        private static void VerifyEnergyGridOfGeozoneHavingNoSubGeozone(EnergyPage energyPage, DataTable tblBodyGrid, DataTable tblTotalRowGrid)
        {
            decimal measuredTotal = 0;
            decimal beforeTotal = 0;
            decimal burningHoursTotal = 0;
            decimal energySavingsTotal = 0;
            decimal energyAverageSavingsTotal = 0;
            decimal pollutionSavingsTotal = 0;

            decimal measuredTotalFooter = 0;
            decimal beforeTotalFooter = 0;
            decimal burningHoursTotalFooter = 0;
            decimal energySavingsTotalFooter = 0;
            decimal pollutionSavingsTotalFooter = 0;

            var vwBodyGrid = tblBodyGrid.DefaultView;
            vwBodyGrid.RowFilter = "[Energy savings (%)] <> ''";
            var numberOfRowsToCountAverage = vwBodyGrid.Count;

            foreach (DataRow row in tblBodyGrid.Rows)
            {
                measuredTotal += row["Measured (kwh)"].ToString() == string.Empty ? 0 : Convert.ToDecimal(row["Measured (kwh)"]);
                beforeTotal += row["Before (kwh)"].ToString() == string.Empty ? 0 : Convert.ToDecimal(row["Before (kwh)"]);
                burningHoursTotal += row["Burning hours"].ToString() == string.Empty ? 0 : Convert.ToDecimal(row["Burning hours"]);
                energySavingsTotal += row["Energy savings (%)"].ToString() == string.Empty ? 0 : Convert.ToDecimal(row["Energy savings (%)"]);
                pollutionSavingsTotal += row["Pollution savings (tons of CO2)"].ToString() == string.Empty ? 0 : Convert.ToDecimal(row["Pollution savings (tons of CO2)"]);
            }

            measuredTotalFooter += tblTotalRowGrid.Rows[0]["Measured (kwh)"].ToString() == string.Empty ? 0 : Convert.ToDecimal(tblTotalRowGrid.Rows[0]["Measured (kwh)"]);
            beforeTotalFooter += tblTotalRowGrid.Rows[0]["Before (kwh)"].ToString() == string.Empty ? 0 : Convert.ToDecimal(tblTotalRowGrid.Rows[0]["Before (kwh)"]);
            burningHoursTotalFooter += tblTotalRowGrid.Rows[0]["Burning hours"].ToString() == string.Empty ? 0 : Convert.ToDecimal(tblTotalRowGrid.Rows[0]["Burning hours"]);
            energySavingsTotalFooter += tblTotalRowGrid.Rows[0]["Energy savings (%)"].ToString() == string.Empty ? 0 : Convert.ToDecimal(tblTotalRowGrid.Rows[0]["Energy savings (%)"].ToString().Replace(" %", ""));
            pollutionSavingsTotalFooter += tblTotalRowGrid.Rows[0]["Pollution savings (tons of CO2)"].ToString() == string.Empty ? 0 : Convert.ToDecimal(tblTotalRowGrid.Rows[0]["Pollution savings (tons of CO2)"]);

            int exactDigit = 2;

            measuredTotal = Math.Round(measuredTotal, exactDigit);
            beforeTotal = Math.Round(beforeTotal, exactDigit);
            burningHoursTotal = Math.Round(burningHoursTotal, exactDigit);
            energyAverageSavingsTotal = numberOfRowsToCountAverage == 0 ? 0: Math.Round(energySavingsTotal / numberOfRowsToCountAverage, 1);
            pollutionSavingsTotal = Math.Round(pollutionSavingsTotal, exactDigit);

            measuredTotalFooter = Math.Round(measuredTotalFooter, exactDigit);
            beforeTotalFooter = Math.Round(beforeTotalFooter, exactDigit);
            burningHoursTotalFooter = Math.Round(burningHoursTotalFooter, exactDigit);
            energySavingsTotalFooter = Math.Round(energySavingsTotalFooter, exactDigit);
            pollutionSavingsTotalFooter = Math.Round(pollutionSavingsTotalFooter, exactDigit);

            VerifyEqual(string.Format("Verify added value = total value = {0}", measuredTotal), measuredTotal, measuredTotalFooter);
            VerifyEqual(string.Format("Verify added value = total value = {0}", beforeTotal), beforeTotal, beforeTotalFooter);
            VerifyEqual(string.Format("Verify added value = total value = {0}", burningHoursTotal), burningHoursTotal, burningHoursTotalFooter);
            VerifyEqual(string.Format("Verify added value = total value = {0}", energyAverageSavingsTotal), energyAverageSavingsTotal, energySavingsTotalFooter);
            VerifyEqual(string.Format("Verify added value = total value = {0}", pollutionSavingsTotal), pollutionSavingsTotal, pollutionSavingsTotalFooter);
        }

        private static void VerifyEnergyGridOfGeozoneWithSubGeozones(EnergyPage energyPage, DataTable tblBodyGrid, DataTable tblTotalRowGrid)
        {
            /*
             Notes: Enery Savings & Pollution Savings can be calculated correctly via grid, so skipped
             */

            decimal measuredTotal = 0;
            decimal beforeTotal = 0;
            //decimal energySavingsTotal = 0;
            //decimal energyAverageSavingsTotal = 0;
            //decimal pollutionSavingsTotal = 0;

            decimal measuredTotalFooter = 0;
            decimal beforeTotalFooter = 0;
            //decimal energySavingsTotalFooter = 0;
            //decimal pollutionSavingsTotalFooter = 0;

            //var vwBodyGrid = tblBodyGrid.DefaultView;
            //vwBodyGrid.RowFilter = "[Energy savings (%)] <> ''";
            //var numberOfRowsToCountAverage = vwBodyGrid.Count;

            foreach (DataRow row in tblBodyGrid.Rows)
            {
                measuredTotal += row["Measured (kwh)"].ToString() == string.Empty ? 0 : Convert.ToDecimal(row["Measured (kwh)"]);
                beforeTotal += row["Before (kwh)"].ToString() == string.Empty ? 0 : Convert.ToDecimal(row["Before (kwh)"]);
                //energySavingsTotal += row["Energy savings (%)"].ToString() == string.Empty ? 0 : Convert.ToDecimal(row["Energy savings (%)"]);
                //pollutionSavingsTotal += row["Pollution savings (tons of CO2)"].ToString() == string.Empty ? 0 : Convert.ToDecimal(row["Pollution savings (tons of CO2)"]);
            }

            measuredTotalFooter += tblTotalRowGrid.Rows[0]["Measured (kwh)"].ToString() == string.Empty ? 0 : Convert.ToDecimal(tblTotalRowGrid.Rows[0]["Measured (kwh)"]);
            beforeTotalFooter += tblTotalRowGrid.Rows[0]["Before (kwh)"].ToString() == string.Empty ? 0 : Convert.ToDecimal(tblTotalRowGrid.Rows[0]["Before (kwh)"]);
            //energySavingsTotalFooter += tblTotalRowGrid.Rows[0]["Energy savings (%)"].ToString() == string.Empty ? 0 : Convert.ToDecimal(tblTotalRowGrid.Rows[0]["Energy savings (%)"].ToString().Replace(" %", ""));
            //pollutionSavingsTotalFooter += tblTotalRowGrid.Rows[0]["Pollution savings (tons of CO2)"].ToString() == string.Empty ? 0 : Convert.ToDecimal(tblTotalRowGrid.Rows[0]["Pollution savings (tons of CO2)"]);

            int exactDigit = 2;

            measuredTotal = Math.Round(measuredTotal, exactDigit);
            beforeTotal = Math.Round(beforeTotal, exactDigit);
            //energyAverageSavingsTotal = Math.Round(energySavingsTotal / numberOfRowsToCountAverage, exactDigit);
            //pollutionSavingsTotal = Math.Round(pollutionSavingsTotal, exactDigit);

            measuredTotalFooter = Math.Round(measuredTotalFooter, exactDigit);
            beforeTotalFooter = Math.Round(beforeTotalFooter, exactDigit);
            //energySavingsTotalFooter = Math.Round(energySavingsTotalFooter, exactDigit);
            //pollutionSavingsTotalFooter = Math.Round(pollutionSavingsTotalFooter, exactDigit);

            VerifyEqual(string.Format("Verify added value = total value = {0}", measuredTotal), measuredTotal, measuredTotalFooter);
            VerifyEqual(string.Format("Verify added value = total value = {0}", beforeTotal), beforeTotal, beforeTotalFooter);
            //VerifyEqual(string.Format("Verify added value = total value = {0}", energyAverageSavingsTotal), energyAverageSavingsTotal, energySavingsTotalFooter);
            //VerifyEqual(string.Format("Verify added value = total value = {0}", pollutionSavingsTotal), pollutionSavingsTotal, pollutionSavingsTotalFooter);
        }

        #endregion
    }
}