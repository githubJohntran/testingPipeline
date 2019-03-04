using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using StreetlightVision.Extensions;
using System;

namespace StreetlightVision.Pages.UI
{
    public class MacAddressPanel : PanelBase
    {
        #region Variables

        #endregion //Variables

        #region IWebElements

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-device-macaddress-backButton']")]
        private IWebElement backButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-device-macaddress-title'] .equipmentgl-editor-title-label")]
        private IWebElement panelTitle;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-device-macaddress-barcode'] > div.slv-barcode-header > div")]
        private IWebElement scanQrCodeLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-device-macaddress-barcode'] > div.slv-barcode-value > div > div")]
        private IWebElement uniqueAddressLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-device-macaddress-barcode-barcodeResult-0']")]
        private IWebElement uniqueAddressInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-device-macaddress-barcode'] > div.slv-barcode-header > button")]
        private IWebElement cameraButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-device-macaddress-content'] > button")]
        private IWebElement saveButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-device-macaddress-barcode'] div.slv-barcode-content.slv-barcode-picture")]
        private IWebElement barcodeImage;

        #endregion //IWebElements

        #region Constructor

        public MacAddressPanel(IWebDriver driver, PageBase page)
            : base(driver, page)
        {
            PageFactory.InitElements(this, new RetryingElementLocator(driver, TimeSpan.FromMilliseconds(Settings.LocatorRetryingTimeout), TimeSpan.FromMilliseconds(Settings.LocatorPollingInterval)));
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
        /// Click 'Camera' button
        /// </summary>
        public void ClickCameraButton()
        {
            cameraButton.ClickEx();
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
        /// Get 'ScanQrCode' label text
        /// </summary>
        /// <returns></returns>
        public string GetScanQrCodeText()
        {
            return scanQrCodeLabel.Text;
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

        /// <summary>
        /// Get 'BarcodeImage' input value
        /// </summary>
        /// <returns></returns>
        public string GetBarcodeImageValue()
        {
            return barcodeImage.ImageValue();
        }

        #endregion //Get methods

        #endregion //Basic methods

        #region Business methods

        #endregion //Business methods
    }
}
