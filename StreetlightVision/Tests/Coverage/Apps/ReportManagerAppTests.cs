using NUnit.Framework;
using StreetlightVision.Extensions;
using StreetlightVision.Models;
using StreetlightVision.Pages;
using StreetlightVision.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Xml;

namespace StreetlightVision.Tests.Coverage.Apps
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    public class ReportManagerAppTests : TestBase
    {
        #region Variables

        private const int REPORT_WAIT_MINUTES = 5;

        #endregion //Variables

        #region Contructors

        #endregion //Contructors        

        #region Test Cases       

        [Test, DynamicRetry]
        [Description("RM_01 'Citigis report' Report")]
        public void RM_01()
        {
            var testData = GetTestDataOfTestRM_01();

            var xmlGeoZone = testData["GeoZone"].ToString();
            var geozoneName = xmlGeoZone.GetChildName();
            var parentGeozoneName = xmlGeoZone.GetParentName();
            var reportName = SLVHelper.GenerateUniqueName(string.Format("[{0}][{1}] {2}", Browser.Name, Settings.Users["DefaultTest"].Username, TestContext.CurrentContext.Test.MethodName));

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.ReportManager);

            Step("1. Go to Report Manager app");
            Step("2. Expected Report Manager is routed and loaded successfully");
            var reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();

            Step("3. Select a geozone");
            reportManagerPage.GeozoneTreeMainPanel.SelectNode(xmlGeoZone);

            Step("4. Add new report with 'Citigis report' type");
            reportManagerPage.GridPanel.ClickAddReportToolbarButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.ReportEditorPanel.WaitForPanelLoaded();
            reportManagerPage.ReportEditorPanel.SelectTypeDropDown("Citigis report");
            reportManagerPage.WaitForPreviousActionComplete();

            Step("5. Expected Report Details panel appears with section:");
            Step("  o Name");
            Step("  o Type");
            Step("  o Properties tab");
            Step("   * Configuration section");
            Step("    - description");
            Step("    - From (hour, 0-24): range [00-23]");
            Step("    - From (minute, 0-59): range [00-59]");
            Step("    - To (hour, 0-24): range [00-23]");
            Step("    - To (minute, 0-59): range [00-59]");
            Step("    - Periodicity (minutes): range");
            Step("     + 5 minutes");
            Step("     + 10 minutes");
            Step("     + 15 minutes");
            Step("     + 30 minutes");
            Step("     + 1 hour");
            Step("     + 2 hours");
            Step("     + 3 hours");
            Step("     + 6 hours");
            Step("     + 12 hours");
            Step("    - Lamp failures");
            Step("    - Lamp threshold");
            Step("    - Comm. failures");
            Step("    - Comm. threshold");
            Step("  o Export tab");
            Step("   * FTP section");
            Step("    - Host");
            Step("    - SFTP mode");
            Step("    - User");
            Step("    - Password");
            Step("    - Directory");
            Step("    - Passive mode");
            VerifyCitigisReportDetails(reportManagerPage);

            Step("6. Fill values into report fields:");
            Step("  o Name: a unique name");
            Step("  o Other fields (if not specifically indicated):");
            Step("   - Text: Any");
            Step("   - Numeric: Any");
            Step("   - Dropdown: any in list, some in list (multi-selectable)");
            Step("   - Checkbox: randomly un/check");
            var dropdownLists = new string[] { "Lamp failures", "Comm. failures" };
            reportManagerPage.ReportEditorPanel.EnterNameInput(reportName);
            reportManagerPage.ReportEditorPanel.EnterEditablePropertiesValue(dropdownLists);
            reportManagerPage.ReportEditorPanel.SelectTab("Properties");
            reportManagerPage.ReportEditorPanel.SelectRandomLampFailureDropDownList();
            reportManagerPage.ReportEditorPanel.SelectRandomCommFailureDropDownList();
            var notedSelectedLampFailures = reportManagerPage.ReportEditorPanel.GetLampFailuresValues();
            var notedSelectedCommFailures = reportManagerPage.ReportEditorPanel.GetCommFailuresValues();
            var notedValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);           

            Step("7. Click Save icon");
            reportManagerPage.ReportEditorPanel.ClickSaveButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisappeared();

            Step("8. Expected The new report is saved as a new row in grid of reports");
            var reportsList = reportManagerPage.GridPanel.GetListOfColumnData("Name");
            VerifyEqual(string.Format("8. Verify new report {0} is saved as a new row in grid of reports", reportName), true, reportsList.Exists(p => p.Equals(reportName)));

            Step("9. Select the newly-created report");
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();

            Step("10. Expected Report Details panel appears with section; Verify input values are remained");
            var actualSelectedLampFailures = reportManagerPage.ReportEditorPanel.GetLampFailuresValues();
            var actualSelectedCommFailures = reportManagerPage.ReportEditorPanel.GetCommFailuresValues();
            var actualValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);
            VerifyEqual("10. Verify input values are remained", notedValues, actualValues);
            VerifyEqual("[SC-631] 10. Verify Selected Lamp Failures are remained", notedSelectedLampFailures, actualSelectedLampFailures);
            VerifyEqual("[SC-631] 10. Verify Selected Comm. Failures are remained", notedSelectedCommFailures, actualSelectedCommFailures);

            Step("11. Reload browser and browse to the created report again");
            desktopPage = Browser.RefreshLoggedInCMS();
            reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();

            Step("12. Expected Verify input values are remained");         
            actualSelectedLampFailures = reportManagerPage.ReportEditorPanel.GetLampFailuresValues();
            actualSelectedCommFailures = reportManagerPage.ReportEditorPanel.GetCommFailuresValues();
            actualValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);
            VerifyEqual("12. Verify input values are remained", notedValues, actualValues);
            VerifyEqual("[SC-631] 12. Verify Selected Lamp Failures are remained", notedSelectedLampFailures, actualSelectedLampFailures);
            VerifyEqual("[SC-631] 12. Verify Selected Comm. Failures are remained", notedSelectedCommFailures, actualSelectedCommFailures);

            Step("13. Change values of the report (if a checkbox was checked, unchecked it and otherwise. Select all if it is a multi-selectable dropdown list) and save");
            reportManagerPage.ReportEditorPanel.EnterEditablePropertiesValue(dropdownLists);
            reportManagerPage.ReportEditorPanel.SelectTab("Properties");
            reportManagerPage.ReportEditorPanel.SelectAllLampFailureDropDownList();
            reportManagerPage.ReportEditorPanel.SelectAllCommFailureDropDownList();
            notedSelectedLampFailures = reportManagerPage.ReportEditorPanel.GetLampFailuresValues();
            notedSelectedCommFailures = reportManagerPage.ReportEditorPanel.GetCommFailuresValues();
            notedValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);           

            reportManagerPage.ReportEditorPanel.ClickSaveButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisappeared();

            Step("14. Expected");
            Step("  o Name fields is editable (#1325077)");
            Step("  o Type field is readonly");
            Step("  o All fields are editable");
            Step("  o Saving is a success");
            VerifyEqual("[#1325077] 14. Verify Name field is editable", true, !reportManagerPage.ReportEditorPanel.IsNameInputReadOnly());
            VerifyEqual("14. Verify Type field is readonly", true, reportManagerPage.ReportEditorPanel.IsTypeDropDownReadOnly());
            VerifyEqual("14. Verify All fields are editable", true, reportManagerPage.ReportEditorPanel.AreFieldsEditable());

            Step("15. Select the newly-created report");
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();

            Step("16. Expected Report Details panel appears with section; Verify input values are remained");
            actualSelectedLampFailures = reportManagerPage.ReportEditorPanel.GetLampFailuresValues();
            actualSelectedCommFailures = reportManagerPage.ReportEditorPanel.GetCommFailuresValues();
            actualValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);
            VerifyEqual("16. Verify input values are remained", notedValues, actualValues);
            VerifyEqual("[SC-631] 16. Verify All selected Lamp Failures are remained", notedSelectedLampFailures, actualSelectedLampFailures);
            VerifyEqual("[SC-631] 16. Verify All selected Comm. Failures are remained", notedSelectedCommFailures, actualSelectedCommFailures);

            Step("17. Reload browser and browse to the created report again");
            desktopPage = Browser.RefreshLoggedInCMS();
            reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();

            Step("18. Expected Verify input values are remained");
            actualSelectedLampFailures = reportManagerPage.ReportEditorPanel.GetLampFailuresValues();
            actualSelectedCommFailures = reportManagerPage.ReportEditorPanel.GetCommFailuresValues();
            actualValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);
            VerifyEqual("18. Verify input values are remained", notedValues, actualValues);
            VerifyEqual("[SC-631] 18. Verify All selected Lamp Failures are remained", notedSelectedLampFailures, actualSelectedLampFailures);
            VerifyEqual("[SC-631] 18. Verify All selected Comm. Failures are remained", notedSelectedCommFailures, actualSelectedCommFailures);

            Step("19. Click Delete button in Report Details panel");
            reportManagerPage.ReportEditorPanel.ClickDeleteButton();
            reportManagerPage.WaitForPopupDialogDisplayed();

            Step("20. Expected A confirmation dialog appears with title \"Confirmation\" & message \"Would you like to delete '{{Report Name}}' report ? \"");
            var expectedMessage = string.Format("Would you like to delete '{0}' report ?", reportName);
            VerifyEqual("20. Verify dialog title is 'Confirmation'", "Confirmation", reportManagerPage.Dialog.GetDialogTitleText());
            VerifyEqual(string.Format("20. Verify dialog message is '{0}'", expectedMessage), expectedMessage, reportManagerPage.Dialog.GetMessageText());

            Step("21. Click No");
            reportManagerPage.Dialog.ClickNoButton();
            reportManagerPage.WaitForPopupDialogDisappeared();

            Step("22. Expected The dialog disappears");
            VerifyEqual("22. Verify The dialog disappears", false, reportManagerPage.IsPopupDialogDisplayed());

            Step("23. Click Delete button in Report Details panel again");
            reportManagerPage.ReportEditorPanel.ClickDeleteButton();
            reportManagerPage.WaitForPopupDialogDisplayed();

            Step("24. Expected A confirmation dialog appears with title \"Confirmation\" & message \"Would you like to delete '{{Report Name}}' report ? \"");
            VerifyEqual("24. Verify dialog title is 'Confirmation'", "Confirmation", reportManagerPage.Dialog.GetDialogTitleText());
            VerifyEqual(string.Format("24. Verify dialog message is '{0}'", expectedMessage), expectedMessage, reportManagerPage.Dialog.GetMessageText());

            Step("25. Click Yes");
            reportManagerPage.Dialog.ClickYesButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForPopupDialogDisappeared();

            Step("26. Expected The dialog disappears; the report is no longer present in the grid panel");
            VerifyEqual("26. Verify The dialog disappears", false, reportManagerPage.IsPopupDialogDisplayed());
            reportsList = reportManagerPage.GridPanel.GetListOfColumnData("Name");
            VerifyEqual("26. Verify The report is no longer present in the grid panel", false, reportsList.Exists(p => p.Equals(reportName)));

            Step("27. Reload browser and go to Report Manager");
            desktopPage = Browser.RefreshLoggedInCMS();
            reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();

            Step("28. Select the geozone where the deleted report was created");
            reportManagerPage.GeozoneTreeMainPanel.SelectNode(xmlGeoZone);

            Step("29. Expected The report is no longer present in the grid panel");
            reportsList = reportManagerPage.GridPanel.GetListOfColumnData("Name");
            VerifyEqual("29. Verify The report is no longer present in the grid panel", false, reportsList.Exists(p => p.Equals(reportName)));
        }

        [Test, DynamicRetry]
        [Description("RM_02 'Day Burner Report' Report")]
        public void RM_02()
        {
            var testData = GetTestDataOfTestRM_02();

            var xmlGeoZone = testData["GeoZone"].ToString();
            var geozoneName = xmlGeoZone.GetChildName();
            var parentGeozoneName = xmlGeoZone.GetParentName();
            var reportName = SLVHelper.GenerateUniqueName(string.Format("[{0}][{1}] {2}", Browser.Name, Settings.Users["DefaultTest"].Username, TestContext.CurrentContext.Test.MethodName));

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.ReportManager);

            Step("1. Go to Report Manager app");
            Step("2. Expected Report Manager is routed and loaded successfully");
            var reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();

            Step("3. Select a geozone");
            reportManagerPage.GeozoneTreeMainPanel.SelectNode(xmlGeoZone);

            Step("4. Add new report with 'Day Burner Report' type");
            reportManagerPage.GridPanel.ClickAddReportToolbarButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.ReportEditorPanel.WaitForPanelLoaded();
            reportManagerPage.ReportEditorPanel.SelectTypeDropDown("Day Burner Report");
            reportManagerPage.WaitForPreviousActionComplete();

            Step("5. Expected Report Details panel appears with section:");
            Step("  o Name");
            Step("  o Type");
            Step("  o Scheduler tab");
            Step("   * Configuration section");
            Step("    - Hour (HH): range [00-23]");
            Step("    - Minute: range [00-59]");
            Step("    - Periodicity : list");
            Step("     + Every day");
            Step("     + Every monday");
            Step("     + Every tuesday");
            Step("     + Every wednesday");
            Step("     + Every friday");
            Step("     + Every saturday");
            Step("     + Every sunday");
            Step("  o Export tab");
            Step("   * FTP section");
            Step("    - FTP Host");
            Step("    - SFTP mode");
            Step("    - FTP user");
            Step("    - FTP password");
            Step("    - FTP file name");
            Step("    - FTP passive mode");
            Step("    - FTP enabled");
            Step("    - FTP format: list");
            Step("     + CSV");
            Step("     + Html");
            Step("     + Plain text");
            Step("   * Mail section");
            Step("    - Mail enabled");
            Step("    - Mail format: list");
            Step("     + CSV");
            Step("     + Html");
            Step("     + Plain text");
            Step("    - Report in mail's body");
            Step("    - Subject");
            Step("    - From");
            Step("    - Contacts:list");
            Step("  o Report tab");
            Step("   * Configuration section");
            Step("    - Device Categories");
            Step("    - Recurse");
            VerifyDayBurnReportDetails(reportManagerPage);

            Step("6. Fill values into report fields:");
            Step("  o Name: a unique name");
            Step("  o Other fields (if not specifically indicated):");
            Step("   - Text: Any");
            Step("   - Numeric: Any");
            Step("   - Dropdown: any in list, some in list (multi-selectable)");
            Step("   - Checkbox: randomly un/check");
            var dropdownLists = new string[] { "Contacts", "Device Categories" };
            reportManagerPage.ReportEditorPanel.EnterNameInput(reportName);
            reportManagerPage.ReportEditorPanel.EnterEditablePropertiesValue(dropdownLists);
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            reportManagerPage.ReportEditorPanel.SelectRandomContactsDropDownList();
            var notedSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            reportManagerPage.ReportEditorPanel.SelectRandomDeviceCategoriesDropDownList();
            var notedSelectedDeviceCategories = reportManagerPage.ReportEditorPanel.GetDeviceCategoriesValues();
            var notedValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);           

            Step("7. Click Save icon");
            reportManagerPage.ReportEditorPanel.ClickSaveButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisappeared();

            Step("8. Expected The new report is saved as a new row in grid of reports");
            var reportsList = reportManagerPage.GridPanel.GetListOfColumnData("Name");
            VerifyEqual(string.Format("Verify new report {0} is saved as a new row in grid of reports", reportName), true, reportsList.Exists(p => p.Equals(reportName)));

            Step("9. Select the newly-created report");
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();

            Step("10. Expected Report Details panel appears with section; Verify input values are remained");
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            var actualSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            var actualSelectedDeviceCategories = reportManagerPage.ReportEditorPanel.GetDeviceCategoriesValues();
            var actualValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);
            VerifyEqual("10. Verify input values are remained", notedValues, actualValues);
            VerifyEqual("10. Verify Selected Contacts are remained", notedSelectedContacts, actualSelectedContacts);
            VerifyEqual("10. Verify Selected Device Categories are remained", notedSelectedDeviceCategories, actualSelectedDeviceCategories);

            Step("11. Reload browser and browse to the created report again");
            desktopPage = Browser.RefreshLoggedInCMS();
            reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();

            Step("12. Expected Verify input values are remained");
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            actualSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            actualSelectedDeviceCategories = reportManagerPage.ReportEditorPanel.GetDeviceCategoriesValues();
            actualValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);
            VerifyEqual("12. Verify input values are remained", notedValues, actualValues);
            VerifyEqual("12. Verify Selected Contacts are remained", notedSelectedContacts, actualSelectedContacts);
            VerifyEqual("12. Verify Selected Device Categories are remained", notedSelectedDeviceCategories, actualSelectedDeviceCategories);

            Step("13. Change values of the report (if a checkbox was checked, unchecked it and otherwise. Select all if it is a multi-selectable dropdown list) and save");
            reportManagerPage.ReportEditorPanel.EnterEditablePropertiesValue(dropdownLists);
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            reportManagerPage.ReportEditorPanel.SelectAllContactsDropDownList();
            notedSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            reportManagerPage.ReportEditorPanel.SelectAllDeviceCategoriesDropDownList();
            notedSelectedDeviceCategories = reportManagerPage.ReportEditorPanel.GetDeviceCategoriesValues();
            notedValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);            

            reportManagerPage.ReportEditorPanel.ClickSaveButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisappeared();

            Step("14. Expected");
            Step("  o Name fields is editable (#1325077)");
            Step("  o Type field is readonly");
            Step("  o All fields are editable");
            Step("  o Saving is a success");
            VerifyEqual("[#1325077] 14. Verify Name field is editable", true, !reportManagerPage.ReportEditorPanel.IsNameInputReadOnly());
            VerifyEqual("14. Verify Type field is readonly", true, reportManagerPage.ReportEditorPanel.IsTypeDropDownReadOnly());
            VerifyEqual("14. Verify All fields are editable", true, reportManagerPage.ReportEditorPanel.AreFieldsEditable());

            Step("15. Select the newly-created report");
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();

            Step("16. Expected Report Details panel appears with section; Verify input values are remained");
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            actualSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            actualSelectedDeviceCategories = reportManagerPage.ReportEditorPanel.GetDeviceCategoriesValues();
            actualValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);
            VerifyEqual("16. Verify input values are remained", notedValues, actualValues);
            VerifyEqual("16. Verify Selected Contacts are remained", notedSelectedContacts, actualSelectedContacts);
            VerifyEqual("16. Verify Selected Device Categories are remained", notedSelectedDeviceCategories, actualSelectedDeviceCategories);

            Step("17. Reload browser and browse to the created report again");
            desktopPage = Browser.RefreshLoggedInCMS();
            reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();

            Step("18. Expected Verify input values are remained");
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            actualSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            actualSelectedDeviceCategories = reportManagerPage.ReportEditorPanel.GetDeviceCategoriesValues();
            actualValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);
            VerifyEqual("18. Verify input values are remained", notedValues, actualValues);
            VerifyEqual("18. Verify Selected Contacts are remained", notedSelectedContacts, actualSelectedContacts);
            VerifyEqual("18. Verify Selected Device Categories are remained", notedSelectedDeviceCategories, actualSelectedDeviceCategories);

            Step("19. Click Delete button in Report Details panel");
            reportManagerPage.ReportEditorPanel.ClickDeleteButton();
            reportManagerPage.WaitForPopupDialogDisplayed();

            Step("20. Expected A confirmation dialog appears with title \"Confirmation\" & message \"Would you like to delete '{{Report Name}}' report ? \"");
            var expectedMessage = string.Format("Would you like to delete '{0}' report ?", reportName);
            VerifyEqual("20. Verify dialog title is 'Confirmation'", "Confirmation", reportManagerPage.Dialog.GetDialogTitleText());
            VerifyEqual(string.Format("Verify dialog message is '{0}'", expectedMessage), expectedMessage, reportManagerPage.Dialog.GetMessageText());

            Step("21. Click No");
            reportManagerPage.Dialog.ClickNoButton();
            reportManagerPage.WaitForPopupDialogDisappeared();

            Step("22. Expected The dialog disappears");
            VerifyEqual("22. Verify The dialog disappears", false, reportManagerPage.IsPopupDialogDisplayed());

            Step("23. Click Delete button in Report Details panel again");
            reportManagerPage.ReportEditorPanel.ClickDeleteButton();
            reportManagerPage.WaitForPopupDialogDisplayed();

            Step("24. Expected A confirmation dialog appears with title \"Confirmation\" & message \"Would you like to delete '{{Report Name}}' report ? \"");
            VerifyEqual("24. Verify dialog title is 'Confirmation'", "Confirmation", reportManagerPage.Dialog.GetDialogTitleText());
            VerifyEqual(string.Format("24. Verify dialog message is '{0}'", expectedMessage), expectedMessage, reportManagerPage.Dialog.GetMessageText());

            Step("25. Click Yes");
            reportManagerPage.Dialog.ClickYesButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForPopupDialogDisappeared();

            Step("26. Expected The dialog disappears; the report is no longer present in the grid panel");
            VerifyEqual("26. Verify The dialog disappears", false, reportManagerPage.IsPopupDialogDisplayed());
            reportsList = reportManagerPage.GridPanel.GetListOfColumnData("Name");
            VerifyEqual("26. Verify The report is no longer present in the grid panel", false, reportsList.Exists(p => p.Equals(reportName)));

            Step("27. Reload browser and go to Report Manager");
            desktopPage = Browser.RefreshLoggedInCMS();
            reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();

            Step("28. Select the geozone where the deleted report was created");
            reportManagerPage.GeozoneTreeMainPanel.SelectNode(xmlGeoZone);

            Step("29. Expected The report is no longer present in the grid panel");
            reportsList = reportManagerPage.GridPanel.GetListOfColumnData("Name");
            VerifyEqual("29. Verify The report is no longer present in the grid panel", false, reportsList.Exists(p => p.Equals(reportName)));
        }

        [Test, DynamicRetry]
        [Description("RM_03 'Failures HTML report' Report")]
        public void RM_03()
        {
            var testData = GetTestDataOfTestRM_03();

            var xmlGeoZone = testData["GeoZone"].ToString();
            var geozoneName = xmlGeoZone.GetChildName();
            var parentGeozoneName = xmlGeoZone.GetParentName();
            var reportName = SLVHelper.GenerateUniqueName(string.Format("[{0}][{1}] {2}", Browser.Name, Settings.Users["DefaultTest"].Username, TestContext.CurrentContext.Test.MethodName));

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.ReportManager);

            Step("1. Go to Report Manager app");
            Step("2. Expected Report Manager is routed and loaded successfully");
            var reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();

            Step("3. Select a geozone");
            reportManagerPage.GeozoneTreeMainPanel.SelectNode(xmlGeoZone);

            Step("4. Add new report with 'Failures HTML report' type");
            reportManagerPage.GridPanel.ClickAddReportToolbarButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.ReportEditorPanel.WaitForPanelLoaded();
            reportManagerPage.ReportEditorPanel.SelectTypeDropDown("Failures HTML report");
            reportManagerPage.WaitForPreviousActionComplete();

            Step("5. Expected Report Details panel appears with section:");
            Step("  o Name");
            Step("  o Type");
            Step("  o Properties tab");
            Step("   * Configuration section");
            Step("    - description");
            Step("    - Report details: list");
            Step("     + Auto");
            Step("     + Direct sub-geozones only");
            Step("     + All sub-geozones");
            Step("     + All devices");
            Step("    - Filtering mode: list");
            Step("     + No filter");
            Step("     + Critical failures and warnings only");
            Step("     + Critical failures only");
            Step("  o Scheduler tab");
            Step("   * Configuration section");
            Step("    - Hour (HH): range [00-23]");
            Step("    - Minute: range [00-59]");
            Step("    - Periodicity : list");
            Step("     + Every day");
            Step("     + Every monday");
            Step("     + Every tuesday");
            Step("     + Every wednesday");
            Step("     + Every friday");
            Step("     + Every saturday");
            Step("     + Every sunday");
            Step("  o Export tab");
            Step("   * Mail section");
            Step("    - Subject");
            Step("    - From");
            Step("    - Contacts:list");
            Step("   * Configuration section");
            Step("    - HTML format");
            VerifyFailuresHTMLReportDetails(reportManagerPage);

            Step("6. Fill values into report fields:");
            Step("  o Name: a unique name");
            Step("  o Other fields (if not specifically indicated):");
            Step("   - Text: Any");
            Step("   - Numeric: Any");
            Step("   - Dropdown: any in list, some in list (multi-selectable)");
            Step("   - Checkbox: randomly un/check");
            var dropdownLists = new string[] { "Contacts *" };
            reportManagerPage.ReportEditorPanel.EnterNameInput(reportName);
            reportManagerPage.ReportEditorPanel.EnterEditablePropertiesValue(dropdownLists);
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            reportManagerPage.ReportEditorPanel.SelectRandomContactsDropDownList();
            var notedSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            var notedValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);           

            Step("7. Click Save icon");
            reportManagerPage.ReportEditorPanel.ClickSaveButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisappeared();

            Step("8. Expected The new report is saved as a new row in grid of reports");
            var reportsList = reportManagerPage.GridPanel.GetListOfColumnData("Name");
            VerifyEqual(string.Format("8. Verify new report {0} is saved as a new row in grid of reports", reportName), true, reportsList.Exists(p => p.Equals(reportName)));

            Step("9. Select the newly-created report");
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();

            Step("10. Expected Report Details panel appears with section; Verify input values are remained");
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            var actualSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            var actualValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);
            VerifyEqual("10. Verify input values are remained", notedValues, actualValues);
            VerifyEqual("10. Verify Selected Contacts are remained", notedSelectedContacts, actualSelectedContacts);

            Step("11. Reload browser and browse to the created report again");
            desktopPage = Browser.RefreshLoggedInCMS();
            reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();

            Step("12. Expected Verify input values are remained");           
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            actualSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            actualValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);
            VerifyEqual("12. Verify input values are remained", notedValues, actualValues);
            VerifyEqual("12. Verify Selected Contacts are remained", notedSelectedContacts, actualSelectedContacts);

            Step("13. Change values of the report (if a checkbox was checked, unchecked it and otherwise. Select all if it is a multi-selectable dropdown list) and save");
            reportManagerPage.ReportEditorPanel.EnterEditablePropertiesValue(dropdownLists);
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            reportManagerPage.ReportEditorPanel.SelectAllContactsDropDownList();
            notedSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            notedValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);            

            reportManagerPage.ReportEditorPanel.ClickSaveButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisappeared();

            Step("14. Expected");
            Step("  o Name fields is editable (#1325077)");
            Step("  o Type field is readonly");
            Step("  o All fields are editable");
            Step("  o Saving is a success");
            VerifyEqual("[#1325077] 14. Verify Name field is editable", true, !reportManagerPage.ReportEditorPanel.IsNameInputReadOnly());
            VerifyEqual("14. Verify Type field is readonly", true, reportManagerPage.ReportEditorPanel.IsTypeDropDownReadOnly());
            VerifyEqual("14. Verify All fields are editable", true, reportManagerPage.ReportEditorPanel.AreFieldsEditable());

            Step("15. Select the newly-created report");
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();

            Step("16. Expected Report Details panel appears with section; Verify input values are remained");
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            actualSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            actualValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);
            VerifyEqual("16. Verify input values are remained", notedValues, actualValues);
            VerifyEqual("16. Verify Selected Contacts are remained", notedSelectedContacts, actualSelectedContacts);

            Step("17. Reload browser and browse to the created report again");
            desktopPage = Browser.RefreshLoggedInCMS();
            reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();

            Step("18. Expected Verify input values are remained");
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            actualSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            actualValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);
            VerifyEqual("18. Verify input values are remained", notedValues, actualValues);
            VerifyEqual("18. Verify Selected Contacts are remained", notedSelectedContacts, actualSelectedContacts);

            Step("19. Click Delete button in Report Details panel");
            reportManagerPage.ReportEditorPanel.ClickDeleteButton();
            reportManagerPage.WaitForPopupDialogDisplayed();

            Step("20. Expected A confirmation dialog appears with title \"Confirmation\" & message \"Would you like to delete '{{Report Name}}' report ? \"");
            var expectedMessage = string.Format("Would you like to delete '{0}' report ?", reportName);
            VerifyEqual("20. Verify dialog title is 'Confirmation'", "Confirmation", reportManagerPage.Dialog.GetDialogTitleText());
            VerifyEqual(string.Format("20. Verify dialog message is '{0}'", expectedMessage), expectedMessage, reportManagerPage.Dialog.GetMessageText());

            Step("21. Click No");
            reportManagerPage.Dialog.ClickNoButton();
            reportManagerPage.WaitForPopupDialogDisappeared();

            Step("22. Expected The dialog disappears");
            VerifyEqual("22. Verify The dialog disappears", false, reportManagerPage.IsPopupDialogDisplayed());

            Step("23. Click Delete button in Report Details panel again");
            reportManagerPage.ReportEditorPanel.ClickDeleteButton();
            reportManagerPage.WaitForPopupDialogDisplayed();

            Step("24. Expected A confirmation dialog appears with title \"Confirmation\" & message \"Would you like to delete '{{Report Name}}' report ? \"");
            VerifyEqual("24. Verify dialog title is 'Confirmation'", "Confirmation", reportManagerPage.Dialog.GetDialogTitleText());
            VerifyEqual(string.Format("24. Verify dialog message is '{0}'", expectedMessage), expectedMessage, reportManagerPage.Dialog.GetMessageText());

            Step("25. Click Yes");
            reportManagerPage.Dialog.ClickYesButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForPopupDialogDisappeared();

            Step("26. Expected The dialog disappears; the report is no longer present in the grid panel");
            VerifyEqual("26. Verify The dialog disappears", false, reportManagerPage.IsPopupDialogDisplayed());
            reportsList = reportManagerPage.GridPanel.GetListOfColumnData("Name");
            VerifyEqual("26. Verify The report is no longer present in the grid panel", false, reportsList.Exists(p => p.Equals(reportName)));

            Step("27. Reload browser and go to Report Manager");
            desktopPage = Browser.RefreshLoggedInCMS();
            reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();

            Step("28. Select the geozone where the deleted report was created");
            reportManagerPage.GeozoneTreeMainPanel.SelectNode(xmlGeoZone);

            Step("29. Expected The report is no longer present in the grid panel");
            reportsList = reportManagerPage.GridPanel.GetListOfColumnData("Name");
            VerifyEqual("29. Verify The report is no longer present in the grid panel", false, reportsList.Exists(p => p.Equals(reportName)));
        }

        [Test, DynamicRetry]
        [Description("RM_04 'Failures report' Report")]
        public void RM_04()
        {
            var testData = GetTestDataOfTestRM_04();

            var xmlGeoZone = testData["GeoZone"].ToString();
            var geozoneName = xmlGeoZone.GetChildName();
            var parentGeozoneName = xmlGeoZone.GetParentName();
            var reportName = SLVHelper.GenerateUniqueName(string.Format("[{0}][{1}] {2}", Browser.Name, Settings.Users["DefaultTest"].Username, TestContext.CurrentContext.Test.MethodName));

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.ReportManager);

            Step("1. Go to Report Manager app");
            Step("2. Expected Report Manager is routed and loaded successfully");
            var reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();

            Step("3. Select a geozone");
            reportManagerPage.GeozoneTreeMainPanel.SelectNode(xmlGeoZone);

            Step("4. Add new report with 'Failures report' type");
            reportManagerPage.GridPanel.ClickAddReportToolbarButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.ReportEditorPanel.WaitForPanelLoaded();
            reportManagerPage.ReportEditorPanel.SelectTypeDropDown("Failures report");
            reportManagerPage.WaitForPreviousActionComplete();

            Step("5. Expected Report Details panel appears with section:");
            Step("  o Name");
            Step("  o Type");
            Step("  o Properties tab");
            Step("   * Configuration section");
            Step("    - description");
            Step("  o Scheduler tab");
            Step("   * Configuration section");
            Step("    - Hour: range [00-23]");
            Step("    - Minute: range [00-59]");
            Step("  o Export tab");
            Step("   * FTP section");
            Step("    - FTP Host");
            Step("    - SFTP mode");
            Step("    - FTP user");
            Step("    - FTP password");
            Step("    - FTP full file path");
            Step("    - FTP passive mode");
            VerifyFailuresReportDetails(reportManagerPage);

            Step("6. Fill values into report fields:");
            Step("  o Name: a unique name");
            Step("  o Other fields (if not specifically indicated):");
            Step("   - Text: Any");
            Step("   - Numeric: Any");
            Step("   - Dropdown: any in list, some in list (multi-selectable)");
            Step("   - Checkbox: randomly un/check");
            reportManagerPage.ReportEditorPanel.EnterNameInput(reportName);
            reportManagerPage.ReportEditorPanel.EnterEditablePropertiesValue();
            var notedValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues();            

            Step("7. Click Save icon");
            reportManagerPage.ReportEditorPanel.ClickSaveButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisappeared();

            Step("8. Expected The new report is saved as a new row in grid of reports");
            var reportsList = reportManagerPage.GridPanel.GetListOfColumnData("Name");
            VerifyEqual(string.Format("8. Verify new report {0} is saved as a new row in grid of reports", reportName), true, reportsList.Exists(p => p.Equals(reportName)));

            Step("9. Select the newly-created report");
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();

            Step("10. Expected Report Details panel appears with section; Verify input values are remained");
            var actualValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues();
            VerifyEqual("10. Verify input values are remained", notedValues, actualValues);

            Step("11. Reload browser and browse to the created report again");
            desktopPage = Browser.RefreshLoggedInCMS();
            reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();

            Step("12. Expected Verify input values are remained");            
            actualValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues();
            VerifyEqual("12. Verify input values are remained", notedValues, actualValues);

            Step("13. Change values of the report (if a checkbox was checked, unchecked it and otherwise. Select all if it is a multi-selectable dropdown list) and save");
            reportManagerPage.ReportEditorPanel.EnterEditablePropertiesValue();
            notedValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues();
            reportManagerPage.ReportEditorPanel.ClickSaveButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisappeared();

            Step("14. Expected");
            Step("  o Name fields is editable (#1325077)");
            Step("  o Type field is readonly");
            Step("  o All fields are editable");
            Step("  o Saving is a success");
            VerifyEqual("[#1325077] 14. Verify Name field is editable", true, !reportManagerPage.ReportEditorPanel.IsNameInputReadOnly());
            VerifyEqual("14. Verify Type field is readonly", true, reportManagerPage.ReportEditorPanel.IsTypeDropDownReadOnly());
            VerifyEqual("14. Verify All fields are editable", true, reportManagerPage.ReportEditorPanel.AreFieldsEditable());

            Step("15. Select the newly-created report");
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();

            Step("16. Expected Report Details panel appears with section; Verify input values are remained");
            actualValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues();
            VerifyEqual("16. Verify input values are remained", notedValues, actualValues);

            Step("17. Reload browser and browse to the created report again");
            desktopPage = Browser.RefreshLoggedInCMS();
            reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();

            Step("18. Expected Verify input values are remained");
            actualValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues();
            VerifyEqual("18. Verify input values are remained", notedValues, actualValues);

            Step("19. Click Delete button in Report Details panel");
            reportManagerPage.ReportEditorPanel.ClickDeleteButton();
            reportManagerPage.WaitForPopupDialogDisplayed();

            Step("20. Expected A confirmation dialog appears with title \"Confirmation\" & message \"Would you like to delete '{{Report Name}}' report ? \"");
            var expectedMessage = string.Format("Would you like to delete '{0}' report ?", reportName);
            VerifyEqual("20. Verify dialog title is 'Confirmation'", "Confirmation", reportManagerPage.Dialog.GetDialogTitleText());
            VerifyEqual(string.Format("20. Verify dialog message is '{0}'", expectedMessage), expectedMessage, reportManagerPage.Dialog.GetMessageText());

            Step("21. Click No");
            reportManagerPage.Dialog.ClickNoButton();
            reportManagerPage.WaitForPopupDialogDisappeared();

            Step("22. Expected The dialog disappears");
            VerifyEqual("22. Verify The dialog disappears", false, reportManagerPage.IsPopupDialogDisplayed());

            Step("23. Click Delete button in Report Details panel again");
            reportManagerPage.ReportEditorPanel.ClickDeleteButton();
            reportManagerPage.WaitForPopupDialogDisplayed();

            Step("24. Expected A confirmation dialog appears with title \"Confirmation\" & message \"Would you like to delete '{{Report Name}}' report ? \"");
            VerifyEqual("24. Verify dialog title is 'Confirmation'", "Confirmation", reportManagerPage.Dialog.GetDialogTitleText());
            VerifyEqual(string.Format("24. Verify dialog message is '{0}'", expectedMessage), expectedMessage, reportManagerPage.Dialog.GetMessageText());

            Step("25. Click Yes");
            reportManagerPage.Dialog.ClickYesButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForPopupDialogDisappeared();

            Step("26. Expected The dialog disappears; the report is no longer present in the grid panel");
            VerifyEqual("26. Verify The dialog disappears", false, reportManagerPage.IsPopupDialogDisplayed());
            reportsList = reportManagerPage.GridPanel.GetListOfColumnData("Name");
            VerifyEqual("26. Verify The report is no longer present in the grid panel", false, reportsList.Exists(p => p.Equals(reportName)));

            Step("27. Reload browser and go to Report Manager");
            desktopPage = Browser.RefreshLoggedInCMS();
            reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();

            Step("28. Select the geozone where the deleted report was created");
            reportManagerPage.GeozoneTreeMainPanel.SelectNode(xmlGeoZone);

            Step("29. Expected The report is no longer present in the grid panel");
            reportsList = reportManagerPage.GridPanel.GetListOfColumnData("Name");
            VerifyEqual("29. Verify The report is no longer present in the grid panel", false, reportsList.Exists(p => p.Equals(reportName)));
        }

        [Test, DynamicRetry]
        [Description("RM_05 'Generic device last values' Report")]
        public void RM_05()
        {
            var testData = GetTestDataOfTestRM_05();

            var xmlGeoZone = testData["GeoZone"].ToString();
            var geozoneName = xmlGeoZone.GetChildName();
            var parentGeozoneName = xmlGeoZone.GetParentName();
            var reportName = SLVHelper.GenerateUniqueName(string.Format("[{0}][{1}]{2}", Browser.Name, Settings.Users["DefaultTest"].Username, TestContext.CurrentContext.Test.MethodName));

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.ReportManager);

            Step("1. Go to Report Manager app");
            Step("2. Expected Report Manager is routed and loaded successfully");
            var reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();

            Step("3. Select a geozone");
            reportManagerPage.GeozoneTreeMainPanel.SelectNode(xmlGeoZone);

            Step("4. Add new report with 'Generic device last values' type");
            reportManagerPage.GridPanel.ClickAddReportToolbarButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.ReportEditorPanel.WaitForPanelLoaded();
            reportManagerPage.ReportEditorPanel.SelectTypeDropDown("Generic device last values");
            reportManagerPage.WaitForPreviousActionComplete();

            Step("5. Expected Report Details panel appears with section:");
            Step("  o Name");
            Step("  o Type");
            Step("  o Scheduler tab");
            Step("   * Configuration section");
            Step("    - Hour (HH): range [00-23]");
            Step("    - Minute: range [00-59]");
            Step("    - Periodicity : list");
            Step("     + Every day");
            Step("     + Every monday");
            Step("     + Every tuesday");
            Step("     + Every wednesday");
            Step("     + Every friday");
            Step("     + Every saturday");
            Step("     + Every sunday");
            Step("  o Export tab");
            Step("   * FTP section");
            Step("    - FTP host");
            Step("    - SFTP mode");
            Step("    - FTP user");
            Step("    - FTP password");
            Step("    - FTP filename");
            Step("    - FTP passive mode");
            Step("    - FTP enabled");
            Step("    - FTP format: list");
            Step("     + CSV");
            Step("     + Html");
            Step("     + Plain text");
            Step("   * Mail section");
            Step("    - Mail enabled");
            Step("    - Mail format: list");
            Step("     + CSV");
            Step("     + Html");
            Step("     + Plain text");
            Step("    - In mail body");
            Step("    - Subject");
            Step("    - From");
            Step("    - Contacts:list");
            Step("  o Report tab");
            Step("   * Configuration section");
            Step("    - Categories:list");
            Step("    - Recurse");
            Step("    - Values:list");
            VerifyGenericDeviceLastValuesReportDetails(reportManagerPage);

            Step("6. Fill values into report fields:");
            Step("  o Name: a unique name");
            Step("  o Other fields (if not specifically indicated):");
            Step("   - Text: Any");
            Step("   - Numeric: Any");
            Step("   - Dropdown: any in list, some in list (multi-selectable)");
            Step("   - Checkbox: randomly un/check");
            var dropdownLists = new string[] { "Contacts", "Categories", "Values" };
            reportManagerPage.ReportEditorPanel.EnterNameInput(reportName);
            reportManagerPage.ReportEditorPanel.EnterEditablePropertiesValue(dropdownLists);
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            reportManagerPage.ReportEditorPanel.SelectRandomContactsDropDownList();
            var notedSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            reportManagerPage.ReportEditorPanel.SelectRandomDeviceCategoriesDropDownList();
            var notedSelectedDeviceCategories = reportManagerPage.ReportEditorPanel.GetDeviceCategoriesValues();
            var notedValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);

            Step("7. Click Save icon");
            reportManagerPage.ReportEditorPanel.ClickSaveButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisappeared();

            Step("8. Expected The new report is saved as a new row in grid of reports");
            var reportsList = reportManagerPage.GridPanel.GetListOfColumnData("Name");
            VerifyEqual(string.Format("8. Verify new report {0} is saved as a new row in grid of reports", reportName), true, reportsList.Exists(p => p.Equals(reportName)));

            Step("9. Select the newly-created report");
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();

            Step("10. Expected Report Details panel appears with section; Verify input values are remained");
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            var actualSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            var actualSelectedDeviceCategories = reportManagerPage.ReportEditorPanel.GetDeviceCategoriesValues();
            var actualValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);
            VerifyEqual("10. Verify input values are remained", notedValues, actualValues);
            VerifyEqual("10. Verify Selected Contacts are remained", notedSelectedContacts, actualSelectedContacts);
            VerifyEqual("10. Verify Selected Categories are remained", notedSelectedDeviceCategories, actualSelectedDeviceCategories);

            Step("11. Reload browser and browse to the created report again");
            desktopPage = Browser.RefreshLoggedInCMS();
            reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();

            Step("12. Expected Verify input values are remained");
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            actualSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            actualSelectedDeviceCategories = reportManagerPage.ReportEditorPanel.GetDeviceCategoriesValues();
            actualValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);
            VerifyEqual("12. Verify input values are remained", notedValues, actualValues);
            VerifyEqual("12. Verify Selected Contacts are remained", notedSelectedContacts, actualSelectedContacts);
            VerifyEqual("12. Verify Selected Categories are remained", notedSelectedDeviceCategories, actualSelectedDeviceCategories);

            Step("13. Change values of the report (if a checkbox was checked, unchecked it and otherwise. Select all if it is a multi-selectable dropdown list) and save");
            reportManagerPage.ReportEditorPanel.EnterEditablePropertiesValue(dropdownLists);
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            reportManagerPage.ReportEditorPanel.SelectAllContactsDropDownList();
            notedSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            reportManagerPage.ReportEditorPanel.SelectRandomValuesDropDownList();
            var notedSelectedValues = reportManagerPage.ReportEditorPanel.GetValuesValues();
            notedValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);

            reportManagerPage.ReportEditorPanel.ClickSaveButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisappeared();

            Step("14. Expected");
            Step("  o Name fields is editable (#1325077)");
            Step("  o Type field is readonly");
            Step("  o All fields are editable");
            Step("  o Saving is a success");
            VerifyEqual("[#1325077] 14. Verify Name field is editable", true, !reportManagerPage.ReportEditorPanel.IsNameInputReadOnly());
            VerifyEqual("14. Verify Type field is readonly", true, reportManagerPage.ReportEditorPanel.IsTypeDropDownReadOnly());
            VerifyEqual("14. Verify All fields are editable", true, reportManagerPage.ReportEditorPanel.AreFieldsEditable());

            Step("15. Select the newly-created report");
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();

            Step("16. Expected Report Details panel appears with section; Verify input values are remained");
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            actualSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            actualSelectedDeviceCategories = reportManagerPage.ReportEditorPanel.GetDeviceCategoriesValues();
            var actualSelectedValues = reportManagerPage.ReportEditorPanel.GetValuesValues();
            actualValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);
            VerifyEqual("16. Verify input values are remained", notedValues, actualValues);
            VerifyEqual("16. Verify Selected Contacts are remained", notedSelectedContacts, actualSelectedContacts);
            VerifyEqual("16. Verify Selected Categories are remained", notedSelectedDeviceCategories, actualSelectedDeviceCategories);
            VerifyEqual("16. Verify Selected Values are remained", notedSelectedValues, actualSelectedValues);

            Step("17. Reload browser and browse to the created report again");
            desktopPage = Browser.RefreshLoggedInCMS();
            reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();

            Step("18. Expected Verify input values are remained");
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            actualSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            actualSelectedDeviceCategories = reportManagerPage.ReportEditorPanel.GetDeviceCategoriesValues();
            actualSelectedValues = reportManagerPage.ReportEditorPanel.GetValuesValues();
            actualValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);
            VerifyEqual("18. Verify input values are remained", notedValues, actualValues);
            VerifyEqual("18. Verify Selected Contacts are remained", notedSelectedContacts, actualSelectedContacts);
            VerifyEqual("18. Verify Selected Device Categories are remained", notedSelectedDeviceCategories, actualSelectedDeviceCategories);
            VerifyEqual("18. Verify Selected Values are remained", notedSelectedValues, actualSelectedValues);

            //Select all
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            reportManagerPage.ReportEditorPanel.SelectAllDeviceCategoriesDropDownList();
            notedSelectedDeviceCategories = reportManagerPage.ReportEditorPanel.GetDeviceCategoriesValues();
            reportManagerPage.ReportEditorPanel.ClickSaveButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisappeared();
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            actualSelectedDeviceCategories = reportManagerPage.ReportEditorPanel.GetDeviceCategoriesValues();
            VerifyEqual("18. Verify Selected Categories are remained", notedSelectedDeviceCategories, actualSelectedDeviceCategories);

            desktopPage = Browser.RefreshLoggedInCMS();
            reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            actualSelectedDeviceCategories = reportManagerPage.ReportEditorPanel.GetDeviceCategoriesValues();
            VerifyEqual("18. Verify Selected Device Categories are remained", notedSelectedDeviceCategories, actualSelectedDeviceCategories);

            reportManagerPage.ReportEditorPanel.SelectAllValuesDropDownList();
            notedSelectedValues = reportManagerPage.ReportEditorPanel.GetValuesValues();
            reportManagerPage.ReportEditorPanel.ClickSaveButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisappeared();
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            actualSelectedDeviceCategories = reportManagerPage.ReportEditorPanel.GetDeviceCategoriesValues();
            actualSelectedValues = reportManagerPage.ReportEditorPanel.GetValuesValues();
            VerifyEqual("18. Verify Selected Categories are remained", notedSelectedDeviceCategories, actualSelectedDeviceCategories);
            VerifyEqual("18. Verify Selected Values are remained", notedSelectedValues, actualSelectedValues);

            desktopPage = Browser.RefreshLoggedInCMS();
            reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            actualSelectedDeviceCategories = reportManagerPage.ReportEditorPanel.GetDeviceCategoriesValues();
            actualSelectedValues = reportManagerPage.ReportEditorPanel.GetValuesValues();
            VerifyEqual("18. Verify Selected Categories are remained", notedSelectedDeviceCategories, actualSelectedDeviceCategories);
            VerifyEqual("18. Verify Selected Values are remained", notedSelectedValues, actualSelectedValues);

            Step("19. Click Delete button in Report Details panel");
            reportManagerPage.ReportEditorPanel.ClickDeleteButton();
            reportManagerPage.WaitForPopupDialogDisplayed();

            Step("20. Expected A confirmation dialog appears with title \"Confirmation\" & message \"Would you like to delete '{{Report Name}}' report ? \"");
            var expectedMessage = string.Format("Would you like to delete '{0}' report ?", reportName);
            VerifyEqual("20. Verify dialog title is 'Confirmation'", "Confirmation", reportManagerPage.Dialog.GetDialogTitleText());
            VerifyEqual(string.Format("20. Verify dialog message is '{0}'", expectedMessage), expectedMessage, reportManagerPage.Dialog.GetMessageText());

            Step("21. Click No");
            reportManagerPage.Dialog.ClickNoButton();
            reportManagerPage.WaitForPopupDialogDisappeared();

            Step("22. Expected The dialog disappears");
            VerifyEqual("22. Verify The dialog disappears", false, reportManagerPage.IsPopupDialogDisplayed());

            Step("23. Click Delete button in Report Details panel again");
            reportManagerPage.ReportEditorPanel.ClickDeleteButton();
            reportManagerPage.WaitForPopupDialogDisplayed();

            Step("24. Expected A confirmation dialog appears with title \"Confirmation\" & message \"Would you like to delete '{{Report Name}}' report ? \"");
            VerifyEqual("24. Verify dialog title is 'Confirmation'", "Confirmation", reportManagerPage.Dialog.GetDialogTitleText());
            VerifyEqual(string.Format("24. Verify dialog message is '{0}'", expectedMessage), expectedMessage, reportManagerPage.Dialog.GetMessageText());

            Step("25. Click Yes");
            reportManagerPage.Dialog.ClickYesButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForPopupDialogDisappeared();

            Step("26. Expected The dialog disappears; the report is no longer present in the grid panel");
            VerifyEqual("26. Verify The dialog disappears", false, reportManagerPage.IsPopupDialogDisplayed());
            reportsList = reportManagerPage.GridPanel.GetListOfColumnData("Name");
            VerifyEqual("26. Verify The report is no longer present in the grid panel", false, reportsList.Exists(p => p.Equals(reportName)));

            Step("27. Reload browser and go to Report Manager");
            desktopPage = Browser.RefreshLoggedInCMS();
            reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();

            Step("28. Select the geozone where the deleted report was created");
            reportManagerPage.GeozoneTreeMainPanel.SelectNode(xmlGeoZone);

            Step("29. Expected The report is no longer present in the grid panel");
            reportsList = reportManagerPage.GridPanel.GetListOfColumnData("Name");
            VerifyEqual("29. Verify The report is no longer present in the grid panel", false, reportsList.Exists(p => p.Equals(reportName)));
        }

        [Test, DynamicRetry]
        [Description("RM_06 'Generic device values' Report")]
        public void RM_06()
        {
            var testData = GetTestDataOfTestRM_06();

            var xmlGeoZone = testData["GeoZone"].ToString();
            var geozoneName = xmlGeoZone.GetChildName();
            var parentGeozoneName = xmlGeoZone.GetParentName();
            var reportName = SLVHelper.GenerateUniqueName(string.Format("[{0}][{1}] {2}", Browser.Name, Settings.Users["DefaultTest"].Username, TestContext.CurrentContext.Test.MethodName));

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.ReportManager);

            Step("1. Go to Report Manager app");
            Step("2. Expected Report Manager is routed and loaded successfully");
            var reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();

            Step("3. Select a geozone");
            reportManagerPage.GeozoneTreeMainPanel.SelectNode(xmlGeoZone);

            Step("4. Add new report with 'Generic device values' type");
            reportManagerPage.GridPanel.ClickAddReportToolbarButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.ReportEditorPanel.WaitForPanelLoaded();
            reportManagerPage.ReportEditorPanel.SelectTypeDropDown("Generic device values");
            reportManagerPage.WaitForPreviousActionComplete();

            Step("5. Expected Report Details panel appears with section:");
            Step("  o Name");
            Step("  o Type");
            Step("  o Scheduler tab");
            Step("   * Configuration section");
            Step("    - Hour (HH): range [00-23]");
            Step("    - Minute: range [00-59]");
            Step("    - Periodicity : list");
            Step("     + Every day");
            Step("     + Every monday");
            Step("     + Every tuesday");
            Step("     + Every wednesday");
            Step("     + Every friday");
            Step("     + Every saturday");
            Step("     + Every sunday");
            Step("  o Export tab");
            Step("   * FTP section");
            Step("    - FTP host");
            Step("    - SFTP mode");
            Step("    - FTP user");
            Step("    - FTP password");
            Step("    - FTP filename");
            Step("    - FTP passive mode");
            Step("    - FTP enabled");
            Step("    - FTP format: list");
            Step("     + CSV");
            Step("     + Html");
            Step("     + Plain text");
            Step("   * Mail section");
            Step("    - Mail enabled");
            Step("    - Mail format: list");
            Step("     + CSV");
            Step("     + Html");
            Step("     + Plain text");
            Step("    - In mail body");
            Step("    - Subject");
            Step("    - From");
            Step("    - Contacts:list");
            Step("  o Report tab");
            Step("   * Configuration section");
            Step("    - Categories:list");
            Step("    - Meanings:list");
            Step("    - Last value only");
            VerifyGenericDeviceValuesReportDetails(reportManagerPage);

            Step("6. Fill values into report fields:");
            Step("  o Name: a unique name");
            Step("  o Other fields (if not specifically indicated):");
            Step("   - Text: Any");
            Step("   - Numeric: Any");
            Step("   - Dropdown: any in list, some in list (multi-selectable)");
            Step("   - Checkbox: randomly un/check");
            var dropdownLists = new string[] { "Contacts", "Categories", "Meanings" };
            reportManagerPage.ReportEditorPanel.EnterNameInput(reportName);
            reportManagerPage.ReportEditorPanel.EnterEditablePropertiesValue(dropdownLists);
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            reportManagerPage.ReportEditorPanel.SelectRandomContactsDropDownList();
            var notedSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            reportManagerPage.ReportEditorPanel.SelectRandomDeviceCategoriesDropDownList();
            reportManagerPage.ReportEditorPanel.SelectRandomMeaningsDropDownList();
            var notedSelectedDeviceCategories = reportManagerPage.ReportEditorPanel.GetDeviceCategoriesValues();
            var notedSelectedMeanings = reportManagerPage.ReportEditorPanel.GetMeaningsValues();
            var notedValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);

            Step("7. Click Save icon");
            reportManagerPage.ReportEditorPanel.ClickSaveButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisappeared();

            Step("8. Expected The new report is saved as a new row in grid of reports");
            var reportsList = reportManagerPage.GridPanel.GetListOfColumnData("Name");
            VerifyEqual(string.Format("8. Verify new report {0} is saved as a new row in grid of reports", reportName), true, reportsList.Exists(p => p.Equals(reportName)));

            Step("9. Select the newly-created report");
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();

            Step("10. Expected Report Details panel appears with section; Verify input values are remained");
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            var actualSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            var actualSelectedDeviceCategories = reportManagerPage.ReportEditorPanel.GetDeviceCategoriesValues();
            var actualSelectedMeanings = reportManagerPage.ReportEditorPanel.GetMeaningsValues();
            var actualValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);
            VerifyEqual("10. Verify input values are remained", notedValues, actualValues);
            VerifyEqual("10. Verify Selected Contacts are remained", notedSelectedContacts, actualSelectedContacts);
            VerifyEqual("10. Verify Selected Categories are remained", notedSelectedDeviceCategories, actualSelectedDeviceCategories);
            VerifyEqual("[SC-631] 10. Verify Meanings are remained", notedSelectedMeanings, actualSelectedMeanings);

            Step("11. Reload browser and browse to the created report again");
            desktopPage = Browser.RefreshLoggedInCMS();
            reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();

            Step("12. Expected Verify input values are remained");
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            actualSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            actualSelectedDeviceCategories = reportManagerPage.ReportEditorPanel.GetDeviceCategoriesValues();
            actualSelectedMeanings = reportManagerPage.ReportEditorPanel.GetMeaningsValues();
            actualValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);
            VerifyEqual("12. Verify input values are remained", notedValues, actualValues);
            VerifyEqual("12. Verify Selected Contacts are remained", notedSelectedContacts, actualSelectedContacts);
            VerifyEqual("12. Verify Selected Categories are remained", notedSelectedDeviceCategories, actualSelectedDeviceCategories);
            VerifyEqual("[SC-631] 12. Verify Meanings are remained", notedSelectedMeanings, actualSelectedMeanings);

            Step("13. Change values of the report (if a checkbox was checked, unchecked it and otherwise. Select all if it is a multi-selectable dropdown list) and save");
            reportManagerPage.ReportEditorPanel.EnterEditablePropertiesValue(dropdownLists);
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            reportManagerPage.ReportEditorPanel.SelectAllContactsDropDownList();
            notedSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            reportManagerPage.ReportEditorPanel.SelectAllDeviceCategoriesDropDownList();
            reportManagerPage.ReportEditorPanel.SelectAllMeaningsDropDownList();
            notedSelectedDeviceCategories = reportManagerPage.ReportEditorPanel.GetDeviceCategoriesValues();
            notedSelectedMeanings = reportManagerPage.ReportEditorPanel.GetMeaningsValues();
            notedValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);

            reportManagerPage.ReportEditorPanel.ClickSaveButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisappeared();

            Step("14. Expected");
            Step("  o Name fields is editable (#1325077)");
            Step("  o Type field is readonly");
            Step("  o All fields are editable");
            Step("  o Saving is a success");
            VerifyEqual("[#1325077] 14. Verify Name field is editable", true, !reportManagerPage.ReportEditorPanel.IsNameInputReadOnly());
            VerifyEqual("14. Verify Type field is readonly", true, reportManagerPage.ReportEditorPanel.IsTypeDropDownReadOnly());
            VerifyEqual("14. Verify All fields are editable", true, reportManagerPage.ReportEditorPanel.AreFieldsEditable());

            Step("15. Select the newly-created report");
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();

            Step("16. Expected Report Details panel appears with section; Verify input values are remained");
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            actualSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            actualSelectedDeviceCategories = reportManagerPage.ReportEditorPanel.GetDeviceCategoriesValues();
            actualSelectedMeanings = reportManagerPage.ReportEditorPanel.GetMeaningsValues();
            actualValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);
            VerifyEqual("16. Verify input values are remained", notedValues, actualValues);
            VerifyEqual("16. Verify Selected Contacts are remained", notedSelectedContacts, actualSelectedContacts);
            VerifyEqual("16. Verify Selected Categories are remained", notedSelectedDeviceCategories, actualSelectedDeviceCategories);
            VerifyEqual("[SC-631] 16. Verify Meanings are remained", notedSelectedMeanings, actualSelectedMeanings);

            Step("17. Reload browser and browse to the created report again");
            desktopPage = Browser.RefreshLoggedInCMS();
            reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();

            Step("18. Expected Verify input values are remained");
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            actualSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            actualSelectedDeviceCategories = reportManagerPage.ReportEditorPanel.GetDeviceCategoriesValues();
            actualSelectedMeanings = reportManagerPage.ReportEditorPanel.GetMeaningsValues();
            actualValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);
            VerifyEqual("18. Verify input values are remained", notedValues, actualValues);
            VerifyEqual("18. Verify Selected Contacts are remained", notedSelectedContacts, actualSelectedContacts);
            VerifyEqual("18. Verify Selected Device Categories are remained", notedSelectedDeviceCategories, actualSelectedDeviceCategories);
            VerifyEqual("[SC-631] 18. Verify Meanings are remained", notedSelectedMeanings, actualSelectedMeanings);

            Step("19. Click Delete button in Report Details panel");
            reportManagerPage.ReportEditorPanel.ClickDeleteButton();
            reportManagerPage.WaitForPopupDialogDisplayed();

            Step("20. Expected A confirmation dialog appears with title \"Confirmation\" & message \"Would you like to delete '{{Report Name}}' report ? \"");
            var expectedMessage = string.Format("Would you like to delete '{0}' report ?", reportName);
            VerifyEqual("20. Verify dialog title is 'Confirmation'", "Confirmation", reportManagerPage.Dialog.GetDialogTitleText());
            VerifyEqual(string.Format("20. Verify dialog message is '{0}'", expectedMessage), expectedMessage, reportManagerPage.Dialog.GetMessageText());

            Step("21. Click No");
            reportManagerPage.Dialog.ClickNoButton();
            reportManagerPage.WaitForPopupDialogDisappeared();

            Step("22. Expected The dialog disappears");
            VerifyEqual("Verify The dialog disappears", false, reportManagerPage.IsPopupDialogDisplayed());

            Step("23. Click Delete button in Report Details panel again");
            reportManagerPage.ReportEditorPanel.ClickDeleteButton();
            reportManagerPage.WaitForPopupDialogDisplayed();

            Step("24. Expected A confirmation dialog appears with title \"Confirmation\" & message \"Would you like to delete '{{Report Name}}' report ? \"");
            VerifyEqual("24. Verify dialog title is 'Confirmation'", "Confirmation", reportManagerPage.Dialog.GetDialogTitleText());
            VerifyEqual(string.Format("24. Verify dialog message is '{0}'", expectedMessage), expectedMessage, reportManagerPage.Dialog.GetMessageText());

            Step("25. Click Yes");
            reportManagerPage.Dialog.ClickYesButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForPopupDialogDisappeared();

            Step("26. Expected The dialog disappears; the report is no longer present in the grid panel");
            VerifyEqual("26. Verify The dialog disappears", false, reportManagerPage.IsPopupDialogDisplayed());
            reportsList = reportManagerPage.GridPanel.GetListOfColumnData("Name");
            VerifyEqual("26. Verify The report is no longer present in the grid panel", false, reportsList.Exists(p => p.Equals(reportName)));

            Step("27. Reload browser and go to Report Manager");
            desktopPage = Browser.RefreshLoggedInCMS();
            reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();

            Step("28. Select the geozone where the deleted report was created");
            reportManagerPage.GeozoneTreeMainPanel.SelectNode(xmlGeoZone);

            Step("29. Expected The report is no longer present in the grid panel");
            reportsList = reportManagerPage.GridPanel.GetListOfColumnData("Name");
            VerifyEqual("29. Verify The report is no longer present in the grid panel", false, reportsList.Exists(p => p.Equals(reportName)));
        }

        [Test, DynamicRetry]
        [Description("RM_07 'Latency Report' Report")]
        public void RM_07()
        {
            var testData = GetTestDataOfTestRM_07();

            var xmlGeoZone = testData["GeoZone"].ToString();
            var geozoneName = xmlGeoZone.GetChildName();
            var parentGeozoneName = xmlGeoZone.GetParentName();
            var reportName = SLVHelper.GenerateUniqueName(string.Format("[{0}][{1}] {2}", Browser.Name, Settings.Users["DefaultTest"].Username, TestContext.CurrentContext.Test.MethodName));

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.ReportManager);

            Step("1. Go to Report Manager app");
            Step("2. Expected Report Manager is routed and loaded successfully");
            var reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();

            Step("3. Select a geozone");
            reportManagerPage.GeozoneTreeMainPanel.SelectNode(xmlGeoZone);

            Step("4. Add new report with 'Latency Report' type");
            reportManagerPage.GridPanel.ClickAddReportToolbarButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.ReportEditorPanel.WaitForPanelLoaded();
            reportManagerPage.ReportEditorPanel.SelectTypeDropDown("Latency Report");
            reportManagerPage.WaitForPreviousActionComplete();

            Step("5. Expected Report Details panel appears with section:");
            Step("  o Name");
            Step("  o Type");
            Step("  o Scheduler tab");
            Step("   * Configuration section");
            Step("    - Hour (HH): range [00-23]");
            Step("    - Minute: range [00-59]");
            Step("    - Periodicity : list");
            Step("     + Every day");
            Step("     + Every monday");
            Step("     + Every tuesday");
            Step("     + Every wednesday");
            Step("     + Every friday");
            Step("     + Every saturday");
            Step("     + Every sunday");
            Step("  o Export tab");
            Step("   * FTP section");
            Step("    - FTP Host");
            Step("    - SFTP mode");
            Step("    - FTP user");
            Step("    - FTP password");
            Step("    - FTP file name");
            Step("    - FTP passive mode");
            Step("    - FTP enabled");
            Step("    - FTP format: list");
            Step("     + CSV");
            Step("     + Html");
            Step("     + Plain text");
            Step("   * Mail section");
            Step("    - Mail enabled");
            Step("    - Mail format: list");
            Step("     + CSV");
            Step("     + Html");
            Step("     + Plain text");
            Step("    - Report in mail's body");
            Step("    - Subject");
            Step("    - From");
            Step("    - Contacts:list");
            Step("  o Report tab");
            Step("   * Configuration section");
            Step("    - Device Categories");
            Step("    - Recurse");
            Step("    - Command: list");
            Step("     + Lamp level command");
            Step("     + Lamp switch command");
            Step("    - Feedback: list");
            Step("     + Lamp level feedback");
            Step("     + Lamp switch feedback");
            Step("    - modeMeaningStrId: list");
            Step("     + Lamp command mode");
            Step("     + Light control mode");
            Step("    - From/To mode: list");
            Step("     + Fixed local time");
            Step("     + Last hours");
            Step("    - From last hours: range [1 hour - 12 hours]");
            Step("    - From (local time): range [00:00 - 23:00]");
            Step("    - To (local time): range [00:00 - 23:00]");
            Step("    - Command value");
            VerifyLatencyReportDetails(reportManagerPage);

            Step("6. Fill values into report fields:");
            Step("  o Name: a unique name");
            Step("  o Other fields (if not specifically indicated):");
            Step("   - Text: Any");
            Step("   - Numeric: Any");
            Step("   - Dropdown: any in list, some in list (multi-selectable)");
            Step("   - Checkbox: randomly un/check");
            var dropdownLists = new string[] { "Contacts", "Device Categories" };
            reportManagerPage.ReportEditorPanel.EnterNameInput(reportName);
            reportManagerPage.ReportEditorPanel.EnterEditablePropertiesValue(dropdownLists);
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            reportManagerPage.ReportEditorPanel.SelectRandomContactsDropDownList();
            var notedSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            reportManagerPage.ReportEditorPanel.SelectRandomDeviceCategoriesDropDownList();
            var notedSelectedDeviceCategories = reportManagerPage.ReportEditorPanel.GetDeviceCategoriesValues();
            var notedValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);

            Step("7. Click Save icon");
            reportManagerPage.ReportEditorPanel.ClickSaveButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisappeared();

            Step("8. Expected The new report is saved as a new row in grid of reports");
            var reportsList = reportManagerPage.GridPanel.GetListOfColumnData("Name");
            VerifyEqual(string.Format("8. Verify new report {0} is saved as a new row in grid of reports", reportName), true, reportsList.Exists(p => p.Equals(reportName)));

            Step("9. Select the newly-created report");
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();

            Step("10. Expected Report Details panel appears with section; Verify input values are remained");
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            var actualSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            var actualSelectedDeviceCategories = reportManagerPage.ReportEditorPanel.GetDeviceCategoriesValues();
            var actualValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);
            VerifyEqual("10. Verify input values are remained", notedValues, actualValues);
            VerifyEqual("10. Verify Selected Contacts are remained", notedSelectedContacts, actualSelectedContacts);
            VerifyEqual("10. Verify Selected Device Categories are remained", notedSelectedDeviceCategories, actualSelectedDeviceCategories);

            Step("11. Reload browser and browse to the created report again");
            desktopPage = Browser.RefreshLoggedInCMS();
            reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();

            Step("12. Expected Verify input values are remained");
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            actualSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            actualSelectedDeviceCategories = reportManagerPage.ReportEditorPanel.GetDeviceCategoriesValues();
            actualValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);
            VerifyEqual("12. Verify input values are remained", notedValues, actualValues);
            VerifyEqual("12. Verify Selected Contacts are remained", notedSelectedContacts, actualSelectedContacts);
            VerifyEqual("12. Verify Selected Device Categories are remained", notedSelectedDeviceCategories, actualSelectedDeviceCategories);

            Step("13. Change values of the report (if a checkbox was checked, unchecked it and otherwise. Select all if it is a multi-selectable dropdown list) and save");
            reportManagerPage.ReportEditorPanel.EnterEditablePropertiesValue(dropdownLists);
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            reportManagerPage.ReportEditorPanel.SelectAllContactsDropDownList();
            notedSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            reportManagerPage.ReportEditorPanel.SelectAllDeviceCategoriesDropDownList();
            notedSelectedDeviceCategories = reportManagerPage.ReportEditorPanel.GetDeviceCategoriesValues();
            notedValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);

            reportManagerPage.ReportEditorPanel.ClickSaveButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisappeared();

            Step("14. Expected");
            Step("  o Name fields is editable (#1325077)");
            Step("  o Type field is readonly");
            Step("  o All fields are editable");
            Step("  o Saving is a success");
            VerifyEqual("[#1325077] 14. Verify Name field is editable", true, !reportManagerPage.ReportEditorPanel.IsNameInputReadOnly());
            VerifyEqual("14. Verify Type field is readonly", true, reportManagerPage.ReportEditorPanel.IsTypeDropDownReadOnly());
            VerifyEqual("14. Verify All fields are editable", true, reportManagerPage.ReportEditorPanel.AreFieldsEditable());

            Step("15. Select the newly-created report");
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();

            Step("16. Expected Report Details panel appears with section; Verify input values are remained");
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            actualSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            actualSelectedDeviceCategories = reportManagerPage.ReportEditorPanel.GetDeviceCategoriesValues();
            actualValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);
            VerifyEqual("16. Verify input values are remained", notedValues, actualValues);
            VerifyEqual("16. Verify Selected Contacts are remained", notedSelectedContacts, actualSelectedContacts);
            VerifyEqual("16. Verify Selected Device Categories are remained", notedSelectedDeviceCategories, actualSelectedDeviceCategories);

            Step("17. Reload browser and browse to the created report again");
            desktopPage = Browser.RefreshLoggedInCMS();
            reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();

            Step("18. Expected Verify input values are remained");
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            actualSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            actualSelectedDeviceCategories = reportManagerPage.ReportEditorPanel.GetDeviceCategoriesValues();
            actualValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);
            VerifyEqual("18. Verify input values are remained", notedValues, actualValues);
            VerifyEqual("18. Verify Selected Contacts are remained", notedSelectedContacts, actualSelectedContacts);
            VerifyEqual("18. Verify Selected Device Categories are remained", notedSelectedDeviceCategories, actualSelectedDeviceCategories);

            Step("19. Click Delete button in Report Details panel");
            reportManagerPage.ReportEditorPanel.ClickDeleteButton();
            reportManagerPage.WaitForPopupDialogDisplayed();

            Step("20. Expected A confirmation dialog appears with title \"Confirmation\" & message \"Would you like to delete '{{Report Name}}' report ? \"");
            var expectedMessage = string.Format("Would you like to delete '{0}' report ?", reportName);
            VerifyEqual("20. Verify dialog title is 'Confirmation'", "Confirmation", reportManagerPage.Dialog.GetDialogTitleText());
            VerifyEqual(string.Format("20. Verify dialog message is '{0}'", expectedMessage), expectedMessage, reportManagerPage.Dialog.GetMessageText());

            Step("21. Click No");
            reportManagerPage.Dialog.ClickNoButton();
            reportManagerPage.WaitForPopupDialogDisappeared();

            Step("22. Expected The dialog disappears");
            VerifyEqual("22. Verify The dialog disappears", false, reportManagerPage.IsPopupDialogDisplayed());

            Step("23. Click Delete button in Report Details panel again");
            reportManagerPage.ReportEditorPanel.ClickDeleteButton();
            reportManagerPage.WaitForPopupDialogDisplayed();

            Step("24. Expected A confirmation dialog appears with title \"Confirmation\" & message \"Would you like to delete '{{Report Name}}' report ? \"");
            VerifyEqual("23. Verify dialog title is 'Confirmation'", "Confirmation", reportManagerPage.Dialog.GetDialogTitleText());
            VerifyEqual(string.Format("23. Verify dialog message is '{0}'", expectedMessage), expectedMessage, reportManagerPage.Dialog.GetMessageText());

            Step("25. Click Yes");
            reportManagerPage.Dialog.ClickYesButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForPopupDialogDisappeared();

            Step("26. Expected The dialog disappears; the report is no longer present in the grid panel");
            VerifyEqual("26. Verify The dialog disappears", false, reportManagerPage.IsPopupDialogDisplayed());
            reportsList = reportManagerPage.GridPanel.GetListOfColumnData("Name");
            VerifyEqual("26. Verify The report is no longer present in the grid panel", false, reportsList.Exists(p => p.Equals(reportName)));

            Step("27. Reload browser and go to Report Manager");
            desktopPage = Browser.RefreshLoggedInCMS();
            reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();

            Step("28. Select the geozone where the deleted report was created");
            reportManagerPage.GeozoneTreeMainPanel.SelectNode(xmlGeoZone);

            Step("29. Expected The report is no longer present in the grid panel");
            reportsList = reportManagerPage.GridPanel.GetListOfColumnData("Name");
            VerifyEqual("29. Verify The report is no longer present in the grid panel", false, reportsList.Exists(p => p.Equals(reportName)));
        }

        [Test, DynamicRetry]
        [Description("RM_08 'Lifetime report' Report")]
        public void RM_08()
        {
            var testData = GetTestDataOfTestRM_08();

            var xmlGeoZone = testData["GeoZone"].ToString();
            var geozoneName = xmlGeoZone.GetChildName();
            var parentGeozoneName = xmlGeoZone.GetParentName();
            var reportName = SLVHelper.GenerateUniqueName(string.Format("[{0}][{1}] {2}", Browser.Name, Settings.Users["DefaultTest"].Username, TestContext.CurrentContext.Test.MethodName));

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.ReportManager);

            Step("1. Go to Report Manager app");
            Step("2. Expected Report Manager is routed and loaded successfully");
            var reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();

            Step("3. Select a geozone");
            reportManagerPage.GeozoneTreeMainPanel.SelectNode(xmlGeoZone);

            Step("4. Add new report with 'Lifetime report' type");
            reportManagerPage.GridPanel.ClickAddReportToolbarButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.ReportEditorPanel.WaitForPanelLoaded();
            reportManagerPage.ReportEditorPanel.SelectTypeDropDown("Lifetime report");
            reportManagerPage.WaitForPreviousActionComplete();

            Step("5. Expected Report Details panel appears with section:");
            Step("  o Name");
            Step("  o Type");
            Step("  o Properties tab");
            Step("   * Configuration section");
            Step("    - description");
            Step("    - Critical lifetime");
            Step("  o Scheduler tab");
            Step("   * Configuration section");
            Step("    - Hour (HH): range [00-23]");
            Step("    - Minute: range [00-59]");
            Step("    - Periodicity : list");
            Step("     + Every day");
            Step("     + Every monday");
            Step("     + Every tuesday");
            Step("     + Every wednesday");
            Step("     + Every friday");
            Step("     + Every saturday");
            Step("     + Every sunday");
            Step("  o Export tab");
            Step("   * Mail section");
            Step("    - Subject");
            Step("    - From");
            Step("    - Contacts:list");
            Step("   * Configuration section");
            Step("    - HTML format");
            VerifyLifetimeReportDetails(reportManagerPage);

            Step("6. Fill values into report fields:");
            Step("  o Name: a unique name");
            Step("  o Other fields (if not specifically indicated):");
            Step("   - Text: Any");
            Step("   - Numeric: Any");
            Step("   - Dropdown: any in list, some in list (multi-selectable)");
            Step("   - Checkbox: randomly un/check");
            var dropdownLists = new string[] { "Contacts *" };
            reportManagerPage.ReportEditorPanel.EnterNameInput(reportName);
            reportManagerPage.ReportEditorPanel.EnterEditablePropertiesValue(dropdownLists);
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            reportManagerPage.ReportEditorPanel.SelectRandomContactsDropDownList();
            var notedSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            var notedValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);

            Step("7. Click Save icon");
            reportManagerPage.ReportEditorPanel.ClickSaveButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisappeared();

            Step("8. Expected The new report is saved as a new row in grid of reports");
            var reportsList = reportManagerPage.GridPanel.GetListOfColumnData("Name");
            VerifyEqual(string.Format("8. Verify new report {0} is saved as a new row in grid of reports", reportName), true, reportsList.Exists(p => p.Equals(reportName)));

            Step("9. Select the newly-created report");
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();

            Step("10. Expected Report Details panel appears with section; Verify input values are remained");
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            var actualSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            var actualValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);
            VerifyEqual("10. Verify input values are remained", notedValues, actualValues);
            VerifyEqual("10. Verify Selected Contacts are remained", notedSelectedContacts, actualSelectedContacts);

            Step("11. Reload browser and browse to the created report again");
            desktopPage = Browser.RefreshLoggedInCMS();
            reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();

            Step("12. Expected Verify input values are remained");
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            actualSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            actualValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);
            VerifyEqual("12. Verify input values are remained", notedValues, actualValues);
            VerifyEqual("12. Verify Selected Contacts are remained", notedSelectedContacts, actualSelectedContacts);

            Step("13. Change values of the report (if a checkbox was checked, unchecked it and otherwise. Select all if it is a multi-selectable dropdown list) and save");
            reportManagerPage.ReportEditorPanel.EnterEditablePropertiesValue(dropdownLists);
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            reportManagerPage.ReportEditorPanel.SelectAllContactsDropDownList();
            notedSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            notedValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);

            reportManagerPage.ReportEditorPanel.ClickSaveButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisappeared();

            Step("14. Expected");
            Step("  o Name fields is editable (#1325077)");
            Step("  o Type field is readonly");
            Step("  o All fields are editable");
            Step("  o Saving is a success");
            VerifyEqual("[#1325077] 14. Verify Name field is editable", true, !reportManagerPage.ReportEditorPanel.IsNameInputReadOnly());
            VerifyEqual("14. Verify Type field is readonly", true, reportManagerPage.ReportEditorPanel.IsTypeDropDownReadOnly());
            VerifyEqual("14. Verify All fields are editable", true, reportManagerPage.ReportEditorPanel.AreFieldsEditable());

            Step("15. Select the newly-created report");
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();

            Step("16. Expected Report Details panel appears with section; Verify input values are remained");
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            actualSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            actualValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);
            VerifyEqual("16. Verify input values are remained", notedValues, actualValues);
            VerifyEqual("16. Verify Selected Contacts are remained", notedSelectedContacts, actualSelectedContacts);

            Step("17. Reload browser and browse to the created report again");
            desktopPage = Browser.RefreshLoggedInCMS();
            reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();

            Step("18. Expected Verify input values are remained");
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            actualSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            actualValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);
            VerifyEqual("18. Verify input values are remained", notedValues, actualValues);
            VerifyEqual("18. Verify Selected Contacts are remained", notedSelectedContacts, actualSelectedContacts);

            Step("19. Click Delete button in Report Details panel");
            reportManagerPage.ReportEditorPanel.ClickDeleteButton();
            reportManagerPage.WaitForPopupDialogDisplayed();

            Step("20. Expected A confirmation dialog appears with title \"Confirmation\" & message \"Would you like to delete '{{Report Name}}' report ? \"");
            var expectedMessage = string.Format("Would you like to delete '{0}' report ?", reportName);
            VerifyEqual("20. Verify dialog title is 'Confirmation'", "Confirmation", reportManagerPage.Dialog.GetDialogTitleText());
            VerifyEqual(string.Format("20. Verify dialog message is '{0}'", expectedMessage), expectedMessage, reportManagerPage.Dialog.GetMessageText());

            Step("21. Click No");
            reportManagerPage.Dialog.ClickNoButton();
            reportManagerPage.WaitForPopupDialogDisappeared();

            Step("22. Expected The dialog disappears");
            VerifyEqual("22. Verify The dialog disappears", false, reportManagerPage.IsPopupDialogDisplayed());

            Step("23. Click Delete button in Report Details panel again");
            reportManagerPage.ReportEditorPanel.ClickDeleteButton();
            reportManagerPage.WaitForPopupDialogDisplayed();

            Step("24. Expected A confirmation dialog appears with title \"Confirmation\" & message \"Would you like to delete '{{Report Name}}' report ? \"");
            VerifyEqual("24. Verify dialog title is 'Confirmation'", "Confirmation", reportManagerPage.Dialog.GetDialogTitleText());
            VerifyEqual(string.Format("24. Verify dialog message is '{0}'", expectedMessage), expectedMessage, reportManagerPage.Dialog.GetMessageText());

            Step("25. Click Yes");
            reportManagerPage.Dialog.ClickYesButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForPopupDialogDisappeared();

            Step("26. Expected The dialog disappears; the report is no longer present in the grid panel");
            VerifyEqual("26. Verify The dialog disappears", false, reportManagerPage.IsPopupDialogDisplayed());
            reportsList = reportManagerPage.GridPanel.GetListOfColumnData("Name");
            VerifyEqual("26. Verify The report is no longer present in the grid panel", false, reportsList.Exists(p => p.Equals(reportName)));

            Step("27. Reload browser and go to Report Manager");
            desktopPage = Browser.RefreshLoggedInCMS();
            reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();

            Step("28. Select the geozone where the deleted report was created");
            reportManagerPage.GeozoneTreeMainPanel.SelectNode(xmlGeoZone);

            Step("29. Expected The report is no longer present in the grid panel");
            reportsList = reportManagerPage.GridPanel.GetListOfColumnData("Name");
            VerifyEqual("29. Verify The report is no longer present in the grid panel", false, reportsList.Exists(p => p.Equals(reportName)));
        }

        [Test, DynamicRetry]
        [Description("RM_09 'Low Power Factor Report' Report")]
        public void RM_09()
        {
            var testData = GetTestDataOfTestRM_09();

            var xmlGeoZone = testData["GeoZone"].ToString();
            var geozoneName = xmlGeoZone.GetChildName();
            var parentGeozoneName = xmlGeoZone.GetParentName();
            var reportName = SLVHelper.GenerateUniqueName(string.Format("[{0}][{1}] {2}", Browser.Name, Settings.Users["DefaultTest"].Username, TestContext.CurrentContext.Test.MethodName));

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.ReportManager);

            Step("1. Go to Report Manager app");
            Step("2. Expected Report Manager is routed and loaded successfully");
            var reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();

            Step("3. Select a geozone");
            reportManagerPage.GeozoneTreeMainPanel.SelectNode(xmlGeoZone);

            Step("4. Add new report with 'Low Power Factor Report' type");
            reportManagerPage.GridPanel.ClickAddReportToolbarButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.ReportEditorPanel.WaitForPanelLoaded();
            reportManagerPage.ReportEditorPanel.SelectTypeDropDown("Low Power Factor Report");
            reportManagerPage.WaitForPreviousActionComplete();

            Step("5. Expected Report Details panel appears with section:");
            Step("  o Name");
            Step("  o Type");
            Step("  o Scheduler tab");
            Step("   * Configuration section");
            Step("    - Hour (HH): range [00-23]");
            Step("    - Minute: range [00-59]");
            Step("    - Periodicity : list");
            Step("     + Every day");
            Step("     + Every monday");
            Step("     + Every tuesday");
            Step("     + Every wednesday");
            Step("     + Every friday");
            Step("     + Every saturday");
            Step("     + Every sunday");
            Step("  o Export tab");
            Step("   * FTP section");
            Step("    - FTP Host");
            Step("    - SFTP mode");
            Step("    - FTP user");
            Step("    - FTP password");
            Step("    - FTP file name");
            Step("    - FTP passive mode");
            Step("    - FTP enabled");
            Step("    - FTP format: list");
            Step("     + CSV");
            Step("     + Html");
            Step("     + Plain text");
            Step("   * Mail section");
            Step("    - Mail enabled");
            Step("    - Mail format: list");
            Step("     + CSV");
            Step("     + Html");
            Step("     + Plain text");
            Step("    - Report in mail's body");
            Step("    - Subject");
            Step("    - From");
            Step("    - Contacts:list");
            Step("  o Report tab");
            Step("   * Configuration section");
            Step("    - Device Categories");
            Step("    - Recurse");
            VerifyLowPowerFactorReportDetails(reportManagerPage);

            Step("6. Fill values into report fields:");
            Step("  o Name: a unique name");
            Step("  o Other fields (if not specifically indicated):");
            Step("   - Text: Any");
            Step("   - Numeric: Any");
            Step("   - Dropdown: any in list, some in list (multi-selectable)");
            Step("   - Checkbox: randomly un/check");
            var dropdownLists = new string[] { "Contacts", "Device Categories" };
            reportManagerPage.ReportEditorPanel.EnterNameInput(reportName);
            reportManagerPage.ReportEditorPanel.EnterEditablePropertiesValue(dropdownLists);
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            reportManagerPage.ReportEditorPanel.SelectRandomContactsDropDownList();
            var notedSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            reportManagerPage.ReportEditorPanel.SelectRandomDeviceCategoriesDropDownList();
            var notedSelectedDeviceCategories = reportManagerPage.ReportEditorPanel.GetDeviceCategoriesValues();
            var notedValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);

            Step("7. Click Save icon");
            reportManagerPage.ReportEditorPanel.ClickSaveButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisappeared();

            Step("8. Expected The new report is saved as a new row in grid of reports");
            var reportsList = reportManagerPage.GridPanel.GetListOfColumnData("Name");
            VerifyEqual(string.Format("8. Verify new report {0} is saved as a new row in grid of reports", reportName), true, reportsList.Exists(p => p.Equals(reportName)));

            Step("9. Select the newly-created report");
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();

            Step("10. Expected Report Details panel appears with section; Verify input values are remained");
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            var actualSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            var actualSelectedDeviceCategories = reportManagerPage.ReportEditorPanel.GetDeviceCategoriesValues();
            var actualValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);
            VerifyEqual("10. Verify input values are remained", notedValues, actualValues);
            VerifyEqual("10. Verify Selected Contacts are remained", notedSelectedContacts, actualSelectedContacts);
            VerifyEqual("10. Verify Selected Device Categories are remained", notedSelectedDeviceCategories, actualSelectedDeviceCategories);

            Step("11. Reload browser and browse to the created report again");
            desktopPage = Browser.RefreshLoggedInCMS();
            reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();

            Step("12. Expected Verify input values are remained");
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            actualSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            actualSelectedDeviceCategories = reportManagerPage.ReportEditorPanel.GetDeviceCategoriesValues();
            actualValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);
            VerifyEqual("12. Verify input values are remained", notedValues, actualValues);
            VerifyEqual("12. Verify Selected Contacts are remained", notedSelectedContacts, actualSelectedContacts);
            VerifyEqual("12. Verify Selected Device Categories are remained", notedSelectedDeviceCategories, actualSelectedDeviceCategories);

            Step("13. Change values of the report (if a checkbox was checked, unchecked it and otherwise. Select all if it is a multi-selectable dropdown list) and save");
            reportManagerPage.ReportEditorPanel.EnterEditablePropertiesValue(dropdownLists);
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            reportManagerPage.ReportEditorPanel.SelectAllContactsDropDownList();
            notedSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            reportManagerPage.ReportEditorPanel.SelectAllDeviceCategoriesDropDownList();
            notedSelectedDeviceCategories = reportManagerPage.ReportEditorPanel.GetDeviceCategoriesValues();
            notedValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);

            reportManagerPage.ReportEditorPanel.ClickSaveButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisappeared();

            Step("14. Expected");
            Step("  o Name fields is editable (#1325077)");
            Step("  o Type field is readonly");
            Step("  o All fields are editable");
            Step("  o Saving is a success");
            VerifyEqual("[#1325077] 14. Verify Name field is editable", true, !reportManagerPage.ReportEditorPanel.IsNameInputReadOnly());
            VerifyEqual("14. Verify Type field is readonly", true, reportManagerPage.ReportEditorPanel.IsTypeDropDownReadOnly());
            VerifyEqual("14. Verify All fields are editable", true, reportManagerPage.ReportEditorPanel.AreFieldsEditable());

            Step("15. Select the newly-created report");
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();

            Step("16. Expected Report Details panel appears with section; Verify input values are remained");
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            actualSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            actualSelectedDeviceCategories = reportManagerPage.ReportEditorPanel.GetDeviceCategoriesValues();
            actualValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);
            VerifyEqual("16. Verify input values are remained", notedValues, actualValues);
            VerifyEqual("16. Verify Selected Contacts are remained", notedSelectedContacts, actualSelectedContacts);
            VerifyEqual("16. Verify Selected Device Categories are remained", notedSelectedDeviceCategories, actualSelectedDeviceCategories);

            Step("17. Reload browser and browse to the created report again");
            desktopPage = Browser.RefreshLoggedInCMS();
            reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();

            Step("18. Expected Verify input values are remained");
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            actualSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            actualSelectedDeviceCategories = reportManagerPage.ReportEditorPanel.GetDeviceCategoriesValues();
            actualValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);
            VerifyEqual("18. Verify input values are remained", notedValues, actualValues);
            VerifyEqual("18. Verify Selected Contacts are remained", notedSelectedContacts, actualSelectedContacts);
            VerifyEqual("18. Verify Selected Device Categories are remained", notedSelectedDeviceCategories, actualSelectedDeviceCategories);

            Step("19. Click Delete button in Report Details panel");
            reportManagerPage.ReportEditorPanel.ClickDeleteButton();
            reportManagerPage.WaitForPopupDialogDisplayed();

            Step("20. Expected A confirmation dialog appears with title \"Confirmation\" & message \"Would you like to delete '{{Report Name}}' report ? \"");
            var expectedMessage = string.Format("Would you like to delete '{0}' report ?", reportName);
            VerifyEqual("20. Verify dialog title is 'Confirmation'", "Confirmation", reportManagerPage.Dialog.GetDialogTitleText());
            VerifyEqual(string.Format("20. Verify dialog message is '{0}'", expectedMessage), expectedMessage, reportManagerPage.Dialog.GetMessageText());

            Step("21. Click No");
            reportManagerPage.Dialog.ClickNoButton();
            reportManagerPage.WaitForPopupDialogDisappeared();

            Step("22. Expected The dialog disappears");
            VerifyEqual("22. Verify The dialog disappears", false, reportManagerPage.IsPopupDialogDisplayed());

            Step("23. Click Delete button in Report Details panel again");
            reportManagerPage.ReportEditorPanel.ClickDeleteButton();
            reportManagerPage.WaitForPopupDialogDisplayed();

            Step("24. Expected A confirmation dialog appears with title \"Confirmation\" & message \"Would you like to delete '{{Report Name}}' report ? \"");
            VerifyEqual("24. Verify dialog title is 'Confirmation'", "Confirmation", reportManagerPage.Dialog.GetDialogTitleText());
            VerifyEqual(string.Format("24. Verify dialog message is '{0}'", expectedMessage), expectedMessage, reportManagerPage.Dialog.GetMessageText());

            Step("25. Click Yes");
            reportManagerPage.Dialog.ClickYesButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForPopupDialogDisappeared();

            Step("26. Expected The dialog disappears; the report is no longer present in the grid panel");
            VerifyEqual("26. Verify The dialog disappears", false, reportManagerPage.IsPopupDialogDisplayed());
            reportsList = reportManagerPage.GridPanel.GetListOfColumnData("Name");
            VerifyEqual("26. Verify The report is no longer present in the grid panel", false, reportsList.Exists(p => p.Equals(reportName)));

            Step("27. Reload browser and go to Report Manager");
            desktopPage = Browser.RefreshLoggedInCMS();
            reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();

            Step("28. Select the geozone where the deleted report was created");
            reportManagerPage.GeozoneTreeMainPanel.SelectNode(xmlGeoZone);

            Step("29. Expected The report is no longer present in the grid panel");
            reportsList = reportManagerPage.GridPanel.GetListOfColumnData("Name");
            VerifyEqual("29. Verify The report is no longer present in the grid panel", false, reportsList.Exists(p => p.Equals(reportName)));
        }

        [Test, DynamicRetry]
        [Description("RM_10 'No data ever received' Report")]
        public void RM_10()
        {
            var testData = GetTestDataOfTestRM_10();

            var xmlGeoZone = testData["GeoZone"].ToString();
            var geozoneName = xmlGeoZone.GetChildName();
            var parentGeozoneName = xmlGeoZone.GetParentName();
            var reportName = SLVHelper.GenerateUniqueName(string.Format("[{0}][{1}] {2}", Browser.Name, Settings.Users["DefaultTest"].Username, TestContext.CurrentContext.Test.MethodName));

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.ReportManager);

            Step("1. Go to Report Manager app");
            Step("2. Expected Report Manager is routed and loaded successfully");
            var reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();

            Step("3. Select a geozone");
            reportManagerPage.GeozoneTreeMainPanel.SelectNode(xmlGeoZone);

            Step("4. Add new report with 'No data ever received' type");
            reportManagerPage.GridPanel.ClickAddReportToolbarButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.ReportEditorPanel.WaitForPanelLoaded();
            reportManagerPage.ReportEditorPanel.SelectTypeDropDown("No data ever received");
            reportManagerPage.WaitForPreviousActionComplete();

            Step("5. Expected Report Details panel appears with section:");
            Step("  o Name");
            Step("  o Type");
            Step("  o Scheduler tab");
            Step("   * Configuration section");
            Step("    - Hour (HH): range [00-23]");
            Step("    - Minute: range [00-59]");
            Step("    - Periodicity : list");
            Step("     + Every day");
            Step("     + Every monday");
            Step("     + Every tuesday");
            Step("     + Every wednesday");
            Step("     + Every friday");
            Step("     + Every saturday");
            Step("     + Every sunday");
            Step("  o Export tab");
            Step("   * FTP section");
            Step("    - FTP Host");
            Step("    - SFTP mode");
            Step("    - FTP user");
            Step("    - FTP password");
            Step("    - FTP file name");
            Step("    - FTP passive mode");
            Step("    - FTP enabled");
            Step("    - FTP format: list");
            Step("     + CSV");
            Step("     + Html");
            Step("     + Plain text");
            Step("   * Mail section");
            Step("    - Mail enabled");
            Step("    - Mail format: list");
            Step("     + CSV");
            Step("     + Html");
            Step("     + Plain text");
            Step("    - Report in mail's body");
            Step("    - Subject");
            Step("    - From");
            Step("    - Contacts:list");
            Step("  o Report tab");
            Step("   * Configuration section");
            Step("    - Device Categories");
            Step("    - Recurse");
            VerifyNoDataEverReceivedDetails(reportManagerPage);

            Step("6. Fill values into report fields:");
            Step("  o Name: a unique name");
            Step("  o Other fields (if not specifically indicated):");
            Step("   - Text: Any");
            Step("   - Numeric: Any");
            Step("   - Dropdown: any in list, some in list (multi-selectable)");
            Step("   - Checkbox: randomly un/check");
            var dropdownLists = new string[] { "Contacts", "Device Categories" };
            reportManagerPage.ReportEditorPanel.EnterNameInput(reportName);
            reportManagerPage.ReportEditorPanel.EnterEditablePropertiesValue(dropdownLists);
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            reportManagerPage.ReportEditorPanel.SelectRandomContactsDropDownList();
            var notedSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            reportManagerPage.ReportEditorPanel.SelectRandomDeviceCategoriesDropDownList();
            var notedSelectedDeviceCategories = reportManagerPage.ReportEditorPanel.GetDeviceCategoriesValues();
            var notedValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);

            Step("7. Click Save icon");
            reportManagerPage.ReportEditorPanel.ClickSaveButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisappeared();

            Step("8. Expected The new report is saved as a new row in grid of reports");
            var reportsList = reportManagerPage.GridPanel.GetListOfColumnData("Name");
            VerifyEqual(string.Format("8. Verify new report {0} is saved as a new row in grid of reports", reportName), true, reportsList.Exists(p => p.Equals(reportName)));

            Step("9. Select the newly-created report");
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();

            Step("10. Expected Report Details panel appears with section; Verify input values are remained");
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            var actualSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            var actualSelectedDeviceCategories = reportManagerPage.ReportEditorPanel.GetDeviceCategoriesValues();
            var actualValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);
            VerifyEqual("10. Verify input values are remained", notedValues, actualValues);
            VerifyEqual("10. Verify Selected Contacts are remained", notedSelectedContacts, actualSelectedContacts);
            VerifyEqual("10. Verify Selected Device Categories are remained", notedSelectedDeviceCategories, actualSelectedDeviceCategories);

            Step("11. Reload browser and browse to the created report again");
            desktopPage = Browser.RefreshLoggedInCMS();
            reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();

            Step("12. Expected Verify input values are remained");
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            actualSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            actualSelectedDeviceCategories = reportManagerPage.ReportEditorPanel.GetDeviceCategoriesValues();
            actualValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);
            VerifyEqual("12. Verify input values are remained", notedValues, actualValues);
            VerifyEqual("12. Verify Selected Contacts are remained", notedSelectedContacts, actualSelectedContacts);
            VerifyEqual("12. Verify Selected Device Categories are remained", notedSelectedDeviceCategories, actualSelectedDeviceCategories);

            Step("13. Change values of the report (if a checkbox was checked, unchecked it and otherwise. Select all if it is a multi-selectable dropdown list) and save");
            reportManagerPage.ReportEditorPanel.EnterEditablePropertiesValue(dropdownLists);
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            reportManagerPage.ReportEditorPanel.SelectAllContactsDropDownList();
            notedSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            reportManagerPage.ReportEditorPanel.SelectAllDeviceCategoriesDropDownList();
            notedSelectedDeviceCategories = reportManagerPage.ReportEditorPanel.GetDeviceCategoriesValues();
            notedValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);

            reportManagerPage.ReportEditorPanel.ClickSaveButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisappeared();

            Step("14. Expected");
            Step("  o Name fields is editable (#1325077)");
            Step("  o Type field is readonly");
            Step("  o All fields are editable");
            Step("  o Saving is a success");
            VerifyEqual("[#1325077] 14. Verify Name field is editable", true, !reportManagerPage.ReportEditorPanel.IsNameInputReadOnly());
            VerifyEqual("14. Verify Type field is readonly", true, reportManagerPage.ReportEditorPanel.IsTypeDropDownReadOnly());
            VerifyEqual("14. Verify All fields are editable", true, reportManagerPage.ReportEditorPanel.AreFieldsEditable());

            Step("15. Select the newly-created report");
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();

            Step("16. Expected Report Details panel appears with section; Verify input values are remained");
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            actualSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            actualSelectedDeviceCategories = reportManagerPage.ReportEditorPanel.GetDeviceCategoriesValues();
            actualValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);
            VerifyEqual("16. Verify input values are remained", notedValues, actualValues);
            VerifyEqual("16. Verify Selected Contacts are remained", notedSelectedContacts, actualSelectedContacts);
            VerifyEqual("16. Verify Selected Device Categories are remained", notedSelectedDeviceCategories, actualSelectedDeviceCategories);

            Step("17. Reload browser and browse to the created report again");
            desktopPage = Browser.RefreshLoggedInCMS();
            reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();

            Step("18. Expected Verify input values are remained");
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            actualSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            actualSelectedDeviceCategories = reportManagerPage.ReportEditorPanel.GetDeviceCategoriesValues();
            actualValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);
            VerifyEqual("18. Verify input values are remained", notedValues, actualValues);
            VerifyEqual("18. Verify Selected Contacts are remained", notedSelectedContacts, actualSelectedContacts);
            VerifyEqual("18. Verify Selected Device Categories are remained", notedSelectedDeviceCategories, actualSelectedDeviceCategories);

            Step("19. Click Delete button in Report Details panel");
            reportManagerPage.ReportEditorPanel.ClickDeleteButton();
            reportManagerPage.WaitForPopupDialogDisplayed();

            Step("20. Expected A confirmation dialog appears with title \"Confirmation\" & message \"Would you like to delete '{{Report Name}}' report ? \"");
            var expectedMessage = string.Format("Would you like to delete '{0}' report ?", reportName);
            VerifyEqual("20. Verify dialog title is 'Confirmation'", "Confirmation", reportManagerPage.Dialog.GetDialogTitleText());
            VerifyEqual(string.Format("20. Verify dialog message is '{0}'", expectedMessage), expectedMessage, reportManagerPage.Dialog.GetMessageText());

            Step("21. Click No");
            reportManagerPage.Dialog.ClickNoButton();
            reportManagerPage.WaitForPopupDialogDisappeared();

            Step("22. Expected The dialog disappears");
            VerifyEqual("22. Verify The dialog disappears", false, reportManagerPage.IsPopupDialogDisplayed());

            Step("23. Click Delete button in Report Details panel again");
            reportManagerPage.ReportEditorPanel.ClickDeleteButton();
            reportManagerPage.WaitForPopupDialogDisplayed();

            Step("24. Expected A confirmation dialog appears with title \"Confirmation\" & message \"Would you like to delete '{{Report Name}}' report ? \"");
            VerifyEqual("24. Verify dialog title is 'Confirmation'", "Confirmation", reportManagerPage.Dialog.GetDialogTitleText());
            VerifyEqual(string.Format("24. Verify dialog message is '{0}'", expectedMessage), expectedMessage, reportManagerPage.Dialog.GetMessageText());

            Step("25. Click Yes");
            reportManagerPage.Dialog.ClickYesButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForPopupDialogDisappeared();

            Step("26. Expected The dialog disappears; the report is no longer present in the grid panel");
            VerifyEqual("26. Verify The dialog disappears", false, reportManagerPage.IsPopupDialogDisplayed());
            reportsList = reportManagerPage.GridPanel.GetListOfColumnData("Name");
            VerifyEqual("26. Verify The report is no longer present in the grid panel", false, reportsList.Exists(p => p.Equals(reportName)));

            Step("27. Reload browser and go to Report Manager");
            desktopPage = Browser.RefreshLoggedInCMS();
            reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();

            Step("28. Select the geozone where the deleted report was created");
            reportManagerPage.GeozoneTreeMainPanel.SelectNode(xmlGeoZone);

            Step("29. Expected The report is no longer present in the grid panel");
            reportsList = reportManagerPage.GridPanel.GetListOfColumnData("Name");
            VerifyEqual("29. Verify The report is no longer present in the grid panel", false, reportsList.Exists(p => p.Equals(reportName)));
        }

        [Test, DynamicRetry]
        [Description("RM_11 'OnOff segment report' Report")]
        public void RM_11()
        {
            var testData = GetTestDataOfTestRM_11();

            var xmlGeoZone = testData["GeoZone"].ToString();
            var geozoneName = xmlGeoZone.GetChildName();
            var parentGeozoneName = xmlGeoZone.GetParentName();
            var reportName = SLVHelper.GenerateUniqueName(string.Format("[{0}][{1}] {2}", Browser.Name, Settings.Users["DefaultTest"].Username, TestContext.CurrentContext.Test.MethodName));

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.ReportManager);

            Step("1. Go to Report Manager app");
            Step("2. Expected Report Manager is routed and loaded successfully");
            var reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();

            Step("3. Select a geozone");
            reportManagerPage.GeozoneTreeMainPanel.SelectNode(xmlGeoZone);

            Step("4. Add new report with 'OnOff segment report' type");
            reportManagerPage.GridPanel.ClickAddReportToolbarButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.ReportEditorPanel.WaitForPanelLoaded();
            reportManagerPage.ReportEditorPanel.SelectTypeDropDown("OnOff segment report");
            reportManagerPage.WaitForPreviousActionComplete();

            Step("5. Expected Report Details panel appears with section:");
            Step("  o Name");
            Step("  o Type");
            Step("  o Properties tab");
            Step("   * Configuration section");
            Step("    - description");
            Step("    - Number of days");
            Step("  o Scheduler tab");
            Step("   * Configuration section");
            Step("    - Hour (HH): range [00-23]");
            Step("    - Minute: range [00-59]");
            Step("    - Periodicity : list");
            Step("     + Every day");
            Step("     + Every monday");
            Step("     + Every tuesday");
            Step("     + Every wednesday");
            Step("     + Every friday");
            Step("     + Every saturday");
            Step("     + Every sunday");
            Step("  o Export tab");
            Step("   * Mail section");
            Step("    - Subject");
            Step("    - From");
            Step("    - Contacts:list");
            Step("   * Configuration section");
            Step("    - HTML format");
            VerifyOnOffSegmentReportDetails(reportManagerPage);

            Step("6. Fill values into report fields:");
            Step("  o Name: a unique name");
            Step("  o Other fields (if not specifically indicated):");
            Step("   - Text: Any");
            Step("   - Numeric: Any");
            Step("   - Dropdown: any in list, some in list (multi-selectable)");
            Step("   - Checkbox: randomly un/check");
            var dropdownLists = new string[] { "Contacts *" };
            reportManagerPage.ReportEditorPanel.EnterNameInput(reportName);
            reportManagerPage.ReportEditorPanel.EnterEditablePropertiesValue(dropdownLists);
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            reportManagerPage.ReportEditorPanel.SelectRandomContactsDropDownList();
            var notedSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            var notedValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);

            Step("7. Click Save icon");
            reportManagerPage.ReportEditorPanel.ClickSaveButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisappeared();

            Step("8. Expected The new report is saved as a new row in grid of reports");
            var reportsList = reportManagerPage.GridPanel.GetListOfColumnData("Name");
            VerifyEqual(string.Format("8. Verify new report {0} is saved as a new row in grid of reports", reportName), true, reportsList.Exists(p => p.Equals(reportName)));

            Step("9. Select the newly-created report");
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();

            Step("10. Expected Report Details panel appears with section; Verify input values are remained");
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            var actualSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            var actualValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);
            VerifyEqual("10. Verify input values are remained", notedValues, actualValues);
            VerifyEqual("10. Verify Selected Contacts are remained", notedSelectedContacts, actualSelectedContacts);

            Step("11. Reload browser and browse to the created report again");
            desktopPage = Browser.RefreshLoggedInCMS();
            reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();

            Step("12. Expected Verify input values are remained");
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            actualSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            actualValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);
            VerifyEqual("[#1429525] 12. Verify input values are remained", notedValues, actualValues);
            VerifyEqual("12. Verify Selected Contacts are remained", notedSelectedContacts, actualSelectedContacts);

            Step("13. Change values of the report (if a checkbox was checked, unchecked it and otherwise. Select all if it is a multi-selectable dropdown list) and save");
            reportManagerPage.ReportEditorPanel.EnterEditablePropertiesValue(dropdownLists);
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            reportManagerPage.ReportEditorPanel.SelectAllContactsDropDownList();
            notedSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            notedValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);

            reportManagerPage.ReportEditorPanel.ClickSaveButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisappeared();

            Step("14. Expected");
            Step("  o Name fields is editable (#1325077)");
            Step("  o Type field is readonly");
            Step("  o All fields are editable");
            Step("  o Saving is a success");
            VerifyEqual("[#1325077] 14. Verify Name field is editable", true, !reportManagerPage.ReportEditorPanel.IsNameInputReadOnly());
            VerifyEqual("14. Verify Type field is readonly", true, reportManagerPage.ReportEditorPanel.IsTypeDropDownReadOnly());
            VerifyEqual("14. Verify All fields are editable", true, reportManagerPage.ReportEditorPanel.AreFieldsEditable());

            Step("15. Select the newly-created report");
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();

            Step("16. Expected Report Details panel appears with section; Verify input values are remained");
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            actualSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            actualValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);
            VerifyEqual("16. Verify input values are remained", notedValues, actualValues);
            VerifyEqual("16. Verify Selected Contacts are remained", notedSelectedContacts, actualSelectedContacts);

            Step("17. Reload browser and browse to the created report again");
            desktopPage = Browser.RefreshLoggedInCMS();
            reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();

            Step("18. Expected Verify input values are remained");
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            actualSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            actualValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);
            VerifyEqual("[#1429525] 18. Verify input values are remained", notedValues, actualValues);
            VerifyEqual("18. Verify Selected Contacts are remained", notedSelectedContacts, actualSelectedContacts);

            Step("19. Click Delete button in Report Details panel");
            reportManagerPage.ReportEditorPanel.ClickDeleteButton();
            reportManagerPage.WaitForPopupDialogDisplayed();

            Step("20. Expected A confirmation dialog appears with title \"Confirmation\" & message \"Would you like to delete '{{Report Name}}' report ? \"");
            var expectedMessage = string.Format("Would you like to delete '{0}' report ?", reportName);
            VerifyEqual("20. Verify dialog title is 'Confirmation'", "Confirmation", reportManagerPage.Dialog.GetDialogTitleText());
            VerifyEqual(string.Format("20. Verify dialog message is '{0}'", expectedMessage), expectedMessage, reportManagerPage.Dialog.GetMessageText());

            Step("21. Click No");
            reportManagerPage.Dialog.ClickNoButton();
            reportManagerPage.WaitForPopupDialogDisappeared();

            Step("22. Expected The dialog disappears");
            VerifyEqual("22. Verify The dialog disappears", false, reportManagerPage.IsPopupDialogDisplayed());

            Step("23. Click Delete button in Report Details panel again");
            reportManagerPage.ReportEditorPanel.ClickDeleteButton();
            reportManagerPage.WaitForPopupDialogDisplayed();

            Step("24. Expected A confirmation dialog appears with title \"Confirmation\" & message \"Would you like to delete '{{Report Name}}' report ? \"");
            VerifyEqual("24. Verify dialog title is 'Confirmation'", "Confirmation", reportManagerPage.Dialog.GetDialogTitleText());
            VerifyEqual(string.Format("24. Verify dialog message is '{0}'", expectedMessage), expectedMessage, reportManagerPage.Dialog.GetMessageText());

            Step("25. Click Yes");
            reportManagerPage.Dialog.ClickYesButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForPopupDialogDisappeared();

            Step("26. Expected The dialog disappears; the report is no longer present in the grid panel");
            VerifyEqual("Verify The dialog disappears", false, reportManagerPage.IsPopupDialogDisplayed());
            reportsList = reportManagerPage.GridPanel.GetListOfColumnData("Name");
            VerifyEqual("26. Verify The report is no longer present in the grid panel", false, reportsList.Exists(p => p.Equals(reportName)));

            Step("27. Reload browser and go to Report Manager");
            desktopPage = Browser.RefreshLoggedInCMS();
            reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();

            Step("28. Select the geozone where the deleted report was created");
            reportManagerPage.GeozoneTreeMainPanel.SelectNode(xmlGeoZone);

            Step("29. Expected The report is no longer present in the grid panel");
            reportsList = reportManagerPage.GridPanel.GetListOfColumnData("Name");
            VerifyEqual("29. Verify The report is no longer present in the grid panel", false, reportsList.Exists(p => p.Equals(reportName)));
        }

        [Test, DynamicRetry]
        [Description("RM_12 'Over 140V Voltage Report' Report")]
        public void RM_12()
        {
            var testData = GetTestDataOfTestRM_12();

            var xmlGeoZone = testData["GeoZone"].ToString();
            var geozoneName = xmlGeoZone.GetChildName();
            var parentGeozoneName = xmlGeoZone.GetParentName();
            var reportName = SLVHelper.GenerateUniqueName(string.Format("[{0}][{1}] {2}", Browser.Name, Settings.Users["DefaultTest"].Username, TestContext.CurrentContext.Test.MethodName));

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.ReportManager);

            Step("1. Go to Report Manager app");
            Step("2. Expected Report Manager is routed and loaded successfully");
            var reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();

            Step("3. Select a geozone");
            reportManagerPage.GeozoneTreeMainPanel.SelectNode(xmlGeoZone);

            Step("4. Add new report with 'Over 140V Voltage Report' type");
            reportManagerPage.GridPanel.ClickAddReportToolbarButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.ReportEditorPanel.WaitForPanelLoaded();
            reportManagerPage.ReportEditorPanel.SelectTypeDropDown("Over 140V Voltage Report");
            reportManagerPage.WaitForPreviousActionComplete();

            Step("5. Expected Report Details panel appears with section:");
            Step("  o Name");
            Step("  o Type");
            Step("  o Scheduler tab");
            Step("   * Configuration section");
            Step("    - Hour (HH): range [00-23]");
            Step("    - Minute: range [00-59]");
            Step("    - Periodicity : list");
            Step("     + Every day");
            Step("     + Every monday");
            Step("     + Every tuesday");
            Step("     + Every wednesday");
            Step("     + Every friday");
            Step("     + Every saturday");
            Step("     + Every sunday");
            Step("  o Export tab");
            Step("   * FTP section");
            Step("    - FTP Host");
            Step("    - SFTP mode");
            Step("    - FTP user");
            Step("    - FTP password");
            Step("    - FTP file name");
            Step("    - FTP passive mode");
            Step("    - FTP enabled");
            Step("    - FTP format: list");
            Step("     + CSV");
            Step("     + Html");
            Step("     + Plain text");
            Step("   * Mail section");
            Step("    - Mail enabled");
            Step("    - Mail format: list");
            Step("     + CSV");
            Step("     + Html");
            Step("     + Plain text");
            Step("    - Report in mail's body");
            Step("    - Subject");
            Step("    - From");
            Step("    - Contacts:list");
            Step("  o Report tab");
            Step("   * Configuration section");
            Step("    - Device Categories");
            Step("    - Recurse");
            VerifyOver140VVoltageReportDetails(reportManagerPage);

            Step("6. Fill values into report fields:");
            Step("  o Name: a unique name");
            Step("  o Other fields (if not specifically indicated):");
            Step("   - Text: Any");
            Step("   - Numeric: Any");
            Step("   - Dropdown: any in list, some in list (multi-selectable)");
            Step("   - Checkbox: randomly un/check");
            var dropdownLists = new string[] { "Contacts", "Device Categories" };
            reportManagerPage.ReportEditorPanel.EnterNameInput(reportName);
            reportManagerPage.ReportEditorPanel.EnterEditablePropertiesValue(dropdownLists);
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            reportManagerPage.ReportEditorPanel.SelectRandomContactsDropDownList();
            var notedSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            reportManagerPage.ReportEditorPanel.SelectRandomDeviceCategoriesDropDownList();
            var notedSelectedDeviceCategories = reportManagerPage.ReportEditorPanel.GetDeviceCategoriesValues();
            var notedValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);

            Step("7. Click Save icon");
            reportManagerPage.ReportEditorPanel.ClickSaveButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisappeared();

            Step("8. Expected The new report is saved as a new row in grid of reports");
            var reportsList = reportManagerPage.GridPanel.GetListOfColumnData("Name");
            VerifyEqual(string.Format("8. Verify new report {0} is saved as a new row in grid of reports", reportName), true, reportsList.Exists(p => p.Equals(reportName)));

            Step("9. Select the newly-created report");
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();

            Step("10. Expected Report Details panel appears with section; Verify input values are remained");
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            var actualSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            var actualSelectedDeviceCategories = reportManagerPage.ReportEditorPanel.GetDeviceCategoriesValues();
            var actualValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);
            VerifyEqual("10. Verify input values are remained", notedValues, actualValues);
            VerifyEqual("10. Verify Selected Contacts are remained", notedSelectedContacts, actualSelectedContacts);
            VerifyEqual("10. Verify Selected Device Categories are remained", notedSelectedDeviceCategories, actualSelectedDeviceCategories);

            Step("11. Reload browser and browse to the created report again");
            desktopPage = Browser.RefreshLoggedInCMS();
            reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();

            Step("12. Expected Verify input values are remained");
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            actualSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            actualSelectedDeviceCategories = reportManagerPage.ReportEditorPanel.GetDeviceCategoriesValues();
            actualValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);
            VerifyEqual("12. Verify input values are remained", notedValues, actualValues);
            VerifyEqual("12. Verify Selected Contacts are remained", notedSelectedContacts, actualSelectedContacts);
            VerifyEqual("12. Verify Selected Device Categories are remained", notedSelectedDeviceCategories, actualSelectedDeviceCategories);

            Step("13. Change values of the report (if a checkbox was checked, unchecked it and otherwise. Select all if it is a multi-selectable dropdown list) and save");
            reportManagerPage.ReportEditorPanel.EnterEditablePropertiesValue(dropdownLists);
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            reportManagerPage.ReportEditorPanel.SelectAllContactsDropDownList();
            notedSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            reportManagerPage.ReportEditorPanel.SelectAllDeviceCategoriesDropDownList();
            notedSelectedDeviceCategories = reportManagerPage.ReportEditorPanel.GetDeviceCategoriesValues();
            notedValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);

            reportManagerPage.ReportEditorPanel.ClickSaveButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisappeared();

            Step("14. Expected");
            Step("  o Name fields is editable (#1325077)");
            Step("  o Type field is readonly");
            Step("  o All fields are editable");
            Step("  o Saving is a success");
            VerifyEqual("[#1325077] 14. Verify Name field is editable", true, !reportManagerPage.ReportEditorPanel.IsNameInputReadOnly());
            VerifyEqual("14. Verify Type field is readonly", true, reportManagerPage.ReportEditorPanel.IsTypeDropDownReadOnly());
            VerifyEqual("14. Verify All fields are editable", true, reportManagerPage.ReportEditorPanel.AreFieldsEditable());

            Step("15. Select the newly-created report");
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();

            Step("16. Expected Report Details panel appears with section; Verify input values are remained");
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            actualSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            actualSelectedDeviceCategories = reportManagerPage.ReportEditorPanel.GetDeviceCategoriesValues();
            actualValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);
            VerifyEqual("16. Verify input values are remained", notedValues, actualValues);
            VerifyEqual("16. Verify Selected Contacts are remained", notedSelectedContacts, actualSelectedContacts);
            VerifyEqual("16. Verify Selected Device Categories are remained", notedSelectedDeviceCategories, actualSelectedDeviceCategories);

            Step("17. Reload browser and browse to the created report again");
            desktopPage = Browser.RefreshLoggedInCMS();
            reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();

            Step("18. Expected Verify input values are remained");
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            actualSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            actualSelectedDeviceCategories = reportManagerPage.ReportEditorPanel.GetDeviceCategoriesValues();
            actualValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);
            VerifyEqual("18. Verify input values are remained", notedValues, actualValues);
            VerifyEqual("18. Verify Selected Contacts are remained", notedSelectedContacts, actualSelectedContacts);
            VerifyEqual("18. Verify Selected Device Categories are remained", notedSelectedDeviceCategories, actualSelectedDeviceCategories);

            Step("19. Click Delete button in Report Details panel");
            reportManagerPage.ReportEditorPanel.ClickDeleteButton();
            reportManagerPage.WaitForPopupDialogDisplayed();

            Step("20. Expected A confirmation dialog appears with title \"Confirmation\" & message \"Would you like to delete '{{Report Name}}' report ? \"");
            var expectedMessage = string.Format("Would you like to delete '{0}' report ?", reportName);
            VerifyEqual("20. Verify dialog title is 'Confirmation'", "Confirmation", reportManagerPage.Dialog.GetDialogTitleText());
            VerifyEqual(string.Format("20. Verify dialog message is '{0}'", expectedMessage), expectedMessage, reportManagerPage.Dialog.GetMessageText());

            Step("21. Click No");
            reportManagerPage.Dialog.ClickNoButton();
            reportManagerPage.WaitForPopupDialogDisappeared();

            Step("22. Expected The dialog disappears");
            VerifyEqual("22. Verify The dialog disappears", false, reportManagerPage.IsPopupDialogDisplayed());

            Step("23. Click Delete button in Report Details panel again");
            reportManagerPage.ReportEditorPanel.ClickDeleteButton();
            reportManagerPage.WaitForPopupDialogDisplayed();

            Step("24. Expected A confirmation dialog appears with title \"Confirmation\" & message \"Would you like to delete '{{Report Name}}' report ? \"");
            VerifyEqual("24. Verify dialog title is 'Confirmation'", "Confirmation", reportManagerPage.Dialog.GetDialogTitleText());
            VerifyEqual(string.Format("24. Verify dialog message is '{0}'", expectedMessage), expectedMessage, reportManagerPage.Dialog.GetMessageText());

            Step("25. Click Yes");
            reportManagerPage.Dialog.ClickYesButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForPopupDialogDisappeared();

            Step("26. Expected The dialog disappears; the report is no longer present in the grid panel");
            VerifyEqual("26. Verify The dialog disappears", false, reportManagerPage.IsPopupDialogDisplayed());
            reportsList = reportManagerPage.GridPanel.GetListOfColumnData("Name");
            VerifyEqual("26. Verify The report is no longer present in the grid panel", false, reportsList.Exists(p => p.Equals(reportName)));

            Step("27. Reload browser and go to Report Manager");
            desktopPage = Browser.RefreshLoggedInCMS();
            reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();

            Step("28. Select the geozone where the deleted report was created");
            reportManagerPage.GeozoneTreeMainPanel.SelectNode(xmlGeoZone);

            Step("29. Expected The report is no longer present in the grid panel");
            reportsList = reportManagerPage.GridPanel.GetListOfColumnData("Name");
            VerifyEqual("29. Verify The report is no longer present in the grid panel", false, reportsList.Exists(p => p.Equals(reportName)));
        }

        [Test, DynamicRetry]
        [Description("RM_13 'Over Voltage Report' Report")]
        public void RM_13()
        {
            var testData = GetTestDataOfTestRM_13();

            var xmlGeoZone = testData["GeoZone"].ToString();
            var geozoneName = xmlGeoZone.GetChildName();
            var parentGeozoneName = xmlGeoZone.GetParentName();
            var reportName = SLVHelper.GenerateUniqueName(string.Format("[{0}][{1}] {2}", Browser.Name, Settings.Users["DefaultTest"].Username, TestContext.CurrentContext.Test.MethodName));

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.ReportManager);

            Step("1. Go to Report Manager app");
            Step("2. Expected Report Manager is routed and loaded successfully");
            var reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();

            Step("3. Select a geozone");
            reportManagerPage.GeozoneTreeMainPanel.SelectNode(xmlGeoZone);

            Step("4. Add new report with 'Over Voltage Report' type");
            reportManagerPage.GridPanel.ClickAddReportToolbarButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.ReportEditorPanel.WaitForPanelLoaded();
            reportManagerPage.ReportEditorPanel.SelectTypeDropDown("Over Voltage Report");
            reportManagerPage.WaitForPreviousActionComplete();

            Step("5. Expected Report Details panel appears with section:");
            Step("  o Name");
            Step("  o Type");
            Step("  o Scheduler tab");
            Step("   * Configuration section");
            Step("    - Hour (HH): range [00-23]");
            Step("    - Minute: range [00-59]");
            Step("    - Periodicity : list");
            Step("     + Every day");
            Step("     + Every monday");
            Step("     + Every tuesday");
            Step("     + Every wednesday");
            Step("     + Every friday");
            Step("     + Every saturday");
            Step("     + Every sunday");
            Step("  o Export tab");
            Step("   * FTP section");
            Step("    - FTP Host");
            Step("    - SFTP mode");
            Step("    - FTP user");
            Step("    - FTP password");
            Step("    - FTP file name");
            Step("    - FTP passive mode");
            Step("    - FTP enabled");
            Step("    - FTP format: list");
            Step("     + CSV");
            Step("     + Html");
            Step("     + Plain text");
            Step("   * Mail section");
            Step("    - Mail enabled");
            Step("    - Mail format: list");
            Step("     + CSV");
            Step("     + Html");
            Step("     + Plain text");
            Step("    - Report in mail's body");
            Step("    - Subject");
            Step("    - From");
            Step("    - Contacts:list");
            Step("  o Report tab");
            Step("   * Configuration section");
            Step("    - Device Categories:list");
            Step("    - Recurse");
            VerifyOverVoltageReportDetails(reportManagerPage);

            Step("6. Fill values into report fields:");
            Step("  o Name: a unique name");
            Step("  o Other fields (if not specifically indicated):");
            Step("   - Text: Any");
            Step("   - Numeric: Any");
            Step("   - Dropdown: any in list, some in list (multi-selectable)");
            Step("   - Checkbox: randomly un/check");
            var dropdownLists = new string[] { "Contacts", "Device Categories" };
            reportManagerPage.ReportEditorPanel.EnterNameInput(reportName);
            reportManagerPage.ReportEditorPanel.EnterEditablePropertiesValue(dropdownLists);
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            reportManagerPage.ReportEditorPanel.SelectRandomContactsDropDownList();
            var notedSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            reportManagerPage.ReportEditorPanel.SelectRandomDeviceCategoriesDropDownList();
            var notedSelectedDeviceCategories = reportManagerPage.ReportEditorPanel.GetDeviceCategoriesValues();
            var notedValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);

            Step("7. Click Save icon");
            reportManagerPage.ReportEditorPanel.ClickSaveButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisappeared();

            Step("8. Expected The new report is saved as a new row in grid of reports");
            var reportsList = reportManagerPage.GridPanel.GetListOfColumnData("Name");
            VerifyEqual(string.Format("8. Verify new report {0} is saved as a new row in grid of reports", reportName), true, reportsList.Exists(p => p.Equals(reportName)));

            Step("9. Select the newly-created report");
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();

            Step("10. Expected Report Details panel appears with section; Verify input values are remained");
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            var actualSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            var actualSelectedDeviceCategories = reportManagerPage.ReportEditorPanel.GetDeviceCategoriesValues();
            var actualValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);
            VerifyEqual("10. Verify input values are remained", notedValues, actualValues);
            VerifyEqual("10. Verify Selected Contacts are remained", notedSelectedContacts, actualSelectedContacts);
            VerifyEqual("10. Verify Selected Device Categories are remained", notedSelectedDeviceCategories, actualSelectedDeviceCategories);

            Step("11. Reload browser and browse to the created report again");
            desktopPage = Browser.RefreshLoggedInCMS();
            reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();

            Step("12. Expected Verify input values are remained");
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            actualSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            actualSelectedDeviceCategories = reportManagerPage.ReportEditorPanel.GetDeviceCategoriesValues();
            actualValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);
            VerifyEqual("12. Verify input values are remained", notedValues, actualValues);
            VerifyEqual("12. Verify Selected Contacts are remained", notedSelectedContacts, actualSelectedContacts);
            VerifyEqual("12. Verify Selected Device Categories are remained", notedSelectedDeviceCategories, actualSelectedDeviceCategories);

            Step("13. Change values of the report (if a checkbox was checked, unchecked it and otherwise. Select all if it is a multi-selectable dropdown list) and save");
            reportManagerPage.ReportEditorPanel.EnterEditablePropertiesValue(dropdownLists);
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            reportManagerPage.ReportEditorPanel.SelectAllContactsDropDownList();
            notedSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            reportManagerPage.ReportEditorPanel.SelectAllDeviceCategoriesDropDownList();
            notedSelectedDeviceCategories = reportManagerPage.ReportEditorPanel.GetDeviceCategoriesValues();
            notedValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);

            reportManagerPage.ReportEditorPanel.ClickSaveButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisappeared();

            Step("14. Expected");
            Step("  o Name fields is editable (#1325077)");
            Step("  o Type field is readonly");
            Step("  o All fields are editable");
            Step("  o Saving is a success");
            VerifyEqual("[#1325077] 14. Verify Name field is editable", true, !reportManagerPage.ReportEditorPanel.IsNameInputReadOnly());
            VerifyEqual("14. Verify Type field is readonly", true, reportManagerPage.ReportEditorPanel.IsTypeDropDownReadOnly());
            VerifyEqual("14. Verify All fields are editable", true, reportManagerPage.ReportEditorPanel.AreFieldsEditable());

            Step("15. Select the newly-created report");
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();

            Step("16. Expected Report Details panel appears with section; Verify input values are remained");
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            actualSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            actualSelectedDeviceCategories = reportManagerPage.ReportEditorPanel.GetDeviceCategoriesValues();
            actualValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);
            VerifyEqual("16. Verify input values are remained", notedValues, actualValues);
            VerifyEqual("16. Verify Selected Contacts are remained", notedSelectedContacts, actualSelectedContacts);
            VerifyEqual("16. Verify Selected Device Categories are remained", notedSelectedDeviceCategories, actualSelectedDeviceCategories);

            Step("17. Reload browser and browse to the created report again");
            desktopPage = Browser.RefreshLoggedInCMS();
            reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();

            Step("18. Expected Verify input values are remained");
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            actualSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            actualSelectedDeviceCategories = reportManagerPage.ReportEditorPanel.GetDeviceCategoriesValues();
            actualValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);
            VerifyEqual("18. Verify input values are remained", notedValues, actualValues);
            VerifyEqual("18. Verify Selected Contacts are remained", notedSelectedContacts, actualSelectedContacts);
            VerifyEqual("18. Verify Selected Device Categories are remained", notedSelectedDeviceCategories, actualSelectedDeviceCategories);

            Step("19. Click Delete button in Report Details panel");
            reportManagerPage.ReportEditorPanel.ClickDeleteButton();
            reportManagerPage.WaitForPopupDialogDisplayed();

            Step("20. Expected A confirmation dialog appears with title \"Confirmation\" & message \"Would you like to delete '{{Report Name}}' report ? \"");
            var expectedMessage = string.Format("Would you like to delete '{0}' report ?", reportName);
            VerifyEqual("20. Verify dialog title is 'Confirmation'", "Confirmation", reportManagerPage.Dialog.GetDialogTitleText());
            VerifyEqual(string.Format("20. Verify dialog message is '{0}'", expectedMessage), expectedMessage, reportManagerPage.Dialog.GetMessageText());

            Step("21. Click No");
            reportManagerPage.Dialog.ClickNoButton();
            reportManagerPage.WaitForPopupDialogDisappeared();

            Step("22. Expected The dialog disappears");
            VerifyEqual("Verify The dialog disappears", false, reportManagerPage.IsPopupDialogDisplayed());

            Step("23. Click Delete button in Report Details panel again");
            reportManagerPage.ReportEditorPanel.ClickDeleteButton();
            reportManagerPage.WaitForPopupDialogDisplayed();

            Step("24. Expected A confirmation dialog appears with title \"Confirmation\" & message \"Would you like to delete '{{Report Name}}' report ? \"");
            VerifyEqual("24. Verify dialog title is 'Confirmation'", "Confirmation", reportManagerPage.Dialog.GetDialogTitleText());
            VerifyEqual(string.Format("24. Verify dialog message is '{0}'", expectedMessage), expectedMessage, reportManagerPage.Dialog.GetMessageText());

            Step("25. Click Yes");
            reportManagerPage.Dialog.ClickYesButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForPopupDialogDisappeared();

            Step("26. Expected The dialog disappears; the report is no longer present in the grid panel");
            VerifyEqual("26. Verify The dialog disappears", false, reportManagerPage.IsPopupDialogDisplayed());
            reportsList = reportManagerPage.GridPanel.GetListOfColumnData("Name");
            VerifyEqual("26. Verify The report is no longer present in the grid panel", false, reportsList.Exists(p => p.Equals(reportName)));

            Step("27. Reload browser and go to Report Manager");
            desktopPage = Browser.RefreshLoggedInCMS();
            reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();

            Step("28. Select the geozone where the deleted report was created");
            reportManagerPage.GeozoneTreeMainPanel.SelectNode(xmlGeoZone);

            Step("29. Expected The report is no longer present in the grid panel");
            reportsList = reportManagerPage.GridPanel.GetListOfColumnData("Name");
            VerifyEqual("29. Verify The report is no longer present in the grid panel", false, reportsList.Exists(p => p.Equals(reportName)));
        }

        [Test, DynamicRetry]
        [Description("RM_14 'Over Wattage Report' Report")]
        public void RM_14()
        {
            var testData = GetTestDataOfTestRM_13();

            var xmlGeoZone = testData["GeoZone"].ToString();
            var geozoneName = xmlGeoZone.GetChildName();
            var parentGeozoneName = xmlGeoZone.GetParentName();
            var reportName = SLVHelper.GenerateUniqueName(string.Format("[{0}][{1}] {2}", Browser.Name, Settings.Users["DefaultTest"].Username, TestContext.CurrentContext.Test.MethodName));

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.ReportManager);

            Step("1. Go to Report Manager app");
            Step("2. Expected Report Manager is routed and loaded successfully");
            var reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();

            Step("3. Select a geozone");
            reportManagerPage.GeozoneTreeMainPanel.SelectNode(xmlGeoZone);

            Step("4. Add new report with 'Over Wattage Report' type");
            reportManagerPage.GridPanel.ClickAddReportToolbarButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.ReportEditorPanel.WaitForPanelLoaded();
            reportManagerPage.ReportEditorPanel.SelectTypeDropDown("Over Wattage Report");
            reportManagerPage.WaitForPreviousActionComplete();

            Step("5. Expected Report Details panel appears with section:");
            Step("  o Name");
            Step("  o Type");
            Step("  o Scheduler tab");
            Step("   * Configuration section");
            Step("    - Hour (HH): range [00-23]");
            Step("    - Minute: range [00-59]");
            Step("    - Periodicity : list");
            Step("     + Every day");
            Step("     + Every monday");
            Step("     + Every tuesday");
            Step("     + Every wednesday");
            Step("     + Every friday");
            Step("     + Every saturday");
            Step("     + Every sunday");
            Step("  o Export tab");
            Step("   * FTP section");
            Step("    - FTP Host");
            Step("    - SFTP mode");
            Step("    - FTP user");
            Step("    - FTP password");
            Step("    - FTP file name");
            Step("    - FTP passive mode");
            Step("    - FTP enabled");
            Step("    - FTP format: list");
            Step("     + CSV");
            Step("     + Html");
            Step("     + Plain text");
            Step("   * Mail section");
            Step("    - Mail enabled");
            Step("    - Mail format: list");
            Step("     + CSV");
            Step("     + Html");
            Step("     + Plain text");
            Step("    - Report in mail's body");
            Step("    - Subject");
            Step("    - From");
            Step("    - Contacts:list");
            Step("  o Report tab");
            Step("   * Configuration section");
            Step("    - Device Categories:list");
            Step("    - Recurse");
            VerifyOverWattageReportDetails(reportManagerPage);

            Step("6. Fill values into report fields:");
            Step("  o Name: a unique name");
            Step("  o Other fields (if not specifically indicated):");
            Step("   - Text: Any");
            Step("   - Numeric: Any");
            Step("   - Dropdown: any in list, some in list (multi-selectable)");
            Step("   - Checkbox: randomly un/check");
            var dropdownLists = new string[] { "Contacts", "Device Categories" };
            reportManagerPage.ReportEditorPanel.EnterNameInput(reportName);
            reportManagerPage.ReportEditorPanel.EnterEditablePropertiesValue(dropdownLists);
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            reportManagerPage.ReportEditorPanel.SelectRandomContactsDropDownList();
            var notedSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            reportManagerPage.ReportEditorPanel.SelectRandomDeviceCategoriesDropDownList();
            var notedSelectedDeviceCategories = reportManagerPage.ReportEditorPanel.GetDeviceCategoriesValues();
            var notedValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);

            Step("7. Click Save icon");
            reportManagerPage.ReportEditorPanel.ClickSaveButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisappeared();

            Step("8. Expected The new report is saved as a new row in grid of reports");
            var reportsList = reportManagerPage.GridPanel.GetListOfColumnData("Name");
            VerifyEqual(string.Format("8. Verify new report {0} is saved as a new row in grid of reports", reportName), true, reportsList.Exists(p => p.Equals(reportName)));

            Step("9. Select the newly-created report");
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();

            Step("10. Expected Report Details panel appears with section; Verify input values are remained");
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            var actualSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            var actualSelectedDeviceCategories = reportManagerPage.ReportEditorPanel.GetDeviceCategoriesValues();
            var actualValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);
            VerifyEqual("10. Verify input values are remained", notedValues, actualValues);
            VerifyEqual("10. Verify Selected Contacts are remained", notedSelectedContacts, actualSelectedContacts);
            VerifyEqual("10. Verify Selected Device Categories are remained", notedSelectedDeviceCategories, actualSelectedDeviceCategories);

            Step("11. Reload browser and browse to the created report again");
            desktopPage = Browser.RefreshLoggedInCMS();
            reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();

            Step("12. Expected Verify input values are remained");
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            actualSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            actualSelectedDeviceCategories = reportManagerPage.ReportEditorPanel.GetDeviceCategoriesValues();
            actualValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);
            VerifyEqual("12. Verify input values are remained", notedValues, actualValues);
            VerifyEqual("12. Verify Selected Contacts are remained", notedSelectedContacts, actualSelectedContacts);
            VerifyEqual("12. Verify Selected Device Categories are remained", notedSelectedDeviceCategories, actualSelectedDeviceCategories);

            Step("13. Change values of the report (if a checkbox was checked, unchecked it and otherwise. Select all if it is a multi-selectable dropdown list) and save");
            reportManagerPage.ReportEditorPanel.EnterEditablePropertiesValue(dropdownLists);
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            reportManagerPage.ReportEditorPanel.SelectAllContactsDropDownList();
            notedSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            reportManagerPage.ReportEditorPanel.SelectAllDeviceCategoriesDropDownList();
            notedSelectedDeviceCategories = reportManagerPage.ReportEditorPanel.GetDeviceCategoriesValues();
            notedValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);

            reportManagerPage.ReportEditorPanel.ClickSaveButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisappeared();

            Step("14. Expected");
            Step("  o Name fields is editable (#1325077)");
            Step("  o Type field is readonly");
            Step("  o All fields are editable");
            Step("  o Saving is a success");
            VerifyEqual("[#1325077] 14. Verify Name field is editable", true, !reportManagerPage.ReportEditorPanel.IsNameInputReadOnly());
            VerifyEqual("14. Verify Type field is readonly", true, reportManagerPage.ReportEditorPanel.IsTypeDropDownReadOnly());
            VerifyEqual("14. Verify All fields are editable", true, reportManagerPage.ReportEditorPanel.AreFieldsEditable());

            Step("15. Select the newly-created report");
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();

            Step("16. Expected Report Details panel appears with section; Verify input values are remained");
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            actualSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            actualSelectedDeviceCategories = reportManagerPage.ReportEditorPanel.GetDeviceCategoriesValues();
            actualValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);
            VerifyEqual("16. Verify input values are remained", notedValues, actualValues);
            VerifyEqual("16. Verify Selected Contacts are remained", notedSelectedContacts, actualSelectedContacts);
            VerifyEqual("16. Verify Selected Device Categories are remained", notedSelectedDeviceCategories, actualSelectedDeviceCategories);

            Step("17. Reload browser and browse to the created report again");
            desktopPage = Browser.RefreshLoggedInCMS();
            reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();

            Step("18. Expected Verify input values are remained");
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            actualSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            actualSelectedDeviceCategories = reportManagerPage.ReportEditorPanel.GetDeviceCategoriesValues();
            actualValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);
            VerifyEqual("18. Verify input values are remained", notedValues, actualValues);
            VerifyEqual("18. Verify Selected Contacts are remained", notedSelectedContacts, actualSelectedContacts);
            VerifyEqual("18. Verify Selected Device Categories are remained", notedSelectedDeviceCategories, actualSelectedDeviceCategories);

            Step("19. Click Delete button in Report Details panel");
            reportManagerPage.ReportEditorPanel.ClickDeleteButton();
            reportManagerPage.WaitForPopupDialogDisplayed();

            Step("20. Expected A confirmation dialog appears with title \"Confirmation\" & message \"Would you like to delete '{{Report Name}}' report ? \"");
            var expectedMessage = string.Format("Would you like to delete '{0}' report ?", reportName);
            VerifyEqual("20. Verify dialog title is 'Confirmation'", "Confirmation", reportManagerPage.Dialog.GetDialogTitleText());
            VerifyEqual(string.Format("20. Verify dialog message is '{0}'", expectedMessage), expectedMessage, reportManagerPage.Dialog.GetMessageText());

            Step("21. Click No");
            reportManagerPage.Dialog.ClickNoButton();
            reportManagerPage.WaitForPopupDialogDisappeared();

            Step("22. Expected The dialog disappears");
            VerifyEqual("Verify The dialog disappears", false, reportManagerPage.IsPopupDialogDisplayed());

            Step("23. Click Delete button in Report Details panel again");
            reportManagerPage.ReportEditorPanel.ClickDeleteButton();
            reportManagerPage.WaitForPopupDialogDisplayed();

            Step("24. Expected A confirmation dialog appears with title \"Confirmation\" & message \"Would you like to delete '{{Report Name}}' report ? \"");
            VerifyEqual("24. Verify dialog title is 'Confirmation'", "Confirmation", reportManagerPage.Dialog.GetDialogTitleText());
            VerifyEqual(string.Format("24. Verify dialog message is '{0}'", expectedMessage), expectedMessage, reportManagerPage.Dialog.GetMessageText());

            Step("25. Click Yes");
            reportManagerPage.Dialog.ClickYesButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForPopupDialogDisappeared();

            Step("26. Expected The dialog disappears; the report is no longer present in the grid panel");
            VerifyEqual("26. Verify The dialog disappears", false, reportManagerPage.IsPopupDialogDisplayed());
            reportsList = reportManagerPage.GridPanel.GetListOfColumnData("Name");
            VerifyEqual("26. Verify The report is no longer present in the grid panel", false, reportsList.Exists(p => p.Equals(reportName)));

            Step("27. Reload browser and go to Report Manager");
            desktopPage = Browser.RefreshLoggedInCMS();
            reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();

            Step("28. Select the geozone where the deleted report was created");
            reportManagerPage.GeozoneTreeMainPanel.SelectNode(xmlGeoZone);

            Step("29. Expected The report is no longer present in the grid panel");
            reportsList = reportManagerPage.GridPanel.GetListOfColumnData("Name");
            VerifyEqual("29. Verify The report is no longer present in the grid panel", false, reportsList.Exists(p => p.Equals(reportName)));
        }

        [Test, DynamicRetry]
        [Description("RM_15 'Symology report' Report")]
        public void RM_15()
        {
            var testData = GetTestDataOfTestRM_15();

            var xmlGeoZone = testData["GeoZone"].ToString();
            var geozoneName = xmlGeoZone.GetChildName();
            var parentGeozoneName = xmlGeoZone.GetParentName();
            var reportName = SLVHelper.GenerateUniqueName(string.Format("[{0}][{1}] {2}", Browser.Name, Settings.Users["DefaultTest"].Username, TestContext.CurrentContext.Test.MethodName));

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.ReportManager);

            Step("1. Go to Report Manager app");
            Step("2. Expected Report Manager is routed and loaded successfully");
            var reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();

            Step("3. Select a geozone");
            reportManagerPage.GeozoneTreeMainPanel.SelectNode(xmlGeoZone);

            Step("4. Add new report with 'Symology report' type");
            reportManagerPage.GridPanel.ClickAddReportToolbarButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.ReportEditorPanel.WaitForPanelLoaded();
            reportManagerPage.ReportEditorPanel.SelectTypeDropDown("Symology report");
            reportManagerPage.WaitForPreviousActionComplete();

            Step("5. Expected Report Details panel appears with section:");
            Step("  o Name");
            Step("  o Type");
            Step("  o Properties tab");
            Step("   * Configuration section");
            Step("    - description");
            Step("    - Down meanings: list");
            Step("    - Contact type");
            Step("    - Customer full name");
            Step("    - Customer short code");
            Step("    - Service code");
            Step("    - Unit name");
            Step("    - Web customer ID");
            Step("  o Scheduler tab");
            Step("   * Configuration section");
            Step("    - Hour of day: range [00-23]");
            Step("    - Minute: range [00-59]");
            Step("  o Export tab");
            Step("   * FTP section");
            Step("    - FTP host");
            Step("    - FTP user");
            Step("    - FTP password");
            Step("    - FTP dir");
            Step("    - FTP passive mode");
            VerifySymologyReportDetails(reportManagerPage);

            Step("6. Fill values into report fields:");
            Step("  o Name: a unique name");
            Step("  o Other fields (if not specifically indicated):");
            Step("   - Text: Any");
            Step("   - Numeric: Any");
            Step("   - Dropdown: any in list, some in list (multi-selectable)");
            Step("   - Checkbox: randomly un/check");
            var dropdownLists = new string[] { "Down meanings" };
            reportManagerPage.ReportEditorPanel.EnterNameInput(reportName);
            reportManagerPage.ReportEditorPanel.EnterEditablePropertiesValue(dropdownLists);
            reportManagerPage.ReportEditorPanel.SelectTab("Properties");
            reportManagerPage.ReportEditorPanel.SelectRandomDownMeaningsDropDownList();
            var notedSelectedDownMeanings = reportManagerPage.ReportEditorPanel.GetDownMeaningsValues();
            var notedValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);

            Step("7. Click Save icon");
            reportManagerPage.ReportEditorPanel.ClickSaveButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisappeared();

            Step("8. Expected The new report is saved as a new row in grid of reports");
            var reportsList = reportManagerPage.GridPanel.GetListOfColumnData("Name");
            VerifyEqual(string.Format("8. Verify new report {0} is saved as a new row in grid of reports", reportName), true, reportsList.Exists(p => p.Equals(reportName)));

            Step("9. Select the newly-created report");
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();

            Step("10. Expected Report Details panel appears with section; Verify input values are remained");
            reportManagerPage.ReportEditorPanel.SelectTab("Properties");
            var actualSelectedDownMeanings = reportManagerPage.ReportEditorPanel.GetDownMeaningsValues();
            var actualValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);
            VerifyEqual("10. Verify input values are remained", notedValues, actualValues);
            VerifyEqual("10. Verify Selected Down meanings are remained", notedSelectedDownMeanings, actualSelectedDownMeanings);

            Step("11. Reload browser and browse to the created report again");
            desktopPage = Browser.RefreshLoggedInCMS();
            reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();

            Step("12. Expected Verify input values are remained");
            reportManagerPage.ReportEditorPanel.SelectTab("Properties");
            actualSelectedDownMeanings = reportManagerPage.ReportEditorPanel.GetDownMeaningsValues();
            actualValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);
            VerifyEqual("12. Verify input values are remained", notedValues, actualValues);
            VerifyEqual("12. Verify Selected Down meanings are remained", notedSelectedDownMeanings, actualSelectedDownMeanings);

            Step("13. Change values of the report (if a checkbox was checked, unchecked it and otherwise. Select all if it is a multi-selectable dropdown list) and save");
            reportManagerPage.ReportEditorPanel.EnterEditablePropertiesValue(dropdownLists);
            reportManagerPage.ReportEditorPanel.SelectTab("Properties");
            reportManagerPage.ReportEditorPanel.SelectAllDownMeaningsDropDownList();
            notedSelectedDownMeanings = reportManagerPage.ReportEditorPanel.GetDownMeaningsValues();
            notedValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);

            reportManagerPage.ReportEditorPanel.ClickSaveButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisappeared();

            Step("14. Expected");
            Step("  o Name fields is editable (#1325077)");
            Step("  o Type field is readonly");
            Step("  o All fields are editable");
            Step("  o Saving is a success");
            VerifyEqual("[#1325077] 14. Verify Name field is editable", true, !reportManagerPage.ReportEditorPanel.IsNameInputReadOnly());
            VerifyEqual("14. Verify Type field is readonly", true, reportManagerPage.ReportEditorPanel.IsTypeDropDownReadOnly());
            VerifyEqual("14. Verify All fields are editable", true, reportManagerPage.ReportEditorPanel.AreFieldsEditable());

            Step("15. Select the newly-created report");
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();

            Step("16. Expected Report Details panel appears with section; Verify input values are remained");
            reportManagerPage.ReportEditorPanel.SelectTab("Properties");
            actualSelectedDownMeanings = reportManagerPage.ReportEditorPanel.GetDownMeaningsValues();
            actualValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);
            VerifyEqual("16. Verify input values are remained", notedValues, actualValues);
            VerifyEqual("16. Verify Selected Down meanings are remained", notedSelectedDownMeanings, actualSelectedDownMeanings);

            Step("17. Reload browser and browse to the created report again");
            desktopPage = Browser.RefreshLoggedInCMS();
            reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();

            Step("18. Expected Verify input values are remained");
            reportManagerPage.ReportEditorPanel.SelectTab("Properties");
            actualSelectedDownMeanings = reportManagerPage.ReportEditorPanel.GetDownMeaningsValues();
            actualValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);
            VerifyEqual("18. Verify input values are remained", notedValues, actualValues);
            VerifyEqual("18. Verify Selected Down meanings are remained", notedSelectedDownMeanings, actualSelectedDownMeanings);

            Step("19. Click Delete button in Report Details panel");
            reportManagerPage.ReportEditorPanel.ClickDeleteButton();
            reportManagerPage.WaitForPopupDialogDisplayed();

            Step("20. Expected A confirmation dialog appears with title \"Confirmation\" & message \"Would you like to delete '{{Report Name}}' report ? \"");
            var expectedMessage = string.Format("Would you like to delete '{0}' report ?", reportName);
            VerifyEqual("20. Verify dialog title is 'Confirmation'", "Confirmation", reportManagerPage.Dialog.GetDialogTitleText());
            VerifyEqual(string.Format("20. Verify dialog message is '{0}'", expectedMessage), expectedMessage, reportManagerPage.Dialog.GetMessageText());

            Step("21. Click No");
            reportManagerPage.Dialog.ClickNoButton();
            reportManagerPage.WaitForPopupDialogDisappeared();

            Step("22. Expected The dialog disappears");
            VerifyEqual("22. Verify The dialog disappears", false, reportManagerPage.IsPopupDialogDisplayed());

            Step("23. Click Delete button in Report Details panel again");
            reportManagerPage.ReportEditorPanel.ClickDeleteButton();
            reportManagerPage.WaitForPopupDialogDisplayed();

            Step("24. Expected A confirmation dialog appears with title \"Confirmation\" & message \"Would you like to delete '{{Report Name}}' report ? \"");
            VerifyEqual("24. Verify dialog title is 'Confirmation'", "Confirmation", reportManagerPage.Dialog.GetDialogTitleText());
            VerifyEqual(string.Format("24. Verify dialog message is '{0}'", expectedMessage), expectedMessage, reportManagerPage.Dialog.GetMessageText());

            Step("25. Click Yes");
            reportManagerPage.Dialog.ClickYesButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForPopupDialogDisappeared();

            Step("26. Expected The dialog disappears; the report is no longer present in the grid panel");
            VerifyEqual("26. Verify The dialog disappears", false, reportManagerPage.IsPopupDialogDisplayed());
            reportsList = reportManagerPage.GridPanel.GetListOfColumnData("Name");
            VerifyEqual("26. Verify The report is no longer present in the grid panel", false, reportsList.Exists(p => p.Equals(reportName)));

            Step("27. Reload browser and go to Report Manager");
            desktopPage = Browser.RefreshLoggedInCMS();
            reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();

            Step("28. Select the geozone where the deleted report was created");
            reportManagerPage.GeozoneTreeMainPanel.SelectNode(xmlGeoZone);

            Step("29. Expected The report is no longer present in the grid panel");
            reportsList = reportManagerPage.GridPanel.GetListOfColumnData("Name");
            VerifyEqual("29. Verify The report is no longer present in the grid panel", false, reportsList.Exists(p => p.Equals(reportName)));
        }

        [Test, DynamicRetry]
        [Description("RM_16 'UMSUG report' Report")]
        public void RM_16()
        {
            var testData = GetTestDataOfTestRM_16();

            var xmlGeoZone = testData["GeoZone"].ToString();
            var geozoneName = xmlGeoZone.GetChildName();
            var parentGeozoneName = xmlGeoZone.GetParentName();
            var reportName = SLVHelper.GenerateUniqueName(string.Format("[{0}][{1}] {2}", Browser.Name, Settings.Users["DefaultTest"].Username, TestContext.CurrentContext.Test.MethodName));

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.ReportManager);

            Step("1. Go to Report Manager app");
            Step("2. Expected Report Manager is routed and loaded successfully");
            var reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();

            Step("3. Select a geozone");
            reportManagerPage.GeozoneTreeMainPanel.SelectNode(xmlGeoZone);

            Step("4. Add new report with 'UMSUG report' type");
            reportManagerPage.GridPanel.ClickAddReportToolbarButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.ReportEditorPanel.WaitForPanelLoaded();
            reportManagerPage.ReportEditorPanel.SelectTypeDropDown("UMSUG report");
            reportManagerPage.WaitForPreviousActionComplete();

            Step("5. Expected Report Details panel appears with section:");
            Step("  o Name");
            Step("  o Type");
            Step("  o Properties tab");
            Step("   * Configuration section");
            Step("    - description");
            Step("    - Minimum delta time (minutes)");
            Step("    - Minimum delta level (%)");
            Step("    - Down failures: list");
            Step("    - Max retry");
            Step("  o Scheduler tab");
            Step("   * Configuration section");
            Step("    - Hour: range [00-23]");
            Step("    - Minute: range [00-59]");
            Step("  o Export tab");
            Step("   * FTP section");
            Step("    - FTP host");
            Step("    - FTP user");
            Step("    - FTP password");
            Step("    - directory");
            Step("    - FTP passive mode");
            VerifyUMSUGReportDetails(reportManagerPage);

            Step("6. Fill values into report fields:");
            Step("  o Name: a unique name");
            Step("  o Other fields (if not specifically indicated):");
            Step("   - Text: Any");
            Step("   - Numeric: Any");
            Step("   - Dropdown: any in list, some in list (multi-selectable)");
            Step("   - Checkbox: randomly un/check");
            var dropdownLists = new string[] { "Down failures" };
            reportManagerPage.ReportEditorPanel.EnterNameInput(reportName);
            reportManagerPage.ReportEditorPanel.EnterEditablePropertiesValue(dropdownLists);
            reportManagerPage.ReportEditorPanel.SelectTab("Properties");
            reportManagerPage.ReportEditorPanel.SelectRandomDownFailuresDropDownList();
            var notedSelectedDownFailures = reportManagerPage.ReportEditorPanel.GetDownFailuresValues();
            var notedValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);

            Step("7. Click Save icon");
            reportManagerPage.ReportEditorPanel.ClickSaveButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisappeared();

            Step("8. Expected The new report is saved as a new row in grid of reports");
            var reportsList = reportManagerPage.GridPanel.GetListOfColumnData("Name");
            VerifyEqual(string.Format("8. Verify new report {0} is saved as a new row in grid of reports", reportName), true, reportsList.Exists(p => p.Equals(reportName)));

            Step("9. Select the newly-created report");
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();

            Step("10. Expected Report Details panel appears with section; Verify input values are remained");
            reportManagerPage.ReportEditorPanel.SelectTab("Properties");
            var actualSelectedDownFailures = reportManagerPage.ReportEditorPanel.GetDownFailuresValues();
            var actualValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);
            VerifyEqual("10. Verify input values are remained", notedValues, actualValues);
            VerifyEqual("[SC-631] 10. Verify Selected Down failures are remained", notedSelectedDownFailures, actualSelectedDownFailures);

            Step("11. Reload browser and browse to the created report again");
            desktopPage = Browser.RefreshLoggedInCMS();
            reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();

            Step("12. Expected Verify input values are remained");
            reportManagerPage.ReportEditorPanel.SelectTab("Properties");
            actualSelectedDownFailures = reportManagerPage.ReportEditorPanel.GetDownFailuresValues();
            actualValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);
            VerifyEqual("12. Verify input values are remained", notedValues, actualValues);
            VerifyEqual("[SC-631] 12. Verify Selected Down failures are remained", notedSelectedDownFailures, actualSelectedDownFailures);

            Step("13. Change values of the report (if a checkbox was checked, unchecked it and otherwise. Select all if it is a multi-selectable dropdown list) and save");
            reportManagerPage.ReportEditorPanel.EnterEditablePropertiesValue(dropdownLists);
            reportManagerPage.ReportEditorPanel.SelectTab("Properties");
            reportManagerPage.ReportEditorPanel.SelectAllDownFailuresDropDownList();
            notedSelectedDownFailures = reportManagerPage.ReportEditorPanel.GetDownFailuresValues();
            notedValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);

            reportManagerPage.ReportEditorPanel.ClickSaveButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisappeared();

            Step("14. Expected");
            Step("  o Name fields is editable (#1325077)");
            Step("  o Type field is readonly");
            Step("  o All fields are editable");
            Step("  o Saving is a success");
            VerifyEqual("[#1325077] 14. Verify Name field is editable", true, !reportManagerPage.ReportEditorPanel.IsNameInputReadOnly());
            VerifyEqual("14. Verify Type field is readonly", true, reportManagerPage.ReportEditorPanel.IsTypeDropDownReadOnly());
            VerifyEqual("14. Verify All fields are editable", true, reportManagerPage.ReportEditorPanel.AreFieldsEditable());

            Step("15. Select the newly-created report");
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();

            Step("16. Expected Report Details panel appears with section; Verify input values are remained");
            reportManagerPage.ReportEditorPanel.SelectTab("Properties");
            actualSelectedDownFailures = reportManagerPage.ReportEditorPanel.GetDownFailuresValues();
            actualValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);
            VerifyEqual("16. Verify input values are remained", notedValues, actualValues);
            VerifyEqual("[SC-631] 16. Verify Selected Down failures are remained", notedSelectedDownFailures, actualSelectedDownFailures);

            Step("17. Reload browser and browse to the created report again");
            desktopPage = Browser.RefreshLoggedInCMS();
            reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();

            Step("18. Expected Verify input values are remained");
            reportManagerPage.ReportEditorPanel.SelectTab("Properties");
            actualSelectedDownFailures = reportManagerPage.ReportEditorPanel.GetDownFailuresValues();
            actualValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);
            VerifyEqual("18. Verify input values are remained", notedValues, actualValues);
            VerifyEqual("[SC-631] 18. Verify Selected Down failures are remained", notedSelectedDownFailures, actualSelectedDownFailures);

            Step("19. Click Delete button in Report Details panel");
            reportManagerPage.ReportEditorPanel.ClickDeleteButton();
            reportManagerPage.WaitForPopupDialogDisplayed();

            Step("20. Expected A confirmation dialog appears with title \"Confirmation\" & message \"Would you like to delete '{{Report Name}}' report ? \"");
            var expectedMessage = string.Format("Would you like to delete '{0}' report ?", reportName);
            VerifyEqual("20. Verify dialog title is 'Confirmation'", "Confirmation", reportManagerPage.Dialog.GetDialogTitleText());
            VerifyEqual(string.Format("20. Verify dialog message is '{0}'", expectedMessage), expectedMessage, reportManagerPage.Dialog.GetMessageText());

            Step("21. Click No");
            reportManagerPage.Dialog.ClickNoButton();
            reportManagerPage.WaitForPopupDialogDisappeared();

            Step("22. Expected The dialog disappears");
            VerifyEqual("22. Verify The dialog disappears", false, reportManagerPage.IsPopupDialogDisplayed());

            Step("23. Click Delete button in Report Details panel again");
            reportManagerPage.ReportEditorPanel.ClickDeleteButton();
            reportManagerPage.WaitForPopupDialogDisplayed();

            Step("24. Expected A confirmation dialog appears with title \"Confirmation\" & message \"Would you like to delete '{{Report Name}}' report ? \"");
            VerifyEqual("24. Verify dialog title is 'Confirmation'", "Confirmation", reportManagerPage.Dialog.GetDialogTitleText());
            VerifyEqual(string.Format("24. Verify dialog message is '{0}'", expectedMessage), expectedMessage, reportManagerPage.Dialog.GetMessageText());

            Step("25. Click Yes");
            reportManagerPage.Dialog.ClickYesButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForPopupDialogDisappeared();

            Step("26. Expected The dialog disappears; the report is no longer present in the grid panel");
            VerifyEqual("26. Verify The dialog disappears", false, reportManagerPage.IsPopupDialogDisplayed());
            reportsList = reportManagerPage.GridPanel.GetListOfColumnData("Name");
            VerifyEqual("26. Verify The report is no longer present in the grid panel", false, reportsList.Exists(p => p.Equals(reportName)));

            Step("27. Reload browser and go to Report Manager");
            desktopPage = Browser.RefreshLoggedInCMS();
            reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();

            Step("28. Select the geozone where the deleted report was created");
            reportManagerPage.GeozoneTreeMainPanel.SelectNode(xmlGeoZone);

            Step("29. Expected The report is no longer present in the grid panel");
            reportsList = reportManagerPage.GridPanel.GetListOfColumnData("Name");
            VerifyEqual("29. Verify The report is no longer present in the grid panel", false, reportsList.Exists(p => p.Equals(reportName)));
        }

        [Test, DynamicRetry]
        [Description("RM_17 'Unbalanced 3-Phase Cabinets Report' Report")]
        public void RM_17()
        {
            var testData = GetTestDataOfTestRM_17();

            var xmlGeoZone = testData["GeoZone"].ToString();
            var geozoneName = xmlGeoZone.GetChildName();
            var parentGeozoneName = xmlGeoZone.GetParentName();
            var reportName = SLVHelper.GenerateUniqueName(string.Format("[{0}][{1}] {2}", Browser.Name, Settings.Users["DefaultTest"].Username, TestContext.CurrentContext.Test.MethodName));

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.ReportManager);

            Step("1. Go to Report Manager app");
            Step("2. Expected Report Manager is routed and loaded successfully");
            var reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();

            Step("3. Select a geozone");
            reportManagerPage.GeozoneTreeMainPanel.SelectNode(xmlGeoZone);

            Step("4. Add new report with 'Unbalanced 3-Phase Cabinets Report' type");
            reportManagerPage.GridPanel.ClickAddReportToolbarButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.ReportEditorPanel.WaitForPanelLoaded();
            reportManagerPage.ReportEditorPanel.SelectTypeDropDown("Unbalanced 3-Phase Cabinets Report");
            reportManagerPage.WaitForPreviousActionComplete();

            Step("5. Expected Report Details panel appears with section:");
            Step("  o Name");
            Step("  o Type");
            Step("  o Scheduler tab");
            Step("   * Configuration section");
            Step("    - Hour (HH): range [00-23]");
            Step("    - Minute: range [00-59]");
            Step("    - Periodicity : list");
            Step("     + Every day");
            Step("     + Every monday");
            Step("     + Every tuesday");
            Step("     + Every wednesday");
            Step("     + Every friday");
            Step("     + Every saturday");
            Step("     + Every sunday");
            Step("  o Export tab");
            Step("   * FTP section");
            Step("    - FTP Host");
            Step("    - SFTP mode");
            Step("    - FTP user");
            Step("    - FTP password");
            Step("    - FTP file name");
            Step("    - FTP passive mode");
            Step("    - FTP enabled");
            Step("    - FTP format: list");
            Step("     + CSV");
            Step("     + Html");
            Step("     + Plain text");
            Step("   * Mail section");
            Step("    - Mail enabled");
            Step("    - Mail format: list");
            Step("     + CSV");
            Step("     + Html");
            Step("     + Plain text");
            Step("    - Report in mail's body");
            Step("    - Subject");
            Step("    - From");
            Step("    - Contacts:list");
            Step("  o Report tab");
            Step("   * Configuration section");
            Step("    - Device Categories");
            Step("    - Recurse");
            VerifyUnbalanced3PhaseCabinetsReportDetails(reportManagerPage);

            Step("6. Fill values into report fields:");
            Step("  o Name: a unique name");
            Step("  o Other fields (if not specifically indicated):");
            Step("   - Text: Any");
            Step("   - Numeric: Any");
            Step("   - Dropdown: any in list, some in list (multi-selectable)");
            Step("   - Checkbox: randomly un/check");
            var dropdownLists = new string[] { "Contacts", "Device Categories" };
            reportManagerPage.ReportEditorPanel.EnterNameInput(reportName);
            reportManagerPage.ReportEditorPanel.EnterEditablePropertiesValue(dropdownLists);
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            reportManagerPage.ReportEditorPanel.SelectRandomContactsDropDownList();
            var notedSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            reportManagerPage.ReportEditorPanel.SelectRandomDeviceCategoriesDropDownList();
            var notedSelectedDeviceCategories = reportManagerPage.ReportEditorPanel.GetDeviceCategoriesValues();
            var notedValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);

            Step("7. Click Save icon");
            reportManagerPage.ReportEditorPanel.ClickSaveButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisappeared();

            Step("8. Expected The new report is saved as a new row in grid of reports");
            var reportsList = reportManagerPage.GridPanel.GetListOfColumnData("Name");
            VerifyEqual(string.Format("8. Verify new report {0} is saved as a new row in grid of reports", reportName), true, reportsList.Exists(p => p.Equals(reportName)));

            Step("9. Select the newly-created report");
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();

            Step("10. Expected Report Details panel appears with section; Verify input values are remained");
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            var actualSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            var actualSelectedDeviceCategories = reportManagerPage.ReportEditorPanel.GetDeviceCategoriesValues();
            var actualValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);
            VerifyEqual("10. Verify input values are remained", notedValues, actualValues);
            VerifyEqual("10. Verify Selected Contacts are remained", notedSelectedContacts, actualSelectedContacts);
            VerifyEqual("10. Verify Selected Device Categories are remained", notedSelectedDeviceCategories, actualSelectedDeviceCategories);

            Step("11. Reload browser and browse to the created report again");
            desktopPage = Browser.RefreshLoggedInCMS();
            reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();

            Step("12. Expected Verify input values are remained");
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            actualSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            actualSelectedDeviceCategories = reportManagerPage.ReportEditorPanel.GetDeviceCategoriesValues();
            actualValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);
            VerifyEqual("12. Verify input values are remained", notedValues, actualValues);
            VerifyEqual("12. Verify Selected Contacts are remained", notedSelectedContacts, actualSelectedContacts);
            VerifyEqual("12. Verify Selected Device Categories are remained", notedSelectedDeviceCategories, actualSelectedDeviceCategories);

            Step("13. Change values of the report (if a checkbox was checked, unchecked it and otherwise. Select all if it is a multi-selectable dropdown list) and save");
            reportManagerPage.ReportEditorPanel.EnterEditablePropertiesValue(dropdownLists);
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            reportManagerPage.ReportEditorPanel.SelectAllContactsDropDownList();
            notedSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            reportManagerPage.ReportEditorPanel.SelectAllDeviceCategoriesDropDownList();
            notedSelectedDeviceCategories = reportManagerPage.ReportEditorPanel.GetDeviceCategoriesValues();
            notedValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);

            reportManagerPage.ReportEditorPanel.ClickSaveButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisappeared();

            Step("14. Expected");
            Step("  o Name fields is editable (#1325077)");
            Step("  o Type field is readonly");
            Step("  o All fields are editable");
            Step("  o Saving is a success");
            VerifyEqual("[#1325077] 14. Verify Name field is editable", true, !reportManagerPage.ReportEditorPanel.IsNameInputReadOnly());
            VerifyEqual("14. Verify Type field is readonly", true, reportManagerPage.ReportEditorPanel.IsTypeDropDownReadOnly());
            VerifyEqual("14. Verify All fields are editable", true, reportManagerPage.ReportEditorPanel.AreFieldsEditable());

            Step("15. Select the newly-created report");
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();

            Step("16. Expected Report Details panel appears with section; Verify input values are remained");
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            actualSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            actualSelectedDeviceCategories = reportManagerPage.ReportEditorPanel.GetDeviceCategoriesValues();
            actualValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);
            VerifyEqual("16. Verify input values are remained", notedValues, actualValues);
            VerifyEqual("16. Verify Selected Contacts are remained", notedSelectedContacts, actualSelectedContacts);
            VerifyEqual("16. Verify Selected Device Categories are remained", notedSelectedDeviceCategories, actualSelectedDeviceCategories);

            Step("17. Reload browser and browse to the created report again");
            desktopPage = Browser.RefreshLoggedInCMS();
            reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();

            Step("18. Expected Verify input values are remained");
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            actualSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            actualSelectedDeviceCategories = reportManagerPage.ReportEditorPanel.GetDeviceCategoriesValues();
            actualValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);
            VerifyEqual("18. Verify input values are remained", notedValues, actualValues);
            VerifyEqual("18. Verify Selected Contacts are remained", notedSelectedContacts, actualSelectedContacts);
            VerifyEqual("18. Verify Selected Device Categories are remained", notedSelectedDeviceCategories, actualSelectedDeviceCategories);

            Step("19. Click Delete button in Report Details panel");
            reportManagerPage.ReportEditorPanel.ClickDeleteButton();
            reportManagerPage.WaitForPopupDialogDisplayed();

            Step("20. Expected A confirmation dialog appears with title \"Confirmation\" & message \"Would you like to delete '{{Report Name}}' report ? \"");
            var expectedMessage = string.Format("Would you like to delete '{0}' report ?", reportName);
            VerifyEqual("20. Verify dialog title is 'Confirmation'", "Confirmation", reportManagerPage.Dialog.GetDialogTitleText());
            VerifyEqual(string.Format("20. Verify dialog message is '{0}'", expectedMessage), expectedMessage, reportManagerPage.Dialog.GetMessageText());

            Step("21. Click No");
            reportManagerPage.Dialog.ClickNoButton();
            reportManagerPage.WaitForPopupDialogDisappeared();

            Step("22. Expected The dialog disappears");
            VerifyEqual("22. Verify The dialog disappears", false, reportManagerPage.IsPopupDialogDisplayed());

            Step("23. Click Delete button in Report Details panel again");
            reportManagerPage.ReportEditorPanel.ClickDeleteButton();
            reportManagerPage.WaitForPopupDialogDisplayed();

            Step("24. Expected A confirmation dialog appears with title \"Confirmation\" & message \"Would you like to delete '{{Report Name}}' report ? \"");
            VerifyEqual("24. Verify dialog title is 'Confirmation'", "Confirmation", reportManagerPage.Dialog.GetDialogTitleText());
            VerifyEqual(string.Format("24. Verify dialog message is '{0}'", expectedMessage), expectedMessage, reportManagerPage.Dialog.GetMessageText());

            Step("25. Click Yes");
            reportManagerPage.Dialog.ClickYesButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForPopupDialogDisappeared();

            Step("26. Expected The dialog disappears; the report is no longer present in the grid panel");
            VerifyEqual("26. Verify The dialog disappears", false, reportManagerPage.IsPopupDialogDisplayed());
            reportsList = reportManagerPage.GridPanel.GetListOfColumnData("Name");
            VerifyEqual("26. Verify The report is no longer present in the grid panel", false, reportsList.Exists(p => p.Equals(reportName)));

            Step("27. Reload browser and go to Report Manager");
            desktopPage = Browser.RefreshLoggedInCMS();
            reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();

            Step("28. Select the geozone where the deleted report was created");
            reportManagerPage.GeozoneTreeMainPanel.SelectNode(xmlGeoZone);

            Step("29. Expected The report is no longer present in the grid panel");
            reportsList = reportManagerPage.GridPanel.GetListOfColumnData("Name");
            VerifyEqual("29. Verify The report is no longer present in the grid panel", false, reportsList.Exists(p => p.Equals(reportName)));
        }

        [Test, DynamicRetry]
        [Description("RM_18 'Under 110V Voltage Report' Report")]
        public void RM_18()
        {
            var testData = GetTestDataOfTestRM_18();

            var xmlGeoZone = testData["GeoZone"].ToString();
            var geozoneName = xmlGeoZone.GetChildName();
            var parentGeozoneName = xmlGeoZone.GetParentName();
            var reportName = SLVHelper.GenerateUniqueName(string.Format("[{0}][{1}] {2}", Browser.Name, Settings.Users["DefaultTest"].Username, TestContext.CurrentContext.Test.MethodName));

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.ReportManager);

            Step("1. Go to Report Manager app");
            Step("2. Expected Report Manager is routed and loaded successfully");
            var reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();

            Step("3. Select a geozone");
            reportManagerPage.GeozoneTreeMainPanel.SelectNode(xmlGeoZone);

            Step("4. Add new report with 'Under 110V Voltage Report' type");
            reportManagerPage.GridPanel.ClickAddReportToolbarButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.ReportEditorPanel.WaitForPanelLoaded();
            reportManagerPage.ReportEditorPanel.SelectTypeDropDown("Under 110V Voltage Report");
            reportManagerPage.WaitForPreviousActionComplete();

            Step("5. Expected Report Details panel appears with section:");
            Step("  o Name");
            Step("  o Type");
            Step("  o Scheduler tab");
            Step("   * Configuration section");
            Step("    - Hour (HH): range [00-23]");
            Step("    - Minute: range [00-59]");
            Step("    - Periodicity : list");
            Step("     + Every day");
            Step("     + Every monday");
            Step("     + Every tuesday");
            Step("     + Every wednesday");
            Step("     + Every friday");
            Step("     + Every saturday");
            Step("     + Every sunday");
            Step("  o Export tab");
            Step("   * FTP section");
            Step("    - FTP Host");
            Step("    - SFTP mode");
            Step("    - FTP user");
            Step("    - FTP password");
            Step("    - FTP file name");
            Step("    - FTP passive mode");
            Step("    - FTP enabled");
            Step("    - FTP format: list");
            Step("     + CSV");
            Step("     + Html");
            Step("     + Plain text");
            Step("   * Mail section");
            Step("    - Mail enabled");
            Step("    - Mail format: list");
            Step("     + CSV");
            Step("     + Html");
            Step("     + Plain text");
            Step("    - Report in mail's body");
            Step("    - Subject");
            Step("    - From");
            Step("    - Contacts:list");
            Step("  o Report tab");
            Step("   * Configuration section");
            Step("    - Device Categories");
            Step("    - Recurse");
            VerifyUnder110VVoltageReportDetails(reportManagerPage);

            Step("6. Fill values into report fields:");
            Step("  o Name: a unique name");
            Step("  o Other fields (if not specifically indicated):");
            Step("   - Text: Any");
            Step("   - Numeric: Any");
            Step("   - Dropdown: any in list, some in list (multi-selectable)");
            Step("   - Checkbox: randomly un/check");
            var dropdownLists = new string[] { "Contacts", "Device Categories" };
            reportManagerPage.ReportEditorPanel.EnterNameInput(reportName);
            reportManagerPage.ReportEditorPanel.EnterEditablePropertiesValue(dropdownLists);
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            reportManagerPage.ReportEditorPanel.SelectRandomContactsDropDownList();
            var notedSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            reportManagerPage.ReportEditorPanel.SelectRandomDeviceCategoriesDropDownList();
            var notedSelectedDeviceCategories = reportManagerPage.ReportEditorPanel.GetDeviceCategoriesValues();
            var notedValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);

            Step("7. Click Save icon");
            reportManagerPage.ReportEditorPanel.ClickSaveButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisappeared();

            Step("8. Expected The new report is saved as a new row in grid of reports");
            var reportsList = reportManagerPage.GridPanel.GetListOfColumnData("Name");
            VerifyEqual(string.Format("8. Verify new report {0} is saved as a new row in grid of reports", reportName), true, reportsList.Exists(p => p.Equals(reportName)));

            Step("9. Select the newly-created report");
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();

            Step("10. Expected Report Details panel appears with section; Verify input values are remained");
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            var actualSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            var actualSelectedDeviceCategories = reportManagerPage.ReportEditorPanel.GetDeviceCategoriesValues();
            var actualValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);
            VerifyEqual("10. Verify input values are remained", notedValues, actualValues);
            VerifyEqual("10. Verify Selected Contacts are remained", notedSelectedContacts, actualSelectedContacts);
            VerifyEqual("10. Verify Selected Device Categories are remained", notedSelectedDeviceCategories, actualSelectedDeviceCategories);

            Step("11. Reload browser and browse to the created report again");
            desktopPage = Browser.RefreshLoggedInCMS();
            reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();

            Step("12. Expected Verify input values are remained");
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            actualSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            actualSelectedDeviceCategories = reportManagerPage.ReportEditorPanel.GetDeviceCategoriesValues();
            actualValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);
            VerifyEqual("12. Verify input values are remained", notedValues, actualValues);
            VerifyEqual("12. Verify Selected Contacts are remained", notedSelectedContacts, actualSelectedContacts);
            VerifyEqual("12. Verify Selected Device Categories are remained", notedSelectedDeviceCategories, actualSelectedDeviceCategories);

            Step("13. Change values of the report (if a checkbox was checked, unchecked it and otherwise. Select all if it is a multi-selectable dropdown list) and save");
            reportManagerPage.ReportEditorPanel.EnterEditablePropertiesValue(dropdownLists);
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            reportManagerPage.ReportEditorPanel.SelectAllContactsDropDownList();
            notedSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            reportManagerPage.ReportEditorPanel.SelectAllDeviceCategoriesDropDownList();
            notedSelectedDeviceCategories = reportManagerPage.ReportEditorPanel.GetDeviceCategoriesValues();
            notedValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);

            reportManagerPage.ReportEditorPanel.ClickSaveButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisappeared();

            Step("14. Expected");
            Step("  o Name fields is editable (#1325077)");
            Step("  o Type field is readonly");
            Step("  o All fields are editable");
            Step("  o Saving is a success");
            VerifyEqual("[#1325077] 14. Verify Name field is editable", true, !reportManagerPage.ReportEditorPanel.IsNameInputReadOnly());
            VerifyEqual("14. Verify Type field is readonly", true, reportManagerPage.ReportEditorPanel.IsTypeDropDownReadOnly());
            VerifyEqual("14. Verify All fields are editable", true, reportManagerPage.ReportEditorPanel.AreFieldsEditable());

            Step("15. Select the newly-created report");
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();

            Step("16. Expected Report Details panel appears with section; Verify input values are remained");
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            actualSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            actualSelectedDeviceCategories = reportManagerPage.ReportEditorPanel.GetDeviceCategoriesValues();
            actualValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);
            VerifyEqual("16. Verify input values are remained", notedValues, actualValues);
            VerifyEqual("16. Verify Selected Contacts are remained", notedSelectedContacts, actualSelectedContacts);
            VerifyEqual("16. Verify Selected Device Categories are remained", notedSelectedDeviceCategories, actualSelectedDeviceCategories);

            Step("17. Reload browser and browse to the created report again");
            desktopPage = Browser.RefreshLoggedInCMS();
            reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();

            Step("18. Expected Verify input values are remained");
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            actualSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            actualSelectedDeviceCategories = reportManagerPage.ReportEditorPanel.GetDeviceCategoriesValues();
            actualValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);
            VerifyEqual("18. Verify input values are remained", notedValues, actualValues);
            VerifyEqual("18. Verify Selected Contacts are remained", notedSelectedContacts, actualSelectedContacts);
            VerifyEqual("18. Verify Selected Device Categories are remained", notedSelectedDeviceCategories, actualSelectedDeviceCategories);

            Step("19. Click Delete button in Report Details panel");
            reportManagerPage.ReportEditorPanel.ClickDeleteButton();
            reportManagerPage.WaitForPopupDialogDisplayed();

            Step("20. Expected A confirmation dialog appears with title \"Confirmation\" & message \"Would you like to delete '{{Report Name}}' report ? \"");
            var expectedMessage = string.Format("Would you like to delete '{0}' report ?", reportName);
            VerifyEqual("20. Verify dialog title is 'Confirmation'", "Confirmation", reportManagerPage.Dialog.GetDialogTitleText());
            VerifyEqual(string.Format("20. Verify dialog message is '{0}'", expectedMessage), expectedMessage, reportManagerPage.Dialog.GetMessageText());

            Step("21. Click No");
            reportManagerPage.Dialog.ClickNoButton();
            reportManagerPage.WaitForPopupDialogDisappeared();

            Step("22. Expected The dialog disappears");
            VerifyEqual("Verify The dialog disappears", false, reportManagerPage.IsPopupDialogDisplayed());

            Step("23. Click Delete button in Report Details panel again");
            reportManagerPage.ReportEditorPanel.ClickDeleteButton();
            reportManagerPage.WaitForPopupDialogDisplayed();

            Step("24. Expected A confirmation dialog appears with title \"Confirmation\" & message \"Would you like to delete '{{Report Name}}' report ? \"");
            VerifyEqual("24. Verify dialog title is 'Confirmation'", "Confirmation", reportManagerPage.Dialog.GetDialogTitleText());
            VerifyEqual(string.Format("24. Verify dialog message is '{0}'", expectedMessage), expectedMessage, reportManagerPage.Dialog.GetMessageText());

            Step("25. Click Yes");
            reportManagerPage.Dialog.ClickYesButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForPopupDialogDisappeared();

            Step("26. Expected The dialog disappears; the report is no longer present in the grid panel");
            VerifyEqual("26. Verify The dialog disappears", false, reportManagerPage.IsPopupDialogDisplayed());
            reportsList = reportManagerPage.GridPanel.GetListOfColumnData("Name");
            VerifyEqual("26. Verify The report is no longer present in the grid panel", false, reportsList.Exists(p => p.Equals(reportName)));

            Step("27. Reload browser and go to Report Manager");
            desktopPage = Browser.RefreshLoggedInCMS();
            reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();

            Step("28. Select the geozone where the deleted report was created");
            reportManagerPage.GeozoneTreeMainPanel.SelectNode(xmlGeoZone);

            Step("29. Expected The report is no longer present in the grid panel");
            reportsList = reportManagerPage.GridPanel.GetListOfColumnData("Name");
            VerifyEqual("29. Verify The report is no longer present in the grid panel", false, reportsList.Exists(p => p.Equals(reportName)));
        }

        [Test, DynamicRetry]
        [Description("RM_19 'Under Voltage Report' Report")]
        public void RM_19()
        {
            var testData = GetTestDataOfTestRM_19();

            var xmlGeoZone = testData["GeoZone"].ToString();
            var geozoneName = xmlGeoZone.GetChildName();
            var parentGeozoneName = xmlGeoZone.GetParentName();
            var reportName = SLVHelper.GenerateUniqueName(string.Format("[{0}][{1}] {2}", Browser.Name, Settings.Users["DefaultTest"].Username, TestContext.CurrentContext.Test.MethodName));

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.ReportManager);

            Step("1. Go to Report Manager app");
            Step("2. Expected Report Manager is routed and loaded successfully");
            var reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();

            Step("3. Select a geozone");
            reportManagerPage.GeozoneTreeMainPanel.SelectNode(xmlGeoZone);

            Step("4. Add new report with 'Under Voltage Report' type");
            reportManagerPage.GridPanel.ClickAddReportToolbarButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.ReportEditorPanel.WaitForPanelLoaded();
            reportManagerPage.ReportEditorPanel.SelectTypeDropDown("Under Voltage Report");
            reportManagerPage.WaitForPreviousActionComplete();

            Step("5. Expected Report Details panel appears with section:");
            Step("  o Name");
            Step("  o Type");
            Step("  o Scheduler tab");
            Step("   * Configuration section");
            Step("    - Hour (HH): range [00-23]");
            Step("    - Minute: range [00-59]");
            Step("    - Periodicity : list");
            Step("     + Every day");
            Step("     + Every monday");
            Step("     + Every tuesday");
            Step("     + Every wednesday");
            Step("     + Every friday");
            Step("     + Every saturday");
            Step("     + Every sunday");
            Step("  o Export tab");
            Step("   * FTP section");
            Step("    - FTP Host");
            Step("    - SFTP mode");
            Step("    - FTP user");
            Step("    - FTP password");
            Step("    - FTP file name");
            Step("    - FTP passive mode");
            Step("    - FTP enabled");
            Step("    - FTP format: list");
            Step("     + CSV");
            Step("     + Html");
            Step("     + Plain text");
            Step("   * Mail section");
            Step("    - Mail enabled");
            Step("    - Mail format: list");
            Step("     + CSV");
            Step("     + Html");
            Step("     + Plain text");
            Step("    - Report in mail's body");
            Step("    - Subject");
            Step("    - From");
            Step("    - Contacts:list");
            Step("  o Report tab");
            Step("   * Configuration section");
            Step("    - Device Categories:list");
            Step("    - Recurse");
            VerifyUnderVoltageReportDetails(reportManagerPage);

            Step("6. Fill values into report fields:");
            Step("  o Name: a unique name");
            Step("  o Other fields (if not specifically indicated):");
            Step("   - Text: Any");
            Step("   - Numeric: Any");
            Step("   - Dropdown: any in list, some in list (multi-selectable)");
            Step("   - Checkbox: randomly un/check");
            var dropdownLists = new string[] { "Contacts", "Device Categories" };
            reportManagerPage.ReportEditorPanel.EnterNameInput(reportName);
            reportManagerPage.ReportEditorPanel.EnterEditablePropertiesValue(dropdownLists);
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            reportManagerPage.ReportEditorPanel.SelectRandomContactsDropDownList();
            var notedSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            reportManagerPage.ReportEditorPanel.SelectRandomDeviceCategoriesDropDownList();
            var notedSelectedDeviceCategories = reportManagerPage.ReportEditorPanel.GetDeviceCategoriesValues();
            var notedValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);

            Step("7. Click Save icon");
            reportManagerPage.ReportEditorPanel.ClickSaveButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisappeared();

            Step("8. Expected The new report is saved as a new row in grid of reports");
            var reportsList = reportManagerPage.GridPanel.GetListOfColumnData("Name");
            VerifyEqual(string.Format("8. Verify new report {0} is saved as a new row in grid of reports", reportName), true, reportsList.Exists(p => p.Equals(reportName)));

            Step("9. Select the newly-created report");
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();

            Step("10. Expected Report Details panel appears with section; Verify input values are remained");
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            var actualSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            var actualSelectedDeviceCategories = reportManagerPage.ReportEditorPanel.GetDeviceCategoriesValues();
            var actualValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);
            VerifyEqual("10. Verify input values are remained", notedValues, actualValues);
            VerifyEqual("10. Verify Selected Contacts are remained", notedSelectedContacts, actualSelectedContacts);
            VerifyEqual("10. Verify Selected Device Categories are remained", notedSelectedDeviceCategories, actualSelectedDeviceCategories);

            Step("11. Reload browser and browse to the created report again");
            desktopPage = Browser.RefreshLoggedInCMS();
            reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();

            Step("12. Expected Verify input values are remained");
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            actualSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            actualSelectedDeviceCategories = reportManagerPage.ReportEditorPanel.GetDeviceCategoriesValues();
            actualValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);
            VerifyEqual("12. Verify input values are remained", notedValues, actualValues);
            VerifyEqual("12. Verify Selected Contacts are remained", notedSelectedContacts, actualSelectedContacts);
            VerifyEqual("12. Verify Selected Device Categories are remained", notedSelectedDeviceCategories, actualSelectedDeviceCategories);

            Step("13. Change values of the report (if a checkbox was checked, unchecked it and otherwise. Select all if it is a multi-selectable dropdown list) and save");
            reportManagerPage.ReportEditorPanel.EnterEditablePropertiesValue(dropdownLists);
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            reportManagerPage.ReportEditorPanel.SelectAllContactsDropDownList();
            notedSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            reportManagerPage.ReportEditorPanel.SelectAllDeviceCategoriesDropDownList();
            notedSelectedDeviceCategories = reportManagerPage.ReportEditorPanel.GetDeviceCategoriesValues();
            notedValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);

            reportManagerPage.ReportEditorPanel.ClickSaveButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisappeared();

            Step("14. Expected");
            Step("  o Name fields is editable (#1325077)");
            Step("  o Type field is readonly");
            Step("  o All fields are editable");
            Step("  o Saving is a success");
            VerifyEqual("[#1325077] 14. Verify Name field is editable", true, !reportManagerPage.ReportEditorPanel.IsNameInputReadOnly());
            VerifyEqual("14. Verify Type field is readonly", true, reportManagerPage.ReportEditorPanel.IsTypeDropDownReadOnly());
            VerifyEqual("14. Verify All fields are editable", true, reportManagerPage.ReportEditorPanel.AreFieldsEditable());

            Step("15. Select the newly-created report");
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();

            Step("16. Expected Report Details panel appears with section; Verify input values are remained");
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            actualSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            actualSelectedDeviceCategories = reportManagerPage.ReportEditorPanel.GetDeviceCategoriesValues();
            actualValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);
            VerifyEqual("16. Verify input values are remained", notedValues, actualValues);
            VerifyEqual("16. Verify Selected Contacts are remained", notedSelectedContacts, actualSelectedContacts);
            VerifyEqual("16. Verify Selected Device Categories are remained", notedSelectedDeviceCategories, actualSelectedDeviceCategories);

            Step("17. Reload browser and browse to the created report again");
            desktopPage = Browser.RefreshLoggedInCMS();
            reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();

            Step("18. Expected Verify input values are remained");
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            actualSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            actualSelectedDeviceCategories = reportManagerPage.ReportEditorPanel.GetDeviceCategoriesValues();
            actualValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);
            VerifyEqual("18. Verify input values are remained", notedValues, actualValues);
            VerifyEqual("18. Verify Selected Contacts are remained", notedSelectedContacts, actualSelectedContacts);
            VerifyEqual("18. Verify Selected Device Categories are remained", notedSelectedDeviceCategories, actualSelectedDeviceCategories);

            Step("19. Click Delete button in Report Details panel");
            reportManagerPage.ReportEditorPanel.ClickDeleteButton();
            reportManagerPage.WaitForPopupDialogDisplayed();

            Step("20. Expected A confirmation dialog appears with title \"Confirmation\" & message \"Would you like to delete '{{Report Name}}' report ? \"");
            var expectedMessage = string.Format("Would you like to delete '{0}' report ?", reportName);
            VerifyEqual("20. Verify dialog title is 'Confirmation'", "Confirmation", reportManagerPage.Dialog.GetDialogTitleText());
            VerifyEqual(string.Format("20. Verify dialog message is '{0}'", expectedMessage), expectedMessage, reportManagerPage.Dialog.GetMessageText());

            Step("21. Click No");
            reportManagerPage.Dialog.ClickNoButton();
            reportManagerPage.WaitForPopupDialogDisappeared();

            Step("22. Expected The dialog disappears");
            VerifyEqual("22. Verify The dialog disappears", false, reportManagerPage.IsPopupDialogDisplayed());

            Step("23. Click Delete button in Report Details panel again");
            reportManagerPage.ReportEditorPanel.ClickDeleteButton();
            reportManagerPage.WaitForPopupDialogDisplayed();

            Step("24. Expected A confirmation dialog appears with title \"Confirmation\" & message \"Would you like to delete '{{Report Name}}' report ? \"");
            VerifyEqual("24. Verify dialog title is 'Confirmation'", "Confirmation", reportManagerPage.Dialog.GetDialogTitleText());
            VerifyEqual(string.Format("24. Verify dialog message is '{0}'", expectedMessage), expectedMessage, reportManagerPage.Dialog.GetMessageText());

            Step("25. Click Yes");
            reportManagerPage.Dialog.ClickYesButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForPopupDialogDisappeared();

            Step("26. Expected The dialog disappears; the report is no longer present in the grid panel");
            VerifyEqual("Verify The dialog disappears", false, reportManagerPage.IsPopupDialogDisplayed());
            reportsList = reportManagerPage.GridPanel.GetListOfColumnData("Name");
            VerifyEqual("26. Verify The report is no longer present in the grid panel", false, reportsList.Exists(p => p.Equals(reportName)));

            Step("27. Reload browser and go to Report Manager");
            desktopPage = Browser.RefreshLoggedInCMS();
            reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();

            Step("28. Select the geozone where the deleted report was created");
            reportManagerPage.GeozoneTreeMainPanel.SelectNode(xmlGeoZone);

            Step("29. Expected The report is no longer present in the grid panel");
            reportsList = reportManagerPage.GridPanel.GetListOfColumnData("Name");
            VerifyEqual("29. Verify The report is no longer present in the grid panel", false, reportsList.Exists(p => p.Equals(reportName)));
        }

        [Test, DynamicRetry]
        [Description("RM_20 'Under Wattage Report' Report")]
        public void RM_20()
        {
            var testData = GetTestDataOfTestRM_20();

            var xmlGeoZone = testData["GeoZone"].ToString();
            var geozoneName = xmlGeoZone.GetChildName();
            var parentGeozoneName = xmlGeoZone.GetParentName();
            var reportName = SLVHelper.GenerateUniqueName(string.Format("[{0}][{1}] {2}", Browser.Name, Settings.Users["DefaultTest"].Username, TestContext.CurrentContext.Test.MethodName));

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.ReportManager);

            Step("1. Go to Report Manager app");
            Step("2. Expected Report Manager is routed and loaded successfully");
            var reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();

            Step("3. Select a geozone");
            reportManagerPage.GeozoneTreeMainPanel.SelectNode(xmlGeoZone);

            Step("4. Add new report with 'Under Wattage Report' type");
            reportManagerPage.GridPanel.ClickAddReportToolbarButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.ReportEditorPanel.WaitForPanelLoaded();
            reportManagerPage.ReportEditorPanel.SelectTypeDropDown("Under Wattage Report");
            reportManagerPage.WaitForPreviousActionComplete();

            Step("5. Expected Report Details panel appears with section:");
            Step("  o Name");
            Step("  o Type");
            Step("  o Scheduler tab");
            Step("   * Configuration section");
            Step("    - Hour (HH): range [00-23]");
            Step("    - Minute: range [00-59]");
            Step("    - Periodicity : list");
            Step("     + Every day");
            Step("     + Every monday");
            Step("     + Every tuesday");
            Step("     + Every wednesday");
            Step("     + Every friday");
            Step("     + Every saturday");
            Step("     + Every sunday");
            Step("  o Export tab");
            Step("   * FTP section");
            Step("    - FTP Host");
            Step("    - SFTP mode");
            Step("    - FTP user");
            Step("    - FTP password");
            Step("    - FTP file name");
            Step("    - FTP passive mode");
            Step("    - FTP enabled");
            Step("    - FTP format: list");
            Step("     + CSV");
            Step("     + Html");
            Step("     + Plain text");
            Step("   * Mail section");
            Step("    - Mail enabled");
            Step("    - Mail format: list");
            Step("     + CSV");
            Step("     + Html");
            Step("     + Plain text");
            Step("    - Report in mail's body");
            Step("    - Subject");
            Step("    - From");
            Step("    - Contacts:list");
            Step("  o Report tab");
            Step("   * Configuration section");
            Step("    - Device Categories:list");
            Step("    - Recurse");
            VerifyUnderWattageReportDetails(reportManagerPage);

            Step("6. Fill values into report fields:");
            Step("  o Name: a unique name");
            Step("  o Other fields (if not specifically indicated):");
            Step("   - Text: Any");
            Step("   - Numeric: Any");
            Step("   - Dropdown: any in list, some in list (multi-selectable)");
            Step("   - Checkbox: randomly un/check");
            var dropdownLists = new string[] { "Contacts", "Device Categories" };
            reportManagerPage.ReportEditorPanel.EnterNameInput(reportName);
            reportManagerPage.ReportEditorPanel.EnterEditablePropertiesValue(dropdownLists);
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            reportManagerPage.ReportEditorPanel.SelectRandomContactsDropDownList();
            var notedSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            reportManagerPage.ReportEditorPanel.SelectRandomDeviceCategoriesDropDownList();
            var notedSelectedDeviceCategories = reportManagerPage.ReportEditorPanel.GetDeviceCategoriesValues();
            var notedValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);

            Step("7. Click Save icon");
            reportManagerPage.ReportEditorPanel.ClickSaveButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisappeared();

            Step("8. Expected The new report is saved as a new row in grid of reports");
            var reportsList = reportManagerPage.GridPanel.GetListOfColumnData("Name");
            VerifyEqual(string.Format("8. Verify new report {0} is saved as a new row in grid of reports", reportName), true, reportsList.Exists(p => p.Equals(reportName)));

            Step("9. Select the newly-created report");
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();

            Step("10. Expected Report Details panel appears with section; Verify input values are remained");
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            var actualSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            var actualSelectedDeviceCategories = reportManagerPage.ReportEditorPanel.GetDeviceCategoriesValues();
            var actualValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);
            VerifyEqual("10. Verify input values are remained", notedValues, actualValues);
            VerifyEqual("10. Verify Selected Contacts are remained", notedSelectedContacts, actualSelectedContacts);
            VerifyEqual("10. Verify Selected Device Categories are remained", notedSelectedDeviceCategories, actualSelectedDeviceCategories);

            Step("11. Reload browser and browse to the created report again");
            desktopPage = Browser.RefreshLoggedInCMS();
            reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();

            Step("12. Expected Verify input values are remained");
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            actualSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            actualSelectedDeviceCategories = reportManagerPage.ReportEditorPanel.GetDeviceCategoriesValues();
            actualValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);
            VerifyEqual("12. Verify input values are remained", notedValues, actualValues);
            VerifyEqual("12. Verify Selected Contacts are remained", notedSelectedContacts, actualSelectedContacts);
            VerifyEqual("12. Verify Selected Device Categories are remained", notedSelectedDeviceCategories, actualSelectedDeviceCategories);

            Step("13. Change values of the report (if a checkbox was checked, unchecked it and otherwise. Select all if it is a multi-selectable dropdown list) and save");
            reportManagerPage.ReportEditorPanel.EnterEditablePropertiesValue(dropdownLists);
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            reportManagerPage.ReportEditorPanel.SelectAllContactsDropDownList();
            notedSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            reportManagerPage.ReportEditorPanel.SelectAllDeviceCategoriesDropDownList();
            notedSelectedDeviceCategories = reportManagerPage.ReportEditorPanel.GetDeviceCategoriesValues();
            notedValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);

            reportManagerPage.ReportEditorPanel.ClickSaveButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisappeared();

            Step("14. Expected");
            Step("  o Name fields is editable (#1325077)");
            Step("  o Type field is readonly");
            Step("  o All fields are editable");
            Step("  o Saving is a success");
            VerifyEqual("[#1325077] 14. Verify Name field is editable", true, !reportManagerPage.ReportEditorPanel.IsNameInputReadOnly());
            VerifyEqual("14. Verify Type field is readonly", true, reportManagerPage.ReportEditorPanel.IsTypeDropDownReadOnly());
            VerifyEqual("14. Verify All fields are editable", true, reportManagerPage.ReportEditorPanel.AreFieldsEditable());

            Step("15. Select the newly-created report");
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();

            Step("16. Expected Report Details panel appears with section; Verify input values are remained");
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            actualSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            actualSelectedDeviceCategories = reportManagerPage.ReportEditorPanel.GetDeviceCategoriesValues();
            actualValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);
            VerifyEqual("16. Verify input values are remained", notedValues, actualValues);
            VerifyEqual("16. Verify Selected Contacts are remained", notedSelectedContacts, actualSelectedContacts);
            VerifyEqual("16. Verify Selected Device Categories are remained", notedSelectedDeviceCategories, actualSelectedDeviceCategories);

            Step("17. Reload browser and browse to the created report again");
            desktopPage = Browser.RefreshLoggedInCMS();
            reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();

            Step("18. Expected Verify input values are remained");
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            actualSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            actualSelectedDeviceCategories = reportManagerPage.ReportEditorPanel.GetDeviceCategoriesValues();
            actualValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);
            VerifyEqual("18. Verify input values are remained", notedValues, actualValues);
            VerifyEqual("18. Verify Selected Contacts are remained", notedSelectedContacts, actualSelectedContacts);
            VerifyEqual("18. Verify Selected Device Categories are remained", notedSelectedDeviceCategories, actualSelectedDeviceCategories);

            Step("19. Click Delete button in Report Details panel");
            reportManagerPage.ReportEditorPanel.ClickDeleteButton();
            reportManagerPage.WaitForPopupDialogDisplayed();

            Step("20. Expected A confirmation dialog appears with title \"Confirmation\" & message \"Would you like to delete '{{Report Name}}' report ? \"");
            var expectedMessage = string.Format("Would you like to delete '{0}' report ?", reportName);
            VerifyEqual("20. Verify dialog title is 'Confirmation'", "Confirmation", reportManagerPage.Dialog.GetDialogTitleText());
            VerifyEqual(string.Format("20. Verify dialog message is '{0}'", expectedMessage), expectedMessage, reportManagerPage.Dialog.GetMessageText());

            Step("21. Click No");
            reportManagerPage.Dialog.ClickNoButton();
            reportManagerPage.WaitForPopupDialogDisappeared();

            Step("22. Expected The dialog disappears");
            VerifyEqual("22. Verify The dialog disappears", false, reportManagerPage.IsPopupDialogDisplayed());

            Step("23. Click Delete button in Report Details panel again");
            reportManagerPage.ReportEditorPanel.ClickDeleteButton();
            reportManagerPage.WaitForPopupDialogDisplayed();

            Step("24. Expected A confirmation dialog appears with title \"Confirmation\" & message \"Would you like to delete '{{Report Name}}' report ? \"");
            VerifyEqual("24. Verify dialog title is 'Confirmation'", "Confirmation", reportManagerPage.Dialog.GetDialogTitleText());
            VerifyEqual(string.Format("24. Verify dialog message is '{0}'", expectedMessage), expectedMessage, reportManagerPage.Dialog.GetMessageText());

            Step("25. Click Yes");
            reportManagerPage.Dialog.ClickYesButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForPopupDialogDisappeared();

            Step("26. Expected The dialog disappears; the report is no longer present in the grid panel");
            VerifyEqual("26. Verify The dialog disappears", false, reportManagerPage.IsPopupDialogDisplayed());
            reportsList = reportManagerPage.GridPanel.GetListOfColumnData("Name");
            VerifyEqual("26. Verify The report is no longer present in the grid panel", false, reportsList.Exists(p => p.Equals(reportName)));

            Step("27. Reload browser and go to Report Manager");
            desktopPage = Browser.RefreshLoggedInCMS();
            reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();

            Step("28. Select the geozone where the deleted report was created");
            reportManagerPage.GeozoneTreeMainPanel.SelectNode(xmlGeoZone);

            Step("29. Expected The report is no longer present in the grid panel");
            reportsList = reportManagerPage.GridPanel.GetListOfColumnData("Name");
            VerifyEqual("29. Verify The report is no longer present in the grid panel", false, reportsList.Exists(p => p.Equals(reportName)));
        }

        [Test, DynamicRetry]
        [Description("RM_21 'Weekly Energy Report' Report")]
        public void RM_21()
        {
            var testData = GetTestDataOfTestRM_21();

            var xmlGeoZone = testData["GeoZone"].ToString();
            var geozoneName = xmlGeoZone.GetChildName();
            var parentGeozoneName = xmlGeoZone.GetParentName();
            var reportName = SLVHelper.GenerateUniqueName(string.Format("[{0}][{1}] {2}", Browser.Name, Settings.Users["DefaultTest"].Username, TestContext.CurrentContext.Test.MethodName));

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.ReportManager);

            Step("1. Go to Report Manager app");
            Step("2. Expected Report Manager is routed and loaded successfully");
            var reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();

            Step("3. Select a geozone");
            reportManagerPage.GeozoneTreeMainPanel.SelectNode(xmlGeoZone);

            Step("4. Add new report with 'Weekly Energy Report' type");
            reportManagerPage.GridPanel.ClickAddReportToolbarButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.ReportEditorPanel.WaitForPanelLoaded();
            reportManagerPage.ReportEditorPanel.SelectTypeDropDown("Weekly Energy Report");
            reportManagerPage.WaitForPreviousActionComplete();

            Step("5. Expected Report Details panel appears with section:");
            Step("  o Name");
            Step("  o Type");
            Step("  o Properties tab");
            Step("   * Configuration section");
            Step("    - description");
            Step("  o Scheduler tab");
            Step("   * Configuration section");
            Step("    - Hour: range [00-23]");
            Step("    - Minute: range [00-59]");
            Step("    - Periodicity : list");
            Step("     + Every day");
            Step("     + Every monday");
            Step("     + Every tuesday");
            Step("     + Every wednesday");
            Step("     + Every friday");
            Step("     + Every saturday");
            Step("     + Every sunday");
            Step("  o Export tab");
            Step("   * Mail section");
            Step("    - Subject");
            Step("    - From");
            Step("    - Contacts:list");
            Step("   * Configuration section");
            Step("    - HTML format");
            VerifyWeeklyEnergyDetails(reportManagerPage);

            Step("6. Fill values into report fields:");
            Step("  o Name: a unique name");
            Step("  o Other fields (if not specifically indicated):");
            Step("   - Text: Any");
            Step("   - Numeric: Any");
            Step("   - Dropdown: any in list, some in list (multi-selectable)");
            Step("   - Checkbox: randomly un/check");
            var dropdownLists = new string[] { "Contacts *" };
            reportManagerPage.ReportEditorPanel.EnterNameInput(reportName);
            reportManagerPage.ReportEditorPanel.EnterEditablePropertiesValue(dropdownLists);
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            reportManagerPage.ReportEditorPanel.SelectRandomContactsDropDownList();
            var notedSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            var notedValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);

            Step("7. Click Save icon");
            reportManagerPage.ReportEditorPanel.ClickSaveButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisappeared();

            Step("8. Expected The new report is saved as a new row in grid of reports");
            var reportsList = reportManagerPage.GridPanel.GetListOfColumnData("Name");
            VerifyEqual(string.Format("8. Verify new report {0} is saved as a new row in grid of reports", reportName), true, reportsList.Exists(p => p.Equals(reportName)));

            Step("9. Select the newly-created report");
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();

            Step("10. Expected Report Details panel appears with section; Verify input values are remained");
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            var actualSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            var actualValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);
            VerifyEqual("10. Verify input values are remained", notedValues, actualValues);
            VerifyEqual("10. Verify Selected Contacts are remained", notedSelectedContacts, actualSelectedContacts);

            Step("11. Reload browser and browse to the created report again");
            desktopPage = Browser.RefreshLoggedInCMS();
            reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();

            Step("12. Expected Verify input values are remained");
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            actualSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            actualValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);
            VerifyEqual("12. Verify input values are remained", notedValues, actualValues);
            VerifyEqual("12. Verify Selected Contacts are remained", notedSelectedContacts, actualSelectedContacts);

            Step("13. Change values of the report (if a checkbox was checked, unchecked it and otherwise. Select all if it is a multi-selectable dropdown list) and save");
            reportManagerPage.ReportEditorPanel.EnterEditablePropertiesValue(dropdownLists);
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            reportManagerPage.ReportEditorPanel.SelectAllContactsDropDownList();
            notedSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            notedValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);

            reportManagerPage.ReportEditorPanel.ClickSaveButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisappeared();

            Step("14. Expected");
            Step("  o Name fields is editable (#1325077)");
            Step("  o Type field is readonly");
            Step("  o All fields are editable");
            Step("  o Saving is a success");
            VerifyEqual("[#1325077] 14. Verify Name field is editable", true, !reportManagerPage.ReportEditorPanel.IsNameInputReadOnly());
            VerifyEqual("14. Verify Type field is readonly", true, reportManagerPage.ReportEditorPanel.IsTypeDropDownReadOnly());
            VerifyEqual("14. Verify All fields are editable", true, reportManagerPage.ReportEditorPanel.AreFieldsEditable());

            Step("15. Select the newly-created report");
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();

            Step("16. Expected Report Details panel appears with section; Verify input values are remained");
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            actualSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            actualValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);
            VerifyEqual("16. Verify input values are remained", notedValues, actualValues);
            VerifyEqual("16. Verify Selected Contacts are remained", notedSelectedContacts, actualSelectedContacts);

            Step("17. Reload browser and browse to the created report again");
            desktopPage = Browser.RefreshLoggedInCMS();
            reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();

            Step("18. Expected Verify input values are remained");
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            actualSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            actualValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);
            VerifyEqual("18. Verify input values are remained", notedValues, actualValues);
            VerifyEqual("18. Verify Selected Contacts are remained", notedSelectedContacts, actualSelectedContacts);

            Step("19. Click Delete button in Report Details panel");
            reportManagerPage.ReportEditorPanel.ClickDeleteButton();
            reportManagerPage.WaitForPopupDialogDisplayed();

            Step("20. Expected A confirmation dialog appears with title \"Confirmation\" & message \"Would you like to delete '{{Report Name}}' report ? \"");
            var expectedMessage = string.Format("Would you like to delete '{0}' report ?", reportName);
            VerifyEqual("20. Verify dialog title is 'Confirmation'", "Confirmation", reportManagerPage.Dialog.GetDialogTitleText());
            VerifyEqual(string.Format("20. Verify dialog message is '{0}'", expectedMessage), expectedMessage, reportManagerPage.Dialog.GetMessageText());

            Step("21. Click No");
            reportManagerPage.Dialog.ClickNoButton();
            reportManagerPage.WaitForPopupDialogDisappeared();

            Step("22. Expected The dialog disappears");
            VerifyEqual("22. Verify The dialog disappears", false, reportManagerPage.IsPopupDialogDisplayed());

            Step("23. Click Delete button in Report Details panel again");
            reportManagerPage.ReportEditorPanel.ClickDeleteButton();
            reportManagerPage.WaitForPopupDialogDisplayed();

            Step("24. Expected A confirmation dialog appears with title \"Confirmation\" & message \"Would you like to delete '{{Report Name}}' report ? \"");
            VerifyEqual("24. Verify dialog title is 'Confirmation'", "Confirmation", reportManagerPage.Dialog.GetDialogTitleText());
            VerifyEqual(string.Format("24. Verify dialog message is '{0}'", expectedMessage), expectedMessage, reportManagerPage.Dialog.GetMessageText());

            Step("25. Click Yes");
            reportManagerPage.Dialog.ClickYesButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForPopupDialogDisappeared();

            Step("26. Expected The dialog disappears; the report is no longer present in the grid panel");
            VerifyEqual("26. Verify The dialog disappears", false, reportManagerPage.IsPopupDialogDisplayed());
            reportsList = reportManagerPage.GridPanel.GetListOfColumnData("Name");
            VerifyEqual("26. Verify The report is no longer present in the grid panel", false, reportsList.Exists(p => p.Equals(reportName)));

            Step("27. Reload browser and go to Report Manager");
            desktopPage = Browser.RefreshLoggedInCMS();
            reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();

            Step("28. Select the geozone where the deleted report was created");
            reportManagerPage.GeozoneTreeMainPanel.SelectNode(xmlGeoZone);

            Step("29. Expected The report is no longer present in the grid panel");
            reportsList = reportManagerPage.GridPanel.GetListOfColumnData("Name");
            VerifyEqual("29. Verify The report is no longer present in the grid panel", false, reportsList.Exists(p => p.Equals(reportName)));
        }

        [Test, DynamicRetry]
        [Description("RM_22 'FlashNet LampFailure Report' Report")]
        public void RM_22()
        {
            var testData = GetTestDataOfTestRM_22();

            var xmlGeoZone = testData["GeoZone"].ToString();
            var geozoneName = xmlGeoZone.GetChildName();
            var parentGeozoneName = xmlGeoZone.GetParentName();
            var reportName = SLVHelper.GenerateUniqueName(string.Format("[{0}][{1}] {2}", Browser.Name, Settings.Users["DefaultTest"].Username, TestContext.CurrentContext.Test.MethodName));

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.ReportManager);

            Step("1. Go to Report Manager app");
            Step("2. Expected Report Manager is routed and loaded successfully");
            var reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();

            Step("3. Select a geozone");
            reportManagerPage.GeozoneTreeMainPanel.SelectNode(xmlGeoZone);

            Step("4. Add new report with 'FlashNet LampFailure Report' type");
            reportManagerPage.GridPanel.ClickAddReportToolbarButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.ReportEditorPanel.WaitForPanelLoaded();
            reportManagerPage.ReportEditorPanel.SelectTypeDropDown("FlashNet LampFailure Report");
            reportManagerPage.WaitForPreviousActionComplete();

            Step("5. Expected Report Details panel appears with section:");
            Step("  o Name");
            Step("  o Type");
            Step("  o Scheduler tab");
            Step("   * Configuration section");
            Step("    - Hour (HH): range [00-23]");
            Step("    - Minute: range [00-59]");
            Step("    - Periodicity : list");
            Step("     + Every day");
            Step("     + Every monday");
            Step("     + Every tuesday");
            Step("     + Every wednesday");
            Step("     + Every friday");
            Step("     + Every saturday");
            Step("     + Every sunday");
            Step("  o Export tab");
            Step("   * FTP section");
            Step("    - FTP Host");
            Step("    - SFTP mode");
            Step("    - FTP user");
            Step("    - FTP password");
            Step("    - FTP file name");
            Step("    - FTP passive mode");
            Step("    - FTP enabled");
            Step("    - FTP format: list");
            Step("     + CSV");
            Step("     + Html");
            Step("     + Plain text");
            Step("   * Mail section");
            Step("    - Mail enabled");
            Step("    - Mail format: list");
            Step("     + CSV");
            Step("     + Html");
            Step("     + Plain text");
            Step("    - Report in mail's body");
            Step("    - Subject");
            Step("    - From");
            Step("    - Contacts:list");
            Step("  o Report tab");
            Step("   * Configuration section");
            Step("    - Device Categories:list");
            Step("    - Recurse");
            VerifyFlashNetLampFailureReportDetails(reportManagerPage);

            Step("6. Fill values into report fields:");
            Step("  o Name: a unique name");
            Step("  o Other fields (if not specifically indicated):");
            Step("   - Text: Any");
            Step("   - Numeric: Any");
            Step("   - Dropdown: any in list, some in list (multi-selectable)");
            Step("   - Checkbox: randomly un/check");
            var dropdownLists = new string[] { "Contacts", "Device Categories" };
            reportManagerPage.ReportEditorPanel.EnterNameInput(reportName);
            reportManagerPage.ReportEditorPanel.EnterEditablePropertiesValue(dropdownLists);
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            reportManagerPage.ReportEditorPanel.SelectRandomContactsDropDownList();
            var notedSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            reportManagerPage.ReportEditorPanel.SelectRandomDeviceCategoriesDropDownList();
            var notedSelectedDeviceCategories = reportManagerPage.ReportEditorPanel.GetDeviceCategoriesValues();
            var notedValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);

            Step("7. Click Save icon");
            reportManagerPage.ReportEditorPanel.ClickSaveButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisappeared();

            Step("8. Expected The new report is saved as a new row in grid of reports");
            var reportsList = reportManagerPage.GridPanel.GetListOfColumnData("Name");
            VerifyEqual(string.Format("8. Verify new report {0} is saved as a new row in grid of reports", reportName), true, reportsList.Exists(p => p.Equals(reportName)));

            Step("9. Select the newly-created report");
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();

            Step("10. Expected Report Details panel appears with section; Verify input values are remained");
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            var actualSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            var actualSelectedDeviceCategories = reportManagerPage.ReportEditorPanel.GetDeviceCategoriesValues();
            var actualValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);
            VerifyEqual("10. Verify input values are remained", notedValues, actualValues);
            VerifyEqual("10. Verify Selected Contacts are remained", notedSelectedContacts, actualSelectedContacts);
            VerifyEqual("10. Verify Selected Device Categories are remained", notedSelectedDeviceCategories, actualSelectedDeviceCategories);

            Step("11. Reload browser and browse to the created report again");
            desktopPage = Browser.RefreshLoggedInCMS();
            reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();

            Step("12. Expected Verify input values are remained");
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            actualSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            actualSelectedDeviceCategories = reportManagerPage.ReportEditorPanel.GetDeviceCategoriesValues();
            actualValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);
            VerifyEqual("12. Verify input values are remained", notedValues, actualValues);
            VerifyEqual("12. Verify Selected Contacts are remained", notedSelectedContacts, actualSelectedContacts);
            VerifyEqual("12. Verify Selected Device Categories are remained", notedSelectedDeviceCategories, actualSelectedDeviceCategories);

            Step("13. Change values of the report (if a checkbox was checked, unchecked it and otherwise. Select all if it is a multi-selectable dropdown list) and save");
            reportManagerPage.ReportEditorPanel.EnterEditablePropertiesValue(dropdownLists);
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            reportManagerPage.ReportEditorPanel.SelectAllContactsDropDownList();
            notedSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            reportManagerPage.ReportEditorPanel.SelectAllDeviceCategoriesDropDownList();
            notedSelectedDeviceCategories = reportManagerPage.ReportEditorPanel.GetDeviceCategoriesValues();
            notedValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);

            reportManagerPage.ReportEditorPanel.ClickSaveButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisappeared();

            Step("14. Expected");
            Step("  o Name fields is editable (#1325077)");
            Step("  o Type field is readonly");
            Step("  o All fields are editable");
            Step("  o Saving is a success");
            VerifyEqual("[#1325077] 14. Verify Name field is editable", true, !reportManagerPage.ReportEditorPanel.IsNameInputReadOnly());
            VerifyEqual("14. Verify Type field is readonly", true, reportManagerPage.ReportEditorPanel.IsTypeDropDownReadOnly());
            VerifyEqual("14. Verify All fields are editable", true, reportManagerPage.ReportEditorPanel.AreFieldsEditable());

            Step("15. Select the newly-created report");
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();

            Step("16. Expected Report Details panel appears with section; Verify input values are remained");
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            actualSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            actualSelectedDeviceCategories = reportManagerPage.ReportEditorPanel.GetDeviceCategoriesValues();
            actualValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);
            VerifyEqual("16. Verify input values are remained", notedValues, actualValues);
            VerifyEqual("16. Verify Selected Contacts are remained", notedSelectedContacts, actualSelectedContacts);
            VerifyEqual("16. Verify Selected Device Categories are remained", notedSelectedDeviceCategories, actualSelectedDeviceCategories);

            Step("17. Reload browser and browse to the created report again");
            desktopPage = Browser.RefreshLoggedInCMS();
            reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();

            Step("18. Expected Verify input values are remained");
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            actualSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            actualSelectedDeviceCategories = reportManagerPage.ReportEditorPanel.GetDeviceCategoriesValues();
            actualValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);
            VerifyEqual("18. Verify input values are remained", notedValues, actualValues);
            VerifyEqual("18. Verify Selected Contacts are remained", notedSelectedContacts, actualSelectedContacts);
            VerifyEqual("18. Verify Selected Device Categories are remained", notedSelectedDeviceCategories, actualSelectedDeviceCategories);

            Step("19. Click Delete button in Report Details panel");
            reportManagerPage.ReportEditorPanel.ClickDeleteButton();
            reportManagerPage.WaitForPopupDialogDisplayed();

            Step("20. Expected A confirmation dialog appears with title \"Confirmation\" & message \"Would you like to delete '{{Report Name}}' report ? \"");
            var expectedMessage = string.Format("Would you like to delete '{0}' report ?", reportName);
            VerifyEqual("20. Verify dialog title is 'Confirmation'", "Confirmation", reportManagerPage.Dialog.GetDialogTitleText());
            VerifyEqual(string.Format("20. Verify dialog message is '{0}'", expectedMessage), expectedMessage, reportManagerPage.Dialog.GetMessageText());

            Step("21. Click No");
            reportManagerPage.Dialog.ClickNoButton();
            reportManagerPage.WaitForPopupDialogDisappeared();

            Step("22. Expected The dialog disappears");
            VerifyEqual("22. Verify The dialog disappears", false, reportManagerPage.IsPopupDialogDisplayed());

            Step("23. Click Delete button in Report Details panel again");
            reportManagerPage.ReportEditorPanel.ClickDeleteButton();
            reportManagerPage.WaitForPopupDialogDisplayed();

            Step("24. Expected A confirmation dialog appears with title \"Confirmation\" & message \"Would you like to delete '{{Report Name}}' report ? \"");
            VerifyEqual("24. Verify dialog title is 'Confirmation'", "Confirmation", reportManagerPage.Dialog.GetDialogTitleText());
            VerifyEqual(string.Format("24. Verify dialog message is '{0}'", expectedMessage), expectedMessage, reportManagerPage.Dialog.GetMessageText());

            Step("25. Click Yes");
            reportManagerPage.Dialog.ClickYesButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForPopupDialogDisappeared();

            Step("26. Expected The dialog disappears; the report is no longer present in the grid panel");
            VerifyEqual("26. Verify The dialog disappears", false, reportManagerPage.IsPopupDialogDisplayed());
            reportsList = reportManagerPage.GridPanel.GetListOfColumnData("Name");
            VerifyEqual("26. Verify The report is no longer present in the grid panel", false, reportsList.Exists(p => p.Equals(reportName)));

            Step("27. Reload browser and go to Report Manager");
            desktopPage = Browser.RefreshLoggedInCMS();
            reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();

            Step("28. Select the geozone where the deleted report was created");
            reportManagerPage.GeozoneTreeMainPanel.SelectNode(xmlGeoZone);

            Step("29. Expected The report is no longer present in the grid panel");
            reportsList = reportManagerPage.GridPanel.GetListOfColumnData("Name");
            VerifyEqual("29. Verify The report is no longer present in the grid panel", false, reportsList.Exists(p => p.Equals(reportName)));
        }

        [Test, DynamicRetry]
        [Description("RM_23 'Location change report' Report")]
        public void RM_23()
        {
            var testData = GetTestDataOfTestRM_23();

            var xmlGeoZone = testData["GeoZone"].ToString();
            var geozoneName = xmlGeoZone.GetChildName();
            var parentGeozoneName = xmlGeoZone.GetParentName();
            var reportName = SLVHelper.GenerateUniqueName(string.Format("[{0}][{1}] {2}", Browser.Name, Settings.Users["DefaultTest"].Username, TestContext.CurrentContext.Test.MethodName));

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.ReportManager);

            Step("1. Go to Report Manager app");
            Step("2. Expected Report Manager is routed and loaded successfully");
            var reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();

            Step("3. Select a geozone");
            reportManagerPage.GeozoneTreeMainPanel.SelectNode(xmlGeoZone);

            Step("4. Add new report with 'Location change report' type");
            reportManagerPage.GridPanel.ClickAddReportToolbarButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.ReportEditorPanel.WaitForPanelLoaded();
            reportManagerPage.ReportEditorPanel.SelectTypeDropDown("Location change report");
            reportManagerPage.WaitForPreviousActionComplete();

            Step("5. Expected Report Details panel appears with section:");
            Step("  o Name");
            Step("  o Type");
            Step("  o Scheduler tab");
            Step("   * Configuration section");
            Step("    - Hour (HH): range [00-23]");
            Step("    - Minute: range [00-59]");
            Step("    - Periodicity : list");
            Step("     + Every day");
            Step("     + Every monday");
            Step("     + Every tuesday");
            Step("     + Every wednesday");
            Step("     + Every friday");
            Step("     + Every saturday");
            Step("     + Every sunday");
            Step("  o Export tab");
            Step("   * FTP section");
            Step("    - FTP Host");
            Step("    - SFTP mode");
            Step("    - FTP user");
            Step("    - FTP password");
            Step("    - FTP file name");
            Step("    - FTP passive mode");
            Step("    - FTP enabled");
            Step("    - FTP format: list");
            Step("     + CSV");
            Step("     + Html");
            Step("     + Plain text");
            Step("   * Mail section");
            Step("    - Mail enabled");
            Step("    - Mail format: list");
            Step("     + CSV");
            Step("     + Html");
            Step("     + Plain text");
            Step("    - Report in mail's body");
            Step("    - Subject");
            Step("    - From");
            Step("    - Contacts:list");
            Step("  o Report tab");
            Step("   * Configuration section");
            Step("    - Device Categories:list");
            Step("    - Period: list");
            Step("     + 1 day");
            Step("     + 1 week");
            Step("     + 1 month");
            Step("    - Minimum included");
            Step("    - Minimum shift (for unit change)");
            Step("    - Minimum shift (for whole period)");
            Step("    - Recurse");
            VerifyLocationChangeReportDetails(reportManagerPage);

            Step("6. Fill values into report fields:");
            Step("  o Name: a unique name");
            Step("  o Other fields (if not specifically indicated):");
            Step("   - Text: Any");
            Step("   - Numeric: Any");
            Step("   - Dropdown: any in list, some in list (multi-selectable)");
            Step("   - Checkbox: randomly un/check");
            var dropdownLists = new string[] { "Contacts", "Device Categories" };
            reportManagerPage.ReportEditorPanel.EnterNameInput(reportName);
            reportManagerPage.ReportEditorPanel.EnterEditablePropertiesValue(dropdownLists);
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            reportManagerPage.ReportEditorPanel.SelectRandomContactsDropDownList();
            var notedSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            reportManagerPage.ReportEditorPanel.SelectRandomDeviceCategoriesDropDownList();
            var notedSelectedDeviceCategories = reportManagerPage.ReportEditorPanel.GetDeviceCategoriesValues();
            var notedValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);

            Step("7. Click Save icon");
            reportManagerPage.ReportEditorPanel.ClickSaveButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisappeared();

            Step("8. Expected The new report is saved as a new row in grid of reports");
            var reportsList = reportManagerPage.GridPanel.GetListOfColumnData("Name");
            VerifyEqual(string.Format("8. Verify new report {0} is saved as a new row in grid of reports", reportName), true, reportsList.Exists(p => p.Equals(reportName)));

            Step("9. Select the newly-created report");
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();

            Step("10. Expected Report Details panel appears with section; Verify input values are remained");
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            var actualSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            var actualSelectedDeviceCategories = reportManagerPage.ReportEditorPanel.GetDeviceCategoriesValues();
            var actualValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);
            VerifyEqual("10. Verify input values are remained", notedValues, actualValues);
            VerifyEqual("10. Verify Selected Contacts are remained", notedSelectedContacts, actualSelectedContacts);
            VerifyEqual("10. Verify Selected Device Categories are remained", notedSelectedDeviceCategories, actualSelectedDeviceCategories);

            Step("11. Reload browser and browse to the created report again");
            desktopPage = Browser.RefreshLoggedInCMS();
            reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();

            Step("12. Expected Verify input values are remained");
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            actualSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            actualSelectedDeviceCategories = reportManagerPage.ReportEditorPanel.GetDeviceCategoriesValues();
            actualValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);
            VerifyEqual("12. Verify input values are remained", notedValues, actualValues);
            VerifyEqual("12. Verify Selected Contacts are remained", notedSelectedContacts, actualSelectedContacts);
            VerifyEqual("12. Verify Selected Device Categories are remained", notedSelectedDeviceCategories, actualSelectedDeviceCategories);

            Step("13. Change values of the report (if a checkbox was checked, unchecked it and otherwise. Select all if it is a multi-selectable dropdown list) and save");
            reportManagerPage.ReportEditorPanel.EnterEditablePropertiesValue(dropdownLists);
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            reportManagerPage.ReportEditorPanel.SelectAllContactsDropDownList();
            notedSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            reportManagerPage.ReportEditorPanel.SelectAllDeviceCategoriesDropDownList();
            notedSelectedDeviceCategories = reportManagerPage.ReportEditorPanel.GetDeviceCategoriesValues();
            notedValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);

            reportManagerPage.ReportEditorPanel.ClickSaveButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisappeared();

            Step("14. Expected");
            Step("  o Name fields is editable (#1325077)");
            Step("  o Type field is readonly");
            Step("  o All fields are editable");
            Step("  o Saving is a success");
            VerifyEqual("[#1325077] 14. Verify Name field is editable", true, !reportManagerPage.ReportEditorPanel.IsNameInputReadOnly());
            VerifyEqual("14. Verify Type field is readonly", true, reportManagerPage.ReportEditorPanel.IsTypeDropDownReadOnly());
            VerifyEqual("14. Verify All fields are editable", true, reportManagerPage.ReportEditorPanel.AreFieldsEditable());

            Step("15. Select the newly-created report");
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();

            Step("16. Expected Report Details panel appears with section; Verify input values are remained");
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            actualSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            actualSelectedDeviceCategories = reportManagerPage.ReportEditorPanel.GetDeviceCategoriesValues();
            actualValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);
            VerifyEqual("16. Verify input values are remained", notedValues, actualValues);
            VerifyEqual("16. Verify Selected Contacts are remained", notedSelectedContacts, actualSelectedContacts);
            VerifyEqual("16. Verify Selected Device Categories are remained", notedSelectedDeviceCategories, actualSelectedDeviceCategories);

            Step("17. Reload browser and browse to the created report again");
            desktopPage = Browser.RefreshLoggedInCMS();
            reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();

            Step("18. Expected Verify input values are remained");
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            actualSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            actualSelectedDeviceCategories = reportManagerPage.ReportEditorPanel.GetDeviceCategoriesValues();
            actualValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);
            VerifyEqual("18. Verify input values are remained", notedValues, actualValues);
            VerifyEqual("18. Verify Selected Contacts are remained", notedSelectedContacts, actualSelectedContacts);
            VerifyEqual("18. Verify Selected Device Categories are remained", notedSelectedDeviceCategories, actualSelectedDeviceCategories);

            Step("19. Click Delete button in Report Details panel");
            reportManagerPage.ReportEditorPanel.ClickDeleteButton();
            reportManagerPage.WaitForPopupDialogDisplayed();

            Step("20. Expected A confirmation dialog appears with title \"Confirmation\" & message \"Would you like to delete '{{Report Name}}' report ? \"");
            var expectedMessage = string.Format("Would you like to delete '{0}' report ?", reportName);
            VerifyEqual("20. Verify dialog title is 'Confirmation'", "Confirmation", reportManagerPage.Dialog.GetDialogTitleText());
            VerifyEqual(string.Format("20. Verify dialog message is '{0}'", expectedMessage), expectedMessage, reportManagerPage.Dialog.GetMessageText());

            Step("21. Click No");
            reportManagerPage.Dialog.ClickNoButton();
            reportManagerPage.WaitForPopupDialogDisappeared();

            Step("22. Expected The dialog disappears");
            VerifyEqual("22. Verify The dialog disappears", false, reportManagerPage.IsPopupDialogDisplayed());

            Step("23. Click Delete button in Report Details panel again");
            reportManagerPage.ReportEditorPanel.ClickDeleteButton();
            reportManagerPage.WaitForPopupDialogDisplayed();

            Step("24. Expected A confirmation dialog appears with title \"Confirmation\" & message \"Would you like to delete '{{Report Name}}' report ? \"");
            VerifyEqual("24. Verify dialog title is 'Confirmation'", "Confirmation", reportManagerPage.Dialog.GetDialogTitleText());
            VerifyEqual(string.Format("24. Verify dialog message is '{0}'", expectedMessage), expectedMessage, reportManagerPage.Dialog.GetMessageText());

            Step("25. Click Yes");
            reportManagerPage.Dialog.ClickYesButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForPopupDialogDisappeared();

            Step("26. Expected The dialog disappears; the report is no longer present in the grid panel");
            VerifyEqual("26. Verify The dialog disappears", false, reportManagerPage.IsPopupDialogDisplayed());
            reportsList = reportManagerPage.GridPanel.GetListOfColumnData("Name");
            VerifyEqual("26. Verify The report is no longer present in the grid panel", false, reportsList.Exists(p => p.Equals(reportName)));

            Step("27. Reload browser and go to Report Manager");
            desktopPage = Browser.RefreshLoggedInCMS();
            reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();

            Step("28. Select the geozone where the deleted report was created");
            reportManagerPage.GeozoneTreeMainPanel.SelectNode(xmlGeoZone);

            Step("29. Expected The report is no longer present in the grid panel");
            reportsList = reportManagerPage.GridPanel.GetListOfColumnData("Name");
            VerifyEqual("29. Verify The report is no longer present in the grid panel", false, reportsList.Exists(p => p.Equals(reportName)));
        }

        [Test, DynamicRetry]
        [Description("RM_24 Report in geozone")]
        public void RM_24()
        {
            var testData = GetTestDataOfTestRM_24();

            var xmlGeoZoneA = testData["GeoZoneA"].ToString();
            var xmlGeoZoneB = testData["GeoZoneB"].ToString();
            var reportName = SLVHelper.GenerateUniqueName(string.Format("[{0}][{1}] {2}", Browser.Name, Settings.Users["DefaultTest"].Username, TestContext.CurrentContext.Test.MethodName));

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.ReportManager);

            Step("1. Go to Report Manager app");
            Step("2. Expected Report Manager is routed and loaded successfully");
            var reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();

            Step("3. Create any report in any geozone (e.g A) other than the root geozone (GeoZones)");
            CreateQuickNewReportRandomType(reportManagerPage, reportName, xmlGeoZoneA);
            
            Step("4. Select the root geozone (GeoZones)");
            reportManagerPage.GeozoneTreeMainPanel.SelectNode(Settings.RootGeozoneName);

            Step("5. Expected The created report is present in grid panel");
            var reportsList = reportManagerPage.GridPanel.GetListOfColumnData("Name");
            VerifyEqual("5. Verify The created report is present in grid panel", true, reportsList.Exists(p => p.Equals(reportName)));

            Step("6. Select another geozone (e.g B)");
            reportManagerPage.GeozoneTreeMainPanel.SelectNode(xmlGeoZoneB);

            Step("7. Expected The created report is NOT present in grid panel");
            reportsList = reportManagerPage.GridPanel.GetListOfColumnData("Name");
            VerifyEqual("7. Verify The created report is NOT present in grid panel", false, reportsList.Exists(p => p.Equals(reportName)));

            Step("8. Select geozone A");
            reportManagerPage.GeozoneTreeMainPanel.SelectNode(xmlGeoZoneA);

            Step("9. Expected The created report is present in grid panel");
            reportsList = reportManagerPage.GridPanel.GetListOfColumnData("Name");
            VerifyEqual("9. Verify The created report is present in grid panel", true, reportsList.Exists(p => p.Equals(reportName)));

            Step("10. Delele the report");
            reportManagerPage.DeleteReport(reportName);

            Step("11. Expected The created report is no longer present in grid panel");
            reportsList = reportManagerPage.GridPanel.GetListOfColumnData("Name");
            VerifyEqual("11. Verify The created report is NOT present in grid panel", false, reportsList.Exists(p => p.Equals(reportName)));

            Step("12. Select the root GeoZones");
            reportManagerPage.GeozoneTreeMainPanel.SelectNode(Settings.RootGeozoneName);

            Step("13. Expected The created report is no longer present in grid panel");
            reportsList = reportManagerPage.GridPanel.GetListOfColumnData("Name");
            VerifyEqual("13. Verify The created report is NOT present in grid panel", false, reportsList.Exists(p => p.Equals(reportName)));

            Step("14. Select geozone B");
            reportManagerPage.GeozoneTreeMainPanel.SelectNode(xmlGeoZoneB);

            Step("15. Expected The created report is NOT longer present in grid panel");
            reportsList = reportManagerPage.GridPanel.GetListOfColumnData("Name");
            VerifyEqual("15. Verify The created report is NOT present in grid panel", false, reportsList.Exists(p => p.Equals(reportName)));

            Step("16. Select geozone A");
            reportManagerPage.GeozoneTreeMainPanel.SelectNode(xmlGeoZoneA);

            Step("17. Expected The created report is no longer present in grid panel");
            reportsList = reportManagerPage.GridPanel.GetListOfColumnData("Name");
            VerifyEqual("17. Verify The created report is NOT present in grid panel", false, reportsList.Exists(p => p.Equals(reportName)));
        }

        [Test, DynamicRetry]
        [Description("RM_25 'Generic device values [Run Once]' Report")]
        public void RM_25()
        {
            var testData = GetTestDataOfTestRM_25();
            var xmlGeoZone = testData["GeoZone"].ToString();
            var reportName = SLVHelper.GenerateUniqueName(string.Format("[{0}][{1}] {2}", Browser.Name, Settings.Users["DefaultTest"].Username, TestContext.CurrentContext.Test.MethodName));

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.ReportManager);

            Step("1. Go to Report Manager app");
            Step("2. Expected Report Manager is routed and loaded successfully");
            var reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();

            Step("3. Select a geozone");
            reportManagerPage.GeozoneTreeMainPanel.SelectNode(xmlGeoZone);

            Step("4. Add new report with 'Generic device values [Run Once]' type");
            reportManagerPage.GridPanel.ClickAddReportToolbarButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.ReportEditorPanel.WaitForPanelLoaded();
            reportManagerPage.ReportEditorPanel.SelectTypeDropDown("Generic device values [Run Once]");
            reportManagerPage.WaitForPreviousActionComplete();

            Step("5. Expected Report Details panel appears with section:");
            Step("  o Name");
            Step("  o Type");
            Step("  o Export tab");
            Step("   * FTP section");
            Step("    - FTP host");
            Step("    - SFTP mode");
            Step("    - FTP user");
            Step("    - FTP password");
            Step("    - FTP filename");
            Step("    - FTP passive mode");
            Step("    - FTP enabled");
            Step("    - FTP format: list");
            Step("     + CSV");
            Step("     + Html");
            Step("     + Plain text");
            Step("   * Mail section");
            Step("    - Mail enabled");
            Step("    - Mail format: list");
            Step("     + CSV");
            Step("     + Html");
            Step("     + Plain text");
            Step("    - Report in mail's body");
            Step("    - Subject");
            Step("    - From");
            Step("    - Contacts:list");
            Step("  o Report tab");
            Step("   * Configuration section");
            Step("    - Device Categories: list");
            Step("    - Values: list");
            Step("    - Start Day: M/D/YYYY");
            Step("    - Start time (HH:mm)");
            Step("    - End day: M/D/YYYY");
            Step("    - End time (HH:mm)");
            VerifyGenericDeviceValuesRunOnceReportDetails(reportManagerPage);

            Step("6. Fill values into report fields:");
            Step("  o Name: a unique name");
            Step("  o Other fields (if not specifically indicated):");
            Step("   - Text: Any");
            Step("   - Numeric: Any");
            Step("   - Dropdown: any in list, some in list (multi-selectable)");
            Step("   - Checkbox: randomly un/check");
            var exceptLists = new string[] { "Contacts", "Device Categories", "Values", "Start day", "End day" };
            reportManagerPage.ReportEditorPanel.EnterNameInput(reportName);
            reportManagerPage.ReportEditorPanel.EnterEditablePropertiesValue(exceptLists);
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            reportManagerPage.ReportEditorPanel.SelectRandomContactsDropDownList();
            var notedSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            reportManagerPage.ReportEditorPanel.SelectRandomDeviceCategoriesDropDownList();
            reportManagerPage.ReportEditorPanel.SelectRandomMeaningsDropDownList();
            reportManagerPage.ReportEditorPanel.EnterStartDayDateInput(new DateTime(SLVHelper.GenerateInteger(1800, 1950), SLVHelper.GenerateInteger(12), SLVHelper.GenerateInteger(28)));
            reportManagerPage.ReportEditorPanel.EnterEndDayDateInput(new DateTime(SLVHelper.GenerateInteger(1951, 2020), SLVHelper.GenerateInteger(12), SLVHelper.GenerateInteger(28)));
            var notedSelectedDeviceCategories = reportManagerPage.ReportEditorPanel.GetDeviceCategoriesValues();
            var notedSelectedValues = reportManagerPage.ReportEditorPanel.GetMeaningsValues();
            var notedValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(exceptLists);
            var notedStartDay = reportManagerPage.ReportEditorPanel.GetStartDayValue();            
            var notedEndDay = reportManagerPage.ReportEditorPanel.GetEndDayValue();

            Step("7. Click Save icon");
            reportManagerPage.ReportEditorPanel.ClickSaveButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisappeared();

            Step("8. Expected The new report is saved as a new row in grid of reports");
            var reportsList = reportManagerPage.GridPanel.GetListOfColumnData("Name");
            VerifyEqual(string.Format("8. Verify new report {0} is saved as a new row in grid of reports", reportName), true, reportsList.Exists(p => p.Equals(reportName)));

            Step("9. Select the newly-created report");
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();

            Step("10. Expected Report Details panel appears with section; Verify input values are remained");
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            var actualSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            var actualSelectedDeviceCategories = reportManagerPage.ReportEditorPanel.GetDeviceCategoriesValues();
            var actualSelectedValues = reportManagerPage.ReportEditorPanel.GetMeaningsValues();
            var actualValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(exceptLists);
            var actualStartDay = reportManagerPage.ReportEditorPanel.GetStartDayValue();
            var actualEndDay = reportManagerPage.ReportEditorPanel.GetEndDayValue();
            VerifyEqual("10. Verify input values are remained", notedValues, actualValues);
            VerifyEqual("10. Verify input values are remained [Start Day]", notedStartDay, actualStartDay);
            VerifyEqual("10. Verify input values are remained [End Day]", notedEndDay, actualEndDay);
            VerifyEqual("10. Verify Selected Contacts are remained", notedSelectedContacts, actualSelectedContacts);
            VerifyEqual("10. Verify Selected Categories are remained", notedSelectedDeviceCategories, actualSelectedDeviceCategories);
            VerifyEqual("[SC-631] 10. Verify Values are remained", notedSelectedValues, actualSelectedValues);

            Step("11. Reload browser and browse to the created report again");
            desktopPage = Browser.RefreshLoggedInCMS();
            reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();

            Step("12. Expected Verify input values are remained");
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            actualSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            actualSelectedDeviceCategories = reportManagerPage.ReportEditorPanel.GetDeviceCategoriesValues();
            actualSelectedValues = reportManagerPage.ReportEditorPanel.GetMeaningsValues();
            actualValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(exceptLists);
            actualStartDay = reportManagerPage.ReportEditorPanel.GetStartDayValue();
            actualEndDay = reportManagerPage.ReportEditorPanel.GetEndDayValue();
            VerifyEqual("12. Verify input values are remained", notedValues, actualValues);
            VerifyEqual("12. Verify input values are remained [Start Day]", notedStartDay, actualStartDay);
            VerifyEqual("12. Verify input values are remained [End Day]", notedEndDay, actualEndDay);
            VerifyEqual("12. Verify Selected Contacts are remained", notedSelectedContacts, actualSelectedContacts);
            VerifyEqual("12. Verify Selected Categories are remained", notedSelectedDeviceCategories, actualSelectedDeviceCategories);
            VerifyEqual("[SC-631] 12. Verify Values are remained", notedSelectedValues, actualSelectedValues);

            Step("13. Change values of the report (if a checkbox was checked, unchecked it and otherwise. Select all if it is a multi-selectable dropdown list) and save");
            reportManagerPage.ReportEditorPanel.EnterEditablePropertiesValue(exceptLists);
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            reportManagerPage.ReportEditorPanel.SelectAllContactsDropDownList();
            notedSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            reportManagerPage.ReportEditorPanel.SelectAllDeviceCategoriesDropDownList();
            reportManagerPage.ReportEditorPanel.SelectAllMeaningsDropDownList();
            reportManagerPage.ReportEditorPanel.EnterStartDayDateInput(new DateTime(SLVHelper.GenerateInteger(1800, 1950), SLVHelper.GenerateInteger(12), SLVHelper.GenerateInteger(28)));
            reportManagerPage.ReportEditorPanel.EnterEndDayDateInput(new DateTime(SLVHelper.GenerateInteger(1951, 2020), SLVHelper.GenerateInteger(12), SLVHelper.GenerateInteger(28)));
            notedSelectedDeviceCategories = reportManagerPage.ReportEditorPanel.GetDeviceCategoriesValues();
            notedSelectedValues = reportManagerPage.ReportEditorPanel.GetMeaningsValues();
            notedValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(exceptLists);
            notedStartDay = reportManagerPage.ReportEditorPanel.GetStartDayValue();
            notedEndDay = reportManagerPage.ReportEditorPanel.GetEndDayValue();

            reportManagerPage.ReportEditorPanel.ClickSaveButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisappeared();

            Step("14. Expected");
            Step("  o Name fields is editable (#1325077)");
            Step("  o Type field is readonly");
            Step("  o All fields are editable");
            Step("  o Saving is a success");
            VerifyEqual("[#1325077] 14. Verify Name field is editable", true, !reportManagerPage.ReportEditorPanel.IsNameInputReadOnly());
            VerifyEqual("14. Verify Type field is readonly", true, reportManagerPage.ReportEditorPanel.IsTypeDropDownReadOnly());
            VerifyEqual("14. Verify All fields are editable", true, reportManagerPage.ReportEditorPanel.AreFieldsEditable());

            Step("15. Select the newly-created report");
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();

            Step("16. Expected Report Details panel appears with section; Verify input values are remained");
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            actualSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            actualSelectedDeviceCategories = reportManagerPage.ReportEditorPanel.GetDeviceCategoriesValues();
            actualSelectedValues = reportManagerPage.ReportEditorPanel.GetMeaningsValues();
            actualValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(exceptLists);
            actualStartDay = reportManagerPage.ReportEditorPanel.GetStartDayValue();
            actualEndDay = reportManagerPage.ReportEditorPanel.GetEndDayValue();
            VerifyEqual("16. Verify input values are remained", notedValues, actualValues);
            VerifyEqual("16. Verify input values are remained [Start Day]", notedStartDay, actualStartDay);
            VerifyEqual("16. Verify input values are remained [End Day]", notedEndDay, actualEndDay);
            VerifyEqual("16. Verify Selected Contacts are remained", notedSelectedContacts, actualSelectedContacts);
            VerifyEqual("16. Verify Selected Categories are remained", notedSelectedDeviceCategories, actualSelectedDeviceCategories);
            VerifyEqual("[SC-631] 16. Verify Values are remained", notedSelectedValues, actualSelectedValues);

            Step("17. Reload browser and browse to the created report again");
            desktopPage = Browser.RefreshLoggedInCMS();
            reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();

            Step("18. Expected Verify input values are remained");
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            actualSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            actualSelectedDeviceCategories = reportManagerPage.ReportEditorPanel.GetDeviceCategoriesValues();
            actualSelectedValues = reportManagerPage.ReportEditorPanel.GetMeaningsValues();
            actualValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(exceptLists);
            actualStartDay = reportManagerPage.ReportEditorPanel.GetStartDayValue();
            actualEndDay = reportManagerPage.ReportEditorPanel.GetEndDayValue();
            VerifyEqual("18. Verify input values are remained", notedValues, actualValues);
            VerifyEqual("18. Verify input values are remained [Start Day]", notedStartDay, actualStartDay);
            VerifyEqual("18. Verify input values are remained [End Day]", notedEndDay, actualEndDay);
            VerifyEqual("18. Verify Selected Contacts are remained", notedSelectedContacts, actualSelectedContacts);
            VerifyEqual("18. Verify Selected Device Categories are remained", notedSelectedDeviceCategories, actualSelectedDeviceCategories);
            VerifyEqual("[SC-631] 18. Verify Values are remained", notedSelectedValues, actualSelectedValues);

            Step("19. Click Delete button in Report Details panel");
            reportManagerPage.ReportEditorPanel.ClickDeleteButton();
            reportManagerPage.WaitForPopupDialogDisplayed();

            Step("20. Expected A confirmation dialog appears with title \"Confirmation\" & message \"Would you like to delete '{{Report Name}}' report ? \"");
            var expectedMessage = string.Format("Would you like to delete '{0}' report ?", reportName);
            VerifyEqual("20. Verify dialog title is 'Confirmation'", "Confirmation", reportManagerPage.Dialog.GetDialogTitleText());
            VerifyEqual(string.Format("20. Verify dialog message is '{0}'", expectedMessage), expectedMessage, reportManagerPage.Dialog.GetMessageText());

            Step("21. Click No");
            reportManagerPage.Dialog.ClickNoButton();
            reportManagerPage.WaitForPopupDialogDisappeared();

            Step("22. Expected The dialog disappears");
            VerifyEqual("Verify The dialog disappears", false, reportManagerPage.IsPopupDialogDisplayed());

            Step("23. Click Delete button in Report Details panel again");
            reportManagerPage.ReportEditorPanel.ClickDeleteButton();
            reportManagerPage.WaitForPopupDialogDisplayed();

            Step("24. Expected A confirmation dialog appears with title \"Confirmation\" & message \"Would you like to delete '{{Report Name}}' report ? \"");
            VerifyEqual("24. Verify dialog title is 'Confirmation'", "Confirmation", reportManagerPage.Dialog.GetDialogTitleText());
            VerifyEqual(string.Format("24. Verify dialog message is '{0}'", expectedMessage), expectedMessage, reportManagerPage.Dialog.GetMessageText());

            Step("25. Click Yes");
            reportManagerPage.Dialog.ClickYesButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForPopupDialogDisappeared();

            Step("26. Expected The dialog disappears; the report is no longer present in the grid panel");
            VerifyEqual("26. Verify The dialog disappears", false, reportManagerPage.IsPopupDialogDisplayed());
            reportsList = reportManagerPage.GridPanel.GetListOfColumnData("Name");
            VerifyEqual("26. Verify The report is no longer present in the grid panel", false, reportsList.Exists(p => p.Equals(reportName)));

            Step("27. Reload browser and go to Report Manager");
            desktopPage = Browser.RefreshLoggedInCMS();
            reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();

            Step("28. Select the geozone where the deleted report was created");
            reportManagerPage.GeozoneTreeMainPanel.SelectNode(xmlGeoZone);

            Step("29. Expected The report is no longer present in the grid panel");
            reportsList = reportManagerPage.GridPanel.GetListOfColumnData("Name");
            VerifyEqual("29. Verify The report is no longer present in the grid panel", false, reportsList.Exists(p => p.Equals(reportName)));
        }

        [Test, DynamicRetry]
        [Description("RM_26 CSV attachments are sent with a .txt extension")]
        public void RM_26()
        {
            var testData = GetTestDataOfTestRM_26();
            var geozone = testData["Geozone"].ToString();
            var reportData = testData["ReportData"] as XmlNode;
            var mail = testData["Mail"] as XmlNode;
            var ftp = testData["Ftp"] as XmlNode;
            var reportType = reportData.GetAttrVal("ReportType");
            var reportName = SLVHelper.GenerateUniqueName(string.Format("[{0}][{1}][{2}] {3}", Browser.Name, Settings.Users["DefaultTest"].Username, TestContext.CurrentContext.Test.MethodName, reportType));
            var mailSubject = string.Format("[{0}][{1}] {2}", Settings.Users["DefaultTest"].Username, TestContext.CurrentContext.Test.MethodName, reportType);
            var ftpFilePattern = SLVHelper.GenerateUniqueName(TestContext.CurrentContext.Test.MethodName + "-" + Settings.Users["DefaultTest"].Username);

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.ReportManager);

            Step("1. Go to Report Manager app");
            Step("2. Verify Report Manager page is routed and loaded successfully");
            var reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;

            Step("3. Create a new report with a random report type which we have known how to run it in any geozone.");
            Step(" - Make sure that the mail configuration having: Mail format: CSV, Report in mail's body: unchecked (to create attachments in the mail)");
            reportManagerPage.GeozoneTreeMainPanel.SelectNode(geozone);
            var currentDate = Settings.GetServerTime();
            var runDate = currentDate.AddMinutes(REPORT_WAIT_MINUTES);

            EmailUtility.CleanInbox(mailSubject);
            reportManagerPage.GridPanel.ClickAddReportToolbarButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.ReportEditorPanel.WaitForPanelLoaded();
            reportManagerPage.ReportEditorPanel.EnterNameInput(reportName);
            reportManagerPage.ReportEditorPanel.SelectTypeDropDown(reportType);
            reportManagerPage.WaitForPreviousActionComplete();

            // Export tab
            //FTP
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            reportManagerPage.ReportEditorPanel.EnterFtpHostInput(ftp.GetAttrVal("Host"));
            reportManagerPage.ReportEditorPanel.TickSftpModeCheckbox(bool.Parse(ftp.GetAttrVal("SFTPMode")));
            reportManagerPage.ReportEditorPanel.EnterFtpUserInput(ftp.GetAttrVal("User"));
            reportManagerPage.ReportEditorPanel.EnterFtpPasswordInput(ftp.GetAttrVal("Password"));
            reportManagerPage.ReportEditorPanel.EnterFtpFilenameInput(ftp.GetAttrVal("Directory") + ftpFilePattern);
            reportManagerPage.ReportEditorPanel.TickFtpEnabledCheckbox(bool.Parse(ftp.GetAttrVal("Enabled")));
            reportManagerPage.ReportEditorPanel.SelectFtpFormatDropDown(reportData.GetAttrVal("FtpFormat"));

            //Mail
            reportManagerPage.ReportEditorPanel.TickMailEnabledCheckbox(bool.Parse(reportData.GetAttrVal("MailEnabled")));
            reportManagerPage.ReportEditorPanel.SelectMailFormatDropDown(reportData.GetAttrVal("MailFormat"));
            reportManagerPage.ReportEditorPanel.TickReportMailBodyCheckbox(bool.Parse(reportData.GetAttrVal("ReportMailBody")));
            reportManagerPage.ReportEditorPanel.EnterSubjectInput(mailSubject);
            reportManagerPage.ReportEditorPanel.EnterFromInput(mail.GetAttrVal("From"));
            reportManagerPage.ReportEditorPanel.SelectContactsListDropDown(mail.GetAttrVal("Contacts"));

            // Report tab
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            reportManagerPage.ReportEditorPanel.SelectAllDeviceCategoriesDropDownList();
            reportManagerPage.ReportEditorPanel.SelectAllMeaningsDropDownList();
            reportManagerPage.ReportEditorPanel.EnterStartDayDateInput(currentDate);
            reportManagerPage.ReportEditorPanel.EnterStartTimeInput(string.Format("{0}:{1}", currentDate.Hour.ToString("D2"), currentDate.Minute.ToString("D2")));
            reportManagerPage.ReportEditorPanel.EnterEndDayDateInput(currentDate);
            reportManagerPage.ReportEditorPanel.EnterEndTimeInput(string.Format("{0}:{1}", runDate.Hour.ToString("D2"), runDate.Minute.ToString("D2")));

            reportManagerPage.ReportEditorPanel.ClickSaveButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisappeared();

            Step("4. Wait for the report run successfully");
            Step("5. Check the attachments in mail and the file uploaded to FTP");
            Step("6. Verify The extension of the attachment and file are CSV");
            var ftpUtility = new FtpUtility(ftp.GetAttrVal("Host"), ftp.GetAttrVal("User"), ftp.GetAttrVal("Password"), ftp.GetAttrVal("Directory"));
            var fileName = ftpUtility.WaitAndGetFileName(ftpFilePattern);
            if (fileName != null)
            {
                VerifyEqual("5. Verify file extension is CSV", true, fileName.Contains(".csv"));
            }
            else
            {
                Warning(string.Format("5. File with pattern {0} does not exist on FTP", ftpFilePattern));
            }

            var tasks = new List<Task>();
            var task = Task.Run(() =>
            {
                var newMail = EmailUtility.GetNewEmail(mailSubject);
                var hasNewMail = newMail != null;
                VerifyTrue(string.Format("5. Verify Report '{0}' has an email sent from '{1}' (Report created: {2}, Expected email revieved: {3})", reportName, mailSubject, Settings.GetServerTime().ToString("G"), currentDate.AddMinutes(REPORT_WAIT_MINUTES).ToString("G")), hasNewMail, "Email sent", "No email sent");
                if (hasNewMail)
                {
                    var attachment = newMail.Attachments.FirstOrDefault();
                    if (attachment != null)
                    {
                        VerifyEqual("6. Verify attachment file extension is CSV", true, attachment.Name.Contains(".csv"));
                    }
                    else
                    {
                        Warning("6. Mail does not have Attachment");
                    }
                }
            });
            tasks.Add(task);
            Task.WaitAll(tasks.ToArray());

            try
            {
                Info("Delete report after testing");
                reportManagerPage.DeleteReport(reportName);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("RM_27 Report UI (covered SC-334)")]
        public void RM_27()
        {
            var testData = GetTestDataOfTestRM_27();
            var geozone = testData["Geozone"].ToString();
            var reportData = testData["ReportData"] as XmlNode;
            var mail = testData["Mail"] as XmlNode;
            var reportType = reportData.GetAttrVal("ReportType");
            var reportName = SLVHelper.GenerateUniqueName(string.Format("[{0}][{1}][{2}] {3}", Browser.Name, Settings.Users["DefaultTest"].Username, TestContext.CurrentContext.Test.MethodName, reportType));
            var mailSubject = string.Format("[{0}][{1}] {2}", Settings.Users["DefaultTest"].Username, TestContext.CurrentContext.Test.MethodName, reportType);
            var reportDetails = reportData.GetAttrVal("ReportDetails");
            var filteringMode = reportData.GetAttrVal("FilteringMode");
            var periodicity = reportData.GetAttrVal("Periodicity");
            var timezone = Settings.DEFAULT_TIMEZONE;
            var fullGeozone = string.Format("{0} ({1})", geozone.GetChildName(), geozone.GetParentName());

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.ReportManager);

            Step("1. Go to Report Manager app");
            Step("2. Verify Report Manager page is routed and loaded successfully");            
            var reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;

            Step("3. Create a new report with a random report type which we have known how to run it in any geozone");
            Step("4. Take note the name, geozone, type, schedule (Hour, Minute, Periodicity, Timezone)");            
            reportManagerPage.GeozoneTreeMainPanel.SelectNode(geozone);
            var currentDate = Settings.GetServerTime();
            var runDate = currentDate.AddMinutes(REPORT_WAIT_MINUTES);
            var mailHour = runDate.Hour;
            var mailMinute = runDate.Minute;

            var expectedTime = string.Format("{0:D2}:{1:D2}", mailHour, mailMinute);
            var expectedSchedule = string.Format("{0} at {1:D2}:{2:D2} [{3}]", periodicity, mailHour, mailMinute, timezone.SplitAndGetAt(new string[] { "[" }, 0));
            var expectedNextExecution = string.Format("{0} {1}:{2:D2}:00 {3}", currentDate.ToString("M/d/yy"), mailHour > 12 ? mailHour - 12 : mailHour, mailMinute, mailHour > 12 ? "PM" : "AM");

            EmailUtility.CleanInbox(mailSubject);
            reportManagerPage.GridPanel.ClickAddReportToolbarButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.ReportEditorPanel.WaitForPanelLoaded();
            reportManagerPage.ReportEditorPanel.EnterNameInput(reportName);
            reportManagerPage.ReportEditorPanel.SelectTypeDropDown(reportType);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.ReportEditorPanel.SelectTab("Properties");
            reportManagerPage.ReportEditorPanel.EnterDescriptionInput(string.Format("Automated {0} Description", reportType));
            reportManagerPage.ReportEditorPanel.SelectReportDetailsDropDown(reportDetails);
            reportManagerPage.ReportEditorPanel.SelectFilteringModeDropDown(filteringMode);
            reportManagerPage.ReportEditorPanel.SelectTab("Scheduler");
            reportManagerPage.ReportEditorPanel.SelectPeriodicityDropDown(periodicity);
            reportManagerPage.ReportEditorPanel.SelectHourDropDown(string.Format("{0:D2}", mailHour));
            reportManagerPage.ReportEditorPanel.SelectMinuteDropDown(string.Format("{0:D2}", mailMinute));
            reportManagerPage.ReportEditorPanel.SelectTimezoneDropDown(timezone);
            reportManagerPage.ReportEditorPanel.ClickSaveButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisappeared();

            Step("5. Verify The new row is added to the report list with the following columns");
            Step(" o Name: the report's name");
            Step(" o GeoZone: the report's geozone (the parent's geozone). Ex: Equipment Inventory (Automation)");
            Step(" o Template: the report's type. Ex: Location change report");
            Step(" o Schedule: {Periodicity} at {Hour}:{Minute} {[Timezone]}. Ex: Every day at 03h27 [Coordinated Universal Time]");
            Step(" o Last execution: empty (because not run yet)");
            Step(" o Next execution: calculated by schedule's periodicity.");
            Step("  + Ex: if Periodicity: Every day > Next execution = the next datetime (format: yyyy-mm-dd hh:MM:ss). Ex: 2018-1-18 03:27:00");
            Step("  + Ex: if Periodicity: Every Monday > Next execution = the next Monday (format: yyyy-mm-dd hh:MM:ss). Ex: 2018-1-22 03:27:00");
            var dt = reportManagerPage.GridPanel.BuildDataTableFromGrid();
            var rows = dt.Select(string.Format("Name = '{0}'", reportName));           
            if (rows.Any())
            {
                var colName = rows[0]["Name"].ToString();
                var colGeozone = rows[0]["GeoZone"].ToString();
                var colTemplate = rows[0]["Template"].ToString();
                var colSchedule = rows[0]["Schedule"].ToString();
                var colLastExecution = rows[0]["Last execution"].ToString();
                var colNextExecution = rows[0]["Next execution"].ToString();
                var actualNextExecution = colNextExecution.Substring(0, colNextExecution.Length - 4);

                VerifyEqual(string.Format("5. Verify Name: the report's name is '{0}'", reportName), reportName, colName);
                VerifyEqual(string.Format("5. Verify GeoZone: the report's geozone (the parent's geozone) is '{0}'", fullGeozone), fullGeozone, colGeozone);
                VerifyEqual(string.Format("[#1429532] 5. Verify Template: the report's type is '{0}'", reportType), reportType, colTemplate);
                VerifyEqual(string.Format("5. Verify Schedule: [Periodicity] at [Hour]:[Minute] [Timezone] is '{0}'", expectedSchedule), expectedSchedule, colSchedule);
                VerifyEqual("[#1429527] 5. Verify Last execution: empty (because not run yet)", "", colLastExecution);
                VerifyEqual(string.Format("5. Verify Next execution: calculated by schedule's periodicity is '{0}'", expectedNextExecution), expectedNextExecution, actualNextExecution);
            }
            else
                Warning(string.Format("5. There is no row with report name '{0}'", reportName));

            Step("6. Press the report row on the list to open the Report Editor panel");
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();
            reportManagerPage.ReportEditorPanel.SelectTab("Scheduler");

            Step("7. Update the scheduler values such as Hour, Minute, Periodicity, Timezone and save changes");
            var newHour = mailHour >= 23 ? mailHour - 1 : mailHour + 1;
            var newMinute = mailMinute >= 59 ? mailMinute - 1 : mailMinute + 1;
            var periodicityList = reportManagerPage.ReportEditorPanel.GetListOfPeriodicity();
            periodicityList.Remove(periodicity);
            var newPeriodicity = periodicityList.PickRandom();
            var weekday = newPeriodicity.SplitAndGetAt(new char[] { ' ' }, 1).ToUpperFirstChar();
            var nextDate = Settings.GetNextWeekday(currentDate, weekday);
            expectedTime = string.Format("{0:D2}:{1:D2}", newHour, newMinute);
            expectedSchedule = string.Format("{0} at {1:D2}:{2:D2} [{3}]", newPeriodicity, newHour, newMinute, timezone.SplitAndGetAt(new string[] { "[" }, 0));
            expectedNextExecution = string.Format("{0} {1}:{2:D2}:00 {3}", nextDate.ToString("M/d/yy"), newHour > 12 ? newHour - 12 : newHour, newMinute, newHour > 12 ? "PM" : "AM");
           
            reportManagerPage.ReportEditorPanel.SelectPeriodicityDropDown(newPeriodicity);
            reportManagerPage.ReportEditorPanel.SelectHourDropDown(string.Format("{0:D2}", newHour));
            reportManagerPage.ReportEditorPanel.SelectMinuteDropDown(string.Format("{0:D2}", newMinute));
            reportManagerPage.ReportEditorPanel.ClickSaveButton();
            reportManagerPage.WaitForPreviousActionComplete();
            if(reportManagerPage.IsPopupDialogDisplayed() && reportManagerPage.Dialog.GetMessageText().Equals("This report name already exists."))
            {
                Warning("#1437477 - 'Report already exists' message shown after editing and saving a report");
                return;
            }
            else
                reportManagerPage.WaitForReportDetailsDisappeared();

            Step("8. Verify the row in the report list is updated the following columns");
            Step(" o Schedule: {Periodicity} at {Hour}:{Minute} {[Timezone]}");
            Step(" o Last execution: still empty (because not run yet)");
            Step(" o Next execution: calculated by schedule's periodicity.");
            dt = reportManagerPage.GridPanel.BuildDataTableFromGrid();
            rows = dt.Select(string.Format("Name = '{0}'", reportName));
            if (rows.Any())
            {                
                var colSchedule = rows[0]["Schedule"].ToString();
                var colLastExecution = rows[0]["Last execution"].ToString();
                var colNextExecution = rows[0]["Next execution"].ToString();
                var actualNextExecution = colNextExecution.Substring(0, colNextExecution.Length - 4);
                
                VerifyEqual(string.Format("8. Verify Schedule: [Periodicity] at [Hour]:[Minute] [Timezone] is '{0}'", expectedSchedule), expectedSchedule, colSchedule);
                VerifyEqual("8. Verify Last execution: empty (because not run yet)", "", colLastExecution);
                VerifyEqual(string.Format("8. Verify Next execution: calculated by schedule's periodicity is '{0}'", expectedNextExecution), expectedNextExecution, actualNextExecution);
            }
            else
                Warning(string.Format("8. There is no row with report name '{0}'", reportName));

            Step("9. Update the scheduler to make the report run a few minutes after and wait for report run successfully");
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();
            reportManagerPage.ReportEditorPanel.SelectTab("Scheduler");
            reportManagerPage.ReportEditorPanel.SelectPeriodicityDropDown(periodicity);
            reportManagerPage.ReportEditorPanel.SelectHourDropDown(string.Format("{0:D2}", mailHour));
            reportManagerPage.ReportEditorPanel.SelectMinuteDropDown(string.Format("{0:D2}", mailMinute));
            reportManagerPage.ReportEditorPanel.SelectTimezoneDropDown(timezone);
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            reportManagerPage.ReportEditorPanel.EnterSubjectInput(mailSubject);
            reportManagerPage.ReportEditorPanel.EnterFromInput(mail.GetAttrVal("From"));
            reportManagerPage.ReportEditorPanel.SelectContactsListDropDown(mail.GetAttrVal("Contacts"));
            reportManagerPage.ReportEditorPanel.ClickSaveButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisappeared();            

            var tasks = new List<Task>();
            var task = Task.Run(() =>
            {
                var newMail = EmailUtility.GetNewEmail(mailSubject);                
                VerifyTrue(string.Format("10. Verify Report '{0}' has an email sent from '{1}' (Report created: {2}, Expected email revieved: {3})", reportName, mailSubject, Settings.GetServerTime().ToString("G"), currentDate.AddMinutes(REPORT_WAIT_MINUTES).ToString("G")), newMail != null, "Email sent", "No email sent");                
            });
            tasks.Add(task);
            Task.WaitAll(tasks.ToArray());

            Step("10. Refresh page and go to Report Manager app again");
            desktopPage = Browser.RefreshLoggedInCMS();
            reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.GeozoneTreeMainPanel.SelectNode(geozone);

            Step("11. Verify The following column in the report list is updated");
            Step(" o Last execution: the current datetime (format: yyyy-mm-dd hh:MM:ss). Ex: 2018-1-16 03:27:00");
            dt = reportManagerPage.GridPanel.BuildDataTableFromGrid();
            rows = dt.Select(string.Format("Name = '{0}'", reportName));
            if (rows.Any())
            {
                var expectedLastExecution = string.Format("{0} {1}:{2:D2}:00 {3}", currentDate.ToString("M/d/yy"), mailHour > 12 ? mailHour - 12 : mailHour, mailMinute, mailHour > 12 ? "PM" : "AM");
                var colLastExecution = rows[0]["Last execution"].ToString();
                var actualLastExecution = colLastExecution.Substring(0, colLastExecution.Length - 4);
                VerifyEqual(string.Format("11. Verify Last execution: the current datetime is '{0}'", expectedLastExecution), expectedLastExecution, actualLastExecution);
            }
            else
                Warning(string.Format("11. There is no row with report name '{0}'", reportName));

            try
            {
                Info("Delete report after testing");
                reportManagerPage.DeleteReport(reportName);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("RM_28 Report UI-France (covered SC-334)")]
        public void RM_28()
        {
            var userModel = CreateNewProfileAndUser(language: "fr_FR");
            var testData = GetTestDataOfTestRM_28();
            var geozone = testData["Geozone"].ToString();
            var reportData = testData["ReportData"] as XmlNode;
            var mail = testData["Mail"] as XmlNode;
            var reportType = reportData.GetAttrVal("ReportType");
            var reportName = SLVHelper.GenerateUniqueName(string.Format("[{0}][{1}][{2}] {3}", Browser.Name, userModel.Username, TestContext.CurrentContext.Test.MethodName, reportType));
            var mailSubject = string.Format("[{0}][{1}] {2}", userModel.Username, TestContext.CurrentContext.Test.MethodName, reportType);
            var reportDetails = reportData.GetAttrVal("ReportDetails");
            var filteringMode = reportData.GetAttrVal("FilteringMode");
            var periodicity = reportData.GetAttrVal("Periodicity");
            var timezone = reportData.GetAttrVal("Timezone");
            var fullGeozone = string.Format("{0} ({1})", geozone.GetChildName(), geozone.GetParentName());

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(userModel.Username, userModel.Password);
            desktopPage.InstallAppsIfNotExist(SLVHelper.ConvertAppName(App.ReportManager, "French"));

            Step("1. Go to Gestionnaire de rapports (Report Manager) app");
            Step("2. Verify Gestionnaire de rapports page is routed and loaded successfully");
            var reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;

            Step("3. Create a new report with a report type 'Rapport de pannes HTML'");
            Step("4. Take note the name, geozone, type, schedule (Hour, Minute, Periodicity, Timezone)");
            reportManagerPage.GeozoneTreeMainPanel.SelectNode(geozone);
            var currentDate = Settings.GetServerTime();
            var runDate = currentDate.AddMinutes(REPORT_WAIT_MINUTES);
            var mailHour = runDate.Hour;
            var mailMinute = runDate.Minute;

            var expectedSchedule = string.Format("{0} à {1:D2}h{2:D2} [{3}]", periodicity, mailHour, mailMinute, timezone.SplitAndGetAt(new string[] { "[" }, 0));
            var expectedNextExecution = string.Format("{0} {1:D2}:{2:D2}:00 UTC", currentDate.ToString("dd/MM/yy"), mailHour, mailMinute);

            EmailUtility.CleanInbox(mailSubject);
            reportManagerPage.GridPanel.ClickAddReportToolbarButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.ReportEditorPanel.WaitForPanelLoaded();
            reportManagerPage.ReportEditorPanel.EnterNameInput(reportName);
            reportManagerPage.ReportEditorPanel.SelectTypeDropDown(reportType);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.ReportEditorPanel.SelectTab("Propriétés");
            reportManagerPage.ReportEditorPanel.EnterDescriptionInput(string.Format("Automated {0} Description", reportType));
            reportManagerPage.ReportEditorPanel.SelectReportDetailsDropDown(reportDetails);
            reportManagerPage.ReportEditorPanel.SelectFilteringModeDropDown(filteringMode);
            reportManagerPage.ReportEditorPanel.SelectTab("Scheduler");            
            reportManagerPage.ReportEditorPanel.SelectHourDropDown(string.Format("{0:D2}", mailHour));
            reportManagerPage.ReportEditorPanel.SelectMinuteDropDown(string.Format("{0:D2}", mailMinute));
            reportManagerPage.ReportEditorPanel.SelectPeriodicityDropDown(periodicity);
            reportManagerPage.ReportEditorPanel.SelectTimezoneDropDown(timezone);
            reportManagerPage.ReportEditorPanel.ClickSaveButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisappeared();

            Step("5. Verify The new row is added to the report list with the following columns");
            Step(" o Nom: the report's name");
            Step(" o GeoZone: the report's geozone (the parent's geozone). Ex: Equipment Inventory (Automation)");
            Step(" o Modèle: the report's type. Ex: 'Rapport de pannes HTML'");
            Step(" o Programmation: {Periodicity} à {Hour}h{Minute} {[Timezone]}. Ex: Chaque jour à 03h20 [Temps universel coordonné]");
            Step(" o Dernière exécution: empty (because not run yet)");
            Step(" o Prochaine exécution: calculated by schedule's periodicity.");
            Step("  + Ex: if Periodicity: Every day > Next execution = the next datetime (format: dd/mm/yy hh:MM:ss Timezone). Ex: 24/01/18 03:27:00 UTC");
            Step("  + Ex: if Periodicity: Every Monday > Next execution = the next Monday (format: dd/mm/yy hh:MM:ss Timezone). Ex: 24/01/18 03:27:00 UTC");
            var dt = reportManagerPage.GridPanel.BuildDataTableFromGrid();
            var rows = dt.Select(string.Format("Nom = '{0}'", reportName));
            if (rows.Any())
            {
                var colName = rows[0]["Nom"].ToString();
                var colGeozone = rows[0]["GeoZone"].ToString();
                var colTemplate = rows[0]["Modèle"].ToString();
                var colSchedule = rows[0]["Programmation"].ToString();
                var colLastExecution = rows[0]["Dernière exécution"].ToString();
                var colNextExecution = rows[0]["Prochaine exécution"].ToString();

                VerifyEqual(string.Format("5. Verify Nom: the report's name is '{0}'", reportName), reportName, colName);
                VerifyEqual(string.Format("5. Verify GeoZone: the report's geozone (the parent's geozone) is '{0}'", fullGeozone), fullGeozone, colGeozone);
                VerifyEqual(string.Format("[#1429532] 5. Verify Modèle: the report's type is '{0}'", reportType), reportType, colTemplate);
                VerifyEqual(string.Format("5. Verify Programmation: [Periodicity] à [Hour]h[Minute] [Timezone] is '{0}'", expectedSchedule), expectedSchedule, colSchedule);
                VerifyEqual("[#1429527] 5. Verify Dernière exécution: empty (because not run yet)", "", colLastExecution);
                VerifyEqual(string.Format("5. Verify Prochaine exécution: calculated by schedule's periodicity is '{0}'", expectedNextExecution), expectedNextExecution, colNextExecution);
            }
            else
                Warning(string.Format("5. There is no row with report name '{0}'", reportName));

            Step("6. Press the report row on the list to open the Report Editor panel");
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();
            reportManagerPage.ReportEditorPanel.SelectTab("Scheduler");

            Step("7. Update the scheduler values such as Hour, Minute, Periodicity, Timezone and save changes");            
            var newHour = mailHour >= 23 ? mailHour - 1 : mailHour + 1;
            var newMinute = mailMinute >= 59 ? mailMinute - 1 : mailMinute + 1;
            var periodicityList = reportManagerPage.ReportEditorPanel.GetListOfPeriodicity();
            periodicityList.Remove(periodicity);
            var newPeriodicity = periodicityList.PickRandom();
            var weekday = newPeriodicity.SplitAndGetAt(new char[] { ' ' }, 1);
            var nextDate = Settings.GetNextWeekday(currentDate, SLVHelper.ConvertWeekdayNameToEnglish(weekday, "French"));
            expectedSchedule = string.Format("{0} à {1:D2}h{2:D2} [{3}]", newPeriodicity, newHour, newMinute, timezone.SplitAndGetAt(new string[] { "[" }, 0));
            expectedNextExecution = string.Format("{0} {1:D2}:{2:D2}:00 UTC", nextDate.ToString("dd/MM/yy"), newHour, newMinute);

            reportManagerPage.ReportEditorPanel.SelectPeriodicityDropDown(newPeriodicity);
            reportManagerPage.ReportEditorPanel.SelectHourDropDown(string.Format("{0:D2}", newHour));
            reportManagerPage.ReportEditorPanel.SelectMinuteDropDown(string.Format("{0:D2}", newMinute));
            reportManagerPage.ReportEditorPanel.ClickSaveButton();
            reportManagerPage.WaitForPreviousActionComplete();
            if (reportManagerPage.IsPopupDialogDisplayed() && reportManagerPage.Dialog.GetMessageText().Equals("Ce nom existe déjà !"))
            {
                Warning("#1437477 - 'Report already exists' message shown after editing and saving a report");
                return;
            }
            else
                reportManagerPage.WaitForReportDetailsDisappeared();

            Step("8. Verify the row in the report list is updated the following columns");
            Step(" o Programmation: {Periodicity} à {Hour}:{Minute} {[Timezone]}");
            Step(" o Dernière exécution: still empty (because not run yet)");
            Step(" o Prochaine exécution: calculated by schedule's periodicity.");
            dt = reportManagerPage.GridPanel.BuildDataTableFromGrid();
            rows = dt.Select(string.Format("Nom = '{0}'", reportName));
            if (rows.Any())
            {
                var colSchedule = rows[0]["Programmation"].ToString();
                var colLastExecution = rows[0]["Dernière exécution"].ToString();
                var colNextExecution = rows[0]["Prochaine exécution"].ToString();

                VerifyEqual(string.Format("8. Verify Programmation: [Periodicity] à [Hour]h[Minute] [Timezone] is '{0}'", expectedSchedule), expectedSchedule, colSchedule);
                VerifyEqual("8. Verify Dernière exécution: still empty (because not run yet)", "", colLastExecution);
                VerifyEqual(string.Format("8. Verify  Prochaine exécution: calculated by schedule's periodicity is '{0}'", expectedNextExecution), expectedNextExecution, colNextExecution);
            }
            else
                Warning(string.Format("8. There is no row with report name '{0}'", reportName));

            Step("9. Update the scheduler to make the report run a few minutes after and wait for report run successfully");
            currentDate = Settings.GetServerTime();
            runDate = currentDate.AddMinutes(REPORT_WAIT_MINUTES);
            mailHour = runDate.Hour;
            mailMinute = runDate.Minute;
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();
            reportManagerPage.ReportEditorPanel.SelectTab("Scheduler");            
            reportManagerPage.ReportEditorPanel.SelectHourDropDown(string.Format("{0:D2}", mailHour));
            reportManagerPage.ReportEditorPanel.SelectMinuteDropDown(string.Format("{0:D2}", mailMinute));
            reportManagerPage.ReportEditorPanel.SelectPeriodicityDropDown(periodicity);
            reportManagerPage.ReportEditorPanel.SelectTimezoneDropDown(timezone);
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            reportManagerPage.ReportEditorPanel.EnterSubjectInput(mailSubject);
            reportManagerPage.ReportEditorPanel.EnterFromInput(mail.GetAttrVal("From"));
            reportManagerPage.ReportEditorPanel.SelectContactsListDropDown(mail.GetAttrVal("Contacts"));
            reportManagerPage.ReportEditorPanel.ClickSaveButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisappeared();

            var tasks = new List<Task>();
            var task = Task.Run(() =>
            {
                var newMail = EmailUtility.GetNewEmail(mailSubject);
                VerifyTrue(string.Format("10. Verify Report '{0}' has an email sent from '{1}' (Report created: {2}, Expected email revieved: {3})", reportName, mailSubject, Settings.GetServerTime().ToString("G"), currentDate.AddMinutes(REPORT_WAIT_MINUTES).ToString("G")), newMail != null, "Email sent", "No email sent");
            });
            tasks.Add(task);
            Task.WaitAll(tasks.ToArray());

            Step("10. Refresh page and go to Gestionnaire de rapports (Report Manager) app");
            desktopPage = Browser.RefreshLoggedInCMS();
            reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.GeozoneTreeMainPanel.SelectNode(geozone);

            Step("11. Verify The following column in the report list is updated");
            Step(" o Dernière exécution: the current datetime (format: dd/mm/yy hh:MM:ss). Ex: 24/01/18 03:27:00 UTC");
            dt = reportManagerPage.GridPanel.BuildDataTableFromGrid();
            rows = dt.Select(string.Format("Nom = '{0}'", reportName));
            if (rows.Any())
            {
                var expectedLastExecution = string.Format("{0} {1:D2}:{2:D2}:00 UTC", currentDate.ToString("dd/MM/yy"), mailHour, mailMinute);
                var colLastExecution = rows[0]["Dernière exécution"].ToString();
                var actualLastExecution = colLastExecution;
                VerifyEqual(string.Format("11. Verify Dernière exécution: the current datetime is '{0}'", expectedLastExecution), expectedLastExecution, colLastExecution);
            }
            else
                Warning(string.Format("11. There is no row with report name '{0}'", reportName));

            try
            {
                Info("Delete report after testing");
                reportManagerPage.DeleteReport(reportName);
                DeleteUserAndProfile(userModel);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("RM_29 Generic device values [Run Once] (covered SC-334)")]
        public void RM_29()
        {
            var testData = GetTestDataOfTestRM_29();
            var geozone = testData["Geozone"].ToString();
            var reportData = testData["ReportData"] as XmlNode;
            var mail = testData["Mail"] as XmlNode;
            var reportType = reportData.GetAttrVal("ReportType");
            var reportName = SLVHelper.GenerateUniqueName(string.Format("[{0}][{1}][{2}] {3}", Browser.Name, Settings.Users["DefaultTest"].Username, TestContext.CurrentContext.Test.MethodName, reportType));
            var mailSubject = string.Format("[{0}][{1}] {2}", Settings.Users["DefaultTest"].Username, TestContext.CurrentContext.Test.MethodName, reportType);
            var fullGeozone = !geozone.Equals(Settings.RootGeozoneName)? string.Format("{0} ({1})", geozone.GetChildName(), geozone.GetParentName()): Settings.RootGeozoneName;
            var currentDate = Settings.GetServerTime();

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.ReportManager);

            Step("1. Go to Report Manager app");
            Step("2. Verify Report Manager page is routed and loaded successfully");
            var reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;

            Step("3. Create a new report with a report type 'Generic device values [Run Once]'");
            Step("4. Take note the name, geozone, type");
            reportManagerPage.GeozoneTreeMainPanel.SelectNode(geozone);           
            EmailUtility.CleanInbox(mailSubject);
            reportManagerPage.GridPanel.ClickAddReportToolbarButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.ReportEditorPanel.WaitForPanelLoaded();
            reportManagerPage.ReportEditorPanel.EnterNameInput(reportName);
            reportManagerPage.ReportEditorPanel.SelectTypeDropDown(reportType);
            reportManagerPage.WaitForPreviousActionComplete();

            reportManagerPage.ReportEditorPanel.TickMailEnabledCheckbox(bool.Parse(reportData.GetAttrVal("MailEnabled")));
            reportManagerPage.ReportEditorPanel.SelectMailFormatDropDown(reportData.GetAttrVal("MailFormat"));
            reportManagerPage.ReportEditorPanel.TickReportMailBodyCheckbox(bool.Parse(reportData.GetAttrVal("ReportMailBody")));
            reportManagerPage.ReportEditorPanel.EnterSubjectInput(mailSubject);
            reportManagerPage.ReportEditorPanel.EnterFromInput(mail.GetAttrVal("From"));
            reportManagerPage.ReportEditorPanel.SelectContactsListDropDown(mail.GetAttrVal("Contacts"));

            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            reportManagerPage.ReportEditorPanel.SelectAllDeviceCategoriesDropDownList();
            reportManagerPage.ReportEditorPanel.SelectAllMeaningsDropDownList();
            reportManagerPage.ReportEditorPanel.EnterStartDayDateInput(currentDate);
            reportManagerPage.ReportEditorPanel.EnterEndDayDateInput(currentDate);   

            reportManagerPage.ReportEditorPanel.ClickSaveButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisappeared();

            var runDateTime = Settings.GetServerTime().AddMinutes(5);
            var expectedSchedule = string.Format("Once at {0}", runDateTime.ToString("M/d/yy h:mm"));
            var expectedNextExecution = runDateTime.ToString("M/d/yy h:mm");

            Step("5. Verify The new row is added to the report list with the following columns");
            Step(" o Name: the report's name");
            Step(" o GeoZone: the report's geozone (the parent's geozone). Ex: Equipment Inventory (Automation)");
            Step(" o Template: 'Generic device values [Run Once]'");
            Step(" o Schedule: 'Once at', current UTC time (M/d/yy h:mm:ss) + 5 minutes. Ex: Once at 1/24/18 3:52:35. Note: '5 minutes' means this report will be run after created 5 minutes");
            Step(" o Last execution: empty (because not run yet)");
            Step(" o Next execution: current UTC time (M/d/yy h:mm:ss) + 5 minutes + AM(or PM) +[timezone]. Ex: 1/24/18 4:19:27 AM UTC.");

            var dt = reportManagerPage.GridPanel.BuildDataTableFromGrid();
            var rows = dt.Select(string.Format("Name = '{0}'", reportName));
            if (rows.Any())
            {
                var colName = rows[0]["Name"].ToString();
                var colGeozone = rows[0]["GeoZone"].ToString();
                var colTemplate = rows[0]["Template"].ToString();
                var colSchedule = rows[0]["Schedule"].ToString();
                var colLastExecution = rows[0]["Last execution"].ToString();
                var colNextExecution = rows[0]["Next execution"].ToString();                

                VerifyEqual(string.Format("5. Verify Name: the report's name is '{0}'", reportName), reportName, colName);
                VerifyEqual(string.Format("5. Verify GeoZone: the report's geozone (the parent's geozone) is '{0}'", fullGeozone), fullGeozone, colGeozone);
                VerifyEqual(string.Format("[#1429532] 5. Verify Template: the report's type is '{0}'", reportType), reportType, colTemplate);                
                VerifyTrue("5. Verify Schedule: 'Once at', current UTC time (M/d/yy h:mm:ss) + 5 minutes. Ex: Once at 1/24/18 3:52:35", colSchedule.Contains(expectedSchedule), expectedSchedule, colSchedule);
                VerifyEqual("[#1429527] 5. Verify Last execution: empty (because not run yet)", "", colLastExecution);                
                VerifyTrue("5. Verify Next execution: current UTC time (M/d/yy h:mm:ss) + 5 minutes + AM(or PM) +[timezone]. Ex: 1/24/18 4:19:27 AM UTC.", colNextExecution.Contains(expectedNextExecution), expectedNextExecution, colNextExecution);
            }
            else
                Warning(string.Format("5. There is no row with report name '{0}'", reportName));

            var tasks = new List<Task>();
            var task = Task.Run(() =>
            {
                var newMail = EmailUtility.GetNewEmail(mailSubject);                
                VerifyTrue(string.Format("5. Verify Report '{0}' has an email sent from '{1}' (Report created: {2}, Expected email revieved: {3})", reportName, mailSubject, Settings.GetServerTime().ToString("G"), currentDate.AddMinutes(REPORT_WAIT_MINUTES).ToString("G")), newMail != null, "Email sent", "No email sent");                
            });
            tasks.Add(task);
            Task.WaitAll(tasks.ToArray());

            Step("6. Wait for report run successfully and refresh the page");
            desktopPage = Browser.RefreshLoggedInCMS();
            reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.GeozoneTreeMainPanel.SelectNode(geozone);

            Step("7. Verify The following column in the report list is updated");
            Step(" o Last execution: current UTC time (mm/dd/yy h:mm:ss) + 5 minutes + AM(or PM) +[timezone]. Ex: 1/24/18 4:19:27 AM UTC");
            Step(" o Next execution: emtpy");
            dt = reportManagerPage.GridPanel.BuildDataTableFromGrid();
            rows = dt.Select(string.Format("Name = '{0}'", reportName));
            if (rows.Any())
            {                
                var colLastExecution = rows[0]["Last execution"].ToString();
                var colNextExecution = rows[0]["Next execution"].ToString();
                                
                VerifyTrue("7. Verify Last execution: current UTC time (mm/dd/yy h:mm:ss) + 5 minutes + AM(or PM) +[timezone]. Ex: 1/24/18 4:19:27 AM UTC", colLastExecution.Contains(expectedNextExecution), expectedNextExecution, colLastExecution);
                VerifyEqual("7. Verify Next execution: empty", "", colNextExecution);
            }
            else
                Warning(string.Format("7. There is no row with report name '{0}'", reportName));

            try
            {
                Info("Delete report after testing");
                reportManagerPage.DeleteReport(reportName);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("RM_30 Generic device values [Run Once]-France (covered SC-334)")]
        public void RM_30()
        {
            var userModel = CreateNewProfileAndUser(language: "fr_FR");
            var testData = GetTestDataOfTestRM_30();
            var geozone = testData["Geozone"].ToString();
            var reportData = testData["ReportData"] as XmlNode;
            var mail = testData["Mail"] as XmlNode;
            var reportType = reportData.GetAttrVal("ReportType");
            var reportName = SLVHelper.GenerateUniqueName(string.Format("[{0}][{1}][{2}] {3}", Browser.Name, userModel.Username, TestContext.CurrentContext.Test.MethodName, reportType));
            var mailSubject = string.Format("[{0}][{1}] {2}", userModel.Username, TestContext.CurrentContext.Test.MethodName, reportType);
            var fullGeozone = !geozone.Equals(Settings.RootGeozoneName) ? string.Format("{0} ({1})", geozone.GetChildName(), geozone.GetParentName()) : Settings.RootGeozoneName;
            var currentDate = Settings.GetServerTime();

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");
           
            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(userModel.Username, userModel.Password);
            desktopPage.InstallAppsIfNotExist(SLVHelper.ConvertAppName(App.ReportManager, "French"));

            Step("1. Go to Gestionnaire de rapports (Report Manager) app");
            Step("2. Verify Gestionnaire de rapports page is routed and loaded successfully");
            var reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;

            Step("3. Create a new report with a report type 'Rapport générique historique variables (Exécution unique)'");
            Step("4. Take note the name, geozone, type");
            reportManagerPage.GeozoneTreeMainPanel.SelectNode(geozone);
            EmailUtility.CleanInbox(mailSubject);
            reportManagerPage.GridPanel.ClickAddReportToolbarButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.ReportEditorPanel.WaitForPanelLoaded();
            reportManagerPage.ReportEditorPanel.EnterNameInput(reportName);
            reportManagerPage.ReportEditorPanel.SelectTypeDropDown(reportType);
            reportManagerPage.WaitForPreviousActionComplete();

            reportManagerPage.ReportEditorPanel.TickMailEnabledCheckbox(bool.Parse(reportData.GetAttrVal("MailEnabled")));
            reportManagerPage.ReportEditorPanel.SelectMailFormatDropDown(reportData.GetAttrVal("MailFormat"));
            reportManagerPage.ReportEditorPanel.TickReportMailBodyCheckbox(bool.Parse(reportData.GetAttrVal("ReportMailBody")));
            reportManagerPage.ReportEditorPanel.EnterSubjectInput(mailSubject);
            reportManagerPage.ReportEditorPanel.EnterFromInput(mail.GetAttrVal("From"));
            reportManagerPage.ReportEditorPanel.SelectContactsListDropDown(mail.GetAttrVal("Contacts"));

            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            reportManagerPage.ReportEditorPanel.SelectAllDeviceCategoriesDropDownList();
            reportManagerPage.ReportEditorPanel.SelectAllMeaningsDropDownList();
            reportManagerPage.ReportEditorPanel.EnterStartDayDateInput(currentDate);
            reportManagerPage.ReportEditorPanel.EnterEndDayDateInput(currentDate);          

            reportManagerPage.ReportEditorPanel.ClickSaveButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisappeared();

            var runDateTime = Settings.GetServerTime().AddMinutes(5);
            var expectedSchedule = string.Format("Une fois, {0}", runDateTime.ToString("dd/MM/yy HH:mm"));
            var expectedNextExecution = runDateTime.ToString("dd/MM/yy HH:mm");

            Step("5. Verify The new row is added to the report list with the following columns");
            Step(" o Nom: the report's name");
            Step(" o GeoZone: the report's geozone (the parent's geozone). Ex: Equipment Inventory (Automation)");
            Step(" o Modèle: 'Rapport générique historique variables (Exécution unique)'");
            Step(" o Programmation: 'Une fois', current UTC time (dd/MM/yy HH:mm:ss) + 5 minutes. Ex: Une fois, 24/01/18 03:52:35. This report will be run after created 5 minutes");
            Step(" o Dernière exécution: empty (because not run yet)");
            Step(" o Prochaine exécution: current UTC time (dd/MM/yy HH:mm:ss) + 5 minutes + [timezone]. Ex: 24/01/18 03:52:35 UTC");
            var dt = reportManagerPage.GridPanel.BuildDataTableFromGrid();
            var rows = dt.Select(string.Format("Nom = '{0}'", reportName));
            if (rows.Any())
            {
                var colName = rows[0]["Nom"].ToString();
                var colGeozone = rows[0]["GeoZone"].ToString();
                var colTemplate = rows[0]["Modèle"].ToString();
                var colSchedule = rows[0]["Programmation"].ToString();
                var colLastExecution = rows[0]["Dernière exécution"].ToString();
                var colNextExecution = rows[0]["Prochaine exécution"].ToString();
                
                VerifyEqual(string.Format("5. Verify Nom: the report's name is '{0}'", reportName), reportName, colName);
                VerifyEqual(string.Format("5. Verify GeoZone: the report's geozone (the parent's geozone) is '{0}'", fullGeozone), fullGeozone, colGeozone);
                VerifyEqual(string.Format("[#1429532] 5. Verify Modèle: the report's type is '{0}'", reportType), reportType, colTemplate);
                VerifyTrue("5. Verify Programmation: 'Une fois', current UTC time (dd/MM/yy HH:mm:ss) + 5 minutes", colSchedule.Contains(expectedSchedule), expectedSchedule, colSchedule);
                VerifyEqual("[#1429527] 5. Verify Dernière exécution: empty (because not run yet)", "", colLastExecution);
                VerifyTrue("5. Verify  Prochaine exécution: current UTC time (dd/MM/yy HH:mm:ss) + 5 minutes + [timezone]", colNextExecution.Contains(expectedNextExecution), expectedNextExecution, colNextExecution);
            }
            else
                Warning(string.Format("5. There is no row with report name '{0}'", reportName));

            var tasks = new List<Task>();
            var task = Task.Run(() =>
            {
                var newMail = EmailUtility.GetNewEmail(mailSubject);
                VerifyTrue(string.Format("5. Verify Report '{0}' has an email sent from '{1}' (Report created: {2}, Expected email revieved: {3})", reportName, mailSubject, Settings.GetServerTime().ToString("G"), currentDate.AddMinutes(REPORT_WAIT_MINUTES).ToString("G")), newMail != null, "Email sent", "No email sent");
            });
            tasks.Add(task);
            Task.WaitAll(tasks.ToArray());

            Step("6. Wait for report run successfully and refresh the page");
            desktopPage = Browser.RefreshLoggedInCMS();
            reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.GeozoneTreeMainPanel.SelectNode(geozone);

            Step("7. Verify The following column in the report list is updated");
            Step(" o Dernière exécution: current UTC time (dd/MM/yy HH:mm:ss) + 5 minutes + [timezone]. Ex: 24/01/18 03:52:35 UTC");
            Step(" o Prochaine exécution: emtpy");
            dt = reportManagerPage.GridPanel.BuildDataTableFromGrid();
            rows = dt.Select(string.Format("Nom = '{0}'", reportName));
            if (rows.Any())
            {
                var colLastExecution = rows[0]["Dernière exécution"].ToString();
                var colNextExecution = rows[0]["Prochaine exécution"].ToString();

                VerifyTrue("7. Verify Dernière exécution: current UTC time (dd/MM/yy HH:mm:ss) + 5 minutes + [timezone]. Ex: 24/01/18 03:52:35 UTC", colLastExecution.Contains(expectedNextExecution), expectedNextExecution, colLastExecution);
                VerifyEqual("7. Verify Prochaine exécution: empty", "", colNextExecution);
            }
            else
                Warning(string.Format("7. There is no row with report name '{0}'", reportName));

            try
            {
                Info("Delete report after testing");                
                reportManagerPage.DeleteReport(reportName);
                DeleteUserAndProfile(userModel);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("RM_31 SC-1084 - Healthy devices are marked as 'false' in Failures HTML reports")]
        public void RM_31()
        {
            var testData = GetTestDataOfTestRM_31();
            var geozonePath = testData["Geozone"].ToString();
            var geozoneName = geozonePath.GetChildName();
            var controllerId = testData["ControllerId"].ToString();
            var controllerName = testData["ControllerName"].ToString();
            var reportData = testData["ReportData"] as XmlNode;
            var mail = testData["Mail"] as XmlNode;
            var reportType = reportData.GetAttrVal("ReportType");
            var reportDetails = reportData.GetAttrVal("ReportDetails");
            var filteringMode = reportData.GetAttrVal("FilteringMode");
            var periodicity = reportData.GetAttrVal("Periodicity");
            var timezone = Settings.DEFAULT_TIMEZONE;

            var reportName = SLVHelper.GenerateUniqueName(string.Format("[{0}][{1}][{2}] {3}", Browser.Name, Settings.Users["DefaultTest"].Username, TestContext.CurrentContext.Test.MethodName, reportType));
            var reportName1 = reportName + "-PlainText";
            var reportName2 = reportName + "-Html";
            var mailSubject = string.Format("[{0}][{1}] {2}", Settings.Users["DefaultTest"].Username, TestContext.CurrentContext.Test.MethodName, reportType);
            var mailSubject1 = mailSubject + "-PlainText";
            var mailSubject2 = mailSubject + "-Html";
            var fullGeozone = !geozonePath.Equals(Settings.RootGeozoneName) ? string.Format("{0} ({1})", geozonePath.GetChildName(), geozonePath.GetParentName()) : Settings.RootGeozoneName;
            var updatedMailSubject = mailSubject + "(Updated)";
            var updatedMailSubject1 = updatedMailSubject + "-PlainText";
            var updatedMailSubject2 = updatedMailSubject + "-Html";
            var newGeozoneName = SLVHelper.GenerateUniqueName("GZNRM31");
            var streetlight = SLVHelper.GenerateUniqueName("STL");

            Step("-> Create data for testing");            
            CreateNewGeozone(newGeozoneName, geozoneName);
            CreateNewDevice(DeviceType.Streetlight, streetlight, controllerId, newGeozoneName);

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - Create a new streetlight in a new geozone to specify the report");
            Step("**** Precondition ****\n");            

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.ReportManager);

            Step("1. Go to Report Manager app");
            Step("2. Verify Report Manager page is routed and loaded successfully");
            var reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;

            Step("3. Create 2 new reports with a report type 'Failures HTML report' and");
            Step("  o Name: random");
            Step("  o Property tab:");
            Step("   + Description: random");
            Step("  o Schedule tab:");
            Step("   + set the time about 5 minutes later to make sure sending simulated command already.");
            Step("   + Periodicity: Every day");
            Step("  o Export tab:");
            Step("   + Mail: set the Subject, From, Contacts");
            Step("   + The 1st report . Configuration: HTML format (unchecked -> to create a plain text email)");
            Step("   + The 2nd report . Configuration: HTML format (checked -> to create a HTML-format email)");
            reportManagerPage.GeozoneTreeMainPanel.SelectNode(geozonePath + @"\" + newGeozoneName);
            var currentDate = Settings.GetServerTime();
            var runDate = currentDate.AddMinutes(REPORT_WAIT_MINUTES);            
            EmailUtility.CleanInbox(mailSubject);
            //Create report 1
            reportManagerPage.GridPanel.ClickAddReportToolbarButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.ReportEditorPanel.WaitForPanelLoaded();
            reportManagerPage.ReportEditorPanel.EnterNameInput(reportName1);
            reportManagerPage.ReportEditorPanel.SelectTypeDropDown(reportType);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.ReportEditorPanel.SelectTab("Properties");
            reportManagerPage.ReportEditorPanel.EnterDescriptionInput(string.Format("Automated {0} Description for report 1", reportType));
            reportManagerPage.ReportEditorPanel.SelectReportDetailsDropDown(reportDetails);
            reportManagerPage.ReportEditorPanel.SelectFilteringModeDropDown(filteringMode);
            reportManagerPage.ReportEditorPanel.SelectTab("Scheduler");
            reportManagerPage.ReportEditorPanel.SelectPeriodicityDropDown(periodicity);
            reportManagerPage.ReportEditorPanel.SelectHourDropDown(string.Format("{0:D2}", runDate.Hour));
            reportManagerPage.ReportEditorPanel.SelectMinuteDropDown(string.Format("{0:D2}", runDate.Minute));
            reportManagerPage.ReportEditorPanel.SelectTimezoneDropDown(timezone);
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            reportManagerPage.ReportEditorPanel.EnterSubjectInput(mailSubject1);
            reportManagerPage.ReportEditorPanel.EnterFromInput(mail.GetAttrVal("From"));
            reportManagerPage.ReportEditorPanel.SelectContactsListDropDown(mail.GetAttrVal("Contacts"));
            reportManagerPage.ReportEditorPanel.ClickSaveButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisappeared();

            //Create report 2
            reportManagerPage.GridPanel.ClickAddReportToolbarButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.ReportEditorPanel.WaitForPanelLoaded();
            reportManagerPage.ReportEditorPanel.EnterNameInput(reportName2);
            reportManagerPage.ReportEditorPanel.SelectTypeDropDown(reportType);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.ReportEditorPanel.SelectTab("Properties");
            reportManagerPage.ReportEditorPanel.EnterDescriptionInput(string.Format("Automated {0} Description for report 2", reportType));
            reportManagerPage.ReportEditorPanel.SelectReportDetailsDropDown(reportDetails);
            reportManagerPage.ReportEditorPanel.SelectFilteringModeDropDown(filteringMode);
            reportManagerPage.ReportEditorPanel.SelectTab("Scheduler");
            reportManagerPage.ReportEditorPanel.SelectPeriodicityDropDown(periodicity);
            reportManagerPage.ReportEditorPanel.SelectHourDropDown(string.Format("{0:D2}", runDate.Hour));
            reportManagerPage.ReportEditorPanel.SelectMinuteDropDown(string.Format("{0:D2}", runDate.Minute));
            reportManagerPage.ReportEditorPanel.SelectTimezoneDropDown(timezone);
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            reportManagerPage.ReportEditorPanel.EnterSubjectInput(mailSubject2);
            reportManagerPage.ReportEditorPanel.EnterFromInput(mail.GetAttrVal("From"));
            reportManagerPage.ReportEditorPanel.SelectContactsListDropDown(mail.GetAttrVal("Contacts"));
            reportManagerPage.ReportEditorPanel.TickHtmlFormatCheckbox(true);
            reportManagerPage.ReportEditorPanel.ClickSaveButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisappeared();

            Step("4. Verify The created report displays in the list");
            VerifyEqual(string.Format("4. Verify {0} displays in the list", reportName1), true, reportManagerPage.GridPanel.IsColumnHasTextPresent("Name", reportName1));
            VerifyEqual(string.Format("4. Verify {0} displays in the list", reportName2), true, reportManagerPage.GridPanel.IsColumnHasTextPresent("Name", reportName2));
            
            Step("5. Send the simulated command to set Lamp Failure=true");
            var request = SetValueToDevice(controllerId, streetlight, "LampFailure", "true", Settings.GetServerTime());            

            Step("6. Verify The simulated command sent successfully.");
            VerifyEqual(string.Format("6. Verify the request is sent successfully (attribute: {0}, value: {1})", "LampFailure", "true"), true, request);

            Step("7. Wait until the report has already run and check mails");
            Step("8. Verify The subject of the email containing the datetime when the report is created");
            Step("  o Format: Subject of email + (mm/dd/yy [Time]:[Minute] AM/PM)");
            Step("  o Ex: dtran_sc_1084_FailuresHTMLReport_htlmFormat (1/25/18 3:45 AM)");
            Step("  o Note: this is to cover SC-1040");
            Step("9. Verify");
            Step("  o In the body of the plain-text mail, the report displays");
            Step("   + Name of testing device:");
            Step("   + Make sure the following fields:");
            Step("   + Faulty: yes");
            Step("   + Critical faulty: yes");
            Step("   + Failures: Lamp failure");
            Step(" Ex: - DataHistorySL01:");
            Step("  Address: Unnamed Road");
            Step("  Faulty: yes");
            Step("  Critical faulty: yes");
            Step("  Failures: Communication failure / Lamp failure");
            Step("  o In the body of the HTML-format mail, the report displays in columns");
            Step("   + Device: Name of testing device");
            Step("   + Faulty: yes");
            Step("   + Critical faulty: yes");
            Step("   + Failures: Lamp failure");
            var reportsSentMail = new List<MailMessage>();
            var tasks = new List<Task>();
            var task1 = Task.Run(() =>
            {
                var newMail = EmailUtility.GetNewEmail(mailSubject1);
                var hasNewMail = newMail != null;
                VerifyTrue(string.Format("Verify Report '{0}' has an email sent from '{1}'", reportName1, mailSubject1), hasNewMail, "Email sent", "No email sent");
                if (hasNewMail)
                {
                    var expectedSubject = string.Format("{0} ({1})", mailSubject1, runDate.ToString("M/d/yy h:mm tt"));
                    VerifyEqual("8. Verify Format: Subject of email + (mm/dd/yy [Time]:[Minute] AM/PM)", expectedSubject, newMail.Subject);
                    VerifyEqual(string.Format("8. Verify body of '{0}' is plain text", reportName1), true, !newMail.IsBodyHtml);
                    VerifyEqual(string.Format("9. Verify In the body of the plain-text mail has Name of testing device is '{0}'", streetlight), true, newMail.Body.IndexOf(streetlight) > 0);
                    VerifyEqual("9. Verify In the body of the plain-text mail has field 'Faulty: yes'", true, newMail.Body.IndexOf("Faulty: yes") > 0);
                    VerifyEqual("[#1429535] 9. Verify In the body of the plain-text mai has field 'Critical faulty: yes'", true, newMail.Body.IndexOf("Critical faulty: yes") > 0);
                    VerifyEqual("9. Verify In the body of the plain-text mai has field 'Failures: Lamp failure'", true, newMail.Body.IndexOf("Failures: Lamp failure") > 0);
                }
            });
            tasks.Add(task1);
            var task2 = Task.Run(() =>
            {
                var newMail = EmailUtility.GetNewEmail(mailSubject2);
                var hasNewMail = newMail != null;
                VerifyTrue(string.Format("Verify Report '{0}' has an email sent from '{1}'", reportName2, mailSubject2), hasNewMail, "Email sent", "No email sent");
                if (hasNewMail)
                {
                    var expectedSubject = string.Format("{0} ({1})", mailSubject2, runDate.ToString("M/d/yy h:mm tt"));
                    VerifyEqual("8. Verify Format: Subject of email + (mm/dd/yy [Time]:[Minute] AM/PM)", expectedSubject, newMail.Subject);
                    VerifyEqual(string.Format("8. Verify body of '{0}' is html", reportName1), true, newMail.IsBodyHtml);

                    HtmlUtility htmlUtility = new HtmlUtility(newMail.Body);
                    var dt = htmlUtility.GetTable(@"html/body/table");
                    var rows = dt.Select(string.Format("Device = '{0}'", streetlight));
                    if (rows.Any())
                    {
                        VerifyEqual(string.Format("9. Verify In the body of the HTML-format mail has Name of testing device is '{0}'", streetlight), streetlight, rows[0]["Device"]);
                        VerifyEqual("9. Verify In the body of the HTML-format mail has field 'Faulty: yes'", "yes", rows[0]["Faulty"]);
                        VerifyEqual("[#1429535] 9. Verify In the body of the HTML-format mai has field 'Critical faulty: yes'", "yes", rows[0]["Critical faulty"]);
                        VerifyEqual("9. Verify In the body of the HTML-format mai has field 'Failures: Lamp failure'", "Lamp failure", rows[0]["Failures"]);
                    }
                    else
                    {
                        Warning(string.Format("9. There is no row with device name '{0}' in email", streetlight));
                    }
                }
            });
            tasks.Add(task2);
            Task.WaitAll(tasks.ToArray());

            Step("10. Send the simulated command to set Lamp Failure =false");
            request = SetValueToDevice(controllerId, streetlight, "LampFailure", "false", Settings.GetServerTime());
            
            Step("11. Verify The simulated command sent successfully.");
            VerifyEqual(string.Format("11. Verify the request is sent successfully (attribute: {0}, value: {1})", "LampFailure", "false"), true, request);

            Step("12. Update the time of the two reports to let it run a few minute later and check mails again");
            tasks.Clear();
            currentDate = Settings.GetServerTime();
            runDate = currentDate.AddMinutes(REPORT_WAIT_MINUTES);
            EmailUtility.CleanInbox(updatedMailSubject);
            //Update report 1
            reportManagerPage.GridPanel.ClickGridRecord(reportName1);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();
            reportManagerPage.ReportEditorPanel.SelectTab("Scheduler");
            reportManagerPage.ReportEditorPanel.SelectHourDropDown(string.Format("{0:D2}", runDate.Hour));
            reportManagerPage.ReportEditorPanel.SelectMinuteDropDown(string.Format("{0:D2}", runDate.Minute));
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            reportManagerPage.ReportEditorPanel.EnterSubjectInput(updatedMailSubject1);
            reportManagerPage.ReportEditorPanel.ClickSaveButton();
            reportManagerPage.WaitForPreviousActionComplete();
            if (reportManagerPage.IsPopupDialogDisplayed() && reportManagerPage.Dialog.GetMessageText().Equals("This report name already exists."))
            {
                Warning("#1437477 - 'Report already exists' message shown after editing and saving a report");
                return;
            }
            else
                reportManagerPage.WaitForReportDetailsDisappeared();

            //Update report 2
            reportManagerPage.GridPanel.ClickGridRecord(reportName2);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();
            reportManagerPage.ReportEditorPanel.SelectTab("Scheduler");
            reportManagerPage.ReportEditorPanel.SelectHourDropDown(string.Format("{0:D2}", runDate.Hour));
            reportManagerPage.ReportEditorPanel.SelectMinuteDropDown(string.Format("{0:D2}", runDate.Minute));
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            reportManagerPage.ReportEditorPanel.EnterSubjectInput(updatedMailSubject2);
            reportManagerPage.ReportEditorPanel.ClickSaveButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisappeared();

            Step("13. Verify");
            Step(" • In the body of the plain-text mail, the report displays");
            Step("  o Name of testing device:");
            Step("  o Make sure the following fields:");
            Step("   + Faulty: no");
            Step("   + Critical faulty: no");
            Step("   + Failures: empty");
            Step(" • In the body of the HTML-format mail, the report displays in columns");
            Step("  o Device: Name of testing device");
            Step("  o Faulty: no");
            Step("  o Critical faulty: no");
            Step("  o Failures: empty");

            task1 = Task.Run(() =>
            {
                var newMail = EmailUtility.GetNewEmail(updatedMailSubject1);
                var hasNewMail = newMail != null;
                VerifyTrue(string.Format("Verify Updated Report '{0}' has an email sent from '{1}'", reportName1, updatedMailSubject1), hasNewMail, "Email sent", "No email sent");
                if (hasNewMail)
                {
                    var expectedSubject = string.Format("{0} ({1})", updatedMailSubject1, runDate.ToString("M/d/yy h:mm tt"));
                    VerifyEqual("13. Verify Format: Subject of email + (mm/dd/yy [Time]:[Minute] AM/PM)", expectedSubject, newMail.Subject);
                    VerifyEqual(string.Format("13. Verify body of '{0}' is plain text", reportName1), true, !newMail.IsBodyHtml);
                    VerifyEqual(string.Format("13. Verify In the body of the plain-text mail has Name of testing device is '{0}'", streetlight), true, newMail.Body.IndexOf(streetlight) > 0);
                    VerifyEqual("13. Verify In the body of the plain-text mail has field 'Faulty: no'", true, newMail.Body.IndexOf("Faulty: no") > 0);
                    VerifyEqual("13. Verify In the body of the plain-text mai has field 'Critical faulty: no'", true, newMail.Body.IndexOf("Critical faulty: no") > 0);
                    VerifyEqual("13. Verify In the body of the plain-text mai has field 'Failures:'", true, newMail.Body.IndexOf("Failures:") > 0);
                }
            });
            tasks.Add(task1);
            task2 = Task.Run(() =>
            {
                var newMail = EmailUtility.GetNewEmail(updatedMailSubject2);
                var hasNewMail = newMail != null;
                VerifyTrue(string.Format("Verify Updated Report '{0}' has an email sent from '{1}'", reportName2, updatedMailSubject2), hasNewMail, "Email sent", "No email sent");
                if (hasNewMail)
                {
                    var expectedSubject = string.Format("{0} ({1})", updatedMailSubject2, runDate.ToString("M/d/yy h:mm tt"));
                    VerifyEqual("13. Verify Format: Subject of email + (mm/dd/yy [Time]:[Minute] AM/PM)", expectedSubject, newMail.Subject);
                    VerifyEqual(string.Format("13. Verify body of '{0}' is html", reportName1), true, newMail.IsBodyHtml);

                    HtmlUtility htmlUtility = new HtmlUtility(newMail.Body);
                    var dt = htmlUtility.GetTable(@"html/body/table");
                    var rows = dt.Select(string.Format("Device = '{0}'", streetlight));
                    if (rows.Any())
                    {
                        VerifyEqual(string.Format("13. Verify In the body of the HTML-format mail has Name of testing device is '{0}'", streetlight), streetlight, rows[0]["Device"]);
                        VerifyEqual("13. Verify In the body of the HTML-format mail has field 'Faulty: no'", "no", rows[0]["Faulty"]);
                        VerifyEqual("13. Verify In the body of the HTML-format mai has field 'Critical faulty: no'", "no", rows[0]["Critical faulty"]);
                        VerifyEqual("13. Verify In the body of the HTML-format mai has field 'Failures: empty'", "", rows[0]["Failures"]);
                    }
                    else
                    {
                        Warning(string.Format("9. There is no row with device name '{0}' in email", streetlight));
                    }
                }
            });
            tasks.Add(task2);
            Task.WaitAll(tasks.ToArray());

            try
            {
                Info("Delete reports after testing");
                reportManagerPage.DeleteReport(reportName1);
                reportManagerPage.DeleteReport(reportName2);
                DeleteGeozone(newGeozoneName);
            }
            catch { }            
        }

        [Test, DynamicRetry]
        [Description("RM_32 SC-1084 - Healthy devices are marked as 'false' in Failures HTML reports -France")]
        public void RM_32()
        {
            var userModel = CreateNewProfileAndUser(language: "fr_FR");
            var testData = GetTestDataOfTestRM_32();
            var geozonePath = testData["Geozone"].ToString();
            var geozoneName = geozonePath.GetChildName();
            var controllerId = testData["ControllerId"].ToString();
            var controllerName = testData["ControllerName"].ToString();
            var reportData = testData["ReportData"] as XmlNode;
            var mail = testData["Mail"] as XmlNode;
            var reportType = reportData.GetAttrVal("ReportType");
            var reportDetails = reportData.GetAttrVal("ReportDetails");
            var filteringMode = reportData.GetAttrVal("FilteringMode");
            var periodicity = reportData.GetAttrVal("Periodicity");
            var timezone = reportData.GetAttrVal("Timezone");
            var contact = string.Format("{0} {1}", userModel.Username, userModel.Email);

            var reportName = SLVHelper.GenerateUniqueName(string.Format("[{0}][{1}][{2}] {3}", Browser.Name, userModel.Username, TestContext.CurrentContext.Test.MethodName, reportType));
            var reportName1 = reportName + "-PlainText";
            var reportName2 = reportName + "-Html";
            var mailSubject = string.Format("[{0}][{1}] {2}", userModel.Username, TestContext.CurrentContext.Test.MethodName, reportType);
            var mailSubject1 = mailSubject + "-PlainText";
            var mailSubject2 = mailSubject + "-Html";
            var fullGeozone = !geozonePath.Equals(Settings.RootGeozoneName) ? string.Format("{0} ({1})", geozonePath.GetChildName(), geozonePath.GetParentName()) : Settings.RootGeozoneName;
            var updatedMailSubject = mailSubject + "(Updated)";
            var updatedMailSubject1 = updatedMailSubject + "-PlainText";
            var updatedMailSubject2 = updatedMailSubject + "-Html";
            var newGeozoneName = SLVHelper.GenerateUniqueName("GZNRM32");
            var streetlight = SLVHelper.GenerateUniqueName("STL");

            Step("-> Create data for testing");
            CreateNewGeozone(newGeozoneName, geozoneName);
            CreateNewDevice(DeviceType.Streetlight, streetlight, controllerId, newGeozoneName);

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - Create a new streetlight in a new geozone to specify the report");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(userModel.Username, userModel.Password);
            desktopPage.InstallAppsIfNotExist(SLVHelper.ConvertAppName(App.ReportManager, "French"));          

            Step("1. Go to Report Manager app");
            Step("2. Verify Report Manager page is routed and loaded successfully");
            var reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;

            Step("3. Create 2 new reports with a report type 'Failures HTML report' and");
            Step("  o Nom: random");
            Step("  o Propriétés tab:");
            Step("   + Description: random");
            Step("  o Schedule tab:");
            Step("   + set the time about 5 minutes later to make sure sending simulated command already.");
            Step("   + Periodicity: Chaque jour");
            Step("  o Export tab:");
            Step("   + Mail: set the Sujet, De, Contacts");
            Step("   + The 1st report . Configuration: HTML format (unchecked -> to create a plain text email)");
            Step("   + The 2nd report . Configuration: HTML format (checked -> to create a HTML-format email)");
            reportManagerPage.GeozoneTreeMainPanel.SelectNode(geozonePath + @"\" + newGeozoneName);
            var currentDate = Settings.GetServerTime();
            var runDate = currentDate.AddMinutes(REPORT_WAIT_MINUTES);
            EmailUtility.CleanInbox(mailSubject);
            //Create report 1
            reportManagerPage.GridPanel.ClickAddReportToolbarButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.ReportEditorPanel.WaitForPanelLoaded();
            reportManagerPage.ReportEditorPanel.EnterNameInput(reportName1);
            reportManagerPage.ReportEditorPanel.SelectTypeDropDown(reportType);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.ReportEditorPanel.SelectTab("Propriétés");
            reportManagerPage.ReportEditorPanel.EnterDescriptionInput(string.Format("Automated {0} Description for report 1", reportType));
            reportManagerPage.ReportEditorPanel.SelectReportDetailsDropDown(reportDetails);
            reportManagerPage.ReportEditorPanel.SelectFilteringModeDropDown(filteringMode);
            reportManagerPage.ReportEditorPanel.SelectTab("Scheduler");
            reportManagerPage.ReportEditorPanel.SelectPeriodicityDropDown(periodicity);
            reportManagerPage.ReportEditorPanel.SelectHourDropDown(string.Format("{0:D2}", runDate.Hour));
            reportManagerPage.ReportEditorPanel.SelectMinuteDropDown(string.Format("{0:D2}", runDate.Minute));
            reportManagerPage.ReportEditorPanel.SelectTimezoneDropDown(timezone);
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            reportManagerPage.ReportEditorPanel.EnterSubjectInput(mailSubject1);
            reportManagerPage.ReportEditorPanel.EnterFromInput(mail.GetAttrVal("From"));
            reportManagerPage.ReportEditorPanel.SelectContactsListDropDown(contact);
            reportManagerPage.ReportEditorPanel.ClickSaveButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisappeared();

            //Create report 2
            reportManagerPage.GridPanel.ClickAddReportToolbarButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.ReportEditorPanel.WaitForPanelLoaded();
            reportManagerPage.ReportEditorPanel.EnterNameInput(reportName2);
            reportManagerPage.ReportEditorPanel.SelectTypeDropDown(reportType);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.ReportEditorPanel.SelectTab("Properties");
            reportManagerPage.ReportEditorPanel.EnterDescriptionInput(string.Format("Automated {0} Description for report 2", reportType));
            reportManagerPage.ReportEditorPanel.SelectReportDetailsDropDown(reportDetails);
            reportManagerPage.ReportEditorPanel.SelectFilteringModeDropDown(filteringMode);
            reportManagerPage.ReportEditorPanel.SelectTab("Scheduler");
            reportManagerPage.ReportEditorPanel.SelectPeriodicityDropDown(periodicity);
            reportManagerPage.ReportEditorPanel.SelectHourDropDown(string.Format("{0:D2}", runDate.Hour));
            reportManagerPage.ReportEditorPanel.SelectMinuteDropDown(string.Format("{0:D2}", runDate.Minute));
            reportManagerPage.ReportEditorPanel.SelectTimezoneDropDown(timezone);
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            reportManagerPage.ReportEditorPanel.EnterSubjectInput(mailSubject2);
            reportManagerPage.ReportEditorPanel.EnterFromInput(mail.GetAttrVal("From"));
            reportManagerPage.ReportEditorPanel.SelectContactsListDropDown(contact);
            reportManagerPage.ReportEditorPanel.TickHtmlFormatCheckbox(true);
            reportManagerPage.ReportEditorPanel.ClickSaveButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisappeared();

            Step("4. Verify The created report displays in the list");
            VerifyEqual(string.Format("4. Verify {0} displays in the list", reportName1), true, reportManagerPage.GridPanel.IsColumnHasTextPresent("Nom", reportName1));
            VerifyEqual(string.Format("4. Verify {0} displays in the list", reportName2), true, reportManagerPage.GridPanel.IsColumnHasTextPresent("Nom", reportName2));

            Step("5. Send the simulated command to set Lamp Failure=true");
            var request = SetValueToDevice(controllerId, streetlight, "LampFailure", "true", Settings.GetServerTime());

            Step("6. Verify The simulated command sent successfully.");
            VerifyEqual(string.Format("6. Verify the request is sent successfully (attribute: {0}, value: {1})", "LampFailure", "true"), true, request);

            Step("7. Wait until the report has already run and check mails");
            Step("8. Verify The subject of the email containing the datetime when the report is created");
            Step("  o Format: Subject of email + (dd/MM/yy [Time(24h)]:[Minute]");
            Step("  o Ex: dtran_sc_1084_FailuresHTMLReport_htlmFormat (25/01/18 3:45)");
            Step("  o Note: this is to cover SC-1040");
            Step("9. Verify");
            Step("  o In the body of the plain-text mail, the report displays");
            Step("   + Name of testing device:");
            Step("   + Make sure the following fields:");
            Step("    + Défaut: oui");
            Step("    + Défaut critique: oui");
            Step("    + Pannes: Panne de lampe");
            Step(" Ex: - DataHistorySL01: Défaut: oui Défaut critique: oui Pannes: Panne de lampe");
            Step("  o In the body of the HTML-format mail, the report displays in columns");
            Step("   + Equipement: Name of testing device");
            Step("   + Défaut: oui");
            Step("   + Défaut critique: oui");
            Step("   + Pannes: Panne de lampe");
            var reportsSentMail = new List<MailMessage>();
            var tasks = new List<Task>();
            var task1 = Task.Run(() =>
            {
                var newMail = EmailUtility.GetNewEmail(mailSubject1);
                var hasNewMail = newMail != null;
                VerifyTrue(string.Format("Verify Report '{0}' has an email sent from '{1}'", reportName1, mailSubject1), hasNewMail, "Email sent", "No email sent");
                if (hasNewMail)
                {
                    var expectedSubject = string.Format("{0} ({1})", mailSubject1, runDate.ToString("dd/MM/yy HH:mm"));
                    VerifyEqual("8. Verify Format: Subject of email + (dd/MM/yy [Time(24h)]:[Minute])", expectedSubject, newMail.Subject);
                    VerifyEqual(string.Format("8. Verify body of '{0}' is plain text", reportName1), true, !newMail.IsBodyHtml);
                    VerifyEqual(string.Format("9. Verify In the body of the plain-text mail has Name of testing device is '{0}'", streetlight), true, newMail.Body.IndexOf(streetlight) > 0);
                    VerifyEqual("9. Verify In the body of the plain-text mail has field 'Défaut: oui'", true, newMail.Body.IndexOf("Défaut: oui") > 0);
                    VerifyEqual("[#1429535] 9. Verify In the body of the plain-text mai has field 'Défaut critique: oui'", true, newMail.Body.IndexOf("Défaut critique: oui") > 0);
                    VerifyEqual("9. Verify In the body of the plain-text mai has field 'Pannes: Panne de lampe'", true, newMail.Body.IndexOf("Pannes: Panne de lampe") > 0);
                }
            });
            tasks.Add(task1);
            var task2 = Task.Run(() =>
            {
                var newMail = EmailUtility.GetNewEmail(mailSubject2);
                var hasNewMail = newMail != null;
                VerifyTrue(string.Format("Verify Report '{0}' has an email sent from '{1}'", reportName2, mailSubject2), hasNewMail, "Email sent", "No email sent");
                if (hasNewMail)
                {
                    var expectedSubject = string.Format("{0} ({1})", mailSubject2, runDate.ToString("dd/MM/yy HH:mm"));
                    VerifyEqual("8. Verify Format: Subject of email + (dd/MM/yy [Time(24h)]:[Minute])", expectedSubject, newMail.Subject);
                    VerifyEqual(string.Format("8. Verify body of '{0}' is html", reportName1), true, newMail.IsBodyHtml);

                    HtmlUtility htmlUtility = new HtmlUtility(newMail.Body);
                    var dt = htmlUtility.GetTable(@"html/body/table");
                    var rows = dt.Select(string.Format("Equipement = '{0}'", streetlight));
                    if (rows.Any())
                    {
                        VerifyEqual(string.Format("9. Verify In the body of the HTML-format mail has Name of testing device is '{0}'", streetlight), streetlight, rows[0]["Equipement"]);
                        VerifyEqual("9. Verify In the body of the HTML-format mail has field 'Défaut: oui'", "oui", rows[0]["Défaut"]);
                        VerifyEqual("[#1429535] 9. Verify In the body of the HTML-format mai has field 'Défaut critique: oui'", "oui", rows[0]["Défaut critique"]);
                        VerifyEqual("9. Verify In the body of the HTML-format mai has field 'Pannes: Panne de lampe'", "Panne de lampe", rows[0]["Pannes"]);
                    }
                    else
                    {
                        Warning(string.Format("9. There is no row with device name '{0}' in email", streetlight));
                    }
                }
            });
            tasks.Add(task2);
            Task.WaitAll(tasks.ToArray());

            Step("10. Send the simulated command to set Lamp Failure =false");
            request = SetValueToDevice(controllerId, streetlight, "LampFailure", "false", Settings.GetServerTime());

            Step("11. Verify The simulated command sent successfully.");
            VerifyEqual(string.Format("11. Verify the request is sent successfully (attribute: {0}, value: {1})", "LampFailure", "false"), true, request);

            Step("12. Update the time of the two reports to let it run a few minute later and check mails again");
            tasks.Clear();
            currentDate = Settings.GetServerTime();
            runDate = currentDate.AddMinutes(REPORT_WAIT_MINUTES);
            EmailUtility.CleanInbox(updatedMailSubject);
            //Update report 1
            reportManagerPage.GridPanel.ClickGridRecord(reportName1);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();
            reportManagerPage.ReportEditorPanel.SelectTab("Scheduler");
            reportManagerPage.ReportEditorPanel.SelectHourDropDown(string.Format("{0:D2}", runDate.Hour));
            reportManagerPage.ReportEditorPanel.SelectMinuteDropDown(string.Format("{0:D2}", runDate.Minute));
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            reportManagerPage.ReportEditorPanel.EnterSubjectInput(updatedMailSubject1);
            reportManagerPage.ReportEditorPanel.ClickSaveButton();
            reportManagerPage.WaitForPreviousActionComplete();
            if (reportManagerPage.IsPopupDialogDisplayed() && reportManagerPage.Dialog.GetMessageText().Equals("Ce nom existe déjà !"))
            {
                Warning("#1437477 - 'Report already exists' message shown after editing and saving a report");
                return;
            }
            else
                reportManagerPage.WaitForReportDetailsDisappeared();

            //Update report 2
            reportManagerPage.GridPanel.ClickGridRecord(reportName2);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();
            reportManagerPage.ReportEditorPanel.SelectTab("Scheduler");
            reportManagerPage.ReportEditorPanel.SelectHourDropDown(string.Format("{0:D2}", runDate.Hour));
            reportManagerPage.ReportEditorPanel.SelectMinuteDropDown(string.Format("{0:D2}", runDate.Minute));
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            reportManagerPage.ReportEditorPanel.EnterSubjectInput(updatedMailSubject2);
            reportManagerPage.ReportEditorPanel.ClickSaveButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisappeared();

            Step("13. Verify");
            Step(" • In the body of the plain-text mail, the report displays");
            Step("  o Name of testing device:");
            Step("  o Make sure the following fields:");
            Step("   + Défaut: non");
            Step("   + Défaut critique: non");
            Step("   + Pannes: empty");
            Step(" • In the body of the HTML-format mail, the report displays in columns");
            Step("  o Device: Name of testing device");
            Step("  o Défaut: non");
            Step("  o Défaut critique: non");
            Step("  o Pannes: empty");

            task1 = Task.Run(() =>
            {
                var newMail = EmailUtility.GetNewEmail(updatedMailSubject1);
                var hasNewMail = newMail != null;
                VerifyTrue(string.Format("Verify Updated Report '{0}' has an email sent from '{1}'", reportName1, updatedMailSubject1), hasNewMail, "Email sent", "No email sent");
                if (hasNewMail)
                {
                    var expectedSubject = string.Format("{0} ({1})", updatedMailSubject1, runDate.ToString("dd/MM/yy HH:mm"));
                    VerifyEqual("13. Verify Format: Subject of email + (dd/mm/yy [Time]:[Minute])", expectedSubject, newMail.Subject);
                    VerifyEqual(string.Format("13. Verify body of '{0}' is plain text", reportName1), true, !newMail.IsBodyHtml);
                    VerifyEqual(string.Format("13. Verify In the body of the plain-text mail has Name of testing device is '{0}'", streetlight), true, newMail.Body.IndexOf(streetlight) > 0);
                    VerifyEqual("13. Verify In the body of the plain-text mail has field 'Défaut: non'", true, newMail.Body.IndexOf("Défaut: non") > 0);
                    VerifyEqual("13. Verify In the body of the plain-text mai has field 'Défaut critique: non'", true, newMail.Body.IndexOf("Défaut critique: non") > 0);
                    VerifyEqual("13. Verify In the body of the plain-text mai has field 'Pannes:'", true, newMail.Body.IndexOf("Pannes:") > 0);
                }
            });
            tasks.Add(task1);
            task2 = Task.Run(() =>
            {
                var newMail = EmailUtility.GetNewEmail(updatedMailSubject2);
                var hasNewMail = newMail != null;
                VerifyTrue(string.Format("Verify Updated Report '{0}' has an email sent from '{1}'", reportName2, updatedMailSubject2), hasNewMail, "Email sent", "No email sent");
                if (hasNewMail)
                {
                    var expectedSubject = string.Format("{0} ({1})", updatedMailSubject2, runDate.ToString("dd/MM/yy HH:mm"));
                    VerifyEqual("13. Verify Format: Subject of email + (dd/mm/yy [Time]:[Minute])", expectedSubject, newMail.Subject);
                    VerifyEqual(string.Format("13. Verify body of '{0}' is html", reportName1), true, newMail.IsBodyHtml);

                    HtmlUtility htmlUtility = new HtmlUtility(newMail.Body);
                    var dt = htmlUtility.GetTable(@"html/body/table");
                    var rows = dt.Select(string.Format("Equipement = '{0}'", streetlight));
                    if (rows.Any())
                    {
                        VerifyEqual(string.Format("13. Verify In the body of the HTML-format mail has Name of testing device is '{0}'", streetlight), streetlight, rows[0]["Equipement"]);
                        VerifyEqual("13. Verify In the body of the HTML-format mail has field 'Défaut: non'", "non", rows[0]["Défaut"]);
                        VerifyEqual("13. Verify In the body of the HTML-format mai has field 'Défaut critique: non'", "non", rows[0]["Défaut critique"]);
                        VerifyEqual("13. Verify In the body of the HTML-format mai has field 'Pannes: empty'", "", rows[0]["Pannes"]);
                    }
                    else
                    {
                        Warning(string.Format("13. There is no row with device name '{0}' in email", streetlight));
                    }
                }
            });
            tasks.Add(task2);
            Task.WaitAll(tasks.ToArray());

            try
            {
                Info("Delete reports after testing");
                reportManagerPage.DeleteReport(reportName1);
                reportManagerPage.DeleteReport(reportName2);                
                DeleteGeozone(newGeozoneName);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("RM_33 'Advanced Search' Report")]
        public void RM_33()
        {
            var reportName = SLVHelper.GenerateUniqueName(string.Format("[{0}][{1}] {2}", Browser.Name, Settings.Users["DefaultTest"].Username, TestContext.CurrentContext.Test.MethodName));
            var geozone = SLVHelper.GenerateUniqueName("GZNRM33");
            var controller = SLVHelper.GenerateUniqueName("CTRL");
            var expectedSavedSearch = SLVHelper.GenerateUniqueName("SRM33");
            var streetlight = SLVHelper.GenerateUniqueName("STL");
            var macAddress = SLVHelper.GenerateMACAddress();

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - Create a testing geozone containing a streetlight with a valid unique address.");
            Step(" - Create an advanced search for the testing geozone with the criteria: Unique address");
            Step("**** Precondition ****\n");

            Step("-> Create data for testing");
            DeleteGeozones("GZNRM33*");
            CreateNewGeozone(geozone);
            CreateNewController(controller, geozone);
            CreateNewDevice(DeviceType.Streetlight, streetlight, controller, geozone);
            SetValueToDevice(controller, streetlight, "MacAddress", macAddress, Settings.GetServerTime());            

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.AdvancedSearch, App.ReportManager);

            Step("-> Create an advanced search for the testing geozone with the criteria: Unique address");
            var advancedSearchPage = desktopPage.GoToApp(App.AdvancedSearch) as AdvancedSearchPage;
            advancedSearchPage.CreateNewSearch(expectedSavedSearch, geozone, "Unique address", "=", macAddress, true);

            Step("1. Go to Report Manager app");
            Step("2. Expected Report Manager is routed and loaded successfully");
            desktopPage = Browser.RefreshLoggedInCMS();
            var reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();

            Step("3. Select a geozone");
            reportManagerPage.GeozoneTreeMainPanel.SelectNode(geozone);

            Step("4. Add new report with 'Advanced Search Report' type");
            reportManagerPage.GridPanel.ClickAddReportToolbarButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.ReportEditorPanel.WaitForPanelLoaded();
            reportManagerPage.ReportEditorPanel.SelectTypeDropDown("Advanced Search Report");
            reportManagerPage.WaitForPreviousActionComplete();

            Step("5. Expected Report Details panel appears with section:");
            Step("  o Name");
            Step("  o Type");
            Step("  o Scheduler tab");
            Step("   * Configuration section");
            Step("    - Hour (HH): range [00-23]");
            Step("    - Minute: range [00-59]");
            Step("    - Periodicity : list");
            Step("     + Every day");
            Step("     + Every monday");
            Step("     + Every tuesday");
            Step("     + Every wednesday");
            Step("     + Every friday");
            Step("     + Every saturday");
            Step("     + Every sunday");
            Step("  o Report tab");
            Step("   * Configuration section");
            Step("    - Save search: select the advanced search from the precondition");
            Step("  o Export tab");
            Step("   * FTP section");
            Step("    - FTP Host");
            Step("    - SFTP mode");
            Step("    - FTP user");
            Step("    - FTP password");
            Step("    - FTP file name");
            Step("    - FTP passive mode");
            Step("    - FTP enabled");
            Step("    - FTP format: list");
            Step("     + CSV");
            Step("     + Html");
            Step("     + Plain text");
            Step("   * Mail section");
            Step("    - Mail enabled");
            Step("    - Mail format: list");
            Step("     + CSV");
            Step("     + Html");
            Step("     + Plain text");
            Step("    - Report in mail's body");
            Step("    - Subject");
            Step("    - From");
            Step("    - Contacts:list");
            VerifyAdvancedSearchReportDetails(reportManagerPage, expectedSavedSearch);

            Step("6. Fill values into report fields:");
            Step("  o Name: a unique name");
            Step("  o Other fields (if not specifically indicated):");
            Step("   - Text: Any");
            Step("   - Numeric: Any");
            Step("   - Dropdown: any in list, some in list (multi-selectable)");
            Step("   - Checkbox: randomly un/check");
            var dropdownLists = new string[] { "Contacts" };
            reportManagerPage.ReportEditorPanel.EnterNameInput(reportName);
            reportManagerPage.ReportEditorPanel.EnterEditablePropertiesValue(dropdownLists);
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            reportManagerPage.ReportEditorPanel.SelectRandomContactsDropDownList();
            var notedSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            var notedValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);

            Step("7. Click Save icon");
            reportManagerPage.ReportEditorPanel.ClickSaveButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisappeared();

            Step("8. Expected The new report is saved as a new row in grid of reports");
            var reportsList = reportManagerPage.GridPanel.GetListOfColumnData("Name");
            VerifyEqual(string.Format("8. Verify new report {0} is saved as a new row in grid of reports", reportName), true, reportsList.Exists(p => p.Equals(reportName)));

            Step("9. Select the newly-created report");
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();

            Step("10. Expected Report Details panel appears with section; Verify input values are remained");
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            var actualSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            var actualValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);
            VerifyEqual("10. Verify input values are remained", notedValues, actualValues);
            VerifyEqual("10. Verify Selected Contacts are remained", notedSelectedContacts, actualSelectedContacts);

            Step("11. Reload browser and browse to the created report again");
            desktopPage = Browser.RefreshLoggedInCMS();
            reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();

            Step("12. Expected Verify input values are remained");
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            actualSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            actualValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);
            VerifyEqual("12. Verify input values are remained", notedValues, actualValues);
            VerifyEqual("12. Verify Selected Contacts are remained", notedSelectedContacts, actualSelectedContacts);

            Step("13. Change values of the report (if a checkbox was checked, unchecked it and otherwise. Select all if it is a multi-selectable dropdown list) and save");
            reportManagerPage.ReportEditorPanel.EnterEditablePropertiesValue(dropdownLists);
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            reportManagerPage.ReportEditorPanel.SelectAllContactsDropDownList();
            notedSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            notedValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);

            reportManagerPage.ReportEditorPanel.ClickSaveButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisappeared();

            Step("14. Expected");
            Step("  o Name fields is editable (#1325077)");
            Step("  o Type field is readonly");
            Step("  o All fields are editable");
            Step("  o Saving is a success");
            VerifyEqual("[#1325077] 14. Verify Name field is editable", true, !reportManagerPage.ReportEditorPanel.IsNameInputReadOnly());
            VerifyEqual("14. Verify Type field is readonly", true, reportManagerPage.ReportEditorPanel.IsTypeDropDownReadOnly());
            VerifyEqual("14. Verify All fields are editable", true, reportManagerPage.ReportEditorPanel.AreFieldsEditable());

            Step("15. Select the newly-created report");
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();

            Step("16. Expected Report Details panel appears with section; Verify input values are remained");
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            actualSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            actualValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);
            VerifyEqual("16. Verify input values are remained", notedValues, actualValues);
            VerifyEqual("16. Verify Selected Contacts are remained", notedSelectedContacts, actualSelectedContacts);

            Step("17. Reload browser and browse to the created report again");
            desktopPage = Browser.RefreshLoggedInCMS();
            reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();
            reportManagerPage.GridPanel.ClickGridRecord(reportName);
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisplayed();

            Step("18. Expected Verify input values are remained");
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            actualSelectedContacts = reportManagerPage.ReportEditorPanel.GetContactsValues();
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            actualValues = reportManagerPage.ReportEditorPanel.GetPropertiesAndValues(dropdownLists);
            VerifyEqual("18. Verify input values are remained", notedValues, actualValues);
            VerifyEqual("18. Verify Selected Contacts are remained", notedSelectedContacts, actualSelectedContacts);

            Step("19. Click Delete button in Report Details panel");
            reportManagerPage.ReportEditorPanel.ClickDeleteButton();
            reportManagerPage.WaitForPopupDialogDisplayed();

            Step("20. Expected A confirmation dialog appears with title \"Confirmation\" & message \"Would you like to delete '{{Report Name}}' report ? \"");
            var expectedMessage = string.Format("Would you like to delete '{0}' report ?", reportName);
            VerifyEqual("20. Verify dialog title is 'Confirmation'", "Confirmation", reportManagerPage.Dialog.GetDialogTitleText());
            VerifyEqual(string.Format("20. Verify dialog message is '{0}'", expectedMessage), expectedMessage, reportManagerPage.Dialog.GetMessageText());

            Step("21. Click No");
            reportManagerPage.Dialog.ClickNoButton();
            reportManagerPage.WaitForPopupDialogDisappeared();

            Step("22. Expected The dialog disappears");
            VerifyEqual("22. Verify The dialog disappears", false, reportManagerPage.IsPopupDialogDisplayed());

            Step("23. Click Delete button in Report Details panel again");
            reportManagerPage.ReportEditorPanel.ClickDeleteButton();
            reportManagerPage.WaitForPopupDialogDisplayed();

            Step("24. Expected A confirmation dialog appears with title \"Confirmation\" & message \"Would you like to delete '{{Report Name}}' report ? \"");
            VerifyEqual("24. Verify dialog title is 'Confirmation'", "Confirmation", reportManagerPage.Dialog.GetDialogTitleText());
            VerifyEqual(string.Format("24. Verify dialog message is '{0}'", expectedMessage), expectedMessage, reportManagerPage.Dialog.GetMessageText());

            Step("25. Click Yes");
            reportManagerPage.Dialog.ClickYesButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForPopupDialogDisappeared();

            Step("26. Expected The dialog disappears; the report is no longer present in the grid panel");
            VerifyEqual("26. Verify The dialog disappears", false, reportManagerPage.IsPopupDialogDisplayed());
            reportsList = reportManagerPage.GridPanel.GetListOfColumnData("Name");
            VerifyEqual("26. Verify The report is no longer present in the grid panel", false, reportsList.Exists(p => p.Equals(reportName)));

            Step("27. Reload browser and go to Report Manager");
            desktopPage = Browser.RefreshLoggedInCMS();
            reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();

            Step("28. Select the geozone where the deleted report was created");
            reportManagerPage.GeozoneTreeMainPanel.SelectNode(geozone);

            Step("29. Expected The report is no longer present in the grid panel");
            reportsList = reportManagerPage.GridPanel.GetListOfColumnData("Name");
            VerifyEqual("29. Verify The report is no longer present in the grid panel", false, reportsList.Exists(p => p.Equals(reportName)));

            try
            {
                advancedSearchPage = reportManagerPage.AppBar.SwitchTo(App.AdvancedSearch) as AdvancedSearchPage;
                advancedSearchPage.SearchWizardPopupPanel.ClickCloseButton();
                advancedSearchPage.WaitForSearchWizardPopupPanelDisappeared();
                advancedSearchPage.GridPanel.DeleleRequest(expectedSavedSearch);
                DeleteGeozone(geozone);
            }
            catch { }
             
        }

        [Test, DynamicRetry]
        [Description("Advanced Search Report - Upload report file to Ftp server and Sending Email to user -SC-205")]
        public void RM_34()
        {
            var testData = GetTestDataOfTestRM_34();   
            var reportData = testData["ReportData"] as XmlNode;
            var mail = testData["Mail"] as XmlNode;
            var ftp = testData["Ftp"] as XmlNode;

            var geozone = SLVHelper.GenerateUniqueName("GZNRM34");
            var searchName = SLVHelper.GenerateUniqueName("SRM34");
            var controller = SLVHelper.GenerateUniqueName("CTRL");          
            var streetlight = SLVHelper.GenerateUniqueName("CTL");
            var address1 = SLVHelper.GenerateString(9);       
            var ftpFilePattern = SLVHelper.GenerateUniqueName(TestContext.CurrentContext.Test.MethodName + "-" + Settings.Users["DefaultTest"].Username);
            var reportType = reportData.GetAttrVal("ReportType");
            var reportName = SLVHelper.GenerateUniqueName(string.Format("[{0}][{1}][{2}] {3}", Browser.Name , Settings.Users["DefaultTest"].Username, TestContext.CurrentContext.Test.MethodName, reportType));
            var mailSubject = string.Format("[{0}][{1}] {2}", Settings.Users["DefaultTest"].Username, TestContext.CurrentContext.Test.MethodName, reportType);
            var periodicity = reportData.GetAttrVal("Periodicity");
            var timezone = Settings.DEFAULT_TIMEZONE;
            var fullGeozone = string.Format("{0} ({1})", geozone, Settings.RootGeozoneName);

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - Create a testing geozone containing a controller and a streetlight with the same Address 1 value");
            Step(" - Create an advance search for the testing geozone and the criteria is Address 1; operator: contains; value of testing Address 1");
            Step("**** Precondition ****\n");

            Step("-> Create data for testing");
            DeleteGeozones("GZNRM34*");
            CreateNewGeozone(geozone);
            CreateNewController(controller, geozone);
            SetValueToController(controller, "address", address1, Settings.GetServerTime());            
            CreateNewDevice(DeviceType.Streetlight, streetlight, controller, geozone);
            SetValueToDevice(controller, streetlight, "address", address1, Settings.GetServerTime());

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.AdvancedSearch, App.ReportManager);
            Step("-> Create an advanced search for the testing geozone with the criteria: Address 1");
            var advancedSearchPage = desktopPage.GoToApp(App.AdvancedSearch) as AdvancedSearchPage;
            advancedSearchPage.CreateNewSearch(searchName, geozone, "Address 1", "contains", address1, true);

            Step("1. Go to Report Manager app");
            Step("2. Expected Report Manager is routed and loaded successfully");
            desktopPage = Browser.RefreshLoggedInCMS();
            var reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();

            Step("3. Select the testing geozone");
            reportManagerPage.GeozoneTreeMainPanel.SelectNode(geozone);

            Step("4. Create and save new report with parameters:");
            Step(" - Name: 'Advanced Search Report { { current time stamp} }");
            Step(" - Type: 'Advanced Search Report'");
            Step(" - Scheduler tab:");
            Step("  + Hour (HH): the current value");
            Step("  + Minute: the current value + 2 (the report will run after 2 minutes later)");
            Step("  + Periodicity: Every day");
            Step("  + TimeZone: UTC");
            Step(" - Report tab:");
            Step("  + Select the advance search in the precondition");
            Step(" - Export tab:");
            Step("  + ftp: working parameters");
            Step("  + email: working parameters");
            var currentDate = Settings.GetServerTime();
            var runDateTime = currentDate.AddMinutes(REPORT_WAIT_MINUTES);
            var mailHour = runDateTime.Hour;
            var mailMinute = runDateTime.Minute;
            EmailUtility.CleanInbox(mailSubject);
            reportManagerPage.GridPanel.ClickAddReportToolbarButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.ReportEditorPanel.WaitForPanelLoaded();
            reportManagerPage.ReportEditorPanel.EnterNameInput(reportName);
            reportManagerPage.ReportEditorPanel.SelectTypeDropDown(reportType);
            reportManagerPage.WaitForPreviousActionComplete();

            //Scheduler tab
            reportManagerPage.ReportEditorPanel.SelectTab("Scheduler");    
            reportManagerPage.ReportEditorPanel.SelectHourDropDown(string.Format("{0:D2}", mailHour));
            reportManagerPage.ReportEditorPanel.SelectMinuteDropDown(string.Format("{0:D2}", mailMinute));
            reportManagerPage.ReportEditorPanel.SelectPeriodicityDropDown(periodicity);
            reportManagerPage.ReportEditorPanel.SelectTimezoneDropDown(timezone);

            //Report tab
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            reportManagerPage.ReportEditorPanel.SelectSavedSearchDropDown(searchName);

            //Export tab
            //FTP
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            reportManagerPage.ReportEditorPanel.EnterFtpHostInput(ftp.GetAttrVal("Host"));
            reportManagerPage.ReportEditorPanel.TickSftpModeCheckbox(bool.Parse(ftp.GetAttrVal("SFTPMode")));
            reportManagerPage.ReportEditorPanel.EnterFtpUserInput(ftp.GetAttrVal("User"));
            reportManagerPage.ReportEditorPanel.EnterFtpPasswordInput(ftp.GetAttrVal("Password"));
            reportManagerPage.ReportEditorPanel.EnterFtpFilenameInput(ftp.GetAttrVal("Directory") + ftpFilePattern);
            reportManagerPage.ReportEditorPanel.TickFtpEnabledCheckbox(bool.Parse(ftp.GetAttrVal("Enabled")));
            reportManagerPage.ReportEditorPanel.SelectFtpFormatDropDown(reportData.GetAttrVal("FtpFormat"));

            //Mail
            reportManagerPage.ReportEditorPanel.TickMailEnabledCheckbox(bool.Parse(reportData.GetAttrVal("MailEnabled")));
            reportManagerPage.ReportEditorPanel.SelectMailFormatDropDown(reportData.GetAttrVal("MailFormat"));
            reportManagerPage.ReportEditorPanel.TickReportMailBodyCheckbox(bool.Parse(reportData.GetAttrVal("ReportMailBody")));
            reportManagerPage.ReportEditorPanel.EnterSubjectInput(mailSubject);
            reportManagerPage.ReportEditorPanel.EnterFromInput(mail.GetAttrVal("From"));
            reportManagerPage.ReportEditorPanel.SelectContactsListDropDown(mail.GetAttrVal("Contacts"));           

            reportManagerPage.ReportEditorPanel.ClickSaveButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisappeared();
            
            var expectedSchedule = string.Format("Every day at {0:D2}:{1:D2}", mailHour, mailMinute);
            var expectedNextExecution = runDateTime.ToString("M/d/yy h:mm");

            Step("5. Verify The newly-created report is presented in grid with columns");
            Step(" - Name: The name of the report");
            Step(" - GeoZone: the testing geozone (the parent geozone if existing)");
            Step(" - Template: 'Advanced Search Report'");
            Step(" - Schedule: Every day at [Hour]:[Minute] [TimeZone]");
            Step(" - Last execution: empty");
            Step(" - Next execution: today with format M/d/yy hh:mm:00 AM/PM TimeZone. Ex: 6/12/18 4:09:00 AM UTC");
            var dt = reportManagerPage.GridPanel.BuildDataTableFromGrid();
            var rows = dt.Select(string.Format("Name = '{0}'", reportName));
            if (rows.Any())
            {
                var colName = rows[0]["Name"].ToString();
                var colGeozone = rows[0]["GeoZone"].ToString();
                var colTemplate = rows[0]["Template"].ToString();
                var colSchedule = rows[0]["Schedule"].ToString();
                var colLastExecution = rows[0]["Last execution"].ToString();
                var colNextExecution = rows[0]["Next execution"].ToString();

                VerifyEqual(string.Format("5. Verify Name: the report's name is '{0}'", reportName), reportName, colName);
                VerifyEqual(string.Format("5. Verify GeoZone: the testing geozone (the parent geozone if existing) is '{0}'", fullGeozone), fullGeozone, colGeozone);
                VerifyEqual(string.Format("[#1429532] 5. Verify Template: the report's type is '{0}'", reportType), reportType, colTemplate);
                VerifyTrue("5. Verify Schedule: Every day at [Hour]:[Minute] [TimeZone].", colSchedule.Contains(expectedSchedule), expectedSchedule, colSchedule);
                VerifyEqual("[#1429527] 5. Verify Last execution: empty", "", colLastExecution);
                VerifyTrue("5. Verify Next execution: today with format M/d/yy h:mm:00 AM/PM TimeZone. Ex: 6/12/18 4:09:00 AM UTC", colNextExecution.Contains(expectedNextExecution), expectedNextExecution, colNextExecution);
            }
            else
                Warning(string.Format("5. There is no row with report name '{0}'", reportName));

            Step("6. Wait until the end time past, then check FTP report folder");
            Step("7. Expected");
            Step(" - A report (csv file) is uploaded to FTP folder (There is a bug for this: SC-232)");
            Step(" - Note: No file in ftp. Reopen SC-205");
            var ftpUtility = new FtpUtility(ftp.GetAttrVal("Host"), ftp.GetAttrVal("User"), ftp.GetAttrVal("Password"), ftp.GetAttrVal("Directory"));
            var fileName = ftpUtility.WaitAndGetFileName(ftpFilePattern);
            if (fileName != null)
            {
                VerifyEqual("5. Verify file exists in FTP report folder and file extension is CSV", true, fileName.Contains(".csv"));
            }
            else
            {
                Warning(string.Format("[SC-205] 5. File with pattern {0} does not exist on FTP", ftpFilePattern));
            }

            Step("8. Check the email");
            Step("9. Verify An email of report is sent to users with");
            Step(" - The tittle: the name of the report and the time it is generated. Ex: New Report (6/12/18 4:16 AM)");
            Step(" - The attachement is csv file with");
            Step("  + Name: advanced_search.csv");
            Step("  + The headers are: name;address;ConfigStatus;DimmingGroupName;installStatus;MacAddress");
            var tasks = new List<Task>();
            var task = Task.Run(() =>
            {
                var newMail = EmailUtility.GetNewEmail(mailSubject);
                var hasNewMail = newMail != null;
                VerifyTrue(string.Format("5. Verify Report '{0}' has an email sent from '{1}' (Report created: {2}, Expected email revieved: {3})", reportName, mailSubject, Settings.GetServerTime().ToString("G"), currentDate.AddMinutes(REPORT_WAIT_MINUTES).ToString("G")), hasNewMail, "Email sent", "No email sent");
                if (hasNewMail)
                {
                    var attachment = newMail.Attachments.FirstOrDefault();
                    if (attachment != null)
                    {                        
                        VerifyEqual("9. Verify The attachement is csv file and name is advanced_search.csv", "advanced_search.csv", attachment.Name);

                        var reader = new StreamReader(attachment.ContentStream);
                        var expectedHeader = "name;address;ConfigStatus;DimmingGroupName;installStatus;MacAddress";
                        var expectedHeaderArr = expectedHeader.SplitEx(new string[] { ";" });
                        var actualHeader = reader.ReadLine();
                        var actualHeaderArr = actualHeader.SplitEx(new string[] { ";" });
                        VerifyTrue("9. Verify The headers are: name;address;ConfigStatus;DimmingGroupName;installStatus;MacAddress", actualHeaderArr.CheckIfIncluded(expectedHeaderArr), expectedHeader, actualHeader);
                    }
                    else
                    {
                        Warning("9. Mail does not have Attachment");
                    }
                }
            });
            tasks.Add(task);
            Task.WaitAll(tasks.ToArray());

            Step("10. Refresh page and check the report on the grid");
            desktopPage = Browser.RefreshLoggedInCMS();
            reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.GeozoneTreeMainPanel.SelectNode(geozone);
            reportManagerPage.WaitForGridPanelDisplayed();

            Step("11. Verify The record is updated");
            Step(" - Last execution= Next execution. Ex:6/12/18 4:09:00 AM UTC");
            Step(" - Next execution= tomorrow with format M/d/yy hh:mm:00 AM/PM TimeZone. Ex: 6/13/18 4:09:00 AM UTC");
            dt = reportManagerPage.GridPanel.BuildDataTableFromGrid();
            rows = dt.Select(string.Format("Name = '{0}'", reportName));
            if (rows.Any())
            {
                var colLastExecution = rows[0]["Last execution"].ToString();
                var colNextExecution = rows[0]["Next execution"].ToString();                
                VerifyTrue("11. Verify Last execution= Next execution. Ex:6/12/18 4:09:00 AM UTC", colLastExecution.Contains(expectedNextExecution), expectedNextExecution, colLastExecution);
                expectedNextExecution = runDateTime.AddDays(1).ToString("M/d/yy h:mm");
                VerifyTrue("11. Verify Next execution: tomorrow with format M/d/yy hh:MM:00 AM/PM TimeZone. Ex: 6/12/18 4:09:00 AM UTC", colNextExecution.Contains(expectedNextExecution), expectedNextExecution, colNextExecution);
            }
            else
                Warning(string.Format("11. There is no row with report name '{0}'", reportName));

            try
            {
                reportManagerPage.DeleteReport(reportName);                
                advancedSearchPage = reportManagerPage.AppBar.SwitchTo(App.AdvancedSearch) as AdvancedSearchPage;
                advancedSearchPage.SearchWizardPopupPanel.ClickCloseButton();
                advancedSearchPage.WaitForSearchWizardPopupPanelDisappeared();
                advancedSearchPage.GridPanel.DeleleRequest(searchName);
                DeleteGeozone(geozone);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("Advanced Search Report - Search from a geozone on which the advanced search is not based -SC-205")]
        public void RM_35()
        {
            var testData = GetTestDataOfTestRM_35();
            var reportData = testData["ReportData"] as XmlNode;
            var mail = testData["Mail"] as XmlNode;
            var ftp = testData["Ftp"] as XmlNode;
            
            var geozone1 = SLVHelper.GenerateUniqueName("GZNRM3501");
            var geozone2 = SLVHelper.GenerateUniqueName("GZNRM3502");
            var searchName = SLVHelper.GenerateUniqueName("SRM35");
            var controller = SLVHelper.GenerateUniqueName("CTRL");
            var streetlight1 = SLVHelper.GenerateUniqueName("STL01");
            var streetlight2 = SLVHelper.GenerateUniqueName("STL02");
            var macAddress = SLVHelper.GenerateMACAddress();
            var ftpFilePattern = SLVHelper.GenerateUniqueName(TestContext.CurrentContext.Test.MethodName + "-" + Settings.Users["DefaultTest"].Username);
            var reportType = reportData.GetAttrVal("ReportType");
            var reportName = SLVHelper.GenerateUniqueName(string.Format("[{0}][{1}][{2}] {3}", Browser.Name, Settings.Users["DefaultTest"].Username, TestContext.CurrentContext.Test.MethodName, reportType));
            var mailSubject = string.Format("[{0}][{1}] {2}", Settings.Users["DefaultTest"].Username, TestContext.CurrentContext.Test.MethodName, reportType);
            var periodicity = reportData.GetAttrVal("Periodicity");
            var timezone = Settings.DEFAULT_TIMEZONE;
            var fullGeozone2 =  string.Format("{0} ({1})", geozone2, Settings.RootGeozoneName);

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - Create the 1st geozone containing a controller and 1st streetlight with the Install Status: Installed");
            Step(" - Create 2nd geozone containing 2nd streetlight with the Install Status: Installed; a valid mac address and using the same controller of 1st streetlight");
            Step(" - Create an advance search for 1st geozone and the criteria is Install; operator: in; value = installed. This advance search matches only 1st streetlight");
            Step("**** Precondition ****\n");

            Step("-> Create data for testing");
            DeleteGeozones("GZNRM35*");
            CreateNewGeozone(geozone1);
            CreateNewGeozone(geozone2);
            CreateNewController(controller, geozone1);
            CreateNewDevice(DeviceType.Streetlight, streetlight1, controller, geozone1);
            SetValueToDevice(controller, streetlight1, "installStatus", "Installed", Settings.GetServerTime());
            CreateNewDevice(DeviceType.Streetlight, streetlight2, controller, geozone2);
            SetValueToDevice(controller, streetlight2, "MacAddress", macAddress, Settings.GetServerTime());
            SetValueToDevice(controller, streetlight2, "installStatus", "Installed", Settings.GetServerTime());

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.AdvancedSearch, App.ReportManager);

            Step("-> Create an advanced search for 1st geozone and the criteria is Install; operator: in; value = installed");
            var advancedSearchPage = desktopPage.GoToApp(App.AdvancedSearch) as AdvancedSearchPage;
            advancedSearchPage.CreateNewSearch(searchName, geozone1, "Install status", "in", "Installed", true);

            Step("1. Go to Report Manager app");
            Step("2. Expected Report Manager is routed and loaded successfully");
            desktopPage = Browser.RefreshLoggedInCMS();
            var reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;
            reportManagerPage.WaitForGridPanelDisplayed();

            Step("3. Select 2nd geozone");
            reportManagerPage.GeozoneTreeMainPanel.SelectNode(geozone2);

            Step("4. Create and save new report with parameters:");
            Step(" - Name: 'Advanced Search Report { { current time stamp} }");
            Step(" - Type: 'Advanced Search Report'");
            Step(" - Scheduler tab:");
            Step("  + Hour (HH): the current value");
            Step("  + Minute: the current value + 2 (the report will run after 2 minutes later)");
            Step("  + Periodicity: Every day");
            Step("  + TimeZone: UTC");
            Step(" - Report tab:");
            Step("  + Select the advance search in the precondition");
            Step(" - Export tab:");
            Step("  + ftp: working parameters");
            Step("  + email: working parameters");
            var currentDate = Settings.GetServerTime();
            var runDateTime = currentDate.AddMinutes(REPORT_WAIT_MINUTES);
            var mailHour = runDateTime.Hour;
            var mailMinute = runDateTime.Minute;
            EmailUtility.CleanInbox(mailSubject);
            reportManagerPage.GridPanel.ClickAddReportToolbarButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.ReportEditorPanel.WaitForPanelLoaded();
            reportManagerPage.ReportEditorPanel.EnterNameInput(reportName);
            reportManagerPage.ReportEditorPanel.SelectTypeDropDown(reportType);
            reportManagerPage.WaitForPreviousActionComplete();

            //Scheduler tab
            reportManagerPage.ReportEditorPanel.SelectTab("Scheduler");
            reportManagerPage.ReportEditorPanel.SelectHourDropDown(string.Format("{0:D2}", mailHour));
            reportManagerPage.ReportEditorPanel.SelectMinuteDropDown(string.Format("{0:D2}", mailMinute));
            reportManagerPage.ReportEditorPanel.SelectPeriodicityDropDown(periodicity);
            reportManagerPage.ReportEditorPanel.SelectTimezoneDropDown(timezone);

            //Report tab
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            reportManagerPage.ReportEditorPanel.SelectSavedSearchDropDown(searchName);

            //Export tab
            //FTP
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            reportManagerPage.ReportEditorPanel.EnterFtpHostInput(ftp.GetAttrVal("Host"));
            reportManagerPage.ReportEditorPanel.TickSftpModeCheckbox(bool.Parse(ftp.GetAttrVal("SFTPMode")));
            reportManagerPage.ReportEditorPanel.EnterFtpUserInput(ftp.GetAttrVal("User"));
            reportManagerPage.ReportEditorPanel.EnterFtpPasswordInput(ftp.GetAttrVal("Password"));
            reportManagerPage.ReportEditorPanel.EnterFtpFilenameInput(ftp.GetAttrVal("Directory") + ftpFilePattern);
            reportManagerPage.ReportEditorPanel.TickFtpEnabledCheckbox(bool.Parse(ftp.GetAttrVal("Enabled")));
            reportManagerPage.ReportEditorPanel.SelectFtpFormatDropDown(reportData.GetAttrVal("FtpFormat"));

            //Mail
            reportManagerPage.ReportEditorPanel.TickMailEnabledCheckbox(bool.Parse(reportData.GetAttrVal("MailEnabled")));
            reportManagerPage.ReportEditorPanel.SelectMailFormatDropDown(reportData.GetAttrVal("MailFormat"));
            reportManagerPage.ReportEditorPanel.TickReportMailBodyCheckbox(bool.Parse(reportData.GetAttrVal("ReportMailBody")));
            reportManagerPage.ReportEditorPanel.EnterSubjectInput(mailSubject);
            reportManagerPage.ReportEditorPanel.EnterFromInput(mail.GetAttrVal("From"));
            reportManagerPage.ReportEditorPanel.SelectContactsListDropDown(mail.GetAttrVal("Contacts"));

            reportManagerPage.ReportEditorPanel.ClickSaveButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisappeared();

            var expectedSchedule = string.Format("Every day at {0:D2}:{1:D2}", mailHour, mailMinute);
            var expectedNextExecution = runDateTime.ToString("M/d/yy h:mm");

            Step("5. Verify The newly-created report is presented in grid with columns");
            Step(" - Name: The name of the report");
            Step(" - GeoZone: the testing geozone (the parent geozone if existing)");
            Step(" - Template: 'Advanced Search Report'");
            Step(" - Schedule: Every day at [Hour]:[Minute] [TimeZone]");
            Step(" - Last execution: empty");
            Step(" - Next execution: today with format M/d/yy hh:mm:00 AM/PM TimeZone. Ex: 6/12/18 4:09:00 AM UTC");
            var dt = reportManagerPage.GridPanel.BuildDataTableFromGrid();
            var rows = dt.Select(string.Format("Name = '{0}'", reportName));
            if (rows.Any())
            {
                var colName = rows[0]["Name"].ToString();
                var colGeozone = rows[0]["GeoZone"].ToString();
                var colTemplate = rows[0]["Template"].ToString();
                var colSchedule = rows[0]["Schedule"].ToString();
                var colLastExecution = rows[0]["Last execution"].ToString();
                var colNextExecution = rows[0]["Next execution"].ToString();

                VerifyEqual(string.Format("5. Verify Name: the report's name is '{0}'", reportName), reportName, colName);
                VerifyEqual(string.Format("5. Verify GeoZone: the testing geozone (the parent geozone if existing) is '{0}'", fullGeozone2), fullGeozone2, colGeozone);
                VerifyEqual(string.Format("[#1429532] 5. Verify Template: the report's type is '{0}'", reportType), reportType, colTemplate);
                VerifyTrue("5. Verify Schedule: Every day at [Hour]:[Minute] [TimeZone].", colSchedule.Contains(expectedSchedule), expectedSchedule, colSchedule);
                VerifyEqual("[#1429527] 5. Verify Last execution: empty", "", colLastExecution);
                VerifyTrue("5. Verify Next execution: today with format M/d/yy h:mm:00 AM/PM TimeZone. Ex: 6/12/18 4:09:00 AM UTC", colNextExecution.Contains(expectedNextExecution), expectedNextExecution, colNextExecution);
            }
            else
                Warning(string.Format("5. There is no row with report name '{0}'", reportName));

            Step("6. Wait until the end time past, then check FTP report folder");
            Step("7. Expected");
            Step(" - A report (csv file) is uploaded to FTP folder (There is a bug for this: SC-232)");
            Step(" - Note: No file in ftp. Reopen SC-205");
            var ftpUtility = new FtpUtility(ftp.GetAttrVal("Host"), ftp.GetAttrVal("User"), ftp.GetAttrVal("Password"), ftp.GetAttrVal("Directory"));
            var fileName = ftpUtility.WaitAndGetFileName(ftpFilePattern);
            if (fileName != null)
            {
                VerifyEqual("5. Verify file exists in FTP report folder and file extension is CSV", true, fileName.Contains(".csv"));
            }
            else
            {
                Warning(string.Format("[SC-205] 5. File with pattern {0} does not exist on FTP", ftpFilePattern));
            }

            Step("8. Check the email");
            Step("9. Verify An email of report is sent to users with");
            Step(" - The tittle: the name of the report and the time it is generated. Ex: New Report (6/12/18 4:16 AM)");
            Step(" - The attachement is csv file with");
            Step("  + Name: advanced_search.csv");
            Step("  + The headers are: name;address;ConfigStatus;DimmingGroupName;installStatus;MacAddress");
            Step("  + The Mac Address on the 2nd streetlight 2 is in the file.");
            var tasks = new List<Task>();
            var task = Task.Run(() =>
            {
                var newMail = EmailUtility.GetNewEmail(mailSubject);
                var hasNewMail = newMail != null;
                VerifyTrue(string.Format("5. Verify Report '{0}' has an email sent from '{1}' (Report created: {2}, Expected email revieved: {3})", reportName, mailSubject, Settings.GetServerTime().ToString("G"), currentDate.AddMinutes(REPORT_WAIT_MINUTES).ToString("G")), hasNewMail, "Email sent", "No email sent");
                if (hasNewMail)
                {
                    var attachment = newMail.Attachments.FirstOrDefault();
                    if (attachment != null)
                    {
                        VerifyEqual("9. Verify The attachement is csv file and name is advanced_search.csv", "advanced_search.csv", attachment.Name);

                        var reader = new StreamReader(attachment.ContentStream);
                        var expectedHeader = "name;address;ConfigStatus;DimmingGroupName;installStatus;MacAddress";
                        var expectedHeaderArr = expectedHeader.SplitEx(new string[] { ";" });
                        var actualHeader = reader.ReadLine();
                        var actualHeaderArr = actualHeader.SplitEx(new string[] { ";" });
                        VerifyTrue("9. Verify The headers are: name;address;ConfigStatus;DimmingGroupName;installStatus;MacAddress", actualHeaderArr.CheckIfIncluded(expectedHeaderArr), expectedHeader, actualHeader);
                        
                        var contentLines = reader.ReadToEnd();
                        VerifyTrue("9. Verify The streetlight 2 is in the file", contentLines.Contains(streetlight2), streetlight2, contentLines);
                        VerifyTrue("9. Verify The Mac Address on the 2nd streetlight 2 is in the file", contentLines.Contains(macAddress), macAddress, contentLines);
                    }
                    else
                    {
                        Warning("9. Mail does not have Attachment");
                    }
                }
            });
            tasks.Add(task);
            Task.WaitAll(tasks.ToArray());

            try
            {
                reportManagerPage.DeleteReport(reportName);                
                advancedSearchPage = reportManagerPage.AppBar.SwitchTo(App.AdvancedSearch) as AdvancedSearchPage;
                advancedSearchPage.SearchWizardPopupPanel.ClickCloseButton();
                advancedSearchPage.WaitForSearchWizardPopupPanelDisappeared();
                advancedSearchPage.GridPanel.DeleleRequest(searchName);
                DeleteGeozone(geozone1);
                DeleteGeozone(geozone2);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("RM_36 Check that SFTP reports are successfully generated")]
        public void RM_36()
        {
            var testData = GetTestDataOfTestRM_36();
            var geozone = testData["Geozone"].ToString();
            var reportData = testData["ReportData"] as XmlNode;
            var mail = testData["Mail"] as XmlNode;
            var ftp = testData["Ftp"] as XmlNode;
            var reportType = reportData.GetAttrVal("ReportType");
            var reportName = SLVHelper.GenerateUniqueName(string.Format("[{0}][{1}][{2}] {3}", Browser.Name, Settings.Users["DefaultTest"].Username, TestContext.CurrentContext.Test.MethodName, reportType));
            var mailSubject = string.Format("[{0}][{1}] {2}", Settings.Users["DefaultTest"].Username, TestContext.CurrentContext.Test.MethodName, reportType);
            var ftpFilePattern = SLVHelper.GenerateUniqueName(TestContext.CurrentContext.Test.MethodName + "-" + Settings.Users["DefaultTest"].Username);

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.ReportManager);

            Step("1. Go to Report Manager app");
            Step("2. Verify Report Manager page is routed and loaded successfully");
            var reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;

            Step("3. Create a new report with a random report type which we have known how to run it in any geozone.");
            Step(" -  Make sure that the FTP configuration having: SFTP Mode checked");
            reportManagerPage.GeozoneTreeMainPanel.SelectNode(geozone);
            var currentDate = Settings.GetServerTime();
            var runDate = currentDate.AddMinutes(REPORT_WAIT_MINUTES);

            EmailUtility.CleanInbox(mailSubject);
            reportManagerPage.GridPanel.ClickAddReportToolbarButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.ReportEditorPanel.WaitForPanelLoaded();
            reportManagerPage.ReportEditorPanel.EnterNameInput(reportName);
            reportManagerPage.ReportEditorPanel.SelectTypeDropDown(reportType);
            reportManagerPage.WaitForPreviousActionComplete();

            // Export tab
            //FTP
            reportManagerPage.ReportEditorPanel.SelectTab("Export");
            reportManagerPage.ReportEditorPanel.EnterFtpHostInput(ftp.GetAttrVal("Host"));
            reportManagerPage.ReportEditorPanel.TickSftpModeCheckbox(true);            
            reportManagerPage.ReportEditorPanel.EnterFtpUserInput(ftp.GetAttrVal("User"));
            reportManagerPage.ReportEditorPanel.EnterFtpPasswordInput(ftp.GetAttrVal("Password"));
            reportManagerPage.ReportEditorPanel.EnterFtpFilenameInput(ftp.GetAttrVal("Directory") + ftpFilePattern);
            reportManagerPage.ReportEditorPanel.TickFtpEnabledCheckbox(bool.Parse(ftp.GetAttrVal("Enabled")));
            reportManagerPage.ReportEditorPanel.SelectFtpFormatDropDown(reportData.GetAttrVal("FtpFormat"));

            //Mail
            reportManagerPage.ReportEditorPanel.TickMailEnabledCheckbox(bool.Parse(reportData.GetAttrVal("MailEnabled")));
            reportManagerPage.ReportEditorPanel.SelectMailFormatDropDown(reportData.GetAttrVal("MailFormat"));
            reportManagerPage.ReportEditorPanel.TickReportMailBodyCheckbox(bool.Parse(reportData.GetAttrVal("ReportMailBody")));
            reportManagerPage.ReportEditorPanel.EnterSubjectInput(mailSubject);
            reportManagerPage.ReportEditorPanel.EnterFromInput(mail.GetAttrVal("From"));
            reportManagerPage.ReportEditorPanel.SelectContactsListDropDown(mail.GetAttrVal("Contacts"));

            // Report tab
            reportManagerPage.ReportEditorPanel.SelectTab("Report");
            reportManagerPage.ReportEditorPanel.SelectAllDeviceCategoriesDropDownList();
            reportManagerPage.ReportEditorPanel.SelectAllMeaningsDropDownList();
            reportManagerPage.ReportEditorPanel.EnterStartDayDateInput(currentDate);
            reportManagerPage.ReportEditorPanel.EnterStartTimeInput(string.Format("{0}:{1}", currentDate.Hour.ToString("D2"), currentDate.Minute.ToString("D2")));
            reportManagerPage.ReportEditorPanel.EnterEndDayDateInput(currentDate);
            reportManagerPage.ReportEditorPanel.EnterEndTimeInput(string.Format("{0}:{1}", runDate.Hour.ToString("D2"), runDate.Minute.ToString("D2")));

            reportManagerPage.ReportEditorPanel.ClickSaveButton();
            reportManagerPage.WaitForPreviousActionComplete();
            reportManagerPage.WaitForReportDetailsDisappeared();

            Step("4. Wait for the report run successfully");
            Step("5. Check the attachments in mail and the file uploaded to FTP server with SFTP Mode enabled");
            Step("6. The extension of the attachment and file are CSV");
            var ftpUtility = new FtpUtility(ftp.GetAttrVal("Host"), ftp.GetAttrVal("User"), ftp.GetAttrVal("Password"), ftp.GetAttrVal("Directory"));
            var fileName = ftpUtility.WaitAndGetFileName(ftpFilePattern, true);
            if (fileName != null)
            {
                VerifyEqual("5. Verify file extension is CSV", true, fileName.Contains(".csv"));
            }
            else
            {
                Warning(string.Format("5. File with pattern {0} does not exist on FTP with SFTP Mode enabled", ftpFilePattern));
            }

            var tasks = new List<Task>();
            var task = Task.Run(() =>
            {
                var newMail = EmailUtility.GetNewEmail(mailSubject);
                var hasNewMail = newMail != null;
                VerifyTrue(string.Format("5. Verify Report '{0}' has an email sent from '{1}' (Report created: {2}, Expected email revieved: {3})", reportName, mailSubject, Settings.GetServerTime().ToString("G"), currentDate.AddMinutes(REPORT_WAIT_MINUTES).ToString("G")), hasNewMail, "Email sent", "No email sent");
                if (hasNewMail)
                {
                    var attachment = newMail.Attachments.FirstOrDefault();
                    if (attachment != null)
                    {
                        VerifyEqual("6. Verify attachment file extension is CSV", true, attachment.Name.Contains(".csv"));
                    }
                    else
                    {
                        Warning("6. Mail does not have Attachment");
                    }
                }
            });
            tasks.Add(task);
            Task.WaitAll(tasks.ToArray());

            try
            {
                reportManagerPage.DeleteReport(reportName);
            }
            catch { }
        }

        #endregion //Test Cases

        #region Private methods

        private string CreateQuickNewReportCurrentType(ReportManagerPage page, string reportName, string geozone = "")
        {
            if (!string.IsNullOrEmpty(geozone))
                page.GeozoneTreeMainPanel.SelectNode(geozone);
            page.GridPanel.ClickAddReportToolbarButton();
            page.WaitForPreviousActionComplete();
            page.ReportEditorPanel.WaitForPanelLoaded();
            page.ReportEditorPanel.EnterNameInput(reportName);
            var reportTemplate = page.ReportEditorPanel.GetTypeValue();
            page.ReportEditorPanel.ClickSaveButton();
            page.WaitForPreviousActionComplete();

            return reportTemplate;
        }

        private string CreateQuickNewReportRandomType(ReportManagerPage page, string reportName, string geozone = "")
        {
            if (!string.IsNullOrEmpty(geozone))
                page.GeozoneTreeMainPanel.SelectNode(geozone);
            page.GridPanel.ClickAddReportToolbarButton();
            page.WaitForPreviousActionComplete();
            page.ReportEditorPanel.WaitForPanelLoaded();
            page.ReportEditorPanel.EnterNameInput(reportName);
            var listTypes = page.ReportEditorPanel.GetListOfReportTypes();
            var reportTemplate = listTypes.PickRandom();
            page.ReportEditorPanel.SelectTypeDropDown(reportTemplate);
            page.WaitForPreviousActionComplete();
            page.ReportEditorPanel.ClickSaveButton();
            page.WaitForPreviousActionComplete();

            return reportTemplate;
        }

        private string GetHourServer(string localHour)
        {
            int localHourInt = int.Parse(localHour);
            localHourInt = localHourInt == 0 ? 24 : localHourInt;
            var serverTimeZone = Settings.GetServerTimeZone();
            var localTimeZone = Settings.GetLocalTimeZone();

            var diffHour = (localTimeZone - serverTimeZone);
            var newServerHour = localHourInt - diffHour;

            if (newServerHour == 0) return "00";
            if (newServerHour < 0) return (24 - (diffHour - localHourInt)).ToString("D2");

            return newServerHour.ToString("D2");
        }

        private string GetTimeServer(string localTime)
        {
            var localHour = localTime.SplitAndGetAt(new char[] { ':' }, 0);
            int localHourInt = int.Parse(localHour);
            localHourInt = localHourInt == 0 ? 24 : localHourInt;
            var serverTimeZone = Settings.GetServerTimeZone();
            var localTimeZone = Settings.GetLocalTimeZone();

            var diffHour = (localTimeZone - serverTimeZone);
            var newServerHour = localHourInt - diffHour;

            if (newServerHour == 0) return "00:00";
            if (newServerHour < 0) return string.Format("{0:D2}:00", (24 - (diffHour - localHourInt)));

            return string.Format("{0:D2}:00", newServerHour);
        }

        #region Verify methods

        private void VerifyCitigisReportDetails(ReportManagerPage page)
        {
            var expectedTabs = new List<string> { "Properties", "Export" };
            var expectedRange00_23 = SLVHelper.GenerateListStringInterger(0, 23);
            var expectedRange00_59 = SLVHelper.GenerateListStringInterger(0, 59);
            var expectedPeriodicity = new List<string> { "5 minutes", "10 minutes", "15 minutes", "30 minutes", "1 hour", "2 hours", "3 hours", "6 hours", "12 hours" };

            VerifyEqual("Verify Name is displayed", true, page.ReportEditorPanel.IsNameFieldDisplayed());
            VerifyEqual("Verify Type is displayed", true, page.ReportEditorPanel.IsTypeFieldDisplayed());

            var tabs = page.ReportEditorPanel.GetListOfTabsName();
            VerifyEqual("Verify Tabs as expected", expectedTabs, tabs);

            page.ReportEditorPanel.SelectTab("Properties");
            var expectedPropertiesGroups = new List<string> { "Configuration" };
            var groups = page.ReportEditorPanel.GetListOfGroupsNameActiveTab();
            VerifyEqual("Verify Groups as expected", expectedPropertiesGroups, groups);
            var propertiesTabFields = page.ReportEditorPanel.GetPropertiesAndItemsActiveTab("Lamp failures", "Comm. failures");
            VerifyEqual("Verify Description is displayed", true, propertiesTabFields.ContainsKey("description"));
            var isFromHourDisplayed = propertiesTabFields.ContainsKey("From (hour,0-24)");
            var isFromMinuteDisplayed = propertiesTabFields.ContainsKey("From (minute,0-59)");
            var isToHourDisplayed = propertiesTabFields.ContainsKey("To (hour,0-24)");
            var isToMinuteDisplayed = propertiesTabFields.ContainsKey("To (minute,0-59)");
            var isPeriodicityMinuteDisplayed = propertiesTabFields.ContainsKey("Periodicity (minutes)");
            VerifyEqual("Verify From (hour,0-24) is displayed", true, isFromHourDisplayed);
            if (isFromHourDisplayed) VerifyEqual("Verify From (hour,0-24) is range [00-23]", expectedRange00_23, (List<string>)propertiesTabFields["From (hour,0-24)"]);
            VerifyEqual("Verify From (minute,0-59) is displayed", true, isFromMinuteDisplayed);
            if (isFromMinuteDisplayed) VerifyEqual("Verify From (minute,0-59) is range [00-59]", expectedRange00_59, (List<string>)propertiesTabFields["From (minute,0-59)"]);
            VerifyEqual("Verify To (hour,0-24) is displayed", true, isToHourDisplayed);
            if (isToHourDisplayed) VerifyEqual("Verify To (hour,0-24) is range [00-23]", expectedRange00_23, (List<string>)propertiesTabFields["To (hour,0-24)"]);
            VerifyEqual("Verify To (minute,0-59) is displayed", true, isToMinuteDisplayed);
            if (isToMinuteDisplayed) VerifyEqual("Verify To (minute,0-59) is range [00-59]", expectedRange00_59, (List<string>)propertiesTabFields["To (minute,0-59)"]);
            VerifyEqual("Verify Periodicity (minutes) is displayed", true, isPeriodicityMinuteDisplayed);
            if (isPeriodicityMinuteDisplayed) VerifyEqual("Verify Periodicity (minutes) is range as expected", expectedPeriodicity, (List<string>)propertiesTabFields["Periodicity (minutes)"]);
            VerifyEqual("Verify Lamp failures is displayed", true, propertiesTabFields.ContainsKey("Lamp failures"));
            VerifyEqual("Verify Lamp threshold is displayed", true, propertiesTabFields.ContainsKey("Lamp threshold"));
            VerifyEqual("Verify Comm. failures is displayed", true, propertiesTabFields.ContainsKey("Comm. failures"));
            VerifyEqual("Verify Comm. threshold is displayed", true, propertiesTabFields.ContainsKey("Comm. threshold"));

            page.ReportEditorPanel.SelectTab("Export");
            var expectedExportGroups = new List<string> { "FTP" };
            groups = page.ReportEditorPanel.GetListOfGroupsNameActiveTab();
            VerifyEqual("Verify Groups as expected", expectedExportGroups, groups);
            var exportTabFields = page.ReportEditorPanel.GetPropertiesAndItemsActiveTab();
            VerifyEqual("Verify Host is displayed", true, exportTabFields.ContainsKey("Host"));
            VerifyEqual("Verify SFTP mode is displayed", true, exportTabFields.ContainsKey("SFTP mode"));
            VerifyEqual("Verify User is displayed", true, exportTabFields.ContainsKey("User"));
            VerifyEqual("Verify Password is displayed", true, exportTabFields.ContainsKey("Password"));
            VerifyEqual("Verify Directory is displayed", true, exportTabFields.ContainsKey("Directory"));
            VerifyEqual("Verify Passive mode is displayed", true, exportTabFields.ContainsKey("Passive mode"));
        }

        private void VerifyDayBurnReportDetails(ReportManagerPage page)
        {
            var expectedTabs = new List<string> { "Scheduler", "Export", "Report" };
            var expectedRange00_23 = SLVHelper.GenerateListStringInterger(0, 23);
            var expectedRange00_59 = SLVHelper.GenerateListStringInterger(0, 59);
            var expectedPeriodicity = new List<string> { "Every day", "Every monday", "Every tuesday", "Every wednesday", "Every thursday", "Every friday", "Every saturday", "Every sunday" };
            var expectedFormats = new List<string> { "CSV", "Html", "Plain text" };

            VerifyEqual("Verify Name is displayed", true, page.ReportEditorPanel.IsNameFieldDisplayed());
            VerifyEqual("Verify Type is displayed", true, page.ReportEditorPanel.IsTypeFieldDisplayed());

            var tabs = page.ReportEditorPanel.GetListOfTabsName();
            VerifyEqual("Verify Tabs as expected", expectedTabs, tabs);

            page.ReportEditorPanel.SelectTab("Scheduler");
            var expectedSchedulerGroups = new List<string> { "Configuration" };
            var groups = page.ReportEditorPanel.GetListOfGroupsNameActiveTab();
            VerifyEqual("Verify Groups as expected", expectedSchedulerGroups, groups);
            var schedulerTabFields = page.ReportEditorPanel.GetPropertiesAndItemsActiveTab();
            var isHourDisplayed = schedulerTabFields.ContainsKey("Hour (HH)");
            var isMinuteDisplayed = schedulerTabFields.ContainsKey("Minute");
            var isPeriodicityMinuteDisplayed = schedulerTabFields.ContainsKey("Periodicity");
            VerifyEqual("Verify Hour (HH) is displayed", true, isHourDisplayed);
            if (isHourDisplayed) VerifyEqual("Verify Hour (HH) is range [00-23]", expectedRange00_23, (List<string>)schedulerTabFields["Hour (HH)"]);
            VerifyEqual("[SC-597] Verify Minute is displayed", true, isMinuteDisplayed);
            if (isMinuteDisplayed) VerifyEqual("Verify Minute is range [00-59]", expectedRange00_59, (List<string>)schedulerTabFields["Minute"]);
            VerifyEqual("Verify Periodicity is displayed", true, isPeriodicityMinuteDisplayed);
            if (isPeriodicityMinuteDisplayed) VerifyEqual("Verify Periodicity is range as expected", expectedPeriodicity, (List<string>)schedulerTabFields["Periodicity"]);

            page.ReportEditorPanel.SelectTab("Export");
            var expectedExportGroups = new List<string> { "FTP", "Mail" };
            groups = page.ReportEditorPanel.GetListOfGroupsNameActiveTab();
            VerifyEqual("Verify Groups as expected", expectedExportGroups, groups);
            var exportTabFields = page.ReportEditorPanel.GetPropertiesAndItemsActiveTab();
            //FTP
            VerifyEqual("Verify FTP Host is displayed", true, exportTabFields.ContainsKey("FTP Host"));
            VerifyEqual("Verify SFTP mode is displayed", true, exportTabFields.ContainsKey("SFTP mode"));
            VerifyEqual("Verify FTP user is displayed", true, exportTabFields.ContainsKey("FTP user"));
            VerifyEqual("Verify FTP password is displayed", true, exportTabFields.ContainsKey("FTP password"));
            VerifyEqual("Verify FTP file name is displayed", true, exportTabFields.ContainsKey("FTP file name"));
            VerifyEqual("[SC-596] Verify FTP passive mode is displayed", true, exportTabFields.ContainsKey("FTP passive mode"));
            VerifyEqual("Verify FTP enabled is displayed", true, exportTabFields.ContainsKey("FTP enabled"));
            var isFTPFormatDisplayed = exportTabFields.ContainsKey("FTP format");
            VerifyEqual("Verify FTP format is displayed", true, isFTPFormatDisplayed);
            if (isFTPFormatDisplayed) VerifyEqual("Verify FTP format is range as expected", expectedFormats, (List<string>)exportTabFields["FTP format"]);
            //Mail
            VerifyEqual("Verify Mail enabled is displayed", true, exportTabFields.ContainsKey("Mail enabled"));
            var isMailFormatDisplayed = exportTabFields.ContainsKey("Mail format");
            VerifyEqual("Verify Mail format is displayed", true, isMailFormatDisplayed);
            if (isFTPFormatDisplayed) VerifyEqual("Verify Mail format is range as expected", expectedFormats, (List<string>)exportTabFields["Mail format"]);
            VerifyEqual("Verify Report in mail's body is displayed", true, exportTabFields.ContainsKey("Report in mail's body"));
            VerifyEqual("Verify Subject is displayed", true, exportTabFields.ContainsKey("Subject"));
            VerifyEqual("Verify From is displayed", true, exportTabFields.ContainsKey("From"));
            VerifyEqual("Verify Contacts is displayed as list", true, exportTabFields.ContainsKey("Contacts") && ((List<string>)exportTabFields["Contacts"]).Any());

            page.ReportEditorPanel.SelectTab("Report");
            var expectedReportGroups = new List<string> { "Configuration" };
            groups = page.ReportEditorPanel.GetListOfGroupsNameActiveTab();
            VerifyEqual("Verify Groups as expected", expectedReportGroups, groups);
            var reportTabFields = page.ReportEditorPanel.GetPropertiesAndItemsActiveTab();
            VerifyEqual("Verify Device Categories is displayed", true, reportTabFields.ContainsKey("Device Categories") && ((List<string>)reportTabFields["Device Categories"]).Any());
            VerifyEqual("Verify Recurse is displayed", true, reportTabFields.ContainsKey("Recurse"));
        }

        private void VerifyFailuresHTMLReportDetails(ReportManagerPage page)
        {
            var expectedTabs = new List<string> { "Properties", "Scheduler", "Export" };
            var expectedRange00_23 = SLVHelper.GenerateListStringInterger(0, 23);
            var expectedRange00_59 = SLVHelper.GenerateListStringInterger(0, 59);
            var expectedPeriodicity = new List<string> { "Every day", "Every monday", "Every tuesday", "Every wednesday", "Every thursday", "Every friday", "Every saturday", "Every sunday" };
            var expectedReportDetails = new List<string> { "Auto", "Direct sub-geozones only", "All sub-geozones", "All devices" };
            var expectedFilteringModes = new List<string> { "No filter", "Critical failures and warnings only", "Critical failures only" };

            VerifyEqual("Verify Name is displayed", true, page.ReportEditorPanel.IsNameFieldDisplayed());
            VerifyEqual("Verify Type is displayed", true, page.ReportEditorPanel.IsTypeFieldDisplayed());

            var tabs = page.ReportEditorPanel.GetListOfTabsName();
            VerifyEqual("Verify Tabs as expected", expectedTabs, tabs);

            page.ReportEditorPanel.SelectTab("Properties");
            var expectedPropertiesGroups = new List<string> { "Configuration" };
            var groups = page.ReportEditorPanel.GetListOfGroupsNameActiveTab();
            VerifyEqual("Verify Groups as expected", expectedPropertiesGroups, groups);
            var propertiesTabFields = page.ReportEditorPanel.GetPropertiesAndItemsActiveTab();
            VerifyEqual("Verify Description is displayed", true, propertiesTabFields.ContainsKey("description"));
            var isReportDetailsDisplayed = propertiesTabFields.ContainsKey("Report details");
            VerifyEqual("Verify Report details is displayed", true, isReportDetailsDisplayed);
            if (isReportDetailsDisplayed) VerifyEqual("Verify Report details is list as expected", expectedReportDetails, (List<string>)propertiesTabFields["Report details"]);
            var isFilteringModeDisplayed = propertiesTabFields.ContainsKey("Filtering mode");
            VerifyEqual("Verify Filtering mode is displayed", true, isFilteringModeDisplayed);
            if (isFilteringModeDisplayed) VerifyEqual("Verify Filtering mode is list as expected", expectedFilteringModes, (List<string>)propertiesTabFields["Filtering mode"]);

            page.ReportEditorPanel.SelectTab("Scheduler");
            var expectedSchedulerGroups = new List<string> { "Configuration" };
            groups = page.ReportEditorPanel.GetListOfGroupsNameActiveTab();
            VerifyEqual("Verify Groups as expected", expectedSchedulerGroups, groups);
            var schedulerTabFields = page.ReportEditorPanel.GetPropertiesAndItemsActiveTab();
            var isHourDisplayed = schedulerTabFields.ContainsKey("Hour (HH)");
            var isMinuteDisplayed = schedulerTabFields.ContainsKey("Minute");
            var isPeriodicityMinuteDisplayed = schedulerTabFields.ContainsKey("Periodicity");
            VerifyEqual("Verify Hour (HH) is displayed", true, isHourDisplayed);
            if (isHourDisplayed) VerifyEqual("Verify Hour (HH) is range [00-23]", expectedRange00_23, (List<string>)schedulerTabFields["Hour (HH)"]);
            VerifyEqual("[SC-597] Verify Minute is displayed", true, isMinuteDisplayed);
            if (isMinuteDisplayed) VerifyEqual("Verify Minute is range [00-59]", expectedRange00_59, (List<string>)schedulerTabFields["Minute"]);
            VerifyEqual("Verify Periodicity is displayed", true, isPeriodicityMinuteDisplayed);
            if (isPeriodicityMinuteDisplayed) VerifyEqual("Verify Periodicity is range as expected", expectedPeriodicity, (List<string>)schedulerTabFields["Periodicity"]);

            page.ReportEditorPanel.SelectTab("Export");
            var expectedExportGroups = new List<string> { "Mail", "Configuration" };
            groups = page.ReportEditorPanel.GetListOfGroupsNameActiveTab();
            VerifyEqual("Verify Groups as expected", expectedExportGroups, groups);
            var exportTabFields = page.ReportEditorPanel.GetPropertiesAndItemsActiveTab();
            //Mail           
            VerifyEqual("Verify Subject is displayed", true, exportTabFields.ContainsKey("Subject"));
            VerifyEqual("Verify From is displayed", true, exportTabFields.ContainsKey("From"));
            VerifyEqual("Verify Contacts is displayed as list", true, exportTabFields.ContainsKey("Contacts *") && ((List<string>)exportTabFields["Contacts *"]).Any());
            //Configuration      
            VerifyEqual("Verify HTML format is displayed", true, exportTabFields.ContainsKey("HTML format"));
        }

        private void VerifyFailuresReportDetails(ReportManagerPage page)
        {
            var expectedTabs = new List<string> { "Properties", "Scheduler", "Export" };
            var expectedRange00_23 = SLVHelper.GenerateListStringInterger(0, 23);
            var expectedRange00_59 = SLVHelper.GenerateListStringInterger(0, 59);

            VerifyEqual("Verify Name is displayed", true, page.ReportEditorPanel.IsNameFieldDisplayed());
            VerifyEqual("Verify Type is displayed", true, page.ReportEditorPanel.IsTypeFieldDisplayed());

            var tabs = page.ReportEditorPanel.GetListOfTabsName();
            VerifyEqual("Verify Tabs as expected", expectedTabs, tabs);

            page.ReportEditorPanel.SelectTab("Properties");
            var expectedPropertiesGroups = new List<string> { "Configuration" };
            var groups = page.ReportEditorPanel.GetListOfGroupsNameActiveTab();
            VerifyEqual("Verify Groups as expected", expectedPropertiesGroups, groups);
            var propertiesTabFields = page.ReportEditorPanel.GetPropertiesAndItemsActiveTab();
            VerifyEqual("Verify Description is displayed", true, propertiesTabFields.ContainsKey("description"));

            page.ReportEditorPanel.SelectTab("Scheduler");
            var expectedSchedulerGroups = new List<string> { "Configuration" };
            groups = page.ReportEditorPanel.GetListOfGroupsNameActiveTab();
            VerifyEqual("Verify Groups as expected", expectedSchedulerGroups, groups);
            var schedulerTabFields = page.ReportEditorPanel.GetPropertiesAndItemsActiveTab();
            var isHourDisplayed = schedulerTabFields.ContainsKey("Hour");
            var isMinuteDisplayed = schedulerTabFields.ContainsKey("Minute");
            VerifyEqual("Verify Hour is displayed", true, isHourDisplayed);
            if (isHourDisplayed) VerifyEqual("Verify Hour is range [00-23]", expectedRange00_23, (List<string>)schedulerTabFields["Hour"]);
            VerifyEqual("[SC-597] Verify Minute is displayed", true, isMinuteDisplayed);
            if (isMinuteDisplayed) VerifyEqual("Verify Minute is range [00-59]", expectedRange00_59, (List<string>)schedulerTabFields["Minute"]);

            page.ReportEditorPanel.SelectTab("Export");
            var expectedExportGroups = new List<string> { "FTP" };
            groups = page.ReportEditorPanel.GetListOfGroupsNameActiveTab();
            VerifyEqual("Verify Groups as expected", expectedExportGroups, groups);
            var exportTabFields = page.ReportEditorPanel.GetPropertiesAndItemsActiveTab();
            VerifyEqual("Verify FTP host is displayed", true, exportTabFields.ContainsKey("FTP host"));
            VerifyEqual("Verify SFTP mode is displayed", true, exportTabFields.ContainsKey("SFTP mode"));
            VerifyEqual("Verify FTP user is displayed", true, exportTabFields.ContainsKey("FTP user"));
            VerifyEqual("Verify FTP password is displayed", true, exportTabFields.ContainsKey("FTP password"));
            VerifyEqual("Verify FTP file path is displayed", true, exportTabFields.ContainsKey("FTP full file path"));
            VerifyEqual("Verify FTP passive mode is displayed", true, exportTabFields.ContainsKey("FTP passive mode"));
        }

        private void VerifyGenericDeviceLastValuesReportDetails(ReportManagerPage page)
        {
            var expectedTabs = new List<string> { "Scheduler", "Export", "Report" };
            var expectedRange00_23 = SLVHelper.GenerateListStringInterger(0, 23);
            var expectedRange00_59 = SLVHelper.GenerateListStringInterger(0, 59);
            var expectedPeriodicity = new List<string> { "Every day", "Every monday", "Every tuesday", "Every wednesday", "Every thursday", "Every friday", "Every saturday", "Every sunday" };
            var expectedFormats = new List<string> { "CSV", "Html", "Plain text" };

            VerifyEqual("Verify Name is displayed", true, page.ReportEditorPanel.IsNameFieldDisplayed());
            VerifyEqual("Verify Type is displayed", true, page.ReportEditorPanel.IsTypeFieldDisplayed());

            var tabs = page.ReportEditorPanel.GetListOfTabsName();
            VerifyEqual("Verify Tabs as expected", expectedTabs, tabs);

            page.ReportEditorPanel.SelectTab("Scheduler");
            var expectedSchedulerGroups = new List<string> { "Configuration" };
            var groups = page.ReportEditorPanel.GetListOfGroupsNameActiveTab();
            VerifyEqual("Verify Groups as expected", expectedSchedulerGroups, groups);
            var schedulerTabFields = page.ReportEditorPanel.GetPropertiesAndItemsActiveTab();
            var isHourDisplayed = schedulerTabFields.ContainsKey("Hour (HH)");
            var isMinuteDisplayed = schedulerTabFields.ContainsKey("Minute");
            var isPeriodicityMinuteDisplayed = schedulerTabFields.ContainsKey("Periodicity");
            VerifyEqual("Verify Hour (HH) is displayed", true, isHourDisplayed);
            if (isHourDisplayed) VerifyEqual("Verify Hour (HH) is range [00-23]", expectedRange00_23, (List<string>)schedulerTabFields["Hour (HH)"]);
            VerifyEqual("[SC-597] Verify Minute is displayed", true, isMinuteDisplayed);
            if (isMinuteDisplayed) VerifyEqual("Verify Minute is range [00-59]", expectedRange00_59, (List<string>)schedulerTabFields["Minute"]);
            VerifyEqual("Verify Periodicity is displayed", true, isPeriodicityMinuteDisplayed);
            if (isPeriodicityMinuteDisplayed) VerifyEqual("Verify Periodicity is range as expected", expectedPeriodicity, (List<string>)schedulerTabFields["Periodicity"]);

            page.ReportEditorPanel.SelectTab("Export");
            var expectedExportGroups = new List<string> { "FTP", "Mail" };
            groups = page.ReportEditorPanel.GetListOfGroupsNameActiveTab();
            VerifyEqual("Verify Groups as expected", expectedExportGroups, groups);
            var exportTabFields = page.ReportEditorPanel.GetPropertiesAndItemsActiveTab();
            //FTP
            VerifyEqual("Verify FTP host is displayed", true, exportTabFields.ContainsKey("FTP host"));
            VerifyEqual("Verify SFTP mode is displayed", true, exportTabFields.ContainsKey("SFTP mode"));
            VerifyEqual("Verify FTP user is displayed", true, exportTabFields.ContainsKey("FTP user"));
            VerifyEqual("Verify FTP password is displayed", true, exportTabFields.ContainsKey("FTP password"));
            VerifyEqual("Verify FTP filename is displayed", true, exportTabFields.ContainsKey("FTP filename"));
            VerifyEqual("[SC-596] Verify FTP passive mode is displayed", true, exportTabFields.ContainsKey("FTP passive mode"));
            VerifyEqual("Verify FTP enabled is displayed", true, exportTabFields.ContainsKey("FTP enabled"));
            var isFTPFormatDisplayed = exportTabFields.ContainsKey("FTP format");
            VerifyEqual("Verify FTP format is displayed", true, isFTPFormatDisplayed);
            if (isFTPFormatDisplayed) VerifyEqual("Verify FTP format is range as expected", expectedFormats, (List<string>)exportTabFields["FTP format"]);
            //Mail
            VerifyEqual("Verify Mail enabled is displayed", true, exportTabFields.ContainsKey("Mail enabled"));
            var isMailFormatDisplayed = exportTabFields.ContainsKey("Mail format");
            VerifyEqual("Verify Mail format is displayed", true, isMailFormatDisplayed);
            if (isFTPFormatDisplayed) VerifyEqual("Verify Mail format is range as expected", expectedFormats, (List<string>)exportTabFields["Mail format"]);
            VerifyEqual("Verify In mail body is displayed", true, exportTabFields.ContainsKey("In mail body"));
            VerifyEqual("Verify Subject is displayed", true, exportTabFields.ContainsKey("Subject"));
            VerifyEqual("Verify From is displayed", true, exportTabFields.ContainsKey("From"));
            VerifyEqual("Verify Contacts is displayed as list", true, exportTabFields.ContainsKey("Contacts") && ((List<string>)exportTabFields["Contacts"]).Any());

            page.ReportEditorPanel.SelectTab("Report");
            var expectedReportGroups = new List<string> { "Configuration" };
            groups = page.ReportEditorPanel.GetListOfGroupsNameActiveTab();
            VerifyEqual("Verify Groups as expected", expectedReportGroups, groups);
            var reportTabFields = page.ReportEditorPanel.GetPropertiesAndItemsActiveTab();
            VerifyEqual("Verify Categories is displayed as list", true, reportTabFields.ContainsKey("Categories") && ((List<string>)reportTabFields["Categories"]).Any());
            VerifyEqual("Verify Recurse in sub-geozones is displayed", true, reportTabFields.ContainsKey("Recurse in sub-geozones"));
            VerifyEqual("Verify Values is displayed as list", true, reportTabFields.ContainsKey("Values") && ((List<string>)reportTabFields["Values"]).Any());
        }

        private void VerifyGenericDeviceValuesReportDetails(ReportManagerPage page)
        {
            var expectedTabs = new List<string> { "Scheduler", "Export", "Report" };
            var expectedRange00_23 = SLVHelper.GenerateListStringInterger(0, 23);
            var expectedRange00_59 = SLVHelper.GenerateListStringInterger(0, 59);
            var expectedPeriodicity = new List<string> { "Every day", "Every monday", "Every tuesday", "Every wednesday", "Every thursday", "Every friday", "Every saturday", "Every sunday" };
            var expectedFormats = new List<string> { "CSV", "Html", "Plain text" };

            VerifyEqual("Verify Name is displayed", true, page.ReportEditorPanel.IsNameFieldDisplayed());
            VerifyEqual("Verify Type is displayed", true, page.ReportEditorPanel.IsTypeFieldDisplayed());

            var tabs = page.ReportEditorPanel.GetListOfTabsName();
            VerifyEqual("Verify Tabs as expected", expectedTabs, tabs);

            page.ReportEditorPanel.SelectTab("Scheduler");
            var expectedSchedulerGroups = new List<string> { "Configuration" };
            var groups = page.ReportEditorPanel.GetListOfGroupsNameActiveTab();
            VerifyEqual("Verify Groups as expected", expectedSchedulerGroups, groups);
            var schedulerTabFields = page.ReportEditorPanel.GetPropertiesAndItemsActiveTab();
            var isHourDisplayed = schedulerTabFields.ContainsKey("Hour (HH)");
            var isMinuteDisplayed = schedulerTabFields.ContainsKey("Minute");
            var isPeriodicityMinuteDisplayed = schedulerTabFields.ContainsKey("Periodicity");
            VerifyEqual("Verify Hour (HH) is displayed", true, isHourDisplayed);
            if (isHourDisplayed) VerifyEqual("Verify Hour (HH) is range [00-23]", expectedRange00_23, (List<string>)schedulerTabFields["Hour (HH)"]);
            VerifyEqual("[SC-597] Verify Minute is displayed", true, isMinuteDisplayed);
            if (isMinuteDisplayed) VerifyEqual("Verify Minute is range [00-59]", expectedRange00_59, (List<string>)schedulerTabFields["Minute"]);
            VerifyEqual("Verify Periodicity is displayed", true, isPeriodicityMinuteDisplayed);
            if (isPeriodicityMinuteDisplayed) VerifyEqual("Verify Periodicity is range as expected", expectedPeriodicity, (List<string>)schedulerTabFields["Periodicity"]);

            page.ReportEditorPanel.SelectTab("Export");
            var expectedExportGroups = new List<string> { "FTP", "Mail" };
            groups = page.ReportEditorPanel.GetListOfGroupsNameActiveTab();
            VerifyEqual("Verify Groups as expected", expectedExportGroups, groups);
            var exportTabFields = page.ReportEditorPanel.GetPropertiesAndItemsActiveTab();
            //FTP
            VerifyEqual("Verify FTP host is displayed", true, exportTabFields.ContainsKey("FTP host"));
            VerifyEqual("Verify SFTP mode is displayed", true, exportTabFields.ContainsKey("SFTP mode"));
            VerifyEqual("Verify FTP user is displayed", true, exportTabFields.ContainsKey("FTP user"));
            VerifyEqual("Verify FTP password is displayed", true, exportTabFields.ContainsKey("FTP password"));
            VerifyEqual("Verify FTP filename is displayed", true, exportTabFields.ContainsKey("FTP filename"));
            VerifyEqual("[SC-596] Verify FTP passive mode is displayed", true, exportTabFields.ContainsKey("FTP passive mode"));
            VerifyEqual("Verify FTP enabled is displayed", true, exportTabFields.ContainsKey("FTP enabled"));
            var isFTPFormatDisplayed = exportTabFields.ContainsKey("FTP format");
            VerifyEqual("Verify FTP format is displayed", true, isFTPFormatDisplayed);
            if (isFTPFormatDisplayed) VerifyEqual("Verify FTP format is range as expected", expectedFormats, (List<string>)exportTabFields["FTP format"]);
            //Mail
            VerifyEqual("Verify Mail enabled is displayed", true, exportTabFields.ContainsKey("Mail enabled"));
            var isMailFormatDisplayed = exportTabFields.ContainsKey("Mail format");
            VerifyEqual("Verify Mail format is displayed", true, isMailFormatDisplayed);
            if (isFTPFormatDisplayed) VerifyEqual("Verify Mail format is range as expected", expectedFormats, (List<string>)exportTabFields["Mail format"]);
            VerifyEqual("Verify In mail body is displayed", true, exportTabFields.ContainsKey("In mail body"));
            VerifyEqual("Verify Subject is displayed", true, exportTabFields.ContainsKey("Subject"));
            VerifyEqual("Verify From is displayed", true, exportTabFields.ContainsKey("From"));
            VerifyEqual("Verify Contacts is displayed as list", true, exportTabFields.ContainsKey("Contacts") && ((List<string>)exportTabFields["Contacts"]).Any());

            page.ReportEditorPanel.SelectTab("Report");
            var expectedReportGroups = new List<string> { "Configuration" };
            groups = page.ReportEditorPanel.GetListOfGroupsNameActiveTab();
            VerifyEqual("Verify Groups as expected", expectedReportGroups, groups);
            var reportTabFields = page.ReportEditorPanel.GetPropertiesAndItemsActiveTab();
            VerifyEqual("Verify Categories is displayed as list", true, reportTabFields.ContainsKey("Categories") && ((List<string>)reportTabFields["Categories"]).Any());
            VerifyEqual("Verify Meanings is displayed as list", true, reportTabFields.ContainsKey("Meanings") && ((List<string>)reportTabFields["Meanings"]).Any());
            VerifyEqual("Verify Last value only is displayed", true, reportTabFields.ContainsKey("Last value only"));
        }

        private void VerifyLatencyReportDetails(ReportManagerPage page)
        {
            var expectedTabs = new List<string> { "Export", "Scheduler", "Report" };
            var expectedRange00_23 = SLVHelper.GenerateListStringInterger(0, 23);
            var expectedRange00_59 = SLVHelper.GenerateListStringInterger(0, 59);
            var expectedPeriodicity = new List<string> { "Every day", "Every monday", "Every tuesday", "Every wednesday", "Every thursday", "Every friday", "Every saturday", "Every sunday" };
            var expectedFormats = new List<string> { "CSV", "Html", "Plain text" };
            var expectedCommands = new List<string> { "Lamp level command", "Lamp switch command" };
            var expectedFeedbacks = new List<string> { "Lamp level feedback", "Lamp switch feedback" };
            var expectedModes = new List<string> { "Lamp command mode", "Light control mode" };
            var expectedFromToModes = new List<string> { "Fixed local time", "Last hours" };
            var expectedLastHours = new List<string> { "1 hour", "2 hours", "3 hours", "4 hours", "5 hours", "6 hours", "7 hours", "8 hours", "9 hours", "10 hours", "11 hours", "12 hours" };

            var expectedTime00_23 = SLVHelper.GenerateListStringTime(0, 23);

            VerifyEqual("Verify Name is displayed", true, page.ReportEditorPanel.IsNameFieldDisplayed());
            VerifyEqual("Verify Type is displayed", true, page.ReportEditorPanel.IsTypeFieldDisplayed());

            var tabs = page.ReportEditorPanel.GetListOfTabsName();
            VerifyEqual("[SC-599] Verify Tabs as expected", expectedTabs, tabs);

            page.ReportEditorPanel.SelectTab("Scheduler");
            var expectedSchedulerGroups = new List<string> { "Configuration" };
            var groups = page.ReportEditorPanel.GetListOfGroupsNameActiveTab();
            VerifyEqual("Verify Groups as expected", expectedSchedulerGroups, groups);
            var schedulerTabFields = page.ReportEditorPanel.GetPropertiesAndItemsActiveTab();
            var isHourDisplayed = schedulerTabFields.ContainsKey("Hour (HH)");
            var isMinuteDisplayed = schedulerTabFields.ContainsKey("Minute");
            var isPeriodicityMinuteDisplayed = schedulerTabFields.ContainsKey("Periodicity");
            VerifyEqual("Verify Hour (HH) is displayed", true, isHourDisplayed);
            if (isHourDisplayed) VerifyEqual("Verify Hour (HH) is range [00-23]", expectedRange00_23, (List<string>)schedulerTabFields["Hour (HH)"]);
            VerifyEqual("[SC-597] Verify Minute is displayed", true, isMinuteDisplayed);
            if (isMinuteDisplayed) VerifyEqual("Verify Minute is range [00-59]", expectedRange00_59, (List<string>)schedulerTabFields["Minute"]);
            VerifyEqual("Verify Periodicity is displayed", true, isPeriodicityMinuteDisplayed);
            if (isPeriodicityMinuteDisplayed) VerifyEqual("Verify Periodicity is range as expected", expectedPeriodicity, (List<string>)schedulerTabFields["Periodicity"]);

            page.ReportEditorPanel.SelectTab("Export");
            var expectedExportGroups = new List<string> { "FTP", "Mail" };
            groups = page.ReportEditorPanel.GetListOfGroupsNameActiveTab();
            VerifyEqual("Verify Groups as expected", expectedExportGroups, groups);
            var exportTabFields = page.ReportEditorPanel.GetPropertiesAndItemsActiveTab();
            //FTP
            VerifyEqual("Verify FTP Host is displayed", true, exportTabFields.ContainsKey("FTP Host"));
            VerifyEqual("Verify SFTP mode is displayed", true, exportTabFields.ContainsKey("SFTP mode"));
            VerifyEqual("Verify FTP user is displayed", true, exportTabFields.ContainsKey("FTP user"));
            VerifyEqual("Verify FTP password is displayed", true, exportTabFields.ContainsKey("FTP password"));
            VerifyEqual("Verify FTP file name is displayed", true, exportTabFields.ContainsKey("FTP file name"));
            VerifyEqual("[SC-596] Verify FTP passive mode is displayed", true, exportTabFields.ContainsKey("FTP passive mode"));
            VerifyEqual("Verify FTP enabled is displayed", true, exportTabFields.ContainsKey("FTP enabled"));
            var isFTPFormatDisplayed = exportTabFields.ContainsKey("FTP format");
            VerifyEqual("Verify FTP format is displayed", true, isFTPFormatDisplayed);
            if (isFTPFormatDisplayed) VerifyEqual("Verify FTP format is range as expected", expectedFormats, (List<string>)exportTabFields["FTP format"]);
            //Mail
            VerifyEqual("Verify Mail enabled is displayed", true, exportTabFields.ContainsKey("Mail enabled"));
            var isMailFormatDisplayed = exportTabFields.ContainsKey("Mail format");
            VerifyEqual("Verify Mail format is displayed", true, isMailFormatDisplayed);
            if (isFTPFormatDisplayed) VerifyEqual("Verify Mail format is range as expected", expectedFormats, (List<string>)exportTabFields["Mail format"]);
            VerifyEqual("Verify Report in mail's body is displayed", true, exportTabFields.ContainsKey("Report in mail's body"));
            VerifyEqual("Verify Subject is displayed", true, exportTabFields.ContainsKey("Subject"));
            VerifyEqual("Verify From is displayed", true, exportTabFields.ContainsKey("From"));
            VerifyEqual("Verify Contacts is displayed as list", true, exportTabFields.ContainsKey("Contacts") && ((List<string>)exportTabFields["Contacts"]).Any());

            page.ReportEditorPanel.SelectTab("Report");
            var expectedReportGroups = new List<string> { "Configuration" };
            groups = page.ReportEditorPanel.GetListOfGroupsNameActiveTab();
            VerifyEqual("Verify Groups as expected", expectedReportGroups, groups);
            var reportTabFields = page.ReportEditorPanel.GetPropertiesAndItemsActiveTab();
            VerifyEqual("Verify Device Categories is displayed", true, reportTabFields.ContainsKey("Device Categories") && ((List<string>)reportTabFields["Device Categories"]).Any());
            VerifyEqual("Verify Recurse is displayed", true, reportTabFields.ContainsKey("Recurse"));

            var isCommandDisplayed = reportTabFields.ContainsKey("Command");
            VerifyEqual("Verify Command is displayed", true, isCommandDisplayed);
            if (isCommandDisplayed) VerifyEqual("Verify Command is range as expected", expectedCommands, (List<string>)reportTabFields["Command"]);

            var isFeedbackDisplayed = reportTabFields.ContainsKey("Feedback");
            VerifyEqual("Verify Feedback is displayed", true, isFeedbackDisplayed);
            if (isFeedbackDisplayed) VerifyEqual("Verify Feedback is range as expected", expectedFeedbacks, (List<string>)reportTabFields["Feedback"]);

            var isModeDisplayed = reportTabFields.ContainsKey("modeMeaningStrId");
            VerifyTrue("[SC-597] Verify modeMeaningStrId is not displayed (replaced by a meaning label)", isModeDisplayed == false, false, isModeDisplayed);            
            if (isModeDisplayed) VerifyEqual("Verify modeMeaningStrId is range as expected", expectedModes, (List<string>)reportTabFields["modeMeaningStrId"]);

            var isFromToModeDisplayed = reportTabFields.ContainsKey("From/To mode");
            VerifyEqual("Verify From/To mode is displayed", true, isFromToModeDisplayed);
            if (isFromToModeDisplayed) VerifyEqual("Verify From/To mode is range as expected", expectedFromToModes, (List<string>)reportTabFields["From/To mode"]);

            var isFromLastHourDisplayed = reportTabFields.ContainsKey("From last hours");
            VerifyEqual("Verify From last hours is displayed", true, isFromLastHourDisplayed);
            if (isFromLastHourDisplayed) VerifyEqual("Verify From last hours is range as expected", expectedLastHours, (List<string>)reportTabFields["From last hours"], false);

            var isFromLocalTimeDisplayed = reportTabFields.ContainsKey("From (localtime)");
            VerifyEqual("Verify From (localtime) is displayed", true, isFromLocalTimeDisplayed);
            if (isFromLocalTimeDisplayed) VerifyEqual("Verify From (localtime) is range as expected", expectedTime00_23, (List<string>)reportTabFields["From (localtime)"]);

            var isToLocalTimeDisplayed = reportTabFields.ContainsKey("To (localtime)");
            VerifyEqual("Verify To (localtime) is displayed", true, isToLocalTimeDisplayed);
            if (isToLocalTimeDisplayed) VerifyEqual("[SC-601] Verify To (local time) is range as expected", expectedTime00_23, (List<string>)reportTabFields["To (localtime)"]);

            VerifyEqual("Verify Command value is displayed", true, reportTabFields.ContainsKey("Command value"));
        }

        private void VerifyLifetimeReportDetails(ReportManagerPage page)
        {
            var expectedTabs = new List<string> { "Properties", "Scheduler", "Export" };
            var expectedRange00_23 = SLVHelper.GenerateListStringInterger(0, 23);
            var expectedRange00_59 = SLVHelper.GenerateListStringInterger(0, 59);
            var expectedPeriodicity = new List<string> { "Every day", "Every monday", "Every tuesday", "Every wednesday", "Every thursday", "Every friday", "Every saturday", "Every sunday" };

            VerifyEqual("Verify Name is displayed", true, page.ReportEditorPanel.IsNameFieldDisplayed());
            VerifyEqual("Verify Type is displayed", true, page.ReportEditorPanel.IsTypeFieldDisplayed());

            var tabs = page.ReportEditorPanel.GetListOfTabsName();
            VerifyEqual("Verify Tabs as expected", expectedTabs, tabs);

            page.ReportEditorPanel.SelectTab("Properties");
            var expectedPropertiesGroups = new List<string> { "Configuration" };
            var groups = page.ReportEditorPanel.GetListOfGroupsNameActiveTab();
            VerifyEqual("Verify Groups as expected", expectedPropertiesGroups, groups);
            var propertiesTabFields = page.ReportEditorPanel.GetPropertiesAndItemsActiveTab();
            VerifyEqual("Verify Description is displayed", true, propertiesTabFields.ContainsKey("description"));
            VerifyEqual("Verify Critical lifetime (%, 0-100) is displayed", true, propertiesTabFields.ContainsKey("Critical lifetime (%, 0-100)"));

            page.ReportEditorPanel.SelectTab("Scheduler");
            var expectedSchedulerGroups = new List<string> { "Configuration" };
            groups = page.ReportEditorPanel.GetListOfGroupsNameActiveTab();
            VerifyEqual("Verify Groups as expected", expectedSchedulerGroups, groups);
            var schedulerTabFields = page.ReportEditorPanel.GetPropertiesAndItemsActiveTab();
            var isHourDisplayed = schedulerTabFields.ContainsKey("Hour (HH)");
            var isMinuteDisplayed = schedulerTabFields.ContainsKey("Minute");
            var isPeriodicityMinuteDisplayed = schedulerTabFields.ContainsKey("Periodicity");
            VerifyEqual("Verify Hour (HH) is displayed", true, isHourDisplayed);
            if (isHourDisplayed) VerifyEqual("Verify Hour (HH) is range [00-23]", expectedRange00_23, (List<string>)schedulerTabFields["Hour (HH)"]);
            VerifyEqual("[SC-597] Verify Minute is displayed", true, isMinuteDisplayed);
            if (isMinuteDisplayed) VerifyEqual("Verify Minute is range [00-59]", expectedRange00_59, (List<string>)schedulerTabFields["Minute"]);
            VerifyEqual("Verify Periodicity is displayed", true, isPeriodicityMinuteDisplayed);
            if (isPeriodicityMinuteDisplayed) VerifyEqual("Verify Periodicity is range as expected", expectedPeriodicity, (List<string>)schedulerTabFields["Periodicity"]);

            page.ReportEditorPanel.SelectTab("Export");
            var expectedExportGroups = new List<string> { "Mail", "Configuration" };
            groups = page.ReportEditorPanel.GetListOfGroupsNameActiveTab();
            VerifyEqual("Verify Groups as expected", expectedExportGroups, groups);
            var exportTabFields = page.ReportEditorPanel.GetPropertiesAndItemsActiveTab();
            //Mail           
            VerifyEqual("Verify Subject is displayed", true, exportTabFields.ContainsKey("Subject"));
            VerifyEqual("Verify From is displayed", true, exportTabFields.ContainsKey("From"));
            VerifyEqual("Verify Contacts is displayed as list", true, exportTabFields.ContainsKey("Contacts *") && ((List<string>)exportTabFields["Contacts *"]).Any());
            //Configuration      
            VerifyEqual("Verify HTML format is displayed", true, exportTabFields.ContainsKey("HTML format"));
        }

        private void VerifyLowPowerFactorReportDetails(ReportManagerPage page)
        {
            var expectedTabs = new List<string> { "Scheduler", "Export", "Report" };
            var expectedRange00_23 = SLVHelper.GenerateListStringInterger(0, 23);
            var expectedRange00_59 = SLVHelper.GenerateListStringInterger(0, 59);
            var expectedPeriodicity = new List<string> { "Every day", "Every monday", "Every tuesday", "Every wednesday", "Every thursday", "Every friday", "Every saturday", "Every sunday" };
            var expectedFormats = new List<string> { "CSV", "Html", "Plain text" };

            VerifyEqual("Verify Name is displayed", true, page.ReportEditorPanel.IsNameFieldDisplayed());
            VerifyEqual("Verify Type is displayed", true, page.ReportEditorPanel.IsTypeFieldDisplayed());

            var tabs = page.ReportEditorPanel.GetListOfTabsName();
            VerifyEqual("Verify Tabs as expected", expectedTabs, tabs);

            page.ReportEditorPanel.SelectTab("Scheduler");
            var expectedSchedulerGroups = new List<string> { "Configuration" };
            var groups = page.ReportEditorPanel.GetListOfGroupsNameActiveTab();
            VerifyEqual("Verify Groups as expected", expectedSchedulerGroups, groups);
            var schedulerTabFields = page.ReportEditorPanel.GetPropertiesAndItemsActiveTab();
            var isHourDisplayed = schedulerTabFields.ContainsKey("Hour (HH)");
            var isMinuteDisplayed = schedulerTabFields.ContainsKey("Minute");
            var isPeriodicityMinuteDisplayed = schedulerTabFields.ContainsKey("Periodicity");
            VerifyEqual("Verify Hour (HH) is displayed", true, isHourDisplayed);
            if (isHourDisplayed) VerifyEqual("Verify Hour (HH) is range [00-23]", expectedRange00_23, (List<string>)schedulerTabFields["Hour (HH)"]);
            VerifyEqual("[SC-597] Verify Minute is displayed", true, isMinuteDisplayed);
            if (isMinuteDisplayed) VerifyEqual("Verify Minute is range [00-59]", expectedRange00_59, (List<string>)schedulerTabFields["Minute"]);
            VerifyEqual("Verify Periodicity is displayed", true, isPeriodicityMinuteDisplayed);
            if (isPeriodicityMinuteDisplayed) VerifyEqual("Verify Periodicity is range as expected", expectedPeriodicity, (List<string>)schedulerTabFields["Periodicity"]);

            page.ReportEditorPanel.SelectTab("Export");
            var expectedExportGroups = new List<string> { "FTP", "Mail" };
            groups = page.ReportEditorPanel.GetListOfGroupsNameActiveTab();
            VerifyEqual("Verify Groups as expected", expectedExportGroups, groups);
            var exportTabFields = page.ReportEditorPanel.GetPropertiesAndItemsActiveTab();
            //FTP
            VerifyEqual("Verify FTP Host is displayed", true, exportTabFields.ContainsKey("FTP Host"));
            VerifyEqual("Verify SFTP mode is displayed", true, exportTabFields.ContainsKey("SFTP mode"));
            VerifyEqual("Verify FTP user is displayed", true, exportTabFields.ContainsKey("FTP user"));
            VerifyEqual("Verify FTP password is displayed", true, exportTabFields.ContainsKey("FTP password"));
            VerifyEqual("Verify FTP file name is displayed", true, exportTabFields.ContainsKey("FTP file name"));
            VerifyEqual("[SC-596] Verify FTP passive mode is displayed", true, exportTabFields.ContainsKey("FTP passive mode"));
            VerifyEqual("Verify FTP enabled is displayed", true, exportTabFields.ContainsKey("FTP enabled"));
            var isFTPFormatDisplayed = exportTabFields.ContainsKey("FTP format");
            VerifyEqual("Verify FTP format is displayed", true, isFTPFormatDisplayed);
            if (isFTPFormatDisplayed) VerifyEqual("Verify FTP format is range as expected", expectedFormats, (List<string>)exportTabFields["FTP format"]);
            //Mail
            VerifyEqual("Verify Mail enabled is displayed", true, exportTabFields.ContainsKey("Mail enabled"));
            var isMailFormatDisplayed = exportTabFields.ContainsKey("Mail format");
            VerifyEqual("Verify Mail format is displayed", true, isMailFormatDisplayed);
            if (isFTPFormatDisplayed) VerifyEqual("Verify Mail format is range as expected", expectedFormats, (List<string>)exportTabFields["Mail format"]);
            VerifyEqual("Verify Report in mail's body is displayed", true, exportTabFields.ContainsKey("Report in mail's body"));
            VerifyEqual("Verify Subject is displayed", true, exportTabFields.ContainsKey("Subject"));
            VerifyEqual("Verify From is displayed", true, exportTabFields.ContainsKey("From"));
            VerifyEqual("Verify Contacts is displayed as list", true, exportTabFields.ContainsKey("Contacts") && ((List<string>)exportTabFields["Contacts"]).Any());

            page.ReportEditorPanel.SelectTab("Report");
            var expectedReportGroups = new List<string> { "Configuration" };
            groups = page.ReportEditorPanel.GetListOfGroupsNameActiveTab();
            VerifyEqual("Verify Groups as expected", expectedReportGroups, groups);
            var reportTabFields = page.ReportEditorPanel.GetPropertiesAndItemsActiveTab();
            VerifyEqual("Verify Device Categories is displayed", true, reportTabFields.ContainsKey("Device Categories") && ((List<string>)reportTabFields["Device Categories"]).Any());
            VerifyEqual("Verify Recurse is displayed", true, reportTabFields.ContainsKey("Recurse"));
        }

        private void VerifyNoDataEverReceivedDetails(ReportManagerPage page)
        {
            var expectedTabs = new List<string> { "Scheduler", "Export", "Report" };
            var expectedRange00_23 = SLVHelper.GenerateListStringInterger(0, 23);
            var expectedRange00_59 = SLVHelper.GenerateListStringInterger(0, 59);
            var expectedPeriodicity = new List<string> { "Every day", "Every monday", "Every tuesday", "Every wednesday", "Every thursday", "Every friday", "Every saturday", "Every sunday" };
            var expectedFormats = new List<string> { "CSV", "Html", "Plain text" };

            VerifyEqual("Verify Name is displayed", true, page.ReportEditorPanel.IsNameFieldDisplayed());
            VerifyEqual("Verify Type is displayed", true, page.ReportEditorPanel.IsTypeFieldDisplayed());

            var tabs = page.ReportEditorPanel.GetListOfTabsName();
            VerifyEqual("Verify Tabs as expected", expectedTabs, tabs);

            page.ReportEditorPanel.SelectTab("Scheduler");
            var expectedSchedulerGroups = new List<string> { "Configuration" };
            var groups = page.ReportEditorPanel.GetListOfGroupsNameActiveTab();
            VerifyEqual("Verify Groups as expected", expectedSchedulerGroups, groups);
            var schedulerTabFields = page.ReportEditorPanel.GetPropertiesAndItemsActiveTab();
            var isHourDisplayed = schedulerTabFields.ContainsKey("Hour (HH)");
            var isMinuteDisplayed = schedulerTabFields.ContainsKey("Minute");
            var isPeriodicityMinuteDisplayed = schedulerTabFields.ContainsKey("Periodicity");
            VerifyEqual("Verify Hour (HH) is displayed", true, isHourDisplayed);
            if (isHourDisplayed) VerifyEqual("Verify Hour (HH) is range [00-23]", expectedRange00_23, (List<string>)schedulerTabFields["Hour (HH)"]);
            VerifyEqual("[SC-597] Verify Minute is displayed", true, isMinuteDisplayed);
            if (isMinuteDisplayed) VerifyEqual("Verify Minute is range [00-59]", expectedRange00_59, (List<string>)schedulerTabFields["Minute"]);
            VerifyEqual("Verify Periodicity is displayed", true, isPeriodicityMinuteDisplayed);
            if (isPeriodicityMinuteDisplayed) VerifyEqual("Verify Periodicity is range as expected", expectedPeriodicity, (List<string>)schedulerTabFields["Periodicity"]);

            page.ReportEditorPanel.SelectTab("Export");
            var expectedExportGroups = new List<string> { "FTP", "Mail" };
            groups = page.ReportEditorPanel.GetListOfGroupsNameActiveTab();
            VerifyEqual("Verify Groups as expected", expectedExportGroups, groups);
            var exportTabFields = page.ReportEditorPanel.GetPropertiesAndItemsActiveTab();
            //FTP
            VerifyEqual("Verify FTP Host is displayed", true, exportTabFields.ContainsKey("FTP Host"));
            VerifyEqual("Verify SFTP mode is displayed", true, exportTabFields.ContainsKey("SFTP mode"));
            VerifyEqual("Verify FTP user is displayed", true, exportTabFields.ContainsKey("FTP user"));
            VerifyEqual("Verify FTP password is displayed", true, exportTabFields.ContainsKey("FTP password"));
            VerifyEqual("Verify FTP file name is displayed", true, exportTabFields.ContainsKey("FTP file name"));
            VerifyEqual("[SC-596] Verify FTP passive mode is displayed", true, exportTabFields.ContainsKey("FTP passive mode"));
            VerifyEqual("Verify FTP enabled is displayed", true, exportTabFields.ContainsKey("FTP enabled"));
            var isFTPFormatDisplayed = exportTabFields.ContainsKey("FTP format");
            VerifyEqual("Verify FTP format is displayed", true, isFTPFormatDisplayed);
            if (isFTPFormatDisplayed) VerifyEqual("Verify FTP format is range as expected", expectedFormats, (List<string>)exportTabFields["FTP format"]);
            //Mail
            VerifyEqual("Verify Mail enabled is displayed", true, exportTabFields.ContainsKey("Mail enabled"));
            var isMailFormatDisplayed = exportTabFields.ContainsKey("Mail format");
            VerifyEqual("Verify Mail format is displayed", true, isMailFormatDisplayed);
            if (isFTPFormatDisplayed) VerifyEqual("Verify Mail format is range as expected", expectedFormats, (List<string>)exportTabFields["Mail format"]);
            VerifyEqual("Verify Report in mail's body is displayed", true, exportTabFields.ContainsKey("Report in mail's body"));
            VerifyEqual("Verify Subject is displayed", true, exportTabFields.ContainsKey("Subject"));
            VerifyEqual("Verify From is displayed", true, exportTabFields.ContainsKey("From"));
            VerifyEqual("Verify Contacts is displayed as list", true, exportTabFields.ContainsKey("Contacts") && ((List<string>)exportTabFields["Contacts"]).Any());

            page.ReportEditorPanel.SelectTab("Report");
            var expectedReportGroups = new List<string> { "Configuration" };
            groups = page.ReportEditorPanel.GetListOfGroupsNameActiveTab();
            VerifyEqual("Verify Groups as expected", expectedReportGroups, groups);
            var reportTabFields = page.ReportEditorPanel.GetPropertiesAndItemsActiveTab();
            VerifyEqual("Verify Device Categories is displayed", true, reportTabFields.ContainsKey("Device Categories") && ((List<string>)reportTabFields["Device Categories"]).Any());
            VerifyEqual("Verify Recurse is displayed", true, reportTabFields.ContainsKey("Recurse"));
        }

        private void VerifyOnOffSegmentReportDetails(ReportManagerPage page)
        {
            var expectedTabs = new List<string> { "Properties", "Scheduler", "Export" };
            var expectedRange00_23 = SLVHelper.GenerateListStringInterger(0, 23);
            var expectedRange00_59 = SLVHelper.GenerateListStringInterger(0, 59);
            var expectedPeriodicity = new List<string> { "Every day", "Every monday", "Every tuesday", "Every wednesday", "Every thursday", "Every friday", "Every saturday", "Every sunday" };

            VerifyEqual("Verify Name is displayed", true, page.ReportEditorPanel.IsNameFieldDisplayed());
            VerifyEqual("Verify Type is displayed", true, page.ReportEditorPanel.IsTypeFieldDisplayed());

            var tabs = page.ReportEditorPanel.GetListOfTabsName();
            VerifyEqual("Verify Tabs as expected", expectedTabs, tabs);

            page.ReportEditorPanel.SelectTab("Properties");
            var expectedPropertiesGroups = new List<string> { "Configuration" };
            var groups = page.ReportEditorPanel.GetListOfGroupsNameActiveTab();
            VerifyEqual("Verify Groups as expected", expectedPropertiesGroups, groups);
            var propertiesTabFields = page.ReportEditorPanel.GetPropertiesAndItemsActiveTab();
            VerifyEqual("Verify Description is displayed", true, propertiesTabFields.ContainsKey("description"));
            VerifyEqual("Verify Number of days is displayed", true, propertiesTabFields.ContainsKey("Number of days"));

            page.ReportEditorPanel.SelectTab("Scheduler");
            var expectedSchedulerGroups = new List<string> { "Configuration" };
            groups = page.ReportEditorPanel.GetListOfGroupsNameActiveTab();
            VerifyEqual("Verify Groups as expected", expectedSchedulerGroups, groups);
            var schedulerTabFields = page.ReportEditorPanel.GetPropertiesAndItemsActiveTab();
            var isHourDisplayed = schedulerTabFields.ContainsKey("Hour (HH)");
            var isMinuteDisplayed = schedulerTabFields.ContainsKey("Minute");
            var isPeriodicityMinuteDisplayed = schedulerTabFields.ContainsKey("Periodicity");
            VerifyEqual("Verify Hour (HH) is displayed", true, isHourDisplayed);
            if (isHourDisplayed) VerifyEqual("Verify Hour (HH) is range [00-23]", expectedRange00_23, (List<string>)schedulerTabFields["Hour (HH)"]);
            VerifyEqual("[SC-597] Verify Minute is displayed", true, isMinuteDisplayed);
            if (isMinuteDisplayed) VerifyEqual("Verify Minute is range [00-59]", expectedRange00_59, (List<string>)schedulerTabFields["Minute"]);
            VerifyEqual("Verify Periodicity is displayed", true, isPeriodicityMinuteDisplayed);
            if (isPeriodicityMinuteDisplayed) VerifyEqual("Verify Periodicity is range as expected", expectedPeriodicity, (List<string>)schedulerTabFields["Periodicity"]);

            page.ReportEditorPanel.SelectTab("Export");
            var expectedExportGroups = new List<string> { "Mail", "Configuration" };
            groups = page.ReportEditorPanel.GetListOfGroupsNameActiveTab();
            VerifyEqual("Verify Groups as expected", expectedExportGroups, groups);
            var exportTabFields = page.ReportEditorPanel.GetPropertiesAndItemsActiveTab();
            //Mail           
            VerifyEqual("Verify Subject is displayed", true, exportTabFields.ContainsKey("Subject"));
            VerifyEqual("Verify From is displayed", true, exportTabFields.ContainsKey("From"));
            VerifyEqual("Verify Contacts is displayed as list", true, exportTabFields.ContainsKey("Contacts *") && ((List<string>)exportTabFields["Contacts *"]).Any());
            //Configuration      
            VerifyEqual("Verify HTML format is displayed", true, exportTabFields.ContainsKey("HTML format"));
        }

        private void VerifyOver140VVoltageReportDetails(ReportManagerPage page)
        {
            var expectedTabs = new List<string> { "Scheduler", "Export", "Report" };
            var expectedRange00_23 = SLVHelper.GenerateListStringInterger(0, 23);
            var expectedRange00_59 = SLVHelper.GenerateListStringInterger(0, 59);
            var expectedPeriodicity = new List<string> { "Every day", "Every monday", "Every tuesday", "Every wednesday", "Every thursday", "Every friday", "Every saturday", "Every sunday" };
            var expectedFormats = new List<string> { "CSV", "Html", "Plain text" };

            VerifyEqual("Verify Name is displayed", true, page.ReportEditorPanel.IsNameFieldDisplayed());
            VerifyEqual("Verify Type is displayed", true, page.ReportEditorPanel.IsTypeFieldDisplayed());

            var tabs = page.ReportEditorPanel.GetListOfTabsName();
            VerifyEqual("Verify Tabs as expected", expectedTabs, tabs);

            page.ReportEditorPanel.SelectTab("Scheduler");
            var expectedSchedulerGroups = new List<string> { "Configuration" };
            var groups = page.ReportEditorPanel.GetListOfGroupsNameActiveTab();
            VerifyEqual("Verify Groups as expected", expectedSchedulerGroups, groups);
            var schedulerTabFields = page.ReportEditorPanel.GetPropertiesAndItemsActiveTab();
            var isHourDisplayed = schedulerTabFields.ContainsKey("Hour (HH)");
            var isMinuteDisplayed = schedulerTabFields.ContainsKey("Minute");
            var isPeriodicityMinuteDisplayed = schedulerTabFields.ContainsKey("Periodicity");
            VerifyEqual("Verify Hour (HH) is displayed", true, isHourDisplayed);
            if (isHourDisplayed) VerifyEqual("Verify Hour (HH) is range [00-23]", expectedRange00_23, (List<string>)schedulerTabFields["Hour (HH)"]);
            VerifyEqual("[SC-597] Verify Minute is displayed", true, isMinuteDisplayed);
            if (isMinuteDisplayed) VerifyEqual("Verify Minute is range [00-59]", expectedRange00_59, (List<string>)schedulerTabFields["Minute"]);
            VerifyEqual("Verify Periodicity is displayed", true, isPeriodicityMinuteDisplayed);
            if (isPeriodicityMinuteDisplayed) VerifyEqual("Verify Periodicity is range as expected", expectedPeriodicity, (List<string>)schedulerTabFields["Periodicity"]);

            page.ReportEditorPanel.SelectTab("Export");
            var expectedExportGroups = new List<string> { "FTP", "Mail" };
            groups = page.ReportEditorPanel.GetListOfGroupsNameActiveTab();
            VerifyEqual("Verify Groups as expected", expectedExportGroups, groups);
            var exportTabFields = page.ReportEditorPanel.GetPropertiesAndItemsActiveTab();
            //FTP
            VerifyEqual("Verify FTP Host is displayed", true, exportTabFields.ContainsKey("FTP Host"));
            VerifyEqual("Verify SFTP mode is displayed", true, exportTabFields.ContainsKey("SFTP mode"));
            VerifyEqual("Verify FTP user is displayed", true, exportTabFields.ContainsKey("FTP user"));
            VerifyEqual("Verify FTP password is displayed", true, exportTabFields.ContainsKey("FTP password"));
            VerifyEqual("Verify FTP file name is displayed", true, exportTabFields.ContainsKey("FTP file name"));
            VerifyEqual("[SC-596] Verify FTP passive mode is displayed", true, exportTabFields.ContainsKey("FTP passive mode"));
            VerifyEqual("Verify FTP enabled is displayed", true, exportTabFields.ContainsKey("FTP enabled"));
            var isFTPFormatDisplayed = exportTabFields.ContainsKey("FTP format");
            VerifyEqual("Verify FTP format is displayed", true, isFTPFormatDisplayed);
            if (isFTPFormatDisplayed) VerifyEqual("Verify FTP format is range as expected", expectedFormats, (List<string>)exportTabFields["FTP format"]);
            //Mail
            VerifyEqual("Verify Mail enabled is displayed", true, exportTabFields.ContainsKey("Mail enabled"));
            var isMailFormatDisplayed = exportTabFields.ContainsKey("Mail format");
            VerifyEqual("Verify Mail format is displayed", true, isMailFormatDisplayed);
            if (isFTPFormatDisplayed) VerifyEqual("Verify Mail format is range as expected", expectedFormats, (List<string>)exportTabFields["Mail format"]);
            VerifyEqual("Verify Report in mail's body is displayed", true, exportTabFields.ContainsKey("Report in mail's body"));
            VerifyEqual("Verify Subject is displayed", true, exportTabFields.ContainsKey("Subject"));
            VerifyEqual("Verify From is displayed", true, exportTabFields.ContainsKey("From"));
            VerifyEqual("Verify Contacts is displayed as list", true, exportTabFields.ContainsKey("Contacts") && ((List<string>)exportTabFields["Contacts"]).Any());

            page.ReportEditorPanel.SelectTab("Report");
            var expectedReportGroups = new List<string> { "Configuration" };
            groups = page.ReportEditorPanel.GetListOfGroupsNameActiveTab();
            VerifyEqual("Verify Groups as expected", expectedReportGroups, groups);
            var reportTabFields = page.ReportEditorPanel.GetPropertiesAndItemsActiveTab();
            VerifyEqual("Verify Device Categories is displayed", true, reportTabFields.ContainsKey("Device Categories") && ((List<string>)reportTabFields["Device Categories"]).Any());
            VerifyEqual("Verify Recurse is displayed", true, reportTabFields.ContainsKey("Recurse"));
        }

        private void VerifyOverVoltageReportDetails(ReportManagerPage page)
        {
            var expectedTabs = new List<string> { "Scheduler", "Export", "Report" };
            var expectedRange00_23 = SLVHelper.GenerateListStringInterger(0, 23);
            var expectedRange00_59 = SLVHelper.GenerateListStringInterger(0, 59);
            var expectedPeriodicity = new List<string> { "Every day", "Every monday", "Every tuesday", "Every wednesday", "Every thursday", "Every friday", "Every saturday", "Every sunday" };
            var expectedFormats = new List<string> { "CSV", "Html", "Plain text" };

            VerifyEqual("Verify Name is displayed", true, page.ReportEditorPanel.IsNameFieldDisplayed());
            VerifyEqual("Verify Type is displayed", true, page.ReportEditorPanel.IsTypeFieldDisplayed());

            var tabs = page.ReportEditorPanel.GetListOfTabsName();
            VerifyEqual("Verify Tabs as expected", expectedTabs, tabs);

            page.ReportEditorPanel.SelectTab("Scheduler");
            var expectedSchedulerGroups = new List<string> { "Configuration" };
            var groups = page.ReportEditorPanel.GetListOfGroupsNameActiveTab();
            VerifyEqual("Verify Groups as expected", expectedSchedulerGroups, groups);
            var schedulerTabFields = page.ReportEditorPanel.GetPropertiesAndItemsActiveTab();
            var isHourDisplayed = schedulerTabFields.ContainsKey("Hour (HH)");
            var isMinuteDisplayed = schedulerTabFields.ContainsKey("Minute");
            var isPeriodicityMinuteDisplayed = schedulerTabFields.ContainsKey("Periodicity");
            VerifyEqual("Verify Hour (HH) is displayed", true, isHourDisplayed);
            if (isHourDisplayed) VerifyEqual("Verify Hour (HH) is range [00-23]", expectedRange00_23, (List<string>)schedulerTabFields["Hour (HH)"]);
            VerifyEqual("[SC-597] Verify Minute is displayed", true, isMinuteDisplayed);
            if (isMinuteDisplayed) VerifyEqual("Verify Minute is range [00-59]", expectedRange00_59, (List<string>)schedulerTabFields["Minute"]);
            VerifyEqual("Verify Periodicity is displayed", true, isPeriodicityMinuteDisplayed);
            if (isPeriodicityMinuteDisplayed) VerifyEqual("Verify Periodicity is range as expected", expectedPeriodicity, (List<string>)schedulerTabFields["Periodicity"]);

            page.ReportEditorPanel.SelectTab("Export");
            var expectedExportGroups = new List<string> { "FTP", "Mail" };
            groups = page.ReportEditorPanel.GetListOfGroupsNameActiveTab();
            VerifyEqual("Verify Groups as expected", expectedExportGroups, groups);
            var exportTabFields = page.ReportEditorPanel.GetPropertiesAndItemsActiveTab();
            //FTP
            VerifyEqual("Verify FTP Host is displayed", true, exportTabFields.ContainsKey("FTP Host"));
            VerifyEqual("Verify SFTP mode is displayed", true, exportTabFields.ContainsKey("SFTP mode"));
            VerifyEqual("Verify FTP user is displayed", true, exportTabFields.ContainsKey("FTP user"));
            VerifyEqual("Verify FTP password is displayed", true, exportTabFields.ContainsKey("FTP password"));
            VerifyEqual("Verify FTP file name is displayed", true, exportTabFields.ContainsKey("FTP file name"));
            VerifyEqual("[SC-596] Verify FTP passive mode is displayed", true, exportTabFields.ContainsKey("FTP passive mode"));
            VerifyEqual("Verify FTP enabled is displayed", true, exportTabFields.ContainsKey("FTP enabled"));
            var isFTPFormatDisplayed = exportTabFields.ContainsKey("FTP format");
            VerifyEqual("Verify FTP format is displayed", true, isFTPFormatDisplayed);
            if (isFTPFormatDisplayed) VerifyEqual("Verify FTP format is range as expected", expectedFormats, (List<string>)exportTabFields["FTP format"]);
            //Mail
            VerifyEqual("Verify Mail enabled is displayed", true, exportTabFields.ContainsKey("Mail enabled"));
            var isMailFormatDisplayed = exportTabFields.ContainsKey("Mail format");
            VerifyEqual("Verify Mail format is displayed", true, isMailFormatDisplayed);
            if (isFTPFormatDisplayed) VerifyEqual("Verify Mail format is range as expected", expectedFormats, (List<string>)exportTabFields["Mail format"]);
            VerifyEqual("Verify Report in mail's body is displayed", true, exportTabFields.ContainsKey("Report in mail's body"));
            VerifyEqual("Verify Subject is displayed", true, exportTabFields.ContainsKey("Subject"));
            VerifyEqual("Verify From is displayed", true, exportTabFields.ContainsKey("From"));
            VerifyEqual("Verify Contacts is displayed as list", true, exportTabFields.ContainsKey("Contacts") && ((List<string>)exportTabFields["Contacts"]).Any());

            page.ReportEditorPanel.SelectTab("Report");
            var expectedReportGroups = new List<string> { "Configuration" };
            groups = page.ReportEditorPanel.GetListOfGroupsNameActiveTab();
            VerifyEqual("Verify Groups as expected", expectedReportGroups, groups);
            var reportTabFields = page.ReportEditorPanel.GetPropertiesAndItemsActiveTab();
            VerifyEqual("Verify Device Categories is displayed", true, reportTabFields.ContainsKey("Device Categories") && ((List<string>)reportTabFields["Device Categories"]).Any());
            VerifyEqual("Verify Recurse is displayed", true, reportTabFields.ContainsKey("Recurse"));
        }

        private void VerifyOverWattageReportDetails(ReportManagerPage page)
        {
            var expectedTabs = new List<string> { "Scheduler", "Export", "Report" };
            var expectedRange00_23 = SLVHelper.GenerateListStringInterger(0, 23);
            var expectedRange00_59 = SLVHelper.GenerateListStringInterger(0, 59);
            var expectedPeriodicity = new List<string> { "Every day", "Every monday", "Every tuesday", "Every wednesday", "Every thursday", "Every friday", "Every saturday", "Every sunday" };
            var expectedFormats = new List<string> { "CSV", "Html", "Plain text" };

            VerifyEqual("Verify Name is displayed", true, page.ReportEditorPanel.IsNameFieldDisplayed());
            VerifyEqual("Verify Type is displayed", true, page.ReportEditorPanel.IsTypeFieldDisplayed());

            var tabs = page.ReportEditorPanel.GetListOfTabsName();
            VerifyEqual("Verify Tabs as expected", expectedTabs, tabs);

            page.ReportEditorPanel.SelectTab("Scheduler");
            var expectedSchedulerGroups = new List<string> { "Configuration" };
            var groups = page.ReportEditorPanel.GetListOfGroupsNameActiveTab();
            VerifyEqual("Verify Groups as expected", expectedSchedulerGroups, groups);
            var schedulerTabFields = page.ReportEditorPanel.GetPropertiesAndItemsActiveTab();
            var isHourDisplayed = schedulerTabFields.ContainsKey("Hour (HH)");
            var isMinuteDisplayed = schedulerTabFields.ContainsKey("Minute");
            var isPeriodicityMinuteDisplayed = schedulerTabFields.ContainsKey("Periodicity");
            VerifyEqual("Verify Hour (HH) is displayed", true, isHourDisplayed);
            if (isHourDisplayed) VerifyEqual("Verify Hour (HH) is range [00-23]", expectedRange00_23, (List<string>)schedulerTabFields["Hour (HH)"]);
            VerifyEqual("[SC-597] Verify Minute is displayed", true, isMinuteDisplayed);
            if (isMinuteDisplayed) VerifyEqual("Verify Minute is range [00-59]", expectedRange00_59, (List<string>)schedulerTabFields["Minute"]);
            VerifyEqual("Verify Periodicity is displayed", true, isPeriodicityMinuteDisplayed);
            if (isPeriodicityMinuteDisplayed) VerifyEqual("Verify Periodicity is range as expected", expectedPeriodicity, (List<string>)schedulerTabFields["Periodicity"]);

            page.ReportEditorPanel.SelectTab("Export");
            var expectedExportGroups = new List<string> { "FTP", "Mail" };
            groups = page.ReportEditorPanel.GetListOfGroupsNameActiveTab();
            VerifyEqual("Verify Groups as expected", expectedExportGroups, groups);
            var exportTabFields = page.ReportEditorPanel.GetPropertiesAndItemsActiveTab();
            //FTP
            VerifyEqual("Verify FTP Host is displayed", true, exportTabFields.ContainsKey("FTP Host"));
            VerifyEqual("Verify SFTP mode is displayed", true, exportTabFields.ContainsKey("SFTP mode"));
            VerifyEqual("Verify FTP user is displayed", true, exportTabFields.ContainsKey("FTP user"));
            VerifyEqual("Verify FTP password is displayed", true, exportTabFields.ContainsKey("FTP password"));
            VerifyEqual("Verify FTP file name is displayed", true, exportTabFields.ContainsKey("FTP file name"));
            VerifyEqual("[SC-596] Verify FTP passive mode is displayed", true, exportTabFields.ContainsKey("FTP passive mode"));
            VerifyEqual("Verify FTP enabled is displayed", true, exportTabFields.ContainsKey("FTP enabled"));
            var isFTPFormatDisplayed = exportTabFields.ContainsKey("FTP format");
            VerifyEqual("Verify FTP format is displayed", true, isFTPFormatDisplayed);
            if (isFTPFormatDisplayed) VerifyEqual("Verify FTP format is range as expected", expectedFormats, (List<string>)exportTabFields["FTP format"]);
            //Mail
            VerifyEqual("Verify Mail enabled is displayed", true, exportTabFields.ContainsKey("Mail enabled"));
            var isMailFormatDisplayed = exportTabFields.ContainsKey("Mail format");
            VerifyEqual("Verify Mail format is displayed", true, isMailFormatDisplayed);
            if (isFTPFormatDisplayed) VerifyEqual("Verify Mail format is range as expected", expectedFormats, (List<string>)exportTabFields["Mail format"]);
            VerifyEqual("Verify Report in mail's body is displayed", true, exportTabFields.ContainsKey("Report in mail's body"));
            VerifyEqual("Verify Subject is displayed", true, exportTabFields.ContainsKey("Subject"));
            VerifyEqual("Verify From is displayed", true, exportTabFields.ContainsKey("From"));
            VerifyEqual("Verify Contacts is displayed as list", true, exportTabFields.ContainsKey("Contacts") && ((List<string>)exportTabFields["Contacts"]).Any());

            page.ReportEditorPanel.SelectTab("Report");
            var expectedReportGroups = new List<string> { "Configuration" };
            groups = page.ReportEditorPanel.GetListOfGroupsNameActiveTab();
            VerifyEqual("Verify Groups as expected", expectedReportGroups, groups);
            var reportTabFields = page.ReportEditorPanel.GetPropertiesAndItemsActiveTab();
            VerifyEqual("Verify Device Categories is displayed", true, reportTabFields.ContainsKey("Device Categories") && ((List<string>)reportTabFields["Device Categories"]).Any());
            VerifyEqual("Verify Recurse is displayed", true, reportTabFields.ContainsKey("Recurse"));
        }

        private void VerifySymologyReportDetails(ReportManagerPage page)
        {
            var expectedTabs = new List<string> { "Scheduler", "Properties", "Export" };
            var expectedRange00_23 = SLVHelper.GenerateListStringInterger(0, 23);
            var expectedRange00_59 = SLVHelper.GenerateListStringInterger(0, 59);

            VerifyEqual("Verify Name is displayed", true, page.ReportEditorPanel.IsNameFieldDisplayed());
            VerifyEqual("Verify Type is displayed", true, page.ReportEditorPanel.IsTypeFieldDisplayed());

            var tabs = page.ReportEditorPanel.GetListOfTabsName();
            VerifyEqual("Verify Tabs as expected", expectedTabs, tabs);

            page.ReportEditorPanel.SelectTab("Properties");
            var expectedPropertiesGroups = new List<string> { "Configuration" };
            var groups = page.ReportEditorPanel.GetListOfGroupsNameActiveTab();
            VerifyEqual("Verify Groups as expected", expectedPropertiesGroups, groups);
            var propertiesTabFields = page.ReportEditorPanel.GetPropertiesAndItemsActiveTab();
            VerifyEqual("Verify Description is displayed", true, propertiesTabFields.ContainsKey("description"));
            VerifyEqual("Verify Down meanings is displayed as list", true, propertiesTabFields.ContainsKey("Down meanings") && ((List<string>)propertiesTabFields["Down meanings"]).Any());
            VerifyEqual("Verify Contact type is displayed", true, propertiesTabFields.ContainsKey("Contact type"));
            VerifyEqual("Verify Customer full name is displayed", true, propertiesTabFields.ContainsKey("Customer full name"));
            VerifyEqual("Verify Customer short code is displayed", true, propertiesTabFields.ContainsKey("Customer short code"));
            VerifyEqual("Verify Service code is displayed", true, propertiesTabFields.ContainsKey("Service code"));
            VerifyEqual("Verify Unit name is displayed", true, propertiesTabFields.ContainsKey("Unit name"));
            VerifyEqual("Verify Web customer ID is displayed", true, propertiesTabFields.ContainsKey("Web customer ID"));

            page.ReportEditorPanel.SelectTab("Scheduler");
            var expectedSchedulerGroups = new List<string> { "Configuration" };
            groups = page.ReportEditorPanel.GetListOfGroupsNameActiveTab();
            VerifyEqual("Verify Groups as expected", expectedSchedulerGroups, groups);
            var schedulerTabFields = page.ReportEditorPanel.GetPropertiesAndItemsActiveTab();
            var isHourDisplayed = schedulerTabFields.ContainsKey("Hour of day");
            var isMinuteDisplayed = schedulerTabFields.ContainsKey("Minute");
            VerifyEqual("Verify Hour of day is displayed", true, isHourDisplayed);
            if (isHourDisplayed) VerifyEqual("Verify Hour of day is range [00-23]", expectedRange00_23, (List<string>)schedulerTabFields["Hour of day"]);
            VerifyEqual("[SC-597] Verify Minute is displayed", true, isMinuteDisplayed);
            if (isMinuteDisplayed) VerifyEqual("Verify Minute is range [00-59]", expectedRange00_59, (List<string>)schedulerTabFields["Minute"]);

            page.ReportEditorPanel.SelectTab("Export");
            var expectedExportGroups = new List<string> { "FTP" };
            groups = page.ReportEditorPanel.GetListOfGroupsNameActiveTab();
            VerifyEqual("Verify Groups as expected", expectedExportGroups, groups);
            var exportTabFields = page.ReportEditorPanel.GetPropertiesAndItemsActiveTab();
            VerifyEqual("Verify FTP host is displayed", true, exportTabFields.ContainsKey("FTP host"));
            VerifyEqual("Verify FTP user is displayed", true, exportTabFields.ContainsKey("FTP user"));
            VerifyEqual("Verify FTP password is displayed", true, exportTabFields.ContainsKey("FTP password"));
            VerifyEqual("Verify FTP dir is displayed", true, exportTabFields.ContainsKey("FTP dir"));
            VerifyEqual("Verify FTP passive mode is displayed", true, exportTabFields.ContainsKey("FTP passive mode"));
        }

        private void VerifyUMSUGReportDetails(ReportManagerPage page)
        {
            var expectedTabs = new List<string> { "Scheduler", "Properties", "Export" };
            var expectedRange00_23 = SLVHelper.GenerateListStringInterger(0, 23);
            var expectedRange00_59 = SLVHelper.GenerateListStringInterger(0, 59);

            VerifyEqual("Verify Name is displayed", true, page.ReportEditorPanel.IsNameFieldDisplayed());
            VerifyEqual("Verify Type is displayed", true, page.ReportEditorPanel.IsTypeFieldDisplayed());

            var tabs = page.ReportEditorPanel.GetListOfTabsName();
            VerifyEqual("Verify Tabs as expected", expectedTabs, tabs);

            page.ReportEditorPanel.SelectTab("Scheduler");
            var expectedSchedulerGroups = new List<string> { "Configuration" };
            var groups = page.ReportEditorPanel.GetListOfGroupsNameActiveTab();
            VerifyEqual("Verify Groups as expected", expectedSchedulerGroups, groups);
            var schedulerTabFields = page.ReportEditorPanel.GetPropertiesAndItemsActiveTab();
            var isHourDisplayed = schedulerTabFields.ContainsKey("Hour");
            var isMinuteDisplayed = schedulerTabFields.ContainsKey("Minute");
            VerifyEqual("Verify Hour is displayed", true, isHourDisplayed);
            if (isHourDisplayed) VerifyEqual("Verify Hour is range [00-23]", expectedRange00_23, (List<string>)schedulerTabFields["Hour"]);
            VerifyEqual("[SC-597] Verify Minute is displayed", true, isMinuteDisplayed);
            if (isMinuteDisplayed) VerifyEqual("Verify Minute is range [00-59]", expectedRange00_59, (List<string>)schedulerTabFields["Minute"]);

            page.ReportEditorPanel.SelectTab("Properties");
            var expectedPropertiesGroups = new List<string> { "Configuration" };
            groups = page.ReportEditorPanel.GetListOfGroupsNameActiveTab();
            VerifyEqual("Verify Groups as expected", expectedPropertiesGroups, groups);
            var propertiesTabFields = page.ReportEditorPanel.GetPropertiesAndItemsActiveTab();
            VerifyEqual("Verify Description is displayed", true, propertiesTabFields.ContainsKey("description"));
            VerifyEqual("Verify Minimum delta time (minutes) is displayed", true, propertiesTabFields.ContainsKey("Minimum delta time (minutes)"));
            VerifyEqual("Verify Minimum delta level (%) is displayed", true, propertiesTabFields.ContainsKey("Minimum delta level (%)"));
            VerifyEqual("Verify Down failures is displayed as list", true, propertiesTabFields.ContainsKey("Down failures") && ((List<string>)propertiesTabFields["Down failures"]).Any());
            VerifyEqual("Verify Max retry is displayed", true, propertiesTabFields.ContainsKey("Max retry"));            

            page.ReportEditorPanel.SelectTab("Export");
            var expectedExportGroups = new List<string> { "FTP" };
            groups = page.ReportEditorPanel.GetListOfGroupsNameActiveTab();
            VerifyEqual("Verify Groups as expected", expectedExportGroups, groups);
            var exportTabFields = page.ReportEditorPanel.GetPropertiesAndItemsActiveTab();
            VerifyEqual("Verify FTP host is displayed", true, exportTabFields.ContainsKey("FTP host"));
            VerifyEqual("Verify FTP user is displayed", true, exportTabFields.ContainsKey("FTP user"));
            VerifyEqual("Verify FTP password is displayed", true, exportTabFields.ContainsKey("FTP password"));
            VerifyEqual("Verify directory is displayed", true, exportTabFields.ContainsKey("directory"));
            VerifyEqual("Verify FTP passive mode is displayed", true, exportTabFields.ContainsKey("FTP passive mode"));
        }

        private void VerifyUnbalanced3PhaseCabinetsReportDetails(ReportManagerPage page)
        {
            var expectedTabs = new List<string> { "Scheduler", "Export", "Report" };
            var expectedRange00_23 = SLVHelper.GenerateListStringInterger(0, 23);
            var expectedRange00_59 = SLVHelper.GenerateListStringInterger(0, 59);
            var expectedPeriodicity = new List<string> { "Every day", "Every monday", "Every tuesday", "Every wednesday", "Every thursday", "Every friday", "Every saturday", "Every sunday" };
            var expectedFormats = new List<string> { "CSV", "Html", "Plain text" };

            VerifyEqual("Verify Name is displayed", true, page.ReportEditorPanel.IsNameFieldDisplayed());
            VerifyEqual("Verify Type is displayed", true, page.ReportEditorPanel.IsTypeFieldDisplayed());

            var tabs = page.ReportEditorPanel.GetListOfTabsName();
            VerifyEqual("Verify Tabs as expected", expectedTabs, tabs);

            page.ReportEditorPanel.SelectTab("Scheduler");
            var expectedSchedulerGroups = new List<string> { "Configuration" };
            var groups = page.ReportEditorPanel.GetListOfGroupsNameActiveTab();
            VerifyEqual("Verify Groups as expected", expectedSchedulerGroups, groups);
            var schedulerTabFields = page.ReportEditorPanel.GetPropertiesAndItemsActiveTab();
            var isHourDisplayed = schedulerTabFields.ContainsKey("Hour (HH)");
            var isMinuteDisplayed = schedulerTabFields.ContainsKey("Minute");
            var isPeriodicityMinuteDisplayed = schedulerTabFields.ContainsKey("Periodicity");
            VerifyEqual("Verify Hour (HH) is displayed", true, isHourDisplayed);
            if (isHourDisplayed) VerifyEqual("Verify Hour (HH) is range [00-23]", expectedRange00_23, (List<string>)schedulerTabFields["Hour (HH)"]);
            VerifyEqual("[SC-597] Verify Minute is displayed", true, isMinuteDisplayed);
            if (isMinuteDisplayed) VerifyEqual("Verify Minute is range [00-59]", expectedRange00_59, (List<string>)schedulerTabFields["Minute"]);
            VerifyEqual("Verify Periodicity is displayed", true, isPeriodicityMinuteDisplayed);
            if (isPeriodicityMinuteDisplayed) VerifyEqual("Verify Periodicity is range as expected", expectedPeriodicity, (List<string>)schedulerTabFields["Periodicity"]);

            page.ReportEditorPanel.SelectTab("Export");
            var expectedExportGroups = new List<string> { "FTP", "Mail" };
            groups = page.ReportEditorPanel.GetListOfGroupsNameActiveTab();
            VerifyEqual("Verify Groups as expected", expectedExportGroups, groups);
            var exportTabFields = page.ReportEditorPanel.GetPropertiesAndItemsActiveTab();
            //FTP
            VerifyEqual("Verify FTP Host is displayed", true, exportTabFields.ContainsKey("FTP Host"));
            VerifyEqual("Verify SFTP mode is displayed", true, exportTabFields.ContainsKey("SFTP mode"));
            VerifyEqual("Verify FTP user is displayed", true, exportTabFields.ContainsKey("FTP user"));
            VerifyEqual("Verify FTP password is displayed", true, exportTabFields.ContainsKey("FTP password"));
            VerifyEqual("Verify FTP file name is displayed", true, exportTabFields.ContainsKey("FTP file name"));
            VerifyEqual("[SC-596] Verify FTP passive mode is displayed", true, exportTabFields.ContainsKey("FTP passive mode"));
            VerifyEqual("Verify FTP enabled is displayed", true, exportTabFields.ContainsKey("FTP enabled"));
            var isFTPFormatDisplayed = exportTabFields.ContainsKey("FTP format");
            VerifyEqual("Verify FTP format is displayed", true, isFTPFormatDisplayed);
            if (isFTPFormatDisplayed) VerifyEqual("Verify FTP format is range as expected", expectedFormats, (List<string>)exportTabFields["FTP format"]);
            //Mail
            VerifyEqual("Verify Mail enabled is displayed", true, exportTabFields.ContainsKey("Mail enabled"));
            var isMailFormatDisplayed = exportTabFields.ContainsKey("Mail format");
            VerifyEqual("Verify Mail format is displayed", true, isMailFormatDisplayed);
            if (isFTPFormatDisplayed) VerifyEqual("Verify Mail format is range as expected", expectedFormats, (List<string>)exportTabFields["Mail format"]);
            VerifyEqual("Verify Report in mail's body is displayed", true, exportTabFields.ContainsKey("Report in mail's body"));
            VerifyEqual("Verify Subject is displayed", true, exportTabFields.ContainsKey("Subject"));
            VerifyEqual("Verify From is displayed", true, exportTabFields.ContainsKey("From"));
            VerifyEqual("Verify Contacts is displayed as list", true, exportTabFields.ContainsKey("Contacts") && ((List<string>)exportTabFields["Contacts"]).Any());

            page.ReportEditorPanel.SelectTab("Report");
            var expectedReportGroups = new List<string> { "Configuration" };
            groups = page.ReportEditorPanel.GetListOfGroupsNameActiveTab();
            VerifyEqual("Verify Groups as expected", expectedReportGroups, groups);
            var reportTabFields = page.ReportEditorPanel.GetPropertiesAndItemsActiveTab();
            VerifyEqual("Verify Device Categories is displayed", true, reportTabFields.ContainsKey("Device Categories") && ((List<string>)reportTabFields["Device Categories"]).Any());
            VerifyEqual("Verify Recurse is displayed", true, reportTabFields.ContainsKey("Recurse"));
        }

        private void VerifyUnder110VVoltageReportDetails(ReportManagerPage page)
        {
            var expectedTabs = new List<string> { "Scheduler", "Export", "Report" };
            var expectedRange00_23 = SLVHelper.GenerateListStringInterger(0, 23);
            var expectedRange00_59 = SLVHelper.GenerateListStringInterger(0, 59);
            var expectedPeriodicity = new List<string> { "Every day", "Every monday", "Every tuesday", "Every wednesday", "Every thursday", "Every friday", "Every saturday", "Every sunday" };
            var expectedFormats = new List<string> { "CSV", "Html", "Plain text" };

            VerifyEqual("Verify Name is displayed", true, page.ReportEditorPanel.IsNameFieldDisplayed());
            VerifyEqual("Verify Type is displayed", true, page.ReportEditorPanel.IsTypeFieldDisplayed());

            var tabs = page.ReportEditorPanel.GetListOfTabsName();
            VerifyEqual("Verify Tabs as expected", expectedTabs, tabs);

            page.ReportEditorPanel.SelectTab("Scheduler");
            var expectedSchedulerGroups = new List<string> { "Configuration" };
            var groups = page.ReportEditorPanel.GetListOfGroupsNameActiveTab();
            VerifyEqual("Verify Groups as expected", expectedSchedulerGroups, groups);
            var schedulerTabFields = page.ReportEditorPanel.GetPropertiesAndItemsActiveTab();
            var isHourDisplayed = schedulerTabFields.ContainsKey("Hour (HH)");
            var isMinuteDisplayed = schedulerTabFields.ContainsKey("Minute");
            var isPeriodicityMinuteDisplayed = schedulerTabFields.ContainsKey("Periodicity");
            VerifyEqual("Verify Hour (HH) is displayed", true, isHourDisplayed);
            if (isHourDisplayed) VerifyEqual("Verify Hour (HH) is range [00-23]", expectedRange00_23, (List<string>)schedulerTabFields["Hour (HH)"]);
            VerifyEqual("[SC-597] Verify Minute is displayed", true, isMinuteDisplayed);
            if (isMinuteDisplayed) VerifyEqual("Verify Minute is range [00-59]", expectedRange00_59, (List<string>)schedulerTabFields["Minute"]);
            VerifyEqual("Verify Periodicity is displayed", true, isPeriodicityMinuteDisplayed);
            if (isPeriodicityMinuteDisplayed) VerifyEqual("Verify Periodicity is range as expected", expectedPeriodicity, (List<string>)schedulerTabFields["Periodicity"]);

            page.ReportEditorPanel.SelectTab("Export");
            var expectedExportGroups = new List<string> { "FTP", "Mail" };
            groups = page.ReportEditorPanel.GetListOfGroupsNameActiveTab();
            VerifyEqual("Verify Groups as expected", expectedExportGroups, groups);
            var exportTabFields = page.ReportEditorPanel.GetPropertiesAndItemsActiveTab();
            //FTP
            VerifyEqual("Verify FTP Host is displayed", true, exportTabFields.ContainsKey("FTP Host"));
            VerifyEqual("Verify SFTP mode is displayed", true, exportTabFields.ContainsKey("SFTP mode"));
            VerifyEqual("Verify FTP user is displayed", true, exportTabFields.ContainsKey("FTP user"));
            VerifyEqual("Verify FTP password is displayed", true, exportTabFields.ContainsKey("FTP password"));
            VerifyEqual("Verify FTP file name is displayed", true, exportTabFields.ContainsKey("FTP file name"));
            VerifyEqual("[SC-596] Verify FTP passive mode is displayed", true, exportTabFields.ContainsKey("FTP passive mode"));
            VerifyEqual("Verify FTP enabled is displayed", true, exportTabFields.ContainsKey("FTP enabled"));
            var isFTPFormatDisplayed = exportTabFields.ContainsKey("FTP format");
            VerifyEqual("Verify FTP format is displayed", true, isFTPFormatDisplayed);
            if (isFTPFormatDisplayed) VerifyEqual("Verify FTP format is range as expected", expectedFormats, (List<string>)exportTabFields["FTP format"]);
            //Mail
            VerifyEqual("Verify Mail enabled is displayed", true, exportTabFields.ContainsKey("Mail enabled"));
            var isMailFormatDisplayed = exportTabFields.ContainsKey("Mail format");
            VerifyEqual("Verify Mail format is displayed", true, isMailFormatDisplayed);
            if (isFTPFormatDisplayed) VerifyEqual("Verify Mail format is range as expected", expectedFormats, (List<string>)exportTabFields["Mail format"]);
            VerifyEqual("Verify Report in mail's body is displayed", true, exportTabFields.ContainsKey("Report in mail's body"));
            VerifyEqual("Verify Subject is displayed", true, exportTabFields.ContainsKey("Subject"));
            VerifyEqual("Verify From is displayed", true, exportTabFields.ContainsKey("From"));
            VerifyEqual("Verify Contacts is displayed as list", true, exportTabFields.ContainsKey("Contacts") && ((List<string>)exportTabFields["Contacts"]).Any());

            page.ReportEditorPanel.SelectTab("Report");
            var expectedReportGroups = new List<string> { "Configuration" };
            groups = page.ReportEditorPanel.GetListOfGroupsNameActiveTab();
            VerifyEqual("Verify Groups as expected", expectedReportGroups, groups);
            var reportTabFields = page.ReportEditorPanel.GetPropertiesAndItemsActiveTab();
            VerifyEqual("Verify Device Categories is displayed", true, reportTabFields.ContainsKey("Device Categories") && ((List<string>)reportTabFields["Device Categories"]).Any());
            VerifyEqual("Verify Recurse is displayed", true, reportTabFields.ContainsKey("Recurse"));
        }

        private void VerifyUnderVoltageReportDetails(ReportManagerPage page)
        {
            var expectedTabs = new List<string> { "Scheduler", "Export", "Report" };
            var expectedRange00_23 = SLVHelper.GenerateListStringInterger(0, 23);
            var expectedRange00_59 = SLVHelper.GenerateListStringInterger(0, 59);
            var expectedPeriodicity = new List<string> { "Every day", "Every monday", "Every tuesday", "Every wednesday", "Every thursday", "Every friday", "Every saturday", "Every sunday" };
            var expectedFormats = new List<string> { "CSV", "Html", "Plain text" };

            VerifyEqual("Verify Name is displayed", true, page.ReportEditorPanel.IsNameFieldDisplayed());
            VerifyEqual("Verify Type is displayed", true, page.ReportEditorPanel.IsTypeFieldDisplayed());

            var tabs = page.ReportEditorPanel.GetListOfTabsName();
            VerifyEqual("Verify Tabs as expected", expectedTabs, tabs);

            page.ReportEditorPanel.SelectTab("Scheduler");
            var expectedSchedulerGroups = new List<string> { "Configuration" };
            var groups = page.ReportEditorPanel.GetListOfGroupsNameActiveTab();
            VerifyEqual("Verify Groups as expected", expectedSchedulerGroups, groups);
            var schedulerTabFields = page.ReportEditorPanel.GetPropertiesAndItemsActiveTab();
            var isHourDisplayed = schedulerTabFields.ContainsKey("Hour (HH)");
            var isMinuteDisplayed = schedulerTabFields.ContainsKey("Minute");
            var isPeriodicityMinuteDisplayed = schedulerTabFields.ContainsKey("Periodicity");
            VerifyEqual("Verify Hour (HH) is displayed", true, isHourDisplayed);
            if (isHourDisplayed) VerifyEqual("Verify Hour (HH) is range [00-23]", expectedRange00_23, (List<string>)schedulerTabFields["Hour (HH)"]);
            VerifyEqual("[SC-597] Verify Minute is displayed", true, isMinuteDisplayed);
            if (isMinuteDisplayed) VerifyEqual("Verify Minute is range [00-59]", expectedRange00_59, (List<string>)schedulerTabFields["Minute"]);
            VerifyEqual("Verify Periodicity is displayed", true, isPeriodicityMinuteDisplayed);
            if (isPeriodicityMinuteDisplayed) VerifyEqual("Verify Periodicity is range as expected", expectedPeriodicity, (List<string>)schedulerTabFields["Periodicity"]);

            page.ReportEditorPanel.SelectTab("Export");
            var expectedExportGroups = new List<string> { "FTP", "Mail" };
            groups = page.ReportEditorPanel.GetListOfGroupsNameActiveTab();
            VerifyEqual("Verify Groups as expected", expectedExportGroups, groups);
            var exportTabFields = page.ReportEditorPanel.GetPropertiesAndItemsActiveTab();
            //FTP
            VerifyEqual("Verify FTP Host is displayed", true, exportTabFields.ContainsKey("FTP Host"));
            VerifyEqual("Verify SFTP mode is displayed", true, exportTabFields.ContainsKey("SFTP mode"));
            VerifyEqual("Verify FTP user is displayed", true, exportTabFields.ContainsKey("FTP user"));
            VerifyEqual("Verify FTP password is displayed", true, exportTabFields.ContainsKey("FTP password"));
            VerifyEqual("Verify FTP file name is displayed", true, exportTabFields.ContainsKey("FTP file name"));
            VerifyEqual("[SC-596] Verify FTP passive mode is displayed", true, exportTabFields.ContainsKey("FTP passive mode"));
            VerifyEqual("Verify FTP enabled is displayed", true, exportTabFields.ContainsKey("FTP enabled"));
            var isFTPFormatDisplayed = exportTabFields.ContainsKey("FTP format");
            VerifyEqual("Verify FTP format is displayed", true, isFTPFormatDisplayed);
            if (isFTPFormatDisplayed) VerifyEqual("Verify FTP format is range as expected", expectedFormats, (List<string>)exportTabFields["FTP format"]);
            //Mail
            VerifyEqual("Verify Mail enabled is displayed", true, exportTabFields.ContainsKey("Mail enabled"));
            var isMailFormatDisplayed = exportTabFields.ContainsKey("Mail format");
            VerifyEqual("Verify Mail format is displayed", true, isMailFormatDisplayed);
            if (isFTPFormatDisplayed) VerifyEqual("Verify Mail format is range as expected", expectedFormats, (List<string>)exportTabFields["Mail format"]);
            VerifyEqual("Verify Report in mail's body is displayed", true, exportTabFields.ContainsKey("Report in mail's body"));
            VerifyEqual("Verify Subject is displayed", true, exportTabFields.ContainsKey("Subject"));
            VerifyEqual("Verify From is displayed", true, exportTabFields.ContainsKey("From"));
            VerifyEqual("Verify Contacts is displayed as list", true, exportTabFields.ContainsKey("Contacts") && ((List<string>)exportTabFields["Contacts"]).Any());

            page.ReportEditorPanel.SelectTab("Report");
            var expectedReportGroups = new List<string> { "Configuration" };
            groups = page.ReportEditorPanel.GetListOfGroupsNameActiveTab();
            VerifyEqual("Verify Groups as expected", expectedReportGroups, groups);
            var reportTabFields = page.ReportEditorPanel.GetPropertiesAndItemsActiveTab();
            VerifyEqual("Verify Device Categories is displayed", true, reportTabFields.ContainsKey("Device Categories") && ((List<string>)reportTabFields["Device Categories"]).Any());
            VerifyEqual("Verify Recurse is displayed", true, reportTabFields.ContainsKey("Recurse"));
        }

        private void VerifyUnderWattageReportDetails(ReportManagerPage page)
        {
            var expectedTabs = new List<string> { "Scheduler", "Export", "Report" };
            var expectedRange00_23 = SLVHelper.GenerateListStringInterger(0, 23);
            var expectedRange00_59 = SLVHelper.GenerateListStringInterger(0, 59);
            var expectedPeriodicity = new List<string> { "Every day", "Every monday", "Every tuesday", "Every wednesday", "Every thursday", "Every friday", "Every saturday", "Every sunday" };
            var expectedFormats = new List<string> { "CSV", "Html", "Plain text" };

            VerifyEqual("Verify Name is displayed", true, page.ReportEditorPanel.IsNameFieldDisplayed());
            VerifyEqual("Verify Type is displayed", true, page.ReportEditorPanel.IsTypeFieldDisplayed());

            var tabs = page.ReportEditorPanel.GetListOfTabsName();
            VerifyEqual("Verify Tabs as expected", expectedTabs, tabs);

            page.ReportEditorPanel.SelectTab("Scheduler");
            var expectedSchedulerGroups = new List<string> { "Configuration" };
            var groups = page.ReportEditorPanel.GetListOfGroupsNameActiveTab();
            VerifyEqual("Verify Groups as expected", expectedSchedulerGroups, groups);
            var schedulerTabFields = page.ReportEditorPanel.GetPropertiesAndItemsActiveTab();
            var isHourDisplayed = schedulerTabFields.ContainsKey("Hour (HH)");
            var isMinuteDisplayed = schedulerTabFields.ContainsKey("Minute");
            var isPeriodicityMinuteDisplayed = schedulerTabFields.ContainsKey("Periodicity");
            VerifyEqual("Verify Hour (HH) is displayed", true, isHourDisplayed);
            if (isHourDisplayed) VerifyEqual("Verify Hour (HH) is range [00-23]", expectedRange00_23, (List<string>)schedulerTabFields["Hour (HH)"]);
            VerifyEqual("[SC-597] Verify Minute is displayed", true, isMinuteDisplayed);
            if (isMinuteDisplayed) VerifyEqual("Verify Minute is range [00-59]", expectedRange00_59, (List<string>)schedulerTabFields["Minute"]);
            VerifyEqual("Verify Periodicity is displayed", true, isPeriodicityMinuteDisplayed);
            if (isPeriodicityMinuteDisplayed) VerifyEqual("Verify Periodicity is range as expected", expectedPeriodicity, (List<string>)schedulerTabFields["Periodicity"]);

            page.ReportEditorPanel.SelectTab("Export");
            var expectedExportGroups = new List<string> { "FTP", "Mail" };
            groups = page.ReportEditorPanel.GetListOfGroupsNameActiveTab();
            VerifyEqual("Verify Groups as expected", expectedExportGroups, groups);
            var exportTabFields = page.ReportEditorPanel.GetPropertiesAndItemsActiveTab();
            //FTP
            VerifyEqual("Verify FTP Host is displayed", true, exportTabFields.ContainsKey("FTP Host"));
            VerifyEqual("Verify SFTP mode is displayed", true, exportTabFields.ContainsKey("SFTP mode"));
            VerifyEqual("Verify FTP user is displayed", true, exportTabFields.ContainsKey("FTP user"));
            VerifyEqual("Verify FTP password is displayed", true, exportTabFields.ContainsKey("FTP password"));
            VerifyEqual("Verify FTP file name is displayed", true, exportTabFields.ContainsKey("FTP file name"));
            VerifyEqual("[SC-596] Verify FTP passive mode is displayed", true, exportTabFields.ContainsKey("FTP passive mode"));
            VerifyEqual("Verify FTP enabled is displayed", true, exportTabFields.ContainsKey("FTP enabled"));
            var isFTPFormatDisplayed = exportTabFields.ContainsKey("FTP format");
            VerifyEqual("Verify FTP format is displayed", true, isFTPFormatDisplayed);
            if (isFTPFormatDisplayed) VerifyEqual("Verify FTP format is range as expected", expectedFormats, (List<string>)exportTabFields["FTP format"]);
            //Mail
            VerifyEqual("Verify Mail enabled is displayed", true, exportTabFields.ContainsKey("Mail enabled"));
            var isMailFormatDisplayed = exportTabFields.ContainsKey("Mail format");
            VerifyEqual("Verify Mail format is displayed", true, isMailFormatDisplayed);
            if (isFTPFormatDisplayed) VerifyEqual("Verify Mail format is range as expected", expectedFormats, (List<string>)exportTabFields["Mail format"]);
            VerifyEqual("Verify Report in mail's body is displayed", true, exportTabFields.ContainsKey("Report in mail's body"));
            VerifyEqual("Verify Subject is displayed", true, exportTabFields.ContainsKey("Subject"));
            VerifyEqual("Verify From is displayed", true, exportTabFields.ContainsKey("From"));
            VerifyEqual("Verify Contacts is displayed as list", true, exportTabFields.ContainsKey("Contacts") && ((List<string>)exportTabFields["Contacts"]).Any());

            page.ReportEditorPanel.SelectTab("Report");
            var expectedReportGroups = new List<string> { "Configuration" };
            groups = page.ReportEditorPanel.GetListOfGroupsNameActiveTab();
            VerifyEqual("Verify Groups as expected", expectedReportGroups, groups);
            var reportTabFields = page.ReportEditorPanel.GetPropertiesAndItemsActiveTab();
            VerifyEqual("Verify Device Categories is displayed", true, reportTabFields.ContainsKey("Device Categories") && ((List<string>)reportTabFields["Device Categories"]).Any());
            VerifyEqual("Verify Recurse is displayed", true, reportTabFields.ContainsKey("Recurse"));
        }

        private void VerifyWeeklyEnergyDetails(ReportManagerPage page)
        {
            var expectedTabs = new List<string> { "Properties", "Scheduler", "Export" };
            var expectedRange00_23 = SLVHelper.GenerateListStringInterger(0, 23);
            var expectedRange00_59 = SLVHelper.GenerateListStringInterger(0, 59);
            var expectedPeriodicity = new List<string> { "Every day", "Every monday", "Every tuesday", "Every wednesday", "Every thursday", "Every friday", "Every saturday", "Every sunday" };

            VerifyEqual("Verify Name is displayed", true, page.ReportEditorPanel.IsNameFieldDisplayed());
            VerifyEqual("Verify Type is displayed", true, page.ReportEditorPanel.IsTypeFieldDisplayed());

            var tabs = page.ReportEditorPanel.GetListOfTabsName();
            VerifyEqual("Verify Tabs as expected", expectedTabs, tabs);

            page.ReportEditorPanel.SelectTab("Properties");
            var expectedPropertiesGroups = new List<string> { "Configuration" };
            var groups = page.ReportEditorPanel.GetListOfGroupsNameActiveTab();
            VerifyEqual("Verify Groups as expected", expectedPropertiesGroups, groups);
            var propertiesTabFields = page.ReportEditorPanel.GetPropertiesAndItemsActiveTab();
            VerifyEqual("Verify Description is displayed", true, propertiesTabFields.ContainsKey("description"));

            page.ReportEditorPanel.SelectTab("Scheduler");
            var expectedSchedulerGroups = new List<string> { "Configuration" };
            groups = page.ReportEditorPanel.GetListOfGroupsNameActiveTab();
            VerifyEqual("Verify Groups as expected", expectedSchedulerGroups, groups);
            var schedulerTabFields = page.ReportEditorPanel.GetPropertiesAndItemsActiveTab();
            var isHourDisplayed = schedulerTabFields.ContainsKey("Hour");
            var isMinuteDisplayed = schedulerTabFields.ContainsKey("Minute");
            var isPeriodicityMinuteDisplayed = schedulerTabFields.ContainsKey("Periodicity");
            VerifyEqual("Verify Hour is displayed", true, isHourDisplayed);
            if (isHourDisplayed) VerifyEqual("Verify Hour is range [00-23]", expectedRange00_23, (List<string>)schedulerTabFields["Hour"]);
            VerifyEqual("[SC-597] Verify Minute is displayed", true, isMinuteDisplayed);
            if (isMinuteDisplayed) VerifyEqual("Verify Minute is range [00-59]", expectedRange00_59, (List<string>)schedulerTabFields["Minute"]);
            VerifyEqual("Verify Periodicity is displayed", true, isPeriodicityMinuteDisplayed);
            if (isPeriodicityMinuteDisplayed) VerifyEqual("Verify Periodicity is range as expected", expectedPeriodicity, (List<string>)schedulerTabFields["Periodicity"]);

            page.ReportEditorPanel.SelectTab("Export");
            var expectedExportGroups = new List<string> { "Mail", "Configuration" };
            groups = page.ReportEditorPanel.GetListOfGroupsNameActiveTab();
            VerifyEqual("Verify Groups as expected", expectedExportGroups, groups);
            var exportTabFields = page.ReportEditorPanel.GetPropertiesAndItemsActiveTab();
            //Mail           
            VerifyEqual("Verify Subject is displayed", true, exportTabFields.ContainsKey("Subject"));
            VerifyEqual("Verify From is displayed", true, exportTabFields.ContainsKey("From"));
            VerifyEqual("Verify Contacts is displayed as list", true, exportTabFields.ContainsKey("Contacts *") && ((List<string>)exportTabFields["Contacts *"]).Any());
            //Configuration      
            VerifyEqual("Verify HTML format is displayed", true, exportTabFields.ContainsKey("HTML format"));
        }

        private void VerifyFlashNetLampFailureReportDetails(ReportManagerPage page)
        {
            var expectedTabs = new List<string> { "Scheduler", "Export", "Report" };
            var expectedRange00_23 = SLVHelper.GenerateListStringInterger(0, 23);
            var expectedRange00_59 = SLVHelper.GenerateListStringInterger(0, 59);
            var expectedPeriodicity = new List<string> { "Every day", "Every monday", "Every tuesday", "Every wednesday", "Every thursday", "Every friday", "Every saturday", "Every sunday" };
            var expectedFormats = new List<string> { "CSV", "Html", "Plain text" };

            VerifyEqual("Verify Name is displayed", true, page.ReportEditorPanel.IsNameFieldDisplayed());
            VerifyEqual("Verify Type is displayed", true, page.ReportEditorPanel.IsTypeFieldDisplayed());

            var tabs = page.ReportEditorPanel.GetListOfTabsName();
            VerifyEqual("Verify Tabs as expected", expectedTabs, tabs);

            page.ReportEditorPanel.SelectTab("Scheduler");
            var expectedSchedulerGroups = new List<string> { "Configuration" };
            var groups = page.ReportEditorPanel.GetListOfGroupsNameActiveTab();
            VerifyEqual("Verify Groups as expected", expectedSchedulerGroups, groups);
            var schedulerTabFields = page.ReportEditorPanel.GetPropertiesAndItemsActiveTab();
            var isHourDisplayed = schedulerTabFields.ContainsKey("Hour (HH)");
            var isMinuteDisplayed = schedulerTabFields.ContainsKey("Minute");
            var isPeriodicityMinuteDisplayed = schedulerTabFields.ContainsKey("Periodicity");
            VerifyEqual("Verify Hour (HH) is displayed", true, isHourDisplayed);
            if (isHourDisplayed) VerifyEqual("Verify Hour (HH) is range [00-23]", expectedRange00_23, (List<string>)schedulerTabFields["Hour (HH)"]);
            VerifyEqual("[SC-597] Verify Minute is displayed", true, isMinuteDisplayed);
            if (isMinuteDisplayed) VerifyEqual("Verify Minute is range [00-59]", expectedRange00_59, (List<string>)schedulerTabFields["Minute"]);
            VerifyEqual("Verify Periodicity is displayed", true, isPeriodicityMinuteDisplayed);
            if (isPeriodicityMinuteDisplayed) VerifyEqual("Verify Periodicity is range as expected", expectedPeriodicity, (List<string>)schedulerTabFields["Periodicity"]);

            page.ReportEditorPanel.SelectTab("Export");
            var expectedExportGroups = new List<string> { "FTP", "Mail" };
            groups = page.ReportEditorPanel.GetListOfGroupsNameActiveTab();
            VerifyEqual("Verify Groups as expected", expectedExportGroups, groups);
            var exportTabFields = page.ReportEditorPanel.GetPropertiesAndItemsActiveTab();
            //FTP
            VerifyEqual("Verify FTP Host is displayed", true, exportTabFields.ContainsKey("FTP Host"));
            VerifyEqual("Verify SFTP mode is displayed", true, exportTabFields.ContainsKey("SFTP mode"));
            VerifyEqual("Verify FTP user is displayed", true, exportTabFields.ContainsKey("FTP user"));
            VerifyEqual("Verify FTP password is displayed", true, exportTabFields.ContainsKey("FTP password"));
            VerifyEqual("Verify FTP file name is displayed", true, exportTabFields.ContainsKey("FTP file name"));
            VerifyEqual("[SC-596] Verify FTP passive mode is displayed", true, exportTabFields.ContainsKey("FTP passive mode"));
            VerifyEqual("Verify FTP enabled is displayed", true, exportTabFields.ContainsKey("FTP enabled"));
            var isFTPFormatDisplayed = exportTabFields.ContainsKey("FTP format");
            VerifyEqual("Verify FTP format is displayed", true, isFTPFormatDisplayed);
            if (isFTPFormatDisplayed) VerifyEqual("Verify FTP format is range as expected", expectedFormats, (List<string>)exportTabFields["FTP format"]);
            //Mail
            VerifyEqual("Verify Mail enabled is displayed", true, exportTabFields.ContainsKey("Mail enabled"));
            var isMailFormatDisplayed = exportTabFields.ContainsKey("Mail format");
            VerifyEqual("Verify Mail format is displayed", true, isMailFormatDisplayed);
            if (isFTPFormatDisplayed) VerifyEqual("Verify Mail format is range as expected", expectedFormats, (List<string>)exportTabFields["Mail format"]);
            VerifyEqual("Verify Report in mail's body is displayed", true, exportTabFields.ContainsKey("Report in mail's body"));
            VerifyEqual("Verify Subject is displayed", true, exportTabFields.ContainsKey("Subject"));
            VerifyEqual("Verify From is displayed", true, exportTabFields.ContainsKey("From"));
            VerifyEqual("Verify Contacts is displayed as list", true, exportTabFields.ContainsKey("Contacts") && ((List<string>)exportTabFields["Contacts"]).Any());

            page.ReportEditorPanel.SelectTab("Report");
            var expectedReportGroups = new List<string> { "Configuration" };
            groups = page.ReportEditorPanel.GetListOfGroupsNameActiveTab();
            VerifyEqual("Verify Groups as expected", expectedReportGroups, groups);
            var reportTabFields = page.ReportEditorPanel.GetPropertiesAndItemsActiveTab();
            VerifyEqual("Verify Device Categories is displayed", true, reportTabFields.ContainsKey("Device Categories") && ((List<string>)reportTabFields["Device Categories"]).Any());
            VerifyEqual("Verify Recurse is displayed", true, reportTabFields.ContainsKey("Recurse"));
        }

        private void VerifyLocationChangeReportDetails(ReportManagerPage page)
        {
            var expectedTabs = new List<string> { "Export", "Scheduler", "Report" };
            var expectedRange00_23 = SLVHelper.GenerateListStringInterger(0, 23);
            var expectedRange00_59 = SLVHelper.GenerateListStringInterger(0, 59);
            var expectedPeriodicity = new List<string> { "Every day", "Every monday", "Every tuesday", "Every wednesday", "Every thursday", "Every friday", "Every saturday", "Every sunday" };
            var expectedFormats = new List<string> { "CSV"};
            var expectedPeriod = new List<string> { "1 day", "1 week", "1 month" };

            VerifyEqual("Verify Name is displayed", true, page.ReportEditorPanel.IsNameFieldDisplayed());
            VerifyEqual("Verify Type is displayed", true, page.ReportEditorPanel.IsTypeFieldDisplayed());

            var tabs = page.ReportEditorPanel.GetListOfTabsName();
            VerifyEqual("[SC-599] Verify Tabs as expected", expectedTabs, tabs);

            page.ReportEditorPanel.SelectTab("Scheduler");
            var expectedSchedulerGroups = new List<string> { "Configuration" };
            var groups = page.ReportEditorPanel.GetListOfGroupsNameActiveTab();
            VerifyEqual("Verify Groups as expected", expectedSchedulerGroups, groups);
            var schedulerTabFields = page.ReportEditorPanel.GetPropertiesAndItemsActiveTab();
            var isHourDisplayed = schedulerTabFields.ContainsKey("Hour (HH)");
            var isMinuteDisplayed = schedulerTabFields.ContainsKey("Minute");
            var isPeriodicityMinuteDisplayed = schedulerTabFields.ContainsKey("Periodicity");
            VerifyEqual("Verify Hour (HH) is displayed", true, isHourDisplayed);
            if (isHourDisplayed) VerifyEqual("Verify Hour (HH) is range [00-23]", expectedRange00_23, (List<string>)schedulerTabFields["Hour (HH)"]);
            VerifyEqual("[SC-597] Verify Minute is displayed", true, isMinuteDisplayed);
            if (isMinuteDisplayed) VerifyEqual("Verify Minute is range [00-59]", expectedRange00_59, (List<string>)schedulerTabFields["Minute"]);
            VerifyEqual("Verify Periodicity is displayed", true, isPeriodicityMinuteDisplayed);
            if (isPeriodicityMinuteDisplayed) VerifyEqual("Verify Periodicity is range as expected", expectedPeriodicity, (List<string>)schedulerTabFields["Periodicity"]);

            page.ReportEditorPanel.SelectTab("Export");
            var expectedExportGroups = new List<string> { "FTP", "Mail" };
            groups = page.ReportEditorPanel.GetListOfGroupsNameActiveTab();
            VerifyEqual("Verify Groups as expected", expectedExportGroups, groups);
            var exportTabFields = page.ReportEditorPanel.GetPropertiesAndItemsActiveTab();
            //FTP
            VerifyEqual("Verify FTP Host is displayed", true, exportTabFields.ContainsKey("FTP Host"));
            VerifyEqual("Verify SFTP mode is displayed", true, exportTabFields.ContainsKey("SFTP mode"));
            VerifyEqual("Verify FTP user is displayed", true, exportTabFields.ContainsKey("FTP user"));
            VerifyEqual("Verify FTP password is displayed", true, exportTabFields.ContainsKey("FTP password"));
            VerifyEqual("Verify FTP file name is displayed", true, exportTabFields.ContainsKey("FTP file name"));
            VerifyEqual("[SC-596] Verify FTP passive mode is displayed", true, exportTabFields.ContainsKey("FTP passive mode"));
            VerifyEqual("Verify FTP enabled is displayed", true, exportTabFields.ContainsKey("FTP enabled"));
            var isFTPFormatDisplayed = exportTabFields.ContainsKey("FTP format");
            VerifyEqual("Verify FTP format is displayed", true, isFTPFormatDisplayed);
            if (isFTPFormatDisplayed) VerifyEqual("Verify FTP format is range as expected", expectedFormats, (List<string>)exportTabFields["FTP format"]);
            //Mail
            VerifyEqual("Verify Mail enabled is displayed", true, exportTabFields.ContainsKey("Mail enabled"));
            var isMailFormatDisplayed = exportTabFields.ContainsKey("Mail format");
            VerifyEqual("Verify Mail format is displayed", true, isMailFormatDisplayed);
            if (isFTPFormatDisplayed) VerifyEqual("Verify Mail format is range as expected", expectedFormats, (List<string>)exportTabFields["Mail format"]);
            VerifyEqual("Verify Report in mail's body is displayed", true, exportTabFields.ContainsKey("Report in mail's body"));
            VerifyEqual("Verify Subject is displayed", true, exportTabFields.ContainsKey("Subject"));
            VerifyEqual("Verify From is displayed", true, exportTabFields.ContainsKey("From"));
            VerifyEqual("Verify Contacts is displayed as list", true, exportTabFields.ContainsKey("Contacts") && ((List<string>)exportTabFields["Contacts"]).Any());

            page.ReportEditorPanel.SelectTab("Report");
            var expectedReportGroups = new List<string> { "Configuration" };
            groups = page.ReportEditorPanel.GetListOfGroupsNameActiveTab();
            VerifyEqual("Verify Groups as expected", expectedReportGroups, groups);
            var reportTabFields = page.ReportEditorPanel.GetPropertiesAndItemsActiveTab();
            VerifyEqual("Verify Device Categories is displayed", true, reportTabFields.ContainsKey("Device Categories") && ((List<string>)reportTabFields["Device Categories"]).Any());
            var isPeriodDisplayed = reportTabFields.ContainsKey("Period");
            VerifyEqual("Verify Period is displayed", true, isPeriodDisplayed);
            if (isPeriodDisplayed) VerifyEqual("Verify Period is range as expected", expectedPeriod, (List<string>)reportTabFields["Period"], false);
            VerifyEqual("Verify Minimum included is displayed", true, reportTabFields.ContainsKey("Minimum included"));
            VerifyEqual("Verify Minimum shift (for unit change) is displayed", true, reportTabFields.ContainsKey("Minimum shift (for unit change)"));
            VerifyEqual("Verify Minimum shift (for whole period) is displayed", true, reportTabFields.ContainsKey("Minimum shift (for whole period)"));
            VerifyEqual("Verify Recurse is displayed", true, reportTabFields.ContainsKey("Recurse"));
        }

        private void VerifyGenericDeviceValuesRunOnceReportDetails(ReportManagerPage page)
        {
            var expectedTabs = new List<string> { "Export", "Report" };            
            var expectedFormats = new List<string> { "CSV", "Html", "Plain text" };

            VerifyEqual("Verify Name is displayed", true, page.ReportEditorPanel.IsNameFieldDisplayed());
            VerifyEqual("Verify Type is displayed", true, page.ReportEditorPanel.IsTypeFieldDisplayed());

            var tabs = page.ReportEditorPanel.GetListOfTabsName();
            VerifyEqual("Verify Tabs as expected", expectedTabs, tabs);
            
            page.ReportEditorPanel.SelectTab("Export");
            var expectedExportGroups = new List<string> { "FTP", "Mail" };
            var groups = page.ReportEditorPanel.GetListOfGroupsNameActiveTab();
            VerifyEqual("Verify Groups as expected", expectedExportGroups, groups);
            var exportTabFields = page.ReportEditorPanel.GetPropertiesAndItemsActiveTab();
            //FTP
            VerifyEqual("Verify FTP Host is displayed", true, exportTabFields.ContainsKey("FTP Host"));
            VerifyEqual("Verify SFTP mode is displayed", true, exportTabFields.ContainsKey("SFTP mode"));
            VerifyEqual("Verify FTP user is displayed", true, exportTabFields.ContainsKey("FTP user"));
            VerifyEqual("Verify FTP password is displayed", true, exportTabFields.ContainsKey("FTP password"));
            VerifyEqual("Verify FTP file name is displayed", true, exportTabFields.ContainsKey("FTP file name"));
            VerifyEqual("Verify FTP passive mode is displayed", true, exportTabFields.ContainsKey("FTP passive mode"));
            VerifyEqual("Verify FTP enabled is displayed", true, exportTabFields.ContainsKey("FTP enabled"));
            var isFTPFormatDisplayed = exportTabFields.ContainsKey("FTP format");
            VerifyEqual("Verify FTP format is displayed", true, isFTPFormatDisplayed);
            if (isFTPFormatDisplayed) VerifyEqual("Verify FTP format is range as expected", expectedFormats, (List<string>)exportTabFields["FTP format"]);
            //Mail
            VerifyEqual("Verify Mail enabled is displayed", true, exportTabFields.ContainsKey("Mail enabled"));
            var isMailFormatDisplayed = exportTabFields.ContainsKey("Mail format");
            VerifyEqual("Verify Mail format is displayed", true, isMailFormatDisplayed);
            if (isFTPFormatDisplayed) VerifyEqual("Verify Mail format is range as expected", expectedFormats, (List<string>)exportTabFields["Mail format"]);
            VerifyEqual("Verify Report in mail's body is displayed", true, exportTabFields.ContainsKey("Report in mail's body"));
            VerifyEqual("Verify Subject is displayed", true, exportTabFields.ContainsKey("Subject"));
            VerifyEqual("Verify From is displayed", true, exportTabFields.ContainsKey("From"));
            VerifyEqual("Verify Contacts is displayed as list", true, exportTabFields.ContainsKey("Contacts") && ((List<string>)exportTabFields["Contacts"]).Any());

            page.ReportEditorPanel.SelectTab("Report");
            var expectedReportGroups = new List<string> { "Configuration" };
            groups = page.ReportEditorPanel.GetListOfGroupsNameActiveTab();
            VerifyEqual("Verify Groups as expected", expectedReportGroups, groups);
            var reportTabFields = page.ReportEditorPanel.GetPropertiesAndItemsActiveTab();
            VerifyEqual("Verify Device Categories is displayed as list", true, reportTabFields.ContainsKey("Device Categories") && ((List<string>)reportTabFields["Device Categories"]).Any());
            VerifyEqual("Verify Values is displayed as list", true, reportTabFields.ContainsKey("Values") && ((List<string>)reportTabFields["Values"]).Any());
            VerifyEqual("Verify Start day is displayed", true, reportTabFields.ContainsKey("Start day"));
            VerifyEqual("Verify Start time (HH:mm) is displayed", true, reportTabFields.ContainsKey("Start time (HH:mm)"));
            VerifyEqual("Verify End day is displayed", true, reportTabFields.ContainsKey("End day"));
            VerifyEqual("Verify End time (HH:mm) is displayed", true, reportTabFields.ContainsKey("End time (HH:mm)"));
        }

        private void VerifyAdvancedSearchReportDetails(ReportManagerPage page, string expectedSavedSearch)
        {
            var expectedTabs = new List<string> { "Scheduler", "Report", "Export" };
            var expectedRange00_23 = SLVHelper.GenerateListStringInterger(0, 23);
            var expectedRange00_59 = SLVHelper.GenerateListStringInterger(0, 59);
            var expectedPeriodicity = new List<string> { "Every day", "Every monday", "Every tuesday", "Every wednesday", "Every thursday", "Every friday", "Every saturday", "Every sunday" };
            var expectedFormats = new List<string> { "CSV", "Html", "Plain text" };

            VerifyEqual("Verify Name is displayed", true, page.ReportEditorPanel.IsNameFieldDisplayed());
            VerifyEqual("Verify Type is displayed", true, page.ReportEditorPanel.IsTypeFieldDisplayed());

            var tabs = page.ReportEditorPanel.GetListOfTabsName();
            VerifyEqual("Verify Tabs as expected", expectedTabs, tabs);

            page.ReportEditorPanel.SelectTab("Scheduler");
            var expectedSchedulerGroups = new List<string> { "Configuration" };
            var groups = page.ReportEditorPanel.GetListOfGroupsNameActiveTab();
            VerifyEqual("Verify Groups as expected", expectedSchedulerGroups, groups);
            var schedulerTabFields = page.ReportEditorPanel.GetPropertiesAndItemsActiveTab();
            var isHourDisplayed = schedulerTabFields.ContainsKey("Hour (HH)");
            var isMinuteDisplayed = schedulerTabFields.ContainsKey("Minute");
            var isPeriodicityMinuteDisplayed = schedulerTabFields.ContainsKey("Periodicity");
            VerifyEqual("Verify Hour (HH) is displayed", true, isHourDisplayed);
            if (isHourDisplayed) VerifyEqual("Verify Hour (HH) is range [00-23]", expectedRange00_23, (List<string>)schedulerTabFields["Hour (HH)"]);
            VerifyEqual("[SC-597] Verify Minute is displayed", true, isMinuteDisplayed);
            if (isMinuteDisplayed) VerifyEqual("Verify Minute is range [00-59]", expectedRange00_59, (List<string>)schedulerTabFields["Minute"]);
            VerifyEqual("Verify Periodicity is displayed", true, isPeriodicityMinuteDisplayed);
            if (isPeriodicityMinuteDisplayed) VerifyEqual("Verify Periodicity is range as expected", expectedPeriodicity, (List<string>)schedulerTabFields["Periodicity"]);
                        
            page.ReportEditorPanel.SelectTab("Report");
            var expectedReportGroups = new List<string> { "Configuration" };
            groups = page.ReportEditorPanel.GetListOfGroupsNameActiveTab();
            VerifyEqual("Verify Groups as expected", expectedReportGroups, groups);
            var reportTabFields = page.ReportEditorPanel.GetPropertiesAndItemsActiveTab();
            var savedSearches = ((List<string>)reportTabFields["Saved search"]);
            VerifyEqual("Verify Saved search is displayed", true, reportTabFields.ContainsKey("Saved search"));
            VerifyEqual("Verify Saved search contains the advanced search from the precondition", true, savedSearches.Contains(expectedSavedSearch));

            page.ReportEditorPanel.SelectTab("Export");
            var expectedExportGroups = new List<string> { "FTP", "Mail" };
            groups = page.ReportEditorPanel.GetListOfGroupsNameActiveTab();
            VerifyEqual("Verify Groups as expected", expectedExportGroups, groups);
            var exportTabFields = page.ReportEditorPanel.GetPropertiesAndItemsActiveTab();
            //FTP
            VerifyEqual("Verify FTP Host is displayed", true, exportTabFields.ContainsKey("FTP Host"));
            VerifyEqual("Verify SFTP mode is displayed", true, exportTabFields.ContainsKey("SFTP mode"));
            VerifyEqual("Verify FTP user is displayed", true, exportTabFields.ContainsKey("FTP user"));
            VerifyEqual("Verify FTP password is displayed", true, exportTabFields.ContainsKey("FTP password"));
            VerifyEqual("Verify FTP file name is displayed", true, exportTabFields.ContainsKey("FTP file name"));
            VerifyEqual("Verify FTP passive mode is displayed", true, exportTabFields.ContainsKey("FTP passive mode"));
            VerifyEqual("Verify FTP enabled is displayed", true, exportTabFields.ContainsKey("FTP enabled"));
            var isFTPFormatDisplayed = exportTabFields.ContainsKey("FTP format");
            VerifyEqual("Verify FTP format is displayed", true, isFTPFormatDisplayed);
            if (isFTPFormatDisplayed) VerifyEqual("Verify FTP format is range as expected", expectedFormats, (List<string>)exportTabFields["FTP format"]);
            //Mail
            VerifyEqual("Verify Mail enabled is displayed", true, exportTabFields.ContainsKey("Mail enabled"));
            var isMailFormatDisplayed = exportTabFields.ContainsKey("Mail format");
            VerifyEqual("Verify Mail format is displayed", true, isMailFormatDisplayed);
            if (isFTPFormatDisplayed) VerifyEqual("Verify Mail format is range as expected", expectedFormats, (List<string>)exportTabFields["Mail format"]);
            VerifyEqual("Verify Report in mail's body is displayed", true, exportTabFields.ContainsKey("Report in mail's body"));
            VerifyEqual("Verify Subject is displayed", true, exportTabFields.ContainsKey("Subject"));
            VerifyEqual("Verify From is displayed", true, exportTabFields.ContainsKey("From"));
            VerifyEqual("Verify Contacts is displayed as list", true, exportTabFields.ContainsKey("Contacts") && ((List<string>)exportTabFields["Contacts"]).Any());

        }

        #endregion //Verify methods

        #region Input XML data

        private Dictionary<string, string> GetTestDataOfTestRM_01()
        {
            var testCaseName = "RM_01";
            var xmlUtility = new XmlUtility(Settings.RM_TEST_DATA_FILE_PATH);
            var testData = new Dictionary<string, string>();

            testData.Add("GeoZone", xmlUtility.GetSingleNodeText(string.Format(Settings.RM_XPATH_PREFIX, testCaseName, "GeoZone")));

            return testData;
        }

        private Dictionary<string, string> GetTestDataOfTestRM_02()
        {
            var testCaseName = "RM_02";
            var xmlUtility = new XmlUtility(Settings.RM_TEST_DATA_FILE_PATH);
            var testData = new Dictionary<string, string>();

            testData.Add("GeoZone", xmlUtility.GetSingleNodeText(string.Format(Settings.RM_XPATH_PREFIX, testCaseName, "GeoZone")));

            return testData;
        }

        private Dictionary<string, string> GetTestDataOfTestRM_03()
        {
            var testCaseName = "RM_03";
            var xmlUtility = new XmlUtility(Settings.RM_TEST_DATA_FILE_PATH);
            var testData = new Dictionary<string, string>();

            testData.Add("GeoZone", xmlUtility.GetSingleNodeText(string.Format(Settings.RM_XPATH_PREFIX, testCaseName, "GeoZone")));

            return testData;
        }

        private Dictionary<string, string> GetTestDataOfTestRM_04()
        {
            var testCaseName = "RM_04";
            var xmlUtility = new XmlUtility(Settings.RM_TEST_DATA_FILE_PATH);
            var testData = new Dictionary<string, string>();

            testData.Add("GeoZone", xmlUtility.GetSingleNodeText(string.Format(Settings.RM_XPATH_PREFIX, testCaseName, "GeoZone")));

            return testData;
        }

        private Dictionary<string, string> GetTestDataOfTestRM_05()
        {
            var testCaseName = "RM_05";
            var xmlUtility = new XmlUtility(Settings.RM_TEST_DATA_FILE_PATH);
            var testData = new Dictionary<string, string>();

            testData.Add("GeoZone", xmlUtility.GetSingleNodeText(string.Format(Settings.RM_XPATH_PREFIX, testCaseName, "GeoZone")));

            return testData;
        }

        private Dictionary<string, string> GetTestDataOfTestRM_06()
        {
            var testCaseName = "RM_06";
            var xmlUtility = new XmlUtility(Settings.RM_TEST_DATA_FILE_PATH);
            var testData = new Dictionary<string, string>();

            testData.Add("GeoZone", xmlUtility.GetSingleNodeText(string.Format(Settings.RM_XPATH_PREFIX, testCaseName, "GeoZone")));

            return testData;
        }

        private Dictionary<string, string> GetTestDataOfTestRM_07()
        {
            var testCaseName = "RM_07";
            var xmlUtility = new XmlUtility(Settings.RM_TEST_DATA_FILE_PATH);
            var testData = new Dictionary<string, string>();

            testData.Add("GeoZone", xmlUtility.GetSingleNodeText(string.Format(Settings.RM_XPATH_PREFIX, testCaseName, "GeoZone")));

            return testData;
        }

        private Dictionary<string, string> GetTestDataOfTestRM_08()
        {
            var testCaseName = "RM_08";
            var xmlUtility = new XmlUtility(Settings.RM_TEST_DATA_FILE_PATH);
            var testData = new Dictionary<string, string>();

            testData.Add("GeoZone", xmlUtility.GetSingleNodeText(string.Format(Settings.RM_XPATH_PREFIX, testCaseName, "GeoZone")));

            return testData;
        }

        private Dictionary<string, string> GetTestDataOfTestRM_09()
        {
            var testCaseName = "RM_09";
            var xmlUtility = new XmlUtility(Settings.RM_TEST_DATA_FILE_PATH);
            var testData = new Dictionary<string, string>();

            testData.Add("GeoZone", xmlUtility.GetSingleNodeText(string.Format(Settings.RM_XPATH_PREFIX, testCaseName, "GeoZone")));

            return testData;
        }

        private Dictionary<string, string> GetTestDataOfTestRM_10()
        {
            var testCaseName = "RM_10";
            var xmlUtility = new XmlUtility(Settings.RM_TEST_DATA_FILE_PATH);
            var testData = new Dictionary<string, string>();

            testData.Add("GeoZone", xmlUtility.GetSingleNodeText(string.Format(Settings.RM_XPATH_PREFIX, testCaseName, "GeoZone")));

            return testData;
        }

        private Dictionary<string, string> GetTestDataOfTestRM_11()
        {
            var testCaseName = "RM_11";
            var xmlUtility = new XmlUtility(Settings.RM_TEST_DATA_FILE_PATH);
            var testData = new Dictionary<string, string>();

            testData.Add("GeoZone", xmlUtility.GetSingleNodeText(string.Format(Settings.RM_XPATH_PREFIX, testCaseName, "GeoZone")));

            return testData;
        }

        private Dictionary<string, string> GetTestDataOfTestRM_12()
        {
            var testCaseName = "RM_12";
            var xmlUtility = new XmlUtility(Settings.RM_TEST_DATA_FILE_PATH);
            var testData = new Dictionary<string, string>();

            testData.Add("GeoZone", xmlUtility.GetSingleNodeText(string.Format(Settings.RM_XPATH_PREFIX, testCaseName, "GeoZone")));

            return testData;
        }

        private Dictionary<string, string> GetTestDataOfTestRM_13()
        {
            var testCaseName = "RM_13";
            var xmlUtility = new XmlUtility(Settings.RM_TEST_DATA_FILE_PATH);
            var testData = new Dictionary<string, string>();

            testData.Add("GeoZone", xmlUtility.GetSingleNodeText(string.Format(Settings.RM_XPATH_PREFIX, testCaseName, "GeoZone")));

            return testData;
        }

        private Dictionary<string, string> GetTestDataOfTestRM_14()
        {
            var testCaseName = "RM_14";
            var xmlUtility = new XmlUtility(Settings.RM_TEST_DATA_FILE_PATH);
            var testData = new Dictionary<string, string>();

            testData.Add("GeoZone", xmlUtility.GetSingleNodeText(string.Format(Settings.RM_XPATH_PREFIX, testCaseName, "GeoZone")));

            return testData;
        }

        private Dictionary<string, string> GetTestDataOfTestRM_15()
        {
            var testCaseName = "RM_15";
            var xmlUtility = new XmlUtility(Settings.RM_TEST_DATA_FILE_PATH);
            var testData = new Dictionary<string, string>();

            testData.Add("GeoZone", xmlUtility.GetSingleNodeText(string.Format(Settings.RM_XPATH_PREFIX, testCaseName, "GeoZone")));

            return testData;
        }

        private Dictionary<string, string> GetTestDataOfTestRM_16()
        {
            var testCaseName = "RM_16";
            var xmlUtility = new XmlUtility(Settings.RM_TEST_DATA_FILE_PATH);
            var testData = new Dictionary<string, string>();

            testData.Add("GeoZone", xmlUtility.GetSingleNodeText(string.Format(Settings.RM_XPATH_PREFIX, testCaseName, "GeoZone")));

            return testData;
        }

        private Dictionary<string, string> GetTestDataOfTestRM_17()
        {
            var testCaseName = "RM_17";
            var xmlUtility = new XmlUtility(Settings.RM_TEST_DATA_FILE_PATH);
            var testData = new Dictionary<string, string>();

            testData.Add("GeoZone", xmlUtility.GetSingleNodeText(string.Format(Settings.RM_XPATH_PREFIX, testCaseName, "GeoZone")));

            return testData;
        }

        private Dictionary<string, string> GetTestDataOfTestRM_18()
        {
            var testCaseName = "RM_18";
            var xmlUtility = new XmlUtility(Settings.RM_TEST_DATA_FILE_PATH);
            var testData = new Dictionary<string, string>();

            testData.Add("GeoZone", xmlUtility.GetSingleNodeText(string.Format(Settings.RM_XPATH_PREFIX, testCaseName, "GeoZone")));

            return testData;
        }

        private Dictionary<string, string> GetTestDataOfTestRM_19()
        {
            var testCaseName = "RM_19";
            var xmlUtility = new XmlUtility(Settings.RM_TEST_DATA_FILE_PATH);
            var testData = new Dictionary<string, string>();

            testData.Add("GeoZone", xmlUtility.GetSingleNodeText(string.Format(Settings.RM_XPATH_PREFIX, testCaseName, "GeoZone")));

            return testData;
        }

        private Dictionary<string, string> GetTestDataOfTestRM_20()
        {
            var testCaseName = "RM_20";
            var xmlUtility = new XmlUtility(Settings.RM_TEST_DATA_FILE_PATH);
            var testData = new Dictionary<string, string>();

            testData.Add("GeoZone", xmlUtility.GetSingleNodeText(string.Format(Settings.RM_XPATH_PREFIX, testCaseName, "GeoZone")));

            return testData;
        }

        private Dictionary<string, string> GetTestDataOfTestRM_21()
        {
            var testCaseName = "RM_21";
            var xmlUtility = new XmlUtility(Settings.RM_TEST_DATA_FILE_PATH);
            var testData = new Dictionary<string, string>();

            testData.Add("GeoZone", xmlUtility.GetSingleNodeText(string.Format(Settings.RM_XPATH_PREFIX, testCaseName, "GeoZone")));

            return testData;
        }

        private Dictionary<string, string> GetTestDataOfTestRM_22()
        {
            var testCaseName = "RM_22";
            var xmlUtility = new XmlUtility(Settings.RM_TEST_DATA_FILE_PATH);
            var testData = new Dictionary<string, string>();

            testData.Add("GeoZone", xmlUtility.GetSingleNodeText(string.Format(Settings.RM_XPATH_PREFIX, testCaseName, "GeoZone")));

            return testData;
        }

        private Dictionary<string, string> GetTestDataOfTestRM_23()
        {
            var testCaseName = "RM_23";
            var xmlUtility = new XmlUtility(Settings.RM_TEST_DATA_FILE_PATH);
            var testData = new Dictionary<string, string>();

            testData.Add("GeoZone", xmlUtility.GetSingleNodeText(string.Format(Settings.RM_XPATH_PREFIX, testCaseName, "GeoZone")));

            return testData;
        }

        private Dictionary<string, string> GetTestDataOfTestRM_24()
        {
            var testCaseName = "RM_24";
            var xmlUtility = new XmlUtility(Settings.RM_TEST_DATA_FILE_PATH);
            var testData = new Dictionary<string, string>();

            testData.Add("GeoZoneA", xmlUtility.GetSingleNodeText(string.Format(Settings.RM_XPATH_PREFIX, testCaseName, "GeoZoneA")));
            testData.Add("GeoZoneB", xmlUtility.GetSingleNodeText(string.Format(Settings.RM_XPATH_PREFIX, testCaseName, "GeoZoneB")));

            return testData;
        }

        private Dictionary<string, string> GetTestDataOfTestRM_25()
        {
            var testCaseName = "RM_25";
            var xmlUtility = new XmlUtility(Settings.RM_TEST_DATA_FILE_PATH);
            var testData = new Dictionary<string, string>();

            testData.Add("GeoZone", xmlUtility.GetSingleNodeText(string.Format(Settings.RM_XPATH_PREFIX, testCaseName, "GeoZone")));

            return testData;
        }

        private Dictionary<string, object> GetTestDataOfTestRM_26()
        {
            var testCaseName = "RM_26";
            var xmlUtility = new XmlUtility(Settings.RM_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, object>();
            testData.Add("Geozone", xmlUtility.GetSingleNodeText(string.Format(Settings.RM_XPATH_PREFIX, testCaseName, "Geozone")));
            testData.Add("ReportData", xmlUtility.GetSingleNode(string.Format(Settings.RM_XPATH_PREFIX, testCaseName, "ReportData")));
            testData.Add("Mail", xmlUtility.GetSingleNode(string.Format(Settings.RM_XPATH_PREFIX, testCaseName, "Mail")));
            testData.Add("Ftp", xmlUtility.GetSingleNode(string.Format(Settings.RM_XPATH_PREFIX, testCaseName, "Ftp")));

            return testData;
        }

        private Dictionary<string, object> GetTestDataOfTestRM_27()
        {
            var testCaseName = "RM_27";
            var xmlUtility = new XmlUtility(Settings.RM_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, object>();
            testData.Add("Geozone", xmlUtility.GetSingleNodeText(string.Format(Settings.RM_XPATH_PREFIX, testCaseName, "Geozone")));
            testData.Add("ReportData", xmlUtility.GetSingleNode(string.Format(Settings.RM_XPATH_PREFIX, testCaseName, "ReportData")));
            testData.Add("Mail", xmlUtility.GetSingleNode(string.Format(Settings.RM_XPATH_PREFIX, testCaseName, "Mail")));

            return testData;
        }

        private Dictionary<string, object> GetTestDataOfTestRM_28()
        {
            var testCaseName = "RM_28";
            var xmlUtility = new XmlUtility(Settings.RM_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, object>();
            testData.Add("Geozone", xmlUtility.GetSingleNodeText(string.Format(Settings.RM_XPATH_PREFIX, testCaseName, "Geozone")));
            testData.Add("ReportData", xmlUtility.GetSingleNode(string.Format(Settings.RM_XPATH_PREFIX, testCaseName, "ReportData")));
            testData.Add("Mail", xmlUtility.GetSingleNode(string.Format(Settings.RM_XPATH_PREFIX, testCaseName, "Mail")));

            return testData;
        }

        private Dictionary<string, object> GetTestDataOfTestRM_29()
        {
            var testCaseName = "RM_29";
            var xmlUtility = new XmlUtility(Settings.RM_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, object>();
            testData.Add("Geozone", xmlUtility.GetSingleNodeText(string.Format(Settings.RM_XPATH_PREFIX, testCaseName, "Geozone")));
            testData.Add("ReportData", xmlUtility.GetSingleNode(string.Format(Settings.RM_XPATH_PREFIX, testCaseName, "ReportData")));
            testData.Add("Mail", xmlUtility.GetSingleNode(string.Format(Settings.RM_XPATH_PREFIX, testCaseName, "Mail")));            

            return testData;
        }

        private Dictionary<string, object> GetTestDataOfTestRM_30()
        {
            var testCaseName = "RM_30";
            var xmlUtility = new XmlUtility(Settings.RM_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, object>();
            testData.Add("Geozone", xmlUtility.GetSingleNodeText(string.Format(Settings.RM_XPATH_PREFIX, testCaseName, "Geozone")));
            testData.Add("ReportData", xmlUtility.GetSingleNode(string.Format(Settings.RM_XPATH_PREFIX, testCaseName, "ReportData")));
            testData.Add("Mail", xmlUtility.GetSingleNode(string.Format(Settings.RM_XPATH_PREFIX, testCaseName, "Mail")));

            return testData;
        }

        private Dictionary<string, object> GetTestDataOfTestRM_31()
        {
            var testCaseName = "RM_31";
            var xmlUtility = new XmlUtility(Settings.RM_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, object>();
            testData.Add("Geozone", xmlUtility.GetSingleNodeText(string.Format(Settings.RM_XPATH_PREFIX, testCaseName, "Geozone")));
            var controllerInfo = xmlUtility.GetSingleNode(string.Format(Settings.RM_XPATH_PREFIX, testCaseName, "Controller"));
            testData.Add("ControllerName", controllerInfo.GetAttrVal("name"));
            testData.Add("ControllerId", controllerInfo.GetAttrVal("id"));
            testData.Add("ReportData", xmlUtility.GetSingleNode(string.Format(Settings.RM_XPATH_PREFIX, testCaseName, "ReportData")));
            testData.Add("Mail", xmlUtility.GetSingleNode(string.Format(Settings.RM_XPATH_PREFIX, testCaseName, "Mail")));
            
            return testData;
        }

        private Dictionary<string, object> GetTestDataOfTestRM_32()
        {
            var testCaseName = "RM_32";
            var xmlUtility = new XmlUtility(Settings.RM_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, object>();
            testData.Add("Geozone", xmlUtility.GetSingleNodeText(string.Format(Settings.RM_XPATH_PREFIX, testCaseName, "Geozone")));
            var controllerInfo = xmlUtility.GetSingleNode(string.Format(Settings.RM_XPATH_PREFIX, testCaseName, "Controller"));
            testData.Add("ControllerName", controllerInfo.GetAttrVal("name"));
            testData.Add("ControllerId", controllerInfo.GetAttrVal("id"));
            testData.Add("ReportData", xmlUtility.GetSingleNode(string.Format(Settings.RM_XPATH_PREFIX, testCaseName, "ReportData")));
            testData.Add("Mail", xmlUtility.GetSingleNode(string.Format(Settings.RM_XPATH_PREFIX, testCaseName, "Mail")));

            return testData;
        }

        private Dictionary<string, object> GetTestDataOfTestRM_34()
        {
            var testCaseName = "RM_34";
            var xmlUtility = new XmlUtility(Settings.RM_TEST_DATA_FILE_PATH);
            var testData = new Dictionary<string, object>();
            
            testData.Add("ReportData", xmlUtility.GetSingleNode(string.Format(Settings.RM_XPATH_PREFIX, testCaseName, "ReportData")));
            testData.Add("Mail", xmlUtility.GetSingleNode(string.Format(Settings.RM_XPATH_PREFIX, testCaseName, "Mail")));
            testData.Add("Ftp", xmlUtility.GetSingleNode(string.Format(Settings.RM_XPATH_PREFIX, testCaseName, "Ftp")));

            return testData;
        }

        private Dictionary<string, object> GetTestDataOfTestRM_35()
        {
            var testCaseName = "RM_35";
            var xmlUtility = new XmlUtility(Settings.RM_TEST_DATA_FILE_PATH);
            var testData = new Dictionary<string, object>();
            
            testData.Add("ReportData", xmlUtility.GetSingleNode(string.Format(Settings.RM_XPATH_PREFIX, testCaseName, "ReportData")));
            testData.Add("Mail", xmlUtility.GetSingleNode(string.Format(Settings.RM_XPATH_PREFIX, testCaseName, "Mail")));
            testData.Add("Ftp", xmlUtility.GetSingleNode(string.Format(Settings.RM_XPATH_PREFIX, testCaseName, "Ftp")));

            return testData;
        }

        private Dictionary<string, object> GetTestDataOfTestRM_36()
        {
            var testCaseName = "RM_36";
            var xmlUtility = new XmlUtility(Settings.RM_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, object>();
            testData.Add("Geozone", xmlUtility.GetSingleNodeText(string.Format(Settings.RM_XPATH_PREFIX, testCaseName, "Geozone")));
            testData.Add("ReportData", xmlUtility.GetSingleNode(string.Format(Settings.RM_XPATH_PREFIX, testCaseName, "ReportData")));
            testData.Add("Mail", xmlUtility.GetSingleNode(string.Format(Settings.RM_XPATH_PREFIX, testCaseName, "Mail")));
            testData.Add("Ftp", xmlUtility.GetSingleNode(string.Format(Settings.RM_XPATH_PREFIX, testCaseName, "Ftp")));

            return testData;
        }

        #endregion //Input XML data

        #endregion //Private methods
    }
}
