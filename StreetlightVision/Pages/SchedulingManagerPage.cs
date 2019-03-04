using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using System;
using StreetlightVision.Extensions;
using StreetlightVision.Pages.UI;
using System.Threading;
using StreetlightVision.Utilities;

namespace StreetlightVision.Pages
{
    public class SchedulingManagerPage : PageBase
    {
        #region Variables

        private SchedulingManagerPanel _schedulingManagerPanel;
        private CommissionPopupPanel _commissionPopupPanel;        
        private ControlProgramEditorPanel _controlProgramEditorPanel;
        private ControlProgramItemsPopupPanel _controlProgramItemsPopupPanel;
        private ControlProgramCommandTypePopupPanel _controlProgramCommandTypePopupPanel;
        private CalendarEditorPanel _calendarEditorPanel;
        private CalendarEditorItemsPopupPanel _calendarEditorItemsPopupPanel;
        private CalendarControlProgramsPopupPanel _calendarControlProgramsPopupPanel;
        private GridPanel _gridPanel;

        #endregion //Variables

        #region IWebElements        

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-desktop-main'] .slv-panel-driven-layout-show-left-panel-button")]
        private IWebElement openButton;

        #endregion //IWebElements

        #region Constructor

        public SchedulingManagerPage(IWebDriver driver)
            : base(driver)                                                                                                                                                                                                                                                                                                                                                                             
        {
            PageFactory.InitElements(this, new RetryingElementLocator(driver, TimeSpan.FromMilliseconds(Settings.LocatorRetryingTimeout), TimeSpan.FromMilliseconds(Settings.LocatorPollingInterval)));

            WaitForPageReady();
        }

        #endregion //Constructor

        #region Properties

        public SchedulingManagerPanel SchedulingManagerPanel
        {
            get
            {
                if (_schedulingManagerPanel == null)
                {
                    _schedulingManagerPanel = new SchedulingManagerPanel(this.Driver, this);
                }

                return _schedulingManagerPanel;
            }
        }        

        public CommissionPopupPanel CommissionPopupPanel
        {
            get
            {
                if (_commissionPopupPanel == null)
                {
                    _commissionPopupPanel = new CommissionPopupPanel(this.Driver, this);
                }

                return _commissionPopupPanel;
            }
        }

        public ControlProgramEditorPanel ControlProgramEditorPanel
        {
            get
            {
                if (_controlProgramEditorPanel == null)
                {
                    _controlProgramEditorPanel = new ControlProgramEditorPanel(this.Driver, this);
                }

                return _controlProgramEditorPanel;
            }            
        }

        public ControlProgramItemsPopupPanel ControlProgramItemsPopupPanel
        {
            get
            {
                if (_controlProgramItemsPopupPanel == null)
                {
                    _controlProgramItemsPopupPanel = new ControlProgramItemsPopupPanel(this.Driver, this);
                }

                return _controlProgramItemsPopupPanel;
            }
        }

        public ControlProgramCommandTypePopupPanel ControlProgramCommandTypePopupPanel
        {
            get
            {
                if (_controlProgramCommandTypePopupPanel == null)
                {
                    _controlProgramCommandTypePopupPanel = new ControlProgramCommandTypePopupPanel(this.Driver, this);
                }

                return _controlProgramCommandTypePopupPanel;
            }
        }

        public CalendarEditorPanel CalendarEditorPanel
        {
            get
            {
                if (_calendarEditorPanel == null)
                {
                    _calendarEditorPanel = new CalendarEditorPanel(this.Driver, this);
                }

                return _calendarEditorPanel;
            }
        }

        public CalendarEditorItemsPopupPanel CalendarEditorItemsPopupPanel
        {
            get
            {
                if (_calendarEditorItemsPopupPanel == null)
                {
                    _calendarEditorItemsPopupPanel = new CalendarEditorItemsPopupPanel(this.Driver, this);
                }

                return _calendarEditorItemsPopupPanel;
            }
        }

        public CalendarControlProgramsPopupPanel CalendarControlProgramsPopupPanel
        {
            get
            {
                if (_calendarControlProgramsPopupPanel == null)
                {
                    _calendarControlProgramsPopupPanel = new CalendarControlProgramsPopupPanel(this.Driver, this);
                }

                return _calendarControlProgramsPopupPanel;
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

        #endregion //Properties

        #region Basic methods

        #region Actions

        /// <summary>
        /// Click 'Open' button
        /// </summary>
        public void ClickOpenButton()
        {
            openButton.ClickEx();
        }

        #endregion //Actions

        #region Get methods

        #endregion //Get methods

        #endregion //Basic methods

        #region Business methods

        public override void WaitForPopupDialogDisplayed()
        {
            Wait.ForElementDisplayed(By.CssSelector("[id='w2ui-popup']"));
            Wait.ForSeconds(2);
        }

        public override void WaitForPopupMessageDialogDisplayed()
        {
            Wait.ForElementDisplayed(By.CssSelector("[id='w2ui-popup'] [id^='w2ui-message']"));
            Wait.ForSeconds(2);
        }

        public void WaitForControlProgramDisplayed()
        {
            Wait.ForElementStyle(By.CssSelector("[id$='panels_panel_main']"), "left: 380px");
        }

        public bool HasPopupDialogDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id='w2ui-popup']"));
        }

        public void DeleteCalendar(string name)
        {
            SchedulingManagerPanel.SelectCalendar(name);
            WaitForPreviousActionComplete();
            SchedulingManagerPanel.ClickDeleteCalendarButton();
            WaitForPopupDialogDisplayed();
            if (Dialog.GetDialogTitleText().Equals("Confirmation"))
            {
                Dialog.ClickYesButton();
                WaitForPreviousActionComplete();
                WaitForPopupDialogDisappeared();
            }
            else if (Dialog.GetDialogTitleText().Equals("Notification"))
            {
                Dialog.ClickOkButton();
                WaitForPopupDialogDisappeared();
            }
        }

        public void DeleteControlProgram(string name, string geozoneName = "")
        {
            if (string.IsNullOrEmpty(geozoneName))
                SchedulingManagerPanel.SelectControlProgram(name);
            else
                SchedulingManagerPanel.SelectControlProgram(name, geozoneName);
            WaitForPreviousActionComplete();
            SchedulingManagerPanel.ClickDeleteControlProgramButton();
            WaitForPopupDialogDisplayed();
            if (Dialog.GetDialogTitleText().Equals("Confirmation"))
            {
                Dialog.ClickYesButton();
                WaitForPreviousActionComplete();
                WaitForPopupDialogDisappeared();
            }
            else if (Dialog.GetDialogTitleText().Equals("Notification"))
            {
                Dialog.ClickOkButton();
                WaitForPopupDialogDisappeared();
            }
        }

        public bool IsControlProgramEditorDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='main-panel'] > div:nth-child(1) .schedulermanager-editor-title"));
        }

        public bool IsCalendarEditorDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='main-panel'] > div:nth-child(2) .schedulermanager-editor-title"));
        }

        public bool IsCenterLoaderDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='schedulermanager-loader']"));
        }

        #endregion //Business methods

        protected override void WaitForPageReady()
        {
            base.WaitForPageReady();            
            SchedulingManagerPanel.WaitForPanelLoaded();
            ControlProgramEditorPanel.WaitForPanelLoaded();
        }
    }
}
