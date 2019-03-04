using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using System;
using StreetlightVision.Utilities;
using System.Threading;
using StreetlightVision.Extensions;

namespace StreetlightVision.Pages.UI
{
    public class SettingsPanel : PanelBase
    {
        #region Variables

        private ChangeMyPasswordPanel _changeMyPasswordPanel;
        private AboutPanel _aboutPanel;
        private StorePanel _storePanel;

        #endregion //Variables

        #region IWebElements

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-desktop-settings-content'] .slv-item:nth-child(1)")]
        private IWebElement logoutLink;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-desktop-settings-content'] .slv-item:nth-child(2)")]
        private IWebElement changeMyPasswordLink;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-desktop-settings-content'] .slv-item:nth-child(3)")]
        private IWebElement storeLink;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-desktop-settings-content'] .slv-item:nth-child(4)")]
        private IWebElement aboutLink;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-desktop-settings']")]
        private IWebElement settingsPanel;

        #endregion //IWebElements

        #region Constructor

        public SettingsPanel(IWebDriver driver, PageBase page)
            : base(driver, page)
        {
            PageFactory.InitElements(this, new RetryingElementLocator(driver, TimeSpan.FromMilliseconds(Settings.LocatorRetryingTimeout), TimeSpan.FromMilliseconds(Settings.LocatorPollingInterval)));
            WaitForPanelLoaded();
        }

        #endregion //Constructor

        #region Properties

        public ChangeMyPasswordPanel ChangeMyPasswordPanel
        {
            get
            {
                if (_changeMyPasswordPanel == null)
                {
                    _changeMyPasswordPanel = new ChangeMyPasswordPanel(this.Driver, this.Page);
                }

                return _changeMyPasswordPanel;
            }
        }

        public AboutPanel AboutPanel
        {
            get
            {
                if (_aboutPanel == null)
                {
                    _aboutPanel = new AboutPanel(this.Driver, this.Page);
                }

                return _aboutPanel;
            }
        }

        public StorePanel StorePanel
        {
            get
            {
                if (_storePanel == null)
                {
                    _storePanel = new StorePanel(this.Driver, this.Page);
                }

                return _storePanel;
            }
        }

        #endregion //Properties

        #region Basic methods

        #region Actions

        public LogoutPage ClickLogoutLink()
        {
            JSUtility.ClickOnElement("[id='slv-view-desktop-settings-content'] .slv-item:nth-child(1)");

            return new LogoutPage(this.Driver);
        }

        public void ClickChangeMyPasswordLink()
        {
            JSUtility.ClickOnElement("[id='slv-view-desktop-settings-content'] .slv-item:nth-child(2)");
        }

        public void ClickStoreLink()
        {
            JSUtility.ClickOnElement("[id='slv-view-desktop-settings-content'] .slv-item:nth-child(3)");            
        }

        public void ClickAboutLink()
        {
            JSUtility.ClickOnElement("[id='slv-view-desktop-settings-content'] .slv-item:nth-child(4)");            
        }

        #endregion //Actions

        #region Get methods

        public string GetPanelHeaderText()
        {
            throw new NotImplementedException();
        }

        #endregion //Get methods

        #endregion //Basic methods

        #region Business methods  

        #region Wait methods 

        public void WaitForStorePanelDisplayed()
        {
            Wait.ForElementDisplayed(By.CssSelector("[id='desktop-settings-panel-store']"));
            Wait.ForElementStyle(By.CssSelector("[id='desktop-settings-panel-store']"), "left: 0px");
        }

        public void WaitForStorePanelDisappeared()
        {
            Wait.ForElementInvisible(By.CssSelector("[id='desktop-settings-panel-store']"));
            Wait.ForElementStyle(By.CssSelector("[id='desktop-settings-panel-store']"), "left: 350px");
        }

        public void WaitForAboutPanelDisplayed()
        {
            Wait.ForElementDisplayed(By.CssSelector("[id='desktop-settings-panel-about']"));
            Wait.ForElementStyle(By.CssSelector("[id='desktop-settings-panel-about']"), "left: 0px");
        }

        public void WaitForAboutPanelDisappeared()
        {
            Wait.ForElementInvisible(By.CssSelector("[id='desktop-settings-panel-about']"));
            Wait.ForElementStyle(By.CssSelector("[id='desktop-settings-panel-about']"), "left: 350px");
        }

        public void WaitForChangeMyPasswordPanelDisplayed()
        {
            Wait.ForElementDisplayed(By.CssSelector("[id='desktop-settings-panel-password']"));
            Wait.ForElementStyle(By.CssSelector("[id='desktop-settings-panel-password']"), "left: 0px");
        }

        public void WaitForChangeMyPasswordPanelDisappeared()
        {
            Wait.ForElementInvisible(By.CssSelector("[id='desktop-settings-panel-password']"));
            Wait.ForElementStyle(By.CssSelector("[id='desktop-settings-panel-password']"), "left: 350px");
        }

        #endregion //Wait methods 

        #endregion //Business methods       

        public override void WaitForPanelLoaded()
        {
            if (string.Equals(Settings.Browser, "Chrome"))
            {
                Wait.ForElementStyle(By.CssSelector("[id='slv-view-desktop-settings']"), string.Format("left: {0}px", WebDriverContext.JsExecutor.ExecuteScript("return screen.availWidth - 350")));
            }
            else
                Wait.ForMilliseconds(500);
        }
    }
}
