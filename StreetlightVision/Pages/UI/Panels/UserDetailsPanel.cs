using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using StreetlightVision.Extensions;
using System;

namespace StreetlightVision.Pages.UI
{
    public class UserDetailsPanel : PanelBase
    {
        #region Variables

        #endregion //Variables

        #region IWebElements

        [FindsBy(How = How.CssSelector, Using = "[id$='userEditor-buttons'] [id='tb_userButtons_item_cancel'] > table")]
        private IWebElement cancelButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='userEditor-buttons'] [id='tb_userButtons_item_save'] > table")]
        private IWebElement saveButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='userEditor-buttons'] [id='tb_userButtons_item_delete'] > table")]
        private IWebElement deleteButton;        

        [FindsBy(How = How.CssSelector, Using = "[id$='userEditor'] .user-editor-picture")]
        private IWebElement userImage;

        [FindsBy(How = How.CssSelector, Using = "[id$='userEditor-content-lastname-label']")]
        private IWebElement lastNameLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='userEditor-content-lastname-field']")]
        private IWebElement lastNameInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='userEditor-content-firstname-label']")]
        private IWebElement firstNameLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='userEditor-content-firstname-field']")]
        private IWebElement firstNameInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='userEditor-content-login-label']")]
        private IWebElement loginLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='userEditor-content-login-value']")]
        private IWebElement loginValueLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='userEditor-content-login-field']")]
        private IWebElement loginInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='userEditor-content-password-new-label']")]
        private IWebElement passwordLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='userEditor-content-password-new-field']")]
        private IWebElement passwordInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='userEditor-content-password-confirm-label']")]
        private IWebElement confirmPasswordLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='userEditor-content-password-confirm-field']")]
        private IWebElement confirmPasswordInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='userEditor-content-phone-label']")]
        private IWebElement phoneLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='userEditor-content-phone-field']")]
        private IWebElement phoneInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='userEditor-content-mobile-label']")]
        private IWebElement mobileLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='userEditor-content-mobile-field']")]
        private IWebElement mobileInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='userEditor-content-email-label']")]
        private IWebElement emailLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='userEditor-content-email-field']")]
        private IWebElement emailInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='userEditor-content-address-label']")]
        private IWebElement addressLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='userEditor-content-address-field']")]
        private IWebElement addressInput;

        #endregion //IWebElements

        #region Constructor

        public UserDetailsPanel(IWebDriver driver, PageBase page)
            : base(driver, page)
        {
            PageFactory.InitElements(this, new RetryingElementLocator(driver, TimeSpan.FromMilliseconds(Settings.LocatorRetryingTimeout), TimeSpan.FromMilliseconds(Settings.LocatorPollingInterval)));
        }

        #endregion //Constructor

        #region Basic methods  

        #region Actions

        /// <summary>
        /// Click 'Cancel' button
        /// </summary>
        public void ClickCancelButton()
        {
            cancelButton.ClickEx();
        }

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
        /// Focus 'LastName' input
        /// </summary>
        public void FocusLastNameInput()
        {
            lastNameInput.ClickEx();
        }

        /// <summary>
        /// Enter a value for 'LastName' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterLastNameInput(string value)
        {
            lastNameInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'FirstName' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterFirstNameInput(string value)
        {
            firstNameInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'Login' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterLoginInput(string value)
        {
            loginInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'Password' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterPasswordInput(string value)
        {
            passwordInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'ConfirmPassword' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterConfirmPasswordInput(string value)
        {
            confirmPasswordInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'Phone' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterPhoneInput(string value)
        {
            phoneInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'Mobile' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterMobileInput(string value)
        {
            mobileInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'Email' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterEmailInput(string value)
        {
            emailInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'Address' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterAddressInput(string value)
        {
            addressInput.Enter(value);
        }        

        #endregion //Actions

        #region Get methods

        /// <summary>
        /// Get 'UserImage' input value
        /// </summary>
        /// <returns></returns>
        public string GetUserImageValue()
        {
            return userImage.ImageValue();
        }

        /// <summary>
        /// Get 'LastName' label text
        /// </summary>
        /// <returns></returns>
        public string GetLastNameText()
        {
            return lastNameLabel.Text;
        }

        /// <summary>
        /// Get 'LastName' input value
        /// </summary>
        /// <returns></returns>
        public string GetLastNameValue()
        {
            return lastNameInput.Value();
        }

        /// <summary>
        /// Get 'FirstName' label text
        /// </summary>
        /// <returns></returns>
        public string GetFirstNameText()
        {
            return firstNameLabel.Text;
        }

        /// <summary>
        /// Get 'FirstName' input value
        /// </summary>
        /// <returns></returns>
        public string GetFirstNameValue()
        {
            return firstNameInput.Value();
        }

        /// <summary>
        /// Get 'Login' label text
        /// </summary>
        /// <returns></returns>
        public string GetLoginText()
        {
            return loginLabel.Text;
        }

        /// <summary>
        /// Get 'LoginValue' label text
        /// </summary>
        /// <returns></returns>
        public string GetLoginValueText()
        {
            return loginValueLabel.Text;
        }

        /// <summary>
        /// Get 'Login' input value
        /// </summary>
        /// <returns></returns>
        public string GetLoginValue()
        {
            return loginInput.Value();
        }

        /// <summary>
        /// Get 'Password' label text
        /// </summary>
        /// <returns></returns>
        public string GetPasswordText()
        {
            return passwordLabel.Text;
        }

        /// <summary>
        /// Get 'Password' input value
        /// </summary>
        /// <returns></returns>
        public string GetPasswordValue()
        {
            return passwordInput.Value();
        }

        /// <summary>
        /// Get 'ConfirmPassword' label text
        /// </summary>
        /// <returns></returns>
        public string GetConfirmPasswordText()
        {
            return confirmPasswordLabel.Text;
        }

        /// <summary>
        /// Get 'ConfirmPassword' input value
        /// </summary>
        /// <returns></returns>
        public string GetConfirmPasswordValue()
        {
            return confirmPasswordInput.Value();
        }

        /// <summary>
        /// Get 'Phone' label text
        /// </summary>
        /// <returns></returns>
        public string GetPhoneText()
        {
            return phoneLabel.Text;
        }

        /// <summary>
        /// Get 'Phone' input value
        /// </summary>
        /// <returns></returns>
        public string GetPhoneValue()
        {
            return phoneInput.Value();
        }

        /// <summary>
        /// Get 'Mobile' label text
        /// </summary>
        /// <returns></returns>
        public string GetMobileText()
        {
            return mobileLabel.Text;
        }

        /// <summary>
        /// Get 'Mobile' input value
        /// </summary>
        /// <returns></returns>
        public string GetMobileValue()
        {
            return mobileInput.Value();
        }

        /// <summary>
        /// Get 'Email' label text
        /// </summary>
        /// <returns></returns>
        public string GetEmailText()
        {
            return emailLabel.Text;
        }

        /// <summary>
        /// Get 'Email' input value
        /// </summary>
        /// <returns></returns>
        public string GetEmailValue()
        {
            return emailInput.Value();
        }

        /// <summary>
        /// Get 'Address' label text
        /// </summary>
        /// <returns></returns>
        public string GetAddressText()
        {
            return addressLabel.Text;
        }

        /// <summary>
        /// Get 'Address' input value
        /// </summary>
        /// <returns></returns>
        public string GetAddressValue()
        {
            return addressInput.Value();
        }        

        #endregion //Get methods

        #endregion //Basic methods

        #region Business methods     
        
        public bool IsLastNameInputReadOnly()
        {
            return lastNameInput.IsReadOnly();
        }

        public bool IsFirstNameInputReadOnly()
        {
            return firstNameInput.IsReadOnly();
        }
        
        public bool IsPhoneInputReadOnly()
        {
            return phoneInput.IsReadOnly();
        }

        public bool IsMobileInputReadOnly()
        {
            return mobileInput.IsReadOnly();
        }

        public bool IsEmailInputReadOnly()
        {
            return emailInput.IsReadOnly();
        }

        public bool IsAddressInputReadOnly()
        {
            return addressInput.IsReadOnly();
        }

        #endregion //Business methods

        public override void WaitForPanelLoaded()
        {
            Wait.ForElementText(lastNameLabel);
        }

        public override void WaitForPreviousActionComplete()
        {
            base.WaitForPreviousActionComplete();
        }
    }
}