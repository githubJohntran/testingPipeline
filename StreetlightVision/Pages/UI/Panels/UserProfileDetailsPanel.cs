using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using StreetlightVision.Extensions;
using StreetlightVision.Models;
using StreetlightVision.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace StreetlightVision.Pages.UI
{
    public class UserProfileDetailsPanel : PanelBase
    {
        #region Variables

        #endregion //Variables

        #region IWebElements
        
        [FindsBy(How = How.CssSelector, Using = "[id$='profileEditor-buttons'] [id='tb_profileButtons_item_save'] > table")]
        private IWebElement saveButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='profileEditor-buttons'] [id='tb_profileButtons_item_delete'] > table")]
        private IWebElement deleteButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='profileEditor-content-name-label']")]
        private IWebElement profileNameLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='profileEditor-content-name-value']")]
        private IWebElement profileNameValueLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='profileEditor-content-name-field']")]
        private IWebElement profileNameInput;

        [FindsBy(How = How.CssSelector, Using = "div[id$='profileEditor-content-language-label']")]
        private IWebElement languageLabel;

        [FindsBy(How = How.CssSelector, Using = "div[id$='profileEditor-content-language-field']")]
        private IWebElement languageDropDown;

        [FindsBy(How = How.CssSelector, Using = "div[id$='profileEditor-content-geozone-label']")]
        private IWebElement geozoneLabel;

        [FindsBy(How = How.CssSelector, Using = "input[id$='profileEditor-content-geozone-field']")]
        private IWebElement geozoneInput;

        [FindsBy(How = How.CssSelector, Using = "div[id$='profileEditor-content-skin-label']")]
        private IWebElement skinLabel;

        [FindsBy(How = How.CssSelector, Using = "div[id$='profileEditor-content-skin-field']")]
        private IWebElement skinDropDown;

        [FindsBy(How = How.CssSelector, Using = "div[id$='profileEditor-content-applications']")]
        private IWebElement applicationsLabel;        

        [FindsBy(How = How.CssSelector, Using = "[id$='profileEditor-content-applications'] .user-app-item")]
        private IList<IWebElement> allApplicationsList;

        [FindsBy(How = How.CssSelector, Using = "[id$='profileEditor-content-applications'] .user-app-item:not(.user-app-item-disable)")]
        private IList<IWebElement> availableApplicationsList;

        [FindsBy(How = How.CssSelector, Using = "[id$='profileEditor-content-applications'] .user-app-item.user-app-item-disable")]
        private IList<IWebElement> disabledApplicationsList;

        [FindsBy(How = How.CssSelector, Using = "div[id$='profileEditor-content-blockedActions-label']")]
        private IWebElement blockActionsLabel;

        [FindsBy(How = How.CssSelector, Using = "div[id$='profileEditor-content-blockedaction-field']")]
        private IWebElement blockActionsDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id$='profileEditor-content-blockedActions-list']")]
        private IWebElement blockedActionsContainer;

        [FindsBy(How = How.CssSelector, Using = "[id$='profileEditor-content-blockedActions-list'] .user-action-item")]
        private IList<IWebElement> blockedActionsList;

        #endregion //IWebElements

        #region Constructor

        public UserProfileDetailsPanel(IWebDriver driver, PageBase page)
            : base(driver, page)
        {
            PageFactory.InitElements(this, new RetryingElementLocator(driver, TimeSpan.FromMilliseconds(Settings.LocatorRetryingTimeout), TimeSpan.FromMilliseconds(Settings.LocatorPollingInterval)));
        }

        #endregion //Constructor

        #region Basic methods        

        #region Actions

        /// <summary>
        /// Click 'Save' button
        /// </summary>
        public void ClickSaveButton()
        {
            saveButton.ClickEx();
        }

        /// <summary>
        /// Click 'Delete' button
        /// </summary>
        public void ClickDeleteButton()
        {
            deleteButton.ClickEx();
        }

        /// <summary>
        /// Focus 'ProfileName' input
        /// </summary>
        public void FocusProfileNameInput()
        {
            profileNameInput.ClickEx();
        }

        /// <summary>
        /// Enter a value for 'ProfileName' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterProfileNameInput(string value)
        {
            profileNameInput.Enter(value);
        }

        /// <summary>
        /// Select an item of 'Language' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectLanguageDropDown(string value)
        {
            languageDropDown.Select(value);
        }

        /// <summary>
        /// Enter 'Geozone' input value
        /// </summary>
        /// <param name="value"></param>
        public void EnterGeozoneInput(string value)
        {
            geozoneInput.Enter(value);
        }

        /// <summary>
        /// Enter 'Geozone' input value with auto complete
        /// </summary>
        /// <param name="value"></param>
        public void EnterGeozoneInputAutoComplete(string value)
        {
            geozoneInput.Enter(value, true, true);
        }

        /// <summary>
        /// Select an item of 'Skin' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectSkinDropDown(string value)
        {
            skinDropDown.Select(value);
        }

        /// <summary>
        /// Select an item of 'BlockActions' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectBlockActionsDropDown(string value)
        {
            blockActionsDropDown.Select(value);
        }

        #endregion //Actions

        #region Get methods

        /// <summary>
        /// Get 'ProfileName' label text
        /// </summary>
        /// <returns></returns>
        public string GetProfileNameText()
        {
            return profileNameLabel.Text;
        }

        /// <summary>
        /// Get 'ProfileNameValue' label text
        /// </summary>
        /// <returns></returns>
        public string GetProfileNameValueText()
        {
            return profileNameValueLabel.Text;
        }

        /// <summary>
        /// Get 'ProfileName' input value
        /// </summary>
        /// <returns></returns>
        public string GetProfileNameValue()
        {
            return profileNameInput.Value();
        }

        /// <summary>
        /// Get 'Language' label text
        /// </summary>
        /// <returns></returns>
        public string GetLanguageText()
        {
            return languageLabel.Text;
        }

        /// <summary>
        /// Get 'Language' input value
        /// </summary>
        /// <returns></returns>
        public string GetLanguageValue()
        {
            return languageDropDown.Text;
        }

        /// <summary>
        /// Get 'Geozone' label text
        /// </summary>
        /// <returns></returns>
        public string GetGeozoneText()
        {
            return geozoneLabel.Text;
        }

        /// <summary>
        /// Get 'Geozone' input value
        /// </summary>
        /// <returns></returns>
        public string GetGeozoneValue()
        {
            return geozoneInput.Value();
        }

        /// <summary>
        /// Get 'Skin' label text
        /// </summary>
        /// <returns></returns>
        public string GetSkinText()
        {
            return skinLabel.Text;
        }

        /// <summary>
        /// Get 'Skin' input value
        /// </summary>
        /// <returns></returns>
        public string GetSkinValue()
        {
            return skinDropDown.Text;
        }

        /// <summary>
        /// Get 'Applications' label text
        /// </summary>
        /// <returns></returns>
        public string GetApplicationsText()
        {
            return applicationsLabel.Text;
        }

        /// <summary>
        /// Get 'BlockActions' label text
        /// </summary>
        /// <returns></returns>
        public string GetBlockActionsText()
        {
            return blockActionsLabel.Text;
        }

        /// <summary>
        /// Get 'BlockActions' input value
        /// </summary>
        /// <returns></returns>
        public string GetBlockActionsValue()
        {
            return blockActionsDropDown.Text;
        }

        #endregion //Get methods

        #endregion //Basic methods

        #region Business methods

        public void WaitForNameDisplayed(string name)
        {
            Wait.ForElementValue(profileNameInput);
        }

        /// <summary>
        /// Un-check random apps with specific number count
        /// </summary>
        /// <param name="count"></param>
        public void UncheckRandomApps(int count)
        {
            var apps = new List<IWebElement>(availableApplicationsList);
            apps.RemoveAll(p => p.Text.Equals(App.Users));
            apps.RemoveAll(p => p.Text.Equals(App.BackOffice)); // An issue: cannot un-check this app

            var randomApps = apps.PickRandom(count);
            foreach (var app in randomApps)
            {
                var checkbox = app.FindElement(By.CssSelector(".checkbox"));
                checkbox.Check(false);
            }
        }

        /// <summary>
        /// Un-check apps
        /// </summary>
        /// <param name="apps"></param>
        public void UncheckApps(params string[] apps)
        {
            var availableApps = new List<IWebElement>(availableApplicationsList);            
            foreach (var app in apps)
            {
                var appElement = availableApps.FirstOrDefault(p => app.Equals(p.Text.Trim()));
                if (appElement != null)
                {
                    var checkbox = appElement.FindElement(By.CssSelector(".checkbox"));
                    checkbox.Check(false);
                }
            }
        }

        /// <summary>
        /// Check apps
        /// </summary>
        /// <param name="apps"></param>
        public void CheckApps(params string[] apps)
        {
            var disabledApps = new List<IWebElement>(disabledApplicationsList);
            foreach (var app in apps)
            {
                var appElement = disabledApps.FirstOrDefault(p => app.Equals(p.Text.Trim()));
                if (appElement != null)
                {
                    var checkbox = appElement.FindElement(By.CssSelector(".checkbox"));
                    checkbox.Check(true);
                }
            }
        }

        /// <summary>
        /// Reset all blocked Apps
        /// </summary>
        public void ResetBlockedApps()
        {
            var apps = new List<IWebElement>(disabledApplicationsList);
            foreach (var app in apps)
            {
                var checkbox = app.FindElement(By.CssSelector(".checkbox"));
                checkbox.Check(true);
            }
        }

        /// <summary>
        /// Un-check all blocked Apps
        /// </summary>
        public void UncheckAllApps()
        {
            var apps = new List<IWebElement>(allApplicationsList);
            foreach (var app in apps)
            {
                var checkbox = app.FindElement(By.CssSelector(".checkbox"));
                checkbox.Check(false);
            }
            Wait.ForSeconds(2);
        }

        /// <summary>
        /// Get all apps name
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllAppsName()
        {
            return allApplicationsList.Select(p => p.FindElement(By.CssSelector("[id$='title']")).Text).ToList();
        }

        /// <summary>
        /// Get all available apps name
        /// </summary>
        /// <returns></returns>
        public List<string> GetAvailableAppsName()
        {
            return availableApplicationsList.Select(p => p.FindElement(By.CssSelector("[id$='title']")).Text).ToList();
        }

        /// <summary>
        /// Get all disabled apps name
        /// </summary>
        /// <returns></returns>
        public List<string> GetDisabledAppsName()
        {
            return disabledApplicationsList.Select(p => p.FindElement(By.CssSelector("[id$='title']")).Text).ToList();
        }

        /// <summary>
        /// Remove all current blocked actions
        /// </summary>
        public void RemoveAllBlockedActions()
        {
            blockedActionsContainer.ScrollToElementByJS();
            if (Driver.FindElements(By.CssSelector("[id$='profileEditor-content-blockedActions-list'] .user-action-item")).Count > 0)
            {
                foreach (var item in blockedActionsList)
                {
                    var removeBy = By.CssSelector(".slv-item-over div.icon-delete.user-action-item-button");
                    //Hover the mouse to display "Remove" button
                    item.MoveTo();
                    Wait.ForElementStyle(removeBy, "visibility: visible");
                    var removeBtn = item.FindElement(removeBy);
                    removeBtn.ClickEx();
                    Wait.ForMilliseconds(100);
                }
            }           
        }

        /// <summary>
        /// Remove a specific blocked action
        /// </summary>
        /// <param name="name"></param>
        public void RemoveBlockedAction(string name)
        {
            blockedActionsContainer.ScrollToElementByJS();
            if (Driver.FindElements(By.CssSelector("[id$='profileEditor-content-blockedActions-list'] .user-action-item")).Count > 0)
            {
                var item = blockedActionsList.FirstOrDefault(p => p.Text.Equals(name));
                var removeBy = By.CssSelector(".slv-item-over div.icon-delete.user-action-item-button");                
                item.MoveTo();
                Wait.ForElementStyle(removeBy, "visibility: visible");
                var removeBtn = item.FindElement(removeBy);
                removeBtn.ClickEx();
                Wait.ForMilliseconds(100);
            }
        }

        /// <summary>
        /// Get blocked actions count
        /// </summary>
        /// <returns></returns>
        public int GetBlockedActionsCount()
        {
            return Driver.FindElements(By.CssSelector("[id$='profileEditor-content-blockedActions-list'] .user-action-item")).Count;
        }

        /// <summary>
        /// Get current blocked actions name
        /// </summary>
        /// <returns></returns>
        public List<string> GetBlockedActionsName()
        {
            blockedActionsContainer.ScrollToElementByJS();
            return blockedActionsList.Select(p => p.Text).ToList();
        }

        /// <summary>
        /// Get all items of 'BlockActions' dropdown 
        /// </summary>
        /// <returns></returns>
        public List<string> GetListOfBlockedActionsName()
        {
            return blockActionsDropDown.GetAllItems();
        }

        /// <summary>
        /// Select all items of 'BlockActions' dropdown 
        /// </summary>
        public void SelectAllBlockedActions()
        {
            var blockedActions = blockActionsDropDown.GetAllItems();
            foreach (var action in blockedActions)
            {
                blockActionsDropDown.Select(action);
            }
        }

        /// <summary>
        /// Select items of 'BlockActions' dropdown 
        /// </summary>
        public void SelectBlockedActions(List<string> blockedActions)
        {
            foreach (var action in blockedActions)
            {
                blockActionsDropDown.Select(action);
            }
        }

        /// <summary>
        /// Get all autocomplete items of 'Geozone' input 
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllAutoCompleteGeozoneItems()
        {
            return geozoneInput.GetAllAutoCompleteItems();
        }

        #endregion //Business methods

        public override void WaitForPanelLoaded()
        {
            Wait.ForElementText(profileNameValueLabel);
            Wait.ForElementsDisplayed(allApplicationsList);            
        }

        public override void WaitForPreviousActionComplete()
        {
            base.WaitForPreviousActionComplete();
        }
    }
}
