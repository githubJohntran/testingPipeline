using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using StreetlightVision.Extensions;
using System;

namespace StreetlightVision.Pages.UI
{
    public class ChangeMyPasswordPanel : PanelBase
    {
        #region Variables

        #endregion //Variables

        #region IWebElements

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-desktop-settings-backButton']")]
        private IWebElement backButton;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-desktop-settings-title'] .side-panel-title-label")]
        private IWebElement panelHeaderLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='desktop-settings-panel-password'] input:nth-of-type(1)")]
        private IWebElement currentPasswordInput;        

        [FindsBy(How = How.CssSelector, Using = "[id='desktop-settings-panel-password'] input:nth-of-type(2)")]
        private IWebElement newPasswordInput;

        [FindsBy(How = How.CssSelector, Using = "[id='desktop-settings-panel-password'] input:nth-of-type(3)")]
        private IWebElement newPasswordConfirmedInput;

        [FindsBy(How = How.CssSelector, Using = "[id='desktop-settings-panel-password'] .slv-warning")]
        private IWebElement errorMessageLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='desktop-settings-panel-password'] button")]
        private IWebElement changePasswordButton;

        #endregion //IWebElements

        #region Constructor

        public ChangeMyPasswordPanel(IWebDriver driver, PageBase page)
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
        /// Enter a value for 'CurrentPassword' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterCurrentPasswordInput(string value)
        {
            currentPasswordInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'NewPassword' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterNewPasswordInput(string value)
        {
            newPasswordInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'NewPasswordConfirmed' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterNewPasswordConfirmedInput(string value)
        {
            newPasswordConfirmedInput.Enter(value);
        }

        /// <summary>
        /// Click 'ChangePassword' button
        /// </summary>
        public void ClickChangePasswordButton()
        {
            changePasswordButton.ClickEx();
        }

        #endregion //Actions

        #region Get methods

        /// <summary>
        /// Get 'PanelHeader' label text
        /// </summary>
        /// <returns></returns>
        public string GetPanelHeaderText()
        {
            return panelHeaderLabel.Text;
        }

        /// <summary>
        /// Get 'CurrentPassword' input value
        /// </summary>
        /// <returns></returns>
        public string GetCurrentPasswordValue()
        {
            return currentPasswordInput.Value();
        }

        /// <summary>
        /// Get 'NewPassword' input value
        /// </summary>
        /// <returns></returns>
        public string GetNewPasswordValue()
        {
            return newPasswordInput.Value();
        }

        /// <summary>
        /// Get 'NewPasswordConfirmed' input value
        /// </summary>
        /// <returns></returns>
        public string GetNewPasswordConfirmedValue()
        {
            return newPasswordConfirmedInput.Value();
        }

        /// <summary>
        /// Get 'ErrorMessage' label text
        /// </summary>
        /// <returns></returns>
        public string GetErrorMessageText()
        {
            return errorMessageLabel.Text;
        }

        #endregion //Get methods

        #endregion //Basic methods

        #region Business methods

        #endregion //Business methods

        public override void WaitForPanelLoaded()
        {
            Wait.ForElementStyle(backButton, "display: block");
        }
    }
}
