using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using StreetlightVision.Extensions;
using StreetlightVision.Utilities;
using System;
using System.Threading;

namespace StreetlightVision.Pages
{
    public class LoginPage : PageBase
    {
        #region Variables

        #endregion //Variables

        #region IWebElements

        [FindsBy(How = How.CssSelector, Using = "div.header")]
        private IWebElement loginTitle;

        [FindsBy(How = How.CssSelector, Using = "[id='slv_login_username'")]
        private IWebElement usernameInput;

        [FindsBy(How = How.CssSelector, Using = "[id='slv_login_password']")]
        private IWebElement passwordInput;

        [FindsBy(How = How.CssSelector, Using = "[id='slv_login_submit']")]
        private IWebElement loginButton;

        [FindsBy(How = How.CssSelector, Using = "[id='slv_preloader_canvas']")]
        private IWebElement authenticationMessageSection;

        [FindsBy(How = How.CssSelector, Using = "[id='slv_login_forgot'] a")]
        private IWebElement forgotPasswordLink;

        [FindsBy(How = How.CssSelector, Using = "[id='slv_login_forgot_name']")]
        private IWebElement forgotUsernameInput;

        [FindsBy(How = How.CssSelector, Using = "[id='slv_login_forgot_submit']")]
        private IWebElement forgotPasswordButton;

        [FindsBy(How = How.CssSelector, Using = "[id='slv_login_forgot_msg']")]
        private IWebElement forgotErrorMessageText;

        [FindsBy(How = How.CssSelector, Using = "[id='slv_login_forgot']")]
        private IWebElement forgotMainForm;

        [FindsBy(How = How.CssSelector, Using = "[id='slv_login_forgot_form']")]
        private IWebElement forgotForm;

        #endregion //IWebElements

        #region Constructor

        public LoginPage(IWebDriver driver)
            : base(driver)
        {
            PageFactory.InitElements(this, new RetryingElementLocator(driver, TimeSpan.FromMilliseconds(Settings.LocatorRetryingTimeout), TimeSpan.FromMilliseconds(Settings.LocatorPollingInterval)));

            WaitForPageReady();
            HideCookieMessage();
        }

        #endregion //Constructor

        #region Basic methods

        #region Actions

        /// <summary>
        /// Enter a value for 'Username' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterUsernameInput(string value)
        {
            usernameInput.Enter(value);
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
        /// Click 'Login' button
        /// </summary>
        public void ClickLoginButton()
        {
            loginButton.ClickEx();
        }

        /// <summary>
        /// Enter a value for 'ForgotUsername' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterForgotUsernameInput(string value)
        {
            forgotUsernameInput.Enter(value);
        }

        /// <summary>
        /// Click 'ForgotPassword' link
        /// </summary>
        public void ClickForgotPasswordLink()
        {
            forgotPasswordLink.ClickEx();
        }

        /// <summary>
        /// Click 'ForgotPassword' button
        /// </summary>
        public void ClickForgotPasswordButton()
        {
            forgotPasswordButton.ClickEx();
        }

        #endregion //Actions

        #region Get methods

        /// <summary>
        /// Get 'LoginTitle' text
        /// </summary>
        /// <returns></returns>
        public string GetLoginTitleText()
        {
            return loginTitle.Text;
        }

        /// <summary>
        /// Get 'Username' input value
        /// </summary>
        /// <returns></returns>
        public string GetUsernameValue()
        {
            return usernameInput.Value();
        }

        /// <summary>
        /// Get 'Username' input placeholder
        /// </summary>
        /// <returns></returns>
        public string GetUsernamePlaceholder()
        {
            return usernameInput.GetAttribute("placeholder");
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
        /// Get 'Password' input placeholder
        /// </summary>
        /// <returns></returns>
        public string GetPasswordPlaceholder()
        {
            return passwordInput.GetAttribute("placeholder");
        }

        /// <summary>
        /// Get 'ForgotUsername' input value
        /// </summary>
        /// <returns></returns>
        public string GetForgotUsernameValue()
        {
            return forgotUsernameInput.Value();
        }

        /// <summary>
        /// Get 'ForgotUsername' input placeholder
        /// </summary>
        /// <returns></returns>
        public string GetForgotUsernamePlaceholder()
        {
            return forgotUsernameInput.GetAttribute("placeholder");
        }

        /// <summary>
        /// Get 'ForgotPasswordLink' text
        /// </summary>
        /// <returns></returns>
        public string GetForgotPasswordLinkText()
        {
            return forgotPasswordLink.Text;
        }

        /// <summary>
        /// Get 'ForgotMessage' text
        /// </summary>
        /// <returns></returns>
        public string GetForgotMessageText()
        {
            return forgotErrorMessageText.Text;
        }


        #endregion //Get methods

        #endregion //Basic methods

        #region Business methods

        #region Wait method

        public void WaitForLoginForgotFormDisplayed()
        {
            Wait.ForElementStyle(forgotForm, "opacity: 1");
        }

        public void WaitForLoginForgotFormDisappeared()
        {
            Wait.ForElementStyle(forgotForm, "opacity: 0");
        }

        public void WaitForForgotMessageDisplayed()
        {
            Wait.ForElementDisplayed(By.CssSelector("[id='slv_login_forgot_msg']"));
        }

        public void WaitForForgotMessageDisappeared()
        {
            Wait.ForElementInvisible(By.CssSelector("[id='slv_login_forgot_msg']"));
        }

        public void WaitForForgotPasswordButtonEnabled()
        {
            Wait.ForSeconds(1);
            Wait.ForElementStyle(forgotPasswordButton, "button_normal.png");
        }

        #endregion //Wait method

        public DesktopPage LoginAsValidUser(string username, string password)
        {
            EnterUsernameInput(username);

            EnterPasswordInput(password);

            // Wait for login button is installed for click action
            WaitForLoginButtonEnabled();

            ClickLoginButton();

            return new DesktopPage(WebDriverContext.CurrentDriver);
        }

        public LoginPage LoginAsInvalidUser(string username, string password)
        {
            EnterUsernameInput(username);

            EnterPasswordInput(password);

            // Wait for login button is installed for click action
            WaitForLoginButtonEnabled();
            ClickLoginButton();
            Wait.ForSeconds(2);

            return this;
        }

        public LoginPage LoginAsInvalidUserWithEmptyUsername(string password)
        {
            EnterUsernameInput(string.Empty);
            EnterPasswordInput(password);
            ClickLoginButton();
            Wait.ForSeconds(2);

            return this;
        }

        public LoginPage LoginAsInvalidUserWithEmptyPassword(string username)
        {
            EnterUsernameInput(username);
            EnterPasswordInput(string.Empty);
            ClickLoginButton();
            Wait.ForSeconds(2);

            return this;
        }

        public BackOfficePage LoginAsValidUserToBackOffice(string username, string password)
        {
            EnterUsernameInput(username);

            EnterPasswordInput(password);

            // Wait for login button is installed for click action
            WaitForLoginButtonEnabled();
            ClickLoginButton();

            return new BackOfficePage(WebDriverContext.CurrentDriver);
        }

        public bool IsForgotMessageDisplayed()
        {
            return forgotErrorMessageText.Displayed;
        }

        public ResetPasswordPage NavigateToResetPasswordLink(string url)
        {
            Driver.Navigate().GoToUrl(url);

            return new ResetPasswordPage(this.Driver);
        }

        #endregion //Business methods

        #region Private methods

        /// <summary>
        /// Wait for loggin button is enabled after user name and password are both entered
        /// </summary>
        private void WaitForLoginButtonEnabled()
        {
            Wait.ForElementStyle(loginButton, "button_normal.png");
            Wait.ForSeconds(1);
        }

        #endregion //Private methods

        protected override void WaitForPageReady()
        {
            Wait.ForElementStyle(forgotMainForm, "opacity: 1");            
            Wait.ForElementText(forgotPasswordLink);
        }
    }
}
