using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using SeleniumExtras.PageObjects;
using SeleniumExtras.WaitHelpers;
using StreetlightVision.Extensions;
using StreetlightVision.Models;
using StreetlightVision.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace StreetlightVision.Pages.UI
{
    public class Map : IWaitable
    {
        #region Variables

        private IWebDriver _driver;
        private PageBase _page;
        private DeviceClusterPopupPanel _deviceClusterPopupPanel;

        #endregion //Variables

        #region IWebElements

        [FindsBy(How = How.CssSelector, Using = "[id$='map']")]
        private IWebElement mapContainer;

        [FindsBy(How = How.CssSelector, Using = "[id$='map'] div.leaflet-marker-icon.leaflet-icon-background.leaflet-zoom-animated.leaflet-interactive")]
        private IList<IWebElement> devicesList;

        [FindsBy(How = How.CssSelector, Using = "[id$='map'] div.map-device-selection[style*='display: block']")]
        private IWebElement selectedDevice;

        [FindsBy(How = How.CssSelector, Using = "[id$='map'] div.map-device-tooltip")]
        private IList<IWebElement> deviceTooltipsList;

        [FindsBy(How = How.CssSelector, Using = "[id^='selection-marker']")]
        private IWebElement selectionMarker;

        [FindsBy(How = How.CssSelector, Using = "[id$='recorder-button'] [class$='recorder-icon']")]
        private IWebElement recorderButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='recorder-button'] .slv-label")]
        private IWebElement recorderLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='recorder-button'] [class$='recorder-close']")]
        private IWebElement recorderCancelButton;        

        #region Top right buttons

        [FindsBy(How = How.CssSelector, Using = "[id$='map'] div.leaflet-control-button.map-button-target")]
        private IWebElement followMeButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='map'] div.leaflet-control-button[style*='add.png']")]
        private IWebElement addStreetlightButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='map'] .leaflet-top.leaflet-right .leaflet-control-toolbar .leaflet-control-button:nth-child(1)")]
        private IWebElement earthOrPlanButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='map'] .leaflet-top.leaflet-right .leaflet-control-toolbar .leaflet-control-button:nth-child(2)")]
        private IWebElement chooseMapSourceButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='map'] div.leaflet-control-button.map-button-geozone")]
        private IWebElement globalEarthIcon;

        [FindsBy(How = How.CssSelector, Using = "[id='map-geozone-panel'] button.icon-refresh")]
        private IWebElement realtimeRefreshButton;

        [FindsBy(How = How.CssSelector, Using = "[id='map-geozone-panel'] button.icon-play")]
        private IWebElement realtimeExecuteAutoRefreshButton;

        [FindsBy(How = How.CssSelector, Using = "[id='map-geozone-panel'] button.icon-pause")]
        private IWebElement realtimeStopAutoRefreshButton;

        [FindsBy(How = How.CssSelector, Using = "[id='map-geozone-refresh-rate-field']")]
        private IWebElement realtimeRefreshRateNumericInput;

        [FindsBy(How = How.CssSelector, Using = "[id='failuretracking-gl-map-geozone-panel']")]
        private IWebElement filterGeozonePopup;

        [FindsBy(How = How.CssSelector, Using = "[id='failuretracking-gl-field-checkbox']")]
        private IWebElement filterGeozoneCheckbox;

        [FindsBy(How = How.CssSelector, Using = "[id='failuretracking-gl-map-selected-geozone']")]
        private IWebElement filterGeozoneLabel;

        #region Bing Map sub items

        [FindsBy(How = How.CssSelector, Using = "[id$='map-source-panel'] > div:nth-child(1) > div.map-source-item-title")]
        private IWebElement openStreetMapItemLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='map-source-panel'] > div:nth-child(1) > div.map-source-item-icon.map-source-openmap")]
        private IWebElement openStreetMapItemButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='map-source-panel'] > div:nth-child(2) > div.map-source-item-title")]
        private IWebElement googleMapItemLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='map-source-panel'] > div:nth-child(2) > div.map-source-item-icon.map-source-googlemap")]
        private IWebElement googleMapItemButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='map-source-panel'] > div:nth-child(3) > div.map-source-item-title")]
        private IWebElement bingMapItemLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='map-source-panel'] > div:nth-child(3) > div.map-source-item-icon.map-source-bingmap")]
        private IWebElement bingMapItemButton;

        #endregion // Bing Map sub items

        [FindsBy(How = How.CssSelector, Using = "[id$='map'] a.leaflet-control-zoom-in")]
        private IWebElement zoomInButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='map'] a.leaflet-control-zoom-out")]
        private IWebElement zoomOutButton;

        #region GL Map

        [FindsBy(How = How.CssSelector, Using = "div.mapboxgl-map canvas.mapboxgl-canvas")]
        private IWebElement mapGLCanvas;

        [FindsBy(How = How.CssSelector, Using = "div.mapboxgl-map div.mapboxgl-ctrl-scale")]
        private IWebElement mapGLScaleLabel;

        [FindsBy(How = How.CssSelector, Using = "div.mapboxgl-map button.mapboxgl-ctrl-zoom-in")]
        private IWebElement zoomInGLButton;

        [FindsBy(How = How.CssSelector, Using = "div.mapboxgl-map button.mapboxgl-ctrl-zoom-out")]
        private IWebElement zoomOutGLButton;

        [FindsBy(How = How.CssSelector, Using = "div.mapboxgl-map [id='mapboxgl-ctrl-zoom-slider-vranger']")]
        private IWebElement zoomSliderGLInput;

        [FindsBy(How = How.CssSelector, Using = "div.mapboxgl-map div.mapboxgl-ctrl-zoom-slider")]
        private IWebElement zoomSliderGL;

        [FindsBy(How = How.CssSelector, Using = "div.mapboxgl-map button.mapboxgl-ctrl-view")]
        private IWebElement viewGLButton;

        [FindsBy(How = How.CssSelector, Using = "div.mapboxgl-map button.mapboxgl-ctrl-source:not(.mapboxgl-ctrl-icon-hidden)")]
        private IWebElement mapSourceGLButton;

        [FindsBy(How = How.CssSelector, Using = "div.mapboxgl-map button.mapboxgl-ctrl-source[data-source='BingMap']")]
        private IWebElement bingMapGLButton;

        [FindsBy(How = How.CssSelector, Using = "div.mapboxgl-map button.mapboxgl-ctrl-source[data-source='GoogleMap']")]
        private IWebElement googleMapGLButton;

        [FindsBy(How = How.CssSelector, Using = "div.mapboxgl-map button.mapboxgl-ctrl-source[data-source='OpenMap']")]
        private IWebElement openMapGLButton;

        [FindsBy(How = How.CssSelector, Using = "div.mapboxgl-map button.mapboxgl-ctrl-compass")]
        private IWebElement compassButton;

        [FindsBy(How = How.CssSelector, Using = "div.mapboxgl-map button.mapboxgl-ctrl-geolocate")]
        private IWebElement locateButton;

        #endregion

        #endregion //Top right buttons

        #region Bottom right labels

        [FindsBy(How = How.CssSelector, Using = "[id$='map'] > div.leaflet-control-container > div.leaflet-bottom.leaflet-right > div > div:nth-child(1)")]
        private IWebElement lattitudeLocationLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='map'] > div.leaflet-control-container > div.leaflet-bottom.leaflet-right > div > div:nth-child(2)")]
        private IWebElement longitudeLocationLabel;

        #endregion //Bottom right labels

        #endregion //IWebElements

        #region Constructor

        public Map(IWebDriver driver, PageBase page)
        {
            _driver = driver;
            _page = page;

            PageFactory.InitElements(this, new RetryingElementLocator(driver, TimeSpan.FromMilliseconds(Settings.LocatorRetryingTimeout), TimeSpan.FromMilliseconds(Settings.LocatorPollingInterval)));
        }

        #endregion //Constructor

        #region Properties

        public IWebDriver Driver
        {
            get
            {
                return _driver;
            }
        }

        public PageBase Page
        {
            get { return _page; }
        }

        public DeviceClusterPopupPanel DeviceClusterPopupPanel
        {
            get
            {
                if (_deviceClusterPopupPanel == null)
                {
                    _deviceClusterPopupPanel = new DeviceClusterPopupPanel(this.Driver, this.Page);
                }

                return _deviceClusterPopupPanel;
            }
        }

        #endregion //Properties

        #region Basic methods        

        #region Actions

        /// <summary>
        /// Move to active device
        /// </summary>
        public void MoveToActiveDevice()
        {
            selectedDevice.MoveTo();
        }

        /// <summary>
        /// Click 'Recorder' button
        /// </summary>
        public void ClickRecorderButton()
        {
            recorderButton.ClickEx();
        }

        /// <summary>
        /// Click 'RecorderCancel' button
        /// </summary>
        public void ClickRecorderCancelButton()
        {
            recorderCancelButton.ClickEx();
        }

        #region Top right buttons

        /// <summary>
        /// Move to Global Earth icon in map
        /// </summary>
        public void MoveToGlobalEarthIcon()
        {
            Wait.ForElementDisplayed(globalEarthIcon);
            globalEarthIcon.MoveTo();
        }

        /// <summary>
        /// Click 'FollowMe' button
        /// </summary>
        public void ClickFollowMeButton()
        {
            followMeButton.ClickEx();           
        }

        /// <summary>
        /// Click 'AddStreetlight' button
        /// </summary>
        public void ClickAddStreetlightButton()
        {
            addStreetlightButton.ClickEx();
        }

        /// <summary>
        /// Click 'EarthOrPlan' button
        /// </summary>
        public void ClickEarthOrPlanButton()
        {
            earthOrPlanButton.ClickEx();
        }

        /// <summary>
        /// Click 'ChooseMapSource' button
        /// </summary>
        public void ClickChooseMapSourceButton()
        {
            chooseMapSourceButton.ClickEx();
        }

        /// <summary>
        /// Click 'RealtimeRefresh' button
        /// </summary>
        public void ClickRealtimeRefreshButton()
        {
            realtimeRefreshButton.ClickEx();
        }

        /// <summary>
        /// Click 'RealtimeExecuteAutoRefresh' button
        /// </summary>
        public void ClickRealtimeExecuteAutoRefreshButton()
        {
            realtimeExecuteAutoRefreshButton.ClickEx();
        }

        /// <summary>
        /// Click 'RealtimeStopAutoRefresh' button
        /// </summary>
        public void ClickRealtimeStopAutoRefreshButtonButton()
        {
            realtimeStopAutoRefreshButton.ClickEx();
        }

        /// <summary>
        /// Enter a value for 'RealtimeRefreshRate' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterRealtimeRefreshRateNumericInput(string value)
        {
            realtimeRefreshRateNumericInput.Enter(value);
        }

        #region Bing Map sub items

        /// <summary>
        /// Click 'OpenStreetMapItem' button
        /// </summary>
        public void ClickOpenStreetMapItemButton()
        {
            openStreetMapItemButton.ClickEx();
        }

        /// <summary>
        /// Click 'GoogleMapItem' button
        /// </summary>
        public void ClickGoogleMapItemButton()
        {
            googleMapItemButton.ClickEx();
        }

        /// <summary>
        /// Click 'BingMapItem' button
        /// </summary>
        public void ClickBingMapItemButton()
        {
            bingMapItemButton.ClickEx();
        }

        #endregion // Bing Map sub items

        /// <summary>
        /// Click 'ZoomIn' button
        /// </summary>
        public void ClickZoomInButton()
        {
            zoomInButton.ClickEx();
        }

        /// <summary>
        /// Click 'ZoomOut' button
        /// </summary>
        public void ClickZoomOutButton()
        {
            zoomOutButton.ClickEx();
        }

        #endregion //Top right buttons

        #region Bottom right labels

        #endregion //Bottom right labels

        #region GL Map

        /// <summary>
        /// Click 'ZoomInGL' button
        /// </summary>
        public void ClickZoomInGLButton()
        {
            zoomInGLButton.ClickEx();
        }

        /// <summary>
        /// Click 'ZoomOutGL' button
        /// </summary>
        public void ClickZoomOutGLButton()
        {
            zoomOutGLButton.ClickEx();
        }

        /// <summary>
        /// Enter a value for 'ZoomSliderGL' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterZoomSliderGLInput(string value)
        {
            zoomSliderGLInput.Enter(value);
        }

        /// <summary>
        /// Click 'ViewGL' button
        /// </summary>
        public void ClickViewGLButton()
        {
            viewGLButton.ClickEx();
        }

        /// <summary>
        /// Click 'MapSourceGL' button
        /// </summary>
        public void ClickMapSourceGLButton()
        {
            mapSourceGLButton.ClickEx();
        }

        /// <summary>
        /// Click 'BingMapGL' button
        /// </summary>
        public void ClickBingMapGLButton()
        {
            bingMapGLButton.ClickEx();
        }

        /// <summary>
        /// Click 'GoogleMapGL' button
        /// </summary>
        public void ClickGoogleMapGLButton()
        {
            googleMapGLButton.ClickEx();
        }

        /// <summary>
        /// Click 'OpenMapGL' button
        /// </summary>
        public void ClickOpenMapGLButton()
        {
            openMapGLButton.ClickEx();
        }

        /// <summary>
        /// Click 'Compass' button
        /// </summary>
        public void ClickCompassButton()
        {
            compassButton.ClickEx();
        }

        /// <summary>
        /// Click 'Locate' button
        /// </summary>
        public void ClickLocateButton()
        {
            locateButton.ClickEx();
        }

        #endregion       

        #endregion //Actions

        #region Get methods        

        /// <summary>
        /// Get 'Recorder' label text
        /// </summary>
        /// <returns></returns>
        public string GetRecorderText()
        {
            return recorderLabel.Text;
        }

        #region Top right buttons

        /// <summary>
        /// Get 'RealtimeRefreshRate' input value
        /// </summary>
        /// <returns></returns>
        public string GetRealtimeRefreshRateValue()
        {
            return realtimeRefreshRateNumericInput.Value();
        }

        #region Bing Map sub items

        /// <summary>
        /// Get 'OpenStreetMapItem' label text
        /// </summary>
        /// <returns></returns>
        public string GetOpenStreetMapItemText()
        {
            return openStreetMapItemLabel.Text;
        }

        /// <summary>
        /// Get 'GoogleMapItem' label text
        /// </summary>
        /// <returns></returns>
        public string GetGoogleMapItemText()
        {
            return googleMapItemLabel.Text;
        }

        /// <summary>
        /// Get 'BingMapItem' label text
        /// </summary>
        /// <returns></returns>
        public string GetBingMapItemText()
        {
            return bingMapItemLabel.Text;
        }

        #endregion // Bing Map sub items

        #endregion //Top right buttons

        #region Bottom right labels

        /// <summary>
        /// Get 'LattitudeLocation' label text
        /// </summary>
        /// <returns></returns>
        public string GetLattitudeLocationText()
        {
            return lattitudeLocationLabel.Text;
        }

        /// <summary>
        /// Get 'LongitudeLocation' label text
        /// </summary>
        /// <returns></returns>
        public string GetLongitudeLocationText()
        {
            return longitudeLocationLabel.Text;
        }

        #endregion //Bottom right labels

        #region GL Map

        /// <summary>
        /// Get 'MapGLScale' label text
        /// </summary>
        /// <returns></returns>
        public string GetMapGLScaleText()
        {
            return mapGLScaleLabel.Text;
        }

        /// <summary>
        /// Get 'ZoomSliderGL' input value
        /// </summary>
        /// <returns></returns>
        public string GetZoomSliderGLValue()
        {
            return zoomSliderGLInput.Value();
        }

        #endregion

        #endregion //Get methods

        #endregion //Basic methods

        #region Business methods

        public void ClickRandomPoint()
        {
            var mapSize = mapContainer.Size;
            var x = mapSize.Width / 2 + SLVHelper.GenerateInteger(-200, 200);
            var y = mapSize.Height / 2 + SLVHelper.GenerateInteger(-300, 300);
            
            mapContainer.MoveToAndClick(x, y);
        }

        public void ClickCentralPoint()
        {
            var mapSize = mapContainer.Size;
            var x = mapSize.Width / 2;
            var y = mapSize.Height / 2;

            mapContainer.MoveToAndClick(x, y);
        }

        /// <summary>
        /// Position a new device on map
        /// </summary>
        public void PositionNewDevice()
        {
            var mapCenterX = mapContainer.Size.Width / 2;
            var mapCenterY = mapContainer.Size.Height / 2;
            var random = new Random();
            var randomX = random.Next(mapCenterX - 40, mapCenterX + 40);
            var randomY = random.Next(mapCenterY - 80, mapCenterY + 80);

            mapContainer.MoveToAndClick(randomX, randomY);
        }

        #region Wait methods

        /// <summary>
        /// Wait for recorder boundary displayed
        /// </summary>
        public void WaitForRecorderDisplayed()
        {
            Wait.ForElementDisplayed(By.CssSelector("[id$='recorder'].map"));
            Wait.ForElementText(By.CssSelector("[id$='recorder'] [id$='recorder-button'] > div.slv-label"));
        }

        /// <summary>
        /// Wait for recorder boundary disappeared
        /// </summary>
        public void WaitForRecorderDisappeared()
        {
            Wait.ForElementInvisible(By.CssSelector("[id$='recorder'].map"));
        }

        public void WaitForRealtimeRefreshPanelDisplayed()
        {
            Wait.ForElementDisplayed(By.CssSelector("[id='map-geozone-panel'][class*='toolbar-refresh-panel']"));
        }

        public void WaitForRealtimeRefreshPanelDisappeared()
        {
            Wait.ForElementInvisible(By.CssSelector("[id='map-geozone-panel'][class*='toolbar-refresh-panel']"));
        }

        public void WaitForLocationMarkerDisplayed()
        {
            Wait.ForElementDisplayed(By.CssSelector("[id^='user-marker']"));
        }

        public void WaitForDevicesDisplayedOnGLMap()
        {
            Wait.ForGLMapStopFlying();
        }

        public void WaitForTooltipGLDisplayed()
        {
            Wait.ForElementStyle(By.CssSelector("div.mapboxgl-map div.slv-map-tooltip"), "display: block");
            Wait.ForSeconds(1);
        }

        public void WaitForDeviceClusterPopupPanelDisplayed()
        {
            Wait.ForElementStyle(By.CssSelector(".slv-map-cluster-popup"), "display: block");
            Wait.ForSeconds(1);
        }

        public void WaitForDeviceClusterPopupPanelDisappeared()
        {
            Wait.ForElementInvisible(By.CssSelector(".slv-map-cluster-popup"));
            Wait.ForSeconds(1);
        }

        public void WaitForProgressGLCompleted()
        {
            while (true)
            {
                var count = WebDriverContext.CurrentDriver.FindElements(By.CssSelector("html:not(.nprogress-busy)")).Count;
                Wait.ForSeconds(2);
                if (count > 0)
                {
                    break;
                }
            }
            Wait.ForSeconds(2);
        }

        #endregion //Wait methods

        #region Html Map

        /// <summary>
        /// Get active device name of tooltip
        /// </summary>
        /// <returns></returns>
        public string GetActiveDeviceName()
        {
            var selectedTooltip = deviceTooltipsList.FirstOrDefault(p => p.GetAttribute("style").Contains("display: block"));
            var name = selectedTooltip.FindElement(By.CssSelector("div:nth-child(2) > div:nth-child(1)")).Text;

            return name;
        }

        /// <summary>
        /// Get active device status of tooltip
        /// </summary>
        /// <returns></returns>
        public string GetActiveDeviceStatus()
        {
            var selectedTooltip = deviceTooltipsList.FirstOrDefault(p => p.GetAttribute("style").Contains("display: block"));
            var status = selectedTooltip.FindElement(By.CssSelector("div:nth-child(2) > div:nth-child(3)")).Text;

            return status;
        }

        /// <summary>
        /// Get active device mode of tooltip
        /// </summary>
        /// <returns></returns>
        public string GetActiveDeviceMode()
        {
            var selectedTooltip = deviceTooltipsList.FirstOrDefault(p => p.GetAttribute("style").Contains("display: block"));
            var mode = selectedTooltip.FindElement(By.CssSelector("div:nth-child(2) > div:nth-child(5)")).Text;

            return mode;
        }

        /// <summary>
        /// Is selection marker present
        /// </summary>
        /// <returns></returns>
        public bool IsSelectionMarkerPresent()
        {
            return selectionMarker.Displayed;
        }

        /// <summary>
        /// Is user location marker present
        /// </summary>
        /// <returns></returns>
        public bool IsLocationMarkerPresent()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id^='user-marker']"));
        }

        /// <summary>
        /// Take a screenshot
        /// </summary>
        /// <returns></returns>
        public byte[] TakeScreenshotAsBytes()
        {
            return mapContainer.TakeScreenshotAsBytes();
        }

        /// <summary>
        /// Take a screenshot
        /// </summary>
        /// <returns></returns>
        public Bitmap TakeScreenshotAsBitmap()
        {
            return mapContainer.TakeScreenshotAsBitmap();
        }

        /// <summary>
        /// Select multiple devices on map
        /// </summary>
        /// <param name="devices"></param>
        /// 
        public void SelectDevices(List<string> devices)
        {
            var displayedDevices = devicesList.Where(p => p.GetStyleAttr("display").Equals("block"));
            var devicesNeedToSelect = new List<IWebElement>();
            var action = new Actions(this.Driver);
            var selectedDevices = new List<string>(devices);
            foreach (var device in displayedDevices)
            {
                device.MoveTo();
                var name = GetActiveDeviceName();
                if (selectedDevices.Any(p => p.Equals(name)))
                {
                    devicesNeedToSelect.Add(device);
                    selectedDevices.Remove(name);
                }
                if (selectedDevices.Count == 0) break;
            }

            foreach (var device in devicesNeedToSelect)
            {
                action.KeyDown(Keys.Control).Click(device).KeyUp(Keys.Control).Build().Perform();
                WaitForPreviousActionComplete();
                Wait.ForSeconds(1);
            }
        }

        /// <summary>
        /// Select multiple devices on map
        /// </summary>
        /// <param name="devices"></param>
        public void SelectDevices(params string[] devices)
        {
            SelectDevices(devices.ToList());
        }

        /// <summary>
        /// Select a device on map
        /// </summary>
        /// <param name="deviceName"></param>
        public void SelectDevice(string deviceName)
        {
            var deviceList = new List<string>();

            deviceList.Add(deviceName);

            SelectDevices(deviceList);
        }

        /// <summary>
        /// Zoom-in by clicking on slider
        /// </summary>
        public void ZoomIn(ZoomLevel level)
        {
            var expectedLevel = (int)level;
            int currentLevel = GetCurrentZoomLevel();
            while (true)
            {
                if (currentLevel >= expectedLevel) break;
                ClickZoomInButton();
                WaitForPreviousActionComplete();
                currentLevel = GetCurrentZoomLevel();
            }
        }

        /// <summary>
        /// Zoom-out by clicking on slider
        /// </summary>
        /// <param name="level"></param>
        public void ZoomOut(ZoomLevel level)
        {
            var expectedLevel = (int)level;
            int currentLevel = GetCurrentZoomLevel();
            while (true)
            {
                if (currentLevel <= expectedLevel) break;
                ClickZoomOutButton();
                WaitForPreviousActionComplete();
                currentLevel = GetCurrentZoomLevel();
            }
        }

        /// <summary>
        /// Zoom to specifiec level by clicking on slider
        /// </summary>
        /// <param name="level"></param>
        public void ZoomTo(ZoomLevel level)
        {
            var expectedLevel = (int)level;
            int currentLevel = GetCurrentZoomLevel();

            if (currentLevel < expectedLevel)
                ZoomIn(level);
            else
                ZoomOut(level);

            Wait.ForSeconds(1);
        }

        /// <summary>
        /// Get background icon of specific streetlight name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public byte[] GetStreetlightBgIconBytes(string name)
        {
            var allDevices = Driver.FindElements(By.CssSelector("[id$='map'] div.leaflet-marker-icon.leaflet-icon-background.leaflet-zoom-animated.leaflet-interactive"));
            var displayedDevices = allDevices.Where(p => p.Displayed).ToList();
            foreach (var device in displayedDevices)
            {
                var hasExisted = device.ChildExists(By.CssSelector("div[class^='leaflet-icon-device-streetlight']"));
                if (!hasExisted) continue;

                device.MoveTo();
                Wait.ForElementDisplayed(By.CssSelector("[id$='map'] div.map-device-tooltip[style*='display: block']"));

                var currentName = GetActiveDeviceName();
                if (currentName.Equals(name))
                {
                    var img = device.FindElement(By.CssSelector("img"));
                    var src = img.GetAttribute("src");
                    if (src == null) return new byte[0];
                    src = src.Replace("data:image/png;base64,", string.Empty);

                    return Convert.FromBase64String(src);
                }
            }

            return null;
        }

        public byte[] GetStreetlightBulbIconBytes(string name)
        {
            var allDevices = Driver.FindElements(By.CssSelector("[id$='map'] div.leaflet-marker-icon.leaflet-icon-background.leaflet-zoom-animated.leaflet-interactive"));
            var displayedDevices = allDevices.Where(p => p.Displayed).ToList();
            foreach (var device in displayedDevices)
            {
                var hasExisted = device.ChildExists(By.CssSelector("div[class^='leaflet-icon-device-streetlight']"));
                if (!hasExisted) continue;

                device.MoveTo();
                Wait.ForElementDisplayed(By.CssSelector("[id$='map'] div.map-device-tooltip[style*='display: block']"));

                var currentName = GetActiveDeviceName();
                if (currentName.Equals(name))
                {                    
                    var bulbIcon = device.FindElement(By.CssSelector(".leaflet-icon-device-streetlight-dark"));
                    var backgroundImgUrl = bulbIcon.GetBackgroundImageUrl();

                    return SLVHelper.DownloadFileData(backgroundImgUrl);
                }
            }

            return null;
        }

        public List<string> GetDevicesDisplayed()
        {
            var result = new List<string>();
            var allDevices = Driver.FindElements(By.CssSelector("[id$='map'] div.leaflet-marker-icon.leaflet-icon-background.leaflet-zoom-animated.leaflet-interactive"));
            var displayedDevices = allDevices.Where(p => p.Displayed).ToList();
            foreach (var device in displayedDevices)
            {
                var hasExisted = device.ChildExists(By.CssSelector("div[class^='leaflet-icon-device-streetlight']"));
                if (!hasExisted) continue;

                device.MoveTo();

                result.Add(GetActiveDeviceName());
            }

            return result;
        }

        public int CountNumberOfDevicesOnMap()
        {
            return Driver.FindElements(By.CssSelector("div.leaflet-marker-icon.leaflet-icon-background.leaflet-zoom-animated.leaflet-interactive[style*='display: block']")).Count;
        }

        public bool IsEarthButtonDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='map'] .leaflet-top.leaflet-right .leaflet-control-toolbar .leaflet-control-button.map-button-earth"));
        }

        public bool IsPlanButtonDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='map'] .leaflet-top.leaflet-right .leaflet-control-toolbar .leaflet-control-button.map-button-plan"));
        }        

        public void MoveToChooseMapSourceButton()
        {
            chooseMapSourceButton.MoveTo();
        }

        public List<string> GetListOfMapSourceItems()
        {
            return JSUtility.GetElementsText("[id$='map'] [id='map-source-panel'] .map-source-item .map-source-item-title");
        }

        public void ChooseMapSource(MapSource source)
        {            
            MoveToChooseMapSourceButton();
            Wait.ForSeconds(2);
            Wait.ForElementDisplayed(By.CssSelector("[id$='map'] [id='map-source-panel'][style*='display: block']"));
            By mapBy = By.CssSelector("[id$='map'] [id='map-source-panel'] .map-source-item:nth-child(1)");
            switch (source)
            {
                case MapSource.OpenStreetMap:
                    mapBy = By.CssSelector("[id$='map'] [id='map-source-panel'] .map-source-item:nth-child(1)");                    
                    break;
                case MapSource.GoogleMaps:
                    mapBy = By.CssSelector("[id$='map'] [id='map-source-panel'] .map-source-item:nth-child(2)");
                    break;
                case MapSource.BingMaps:
                    mapBy = By.CssSelector("[id$='map'] [id='map-source-panel'] .map-source-item:nth-child(3)");
                    break;
            }
            var map = Driver.FindElement(mapBy);
            map.ClickEx();
            Page.AppBar.ClickHeaderBartop();
            Wait.ForElementInvisible(By.CssSelector("[id$='map'] [id='map-source-panel']"));
            Wait.ForSeconds(2);
        }

        public string GetMapSourceIconUrl()
        {
            return chooseMapSourceButton.GetBackgroundImageUrl();
        }

        /// <summary>
        /// Get scale of HTML map at bottom left
        /// </summary>
        /// <returns></returns>
        public string GetMapScaleLineText()
        {
            return JSUtility.GetElementText(".map .leaflet-control-scale .leaflet-control-scale-line:nth-child(1)");
        }

        #endregion //Html Map 

        #region GL Map

        public void MoveToDeviceGL(string longitude, string latitude)
        {            
            try
            {
                Point p = GetDevicePosition(longitude, latitude);
                mapGLCanvas.MoveTo(mapGLCanvas.Size.Width / 2, 0);
                mapGLCanvas.MoveTo(p);
                WaitForTooltipGLDisplayed();
            }
            catch (UnhandledAlertException)
            {                
                SLVHelper.AllowSecurityAlert();
                Point p = GetDevicePosition(longitude, latitude);
                mapGLCanvas.MoveTo(mapGLCanvas.Size.Width / 2, 0);
                mapGLCanvas.MoveTo(p);
                WaitForTooltipGLDisplayed();
            }
        }

        public void MoveToSelectedDeviceGL()
        {
            try
            {
                var deviceModel = GetFirstSelectedDevice();
                Point p = GetDevicePosition(deviceModel.Longitude, deviceModel.Latitude);
                mapGLCanvas.MoveTo(mapGLCanvas.Size.Width / 2, 0);
                mapGLCanvas.MoveTo(p);
                WaitForTooltipGLDisplayed();
            }
            catch (UnhandledAlertException)
            {
                SLVHelper.AllowSecurityAlert();
                var deviceModel = GetFirstSelectedDevice();
                Point p = GetDevicePosition(deviceModel.Longitude, deviceModel.Latitude);
                mapGLCanvas.MoveTo(mapGLCanvas.Size.Width / 2, 0);
                mapGLCanvas.MoveTo(p);
                WaitForTooltipGLDisplayed();
            }
        }

        public string MoveAndGetDeviceNameGL(string longitude, string latitude)
        {
            MoveToDeviceGL(longitude, latitude);
            var tooltipText = JSUtility.GetElementText("div.mapboxgl-map div.slv-map-tooltip");
            var name = tooltipText.SplitAndGetAt(0);
            return name;            
        }

        public string MoveAndGetDeviceStatusGL(string longitude, string latitude)
        {
            MoveToDeviceGL(longitude, latitude);
            var tooltipText = JSUtility.GetElementText("div.mapboxgl-map div.slv-map-tooltip");
            var status = tooltipText.SplitAndGetAt(1);
            return status.SplitAndGetAt(new char[] { ':' }, 1);
        }

        public string MoveAndGetDeviceModeGL(string longitude, string latitude)
        {
            MoveToDeviceGL(longitude, latitude);
            var tooltipText = JSUtility.GetElementText("div.mapboxgl-map div.slv-map-tooltip");
            var mode = tooltipText.SplitAndGetAt(2);
            return mode.SplitAndGetAt(new char[] { ':' }, 1);
        }

        public string MoveAndGetDeviceLevelGL(string longitude, string latitude)
        {
            MoveToDeviceGL(longitude, latitude);
            var tooltipText = JSUtility.GetElementText("div.mapboxgl-map div.slv-map-tooltip");
            var level = tooltipText.SplitAndGetAt(3);
            return level.SplitAndGetAt(new char[] { ':' }, 1);
        }

        public string GetDeviceNameGL()
        {
            var tooltipText = JSUtility.GetElementText("div.mapboxgl-map div.slv-map-tooltip");
            var name = tooltipText.SplitAndGetAt(0);
            return name;
        }

        public string GetDeviceStatusGL()
        {
            var tooltipText = JSUtility.GetElementText("div.mapboxgl-map div.slv-map-tooltip");
            var status = tooltipText.SplitAndGetAt(1);
            return status.SplitAndGetAt(new char[] { ':' }, 1);
        }

        public string GetDeviceModeGL()
        {
            var tooltipText = JSUtility.GetElementText("div.mapboxgl-map div.slv-map-tooltip");
            var mode = tooltipText.SplitAndGetAt(2);
            return mode.SplitAndGetAt(new char[] { ':' }, 1);
        }

        public string GetDeviceLevelGL()
        {
            var tooltipText = JSUtility.GetElementText("div.mapboxgl-map div.slv-map-tooltip");
            var level = tooltipText.SplitAndGetAt(3);
            return level.SplitAndGetAt(new char[] { ':' }, 1);
        }

        public string GetTooltipDevicesCountGL()
        {
            var tooltipText = JSUtility.GetElementText("div.mapboxgl-map div.slv-map-tooltip");
            var devices = tooltipText.SplitAndGetAt(1);
            return devices;
        }

        public string GetTooltipBackgroundColor()
        {
            var tooltip = Driver.FindElement(By.CssSelector("div.mapboxgl-map div.slv-map-tooltip"));
            var bg = tooltip.GetCssValue("background-color");
            var splitString = bg.Split(new string[] { "rgba(", ")", "," }, StringSplitOptions.RemoveEmptyEntries);
            var splitInts = splitString.Select(item => int.Parse(item.Trim())).ToArray();
            var color = Color.FromArgb(splitInts[3], splitInts[0], splitInts[1], splitInts[2]);

            return color.ToHex();
        }

        public string GetTooltipDeviceId()
        {
            var tooltipText = JSUtility.GetElementText("div.mapboxgl-map div.slv-map-tooltip");
            var deviceId = tooltipText.SplitAndGetAt(1);
            return deviceId;
        }

        public string GetTooltipTextColor()
        {
            var tooltip = Driver.FindElement(By.CssSelector("div.mapboxgl-map div.slv-map-tooltip"));
            var textColor = tooltip.GetCssValue("color");
            var splitString = textColor.Split(new string[] { "rgba(", ")", "," }, StringSplitOptions.RemoveEmptyEntries);
            var splitInts = splitString.Select(item => int.Parse(item.Trim())).ToArray();
            var color = Color.FromArgb(splitInts[3], splitInts[0], splitInts[1], splitInts[2]);

            return color.ToHex();
        }

        public void SelectDeviceGL(string longitude, string latitude)
        {
            Point p = GetDevicePosition(longitude, latitude);
            mapGLCanvas.MoveToAndClick(p.X, p.Y);
            WaitForPreviousActionComplete();
        }

        /// <summary>
        /// Select more than 1 device in GL Map with SHIFT key
        /// </summary>
        /// <param name="longitude"></param>
        /// <param name="latitude"></param>
        public void SelectDevicesGL(string longitude, string latitude)
        {
            if (Browser.Name.Equals("IE"))
            {
                Point p;
                try
                {
                   p = GetDevicePosition(longitude, latitude);
                }
                catch (UnhandledAlertException)
                {
                    WebDriverContext.Wait.Until(ExpectedConditions.AlertState(false));
                   p = GetDevicePosition(longitude, latitude);
                }
                WinApiUtility.KeyDown(VK_KeyCode.LSHIFT);
                try
                {
                    mapGLCanvas.MoveToAndClick(p.X, p.Y);
                    WaitForPreviousActionComplete();
                }
                catch (UnhandledAlertException)
                {
                    WebDriverContext.Wait.Until(ExpectedConditions.AlertState(false));
                    mapGLCanvas.MoveToAndClick(p.X, p.Y);
                    WaitForPreviousActionComplete();
                }
                WinApiUtility.KeyUp(VK_KeyCode.LSHIFT);
            }
            else
            {
                var p = GetDevicePosition(longitude, latitude);
                mapGLCanvas.MoveToAndClickWithShiftKey(p.X, p.Y);
                WaitForPreviousActionComplete();
            }
            mapGLCanvas.MoveTo(0, 0);
        }

        /// <summary>
        ///  Select more than 1 device in GL Map with CTRL key
        /// </summary>
        /// <param name="longitude"></param>
        /// <param name="latitude"></param>
        public void SelectDevicesGLWithCtrlKey(string longitude, string latitude)
        {
            if (Browser.Name.Equals("IE"))
            {
                Point p;
                try
                {
                    p = GetDevicePosition(longitude, latitude);
                }
                catch (UnhandledAlertException)
                {
                    WebDriverContext.Wait.Until(ExpectedConditions.AlertState(false));
                    p = GetDevicePosition(longitude, latitude);
                }
                WinApiUtility.KeyDown(VK_KeyCode.LCTRL);
                try
                {
                    mapGLCanvas.MoveToAndClick(p.X, p.Y);
                    WaitForPreviousActionComplete();
                }
                catch (UnhandledAlertException)
                {
                    WebDriverContext.Wait.Until(ExpectedConditions.AlertState(false));
                    mapGLCanvas.MoveToAndClick(p.X, p.Y);
                    WaitForPreviousActionComplete();
                }
                WinApiUtility.KeyUp(VK_KeyCode.LCTRL);
            }
            else
            {
                var p = GetDevicePosition(longitude, latitude);
                mapGLCanvas.MoveToAndClickWithCtrlKey(p.X, p.Y);
                WaitForPreviousActionComplete();
            }
            mapGLCanvas.MoveTo(0, 0);
        }

        /// <summary>
        /// Select more than 1 device in GL Map with S key
        /// </summary>
        /// <param name="longitude"></param>
        /// <param name="latitude"></param>
        public void SelectDevicesGLWithSKey(string longitude, string latitude)
        {
            if (Browser.Name.Equals("IE"))
            {
                Point p;
                try
                {
                    p = GetDevicePosition(longitude, latitude);
                }
                catch (UnhandledAlertException)
                {
                    WebDriverContext.Wait.Until(ExpectedConditions.AlertState(false));
                    p = GetDevicePosition(longitude, latitude);
                }
                WinApiUtility.KeyDown(VK_KeyCode.S);
                try
                {
                    mapGLCanvas.MoveToAndClick(p.X, p.Y);
                    WaitForPreviousActionComplete();
                }
                catch (UnhandledAlertException)
                {
                    WebDriverContext.Wait.Until(ExpectedConditions.AlertState(false));
                    mapGLCanvas.MoveToAndClick(p.X, p.Y);
                    WaitForPreviousActionComplete();
                }
                WinApiUtility.KeyUp(VK_KeyCode.S);
            }
            else
            {
                var p = GetDevicePosition(longitude, latitude);
                mapGLCanvas.MoveToAndClickWithSKey(p.X, p.Y);
                WaitForPreviousActionComplete();
            }
            mapGLCanvas.MoveTo(0, 0);
        }

        /// <summary>
        /// Get longitude and latitude of first selected device
        /// </summary>
        /// <returns></returns>
        public DeviceModel GetFirstSelectedDevice()
        {
            var result = GetSelectedDevices();

            return result != null ? result.FirstOrDefault() : null;
        }

        /// <summary>
        /// Check if have any selected devices in Map GL
        /// </summary>
        /// <returns></returns>
        public bool HasSelectedDevicesInMapGL()
        {
            var result = GetSelectedDevices();

            return result != null && result.Any();
        }

        /// <summary>
        /// Select multiple devices in GL Map with long;lat list of devices (ex: 2.34269;48.85032)
        /// </summary>
        /// <param name="devicesLongLatList"></param>
        public void SelectDevicesGL(List<string> devicesLongLatList)
        {
            if (Browser.Name.Equals("IE"))
            {
                var arrLongLat = devicesLongLatList[0].SplitEx(new char[] { ';', ',' });
                var p = GetDevicePosition(arrLongLat[0], arrLongLat[1]);
                mapGLCanvas.MoveToAndClick(p.X, p.Y);
                try
                {
                    WaitForPreviousActionComplete();
                }
                catch (UnhandledAlertException)
                {
                    WebDriverContext.Wait.Until(ExpectedConditions.AlertState(false));
                    WaitForPreviousActionComplete();
                }
                Wait.ForSeconds(1);

                for (int i = 1; i < devicesLongLatList.Count; i++)
                {
                    var item = devicesLongLatList[i];
                    arrLongLat = item.SplitEx(new char[] { ';', ',' });
                    try
                    {
                        p = GetDevicePosition(arrLongLat[0], arrLongLat[1]);
                    }
                    catch (UnhandledAlertException)
                    {
                        WebDriverContext.Wait.Until(ExpectedConditions.AlertState(false));
                        p = GetDevicePosition(arrLongLat[0], arrLongLat[1]);
                    }
                    WinApiUtility.KeyDown(VK_KeyCode.LSHIFT);
                    try
                    {
                        mapGLCanvas.MoveToAndClick(p.X, p.Y);
                        WaitForPreviousActionComplete();
                    }
                    catch (UnhandledAlertException)
                    {
                        WebDriverContext.Wait.Until(ExpectedConditions.AlertState(false));
                        mapGLCanvas.MoveToAndClick(p.X, p.Y);
                        WaitForPreviousActionComplete();
                    }
                    WinApiUtility.KeyUp(VK_KeyCode.LSHIFT);
                }
            }
            else
            {
                var arrLongLat = devicesLongLatList[0].SplitEx(new char[] { ';', ',' });
                var p = GetDevicePosition(arrLongLat[0], arrLongLat[1]);
                mapGLCanvas.MoveToAndClick(p.X, p.Y);
                WaitForPreviousActionComplete();
                Wait.ForSeconds(2);
                for (int i = 1; i < devicesLongLatList.Count; i++)
                {                    
                    var item = devicesLongLatList[i];
                    arrLongLat = item.SplitEx(new char[] { ';', ',' });
                    p = GetDevicePosition(arrLongLat[0], arrLongLat[1]);
                    mapGLCanvas.MoveToAndClickWithShiftKey(p.X, p.Y);
                    WaitForPreviousActionComplete();
                    Wait.ForSeconds(2);
                }
            }
            mapGLCanvas.MoveTo(0, 0);
        }

        public void SelectDevicesHoldAndMoveTo(List<string> devicesLongLatList, int toOffsetX, int toOffsetY, int waitSeconds = 1)
        {
            var arrLongLat = devicesLongLatList[0].SplitEx(new char[] { ';', ',' });
            var p = GetDevicePosition(arrLongLat[0], arrLongLat[1]);
            if (Browser.Name.Equals("IE"))
            {               
                mapGLCanvas.MoveToAndClick(p.X, p.Y);
                try
                {
                    WaitForPreviousActionComplete();
                }
                catch (UnhandledAlertException)
                {
                    WebDriverContext.Wait.Until(ExpectedConditions.AlertState(false));
                    WaitForPreviousActionComplete();
                }
                Wait.ForSeconds(1);

                for (int i = 1; i < devicesLongLatList.Count; i++)
                {
                    var item = devicesLongLatList[i];
                    arrLongLat = item.SplitEx(new char[] { ';', ',' });
                    try
                    {
                        p = GetDevicePosition(arrLongLat[0], arrLongLat[1]);
                    }
                    catch (UnhandledAlertException)
                    {
                        WebDriverContext.Wait.Until(ExpectedConditions.AlertState(false));
                        p = GetDevicePosition(arrLongLat[0], arrLongLat[1]);
                    }
                    WinApiUtility.KeyDown(VK_KeyCode.LSHIFT);
                    try
                    {
                        mapGLCanvas.MoveToAndClick(p.X, p.Y);
                        WaitForPreviousActionComplete();
                    }
                    catch (UnhandledAlertException)
                    {
                        WebDriverContext.Wait.Until(ExpectedConditions.AlertState(false));
                        mapGLCanvas.MoveToAndClick(p.X, p.Y);
                        WaitForPreviousActionComplete();
                    }
                    WinApiUtility.KeyUp(VK_KeyCode.LSHIFT);
                }
            }
            else
            {
                mapGLCanvas.MoveToAndClick(p.X, p.Y);
                WaitForPreviousActionComplete();
                Wait.ForSeconds(2);
                for (int i = 1; i < devicesLongLatList.Count; i++)
                {
                    var item = devicesLongLatList[i];
                    arrLongLat = item.SplitEx(new char[] { ';', ',' });
                    p = GetDevicePosition(arrLongLat[0], arrLongLat[1]);
                    mapGLCanvas.MoveToAndClickWithShiftKey(p.X, p.Y);
                    WaitForPreviousActionComplete();
                    Wait.ForSeconds(2);
                }
            }
            mapGLCanvas.ClickHoldAndMoveTo(p.X + toOffsetX, p.Y + toOffsetY, waitSeconds);
            mapGLCanvas.MoveTo(0, 0);
        }

        /// <summary>
        /// Get longitude and latitude of selected devices
        /// </summary>
        /// <returns></returns>
        public List<DeviceModel> GetSelectedDevices()
        {
            Wait.ForGLMapStopFlying();

            var result = new List<DeviceModel>();
            var devicesInfo = (Dictionary<string, object>)WebDriverContext.JsExecutor.ExecuteScript("return plugin.userContext.activeView.layout.mapControl.markerManager.selection;");

            if (devicesInfo == null)
                return null;

            foreach (var deviceId in devicesInfo.Keys)
            {
                var deviceInfo = (Dictionary<string, object>)devicesInfo[deviceId];
                var deviceModel = new DeviceModel()
                {
                    Longitude = deviceInfo["lng"].ToString(),
                    Latitude = deviceInfo["lat"].ToString()
                };
                result.Add(deviceModel);
            }

            return result;
        }

        /// <summary>
        /// Get real x,y on screen of device base on longitude, latitude 
        /// </summary>
        /// <param name="longitude"></param>
        /// <param name="latitude"></param>
        /// <returns></returns>
        public Point GetDevicePosition(string longitude, string latitude)
        {
            Wait.ForSeconds(4); //wait for map stop flying completely

            var result = WebDriverContext.JsExecutor.ExecuteScript("return JSON.stringify(plugin.userContext.activeView.layout.mapControl.map.project([arguments[0], arguments[1]]))", longitude, latitude);
            var jToken = JToken.Parse(result.ToString());
            var x = jToken["x"].Value<int>();
            var y = jToken["y"].Value<int>() - 5;

            return new Point(x, y);
        }

        /// <summary>
        /// Get sprite status of device base on longitude, latitude 
        /// </summary>
        /// <param name="longitude"></param>
        /// <param name="latitude"></param>
        /// <returns>
        ///  - ok(green)
        ///  - error(red)
        ///  - warning(orange)
        ///  - ready(grey)
        /// </returns>
        public string GetDeviceSpriteStatus(string longitude, string latitude)
        {
            var latitudeValue = double.Parse(latitude) + 0.0001; // For SC-604             
            var script = "var callback = arguments[0]; plugin.userContext.activeView.layout.mapControl.markerManager.queryCoords({lat : " + latitudeValue.ToString() + ", lng : " + longitude + " }).then(function (result) { return callback(result[0]); });";            
            var result = (Dictionary<string, object>)WebDriverContext.JsExecutor.ExecuteAsyncScript(script);

            if (result == null || !result.Keys.Any()) return string.Empty;

            var sprite = result["sprite"].ToString();
            var status = sprite.SplitAndGetAt(new string[] { "-" }, 1);

            return status;
        }

        /// <summary>
        /// Get sprite string of device/cluster in Map GL via API
        /// </summary>
        /// <param name="longitude"></param>
        /// <param name="latitude"></param>
        /// <returns></returns>
        public string GetSpriteAPIString(string longitude, string latitude)
        {
            dynamic result = new ExpandoObject();
            var latitudeValue = double.Parse(latitude) + 0.0001; // For SC-604             
            var script = "var callback = arguments[0]; plugin.userContext.activeView.layout.mapControl.markerManager.queryCoords({lat : " + latitudeValue.ToString() + ", lng : " + longitude + " }).then(function (result) { return callback(result[0]); });";
            
            var resultScript = (Dictionary<string, object>)WebDriverContext.JsExecutor.ExecuteAsyncScript(script);

            if (resultScript == null || !resultScript.Keys.Any())           
                return string.Empty;
            
            return resultScript["sprite"].ToString();
        }

        /// <summary>
        ///  Get sprite of device base on longitude, latitude 
        /// </summary>
        /// <param name="longitude"></param>
        /// <param name="latitude"></param>
        /// <returns>dynamic: DeviceType, Status, IsSelected</returns>
        public dynamic GetDeviceSprite(string longitude, string latitude)
        {
            dynamic result = new ExpandoObject();
            var sprite = GetSpriteAPIString(longitude, latitude);

            if (string.IsNullOrEmpty(sprite))
            {
                result.Visible = false;
                result.DeviceType = string.Empty;
                result.Status = string.Empty;
                result.IsSelected = false;
                result.IsManualMode = false;
                result.DimmingLevel = "0";

                return result;
            }
            
            var spriteArr = sprite.SplitEx(new string[] { "-" });
            result.Visible = true;
            result.DeviceType = spriteArr[0];
            result.Status = spriteArr.Count > 1 ? spriteArr[1] : string.Empty;           
            result.IsSelected = spriteArr.Contains("selected") ? true : false;
            result.IsManualMode = spriteArr.Contains("manual") ? true : false;
            result.DimmingLevel = "0";

            if (spriteArr.Count > 2)
            {
                var dimmingLevel = spriteArr[2];
                int outValue = 0;
                if(int.TryParse(dimmingLevel, out outValue))
                {
                    result.DimmingLevel = dimmingLevel;
                }
            }         

            return result;
        }        

        /// <summary>
        /// Get sprite of cluster base on longitude, latitude 
        /// </summary>
        /// <param name="longitude"></param>
        /// <param name="latitude"></param>
        /// <returns>dynamic: DeviceCount, Status, IsSelected</returns>
        public dynamic GetClusterSprite(string longitude, string latitude)
        {
            dynamic result = new ExpandoObject();

            var sprite = GetSpriteAPIString(longitude, latitude);
            if (string.IsNullOrEmpty(sprite))
            {
                result.Visible = false;
                result.DeviceCount = "0";
                result.Status = string.Empty;
                result.IsSelected = false;

                return result;
            }
           
            var spriteArr = sprite.SplitEx(new string[] { "-" });
            result.Visible = true;
            result.DeviceCount = spriteArr.Count > 1 ? spriteArr[1]: "0";
            result.Status = spriteArr.Count > 2 ? spriteArr[2] : string.Empty;
            result.IsSelected = spriteArr.Contains("selected") ? true : false;

            return result;
        }

        /// <summary>
        /// Get information of active device in Map GL
        /// </summary>
        /// <returns></returns>
        public DeviceModel GetActiveDevice()
        {
            var latitude = WebDriverContext.JsExecutor.ExecuteScript("return plugin.userContext.getActiveDevice().lat");
            var longitude = WebDriverContext.JsExecutor.ExecuteScript("return plugin.userContext.getActiveDevice().lng");

            var device = new DeviceModel()
            {
                Latitude = latitude.ToString(),
                Longitude = longitude.ToString()
            };

            return device;
        }

        /// <summary>
        /// Get devices count of map boundaries
        /// </summary>
        /// <param name="longMin"></param>
        /// <param name="longMax"></param>
        /// <param name="latMin"></param>
        /// <param name="latMax"></param>
        /// <returns></returns>
        public int GetDeviceCount(string longMin, string longMax, string latMin, string latMax)
        {           
            var script = "var callback = arguments[0]; plugin.userContext.mapControl.markerManager.queryBoundaries({minX : " + longMin + ", maxX : " + longMax + ", minY : " + latMin + ", maxY : " + latMax + " }).then(function (result) { return callback(result.length); });";
            var result = WebDriverContext.JsExecutor.ExecuteAsyncScript(script);
            
            return int.Parse(result.ToString());
        }

        /// <summary>
        /// Get status of all streetlight
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetStreetlightsStatusGL(params string[] streetlightsLongLatList)
        {
            Wait.ForSeconds(2);
            var dic = new Dictionary<string, string>();
            foreach (var item in streetlightsLongLatList)
            {
                var arrLongLat = item.SplitEx(new char[] { ';', ',' });
                MoveToDeviceGL(arrLongLat[0], arrLongLat[1]);
                var name = GetDeviceNameGL();
                var status = GetDeviceStatusGL();
                if (!dic.ContainsKey(name))
                    dic.Add(name, status);
                else
                    dic[name] = status;
            }

            return dic;
        }

        /// <summary>
        /// Take a screenshot
        /// </summary>
        /// <returns></returns>
        public byte[] TakeScreenshotAsBytesGL()
        {
            return mapGLCanvas.TakeScreenshotAsBytes();
        }

        /// <summary>
        /// Zoom-in by clicking on slider
        /// </summary>
        public void ZoomInToGLLevel(ZoomGLLevel level)
        {
            var expectedLevel = (int)level;
            int currentLevel = GetCurrentZoomGLLevel();
            while (true)
            {
                if (currentLevel <= expectedLevel) break;
                ClickZoomInGLButton();
                WaitForDevicesDisplayedOnGLMap();
                currentLevel = GetCurrentZoomGLLevel();
            }
        }

        /// <summary>
        /// Zoom-out by clicking on slider
        /// </summary>
        /// <param name="level"></param>
        public void ZoomOutToGLLevel(ZoomGLLevel level)
        {
            var expectedLevel = (int)level;
            int currentLevel = GetCurrentZoomGLLevel();
            while (true)
            {
                if (currentLevel >= expectedLevel) break;
                ClickZoomOutGLButton();
                WaitForDevicesDisplayedOnGLMap();
                currentLevel = GetCurrentZoomGLLevel();
            }
        }

        /// <summary>
        /// Zoom to specifiec level by clicking on slider
        /// </summary>
        /// <param name="level"></param>
        public void ZoomToGLLevel(ZoomGLLevel level)
        {
            var expectedLevel = (int)level;
            int currentLevel = GetCurrentZoomGLLevel();

            if (currentLevel > expectedLevel)
                ZoomInToGLLevel(level);
            else
                ZoomOutToGLLevel(level);
        }

        /// <summary>
        /// Scroll mouse to a specific level at a center of screen on map GL
        /// </summary>
        public void DragMapToRandomLocation(int numberOfMoves = 2)
        {
            Actions moveMapAction = new Actions(Driver);
            moveMapAction.ClickAndHold(mapContainer);
            for (var i = 0; i < numberOfMoves; i++)
            {
                moveMapAction.MoveByOffset(new Random().Next(-80, 80), new Random().Next(-60, 60));
            }
            moveMapAction.Build().Perform();
        }

        /// <summary>
        /// Get current map GL view (Road/Aerial)
        /// </summary>
        /// <returns></returns>
        public string GetCurrentMapViewGL()
        {
            return viewGLButton.GetAttribute("title");
        }

        public bool IsRecorderDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='recorder'].map"));
        }

        public bool IsRecorderIconDisplayed()
        {
            var imgBy = By.CssSelector("[id$='equipmentgl-recorder-button'] .equipment-gl-recorder-icon img");
            if (!Driver.FindElements(imgBy).Any())
                return false;
            var img = Driver.FindElement(imgBy);
            var iconUrl = img.GetAttribute("src");

            return SLVHelper.IsServerFileExists(iconUrl);
        }

        public void TickFilterGeozoneCheckbox(bool value = true)
        {
            MoveToGlobalEarthIcon();
            Wait.ForElementDisplayed(filterGeozonePopup);
            if (filterGeozoneCheckbox.Selected != value)
            {
                filterGeozoneLabel.ClickEx();
            }
        }        

        public bool IsFilterGeozoneCheckboxChecked()
        {
            return filterGeozoneCheckbox.Selected;
        }

        public bool IsGlobalEarthIconDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector(".mapboxgl-map .leaflet-control-toolbar .map-button-geozone.leaflet-control-button"));
        }

        public Point GetDeviceClusterPopupPanelLocation()
        {
            var deviceClusterPanel = Driver.FindElement(By.CssSelector(".slv-map-cluster-popup"));

            return deviceClusterPanel.Location;
        }

        /// <summary>
        /// Drag and drop Device Cluster Popup Panel to specific offset X,Y
        /// </summary>
        /// <param name="offsetX"></param>
        /// <param name="offsetY"></param>
        public void DragAndDropDeviceClusterPopupPanel(int offsetX, int offsetY)
        {
            var headerPanel = Driver.FindElement(By.CssSelector(".slv-map-cluster-popup .header"));
            headerPanel.DragAndDropToOffsetByJS(offsetX, offsetY, false);
        }

        public bool IsDeviceClusterPopupPanelDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector(".slv-map-cluster-popup"));
        }        

        /// <summary>
        /// Click and hold a device with long,lat to another position x,y
        /// </summary>
        /// <param name="longitude"></param>
        /// <param name="latitude"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void ClickHoldDeviceAndMoveTo(string longitude, string latitude, int x, int y, int waitSeconds = 1)
        {
            Point p = GetDevicePosition(longitude, latitude);
            mapGLCanvas.ClickHoldAndMoveTo(p.X, p.Y, p.X + x, p.Y + y, waitSeconds);
        }

        /// <summary>
        /// Click and hold map with Shift key
        /// </summary>
        public void ClickHoldAndMoveToWithShiftKey()
        {           
            WinApiUtility.KeyDown(VK_KeyCode.LSHIFT);
            DragMapToRandomLocation();
            WinApiUtility.KeyUp(VK_KeyCode.LSHIFT);
        }

        /// <summary>
        /// Click and hold map with S key
        /// </summary>
        public void ClickHoldAndMoveToWithSKey()
        {
            WinApiUtility.KeyDown(VK_KeyCode.S);
            DragMapToRandomLocation();
            WinApiUtility.KeyUp(VK_KeyCode.S);
        }

        /// <summary>
        /// Click and hold a device with long,lat to another device with long,lat
        /// </summary>
        /// <param name="fromLongitude"></param>
        /// <param name="fromLatitude"></param>
        /// <param name="toLongitude"></param>
        /// <param name="toLatitude"></param>
        public void ClickHoldDeviceAndMoveTo(string fromLongitude, string fromLatitude, string toLongitude, string toLatitude, int waitSeconds = 1)
        {
            Point pFrom = GetDevicePosition(fromLongitude, fromLatitude);
            Point pTo = GetDevicePosition(toLongitude, toLatitude);
            mapGLCanvas.ClickHoldAndMoveTo(pFrom.X, pFrom.Y, pTo.X, pTo.Y, waitSeconds);
        }

        /// <summary>
        /// Check if location search marker displayed
        /// </summary>
        /// <returns></returns>
        public bool IsLocationSearchMarkerDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("figure.slv-map-marker-container"));
        }

        /// <summary>
        /// Get location search marker image src
        /// </summary>
        /// <returns></returns>
        public string GetLocationSearchMarkerImageSrc()
        {
            var img = Driver.FindElement(By.CssSelector("figure.slv-map-marker-container > img"));

            return img.GetAttribute("src");
        }

        /// <summary>
        /// Get location search marker caption
        /// </summary>
        /// <returns></returns>
        public string GetLocationSearchMarkerCaption()
        {
            return JSUtility.GetElementText("figure.slv-map-marker-container > figcaption").Trim();
        }

        public void WaitForLocationSearchMarkerDisplayed()
        {            
            Wait.ForElementDisplayed(By.CssSelector("figure.slv-map-marker-container"));
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Get the current zoom level
        /// </summary>
        /// <returns></returns>
        private int GetCurrentZoomLevel()
        {
            var proxyZoom = Driver.FindElement(By.CssSelector("div.leaflet-proxy.leaflet-zoom-animated"));
            var style = proxyZoom.GetAttribute("style");
            var scaleRegex = Regex.Match(style, @"scale\((\d*)\)");
            var scaleValue = int.Parse(scaleRegex.Groups[1].Value);

            return scaleValue;
        }

        /// <summary>
        /// Get the current zoom GL level
        /// </summary>
        /// <returns></returns>
        private int GetCurrentZoomGLLevel()
        {
            int scaleValue = 0;
            var scaleText = GetMapGLScaleText();

            if (Regex.IsMatch(scaleText, @"(\d{1,}) m"))
                scaleValue = int.Parse(Regex.Match(scaleText, @"(\d{1,}) m").Groups[1].Value);
            else if (Regex.IsMatch(scaleText, @"(\d{1,}) km"))
                scaleValue = 1000 * int.Parse(Regex.Match(scaleText, @"(\d{1,}) km").Groups[1].Value);

            return scaleValue;
        }

        #endregion

        #endregion //Business methods

        #region Implemented methods

        /// <summary>
        /// Wait until an action completely loaded
        /// </summary>
        public void WaitForCompletelyLoaded()
        {
            Wait.ForLoadingIconDisappeared();
            WaitForProgressGLCompleted();
        }

        /// <summary>
        /// Wait until an action completely loaded
        /// </summary>
        public void WaitForPreviousActionComplete()
        {
            Wait.ForLoadingIconDisappeared();
            Wait.ForProgressCompleted();
        }

        #endregion

        public void WaitForLoaded()
        {
            Wait.ForElementsDisplayed(By.CssSelector("[id$='map'] a.leaflet-control-zoom-in"));
        }
    }
}
