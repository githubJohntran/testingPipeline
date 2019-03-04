using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using StreetlightVision.Extensions;
using StreetlightVision.Pages.UI;
using StreetlightVision.Utilities;
using System;

namespace StreetlightVision.Pages
{
    public class RealTimeControlPage : PageBase
    {
        #region Variables

        private GeozoneTreeMainPanel _geozoneTreeMainPanel;
        private SunriseSunsetTimesPopupPanel _sunriseSunsetTimesPopupPanel;
        private StreetlightWidgetPanel _streetlightWidgetPanel;
        private ControllerWidgetPanel _controllerWidgetPanel;
        private CabinetControllerWidgetPanel _cabinetControllerWidgetPanel;
        private DeviceWidgetPanel _deviceWidgetPanel;
        private Map _map;
        private MultipleStreetlightsWidgetPanel _multipleStreetlightsWidgetPanel;

        #endregion //Variables

        #region IWebElements

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-desktop-main'] [id$='browser-show-button']")]
        private IWebElement showButton;

        #endregion //IWebElements

        #region Constructor

        public RealTimeControlPage(IWebDriver driver)
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

        public SunriseSunsetTimesPopupPanel SunriseSunsetTimesPopupPanel
        {
            get
            {
                if (_sunriseSunsetTimesPopupPanel == null)
                {
                    _sunriseSunsetTimesPopupPanel = new SunriseSunsetTimesPopupPanel(this.Driver, this);
                }

                return _sunriseSunsetTimesPopupPanel;
            }
        }

        public StreetlightWidgetPanel StreetlightWidgetPanel
        {
            get
            {
                if (_streetlightWidgetPanel == null)
                {
                    _streetlightWidgetPanel = new StreetlightWidgetPanel(this.Driver, this);
                }

                return _streetlightWidgetPanel;
            }
        }

        public ControllerWidgetPanel ControllerWidgetPanel
        {
            get
            {
                if (_controllerWidgetPanel == null)
                {
                    _controllerWidgetPanel = new ControllerWidgetPanel(this.Driver, this);
                }

                return _controllerWidgetPanel;
            }
        }

        public CabinetControllerWidgetPanel CabinetControllerWidgetPanel
        {
            get
            {
                if (_cabinetControllerWidgetPanel == null)
                {
                    _cabinetControllerWidgetPanel = new CabinetControllerWidgetPanel(this.Driver, this);
                }

                return _cabinetControllerWidgetPanel;
            }
        }

        public DeviceWidgetPanel DeviceWidgetPanel
        {
            get
            {
                if (_deviceWidgetPanel == null)
                {
                    _deviceWidgetPanel = new DeviceWidgetPanel(this.Driver, this);
                }

                return _deviceWidgetPanel;
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

        public MultipleStreetlightsWidgetPanel MultipleStreetlightsWidgetPanel
        {
            get
            {
                if (_multipleStreetlightsWidgetPanel == null)
                {
                    _multipleStreetlightsWidgetPanel = new MultipleStreetlightsWidgetPanel(this.Driver, this);
                }

                return _multipleStreetlightsWidgetPanel;
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

        public void WaitForMultipleStreetlightsWidgetDisplayed()
        {
            Wait.ForElementDisplayed(By.CssSelector("[id$='widgetPanel'] [id*='streetlights'].realtime-gl-widget-streetlights"));
            Wait.ForElementText(By.CssSelector("[id$='widgetPanel'] [id*='streetlights'][id$='time'].tile-title"));
            Wait.ForSeconds(3);
        }

        public void WaitForMultipleStreetlightsWidgetDisappeared()
        {
            Wait.ForElementInvisible(By.CssSelector("[id$='widgetPanel'] [id*='streetlights'].realtime-gl-widget-streetlights"));
        }

        public void WaitForStreetlightWidgetDisplayed(string deviceName)
        {
            StreetlightWidgetPanel.WaitForNameText(deviceName);
        }

        public void WaitForStreetlightWidgetDisappeared()
        {
            Wait.ForElementInvisible(By.CssSelector("[id$='widgetPanel'] div[id*='streetlight'].streetlight"));
        }

        public void WaitForControllerWidgetDisplayed(string deviceName)
        {
            ControllerWidgetPanel.WaitForNameText(deviceName);
        }

        public void WaitForControllerWidgetDisappeared()
        {
            Wait.ForElementInvisible(By.CssSelector("[id$='widgetPanel'] div[id*='controller'].controller"));
        }

        public void WaitForCabinetControllerWidgetDisplayed(string deviceName)
        {
            CabinetControllerWidgetPanel.WaitForNameText(deviceName);
        }

        public void WaitForCabinetControllerWidgetDisappeared()
        {
            Wait.ForElementInvisible(By.CssSelector("[id$='widgetPanel'] .cabinetcontroller"));
        }

        public void WaitForPopupMessageDisplayed()
        {
            Dialog.WaitForPanelLoaded();
        }

        public void WaitForPopupMessageDisappeared()
        {
            Wait.ForElementInvisible(By.CssSelector("[id='w2ui-popup']"));
        }

        public void WaitForMainGeozoneTreeDisplayed()
        {
            Wait.ForElementStyle(By.CssSelector("[id$='browser']"), "left: 20px");
        }

        public void WaitForMainGeozoneTreeDisappeared()
        {
            Wait.ForElementStyle(By.CssSelector("[id$='browser']"), "left: -330px");
        }

        public void OpenGeozoneTreeIfNotExpand()
        {
            var isShowButtonDisplayed = ElementUtility.IsDisplayed(By.CssSelector("[id='slv-view-desktop-main'] [id$='browser-show-button']"));
            if (isShowButtonDisplayed)
            {
                ClickShowButton();
                WaitForMainGeozoneTreeDisplayed();
            }
        }

        public bool IsWidgetDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='widgetPanel'] .slv-control.slv-rounded-control"));
        }

        public bool IsControllerWidgetDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='widgetPanel'] [id*='controller']"));
        }

        public bool IsStreetlightWidgetDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='widgetPanel'] [id*='streetlight']"));
        }

        public bool IsCabinetControllerWidgetDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='widgetPanel'] .cabinetcontroller"));
        }

        public bool IsSunriseSunsetPopupDialogDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id='w2ui-popup'] .slv-ephemeris"));
        }

        public bool IsMultipleStreetlightsWidgetDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='widgetPanel'] [id*='streetlights'].realtime-gl-widget-streetlights"));
        }

        #endregion //Business methods

        protected override void WaitForPageReady()
        {
            base.WaitForPageReady();
            Map.WaitForProgressGLCompleted();
            OpenGeozoneTreeIfNotExpand();
            GeozoneTreeMainPanel.WaitForPanelLoaded();
        }
    }
}
