using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using StreetlightVision.Extensions;
using StreetlightVision.Pages.UI;
using System;
using System.Threading;

namespace StreetlightVision.Pages
{
    public class DashboardPage : PageBase
    {
        #region Variables

        private GeozoneTreeMainPanel _geozoneTreeMainPanel;

        #endregion //Variables

        #region IWebElements

        [FindsBy(How = How.CssSelector, Using = "[id='dashboard_box']")]
        private IWebElement chartsArea;

        [FindsBy(How = How.CssSelector, Using = "[id='dashboard_power_chart']")]
        private IWebElement energySavingsChart;

        [FindsBy(How = How.CssSelector, Using = "[id='dashboard_failures']")]
        private IWebElement failuresChart;

        [FindsBy(How = How.CssSelector, Using = "[id='dashboard_inventory_lifetime']")]
        private IWebElement deviceLifetimeChart;

        [FindsBy(How = How.CssSelector, Using = "[id='dashboard_power_chart'] .title")]
        private IWebElement energySavingsChartTitle;

        [FindsBy(How = How.CssSelector, Using = "[id='dashboard_failures'] .title")]
        private IWebElement failuresChartTitle;

        [FindsBy(How = How.CssSelector, Using = "[id='dashboard_inventory_lifetime'] .title")]
        private IWebElement deviceLifetimeChartTitle;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-desktop-main'] [id$='browser-show-button']")]
        private IWebElement showButton;

        #endregion //IWebElements

        #region Constructor

        public DashboardPage(IWebDriver driver)
            : base(driver)
        {
            PageFactory.InitElements(this, new RetryingElementLocator(driver, TimeSpan.FromMilliseconds(Settings.LocatorRetryingTimeout), TimeSpan.FromMilliseconds(Settings.LocatorPollingInterval)));

            WaitForPageReady();
        }

        #endregion //Constructor

        #region Properties

        public GeozoneTreeMainPanel MainGeozoneTreePanel
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

        public string GetEnergySavingsChartTitleText()
        {
            return energySavingsChartTitle.Text;
        }

        public string GetFailuresChartTitleText()
        {
            return failuresChartTitle.Text;
        }

        public string GetDeviceLifetimeChartTitleText()
        {
            return deviceLifetimeChartTitle.Text;
        }

        #endregion //Get methods

        #endregion //Basic methods

        #region Business methods

        public bool IsEnergySavingsChartVisible()
        {
            return energySavingsChart.Displayed;
        }

        public bool IsFailuresChartVisible()
        {
            return failuresChart.Displayed;
        }

        public bool IsDeviceLifetimeChartVisible()
        {
            return deviceLifetimeChart.Displayed;
        }

        public byte[] SaveChartsAsBytes()
        {
            return chartsArea.TakeScreenshotAsBytes();
        }

        #endregion //Business methods

        #region Wait methods

        /// <summary>
        /// Wait for charts 
        /// </summary>
        public void WaitForChartsCompletelyDrawn()
        {
            Wait.ForSeconds(2);
        }

        #endregion
    }
}
