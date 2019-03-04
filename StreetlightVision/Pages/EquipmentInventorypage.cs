using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using SeleniumExtras.PageObjects;
using StreetlightVision.Extensions;
using StreetlightVision.Models;
using StreetlightVision.Pages.UI;
using StreetlightVision.Utilities;
using System;
using System.Linq;

namespace StreetlightVision.Pages
{
    public class EquipmentInventoryPage : PageBase
    {
        #region Variables

        private GridPanel _gridPanel;
        private Map _map;
        private GeozoneTreeMainPanel _geozoneTreeMainPanel;
        private DeviceEditorPanel _deviceEditorPanel;
        private MultipleDevicesEditorPanel _multipleDevicesEditorPanel;
        private StreetlightEditorPanel _streetlightEditorPanel;
        private GeozoneEditorPanel _geozoneEditorPanel;
        private ControllerEditorPanel _controllerEditorPanel;

        private SwitchEditorPanel _switchEditorPanel;
        private ElectricalCounterEditorPanel _electricalCounterEditorPanel;
        private CabinetControllerEditorPanel _cabinetControllerEditorPanel;

        #endregion //Variables

        #region IWebElements

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-desktop-main'] [id$='browser-show-button']")]
        private IWebElement showButton;

        #endregion //IWebElements

        #region Constructor

        public EquipmentInventoryPage(IWebDriver driver)
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

        public DeviceEditorPanel DeviceEditorPanel
        {
            get
            {
                if (_deviceEditorPanel == null)
                {
                    _deviceEditorPanel = new DeviceEditorPanel(this.Driver, this);
                }

                return _deviceEditorPanel;
            }
        }

        public MultipleDevicesEditorPanel MultipleDevicesEditorPanel
        {
            get
            {
                if (_multipleDevicesEditorPanel == null)
                {
                    _multipleDevicesEditorPanel = new MultipleDevicesEditorPanel(this.Driver, this);
                }

                return _multipleDevicesEditorPanel;
            }
        }

        public StreetlightEditorPanel StreetlightEditorPanel
        {
            get
            {
                if (_streetlightEditorPanel == null)
                {
                    _streetlightEditorPanel = new StreetlightEditorPanel(this.Driver, this);
                }

                return _streetlightEditorPanel;
            }
        }

        public GeozoneEditorPanel GeozoneEditorPanel
        {
            get
            {
                if (_geozoneEditorPanel == null)
                {
                    _geozoneEditorPanel = new GeozoneEditorPanel(this.Driver, this);
                }

                return _geozoneEditorPanel;
            }
        }

        public ControllerEditorPanel ControllerEditorPanel
        {
            get
            {
                if (_controllerEditorPanel == null)
                {
                    _controllerEditorPanel = new ControllerEditorPanel(this.Driver, this);
                }

                return _controllerEditorPanel;
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

        public SwitchEditorPanel SwitchEditorPanel
        {
            get
            {
                if (_switchEditorPanel == null)
                {
                    _switchEditorPanel = new SwitchEditorPanel(this.Driver, this);
                }

                return _switchEditorPanel;
            }
        }

        public ElectricalCounterEditorPanel ElectricalCounterEditorPanel
        {
            get
            {
                if (_electricalCounterEditorPanel == null)
                {
                    _electricalCounterEditorPanel = new ElectricalCounterEditorPanel(this.Driver, this);
                }

                return _electricalCounterEditorPanel;
            }
        }

        public CabinetControllerEditorPanel CabinetControllerEditorPanel
        {
            get
            {
                if (_cabinetControllerEditorPanel == null)
                {
                    _cabinetControllerEditorPanel = new CabinetControllerEditorPanel(this.Driver, this);
                }

                return _cabinetControllerEditorPanel;
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

        public void WaitForMainGeozoneTreeDisplayed()
        {
            Wait.ForElementStyle(By.CssSelector("[id$='browser']"), "left: 20px");
        }

        public void WaitForMainGeozoneTreeDisappeared()
        {
            Wait.ForElementStyle(By.CssSelector("[id$='browser']"), "left: -330px");
        }

        public void WaitForCustomReportDisplayed()
        {
            Wait.ForElementDisplayed(By.CssSelector("[id$='custom-report']"));
            Wait.ForElementStyle(By.CssSelector("[id$='custom-report']"), "left: 0px");
        }

        public void WaitForCustomReportDisappeared()
        {
            Wait.ForElementStyle(By.CssSelector("[id$='custom-report']"), string.Format("left: {0}px", WebDriverContext.JsExecutor.ExecuteScript("return screen.availWidth - 350 - 40")));
        }

        public void WaitForEditorPanelDisplayed()
        {
            Wait.ForElementDisplayed(By.CssSelector("[id$='editor'].slv-rounded-control"));
            Wait.ForElementStyle(By.CssSelector("[id$='editor'].slv-rounded-control"), string.Format("left: {0}px", WebDriverContext.JsExecutor.ExecuteScript("return window.innerWidth - 350 - 60")));
        }

        public void WaitForEditorPanelDisappeared()
        {
            WaitForGeozoneEditorPanelDisappeared();
            WaitForDeviceEditorPanelDisappeared();
        }

        public void WaitForGeozoneEditorPanelDisplayed()
        {
            Wait.ForElementDisplayed(By.CssSelector("[id$='editor-geozone']"));
            Wait.ForElementStyle(By.CssSelector("[id$='editor']"), string.Format("left: {0}px", WebDriverContext.JsExecutor.ExecuteScript("return screen.availWidth - 350 - 60")));
        }

        public void WaitForGeozoneEditorPanelDisappeared()
        {
            Wait.ForElementStyle(By.CssSelector("[id$='editor-geozone']"), "display: none");
        }

        public void WaitForDeviceEditorPanelDisplayed()
        {
            Wait.ForElementDisplayed(By.CssSelector("[id$='editor-device']"));
            Wait.ForElementStyle(By.CssSelector("[id$='editor'].slv-rounded-control"), string.Format("left: {0}px", WebDriverContext.JsExecutor.ExecuteScript("return window.innerWidth - 350 - 60")));
        }

        public void WaitForDeviceEditorPanelDisappeared()
        {
            Wait.ForElementInvisible(By.CssSelector("[id$='editor-device']"));
        }

        public void WaitForMultipleDevicesEditorDisplayed()
        {
            Wait.ForElementDisplayed(By.CssSelector("[id$='editor-devices']"));
            Wait.ForElementsDisplayed(By.CssSelector("[id$='editor-devices-content-layout-list'] .equipment-gl-list-item"));
        }

        public void WaitForMultipleDevicesEditorDisappeared()
        {
            Wait.ForElementInvisible(By.CssSelector("[id$='editor-devices']"));
        }


        /// <summary>
        /// Wait until Open file dialog completely closed
        /// </summary>
        public void WaitUntilOpenFileDialogDisappears()
        {
            Wait.ForSeconds(2);
            if (Browser.Name.Equals("IE"))
            {
                Wait.ForSeconds(2); // This LOC is needed only for IE to avoid "Modal dialog present" exception
            }
        }

        /// <summary>
        /// Click Delete icon button in Geozone Details Panel, if confirmed is true, Yes button on the confirmation dialog will be clicked, else, No button will
        /// </summary>
        /// <param name="confirmed"></param>
        public void DeleteGeozone(bool confirmed)
        {
            GeozoneEditorPanel.ClickDeleteButton();
            WaitForPopupDialogDisplayed();
            if (confirmed)
            {
                Dialog.ClickYesButton();
                WaitForPreviousActionComplete();
                WaitForHeaderMessageDisappeared();
            }
            else
            {
                Dialog.ClickNoButton();
            }
        }

        /// <summary>
        ///  Action to create device with basic info 
        /// </summary>
        /// <param name="deviceType"></param>
        /// <param name="deviceName"></param>
        /// <param name="controllerID"></param>
        /// <param name="identifier"></param>
        /// <param name="typeOfEquipment"></param>
        /// <param name="gatewayHostName"></param>
        public void CreateDevice(DeviceType deviceType, string deviceName = "", string controllerID = "",
            string identifier = "", string typeOfEquipment = "", string gatewayHostName = "", bool positionAtScreenCenter = false)
        {
            //Click on the Add button at the top of the geozone widget
            //Select “Add Device”
            GeozoneEditorPanel.ClickAddButton();
            GeozoneEditorPanel.ClickAddDeviceMenuItem();
            GeozoneEditorPanel.WaitForNewDevicePanelDisplayed();

            //Select "Device" in the list that is displayed in the 'New Device'
            GeozoneEditorPanel.NewDevicePanel.SelectDevice(deviceType);
            GeozoneEditorPanel.NewDevicePanel.WaitForNewDevicePropertiesSectionDisplayed();

            //Input "Name" in the Name field   
            GeozoneEditorPanel.NewDevicePanel.EnterNameInput(deviceName);

            if (deviceType.Equals(DeviceType.Controller))
            {
                //Input "Controller ID" in the Controller ID field
                if (GeozoneEditorPanel.NewDevicePanel.IsControllerIdInputDisplayed())
                    GeozoneEditorPanel.NewDevicePanel.EnterControllerIdInput(controllerID);
                else
                    Assert.Warn("#1398355 - Controller ID is a drop down field instead of a simple text field in the Controller creation panel");
            }
            else
            {
                //Input "Controller ID" in the Controller ID drop down
                if (deviceType.HasControllerID)
                    GeozoneEditorPanel.NewDevicePanel.SelectControllerIdDropDown(controllerID);
            }

            //Input "Identifier" in the Identifier field
            if (deviceType.HasIdentifier)
            {
                if (!GeozoneEditorPanel.NewDevicePanel.IsIdentifierInputReadOnly())
                    GeozoneEditorPanel.NewDevicePanel.EnterIdentifierInput(identifier);
                else
                    Assert.Warn("#1398297 - Unable to create new devices belonging to certain categories");
            }

            //Input "Type of equipment" in the drop down
            if (deviceType.HasTypeOfEquipment)
                GeozoneEditorPanel.NewDevicePanel.SelectTypeOfEquipmentDropDown(typeOfEquipment);

            //Input "Gateway Host Name" in the Gateway Host Name field
            if (deviceType.HasGatewayHostName)
                GeozoneEditorPanel.NewDevicePanel.EnterHostNameInput(deviceType, gatewayHostName);

            //Click on 'Postition the device'
            GeozoneEditorPanel.NewDevicePanel.ClickPositionDeviceButton();
            GeozoneEditorPanel.WaitForNewDevicePanelDisappeared();

            //Click to put device on map
            Map.WaitForRecorderDisplayed();
            if (positionAtScreenCenter)
                Map.ClickCentralPoint();
            else
                Map.ClickRandomPoint();

            WaitForPreviousActionComplete();
            Map.WaitForRecorderDisappeared();
            GeozoneEditorPanel.WaitForNewDevicePanelDisappeared();
            WaitForPreviousActionComplete();
        }

        /// <summary>
        /// Create a new geozone
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parentGeozone"></param>
        public void CreateGeozone(string name = "", string parentGeozone = "", ZoomGLLevel zoomLevel = ZoomGLLevel.km10)
        {
            if (!string.IsNullOrEmpty(parentGeozone)) //if parentGeozone is empty, create new gezone on current parent geozone
            {
                GeozoneTreeMainPanel.SelectNode(parentGeozone);
            }
            //Click on the Add button at the top of the geozone widget
            //Select “Add Geozone”
            GeozoneEditorPanel.ClickAddButton();
            GeozoneEditorPanel.ClickAddGeozoneMenuItem();
            WaitForGeozoneEditorPanelDisappeared();

            Map.WaitForRecorderDisplayed();
            Map.DragMapToRandomLocation();
            WaitForPreviousActionComplete();
            Map.ZoomToGLLevel(zoomLevel);
            Map.ClickRecorderButton();
            Map.WaitForRecorderDisappeared();
            WaitForPreviousActionComplete();
            WaitForGeozoneEditorPanelDisplayed();

            if (!string.IsNullOrEmpty(name)) //if name is empty, get default name (e.g. New GeoZone 1)
            {
                GeozoneEditorPanel.EnterNameInput(name);
            }

            GeozoneEditorPanel.ClickSaveButton();
            WaitForPreviousActionComplete();
            WaitForGeozoneEditorPanelDisappeared();
        }

        /// <summary>
        /// Delete a specific streetlight
        /// </summary>
        /// <param name="pathName"></param>
        public void DeleteStreetlight(string pathName)
        {
            GeozoneTreeMainPanel.SelectNode(pathName);
            WaitForDeviceEditorPanelDisplayed();
            StreetlightEditorPanel.ClickDeleteButton();
            WaitForPopupDialogDisplayed();
            Dialog.ClickYesButton();
            WaitForPreviousActionComplete();
            WaitForHeaderMessageDisappeared();
        }

        /// <summary>
        /// Delete current selected device
        /// </summary>
        public void DeleteCurrentDevice()
        {
            DeviceEditorPanel.ClickDeleteButton();
            WaitForPopupDialogDisplayed();
            if(Dialog.IsOkButtonDisplayed()) Dialog.ClickOkButton();
            Dialog.ClickYesButton();
            WaitForPreviousActionComplete();
            WaitForHeaderMessageDisappeared();
        }

        public void DeleteNode(string nodeName)
        {
            GeozoneTreeMainPanel.SelectNode(nodeName);            
            WaitForEditorPanelDisplayed();
            if (IsPopupDialogDisplayed() && Dialog.IsOkButtonDisplayed())
            {
                Dialog.ClickOkButton();
                WaitForPopupDialogDisappeared();
            }
            var nodeType = GeozoneTreeMainPanel.GetSelectedNodeType();
            var deleteButton = Driver.FindElement(By.CssSelector("[id$='editor-deviceequipmentDeviceButtons_item_delete'] .w2ui-button"));
            if (nodeType == NodeType.GeoZone)
            {
                deleteButton = Driver.FindElement(By.CssSelector("[id$='editor-geozone-buttons-toolbar_item_delete'] .w2ui-button"));
            }            
            deleteButton.ClickEx();
            WaitForPopupDialogDisplayed();
            Dialog.ClickYesButton();
            WaitForPreviousActionComplete();
        }

        /// <summary>
        /// Delete devices with Name pattern
        /// </summary>
        public void DeleteDevices(string namePattern)
        {
            GeozoneTreeMainPanel.ChangeSearchAttribute("Name", "Contains");
            GeozoneTreeMainPanel.EnterSearchTextInput(namePattern);
            GeozoneTreeMainPanel.ClickSearchButton();
            WaitForPreviousActionComplete();
            GeozoneTreeMainPanel.WaitForSearchResultPanelDisplayed();
            var devices = GeozoneTreeMainPanel.SearchResultsGeozonePanel.GetListOfSearchResult("Equipment");
            foreach (var device in devices)
            {
                GeozoneTreeMainPanel.SearchResultsGeozonePanel.SelectFoundDevice(device);
                WaitForPreviousActionComplete();
                WaitForDeviceEditorPanelDisplayed();
                DeleteCurrentDevice();
            }
            GeozoneTreeMainPanel.ClickCollapseSearchButton();
            GeozoneTreeMainPanel.SearchResultsGeozonePanel.ClickBackButton();
            GeozoneTreeMainPanel.WaitForSearchResultPanelDisappeared();
        }

        public void MoveNodeToGeozoneAndClickYesConfirmed(string sourceName, string destName)
        {
            GeozoneTreeMainPanel.MoveNodeToGeozone(sourceName, destName);
            WaitForPopupDialogDisplayed();
            Dialog.ClickYesButton();
            WaitForPreviousActionComplete();
        }

        public bool IsCustomReportPanelDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='equipment-custom-report'][style*='left: 0px']"));
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

        public bool IsDeviceEditorPanelDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-device']"));
        }

        public bool IsGeozoneEditorPanelDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-geozone']"));
        }

        public bool IsMultipleDevicesEditorPanelDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-devices']"));
        }

        public void PressDeleteKey()
        {
            var action = new Actions(Driver);
            action.SendKeys(Keys.Delete).Build().Perform();
        }

        public bool HasPopupDialogDisplayed()
        {
            Wait.ForSeconds(1);
            return ElementUtility.IsDisplayed(By.CssSelector("[id='w2ui-popup']"));
        }

        /// <summary>
        /// Import a file
        /// </summary>
        /// <param name="fileNamePath"></param>
        public void Import(string fileNamePath)
        {
            GeozoneEditorPanel.ClickMoreButton();
            GeozoneEditorPanel.ClickImportMenuItem();
            SLVHelper.OpenFileFromFileDialog(fileNamePath);
            WaitUntilOpenFileDialogDisappears();
            WaitForPreviousActionComplete();
            if (!GeozoneEditorPanel.IsImportPanelDisplayed())
            {
                GeozoneEditorPanel.ClickMoreButton();
                GeozoneEditorPanel.ClickImportMenuItem();
                SLVHelper.OpenFileFromFileDialog(fileNamePath);
                WaitUntilOpenFileDialogDisappears();
                WaitForPreviousActionComplete();
                GeozoneEditorPanel.WaitForImportPanelDisplayed();
            }
        }

        /// <summary>
        /// Replace Nodes with CSV file
        /// </summary>
        /// <param name="fileNamePath"></param>
        public void ReplaceNodes(string fileNamePath)
        {
            GeozoneEditorPanel.ClickMoreButton();
            GeozoneEditorPanel.ClickReplaceNodesMenuItem();
            SLVHelper.OpenFileFromFileDialog(fileNamePath);
            WaitUntilOpenFileDialogDisappears();
            WaitForPreviousActionComplete();
            if (!GeozoneEditorPanel.IsReplaceNodesPanelDisplayed())
            {
                GeozoneEditorPanel.ClickMoreButton();
                GeozoneEditorPanel.ClickReplaceNodesMenuItem();
                SLVHelper.OpenFileFromFileDialog(fileNamePath);
                WaitUntilOpenFileDialogDisappears();
                WaitForPreviousActionComplete();
                GeozoneEditorPanel.WaitForReplaceNodesPanelDisplayed();
            }
        }

        /// <summary>
        ///  Export gezone
        /// </summary>
        public void Export()
        {
            GeozoneEditorPanel.ClickMoreButton();
            GeozoneEditorPanel.ClickExportMenuItem();
            GeozoneEditorPanel.WaitForExportPanelDisplayed();
            WaitForPreviousActionComplete();
        }

        /// <summary>
        /// Click Reload on import panel
        /// </summary>
        public void ReloadImport()
        {
            GeozoneEditorPanel.ImportPanel.ClickReloadButton();
            GeozoneEditorPanel.WaitForImportPanelDisappeared();
            WaitForPreviousActionComplete();
        }

        /// <summary>
        /// Cick Reload on Replace Nodes panel
        /// </summary>
        public void ReloadReplaceNodes()
        {
            GeozoneEditorPanel.ReplaceNodesPanel.ClickReloadButton();
            GeozoneEditorPanel.WaitForReplaceNodesPanelDisappeared();
            WaitForPreviousActionComplete();
        }

        #endregion //Business methods

        protected override void WaitForPageReady()
        {            
            base.WaitForPageReady();
            Map.WaitForProgressGLCompleted();
            OpenGeozoneTreeIfNotExpand();
        }
    }
}
