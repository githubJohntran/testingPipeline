using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using System;
using System.Linq;
using StreetlightVision.Utilities;
using StreetlightVision.Extensions;
using System.Collections.Generic;
using StreetlightVision.Models;
using System.Drawing;

namespace StreetlightVision.Pages.UI
{
    public class AppBar
    {
        #region Variables

        private Dictionary<string, Type> _dicApps = new Dictionary<string, Type>();

        private PageBase _page;
        private IWebDriver _driver;

        #endregion //Variables

        #region IWebElements

        [FindsBy(How = How.CssSelector, Using = "[id='desktop-header-bartop'")]
        private IWebElement headerBartop;

        [FindsBy(How = How.CssSelector, Using = "[id='desktop-header-appbound'] .slv-header-logo")]
        private IWebElement logo;

        [FindsBy(How = How.CssSelector, Using = "[id='desktop-header-appname']")]
        private IWebElement appNameText;

        [FindsBy(How = How.CssSelector, Using = "[id='desktop-header-username']")]
        private IWebElement userNameText;

        [FindsBy(How = How.CssSelector, Using = "[id='headerButtonsCanvas']")]
        private IWebElement headerButtonsCanvas;

        [FindsBy(How = How.CssSelector, Using = "[id='switchButtonCanvas']")]
        private IWebElement switchButtonCanvas;

        [FindsBy(How = How.CssSelector, Using = "[id='switcherCanvas']")]
        private IWebElement appSwitcherPanel;

        #endregion //IWebElements

        #region Constructor

        public AppBar(IWebDriver driver, PageBase page)
        {
            _driver = driver;
            _page = page;

            PageFactory.InitElements(this, new RetryingElementLocator(driver, TimeSpan.FromMilliseconds(Settings.LocatorRetryingTimeout), TimeSpan.FromMilliseconds(Settings.LocatorPollingInterval)));
            InitPageMapping();
        }

        #endregion //Constructor

        #region Basic methods

        #region Actions        

        /// <summary>
        ///  Click on Application button
        /// </summary>
        public void ClickApplicationsButton()
        {
            var canvasWidth = headerButtonsCanvas.Size.Width;
            var canvasHeight = headerButtonsCanvas.Size.Height;
            headerButtonsCanvas.MoveToAndClick(canvasWidth - canvasHeight * 2, canvasHeight / 2);

            Wait.ForElementDisplayed(By.CssSelector("[id='slv-view-desktop-footer']"));
            Wait.ForSeconds(2);
        }        

        /// <summary>
        /// Click on Setting button
        /// </summary>
        public void ClickSettingsButton()
        {
            var canvasWidth = headerButtonsCanvas.Size.Width;
            var canvasHeight = headerButtonsCanvas.Size.Height;
            headerButtonsCanvas.MoveToAndClick(canvasWidth - canvasHeight / 2, canvasHeight / 2);
        }

        /// <summary>
        /// Click on middle of header bartop
        /// </summary>
        public void ClickHeaderBartop()
        {
            headerBartop.MoveToAndClick(headerBartop.Size.Width / 2, headerBartop.Size.Height / 2);
        }

        #endregion //Actions

        #region Get methods

        public string GetUserName()
        {
            throw new NotImplementedException();
        }

        #endregion //Get methods

        #endregion //Basic methods

        #region Business methods

        private void InitPageMapping()
        {
            _dicApps.Add(App.AdvancedSearch, typeof(AdvancedSearchPage));
            _dicApps.Add(App.AlarmManager, typeof(AlarmManagerPage));
            _dicApps.Add(App.Alarms, typeof(AlarmsPage));
            _dicApps.Add(App.BatchControl, typeof(BatchControlPage));
            _dicApps.Add(App.ControlCenter, typeof(ControlCenterlPage));
            _dicApps.Add(App.Dashboard, typeof(DashboardPage));
            _dicApps.Add(App.DataHistory, typeof(DataHistoryPage));
            _dicApps.Add(App.DeviceLifetime, typeof(DeviceLifetimePage));
            _dicApps.Add(App.DeviceHistory, typeof(DeviceHistoryPage));
            _dicApps.Add(App.Energy, typeof(EnergyPage));
            _dicApps.Add(App.EquipmentInventory, typeof(EquipmentInventoryPage));
            _dicApps.Add(App.FailureAnalysis, typeof(FailureAnalysisPage));
            _dicApps.Add(App.FailureTracking, typeof(FailureTrackingPage));
            _dicApps.Add(App.Installation, typeof(InstallationPage));
            _dicApps.Add(App.InventoryVerification, typeof(InventoryVerificationPage));
            _dicApps.Add(App.LogViewer, typeof(LogViewerPage));
            _dicApps.Add(App.MonthlyEnergySaving, typeof(MonthlyEnergySavingsPage));
            _dicApps.Add(App.RealTimeControl, typeof(RealTimeControlPage));
            _dicApps.Add(App.ReportManager, typeof(ReportManagerPage));
            _dicApps.Add(App.SchedulingManager, typeof(SchedulingManagerPage));
            _dicApps.Add(App.Users, typeof(UsersPage));
            _dicApps.Add(App.WorkOrders, typeof(WorkOrdersPage));
        }

        private void ShowSwitcher()
        {
            switchButtonCanvas.ClickEx();
            Wait.ForElementStyle(appSwitcherPanel, "background-color: rgba(204, 204, 204, 0.66");
            Wait.ForSeconds(4);
        }

        private void HideSwitcher()
        {
            ClickHeaderBartop();
            Wait.ForElementStyle(appSwitcherPanel, "background-color: rgba(255, 255, 255, 0");
            Wait.ForSeconds(2);
        }

        /// <summary>
        /// Switch to specific app
        /// </summary>
        /// <param name="appName"></param>
        public PageBase SwitchTo(string appName, string language = "English")
        {
            if (!_dicApps.Any()) InitPageMapping();
            var appsSwitcher = SLVHelper.GetListOfSwitcherApps();
            ShowSwitcher();

            // List to contain clicked point of apps
            var appPoints = new List<Point>();

            var appSwitcherPanelInfo = new
            {
                Left = appSwitcherPanel.Location.X,
                Top = appSwitcherPanel.Location.Y,
                SwitcherWidth = appSwitcherPanel.Size.Width,
                SwitcherHeight = appSwitcherPanel.Size.Height,
                PaddingLeft = 8,
                PaddingRight = 8,
                PaddingTop = 36,
                PaddingBottom = 8,
                AppWidth = 105,
                AppHeight = 79
            };

            var numberOfRowsAsFloat = (float)(appSwitcherPanelInfo.SwitcherHeight - appSwitcherPanelInfo.PaddingTop - appSwitcherPanelInfo.PaddingBottom) / appSwitcherPanelInfo.AppHeight;
            var numberOfColumnsAsFloat = (float)(appSwitcherPanelInfo.SwitcherWidth - appSwitcherPanelInfo.PaddingLeft - appSwitcherPanelInfo.PaddingRight) / appSwitcherPanelInfo.AppWidth;

            var numberOfRows = Math.Round(numberOfRowsAsFloat);
            var numberOfColumns = Math.Round(numberOfColumnsAsFloat);

            for (var i = 1; i <= numberOfRows; i++)
            {
                for (var j = 1; j <= numberOfColumns; j++)
                {
                    var x = appSwitcherPanelInfo.Left + appSwitcherPanelInfo.PaddingLeft + appSwitcherPanelInfo.AppWidth * j;
                    var y = appSwitcherPanelInfo.Top + appSwitcherPanelInfo.PaddingTop + appSwitcherPanelInfo.AppHeight * i;

                    var clickedX = x - appSwitcherPanelInfo.AppWidth / 2;
                    var clickedY = y - appSwitcherPanelInfo.AppHeight / 2;

                    if (appPoints.Count <= appsSwitcher.Count)
                    {
                        appPoints.Add(new Point(clickedX, clickedY));
                    }
                }
            }

            var appIndex = appsSwitcher.IndexOf(SLVHelper.ConvertAppName(appName, language));
            var appPoint = appPoints.ElementAt(appIndex);
            appSwitcherPanel.MoveToAndClick(appPoint.X, appPoint.Y);
            WaitForSwitcherPanelDisappeared();           

            return (PageBase)Activator.CreateInstance(_dicApps[appName], _driver);
        }

        /// <summary>
        /// Get current app displays on Switcher
        /// </summary>
        /// <returns></returns>
        public string GetCurrentAppSwitcher()
        {
            ShowSwitcher();
            HideSwitcher();
            return SLVHelper.GetCurrentAppSwitcher();
        }

        public void WaitForSwitcherPanelDisappeared()
        {
            Wait.ForElementStyle(appSwitcherPanel, "display: none");
        }
        
        #endregion //Business methods
    }
}
