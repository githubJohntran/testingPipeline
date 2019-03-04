using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using StreetlightVision.Extensions;
using StreetlightVision.Pages.UI;
using System;
using System.Threading;

namespace StreetlightVision.Pages
{
    public class MonthlyEnergySavingsPage : PageBase
    {
        #region Variables

        private GeozoneTreeMainPanel _geozoneTreeMainPanel;

        #endregion //Variables

        #region IWebElements

        [FindsBy(How = How.CssSelector, Using = "[id='tb_energysaving_header_w2ui_item_yearly'] table.w2ui-button")]
        private IWebElement yearlyToolbarOption;

        [FindsBy(How = How.CssSelector, Using = "[id='tb_energysaving_header_w2ui_item_monthly'] table.w2ui-button")]
        private IWebElement monthlyToolbarOption;

        [FindsBy(How = How.CssSelector, Using = "[id='tb_energysaving_header_w2ui_item_weekly'] table.w2ui-button")]
        private IWebElement weeklyToolbarOption;

        [FindsBy(How = How.CssSelector, Using = "[id='tb_energysaving_header_w2ui_item_daily'] table.w2ui-button")]
        private IWebElement dailyToolbarOption;

        [FindsBy(How = How.CssSelector, Using = "[id='tb_energysaving_header_w2ui_item_export'] table.w2ui-button")]
        private IWebElement exportToolbarButton;

        [FindsBy(How = How.CssSelector, Using = "[id='tb_energysaving_header_w2ui_item_datefields'] input[type='date1']")]
        private IWebElement fromDateInput;

        [FindsBy(How = How.CssSelector, Using = "[id='tb_energysaving_header_w2ui_item_datefields'] input[type='date2']")]
        private IWebElement toDateInput;

        [FindsBy(How = How.CssSelector, Using = "[id='tb_energysaving_header_w2ui_item_update'] table.w2ui-button")]
        private IWebElement executeToolbarButton;

        [FindsBy(How = How.CssSelector, Using = "[id='tb_energysaving_header_w2ui_item_grouped'] table.w2ui-button")]
        private IWebElement groupedToolbarOption;

        [FindsBy(How = How.CssSelector, Using = "[id='tb_energysaving_header_w2ui_item_difference'] table.w2ui-button")]
        private IWebElement differenceToolbarOption;

        [FindsBy(How = How.CssSelector, Using = "[id='energysaving_chart']")]
        private IWebElement chartsArea;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-desktop-main'] [id$='browser-show-button']")]
        private IWebElement showButton;

        #endregion //IWebElements

        #region Constructor

        public MonthlyEnergySavingsPage(IWebDriver driver)
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

        #endregion //Properties

        #region Basic methods

        #region Actions

        public void ClickYearlyOption()
        {
            yearlyToolbarOption.ClickEx();
        }

        public void ClickMonthlyOption()
        {
            monthlyToolbarOption.ClickEx();
        }

        public void ClickWeeklyOption()
        {
            weeklyToolbarOption.ClickEx();
        }

        public void ClickDailyOption()
        {
            dailyToolbarOption.ClickEx();
        }

        public void ClickExportIcon()
        {
            exportToolbarButton.ClickEx();
        }

        public void EnterDateFrom(DateTime date)
        {
            fromDateInput.Enter(date);
        }

        public void EnterDateTo(DateTime date)
        {
            toDateInput.Enter(date);
        }

        public void ClickExecuteButton()
        {
            executeToolbarButton.ClickEx();
        }

        public void ClickGroupedOption()
        {
            groupedToolbarOption.ClickEx();
        }

        public void ClickDifferenceOption()
        {
            differenceToolbarOption.ClickEx();
        }

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

        public byte[] SaveChartsAsBytes()
        {
            return chartsArea.TakeScreenshotAsBytes();
        }

        /// <summary>
        /// Wait for charts 
        /// </summary>
        public void WaitForChartsCompletelyDrawn()
        {
            Wait.ForSeconds(3);
        }

        #endregion //Business methods
    }
}
