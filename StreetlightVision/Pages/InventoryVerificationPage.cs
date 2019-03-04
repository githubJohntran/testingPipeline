using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using StreetlightVision.Extensions;
using StreetlightVision.Pages.UI;
using StreetlightVision.Utilities;
using System;

namespace StreetlightVision.Pages
{
    public class InventoryVerificationPage: PageBase
    {
        #region Variables

        private GeozoneTreeMainPanel _geozoneTreeMainPanel;
        private VerificationPopupPanel _verificationPopupPanel;
        private Map _map;

        #endregion //Variables

        #region IWebElements

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-desktop-main'] [id$='browser-show-button']")]
        private IWebElement showButton;

        #endregion //IWebElements

        #region Constructor

        public InventoryVerificationPage(IWebDriver driver)
            : base(driver)
        {
            PageFactory.InitElements(this, new RetryingElementLocator(driver, TimeSpan.FromMilliseconds(Settings.LocatorRetryingTimeout), TimeSpan.FromMilliseconds(Settings.LocatorPollingInterval)));
            WaitForPageReady();
        }

        #endregion //Constructor

        #region Properties

        public GeozoneTreeMainPanel GeozoneTreeMainPanel
        {
            get
            {
                if (_geozoneTreeMainPanel == null)
                {
                    _geozoneTreeMainPanel = new GeozoneTreeMainPanel(this.Driver, this);
                }

                return _geozoneTreeMainPanel;
            }
        }

        public VerificationPopupPanel VerificationPopupPanel
        {
            get
            {
                if (_verificationPopupPanel == null)
                {
                    _verificationPopupPanel = new VerificationPopupPanel(this.Driver, this);
                }

                return _verificationPopupPanel;
            }            
        }

        public Map Map
        {
            get
            {
                if (_map == null)
                {
                    _map = new Map(this.Driver, this);
                }

                return _map;
            }
        }

        #endregion //Properties

        #region Basic methods

        #region Actions

        /// <summary>
        /// Click 'Show' button
        /// </summary>
        public void ClickShowButton()
        {
            showButton.ClickEx();
        }

        #endregion //Actions

        #region Get methods

        #endregion //Get methods

        #endregion //Basic methods

        #region Business methods        

        public void WaitForMainGeozoneTreeDisplayed()
        {
            Wait.ForElementStyle(By.CssSelector("[id$='browser']"), "left: 20px");
        }

        public void WaitForMainGeozoneTreeDisappeared()
        {
            Wait.ForElementStyle(By.CssSelector("[id$='browser']"), "left: -330px");
        }
        public void OpenGeozoneTreeIfNotExpand()
        {
            var isShowButtonDisplayed = ElementUtility.IsDisplayed(By.CssSelector("[id='slv-view-desktop-main'] [id$='browser-show-button']"));
            if (isShowButtonDisplayed)
            {
                ClickShowButton();
                WaitForMainGeozoneTreeDisplayed();
            }
        }

        #endregion //Business methods

        protected override void WaitForPageReady()
        {
            base.WaitForPageReady();
            OpenGeozoneTreeIfNotExpand();
            GeozoneTreeMainPanel.WaitForPanelLoaded();
            Map.WaitForLoaded();
        }
    }
}
