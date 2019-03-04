using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using System;
using StreetlightVision.Extensions;
using System.Linq;
using StreetlightVision.Utilities;

namespace StreetlightVision.Pages.UI
{
    public class VerificationPopupPanel: PanelBase
    {
        #region Variables        

        #endregion //Variables

        #region IWebElements

        [FindsBy(How = How.CssSelector, Using = "[id$='wizard-title']")]
        private IWebElement panelTitle;

        [FindsBy(How = How.CssSelector, Using = "[id$='wizard-button-next']")]
        private IWebElement nextButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='wizard-button-previous']")]
        private IWebElement backButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='wizard-button-cancel']")]
        private IWebElement cancelButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='wizard-button-finish']")]
        private IWebElement finishButton;

        #region Basic Info Form

        [FindsBy(How = How.CssSelector, Using = "[id='preinstall.step1-title']")]
        private IWebElement basicInfoFormCaptionLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-controllerStrId'] .slv-label.editor-label")]
        private IWebElement controllerIdLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-controllerStrId-field']")]
        private IWebElement controllerIdDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-idOnController'] .slv-label.editor-label")]
        private IWebElement identifierLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-idOnController-field']")]
        private IWebElement identifierInput;

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

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-LampIsAtRightPosition'] .slv-label.editor-label")]
        private IWebElement isLampPositionCorrectLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-LampIsAtRightPosition-field']")]
        private IWebElement isLampPositionCorrectDropDown;

        #endregion //Basic Info Form

        #region More Info Form

        [FindsBy(How = How.CssSelector, Using = "[id='preinstall.step2-title']")]
        private IWebElement moreInfoFormCaptionLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-LampIsFunctionning'] .slv-label.editor-label")]
        private IWebElement isLampFunctionningLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-LampIsFunctionning-field']")]
        private IWebElement isLampFunctionningCheckbox;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-LampIsAcorn'] .slv-label.editor-label")]
        private IWebElement isLampAcornLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-LampIsAcorn-field']")]
        private IWebElement isLampAcornCheckbox;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-comment'] .slv-label.editor-label")]
        private IWebElement commentLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-comment-field']")]
        private IWebElement commentInput;

        #endregion //More Info Form

        #region Reason Form

        [FindsBy(How = How.CssSelector, Using = "[id='preinstall.step3-title']")]
        private IWebElement reasonFormCaptionLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-Reason'] .slv-label.editor-label")]
        private IWebElement reasonLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-Reason-field']")]
        private IWebElement reasonDropDown;

        #endregion //Reason Form

        #region Take Photo Form
        
        [FindsBy(How = How.CssSelector, Using = "[id$='wizard-panel-preinstall.step4'] div.slv-snapshot-title")]
        private IWebElement takePhotoFormCaptionLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='wizard-panel-preinstall.step4'] button.icon-camera.slv-snapshot-button")]
        private IWebElement cameraButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='wizard-panel-preinstall.step4-image']")]
        private IWebElement snapshotImage;

        [FindsBy(How = How.CssSelector, Using = "[id$='wizard-panel-preinstall.step4-input']")]
        private IWebElement snapshotInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='wizard-panel-preinstall.step4'] div.slv-snapshot-message.slv-warning")]
        private IWebElement warningMessageLabel;

        #endregion //Take Photo Form

        #region Run Test Form
        
        [FindsBy(How = How.CssSelector, Using = "[id$='wizard-panel-preinstall.step5'] div.slv-test-header .slv-test-title")]
        private IWebElement runTestFormCaptionLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='wizard-panel-preinstall.step5'] button.slv-test-run-button")]
        private IWebElement runTestButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='wizard-panel-preinstall.step5'] div.slv-test-errorMessage")]
        private IWebElement errorMessageLabel;

        #endregion //Run Test Form

        #region Finish Form

        [FindsBy(How = How.CssSelector, Using = "div.slv-label.slv-wizard-message")]
        private IWebElement finishMessageLabel;        

        #endregion //Finish Form

        #endregion //IWebElements

        #region Constructor

        public VerificationPopupPanel(IWebDriver driver, PageBase page)
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
            nextButton.ClickEx();
        }        

        /// <summary>
        /// Click 'Back' button
        /// </summary>
        public void ClickBackButton()
        {
            backButton.ClickEx();
        }

        /// <summary>
        /// Click 'Cancel' button
        /// </summary>
        public void ClickCancelButton()
        {
            cancelButton.ClickEx();
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
        /// Select an item of 'IsLampPositionCorrect' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectIsLampPositionCorrectDropDown(string value)
        {
            isLampPositionCorrectDropDown.Select(value);
        }

        #endregion //Basic Info Form

        #region More Info Form

        /// <summary>
        /// Tick 'IsLampFunctionning' checkbox
        /// </summary>
        /// <param name="value"></param>
        public void TickIsLampFunctionningCheckbox(bool value)
        {
            isLampFunctionningCheckbox.Check(value);
        }

        /// <summary>
        /// Tick 'IsLampAcorn' checkbox
        /// </summary>
        /// <param name="value"></param>
        public void TickIsLampAcornCheckbox(bool value)
        {
            isLampAcornCheckbox.Check(value);
        }

        /// <summary>
        /// Enter a value for 'Comment' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterCommentInput(string value)
        {
            commentInput.Enter(value);
        }

        #endregion //More Info Form

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
        /// Get 'IsLampPositionCorrect' label text
        /// </summary>
        /// <returns></returns>
        public string GetIsLampPositionCorrectText()
        {
            return isLampPositionCorrectLabel.Text;
        }

        /// <summary>
        /// Get 'IsLampPositionCorrect' input value
        /// </summary>
        /// <returns></returns>
        public string GetIsLampPositionCorrectValue()
        {
            return isLampPositionCorrectDropDown.Text;
        }

        #endregion //Basic Info Form

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
        /// Get 'IsLampFunctionning' label text
        /// </summary>
        /// <returns></returns>
        public string GetIsLampFunctionningText()
        {
            return isLampFunctionningLabel.Text;
        }

        /// <summary>
        /// Get 'IsLampFunctionning' input value
        /// </summary>
        /// <returns></returns>
        public bool GetIsLampFunctionningValue()
        {
            return isLampFunctionningCheckbox.Selected;
        }

        /// <summary>
        /// Get 'IsLampAcorn' label text
        /// </summary>
        /// <returns></returns>
        public string GetIsLampAcornText()
        {
            return isLampAcornLabel.Text;
        }

        /// <summary>
        /// Get 'IsLampAcorn' input value
        /// </summary>
        /// <returns></returns>
        public bool GetIsLampAcornValue()
        {
            return isLampAcornCheckbox.Selected;
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

        #endregion //More Info Form

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
        /// Get 'ErrorMessage' label text
        /// </summary>
        /// <returns></returns>
        public string GetErrorMessageText()
        {
            return errorMessageLabel.Text;
        }

        #endregion //Run Test Form

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

        public void WaitForBasicInfoFormDisplayed()
        {
            Wait.ForElementStyle(By.CssSelector("[id$='wizard-panels'].slv-wizard"), "left: 0px");
        }

        public void WaitForMoreInfoFormDisplayed()
        {
            Wait.ForElementStyle(By.CssSelector("[id$='wizard-panels'].slv-wizard"), "left: -500px");
        }

        public void WaitForReasonFormDisplayed()
        {
            Wait.ForElementStyle(By.CssSelector("[id$='wizard-panels'].slv-wizard"), "left: -1000px");
        }

        public void WaitForPhotoFormDisplayed()
        {
            Wait.ForElementStyle(By.CssSelector("[id$='wizard-panels'].slv-wizard"), "left: -1500px");
        }

        public void WaitForRunTestFormDisplayed()
        {
            Wait.ForElementStyle(By.CssSelector("[id$='wizard-panels'].slv-wizard"), "left: -2000px");
        }

        public void WaitForFinishFormDisplayed()
        {
            Wait.ForElementStyle(By.CssSelector("[id$='wizard-panels'].slv-wizard"), "left: -2500px");
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

        public bool IsLampPositionCorrectEditable()
        {
            return !isLampPositionCorrectDropDown.IsReadOnly();
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

        #endregion //Business methods
    }
}
