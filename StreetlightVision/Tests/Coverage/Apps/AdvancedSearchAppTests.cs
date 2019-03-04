using NUnit.Framework;
using StreetlightVision.Extensions;
using StreetlightVision.Models;
using StreetlightVision.Pages;
using StreetlightVision.Utilities;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;

namespace StreetlightVision.Tests.Coverage.Apps
{
    [TestFixture]
    [NonParallelizable]
    public class AdvancedSearchAppTests : TestBase
    {
        #region Variables

        private readonly string _advancedSeacrhExportedFilePattern = "Advanced_Search*.csv";

        #endregion //Variables

        #region Contructors

        #endregion //Contructors       

        #region Test Cases

        [Test, DynamicRetry]
        [Description("AS_01 Create new Advanced Search")]
        public void AS_01()
        {
            var testData = GetTestDataOfAS_01();
            var xmlGeoZone = testData["Geozone"].ToString();
            var xmlAllAttributes = testData["AllAttributes"] as List<string>;
            var xmlCheckedAttributes = testData["CheckedAttributes"] as List<string>;

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - There is no custom report");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.AdvancedSearch);

            Step("1. Go to AdvancedSearch app");
            Step("2. Expected AdvancedSearch page is routed. New Advanced Search dialog appears with title \"My advanced searches\":");
            Step(" - \"Select a saved search\" combobox text place holder \"Select a saved search\"");
            Step(" - \"New advanced search\" button with Plus icon");
            var advancedSearchPage = desktopPage.GoToApp(App.AdvancedSearch) as AdvancedSearchPage;
            advancedSearchPage.WaitForSearchWizardPopupPanelDisplayed();

            Step("*** Remove all current searches in dropdown ***");
            advancedSearchPage.SearchWizardPopupPanel.ClickCloseButton();
            advancedSearchPage.WaitForSearchWizardPopupPanelDisappeared();
            advancedSearchPage.GridPanel.DeleleAllRequests();
            advancedSearchPage.GridPanel.ClickEditButton();
            advancedSearchPage.WaitForSearchWizardPopupPanelDisplayed();
            Step("***********************************************");

            VerifyEqual("2. Verify New Advanced Search dialog appears with title \"My advanced searches\"", "My advanced searches", advancedSearchPage.SearchWizardPopupPanel.GetPanelTitleText());
            VerifyEqual("2. Verify \"Select a saved search\" combobox text place holder", "Select a saved search", advancedSearchPage.SearchWizardPopupPanel.GetSavedSearchPlaceHolderValue());
            VerifyEqual("2. Verify \"New advanced search\" button text", "New advanced search", advancedSearchPage.SearchWizardPopupPanel.GetAddNewSearchButtonText());

            Step("3. Dropdown \"Select a saved search\" combobox");
            Step("4. Expected A menu is dropped down with a search text field and an item with text \"No matches found\"");
            var savedSearchList = advancedSearchPage.SearchWizardPopupPanel.GetSavedSearchDropDownItems();
            VerifyTrue("4. Verify there is an item with text \"No matches found\"", savedSearchList.Count == 1 && savedSearchList.FirstOrDefault().Equals("No matches found"), "No matches found", savedSearchList.FirstOrDefault());

            Step("5. Click \"New advanced search\" button");
            advancedSearchPage.SearchWizardPopupPanel.ClickNewAdvancedSearchButton();
            advancedSearchPage.SearchWizardPopupPanel.WaitForNewSearchNameInputVisible();

            Step("6. Expected Next steps is routed:");
            Step(" - An empty textbox to fill new search's name");
            Step(" - \"Select a saved search\" button with magnifier icon");
            Step(" - Next button is not present");
            VerifyEqual("6. Verify An empty textbox to fill new search's name", string.Empty, advancedSearchPage.SearchWizardPopupPanel.GetSearchNameValue());
            VerifyEqual("6. Verify \"Select Advanced Search\" button text", "Select a saved search", advancedSearchPage.SearchWizardPopupPanel.GetSelectSearchButtonText());
            VerifyEqual("6. Verify Next button is not present", false, advancedSearchPage.SearchWizardPopupPanel.IsNextButtonVisible());

            Step("7. Click \"Select a saved search\"");
            advancedSearchPage.SearchWizardPopupPanel.ClickSelectSavedSearchButton();
            advancedSearchPage.SearchWizardPopupPanel.WaitForSavedSearhNameDropDownVisible();

            Step("8. Expected Previous step (the first step) is returned");
            VerifyEqual("8. Verify \"Select a saved search\" combobox text place holder", "Select a saved search", advancedSearchPage.SearchWizardPopupPanel.GetSavedSearchPlaceHolderValue());
            VerifyEqual("8. Verify \"New advanced search\" button text", "New advanced search", advancedSearchPage.SearchWizardPopupPanel.GetAddNewSearchButtonText());

            Step("9. Click \"New advanced search\" button to return the step");
            advancedSearchPage.SearchWizardPopupPanel.ClickNewAdvancedSearchButton();
            advancedSearchPage.SearchWizardPopupPanel.WaitForNewSearchNameInputVisible();

            Step("10. Enter a search's name");
            var searchName = SLVHelper.GenerateUniqueName(TestContext.CurrentContext.Test.MethodName);
            advancedSearchPage.SearchWizardPopupPanel.EnterNewSearchNameInput(searchName);

            Step("11. Expected Next button is present");
            VerifyEqual("11. Verify Next button is present", true, advancedSearchPage.SearchWizardPopupPanel.IsNextButtonVisible());

            Step("12. Click Next button");
            advancedSearchPage.SearchWizardPopupPanel.ClickNextButton();
            advancedSearchPage.SearchWizardPopupPanel.WaitForGeozoneFormDisplayed();

            Step("13. Expected Next screen is routed:");
            Step(" - Title of the dialog is now search's name plus asterisk (*); Back icon is present in dialog's title as well");
            Step(" - Message \"Select the geozone to search in\" is above; below is geozone tree");
            Step(" - Next button is available");
            VerifyEqual("13. Verify Title of the dialog is now report's name plus asterisk (*)", string.Format("{0}*", searchName), advancedSearchPage.SearchWizardPopupPanel.GetPanelTitleText());
            VerifyEqual("13. Verify Back icon is present in dialog's title as well", true, advancedSearchPage.SearchWizardPopupPanel.IsBackButtonVisible());
            VerifyEqual("13. Verify Message \"Select the geozone to search in\" is above", "Select the geozone to search in", advancedSearchPage.SearchWizardPopupPanel.GetSelectGeozoneCaptionText());
            VerifyEqual("13. Verify below is geozone tree", true, advancedSearchPage.SearchWizardPopupPanel.GeozoneTreePopupPanel.IsPanelVisible());
            VerifyEqual("13. Verify Next button is present", true, advancedSearchPage.SearchWizardPopupPanel.IsNextButtonVisible());

            Step("14. Select a geozone then click Next");
            advancedSearchPage.SearchWizardPopupPanel.GeozoneTreePopupPanel.SelectNode(xmlGeoZone);
            advancedSearchPage.SearchWizardPopupPanel.ClickNextButton();
            advancedSearchPage.SearchWizardPopupPanel.WaitForAttributeFormDisplayed();

            Step("15. Expected Next step is routed:");
            Step(" - Message \"Select the attributes to display in the search results\"");
            Step(" - List of attributes for selecting");
            Step("  + Account number");
            Step("  + Activate");
            Step("  + Address 1");
            Step("  + Address 2");
            Step("  + Ballast brand");
            Step("  + Ballast failure");
            Step("  + Ballast type");
            Step("  + Bracket brand");
            Step("  + Bracket color");
            Step("  + Bracket model");
            Step("  + Bracket type");
            Step("  + Cabinet Controller");
            Step("  + Cabinet wattage");
            Step("  + Calendar changed");
            Step("  + Calendar commission failure");
            Step("  + Category");
            Step("  + City");
            Step("  + Color code");
            Step("  + Comm. media");
            Step("  + Comment");
            Step("  + Communication failure");
            Step("  + Communication status");
            Step("  + Config status msg");
            Step("  + Configuration status");
            Step("  + Controller ID");
            Step("  + Controller install date");
            Step("  + Customer name");
            Step("  + Customer number");
            Step("  + Cycling count");
            Step("  + Cycling lamp");
            Step("  + Day burner");
            Step("  + Device reset count");
            Step("  + Dimming group");
            Step("  + Dimming interface");
            Step("  + Edge");
            Step("  + Edge Mode");
            Step("  + Energy supplier");
            Step("  + External comms failure");
            Step("  + Fixed saved power");
            Step("  + Gateway Host Name");
            Step("  + High current");
            Step("  + High lamp voltage");
            Step("  + High light level");
            Step("  + High OLC temperature");
            Step("  + High power");
            Step("  + High-to-low delay");
            Step("  + Identifier");
            Step("  + Install status");
            Step("  + Invalid calendar");
            Step("  + Invalid program");
            Step("  + Lamp burning hours");
            Step("  + Lamp command mode");
            Step("  + Lamp energy");
            Step("  + Lamp failure");
            Step("  + Lamp install date");
            Step("  + Lamp level command");
            Step("  + Lamp level feedback");
            Step("  + Lamp switch command");
            Step("  + Lamp switch feedback");
            Step("  + Lamp Type");
            Step("  + Lamp wattage (W)");
            Step("  + Last event log sequence");
            Step("  + Last event request time");
            Step("  + Last IMU log sequence");
            Step("  + Last IMU request time");
            Step("  + Last meter log sequence");
            Step("  + Last meter request time");
            Step("  + Latitude");
            Step("  + Light control mode");
            Step("  + Light distribution");
            Step("  + Longitude");
            Step("  + Low current");
            Step("  + Low lamp voltage");
            Step("  + Low power");
            Step("  + Low power factor");
            Step("  + Low-to-high delay");
            Step("  + Luminaire brand");
            Step("  + Luminaire install date");
            Step("  + Luminaire model");
            Step("  + Luminaire type");
            Step("  + Lux level (Lux)");
            Step("  + Mains control mode");
            Step("  + Mains current");
            Step("  + Mains voltage (V)");
            Step("  + Maintain delay");
            Step("  + Map number");
            Step("  + Metered power (W)");
            Step("  + Network type");
            Step("  + Open circuit");
            Step("  + Orientation");
            Step("  + Pole color code");
            Step("  + Pole height");
            Step("  + Pole install date");
            Step("  + Pole material");
            Step("  + Pole or head install");
            Step("  + Pole shape");
            Step("  + Pole status");
            Step("  + Pole type");
            Step("  + Power Supply Failure");
            Step("  + Program changed");
            Step("  + Relay1CommandMode");
            Step("  + Relay1Level");
            Step("  + Relay1State");
            Step("  + Relay failure");
            Step("  + Section");
            Step("  + Segment");
            Step("  + Sensor group");
            Step("  + Software version");
            Step("  + Start delay");
            Step("  + Status");
            Step("  + Sum power factor");
            Step("  + Supply losses");
            Step("  + Switch ON counter");
            Step("  + TalqAddress");
            Step("  + Temperature");
            Step("  + Type of equipment");
            Step("  + Type of ground fixing");
            Step("  + Unique address");
            Step("  + Zip code");
            Step(" - Attributes: \"Address 1\", \"Configuration status\", \"Dimming group\", \"Install status\", \"Unique address\" are selected by default");
            var allAttributeList = advancedSearchPage.SearchWizardPopupPanel.GetAllAttributeList();
            xmlAllAttributes.Sort();
            allAttributeList.Sort();
            var checkedAttributeList = advancedSearchPage.SearchWizardPopupPanel.GetCheckedAttributeList();
            VerifyEqual("15. Verify Message \"Select the attributes to display in the search results\"", "Select the attributes to display in the search results", advancedSearchPage.SearchWizardPopupPanel.GetSelectAttributeCaptionText());
            bool areAllAttributesMatched = xmlAllAttributes.Count == allAttributeList.Count && xmlAllAttributes.CheckIfIncluded(allAttributeList);
            VerifyTrue("15. Verify List of attributes for selecting", areAllAttributesMatched == true, string.Join(", ", xmlAllAttributes), string.Join(", ", allAttributeList));
            bool areCheckedAttributesMatched = xmlCheckedAttributes.Count == checkedAttributeList.Count && xmlCheckedAttributes.CheckIfIncluded(checkedAttributeList);
            VerifyTrue("15. Verify Attributes: \"Address 1\", \"Configuration status\", \"Dimming group\", \"Install status\", \"Unique address\" are selected by default", areCheckedAttributesMatched == true, string.Join(", ", xmlCheckedAttributes), string.Join(", ", checkedAttributeList));

            Step("16. Tick 'Category' attribute");
            advancedSearchPage.SearchWizardPopupPanel.CheckAttributeList("Category");

            Step("17. Click Next");
            advancedSearchPage.SearchWizardPopupPanel.ClickNextButton();
            advancedSearchPage.SearchWizardPopupPanel.WaitForFilterFormDisplayed();

            Step("18. Expected Next step is routed:");
            Step(" - Message \"Filter your results by adding search criteria\"");
            Step(" - Filter condition panel");
            VerifyEqual("18. Verify Message \"Filter your results by adding search criteria\"", "Filter your results by adding search criteria", advancedSearchPage.SearchWizardPopupPanel.GetFilterCaptionText());
            VerifyEqual("18. Verify Filter condition panel", true, advancedSearchPage.SearchWizardPopupPanel.IsFilterMenuVisible());

            Step("19. Select the combobox and choose 'Category'");
            var itemsName = advancedSearchPage.SearchWizardPopupPanel.GetListOfFilterItemsName();

            Step("20. Verify There is an attribute 'Category'");
            VerifyEqual("20. Verify There is an attribute 'Category'", true, itemsName.Exists(p => p.Equals("Category")));

            Step("21. Click Next");
            advancedSearchPage.SearchWizardPopupPanel.ClickNextButton();
            advancedSearchPage.SearchWizardPopupPanel.WaitForFinishFormDisplayed();
            advancedSearchPage.SearchWizardPopupPanel.WaitForDeviceSearchCompleted();

            Step("22. Expected Next step is routed:");
            Step(" - Message {{n}} devices match your search criteria. Click on Finish to see the results");
            Step(" - Next button is not present anymore, Finish button is present");
            var patternMessage = @"(\d*) devices match your search criteria. Click on Finish to see the results";
            var actualMessage = string.Format("{0} {1}", advancedSearchPage.SearchWizardPopupPanel.GetCriteriaMessageText(), advancedSearchPage.SearchWizardPopupPanel.GetFinishMessageText());
            var messageRegex = Regex.Match(actualMessage, patternMessage);
            VerifyEqual("22. Verify search result message is: '{n} devices match your search criteria. Click on Finish to see the results'", true, messageRegex.Success);
            VerifyEqual("22. Verify Next button is not present anymore", false, advancedSearchPage.SearchWizardPopupPanel.IsNextButtonVisible());
            VerifyEqual("22. Verify Finish button is present", true, advancedSearchPage.SearchWizardPopupPanel.IsFinishButtonVisible());
            var expectedDevices = int.Parse(messageRegex.Groups[1].ToString());

            Step("23. Click Finish button");
            advancedSearchPage.SearchWizardPopupPanel.ClickFinishButton();
            advancedSearchPage.WaitForSearchWizardPopupPanelDisappeared();
            advancedSearchPage.WaitForPreviousActionComplete();
            advancedSearchPage.GridPanel.WaitForPanelLoaded();

            Step("24. Expected");
            Step(" - The dialog is closed");
            Step(" - Newly-created report is present in the list of advanced search and is selected");
            Step(" - Grid displays data of the newly-created report");
            var tblGrid = GetGridData(advancedSearchPage, expectedDevices);
            VerifyEqual("24. Verify dialog is closed", false, advancedSearchPage.SearchWizardPopupPanel.IsPanelVisible());
            VerifyEqual("24. Verify Newly-created report is present in the list of advanced search and is selected", searchName, advancedSearchPage.GridPanel.GetSelectOrAddSearchValue());
            VerifyEqual("24. Verify Grid displays data of the newly-created report", expectedDevices, tblGrid.Rows.Count);
            var columnHeaders = advancedSearchPage.GridPanel.GetListOfColumnsHeader();            

            Step("25. Verify There is a column 'Category' displaying in the grid");
            VerifyEqual("25. Verify There is a column 'Category' displaying in the grid", true, columnHeaders.Exists(p => p.Equals("Category")));

            Step("26. Select 'Show/Hide Columns' button and uncheck the 'Category'");
            advancedSearchPage.GridPanel.UncheckColumnsInShowHideColumnsMenu("Category");

            Step("27. Verify Category column is hidden in the grid");
            columnHeaders = advancedSearchPage.GridPanel.GetListOfColumnsHeader();
            VerifyEqual("27. Verify Category column is hidden in the grid", true, !columnHeaders.Exists(p => p.Equals("Category")));
            
            Step("28. Un/check Timestamp checkbox");
            advancedSearchPage.GridPanel.ClickTimeStampToolbarButton();

            Step("29. Expected Grid added Timestamp column for each displayed attributed as in the picture");
            columnHeaders.Remove("Device");
            var timestampColCount = advancedSearchPage.GridPanel.GetCountOfTimestampColumn();
            VerifyEqual("29. Verify Grid added Timestamp column for each displayed attributed", true, columnHeaders.Count == timestampColCount);

            Step("30. Click Edit (cog) icon in grid");
            advancedSearchPage.GridPanel.ClickEditButton();
            advancedSearchPage.WaitForSearchWizardPopupPanelDisplayed();

            Step("31. Expected The same as expectation at step #2");
            VerifyEqual("31. Verify New Advanced Search dialog appears with title \"My advanced searches\"", "My advanced searches", advancedSearchPage.SearchWizardPopupPanel.GetPanelTitleText());
            VerifyEqual("31. Verify New search name is selected", searchName, advancedSearchPage.SearchWizardPopupPanel.GetSavedSearchPlaceHolderValue());
            VerifyEqual("31. Verify \"New advanced search\" button text", "New advanced search", advancedSearchPage.SearchWizardPopupPanel.GetAddNewSearchButtonText());

            try
            {
                advancedSearchPage.SearchWizardPopupPanel.ClickCloseButton();
                advancedSearchPage.WaitForSearchWizardPopupPanelDisappeared();
                advancedSearchPage.GridPanel.DeleleSelectedRequest();
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("AS_02 Create new Advanced Search validations")]
        public void AS_02()
        {
            var testData = GetTestDataOfAS_02();
            var xmlGeoZone = testData["Geozone"];
            var xmlFilterName = testData["FilterName"];
            var xmlFilterOperator = testData["FilterOperator"];
            var xmlFilterValue = testData["FilterValue"];

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - There is no custom report");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.AdvancedSearch);

            Step("1. Go to AdvancedSearch app");
            Step("2. Expected AdvancedSearch page is routed. New Advanced Search dialog appears with title \"My advanced searches\":");
            Step(" - \"Select a saved search\" combobox text place holder \"Select a saved search\"");
            Step(" - \"New advanced search\" button with Plus icon");
            var advancedSearchPage = desktopPage.GoToApp(App.AdvancedSearch) as AdvancedSearchPage;
            advancedSearchPage.WaitForSearchWizardPopupPanelDisplayed();

            Step("*** Remove all current searches in dropdown ***");
            advancedSearchPage.SearchWizardPopupPanel.ClickCloseButton();
            advancedSearchPage.WaitForSearchWizardPopupPanelDisappeared();
            advancedSearchPage.GridPanel.DeleleAllRequests();
            advancedSearchPage.GridPanel.ClickEditButton();
            advancedSearchPage.WaitForSearchWizardPopupPanelDisplayed();
            Step("***********************************************");

            VerifyEqual("2. Verify New Advanced Search dialog appears with title \"My advanced searches\"", "My advanced searches", advancedSearchPage.SearchWizardPopupPanel.GetPanelTitleText());
            VerifyEqual("2. Verify \"Select a saved search\" combobox text place holder", "Select a saved search", advancedSearchPage.SearchWizardPopupPanel.GetSavedSearchPlaceHolderValue());
            VerifyEqual("2. Verify \"New advanced search\" button text", "New advanced search", advancedSearchPage.SearchWizardPopupPanel.GetAddNewSearchButtonText());

            Step("3. Dropdown \"Select a saved search\" combobox");
            Step("4. Expected A menu is dropped down with a search text field and an item with text \"No matches found\"");
            var savedSearchList = advancedSearchPage.SearchWizardPopupPanel.GetSavedSearchDropDownItems();
            VerifyTrue("Verify there is an item with text \"No matches found\"", savedSearchList.Count == 1 && savedSearchList.FirstOrDefault().Equals("No matches found"), "No matches found", savedSearchList.FirstOrDefault());

            Step("5. Click \"New advanced search\" button");
            advancedSearchPage.SearchWizardPopupPanel.ClickNewAdvancedSearchButton();
            advancedSearchPage.SearchWizardPopupPanel.WaitForNewSearchNameInputVisible();

            Step("6. Expected Next steps is routed:");
            Step(" - An empty textbox to fill new search's name");
            Step(" - \"Select a saved search\" button with magnifier icon");
            Step(" - Next button is not present");
            VerifyEqual("6. Verify An empty textbox to fill new search's name", string.Empty, advancedSearchPage.SearchWizardPopupPanel.GetSearchNameValue());
            VerifyEqual("6. Verify \"Select Advanced Search\" button text", "Select a saved search", advancedSearchPage.SearchWizardPopupPanel.GetSelectSearchButtonText());
            VerifyEqual("6. Verify Next button is not present", false, advancedSearchPage.SearchWizardPopupPanel.IsNextButtonVisible());

            Step("7. Enter a search's name");
            var searchName = SLVHelper.GenerateUniqueName(TestContext.CurrentContext.Test.MethodName);
            advancedSearchPage.SearchWizardPopupPanel.EnterNewSearchNameInput(searchName);

            Step("8. Expected Next button is present");
            VerifyEqual("8. Verify Next button is present", true, advancedSearchPage.SearchWizardPopupPanel.IsNextButtonVisible());

            Step("9. Execute steps from step #12 to the end in test case AS_01 Create new Advanced Search. Filter conditions should be selected (filtering attributes/conditions, leave criteria empty or fill, etc.) randomly");
            Step("10. Expected No matter what filter conditions, once Next button is clicked, next step is routed and finally a new report is created successfully");
            advancedSearchPage.SearchWizardPopupPanel.ClickNextButton();
            advancedSearchPage.SearchWizardPopupPanel.WaitForGeozoneFormDisplayed();
            VerifyEqual("10. Verify Title of the dialog is now report's name plus asterisk (*)", string.Format("{0}*", searchName), advancedSearchPage.SearchWizardPopupPanel.GetPanelTitleText());
            VerifyEqual("10. Verify Back icon is present in dialog's title as well", true, advancedSearchPage.SearchWizardPopupPanel.IsBackButtonVisible());
            VerifyEqual("10. Verify Message \"Select the geozone to search in\" is above", "Select the geozone to search in", advancedSearchPage.SearchWizardPopupPanel.GetSelectGeozoneCaptionText());
            VerifyEqual("10. Verify below is geozone tree", true, advancedSearchPage.SearchWizardPopupPanel.GeozoneTreePopupPanel.IsPanelVisible());
            VerifyEqual("10. Verify Next button is present", true, advancedSearchPage.SearchWizardPopupPanel.IsNextButtonVisible());

            advancedSearchPage.SearchWizardPopupPanel.GeozoneTreePopupPanel.SelectNode(xmlGeoZone);
            advancedSearchPage.SearchWizardPopupPanel.ClickNextButton();
            advancedSearchPage.SearchWizardPopupPanel.WaitForAttributeFormDisplayed();
            VerifyEqual("10. Verify Message \"Select the attributes to display in the search results\"", "Select the attributes to display in the search results", advancedSearchPage.SearchWizardPopupPanel.GetSelectAttributeCaptionText());

            advancedSearchPage.SearchWizardPopupPanel.ClickNextButton();
            advancedSearchPage.SearchWizardPopupPanel.WaitForFilterFormDisplayed();
            VerifyEqual("10. Verify Message \"Filter your results by adding search criteria\"", "Filter your results by adding search criteria", advancedSearchPage.SearchWizardPopupPanel.GetFilterCaptionText());
            VerifyEqual("10. Verify Filter condition panel", true, advancedSearchPage.SearchWizardPopupPanel.IsFilterMenuVisible());
            advancedSearchPage.SearchWizardPopupPanel.SelectFirstFilterNameDropDown(xmlFilterName);
            advancedSearchPage.SearchWizardPopupPanel.SelectFirstFilterOperatorDropDown(xmlFilterOperator);
            advancedSearchPage.SearchWizardPopupPanel.EnterOrSelectFirstFilterValue(xmlFilterValue);
            advancedSearchPage.SearchWizardPopupPanel.ClickFiltersSaveButton();

            advancedSearchPage.SearchWizardPopupPanel.ClickNextButton();
            advancedSearchPage.SearchWizardPopupPanel.WaitForFinishFormDisplayed();
            advancedSearchPage.SearchWizardPopupPanel.WaitForDeviceSearchCompleted();
            VerifyEqual("10. Verify Next button is not present anymore", false, advancedSearchPage.SearchWizardPopupPanel.IsNextButtonVisible());
            VerifyEqual("10. Verify Finish button is present", true, advancedSearchPage.SearchWizardPopupPanel.IsFinishButtonVisible());

            advancedSearchPage.SearchWizardPopupPanel.ClickFinishButton();
            advancedSearchPage.WaitForSearchWizardPopupPanelDisappeared();
            advancedSearchPage.WaitForPreviousActionComplete();
            advancedSearchPage.GridPanel.WaitForPanelLoaded();

            VerifyEqual("10. Verify dialog is closed", false, advancedSearchPage.SearchWizardPopupPanel.IsPanelVisible());
            VerifyEqual("10. Verify Newly-created report is present in the list of advanced search and is selected", searchName, advancedSearchPage.GridPanel.GetSelectOrAddSearchValue());

            try
            {
                advancedSearchPage.GridPanel.DeleleSelectedRequest();
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("AS_03 Edit existing Advanced Search")]
        public void AS_03()
        {
            var testData = GetTestDataOfAS_03();
            var xmlGeoZone = testData["Geozone"];
            var xmlSearchName1 = SLVHelper.GenerateUniqueName(testData["SearchName1"]);
            var xmlSearchName2 = SLVHelper.GenerateUniqueName(testData["SearchName2"]);

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - There is already at least 1 report");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.AdvancedSearch);

            Step("1. Go to AdvancedSearch app");
            Step("2. Expected AdvancedSearch page is routed. New Advanced Search dialog appears with title \"My advanced searches\":");
            Step(" - \"Select a saved search\" combobox text place holder \"Select a saved search\"");
            Step(" - \"New advanced search\" button with Plus icon");
            var advancedSearchPage = desktopPage.GoToApp(App.AdvancedSearch) as AdvancedSearchPage;
            advancedSearchPage.WaitForSearchWizardPopupPanelDisplayed();

            Step("*** Remove all searches and create new test search in dropdown ***");
            advancedSearchPage.SearchWizardPopupPanel.ClickCloseButton();
            advancedSearchPage.WaitForSearchWizardPopupPanelDisappeared();
            advancedSearchPage.GridPanel.DeleleAllRequests();
            advancedSearchPage.CreateNewSearch(xmlSearchName1, xmlGeoZone);
            advancedSearchPage.CreateNewSearch(xmlSearchName2, xmlGeoZone);
            advancedSearchPage.GridPanel.ClickEditButton();
            advancedSearchPage.WaitForSearchWizardPopupPanelDisplayed();
            Step("***********************************************");

            VerifyEqual("2. Verify New Advanced Search dialog appears with title \"My advanced searches\"", "My advanced searches", advancedSearchPage.SearchWizardPopupPanel.GetPanelTitleText());
            VerifyEqual("2. Verify \"Select a saved search\" combobox text place holder", "Select a saved search", advancedSearchPage.SearchWizardPopupPanel.GetSavedSearchPlaceHolderValue());
            VerifyEqual("2. Verify \"New advanced search\" button text", "New advanced search", advancedSearchPage.SearchWizardPopupPanel.GetAddNewSearchButtonText());

            Step("3.Dropdown \"Select a saved search\" combobox (prepare at least 2 advanced searches)");
            Step("4. Expected Existing Advanced Search are listed in dropdown menu");
            var savedSearchList = advancedSearchPage.SearchWizardPopupPanel.GetSavedSearchDropDownItems();
            VerifyTrue("Verify There is already at least 1 report", savedSearchList.Any() && !savedSearchList.FirstOrDefault().Equals("No matches found"), "Have searches", savedSearchList.FirstOrDefault());

            Step("5. Enter a value into Search text field that matches an existing search's name");
            Step("6. Expected Only matched report is present in the list");
            VerifyEqual("6. Verify Search text field that matches an existing search's name", true, advancedSearchPage.SearchWizardPopupPanel.IsSearchNameExisting(xmlSearchName1));

            Step("7. Clear the search value then enter a value into Search text field that doesn't match an existing search's name");
            Step("8. Expected No search is shown in the list except a text item \"No matched found\"");
            VerifyEqual("8. Verify No search is shown in the list except a text", false, advancedSearchPage.SearchWizardPopupPanel.IsSearchNameExisting(SLVHelper.GenerateUniqueName("Any Search")));

            Step("9. Clear the search value");
            Step("10. Expected All existing reports are displayed");
            var currentSavedSearchList = advancedSearchPage.SearchWizardPopupPanel.GetSavedSearchDropDownItems();
            VerifyEqual("10. Verify All existing reports are displayed", savedSearchList.Count, currentSavedSearchList.Count);

            Step("11. Select a search");
            advancedSearchPage.SearchWizardPopupPanel.SelectSearchNameDropDown(xmlSearchName1);

            Step("12. Expected Finish and Next button are both present");
            VerifyEqual("12. Verify Next button is present", true, advancedSearchPage.SearchWizardPopupPanel.IsNextButtonVisible());
            VerifyEqual("12. Verify Finish button is present", true, advancedSearchPage.SearchWizardPopupPanel.IsFinishButtonVisible());

            Step("13. Click Finish button");
            advancedSearchPage.SearchWizardPopupPanel.ClickFinishButton();
            advancedSearchPage.WaitForSearchWizardPopupPanelDisappeared();
            advancedSearchPage.WaitForPreviousActionComplete();
            advancedSearchPage.GridPanel.WaitForGridContentAvailable();

            Step("14. Expected Selected report is selected in dropdown and its data are displayed in grid");
            VerifyEqual("14. Verify Selected report is selected in dropdown", xmlSearchName1, advancedSearchPage.GridPanel.GetSelectOrAddSearchValue());
            var tblGrid = GetGridData(advancedSearchPage, 0);
            VerifyEqual("14. Verify its data are displayed in grid", true, tblGrid.Rows.Count > 0);

            Step("15. Click Edit (cog) icon");
            advancedSearchPage.GridPanel.ClickEditButton();
            advancedSearchPage.WaitForSearchWizardPopupPanelDisplayed();

            Step("16. Expected the same as step #4");
            savedSearchList = advancedSearchPage.SearchWizardPopupPanel.GetSavedSearchDropDownItems();
            VerifyTrue("16. Verify There is already at least 1 report", savedSearchList.Any() && !savedSearchList.FirstOrDefault().Equals("No matches found"), "Have searches", savedSearchList.FirstOrDefault());

            Step("17. Select a search from \"Select a saved search\" combobox");
            advancedSearchPage.SearchWizardPopupPanel.SelectSearchNameDropDown(xmlSearchName2);

            Step("18. Click Next button");
            advancedSearchPage.SearchWizardPopupPanel.ClickNextButton();
            advancedSearchPage.SearchWizardPopupPanel.WaitForGeozoneFormDisplayed();

            Step("19. Expected Next step is routed. Dialog's title now is the name of selected report without asterisk (*) character at the end");
            VerifyEqual("19. Verify Message \"Select the geozone to search in\" is above", "Select the geozone to search in", advancedSearchPage.SearchWizardPopupPanel.GetSelectGeozoneCaptionText());
            VerifyEqual("19. Verify Dialog's title now is the name of selected report without asterisk (*) character at the end", xmlSearchName2, advancedSearchPage.SearchWizardPopupPanel.GetPanelTitleText());

            Step("20. Keep moving Next steps");
            Step("21. All these steps are the same at with steps of test cases AS_01 Create new Advanced Search. However, data of steps' screens are filled with data of selected report");
            VerifyEqual(string.Format("21. Verify selected Geozone is '{0}'", xmlGeoZone.GetChildName()), xmlGeoZone.GetChildName(), advancedSearchPage.SearchWizardPopupPanel.GeozoneTreePopupPanel.GetSelectedNodeName());
            advancedSearchPage.SearchWizardPopupPanel.ClickNextButton();
            advancedSearchPage.SearchWizardPopupPanel.WaitForAttributeFormDisplayed();
            VerifyEqual("21. Verify Message \"Select the attributes to display in the search results\"", "Select the attributes to display in the search results", advancedSearchPage.SearchWizardPopupPanel.GetSelectAttributeCaptionText());
            advancedSearchPage.SearchWizardPopupPanel.ClickNextButton();
            advancedSearchPage.SearchWizardPopupPanel.WaitForFilterFormDisplayed();
            VerifyEqual("21. Verify Message \"Filter your results by adding search criteria\"", "Filter your results by adding search criteria", advancedSearchPage.SearchWizardPopupPanel.GetFilterCaptionText());
            advancedSearchPage.SearchWizardPopupPanel.ClickNextButton();
            advancedSearchPage.SearchWizardPopupPanel.WaitForFinishFormDisplayed();
            advancedSearchPage.SearchWizardPopupPanel.WaitForDeviceSearchCompleted();

            var patternMessage = @"(\d*) devices match your search criteria. Click on Finish to see the results";
            var actualMessage = string.Format("{0} {1}", advancedSearchPage.SearchWizardPopupPanel.GetCriteriaMessageText(), advancedSearchPage.SearchWizardPopupPanel.GetFinishMessageText());
            var messageRegex = Regex.Match(actualMessage, patternMessage);
            VerifyEqual("21. Verify search result message is: '{n} devices match your search criteria. Click on Finish to see the results'", true, messageRegex.Success);
            VerifyEqual("21. Verify Next button is not present anymore", false, advancedSearchPage.SearchWizardPopupPanel.IsNextButtonVisible());
            VerifyEqual("21. Verify Finish button is present", true, advancedSearchPage.SearchWizardPopupPanel.IsFinishButtonVisible());
            var expectedDevices = int.Parse(messageRegex.Groups[1].ToString());

            advancedSearchPage.SearchWizardPopupPanel.ClickFinishButton();
            advancedSearchPage.WaitForSearchWizardPopupPanelDisappeared();
            advancedSearchPage.WaitForPreviousActionComplete();
            advancedSearchPage.GridPanel.WaitForGridContentAvailable();
            tblGrid = GetGridData(advancedSearchPage, expectedDevices);
            VerifyEqual("21. Verify dialog is closed", false, advancedSearchPage.SearchWizardPopupPanel.IsPanelVisible());
            VerifyEqual("21. Verify Grid displays data of the current report", expectedDevices, tblGrid.Rows.Count);

            try
            {
                //Delete 2 searches
                advancedSearchPage.GridPanel.DeleleRequest(xmlSearchName2);
                advancedSearchPage.GridPanel.DeleleRequest(xmlSearchName1);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("AS_04 Create new Advanced Search with device filters")]
        public void AS_04()
        {
            var testData = GetTestDataOfAS_04();
            var xmlGeoZone = testData["Geozone"];
            var xmlFilterName = testData["FilterName"];
            var xmlFilterOperator = testData["FilterOperator"];
            var xmlFilterValue = testData["FilterValue"];

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - There is no custom report");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.AdvancedSearch);

            Step("1. Go to AdvancedSearch app");
            Step("2. Expected AdvancedSearch page is routed. New Advanced Search dialog appears with title \"My advanced searches\":");
            Step(" - \"Select a saved search\" combobox text place holder \"Select a saved search\"");
            Step(" - \"New advanced search\" button with Plus icon");
            var advancedSearchPage = desktopPage.GoToApp(App.AdvancedSearch) as AdvancedSearchPage;
            advancedSearchPage.WaitForSearchWizardPopupPanelDisplayed();

            Step("*** Remove all current searches in dropdown ***");
            advancedSearchPage.SearchWizardPopupPanel.ClickCloseButton();
            advancedSearchPage.WaitForSearchWizardPopupPanelDisappeared();
            advancedSearchPage.GridPanel.DeleleAllRequests();
            advancedSearchPage.GridPanel.ClickEditButton();
            advancedSearchPage.WaitForSearchWizardPopupPanelDisplayed();
            Step("***********************************************");

            VerifyEqual("2. Verify New Advanced Search dialog appears with title \"My advanced searches\"", "My advanced searches", advancedSearchPage.SearchWizardPopupPanel.GetPanelTitleText());
            VerifyEqual("2. Verify \"Select a saved search\" combobox text place holder", "Select a saved search", advancedSearchPage.SearchWizardPopupPanel.GetSavedSearchPlaceHolderValue());
            VerifyEqual("2. Verify \"New advanced search\" button text", "New advanced search", advancedSearchPage.SearchWizardPopupPanel.GetAddNewSearchButtonText());

            Step("3. Dropdown \"Select a saved search\" combobox");
            Step("4. Expected A menu is dropped down with a search text field and an item with text \"No matches found\"");
            var savedSearchList = advancedSearchPage.SearchWizardPopupPanel.GetSavedSearchDropDownItems();
            VerifyTrue("4. Verify there is an item with text \"No matches found\"", savedSearchList.Count == 1 && savedSearchList.FirstOrDefault().Equals("No matches found"), "No matches found", savedSearchList.FirstOrDefault());

            Step("5. Click \"New advanced search\" button");
            advancedSearchPage.SearchWizardPopupPanel.ClickNewAdvancedSearchButton();
            advancedSearchPage.SearchWizardPopupPanel.WaitForNewSearchNameInputVisible();

            Step("6. Expected Next steps is routed:");
            Step(" - An empty textbox to fill new search's name");
            Step(" - \"Select a saved search\" button with magnifier icon");
            Step(" - Next button is not present");
            VerifyEqual("6. Verify An empty textbox to fill new search's name", string.Empty, advancedSearchPage.SearchWizardPopupPanel.GetSearchNameValue());
            VerifyEqual("6. Verify \"Select Advanced Search\" button text", "Select a saved search", advancedSearchPage.SearchWizardPopupPanel.GetSelectSearchButtonText());
            VerifyEqual("6. Verify Next button is not present", false, advancedSearchPage.SearchWizardPopupPanel.IsNextButtonVisible());

            Step("7. Click \"Select a saved search\"");
            advancedSearchPage.SearchWizardPopupPanel.ClickSelectSavedSearchButton();
            advancedSearchPage.SearchWizardPopupPanel.WaitForSavedSearhNameDropDownVisible();

            Step("8. Expected Previous step (the first step) is returned");
            VerifyEqual("8. Verify \"Select a saved search\" combobox text place holder", "Select a saved search", advancedSearchPage.SearchWizardPopupPanel.GetSavedSearchPlaceHolderValue());
            VerifyEqual("8. Verify \"New advanced search\" button text", "New advanced search", advancedSearchPage.SearchWizardPopupPanel.GetAddNewSearchButtonText());

            Step("9. Click \"New advanced search\" button to return the step");
            advancedSearchPage.SearchWizardPopupPanel.ClickNewAdvancedSearchButton();
            advancedSearchPage.SearchWizardPopupPanel.WaitForNewSearchNameInputVisible();

            Step("10. Enter a search's name");
            var searchName = SLVHelper.GenerateUniqueName(TestContext.CurrentContext.Test.MethodName);
            advancedSearchPage.SearchWizardPopupPanel.EnterNewSearchNameInput(searchName);

            Step("11. Expected Next button is present");
            VerifyEqual("11. Verify Next button is present", true, advancedSearchPage.SearchWizardPopupPanel.IsNextButtonVisible());

            Step("12. Click Next button");
            advancedSearchPage.SearchWizardPopupPanel.ClickNextButton();
            advancedSearchPage.SearchWizardPopupPanel.WaitForGeozoneFormDisplayed();

            Step("13. Expected Next screen is routed:");
            Step(" - Title of the dialog is now search's name plus asterisk (*); Back icon is present in dialog's title as well");
            Step(" - Message \"Select the geozone to search in\" is above; below is geozone tree");
            Step(" - Next button is available");
            VerifyEqual("13. Verify Title of the dialog is now report's name plus asterisk (*)", string.Format("{0}*", searchName), advancedSearchPage.SearchWizardPopupPanel.GetPanelTitleText());
            VerifyEqual("13. Verify Back icon is present in dialog's title as well", true, advancedSearchPage.SearchWizardPopupPanel.IsBackButtonVisible());
            VerifyEqual("13. Verify Message \"Select the geozone to search in\" is above", "Select the geozone to search in", advancedSearchPage.SearchWizardPopupPanel.GetSelectGeozoneCaptionText());
            VerifyEqual("13. Verify below is geozone tree", true, advancedSearchPage.SearchWizardPopupPanel.GeozoneTreePopupPanel.IsPanelVisible());
            VerifyEqual("13. Verify Next button is present", true, advancedSearchPage.SearchWizardPopupPanel.IsNextButtonVisible());

            Step("14. Select a geozone then click Next");
            advancedSearchPage.SearchWizardPopupPanel.GeozoneTreePopupPanel.SelectNode(xmlGeoZone);
            advancedSearchPage.SearchWizardPopupPanel.ClickNextButton();
            advancedSearchPage.SearchWizardPopupPanel.WaitForAttributeFormDisplayed();

            Step("15. Expected Next step is routed:");
            Step(" - Message \"Select the attributes to display in the search results\"");
            Step(" - List of attributes for selecting");
            VerifyEqual("15. Verify Message \"Select the attributes to display in the search results\"", "Select the attributes to display in the search results", advancedSearchPage.SearchWizardPopupPanel.GetSelectAttributeCaptionText());
            var allAttributeList = advancedSearchPage.SearchWizardPopupPanel.GetAllAttributeList();
            VerifyEqual("15. Verify List of attributes has values", true, allAttributeList.Any());

            Step("16. Click Next");
            advancedSearchPage.SearchWizardPopupPanel.ClickNextButton();
            advancedSearchPage.SearchWizardPopupPanel.WaitForFilterFormDisplayed();

            Step("17. Expected Next step is routed:");
            Step(" - Message \"Filter your results by adding search criteria\"");
            Step(" - Filter condition panel");
            VerifyEqual("17. Verify Message \"Filter your results by adding search criteria\"", "Filter your results by adding search criteria", advancedSearchPage.SearchWizardPopupPanel.GetFilterCaptionText());
            VerifyEqual("17. Verify Filter condition panel", true, advancedSearchPage.SearchWizardPopupPanel.IsFilterMenuVisible());

            Step("18. Randomly select attributes/condition/condition criteria to add");
            advancedSearchPage.SearchWizardPopupPanel.SelectFirstFilterNameDropDown(xmlFilterName);
            advancedSearchPage.SearchWizardPopupPanel.SelectFirstFilterOperatorDropDown(xmlFilterOperator);
            advancedSearchPage.SearchWizardPopupPanel.EnterOrSelectFirstFilterValue(xmlFilterValue);
            advancedSearchPage.SearchWizardPopupPanel.ClickFiltersSaveButton();

            Step("19. Expected Added filter is inserted on the top as a row with Remove (Trash) icon at the end of row; The row to add filter is always at the end of the list with Save (Disk) icon");
            VerifyEqual("19. Verify Added filter is inserted on the top as a row with Remove (Trash) icon at the end of row", true, advancedSearchPage.SearchWizardPopupPanel.IsFirstFilterRowHasTrashIcon());
            VerifyEqual("19. Verify The row to add filter is always at the end of the list with Save (Disk) icon", true, advancedSearchPage.SearchWizardPopupPanel.IsLastFilterRowHasSaveIcon());

            Step("20. Click Next");
            advancedSearchPage.SearchWizardPopupPanel.ClickNextButton();
            advancedSearchPage.SearchWizardPopupPanel.WaitForFinishFormDisplayed();
            advancedSearchPage.SearchWizardPopupPanel.WaitForDeviceSearchCompleted();

            Step("21. Expected Next step is routed:");
            Step(" - Message {{n}} devices match your search criteria. Click on Finish to see the results");
            Step(" - Next button is not present anymore, Finish button is present");
            var patternMessage = @"(\d*) devices match your search criteria. Click on Finish to see the results";
            var actualMessage = string.Format("{0} {1}", advancedSearchPage.SearchWizardPopupPanel.GetCriteriaMessageText(), advancedSearchPage.SearchWizardPopupPanel.GetFinishMessageText());
            var messageRegex = Regex.Match(actualMessage, patternMessage);
            VerifyEqual("21. Verify search result message is: '{n} devices match your search criteria. Click on Finish to see the results'", true, messageRegex.Success);
            VerifyEqual("21. Verify Next button is not present anymore", false, advancedSearchPage.SearchWizardPopupPanel.IsNextButtonVisible());
            VerifyEqual("21. Verify Finish button is present", true, advancedSearchPage.SearchWizardPopupPanel.IsFinishButtonVisible());
            var expectedDevices = int.Parse(messageRegex.Groups[1].ToString());

            Step("22. Click Finish button");
            advancedSearchPage.SearchWizardPopupPanel.ClickFinishButton();
            advancedSearchPage.WaitForSearchWizardPopupPanelDisappeared();
            advancedSearchPage.WaitForPreviousActionComplete();
            advancedSearchPage.GridPanel.WaitForPanelLoaded();

            Step("23. Expected");
            Step(" - The dialog is closed");
            Step(" - Newly-created report is present in the list of advanced search and is selected");
            Step(" - Grid displays data of the newly-created report");
            var tblGrid = GetGridData(advancedSearchPage, expectedDevices);
            VerifyEqual("23. Verify dialog is closed", false, advancedSearchPage.SearchWizardPopupPanel.IsPanelVisible());
            VerifyEqual("23. Verify Newly-created report is present in the list of advanced search and is selected", searchName, advancedSearchPage.GridPanel.GetSelectOrAddSearchValue());
            VerifyEqual("23. Verify Grid displays data of the newly-created report", expectedDevices, tblGrid.Rows.Count);

            try
            {
                advancedSearchPage.GridPanel.DeleleSelectedRequest();
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("AS_05 Create new Advanced Search with selected attributes")]
        public void AS_05()
        {
            var testData = GetTestDataOfAS_05();
            var xmlGeoZone = testData["Geozone"].ToString();
            var xmlCheckedAttributes = testData["CheckedAttributes"] as List<string>;

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - There is no custom report");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.AdvancedSearch);

            Step("1. Go to AdvancedSearch app");
            Step("2. Expected AdvancedSearch page is routed. New Advanced Search dialog appears with title \"My advanced searches\":");
            Step(" - \"Select a saved search\" combobox text place holder \"Select a saved search\"");
            Step(" - \"New advanced search\" button with Plus icon");
            var advancedSearchPage = desktopPage.GoToApp(App.AdvancedSearch) as AdvancedSearchPage;
            advancedSearchPage.WaitForSearchWizardPopupPanelDisplayed();

            Step("*** Remove all current searches in dropdown ***");
            advancedSearchPage.SearchWizardPopupPanel.ClickCloseButton();
            advancedSearchPage.WaitForSearchWizardPopupPanelDisappeared();
            advancedSearchPage.GridPanel.DeleleAllRequests();
            advancedSearchPage.GridPanel.ClickEditButton();
            advancedSearchPage.WaitForSearchWizardPopupPanelDisplayed();
            Step("***********************************************");

            VerifyEqual("2. Verify New Advanced Search dialog appears with title \"My advanced searches\"", "My advanced searches", advancedSearchPage.SearchWizardPopupPanel.GetPanelTitleText());
            VerifyEqual("2. Verify \"Select a saved search\" combobox text place holder", "Select a saved search", advancedSearchPage.SearchWizardPopupPanel.GetSavedSearchPlaceHolderValue());
            VerifyEqual("2. Verify \"New advanced search\" button text", "New advanced search", advancedSearchPage.SearchWizardPopupPanel.GetAddNewSearchButtonText());

            Step("3. Dropdown \"Select a saved search\" combobox");
            Step("4. Expected A menu is dropped down with a search text field and an item with text \"No matches found\"");
            var savedSearchList = advancedSearchPage.SearchWizardPopupPanel.GetSavedSearchDropDownItems();
            VerifyTrue("4. Verify there is an item with text \"No matches found\"", savedSearchList.Count == 1 && savedSearchList.FirstOrDefault().Equals("No matches found"), "No matches found", savedSearchList.FirstOrDefault());

            Step("5. Click \"New advanced search\" button");
            advancedSearchPage.SearchWizardPopupPanel.ClickNewAdvancedSearchButton();
            advancedSearchPage.SearchWizardPopupPanel.WaitForNewSearchNameInputVisible();

            Step("6. Expected Next steps is routed:");
            Step(" - An empty textbox to fill new search's name");
            Step(" - \"Select a saved search\" button with magnifier icon");
            Step(" - Next button is not present");
            VerifyEqual("6. Verify An empty textbox to fill new search's name", string.Empty, advancedSearchPage.SearchWizardPopupPanel.GetSearchNameValue());
            VerifyEqual("6. Verify \"Select Advanced Search\" button text", "Select a saved search", advancedSearchPage.SearchWizardPopupPanel.GetSelectSearchButtonText());
            VerifyEqual("6. Verify Next button is not present", false, advancedSearchPage.SearchWizardPopupPanel.IsNextButtonVisible());

            Step("7. Click \"Select a saved search\"");
            advancedSearchPage.SearchWizardPopupPanel.ClickSelectSavedSearchButton();
            advancedSearchPage.SearchWizardPopupPanel.WaitForSavedSearhNameDropDownVisible();

            Step("8. Expected Previous step (the first step) is returned");
            VerifyEqual("8. Verify \"Select a saved search\" combobox text place holder", "Select a saved search", advancedSearchPage.SearchWizardPopupPanel.GetSavedSearchPlaceHolderValue());
            VerifyEqual("8. Verify \"New advanced search\" button text", "New advanced search", advancedSearchPage.SearchWizardPopupPanel.GetAddNewSearchButtonText());

            Step("9. Click \"New advanced search\" button to return the step");
            advancedSearchPage.SearchWizardPopupPanel.ClickNewAdvancedSearchButton();
            advancedSearchPage.SearchWizardPopupPanel.WaitForNewSearchNameInputVisible();

            Step("10. Enter a search's name");
            var searchName = SLVHelper.GenerateUniqueName(TestContext.CurrentContext.Test.MethodName);
            advancedSearchPage.SearchWizardPopupPanel.EnterNewSearchNameInput(searchName);

            Step("11. Expected Next button is present");
            VerifyEqual("11. Verify Next button is present", true, advancedSearchPage.SearchWizardPopupPanel.IsNextButtonVisible());

            Step("12. Click Next button");
            advancedSearchPage.SearchWizardPopupPanel.ClickNextButton();
            advancedSearchPage.SearchWizardPopupPanel.WaitForGeozoneFormDisplayed();

            Step("13. Expected Next screen is routed:");
            Step(" - Title of the dialog is now search's name plus asterisk (*); Back icon is present in dialog's title as well");
            Step(" - Message \"Select the geozone to search in\" is above; below is geozone tree");
            Step(" - Next button is available");
            var geozoneFormCaption = advancedSearchPage.SearchWizardPopupPanel.GetSelectGeozoneCaptionText();
            VerifyEqual("13. Verify Title of the dialog is now report's name plus asterisk (*)", string.Format("{0}*", searchName), advancedSearchPage.SearchWizardPopupPanel.GetPanelTitleText());
            VerifyEqual("13. Verify Back icon is present in dialog's title as well", true, advancedSearchPage.SearchWizardPopupPanel.IsBackButtonVisible());
            VerifyEqual("13. Verify Message \"Select the geozone to search in\" is above", "Select the geozone to search in", geozoneFormCaption);
            VerifyEqual("13. Verify below is geozone tree", true, advancedSearchPage.SearchWizardPopupPanel.GeozoneTreePopupPanel.IsPanelVisible());
            VerifyEqual("13. Verify Next button is present", true, advancedSearchPage.SearchWizardPopupPanel.IsNextButtonVisible());

            Step("14. Select a geozone then click Next");
            advancedSearchPage.SearchWizardPopupPanel.GeozoneTreePopupPanel.SelectNode(xmlGeoZone);
            advancedSearchPage.SearchWizardPopupPanel.ClickNextButton();
            advancedSearchPage.SearchWizardPopupPanel.WaitForAttributeFormDisplayed();

            Step("15. Expected Next step is routed:");
            Step(" - Message \"Select the attributes to display in the search results\"");
            Step(" - List of attributes for selecting");
            Step(" - Attributes: \"Address 1\", \"Configuration status\", \"Dimming group\", \"Install status\", \"Lamp Wattage(W)\", \"Unique address\" are selected by default");
            VerifyEqual("15. Verify Message \"Select the attributes to display in the search results\"", "Select the attributes to display in the search results", advancedSearchPage.SearchWizardPopupPanel.GetSelectAttributeCaptionText());
            var allAttributeList = advancedSearchPage.SearchWizardPopupPanel.GetAllAttributeList();
            VerifyEqual("15. Verify List of attributes has values", true, allAttributeList.Any());
            var checkedAttributeList = advancedSearchPage.SearchWizardPopupPanel.GetCheckedAttributeList();
            var uncheckedAttributeList = advancedSearchPage.SearchWizardPopupPanel.GetUncheckedAttributeList();
            bool areCheckedAttributesMatched = xmlCheckedAttributes.Count == checkedAttributeList.Count && xmlCheckedAttributes.CheckIfIncluded(checkedAttributeList);
            VerifyTrue("15. Verify Attributes: \"Address 1\", \"Configuration status\", \"Dimming group\", \"Install status\", \"Unique address\" are selected by default", areCheckedAttributesMatched == true, string.Join(", ", xmlCheckedAttributes), string.Join(", ", checkedAttributeList));

            Step("16. Randomly de/select some attributes");
            var expectedCheckedAttributeList = new List<string>(checkedAttributeList);
            var randomUncheckList = checkedAttributeList.PickRandom(2);
            var randomCheckList = uncheckedAttributeList.PickRandom(2);
            expectedCheckedAttributeList.RemoveAll(p => randomUncheckList.Contains(p));
            expectedCheckedAttributeList.AddRange(randomCheckList);

            advancedSearchPage.SearchWizardPopupPanel.CheckAttributeList(randomCheckList.ToArray());
            advancedSearchPage.SearchWizardPopupPanel.UncheckAttributeList(randomUncheckList.ToArray());
            var actualNewCheckedAttributeList = advancedSearchPage.SearchWizardPopupPanel.GetCheckedAttributeList();
            Step("17. Expected Selected attributes are checked; Deselected attributes are unchecked");
            VerifyEqual("17. Verify Selected attributes are checked; Deselected attributes are unchecked", expectedCheckedAttributeList, actualNewCheckedAttributeList, false);

            Step("18. Click Next");
            advancedSearchPage.SearchWizardPopupPanel.ClickNextButton();
            advancedSearchPage.SearchWizardPopupPanel.WaitForFilterFormDisplayed();

            Step("19. Expected Next step is routed:");
            Step(" - Message \"Filter your results by adding search criteria\"");
            Step(" - Filter condition panel");
            VerifyEqual("19. Verify Message \"Filter your results by adding search criteria\"", "Filter your results by adding search criteria", advancedSearchPage.SearchWizardPopupPanel.GetFilterCaptionText());
            VerifyEqual("19. Verify Filter condition panel", true, advancedSearchPage.SearchWizardPopupPanel.IsFilterMenuVisible());

            Step("20. Click Next");
            advancedSearchPage.SearchWizardPopupPanel.ClickNextButton();
            advancedSearchPage.SearchWizardPopupPanel.WaitForFinishFormDisplayed();
            advancedSearchPage.SearchWizardPopupPanel.WaitForDeviceSearchCompleted();

            Step("21. Expected Next step is routed:");
            Step(" - Message {{n}} devices match your search criteria. Click on Finish to see the results");
            Step(" - Next button is not present anymore, Finish button is present");
            var patternMessage = @"(\d*) devices match your search criteria. Click on Finish to see the results";
            var actualMessage = string.Format("{0} {1}", advancedSearchPage.SearchWizardPopupPanel.GetCriteriaMessageText(), advancedSearchPage.SearchWizardPopupPanel.GetFinishMessageText());
            var messageRegex = Regex.Match(actualMessage, patternMessage);
            VerifyEqual("21. Verify search result message is: '{n} devices match your search criteria. Click on Finish to see the results'", true, messageRegex.Success);
            VerifyEqual("21. Verify Next button is not present anymore", false, advancedSearchPage.SearchWizardPopupPanel.IsNextButtonVisible());
            VerifyEqual("21. Verify Finish button is present", true, advancedSearchPage.SearchWizardPopupPanel.IsFinishButtonVisible());
            var expectedDevices = int.Parse(messageRegex.Groups[1].ToString());

            Step("22. Click Finish button");
            advancedSearchPage.SearchWizardPopupPanel.ClickFinishButton();
            advancedSearchPage.WaitForSearchWizardPopupPanelDisappeared();
            advancedSearchPage.WaitForPreviousActionComplete();
            advancedSearchPage.GridPanel.WaitForPanelLoaded();

            Step("23. Expected");
            Step(" - The dialog is closed");
            Step(" - Newly-created report is present in the list of advanced search and is selected");
            Step(" - Grid displays data of the newly-created report");
            var tblGrid = GetGridData(advancedSearchPage, expectedDevices);
            VerifyEqual("23. Verify dialog is closed", false, advancedSearchPage.SearchWizardPopupPanel.IsPanelVisible());
            VerifyEqual("23. Verify Newly-created report is present in the list of advanced search and is selected", searchName, advancedSearchPage.GridPanel.GetSelectOrAddSearchValue());
            VerifyEqual("23. Verify Grid displays data of the newly-created report", expectedDevices, tblGrid.Rows.Count);
            var columnHeaders = advancedSearchPage.GridPanel.GetListOfColumnsHeader();
            columnHeaders.Remove("Device");

            Step("24. Un/check Timestamp checkbox");
            advancedSearchPage.GridPanel.ClickTimeStampToolbarButton();

            Step("25. Expected Grid added Timestamp column for each displayed attributed as in the picture");
            var timestampColCount = advancedSearchPage.GridPanel.GetCountOfTimestampColumn();
            VerifyEqual("25. Verify Grid added Timestamp column for each displayed attributed", true, columnHeaders.Count == timestampColCount);

            Step("26. Click Edit (cog) icon in grid");
            advancedSearchPage.GridPanel.ClickEditButton();
            advancedSearchPage.WaitForSearchWizardPopupPanelDisplayed();

            Step("27. Expected The same as expectation at step #2");
            VerifyEqual("27. Verify New Advanced Search dialog appears with title \"My advanced searches\"", "My advanced searches", advancedSearchPage.SearchWizardPopupPanel.GetPanelTitleText());
            VerifyEqual("27. Verify New search name is selected", searchName, advancedSearchPage.SearchWizardPopupPanel.GetSavedSearchPlaceHolderValue());
            VerifyEqual("27. Verify \"New advanced search\" button text", "New advanced search", advancedSearchPage.SearchWizardPopupPanel.GetAddNewSearchButtonText());

            try
            {
                advancedSearchPage.SearchWizardPopupPanel.ClickCloseButton();
                advancedSearchPage.WaitForSearchWizardPopupPanelDisappeared();
                advancedSearchPage.GridPanel.DeleleSelectedRequest();
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("AS_06 Show-Hide columns")]
        public void AS_06()
        {
            var testData = GetTestDataOfAS_06();
            var xmlGeoZone = testData["Geozone"];

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - There is already at least 1 report");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.AdvancedSearch);

            Step("1. Go to Advanced Search page from Desktop or App Switch");
            Step("2. If \"My Advanced Search\" dialog appears, close it");
            var advancedSearchPage = desktopPage.GoToApp(App.AdvancedSearch) as AdvancedSearchPage;
            advancedSearchPage.WaitForSearchWizardPopupPanelDisplayed();

            Step("*** Remove all searches and create new test search in dropdown ***");
            advancedSearchPage.SearchWizardPopupPanel.ClickCloseButton();
            advancedSearchPage.WaitForSearchWizardPopupPanelDisappeared();
            advancedSearchPage.GridPanel.DeleleAllRequests();
            Step("***********************************************");

            Step("3. Dropdown \"Select a saved search\" combobox");
            Step("4. In Main panel, click Columns icon");
            var checkedColumns = advancedSearchPage.GridPanel.GetAllCheckedColumnsInShowHideColumnsMenu();
            var uncheckedColumns = advancedSearchPage.GridPanel.GetAllUncheckedColumnsInShowHideColumnsMenu();
            advancedSearchPage.AppBar.ClickHeaderBartop();

            Step("5. Expected All checked columns are displayed in grid");
            Step(" - List of columns are dropped down");
            Step(" - Columns which is being displayed in the grid are checked");
            var gridColumns = advancedSearchPage.GridPanel.GetListOfColumnsHeader();
            gridColumns.Remove("Device");
            VerifyEqual("5. Verify List of columns are dropped down", true, checkedColumns.Any());
            VerifyEqual("5. Verify Columns which is being displayed in the grid are checked", checkedColumns, gridColumns, false);

            Step("6. Un/check some columns");
            var randomCheckedColumns = checkedColumns.PickRandom(2);
            var randomuncheckedColumns = uncheckedColumns.PickRandom(2);
            advancedSearchPage.GridPanel.UncheckColumnsInShowHideColumnsMenu(randomCheckedColumns.ToArray());
            advancedSearchPage.GridPanel.CheckColumnsInShowHideColumnsMenu(randomuncheckedColumns.ToArray());

            checkedColumns = advancedSearchPage.GridPanel.GetAllCheckedColumnsInShowHideColumnsMenu();

            Step("7. Expected");
            Step(" - Unchecked columns are hide in the grid while checked columns are displayed in the grid");
            Step(" - If Timestamp checkbox is checked: each displayed column is displayed in grid with a header which is devided into 2 rows: 1st row is column name, 2nd row is \"Value\" and \"Timestamp\"");
            Step(" - If Timestamp checkbox is unchecked, only column name is displayed");

            gridColumns = advancedSearchPage.GridPanel.GetListOfColumnsHeader();
            gridColumns.Remove("Device");
            VerifyEqual("7.1 Verify Unchecked columns are hide in the grid while checked columns are displayed in the grid", checkedColumns, gridColumns, false);
            advancedSearchPage.AppBar.ClickHeaderBartop();

            var isTimeStampButtonChecked = advancedSearchPage.GridPanel.IsTimeStampButtonChecked();
            if (!isTimeStampButtonChecked) advancedSearchPage.GridPanel.ClickTimeStampToolbarButton();

            var isColumnHasValueAndTimestamp = true;
            foreach (var column in gridColumns)
            {
                if (!advancedSearchPage.GridPanel.IsColumnHasValueAndTimestamp(column))
                {
                    isColumnHasValueAndTimestamp = false;
                    break;
                }
            }

            VerifyEqual("7.2 Verify If Timestamp checkbox is checked: each displayed column is displayed in grid with a header which is devided into 2 rows: 1st row is column name, 2nd row is \"Value\" and \"Timestamp\"", true, isColumnHasValueAndTimestamp);

            var isColumnHasNoValueAndTimestamp = true;
            advancedSearchPage.GridPanel.ClickTimeStampToolbarButton();
            foreach (var column in gridColumns)
            {
                if (advancedSearchPage.GridPanel.IsColumnHasValueAndTimestamp(column))
                {
                    isColumnHasNoValueAndTimestamp = false;
                    break;
                }
            }
            VerifyEqual("7.3 Verify If Timestamp checkbox is unchecked, only column name is displayed", true, isColumnHasNoValueAndTimestamp);
        }

        [Test, DynamicRetry]
        [Description("AS_07 Advanced Search with all columns")]
        public void AS_07()
        {
            var testData = GetTestDataOfAS_07();
            var xmlGeoZone = testData["Geozone"].ToString();
            var xmlFilterName = testData["FilterName"];
            var xmlFilterOperator = testData["FilterOperator"];
            var xmlFilterValue = testData["FilterValue"];

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - There is no custom report");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.AdvancedSearch);

            Step("1. Run test cases \"AS_01 Create new Advanced Search\" with all attributes selected");
            Step("2. Expected Same at test case AS_01 with all attributes displayed in grid after generated");
            var advancedSearchPage = desktopPage.GoToApp(App.AdvancedSearch) as AdvancedSearchPage;
            advancedSearchPage.WaitForSearchWizardPopupPanelDisplayed();

            Step("*** Remove all current searches in dropdown ***");
            advancedSearchPage.SearchWizardPopupPanel.ClickCloseButton();
            advancedSearchPage.WaitForSearchWizardPopupPanelDisappeared();
            advancedSearchPage.GridPanel.DeleleAllRequests();
            advancedSearchPage.GridPanel.ClickEditButton();
            advancedSearchPage.WaitForSearchWizardPopupPanelDisplayed();
            Step("***********************************************");

            VerifyEqual("2. Verify New Advanced Search dialog appears with title \"My advanced searches\"", "My advanced searches", advancedSearchPage.SearchWizardPopupPanel.GetPanelTitleText());
            VerifyEqual("2. Verify \"Select a saved search\" combobox text place holder", "Select a saved search", advancedSearchPage.SearchWizardPopupPanel.GetSavedSearchPlaceHolderValue());
            VerifyEqual("2. Verify \"New advanced search\" button text", "New advanced search", advancedSearchPage.SearchWizardPopupPanel.GetAddNewSearchButtonText());
            Step("-> Dropdown \"Select a saved search\" combobox");
            var savedSearchList = advancedSearchPage.SearchWizardPopupPanel.GetSavedSearchDropDownItems();
            VerifyTrue("2. Verify there is an item with text \"No matches found\"", savedSearchList.Count == 1 && savedSearchList.FirstOrDefault().Equals("No matches found"), "No matches found", savedSearchList.FirstOrDefault());

            Step("-> Click \"New advanced search\" button");
            advancedSearchPage.SearchWizardPopupPanel.ClickNewAdvancedSearchButton();
            advancedSearchPage.SearchWizardPopupPanel.WaitForNewSearchNameInputVisible();
            VerifyEqual("2. Verify An empty textbox to fill new search's name", string.Empty, advancedSearchPage.SearchWizardPopupPanel.GetSearchNameValue());
            VerifyEqual("2. Verify \"Select Advanced Search\" button text", "Select a saved search", advancedSearchPage.SearchWizardPopupPanel.GetSelectSearchButtonText());
            VerifyEqual("2. Verify Next button is not present", false, advancedSearchPage.SearchWizardPopupPanel.IsNextButtonVisible());

            Step("-> Click \"Select a saved search\"");
            advancedSearchPage.SearchWizardPopupPanel.ClickSelectSavedSearchButton();
            advancedSearchPage.SearchWizardPopupPanel.WaitForSavedSearhNameDropDownVisible();

            Step("-> Expected Previous step (the first step) is returned");
            VerifyEqual("2. Verify \"Select a saved search\" combobox text place holder", "Select a saved search", advancedSearchPage.SearchWizardPopupPanel.GetSavedSearchPlaceHolderValue());
            VerifyEqual("2. Verify \"New advanced search\" button text", "New advanced search", advancedSearchPage.SearchWizardPopupPanel.GetAddNewSearchButtonText());

            var searchName = SLVHelper.GenerateUniqueName(TestContext.CurrentContext.Test.MethodName);
            advancedSearchPage.SearchWizardPopupPanel.ClickNewAdvancedSearchButton();
            advancedSearchPage.SearchWizardPopupPanel.WaitForNewSearchNameInputVisible();
            advancedSearchPage.SearchWizardPopupPanel.EnterNewSearchNameInput(searchName);
            advancedSearchPage.SearchWizardPopupPanel.ClickNextButton();
            advancedSearchPage.SearchWizardPopupPanel.WaitForGeozoneFormDisplayed();
            advancedSearchPage.SearchWizardPopupPanel.GeozoneTreePopupPanel.SelectNode(xmlGeoZone);
            advancedSearchPage.SearchWizardPopupPanel.ClickNextButton();
            advancedSearchPage.SearchWizardPopupPanel.WaitForAttributeFormDisplayed();
            advancedSearchPage.SearchWizardPopupPanel.CheckAllAttributeList();
            var allAttributes = advancedSearchPage.SearchWizardPopupPanel.GetAllAttributeList();
            advancedSearchPage.SearchWizardPopupPanel.ClickNextButton();
            advancedSearchPage.SearchWizardPopupPanel.WaitForFilterFormDisplayed();
            advancedSearchPage.SearchWizardPopupPanel.ClickNextButton();
            advancedSearchPage.SearchWizardPopupPanel.WaitForFinishFormDisplayed();
            advancedSearchPage.SearchWizardPopupPanel.WaitForDeviceSearchCompleted();
            var messageRegex = Regex.Match(string.Format("{0} {1}", advancedSearchPage.SearchWizardPopupPanel.GetCriteriaMessageText(), advancedSearchPage.SearchWizardPopupPanel.GetFinishMessageText()), @"(\d*) devices match your search criteria. Click on Finish to see the results");
            var searchDevicesCount = int.Parse(messageRegex.Groups[1].ToString());
            advancedSearchPage.SearchWizardPopupPanel.ClickFinishButton();
            advancedSearchPage.WaitForSearchWizardPopupPanelDisappeared();
            advancedSearchPage.WaitForPreviousActionComplete();
            advancedSearchPage.GridPanel.WaitForGridContentAvailable();

            var tblGrid = GetGridData(advancedSearchPage, searchDevicesCount);
            VerifyEqual("2. Verify dialog is closed", false, advancedSearchPage.SearchWizardPopupPanel.IsPanelVisible());
            VerifyEqual("2. Verify Newly-created report is present in the list of advanced search and is selected", searchName, advancedSearchPage.GridPanel.GetSelectOrAddSearchValue());
            VerifyEqual("2. Verify Grid displays data of the newly-created report", searchDevicesCount, tblGrid.Rows.Count);
            var columnHeaders = advancedSearchPage.GridPanel.GetListOfColumnsHeader();
            columnHeaders.Remove("Device");
            VerifyEqual("2. Verify All attributes displayed in grid after generated", allAttributes, columnHeaders, false);

            Step("-> Un/check Timestamp checkbox");
            advancedSearchPage.GridPanel.ClickTimeStampToolbarButton();
            var timestampColCount = advancedSearchPage.GridPanel.GetCountOfTimestampColumn();
            VerifyEqual("2. Verify Grid added Timestamp column for each displayed attributed", true, columnHeaders.Count == timestampColCount);

            advancedSearchPage.GridPanel.ClickEditButton();
            advancedSearchPage.WaitForSearchWizardPopupPanelDisplayed();
            VerifyEqual("2. Verify New Advanced Search dialog appears with title \"My advanced searches\"", "My advanced searches", advancedSearchPage.SearchWizardPopupPanel.GetPanelTitleText());
            VerifyEqual("2. Verify New search name is selected", searchName, advancedSearchPage.SearchWizardPopupPanel.GetSavedSearchPlaceHolderValue());
            VerifyEqual("2. Verify \"New advanced search\" button text", "New advanced search", advancedSearchPage.SearchWizardPopupPanel.GetAddNewSearchButtonText());

            advancedSearchPage.SearchWizardPopupPanel.ClickCloseButton();
            advancedSearchPage.WaitForSearchWizardPopupPanelDisappeared();
            advancedSearchPage.GridPanel.DeleleSelectedRequest();

            Step("3. Run test cases \"AS_02 Create new Advanced Search validations\" with all attributes selected");
            Step("4. Expected Same at test case AS_02 with all attributes displayed in grid after generated");
            advancedSearchPage.GridPanel.ClickEditButton();
            advancedSearchPage.WaitForSearchWizardPopupPanelDisplayed();
            VerifyEqual("4. Verify New Advanced Search dialog appears with title \"My advanced searches\"", "My advanced searches", advancedSearchPage.SearchWizardPopupPanel.GetPanelTitleText());
            VerifyEqual("4. Verify \"Select a saved search\" combobox text place holder", "Select a saved search", advancedSearchPage.SearchWizardPopupPanel.GetSavedSearchPlaceHolderValue());
            VerifyEqual("4. Verify \"New advanced search\" button text", "New advanced search", advancedSearchPage.SearchWizardPopupPanel.GetAddNewSearchButtonText());

            Step("-> Dropdown \"Select a saved search\" combobox");
            savedSearchList = advancedSearchPage.SearchWizardPopupPanel.GetSavedSearchDropDownItems();
            VerifyTrue("4. Verify there is an item with text \"No matches found\"", savedSearchList.Count == 1 && savedSearchList.FirstOrDefault().Equals("No matches found"), "No matches found", savedSearchList.FirstOrDefault());
            advancedSearchPage.SearchWizardPopupPanel.ClickNewAdvancedSearchButton();
            advancedSearchPage.SearchWizardPopupPanel.WaitForNewSearchNameInputVisible();
            advancedSearchPage.SearchWizardPopupPanel.EnterNewSearchNameInput(searchName);
            advancedSearchPage.SearchWizardPopupPanel.ClickNextButton();
            advancedSearchPage.SearchWizardPopupPanel.WaitForGeozoneFormDisplayed();
            advancedSearchPage.SearchWizardPopupPanel.GeozoneTreePopupPanel.SelectNode(xmlGeoZone);
            advancedSearchPage.SearchWizardPopupPanel.ClickNextButton();
            advancedSearchPage.SearchWizardPopupPanel.WaitForAttributeFormDisplayed();
            advancedSearchPage.SearchWizardPopupPanel.CheckAllAttributeList();
            allAttributes = advancedSearchPage.SearchWizardPopupPanel.GetAllAttributeList();
            advancedSearchPage.SearchWizardPopupPanel.ClickNextButton();
            advancedSearchPage.SearchWizardPopupPanel.WaitForFilterFormDisplayed();
            advancedSearchPage.SearchWizardPopupPanel.SelectFirstFilterNameDropDown(xmlFilterName);
            advancedSearchPage.SearchWizardPopupPanel.SelectFirstFilterOperatorDropDown(xmlFilterOperator);
            advancedSearchPage.SearchWizardPopupPanel.EnterOrSelectFirstFilterValue(xmlFilterValue);
            advancedSearchPage.SearchWizardPopupPanel.ClickFiltersSaveButton();
            advancedSearchPage.SearchWizardPopupPanel.ClickNextButton();
            advancedSearchPage.SearchWizardPopupPanel.WaitForFinishFormDisplayed();
            advancedSearchPage.SearchWizardPopupPanel.WaitForDeviceSearchCompleted();
            messageRegex = Regex.Match(string.Format("{0} {1}", advancedSearchPage.SearchWizardPopupPanel.GetCriteriaMessageText(), advancedSearchPage.SearchWizardPopupPanel.GetFinishMessageText()), @"(\d*) devices match your search criteria. Click on Finish to see the results");
            searchDevicesCount = int.Parse(messageRegex.Groups[1].ToString());
            advancedSearchPage.SearchWizardPopupPanel.ClickFinishButton();
            advancedSearchPage.WaitForSearchWizardPopupPanelDisappeared();
            advancedSearchPage.WaitForPreviousActionComplete();
            advancedSearchPage.GridPanel.WaitForGridContentAvailable();

            tblGrid = GetGridData(advancedSearchPage, searchDevicesCount);
            VerifyEqual("4. Verify dialog is closed", false, advancedSearchPage.SearchWizardPopupPanel.IsPanelVisible());
            VerifyEqual("4. Verify Newly-created report is present in the list of advanced search and is selected", searchName, advancedSearchPage.GridPanel.GetSelectOrAddSearchValue());
            VerifyEqual("4. Verify Grid displays data of the newly-created report", searchDevicesCount, tblGrid.Rows.Count);
            columnHeaders = advancedSearchPage.GridPanel.GetListOfColumnsHeader();
            columnHeaders.Remove("Device");
            VerifyEqual("4. Verify All attributes displayed in grid after generated", allAttributes, columnHeaders, false);

            Step("-> Un/check Timestamp checkbox");
            advancedSearchPage.GridPanel.ClickTimeStampToolbarButton();
            timestampColCount = advancedSearchPage.GridPanel.GetCountOfTimestampColumn();
            VerifyEqual("4. Verify Grid added Timestamp column for each displayed attributed", true, columnHeaders.Count == timestampColCount);

            advancedSearchPage.GridPanel.ClickEditButton();
            advancedSearchPage.WaitForSearchWizardPopupPanelDisplayed();
            VerifyEqual("4. Verify New Advanced Search dialog appears with title \"My advanced searches\"", "My advanced searches", advancedSearchPage.SearchWizardPopupPanel.GetPanelTitleText());
            VerifyEqual("4. Verify New search name is selected", searchName, advancedSearchPage.SearchWizardPopupPanel.GetSavedSearchPlaceHolderValue());
            VerifyEqual("4. Verify \"New advanced search\" button text", "New advanced search", advancedSearchPage.SearchWizardPopupPanel.GetAddNewSearchButtonText());

            try
            {
                advancedSearchPage.SearchWizardPopupPanel.ClickCloseButton();
                advancedSearchPage.WaitForSearchWizardPopupPanelDisappeared();
                advancedSearchPage.GridPanel.DeleleSelectedRequest();
            }
            catch { }
        }        

        [Test, DynamicRetry]
        [Description("AS_09 Sort columns")]
        public void AS_09()
        {
            var testData = GetTestDataOfAS_09();
            var xmlGeoZone = testData["Geozone"].ToString();
            var searchName = SLVHelper.GenerateUniqueName(TestContext.CurrentContext.Test.MethodName);

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - At least a search created earlier");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.AdvancedSearch);

            Step("1. Go to Advanced Search page from Desktop or App Switch");
            Step("2. If \"My Advanced Search\" dialog appears, close it");
            var advancedSearchPage = desktopPage.GoToApp(App.AdvancedSearch) as AdvancedSearchPage;
            advancedSearchPage.WaitForSearchWizardPopupPanelDisplayed();

            Step("*** Remove all searches and create new test search in dropdown ***");
            advancedSearchPage.SearchWizardPopupPanel.ClickCloseButton();
            advancedSearchPage.WaitForSearchWizardPopupPanelDisappeared();
            advancedSearchPage.GridPanel.DeleleAllRequests();
            advancedSearchPage.CreateNewSearch(searchName, xmlGeoZone);
            Step("***********************************************");

            Step("3. Select a advanced search from search list");
            advancedSearchPage.GridPanel.SelectSelectOrAddSearchDropDown(searchName);
            advancedSearchPage.WaitForPreviousActionComplete();
            advancedSearchPage.GridPanel.WaitForGridContentAvailable();

            Step("4. Expected Data of selected search is loaded into grid");
            var sortedColumnName = "Device";
            var deviceNameList = advancedSearchPage.GridPanel.GetListOfColumnData(sortedColumnName);
            VerifyEqual("4. Verify Data of selected search is loaded into grid", true, deviceNameList.Any());

            Step("5. Click Column header of a column");
            Step(string.Format(" --> Click sort on column '{0}'", sortedColumnName));
            advancedSearchPage.GridPanel.ClickGridColumnHeader(sortedColumnName);
            advancedSearchPage.GridPanel.WaitForLeftFooterTextDisplayed();

            Step("6. Expected Data is sorted by clicked header column ascendingly");
            deviceNameList = FormatDataColumnList(advancedSearchPage.GridPanel.GetListOfColumnData(sortedColumnName));
            VerifyEqual(string.Format("[{0}] Verify Data is sorted by clicked header column ascendingly", sortedColumnName), true, deviceNameList.IsIncreasing());

            Step("7. Click Column header of that column again");
            advancedSearchPage.GridPanel.ClickGridColumnHeader(sortedColumnName);
            advancedSearchPage.GridPanel.WaitForLeftFooterTextDisplayed();

            Step("8. Expected Data is sorted by clicked header column descendingly");
            deviceNameList = FormatDataColumnList(advancedSearchPage.GridPanel.GetListOfColumnData(sortedColumnName));
            VerifyEqual(string.Format("[{0}] Verify Data is sorted by clicked header column descendingly", sortedColumnName), true, deviceNameList.IsDecreasing());

            Step("9. Repeat on other sortable columns");
            Step("10. Expected Data is sorted and displayed accordingly on grid");
            var headers = advancedSearchPage.GridPanel.GetListOfColumnsHeader();
            headers.Remove(sortedColumnName);
            foreach (var header in headers)
            {
                Step(string.Format(" --> Click sort on column '{0}'", header));
                advancedSearchPage.GridPanel.ClickGridColumnHeader(header);
                advancedSearchPage.GridPanel.WaitForLeftFooterTextDisplayed();
                var columnDataList = FormatDataColumnList(advancedSearchPage.GridPanel.GetListOfColumnData(header));
                VerifyEqual(string.Format("[{0}] Verify Data is sorted by clicked header column ascendingly", header), true, columnDataList.IsIncreasing());

                Step(" --> Click Column header of that column again");
                advancedSearchPage.GridPanel.ClickGridColumnHeader(header);
                advancedSearchPage.GridPanel.WaitForLeftFooterTextDisplayed();
                columnDataList = FormatDataColumnList(advancedSearchPage.GridPanel.GetListOfColumnData(header));
                VerifyEqual(string.Format("[{0}] Verify Data is sorted by clicked header column descendingly", header), true, columnDataList.IsDecreasing());
            }
        }

        [Test, DynamicRetry]
        [Ignore("To be deleted (Not Applicable)")]
        [Description("AS_10 Execute an existing Advanced Search")]
        public void AS_10()
        {
        }

        [Test, DynamicRetry]
        [Description("AS_11 Export Advanced Search")]
        [NonParallelizable]
        public void AS_11()
        {
            var testData = GetTestDataOfAS_11();
            var xmlGeoZone = testData["Geozone"].ToString();
            var searchName = SLVHelper.GenerateUniqueName(TestContext.CurrentContext.Test.MethodName);

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - At least a search created earlier");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.AdvancedSearch);

            Step("1. Go to Advanced Search page from Desktop or App Switch");
            Step("2. If \"My Advanced Search\" dialog appears, close it");
            var advancedSearchPage = desktopPage.GoToApp(App.AdvancedSearch) as AdvancedSearchPage;
            advancedSearchPage.WaitForSearchWizardPopupPanelDisplayed();

            Step("3. Select an advanced search (create one if not existing)");
            advancedSearchPage.SearchWizardPopupPanel.ClickCloseButton();
            advancedSearchPage.WaitForSearchWizardPopupPanelDisappeared();
            var searchesList = advancedSearchPage.GridPanel.GetListOfSearchDropDownItems();

            var createdNewSearch = !searchesList.Any();
            if (createdNewSearch)
                advancedSearchPage.CreateNewSearch(searchName, xmlGeoZone);
            else
                searchName = searchesList.PickRandom();

            advancedSearchPage.GridPanel.SelectSelectOrAddSearchDropDown(searchName);
            advancedSearchPage.WaitForPreviousActionComplete();
            advancedSearchPage.GridPanel.WaitForGridContentAvailable();

            Step("4. Click Export (Generate CSV) icon");
            SLVHelper.DeleteAllFilesByPattern(_advancedSeacrhExportedFilePattern);
            advancedSearchPage.GridPanel.ClickExportAdvancedSearchToolbarButton();

            Step("5. Expected After a while, the export icon turns into Download icon");
            advancedSearchPage.WaitForPreviousActionComplete();
            VerifyEqual("5. Verify the export icon turns into Download icon", true, advancedSearchPage.GridPanel.IsDownloadToolbarButtonVisible());

            Step("6. Click Download icon");
            advancedSearchPage.GridPanel.ClickDownloadToolbarButton();
            SLVHelper.SaveDownloads();

            Step("7. Expected A csv is downloaded");
            Step(" - Its name's pattern: 'Advanced_Search.*.csv'");
            VerifyEqual("Verify A CSV with pattern 'Advanced_Search.*.csv'' is downloaded", true, SLVHelper.CheckFileExists(_advancedSeacrhExportedFilePattern));
            Step(" - Its content reflects grid's content");
            var tblGrid = advancedSearchPage.GridPanel.BuildDataTableFromGrid();
            var totalCount = advancedSearchPage.GridPanel.GetTotalCount();
            int recordsOfPage = 200;
            if (totalCount > recordsOfPage)
            {
                var page = totalCount / recordsOfPage;
                for (int j = 0; j < page; j++)
                {
                    advancedSearchPage.GridPanel.ClickFooterPageNextButton();
                    advancedSearchPage.GridPanel.WaitForLeftFooterTextDisplayed();
                    var pageTable = advancedSearchPage.GridPanel.BuildDataTableFromGrid();
                    tblGrid.Merge(pageTable);
                }
            }
            var tblCSV = SLVHelper.BuildDataTableFromLastDownloadedCSV(_advancedSeacrhExportedFilePattern);
            var tblGridFormatted = tblGrid.Copy();
            var tblCSVFormatted = tblCSV.Copy();

            FormatAdvancedSearchDataTables(ref tblGridFormatted, ref tblCSVFormatted);
            VerifyEqual("7. Verify exported CSV file reflects what is being shown in the grid", tblGridFormatted, tblCSVFormatted);

            if (createdNewSearch)
                advancedSearchPage.GridPanel.DeleleSelectedRequest();
        }

        [Test, DynamicRetry]
        [Description("AS_12 Add a request")]
        public void AS_12()
        {
            var testData = GetTestDataOfAS_12();
            var xmlGeoZone = testData["Geozone"].ToString();
            var searchName = SLVHelper.GenerateUniqueName(TestContext.CurrentContext.Test.MethodName);

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - At least a search created earlier");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.AdvancedSearch);

            Step("1. Go to Advanced Search app");
            Step("2. If \"My Advanced Search\" dialog appears, close it");
            var advancedSearchPage = desktopPage.GoToApp(App.AdvancedSearch) as AdvancedSearchPage;
            advancedSearchPage.WaitForSearchWizardPopupPanelDisplayed();

            Step("3. Drop down the search list");
            Step("4. Select a search (Create one if not existing)");
            advancedSearchPage.SearchWizardPopupPanel.ClickCloseButton();
            advancedSearchPage.WaitForSearchWizardPopupPanelDisappeared();
            var searchesList = advancedSearchPage.GridPanel.GetListOfSearchDropDownItems();

            if (!searchesList.Any())
                advancedSearchPage.CreateNewSearch(searchName, xmlGeoZone);
            else
                searchName = searchesList.PickRandom();

            advancedSearchPage.GridPanel.SelectSelectOrAddSearchDropDown(searchName);
            advancedSearchPage.WaitForPreviousActionComplete();
            advancedSearchPage.GridPanel.WaitForGridContentAvailable();

            Step("5. Expected Data of selected search is loaded into grid");
            var searchDeviceList = advancedSearchPage.GridPanel.GetListOfColumnData("Device");
            VerifyEqual("5. Verify Data of selected search is loaded into grid", true, searchDeviceList.Any());

            Step("6. Drop down the search list");
            advancedSearchPage.GridPanel.ClickSearchDropDown();

            Step("7. Enter a name which is not yet existing");
            var newSearch = SLVHelper.GenerateUniqueName("New Auto");
            var newSearchWithAsterisk = newSearch + "*";
            advancedSearchPage.GridPanel.EnterSearchDropDownInputValue(SLVHelper.GenerateUniqueName("New Auto"));

            Step("8. Expected The list is shorten. There is an item entered at step #7 with an asterisk (*)");
            var listItems = advancedSearchPage.GridPanel.GetListOfSearchDropDownItems(false);
            VerifyEqual("8. There is an item entered at step #7 with an asterisk (*)", true, listItems.Contains(newSearchWithAsterisk));

            Step("9. Hit Enter or click on that item");
            advancedSearchPage.GridPanel.HitEnterSearchDropDown();

            Step("10. Expected The item is selected with Plus icon");
            var currentSearch = advancedSearchPage.GridPanel.GetSelectOrAddSearchValue();
            VerifyEqual("10. Verify The item is selected", newSearchWithAsterisk, currentSearch);
            VerifyEqual("10. Verify Plus icon is displayed", true, advancedSearchPage.GridPanel.IsPlusSearchButtonDisplayed());

            Step("11. Click Plus icon");
            advancedSearchPage.GridPanel.ClickPlusSearchDropDownButton();
            advancedSearchPage.WaitForPreviousActionComplete();

            Step("12. Expected");
            Step(" - Asterisk (*) character is cut");
            Step(" - Plus button becomes Save icon");
            Step(" - Data of new search is the data of previously-loaded search");
            currentSearch = advancedSearchPage.GridPanel.GetSelectOrAddSearchValue();
            var newSearchDeviceList = advancedSearchPage.GridPanel.GetListOfColumnData("Device");
            VerifyEqual("12. Verify Asterisk (*) character is cut", newSearch, currentSearch);
            VerifyEqual("12. Verify Plus icon is not displayed", false, advancedSearchPage.GridPanel.IsPlusSearchButtonDisplayed());
            VerifyEqual("12. Verify Save icon is displayed", true, advancedSearchPage.GridPanel.IsSaveSearchButtonDisplayed());
            VerifyEqual("12. Verify Data of new search is the data of previously-loaded searcd", searchDeviceList, newSearchDeviceList, false);

            Step("13. Reload browser then go to Advanced Search again");
            desktopPage = Browser.RefreshLoggedInCMS();
            advancedSearchPage = desktopPage.GoToApp(App.AdvancedSearch) as AdvancedSearchPage;
            advancedSearchPage.WaitForSearchWizardPopupPanelDisplayed();
            advancedSearchPage.SearchWizardPopupPanel.ClickCloseButton();
            advancedSearchPage.WaitForSearchWizardPopupPanelDisappeared();

            Step("14. Expected New search is present in search list");
            searchesList = advancedSearchPage.GridPanel.GetListOfSearchDropDownItems();
            VerifyEqual("14. Verify New search is present in search list", true, searchesList.Contains(newSearch));

            try
            {
                //Remove new search
                advancedSearchPage.GridPanel.SelectSelectOrAddSearchDropDown(newSearch);
                advancedSearchPage.WaitForPreviousActionComplete();
                advancedSearchPage.GridPanel.WaitForGridContentAvailable();
                advancedSearchPage.GridPanel.DeleleSelectedRequest();
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("AS_13 Select an existing request")]
        public void AS_13()
        {
            var testData = GetTestDataOfAS_13();
            var xmlGeoZone = testData["Geozone"].ToString();
            var searchName = SLVHelper.GenerateUniqueName(TestContext.CurrentContext.Test.MethodName);

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - At least a search created earlier");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.AdvancedSearch);

            Step("1. Go to Advanced Search page from Desktop or App Switch");
            Step("2. If \"My Advanced Search\" dialog appears, close it");
            var advancedSearchPage = desktopPage.GoToApp(App.AdvancedSearch) as AdvancedSearchPage;
            advancedSearchPage.WaitForSearchWizardPopupPanelDisplayed();

            Step("3. Drop down the search list");
            Step("4. Select a search (Create one if not existing)");
            advancedSearchPage.SearchWizardPopupPanel.ClickCloseButton();
            advancedSearchPage.WaitForSearchWizardPopupPanelDisappeared();
            var searchesList = advancedSearchPage.GridPanel.GetListOfSearchDropDownItems();

            var createdNewSearch = !searchesList.Any();
            if (createdNewSearch)
                advancedSearchPage.CreateNewSearch(searchName, xmlGeoZone);
            else
                searchName = searchesList.PickRandom();

            advancedSearchPage.GridPanel.SelectSelectOrAddSearchDropDown(searchName);
            advancedSearchPage.WaitForPreviousActionComplete();
            advancedSearchPage.GridPanel.WaitForGridContentAvailable();

            Step("5. Expected Data of selected search is loaded into grid");
            var searchDeviceList = advancedSearchPage.GridPanel.GetListOfColumnData("Device");
            VerifyEqual("5. Verify Data of selected search is loaded into grid", true, searchDeviceList.Any());

            if (createdNewSearch)
                advancedSearchPage.GridPanel.DeleleSelectedRequest();
        }

        [Test, DynamicRetry]
        [Description("AS_14 Open Equipment Inventory of selected device")]
        public void AS_14()
        {
            var testData = GetTestDataOfAS_14();
            var xmlGeoZone = testData["Geozone"].ToString();
            var searchName = SLVHelper.GenerateUniqueName(TestContext.CurrentContext.Test.MethodName);

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - At least a search created earlier");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.AdvancedSearch);

            Step("1. Go to Advanced Search page from Desktop or App Switch");
            Step("2. If \"My Advanced Search\" dialog appears, close it");
            var advancedSearchPage = desktopPage.GoToApp(App.AdvancedSearch) as AdvancedSearchPage;
            advancedSearchPage.WaitForSearchWizardPopupPanelDisplayed();

            Step("3. Drop down the search list");
            Step("4. Select a search (Create one if not existing)");
            advancedSearchPage.SearchWizardPopupPanel.ClickCloseButton();
            advancedSearchPage.WaitForSearchWizardPopupPanelDisappeared();
            var searchesList = advancedSearchPage.GridPanel.GetListOfSearchDropDownItems();
            var createdNewSearch = !searchesList.Any();
            if (createdNewSearch)
                advancedSearchPage.CreateNewSearch(searchName, xmlGeoZone);
            else
                searchName = searchesList.PickRandom();

            advancedSearchPage.GridPanel.SelectSelectOrAddSearchDropDown(searchName);
            advancedSearchPage.WaitForPreviousActionComplete();
            advancedSearchPage.GridPanel.WaitForGridContentAvailable();

            Step("5. Expected Data of selected search is loaded into grid");
            var searchDeviceList = advancedSearchPage.GridPanel.GetListOfColumnData("Device");
            VerifyEqual("5. Verify Data of selected search is loaded into grid", true, searchDeviceList.Any());

            Step("6. Double click on a device in grid");
            var deviceName = searchDeviceList.PickRandom();
            advancedSearchPage.GridPanel.DoubleClickGridRecord(deviceName);
            advancedSearchPage.WaitForSwitcherOverlayPanelDisplayed();

            Step("7. Expected 4 entries to apps are displayed");
            var listAppsSwitcher = advancedSearchPage.SwitcherOverlayPanel.GetListOfAppsToSwitch();
            VerifyEqual("7. Verify 4 entries to apps are displayed", 4, listAppsSwitcher.Count);

            Step("8. Click Equipment Inventory");
            var equipmentInventoryPage = advancedSearchPage.SwitcherOverlayPanel.SwitchToEquipmentInventoryApp();
            equipmentInventoryPage.WaitForDeviceEditorPanelDisplayed();

            Step("9. Expected Equipment Inventory app is routed and display data of selected device");
            VerifyEqual("9. Verify Display data of selected device", deviceName, equipmentInventoryPage.DeviceEditorPanel.GetNameValue());

            try
            {
                //Remove test data if having
                if (createdNewSearch)
                {
                    advancedSearchPage = equipmentInventoryPage.AppBar.SwitchTo(App.AdvancedSearch) as AdvancedSearchPage;
                    advancedSearchPage.GridPanel.DeleleSelectedRequest();
                }
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("AS_15 Open Real-time Control of selected device")]
        public void AS_15()
        {
            var testData = GetTestDataOfAS_15();
            var xmlGeoZone = testData["Geozone"].ToString();
            var searchName = SLVHelper.GenerateUniqueName("AS15");

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - At least a search created earlier");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.AdvancedSearch);

            Step("1. Go to Advanced Search page from Desktop or App Switch");
            Step("2. If \"My Advanced Search\" dialog appears, close it");
            var advancedSearchPage = desktopPage.GoToApp(App.AdvancedSearch) as AdvancedSearchPage;
            advancedSearchPage.WaitForSearchWizardPopupPanelDisplayed();

            Step("3. Drop down the search list");
            Step("4. Select a search (Create one if not existing)");
            advancedSearchPage.CreateNewSearch(searchName, xmlGeoZone, true);
            advancedSearchPage.GridPanel.SelectSelectOrAddSearchDropDown(searchName);
            advancedSearchPage.WaitForPreviousActionComplete();
            advancedSearchPage.GridPanel.WaitForGridContentAvailable();

            Step("5. Expected Data of selected search is loaded into grid");
            var searchDeviceList = advancedSearchPage.GridPanel.GetListOfColumnData("Device");
            searchDeviceList = searchDeviceList.Where(p => p.Contains("Telematics")).ToList();
            VerifyEqual("5. Verify Data of selected search is loaded into grid", true, searchDeviceList.Any());

            Step("6. Double click on a device in grid");
            var deviceName = searchDeviceList.PickRandom();
            advancedSearchPage.GridPanel.DoubleClickGridRecord(deviceName);
            advancedSearchPage.WaitForSwitcherOverlayPanelDisplayed();

            Step("7. Expected 4 entries to apps are displayed");
            var listAppsSwitcher = advancedSearchPage.SwitcherOverlayPanel.GetListOfAppsToSwitch();
            VerifyEqual("7. Verify 4 entries to apps are displayed", 4, listAppsSwitcher.Count);

            Step("8. Click Real-time Control");
            var realtimeControlPage = advancedSearchPage.SwitcherOverlayPanel.SwitchToRealtimeControlApp();
            realtimeControlPage.WaitForStreetlightWidgetDisplayed(deviceName);

            Step("9. Expected Real-time Control app is routed and display data of selected device");
            VerifyEqual("9. Verify Display data of selected device", deviceName, realtimeControlPage.StreetlightWidgetPanel.GetDeviceNameText());

            try
            {
                advancedSearchPage = realtimeControlPage.AppBar.SwitchTo(App.AdvancedSearch) as AdvancedSearchPage;
                advancedSearchPage.GridPanel.DeleleSelectedRequest();
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("AS_16 Open Data History of selected device")]
        public void AS_16()
        {
            var testData = GetTestDataOfAS_16();
            var xmlGeoZone = testData["Geozone"].ToString();
            var searchName = SLVHelper.GenerateUniqueName(TestContext.CurrentContext.Test.MethodName);

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - At least a search created earlier");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.AdvancedSearch);

            Step("1. Go to Advanced Search page from Desktop or App Switch");
            Step("2. If \"My Advanced Search\" dialog appears, close it");
            var advancedSearchPage = desktopPage.GoToApp(App.AdvancedSearch) as AdvancedSearchPage;
            advancedSearchPage.WaitForSearchWizardPopupPanelDisplayed();

            Step("3. Drop down the search list");
            Step("4. Select a search (Create one if not existing)");
            advancedSearchPage.SearchWizardPopupPanel.ClickCloseButton();
            advancedSearchPage.WaitForSearchWizardPopupPanelDisappeared();
            var searchesList = advancedSearchPage.GridPanel.GetListOfSearchDropDownItems();
            var createdNewSearch = !searchesList.Any();
            if (createdNewSearch)
                advancedSearchPage.CreateNewSearch(searchName, xmlGeoZone);
            else
                searchName = searchesList.PickRandom();

            advancedSearchPage.GridPanel.SelectSelectOrAddSearchDropDown(searchName);
            advancedSearchPage.WaitForPreviousActionComplete();
            advancedSearchPage.GridPanel.WaitForGridContentAvailable();

            Step("5. Expected Data of selected search is loaded into grid");
            var searchDeviceList = advancedSearchPage.GridPanel.GetListOfColumnData("Device");
            searchDeviceList = searchDeviceList.Where(p => p.Contains("Telematics")).ToList();
            VerifyEqual("5. Verify Data of selected search is loaded into grid", true, searchDeviceList.Any());

            Step("6. Double click on a device in grid");
            var deviceName = searchDeviceList.PickRandom();
            advancedSearchPage.GridPanel.DoubleClickGridRecord(deviceName);
            advancedSearchPage.WaitForSwitcherOverlayPanelDisplayed();

            Step("7. Expected 4 entries to apps are displayed");
            var listAppsSwitcher = advancedSearchPage.SwitcherOverlayPanel.GetListOfAppsToSwitch();
            VerifyEqual("7. Verify 4 entries to apps are displayed", 4, listAppsSwitcher.Count);

            Step("8. Click Data History");
            var dataHistoryPage = advancedSearchPage.SwitcherOverlayPanel.SwitchToDataHistoryApp();
            dataHistoryPage.WaitForGridPanelDisplayed();

            Step("9. Expected Data History app is routed and display data of selected device");
            VerifyEqual("9. Verify Display data of selected device", deviceName, dataHistoryPage.LastValuesPanel.GetSelectedDeviceText());

            try
            {
                //Remove test data if having
                if (createdNewSearch)
                {
                    advancedSearchPage = dataHistoryPage.AppBar.SwitchTo(App.AdvancedSearch) as AdvancedSearchPage;
                    advancedSearchPage.GridPanel.DeleleSelectedRequest();
                }
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("AS_17 Open Failure Tracking of selected device")]
        public void AS_17()
        {
            var testData = GetTestDataOfAS_17();
            var xmlGeoZone = testData["Geozone"].ToString();
            var searchName = SLVHelper.GenerateUniqueName(TestContext.CurrentContext.Test.MethodName);

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - At least a search created earlier");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.AdvancedSearch);

            Step("1. Go to Advanced Search page from Desktop or App Switch");
            Step("2. If \"My Advanced Search\" dialog appears, close it");
            var advancedSearchPage = desktopPage.GoToApp(App.AdvancedSearch) as AdvancedSearchPage;
            advancedSearchPage.WaitForSearchWizardPopupPanelDisplayed();

            Step("3. Drop down the search list");
            Step("4. Select a search (Create one if not existing)");
            advancedSearchPage.CreateNewSearch(searchName, xmlGeoZone, true);

            advancedSearchPage.GridPanel.SelectSelectOrAddSearchDropDown(searchName);
            advancedSearchPage.WaitForPreviousActionComplete();
            advancedSearchPage.GridPanel.WaitForGridContentAvailable();

            Step("5. Expected Data of selected search is loaded into grid");
            var searchDeviceList = advancedSearchPage.GridPanel.GetListOfColumnData("Device");
            searchDeviceList = searchDeviceList.Where(p => p.Contains("Telematics")).ToList();
            VerifyEqual("5. Verify Data of selected search is loaded into grid", true, searchDeviceList.Any());

            Step("6. Double click on a device in grid");
            var deviceName = searchDeviceList.PickRandom();
            advancedSearchPage.GridPanel.DoubleClickGridRecord(deviceName);
            advancedSearchPage.WaitForSwitcherOverlayPanelDisplayed();

            Step("7. Expected 4 entries to apps are displayed");
            var listAppsSwitcher = advancedSearchPage.SwitcherOverlayPanel.GetListOfAppsToSwitch();
            VerifyEqual("7. Verify 4 entries to apps are displayed", 4, listAppsSwitcher.Count);

            Step("8. Click Failure Tracking");
            var failureTrackingPage = advancedSearchPage.SwitcherOverlayPanel.SwitchToFailureTrackingApp();

            Step("9. Expected Failure Tracking app is routed and display data of selected device");
            VerifyEqual("9 .Verify Display data of selected device", deviceName, failureTrackingPage.GeozoneTreeMainPanel.GetSelectedNodeName());

            try
            {
                advancedSearchPage = failureTrackingPage.AppBar.SwitchTo(App.AdvancedSearch) as AdvancedSearchPage;
                advancedSearchPage.GridPanel.DeleleSelectedRequest();
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("AS_18 SC-1780 Inventory app broken after using the switcher twice from advanced search app")]
        public void AS_18()
        {
            var geozone = Settings.RootGeozoneName;
            var searchName = SLVHelper.GenerateUniqueName(TestContext.CurrentContext.Test.MethodName);

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.AdvancedSearch, App.EquipmentInventory, App.RealTimeControl, App.DataHistory, App.FailureTracking);

            Step("1. Go to Equipment Inventory");
            var equipmentInventoryPage = desktopPage.GoToApp(App.EquipmentInventory) as EquipmentInventoryPage;

            Step("2. Verify Equipment Inventory page is routed");
            VerifyEqual("2. Verify Equipment Inventory page is routed", Settings.RootGeozoneName, equipmentInventoryPage.GeozoneTreeMainPanel.GetSelectedNodeName());
            
            Step("3. Press Application Switcher on the top left corner and select Advanced Search");
            Step("4. Verify Advanced Search page is routed");
            var advancedSearchPage = equipmentInventoryPage.AppBar.SwitchTo(App.AdvancedSearch) as AdvancedSearchPage;
            
            Step("5. Create a new advanced search with default values and selecting the root geozone");
            advancedSearchPage.CreateNewSearch(searchName, geozone, true);
            advancedSearchPage.GridPanel.SelectSelectOrAddSearchDropDown(searchName);
            advancedSearchPage.WaitForPreviousActionComplete();
            advancedSearchPage.GridPanel.WaitForGridContentAvailable();

            Step("6. Verify A list of device is displayed for the advanced search");
            var devices = advancedSearchPage.GridPanel.GetListOfColumnData("Device");
            var device = devices.PickRandom();
            VerifyEqual("6. Verify A list of device is displayed for the advanced search", true, devices.Any());

            Step("7. Double click on a device in grid");
            advancedSearchPage.GridPanel.DoubleClickGridRecord(device);
            advancedSearchPage.WaitForSwitcherOverlayPanelDisplayed();

            Step("8. Verify A pop-up displays for changing screen");
            VerifyEqual("8. Verify A pop-up displays for changing screen", true, advancedSearchPage.IsSwitcherOverlayDisplayed());

            Step("9. Click on the grid again, then double click on another device");
            advancedSearchPage.SwitcherOverlayPanel.ClickSwitcherCancelButton();
            advancedSearchPage.WaitForSwitcherOverlayPanelDisappeared();
            devices.Remove(device);
            device = devices.PickRandom();
            advancedSearchPage.GridPanel.DoubleClickGridRecord(device);
            advancedSearchPage.WaitForSwitcherOverlayPanelDisplayed();

            Step("10. Verify A pop-up displays for changing screen");
            VerifyEqual("10. Verify A pop-up displays for changing screen", true, advancedSearchPage.IsSwitcherOverlayDisplayed());

            Step("11. Click Equipment Inventory");
            equipmentInventoryPage = advancedSearchPage.SwitcherOverlayPanel.SwitchToEquipmentInventoryApp();

            Step("12. Verify Equipment Inventory app is routed and display data of selected device");
            VerifyEqual("[#1291836] 12. Verify Equipment Inventory app is routed and display data of selected device", device, equipmentInventoryPage.GeozoneTreeMainPanel.GetSelectedNodeName());            
            if (equipmentInventoryPage.Map.IsRecorderDisplayed()) Warning("#1291836 - Unable to switch to Equipment Inventory after performing an advanced search in certain cases");

            try
            {
                advancedSearchPage = equipmentInventoryPage.AppBar.SwitchTo(App.AdvancedSearch) as AdvancedSearchPage;
                advancedSearchPage.GridPanel.DeleleSelectedRequest();
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("AS - 1426883 - 1374510 Filter by Category in Advanced Search")]
        public void AS_1426883()
        {
            var testData = GetTestDataOfAS_1426883();
            var geozone = testData["Geozone"].ToString();
            var categories = testData["Categories"] as List<string>;
            var searchName = SLVHelper.GenerateUniqueName(TestContext.CurrentContext.Test.MethodName);

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.AdvancedSearch);

            Step("1. Go to Advanced Search, press 'New Advanced Search' on 'My Advanced searches' pop-up");
            var advancedSearchPage = desktopPage.GoToApp(App.AdvancedSearch) as AdvancedSearchPage;
            advancedSearchPage.WaitForSearchWizardPopupPanelDisplayed();

            Step("2. Enter the name and press Next");
            advancedSearchPage.SearchWizardPopupPanel.ClickNewAdvancedSearchButton();
            advancedSearchPage.SearchWizardPopupPanel.WaitForNewSearchNameInputVisible();
            advancedSearchPage.SearchWizardPopupPanel.EnterNewSearchNameInput(searchName);    

            Step("3. Select the geozone: GeoZones > Automation > All device types, then press Next");
            advancedSearchPage.SearchWizardPopupPanel.ClickNextButton();
            advancedSearchPage.SearchWizardPopupPanel.WaitForGeozoneFormDisplayed();
            advancedSearchPage.SearchWizardPopupPanel.GeozoneTreePopupPanel.SelectNode(geozone);
            advancedSearchPage.SearchWizardPopupPanel.ClickNextButton();
            advancedSearchPage.SearchWizardPopupPanel.WaitForAttributeFormDisplayed();

            Step("4. Check the attribute 'Category', then press Next until finish");
            advancedSearchPage.SearchWizardPopupPanel.CheckAttributeList("Category");
            advancedSearchPage.SearchWizardPopupPanel.ClickNextButton();
            advancedSearchPage.SearchWizardPopupPanel.WaitForFilterFormDisplayed();
            advancedSearchPage.SearchWizardPopupPanel.ClickNextButton();
            advancedSearchPage.SearchWizardPopupPanel.WaitForFinishFormDisplayed();
            advancedSearchPage.SearchWizardPopupPanel.WaitForDeviceSearchCompleted();

            Step("5. Press 'Finish' button");
            advancedSearchPage.SearchWizardPopupPanel.ClickFinishButton();
            advancedSearchPage.WaitForSearchWizardPopupPanelDisappeared();
            advancedSearchPage.WaitForPreviousActionComplete();
            advancedSearchPage.GridPanel.WaitForGridContentAvailable();

            Step("6. Verify The Category column on the grid displays the Category name based on the type of the device.");
            Step(" o Type: Category Name");
            Step(" o Controller: Controller Device");
            Step(" o Building: Building");
            Step(" o Camera: Camera IP");
            Step(" o City object: City object");
            Step(" o Electrical Meter: Electrical Counter");
            Step(" o Sensor: Environmental Sensor");
            Step(" o Input: Inputs");
            Step(" o Nature: Nature");
            Step(" o Network component: Network component");
            Step(" o Occupancy sensor: Occupancy sensor");
            Step(" o Output: Outputs");
            Step(" o Parking place: Parking place");
            Step(" o Streetlight: StreetLight");
            Step(" o Switch: Switch Device");
            Step(" o Tank: Tank");
            Step(" o Transport Signage: Transport Signage");
            Step(" o Vehicle: Vehicle");
            Step(" o Vehicle Charging Station: Vehicle Charging Station");
            Step(" o Weather station: Weather station");
            Step(" o Waste Container: Waste container");
            Step(" o Cabinet Controller: Cabinet Controller");
            var actualCategoryList = advancedSearchPage.GridPanel.GetListOfColumnData("Category");
            VerifyEqual("6. Verify The Category column on the grid displays the Category name based on the type of the device.", categories, actualCategoryList, false);

            Step("7. Press Edit button, then press Next until 'Filter your results by adding search criteria' screen");
            advancedSearchPage.GridPanel.ClickEditButton();
            advancedSearchPage.WaitForSearchWizardPopupPanelDisplayed();
            advancedSearchPage.SearchWizardPopupPanel.ClickNextButton();
            advancedSearchPage.SearchWizardPopupPanel.WaitForGeozoneFormDisplayed();
            advancedSearchPage.SearchWizardPopupPanel.GeozoneTreePopupPanel.SelectNode(geozone);
            advancedSearchPage.SearchWizardPopupPanel.ClickNextButton();
            advancedSearchPage.SearchWizardPopupPanel.WaitForAttributeFormDisplayed();
            advancedSearchPage.SearchWizardPopupPanel.ClickNextButton();
            advancedSearchPage.SearchWizardPopupPanel.WaitForFilterFormDisplayed();          

            Step("8. Select 'Category' as a search criteria with the operator: In");
            var rndCategories = categories.PickRandom(5);
            advancedSearchPage.SearchWizardPopupPanel.SelectFirstFilterNameDropDown("Category");
            advancedSearchPage.SearchWizardPopupPanel.SelectFirstFilterOperatorDropDown("in");           

            Step("9. Select 5 random types of devices in the 3rd textbox, then press Save button and Next");
            foreach (var attribute in rndCategories)
            {
                advancedSearchPage.SearchWizardPopupPanel.SelectFirstFilterMultipleValueDropDown(attribute);
            }
            advancedSearchPage.SearchWizardPopupPanel.ClickFiltersSaveButton();
            advancedSearchPage.SearchWizardPopupPanel.ClickNextButton();
            advancedSearchPage.SearchWizardPopupPanel.WaitForFinishFormDisplayed();
            advancedSearchPage.SearchWizardPopupPanel.WaitForDeviceSearchCompleted();

            Step("10. Verify There is a text displaying '5 devices match your search criteria. Click on Finish to see the results'");
            var expectedMessage = "5 devices match your search criteria. Click on Finish to see the results";
            var acutalMessage = string.Format("{0} {1}", advancedSearchPage.SearchWizardPopupPanel.GetCriteriaMessageText(), advancedSearchPage.SearchWizardPopupPanel.GetFinishMessageText());
            VerifyEqual("10. Verify There is a text displaying '5 devices match your search criteria. Click on Finish to see the results'", expectedMessage, acutalMessage);

            Step("11. Press Finish button");
            advancedSearchPage.SearchWizardPopupPanel.ClickFinishButton();
            advancedSearchPage.WaitForSearchWizardPopupPanelDisappeared();
            advancedSearchPage.WaitForPreviousActionComplete();
            advancedSearchPage.GridPanel.WaitForGridContentAvailable();

            Step("12. Verify The grid displays only 5 devices matching the search criteria");            
            var devices = advancedSearchPage.GridPanel.GetListOfColumnData("Device");
            VerifyEqual("12. Verify The grid displays only 5 devices matching the search criteria", 5, devices.Count);

            Step("13. Verify The Category column displays category of each device in the grid");
            actualCategoryList = advancedSearchPage.GridPanel.GetListOfColumnData("Category");
            VerifyEqual("13. Verify The grid displays only 5 devices matching the search criteria", rndCategories, actualCategoryList, false);

            try
            {
                advancedSearchPage.GridPanel.DeleleSelectedRequest();
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("AS - 1113377 - special char as part of Calendar Name(dimming group name) doesn't work well in Advanced Search")]
        public void AS_1113377()
        {
            var geozone = SLVHelper.GenerateUniqueName("GZNAS1113377");
            var controller = SLVHelper.GenerateUniqueName("CTRL");
            var streetlight = SLVHelper.GenerateUniqueName("STL");
            var searchName = SLVHelper.GenerateUniqueName(TestContext.CurrentContext.Test.MethodName);
            var dimmingGroup = SLVHelper.GenerateUniqueName("Cal" + SLVHelper.GenerateSpecialString());

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - Create a testing geozone containing a streetlight");
            Step(" - In Streetlight Editor panel, set Dimming Group value = a name with spcial chars, then save changes. Ex: ~!@#$%^&*()-+$");
            Step("**** Precondition ****\n");

            Step("-> Create data for testing");
            DeleteGeozones("GZNAS1113377*");
            CreateNewCalendar(dimmingGroup);
            CreateNewGeozone(geozone);
            CreateNewController(controller, geozone);
            CreateNewDevice(DeviceType.Streetlight, streetlight, controller, geozone);
            SetValueToDevice(controller, streetlight, "DimmingGroupName", dimmingGroup, Settings.GetServerTime());

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.AdvancedSearch);

            Step("1. Go to Advanced Search, press 'New Advanced Search' on 'My Advanced searches' pop-up");
            var advancedSearchPage = desktopPage.GoToApp(App.AdvancedSearch) as AdvancedSearchPage;
            advancedSearchPage.WaitForSearchWizardPopupPanelDisplayed();

            Step("2. Enter the name and press Next until the screen with the description 'Filter your results by adding search criteria'");
            advancedSearchPage.SearchWizardPopupPanel.ClickNewAdvancedSearchButton();
            advancedSearchPage.SearchWizardPopupPanel.WaitForNewSearchNameInputVisible();
            advancedSearchPage.SearchWizardPopupPanel.EnterNewSearchNameInput(searchName);            
            advancedSearchPage.SearchWizardPopupPanel.ClickNextButton();
            advancedSearchPage.SearchWizardPopupPanel.WaitForGeozoneFormDisplayed();
            advancedSearchPage.SearchWizardPopupPanel.ClickNextButton();
            advancedSearchPage.SearchWizardPopupPanel.WaitForAttributeFormDisplayed();            
            advancedSearchPage.SearchWizardPopupPanel.ClickNextButton();
            advancedSearchPage.SearchWizardPopupPanel.WaitForFilterFormDisplayed();

            Step("3. Select 'Dimming group' for the combo-box, and select the calendar with special characters in the text-box, then press Save button, and press Next");
            advancedSearchPage.SearchWizardPopupPanel.SelectFirstFilterNameDropDown("Dimming group");
            advancedSearchPage.SearchWizardPopupPanel.SelectFirstFilterOperatorDropDown("in");
            advancedSearchPage.SearchWizardPopupPanel.SelectFirstFilterMultipleValueDropDown(dimmingGroup);
            advancedSearchPage.SearchWizardPopupPanel.ClickFiltersSaveButton();
            advancedSearchPage.SearchWizardPopupPanel.ClickNextButton();
            advancedSearchPage.SearchWizardPopupPanel.WaitForFinishFormDisplayed();
            advancedSearchPage.SearchWizardPopupPanel.WaitForDeviceSearchCompleted();           

            Step("4. Verify In the search pop-up, there is a description '1 devices match your search criteria. Click on Finish to see the results'");
            var expectedMessage = "1 devices match your search criteria. Click on Finish to see the results";
            var acutalMessage = string.Format("{0} {1}", advancedSearchPage.SearchWizardPopupPanel.GetCriteriaMessageText(), advancedSearchPage.SearchWizardPopupPanel.GetFinishMessageText());
            VerifyEqual("4. Verify There is a text displaying '1 devices match your search criteria. Click on Finish to see the results'", expectedMessage, acutalMessage);
            
            Step("5. Press 'Finish' button");
            advancedSearchPage.SearchWizardPopupPanel.ClickFinishButton();
            advancedSearchPage.WaitForSearchWizardPopupPanelDisappeared();
            advancedSearchPage.WaitForPreviousActionComplete();
            advancedSearchPage.GridPanel.WaitForGridContentAvailable();

            Step("6. Verify The streetlight which were assigned the calendar having special name as a Dimming Group should appear in the list.");
            var devices = advancedSearchPage.GridPanel.GetListOfColumnData("Device");
            VerifyTrue("6. Verify The streetlight which were assigned the calendar having special name as a Dimming Group should appear in the list.", devices.Count == 1 && devices.Any(p => p.Equals(streetlight)), streetlight, string.Join(", ", devices));

            try
            {                
                DeleteGeozone(geozone);
                DeleteCalendar(dimmingGroup);
                advancedSearchPage.GridPanel.DeleleSelectedRequest();
            }
            catch { }
        }

        #endregion //Test Cases

        #region Private methods    

        /// <summary>
        /// Formate data of column for checking sort
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public List<string> FormatDataColumnList(List<string> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] == "---" || list[i] == "-")
                    list[i] = string.Empty;

            }
            return list;
        }

        /// <summary>
        /// Make sure 2 compared tables proper formatted before compared
        /// </summary>
        /// <param name="gridDataTable"></param>
        /// <param name="csvDataTable"></param>
        private void FormatAdvancedSearchDataTables(ref DataTable gridDataTable, ref DataTable csvDataTable)
        {
            if (csvDataTable.Columns.Contains("id"))
            {
                csvDataTable.Columns.Remove("id");
            }

            //Ignore checking Lamp Type column because of data is different
            if (gridDataTable.Columns.Contains("Lamp Type"))
            {
                gridDataTable.Columns.Remove("Lamp Type");
            }

            //Ignore checking Lamp Type column because of data is different
            if (csvDataTable.Columns.Contains("brandId"))
            {
                csvDataTable.Columns.Remove("brandId");
            }

            foreach (DataColumn col in csvDataTable.Columns)
            {
                if (col.ColumnName == "name")
                {
                    csvDataTable.Columns["name"].ColumnName = "Device";
                }
                else if (col.ColumnName == "address")
                {
                    csvDataTable.Columns["address"].ColumnName = "Address 1";
                }
                else if (col.ColumnName == "MacAddress")
                {
                    csvDataTable.Columns["MacAddress"].ColumnName = "Unique address";
                }
                else if (col.ColumnName == "DimmingGroupName")
                {
                    csvDataTable.Columns["DimmingGroupName"].ColumnName = "Dimming group";
                }
                else if (col.ColumnName == "installStatus")
                {
                    csvDataTable.Columns["installStatus"].ColumnName = "Install status";
                }
                else if (col.ColumnName == "ConfigStatus")
                {
                    csvDataTable.Columns["ConfigStatus"].ColumnName = "Configuration status";
                }
            }

            foreach (DataColumn col in gridDataTable.Columns)
            {
                foreach (DataRow row in gridDataTable.Rows)
                {
                    if (row[col.ColumnName].ToString() == "---")
                        row[col.ColumnName] = string.Empty;
                }
            }
        }        

        /// <summary>
        /// Get data of grid search
        /// </summary>
        /// <param name="page"></param>
        /// <param name="devicesCount"></param>
        /// <returns></returns>
        public DataTable GetGridData(AdvancedSearchPage page, int devicesCount)
        {
            int recordsOfPage = 200; //default in Back Office config
            var tblGrid = page.GridPanel.BuildDataTableFromGrid();
            if (devicesCount > recordsOfPage)
            {
                var pageCount = devicesCount / recordsOfPage;
                for (int j = 0; j < pageCount; j++)
                {
                    page.GridPanel.ClickFooterPageNextButton();
                    page.GridPanel.WaitForLeftFooterTextDisplayed();
                    var pageTable = page.GridPanel.BuildDataTableFromGrid();
                    tblGrid.Merge(pageTable);
                }
            }

            return tblGrid;
        }

        #region Verify methods

        #endregion //Verify methods

        #region Input XML data

        private Dictionary<string, object> GetTestDataOfAS_01()
        {
            var testCaseName = "AS01";
            var xmlUtility = new XmlUtility(Settings.AS_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, object>();
            testData.Add("Geozone", xmlUtility.GetSingleNodeText(string.Format(Settings.AS_XPATH_PREFIX, testCaseName, "Geozone")));
            testData.Add("AllAttributes", xmlUtility.GetChildNodesText(string.Format(Settings.AS_XPATH_PREFIX, testCaseName, "AllAttributes")));
            testData.Add("CheckedAttributes", xmlUtility.GetChildNodesText(string.Format(Settings.AS_XPATH_PREFIX, testCaseName, "CheckedAttributes")));

            return testData;
        }

        private Dictionary<string, string> GetTestDataOfAS_02()
        {
            var testCaseName = "AS02";
            var xmlUtility = new XmlUtility(Settings.AS_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, string>();
            testData.Add("Geozone", xmlUtility.GetSingleNodeText(string.Format(Settings.AS_XPATH_PREFIX, testCaseName, "Geozone")));
            var filter = xmlUtility.GetSingleNode(string.Format(Settings.AS_XPATH_PREFIX, testCaseName, "Filter"));
            testData.Add("FilterName", filter.GetChildNodeText("Name"));
            testData.Add("FilterOperator", filter.GetChildNodeText("Operator"));
            testData.Add("FilterValue", filter.GetChildNodeText("Value"));

            return testData;
        }

        private Dictionary<string, string> GetTestDataOfAS_03()
        {
            var testCaseName = "AS03";
            var xmlUtility = new XmlUtility(Settings.AS_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, string>();
            testData.Add("Geozone", xmlUtility.GetSingleNodeText(string.Format(Settings.AS_XPATH_PREFIX, testCaseName, "Geozone")));
            testData.Add("SearchName1", xmlUtility.GetSingleNodeText(string.Format(Settings.AS_XPATH_PREFIX, testCaseName, "SearchName1")));
            testData.Add("SearchName2", xmlUtility.GetSingleNodeText(string.Format(Settings.AS_XPATH_PREFIX, testCaseName, "SearchName2")));

            return testData;
        }

        private Dictionary<string, string> GetTestDataOfAS_04()
        {
            var testCaseName = "AS04";
            var xmlUtility = new XmlUtility(Settings.AS_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, string>();
            testData.Add("Geozone", xmlUtility.GetSingleNodeText(string.Format(Settings.AS_XPATH_PREFIX, testCaseName, "Geozone")));
            var filter = xmlUtility.GetSingleNode(string.Format(Settings.AS_XPATH_PREFIX, testCaseName, "Filter"));
            testData.Add("FilterName", filter.GetChildNodeText("Name"));
            testData.Add("FilterOperator", filter.GetChildNodeText("Operator"));
            testData.Add("FilterValue", filter.GetChildNodeText("Value"));

            return testData;
        }

        private Dictionary<string, object> GetTestDataOfAS_05()
        {
            var testCaseName = "AS05";
            var xmlUtility = new XmlUtility(Settings.AS_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, object>();
            testData.Add("Geozone", xmlUtility.GetSingleNodeText(string.Format(Settings.AS_XPATH_PREFIX, testCaseName, "Geozone")));
            testData.Add("CheckedAttributes", xmlUtility.GetChildNodesText(string.Format(Settings.AS_XPATH_PREFIX, testCaseName, "CheckedAttributes")));

            return testData;
        }

        private Dictionary<string, string> GetTestDataOfAS_06()
        {
            var testCaseName = "AS06";
            var xmlUtility = new XmlUtility(Settings.AS_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, string>();
            testData.Add("Geozone", xmlUtility.GetSingleNodeText(string.Format(Settings.AS_XPATH_PREFIX, testCaseName, "Geozone")));

            return testData;
        }

        private Dictionary<string, string> GetTestDataOfAS_07()
        {
            var testCaseName = "AS07";
            var xmlUtility = new XmlUtility(Settings.AS_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, string>();
            testData.Add("Geozone", xmlUtility.GetSingleNodeText(string.Format(Settings.AS_XPATH_PREFIX, testCaseName, "Geozone")));
            var filter = xmlUtility.GetSingleNode(string.Format(Settings.AS_XPATH_PREFIX, testCaseName, "Filter"));
            testData.Add("FilterName", filter.GetChildNodeText("Name"));
            testData.Add("FilterOperator", filter.GetChildNodeText("Operator"));
            testData.Add("FilterValue", filter.GetChildNodeText("Value"));

            return testData;
        }

        private Dictionary<string, string> GetTestDataOfAS_09()
        {
            var testCaseName = "AS09";
            var xmlUtility = new XmlUtility(Settings.AS_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, string>();
            testData.Add("Geozone", xmlUtility.GetSingleNodeText(string.Format(Settings.AS_XPATH_PREFIX, testCaseName, "Geozone")));

            return testData;
        }

        private Dictionary<string, string> GetTestDataOfAS_11()
        {
            var testCaseName = "AS11";
            var xmlUtility = new XmlUtility(Settings.AS_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, string>();
            testData.Add("Geozone", xmlUtility.GetSingleNodeText(string.Format(Settings.AS_XPATH_PREFIX, testCaseName, "Geozone")));

            return testData;
        }

        private Dictionary<string, string> GetTestDataOfAS_12()
        {
            var testCaseName = "AS12";
            var xmlUtility = new XmlUtility(Settings.AS_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, string>();
            testData.Add("Geozone", xmlUtility.GetSingleNodeText(string.Format(Settings.AS_XPATH_PREFIX, testCaseName, "Geozone")));

            return testData;
        }

        private Dictionary<string, string> GetTestDataOfAS_13()
        {
            var testCaseName = "AS13";
            var xmlUtility = new XmlUtility(Settings.AS_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, string>();
            testData.Add("Geozone", xmlUtility.GetSingleNodeText(string.Format(Settings.AS_XPATH_PREFIX, testCaseName, "Geozone")));

            return testData;
        }

        private Dictionary<string, string> GetTestDataOfAS_14()
        {
            var testCaseName = "AS14";
            var xmlUtility = new XmlUtility(Settings.AS_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, string>();
            testData.Add("Geozone", xmlUtility.GetSingleNodeText(string.Format(Settings.AS_XPATH_PREFIX, testCaseName, "Geozone")));

            return testData;
        }

        private Dictionary<string, string> GetTestDataOfAS_15()
        {
            var testCaseName = "AS15";
            var xmlUtility = new XmlUtility(Settings.AS_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, string>();
            testData.Add("Geozone", xmlUtility.GetSingleNodeText(string.Format(Settings.AS_XPATH_PREFIX, testCaseName, "Geozone")));

            return testData;
        }

        private Dictionary<string, string> GetTestDataOfAS_16()
        {
            var testCaseName = "AS16";
            var xmlUtility = new XmlUtility(Settings.AS_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, string>();
            testData.Add("Geozone", xmlUtility.GetSingleNodeText(string.Format(Settings.AS_XPATH_PREFIX, testCaseName, "Geozone")));

            return testData;
        }

        private Dictionary<string, string> GetTestDataOfAS_17()
        {
            var testCaseName = "AS17";
            var xmlUtility = new XmlUtility(Settings.AS_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, string>();
            testData.Add("Geozone", xmlUtility.GetSingleNodeText(string.Format(Settings.AS_XPATH_PREFIX, testCaseName, "Geozone")));

            return testData;
        }

        private Dictionary<string, object> GetTestDataOfAS_1426883()
        {
            var testCaseName = "AS1426883";
            var xmlUtility = new XmlUtility(Settings.AS_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, object>();
            testData.Add("Geozone", xmlUtility.GetSingleNodeText(string.Format(Settings.AS_XPATH_PREFIX, testCaseName, "Geozone")));
            testData.Add("Categories", xmlUtility.GetChildNodesText(string.Format(Settings.AS_XPATH_PREFIX, testCaseName, "Categories")));

            return testData;
        }

        #endregion //Input XML data

        #endregion //Private methods
    }
}
