using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using System;
using StreetlightVision.Pages.UI;
using StreetlightVision.Extensions;

namespace StreetlightVision.Pages
{
    public class ControlCenterlPage : PageBase
    {
        #region Variables

        private GeozoneTreeMainPanel _geozoneTreeMainPanel;              
        private ControlCenterMonitoringPanel _controlCenterMonitoringPanel;
        private ControlCenterMonitoringSettingPanel _controlCenterMonitoringSettingPanel;

        #endregion //Variables

        #region IWebElements
        
        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-desktop-main'] [id$='browser-show-button']")]
        private IWebElement showButton;

        #endregion //IWebElements

        #region Constructor

        public ControlCenterlPage(IWebDriver driver)
            : base(driver)                                                                                                                                                                                                                                                                                                                                                                             
        {
            PageFactory.InitElements(this, new RetryingElementLocator(driver, TimeSpan.FromMilliseconds(Settings.LocatorRetryingTimeout), TimeSpan.FromMilliseconds(Settings.LocatorPollingInterval)));
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

        public ControlCenterMonitoringPanel ControlCenterMonitoringPanel
        {
            get
            {
                if (_controlCenterMonitoringPanel == null)
                {
                    _controlCenterMonitoringPanel = new ControlCenterMonitoringPanel(this.Driver, this);
                }

                return _controlCenterMonitoringPanel;
            }            
        }

        public ControlCenterMonitoringSettingPanel ControlCenterMonitoringSettingPanel
        {
            get
            {
                if (_controlCenterMonitoringSettingPanel == null)
                {
                    _controlCenterMonitoringSettingPanel = new ControlCenterMonitoringSettingPanel(this.Driver, this);
                }

                return _controlCenterMonitoringSettingPanel;
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

        protected override void WaitForPageReady()
        {

        }        
    }
}
