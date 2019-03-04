using NUnit.Framework;
using StreetlightVision.Extensions;
using StreetlightVision.Models;
using StreetlightVision.Pages;
using StreetlightVision.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StreetlightVision.Tests.Coverage.Apps
{
    [TestFixture]
    [NonParallelizable]
    public class BackOfficeAppTests : TestBase
    {
        #region Variables

        private readonly string _dataHistoryGridExportedFilePattern = "Data_history*.csv";

        #endregion //Variables

        #region Contructors

        #endregion //Contructors

        #region Test Cases

        [Test, DynamicRetry]
        [Description("BO_01_01 App availability - Back Office is not found in Desktop page")]
        public void BO_01_01()
        {
            Step("**** Precondition ****");
            Step(" - User must belong to root/admin group");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            Step("1. After user logs in, verify Back Office is NOT found in Desktop page");
            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["admin"].Username, Settings.Users["admin"].Password);
            VerifyEqual("1. Verify Back Office is NOT found in Desktop page", false, desktopPage.CheckAppInstalled(App.BackOffice));

            //Reset language for admin user if it's changed
            var usersPage = desktopPage.GoToApp(App.Users) as UsersPage;
            usersPage.UserProfileListPanel.SelectProfile(Settings.Users["admin"].Profile);
            var language = usersPage.UserProfileDetailsPanel.GetLanguageValue();
            if(!Settings.Users["admin"].Language.Equals(language))
            {
                usersPage.UserProfileDetailsPanel.SelectLanguageDropDown(Settings.Users["admin"].Language);
                usersPage.UserProfileDetailsPanel.ClickSaveButton();
                usersPage.WaitForPreviousActionComplete();
                usersPage.WaitForHeaderMessageDisappeared();
            }
        }

        [Test, DynamicRetry]
        [Description("BO_01_02 App availability - Back Office is not found in Store")]
        public void BO_01_02()
        {
            Step("**** Precondition ****");
            Step(" - User must belong to root/admin group");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            Step("1. Go to Store from Settings menu");
            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["admin"].Username, Settings.Users["admin"].Password);

            Step("2. Expected Verify Back Office is NOT found in application and widget list");
            desktopPage.AppBar.ClickSettingsButton();
            desktopPage.SettingsPanel.ClickStoreLink();
            desktopPage.SettingsPanel.WaitForStorePanelDisplayed();
            var listStoreApps = desktopPage.SettingsPanel.StorePanel.GetAllAppsName();
            VerifyEqual("2. Verify Back Office is NOT found in application and widget list", false, listStoreApps.Contains(App.BackOffice));
        }

        [Test, DynamicRetry]
        [Description("BO_01_03 App availability - Back Office is not found in Users")]
        public void BO_01_03()
        {
            var profiles = Settings.Users.Select(p => p.Value.Profile).ToList();

            Step("**** Precondition ****");
            Step(" - User must belong to root/admin group");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["admin"].Username, Settings.Users["admin"].Password);

            Step("1. Go to Users app");
            Step("2. Expected Users page is routed and loaded successfully");
            var usersPage = desktopPage.GoToApp(App.Users) as UsersPage;

            Step("3. Select in turn root, admin and an any profile");
            Step("4. Expected Verify Back Office is NOT found in Applications list");
            foreach (var profile in profiles)
            {
                usersPage.UserProfileListPanel.SelectProfile(profile);
                usersPage.UserProfileDetailsPanel.WaitForNameDisplayed(profile);
                var allApps = usersPage.UserProfileDetailsPanel.GetAllAppsName();
                VerifyEqual(string.Format("[{0}] 4. Verify Verify Back Office is NOT found in Applications list", profile), false, allApps.Contains(App.BackOffice));
            }
        }

        [Test, DynamicRetry]
        [Description("BO_01_04 App availability - Back Office is accessed via specific URL")]
        public void BO_01_04()
        {
            Step("**** Precondition ****");
            Step(" - User must belong to root/admin group");
            Step(" - User has NOT logged in");
            Step("**** Precondition ****\n");

            Step("1. Go to Back Office url (looks like 'https://slvci-01.eng.ssnsgs.net:8443/reports/groundcontrol/?app=BackOffice')");
            Step("2. Expected Login page is routed and loaded successfully");
            var loginPage = Browser.OpenBackOfficeApp();

            Step("3. Log in");
            var backOfficePage = loginPage.LoginAsValidUserToBackOffice(Settings.Users["admin"].Username, Settings.Users["admin"].Password);

            Step("4. Expected Back Office app is routed and loaded successfully. Verify page title is 'Back Office'");
            VerifyEqual("4. Verify Verify Back Office is NOT found in Applications list", App.BackOffice, WebDriverContext.CurrentDriver.Title);
        }

        [Test, DynamicRetry]
        [Description("BO_02_01 Basic UI")]
        public void BO_02_01()
        {
            var testData = GetTestDataOfBO_02_01();
            var expectedAppsList = testData["Apps"] as List<string>;

            Step("**** Precondition ****");
            Step(" - User must belong to root/admin group");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenBackOfficeApp();
            var backOfficePage = loginPage.LoginAsValidUserToBackOffice(Settings.Users["admin"].Username, Settings.Users["admin"].Password);
            Step("1. Verify:");
            Step(" - Left (Option Items) panel has title 'Options'");
            Step(" - Left (Options Items) panel has a list of customizable applications:");
            Step(" - General (is being selected)");
            Step(" - Desktop");
            Step(" - Equipment Inventory");
            Step(" - Real-time Control");
            Step(" - Data history");
            Step(" - Device History");
            Step(" - Failure Analysis");
            Step(" - Failure Tracking");
            Step(" - Advanced Search");
            Step(" - Main (Option Details) panel has title 'General'");

            var actualAppsList = backOfficePage.BackOfficeOptionsPanel.GetConfigurationNameList();
            VerifyEqual("1. Verify Left (Option Items) panel has title 'Options'", "Options", backOfficePage.BackOfficeOptionsPanel.GetPanelTitleText());
            VerifyEqual("1. Verify General (is being selected)", "General", backOfficePage.BackOfficeOptionsPanel.GetSelectedConfigurationName());
            VerifyEqual("1. Verify Left (Options Items) panel has a list of customizable applications", expectedAppsList, actualAppsList, false);

            Step("2. Click each item in Option Items panel");
            Step("3. Expected");
            Step(" - Clicked item becomes selected");
            Step(" - Option Details panel updates its title to clicked item");
            actualAppsList.Remove("General");
            foreach (var app in actualAppsList)
            {
                backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(app);
                VerifyEqual(string.Format("3. Verify '{0}' item becomes selected", app), app, backOfficePage.BackOfficeOptionsPanel.GetSelectedConfigurationName());
                VerifyEqual(string.Format("3. Verify Option Details panel updates its title to clicked item '{0}'", app), app, backOfficePage.BackOfficeDetailsPanel.GetPanelTitleText());
            }
        }

        [Test, DynamicRetry]
        [Description("BO_03_01 Options - General - UI")]
        public void BO_03_01()
        {
            Step("**** Precondition ****");
            Step(" - User must belong to root/admin group");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenBackOfficeApp();
            var backOfficePage = loginPage.LoginAsValidUserToBackOffice(Settings.Users["admin"].Username, Settings.Users["admin"].Password);
            Step("1. Select General item");

            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration("General");
            Step("2. Expected UI as defined");
            VerifyEqual("2. Verify Authentication timeout", "Authentication timeout", backOfficePage.BackOfficeDetailsPanel.GetGeneralTimeoutAuthenticationText());
            VerifyEqual("2. Verify Authentication timeout input enable", true, backOfficePage.BackOfficeDetailsPanel.IsGeneralTimeoutAuthenticationNumericInputEditable());
            VerifyEqual("2. Verify Authentication timeout description", "Timeout (in minutes) before users are automatically logged out.", backOfficePage.BackOfficeDetailsPanel.GetGeneralTimeoutAuthenticationDescText());

            VerifyEqual("2. Verify Default report configuration", "Default report configuration", backOfficePage.BackOfficeDetailsPanel.GetGeneralDefaultReportConfigurationText());
            VerifyEqual("2. Verify Export CSV separator", "Export CSV separator", backOfficePage.BackOfficeDetailsPanel.GetGeneralExportCsvSeparatorText());
            VerifyEqual("2. Verify Export CSV separator input enable", true, backOfficePage.BackOfficeDetailsPanel.IsGeneralExportCsvSeparatorInputEditable());
            VerifyEqual("2. Verify Export CSV separator description", "Separator used in CSV files provided by the SLV CMS.", backOfficePage.BackOfficeDetailsPanel.GetGeneralExportCsvSeparatorDescText());

            VerifyEqual("2. Verify Default search configuration", "Default search configuration", backOfficePage.BackOfficeDetailsPanel.GetGeneralDefaultSearchConfigurationText());
            VerifyEqual("2. Verify Search attributes", "Search attributes", backOfficePage.BackOfficeDetailsPanel.GetGeneralSearchAttributesText());
            VerifyEqual("2. Verify Search attributes list", true, backOfficePage.BackOfficeDetailsPanel.GetGeneralSearchAttributsNameList().Count > 0);
            VerifyEqual("2. Verify Search attributes description", "Attributes available in the search bar at the bottom of the geozone tree view.", backOfficePage.BackOfficeDetailsPanel.GetGeneralSearchAttributesDescText());
        }

        [Test, DynamicRetry]
        [Description("BO_03_02 Options - General - Timeout authentication")]
        public void BO_03_02()
        {
            var testData = GetTestDataOfBO_03_02();
            var expectedTimeout = testData["Timeout"];

            Step("**** Precondition ****");
            Step(" - User must belong to root/admin group");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenBackOfficeApp();
            var backOfficePage = loginPage.LoginAsValidUserToBackOffice(Settings.Users["admin"].Username, Settings.Users["admin"].Password);
            Step("1. Select General item");
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration("General");

            Step("2. Set Timeout authentication = {{timeout minutes}} then save");
            var currentTimeout = backOfficePage.BackOfficeDetailsPanel.GetGeneralTimeoutAuthenticationValue();
            backOfficePage.BackOfficeDetailsPanel.EnterGeneralTimeoutAuthenticationNumericInput(expectedTimeout);
            backOfficePage.BackOfficeDetailsPanel.ClickSaveButton();
            backOfficePage.WaitForPreviousActionComplete();

            Step("3. Reload Back Office page then select General item");
            backOfficePage = Browser.RefreshBackOfficePage();
            backOfficePage.WaitForPreviousActionComplete();
            backOfficePage.BackOfficeOptionsPanel.WaitForPanelLoaded();

            Step("4. Expected New timeout authentication is remained");
            var actualTimeout = backOfficePage.BackOfficeDetailsPanel.GetGeneralTimeoutAuthenticationValue();
            VerifyEqual("4. Verify New timeout authentication is remained", expectedTimeout, actualTimeout);

            Step("5. Go to SLV app");
            var desktopPage = Browser.NavigateToLoggedInCMS();
            var usersPage = desktopPage.GoToApp(App.Users) as UsersPage;
            Step("-> Time: " + DateTime.Now.ToLongTimeString());

            Step("6. Wait for {timeout minutes - 1} minute (if {timeout minutes} = 1, wait exactly for 1 minute)");
            var waitTime = expectedTimeout == "1" ? 1 : int.Parse(expectedTimeout) - 1;
            Wait.ForMinutes(waitTime);
            Step("-> Time: " + DateTime.Now.ToLongTimeString());
            usersPage.WaitForHeaderMessageDisplayed();
            Step("7. Expected Topbar message appears with text 'You will be automatically logged out in 1 minute.To remain logged in move your mouse over this window.'");
            var expectedHeaderMessage = "You will be automatically logged out in 1 minute.To remain logged in move your mouse over this window.";
            var actualHeaderMessage = usersPage.GetHeaderMessage();
            VerifyEqual(string.Format("7. Verify Topbar message appears with text '{0}'", expectedHeaderMessage), expectedHeaderMessage, actualHeaderMessage);

            Step("-> Time: " + DateTime.Now.ToLongTimeString());
            Step("8. Wait for 1 minute");
            Wait.ForMinutes(1);
            Step("-> Time: " + DateTime.Now.ToLongTimeString());

            Step("9. Expected Logout page is redirected");
            var logoutPage = usersPage.TimeoutAuthentication();
            VerifyEqual("9. Verify Logout page is redirected", "You are now disconnected from the Streetlight.Vision CMS Web Server", logoutPage.GetLogoutMessageText());

            Step("10. Reload browser");
            loginPage = Browser.RefreshLoginPage();

            Step("11. Expected Login page is routed and loaded successfully");
            VerifyEqual("11. Verify Login page is loaded", "Sign In", loginPage.GetLoginTitleText());

            try
            {
                var timeoutToRestore = "120";
                Info(string.Format("Reset Timeout to {0}", timeoutToRestore));
                loginPage = Browser.OpenBackOfficeApp();
                backOfficePage = loginPage.LoginAsValidUserToBackOffice(Settings.Users["admin"].Username, Settings.Users["admin"].Password);
                backOfficePage.BackOfficeOptionsPanel.SelectConfiguration("General");
                backOfficePage.BackOfficeDetailsPanel.EnterGeneralTimeoutAuthenticationNumericInput(timeoutToRestore);
                backOfficePage.BackOfficeDetailsPanel.ClickSaveButton();
                backOfficePage.WaitForPreviousActionComplete();
            }
            catch { }
        }        

        [Test, DynamicRetry]
        [Description("BO_03_04 Options - General - Export CSV separator")]
        public void BO_03_04()
        {
            var testData = GetTestDataOfBO_03_04();
            var xmlCsvSeparator = testData["CsvSeparator"];

            Step("**** Precondition ****");
            Step(" - User must belong to root/admin group");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenBackOfficeApp();
            var backOfficePage = loginPage.LoginAsValidUserToBackOffice(Settings.Users["admin"].Username, Settings.Users["admin"].Password);

            Step("1. Select General item");
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration("General");

            Step("2. Set Export CSV separator = {{separator}} then save");
            var currentCsvSeparator = backOfficePage.BackOfficeDetailsPanel.GetGeneralExportCsvSeparatorValue();
            backOfficePage.BackOfficeDetailsPanel.EnterGeneralExportCsvSeparatorInput(xmlCsvSeparator);
            backOfficePage.BackOfficeDetailsPanel.ClickSaveButton();
            backOfficePage.WaitForPreviousActionComplete();

            Step("3. Reload Back Office page then select General item");
            backOfficePage = Browser.RefreshBackOfficePage();
            backOfficePage.WaitForPreviousActionComplete();
            backOfficePage.BackOfficeOptionsPanel.WaitForPanelLoaded();

            Step("4. Expected New Export CSV separator is remained");
            var actualCsvSeparator = backOfficePage.BackOfficeDetailsPanel.GetGeneralExportCsvSeparatorValue();
            VerifyEqual("4. Verify New Export CSV separator is remained", xmlCsvSeparator, actualCsvSeparator);

            Step("5. Go to SLV app");
            var desktopPage = Browser.NavigateToLoggedInCMS();
            desktopPage.InstallAppsIfNotExist(App.DataHistory, App.DeviceHistory);

            Step("6. Go to Data History");
            Step("7. Click Generate CSV button then click Download button");
            Step("8. Expected A CSV file is downloaded and its separator is {{separator}}");
            var dataHistoryPage = desktopPage.GoToApp(App.DataHistory) as DataHistoryPage;
            dataHistoryPage.WaitForGridPanelDisplayed();
            SLVHelper.DeleteAllFilesByPattern(_dataHistoryGridExportedFilePattern);
            dataHistoryPage.GridPanel.ClickGenerateCSVToolbarButton();
            dataHistoryPage.WaitForPreviousActionComplete();
            dataHistoryPage.GridPanel.ClickDownloadToolbarButton();
            SLVHelper.SaveDownloads();
            var headerLine = SLVHelper.GetHeaderLineFromDownloadedCSV(_dataHistoryGridExportedFilePattern);
            int separatorCount = headerLine.Count(p => p == char.Parse(xmlCsvSeparator));
            VerifyTrue(string.Format("8. Verify A CSV file is downloaded and its separator is '{0}'", xmlCsvSeparator), separatorCount > 1, string.Format("'{0}' exists in CSV", xmlCsvSeparator), string.Format("'{0}' does not exist in CSV", xmlCsvSeparator));

            try
            {
                Info(string.Format("Reset CSV separator to '{0}'", currentCsvSeparator));
                backOfficePage = Browser.NavigateToLoggedInBackOfficeApp();
                backOfficePage.BackOfficeOptionsPanel.SelectConfiguration("General");
                backOfficePage.BackOfficeDetailsPanel.EnterGeneralExportCsvSeparatorInput(currentCsvSeparator);
                backOfficePage.BackOfficeDetailsPanel.ClickSaveButton();
                backOfficePage.WaitForPreviousActionComplete();
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("BO_03_05 Options - General - Search attributes")]
        public void BO_03_05()
        {
            Step("**** Precondition ****");
            Step(" - User must belong to root/admin group");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenBackOfficeApp();
            var backOfficePage = loginPage.LoginAsValidUserToBackOffice(Settings.Users["admin"].Username, Settings.Users["admin"].Password);

            Step("1. Select General item");
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration("General");
            var expectedAttributes = backOfficePage.GetAttributesKey();

            Step("2. Randomly remove one of following attributes then save");
            Step(" - name (Name)");
            Step(" - idOnController (Identifier)");
            Step(" - controllerStrId (Controller ID)");
            Step(" - categoryStrId (Category)");
            Step(" - MacAddress (Unique address)");
            Step(" - DimmingGroupName (Dimming group)");
            Step(" - installStatus (Install status)");
            Step(" - ConfigStatus (Configuration status)");
            Step(" - address (Address 1)");
            Step(" - location.streetdescription (Address 2)");
            Step(" - location.city (City)");
            Step(" - location.zipcode (Zip code)");
            Step(" - client.name (Customer name)");
            Step(" - client.number (Customer number)");
            Step(" - luminaire.model (Luminaire model)");
            Step(" - SoftwareVersion (Software version)");
            Step(" - comment (Comment)");
            var currentAttributes = backOfficePage.BackOfficeDetailsPanel.GetGeneralSearchAttributsNameList();
            expectedAttributes.Sort();
            currentAttributes.Sort();
            VerifyEqual("2. Verify Attributes are matched as expected", expectedAttributes, currentAttributes, false);

            var randomAttributes = expectedAttributes.PickRandom(2);
            var remainingAttributes = expectedAttributes.Clone().ToList();
            remainingAttributes.RemoveAll(p => randomAttributes.Contains(p));
            backOfficePage.BackOfficeDetailsPanel.RemoveGeneralAttributes(randomAttributes.ToArray());
            backOfficePage.BackOfficeDetailsPanel.ClickSaveButton();
            backOfficePage.WaitForPreviousActionComplete();
            var expectedAttribuesName = backOfficePage.GetAttributesName(remainingAttributes.ToArray());
            Step(" --> Removed attributes: {0}", string.Join(", ", randomAttributes));
            Step(" --> Remaining attributes: {0}", string.Join(", ", remainingAttributes));
            Step(" --> Remaining attributes name: {0}", string.Join(", ", expectedAttribuesName));

            Step("3. Reload Back Office page then select General item");
            backOfficePage = Browser.RefreshBackOfficePage();
            backOfficePage.WaitForPreviousActionComplete();
            backOfficePage.BackOfficeOptionsPanel.WaitForPanelLoaded();
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration("General");

            Step("4. Expected Removed attributes are no longer in the list, intact are remained");
            var actualAttributes = backOfficePage.BackOfficeDetailsPanel.GetGeneralSearchAttributsNameList();
            Step(" --> Actual attributes: {0}", string.Join(", ", actualAttributes));
            VerifyTrue("Verify Removed attributes are no longer in the list", !actualAttributes.Exists(p => randomAttributes.Contains(p)), string.Join(", ", randomAttributes), string.Join(", ", actualAttributes));            
            remainingAttributes.Sort();
            actualAttributes.Sort();
            VerifyEqual("4. Verify Attribues intact are remained", remainingAttributes, actualAttributes, false);

            Step("5. Go to SLV app");
            var desktopPage = Browser.NavigateToLoggedInCMS();
            desktopPage.InstallAppsIfNotExist(App.EquipmentInventory, App.RealTimeControl);

            Step("6. Go to apps which invoke geozone tree (Real-time Control, etc.)");
            Step("7. Expand Search bar at the bottom");
            Step("8. Expected Verify attribute dropdown contains all attributes in Search attribute list");

            Step("-> Go to Real-time Control");
            var realtimeControlPage = desktopPage.GoToApp(App.RealTimeControl) as RealTimeControlPage;
            realtimeControlPage.GeozoneTreeMainPanel.ClickExpandSearchButton();
            var attributesNameDropDown = realtimeControlPage.GeozoneTreeMainPanel.GetAllAttributeDropDownItems();
            Step(" --> Dropdown attributes name: {0}", string.Join(", ", attributesNameDropDown));
            VerifyEqual("Verify Verify attribute dropdown contains all attributes in Search attribute list", expectedAttribuesName, attributesNameDropDown, false);
            Step("9. Back to Back Office and select General item");
            backOfficePage = Browser.NavigateToLoggedInBackOfficeApp();
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration("General");

            Step("10. Randomly add some of removed attributes at step #2 then save");
            backOfficePage.BackOfficeDetailsPanel.AddGeneralAttributes(randomAttributes.ToArray());
            backOfficePage.BackOfficeDetailsPanel.ClickSaveButton();
            backOfficePage.WaitForPreviousActionComplete();
            expectedAttribuesName = backOfficePage.GetAttributesName(expectedAttributes.ToArray());
            Step(" --> Added attributes: {0}", string.Join(", ", randomAttributes));
            Step(" --> All attributes name: {0}", string.Join(", ", expectedAttribuesName));

            Step("11. Reload Back Office page then select General item");
            backOfficePage = Browser.RefreshBackOfficePage();
            backOfficePage.WaitForPreviousActionComplete();
            backOfficePage.BackOfficeOptionsPanel.WaitForPanelLoaded();
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration("General");

            Step("12. Expected Intact and new ones are displayed");
            actualAttributes = backOfficePage.BackOfficeDetailsPanel.GetGeneralSearchAttributsNameList();
            VerifyEqual("12. Verify Intact and new ones are displayed", expectedAttributes, actualAttributes, false);

            Step("13. Go to SLV app");
            desktopPage = Browser.NavigateToLoggedInCMS();

            Step("14. Go to apps which invoke geozone tree (Equipment Inventory, Real-time Control, etc.)");
            Step("15. Expand Search bar at the bottom");
            Step("16. Expected Verify attribute dropdown contains all attributes in Search attribute list");
            Step("-> Go to Real-time Control");
            realtimeControlPage = desktopPage.GoToApp(App.RealTimeControl) as RealTimeControlPage;
            realtimeControlPage.GeozoneTreeMainPanel.ClickExpandSearchButton();
            attributesNameDropDown = realtimeControlPage.GeozoneTreeMainPanel.GetAllAttributeDropDownItems();
            Step(" --> Dropdown attributes name: {0}", string.Join(", ", attributesNameDropDown));
            VerifyEqual("16. Verify Verify attribute dropdown contains all attributes in Search attribute list", expectedAttribuesName, attributesNameDropDown, false);
        }

        [Test, DynamicRetry]
        [Description("BO_04_01 Options - Desktop - UI")]
        public void BO_04_01()
        {
            Step("**** Precondition ****");
            Step(" - User must belong to root/admin group");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenBackOfficeApp();
            var backOfficePage = loginPage.LoginAsValidUserToBackOffice(Settings.Users["admin"].Username, Settings.Users["admin"].Password);
            Step("1. Select Desktop item");

            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration("Desktop");
            Step("2. Expected UI as defined");
            VerifyEqual("2. Verify Applications list label", "Applications list", backOfficePage.BackOfficeDetailsPanel.GetDesktopApplicationsListText());
            VerifyEqual("2. Verify Applications list", true, backOfficePage.BackOfficeDetailsPanel.GetDesktopAllApplicationsNameList().Count > 0);
            VerifyEqual("2. Verify Applications list description", "The list of apps added by default on the desktop.", backOfficePage.BackOfficeDetailsPanel.GetDesktopApplicationsListDescText());
            VerifyEqual("2. Verify Widgets list label", "Widgets list", backOfficePage.BackOfficeDetailsPanel.GetDesktopWidgetsListText());
            VerifyEqual("2. Verify Widgets list", true, backOfficePage.BackOfficeDetailsPanel.GetDesktopAllWidgetsNameList().Count > 0);
            VerifyEqual("2. Verify Widgets list description", "The list of widgets added by default on the desktop.", backOfficePage.BackOfficeDetailsPanel.GetDesktopWidgetsListDescText());            
        }

        [Test, DynamicRetry]
        [Description("BO_04_02 Options - Desktop - Applications Widgets List")]
        public void BO_04_02()
        {
            var testData = GetTestDataOfBO_04_02();
            var expectedAppsList = testData["Apps"] as List<string>;
            var expectedWidgetsList = testData["Widgets"] as List<string>;

            Step("**** Precondition ****");
            Step(" - User must belong to root/admin group");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenBackOfficeApp();
            var backOfficePage = loginPage.LoginAsValidUserToBackOffice(Settings.Users["admin"].Username, Settings.Users["admin"].Password);
            Step("1. Select Desktop item");
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration("Desktop");

            Step("2. Expected");
            Step("- List of applications:");
            Step(" + Failure Analysis");
            Step(" + Failure Tracking");
            Step(" + Real-time Control");
            Step(" + Batch Control");
            Step(" + Alarms");
            Step(" + Energy");
            Step(" + Monthly Energy Saving");
            Step(" + Device lifetime");
            Step(" + Data history");
            Step(" + Device History");
            Step(" + Equipment Inventory");
            Step(" + Users");
            Step(" + Alarm Manager");
            Step(" + Report Manager");
            Step(" + Scheduling Manager");
            Step(" + Dashboard");
            Step(" + Work Orders");
            Step(" + Control Center");
            Step(" + Inventory Verification");
            Step(" + Installation");
            Step(" + Advanced Search");
            Step(" + Log Viewer");
            Step("- List of widgets:");
            Step(" + Gateway");
            Step(" + Luminaire Controller");
            Step(" + Circutor CVM Meter");
            Step(" + Electrical Meter");
            Step(" + Camera");
            Step(" + Weather");
            Step(" + Clock");
            Step(" + Vehicule Charging Station");
            Step(" + Geozone Failures Monitor");
            Step(" + Pollution Sensor Widget");
            Step(" + XCam Monitor");
            Step(" + Sunrise Sunset Times");
            Step(" + IoT Edge Router Status");
            Step(" + Spoony widget");
            Step(" + Security widget");
            Step(" + Environmental Sensor");
            var actualAppsList = backOfficePage.BackOfficeDetailsPanel.GetDesktopAllApplicationsNameList();
            actualAppsList.Remove("FailureTrackingGL"); //Remove this app            
            VerifyEqual("2. Verify Apps as expected", expectedAppsList, actualAppsList, false);
            var actualWidgetList = backOfficePage.BackOfficeDetailsPanel.GetDesktopAllWidgetsNameList();
            VerifyEqual("2. Verify Widgets as expected", expectedWidgetsList, actualWidgetList, false);

            Step("3. Randomly un/checked applications and widgets in lists then save");
            backOfficePage.BackOfficeDetailsPanel.UncheckRandomApps(2);
            backOfficePage.BackOfficeDetailsPanel.CheckRandomApps(2);
            backOfficePage.BackOfficeDetailsPanel.CheckApps(App.Users); //for remove user after testing
            backOfficePage.BackOfficeDetailsPanel.UncheckRandomWidgets(1);
            backOfficePage.BackOfficeDetailsPanel.CheckRandomWidgets(2);            
            backOfficePage.BackOfficeDetailsPanel.ClickSaveButton();
            backOfficePage.WaitForPreviousActionComplete();
            var checkedApps = backOfficePage.BackOfficeDetailsPanel.GetDesktopAvailableApplicationsNameList();
            var uncheckedApps = backOfficePage.BackOfficeDetailsPanel.GetDesktopDisableApplicationsNameList();
            var checkedWidget = backOfficePage.BackOfficeDetailsPanel.GetDesktopAvailableWidgetsNameList();
            var uncheckedWidget = backOfficePage.BackOfficeDetailsPanel.GetDesktopDisableWidgetsNameList();

            Step("4. Reload Back Office page then select Desktop item");
            backOfficePage = Browser.RefreshBackOfficePage();
            backOfficePage.WaitForPreviousActionComplete();
            backOfficePage.BackOfficeOptionsPanel.WaitForPanelLoaded();
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration("Desktop");

            Step("5. Expected Un/checked items are remained");
            Step("--> SLV-3177: Back Office - Cannot save from the Desktop, Failure Analysis and Failure Tracking pages");
            var actualCheckedApps = backOfficePage.BackOfficeDetailsPanel.GetDesktopAvailableApplicationsNameList();
            var actualUncheckedApps = backOfficePage.BackOfficeDetailsPanel.GetDesktopDisableApplicationsNameList();
            var actualCheckedWidget = backOfficePage.BackOfficeDetailsPanel.GetDesktopAvailableWidgetsNameList();
            var actualUncheckedWidget = backOfficePage.BackOfficeDetailsPanel.GetDesktopDisableWidgetsNameList();
            VerifyEqual("5. Verify checked apps are remained", checkedApps, actualCheckedApps, false);
            VerifyEqual("5. Verify unchecked apps are remained", uncheckedApps, actualUncheckedApps, false);
            VerifyEqual("5. Verify checked widgets are remained", checkedWidget, actualCheckedWidget, false);
            VerifyEqual("5. Verify unchecked widgets are remained", uncheckedWidget, actualUncheckedWidget, false);

            Step("6. Go to SLV app");
            var desktopPage = Browser.NavigateToLoggedInCMS();
            desktopPage.InstallAppsIfNotExist(App.Users);
            var usersPage = desktopPage.GoToApp(App.Users) as UsersPage;

            Step("7. Create a new user and log in with that user");
            var userModel = usersPage.CreateNewProfileAndUser();
            desktopPage = SLVHelper.LogoutAndLogin(desktopPage, userModel.Username, userModel.Password);           

            Step("8. Expected Verify Desktop page displays only checked applications and widgets");
            var desktopApps = desktopPage.GetInstalledAppsName();
            VerifyEqual("8. Verify Desktop page displays only checked applications", actualCheckedApps, desktopApps, false);

            var desktopWidgets = desktopPage.GetInstalledWidgetsName();
            VerifyEqual("8. Verify Desktop page displays only checked widgets", actualCheckedWidget, desktopWidgets, false);

            try
            {
                DeleteUserAndProfile(userModel);
            }
            catch { }

        }        

        [Test, DynamicRetry]
        [Description("BO_05_01 Options - Equipment Inventory - UI")]
        public void BO_05_01()
        {
            Step("**** Precondition ****");
            Step(" - User must belong to root/admin group");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenBackOfficeApp();
            var backOfficePage = loginPage.LoginAsValidUserToBackOffice(Settings.Users["admin"].Username, Settings.Users["admin"].Password);
            Step("1. Select Equipment Inventory item");

            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.EquipmentInventory);
            Step("2. Expected UI as defined");
            VerifyEqual("2. Verify Treeview configuration", "Treeview configuration", backOfficePage.BackOfficeDetailsPanel.GetEquipmentTreeviewConfigurationText());
            VerifyEqual("2. Verify Display devices", "Display devices", backOfficePage.BackOfficeDetailsPanel.GetEquipmentDisplayDevicesText());
            VerifyEqual("2. Verify Display devices checkbox enable", true, backOfficePage.BackOfficeDetailsPanel.IsEquipmentDisplayDevicesCheckboxEditable());
            VerifyEqual("2. Verify Display devices description", "Defines if devices are displayed in geozone tree view or not.", backOfficePage.BackOfficeDetailsPanel.GetEquipmentDisplayDevicesDescText());
            VerifyEqual("2. Verify Enable map filter", "Enable map filter", backOfficePage.BackOfficeDetailsPanel.GetEquipmentMapFilterText());
            VerifyEqual("2. Verify Enable map filter checkbox enable", true, backOfficePage.BackOfficeDetailsPanel.IsEquipmentMapFilterCheckboxEditable());
            VerifyEqual("2. Verify Enable map filter description", "Defines if the filter on devices found in the search is allowed on the map.", backOfficePage.BackOfficeDetailsPanel.GetEquipmentMapFilterDescText());

            VerifyEqual("2. Verify Search attributes", "Search attributes", backOfficePage.BackOfficeDetailsPanel.GetEquipmentSearchAttributesText());
            VerifyEqual("2. Verify Search attributes list", true, backOfficePage.BackOfficeDetailsPanel.GetEquipmentSearchAttributesNameList().Count > 0);
            VerifyEqual("2. Verify Search attributes description", "Attributes available in the search bar at the bottom of the geozone tree view.", backOfficePage.BackOfficeDetailsPanel.GetEquipmentSearchAttributesDescText());

            VerifyEqual("2. Verify Editor configuration", "Editor configuration", backOfficePage.BackOfficeDetailsPanel.GetEquipmentEditorConfigurationText());
            VerifyEqual("2. Verify Enable device location", "Enable device location", backOfficePage.BackOfficeDetailsPanel.GetEquipmentDeviceLocationText());
            VerifyEqual("2. Verify Enable device location checkbox enable", true, backOfficePage.BackOfficeDetailsPanel.IsEquipmentDeviceLocationCheckboxEditable());
            VerifyEqual("2. Verify Enable device location description", "Enables location (longitude, latitude) field in the device editor.", backOfficePage.BackOfficeDetailsPanel.GetEquipmentDeviceLocationDescText());
            VerifyEqual("2. Verify Enable geozone parent", "Enable geozone parent", backOfficePage.BackOfficeDetailsPanel.GetEquipmentParentGeozoneText());
            VerifyEqual("2. Verify Enable geozone parent checkbox enable", true, backOfficePage.BackOfficeDetailsPanel.IsEquipmentParentGeozoneCheckboxEditable());
            VerifyEqual("2. Verify Enable geozone parent description", "Enables geozone parent field in the geozone editor.", backOfficePage.BackOfficeDetailsPanel.GetEquipmentParentGeozoneDescText());

            VerifyEqual("2. Verify Report  configuration", "Report configuration", backOfficePage.BackOfficeDetailsPanel.GetEquipmentReportConfigurationText());
            VerifyEqual("2. Verify Event time visible", "Event time visible", backOfficePage.BackOfficeDetailsPanel.GetEquipmentEventTimeVisibleText());
            VerifyEqual("2. Verify Event time visible checkbox enable", true, backOfficePage.BackOfficeDetailsPanel.IsEquipmentEventTimeVisibleCheckboxEditable());
            VerifyEqual("2. Verify Event time visible description", "Defines if event time columns are visibled for each attributs in the report table.", backOfficePage.BackOfficeDetailsPanel.GetEquipmentEventTimeVisibleDescText());
            VerifyEqual("2. Verify Rows count per page", "Rows count per page", backOfficePage.BackOfficeDetailsPanel.GetEquipmentRowsPerPageText());
            VerifyEqual("2. Verify Rows count per page input enable", true, backOfficePage.BackOfficeDetailsPanel.IsEquipmentRowCountPerPageInputEditable());
            VerifyEqual("2. Verify Rows count per page description", "The count of rows loaded and displayed per page in the report table.", backOfficePage.BackOfficeDetailsPanel.GetEquipmentRowsPerPageDescText());

            VerifyEqual("2. Verify Toolbar list label", "Toolbar", backOfficePage.BackOfficeDetailsPanel.GetEquipmentToolbarItemsText());
            VerifyEqual("2. Verify Toolbar list", true, backOfficePage.BackOfficeDetailsPanel.GetEquipmentToolbarItemsNameList().Count > 0);
            VerifyEqual("2. Verify Toolbar list description", "The items of toolbar availables from report grid.", backOfficePage.BackOfficeDetailsPanel.GetEquipmentToolbarItemsDescText());
        }

        [Test, DynamicRetry]
        [Description("BO_05_02 Options - Equipment Inventory - Display devices in geozone tree")]
        public void BO_05_02()
        {
            var testData = GetTestDataOfBO_05_02();
            var xmlGeozone = testData["Geozone"];

            Step("**** Precondition ****");
            Step(" - User must belong to root/admin group");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenBackOfficeApp();
            var backOfficePage = loginPage.LoginAsValidUserToBackOffice(Settings.Users["admin"].Username, Settings.Users["admin"].Password);

            Step("1. Select Equipment Inventory item");
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.EquipmentInventory);

            Step("2. Uncheck Display devices option if it is being checked or check if it's being unchecked then save");
            var firstDisplayDevicesValue = backOfficePage.BackOfficeDetailsPanel.GetEquipmentDisplayDevicesValue();
            backOfficePage.BackOfficeDetailsPanel.TickEquipmentDisplayDevicesCheckbox(!firstDisplayDevicesValue);
            backOfficePage.BackOfficeDetailsPanel.ClickSaveButton();
            backOfficePage.WaitForPreviousActionComplete();

            Step("3. Reload Back Office page then select Equipment Inventory item");
            backOfficePage = Browser.RefreshBackOfficePage();
            backOfficePage.WaitForPreviousActionComplete();
            backOfficePage.BackOfficeOptionsPanel.WaitForPanelLoaded();
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.EquipmentInventory);

            Step("4. Expected Changed value is remained");
            var actualDisplayDevices = backOfficePage.BackOfficeDetailsPanel.GetEquipmentDisplayDevicesValue();
            VerifyEqual("4. Verify Changed value is remained", !firstDisplayDevicesValue, actualDisplayDevices);

            Step("5. Go to SLV app");
            var desktopPage = Browser.NavigateToLoggedInCMS();
            desktopPage.InstallAppsIfNotExist(App.EquipmentInventory);

            Step("6. Go to Equipment Inventory page");
            var equipmentInventoryPage = desktopPage.GoToApp(App.EquipmentInventory) as EquipmentInventoryPage;
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(xmlGeozone);

            Step("7. Expected");
            Step(" - Geozone tree displays devices in case the option is checked. Otherwise, it does NOT display devices in case the option is unchecked");
            Step(" - Filters toolbar button is displayed in case the option is checked and otherwise");
            if (actualDisplayDevices)
            {
                VerifyEqual("7. Verify Geozone tree displays devices", true, equipmentInventoryPage.GeozoneTreeMainPanel.GetChildDeviceNamesOfSelectedNode().Count > 0);
                VerifyEqual("7. Verify Filters toolbar button is displayed", true, equipmentInventoryPage.GeozoneTreeMainPanel.IsFilterButtonVisible());
            }
            else
            {
                VerifyEqual("7. Verify Geozone tree does NOT display devices", false, equipmentInventoryPage.GeozoneTreeMainPanel.GetChildDeviceNamesOfSelectedNode().Count > 0);
                VerifyEqual("7. Verify Filters toolbar button is NOT displayed", false, equipmentInventoryPage.GeozoneTreeMainPanel.IsFilterButtonVisible());
            }

            Step("8. Repeat the test with Display devices option is the other way round with step #2");
            backOfficePage = Browser.NavigateToLoggedInBackOfficeApp();
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.EquipmentInventory);
            backOfficePage.BackOfficeDetailsPanel.TickEquipmentDisplayDevicesCheckbox(firstDisplayDevicesValue);
            backOfficePage.BackOfficeDetailsPanel.ClickSaveButton();
            backOfficePage.WaitForPreviousActionComplete();

            backOfficePage = Browser.RefreshBackOfficePage();
            backOfficePage.WaitForPreviousActionComplete();
            backOfficePage.BackOfficeOptionsPanel.WaitForPanelLoaded();
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.EquipmentInventory);
            actualDisplayDevices = backOfficePage.BackOfficeDetailsPanel.GetEquipmentDisplayDevicesValue();
            VerifyEqual("Verify Changed value is remained", firstDisplayDevicesValue, actualDisplayDevices);

            desktopPage = Browser.NavigateToLoggedInCMS();
            equipmentInventoryPage = desktopPage.GoToApp(App.EquipmentInventory) as EquipmentInventoryPage;
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(xmlGeozone);
            if (actualDisplayDevices)
            {
                VerifyEqual("8. Verify Geozone tree displays devices", true, equipmentInventoryPage.GeozoneTreeMainPanel.GetChildDeviceNamesOfSelectedNode().Count > 0);
                VerifyEqual("8. Verify Filters toolbar button is displayed", true, equipmentInventoryPage.GeozoneTreeMainPanel.IsFilterButtonVisible());
            }
            else
            {
                VerifyEqual("8. Verify Geozone tree does NOT display devices", false, equipmentInventoryPage.GeozoneTreeMainPanel.GetChildDeviceNamesOfSelectedNode().Count > 0);
                VerifyEqual("8. Verify Filters toolbar button is NOT displayed", false, equipmentInventoryPage.GeozoneTreeMainPanel.IsFilterButtonVisible());
            }
        }

        [Test, DynamicRetry]
        [Description("BO_05_03 Options - Equipment Inventory - Enable map filter in geozone")]
        public void BO_05_03()
        {
            var searchName = "Telematics";

            Step("**** Precondition ****");
            Step(" - User must belong to root/admin group");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenBackOfficeApp();
            var backOfficePage = loginPage.LoginAsValidUserToBackOffice(Settings.Users["admin"].Username, Settings.Users["admin"].Password);

            Step("1. Select Equipment Inventory item");
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.EquipmentInventory);

            Step("2. Uncheck Enable map option if it is being checked or check if it's being unchecked");
            var firstMapFilterValue = backOfficePage.BackOfficeDetailsPanel.GetEquipmentMapFilterValue();
            backOfficePage.BackOfficeDetailsPanel.TickEquipmentMapFilterCheckbox(!firstMapFilterValue);

            Step("3. Check Display devices option then save");
            backOfficePage.BackOfficeDetailsPanel.TickEquipmentDisplayDevicesCheckbox(true);
            backOfficePage.BackOfficeDetailsPanel.ClickSaveButton();
            backOfficePage.WaitForPreviousActionComplete();

            Step("4. Reload Back Office page then select Equipment Inventory item");
            backOfficePage = Browser.RefreshBackOfficePage();
            backOfficePage.WaitForPreviousActionComplete();
            backOfficePage.BackOfficeOptionsPanel.WaitForPanelLoaded();
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.EquipmentInventory);

            Step("5. Expected Changed value is remained");
            var actualMapFilter = backOfficePage.BackOfficeDetailsPanel.GetEquipmentMapFilterValue();
            VerifyEqual("5. Verify Changed value is remained", !firstMapFilterValue, actualMapFilter);

            Step("6. Go to SLV app");
            var desktopPage = Browser.NavigateToLoggedInCMS();
            desktopPage.InstallAppsIfNotExist(App.EquipmentInventory);

            Step("7. Go to Equipment Inventory page");
            var equipmentInventoryPage = desktopPage.GoToApp(App.EquipmentInventory) as EquipmentInventoryPage;
            equipmentInventoryPage.GeozoneTreeMainPanel.ChangeSearchAttribute("Name", "Contains");
            equipmentInventoryPage.GeozoneTreeMainPanel.EnterSearchTextInput(searchName);
            equipmentInventoryPage.GeozoneTreeMainPanel.ClickSearchButton();
            equipmentInventoryPage.WaitForPreviousActionComplete();

            Step("8. Expected Search Results panel appears. 'Filter results on the map' toolbar button is displayed in case the option is checked and otherwise");
            VerifyEqual("8. Verify Search Results panel appears. 'Filter results on the map' toolbar button is displayed in case the option is checked and otherwise", !firstMapFilterValue, equipmentInventoryPage.GeozoneTreeMainPanel.SearchResultsGeozonePanel.IsMapFilterButtonVisible());

            Step("9. Repeat the test with Enable map option is the other way round with step #2");
            backOfficePage = Browser.NavigateToLoggedInBackOfficeApp();
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.EquipmentInventory);
            backOfficePage.BackOfficeDetailsPanel.TickEquipmentMapFilterCheckbox(firstMapFilterValue);
            backOfficePage.BackOfficeDetailsPanel.ClickSaveButton();
            backOfficePage.WaitForPreviousActionComplete();

            backOfficePage = Browser.RefreshBackOfficePage();
            backOfficePage.WaitForPreviousActionComplete();
            backOfficePage.BackOfficeOptionsPanel.WaitForPanelLoaded();
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.EquipmentInventory);
            actualMapFilter = backOfficePage.BackOfficeDetailsPanel.GetEquipmentMapFilterValue();
            VerifyEqual("9. Verify Changed value is remained", firstMapFilterValue, actualMapFilter);

            desktopPage = Browser.NavigateToLoggedInCMS();
            desktopPage.InstallAppsIfNotExist(App.EquipmentInventory);
            equipmentInventoryPage = desktopPage.GoToApp(App.EquipmentInventory) as EquipmentInventoryPage;
            equipmentInventoryPage.GeozoneTreeMainPanel.ChangeSearchAttribute("Name", "Contains");
            equipmentInventoryPage.GeozoneTreeMainPanel.EnterSearchTextInput(searchName);
            equipmentInventoryPage.GeozoneTreeMainPanel.ClickSearchButton();
            equipmentInventoryPage.WaitForPreviousActionComplete();
            VerifyEqual("9. Verify Search Results panel appears. 'Filter results on the map' toolbar button is displayed in case the option is checked and otherwise", firstMapFilterValue, equipmentInventoryPage.GeozoneTreeMainPanel.SearchResultsGeozonePanel.IsMapFilterButtonVisible());
        }

        [Test, DynamicRetry]
        [Description("BO_05_04 Options - Equipment Inventory - Enable device location in device editor")]
        public void BO_05_04()
        {
            var testData = GetTestDataOfBO_05_04();
            var xmlGeozone = testData["Geozone"];

            Step("**** Precondition ****");
            Step(" - User must belong to root/admin group");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenBackOfficeApp();
            var backOfficePage = loginPage.LoginAsValidUserToBackOffice(Settings.Users["admin"].Username, Settings.Users["admin"].Password);

            Step("1. Select Equipment Inventory item");
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.EquipmentInventory);

            Step("2. Uncheck Enable device location if it is being checked or check if it's being unchecked");
            var firstDeviceLocationValue = backOfficePage.BackOfficeDetailsPanel.GetEquipmentDeviceLocationValue();
            backOfficePage.BackOfficeDetailsPanel.TickEquipmentDeviceLocationCheckbox(!firstDeviceLocationValue);
            backOfficePage.BackOfficeDetailsPanel.ClickSaveButton();
            backOfficePage.WaitForPreviousActionComplete();

            Step("3. Reload Back Office page then select Equipment Inventory item");
            backOfficePage = Browser.RefreshBackOfficePage();
            backOfficePage.WaitForPreviousActionComplete();
            backOfficePage.BackOfficeOptionsPanel.WaitForPanelLoaded();
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.EquipmentInventory);

            Step("4. Expected Changed value is remained");
            var actualDeviceLocation = backOfficePage.BackOfficeDetailsPanel.GetEquipmentDeviceLocationValue();
            VerifyEqual("4. Verify Changed value is remained", !firstDeviceLocationValue, actualDeviceLocation);

            Step("5. Go to SLV app");
            var desktopPage = Browser.NavigateToLoggedInCMS();
            desktopPage.InstallAppsIfNotExist(App.EquipmentInventory);

            Step("6. Go to Equipment Inventory page");
            var equipmentInventoryPage = desktopPage.GoToApp(App.EquipmentInventory) as EquipmentInventoryPage;

            Step("7. Select a device");
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(xmlGeozone);
            var devices = equipmentInventoryPage.GeozoneTreeMainPanel.GetChildNodeNamesOfSelectedNode(NodeType.Streetlight);
            var device = devices.PickRandom();
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(device);

            Step("8. Expected Longitude and latitude fields are editable in case the option is checked and otherwise the fields are read-only");
            var isLongitudeReadOnly = equipmentInventoryPage.StreetlightEditorPanel.IsLongitudeInputReadOnly();
            var isLatitudeReadOnly = equipmentInventoryPage.StreetlightEditorPanel.IsLatitudeInputReadOnly();
            if (actualDeviceLocation)
            {
                VerifyEqual("8. Verify Longitude field is editable", false, isLongitudeReadOnly);
                VerifyEqual("8. Verify Latitude field is editable", false, isLatitudeReadOnly);
            }
            else
            {
                VerifyEqual("8. Verify Longitude field is read-only", true, isLongitudeReadOnly);
                VerifyEqual("8. Verify Latitude field is read-only", true, isLatitudeReadOnly);
            }

            Step("9. Repeat the test with Enable device location option is the other way round with step #2");
            backOfficePage = Browser.NavigateToLoggedInBackOfficeApp();
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.EquipmentInventory);
            backOfficePage.BackOfficeDetailsPanel.TickEquipmentDeviceLocationCheckbox(firstDeviceLocationValue);
            backOfficePage.BackOfficeDetailsPanel.ClickSaveButton();
            backOfficePage.WaitForPreviousActionComplete();

            backOfficePage = Browser.RefreshBackOfficePage();
            backOfficePage.WaitForPreviousActionComplete();
            backOfficePage.BackOfficeOptionsPanel.WaitForPanelLoaded();
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.EquipmentInventory);
            actualDeviceLocation = backOfficePage.BackOfficeDetailsPanel.GetEquipmentDeviceLocationValue();
            VerifyEqual("9. Verify Changed value is remained", firstDeviceLocationValue, actualDeviceLocation);

            desktopPage = Browser.NavigateToLoggedInCMS();
            equipmentInventoryPage = desktopPage.GoToApp(App.EquipmentInventory) as EquipmentInventoryPage;
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(xmlGeozone);
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(device);
            isLongitudeReadOnly = equipmentInventoryPage.StreetlightEditorPanel.IsLongitudeInputReadOnly();
            isLatitudeReadOnly = equipmentInventoryPage.StreetlightEditorPanel.IsLatitudeInputReadOnly();
            if (actualDeviceLocation)
            {
                VerifyEqual("9. Verify Longitude field is editable", false, isLongitudeReadOnly);
                VerifyEqual("9. Verify Latitude field is editable", false, isLatitudeReadOnly);
            }
            else
            {
                VerifyEqual("9. Verify Longitude field is read-only", true, isLongitudeReadOnly);
                VerifyEqual("9. Verify Latitude field is read-only", true, isLatitudeReadOnly);
            }
        }

        [Test, DynamicRetry]
        [Description("BO_05_05 Options - Equipment Inventory - Enable geozone parent in geozone editor")]
        public void BO_05_05()
        {
            var testData = GetTestDataOfBO_05_05();
            var xmlGeozone = testData["Geozone"];

            Step("**** Precondition ****");
            Step(" - User must belong to root/admin group");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenBackOfficeApp();
            var backOfficePage = loginPage.LoginAsValidUserToBackOffice(Settings.Users["admin"].Username, Settings.Users["admin"].Password);

            Step("1. Select Equipment Inventory item");
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.EquipmentInventory);

            Step("2. Uncheck Enable geozone parent if it is being checked or check if it's being unchecked");
            var firstParentGeozoneValue = backOfficePage.BackOfficeDetailsPanel.GetEquipmentParentGeozoneValue();
            backOfficePage.BackOfficeDetailsPanel.TickEquipmentParentGeozoneCheckbox(!firstParentGeozoneValue);
            backOfficePage.BackOfficeDetailsPanel.ClickSaveButton();
            backOfficePage.WaitForPreviousActionComplete();

            Step("3. Reload Back Office page then select Equipment Inventory item");
            backOfficePage = Browser.RefreshBackOfficePage();
            backOfficePage.WaitForPreviousActionComplete();
            backOfficePage.BackOfficeOptionsPanel.WaitForPanelLoaded();
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.EquipmentInventory);

            Step("4. Expected Changed value is remained");
            var actualParentGeozone = backOfficePage.BackOfficeDetailsPanel.GetEquipmentParentGeozoneValue();
            VerifyEqual("4. Verify Changed value is remained", !firstParentGeozoneValue, actualParentGeozone);

            Step("5. Go to SLV app");
            var desktopPage = Browser.NavigateToLoggedInCMS();
            desktopPage.InstallAppsIfNotExist(App.EquipmentInventory);

            Step("6. Go to Equipment Inventory page");
            var equipmentInventoryPage = desktopPage.GoToApp(App.EquipmentInventory) as EquipmentInventoryPage;

            Step("7. Select a device");
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(xmlGeozone);

            Step("8. Parent geozone field is editable dropdown list in case the option is checked and otherwise the read-only input");
            var isParentGeozoneDropDownVisible = equipmentInventoryPage.GeozoneEditorPanel.IsParentGeozoneDropDownVisible();
            var isParentGeozoneInputVisible = equipmentInventoryPage.GeozoneEditorPanel.IsParentGeozoneInputVisible();
            if (actualParentGeozone)
            {
                var isParentGeozoneDropDownReadOnly = equipmentInventoryPage.GeozoneEditorPanel.IsParentGeozoneDropDownReadOnly();
                VerifyEqual("8. Verify Geozone field is editable dropdown list", true, isParentGeozoneDropDownVisible == true && isParentGeozoneInputVisible == false && isParentGeozoneDropDownReadOnly == false);
            }
            else
            {
                var isParentGeozoneInputReadOnly = equipmentInventoryPage.GeozoneEditorPanel.IsParentGeozoneInputReadOnly();
                VerifyEqual("8. Verify Geozone field is read-only input", true, isParentGeozoneDropDownVisible == false && isParentGeozoneInputVisible == true && isParentGeozoneInputReadOnly == true);
            }

            Step("9. Repeat the test with Enable geozone parent option is the other way round with step #2");
            backOfficePage = Browser.NavigateToLoggedInBackOfficeApp();
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.EquipmentInventory);
            backOfficePage.BackOfficeDetailsPanel.TickEquipmentParentGeozoneCheckbox(firstParentGeozoneValue);
            backOfficePage.BackOfficeDetailsPanel.ClickSaveButton();
            backOfficePage.WaitForPreviousActionComplete();

            backOfficePage = Browser.RefreshBackOfficePage();
            backOfficePage.WaitForPreviousActionComplete();
            backOfficePage.BackOfficeOptionsPanel.WaitForPanelLoaded();
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.EquipmentInventory);
            actualParentGeozone = backOfficePage.BackOfficeDetailsPanel.GetEquipmentParentGeozoneValue();
            VerifyEqual("9. Verify Changed value is remained", firstParentGeozoneValue, actualParentGeozone);

            desktopPage = Browser.NavigateToLoggedInCMS();
            equipmentInventoryPage = desktopPage.GoToApp(App.EquipmentInventory) as EquipmentInventoryPage;
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(xmlGeozone);
            isParentGeozoneDropDownVisible = equipmentInventoryPage.GeozoneEditorPanel.IsParentGeozoneDropDownVisible();
            isParentGeozoneInputVisible = equipmentInventoryPage.GeozoneEditorPanel.IsParentGeozoneInputVisible();
            if (actualParentGeozone)
            {
                var isParentGeozoneDropDownReadOnly = equipmentInventoryPage.GeozoneEditorPanel.IsParentGeozoneDropDownReadOnly();
                VerifyEqual("9. Verify Geozone field is editable dropdown list", true, isParentGeozoneDropDownVisible == true && isParentGeozoneInputVisible == false && isParentGeozoneDropDownReadOnly == false);
            }
            else
            {
                var isParentGeozoneInputReadOnly = equipmentInventoryPage.GeozoneEditorPanel.IsParentGeozoneInputReadOnly();
                VerifyEqual("9. Verify Geozone field is read-only input", true, isParentGeozoneDropDownVisible == false && isParentGeozoneInputVisible == true && isParentGeozoneInputReadOnly == true);
            }
        }

        [Test, DynamicRetry]
        [Description("BO_05_06 Options - Equipment Inventory - Event time visible")]
        public void BO_05_06()
        {
            var testData = GetTestDataOfBO_05_06();
            var xmlGeozone = testData["Geozone"];

            Step("**** Precondition ****");
            Step(" - User must belong to root/admin group");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenBackOfficeApp();
            var backOfficePage = loginPage.LoginAsValidUserToBackOffice(Settings.Users["admin"].Username, Settings.Users["admin"].Password);

            Step("1. Select Equipment Inventory item");
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.EquipmentInventory);

            Step("2. Uncheck Event time visible if it is being checked or check if it's being unchecked then save");
            var firstEventTimeVisible = backOfficePage.BackOfficeDetailsPanel.GetEquipmentEventTimeVisibleValue();
            backOfficePage.BackOfficeDetailsPanel.TickEquipmentEventTimeVisibleCheckbox(!firstEventTimeVisible);
            backOfficePage.BackOfficeDetailsPanel.ClickSaveButton();
            backOfficePage.WaitForPreviousActionComplete();

            Step("3. Reload Back Office page then select Equipment Inventory item");
            backOfficePage = Browser.RefreshBackOfficePage();
            backOfficePage.WaitForPreviousActionComplete();
            backOfficePage.BackOfficeOptionsPanel.WaitForPanelLoaded();
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.EquipmentInventory);

            Step("4. Expected Changed value is remained");
            var actualEventTimeVisible = backOfficePage.BackOfficeDetailsPanel.GetEquipmentEventTimeVisibleValue();
            VerifyEqual("4. Verify Changed value is remained", !firstEventTimeVisible, actualEventTimeVisible);

            Step("5. Go to SLV app");
            var desktopPage = Browser.NavigateToLoggedInCMS();
            desktopPage.InstallAppsIfNotExist(App.EquipmentInventory);

            Step("6. Go to Equipment Inventory page");
            var equipmentInventoryPage = desktopPage.GoToApp(App.EquipmentInventory) as EquipmentInventoryPage;

            Step("7. Select a geozone and click Custom report at the bottom of Geozone Details panel");
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(xmlGeozone);
            equipmentInventoryPage.WaitForEditorPanelDisplayed();
            equipmentInventoryPage.GeozoneEditorPanel.ClickCustomReportButton();
            equipmentInventoryPage.WaitForPreviousActionComplete();
            equipmentInventoryPage.GridPanel.WaitForGridContentAvailable();

            Step("8. Expected Custom report grid panel is displayed:");
            Step(" - Timestamp checkbox in grid toolbar is checked in case the option is checked and otherwise");
            Step(" - In case the option is checked, each column in grid (except Line # and Device columns) displays 2 rows: the above row is attribute name, the below row is 'Value' and 'Timestamp'; otherwise, in case the option is unchecked, each column displays its attribute name only");
            var isTimeStampButtonChecked = equipmentInventoryPage.GridPanel.IsTimeStampButtonChecked();
            var isColumnHeadersHasValueAndTimestamp = equipmentInventoryPage.GridPanel.IsColumnHeadersHasValueAndTimestamp();
            if (actualEventTimeVisible)
            {
                VerifyEqual("8. Verify Timestamp checkbox in grid toolbar is checked and each column in grid (except Line # and Device columns) displays 2 rows: the above row is attribute name, the below row is 'Value' and 'Timestamp'", true, isTimeStampButtonChecked && isColumnHeadersHasValueAndTimestamp);
            }
            else
            {
                VerifyEqual("8. Verify Timestamp checkbox in grid toolbar is unchecked and each column displays its attribute name only (not have 'Value' and 'Timestamp')", true, isTimeStampButtonChecked == false && isColumnHeadersHasValueAndTimestamp == false);
            }

            Step("9. Repeat the test with Event time visible option is the other way round with step #2");
            backOfficePage = Browser.NavigateToLoggedInBackOfficeApp();
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.EquipmentInventory);
            backOfficePage.BackOfficeDetailsPanel.TickEquipmentEventTimeVisibleCheckbox(firstEventTimeVisible);
            backOfficePage.BackOfficeDetailsPanel.ClickSaveButton();
            backOfficePage.WaitForPreviousActionComplete();

            backOfficePage = Browser.RefreshBackOfficePage();
            backOfficePage.WaitForPreviousActionComplete();
            backOfficePage.BackOfficeOptionsPanel.WaitForPanelLoaded();
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.EquipmentInventory);
            actualEventTimeVisible = backOfficePage.BackOfficeDetailsPanel.GetEquipmentEventTimeVisibleValue();
            VerifyEqual("9. Verify Changed value is remained", firstEventTimeVisible, actualEventTimeVisible);

            desktopPage = Browser.NavigateToLoggedInCMS();
            equipmentInventoryPage = desktopPage.GoToApp(App.EquipmentInventory) as EquipmentInventoryPage;
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(xmlGeozone);
            equipmentInventoryPage.WaitForEditorPanelDisplayed();
            equipmentInventoryPage.GeozoneEditorPanel.ClickCustomReportButton();
            equipmentInventoryPage.WaitForPreviousActionComplete();
            equipmentInventoryPage.GridPanel.WaitForGridContentAvailable();

            isTimeStampButtonChecked = equipmentInventoryPage.GridPanel.IsTimeStampButtonChecked();
            isColumnHeadersHasValueAndTimestamp = equipmentInventoryPage.GridPanel.IsColumnHeadersHasValueAndTimestamp();
            if (actualEventTimeVisible)
            {
                VerifyEqual("9. Verify Timestamp checkbox in grid toolbar is checked and each column in grid (except Line # and Device columns) displays 2 rows: the above row is attribute name, the below row is 'Value' and 'Timestamp'", true, isTimeStampButtonChecked && isColumnHeadersHasValueAndTimestamp);
            }
            else
            {
                VerifyEqual("9. Verify Timestamp checkbox in grid toolbar is unchecked and each column displays its attribute name only (not have 'Value' and 'Timestamp')", true, isTimeStampButtonChecked == false && isColumnHeadersHasValueAndTimestamp == false);
            }
        }

        [Test, DynamicRetry]
        [Description("BO_05_07 Options - Equipment Inventory - Rows count per page")]
        public void BO_05_07()
        {
            var testData = GetTestDataOfBO_05_07();
            var xmlRowsPerPage = testData["RowsPerPage"];

            Step("**** Precondition ****");
            Step(" - User must belong to root/admin group");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenBackOfficeApp();
            var backOfficePage = loginPage.LoginAsValidUserToBackOffice(Settings.Users["admin"].Username, Settings.Users["admin"].Password);

            Step("1. Select Equipment Inventory item");
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.EquipmentInventory);

            Step("2. Change Rows count per page then save");
            var currentRowsPerPage = backOfficePage.BackOfficeDetailsPanel.GetEquipmentRowsPerPageValue();
            backOfficePage.BackOfficeDetailsPanel.TickEquipmentEventTimeVisibleCheckbox(false);
            backOfficePage.BackOfficeDetailsPanel.EnterEquipmentRowsPerPageNumericInput(xmlRowsPerPage);
            backOfficePage.BackOfficeDetailsPanel.ClickSaveButton();
            backOfficePage.WaitForPreviousActionComplete();

            Step("3. Reload Back Office page then select Equipment Inventory item");
            backOfficePage = Browser.RefreshBackOfficePage();
            backOfficePage.WaitForPreviousActionComplete();
            backOfficePage.BackOfficeOptionsPanel.WaitForPanelLoaded();
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.EquipmentInventory);

            Step("4. Expected Changed value is remained");
            var actualRowsPerPage = backOfficePage.BackOfficeDetailsPanel.GetEquipmentRowsPerPageValue();
            VerifyEqual("4. Verify Changed value is remained", xmlRowsPerPage, actualRowsPerPage);

            Step("5. Go to SLV app");
            var desktopPage = Browser.NavigateToLoggedInCMS();
            desktopPage.InstallAppsIfNotExist(App.EquipmentInventory);

            Step("6. Go to Equipment Inventory page");
            var equipmentInventoryPage = desktopPage.GoToApp(App.EquipmentInventory) as EquipmentInventoryPage;

            Step("7. Select the root geozone and click Custom report at the bottom of Geozone Details panel");
            equipmentInventoryPage.WaitForEditorPanelDisplayed();
            var geozoneDevicesCount = equipmentInventoryPage.GeozoneTreeMainPanel.GetSelectedNodeDevicesCount();

            equipmentInventoryPage.GeozoneEditorPanel.ClickCustomReportButton();
            equipmentInventoryPage.WaitForPreviousActionComplete();
            equipmentInventoryPage.GridPanel.WaitForGridContentAvailable();

            Step("8. Expected Custom report grid panel is displayed: number of devices each page (except the last page) equals the value set at step #2. Number of devices of the last page should be less than or equal the value set at step #2");
            var rowsPerPage = int.Parse(xmlRowsPerPage);

            if (geozoneDevicesCount >= rowsPerPage)
            {
                var tblGrid = equipmentInventoryPage.GridPanel.BuildDataTableFromGrid();
                VerifyEqual(string.Format("8. Verify number of devices each page 1 is '{0}'", xmlRowsPerPage), true, tblGrid.Rows.Count == rowsPerPage);

                var page = geozoneDevicesCount / rowsPerPage;
                for (int j = 0; j < page - 1; j++)
                {
                    equipmentInventoryPage.GridPanel.ClickFooterPageNextButton();
                    equipmentInventoryPage.GridPanel.WaitForLeftFooterTextDisplayed();

                    tblGrid = equipmentInventoryPage.GridPanel.BuildDataTableFromGrid();
                    VerifyEqual(string.Format("8. Verify number of devices each page {0} is {1}'", j + 2, xmlRowsPerPage), true, tblGrid.Rows.Count == rowsPerPage);
                }
            }
            else
            {
                Warning(string.Format("8. Root geozone has devices count {0} < rows per page {1}", geozoneDevicesCount, xmlRowsPerPage));
            }

            try
            {
                Info(string.Format("Reset Rows per Page to {0}", currentRowsPerPage));
                backOfficePage = Browser.NavigateToLoggedInBackOfficeApp();
                backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.EquipmentInventory);
                backOfficePage.BackOfficeDetailsPanel.EnterEquipmentRowsPerPageNumericInput(currentRowsPerPage);
                backOfficePage.BackOfficeDetailsPanel.ClickSaveButton();
                backOfficePage.WaitForPreviousActionComplete();
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("BO_05_08 Options - Equipment Inventory - Toolbar")]
        public void BO_05_08()
        {
            Step("**** Precondition ****");
            Step(" - User must belong to root/admin group");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenBackOfficeApp();
            var backOfficePage = loginPage.LoginAsValidUserToBackOffice(Settings.Users["admin"].Username, Settings.Users["admin"].Password);

            Step("1. Select Equipment Inventory item");
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.EquipmentInventory);

            Step("2. Check all toolbar items then save");
            var currentCheckedToolbarItemsList = backOfficePage.BackOfficeDetailsPanel.GetEquipmentAvailableToolbarItemsNameList();
            backOfficePage.BackOfficeDetailsPanel.CheckEquipmentAllToolbarItems();
            backOfficePage.BackOfficeDetailsPanel.ClickSaveButton();
            backOfficePage.WaitForPreviousActionComplete();
            var allCheckedToolbarItemsList = backOfficePage.BackOfficeDetailsPanel.GetEquipmentAvailableToolbarItemsNameList();

            Step("3. Reload Back Office page then select Equipment Inventory item");
            backOfficePage = Browser.RefreshBackOfficePage();
            backOfficePage.WaitForPreviousActionComplete();
            backOfficePage.BackOfficeOptionsPanel.WaitForPanelLoaded();
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.EquipmentInventory);

            Step("4. Expected Changed value is remained");
            var actualCheckedToolbarItems = backOfficePage.BackOfficeDetailsPanel.GetEquipmentAvailableToolbarItemsNameList();
            VerifyEqual("4. Verify Changed value is remained", allCheckedToolbarItemsList, actualCheckedToolbarItems, false);

            Step("5. Go to SLV app");
            var desktopPage = Browser.NavigateToLoggedInCMS();
            desktopPage.InstallAppsIfNotExist(App.EquipmentInventory);

            Step("6. Go to Equipment Inventory page");
            var equipmentInventoryPage = desktopPage.GoToApp(App.EquipmentInventory) as EquipmentInventoryPage;

            Step("7. Select the root geozone and click Custom report at the bottom of Geozone Details panel");
            equipmentInventoryPage.WaitForEditorPanelDisplayed();
            equipmentInventoryPage.GeozoneEditorPanel.ClickCustomReportButton();
            equipmentInventoryPage.WaitForPreviousActionComplete();
            equipmentInventoryPage.GridPanel.WaitForGridContentAvailable();

            Step("8. Expected Verify all checked toolbar buttons present in the grid toolbar");
            var availableToolbarItems = equipmentInventoryPage.GridPanel.GetBackOfficeAvailableToolbarButtons();
            VerifyEqual("8. Verify all checked toolbar buttons present in the grid toolbar", allCheckedToolbarItemsList, availableToolbarItems, false);

            Step("9. Repeat the test in case all items are unchecked and some items are un/checked randomly. Verify checked items are displayed and unchecked ones are not displayed in grid toolbar");
            backOfficePage = Browser.NavigateToLoggedInBackOfficeApp();
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.EquipmentInventory);
            backOfficePage.BackOfficeDetailsPanel.UncheckEquipmentRandomToolbarItems(2);
            backOfficePage.BackOfficeDetailsPanel.ClickSaveButton();
            backOfficePage.WaitForPreviousActionComplete();
            allCheckedToolbarItemsList = backOfficePage.BackOfficeDetailsPanel.GetEquipmentAvailableToolbarItemsNameList();

            backOfficePage = Browser.RefreshBackOfficePage();
            backOfficePage.WaitForPreviousActionComplete();
            backOfficePage.BackOfficeOptionsPanel.WaitForPanelLoaded();
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.EquipmentInventory);
            actualCheckedToolbarItems = backOfficePage.BackOfficeDetailsPanel.GetEquipmentAvailableToolbarItemsNameList();
            VerifyEqual("9. Verify Changed value is remained", allCheckedToolbarItemsList, actualCheckedToolbarItems, false);

            desktopPage = Browser.NavigateToLoggedInCMS();
            equipmentInventoryPage = desktopPage.GoToApp(App.EquipmentInventory) as EquipmentInventoryPage;
            equipmentInventoryPage.WaitForPreviousActionComplete();
            equipmentInventoryPage.WaitForEditorPanelDisplayed();
            equipmentInventoryPage.GeozoneEditorPanel.ClickCustomReportButton();
            equipmentInventoryPage.WaitForPreviousActionComplete();
            equipmentInventoryPage.GridPanel.WaitForGridContentAvailable();

            availableToolbarItems = equipmentInventoryPage.GridPanel.GetBackOfficeAvailableToolbarButtons();
            VerifyEqual("9. Verify all checked toolbar buttons present in the grid toolbar", allCheckedToolbarItemsList, availableToolbarItems, false);

            try
            {
                Info(string.Format("Reset Toolbar Items to {0}", string.Join(",", currentCheckedToolbarItemsList)));
                backOfficePage = Browser.NavigateToLoggedInBackOfficeApp();
                backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.EquipmentInventory);
                backOfficePage.BackOfficeDetailsPanel.UncheckEquipmentAllToolbarItems();
                backOfficePage.BackOfficeDetailsPanel.CheckEquipmentToolbarItems(currentCheckedToolbarItemsList.ToArray());
                backOfficePage.BackOfficeDetailsPanel.ClickSaveButton();
                backOfficePage.WaitForPreviousActionComplete();
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("BO_05_09 Options - Equipment Inventory - Search attributes")]
        public void BO_05_09()
        {
            Step("**** Precondition ****");
            Step(" - User must belong to root/admin group");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenBackOfficeApp();
            var backOfficePage = loginPage.LoginAsValidUserToBackOffice(Settings.Users["admin"].Username, Settings.Users["admin"].Password);

            Step("1. Select Equipment Inventory item");
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.EquipmentInventory);
            var expectedAttributes = backOfficePage.GetAttributesKey();

            Step("2. Randomly remove one of following attributes then save");
            Step(" - name (Name)");
            Step(" - idOnController (Identifier)");
            Step(" - controllerStrId (Controller ID)");
            Step(" - categoryStrId (Category)");
            Step(" - MacAddress (Unique address)");
            Step(" - DimmingGroupName (Dimming group)");
            Step(" - installStatus (Install status)");
            Step(" - ConfigStatus (Configuration status)");
            Step(" - address (Address 1)");
            Step(" - location.streetdescription (Address 2)");
            Step(" - location.city (City)");
            Step(" - location.zipcode (Zip code)");
            Step(" - client.name (Customer name)");
            Step(" - client.number (Customer number)");
            Step(" - luminaire.model (Luminaire model)");
            Step(" - SoftwareVersion (Software version)");
            Step(" - comment (Comment)");
            var currentAttributes = backOfficePage.BackOfficeDetailsPanel.GetEquipmentSearchAttributesNameList();            
            expectedAttributes.Sort();
            currentAttributes.Sort();
            VerifyEqual("2. Verify Attributes are matched as expected", expectedAttributes, currentAttributes, false);

            var randomAttributes = expectedAttributes.PickRandom(2);
            var remainingAttributes = expectedAttributes.Clone().ToList();
            remainingAttributes.RemoveAll(p => randomAttributes.Contains(p));
            backOfficePage.BackOfficeDetailsPanel.RemoveEquipmentAttributes(randomAttributes.ToArray());
            backOfficePage.BackOfficeDetailsPanel.ClickSaveButton();
            backOfficePage.WaitForPreviousActionComplete();
            var expectedAttribuesName = backOfficePage.GetAttributesName(remainingAttributes.ToArray());
            Step(" --> Removed attributes: {0}", string.Join(", ", randomAttributes));
            Step(" --> Remaining attributes: {0}", string.Join(", ", remainingAttributes));
            Step(" --> Remaining attributes name: {0}", string.Join(", ", expectedAttribuesName));

            Step("3. Reload Back Office page then select Equipment Inventory item");
            backOfficePage = Browser.RefreshBackOfficePage();
            backOfficePage.WaitForPreviousActionComplete();
            backOfficePage.BackOfficeOptionsPanel.WaitForPanelLoaded();
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.EquipmentInventory);

            Step("4. Expected Removed attributes are no longer in the list, intact are remained");
            var actualAttributes = backOfficePage.BackOfficeDetailsPanel.GetEquipmentSearchAttributesNameList();
            Step(" --> Actual attributes: {0}", string.Join(", ", actualAttributes));
            VerifyTrue("4. Verify Removed attributes are no longer in the list", !actualAttributes.Exists(p => randomAttributes.Contains(p)), string.Join(", ", randomAttributes), string.Join(", ", actualAttributes));            
            remainingAttributes.Sort();
            actualAttributes.Sort();
            VerifyEqual("4. Verify Attribues intact are remained", remainingAttributes, actualAttributes, false);

            Step("5. Go to SLV app");
            var desktopPage = Browser.NavigateToLoggedInCMS();
            desktopPage.InstallAppsIfNotExist(App.EquipmentInventory);

            Step("6. Go to Equipment Inventory");
            Step("7. Expand Search bar at the bottom");
            Step("8. Expected Verify attribute dropdown contains all attributes in Search attribute list");
            var equipmentInventoryPage = desktopPage.GoToApp(App.EquipmentInventory) as EquipmentInventoryPage;
            equipmentInventoryPage.GeozoneTreeMainPanel.ClickExpandSearchButton();
            var attributesNameDropDown = equipmentInventoryPage.GeozoneTreeMainPanel.GetAllAttributeDropDownItems();
            Step(" --> Dropdown attributes name: {0}", string.Join(", ", attributesNameDropDown));
            VerifyEqual("8. Verify Verify attribute dropdown contains all attributes in Search attribute list", expectedAttribuesName, attributesNameDropDown, false);

            try
            {
                Info("Reset random apps removed");
                backOfficePage = Browser.NavigateToLoggedInBackOfficeApp();
                backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.EquipmentInventory);
                backOfficePage.BackOfficeDetailsPanel.AddEquipmentAttributes(randomAttributes.ToArray());
                backOfficePage.BackOfficeDetailsPanel.ClickSaveButton();
                backOfficePage.WaitForPreviousActionComplete();
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("BO_06_01 Options - Real-time Control - UI")]
        public void BO_06_01()
        {
            Step("**** Precondition ****");
            Step(" - User must belong to root/admin group");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenBackOfficeApp();
            var backOfficePage = loginPage.LoginAsValidUserToBackOffice(Settings.Users["admin"].Username, Settings.Users["admin"].Password);
            Step("1. Select Real-time Control item");
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.RealTimeControl);

            Step("2. Expected UI as defined");
            VerifyEqual("2. Verify Treeview configuration", "Treeview configuration", backOfficePage.BackOfficeDetailsPanel.GetRealtimeTreeviewConfigurationText());
            VerifyEqual("2. Verify Display devices", "Display devices", backOfficePage.BackOfficeDetailsPanel.GetRealtimeDisplayDevicesText());
            VerifyEqual("2. Verify Display devices checkbox enable", true, backOfficePage.BackOfficeDetailsPanel.IsRealtimeDisplayDevicesCheckboxEditable());
            VerifyEqual("2. Verify Display devices description", "Defines if devices are displayed in geozone tree view or not.", backOfficePage.BackOfficeDetailsPanel.GetRealtimeDisplayDevicesDescText());
            VerifyEqual("2. Verify Enable map filter", "Enable map filter", backOfficePage.BackOfficeDetailsPanel.GetRealtimeMapFilterText());
            VerifyEqual("2. Verify Enable map filter checkbox enable", true, backOfficePage.BackOfficeDetailsPanel.IsRealtimeMapFilterCheckboxEditable());
            VerifyEqual("2. Verify Enable map filter description", "Defines if the filter on devices found in the search is allowed on the map.", backOfficePage.BackOfficeDetailsPanel.GetRealtimeMapFilterDescText());
        }

        [Test, DynamicRetry]
        [Description("BO_06_02 Options - Real-time Control - Display devices in geozone tree")]
        public void BO_06_02()
        {
            var testData = GetTestDataOfBO_06_02();
            var xmlGeozone = testData["Geozone"];

            Step("**** Precondition ****");
            Step(" - User must belong to root/admin group");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenBackOfficeApp();
            var backOfficePage = loginPage.LoginAsValidUserToBackOffice(Settings.Users["admin"].Username, Settings.Users["admin"].Password);

            Step("1. Select Real-time Control item");
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.RealTimeControl);

            Step("2. Uncheck Display devices option if it is being checked or check if it's being unchecked then save");
            var firstDisplayDevicesValue = backOfficePage.BackOfficeDetailsPanel.GetRealtimeDisplayDevicesValue();
            backOfficePage.BackOfficeDetailsPanel.TickRealtimeDisplayDevicesCheckbox(!firstDisplayDevicesValue);
            backOfficePage.BackOfficeDetailsPanel.ClickSaveButton();
            backOfficePage.WaitForPreviousActionComplete();

            Step("3. Reload Back Office page then select Real-time Control item");
            backOfficePage = Browser.RefreshBackOfficePage();
            backOfficePage.WaitForPreviousActionComplete();
            backOfficePage.BackOfficeOptionsPanel.WaitForPanelLoaded();
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.RealTimeControl);

            Step("4. Expected Changed value is remained");
            var actualDisplayDevices = backOfficePage.BackOfficeDetailsPanel.GetRealtimeDisplayDevicesValue();
            VerifyEqual("4. Verify Changed value is remained", !firstDisplayDevicesValue, actualDisplayDevices);

            Step("5. Go to SLV app");
            var desktopPage = Browser.NavigateToLoggedInCMS();
            desktopPage.InstallAppsIfNotExist(App.RealTimeControl);

            Step("6. Go to Real-time Control page");
            var realtimeControlPage = desktopPage.GoToApp(App.RealTimeControl) as RealTimeControlPage;
            realtimeControlPage.GeozoneTreeMainPanel.SelectNode(xmlGeozone);

            Step("7. Expected");
            Step(" - Geozone tree displays devices in case the option is checked. Otherwise, it does NOT display devices in case the option is unchecked");
            Step(" - Filters toolbar button is displayed in case the option is checked and otherwise");
            if (actualDisplayDevices)
            {
                VerifyEqual("[SLV-3231] 7. Verify Geozone tree displays devices", true, realtimeControlPage.GeozoneTreeMainPanel.GetChildDeviceNamesOfSelectedNode().Count > 0);
                VerifyEqual("[SLV-3231] 7. Verify Filters toolbar button is displayed", true, realtimeControlPage.GeozoneTreeMainPanel.IsFilterButtonVisible());
            }
            else
            {
                VerifyEqual("[SLV-3231] 7. Verify Geozone tree does NOT display devices", false, realtimeControlPage.GeozoneTreeMainPanel.GetChildDeviceNamesOfSelectedNode().Count > 0);
                VerifyEqual("[SLV-3231] 7. Verify Filters toolbar button is NOT displayed", false, realtimeControlPage.GeozoneTreeMainPanel.IsFilterButtonVisible());
            }

            Step("8. Repeat the test with Display devices option is the other way round with step #2");
            backOfficePage = Browser.NavigateToLoggedInBackOfficeApp();
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.RealTimeControl);
            backOfficePage.BackOfficeDetailsPanel.TickRealtimeDisplayDevicesCheckbox(firstDisplayDevicesValue);
            backOfficePage.BackOfficeDetailsPanel.ClickSaveButton();
            backOfficePage.WaitForPreviousActionComplete();

            backOfficePage = Browser.RefreshBackOfficePage();
            backOfficePage.WaitForPreviousActionComplete();
            backOfficePage.BackOfficeOptionsPanel.WaitForPanelLoaded();
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.RealTimeControl);
            actualDisplayDevices = backOfficePage.BackOfficeDetailsPanel.GetRealtimeDisplayDevicesValue();
            VerifyEqual("8. Verify Changed value is remained", firstDisplayDevicesValue, actualDisplayDevices);

            desktopPage = Browser.NavigateToLoggedInCMS();
            realtimeControlPage = desktopPage.GoToApp(App.RealTimeControl) as RealTimeControlPage;
            realtimeControlPage.GeozoneTreeMainPanel.SelectNode(xmlGeozone);
            if (actualDisplayDevices)
            {
                VerifyEqual("[SLV-3231] 8. Verify Geozone tree displays devices", true, realtimeControlPage.GeozoneTreeMainPanel.GetChildDeviceNamesOfSelectedNode().Count > 0);
                VerifyEqual("[SLV-3231] 8. Verify Filters toolbar button is displayed", true, realtimeControlPage.GeozoneTreeMainPanel.IsFilterButtonVisible());
            }
            else
            {
                VerifyEqual("[SLV-3231] 8. Verify Geozone tree does NOT display devices", false, realtimeControlPage.GeozoneTreeMainPanel.GetChildDeviceNamesOfSelectedNode().Count > 0);
                VerifyEqual("[SLV-3231] 8. Verify Filters toolbar button is NOT displayed", false, realtimeControlPage.GeozoneTreeMainPanel.IsFilterButtonVisible());
            }
        }

        [Test, DynamicRetry]
        [Description("BO_06_03 Options - Real-time Control - Enable map filter in geozone")]
        public void BO_06_03()
        {
            var searchName = "Telematics";

            Step("**** Precondition ****");
            Step(" - User must belong to root/admin group");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenBackOfficeApp();
            var backOfficePage = loginPage.LoginAsValidUserToBackOffice(Settings.Users["admin"].Username, Settings.Users["admin"].Password);

            Step("1. Select Real-time Control item");
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.RealTimeControl);

            Step("2. Uncheck Enable map option if it is being checked or check if it's being unchecked");
            var firstMapFilterValue = backOfficePage.BackOfficeDetailsPanel.GetRealtimeMapFilterValue();
            backOfficePage.BackOfficeDetailsPanel.TickRealtimeMapFilterCheckbox(!firstMapFilterValue);

            Step("3. Check Display devices option then save");
            backOfficePage.BackOfficeDetailsPanel.TickRealtimeDisplayDevicesCheckbox(true);
            backOfficePage.BackOfficeDetailsPanel.ClickSaveButton();
            backOfficePage.WaitForPreviousActionComplete();

            Step("4. Reload Back Office page then select Real-time Control item");
            backOfficePage = Browser.RefreshBackOfficePage();
            backOfficePage.WaitForPreviousActionComplete();
            backOfficePage.BackOfficeOptionsPanel.WaitForPanelLoaded();
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.RealTimeControl);

            Step("5. Expected Changed value is remained");
            var actualMapFilter = backOfficePage.BackOfficeDetailsPanel.GetRealtimeMapFilterValue();
            VerifyEqual("5. Verify Changed value is remained", !firstMapFilterValue, actualMapFilter);

            Step("6. Go to SLV app");
            var desktopPage = Browser.NavigateToLoggedInCMS();
            desktopPage.InstallAppsIfNotExist(App.RealTimeControl);

            Step("7. Go to Real-time Control page");
            var realtimeControlPage = desktopPage.GoToApp(App.RealTimeControl) as RealTimeControlPage;
            realtimeControlPage.GeozoneTreeMainPanel.ChangeSearchAttribute("Name", "Contains");
            realtimeControlPage.GeozoneTreeMainPanel.EnterSearchTextInput(searchName);
            realtimeControlPage.GeozoneTreeMainPanel.ClickSearchButton();
            realtimeControlPage.WaitForPreviousActionComplete();

            Step("8. Expected Search Results panel appears. 'Filter results on the map' toolbar button is displayed in case the option is checked and otherwise");
            VerifyEqual("[SLV-3206] 8. Verify Search Results panel appears. 'Filter results on the map' toolbar button is displayed in case the option is checked and otherwise", !firstMapFilterValue, realtimeControlPage.GeozoneTreeMainPanel.SearchResultsGeozonePanel.IsMapFilterButtonVisible());

            Step("9. Repeat the test with Enable map option is the other way round with step #2");
            backOfficePage = Browser.NavigateToLoggedInBackOfficeApp();
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.RealTimeControl);
            backOfficePage.BackOfficeDetailsPanel.TickRealtimeMapFilterCheckbox(firstMapFilterValue);
            backOfficePage.BackOfficeDetailsPanel.ClickSaveButton();
            backOfficePage.WaitForPreviousActionComplete();

            backOfficePage = Browser.RefreshBackOfficePage();
            backOfficePage.WaitForPreviousActionComplete();
            backOfficePage.BackOfficeOptionsPanel.WaitForPanelLoaded();
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.RealTimeControl);
            actualMapFilter = backOfficePage.BackOfficeDetailsPanel.GetRealtimeMapFilterValue();
            VerifyEqual("9. Verify Changed value is remained", firstMapFilterValue, actualMapFilter);

            desktopPage = Browser.NavigateToLoggedInCMS();
            realtimeControlPage = desktopPage.GoToApp(App.RealTimeControl) as RealTimeControlPage;
            realtimeControlPage.GeozoneTreeMainPanel.ChangeSearchAttribute("Name", "Contains");
            realtimeControlPage.GeozoneTreeMainPanel.EnterSearchTextInput(searchName);
            realtimeControlPage.GeozoneTreeMainPanel.ClickSearchButton();
            realtimeControlPage.WaitForPreviousActionComplete();

            VerifyEqual("[SLV-3674] 9. Verify Search Results panel appears. 'Filter results on the map' toolbar button is displayed in case the option is checked and otherwise", firstMapFilterValue, realtimeControlPage.GeozoneTreeMainPanel.SearchResultsGeozonePanel.IsMapFilterButtonVisible());
        }

        [Test, DynamicRetry]
        [Description("BO_07_01 Options - Data History - UI")]
        public void BO_07_01()
        {
            Step("**** Precondition ****");
            Step(" - User must belong to root/admin group");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenBackOfficeApp();
            var backOfficePage = loginPage.LoginAsValidUserToBackOffice(Settings.Users["admin"].Username, Settings.Users["admin"].Password);
            Step("1. Select Data History item");
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.DataHistory);

            Step("2. Expected UI as defined");
            VerifyEqual("2. Verify Treeview configuration", "Treeview configuration", backOfficePage.BackOfficeDetailsPanel.GetDataHistoryTreeviewConfigurationText());
            VerifyEqual("2. Verify Display devices", "Display devices", backOfficePage.BackOfficeDetailsPanel.GetDataHistoryDisplayDevicesText());
            VerifyEqual("2. Verify Display devices checkbox enable", true, backOfficePage.BackOfficeDetailsPanel.IsDataHistoryDisplayDevicesCheckboxEditable());
            VerifyEqual("2. Verify Display devices description", "Defines if devices are displayed in geozone tree view or not.", backOfficePage.BackOfficeDetailsPanel.GetDataHistoryDisplayDevicesDescText());

            VerifyEqual("2. Verify Report  configuration", "Report configuration", backOfficePage.BackOfficeDetailsPanel.GetDataHistoryReportConfigurationText());
            VerifyEqual("2. Verify Event time visible", "Event time visible", backOfficePage.BackOfficeDetailsPanel.GetDataHistoryEventTimeVisibleText());
            VerifyEqual("2. Verify Event time visible checkbox enable", true, backOfficePage.BackOfficeDetailsPanel.IsDataHistoryEventTimeVisibleCheckboxEditable());
            VerifyEqual("2. Verify Event time visible description", "Defines if event time columns are visibled for each attributs in the report table.", backOfficePage.BackOfficeDetailsPanel.GetDataHistoryEventTimeVisibleDescText());
            VerifyEqual("2. Verify Rows count per page", "Rows count per page", backOfficePage.BackOfficeDetailsPanel.GetDataHistoryRowsPerPageText());
            VerifyEqual("2. Verify Rows count per page input enable", true, backOfficePage.BackOfficeDetailsPanel.IsDataHistoryRowCountPerPageInputEditable());
            VerifyEqual("2. Verify Rows count per page description", "The count of rows loaded and displayed per page in the report table.", backOfficePage.BackOfficeDetailsPanel.GetDataHistoryRowsPerPageDescText());

            VerifyEqual("2. Verify Toolbar list label", "Toolbar", backOfficePage.BackOfficeDetailsPanel.GetDataHistoryToolbarItemsText());
            VerifyEqual("2. Verify Toolbar list", true, backOfficePage.BackOfficeDetailsPanel.GetDataHistoryToolbarItemsNameList().Count > 0);
            VerifyEqual("2. Verify Toolbar list description", "The items of toolbar availables from report grid.", backOfficePage.BackOfficeDetailsPanel.GetDataHistoryToolbarItemsDescText());
        }

        [Test, DynamicRetry]
        [Description("BO_07_02 Options - Data History - Display devices in geozone tree")]
        public void BO_07_02()
        {
            var testData = GetTestDataOfBO_07_02();
            var xmlGeozone = testData["Geozone"];

            Step("**** Precondition ****");
            Step(" - User must belong to root/admin group");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenBackOfficeApp();
            var backOfficePage = loginPage.LoginAsValidUserToBackOffice(Settings.Users["admin"].Username, Settings.Users["admin"].Password);

            Step("1. Select Data History item");
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.DataHistory);

            Step("2. Uncheck Display devices option if it is being checked or check if it's being unchecked then save");
            var firstDisplayDevicesValue = backOfficePage.BackOfficeDetailsPanel.GetDataHistoryDisplayDevicesValue();
            backOfficePage.BackOfficeDetailsPanel.TickDataHistoryDisplayDevicesCheckbox(!firstDisplayDevicesValue);
            backOfficePage.BackOfficeDetailsPanel.ClickSaveButton();
            backOfficePage.WaitForPreviousActionComplete();

            Step("3. Reload Back Office page then select Data History item");
            backOfficePage = Browser.RefreshBackOfficePage();
            backOfficePage.WaitForPreviousActionComplete();
            backOfficePage.BackOfficeOptionsPanel.WaitForPanelLoaded();
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.DataHistory);

            Step("4. Expected Changed value is remained");
            var actualDisplayDevices = backOfficePage.BackOfficeDetailsPanel.GetDataHistoryDisplayDevicesValue();
            VerifyEqual("4. Verify Changed value is remained", !firstDisplayDevicesValue, actualDisplayDevices);

            Step("5. Go to SLV app");
            var desktopPage = Browser.NavigateToLoggedInCMS();
            desktopPage.InstallAppsIfNotExist(App.DataHistory);

            Step("6. Go to Data History page");
            var dataHistoryPage = desktopPage.GoToApp(App.DataHistory) as DataHistoryPage;
            dataHistoryPage.GeozoneTreeMainPanel.SelectNode(xmlGeozone);

            Step("7. Expected");
            Step(" - Geozone tree displays devices in case the option is checked. Otherwise, it does NOT display devices in case the option is unchecked");
            Step(" - Filters toolbar button is displayed in case the option is checked and otherwise");
            if (actualDisplayDevices)
            {
                VerifyEqual("7. Verify Geozone tree displays devices", true, dataHistoryPage.GeozoneTreeMainPanel.GetChildDeviceNamesOfSelectedNode().Count > 0);
            }
            else
            {
                VerifyEqual("7. Verify Geozone tree does NOT display devices", false, dataHistoryPage.GeozoneTreeMainPanel.GetChildDeviceNamesOfSelectedNode().Count > 0);
            }

            Step("8. Repeat the test with Display devices option is the other way round with step #2");
            backOfficePage = Browser.NavigateToLoggedInBackOfficeApp();
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.DataHistory);
            backOfficePage.BackOfficeDetailsPanel.TickDataHistoryDisplayDevicesCheckbox(firstDisplayDevicesValue);
            backOfficePage.BackOfficeDetailsPanel.ClickSaveButton();
            backOfficePage.WaitForPreviousActionComplete();

            backOfficePage = Browser.RefreshBackOfficePage();
            backOfficePage.WaitForPreviousActionComplete();
            backOfficePage.BackOfficeOptionsPanel.WaitForPanelLoaded();
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.DataHistory);
            actualDisplayDevices = backOfficePage.BackOfficeDetailsPanel.GetDataHistoryDisplayDevicesValue();
            VerifyEqual("8. Verify Changed value is remained", firstDisplayDevicesValue, actualDisplayDevices);

            desktopPage = Browser.NavigateToLoggedInCMS();
            dataHistoryPage = desktopPage.GoToApp(App.DataHistory) as DataHistoryPage;
            dataHistoryPage.GeozoneTreeMainPanel.SelectNode(xmlGeozone);
            if (actualDisplayDevices)
            {
                VerifyEqual("8. Verify Geozone tree displays devices", true, dataHistoryPage.GeozoneTreeMainPanel.GetChildDeviceNamesOfSelectedNode().Count > 0);
            }
            else
            {
                VerifyEqual("8. Verify Geozone tree does NOT display devices", false, dataHistoryPage.GeozoneTreeMainPanel.GetChildDeviceNamesOfSelectedNode().Count > 0);
            }
        }

        [Test, DynamicRetry]
        [Description("BO_07_03 Options - Data History - Event time visible")]
        public void BO_07_03()
        {
            var testData = GetTestDataOfBO_07_03();
            var xmlGeozone = testData["Geozone"];

            Step("**** Precondition ****");
            Step(" - User must belong to root/admin group");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenBackOfficeApp();
            var backOfficePage = loginPage.LoginAsValidUserToBackOffice(Settings.Users["admin"].Username, Settings.Users["admin"].Password);

            Step("1. Select Data History item");
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.DataHistory);

            Step("2. Uncheck Event time visible if it is being checked or check if it's being unchecked then save");
            var firstEventTimeVisible = backOfficePage.BackOfficeDetailsPanel.GetDataHistoryEventTimeVisibleValue();
            backOfficePage.BackOfficeDetailsPanel.TickDataHistoryEventTimeVisibleCheckbox(!firstEventTimeVisible);
            backOfficePage.BackOfficeDetailsPanel.ClickSaveButton();
            backOfficePage.WaitForPreviousActionComplete();

            Step("3. Reload Back Office page then select Data History item");
            backOfficePage = Browser.RefreshBackOfficePage();
            backOfficePage.WaitForPreviousActionComplete();
            backOfficePage.BackOfficeOptionsPanel.WaitForPanelLoaded();
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.DataHistory);

            Step("4. Expected Changed value is remained");
            var actualEventTimeVisible = backOfficePage.BackOfficeDetailsPanel.GetDataHistoryEventTimeVisibleValue();
            VerifyEqual("4. Verify Changed value is remained", !firstEventTimeVisible, actualEventTimeVisible);

            Step("5. Go to SLV app");
            var desktopPage = Browser.NavigateToLoggedInCMS();
            desktopPage.InstallAppsIfNotExist(App.DataHistory);

            Step("6. Go to Data History page");
            var dataHistoryPage = desktopPage.GoToApp(App.DataHistory) as DataHistoryPage;
            dataHistoryPage.GridPanel.WaitForGridContentAvailable();

            Step("7. Expected Custom report grid panel is displayed:");
            Step(" - Timestamp checkbox in grid toolbar is checked in case the option is checked and otherwise");
            Step(" - In case the option is checked, each column in grid (except Line # and Device columns) displays 2 rows: the above row is attribute name, the below row is 'Value' and 'Timestamp'; otherwise, in case the option is unchecked, each column displays its attribute name only");
            var isTimeStampButtonChecked = dataHistoryPage.GridPanel.IsTimeStampButtonChecked();
            var isColumnHeadersHasValueAndTimestamp = dataHistoryPage.GridPanel.IsColumnHeadersHasValueAndTimestamp();
            if (actualEventTimeVisible)
            {
                VerifyEqual("7. Verify Timestamp checkbox in grid toolbar is checked", true, isTimeStampButtonChecked);
                VerifyEqual("7. Verify Each column in grid (except Line # and Device columns) displays 2 rows: the above row is attribute name, the below row is 'Value' and 'Timestamp'", true, isColumnHeadersHasValueAndTimestamp);
            }
            else
            {
                VerifyEqual("7. Verify Timestamp checkbox in grid toolbar is unchecked", false, isTimeStampButtonChecked);
                VerifyEqual("7. Verify Each column displays its attribute name only (not have 'Value' and 'Timestamp')", false, isColumnHeadersHasValueAndTimestamp);
            }

            Step("8. Repeat the test with Event time visible option is the other way round with step #2");
            backOfficePage = Browser.NavigateToLoggedInBackOfficeApp();
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.DataHistory);
            backOfficePage.BackOfficeDetailsPanel.TickDataHistoryEventTimeVisibleCheckbox(firstEventTimeVisible);
            backOfficePage.BackOfficeDetailsPanel.ClickSaveButton();
            backOfficePage.WaitForPreviousActionComplete();

            backOfficePage = Browser.RefreshBackOfficePage();
            backOfficePage.WaitForPreviousActionComplete();
            backOfficePage.BackOfficeOptionsPanel.WaitForPanelLoaded();
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.DataHistory);
            actualEventTimeVisible = backOfficePage.BackOfficeDetailsPanel.GetDataHistoryEventTimeVisibleValue();
            VerifyEqual("8. Verify Changed value is remained", firstEventTimeVisible, actualEventTimeVisible);

            desktopPage = Browser.NavigateToLoggedInCMS();
            dataHistoryPage = desktopPage.GoToApp(App.DataHistory) as DataHistoryPage;
            dataHistoryPage.GridPanel.WaitForGridContentAvailable();

            isTimeStampButtonChecked = dataHistoryPage.GridPanel.IsTimeStampButtonChecked();
            isColumnHeadersHasValueAndTimestamp = dataHistoryPage.GridPanel.IsColumnHeadersHasValueAndTimestamp();
            if (actualEventTimeVisible)
            {
                VerifyEqual("8. Verify Timestamp checkbox in grid toolbar is checked", true, isTimeStampButtonChecked);
                VerifyEqual("8. Verify Each column in grid (except Line # and Device columns) displays 2 rows: the above row is attribute name, the below row is 'Value' and 'Timestamp'", true, isColumnHeadersHasValueAndTimestamp);
            }
            else
            {
                VerifyEqual("8. Verify Timestamp checkbox in grid toolbar is unchecked", false, isTimeStampButtonChecked);
                VerifyEqual("8. Verify Each column displays its attribute name only (not have 'Value' and 'Timestamp')", false, isColumnHeadersHasValueAndTimestamp);
            }
        }

        [Test, DynamicRetry]
        [Description("BO_07_04 Options - Data History - Rows count per page")]
        public void BO_07_04()
        {
            var testData = GetTestDataOfBO_07_04();
            var xmlGeozone = testData["Geozone"];
            var xmlRowsPerPage = testData["RowsPerPage"];

            Step("**** Precondition ****");
            Step(" - User must belong to root/admin group");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenBackOfficeApp();
            var backOfficePage = loginPage.LoginAsValidUserToBackOffice(Settings.Users["admin"].Username, Settings.Users["admin"].Password);

            Step("1. Select Data History item");
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.DataHistory);

            Step("2. Change Rows count per page then save");
            var currentRowsPerPage = backOfficePage.BackOfficeDetailsPanel.GetDataHistoryRowsPerPageValue();
            backOfficePage.BackOfficeDetailsPanel.TickDataHistoryEventTimeVisibleCheckbox(false);
            backOfficePage.BackOfficeDetailsPanel.EnterDataHistoryRowsPerPageNumericInput(xmlRowsPerPage);
            backOfficePage.BackOfficeDetailsPanel.ClickSaveButton();
            backOfficePage.WaitForPreviousActionComplete();

            Step("3. Reload Back Office page then select Data History item");
            backOfficePage = Browser.RefreshBackOfficePage();
            backOfficePage.WaitForPreviousActionComplete();
            backOfficePage.BackOfficeOptionsPanel.WaitForPanelLoaded();
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.DataHistory);

            Step("4. Expected Changed value is remained");
            var actualRowsPerPage = backOfficePage.BackOfficeDetailsPanel.GetDataHistoryRowsPerPageValue();
            VerifyEqual("4. Verify Changed value is remained", xmlRowsPerPage, actualRowsPerPage);

            Step("5. Go to SLV app");
            var desktopPage = Browser.NavigateToLoggedInCMS();
            desktopPage.InstallAppsIfNotExist(App.DataHistory);

            Step("6. Go to Data History page");
            var dataHistoryPage = desktopPage.GoToApp(App.DataHistory) as DataHistoryPage;

            Step("7. Select a geozone");
            dataHistoryPage.GeozoneTreeMainPanel.SelectNode(xmlGeozone);
            var geozoneDevicesCount = dataHistoryPage.GeozoneTreeMainPanel.GetSelectedNodeDevicesCount();
            dataHistoryPage.GridPanel.WaitForGridContentAvailable();

            Step("8. Expected Custom report grid panel is displayed: number of devices each page (except the last page) equals the value set at step #2. Number of devices of the last page should be less than or equal the value set at step #2");
            var rowsPerPage = int.Parse(xmlRowsPerPage);

            if (geozoneDevicesCount >= rowsPerPage)
            {
                var tblGrid = dataHistoryPage.GridPanel.BuildDataTableFromGrid();
                VerifyEqual(string.Format("8. Verify number of devices each page 1 is '{0}'", xmlRowsPerPage), true, tblGrid.Rows.Count == rowsPerPage);

                var page = geozoneDevicesCount / rowsPerPage;
                for (int j = 0; j < page - 1; j++)
                {
                    dataHistoryPage.GridPanel.ClickFooterPageNextButton();
                    dataHistoryPage.GridPanel.WaitForLeftFooterTextDisplayed();

                    tblGrid = dataHistoryPage.GridPanel.BuildDataTableFromGrid();
                    VerifyEqual(string.Format("8. Verify number of devices each page {0} is {1}'", j + 2, xmlRowsPerPage), true, tblGrid.Rows.Count == rowsPerPage);
                }
            }
            else
            {
                Warning(string.Format("8. Verify Geozone has devices count {0} < rows per page {1}", geozoneDevicesCount, xmlRowsPerPage));
            }

            Info(string.Format("Reset Rows per Page to {0}", currentRowsPerPage));
            backOfficePage = Browser.NavigateToLoggedInBackOfficeApp();
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.DataHistory);
            backOfficePage.BackOfficeDetailsPanel.EnterDataHistoryRowsPerPageNumericInput(currentRowsPerPage);
            backOfficePage.BackOfficeDetailsPanel.ClickSaveButton();
            backOfficePage.WaitForPreviousActionComplete();
        }

        [Test, DynamicRetry]
        [Description("BO_07_05 Options - Data History - Toolbar")]
        public void BO_07_05()
        {
            Step("**** Precondition ****");
            Step(" - User must belong to root/admin group");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenBackOfficeApp();
            var backOfficePage = loginPage.LoginAsValidUserToBackOffice(Settings.Users["admin"].Username, Settings.Users["admin"].Password);

            Step("1. Select Data History item");
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.DataHistory);

            Step("2. Check all toolbar items then save");
            var currentCheckedToolbarItemsList = backOfficePage.BackOfficeDetailsPanel.GetDataHistoryAvailableToolbarItemsNameList();
            backOfficePage.BackOfficeDetailsPanel.CheckDataHistoryAllToolbarItems();
            backOfficePage.BackOfficeDetailsPanel.ClickSaveButton();
            backOfficePage.WaitForPreviousActionComplete();
            var allCheckedToolbarItemsList = backOfficePage.BackOfficeDetailsPanel.GetDataHistoryAvailableToolbarItemsNameList();

            Step("3. Reload Back Office page then select Data History item");
            backOfficePage = Browser.RefreshBackOfficePage();
            backOfficePage.WaitForPreviousActionComplete();
            backOfficePage.BackOfficeOptionsPanel.WaitForPanelLoaded();
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.DataHistory);

            Step("4. Expected Changed value is remained");
            var actualCheckedToolbarItems = backOfficePage.BackOfficeDetailsPanel.GetDataHistoryAvailableToolbarItemsNameList();
            VerifyEqual("4. Verify Changed value is remained", allCheckedToolbarItemsList, actualCheckedToolbarItems, false);

            Step("5. Go to SLV app");
            var desktopPage = Browser.NavigateToLoggedInCMS();
            desktopPage.InstallAppsIfNotExist(App.DataHistory);

            Step("6. Go to Data History page");
            Step("7. Select the root geozone");
            var dataHistoryPage = desktopPage.GoToApp(App.DataHistory) as DataHistoryPage;

            Step("8. Expected Verify all checked toolbar buttons present in the grid toolbar");
            var availableToolbarItems = dataHistoryPage.GridPanel.GetBackOfficeAvailableToolbarButtons();
            VerifyEqual("8. Verify all checked toolbar buttons present in the grid toolbar", allCheckedToolbarItemsList, availableToolbarItems, false);

            Step("9. Repeat the test in case all items are unchecked and some items are un/checked randomly. Verify checked items are displayed and unchecked ones are not displayed in grid toolbar");
            backOfficePage = Browser.NavigateToLoggedInBackOfficeApp();
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.DataHistory);
            backOfficePage.BackOfficeDetailsPanel.UncheckDataHistoryRandomToolbarItems(2);
            backOfficePage.BackOfficeDetailsPanel.ClickSaveButton();
            backOfficePage.WaitForPreviousActionComplete();
            allCheckedToolbarItemsList = backOfficePage.BackOfficeDetailsPanel.GetDataHistoryAvailableToolbarItemsNameList();

            backOfficePage = Browser.RefreshBackOfficePage();
            backOfficePage.WaitForPreviousActionComplete();
            backOfficePage.BackOfficeOptionsPanel.WaitForPanelLoaded();
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.DataHistory);
            actualCheckedToolbarItems = backOfficePage.BackOfficeDetailsPanel.GetDataHistoryAvailableToolbarItemsNameList();
            VerifyEqual("9. Verify Changed value is remained", allCheckedToolbarItemsList, actualCheckedToolbarItems, false);

            desktopPage = Browser.NavigateToLoggedInCMS();
            dataHistoryPage = desktopPage.GoToApp(App.DataHistory) as DataHistoryPage;

            availableToolbarItems = dataHistoryPage.GridPanel.GetBackOfficeAvailableToolbarButtons();
            VerifyEqual("9. Verify all checked toolbar buttons present in the grid toolbar", allCheckedToolbarItemsList, availableToolbarItems, false);

            Info(string.Format("Reset Toolbar Items to {0}", string.Join(",", currentCheckedToolbarItemsList)));
            backOfficePage = Browser.NavigateToLoggedInBackOfficeApp();
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.DataHistory);
            backOfficePage.BackOfficeDetailsPanel.UncheckDataHistoryAllToolbarItems();
            backOfficePage.BackOfficeDetailsPanel.CheckDataHistoryToolbarItems(currentCheckedToolbarItemsList.ToArray());
            backOfficePage.BackOfficeDetailsPanel.ClickSaveButton();
            backOfficePage.WaitForPreviousActionComplete();
        }

        [Test, DynamicRetry]
        [Description("BO_08_01 Options - Device History - UI")]
        public void BO_08_01()
        {
            Step("**** Precondition ****");
            Step(" - User must belong to root/admin group");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenBackOfficeApp();
            var backOfficePage = loginPage.LoginAsValidUserToBackOffice(Settings.Users["admin"].Username, Settings.Users["admin"].Password);
            Step("1. Select Device History item");

            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.DeviceHistory);
            Step("2. Expected UI as defined");
            VerifyEqual("2. Verify Treeview configuration", "Treeview configuration", backOfficePage.BackOfficeDetailsPanel.GetDeviceHistoryTreeviewConfigurationText());
            VerifyEqual("2. Verify Display devices", "Display devices", backOfficePage.BackOfficeDetailsPanel.GetDeviceHistoryDisplayDevicesText());
            VerifyEqual("2. Verify Display devices checkbox enable", true, backOfficePage.BackOfficeDetailsPanel.IsDeviceHistoryDisplayDevicesCheckboxEditable());
            VerifyEqual("2. Verify Display devices description", "Defines if devices are displayed in geozone tree view or not.", backOfficePage.BackOfficeDetailsPanel.GetDeviceHistoryDisplayDevicesDescText());

            VerifyEqual("2. Verify Report  configuration", "Report configuration", backOfficePage.BackOfficeDetailsPanel.GetDeviceHistoryReportConfigurationText());
            VerifyEqual("2. Verify Event time visible", "Event time visible", backOfficePage.BackOfficeDetailsPanel.GetDeviceHistoryEventTimeVisibleText());
            VerifyEqual("2. Verify Event time visible checkbox enable", true, backOfficePage.BackOfficeDetailsPanel.IsDeviceHistoryEventTimeVisibleCheckboxEditable());
            VerifyEqual("2. Verify Event time visible description", "Defines if event time columns are visibled for each attributs in the report table.", backOfficePage.BackOfficeDetailsPanel.GetDeviceHistoryEventTimeVisibleDescText());
            VerifyEqual("2. Verify Rows count per page", "Rows count per page", backOfficePage.BackOfficeDetailsPanel.GetDeviceHistoryRowsPerPageText());
            VerifyEqual("2. Verify Rows count per page input enable", true, backOfficePage.BackOfficeDetailsPanel.IsDeviceHistoryRowCountPerPageInputEditable());
            VerifyEqual("2. Verify Rows count per page description", "The count of rows loaded and displayed per page in the report table.", backOfficePage.BackOfficeDetailsPanel.GetDeviceHistoryRowsPerPageDescText());
        }

        [Test, DynamicRetry]
        [Description("BO_08_02 Options - Device History - Display devices in geozone tree")]
        public void BO_08_02()
        {
            var testData = GetTestDataOfBO_08_02();
            var xmlGeozone = testData["Geozone"];

            Step("**** Precondition ****");
            Step(" - User must belong to root/admin group");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenBackOfficeApp();
            var backOfficePage = loginPage.LoginAsValidUserToBackOffice(Settings.Users["admin"].Username, Settings.Users["admin"].Password);

            Step("1. Select Device History item");
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.DeviceHistory);

            Step("2. Uncheck Display devices option if it is being checked or check if it's being unchecked then save");
            var firstDisplayDevicesValue = backOfficePage.BackOfficeDetailsPanel.GetDeviceHistoryDisplayDevicesValue();
            backOfficePage.BackOfficeDetailsPanel.TickDeviceHistoryDisplayDevicesCheckbox(!firstDisplayDevicesValue);
            backOfficePage.BackOfficeDetailsPanel.ClickSaveButton();
            backOfficePage.WaitForPreviousActionComplete();

            Step("3. Reload Back Office page then select Device History item");
            backOfficePage = Browser.RefreshBackOfficePage();
            backOfficePage.WaitForPreviousActionComplete();
            backOfficePage.BackOfficeOptionsPanel.WaitForPanelLoaded();
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.DeviceHistory);

            Step("4. Expected Changed value is remained");
            Step("--> SLV-3687: BackOffice - Unable to save Device History config");
            var actualDisplayDevices = backOfficePage.BackOfficeDetailsPanel.GetDeviceHistoryDisplayDevicesValue();
            VerifyEqual("[SLV-3687] 4. Verify Changed value is remained", !firstDisplayDevicesValue, actualDisplayDevices);

            Step("5. Go to SLV app");
            var desktopPage = Browser.NavigateToLoggedInCMS();
            desktopPage.InstallAppsIfNotExist(App.DeviceHistory);

            Step("6. Go to Device History page");
            var deviceHistoryPage = desktopPage.GoToApp(App.DeviceHistory) as DeviceHistoryPage;
            deviceHistoryPage.GeozoneTreeMainPanel.SelectNode(xmlGeozone);

            Step("7. Expected");
            Step(" - Geozone tree displays devices in case the option is checked. Otherwise, it does NOT display devices in case the option is unchecked");
            Step(" - Filters toolbar button is displayed in case the option is checked and otherwise");
            if (actualDisplayDevices)
            {
                VerifyEqual("7. Verify Geozone tree displays devices", true, deviceHistoryPage.GeozoneTreeMainPanel.GetChildDeviceNamesOfSelectedNode().Count > 0);
            }
            else
            {
                VerifyEqual("7. Verify Geozone tree does NOT display devices", false, deviceHistoryPage.GeozoneTreeMainPanel.GetChildDeviceNamesOfSelectedNode().Count > 0);
            }

            Step("8. Repeat the test with Display devices option is the other way round with step #2");
            backOfficePage = Browser.NavigateToLoggedInBackOfficeApp();
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.DeviceHistory);
            backOfficePage.BackOfficeDetailsPanel.TickDeviceHistoryDisplayDevicesCheckbox(firstDisplayDevicesValue);
            backOfficePage.BackOfficeDetailsPanel.ClickSaveButton();
            backOfficePage.WaitForPreviousActionComplete();

            backOfficePage = Browser.RefreshBackOfficePage();
            backOfficePage.WaitForPreviousActionComplete();
            backOfficePage.BackOfficeOptionsPanel.WaitForPanelLoaded();
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.DeviceHistory);
            actualDisplayDevices = backOfficePage.BackOfficeDetailsPanel.GetDeviceHistoryDisplayDevicesValue();
            VerifyEqual("8. Verify Changed value is remained", firstDisplayDevicesValue, actualDisplayDevices);

            desktopPage = Browser.NavigateToLoggedInCMS();
            deviceHistoryPage = desktopPage.GoToApp(App.DeviceHistory) as DeviceHistoryPage;
            deviceHistoryPage.GeozoneTreeMainPanel.SelectNode(xmlGeozone);
            if (actualDisplayDevices)
            {
                VerifyEqual("8. Verify Geozone tree displays devices", true, deviceHistoryPage.GeozoneTreeMainPanel.GetChildDeviceNamesOfSelectedNode().Count > 0);
            }
            else
            {
                VerifyEqual("8. Verify Geozone tree does NOT display devices", false, deviceHistoryPage.GeozoneTreeMainPanel.GetChildDeviceNamesOfSelectedNode().Count > 0);
            }
        }

        [Test, DynamicRetry]
        [Description("BO_08_03 Options - Device History - Event time visible")]
        public void BO_08_03()
        {
            var testData = GetTestDataOfBO_08_03();
            var xmlGeozone = testData["Geozone"];

            Step("**** Precondition ****");
            Step(" - User must belong to root/admin group");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenBackOfficeApp();
            var backOfficePage = loginPage.LoginAsValidUserToBackOffice(Settings.Users["admin"].Username, Settings.Users["admin"].Password);

            Step("1. Select Device History item");
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.DeviceHistory);

            Step("2. Uncheck Event time visible if it is being checked or check if it's being unchecked then save");
            var firstEventTimeVisible = backOfficePage.BackOfficeDetailsPanel.GetDeviceHistoryEventTimeVisibleValue();
            backOfficePage.BackOfficeDetailsPanel.TickDeviceHistoryEventTimeVisibleCheckbox(!firstEventTimeVisible);
            backOfficePage.BackOfficeDetailsPanel.ClickSaveButton();
            backOfficePage.WaitForPreviousActionComplete();

            Step("3. Reload Back Office page then select Device History item");
            backOfficePage = Browser.RefreshBackOfficePage();
            backOfficePage.WaitForPreviousActionComplete();
            backOfficePage.BackOfficeOptionsPanel.WaitForPanelLoaded();
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.DeviceHistory);

            Step("4. Expected Changed value is remained");            
            var actualEventTimeVisible = backOfficePage.BackOfficeDetailsPanel.GetDeviceHistoryEventTimeVisibleValue();
            VerifyEqual("[SLV-3687] 4. Verify Changed value is remained", !firstEventTimeVisible, actualEventTimeVisible);

            Step("5. Go to SLV app");
            var desktopPage = Browser.NavigateToLoggedInCMS();
            desktopPage.InstallAppsIfNotExist(App.DeviceHistory);

            Step("6. Go to Device History page");
            var deviceHistoryPage = desktopPage.GoToApp(App.DeviceHistory) as DeviceHistoryPage;
            deviceHistoryPage.GeozoneTreeMainPanel.SelectNode(xmlGeozone);
            deviceHistoryPage.GridPanel.WaitForGridContentAvailable();

            Step("7. Expected Custom report grid panel is displayed:");            
            Step(" - In case the option is checked, each column in grid (except Line # and Device columns) displays 2 rows: the above row is attribute name, the below row is 'Value' and 'Timestamp'; otherwise, in case the option is unchecked, each column displays its attribute name only");
            
            var isColumnHeadersHasValueAndTimestamp = deviceHistoryPage.GridPanel.IsColumnHeadersHasValueAndTimestamp();
            if (actualEventTimeVisible)
            {
                VerifyEqual("7. Verify Each column in grid (except Line # and Device columns) displays 2 rows: the above row is attribute name, the below row is 'Value' and 'Timestamp'", true, isColumnHeadersHasValueAndTimestamp);
            }
            else
            {
                VerifyEqual("7. Verify Each column displays its attribute name only (not have 'Value' and 'Timestamp')", false, isColumnHeadersHasValueAndTimestamp);
            }

            Step("8. Repeat the test with Event time visible option is the other way round with step #2");
            backOfficePage = Browser.NavigateToLoggedInBackOfficeApp();
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.DeviceHistory);
            backOfficePage.BackOfficeDetailsPanel.TickDeviceHistoryEventTimeVisibleCheckbox(firstEventTimeVisible);
            backOfficePage.BackOfficeDetailsPanel.ClickSaveButton();
            backOfficePage.WaitForPreviousActionComplete();

            backOfficePage = Browser.RefreshBackOfficePage();
            backOfficePage.WaitForPreviousActionComplete();
            backOfficePage.BackOfficeOptionsPanel.WaitForPanelLoaded();
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.DeviceHistory);
            actualEventTimeVisible = backOfficePage.BackOfficeDetailsPanel.GetDeviceHistoryEventTimeVisibleValue();
            VerifyEqual("8. Verify Changed value is remained", firstEventTimeVisible, actualEventTimeVisible);

            desktopPage = Browser.NavigateToLoggedInCMS();
            deviceHistoryPage = desktopPage.GoToApp(App.DeviceHistory) as DeviceHistoryPage;
            deviceHistoryPage.GeozoneTreeMainPanel.SelectNode(xmlGeozone);
            deviceHistoryPage.GridPanel.WaitForGridContentAvailable();
            
            isColumnHeadersHasValueAndTimestamp = deviceHistoryPage.GridPanel.IsColumnHeadersHasValueAndTimestamp();
            if (actualEventTimeVisible)
            {
                VerifyEqual("8. Verify Each column in grid (except Line # and Device columns) displays 2 rows: the above row is attribute name, the below row is 'Value' and 'Timestamp'", true, isColumnHeadersHasValueAndTimestamp);
            }
            else
            {
                VerifyEqual("8. Verify Each column displays its attribute name only (not have 'Value' and 'Timestamp')", false, isColumnHeadersHasValueAndTimestamp);
            }
        }

        [Test, DynamicRetry]
        [Description("BO_08_04 Options - Device History - Rows count per page")]
        public void BO_08_04()
        {
            var testData = GetTestDataOfBO_08_04();
            var xmlGeozone = testData["Geozone"];
            var xmlRowsPerPage = testData["RowsPerPage"];

            Step("**** Precondition ****");
            Step(" - User must belong to root/admin group");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenBackOfficeApp();
            var backOfficePage = loginPage.LoginAsValidUserToBackOffice(Settings.Users["admin"].Username, Settings.Users["admin"].Password);

            Step("1. Select Device History item");
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.DeviceHistory);

            Step("2. Change Rows count per page then save");
            var currentRowsPerPage = backOfficePage.BackOfficeDetailsPanel.GetDeviceHistoryRowsPerPageValue();
            backOfficePage.BackOfficeDetailsPanel.TickDeviceHistoryEventTimeVisibleCheckbox(false);
            backOfficePage.BackOfficeDetailsPanel.EnterDeviceHistoryRowsPerPageNumericInput(xmlRowsPerPage);
            backOfficePage.BackOfficeDetailsPanel.ClickSaveButton();
            backOfficePage.WaitForPreviousActionComplete();

            Step("3. Reload Back Office page then select Device History item");
            backOfficePage = Browser.RefreshBackOfficePage();
            backOfficePage.WaitForPreviousActionComplete();
            backOfficePage.BackOfficeOptionsPanel.WaitForPanelLoaded();
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.DeviceHistory);

            Step("4. Expected Changed value is remained");
            Step("--> SLV-3687: BackOffice - Unable to save Device History config");
            var actualRowsPerPage = backOfficePage.BackOfficeDetailsPanel.GetDeviceHistoryRowsPerPageValue();
            VerifyEqual("[SLV-3687] 4. Verify Changed value is remained", xmlRowsPerPage, actualRowsPerPage);

            Step("5. Go to SLV app");
            var desktopPage = Browser.NavigateToLoggedInCMS();
            desktopPage.InstallAppsIfNotExist(App.DeviceHistory);

            Step("6. Go to Device History page");
            var deviceHistoryPage = desktopPage.GoToApp(App.DeviceHistory) as DeviceHistoryPage;

            Step("7. Select a geozone");
            deviceHistoryPage.GeozoneTreeMainPanel.SelectNode(xmlGeozone);
            var geozoneDevicesCount = deviceHistoryPage.GeozoneTreeMainPanel.GetSelectedNodeDevicesCount();
            deviceHistoryPage.GridPanel.WaitForGridContentAvailable();

            Step("8. Expected Custom report grid panel is displayed: number of devices each page (except the last page) equals the value set at step #2. Number of devices of the last page should be less than or equal the value set at step #2");
            var rowsPerPage = int.Parse(xmlRowsPerPage);

            if (geozoneDevicesCount >= rowsPerPage)
            {
                var tblGrid = deviceHistoryPage.GridPanel.BuildDataTableFromGrid();
                VerifyEqual(string.Format("8. Verify number of devices each page 1 is '{0}'", xmlRowsPerPage), true, tblGrid.Rows.Count == rowsPerPage);

                var page = geozoneDevicesCount / rowsPerPage;
                for (int j = 0; j < page - 1; j++)
                {
                    deviceHistoryPage.GridPanel.ClickFooterPageNextButton();
                    deviceHistoryPage.GridPanel.WaitForLeftFooterTextDisplayed();

                    tblGrid = deviceHistoryPage.GridPanel.BuildDataTableFromGrid();
                    VerifyEqual(string.Format("8. Verify number of devices each page {0} is {1}'", j + 2, xmlRowsPerPage), true, tblGrid.Rows.Count == rowsPerPage);
                }
            }
            else
            {                
                Warning(string.Format("8. Verify Geozone has devices count {0} < rows per page {1}", geozoneDevicesCount, xmlRowsPerPage));
            }

            Info(string.Format("Reset Rows per Page to {0}", currentRowsPerPage));
            backOfficePage = Browser.NavigateToLoggedInBackOfficeApp();
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.DeviceHistory);
            backOfficePage.BackOfficeDetailsPanel.EnterDeviceHistoryRowsPerPageNumericInput(currentRowsPerPage);
            backOfficePage.BackOfficeDetailsPanel.ClickSaveButton();
            backOfficePage.WaitForPreviousActionComplete();
        }

        [Test, DynamicRetry]
        [Description("BO_09_01 Options - Failure Analysis - UI")]
        public void BO_09_01()
        {
            Step("**** Precondition ****");
            Step(" - User must belong to root/admin group");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenBackOfficeApp();
            var backOfficePage = loginPage.LoginAsValidUserToBackOffice(Settings.Users["admin"].Username, Settings.Users["admin"].Password);
            Step("1. Select Failure Analysis item");
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.FailureAnalysis);

            Step("2. Expected UI as defined");
            VerifyEqual("2. Verify Treeview configuration", "Treeview configuration", backOfficePage.BackOfficeDetailsPanel.GetFailureAnalysisTreeviewConfigurationText());
            VerifyEqual("2. Verify Display devices", "Display devices", backOfficePage.BackOfficeDetailsPanel.GetFailureAnalysisDisplayDevicesText());
            VerifyEqual("2. Verify Display devices checkbox enable", true, backOfficePage.BackOfficeDetailsPanel.IsFailureAnalysisDisplayDevicesCheckboxEditable());
            VerifyEqual("2. Verify Display devices description", "Defines if devices are displayed in geozone tree view or not.", backOfficePage.BackOfficeDetailsPanel.GetFailureAnalysisDisplayDevicesDescText());

            VerifyEqual("2. Verify Search attributes", "Search attributes", backOfficePage.BackOfficeDetailsPanel.GetFailureAnalysisSearchAttributesText());
            VerifyEqual("2. Verify Search attributes list", true, backOfficePage.BackOfficeDetailsPanel.GetFailureAnalysisSearchAttributesNameList().Count > 0);
            VerifyEqual("2. Verify Search attributes description", "Attributes available in the search bar at the bottom of the geozone tree view.", backOfficePage.BackOfficeDetailsPanel.GetFailureAnalysisSearchAttributesDescText());
        }

        [Test, DynamicRetry]
        [Description("BO_09_02 Options - Failure Analysis - Display devices in geozone tree")]
        public void BO_09_02()
        {
            var testData = GetTestDataOfBO_09_02();
            var xmlGeozone = testData["Geozone"];

            Step("**** Precondition ****");
            Step(" - User must belong to root/admin group");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenBackOfficeApp();
            var backOfficePage = loginPage.LoginAsValidUserToBackOffice(Settings.Users["admin"].Username, Settings.Users["admin"].Password);

            Step("1. Select Failure Analysis item");
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.FailureAnalysis);

            Step("2. Uncheck Display devices option if it is being checked or check if it's being unchecked then save");
            var firstDisplayDevicesValue = backOfficePage.BackOfficeDetailsPanel.GetFailureAnalysisDisplayDevicesValue();
            backOfficePage.BackOfficeDetailsPanel.TickFailureAnalysisDisplayDevicesCheckbox(!firstDisplayDevicesValue);
            backOfficePage.BackOfficeDetailsPanel.ClickSaveButton();
            backOfficePage.WaitForPreviousActionComplete();

            Step("3. Reload Back Office page then select Failure Analysis item");
            backOfficePage = Browser.RefreshBackOfficePage();
            backOfficePage.WaitForPreviousActionComplete();
            backOfficePage.BackOfficeOptionsPanel.WaitForPanelLoaded();
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.FailureAnalysis);

            Step("4. Expected Changed value is remained");
            Step("--> SLV-3177: Back Office - Cannot save from the Desktop, Failure Analysis and Failure Tracking pages");
            var actualDisplayDevices = backOfficePage.BackOfficeDetailsPanel.GetFailureAnalysisDisplayDevicesValue();
            VerifyEqual("4. Verify Changed value is remained", !firstDisplayDevicesValue, actualDisplayDevices);

            Step("5. Go to SLV app");
            var desktopPage = Browser.NavigateToLoggedInCMS();
            desktopPage.InstallAppsIfNotExist(App.FailureAnalysis);

            Step("6. Go to Failure Analysis page");
            var failureAnalysisPage = desktopPage.GoToApp(App.FailureAnalysis) as FailureAnalysisPage;
            failureAnalysisPage.GeozoneTreeMainPanel.SelectNode(xmlGeozone);

            Step("7. Expected");
            Step(" - Geozone tree displays devices in case the option is checked. Otherwise, it does NOT display devices in case the option is unchecked");
            Step(" - Filters toolbar button is displayed in case the option is checked and otherwise");
            if (actualDisplayDevices)
            {
                VerifyEqual("7. Verify Geozone tree displays devices", true, failureAnalysisPage.GeozoneTreeMainPanel.GetChildDeviceNamesOfSelectedNode().Count > 0);
            }
            else
            {
                VerifyEqual("7. Verify Geozone tree does NOT display devices", false, failureAnalysisPage.GeozoneTreeMainPanel.GetChildDeviceNamesOfSelectedNode().Count > 0);
            }

            Step("8. Repeat the test with Display devices option is the other way round with step #2");
            backOfficePage = Browser.NavigateToLoggedInBackOfficeApp();
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.FailureAnalysis);
            backOfficePage.BackOfficeDetailsPanel.TickFailureAnalysisDisplayDevicesCheckbox(firstDisplayDevicesValue);
            backOfficePage.BackOfficeDetailsPanel.ClickSaveButton();
            backOfficePage.WaitForPreviousActionComplete();

            backOfficePage = Browser.RefreshBackOfficePage();
            backOfficePage.WaitForPreviousActionComplete();
            backOfficePage.BackOfficeOptionsPanel.WaitForPanelLoaded();
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.FailureAnalysis);
            actualDisplayDevices = backOfficePage.BackOfficeDetailsPanel.GetFailureAnalysisDisplayDevicesValue();
            VerifyEqual("8. Verify Changed value is remained", firstDisplayDevicesValue, actualDisplayDevices);

            desktopPage = Browser.NavigateToLoggedInCMS();
            failureAnalysisPage = desktopPage.GoToApp(App.FailureAnalysis) as FailureAnalysisPage;
            failureAnalysisPage.GeozoneTreeMainPanel.SelectNode(xmlGeozone);
            if (actualDisplayDevices)
            {
                VerifyEqual("8. Verify Geozone tree displays devices", true, failureAnalysisPage.GeozoneTreeMainPanel.GetChildDeviceNamesOfSelectedNode().Count > 0);
            }
            else
            {
                VerifyEqual("8. Verify Geozone tree does NOT display devices", false, failureAnalysisPage.GeozoneTreeMainPanel.GetChildDeviceNamesOfSelectedNode().Count > 0);
            }
        }

        [Test, DynamicRetry]
        [Description("BO_09_03 Options - Failure Analysis - Search attributes")]
        public void BO_09_03()
        {
            Step("**** Precondition ****");
            Step(" - User must belong to root/admin group");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenBackOfficeApp();
            var backOfficePage = loginPage.LoginAsValidUserToBackOffice(Settings.Users["admin"].Username, Settings.Users["admin"].Password);

            Step("1. Select Failure Analysis item");
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.FailureAnalysis);
            var expectedAttributes = backOfficePage.GetAttributesKey();

            Step("2. Randomly remove one of following attributes then save");
            Step(" - name (Name)");
            Step(" - idOnController (Identifier)");
            Step(" - controllerStrId (Controller ID)");
            Step(" - categoryStrId (Category)");
            Step(" - MacAddress (Unique address)");
            Step(" - DimmingGroupName (Dimming group)");
            Step(" - installStatus (Install status)");
            Step(" - ConfigStatus (Configuration status)");
            Step(" - address (Address 1)");
            Step(" - location.streetdescription (Address 2)");
            Step(" - location.city (City)");
            Step(" - location.zipcode (Zip code)");
            Step(" - client.name (Customer name)");
            Step(" - client.number (Customer number)");
            Step(" - luminaire.model (Luminaire model)");
            Step(" - SoftwareVersion (Software version)");
            Step(" - comment (Comment)");
            var currentAttributes = backOfficePage.BackOfficeDetailsPanel.GetFailureAnalysisSearchAttributesNameList();            
            expectedAttributes.Sort();
            currentAttributes.Sort();
            VerifyEqual("[SC-2002] 2. Verify Attributes are matched as expected", expectedAttributes, currentAttributes, false);

            var randomAttributes = expectedAttributes.PickRandom(2);
            var remainingAttributes = expectedAttributes.Clone().ToList();
            remainingAttributes.RemoveAll(p => randomAttributes.Contains(p));
            backOfficePage.BackOfficeDetailsPanel.RemoveFailureAnalysisAttributes(randomAttributes.ToArray());
            backOfficePage.BackOfficeDetailsPanel.ClickSaveButton();
            backOfficePage.WaitForPreviousActionComplete();
            var expectedAttribuesName = backOfficePage.GetAttributesName(remainingAttributes.ToArray());
            Step(" --> Removed attributes: {0}", string.Join(", ", randomAttributes));
            Step(" --> Remaining attributes: {0}", string.Join(", ", remainingAttributes));
            Step(" --> Remaining attributes name: {0}", string.Join(", ", expectedAttribuesName));

            Step("3. Reload Back Office page then select Failure Analysis item");
            backOfficePage = Browser.RefreshBackOfficePage();
            backOfficePage.WaitForPreviousActionComplete();
            backOfficePage.BackOfficeOptionsPanel.WaitForPanelLoaded();
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.FailureAnalysis);

            Step("4. Expected Removed attributes are no longer in the list, intact are remained");
            Step("--> SLV-3177: Back Office - Cannot save from the Desktop, Failure Analysis and Failure Tracking pages");
            var actualAttributes = backOfficePage.BackOfficeDetailsPanel.GetFailureAnalysisSearchAttributesNameList();
            Step(" --> Actual attributes: {0}", string.Join(", ", actualAttributes));
            VerifyTrue("4. Verify Removed attributes are no longer in the list", !actualAttributes.Exists(p => randomAttributes.Contains(p)), string.Join(", ", randomAttributes), string.Join(", ", actualAttributes));            
            remainingAttributes.Sort();
            actualAttributes.Sort();
            VerifyEqual("4. Verify Attributes intact are remained", remainingAttributes, actualAttributes, false);

            Step("5. Go to SLV app");
            var desktopPage = Browser.NavigateToLoggedInCMS();
            desktopPage.InstallAppsIfNotExist(App.FailureAnalysis);

            Step("6. Go to Failure Analysis");
            Step("7. Expand Search bar at the bottom");
            Step("8. Expected Verify attribute dropdown contains all attributes in Search attribute list");
            var failureAnalysisPage = desktopPage.GoToApp(App.FailureAnalysis) as FailureAnalysisPage;
            failureAnalysisPage.GeozoneTreeMainPanel.ClickExpandSearchButton();
            var attributesNameDropDown = failureAnalysisPage.GeozoneTreeMainPanel.GetAllAttributeDropDownItems();
            Step(" --> Dropdown attributes name: {0}", string.Join(", ", attributesNameDropDown));
            VerifyEqual("8. Verify Verify attribute dropdown contains all attributes in Search attribute list", expectedAttribuesName, attributesNameDropDown, false);

            Info("Reset random apps removed");
            backOfficePage = Browser.NavigateToLoggedInBackOfficeApp();
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.FailureAnalysis);
            backOfficePage.BackOfficeDetailsPanel.AddFailureAnalysisAttributes(randomAttributes.ToArray());
            backOfficePage.BackOfficeDetailsPanel.ClickSaveButton();
            backOfficePage.WaitForPreviousActionComplete();
        }

        [Test, DynamicRetry]
        [Description("BO_10_01 Options - Failure Tracking - UI")]
        public void BO_10_01()
        {
            Step("**** Precondition ****");
            Step(" - User must belong to root/admin group");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenBackOfficeApp();
            var backOfficePage = loginPage.LoginAsValidUserToBackOffice(Settings.Users["admin"].Username, Settings.Users["admin"].Password);
            Step("1. Select Failure Tracking item");
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.FailureTracking);

            Step("2. Expected UI as defined");
            VerifyEqual("2. Verify Treeview configuration", "Treeview configuration", backOfficePage.BackOfficeDetailsPanel.GetFailureTrackingTreeviewConfigurationText());
            VerifyEqual("2. Verify Display devices", "Display devices", backOfficePage.BackOfficeDetailsPanel.GetFailureTrackingDisplayDevicesText());
            VerifyEqual("2. Verify Display devices checkbox enable", true, backOfficePage.BackOfficeDetailsPanel.IsFailureTrackingDisplayDevicesCheckboxEditable());
            VerifyEqual("2. Verify Display devices description", "Defines if devices are displayed in geozone tree view or not.", backOfficePage.BackOfficeDetailsPanel.GetFailureTrackingDisplayDevicesDescText());
            VerifyEqual("2. Verify Enable map filter", "Enable map filter", backOfficePage.BackOfficeDetailsPanel.GetFailureTrackingMapFilterText());
            VerifyEqual("2. Verify Enable map filter checkbox enable", true, backOfficePage.BackOfficeDetailsPanel.IsFailureTrackingMapFilterCheckboxEditable());
            VerifyEqual("2. Verify Enable map filter description", "Defines if the filter on devices found in the search is allowed on the map.", backOfficePage.BackOfficeDetailsPanel.GetFailureTrackingMapFilterDescText());

            VerifyEqual("2. Verify Search attributes", "Search attributes", backOfficePage.BackOfficeDetailsPanel.GetFailureTrackingSearchAttributesText());
            VerifyEqual("2. Verify Search attributes list", true, backOfficePage.BackOfficeDetailsPanel.GetFailureTrackingSearchAttributesNameList().Count > 0);
            VerifyEqual("2. Verify Search attributes description", "Attributes available in the search bar at the bottom of the geozone tree view.", backOfficePage.BackOfficeDetailsPanel.GetFailureTrackingSearchAttributesDescText());
        }

        [Test, DynamicRetry]
        [Description("BO_10_02 Options - Failure Tracking - Display devices in geozone tree")]
        public void BO_10_02()
        {
            var testData = GetTestDataOfBO_10_02();
            var xmlGeozone = testData["Geozone"];

            Step("**** Precondition ****");
            Step(" - User must belong to root/admin group");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenBackOfficeApp();
            var backOfficePage = loginPage.LoginAsValidUserToBackOffice(Settings.Users["admin"].Username, Settings.Users["admin"].Password);

            Step("1. Select Failure Tracking item");
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.FailureTracking);

            Step("2. Uncheck Display devices option if it is being checked or check if it's being unchecked then save");
            var firstDisplayDevicesValue = backOfficePage.BackOfficeDetailsPanel.GetFailureTrackingDisplayDevicesValue();
            backOfficePage.BackOfficeDetailsPanel.TickFailureTrackingDisplayDevicesCheckbox(!firstDisplayDevicesValue);
            backOfficePage.BackOfficeDetailsPanel.ClickSaveButton();
            backOfficePage.WaitForPreviousActionComplete();

            Step("3. Reload Back Office page then select Failure Tracking item");
            backOfficePage = Browser.RefreshBackOfficePage();
            backOfficePage.WaitForPreviousActionComplete();
            backOfficePage.BackOfficeOptionsPanel.WaitForPanelLoaded();
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.FailureTracking);

            Step("4. Expected Changed value is remained");
            Step("--> SLV-3177: Back Office - Cannot save from the Desktop, Failure Tracking and Failure Tracking pages");
            var actualDisplayDevices = backOfficePage.BackOfficeDetailsPanel.GetFailureTrackingDisplayDevicesValue();
            VerifyEqual("4. Verify Changed value is remained", !firstDisplayDevicesValue, actualDisplayDevices);

            Step("5. Go to SLV app");
            var desktopPage = Browser.NavigateToLoggedInCMS();
            desktopPage.InstallAppsIfNotExist(App.FailureTracking);

            Step("6. Go to Failure Tracking page");
            var failureTrackingPage = desktopPage.GoToApp(App.FailureTracking) as FailureTrackingPage;
            failureTrackingPage.GeozoneTreeMainPanel.SelectNode(xmlGeozone);

            Step("7. Expected");
            Step(" - Geozone tree displays devices in case the option is checked. Otherwise, it does NOT display devices in case the option is unchecked");
            Step(" - Filters toolbar button is displayed in case the option is checked and otherwise");
            if (actualDisplayDevices)
            {
                VerifyEqual("7. Verify Geozone tree displays devices", true, failureTrackingPage.GeozoneTreeMainPanel.GetChildDeviceNamesOfSelectedNode().Count > 0);
            }
            else
            {
                VerifyEqual("7. Verify Geozone tree does NOT display devices", false, failureTrackingPage.GeozoneTreeMainPanel.GetChildDeviceNamesOfSelectedNode().Count > 0);
            }

            Step("8. Repeat the test with Display devices option is the other way round with step #2");
            backOfficePage = Browser.NavigateToLoggedInBackOfficeApp();
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.FailureTracking);
            backOfficePage.BackOfficeDetailsPanel.TickFailureTrackingDisplayDevicesCheckbox(firstDisplayDevicesValue);
            backOfficePage.BackOfficeDetailsPanel.ClickSaveButton();
            backOfficePage.WaitForPreviousActionComplete();

            backOfficePage = Browser.RefreshBackOfficePage();
            backOfficePage.WaitForPreviousActionComplete();
            backOfficePage.BackOfficeOptionsPanel.WaitForPanelLoaded();
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.FailureTracking);
            actualDisplayDevices = backOfficePage.BackOfficeDetailsPanel.GetFailureTrackingDisplayDevicesValue();
            VerifyEqual("8. Verify Changed value is remained", firstDisplayDevicesValue, actualDisplayDevices);

            desktopPage = Browser.NavigateToLoggedInCMS();
            failureTrackingPage = desktopPage.GoToApp(App.FailureTracking) as FailureTrackingPage;
            failureTrackingPage.GeozoneTreeMainPanel.SelectNode(xmlGeozone);
            if (actualDisplayDevices)
            {
                VerifyEqual("8. Verify Geozone tree displays devices", true, failureTrackingPage.GeozoneTreeMainPanel.GetChildDeviceNamesOfSelectedNode().Count > 0);
            }
            else
            {
                VerifyEqual("8. Verify Geozone tree does NOT display devices", false, failureTrackingPage.GeozoneTreeMainPanel.GetChildDeviceNamesOfSelectedNode().Count > 0);
            }
        }

        [Test, DynamicRetry]
        [Description("BO_10_03 Options - Failure Tracking - Enable map filter in geozone")]
        public void BO_10_03()
        {
            var searchName = "Telematics";

            Step("**** Precondition ****");
            Step(" - User must belong to root/admin group");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenBackOfficeApp();
            var backOfficePage = loginPage.LoginAsValidUserToBackOffice(Settings.Users["admin"].Username, Settings.Users["admin"].Password);

            Step("1. Select Failure Tracking item");
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.FailureTracking);

            Step("2. Uncheck Enable map option if it is being checked or check if it's being unchecked");
            var firstMapFilterValue = backOfficePage.BackOfficeDetailsPanel.GetFailureTrackingMapFilterValue();
            backOfficePage.BackOfficeDetailsPanel.TickFailureTrackingMapFilterCheckbox(!firstMapFilterValue);

            Step("3. Check Display devices option then save");
            backOfficePage.BackOfficeDetailsPanel.TickFailureTrackingDisplayDevicesCheckbox(true);
            backOfficePage.BackOfficeDetailsPanel.ClickSaveButton();
            backOfficePage.WaitForPreviousActionComplete();

            Step("4. Reload Back Office page then select Failure Tracking item");
            backOfficePage = Browser.RefreshBackOfficePage();
            backOfficePage.WaitForPreviousActionComplete();
            backOfficePage.BackOfficeOptionsPanel.WaitForPanelLoaded();
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.FailureTracking);

            Step("5. Expected Changed value is remained");
            Step("--> SLV-3177: Back Office - Cannot save from the Desktop, Failure Tracking and Failure Tracking pages");
            var actualMapFilter = backOfficePage.BackOfficeDetailsPanel.GetFailureTrackingMapFilterValue();
            VerifyEqual("5. Verify Changed value is remained", !firstMapFilterValue, actualMapFilter);

            Step("6. Go to SLV app");
            var desktopPage = Browser.NavigateToLoggedInCMS();
            desktopPage.InstallAppsIfNotExist(App.FailureTracking);

            Step("7. Go to Failure Tracking page");
            var failureTrackingPage = desktopPage.GoToApp(App.FailureTracking) as FailureTrackingPage;

            Step("8. Search for a device from Search bar under geozone tree");
            failureTrackingPage.GeozoneTreeMainPanel.ChangeSearchAttribute("Name", "Contains");
            failureTrackingPage.GeozoneTreeMainPanel.EnterSearchTextInput(searchName);
            failureTrackingPage.GeozoneTreeMainPanel.ClickSearchButton();
            failureTrackingPage.WaitForPreviousActionComplete();

            Step("9. Expected Search Results panel appears. 'Filter results on the map' toolbar button is displayed in case the option is checked and otherwise");
            VerifyEqual("[SLV-3206] 9. Verify Search Results panel appears. 'Filter results on the map' toolbar button is displayed in case the option is checked and otherwise", !firstMapFilterValue, failureTrackingPage.GeozoneTreeMainPanel.SearchResultsGeozonePanel.IsMapFilterButtonVisible());

            Step("10. Repeat the test with Enable map option is the other way round with step #2");
            backOfficePage = Browser.NavigateToLoggedInBackOfficeApp();
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.FailureTracking);
            backOfficePage.BackOfficeDetailsPanel.TickFailureTrackingMapFilterCheckbox(firstMapFilterValue);
            backOfficePage.BackOfficeDetailsPanel.ClickSaveButton();
            backOfficePage.WaitForPreviousActionComplete();

            backOfficePage = Browser.RefreshBackOfficePage();
            backOfficePage.WaitForPreviousActionComplete();
            backOfficePage.BackOfficeOptionsPanel.WaitForPanelLoaded();
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.FailureTracking);
            actualMapFilter = backOfficePage.BackOfficeDetailsPanel.GetFailureTrackingMapFilterValue();
            VerifyEqual("10. Verify Changed value is remained", firstMapFilterValue, actualMapFilter);

            desktopPage = Browser.NavigateToLoggedInCMS();
            failureTrackingPage = desktopPage.GoToApp(App.FailureTracking) as FailureTrackingPage;
            failureTrackingPage.GeozoneTreeMainPanel.ChangeSearchAttribute("Name", "Contains");
            failureTrackingPage.GeozoneTreeMainPanel.EnterSearchTextInput(searchName);
            failureTrackingPage.GeozoneTreeMainPanel.ClickSearchButton();
            failureTrackingPage.WaitForPreviousActionComplete();

            VerifyEqual("[SLV-3206] Search Results panel appears. 'Filter results on the map' toolbar button is displayed in case the option is checked and otherwise", firstMapFilterValue, failureTrackingPage.GeozoneTreeMainPanel.SearchResultsGeozonePanel.IsMapFilterButtonVisible());
        }

        [Test, DynamicRetry]
        [Description("BO_10_04 Options - Failure Tracking - Search attributes")]
        public void BO_10_04()
        {
            Step("**** Precondition ****");
            Step(" - User must belong to root/admin group");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenBackOfficeApp();
            var backOfficePage = loginPage.LoginAsValidUserToBackOffice(Settings.Users["admin"].Username, Settings.Users["admin"].Password);

            Step("1. Select Failure Tracking item");
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.FailureTracking);
            var expectedAttributes = backOfficePage.GetAttributesKey();

            Step("2. Randomly remove one of following attributes then save");
            Step(" - name (Name)");
            Step(" - idOnController (Identifier)");
            Step(" - controllerStrId (Controller ID)");
            Step(" - categoryStrId (Category)");
            Step(" - MacAddress (Unique address)");
            Step(" - DimmingGroupName (Dimming group)");
            Step(" - installStatus (Install status)");
            Step(" - ConfigStatus (Configuration status)");
            Step(" - address (Address 1)");
            Step(" - location.streetdescription (Address 2)");
            Step(" - location.city (City)");
            Step(" - location.zipcode (Zip code)");
            Step(" - client.name (Customer name)");
            Step(" - client.number (Customer number)");
            Step(" - luminaire.model (Luminaire model)");
            Step(" - SoftwareVersion (Software version)");
            Step(" - comment (Comment)");
            var currentAttributes = backOfficePage.BackOfficeDetailsPanel.GetFailureTrackingSearchAttributesNameList();
            VerifyEqual("[SC-2002] 2. Verify Attribues are matched as expected", expectedAttributes, currentAttributes, false);

            var randomAttributes = expectedAttributes.PickRandom(2);
            var remainingAttributes = expectedAttributes.Clone().ToList();
            remainingAttributes.RemoveAll(p => randomAttributes.Contains(p));
            backOfficePage.BackOfficeDetailsPanel.RemoveFailureTrackingAttributes(randomAttributes.ToArray());
            backOfficePage.BackOfficeDetailsPanel.ClickSaveButton();
            backOfficePage.WaitForPreviousActionComplete();
            var expectedAttribuesName = backOfficePage.GetAttributesName(remainingAttributes.ToArray());
            Step(" --> Removed attributes: {0}", string.Join(", ", randomAttributes));
            Step(" --> Remaining attributes: {0}", string.Join(", ", remainingAttributes));
            Step(" --> Remaining attributes name: {0}", string.Join(", ", expectedAttribuesName));

            Step("3. Reload Back Office page then select Failure Tracking item");
            backOfficePage = Browser.RefreshBackOfficePage();
            backOfficePage.WaitForPreviousActionComplete();
            backOfficePage.BackOfficeOptionsPanel.WaitForPanelLoaded();
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.FailureTracking);

            Step("4. Expected Removed attributes are no longer in the list, intact are remained");
            Step("--> SLV-3177: Back Office - Cannot save from the Desktop, Failure Tracking and Failure Tracking pages");
            var actualAttributes = backOfficePage.BackOfficeDetailsPanel.GetFailureTrackingSearchAttributesNameList();
            Step(" --> Actual attributes: {0}", string.Join(", ", actualAttributes));
            VerifyTrue("4. Verify Removed attributes are no longer in the list", !actualAttributes.Exists(p => randomAttributes.Contains(p)), string.Join(", ", randomAttributes), string.Join(", ", actualAttributes));
            remainingAttributes.Sort();
            actualAttributes.Sort();
            VerifyEqual("4. Verify Attributes intact are remained", remainingAttributes, actualAttributes, false);

            Step("5. Go to SLV app");
            var desktopPage = Browser.NavigateToLoggedInCMS();
            desktopPage.InstallAppsIfNotExist(App.FailureTracking);

            Step("6. Go to Failure Tracking");
            Step("7. Expand Search bar at the bottom");
            Step("8. Expected Verify attribute dropdown contains all attributes in Search attribute list");
            var failureTrackingPage = desktopPage.GoToApp(App.FailureTracking) as FailureTrackingPage;
            failureTrackingPage.GeozoneTreeMainPanel.ClickExpandSearchButton();
            var attributesNameDropDown = failureTrackingPage.GeozoneTreeMainPanel.GetAllAttributeDropDownItems();
            Step(" --> Dropdown attributes name: {0}", string.Join(", ", attributesNameDropDown));
            VerifyEqual("8. Verify Verify attribute dropdown contains all attributes in Search attribute list", expectedAttribuesName, attributesNameDropDown, false);

            Info("Reset random apps removed");
            backOfficePage = Browser.NavigateToLoggedInBackOfficeApp();
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.FailureTracking);
            backOfficePage.BackOfficeDetailsPanel.AddFailureTrackingAttributes(randomAttributes.ToArray());
            backOfficePage.BackOfficeDetailsPanel.ClickSaveButton();
            backOfficePage.WaitForPreviousActionComplete();
        }

        [Test, DynamicRetry]
        [Description("BO_11_01 Options - Advanced Search - UI")]
        public void BO_11_01()
        {
            Step("**** Precondition ****");
            Step(" - User must belong to root/admin group");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenBackOfficeApp();
            var backOfficePage = loginPage.LoginAsValidUserToBackOffice(Settings.Users["admin"].Username, Settings.Users["admin"].Password);
            Step("1. Select Advanced Search item");

            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.AdvancedSearch);
            Step("2. Expected UI as defined");
            VerifyEqual("2. Verify Report  configuration", "Report configuration", backOfficePage.BackOfficeDetailsPanel.GetAdvancedSearchReportConfigurationText());
            VerifyEqual("2. Verify Event time visible", "Event time visible", backOfficePage.BackOfficeDetailsPanel.GetAdvancedSearchEventTimeVisibleText());
            VerifyEqual("2. Verify Event time visible checkbox enable", true, backOfficePage.BackOfficeDetailsPanel.IsAdvancedSearchEventTimeVisibleCheckboxEditable());
            VerifyEqual("2. Verify Event time visible description", "Defines if event time columns are visibled for each attributs in the report table.", backOfficePage.BackOfficeDetailsPanel.GetAdvancedSearchEventTimeVisibleDescText());
            VerifyEqual("2. Verify Rows count per page", "Rows count per page", backOfficePage.BackOfficeDetailsPanel.GetAdvancedSearchRowsPerPageText());
            VerifyEqual("2. Verify Rows count per page input enable", true, backOfficePage.BackOfficeDetailsPanel.IsAdvancedSearchRowCountPerPageInputEditable());
            VerifyEqual("2. Verify Rows count per page description", "The count of rows loaded and displayed per page in the report table.", backOfficePage.BackOfficeDetailsPanel.GetAdvancedSearchRowsPerPageDescText());

            VerifyEqual("2. Verify Toolbar list label", "Toolbar", backOfficePage.BackOfficeDetailsPanel.GetAdvancedSearchToolbarItemsText());
            VerifyEqual("2. Verify Toolbar list", true, backOfficePage.BackOfficeDetailsPanel.GetAdvancedSearchToolbarItemsNameList().Count > 0);
            VerifyEqual("2. Verify Toolbar list description", "The items of toolbar availables from report grid.", backOfficePage.BackOfficeDetailsPanel.GetAdvancedSearchToolbarItemsDescText());
        }

        [Test, DynamicRetry]
        [Description("BO_11_02 Options - Advanced Search - Event time visible")]
        public void BO_11_02()
        {
            var testData = GetTestDataOfBO_10_02();
            var xmlGeozone = testData["Geozone"];

            Step("**** Precondition ****");
            Step(" - User must belong to root/admin group");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenBackOfficeApp();
            var backOfficePage = loginPage.LoginAsValidUserToBackOffice(Settings.Users["admin"].Username, Settings.Users["admin"].Password);

            Step("1. Select Advanced Search item");
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.AdvancedSearch);

            Step("2. Uncheck Event time visible if it is being checked or check if it's being unchecked then save");
            var firstEventTimeVisible = backOfficePage.BackOfficeDetailsPanel.GetAdvancedSearchEventTimeVisibleValue();
            backOfficePage.BackOfficeDetailsPanel.TickAdvancedSearchEventTimeVisibleCheckbox(!firstEventTimeVisible);
            backOfficePage.BackOfficeDetailsPanel.ClickSaveButton();
            backOfficePage.WaitForPreviousActionComplete();

            Step("3. Reload Back Office page then select Advanced Search item");
            backOfficePage = Browser.RefreshBackOfficePage();
            backOfficePage.WaitForPreviousActionComplete();
            backOfficePage.BackOfficeOptionsPanel.WaitForPanelLoaded();
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.AdvancedSearch);

            Step("4. Expected Changed value is remained");
            var actualEventTimeVisible = backOfficePage.BackOfficeDetailsPanel.GetAdvancedSearchEventTimeVisibleValue();
            VerifyEqual("4. Verify Changed value is remained", !firstEventTimeVisible, actualEventTimeVisible);

            Step("5. Go to SLV app");
            var desktopPage = Browser.NavigateToLoggedInCMS();
            desktopPage.InstallAppsIfNotExist(App.AdvancedSearch);

            Step("6. Go to Advanced Search page");
            var advancedSearchPage = desktopPage.GoToApp(App.AdvancedSearch) as AdvancedSearchPage;
            advancedSearchPage.WaitForSearchWizardPopupPanelDisplayed();

            Step("7. Expected Advanced Search wizard appears");
            VerifyEqual("7. Verify Advanced Search dialog appears with title \"My advanced searches\"", "My advanced searches", advancedSearchPage.SearchWizardPopupPanel.GetPanelTitleText());

            Step("8. Close the wizard");
            advancedSearchPage.SearchWizardPopupPanel.ClickCloseButton();

            Step("9. Expected The wizard disappears");
            advancedSearchPage.WaitForSearchWizardPopupPanelDisappeared();

            Step("10. Expected Custom report grid panel is displayed:");
            Step(" - Timestamp checkbox in grid toolbar is checked in case the option is checked and otherwise");
            Step(" - In case the option is checked, each column in grid (except Line # and Device columns) displays 2 rows: the above row is attribute name, the below row is 'Value' and 'Timestamp'; otherwise, in case the option is unchecked, each column displays its attribute name only");
            var isTimeStampButtonChecked = advancedSearchPage.GridPanel.IsTimeStampButtonChecked();
            var isColumnHeadersHasValueAndTimestamp = advancedSearchPage.GridPanel.IsColumnHeadersHasValueAndTimestamp();
            if (actualEventTimeVisible)
            {
                VerifyEqual("10. Verify Timestamp checkbox in grid toolbar is checked and each column in grid (except Line # and Device columns) displays 2 rows: the above row is attribute name, the below row is 'Value' and 'Timestamp'", true, isTimeStampButtonChecked && isColumnHeadersHasValueAndTimestamp);
            }
            else
            {
                VerifyEqual("10. Verify Timestamp checkbox in grid toolbar is unchecked and each column displays its attribute name only (not have 'Value' and 'Timestamp')", true, isTimeStampButtonChecked == false && isColumnHeadersHasValueAndTimestamp == false);
            }

            Step("11. Repeat the test with Event time visible option is the other way round with step #2");
            backOfficePage = Browser.NavigateToLoggedInBackOfficeApp();
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.AdvancedSearch);
            backOfficePage.BackOfficeDetailsPanel.TickAdvancedSearchEventTimeVisibleCheckbox(firstEventTimeVisible);
            backOfficePage.BackOfficeDetailsPanel.ClickSaveButton();
            backOfficePage.WaitForPreviousActionComplete();

            backOfficePage = Browser.RefreshBackOfficePage();
            backOfficePage.WaitForPreviousActionComplete();
            backOfficePage.BackOfficeOptionsPanel.WaitForPanelLoaded();
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.AdvancedSearch);
            actualEventTimeVisible = backOfficePage.BackOfficeDetailsPanel.GetAdvancedSearchEventTimeVisibleValue();
            VerifyEqual("11. Verify Changed value is remained", firstEventTimeVisible, actualEventTimeVisible);

            desktopPage = Browser.NavigateToLoggedInCMS();
            advancedSearchPage = desktopPage.GoToApp(App.AdvancedSearch) as AdvancedSearchPage;
            advancedSearchPage.WaitForSearchWizardPopupPanelDisplayed();
            advancedSearchPage.SearchWizardPopupPanel.ClickCloseButton();
            advancedSearchPage.WaitForSearchWizardPopupPanelDisappeared();

            isTimeStampButtonChecked = advancedSearchPage.GridPanel.IsTimeStampButtonChecked();
            isColumnHeadersHasValueAndTimestamp = advancedSearchPage.GridPanel.IsColumnHeadersHasValueAndTimestamp();
            if (actualEventTimeVisible)
            {
                VerifyEqual("11. Verify Timestamp checkbox in grid toolbar is checked and each column in grid (except Line # and Device columns) displays 2 rows: the above row is attribute name, the below row is 'Value' and 'Timestamp'", true, isTimeStampButtonChecked && isColumnHeadersHasValueAndTimestamp);
            }
            else
            {
                VerifyEqual("11. Verify Timestamp checkbox in grid toolbar is unchecked and each column displays its attribute name only (not have 'Value' and 'Timestamp')", true, isTimeStampButtonChecked == false && isColumnHeadersHasValueAndTimestamp == false);
            }
        }

        [Test, DynamicRetry]
        [Description("BO_11_03 Options - Advanced Search - Rows count per page")]
        public void BO_11_03()
        {
            var testData = GetTestDataOfBO_11_03();
            var xmlGeozone = testData["Geozone"];
            var xmlRowsPerPage = testData["RowsPerPage"];

            Step("**** Precondition ****");
            Step(" - User must belong to root/admin group");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenBackOfficeApp();
            var backOfficePage = loginPage.LoginAsValidUserToBackOffice(Settings.Users["admin"].Username, Settings.Users["admin"].Password);

            Step("1. Select Advanced Search item");
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.AdvancedSearch);

            Step("2. Change Rows count per page then save");
            var currentRowsPerPage = backOfficePage.BackOfficeDetailsPanel.GetAdvancedSearchRowsPerPageValue();
            backOfficePage.BackOfficeDetailsPanel.TickAdvancedSearchEventTimeVisibleCheckbox(false);
            backOfficePage.BackOfficeDetailsPanel.EnterAdvancedSearchRowsPerPageNumericInput(xmlRowsPerPage);
            backOfficePage.BackOfficeDetailsPanel.ClickSaveButton();
            backOfficePage.WaitForPreviousActionComplete();

            Step("3. Reload Back Office page then select Advanced Search item");
            backOfficePage = Browser.RefreshBackOfficePage();
            backOfficePage.WaitForPreviousActionComplete();
            backOfficePage.BackOfficeOptionsPanel.WaitForPanelLoaded();
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.AdvancedSearch);

            Step("4. Expected Changed value is remained");
            var actualRowsPerPage = backOfficePage.BackOfficeDetailsPanel.GetAdvancedSearchRowsPerPageValue();
            VerifyEqual("4. Verify Changed value is remained", xmlRowsPerPage, actualRowsPerPage);

            Step("5. Go to SLV app");
            var desktopPage = Browser.NavigateToLoggedInCMS();
            desktopPage.InstallAppsIfNotExist(App.AdvancedSearch);

            Step("6. Go to Advanced Search page");
            var advancedSearchPage = desktopPage.GoToApp(App.AdvancedSearch) as AdvancedSearchPage;

            Step("7. Expected Advanced Search wizard appears");
            advancedSearchPage.WaitForSearchWizardPopupPanelDisplayed();

            Step("9. Create an advanced search whose number of devices > rows count per page at step #2");
            var searchName = SLVHelper.GenerateUniqueName(xmlGeozone);
            advancedSearchPage.SearchWizardPopupPanel.ClickNewAdvancedSearchButton();
            advancedSearchPage.SearchWizardPopupPanel.WaitForNewSearchNameInputVisible();
            advancedSearchPage.SearchWizardPopupPanel.EnterNewSearchNameInput(searchName);
            advancedSearchPage.SearchWizardPopupPanel.ClickNextButton();
            advancedSearchPage.SearchWizardPopupPanel.WaitForGeozoneFormDisplayed();
            advancedSearchPage.SearchWizardPopupPanel.GeozoneTreePopupPanel.SelectNode(xmlGeozone);
            var geozoneDevicesCount = advancedSearchPage.SearchWizardPopupPanel.GeozoneTreePopupPanel.GetSelectedNodeDevicesCount();
            advancedSearchPage.SearchWizardPopupPanel.ClickNextButton();
            advancedSearchPage.SearchWizardPopupPanel.WaitForAttributeFormDisplayed();
            advancedSearchPage.SearchWizardPopupPanel.ClickNextButton();
            advancedSearchPage.SearchWizardPopupPanel.WaitForFilterFormDisplayed();
            advancedSearchPage.SearchWizardPopupPanel.ClickNextButton();
            advancedSearchPage.SearchWizardPopupPanel.WaitForFinishFormDisplayed();
            advancedSearchPage.SearchWizardPopupPanel.ClickFinishButton();
            advancedSearchPage.WaitForSearchWizardPopupPanelDisappeared();
            advancedSearchPage.WaitForPreviousActionComplete();
            advancedSearchPage.GridPanel.WaitForGridContentAvailable();

            Step("9. Expected Custom report grid panel is displayed: number of devices each page (except the last page) equals the value set at step #2. Number of devices of the last page should be less than or equal the value set at step #2");
            var rowsPerPage = int.Parse(xmlRowsPerPage);
            if (geozoneDevicesCount >= rowsPerPage)
            {
                var tblGrid = advancedSearchPage.GridPanel.BuildDataTableFromGrid();
                VerifyEqual(string.Format("9. Verify number of devices each page 1 is '{0}'", xmlRowsPerPage), true, tblGrid.Rows.Count == rowsPerPage);

                var page = geozoneDevicesCount / rowsPerPage;
                for (int j = 0; j < page - 1; j++)
                {
                    advancedSearchPage.GridPanel.ClickFooterPageNextButton();
                    advancedSearchPage.GridPanel.WaitForLeftFooterTextDisplayed();

                    tblGrid = advancedSearchPage.GridPanel.BuildDataTableFromGrid();
                    VerifyEqual(string.Format("9. Verify number of devices each page {0} is {1}'", j + 2, xmlRowsPerPage), true, tblGrid.Rows.Count == rowsPerPage);
                }
            }
            else
            {                
                Warning(string.Format("9. Verify Geozone has devices count {0} < rows per page {1}", geozoneDevicesCount, xmlRowsPerPage));
            }

            //Remove new search
            advancedSearchPage.GridPanel.DeleleSelectedRequest();

            Info(string.Format("Reset Rows per Page to {0}", currentRowsPerPage));
            backOfficePage = Browser.NavigateToLoggedInBackOfficeApp();
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.AdvancedSearch);
            backOfficePage.BackOfficeDetailsPanel.EnterAdvancedSearchRowsPerPageNumericInput(currentRowsPerPage);
            backOfficePage.BackOfficeDetailsPanel.ClickSaveButton();
            backOfficePage.WaitForPreviousActionComplete();
        }

        [Test, DynamicRetry]
        [Description("BO_11_04 Options - Advanced Search - Toolbar")]
        public void BO_11_04()
        {
            Step("**** Precondition ****");
            Step(" - User must belong to root/admin group");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenBackOfficeApp();
            var backOfficePage = loginPage.LoginAsValidUserToBackOffice(Settings.Users["admin"].Username, Settings.Users["admin"].Password);

            Step("1. Select Advanced Search item");
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.AdvancedSearch);
            Step("2. Check all toolbar items then save");
            var currentCheckedToolbarItemsList = backOfficePage.BackOfficeDetailsPanel.GetAdvancedSearchAvailableToolbarItemsNameList();
            backOfficePage.BackOfficeDetailsPanel.CheckAdvancedSearchAllToolbarItems();
            backOfficePage.BackOfficeDetailsPanel.ClickSaveButton();
            backOfficePage.WaitForPreviousActionComplete();
            var allCheckedToolbarItemsList = backOfficePage.BackOfficeDetailsPanel.GetAdvancedSearchAvailableToolbarItemsNameList();

            Step("3. Reload Back Office page then select Advanced Search item");
            backOfficePage = Browser.RefreshBackOfficePage();
            backOfficePage.WaitForPreviousActionComplete();
            backOfficePage.BackOfficeOptionsPanel.WaitForPanelLoaded();
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.AdvancedSearch);

            Step("4. Expected Changed value is remained");
            var actualCheckedToolbarItems = backOfficePage.BackOfficeDetailsPanel.GetAdvancedSearchAvailableToolbarItemsNameList();
            VerifyEqual("4. Verify Changed value is remained", allCheckedToolbarItemsList, actualCheckedToolbarItems, false);

            Step("5. Go to SLV app");
            var desktopPage = Browser.NavigateToLoggedInCMS();
            desktopPage.InstallAppsIfNotExist(App.AdvancedSearch);

            Step("6. Go to Advanced Search page");
            var advancedSearchPage = desktopPage.GoToApp(App.AdvancedSearch) as AdvancedSearchPage;
            advancedSearchPage.WaitForSearchWizardPopupPanelDisplayed();

            Step("7. Expected Advanced Search wizard appears");
            VerifyEqual("7. Verify Advanced Search dialog appears with title \"My advanced searches\"", "My advanced searches", advancedSearchPage.SearchWizardPopupPanel.GetPanelTitleText());

            Step("8. Close the wizard");
            advancedSearchPage.SearchWizardPopupPanel.ClickCloseButton();

            Step("9. Expected The wizard disappears");
            advancedSearchPage.WaitForSearchWizardPopupPanelDisappeared();

            Step("10. Expected Verify all checked toolbar buttons present in the grid toolbar");
            var availableToolbarItems = advancedSearchPage.GridPanel.GetBackOfficeAvailableToolbarButtons();
            VerifyEqual("10. Verify all checked toolbar buttons present in the grid toolbar", allCheckedToolbarItemsList, availableToolbarItems, false);

            Step("11. Repeat the test in case all items are unchecked and some items are un/checked randomly. Verify checked items are displayed and unchecked ones are not displayed in grid toolbar");
            backOfficePage = Browser.NavigateToLoggedInBackOfficeApp();
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.AdvancedSearch);
            backOfficePage.BackOfficeDetailsPanel.UncheckAdvancedSearchRandomToolbarItems(2);
            backOfficePage.BackOfficeDetailsPanel.ClickSaveButton();
            backOfficePage.WaitForPreviousActionComplete();
            allCheckedToolbarItemsList = backOfficePage.BackOfficeDetailsPanel.GetAdvancedSearchAvailableToolbarItemsNameList();

            backOfficePage = Browser.RefreshBackOfficePage();
            backOfficePage.WaitForPreviousActionComplete();
            backOfficePage.BackOfficeOptionsPanel.WaitForPanelLoaded();
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.AdvancedSearch);
            actualCheckedToolbarItems = backOfficePage.BackOfficeDetailsPanel.GetAdvancedSearchAvailableToolbarItemsNameList();
            VerifyEqual("11. Verify Changed value is remained", allCheckedToolbarItemsList, actualCheckedToolbarItems, false);

            desktopPage = Browser.NavigateToLoggedInCMS();
            advancedSearchPage = desktopPage.GoToApp(App.AdvancedSearch) as AdvancedSearchPage;
            advancedSearchPage.WaitForSearchWizardPopupPanelDisplayed();

            availableToolbarItems = advancedSearchPage.GridPanel.GetBackOfficeAvailableToolbarButtons();
            VerifyEqual("11. Verify all checked toolbar buttons present in the grid toolbar", allCheckedToolbarItemsList, availableToolbarItems, false);

            Info(string.Format("Reset Toolbar Items to {0}", string.Join(",", currentCheckedToolbarItemsList)));
            backOfficePage = Browser.NavigateToLoggedInBackOfficeApp();
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.AdvancedSearch);
            backOfficePage.BackOfficeDetailsPanel.UncheckAdvancedSearchAllToolbarItems();
            backOfficePage.BackOfficeDetailsPanel.CheckAdvancedSearchToolbarItems(currentCheckedToolbarItemsList.ToArray());
            backOfficePage.BackOfficeDetailsPanel.ClickSaveButton();
            backOfficePage.WaitForPreviousActionComplete();
        }

        #endregion //Test Cases

        #region Private methods        

        #region Verify methods

        #endregion //Verify methods

        #region Input XML data

        /// <summary>
        /// Read test data for BO_02_01
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, object> GetTestDataOfBO_02_01()
        {
            var testCaseName = "BO_02_01";
            var xmlUtility = new XmlUtility(Settings.BO_TEST_DATA_FILE_PATH);
            var testData = new Dictionary<string, object>();
            testData.Add("Apps", xmlUtility.GetChildNodesText(string.Format(Settings.BO_XPATH_PREFIX, testCaseName, "Apps")));
            return testData;
        }

        /// <summary>
        /// Read test data for BO_03_02
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfBO_03_02()
        {
            var testCaseName = "BO_03_02";
            var xmlUtility = new XmlUtility(Settings.BO_TEST_DATA_FILE_PATH);
            var testData = new Dictionary<string, string>();
            testData.Add("Timeout", xmlUtility.GetSingleNodeText(string.Format(Settings.BO_XPATH_PREFIX, testCaseName, "Timeout")));
            return testData;
        }        

        /// <summary>
        /// Read test data for BO_03_04
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfBO_03_04()
        {
            var testCaseName = "BO_03_04";
            var xmlUtility = new XmlUtility(Settings.BO_TEST_DATA_FILE_PATH);
            var testData = new Dictionary<string, string>();
            testData.Add("CsvSeparator", xmlUtility.GetSingleNodeText(string.Format(Settings.BO_XPATH_PREFIX, testCaseName, "CsvSeparator")));
            return testData;
        }

        /// <summary>
        /// Read test data for BO_03_05
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, object> GetTestDataOfBO_03_05()
        {
            var testCaseName = "BO_03_05";
            var xmlUtility = new XmlUtility(Settings.BO_TEST_DATA_FILE_PATH);
            var testData = new Dictionary<string, object>();
            testData.Add("Attributes", xmlUtility.GetChildNodesText(string.Format(Settings.BO_XPATH_PREFIX, testCaseName, "Attributes")));
            return testData;
        }

        /// <summary>
        /// Read test data for BO_04_02
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, object> GetTestDataOfBO_04_02()
        {
            var testCaseName = "BO_04_02";
            var xmlUtility = new XmlUtility(Settings.BO_TEST_DATA_FILE_PATH);
            var testData = new Dictionary<string, object>();
            testData.Add("Apps", xmlUtility.GetChildNodesText(string.Format(Settings.BO_XPATH_PREFIX, testCaseName, "Apps")));
            testData.Add("Widgets", xmlUtility.GetChildNodesText(string.Format(Settings.BO_XPATH_PREFIX, testCaseName, "Widgets")));
            return testData;
        }        

        /// <summary>
        /// Read test data for BO_05_02
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfBO_05_02()
        {
            var testCaseName = "BO_05_02";
            var xmlUtility = new XmlUtility(Settings.BO_TEST_DATA_FILE_PATH);
            var testData = new Dictionary<string, string>();
            testData.Add("Geozone", xmlUtility.GetSingleNodeText(string.Format(Settings.BO_XPATH_PREFIX, testCaseName, "Geozone")));

            return testData;
        }

        /// <summary>
        /// Read test data for BO_05_04
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfBO_05_04()
        {
            var testCaseName = "BO_05_04";
            var xmlUtility = new XmlUtility(Settings.BO_TEST_DATA_FILE_PATH);
            var testData = new Dictionary<string, string>();
            testData.Add("Geozone", xmlUtility.GetSingleNodeText(string.Format(Settings.BO_XPATH_PREFIX, testCaseName, "Geozone")));

            return testData;
        }

        /// <summary>
        /// Read test data for BO_05_05
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfBO_05_05()
        {
            var testCaseName = "BO_05_05";
            var xmlUtility = new XmlUtility(Settings.BO_TEST_DATA_FILE_PATH);
            var testData = new Dictionary<string, string>();
            testData.Add("Geozone", xmlUtility.GetSingleNodeText(string.Format(Settings.BO_XPATH_PREFIX, testCaseName, "Geozone")));

            return testData;
        }

        /// <summary>
        /// Read test data for BO_05_06
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfBO_05_06()
        {
            var testCaseName = "BO_05_06";
            var xmlUtility = new XmlUtility(Settings.BO_TEST_DATA_FILE_PATH);
            var testData = new Dictionary<string, string>();
            testData.Add("Geozone", xmlUtility.GetSingleNodeText(string.Format(Settings.BO_XPATH_PREFIX, testCaseName, "Geozone")));

            return testData;
        }

        /// <summary>
        /// Read test data for BO_05_07
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfBO_05_07()
        {
            var testCaseName = "BO_05_07";
            var xmlUtility = new XmlUtility(Settings.BO_TEST_DATA_FILE_PATH);
            var testData = new Dictionary<string, string>();
            testData.Add("RowsPerPage", xmlUtility.GetSingleNodeText(string.Format(Settings.BO_XPATH_PREFIX, testCaseName, "RowsPerPage")));

            return testData;
        }

        /// <summary>
        /// Read test data for BO_06_02
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfBO_06_02()
        {
            var testCaseName = "BO_06_02";
            var xmlUtility = new XmlUtility(Settings.BO_TEST_DATA_FILE_PATH);
            var testData = new Dictionary<string, string>();
            testData.Add("Geozone", xmlUtility.GetSingleNodeText(string.Format(Settings.BO_XPATH_PREFIX, testCaseName, "Geozone")));

            return testData;
        }

        /// <summary>
        /// Read test data for BO_07_02
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfBO_07_02()
        {
            var testCaseName = "BO_07_02";
            var xmlUtility = new XmlUtility(Settings.BO_TEST_DATA_FILE_PATH);
            var testData = new Dictionary<string, string>();
            testData.Add("Geozone", xmlUtility.GetSingleNodeText(string.Format(Settings.BO_XPATH_PREFIX, testCaseName, "Geozone")));

            return testData;
        }

        /// <summary>
        /// Read test data for BO_07_03
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfBO_07_03()
        {
            var testCaseName = "BO_07_03";
            var xmlUtility = new XmlUtility(Settings.BO_TEST_DATA_FILE_PATH);
            var testData = new Dictionary<string, string>();
            testData.Add("Geozone", xmlUtility.GetSingleNodeText(string.Format(Settings.BO_XPATH_PREFIX, testCaseName, "Geozone")));

            return testData;
        }

        /// <summary>
        /// Read test data for BO_07_04
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfBO_07_04()
        {
            var testCaseName = "BO_07_04";
            var xmlUtility = new XmlUtility(Settings.BO_TEST_DATA_FILE_PATH);
            var testData = new Dictionary<string, string>();
            testData.Add("Geozone", xmlUtility.GetSingleNodeText(string.Format(Settings.BO_XPATH_PREFIX, testCaseName, "Geozone")));
            testData.Add("RowsPerPage", xmlUtility.GetSingleNodeText(string.Format(Settings.BO_XPATH_PREFIX, testCaseName, "RowsPerPage")));

            return testData;
        }

        /// <summary>
        /// Read test data for BO_08_02
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfBO_08_02()
        {
            var testCaseName = "BO_08_02";
            var xmlUtility = new XmlUtility(Settings.BO_TEST_DATA_FILE_PATH);
            var testData = new Dictionary<string, string>();
            testData.Add("Geozone", xmlUtility.GetSingleNodeText(string.Format(Settings.BO_XPATH_PREFIX, testCaseName, "Geozone")));

            return testData;
        }

        /// <summary>
        /// Read test data for BO_08_03
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfBO_08_03()
        {
            var testCaseName = "BO_08_03";
            var xmlUtility = new XmlUtility(Settings.BO_TEST_DATA_FILE_PATH);
            var testData = new Dictionary<string, string>();
            testData.Add("Geozone", xmlUtility.GetSingleNodeText(string.Format(Settings.BO_XPATH_PREFIX, testCaseName, "Geozone")));

            return testData;
        }

        /// <summary>
        /// Read test data for BO_08_04
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfBO_08_04()
        {
            var testCaseName = "BO_08_04";
            var xmlUtility = new XmlUtility(Settings.BO_TEST_DATA_FILE_PATH);
            var testData = new Dictionary<string, string>();
            testData.Add("Geozone", xmlUtility.GetSingleNodeText(string.Format(Settings.BO_XPATH_PREFIX, testCaseName, "Geozone")));
            testData.Add("RowsPerPage", xmlUtility.GetSingleNodeText(string.Format(Settings.BO_XPATH_PREFIX, testCaseName, "RowsPerPage")));

            return testData;
        }

        /// <summary>
        /// Read test data for BO_09_02
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfBO_09_02()
        {
            var testCaseName = "BO_09_02";
            var xmlUtility = new XmlUtility(Settings.BO_TEST_DATA_FILE_PATH);
            var testData = new Dictionary<string, string>();
            testData.Add("Geozone", xmlUtility.GetSingleNodeText(string.Format(Settings.BO_XPATH_PREFIX, testCaseName, "Geozone")));

            return testData;
        }

        /// <summary>
        /// Read test data for BO_10_02
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfBO_10_02()
        {
            var testCaseName = "BO_10_02";
            var xmlUtility = new XmlUtility(Settings.BO_TEST_DATA_FILE_PATH);
            var testData = new Dictionary<string, string>();
            testData.Add("Geozone", xmlUtility.GetSingleNodeText(string.Format(Settings.BO_XPATH_PREFIX, testCaseName, "Geozone")));

            return testData;
        }

        /// <summary>
        /// Read test data for BO_11_02
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfBO_11_02()
        {
            var testCaseName = "BO_11_02";
            var xmlUtility = new XmlUtility(Settings.BO_TEST_DATA_FILE_PATH);
            var testData = new Dictionary<string, string>();
            testData.Add("Geozone", xmlUtility.GetSingleNodeText(string.Format(Settings.BO_XPATH_PREFIX, testCaseName, "Geozone")));

            return testData;
        }

        /// <summary>
        /// Read test data for BO_11_03
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfBO_11_03()
        {
            var testCaseName = "BO_11_03";
            var xmlUtility = new XmlUtility(Settings.BO_TEST_DATA_FILE_PATH);
            var testData = new Dictionary<string, string>();
            testData.Add("Geozone", xmlUtility.GetSingleNodeText(string.Format(Settings.BO_XPATH_PREFIX, testCaseName, "Geozone")));
            testData.Add("RowsPerPage", xmlUtility.GetSingleNodeText(string.Format(Settings.BO_XPATH_PREFIX, testCaseName, "RowsPerPage")));

            return testData;
        }

        #endregion //Input XML data

        #endregion //Private methods
    }
}