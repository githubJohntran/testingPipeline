using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using StreetlightVision.Extensions;
using StreetlightVision.Utilities;
using System;
using System.Collections.Generic;

namespace StreetlightVision.Pages.UI
{
    public class ImportPanel : PanelBase
    {
        #region Variables

        #endregion //Variables

        #region IWebElements

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-import-backButton']")]
        private IWebElement backButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-import-title'] .equipment-gl-editor-title-label")]
        private IWebElement panelTitle;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-import-buttons-toolbar_item_commission'] table.w2ui-button")]
        private IWebElement commissionButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-import-buttons-toolbar_item_reload'] table.w2ui-button")]
        private IWebElement reloadButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-import-content'] .equipment-gl-editor-message div")]
        private IWebElement messageCaption;        

        #endregion //IWebElements

        #region Constructor

        public ImportPanel(IWebDriver driver, PageBase page)
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
        /// Click 'Commission' button
        /// </summary>
        public void ClickCommissionButton()
        {
            commissionButton.ClickEx();
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

        public List<string> GetListOfErrors()
        {
            return JSUtility.GetElementsText("[id$='editor-geozone-import-errors'] .equipment-gl-list-item .equipment-gl-list-item-label");
        }

        #endregion //Business methods        
    }
}
