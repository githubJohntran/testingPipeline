using NUnit.Framework;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using StreetlightVision.Extensions;
using StreetlightVision.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;

namespace StreetlightVision.Pages.UI
{
    public class CalendarEditorPanel : PanelBase
    {
        #region Variables

        private GridPanel _gridPanel;
        private string _cssDateTemplate = "[id$='main-panel'] > div:nth-child(2) .slv-schedulermanager-scheduler-calendar .w2ui-date[date='{0}']";

        #endregion //Variables

        #region IWebElements

        [FindsBy(How = How.CssSelector, Using = "[id$='main-panel'] > div:nth-child(2) .schedulermanager-editor-title")]
        private IWebElement panelTitle;
        
        [FindsBy(How = How.CssSelector, Using = "[id$='main-panel'] > div:nth-child(2) .schedulermanager-editor-yearbefore-button")]
        private IWebElement yearBeforeButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='main-panel'] > div:nth-child(2) .schedulermanager-editor-currentyear-label")]
        private IWebElement currentYearLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='main-panel'] > div:nth-child(2) .schedulermanager-editor-yearafter-button")]
        private IWebElement yearAfterButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='main-panel'] > div:nth-child(2) .schedulermanager-editor-scheduleritems-button")]
        private IWebElement calendarItemsButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='main-panel'] > div:nth-child(2) .schedulermanager-editor-clear-button")]
        private IWebElement clearButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='main-panel'] > div:nth-child(2) .schedulermanager-editor-save-button")]
        private IWebElement saveButton;
        
        [FindsBy(How = How.CssSelector, Using = "[id$='main-panel'] > div:nth-child(2) .schedulermanager-schedulers-editor-fields-div > div:nth-child(2) label")]
        private IWebElement nameLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='main-panel'] > div:nth-child(2) .schedulermanager-schedulers-editor-fields-div > div:nth-child(2) input")]
        private IWebElement nameInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='main-panel'] > div:nth-child(2).schedulermanager-schedulers-editor-fields-div > div:nth-child(3) label")]
        private IWebElement descriptionLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='main-panel'] > div:nth-child(2) .schedulermanager-schedulers-editor-fields-div > div:nth-child(3) textarea")]
        private IWebElement descriptionInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='main-panel'] > div:nth-child(2) .schedulermanager-schedulers-editor-devicescount-label")]
        private IWebElement devicesCountLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='main-panel'] > div:nth-child(2) .slv-schedulermanager-scheduler-calendar")]
        private IList<IWebElement> calendarList;

        [FindsBy(How = How.CssSelector, Using = "[id$='main-panel'] > div:nth-child(2) .slv-schedulermanager-scheduler-calendar .w2ui-date")]
        private IList<IWebElement> calendarDateList;

        #endregion //IWebElements

        #region Constructor

        public CalendarEditorPanel(IWebDriver driver, PageBase page) : base(driver, page)
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

        /// <summary>
        /// Click 'YearBefore' button
        /// </summary>
        public void ClickYearBeforeButton()
        {
            yearBeforeButton.ClickEx();
        }

        /// <summary>
        /// Click 'YearAfter' button
        /// </summary>
        public void ClickYearAfterButton()
        {
            yearAfterButton.ClickEx();
        }

        /// <summary>
        /// Click 'CalendarItems' button
        /// </summary>
        public void ClickCalendarItemsButton()
        {
            calendarItemsButton.ClickEx();
        }

        /// <summary>
        /// Click 'Clear' button
        /// </summary>
        public void ClickClearButton()
        {
            clearButton.ClickEx();
        }

        /// <summary>
        /// Click 'Save' button
        /// </summary>
        public void ClickSaveButton()
        {
            saveButton.ClickEx();
        }

        /// <summary>
        /// Enter a value for 'Name' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterNameInput(string value)
        {
            nameInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'Description' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterDescriptionInput(string value)
        {
            descriptionInput.Enter(value);
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
        /// Get 'CurrentYear' label text
        /// </summary>
        /// <returns></returns>
        public string GetCurrentYearText()
        {
            return currentYearLabel.Text;
        }

        /// <summary>
        /// Get 'Name' label text
        /// </summary>
        /// <returns></returns>
        public string GetNameText()
        {
            return nameLabel.Text;
        }

        /// <summary>
        /// Get 'Name' input value
        /// </summary>
        /// <returns></returns>
        public string GetNameValue()
        {
            return nameInput.Value();
        }

        /// <summary>
        /// Get 'Description' label text
        /// </summary>
        /// <returns></returns>
        public string GetDescriptionText()
        {
            return descriptionLabel.Text;
        }

        /// <summary>
        /// Get 'Description' input value
        /// </summary>
        /// <returns></returns>
        public string GetDescriptionValue()
        {
            return descriptionInput.Value();
        }

        /// <summary>
        /// Get 'DevicesCount' label text
        /// </summary>
        /// <returns></returns>
        public string GetDevicesCountText()
        {
            return devicesCountLabel.Text;
        }

        #endregion //Get methods

        #endregion //Basic methods

        #region Business methods

        public bool IsNameInputReadOnly()
        {
            return nameInput.IsReadOnly();
        }

        public bool IsDescriptionInputReadOnly()
        {
            return descriptionInput.IsReadOnly();
        }

        public bool IsCalendarItemsButtonVisible()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='main-panel'] > div:nth-child(2) .schedulermanager-editor-scheduleritems-button"));
        }

        public bool IsClearButtonVisible()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='main-panel'] > div:nth-child(2) .schedulermanager-editor-clear-button"));
        }

        public bool IsSaveButtonVisible()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='main-panel'] > div:nth-child(2) .schedulermanager-editor-save-button"));
        }

        public bool IsYearBeforeButtonVisible()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='main-panel'] > div:nth-child(2) .schedulermanager-editor-yearbefore-button"));
        }

        public bool IsYearAfterButtonVisible()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='main-panel'] > div:nth-child(2) .schedulermanager-editor-yearafter-button"));
        }

        public void ClickCalendarDate(string date)
        {
            var curDate = Driver.FindElement(By.CssSelector(string.Format(_cssDateTemplate, date)));
            if (curDate != null)
            {
                curDate.ClickEx();
            }
            else
                throw new Exception("Calendar Date is not valid");
        }

        public string ClickRandomCalendarDate()
        {
            var curDate = calendarDateList.PickRandom();
            curDate.ClickEx();

            return curDate.GetAttribute("date");
        }

        /// <summary>
        /// Drag and drop random days of a week on Calendar Date starting from Monday
        /// </summary>
        /// <param name="year"></param>
        /// <param name="daysCount"></param>
        /// <returns></returns>
        public List<DateTime> DragAndDropRandomDaysInWeek(int year, int daysCount = 7)
        {
            devicesCountLabel.ClickEx();
            var offsetY = 85;
            var result = new List<DateTime>();           
            var firstMonday = Settings.FindFirstDayOfWeek(year, DayOfWeek.Monday);
            var dateFrom = firstMonday.AddDays(new List<int> { 7, 14, 21 }.PickRandom());
            result.Add(dateFrom);
            for (int i = 1; i < daysCount; i++)
            {
                var nextDate = dateFrom.AddDays(i);
                result.Add(nextDate);
            }
            var dateTo = dateFrom.AddDays(daysCount - 1);    
            var dateFromElement = Driver.FindElement(By.CssSelector(string.Format(_cssDateTemplate, dateFrom.ToString("M/d/yyyy"))));
            var dateToElement = Driver.FindElement(By.CssSelector(string.Format(_cssDateTemplate, dateTo.ToString("M/d/yyyy"))));
            dateFromElement.MoveTo();
            WinApiUtility.DragAndDrop(new Point(dateFromElement.Location.X + 5, dateFromElement.Location.Y + offsetY), new Point(dateToElement.Location.X + 15, dateToElement.Location.Y + offsetY), false);

            return result;
        }

        /// <summary>
        /// Drag and drop random days on Calendar Date with specific start day
        /// </summary>
        /// <param name="year"></param>
        /// <param name="startDay"></param>
        /// <param name="daysCount"></param>
        /// <returns></returns>
        public List<DateTime> DragAndDropRandomDays(int year, DayOfWeek startDay, int daysCount = 7)
        {
            devicesCountLabel.ClickEx();
            var offsetY = 85;            
            var result = new List<DateTime>();
            var firstStartDay = Settings.FindFirstDayOfWeek(year, startDay);
            var dateFrom = firstStartDay.AddDays(new List<int> { 7, 14, 21 }.PickRandom());
            result.Add(dateFrom);
            for (int i = 1; i < daysCount; i++)
            {
                var nextDate = dateFrom.AddDays(i);
                result.Add(nextDate);
            }
            var dateTo = dateFrom.AddDays(daysCount - 1);     
            var dateFromElement = Driver.FindElement(By.CssSelector(string.Format(_cssDateTemplate, dateFrom.ToString("M/d/yyyy"))));
            var dateToElement = Driver.FindElement(By.CssSelector(string.Format(_cssDateTemplate, dateTo.ToString("M/d/yyyy"))));
            dateFromElement.MoveTo();
            WinApiUtility.DragAndDrop(new Point(dateFromElement.Location.X + 5, dateFromElement.Location.Y + offsetY), new Point(dateToElement.Location.X + 15, dateToElement.Location.Y + offsetY), false);

            return result;
        }

        /// <summary>
        /// Drag and drop random days on Calendar Date with specific start day
        /// </summary>
        /// <param name="year"></param>
        /// <param name="startDay"></param>
        /// <param name="daysCount"></param>
        /// <returns></returns>
        public List<DateTime> DragAndDropRandomDays(int year, int month, DayOfWeek startDay, int daysCount = 7)
        {
            devicesCountLabel.ClickEx();
            var offsetY = 85;
            var result = new List<DateTime>();
            var firstStartDay = Settings.FindFirstDayOfWeek(year, month, startDay);
            var dateFrom = firstStartDay.AddDays(new List<int> { 7, 14, 21 }.PickRandom());
            result.Add(dateFrom);
            for (int i = 1; i < daysCount; i++)
            {
                var nextDate = dateFrom.AddDays(i);
                result.Add(nextDate);
            }
            var dateTo = dateFrom.AddDays(daysCount - 1);
            var dateFromElement = Driver.FindElement(By.CssSelector(string.Format(_cssDateTemplate, dateFrom.ToString("M/d/yyyy"))));
            var dateToElement = Driver.FindElement(By.CssSelector(string.Format(_cssDateTemplate, dateTo.ToString("M/d/yyyy"))));
            dateFromElement.MoveTo();
            WinApiUtility.DragAndDrop(new Point(dateFromElement.Location.X + 5, dateFromElement.Location.Y + offsetY), new Point(dateToElement.Location.X + 15, dateToElement.Location.Y + offsetY), false);

            return result;
        }

        /// <summary>
        /// Drag and drop from date to date
        /// </summary>
        /// <param name="year"></param>
        /// <param name="startDay"></param>
        /// <param name="daysCount"></param>
        /// <returns></returns>
        public List<DateTime> DragAndDropFromDateToDate(DateTime from, DateTime to)
        {
            devicesCountLabel.ClickEx();
            var offsetY = 85;
            var result = new List<DateTime>();          
            result.Add(from);
            result.Add(to);
            var dateFromElement = Driver.FindElement(By.CssSelector(string.Format(_cssDateTemplate, from.ToString("M/d/yyyy"))));
            var dateToElement = Driver.FindElement(By.CssSelector(string.Format(_cssDateTemplate, to.ToString("M/d/yyyy"))));
            dateFromElement.MoveTo();
            WinApiUtility.DragAndDrop(new Point(dateFromElement.Location.X + 5, dateFromElement.Location.Y + offsetY), new Point(dateToElement.Location.X + 15, dateToElement.Location.Y + offsetY), false);

            return result;
        }

        /// <summary>
        /// Drag and drop the last day of a month and the beginning day of the next month 
        /// </summary>
        /// <param name="year"></param>
        /// <param name="startDay"></param>
        /// <param name="daysCount"></param>
        /// <returns></returns>
        public List<DateTime> DragAndDropLastDayMonthToFirstDayNextMonth(int year, int month)
        {
            devicesCountLabel.ClickEx();
            var offsetY = 85;
            var result = new List<DateTime>();
            var firstDayMonth = new DateTime(year, month, 1);            
            var firstDayNextMonth = firstDayMonth.AddMonths(1);
            var lastDayMonth = firstDayNextMonth.AddDays(-1);
            var dateFrom = lastDayMonth;
            result.Add(dateFrom);
            var dateTo = firstDayNextMonth;
            result.Add(dateTo);
            var dateFromElement = Driver.FindElement(By.CssSelector(string.Format(_cssDateTemplate, dateFrom.ToString("M/d/yyyy"))));
            var dateToElement = Driver.FindElement(By.CssSelector(string.Format(_cssDateTemplate, dateTo.ToString("M/d/yyyy"))));
            dateFromElement.MoveTo();
            WinApiUtility.DragAndDrop(new Point(dateFromElement.Location.X + 5, dateFromElement.Location.Y + offsetY), new Point(dateToElement.Location.X + 15, dateToElement.Location.Y + offsetY), false);

            return result;
        }

        public Color GetCalendarDateColor(string date)
        {
            var curDate = Driver.FindElement(By.CssSelector(string.Format(_cssDateTemplate, date)));
            if (curDate != null)
            {
                return curDate.GetStyleColorAttr("background-color");
            }

            return Color.Empty;
        }

        public void HoverCalendarDate(string date)
        {            
            Page.AppBar.ClickHeaderBartop();
            var curDate = Driver.FindElement(By.CssSelector(string.Format(_cssDateTemplate, date)));
            if (curDate != null)
            {
                if (curDate.GetStyleColorAttr("background-color").IsEmpty) return;
                curDate.MoveTo();
                if (Browser.Name.Equals("Chrome"))
                    Wait.ForMilliseconds(500); // Tooltip sometimes not displayed when hovering over calendar with Chrome
                if (!IsTooltipDateDisplayed())
                    Assert.Warn(string.Format("[SC-1074] Scheduling Manager - Control program tooltip sometimes not displayed when hovering over calendar with Chrome (Issue on date: {0})", date));
            }
        }

        public string GetTooltipDateTitleIcon()
        {
            var iconElement = Driver.FindElement(By.CssSelector("[id='w2ui-overlay'] .slv-schedulermanager-scheduler-date-overlay-recurrence div:nth-child(1)"));

            return iconElement.GetStyleAttr("background-image");
        }

        public string GetTooltipDateTitleText()
        {
            return JSUtility.GetElementText("[id='w2ui-overlay'] .slv-schedulermanager-scheduler-date-overlay-recurrence div:nth-child(2)");
        }

        public string GetTooltipDateControlProgramName()
        {
            return JSUtility.GetElementText("[id='w2ui-overlay'] .slv-schedulermanager-scheduler-date-overlay-schedule div:nth-child(2)");
        }

        public bool IsTooltipDateDisplayed()
        {            
            return ElementUtility.IsDisplayed(By.CssSelector("[id='w2ui-overlay']"));
        }

        /// <summary>
        /// Get devices count are using current calendar
        /// </summary>
        /// <returns></returns>
        public int GetDevicesUsed()
        {
            var pattern = @"(\d{1,}) devices are using this calendar";
            var devicesRegex = Regex.Match(GetDevicesCountText(), pattern);
            if (devicesRegex.Success)
                return int.Parse(devicesRegex.Groups[1].Value);

            return 0;
        }

        /// <summary>
        /// Get get of calendar months name
        /// </summary>
        /// <returns></returns>
        public List<string> GetListOfCalendarMonthsName()
        {
            return JSUtility.GetElementsText("[id$='main-panel'] > div:nth-child(2) .slv-schedulermanager-scheduler-calendar .title");
        }

        /// <summary>
        /// Get dictionary with date and color
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, Color> GetCalendars()
        {
            var result = new Dictionary<string, Color>();

            var dates = JSUtility.GetElementsAttributeValue("[id$='main-panel'] > div:nth-child(2) .slv-schedulermanager-scheduler-calendar .w2ui-date", "date");
            foreach (var date in dates)
            {
                var color = GetCalendarDateColor(date);
                if (!result.ContainsKey(date))
                    result.Add(date, color);
            }

            return result;
        }
        
        /// <summary>
        /// Clear Name input (and grid updated)
        /// </summary>
        public void ClearNameInput()
        {
            nameInput.SendKeys(Keys.Control + "a");
            nameInput.SendKeys(" ");
            nameInput.SendKeys(Keys.Backspace);
            Wait.ForSeconds(2);
        }

        #endregion //Business methods
    }
}
