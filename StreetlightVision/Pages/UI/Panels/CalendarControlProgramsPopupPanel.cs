using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using StreetlightVision.Extensions;
using StreetlightVision.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace StreetlightVision.Pages.UI
{
    public class CalendarControlProgramsPopupPanel : PanelBase
    {
        #region Variables   

        #endregion //Variables

        #region IWebElements       

        [FindsBy(How = How.CssSelector, Using = "[id='w2ui-popup'] .w2ui-msg-title")]
        private IWebElement panelTitle;

        [FindsBy(How = How.CssSelector, Using = "[id='w2ui-popup'] [id$='grid_search_all']")]
        private IWebElement searchInput;

        [FindsBy(How = How.CssSelector, Using = "[id='w2ui-popup'] [id$='grid_searchClear']")]
        private IWebElement clearSearchButton;

        [FindsBy(How = How.CssSelector, Using = "[id='w2ui-popup'] [id$='toolbar_item_w2ui-search-advanced'] > table")]
        private IWebElement searchAdvancedButton;

        [FindsBy(How = How.CssSelector, Using = "[id='w2ui-popup'] [id*='grid_toolbar_item_schedule-selection'][id$='radio-none'] > table")]
        private IWebElement radioNoneButton;

        [FindsBy(How = How.CssSelector, Using = "[id='w2ui-popup'] [id*='grid_toolbar_item_schedule-selection'][id$='radio-last'] > table")]
        private IWebElement radioLastButton;

        [FindsBy(How = How.CssSelector, Using = "[id='w2ui-popup'] [id*='grid_toolbar_item_schedule-selection'][id$='radio-weekly'] > table")]
        private IWebElement radioWeeklyButton;

        [FindsBy(How = How.CssSelector, Using = "[id='w2ui-popup'] [id*='grid_toolbar_item_schedule-selection'][id$='radio-monthly'] > table")]
        private IWebElement radioMonthlyButton;

        [FindsBy(How = How.CssSelector, Using = "[id='w2ui-popup'] [id*='grid_toolbar_item_schedule-selection'][id$='radio-yearly'] > table")]
        private IWebElement radioYearlyButton;

        [FindsBy(How = How.CssSelector, Using = "[id='w2ui-popup'] .w2ui-msg-close")]
        private IWebElement closeButton;

        [FindsBy(How = How.CssSelector, Using = "[id='w2ui-popup'] [id$='btn-undo']")]
        private IWebElement cancelButton;

        [FindsBy(How = How.CssSelector, Using = "[id='w2ui-popup'] [id$='btn-save']")]
        private IWebElement saveButton;   
        
        [FindsBy(How = How.CssSelector, Using = "[id='w2ui-popup'] tr[id*='grid_rec']")]
        private IList<IWebElement> calendarItemsList;

        [FindsBy(How = How.CssSelector, Using = "[id='w2ui-popup'] tr[id*='grid_rec'].w2ui-selected")]
        private IWebElement selectedCalendarItem;        

        #endregion //IWebElements

        #region Constructor

        public CalendarControlProgramsPopupPanel(IWebDriver driver, PageBase page)
            : base(driver, page) 
        {
            PageFactory.InitElements(this, new RetryingElementLocator(driver, TimeSpan.FromMilliseconds(Settings.LocatorRetryingTimeout), TimeSpan.FromMilliseconds(Settings.LocatorPollingInterval)));
        }

        #endregion //Constructor

        #region Properties

        #endregion //Properties

        #region Basic methods  

        #region Actions

        /// <summary>
        /// Enter a value for 'Search' input
        /// </summary>
        /// <param name="value"></param>
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

        /// <summary>
        /// Click 'SearchAdvanced' button
        /// </summary>
        public void ClickSearchAdvancedButton()
        {
            searchAdvancedButton.ClickEx();
        }

        /// <summary>
        /// Click 'RadioNone' button
        /// </summary>
        public void ClickRadioNoneButton()
        {
            radioNoneButton.ClickEx();
        }

        /// <summary>
        /// Click 'RadioLast' button
        /// </summary>
        public void ClickRadioLastButton()
        {
            radioLastButton.ClickEx();
        }

        /// <summary>
        /// Click 'RadioWeekly' button
        /// </summary>
        public void ClickRadioWeeklyButton()
        {
            radioWeeklyButton.ClickEx();
        }

        /// <summary>
        /// Click 'RadioMonthly' button
        /// </summary>
        public void ClickRadioMonthlyButton()
        {
            radioMonthlyButton.ClickEx();
        }

        /// <summary>
        /// Click 'RadioYearly' button
        /// </summary>
        public void ClickRadioYearlyButton()
        {
            radioYearlyButton.ClickEx();
        }     

        /// <summary>
        /// Click 'Close' button
        /// </summary>
        public void ClickCloseButton()
        {
            closeButton.ClickEx();
        }

        /// <summary>
        /// Click 'Cancel' button
        /// </summary>
        public void ClickCancelButton()
        {
            cancelButton.ClickEx();
        }

        /// <summary>
        /// Click 'Save' button
        /// </summary>
        public void ClickSaveButton()
        {
            saveButton.ClickEx();
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

        #endregion //Get methods

        #endregion //Basic methods

        #region Business methods

        /// <summary>
        /// Get Calendar items count
        /// </summary>
        /// <returns></returns>
        public int GetItemsCount()
        {
            return calendarItemsList.Count;
        }

        public List<Color> GetListOfItemsColor()
        {
            var colorsCol = Driver.FindElements(By.CssSelector("[id='w2ui-popup'] tr[id*='grid_rec'] .w2ui-grid-data[col='0'] div"));
            var colors = colorsCol.Select(p => p.GetStyleColorAttr("background-color")).ToList();

            return colors;
        }

        public List<string> GetListOfItemsName()
        {
            return JSUtility.GetElementsText("[id='w2ui-popup'] tr[id*='grid_rec'] .w2ui-grid-data[col='1'] div");
        }        

        public List<string> GetListOfItemsGeozone()
        {
            return JSUtility.GetElementsText("[id='w2ui-popup'] tr[id*='grid_rec'] .w2ui-grid-data[col='2'] div");
        }

        public Color GetSelectedItemColor()
        {
            var colorCol = Driver.FindElement(By.CssSelector("[id='w2ui-popup'] tr[id*='grid_rec'].w2ui-selected .w2ui-grid-data[col='0'] div"));

            return colorCol.GetStyleColorAttr("background-color");
        }

        public string GetSelectedItemName()
        {
            return JSUtility.GetElementText("[id='w2ui-popup'] tr[id*='grid_rec'].w2ui-selected .w2ui-grid-data[col='1'] div");
        }        

        public string GetSelectedItemGeozone()
        {
            return JSUtility.GetElementText("[id='w2ui-popup'] tr[id*='grid_rec'].w2ui-selected .w2ui-grid-data[col='2'] div");
        }

        public List<string> GetListOfOptionsName()
        {
            return JSUtility.GetElementsText("[id='w2ui-popup'] [id*='grid_toolbar_item_schedule-selection'][id*='radio'] .w2ui-tb-caption");
        }

        public string GetSelectedOptionName()
        {
            return JSUtility.GetElementText("[id='w2ui-popup'] [id*='grid_toolbar_item_schedule-selection'][id*='radio'] .checked .w2ui-tb-caption");
        }

        public int GetListOfColumnsCount()
        {
            return Driver.FindElements(By.CssSelector("[id='w2ui-popup'] [id$='grid_body'] [line='0'] [col]")).Count;
        }

        public void SelectItem(string name)
        {
            var gridRecordsBy = By.CssSelector("[id='w2ui-popup'] tr[id*='grid_rec']");
            Wait.ForElementsDisplayed(gridRecordsBy);
            var gridRecordsList = Driver.FindElements(gridRecordsBy);
            var currentRec = gridRecordsList.FirstOrDefault(p => p.FindElement(By.CssSelector(".w2ui-grid-data[col='1']")).Text.Equals(name));
            currentRec.ClickEx();
            Wait.ForElementDisplayed(By.CssSelector("[id='w2ui-popup'] tr[id*='grid_rec'].w2ui-selected"));
            Wait.ForSeconds(1);
        }

        public string SelectRandomItem()
        {
            string name = GetListOfItemsName().PickRandom();
            var gridRecordsBy = By.CssSelector("[id='w2ui-popup'] tr[id*='grid_rec']");
            Wait.ForElementsDisplayed(gridRecordsBy);
            var gridRecordsList = Driver.FindElements(gridRecordsBy);
            var currentRec = gridRecordsList.FirstOrDefault(p => p.FindElement(By.CssSelector(".w2ui-grid-data[col='1']")).Text.Equals(name));
            currentRec.ClickEx();
            Wait.ForElementDisplayed(By.CssSelector("[id='w2ui-popup'] tr[id*='grid_rec'].w2ui-selected"));
            Wait.ForSeconds(1);

            return name;
        }

        public string SelectRandomItem(params string[] exceptList)
        {
            string name = GetListOfItemsName().Where(p => !exceptList.Contains(p)).PickRandom();
            var gridRecordsBy = By.CssSelector("[id='w2ui-popup'] tr[id*='grid_rec']");
            Wait.ForElementsDisplayed(gridRecordsBy);
            var gridRecordsList = Driver.FindElements(gridRecordsBy);
            var currentRec = gridRecordsList.FirstOrDefault(p => p.FindElement(By.CssSelector(".w2ui-grid-data[col='1']")).Text.Equals(name));
            currentRec.ClickEx();
            Wait.ForElementDisplayed(By.CssSelector("[id='w2ui-popup'] tr[id*='grid_rec'].w2ui-selected"));
            Wait.ForSeconds(1);

            return name;
        }

        public bool IsDeleteButtonVisible()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id='w2ui-popup'] [id$='toolbar_item_delete'] > table"));
        }

        public bool IsSearchInputVisible()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id='w2ui-popup'] [id$='grid_search_all']"));
        }

        public bool IsSearchAdvancedButtonVisible()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id='w2ui-popup'] [id$='toolbar_item_w2ui-search-advanced'] > table"));
        }

        public bool IsCancelButtonVisible()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id='w2ui-popup'] [id$='btn-undo']"));
        }

        public bool IsSaveButtonVisible()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id='w2ui-popup'] [id$='btn-save']"));
        }

        #endregion //Business methods
    }
}