using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using System;
using StreetlightVision.Extensions;
using StreetlightVision.Pages.UI;

namespace StreetlightVision.Pages
{
    public class FailureAnalysisPage : PageBase
    {
        #region Variables

        private GeozoneTreeMainPanel _geozoneTreeMainPanel;
        private LastValuesPanel _lastValuesPanel;
        private GridPanel _gridPanel;
        private GraphPanel _graphPanel;

        #endregion //Variables

        #region IWebElements

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-desktop-main'] [id$='browser-show-button']")]
        private IWebElement showButton;

        #endregion //IWebElements

        #region Constructor

        public FailureAnalysisPage(IWebDriver driver)
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

        public LastValuesPanel LastValuesPanel
        {
            get
            {
                if (_lastValuesPanel == null)
                {
                    _lastValuesPanel = new LastValuesPanel(this.Driver, this);
                }

                return _lastValuesPanel;
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

        public GraphPanel GraphPanel
        {
            get
            {
                if (_graphPanel == null)
                {
                    _graphPanel = new GraphPanel(this.Driver, this);
                }

                return _graphPanel;
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

        public void WaitForGridPanelDisplayed()
        {
            Wait.ForElementStyle(By.CssSelector("[id$='panels_panel_main']"), "left: 380px");
        }

        #endregion //Business methods

        #region Private  methods

        #endregion //Private  methods

        protected override void WaitForPageReady()
        {
            base.WaitForPageReady();
            this.GeozoneTreeMainPanel.WaitForPanelLoaded();
            this.GridPanel.WaitForPanelLoaded();
        }
    }
}
