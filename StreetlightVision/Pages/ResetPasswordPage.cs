using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using StreetlightVision.Extensions;
using System;

namespace StreetlightVision.Pages
{
    public class ResetPasswordPage : PageBase
    {
        #region Variables

        #endregion //Variables

        #region IWebElements

        [FindsBy(How = How.CssSelector, Using = "span.propertyLabel")]
        private IWebElement resetPasswordMessage;

        #endregion //IWebElements

        #region Constructor

        public ResetPasswordPage(IWebDriver driver)
            : base(driver)
        {
            PageFactory.InitElements(this, new RetryingElementLocator(driver, TimeSpan.FromMilliseconds(Settings.LocatorRetryingTimeout), TimeSpan.FromMilliseconds(Settings.LocatorPollingInterval)));
            
            WaitForPageReady();
        }

        #endregion //Constructor

        #region Basic methods

        #region Actions      

        #endregion //Actions

        #region Get methods

        /// <summary>
        /// Get 'ResetPasswordMessage' text
        /// </summary>
        /// <returns></returns>
        public string GetResetPasswordMessageText()
        {
            return resetPasswordMessage.Text;
        }        

        #endregion //Get methods

        #endregion //Basic methods

        #region Business methods        

        #endregion //Business methods

        #region Private methods        

        #endregion //Private methods

        protected override void WaitForPageReady()
        {
            Wait.ForElementText(resetPasswordMessage);
        }
    }
}
