using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using StreetlightVision.Extensions;
using StreetlightVision.Utilities;
using System;
using System.Collections.Generic;

namespace StreetlightVision.Pages.UI
{
    public class ReplaceNodesPanel : PanelBase
    {
        #region Variables

        #endregion //Variables

        #region IWebElements

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-replace-nodes-backButton']")]
        private IWebElement backButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-replace-nodes-title'] .equipment-gl-editor-title-label")]
        private IWebElement panelTitle;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-replace-nodes-buttons-toolbar_item_reload'] table.w2ui-button")]
        private IWebElement reloadButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-replace-nodes-content'] .equipment-gl-editor-message div")]
        private IWebElement messageCaption;        

        #endregion //IWebElements

        #region Constructor

        public ReplaceNodesPanel(IWebDriver driver, PageBase page)
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
        /// Click 'Reload' button
        /// </summary>
        public void ClickReloadButton()
        {
            reloadButton.ClickEx();
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

        /// <summary>
        /// Get 'messageCaption' text
        /// </summary>
        /// <returns></returns>
        public string GetMessageCaptionText()
        {
            return messageCaption.Text;
        }

        #endregion //Get methods

        #endregion //Basic methods

        #region Business methods

        public List<string> GetListOfMessageIcons()
        {
            var result = new List<string>();
            var iconElements = Driver.FindElements(By.CssSelector("[id$='editor-geozone-replace-nodes-messages'] .equipment-gl-list-item .equipment-gl-list-item-icon"));
            foreach (var element in iconElements)
            {
                var backgroundImg = element.GetStyleAttr("background-image");
                var url = backgroundImg.Replace("url(\"", string.Empty).Replace("\")", string.Empty);
                result.Add(url);
            }

            return result;
        }

        public List<string> GetListOfMessages()
        {
            return JSUtility.GetElementsText("[id$='editor-geozone-replace-nodes-messages'] .equipment-gl-list-item .equipment-gl-list-item-label");
        }        

        public string GetMessageTooltip(string device)
        {
            var messageElements = Driver.FindElements(By.CssSelector("[id$='editor-geozone-replace-nodes-messages'] .equipment-gl-list-item"));
            foreach (var element in messageElements)
            {
                var tooltip = element.GetAttribute("title");
                if (tooltip.Contains(device))
                    return tooltip;
            }

            return string.Empty;
        }

        #endregion //Business methods        
    }
}
