using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using StreetlightVision.Extensions;
using System;
using System.Collections.Generic;

namespace StreetlightVision.Pages.UI
{
    public class ControlCenterMonitoringPanel : PanelBase
    {
        #region Variables

        #endregion //Variables

        #region IWebElements        

        [FindsBy(How = How.CssSelector, Using = "[id='ControlCenter-controlcenter-editor'] [id='tb_controlcenterMonitoringButtons_item_close'] >  table")]
        private IWebElement closeButton;

        [FindsBy(How = How.CssSelector, Using = "[id='ControlCenter-controlcenter-editor'] [id='tb_controlcenterMonitoringButtons_item_start'] >  table")]
        private IWebElement startStopButton;

        [FindsBy(How = How.CssSelector, Using = "[id='ControlCenter-controlcenter-editor'] [id='tb_controlcenterMonitoringButtons_item_settings'] >  table")]
        private IWebElement settingButton;

        [FindsBy(How = How.CssSelector, Using = "[id='ControlCenter-controlcenter-editor'] [id*='monitoring-item'][id*=' monitor-content']")]
        private IList<IWebElement> monitoringItemsList;

        #endregion //IWebElements

        #region Constructor

        public ControlCenterMonitoringPanel(IWebDriver driver, PageBase page) : base(driver, page)
        {
            PageFactory.InitElements(this, new RetryingElementLocator(driver, TimeSpan.FromMilliseconds(Settings.LocatorRetryingTimeout), TimeSpan.FromMilliseconds(Settings.LocatorPollingInterval)));
        }

        #endregion //Constructor

        #region Basic methods     

        #region Actions

        /// <summary>
        /// Click 'Close' button
        /// </summary>
        public void ClickCloseButton()
        {
            closeButton.ClickEx();
        }

        /// <summary>
        /// Click 'StartStop' button
        /// </summary>
        public void ClickStartStopButton()
        {
            startStopButton.ClickEx();
        }

        /// <summary>
        /// Click 'Setting' button
        /// </summary>
        public void ClickSettingButton()
        {
            settingButton.ClickEx();
        }

        #endregion //Actions

        #region Get methods

        #endregion //Get methods

        #endregion //Basic methods

        #region Business methods

        /// <summary>
        /// Select a tab with a specific name
        /// </summary>
        /// <param name="name"></param>
        public void SelectTab(string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get all item list of a specific tab name
        /// </summary>
        /// <param name="tabName"></param>
        /// <returns></returns>
        public string GetItemsList(string tabName)
        {
            throw new NotImplementedException();
        }

        #endregion //Business methods
    }
}
