using NUnit.Framework;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using SeleniumExtras.WaitHelpers;
using StreetlightVision.Extensions;
using StreetlightVision.Models;
using StreetlightVision.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace StreetlightVision.Pages.UI
{
    public class GeozoneTreeMainPanel : PanelBase
    {
        #region Variables

        private SearchResultsGeozonePanel _geozoneSearchResultPanel;
        private FiltersPanel _filtersPanel;
        private MapSearchPanel _mapSearchPanel;

        #endregion //Variables

        #region IWebElements

        [FindsBy(How = How.CssSelector, Using = "[id$='browser']")]
        private IWebElement geozoneTreeContainer;

        [FindsBy(How = How.CssSelector, Using = "[id$='browser'] [id$='browser-content'] div.w2ui-sidebar-div")]
        private IWebElement treeContainer;

        [FindsBy(How = How.CssSelector, Using = "[id$='browser-content'] div.side-panel-title-label, [id$='settings'][style*='display: block'] div.side-panel-title-label")]
        private IWebElement panelTitle;

        [FindsBy(How = How.CssSelector, Using = "[id$='browser-content'] div.w2ui-node, [id$='settings'][style*='display: block'] div.w2ui-node")]
        private IList<IWebElement> nodesList;

        [FindsBy(How = How.CssSelector, Using = "[id$='browser-content'] div.w2ui-node.w2ui-selected, [id$='settings'][style*='display: block'] div.w2ui-node.w2ui-selected")]
        private IWebElement selectedNode;

        [FindsBy(How = How.CssSelector, Using = "[id$='browser-content'] div.w2ui-node.w2ui-selected + div.w2ui-node-sub > .w2ui-node, [id$='settings'][style*='display: block'] div.w2ui-node.w2ui-selected + div.w2ui-node-sub > .w2ui-node")]
        private IList<IWebElement> selectedSubNodeList;

        [FindsBy(How = How.CssSelector, Using = "[id$='browser-content'] div.w2ui-node.w2ui-selected + div.w2ui-node-sub > .w2ui-node .w2ui-node-caption:first-child, [id$='settings'][style*='display: block'] div.w2ui-node.w2ui-selected + div.w2ui-node-sub > .w2ui-node .w2ui-node-caption:first-child")]
        private IList<IWebElement> selectedSubNodeTextList;

        [FindsBy(How = How.CssSelector, Using = "[id$='browser-content'] div.w2ui-node.w2ui-selected + div.w2ui-node-sub > .w2ui-node .w2ui-node-image, [id$='settings'][style*='display: block'] div.w2ui-node.w2ui-selected + div.w2ui-node-sub > .w2ui-node .w2ui-node-image")]
        private IList<IWebElement> selectedSubNodeImageList;

        [FindsBy(How = How.CssSelector, Using = "[id$='browser-filterButton']")]
        private IWebElement filterButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='browser-mapFilterButton']")]
        private IWebElement mapSearchButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='browser-closeButton']")]
        private IWebElement closeButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='browser-show-button']")]
        private IWebElement showTreeButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='browser-content'] div.w2ui-node.w2ui-selected div.w2ui-node-sub-caption.w2ui-node-caption")]
        private IWebElement selectedNodeDevicesNumber;

        [FindsBy(How = How.CssSelector, Using = "[id$='browser-content'] div.w2ui-node.w2ui-selected div.w2ui-node-image, [id$='settings'][style*='display: block'] div.w2ui-node.w2ui-selected div.w2ui-node-image")]
        private IWebElement selectedNodeImage;

        #region Advanced Search

        [FindsBy(How = How.CssSelector, Using = "[id$='browser-treeview-search-advanced-button']")]
        private IWebElement expandSearchButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='browser-treeview-search-field']")]
        private IWebElement searchTextInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='browser-treeview-search-field'].select2-container")]
        private IWebElement searchFieldDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id$='browser-treeview-search-button']")]
        private IWebElement searchButton;

        [FindsBy(How = How.CssSelector, Using = "div[id$='browser-treeview-search-attribute-field']")]
        private IWebElement attributeDropDown;

        [FindsBy(How = How.CssSelector, Using = "div[id$='browser-treeview-search-operator-field']")]
        private IWebElement operatorDropDown;

        #endregion //Advanced Search

        #endregion //IWebElements

        #region Constructor

        public GeozoneTreeMainPanel(IWebDriver driver, PageBase page)
            : base(driver, page)
        {
            PageFactory.InitElements(this, new RetryingElementLocator(driver, TimeSpan.FromMilliseconds(Settings.LocatorRetryingTimeout), TimeSpan.FromMilliseconds(Settings.LocatorPollingInterval)));

            WaitForPanelLoaded();
        }

        #endregion //Constructor

        #region Properties

        public SearchResultsGeozonePanel SearchResultsGeozonePanel
        {
            get
            {
                if (_geozoneSearchResultPanel == null)
                {
                    _geozoneSearchResultPanel = new SearchResultsGeozonePanel(this.Driver, this.Page);
                }

                return _geozoneSearchResultPanel;
            }
        }

        public FiltersPanel FiltersPanel
        {
            get
            {
                if (_filtersPanel == null)
                {
                    _filtersPanel = new FiltersPanel(this.Driver, this.Page);
                }

                return _filtersPanel;
            }
        }

        public MapSearchPanel MapSearchPanel
        {
            get
            {
                if (_mapSearchPanel == null)
                {
                    _mapSearchPanel = new MapSearchPanel(this.Driver, this.Page);
                }

                return _mapSearchPanel;
            }
        }

        #endregion

        #region Basic methods

        #region Actions

        /// <summary>
        /// Click 'Filter' button
        /// </summary>
        public void ClickFilterButton()
        {
            filterButton.ClickEx();
        }

        /// <summary>
        /// Click 'MapSearch' button
        /// </summary>
        public void ClickMapSearchButton()
        {
            mapSearchButton.ClickEx();
        }

        /// <summary>
        /// Click 'Close' button
        /// </summary>
        public void ClickCloseButton()
        {
            closeButton.ClickEx();
        }

        /// <summary>
        /// Click 'ShowTree' button
        /// </summary>
        public void ClickShowTreeButton()
        {
            showTreeButton.ClickEx();
        }

        #region Advanced Search

        /// <summary>
        /// Click 'Expand' button
        /// </summary>
        public void ClickExpandSearchButton()
        {
            if (WebDriverContext.CurrentDriver.FindElements(By.CssSelector("[id$='browser-treeview-search-advanced-button'] .icon-down")).Count > 0)
            {
                expandSearchButton.ClickEx();
            }
            Wait.ForElementsDisplayed(By.CssSelector("[id$='browser-treeview-search-advanced-button'] .icon-up"));
        }

        /// <summary>
        /// Click 'Collapse' button
        /// </summary>
        public void ClickCollapseSearchButton()
        {
            if (WebDriverContext.CurrentDriver.FindElements(By.CssSelector("[id$='browser-treeview-search-advanced-button'] .icon-up")).Count > 0)
            {
                expandSearchButton.ClickEx();
            }
            Wait.ForElementsDisplayed(By.CssSelector("[id$='browser-treeview-search-advanced-button'] .icon-down"));
        }

        /// <summary>
        /// Enter a value for 'NameSearch' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterSearchTextInput(string value)
        {
            searchTextInput.Enter(value);
        }

        /// <summary>
        /// Select an item of 'SearchField' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectSearchFieldDropDown(string value)
        {
            searchFieldDropDown.Select(value);
        }

        /// <summary>
        /// Click 'Search' button
        /// </summary>
        public void ClickSearchButton()
        {
            searchButton.ClickEx();
        }

        /// <summary>
        /// Select an item of 'Attribute' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectAttributeDropDown(string value)
        {
            attributeDropDown.Select(value);
        }

        /// <summary>
        /// Select an item of 'Operation' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectOperatorDropDown(string value)
        {
            operatorDropDown.Select(value);
        }

        #endregion //Advanced Search

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

        #region Advanced Search

        /// <summary>
        /// Get 'NameSearch' input value
        /// </summary>
        /// <returns></returns>
        public string GetSearchTextValue()
        {
            return searchTextInput.Value();
        }

        /// <summary>
        /// Get 'SearchField' input value
        /// </summary>
        /// <returns></returns>
        public string GetSearchFieldValue()
        {
            return searchFieldDropDown.Text;
        }

        /// <summary>
        /// Get 'Attribute' input value
        /// </summary>
        /// <returns></returns>
        public string GetAttributeValue()
        {
            return attributeDropDown.Text;
        }

        /// <summary>
        /// Get 'Operation' input value
        /// </summary>
        /// <returns></returns>
        public string GetOperationValue()
        {
            return operatorDropDown.Text;
        }

        #endregion //Advanced Search

        #endregion //Get methods

        #endregion //Basic methods

        #region Business methods

        /// <summary>
        /// Select a node on GeoZones tree with a specific text name or routing path (ie. Real Time Control Area\Telematics 01)
        /// </summary>
        /// <param name="nodePath"></param>
        /// <param name="useGLMap"></param>
        public void SelectNode(string nodePath)
        {  
            var nodeNames = nodePath.SplitEx(new string[] { @"\" });
            for (var i = 0; i < nodeNames.Count; i++)
            {
                var retry = 0;
                WebDriverContext.Wait.Until(driver =>
                {                    
                    try
                    {
                        var node = nodesList.FirstOrDefault(p => p.Text.SplitAndGetAt(0).Equals(nodeNames[i]));
                        WaitForPreviousActionComplete();
                        node.ClickEx();
                        WaitForPreviousActionComplete();

                        if(selectedNode.Text != nodeNames[i])
                        {
                            node = nodesList.FirstOrDefault(p => p.Text.SplitAndGetAt(0).Equals(nodeNames[i]));
                            node.ClickEx();
                            WaitForPreviousActionComplete();
                        }

                        if (Driver.FindElements(By.CssSelector("div.mapboxgl-map")).Count > 0)
                        {
                            Wait.ForGLMapStopFlying();
                        }

                        return true;
                    }
                    catch (StaleElementReferenceException)
                    {
                        return false;
                    }
                    catch (NullReferenceException)
                    {
                        if (retry == 3)
                            Assert.Warn(string.Format("'{0}' does not exist !", nodeNames[i]));
                        retry++;
                        return false;
                    }
                    catch (UnhandledAlertException)
                    {
                        SLVHelper.AllowSecurityAlert();
                        return true;
                    }
                });
            }            
        }

        /// <summary>
        ///  Expand a node on GeoZones tree
        /// </summary>
        /// <param name="nodeName"></param>
        public void ExpandNode(string nodeName)
        {
            var retry = 0;
            WebDriverContext.Wait.Until(driver =>
            {
                try
                {
                    var node = nodesList.FirstOrDefault(p => p.Text.SplitAndGetAt(0).Equals(nodeName));
                    WaitForPreviousActionComplete();
                    var expandIcon = node.FindElement(By.CssSelector(".w2ui-node-dots .w2ui-expand"));
                    expandIcon.ClickEx();
                    Wait.ForMilliseconds(500);
                    return true;
                }
                catch (StaleElementReferenceException)
                {
                    return false;
                }
                catch (NullReferenceException)
                {
                    if (retry == 3)
                        Assert.Warn(string.Format("'{0}' does not exist !", nodeName));
                    retry++;
                    return false;
                }
                catch (UnhandledAlertException)
                {
                    SLVHelper.AllowSecurityAlert();
                    return true;
                }
            });
        }       

        /// <summary>
        /// Get current selected node text (inlcude devices count)
        /// </summary>
        /// <returns></returns>
        public string GetSelectedNodeText()
        {
            if (Driver.FindElements(By.CssSelector("[id$='browser-content'] div.w2ui-node.w2ui-selected, [id$='settings'][style*='display: block'] div.w2ui-node.w2ui-selected")).Count > 0)
                return selectedNode.Text;
            return string.Empty;
        }

        /// <summary>
        /// Get current selected node name
        /// </summary>
        /// <returns></returns>
        public string GetSelectedNodeName()
        {
            if (Driver.FindElements(By.CssSelector("[id$='browser-content'] div.w2ui-node.w2ui-selected, [id$='settings'][style*='display: block'] div.w2ui-node.w2ui-selected")).Count > 0)
                return selectedNode.Text.SplitAndGetAt(0);
            return string.Empty;
        }

        /// <summary>
        /// Get devices count of current selected GeoZone
        /// </summary>
        /// <returns></returns>
        public int GetSelectedNodeDevicesCount()
        {
            var devicesText = selectedNodeDevicesNumber.Text;
            if (devicesText == "no device") return 0;
            var devicesRegex = Regex.Match(devicesText, @"(\d*) device*");
            var devices = int.Parse(devicesRegex.Groups[1].Value);

            return devices;
        }

        /// <summary>
        /// Get device count text of current selected GeoZone
        /// </summary>
        /// <returns></returns>
        public string GetSelectedNodeDevicesCountText()
        {
            return selectedNodeDevicesNumber.Text;
        }

        /// <summary>
        ///  Get device count text of specific node name
        /// </summary>
        /// <param name="nodeName"></param>
        /// <returns></returns>
        public string GetNodeDevicesCountText(string nodeName)
        {
            List<string> nodeNameList = new List<string>();
            var node = nodesList.FirstOrDefault(p => p.Text.SplitAndGetAt(0).Equals(nodeName));            

            return node.FindElement(By.CssSelector(".w2ui-node-sub-caption.w2ui-node-caption")).Text;
        }

        /// <summary>
        /// Get devices count of specific node name
        /// </summary>
        /// <param name="nodeName"></param>
        /// <returns></returns>
        public int GetNodeDevicesCount(string nodeName)
        {
            var devicesText = GetNodeDevicesCountText(nodeName);
            if (devicesText == "no device") return 0;
            var devicesRegex = Regex.Match(devicesText, @"(\d*) device*");
            var devices = int.Parse(devicesRegex.Groups[1].Value);

            return devices;
        }

        public string GetSelectedNodeGeozonePath(bool includedRootGeozone = true)
        {
            var listGeozones = new List<string>();

            var parentNode = selectedNode.GetParentElement().GetPreviousElement();
            var parentName = parentNode.Text.SplitAndGetAt(0);
            listGeozones.Add(parentName);
            while (!parentNode.Text.SplitAndGetAt(0).Equals(Settings.RootGeozoneName))
            {
                parentNode = parentNode.GetParentElement().GetPreviousElement();
                parentName = parentNode.Text.SplitAndGetAt(0);
                listGeozones.Add(parentName);
            }
            if (!includedRootGeozone)
                listGeozones.Remove(Settings.RootGeozoneName);
            listGeozones.Reverse();

            return string.Join(@"\", listGeozones.ToArray());
        }

        public string GetSelectedNodeDevicePath(bool includedRootGeozone = true)
        {
            var nodes = new List<string>();
            var parentNode = selectedNode.GetParentElement().GetPreviousElement();
            var parentName = parentNode.Text.SplitAndGetAt(0);
            nodes.Add(parentName);
            while (!parentNode.Text.SplitAndGetAt(0).Equals(Settings.RootGeozoneName))
            {
                parentNode = parentNode.GetParentElement().GetPreviousElement();
                parentName = parentNode.Text.SplitAndGetAt(0);
                nodes.Add(parentName);
            }
            if (!includedRootGeozone)
                nodes.Remove(Settings.RootGeozoneName);
            nodes.Reverse();
            nodes.Add(GetSelectedNodeName());

            return string.Join(@"\", nodes.ToArray());
        }

        public string GetSelectedNodeParentGeozone()
        {
            var listGeozones = new List<string>();

            var parentNode = selectedNode.GetParentElement().GetPreviousElement();
            var parentName = parentNode.Text.SplitAndGetAt(0);

            return parentName;
        }

        /// <summary>
        /// Get all nodes of selected geozone
        /// </summary>
        /// <returns></returns>
        public List<string> GetChildNodeNamesOfSelectedNode()
        {
            List<string> nodeList = new List<string>();

            if (IsSelectedNodeHasChildNodes())
            {
                selectedSubNodeTextList.WaitForReady();
                foreach (var subNode in selectedSubNodeTextList)
                {
                    nodeList.Add(subNode.Text);
                }
            }

            return nodeList;
        }

        /// <summary>
        /// Get all node of selected geozone by node type
        /// </summary>
        /// <returns></returns>
        public List<string> GetChildNodeNamesOfSelectedNode(NodeType nodeType = NodeType.All)
        {
            var nodeTypeClass = string.Empty;

            switch (nodeType)
            {
                case NodeType.GeoZone:
                    nodeTypeClass = "icon-geozone";
                    break;

                case NodeType.Streetlight:
                    nodeTypeClass = "icon-device-streetlight";
                    break;

                case NodeType.Controller:
                    nodeTypeClass = "icon-device-controllerdevice";
                    break;

                case NodeType.Switch:
                    nodeTypeClass = "icon-device-switch";
                    break;

                case NodeType.ElectricalCounter:
                    nodeTypeClass = "icon-device-electricalCounter";
                    break;

                case NodeType.AudioPlayer:
                    nodeTypeClass = "icon-device-audioPlayer";
                    break;

                case NodeType.CameraIp:
                    nodeTypeClass = "icon-device-cameraip";
                    break;

                case NodeType.Building:
                    nodeTypeClass = "icon-device-building";
                    break;

                case NodeType.ParkingPlace:
                    nodeTypeClass = "icon-device-parkingPlace";
                    break;

                case NodeType.Tank:
                    nodeTypeClass = "icon-device-tank";
                    break;

                case NodeType.WasteContainer:
                    nodeTypeClass = "icon-device-wasteContainer";
                    break;

                case NodeType.WeatherStation:
                    nodeTypeClass = "icon-device-weatherStation";
                    break;

                case NodeType.CabinetController:
                    nodeTypeClass = "icomoon-cabinet-controller";
                    break;

                default:
                    break;
            }

            List<string> nodeList = new List<string>();

            if (IsSelectedNodeHasChildNodes())
            {
                selectedSubNodeList.WaitForReady();
                foreach (var subNode in selectedSubNodeList)
                {
                    if (nodeType == NodeType.All)
                    {
                        nodeList.Add(subNode.FindElement(By.CssSelector(".w2ui-node-caption:first-child")).Text);
                    }
                    else
                    {
                        if (subNode.FindElement(By.CssSelector(".w2ui-node-image-ltr")).GetAttribute("class").Contains(nodeTypeClass))
                        {
                            nodeList.Add(subNode.FindElement(By.CssSelector(".w2ui-node-caption:first-child")).Text);
                        }
                    }
                }
            }

            return nodeList;
        }        

        /// <summary>
        /// Get all device nodes of selected geozone
        /// </summary>
        /// <returns></returns>
        public List<string> GetChildDeviceNamesOfSelectedNode()
        {
            var allNodes = GetChildNodeNamesOfSelectedNode();
            var geozoneNodes = GetChildNodeNamesOfSelectedNode(NodeType.GeoZone);
            return allNodes.Except(geozoneNodes).ToList();
        }

        /// <summary>
        /// Get all child nodes of specific node Name
        /// </summary>
        /// <param name="nodeName"></param>
        /// <returns></returns>
        public List<string> GetChildNodeNames(string nodeName)
        {
            List<string> nodeNameList = new List<string>();
            var node = nodesList.FirstOrDefault(p => p.Text.SplitAndGetAt(0).Equals(nodeName));
            var subNodes = node.GetNextElement().FindElements(By.CssSelector(".w2ui-node"));
            foreach (var subNode in subNodes)
            {
                nodeNameList.Add(subNode.Text);
            }

            return nodeNameList;
        }

        /// <summary>
        /// Get all child nodes of specific node Name by node type
        /// </summary>
        /// <param name="nodeName"></param>
        /// <param name="nodeType"></param>
        /// <returns></returns>
        public List<string> GetChildNodeNames(string nodeName, NodeType nodeType = NodeType.All)
        {
            var nodeTypeClass = string.Empty;

            switch (nodeType)
            {
                case NodeType.GeoZone:
                    nodeTypeClass = "icon-geozone";
                    break;

                case NodeType.Streetlight:
                    nodeTypeClass = "icon-device-streetlight";
                    break;

                case NodeType.Controller:
                    nodeTypeClass = "icon-device-controllerdevice";
                    break;

                case NodeType.Switch:
                    nodeTypeClass = "icon-device-switch";
                    break;

                case NodeType.ElectricalCounter:
                    nodeTypeClass = "icon-device-electricalCounter";
                    break;

                case NodeType.AudioPlayer:
                    nodeTypeClass = "icon-device-audioPlayer";
                    break;

                case NodeType.CameraIp:
                    nodeTypeClass = "icon-device-cameraip";
                    break;

                case NodeType.Building:
                    nodeTypeClass = "icon-device-building";
                    break;

                case NodeType.ParkingPlace:
                    nodeTypeClass = "icon-device-parkingPlace";
                    break;

                case NodeType.Tank:
                    nodeTypeClass = "icon-device-tank";
                    break;

                case NodeType.WasteContainer:
                    nodeTypeClass = "icon-device-wasteContainer";
                    break;

                case NodeType.WeatherStation:
                    nodeTypeClass = "icon-device-weatherStation";
                    break;

                case NodeType.CabinetController:
                    nodeTypeClass = "icomoon-cabinet-controller";
                    break;

                default:
                    break;
            }

            List<string> nodeNameList = new List<string>();
            var node = nodesList.FirstOrDefault(p => p.Text.SplitAndGetAt(0).Equals(nodeName));
            var subNodes = node.GetNextElement().FindElements(By.CssSelector(".w2ui-node"));
            foreach (var subNode in subNodes)
            {
                if (nodeType == NodeType.All)
                {
                    nodeNameList.Add(subNode.FindElement(By.CssSelector(".w2ui-node-caption:first-child")).Text);
                }
                else
                {
                    if (subNode.FindElement(By.CssSelector(".w2ui-node-image-ltr")).GetAttribute("class").Contains(nodeTypeClass))
                    {
                        nodeNameList.Add(subNode.FindElement(By.CssSelector(".w2ui-node-caption:first-child")).Text);
                    }
                }
            }

            return nodeNameList;
        }

        /// <summary>
        /// Count nodes in a selected geozone based on specific node type
        /// </summary>
        /// <param name="nodeType"></param>
        /// <returns></returns>
        public long CountNodesOfSelectedGeozone(NodeType nodeType = NodeType.All)
        {
            var nodeTypeClass = string.Empty;

            switch (nodeType)
            {
                case NodeType.GeoZone:
                    nodeTypeClass = "icon-geozone";
                    break;

                case NodeType.Streetlight:
                    nodeTypeClass = "icon-device-streetlight";
                    break;

                case NodeType.Controller:
                    nodeTypeClass = "icon-device-controllerdevice";
                    break;

                default:
                    break;
            }

            var count = nodeType == NodeType.All ? selectedSubNodeImageList.Count : selectedSubNodeImageList.Count(el => el.GetAttribute("class").Contains(nodeTypeClass));

            return count;
        }

        /// <summary>
        /// Get the node type of selected node
        /// </summary>
        /// <returns></returns>
        public NodeType GetSelectedNodeType()
        {
            var selectedCssClass = selectedNodeImage.GetAttribute("class");

            if (selectedCssClass.Contains("icon-geozone"))
                return NodeType.GeoZone;
            else if (selectedCssClass.Contains("icon-device-building"))
                return NodeType.Building;
            else if (selectedCssClass.Contains("icon-device-cameraip"))
                return NodeType.CameraIp;
            else if (selectedCssClass.Contains("icon-device-cityObject"))
                return NodeType.CityObject;
            else if (selectedCssClass.Contains("icon-device-controllerdevice"))
                return NodeType.Controller;
            else if (selectedCssClass.Contains("icon-device-electricalCounter"))
                return NodeType.ElectricalCounter;
            else if (selectedCssClass.Contains("icon-device-input"))
                return NodeType.Input;
            else if (selectedCssClass.Contains("icon-device-nature"))
                return NodeType.Nature;
            else if (selectedCssClass.Contains("icon-device-networkComponent"))
                return NodeType.NetworkComponent;
            else if (selectedCssClass.Contains("icon-device-occupancySensor"))
                return NodeType.OccupancySensor;
            else if (selectedCssClass.Contains("icon-device-output"))
                return NodeType.Output;
            else if (selectedCssClass.Contains("icon-device-parkingPlace"))
                return NodeType.ParkingPlace;
            else if (selectedCssClass.Contains("icon-device-streetlight"))
                return NodeType.Streetlight;
            else if (selectedCssClass.Contains("icon-device-switch"))
                return NodeType.Switch;
            else if (selectedCssClass.Contains("icon-device-tank"))
                return NodeType.Tank;
            else if (selectedCssClass.Contains("icon-device-transportSignage"))
                return NodeType.TransportSignage;
            else if (selectedCssClass.Contains("icon-device-vehicle"))
                return NodeType.Vehicle;
            else if (selectedCssClass.Contains("icon-device-vehicleChargingStation"))
                return NodeType.VehicleChargingStation;
            else if (selectedCssClass.Contains("icon-device-wasteContainer"))
                return NodeType.WasteContainer;
            else if (selectedCssClass.Contains("icon-device-weatherStation"))
                return NodeType.WeatherStation;
            else if (selectedCssClass.Contains("icon-device-envSensor"))
                return NodeType.EnvSensor;
            else if (selectedCssClass.Contains("icomoon-cabinet-controller"))
                return NodeType.CabinetController;
            return NodeType.Unknown;
        }

        /// <summary>
        /// Get all devices count text of geozones displayed on tree
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetDevicesCountTextOfGeozones()
        {
            var result = new Dictionary<string, string>();
            nodesList.WaitForReady();
            foreach (var node in nodesList)
            {
                var geozoneName = node.Text.SplitAndGetAt(0);
                if (!result.ContainsKey(geozoneName))
                {
                    result.Add(geozoneName, node.Text.SplitAndGetAt(1));
                }
            }

            return result;
        }

        /// <summary>
        /// Check if panel is visible
        /// </summary>
        /// <returns></returns>
        public bool IsPanelVisible()
        {
            return closeButton.Displayed;
        }

        /// <summary>
        /// Hide geozone tree by setting its opacity to 0
        /// </summary>
        public void SetGeozoneTreeContainerOpacityToZero()
        {
            geozoneTreeContainer.SetOpacity("0");
        }

        /// <summary>
        /// Unhide geozone tree by setting its opacity to default value
        /// </summary>
        public void SetGeozoneTreeContainerOpacityToDefault()
        {
            geozoneTreeContainer.SetOpacity("1");
        }

        /// <summary>
        /// Move a node to a geozone
        /// </summary>
        /// <param name="sourceNode"></param>
        /// <param name="destGeozone"></param>
        public void MoveNodeToGeozone(string sourceNode, string destGeozone)
        {
            var sourceElement = nodesList.FirstOrDefault(node => node.Text.Contains(sourceNode));
            var destElement = nodesList.FirstOrDefault(node => node.Text.Contains(destGeozone));

            JSUtility.DragAndDropByJS(sourceElement, destElement);
        }

        /// <summary>
        /// Check if search bar is visible
        /// </summary>
        /// <returns></returns>
        public bool IsSearchBarVisible()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("div[id$='browser-treeview-search'].slv-search-box"));
        }

        /// <summary>
        /// Check if Filter button is visible
        /// </summary>
        /// <returns></returns>
        public bool IsFilterButtonVisible()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='browser-filterButton']"));
        }

        /// <summary>
        /// Check if Map Filter button is visible
        /// </summary>
        /// <returns></returns>
        public bool IsMapFilterButtonVisible()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='browser-mapFilterButton']"));
        }

        /// <summary>
        /// Check if Map Search Panel displayed
        /// </summary>
        /// <returns></returns>
        public bool IsMapSearchPanelDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='browser-mapfilter'].treeview-filter-editor"));
        }

        /// <summary>
        /// Check if Devices count text is visible
        /// </summary>
        /// <returns></returns>
        public bool IsDevicesCountVisible()
        {
            return Driver.FindElements(By.CssSelector("[id$='browser-content'] div.w2ui-node div.w2ui-node-sub-caption.w2ui-node-caption")).Count > 0;
        }

        /// <summary>
        /// Get all items of Attribute DropDown
        /// </summary>
        public List<string> GetAllAttributeDropDownItems()
        {
            return attributeDropDown.GetAllItems();
        }

        /// <summary>
        /// Check if a selected geozone has any child nodes
        /// </summary>
        /// <returns></returns>
        public bool IsSelectedNodeHasChildNodes()
        {
            return Driver.FindElements(By.CssSelector("[id$='browser-content'] div.w2ui-node.w2ui-selected + div.w2ui-node-sub > .w2ui-node, [id$='settings'][style*='display: block'] div.w2ui-node.w2ui-selected + div.w2ui-node-sub > .w2ui-node")).Count > 0;
        }

        /// <summary>
        /// To check if a geozone has child nodes no matter what it is being expanded or not
        /// </summary>
        /// <returns></returns>
        public bool IsSelectedGeozoneExpandable()
        {
            return Driver.FindElements(By.CssSelector("[id$='browser-content'] div.w2ui-node.w2ui-selected div.w2ui-expand.w2ui-expand-plus, [id$='browser-content'] div.w2ui-node.w2ui-selected div.w2ui-expand.w2ui-expand-minus, [id$='settings'][style*='display: block'] div.w2ui-node.w2ui-selected div.w2ui-expand.w2ui-expand-plus, [id$='settings'][style*='display: block'] div.w2ui-node.w2ui-selected div.w2ui-expand.w2ui-expand-minus")).Count > 0;
        }

        /// <summary>
        /// To check if a node is visible on geozone tree
        /// </summary>
        /// <returns></returns>
        public bool IsNodeVisible(string name)
        {
            var visibleNodesList = JSUtility.GetElementsText("[id$='browser-content'] div.w2ui-node .w2ui-node-caption:not(.w2ui-node-sub-caption)");
            
            return visibleNodesList.Exists(p => p.Equals(name));
        }

        /// <summary>
        /// To check if a selected node has Expand icon
        /// </summary>
        /// <returns></returns>
        public bool IsSelectedNodeHasExpandIcon()
        {
            return Driver.FindElements(By.CssSelector("[id$='browser-content'] div.w2ui-node.w2ui-selected .w2ui-node-dots .w2ui-expand.w2ui-expand-plus")).Count > 0;
        }

        /// <summary>
        /// To check if a node has Expand icon
        /// </summary>
        /// <returns></returns>
        public bool IsNodeHasExpandIcon(string nodeName)
        {
            List<string> nodeNameList = new List<string>();
            var node = nodesList.FirstOrDefault(p => p.Text.SplitAndGetAt(0).Equals(nodeName));

            return node.FindElements(By.CssSelector(".w2ui-node-dots .w2ui-expand.w2ui-expand-plus")).Count > 0;
        }

        /// <summary>
        /// Check if geozone has selected node
        /// </summary>
        /// <returns></returns>
        public bool HasSelectedNode()
        {
            return Driver.FindElements(By.CssSelector("[id$='browser-content'] div.w2ui-node.w2ui-selected, [id$='settings'][style*='display: block'] div.w2ui-node.w2ui-selected")).Count > 0;
        }

        public void WaitForSearchResultPanelDisplayed()
        {
            Wait.ForElementStyle(By.CssSelector("[id$='browser-treeview-search-result'].slv-search-result"), "left: 0px");
        }

        public void WaitForSearchResultPanelDisappeared()
        {
            Wait.ForElementStyle(By.CssSelector("[id$='browser-treeview-search-result'].slv-search-result"), "left: -350px");
        }

        public void WaitForFilterPanelDisplayed()
        {
            Wait.ForElementStyle(By.CssSelector("[id$='browser-filter'].treeview-filter-editor"), "left: 0px");
        }

        public void WaitForFilterPanelDisappeared()
        {
            Wait.ForElementStyle(By.CssSelector("[id$='browser-filter'].treeview-filter-editor"), "left: -350px");
        }

        public void WaitForMapSearchPanelDisplayed()
        {
            Wait.ForElementStyle(By.CssSelector("[id$='browser-mapfilter'].treeview-filter-editor"), "left: 0px");
        }

        public void WaitForMapSearchPanelDisappeared()
        {
            Wait.ForElementStyle(By.CssSelector("[id$='browser-mapfilter'].treeview-filter-editor"), "left: -350px");
        }

        public bool IsSearchResultPanelDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='browser-treeview-search-result'].slv-search-result"));
        }

        public void WaitForSelectedNodeText(string text)
        {
            WebDriverContext.Wait.Until(ExpectedConditions.TextToBePresentInElement(selectedNode, text));
        }

        public void ChangeSearchAttribute(string attribute, string operation = "")
        {
            ClickExpandSearchButton();
            if (!attribute.Equals(GetAttributeValue().Trim()))
            {
                SelectAttributeDropDown(attribute);               
            }
            if (!string.IsNullOrEmpty(operation) && !operation.Equals(GetOperationValue().Trim()))
            {
                SelectOperatorDropDown(operation);
            }
        }

        public int GetExpandedNodesCount()
        {
            return Driver.FindElements(By.CssSelector("[id$='browser-content'] .w2ui-node .w2ui-expand-minus")).Count;
        }

        public void HoverMapSearchButton()
        {
            mapSearchButton.MoveTo();
        }

        public string GetMapSearchButtonTooltip()
        {
           return mapSearchButton.GetAttribute("title");
        }

        public void ClickSelectedNodeHasExpandIcon()
        {
            Driver.FindElement(By.CssSelector("[id$='browser-content'] div.w2ui-node.w2ui-selected .w2ui-node-dots .w2ui-expand")).ClickEx();
            Wait.ForMilliseconds(500);
        }

        #endregion //Business methods

        public override void WaitForPanelLoaded()
        {           
            Wait.ForElementDisplayed(By.CssSelector("[id$='browser-content'] .treeview-close-button"));
        }
    }
}
