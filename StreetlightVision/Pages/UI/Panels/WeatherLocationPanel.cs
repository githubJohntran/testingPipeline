using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using StreetlightVision.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StreetlightVision.Pages.UI
{
    public class WeatherLocationPanel : PanelBase
    {
        #region Variables
        
        #endregion //Variables

        #region IWebElements        

        [FindsBy(How = How.CssSelector, Using = "[id$='settings'][id*='weather'][style*='display: block'] div.side-panel-title-label")]
        private IWebElement panelTitle;

        [FindsBy(How = How.CssSelector, Using = "[id$='settings'][id*='weather'][style*='display: block'] div.weatherSettingLabel.slv-label")]
        private IWebElement cityLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='settings'][id*='weather'][style*='display: block'] input[id='weather-settings-location']")]
        private IWebElement locationInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='settings'][id*='weather'][style*='display: block'] button[id='weather-settings-search-button']")]
        private IWebElement searchButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='settings'][id*='weather'][style*='display: block'] [id='weather-settings-list'] > span")]
        private IWebElement locationFoundLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='settings'][id*='weather'][style*='display: block'] [id='weather-settings-list'] a.weatherSettingsAddress")]
        private IList<IWebElement> addressList;

        #endregion //IWebElements

        #region Constructor

        public WeatherLocationPanel(IWebDriver driver, PageBase page)
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
        /// Focus Location input
        /// </summary>
        public void FocusLocationInput()
        {
            locationInput.ClickEx();
        }

        /// <summary>
        /// Enter a value for 'Location' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterLocationInput(string value)
        {
            locationInput.Enter(value);
        }

        /// <summary>
        /// Click 'Search' button
        /// </summary>
        public void ClickSearchButton()
        {
            searchButton.ClickEx();
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
        /// Get 'City' label text
        /// </summary>
        /// <returns></returns>
        public string GetCityText()
        {
            return cityLabel.Text;
        }

        /// <summary>
        /// Get 'Location' input value
        /// </summary>
        /// <returns></returns>
        public string GetLocationValue()
        {
            return locationInput.Value();
        }

        /// <summary>
        /// Get 'LocationFound' label text
        /// </summary>
        /// <returns></returns>
        public string GetLocationFoundText()
        {
            return locationFoundLabel.Text;
        }

        #endregion //Get methods

        #endregion //Basic methods

        #region Business methods
        
        public void WaitForSearchResultDisplayed()
        {
            Wait.ForElementsDisplayed(addressList);
        }

        public void WaitForSearchResultDisappeared()
        {
            Wait.ForElementInvisible(By.CssSelector("[id$='settings'][id*='weather'][style*='display: block'] [id='weather-settings-list'] > span"));
        }

        public List<string> GetAddressListText()
        {
            return addressList.Select(p => p.Text).ToList();
        }

        /// <summary>
        /// Select the first address in list search result
        /// </summary>
        public void SelectFirstAddress()
        {
            var firstAddress = addressList.FirstOrDefault();
            firstAddress.ClickEx();
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
