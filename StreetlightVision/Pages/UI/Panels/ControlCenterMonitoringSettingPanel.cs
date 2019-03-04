using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using StreetlightVision.Extensions;
using System;

namespace StreetlightVision.Pages.UI
{
    public class ControlCenterMonitoringSettingPanel : DeviceWidgetPanelBase
    {
        #region Variables

        #endregion //Variables

        #region IWebElements        

        [FindsBy(How = How.CssSelector, Using = "[id='ControlCenter-controlcenter-editor-monitoring-settings'] [id='tb_controlcenterMonitoringButtons_item_close'] >  table")]
        private IWebElement backButton;

        [FindsBy(How = How.CssSelector, Using = "[id='ControlCenter-controlcenter-editor-monitoring-settings'] .controlcenter-editor-title-label")]
        private IWebElement panelTitle;

        [FindsBy(How = How.CssSelector, Using = "[id='ControlCenter-controlcenter-editor-monitoring-settings-commands-label']")]
        private IWebElement commandsCaptionLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='ControlCenter-controlcenter-editor-monitoring-settings-command-ping-title']")]
        private IWebElement connectSegmentControllerLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='ControlCenter-controlcenter-editor-monitoring-settings-command-ping-checkbox']")]
        private IWebElement connectSegmentControllerCheckbox;

        [FindsBy(How = How.CssSelector, Using = "[id='ControlCenter-controlcenter-editor-monitoring-settings-command-ping-icon']")]
        private IWebElement connectSegmentControllerIcon;

        [FindsBy(How = How.CssSelector, Using = "[id='ControlCenter-controlcenter-editor-monitoring-settings-command-clock-title']")]
        private IWebElement checkSegmentControllerClockLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='ControlCenter-controlcenter-editor-monitoring-settings-command-clock-checkbox']")]
        private IWebElement checkSegmentControllerClockCheckbox;

        [FindsBy(How = How.CssSelector, Using = "[id='ControlCenter-controlcenter-editor-monitoring-settings-command-clock-icon']")]
        private IWebElement checkSegmentControllerClockIcon;

        [FindsBy(How = How.CssSelector, Using = "[id='ControlCenter-controlcenter-editor-monitoring-settings-command-failures-checkbox']")]
        private IWebElement checkCriticalFailuresLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='ControlCenter-controlcenter-editor-monitoring-settings-command-failures-checkbox']")]
        private IWebElement checkCriticalFailuresCheckbox;

        [FindsBy(How = How.CssSelector, Using = "[id='ControlCenter-controlcenter-editor-monitoring-settings-command-failures-icon']")]
        private IWebElement checkCriticalFailuresIcon;

        [FindsBy(How = How.CssSelector, Using = "[id='ControlCenter-controlcenter-editor-monitoring-settings-command-warning-title']")]
        private IWebElement checkWarningsLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='ControlCenter-controlcenter-editor-monitoring-settings-command-warning-checkbox']")]
        private IWebElement checkWarningsCheckbox;

        [FindsBy(How = How.CssSelector, Using = "[id='ControlCenter-controlcenter-editor-monitoring-settings-command-warning-icon']")]
        private IWebElement checkWarningsIcon;
                
        [FindsBy(How = How.CssSelector, Using = "[id='ControlCenter-controlcenter-editor-monitoring-settings-parameters-label']")]
        private IWebElement parametersCaptionLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='ControlCenter-controlcenter-editor-monitoring-settings-simultaneousGeozone-label']")]
        private IWebElement simultaneousGeozoneLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='ControlCenter-controlcenter-editor-monitoring-settings-simultaneousGeozone-field']")]
        private IWebElement simultaneousGeozoneNumbericInput;

        [FindsBy(How = How.CssSelector, Using = "[id='ControlCenter-controlcenter-editor-monitoring-settings-refreshRate-label']")]
        private IWebElement refreshRateLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='ControlCenter-controlcenter-editor-monitoring-settings-refreshRate-field']")]
        private IWebElement refreshRateNumbericInput;

        [FindsBy(How = How.CssSelector, Using = "[id='ControlCenter-controlcenter-editor-monitoring-settings-alertSound-title']")]
        private IWebElement alertSoundLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='ControlCenter-controlcenter-editor-monitoring-settings-alertSound-checkbox']")]
        private IWebElement alertSoundCheckbox;

        #endregion //IWebElements

        #region Constructor

        public ControlCenterMonitoringSettingPanel(IWebDriver driver, PageBase page) : base(driver, page)
        {
            PageFactory.InitElements(this, new RetryingElementLocator(driver, TimeSpan.FromMilliseconds(Settings.LocatorRetryingTimeout), TimeSpan.FromMilliseconds(Settings.LocatorPollingInterval)));
        }

        #endregion //Constructor

        #region Basic methods

        #region Actions

        /// <summary>
        /// Click 'Back' button
        /// </summary>
        public void ClickBackButton()
        {
            backButton.ClickEx();
        }

        /// <summary>
        /// Tick 'ConnectSegmentController' checkbox
        /// </summary>
        /// <param name="value"></param>
        public void TickConnectSegmentControllerCheckbox(bool value)
        {
            connectSegmentControllerCheckbox.Check(value);
        }

        /// <summary>
        /// Tick 'CheckSegmentControllerClock' checkbox
        /// </summary>
        /// <param name="value"></param>
        public void TickCheckSegmentControllerClockCheckbox(bool value)
        {
            checkSegmentControllerClockCheckbox.Check(value);
        }

        /// <summary>
        /// Tick 'CheckCriticalFailures' checkbox
        /// </summary>
        /// <param name="value"></param>
        public void TickCheckCriticalFailuresCheckbox(bool value)
        {
            checkCriticalFailuresCheckbox.Check(value);
        }

        /// <summary>
        /// Tick 'CheckWarnings' checkbox
        /// </summary>
        /// <param name="value"></param>
        public void TickCheckWarningsCheckbox(bool value)
        {
            checkWarningsCheckbox.Check(value);
        }

        /// <summary>
        /// Enter a value for 'SimultaneousGeozoneNumberic' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterSimultaneousGeozoneNumbericInput(string value)
        {
            simultaneousGeozoneNumbericInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'RefreshRateNumberic' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterRefreshRateNumbericInput(string value)
        {
            refreshRateNumbericInput.Enter(value);
        }

        /// <summary>
        /// Tick 'AlertSound' checkbox
        /// </summary>
        /// <param name="value"></param>
        public void TickAlertSoundCheckbox(bool value)
        {
            alertSoundCheckbox.Check(value);
        }

        #endregion //Actions

        #region Get methods

        /// <summary>
        /// Get 'PanelTitle' text
        /// </summary>
        /// <returns></returns>
        public string GetPanelTitleText()
        {
            return panelTitle.Text;
        }

        /// <summary>
        /// Get 'CommandsCaption' label text
        /// </summary>
        /// <returns></returns>
        public string GetCommandsCaptionText()
        {
            return commandsCaptionLabel.Text;
        }

        /// <summary>
        /// Get 'ConnectSegmentController' label text
        /// </summary>
        /// <returns></returns>
        public string GetConnectSegmentControllerText()
        {
            return connectSegmentControllerLabel.Text;
        }

        /// <summary>
        /// Get 'ConnectSegmentController' checkbox value
        /// </summary>
        /// <returns></returns>
        public bool GetConnectSegmentControllerValue()
        {
            return connectSegmentControllerCheckbox.Selected;
        }

        /// <summary>
        /// Get 'ConnectSegmentControllerIcon' input value
        /// </summary>
        /// <returns></returns>
        public string GetConnectSegmentControllerIconValue()
        {
            return connectSegmentControllerIcon.IconValue();
        }

        /// <summary>
        /// Get 'CheckSegmentControllerClock' label text
        /// </summary>
        /// <returns></returns>
        public string GetCheckSegmentControllerClockText()
        {
            return checkSegmentControllerClockLabel.Text;
        }

        /// <summary>
        /// Get 'CheckSegmentControllerClock' checkbox value
        /// </summary>
        /// <returns></returns>
        public bool GetCheckSegmentControllerClockValue()
        {
            return checkSegmentControllerClockCheckbox.Selected;
        }

        /// <summary>
        /// Get 'CheckSegmentControllerClockIcon' input value
        /// </summary>
        /// <returns></returns>
        public string GetCheckSegmentControllerClockIconValue()
        {
            return checkSegmentControllerClockIcon.IconValue();
        }

        /// <summary>
        /// Get 'CheckCriticalFailures' label text
        /// </summary>
        /// <returns></returns>
        public string GetCheckCriticalFailuresText()
        {
            return checkCriticalFailuresLabel.Text;
        }

        /// <summary>
        /// Get 'CheckCriticalFailures' checkbox value
        /// </summary>
        /// <returns></returns>
        public bool GetCheckCriticalFailuresValue()
        {
            return checkCriticalFailuresCheckbox.Selected;
        }

        /// <summary>
        /// Get 'CheckCriticalFailuresIcon' input value
        /// </summary>
        /// <returns></returns>
        public string GetCheckCriticalFailuresIconValue()
        {
            return checkCriticalFailuresIcon.IconValue();
        }

        /// <summary>
        /// Get 'CheckWarnings' label text
        /// </summary>
        /// <returns></returns>
        public string GetCheckWarningsText()
        {
            return checkWarningsLabel.Text;
        }

        /// <summary>
        /// Get 'CheckWarnings' checkbox value
        /// </summary>
        /// <returns></returns>
        public bool GetCheckWarningsValue()
        {
            return checkWarningsCheckbox.Selected;
        }

        /// <summary>
        /// Get 'CheckWarningsIcon' input value
        /// </summary>
        /// <returns></returns>
        public string GetCheckWarningsIconValue()
        {
            return checkWarningsIcon.IconValue();
        }

        /// <summary>
        /// Get 'ParametersCaption' label text
        /// </summary>
        /// <returns></returns>
        public string GetParametersCaptionText()
        {
            return parametersCaptionLabel.Text;
        }

        /// <summary>
        /// Get 'SimultaneousGeozone' label text
        /// </summary>
        /// <returns></returns>
        public string GetSimultaneousGeozoneText()
        {
            return simultaneousGeozoneLabel.Text;
        }

        /// <summary>
        /// Get 'SimultaneousGeozoneNumberic' input value
        /// </summary>
        /// <returns></returns>
        public string GetSimultaneousGeozoneNumbericValue()
        {
            return simultaneousGeozoneNumbericInput.Value();
        }

        /// <summary>
        /// Get 'RefreshRate' label text
        /// </summary>
        /// <returns></returns>
        public string GetRefreshRateText()
        {
            return refreshRateLabel.Text;
        }

        /// <summary>
        /// Get 'RefreshRateNumberic' input value
        /// </summary>
        /// <returns></returns>
        public string GetRefreshRateNumbericValue()
        {
            return refreshRateNumbericInput.Value();
        }

        /// <summary>
        /// Get 'AlertSound' label text
        /// </summary>
        /// <returns></returns>
        public string GetAlertSoundText()
        {
            return alertSoundLabel.Text;
        }

        /// <summary>
        /// Get 'AlertSound' checkbox value
        /// </summary>
        /// <returns></returns>
        public bool GetAlertSoundValue()
        {
            return alertSoundCheckbox.Selected;
        }

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
