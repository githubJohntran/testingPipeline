using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using StreetlightVision.Extensions;
using StreetlightVision.Pages.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StreetlightVision.Pages
{
    public class DeviceLifetimePage : PageBase
    {
        #region Variables

        private GeozoneTreeMainPanel _geozoneTreeMainPanel;
        private GridPanel _gridPanel;

        #endregion //Variables

        #region IWebElements

        [FindsBy(How = How.CssSelector, Using = "[id='lamplifetime_vertical_chart']")]
        private IWebElement lifetimeRangeChart;

        [FindsBy(How = How.CssSelector, Using = "[id='lamplifetime_vertical_chart'] g text")]
        private IList<IWebElement> lifetimeRangeList;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-desktop-main'] [id$='browser-show-button']")]
        private IWebElement showButton;

        #endregion //IWebElements

        #region Constructor

        public DeviceLifetimePage(IWebDriver driver)
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

        #endregion

        #region Basic methods

        #region Actions

        public void ClickLifetimeRange()
        {
            throw new NotImplementedException();
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

        /// <summary>
        /// Check if lifetime range chart is visible
        /// </summary>
        /// <returns></returns>
        public bool IsLifetimeRangeChartVisible()
        {
            return lifetimeRangeChart.Displayed;
        }

        /// <summary>
        /// Get list of ranges as text
        /// </summary>
        /// <returns></returns>
        public List<string> GetLifetimeTextRanges()
        {
            var ranges = new List<string>();

            foreach (var rangeElement in lifetimeRangeList)
            {
                ranges.Add(rangeElement.Text);
            }

            return ranges;
        }

        public void SelectLifetimeRange(string range)
        {
            var rangeElement = lifetimeRangeList.FirstOrDefault(r => r.Text.Contains(range));

            rangeElement.ClickEx();
        }

        #endregion //Business methods
    }
}
