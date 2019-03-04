using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using StreetlightVision.Pages.UI;
using System;
using StreetlightVision.Extensions;
using StreetlightVision.Utilities;
using System.Threading;

namespace StreetlightVision.Pages
{
    public class DataHistoryPage : PageBase
    {
        #region Variables

        private GeozoneTreeMainPanel _geozoneTreeMainPanel;
        private LastValuesPanel _lastValuesPanel;
        private GridPanel _gridPanel;
        private GraphPanel _graphPanel;
        private SearchWizardPopupPanel _searchWizardPopupPanel;

        #endregion //Variables

        #region IWebElements        

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-desktop-main'] [id$='browser-show-button']")]
        private IWebElement showButton;

        #endregion //IWebElements

        #region Constructor

        public DataHistoryPage(IWebDriver driver)
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
            return Driver.FindElement(By.CssSelector("[id='switchButtonCanvas']")).TakeScreenshotAsBytes();
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
            Wait.ForMilliseconds(500);
        }              

        /// <summary>
        /// Wait for Last Value panel displayed
        /// </summary>
        public void WaitForLastValuePanelDisplayed()
        {            
            Wait.ForElementStyle(By.CssSelector("[id$='last-values-side-panel']"), string.Format("left: 0px"));
        }

        /// <summary>
        /// Wait for Last Value panel hidden
        /// </summary>
        public void WaitForLastValuePanelDisappeared()
        {
            Wait.ForElementStyle(By.CssSelector("[id$='last-values-side-panel']"), string.Format("left: -350px"));
        }

        public void WaitForGridPanelDisplayed()
        {
            Wait.ForElementStyle(By.CssSelector("[id$='panels_panel_main']"), "left: 380px");
            WaitForPreviousActionComplete();
        }

        /// <summary>
        /// Check if Last Value panel displayed
        /// </summary>
        public bool IsLastValuePanelDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='last-values-side-panel']"));
        }

        /// <summary>
        /// Create a new search
        /// </summary>
        /// <param name="reportName"></param>
        /// <param name="geozone"></param>
        public void CreateNewReport(string reportName, string geozone = "")
        {
            GridPanel.ClickEditButton();
            WaitForSearchWizardPopupPanelDisplayed();
            SearchWizardPopupPanel.ClickNewAdvancedSearchButton();
            SearchWizardPopupPanel.EnterNewSearchNameInput(reportName);
            SearchWizardPopupPanel.ClickNextButton();
            if(!string.IsNullOrEmpty(geozone)) SearchWizardPopupPanel.GeozoneTreePopupPanel.SelectNode(geozone);

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

        #endregion //Business methods

        protected override void WaitForPageReady()
        {
            base.WaitForPageReady();
            GeozoneTreeMainPanel.WaitForPanelLoaded();
            GridPanel.WaitForPanelLoaded();
        }
    }
}
