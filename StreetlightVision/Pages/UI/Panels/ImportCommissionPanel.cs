using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using StreetlightVision.Extensions;
using StreetlightVision.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StreetlightVision.Pages.UI
{
    public class ImportCommissionPanel : PanelBase
    {
        #region Variables

        #endregion //Variables

        #region IWebElements

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-import-commission-backButton']")]
        private IWebElement backButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-import-commission'] .equipment-gl-editor-title-label")]
        private IWebElement panelTitle;   

        #endregion //IWebElements

        #region Constructor

        public ImportCommissionPanel(IWebDriver driver, PageBase page)
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
            return JSUtility.GetElementsText("[id$='editor-geozone-import-commission'] .equipment-gl-list-item .equipment-gl-list-item-label");
        }
        
        public List<string> GetListOfSections()
        {
            return JSUtility.GetElementsText("[id$='editor-geozone-import-commission'] .equipment-gl-editor-message-commission-title");
        }

        public List<string> GetListOfMessagesText()
        {
            return JSUtility.GetElementsText("[id$='editor-geozone-import-commission'] .equipment-gl-editor-message-commission-content > div");
        }

        public List<string> GetListOfMessagesIcon()
        {
            var result = new List<string>();
            var icons = Driver.FindElements(By.CssSelector("[id$='editor-geozone-import-commission-content'] .equipment-gl-editor-message-commission-content img"));
            foreach (var icon in icons)
            {
                var src = icon.GetAttribute("src");
                result.Add(src);
            }

            return result;
        }        

        public bool HaveAnyErrorMessages()
        {
            var messages = Driver.FindElements(By.CssSelector("[id$='editor-geozone-import-commission'] .equipment-gl-editor-message-commission-content"));

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
            var messages = Driver.FindElements(By.CssSelector("[id$='editor-geozone-import-commission'] .equipment-gl-editor-message-commission-content"));

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
            var messages = Driver.FindElements(By.CssSelector("[id$='editor-geozone-import-commission'] .equipment-gl-editor-message-commission-content"));

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

        #endregion //Business methods
    }
}
