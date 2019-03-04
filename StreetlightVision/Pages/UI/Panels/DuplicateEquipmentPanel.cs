using ImageMagick;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using StreetlightVision.Extensions;
using StreetlightVision.Models;
using StreetlightVision.Utilities;
using System;
using System.Collections.Generic;

namespace StreetlightVision.Pages.UI
{
    public class DuplicateEquipmentPanel : PanelBase
    {
        #region Variables

        #endregion //Variables

        #region IWebElements

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-device-duplicate-backButton']")]
        private IWebElement backButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-device-duplicate-title'] .equipment-gl-editor-title-label")]
        private IWebElement panelTitle;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-device-duplicate-buttons-toolbar_item_save'] > table")]
        private IWebElement saveButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-device-duplicate-category-icon']")]
        private IWebElement deviceIcon;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-device-duplicate-category-label']")]
        private IWebElement deviceCaptionLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-device-duplicate-name-label'] .equipment-gl-editor-label")]
        private IWebElement nameLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-device-duplicate-properties-name-field']")]
        private IWebElement nameInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-device-duplicate-content-count-field']")]
        private IWebElement countNumericInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-device-duplicate-content-barcode-field']")]
        private IWebElement barcodeInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-device-duplicate-content-barcode-button']")]
        private IWebElement barcodeOkButton;

        #endregion //IWebElements

        #region Constructor

        public DuplicateEquipmentPanel(IWebDriver driver, PageBase page)
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
        /// Click 'Save' button
        /// </summary>
        public void ClickSaveButton()
        {
            saveButton.ClickEx();
        }

        /// <summary>
        /// Enter a value for 'Name' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterNameInput(string value)
        {
            nameInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'Count' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterCountNumericInput(string value)
        {
            countNumericInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'Barcode' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterBarcodeInput(string value)
        {
            barcodeInput.Enter(value);
        }

        /// <summary>
        /// Click 'BarcodeOk' button
        /// </summary>
        public void ClickBarcodeOkButton()
        {
            barcodeOkButton.ClickEx();
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
        /// Get 'DeviceIcon' input value
        /// </summary>
        /// <returns></returns>
        public string GetDeviceIconValue()
        {
            return deviceIcon.IconValue();
        }

        /// <summary>
        /// Get 'DeviceCaption' label text
        /// </summary>
        /// <returns></returns>
        public string GetDeviceCaptionText()
        {
            return deviceCaptionLabel.Text;
        }

        /// <summary>
        /// Get 'Name' label text
        /// </summary>
        /// <returns></returns>
        public string GetNameText()
        {
            return nameLabel.Text;
        }

        /// <summary>
        /// Get 'Name' input value
        /// </summary>
        /// <returns></returns>
        public string GetNameValue()
        {
            return nameInput.Value();
        }

        /// <summary>
        /// Get 'Count' input value
        /// </summary>
        /// <returns></returns>
        public string GetCountValue()
        {
            return countNumericInput.Value();
        }

        /// <summary>
        /// Get 'Barcode' input value
        /// </summary>
        /// <returns></returns>
        public string GetBarcodeValue()
        {
            return barcodeInput.Value();
        }

        #endregion //Get methods

        #endregion //Basic methods

        #region Business methods

        public byte[] GetDeviceIconBytes()
        {
            var backgroundImg = deviceIcon.GetStyleAttr("background-image");
            var url = backgroundImg.Replace("url(\"", string.Empty).Replace("\")", string.Empty);

            return SLVHelper.DownloadFileData(url);
        }

        public bool CheckIfDeviceIcon(DeviceType device)
        {
            var expectedIcon = new MagickImage(device.GetIconBytes());
            var actualIcon = new MagickImage(GetDeviceIconBytes());
            var result = expectedIcon.Compare(actualIcon, ErrorMetric.Absolute);

            return result == 0;
        }

        public bool IsCounterNumericUpDownInput()
        {
            var isInputDisplayed = ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-device-duplicate-content-count-field']"));
            var isDownDisplayed = ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-device-duplicate-name-label'] .arrow-down"));
            var isUpDisplayed = ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-device-duplicate-name-label'] .arrow-up"));

            return isInputDisplayed && isDownDisplayed && isUpDisplayed;
        }

        public bool IsNameInputReadOnly()
        {
            return nameInput.IsReadOnly();
        }

        public bool IsCounterInputReadOnly()
        {
            return countNumericInput.IsReadOnly(); 
        }

        public bool IsBarcodeSectionDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-device-duplicate-content-barcode'].equipment-gl-barcode"));
        }

        public bool IsBarcodeInputDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-device-duplicate-content-barcode-field']"));
        }

        public bool IsBarcodeOkButtonDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-device-duplicate-content-barcode-button']"));
        }

        public List<string> GetListOfDuplicatedDevicesName()
        {
            return JSUtility.GetElementsText("[id$='ditor-device-duplicate-content-list'] .equipment-gl-list-item .equipment-gl-list-item-title");
        }

        #endregion //Business methods

        public override void WaitForPanelLoaded()
        {
            Wait.ForElementStyle(By.CssSelector("[id$='equipmentgl-editor-device-duplicate']"), "left: 0px");
        }
    }
}
