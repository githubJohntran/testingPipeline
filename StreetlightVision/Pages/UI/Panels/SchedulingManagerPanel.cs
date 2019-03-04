using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using StreetlightVision.Extensions;
using StreetlightVision.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;

namespace StreetlightVision.Pages.UI
{
    public class SchedulingManagerPanel : PanelBase
    {
        #region Variables

        private GridPanel _gridPanel;

        #endregion //Variables

        #region IWebElements

        [FindsBy(How = How.CssSelector, Using = "[id$='left-panel'] div.slv-panel-driven-layout-left-panel-title")]
        private IWebElement panelTitle;

        [FindsBy(How = How.CssSelector, Using = "[id$='left-panel'] .slv-panel-driven-layout-left-panel-close-button")]
        private IWebElement closeButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='leftpanel-tabs'] tr > td > div")]
        private IList<IWebElement> tabsList;

        [FindsBy(How = How.CssSelector, Using = "[id$='leftpanel-tabs'] tr > td > div.w2ui-tab.active")]
        private IWebElement activeTab;

        #region Control Program

        [FindsBy(How = How.CssSelector, Using = "[id$='schedules-SchedulesListView_search_all']")]
        private IWebElement searchControlProgramInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='schedules-SchedulesListView_toolbar_item_w2ui-search-advanced'] table.w2ui-button")]
        private IWebElement searchControlProgramButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='schedules-SchedulesListView_searchClear']")]
        private IWebElement clearSearchControlProgramButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='schedules-SchedulesListView_toolbar_item_add'] table.w2ui-button")]
        private IWebElement addControlProgramButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='schedules-SchedulesListView_toolbar_item_delete'] table.w2ui-button")]
        private IWebElement deleteControlProgramButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='schedules-SchedulesListView_toolbar_item_duplicate'] table.w2ui-button")]
        private IWebElement duplicateControlProgramButton;

        #endregion //Control Program

        #region Calendar

        [FindsBy(How = How.CssSelector, Using = "[id$='schedulers-SchedulersListView_search_all']")]
        private IWebElement searchCalendarInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='schedulers-SchedulersListView_toolbar_item_w2ui-search-advanced'] table.w2ui-button")]
        private IWebElement searchCalendarButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='schedulers-SchedulersListView_searchClear']")]
        private IWebElement clearSearchCalendarButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='schedulers-SchedulersListView_toolbar_item_add'] table.w2ui-button")]
        private IWebElement addCalendarButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='schedulers-SchedulersListView_toolbar_item_delete'] table.w2ui-button")]
        private IWebElement deleteCalendarButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='schedulers-SchedulersListView_toolbar_item_duplicate'] table.w2ui-button")]
        private IWebElement duplicateCalendarButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='schedulers-SchedulersListView_toolbar_item_commissioning'] table.w2ui-button")]
        private IWebElement commissioningCalendarButton;

        #endregion //Calendar

        #region Failures

        [FindsBy(How = How.CssSelector, Using = "[id$='failures-FailuresListView_body'] .w2ui-head .checkbox.w2ui-grid-slv-checkbox")]
        private IWebElement allCalendarFailuresCheckbox;
        
        [FindsBy(How = How.CssSelector, Using = "[id$='failures-FailuresListView_search_all']")]
        private IWebElement searchAllFailuresInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='failures-FailuresListView_toolbar_item_w2ui-search-advanced'] table.w2ui-button")]
        private IWebElement searchFailuresButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='failures-FailuresListView_searchClear'].w2ui-search-clear")]
        private IWebElement clearSearchFailuresButton;

        #endregion //Failures

        #endregion //IWebElements

        #region Constructor

        public SchedulingManagerPanel(IWebDriver driver, PageBase page) : base(driver, page)
        {
            PageFactory.InitElements(this, new RetryingElementLocator(driver, TimeSpan.FromMilliseconds(Settings.LocatorRetryingTimeout), TimeSpan.FromMilliseconds(Settings.LocatorPollingInterval)));
        }

        #endregion //Constructor

        #region Properties

        public GridPanel GridPanel
        {
            get
            {
                if (_gridPanel == null)
                {
                    _gridPanel = new GridPanel(this.Driver, this.Page);
                }

                return _gridPanel;
            }
        }

        #endregion //Properties

        #region Basic methods      

        #region Actions

        #region IWebElements

        /// <summary>
        /// Click 'Close' button
        /// </summary>
        public void ClickCloseButton()
        {
            closeButton.ClickEx();
        }

        #region Control Program

        /// <summary>
        /// Enter a value for 'SearchControlProgram' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterSearchControlProgramInput(string value)
        {
            searchControlProgramInput.Enter(value);
        }

        /// <summary>
        /// Click 'SearchControlProgram' button
        /// </summary>
        public void ClickSearchControlProgramButton()
        {
            searchControlProgramButton.ClickEx();
        }

        /// <summary>
        /// Click 'Clear SearchControlProgram' button
        /// </summary>
        public void ClickClearSearchControlProgramButton()
        {
            clearSearchControlProgramButton.ClickEx();
        }

        /// <summary>
        /// Click 'AddControlProgram' button
        /// </summary>
        public void ClickAddControlProgramButton()
        {
            addControlProgramButton.ClickEx();
        }

        /// <summary>
        /// Click 'DeleteControlProgram' button
        /// </summary>
        public void ClickDeleteControlProgramButton()
        {
            deleteControlProgramButton.ClickEx();
        }

        /// <summary>
        /// Click 'DuplicateControlProgram' button
        /// </summary>
        public void ClickDuplicateControlProgramButton()
        {
            duplicateControlProgramButton.ClickEx();
        }

        #endregion //Control Program

        #region Calendar

        /// <summary>
        /// Enter a value for 'SearchCalendar' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterSearchCalendarInput(string value)
        {
            searchCalendarInput.Enter(value);
        }

        /// <summary>
        /// Click 'SearchCalendar' button
        /// </summary>
        public void ClickSearchCalendarButton()
        {
            searchCalendarButton.ClickEx();
        }

        /// <summary>
        /// Click 'ClearSearchCalendar' button
        /// </summary>
        public void ClickClearSearchCalendarButton()
        {
            clearSearchCalendarButton.ClickEx();
        }

        /// <summary>
        /// Click 'AddCalendar' button
        /// </summary>
        public void ClickAddCalendarButton()
        {
            addCalendarButton.ClickEx();
        }

        /// <summary>
        /// Click 'DeleteCalendar' button
        /// </summary>
        public void ClickDeleteCalendarButton()
        {
            deleteCalendarButton.ClickEx();
        }

        /// <summary>
        /// Click 'DuplicateCalendar' button
        /// </summary>
        public void ClickDuplicateCalendarButton()
        {
            duplicateCalendarButton.ClickEx();
        }

        /// <summary>
        /// Click 'CommissioningCalendar' button
        /// </summary>
        public void ClickCommissioningCalendarButton()
        {
            commissioningCalendarButton.ClickEx();
        }

        #endregion //Calendar

        #region Failures

        /// <summary>
        /// Tick 'AllCalendarFailures' checkbox
        /// </summary>
        /// <param name="value"></param>
        public void TickAllCalendarFailuresCheckbox(bool value)
        {
            allCalendarFailuresCheckbox.Check(value);
        }

        /// <summary>
        /// Enter a value for 'SearchAll' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterSearchAllInput(string value)
        {
            searchAllFailuresInput.Enter(value);
        }

        /// <summary>
        /// Click 'SearchFailures' button
        /// </summary>
        public void ClickSearchFailuresButton()
        {
            searchFailuresButton.ClickEx();
        }

        /// <summary>
        /// Click 'ClearSearchFailures' button
        /// </summary>
        public void ClickClearSearchFailuresButton()
        {
            clearSearchFailuresButton.ClickEx();
        }

        #endregion //Failures

        #endregion //IWebElements

        #endregion //Actions

        #region Get methods

        #region IWebElements

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

        #region Control Program

        /// <summary>
        /// Get 'SearchControlProgram' input value
        /// </summary>
        /// <returns></returns>
        public string GetSearchControlProgramValue()
        {
            return searchControlProgramInput.Value();
        }

        #endregion //Control Program

        #region Calendar

        /// <summary>
        /// Get 'SearchCalendar' input value
        /// </summary>
        /// <returns></returns>
        public string GetSearchCalendarValue()
        {
            return searchCalendarInput.Value();
        }

        #endregion //Calendar

        #region Failures

        /// <summary>
        /// Get 'AllCalendarFailures' checkbox
        /// </summary>
        /// <returns></returns>
        public bool GetAllCalendarFailuresCheckboxValue()
        {
            return allCalendarFailuresCheckbox.CheckboxValue();
        }

        /// <summary>
        /// Get 'SearchAllFailures' input value
        /// </summary>
        /// <returns></returns>
        public string GetSearchAllFailuresValue()
        {
            return searchAllFailuresInput.Value();
        }

        #endregion //Failures

        #endregion //IWebElements

        #endregion //Get methods

        #endregion //Basic methods

        #region Business methods        

        /// <summary>
        /// Select a tab with a specific name
        /// </summary>
        /// <param name="name"></param>
        public void SelectTab(string name)
        {
            Wait.ForElementText(activeTab);
            var currentActiveTab = GetActiveTabText();
            if (!currentActiveTab.Equals(name))
            {
                var tab = tabsList.FirstOrDefault(p => p.Text.Equals(name));
                if (tab != null)
                {
                    tab.ClickEx();
                    WaitForPreviousActionComplete();
                    WebDriverContext.Wait.Until(driver => JSUtility.GetElementText("[id$='leftpanel-tabs'] tr > td > div.w2ui-tab.active") == name);
                }
            }
        }

        #region Control Program

        /// <summary>
        /// Build data table from control program grid
        /// </summary>
        /// <returns></returns>
        public DataTable BuildDataTableFromControlProgramGrid()
        {
            Wait.ForSeconds(3);
            var gridContainer = Driver.FindElement(By.CssSelector("[id$='schedules-SchedulesListView_body']"));
            DataTable tblResult = gridContainer.BuildDataTableFromGrid();

            return tblResult;
        }

        /// <summary>
        /// Get color of selected row in ControlProgram grid
        /// </summary>
        /// <returns></returns>
        public Color GetSelectedControlProgramColor()
        {
            var colorElement = Driver.FindElement(By.CssSelector("[id$='schedules-SchedulesListView_body'] [id*='schedules-SchedulesListView_rec'].w2ui-selected td.w2ui-grid-data[col='0'] div div:nth-child(1)"));
            return colorElement.GetStyleColorAttr("background-color"); ;
        }

        /// <summary>
        /// Get name of selected row in ControlProgram grid
        /// </summary>
        /// <returns></returns>
        public string GetSelectedControlProgramName()
        {
            var nameElement = Driver.FindElement(By.CssSelector("[id$='schedules-SchedulesListView_body'] [id*='schedules-SchedulesListView_rec'].w2ui-selected td.w2ui-grid-data[col='1'] div"));
            return nameElement.Text.Trim();
        }

        /// <summary>
        /// Get geozone of selected row in ControlProgram grid
        /// </summary>
        /// <returns></returns>
        public string GetSelectedControlProgramGeozone()
        {
            var geozoneElement = Driver.FindElement(By.CssSelector("[id$='schedules-SchedulesListView_body'] [id*='schedules-SchedulesListView_rec'].w2ui-selected td.w2ui-grid-data[col='2'] div"));
            return geozoneElement.Text.Trim();
        }

        public List<string> GetListOfControlProgramName()
        {
            var tblGrid = BuildDataTableFromControlProgramGrid();
            var result = tblGrid.AsEnumerable().Select(r => r.Field<string>("Name")).ToList();

            return result;
        }

        public bool IsControlProgramPresentInGrid(string name)
        {
            var list = GetListOfControlProgramName();
            return list.Exists(p => p.Equals(name));
        }

        public void SelectControlProgram(string name)
        {
            var gridRecordsBy = By.CssSelector("[id$='schedules-SchedulesListView_body'] [id^='grid'][id*='records'] tr[id^='grid'][id*='rec']");
            Wait.ForElementsDisplayed(gridRecordsBy);
            var gridRecordsList = Driver.FindElements(gridRecordsBy);
            var currentRec = gridRecordsList.FirstOrDefault(p => p.Text.Contains(name));
            currentRec.ClickEx();
        }

        public void SelectControlProgram(string name, string geozoneName)
        {
            var gridRecordsBy = By.CssSelector("[id$='schedules-SchedulesListView_body'] [id^='grid'][id*='records'] tr[id^='grid'][id*='rec']");
            Wait.ForElementsDisplayed(gridRecordsBy);
            var gridRecordsList = Driver.FindElements(gridRecordsBy);
            var currentRec = gridRecordsList.FirstOrDefault(p => p.Text.Contains(name) && p.Text.Contains(geozoneName));
            currentRec.ClickEx();
        }

        public bool IsAddControlProgramButtonVisible()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='schedules-SchedulesListView_toolbar_item_add'] table.w2ui-button"));
        }

        public bool IsDeleteControlProgramButtonVisible()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='schedules-SchedulesListView_toolbar_item_delete'] table.w2ui-button"));
        }

        public bool IsDuplicateControlProgramButtonVisible()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='schedules-SchedulesListView_toolbar_item_duplicate'] table.w2ui-button"));
        }

        #endregion //Control Program 

        #region Calendar

        /// <summary>
        /// Build data table from calendar grid
        /// </summary>
        /// <returns></returns>
        public DataTable BuildDataTableFromCalendarGrid()
        {
            Wait.ForSeconds(2);
            var gridContainer = Driver.FindElement(By.CssSelector("[id$='schedulers-SchedulersListView_body']"));
            DataTable tblResult = gridContainer.BuildDataTableFromGrid();

            return tblResult;
        }

        /// <summary>
        /// Get name of selected row in Calendar grid
        /// </summary>
        /// <returns></returns>
        public string GetSelectedCalendarName()
        {
            var nameElement = Driver.FindElement(By.CssSelector("[id$='schedulers-SchedulersListView_body'] [id*='schedulers-SchedulersListView_rec'].w2ui-selected td.w2ui-grid-data[col='1'] div"));
            return nameElement.Text.TrimEx();
        }

        /// <summary>
        /// Get geozone of selected row in Calendar grid
        /// </summary>
        /// <returns></returns>
        public string GetSelectedCalendarGeozone()
        {
            var geozoneElement = Driver.FindElement(By.CssSelector("[id$='schedulers-SchedulersListView_body'] [id*='schedulers-SchedulersListView_rec'].w2ui-selected td.w2ui-grid-data[col='2'] div"));
            return geozoneElement.Text.TrimEx();
        }

        public List<string> GetListOfCalendarName()
        {
            var tblGrid = BuildDataTableFromCalendarGrid();
            var result = tblGrid.AsEnumerable().Select(r => r.Field<string>("Name")).ToList();

            return result;
        }

        public bool IsCalendarPresentInGrid(string name)
        {
            var list = GetListOfCalendarName();
            return list.Exists(p => p.Equals(name));
        }

        /// <summary>
        /// Check if a calendar is using or not
        /// </summary>
        /// <returns></returns>
        public bool IsCalendarUsed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='schedulers-SchedulersListView_body'] [id*='schedulers-SchedulersListView_rec'].w2ui-selected td.w2ui-grid-data[col='0'] > div > div.scheduler-icon-used"));
        }

        /// <summary>
        /// Select a calendar in grid with specific name
        /// </summary>
        /// <param name="name"></param>
        public void SelectCalendar(string name)
        {
            var gridRecordsBy = By.CssSelector("[id$='schedulers-SchedulersListView_body'] [id^='grid'][id*='records'] tr[id^='grid'][id*='rec']");
            Wait.ForElementsDisplayed(gridRecordsBy);
            var gridRecordsList = Driver.FindElements(gridRecordsBy);
            var currentRec = gridRecordsList.FirstOrDefault(p => p.Text.IndexOf(name) >= 0);
            currentRec.ClickEx();
        }

        /// <summary>
        /// Select a random calendar in grid
        /// </summary>
        public void SelectRandomCalendar()
        {
            var calendars = GetListOfCalendarName();
            calendars.Remove(GetSelectedCalendarName());
            SelectCalendar(calendars.PickRandom());
        }

        public bool IsCommissionCalendarButtonVisible()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='schedulers-SchedulersListView_toolbar_item_commissioning'] table.w2ui-button"));
        }

        public bool IsAddCalendarButtonVisible()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='schedulers-SchedulersListView_toolbar_item_add'] table.w2ui-button"));
        }

        public bool IsDeleteCalendarButtonVisible()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='schedulers-SchedulersListView_toolbar_item_delete'] table.w2ui-button"));
        }

        public bool IsDuplicateCalendarButtonVisible()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='schedulers-SchedulersListView_toolbar_item_duplicate'] table.w2ui-button"));
        }

        public void WaitForCalendarListContains(string item)
        {
            GenericOperation<bool>.Retry(() => GetListOfCalendarName().Contains(item), (c) => c == true, 30);
        }

        #endregion //Calendar

        #region Failures

        /// <summary>
        /// Build data table from failures grid
        /// </summary>
        /// <returns></returns>
        public DataTable BuildDataTableFromFailuresGrid()
        {
            Wait.ForSeconds(2);
            var gridContainer = Driver.FindElement(By.CssSelector("[id$='failures-FailuresListView_body']"));
            DataTable tblResult = gridContainer.BuildDataTableFromGrid();

            return tblResult;
        }

        /// <summary>
        /// Get Calendar name of selected row in Failures grid
        /// </summary>
        /// <returns></returns>
        public string GetSelectedFailuresCalendar()
        {
            var nameElement = Driver.FindElement(By.CssSelector("[id$='failures-FailuresListView_body'] [id*='failures-FailuresListView_rec'].w2ui-selected td.w2ui-grid-data[col='0'] div"));
            return nameElement.Text.Trim();
        }

        /// <summary>
        /// Get devices of selected row in Failures grid
        /// </summary>
        /// <returns></returns>
        public int GetSelectedFailuresDevices()
        {
            var devicesElement = Driver.FindElement(By.CssSelector("[id$='failures-FailuresListView_body'] [id*='failures-FailuresListView_rec'].w2ui-selected td.w2ui-grid-data[col='1'] div"));
            var devices = devicesElement.Text.Trim();

            return string.IsNullOrEmpty(devices) ? 0 : int.Parse(devices);
        }

        /// <summary>
        /// Get geozone of selected row in Failures grid
        /// </summary>
        /// <returns></returns>
        public string GetSelectedFailuresGeozone()
        {
            var geozoneElement = Driver.FindElement(By.CssSelector("[id$='failures-FailuresListView_body'] [id*='failures-FailuresListView_rec'].w2ui-selected td.w2ui-grid-data[col='2'] div"));
            return geozoneElement.Text.Trim();
        }

        public List<string> GetListOfFailuresCalendarName()
        {
            var tblGrid = BuildDataTableFromFailuresGrid();
            var result = tblGrid.AsEnumerable().Select(r => r.Field<string>("Calendar Name")).ToList();

            return result;
        }

        public int GetFailuresDevicesCount()
        {
            var tblGrid = BuildDataTableFromFailuresGrid();
            var result = tblGrid.AsEnumerable().Select(r => r.Field<string>("Devices")).ToList();
            var count = result.Sum(p => string.IsNullOrEmpty(p) ? 0 : int.Parse(p));

            return count;
        }

        public int GetFailuresDevicesCount(string calendar)
        {
            var tblGrid = BuildDataTableFromFailuresGrid();
            var rows = tblGrid.Select(string.Format("[Calendar Name] = '{0}'", calendar));
            if (rows.Any())
            {
                var row = rows.FirstOrDefault();
                return int.Parse(rows[0]["Devices"].ToString());
            }

            return 0;
        }

        public List<string> GetListOfFailuresGeozone()
        {
            var tblGrid = BuildDataTableFromFailuresGrid();
            var result = tblGrid.AsEnumerable().Select(r => r.Field<string>("GeoZone")).ToList();

            return result;
        }        

        public bool IsFailuresCalendarPresentInGrid(string name)
        {
            var list = GetListOfFailuresCalendarName();
            return list.Exists(p => p.Equals(name));
        }

        public void SelectFailuresCalendar(string name)
        {
            var gridRecordsBy = By.CssSelector("[id$='failures-FailuresListView_body'] [id^='grid'][id*='records'] tr[id^='grid'][id*='rec']");
            Wait.ForElementsDisplayed(gridRecordsBy);
            var gridRecordsList = Driver.FindElements(gridRecordsBy);
            var currentRec = gridRecordsList.FirstOrDefault(p => p.Text.Contains(name));
            currentRec.ClickEx();
            Wait.ForSeconds(1);
        }        
        
        public void WaitForFailuresCalendarListContains(string item)
        {
            GenericOperation<bool>.Retry(() => GetListOfCalendarName().Contains(item), (c) => c == true, 30);
        }

        public void WaitForClearSeachButtonDisappered()
        {
            Wait.ForElementsInvisible(By.CssSelector("[id$='failures-FailuresListView_searchClear'].w2ui-search-clear"));
        }

        /// <summary>
        /// Get list of columns header of Failures grid
        /// </summary>
        /// <returns></returns>
        public List<string> GetListOfColumnsHeaderFailures()
        {
            var gridContainer = Driver.FindElement(By.CssSelector("[id$='failures-FailuresListView_body']"));
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

        public bool IsFailuresGridHasColumnCheckbox()
        {
            return Driver.FindElements(By.CssSelector("[id$='failures-FailuresListView_body'] .w2ui-grid-columns input[type='checkbox']")).Any();
        }

        public bool IsFailuresGridRecordHasCheckbox(string textRecord)
        {
            var records = Driver.FindElements(By.CssSelector("[id$='failures-FailuresListView_body'] [id^='grid'][id*='records'] tr[id^='grid'][id*='rec'"));

            foreach (var record in records)
            {
                var columnsData = record.FindElements(By.CssSelector(".w2ui-grid-data div[title]")).Select(p => p.Text.Trim());
                if (columnsData.Contains(textRecord))
                {
                    var hasCheckbox = record.FindElements(By.CssSelector("input[type='checkbox']")).Any();
                    return hasCheckbox;
                }
            }

            return false;
        }

        public bool AreFailuresCalendarGridRecordsUnchecked()
        {
            var records = Driver.FindElements(By.CssSelector("[id$='failures-FailuresListView_body'] [id^='grid'][id*='records'] tr[id^='grid'][id*='rec'"));
            foreach (var record in records)
            {
                var checkbox = record.FindElement(By.CssSelector(".checkbox.w2ui-grid-slv-checkbox"));                
                if (checkbox.CheckboxValue())
                {                    
                    return false;
                }
            }

            return true;
        }

        public bool AreFailuresCalendarGridRecordsChecked()
        {
            var records = Driver.FindElements(By.CssSelector("[id$='failures-FailuresListView_body'] [id^='grid'][id*='records'] tr[id^='grid'][id*='rec'"));
            foreach (var record in records)
            {
                var checkbox = record.FindElement(By.CssSelector(".checkbox.w2ui-grid-slv-checkbox"));
                if (!checkbox.CheckboxValue())
                {
                    return false;
                }
            }

            return true;
        }

        public bool IsFailuresAllFieldSearchTextboxDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='failures-FailuresListView_search_all']"));
        }

        public bool IsFailuresSearchButtonDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='failures-FailuresListView_toolbar_item_w2ui-search-advanced'] table.w2ui-button"));
        }

        /// <summary>
        /// Tick checkbox of a record contains specific text
        /// </summary>
        /// <param name="textRecord"></param>
        /// <param name="value"></param>
        public void TickFailuresCalendarGridRecordCheckbox(string textRecord, bool value)
        {
            var records = Driver.FindElements(By.CssSelector("[id$='failures-FailuresListView_body'] [id^='grid'][id*='records'] tr[id^='grid'][id*='rec'"));
            foreach (var record in records)
            {
                var columnsData = record.FindElements(By.CssSelector(".w2ui-grid-data div[title]")).Select(p => p.Text.Trim());
                if (columnsData.Contains(textRecord))
                {
                    var checkbox = record.FindElements(By.CssSelector(".checkbox.w2ui-grid-slv-checkbox")).FirstOrDefault();
                    if (checkbox != null)
                    {
                        checkbox.Check(value);
                        return;
                    }
                }
            }
        }

        #endregion //Failures

        #endregion //Business methods

        public override void WaitForPanelLoaded()
        {
            Wait.ForSeconds(2);
        }

        public override void WaitForPreviousActionComplete()
        {
            base.WaitForPreviousActionComplete();
        }
    }
}
