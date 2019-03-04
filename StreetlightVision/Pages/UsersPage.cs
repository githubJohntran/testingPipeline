using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using System;
using StreetlightVision.Extensions;
using StreetlightVision.Pages.UI;
using System.Threading;
using System.Linq;
using StreetlightVision.Utilities;
using StreetlightVision.Models;
using NUnit.Framework;

namespace StreetlightVision.Pages
{
    public class UsersPage : PageBase
    {
        #region Variables
        
        private UserProfileListPanel _userProfileListPanel;
        private UserProfileDetailsPanel _userProfileDetailsPanel;
        private UserListPanel _userListPanel;
        private UserDetailsPanel _userDetailsPanel;

        #endregion //Variables

        #region IWebElements


        #endregion //IWebElements

        #region Constructor

        public UsersPage(IWebDriver driver)
            : base(driver)                                                                                                                                                                                                                                                                                                                                                                             
        {
            PageFactory.InitElements(this, new RetryingElementLocator(driver, TimeSpan.FromMilliseconds(Settings.LocatorRetryingTimeout), TimeSpan.FromMilliseconds(Settings.LocatorPollingInterval)));
            WaitForPageReady();
        }

        #endregion //Constructor

        #region Properties        

        public UserProfileListPanel UserProfileListPanel
        {
            get
            {
                if (_userProfileListPanel == null)
                {
                    _userProfileListPanel = new UserProfileListPanel(this.Driver, this);
                }

                return _userProfileListPanel;
            }
        }

        public UserProfileDetailsPanel UserProfileDetailsPanel
        {
            get
            {
                if (_userProfileDetailsPanel == null)
                {
                    _userProfileDetailsPanel = new UserProfileDetailsPanel(this.Driver, this);
                }

                return _userProfileDetailsPanel;
            }
        }

        public UserListPanel UserListPanel
        {
            get
            {
                if (_userListPanel == null)
                {
                    _userListPanel = new UserListPanel(this.Driver, this);
                }

                return _userListPanel;
            }
        }

        public UserDetailsPanel UserDetailsPanel
        {
            get
            {
                if (_userDetailsPanel == null)
                {
                    _userDetailsPanel = new UserDetailsPanel(this.Driver, this);
                }

                return _userDetailsPanel;
            }
        }

        #endregion //Properties

        #region Basic methods

        #region Actions

        #endregion //Actions

        #region Get methods

        #endregion //Get methods

        #endregion //Basic methods

        #region Business methods

        public void WaitForSkinAndLanguageApplied()
        {
            Wait.ForSeconds(2);
        }

        public void WaitForProfileDetailsNameDisplayed()
        {
            Wait.ForElementDisplayed(By.CssSelector("[id$='profileEditor-content-name-field']"));
        }

        public void WaitForProfileDetailsNameDisappeared()
        {
            Wait.ForElementInvisible(By.CssSelector("[id$='profileEditor-content-name-field']"));
        }

        public void WaitForUserDetailsDisplayed()
        {
            Wait.ForElementStyle(By.CssSelector("[id$='profileEditor-usersContainer']"), "left: 0px");
        }

        public void WaitForUserDetailsDisappeared()
        {
            Wait.ForElementStyle(By.CssSelector("[id$='profileEditor-usersContainer']"), "left: 410px");
        }

        public void WaitForDragAndDropCompleted()
        {
            Wait.ForSeconds(1);
        }

        /// <summary>
        /// Create a new profile and add an user to it
        /// </summary>
        /// <param name="profileName"></param>
        /// <param name="profileLanguage"></param>
        /// <param name="profileSkin"></param>
        /// <param name="profileUser"></param>
        /// <param name="userPassword"></param>
        /// <param name="userEmail"></param>
        public void CreateNewProfileAndUser(string profileName, string profileLanguage, string profileSkin, string profileUser, string userPassword, string userEmail)
        {
            UserProfileListPanel.ClickAddNewButton();
            WaitForProfileDetailsNameDisplayed();
            UserProfileDetailsPanel.FocusProfileNameInput();
            UserProfileDetailsPanel.EnterProfileNameInput(profileName);
            if(!string.IsNullOrEmpty(profileLanguage))
                UserProfileDetailsPanel.SelectLanguageDropDown(profileLanguage);
            if (!string.IsNullOrEmpty(profileSkin))
                UserProfileDetailsPanel.SelectSkinDropDown(profileSkin);

            UserProfileDetailsPanel.ClickSaveButton();
            WaitForPreviousActionComplete();
            WaitForHeaderMessageDisappeared();

            UserListPanel.ClickAddNewButton();
            WaitForUserDetailsDisplayed();
            UserDetailsPanel.FocusLastNameInput();
            UserDetailsPanel.EnterLastNameInput(profileUser);
            UserDetailsPanel.EnterLoginInput(profileUser);
            UserDetailsPanel.EnterPasswordInput(userPassword);
            UserDetailsPanel.EnterConfirmPasswordInput(userPassword);
            UserDetailsPanel.EnterEmailInput(userEmail);

            UserDetailsPanel.ClickSaveButton();

            WaitForPreviousActionComplete();
            WaitForHeaderMessageDisappeared();
        }

        /// <summary>
        /// Create new profile and user with current test method name
        /// </summary>
        /// <returns></returns>
        public UserModel CreateNewProfileAndUser(string language = "", string skin = "")
        {
            var userModel = new UserModel();
            var timetamp = DateTime.Now.Timestamp();
            var method = TestContext.CurrentContext.Test.MethodName.Replace("_", string.Empty);
            userModel.Profile = string.Format("{0}{1} Profile", method, timetamp);
            userModel.Username = string.Format("{0}{1}", method, timetamp);
            userModel.Password = Settings.Users["DefaultTest"].Password;
            userModel.Email = Settings.Users["DefaultTest"].Email;
            userModel.FullName = userModel.Username;
            CreateNewProfileAndUser(userModel.Profile, language, skin, userModel.Username, userModel.Password, userModel.Email);

            return userModel;
        }

        /// <summary>
        /// Drag and drop an user of profile to another profile
        /// </summary>
        /// <param name="sourceProfile"></param>
        /// <param name="destProfile"></param>
        /// <param name="user"></param>
        public void DragAndDropUserToProfile(string sourceProfile, string destProfile, string user)
        {
            UserProfileListPanel.SelectProfile(sourceProfile);

            var profiles = Driver.FindElements(By.CssSelector("[id$='profileList'] [id$='user-list'] div.user-profile-item"));

            var elementProfileSource = profiles.FirstOrDefault(p => p.Text.Equals(sourceProfile));            
            var elementProfileDest = profiles.FirstOrDefault(p => p.Text.Equals(destProfile));
            elementProfileSource.ClickEx();

            var users = Driver.FindElements(By.CssSelector("[id$='profileEditor-users-list'] .user-item"));
            var elementUser = users.FirstOrDefault(p => p.Text.Equals(user));

            JSUtility.DragAndDropByJS(elementUser, elementProfileDest);
            WaitForDragAndDropCompleted();
        }

        /// <summary>
        /// Remove a user profile
        /// </summary>
        /// <param name="name"></param>
        public void RemoveProfile(string name)
        {
            UserProfileListPanel.SelectProfile(name);

            var users = UserListPanel.GetUsersListName();

            for (int i = 0; i < users.Count; i++)
            {
                UserListPanel.SelectUser(users[i]);
                WaitForUserDetailsDisplayed();
                UserDetailsPanel.ClickDeleteButton();
                WaitForPopupDialogDisplayed();
                Dialog.ClickYesButton();
                WaitForPreviousActionComplete();
                WaitForHeaderMessageDisappeared();
            }

            UserProfileDetailsPanel.ClickDeleteButton();
            WaitForPopupDialogDisplayed();
            Dialog.ClickYesButton();
            WaitForPreviousActionComplete();
            WaitForHeaderMessageDisappeared();
        }

        #endregion //Business methods

        protected override void WaitForPageReady()
        {
            base.WaitForPageReady();
            UserProfileListPanel.WaitForPanelLoaded();
            UserProfileDetailsPanel.WaitForPanelLoaded();
            UserListPanel.WaitForPanelLoaded();
        }        
    }
}
