using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using StreetlightVision.Extensions;
using StreetlightVision.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StreetlightVision.Pages.UI
{
    public class UserProfileListPanel : PanelBase
    {
        #region Variables

        #endregion //Variables

        #region IWebElements
        
        [FindsBy(How = How.CssSelector, Using = "[id$='profileList'] [id='tb_userToolbar_item_add'] > table")]
        private IWebElement addNewButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='profileList'] [id$='user-list'] div.user-profile-item")]
        private IList<IWebElement> userProfilesList;

        [FindsBy(How = How.CssSelector, Using = "[id$='profileList'] [id$='user-list'] div.user-profile-item.slv-item-selected")]
        private IWebElement selectedUserProfile;

        [FindsBy(How = How.CssSelector, Using = "[id$='profileList'] [id$='user-search'] [id$='user-input'].slv-search-input")]
        private IWebElement seachProfileInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='profileList'] [id$='user-search'] .slv-search-button")]
        private IWebElement seachProfileButton;

        #endregion //IWebElements

        #region Constructor

        public UserProfileListPanel(IWebDriver driver, PageBase page)
            : base(driver, page)
        {
            PageFactory.InitElements(this, new RetryingElementLocator(driver, TimeSpan.FromMilliseconds(Settings.LocatorRetryingTimeout), TimeSpan.FromMilliseconds(Settings.LocatorPollingInterval)));
        }

        #endregion //Constructor

        #region Basic methods

        #region Actions

        /// <summary>
        /// Click 'AddNew' button
        /// </summary>
        public void ClickAddNewButton()
        {
            addNewButton.ClickEx();
        }

        /// <summary>
        /// Enter a value for 'SeachProfile' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterSeachProfileInput(string value)
        {
            seachProfileInput.Enter(value);
        }

        /// <summary>
        /// Click 'SeachProfile' button
        /// </summary>
        public void ClickSeachProfileButton()
        {
            seachProfileButton.ClickEx();
        }

        #endregion //Actions

        #region Get methods

        /// <summary>
        /// Get 'SeachProfile' input value
        /// </summary>
        /// <returns></returns>
        public string GetSeachProfileValue()
        {
            return seachProfileInput.Value();
        }

        #endregion //Get methods

        #endregion //Basic methods

        #region Business methods

        /// <summary>
        /// Select a user profile with a specific profile name
        /// </summary>
        /// <param name="name"></param>
        public void SelectProfile(string name)
        {
            var profile = userProfilesList.FirstOrDefault(p => string.Equals(p.Text, name, StringComparison.InvariantCultureIgnoreCase));
            profile.ClickEx();
        }

        /// <summary>
        /// Get current selected profile
        /// </summary>
        /// <returns></returns>
        public string GetSelectedProfileText()
        {
            if (selectedUserProfile != null)
                return selectedUserProfile.Text;
            return string.Empty;
        }       

        public List<string> GetListOfProfiles()
        {
            return JSUtility.GetElementsText("[id$='profileList'] [id$='user-list'] div.user-profile-item .user-profile-item-title");
        }

        #endregion //Business methods

        public override void WaitForPanelLoaded()
        {
            Wait.ForElementsDisplayed(userProfilesList);
        }

        public override void WaitForPreviousActionComplete()
        {
            base.WaitForPreviousActionComplete();
        }
    }
}
