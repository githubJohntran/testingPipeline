using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using StreetlightVision.Extensions;
using StreetlightVision.Utilities;
using System;
using System.Threading;

namespace StreetlightVision.Pages.UI
{
    public class SunriseSunsetTimesPopupPanel : PanelBase
    {
        #region Variables

        private GeozoneTreePopupPanel _geozoneTreePopupPanel;

        #endregion //Variables

        #region IWebElements       

        #region Header

        [FindsBy(How = How.CssSelector, Using = "[id='w2ui-popup'] div.slv-ephemeris-title")]
        private IWebElement panelTitle;
        
        [FindsBy(How = How.CssSelector, Using = "[id='w2ui-popup'] div.icon-sunrise-sunset.slv-ephemeris-icon")]
        private IWebElement dialogTitleIcon;

        [FindsBy(How = How.CssSelector, Using = "[id='w2ui-popup'] [id$='label-current-year']")]
        private IWebElement calendarYearLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='w2ui-popup'] [id$='button-previous-year']")]
        private IWebElement calendarBackButton;

        [FindsBy(How = How.CssSelector, Using = "[id='w2ui-popup'] [id$='button-next-year']")]
        private IWebElement calendarNextButton;

        #endregion //Header

        #region Content

        [FindsBy(How = How.CssSelector, Using = "[id='w2ui-popup'] [id$='header'].slv-title")]
        private IWebElement headerTitleLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='w2ui-popup'] [id$='chart-content']")]
        private IWebElement chartContentSvg;

        [FindsBy(How = How.CssSelector, Using = "[id='w2ui-popup'] [id$='chart-axis']")]
        private IWebElement chartAxis;

        [FindsBy(How = How.CssSelector, Using = "[id='w2ui-popup'] [id$='chart-cursor']")]
        private IWebElement chartCursorCanvas;

        [FindsBy(How = How.CssSelector, Using = "[id='w2ui-popup'] [id$='footer-value-sunrise']")]
        private IWebElement sunriseTimeLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='w2ui-popup'] [id$='footer-value-sunset']")]
        private IWebElement sunsetTimeLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='w2ui-popup'] [id$='footer-value-night']")]
        private IWebElement nightTimeLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='w2ui-popup'] [id$='footer-value-day']")]
        private IWebElement dayTimeLabel;

        #endregion //Content

        #region Footer

        [FindsBy(How = How.CssSelector, Using = "[id='w2ui-popup'] [id$=timezone']")]
        private IWebElement timezoneLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='w2ui-popup'] [id$='button-export']")]
        private IWebElement exportButton;

        [FindsBy(How = How.CssSelector, Using = "[id='w2ui-popup'] [id$='button-close']")]
        private IWebElement closeButton;

        #endregion //Footer

        #endregion //IWebElements

        #region Constructor

        public SunriseSunsetTimesPopupPanel(IWebDriver driver, PageBase page)
            : base(driver, page) 
        {
            PageFactory.InitElements(this, new RetryingElementLocator(driver, TimeSpan.FromMilliseconds(Settings.LocatorRetryingTimeout), TimeSpan.FromMilliseconds(Settings.LocatorPollingInterval)));
        }

        #endregion //Constructor

        #region Basic methods

        #region Actions

        #region Header

        /// <summary>
        /// Click 'CalendarBack' button
        /// </summary>
        public void ClickCalendarBackButton()
        {
            calendarBackButton.ClickEx();
        }

        /// <summary>
        /// Click 'CalendarNext' button
        /// </summary>
        public void ClickCalendarNextButton()
        {
            calendarNextButton.ClickEx();
        }

        #endregion //Header

        #region Content

        #endregion //Content

        #region Footer

        /// <summary>
        /// Click 'Export' button
        /// </summary>
        public void ClickExportButton()
        {
            exportButton.ClickEx();
        }

        /// <summary>
        /// Click 'Close' button
        /// </summary>
        public void ClickCloseButton()
        {
            closeButton.ClickEx();
        }

        #endregion //Footer

        #endregion //Actions

        #region Get methods

        #region Header

        /// <summary>
        /// Get 'PanelTitle' text
        /// </summary>
        /// <returns></returns>
        public string GetPanelTitleText()
        {
            return panelTitle.Text;
        }

        /// <summary>
        /// Get 'DialogTitleIcon' input value
        /// </summary>
        /// <returns></returns>
        public string GetDialogTitleIconValue()
        {
            return dialogTitleIcon.IconValue();
        }

        /// <summary>
        /// Get 'CalendarYear' label text
        /// </summary>
        /// <returns></returns>
        public string GetCalendarYearText()
        {
            return calendarYearLabel.Text;
        }

        #endregion //Header

        #region Content

        /// <summary>
        /// Get 'HeaderTitle' label text
        /// </summary>
        /// <returns></returns>
        public string GetHeaderTitleText()
        {
            return headerTitleLabel.Text;
        }

        /// <summary>
        /// Get 'SunriseTime' label text
        /// </summary>
        /// <returns></returns>
        public string GetSunriseTimeText()
        {
            return sunriseTimeLabel.Text;
        }

        /// <summary>
        /// Get 'SunsetTime' label text
        /// </summary>
        /// <returns></returns>
        public string GetSunsetTimeText()
        {
            return sunsetTimeLabel.Text;
        }

        /// <summary>
        /// Get 'NightTime' label text
        /// </summary>
        /// <returns></returns>
        public string GetNightTimeText()
        {
            return nightTimeLabel.Text;
        }

        /// <summary>
        /// Get 'DayTime' label text
        /// </summary>
        /// <returns></returns>
        public string GetDayTimeText()
        {
            return dayTimeLabel.Text;
        }

        #endregion //Content

        #region Footer

        /// <summary>
        /// Get 'Timezone' label text
        /// </summary>
        /// <returns></returns>
        public string GetTimezoneText()
        {
            return timezoneLabel.Text;
        }

        #endregion //Footer

        #endregion //Get methods

        #endregion //Basic methods

        #region Business methods

        public void MoveToRandomInsideChart()
        {
            var random = new Random();
            var x = chartContentSvg.Size.Width / 2 - random.Next(-200, 200);
            var y = chartContentSvg.Size.Height / 2 - random.Next(-20, 20);

            chartContentSvg.MoveToAndClick(x, y);
            Wait.ForSeconds(2);
        }

        public bool IsLoaderDisplayed()
        {
            Wait.ForSeconds(2);
            return ElementUtility.IsDisplayed(By.CssSelector("[id='w2ui-popup'] [id$='loader'].loader"));
        }

        #endregion //Business methods
    }
}