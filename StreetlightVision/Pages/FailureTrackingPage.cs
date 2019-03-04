using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using System;
using StreetlightVision.Extensions;
using StreetlightVision.Pages.UI;
using StreetlightVision.Utilities;
using System.Drawing;
using System.Threading;

namespace StreetlightVision.Pages
{
    public class FailureTrackingPage : PageBase
    {
        #region Variables

        private GeozoneTreeMainPanel _geozoneTreeMainPanel;
        private FailureTrackingDetailsPanel _failureTrackingDetailsPanel;
        private Map _map;

        #endregion //Variables

        #region IWebElements   

        #endregion //IWebElements

        #region Constructor

        public FailureTrackingPage(IWebDriver driver)
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

        public FailureTrackingDetailsPanel FailureTrackingDetailsPanel
        {
            get
            {
                if (_failureTrackingDetailsPanel == null)
                {
                    _failureTrackingDetailsPanel = new FailureTrackingDetailsPanel(this.Driver, this);
                }

                return _failureTrackingDetailsPanel;
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

        public void WaitForDetailsPanelDisplayed()
        {
            Wait.ForElementStyle(By.CssSelector("[id$='failuretrackinggl-editor']"), string.Format("left: {0}px", WebDriverContext.JsExecutor.ExecuteScript("return window.innerWidth - 350 - 60")));
        }

        public void WaitForDetailsPanelDisappeared()
        {
            Wait.ForElementInvisible(By.CssSelector("[id$='failuretrackinggl-editor']"));
        }

        public bool IsDetailsPanelDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='failuretrackinggl-editor']"));
        }

        #endregion //Business methods

        protected override void WaitForPageReady()
        {
            base.WaitForPageReady();
            Map.WaitForProgressGLCompleted();
        }
    }
}
