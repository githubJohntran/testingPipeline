using NUnit.Framework;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using StreetlightVision.Extensions;
using StreetlightVision.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StreetlightVision.Pages.UI
{
    public class UserTreePanel : PanelBase
    {
        #region Variables

        #endregion //Variables

        #region IWebElements

        [FindsBy(How = How.CssSelector, Using = "[id$='treeview-content'] div.w2ui-node")]
        private IList<IWebElement> nodesList;

        [FindsBy(How = How.CssSelector, Using = "[id$='treeview-content'] div.w2ui-node.w2ui-selected")]
        private IWebElement selectedNode;

        [FindsBy(How = How.CssSelector, Using = "[id$='left-panel-w2ui_panel_top'] div.slv-panel-driven-layout-left-panel-title")]
        private IWebElement panelTitle;        

        #endregion //IWebElements

        #region Constructor

        public UserTreePanel(IWebDriver driver, PageBase page)
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

        /// <summary>
        /// Get 'PanelTitle' text
        /// </summary>
        /// <returns></returns>
        public string GetPanelTitleText()
        {
            return panelTitle.Text;
        }

        #endregion //Get methods

        #endregion //Basic methods

        #region Business methods

        /// <summary>
        /// Select a node on User tree with a specific text name or routing path (ie. Profile 1\userA)
        /// </summary>
        /// <param name="nodePath"></param>
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
        /// Get current selected node
        /// </summary>
        /// <returns></returns>
        public string GetSelectedNodeText()
        {
            throw new NotImplementedException();
        }

        #endregion //Business methods

        public override void WaitForPanelLoaded()
        {
            Wait.ForElementText(panelTitle);
        }

        public override void WaitForPreviousActionComplete()
        {
            base.WaitForPreviousActionComplete();
        }
    }
}
