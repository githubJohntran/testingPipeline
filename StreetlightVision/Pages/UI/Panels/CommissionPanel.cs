using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using StreetlightVision.Extensions;
using StreetlightVision.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StreetlightVision.Pages.UI
{
    public class CommissionPanel : PanelBase
    {
        #region Variables

        #endregion //Variables

        #region IWebElements

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-device-commission-backButton']")]
        private IWebElement backButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-device-commission'] .equipment-gl-editor-title-label")]
        private IWebElement panelTitle;        

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-device-commission'] button.equipment-gl-commission-button")]
        private IWebElement launchButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-device-commission'] .equipment-gl-list-item")]
        private IList<IWebElement> allSettingList;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-device-commission'] .equipment-gl-list-item:not(.equipment-gl-list-item-disable)")]
        private IList<IWebElement> checkedSettingList;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-device-commission'] .equipment-gl-list-item.equipment-gl-list-item-disable")]
        private IList<IWebElement> uncheckedSettingList;       

        #endregion //IWebElements

        #region Constructor

        public CommissionPanel(IWebDriver driver, PageBase page)
            : base(driver, page)
        {
            PageFactory.InitElements(this, new RetryingElementLocator(driver, TimeSpan.FromMilliseconds(Settings.LocatorRetryingTimeout), TimeSpan.FromMilliseconds(Settings.LocatorPollingInterval)));

            WaitForPanelLoaded();
        }

        #endregion //Constructor

        #region Basic methods

        #region Actions

        /// <summary>
        /// Click 'Back' button
        /// </summary>
        public void ClickBackButton()
        {
           backButton.ClickEx();
        }

        /// <summary>
        /// Click 'Launch' button
        /// </summary>
        public void ClickLaunchButton()
        {
            launchButton.ClickEx();
        }

        #endregion //Actions

        #region Get methods

        /// <summary>
        /// Get 'PanelTitle' text
        /// </summary>
        /// <returns></returns>
        public string GetPanelTitleText()
        {
            return panelTitle.Text;
        }

        #endregion //Get methods

        #endregion //Basic methods

        #region Business methods

        public List<string> GetListOfSettings()
        {
            return JSUtility.GetElementsText("[id$='editor-device-commission'] .equipment-gl-list-item .equipment-gl-list-item-label");
        }

        public List<string> GetListOfCheckedSettings()
        {
            return JSUtility.GetElementsText("[id$='editor-device-commission'] .equipment-gl-list-item:not(.equipment-gl-list-item-disable) .equipment-gl-list-item-label");
        }

        public List<string> GetListOfUncheckedSettings()
        {
            return JSUtility.GetElementsText("[id$='editor-device-commission'] .equipment-gl-list-item.equipment-gl-list-item-disable .equipment-gl-list-item-label");
        }

        public List<string> GetListOfSections()
        {
            return JSUtility.GetElementsText("[id$='editor-device-commission'] .equipment-gl-editor-message-commission-title");
        }

        public List<string> GetListOfMessagesText()
        {
            return JSUtility.GetElementsText("[id$='editor-device-commission'] .equipment-gl-editor-message-commission-content > div");
        }

        public bool HaveAnyErrorMessages()
        {
            var messages = Driver.FindElements(By.CssSelector("[id$='editor-device-commission'] .equipment-gl-editor-message-commission-content"));

            foreach (var message in messages)
            {
                var img = message.FindElement(By.CssSelector("img"));
                if(img != null)
                {
                    var icon = img.GetAttribute("src");
                    if (icon.Contains("status-error.png"))
                        return true;
                }
            }

            return false;
        }

        public bool AreMessagesContainingIconAndText()
        {
            var messages = Driver.FindElements(By.CssSelector("[id$='editor-device-commission'] .equipment-gl-editor-message-commission-content"));

            foreach (var message in messages)
            {
                var img = message.FindElement(By.CssSelector("img"));
                var icon = img.GetAttribute("src");
                var text = message.FindElement(By.CssSelector("div")).Text;

                if (string.IsNullOrEmpty(text) || (!icon.Contains("status-ok.png") && !icon.Contains("status-error.png")))
                    return false;
            }

            return true;
        }

        public bool AreMessagesContainingIconPassedAndText()
        {
            var messages = Driver.FindElements(By.CssSelector("[id$='editor-device-commission'] .equipment-gl-editor-message-commission-content"));

            foreach (var message in messages)
            {
                var img = message.FindElement(By.CssSelector("img"));
                var icon = img.GetAttribute("src");
                var text = message.FindElement(By.CssSelector("div")).Text;

                if (string.IsNullOrEmpty(text) || (!icon.Contains("status-ok.png")))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Check all Setting
        /// </summary>
        public void CheckAllSettings()
        {
            var settings = new List<IWebElement>(allSettingList);
            foreach (var setting in settings)
            {
                var checkbox = setting.FindElement(By.CssSelector(".checkbox"));
                checkbox.Check(true);
            }
        }

        /// <summary>
        /// Un-check all Setting
        /// </summary>
        public void UncheckAllSettings()
        {
            var settings = new List<IWebElement>(allSettingList);
            foreach (var setting in settings)
            {
                var checkbox = setting.FindElement(By.CssSelector(".checkbox"));
                checkbox.Check(false);
            }
        }

        /// <summary>
        /// Un-check random Setting with specific number count
        /// </summary>
        /// <param name="count"></param>
        public void UncheckRandomSettings(int count)
        {
            var settings = new List<IWebElement>(checkedSettingList);
            var randomSettings = settings.PickRandom(count);
            foreach (var setting in randomSettings)
            {
                var checkbox = setting.FindElement(By.CssSelector(".checkbox"));
                checkbox.Check(false);
            }
        }

        /// <summary>
        /// check random Setting with specific number count
        /// </summary>
        /// <param name="count"></param>
        public void CheckRandomSettings(int count)
        {
            var settings = new List<IWebElement>(uncheckedSettingList);
            var randomSettings = settings.PickRandom(count);
            foreach (var setting in randomSettings)
            {
                var checkbox = setting.FindElement(By.CssSelector(".checkbox"));
                checkbox.Check(true);
            }
        }

        /// <summary>
        /// Check Device Type
        /// </summary>
        public void CheckSettings(params string[] settingName)
        {
            var settings = new List<IWebElement>(allSettingList);
            foreach (var setting in settings)
            {
                if (settingName.Contains(setting.Text))
                {
                    var checkbox = setting.FindElement(By.CssSelector(".checkbox"));
                    checkbox.Check(true);
                }
            }
        }

        /// <summary>
        /// Uncheck Device Type
        /// </summary>
        public void UncheckSettings(params string[] settingName)
        {
            var settings = new List<IWebElement>(allSettingList);
            foreach (var setting in settings)
            {
                if (settingName.Contains(setting.Text))
                {
                    var checkbox = setting.FindElement(By.CssSelector(".checkbox"));
                    checkbox.Check(false);
                }
            }
        }

        public void WaitForLaunchButtonDisplayed()
        {
            Wait.ForElementDisplayed(By.CssSelector("[id$='editor-device-commission'] button.equipment-gl-commission-button"));       
        }
        
        public void WaitForLaunchButtonDisappeared()
        {
            Wait.ForElementInvisible(By.CssSelector("[id$='editor-device-commission'] button.equipment-gl-commission-button"));
        }
        
        #endregion //Business methods
    }
}
