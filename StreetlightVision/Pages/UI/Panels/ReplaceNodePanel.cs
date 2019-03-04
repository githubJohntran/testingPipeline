using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using StreetlightVision.Extensions;
using System;

namespace StreetlightVision.Pages.UI
{
    public class ReplaceNodePanel : PanelBase
    {
        #region Variables

        #endregion //Variables

        #region IWebElements

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-device-replaceNode'] .back-button")]
        private IWebElement backButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-device-replaceNode'] .title-label")]
        private IWebElement panelTitle;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-device-replaceNode'] [for='unique-address-field']")]
        private IWebElement uniqueAddressLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-device-replaceNode'] [id='unique-address-field']")]
        private IWebElement uniqueAddressInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-device-replaceNode'] .equipment-gl-barcode-button-save")]
        private IWebElement saveButton;

        #endregion //IWebElements

        #region Constructor

        public ReplaceNodePanel(IWebDriver driver, PageBase page)
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
        /// Enter a value for 'UniqueAddress' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterUniqueAddressInput(string value)
        {
            uniqueAddressInput.Enter(value);
        }

        /// <summary>
        /// Click 'Save' button
        /// </summary>
        public void ClickSaveButton()
        {
            saveButton.ClickEx();
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
        /// Get 'UniqueAddress' label text
        /// </summary>
        /// <returns></returns>
        public string GetUniqueAddressText()
        {
            return uniqueAddressLabel.Text;
        }

        /// <summary>
        /// Get 'UniqueAddress' input value
        /// </summary>
        /// <returns></returns>
        public string GetUniqueAddressValue()
        {
            return uniqueAddressInput.Value();
        }

        #endregion //Get methods

        #endregion //Basic methods

        #region Business methods

        public bool IsUniqueAddressReadOnly()
        {
            return uniqueAddressInput.IsReadOnly();
        }

        public bool IsSaveIconDisplayed()
        {
            return saveButton.Displayed;
        }

        #endregion //Business methods        
    }
}
