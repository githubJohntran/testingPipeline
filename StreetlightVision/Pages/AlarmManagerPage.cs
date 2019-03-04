using OpenQA.Selenium;
using System;
using StreetlightVision.Extensions;
using StreetlightVision.Pages.UI;
using StreetlightVision.Utilities;
using System.Threading;
using SeleniumExtras.PageObjects;

namespace StreetlightVision.Pages
{
    public class AlarmManagerPage : PageBase
    {
        #region Variables

        private GeozoneTreeMainPanel _geozoneTreeMainPanel;
        private GridPanel _gridPanel;
        private AlarmEditorPanel _alarmEditorPanel;

        #endregion //Variables

        #region IWebElements

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-desktop-main'] [id$='browser-show-button']")]
        private IWebElement showButton;

        #endregion //IWebElements

        #region Constructor

        public AlarmManagerPage(IWebDriver driver)
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

        public AlarmEditorPanel AlarmEditorPanel
        {
            get
            {
                if (_alarmEditorPanel == null)
                {
                    _alarmEditorPanel = new AlarmEditorPanel(this.Driver, this);
                }

                return _alarmEditorPanel;
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

        /// <summary>
        /// Create an device alarm - no data received
        /// </summary>
        /// <param name="alarmName"></param>
        /// <param name="deviceName"></param>
        /// <param name="refreshRate"></param>
        /// <param name="toEmail"></param>
        public void CreateDeviceAlarmNoDataReceived(string alarmName, string deviceName, string refreshRate, string toEmail = "")
        {
            GridPanel.ClickAddAlarmToolbarButton();
            WaitForPreviousActionComplete();
            AlarmEditorPanel.EnterNameInput(alarmName);
            AlarmEditorPanel.SelectTypeDropDown("Device alarm: No data received");
            WaitForPreviousActionComplete();
            AlarmEditorPanel.SelectTab("General");
            AlarmEditorPanel.TickNewAlarmIfRetriggerCheckbox(true);
            AlarmEditorPanel.TickAutoAcknowledgeCheckbox(true);
            AlarmEditorPanel.SelectRefreshRateDropDown(refreshRate);
            AlarmEditorPanel.SelectTab("Trigger condition");
            AlarmEditorPanel.SelectDevicesListDropDown(deviceName);
            if (!string.IsNullOrEmpty(toEmail))
            {
                AlarmEditorPanel.SelectTab("Actions");
                AlarmEditorPanel.EnterFromInput("qa@streetlightmonitoring.com");
                AlarmEditorPanel.SelectToListDropDown(toEmail);
                AlarmEditorPanel.EnterSubjectInput(alarmName);
            }
            AlarmEditorPanel.ClickSaveButton();
            WaitForPreviousActionComplete();
        }

        /// <summary>
        /// Create an controller alarm  - no data received
        /// </summary>
        /// <param name="alarmName"></param>
        /// <param name="alarmType"></param>
        /// <param name="controllerName"></param>
        /// <param name="refreshRate"></param>
        /// <param name="toEmail"></param>
        public void CreateControllerAlarmNoDataReceived(string alarmName, string controllerName, string refreshRate, string toEmail = "")
        {
            GridPanel.ClickAddAlarmToolbarButton();
            WaitForPreviousActionComplete();
            AlarmEditorPanel.EnterNameInput(alarmName);
            AlarmEditorPanel.SelectTypeDropDown("Controller alarm: No data received");
            WaitForPreviousActionComplete();
            AlarmEditorPanel.SelectTab("General");
            AlarmEditorPanel.TickNewAlarmIfRetriggerCheckbox(true);
            AlarmEditorPanel.TickAutoAcknowledgeCheckbox(true);
            AlarmEditorPanel.SelectRefreshRateDropDown(refreshRate);
            AlarmEditorPanel.SelectTab("Trigger condition");
            AlarmEditorPanel.SelectControllersListDropDown(controllerName);
            if (!string.IsNullOrEmpty(toEmail))
            {
                AlarmEditorPanel.SelectTab("Actions");
                AlarmEditorPanel.EnterFromInput("qa@streetlightmonitoring.com");
                AlarmEditorPanel.SelectToListDropDown(toEmail);
                AlarmEditorPanel.EnterSubjectInput(alarmName);
            }

            AlarmEditorPanel.ClickSaveButton();
            WaitForPreviousActionComplete();
        }

        /// <summary>
        /// Select and click remove button on Alarm Details panel. If confirmed is true, Yes in Confirmation dialog is clicked, otherwise, No is clicked
        /// </summary>
        /// <param name="alarmName"></param>
        /// <param name="confirmed"></param>
        public void DeleteAlarm(string alarmName, bool confirmed = true)
        {
            if (!IsAlarmEditorPanelDisplayed())
            {
                GridPanel.ClickGridRecord(alarmName);
                WaitForPreviousActionComplete();
                AlarmEditorPanel.WaitForPanelLoaded();
            }

            AlarmEditorPanel.ClickDeleteButton();
            Dialog.WaitForPanelLoaded();
            if (confirmed)
            {
                Dialog.ClickYesButton();
                WaitForPreviousActionComplete();
            }
            else
            {
                Dialog.ClickNoButton();
            }
        }

        /// <summary>
        /// Wait for a mail with specific subject found in inbox
        /// </summary>
        /// <param name="emailSubject"></param>
        public void WaitForMailSubject(string emailSubject)
        {
            WebDriverContext.Wait.Until(driver => EmailUtility.GetNewEmail(emailSubject) != null);
        }

        /// <summary>
        /// Wait until Open file dialog completely closed
        /// </summary>
        public void WaitUntilOpenFileDialogDisappears()
        {
            if (Browser.Name.Equals("IE", StringComparison.InvariantCultureIgnoreCase))
            {
                Wait.ForSeconds(2); // This LOC is needed only for IE to avoid "Modal dialog present" exception                
            }
        }

        public void WaitForPanelRightDisplayed()
        {
            Wait.ForElementDisplayed(By.CssSelector("[id$='alarmmanager-geozone_panel_right']"));
        }

        public void WaitForPanelRightDisappeared()
        {
            Wait.ForElementInvisible(By.CssSelector("[id$='alarmmanager-geozone_panel_right']"));
        }

        public bool IsAlarmEditorPanelDisplayed()
        {
            return Driver.FindElements(By.CssSelector("[id$='alarmmanager-geozone_panel_right'][style*='display: block']")).Count > 0;
        }

        #endregion //Business methods

        protected override void WaitForPageReady()
        {
            Wait.ForSeconds(2);
        }
    }
}
