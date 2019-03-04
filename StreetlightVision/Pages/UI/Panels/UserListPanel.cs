using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using StreetlightVision.Extensions;
using StreetlightVision.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StreetlightVision.Pages.UI
{
    public class UserListPanel : PanelBase
    {
        #region Variables

        #endregion //Variables

        #region IWebElements
        
        [FindsBy(How = How.CssSelector, Using = "[id$='profileEditor-userButtons'] [id='tb_profileToolbar_item_add'] > table")]
        private IWebElement addNewButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='profileEditor-users-list'] .user-item")]
        private IList<IWebElement> usersList;

        [FindsBy(How = How.CssSelector, Using = "[id$='profileEditor-users-list'] .user-item.item-selected")]
        private IWebElement selectedUser;

        #endregion //IWebElements

        #region Constructor

        public UserListPanel(IWebDriver driver, PageBase page)
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

        #endregion //Actions

        #region Get methods

        #endregion //Get methods

        #endregion //Basic methods

        #region Business methods

        /// <summary>
        /// Select a user with a specific user name
        /// </summary>
        /// <param name="name"></param>
        public void SelectUser(string name)
        {
            var user = usersList.FirstOrDefault(p => string.Equals(p.Text, name, StringComparison.InvariantCultureIgnoreCase));
            user.ClickEx();
        }

        /// <summary>
        /// Get current selected user
        /// </summary>
        /// <returns></returns>
        public string GetSelectedUserText()
        {
            if (selectedUser != null)
                return selectedUser.Text;
            return string.Empty;
        }        

        /// <summary>
        /// Get list name text of user list
        /// </summary>
        /// <returns></returns>
        public List<string> GetUsersListName()
        {       
            return JSUtility.GetElementsText("[id$='profileEditor-users-list'] .user-item .user-item-title").Where(p => !string.IsNullOrEmpty(p)).ToList();
        }

        #endregion //Business methods

        public override void WaitForPanelLoaded()
        {
            Wait.ForElementsDisplayed(usersList);
        }

        public override void WaitForPreviousActionComplete()
        {
            base.WaitForPreviousActionComplete();
        }
    }
}
