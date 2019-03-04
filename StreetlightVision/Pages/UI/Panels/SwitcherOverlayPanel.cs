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
    public class SwitcherOverlayPanel : PanelBase
    {
        #region Variables

        #endregion //Variables

        #region IWebElements

        [FindsBy(How = How.CssSelector, Using = "div.slv-switcher[style*='display: block']")]
        private IWebElement switcherPanel;

        [FindsBy(How = How.CssSelector, Using = "div.slv-switcher[style*='display: block'] div.slv-switcher-cancel")]
        private IWebElement switcherCancelButton;

        [FindsBy(How = How.CssSelector, Using = "div.slv-switcher[style*='display: block'] div.slv-switcher-title")]
        private IWebElement panelHeader;

        [FindsBy(How = How.CssSelector, Using = "div.slv-switcher[style*='display: block'] div.slv-switcher-tile")]
        private IList<IWebElement> switcherTileList;

        #endregion //IWebElements

        #region Constructor

        public SwitcherOverlayPanel(IWebDriver driver, PageBase page)
            : base(driver, page)
        {
            PageFactory.InitElements(this, new RetryingElementLocator(driver, TimeSpan.FromMilliseconds(Settings.LocatorRetryingTimeout), TimeSpan.FromMilliseconds(Settings.LocatorPollingInterval)));
            WaitForPanelLoaded();
        }

        #endregion //Constructor

        #region Basic methods

        #region Actions

        /// <summary>
        /// Click 'SwitcherCancel' button
        /// </summary>
        public void ClickSwitcherCancelButton()
        {
            switcherCancelButton.ClickEx();
        }

        #endregion //Actions

        #region Get methods

        /// <summary>
        /// Get 'PanelHeader' label text
        /// </summary>
        /// <returns></returns>
        public string GetPanelHeaderText()
        {
            return panelHeader.Text;
        }

        #endregion //Get methods

        #endregion //Basic methods

        #region Business methods

        public bool IsPanelDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("div.slv-switcher[style*='display: block']"));
        }

        public bool IsCloseButtonDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("div.slv-switcher[style*='display: block'] div.slv-switcher-cancel"));
        }

        public List<string> GetListOfAppsToSwitch()
        {
            return JSUtility.GetElementsText("div.slv-switcher[style*='display: block'] div.slv-switcher-tile div.tile-title");
        }

        public EquipmentInventoryPage SwitchToEquipmentInventoryApp()
        {
            foreach (var tileElement in switcherTileList)
            {
                if (tileElement.FindElement(By.CssSelector("div.tile-title")).Text == App.EquipmentInventory)
                {
                    tileElement.FindElement(By.CssSelector("[id$='content']")).ClickEx();

                    break;
                }
            }

            return new EquipmentInventoryPage(Driver);
        }

        public RealTimeControlPage SwitchToRealtimeControlApp()
        {
            foreach (var tileElement in switcherTileList)
            {
                if (tileElement.FindElement(By.CssSelector("div.tile-title")).Text == App.RealTimeControl)
                {
                    tileElement.FindElement(By.CssSelector("[id$='content']")).ClickEx();

                    break;
                }
            }

            return new RealTimeControlPage(Driver);
        }

        public DataHistoryPage SwitchToDataHistoryApp()
        {
            foreach (var tileElement in switcherTileList)
            {
                if (tileElement.FindElement(By.CssSelector("div.tile-title")).Text.ToUpper() == App.DataHistory.ToUpper())
                {
                    tileElement.FindElement(By.CssSelector("[id$='content']")).ClickEx();

                    break;
                }
            }

            return new DataHistoryPage(Driver);
        }

        public FailureTrackingPage SwitchToFailureTrackingApp()
        {
            foreach (var tileElement in switcherTileList)
            {
                if (tileElement.FindElement(By.CssSelector("div.tile-title")).Text == App.FailureTracking)
                {
                    tileElement.FindElement(By.CssSelector("[id$='content']")).ClickEx();

                    break;
                }
            }

            return new FailureTrackingPage(Driver);
        }

        #endregion //Business methods

        public override void WaitForPanelLoaded()
        {
            Wait.ForElementDisplayed(switcherPanel);
        }
    }
}
