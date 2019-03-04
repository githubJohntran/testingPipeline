using NUnit.Framework;
using StreetlightVision.Extensions;
using StreetlightVision.Models;
using StreetlightVision.Pages;
using StreetlightVision.Utilities;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace StreetlightVision.Tests.Coverage.Apps
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    public class UserAppTests : TestBase
    {
        #region Variables
        
        #endregion //Variables

        #region Contructors

        #endregion //Contructors

        #region Test Cases

        [Test, DynamicRetry]
        [Description("SC-1707 - gray out fields that cannot be modified")]
        public void User_01()
        {
            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - Using default user to create a testing profile and add a user for that profile, then log out");            
            Step("**** Precondition ****\n");

            var newUserModel = CreateNewProfileAndUser();
            var defaultUserModel = Settings.Users["DefaultTest"];
            var loginPage = Browser.OpenCMS();                   

            Step("1. Refresh the page and login to the system with the testing user created at the precondition");
            var desktopPage = loginPage.LoginAsValidUser(newUserModel.Username, newUserModel.Password);
            desktopPage.InstallAppsIfNotExist(App.Users);

            Step("2. Verify user logs in successfully");
            VerifyEqual("2. Verify Username is displayed correctly", newUserModel.FullName, desktopPage.GetUserFullNameText());
            VerifyEqual("2. Verify User Group is displayed correctly", newUserModel.Profile, desktopPage.GetUserProfileNameText());

            Step("3. Select Users app on the desktop");
            Step("4. Verify Users app displays");
            var usersPage = desktopPage.GoToApp(App.Users) as UsersPage;

            Step("5. Select Default Test Profile and select Default Test User");
            usersPage.UserProfileListPanel.SelectProfile(defaultUserModel.Profile);            
            usersPage.UserListPanel.SelectUser(defaultUserModel.FullName);
            usersPage.WaitForUserDetailsDisplayed();

            Step("6. Verify All the fields are readonly");
            VerifyEqual("6. Verify Last Name field is readonly", true, usersPage.UserDetailsPanel.IsLastNameInputReadOnly());
            VerifyEqual("6. Verify First Name field is readonly", true, usersPage.UserDetailsPanel.IsFirstNameInputReadOnly());
            VerifyEqual("6. Verify Phone field is readonly", true, usersPage.UserDetailsPanel.IsPhoneInputReadOnly());
            VerifyEqual("6. Verify Mobile field is readonly", true, usersPage.UserDetailsPanel.IsMobileInputReadOnly());
            VerifyEqual("6. Verify Email field is readonly", true, usersPage.UserDetailsPanel.IsEmailInputReadOnly());
            VerifyEqual("6. Verify Address field is readonly", true, usersPage.UserDetailsPanel.IsAddressInputReadOnly());

            Step("7. Select the testing profile and the testing user");
            usersPage.UserProfileListPanel.SelectProfile(newUserModel.Profile);
            usersPage.UserListPanel.SelectUser(newUserModel.FullName);
            usersPage.WaitForUserDetailsDisplayed();

            Step("8. Verify All the fields are enable");
            VerifyEqual("6. Verify Last Name field is enable", true, !usersPage.UserDetailsPanel.IsLastNameInputReadOnly());
            VerifyEqual("6. Verify First Name field is enable", true, !usersPage.UserDetailsPanel.IsFirstNameInputReadOnly());
            VerifyEqual("6. Verify Phone field is enable", true, !usersPage.UserDetailsPanel.IsPhoneInputReadOnly());
            VerifyEqual("6. Verify Mobile field is enable", true, !usersPage.UserDetailsPanel.IsMobileInputReadOnly());
            VerifyEqual("6. Verify Email field is enable", true, !usersPage.UserDetailsPanel.IsEmailInputReadOnly());
            VerifyEqual("6. Verify Address field is enable", true, !usersPage.UserDetailsPanel.IsAddressInputReadOnly());

            Step("9. Update Phone, Mobile, Email, Address");
            Step("10. Verify All fields are editable");
            var newPhone = SLVHelper.GenerateStringInteger(99999);
            var newMobile = SLVHelper.GenerateStringInteger(99999);
            var newEmail = string.Format("{0}@{1}.com", SLVHelper.GenerateString(9), SLVHelper.GenerateString(5));
            var newAddress = SLVHelper.GenerateString(8);
            usersPage.UserDetailsPanel.EnterPhoneInput(newPhone);
            usersPage.UserDetailsPanel.EnterMobileInput(newMobile);
            usersPage.UserDetailsPanel.EnterEmailInput(newEmail);
            usersPage.UserDetailsPanel.EnterAddressInput(newAddress);

            Step("11. Press Save button and select the user again");
            usersPage.UserDetailsPanel.ClickSaveButton();
            usersPage.WaitForPreviousActionComplete();
            usersPage.WaitForHeaderMessageDisappeared();
            usersPage.UserListPanel.SelectUser(newUserModel.FullName);
            usersPage.WaitForUserDetailsDisplayed();

            Step("12. Verify All fields are updated new values");
            VerifyEqual("12 Verify Phone is updated new value", newPhone, usersPage.UserDetailsPanel.GetPhoneValue());
            VerifyEqual("12 Verify Mobile is updated new value", newMobile, usersPage.UserDetailsPanel.GetMobileValue());
            VerifyEqual("12 Verify Email is updated new value", newEmail, usersPage.UserDetailsPanel.GetEmailValue());
            VerifyEqual("12 Verify Address is updated new value", newAddress, usersPage.UserDetailsPanel.GetAddressValue());

            try
            {                
                DeleteUserAndProfile(newUserModel);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("SC-447 - User profiles with deleted geozones are not displayed in Users")]
        public void User_02()
        {     
            var defaultUserModel = Settings.Users["DefaultTest"];
            var geozone = SLVHelper.GenerateUniqueName("GZNU02");

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - Create a testing geozone");
            Step("**** Precondition ****\n");

            Step("-> Create data for testing");
            DeleteGeozones("GZNU02*");
            DeleteProfiles("User02.*");
            CreateNewGeozone(geozone);            

            Step("1. Login to system and go to Users app");
            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(defaultUserModel.Username, defaultUserModel.Password);
            desktopPage.InstallAppsIfNotExist(App.Users, App.EquipmentInventory);

            Step("2. Verify Users app is loaded successfully");
            var usersPage = desktopPage.GoToApp(App.Users) as UsersPage;

            Step("3. Create a new profile and type a few letters of the testing geozone in the Geozone field.");
            Step("4. Verify The auto field displays in the Geozone field with the testing geozone's name (cover SC-1921)");
            Step("5. Create a new user of the profile");
            Step("6. Verify The user is created successfully");
            var newUserModel = usersPage.CreateNewProfileAndUser();
            usersPage.UserProfileDetailsPanel.EnterGeozoneInputAutoComplete(geozone);
            usersPage.UserProfileDetailsPanel.ClickSaveButton();
            usersPage.WaitForPreviousActionComplete();
            usersPage.WaitForHeaderMessageDisappeared();

            Step("7. Refresh the page, then go to Equipment Inventory and select the testing geozone and try to delete it");
            desktopPage = Browser.RefreshLoggedInCMS();
            var equipmentInventoryPage = desktopPage.GoToApp(App.EquipmentInventory) as EquipmentInventoryPage;
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(geozone);
            equipmentInventoryPage.WaitForGeozoneEditorPanelDisplayed();
            equipmentInventoryPage.GeozoneEditorPanel.ClickDeleteButton();
            equipmentInventoryPage.WaitForPopupDialogDisplayed();
            equipmentInventoryPage.Dialog.ClickYesButton();
            equipmentInventoryPage.WaitForPreviousActionComplete();
            equipmentInventoryPage.WaitForPopupDialogDisplayed();

            Step("8. Verify The pop-up displays to prevent user to detete the geozone");
            Step(" o Title: Error");
            Step(" o Description: 'Can't delete GeoZone[id:###,'testing geozone name'] as it's used by the following user profiles: ['profile name' (###)]");
            var messagePattern = string.Format(@"Can't delete GeoZone\[id:(\d*),{0}\] as it's used by the following user profiles: \[{1} \(\d*\)\]", geozone, newUserModel.Profile);
            var dialogMessage = equipmentInventoryPage.Dialog.GetMessageText();
            VerifyEqual("8. Verify Title of The pop-up is Error", "Error", equipmentInventoryPage.Dialog.GetDialogTitleText());            
            VerifyTrue("8. Verify Description of The pop-up: 'Can't delete GeoZone[id:###,'testing geozone name'] as it's used by the following user profiles: ['profile name' (###)]", Regex.IsMatch(dialogMessage, messagePattern), messagePattern, dialogMessage);
            
            Step("9. Press OK button");
            equipmentInventoryPage.Dialog.ClickOkButton();
            equipmentInventoryPage.WaitForPopupDialogDisappeared();

            Step("10. Verify The geozone is not deleted");
            var selectedNode = equipmentInventoryPage.GeozoneTreeMainPanel.GetSelectedNodeName();
            VerifyEqual("8. Verify The geozone is not deleted", geozone, selectedNode);

            Step("11. Go to Users app and check");
            usersPage = equipmentInventoryPage.AppBar.SwitchTo(App.Users) as UsersPage;
           
            Step("12. Verify The testing profile and user are still available.");
            var profiles = usersPage.UserProfileListPanel.GetListOfProfiles();
            VerifyEqual("12. Verify The testing profile is still available", true, profiles.Contains(newUserModel.Profile));
            usersPage.UserProfileListPanel.SelectProfile(newUserModel.Profile);
            var users = usersPage.UserListPanel.GetUsersListName();
            VerifyEqual("12. Verify The user is still available", true, users.Contains(newUserModel.Username));

            try
            {
                DeleteUserAndProfile(newUserModel);
                DeleteGeozone(geozone);
            }
            catch { }
        }

        #endregion //Test Cases

        #region Private methods        

        #region Verify methods        

        #endregion //Verify methods

        #region Input XML data 
        
        #endregion //Input XML data       

        #endregion //Private methods
    }
}