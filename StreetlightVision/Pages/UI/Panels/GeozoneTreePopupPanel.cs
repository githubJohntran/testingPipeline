using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using StreetlightVision.Extensions;
using StreetlightVision.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace StreetlightVision.Pages.UI
{
    public class GeozoneTreePopupPanel : PanelBase
    {
        #region Variables

        #endregion //Variables

        #region IWebElements

        [FindsBy(How = How.CssSelector, Using = "[id='w2ui-popup'] div.w2ui-node")]
        private IList<IWebElement> nodesList;

        [FindsBy(How = How.CssSelector, Using = "[id='w2ui-popup'] div.w2ui-node.w2ui-selected")]
        private IWebElement selectedNode;

        [FindsBy(How = How.CssSelector, Using = "[id='w2ui-popup'] div.w2ui-node.w2ui-selected div.w2ui-node-sub-caption.w2ui-node-caption")]
        private IWebElement selectedNodeDevicesNumber;        

        #endregion //IWebElements

        #region Constructor

        public GeozoneTreePopupPanel(IWebDriver driver, PageBase page)
            : base(driver, page)
        {
            PageFactory.InitElements(this, new RetryingElementLocator(driver, TimeSpan.FromMilliseconds(Settings.LocatorRetryingTimeout), TimeSpan.FromMilliseconds(Settings.LocatorPollingInterval)));

            WaitForPanelLoaded();
        }

        #endregion //Constructor

        #region Basic methods

        #region Actions

        #endregion //Actions

        #region Get methods

        #endregion //Get methods

        #endregion //Basic methods

        #region Business methods

        /// <summary>
        /// Select a node on GeoZones tree with a specific text name or routing path (ie. Real Time Control Area\Telematics 01)
        /// </summary>
        /// <param name="name"></param>
        public void SelectNode(string nodePath)
        {
            var nodeNames = nodePath.SplitEx(new string[] { @"\" });
            for (var i = 0; i < nodeNames.Count; i++)
            {
                var node = nodesList.FirstOrDefault(p => p.Text.SplitAndGetAt(0).Equals(nodeNames[i]));
                node.ClickEx();
                if (i < nodeNames.Count - 1)
                {
                    Wait.ForSeconds(1);
                }
            }
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
        /// Get devices count of current selected GeoZone
        /// </summary>
        /// <returns></returns>
        public int GetSelectedNodeDevicesCount()
        {
            var devicesText = selectedNodeDevicesNumber.Text;
            var devicesRegex = Regex.Match(devicesText, @"(\d*) device");
            
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
        /// Check if panel is visible
        /// </summary>
        /// <returns></returns>
        public bool IsPanelVisible()
        {
            var labelBy = By.CssSelector("[id$='w2ui-popup'] .slv-customreport-popup-wizard-page-geozone-label");
            var treeViewBy = By.CssSelector("[id$='w2ui-popup'] .slv-customreport-popup-wizard-page-geozone-treeview");
            return WebDriverContext.CurrentDriver.FindElements(labelBy).Count > 0 && WebDriverContext.CurrentDriver.FindElements(treeViewBy).Count > 0;
        }

        #endregion //Business methods

        public override void WaitForPanelLoaded()
        {
            Wait.ForElementsDisplayed(nodesList);
        }
    }
}
