using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using SeleniumExtras.PageObjects;
using StreetlightVision.Extensions;
using StreetlightVision.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;

namespace StreetlightVision.Pages.UI
{
    public class GridPanel : PanelBase
    {
        #region Variables

        #endregion //Variables

        #region IWebElements

        [FindsBy(How = How.CssSelector, Using = "[id$='window-toolbar-title'], [id*='custom-report'][style*='display: block'] [id$='header-title'].slv-treeviewdriven-window-header-title")]
        private IWebElement panelTitle;

        [FindsBy(How = How.CssSelector, Using = "[id*='custom-report'][style*='display: block'] [id$='close-button']")]
        private IWebElement closeButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='toolbar_item_w2ui-reload'] table.w2ui-button")]
        private IWebElement reloadDataToolbarButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='geozone-layout_toolbar_item_w2ui-column-on-off'] table.w2ui-button, [id$='customreport-grid_toolbar_item_w2ui-column-on-off'] table.w2ui-button, [id$='reportmanager-geozone-grid_toolbar_item_w2ui-column-on-off'] table.w2ui-button, [id$='alarms-grid_toolbar_item_w2ui-column-on-off'] table.w2ui-button")]
        private IWebElement shownHideColumnsToolbarButton;

        [FindsBy(How = How.CssSelector, Using = "div.w2ui-col-on-off")]
        private IWebElement showHideColumnMenu;

        [FindsBy(How = How.CssSelector, Using = "div.w2ui-col-on-off table tr")]
        private IList<IWebElement> showHideColumnList;

        [FindsBy(How = How.CssSelector, Using = "div.w2ui-col-on-off label")]
        private IList<IWebElement> showHideColumnLabelList;

        [FindsBy(How = How.CssSelector, Using = "div.w2ui-col-on-off input")]
        private IList<IWebElement> showHideColumnCheckBoxList;

        [FindsBy(How = How.CssSelector, Using = "w2ui-select-field table tr")]
        private IList<IWebElement> searchByList;

        [FindsBy(How = How.CssSelector, Using = ".w2ui-toolbar-search .w2ui-icon.icon-search-down.w2ui-search-down")]
        private IWebElement searchByButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='failures-device-list-view_search_all']")]
        private IWebElement searchInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='customreport-grid_toolbar'] [id$='toolbar_item_w2ui-search-advanced'] table.w2ui-button, [id$='reportmanager-geozone-grid_toolbar_item_w2ui-search-advanced'] table.w2ui-button, [id$='failures-device-list-view_toolbar_item_w2ui-search-advanced'] table.w2ui-button")]
        private IWebElement searchToolbarButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='device-failures-grid-editor_toolbar_item_commissioning'] table.w2ui-button")]
        private IWebElement commissionToolbarButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='device-failures-grid-editor_toolbar_item_help'] table.w2ui-button")]
        private IWebElement reportInformationToolbarButton;

        [FindsBy(How = How.CssSelector, Using = "[id^='w2ui-overlay-searches'] div.searchcriteria")]
        private IWebElement searchCriteriaSection;

        [FindsBy(How = How.CssSelector, Using = "[id$='geozone-grid_searchClear'].w2ui-search-clear, [id$='failures-device-list-view_searchClear'].w2ui-search-clear")]
        private IWebElement clearSearchButton;

        [FindsBy(How = How.CssSelector, Using = "[id^='tb'][id$='customreport-grid_toolbar_item_slv-customreport-toolbar-wizard'] > table")]
        private IWebElement editButton;

        [FindsBy(How = How.CssSelector, Using = "[id^='tb'][id$='customreport-grid_toolbar_item_slv-customreport-toolbar-requests'] > table")]
        private IWebElement selectOrAddSearchDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id^='tb'][id$='customreport-grid_toolbar_item_slv-customreport-toolbar-requests'] .slv-custom-report-toolbar-requests-button.slv-custom-report-toolbar-requests-button-save")]
        private IWebElement saveSearchDropDownButton;

        [FindsBy(How = How.CssSelector, Using = "[id^='tb'][id$='customreport-grid_toolbar_item_slv-customreport-toolbar-requests'] .slv-custom-report-toolbar-requests-button.slv-custom-report-toolbar-requests-button-add")]
        private IWebElement plusSearchDropDownButton;

        [FindsBy(How = How.CssSelector, Using = "[id^='tb'][id$='customreport-grid_toolbar_item_slv-customreport-toolbar-requests'] abbr.select2-search-choice-close")]
        private IWebElement cancelSearchDropDownButton;

        [FindsBy(How = How.CssSelector, Using = "[id^='tb'][id$='customreport-grid_toolbar_item_slv-customreport-toolbar-requests'] .slv-custom-report-toolbar-requests-button-add")]
        private IWebElement addSearchDropDownButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='layout_toolbar_item_export'] table.w2ui-button, [id$='alarmmanager-geozone-grid_toolbar_item_exportAlarms'] table.w2ui-button")]
        private IWebElement exportToolbarButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='customreport-grid_toolbar_item_slv-customreport-toolbar-export'] table.w2ui-button")]
        private IWebElement exportAdvancedSearchToolbarButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='toolbar-export'] table.w2ui-button")]
        private IWebElement generateCSVToolbarButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='toolbar-export'] table.w2ui-button a")]
        private IWebElement downloadToolbarButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='toolbar_item_import'] table.w2ui-button, [id$='alarmmanager-geozone-grid_toolbar_item_importAlarms'] table.w2ui-button")]
        private IWebElement importToolbarButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='toolbar_item_triggered'] table.w2ui-button")]
        private IWebElement showAllAlarmsToolbarOption;

        [FindsBy(How = How.CssSelector, Using = "[id$='toolbar_item_ack'] table.w2ui-button")]
        private IWebElement acknowledgeToolbarButton;

        [FindsBy(How = How.CssSelector, Using = "input[id$='from'], input.slv-historychart-from-date")]
        private IWebElement fromDateInput;

        [FindsBy(How = How.CssSelector, Using = "input[id$='to'], input.slv-historychart-to-date")]
        private IWebElement toDateInput;

        [FindsBy(How = How.CssSelector, Using = "input.slv-historychart-from-time")]
        private IWebElement fromTimeInput;

        [FindsBy(How = How.CssSelector, Using = "input.slv-historychart-to-time")]
        private IWebElement toTimeInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='toolbar_item_refresh'] table.w2ui-button, div.slv-historychart-run")]
        private IWebElement executeButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='toolbar_item_add'] table.w2ui-button")]
        private IWebElement addAlarmToolbarButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='toolbar_item_add'] table.w2ui-button")]
        private IWebElement addReportToolbarButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='toolbar_item_typeMode1'] table.w2ui-button")]
        private IWebElement displayFailuresByTypeToolbarButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='toolbar_item_typeMode2'] table.w2ui-button")]
        private IWebElement displayWarningFailuresToolbarButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='toolbar_item_typeMode3'] table.w2ui-button")]
        private IWebElement displayCriticalFailuresToolbarButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='toolbar-eventtime'] table.w2ui-button")]
        private IWebElement timestampToolbarButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='grid_body'][style*='top: 45px;'], [id$='layout_body'][style*='top: 45px;'], div.w2ui-grid-body[style*='top: 45px;']")]
        private IWebElement gridContainer;

        [FindsBy(How = How.CssSelector, Using = "[id^='grid'][id*='records'] tr[id^='grid'][id*='rec']")]
        private IList<IWebElement> gridRecordsList;

        [FindsBy(How = How.CssSelector, Using = "[id$='window_panel_main'] [id^='grid'][id$='grid_footer'] div.w2ui-footer-left, div.slv-custom-report-layout-grid [id^='grid'][id$='grid_footer'] div.w2ui-footer-left, [id$='panel_main'] [id$='grid-editor_footer'] div.w2ui-footer-left")]
        private IWebElement footerLeftLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='window_panel_main'] [id^='grid'][id$='grid_footer'] div.w2ui-footer-right, div.slv-custom-report-layout-grid [id^='grid'][id$='grid_footer'] div.w2ui-footer-right, [id$='panel_main'] [id$='grid-editor_footer'] div.w2ui-footer-right")]
        private IWebElement footerRightLabel;

        [FindsBy(How = How.CssSelector, Using = "div.slv-custom-report-layout-grid [id^='grid'][id$='grid_footer'] .w2grid-footer-pagebrowser-page-index, [id$='panel_main'] [id$='grid-editor_footer'] .w2grid-footer-pagebrowser-page-index")]
        private IWebElement footerPageIndexInput;

        [FindsBy(How = How.CssSelector, Using = "div.slv-custom-report-layout-grid [id^='grid'][id$='grid_footer'] .w2grid-footer-pagebrowser-button-next, [id$='panel_main'] [id$='grid-editor_footer'] .w2grid-footer-pagebrowser-button-next")]
        private IWebElement footerPageNextButton;

        [FindsBy(How = How.CssSelector, Using = "div.slv-custom-report-layout-grid [id^='grid'][id$='grid_footer'] .w2grid-footer-pagebrowser-button-last, [id$='panel_main'] [id$='grid-editor_footer'] .w2grid-footer-pagebrowser-button-last")]
        private IWebElement footerPageLastButton;

        [FindsBy(How = How.CssSelector, Using = "div.slv-custom-report-layout-grid [id^='grid'][id$='grid_footer'] .w2grid-footer-pagebrowser-button-previous, [id$='panel_main'] [id$='grid-editor_footer'] .w2grid-footer-pagebrowser-button-previous")]
        private IWebElement footerPagePreviousButton;

        [FindsBy(How = How.CssSelector, Using = "div.slv-custom-report-layout-grid [id^='grid'][id$='grid_footer'] .w2grid-footer-pagebrowser-button-first, [id$='panel_main'] [id$='grid-editor_footer'] .w2grid-footer-pagebrowser-button-first")]
        private IWebElement footerPageFirstButton;

        [FindsBy(How = How.CssSelector, Using = "[id='w2ui-overlay'] tr")]
        private IList<IWebElement> searchByColumnList;

        [FindsBy(How = How.CssSelector, Using = "div.slv-custom-report-layout-message .slv-custom-report-message")]
        private IWebElement gridMessage;

        #region Advanced search panel

        [FindsBy(How = How.CssSelector, Using = "[id^='w2ui-overlay-searches'] .w2ui-grid-searches-reset-btn")]
        private IWebElement advancedSearchResetButton;

        [FindsBy(How = How.CssSelector, Using = "[id^='w2ui-overlay-searches'] .w2ui-grid-searches-search-btn")]
        private IWebElement advancedSearchSearchButton;

        [FindsBy(How = How.CssSelector, Using = "[id^='w2ui-overlay-searches'] .w2ui-grid-searches tr:not(:last-child)")]
        private IList<IWebElement> advancedSearchConditionList;

        #endregion //Advanced search panel

        #endregion //IWebElements

        #region Constructor

        public GridPanel(IWebDriver driver, PageBase page) : base(driver, page)
        {
            PageFactory.InitElements(this, new RetryingElementLocator(driver, TimeSpan.FromMilliseconds(Settings.LocatorRetryingTimeout), TimeSpan.FromMilliseconds(Settings.LocatorPollingInterval)));

            WaitForPanelLoaded();
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

        /// <summary>
        /// Click 'ReloadDataToolbarButton' button
        /// </summary>
        public void ClickReloadDataToolbarButton()
        {
            reloadDataToolbarButton.ClickEx();
        }

        /// <summary>
        /// Click 'ShownHideColumnsToolbar' button
        /// </summary>
        public void ClickShownHideColumnsToolbarButton()
        {
            shownHideColumnsToolbarButton.ClickEx();
        }

        /// <summary>
        /// Click 'SearchBy' button
        /// </summary>
        public void ClickSearchByButton()
        {
            searchByButton.ClickEx();
        }

        /// <summary>
        /// Enter a value for 'Search' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterSearchInput(string value)
        {
            searchInput.Enter(value);
        }

        /// <summary>
        /// Click 'SearchToolbar' button
        /// </summary>
        public void ClickSearchToolbarButton()
        {
            searchToolbarButton.ClickEx();
        }

        /// <summary>
        /// Click 'CommissionToolbar' button
        /// </summary>
        public void ClickCommissionToolbarButton()
        {
            commissionToolbarButton.ClickEx();
        }

        /// <summary>
        /// Click 'ReportInformationToolbar' button
        /// </summary>
        public void ClickReportInformationToolbarButton()
        {
            reportInformationToolbarButton.ClickEx();
        }

        /// <summary>
        /// Click 'ClearSearch' button
        /// </summary>
        public void ClickClearSearchButton()
        {
            clearSearchButton.ClickEx();
        }

        /// <summary>
        /// Click 'Edit' button
        /// </summary>
        public void ClickEditButton()
        {
            editButton.ClickEx();
        }

        /// <summary>
        /// Select an item of 'SelectOrAddSearch' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectSelectOrAddSearchDropDown(string value)
        {
            selectOrAddSearchDropDown.Select(value);
        }

        /// <summary>
        /// Click 'SaveSearchDropDown' button
        /// </summary>
        public void ClickSaveSearchDropDownButton()
        {
            saveSearchDropDownButton.ClickEx();
        }

        /// <summary>
        /// Click 'PlusSearchDropDown' button
        /// </summary>
        public void ClickPlusSearchDropDownButton()
        {
            plusSearchDropDownButton.ClickEx();
        }

        /// <summary>
        /// Click 'CancelSearchDropDown' button
        /// </summary>
        public void ClickCancelSearchDropDownButton()
        {
            cancelSearchDropDownButton.ClickEx();
        }

        /// <summary>
        /// Click 'AddSearchDropDown' button
        /// </summary>
        public void ClickAddSearchDropDownButton()
        {
            addSearchDropDownButton.ClickEx();
        }

        /// <summary>
        /// Click 'ExportToolbar' button
        /// </summary>
        public void ClickExportToolbarButton()
        {
            exportToolbarButton.ClickByJS();
        }

        /// <summary>
        /// Click 'ExportAdvancedSearchToolbar' button
        /// </summary>
        public void ClickExportAdvancedSearchToolbarButton()
        {
            exportAdvancedSearchToolbarButton.ClickEx();
        }

        /// <summary>
        /// Click 'GenerateCSVToolbar' button
        /// </summary>
        public void ClickGenerateCSVToolbarButton()
        {
            generateCSVToolbarButton.ClickEx();
        }

        /// <summary>
        /// Click 'DowloadToolbar' button
        /// </summary>
        public void ClickDownloadToolbarButton()
        {
            downloadToolbarButton.ClickByJS();
        }

        /// <summary>
        /// Click 'ImportToolbarButton' button
        /// </summary>
        public void ClickImportToolbarButton()
        {
            importToolbarButton.ClickEx();
        }

        /// <summary>
        /// Click 'ShowAllAlarmsToolbarOption' button
        /// </summary>
        public void ClickShowAllAlarmsToolbarOption()
        {
            showAllAlarmsToolbarOption.ClickEx();
        }

        /// <summary>
        /// Click 'AcknowledgeToolbarButton' button
        /// </summary>
        public void ClickAcknowledgeToolbarButton()
        {
            acknowledgeToolbarButton.ClickEx();
        }

        /// <summary>
        /// Enter a value for 'From' date input
        /// </summary>
        /// <param name="value"></param>
        public void EnterFromDateInput(DateTime value)
        {
            fromDateInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'To' date input
        /// </summary>
        /// <param name="value"></param>
        public void EnterToDateInput(DateTime value)
        {
            toDateInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'From' time input
        /// </summary>
        /// <param name="value"></param>
        public void EnterFromTimeInput(DateTime value)
        {
            fromTimeInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'To' time input
        /// </summary>
        /// <param name="value"></param>
        public void EnterToTimeInput(DateTime value)
        {
            toTimeInput.Enter(value);
        }

        /// <summary>
        /// Click 'Execute' button
        /// </summary>
        public void ClickExecuteButton()
        {
            executeButton.ClickEx();
        }

        /// <summary>
        /// Click 'AddAlarmToolbar' button
        /// </summary>
        public void ClickAddAlarmToolbarButton()
        {
            addAlarmToolbarButton.ClickEx();
        }

        /// <summary>
        /// Click 'AddReportToolbar' button
        /// </summary>
        public void ClickAddReportToolbarButton()
        {
            addReportToolbarButton.ClickEx();
        }

        /// <summary>
        /// Click 'DisplayFailuresByTypeToolbar' button
        /// </summary>
        public void ClickDisplayFailuresByTypeToolbarButton()
        {
            displayFailuresByTypeToolbarButton.ClickByJS();
        }

        /// <summary>
        /// Click 'DisplayWarningFailuresToolbar' button
        /// </summary>
        public void ClickDisplayWarningFailuresToolbarButton()
        {
            displayWarningFailuresToolbarButton.ClickByJS();
        }

        /// <summary>
        /// Click 'DisplayCriticalFailuresToolbar' button
        /// </summary>
        public void ClickDisplayCriticalFailuresToolbarButton()
        {
            displayCriticalFailuresToolbarButton.ClickByJS();
        }

        /// <summary>
        /// Click 'TimeStampToolbar' button
        /// </summary>
        public void ClickTimeStampToolbarButton()
        {
            timestampToolbarButton.ClickEx();
        }

        /// <summary>
        /// Enter a value for 'FooterPageIndex' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterFooterPageIndexInput(string value)
        {
            footerPageIndexInput.Enter(value, true, false, true);
        }

        /// <summary>
        /// Click 'FooterPageNext' button
        /// </summary>
        public void ClickFooterPageNextButton()
        {
            footerPageNextButton.ClickEx();
        }

        /// <summary>
        /// Click 'FooterPageLast' button
        /// </summary>
        public void ClickFooterPageLastButton()
        {
            footerPageLastButton.ClickEx();
        }

        /// <summary>
        /// Click 'FooterPagePrevious' button
        /// </summary>
        public void ClickFooterPagePreviousButton()
        {
            footerPagePreviousButton.ClickEx();
        }

        /// <summary>
        /// Click 'FooterPageFirst' button
        /// </summary>
        public void ClickFooterPageFirstButton()
        {
            footerPageFirstButton.ClickEx();
        }

        #region Advanced search panel

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

        #endregion //Advanced search panel

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
        /// Get 'Search' input placeholder value
        /// </summary>
        /// <returns></returns>
        public string GetSearchPlaceHolderValue()
        {
            return searchInput.GetAttribute("placeholder");
        }

        /// <summary>
        /// Get 'SelectOrAddSearch' dropdown value
        /// </summary>
        /// <returns></returns>
        public string GetSelectOrAddSearchValue()
        {
            return selectOrAddSearchDropDown.Text;
        }

        /// <summary>
        /// Get 'From' date input value
        /// </summary>
        /// <returns></returns>
        public string GetFromDateValue()
        {
            return fromDateInput.Value();
        }

        /// <summary>
        /// Get 'To' date input value
        /// </summary>
        /// <returns></returns>
        public string GetToDateValue()
        {
            return toDateInput.Value();
        }

        /// <summary>
        /// Get 'From' time input value
        /// </summary>
        /// <returns></returns>
        public string GetFromTimeValue()
        {
            return fromTimeInput.Value();
        }

        /// <summary>
        /// Get 'To' time input value
        /// </summary>
        /// <returns></returns>
        public string GetToTimeValue()
        {
            return toTimeInput.Value();
        }

        /// <summary>
        /// Get 'FooterLeft' label text
        /// </summary>
        /// <returns></returns>
        public string GetFooterLeftText()
        {
            return footerLeftLabel.Text;
        }

        /// <summary>
        /// Get 'FooterRight' label text
        /// </summary>
        /// <returns></returns>
        public string GetFooterRightText()
        {
            return footerRightLabel.Text;
        }

        /// <summary>
        /// Get 'FooterPageIndex' input value
        /// </summary>
        /// <returns></returns>
        public string GetFooterPageIndexValue()
        {
            return footerPageIndexInput.Value();
        }

        /// <summary>
        /// Get 'GridMessage' text
        /// </summary>
        /// <returns></returns>
        public string GetGridMessageText()
        {
            return gridMessage.Text;
        }

        #endregion //Get methods

        #endregion //Basic methods

        #region Business methods

        /// <summary>
        /// Wait until data is displayed in grid
        /// </summary>
        public void WaitForGridContentAvailable()
        {
            Wait.ForElementsDisplayed(gridRecordsList);
        }

        /// <summary>
        /// Wait until left footer has text
        /// </summary>
        public void WaitForLeftFooterTextDisplayed()
        {
            Wait.ForElementText(By.CssSelector("[id$='window_panel_main'] [id^='grid'][id$='grid_footer'] div.w2ui-footer-left, div.slv-custom-report-layout-grid [id^='grid'][id$='grid_footer'] div.w2ui-footer-left, [id$='panel_main'] [id$='grid-editor_footer'] div.w2ui-footer-left"));
        }

        public void WaitForAcknowledgeButtonDisplayed()
        {
            Wait.ForElementDisplayed(By.CssSelector("[id$='toolbar_item_ack'] table.w2ui-button .icon-ok"));
        }

        public void WaitForGridMessageDisplayed()
        {
            Wait.ForElementDisplayed(By.CssSelector("div.slv-custom-report-layout-message"));
        }

        public void WaitForGridMessageDisappeared()
        {
            Wait.ForElementInvisible(By.CssSelector("div.slv-custom-report-layout-message"));
        }

        public void WaitForDataReloaded()
        {
            WebDriverContext.Wait.Until(driver =>
            {
                try
                {
                    var reloadTitle = reloadDataToolbarButton.GetAttribute("title");
                    if (!reloadTitle.Equals("Reload data in the list"))
                        return false;

                    return true;
                }
                catch
                {
                    return false;
                }
            });
        }

        public bool IsGridMessageDisplayed()
        {
            return gridMessage.Displayed;
        }

        /// <summary>
        /// Build data table from grid. Data doesn't include total row
        /// </summary>
        /// <returns></returns>
        public DataTable BuildDataTableFromGrid()
        {
            DataTable tblResult = gridContainer.BuildDataTableFromGrid();

            return tblResult;
        }

        /// <summary>
        /// Build a data table from total row. Therefore, the data table contains only 1 row
        /// </summary>
        /// <returns></returns>
        public DataTable BuildDataTableFromTotalRow()
        {
            DataTable tblResult = gridContainer.BuildDataTableFromTotalRow();

            return tblResult;
        }

        /// <summary>
        /// Build Failures data table from grid.
        /// </summary>
        /// <returns></returns>
        public DataTable BuildFailuresDataTable()
        {
            var gridContainer = Driver.FindElement(By.CssSelector("[id$='device-failures-grid-editor_body'].w2ui-grid-body"));
            DataTable tblResult = gridContainer.BuildDataTableFromGrid();

            return tblResult;
        }

        /// <summary>
        /// Build Alarm Manager data table from grid.
        /// </summary>
        /// <returns></returns>
        public DataTable BuildAlarmManagerDataTable()
        {
            Wait.ForSeconds(2);
            var gridContainer = Driver.FindElement(By.CssSelector("[id$='alarmmanager-geozone-grid_body'].w2ui-grid-body"));
            DataTable tblResult = gridContainer.BuildDataTableFromGrid();

            return tblResult;
        }

        /// <summary>
        /// Build Alarm data table from grid.
        /// </summary>
        /// <returns></returns>
        public DataTable BuildAlarmDataTable()
        {
            Wait.ForSeconds(2);
            var gridContainer = Driver.FindElement(By.CssSelector("[id$='alarms-grid_body'].w2ui-grid-body"));
            DataTable tblResult = gridContainer.BuildDataTableFromGrid();

            return tblResult;
        }

        /// <summary>
        /// Get list of columns header of Failures grid
        /// </summary>
        /// <returns></returns>
        public List<string> GetListOfColumnsHeaderFailures()
        {
            var gridContainer = Driver.FindElement(By.CssSelector("[id$='device-failures-grid-editor_body'].w2ui-grid-body"));
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

        /// <summary>
        /// Get list of columns header of grid
        /// </summary>
        /// <returns></returns>
        public List<string> GetListOfColumnsHeader()
        {
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

        /// <summary>
        /// Check if a grid has column group
        /// </summary>
        /// <returns></returns>
        public bool HasColumnGroups()
        {
            return Driver.FindElements(By.CssSelector(".w2ui-head .w2ui-col-group")).Any();
        }

        /// <summary>
        /// Check if a specific column name has Value and Timestamp column
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public bool IsColumnHasValueAndTimestamp(string columnName)
        {
            if (HasColumnGroups())
            {
                var columnGroups = JSUtility.GetElementsText(".w2ui-head .w2ui-col-group:not(.w2ui-col-header)");
                columnGroups.RemoveAll(p => string.IsNullOrEmpty(p));
                var currentColGroup = columnGroups.FirstOrDefault(p => p.Equals(columnName));
                var index = columnGroups.IndexOf(currentColGroup);
                if (currentColGroup != null)
                {
                    var columns = JSUtility.GetElementsText(".w2ui-head .w2ui-col-header:not(.w2ui-col-group)").ToList();
                    var isValueExisting = columns[index * 2].Contains("Value");
                    var isTimestampExisting = columns[index * 2 + 1].Contains("Timestamp");

                    return isValueExisting && isTimestampExisting;
                }
            }

            return false;
        }

        /// <summary>
        /// Get count of Timestamp column appears
        /// </summary>
        /// <returns></returns>
        public int GetCountOfTimestampColumn()
        {
            var results = new List<string>();
            var columnHeaders = gridContainer.GetAllGridHeaders() as IEnumerable;
            var count = 0;
            foreach (var header in columnHeaders)
            {
                var headerText = header as string;
                headerText = headerText.TrimEx();
                if (headerText.Equals("Timestamp")) count++;
            }
            return count;
        }

        /// <summary>
        /// Get count of Value column appears
        /// </summary>
        /// <returns></returns>
        public int GetCountOfValueColumn()
        {
            var results = new List<string>();
            var columnHeaders = gridContainer.GetAllGridHeaders() as IEnumerable;
            var count = 0;
            foreach (var header in columnHeaders)
            {
                var headerText = header as string;
                headerText = headerText.TrimEx();
                if (headerText.Equals("Value")) count++;
            }
            return count;
        }

        /// <summary>
        /// Get list of device in grid
        /// </summary>
        /// <returns></returns>
        public List<string> GetListOfDevices()
        {
            return GetListOfColumnData("Device");
        }

        /// <summary>
        /// Get list of geozones in grid
        /// </summary>
        /// <returns></returns>
        public List<string> GetListOfGeozones()
        {
            DataTable tblGrid = BuildDataTableFromGrid();
            var geozoneList = new List<string>();

            if (tblGrid.Columns.Contains("Geozone"))
            {
                geozoneList = tblGrid.AsEnumerable().Select(r => r.Field<string>("Geozone")).OrderBy(name => name).ToList();
            }

            return geozoneList;
        }

        /// <summary>
        /// Check if From Date field is visible
        /// </summary>
        /// <returns></returns>
        public bool IsFromDateInputVisible()
        {
            return Driver.FindElements(By.CssSelector("input[id$='from'], input.slv-historychart-from-date")).Count > 0;
        }

        /// <summary>
        /// Check if To Date field is visible
        /// </summary>
        /// <returns></returns>
        public bool IsToDateInputVisible()
        {
            return Driver.FindElements(By.CssSelector("input[id$='to'], input.slv-historychart-to-date")).Count > 0;
        }

        /// <summary>
        /// Check if From Time field is visible
        /// </summary>
        /// <returns></returns>
        public bool IsFromTimeInputVisible()
        {
            return Driver.FindElements(By.CssSelector("input.slv-historychart-from-time")).Count > 0;
        }

        /// <summary>
        /// Check if To Time field is visible
        /// </summary>
        /// <returns></returns>
        public bool IsToTimeInputVisible()
        {
            return Driver.FindElements(By.CssSelector("input.slv-historychart-to-time")).Count > 0;
        }

        /// <summary>
        /// Check if panel is visible
        /// </summary>
        /// <returns></returns>
        public bool IsPanelVisible()
        {
            return gridContainer.Displayed;
        }

        /// <summary>
        /// Check if DisplayFailuresByType is visible
        /// </summary>
        /// <returns></returns>
        public bool IsDisplayFailuresByTypeToolbarButtonVisible()
        {
            return displayFailuresByTypeToolbarButton.Displayed;
        }

        /// <summary>
        /// Check if DisplayFailuresByType is toggle ON
        /// </summary>
        /// <returns></returns>
        public bool IsDisplayFailuresByTypeToolbarButtonToggleOn()
        {
            return displayFailuresByTypeToolbarButton.GetAttribute("class").Contains("checked");
        }

        /// <summary>
        /// Check if DisplayWarningFailures is visible
        /// </summary>
        /// <returns></returns>
        public bool IsDisplayWarningFailuresToolbarButtonVisible()
        {
            return displayWarningFailuresToolbarButton.Displayed;
        }

        /// <summary>
        /// Check if DisplayWarningFailures is toggle ON
        /// </summary>
        /// <returns></returns>
        public bool IsDisplayWarningFailuresToolbarButtoToggleOn()
        {
            return displayWarningFailuresToolbarButton.GetAttribute("class").Contains("checked"); 
        }

        /// <summary>
        /// Check if DisplayCriticalFailures is visible
        /// </summary>
        /// <returns></returns>
        public bool IsDisplayCriticalFailuresToolbarButtonVisible()
        {
            return displayCriticalFailuresToolbarButton.Displayed;
        }

        /// <summary>
        /// Check if DisplayCriticalFailures is toggle ON
        /// </summary>
        /// <returns></returns>
        public bool IsDisplayCriticalFailuresToolbarButtonToggleOn()
        {
            return displayCriticalFailuresToolbarButton.GetAttribute("class").Contains("checked"); 
        }

        /// <summary>
        /// Click on a record with specific text contains
        /// </summary>
        /// <param name="text"></param>
        public void ClickGridRecord(string text)
        {
            WebDriverContext.Wait.Until(driver =>
            {
                try
                {
                    var currentRec = gridRecordsList.FirstOrDefault(p => p.Text.Contains(text));
                    currentRec.ClickEx();

                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            });
        }

        /// <summary>
        /// Double click on a record with specific text contains
        /// </summary>
        /// <param name="text"></param>
        public void DoubleClickGridRecord(string text)
        {
            Wait.ForSeconds(1);
            WebDriverContext.Wait.Until(driver =>
            {
                try
                {
                    var currentRec = gridRecordsList.FirstOrDefault(p => p.Text.Contains(text));
                    if (currentRec != null)
                    {
                        currentRec.ScrollToElementByJS();
                        currentRec.DoubleClick();
                        return true;
                    }
                    return false;
                }
                catch (StaleElementReferenceException)
                {
                    return false;
                }
            });
        }

        /// <summary>
        /// Check if show hide column menu is being displayed
        /// </summary>
        /// <returns></returns>
        public bool IsShowHideColumnsMenuDisplayed()
        {
            return Driver.FindElements(By.CssSelector("div.w2ui-col-on-off")).Count > 0;
        }

        /// <summary>
        /// Check if "Plus" button in SearchDropDown is visible
        /// </summary>
        /// <returns></returns>
        public bool IsPlusSearchDropDownButtonVisible()
        {
            return Driver.FindElements(By.CssSelector("[id^='tb'][id$='customreport-grid_toolbar_item_slv-customreport-toolbar-requests'] .slv-custom-report-toolbar-requests-button.slv-custom-report-toolbar-requests-button-add")).Count > 0;
        }

        /// <summary>
        /// Check if "Save" button in SearchDropDown is visible
        /// </summary>
        /// <returns></returns>
        public bool IsSaveSearchDropDownButtonVisible()
        {
            return Driver.FindElements(By.CssSelector("[id^='tb'][id$='customreport-grid_toolbar_item_slv-customreport-toolbar-requests'] .slv-custom-report-toolbar-requests-button.slv-custom-report-toolbar-requests-button-save")).Count > 0;
        }

        /// <summary>
        /// Toggle on show hide columns menu
        /// </summary>
        public void DisplayShowHideColumnsMenu()
        {
            if (!IsShowHideColumnsMenuDisplayed())
            {
                shownHideColumnsToolbarButton.ClickEx();
            }
        }

        /// <summary>
        /// Toggle off show hide columns menu
        /// </summary>
        public void HideShowHideColumnsMenu()
        {
            if (IsShowHideColumnsMenuDisplayed())
            {
                shownHideColumnsToolbarButton.ClickEx();
            }
        }

        /// <summary>
        /// Get all columns in show hide columns menu
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllColumnsInShowHideColumnsMenu()
        {
            DisplayShowHideColumnsMenu();

            var columnList = new List<string>();

            foreach (var columnElement in showHideColumnLabelList)
            {
                columnList.Add(columnElement.Text);
            }
            columnList.RemoveAll(p => string.IsNullOrEmpty(p));

            return columnList;
        }

        /// <summary>
        /// Get all checked columns in show hide columns menu
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllCheckedColumnsInShowHideColumnsMenu()
        {
            DisplayShowHideColumnsMenu();

            var columnList = new List<string>();

            foreach (var columnElement in showHideColumnList)
            {
                if (columnElement.FindElements(By.CssSelector("label[for]")).Count == 0)
                    continue;

                var labelForCheckbox = columnElement.FindElement(By.CssSelector("label[for]"));

                if (columnElement.FindElement(By.CssSelector("input")).Selected)
                {
                    columnList.Add(labelForCheckbox.Text);
                }
            }

            return columnList;
        }

        /// <summary>
        /// Get all unchecked columns in show hide columns menu
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllUncheckedColumnsInShowHideColumnsMenu()
        {
            DisplayShowHideColumnsMenu();

            var columnList = new List<string>();

            foreach (var columnElement in showHideColumnList)
            {
                if (columnElement.FindElements(By.CssSelector("label[for]")).Count == 0)
                    continue;

                var labelForCheckbox = columnElement.FindElement(By.CssSelector("label[for]"));

                if (!columnElement.FindElement(By.CssSelector("input")).Selected)
                {
                    columnList.Add(labelForCheckbox.Text);
                }
            }

            return columnList;
        }

        /// <summary>
        /// Check all columns in show hide columns menu
        /// </summary>
        /// <returns></returns>
        public void CheckAllColumnsInShowHideColumnsMenu()
        {
            WebDriverContext.Wait.Until(driver =>
            {
                try
                {
                    DisplayShowHideColumnsMenu();

                    foreach (var columnElement in showHideColumnList)
                    {
                        if (columnElement.FindElements(By.CssSelector("label[from]")).Count == 0)
                            continue;

                        var labelForCheckbox = columnElement.FindElement(By.CssSelector("label[from]"));

                        if (!columnElement.FindElement(By.CssSelector("input")).Selected)
                        {
                            labelForCheckbox.ClickEx();
                        }
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
        /// Uncheck all columns in show hide columns menu
        /// </summary>
        /// <returns></returns>
        public void UncheckAllColumnsInShowHideColumnsMenu()
        {
            WebDriverContext.Wait.Until(driver =>
            {
                try
                {
                    DisplayShowHideColumnsMenu();

                    foreach (var columnElement in showHideColumnList)
                    {
                        if (columnElement.FindElements(By.CssSelector("label[from]")).Count == 0)
                            continue;

                        var labelForCheckbox = columnElement.FindElement(By.CssSelector("label[from]"));

                        if (columnElement.FindElement(By.CssSelector("input")).Selected)
                        {
                            labelForCheckbox.ClickEx();
                        }
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
        /// Check some columns in show hide columns menu
        /// </summary>
        /// <param name="columns"></param>
        public void CheckColumnsInShowHideColumnsMenu(params string[] columns)
        {
            WebDriverContext.Wait.Until(driver =>
            {
                try
                {
                    DisplayShowHideColumnsMenu();

                    var toBeCheckedColumns = showHideColumnList.Where(c => columns.Any(e => e.Equals(c.Text.Trim()))).ToList();

                    foreach (var columnElement in toBeCheckedColumns)
                    {
                        var labelForCheckbox = columnElement.FindElement(By.CssSelector("label[from]"));

                        if (!columnElement.FindElement(By.CssSelector("input")).Selected)
                        {
                            labelForCheckbox.ClickEx();
                        }
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
        /// Un-Check some columns in show hide columns menu
        /// </summary>
        /// <param name="columns"></param>
        public void UncheckColumnsInShowHideColumnsMenu(params string[] columns)
        {
            WebDriverContext.Wait.Until(driver =>
            {
                try
                {
                    DisplayShowHideColumnsMenu();

                    var toBeUncheckedColumns = showHideColumnList.Where(c => columns.Any(e => e.Equals(c.Text.Trim()))).ToList();

                    foreach (var columnElement in toBeUncheckedColumns)
                    {
                        var labelForCheckbox = columnElement.FindElement(By.CssSelector("label[from]"));

                        if (columnElement.FindElement(By.CssSelector("input")).Selected)
                        {
                            labelForCheckbox.ClickEx();
                        }
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
        /// To check if a record contains specified text present in grid
        /// </summary>
        /// <param name="text"></param>
        public bool IsRecordHasTextPresent(string text)
        {
            WebDriverContext.Wait.Until(driver =>
            {
                try
                {
                    gridRecordsList.FirstOrDefault(p => p.Text.Contains(text));

                    return true;
                }
                catch (StaleElementReferenceException)
                {
                    return false;
                }
            });

            var currentRec = gridRecordsList.FirstOrDefault(p => p.Text.Contains(text));

            return currentRec != null;
        }

        /// <summary>
        /// To check if a column has specified text present
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="textValue"></param>
        /// <returns></returns>
        public bool IsColumnHasTextPresent(string columnName, string textValue)
        {
            var dtGrid = GetListOfColumnData(columnName);
            return dtGrid.Exists(p => p == textValue);
        }

        /// <summary>
        /// To check if a Alarm Manager column has specified text present
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="textValue"></param>
        /// <returns></returns>
        public bool IsAlarmGridHasTextPresent(string columnName, string textValue)
        {
            var dtGrid = GetListOfColumnDataAlarm(columnName);
            return dtGrid.Exists(p => p == textValue);
        }

        /// <summary>
        ///  Check if a row has Error icon in Alarm grid
        /// </summary>
        /// <param name="textValue"></param>
        /// <returns></returns>
        public bool IsAlarmGridHasErrorIcon(string textValue)
        {
            var records = Driver.FindElements(By.CssSelector("[id$='alarms-grid_body'].w2ui-grid-body tr[id^='grid'][id*='alarms-grid_rec']"));

            foreach (var record in records)
            {
                var columnsData = record.FindElements(By.CssSelector(".w2ui-grid-data div[title]")).Select(p => p.Text.Trim());
                if (columnsData.Contains(textValue))
                {
                    var iconColumn = record.FindElement(By.CssSelector(".alarms-grid-icon-status"));
                    if (iconColumn.GetStyleAttr("background-image").Contains("status-error.png"))
                        return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Get list of icon with text in in Alarm grid row
        /// </summary>
        /// <param name="textValue"></param>
        /// <returns></returns>
        public List<string> GetListOfAlarmGridIconColumn(string textValue)
        {
            var result = new List<string>();
            var records = Driver.FindElements(By.CssSelector("[id$='alarms-grid_body'].w2ui-grid-body tr[id^='grid'][id*='alarms-grid_rec']"));

            foreach (var record in records)
            {
                var columnsData = record.FindElements(By.CssSelector(".w2ui-grid-data div[title]")).Select(p => p.Text.Trim());
                if (columnsData.Contains(textValue))
                {
                    var iconColumn = record.FindElement(By.CssSelector(".alarms-grid-icon-status"));
                    result.Add(iconColumn.GetStyleAttr("background-image"));
                }
            }

            return result;
        }

        /// <summary>
        /// Click record has Error Icon in Alarm grid
        /// </summary>
        /// <param name="textValue"></param>
        public void ClickAlarmGridRecordHasErrorIcon(string textValue)
        {
            var records = Driver.FindElements(By.CssSelector("[id$='alarms-grid_body'].w2ui-grid-body tr[id^='grid'][id*='alarms-grid_rec']"));

            foreach (var record in records)
            {
                var columnsData = record.FindElements(By.CssSelector(".w2ui-grid-data div[title]")).Select(p => p.Text.Trim());
                if (columnsData.Contains(textValue))
                {
                    var iconColumn = record.FindElement(By.CssSelector(".alarms-grid-icon-status"));
                    if (iconColumn.GetStyleAttr("background-image").Contains("status-error.png"))
                    {
                        record.ClickEx();
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// To check if a Alarm column has specified text present
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="textValue"></param>
        /// <returns></returns>
        public bool IsAlarmManagerGridHasTextPresent(string columnName, string textValue)
        {
            var dtGrid = GetListOfColumnDataAlarmManager(columnName);
            return dtGrid.Exists(p => p == textValue);
        }

        /// <summary>
        /// Check if "Show all alarm toolbar button" is being toggled on
        /// </summary>
        /// <returns></returns>
        public bool IsShowAllAlarmsOptionToggledOn()
        {
            return showAllAlarmsToolbarOption.GetAttribute("class").Contains("checked");
        }

        /// <summary>
        /// Check if a record contains specific text is read-only or not
        /// </summary>
        /// <param name="textValue"></param>
        /// <returns></returns>
        public bool IsFailuresGridRecordReadOnly(string textValue)
        {
            var records = Driver.FindElements(By.CssSelector("[id^='grid'][id*='records'] tr[id^='grid'][id*='device-failures-grid-editor_rec']"));

            foreach (var record in records)
            {
                var columnsData = record.FindElements(By.CssSelector(".w2ui-grid-data div[title]")).Select(p => p.Text.Trim());
                if(columnsData.Contains(textValue))
                {
                    var customStype = record.GetAttribute("custom_style");
                    if (customStype.Contains("opacity: 0.7"))
                        return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Tick checkbox of a record contains specific text
        /// </summary>
        /// <param name="textRecord"></param>
        /// <param name="value"></param>
        public void TickFailuresGridRecordCheckbox(string textRecord, bool value)
        {
            var records = Driver.FindElements(By.CssSelector("[id^='grid'][id*='records'] tr[id^='grid'][id*='device-failures-grid-editor_rec']"));

            foreach (var record in records)
            {
                var columnsData = record.FindElements(By.CssSelector(".w2ui-grid-data div[title]")).Select(p => p.Text.Trim());
                if (columnsData.Contains(textRecord))
                {
                    var checkbox = record.FindElements(By.CssSelector(".checkbox.w2ui-grid-slv-checkbox")).FirstOrDefault();
                    if (checkbox != null)
                    {
                        checkbox.Check(value);
                        Wait.ForSeconds(2);
                        return;
                    }
                }
            }            
        }

        /// <summary>
        /// Get checkbox value of a record contains specific text
        /// </summary>
        /// <param name="textRecord"></param>
        /// <param name="value"></param>
        public bool GetFailuresGridRecordCheckboxValue(string textRecord)
        {
            var records = Driver.FindElements(By.CssSelector("[id^='grid'][id*='records'] tr[id^='grid'][id*='device-failures-grid-editor_rec']"));

            foreach (var record in records)
            {
                var columnsData = record.FindElements(By.CssSelector(".w2ui-grid-data div[title]")).Select(p => p.Text.Trim());
                if (columnsData.Contains(textRecord))
                {
                    var checkbox = record.FindElements(By.CssSelector(".checkbox.w2ui-grid-slv-checkbox")).FirstOrDefault();
                    if (checkbox != null)
                    {                        
                        return checkbox.CheckboxValue();
                    }
                }
            }

            return false;
        }        

        /// <summary>
        /// Scroll and select a number of last records of failures grid
        /// </summary>
        /// <param name="number"></param>
        public void SelectFailuresGridLastRecords(int number)
        {
            var records = Driver.FindElements(By.CssSelector("[id^='grid'][id*='records'] tr[id^='grid'][id*='device-failures-grid-editor_rec']"));
            int totalCount = records.Count;
            var lastRecords = records.Skip(totalCount - number).Take(number);
            var lastRecord = lastRecords.Last();
            lastRecord.ScrollToElementByJS();
            foreach (var record in lastRecords)
            {
                var checkbox = record.FindElements(By.CssSelector(".checkbox.w2ui-grid-slv-checkbox")).FirstOrDefault();
                if (checkbox != null)
                {
                    checkbox.Check(true);
                }
            }
        }

        /// <summary>
        /// Get check value of last record of failures grid
        /// </summary>
        /// <returns></returns>
        public bool GetFailuresGridLastRecordCheckboxValue()
        {
            var records = Driver.FindElements(By.CssSelector("[id^='grid'][id*='records'] tr[id^='grid'][id*='device-failures-grid-editor_rec']"));
            var lastRecord = records.Last();
            var checkbox = lastRecord.FindElements(By.CssSelector(".checkbox.w2ui-grid-slv-checkbox")).FirstOrDefault();
            if (checkbox != null)
            {
                return checkbox.CheckboxValue();
            }

            return false;
        }

        /// <summary>
        /// Get check value of last records with specific number
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public List<bool> GetFailuresGridLastRecordsCheckboxValue(int number)
        {
            var result = new List<bool>();
            var records = Driver.FindElements(By.CssSelector("[id^='grid'][id*='records'] tr[id^='grid'][id*='device-failures-grid-editor_rec']"));
            int totalCount = records.Count;
            var lastRecords = records.Skip(totalCount - number).Take(number);
            foreach (var record in lastRecords)
            {
                var checkbox = record.FindElements(By.CssSelector(".checkbox.w2ui-grid-slv-checkbox")).FirstOrDefault();
                if (checkbox != null)
                {
                    result.Add(checkbox.CheckboxValue());
                }
            }

            return result;
        }

        /// <summary>
        /// Toggle on or off "Shown all alarms toolbar button"
        /// </summary>
        /// <returns></returns>
        public void ToggleShowAllAlarmsOption(bool on)
        {
            if (on)
            {
                if (!IsShowAllAlarmsOptionToggledOn())
                {
                    showAllAlarmsToolbarOption.ClickEx();
                }
            }
            else
            {
                if (IsShowAllAlarmsOptionToggledOn())
                {
                    showAllAlarmsToolbarOption.ClickEx();
                }
            }
        }

        /// <summary>
        /// Check if "AcknowledgeToolbarButton" is enabled
        /// </summary>
        /// <returns></returns>
        public bool IsAcknowledgeToolbarButtonEnabled()
        {
            return !Driver.FindElement(By.CssSelector("[id$='toolbar_item_ack']")).GetAttribute("class").Contains("disabled");
        }

        /// <summary>
        /// Get data of a specific column of grid
        /// </summary>
        /// <returns></returns>
        public List<string> GetListOfColumnData(string columnName)
        {
            DataTable tblGrid = BuildDataTableFromGrid();
            var result = new List<string>();

            if (tblGrid.Columns.Contains(columnName))
            {
                result = tblGrid.AsEnumerable().Select(r => r.Field<string>(columnName)).ToList();
            }

            return result;
        }

        /// <summary>
        /// Get data of a specific column of Failures grid
        /// </summary>
        /// <returns></returns>
        public List<string> GetListOfColumnDataFailures(string columnName)
        {
            DataTable tblGrid = BuildFailuresDataTable();
            var result = new List<string>();

            if (tblGrid.Columns.Contains(columnName))
            {
                result = tblGrid.AsEnumerable().Select(r => r.Field<string>(columnName)).ToList();
            }

            return result;
        }

        /// <summary>
        /// Get data of a specific column of Alarm Manager grid
        /// </summary>
        /// <returns></returns>
        public List<string> GetListOfColumnDataAlarmManager(string columnName)
        {
            DataTable tblGrid = BuildAlarmManagerDataTable();
            var result = new List<string>();

            if (tblGrid.Columns.Contains(columnName))
            {
                result = tblGrid.AsEnumerable().Select(r => r.Field<string>(columnName)).ToList();
            }

            return result;
        }

        /// <summary>
        /// Get data of a specific column of Alarm grid
        /// </summary>
        /// <returns></returns>
        public List<string> GetListOfColumnDataAlarm(string columnName)
        {
            DataTable tblGrid = BuildAlarmDataTable();
            var result = new List<string>();

            if (tblGrid.Columns.Contains(columnName))
            {
                result = tblGrid.AsEnumerable().Select(r => r.Field<string>(columnName)).ToList();
            }

            return result;
        }

        /// <summary>
        /// Click on search by column
        /// </summary>
        /// <param name="columnName"></param>
        public void ClickOnSearchByColumn(string columnName)
        {
            var column = searchByColumnList.FirstOrDefault(p => p.FindElement(By.CssSelector("td:nth-child(2)")).Text.Equals(columnName));
            column.ClickEx();
        }

        /// <summary>
        /// Tick on checkbox column of specific row text
        /// </summary>
        /// <param name="rowText"></param>
        /// <param name="value"></param>
        public void TickGridColumn(string rowText, bool value)
        {
            var currentRec = gridRecordsList.FirstOrDefault(p => p.Text.Contains(rowText));
            if (currentRec != null)
            {
                currentRec.ClickEx();
                var checkbox = currentRec.FindElement(By.CssSelector("div.checkbox.w2ui-grid-slv-checkbox"));
                checkbox.Check(value);
            }
            else
                Assert.Warn(string.Format("Cannot find row with '{0}'", rowText));
        }

        /// <summary>
        /// Get checkbox value of specific row text
        /// </summary>
        /// <param name="rowText"></param>
        /// <returns></returns>
        public bool GetCheckBoxGridColumnValue(string rowText)
        {
            var alarmChecked = false;
            WebDriverContext.Wait.Until(driver =>
            {
                try
                {
                    var currentRec = gridRecordsList.FirstOrDefault(p => p.Text.Contains(rowText));
                    var checkbox = currentRec.FindElement(By.CssSelector("div.checkbox.w2ui-grid-slv-checkbox"));

                    alarmChecked = checkbox.CheckboxValue();

                    return true;
                }
                catch (StaleElementReferenceException)
                {
                    return false;
                }
            });

            return alarmChecked;
        }

        #region Advanced search panel

        /// <summary>
        /// Select operator of specific Field for search
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        public void SelectOperatorAdvancedSearch(string fieldName, string value)
        {
            var curElement = advancedSearchConditionList.FirstOrDefault(p => p.FindElement(By.CssSelector(".caption")).Text.Equals(fieldName));
            if (curElement != null)
            {
                var dropbox = curElement.FindElement(By.CssSelector(".operator div"));
                dropbox.Select(value, true);
            }
            else
                Assert.Warn(string.Format("Cannot find field '{0}' in Advanced Search Panel", fieldName));

        }

        /// <summary>
        /// Enter value of specific Field for search
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        public void EnterValueAdvancedSearch(string fieldName, string value)
        {
            var curElement = advancedSearchConditionList.FirstOrDefault(p => p.FindElement(By.CssSelector(".caption")).Text.Equals(fieldName));
            if (curElement != null)
            {
                var input = curElement.FindElement(By.CssSelector(".value input"));
                input.Enter(value);
            }
            else
                Assert.Warn(string.Format("Cannot find field '{0}' in Advanced Search Panel", fieldName));
        }

        #endregion //Advanced search panel        

        /// <summary>
        /// Get list of column name sorted down
        /// </summary>
        /// <returns></returns>
        public List<string> GetListOfColumnSortedDown()
        {
            var results = new List<string>();

            var sortedColumns = Driver.FindElements(By.CssSelector("td.w2ui-head .w2ui-col-header.w2ui-col-sorted"));

            foreach (var column in sortedColumns)
            {
                if (column.FindElements(By.CssSelector(".w2ui-sort-down")).Count > 0)
                {
                    results.Add(column.Text.TrimEx());
                }
            }

            return results;
        }

        /// <summary>
        /// Get list of column name sorted up
        /// </summary>
        /// <returns></returns>
        public List<string> GetListOfColumnSortedUp()
        {
            var results = new List<string>();

            var sortedColumns = Driver.FindElements(By.CssSelector("td.w2ui-head .w2ui-col-header.w2ui-col-sorted"));

            foreach (var column in sortedColumns)
            {
                if (column.FindElements(By.CssSelector(".w2ui-sort-up")).Count > 0)
                {
                    results.Add(column.Text.TrimEx());
                }
            }

            return results;
        }

        /// <summary>
        /// Get list of column name sorted
        /// </summary>
        /// <returns></returns>
        public List<string> GetListOfColumnSorted()
        {
            var results = new List<string>();
            var sortedColumns = Driver.FindElements(By.CssSelector("td.w2ui-head .w2ui-col-header.w2ui-col-sorted"));
            foreach (var column in sortedColumns)
            {
                results.Add(column.Text.TrimEx());
            }

            return results;
        }

        /// <summary>
        /// Click on header column to sort
        /// </summary>
        /// <param name="columnName"></param>
        public void ClickGridColumnHeader(string columnName)
        {
            var columns = Driver.FindElements(By.CssSelector("td.w2ui-head .w2ui-col-header"));
            var column = columns.FirstOrDefault(p => p.Text.TrimEx().Equals(columnName));
            if (column != null)
                column.ClickEx();
            else
                Assert.Warn(string.Format("Cannot click on column '{0}'", columnName));
        }

        /// <summary>
        /// Get all searches of dropdown in toolbar
        /// </summary>
        /// <returns></returns>
        public List<string> GetListOfSearchDropDownItems(bool isCloseResult = true)
        {
            var items = selectOrAddSearchDropDown.GetAllItems(isCloseResult);
            items.Remove("---");
            items.Remove("No matches found");
            return items;
        }

        public void ClickSearchDropDown()
        {
            selectOrAddSearchDropDown.ClickEx();
        }

        public void EnterSearchDropDownInputValue(string value)
        {
            IWebElement inputTextBox = Driver.FindElement(By.CssSelector("[id='select2-drop'] input"));
            inputTextBox.Clear();
            inputTextBox.SendKeys(value);
        }

        public void HitEnterSearchDropDown()
        {
            var action = new Actions(Driver);
            action.SendKeys(Keys.Enter).Build().Perform();
            Wait.ForElementInvisible(By.CssSelector("[id='select2-drop']"));
        }

        public bool IsPlusSearchButtonDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id^='tb'][id$='customreport-grid_toolbar_item_slv-customreport-toolbar-requests'] .slv-custom-report-toolbar-requests-button.slv-custom-report-toolbar-requests-button-add"));
        }

        public bool IsSaveSearchButtonDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id^='tb'][id$='customreport-grid_toolbar_item_slv-customreport-toolbar-requests'] .slv-custom-report-toolbar-requests-button.slv-custom-report-toolbar-requests-button-save"));
        }

        public bool IsTimeStampButtonChecked()
        {
            return Driver.FindElements(By.CssSelector("[id$='toolbar-eventtime'] table.w2ui-button span.icon-checked.slv-custom-report-icon-check")).Count > 0;
        }

        public bool IsColumnHeadersHasValueAndTimestamp()
        {
            var headers = JSUtility.GetElementsText("[id$='grid_body'][style*='top: 45px;'] .w2ui-head > div:not(.w2ui-resizer), [id$='layout_body'][style*='top: 45px;'] .w2ui-head > div:not(.w2ui-resizer), div.w2ui-grid-body[style*='top: 45px;'] .w2ui-head > div:not(.w2ui-resizer)");
            headers.RemoveAll(p => string.IsNullOrWhiteSpace(p));

            return headers.Exists(p => p.Contains("Value")) && headers.Exists(p => p.Contains("Timestamp"));
        }

        public bool IsSearchCriteriaSectionDisplayed()
        {
            return Driver.FindElements(By.CssSelector("[id^='w2ui-overlay-searches'] div.searchcriteria")).Count > 0;
        }

        public bool IsAdvancedSearchPopupDisplayed()
        {
            return Driver.FindElements(By.CssSelector("[id^='w2ui-overlay-searches']")).Count > 0;
        }

        /// <summary>
        /// Get all toolbar items available from Back Office toolbar config
        /// </summary>
        /// <returns></returns>
        public List<string> GetBackOfficeAvailableToolbarButtons()
        {
            var result = new List<string>();

            var isTimestampToolbarVisible = Driver.FindElements(By.CssSelector("[id$='toolbar-eventtime'] table.w2ui-button")).Count > 0;
            if (isTimestampToolbarVisible)
                result.Add("Timestamp");

            //var isRequestsToolbarVisible = Driver.FindElements(By.CssSelector("[id$='toolbar-requests'] input")).Count > 0;
            //if (isRequestsToolbarVisible)
            //    result.Add("Requests");

            var isMaximumDateVisible = Driver.FindElements(By.CssSelector("[id$='toolbar-maxdatetime'] input")).Count > 0;
            if (isMaximumDateVisible)
                result.Add("Maximum Date");

            var isExportVisible = Driver.FindElements(By.CssSelector("[id$='toolbar-export'] table.w2ui-button")).Count > 0;
            if (isExportVisible)
                result.Add("Export");

            return result;
        }

        public void WaitForAdvancedSearchPanelDisplayed()
        {
            Wait.ForElementDisplayed(By.CssSelector("[id^='w2ui-overlay-searches'].w2ui-overlay"));
            Wait.ForSeconds(2);
        }

        public void WaitForAdvancedSearchPanelDisappeared()
        {
            Wait.ForElementInvisible(By.CssSelector("[id^='w2ui-overlay-searches'].w2ui-overlay"));
            Wait.ForSeconds(2);
        }

        public void WaitForSearchToolbarButtonEnabled()
        {
            WebDriverContext.Wait.Until(driver => driver.FindElements(By.CssSelector("[id$='customreport-grid_toolbar'] [id$='toolbar_item_w2ui-search-advanced'][class='']")).Count > 0);
        }

        public List<string> GetListOfAdvancedSearchFields()
        {            
            return JSUtility.GetElementsText("[id^='w2ui-overlay-searches'] table tr td.caption");
        }

        public void EnterSearchCriteriaFor1stValueInputField(string field, string value)
        {
            foreach (var fieldSection in advancedSearchConditionList)
            {
                var fieldNameElement = fieldSection.FindElement(By.CssSelector("td.caption"));
                if (fieldNameElement.Text == field)
                {
                    var fieldNameInput = fieldSection.FindElement(By.CssSelector("td.value > input"));
                    fieldNameInput.Enter(value);

                    return;
                }
            }
        }

        public void EnterSearchCriteriaFor2ndValueInputField(string field, string value)
        {
            foreach (var fieldSection in advancedSearchConditionList)
            {
                var fieldNameElement = fieldSection.FindElement(By.CssSelector("td.caption"));
                if (fieldNameElement.Text == field)
                {
                    var fieldNameInput = fieldSection.FindElement(By.CssSelector("td.value > span > input"));
                    fieldNameInput.Enter(value);

                    return;
                }
            }
        }

        public void SelectSearchCriteriaForValueDropdownField(string field, string value)
        {
            foreach (var fieldSection in advancedSearchConditionList)
            {
                var fieldNameElement = fieldSection.FindElement(By.CssSelector("td.caption"));
                if (fieldNameElement.Text == field)
                {
                    var dropdown = fieldSection.FindElement(By.CssSelector("td.value .select2-container"));
                    dropdown.Select(value, true);

                    return;
                }
            }
        }

        public void SelectSearchCriteriaForValueMultipleDropdownField(string field, string value)
        {
            foreach (var fieldSection in advancedSearchConditionList)
            {
                var fieldNameElement = fieldSection.FindElement(By.CssSelector("td.caption"));
                if (fieldNameElement.Text == field)
                {
                    fieldNameElement.ScrollToElementByJS(true);
                    Wait.ForSeconds(1);
                    var dropdown = fieldSection.FindElement(By.CssSelector("td.value .select2-container"));
                    dropdown.SelectMultiple(value);

                    return;
                }
            }
        }

        public void RemoveSearchCriteriaForValueDropdownField(string field, string value)
        {
            foreach (var fieldSection in advancedSearchConditionList)
            {
                var fieldNameElement = fieldSection.FindElement(By.CssSelector("td.caption"));
                if (fieldNameElement.Text == field)
                {
                    fieldNameElement.ScrollToElementByJS(true);
                    Wait.ForSeconds(1);
                    var items = fieldSection.FindElements(By.CssSelector("td.value .select2-container .select2-search-choice"));
                    var item = items.FirstOrDefault(p => p.Text.Trim().Equals(value));
                    if(item !=null)
                    {
                        var removeButton = item.FindElement(By.CssSelector("a.select2-search-choice-close"));
                        removeButton.ClickEx();
                    }

                    return;
                }
            }
        }

        public void SelectSearchCriteriaForOperatorDropdownField(string field, string value)
        {
            foreach (var fieldSection in advancedSearchConditionList)
            {
                var fieldNameElement = fieldSection.FindElement(By.CssSelector("td.caption"));
                if (fieldNameElement.Text == field)
                {
                    var dropdown = fieldSection.FindElement(By.CssSelector("td.operator .select2-container"));
                    dropdown.Select(value, true);

                    return;
                }
            }
        }

        public List<string> GetSearchCriteriaOperatorItems(string field)
        {
            foreach (var fieldSection in advancedSearchConditionList)
            {
                var fieldNameElement = fieldSection.FindElement(By.CssSelector("td.caption"));
                if (fieldNameElement.Text == field)
                {
                    var dropdown = fieldSection.FindElement(By.CssSelector("td.operator .select2-container"));
                    return dropdown.GetAllItems();
                }
            }

            return new List<string>();
        }

        public int GetSearchCriteriaInputsCount(string field)
        {
            foreach (var fieldSection in advancedSearchConditionList)
            {
                var fieldNameElement = fieldSection.FindElement(By.CssSelector("td.caption"));
                if (fieldNameElement.Text == field)
                {
                    var count = fieldSection.FindElements(By.CssSelector("td.value input")).Count;                    
                    if (fieldSection.FindElements(By.CssSelector("td.value > span[style*='display: none']")).Any())
                            return count - 1;
                    return count;
                }
            }

            return 0;
        }

        public string GetSearchCriteriaOperatorValue(string field)
        {
            foreach (var fieldSection in advancedSearchConditionList)
            {
                var fieldNameElement = fieldSection.FindElement(By.CssSelector("td.caption"));
                if (fieldNameElement.Text == field)
                {
                    var dropdown = fieldSection.FindElement(By.CssSelector("td.operator .select2-container"));
                    return dropdown.Text;
                }
            }

            return string.Empty;
        }

        public List<string> GetSearchCriteriaDropDownValueItems(string field)
        {
            foreach (var fieldSection in advancedSearchConditionList)
            {
                var fieldNameElement = fieldSection.FindElement(By.CssSelector("td.caption"));
                if (fieldNameElement.Text == field)
                {
                    var dropdown = fieldSection.FindElement(By.CssSelector("td.value .select2-container"));
                    return dropdown.GetAllItems();
                }
            }

            return new List<string>();
        }

        public string GetSearchCriteria()
        {
            return (string)WebDriverContext.JsExecutor.ExecuteScript("return document.querySelector(\"[id^='w2ui-overlay-searches'] div.searchcriteria\").textContent");
        }

        public List<string> GetListOfSearchCriteria()
        {
            return JSUtility.GetElementsText("[id^='w2ui-overlay-searches'] div.searchcriteria span");
        }

        public bool IsDownloadToolbarButtonVisible()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='toolbar-export'] table.w2ui-button a"));
        }

        /// <summary>
        /// Get total rows count of grid data
        /// </summary>
        /// <returns></returns>
        public int GetTotalCount()
        {
            var regexTotal = Regex.Match(footerRightLabel.Text, @".* of (\d{1,})");
            if (regexTotal.Success)
                return int.Parse(regexTotal.Groups[1].ToString());

            return 0;
        }

        /// <summary>
        /// Delete a request (advanced search)
        /// </summary>
        /// <param name="reportName"></param>
        public void DeleleRequest(string requestName, bool confirmed = true)
        {
            SelectSelectOrAddSearchDropDown(requestName);
            WaitForLeftFooterTextDisplayed();
            ClickCancelSearchDropDownButton();
            Page.WaitForPopupDialogDisplayed();
            if (confirmed)
            {
                Page.Dialog.ClickYesButton();
                Page.WaitForPreviousActionComplete();
            }
            else
            {
                Page.Dialog.ClickNoButton();
            }
            Page.WaitForPopupDialogDisappeared();
        }

        /// <summary>
        /// Delete current selected request (advanced search)
        /// </summary>
        /// <param name="confirmed"></param>
        public void DeleleSelectedRequest(bool confirmed = true)
        {
            ClickCancelSearchDropDownButton();
            Page.WaitForPopupDialogDisplayed();
            if (confirmed)
            {
                Page.Dialog.ClickYesButton();
                Page.WaitForPreviousActionComplete();
            }
            else
            {
                Page.Dialog.ClickNoButton();
            }
            Page.WaitForPopupDialogDisappeared();
        }

        /// <summary>
        /// Delete all requests in dropdown
        /// </summary>
        /// <param name="page"></param>
        public void DeleleAllRequests()
        {
            var searchesList = GetListOfSearchDropDownItems();
            foreach (var item in searchesList)
            {
                SelectSelectOrAddSearchDropDown(item);
                WaitForLeftFooterTextDisplayed();
                ClickCancelSearchDropDownButton();
                Page.WaitForPopupDialogDisplayed();
                Page.Dialog.ClickYesButton();
                Page.WaitForPreviousActionComplete();
                Page.WaitForPopupDialogDisappeared();
            }
        }

        /// <summary>
        /// Get data of selected record in grid
        /// </summary>
        /// <returns></returns>
        public List<string> GetSelectedGridRecordData()
        {
            return JSUtility.GetElementsText("[id^='grid'][id*='records'] tr[id^='grid'][id*='rec'].w2ui-selected .w2ui-grid-data div[title]");
        }

        /// <summary>
        /// Get data of record  with specific column and equal a value in grid
        /// </summary>
        /// <returns></returns>
        public DataRow GetGridRecordDataRowEquals(string columnName, string value)
        {
            var dtGrid = BuildDataTableFromGrid();
            var row = dtGrid.Select(string.Format("{0} = '{1}'", columnName, value)).FirstOrDefault();

            return row;
        }

        /// <summary>
        /// Get data of record  with specific column and contains value in grid
        /// </summary>
        /// <returns></returns>
        public DataRow GetGridRecordDataRowContains(string columnName, string value)
        {
            var dtGrid = BuildDataTableFromGrid();
            var row = dtGrid.Select(string.Format("{0} Like '%{1}%'", columnName, value)).FirstOrDefault();

            return row;
        }

        /// <summary>
        /// Tick to select all rows checkbox in grid
        /// </summary>
        /// <returns></returns>
        public void TickAllRowsCheckbox(bool value)
        {
            var checkbox = Driver.FindElement(By.CssSelector("[id$='device-failures-grid-editor_body'] .w2ui-head .checkbox.w2ui-grid-slv-checkbox"));
            checkbox.Check(value);
            Wait.ForSeconds(2);
        }

        public bool IsFailuresRefreshToolbarButtonDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='device-failures-grid-editor_toolbar_item_w2ui-reload'] table.w2ui-button"));
        }

        public bool IsFailuresCommissionToolbarButtonDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='device-failures-grid-editor_toolbar_item_commissioning'] table.w2ui-button"));
        }

        public bool IsFailuresReportingInformationToolbarButtonDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='device-failures-grid-editor_toolbar_item_help'] table.w2ui-button"));
        }

        public bool IsAdvancedSearchesResetButtonDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id^='w2ui-overlay-searches'] .w2ui-grid-searches-reset-btn"));
        }

        public bool IsAdvancedSearchesSearchButtonDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id^='w2ui-overlay-searches'] .w2ui-grid-searches-search-btn"));
        }

        #endregion //Business methods

        public override void WaitForPanelLoaded()
        {
            Wait.ForSeconds(1);
            Wait.ForElementDisplayed(gridContainer);
        }
    }
}
