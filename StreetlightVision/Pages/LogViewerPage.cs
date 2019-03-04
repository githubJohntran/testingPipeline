using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using StreetlightVision.Extensions;
using StreetlightVision.Pages.UI;
using System;

namespace StreetlightVision.Pages
{
    public class LogViewerPage : PageBase
    {
        #region Variables

        private UserTreePanel _userTreePanel;
        private GridPanel _gridPanel;

        #endregion //Variables

        #region IWebElements

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-desktop-main'] .slv-panel-driven-layout-show-left-panel-button")]
        private IWebElement openButton;

        #endregion //IWebElements

        #region Constructor

        public LogViewerPage(IWebDriver driver)
            : base(driver)
        {
            PageFactory.InitElements(this, new RetryingElementLocator(driver, TimeSpan.FromMilliseconds(Settings.LocatorRetryingTimeout), TimeSpan.FromMilliseconds(Settings.LocatorPollingInterval)));

            WaitForPageReady();
        }

        #endregion //Constructor

        #region Properties

        public UserTreePanel UserTreePanel
        {
            get
            {
                if (_userTreePanel == null)
                {
                    _userTreePanel = new UserTreePanel(this.Driver, this);
                }

                return _userTreePanel;
            }
        }

        public GridPanel GridPanel
        {
            get
            {
                if (_gridPanel == null)
                {
                    _gridPanel = new GridPanel(this.Driver, this);
                }

                return _gridPanel;
            }
        }

        #endregion //Properties

        #region Basic methods

        #region Actions

        /// <summary>
        /// Click 'Open' button
        /// </summary>
        public void ClickOpenButton()
        {
            openButton.ClickEx();
        }

        #endregion //Actions

        #region Get methods

        #endregion //Get methods

        #endregion //Basic methods

        #region Business methods

        #endregion //Business methods

        protected override void WaitForPageReady()
        {
            base.WaitForPageReady();
        }
    }
}
