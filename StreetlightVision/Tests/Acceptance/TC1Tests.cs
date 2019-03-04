using Newtonsoft.Json.Linq;
using NUnit.Framework;
using StreetlightVision.Extensions;
using StreetlightVision.Models;
using StreetlightVision.Pages;
using StreetlightVision.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;


namespace StreetlightVision.Tests.Acceptance
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    public class TC1Tests : TestBase
    {
        #region Variables

        #endregion //Variables

        #region Contructors

        #endregion //Contructors        

        #region Test Cases

        [Test, DynamicRetry]
        [Description("TS 1.1.1 Login page languages")]
        public void TS1_01_01()
        {
            var testData = GetTestDataOfTestTS1_1_1();
            var english = testData["English"] as List<string>;
            var french = testData["French"] as List<string>;

            Step("1. Go to browser settings page");
            Step("2. Change displayed language to any language that SLV supports (English)");
            Step("3. Browse to SLV CMS page");
            var loginPage = Browser.OpenCMS();

            Step("4. Expected Login page is displayed in the languages set at step #2");
            loginPage.ClickForgotPasswordLink();
            loginPage.WaitForLoginForgotFormDisplayed();
            VerifyLoginPageLanguage(loginPage, english);

            if (Browser.Name.Equals("Chrome"))
            {
                Step("1. Go to browser settings page");
                Step("2. Change displayed language to any language that SLV supports (French)");
                WebDriverContext.RenewCurrentDriverWithNewLanguage("fr");

                Step("3. Browse to SLV CMS page");
                loginPage = Browser.OpenCMS();

                Step("4. Expected Login page is displayed in the languages set at step #2");
                loginPage.ClickForgotPasswordLink();
                loginPage.WaitForLoginForgotFormDisplayed();
                VerifyLoginPageLanguage(loginPage, french);
            }
        }

        [Test, DynamicRetry]
        [Description("TS 1.2.1 Login validation")]
        public void TS1_02_01()
        {
            Step("1. Go to SLV CMS");
            Step("2. Expected Login page is routed, message 'Authentication required!' is present");
            var loginPage = Browser.OpenCMS();

            Step("3. Leave User Name and Password field empty then click Arrow icon");
            loginPage.ClickLoginButton();
            Step("4. Expected Nothing happens: CMS doesn't submit login data, no message is displayed");
            var actualPassword = loginPage.GetPasswordValue();
            VerifyTrue("4. Verify password is empty", string.IsNullOrEmpty(actualPassword), string.Empty, actualPassword);

            Step("5. Leave User Name field empty and enter any value into Password field then click Arrow icon");
            var password = "Ga#erqe1";
            loginPage.ClickLoginButton();
            loginPage.LoginAsInvalidUserWithEmptyUsername(password);
            Step("6. Expected Nothing happens: CMS doesn't submit login data, no message is displayed");
            actualPassword = loginPage.GetPasswordValue();
            VerifyTrue(string.Format("6. Verify password is '{0}'", password), password.Equals(actualPassword), password, actualPassword);

            Step("7. Enter any value in User Name field and leave Password field empty then click Arrow icon");
            var username = "userTest1";
            loginPage.ClickLoginButton();
            loginPage.LoginAsInvalidUserWithEmptyPassword(username);
            Step("8. Expected Nothing happens: CMS doesn't submit login data, no message is displayed");
            actualPassword = loginPage.GetPasswordValue();
            VerifyTrue("8. Verify password is empty", string.IsNullOrEmpty(actualPassword), string.Empty, actualPassword);

            Step("9. Enter any value in User Name and Password field, providing that they are not the same user name and password of an existing user, then click Arrow icon");
            username = "userTest2";
            password = "anyPassword";
            loginPage.LoginAsInvalidUser(username, password);
            actualPassword = loginPage.GetPasswordValue();
            VerifyTrue("9. Verify password is empty", string.IsNullOrEmpty(actualPassword), string.Empty, actualPassword);
        } 

        [Test, DynamicRetry]
        [Description("TS 1.3.1 User's apps vs user's profile")]
        public void TS1_03_01()
        {
            var userModel = CreateNewProfileAndUser();
            var user = userModel.Username;
            var password = userModel.Password;
            var expectedProfileName = userModel.Profile;

            Step("1. Open SLV CMS");
            Step("2. Enter correct username and password");
            Step("3. Expected Login success");
            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(user, password);
            desktopPage.InstallAppsIfNotExist(App.Users);

            Step("4. Go to Users app");
            Step("5. Expected Users page is routed and loaded successfully");
            var usersPage = desktopPage.GoToApp(App.Users) as UsersPage;

            Step("6. Select the profile which the logged user belongs to");
            usersPage.UserProfileListPanel.SelectProfile(expectedProfileName);

            Step("7. Expected Profile Detail widget shows detailed settings of the selected profile");
            var actualProfileName = usersPage.UserProfileDetailsPanel.GetProfileNameValueText();
            VerifyEqual(string.Format("7. Verify '{0}' is displayed correctly", expectedProfileName), expectedProfileName, actualProfileName);

            Step("8. In Profile Detail widget, un/check some application then click Save icon");
            usersPage.UserProfileDetailsPanel.UncheckRandomApps(2);
            usersPage.UserProfileDetailsPanel.ClickSaveButton();
            usersPage.WaitForHeaderMessageDisplayed();
            var actualHeaderMessage = usersPage.GetHeaderMessage();
            Step("9. Expected Profile is updated successfully with message 'The user profile has been updated successfully'");
            VerifyEqual("9. Verify profile updated message successfully", "The user profile has been updated successfully", actualHeaderMessage);
            usersPage.WaitForHeaderMessageDisappeared();
            var blockedApps = usersPage.UserProfileDetailsPanel.GetDisabledAppsName();

            Step("10. Refresh browser");
            desktopPage = Browser.RefreshLoggedInCMS();

            Step("11. Verify Desktop page");
            Step("12. Expected The unchecked apps are not present in Desktop page");
            var installedAppsList = desktopPage.GetInstalledAppsName();
            var hasExisted = installedAppsList.Intersect(blockedApps).Any();
            VerifyTrue("12. Verify The unchecked apps are not present in Desktop page", !hasExisted, string.Format("Installed in Desktop: {0}", string.Join(",", installedAppsList)), string.Format("Un-checked Apps: {0}", string.Join(",", blockedApps)));

            Step("13. Click Settings icon then click Store in Settings menu");
            Step("14. Expected The unchecked apps are disabled in Applications tab");
            desktopPage.AppBar.ClickSettingsButton();
            desktopPage.SettingsPanel.ClickStoreLink();
            var listStoreDisableApps = desktopPage.SettingsPanel.StorePanel.GetDisabledAppsName();

            var listIntersect = listStoreDisableApps.Intersect(blockedApps).ToList();
            var expectedUncheckedAppCount = blockedApps.Count;
            var actualIntersectCount = listIntersect.Count;
            VerifyTrue("14. Verify The unchecked apps are disabled in Applications tab", expectedUncheckedAppCount == actualIntersectCount, string.Format("Un-checked Apps: {0}", string.Join(",", blockedApps)), string.Format("Disabled apps intersected: {0}", string.Join(",", listIntersect)));

            try
            {
                DeleteUserAndProfile(userModel);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("TS 1.4.1 About panel")]
        public void TS1_04_01()
        {
            var testData = GetTestDataOfTestTS1_4_1();
            var expectedAboutTitle = testData["Title"];
            var expectedVersion = testData["Version"];
            var expectedCopyright = testData["Copyright"];
            expectedCopyright = string.Format(expectedCopyright, DateTime.Now.Year);

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);

            Step("1. Click Settings icon on the top right corner");
            desktopPage.AppBar.ClickSettingsButton();

            Step("2. Expected Settings menu is shown");
            Step("3. Click About");
            desktopPage.SettingsPanel.ClickAboutLink();

            Step("4. Expected About panel is shown with information relating to SLV CMS (no need to check build number)");
            desktopPage.SettingsPanel.WaitForAboutPanelDisplayed();

            var panelTitle = desktopPage.SettingsPanel.AboutPanel.GetPanelHeaderText();
            var actualAboutTitle = desktopPage.SettingsPanel.AboutPanel.GetSlvTitleText();
            var actualAboutVersion = desktopPage.SettingsPanel.AboutPanel.GetSlvVersionText();
            var actualAboutCopyright = desktopPage.SettingsPanel.AboutPanel.GetSlvCopyrightText();

            VerifyEqual("4. Verify Panel Text is 'About'", "About", panelTitle);
            VerifyEqual(string.Format("4. Verify About Title is '{0}'", expectedAboutTitle), expectedAboutTitle, actualAboutTitle);
            VerifyTrue(string.Format("4. Verify About Version is '{0}'", expectedVersion), actualAboutVersion.StartsWith(expectedVersion), expectedVersion, actualAboutVersion);
            VerifyEqual(string.Format("4. Verify About Copyright is '{0}'", expectedCopyright), expectedCopyright, actualAboutCopyright);

            Step("5. Click Arrow icon on About panel");
            desktopPage.SettingsPanel.AboutPanel.ClickBackButton();

            Step("6. Expected About panel is closed. Settings menu now appears again");
            desktopPage.SettingsPanel.WaitForAboutPanelDisappeared();
            desktopPage.WaitForSettingsPanelDisplayed();
        }  

        [Test, DynamicRetry]
        [Description("TS 1.6.1 Move apps and widgets on Desktop")]
        [NonParallelizable]
        public void TS1_06_01()
        {
            var userModel = CreateNewProfileAndUser();
            const int NUMBER_DRAG_DROP_TIMES = 2;
            var oldLocations = new List<int>();
            var newLocations = new List<int>();

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(userModel.Username, userModel.Password);   
            var requiredAppsArray = desktopPage.GetRandomApps(NUMBER_DRAG_DROP_TIMES * 2).ToArray();
            desktopPage.InstallAppsIfNotExist(requiredAppsArray);

            for (var i = 0; i < requiredAppsArray.Length; i += 2)
            {
                var sourceApp = requiredAppsArray[i];
                var destApp = requiredAppsArray[i + 1];

                var currentDeskptopTiles = SLVHelper.GetListOfDesktopTilesName();
                var oldSourceLocation = currentDeskptopTiles.IndexOf(sourceApp);
                var oldDestLocation = currentDeskptopTiles.IndexOf(destApp);

                oldLocations.Add(oldSourceLocation);
                oldLocations.Add(oldDestLocation);

                Step("1. In Desktop page, click Customize icon in the left bottom");
                desktopPage.ClickFooterCustomizeButton();
                desktopPage.WaitForConfigToolbarDisplayed();

                Step("2. Expected");
                Step(" - Desktop page turns into Design mode");
                Step(" - 'Delete WebApps' icon appears");
                var actualDeleteCaption = desktopPage.GetFooterDeleteCaptionText();
                VerifyEqual("2. Verify 'Delete WebApps' icon appears", "Delete WebApps", actualDeleteCaption);

                Step("3. Drag and drop any application to new position");
                Step(string.Format("-> Drag and drop '{0}' to '{1}'", sourceApp, destApp));
                desktopPage.DragAndDrop(sourceApp, destApp);
                desktopPage.ClickFooterCloseButton();
                desktopPage.WaitForConfigToolbarDisappeared();

                Step("4. Expected The application under test is moved to correct position");
                currentDeskptopTiles = SLVHelper.GetListOfDesktopTilesName();
                var newSourceLocation = currentDeskptopTiles.IndexOf(sourceApp);
                var newDestLocation = currentDeskptopTiles.IndexOf(destApp);

                newLocations.Add(newSourceLocation);
                newLocations.Add(newDestLocation);

                VerifyTrue(string.Format("4. Verify application {0} is moved to correct position", sourceApp), newSourceLocation != oldSourceLocation, oldSourceLocation, newSourceLocation);
                VerifyTrue(string.Format("4. Verify application {0} is moved to correct position", destApp), newDestLocation != oldDestLocation, oldDestLocation, newDestLocation);
            }

            Step("5. Expected The application under test is moved to correct position");
            desktopPage = Browser.RefreshLoggedInCMS();

            Step("6. Expected Applications and widgets are placed in positions as they were before refreshing browser");
            for (var i = 0; i < oldLocations.Count; i++)
            {
                VerifyTrue(string.Format("6. Verify application {0} is moved to correct position", requiredAppsArray[i]), newLocations[i] != oldLocations[i], oldLocations[i], newLocations[i]);
            }

            try
            {
                DeleteUserAndProfile(userModel);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("TS 1.7.1 User's skins and languages")]
        public void TS1_07_01()
        {
            var userModel = CreateNewProfileAndUser();
            var testData = GetTestDataOfTestTS1_7_1();
            var skins = testData["Skins"] as List<string>;
            var languageCodes = testData["LanguageCodes"] as List<string>;

            var user = userModel.Username;
            var password = userModel.Password;
            var expectedProfileName = userModel.Profile;
            var expectedMessage = string.Empty;
            var expectedSkin = string.Empty;
            var expectedLanguageCode = string.Empty;
            var expectedLanguage = string.Empty;

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(user, password);
            expectedMessage = GetProfileUpdatedMessage(desktopPage.GetCurrentLanguageCode());

            Step("1. Go to Users app");
            Step("2. Expected Users page is routed and loaded successfully");
            var usersPage = desktopPage.GoToApp(App.Users) as UsersPage;

            Step("3. Select the profile which the logged user belongs to");
            usersPage.UserProfileListPanel.SelectProfile(expectedProfileName);

            for (int i = 0; i < skins.Count; i++)
            {
                expectedSkin = skins[i];
                expectedLanguageCode = languageCodes[i];
                expectedLanguage = GetLanguageName(expectedLanguageCode);

                Step("4. Change to new skin and language");
                Step("5. Expected The skin and language are selectable");
                usersPage.UserProfileDetailsPanel.SelectLanguageDropDown(expectedLanguage);
                usersPage.UserProfileDetailsPanel.SelectSkinDropDown(expectedSkin);

                Step("6. Click Save");
                usersPage.UserProfileDetailsPanel.ClickSaveButton();
                usersPage.WaitForHeaderMessageDisplayed();
                var actualHeaderMessage = usersPage.GetHeaderMessage();

                Step("7. Expected Profile is updated successfully with message 'The user profile has been updated successfully'");
                VerifyEqual("Verify profile updated message successfully", expectedMessage, actualHeaderMessage);
                usersPage.WaitForHeaderMessageDisappeared();
                usersPage.WaitForSkinAndLanguageApplied();

                Step("8. Refresh browser");
                desktopPage = Browser.RefreshLoggedInCMS();

                Step("9. Expected");
                Step(" - SLV CMS is in new skin (Verify by checking presence of link element and its href attribute contains skin name, e.g. < link type='text / css' rel='stylesheet' media='screen' href='http://5.196.91.118:8080/qa/groundcontrol/skins/streetdark/css/style.min.css' >)");
                Step(" - SLV CMS is in new language (Verify by checking presence of link element and its href attribute contains skin name, e.g. < script type='text / javascript' src='http://5.196.91.118:8080/qa/groundcontrol/js/lib/JQuery/globalize/cultures/globalize.culture.en-US.js'> )");
                var actualSkin = desktopPage.GetCurrentSkin();
                var actualLanguage = desktopPage.GetCurrentLanguageCode();
                VerifyEqual(string.Format("Verify SLV CMS is in '{0}' skin", expectedSkin), expectedSkin, actualSkin);
                VerifyEqual(string.Format("Verify SLV CMS is in '{0}' language", expectedLanguage), expectedLanguageCode, actualLanguage);

                Step("10. Go to Users app again and select the profile at step #3");
                usersPage = desktopPage.GoToApp(App.Users) as UsersPage;
                usersPage.UserProfileListPanel.SelectProfile(expectedProfileName);

                Step("11. Expected Selected skin and language are the skin and languages selected before refreshing");
                var selectedSkin = usersPage.UserProfileDetailsPanel.GetSkinValue();
                var selectedLanguage = usersPage.UserProfileDetailsPanel.GetLanguageValue();
                VerifyEqual(string.Format("Verify selected skin '{0}' is the skin selected before refreshing", expectedSkin), expectedSkin, selectedSkin);
                VerifyEqual(string.Format("Verify selected language '{0}' is the language selected before refreshing", expectedLanguage), expectedLanguage, selectedLanguage);

                Step("12. Repeat the test with all skins and languages");
                expectedMessage = GetProfileUpdatedMessage(expectedLanguageCode);
            }

            try
            {
                DeleteUserAndProfile(userModel);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("TS 1.8.1 Change password")]
        public void TS1_08_01()
        {
            var userModel = CreateNewProfileAndUser();
            var user = userModel.Username;
            var password = userModel.Password;
            var userFullName = userModel.FullName;
            var userProfile = userModel.Profile;
            var newPassword = "Password0~1";

            var expectedTitle = "Change my password";
            var expectedPasswordCannotBeEmty = "Your new password cannot be empty";
            var expectedPasswordIsIncorrect = "Current password is incorrect";
            var expectedConfirmationPasswordIsDifferent = "A different password has been typed for confirmation";
            var expectedPasswordIsNotComplex = "The complexity of your password is not sufficient. Please make sure it contains at least one uppercase, one lowercase, one number and one special character.";

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(user, password);

            Step("1. Click Settings icon on the top right corner");
            Step("2. Expected Settings menu is shown");
            desktopPage.AppBar.ClickSettingsButton();

            Step("3. Click 'Change my password'");
            desktopPage.SettingsPanel.ClickChangeMyPasswordLink();

            Step("4. Expected 'Change my password' panel is shown");
            desktopPage.SettingsPanel.WaitForChangeMyPasswordPanelDisplayed();
            VerifyEqual(string.Format("Verify Panel Text is {0}", expectedTitle), expectedTitle, desktopPage.SettingsPanel.ChangeMyPasswordPanel.GetPanelHeaderText());

            Step("5. Leave all fields empty then click 'Change password' button");
            Step("6. Expected Message 'Your new password cannot be empty' is displayed");
            desktopPage.SettingsPanel.ChangeMyPasswordPanel.ClickChangePasswordButton();
            VerifyEqual(string.Format("6. Verify message '{0}' is correct", expectedPasswordCannotBeEmty), expectedPasswordCannotBeEmty, desktopPage.SettingsPanel.ChangeMyPasswordPanel.GetErrorMessageText());

            Step("7. Enter anything into 'Current password' field, leave 'New password' and 'Confirm new password' fields empty then click 'Change password' button");
            Step("8. Expected Message 'Your new password cannot be empty' is displayed");
            desktopPage.SettingsPanel.ChangeMyPasswordPanel.EnterCurrentPasswordInput("@nyk3ys");
            desktopPage.SettingsPanel.ChangeMyPasswordPanel.ClickChangePasswordButton();
            VerifyEqual(string.Format("8. Verify message '{0}' is correct", expectedPasswordCannotBeEmty), expectedPasswordCannotBeEmty, desktopPage.SettingsPanel.ChangeMyPasswordPanel.GetErrorMessageText());

            Step("9. Enter anything into 'Current password' and 'New password' fields, leave 'Confirm new password' field empty then click 'Change password' button");
            Step("10. Expected Message 'Your new password cannot be empty' is displayed");
            desktopPage.SettingsPanel.ChangeMyPasswordPanel.EnterCurrentPasswordInput("@nyk1ys");
            desktopPage.SettingsPanel.ChangeMyPasswordPanel.EnterNewPasswordInput("1nyk2ys#");
            desktopPage.SettingsPanel.ChangeMyPasswordPanel.ClickChangePasswordButton();
            VerifyEqual(string.Format("10. Verify message '{0}' is correct", expectedPasswordCannotBeEmty), expectedPasswordCannotBeEmty, desktopPage.SettingsPanel.ChangeMyPasswordPanel.GetErrorMessageText());

            Step("11. Fill 'Current password' field with incorrect password, enter anything into 'New password' and 'Confirm new password' fields(the same value) then click 'Change password' button");
            Step("12. Expected Message 'Current password is incorrect' is displayed");
            desktopPage.SettingsPanel.ChangeMyPasswordPanel.EnterCurrentPasswordInput("@abc1ys");
            desktopPage.SettingsPanel.ChangeMyPasswordPanel.EnterNewPasswordInput("1nyK2ys#");
            desktopPage.SettingsPanel.ChangeMyPasswordPanel.EnterNewPasswordConfirmedInput("1nyK2ys#");
            desktopPage.SettingsPanel.ChangeMyPasswordPanel.ClickChangePasswordButton();
            desktopPage.SettingsPanel.ChangeMyPasswordPanel.WaitForPreviousActionComplete();
            VerifyEqual(string.Format("12. Verify message '{0}' is correct", expectedPasswordIsIncorrect), expectedPasswordIsIncorrect, desktopPage.SettingsPanel.ChangeMyPasswordPanel.GetErrorMessageText());

            Step("13. Fill 'Current password' field with correct password, fill 'New password' and 'Confirm new password' fields with different values then click 'Change password' button");
            Step("14. Expected Message 'A different password has been typed for confirmation' is shown");
            desktopPage.SettingsPanel.ChangeMyPasswordPanel.EnterCurrentPasswordInput(password);
            desktopPage.SettingsPanel.ChangeMyPasswordPanel.EnterNewPasswordInput("1nyK2ys#");
            desktopPage.SettingsPanel.ChangeMyPasswordPanel.EnterNewPasswordConfirmedInput("3nYk2ys!");
            desktopPage.SettingsPanel.ChangeMyPasswordPanel.ClickChangePasswordButton();
            desktopPage.SettingsPanel.ChangeMyPasswordPanel.WaitForPreviousActionComplete();
            VerifyEqual(string.Format("14. Verify message '{0}' is correct", expectedConfirmationPasswordIsDifferent), expectedConfirmationPasswordIsDifferent, desktopPage.SettingsPanel.ChangeMyPasswordPanel.GetErrorMessageText());

            Step("15. Fill 'Current password' field with correct password, fill 'New password' and 'Confirm new password' fields with the same value but invalid then click 'Change password' button. (A valid password contains at least one uppercase, one lowercase, one number and one special character)");
            Step("16. Expected Message 'The complexity of your password is not sufficient.Please make sure it contains at least one uppercase, one lowercase, one number and one special character.' is shown");
            desktopPage.SettingsPanel.ChangeMyPasswordPanel.EnterCurrentPasswordInput(password);
            desktopPage.SettingsPanel.ChangeMyPasswordPanel.EnterNewPasswordInput("abcd1234");
            desktopPage.SettingsPanel.ChangeMyPasswordPanel.EnterNewPasswordConfirmedInput("abcd1234");
            desktopPage.SettingsPanel.ChangeMyPasswordPanel.ClickChangePasswordButton();
            desktopPage.SettingsPanel.ChangeMyPasswordPanel.WaitForPreviousActionComplete();
            VerifyEqual(string.Format("16. Verify message '{0}' is correct", expectedPasswordIsNotComplex), expectedPasswordIsNotComplex, desktopPage.SettingsPanel.ChangeMyPasswordPanel.GetErrorMessageText());

            Step("17. Enter correct password in 'Current password' field");
            Step("18. Enter the same valid password in 'New password' and 'Confirm new password' fields. (A valid password contains at least one uppercase, one lowercase, one number and one special character)");
            desktopPage.SettingsPanel.ChangeMyPasswordPanel.EnterCurrentPasswordInput(password);
            desktopPage.SettingsPanel.ChangeMyPasswordPanel.EnterNewPasswordInput(newPassword);
            desktopPage.SettingsPanel.ChangeMyPasswordPanel.EnterNewPasswordConfirmedInput(newPassword);

            Step("19. Click 'Change password' button");
            Step("20. Expected Preload appears then disappears after password changed. Settings menu is closed");
            desktopPage.SettingsPanel.ChangeMyPasswordPanel.ClickChangePasswordButton();
            desktopPage.WaitForSettingsPanelDisappeared();

            Step("21. Log out");
            desktopPage.AppBar.ClickSettingsButton();
            desktopPage.WaitForSettingsPanelDisplayed();
            var logoutPage = desktopPage.SettingsPanel.ClickLogoutLink();

            Step("22. Log in this user with new password");
            Step("23. Expected Login is successfully");
            loginPage = Browser.OpenCMS();
            desktopPage = loginPage.LoginAsValidUser(user, newPassword);

            try
            {
                DeleteUserAndProfile(userModel);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("TS 1.9.1 Forgot password")]
        public void TS1_09_01()
        {
            var userModel = CreateNewProfileAndUser();
            var user = userModel.Username;
            var password = userModel.Password;
            var userFullName = userModel.FullName;
            var userProfile = userModel.Profile;

            var expectedForgetMsgNotFound = "User '{0}' not found!";
            var expectedForgetMsgMissing = "Missing login parameter!";
            var subjectResetPassword = "Password Reset Process";
            var subjectNewPassword = "New password";
            var expectedForgetMsgSentSuccess = "An e-mail with instructions has been sent to the user";
            var expectedResetPasswordMsgSuccess = "Your password has been reset and sent to you by email. Please check your incoming email and use the link to reset your password.";

            Step("1. Go to SLV CMS page");
            Step("2. Expected Login page is routed");
            var loginPage = Browser.OpenCMS();

            Step("3. Click Forgot password? button");
            loginPage.ClickForgotPasswordLink();

            Step("4. Expected An input field for user name (so called Name) is showned below 'Forgot password?' button and an Arrow button");
            loginPage.WaitForLoginForgotFormDisplayed();

            Step("5. Leave Name field empty then click Arrow button next to Name field");
            Step("6. Expected Nothing happens: no validation message, no request sent to server");
            loginPage.ClickForgotPasswordButton();
            VerifyEqual("6. Verify No validation message, no request sent to server", false, loginPage.IsForgotMessageDisplayed());

            Step("7. Enter only spaces");
            loginPage.EnterForgotUsernameInput("  ");
            loginPage.WaitForForgotPasswordButtonEnabled();
            loginPage.ClickForgotPasswordButton();
            loginPage.WaitForForgotMessageDisplayed();

            Step("8. Expected 'Missing login parameters!' message is displayed");
            VerifyEqual(string.Format("[SC-1865] 8. Verify message '{0}' is correct", expectedForgetMsgMissing), expectedForgetMsgMissing, loginPage.GetForgotMessageText());
            loginPage.WaitForForgotMessageDisappeared();

            Step("9. Enter inexisting user name");
            var userTest = "userTest3";
            var expectedForgetMsgNotFoundCase1 = string.Format(expectedForgetMsgNotFound, userTest);
            loginPage.ClickForgotPasswordLink();
            loginPage.WaitForLoginForgotFormDisplayed();
            loginPage.EnterForgotUsernameInput(userTest);
            loginPage.WaitForForgotPasswordButtonEnabled();
            loginPage.ClickForgotPasswordButton();
            loginPage.WaitForForgotMessageDisplayed();

            Step("10. Expected { { User name} } not found!");
            VerifyEqual(string.Format("[SC-1865] 10. Verify message '{0}' is correct", expectedForgetMsgNotFoundCase1), expectedForgetMsgNotFoundCase1, loginPage.GetForgotMessageText());
            loginPage.WaitForForgotMessageDisappeared();

            Step("11. Enter special characters such as <>!@, etc.");
            userTest = "user!@<>";
            var expectedForgetMsgNotFoundCase2 = string.Format(expectedForgetMsgNotFound, userTest);
            loginPage.ClickForgotPasswordLink();
            loginPage.WaitForLoginForgotFormDisplayed();
            loginPage.EnterForgotUsernameInput(userTest);
            loginPage.WaitForForgotPasswordButtonEnabled();
            loginPage.ClickForgotPasswordButton();
            loginPage.WaitForForgotMessageDisplayed();

            Step("12. Expected { { User name} } not found!");
            VerifyEqual(string.Format("[SC-1865] 12. Verify message '{0}' is correct", expectedForgetMsgNotFoundCase2), expectedForgetMsgNotFoundCase2, loginPage.GetForgotMessageText());
            loginPage.WaitForForgotMessageDisappeared();

            Step("13. Enter user name into Name field then click Arrow button next to Name field");
            loginPage.ClickForgotPasswordLink();
            loginPage.WaitForLoginForgotFormDisplayed();
            loginPage.EnterForgotUsernameInput(user);
            loginPage.WaitForForgotPasswordButtonEnabled();

            //Clear inbox of test mail
            EmailUtility.CleanInbox(subjectResetPassword);
            loginPage.ClickForgotPasswordButton();
            loginPage.WaitForForgotMessageDisplayed();

            Step("14. Expected Message 'An e - mail with instructions has been sent to the user' is displayed");
            VerifyEqual(string.Format("[SC-1865] 14. Verify message '{0}' is correct", expectedForgetMsgSentSuccess), expectedForgetMsgSentSuccess, loginPage.GetForgotMessageText());
            loginPage.WaitForForgotMessageDisappeared();

            //Waiting and get reset password link email.
            Step("15. Check email's inbox of the user");
            var link = EmailUtility.GetSLVResetPasswordLink(subjectResetPassword);

            if (!string.IsNullOrEmpty(link))
            {
                //Clear inbox of test mail
                EmailUtility.CleanInbox(subjectNewPassword);

                Step("16. Expected There is an email sent by SLV CMS to instruct you to reset password:");
                Step(" - From: alarming.asia @streetlightmonitoring.com");
                Step(" - To: user's email address");
                Step(" - Subject: Password Reset Process");
                Step(" - Content: Click on the following link to reset the password for user '{0}':Reset Password. ('Reset Password' is displayed as a link that leads to reset password url - the url looks like http://5.196.91.118:8080/reports/public/api/publicconfig/resetPassword?securityKey=a529a825b8e6417c140ef7a6eb8b4ec15b466a1b", user);

                Step("17. Click Reset Password link in the sent email");
                var resetPasswordPage = loginPage.NavigateToResetPasswordLink(link);

                Step("18. Browser launches reset password url then message 'Your password has been reset and sent to you by email. Please check your incoming email and use the link to reset your password.' is displayed");
                var resetPasswordMessage = resetPasswordPage.GetResetPasswordMessageText();
                VerifyEqual(string.Format("18. Verify message '{0}' is correct", expectedResetPasswordMsgSuccess), expectedResetPasswordMsgSuccess, resetPasswordMessage);

                //Waiting and get new password email
                Step("19. Check mailbox again");
                string newPassword = EmailUtility.GetSLVNewPassword(subjectNewPassword);
                if (!string.IsNullOrEmpty(newPassword))
                {
                    Step("20. Expected There is another email sent by SLV CMS which contains new password.");
                    Step("21. Go to SLV CMS page");
                    loginPage = Browser.OpenCMS();

                    Step("22. Log in the user with new password sent in the email");
                    var desktopPage = loginPage.LoginAsValidUser(user, newPassword);

                    Step("23. Expected Login is successful");
                    VerifyEqual("23. Verify Username is displayed correctly", userFullName, desktopPage.GetUserFullNameText());
                    VerifyEqual("23. Verify User Group is displayed correctly", userProfile, desktopPage.GetUserProfileNameText());                   
                }
                else
                {
                    VerifyTrue("19. There is NO email sent by SLV CMS which contains new password", false, "Has new password email", "No new password email");
                }
            }
            else
            {
                VerifyTrue("15. There is NO email sent by SLV CMS to instruct you to reset password", false, "Has reset password email", "No reset password email");
            }

            try
            {
                DeleteUserAndProfile(userModel);
            }
            catch { }
        }      

        [Test, DynamicRetry]
        [Description("TS 1.10.1 Blocked Actions - Commission a controller")]
        [NonParallelizable]
        public void TS1_10_01()
        {
            var testData = GetTestDataOfTestTS1_10_1();
            var geozone = SLVHelper.GenerateUniqueName("GZNTS11001");
            var controller = SLVHelper.GenerateUniqueName("CTRL");
            var expectedBlockAction = testData["BlockAction"];

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - A device controller (called DevCtrl A) which has 'Control technology' property is one of following values:");
            Step("   + Silver Spring IPv6");
            Step("   + Open Smart City Protocol (Telematics, Bouygues Citybox, Dazzletek, Thorn)");
            Step("**** Precondition ****\n");

            Step("-> Create data for testing");
            DeleteGeozones("GZNTS11001*");
            CreateNewGeozone(geozone);
            CreateNewController(controller, geozone);
            var userModel = CreateNewProfileAndUser();

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(userModel.Username, userModel.Password);
            desktopPage.InstallAppsIfNotExist(App.Users, App.EquipmentInventory);           

            Step("1. In Desktop page, click Users tile");
            Step("2. Expected Users page is routed");
            var usersPage = desktopPage.GoToApp(App.Users) as UsersPage;

            Step("3. Select the user profile which User A belongs to");
            usersPage.UserProfileListPanel.SelectProfile(userModel.Profile);

            Step("4. In Profile Detail widget, remove all 'Block Actions' items if any");
            usersPage.UserProfileDetailsPanel.RemoveAllBlockedActions();

            Step("5. Expected All blocked actions are removed off");
            var blockedActionsCount = usersPage.UserProfileDetailsPanel.GetBlockedActionsCount();
            VerifyEqual("5. Verify blocked actions are removed completely", 0, blockedActionsCount);

            Step("6. Add 'Commission a controller' blocked action then click Save icon");
            usersPage.UserProfileDetailsPanel.SelectBlockActionsDropDown(expectedBlockAction);
            usersPage.UserProfileDetailsPanel.ClickSaveButton();
            usersPage.WaitForHeaderMessageDisplayed();
            var actualHeaderMessage = usersPage.GetHeaderMessage();

            Step("7. Expected Profile is updated successfully with message 'The user profile has been updated successfully'");
            VerifyEqual("7. Verify profile updated message successfully", "The user profile has been updated successfully", actualHeaderMessage);
            usersPage.WaitForHeaderMessageDisappeared();

            Step("8. Refresh browser");
            desktopPage = Browser.RefreshLoggedInCMS();

            Step("9. Go to Users app and select that profile again");
            usersPage = desktopPage.GoToApp(App.Users) as UsersPage;

            Step("10. Expected Saved blocked actions are present in the selected blocked actions");
            usersPage.UserProfileListPanel.SelectProfile(userModel.Profile);
            var currentBlockedActions = usersPage.UserProfileDetailsPanel.GetBlockedActionsName();
            VerifyTrue(string.Format("10. Verify saved blocked action '{0}' is present in the selected blocked actions", expectedBlockAction), currentBlockedActions.Any(e => e.Equals(expectedBlockAction)), "present", "Not present");

            Step("11. Go to 'Equipement Inventory' page");
            var equipmentInventoryPage = usersPage.AppBar.SwitchTo(App.EquipmentInventory) as EquipmentInventoryPage;

            Step("12. From 'Equipement Inventory' page, browse to testing controller");
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(string.Format(@"{0}\{1}", geozone, controller));

            Step("13. Expected Commission icon on Detail widget is not available any more");
            var isCommissionButtonDisplayed = equipmentInventoryPage.ControllerEditorPanel.IsCommissionButtonDisplayed();
            VerifyEqual("[#1429499] 13. Verify Commission button is not available any more", false, isCommissionButtonDisplayed);

            try
            {
                DeleteUserAndProfile(userModel);
                DeleteGeozone(geozone);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("TS 1.10.2 Blocked Actions - Delete a device")]
        [NonParallelizable]
        public void TS1_10_02()
        {
            var testData = GetTestDataOfTestTS1_10_2();
            var expectedBlockAction = testData["BlockAction"];
            var geozone = SLVHelper.GenerateUniqueName("GZNTS11002");
            var controller = SLVHelper.GenerateUniqueName("CTRL");
            var streetlight = SLVHelper.GenerateUniqueName("STL");
            var devices = new List<string> { controller, streetlight };
            var device = devices.PickRandom();            

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - User has not any blocked action");
            Step("**** Precondition ****\n");

            Step("-> Create data for testing");
            DeleteGeozones("GZNTS11002*");
            CreateNewGeozone(geozone);
            CreateNewController(controller, geozone);
            CreateNewDevice(DeviceType.Streetlight, streetlight, controller, geozone);
            var userModel = CreateNewProfileAndUser();

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(userModel.Username, userModel.Password);
            desktopPage.InstallAppsIfNotExist(App.Users, App.EquipmentInventory);           

            Step("1. In Desktop page, click Users tile");
            Step("2. Expected Users page is routed");
            var usersPage = desktopPage.GoToApp(App.Users) as UsersPage;

            Step("3. Select the user profile which User A belongs to");
            usersPage.UserProfileListPanel.SelectProfile(userModel.Profile);

            Step("4. In Profile Detail widget, remove all 'Block Actions' items if any");
            usersPage.UserProfileDetailsPanel.RemoveAllBlockedActions();

            Step("5. Expected All blocked actions are removed off");
            var blockedActionsCount = usersPage.UserProfileDetailsPanel.GetBlockedActionsCount();
            VerifyEqual("5. Verify blocked actions are removed completely", 0, blockedActionsCount);

            Step("6. Add 'Delete a device' blocked action then click Save icon");
            usersPage.UserProfileDetailsPanel.SelectBlockActionsDropDown(expectedBlockAction);
            usersPage.UserProfileDetailsPanel.ClickSaveButton();
            usersPage.WaitForHeaderMessageDisplayed();
            var actualHeaderMessage = usersPage.GetHeaderMessage();

            Step("7. Expected Profile is updated successfully with message 'The user profile has been updated successfully'");
            VerifyEqual("7. Verify profile updated message successfully", "The user profile has been updated successfully", actualHeaderMessage);
            usersPage.WaitForHeaderMessageDisappeared();

            Step("8. Refresh browser");
            desktopPage = Browser.RefreshLoggedInCMS();

            Step("9. Go to Users app and select that profile again");
            usersPage = desktopPage.GoToApp(App.Users) as UsersPage;

            Step("10. Expected Saved blocked actions are present in the selected blocked actions");
            usersPage.UserProfileListPanel.SelectProfile(userModel.Profile);
            var currentBlockedActions = usersPage.UserProfileDetailsPanel.GetBlockedActionsName();
            VerifyTrue(string.Format("10. Verify saved blocked action '{0}' is present in the selected blocked actions", expectedBlockAction), currentBlockedActions.Any(e => e.Equals(expectedBlockAction)), "present", "Not present");

            Step("11. Go to 'Equipement Inventory' page");
            var equipmentInventoryPage = usersPage.AppBar.SwitchTo(App.EquipmentInventory) as EquipmentInventoryPage;

            Step("12. From 'Equipement Inventory' page, browser to and select any device");
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(string.Format(@"{0}\{1}", geozone, device));

            Step("13. Expected Detail widget of the selected is displayed without 'Delete' button");
            VerifyEqual("13. Verify Detail widget of the selected is displayed without 'Delete' button", true, !equipmentInventoryPage.DeviceEditorPanel.IsDeleteButtonDisplayed());

            Step("14. Go back to Users app and select the testing User profile");
            usersPage = equipmentInventoryPage.AppBar.SwitchTo(App.Users) as UsersPage;
            usersPage.UserProfileListPanel.SelectProfile(userModel.Profile);

            Step("15. In Profile Detail widget, remove all 'Block Actions' items if any");
            usersPage.UserProfileDetailsPanel.RemoveAllBlockedActions();
            usersPage.UserProfileDetailsPanel.ClickSaveButton();
            usersPage.WaitForPreviousActionComplete();
            usersPage.WaitForHeaderMessageDisappeared();

            Step("16. Refresh browser");
            desktopPage = Browser.RefreshLoggedInCMS();

            Step("17. Go to Users app and select that profile again");
            usersPage = desktopPage.GoToApp(App.Users) as UsersPage;
            usersPage.UserProfileListPanel.SelectProfile(userModel.Profile);

            Step("18. Expected No blocked actions are present in the selected blocked actions");           
            currentBlockedActions = usersPage.UserProfileDetailsPanel.GetBlockedActionsName();
            VerifyTrue("18. Verify No blocked actions are present in the selected blocked actions", !currentBlockedActions.Any(), "No block actions", string.Join(",", currentBlockedActions));

            Step("19. Go to 'Equipement Inventory' page");
            equipmentInventoryPage = usersPage.AppBar.SwitchTo(App.EquipmentInventory) as EquipmentInventoryPage;

            Step("20. From 'Equipement Inventory' page, browser to and select any device");
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(string.Format(@"{0}\{1}", geozone, device));

            Step("21. Expected Detail widget of the selected is displayed with 'Delete' button");
            VerifyEqual("21. Verify Detail widget of the selected is displayed with 'Delete' button", true, equipmentInventoryPage.DeviceEditorPanel.IsDeleteButtonDisplayed());            

            try
            {
                DeleteUserAndProfile(userModel);
                DeleteGeozone(geozone);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("TS 1.10.3 Blocked Actions - Delete a geozone")]
        [NonParallelizable]
        public void TS1_10_03()
        {
            var testData = GetTestDataOfTestTS1_10_3();
            var geozone = SLVHelper.GenerateUniqueName("GZNTS11003");
            var expectedBlockAction = testData["BlockAction"];

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - User has not any blocked action");
            Step("**** Precondition ****\n");

            Step("-> Create data for testing");
            DeleteGeozones("GZNTS11003*");
            CreateNewGeozone(geozone);
            var userModel = CreateNewProfileAndUser();

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(userModel.Username, userModel.Password);
            desktopPage.InstallAppsIfNotExist(App.Users, App.EquipmentInventory);          

            Step("1. In Desktop page, click Users tile");
            Step("2. Expected Users page is routed");
            var usersPage = desktopPage.GoToApp(App.Users) as UsersPage;

            Step("3. Select the user profile which User A belongs to");
            usersPage.UserProfileListPanel.SelectProfile(userModel.Profile);

            Step("4. In Profile Detail widget, remove all 'Block Actions' items if any");
            usersPage.UserProfileDetailsPanel.RemoveAllBlockedActions();

            Step("5. Expected All blocked actions are removed off");
            var blockedActionsCount = usersPage.UserProfileDetailsPanel.GetBlockedActionsCount();
            VerifyEqual("5. Verify blocked actions are removed completely", 0, blockedActionsCount);

            Step("6. Add 'Delete a geozone' blocked action then click Save icon");
            usersPage.UserProfileDetailsPanel.SelectBlockActionsDropDown(expectedBlockAction);
            usersPage.UserProfileDetailsPanel.ClickSaveButton();
            usersPage.WaitForHeaderMessageDisplayed();
            var actualHeaderMessage = usersPage.GetHeaderMessage();

            Step("7. Expected Profile is updated successfully with message 'The user profile has been updated successfully'");
            VerifyEqual("7. Verify profile updated message successfully", "The user profile has been updated successfully", actualHeaderMessage);
            usersPage.WaitForHeaderMessageDisappeared();

            Step("8. Refresh browser");
            desktopPage = Browser.RefreshLoggedInCMS();

            Step("9. Go to Users app and select that profile again");
            usersPage = desktopPage.GoToApp(App.Users) as UsersPage;

            Step("10. Expected Saved blocked actions are present in the selected blocked actions");
            usersPage.UserProfileListPanel.SelectProfile(userModel.Profile);
            var currentBlockedActions = usersPage.UserProfileDetailsPanel.GetBlockedActionsName();
            VerifyTrue(string.Format("10. Verify saved blocked action '{0}' is present in the selected blocked actions", expectedBlockAction), currentBlockedActions.Any(e => e.Equals(expectedBlockAction)), "present", "Not present");

            Step("11. Go to 'Equipement Inventory' page");
            var equipmentInventoryPage = usersPage.AppBar.SwitchTo(App.EquipmentInventory) as EquipmentInventoryPage;

            Step("12. From 'Equipement Inventory' page, browser to and select any geozone");
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(geozone);

            Step("13. Expected Detail widget of the selected is displayed");
            Step("14. Click Delete icon");
            equipmentInventoryPage.GeozoneEditorPanel.ClickDeleteButton();

            Step("15. Expected A confirmation dialog is displayed with message 'Would you like to delete Test GeoZone geozone and all sub geoZones and equipments ?' and Yes and No buttons");
            equipmentInventoryPage.WaitForPopupDialogDisplayed();
            var expectedConfirmMessage = string.Format("Would you like to delete {0} geozone and all sub geoZones and equipments ?", geozone);
            var actualConfirmMessage = equipmentInventoryPage.Dialog.GetMessageText();
            VerifyEqual("15. Verify A confirmation dialog is displayed correctly", expectedConfirmMessage, actualConfirmMessage);

            Step("16. Click No button");
            equipmentInventoryPage.Dialog.ClickNoButton();

            Step("17.Expected The confirmation dialog disappears, nothing else happens");
            equipmentInventoryPage.WaitForPopupDialogDisappeared();

            Step("18. Click Delete icon again");
            equipmentInventoryPage.GeozoneEditorPanel.ClickDeleteButton();

            Step("19. Expected The confirmation dialog appears again");
            equipmentInventoryPage.WaitForPopupDialogDisplayed();

            Step("20. Click Yes button");
            equipmentInventoryPage.Dialog.ClickYesButton();
            equipmentInventoryPage.WaitForPreviousActionComplete();

            Step("21. Expected Preloader appears then disappears. Afterwards, a message is shown to inform the current user is not authorized to perform the action");
            equipmentInventoryPage.WaitForPopupDialogDisplayed();
            var expectedErrorMessage = string.Format("User '{0}' is not authorized to perform 'SLVAssetManagementAPI!deleteGeoZone'!", userModel.Username);
            var actualErrorMessage = equipmentInventoryPage.Dialog.GetMessageText();
            VerifyEqual("21. Verify A error message is displayed", expectedErrorMessage, actualErrorMessage);

            try
            {
                DeleteUserAndProfile(userModel);
                DeleteGeozone(geozone);
            }
            catch { }
        }        

        [Test, DynamicRetry]
        [Description("TS 1.10.5 Blocked Actions - Execute a command on one device")]
        public void TS1_10_05()
        {
            var testData = GetTestDataOfTestTS1_10_5();
            var expectedGeozone = testData["GeoZone"];
            var expectedDevice = testData["Device"];
            var expectedBlockAction = testData["BlockAction"];

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - User has not any blocked action");
            Step("**** Precondition ****\n");

            var userModel = CreateNewProfileAndUser();
            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(userModel.Username, userModel.Password);
            desktopPage.InstallAppsIfNotExist(App.Users, App.RealTimeControl);            

            Step("1. In Desktop page, click Users tile");
            Step("2. Expected Users page is routed");
            var usersPage = desktopPage.GoToApp(App.Users) as UsersPage;

            Step("3. Select the user profile which User A belongs to");
            usersPage.UserProfileListPanel.SelectProfile(userModel.Profile);

            Step("4. In Profile Detail widget, remove all 'Block Actions' items if any");
            usersPage.UserProfileDetailsPanel.RemoveAllBlockedActions();

            Step("5. Expected All blocked actions are removed off");
            var blockedActionsCount = usersPage.UserProfileDetailsPanel.GetBlockedActionsCount();
            VerifyEqual("5. Verify blocked actions are removed completely", 0, blockedActionsCount);

            Step("6. Add 'Execute a command on one device' blocked action then click Save icon");
            usersPage.UserProfileDetailsPanel.SelectBlockActionsDropDown(expectedBlockAction);
            usersPage.UserProfileDetailsPanel.ClickSaveButton();
            usersPage.WaitForHeaderMessageDisplayed();
            var actualHeaderMessage = usersPage.GetHeaderMessage();

            Step("7. Expected Profile is updated successfully with message 'The user profile has been updated successfully'");
            VerifyEqual("7. Verify profile updated message successfully", "The user profile has been updated successfully", actualHeaderMessage);
            usersPage.WaitForHeaderMessageDisappeared();

            Step("8. Refresh browser");
            desktopPage = Browser.RefreshLoggedInCMS();

            Step("9. Go to Users app and select that profile again");
            usersPage = desktopPage.GoToApp(App.Users) as UsersPage;

            Step("10. Expected Saved blocked actions are present in the selected blocked actions");
            usersPage.UserProfileListPanel.SelectProfile(userModel.Profile);
            var currentBlockedActions = usersPage.UserProfileDetailsPanel.GetBlockedActionsName();
            VerifyTrue(string.Format("10. Verify saved blocked action '{0}' is present in the selected blocked actions", expectedBlockAction), currentBlockedActions.Any(e => e.Equals(expectedBlockAction)), "present", "Not present");

            Step("11. Go to 'Real-time Control' page");
            var realtimeControlPage = usersPage.AppBar.SwitchTo(App.RealTimeControl) as RealTimeControlPage;

            Step("12. Select a sub-geozone");
            realtimeControlPage.GeozoneTreeMainPanel.SelectNode(expectedGeozone);

            Step("13. Select a device");
            realtimeControlPage.GeozoneTreeMainPanel.SelectNode(expectedDevice);
            realtimeControlPage.WaitForHeaderMessageDisplayed();

            Step("14. Expected A message displays on the top of the screen");
            Step(" + The text: \"User '' is not authorized to perform 'SLVMonitoringAPI!executeCommand'!\"");
            var expectedMessage = string.Format("User '{0}' is not authorized to perform 'SLVMonitoringAPI!executeCommand'!", userModel.Username);
            actualHeaderMessage = realtimeControlPage.GetHeaderMessage();
            VerifyEqual("14. Verify A message displays on the top of the screen as expected", expectedMessage, actualHeaderMessage);

            Step("15. Expected Device Control widget appears on the nearly top right side");
            realtimeControlPage.WaitForStreetlightWidgetDisplayed(expectedDevice);

            Step("16. Click preconfigured (90%, 80%, 70%, etc.) dimming levels");
            Step("17. Expected Feedback and Command are not set the preconfigured value");
            var notedCommandText = realtimeControlPage.StreetlightWidgetPanel.GetRealtimeCommandValueText();
            var notedFeedbackText = realtimeControlPage.StreetlightWidgetPanel.GetRealtimeFeedbackValueText();
            realtimeControlPage.StreetlightWidgetPanel.ExecuteRandomDimming();
            realtimeControlPage.StreetlightWidgetPanel.WaitForCommandCompleted();
            var actualCommandText = realtimeControlPage.StreetlightWidgetPanel.GetRealtimeCommandValueText();
            var actualFeedbackText = realtimeControlPage.StreetlightWidgetPanel.GetRealtimeFeedbackValueText();
            VerifyEqual("17. Verify Command is not set the preconfigured value", notedCommandText, actualCommandText);
            VerifyEqual("17. Verify Feedback is not set the preconfigured value", notedFeedbackText, actualFeedbackText);

            Step("18. Click ON, OFF; move triagle slider; commit command");
            Step("19. Expected");
            Step(" + Clicking ON/OFF is not affected. Click ON the feedback is not equal to 100% and clicking OFF the feedback is not equal to 0%");
            Step(" + Slider is NOT movable");
            Step(" + Commit value field is NOT editable");
            var notedSliderPosition = realtimeControlPage.StreetlightWidgetPanel.GetIndicatorCursorPositionValue();
            realtimeControlPage.StreetlightWidgetPanel.ExecuteCommand(RealtimeCommand.DimOn);
            realtimeControlPage.StreetlightWidgetPanel.WaitForCommandCompleted();
            actualCommandText = realtimeControlPage.StreetlightWidgetPanel.GetRealtimeCommandValueText();
            actualFeedbackText = realtimeControlPage.StreetlightWidgetPanel.GetRealtimeFeedbackValueText();
            var actualSliderPosition = realtimeControlPage.StreetlightWidgetPanel.GetIndicatorCursorPositionValue();
            VerifyEqual("19. Verify Clicking ON is not affected. (Command Text does not change)", notedCommandText, actualCommandText);
            VerifyEqual("19. Verify Clicking ON is not affected. (Feedback Text does not change)", notedFeedbackText, actualFeedbackText);
            VerifyEqual("19. Verify Slider is NOT movable", notedSliderPosition, actualSliderPosition);            

            realtimeControlPage.StreetlightWidgetPanel.ExecuteCommand(RealtimeCommand.DimOff);
            realtimeControlPage.StreetlightWidgetPanel.WaitForCommandCompleted();
            actualCommandText = realtimeControlPage.StreetlightWidgetPanel.GetRealtimeCommandValueText();
            actualFeedbackText = realtimeControlPage.StreetlightWidgetPanel.GetRealtimeFeedbackValueText();
            actualSliderPosition = realtimeControlPage.StreetlightWidgetPanel.GetIndicatorCursorPositionValue();
            VerifyEqual("19. Verify Clicking OFF is not affected. (Command Text does not change)", notedCommandText, actualCommandText);
            VerifyEqual("19. Verify Clicking OFF is not affected. (Feedback Text does not change)", notedFeedbackText, actualFeedbackText);
            VerifyEqual("19. Verify Slider is NOT movable", notedSliderPosition, actualSliderPosition);

            realtimeControlPage.StreetlightWidgetPanel.ExecuteRandomDimming10To90ByCursor();
            actualCommandText = realtimeControlPage.StreetlightWidgetPanel.GetRealtimeCommandValueText();
            actualFeedbackText = realtimeControlPage.StreetlightWidgetPanel.GetRealtimeFeedbackValueText();
            actualSliderPosition = realtimeControlPage.StreetlightWidgetPanel.GetIndicatorCursorPositionValue();
            VerifyEqual("19. Verify Execute command by cursor is not affected. (Command Text does not change)", notedCommandText, actualCommandText);
            VerifyEqual("19. Verify Execute command by cursor is not affected. (Feedback Text does not change)", notedFeedbackText, actualFeedbackText);
            VerifyEqual("19. Verify Slider is NOT movable", notedSliderPosition, actualSliderPosition);

            realtimeControlPage.StreetlightWidgetPanel.ClickToEditCommandValue();
            var isEditable = realtimeControlPage.StreetlightWidgetPanel.IsCommitCommandEditable();
            VerifyTrue("19. Verify Commit value field is NOT editable", !isEditable, "NOT editable", "Editable");

            try
            {
                //remove new profile and user
                DeleteUserAndProfile(userModel);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("TS 1.10.6 Blocked Actions - Exit from manual mode on a segment controller")]
        public void TS1_10_06()
        {
            var testData = GetTestDataOfTestTS1_10_6();
            var expectedGeozone = testData["GeoZone"];
            var expectedController = testData["Controller"];
            var expectedBlockAction = testData["BlockAction"];

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - User has not any blocked action");
            Step(" - Segment Controller has control technology 'iLON Segment Controller Version 4'");
            Step("**** Precondition ****\n");

            var userModel = CreateNewProfileAndUser();
            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(userModel.Username, userModel.Password);
            desktopPage.InstallAppsIfNotExist(App.Users, App.RealTimeControl);

            Step("1. In Desktop page, click Users tile");
            Step("2. Expected Users page is routed");
            var usersPage = desktopPage.GoToApp(App.Users) as UsersPage;

            Step("3. Select the user profile which User A belongs to");
            usersPage.UserProfileListPanel.SelectProfile(userModel.Profile);

            Step("4. In Profile Detail widget, remove all 'Block Actions' items if any");
            usersPage.UserProfileDetailsPanel.RemoveAllBlockedActions();

            Step("5. Expected All blocked actions are removed off");
            var blockedActionsCount = usersPage.UserProfileDetailsPanel.GetBlockedActionsCount();
            VerifyEqual("5. Verify blocked actions are removed completely", 0, blockedActionsCount);

            var allBlockActions = usersPage.UserProfileDetailsPanel.GetListOfBlockedActionsName();
            VerifyTrue("5. [SLV-3467] Verify Blocked action name is existing", allBlockActions.Exists(p => p.Equals(expectedBlockAction)), "Existing", "Not exist");

            Step("6. Add 'Exit from manual mode on a segment controller output' blocked action then click Save icon");
            usersPage.UserProfileDetailsPanel.SelectBlockActionsDropDown(expectedBlockAction);
            usersPage.UserProfileDetailsPanel.ClickSaveButton();
            usersPage.WaitForHeaderMessageDisplayed();
            var actualHeaderMessage = usersPage.GetHeaderMessage();

            Step("7. Expected Profile is updated successfully with message 'The user profile has been updated successfully'");
            VerifyEqual("7. Verify profile updated message successfully", "The user profile has been updated successfully", actualHeaderMessage);
            usersPage.WaitForHeaderMessageDisappeared();

            Step("8. Refresh browser");
            desktopPage = Browser.RefreshLoggedInCMS();

            Step("9. Go to Users app and select that profile again");
            usersPage = desktopPage.GoToApp(App.Users) as UsersPage;

            Step("10. Expected Saved blocked actions are present in the selected blocked actions");
            usersPage.UserProfileListPanel.SelectProfile(userModel.Profile);
            var currentBlockedActions = usersPage.UserProfileDetailsPanel.GetBlockedActionsName();
            VerifyTrue(string.Format("10. Verify saved blocked action '{0}' is present in the selected blocked actions", expectedBlockAction), currentBlockedActions.Any(e => e.Equals(expectedBlockAction)), "present", "Not present");

            Step("11. Go to 'Real-time Control' page");
            var realtimeControlPage = usersPage.AppBar.SwitchTo(App.RealTimeControl) as RealTimeControlPage;

            Step("12. Select DevCtrlA");
            realtimeControlPage.GeozoneTreeMainPanel.SelectNode(string.Format(@"{0}\{1}", expectedGeozone, expectedController));

            Step("13. Expected Device Control widget appears on the nearly top right side");
            realtimeControlPage.WaitForControllerWidgetDisplayed(expectedController);
            realtimeControlPage.WaitForHeaderMessageDisappeared();

            Step("14. Click AUTOMATIC button on the top left of the widget");
            Step("15. Expected The time value at the bottom right corner of the widget is not changed");
            var notedLastUpdateTime = realtimeControlPage.ControllerWidgetPanel.GetLastUpdateTimeText();
            realtimeControlPage.ControllerWidgetPanel.ClickClockButton();
            realtimeControlPage.ControllerWidgetPanel.WaitForCommandCompleted();
            var actualLastUpdateTime = realtimeControlPage.ControllerWidgetPanel.GetLastUpdateTimeText();
            VerifyEqual("15. Verify The time value at the bottom right corner of the widget is not changed (1st click)", notedLastUpdateTime, actualLastUpdateTime);

            realtimeControlPage.ControllerWidgetPanel.ClickClockButton();
            realtimeControlPage.ControllerWidgetPanel.WaitForCommandCompleted();
            actualLastUpdateTime = realtimeControlPage.ControllerWidgetPanel.GetLastUpdateTimeText();
            VerifyEqual("15. Verify The time value at the bottom right corner of the widget is not changed (2nd click)", notedLastUpdateTime, actualLastUpdateTime);
            
            try
            {
                //remove new profile and user
                DeleteUserAndProfile(userModel);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("TS 1.10.7 Blocked Actions - Exit from manual mode on a device")]
        public void TS1_10_07()
        {
            var testData = GetTestDataOfTestTS1_10_7();
            var expectedGeozone = testData["GeoZone"];
            var expectedDevice = testData["AnyDevice"];
            var expectedBlockAction = testData["BlockAction"];

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - User has not any blocked action");
            Step(" - Streetlight named which is in MANNUAL mode");
            Step("**** Precondition ****\n");

            var userModel = CreateNewProfileAndUser();
            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(userModel.Username, userModel.Password);
            desktopPage.InstallAppsIfNotExist(App.Users, App.RealTimeControl);

            Step(string.Format("Execute a command for streetlight '{0}' is in MANNUAL mode", expectedDevice));
            var realtimeControlPage = desktopPage.GoToApp(App.RealTimeControl) as RealTimeControlPage;
            realtimeControlPage.GeozoneTreeMainPanel.SelectNode(string.Format(@"{0}\{1}", expectedGeozone, expectedDevice));
            realtimeControlPage.WaitForStreetlightWidgetDisplayed(expectedDevice);
            realtimeControlPage.StreetlightWidgetPanel.ExecuteRandomDimming(RealtimeCommand.DimOff, RealtimeCommand.DimOn);
            realtimeControlPage.StreetlightWidgetPanel.WaitForCommandCompleted();
            
            Step("1. In Desktop page, click Users tile");
            Step("2. Expected Users page is routed");
            var usersPage = realtimeControlPage.AppBar.SwitchTo(App.Users) as UsersPage;

            Step("3. Select the user profile which User A belongs to");
            usersPage.UserProfileListPanel.SelectProfile(userModel.Profile);

            Step("4. In Profile Detail widget, remove all 'Block Actions' items if any");
            usersPage.UserProfileDetailsPanel.RemoveAllBlockedActions();

            Step("5. Expected All blocked actions are removed off");
            var blockedActionsCount = usersPage.UserProfileDetailsPanel.GetBlockedActionsCount();
            VerifyEqual("5. Verify blocked actions are removed completely", 0, blockedActionsCount);

            Step("6. Add 'Exit manual mode on a device' blocked action then click Save icon");
            usersPage.UserProfileDetailsPanel.SelectBlockActionsDropDown(expectedBlockAction);
            usersPage.UserProfileDetailsPanel.ClickSaveButton();
            usersPage.WaitForHeaderMessageDisplayed();
            var actualHeaderMessage = usersPage.GetHeaderMessage();

            Step("7. Expected Profile is updated successfully with message 'The user profile has been updated successfully'");
            VerifyEqual("7. Verify profile updated message successfully", "The user profile has been updated successfully", actualHeaderMessage);
            usersPage.WaitForHeaderMessageDisappeared();

            Step("8. Refresh browser");
            desktopPage = Browser.RefreshLoggedInCMS();

            Step("9. Go to Users app and select that profile again");
            usersPage = desktopPage.GoToApp(App.Users) as UsersPage;

            Step("10. Expected Saved blocked actions are present in the selected blocked actions");
            usersPage.UserProfileListPanel.SelectProfile(userModel.Profile);
            var currentBlockedActions = usersPage.UserProfileDetailsPanel.GetBlockedActionsName();
            VerifyTrue(string.Format("10. Verify saved blocked action '{0}' is present in the selected blocked actions", expectedBlockAction), currentBlockedActions.Any(e => e.Equals(expectedBlockAction)), "present", "Not present");

            Step("11. Go to 'Real-time Control' page");
            realtimeControlPage = usersPage.AppBar.SwitchTo(App.RealTimeControl) as RealTimeControlPage;

            Step("12. Select DevCtrlA");
            realtimeControlPage.GeozoneTreeMainPanel.SelectNode(string.Format(@"{0}\{1}", expectedGeozone, expectedDevice));

            Step("13. Expected Device Control widget appears on the nearly top right side");
            realtimeControlPage.WaitForStreetlightWidgetDisplayed(expectedDevice);

            Step("14. Click on the Clock icon next to the text 'MANNUAL' on the top left of the widget");
            Step("15. Expected The text is not changed to 'AUTOMATIC' and the values of Feedback and Command are unchanged.");       
            var notedCommandText = realtimeControlPage.StreetlightWidgetPanel.GetRealtimeCommandValueText();
            var notedFeedbackText = realtimeControlPage.StreetlightWidgetPanel.GetRealtimeFeedbackValueText();
            realtimeControlPage.StreetlightWidgetPanel.ClickClockButton();
            realtimeControlPage.WaitForPreviousActionComplete();
            var actualCommandText = realtimeControlPage.StreetlightWidgetPanel.GetRealtimeCommandValueText();
            var actualFeedbackText = realtimeControlPage.StreetlightWidgetPanel.GetRealtimeFeedbackValueText();
            var actualClockText = realtimeControlPage.StreetlightWidgetPanel.GetClockText();
            VerifyTrue("15. Verify The text is not changed to 'AUTOMATIC'", !actualClockText.Equals("AUTOMATIC"), "Not change to 'AUTOMATIC'", actualClockText);
            VerifyEqual("15. Verify Command is unchanged", notedCommandText, actualCommandText);
            VerifyEqual("15. Verify Feedback is unchanged", notedFeedbackText, actualFeedbackText);

            try
            {
                //remove new profile and user
                DeleteUserAndProfile(userModel);
            }
            catch { }
        }        

        [Test, DynamicRetry]
        [Description("TS 1.10.9 Blocked Actions - Replace a lamp")]
        [NonParallelizable]
        public void TS1_10_09()
        {
            var testData = GetTestDataOfTestTS1_10_9();
            var expectedParentGeozone = testData["GeoZone"];
            var expectedLamp = testData["AnyLamp"];
            var expectedBlockAction = testData["BlockAction"];

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - User has not any blocked action");
            Step("**** Precondition ****\n");

            var userModel = CreateNewProfileAndUser();
            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(userModel.Username, userModel.Password);
            desktopPage.InstallAppsIfNotExist(App.Users, App.EquipmentInventory);

            Step("1. In Desktop page, click Users tile");
            Step("2. Expected Users page is routed");
            var usersPage = desktopPage.GoToApp(App.Users) as UsersPage;

            Step("3. Select the user profile which User A belongs to");
            usersPage.UserProfileListPanel.SelectProfile(userModel.Profile);

            Step("4. In Profile Detail widget, remove all 'Block Actions' items if any");
            usersPage.UserProfileDetailsPanel.RemoveAllBlockedActions();

            Step("5. Expected All blocked actions are removed off");
            var blockedActionsCount = usersPage.UserProfileDetailsPanel.GetBlockedActionsCount();
            VerifyEqual("5. Verify blocked actions are removed completely", 0, blockedActionsCount);
            
            Step("6. Add 'Replace a lamp' blocked action then click Save icon");
            usersPage.UserProfileDetailsPanel.SelectBlockActionsDropDown(expectedBlockAction);
            usersPage.UserProfileDetailsPanel.ClickSaveButton();
            usersPage.WaitForHeaderMessageDisplayed();
            var actualHeaderMessage = usersPage.GetHeaderMessage();

            Step("7. Expected Profile is updated successfully with message 'The user profile has been updated successfully'");
            VerifyEqual("7. Verify profile updated message successfully", "The user profile has been updated successfully", actualHeaderMessage);
            usersPage.WaitForHeaderMessageDisappeared();

            Step("8. Go to 'Equipement Inventory' page");
            var equipmentInventoryPage = usersPage.AppBar.SwitchTo(App.EquipmentInventory) as EquipmentInventoryPage;

            Step("9. From 'Equipement Inventory' page, browser to and select any street light");
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(string.Format(@"{0}\{1}", expectedParentGeozone, expectedLamp));

            Step("10. Expected 'Replace Lamp' button is not visibile any more");
            var isReplaceLampButtonDisplayed = equipmentInventoryPage.StreetlightEditorPanel.IsReplaceLampButtonDisplayed();
            VerifyEqual("10. Verify 'Replace Lamp' button is not visibile any more", false, isReplaceLampButtonDisplayed);

            Step("11. Refresh browser");
            desktopPage = Browser.RefreshLoggedInCMS();

            Step("12. Go to 'Equipement Inventory' page, browser to and select any streetlight");
            equipmentInventoryPage = desktopPage.GoToApp(App.EquipmentInventory) as EquipmentInventoryPage;
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(string.Format(@"{0}\{1}", expectedParentGeozone, expectedLamp));

            Step("13. Expected 'Replace Lamp' button is still not visibile any more");
            isReplaceLampButtonDisplayed = equipmentInventoryPage.StreetlightEditorPanel.IsReplaceLampButtonDisplayed();
            VerifyEqual("13. Verify 'Replace Lamp' button is still not visibile any more", false, isReplaceLampButtonDisplayed);

            try
            {
                //Remove new profile and user
                DeleteUserAndProfile(userModel);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("TS 1.10.10 Blocked Actions - Replace a Light Point Controller")]
        [NonParallelizable]
        public void TS1_10_10()
        {
            var testData = GetTestDataOfTestTS1_10_10();
            var expectedGeozone = testData["GeoZone"];
            var expectedLightPointController = testData["AnyLightPointController"];
            var expectedBlockAction = testData["BlockAction"];
            var expectedUniqueAddress = testData["UniqueAddress"];

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - User has not any blocked action");
            Step("**** Precondition ****\n");

            var userModel = CreateNewProfileAndUser();
            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(userModel.Username, userModel.Password);
            desktopPage.InstallAppsIfNotExist(App.Users, App.EquipmentInventory);

            Step("1. In Desktop page, click Users tile");
            Step("2. Expected Users page is routed");
            var usersPage = desktopPage.GoToApp(App.Users) as UsersPage;

            Step("3. Select the user profile which User A belongs to");
            usersPage.UserProfileListPanel.SelectProfile(userModel.Profile);

            Step("4. In Profile Detail widget, remove all 'Block Actions' items if any");
            usersPage.UserProfileDetailsPanel.RemoveAllBlockedActions();

            Step("5. Expected All blocked actions are removed off");
            var blockedActionsCount = usersPage.UserProfileDetailsPanel.GetBlockedActionsCount();
            VerifyEqual("5. Verify blocked actions are removed completely", 0, blockedActionsCount);
            
            Step("6. Add 'Replace a Light Point Controller' blocked action then click Save icon");
            usersPage.UserProfileDetailsPanel.SelectBlockActionsDropDown(expectedBlockAction);
            usersPage.UserProfileDetailsPanel.ClickSaveButton();
            usersPage.WaitForHeaderMessageDisplayed();
            var actualHeaderMessage = usersPage.GetHeaderMessage();

            Step("7. Expected Profile is updated successfully with message 'The user profile has been updated successfully'");
            VerifyEqual("7. Verify profile updated message successfully", "The user profile has been updated successfully", actualHeaderMessage);
            usersPage.WaitForHeaderMessageDisappeared();

            Step("8. Go to 'Equipement Inventory' page");
            var equipmentInventoryPage = usersPage.AppBar.SwitchTo(App.EquipmentInventory) as EquipmentInventoryPage;

            Step("9. From 'Equipement Inventory' page, browser to and select any streetlight");
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(string.Format(@"{0}\{1}", expectedGeozone, expectedLightPointController));

            Step("10. Expected 'Replace Node' button is not visibile any more");
            var isReplaceNodeButtonDisplayed = equipmentInventoryPage.StreetlightEditorPanel.IsReplaceNodeButtonDisplayed();
            VerifyEqual("10. Verify 'Replace Node' button is not visibile any more", false, isReplaceNodeButtonDisplayed);

            Step("11. Refresh browser");
            desktopPage = Browser.RefreshLoggedInCMS();

            Step("12. Go to 'Equipement Inventory' page, browser to and select any streetlight");
            equipmentInventoryPage = desktopPage.GoToApp(App.EquipmentInventory) as EquipmentInventoryPage;
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(string.Format(@"{0}\{1}", expectedGeozone, expectedLightPointController));

            Step("13. Expected 'Replace Node' button is still not visibile any more");
            isReplaceNodeButtonDisplayed = equipmentInventoryPage.StreetlightEditorPanel.IsReplaceNodeButtonDisplayed();
            VerifyEqual("13. Verify 'Replace Node' button is still not visibile any more", false, isReplaceNodeButtonDisplayed);

            try
            {
                //remove new profile and user
                DeleteUserAndProfile(userModel);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("TS 1.10.14 Blocked Actions - Send real-time commands to a light point")]
        public void TS1_10_14()
        {
            var testData = GetTestDataOfTestTS1_10_14();
            var expectedGeozone = testData["GeoZone"];
            var expectedDevice = testData["Device"];
            var expectedBlockAction = testData["BlockAction"];

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - User has not any blocked action");
            Step(" - The testing streetlight has Feedback and Command values != 100 or 0%");
            Step("**** Precondition ****\n");

            var userModel = CreateNewProfileAndUser();
            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(userModel.Username, userModel.Password);
            desktopPage.InstallAppsIfNotExist(App.Users, App.RealTimeControl);

            Step(string.Format("Execute a command for streetlight '{0}' has Feedback and Command values != 100% or 0%", expectedDevice));
            var realtimeControlPage = desktopPage.GoToApp(App.RealTimeControl) as RealTimeControlPage;
            realtimeControlPage.GeozoneTreeMainPanel.SelectNode(string.Format(@"{0}\{1}", expectedGeozone, expectedDevice));
            realtimeControlPage.WaitForStreetlightWidgetDisplayed(expectedDevice);
            realtimeControlPage.StreetlightWidgetPanel.ExecuteRandomDimming(RealtimeCommand.DimOff, RealtimeCommand.DimOn);
            realtimeControlPage.StreetlightWidgetPanel.WaitForCommandCompleted();
            
            Step("1. In Desktop page, click Users tile");
            Step("2. Expected Users page is routed");
            var usersPage = realtimeControlPage.AppBar.SwitchTo(App.Users) as UsersPage;

            Step("3. Select the user profile which User A belongs to");
            usersPage.UserProfileListPanel.SelectProfile(userModel.Profile);

            Step("4. In Profile Detail widget, remove all 'Block Actions' items if any");
            usersPage.UserProfileDetailsPanel.RemoveAllBlockedActions();

            Step("5. Expected All blocked actions are removed off");
            var blockedActionsCount = usersPage.UserProfileDetailsPanel.GetBlockedActionsCount();
            VerifyEqual("5. Verify blocked actions are removed completely", 0, blockedActionsCount);

            var allBlockActions = usersPage.UserProfileDetailsPanel.GetListOfBlockedActionsName();
            VerifyTrue("5. [SLV-3467] Verify Blocked action name is existing", allBlockActions.Exists(p => p.Equals(expectedBlockAction)), "Existing", "Not exist");

            Step("6. Add 'Send realtime commands to a light point' blocked action then click Save icon");
            usersPage.UserProfileDetailsPanel.SelectBlockActionsDropDown(expectedBlockAction);
            usersPage.UserProfileDetailsPanel.ClickSaveButton();
            usersPage.WaitForHeaderMessageDisplayed();
            var actualHeaderMessage = usersPage.GetHeaderMessage();

            Step("7. Expected Profile is updated successfully with message 'The user profile has been updated successfully'");
            VerifyEqual("7. Verify profile updated message successfully", "The user profile has been updated successfully", actualHeaderMessage);
            usersPage.WaitForHeaderMessageDisappeared();

            Step("8. Refresh browser");
            desktopPage = Browser.RefreshLoggedInCMS();

            Step("9. Go to Users app and select that profile again");
            usersPage = desktopPage.GoToApp(App.Users) as UsersPage;

            Step("10. Expected Saved blocked actions are present in the selected blocked actions");
            usersPage.UserProfileListPanel.SelectProfile(userModel.Profile);
            var currentBlockedActions = usersPage.UserProfileDetailsPanel.GetBlockedActionsName();
            VerifyTrue(string.Format("10. Verify saved blocked action '{0}' is present in the selected blocked actions", expectedBlockAction), currentBlockedActions.Any(e => e.Equals(expectedBlockAction)), "present", "Not present");

            Step("11. Go to 'Real-time Control' page");
            realtimeControlPage = usersPage.AppBar.SwitchTo(App.RealTimeControl) as RealTimeControlPage;

            Step("12. Select a sub-geozone");
            realtimeControlPage.GeozoneTreeMainPanel.SelectNode(expectedGeozone);

            Step("13. Select a device");
            realtimeControlPage.GeozoneTreeMainPanel.SelectNode(expectedDevice);

            Step("14. Expected Device Control widget appears on the nearly top right side");
            realtimeControlPage.WaitForStreetlightWidgetDisplayed(expectedDevice);

            Step("15. Click ON, OFF; move triagle slider; commit command");
            Step("16. Clicking ON/OFF is not affected. Click ON the feedback is not equal to 100% and clicking OFF the feedback is not equal to 0%");
            var notedCommandText = realtimeControlPage.StreetlightWidgetPanel.GetRealtimeCommandValueText();
            var notedFeedbackText = realtimeControlPage.StreetlightWidgetPanel.GetRealtimeFeedbackValueText();
            var notedSliderPosition = realtimeControlPage.StreetlightWidgetPanel.GetIndicatorCursorPositionValue();
            realtimeControlPage.StreetlightWidgetPanel.ExecuteCommand(RealtimeCommand.DimOn);
            realtimeControlPage.StreetlightWidgetPanel.WaitForCommandCompleted();
            var actualCommandText = realtimeControlPage.StreetlightWidgetPanel.GetRealtimeCommandValueText();
            var actualFeedbackText = realtimeControlPage.StreetlightWidgetPanel.GetRealtimeFeedbackValueText();
            var actualSliderPosition = realtimeControlPage.StreetlightWidgetPanel.GetIndicatorCursorPositionValue();
            VerifyTrue("16. Verify Command is not set to 100%", !actualCommandText.Equals("100%"), "<> 100%", actualCommandText);
            VerifyTrue("16. Verify Feedback is not set to 100%", !actualFeedbackText.Equals("100%"), "<> 100%", actualFeedbackText);

            realtimeControlPage.StreetlightWidgetPanel.ExecuteCommand(RealtimeCommand.DimOff);
            realtimeControlPage.StreetlightWidgetPanel.WaitForCommandCompleted();
            actualCommandText = realtimeControlPage.StreetlightWidgetPanel.GetRealtimeCommandValueText();
            actualFeedbackText = realtimeControlPage.StreetlightWidgetPanel.GetRealtimeFeedbackValueText();
            actualSliderPosition = realtimeControlPage.StreetlightWidgetPanel.GetIndicatorCursorPositionValue();
            VerifyTrue("16. Verify Command is not set to 0%", !actualCommandText.Equals("0%"), "<> 0%", actualCommandText);
            VerifyTrue("16. Verify Feedback is not set to 0%", !actualFeedbackText.Equals("0%"), "<> 0%", actualFeedbackText);
            
            realtimeControlPage.StreetlightWidgetPanel.ExecuteRandomDimming10To90ByCursor();
            actualSliderPosition = realtimeControlPage.StreetlightWidgetPanel.GetIndicatorCursorPositionValue();
            VerifyEqual("16. Verify Execute command by cursor is not affected. (Command Text does not change)", notedCommandText, actualCommandText);
            VerifyEqual("16. Verify Execute command by cursor is not affected. (Feedback Text does not change)", notedFeedbackText, actualFeedbackText);
            VerifyEqual("16. Verify Slider is NOT movable", notedSliderPosition, actualSliderPosition);

            realtimeControlPage.StreetlightWidgetPanel.ClickToEditCommandValue();
            var isEditable = realtimeControlPage.StreetlightWidgetPanel.IsCommitCommandEditable();
            VerifyTrue("16. Verify Commit value field is NOT editable", !isEditable, "NOT editable", "Editable");

            Step("17. Click preconfigured (90%, 80%, 70%, etc.) dimming levels");
            Step("18. Expected The values of Feedback and Command are not changed.");
            realtimeControlPage.StreetlightWidgetPanel.ExecuteRandomDimming(RealtimeCommand.DimOff, RealtimeCommand.DimOn);
            realtimeControlPage.StreetlightWidgetPanel.WaitForCommandCompleted();
            actualCommandText = realtimeControlPage.StreetlightWidgetPanel.GetRealtimeCommandValueText();
            actualFeedbackText = realtimeControlPage.StreetlightWidgetPanel.GetRealtimeFeedbackValueText();
            VerifyEqual("18. Verify Execute preconfigured command, Command Text is not change", notedCommandText, actualCommandText);
            VerifyEqual("18. Verify Execute preconfigured command, Feedback Text is not change", notedFeedbackText, actualFeedbackText);
                      
            try
            {
                //remove new profile and user
                DeleteUserAndProfile(userModel);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("TS 1.10.15 Blocked Actions - Switch a segment controller output")]
        public void TS1_10_15()
        {
            var testData = GetTestDataOfTestTS1_10_15();
            var expectedGeozone = testData["GeoZone"];
            var expectedController = testData["Controller"];
            var expectedBlockAction = testData["BlockAction"];

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - User has not any blocked action");
            Step(" - Segment Controller named DevCtrlA has control technology 'iLON Segment Controller Version 4' and the status is AUTOMATIC and the Output 1 is 'ON'");
            Step("**** Precondition ****\n");

            var userModel = CreateNewProfileAndUser();
            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(userModel.Username, userModel.Password);
            desktopPage.InstallAppsIfNotExist(App.Users, App.RealTimeControl);

            Step(string.Format("Set status is AUTOMATIC and the Output 1 is 'ON', the Output 2 is 'OFF' for controller '{0}'", expectedController));
            var realtimeControlPage = desktopPage.GoToApp(App.RealTimeControl) as RealTimeControlPage;
            realtimeControlPage.GeozoneTreeMainPanel.SelectNode(string.Format(@"{0}\{1}", expectedGeozone, expectedController));
            realtimeControlPage.WaitForControllerWidgetDisplayed(expectedController);
            realtimeControlPage.ControllerWidgetPanel.ClickInputsOutputsButton();
            realtimeControlPage.ControllerWidgetPanel.WaitForInputsOutputsPanelDisplayed();
            realtimeControlPage.ControllerWidgetPanel.InputsOutputsWidgetPanel.ClickOutput1OnButton();
            realtimeControlPage.WaitForPopupDialogDisplayed();
            realtimeControlPage.Dialog.ClickYesButton();
            realtimeControlPage.ControllerWidgetPanel.WaitForCommandCompleted();
            realtimeControlPage.ControllerWidgetPanel.InputsOutputsWidgetPanel.ClickOutput2OffButton();
            realtimeControlPage.WaitForPopupDialogDisplayed();
            realtimeControlPage.Dialog.ClickYesButton();
            realtimeControlPage.ControllerWidgetPanel.WaitForCommandCompleted();
            realtimeControlPage.ControllerWidgetPanel.ClickInputsOutputsButton();
            realtimeControlPage.ControllerWidgetPanel.WaitForInputsOutputsPanelDisappeared();
            realtimeControlPage.ControllerWidgetPanel.ClickClockButton();
            realtimeControlPage.ControllerWidgetPanel.WaitForCommandCompleted();
            
            Step("1. In Desktop page, click Users tile");
            Step("2. Expected Users page is routed");
            var usersPage = realtimeControlPage.AppBar.SwitchTo(App.Users) as UsersPage;

            Step("3. Select the user profile which User A belongs to");
            usersPage.UserProfileListPanel.SelectProfile(userModel.Profile);

            Step("4. In Profile Detail widget, remove all 'Block Actions' items if any");
            usersPage.UserProfileDetailsPanel.RemoveAllBlockedActions();

            Step("5. Expected All blocked actions are removed off");
            var blockedActionsCount = usersPage.UserProfileDetailsPanel.GetBlockedActionsCount();
            VerifyEqual("5. Verify blocked actions are removed completely", 0, blockedActionsCount);

            Step("6. Add 'Switch a segment controller output' blocked action then click Save icon");
            usersPage.UserProfileDetailsPanel.SelectBlockActionsDropDown(expectedBlockAction);
            usersPage.UserProfileDetailsPanel.ClickSaveButton();
            usersPage.WaitForHeaderMessageDisplayed();
            var actualHeaderMessage = usersPage.GetHeaderMessage();

            Step("7. Expected Profile is updated successfully with message 'The user profile has been updated successfully'");
            VerifyEqual("7. Verify profile updated message successfully", "The user profile has been updated successfully", actualHeaderMessage);
            usersPage.WaitForHeaderMessageDisappeared();

            Step("8. Refresh browser");
            desktopPage = Browser.RefreshLoggedInCMS();

            Step("9. Go to Users app and select that profile again");
            usersPage = desktopPage.GoToApp(App.Users) as UsersPage;

            Step("10. Expected Saved blocked actions are present in the selected blocked actions");
            usersPage.UserProfileListPanel.SelectProfile(userModel.Profile);
            var currentBlockedActions = usersPage.UserProfileDetailsPanel.GetBlockedActionsName();
            VerifyTrue(string.Format("10. Verify saved blocked action '{0}' is present in the selected blocked actions", expectedBlockAction), currentBlockedActions.Any(e => e.Equals(expectedBlockAction)), "present", "Not present");

            Step("11. Go to 'Real-time Control' page");
            realtimeControlPage = usersPage.AppBar.SwitchTo(App.RealTimeControl) as RealTimeControlPage;

            Step("12. Select DevCtrlA");
            realtimeControlPage.GeozoneTreeMainPanel.SelectNode(string.Format(@"{0}\{1}", expectedGeozone, expectedController));
            realtimeControlPage.WaitForHeaderMessageDisappeared();

            Step("13. Expected Device Control widget appears on the nearly top right side");
            realtimeControlPage.WaitForControllerWidgetDisplayed(expectedController);

            Step("14. Click Inputs|Outputs button on top-middle");
            realtimeControlPage.ControllerWidgetPanel.ClickInputsOutputsButton();

            Step("15. Expected Inputs/Outputs widget appears");
            realtimeControlPage.ControllerWidgetPanel.WaitForInputsOutputsPanelDisplayed();

            Step("16. Click OFF button for Output 1 on the widget");
            Step("17. Expected");
            Step(" + The text 'AUTO' is not changed to 'MANUAL'");
            Step(" + The GREEN circle is not changed to GREY");
            realtimeControlPage.ControllerWidgetPanel.InputsOutputsWidgetPanel.ClickOutput1OffButton();
            realtimeControlPage.WaitForPopupDialogDisplayed();
            realtimeControlPage.Dialog.ClickYesButton();
            realtimeControlPage.ControllerWidgetPanel.WaitForCommandCompleted();
            VerifyEqual("17. Verify Output 1, The text 'AUTO' is not changed to 'MANUAL'", "AUTO", realtimeControlPage.ControllerWidgetPanel.InputsOutputsWidgetPanel.GetOutput1ModeText());
            VerifyTrue("17. Verify Output 1, The GREEN circle is not changed to GREY", realtimeControlPage.ControllerWidgetPanel.InputsOutputsWidgetPanel.IsOutput1StatusGREEN(), "GREEN", "NOT GREEN");

            Step("18. Click On button for Output 2 and press Yes on the comfirmation pop-up on the widget");
            Step("19. Expected");
            Step(" + The text 'MANUAL' is not changed to 'AUTO'");
            Step(" + The GREY circle is not changed to GREEN");
            realtimeControlPage.ControllerWidgetPanel.InputsOutputsWidgetPanel.ClickOutput2OnButton();
            realtimeControlPage.WaitForPopupDialogDisplayed();
            realtimeControlPage.Dialog.ClickYesButton();
            realtimeControlPage.ControllerWidgetPanel.WaitForCommandCompleted();
            VerifyEqual("19. Verify Output 2, The text 'AUTO' is not changed to 'MANUAL'", "AUTO", realtimeControlPage.ControllerWidgetPanel.InputsOutputsWidgetPanel.GetOutput2ModeText());
            VerifyTrue("19. Verify Output 2, The GREY circle is not changed to GREEN", realtimeControlPage.ControllerWidgetPanel.InputsOutputsWidgetPanel.IsOutput2StatusGRAY(), "GRAY", "NOT GRAY");

            try
            {
                //remove new profile and user
                DeleteUserAndProfile(userModel);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("TS 1.10.16 Blocked Actions - Synchronize segment controller’s clock")]
        public void TS1_10_16()
        {
            var testData = GetTestDataOfTestTS1_10_16();
            var expectedGeozone = testData["GeoZone"];
            var expectedController = testData["Controller"];
            var expectedBlockAction = testData["BlockAction"];

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - User has not any blocked action");
            Step(" - Segment Controller named DevCtrlA has control technology 'iLON Segment Controller Version 4'");
            Step("**** Precondition ****\n");

            var userModel = CreateNewProfileAndUser();
            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(userModel.Username, userModel.Password);
            desktopPage.InstallAppsIfNotExist(App.Users, App.RealTimeControl);

            Step("1. In Desktop page, click Users tile");
            Step("2. Expected Users page is routed");
            var usersPage = desktopPage.GoToApp(App.Users) as UsersPage;

            Step("3. Select the user profile which User A belongs to");
            usersPage.UserProfileListPanel.SelectProfile(userModel.Profile);

            Step("4. In Profile Detail widget, remove all 'Block Actions' items if any");
            usersPage.UserProfileDetailsPanel.RemoveAllBlockedActions();

            Step("5. Expected All blocked actions are removed off");
            var blockedActionsCount = usersPage.UserProfileDetailsPanel.GetBlockedActionsCount();
            VerifyEqual("5. Verify blocked actions are removed completely", 0, blockedActionsCount);

            var allBlockActions = usersPage.UserProfileDetailsPanel.GetListOfBlockedActionsName();
            VerifyTrue("[SLV-3467] Verify Blocked action name is existing", allBlockActions.Exists(p => p.Equals(expectedBlockAction)), "Existing", "Not exist");

            Step("6. Add 'Synchronize segment controller's clock' blocked action then click Save icon");
            usersPage.UserProfileDetailsPanel.SelectBlockActionsDropDown(expectedBlockAction);
            usersPage.UserProfileDetailsPanel.ClickSaveButton();
            usersPage.WaitForHeaderMessageDisplayed();
            var actualHeaderMessage = usersPage.GetHeaderMessage();

            Step("7. Expected Profile is updated successfully with message 'The user profile has been updated successfully'");
            VerifyEqual("7. Verify profile updated message successfully", "The user profile has been updated successfully", actualHeaderMessage);
            usersPage.WaitForHeaderMessageDisappeared();

            Step("8. Refresh browser");
            desktopPage = Browser.RefreshLoggedInCMS();

            Step("9. Go to Users app and select that profile again");
            usersPage = desktopPage.GoToApp(App.Users) as UsersPage;

            Step("10. Expected Saved blocked actions are present in the selected blocked actions");
            usersPage.UserProfileListPanel.SelectProfile(userModel.Profile);
            var currentBlockedActions = usersPage.UserProfileDetailsPanel.GetBlockedActionsName();
            VerifyTrue(string.Format("10. Verify saved blocked action '{0}' is present in the selected blocked actions", expectedBlockAction), currentBlockedActions.Any(e => e.Equals(expectedBlockAction)), "present", "Not present");

            Step("11. Go to 'Real-time Control' page");
            var realtimeControlPage = usersPage.AppBar.SwitchTo(App.RealTimeControl) as RealTimeControlPage;

            Step("12. Select DevCtrlA");
            realtimeControlPage.GeozoneTreeMainPanel.SelectNode(string.Format(@"{0}\{1}", expectedGeozone, expectedController));

            Step("13. Expected Device Control widget appears on the nearly top right side");
            realtimeControlPage.WaitForControllerWidgetDisplayed(expectedController);

            Step("14. Click Right Arrow icon on the widget");
            realtimeControlPage.ControllerWidgetPanel.ClickInformationButton();

            Step("15. Expected Date Time widget is displayed");
            realtimeControlPage.ControllerWidgetPanel.WaitForInformationWidgetPanelDisplayed();

            Step("16. Click SYNCHRONIZE button");
            realtimeControlPage.ControllerWidgetPanel.InformationWidgetPanel.ClickDateTimeSynchronizeButton();

            Step("17. Expected Error message is displayed because the logged user is not authorized to perform the action");
            realtimeControlPage.WaitForPreviousActionComplete();
            realtimeControlPage.WaitForPopupMessageDisplayed();
            var expectedErrorMessage = string.Format("User '{0}' is not authorized to perform 'SLVControllerAPI!setSystemTime'!", userModel.Username);
            var actualErrorMessage = realtimeControlPage.Dialog.GetMessageText();
            VerifyEqual("17. Verify A error message is displayed", expectedErrorMessage, actualErrorMessage);

            try
            {
                //remove new profile and user
                DeleteUserAndProfile(userModel);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("TS 1.10.17 Blocked Actions - Update a geozone")]
        [NonParallelizable]
        public void TS1_10_17()
        {
            var testData = GetTestDataOfTestTS1_10_17();
            var expectedParentGeozone = testData["GeoZone"];
            var expectedGeozone = testData["AnyGeoZone"];
            var expectedBlockAction = testData["BlockAction"];

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - User has not any blocked action");
            Step("**** Precondition ****\n");

            var userModel = CreateNewProfileAndUser();
            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(userModel.Username, userModel.Password);
            desktopPage.InstallAppsIfNotExist(App.Users, App.EquipmentInventory);

            Step("1. In Desktop page, click Users tile");
            Step("2. Expected Users page is routed");
            var usersPage = desktopPage.GoToApp(App.Users) as UsersPage;

            Step("3. Select the user profile which User A belongs to");
            usersPage.UserProfileListPanel.SelectProfile(userModel.Profile);

            Step("4. In Profile Detail widget, remove all 'Block Actions' items if any");
            usersPage.UserProfileDetailsPanel.RemoveAllBlockedActions();

            Step("5. Expected All blocked actions are removed off");
            var blockedActionsCount = usersPage.UserProfileDetailsPanel.GetBlockedActionsCount();
            VerifyEqual("5. Verify blocked actions are removed completely", 0, blockedActionsCount);

            Step("6. Add 'Update a geozone' blocked action then click Save icon");
            usersPage.UserProfileDetailsPanel.SelectBlockActionsDropDown(expectedBlockAction);
            usersPage.UserProfileDetailsPanel.ClickSaveButton();
            usersPage.WaitForHeaderMessageDisplayed();
            var actualHeaderMessage = usersPage.GetHeaderMessage();

            Step("7. Expected Profile is updated successfully with message 'The user profile has been updated successfully'");
            VerifyEqual("7. Verify profile updated message successfully", "The user profile has been updated successfully", actualHeaderMessage);
            usersPage.WaitForHeaderMessageDisappeared();

            Step("8. Refresh browser");
            desktopPage = Browser.RefreshLoggedInCMS();

            Step("9. Go to Users app and select that profile again");
            usersPage = desktopPage.GoToApp(App.Users) as UsersPage;

            Step("10. Expected Saved blocked actions are present in the selected blocked actions");
            usersPage.UserProfileListPanel.SelectProfile(userModel.Profile);
            var currentBlockedActions = usersPage.UserProfileDetailsPanel.GetBlockedActionsName();
            VerifyTrue(string.Format("10. Verify saved blocked action '{0}' is present in the selected blocked actions", expectedBlockAction), currentBlockedActions.Any(e => e.Equals(expectedBlockAction)), "present", "Not present");

            Step("11. Go to 'Equipement Inventory' page");
            var equipmentInventoryPage = usersPage.AppBar.SwitchTo(App.EquipmentInventory) as EquipmentInventoryPage;

            Step("12. From 'Equipement Inventory' page, browser to and select any geozone");
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(string.Format(@"{0}\{1}", expectedParentGeozone, expectedGeozone));

            Step("13. Expected Detail widget of the selected is displayed. The map moves to zone of the selected geozone");
            Step("14. Click 'Update geoZone bounds' button");
            equipmentInventoryPage.GeozoneEditorPanel.ClickUpdateBoundsButton();
            equipmentInventoryPage.Map.WaitForRecorderDisplayed();
            equipmentInventoryPage.Map.DragMapToRandomLocation();
            equipmentInventoryPage.WaitForPreviousActionComplete();

            Step("15. Select new zone then click on red widget to submit new location");
            equipmentInventoryPage.Map.ClickRecorderButton();
            equipmentInventoryPage.Map.WaitForRecorderDisappeared();
            equipmentInventoryPage.WaitForPreviousActionComplete();

            Step("16. Expected Preloader appears then disappears. Afterwards, a message is shown to inform the current user is not authorized to perform the action");
            equipmentInventoryPage.WaitForPopupDialogDisplayed();
            var expectedErrorMessage = string.Format("User '{0}' is not authorized to perform 'SLVAssetManagementAPI!updateGeoZone'!", userModel.Username);
            var actualErrorMessage = equipmentInventoryPage.Dialog.GetMessageText();
            VerifyEqual("16. Verify A error message is displayed", expectedErrorMessage, actualErrorMessage);

            try
            {
                //remove new profile and user
                DeleteUserAndProfile(userModel);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("TS 1.10.18 Blocked Actions - Commission calendar")]
        [NonParallelizable]
        public void TS1_10_18()
        {
            var testData = GetTestDataOfTestTS1_10_18();
            var xmlBlockAction = testData["BlockAction"];

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var userModel = CreateNewProfileAndUser();
            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(userModel.Username, userModel.Password);
            desktopPage.InstallAppsIfNotExist(App.Users, App.SchedulingManager);

            Step("1. Go to Users app");
            Step("2. Expected Users page is routed and loaded successfully");
            var usersPage = desktopPage.GoToApp(App.Users) as UsersPage;

            Step("3. Browse to the profile of current logged in user");
            usersPage.UserProfileListPanel.SelectProfile(userModel.Profile);

            Step("4. Remove all blocked actions if any then add 'Commission calendar' then save");
            usersPage.UserProfileDetailsPanel.RemoveAllBlockedActions();

            Step("5. Expected Saving is successful");
            var blockedActionsCount = usersPage.UserProfileDetailsPanel.GetBlockedActionsCount();
            VerifyEqual("5. Verify blocked actions are removed completely", 0, blockedActionsCount);
            usersPage.UserProfileDetailsPanel.SelectBlockActionsDropDown(xmlBlockAction);
            usersPage.UserProfileDetailsPanel.ClickSaveButton();
            usersPage.WaitForHeaderMessageDisplayed();
            var actualHeaderMessage = usersPage.GetHeaderMessage();
            VerifyEqual("5. Verify profile updated message successfully", "The user profile has been updated successfully", actualHeaderMessage);
            usersPage.WaitForHeaderMessageDisappeared();

            Step("6. Switch to Scheduling Manager app");
            var schedulingManagerPage = usersPage.AppBar.SwitchTo(App.SchedulingManager) as SchedulingManagerPage;

            Step("7. Select Calendar tab");
            schedulingManagerPage.SchedulingManagerPanel.SelectTab("Calendar");
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("8. Expected Commission calendar icon button is not displayed");
            var isCommissionCalendarToolbarButtonVisible = schedulingManagerPage.SchedulingManagerPanel.IsCommissionCalendarButtonVisible();
            VerifyEqual("8. Verify Commission calendar icon button is not displayed", false, isCommissionCalendarToolbarButtonVisible);

            Step("9. Select another calendar in the grid");
            var dimmingGroupList = schedulingManagerPage.SchedulingManagerPanel.GetListOfCalendarName();
            dimmingGroupList.Remove("Default Group");
            var anyDimming = dimmingGroupList.PickRandom();
            schedulingManagerPage.SchedulingManagerPanel.SelectCalendar(anyDimming);
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("10. Expected Commission calendar icon button is still not displayed");
            isCommissionCalendarToolbarButtonVisible = schedulingManagerPage.SchedulingManagerPanel.IsCommissionCalendarButtonVisible();
            VerifyEqual("10. Verify Commission calendar icon button is still not displayed", false, isCommissionCalendarToolbarButtonVisible);

            Step("11. Switch back to Users app then remove the blocked action set previously then save");
            usersPage = usersPage.AppBar.SwitchTo(App.Users) as UsersPage;
            usersPage.UserProfileDetailsPanel.RemoveAllBlockedActions();
            usersPage.UserProfileDetailsPanel.ClickSaveButton();
            usersPage.WaitForPreviousActionComplete();
            usersPage.WaitForHeaderMessageDisappeared();

            Step("12. Switch back to Scheduling Manager app and select Calendar tab");
            schedulingManagerPage = usersPage.AppBar.SwitchTo(App.SchedulingManager) as SchedulingManagerPage;

            Step("13. Expected Commission calendar icon button is now displayed");
            isCommissionCalendarToolbarButtonVisible = schedulingManagerPage.SchedulingManagerPanel.IsCommissionCalendarButtonVisible();
            VerifyEqual("13. Verify Commission calendar icon button is now displayed", true, isCommissionCalendarToolbarButtonVisible);

            Step("14. Select another calendar in the grid");
            anyDimming = dimmingGroupList.PickRandom();
            schedulingManagerPage.SchedulingManagerPanel.SelectCalendar(anyDimming);
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("15. Expected Commission calendar icon button is still displayed");
            isCommissionCalendarToolbarButtonVisible = schedulingManagerPage.SchedulingManagerPanel.IsCommissionCalendarButtonVisible();
            VerifyEqual("15. Verify Commission calendar icon button is now displayed", true, isCommissionCalendarToolbarButtonVisible);

            try
            {
                //remove new profile and user
                DeleteUserAndProfile(userModel);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("TS 1.10.19 Blocked Actions - Commission device")]
        [NonParallelizable]
        public void TS1_10_19()
        {
            var testData = GetTestDataOfTestTS1_10_19();
            var xmlBlockAction = testData["BlockAction"];
            var geozone = SLVHelper.GenerateUniqueName("GZNTS11019");
            var controller = SLVHelper.GenerateUniqueName("CTRL");
            var streetlight = SLVHelper.GenerateUniqueName("STL");
            var switchDevice = SLVHelper.GenerateUniqueName("SWH");

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");
            
            Step("-> Create data for testing");
            DeleteGeozones("GZNTS11019*");
            var userModel = CreateNewProfileAndUser();
            CreateNewGeozone(geozone);
            CreateNewController(controller, geozone);
            CreateNewDevice(DeviceType.Streetlight, streetlight, controller, geozone);
            CreateNewDevice(DeviceType.Switch, switchDevice, controller, geozone);

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(userModel.Username, userModel.Password);
            desktopPage.InstallAppsIfNotExist(App.Users, App.EquipmentInventory);

            Step("1. Go to Users app");
            Step("2. Expected Users page is routed and loaded successfully");
            var usersPage = desktopPage.GoToApp(App.Users) as UsersPage;

            Step("3. Browse to the profile of current logged in user");
            usersPage.UserProfileListPanel.SelectProfile(userModel.Profile);

            Step("4. Remove all blocked actions if any then add 'Commission device' then save");
            usersPage.UserProfileDetailsPanel.RemoveAllBlockedActions();

            Step("5. Expected Saving is successful");
            var blockedActionsCount = usersPage.UserProfileDetailsPanel.GetBlockedActionsCount();
            VerifyEqual("5. Verify blocked actions are removed completely", 0, blockedActionsCount);
            usersPage.UserProfileDetailsPanel.SelectBlockActionsDropDown(xmlBlockAction);
            usersPage.UserProfileDetailsPanel.ClickSaveButton();
            usersPage.WaitForHeaderMessageDisplayed();
            var actualHeaderMessage = usersPage.GetHeaderMessage();
            VerifyEqual("5. Verify profile updated message successfully", "The user profile has been updated successfully", actualHeaderMessage);
            usersPage.WaitForHeaderMessageDisappeared();

            Step("6. Switch to Equipment Inventory app app");
            var equipmentInventoryPage = usersPage.AppBar.SwitchTo(App.EquipmentInventory) as EquipmentInventoryPage;

            Step("7. Select a device");
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(geozone);
            var allDevices = equipmentInventoryPage.GeozoneTreeMainPanel.GetChildNodeNamesOfSelectedNode();
            allDevices.Remove(controller);
            var device = allDevices.PickRandom();
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(device);

            Step("8. Expected Device Details panel appears. Commission device icon button is not displayed");
            equipmentInventoryPage.WaitForEditorPanelDisplayed();
            var isCommissionButtonDisplayed = equipmentInventoryPage.DeviceEditorPanel.IsCommissionButtonDisplayed();
            VerifyEqual("8. Verify Commission device icon button is not displayed", true, !isCommissionButtonDisplayed);

            Step("9. Switch back to Users app then remove the blocked action set previously then save");
            usersPage = usersPage.AppBar.SwitchTo(App.Users) as UsersPage;
            usersPage.UserProfileDetailsPanel.RemoveAllBlockedActions();
            usersPage.UserProfileDetailsPanel.ClickSaveButton();
            usersPage.WaitForPreviousActionComplete();
            usersPage.WaitForHeaderMessageDisappeared();

            Step("10. Switch back to Equipment Inventory app and select the previously selected device");
            equipmentInventoryPage = usersPage.AppBar.SwitchTo(App.EquipmentInventory) as EquipmentInventoryPage;

            Step("11. Expected Device Details panel appears. Commission device icon button is now displayed");
            equipmentInventoryPage.WaitForEditorPanelDisplayed();
            equipmentInventoryPage.WaitForPreviousActionComplete();
            equipmentInventoryPage.DeviceEditorPanel.WaitForCommissionButtonDisplayed();
            isCommissionButtonDisplayed = equipmentInventoryPage.DeviceEditorPanel.IsCommissionButtonDisplayed();
            VerifyEqual("11. Verify Commission device icon button is now displayed", true, isCommissionButtonDisplayed);

            try
            {
                DeleteUserAndProfile(userModel);
                DeleteGeozone(geozone);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("TS 1.10.20 Blocked Actions - Delete multiple devices at once")]
        [NonParallelizable]
        public void TS1_10_20()
        {
            var testData = GetTestDataOfTestTS1_10_20();
            var xmlBlockAction = testData["BlockAction"];
            var geozone = SLVHelper.GenerateUniqueName("GZNTS11020");
            var controller = SLVHelper.GenerateUniqueName("CTRL");
            var deviceName1 = SLVHelper.GenerateUniqueName("STL1");
            var deviceName2 = SLVHelper.GenerateUniqueName("STL2");
            var deviceName3 = SLVHelper.GenerateUniqueName("STL3");
            var devicesName = new List<string>() { deviceName1, deviceName2 };
            var devicesLongLat = new List<string>();

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            Step("-> Create data for testing");
            DeleteGeozones("GZNTS11020*");
            var userModel = CreateNewProfileAndUser();
            CreateNewGeozone(geozone, latMin: "10.60203", latMax: "10.60559", lngMin: "106.60801", lngMax: "106.61554");
            CreateNewController(controller, geozone);
            var latitude = SLVHelper.GenerateCoordinate("10.60319", "10.60460");
            var longitude = SLVHelper.GenerateCoordinate("106.61080", "106.61280");
            devicesLongLat.Add(string.Format("{0};{1}", longitude, latitude));
            CreateNewDevice(DeviceType.Streetlight, deviceName1, controller, geozone, lat: latitude, lng: longitude);
            latitude = SLVHelper.GenerateCoordinate("10.60319", "10.60460");
            longitude = SLVHelper.GenerateCoordinate("106.61080", "106.61280");
            devicesLongLat.Add(string.Format("{0};{1}", longitude, latitude));
            CreateNewDevice(DeviceType.Streetlight, deviceName2, controller, geozone, lat: latitude, lng: longitude);
            latitude = SLVHelper.GenerateCoordinate("10.60319", "10.60460");
            longitude = SLVHelper.GenerateCoordinate("106.61080", "106.61280");
            CreateNewDevice(DeviceType.Streetlight, deviceName3, controller, geozone, lat: latitude, lng: longitude);

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(userModel.Username, userModel.Password);
            desktopPage.InstallAppsIfNotExist(App.Users, App.EquipmentInventory);

            Step("1. Go to Users app");
            Step("2. Expected Users page is routed and loaded successfully");
            var usersPage = desktopPage.GoToApp(App.Users) as UsersPage;

            Step("3. Browse to the profile of current logged in user");
            usersPage.UserProfileListPanel.SelectProfile(userModel.Profile);

            Step("4. Remove all blocked actions if any then add 'Modify device's inventory' then save");
            usersPage.UserProfileDetailsPanel.RemoveAllBlockedActions();

            Step("5. Expected Saving is successful");
            var blockedActionsCount = usersPage.UserProfileDetailsPanel.GetBlockedActionsCount();
            VerifyEqual("5. Verify blocked actions are removed completely", 0, blockedActionsCount);
            usersPage.UserProfileDetailsPanel.SelectBlockActionsDropDown(xmlBlockAction);
            usersPage.UserProfileDetailsPanel.ClickSaveButton();
            usersPage.WaitForHeaderMessageDisplayed();
            var actualHeaderMessage = usersPage.GetHeaderMessage();
            VerifyEqual("5. Verify profile updated message successfully", "The user profile has been updated successfully", actualHeaderMessage);
            usersPage.WaitForHeaderMessageDisappeared();

            Step("6. Switch to Equipment Inventory app app");
            var equipmentInventoryPage = usersPage.AppBar.SwitchTo(App.EquipmentInventory) as EquipmentInventoryPage;

            Step("7. Select a geozone");
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(geozone);

            Step("8. Expected Geozone editor panel appears");
            equipmentInventoryPage.WaitForEditorPanelDisplayed();

            Step("9. Select many devices in the map (at least 2) then hit Delete");
            equipmentInventoryPage.Map.SelectDevicesGL(devicesLongLat);
            equipmentInventoryPage.WaitForMultipleDevicesEditorDisplayed();
            equipmentInventoryPage.PressDeleteKey();

            Step("10. Expected No dialog appears");
            var hasPopupDialog = equipmentInventoryPage.HasPopupDialogDisplayed();
            VerifyEqual("10. Verify No dialog appears", false, hasPopupDialog);

            Step("11. Expected Delete button is not displayed in editor toolbar");
            VerifyEqual("11. Verify Delete button is not displayed", false, equipmentInventoryPage.MultipleDevicesEditorPanel.IsDeleteButtonDisplayed());

            Step("12. Switch back to Users app then remove the blocked action set previously then save");
            usersPage = usersPage.AppBar.SwitchTo(App.Users) as UsersPage;
            usersPage.UserProfileListPanel.SelectProfile(userModel.Profile);
            usersPage.UserProfileDetailsPanel.RemoveAllBlockedActions();
            usersPage.UserProfileDetailsPanel.ClickSaveButton();
            usersPage.WaitForPreviousActionComplete();
            usersPage.WaitForHeaderMessageDisappeared();

            Step("13. Switch back to Equipment Inventory app");
            equipmentInventoryPage = usersPage.AppBar.SwitchTo(App.EquipmentInventory) as EquipmentInventoryPage;

            Step("14. Select a geozone which contains devices");
            equipmentInventoryPage.WaitForPreviousActionComplete();
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(geozone);

            Step("15. Expected Geozone editor panel appears");
            equipmentInventoryPage.WaitForEditorPanelDisplayed();

            Step("16. Select many devices in the map (at least 2) then hit Delete");           
            equipmentInventoryPage.Map.SelectDevicesGL(devicesLongLat);
            equipmentInventoryPage.WaitForMultipleDevicesEditorDisplayed();

            Step("17. Expected Delete button is displayed in editor toolbar");
            VerifyEqual("17. Verify Delete button is displayed", true, equipmentInventoryPage.MultipleDevicesEditorPanel.IsDeleteButtonDisplayed());

            Step("18. Expected A confirmation dialog with message 'Would you like to delete selected equipments ?' appears");
            equipmentInventoryPage.MultipleDevicesEditorPanel.ClickDeleteButton();
            equipmentInventoryPage.WaitForPopupDialogDisplayed();
            VerifyEqual("18. Verify A confirmation dialog with message 'Would you like to delete selected equipments ?' appears", "Would you like to delete selected equipments ?", equipmentInventoryPage.Dialog.GetMessageText());

            Step("19. Click Yes");
            equipmentInventoryPage.Dialog.ClickYesButton();
            equipmentInventoryPage.WaitForPopupDialogDisappeared();
            equipmentInventoryPage.WaitForPreviousActionComplete();

            Step("20. Expected The selected devices are deleted successfully by verifying their presensce in geozone tree and the map");
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(geozone);
            var allDevices = equipmentInventoryPage.GeozoneTreeMainPanel.GetChildDeviceNamesOfSelectedNode();
            foreach (var device in devicesName)
            {
                VerifyEqual(string.Format("20. Verify '{0}' is not present in geozone tree as well as the map", device), false, allDevices.Contains(device));
            }

            Step("21. Select another device then hit Delete");
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(deviceName3);
            equipmentInventoryPage.WaitForEditorPanelDisplayed();
            equipmentInventoryPage.StreetlightEditorPanel.ClickDeleteButton();
            equipmentInventoryPage.WaitForPopupDialogDisplayed();
            devicesName.Add(deviceName3);

            Step("22. Expected A confirmation dialog with message 'Would you like to delete {{Device Name}} equipment ?' appears");
            var message = string.Format("Would you like to delete {0} equipment ?", deviceName3);
            VerifyEqual(string.Format("22. Verify A confirmation dialog with message '{0}' appears", message), message, equipmentInventoryPage.Dialog.GetMessageText());

            Step("23. Click Yes");
            equipmentInventoryPage.Dialog.ClickYesButton();
            equipmentInventoryPage.WaitForPopupDialogDisappeared();
            equipmentInventoryPage.WaitForPreviousActionComplete();

            Step("24. Expected The selected device is deleted successfully by verifying their presensce in geozone tree and the map");
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(geozone);
            allDevices = equipmentInventoryPage.GeozoneTreeMainPanel.GetChildDeviceNamesOfSelectedNode();
            foreach (var device in devicesName)
            {
                VerifyEqual(string.Format("24. Verify '{0}' is not present in geozone tree as well as the map", device), false, allDevices.Contains(device));
            }

            Step("25. Reload browser then go to Equipment Inventory again");
            desktopPage = Browser.RefreshLoggedInCMS();
            equipmentInventoryPage = desktopPage.GoToApp(App.EquipmentInventory) as EquipmentInventoryPage;

            Step("26. Select the previously selected geozone");
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(geozone);

            Step("27. Expected All deleted devices are not present in geozone tree as well as the map");
            allDevices = equipmentInventoryPage.GeozoneTreeMainPanel.GetChildDeviceNamesOfSelectedNode();
            foreach (var device in devicesName)
            {
                VerifyEqual(string.Format("27. Verify '{0}' is not present in geozone tree as well as the map", device), false, allDevices.Contains(device));
            }

            try
            {               
                DeleteUserAndProfile(userModel);
                DeleteGeozone(geozone);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("TS 1.10.21 Blocked Actions - Import csv or sdp file")]
        [NonParallelizable]
        [Category("RunAlone")]
        public void TS1_10_21()
        {
            var testData = GetTestDataOfTestTS1_10_21();
            var xmlBlockAction = testData["BlockAction"];
            var geozone = SLVHelper.GenerateUniqueName("GZNTS11021");
            var controller = SLVHelper.GenerateUniqueName("CTRL");
            var fullGeozonePath = Settings.RootGeozoneName + @"/" + geozone;
            var fullPathOfImportedFileName = Settings.GetFullPath(Settings.CSV_FILE_PATH + "TS11021.csv");

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            Step("-> Create data for testing");
            DeleteGeozones("GZNTS11021*");
            var userModel = CreateNewProfileAndUser();
            CreateNewGeozone(geozone);
            CreateNewController(controller, geozone);
            CreateCsvDevices(5, DeviceType.Streetlight, fullPathOfImportedFileName, fullGeozonePath, controller, "SLTS11021");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(userModel.Username, userModel.Password);
            desktopPage.InstallAppsIfNotExist(App.Users, App.EquipmentInventory);

            Step("1. Go to Users app");
            Step("2. Expected Users page is routed and loaded successfully");
            var usersPage = desktopPage.GoToApp(App.Users) as UsersPage;

            Step("3. Browse to the profile of current logged in user");
            usersPage.UserProfileListPanel.SelectProfile(userModel.Profile);

            Step("4. Remove all blocked actions if any then add 'Import *.csv or *.sdp file' then save");
            usersPage.UserProfileDetailsPanel.RemoveAllBlockedActions();

            Step("5. Expected Saving is successful");
            var blockedActionsCount = usersPage.UserProfileDetailsPanel.GetBlockedActionsCount();
            VerifyEqual("5. Verify blocked actions are removed completely", 0, blockedActionsCount);
            usersPage.UserProfileDetailsPanel.SelectBlockActionsDropDown(xmlBlockAction);
            usersPage.UserProfileDetailsPanel.ClickSaveButton();
            usersPage.WaitForHeaderMessageDisplayed();
            var actualHeaderMessage = usersPage.GetHeaderMessage();
            VerifyEqual("5. Verify profile updated message successfully", "The user profile has been updated successfully", actualHeaderMessage);
            usersPage.WaitForHeaderMessageDisappeared();

            Step("6. Switch to Equipment Inventory app app");
            var equipmentInventoryPage = usersPage.AppBar.SwitchTo(App.EquipmentInventory) as EquipmentInventoryPage;

            Step("7. Select a geozone");
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(geozone);

            Step("8. Expected Geozone Editor panel appears");
            equipmentInventoryPage.WaitForEditorPanelDisplayed();

            Step("9. Dropdown More menu");
            Step("10. Click Import if it is still displayed in More menu");
            equipmentInventoryPage.GeozoneEditorPanel.GetListOfMoreMenuItems();
            var moreMenuItems = equipmentInventoryPage.GeozoneEditorPanel.GetListOfMoreMenuItems();
            if (moreMenuItems.Contains("Import"))
            {
                Step("11. Expected Verify nothing happens, Import menu item now disappears");             
                equipmentInventoryPage.GeozoneEditorPanel.ClickMoreButton();
                equipmentInventoryPage.GeozoneEditorPanel.ClickImportMenuItem();
                SLVHelper.OpenFileFromFileDialog(fullPathOfImportedFileName);
                equipmentInventoryPage.WaitUntilOpenFileDialogDisappears();
                equipmentInventoryPage.GeozoneEditorPanel.WaitForImportPanelDisplayed();
                equipmentInventoryPage.WaitForPreviousActionComplete();
                moreMenuItems = equipmentInventoryPage.GeozoneEditorPanel.GetListOfMoreMenuItems();
                VerifyEqual("11. Verify Import menu item now disappears", false, moreMenuItems.Contains("Import"));
            }

            Step("12. Reload browser then go to Equipment Inventory again");
            desktopPage = Browser.RefreshLoggedInCMS();
            equipmentInventoryPage = desktopPage.GoToApp(App.EquipmentInventory) as EquipmentInventoryPage;

            Step("13. Select a geozone and dropdown More menu");
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(geozone);
            equipmentInventoryPage.WaitForEditorPanelDisplayed();

            Step("14. Expected Import menu item is not displayed in More menu");
            moreMenuItems = equipmentInventoryPage.GeozoneEditorPanel.GetListOfMoreMenuItems();
            VerifyEqual("14. Verify Import menu item is not displayed in More menu", false, moreMenuItems.Contains("Import"));

            Step("15. Switch back to Users app then remove the blocked action set previously then save");
            usersPage = usersPage.AppBar.SwitchTo(App.Users) as UsersPage;
            usersPage.UserProfileListPanel.SelectProfile(userModel.Profile);
            usersPage.UserProfileDetailsPanel.RemoveAllBlockedActions();
            usersPage.UserProfileDetailsPanel.ClickSaveButton();
            usersPage.WaitForPreviousActionComplete();
            usersPage.WaitForHeaderMessageDisappeared();

            Step("16. Switch back to Equipment Inventory app and select the previously selected geozone");
            equipmentInventoryPage = usersPage.AppBar.SwitchTo(App.EquipmentInventory) as EquipmentInventoryPage;

            Step("17. Expected Geozone Editor panel appears");
            equipmentInventoryPage.WaitForEditorPanelDisplayed();
            equipmentInventoryPage.WaitForPreviousActionComplete();

            Step("18. Dropdown More menu");
            Step("19. Expected Import menu item is now present back");
            equipmentInventoryPage.GeozoneEditorPanel.GetListOfMoreMenuItems();
            Wait.ForSeconds(2); //For Import item appears
            moreMenuItems = equipmentInventoryPage.GeozoneEditorPanel.GetListOfMoreMenuItems();
            VerifyEqual("19. Verify Import menu item is now present back", true, moreMenuItems.Contains("Import"));

            try
            {
                DeleteUserAndProfile(userModel);
                DeleteGeozone(geozone);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("TS 1.10.22 Blocked Actions - Modify calendar")]
        [NonParallelizable]
        public void TS1_10_22()
        {
            var testData = GetTestDataOfTestTS1_10_22();
            var xmlBlockAction = testData["BlockAction"];

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var userModel = CreateNewProfileAndUser();
            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(userModel.Username, userModel.Password);
            desktopPage.InstallAppsIfNotExist(App.Users, App.SchedulingManager);

            Step("1. Go to Users app");
            Step("2. Expected Users page is routed and loaded successfully");
            var usersPage = desktopPage.GoToApp(App.Users) as UsersPage;

            Step("3. Browse to the profile of current logged in user");
            usersPage.UserProfileListPanel.SelectProfile(userModel.Profile);

            Step("4. Remove all blocked actions if any then add 'Modify calendar' then save");
            usersPage.UserProfileDetailsPanel.RemoveAllBlockedActions();

            Step("5. Expected Saving is successful");
            var blockedActionsCount = usersPage.UserProfileDetailsPanel.GetBlockedActionsCount();
            VerifyEqual("5. Verify blocked actions are removed completely", 0, blockedActionsCount);
            usersPage.UserProfileDetailsPanel.SelectBlockActionsDropDown(xmlBlockAction);
            usersPage.UserProfileDetailsPanel.ClickSaveButton();
            usersPage.WaitForHeaderMessageDisplayed();
            var actualHeaderMessage = usersPage.GetHeaderMessage();
            VerifyEqual("5. Verify profile updated message successfully", "The user profile has been updated successfully", actualHeaderMessage);
            usersPage.WaitForHeaderMessageDisappeared();

            Step("6. Switch to Scheduling Manager app");
            var schedulingManagerPage = usersPage.AppBar.SwitchTo(App.SchedulingManager) as SchedulingManagerPage;

            Step("7. Select Calendar tab and select a calendar");
            schedulingManagerPage.SchedulingManagerPanel.SelectTab("Calendar");
            schedulingManagerPage.WaitForPreviousActionComplete();

            var dimmingGroupList = schedulingManagerPage.SchedulingManagerPanel.GetListOfCalendarName();
            dimmingGroupList.Remove("Default Group");
            var anyDimming = dimmingGroupList.PickRandom();
            schedulingManagerPage.SchedulingManagerPanel.SelectCalendar(anyDimming);
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("8. Expected Verify all fields are unmodifiable:");
            Step(" - Calendar grid toolbar:");
            Step("   + New, Duplicate, Remove buttons are not displayed");
            var isAddCalendarToolbarButtonVisible = schedulingManagerPage.SchedulingManagerPanel.IsAddCalendarButtonVisible();
            var isDeleteCalendarToolbarButtonVisible = schedulingManagerPage.SchedulingManagerPanel.IsDeleteCalendarButtonVisible();
            var isDuplicateCalendarToolbarButtonVisible = schedulingManagerPage.SchedulingManagerPanel.IsDuplicateCalendarButtonVisible();
            VerifyEqual("8. Verify New, Duplicate, Remove buttons are not displayed", true, isAddCalendarToolbarButtonVisible == false && isDeleteCalendarToolbarButtonVisible == false && isDuplicateCalendarToolbarButtonVisible == false);
            Step("   + Commission button is still displayed");
            var isCommissionCalendarToolbarButtonVisible = schedulingManagerPage.SchedulingManagerPanel.IsCommissionCalendarButtonVisible();
            VerifyEqual("8. Verify Commission button is still displayed", true, isCommissionCalendarToolbarButtonVisible);

            Step(" - Calendar editor:");
            Step("   + Name, Description: read-only");
            var isNameInputReadOnly = schedulingManagerPage.CalendarEditorPanel.IsNameInputReadOnly();
            var isDescriptionInputReadOnly = schedulingManagerPage.CalendarEditorPanel.IsDescriptionInputReadOnly();
            VerifyEqual("8. Verify Name, Description: read-only", true, isNameInputReadOnly && isDescriptionInputReadOnly);

            Step("   + 12 month panels: when clicking a cell inside each panel, Control programs editor doesn't appear");
            schedulingManagerPage.CalendarEditorPanel.ClickRandomCalendarDate();
            var hasPopupDialogDisplayed = schedulingManagerPage.HasPopupDialogDisplayed();
            VerifyEqual("8. Verify Control programs editor doesn't appear", false, hasPopupDialogDisplayed);

            Step("   + Calendar items button: visible. When it's click, Calendar items popup appears. Its toolbar buttons are not available (Delete, Up, Down button). Cancel and Save button are not displayed as well");
            var isCalendarItemsButtonVisible = schedulingManagerPage.CalendarEditorPanel.IsCalendarItemsButtonVisible();
            VerifyEqual("8. Verify Calendar items button: visible", true, isCalendarItemsButtonVisible);
            schedulingManagerPage.CalendarEditorPanel.ClickCalendarItemsButton();
            schedulingManagerPage.WaitForPopupDialogDisplayed();
            var isCancelButtonVisible = schedulingManagerPage.CalendarEditorItemsPopupPanel.IsCancelButtonVisible();
            var isSaveButtonVisible = schedulingManagerPage.CalendarEditorItemsPopupPanel.IsSaveButtonVisible();
            var isDeleteButtonVisible = schedulingManagerPage.CalendarEditorItemsPopupPanel.IsDeleteButtonVisible();
            var isUpButtonVisible = schedulingManagerPage.CalendarEditorItemsPopupPanel.IsUpButtonVisible();
            var isDownButtonVisible = schedulingManagerPage.CalendarEditorItemsPopupPanel.IsDownButtonVisible();
            VerifyEqual("8. Verify Delete, Up, Down buttons are not available", true, isDeleteButtonVisible == false && isUpButtonVisible == false && isDownButtonVisible == false);
            VerifyEqual("8. Verify Cancel and Save button are not displayed as well", true, isCancelButtonVisible == false && isSaveButtonVisible == false);
            schedulingManagerPage.ControlProgramItemsPopupPanel.ClickCloseButton();
            schedulingManagerPage.WaitForPopupDialogDisappeared();

            Step("   + Clear and Save button are not displayed");
            var isClearButtonCalendarEditorVisible = schedulingManagerPage.CalendarEditorPanel.IsClearButtonVisible();
            var isSaveButtonCalendarEditorVisible = schedulingManagerPage.CalendarEditorPanel.IsSaveButtonVisible();
            VerifyEqual("8. Verify Clear and Save button are not displayed", true, isClearButtonCalendarEditorVisible == false && isSaveButtonCalendarEditorVisible == false);

            Step("9. Click Back 1 year button then click a date in a month panel");
            Step("10. Expected Control programs table doesn't appear");
            schedulingManagerPage.CalendarEditorPanel.ClickYearBeforeButton();
            VerifyTrue("[SLV-3860] 10. Blank frame when clicking on Next or Previous year above a calendar", schedulingManagerPage.CalendarEditorPanel.IsYearBeforeButtonVisible(), "YearBefore Button is visible", "YearBefore Button is not visible");
            Step(" --> Ignore by SLV-3120");
            schedulingManagerPage.CalendarEditorPanel.ClickRandomCalendarDate();
            hasPopupDialogDisplayed = schedulingManagerPage.HasPopupDialogDisplayed();
            VerifyTrue("[SLV-3120] 10. Verify Control programs editor doesn't appear", hasPopupDialogDisplayed == false, "Not appear", "appears");

            Step("11. Click Forawrd 1 year button then click a date in a month panel");
            Step("12. Expected Control programs table doesn't appear");
            schedulingManagerPage.CalendarEditorPanel.ClickYearAfterButton();
            VerifyTrue("[SLV-3860] 12. Blank frame when clicking on Next or Previous year above a calendar", schedulingManagerPage.CalendarEditorPanel.IsYearAfterButtonVisible(), "YearAfter Button is visible", "YearAfter Button is not visible");
            schedulingManagerPage.CalendarEditorPanel.ClickRandomCalendarDate();
            hasPopupDialogDisplayed = schedulingManagerPage.HasPopupDialogDisplayed();
            VerifyTrue("[SLV-3120] 12. Verify Control programs editor doesn't appear", hasPopupDialogDisplayed == false, "Not appear", "appears");

            Step("13. Switch back to Users app then remove the blocked action set previously then save");
            usersPage = usersPage.AppBar.SwitchTo(App.Users) as UsersPage;
            usersPage.UserProfileDetailsPanel.RemoveAllBlockedActions();
            usersPage.UserProfileDetailsPanel.ClickSaveButton();
            usersPage.WaitForPreviousActionComplete();
            usersPage.WaitForHeaderMessageDisappeared();

            Step("14. Switch back to Scheduling Manager app and select the previously selected calendar");
            schedulingManagerPage = usersPage.AppBar.SwitchTo(App.SchedulingManager) as SchedulingManagerPage;

            Step("15. Expected Verify all fields are modifiable:");
            Step(" - Calendar grid toolbar:");
            Step("   + New, Duplicate, Remove buttons are displayed");
            isAddCalendarToolbarButtonVisible = schedulingManagerPage.SchedulingManagerPanel.IsAddCalendarButtonVisible();
            isDeleteCalendarToolbarButtonVisible = schedulingManagerPage.SchedulingManagerPanel.IsDeleteCalendarButtonVisible();
            isDuplicateCalendarToolbarButtonVisible = schedulingManagerPage.SchedulingManagerPanel.IsDuplicateCalendarButtonVisible();
            VerifyEqual("15. Verify New, Duplicate, Remove buttons are displayed", true, isAddCalendarToolbarButtonVisible && isDeleteCalendarToolbarButtonVisible && isDuplicateCalendarToolbarButtonVisible);
            Step("   + Commission button is displayed");
            isCommissionCalendarToolbarButtonVisible = schedulingManagerPage.SchedulingManagerPanel.IsCommissionCalendarButtonVisible();
            VerifyEqual("15. Verify Commission button is displayed", true, isCommissionCalendarToolbarButtonVisible);

            Step(" - Calendar editor:");
            Step("   + Name, Description: editable");
            isNameInputReadOnly = schedulingManagerPage.CalendarEditorPanel.IsNameInputReadOnly();
            isDescriptionInputReadOnly = schedulingManagerPage.CalendarEditorPanel.IsDescriptionInputReadOnly();
            VerifyEqual("15. Verify Name, Description: editable", true, isNameInputReadOnly == false && isDescriptionInputReadOnly == false);

            Step("   + 12 month panels: when clicking a cell inside each panel, Control programs editor appears");
            schedulingManagerPage.CalendarEditorPanel.ClickRandomCalendarDate();
            schedulingManagerPage.WaitForPopupDialogDisplayed();
            hasPopupDialogDisplayed = schedulingManagerPage.HasPopupDialogDisplayed();
            VerifyEqual("15. Verify Control programs editor appears", true, hasPopupDialogDisplayed);
            schedulingManagerPage.CalendarEditorItemsPopupPanel.ClickCloseButton();
            schedulingManagerPage.WaitForPopupDialogDisappeared();

            Step("   + Calendar items button: visible. When it's click, Calendar items popup appears. Its toolbar buttons are available (Delete, Up, Down button). Cancel and Save button are displayed as well");
            isCalendarItemsButtonVisible = schedulingManagerPage.CalendarEditorPanel.IsCalendarItemsButtonVisible();
            VerifyEqual("15. Verify Calendar items button: visible", true, isCalendarItemsButtonVisible);
            schedulingManagerPage.CalendarEditorPanel.ClickCalendarItemsButton();
            schedulingManagerPage.WaitForPopupDialogDisplayed();
            isCancelButtonVisible = schedulingManagerPage.CalendarEditorItemsPopupPanel.IsCancelButtonVisible();
            isSaveButtonVisible = schedulingManagerPage.CalendarEditorItemsPopupPanel.IsSaveButtonVisible();
            isDeleteButtonVisible = schedulingManagerPage.CalendarEditorItemsPopupPanel.IsDeleteButtonVisible();
            isUpButtonVisible = schedulingManagerPage.CalendarEditorItemsPopupPanel.IsUpButtonVisible();
            isDownButtonVisible = schedulingManagerPage.CalendarEditorItemsPopupPanel.IsDownButtonVisible();
            VerifyEqual("15. Verify Delete, Up, Down buttons are available", true, isDeleteButtonVisible && isUpButtonVisible && isDownButtonVisible);
            VerifyEqual("15. Verify Cancel and Save button are displayed as well", true, isCancelButtonVisible && isSaveButtonVisible);
            schedulingManagerPage.ControlProgramItemsPopupPanel.ClickCloseButton();
            schedulingManagerPage.WaitForPopupDialogDisappeared();

            Step("   + Clear and Save button are displayed");
            isClearButtonCalendarEditorVisible = schedulingManagerPage.CalendarEditorPanel.IsClearButtonVisible();
            isSaveButtonCalendarEditorVisible = schedulingManagerPage.CalendarEditorPanel.IsSaveButtonVisible();
            VerifyEqual("15. Verify Clear and Save button are displayed", true, isClearButtonCalendarEditorVisible && isSaveButtonCalendarEditorVisible);

            Step("16. Click Back 1 year button then click a date in a month panel");
            Step("17 Expected Control programs table appear");
            schedulingManagerPage.CalendarEditorPanel.ClickYearBeforeButton();
            schedulingManagerPage.CalendarEditorPanel.ClickRandomCalendarDate();
            schedulingManagerPage.WaitForPopupDialogDisplayed();
            hasPopupDialogDisplayed = schedulingManagerPage.HasPopupDialogDisplayed();
            VerifyEqual("17. Verify Control programs editor appears", true, hasPopupDialogDisplayed);
            schedulingManagerPage.CalendarEditorItemsPopupPanel.ClickCloseButton();
            schedulingManagerPage.WaitForPopupDialogDisappeared();

            Step("18. Click Forawrd 1 year button then click a date in a month panel");
            Step("19. Expected Control programs table appears");
            schedulingManagerPage.CalendarEditorPanel.ClickYearAfterButton();
            schedulingManagerPage.CalendarEditorPanel.ClickRandomCalendarDate();
            schedulingManagerPage.WaitForPopupDialogDisplayed();
            hasPopupDialogDisplayed = schedulingManagerPage.HasPopupDialogDisplayed();
            VerifyEqual("19. Verify Control programs editor appears", true, hasPopupDialogDisplayed);
            schedulingManagerPage.CalendarEditorItemsPopupPanel.ClickCloseButton();
            schedulingManagerPage.WaitForPopupDialogDisappeared();

            try
            {
                //remove new profile and user
                DeleteUserAndProfile(userModel);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("TS 1.10.23 Blocked Actions - Modify control program")]
        [NonParallelizable]
        public void TS1_10_23()
        {
            var testData = GetTestDataOfTestTS1_10_23();
            var xmlBlockAction = testData["BlockAction"].ToString();
            var xmlControlPrograms = testData["ControlPrograms"] as List<string>;

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var userModel = CreateNewProfileAndUser();
            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(userModel.Username, userModel.Password);
            desktopPage.InstallAppsIfNotExist(App.Users, App.SchedulingManager);

            Step("1. Go to Users app");
            Step("2. Expected Users page is routed and loaded successfully");
            var usersPage = desktopPage.GoToApp(App.Users) as UsersPage;

            Step("3. Browse to the profile of current logged in user");
            usersPage.UserProfileListPanel.SelectProfile(userModel.Profile);

            Step("4. Remove all blocked actions if any then add 'Modify control program' then save");
            usersPage.UserProfileDetailsPanel.RemoveAllBlockedActions();

            Step("5. Expected Saving is successful");
            var blockedActionsCount = usersPage.UserProfileDetailsPanel.GetBlockedActionsCount();
            VerifyEqual("5. Verify blocked actions are removed completely", 0, blockedActionsCount);
            usersPage.UserProfileDetailsPanel.SelectBlockActionsDropDown(xmlBlockAction);
            usersPage.UserProfileDetailsPanel.ClickSaveButton();
            usersPage.WaitForHeaderMessageDisplayed();
            var actualHeaderMessage = usersPage.GetHeaderMessage();
            VerifyEqual("5. Verify profile updated message successfully", "The user profile has been updated successfully", actualHeaderMessage);
            usersPage.WaitForHeaderMessageDisappeared();

            Step("6. Switch to Scheduling Manager app");
            var schedulingManagerPage = usersPage.AppBar.SwitchTo(App.SchedulingManager) as SchedulingManagerPage;

            Step("7. Select Control program tab and select a control program");
            schedulingManagerPage.SchedulingManagerPanel.SelectTab("Control program");
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("8. Expected Verify all fields are unmodifiable:");
            foreach (var controlProgramItem in xmlControlPrograms)
            {
                schedulingManagerPage.SchedulingManagerPanel.SelectControlProgram(controlProgramItem);
                schedulingManagerPage.WaitForPreviousActionComplete();

                var template = schedulingManagerPage.ControlProgramEditorPanel.GetTemplateValue();
                Step(string.Format(" ****** Template: '{0}' ******", template));
                VerifyControlProgramUnmodifiable(schedulingManagerPage, template);
            }

            Step("9. Switch back to Users app then remove the blocked action set previously then save");
            usersPage = usersPage.AppBar.SwitchTo(App.Users) as UsersPage;
            usersPage.UserProfileDetailsPanel.RemoveAllBlockedActions();
            usersPage.UserProfileDetailsPanel.ClickSaveButton();
            usersPage.WaitForPreviousActionComplete();
            usersPage.WaitForHeaderMessageDisappeared();

            Step("10. Switch back to Scheduling Manager app and select the previously selected control program");
            schedulingManagerPage = usersPage.AppBar.SwitchTo(App.SchedulingManager) as SchedulingManagerPage;
            schedulingManagerPage.SchedulingManagerPanel.SelectTab("Calendar");
            schedulingManagerPage.WaitForPreviousActionComplete();
            schedulingManagerPage.SchedulingManagerPanel.SelectTab("Control program");
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("11. Expected Verify all fields are modifiable:");
            foreach (var controlProgramItem in xmlControlPrograms)
            {
                schedulingManagerPage.SchedulingManagerPanel.SelectControlProgram(controlProgramItem);
                schedulingManagerPage.WaitForPreviousActionComplete();

                var template = schedulingManagerPage.ControlProgramEditorPanel.GetTemplateValue();
                Step(string.Format(" ****** Template: '{0}' ******", template));
                VerifyControlProgramModifiable(schedulingManagerPage, template);
            }

            try
            {
                //remove new profile and user
                DeleteUserAndProfile(userModel);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("TS 1.10.24 Blocked Actions - Modify device's inventory")]
        [NonParallelizable]
        public void TS1_10_24()
        {
            var testData = GetTestDataOfTestTS1_10_24();
            var xmlBlockAction = testData["BlockAction"];
            var xmlGeozonePath = testData["Geozone"];

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenBackOfficeApp();
            var backOfficePage = loginPage.LoginAsValidUserToBackOffice(Settings.Users["admin"].Username, Settings.Users["admin"].Password);
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.EquipmentInventory);
            backOfficePage.BackOfficeDetailsPanel.TickEquipmentDeviceLocationCheckbox(false);
            backOfficePage.BackOfficeDetailsPanel.ClickSaveButton();
            backOfficePage.WaitForPreviousActionComplete();
            var desktopPage = Browser.NavigateToLoggedInCMS();
            var userModel = CreateNewProfileAndUser();
            desktopPage = SLVHelper.LogoutAndLogin(desktopPage, userModel.Username, userModel.Password);
            desktopPage.InstallAppsIfNotExist(App.Users, App.EquipmentInventory);

            Step("1. Go to Users app");
            Step("2. Expected Users page is routed and loaded successfully");
            var usersPage = desktopPage.GoToApp(App.Users) as UsersPage;

            Step("3. Browse to the profile of current logged in user");
            usersPage.UserProfileListPanel.SelectProfile(userModel.Profile);

            Step("4. Remove all blocked actions if any then add 'Modify device's inventory' then save");
            usersPage.UserProfileDetailsPanel.RemoveAllBlockedActions();

            Step("5. Expected Saving is successful");
            var blockedActionsCount = usersPage.UserProfileDetailsPanel.GetBlockedActionsCount();
            VerifyEqual("5. Verify blocked actions are removed completely", 0, blockedActionsCount);
            usersPage.UserProfileDetailsPanel.SelectBlockActionsDropDown(xmlBlockAction);
            usersPage.UserProfileDetailsPanel.ClickSaveButton();
            usersPage.WaitForHeaderMessageDisplayed();
            var actualHeaderMessage = usersPage.GetHeaderMessage();
            VerifyEqual("5. Verify profile updated message successfully", "The user profile has been updated successfully", actualHeaderMessage);
            usersPage.WaitForHeaderMessageDisappeared();

            Step("6. Switch to Equipment Inventory app app");
            var equipmentInventoryPage = usersPage.AppBar.SwitchTo(App.EquipmentInventory) as EquipmentInventoryPage;

            Step("7. Select a geozone");
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(xmlGeozonePath);
            var devices = equipmentInventoryPage.GeozoneTreeMainPanel.GetChildDeviceNamesOfSelectedNode();
            var device = devices.PickRandom();

            Step("8. Expected Geozone editor panel appears:");
            equipmentInventoryPage.WaitForEditorPanelDisplayed();
            Step(" - New items menu is not displayed");
            VerifyEqual("8. Verify New items menu is not displayed", false, equipmentInventoryPage.GeozoneEditorPanel.IsAddNewMenuDisplayed());

            Step(" - Import and Replace Nodes in More menu are not displayed; Export is displayed");
            var moreMenuItems = equipmentInventoryPage.GeozoneEditorPanel.GetListOfMoreMenuItems();
            VerifyEqual("8. Verify Import and Replace Nodes in More menu are not displayed", true, !moreMenuItems.Contains("Import") && !moreMenuItems.Contains("Replace Nodes"));
            VerifyEqual("8. Verify Export is displayed", true, moreMenuItems.Contains("Export"));

            Step(" - Save, Remove buttons are not displayed");
            VerifyEqual("8. Verify Save, Remove buttons are not displayed", true, !equipmentInventoryPage.GeozoneEditorPanel.IsSaveButtonDisplayed() && !equipmentInventoryPage.GeozoneEditorPanel.IsDeleteButtonDisplayed());

            Step(" - Update geozone boundary button is not displayed");
            VerifyEqual("8. Verify Update geozone boundary button is not displayed", false, equipmentInventoryPage.GeozoneEditorPanel.IsUpdateBoundsButtonDisplayed());

            Step(" - All editing fields are disabled");
            VerifyEqual("8. Verify Name Input is disable", true, equipmentInventoryPage.GeozoneEditorPanel.IsNameInputReadOnly());
            VerifyEqual("8. Verify Parent Input is disable", true, equipmentInventoryPage.GeozoneEditorPanel.IsParentGeozoneInputReadOnly());
            VerifyEqual("8. Verify Lat Min Input is disable", true, equipmentInventoryPage.GeozoneEditorPanel.IsLatMinInputReadOnly());
            VerifyEqual("8. Verify Lat Max Input is disable", true, equipmentInventoryPage.GeozoneEditorPanel.IsLatMaxInputReadOnly());
            VerifyEqual("8. Verify Long Min Input is disable", true, equipmentInventoryPage.GeozoneEditorPanel.IsLongMinInputReadOnly());
            VerifyEqual("8. Verify Long Max Input is disable", true, equipmentInventoryPage.GeozoneEditorPanel.IsLongMaxInputReadOnly());
            VerifyEqual("8. Verify All Virtual Energy Consumption inputs are disable ", true, equipmentInventoryPage.GeozoneEditorPanel.AreVirtualEnergyConsumptionInputsReadOnly());

            Step("9. Select a device");
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(device);

            Step("10. Expected Device editor panel appears:");
            equipmentInventoryPage.WaitForEditorPanelDisplayed();
            var deviceType = equipmentInventoryPage.GeozoneTreeMainPanel.GetSelectedNodeType();

            Step(" - Replace Lamp, Replace Node, Duplicate, Save, Remove buttons are not displayed(Replace Lamp, Replace Node only for Streetlight and Switch)");
            VerifyDeviceButtonsAreNotDisplayed(equipmentInventoryPage);

            Step(" - All editing fields are disabled");
            VerifyEqual("10. Verify Name Input is disable", true, equipmentInventoryPage.DeviceEditorPanel.IsNameInputReadOnly());
            VerifyEqual("10. Verify Parent Input is disable", true, equipmentInventoryPage.DeviceEditorPanel.IsParentGeozoneInputReadOnly());
            VerifyEqual("10. Verify Latitude Input is disable", true, equipmentInventoryPage.DeviceEditorPanel.IsLatitudeInputReadOnly());
            VerifyEqual("10. Verify Longitude Input is disable", true, equipmentInventoryPage.DeviceEditorPanel.IsLongitudeInputReadOnly());
            var areInputsReadOnly = equipmentInventoryPage.DeviceEditorPanel.AreInputsReadOnly();
            var areDropDownsReadOnly = equipmentInventoryPage.DeviceEditorPanel.AreDropDownsReadOnly();
            var areCheckboxesReadOnly = equipmentInventoryPage.DeviceEditorPanel.AreCheckboxesReadOnly();
            VerifyTrue("[SLV-3686] 10. Verify All editing fields are disabled", areInputsReadOnly && areDropDownsReadOnly && areCheckboxesReadOnly, "Disabled", "Not disabled");

            Step("11. Switch back to Users app then remove the blocked action set previously then save");
            usersPage = usersPage.AppBar.SwitchTo(App.Users) as UsersPage;
            usersPage.UserProfileListPanel.SelectProfile(userModel.Profile);
            usersPage.UserProfileDetailsPanel.RemoveAllBlockedActions();
            usersPage.UserProfileDetailsPanel.ClickSaveButton();
            usersPage.WaitForPreviousActionComplete();
            usersPage.WaitForHeaderMessageDisappeared();

            Step("12. Switch back to Equipment Inventory app and select the previously selected geozone");
            equipmentInventoryPage = usersPage.AppBar.SwitchTo(App.EquipmentInventory) as EquipmentInventoryPage;

            Step("13. Select a geozone");
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(xmlGeozonePath);

            Step("14. Expected Geozone editor panel appears:");
            equipmentInventoryPage.WaitForEditorPanelDisplayed();
            Step(" - All undisplayed items in step #8 are displayed");
            Step(" - All displayed items in step #8 are remained displayed");
            VerifyEqual("14. Verify New items menu is displayed", true, equipmentInventoryPage.GeozoneEditorPanel.IsAddNewMenuDisplayed());
            VerifyEqual("14. Verify Save, Remove buttons are displayed", true, equipmentInventoryPage.GeozoneEditorPanel.IsSaveButtonDisplayed() && equipmentInventoryPage.GeozoneEditorPanel.IsDeleteButtonDisplayed());
            VerifyEqual("14. Verify Update geozone boundary button is displayed", true, equipmentInventoryPage.GeozoneEditorPanel.IsUpdateBoundsButtonDisplayed());

            moreMenuItems = equipmentInventoryPage.GeozoneEditorPanel.GetListOfMoreMenuItems();
            VerifyEqual("14. Verify Import and Replace Nodes in More menu are displayed", true, moreMenuItems.Contains("Import") && moreMenuItems.Contains("Replace Nodes"));
            VerifyEqual("14. Verify Export is displayed", true, moreMenuItems.Contains("Export"));

            Step(" - Editable fields are enabled");
            VerifyEqual("14. Verify Name Input is editable", false, equipmentInventoryPage.GeozoneEditorPanel.IsNameInputReadOnly());
            VerifyEqual("14. Verify Parent DropDown is editable", false, equipmentInventoryPage.GeozoneEditorPanel.IsParentGeozoneDropDownReadOnly());
            VerifyEqual("14. Verify Lat Min Input is still disable", true, equipmentInventoryPage.GeozoneEditorPanel.IsLatMinInputReadOnly());
            VerifyEqual("14. Verify Lat Max Input is still disable", true, equipmentInventoryPage.GeozoneEditorPanel.IsLatMaxInputReadOnly());
            VerifyEqual("14. Verify Long Min Input is still disable", true, equipmentInventoryPage.GeozoneEditorPanel.IsLongMinInputReadOnly());
            VerifyEqual("14. Verify Long Max Input is still disable", true, equipmentInventoryPage.GeozoneEditorPanel.IsLongMaxInputReadOnly());
            VerifyEqual("14. Verify All Virtual Energy Consumption inputs are editable ", true, equipmentInventoryPage.GeozoneEditorPanel.AreVirtualEnergyConsumptionInputsEditable());

            Step("15. Select a device");
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(device);

            Step("16. Expected Device editor panel appears:");
            equipmentInventoryPage.WaitForEditorPanelDisplayed();

            Step(" - Replace Lamp, Replace Node, Duplicate, Save, Remove buttons are displayed (Replace Lamp, Replace Node only for Streetlight and Switch)");
            VerifyDeviceButtonsAreDisplayed(equipmentInventoryPage);

            Step(" - All editing fields are enabled");
            Step(" - Note: Except for 'Identifier' fields of the following devices: Cabinet Controller, Controller, Switch, Streetlight. This field is always read-only.");            
            VerifyEqual("16. Verify Name Input is editable", false, equipmentInventoryPage.DeviceEditorPanel.IsNameInputReadOnly());
            VerifyEqual("16. Verify Parent Input is still disable", true, equipmentInventoryPage.DeviceEditorPanel.IsParentGeozoneInputReadOnly());
            VerifyEqual("16. Verify Latitude Input is still disable", true, equipmentInventoryPage.DeviceEditorPanel.IsLatitudeInputReadOnly());
            VerifyEqual("16. Verify Longitude Input is still disable", true, equipmentInventoryPage.DeviceEditorPanel.IsLongitudeInputReadOnly());
            
            var areInputsEditable = equipmentInventoryPage.DeviceEditorPanel.AreInputsEditable(deviceType);
            var areDropDownsEditable = equipmentInventoryPage.DeviceEditorPanel.AreDropDownsEditable();
            var areCheckboxesEditable = equipmentInventoryPage.DeviceEditorPanel.AreCheckboxesEditable();
            VerifyEqual(string.Format("[{0}] 16. Verify All editing fields are enabled", deviceType.ToString()), true, areInputsEditable && areDropDownsEditable && areCheckboxesEditable);

            try
            {
                //remove new profile and user
                DeleteUserAndProfile(userModel);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("TS 1.10.25 Blocked Actions - Allow SchedulerManager Advanced Mode")]
        [NonParallelizable]
        public void TS1_10_25()
        {
            var testData = GetTestDataOfTestTS1_10_25();
            var xmlBlockAction = testData["BlockAction"].ToString();
            var xmlControlProgramAdvancedMode = testData["ControlProgramAdvancedMode"].ToString();
            var xmlControlProgramsNotAdvancedMode = testData["ControlProgramsNotAdvancedMode"] as List<string>;
            var xmlTemplates = testData["Templates"] as List<string>;

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var userModel = CreateNewProfileAndUser();
            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(userModel.Username, userModel.Password);
            desktopPage.InstallAppsIfNotExist(App.Users, App.SchedulingManager);

            Step("1. Go to Users app");
            Step("2. Expected Users page is routed and loaded successfully");
            var usersPage = desktopPage.GoToApp(App.Users) as UsersPage;

            Step("3. Browse to the profile of current logged in user");
            usersPage.UserProfileListPanel.SelectProfile(userModel.Profile);

            Step("4. Remove all blocked actions if any then add 'Allow SchedulerManager Advanced Mode' then save");
            usersPage.UserProfileDetailsPanel.RemoveAllBlockedActions();

            Step("5. Expected Saving is successful");
            var blockedActionsCount = usersPage.UserProfileDetailsPanel.GetBlockedActionsCount();
            VerifyEqual("5. Verify blocked actions are removed completely", 0, blockedActionsCount);
            usersPage.UserProfileDetailsPanel.SelectBlockActionsDropDown(xmlBlockAction);
            usersPage.UserProfileDetailsPanel.ClickSaveButton();
            usersPage.WaitForHeaderMessageDisplayed();
            var actualHeaderMessage = usersPage.GetHeaderMessage();
            VerifyEqual("5. Verify profile updated message successfully", "The user profile has been updated successfully", actualHeaderMessage);
            usersPage.WaitForHeaderMessageDisappeared();

            Step("6. Switch to Scheduling Manager app");
            var schedulingManagerPage = usersPage.AppBar.SwitchTo(App.SchedulingManager) as SchedulingManagerPage;

            Step("7. Select Control program tab and select a program whose Template is 'Advanced mode'");
            schedulingManagerPage.SchedulingManagerPanel.SelectTab("Control program");
            schedulingManagerPage.WaitForPreviousActionComplete();
            schedulingManagerPage.SchedulingManagerPanel.SelectControlProgram(xmlControlProgramAdvancedMode);
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("8. Expected Template field is disabled and empty");
            var actualTemplate = schedulingManagerPage.ControlProgramEditorPanel.GetTemplateValue();
            var isTemplateDropDownReadOnly = schedulingManagerPage.ControlProgramEditorPanel.IsTemplateDropDownReadOnly();
            VerifyEqual("8. Verify Template field is disabled", true, isTemplateDropDownReadOnly);
            VerifyEqual("8. Verify Template field is empty", string.Empty, actualTemplate);

            Step("9. Select Control program tab and select a program whose Template is not 'Advanced mode'");
            var controlProgramNotAdvancedMode = xmlControlProgramsNotAdvancedMode.PickRandom();
            schedulingManagerPage.SchedulingManagerPanel.SelectControlProgram(controlProgramNotAdvancedMode);
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("10. Expected Template field is disabled. The text is its template name");
            isTemplateDropDownReadOnly = schedulingManagerPage.ControlProgramEditorPanel.IsTemplateDropDownReadOnly();
            actualTemplate = schedulingManagerPage.ControlProgramEditorPanel.GetTemplateValue();
            VerifyEqual("10. Verify Template field is disabled", true, isTemplateDropDownReadOnly);
            VerifyEqual("10. Verify The text is its template name", true, xmlTemplates.Contains(actualTemplate));

            Step("11. Create a new control program");
            schedulingManagerPage.SchedulingManagerPanel.ClickAddControlProgramButton();
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("12. Expected 'Advanced mode' is not present in Template list: Template dropdown list has items:");
            Step(" - Astro ON/OFF");
            Step(" - Astro ON/OFF and fixed time events");
            Step(" - Always ON");
            Step(" - Always OFF");
            Step(" - Day fixed time events");
            var expectedTemplates = new List<string>(xmlTemplates);
            expectedTemplates.Remove("Advanced mode");
            var actualTemplates = schedulingManagerPage.ControlProgramEditorPanel.GetListOfTemplateItems();
            VerifyEqual("12. Verify 'Advanced mode' is not present in Template list", false, actualTemplates.Contains("Advanced mode"));
            VerifyEqual("12. Verify Template dropdown list is as expected", expectedTemplates, actualTemplates, false);

            Step("13. Switch back to Users app then remove the blocked action set previously then save");
            usersPage = usersPage.AppBar.SwitchTo(App.Users) as UsersPage;
            usersPage.UserProfileDetailsPanel.RemoveAllBlockedActions();
            usersPage.UserProfileDetailsPanel.ClickSaveButton();
            usersPage.WaitForPreviousActionComplete();
            usersPage.WaitForHeaderMessageDisappeared();

            Step("14. Switch back to Scheduling Manager app");
            schedulingManagerPage = usersPage.AppBar.SwitchTo(App.SchedulingManager) as SchedulingManagerPage;

            Step("15. Select Control program tab and select a program whose Template is 'Advanced mode'");
            schedulingManagerPage.SchedulingManagerPanel.SelectControlProgram(xmlControlProgramAdvancedMode);
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("16. Expected Template field is disabled. The text is 'Advanced mode'");
            actualTemplate = schedulingManagerPage.ControlProgramEditorPanel.GetTemplateValue();
            isTemplateDropDownReadOnly = schedulingManagerPage.ControlProgramEditorPanel.IsTemplateDropDownReadOnly();
            VerifyEqual("16. Verify Template field is disabled", true, isTemplateDropDownReadOnly);
            VerifyEqual("16. Verify The text is 'Advanced mode'", "Advanced mode", actualTemplate);

            Step("17. Select Control program tab and select a program whose Template is not 'Advanced mode'");
            controlProgramNotAdvancedMode = xmlControlProgramsNotAdvancedMode.PickRandom();
            schedulingManagerPage.SchedulingManagerPanel.SelectControlProgram(controlProgramNotAdvancedMode);
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("18. Expected Template field is disabled. The text is its template name");
            isTemplateDropDownReadOnly = schedulingManagerPage.ControlProgramEditorPanel.IsTemplateDropDownReadOnly();
            actualTemplate = schedulingManagerPage.ControlProgramEditorPanel.GetTemplateValue();
            VerifyEqual("18. Verify Template field is disabled", true, isTemplateDropDownReadOnly);
            VerifyEqual("18. Verify The text is its template name", true, xmlTemplates.Contains(actualTemplate));

            Step("19. Create a new control program");
            schedulingManagerPage.SchedulingManagerPanel.ClickAddControlProgramButton();
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("20. Expected 'Advanced mode' is available in Template list: Template dropdown list has items:");
            Step(" - Astro ON/OFF");
            Step(" - Astro ON/OFF and fixed time events");
            Step(" - Always ON");
            Step(" - Always OFF");
            Step(" - Day fixed time events");
            Step(" - Advanced mode");
            actualTemplates = schedulingManagerPage.ControlProgramEditorPanel.GetListOfTemplateItems();
            VerifyEqual("20. Verify 'Advanced mode' is available in Template list", true, actualTemplates.Contains("Advanced mode"));
            VerifyEqual("20. Verify Template dropdown list is as expected", xmlTemplates, actualTemplates, false);

            try
            {
                //remove new profile and user
                DeleteUserAndProfile(userModel);
            }
            catch { }
        }    

        [Test, DynamicRetry]
        [Description("TS 1.11.1 Blocked Actions - All actions")]
        [NonParallelizable]
        public void TS1_11_01()
        {
            var testData = GetTestDataOfTestTS1_11_1();
            var xmlRTCGeoZone = testData["RTC_GeoZone"].ToString();
            var xmlRTCDevices = testData["RTC_Devices"] as List<DeviceModel>;
            var xmlHttpRequests = testData["RTC_Requests"] as List<string>;
            var xmlUnauthorizedActions = testData["RTC_ExpectedUnauthorizedAction"] as List<string>;
            var xmlRTCController = testData["RTC_Controller"].ToString();
            var xmlEIGeoZone = testData["EI_GeoZone"].ToString();
            var xmlEIController = testData["EI_Controller"].ToString();
            var xmlEIGeoZoneRemove = testData["EI_GeoZoneRemove"].ToString();
            var xmlEIGeoZoneAllTypes = testData["EI_GeoZoneAllTypes"].ToString();
            var xmlUniqueAddress = testData["UniqueAddress"].ToString();
            var xmlControlProgramAdvancedMode = testData["ControlProgramAdvancedMode"].ToString();
            var xmlControlProgramsNotAdvancedMode = testData["ControlProgramsNotAdvancedMode"] as List<string>;
            var xmlTemplates = testData["Templates"] as List<string>;

            var rtcDevicesLongLat = xmlRTCDevices.Select(p => string.Format("{0};{1}", p.Longitude, p.Latitude)).ToList();

            var allControlPrograms = new List<string>(xmlControlProgramsNotAdvancedMode);
            allControlPrograms.Add(xmlControlProgramAdvancedMode);

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - User has not any blocked action");
            Step(" - Segment Controller has control technology 'iLON Segment Controller Version 4' and the status is AUTOMATIC and the Output 1 is 'ON'");
            Step(" - Streetlight which is in MANNUAL mode");            
            Step("**** Precondition ****\n");

            var userModel = CreateNewProfileAndUser();
            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(userModel.Username, userModel.Password);
            desktopPage.InstallAppsIfNotExist(App.Users, App.EquipmentInventory, App.RealTimeControl, App.SchedulingManager);

            Step(string.Format("Set status is AUTOMATIC and the Output 1 is 'ON', the Output 2 is 'OFF' for cotnroller '{0}'", xmlRTCController));
            var realtimeControlPage = desktopPage.GoToApp(App.RealTimeControl) as RealTimeControlPage;
            realtimeControlPage.GeozoneTreeMainPanel.SelectNode(string.Format(@"{0}\{1}", xmlRTCGeoZone, xmlRTCController));
            realtimeControlPage.WaitForControllerWidgetDisplayed(xmlRTCController);
            realtimeControlPage.ControllerWidgetPanel.ClickInputsOutputsButton();
            realtimeControlPage.ControllerWidgetPanel.WaitForInputsOutputsPanelDisplayed();
            realtimeControlPage.ControllerWidgetPanel.InputsOutputsWidgetPanel.ClickOutput1OnButton();
            realtimeControlPage.WaitForPopupDialogDisplayed();
            realtimeControlPage.Dialog.ClickYesButton();
            realtimeControlPage.ControllerWidgetPanel.WaitForCommandCompleted();
            realtimeControlPage.ControllerWidgetPanel.InputsOutputsWidgetPanel.ClickOutput2OffButton();
            realtimeControlPage.WaitForPopupDialogDisplayed();
            realtimeControlPage.Dialog.ClickYesButton();
            realtimeControlPage.ControllerWidgetPanel.WaitForCommandCompleted();
            realtimeControlPage.ControllerWidgetPanel.ClickInputsOutputsButton();
            realtimeControlPage.ControllerWidgetPanel.WaitForInputsOutputsPanelDisappeared();
            realtimeControlPage.ControllerWidgetPanel.ClickClockButton();
            realtimeControlPage.ControllerWidgetPanel.WaitForCommandCompleted();

            var expectedDevice = xmlRTCDevices.PickRandom().Name;
            Step(string.Format("Execute a command for streetlight '{0}' is in MANNUAL mode", expectedDevice));
            realtimeControlPage.GeozoneTreeMainPanel.SelectNode(expectedDevice);
            realtimeControlPage.WaitForStreetlightWidgetDisplayed(expectedDevice);
            realtimeControlPage.StreetlightWidgetPanel.ExecuteRandomDimming(RealtimeCommand.DimOff, RealtimeCommand.DimOn);
            realtimeControlPage.StreetlightWidgetPanel.WaitForCommandCompleted();

            Step("1. In Desktop page, click Users tile");
            Step("2. Expected Users page is routed");
            var usersPage = realtimeControlPage.AppBar.SwitchTo(App.Users) as UsersPage;

            Step("3. Select the user profile which User A belongs to");
            usersPage.UserProfileListPanel.SelectProfile(userModel.Profile);

            Step("In Profile Detail widget, remove all 'Block Actions' items if any");
            usersPage.UserProfileDetailsPanel.RemoveAllBlockedActions();
            Step("Expected All blocked actions are removed off");
            var blockedActionsCount = usersPage.UserProfileDetailsPanel.GetBlockedActionsCount();
            VerifyEqual("3. Verify blocked actions are removed completely", 0, blockedActionsCount);

            Step("4. In Profile Detail widget, verify blocked action list");
            Step("5. [SLV-3050] Expected 23 blocked actions are:");
            Step(" - Commission a controller");
            Step(" - Commission calendar");
            Step(" - Commission device");
            Step(" - Delete a device");
            Step(" - Delete a geozone");
            Step(" - Delete multiple devices at one");
            Step(" - Execute a command on a list of devices");
            Step(" - Execute a command on one device");
            Step(" - Exit from manual mode on a segment controller");
            Step(" - Exit from manual mode on a device");
            Step(" - Exit manual mode on a list of devices");
            Step(" - Import *.csv or *.sdp file");
            Step(" - Modify calendar");
            Step(" - Modify control program");
            Step(" - Modify device's inventory");
            Step(" - Replace a Light Point Controller");
            Step(" - Replace a lamp");
            Step(" - Send real-time commands to a group of light points");
            Step(" - Send real-time commands to a light point");
            Step(" - Switch a segment controller output");
            Step(" - Allow SchedulerManager Advanced Mode");
            Step(" - Synchronize segment controller’s clock");
            Step(" - Update a geozone");

            var blockedActionList = usersPage.UserProfileDetailsPanel.GetListOfBlockedActionsName();
            VerifyEqual("5. Verify 23 blocked actions are existing", 23, blockedActionList.Count);

            Step("6. Block all actions then click Save icon");
            usersPage.UserProfileDetailsPanel.SelectBlockedActions(blockedActionList);
            usersPage.UserProfileDetailsPanel.ClickSaveButton();
            usersPage.WaitForHeaderMessageDisplayed();
            var actualHeaderMessage = usersPage.GetHeaderMessage();

            Step("7. Expected Profile is updated successfully with message 'The user profile has been updated successfully'");
            VerifyEqual("7. Verify profile updated message successfully", "The user profile has been updated successfully", actualHeaderMessage);
            usersPage.WaitForHeaderMessageDisappeared();

            Step("8. Refresh browser");
            desktopPage = Browser.RefreshLoggedInCMS();

            Step("9. Go to Users app and select that profile again");
            usersPage = desktopPage.GoToApp(App.Users) as UsersPage;

            Step("10. Expected Saved blocked actions are present in the selected blocked actions");
            usersPage.UserProfileListPanel.SelectProfile(userModel.Profile);
            var currentBlockedActions = usersPage.UserProfileDetailsPanel.GetBlockedActionsName();
            VerifyEqual("10. Verify Saved blocked actions are present in the selected blocked actions", 23, currentBlockedActions.Count);

            var oddDay = DateTime.Now.Day % 2 != 0;
            if (oddDay)
            {
                usersPage.UserProfileDetailsPanel.RemoveBlockedAction("Modify control program");
                usersPage.UserProfileDetailsPanel.RemoveBlockedAction("Modify device's inventory");
                usersPage.UserProfileDetailsPanel.ClickSaveButton();
                usersPage.WaitForPreviousActionComplete();
                usersPage.WaitForHeaderMessageDisappeared();
            }

            Step("11. Go to 'Equipement Inventory', 'Real-time Control' and 'Scheduling Manager' pages to in turn perform blocked action (For details, see test cases from TS 1.10.1 to Ts 1.10.17)");
            Step("12. Expected Expected result of each action is as the expected result of test cases from TS 1.10.1 to Ts 1.10.17");

            #region Real-time Control

            realtimeControlPage = usersPage.AppBar.SwitchTo(App.RealTimeControl) as RealTimeControlPage;
            realtimeControlPage.GeozoneTreeMainPanel.SelectNode(string.Format(@"{0}\{1}", xmlRTCGeoZone, xmlRTCController));
            realtimeControlPage.WaitForControllerWidgetDisplayed(xmlRTCController);
            realtimeControlPage.WaitForHeaderMessageDisappeared();

            #region TS 1.10.6, TS 1.10.15

            Step("--> TS 1.10.6 Blocked Actions - Exit from manual mode on a segment controller");
            var notedLastUpdateTime = realtimeControlPage.ControllerWidgetPanel.GetLastUpdateTimeText();
            realtimeControlPage.ControllerWidgetPanel.ClickClockButton();
            realtimeControlPage.ControllerWidgetPanel.WaitForCommandCompleted();
            var actualLastUpdateTime = realtimeControlPage.ControllerWidgetPanel.GetLastUpdateTimeText();
            VerifyEqual("[1.10.6] Verify The time value at the bottom right corner of the widget is not changed (1st click)", notedLastUpdateTime, actualLastUpdateTime);
            realtimeControlPage.ControllerWidgetPanel.ClickClockButton();
            realtimeControlPage.ControllerWidgetPanel.WaitForCommandCompleted();
            actualLastUpdateTime = realtimeControlPage.ControllerWidgetPanel.GetLastUpdateTimeText();
            VerifyEqual("[1.10.6] Verify The time value at the bottom right corner of the widget is not changed (2nd click)", notedLastUpdateTime, actualLastUpdateTime);

            Step("--> TS 1.10.15 Blocked Actions - Switch a segment controller output");
            realtimeControlPage.ControllerWidgetPanel.ClickInputsOutputsButton();
            realtimeControlPage.ControllerWidgetPanel.WaitForInputsOutputsPanelDisplayed();
            realtimeControlPage.ControllerWidgetPanel.InputsOutputsWidgetPanel.ClickOutput1OffButton();
            realtimeControlPage.WaitForPopupDialogDisplayed();
            realtimeControlPage.Dialog.ClickYesButton();
            realtimeControlPage.ControllerWidgetPanel.WaitForCommandCompleted();
            VerifyEqual("[1.10.15] Verify Output 1, The text 'AUTO' is not changed to 'MANUAL'", "AUTO", realtimeControlPage.ControllerWidgetPanel.InputsOutputsWidgetPanel.GetOutput1ModeText());
            VerifyTrue("[1.10.15] Verify Output 1, The GREEN circle is not changed to GREY", realtimeControlPage.ControllerWidgetPanel.InputsOutputsWidgetPanel.IsOutput1StatusGREEN(), "GREEN", "NOT GREEN");
            realtimeControlPage.ControllerWidgetPanel.InputsOutputsWidgetPanel.ClickOutput2OnButton();
            realtimeControlPage.WaitForPopupDialogDisplayed();
            realtimeControlPage.Dialog.ClickYesButton();
            realtimeControlPage.ControllerWidgetPanel.WaitForCommandCompleted();
            VerifyEqual("[1.10.15] Verify Output 2, The text 'AUTO' is not changed to 'MANUAL'", "AUTO", realtimeControlPage.ControllerWidgetPanel.InputsOutputsWidgetPanel.GetOutput2ModeText());
            VerifyTrue("[1.10.15] Verify Output 2, The GREY circle is not changed to GREEN", realtimeControlPage.ControllerWidgetPanel.InputsOutputsWidgetPanel.IsOutput2StatusGRAY(), "GRAY", "NOT GRAY");
            realtimeControlPage.ControllerWidgetPanel.ClickInputsOutputsButton();
            realtimeControlPage.ControllerWidgetPanel.WaitForInputsOutputsPanelDisappeared();

            #endregion

            #region TS 1.10.16

            Step("--> TS 1.10.16 Blocked Actions - Synchronize segment controller’s clock");
            realtimeControlPage.ControllerWidgetPanel.ClickInformationButton();
            realtimeControlPage.ControllerWidgetPanel.WaitForInformationWidgetPanelDisplayed();
            realtimeControlPage.ControllerWidgetPanel.InformationWidgetPanel.ClickDateTimeSynchronizeButton();
            realtimeControlPage.WaitForPreviousActionComplete();
            realtimeControlPage.WaitForPopupMessageDisplayed();
            var expectedErrorMessage = string.Format("User '{0}' is not authorized to perform 'SLVControllerAPI!setSystemTime'!", userModel.Username);
            var actualErrorMessage = realtimeControlPage.Dialog.GetMessageText();
            VerifyEqual("[1.10.16] Verify A error message is displayed", expectedErrorMessage, actualErrorMessage);
            realtimeControlPage.Dialog.ClickOkButton();
            realtimeControlPage.WaitForPopupMessageDisappeared();

            #endregion            

            #region TS 1.10.5, TS 1.10.11, TS 1.10.12, TS 1.10.14, TS 1.10.7

            realtimeControlPage.GeozoneTreeMainPanel.SelectNode(expectedDevice);
            realtimeControlPage.WaitForStreetlightWidgetDisplayed(expectedDevice);

            Step("--> TS 1.10.5 Blocked Actions - Execute a command on one device");
            Step("--> TS 1.10.11 Blocked Actions - Send a Switch ON command to a device");
            Step("--> TS 1.10.12 Blocked Actions - Send a Switch OFF command to a device");
            Step("--> TS 1.10.14 Blocked Actions - Send real-time commands to a light point");
            var notedCommandText = realtimeControlPage.StreetlightWidgetPanel.GetRealtimeCommandValueText();
            var notedFeedbackText = realtimeControlPage.StreetlightWidgetPanel.GetRealtimeFeedbackValueText();
            var notedSliderPosition = realtimeControlPage.StreetlightWidgetPanel.GetIndicatorCursorPositionValue();
            realtimeControlPage.StreetlightWidgetPanel.ExecuteCommand(RealtimeCommand.DimOn);
            realtimeControlPage.StreetlightWidgetPanel.WaitForCommandCompleted();
            var actualCommandText = realtimeControlPage.StreetlightWidgetPanel.GetRealtimeCommandValueText();
            var actualFeedbackText = realtimeControlPage.StreetlightWidgetPanel.GetRealtimeFeedbackValueText();
            var actualSliderPosition = realtimeControlPage.StreetlightWidgetPanel.GetIndicatorCursorPositionValue();
            VerifyEqual("[1.10.5-11-12-14] Verify Clicking ON is not affected. (Command Text does not change)", notedCommandText, actualCommandText);
            VerifyEqual("[1.10.5-11-12-14] Verify Clicking ON is not affected. (Feedback Text does not change)", notedFeedbackText, actualFeedbackText);
            VerifyEqual("[1.10.5-11-12-14] Verify Slider is NOT movable", notedSliderPosition, actualSliderPosition);

            realtimeControlPage.StreetlightWidgetPanel.ExecuteCommand(RealtimeCommand.DimOff);
            realtimeControlPage.StreetlightWidgetPanel.WaitForCommandCompleted();
            actualCommandText = realtimeControlPage.StreetlightWidgetPanel.GetRealtimeCommandValueText();
            actualFeedbackText = realtimeControlPage.StreetlightWidgetPanel.GetRealtimeFeedbackValueText();
            actualSliderPosition = realtimeControlPage.StreetlightWidgetPanel.GetIndicatorCursorPositionValue();
            VerifyEqual("[1.10.5-11-12-14] Verify Clicking OFF is not affected. (Command Text does not change)", notedCommandText, actualCommandText);
            VerifyEqual("[1.10.5-11-12-14] Verify Clicking OFF is not affected. (Feedback Text does not change)", notedFeedbackText, actualFeedbackText);
            VerifyEqual("[1.10.5-11-12-14] Verify Slider is NOT movable", notedSliderPosition, actualSliderPosition);

            realtimeControlPage.StreetlightWidgetPanel.ExecuteRandomDimming10To90ByCursor();
            actualSliderPosition = realtimeControlPage.StreetlightWidgetPanel.GetIndicatorCursorPositionValue();
            VerifyEqual("[1.10.5-11-12-14] Verify Execute command by cursor is not affected. (Command Text does not change)", notedCommandText, actualCommandText);
            VerifyEqual("[1.10.5-11-12-14] Verify Execute command by cursor is not affected. (Feedback Text does not change)", notedFeedbackText, actualFeedbackText);
            VerifyEqual("[1.10.5-11-12-14] Verify Slider is NOT movable", notedSliderPosition, actualSliderPosition);

            realtimeControlPage.StreetlightWidgetPanel.ExecuteRandomDimming(RealtimeCommand.DimOff, RealtimeCommand.DimOn);
            realtimeControlPage.StreetlightWidgetPanel.WaitForCommandCompleted();
            actualCommandText = realtimeControlPage.StreetlightWidgetPanel.GetRealtimeCommandValueText();
            actualFeedbackText = realtimeControlPage.StreetlightWidgetPanel.GetRealtimeFeedbackValueText();
            VerifyEqual("[1.10.5-11-12-14] Verify Execute preconfigured command, Command Text is not change", notedCommandText, actualCommandText);
            VerifyEqual("[1.10.5-11-12-14] Verify Execute preconfigured command, Feedback Text is not change", notedFeedbackText, actualFeedbackText);

            Step("--> TS 1.10.7 Blocked Actions - Exit from manual mode on a device");
            realtimeControlPage.StreetlightWidgetPanel.ClickClockButton();
            realtimeControlPage.WaitForPreviousActionComplete();
            actualCommandText = realtimeControlPage.StreetlightWidgetPanel.GetRealtimeCommandValueText();
            actualFeedbackText = realtimeControlPage.StreetlightWidgetPanel.GetRealtimeFeedbackValueText();
            var actualClockText = realtimeControlPage.StreetlightWidgetPanel.GetClockText();
            VerifyTrue("[1.10.7] Verify The text is not changed to 'AUTOMATIC'", !actualClockText.Equals("AUTOMATIC"), "Not change to 'AUTOMATIC'", actualClockText);
            VerifyEqual("[1.10.7] Verify Command is unchanged", notedCommandText, actualCommandText);
            VerifyEqual("[1.10.7] Verify Feedback is unchanged", notedFeedbackText, actualFeedbackText);

            #endregion

            #endregion

            #region Equipment Inventory

            var equipmentInventoryPage = realtimeControlPage.AppBar.SwitchTo(App.EquipmentInventory) as EquipmentInventoryPage;
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(string.Format(@"{0}\{1}", xmlEIGeoZone, xmlEIController));

            #region TS 1.10.1, TS 1.10.2

            Step("--> TS 1.10.1 Blocked Actions - Commission a controller");
            var isCommissionButtonDisplayed = equipmentInventoryPage.ControllerEditorPanel.IsCommissionButtonDisplayed();
            VerifyEqual("[TS 1.10.1] Verify Commission button is not available any more", false, isCommissionButtonDisplayed);

            Step("--> TS 1.10.2 Blocked Actions - Delete a device");
            VerifyEqual("[1.10.2] Verify 'Delete' button is not available any more", true, !equipmentInventoryPage.DeviceEditorPanel.IsDeleteButtonDisplayed());

            #endregion

            #region TS 1.10.3, TS 1.10.17

            Step("--> TS 1.10.3 Blocked Actions - Delete a geozone");
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(xmlEIGeoZoneRemove);
            if (oddDay)
            {
                equipmentInventoryPage.GeozoneEditorPanel.ClickDeleteButton();
                equipmentInventoryPage.WaitForPopupDialogDisplayed();
                equipmentInventoryPage.Dialog.ClickYesButton();
                equipmentInventoryPage.WaitForPreviousActionComplete();
                equipmentInventoryPage.WaitForPopupDialogDisplayed();
                expectedErrorMessage = string.Format("User '{0}' is not authorized to perform 'SLVAssetManagementAPI!deleteGeoZone'!", userModel.Username);
                actualErrorMessage = equipmentInventoryPage.Dialog.GetMessageText();
                VerifyEqual("[1.10.3] Verify A error message is displayed", expectedErrorMessage, actualErrorMessage);
                equipmentInventoryPage.Dialog.ClickOkButton();
                equipmentInventoryPage.WaitForPopupDialogDisappeared();
            }
            else
            {
                VerifyEqual("[1.10.3] Verify Delete button is not displayed by TS 1.10.24 Blocked Actions - Modify device's inventory", true, !equipmentInventoryPage.GeozoneEditorPanel.IsDeleteButtonDisplayed());
            }

            Step("--> TS 1.10.17 Blocked Actions - Update a geozone");
            if (oddDay)
            {
                equipmentInventoryPage.GeozoneEditorPanel.ClickUpdateBoundsButton();
                equipmentInventoryPage.Map.WaitForRecorderDisplayed();
                equipmentInventoryPage.Map.DragMapToRandomLocation();
                equipmentInventoryPage.WaitForPreviousActionComplete();
                equipmentInventoryPage.Map.ClickRecorderButton();
                equipmentInventoryPage.Map.WaitForRecorderDisappeared();
                equipmentInventoryPage.WaitForPreviousActionComplete();
                equipmentInventoryPage.WaitForPopupDialogDisplayed();
                expectedErrorMessage = string.Format("User '{0}' is not authorized to perform 'SLVAssetManagementAPI!updateGeoZone'!", userModel.Username);
                actualErrorMessage = equipmentInventoryPage.Dialog.GetMessageText();
                VerifyEqual("[1.10.17] Verify A error message is displayed", expectedErrorMessage, actualErrorMessage);
                equipmentInventoryPage.Dialog.ClickOkButton();
                equipmentInventoryPage.WaitForPopupDialogDisappeared();
            }
            else
            {
                VerifyEqual("[1.10.17] Verify Update geozone boundary button is not displayed by TS 1.10.24 Blocked Actions - Modify device's inventory", true, !equipmentInventoryPage.GeozoneEditorPanel.IsUpdateBoundsButtonDisplayed());
            }

            #endregion

            #region TS 1.10.9, TS 1.10.10, TS 1.10.19          
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(xmlEIGeoZoneAllTypes);
            equipmentInventoryPage.WaitForEditorPanelDisplayed();
            var allDevices = equipmentInventoryPage.GeozoneTreeMainPanel.GetChildDeviceNamesOfSelectedNode();
            var device = allDevices.PickRandom();

            var moreMenuItems = equipmentInventoryPage.GeozoneEditorPanel.GetListOfMoreMenuItems();
            Step("--> TS 1.10.21 Blocked Actions - Import csv or sdp file");
            VerifyEqual("[1.10.21] Verify Import menu item is not displayed in More menu", false, moreMenuItems.Contains("Import"));

            if (!oddDay)
            {
                Step("--> TS 1.10.24 Blocked Actions - Modify device's inventory");
                VerifyEqual("[1.10.24] Verify New items menu is not displayed", false, equipmentInventoryPage.GeozoneEditorPanel.IsAddNewMenuDisplayed());
                VerifyEqual("[1.10.24] Verify Import and Replace Nodes in More menu are not displayed", true, !moreMenuItems.Contains("Import") && !moreMenuItems.Contains("Replace Nodes"));
                VerifyEqual("[1.10.24] Verify Export is displayed", true, moreMenuItems.Contains("Export"));
                VerifyEqual("[1.10.24] Verify Save, Remove buttons are not displayed", true, !equipmentInventoryPage.GeozoneEditorPanel.IsSaveButtonDisplayed() && !equipmentInventoryPage.GeozoneEditorPanel.IsDeleteButtonDisplayed());
                VerifyEqual("[1.10.24] Verify Update geozone boundary button is not displayed", false, equipmentInventoryPage.GeozoneEditorPanel.IsUpdateBoundsButtonDisplayed());
                VerifyEqual("[1.10.24] Verify Name Input is disable", true, equipmentInventoryPage.GeozoneEditorPanel.IsNameInputReadOnly());
                VerifyEqual("[1.10.24] Verify Parent Input is disable", true, equipmentInventoryPage.GeozoneEditorPanel.IsParentGeozoneInputReadOnly());
                VerifyEqual("[1.10.24] Verify Lat Min Input is disable", true, equipmentInventoryPage.GeozoneEditorPanel.IsLatMinInputReadOnly());
                VerifyEqual("[1.10.24] Verify Lat Max Input is disable", true, equipmentInventoryPage.GeozoneEditorPanel.IsLatMaxInputReadOnly());
                VerifyEqual("[1.10.24] Verify Long Min Input is disable", true, equipmentInventoryPage.GeozoneEditorPanel.IsLongMinInputReadOnly());
                VerifyEqual("[1.10.24] Verify Long Max Input is disable", true, equipmentInventoryPage.GeozoneEditorPanel.IsLongMaxInputReadOnly());

                equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(device);
                equipmentInventoryPage.WaitForEditorPanelDisplayed();
                var deviceType = equipmentInventoryPage.GeozoneTreeMainPanel.GetSelectedNodeType();
                var isSaveButtonDisplayed = equipmentInventoryPage.DeviceEditorPanel.IsSaveButtonDisplayed();
                var isDeleteButtonDisplayed = equipmentInventoryPage.DeviceEditorPanel.IsDeleteButtonDisplayed();
                var isReplaceLampButtonDisplayed = equipmentInventoryPage.DeviceEditorPanel.IsReplaceLampButtonDisplayed();
                var isReplaceNodeButtonDisplayed = equipmentInventoryPage.DeviceEditorPanel.IsReplaceNodeButtonDisplayed();
                var isDuplicateButtonDisplayed = equipmentInventoryPage.DeviceEditorPanel.IsDuplicateButtonDisplayed();
                isCommissionButtonDisplayed = equipmentInventoryPage.DeviceEditorPanel.IsCommissionButtonDisplayed();
                VerifyEqual("[1.10.24] Verify Replace Lamp, Replace Node, Duplicate, Save, Remove buttons are not displayed", true, !isSaveButtonDisplayed && !isDeleteButtonDisplayed && !isReplaceLampButtonDisplayed && !isReplaceNodeButtonDisplayed && !isDuplicateButtonDisplayed);
                VerifyEqual("[1.10.24] Verify Name Input is disable", true, equipmentInventoryPage.DeviceEditorPanel.IsNameInputReadOnly());
                VerifyEqual("[1.10.24] Verify Parent Input is disable", true, equipmentInventoryPage.DeviceEditorPanel.IsParentGeozoneInputReadOnly());
                VerifyEqual("[1.10.24] Verify Latitude Input is disable", true, equipmentInventoryPage.DeviceEditorPanel.IsLatitudeInputReadOnly());
                VerifyEqual("[1.10.24] Verify Longitude Input is disable", true, equipmentInventoryPage.DeviceEditorPanel.IsLongitudeInputReadOnly());
                var areInputsReadOnly = equipmentInventoryPage.DeviceEditorPanel.AreInputsReadOnly();
                var areDropDownsReadOnly = equipmentInventoryPage.DeviceEditorPanel.AreDropDownsReadOnly();
                var areCheckboxesReadOnly = equipmentInventoryPage.DeviceEditorPanel.AreCheckboxesReadOnly();
                VerifyEqual("[1.10.24] Verify All editing fields are disabled", true, areInputsReadOnly && areDropDownsReadOnly && areCheckboxesReadOnly);
            }
            else
            {
                Step("--> Ignore : TS 1.10.24 Blocked Actions - Modify device's inventory");
                equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(device);
                equipmentInventoryPage.WaitForEditorPanelDisplayed();
            }

            Step("--> TS 1.10.9 Blocked Actions - Replace a lamp");
            VerifyEqual("[1.10.9] Verify Replace A Lamp button is not available any more", false, equipmentInventoryPage.DeviceEditorPanel.IsReplaceLampButtonDisplayed());

            Step("--> TS 1.10.10 Blocked Actions - Replace a Light Point Controller");
            VerifyEqual("[1.10.10] Verify Replace Node button is not available any more", false, equipmentInventoryPage.DeviceEditorPanel.IsReplaceNodeButtonDisplayed());

            Step("--> TS 1.10.19 Blocked Actions - Commission device");
            VerifyEqual("[1.10.19] Verify Commission device icon button is not available any more", false, isCommissionButtonDisplayed);

            Step("--> TS 1.10.20 Blocked Actions - Delete multiple devices at once");
            var allTypesGeozoneName = xmlEIGeoZoneAllTypes.GetChildName();
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(allTypesGeozoneName);
            allDevices = equipmentInventoryPage.GeozoneTreeMainPanel.GetChildDeviceNamesOfSelectedNode();
            var anyDevices = allDevices.PickRandom(2);
            var devicesLongLat = new List<string>();
            foreach (var deviceName in anyDevices)
            {
                var deviceModel = GetDevice(deviceName, allTypesGeozoneName);
                if (deviceModel != null) devicesLongLat.Add(string.Format("{0};{1}", deviceModel.Longitude, deviceModel.Latitude));
            }

            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(allTypesGeozoneName);
            equipmentInventoryPage.Map.SelectDevicesGL(devicesLongLat);
            equipmentInventoryPage.WaitForMultipleDevicesEditorDisplayed();
            equipmentInventoryPage.PressDeleteKey();
            var hasPopupDialog = equipmentInventoryPage.HasPopupDialogDisplayed();
            VerifyEqual("[1.10.20] Verify No dialog appears", false, hasPopupDialog);
            VerifyEqual("[1.10.20] Verify Delete button is not displayed", false, equipmentInventoryPage.MultipleDevicesEditorPanel.IsDeleteButtonDisplayed());

            #endregion

            #endregion

            #region Scheduling Manager          

            var schedulingManagerPage = usersPage.AppBar.SwitchTo(App.SchedulingManager) as SchedulingManagerPage;
            schedulingManagerPage.SchedulingManagerPanel.SelectTab("Control program");
            schedulingManagerPage.WaitForPreviousActionComplete();

            if (!oddDay)
            {
                Step("--> TS 1.10.23 Blocked Actions - Modify control program");
                Step("[1.10.23] Verify all fields are unmodifiable:");
                foreach (var controlProgramItem in allControlPrograms)
                {
                    schedulingManagerPage.SchedulingManagerPanel.SelectControlProgram(controlProgramItem);
                    schedulingManagerPage.WaitForPreviousActionComplete();

                    var template = schedulingManagerPage.ControlProgramEditorPanel.GetTemplateValue();
                    Step(string.Format(" ****** Template: '{0}' ******", template));
                    VerifyControlProgramUnmodifiable(schedulingManagerPage, template);
                }
            }
            else
            {
                Step("--> Ignore : TS 1.10.23 Blocked Actions - Modify control program");
            }

            Step("--> TS 1.10.25 Blocked Actions - Allow SchedulerManager Advanced Mode");
            schedulingManagerPage.SchedulingManagerPanel.SelectControlProgram(xmlControlProgramAdvancedMode);
            schedulingManagerPage.WaitForPreviousActionComplete();
            var actualTemplate = schedulingManagerPage.ControlProgramEditorPanel.GetTemplateValue();
            var isTemplateDropDownReadOnly = schedulingManagerPage.ControlProgramEditorPanel.IsTemplateDropDownReadOnly();
            VerifyEqual("[1.10.25] Verify Template field is disabled", true, isTemplateDropDownReadOnly);
            VerifyEqual("[1.10.25] Verify Template field is empty", string.Empty, actualTemplate);

            var controlProgramNotAdvancedMode = xmlControlProgramsNotAdvancedMode.PickRandom();
            schedulingManagerPage.SchedulingManagerPanel.SelectControlProgram(controlProgramNotAdvancedMode);
            schedulingManagerPage.WaitForPreviousActionComplete();
            isTemplateDropDownReadOnly = schedulingManagerPage.ControlProgramEditorPanel.IsTemplateDropDownReadOnly();
            actualTemplate = schedulingManagerPage.ControlProgramEditorPanel.GetTemplateValue();
            VerifyEqual("[1.10.25] Verify Template field is disabled", true, isTemplateDropDownReadOnly);
            VerifyEqual("[1.10.25] Verify The text is its template name", true, xmlTemplates.Contains(actualTemplate));

            if (oddDay)
            {
                schedulingManagerPage.SchedulingManagerPanel.ClickAddControlProgramButton();
                schedulingManagerPage.WaitForPreviousActionComplete();
                var expectedTemplates = new List<string>(xmlTemplates);
                expectedTemplates.Remove("Advanced mode");
                var actualTemplates = schedulingManagerPage.ControlProgramEditorPanel.GetListOfTemplateItems();
                VerifyEqual("[1.10.25] Verify 'Advanced mode' is not present in Template list", false, actualTemplates.Contains("Advanced mode"));
                VerifyEqual("[1.10.25] Verify Template dropdown list is as expected", expectedTemplates, actualTemplates, false);
            }
            else
            {
                Step("--> New buttons is not displayed by TS 1.10.25 Blocked Actions - Allow SchedulerManager Advanced Mode");
            }

            Step("--> TS 1.10.18 Blocked Actions - Commission calendar");
            schedulingManagerPage.SchedulingManagerPanel.SelectTab("Calendar");
            schedulingManagerPage.WaitForPreviousActionComplete();
            var isCommissionCalendarToolbarButtonVisible = schedulingManagerPage.SchedulingManagerPanel.IsCommissionCalendarButtonVisible();
            VerifyEqual("[1.10.18] Verify Commission calendar icon button is not displayed", false, isCommissionCalendarToolbarButtonVisible);
            var dimmingGroupList = schedulingManagerPage.SchedulingManagerPanel.GetListOfCalendarName();
            dimmingGroupList.Remove("Default Group");
            var anyDimming = dimmingGroupList.PickRandom();
            schedulingManagerPage.SchedulingManagerPanel.SelectCalendar(anyDimming);
            schedulingManagerPage.WaitForPreviousActionComplete();
            isCommissionCalendarToolbarButtonVisible = schedulingManagerPage.SchedulingManagerPanel.IsCommissionCalendarButtonVisible();
            VerifyEqual("[1.10.18] Verify Commission calendar icon button is still not displayed", false, isCommissionCalendarToolbarButtonVisible);

            Step("--> TS 1.10.22 Blocked Actions - Modify calendar");
            var isAddCalendarToolbarButtonVisible = schedulingManagerPage.SchedulingManagerPanel.IsAddCalendarButtonVisible();
            var isDeleteCalendarToolbarButtonVisible = schedulingManagerPage.SchedulingManagerPanel.IsDeleteCalendarButtonVisible();
            var isDuplicateCalendarToolbarButtonVisible = schedulingManagerPage.SchedulingManagerPanel.IsDuplicateCalendarButtonVisible();
            VerifyEqual("[1.10.22] Verify New, Duplicate, Remove buttons are not displayed", true, isAddCalendarToolbarButtonVisible == false && isDeleteCalendarToolbarButtonVisible == false && isDuplicateCalendarToolbarButtonVisible == false);
            var isNameInputReadOnly = schedulingManagerPage.CalendarEditorPanel.IsNameInputReadOnly();
            var isDescriptionInputReadOnly = schedulingManagerPage.CalendarEditorPanel.IsDescriptionInputReadOnly();
            VerifyEqual("[1.10.22] Verify Name, Description: read-only", true, isNameInputReadOnly && isDescriptionInputReadOnly);
            schedulingManagerPage.CalendarEditorPanel.ClickRandomCalendarDate();
            var hasPopupDialogDisplayed = schedulingManagerPage.HasPopupDialogDisplayed();
            VerifyEqual("[1.10.22] Verify Control programs editor doesn't appear", false, hasPopupDialogDisplayed);
            var isCalendarItemsButtonVisible = schedulingManagerPage.CalendarEditorPanel.IsCalendarItemsButtonVisible();
            VerifyEqual("[1.10.22] Verify Calendar items button: visible", true, isCalendarItemsButtonVisible);
            schedulingManagerPage.CalendarEditorPanel.ClickCalendarItemsButton();
            schedulingManagerPage.WaitForPopupDialogDisplayed();
            var isCancelButtonVisible = schedulingManagerPage.CalendarEditorItemsPopupPanel.IsCancelButtonVisible();
            var isSaveButtonVisible = schedulingManagerPage.CalendarEditorItemsPopupPanel.IsSaveButtonVisible();
            var isDeleteButtonVisible = schedulingManagerPage.CalendarEditorItemsPopupPanel.IsDeleteButtonVisible();
            var isUpButtonVisible = schedulingManagerPage.CalendarEditorItemsPopupPanel.IsUpButtonVisible();
            var isDownButtonVisible = schedulingManagerPage.CalendarEditorItemsPopupPanel.IsDownButtonVisible();
            VerifyEqual("[1.10.22] Verify Delete, Up, Down buttons are not available", true, isDeleteButtonVisible == false && isUpButtonVisible == false && isDownButtonVisible == false);
            VerifyEqual("[1.10.22] Verify Cancel and Save button are not displayed as well", true, isCancelButtonVisible == false && isSaveButtonVisible == false);
            schedulingManagerPage.ControlProgramItemsPopupPanel.ClickCloseButton();
            schedulingManagerPage.WaitForPopupDialogDisappeared();
            var isClearButtonCalendarEditorVisible = schedulingManagerPage.CalendarEditorPanel.IsClearButtonVisible();
            var isSaveButtonCalendarEditorVisible = schedulingManagerPage.CalendarEditorPanel.IsSaveButtonVisible();
            VerifyEqual("[1.10.22] Verify Clear and Save button are not displayed", true, isClearButtonCalendarEditorVisible == false && isSaveButtonCalendarEditorVisible == false);
            schedulingManagerPage.CalendarEditorPanel.ClickYearBeforeButton();
            Step(" --> Ignore by SLV-3120");
            schedulingManagerPage.CalendarEditorPanel.ClickYearAfterButton();
            Step(" --> Ignore by SLV-3120");

            #endregion

            try
            {
                //remove new profile and user
                DeleteUserAndProfile(userModel);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("TS 1.12.1 Drag and drop user in Users app")]
        public void TS1_12_01()
        {
            var username = Settings.Users["admin"].Username;
            var password = Settings.Users["admin"].Password;
            var profile = Settings.Users["admin"].Profile;

            var testData = GetTestDataOfTestTS1_12_1();
            var expectedProfileA = testData["A_ProfileName"];
            var expectedProfileALanguageCode = testData["A_LanguageCode"];
            var expectedProfileASkin = testData["A_Skin"];
            var expectedUserA = testData["A_User"];
            var expectedUserAPasword = testData["A_Password"];
            var expectedUserAEmail = testData["A_Email"];
            var expectedProfileB = testData["B_ProfileName"];
            var expectedProfileBLanguageCode = testData["B_LanguageCode"];
            var expectedProfileBSkin = testData["B_Skin"];
            var expectedUserB = testData["B_User"];
            var expectedUserBPasword = testData["B_Password"];
            var expectedUserBEmail = testData["B_Email"];

            var timetamp = DateTime.Now.Timestamp();
            expectedProfileA = string.Format("{0}{1}", expectedProfileA, timetamp);
            expectedUserA = string.Format("{0}{1}", expectedUserA, timetamp);
            expectedProfileB = string.Format("{0}{1}", expectedProfileB, timetamp);
            expectedUserB = string.Format("{0}{1}", expectedUserB, timetamp);
            var profileALanguage = GetLanguageName(expectedProfileALanguageCode);
            var profileBLanguage = GetLanguageName(expectedProfileBLanguageCode);

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(username, password);
            desktopPage.InstallAppsIfNotExist(App.Users);

            Step("1. In Desktop page, click Users tile");
            Step("2. Expected Users page is routed");
            var usersPage = desktopPage.GoToApp(App.Users) as UsersPage;

            Step("3. Create and save 2 profiles called A and B with different skins and languages");
            Step("4. Create and save a user in profile A (called UserA) and another in profile B (called UserB)");
            usersPage.CreateNewProfileAndUser(expectedProfileA, profileALanguage, expectedProfileASkin, expectedUserA, expectedUserAPasword, expectedUserAEmail);
            usersPage.CreateNewProfileAndUser(expectedProfileB, profileBLanguage, expectedProfileBSkin, expectedUserB, expectedUserBPasword, expectedUserBEmail);

            Step("5. Drag UserA and drop it into profile B");
            usersPage.DragAndDropUserToProfile(expectedProfileA, expectedProfileB, expectedUserA);

            Step("6. Expected UserA is not in profile A's user list any more. It is now present in profile B's user list");
            var usersListName = usersPage.UserListPanel.GetUsersListName();
            VerifyEqual("6. Verify users list is empty", 0, usersListName.Count);
            VerifyEqual(string.Format("6. Verify {0} is not in {1}'s user list any more", expectedUserA, expectedProfileA), false, usersListName.Exists(p => p.Equals(expectedUserA)));

            Step("7. Drag UserB and drop it into profile A");
            usersPage.DragAndDropUserToProfile(expectedProfileB, expectedProfileA, expectedUserB);

            Step("8. Expected UserB is not in profile B's user list any more. It is now present in profile A's user list");
            usersListName = usersPage.UserListPanel.GetUsersListName();
            VerifyEqual(string.Format("[SC-1933] 8. Verify {0} has 1 user after drag and drop user {1} from {0} to {2}", expectedProfileB, expectedUserB, expectedProfileA), 1, usersListName.Count);
            VerifyEqual(string.Format("[SC-1933] 8. Verify {0} is not in {1}'s user list any more", expectedUserB, expectedProfileB), false, usersListName.Exists(p => p.Equals(expectedUserB)));

            usersPage.UserProfileListPanel.SelectProfile(expectedProfileA);
            usersListName = usersPage.UserListPanel.GetUsersListName();
            VerifyEqual(string.Format("[SC-1933] 8. Verify {0} has 1 user after drag and drop user {1} from {2} to {0}", expectedProfileA, expectedUserB, expectedProfileB), 1, usersListName.Count);
            VerifyEqual(string.Format("[SC-1933] 8. Verify {0} has already contained {1}", expectedProfileA, expectedUserB), true, usersListName.Exists(p => p.Equals(expectedUserB)));

            Step("9. Refresh browser then go to Users app again");
            desktopPage = Browser.RefreshLoggedInCMS();

            Step("10. Expected Users page is routed and loaded successfully");
            usersPage = desktopPage.GoToApp(App.Users) as UsersPage;

            Step("11. Verify profile A");
            usersPage.UserProfileListPanel.SelectProfile(expectedProfileA);

            Step("12. Expected UserB is present in profile A's user list while UserA is not");
            usersListName = usersPage.UserListPanel.GetUsersListName();
            VerifyEqual(string.Format("12. Verify {0} has 1 user", expectedProfileA), 1, usersListName.Count);
            VerifyEqual(string.Format("[SC-1933] 12. Verify {0} is present in {1}'s user list", expectedUserB, expectedProfileA), true, usersListName.Exists(p => p.Equals(expectedUserB)));

            Step("13. Verify profile B");
            usersPage.UserProfileListPanel.SelectProfile(expectedProfileB);

            Step("14. Expected UserA is present in profile B's user list while UserB is not");
            usersListName = usersPage.UserListPanel.GetUsersListName();
            VerifyEqual(string.Format("14. Verify {0} has 1 user", expectedProfileB), 1, usersListName.Count);
            VerifyEqual(string.Format("[SC-1933] 14. Verify {0} is present in {1}'s user list", expectedUserA, expectedProfileB), true, usersListName.Exists(p => p.Equals(expectedUserA)));

            Step("15. Log out then log in with UserA");
            desktopPage = SLVHelper.LogoutAndLogin(usersPage, expectedUserA, expectedUserAPasword);

            Step("16. Expected Skin and language of SLV CMS are of profile B");
            Step(" o Skin is verified by checking presence of link element and its href attribute contains skin name, e.g. < link type='text /css' rel='stylesheet' media='screen' href='http://5.196.91.118:8080/qa/groundcontrol/skins/streetdark/css/style.min.css' >");
            Step(" o Language is verified by checking presence of link element and its href attribute contains skin name, e.g. < script type='text /javascript' src='http://5.196.91.118:8080/qa/groundcontrol/js/lib/JQuery/globalize/cultures/globalize.culture.en-US.js'>");
            var actualSkin = SLVHelper.GetCurrentSkin();
            var actualLanguage = SLVHelper.GetCurrentLanguageCode();
            VerifyEqual(string.Format("[{0}] 16. Verify SLV CMS is in '{1}' skin", expectedUserA, expectedProfileBSkin), expectedProfileBSkin, actualSkin);
            VerifyEqual(string.Format("[{0}] 16. Verify SLV CMS is in '{1}' language", expectedUserA, expectedProfileBLanguageCode), expectedProfileBLanguageCode, actualLanguage);

            Step("17. Log out then log in with UserB");
            desktopPage = SLVHelper.LogoutAndLogin(desktopPage, expectedUserB, expectedUserBPasword);

            Step("18. Expected Skin and language of SLV CMS are of profile A");      
            actualSkin = SLVHelper.GetCurrentSkin();
            actualLanguage = SLVHelper.GetCurrentLanguageCode();
            VerifyEqual(string.Format("[{0}] 18. Verify SLV CMS is in '{1}' skin", expectedUserB, expectedProfileASkin), expectedProfileASkin, actualSkin);
            VerifyEqual(string.Format("[{0}] 18. Verify SLV CMS is in '{1}' language", expectedUserB, expectedProfileALanguageCode), expectedProfileALanguageCode, actualLanguage);

            try
            {
                DeleteProfile(expectedProfileA);
                DeleteProfile(expectedProfileB);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("TS 1.13.1 Logging in Log Viewer app")]
        public void TS1_13_01()
        {
            var testData = GetTestDataOfTestTS1_13_1();
            var geozone = testData["Geozone"].ToString();
            var streetlights = testData["Streetlights"] as List<DeviceModel>;
            var streetlight = streetlights.Select(p => p.Name).PickRandom();

            Step("**** Precondition ****");
            Step(" - Realtime-Control, Log Viewer app has been installed successfully");
            Step("**** Precondition ****\n");          

            Step("1. Create new user and login with it");
            var userModel = CreateNewProfileAndUser();
            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(userModel.Username, userModel.Password);
            desktopPage.InstallAppsIfNotExist(App.LogViewer, App.RealTimeControl);

            Step("2. Go to Real-time Control app to turn ON/OFF or set dimming level a device");
            var realtimeControlPage = desktopPage.GoToApp(App.RealTimeControl) as RealTimeControlPage;
            realtimeControlPage.GeozoneTreeMainPanel.SelectNode(string.Format(@"{0}\{1}", geozone, streetlight));
            realtimeControlPage.StreetlightWidgetPanel.ExecuteRandomDimming();
            realtimeControlPage.StreetlightWidgetPanel.WaitForCommandCompleted();
            realtimeControlPage.StreetlightWidgetPanel.ExecuteRandomDimming();
            realtimeControlPage.StreetlightWidgetPanel.WaitForCommandCompleted();
            realtimeControlPage.StreetlightWidgetPanel.ExecuteRandomDimming();
            realtimeControlPage.StreetlightWidgetPanel.WaitForCommandCompleted();

            Step("3. Go to Log Viewer app");
            desktopPage = Browser.RefreshLoggedInCMS();
            var logViewersPage = desktopPage.GoToApp(App.LogViewer) as LogViewerPage;
            logViewersPage.WaitForPreviousActionComplete();

            Step("4. Select and browse to the logged user");
            logViewersPage.UserTreePanel.SelectNode(string.Format(@"{0}\{1}", userModel.Profile, userModel.Username));
            logViewersPage.WaitForPreviousActionComplete();

            Step("5. Set day for From & To fields then click Execute");
            var fromDate = Settings.GetServerTime().AddDays(-1); 
            var toDate = fromDate.AddDays(2);
            logViewersPage.GridPanel.EnterFromDateInput(fromDate);
            logViewersPage.GridPanel.EnterToDateInput(toDate);
            logViewersPage.GridPanel.ClickExecuteButton();
            logViewersPage.WaitForPreviousActionComplete();

            Step("6. Expected In the grid, there are records whose date within the time noted at step #5 to now:");
            Step(" o Date: within From & To");
            Step(" o User: current logged user name ");
            Step(" o Action: pattern 'com.dotv.streetlightserver.api.*'");
            VerifyLogViewerGridRecords(logViewersPage, fromDate, toDate, userModel.Username);

            try
            {
                //remove new profile and user
                DeleteUserAndProfile(userModel);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("TS 1.14.1 Weather widget")]
        public void TS1_14_01()
        {
            var testData = GetTestDataOfTestTS1_14_1();
            var searchCityListInput = testData["SearchCity"] as List<string>;

            Step("**** Precondition ****");
            Step(" - Create a new profile containing a new user");
            Step(" - Login to CMS successully by the new user");
            Step("**** Precondition ****\n");

            var userModel = CreateNewProfileAndUser();            
            Step("1. After logging in, check all the widgets on the Desktop");
            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(userModel.Username, userModel.Password);

            Step("2. Expected The Weather widget is not added to the desktop");
            VerifyEqual("2. Verify The Weather widget is not added to the desktop", false, desktopPage.CheckWidgetInstalled(Widget.Weather));

            Step("3. Install the Weather widget");
            desktopPage.InstallWidgetsIfNotExist(Widget.Weather);

            //Init city
            desktopPage.OpenWidget(Widget.Weather);
            desktopPage.WeatherLocationPanel.FocusLocationInput();
            desktopPage.WeatherLocationPanel.EnterLocationInput("Ho Chi Minh");
            desktopPage.WeatherLocationPanel.ClickSearchButton();
            desktopPage.WeatherLocationPanel.WaitForSearchResultDisplayed();
            desktopPage.WeatherLocationPanel.SelectFirstAddress();
            desktopPage.WeatherLocationPanel.WaitForSearchResultDisappeared();
            desktopPage.WaitForWeatherLocationDisappeared();

            var currentCityName = string.Empty;
            foreach (var city in searchCityListInput)
            {
                Step("4. From Desktop app, click Weather widget");
                desktopPage.OpenWidget(Widget.Weather);

                Step("5. Expected A widget appears from the right side that allows to search for location");
                desktopPage.WaitForWeatherLocationDisplayed();

                Step("6. Note the name of current city in Weather widget");
                currentCityName = desktopPage.GetFirstWeatherWidgetCityName();

                Step("7. Enter a city name which is different from the one noted at step #3 then click Search button");
                desktopPage.WeatherLocationPanel.FocusLocationInput();
                desktopPage.WeatherLocationPanel.EnterLocationInput(city);
                desktopPage.WeatherLocationPanel.ClickSearchButton();
                desktopPage.WeatherLocationPanel.WaitForSearchResultDisplayed();

                Step("8. Expected A list of cities that match input value is displayed");
                var listAddress = desktopPage.WeatherLocationPanel.GetAddressListText();
                VerifyEqual("8. Verify A list of cities that match input value", true, listAddress.Exists(p => p.Contains(city)));

                Step("9. Select a city in the city list");
                desktopPage.WeatherLocationPanel.SelectFirstAddress();
                desktopPage.WeatherLocationPanel.WaitForSearchResultDisappeared();
                desktopPage.WaitForWeatherLocationDisappeared();

                Step("10. Expected Weather widget now displays weather detail of the city that has been selected");
                desktopPage.WaitForWeatherLoaded();
                var newCityName = desktopPage.GetFirstWeatherWidgetCityName();
                VerifyEqual(string.Format("Verify new weather city name {0} is different with old city name {1}", newCityName, currentCityName), false, string.Equals(currentCityName, newCityName, StringComparison.InvariantCultureIgnoreCase));
                VerifyEqual("10. Verify Weather widget now displays weather the city that has been selected", true, string.Equals(newCityName, city, StringComparison.InvariantCultureIgnoreCase));

                currentCityName = newCityName;
            }

            Step("11. Refresh browser");
            desktopPage = Browser.RefreshLoggedInCMS();

            Step("12. Expected Weather widget still displays weather details of the city selected before refreshing browser");
            desktopPage.WaitForWeatherLoaded();
            var actualCityName = desktopPage.GetFirstWeatherWidgetCityName();
            VerifyEqual(string.Format("12. Verify acutal city name {0} is similiar with current city name {1}", actualCityName, currentCityName), true, string.Equals(currentCityName, actualCityName, StringComparison.InvariantCultureIgnoreCase));

            try
            {
                //remove new profile and user
                DeleteUserAndProfile(userModel);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("TS 1.15.1 Clock widget")]
        public void TS1_15_01()
        {
            var testData = GetTestDataOfTestTS1_15_1();
            var expectedGeoZone = testData["GeoZone"];
            var expectedController = testData["Controller"];

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - Clock widget has been installed successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.DeleteWidgets(Widget.Clock);
            desktopPage.InstallWidgetsIfNotExist(Widget.Clock);

            Step("1. From Desktop app, click Clock widget");
            desktopPage.OpenWidget(Widget.Clock);

            Step("2. Expected A widget with geozone tree appears from the right side that allows to select a controllerId");
            desktopPage.WaitForClockGeozoneTreeDisplayed();

            Step("3. Browse to and select a controllerId in any geozone");
            desktopPage.GeozoneTreeWidgetPanel.SelectNode(string.Format(@"{0}\{1}", expectedGeoZone, expectedController));
            desktopPage.WaitForPreviousActionComplete();

            Step("4. Expected Clock widget now displays clock for the selected controllerId. The title in the clock is the name of the selected controllerId");
            var controllerName = desktopPage.GetFirstClockWidgetControllerName();
            VerifyEqual("4. Verify The title in the clock is the name of the selected controllerId", expectedController, controllerName);

            Step("5. Refresh browser");
            desktopPage = Browser.RefreshLoggedInCMS();

            Step("6. Expected Clock widget still displays clock of the previously selected controller");
            controllerName = desktopPage.GetFirstClockWidgetControllerName();
            VerifyEqual("6. Verify Clock widget still displays clock of the previously selected controller", expectedController, controllerName);
        }

        [Test, DynamicRetry]
        [Description("TS 1.16.1 Sunrise Sunset Times widget")]
        public void TS1_16_01()
        {
            var testData = GetTestDataOfTestTS1_16_1();
            var expectedGeoZones = testData["GeoZones"] as List<string>;            

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - Sunrise Sunset Times widget has been installed successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            Step("1. Open Desktop page");
            Step("2. **Expected** Widget app is present in Desktop page as shown in the picture below");
            desktopPage.DeleteWidgets(Widget.SunriseSunsetTimes);
            desktopPage.InstallWidgetsIfNotExist(Widget.SunriseSunsetTimes);
            var currentGeozone = desktopPage.GetFirstSunriseSunsetWidgetGeozoneName();
            var expectedGeoZone = expectedGeoZones.Where(p => !p.Equals(currentGeozone)).PickRandom();

            Step("3. Click 'Sunrise Sunset Times' tile");
            desktopPage.OpenWidget(Widget.SunriseSunsetTimes);

            Step("4. **Expected** 'Sunrise Sunset Times' widget appears from the right side with geozone tree");
            desktopPage.WaitForSunriseSunsetGeozoneTreeDisplayed();

            Step("5. Select a geozone");
            desktopPage.GeozoneTreeWidgetPanel.SelectNode(expectedGeoZone);

            Step("6. **Expected** 'Sunrise Sunset Times' widget disappears and 'Sunrise Sunset Times' tile updates displaying with the new selected geozone");
            desktopPage.WaitForSunriseSunsetLoaded();
            VerifySunriseSunsetInfo(desktopPage, expectedGeoZone);

            Step("7. Refresh browser");
            desktopPage = Browser.RefreshLoggedInCMS();

            Step("8. Expected Sunrise Sunset Times widget still displays time of the previously selected geozone");
            VerifySunriseSunsetInfo(desktopPage, expectedGeoZone);
        }

        #endregion //Test Cases

        #region Private methods        

        private string GetProfileUpdatedMessage(string languageCode = "en-US")
        {
            string result = string.Empty;

            switch (languageCode)
            {
                case "en-US":
                case "en_US":
                    result = "The user profile has been updated successfully";
                    break;
                case "en-GB":
                case "en_GB":
                    result = "The user profile has been updated successfully";
                    break;
                case "fr-FR":
                case "fr_FR":
                    result = "Le profil utilisateur a été mis à jour avec succès";
                    break;
                case "es-ES":
                case "es_ES":
                    result = "The user profile has been updated successfully";
                    break;
                case "de-DE":
                case "de_DE":
                    result = "Benutzerprofil wurde erfolgreich aktualisiert.";
                    break;
                case "zh-CN":
                case "zh_CN":
                    result = "用户组更新成功";
                    break;
            }

            return result;
        }

        /// <summary>
        /// Get language name of a specific language code
        /// </summary>
        /// <param name="languageCode"></param>
        /// <returns></returns>
        private string GetLanguageName(string languageCode = "en-US")
        {
            string result = string.Empty;

            switch (languageCode)
            {
                case "en-US":
                case "en_US":
                    result = "english (United States)";
                    break;
                case "en-GB":
                case "en_GB":
                    result = "english (England)";
                    break;
                case "fr-FR":
                case "fr_FR":
                    result = "français (France)";
                    break;
                case "es-ES":
                case "es_ES":
                    result = "español (Spain)";
                    break;
                case "de-DE":
                case "de_DE":
                    result = "deutsch (Germany)";
                    break;
                case "zh-CN":
                case "zh_CN":
                    result = "中文 (中国)";
                    break;
            }

            return result;
        }

        public void VerifyControlProgramUnmodifiable(SchedulingManagerPage page, string template)
        {
            Step(" - Control program grid toolbar:");
            Step("   + New, Duplicate, Remove buttons are not displayed");
            var isAddControlProgramToolbarButtonVisible = page.SchedulingManagerPanel.IsAddControlProgramButtonVisible();
            var isDeleteControlProgramToolbarButtonVisible = page.SchedulingManagerPanel.IsDeleteControlProgramButtonVisible();
            var isDuplicateControlProgramToolbarButtonVisible = page.SchedulingManagerPanel.IsDuplicateControlProgramButtonVisible();
            VerifyEqual("Verify New, Duplicate, Remove buttons are not displayed", true, isAddControlProgramToolbarButtonVisible == false && isDeleteControlProgramToolbarButtonVisible == false && isDuplicateControlProgramToolbarButtonVisible == false);

            Step(" - Control program editor:");
            Step("   + Name, Description: read-only");
            var isNameInputReadOnly = page.ControlProgramEditorPanel.IsNameInputReadOnly();
            var isDescriptionInputReadOnly = page.ControlProgramEditorPanel.IsDescriptionInputReadOnly();
            VerifyEqual("Verify Name, Description: read-only", true, isNameInputReadOnly && isDescriptionInputReadOnly);

            Step("   + Chart color picker: disabled");
            VerifyEqual("Verify Chart color picker: disabled", true, page.ControlProgramEditorPanel.IsChartColorPickerReadOnly());

            Step("   + Template: disabled");
            VerifyEqual("Verify Template: disabled", true, page.ControlProgramEditorPanel.IsTemplateDropDownReadOnly());

            Step("   + Dependings on template, dimming level fields (below Template field and above chart) are all disabled. Similarly, Control program items table is displayed. When it's clicked, the popup appears with buttons following buttons NOT displayed: New, Remove, Sort, Cancel, Save");
            var isControlProgramItemsButtonVisible = page.ControlProgramEditorPanel.IsControlProgramItemsButtonVisible();

            switch (template)
            {
                case "Advanced mode":
                    VerifyEqual("Verify Control program items button: visible", true, isControlProgramItemsButtonVisible);
                    VerifyEqual("Verify Timeline DropDown is readonly", true, page.ControlProgramEditorPanel.IsTimelineDropDownReadOnly());
                    page.ControlProgramEditorPanel.ClickControlProgramItemsButton();
                    page.WaitForPopupDialogDisplayed();
                    var isCancelButtonVisible = page.ControlProgramItemsPopupPanel.IsCancelButtonVisible();
                    var isSaveButtonVisible = page.ControlProgramItemsPopupPanel.IsSaveButtonVisible();
                    var isDeleteButtonVisible = page.ControlProgramItemsPopupPanel.IsDeleteButtonVisible();
                    var IsSortButtonVisible = page.ControlProgramItemsPopupPanel.IsSortButtonVisible();
                    var isAddButtonVisible = page.ControlProgramItemsPopupPanel.IsAddButtonVisible();
                    VerifyEqual("Verify New, Remove, Sort, Cancel, Save buttons are not available", true, isDeleteButtonVisible == false && isAddButtonVisible == false && IsSortButtonVisible == false && isCancelButtonVisible == false && isSaveButtonVisible == false);
                    page.ControlProgramItemsPopupPanel.ClickCloseButton();
                    page.WaitForPopupDialogDisappeared();

                    Step("   + Double-clicking inside chart does NOT create a new point");
                    var currentChartDotsCount = page.ControlProgramEditorPanel.GetChartDotsCount();
                    page.ControlProgramEditorPanel.DoubleClickRandomInsideChart();
                    var actualChartDotsCount = page.ControlProgramEditorPanel.GetChartDotsCount();
                    VerifyEqual("Verify NOT create a new point", currentChartDotsCount, actualChartDotsCount);
                    break;

                case "Astro ON/OFF":
                    VerifyEqual("Verify Control program items button: invisible", false, isControlProgramItemsButtonVisible);
                    VerifyEqual("Verify Switch On-Minute Input is readonly", true, page.ControlProgramEditorPanel.IsSwitchOnMinuteInputReadOnly());
                    VerifyEqual("Verify Switch On-Relation DropDown is readonly", true, page.ControlProgramEditorPanel.IsSwitchOnRelationDropDownReadOnly());
                    VerifyEqual("Verify Switch On-SunEvents DropDown is readonly", true, page.ControlProgramEditorPanel.IsSwitchOnSunEventsDropDownReadOnly());
                    VerifyEqual("Verify Switch On-Level Input is readonly", true, page.ControlProgramEditorPanel.IsSwitchOnLevelInputReadOnly());
                    VerifyEqual("Verify Switch Off-Minute Input is readonly", true, page.ControlProgramEditorPanel.IsSwitchOffMinuteInputReadOnly());
                    VerifyEqual("Verify Switch Off-Relation DropDown is readonly", true, page.ControlProgramEditorPanel.IsSwitchOffRelationDropDownReadOnly());
                    VerifyEqual("Verify Switch Off-SunEvents DropDown is readonly", true, page.ControlProgramEditorPanel.IsSwitchOffSunEventsDropDownReadOnly());
                    VerifyEqual("Verify Switch Off-Level Input is readonly", true, page.ControlProgramEditorPanel.IsSwitchOffLevelInputReadOnly());
                    break;

                case "Astro ON/OFF and fixed time events":
                    VerifyEqual("Verify Control program items button: invisible", false, isControlProgramItemsButtonVisible);
                    VerifyEqual("Verify Switch On-Minute Input is readonly", true, page.ControlProgramEditorPanel.IsSwitchOnMinuteInputReadOnly());
                    VerifyEqual("Verify Switch On-Relation DropDown is readonly", true, page.ControlProgramEditorPanel.IsSwitchOnRelationDropDownReadOnly());
                    VerifyEqual("Verify Switch On-SunEvents DropDown is readonly", true, page.ControlProgramEditorPanel.IsSwitchOnSunEventsDropDownReadOnly());
                    VerifyEqual("Verify Switch On-Level Input is readonly", true, page.ControlProgramEditorPanel.IsSwitchOnLevelInputReadOnly());
                    VerifyEqual("Verify Switch Off-Minute Input is readonly", true, page.ControlProgramEditorPanel.IsSwitchOffMinuteInputReadOnly());
                    VerifyEqual("Verify Switch Off-Relation DropDown is readonly", true, page.ControlProgramEditorPanel.IsSwitchOffRelationDropDownReadOnly());
                    VerifyEqual("Verify Switch Off-SunEvents DropDown is readonly", true, page.ControlProgramEditorPanel.IsSwitchOffSunEventsDropDownReadOnly());
                    VerifyEqual("Verify Switch Off-Level Input is readonly", true, page.ControlProgramEditorPanel.IsSwitchOffLevelInputReadOnly());
                    VerifyEqual("Verify Variations-Time Inputs are readonly", true, page.ControlProgramEditorPanel.AreVariationsTimeInputsReadOnly());
                    VerifyEqual("Verify Variations-Level Inputs are readonly", true, page.ControlProgramEditorPanel.AreVariationsLevelInputsReadOnly());
                    break;

                case "Always ON":
                    VerifyEqual("Verify Control program items button: invisible", false, isControlProgramItemsButtonVisible);
                    VerifyEqual("Verify Switch On-Time Input is readonly", true, page.ControlProgramEditorPanel.IsSwitchOnTimeInputReadOnly());
                    VerifyEqual("Verify Switch On-Level Input is readonly", true, page.ControlProgramEditorPanel.IsSwitchOnLevelInputReadOnly());
                    break;

                case "Always OFF":
                    VerifyEqual("Verify Control program items button: invisible", false, isControlProgramItemsButtonVisible);
                    break;

                case "Day fixed time events":
                    VerifyEqual("Verify Control program items button: invisible", false, isControlProgramItemsButtonVisible);
                    VerifyEqual("Verify Variations-Time Inputs are readonly", true, page.ControlProgramEditorPanel.AreVariationsTimeInputsReadOnly());
                    VerifyEqual("Verify Variations-Level Inputs are readonly", true, page.ControlProgramEditorPanel.AreVariationsLevelInputsReadOnly());
                    break;
            }

            Step("   + Save button are not displayed");
            var isSaveButtonCalendarEditorVisible = page.ControlProgramEditorPanel.IsSaveButtonVisible();
            VerifyEqual("Verify Save button are not displayed", false, isSaveButtonCalendarEditorVisible);
        }

        public void VerifyControlProgramModifiable(SchedulingManagerPage page, string template)
        {
            Step(" - Control program grid toolbar:");
            Step("   + New, Duplicate, Remove buttons are displayed");
            var isAddControlProgramToolbarButtonVisible = page.SchedulingManagerPanel.IsAddControlProgramButtonVisible();
            var isDeleteControlProgramToolbarButtonVisible = page.SchedulingManagerPanel.IsDeleteControlProgramButtonVisible();
            var isDuplicateControlProgramToolbarButtonVisible = page.SchedulingManagerPanel.IsDuplicateControlProgramButtonVisible();
            VerifyEqual("Verify New, Duplicate, Remove buttons are displayed", true, isAddControlProgramToolbarButtonVisible && isDeleteControlProgramToolbarButtonVisible && isDuplicateControlProgramToolbarButtonVisible);

            Step(" - Control program editor:");
            Step("   + Name, Description: editable");
            var isNameInputReadOnly = page.ControlProgramEditorPanel.IsNameInputReadOnly();
            var isDescriptionInputReadOnly = page.ControlProgramEditorPanel.IsDescriptionInputReadOnly();
            VerifyEqual("Verify Name, Description: editable", true, isNameInputReadOnly == false && isDescriptionInputReadOnly == false);

            Step("   + Chart color picker: editable");
            VerifyEqual("Verify Chart color picker: editable", false, page.ControlProgramEditorPanel.IsChartColorPickerReadOnly());

            Step("   + Template: disabled");
            VerifyEqual("Verify Template: disabled", true, page.ControlProgramEditorPanel.IsTemplateDropDownReadOnly());

            Step("   + Dependings on template, dimming level fields (below Template field and above chart) are all disabled. Similarly, Control program items table is displayed. When it's clicked, the popup appears with buttons following buttons NOT displayed: New, Remove, Sort, Cancel, Save");
            var isControlProgramItemsButtonVisible = page.ControlProgramEditorPanel.IsControlProgramItemsButtonVisible();

            switch (template)
            {
                case "Advanced mode":
                    VerifyEqual("Verify Control program items button: visible", true, isControlProgramItemsButtonVisible);
                    VerifyEqual("Verify Timeline DropDown is editable", false, page.ControlProgramEditorPanel.IsTimelineDropDownReadOnly());
                    page.ControlProgramEditorPanel.ClickControlProgramItemsButton();
                    page.WaitForPopupDialogDisplayed();
                    var isCancelButtonVisible = page.ControlProgramItemsPopupPanel.IsCancelButtonVisible();
                    var isSaveButtonVisible = page.ControlProgramItemsPopupPanel.IsSaveButtonVisible();
                    var isDeleteButtonVisible = page.ControlProgramItemsPopupPanel.IsDeleteButtonVisible();
                    var IsSortButtonVisible = page.ControlProgramItemsPopupPanel.IsSortButtonVisible();
                    var isAddButtonVisible = page.ControlProgramItemsPopupPanel.IsAddButtonVisible();
                    VerifyEqual("Verify New, Remove, Sort, Cancel, Save buttons are available", true, isDeleteButtonVisible && isAddButtonVisible && IsSortButtonVisible && isCancelButtonVisible && isSaveButtonVisible);
                    page.ControlProgramItemsPopupPanel.ClickCloseButton();
                    page.WaitForPopupDialogDisappeared();

                    Step("   + Double-clicking inside chart does create a new point");
                    var currentChartDotsCount = page.ControlProgramEditorPanel.GetChartDotsCount();
                    page.ControlProgramEditorPanel.DoubleClickRandomInsideChart();
                    var actualChartDotsCount = page.ControlProgramEditorPanel.GetChartDotsCount();
                    VerifyEqual("Verify create a new point", currentChartDotsCount + 1, actualChartDotsCount);
                    break;

                case "Astro ON/OFF":
                    VerifyEqual("Verify Control program items button: invisible", false, isControlProgramItemsButtonVisible);
                    VerifyEqual("Verify Switch On-Minute Input is editable", false, page.ControlProgramEditorPanel.IsSwitchOnMinuteInputReadOnly());
                    VerifyEqual("Verify Switch On-Relation DropDown is editable", false, page.ControlProgramEditorPanel.IsSwitchOnRelationDropDownReadOnly());
                    VerifyEqual("Verify Switch On-SunEvents DropDown is readonly", true, page.ControlProgramEditorPanel.IsSwitchOnSunEventsDropDownReadOnly());
                    VerifyEqual("Verify Switch On-Level Input is editable", false, page.ControlProgramEditorPanel.IsSwitchOnLevelInputReadOnly());
                    VerifyEqual("Verify Switch Off-Minute Input is editable", false, page.ControlProgramEditorPanel.IsSwitchOffMinuteInputReadOnly());
                    VerifyEqual("Verify Switch Off-Relation DropDown is editable", false, page.ControlProgramEditorPanel.IsSwitchOffRelationDropDownReadOnly());
                    VerifyEqual("Verify Switch Off-SunEvents DropDown is readonly", true, page.ControlProgramEditorPanel.IsSwitchOffSunEventsDropDownReadOnly());
                    VerifyEqual("Verify Switch Off-Level Input is readonly", true, page.ControlProgramEditorPanel.IsSwitchOffLevelInputReadOnly());
                    break;

                case "Astro ON/OFF and fixed time events":
                    VerifyEqual("Verify Control program items button: invisible", false, isControlProgramItemsButtonVisible);
                    VerifyEqual("Verify Switch On-Minute Input is editable", false, page.ControlProgramEditorPanel.IsSwitchOnMinuteInputReadOnly());
                    VerifyEqual("Verify Switch On-Relation DropDown is editable", false, page.ControlProgramEditorPanel.IsSwitchOnRelationDropDownReadOnly());
                    VerifyEqual("Verify Switch On-SunEvents DropDown is readonly", true, page.ControlProgramEditorPanel.IsSwitchOnSunEventsDropDownReadOnly());
                    VerifyEqual("Verify Switch On-Level Input is editable", false, page.ControlProgramEditorPanel.IsSwitchOnLevelInputReadOnly());
                    VerifyEqual("Verify Switch Off-Minute Input is editable", false, page.ControlProgramEditorPanel.IsSwitchOffMinuteInputReadOnly());
                    VerifyEqual("Verify Switch Off-Relation DropDown is editable", false, page.ControlProgramEditorPanel.IsSwitchOffRelationDropDownReadOnly());
                    VerifyEqual("Verify Switch Off-SunEvents DropDown is readonly", true, page.ControlProgramEditorPanel.IsSwitchOffSunEventsDropDownReadOnly());
                    VerifyEqual("Verify Switch Off-Level Input is readonly", true, page.ControlProgramEditorPanel.IsSwitchOffLevelInputReadOnly());
                    VerifyEqual("Verify Variations-Time Inputs are editable", true, page.ControlProgramEditorPanel.AreVariationsTimeInputsEditable());
                    VerifyEqual("Verify Variations-Level Inputs are editable", true, page.ControlProgramEditorPanel.AreVariationsLevelInputsEditable());
                    break;

                case "Always ON":
                    VerifyEqual("Verify Control program items button: invisible", false, isControlProgramItemsButtonVisible);
                    VerifyEqual("Verify Switch On-Time Input is editable", false, page.ControlProgramEditorPanel.IsSwitchOnTimeInputReadOnly());
                    VerifyEqual("Verify Switch On-Level Input is editable", false, page.ControlProgramEditorPanel.IsSwitchOnLevelInputReadOnly());
                    break;

                case "Always OFF":
                    VerifyEqual("Verify Control program items button: invisible", false, isControlProgramItemsButtonVisible);
                    break;

                case "Day fixed time events":
                    VerifyEqual("Verify Control program items button: invisible", false, isControlProgramItemsButtonVisible);
                    VerifyEqual("Verify Variations-Time Inputs are editable", true, page.ControlProgramEditorPanel.AreVariationsTimeInputsEditable());
                    VerifyEqual("Verify Variations-Level Inputs are editable", true, page.ControlProgramEditorPanel.AreVariationsLevelInputsEditable());
                    break;
            }

            Step("   + Save button are displayed");
            var isSaveButtonCalendarEditorVisible = page.ControlProgramEditorPanel.IsSaveButtonVisible();
            VerifyEqual("Verify Save button are displayed", true, isSaveButtonCalendarEditorVisible);
        }

        private void VerifyDeviceButtonsAreDisplayed(EquipmentInventoryPage page)
        {
            var nodeType = page.GeozoneTreeMainPanel.GetSelectedNodeType();

            if (nodeType == NodeType.Streetlight || nodeType == NodeType.Switch)
            {
                VerifyEqual(string.Format("[{0}] Verify Replace Lamp, Replace Node, Duplicate, Save, Remove buttons are displayed", nodeType.ToString()), true
                    , page.DeviceEditorPanel.IsSaveButtonDisplayed()
                    && page.DeviceEditorPanel.IsDeleteButtonDisplayed()
                    && page.DeviceEditorPanel.IsReplaceLampButtonDisplayed()
                    && page.DeviceEditorPanel.IsReplaceNodeButtonDisplayed()
                    && page.DeviceEditorPanel.IsDuplicateButtonDisplayed());
            }
            else if(nodeType == NodeType.Controller)
            {
                VerifyEqual(string.Format("[{0}] Verify Save, Remove buttons are displayed", nodeType.ToString()), true
                   , page.DeviceEditorPanel.IsSaveButtonDisplayed()
                   && page.DeviceEditorPanel.IsDeleteButtonDisplayed()
                   && page.DeviceEditorPanel.IsDuplicateButtonDisplayed());
            }
            else
            {
                VerifyEqual(string.Format("[{0}] Verify Duplicate, Save, Remove buttons are displayed", nodeType.ToString()), true
                   , page.DeviceEditorPanel.IsSaveButtonDisplayed()
                   && page.DeviceEditorPanel.IsDeleteButtonDisplayed()
                   && page.DeviceEditorPanel.IsDuplicateButtonDisplayed());
            }
        }

        private void VerifyDeviceButtonsAreNotDisplayed(EquipmentInventoryPage page)
        {
            var nodeType = page.GeozoneTreeMainPanel.GetSelectedNodeType();

            if (nodeType == NodeType.Streetlight || nodeType == NodeType.Switch)
            {
                VerifyEqual(string.Format("[{0}] Verify Replace Lamp, Replace Node, Duplicate, Save, Remove buttons are not displayed", nodeType.ToString()), true
                    , !page.DeviceEditorPanel.IsSaveButtonDisplayed()
                    && !page.DeviceEditorPanel.IsDeleteButtonDisplayed()
                    && !page.DeviceEditorPanel.IsReplaceLampButtonDisplayed()
                    && !page.DeviceEditorPanel.IsReplaceNodeButtonDisplayed()
                    && !page.DeviceEditorPanel.IsDuplicateButtonDisplayed());
            }
            else
            {
                VerifyEqual(string.Format("[{0}] Verify Duplicate, Save, Remove buttons are displayed", nodeType.ToString()), true
                   , !page.DeviceEditorPanel.IsSaveButtonDisplayed()
                   && !page.DeviceEditorPanel.IsDeleteButtonDisplayed()
                   && !page.DeviceEditorPanel.IsDuplicateButtonDisplayed());
            }
        }

        #region XML Input data

        private Dictionary<string, object> GetCommonTestData()
        {
            var realtimeGeozone = Settings.CommonTestData[0];
            var testData = new Dictionary<string, object>();
            testData.Add("Geozone", realtimeGeozone.Path);
            var streetlights = realtimeGeozone.Devices.Where(p => p.Type == DeviceType.Streetlight && p.Status == DeviceStatus.Working && string.IsNullOrEmpty(p.Cluster)).ToList();
            testData.Add("Streetlights", streetlights);

            return testData;
        }

        /// <summary>
        /// Read test data for test case TS 1.1.1
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, object> GetTestDataOfTestTS1_1_1()
        {
            var testCaseName = "TS1_1_1";
            var xmlUtility = new XmlUtility(Settings.TC1_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, object>();
            testData.Add("English", xmlUtility.GetChildNodesText(string.Format(Settings.TC1_XPATH_PREFIX, testCaseName, "English")));
            testData.Add("French", xmlUtility.GetChildNodesText(string.Format(Settings.TC1_XPATH_PREFIX, testCaseName, "French")));

            return testData;
        }

        /// <summary>
        /// Read test data for test case TS 1.4.1
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfTestTS1_4_1()
        {
            var testCaseName = "TS1_4_1";
            var xmlUtility = new XmlUtility(Settings.TC1_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, string>();
            testData.Add("Title", xmlUtility.GetSingleNodeText(string.Format(Settings.TC1_XPATH_PREFIX, testCaseName, "Title")));
            testData.Add("Version", string.Format("Version: {0}", xmlUtility.GetSingleNodeText(string.Format(Settings.TC1_XPATH_PREFIX, testCaseName, "Version"))));
            testData.Add("Copyright", xmlUtility.GetSingleNodeText(string.Format(Settings.TC1_XPATH_PREFIX, testCaseName, "Copyright")));

            return testData;
        }

        /// <summary>
        /// Read test data for test case TS 1.7.1
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, object> GetTestDataOfTestTS1_7_1()
        {
            var testCaseName = "TS1_7_1";
            var xmlUtility = new XmlUtility(Settings.TC1_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, object>();
            testData.Add("Skins", xmlUtility.GetChildNodesText(string.Format(Settings.TC1_XPATH_PREFIX, testCaseName, "Skins")));
            testData.Add("LanguageCodes", xmlUtility.GetChildNodesText(string.Format(Settings.TC1_XPATH_PREFIX, testCaseName, "LanguageCodes")));

            return testData;
        }

        /// <summary>
        /// Read test data for test case TS 1.10.1
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfTestTS1_10_1()
        {
            var testCaseName = "TS1_10_1";
            var xmlUtility = new XmlUtility(Settings.TC1_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, string>();
            testData.Add("BlockAction", xmlUtility.GetSingleNodeText(string.Format(Settings.TC1_XPATH_PREFIX, testCaseName, "BlockAction")));

            return testData;
        }

        /// <summary>
        /// Read test data for test case TS 1.10.2
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfTestTS1_10_2()
        {
            var testCaseName = "TS1_10_2";
            var xmlUtility = new XmlUtility(Settings.TC1_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, string>();
            testData.Add("BlockAction", xmlUtility.GetSingleNodeText(string.Format(Settings.TC1_XPATH_PREFIX, testCaseName, "BlockAction")));

            return testData;
        }

        /// <summary>
        /// Read test data for test case TS 1.10.3
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfTestTS1_10_3()
        {
            var testCaseName = "TS1_10_3";
            var xmlUtility = new XmlUtility(Settings.TC1_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, string>();
            testData.Add("BlockAction", xmlUtility.GetSingleNodeText(string.Format(Settings.TC1_XPATH_PREFIX, testCaseName, "BlockAction")));

            return testData;
        }

        /// <summary>
        /// Read test data for test case TS 1.10.4
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, object> GetTestDataOfTestTS1_10_4()
        {
            var testCaseName = "TS1_10_4";
            var xmlUtility = new XmlUtility(Settings.TC1_TEST_DATA_FILE_PATH);
            var testData = new Dictionary<string, object>();

            testData.Add("BlockAction", xmlUtility.GetSingleNodeText(string.Format(Settings.TC1_XPATH_PREFIX, testCaseName, "BlockAction")));
            var realtimeGeozone = Settings.CommonTestData[0];
            testData.Add("GeoZone", realtimeGeozone.Path);
            var streetlights = realtimeGeozone.Devices.Where(p => p.Type == DeviceType.Streetlight && p.Status == DeviceStatus.Working && string.IsNullOrEmpty(p.Cluster)).ToList();
            streetlights = streetlights.PickRandom(3).ToList();
            testData.Add("Devices", streetlights.Select(p => string.Format("{0};{1}", p.Longitude, p.Latitude)).ToList());

            return testData;
        }

        /// <summary>
        /// Read test data for test case TS 1.10.5
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfTestTS1_10_5()
        {
            var testCaseName = "TS1_10_5";
            var xmlUtility = new XmlUtility(Settings.TC1_TEST_DATA_FILE_PATH);
            var testData = new Dictionary<string, string>();

            testData.Add("BlockAction", xmlUtility.GetSingleNodeText(string.Format(Settings.TC1_XPATH_PREFIX, testCaseName, "BlockAction")));
            var realtimeGeozone = Settings.CommonTestData[0];
            testData.Add("GeoZone", realtimeGeozone.Path);
            var streetlights = realtimeGeozone.Devices.Where(p => p.Type == DeviceType.Streetlight && p.Status == DeviceStatus.Working && string.IsNullOrEmpty(p.Cluster)).ToList();
            testData.Add("Device", streetlights.PickRandom().Name);

            return testData;
        }

        /// <summary>
        /// Read test data for test case TS 1.10.6
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfTestTS1_10_6()
        {
            var testCaseName = "TS1_10_6";
            var xmlUtility = new XmlUtility(Settings.TC1_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, string>();
            testData.Add("BlockAction", xmlUtility.GetSingleNodeText(string.Format(Settings.TC1_XPATH_PREFIX, testCaseName, "BlockAction")));
            var realtimeGeozone = Settings.CommonTestData[0];
            testData.Add("GeoZone", realtimeGeozone.Path);
            var controller = realtimeGeozone.Devices.FirstOrDefault(p => p.Type == DeviceType.Controller && p.Status == DeviceStatus.NonWorking);
            testData.Add("Controller", controller.Name);

            return testData;
        }

        /// <summary>
        /// Read test data for test case TS 1.10.7
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfTestTS1_10_7()
        {
            var testCaseName = "TS1_10_7";
            var xmlUtility = new XmlUtility(Settings.TC1_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, string>();

            testData.Add("BlockAction", xmlUtility.GetSingleNodeText(string.Format(Settings.TC1_XPATH_PREFIX, testCaseName, "BlockAction")));
            var realtimeGeozone = Settings.CommonTestData[0];
            testData.Add("GeoZone", realtimeGeozone.Path);
            var streetlights = realtimeGeozone.Devices.Where(p => p.Type == DeviceType.Streetlight && p.Status == DeviceStatus.Working && string.IsNullOrEmpty(p.Cluster)).ToList();
            testData.Add("AnyDevice", streetlights.PickRandom().Name);

            return testData;
        }

        /// <summary>
        /// Read test data for test case TS 1.10.8
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, object> GetTestDataOfTestTS1_10_8()
        {
            var testCaseName = "TS1_10_8";
            var xmlUtility = new XmlUtility(Settings.TC1_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, object>();
            testData.Add("BlockAction", xmlUtility.GetSingleNodeText(string.Format(Settings.TC1_XPATH_PREFIX, testCaseName, "BlockAction")));
            var realtimeGeozone = Settings.CommonTestData[0];
            testData.Add("GeoZone", realtimeGeozone.Path);
            var streetlights = realtimeGeozone.Devices.Where(p => p.Type == DeviceType.Streetlight && p.Status == DeviceStatus.Working && string.IsNullOrEmpty(p.Cluster)).ToList();
            streetlights = streetlights.PickRandom(2).ToList();
            testData.Add("AnyDevices", streetlights);

            return testData;
        }

        /// <summary>
        /// Read test data for test case TS 1.10.9
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfTestTS1_10_9()
        {
            var testCaseName = "TS1_10_9";
            var xmlUtility = new XmlUtility(Settings.TC1_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, string>();
            testData.Add("BlockAction", xmlUtility.GetSingleNodeText(string.Format(Settings.TC1_XPATH_PREFIX, testCaseName, "BlockAction")));
            var realtimeGeozone = Settings.CommonTestData[0];
            testData.Add("GeoZone", realtimeGeozone.Path);
            var streetlights = realtimeGeozone.Devices.Where(p => p.Type == DeviceType.Streetlight && p.Status == DeviceStatus.Working && string.IsNullOrEmpty(p.Cluster)).ToList();
            testData.Add("AnyLamp", streetlights.PickRandom().Name);

            return testData;
        }

        /// <summary>
        /// Read test data for test case TS 1.10.10
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfTestTS1_10_10()
        {
            var testCaseName = "TS1_10_10";
            var xmlUtility = new XmlUtility(Settings.TC1_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, string>();
            testData.Add("BlockAction", xmlUtility.GetSingleNodeText(string.Format(Settings.TC1_XPATH_PREFIX, testCaseName, "BlockAction")));
            testData.Add("UniqueAddress", SLVHelper.GenerateMACAddress());
            var realtimeGeozone = Settings.CommonTestData[0];
            testData.Add("GeoZone", realtimeGeozone.Path);
            var streetlights = realtimeGeozone.Devices.Where(p => p.Type == DeviceType.Streetlight && p.Status == DeviceStatus.Working && string.IsNullOrEmpty(p.Cluster)).ToList();
            testData.Add("AnyLightPointController", streetlights.PickRandom().Name);

            return testData;
        }

        /// <summary>
        /// Read test data for test case TS 1.10.11
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfTestTS1_10_11()
        {
            var testCaseName = "TS1_10_11";
            var xmlUtility = new XmlUtility(Settings.TC1_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, string>();
            testData.Add("BlockAction", xmlUtility.GetSingleNodeText(string.Format(Settings.TC1_XPATH_PREFIX, testCaseName, "BlockAction")));
            var realtimeGeozone = Settings.CommonTestData[0];
            testData.Add("GeoZone", realtimeGeozone.Path);
            var streetlights = realtimeGeozone.Devices.Where(p => p.Type == DeviceType.Streetlight && p.Status == DeviceStatus.Working && string.IsNullOrEmpty(p.Cluster)).ToList();
            testData.Add("Device", streetlights.PickRandom().Name);

            return testData;
        }

        /// <summary>
        /// Read test data for test case TS 1.10.12
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfTestTS1_10_12()
        {
            var testCaseName = "TS1_10_12";
            var xmlUtility = new XmlUtility(Settings.TC1_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, string>();
            testData.Add("BlockAction", xmlUtility.GetSingleNodeText(string.Format(Settings.TC1_XPATH_PREFIX, testCaseName, "BlockAction")));
            var realtimeGeozone = Settings.CommonTestData[0];
            testData.Add("GeoZone", realtimeGeozone.Path);
            var streetlights = realtimeGeozone.Devices.Where(p => p.Type == DeviceType.Streetlight && p.Status == DeviceStatus.Working && string.IsNullOrEmpty(p.Cluster)).ToList();
            testData.Add("Device", streetlights.PickRandom().Name);

            return testData;
        }

        /// <summary>
        /// Read test data for test case TS 1.10.13
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, object> GetTestDataOfTestTS1_10_13()
        {
            var testCaseName = "TS1_10_13";
            var xmlUtility = new XmlUtility(Settings.TC1_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, object>();
            testData.Add("BlockAction", xmlUtility.GetSingleNodeText(string.Format(Settings.TC1_XPATH_PREFIX, testCaseName, "BlockAction")));
            var realtimeGeozone = Settings.CommonTestData[0];
            testData.Add("GeoZone", realtimeGeozone.Path);
            var streetlights = realtimeGeozone.Devices.Where(p => p.Type == DeviceType.Streetlight && p.Status == DeviceStatus.Working && string.IsNullOrEmpty(p.Cluster)).ToList();
            streetlights = streetlights.PickRandom(2).ToList();
            testData.Add("Devices", streetlights);

            return testData;
        }

        /// <summary>
        /// Read test data for test case TS 1.10.14
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfTestTS1_10_14()
        {
            var testCaseName = "TS1_10_14";
            var xmlUtility = new XmlUtility(Settings.TC1_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, string>();
            testData.Add("BlockAction", xmlUtility.GetSingleNodeText(string.Format(Settings.TC1_XPATH_PREFIX, testCaseName, "BlockAction")));
            var realtimeGeozone = Settings.CommonTestData[0];
            testData.Add("GeoZone", realtimeGeozone.Path);
            var streetlights = realtimeGeozone.Devices.Where(p => p.Type == DeviceType.Streetlight && p.Status == DeviceStatus.Working && string.IsNullOrEmpty(p.Cluster)).ToList();
            testData.Add("Device", streetlights.PickRandom().Name);

            return testData;
        }

        /// <summary>
        /// Read test data for test case TS 1.10.15
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfTestTS1_10_15()
        {
            var testCaseName = "TS1_10_15";
            var xmlUtility = new XmlUtility(Settings.TC1_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, string>();
            testData.Add("BlockAction", xmlUtility.GetSingleNodeText(string.Format(Settings.TC1_XPATH_PREFIX, testCaseName, "BlockAction")));
            var realtimeGeozone = Settings.CommonTestData[0];
            testData.Add("GeoZone", realtimeGeozone.Path);
            var controller = realtimeGeozone.Devices.FirstOrDefault(p => p.Type == DeviceType.Controller && p.Status == DeviceStatus.NonWorking);
            testData.Add("Controller", controller.Name);

            return testData;
        }

        /// <summary>
        /// Read test data for test case TS 1.10.15
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfTestTS1_10_16()
        {
            var testCaseName = "TS1_10_16";
            var xmlUtility = new XmlUtility(Settings.TC1_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, string>();
            testData.Add("BlockAction", xmlUtility.GetSingleNodeText(string.Format(Settings.TC1_XPATH_PREFIX, testCaseName, "BlockAction")));
            var realtimeGeozone = Settings.CommonTestData[0];
            testData.Add("GeoZone", realtimeGeozone.Path);
            var controller = realtimeGeozone.Devices.FirstOrDefault(p => p.Type == DeviceType.Controller && p.Status == DeviceStatus.NonWorking);
            testData.Add("Controller", controller.Name);

            return testData;
        }

        /// <summary>
        /// Read test data for test case TS 1.10.17
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfTestTS1_10_17()
        {
            var testCaseName = "TS1_10_17";
            var xmlUtility = new XmlUtility(Settings.TC1_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, string>();
            testData.Add("GeoZone", xmlUtility.GetSingleNodeText(string.Format(Settings.TC1_XPATH_PREFIX, testCaseName, "GeoZone")));
            testData.Add("AnyGeoZone", xmlUtility.GetSingleNodeText(string.Format(Settings.TC1_XPATH_PREFIX, testCaseName, "AnyGeoZone")));
            testData.Add("BlockAction", xmlUtility.GetSingleNodeText(string.Format(Settings.TC1_XPATH_PREFIX, testCaseName, "BlockAction")));

            return testData;
        }

        /// <summary>
        /// Read test data for test case TS 1.10.18
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfTestTS1_10_18()
        {
            var testCaseName = "TS1_10_18";
            var xmlUtility = new XmlUtility(Settings.TC1_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, string>();
            testData.Add("BlockAction", xmlUtility.GetSingleNodeText(string.Format(Settings.TC1_XPATH_PREFIX, testCaseName, "BlockAction")));

            return testData;
        }

        /// <summary>
        /// Read test data for test case TS 1.10.19
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfTestTS1_10_19()
        {
            var testCaseName = "TS1_10_19";
            var xmlUtility = new XmlUtility(Settings.TC1_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, string>();
            testData.Add("BlockAction", xmlUtility.GetSingleNodeText(string.Format(Settings.TC1_XPATH_PREFIX, testCaseName, "BlockAction")));

            return testData;
        }

        /// <summary>
        /// Read test data for test case TS 1.10.20
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfTestTS1_10_20()
        {
            var testCaseName = "TS1_10_20";
            var xmlUtility = new XmlUtility(Settings.TC1_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, string>();
            testData.Add("BlockAction", xmlUtility.GetSingleNodeText(string.Format(Settings.TC1_XPATH_PREFIX, testCaseName, "BlockAction")));

            return testData;
        }

        /// <summary>
        /// Read test data for test case TS 1.10.21
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfTestTS1_10_21()
        {
            var testCaseName = "TS1_10_21";
            var xmlUtility = new XmlUtility(Settings.TC1_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, string>();
            testData.Add("BlockAction", xmlUtility.GetSingleNodeText(string.Format(Settings.TC1_XPATH_PREFIX, testCaseName, "BlockAction")));

            return testData;
        }

        /// <summary>
        /// Read test data for test case TS 1.10.22
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfTestTS1_10_22()
        {
            var testCaseName = "TS1_10_22";
            var xmlUtility = new XmlUtility(Settings.TC1_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, string>();
            testData.Add("BlockAction", xmlUtility.GetSingleNodeText(string.Format(Settings.TC1_XPATH_PREFIX, testCaseName, "BlockAction")));

            return testData;
        }

        /// <summary>
        /// Read test data for test case TS 1.10.23
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, object> GetTestDataOfTestTS1_10_23()
        {
            var testCaseName = "TS1_10_23";
            var xmlUtility = new XmlUtility(Settings.TC1_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, object>();
            testData.Add("BlockAction", xmlUtility.GetSingleNodeText(string.Format(Settings.TC1_XPATH_PREFIX, testCaseName, "BlockAction")));
            testData.Add("ControlPrograms", xmlUtility.GetChildNodesText(string.Format(Settings.TC1_XPATH_PREFIX, testCaseName, "ControlPrograms")));

            return testData;
        }

        /// <summary>
        /// Read test data for test case TS 1.10.24
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfTestTS1_10_24()
        {
            var testCaseName = "TS1_10_24";
            var xmlUtility = new XmlUtility(Settings.TC1_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, string>();
            testData.Add("BlockAction", xmlUtility.GetSingleNodeText(string.Format(Settings.TC1_XPATH_PREFIX, testCaseName, "BlockAction")));
            testData.Add("Geozone", xmlUtility.GetSingleNodeText(string.Format(Settings.TC1_XPATH_PREFIX, testCaseName, "Geozone")));

            return testData;
        }

        /// <summary>
        /// Read test data for test case TS 1.10.25
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, object> GetTestDataOfTestTS1_10_25()
        {
            var testCaseName = "TS1_10_25";
            var xmlUtility = new XmlUtility(Settings.TC1_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, object>();
            testData.Add("BlockAction", xmlUtility.GetSingleNodeText(string.Format(Settings.TC1_XPATH_PREFIX, testCaseName, "BlockAction")));
            testData.Add("ControlProgramAdvancedMode", xmlUtility.GetSingleNodeText(string.Format(Settings.TC1_XPATH_PREFIX, testCaseName, "ControlProgramAdvancedMode")));
            testData.Add("ControlProgramsNotAdvancedMode", xmlUtility.GetChildNodesText(string.Format(Settings.TC1_XPATH_PREFIX, testCaseName, "ControlProgramsNotAdvancedMode")));
            testData.Add("Templates", xmlUtility.GetChildNodesText(string.Format(Settings.TC1_XPATH_PREFIX, testCaseName, "Templates")));

            return testData;
        }

        /// <summary>
        /// Read test data for test case TS 1.11.1
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, object> GetTestDataOfTestTS1_11_1()
        {
            var testCaseName = "TS1_11_1";
            var xmlUtility = new XmlUtility(Settings.TC1_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, object>();
            testData.Add("RTC_Requests", xmlUtility.GetChildNodesText(string.Format(Settings.TC1_XPATH_PREFIX, testCaseName, "RTC_Requests")));
            testData.Add("RTC_ExpectedUnauthorizedAction", xmlUtility.GetChildNodesText(string.Format(Settings.TC1_XPATH_PREFIX, testCaseName, "RTC_ExpectedUnauthorizedAction")));
            testData.Add("EI_GeoZone", xmlUtility.GetSingleNodeText(string.Format(Settings.TC1_XPATH_PREFIX, testCaseName, "EI_GeoZone")));
            testData.Add("EI_Controller", xmlUtility.GetSingleNodeText(string.Format(Settings.TC1_XPATH_PREFIX, testCaseName, "EI_Controller")));
            testData.Add("EI_GeoZoneRemove", xmlUtility.GetSingleNodeText(string.Format(Settings.TC1_XPATH_PREFIX, testCaseName, "EI_GeoZoneRemove")));
            testData.Add("EI_GeoZoneAllTypes", xmlUtility.GetSingleNodeText(string.Format(Settings.TC1_XPATH_PREFIX, testCaseName, "EI_GeoZoneAllTypes")));
            testData.Add("UniqueAddress", xmlUtility.GetSingleNodeText(string.Format(Settings.TC1_XPATH_PREFIX, testCaseName, "UniqueAddress")));
            testData.Add("ControlProgramAdvancedMode", xmlUtility.GetSingleNodeText(string.Format(Settings.TC1_XPATH_PREFIX, testCaseName, "ControlProgramAdvancedMode")));
            testData.Add("ControlProgramsNotAdvancedMode", xmlUtility.GetChildNodesText(string.Format(Settings.TC1_XPATH_PREFIX, testCaseName, "ControlProgramsNotAdvancedMode")));
            testData.Add("Templates", xmlUtility.GetChildNodesText(string.Format(Settings.TC1_XPATH_PREFIX, testCaseName, "Templates")));
            var realtimeGeozone = Settings.CommonTestData[0];
            testData.Add("RTC_GeoZone", realtimeGeozone.Path);
            var controller = realtimeGeozone.Devices.FirstOrDefault(p => p.Type == DeviceType.Controller && p.Status == DeviceStatus.NonWorking);
            testData.Add("RTC_Controller", controller.Name);
            var streetlights = realtimeGeozone.Devices.Where(p => p.Type == DeviceType.Streetlight && p.Status == DeviceStatus.Working && string.IsNullOrEmpty(p.Cluster)).ToList();
            streetlights = streetlights.PickRandom(2).ToList();
            testData.Add("RTC_Devices", streetlights);

            return testData;
        }

        /// <summary>
        /// Read test data for test case TS 1.12.1
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfTestTS1_12_1()
        {
            var testCaseName = "TS1_12_1";
            var xmlUtility = new XmlUtility(Settings.TC1_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, string>();
            var profileA = xmlUtility.GetSingleNode(string.Format(Settings.TC1_XPATH_PREFIX, testCaseName, "ProfileA"));
            var profileB = xmlUtility.GetSingleNode(string.Format(Settings.TC1_XPATH_PREFIX, testCaseName, "ProfileB"));
            testData.Add("A_ProfileName", profileA.GetChildNodeText("ProfileName"));
            testData.Add("A_LanguageCode", profileA.GetChildNodeText("LanguageCode"));
            testData.Add("A_Skin", profileA.GetChildNodeText("Skin"));
            testData.Add("A_User", profileA.GetChildNodeText("User"));
            testData.Add("A_Password", profileA.GetChildNodeText("Password"));
            testData.Add("A_Email", profileA.GetChildNodeText("Email"));
            testData.Add("B_ProfileName", profileB.GetChildNodeText("ProfileName"));
            testData.Add("B_LanguageCode", profileB.GetChildNodeText("LanguageCode"));
            testData.Add("B_Skin", profileB.GetChildNodeText("Skin"));
            testData.Add("B_User", profileB.GetChildNodeText("User"));
            testData.Add("B_Password", profileB.GetChildNodeText("Password"));
            testData.Add("B_Email", profileB.GetChildNodeText("Email"));
            return testData;
        }

        /// <summary>
        /// Read test data for test case TS 1.13.1
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, object> GetTestDataOfTestTS1_13_1()
        {
            return GetCommonTestData();
        }

        /// <summary>
        /// Read test data for test case TS 1.14.1
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, object> GetTestDataOfTestTS1_14_1()
        {
            var testCaseName = "TS1_14_1";
            var xmlUtility = new XmlUtility(Settings.TC1_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, object>();
            testData.Add("SearchCity", xmlUtility.GetChildNodesText(string.Format(Settings.TC1_XPATH_PREFIX, testCaseName, "SearchCity")));

            return testData;
        }

        /// <summary>
        /// Read test data for test case TS 1.15.1
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfTestTS1_15_1()
        {
            var testCaseName = "TS1_15_1";
            var xmlUtility = new XmlUtility(Settings.TC1_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, string>();
            testData.Add("GeoZone", xmlUtility.GetSingleNodeText(string.Format(Settings.TC1_XPATH_PREFIX, testCaseName, "GeoZone")));
            testData.Add("Controller", xmlUtility.GetSingleNodeText(string.Format(Settings.TC1_XPATH_PREFIX, testCaseName, "Controller")));

            return testData;
        }

        /// <summary>
        /// Read test data for test case TS 1.16.1
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, object> GetTestDataOfTestTS1_16_1()
        {
            var testCaseName = "TS1_16_1";
            var xmlUtility = new XmlUtility(Settings.TC1_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, object>();
            testData.Add("GeoZones", xmlUtility.GetChildNodesText(string.Format(Settings.TC1_XPATH_PREFIX, testCaseName, "GeoZones")));

            return testData;
        }

        #endregion //XML Input data

        #region Verify methods   

        private void VerifyLoginPageLanguage(LoginPage page, List<string> expectedLanguageList)
        {
            string expectedSignIn = expectedLanguageList.ElementAt(0);
            string expectedForgotPassword = expectedLanguageList.ElementAt(1);
            string expectedUsername = expectedLanguageList.ElementAt(2);
            string expectedPassword = expectedLanguageList.ElementAt(3);

            var actualLoginTitle = page.GetLoginTitleText();
            var acutalUsername = page.GetUsernamePlaceholder();
            var actualPassword = page.GetPasswordPlaceholder();
            var actualForgotPasswordLink = page.GetForgotPasswordLinkText();
            var actualForgotName = page.GetForgotUsernamePlaceholder();

            VerifyTrue(string.Format("Verify SignIn text is '{0}'", expectedSignIn), expectedSignIn.Equals(actualLoginTitle), expectedSignIn, actualLoginTitle);
            VerifyTrue(string.Format("Verify Username placeholder is '{0}'", expectedUsername), expectedUsername.Equals(acutalUsername), expectedUsername, acutalUsername);
            VerifyTrue(string.Format("Verify Password placeholder is '{0}'", expectedPassword), expectedPassword.Equals(actualPassword), expectedPassword, actualPassword);
            VerifyTrue(string.Format("Verify ForgotPassword text is '{0}'", expectedForgotPassword), expectedForgotPassword.Equals(actualForgotPasswordLink), expectedForgotPassword, actualForgotPasswordLink);
            VerifyTrue(string.Format("Verify ForgotName placeholder is '{0}'", expectedUsername), expectedUsername.Equals(actualForgotName), expectedUsername, actualForgotName);
        }

        /// <summary>
        /// Verify if a response message from a request is as expected
        /// </summary>
        /// <param name="sessions"></param>
        /// <param name="user"></param>
        /// <param name="expectedRequest"></param>
        /// <param name="expectedUnauthorizedAction"></param>
        private void VerifyResponseMessage(IList<CustomizedSession> sessions, string user, string expectedRequest, string expectedUnauthorizedAction)
        {
            var matchedSession = sessions.FirstOrDefault(r => r.URL.Contains(expectedRequest));
            VerifyTrue("Verify a request for this action is sent to server", matchedSession != null, "A request for this action is sent to server", "No request for this action is sent to server");
            var responseJSON = matchedSession != null ? JToken.Parse(matchedSession.Response) : null;
            var expectedErrorMessage = string.Format("User '{0}' is not authorized to perform '{1}'!", user, expectedUnauthorizedAction);
            var actuallErrorMessage = responseJSON != null ? responseJSON["message"].Value<string>() : string.Empty;
            VerifyTrue("Verify an unauthorized response message is as expected", string.Equals(actuallErrorMessage, expectedErrorMessage), expectedErrorMessage, actuallErrorMessage);
        }

        /// <summary>
        /// Verify grid records of log viewer
        /// </summary>
        /// <param name="appPrefix"></param> 
        private void VerifyLogViewerGridRecords(LogViewerPage page, DateTime from, DateTime to, string currentUser)
        {
            Wait.ForSeconds(2);
            var dtGrid = page.GridPanel.BuildDataTableFromGrid();
            var listDateStr = dtGrid.AsEnumerable().Select(r => r.Field<string>("Date").SplitAndGetAt(new string[] { " " }, 0)).ToList().Distinct().ToList();

            List<DateTime> listDate = listDateStr.ToDateList("dd/MM/yyyy");
            var dateExisted = listDate.Any(d => d <= from && d >= to);
            VerifyTrue(string.Format("Verify Grid records has only date within From & To"), !dateExisted, "All records with date within From & To", "Have record(s) with date without From & To");

            var listUser = dtGrid.AsEnumerable().Select(r => r.Field<string>("User")).ToList().Distinct().ToList();
            var userExisted = listUser.Any(u => !u.Equals(currentUser));
            VerifyTrue(string.Format("Verify Grid records has only user with current logged user"), !userExisted, "All records with current logged user", "Have record(s) with other user");

            var listAction = dtGrid.AsEnumerable().Select(r => r.Field<string>("Action")).ToList().Distinct().ToList();
            var actionExisted = listAction.Exists(a => Regex.IsMatch(a, "com.dotv.streetlightserver.api*"));
            VerifyTrue(string.Format("Verify Grid records has pattern 'com.dotv.streetlightserver.api.*'"), actionExisted, "Have records with 'com.dotv.streetlightserver.api.*'", "No records with 'com.dotv.streetlightserver.api.*'");
        }

        /// <summary>
        /// Verify sunrise sunset widget Info
        /// </summary>
        /// <param name="page"></param>
        /// <param name="geozone"></param>
        private void VerifySunriseSunsetInfo(DesktopPage page, string geozone)
        {
            var actualGeozone = page.GetFirstSunriseSunsetWidgetGeozoneName();
            VerifyEqual(string.Format("Geozone '{0}' is diplayed correctly", geozone), geozone, actualGeozone);
            var actualSunriseValue = page.GetFirstSunriseSunsetWidgetSunriseTime();
            VerifyTrue("[SLV-932] Sunrise Value is diplayed correctly", !string.IsNullOrEmpty(actualSunriseValue), "Sunrise Value should updated with selected Geozone", actualSunriseValue);
            var actualSunsetValue = page.GetFirstSunriseSunsetWidgetSunsetTime();
            VerifyTrue("[SLV-932] Sunset Value is diplayed correctly", !string.IsNullOrEmpty(actualSunsetValue), "Sunset Value should updated with selected Geozone", actualSunsetValue);
        }

        #endregion //Verify methods        

        #endregion //Private methods
    }
}
