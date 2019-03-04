using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using StreetlightVision.Extensions;
using StreetlightVision.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace StreetlightVision.Pages.UI
{
    public class DeviceHistoryPanel : PanelBase
    {
        #region Variables

        #endregion //Variables

        #region IWebElements

        [FindsBy(How = How.CssSelector, Using = "[id='tb_deviceHistoryEditorButtons_item_cancel'] table.w2ui-button")]
        private IWebElement backButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='devicehistory-window-editor'] div.devicehistory-editor-title-icon")]
        private IWebElement deviceIcon;

        [FindsBy(How = How.CssSelector, Using = "[id$='devicehistory-window-editor'] div.devicehistory-editor-title-label")]
        private IWebElement deviceNameTitle;

        [FindsBy(How = How.CssSelector, Using = "[id$='devicehistory-window-editor-content-history-filter'] button.devicehistory-editor-filter-button")]
        private IWebElement filterToolbarButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='devicehistory-window-editor-content-history-filter'] button.devicehistory-editor-filter-button")]
        private IWebElement tableToolbarButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='devicehistory-window-editor-content-history-filter'] div.devicehistory-editor-filter-title")]
        private IWebElement filterTitle;

        [FindsBy(How = How.CssSelector, Using = "[id$='devicehistory-window-editor-content-history-header-select']")]
        private IWebElement durationDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id$='devicehistory-window-editor-content-history-table_toolbar_item_w2ui-reload'] table.w2ui-button")]
        private IWebElement refreshToolbarButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='devicehistory-window-editor-content-history-table_toolbar_item_w2ui-search'] div.w2ui-icon.icon-search-down.w2ui-search-down")]
        private IWebElement selectSearchFieldButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='devicehistory-window-editor-content-history-table_search_all']")]
        private IWebElement searchToolbarInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='devicehistory-window-editor-content-history-table_toolbar_item_w2ui-search-advanced'] table.w2ui-button")]
        private IWebElement searchToolbarButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='devicehistory-window-editor-content-history-table_toolbar_item_export'] table.w2ui-button")]
        private IWebElement exportToolbarButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='devicehistory-window-editor-content-history-table_toolbar_item_filter'] table.w2ui-button")]
        private IWebElement duplicatedLogsToolbarButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='devicehistory-window-editor-content-history-table_body']")]
        private IWebElement gridContainer;

        [FindsBy(How = How.CssSelector, Using = "[id^='w2ui-overlay-searches']")]
        private IWebElement advancedSearchPopup;

        [FindsBy(How = How.CssSelector, Using = "[id^='w2ui-overlay-searches'] .w2ui-grid-searches-reset-btn")]
        private IWebElement advancedSearchResetButton;

        [FindsBy(How = How.CssSelector, Using = "[id^='w2ui-overlay-searches'] .w2ui-grid-searches-search-btn")]
        private IWebElement advancedSearchSearchButton;

        [FindsBy(How = How.CssSelector, Using = "[id^='w2ui-overlay-searches'] .w2ui-grid-searches tr:not(:last-child)")]
        private IList<IWebElement> advancedSearchConditionList;

        #endregion //IWebElements

        #region Constructor

        public DeviceHistoryPanel(IWebDriver driver, PageBase page) : base(driver, page)
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
        /// Click 'FilterToolbar' button
        /// </summary>
        public void ClickFilterToolbarButton()
        {
            filterToolbarButton.ClickEx();
        }

        /// <summary>
        /// Click 'TableToolbar' button
        /// </summary>
        public void ClickTableToolbarButton()
        {
            tableToolbarButton.ClickEx();
        }

        /// <summary>
        /// Select an item of 'Duration' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectDurationDropDown(string value)
        {
            durationDropDown.Select(value);
        }

        /// <summary>
        /// Click 'RefreshToolbar' button
        /// </summary>
        public void ClickRefreshToolbarButton()
        {
            refreshToolbarButton.ClickEx();
        }

        /// <summary>
        /// Click 'SelectSearchField' button
        /// </summary>
        public void ClickSelectSearchFieldButton()
        {
            selectSearchFieldButton.ClickEx();
        }

        /// <summary>
        /// Enter a value for 'SearchToolbar' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterSearchToolbarInput(string value)
        {
            searchToolbarInput.Enter(value);
        }

        /// <summary>
        /// Click 'SearchToolbar' button
        /// </summary>
        public void ClickSearchToolbarButton()
        {
            searchToolbarButton.ClickEx();
        }

        /// <summary>
        /// Click 'AdvancedSearchReset' button
        /// </summary>
        public void ClickAdvancedSearchResetButton()
        {
            advancedSearchResetButton.ClickEx();
        }

        /// <summary>
        /// Click 'AdvancedSearchSearch' button
        /// </summary>
        public void ClickAdvancedSearchSearchButton()
        {
            advancedSearchSearchButton.ClickEx();
        }

        /// <summary>
        /// Click 'ExportToolbar' button
        /// </summary>
        public void ClickExportToolbarButton()
        {
            exportToolbarButton.ClickEx();
        }

        /// <summary>
        /// Click 'ShowDuplicatedLogsToolbar' button
        /// </summary>
        public void ClickDuplicatedLogsToolbarButton()
        {
            duplicatedLogsToolbarButton.ClickEx();
        }

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
        /// Get 'DeviceNameTitle' text
        /// </summary>
        /// <returns></returns>
        public string GetDeviceNameTitleText()
        {
            return deviceNameTitle.Text;
        }

        /// <summary>
        /// Get 'FilterTitle' text
        /// </summary>
        /// <returns></returns>
        public string GetFilterTitleText()
        {
            return filterTitle.Text;
        }

        /// <summary>
        /// Get 'Duration' dropdown value
        /// </summary>
        /// <returns></returns>
        public string GetDurationValue()
        {
            return durationDropDown.Text;
        }

        /// <summary>
        /// Get 'SearchToolbar' input value
        /// </summary>
        /// <returns></returns>
        public string GetSearchToolbarValue()
        {
            return searchToolbarInput.Value();
        }

        #endregion //Get methods

        #endregion //Basic methods

        #region Business methods

        public DataTable BuildDataTableFromGrid()
        {
            return gridContainer.BuildDataTableFromGrid();
        }

        public List<string> GetListOfColumnData(string column)
        {
            var tblGrid = BuildDataTableFromGrid();
            return tblGrid.AsEnumerable().Select(r => r.Field<string>(column)).ToList();
        }

        public List<string> GetAllDurations()
        {
            return durationDropDown.GetAllItems();
        }

        public List<string> GetListOfAdvancedSearchFields()
        {
            var script = "function f() { let lst = []; let items = document.querySelectorAll(\"[id^='w2ui-overlay-searches'] table tr td.caption\"); for(let i = 0; i < items.length; i++) { lst.push(items[i].innerText) }; return lst; }";
            var result = (IList<object>)WebDriverContext.JsExecutor.ExecuteScript(script + "return f();");

            return result.Cast<string>().ToList();
        }

        public void EnterSearchCriteriaForInputField(string field, string value)
        {
            foreach (var fieldSection in advancedSearchConditionList)
            {
                var fieldNameElement = fieldSection.FindElement(By.CssSelector("td.caption"));
                if (fieldNameElement.Text == field)
                {
                    var fieldNameInput = fieldSection.FindElement(By.CssSelector("td.value input"));
                    fieldNameInput.Enter(value);

                    return;
                }
            }
        }

        public void ClearInputFieldsInAdvancedSearchPopup()
        {
            foreach (var fieldSection in advancedSearchConditionList)
            {
                var fieldNameElement = fieldSection.FindElement(By.CssSelector("td.caption"));
                if (fieldNameElement.Text != "Time")
                {
                    var fieldNameInput = fieldSection.FindElement(By.CssSelector("td.value input"));
                    fieldNameInput.Clear();
                }
            }
        }

        public bool IsDuplicatedLogsToggleOn()
        {
            return duplicatedLogsToolbarButton.GetAttribute("class").Contains("checked");
        }

        public void WaitForAdvancedSearchPanelDisplayed()
        {
            Wait.ForElementDisplayed(advancedSearchPopup);
        }

        public void WaitForLoaderSpinDisappeared()
        {
            Wait.ForElementStyle(By.CssSelector("[id$='window-editor-spin'].devicehistory-spin"), "display: none");
        }

        public void WaitForAttributesFilterPanelDisplayed()
        {
            Wait.ForElementStyle(By.CssSelector("[id$='devicehistory-window-editor'] [id$='history-filter-content'].slv-window"), "display: block");
        }

        public void WaitForAttributesFilterPanelDisappeared()
        {
            Wait.ForElementStyle(By.CssSelector("[id$='devicehistory-window-editor'] [id$='history-filter-content'].slv-window"), "display: none");
        }

        public void CheckFilterAttributes(params string[] attributes)
        {
            WebDriverContext.Wait.Until(driver =>
            {
                try
                {
                    var filterList = Driver.FindElements(By.CssSelector("[id$='devicehistory-window-editor'] [id$='history-filter-content'].slv-window .devicehistory-editor-filter-item"));
                    var toBeCheckedAttributes = filterList.Where(c => attributes.Any(e => e.Equals(c.Text.Trim()))).ToList();

                    foreach (var attributeElement in toBeCheckedAttributes)
                    {
                        var checkbox = attributeElement.FindElement(By.CssSelector("div.checkbox"));
                        checkbox.Check(true);
                    }

                    return true;
                }
                catch (StaleElementReferenceException)
                {
                    return false;
                }
            });
        }

        /// <summary>
        /// Get list of all attributes
        /// </summary>
        /// <returns></returns>
        public List<string> GetListOfAttributes()
        {
            return JSUtility.GetElementsText("[id$='history-filter-content'] .devicehistory-editor-filter-item .devicehistory-editor-filter-item-title");
        }

        /// <summary>
        /// Get list of checked attributes
        /// </summary>
        /// <returns></returns>
        public List<string> GetListOfCheckedAttributes()
        {
            return JSUtility.GetElementsText("[id$='history-filter-content'] .devicehistory-editor-filter-item:not(.devicehistory-editor-filter-item-disable) .devicehistory-editor-filter-item-title");
        }

        /// <summary>
        /// Get list of disable attributes
        /// </summary>
        /// <returns></returns>
        public List<string> GetListOfUncheckedAttributes()
        {
            return JSUtility.GetElementsText("[id$='history-filter-content'] .devicehistory-editor-filter-item.devicehistory-editor-filter-item-disable .devicehistory-editor-filter-item-title");
        }

        /// <summary>
        /// Check if grid has no data
        /// </summary>
        /// <returns></returns>
        public bool IsNoDataMessageDisplayed()
        {
            var css = "[id$='devicehistory-window-editor-content-history-message']";
            if (ElementUtility.IsDisplayed(By.CssSelector(css)))
            {
                var text = JSUtility.GetElementText(css);
                if (text.Equals("No data"))
                    return true;
            }

            return false;
        }

        #endregion //Business methods   

        public override void WaitForPanelLoaded()
        {
            Wait.ForElementText(deviceNameTitle);
        }
    }
}
