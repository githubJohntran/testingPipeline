using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using StreetlightVision.Extensions;
using StreetlightVision.Pages.UI;
using StreetlightVision.Utilities;
using System;

namespace StreetlightVision.Pages
{
    public class BatchControlPage : PageBase
    {
        #region Variables

        private GeozoneTreeMainPanel _geozoneTreeMainPanel;
        private RealTimeBatchPanel _batchControlSearchPanel;
        private Map _map;

        #endregion //Variables

        #region IWebElements


        #endregion //IWebElements

        #region Constructor

        public BatchControlPage(IWebDriver driver)
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

        public RealTimeBatchPanel RealTimeBatchPanel
        {
            get
            {
                if (_batchControlSearchPanel == null)
                {
                    _batchControlSearchPanel = new RealTimeBatchPanel(this.Driver, this);
                }

                return _batchControlSearchPanel;
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

        #endregion //Actions

        #region Get methods

        #endregion //Get methods

        #endregion //Basic methods

        #region Business methods        

        public string GetRealtimeBatchWarningMessageTopOfScreen()
        {
            var selectorOfTopMessage = "[id='slv'] [id='slv-message']";
            var topMessage = Driver.FindElement(By.CssSelector(selectorOfTopMessage));
            return topMessage.Text;
        }        

        public void WaitForWarningMessageDisplayed()
        {
            Wait.ForElementDisplayed(By.CssSelector("[id='slv-message'] .slv-realtimebatch-warning-toast"));
        }

        public void WaitForWarningMessageTopOfScreenDisappeared()
        {
            Wait.ForElementsInvisible(By.CssSelector("[id='slv'] [id='slv-message']"));
        }

        public bool IsRealTimeBatchPanelDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id='slv-realtimebatch-gridpanel']"));
        }

        #endregion //Business methods

        protected override void WaitForPageReady()
        {
            base.WaitForPageReady();
        }        
    }
}
