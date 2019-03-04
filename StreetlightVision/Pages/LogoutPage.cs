using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using StreetlightVision.Extensions;
using System;

namespace StreetlightVision.Pages
{
    public class LogoutPage : PageBase
    {
        #region Variables

        #endregion //Variables

        #region IWebElements

        [FindsBy(How = How.CssSelector, Using = "span.propertyLabel")]
        private IWebElement logoutMessage;

        #endregion //IWebElements

        #region Constructor

        public LogoutPage(IWebDriver driver)
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
        /// Get 'LogoutMessage' text
        /// </summary>
        /// <returns></returns>
        public string GetLogoutMessageText()
        {
            return logoutMessage.Text;
        }        

        #endregion //Get methods

        #endregion //Basic methods

        #region Business methods        

        #endregion //Business methods

        #region Private methods        

        #endregion //Private methods

        protected override void WaitForPageReady()
        {
            Wait.ForSeconds(2);
            Wait.ForElementText(logoutMessage);           
        }
    }
}
