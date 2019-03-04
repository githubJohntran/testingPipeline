using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using StreetlightVision.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using StreetlightVision.Utilities;
using System.Data;
using NUnit.Framework;

namespace StreetlightVision.Pages.UI
{
    public class SearchResultsGeozonePanel : PanelBase
    {
        #region Variables

        #endregion //Variables

        #region IWebElements

        [FindsBy(How = How.CssSelector, Using = "[id$='browser-treeview-search-result-backButton'")]
        private IWebElement backButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='browser-treeview-search-result-title'] div.slv-search-title-label")]
        private IWebElement panelTitle;

        [FindsBy(How = How.CssSelector, Using = "[id$='search-result-message']")]
        private IWebElement searchResultMessageLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='browser-treeview-search-result-grid_body']")]
        private IWebElement searchResultContentGrid;

        [FindsBy(How = How.CssSelector, Using = "[id$='browser-treeview-search-result-grid_records'] tr[id^='grid'][id*='rec']")]
        private IList<IWebElement> gridRecordsList;        

        [FindsBy(How = How.CssSelector, Using = "[id$='browser-treeview-search-result-filterButton']")]
        private IWebElement mapFilterButton;

        #endregion //IWebElements

        #region Constructor

        public SearchResultsGeozonePanel(IWebDriver driver, PageBase page)
            : base(driver, page)
        {
            PageFactory.InitElements(this, new RetryingElementLocator(driver, TimeSpan.FromMilliseconds(Settings.LocatorRetryingTimeout), TimeSpan.FromMilliseconds(Settings.LocatorPollingInterval)));

            WaitForPanelLoaded();
        }

        #endregion //Constructor

        #region Basic methods

        #region Actions

        /// <summary>
        /// Click 'Back' button
        /// </summary>
        public void ClickBackButton()
        {
            backButton.ClickEx();
        }

        /// <summary>
        /// Click 'MapFilter' button
        /// </summary>
        public void ClickMapFilterButton()
        {
            mapFilterButton.ClickEx();
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
        /// Get 'SearchResultMessage' label text
        /// </summary>
        /// <returns></returns>
        public string GetSearchResultMessageText()
        {
            return searchResultMessageLabel.Text;
        }

        #endregion //Get methods

        #endregion //Basic methods

        #region Business methods

        /// <summary>
        /// Hide the panel itself
        /// </summary>
        public void Hide()
        {
            ClickBackButton();
        }

        /// <summary>
        /// Check if search result message is displayed
        /// </summary>
        /// <returns></returns>
        public bool IsSearchResultMessageDisplayed()
        {
            return searchResultMessageLabel.Displayed;
        }

        /// <summary>
        /// Check if search result content is displayed
        /// </summary>
        /// <returns></returns>
        public bool IsSearchResultContentFoundDisplayed()
        {
            return searchResultContentGrid.BuildDataTableFromGrid().Rows.Count > 0;
        }        

        /// <summary>
        /// Select the first result in the grid
        /// </summary>
        public List<string> SelectFirstFoundDevice()
        {
            if (!gridRecordsList.Any()) Assert.Warn("*** There is no records found !");
            var firstRecord = gridRecordsList.FirstOrDefault();
            firstRecord.ClickEx();

            return firstRecord.Text.SplitEx();
        }

        /// <summary>
        /// Select the specific name of result in the grid 
        /// </summary>
        /// <param name="name"></param>
        public void SelectFoundDevice(string name)
        {
            var currentRec = gridRecordsList.FirstOrDefault(p => p.Text.SplitAndGetAt(0).Equals(name));
            if (currentRec == null) Assert.Warn("*** There is no record '{0}' found !", name);
            currentRec.ClickEx();
        }                

        /// <summary>
        /// Select random result in the grid
        /// </summary>
        /// <returns></returns>
        public List<string> SelectRandomFoundDevice()
        {
            if (!gridRecordsList.Any()) Assert.Warn("*** There is no records found !");
            var randomRecord = gridRecordsList.Shuffle().FirstOrDefault();
            randomRecord.ClickEx();

            return randomRecord.Text.SplitEx();
        }

        /// <summary>
        /// Get data of a column of search result
        /// </summary>
        /// <returns></returns>
        public List<string> GetListOfSearchResult(string columnName)
        {
            var dtResult = searchResultContentGrid.BuildDataTableFromGrid();
            return dtResult.AsEnumerable().Select(p => p.Field<string>(columnName)).ToList();
        }

        /// <summary>
        /// Get equiment name of selected row in found grid
        /// </summary>
        /// <returns></returns>
        public string GetSelectedEquipmentName()
        {
            var nameElement = Driver.FindElement(By.CssSelector("[id$='browser-treeview-search-result-grid_body'] .w2ui-selected td.w2ui-grid-data[col='1'] div"));
            return nameElement.Text.Trim();
        }

        /// <summary>
        /// Get geozone name of selected row in found grid
        /// </summary>
        /// <returns></returns>
        public string GetSelectedGeozoneName()
        {
            var goeonzeElement = Driver.FindElement(By.CssSelector("[id$='browser-treeview-search-result-grid_body'] .w2ui-selected td.w2ui-grid-data[col='2'] div"));
            return goeonzeElement.Text.Trim();
        }

        /// <summary>
        /// Check whether map filter button is visible
        /// </summary>
        /// <returns></returns>
        public bool IsMapFilterButtonVisible()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='browser-treeview-search-result-filterButton']"));
        }

        public bool IsFilterResultsOnMapTurnedOn()
        {
            return Driver.FindElements(By.CssSelector("[id$='browser-treeview-search-result-filterButton'].icon-map-filter-active")).Count > 0;
        }

        public void WaitForFilterResultsOnMapTurnedOn()
        {
            Wait.ForElementDisplayed(By.CssSelector("[id$='browser-treeview-search-result-filterButton'].icon-map-filter-active"));
        }

        public void WaitForFilterResultsOnMapTurnedOff()
        {
            Wait.ForElementInvisible(By.CssSelector("[id$='browser-treeview-search-result-filterButton'].icon-map-filter-active"));
        }

        #endregion //Business methods

        public void WaitForPanelClosed()
        {
            Wait.ForElementStyle(By.CssSelector("[id$='browser-treeview-search-result']"), "left: -350px");
        }

        public override void WaitForPanelLoaded()
        {
            WaitForPreviousActionComplete();
            Wait.ForElementStyle(By.CssSelector("[id$='browser-treeview-search-result']"), "left: 0px");
        }
    }
}
