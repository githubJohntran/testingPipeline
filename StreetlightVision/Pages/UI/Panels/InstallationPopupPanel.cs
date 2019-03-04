using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using StreetlightVision.Extensions;
using StreetlightVision.Utilities;
using System;
using System.Linq;

namespace StreetlightVision.Pages.UI
{
    public class InstallationPopupPanel : PanelBase
    {
        #region Variables

        #endregion //Variables

        #region IWebElements

        [FindsBy(How = How.CssSelector, Using = "[id$='wizard-title']")]
        private IWebElement panelTitle;

        [FindsBy(How = How.CssSelector, Using = "[id$='wizard-button-next']")]
        private IWebElement nextButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='wizard-button-cancel']")]
        private IWebElement cancelButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='wizard-button-previous']")]
        private IWebElement backButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='wizard-button-finish']")]
        private IWebElement finishButton;

        #region Basic Info Form

        [FindsBy(How = How.CssSelector, Using = "[id='install.step1-title']")]
        private IWebElement basicInfoFormCaptionLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-controllerStrId'] .slv-label.editor-label")]
        private IWebElement controllerIdLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-controllerStrId-field']")]
        private IWebElement controllerIdDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-idOnController'] .slv-label.editor-label")]
        private IWebElement identifierLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-idOnController-field']")]
        private IWebElement identifierInput;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-modelFunctionId'] .slv-label.editor-label")]
        private IWebElement typeOfEquipmentLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-modelFunctionId-field']")]
        private IWebElement typeOfEquipmentDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-name'] .slv-label.editor-label")]
        private IWebElement nameLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-name-field']")]
        private IWebElement nameInput;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-address'] .slv-label.editor-label")]
        private IWebElement address1Label;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-address-field']")]
        private IWebElement address1Input;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-location-streetdescription'] .slv-label.editor-label")]
        private IWebElement address2Label;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-location-streetdescription-field']")]
        private IWebElement address2Input;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-location-city'] .slv-label.editor-label")]
        private IWebElement cityLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-location-city-field']")]
        private IWebElement cityInput;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-location-zipcode'] .slv-label.editor-label")]
        private IWebElement zipcodeLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-location-zipcode-field']")]
        private IWebElement zipcodeInput;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-LampIsAccessible'] .slv-label.editor-label")]
        private IWebElement lampIsAccessibleLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-LampIsAccessible-field']")]
        private IWebElement lampIsAccessibleDropDown;

        #endregion //Basic Info Form        

        #region Reason Form

        [FindsBy(How = How.CssSelector, Using = "[id='install.step2-title']")]
        private IWebElement reasonFormCaptionLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-Reason'] .slv-label.editor-label")]
        private IWebElement reasonLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-Reason-field']")]
        private IWebElement reasonDropDown;

        #endregion //Reason Form

        #region Scan Barcode Form

        [FindsBy(How = How.CssSelector, Using = "[id$='wizard-panel-install.step3'] div.slv-barcode-title")]
        private IWebElement scanBarcodeFormCaptionLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='wizard-panel-install.step3'] button.icon-camera.slv-snapshot-button")]
        private IWebElement barcodeCameraButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='wizard-panel-install.step3'] div.slv-barcode-content.slv-barcode-picture")]
        private IWebElement barcodePicture;

        [FindsBy(How = How.CssSelector, Using = "[id$='wizard-panel-install.step3'] [id='editor-property-MacAddress'] .editor-label")]
        private IWebElement uniqueAddressLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='wizard-panel-install.step3'] [id='editor-property-MacAddress-field']")]
        private IWebElement uniqueAddressInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='wizard-panel-install.step3'] [id='editor-property-device-nic-serialnumber'] .editor-label")]
        private IWebElement nicSerialNumberLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='wizard-panel-install.step3'] [id='editor-property-device-nic-serialnumber-field']")]
        private IWebElement nicSerialNumberInput;

        #endregion //Scan Barcode Form

        #region Light Come ON Form

        [FindsBy(How = How.CssSelector, Using = "[id='install.step4-title']")]
        private IWebElement lightComeOnFormCaptionLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-LampCameOnWhenNodeInstalled'] .slv-label.editor-label")]
        private IWebElement lampCameOnLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-LampCameOnWhenNodeInstalled-field']")]
        private IWebElement lampCameOnCheckbox;

        #endregion //Light Come ON Form

        #region More Info Form

        [FindsBy(How = How.CssSelector, Using = "[id='install.step5-title']")]
        private IWebElement moreInfoFormCaptionLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-brandId'] .slv-label.editor-label")]
        private IWebElement lampTypeLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-brandId-field']")]
        private IWebElement lampTypeDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-power'] .slv-label.editor-label")]
        private IWebElement lampWattageLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-power-field']")]
        private IWebElement lampWattageNumericInput;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-luminaire-type'] .slv-label.editor-label")]
        private IWebElement luminaireTypeLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-luminaire-type-field']")]
        private IWebElement luminaireTypeDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-luminaire-model'] .slv-label.editor-label")]
        private IWebElement luminaireModelLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-luminaire-model-field']")]
        private IWebElement luminaireModelDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-luminaire-distributiontype'] .slv-label.editor-label")]
        private IWebElement lightDistributionLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-luminaire-distributiontype-field']")]
        private IWebElement lightDistributionDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-luminaire-orientation'] .slv-label.editor-label")]
        private IWebElement orientationLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-luminaire-orientation-field']")]
        private IWebElement orientationDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-fixing-length'] .slv-label.editor-label")]
        private IWebElement bracketLengthLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-fixing-length-field']")]
        private IWebElement bracketLengthDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-pole-type'] .slv-label.editor-label")]
        private IWebElement poleTypeLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-pole-type-field']")]
        private IWebElement poleTypeDropDown;

        #endregion //More Info Form

        #region Take Photo Form

        [FindsBy(How = How.CssSelector, Using = "[id$='wizard-panel-install.step6'] div.slv-snapshot-title")]
        private IWebElement takePhotoFormCaptionLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='wizard-panel-install.step6'] button.icon-camera.slv-snapshot-button")]
        private IWebElement cameraButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='wizard-panel-preinstall.step6-image']")]
        private IWebElement snapshotImage;

        [FindsBy(How = How.CssSelector, Using = "[id$='wizard-panel-preinstall.step6-input']")]
        private IWebElement snapshotInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='wizard-panel-install.step6'] div.slv-snapshot-message.slv-warning")]
        private IWebElement warningMessageLabel;

        #endregion //Take Photo Form

        #region Run Test Form

        [FindsBy(How = How.CssSelector, Using = "[id$='wizard-panel-install.step7'] div.slv-test-header .slv-test-title")]
        private IWebElement runTestFormCaptionLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='wizard-panel-install.step7'] button.slv-test-run-button")]
        private IWebElement runTestButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='wizard-panel-install.step7'] div.slv-test-infoMessage")]
        private IWebElement infoMessageLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='wizard-panel-install.step7'] div.slv-test-errorMessage")]
        private IWebElement errorMessageLabel;

        #endregion //Run Test Form

        #region Comment Form

        [FindsBy(How = How.CssSelector, Using = "[id='install.step8-title']")]
        private IWebElement commentFormCaptionLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-comment'] .slv-label.editor-label")]
        private IWebElement commentLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-comment-field']")]
        private IWebElement commentInput;

        #endregion //Comment Form

        #region Finish Form

        [FindsBy(How = How.CssSelector, Using = "div.slv-label.slv-wizard-message")]
        private IWebElement finishMessageLabel;

        #endregion //Finish Form

        #endregion //IWebElements

        #region Constructor

        public InstallationPopupPanel(IWebDriver driver, PageBase page)
            : base(driver, page)
        {
            PageFactory.InitElements(this, new RetryingElementLocator(driver, TimeSpan.FromMilliseconds(Settings.LocatorRetryingTimeout), TimeSpan.FromMilliseconds(Settings.LocatorPollingInterval)));
        }

        #endregion  //Constructor

        #region Basic methods

        #region Actions

        /// <summary>
        /// Click 'Next' button
        /// </summary>
        public void ClickNextButton()
        {
            Wait.ForElementDisplayed(nextButton);
            nextButton.ClickEx();
        }

        /// <summary>
        /// Click 'Cancel' button
        /// </summary>
        public void ClickCancelButton()
        {
            cancelButton.ClickEx();
        }

        /// <summary>
        /// Click 'Back' button
        /// </summary>
        public void ClickBackButton()
        {
            backButton.ClickEx();
        }

        /// <summary>
        /// Click 'Finish' button
        /// </summary>
        public void ClickFinishButton()
        {
            finishButton.ClickEx();
        }

        #region Basic Info Form

        /// <summary>
        /// Select an item of 'ControllerId' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectControllerIdDropDown(string value)
        {
            controllerIdDropDown.Select(value);
        }

        /// <summary>
        /// Enter a value for 'Identifier' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterIdentifierInput(string value)
        {
            identifierInput.Enter(value);
        }

        /// <summary>
        /// Select an item of 'TypeOfEquipment' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectTypeOfEquipmentDropDown(string value)
        {
            typeOfEquipmentDropDown.Select(value);
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
        /// Enter a value for 'Address1' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterAddress1Input(string value)
        {
            address1Input.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'Address2' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterAddress2Input(string value)
        {
            address2Input.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'City' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterCityInput(string value)
        {
            cityInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'Zipcode' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterZipcodeInput(string value)
        {
            zipcodeInput.Enter(value);
        }

        /// <summary>
        /// Select an item of 'LampIsAccessible' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectLampIsAccessibleDropDown(string value)
        {
            lampIsAccessibleDropDown.Select(value);
        }

        #endregion //Basic Info Form        

        #region Reason Form

        /// <summary>
        /// Select an item of 'Reason' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectReasonDropDown(string value)
        {
            reasonDropDown.Select(value);
        }

        #endregion //Reason Form

        #region Scan Barcode Form

        /// <summary>
        /// Click 'BarcodeCamera' button
        /// </summary>
        public void ClickBarcodeCameraButton()
        {
            barcodeCameraButton.ClickEx();
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
        /// Enter a value for 'NicSerialNumber' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterNicSerialNumberInput(string value)
        {
            nicSerialNumberInput.Enter(value);
        }

        #endregion //Scan Barcode Form

        #region Light Come ON Form

        /// <summary>
        /// Tick 'LampCameOn' checkbox
        /// </summary>
        /// <param name="value"></param>
        public void TickLampCameOnCheckbox(bool value)
        {
            lampCameOnCheckbox.Check(value);
        }

        #endregion //Light Come ON Form

        #region More Info Form

        /// <summary>
        /// Select an item of 'LampType' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectLampTypeDropDown(string value)
        {
            lampTypeDropDown.Select(value);
        }

        /// <summary>
        /// Enter a value for 'LampWattage' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterLampWattageNumericInput(string value)
        {
            lampWattageNumericInput.Enter(value);
        }

        /// <summary>
        /// Select an item of 'LuminaireType' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectLuminaireTypeDropDown(string value)
        {
            luminaireTypeDropDown.Select(value);
        }

        /// <summary>
        /// Select an item of 'LuminaireModel' dropdown
        /// </summary>
        /// <param name="value"></param>
        public void SelectLuminaireModelDropDown(string value)
        {
            luminaireModelDropDown.Select(value);
        }

        /// <summary>
        /// Select an item of 'LightDistribution' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectLightDistributionDropDown(string value)
        {
            lightDistributionDropDown.Select(value);
        }

        /// <summary>
        /// Select an item of 'Orientation' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectOrientationDropDown(string value)
        {
            orientationDropDown.Select(value);
        }

        /// <summary>
        /// Select an item of 'BracketLength' dropdown
        /// </summary>
        /// <param name="value"></param>
        public void SelectBracketLengthDropDown(string value)
        {
            bracketLengthDropDown.Select(value);
        }

        /// <summary>
        /// Select a value for'PoleType' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectPoleTypeDropDown(string value)
        {
            poleTypeDropDown.Select(value);
        }

        #endregion //More Info Form

        #region Take Photo Form

        /// <summary>
        /// Click 'Camera' button
        /// </summary>
        public void ClickCameraButton()
        {
            cameraButton.ClickEx();
        }

        /// <summary>
        /// Enter a value for 'Snapshot' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterSnapshotInput(string value)
        {
            snapshotInput.Enter(value);
        }

        #endregion //Take Photo Form

        #region Run Test Form

        /// <summary>
        /// Click 'RunTest' button
        /// </summary>
        public void ClickRunTestButton()
        {
            runTestButton.ClickEx();
        }

        #endregion //Run Test Form

        #region Comment Form

        /// <summary>
        /// Enter a value for 'Comment' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterCommentInput(string value)
        {
            commentInput.Enter(value);
        }

        #endregion //Comment Form

        #region Finish Form

        #endregion //Finish Form

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

        #region Basic Info Form

        /// <summary>
        /// Get 'BasicInfoFormCaption' label text
        /// </summary>
        /// <returns></returns>
        public string GetBasicInfoFormCaptionText()
        {
            return basicInfoFormCaptionLabel.Text;
        }

        /// <summary>
        /// Get 'ControllerId' label text
        /// </summary>
        /// <returns></returns>
        public string GetControllerIdText()
        {
            return controllerIdLabel.Text;
        }

        /// <summary>
        /// Get 'ControllerId' input value
        /// </summary>
        /// <returns></returns>
        public string GetControllerIdValue()
        {
            return controllerIdDropDown.Text;
        }

        /// <summary>
        /// Get 'Identifier' label text
        /// </summary>
        /// <returns></returns>
        public string GetIdentifierText()
        {
            return identifierLabel.Text;
        }

        /// <summary>
        /// Get 'Identifier' input value
        /// </summary>
        /// <returns></returns>
        public string GetIdentifierValue()
        {
            return identifierInput.Value();
        }

        /// <summary>
        /// Get 'TypeOfEquipment' label text
        /// </summary>
        /// <returns></returns>
        public string GetTypeOfEquipmentText()
        {
            return typeOfEquipmentLabel.Text;
        }

        /// <summary>
        /// Get 'TypeOfEquipment' input value
        /// </summary>
        /// <returns></returns>
        public string GetTypeOfEquipmentValue()
        {
            return typeOfEquipmentDropDown.Text;
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
        /// Get 'Address1' label text
        /// </summary>
        /// <returns></returns>
        public string GetAddress1Text()
        {
            return address1Label.Text;
        }

        /// <summary>
        /// Get 'Address1' input value
        /// </summary>
        /// <returns></returns>
        public string GetAddress1Value()
        {
            Wait.ForSeconds(2);
            return address1Input.Value();
        }

        /// <summary>
        /// Get 'Address2' label text
        /// </summary>
        /// <returns></returns>
        public string GetAddress2Text()
        {
            return address2Label.Text;
        }

        /// <summary>
        /// Get 'Address2' input value
        /// </summary>
        /// <returns></returns>
        public string GetAddress2Value()
        {
            return address2Input.Value();
        }

        /// <summary>
        /// Get 'City' label text
        /// </summary>
        /// <returns></returns>
        public string GetCityText()
        {
            return cityLabel.Text;
        }

        /// <summary>
        /// Get 'City' input value
        /// </summary>
        /// <returns></returns>
        public string GetCityValue()
        {
            return cityInput.Value();
        }

        /// <summary>
        /// Get 'Zipcode' label text
        /// </summary>
        /// <returns></returns>
        public string GetZipcodeText()
        {
            return zipcodeLabel.Text;
        }

        /// <summary>
        /// Get 'Zipcode' input value
        /// </summary>
        /// <returns></returns>
        public string GetZipcodeValue()
        {
            return zipcodeInput.Value();
        }

        /// <summary>
        /// Get 'LampIsAccessible' label text
        /// </summary>
        /// <returns></returns>
        public string GetLampIsAccessibleText()
        {
            return lampIsAccessibleLabel.Text;
        }

        /// <summary>
        /// Get 'LampIsAccessible' input value
        /// </summary>
        /// <returns></returns>
        public string GetLampIsAccessibleValue()
        {
            return lampIsAccessibleDropDown.Text;
        }

        #endregion //Basic Info Form        

        #region Reason Form

        /// <summary>
        /// Get 'ReasonFormCaption' label text
        /// </summary>
        /// <returns></returns>
        public string GetReasonFormCaptionText()
        {
            return reasonFormCaptionLabel.Text;
        }

        /// <summary>
        /// Get 'Reason' label text
        /// </summary>
        /// <returns></returns>
        public string GetReasonText()
        {
            return reasonLabel.Text;
        }

        /// <summary>
        /// Get 'Reason' input value
        /// </summary>
        /// <returns></returns>
        public string GetReasonValue()
        {
            return reasonDropDown.Text;
        }

        #endregion //Reason Form

        #region Scan Barcode Form

        /// <summary>
        /// Get 'ScanBarcodeFormCaption' label text
        /// </summary>
        /// <returns></returns>
        public string GetScanBarcodeFormCaptionText()
        {
            return scanBarcodeFormCaptionLabel.Text;
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
        /// Get 'NicSerialNumber' label text
        /// </summary>
        /// <returns></returns>
        public string GetNicSerialNumberText()
        {
            return nicSerialNumberLabel.Text;
        }

        /// <summary>
        /// Get 'NicSerialNumber' input value
        /// </summary>
        /// <returns></returns>
        public string GetNicSerialNumberValue()
        {
            return nicSerialNumberInput.Value();
        }

        #endregion //Scan Barcode Form

        #region Light Come ON Form

        /// <summary>
        /// Get 'LightComeOnFormCaption' label text
        /// </summary>
        /// <returns></returns>
        public string GetLightComeOnFormCaptionText()
        {
            return lightComeOnFormCaptionLabel.Text;
        }

        /// <summary>
        /// Get 'LampCameOn' label text
        /// </summary>
        /// <returns></returns>
        public string GetLampCameOnText()
        {
            return lampCameOnLabel.Text;
        }

        /// <summary>
        /// Get 'LampCameOn' input value
        /// </summary>
        /// <returns></returns>
        public bool GetLampCameOnValue()
        {
            return lampCameOnCheckbox.CheckboxValue();
        }

        #endregion //Light Come ON Form

        #region More Info Form

        /// <summary>
        /// Get 'MoreInfoFormCaption' label text
        /// </summary>
        /// <returns></returns>
        public string GetMoreInfoFormCaptionText()
        {
            return moreInfoFormCaptionLabel.Text;
        }

        /// <summary>
        /// Get 'LampType' label text
        /// </summary>
        /// <returns></returns>
        public string GetLampTypeText()
        {
            return lampTypeLabel.Text;
        }

        /// <summary>
        /// Get 'LampType' input value
        /// </summary>
        /// <returns></returns>
        public string GetLampTypeValue()
        {
            return lampTypeDropDown.Text;
        }

        /// <summary>
        /// Get 'LampWattage' label text
        /// </summary>
        /// <returns></returns>
        public string GetLampWattageText()
        {
            return lampWattageLabel.Text;
        }

        /// <summary>
        /// Get 'LampWattage' input value
        /// </summary>
        /// <returns></returns>
        public string GetLampWattageValue()
        {
            return lampWattageNumericInput.Value();
        }

        /// <summary>
        /// Get 'LuminaireType' label text
        /// </summary>
        /// <returns></returns>
        public string GetLuminaireTypeText()
        {
            return luminaireTypeLabel.Text;
        }

        /// <summary>
        /// Get 'LuminaireType' input value
        /// </summary>
        /// <returns></returns>
        public string GetLuminaireTypeValue()
        {
            return luminaireTypeDropDown.Text;
        }

        /// <summary>
        /// Get 'LuminaireModel' label text
        /// </summary>
        /// <returns></returns>
        public string GetLuminaireModelText()
        {
            return luminaireModelLabel.Text;
        }

        /// <summary>
        /// Get 'LuminaireModel' input value
        /// </summary>
        /// <returns></returns>
        public string GetLuminaireModelValue()
        {
            return luminaireModelDropDown.Text;
        }

        /// <summary>
        /// Get 'LightDistribution' label text
        /// </summary>
        /// <returns></returns>
        public string GetLightDistributionText()
        {
            return lightDistributionLabel.Text;
        }

        /// <summary>
        /// Get 'LightDistribution' input value
        /// </summary>
        /// <returns></returns>
        public string GetLightDistributionValue()
        {
            return lightDistributionDropDown.Text;
        }

        /// <summary>
        /// Get 'Orientation' label text
        /// </summary>
        /// <returns></returns>
        public string GetOrientationText()
        {
            return orientationLabel.Text;
        }

        /// <summary>
        /// Get 'Orientation' input value
        /// </summary>
        /// <returns></returns>
        public string GetOrientationValue()
        {
            return orientationDropDown.Text;
        }

        /// <summary>
        /// Get 'BracketLength' label text
        /// </summary>
        /// <returns></returns>
        public string GetBracketLengthText()
        {
            return bracketLengthLabel.Text;
        }

        /// <summary>
        /// Get 'BracketLength' dropdown value
        /// </summary>
        /// <returns></returns>
        public string GetBracketLengthValue()
        {
            return bracketLengthDropDown.Text;
        }

        /// <summary>
        /// Get 'PoleType' label text
        /// </summary>
        /// <returns></returns>
        public string GetPoleTypeText()
        {
            return poleTypeLabel.Text;
        }

        /// <summary>
        /// Get 'PoleType' dropdown value
        /// </summary>
        /// <returns></returns>
        public string GetPoleTypeValue()
        {
            return poleTypeDropDown.Text;
        }

        #endregion //More Info Form

        #region Take Photo Form

        /// <summary>
        /// Get 'TakePhotoFormCaption' label text
        /// </summary>
        /// <returns></returns>
        public string GetTakePhotoFormCaptionText()
        {
            return takePhotoFormCaptionLabel.Text;
        }

        /// <summary>
        /// Get 'SnapshotImage' input value
        /// </summary>
        /// <returns></returns>
        public string GetSnapshotImageValue()
        {
            return snapshotImage.ImageValue();
        }

        /// <summary>
        /// Get 'Snapshot' input value
        /// </summary>
        /// <returns></returns>
        public string GetSnapshotValue()
        {
            return snapshotInput.Value();
        }

        /// <summary>
        /// Get 'WarningMessage' label text
        /// </summary>
        /// <returns></returns>
        public string GetWarningMessageText()
        {
            return warningMessageLabel.Text;
        }

        #endregion //Take Photo Form

        #region Run Test Form

        /// <summary>
        /// Get 'RunTestFormCaption' label text
        /// </summary>
        /// <returns></returns>
        public string GetRunTestFormCaptionText()
        {
            return runTestFormCaptionLabel.Text;
        }

        /// <summary>
        /// Get 'InfoMessage' label text
        /// </summary>
        /// <returns></returns>
        public string GetInfoMessageText()
        {
            return infoMessageLabel.Text;
        }

        /// <summary>
        /// Get 'ErrorMessage' label text
        /// </summary>
        /// <returns></returns>
        public string GetErrorMessageText()
        {
            return errorMessageLabel.Text;
        }

        #endregion //Run Test Form

        #region Comment Form

        /// <summary>
        /// Get 'CommentFormCaption' label text
        /// </summary>
        /// <returns></returns>
        public string GetCommentFormCaptionText()
        {
            return commentFormCaptionLabel.Text;
        }

        /// <summary>
        /// Get 'Comment' label text
        /// </summary>
        /// <returns></returns>
        public string GetCommentText()
        {
            return commentLabel.Text;
        }

        /// <summary>
        /// Get 'Comment' input value
        /// </summary>
        /// <returns></returns>
        public string GetCommentValue()
        {
            return commentInput.Value();
        }

        #endregion //Comment Form

        #region Finish Form

        /// <summary>
        /// Get 'FinishMessage' label text
        /// </summary>
        /// <returns></returns>
        public string GetFinishMessageText()
        {
            return finishMessageLabel.Text;
        }

        #endregion //Finish Form

        #endregion //Get methods

        #endregion //Basic methods

        #region Business methods

        public void WaitForAddress1HasValue()
        {
            Wait.ForElementValue(address1Input);
        }

        public void WaitForBasicInfoFormDisplayed()
        {
            Wait.ForElementStyle(By.CssSelector("[id$='wizard-panels'].slv-wizard"), "left: 0px");
        }

        public void WaitForReasonFormDisplayed()
        {
            Wait.ForElementStyle(By.CssSelector("[id$='wizard-panels'].slv-wizard"), "left: -500px");
        }

        public void WaitForScanQRCodeFormDisplayed()
        {
            Wait.ForElementStyle(By.CssSelector("[id$='wizard-panels'].slv-wizard"), "left: -1000px");
        }

        public void WaitForLightComeOnFormDisplayed()
        {
            Wait.ForElementStyle(By.CssSelector("[id$='wizard-panels'].slv-wizard"), "left: -1500px");
        }

        public void WaitForMoreInfoFormDisplayed()
        {
            Wait.ForElementStyle(By.CssSelector("[id$='wizard-panels'].slv-wizard"), "left: -2000px");
        }

        public void WaitForPhotoFormDisplayed()
        {
            Wait.ForElementStyle(By.CssSelector("[id$='wizard-panels'].slv-wizard"), "left: -2500px");
        }

        public void WaitForRunTestFormDisplayed()
        {
            Wait.ForElementStyle(By.CssSelector("[id$='wizard-panels'].slv-wizard"), "left: -3000px");
        }

        public void WaitForCommentFormDisplayed()
        {
            Wait.ForElementStyle(By.CssSelector("[id$='wizard-panels'].slv-wizard"), "left: -3500px");
        }

        public void WaitForFinishFormDisplayed()
        {
            Wait.ForElementStyle(By.CssSelector("[id$='wizard-panels'].slv-wizard"), "left: -4000px");
        }

        public void WaitForRunTestCompleted()
        {
            Wait.ForElementDisplayed(By.CssSelector("[id$='wizard-panel-install.step7'] button.slv-test-run-button"));
            Wait.ForElementInvisible(By.CssSelector("[id$='wizard-panel-install.step7'] div.slv-test-progressBar"));
            Wait.ForElementText(By.CssSelector("[id$='wizard-panel-install.step7'] .slv-test-toolbar"));
        }

        public string GetRunTestMessage()
        {
            return JSUtility.GetElementText("[id$='wizard-panel-install.step7'] .slv-test-toolbar").TrimEx();
        }

        #region Get Readonly

        public bool IsControllerIdEditable()
        {
            return !controllerIdDropDown.IsReadOnly();
        }

        public bool IsIdentifierEditable()
        {
            return !identifierInput.IsReadOnly();
        }

        public bool IsNameEditable()
        {
            return !nameInput.IsReadOnly();
        }

        public bool IsAddress1Editable()
        {
            return !address1Input.IsReadOnly();
        }

        public bool IsAddress2Editable()
        {
            return !address2Input.IsReadOnly();
        }

        public bool IsCityEditable()
        {
            return !cityInput.IsReadOnly();
        }

        public bool IsZipcodeEditable()
        {
            return !zipcodeInput.IsReadOnly();
        }

        public bool IsLampIsAccessibleEditable()
        {
            return !lampIsAccessibleDropDown.IsReadOnly();
        }

        #endregion //Get Readonly

        public bool IsBackButtonVisible()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='wizard-button-cancel']"));
        }

        public bool IsFinishButtonVisible()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='wizard-button-finish']"));
        }

        public bool IsNextButtonVisible()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='wizard-button-next']"));
        }

        public bool IsCancelButtonVisible()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='wizard-button-cancel']"));
        }

        public bool IsRunTestButtonVisible()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='wizard-panel-install.step7'] button.slv-test-run-button"));
        }
        
        public bool IsRunTestErrorMessageDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='wizard-panel-install.step7'] div.slv-test-errorMessage"));
        }

        public bool IsCameraButtonVisible()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='wizard-panel-install.step6'] button.icon-camera.slv-snapshot-button"));
        }

        public bool IsSnapshootErrorMessageDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='wizard-panel-install.step6'] div.slv-warning.slv-snapshot-message"));
        }

        #endregion //Business methods
    }
}
