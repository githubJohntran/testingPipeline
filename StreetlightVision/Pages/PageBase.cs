using OpenQA.Selenium;
using System;
using StreetlightVision.Pages.UI;
using StreetlightVision.Utilities;
using StreetlightVision.Extensions;
using SeleniumExtras.PageObjects;

namespace StreetlightVision.Pages
{
    public abstract class PageBase
    {
        #region Variables
        private IWebDriver _driver;
        private AppBar _appBar;
        private SettingsPanel _settingsPanel;
        private Dialog _dialog;

        #endregion //Variables

        #region IWebElements


        [FindsBy(How = How.CssSelector, Using = "div[id='slv-message']")]
        private IWebElement headerMessage;

        #endregion //IWebElements

        #region Constructor

        public PageBase(IWebDriver driver)
        {
            this._driver = driver;
            PageFactory.InitElements(this, new RetryingElementLocator(driver, TimeSpan.FromMilliseconds(Settings.LocatorRetryingTimeout), TimeSpan.FromMilliseconds(Settings.LocatorPollingInterval)));
        }

        #endregion //Constructor

        #region Properties

        public IWebDriver Driver
        {
            get
            {
                return _driver;
            }
        }

        public AppBar AppBar
        {
            get
            {
                if (_appBar == null)
                {
                    _appBar = new AppBar(this.Driver, this);
                }

                return _appBar;
            }
            set
            {
                _appBar = value;
            }
        }

        public SettingsPanel SettingsPanel
        {
            get
            {
                if (_settingsPanel == null)
                {
                    _settingsPanel = new SettingsPanel(this.Driver, this);
                }

                return _settingsPanel;
            }
            set
            {
                _settingsPanel = value;
            }
        }

        public Dialog Dialog
        {
            get
            {
                if (_dialog == null)
                {
                    _dialog = new Dialog(this.Driver, this);
                }

                return _dialog;
            }
        }

        #endregion //Properties

        #region Basic methods

        #region Actions

        #endregion //Actions

        #region Get methods

        #endregion //Get methods

        public string GetHeaderMessage()
        {
            return JSUtility.GetElementText("div[id='slv-message']");
        }

        #endregion //Basic methods

        #region Business methods

        public void WaitForHeaderMessageDisplayed()
        {
            Wait.ForElementDisplayed(By.CssSelector("div[id='slv-message']"));
        }

        public void WaitForHeaderMessageDisappeared()
        {
            Wait.ForElementInvisible(By.CssSelector("div[id='slv-message']"));
        }

        public virtual void WaitForPopupDialogDisplayed()
        {
            Dialog.WaitForPanelLoaded();
        }

        public virtual void WaitForPopupDialogDisappeared()
        {
            Wait.ForElementInvisible(By.CssSelector("[id='w2ui-popup']"));
            Wait.ForMilliseconds(500);
        }

        public virtual void WaitForPopupMessageDialogDisplayed()
        {
            Wait.ForElementDisplayed(By.CssSelector("[id='w2ui-popup'] [id^='w2ui-message']"));
            Wait.ForSeconds(500);
        }

        public virtual void WaitForPopupMessageDialogDisappeared()
        {
            Wait.ForElementInvisible(By.CssSelector("[id='w2ui-popup'] [id^='w2ui-message']"));
            Wait.ForMilliseconds(500);
        }

        public void WaitForSettingsPanelDisplayed()
        {
            Wait.ForElementDisplayed(By.CssSelector("[id='slv-view-desktop-settings']"));
            SettingsPanel.WaitForPanelLoaded();
        }

        public void WaitForSettingsPanelDisappeared()
        {
            Wait.ForElementInvisible(By.CssSelector("[id='slv-view-desktop-settings']"));
        }

        public bool IsHeaderMessageDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("div[id='slv-message']"));
        }

        public bool IsSettingsPanelDisplayed()
        {
            Wait.ForSeconds(2);
            return ElementUtility.IsDisplayed(By.CssSelector("[id='slv-view-desktop-settings']"));
        }

        public LogoutPage TimeoutAuthentication()
        {
            return new LogoutPage(this.Driver);
        }

        public bool IsPopupDialogDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id='w2ui-popup']"));
        }

        public bool IsLoaderSpinDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id='slv-loader']"));
        }

        public bool IsSwitcherOverlayDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector(".slv-switcher"));
        }

        /// <summary>
        /// Hide cookie message shown at bottom
        /// </summary>
        public void HideCookieMessage()
        {
            var byCookieMessage = By.CssSelector("[id='cookieconsent:desc']");
            var byGotItButton = By.CssSelector(".cc-compliance a.cc-btn.cc-dismiss");
            if (Driver.FindElements(byCookieMessage).Count > 0 && ElementUtility.IsDisplayed(byCookieMessage))
            {
                var gotItButton = Driver.FindElement(byGotItButton);
                if (gotItButton != null)
                {
                    gotItButton.ClickEx();
                    Wait.ForElementInvisible(byGotItButton);
                }
            }
        }

        #endregion //Business methods

        protected virtual void WaitForPageReady()
        {
            try
            {
                Wait.ForLoadingIconDisappeared();
                Wait.ForProgressCompleted();
            }
            catch (UnhandledAlertException)
            {
                Wait.ForSeconds(2);
            }
        }

        public virtual void WaitForPreviousActionComplete()
        {
            try
            {
                Wait.ForLoadingIconDisappeared();
                Wait.ForProgressCompleted();
            }
            catch (UnhandledAlertException)
            {
                Wait.ForSeconds(2);
            }
        }
    }
}
