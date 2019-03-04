using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using StreetlightVision.Pages.UI;
using System;
using StreetlightVision.Extensions;
using StreetlightVision.Utilities;
using System.Threading;

namespace StreetlightVision.Pages
{
    public class DeviceHistoryPage : PageBase
    {
        #region Variables

        private GeozoneTreeMainPanel _geozoneTreeMainPanel;
        private GridPanel _gridPanel;
        private GraphPanel _graphPanel;
        private SearchWizardPopupPanel _searchWizardPopupPanel;
        private SwitcherOverlayPanel _switcherOverlayPanel;
        private DeviceHistoryPanel _deviceHistoryPanel;

        #endregion //Variables

        #region IWebElements        

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-desktop-main'] [id$='browser-show-button']")]
        private IWebElement showButton;

        #endregion //IWebElements

        #region Constructor

        public DeviceHistoryPage(IWebDriver driver)
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

        public SearchWizardPopupPanel SearchWizardPopupPanel
        {
            get
            {
                if (_searchWizardPopupPanel == null)
                {
                    _searchWizardPopupPanel = new SearchWizardPopupPanel(this.Driver, this);
                }

                return _searchWizardPopupPanel;
            }
        }

        public SwitcherOverlayPanel SwitcherOverlayPanel
        {
            get
            {
                if (_switcherOverlayPanel == null)
                {
                    _switcherOverlayPanel = new SwitcherOverlayPanel(this.Driver, this);
                }

                return _switcherOverlayPanel;
            }
        }

        public DeviceHistoryPanel DeviceHistoryPanel
        {
            get
            {
                if (_deviceHistoryPanel == null)
                {
                    _deviceHistoryPanel = new DeviceHistoryPanel(this.Driver, this);
                }

                return _deviceHistoryPanel;
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

        /// <summary>
        /// Create a fake chart
        /// </summary>
        /// <returns></returns>
        public byte[] CreateFakeChartAsBytes()
        {
            return WebDriverContext.CurrentDriver.FindElement(By.CssSelector("[id='switchButtonCanvas']")).TakeScreenshotAsBytes();
        }

        /// <summary>
        /// Wait for SearchWizardPopupPanel displayed
        /// </summary>
        public void WaitForSearchWizardPopupPanelDisplayed()
        {
            Wait.ForElementDisplayed(By.CssSelector("[id='w2ui-popup']"));
            Wait.ForMilliseconds(500);
        }

        /// <summary>
        /// Wait for SearchWizardPopupPanel hidden
        /// </summary>
        public void WaitForSearchWizardPopupPanelDisappeared()
        {
            Wait.ForElementInvisible(By.CssSelector("[id='w2ui-popup']"));
        }

        public void WaitForGridPanelDisplayed()
        {
            Wait.ForElementStyle(By.CssSelector("[id$='panels_panel_main']"), "left: 380px");
            WaitForPreviousActionComplete();
        }

        /// <summary>
        /// Create a new search
        /// </summary>
        /// <param name="reportName"></param>
        /// <param name="geozone"></param>
        public void CreateNewReport(string reportName, string geozone)
        {
            GridPanel.ClickEditButton();
            WaitForSearchWizardPopupPanelDisplayed();
            SearchWizardPopupPanel.ClickNewAdvancedSearchButton();
            SearchWizardPopupPanel.EnterNewSearchNameInput(reportName);
            SearchWizardPopupPanel.ClickNextButton();
            SearchWizardPopupPanel.GeozoneTreePopupPanel.SelectNode(geozone);

            SearchWizardPopupPanel.ClickNextButton();
            SearchWizardPopupPanel.WaitForAttributeFormDisplayed();

            SearchWizardPopupPanel.ClickNextButton();
            SearchWizardPopupPanel.WaitForFilterFormDisplayed();

            SearchWizardPopupPanel.ClickNextButton();
            SearchWizardPopupPanel.WaitForFinishFormDisplayed();
            SearchWizardPopupPanel.WaitForDeviceSearchCompleted();
            SearchWizardPopupPanel.ClickFinishButton();
            WaitForSearchWizardPopupPanelDisappeared();
            WaitForPreviousActionComplete();
            GridPanel.WaitForPanelLoaded();
        }

        /// <summary>
        /// Check if geozone tree panel is displayed
        /// </summary>
        /// <returns></returns>
        public bool IsGeozoneTreePanelDisplayed()
        {
            return WebDriverContext.CurrentDriver.FindElements(By.CssSelector("[id$='devicehistory-browser-content']")).Count > 0;
        }

        /// <summary>
        /// Check if grid panel is displayed
        /// </summary>
        /// <returns></returns>
        public bool IsGridPanelDisplayed()
        {
            return WebDriverContext.CurrentDriver.FindElements(By.CssSelector("[id$='devicehistory-window_panel_main']")).Count > 0;
        }

        /// <summary>
        /// Check if device history panel is displayed
        /// </summary>
        /// <returns></returns>
        public bool IsDeviceHistoryPanelDisplayed()
        {
            return WebDriverContext.CurrentDriver.FindElements(By.CssSelector("[id$='devicehistory-window_panel_right'][style*='display: block']")).Count > 0;
        }

        /// <summary>
        /// Wait for device history panel displayed
        /// </summary>
        public void WaitForDeviceHistoryPanelDisplayed()
        {
            WebDriverContext.Wait.Until(driver => driver.FindElements(By.CssSelector("[id$='devicehistory-window_panel_right'][style*='display: block']")).Count > 0);
        }

        /// <summary>
        /// Wait for device history panel disappeared
        /// </summary>
        public void WaitForDeviceHistoryPanelDisappeared()
        {
            WebDriverContext.Wait.Until(driver => driver.FindElements(By.CssSelector("[id$='devicehistory-window_panel_right'][style*='display: none']")).Count > 0);
        }

        public void WaitForSwitcherOverlayPanelDisplayed()
        {
            Wait.ForElementDisplayed(By.CssSelector("div.slv-switcher"));
        }

        public void WaitForSwitcherOverlayPanelDisappeared()
        {
            Wait.ForElementInvisible(By.CssSelector("div.slv-switcher"));
        }

        #endregion //Business methods

        protected override void WaitForPageReady()
        {
            base.WaitForPageReady();
            GeozoneTreeMainPanel.WaitForPanelLoaded();
            GridPanel.WaitForPanelLoaded();
        }
    }
}
