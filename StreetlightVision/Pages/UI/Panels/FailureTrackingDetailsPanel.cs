using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using StreetlightVision.Extensions;
using StreetlightVision.Models;
using StreetlightVision.Utilities;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace StreetlightVision.Pages.UI
{
    public class FailureTrackingDetailsPanel : PanelBase
    {
        #region Variables

        #endregion //Variables

        #region IWebElements

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-tracking-buttons-toolbar_item_close'] table.w2ui-button")]
        private IWebElement closeButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-tracking'] div.failuretracking-gl-editor-title-label")]
        private IWebElement panelTitle;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-tracking-content-category-icon']")]
        private IWebElement deviceIcon;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-tracking-content-name']")]
        private IWebElement deviceNameValueLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-tracking-GeozonePath'] div.failuretracking-gl-editor-list-item-label")]
        private IWebElement geozonePathLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-tracking-GeozonePath'] div.failuretracking-gl-editor-list-item-value")]
        private IWebElement geozonePathValueLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-tracking-Unique address'] div.failuretracking-gl-editor-list-item-label")]
        private IWebElement uniqueAdressLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-tracking-Unique address'] div.failuretracking-gl-editor-list-item-value")]
        private IWebElement uniqueAdressValueLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-tracking-Address 1'] div.failuretracking-gl-editor-list-item-label")]
        private IWebElement adress1Label;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-tracking-Address 1'] div.failuretracking-gl-editor-list-item-value")]
        private IWebElement adress1ValueLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-tracking-City'] div.failuretracking-gl-editor-list-item-label")]
        private IWebElement cityLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-tracking-City'] div.failuretracking-gl-editor-list-item-value")]
        private IWebElement cityValueLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-tracking-mainFrame']")]
        private IList<IWebElement> historyFailuresItemList;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-tracking-content-history-message']")]
        private IWebElement historyFailuresMessageLabel;

        #endregion //IWebElements

        #region Constructor

        public FailureTrackingDetailsPanel(IWebDriver driver, PageBase page)
            : base(driver, page)
        {
            PageFactory.InitElements(this, new RetryingElementLocator(driver, TimeSpan.FromMilliseconds(Settings.LocatorRetryingTimeout), TimeSpan.FromMilliseconds(Settings.LocatorPollingInterval)));
        }

        #endregion //Constructor

        #region Basic methods   

        #region Actions

        /// <summary>
        /// Click 'Close' button
        /// </summary>
        public void ClickCloseButton()
        {
            closeButton.ClickEx();
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
        /// Get 'DeviceIcon' input value
        /// </summary>
        /// <returns></returns>
        public string GetDeviceIconValue()
        {
            return deviceIcon.IconValue();
        }

        /// <summary>
        /// Get 'DeviceNameValue' label text
        /// </summary>
        /// <returns></returns>
        public string GetDeviceNameValueText()
        {
            return deviceNameValueLabel.Text;
        }

        /// <summary>
        /// Get 'GeozonePath' label text
        /// </summary>
        /// <returns></returns>
        public string GetGeozonePathText()
        {
            return geozonePathLabel.Text;
        }

        /// <summary>
        /// Get 'GeozonePathValue' label text
        /// </summary>
        /// <returns></returns>
        public string GetGeozonePathValueText()
        {
            return geozonePathValueLabel.Text;
        }

        /// <summary>
        /// Get 'UniqueAdress' label text
        /// </summary>
        /// <returns></returns>
        public string GetUniqueAdressText()
        {
            return uniqueAdressLabel.Text;
        }

        /// <summary>
        /// Get 'UniqueAdressValue' label text
        /// </summary>
        /// <returns></returns>
        public string GetUniqueAdressValueText()
        {
            return uniqueAdressValueLabel.Text;
        }

        /// <summary>
        /// Get 'UniqueAdress1' label text
        /// </summary>
        /// <returns></returns>
        public string GetAdress1Text()
        {
            return adress1Label.Text;
        }

        /// <summary>
        /// Get 'UniqueAdress1Value' label text
        /// </summary>
        /// <returns></returns>
        public string GetAdress1ValueText()
        {
            return adress1ValueLabel.Text;
        }

        /// <summary>
        /// Get 'City' label text
        /// </summary>
        /// <returns></returns>
        public string GetCityText()
        {
            return cityLabel.Text;
        }

        /// <summary>
        /// Get 'CityValue' label text
        /// </summary>
        /// <returns></returns>
        public string GetCityValueText()
        {
            return cityValueLabel.Text;
        }

        /// <summary>
        /// Get 'HistoryFailuresMessage' label text
        /// </summary>
        /// <returns></returns>
        public string GetHistoryFailuresMessageText()
        {
            return historyFailuresMessageLabel.Text;
        }

        #endregion //Get methods

        #endregion //Basic methods

        public List<string> GetListOfFailureName()
        {
            return JSUtility.GetElementsText("[id$='editor-tracking-content-history-table'] [id$='editor-tracking-mainFrame'] .slv-label:nth-child(2)");
        }

        public List<dynamic> GetListOfFailures()
        {
            var failureList = new List<dynamic>();

            foreach (var failureElement in historyFailuresItemList)
            {
                var failureIconElement = failureElement.FindElement(By.CssSelector("div:nth-child(1)"));
                var failureNameElement = failureElement.FindElement(By.CssSelector("div:nth-child(2)"));
                var failureTimeElement = failureElement.FindElement(By.CssSelector("div:nth-child(3)"));

                dynamic failure = new ExpandoObject();
                var iconClass = failureIconElement.GetAttribute("class");
                failure.Icon = iconClass.Contains("icon-warning") ? "icon-warning" : iconClass.Contains("icon-status-ok") ? "icon-status-ok" : iconClass.Contains("icon-error") ? "icon-error" : string.Empty;
                failure.Name = failureNameElement.Text.Trim();
                failure.Time = failureTimeElement.Text.Trim();

                failureList.Add(failure);
            }

            return failureList;
        }

        #region Business methods

        /// <summary>
        /// Check if panel is visible
        /// </summary>
        /// <returns></returns>
        public bool IsPanelVisible()
        {
            return closeButton.Displayed;
        }

        public string GetDeviceIconImageUrl()
        {
            return deviceIcon.ImageUrl();
        }

        public bool IsHistoryFailuresMessageDisplayed()
        {
            return historyFailuresMessageLabel.Displayed;
        }

        public bool HasIconDevice(DeviceType deviceType)
        {
            var icon = Driver.FindElement(By.CssSelector("[id$='editor-tracking-content-category-icon']"));
            
            if(deviceType == DeviceType.CabinetController)
                return icon.GetAttribute("class").Contains("icomoon-cabinet-controller");

            return false;
        }

        #endregion //Business methods
    }
}