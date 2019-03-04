using ImageMagick;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using StreetlightVision.Extensions;
using StreetlightVision.Models;
using StreetlightVision.Pages;
using StreetlightVision.Pages.UI;
using StreetlightVision.Tests.Smoke;
using StreetlightVision.Utilities;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace StreetlightVision.Tests.Acceptance
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    public class TC3Tests : TestBase
    {
        #region Variables

        private readonly string _dataHistoryGraphExportedFilePattern = "*DataHistory-Export*.csv";
        private const int REPORT_WAIT_MINUTES = 5;
        private const double MILISECONDS_LATENCY = 15000;

        #endregion //Variables

        #region Contructors

        #endregion //Contructors        

        #region Test Cases
        
        [Test, DynamicRetry]
        [Description("TS 3.1.1 Data History - Go to app")]
        [NonParallelizable]
        public void TS3_01_01()
        {
            var testData = GetTestDataOfTestTS3_01_01();
            var controllerId = testData["ControllerId"].ToString();
            var controllerName = testData["ControllerName"].ToString();           
            var searchAttributeValue = testData["SearchAttributeValue"].ToString();
            var inexistingUniqueAddress = testData["InexistingUniqueAddress"].ToString();
            var existingUniqueAddress = testData["ExistingUniqueAddress"].ToString();
            var clickDeviceRequest = testData["ClickDeviceRequest"].ToString();
            var exportFilePattern = testData["ExportFilePattern"].ToString();
            var dicAttributes = testData["Attributes"] as Dictionary<string,string>;
            var newGeozone = SLVHelper.GenerateUniqueName("GZN30101");
            var streetlight = SLVHelper.GenerateUniqueName("SL30101");
            var metertings = new List<string> { "Mains current", "Lamp current", "Lamp voltage (V)" };
            var failures = new List<string> { "High voltage", "Lamp failure" };
            var typeOfEquipment = "ABEL-Vigilon A[Dimmable ballast]";

            Step("**** Precondition ****");
            Step(" - Login to the system successfully");
            Step(" - Create a testing geozone containing a streetlight");
            Step("  + Simulate data for 3 Meterings attributes (LampCurrent, LampVoltage, Current) and 2 Failures (HighVoltage, LampFailure) randomly with eventTime= yesterday datetime");
            Step("**** Precondition ****\n");

            Step("-> Create data for testing");
            DeleteGeozones("GZN30101*");
            CreateNewGeozone(newGeozone);
            CreateNewDevice(DeviceType.Streetlight, streetlight, controllerId, newGeozone, typeOfEquipment);

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.DataHistory);

            var eventTime = Settings.GetServerTime().AddDays(-1);
            Step("-> Simulate data for 3 Meterings attributes (LampCurrent, LampVoltage, Current) and 2 Failures (HighVoltage, LampFailure) randomly with eventTime= yesterday datetime");
            var value = SLVHelper.GenerateStringInteger(999);
            var request = SetValueToDevice(controllerId, streetlight, "LampCurrent", value, eventTime);
            VerifyEqual(string.Format("-> Verify the request is sent successfully (attribute: {0}, value: {1})", "LampCurrent", value), true, request);
            value = SLVHelper.GenerateStringInteger(999);
            request = SetValueToDevice(controllerId, streetlight, "LampVoltage", value, eventTime);
            VerifyEqual(string.Format("-> Verify the request is sent successfully (attribute: {0}, value: {1})", "LampVoltage", value), true, request);
            value = SLVHelper.GenerateStringInteger(999);
            request = SetValueToDevice(controllerId, streetlight, "Current", value, eventTime);
            VerifyEqual(string.Format("-> Verify the request is sent successfully (attribute: {0}, value: {1})", "Current", value), true, request);
            request = SetValueToDevice(controllerId, streetlight, "HighVoltage", true, eventTime);
            VerifyEqual(string.Format("-> Verify the request is sent successfully (attribute: {0}, value: {1})", "HighVoltage", true), true, request);            
            request = SetValueToDevice(controllerId, streetlight, "LampFailure", true, eventTime);
            VerifyEqual(string.Format("-> Verify the request is sent successfully (attribute: {0}, value: {1})", "LampFailure", true), true, request);

            #region TS 3.1.1

            Info("TS 3.1.1");

            Step("1. Go to Data History page from Desktop page or App Switch");
            Step("2. Expected Data History page is routed and loaded successfully");
            var startTime = DateTime.Now;
            var dataHistoryPage = desktopPage.GoToApp(App.DataHistory) as DataHistoryPage;
            var loadingTime = (DateTime.Now - startTime).TotalSeconds;

            Step("3. Log the time to load Data History page in running result: {0}", (int)loadingTime);

            #endregion //TS 3.1.1

            #region TS 3.2.1

            Info("TS 3.2.1");

            Step("1. Select the root geozone");
            var geozoneTreeMainPanel = dataHistoryPage.GeozoneTreeMainPanel;
            geozoneTreeMainPanel.SelectNode(Settings.RootGeozoneName);

            Step("2. Expand Search field below geozone tree and select \"Unique address\" attribute");
            geozoneTreeMainPanel.ClickExpandSearchButton();
            geozoneTreeMainPanel.SelectAttributeDropDown(searchAttributeValue);

            Step("3. Enter an inexisting unique address into Search text field then click Search icon");
            geozoneTreeMainPanel.EnterSearchTextInput(inexistingUniqueAddress);
            geozoneTreeMainPanel.ClickSearchButton();
            dataHistoryPage.WaitForPreviousActionComplete();

            Step("4. Expected Search result widget shows \"No result\". There is not any device found");
            var searchResultsGeozonePanel = dataHistoryPage.GeozoneTreeMainPanel.SearchResultsGeozonePanel;
            VerifyEqual("[TS3.2.1] 4. Verify no device found", true, dataHistoryPage.GeozoneTreeMainPanel.SearchResultsGeozonePanel.IsSearchResultMessageDisplayed());

            Step("5. Enter an existing unique address into Search text field then click Search icon");
            geozoneTreeMainPanel.EnterSearchTextInput(existingUniqueAddress);
            geozoneTreeMainPanel.ClickSearchButton();
            dataHistoryPage.WaitForPreviousActionComplete();

            Step("6. Expected Search results widget appears with only 1 device found");
            var searchDevices = dataHistoryPage.GeozoneTreeMainPanel.SearchResultsGeozonePanel.GetListOfSearchResult("Equipment");
            VerifyEqual("[TS3.2.1] 6. Search results widget appears with only 1 device found", 1, searchDevices.Count);

            #endregion //TS 3.2.1

            #region TS 3.3.1

            Info("TS 3.3.1");

            Step("1. Click on the device in Search results widget");
            searchResultsGeozonePanel.SelectRandomFoundDevice();
            dataHistoryPage.WaitForPreviousActionComplete();

            Step("2. Expected 'Last value' panel appears with 2 tabs: Meterings & Failures");
            dataHistoryPage.LastValuesPanel.WaitForPanelLoaded();
            var actualTabsListText = dataHistoryPage.LastValuesPanel.GetListOfTabs();
            var expectedTabsListText = new List<string>() { "Meterings", "Failures" };
            VerifyEqual("[TS3.3.1] 2. Verify device widget on the left appears which 2 tabs: Meterings and Failures as in pictures", expectedTabsListText, actualTabsListText);

            Step("3. Close 'Last value' panel");
            dataHistoryPage.LastValuesPanel.ClickBackToolbarButton();
            dataHistoryPage.LastValuesPanel.WaitForPanelClosed();

            Step("4. Expected 'Last value' panel is closed");
            VerifyTrue("[TS3.3.1] 4. Verify 'Last value' panel is closed", dataHistoryPage.LastValuesPanel.IsPanelDisplayed() == false, "'Last value' panel is closed", "'Last value' panel is displayed");

            Step("5. Close Search results panel");
            searchResultsGeozonePanel.ClickBackButton();
            searchResultsGeozonePanel.WaitForPanelClosed();

            Step("6. Expected 'Search result' panel is closed");
            VerifyTrue("[TS3.3.1] 6. Verify 'Search result' panel is closed", dataHistoryPage.GeozoneTreeMainPanel.IsSearchResultPanelDisplayed() == false, "'Search result' panel is closed", "'Search result' panel is displayed");

            #endregion //TS 3.3.1

            #region TS 3.4.1, TS 3.5.1

            Info("TS 3.4.1");

            Step("1. Select the testing geozone");
            geozoneTreeMainPanel.SelectNode(newGeozone);

            Step("2. Expected");
            Step("   o	The selected geozone expands and shown its devices");
            var childNodes = geozoneTreeMainPanel.GetChildNodeNamesOfSelectedNode();
            VerifyEqual("[TS3.4.1] 2. Verify the selected geozone expands and shown its devices", 1, childNodes.Count);

            Step("   o	The selected geozone's devices are shown in the grid of main panel");
            var gridDevices = dataHistoryPage.GridPanel.GetListOfDevices();
            VerifyEqual("[TS3.4.1] 2. The selected geozone's devices are shown in the grid of main panel", childNodes, gridDevices, false);

            Step("3. Select the testing streetlight");
            geozoneTreeMainPanel.SelectNode(streetlight);
            dataHistoryPage.LastValuesPanel.WaitForPanelLoaded();

            Step("4. Expected 'Last value' panel appears with title is the name of the selected device");
            var lastValuesPanelTitle = dataHistoryPage.LastValuesPanel.GetSelectedDeviceText();
            VerifyEqual(string.Format("[TS3.4.1] 4. Verify 'Last value' panel appears with title is the name of the selected device {0}", streetlight), streetlight, lastValuesPanelTitle);

            Step("5. Select a value from 'Last value' panel (e.g. Mains current)");
            byte[] graph1ImageAsBytes = null;
            byte[] graph2ImageAsBytes = null;         
            var expectedAttributes = new List<string>();
            var firstAttribute = metertings.PickRandom();
            metertings.Remove(firstAttribute);
            expectedAttributes.Add(firstAttribute);
            graph1ImageAsBytes = dataHistoryPage.CreateFakeChartAsBytes();
            dataHistoryPage.LastValuesPanel.SelectMeteringsAttribute(firstAttribute);
            Wait.ForSeconds(4);
            if (dataHistoryPage.IsLoaderSpinDisplayed())
            {
                Warning("[SC-1015] Unable to display graphs in Data History (Spinning wheel is disappeared)");
                return;
            }

            Step("6. Expected");
            Step("   o	A graph appears in main panel with title is name of the selected device");
            Step("   o	From & To fields are present");
            Step("   o	Inside the graph is graph header containing only one label which is name of the selected value (e.g. Mains current)");
            Step("   o	Under graph header is drawing area");
            graph2ImageAsBytes = dataHistoryPage.GraphPanel.SaveChartsAsBytes();
            var selectedDevicesGraph = dataHistoryPage.GraphPanel.GetSelectedDevices();
            VerifyEqual(string.Format("[TS3.4.1] 6. Verify A graph appears in main panel with title is name of the selected device {0}", streetlight), true, selectedDevicesGraph.Contains(streetlight));
            VerifyEqual("[TS3.4.1] 6. Verify From date input field is visible", true, dataHistoryPage.GridPanel.IsFromDateInputVisible());
            VerifyEqual("[TS3.4.1] 6. Verify From time input field is visible", true, dataHistoryPage.GridPanel.IsFromTimeInputVisible());
            VerifyEqual("[TS3.4.1] 6. Verify To date input field is visible", true, dataHistoryPage.GridPanel.IsToDateInputVisible());
            VerifyEqual("[TS3.4.1] 6. Verify To time input field is visible", true, dataHistoryPage.GridPanel.IsToTimeInputVisible());
            var currentSelectedValues = dataHistoryPage.GraphPanel.GetSelectedValues(streetlight);
            VerifyEqual(string.Format("[TS3.4.1] 6. Verify in graph header, selected attribtue '{0}' is displayed", firstAttribute), true, currentSelectedValues.Any(e => e == firstAttribute));
            VerifyEqual("[TS3.4.1] 6. Verify graph panel is visible", true, dataHistoryPage.GraphPanel.IsPanelVisible());
            Verify2GraphsAsBytes(graph1ImageAsBytes, graph2ImageAsBytes, string.Format("Selected attribtue '{0}'", firstAttribute));

            Step("7. Select more 3 other values from Last value panel: 1 from Meterings tab, 2 from Failures tab");
            graph1ImageAsBytes = dataHistoryPage.GraphPanel.SaveChartsAsBytes();
            var attribute = metertings.PickRandom();
            metertings.Remove(attribute);
            expectedAttributes.Add(attribute);
            dataHistoryPage.LastValuesPanel.SelectMeteringsAttribute(attribute);
            dataHistoryPage.WaitForPreviousActionComplete();
            dataHistoryPage.LastValuesPanel.SelectTab("Failures");
            foreach (var failure in failures)
            {
                dataHistoryPage.LastValuesPanel.SelectFailuresAttribute(failure);
                dataHistoryPage.WaitForPreviousActionComplete();
                expectedAttributes.Add(failure);
            }

            Step("8. Expected Graph header appends 3 labels which are name of the selected values. Graph content changes");
            graph2ImageAsBytes = dataHistoryPage.GraphPanel.SaveChartsAsBytes();
            currentSelectedValues = dataHistoryPage.GraphPanel.GetSelectedValues(streetlight);
            VerifyEqual("[TS3.4.1] 8. Verify Graph header appends 3 labels which are name of the selected values", expectedAttributes, currentSelectedValues, false);
            Verify2GraphsAsBytes(graph1ImageAsBytes, graph2ImageAsBytes, string.Format("Selected attribtues '{0}'", string.Join(", ", expectedAttributes.ToArray())));

            Step("9. Select the 5th value");
            graph1ImageAsBytes = dataHistoryPage.GraphPanel.SaveChartsAsBytes();
            attribute = metertings.PickRandom();
            metertings.Remove(attribute);
            dataHistoryPage.LastValuesPanel.SelectTab("Meterings");
            dataHistoryPage.LastValuesPanel.SelectMeteringsAttribute(attribute);
            dataHistoryPage.WaitForPreviousActionComplete();

            Step("10. Expected The first selected value in graph header is removed, the 5th value is appended. Graph content changes");
            graph2ImageAsBytes = dataHistoryPage.GraphPanel.SaveChartsAsBytes();
            currentSelectedValues = dataHistoryPage.GraphPanel.GetSelectedValues(streetlight);
            VerifyEqual("[TS3.4.1] 10. Verify The first selected value in graph header is removed", true, !currentSelectedValues.Contains(firstAttribute));
            Verify2GraphsAsBytes(graph1ImageAsBytes, graph2ImageAsBytes, string.Format("Selected attribtues '{0}'", string.Join(", ", expectedAttributes.ToArray())));
            
            Step("11. Fill From & To with current date time for both then click Execute and note the graphs");
            var now = Settings.GetServerTime();
            dataHistoryPage.GridPanel.EnterFromDateInput(now);
            dataHistoryPage.GridPanel.EnterToDateInput(now);
            dataHistoryPage.GridPanel.ClickExecuteButton();
            dataHistoryPage.WaitForPreviousActionComplete();
            graph1ImageAsBytes = dataHistoryPage.GraphPanel.SaveChartsAsBytes();

            Step("12. Fill From & To (current date time) fields with 2 years of time period then click Execute");
            dataHistoryPage.GridPanel.EnterFromDateInput(now.AddYears(-2));
            dataHistoryPage.GridPanel.EnterToDateInput(now);
            dataHistoryPage.GridPanel.ClickExecuteButton();
            dataHistoryPage.WaitForPreviousActionComplete();

            Step("13. Expected Graphs change");
            graph2ImageAsBytes = dataHistoryPage.GraphPanel.SaveChartsAsBytes();
            Verify2GraphsAsBytes(graph1ImageAsBytes, graph2ImageAsBytes, "Fill From & To (current date time) fields with 2 years of time period");

            Info("TS 3.5.1");
            Step("1. Mouse-hover on a value which has value");
            dataHistoryPage.LastValuesPanel.SelectTab("Meterings");
            var meteringsAttributesWithValue = dataHistoryPage.LastValuesPanel.GetMeteringsAttributesWithValue();
            var expectedMeteringsHoverAttribute = meteringsAttributesWithValue.FirstOrDefault();
            dataHistoryPage.LastValuesPanel.MoveHoverMeteringsAttribute(expectedMeteringsHoverAttribute);

            Step("2. Expected A tooltip displays with last update date time (e.g. 8/2/2016 11:40:00 AM) (Automation checks by verifying value of title attribute in html tag)");
            var actualMeteringsHoverAttribute = dataHistoryPage.LastValuesPanel.GetMeteringsHoverAttribute();
            var meteringsTooltipAttribute = dataHistoryPage.LastValuesPanel.GetMeteringsTooltipAttribute(expectedMeteringsHoverAttribute);
            var date = new DateTime();
            VerifyTrue("2. Verify datetime format is correct", DateTime.TryParse(meteringsTooltipAttribute, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out date) == true, "Datetime format is correct", "Datetime format is not correct");
            VerifyEqual(string.Format("Verify mouse-hover attribute '{0}' is the correct one", expectedMeteringsHoverAttribute), expectedMeteringsHoverAttribute, actualMeteringsHoverAttribute);

            Step("3. Mouse-hover on a graph");
            Step("4. Expected Each value when hovered displays its value and last update date time (Automation should ignore this check)");

            #endregion //TS 3.4.1, TS 3.5.1

            #region TS 3.6.1

            Info("TS 3.6.1");
            Step("1. Click Export icon in each graph");
            SLVHelper.DeleteAllFilesByPattern(_dataHistoryGraphExportedFilePattern);
            dataHistoryPage.GraphPanel.ClickExportFromGraph(streetlight);
            var deviceGraphHeaders = dataHistoryPage.GraphPanel.GetSelectedValues(streetlight);
            SLVHelper.SaveDownloads();

            Step("2. Expected Each CSV file is downloaded containing values reflected in graph");
            var tblCSV = SLVHelper.BuildDataTableFromLastDownloadedCSV(_dataHistoryGraphExportedFilePattern);
            if (tblCSV.Columns.Contains("Timestamp"))
            {
                tblCSV.Columns.Remove("Timestamp");
            }

            var exportFileAttributes = new List<string>();
            foreach (var col in tblCSV.Columns)
            {
                var att = col.ToString();
                if (!string.IsNullOrWhiteSpace(att))
                    exportFileAttributes.Add(att);
            }

            VerifyEqual(string.Format("[TS3.6.1] 2. Verify number of graph labels is {0}", deviceGraphHeaders.Count), deviceGraphHeaders.Count, exportFileAttributes.Count);
            foreach (var header in deviceGraphHeaders)
            {
                VerifyTrue(string.Format("[TS3.6.1] 2. Verify attribute '{0}' existed in Data History export file", header), exportFileAttributes.Any(a => a.Equals(dicAttributes[header])), string.Format("Attribute {0} existed in Data History export file", header), String.Format("Attribute {0} did not exist in Data History export file", header));
            }

            Step("3. Close graphs");
            dataHistoryPage.GraphPanel.CloseAllGraphs();

            Step("4. Expected All graphs are closed. When the last graph is closed, From & To fields are not present any more and the grid of devices appears again");
            VerifyEqual("[TS3.6.1] 4. Verify From date input field is hidden", true, !dataHistoryPage.GridPanel.IsFromDateInputVisible());
            VerifyEqual("[TS3.6.1] 4. Verify From time input field is hidden", true, !dataHistoryPage.GridPanel.IsFromTimeInputVisible());
            VerifyEqual("[TS3.6.1] 4. Verify To date input field is hidden", true, !dataHistoryPage.GridPanel.IsToDateInputVisible());
            VerifyEqual("[TS3.6.1] 4. Verify To time input field is hidden", true, !dataHistoryPage.GridPanel.IsToTimeInputVisible());
            VerifyEqual("[TS3.6.1] 4. Verify grid panel is visible",  true, dataHistoryPage.GridPanel.IsPanelVisible());

            #endregion //TS 3.6.1            

            try
            {
                DeleteGeozone(newGeozone);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("Data History - Search with Unique address")]
        public void TS3_02_01()
        {
            Assert.Pass("This test is included in TS 3.1.1. See TS 3.1.1 for details");
        }

        [Test, DynamicRetry]
        [Description("Data History - Last value panel")]
        public void TS3_03_01()
        {
            Assert.Pass("This test is included in TS 3.1.1. See TS 3.1.1 for details");
        }

        [Test, DynamicRetry]
        [Description("Data History - Data execution and graph")]
        public void TS3_04_01()
        {
            Assert.Pass("This test is included in TS 3.1.1. See TS 3.1.1 for details");
        }

        [Test, DynamicRetry]
        [Description("Data History - Last update time")]
        public void TS3_05_01()
        {
            Assert.Pass("This test is included in TS 3.1.1. See TS 3.1.1 for details");
        }

        [Test, DynamicRetry]
        [Description("Data History - Export in graph")]
        public void TS3_06_01()
        {
            Assert.Pass("This test is included in TS 3.1.1. See TS 3.1.1 for details");
        }

        [Test, DynamicRetry]
        [Description("TS 3.7.1 Data History - Create custom report with wizard")]
        public void TS3_07_01()
        {
            var testData = GetTestDataOfTestTS3_07_01();

            var rootGeoZoneInput = testData["RootGeoZone"].ToString();
            var childGeoZoneInput = testData["ChildGeoZone"].ToString();
            var deviceListInput = testData["Devices"].ToString();
            var expectedPopupTitle = testData["ExpectedPopupTitle"] as List<string>;
            var reportPrefixInput = testData["ReportPrefix"].ToString();

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.DataHistory);

            Step("1. Go to Data History page from Desktop page or App Switch");
            Step("2. **Expected** Data History page is routed");
            var dataHistoryPage = desktopPage.GoToApp(App.DataHistory) as DataHistoryPage;

            Step("3. Select a geozone which contains only devices (e.g. \"Energy Data Area\")");
            dataHistoryPage.GridPanel.WaitForLeftFooterTextDisplayed();

            var geozoneTreeMainPanel = dataHistoryPage.GeozoneTreeMainPanel;
            geozoneTreeMainPanel.SelectNode(childGeoZoneInput);

            Step("4. **Expected**");
            Step("   o	The selected geozone expands and shown its devices ");
            var childDevicesNode = geozoneTreeMainPanel.GetChildNodeNamesOfSelectedNode();
            int nofChildrenNodes = childDevicesNode.Count;
            VerifyTrue("4. Verify the selected geozone expands and shown its devices", nofChildrenNodes > 0, "The selected geozone shows its devices", "The selected geozone does not show its devices");

            Step("   o	The selected geozone's devices are shown in the grid of main panel ");
            var gridPanel = dataHistoryPage.GridPanel;
            var gridDevices = gridPanel.GetListOfDevices();
            VerifyEqual("4. The selected geozone's devices are shown in the grid of main panel", childDevicesNode, gridDevices);

            Step("5. Click Edit button (Cog icon)");
            gridPanel.ClickEditButton();
            var searchWizardPopupPanel = dataHistoryPage.SearchWizardPopupPanel;

            Step("6. **Expected** My advanced searchs dialog appears");
            VerifyEqual("6. Verify My advanced searchs dialog appears", true, searchWizardPopupPanel.IsPanelVisible());

            Step("7. Click Create Advanced Search");
            searchWizardPopupPanel.ClickNewAdvancedSearchButton();

            Step("8. **Expected** Next step is moved to with an empty text field. Next button is invisible");
            searchWizardPopupPanel.WaitForNewSearchNameInputVisible();
            var newSearhNameInputValue = searchWizardPopupPanel.GetNewSearchNameValue().Trim();
            VerifyEqual("8. Verify Next step is moved to with an empty text field", string.Empty, newSearhNameInputValue);
            VerifyEqual("8. Verify Next button is invisible", false, searchWizardPopupPanel.IsNextButtonVisible());

            Step("9. Enter a report name in text field (Automation should create a name with current timestamp for identifying purpose)");
            var currentDate = DateTime.Now;
            var expectedReportName = string.Format("{0} {1}", reportPrefixInput, GetDateTimeString(currentDate));
            searchWizardPopupPanel.EnterNewSearchNameInput(expectedReportName);

            Step("10. **Expected** Next button is visible");
            VerifyEqual("10. Verify Next button is visible", true, searchWizardPopupPanel.IsNextButtonVisible());

            Step("11. Click Next button");
            searchWizardPopupPanel.ClickNextButton();

            Step("12. **Expected** Next step is moved to with geozone tree");
            var geozoneTreePopupPanel = searchWizardPopupPanel.GeozoneTreePopupPanel;
            VerifyEqual("12. Verify geozone tree is visible", true, geozoneTreePopupPanel.IsPanelVisible());

            Step("13. Select the geozone noted at step #3 then click Next");
            geozoneTreePopupPanel.SelectNode(childGeoZoneInput);
            searchWizardPopupPanel.ClickNextButton();

            Step("14. **Expected** Next step is moved to with list of attribute columns for selecting");
            VerifyEqual("14. Verify list of attribute columns is visible",  true, searchWizardPopupPanel.IsAttributeListMenuVisible());

            Step("15. Uncheck then checked all attributes");
            Step("16. **Expected** All attributes are un/checkable");
            searchWizardPopupPanel.UnCheckAllAttributeList();
            VerifyEqual("16. Verify All attributes are unchecked", true, searchWizardPopupPanel.AreAllAttributeListUnchecked());

            searchWizardPopupPanel.CheckAllAttributeList();
            VerifyEqual("16. Verify All attributes are checked", true, searchWizardPopupPanel.AreAllAttributeListChecked());

            Step("17. Uncheck all then select attributes \"Dimming group\", \"Lamp level feedback\", \"Mains current\", \"Mains voltage (V)\", \"Metered power (W)\", \"Power factor\" then click Next");
            searchWizardPopupPanel.UnCheckAllAttributeList();
            var checkedList = new List<string>() { "Dimming group", "Lamp level feedback", "Mains current", "Mains voltage (V)", "Metered power (W)", "Sum power factor" };
            searchWizardPopupPanel.CheckAttributeList(checkedList.ToArray());
            searchWizardPopupPanel.ClickNextButton();

            Step("18. **Expected** Next step is moved to with filters for reported devices. There is 1 filter by default with a Plus button at the end");
            VerifyEqual("18. Verify filters for reported devices is visible", true, searchWizardPopupPanel.IsFilterMenuVisible());
            VerifyEqual("18. Verify there is 1 filter by default", 1, searchWizardPopupPanel.GetFiltersCount());
            VerifyEqual("18. Verify there is Save button by default", 1, searchWizardPopupPanel.GetFilterSaveButtonsCount());

            Step("19. Click Next");
            searchWizardPopupPanel.ClickNextButton();

            Step("20. **Expected** Finish step is moved to with message \"This report contains {{number of devices of the selected geozone at step #3}} devices. Click on \"Finish\" button to generate it.\" Next button is replaced by Finish button");
            searchWizardPopupPanel.WaitForDeviceSearchCompleted();
            var expectedMessage = string.Format("{0} devices match your search criteria.", nofChildrenNodes);
            var actualMessage = searchWizardPopupPanel.GetCriteriaMessageText();
            VerifyEqual(string.Format("20. Verify search result message is: '{0}'", expectedMessage), expectedMessage, actualMessage);
            VerifyEqual("20. Verify Next button is invisible", false, searchWizardPopupPanel.IsNextButtonVisible());
            VerifyEqual("20. Verify Finish button is visible", true, searchWizardPopupPanel.IsFinishButtonVisible());

            Step("21. Click Finish");
            searchWizardPopupPanel.ClickFinishButton();

            dataHistoryPage.WaitForSearchWizardPopupPanelDisappeared();
            dataHistoryPage.WaitForPreviousActionComplete();
            geozoneTreeMainPanel.WaitForPanelLoaded();
            gridPanel.WaitForPanelLoaded();

            Step("22. **Expected** \"My advanced searchs\" dialog is closed");
            VerifyEqual("22. Verify My advanced searchs dialog is closed", false, searchWizardPopupPanel.IsPanelVisible());

            Step("   o	The geozone selected for the report is selected (highlighted) in the geozone widget ");
            var selectedGeozone = geozoneTreeMainPanel.GetSelectedNodeText().SplitAndGetAt(0);
            VerifyEqual(string.Format("22. Verify selected geozone is: '{0}'", childGeoZoneInput), childGeoZoneInput, selectedGeozone);

            Step("   o	The grid panel's title is the name of the selected geozone ");
            var gridPanelTitle = gridPanel.GetPanelTitleText();
            VerifyEqual(string.Format("22. Verify grid panel's title is {0}", childGeoZoneInput), childGeoZoneInput, gridPanelTitle);

            Step("   o	In the list of advanced searchs, there is an entry of the report which has been created. It is also being selected ");
            var activeReportName = gridPanel.GetSelectOrAddSearchValue();
            VerifyEqual(string.Format("22. Verify active entry of the report is {0}", expectedReportName), expectedReportName, activeReportName);

            Step("   o	The grid displays data for only devices of the selected geozone ");
            gridDevices = gridPanel.GetListOfDevices();
            childDevicesNode = geozoneTreeMainPanel.GetChildDeviceNamesOfSelectedNode();
            VerifyEqual("22. Verify all devices of selected geozone are displayed in grid", childDevicesNode, gridDevices);
        }
        
        [Test, DynamicRetry]
        [Description("TS 3.8.1 Data History - Create custom report without wizard")]
        public void TS3_08_01()
        {
            var testData = GetTestDataOfTestTS3_08_01();

            var rootGeoZoneInput = testData["RootGeoZone"].ToString();
            var childGeoZoneInput = testData["ChildGeoZone"].ToString();
            var deviceListInput = testData["Devices"].ToString();
            var reportPrefixInput = testData["ReportPrefix"].ToString();

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.DataHistory);

            Step("1. Go to Data History page from Desktop page or App Switch");
            Step("2. **Expected** Data History page is routed");
            var dataHistoryPage = desktopPage.GoToApp(App.DataHistory) as DataHistoryPage;
            //Check if Timestamp button is checked, click to uncheck
            dataHistoryPage.GridPanel.WaitForGridContentAvailable();

            Step("3. Select a geozone which contains only devices (e.g. \"Energy Data Area\")");
            dataHistoryPage.GridPanel.WaitForLeftFooterTextDisplayed();
            var geozoneTreeMainPanel = dataHistoryPage.GeozoneTreeMainPanel;
            geozoneTreeMainPanel.SelectNode(childGeoZoneInput);

            Step("4. **Expected**");
            Step("   o	The selected geozone expands and shown its devices ");
            var childNodes = geozoneTreeMainPanel.GetChildNodeNamesOfSelectedNode();
            VerifyTrue("4. Verify the selected geozone expands and shown its devices", childNodes.Count > 0, "The selected geozone shows its devices", "The selected geozone does not show its devices");

            Step("   o	The selected geozone's devices are shown in the grid of main panel ");
            var gridPanel = dataHistoryPage.GridPanel;
            var gridDevices = gridPanel.GetListOfDevices();
            VerifyEqual("4. The selected geozone's devices are shown in the grid of main panel", childNodes, gridDevices);

            Step("5. Drop \"Advanced Search list\" down, enter an inexisting name into Search text (e.g. Data-History-Report-{{currenttimestamp}}) then hit Enter");
            var currentDate = DateTime.Now;
            var reportName = string.Format("{0} {1}", reportPrefixInput, GetDateTimeString(currentDate));
            gridPanel.SelectSelectOrAddSearchDropDown(reportName);
            dataHistoryPage.WaitForPreviousActionComplete();
            gridPanel.WaitForPanelLoaded();

            Step("6. Expected \"Advanced Search list\" field displays \"Data - History - Report -{ { currenttimestamp} }*\" with Plus icon");
            var actualReportName = gridPanel.GetSelectOrAddSearchValue();
            var expectedReportName = string.Format("{0}*", reportName);
            VerifyEqual(string.Format("Verify before saved entry of the report is {0}", expectedReportName), expectedReportName, actualReportName);
            VerifyEqual("6. Verify Plus icon appears", true, gridPanel.IsPlusSearchDropDownButtonVisible());

            Step("7. Click Plus icon inside the list");
            gridPanel.ClickPlusSearchDropDownButton();
            dataHistoryPage.WaitForPreviousActionComplete();

            Step("8. Expected The \" * \" at the beginning of the report name disappears, Plus icon disappears and Save icon appears");
            actualReportName = gridPanel.GetSelectOrAddSearchValue();
            VerifyEqual(string.Format("Verify after saved entry of the report is {0}", reportName), reportName, actualReportName);
            VerifyEqual("8. Verify Plus icon disappears", false, gridPanel.IsPlusSearchDropDownButtonVisible());

            Step("9. Note data (devices, numbers, displayed columns) of the grid");
            var oldDevices = gridPanel.GetListOfDevices();
            var oldDisplayedColumnHeaders = gridPanel.GetListOfColumnsHeader();

            Step("10. Refresh browser then go to Data History page again");
            desktopPage = Browser.RefreshLoggedInCMS();
            dataHistoryPage = desktopPage.GoToApp(App.DataHistory) as DataHistoryPage;
            dataHistoryPage.WaitForPreviousActionComplete();

            Step("11. Drop down and select the previously-created report from the list");
            gridPanel = dataHistoryPage.GridPanel;
            gridPanel.WaitForGridContentAvailable();
            gridPanel.SelectSelectOrAddSearchDropDown(reportName);
            gridPanel.WaitForPanelLoaded();
            dataHistoryPage.WaitForPreviousActionComplete();

            Step("12. Expected Devices, numbers and displayed columns are the same with noted data at step #9");
            var newDevices = gridPanel.GetListOfDevices();
            var newDisplayedColumnHeaders = gridPanel.GetListOfColumnsHeader();
            VerifyEqual("12. Verify Devices are the same with noted data at step #9", oldDevices, newDevices);
            VerifyEqual("12. Verify displayed columns are the same with noted data at step #9", oldDisplayedColumnHeaders, newDisplayedColumnHeaders);

            Step("13. Drop down column list, un/check some columns then click Save from report list");
            gridPanel.CheckColumnsInShowHideColumnsMenu("Lamp Type", "High voltage", "Lamp current");
            dataHistoryPage.AppBar.ClickHeaderBartop();
            gridPanel.ClickSaveSearchDropDownButton();
            dataHistoryPage.WaitForPreviousActionComplete();
            oldDisplayedColumnHeaders = gridPanel.GetListOfColumnsHeader();

            Step("14. Refresh browser then go to Data History page again");
            desktopPage = Browser.RefreshLoggedInCMS();
            dataHistoryPage = desktopPage.GoToApp(App.DataHistory) as DataHistoryPage;

            Step("15. Drop down and select the previously-created report from the list");
            gridPanel = dataHistoryPage.GridPanel;
            gridPanel.WaitForGridContentAvailable();
            gridPanel.SelectSelectOrAddSearchDropDown(reportName);
            dataHistoryPage.WaitForPreviousActionComplete();
            gridPanel.WaitForPanelLoaded();
            newDisplayedColumnHeaders = gridPanel.GetListOfColumnsHeader();

            Step("16. Expected Displayed columns are the same with check columns at step #14");
            VerifyEqual("16. Verify displayed columns are the same with noted data at step #14", oldDisplayedColumnHeaders, newDisplayedColumnHeaders);
        }
        
        [Test, DynamicRetry]
        [Description("Normal Reports")]
        public void NormalReportsTest()
        {
            var testData = GetTestDataOfReports();
            var mail = testData["Mail"] as XmlNode;

            var listReports = new List<dynamic>();

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.ReportManager);

            Step("1. Go to Report Manager app");
            Step("2. Verify Report Manager page is routed and loaded successfully");
            var reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            
            #region TS 3.9.1

            Step("3. Create a report with data of TS 3.9.1");
            var report = CreateNormalReportHasPropertiesTab(reportManagerPage, mail, testData["TS3_9_1"] as XmlNode);
            listReports.Add(report);

            Step("4. Verify The newly-created report is present in grid");
            VerifyNewReportInGrid(reportManagerPage, report.Name);

            #endregion //TS 3.9.1

            #region TS 3.10.1           
            Step("--> TS 3.10.1: Removed 'Mode = Auto'");
            //Step("5. Create a report with data of TS 3.10.1");
            //report = CreateNormalReportHasPropertiesTab(reportManagerPage, mail, testData["TS3_10_1"] as XmlNode);
            //listReports.Add(report);

            //Step("6. Verify The newly-created report is present in grid");
            //VerifyNewReportInGrid(reportManagerPage, report.Name);

            #endregion //TS 3.10.1

            #region TS 3.11.1

            Step("7. Create a report with data of TS 3.11.1");
            report = CreateNormalReportHasPropertiesTab(reportManagerPage, mail, testData["TS3_11_1"] as XmlNode);
            listReports.Add(report);

            Step("8. Verify The newly-created report is present in grid");
            VerifyNewReportInGrid(reportManagerPage, report.Name);

            #endregion //TS 3.11.1

            #region TS 3.12.1

            Step("9. Create a report with data of TS 3.12.1");
            report = CreateNormalReportHasPropertiesTab(reportManagerPage, mail, testData["TS3_12_1"] as XmlNode);
            listReports.Add(report);

            Step("10. Verify The newly-created report is present in grid");
            VerifyNewReportInGrid(reportManagerPage, report.Name);

            #endregion //TS 3.12.1

            #region TS 3.13.1

            Step("11. Create a report with data of TS 3.13.1");
            report = CreateNormalReportHasPropertiesTab(reportManagerPage, mail, testData["TS3_13_1"] as XmlNode);
            listReports.Add(report);

            Step("12. Verify The newly-created report is present in grid");
            VerifyNewReportInGrid(reportManagerPage, report.Name);

            #endregion //TS 3.13.1
            
            #region TS 3.14.1            

            Step("13. Create a report with data of TS 3.14.1");
            report = CreateNormalReportHasPropertiesTab(reportManagerPage, mail, testData["TS3_14_1"] as XmlNode);
            listReports.Add(report);

            Step("14. Verify The newly-created report is present in grid");
            VerifyNewReportInGrid(reportManagerPage, report.Name);

            #endregion //TS 3.14.1            

            #region TS 3.14.2

            Step("--> TS 3.14.2: Removed 'Mode = Auto'");

            //Step("15. Create a report with data of TS 3.14.2");
            //report = CreateNormalReportHasPropertiesTab(reportManagerPage, mail, testData["TS3_14_2"] as XmlNode);
            //listReports.Add(report);

            //Step("16. Verify The newly-created report is present in grid");
            //VerifyNewReportInGrid(reportManagerPage, report.Name);

            #endregion //TS 3.14.2            

            #region TS 3.14.3            

            Step("17 Create a report with data of TS 3.14.3");
            report = CreateNormalReportHasPropertiesTab(reportManagerPage, mail, testData["TS3_14_3"] as XmlNode);
            listReports.Add(report);

            Step("18. Verify The newly-created report is present in grid");
            VerifyNewReportInGrid(reportManagerPage, report.Name);

            #endregion //TS 3.14.3           

            #region TS 3.14.4

            Step("19. Create a report with data of TS 3.14.4");
            report = CreateNormalReportHasPropertiesTab(reportManagerPage, mail, testData["TS3_14_4"] as XmlNode);
            listReports.Add(report);

            Step("20. Verify The newly-created report is present in grid");
            VerifyNewReportInGrid(reportManagerPage, report.Name);

            #endregion //TS 3.14.4           

            #region TS 3.14.5

            Step("21. Create a report with data of TS 3.14.5");
            report = CreateNormalReportHasPropertiesTab(reportManagerPage, mail, testData["TS3_14_5"] as XmlNode);
            listReports.Add(report);

            Step("22. Verify The newly-created report is present in grid");
            VerifyNewReportInGrid(reportManagerPage, report.Name);

            #endregion //TS 3.14.5            

            #region TS 3.15.1           

            Step("23. Create a report with data of TS 3.15.1");
            report = CreateNormalReportHasPropertiesTab(reportManagerPage, mail, testData["TS3_15_1"] as XmlNode);
            listReports.Add(report);

            Step("24. Verify The newly-created report is present in grid");
            VerifyNewReportInGrid(reportManagerPage, report.Name);

            #endregion //TS 3.15.1

            #region TS 3.16.1            

            Step("25. Create a report with data of TS 3.16.1");
            report = CreateNormalReportHasPropertiesTab(reportManagerPage, mail, testData["TS3_16_1"] as XmlNode);
            listReports.Add(report);

            Step("26. Verify The newly-created report is present in grid");
            VerifyNewReportInGrid(reportManagerPage, report.Name);

            #endregion //TS 3.16.1

            #region TS 3.17.1

            Step("27. Create a report with data of TS 3.17.1");
            report = CreateNormalReportHasPropertiesTab(reportManagerPage, mail, testData["TS3_17_1"] as XmlNode);
            listReports.Add(report);

            Step("28. Verify The newly-created report is present in grid");
            VerifyNewReportInGrid(reportManagerPage, report.Name);

            #endregion //TS 3.17.1            

            #region TS 3.17.2
            Step("29. Create a report with data of TS 3.17.2");
            report = CreateNormalReportHasPropertiesTab(reportManagerPage, mail, testData["TS3_17_2"] as XmlNode);
            listReports.Add(report);

            Step("30. Verify The newly-created report is present in grid");
            VerifyNewReportInGrid(reportManagerPage, report.Name);

            #endregion //TS 3.17.2

            #region TS 3.18.1           

            Step("31. Create a report with data of TS 3.18.1");
            report = CreateNormalReportHasPropertiesTab(reportManagerPage, mail, testData["TS3_18_1"] as XmlNode);
            listReports.Add(report);

            Step("32. Verify The newly-created report is present in grid");
            VerifyNewReportInGrid(reportManagerPage, report.Name);

            #endregion //TS 3.18.1

            #region TS 3.19.1            

            Step("33. Create a report with data of TS 3.19.1");
            report = CreateNormalReportHasPropertiesTab(reportManagerPage, mail, testData["TS3_19_1"] as XmlNode);
            listReports.Add(report);

            Step("34. Verify The newly-created report is present in grid");
            VerifyNewReportInGrid(reportManagerPage, report.Name);

            #endregion //TS 3.19.1

            #region TS 3.20.1

            Step("35. Create a report with data of TS 3.20.1");
            report = CreateNormalReportHasPropertiesTab(reportManagerPage, mail, testData["TS3_20_1"] as XmlNode);
            listReports.Add(report);

            Step("36. Verify The newly-created report is present in grid");
            VerifyNewReportInGrid(reportManagerPage, report.Name);

            #endregion //TS 3.20.1            

            #region TS 3.20.2

            Step("37. Create a report with data of TS 3.20.2");
            report = CreateNormalReportHasPropertiesTab(reportManagerPage, mail, testData["TS3_20_2"] as XmlNode);
            listReports.Add(report);

            Step("38. Verify The newly-created report is present in grid");
            VerifyNewReportInGrid(reportManagerPage, report.Name);

            #endregion //TS 3.20.2

            #region TS 3.21.1

            Step("39. Create a report with data of TS 3.21.1");
            report = CreateNormalReportHasPropertiesTab(reportManagerPage, mail, testData["TS3_21_1"] as XmlNode);
            listReports.Add(report);

            Step("40. Verify The newly-created report is present in grid");
            VerifyNewReportInGrid(reportManagerPage, report.Name);

            #endregion //TS 3.21.1

            #region TS 3.22.1

            Step("41. Create a report with data of TS 3.22.1");
            report = CreateNormalReportHasPropertiesTab(reportManagerPage, mail, testData["TS3_22_1"] as XmlNode);
            listReports.Add(report);

            Step("42. Verify The newly-created report is present in grid");
            VerifyNewReportInGrid(reportManagerPage, report.Name);

            #endregion //TS 3.22.1

            #region TS 3.25.1

            Step("43. Create a report with data of TS 3.25.1");
            report = CreateNormalReportHasReportTab(reportManagerPage, mail, testData["TS3_25_1"] as XmlNode);
            listReports.Add(report);

            Step("44. Verify The newly-created report is present in grid");
            VerifyNewReportInGrid(reportManagerPage, report.Name);

            #endregion //TS 3.25.1

            #region TS 3.26.1

            Step("45. Create a report with data of TS 3.26.1");
            report = CreateNormalReportHasReportTab(reportManagerPage, mail, testData["TS3_26_1"] as XmlNode);
            listReports.Add(report);

            Step("46. Verify The newly-created report is present in grid");
            VerifyNewReportInGrid(reportManagerPage, report.Name);

            #endregion //TS 3.26.1

            #region TS 3.27.1

            Step("47. Create a report with data of TS 3.27.1");
            report = CreateNormalReportHasReportTab(reportManagerPage, mail, testData["TS3_27_1"] as XmlNode);
            listReports.Add(report);

            Step("48. Verify The newly-created report is present in grid");
            VerifyNewReportInGrid(reportManagerPage, report.Name);

            #endregion //TS 3.27.1

            #region TS 3.28.1

            Step("49. Create a report with data of TS 3.28.1");
            report = CreateNormalReportHasReportTab(reportManagerPage, mail, testData["TS3_28_1"] as XmlNode);
            listReports.Add(report);

            Step("50. Verify The newly-created report is present in grid");
            VerifyNewReportInGrid(reportManagerPage, report.Name);

            #endregion //TS 3.28.1

            #region TS 3.29.1

            Step("51. Create a report with data of TS 3.29.1");
            report = CreateNormalReportHasReportTab(reportManagerPage, mail, testData["TS3_29_1"] as XmlNode);
            listReports.Add(report);

            Step("52. Verify The newly-created report is present in grid");
            VerifyNewReportInGrid(reportManagerPage, report.Name);

            #endregion //TS 3.25.1

            #region TS 3.30.1

            Step("53. Create a report with data of TS 3.30.1");
            report = CreateNormalReportHasReportTab(reportManagerPage, mail, testData["TS3_30_1"] as XmlNode);
            listReports.Add(report);

            Step("54. Verify The newly-created report is present in grid");
            VerifyNewReportInGrid(reportManagerPage, report.Name);

            #endregion //TS 3.30.1

            #region TS 3.31.1

            Step("55. Create a report with data of TS 3.31.1");
            report = CreateNormalReportHasReportTab(reportManagerPage, mail, testData["TS3_31_1"] as XmlNode);
            listReports.Add(report);

            Step("56. Verify The newly-created report is present in grid");
            VerifyNewReportInGrid(reportManagerPage, report.Name);

            #endregion //TS 3.31.1

            #region TS 3.32.1

            Step("57. Create a report with data of TS 3.32.1");
            report = CreateNormalReportHasReportTab(reportManagerPage, mail, testData["TS3_32_1"] as XmlNode);
            listReports.Add(report);

            Step("58. Verify The newly-created report is present in grid");
            VerifyNewReportInGrid(reportManagerPage, report.Name);

            #endregion //TS 3.32.1

            #region TS 3.33.1

            Step("59. Create a report with data of TS 3.33.1");
            report = CreateNormalReportHasReportTab(reportManagerPage, mail, testData["TS3_33_1"] as XmlNode);
            listReports.Add(report);

            Step("60. Verify The newly-created report is present in grid");
            VerifyNewReportInGrid(reportManagerPage, report.Name);

            #endregion //TS 3.33.1
            

            Step("61. Wait for some time");
            reportManagerPage.GeozoneTreeMainPanel.SelectNode(Settings.RootGeozoneName);

            Step("62. Verify To each created report, an email is sent to configured mail box");
            Step("63. Verify To each created report with HTML format option for email is checked, the received email is in HTML format");
            Step("64. Verify To each created report with HTML format option for email is NOT checked, the received email is NOT in HTML format");

            var tasks = new List<Task>();
            var reportsSentMail = new List<string>();
            foreach (var rep in listReports)
            {
                var task = Task.Run(() =>
                            {
                                var newMail = EmailUtility.GetNewEmail(rep.MailSubject);
                                var hasNewMail = newMail != null;
                                VerifyTrue(string.Format("64. Verify Report '{0}' has an email sent from '{1}' (Report created: {2}, Expected email revieved: {3})", rep.Name, rep.MailSubject, rep.ReportCreatedTime, rep.EmailReceivedTime), hasNewMail, "Email sent", "No email sent");
                                if (hasNewMail)
                                {
                                    reportsSentMail.Add(rep.Name);
                                    var mailSubject = newMail.Subject;
                                    var mailSender = newMail.From;
                                    VerifyTrue(string.Format("64. Verify mail subject '{0}' is as expected", rep.MailSubject), mailSubject.Contains(rep.MailSubject), rep.MailSubject, mailSubject);
                                    VerifyTrue(string.Format("64. Verify mail sender '{0}' is as expected", rep.MailSender), string.Equals(mailSender, rep.MailSender), rep.MailSender, mailSender);
                                    VerifyEqual("64. Verify mail HTML format is as expected", rep.IsHtml, newMail.IsBodyHtml);
                                }
                            });
                tasks.Add(task);
            }

            Task.WaitAll(tasks.ToArray());
            foreach (var name in reportsSentMail)
            {
                reportManagerPage.DeleteReport(name);
            }
        }

        [Test, DynamicRetry]
        [Description("Latency Reports")]
        public void LatencyReportsTest()
        {
            var testData = GetTestDataOfReports();
            var mail = testData["Mail"] as XmlNode;
            var streetlights = testData["Streetlights"] as List<DeviceModel>;
            var controller = testData["Controller"] as DeviceModel;
            var listReports = new List<dynamic>();

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.ReportManager);

            Step("1. Go to Report Manager app");
            Step("2. Verify Report Manager page is routed and loaded successfully");
            var reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;

            #region TS 3.23.1

            Step("3. Create a report with data of TS 3.23.1");
            var report = CreateLatencyReport(reportManagerPage, mail, testData["TS3_23_1"] as XmlNode);
            listReports.Add(report);

            Step("4. Verify The newly-created report is present in grid");
            VerifyNewReportInGrid(reportManagerPage, report.Name);

            #endregion //TS 3.23.1

            #region TS 3.24.1

            Step("5. Create a report with data of TS 3.24.1");
            report = CreateLatencyReport(reportManagerPage, mail, testData["TS3_24_1"] as XmlNode);
            listReports.Add(report);

            Step("6. Verify The newly-created report is present in grid");
            VerifyNewReportInGrid(reportManagerPage, report.Name);

            #endregion //TS 3.24.1

            Step("7. Send a request to simulate latency for each report (The request looks like {{slv_url}}/api/loggingmanagement/setDeviceValues?controllerStrId={0}&idOnController={1}&valueName={2}&value={3}&doLog=true&eventTime={4} where {0} is controller id, {1} is device id, {2} is value name, {3} is value of value name, {4} is event time stamp)");
            Step("8. Verify The request is sent successfully(browser displays success page)");
            foreach (var rep in listReports)
            {
                var streetlight = streetlights.PickRandom();
                SimulateLatencyRequestsToDevice(streetlight.Id, controller.Id, rep.CommandValue, MILISECONDS_LATENCY);
            }

            Step("9. Wait for some time");
            reportManagerPage.GeozoneTreeMainPanel.SelectNode(Settings.RootGeozoneName);

            Step("10. Verify To each created report, an email is sent to configured mail box");
            Step("11. Verify To each created report with HTML format option for email is checked, the received email is in HTML format");
            Step("12. Verify To each created report with HTML format option for email is NOT checked, the received email is NOT in HTML format");
            var tasks = new List<Task>();
            var reportsSentMail = new List<string>();
            foreach (var rep in listReports)
            {
                var task = Task.Run(() =>
                {
                    var newMail = EmailUtility.GetNewEmail(rep.MailSubject);
                    var hasNewMail = newMail != null;
                    VerifyTrue(string.Format("12. Verify Report '{0}' has an email sent from '{1}' (Report created: {2}, Expected email revieved: {3})", rep.Name, rep.MailSubject, rep.ReportCreatedTime, rep.EmailReceivedTime), hasNewMail, "Email sent", "No email sent");
                    if (hasNewMail)
                    {
                        reportsSentMail.Add(rep.Name);
                        var mailSubject = newMail.Subject;
                        var mailSender = newMail.From;
                        VerifyTrue(string.Format("12. Verify mail subject '{0}' is as expected", rep.MailSubject), mailSubject.Contains(rep.MailSubject), rep.MailSubject, mailSubject);
                        VerifyTrue(string.Format("12. Verify mail sender '{0}' is as expected", rep.MailSender), string.Equals(mailSender, rep.MailSender), rep.MailSender, mailSender);
                        VerifyEqual("12. Verify mail HTML format is as expected", rep.IsHtml, newMail.IsBodyHtml);
                    }
                });
                tasks.Add(task);
            }

            Task.WaitAll(tasks.ToArray());
            foreach (var name in reportsSentMail)
            {
                reportManagerPage.DeleteReport(name);
            }
        }

        [Test, DynamicRetry]
        [Description("UMSUG Reports")]
        public void UMSUGReportsTest()
        {
            var testData = GetTestDataOfReports();
            var mail = testData["Mail"] as XmlNode;
            var ftp = testData["Ftp"] as XmlNode;
            var streetlights = testData["Streetlights"] as List<DeviceModel>;
            var controller = testData["Controller"] as DeviceModel;
            var geozone = testData["Geozone"].ToString().GetChildName();
            var expectedCommandValue = 100;
            var streetlight = streetlights.PickRandom();

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.ReportManager);

            Step("1. Go to Report Manager app");
            Step("2. Verify Report Manager page is routed and loaded successfully");
            var reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;

            Step("3. Create a report with data of TS 3.34.1");
            var report = CreateUMSUGReport(reportManagerPage, mail, ftp, testData["TS3_34_1"] as XmlNode);

            Step("4. Verify The newly-created report is present in grid");
            VerifyNewReportInGrid(reportManagerPage, report.Name);

            Step("5. Send a request to simulate latency for each report (The request looks like {{slv_url}}/api/loggingmanagement/setDeviceValues?controllerStrId={0}&idOnController={1}&valueName={2}&value={3}&doLog=true&eventTime={4} where {0} is controller id, {1} is device id, {2} is value name, {3} is value of value name, {4} is event time stamp)");
            Step("6. Verify The request is sent successfully(browser displays success page)");
            var eventTimes = Settings.GetServerTime().AddDays(-1); // Should be the previous day
            SimulateUmsugRequestsToDevice(streetlight.Id, controller.Id, expectedCommandValue.ToString(), eventTimes);            

            Step("7. Wait for some time");
            Step("8. Verify To each created report, an exported file is found in configured FTP folder");
            var geoPrefix = geozone.ToLowerInvariant().Substring(0, 7);
            var yesterday = eventTimes.ToString("yyyyMMdd");
            var ftpFilePattern = string.Format("{0}{1}", geoPrefix, yesterday);

            var ftpUtility = new FtpUtility(ftp.GetAttrVal("Host"), ftp.GetAttrVal("User"), ftp.GetAttrVal("Password"), ftp.GetAttrVal("Directory"));
            var fileName = ftpUtility.WaitAndGetFileName(ftpFilePattern);
            VerifyTrue(string.Format("8. Verify '{0}' exists in FTP directory: {1} (Expected file pattern: {2}, Report created: {3}, FTP Checking: {4})", fileName, ftp.GetAttrVal("Directory"), ftpFilePattern, report.ReportCreatedTime, Settings.GetServerTime()), fileName != null, "SUCCESS", "FAILED");
            
            //Delete report
            if (fileName != null)  reportManagerPage.DeleteReport(report.Name);
        }

        [Test, DynamicRetry]
        [Description("Location Change Reports - Upload report file to Ftp server and Sending Email to user")]
        public void LocationChangeReportsTest()
        {
            var testData = GetTestDataOfReports();
            var mail = testData["Mail"] as XmlNode;
            var ftp = testData["Ftp"] as XmlNode;
            var data = testData["LocationChange"] as XmlNode;
            var geozone = SLVHelper.GenerateUniqueName("GZNLCR");
            var streetlight = SLVHelper.GenerateUniqueName("SLLCR");          
            var controllerId = data.GetAttrVal("ControllerId");
            var listReports = new List<dynamic>();

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" -  Create a new streetlight.");           
            Step("**** Precondition ****\n");

            Step("-> Create data for testing");
            DeleteGeozones("GZNLCR*");
            CreateNewGeozone(geozone);
            CreateNewDevice(DeviceType.Streetlight, streetlight, controllerId, geozone);

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.ReportManager);

            Step("1. Send the command to simulate the location change of a streetlight with");
            Step(" o lat: the new value of latitude");
            Step(" o lng: the new value of longitude");           
            var eventTimes = Settings.GetServerTime();
            var status = SetValueToDevice(controllerId, streetlight, "lat", SLVHelper.GenerateLatitude(), eventTimes);
            VerifyEqual(string.Format("1. Verify Change latitude for streetlight {0} success", streetlight), true, status);
            status = SetValueToDevice(controllerId, streetlight, "lng", SLVHelper.GenerateLongitude(), eventTimes);
            VerifyEqual(string.Format("1. Verify Change longitude for streetlight {0} success", streetlight), true, status);

            Step("2. Go to Report Manager app");
            Step("3. Verify Report Manager page is routed and loaded successfully");
            var reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.GeozoneTreeMainPanel.SelectNode(geozone);

            Step("4. Select a geozone which contains sub-geozones");
            Step("5. Create and save new report with parameters:");
            Step(" o Name: \"Location Change Report { { current time stamp} }\"");
            Step(" o Type: \"Location change report\"");
            Step(" o Scheduler tab:");
            Step("   + Periodicity: Every day");
            Step("   + Hour (HH)/ Minute: Current time + 1 minute");
            Step(" o Export tab:");
            Step("   + ftp: working parameters");
            Step("   + email: working parameters");
            Step(" o Report tab:");
            Step("   + Device Categories: Streetlight");
            Step("   + Period: 1 day");            
            var report = CreateLocationChangeReport(reportManagerPage, mail, ftp, data);
            listReports.Add(report);

            Step("6. Verify The newly-created report is presented in grid");           
            VerifyNewReportInGrid(reportManagerPage, report.Name);           

            Step("7. Wait longer than 1 minute then check FTP report folder");
            Step("8. Expected");
            Step(" o A report is uploaded to FTP folder");
            Step(" o An email of report is sent to users");
            var ftpFilePattern = report.FtpFilePattern;
            var ftpUtility = new FtpUtility(ftp.GetAttrVal("Host"), ftp.GetAttrVal("User"), ftp.GetAttrVal("Password"), ftp.GetAttrVal("Directory"));
            var fileName = ftpUtility.WaitAndGetFileName(ftpFilePattern);
            VerifyTrue(string.Format("8. Verify '{0}' exists in FTP directory: {1} ", fileName, ftp.GetAttrVal("Directory")), fileName != null, "SUCCESS", "FAILED");

            var tasks = new List<Task>();
            var reportsSentMail = new List<string>();
            var task = Task.Run(() =>
            {
                var newMail = EmailUtility.GetNewEmail(report.MailSubject);
                var hasNewMail = newMail != null;
                VerifyTrue(string.Format("8. Verify Report '{0}' has an email sent from '{1}' (Report created: {2}, Expected email revieved: {3})", report.Name, report.MailSubject, report.ReportCreatedTime, report.EmailReceivedTime), hasNewMail, "Email sent", "No email sent");
                if (hasNewMail)
                {
                    reportsSentMail.Add(report.Name);
                    var mailSubject = newMail.Subject;
                    var mailSender = newMail.From;
                    VerifyTrue(string.Format("8. Verify mail subject '{0}' is as expected", report.MailSubject), mailSubject.Contains(report.MailSubject), report.MailSubject, mailSubject);
                    VerifyTrue(string.Format("8. Verify mail sender '{0}' is as expected", report.MailSender), string.Equals(mailSender, report.MailSender), report.MailSender, mailSender);
                    VerifyEqual("8. Verify mail HTML format is as expected", report.IsHtml, newMail.IsBodyHtml);
                }
            });
            tasks.Add(task);
            Task.WaitAll(tasks.ToArray());

            try
            {
                //Delete report
                if (reportsSentMail.Any()) reportManagerPage.DeleteReport(report.Name);

                //Delete new streetlight
                DeleteGeozone(geozone);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("Generic device values [Run Once] Reports - Upload report file to Ftp server and Sending Email to user")]
        public void GenericDeviceValuesRunOnceReportsTest()
        {
            var testData = GetTestDataOfReports();
            var mail = testData["Mail"] as XmlNode;
            var ftp = testData["Ftp"] as XmlNode;
            var listReports = new List<dynamic>();

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.ReportManager);

            Step("1. Go to Report Manager app");
            Step("2. Verify Report Manager page is routed and loaded successfully");
            var reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;

            Step("3. Select a geozone GeoZones");
            reportManagerPage.GeozoneTreeMainPanel.SelectNode(Settings.RootGeozoneName);

            Step("4. Create and save new report with parameters:");
            Step(" o Name: \"Generic device value [Run Once] { { current time stamp} }\"");
            Step(" o Type: \"Generic device values [Run Once]\"");          
            Step(" o Export tab:");
            Step("   + ftp: working parameters");
            Step("   + email: working parameters");
            Step(" o Report tab:");
            Step("   + Device Categories: Select All");
            Step("   + Value: Select All");
            Step("   + Start Day: The current date");
            Step("   + Start Time: The current time");
            Step("   + End Day: The current date");
            Step("   + End Time: Start time + 2 minutes");
            var report = CreateGenericDeviceValuesRunOnceReport(reportManagerPage, mail, ftp, testData["GDVRunOnce"] as XmlNode);
            listReports.Add(report);

            Step("6. Verify The newly-created report is presented in grid");
            VerifyNewReportInGrid(reportManagerPage, report.Name);

            Step("7. Wait longer than 1 minute then check FTP report folder");
            Step("8. Expected");
            Step(" o A report is uploaded to FTP folder");
            Step(" o An email of report is sent to users");
            var ftpFilePattern = report.FtpFilePattern;
            var ftpUtility = new FtpUtility(ftp.GetAttrVal("Host"), ftp.GetAttrVal("User"), ftp.GetAttrVal("Password"), ftp.GetAttrVal("Directory"));
            var fileName = ftpUtility.WaitAndGetFileName(ftpFilePattern);
            VerifyTrue(string.Format("8. Verify '{0}' exists in FTP directory: {1} ", fileName, ftp.GetAttrVal("Directory")), fileName != null, "SUCCESS", "FAILED");

            var tasks = new List<Task>();
            var reportsSentMail = new List<string>();
            var task = Task.Run(() =>
            {
                var newMail = EmailUtility.GetNewEmail(report.MailSubject);
                var hasNewMail = newMail != null;
                VerifyTrue(string.Format("8. Verify Report '{0}' has an email sent from '{1}' (Report created: {2}, Expected email revieved: {3})", report.Name, report.MailSubject, report.ReportCreatedTime, report.EmailReceivedTime), hasNewMail, "Email sent", "No email sent");
                if (hasNewMail)
                {
                    reportsSentMail.Add(report.Name);
                    var mailSubject = newMail.Subject;
                    var mailSender = newMail.From;
                    VerifyTrue(string.Format("8. Verify mail subject '{0}' is as expected", report.MailSubject), mailSubject.Contains(report.MailSubject), report.MailSubject, mailSubject);
                    VerifyTrue(string.Format("8. Verify mail sender '{0}' is as expected", report.MailSender), string.Equals(mailSender, report.MailSender), report.MailSender, mailSender);
                    VerifyEqual("8. Verify mail HTML format is as expected", report.IsHtml, newMail.IsBodyHtml);
                }
            });
            tasks.Add(task);
            Task.WaitAll(tasks.ToArray());

            //Delete report
            if (reportsSentMail.Any()) reportManagerPage.DeleteReport(report.Name);            
        }

        #endregion //Test Cases

        #region Private methods

        #region XML Input data

        private Dictionary<string, object> GetTestDataOfReports()
        {
            var testCaseName = "Reports";
            var xmlUtility = new XmlUtility(Settings.TC3_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, object>();
            testData.Add("TS3_9_1", xmlUtility.GetSingleNode(string.Format(Settings.TC3_XPATH_PREFIX, testCaseName, "TS3_9_1")));
            testData.Add("TS3_10_1", xmlUtility.GetSingleNode(string.Format(Settings.TC3_XPATH_PREFIX, testCaseName, "TS3_10_1")));
            testData.Add("TS3_11_1", xmlUtility.GetSingleNode(string.Format(Settings.TC3_XPATH_PREFIX, testCaseName, "TS3_11_1")));
            testData.Add("TS3_12_1", xmlUtility.GetSingleNode(string.Format(Settings.TC3_XPATH_PREFIX, testCaseName, "TS3_12_1")));
            testData.Add("TS3_13_1", xmlUtility.GetSingleNode(string.Format(Settings.TC3_XPATH_PREFIX, testCaseName, "TS3_13_1")));
            testData.Add("TS3_14_1", xmlUtility.GetSingleNode(string.Format(Settings.TC3_XPATH_PREFIX, testCaseName, "TS3_14_1")));
            testData.Add("TS3_14_2", xmlUtility.GetSingleNode(string.Format(Settings.TC3_XPATH_PREFIX, testCaseName, "TS3_14_2")));
            testData.Add("TS3_14_3", xmlUtility.GetSingleNode(string.Format(Settings.TC3_XPATH_PREFIX, testCaseName, "TS3_14_3")));
            testData.Add("TS3_14_4", xmlUtility.GetSingleNode(string.Format(Settings.TC3_XPATH_PREFIX, testCaseName, "TS3_14_4")));
            testData.Add("TS3_14_5", xmlUtility.GetSingleNode(string.Format(Settings.TC3_XPATH_PREFIX, testCaseName, "TS3_14_5")));
            testData.Add("TS3_15_1", xmlUtility.GetSingleNode(string.Format(Settings.TC3_XPATH_PREFIX, testCaseName, "TS3_15_1")));
            testData.Add("TS3_16_1", xmlUtility.GetSingleNode(string.Format(Settings.TC3_XPATH_PREFIX, testCaseName, "TS3_16_1")));
            testData.Add("TS3_17_1", xmlUtility.GetSingleNode(string.Format(Settings.TC3_XPATH_PREFIX, testCaseName, "TS3_17_1")));
            testData.Add("TS3_17_2", xmlUtility.GetSingleNode(string.Format(Settings.TC3_XPATH_PREFIX, testCaseName, "TS3_17_2")));
            testData.Add("TS3_18_1", xmlUtility.GetSingleNode(string.Format(Settings.TC3_XPATH_PREFIX, testCaseName, "TS3_18_1")));
            testData.Add("TS3_19_1", xmlUtility.GetSingleNode(string.Format(Settings.TC3_XPATH_PREFIX, testCaseName, "TS3_19_1")));
            testData.Add("TS3_20_1", xmlUtility.GetSingleNode(string.Format(Settings.TC3_XPATH_PREFIX, testCaseName, "TS3_20_1")));
            testData.Add("TS3_20_2", xmlUtility.GetSingleNode(string.Format(Settings.TC3_XPATH_PREFIX, testCaseName, "TS3_20_2")));
            testData.Add("TS3_21_1", xmlUtility.GetSingleNode(string.Format(Settings.TC3_XPATH_PREFIX, testCaseName, "TS3_21_1")));
            testData.Add("TS3_22_1", xmlUtility.GetSingleNode(string.Format(Settings.TC3_XPATH_PREFIX, testCaseName, "TS3_22_1")));

            testData.Add("TS3_25_1", xmlUtility.GetSingleNode(string.Format(Settings.TC3_XPATH_PREFIX, testCaseName, "TS3_25_1")));
            testData.Add("TS3_26_1", xmlUtility.GetSingleNode(string.Format(Settings.TC3_XPATH_PREFIX, testCaseName, "TS3_26_1")));
            testData.Add("TS3_27_1", xmlUtility.GetSingleNode(string.Format(Settings.TC3_XPATH_PREFIX, testCaseName, "TS3_27_1")));
            testData.Add("TS3_28_1", xmlUtility.GetSingleNode(string.Format(Settings.TC3_XPATH_PREFIX, testCaseName, "TS3_28_1")));
            testData.Add("TS3_29_1", xmlUtility.GetSingleNode(string.Format(Settings.TC3_XPATH_PREFIX, testCaseName, "TS3_29_1")));
            testData.Add("TS3_30_1", xmlUtility.GetSingleNode(string.Format(Settings.TC3_XPATH_PREFIX, testCaseName, "TS3_30_1")));
            testData.Add("TS3_31_1", xmlUtility.GetSingleNode(string.Format(Settings.TC3_XPATH_PREFIX, testCaseName, "TS3_31_1")));
            testData.Add("TS3_32_1", xmlUtility.GetSingleNode(string.Format(Settings.TC3_XPATH_PREFIX, testCaseName, "TS3_32_1")));
            testData.Add("TS3_33_1", xmlUtility.GetSingleNode(string.Format(Settings.TC3_XPATH_PREFIX, testCaseName, "TS3_33_1")));

            testData.Add("TS3_23_1", xmlUtility.GetSingleNode(string.Format(Settings.TC3_XPATH_PREFIX, testCaseName, "TS3_23_1")));
            testData.Add("TS3_24_1", xmlUtility.GetSingleNode(string.Format(Settings.TC3_XPATH_PREFIX, testCaseName, "TS3_24_1")));

            testData.Add("TS3_34_1", xmlUtility.GetSingleNode(string.Format(Settings.TC3_XPATH_PREFIX, testCaseName, "TS3_34_1")));

            testData.Add("LocationChange", xmlUtility.GetSingleNode(string.Format(Settings.TC3_XPATH_PREFIX, testCaseName, "LocationChange")));

            testData.Add("GDVRunOnce", xmlUtility.GetSingleNode(string.Format(Settings.TC3_XPATH_PREFIX, testCaseName, "GDVRunOnce")));

            testData.Add("Mail", xmlUtility.GetSingleNode(string.Format(Settings.TC3_XPATH_PREFIX, testCaseName, "Mail")));
            testData.Add("Ftp", xmlUtility.GetSingleNode(string.Format(Settings.TC3_XPATH_PREFIX, testCaseName, "Ftp")));

            var realtimeGeozone = Settings.CommonTestData[0];
            var streetlights = realtimeGeozone.Devices.Where(p => p.Type == DeviceType.Streetlight && p.Status == DeviceStatus.Working).ToList();
            var controller = realtimeGeozone.Devices.FirstOrDefault(p => p.Type == DeviceType.Controller && p.Status == DeviceStatus.Working);
            testData.Add("Geozone", realtimeGeozone.Path);
            testData.Add("Streetlights", streetlights);
            testData.Add("Controller", controller);

            return testData;
        }

        /// <summary>
        /// Read test data for test case TS 3.1.1
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, object> GetTestDataOfTestTS3_01_01()
        {
            var testCaseName = "TS3_1_1";
            var xmlUtility = new XmlUtility(Settings.TC3_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, object>();
            var controllerInfo = xmlUtility.GetSingleNode(string.Format(Settings.TC3_XPATH_PREFIX, testCaseName, "Controller"));
            testData.Add("ControllerId", controllerInfo.GetAttrVal("id"));
            testData.Add("ControllerName", controllerInfo.GetAttrVal("name"));          
            testData.Add("SearchAttributeValue", xmlUtility.GetSingleNodeText(string.Format(Settings.TC3_XPATH_PREFIX, testCaseName, "SearchAttributeValue")));
            testData.Add("InexistingUniqueAddress", xmlUtility.GetSingleNodeText(string.Format(Settings.TC3_XPATH_PREFIX, testCaseName, "InexistingUniqueAddress")));
            testData.Add("ExistingUniqueAddress", xmlUtility.GetSingleNodeText(string.Format(Settings.TC3_XPATH_PREFIX, testCaseName, "ExistingUniqueAddress")));
            testData.Add("ClickDeviceRequest", xmlUtility.GetSingleNodeText(string.Format(Settings.TC3_XPATH_PREFIX, testCaseName, "ClickDeviceRequest")));
            testData.Add("ExportFilePattern", xmlUtility.GetSingleNodeText(string.Format(Settings.TC3_XPATH_PREFIX, testCaseName, "ExportFilePattern")));
            var attributesNodes = xmlUtility.GetChildNodes(string.Format(Settings.TC3_XPATH_PREFIX, testCaseName, "Attributes"));
            var dicAttributes = new Dictionary<string, string>();
            foreach (var node in attributesNodes)
            {
                dicAttributes.Add(node.GetAttrVal("name"), node.GetAttrVal("id"));
            }
            testData.Add("Attributes", dicAttributes);

            return testData;
        }

        /// <summary>
        ///  Read test data for test case TS 3.7.1
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, object> GetTestDataOfTestTS3_07_01()
        {
            var testCaseName = "TS3_7_1";
            var xmlUtility = new XmlUtility(Settings.TC3_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, object>();

            testData.Add("RootGeoZone", xmlUtility.GetSingleNodeText(string.Format(Settings.TC3_XPATH_PREFIX, testCaseName, "RootGeoZone")));
            testData.Add("ChildGeoZone", xmlUtility.GetSingleNodeText(string.Format(Settings.TC3_XPATH_PREFIX, testCaseName, "ChildGeoZone")));
            testData.Add("Devices", xmlUtility.GetChildNodesText(string.Format(Settings.TC3_XPATH_PREFIX, testCaseName, "Devices")));
            testData.Add("ExpectedPopupTitle", xmlUtility.GetSingleNodeText(string.Format(Settings.TC3_XPATH_PREFIX, testCaseName, "ExpectedPopupTitle")));
            testData.Add("ReportPrefix", xmlUtility.GetSingleNodeText(string.Format(Settings.TC3_XPATH_PREFIX, testCaseName, "ReportPrefix")));

            return testData;
        }

        /// <summary>
        ///  Read test data for test case TS 3.8.1
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, object> GetTestDataOfTestTS3_08_01()
        {
            var testCaseName = "TS3_8_1";
            var xmlUtility = new XmlUtility(Settings.TC3_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, object>();

            testData.Add("RootGeoZone", xmlUtility.GetSingleNodeText(string.Format(Settings.TC3_XPATH_PREFIX, testCaseName, "RootGeoZone")));
            testData.Add("ChildGeoZone", xmlUtility.GetSingleNodeText(string.Format(Settings.TC3_XPATH_PREFIX, testCaseName, "ChildGeoZone")));
            testData.Add("Devices", xmlUtility.GetChildNodesText(string.Format(Settings.TC3_XPATH_PREFIX, testCaseName, "Devices")));
            testData.Add("ReportPrefix", xmlUtility.GetSingleNodeText(string.Format(Settings.TC3_XPATH_PREFIX, testCaseName, "ReportPrefix")));

            return testData;
        }        

        #endregion //XML Input data

        #region Verify methods

        private static void Verify2GraphsAsBytes(byte[] graph1ImageAsBytes, byte[] graph2ImageAsBytes, string message)
        {
            var graph1Image = new MagickImage(graph1ImageAsBytes);
            var graph2Image = new MagickImage(graph2ImageAsBytes);
            var result = graph1Image.Compare(graph2Image, ErrorMetric.MeanAbsolute);
            VerifyTrue(string.Format("Verify data in charts change accordingly {0}", message), result > 0, true, result > 0);
        }

        private void VerifyNewReportInGrid(ReportManagerPage page, string reportName)
        {
            var reportsList = page.GridPanel.GetListOfColumnData("Name");
            var newReport = reportsList.FirstOrDefault(item => string.Equals(item.SplitAndGetAt(0), reportName));
            VerifyEqual(string.Format("Verify new report {0} is present in grid", reportName), true, newReport != null);
        }

        #endregion //Verify methods

        #region Common methods

        private dynamic CreateNormalReportHasPropertiesTab(ReportManagerPage page, XmlNode mail, XmlNode data)
        {
            var geozone = data.GetAttrVal("Geozone");
            var reportType = data.GetAttrVal("ReportType");
            var reportDetails = data.GetAttrVal("ReportDetails");
            var filteringMode = data.GetAttrVal("FilteringMode");
            var criticalLifetime = data.GetAttrVal("CriticalLifetime");
            var numberOfDays = data.GetAttrVal("NumberOfDays");
            var periodicity = data.GetAttrVal("Periodicity");
            var htmlFormat = bool.Parse(data.GetAttrVal("HtmlFormat"));
            var reportName = SLVHelper.GenerateUniqueName(string.Format("[{0}][{1}][{2}] {3}", Browser.Name, Settings.Users["DefaultTest"].Username, data.Name, reportType));
            var sufix = filteringMode != null ? string.Format("{0}-Html_{1}", filteringMode, htmlFormat) : string.Format("Html_{0}", htmlFormat);
            var mailSubject = string.Format("[{0}][{1}][{2}] {3}-{4}", Browser.Name, Settings.Users["DefaultTest"].Username, data.Name, reportType, sufix);
            var reportRunDate = Settings.GetServerTime().AddMinutes(REPORT_WAIT_MINUTES);
            var mailHour = reportRunDate.Hour;
            var mailMinute = reportRunDate.Minute;

            EmailUtility.CleanInbox(mailSubject);

            page.GeozoneTreeMainPanel.SelectNode(geozone);
            page.GridPanel.ClickAddReportToolbarButton();
            page.WaitForPreviousActionComplete();
            page.ReportEditorPanel.WaitForPanelLoaded();
            page.ReportEditorPanel.EnterNameInput(reportName);
            page.ReportEditorPanel.SelectTypeDropDown(reportType);
            page.WaitForPreviousActionComplete();

            // Properties tab
            page.ReportEditorPanel.EnterDescriptionInput(string.Format("Automated {0} Description", reportType));
            if (!string.IsNullOrEmpty(reportDetails)) page.ReportEditorPanel.SelectReportDetailsDropDown(reportDetails);
            if (!string.IsNullOrEmpty(filteringMode)) page.ReportEditorPanel.SelectFilteringModeDropDown(filteringMode);
            if (!string.IsNullOrEmpty(criticalLifetime)) page.ReportEditorPanel.EnterCriticalLifetimeNumericInput(criticalLifetime);
            if (!string.IsNullOrEmpty(numberOfDays)) page.ReportEditorPanel.EnterNumberOfDaysInput(numberOfDays);

            // Scheduler tab
            page.ReportEditorPanel.SelectTab("Scheduler");
            page.ReportEditorPanel.SelectPeriodicityDropDown(periodicity);
            page.ReportEditorPanel.SelectHourDropDown(string.Format("{0:D2}", mailHour));
            page.ReportEditorPanel.SelectMinuteDropDown(string.Format("{0:D2}", mailMinute));
            page.ReportEditorPanel.SelectTimezoneDropDown(Settings.DEFAULT_TIMEZONE);

            // Export tab
            page.ReportEditorPanel.SelectTab("Export");
            page.ReportEditorPanel.EnterSubjectInput(mailSubject);
            page.ReportEditorPanel.EnterFromInput(mail.GetAttrVal("From"));
            page.ReportEditorPanel.SelectContactsListDropDown(mail.GetAttrVal("Contacts"));
            page.ReportEditorPanel.TickHtmlFormatCheckbox(htmlFormat);

            page.ReportEditorPanel.ClickSaveButton();
            page.WaitForPreviousActionComplete();
            page.WaitForReportDetailsDisappeared();

            dynamic report = new ExpandoObject();
            report.Name = reportName;
            report.MailSubject = mailSubject;
            report.MailSender = mail.GetAttrVal("From");
            report.IsHtml = htmlFormat;
            report.ReportCreatedTime = Settings.GetServerTime().ToString("G");
            report.EmailReceivedTime = reportRunDate.ToString("G");

            return report;
        }

        private dynamic CreateNormalReportHasReportTab(ReportManagerPage page, XmlNode mail, XmlNode data)
        {
            var geozone = data.GetAttrVal("Geozone");
            var reportType = data.GetAttrVal("ReportType");
            var periodicity = data.GetAttrVal("Periodicity");
            var mailFormat = data.GetAttrVal("MailFormat");
            var deviceCategories = data.GetAttrVal("DeviceCategories");
            var reportName = SLVHelper.GenerateUniqueName(string.Format("[{0}][{1}][{2}] {3}", Browser.Name, Settings.Users["DefaultTest"].Username, data.Name, reportType));
            var mailSubject = string.Format("[{0}][{1}][{2}] {3}-{4}-Categories_{5}", Browser.Name, Settings.Users["DefaultTest"].Username, data.Name, reportType, mailFormat, deviceCategories);
            var reportRunDate = Settings.GetServerTime().AddMinutes(REPORT_WAIT_MINUTES);
            var mailHour = reportRunDate.Hour;
            var mailMinute = reportRunDate.Minute;

            EmailUtility.CleanInbox(mailSubject);

            page.GeozoneTreeMainPanel.SelectNode(geozone);
            page.GridPanel.ClickAddReportToolbarButton();
            page.WaitForPreviousActionComplete();
            page.ReportEditorPanel.WaitForPanelLoaded();
            page.ReportEditorPanel.EnterNameInput(reportName);
            page.ReportEditorPanel.SelectTypeDropDown(reportType);
            page.WaitForPreviousActionComplete();

            // Scheduler tab
            page.ReportEditorPanel.SelectHourDropDown(string.Format("{0:D2}", mailHour));
            page.ReportEditorPanel.SelectMinuteDropDown(string.Format("{0:D2}", mailMinute));
            page.ReportEditorPanel.SelectPeriodicityDropDown(periodicity);
            page.ReportEditorPanel.SelectTimezoneDropDown(Settings.DEFAULT_TIMEZONE);

            // Export tab
            page.ReportEditorPanel.SelectTab("Export");
            page.ReportEditorPanel.TickMailEnabledCheckbox(bool.Parse(data.GetAttrVal("MailEnabled")));
            page.ReportEditorPanel.SelectMailFormatDropDown(mailFormat);
            page.ReportEditorPanel.TickReportMailBodyCheckbox(bool.Parse(data.GetAttrVal("ReportMailBody")));
            page.ReportEditorPanel.EnterSubjectInput(mailSubject);
            page.ReportEditorPanel.EnterFromInput(mail.GetAttrVal("From"));
            page.ReportEditorPanel.SelectContactsListDropDown(mail.GetAttrVal("Contacts"));

            // Report tab
            page.ReportEditorPanel.SelectTab("Report");
            page.ReportEditorPanel.SelectDeviceCategoriesListDropDown(data.GetAttrVal("DeviceCategories"));
            page.ReportEditorPanel.TickRecurseCheckbox(bool.Parse(data.GetAttrVal("Recurse")));

            page.ReportEditorPanel.ClickSaveButton();
            page.WaitForPreviousActionComplete();
            page.WaitForReportDetailsDisappeared();

            dynamic report = new ExpandoObject();
            report.Name = reportName;
            report.MailSubject = mailSubject;
            report.MailSender = mail.GetAttrVal("From");
            report.IsHtml = mailFormat == "Html";
            report.ReportCreatedTime = Settings.GetServerTime().ToString("G");
            report.EmailReceivedTime = reportRunDate.ToString("G");

            return report;
        }

        private dynamic CreateLatencyReport(ReportManagerPage page, XmlNode mail, XmlNode data)
        {
            var geozone = data.GetAttrVal("Geozone");
            var reportType = data.GetAttrVal("ReportType");
            var periodicity = data.GetAttrVal("Periodicity");
            var mailFormat = data.GetAttrVal("MailFormat");
            var deviceCategories = data.GetAttrVal("DeviceCategories");
            var commandValue = data.GetAttrVal("CommandValue");

            var reportName = SLVHelper.GenerateUniqueName(string.Format("[{0}][{1}][{2}] {3}", Browser.Name, Settings.Users["DefaultTest"].Username, data.Name, reportType));
            var mailSubject = string.Format("[{0}][{1}][{2}] {3}-{4}-Categories_{5}-Command_{6}", Browser.Name, Settings.Users["DefaultTest"].Username, data.Name, reportType, mailFormat, deviceCategories, commandValue);
            var reportRunDate = Settings.GetServerTime().AddMinutes(REPORT_WAIT_MINUTES);
            var mailHour = reportRunDate.Hour;
            var mailMinute = reportRunDate.Minute;

            EmailUtility.CleanInbox(mailSubject);

            page.GeozoneTreeMainPanel.SelectNode(geozone);
            page.GridPanel.ClickAddReportToolbarButton();
            page.WaitForPreviousActionComplete();
            page.ReportEditorPanel.WaitForPanelLoaded();
            page.ReportEditorPanel.EnterNameInput(reportName);
            page.ReportEditorPanel.SelectTypeDropDown(reportType);
            page.WaitForPreviousActionComplete();

            // Export tab
            page.ReportEditorPanel.SelectTab("Export");
            page.ReportEditorPanel.TickMailEnabledCheckbox(bool.Parse(data.GetAttrVal("MailEnabled")));
            page.ReportEditorPanel.SelectMailFormatDropDown(mailFormat);
            page.ReportEditorPanel.TickReportMailBodyCheckbox(bool.Parse(data.GetAttrVal("ReportMailBody")));
            page.ReportEditorPanel.EnterSubjectInput(mailSubject);
            page.ReportEditorPanel.EnterFromInput(mail.GetAttrVal("From"));
            page.ReportEditorPanel.SelectContactsListDropDown(mail.GetAttrVal("Contacts"));

            // Scheduler tab
            page.ReportEditorPanel.SelectTab("Scheduler");
            page.ReportEditorPanel.SelectHourDropDown(string.Format("{0:D2}", mailHour));
            page.ReportEditorPanel.SelectMinuteDropDown(string.Format("{0:D2}", mailMinute));
            page.ReportEditorPanel.SelectPeriodicityDropDown(periodicity);
            page.ReportEditorPanel.SelectTimezoneDropDown(Settings.DEFAULT_TIMEZONE);

            // Report tab
            page.ReportEditorPanel.SelectTab("Report");
            page.ReportEditorPanel.SelectDeviceCategoriesListDropDown(data.GetAttrVal("DeviceCategories"));
            page.ReportEditorPanel.TickRecurseCheckbox(bool.Parse(data.GetAttrVal("Recurse")));
            page.ReportEditorPanel.SelectCommandDropDown(data.GetAttrVal("Command"));
            page.ReportEditorPanel.SelectFeedbackDropDown(data.GetAttrVal("Feedback"));
            page.ReportEditorPanel.SelectModeMeaningDropDown(data.GetAttrVal("ModeMeaningStrId"));
            page.ReportEditorPanel.SelectFromToModeDropDown(data.GetAttrVal("FromToMode"));
            page.ReportEditorPanel.SelectFromLastHoursDropDown(data.GetAttrVal("FromLastHours"));
            page.ReportEditorPanel.EnterCommandValueInput(commandValue);

            page.ReportEditorPanel.ClickSaveButton();
            page.WaitForPreviousActionComplete();
            page.WaitForReportDetailsDisappeared();

            dynamic report = new ExpandoObject();
            report.Name = reportName;
            report.CommandValue = commandValue;
            report.MailSubject = mailSubject;
            report.MailSender = mail.GetAttrVal("From");
            report.IsHtml = mailFormat == "Html";
            report.ReportCreatedTime = Settings.GetServerTime().ToString("G");
            report.EmailReceivedTime = reportRunDate.ToString("G");

            return report;
        }

        private dynamic CreateUMSUGReport(ReportManagerPage page, XmlNode mail, XmlNode ftp, XmlNode data)
        {
            var geozone = data.GetAttrVal("Geozone");
            var reportType = data.GetAttrVal("ReportType");
            var downFailures = data.GetAttrVal("DownFailures");
            var periodicity = data.GetAttrVal("Periodicity");
            var reportName = SLVHelper.GenerateUniqueName(string.Format("[{0}][{1}][{2}] {3}", Browser.Name, Settings.Users["DefaultTest"].Username, data.Name, reportType));
            var mailSubject = string.Format("[{0}][{1}][{2}] {3}-DownFailures_{4}", Browser.Name, Settings.Users["DefaultTest"].Username, data.Name, reportType, downFailures);
            var reportRunDate = Settings.GetServerTime().AddMinutes(REPORT_WAIT_MINUTES);
            var mailHour = reportRunDate.Hour;
            var mailMinute = reportRunDate.Minute;

            EmailUtility.CleanInbox(mailSubject);

            page.GeozoneTreeMainPanel.SelectNode(geozone);
            page.GridPanel.ClickAddReportToolbarButton();
            page.WaitForPreviousActionComplete();
            page.ReportEditorPanel.WaitForPanelLoaded();
            page.ReportEditorPanel.EnterNameInput(reportName);
            page.ReportEditorPanel.SelectTypeDropDown(reportType);
            page.WaitForPreviousActionComplete();                        

            // Scheduler tab
            page.ReportEditorPanel.SelectTab("Scheduler");
            page.ReportEditorPanel.SelectPeriodicityDropDown(periodicity);
            page.ReportEditorPanel.SelectHourDropDown(string.Format("{0:D2}", mailHour));
            page.ReportEditorPanel.SelectMinuteDropDown(string.Format("{0:D2}", mailMinute));

            // Properties tab
            page.ReportEditorPanel.SelectTab("Properties");
            page.ReportEditorPanel.EnterDescriptionInput(string.Format("Automated {0} Description", reportType));
            var downFailureItems = page.ReportEditorPanel.GetListOfDownMeaningsItems();
            if (!downFailureItems.Contains("Lamp Failure")) Warning("#1429517 - Lamp failure is not available in the list of failures in the UMSUG report");
            page.ReportEditorPanel.SelectDownFailuresListDropDown(downFailures);

            // Export tab
            page.ReportEditorPanel.SelectTab("Export");
            page.ReportEditorPanel.EnterFtpHostInput(ftp.GetAttrVal("Host"));
            page.ReportEditorPanel.EnterFtpUserInput(ftp.GetAttrVal("User"));
            page.ReportEditorPanel.EnterFtpPasswordInput(ftp.GetAttrVal("Password"));
            page.ReportEditorPanel.EnterFtpDirectoryInput(ftp.GetAttrVal("Directory"));
            page.ReportEditorPanel.TickFtpPassiveModeCheckbox(bool.Parse(ftp.GetAttrVal("PassiveMode")));

            page.ReportEditorPanel.ClickSaveButton();
            page.WaitForPreviousActionComplete();
            page.WaitForReportDetailsDisappeared();

            dynamic report = new ExpandoObject();
            report.Name = reportName;
            report.ReportCreatedTime = Settings.GetServerTime().ToString("G");

            return report;
        }

        private dynamic CreateLocationChangeReport(ReportManagerPage page, XmlNode mail, XmlNode ftp, XmlNode data)
        {
            var reportType = data.GetAttrVal("ReportType");
            var mailFormat = data.GetAttrVal("MailFormat");
            var periodicity = data.GetAttrVal("Periodicity");
            var reportName = SLVHelper.GenerateUniqueName(string.Format("[{0}][{1}][{2}] {3}", Browser.Name, Settings.Users["DefaultTest"].Username, data.Name, reportType));
            var mailSubject = string.Format("[{0}][{1}][{2}] {3}", Browser.Name, Settings.Users["DefaultTest"].Username, data.Name, reportType);
            var ftpFilePattern = SLVHelper.GenerateUniqueName("LCR-" + Settings.Users["DefaultTest"].Username);
            var reportRunDate = Settings.GetServerTime().AddMinutes(REPORT_WAIT_MINUTES);
            var mailHour = reportRunDate.Hour;
            var mailMinute = reportRunDate.Minute;

            EmailUtility.CleanInbox(mailSubject);
            
            page.GridPanel.ClickAddReportToolbarButton();
            page.WaitForPreviousActionComplete();
            page.ReportEditorPanel.WaitForPanelLoaded();
            page.ReportEditorPanel.EnterNameInput(reportName);
            page.ReportEditorPanel.SelectTypeDropDown(reportType);
            page.WaitForPreviousActionComplete();            

            // Export tab
            //FTP
            page.ReportEditorPanel.SelectTab("Export");
            page.ReportEditorPanel.EnterFtpHostInput(ftp.GetAttrVal("Host"));
            page.ReportEditorPanel.EnterFtpUserInput(ftp.GetAttrVal("User"));
            page.ReportEditorPanel.EnterFtpPasswordInput(ftp.GetAttrVal("Password"));
            page.ReportEditorPanel.EnterFtpFilenameInput(ftp.GetAttrVal("Directory") + ftpFilePattern);
            page.ReportEditorPanel.TickFtpEnabledCheckbox(bool.Parse(ftp.GetAttrVal("Enabled")));
            page.ReportEditorPanel.SelectFtpFormatDropDown("CSV");

            //Mail
            page.ReportEditorPanel.TickMailEnabledCheckbox(bool.Parse(data.GetAttrVal("MailEnabled")));
            page.ReportEditorPanel.SelectMailFormatDropDown(mailFormat);
            page.ReportEditorPanel.TickReportMailBodyCheckbox(bool.Parse(data.GetAttrVal("ReportMailBody")));
            page.ReportEditorPanel.EnterSubjectInput(mailSubject);
            page.ReportEditorPanel.EnterFromInput(mail.GetAttrVal("From"));
            page.ReportEditorPanel.SelectContactsListDropDown(mail.GetAttrVal("Contacts"));

            // Scheduler tab
            page.ReportEditorPanel.SelectTab("Scheduler");
            page.ReportEditorPanel.SelectHourDropDown(string.Format("{0:D2}", mailHour));
            page.ReportEditorPanel.SelectMinuteDropDown(string.Format("{0:D2}", mailMinute));
            page.ReportEditorPanel.SelectPeriodicityDropDown(periodicity);
            page.ReportEditorPanel.SelectTimezoneDropDown(Settings.DEFAULT_TIMEZONE);

            // Report tab
            page.ReportEditorPanel.SelectTab("Report");
            page.ReportEditorPanel.SelectDeviceCategoriesListDropDown(data.GetAttrVal("DeviceCategories"));
            page.ReportEditorPanel.SelectPeriodDropDown(data.GetAttrVal("Period"));
            page.ReportEditorPanel.TickRecurseCheckbox(bool.Parse(data.GetAttrVal("Recurse")));

            page.ReportEditorPanel.ClickSaveButton();
            page.WaitForPreviousActionComplete();
            page.WaitForReportDetailsDisappeared();
            
            dynamic report = new ExpandoObject();
            report.Name = reportName;
            report.MailSubject = mailSubject;
            report.MailSender = mail.GetAttrVal("From");
            report.IsHtml = mailFormat == "Html";
            report.ReportCreatedTime = Settings.GetServerTime().ToString("G");
            report.EmailReceivedTime = reportRunDate.ToString("G");
            report.FtpFilePattern = ftpFilePattern;

            return report;
        }

        private dynamic CreateGenericDeviceValuesRunOnceReport(ReportManagerPage page, XmlNode mail, XmlNode ftp, XmlNode data)
        {
            var geozone = data.GetAttrVal("Geozone");
            var reportType = data.GetAttrVal("ReportType");
            var mailFormat = data.GetAttrVal("MailFormat");
            var reportName = SLVHelper.GenerateUniqueName(string.Format("[{0}][{1}][{2}] {3}", Browser.Name, Settings.Users["DefaultTest"].Username, data.Name, reportType));
            var mailSubject = string.Format("[{0}][{1}][{2}] {3}", Browser.Name, Settings.Users["DefaultTest"].Username, data.Name, reportType);
            var ftpFilePattern = SLVHelper.GenerateUniqueName("GDVRunOnce-" + Settings.Users["DefaultTest"].Username);
            var reportRunDate = Settings.GetServerTime().AddMinutes(REPORT_WAIT_MINUTES);
            var mailHour = reportRunDate.Hour;
            var mailMinute = reportRunDate.Minute;

            EmailUtility.CleanInbox(mailSubject);

            page.GeozoneTreeMainPanel.SelectNode(geozone);
            page.GridPanel.ClickAddReportToolbarButton();
            page.WaitForPreviousActionComplete();
            page.ReportEditorPanel.WaitForPanelLoaded();
            page.ReportEditorPanel.EnterNameInput(reportName);
            page.ReportEditorPanel.SelectTypeDropDown(reportType);
            page.WaitForPreviousActionComplete();

            // Export tab
            //FTP
            page.ReportEditorPanel.SelectTab("Export");
            page.ReportEditorPanel.EnterFtpHostInput(ftp.GetAttrVal("Host"));
            page.ReportEditorPanel.EnterFtpUserInput(ftp.GetAttrVal("User"));
            page.ReportEditorPanel.EnterFtpPasswordInput(ftp.GetAttrVal("Password"));
            page.ReportEditorPanel.EnterFtpFilenameInput(ftp.GetAttrVal("Directory") + ftpFilePattern);
            page.ReportEditorPanel.TickFtpEnabledCheckbox(bool.Parse(ftp.GetAttrVal("Enabled")));
            page.ReportEditorPanel.SelectFtpFormatDropDown(data.GetAttrVal("FtpFormat"));

            //Mail
            page.ReportEditorPanel.TickMailEnabledCheckbox(bool.Parse(data.GetAttrVal("MailEnabled")));
            page.ReportEditorPanel.SelectMailFormatDropDown(mailFormat);
            page.ReportEditorPanel.TickReportMailBodyCheckbox(bool.Parse(data.GetAttrVal("ReportMailBody")));
            page.ReportEditorPanel.EnterSubjectInput(mailSubject);
            page.ReportEditorPanel.EnterFromInput(mail.GetAttrVal("From"));
            page.ReportEditorPanel.SelectContactsListDropDown(mail.GetAttrVal("Contacts"));
            
            // Report tab
            page.ReportEditorPanel.SelectTab("Report");
            page.ReportEditorPanel.SelectAllDeviceCategoriesDropDownList();
            page.ReportEditorPanel.SelectAllMeaningsDropDownList();
            page.ReportEditorPanel.EnterStartDayDateInput(reportRunDate);
            page.ReportEditorPanel.EnterStartTimeInput(string.Format("{0}:{1}", reportRunDate.Hour.ToString("D2"), reportRunDate.Minute.ToString("D2")));
            page.ReportEditorPanel.EnterEndDayDateInput(reportRunDate);
            page.ReportEditorPanel.EnterEndTimeInput(string.Format("{0}:{1}", mailHour.ToString("D2"), mailMinute.ToString("D2")));

            page.ReportEditorPanel.ClickSaveButton();
            page.WaitForPreviousActionComplete();
            page.WaitForReportDetailsDisappeared();

            dynamic report = new ExpandoObject();
            report.Name = reportName;
            report.MailSubject = mailSubject;
            report.MailSender = mail.GetAttrVal("From");
            report.IsHtml = mailFormat == "Html";
            report.ReportCreatedTime = Settings.GetServerTime().ToString("G");
            report.EmailReceivedTime = reportRunDate.ToString("G");
            report.FtpFilePattern = ftpFilePattern;

            return report;
        }

        /// <summary>
        /// Get the attributes mapper dictionary
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private IDictionary<string, string> GetAttributesMapperDictFromJson(JToken token)
        {
            var dict = new Dictionary<string, string>();
            if (token.Type == JTokenType.Array)
            {
                //"label" : "Lamp level command",
                //"name" : "LampCommandLevel",
                foreach (var item in token.Children<JObject>())
                {
                    var key = item["label"];
                    var value = item["name"];
                    if (key != null && value != null)
                        dict.Add(key.ToString(), value.ToString());
                }
            }
            else if (token.Type == JTokenType.Object)
            {
                var key = token["label"];
                var value = token["name"];
                if (key != null && value != null)
                    dict.Add(key.ToString(), value.ToString());
            }

            return dict;
        }

        /// <summary>
        /// Get DateTime String - yyyyMMdd HH:mm:ss.FF
        /// </summary>
        /// <returns></returns>
        private string GetDateTimeString(DateTime dt)
        {
            return dt.ToString("yyyyMMdd HH:mm:ss.FF");
        }

        /// <summary>
        /// Simulate latency requests to device with specific 'commandValue'
        /// </summary>
        /// <param name="commandValue"></param>
        /// <param name="latencyValue"></param>
        private void SimulateLatencyRequestsToDevice(string deviceId, string controllerId, string commandValue, double latencyValue)
        {
            var now = Settings.GetServerTime();
            var later = now.AddMilliseconds(latencyValue);

            // Send another value command before sending testing one to make sure we will receive the latest changes in email
            var anotherValue = "99";
            var commandName = "LampCommandLevel";
            var requestSuccess = SetValueToDevice(controllerId, deviceId, commandName, anotherValue, now);
            if (!requestSuccess) Warning(string.Format("Cannot send request to {0}-{1}-{2}-{3}", controllerId, deviceId, commandName, anotherValue));

            commandName = "LampCommandMode";
            requestSuccess = SetValueToDevice(controllerId, deviceId, commandName, anotherValue, now);
            if (!requestSuccess) Warning(string.Format("Cannot send request to {0}-{1}-{2}-{3}", controllerId, deviceId, commandName, anotherValue));

            commandName = "LampLevel";
            requestSuccess = SetValueToDevice(controllerId, deviceId, commandName, anotherValue, later);
            if (!requestSuccess) Warning(string.Format("Cannot send request to {0}-{1}-{2}-{3}", controllerId, deviceId, commandName, anotherValue));

            // Send testing command value
            commandName = "LampCommandLevel";
            requestSuccess = SetValueToDevice(controllerId, deviceId, commandName, commandValue, now);
            if (!requestSuccess) Warning(string.Format("Cannot send request to {0}-{1}-{2}-{3}", controllerId, deviceId, commandName, anotherValue));

            commandName = "LampCommandMode";
            requestSuccess = SetValueToDevice(controllerId, deviceId, commandName, commandValue, now);
            if (!requestSuccess) Warning(string.Format("Cannot send request to {0}-{1}-{2}-{3}", controllerId, deviceId, commandName, anotherValue));

            commandName = "LampLevel";
            requestSuccess = SetValueToDevice(controllerId, deviceId, commandName, commandValue, later);
            if (!requestSuccess) Warning(string.Format("Cannot send request to {0}-{1}-{2}-{3}", controllerId, deviceId, commandName, anotherValue));
        }

        /// <summary>
        /// Simulate UMSUG request to a device
        /// </summary>
        /// <param name="deviceId"></param>
        /// <param name="controllerId"></param>
        /// <param name="commandValue"></param>
        /// <param name="eventTimes"></param>
        private void SimulateUmsugRequestsToDevice(string deviceId, string controllerId, string commandValue, DateTime eventTimes)
        {
            var requestSuccess = SetValueToDevice(controllerId, deviceId, "LampSwitch", "1", eventTimes);
            if (!requestSuccess) Warning(string.Format("Cannot send request to {0}-{1}-LampSwitch:1", controllerId, deviceId));

            requestSuccess = SetValueToDevice(controllerId, deviceId, "LampLevel", "1", eventTimes);
            if (!requestSuccess) Warning(string.Format("Cannot send request to {0}-{1}-LampLevel:1", controllerId, deviceId));
        }

        #endregion //Common methods

        #endregion //Private methods
    }
}
