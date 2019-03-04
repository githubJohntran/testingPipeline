using NUnit.Framework;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using StreetlightVision.Extensions;
using StreetlightVision.Models;
using StreetlightVision.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StreetlightVision.Pages.UI
{
    public class GeozoneTreeWidgetPanel : PanelBase
    {
        #region Variables
        
        #endregion //Variables

        #region IWebElements

        [FindsBy(How = How.CssSelector, Using = "[id$='settings'][style*='display: block']")]
        private IWebElement geozoneTreeContainer;

        [FindsBy(How = How.CssSelector, Using = "[id$='settings'][style*='display: block'] div.side-panel-title-label")]
        private IWebElement panelTitle;

        [FindsBy(How = How.CssSelector, Using = "[id$='settings'][style*='display: block'] div.w2ui-node")]
        private IList<IWebElement> nodesList;

        [FindsBy(How = How.CssSelector, Using = "[id$='settings'][style*='display: block'] div.w2ui-node.w2ui-selected")]
        private IWebElement selectedNode;

        [FindsBy(How = How.CssSelector, Using = "[id$='settings'][style*='display: block'] div.w2ui-node.w2ui-selected + div.w2ui-node-sub > .w2ui-node")]
        private IList<IWebElement> selectedSubNodeList;

        [FindsBy(How = How.CssSelector, Using = "[id$='settings'][style*='display: block'] div.w2ui-node.w2ui-selected + div.w2ui-node-sub > .w2ui-node .w2ui-node-caption:first-child")]
        private IList<IWebElement> selectedSubNodeTextList;

        [FindsBy(How = How.CssSelector, Using = "[id$='settings'][style*='display: block'] div.w2ui-node.w2ui-selected + div.w2ui-node-sub > .w2ui-node .w2ui-node-image")]
        private IList<IWebElement> selectedSubNodeImageList;

        [FindsBy(How = How.CssSelector, Using = "[id$='settings'][style*='display: block'] div[id$='settings-timezones']")]
        private IWebElement timezonesDropDown;
        
        [FindsBy(How = How.CssSelector, Using = "[id$='settings'][style*='display: block'] div[id$='settings-backButton']")]
        private IWebElement backButton;

        #endregion //IWebElements

        #region Constructor

        public GeozoneTreeWidgetPanel(IWebDriver driver, PageBase page)
            : base(driver, page)
        {
            PageFactory.InitElements(this, new RetryingElementLocator(driver, TimeSpan.FromMilliseconds(Settings.LocatorRetryingTimeout), TimeSpan.FromMilliseconds(Settings.LocatorPollingInterval)));

            WaitForPanelLoaded();
        }

        #endregion //Constructor

        #region Properties

        #endregion

        #region Basic methods

        #region Actions     

        /// <summary>
        /// Select an item of 'Timezones' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectTimezonesDropDown(string value)
        {
            timezonesDropDown.Select(value);
        }

        /// <summary>
        /// Click 'Back' button
        /// </summary>
        public void ClickBackButton()
        {
            backButton.ClickEx();
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
        /// Get 'Timezones' dropdown value
        /// </summary>
        /// <returns></returns>
        public string GetTimezonesValue()
        {
            return timezonesDropDown.Text;
        }

        #endregion //Get methods

        #endregion //Basic methods

        #region Business methods

        /// <summary>
        /// Select a node on GeoZones tree with a specific text name or routing path (ie. Real Time Control Area\Smartsim)
        /// </summary>
        /// <param name="nodePath"></param>
        public void SelectNode(string nodePath)
        {
            var nodeNames = nodePath.SplitEx(new string[] { @"\" });

            for (var i = 0; i < nodeNames.Count; i++)
            {                
                WebDriverContext.Wait.Until(driver =>
                {
                    try
                    {
                        var node = nodesList.FirstOrDefault(p => p.Text.SplitAndGetAt(0).Equals(nodeNames[i]));
                        if (i == nodeNames.Count - 1)
                        {
                            node.ClickEx();
                            return true;
                        }
                        var byExpand = By.CssSelector(".w2ui-expand.w2ui-expand-ltr.w2ui-expand-plus");
                        if (node.FindElements(byExpand).Any())
                        {
                            var expand = node.FindElement(byExpand);
                            expand.ClickEx();
                        }
                        return true;
                    }
                    catch (Exception ex)
                    {
                        return false;
                    }
                });
            }
        }

        /// <summary>
        /// Get current selected node
        /// </summary>
        /// <returns></returns>
        public IWebElement GetSelectedNode()
        {
            return selectedNode;
        }

        /// <summary>
        /// Get current selected node text (inlcude devices count)
        /// </summary>
        /// <returns></returns>
        public string GetSelectedNodeText()
        {
            if (selectedNode != null)
                return selectedNode.Text;
            return string.Empty;
        }

        /// <summary>
        /// Get current selected node name
        /// </summary>
        /// <returns></returns>
        public string GetSelectedNodeName()
        {
            if (selectedNode != null)
                return selectedNode.Text.SplitAndGetAt(0);
            return string.Empty;
        }

        /// <summary>
        /// Get all nodes of selected geozone
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllNodesOfSelectedGeozone()
        {
            List<string> nodeList = new List<string>();

            foreach (var subNode in selectedSubNodeTextList)
            {
                nodeList.Add(subNode.Text);
            }

            return nodeList;
        }

        /// <summary>
        /// Get all node of selected geozone by node type
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllNodesOfSelectedGeozone(NodeType nodeType = NodeType.All)
        {
            var nodeTypeClass = string.Empty;

            switch (nodeType)
            {
                case NodeType.GeoZone:
                    nodeTypeClass = "icon-geozone";
                    break;

                case NodeType.Streetlight:
                    nodeTypeClass = "icon-device-controllerdevice";
                    break;

                default:
                    break;
            }

            List<string> nodeList = new List<string>();

            foreach (var subNode in selectedSubNodeList)
            {
                if (nodeType == NodeType.All)
                {
                    nodeList.Add(subNode.FindElement(By.CssSelector(".w2ui-node-caption:first-child")).Text);
                }
                else
                {
                    if (subNode.FindElement(By.CssSelector(".w2ui-node-image")).GetAttribute("class").Contains(nodeTypeClass))
                    {
                        nodeList.Add(subNode.FindElement(By.CssSelector(".w2ui-node-caption:first-child")).Text);
                    }
                }
            }

            return nodeList;
        }

        /// <summary>
        /// Get all device nodes of selected geozone
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllDeviceNodesOfSelectedGeozone()
        {
            var allNodes = GetAllNodesOfSelectedGeozone();
            var geozoneNodes = GetAllNodesOfSelectedGeozone(NodeType.GeoZone);
            return allNodes.Except(geozoneNodes).ToList();
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
                    nodeTypeClass = "icon-device-controllerdevice";
                    break;

                default:
                    break;
            }

            var count = nodeType == NodeType.All ? selectedSubNodeImageList.Count : selectedSubNodeImageList.Count(el => el.GetAttribute("class").Contains(nodeTypeClass));

            return count;
        }

        public bool IsBackButtonDisplayed()
        {
            return backButton.Displayed;
        }

        public bool IsTimezonesDropDownDisplayed()
        {
            return timezonesDropDown.Displayed;
        }

        public bool IsGeozoneTreeDisplayed()
        {
            return geozoneTreeContainer.Displayed;
        }

        public void SelectRandomTimezoneDropDown()
        {
            var currentValue = GetTimezonesValue();
            var listItems = timezonesDropDown.GetAllItems();
            listItems.Remove(currentValue);
            timezonesDropDown.Select(listItems.PickRandom());
        }

        #endregion //Business methods

        public override void WaitForPanelLoaded()
        {
            Wait.ForElementsDisplayed(nodesList);
        }

        public override void WaitForPreviousActionComplete()
        {
            base.WaitForPreviousActionComplete();
        }
    }
}
