using NUnit.Framework;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using StreetlightVision.Extensions;
using StreetlightVision.Models;
using StreetlightVision.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;

namespace StreetlightVision.Pages.UI
{
    public class MapSearchPanel : PanelBase
    {
        #region Variables

        #endregion //Variables

        #region IWebElements

        [FindsBy(How = How.CssSelector, Using = "[id$='browser-mapfilter-title'] .back-button")]
        private IWebElement backToolbarButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='browser-mapfilter-title'] .title-label")]
        private IWebElement panelTitle;

        [FindsBy(How = How.CssSelector, Using = "[id$='browser-mapfilter-content'] [id='slv-map-filter-input-container'] input")]
        private IWebElement searchInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='browser-mapfilter-content'] [id='slv-map-filter-input-container'] .geocoder-icon-close")]
        private IWebElement clearSearchButton;

        #endregion //IWebElements

        #region Constructor

        public MapSearchPanel(IWebDriver driver, PageBase page) : base(driver, page)
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

        /// <summary>
        /// Enter a value for 'Search' input
        /// </summary>
        public void EnterSearchInput(string value)
        {
            searchInput.Enter(value);
        }

        /// <summary>
        /// Click 'ClearSearch' button
        /// </summary>
        public void ClickClearSearchButton()
        {
            clearSearchButton.ClickEx();
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
        /// Get 'Search' input value
        /// </summary>
        /// <returns></returns>
        public string GetSearchValue()
        {
            return searchInput.Value();
        }

        /// <summary>
        /// Get 'Search' input placeholder
        /// </summary>
        /// <returns></returns>
        public string GetSearchPlaceholder()
        {
            return searchInput.GetAttribute("placeholder");
        }

        #endregion //Get methods

        #endregion //Basic methods

        #region Business methods

        public string GetContentText()
        {
            return JSUtility.GetElementText("[id$='browser-mapfilter-content'] label");
        }

        public bool IsBackButtonDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='browser-mapfilter-title'] .back-button"));
        }

        public bool IsSearchInputDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='browser-mapfilter-content'] [id='slv-map-filter-input-container']"));
        }

        public bool IsSearchInputHasMagnifyingGlass()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='browser-mapfilter-content'] [id='slv-map-filter-input-container'] .geocoder-icon-search"));
        }

        public List<string> GetSearchSuggestionsBoldText()
        {
            return JSUtility.GetElementsText("[id$='browser-mapfilter-content'] [id='slv-map-filter-input-container'] .suggestions li > a > strong");
        }

        public List<string> GetSearchSuggestionsText()
        {
            return JSUtility.GetElementsText("[id$='browser-mapfilter-content'] [id='slv-map-filter-input-container'] .suggestions li > a");
        }

        public void WaitForSuggestionsDisplayed()
        {
            Wait.ForElementStyle(By.CssSelector("[id$='browser-mapfilter-content'] [id='slv-map-filter-input-container'] .suggestions"), "display: block");
        }

        public void WaitForSuggestionsDisappeared()
        {
            Wait.ForElementStyle(By.CssSelector("[id$='browser-mapfilter-content'] [id='slv-map-filter-input-container'] .suggestions"), "display: none");
        }
        
        public void SelectSearchSuggestion()
        {
            var suggestions = Driver.FindElements(By.CssSelector("[id$='browser-mapfilter-content'] [id='slv-map-filter-input-container'] .suggestions li"));
            if (!suggestions.Any()) Assert.Warn("There is no suggestions in Map Search");
            suggestions.FirstOrDefault().ClickEx();
        }

        public void WaitForClearSearchButtonDisappeared()
        {
            Wait.ForElementInvisible(By.CssSelector("[id$='browser-mapfilter-content'] [id='slv-map-filter-input-container'] .geocoder-icon-close"));
        }        

        public List<string> GetListOfMessages()
        {
            return JSUtility.GetElementsText("[id='message-block'] [data-block='message'] p");
        }

        public string GetClosetDevicesTitle()
        {
            return JSUtility.GetElementText("[id='message-block'] [data-block='devices'] label");
        }

        public string GetErrorsTitle()
        {
            return JSUtility.GetElementText("[id='message-block'] [data-block='error'] label");
        }        

        public List<string> GetListOfErrors()
        {
            return JSUtility.GetElementsText("[id='message-block'] [data-block='error'] p");
        }

        public DataTable BuildClosetDevicesDataTable()
        {
            var gridContainer = Driver.FindElement(By.CssSelector("[id$='mapfilter-device-grid_body'].w2ui-grid-body"));
            DataTable tblResult = gridContainer.BuildDataTableFromGrid();

            return tblResult;
        }

        public List<string> GetListOfColumnDataClosetDevices(string columnName)
        {
            DataTable tblGrid = BuildClosetDevicesDataTable();
            var result = new List<string>();

            if (tblGrid.Columns.Contains(columnName))
            {
                result = tblGrid.AsEnumerable().Select(r => r.Field<string>(columnName)).ToList();
            }

            return result;
        }

        public List<string> GetListOfClosetDevicesImage()
        {
            var result = new List<string>();
            var imageElements = Driver.FindElements(By.CssSelector("[id$='mapfilter-device-grid_body'] .record-device .record-device-image"));

            foreach (var imageElement in imageElements)
            {
                var ulr = imageElement.GetBackgroundImageUrl();
                result.Add(ulr);
            }

            return result;
        }

        public List<string> GetListOfColumnsHeaderClosetDevices()
        {
            var gridContainer = Driver.FindElement(By.CssSelector("[id$='mapfilter-device-grid_body']"));
            var results = new List<string>();
            var columnHeaders = gridContainer.GetGridHeaders() as IEnumerable;

            foreach (var header in columnHeaders)
            {
                var headerText = header as string;
                headerText = headerText.TrimEx();
                if (!string.IsNullOrEmpty(headerText))
                    results.Add(headerText);
            }

            return results.Distinct().ToList();
        }

        public void ClickGridColumnHeader(string columnName)
        {
            var columns = Driver.FindElements(By.CssSelector("td.w2ui-head .w2ui-col-header"));
            var column = columns.FirstOrDefault(p => p.Text.TrimEx().Equals(columnName));
            if (column != null)
                column.ClickEx();
            else
                Assert.Warn(string.Format("Cannot click on column '{0}'", columnName));
        }

        public void ClickGridRecord(string text)
        {
            var gridRecordsList = Driver.FindElements(By.CssSelector("[id$='mapfilter-device-grid_body'] tr[id^='grid'][id*='rec']"));
            WebDriverContext.Wait.Until(driver =>
            {
                try
                {
                    var currentRec = gridRecordsList.FirstOrDefault(p => p.Text.IndexOf(text) >= 0);
                    currentRec.ClickEx();

                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            });
        }

        #endregion //Business methods
    }
}
