using ImageMagick;
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
    public class InformationWidgetPanel : PanelBase
    {
        #region Variables

        #endregion //Variables

        #region IWebElements

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] .verso-div .widget-bottom-title > .device-image, [id$='widgetPanel'] [id$='verso'] [id$='title'] > img")]
        private IWebElement deviceIcon;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] .verso-div .widget-bottom-title > .device-name, [id$='widgetPanel'] [id$='verso'] [id$='title'] > span")]
        private IWebElement deviceNameLabel;

        #endregion //IWebElements

        #region Streetlight

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] [id$='verso'] [id$='picture'] > img")]
        private IWebElement streetlightImage;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] .verso-div .streetlight-info:nth-child(1) .streetlight-info-label")]
        private IWebElement identifierLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] .verso-div .streetlight-info:nth-child(1) .streetlight-info-value")]
        private IWebElement identifierValueLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] .verso-div .streetlight-info:nth-child(2) .streetlight-info-label")]
        private IWebElement modelLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] .verso-div .streetlight-info:nth-child(2) .streetlight-info-value")]
        private IWebElement modelValueLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] .verso-div .streetlight-info:nth-child(3) .streetlight-info-label")]
        private IWebElement uniqueAddressLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] .verso-div .streetlight-info:nth-child(3) .streetlight-info-value")]
        private IWebElement uniqueAddressValueLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] .verso-div .streetlight-info:nth-child(4) .streetlight-info-label")]
        private IWebElement dimmingGroupLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] .verso-div .streetlight-info:nth-child(4) .streetlight-info-value")]
        private IWebElement dimmingGroupValueLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] .verso-div .streetlight-info:nth-child(5) .streetlight-info-label")]
        private IWebElement powerLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] .verso-div .streetlight-info:nth-child(5) .streetlight-info-value")]
        private IWebElement powerValueLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] .verso-div .streetlight-info:nth-child(6) .streetlight-info-label")]
        private IWebElement lampInstallDateLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] .verso-div .streetlight-info:nth-child(6) .streetlight-info-value")]
        private IWebElement lampInstallDateValueLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] .verso-div .streetlight-info:nth-child(7) .streetlight-info-label")]
        private IWebElement addressLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] .verso-div .streetlight-info:nth-child(7) .streetlight-info-value")]
        private IWebElement addressValueLabel;

        #endregion //Streetlight

        #region Controller

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] [id$='verso'] [id$='host']")]
        private IWebElement hostLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] [id$='verso'] [id$='systemTime'][style*='display: block;'][style*='top: 40px;'] > div:nth-child(1)")]
        private IWebElement groupControlLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] [id$='verso'] [id$='systemTime'][style*='display: block;'][style*='top: 40px;'] > div:nth-child(2) .controller-editor-panel:nth-child(1) > .slv-label")]
        private IWebElement groupControlSelectAGroupLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] [id$='verso'] [id$='systemTime'][style*='display: block;'][style*='top: 40px;'] > div:nth-child(2) .controller-editor-panel:nth-child(2) > .slv-label")]
        private IWebElement groupControlSelectACommandLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] [id$='verso'] [id*='s2id'][id$='group-field'].select2-container")]
        private IWebElement selectGroupDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] [id$='verso'] [id*='s2id'][id$='command-field'].select2-container")]
        private IWebElement selectCommandDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] [id$='verso'] button.icon-play.controller-command-button")]
        private IWebElement executeCommandButton;        

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] [id$='verso'] [id$='systemTimeRefreshButton']")]
        private IWebElement dateTimeRefreshButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] [id$='verso'] [id$='systemTimeSynchronizeButton']")]
        private IWebElement dateTimeSynchronizeButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] [id$='verso'] [id$='timeZoneRefreshButton']")]
        private IWebElement timezoneRefreshButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] [id$='verso'] [id$='timeZoneSunriseButton']")]
        private IWebElement timezoneSunriseSunsetButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] [id$='verso'] [id$='sendDatalogsButton']")]
        private IWebElement sendDatalogsButton;

        #endregion //Controller

        #region Constructor

        public InformationWidgetPanel(IWebDriver driver, PageBase page)
            : base(driver, page)
        {
            PageFactory.InitElements(this, new RetryingElementLocator(driver, TimeSpan.FromMilliseconds(Settings.LocatorRetryingTimeout), TimeSpan.FromMilliseconds(Settings.LocatorPollingInterval)));
            WaitForPanelLoaded();
        }

        #endregion //Constructor

        #region Basic methods

        #region Actions

        #region Streetlight

        #endregion //Streetlight

        #region Controller

        /// <summary>
        /// Select an item of 'SelectGroup' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectSelectGroupDropDown(string value)
        {
            selectGroupDropDown.Select(value, true);
        }

        /// <summary>
        /// Select an item of 'SelecCommand' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectSelecCommandDropDown(string value)
        {
            selectCommandDropDown.Select(value, true);
        }

        /// <summary>
        /// Click 'ExecuteCommand' button
        /// </summary>
        public void ClickExecuteCommandButton()
        {
            executeCommandButton.ClickEx();
        }

        /// <summary>
        /// Click 'DateTimeRefresh' button
        /// </summary>
        public void ClickDateTimeRefreshButton()
        {
            dateTimeRefreshButton.ClickEx();
        }

        /// <summary>
        /// Click 'DateTimeSynchronize' button
        /// </summary>
        public void ClickDateTimeSynchronizeButton()
        {
            dateTimeSynchronizeButton.ClickEx();
        }

        /// <summary>
        /// Click 'TimezoneRefresh' button
        /// </summary>
        public void ClickTimezoneRefreshButton()
        {
            timezoneRefreshButton.ClickEx();
        }

        /// <summary>
        /// Click 'TimezoneSunriseSunset' button
        /// </summary>
        public void ClickTimezoneSunriseSunsetButton()
        {
            timezoneSunriseSunsetButton.ClickEx();
        }

        /// <summary>
        /// Click 'SendDatalogs' button
        /// </summary>
        public void ClickSendDatalogsButton()
        {
            sendDatalogsButton.ClickEx();
        }

        #endregion //Controller

        #endregion //Actions

        #region Get methods

        /// <summary>
        /// Get 'DeviceIcon' input value
        /// </summary>
        /// <returns></returns>
        public string GetDeviceIconValue()
        {
            return deviceIcon.IconValue();
        }

        /// <summary>
        /// Get 'DeviceName' label text
        /// </summary>
        /// <returns></returns>
        public string GetDeviceNameText()
        {
            return deviceNameLabel.Text;
        }


        #region Streetlight

        /// <summary>
        /// Get 'StreetlightImage' input value
        /// </summary>
        /// <returns></returns>
        public string GetStreetlightImageValue()
        {
            return streetlightImage.ImageValue();
        }

        /// <summary>
        /// Get 'Identifier' label text
        /// </summary>
        /// <returns></returns>
        public string GetIdentifierText()
        {
            return identifierLabel.Text;
        }

        /// <summary>
        /// Get 'IdentifierValue' label text
        /// </summary>
        /// <returns></returns>
        public string GetIdentifierValueText()
        {
            return identifierValueLabel.Text;
        }

        /// <summary>
        /// Get 'Model' label text
        /// </summary>
        /// <returns></returns>
        public string GetModelText()
        {
            return modelLabel.Text;
        }

        /// <summary>
        /// Get 'ModelValue' label text
        /// </summary>
        /// <returns></returns>
        public string GetModelValueText()
        {
            return modelValueLabel.Text;
        }

        /// <summary>
        /// Get 'UniqueAddress' label text
        /// </summary>
        /// <returns></returns>
        public string GetUniqueAddressText()
        {
            return uniqueAddressLabel.Text;
        }

        /// <summary>
        /// Get 'UniqueAddressValue' label text
        /// </summary>
        /// <returns></returns>
        public string GetUniqueAddressValueText()
        {
            return uniqueAddressValueLabel.Text;
        }

        /// <summary>
        /// Get 'DimmingGroup' label text
        /// </summary>
        /// <returns></returns>
        public string GetDimmingGroupText()
        {
            return dimmingGroupLabel.Text;
        }

        /// <summary>
        /// Get 'DimmingGroupValue' label text
        /// </summary>
        /// <returns></returns>
        public string GetDimmingGroupValueText()
        {
            return dimmingGroupValueLabel.Text;
        }

        /// <summary>
        /// Get 'Power' label text
        /// </summary>
        /// <returns></returns>
        public string GetPowerText()
        {
            return powerLabel.Text;
        }

        /// <summary>
        /// Get 'PowerValue' label text
        /// </summary>
        /// <returns></returns>
        public string GetPowerValueText()
        {
            return powerValueLabel.Text;
        }

        /// <summary>
        /// Get 'LampInstallDate' label text
        /// </summary>
        /// <returns></returns>
        public string GetLampInstallDateText()
        {
            return lampInstallDateLabel.Text;
        }

        /// <summary>
        /// Get 'LampInstallDateValue' label text
        /// </summary>
        /// <returns></returns>
        public string GetLampInstallDateValueText()
        {
            return lampInstallDateValueLabel.Text;
        }

        /// <summary>
        /// Get 'Address' label text
        /// </summary>
        /// <returns></returns>
        public string GetAddressText()
        {
            return addressLabel.Text;
        }

        /// <summary>
        /// Get 'AddressValue' label text
        /// </summary>
        /// <returns></returns>
        public string GetAddressValueText()
        {
            return addressValueLabel.Text;
        }

        #endregion //Streetlight

        #region Controller

        /// <summary>
        /// Get 'Host' label text
        /// </summary>
        /// <returns></returns>
        public string GetHostText()
        {
            return hostLabel.Text;
        }

        /// <summary>
        /// Get 'GroupControl' label text
        /// </summary>
        /// <returns></returns>
        public string GetGroupControlText()
        {
            return groupControlLabel.Text;
        }

        /// <summary>
        /// Get 'GroupControlSelectAGroupLabel' label text
        /// </summary>
        /// <returns></returns>
        public string GetGroupControlSelectAGroupText()
        {
            return groupControlSelectAGroupLabel.Text;
        }

        /// <summary>
        /// Get 'GroupControlSelectACommandLabel' label text
        /// </summary>
        /// <returns></returns>
        public string GetGroupControlSelectACommandText()
        {
            return groupControlSelectACommandLabel.Text;
        }

        /// <summary>
        /// Get 'SelectGroup' dropdown value
        /// </summary>
        /// <returns></returns>
        public string GetSelectGroupValue()
        {
            return selectGroupDropDown.Text;
        }

        /// <summary>
        /// Get 'SelectCommand' dropdown value
        /// </summary>
        /// <returns></returns>
        public string GetSelectCommandValue()
        {
            return selectCommandDropDown.Text;
        }

        #endregion //Controller

        #endregion //Get methods

        #endregion //Basic methods

        #region Business methods

        public void ClickDeviceIcon()
        {
            deviceIcon.ClickEx();
        }

        public byte[] GetDeviceIconBytes()
        {
            var url = deviceIcon.GetAttribute("src");

            return SLVHelper.DownloadFileData(url);
        }

        #region Controller

        public bool IsHostNameDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='widgetPanel'] [id$='verso'] [id$='host']"));
        }

        public bool IsExecuteButtonDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='widgetPanel'] [id$='verso'] button.icon-play.controller-command-button"));
        }       

        public List<string> GetDimmingGroupItems()
        {
            return selectGroupDropDown.GetAllItems();
        }

        public List<string> GetCommandItems()
        {
            return selectCommandDropDown.GetAllItems();
        }        
       
        public bool CheckIfDeviceIcon(DeviceType device)
        {
            var expectedIcon = new MagickImage(device.GetIconBytes());
            var actualIcon = new MagickImage(GetDeviceIconBytes());
            var result = expectedIcon.Compare(actualIcon, ErrorMetric.Absolute);

            return result == 0;
        }

        #endregion //Controller

        #endregion //Business methods

    }
}
