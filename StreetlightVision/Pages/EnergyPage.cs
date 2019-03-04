using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using StreetlightVision.Extensions;
using StreetlightVision.Pages.UI;
using System;

namespace StreetlightVision.Pages
{
    public class EnergyPage : PageBase
    {
        #region Variables

        private GeozoneTreeMainPanel _geozoneTreeMainPanel;
        private GridPanel _gridPanel;
        private LastValuesPanel _lastValuePanel;
        private GraphPanel _graphPanel;

        #endregion //Variables

        #region IWebElements

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-desktop-main'] [id$='browser-show-button']")]
        private IWebElement showButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='energy-browser-content'] div.w2ui-node-caption")]
        private IWebElement lastLoadedElement;

        #endregion //IWebElements

        #region Constructor

        public EnergyPage(IWebDriver driver)
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

        public LastValuesPanel LastValuesPanel
        {
            get
            {
                if (_lastValuePanel == null)
                {
                    _lastValuePanel = new LastValuesPanel(this.Driver, this);
                }

                return _lastValuePanel;
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

        #endregion //Business methods

        #region Private methods

        #endregion //Private  methods
    }
}
