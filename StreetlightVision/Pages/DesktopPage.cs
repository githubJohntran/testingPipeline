using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using StreetlightVision.Extensions;
using StreetlightVision.Models;
using StreetlightVision.Utilities;
using System;
using System.Collections.Generic;
using System.Threading;
using StreetlightVision.Pages.UI;
using System.Linq;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Reflection;
using NUnit.Framework;
using System.IO;

namespace StreetlightVision.Pages
{
    public class DesktopPage : PageBase
    {
        #region Variables

        private Dictionary<string, Tuple<Type, IWebElement>> _dicApps = new Dictionary<string, Tuple<Type, IWebElement>>();
        private Dictionary<string, IList<IWebElement>> _dicWidgets = new Dictionary<string, IList<IWebElement>>();

        private GeozoneTreeWidgetPanel _geozoneTreeWidgetPanel;
        private WeatherLocationPanel _weatherLocationPanel;

        #endregion //Variables

        #region IWebElements

        [FindsBy(How = How.CssSelector, Using = "script[src*='/groundcontrol/js/lib/JQuery/globalize/cultures/globalize.culture']")]
        private IWebElement currentLanguageScript;

        [FindsBy(How = How.CssSelector, Using = "link[href*='/groundcontrol/skins/'][href$='css/style.min.css']")]
        private IWebElement currentSkinCss;

        #region Apps

        [FindsBy(How = How.CssSelector, Using = "[id$='customreport-tile-content']")]
        private IWebElement advancedSearchTile;

        [FindsBy(How = How.CssSelector, Using = "[id$='alarmmanager-tile-content']")]
        private IWebElement alarmManagerTile;

        [FindsBy(How = How.CssSelector, Using = "[id$='alarms-tile-content']")]
        private IWebElement alarmsTile;

        [FindsBy(How = How.CssSelector, Using = "[id$='realtimebatch-tile-content']")]
        private IWebElement batchControlTile;

        [FindsBy(How = How.CssSelector, Using = "[id$='controlcenter-tile-content']")]
        private IWebElement controlCenterTile;

        [FindsBy(How = How.CssSelector, Using = "[id$='dashboard-tile-content']")]
        private IWebElement dashboardTile;

        [FindsBy(How = How.CssSelector, Using = "[id$='datahistory-tile-content']")]
        private IWebElement dataHistoryTile;

        [FindsBy(How = How.CssSelector, Using = "[id$='devicehistory-tile-content']")]
        private IWebElement deviceHistoryTile;

        [FindsBy(How = How.CssSelector, Using = "[id$='lifetime-tile-content']")]
        private IWebElement deviceLifetimeTile;

        [FindsBy(How = How.CssSelector, Using = "[id$='energy-tile-content']")]
        private IWebElement energyTile;

        [FindsBy(How = How.CssSelector, Using = "[id$='equipmentgl-tile-content']")]
        private IWebElement equipmentInventoryTile;

        [FindsBy(How = How.CssSelector, Using = "[id$='failure-tile-content']")]
        private IWebElement failureAnalysisTile;

        [FindsBy(How = How.CssSelector, Using = "[id$='failuretrackinggl-tile-content']")]
        private IWebElement failureTrackingTile;

        [FindsBy(How = How.CssSelector, Using = "[id$='failuretrackinggl-tile-content']")]
        private IWebElement failureTrackingPreviewTile;
        
        [FindsBy(How = How.CssSelector, Using = "[id$='installation-tile-content']")]
        private IWebElement installationTile;

        [FindsBy(How = How.CssSelector, Using = "[id$='presweep-tile-content']")]
        private IWebElement inventoryVerificationTile;

        [FindsBy(How = How.CssSelector, Using = "[id$='logviewer-tile-content']")]
        private IWebElement logViewerTile;

        [FindsBy(How = How.CssSelector, Using = "[id$='energysaving-tile-content']")]
        private IWebElement monthlyEnergySavingTile;

        [FindsBy(How = How.CssSelector, Using = "[id$='realtimegl-tile-content']")]
        private IWebElement realtimeControlTile;

        [FindsBy(How = How.CssSelector, Using = "[id$='reportmanager-tile-content']")]
        private IWebElement reportManagerTile;

        [FindsBy(How = How.CssSelector, Using = "[id$='schedulermanager-tile-content']")]
        private IWebElement schedulingManagerTile;

        [FindsBy(How = How.CssSelector, Using = "[id$='user-tile-content']")]
        private IWebElement usersTile;

        [FindsBy(How = How.CssSelector, Using = "[id$='workorder-tile-content']")]
        private IWebElement workOrdersTile;

        [FindsBy(How = How.CssSelector, Using = "[id$='tile-title'].tile-title")]
        private IList<IWebElement> appNamesList;

        [FindsBy(How = How.CssSelector, Using = "[id$='user-tile-content'] [id$='user-badge'] > div:nth-child(1)")]
        private IWebElement userFullNameLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='user-tile-content'] [id$='user-badge'] > div:nth-child(2)")]
        private IWebElement userProfileNameLabel;

        #endregion

        #region Widget

        [FindsBy(How = How.CssSelector, Using = "[id*='camera'].tile-background")]
        private IList<IWebElement> cameraTileList;

        [FindsBy(How = How.CssSelector, Using = "[id*='circuitormini'].tile-background")]
        private IList<IWebElement> circutorCVMMeterTileList;

        [FindsBy(How = How.CssSelector, Using = "[id*='clock'].tile-background")]
        private IList<IWebElement> clockTileList;

        [FindsBy(How = How.CssSelector, Using = "[id*='electricalcounter'].tile-background")]
        private IList<IWebElement> electricalMeterTileList;

        [FindsBy(How = How.CssSelector, Using = "[id*='controller'].tile-background")]
        private IList<IWebElement> gatewayTileList;

        [FindsBy(How = How.CssSelector, Using = "[id*='failures'].tile-background")]
        private IList<IWebElement> geozoneFailuresMonitorTileList;

        [FindsBy(How = How.CssSelector, Using = "[id*='iotrouterstatus'].tile-background")]
        private IList<IWebElement> ioTEdgeRouterStatusTileList;

        [FindsBy(How = How.CssSelector, Using = "[id*='streetlight'].tile-background")]
        private IList<IWebElement> luminaireControllerTileList;

        [FindsBy(How = How.CssSelector, Using = "[id*='pollutionsensor'].tile-background")]
        private IList<IWebElement> pollutionSensorWidgetTileList;

        [FindsBy(How = How.CssSelector, Using = "[id*='security'].tile-background")]
        private IList<IWebElement> securityWidgetTileList;

        [FindsBy(How = How.CssSelector, Using = "[id*='spoony'].tile-background")]
        private IList<IWebElement> spoonyWidgetTileList;

        [FindsBy(How = How.CssSelector, Using = "[id*='sunrisesunset'].tile-background")]
        private IList<IWebElement> sunriseSunsetTimesTileList;

        [FindsBy(How = How.CssSelector, Using = "[id*='vcs'].tile-background")]
        private IList<IWebElement> vehicleChargingStationTileList;

        [FindsBy(How = How.CssSelector, Using = "[id*='weather'].tile-background")]
        private IList<IWebElement> weatherTileList;

        [FindsBy(How = How.CssSelector, Using = "[id*='xcam'].tile-background")]
        private IList<IWebElement> xCamMonitorTileList;

        [FindsBy(How = How.CssSelector, Using = "[id*='environmentalsensor'].tile-background")]
        private IList<IWebElement> environmentalSensorTileList;

        #endregion

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-desktop-tiles']")]
        private IWebElement desktopTilesContainer;

        [FindsBy(How = How.CssSelector, Using = "[id='tb_slv-view-desktop-footer-mainToolbar_item_configuration'] > table")]
        private IWebElement footerCustomizeButton;

        [FindsBy(How = How.CssSelector, Using = "[id='tb_slv-view-desktop-footer-configToolbar_item_delete'] > table")]
        private IWebElement footerDeleteButton;

        [FindsBy(How = How.CssSelector, Using = "[id='tb_slv-view-desktop-footer-configToolbar_item_delete'] .w2ui-tb-caption")]
        private IWebElement footerDeleteCaptionLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='tb_slv-view-desktop-footer-configToolbar_item_close'] > table")]
        private IWebElement footerCloseButton;

        #endregion //IWebElements

        #region Constructor

        public DesktopPage(IWebDriver driver)
            : base(driver)
        {
            PageFactory.InitElements(this, new RetryingElementLocator(driver, TimeSpan.FromMilliseconds(Settings.LocatorRetryingTimeout), TimeSpan.FromMilliseconds(Settings.LocatorPollingInterval)));

            WaitForPageReady();
            InitAppMapping();
            InitWidgetMapping();
        }

        #endregion //Constructor

        #region Properties    

        public GeozoneTreeWidgetPanel GeozoneTreeWidgetPanel
        {
            get
            {
                if (_geozoneTreeWidgetPanel == null)
                {
                    _geozoneTreeWidgetPanel = new GeozoneTreeWidgetPanel(this.Driver, this);
                }

                return _geozoneTreeWidgetPanel;
            }
        }

        public WeatherLocationPanel WeatherLocationPanel
        {
            get
            {
                if (_weatherLocationPanel == null)
                {
                    _weatherLocationPanel = new WeatherLocationPanel(this.Driver, this);
                }

                return _weatherLocationPanel;
            }
        }

        #endregion //Properties

        #region Basic methods

        #region Actions
        /// <summary>
        /// Click 'FooterCustomize' button
        /// </summary>
        public void ClickFooterCustomizeButton()
        {
            footerCustomizeButton.ClickEx();
        }

        /// <summary>
        /// Click 'FooterDelete' button
        /// </summary>
        public void ClickFooterDeleteButton()
        {
            footerDeleteButton.ClickEx();
        }

        /// <summary>
        /// Click 'FooterClose' button
        /// </summary>
        public void ClickFooterCloseButton()
        {
            footerCloseButton.ClickEx();
        }

        #endregion //Actions

        #region Get methods       

        /// <summary>
        /// Get 'UserFullName' label text
        /// </summary>
        /// <returns></returns>
        public string GetUserFullNameText()
        {
            return userFullNameLabel.Text;
        }

        /// <summary>
        /// Get 'UserProfileName' label text
        /// </summary>
        /// <returns></returns>
        public string GetUserProfileNameText()
        {
            return userProfileNameLabel.Text;
        }

        /// <summary>
        /// Get 'FooterDeleteCaption' label text
        /// </summary>
        /// <returns></returns>
        public string GetFooterDeleteCaptionText()
        {
            return footerDeleteCaptionLabel.Text;
        }

        #endregion //Get methods

        #endregion //Basic methods

        #region Business methods

        #region Wait methods

        public void WaitForConfigToolbarDisplayed()
        {
            Wait.ForElementDisplayed(By.CssSelector("[id='slv-view-desktop-footer-configToolbar']"));            
        }

        public void WaitForConfigToolbarDisappeared()
        {
            Wait.ForElementInvisible(By.CssSelector("[id='slv-view-desktop-footer-configToolbar']"));
        }

        public void WaitForWeatherLocationDisplayed()
        {
            Wait.ForElementStyle(By.CssSelector("[id$='settings'][id*='weather'][style*='display: block']"), string.Format("left: {0}px", WebDriverContext.JsExecutor.ExecuteScript("return screen.availWidth - 350")));
        }

        public void WaitForWeatherLocationDisappeared()
        {
            Wait.ForElementStyle(By.CssSelector("[id$='settings'][id*='weather']"), string.Format("left: {0}px", WebDriverContext.JsExecutor.ExecuteScript("return screen.availWidth")));
        }

        public void WaitForWeatherLoaded()
        {
            Wait.ForElementInvisible(By.CssSelector("div.weather-spin"));
        }

        public void WaitForClockGeozoneTreeDisplayed()
        {
            Wait.ForElementStyle(By.CssSelector("[id$='settings'][id*='clock'][style*='display: block']"), string.Format("left: {0}px", WebDriverContext.JsExecutor.ExecuteScript("return screen.availWidth - 350")));
        }

        public void WaitForClockGeozoneTreeDisappeared()
        {
            Wait.ForElementStyle(By.CssSelector("[id$='settings'][id*='clock']"), string.Format("left: {0}px", WebDriverContext.JsExecutor.ExecuteScript("return screen.availWidth")));
        }

        public void WaitForSunriseSunsetGeozoneTreeDisplayed()
        {
            Wait.ForElementStyle(By.CssSelector("[id$='settings'][id*='sunrisesunset'][style*='display: block']"), string.Format("left: {0}px", WebDriverContext.JsExecutor.ExecuteScript("return screen.availWidth - 350")));
        }

        public void WaitForSunriseSunsetGeozoneTreeDisappeared()
        {
            Wait.ForElementStyle(By.CssSelector("[id$='settings'][id*='sunrisesunset']"), string.Format("left: {0}px", WebDriverContext.JsExecutor.ExecuteScript("return screen.availWidth")));
        }

        public void WaitForSunriseSunsetLoaded()
        {
            Wait.ForElementInvisible(By.CssSelector("div.sunrisesunset-loader"));
        }
        
        #endregion //Wait methods

        private void InitAppMapping()
        {
            _dicApps.Add(App.AdvancedSearch, new Tuple<Type, IWebElement>(typeof(AdvancedSearchPage), advancedSearchTile));
            _dicApps.Add(App.AlarmManager, new Tuple<Type, IWebElement>(typeof(AlarmManagerPage), alarmManagerTile));
            _dicApps.Add(App.Alarms, new Tuple<Type, IWebElement>(typeof(AlarmsPage), alarmsTile));
            _dicApps.Add(App.BatchControl, new Tuple<Type, IWebElement>(typeof(BatchControlPage), batchControlTile));
            _dicApps.Add(App.ControlCenter, new Tuple<Type, IWebElement>(typeof(ControlCenterlPage), controlCenterTile));
            _dicApps.Add(App.Dashboard, new Tuple<Type, IWebElement>(typeof(DashboardPage), dashboardTile));
            _dicApps.Add(App.DataHistory, new Tuple<Type, IWebElement>(typeof(DataHistoryPage), dataHistoryTile));
            _dicApps.Add(App.DeviceHistory, new Tuple<Type, IWebElement>(typeof(DeviceHistoryPage), deviceHistoryTile));
            _dicApps.Add(App.DeviceLifetime, new Tuple<Type, IWebElement>(typeof(DeviceLifetimePage), deviceLifetimeTile));
            _dicApps.Add(App.Energy, new Tuple<Type, IWebElement>(typeof(EnergyPage), energyTile));
            _dicApps.Add(App.EquipmentInventory, new Tuple<Type, IWebElement>(typeof(EquipmentInventoryPage), equipmentInventoryTile));
            _dicApps.Add(App.FailureAnalysis, new Tuple<Type, IWebElement>(typeof(FailureAnalysisPage), failureAnalysisTile));
            _dicApps.Add(App.FailureTracking, new Tuple<Type, IWebElement>(typeof(FailureTrackingPage), failureTrackingTile));
            _dicApps.Add(App.Installation, new Tuple<Type, IWebElement>(typeof(InstallationPage), installationTile));
            _dicApps.Add(App.InventoryVerification, new Tuple<Type, IWebElement>(typeof(InventoryVerificationPage), inventoryVerificationTile));
            _dicApps.Add(App.LogViewer, new Tuple<Type, IWebElement>(typeof(LogViewerPage), logViewerTile));
            _dicApps.Add(App.MonthlyEnergySaving, new Tuple<Type, IWebElement>(typeof(MonthlyEnergySavingsPage), monthlyEnergySavingTile));
            _dicApps.Add(App.RealTimeControl, new Tuple<Type, IWebElement>(typeof(RealTimeControlPage), realtimeControlTile));
            _dicApps.Add(App.ReportManager, new Tuple<Type, IWebElement>(typeof(ReportManagerPage), reportManagerTile));
            _dicApps.Add(App.SchedulingManager, new Tuple<Type, IWebElement>(typeof(SchedulingManagerPage), schedulingManagerTile));
            _dicApps.Add(App.Users, new Tuple<Type, IWebElement>(typeof(UsersPage), usersTile));
            _dicApps.Add(App.WorkOrders, new Tuple<Type, IWebElement>(typeof(WorkOrdersPage), workOrdersTile));
        }

        private void InitWidgetMapping()
        {
            _dicWidgets.Add(Widget.Camera, cameraTileList);
            _dicWidgets.Add(Widget.CircutorCVMMeter, circutorCVMMeterTileList);
            _dicWidgets.Add(Widget.Clock, clockTileList);
            _dicWidgets.Add(Widget.ElectricityMeter, electricalMeterTileList);
            _dicWidgets.Add(Widget.Gateway, gatewayTileList);
            _dicWidgets.Add(Widget.GeozoneFailuresMonitor, geozoneFailuresMonitorTileList);
            _dicWidgets.Add(Widget.IoTEdgeRouterStatus, ioTEdgeRouterStatusTileList);
            _dicWidgets.Add(Widget.LuminaireController, luminaireControllerTileList);
            _dicWidgets.Add(Widget.PollutionSensorWidget, pollutionSensorWidgetTileList);
            _dicWidgets.Add(Widget.SecurityWidget, securityWidgetTileList);
            _dicWidgets.Add(Widget.SpoonyWidget, spoonyWidgetTileList);
            _dicWidgets.Add(Widget.SunriseSunsetTimes, sunriseSunsetTimesTileList);
            _dicWidgets.Add(Widget.VehicleChargingStation, vehicleChargingStationTileList);
            _dicWidgets.Add(Widget.Weather, weatherTileList);
            _dicWidgets.Add(Widget.XCamMonitor, xCamMonitorTileList);
            _dicWidgets.Add(Widget.EnvironmentalSensor, environmentalSensorTileList);
        }        

        /// <summary>
        /// Get App WebElement with name
        /// </summary>
        /// <param name="appName"></param>
        /// <returns></returns>
        private IWebElement GetAppElement(string appName)
        {
            string cssSelector = string.Empty;

            if (appName.Equals(App.AdvancedSearch))
            {
                cssSelector = "[id$='customreport-tile'].tile";
            }
            else if (appName.Equals(App.AlarmManager))
            {
                cssSelector = "[id$='alarmmanager-tile'].tile";
            }
            else if (appName.Equals(App.Alarms))
            {
                cssSelector = "[id$='alarms-tile'].tile";
            }
            else if (appName.Equals(App.BatchControl))
            {
                cssSelector = "[id$='realtimebatch-tile'].tile";
            }
            else if (appName.Equals(App.ControlCenter))
            {
                cssSelector = "[id$='controlcenter-tile'].tile";
            }
            else if (appName.Equals(App.Dashboard))
            {
                cssSelector = "[id$='dashboard-tile'].tile";
            }
            else if (appName.Equals(App.DataHistory))
            {
                cssSelector = "[id$='datahistory-tile'].tile";
            }
            else if (appName.Equals(App.DeviceHistory))
            {
                cssSelector = "[id$='devicehistory-tile'].tile";
            }
            else if (appName.Equals(App.DeviceLifetime))
            {
                cssSelector = "[id$='lamplifetime-tile'].tile";
            }
            else if (appName.Equals(App.Energy))
            {
                cssSelector = "[id$='energy-tile'].tile";
            }
            else if (appName.Equals(App.EquipmentInventory))
            {
                cssSelector = "[id$='equipmentgl-tile'].tile";
            }
            else if (appName.Equals(App.FailureAnalysis))
            {
                cssSelector = "[id$='failure-tile'].tile";
            }
            else if (appName.Equals(App.FailureTracking))
            {
                cssSelector = "[id$='failuretrackinggl-tile'].tile";
            }
            else if (appName.Equals(App.Installation))
            {
                cssSelector = "[id$='installation-tile'].tile";
            }
            else if (appName.Equals(App.InventoryVerification))
            {
                cssSelector = "[id$='presweep-tile'].tile";
            }
            else if (appName.Equals(App.LogViewer))
            {
                cssSelector = "[id$='logviewer-tile'].tile";
            }
            else if (appName.Equals(App.MonthlyEnergySaving))
            {
                cssSelector = "[id$='energysaving-tile'].tile";
            }
            else if (appName.Equals(App.RealTimeControl))
            {
                cssSelector = "[id$='realtimegl-tile'].tile";
            }
            else if (appName.Equals(App.ReportManager))
            {
                cssSelector = "[id$='reportmanager-tile'].tile";
            }
            else if (appName.Equals(App.SchedulingManager))
            {
                cssSelector = "[id$='schedulermanager-tile'].tile";
            }
            else if (appName.Equals(App.Users))
            {
                cssSelector = "[id$='user-tile'].tile";
            }
            else if (appName.Equals(App.WorkOrders))
            {
                cssSelector = "[id$='workorder-tile'].tile";
            }

            return Driver.FindElement(By.CssSelector(cssSelector));
        }

        public PageBase GoToApp(string name)
        {
            Type appPage = _dicApps[name].Item1;
            var appTile = _dicApps[name].Item2;
            
            appTile.ClickEx();

            return (PageBase)Activator.CreateInstance(appPage, Driver);
        }

        public PageBase GoToRandomApp()
        {
            var installedApps = GetInstalledAppsName();
            var randomApps = installedApps.PickRandom();

            return GoToApp(randomApps);
        }

        /// <summary>
        /// Open widget
        /// </summary>
        /// <param name="name"></param>
        public void OpenWidget(string name)
        {
            var firstWidgetTile = GetFirstWidget(name);
            firstWidgetTile.ClickEx();
        }

        /// <summary>
        /// Check whether app is installed or not
        /// </summary>
        /// <param name="appName"></param>
        /// <returns></returns>
        public bool CheckAppInstalled(string appName)
        {
            var app = appNamesList.FirstOrDefault(a => a.Text.Equals(appName, StringComparison.InvariantCultureIgnoreCase));
            return app == null ? false : true;
        }

        /// <summary>
        /// Install list of new apps
        /// </summary>
        /// <param name="app"></param>
        public void InstallApps(params string[] apps)
        {
            foreach (var app in apps)
            {
                AppBar.ClickSettingsButton();
                WaitForSettingsPanelDisplayed();
                SettingsPanel.ClickStoreLink();
                SettingsPanel.StorePanel.WaitForLastAppDisplayed();
                SettingsPanel.StorePanel.InstallApp(app);
                WaitForPreviousActionComplete();
            }
        }

        /// <summary>
        /// Install list of new apps if not exist
        /// </summary>
        /// <param name="apps"></param>
        /// <returns>true if an app is installed</returns>
        public bool InstallAppsIfNotExist(params string[] apps)
        {
            var isAppInstalled = false;
            foreach (var app in apps)
            {
                if (!CheckAppInstalled(app))
                {
                    AppBar.ClickSettingsButton();
                    if (IsSettingsPanelDisplayed())
                    {
                        WaitForSettingsPanelDisplayed();
                        SettingsPanel.ClickStoreLink();
                        SettingsPanel.StorePanel.WaitForLastAppDisplayed();
                        SettingsPanel.StorePanel.InstallApp(app);
                        WaitForPreviousActionComplete();
                        isAppInstalled = true;
                    }
                    else
                    {
                        Assert.Warn("SC-1127: The Security widget breaks the Desktop");
                    }
                }
            }
            return isAppInstalled;
        }

        /// <summary>
        /// Check whether widget is installed or not
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool CheckWidgetInstalled(string name)
        {
            string cssSelector = string.Empty;

            if (name.Equals(Widget.Gateway))
            {
                cssSelector = "div.slv-control.slv-rounded-control.controller";
            }
            else if (name.Equals(Widget.LuminaireController))
            {
                cssSelector = "div.slv-control.slv-rounded-control.streetlight";
            }
            else if (name.Equals(Widget.CircutorCVMMeter))
            {
                cssSelector = "div.slv-control.slv-rounded-control.circuitormini";
            }
            else if (name.Equals(Widget.ElectricityMeter))
            {
                cssSelector = "div.slv-control.slv-rounded-control.electricalcounter";
            }
            else if (name.Equals(Widget.VehicleChargingStation))
            {
                cssSelector = "div.slv-control.slv-rounded-control.slv-vcs";
            }
            else if (name.Equals(Widget.Camera))
            {
                cssSelector = "div.slv-control.slv-rounded-control.camera";
            }
            else if (name.Equals(Widget.Weather))
            {
                cssSelector = "div.slv-control.slv-rounded-control.weather";
            }
            else if (name.Equals(Widget.Clock))
            {
                cssSelector = "div.slv-control.slv-rounded-control.clock";
            }
            else if (name.Equals(Widget.GeozoneFailuresMonitor))
            {
                cssSelector = "div.slv-control.slv-rounded-control.failureschart";
            }
            else if (name.Equals(Widget.PollutionSensorWidget))
            {
                cssSelector = "div.pollutionsensor_widget";
            }
            else if (name.Equals(Widget.XCamMonitor))
            {
                cssSelector = "div.slv-control.slv-rounded-control.xcam_widget";
            }
            else if (name.Equals(Widget.SunriseSunsetTimes))
            {
                cssSelector = "div.slv-control.slv-rounded-control.sunrisesunset";
            }
            else if (name.Equals(Widget.IoTEdgeRouterStatus))
            {
                cssSelector = "div.iotrouterstatus_widget";
            }
            else if (name.Equals(Widget.SpoonyWidget))
            {
                cssSelector = "div.spoony_widget";
            }
            else if (name.Equals(Widget.SecurityWidget))
            {
                cssSelector = "div.security_widget";
            }
            else if (name.Equals(Widget.EnvironmentalSensor))
            {
                cssSelector = "div.environmentalsensor";
            }
            else
            {
                Assert.Warn("Widget '{0}' is not defined !", cssSelector);
            }

            return Driver.FindElements(By.CssSelector(cssSelector)).Count > 0;
        }

        /// <summary>
        /// Install list of new widget
        /// </summary>
        /// <param name="apps"></param>
        public void InstallWidgets(params string[] widgets)
        {
            foreach (var widget in widgets)
            {
                AppBar.ClickSettingsButton();
                WaitForSettingsPanelDisplayed();
                SettingsPanel.ClickStoreLink();
                SettingsPanel.StorePanel.ClickWidgetsTab();
                SettingsPanel.StorePanel.WaitForLastWidgetDisplayed();
                SettingsPanel.StorePanel.InstallWidget(widget);
                WaitForPreviousActionComplete();
            }
        }

        /// <summary>
        /// Install list of new widget if not exist
        /// </summary>
        /// <param name="apps"></param>
        public void InstallWidgetsIfNotExist(params string[] widgets)
        {
            foreach (var widget in widgets)
            {
                if (!CheckWidgetInstalled(widget))
                {
                    AppBar.ClickSettingsButton();
                    WaitForSettingsPanelDisplayed();
                    SettingsPanel.ClickStoreLink();
                    SettingsPanel.StorePanel.ClickWidgetsTab();
                    SettingsPanel.StorePanel.WaitForLastWidgetDisplayed();
                    SettingsPanel.StorePanel.InstallWidget(widget);
                    WaitForPreviousActionComplete();
                }
            }
        }

        /// <summary>
        /// Get all installed apps name
        /// </summary>
        /// <returns></returns>
        public List<string> GetInstalledAppsName()
        {
            return JSUtility.GetElementsText("[id$='tile-title'].tile-title");
        }

        /// <summary>
        /// Get all installed widgets name
        /// </summary>
        /// <returns></returns>
        public List<string> GetInstalledWidgetsName()
        {
            var result = new List<string>();            
            var allWidgets = Widget.GetList();

            foreach (var widgetName in allWidgets)
            {
                if (CheckWidgetInstalled(widgetName))
                    result.Add(widgetName);
            }

            return result;
        }

        /// <summary>
        /// Get random apps with specific number count
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<string> GetRandomApps(int count)
        {
            var appsList = _dicApps.Keys.ToList();
            var apps = appsList.PickRandom(count);

            return apps;
        }

        /// <summary>
        /// Get current location of app
        /// </summary>
        /// <param name="appName"></param>
        /// <returns></returns>
        public Point GetAppLocation(string appName)
        {
            appNamesList.Select(p => p.Text).ToList();
            var app = appNamesList.FirstOrDefault(e => string.Equals(e.Text, appName, StringComparison.InvariantCultureIgnoreCase));

            return app.Location;
        }

        /// <summary>
        /// Get current language code of SLV page
        /// </summary>
        /// <returns></returns>
        public string GetCurrentLanguageCode()
        {
            var requestBaseUri = Settings.SlvUrl.EndsWith("/") ? Settings.SlvUrl.TrimEnd('/') : Settings.SlvUrl;
            var src = currentLanguageScript.GetAttribute("src");
            var regex = new Regex(string.Format(@"{0}/groundcontrol/js/lib/JQuery/globalize/cultures/globalize.culture.(.*).js", requestBaseUri));
            var result = regex.Match(src);

            return result.Groups[1].ToString();
        }

        /// <summary>
        ///  Get current skin of SLV page
        /// </summary>
        /// <returns></returns>
        public string GetCurrentSkin()
        {
            var requestBaseUri = Settings.SlvUrl.EndsWith("/") ? Settings.SlvUrl.TrimEnd('/') : Settings.SlvUrl;
            var href = currentSkinCss.GetAttribute("href");
            var regex = new Regex(string.Format(@"{0}/groundcontrol/skins/(.*)/css/style.min.css", requestBaseUri));
            var result = regex.Match(href);

            return result.Groups[1].ToString();
        }

        /// <summary>
        /// Get the city name of first weather widget installed
        /// </summary>
        /// <returns></returns>
        public string GetFirstWeatherWidgetCityName()
        {
            return weatherTileList.FirstOrDefault().FindElement(By.CssSelector("div.weatherCity")).Text;
        }

        /// <summary>
        /// Get the controller name of first clock widget installed
        /// </summary>
        /// <returns></returns>
        public string GetFirstClockWidgetControllerName()
        {
            return clockTileList.FirstOrDefault().FindElement(By.CssSelector("div.clock-title")).Text;
        }

        /// <summary>
        /// Get the geozone name of first sunrise sunset widget installed
        /// </summary>
        /// <returns></returns>
        public string GetFirstSunriseSunsetWidgetGeozoneName()
        {
            return sunriseSunsetTimesTileList.FirstOrDefault().FindElement(By.CssSelector("div.sunrisesunset-title")).Text;
        }

        /// <summary>
        /// Get the sunrise time of first sunrise sunset widget installed
        /// </summary>
        /// <returns></returns>
        public string GetFirstSunriseSunsetWidgetSunriseTime()
        {
            return sunriseSunsetTimesTileList.FirstOrDefault().FindElement(By.CssSelector(".sunrisesunset-sunrise div.sunrisesunset-value")).Text;
        }

        /// <summary>
        /// Get the sunrise label of first sunrise sunset widget installed
        /// </summary>
        /// <returns></returns>
        public string GetFirstSunriseSunsetWidgetSunriseLabel()
        {
            return sunriseSunsetTimesTileList.FirstOrDefault().FindElement(By.CssSelector(".sunrisesunset-sunrise div.sunrisesunset-label")).Text;
        }

        /// <summary>
        /// Get the sunset time of first sunrise sunset widget installed
        /// </summary>
        /// <returns></returns>
        public string GetFirstSunriseSunsetWidgetSunsetTime()
        {
            return sunriseSunsetTimesTileList.FirstOrDefault().FindElement(By.CssSelector(".sunrisesunset-sunset div.sunrisesunset-value")).Text;
        }

        /// <summary>
        /// Get the sunset label of first sunrise sunset widget installed
        /// </summary>
        /// <returns></returns>
        public string GetFirstSunriseSunsetWidgetSunsetLabel()
        {
            return sunriseSunsetTimesTileList.FirstOrDefault().FindElement(By.CssSelector(".sunrisesunset-sunset div.sunrisesunset-label")).Text;
        }
        
        /// <summary>
        /// Get warning count on Failure Tracking tile
        /// </summary>
        /// <returns></returns>
        public int GetFailureTrackingWarningsCount()
        {
            if (!ElementUtility.IsDisplayed(By.CssSelector("[id$='failuretrackinggl-badge-warnings']"))) return 0;

            var count = JSUtility.GetElementText("[id$='failuretrackinggl-badge-warnings-count']").TrimEx();

            return string.IsNullOrEmpty(count) ? 0 : int.Parse(count);
        }

        /// <summary>
        /// Get warning percentage on Failure Tracking tile
        /// </summary>
        /// <returns></returns>
        public string GetFailureTrackingWarningsPercent()
        {
            return JSUtility.GetElementText("[id$='failuretrackinggl-badge-warnings-percent']").TrimEx();
        }

        /// <summary>
        /// Get outage count on Failure Tracking tile
        /// </summary>
        /// <returns></returns>
        public int GetFailureTrackingOutagesCount()
        {
            if (!ElementUtility.IsDisplayed(By.CssSelector("[id$='failuretrackinggl-badge-outages']"))) return 0;

            var count = JSUtility.GetElementText("[id$='failuretrackinggl-badge-outages-count']").TrimEx();

            return string.IsNullOrEmpty(count) ? 0 : int.Parse(count);
        }

        /// <summary>
        /// Get outage percent on Failure Tracking tile
        /// </summary>
        /// <returns></returns>
        public string GetFailureTrackingOutagesPercent()
        {
            return JSUtility.GetElementText("[id$='failuretrackinggl-badge-outages-percent']").TrimEx();
        }

        /// <summary>
        /// Get failures count on Scheduling Manager tile
        /// </summary>
        /// <returns></returns>
        public int GetSchedulingManagerFailuresCount()
        {
            if (!ElementUtility.IsDisplayed(By.CssSelector("[id$='schedulermanager-badge-failures']"))) return 0;

            var count = JSUtility.GetElementText("[id$='schedulermanager-badge-failures-count']").TrimEx();

            return string.IsNullOrEmpty(count) ? 0 : int.Parse(count);
        }

        /// <summary>
        /// Get devices count on Equipment Inventory tile
        /// </summary>
        /// <returns></returns>
        public int GetEquipmentInventoryDevicesCount()
        {
            var count = JSUtility.GetElementText("[id$='equipmentgl-badge']").TrimEx();

            return string.IsNullOrEmpty(count) ? 0 : int.Parse(count);
        }

        /// <summary>
        /// Get installed devices count on Installation tile
        /// </summary>
        /// <returns></returns>
        public int GetInstallationInstalledDevicesCount()
        {
            var count = JSUtility.GetElementText("[id$='installation-badge-installed-count']").TrimEx();

            return string.IsNullOrEmpty(count) ? 0 : int.Parse(count);
        }

        /// <summary>
        /// Get installed devices percent on Installation tile
        /// </summary>
        /// <returns></returns>
        public string GetInstallationInstalledDevicesPercent()
        {
            return JSUtility.GetElementText("[id$='installation-badge-installed-percent']").TrimEx();
        }

        /// <summary>
        /// Get no installed devices count on Installation tile
        /// </summary>
        /// <returns></returns>
        public int GetInstallationNoInstalledDevicesCount()
        {
            var count = JSUtility.GetElementText("[id$='installation-badge-noinstalled-count']").TrimEx();

            return string.IsNullOrEmpty(count) ? 0 : int.Parse(count);
        }

        /// <summary>
        /// Get no installed devices percent on Installation tile
        /// </summary>
        /// <returns></returns>
        public string GetInstallationNoInstalledDevicesPercent()
        {
            return JSUtility.GetElementText("[id$='installation-badge-noinstalled-percent']").TrimEx();
        }

        /// <summary>
        /// Move an App to another one
        /// </summary>
        /// <param name="sourceApp"></param>
        /// <param name="targetApp"></param>
        public void DragAndDrop(string sourceApp, string targetApp)
        {
            JSUtility.DragAndDropHtml5(GetAppElement(sourceApp), GetAppElement(targetApp), Position.Center, Position.Left);
        }

        /// Delete all widgets of a specific widget type
        /// </summary>
        /// <param name="widgetName"></param>
        /// <returns></returns>
        public void DeleteWidgets(string widgetName)
        {
            var widgetTileList = _dicWidgets[widgetName];
            if (!widgetTileList.Any()) return;
            ClickFooterCustomizeButton();
            WaitForConfigToolbarDisplayed();
            foreach (var widget in widgetTileList)
            {
                widget.ClickEx();
            }
            ClickFooterDeleteButton();
            ClickFooterCloseButton();
            WaitForConfigToolbarDisappeared();
        }

        /// <summary>
        /// Get first widget of a specific widget type
        /// </summary>
        /// <param name="widgetName"></param>
        /// <returns></returns>
        public IWebElement GetFirstWidget(string widgetName)
        {
            var widgetTileList = _dicWidgets[widgetName];
            if (!widgetTileList.Any())
                throw new Exception(string.Format("No widget '{0}' installed", widgetName));

            return widgetTileList.FirstOrDefault();
        }

        #region Geozone Failures Monitor

        public string GetGeozoneFailuresMonitorCaption()
        {
            try
            {
                var obj = WebDriverContext.JsExecutor.ExecuteScript(@"var apps = plugin.userContext.desktop.applications; var result =[]; for (var i = 0; i < apps.length; i++){ if(apps[i].app.name == arguments[0]) return apps[i].failuresWidget.pieChart.options.widgetCaption; } return result;", Widget.GeozoneFailuresMonitor);
                if (obj != null)
                    return obj.ToString();
            }
            catch { }
            return string.Empty;
        }

        public GFMInformationModel GetGeozoneFailuresMonitorInfo()
        {
            var result = new GFMInformationModel();
            try
            {
                var obj = (IReadOnlyCollection<object>)WebDriverContext.JsExecutor.ExecuteScript(@"var apps = plugin.userContext.desktop.applications; var result =[]; for (var i = 0; i < apps.length; i++){ if(apps[i].app.name == arguments[0]) return apps[i].failuresWidget.failures; } return result;", Widget.GeozoneFailuresMonitor);
                var failuresInfo = obj.ToList();
                if (failuresInfo.Count == 7)
                {
                    result.GeoZoneId = failuresInfo[0].ToString();
                    result.GeoZoneName = failuresInfo[1].ToString();
                    result.DevicesCount = failuresInfo[2] == null ? 0 : int.Parse(failuresInfo[2].ToString());
                    result.NonCriticalCount = failuresInfo[3] == null ? 0 : int.Parse(failuresInfo[3].ToString());
                    result.NonCriticalRatio = failuresInfo[4] == null ? 0 : decimal.Parse(failuresInfo[4].ToString());
                    result.CriticalCount = failuresInfo[5] == null ? 0 : int.Parse(failuresInfo[5].ToString());
                    result.CriticalRatio = failuresInfo[6] == null ? 0 : decimal.Parse(failuresInfo[6].ToString());

                    return result;
                }
            }
            catch { }
            return result;
        }

        public List<GFMOptionModel> GetGeozoneFailuresMonitorPieChartOptions()
        {
            var result = new List<GFMOptionModel>();
            try
            {
                var categories = (IReadOnlyCollection<object>)WebDriverContext.JsExecutor.ExecuteScript(@"var apps = plugin.userContext.desktop.applications; var result =[]; for (var i = 0; i < apps.length; i++){ if(apps[i].app.name == arguments[0]) return apps[i].failuresWidget.pieChart.categories; } return result;", Widget.GeozoneFailuresMonitor);
                var legends = (IReadOnlyCollection<object>)WebDriverContext.JsExecutor.ExecuteScript(@"var apps = plugin.userContext.desktop.applications; var result =[]; for (var i = 0; i < apps.length; i++){ if(apps[i].app.name == arguments[0]) return apps[i].failuresWidget.pieChart.legend; } return result;", Widget.GeozoneFailuresMonitor);
                var categoryList = categories.ToList();
                var legendList = legends.ToList();
                if (categoryList.Count != legendList.Count) return result;

                for (int i = 0; i < categoryList.Count; i++)
                {
                    var category = (Dictionary<string, object>)categoryList[i];
                    var legend = (Dictionary<string, object>)legendList[i];

                    var model = new GFMOptionModel();
                    model.Name = category["name"].ToString();
                    model.Count = int.Parse(category["count"].ToString());
                    model.IsDisplayed = bool.Parse(category["display"].ToString());
                    model.Percentage = decimal.Parse(category["percentage"].ToString());
                    model.X = int.Parse(legend["x"].ToString());
                    model.Y = int.Parse(legend["y"].ToString());
                    model.ColorHex = legend["color"].ToString().ToUpper();
                    result.Add(model);
                }
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        public void ClickGeozoneFailuresMonitorOption(string name)
        {
            var options = GetGeozoneFailuresMonitorPieChartOptions();
            var option = options.FirstOrDefault(p => p.Name.Equals(name));
            if(option == null) throw new Exception(string.Format("Option '{0}' does not exist", name));
            var firstWidgetTile = GetFirstWidget(Widget.GeozoneFailuresMonitor);
            firstWidgetTile.MoveToAndClick(option.X, option.Y);
            Wait.ForSeconds(1); //Waiting for pie chart reloaded
        }

        public void ClickGeozoneFailuresMonitorSetting()
        {
            var obj = (Dictionary<string, object>)WebDriverContext.JsExecutor.ExecuteScript(@"var apps = plugin.userContext.desktop.applications; var result =[]; for (var i = 0; i < apps.length; i++){ if(apps[i].app.name == arguments[0]) return apps[i].failuresWidget.objects[1].pos; } return result;", Widget.GeozoneFailuresMonitor);
            int x = int.Parse(obj["x"].ToString()) + SLVHelper.GenerateInteger(10, 13);
            int y = int.Parse(obj["y"].ToString()) + SLVHelper.GenerateInteger(10, 13);
            var firstWidgetTile = GetFirstWidget(Widget.GeozoneFailuresMonitor);
            firstWidgetTile.ScrollToElementByJS();
            firstWidgetTile.MoveToAndClick(x, y);
        }

        public bool IsGeozoneFailuresMonitorSettingButtonDisplayed()
        {
            try
            {
                var obj = WebDriverContext.JsExecutor.ExecuteScript(@"var apps = plugin.userContext.desktop.applications; var result =[]; for (var i = 0; i < apps.length; i++){ if(apps[i].app.name == arguments[0]) return apps[i].options.settingsEnabled; } return result;", Widget.GeozoneFailuresMonitor);

                if (obj == null)
                    return false;

                return bool.Parse(obj.ToString());
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public void WaitForFailuresMonitorSettingDisplayed()
        {
            Wait.ForElementStyle(By.CssSelector("[id*='failures'][id$='settings'].side-panel"), "display: block");
            Wait.ForMilliseconds(500);
        }

        public void WaitForFailuresMonitorSettingDisappeared()
        {
            Wait.ForElementStyle(By.CssSelector("[id*='failures'][id$='settings'].side-panel"), "display: none");
            Wait.ForSeconds(2); //For canvas reloaded completely.
        }

        public bool IsFailuresMonitorSettingDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id*='failures'][id$='settings'].side-panel"));
        }

        /// <summary>
        /// Take Geozone Failures Monitor Screenshot
        /// </summary>
        /// <returns></returns>
        public byte[] TakeGeozoneFailuresMonitorScreenshot()
        {
            var gfm = GetFirstWidget(Widget.GeozoneFailuresMonitor);

            return gfm.TakeScreenshotAsBytes(0, 0, 0, 35);
        }
       
        /// <summary>
        /// Hover on specific offsetX, offsety on GFM widget
        /// </summary>
        /// <param name="offsetX"></param>
        /// <param name="offsetY"></param>
        public void HoverGeozoneFailuresMonitor(int offsetX, int offsetY)
        {
            var firstWidgetTile = GetFirstWidget(Widget.GeozoneFailuresMonitor);
            firstWidgetTile.ScrollToElementByJS();
            firstWidgetTile.MoveToAndClick(offsetX, offsetY);
            Wait.ForSeconds(1); //Waiting for pie chart reloaded
        }
        
        /// <summary>
        /// Get Geozone Failures Monitor Screenshot with specific file of a folder
        /// </summary>
        /// <param name="key"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public byte[] GetGeozoneFailuresMonitorScreenshot(string folder, string fileName)
        {
            var filePath = string.Format(@"Resources\img\widgets\GeozoneFailuresMonitor\{0}\{1}.png", folder, fileName);
            var bytes = File.ReadAllBytes(Settings.GetFullPath(filePath));

            return bytes;
        }

        #endregion

        #region Environmental Sensor

        public string GetEnvSensorBottomTitle()
        {
            var firstWidgetTile = GetFirstWidget(Widget.EnvironmentalSensor);
            return firstWidgetTile.FindElement(By.CssSelector(".widget-bottom-title span")).Text.Trim();
        }

        public string GetEnvSensorBottomIcon()
        {
            var firstWidgetTile = GetFirstWidget(Widget.EnvironmentalSensor);
            return firstWidgetTile.FindElement(By.CssSelector(".widget-bottom-title img")).GetAttribute("src");
        }

        public string GetEnvSensorHelpButtonText()
        {
            var firstWidgetTile = GetFirstWidget(Widget.EnvironmentalSensor);
            return firstWidgetTile.FindElement(By.CssSelector("button.help")).Text.Trim();
        }

        public void ClickEnvSensorHelpButton()
        {
            var firstWidgetTile = GetFirstWidget(Widget.EnvironmentalSensor);
            var helpButton = firstWidgetTile.FindElement(By.CssSelector("button.help"));
            helpButton.ClickEx();
        }

        public bool IsEnvSensorRefreshButtonDisplayed()
        {
            var firstWidgetTile = GetFirstWidget(Widget.EnvironmentalSensor);
            return firstWidgetTile.FindElement(By.CssSelector(".widget-header [title='Refresh']")).Displayed;
        }

        public void ClickEnvSensorRefreshButton()
        {
            var firstWidgetTile = GetFirstWidget(Widget.EnvironmentalSensor);
            var refreshButton = firstWidgetTile.FindElement(By.CssSelector(".widget-header [title='Refresh']"));
            refreshButton.ClickEx();
            WaitForEnvRefreshCompleted();
        }

        public bool IsEnvSensorSettingButtonDisplayed()
        {
            var firstWidgetTile = GetFirstWidget(Widget.EnvironmentalSensor);
            return firstWidgetTile.FindElement(By.CssSelector(".widget-header [title='Settings']")).Displayed;
        }

        public void ClickEnvSensorSettingButton()
        {
            var firstWidgetTile = GetFirstWidget(Widget.EnvironmentalSensor);
            var settingButton = firstWidgetTile.FindElement(By.CssSelector(".widget-header [title='Settings']"));
            settingButton.ClickEx();
        }

        public string GetEnvSensorStatusIcon()
        {
            var firstWidgetTile = GetFirstWidget(Widget.EnvironmentalSensor);
            return firstWidgetTile.FindElement(By.CssSelector(".status-panel .status-tab .tab-icon")).GetAttribute("src");
        }

        public string GetEnvSensorStatusText()
        {
            var firstWidgetTile = GetFirstWidget(Widget.EnvironmentalSensor);
            return firstWidgetTile.FindElement(By.CssSelector(".status-panel .status-tab .tab-text")).Text.Trim();
        }

        public string GetEnvSensorMeteringsHeaderText()
        {
            var firstWidgetTile = GetFirstWidget(Widget.EnvironmentalSensor);
            return firstWidgetTile.FindElement(By.CssSelector(".meterings .meterings-header")).Text.Trim();
        }

        public Dictionary<string,string> GetDictionaryOfEnvSensorMeterings()
        {
            var result = new Dictionary<string, string>();

            var firstWidgetTile = GetFirstWidget(Widget.EnvironmentalSensor);
            var metertingPhaseList = firstWidgetTile.FindElements(By.CssSelector(".metering-phase"));

            foreach (var metertingPhase in metertingPhaseList)
            {
                var metertingName = metertingPhase.FindElement(By.CssSelector(".metering-label")).Text.Trim();
                var metertingValue = metertingPhase.FindElement(By.CssSelector(".metering-value")).Text.Trim();
                result.Add(metertingName, metertingValue);
            }
        
            return result;
        }

        public bool IsEnvSensorSettingDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id*='environmentalsensor'][id$='settings'].side-panel"));
        }        

        public string GetEnvSensorSettingHeaderText()
        {
            return JSUtility.GetElementText("[id*='environmentalsensor'][id$='settings'].side-panel .side-panel-title-label");
        }

        public bool IsEnvSensorSettingGeozonesDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id*='environmentalsensor'][id$='settings'].side-panel [id$='settings-geozones']"));
        }

        public void WaitForEnvSensorSettingDisplayed()
        {
            Wait.ForElementStyle(By.CssSelector("[id*='environmentalsensor'][id$='settings'].side-panel"), "display: block");
        }

        public void WaitForEnvSensorSettingDisappeared()
        {
            Wait.ForElementStyle(By.CssSelector("[id*='environmentalsensor'][id$='settings'].side-panel"), "display: none");
        }

        public void WaitForEnvRefreshCompleted()
        {
            if (Browser.Name.Equals("IE"))
            {
                Wait.ForSeconds(1); // for IE issue with transform: rotate
            }
            else
            {
                Wait.ForElementStyle(By.CssSelector("[id*='environmentalsensor'] .widget-header [title='Refresh']"), "transform: rotate(0deg)");
                Wait.ForElementsDisplayed(By.CssSelector("[id*='environmentalsensor'] .metering-phase .metering-label"));
                Wait.ForSeconds(1);
            }
        }

        public void ClickEnvSensorStatusTab()
        {
            var firstWidgetTile = GetFirstWidget(Widget.EnvironmentalSensor);
            var statusTab = firstWidgetTile.FindElement(By.CssSelector(".status-tab"));
            statusTab.ClickEx();
        }

        public void WaitForEnvSensorStatusPanelDisplayed()
        {
            Wait.ForElementStyle(By.CssSelector("[id*='environmentalsensor'].tile-background .status-panel"), "top: -2px");
        }

        public void WaitForEnvSensorStatusPanelDisappeared()
        {
            Wait.ForElementStyle(By.CssSelector("[id*='environmentalsensor'].tile-background .status-panel"), "top: -240px");
        }

        public bool IsEnvSensorStatusPanelDisplayed()
        {
            var firstWidgetTile = GetFirstWidget(Widget.EnvironmentalSensor);

            return firstWidgetTile.FindElement(By.CssSelector(".status-panel")).GetStyleAttr("top").Equals("-2px");
        }

        public Dictionary<string, string> GetDictionaryOfEnvSensorStatusFailures()
        {
            var result = new Dictionary<string, string>();

            var firstWidgetTile = GetFirstWidget(Widget.EnvironmentalSensor);
            var failuresList = firstWidgetTile.FindElements(By.CssSelector(".status-panel .status-content-failure"));

            foreach (var failure in failuresList)
            {
                var failureName = failure.FindElement(By.CssSelector(".failure-label")).Text.Trim();
                var failureIcon = failure.FindElement(By.CssSelector(".failure-icon")).GetAttribute("src");
                result.Add(failureName, failureIcon);
            }

            return result;
        }

        #endregion //Environmental Sensor

        #endregion //Business methods

        protected override void WaitForPageReady()
        {
            Wait.ForElementStyle(By.Id("slv_preloader"), "display: none");
            Wait.ForElementsDisplayed(By.CssSelector("[id$='desktop-footer-mainToolbar_item_configuration']"));
        }        
    }
}
