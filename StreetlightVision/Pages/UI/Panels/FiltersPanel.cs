using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using StreetlightVision.Extensions;
using StreetlightVision.Models;
using StreetlightVision.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace StreetlightVision.Pages.UI
{
    public class FiltersPanel : PanelBase
    {
        #region Variables

        #endregion //Variables

        #region IWebElements

        [FindsBy(How = How.CssSelector, Using = "[id$='browser-filter-title'] .back-button")]
        private IWebElement backToolbarButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='browser-filter-title'] .treeview-filter-title-label")]
        private IWebElement panelTitle;
                
        [FindsBy(How = How.CssSelector, Using = "[id$='browser-filter-tabs'] tr > td > div")]
        private IList<IWebElement> tabsList;

        [FindsBy(How = How.CssSelector, Using = "[id$='browser-filter-tabs'] tr > td > div.w2ui-tab.active")]
        private IWebElement activeTab;

        [FindsBy(How = How.CssSelector, Using = "[id$='browser-filter-equipmenent-list'] div.treeview-filter-list-item")]
        private IList<IWebElement> filterTypesList;

        [FindsBy(How = How.CssSelector, Using = ".installation-status-filter .treeview-filter-list-item")]
        private IList<IWebElement> filterDeviceStatusList;

        #endregion //IWebElements

        #region Constructor

        public FiltersPanel(IWebDriver driver, PageBase page) : base(driver, page)
        {
            PageFactory.InitElements(this, new RetryingElementLocator(driver, TimeSpan.FromMilliseconds(Settings.LocatorRetryingTimeout), TimeSpan.FromMilliseconds(Settings.LocatorPollingInterval)));

            WaitForPanelLoaded();
        }

        #endregion //Constructor

        #region Basic methods

        #region Actions

        /// <summary>
        /// Click 'BackToolbar' button
        /// </summary>
        public void ClickBackToolbarButton()
        {
            backToolbarButton.ClickEx();
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
        /// Get 'ActiveTab' label text
        /// </summary>
        /// <returns></returns>
        public string GetActiveTabText()
        {
            return activeTab.Text;
        }

        #endregion //Get methods

        #endregion //Basic methods

        #region Business methods

        /// <summary>
        /// Select a tab with a specific name
        /// </summary>
        /// <param name="name"></param>        
        public void SelectTab(string name)
        {
            var tab = tabsList.FirstOrDefault(p => p.Text.Contains(name));
            if (tab != null)
            {
                tab.ClickEx();
                WebDriverContext.Wait.Until(driver => JSUtility.GetElementText("[id$='browser-filter-tabs'] div.w2ui-tab.active") == name);
            }
        }

        /// <summary>
        /// Tick devices type for filtering
        /// </summary>
        /// <param name="deviceTypes"></param>
        public void CheckTypes(params DeviceType[] deviceTypes)
        {
            foreach (var deviceType in deviceTypes)
            {
                var device = filterTypesList.FirstOrDefault(p => p.Text.Equals(deviceType.Value));
                var checkbox = device.FindElement(By.CssSelector(".treeview-filter-list-item-checkbox"));
                if (checkbox != null)
                    checkbox.Check(true);
            }
        }

        /// <summary>
        /// Tick devices type for filtering
        /// </summary>
        /// <param name="deviceTypes"></param>
        public void UncheckTypes(params DeviceType[] deviceTypes)
        {
            foreach (var deviceType in deviceTypes)
            {
                var device = filterTypesList.FirstOrDefault(p => p.Text.Equals(deviceType.Value));
                var checkbox = device.FindElement(By.CssSelector(".treeview-filter-list-item-checkbox"));
                if (checkbox != null)
                    checkbox.Check(false);
            }
        }

        /// <summary>
        /// Get all items list
        /// </summary>
        /// <returns></returns>
        public List<string> GetListOfDeviceTypes()
        {
            return filterTypesList.Select(p => p.Text).ToList();
        }

        /// <summary>
        /// Get list of device status name
        /// </summary>
        /// <returns></returns>
        public List<string> GetListOfDeviceStatusName()
        {
            return JSUtility.GetElementsText(".installation-status-filter .treeview-filter-list-item .treeview-filter-list-item-title");
        }

        /// <summary>
        /// Get list Of device status color
        /// </summary>
        /// <returns></returns>
        public List<Color> GetListOfDeviceStatusColor()
        {
            var colorsCol = Driver.FindElements(By.CssSelector(".installation-status-filter .treeview-filter-list-item .treeview-filter-list-item-box"));
            var colors = colorsCol.Select(p => p.GetStyleColorAttr("background-color")).ToList();

            return colors;
        }

        /// <summary>
        /// Get dictionary of device status (key: status name, value: color)
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, Color> GetDictionaryOfDeviceStatus()
        {
            var result = new Dictionary<string, Color>();

            foreach (var item in filterDeviceStatusList)
            {
                var name = item.FindElement(By.CssSelector(".treeview-filter-list-item-title")).Text.Trim();
                var color = item.FindElement(By.CssSelector(".treeview-filter-list-item-box")).GetStyleColorAttr("background-color");
                result.Add(name, color);
            }

            return result;
        }

        public int GetDeviceStatusCheckboxRowCount()
        {
            return Driver.FindElements(By.CssSelector(".installation-status-filter .treeview-filter-list-item .treeview-filter-list-item-checkbox input")).Count;
        }

        /// <summary>
        /// Tick devices install status for filtering
        /// </summary>
        /// <param name="deviceTypes"></param>
        public void CheckAllStatus()
        {
            foreach (var item in filterDeviceStatusList)
            {
                var checkbox = item.FindElement(By.CssSelector(".treeview-filter-list-item-checkbox"));
                if (checkbox != null)
                    checkbox.Check(true);
            }

        }
        /// <summary>
        /// Tick devices install status for filtering
        /// </summary>
        /// <param name="statuses"></param>
        public void CheckStatus(params string[] statuses)
        {
            foreach (var status in statuses)
            {
                var item = filterDeviceStatusList.FirstOrDefault(p => p.Text.Equals(status));
                var checkbox = item.FindElement(By.CssSelector(".treeview-filter-list-item-checkbox"));
                if (checkbox != null)
                    checkbox.Check(true);
            }
        }

        /// <summary>
        /// Un-tick devices install status for filtering
        /// </summary>
        /// <param name="deviceTypes"></param>
        public void UncheckAllStatus()
        {
            foreach (var item in filterDeviceStatusList)
            {
                var checkbox = item.FindElement(By.CssSelector(".treeview-filter-list-item-checkbox"));
                if (checkbox != null)
                    checkbox.Check(false);
            }
        }

        /// <summary>
        /// Un-tick devices install status for filtering
        /// </summary>
        /// <param name="deviceTypes"></param>
        public void UncheckStatus(params string[] statuses)
        {
            foreach (var status in statuses)
            {
                var item = filterDeviceStatusList.FirstOrDefault(p => p.Text.Equals(status));
                var checkbox = item.FindElement(By.CssSelector(".treeview-filter-list-item-checkbox"));
                if (checkbox != null)
                    checkbox.Check(false);
            }
        }        

        #endregion //Business methods
    }
}
