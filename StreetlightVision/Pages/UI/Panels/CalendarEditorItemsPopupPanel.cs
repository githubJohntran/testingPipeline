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
    public class CalendarEditorItemsPopupPanel : PanelBase
    {
        #region Variables   

        #endregion //Variables

        #region IWebElements       

        [FindsBy(How = How.CssSelector, Using = "[id='w2ui-popup'] .w2ui-msg-title")]
        private IWebElement panelTitle;

        [FindsBy(How = How.CssSelector, Using = "[id='w2ui-popup'] .w2ui-msg-close")]
        private IWebElement closeButton;

        [FindsBy(How = How.CssSelector, Using = "[id='w2ui-popup'] [id$='btn-undo']")]
        private IWebElement cancelButton;

        [FindsBy(How = How.CssSelector, Using = "[id='w2ui-popup'] [id$='btn-save']")]
        private IWebElement saveButton;

        [FindsBy(How = How.CssSelector, Using = "[id='w2ui-popup'] [id$='toolbar_item_up'] > table")]
        private IWebElement upButton;

        [FindsBy(How = How.CssSelector, Using = "[id='w2ui-popup'] [id$='toolbar_item_down'] > table")]
        private IWebElement downButton;

        [FindsBy(How = How.CssSelector, Using = "[id='w2ui-popup'] [id$='toolbar_item_delete'] > table")]
        private IWebElement deleteButton;

        [FindsBy(How = How.CssSelector, Using = "[id='w2ui-popup'] tr[id*='grid_rec']")]
        private IList<IWebElement> calendarItemsList;

        [FindsBy(How = How.CssSelector, Using = "[id='w2ui-popup'] tr[id*='grid_rec'].w2ui-selected")]
        private IWebElement selectedCalendarItem;

        #endregion //IWebElements

        #region Constructor

        public CalendarEditorItemsPopupPanel(IWebDriver driver, PageBase page)
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

        /// <summary>
        /// Click 'Up' button
        /// </summary>
        public void ClickUpButton()
        {
            upButton.ClickEx();
        }

        /// <summary>
        /// Click 'Down' button
        /// </summary>
        public void ClickDownButton()
        {
            downButton.ClickEx();
        }

        /// <summary>
        /// Click 'Delete' button
        /// </summary>
        public void ClickDeleteButton()
        {
            deleteButton.ClickEx();
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

        public Color GetSelectedItemColor()
        {
            var colorCol = Driver.FindElement(By.CssSelector("[id='w2ui-popup'] tr[id*='grid_rec'].w2ui-selected .w2ui-grid-data[col='0'] div"));

            return colorCol.GetStyleColorAttr("background-color");
        }

        public string GetSelectedItemName()
        {
            return JSUtility.GetElementText("[id='w2ui-popup'] tr[id*='grid_rec'].w2ui-selected .w2ui-grid-data[col='1'] div");
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

        public Color GetColorOfItem(string name)
        {
            var gridRecordsBy = By.CssSelector("[id='w2ui-popup'] tr[id*='grid_rec']");
            Wait.ForElementsDisplayed(gridRecordsBy);
            var gridRecordsList = Driver.FindElements(gridRecordsBy);
            var currentRec = gridRecordsList.FirstOrDefault(p => p.FindElement(By.CssSelector(".w2ui-grid-data[col='1']")).Text.Equals(name));
            if (currentRec != null)
            {
                var colColumn = currentRec.FindElements(By.CssSelector(".w2ui-grid-data[col='0'] div")).FirstOrDefault();
                if (colColumn != null)
                {
                    var color = colColumn.GetStyleColorAttr("background-color");
                    return color;
                }
                return Color.Empty;
            }

            return Color.Empty;
        }

        public int GetListOfColumnsCount()
        {
            return Driver.FindElements(By.CssSelector("[id='w2ui-popup'] [id$='grid_body'] [line='0'] [col]")).Count;
        }

        public bool IsDeleteButtonVisible()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id='w2ui-popup'] [id$='toolbar_item_delete'] > table"));
        }

        public bool IsUpButtonVisible()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id='w2ui-popup'] [id$='toolbar_item_up'] > table"));
        }

        public bool IsDownButtonVisible()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id='w2ui-popup'] [id$='toolbar_item_down'] > table"));
        }

        public bool IsCancelButtonVisible()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id='w2ui-popup'] [id$='btn-undo']"));
        }

        public bool IsSaveButtonVisible()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id='w2ui-popup'] [id$='btn-save']"));
        }

        public bool HasSelectedItem()
        {
            return Driver.FindElements(By.CssSelector("[id='w2ui-popup'] tr[id*='grid_rec'].w2ui-selected")).Any();
        }

        public bool IsItemExisting(string name)
        {
            var listItems = GetListOfItemsName();

            return listItems.Exists(p => p.Equals(name));
        }

        #region Popup Message

        /// <summary>
        /// Wait for poup message displayed
        /// </summary>
        public void WaitForPopupMessageDisplayed()
        {
            Wait.ForElementDisplayed(By.CssSelector("[id='w2ui-popup'] [id='w2ui-message0']"));
            Wait.ForMilliseconds(500);
        }

        /// <summary>
        /// Wait for poup message disappeared
        /// </summary>
        public void WaitForPopupMessageDisappeared()
        {
            Wait.ForElementInvisible(By.CssSelector("[id='w2ui-popup'] [id='w2ui-message0']"));
            Wait.ForMilliseconds(500);
        }

        public bool IsPopupMessageDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id='w2ui-popup'] [id='w2ui-message0']"));
        }

        public bool IsYesButtonDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id='w2ui-message0'].w2ui-popup-message [id='Yes']"));
        }

        public bool IsNoButtonDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id='w2ui-message0'].w2ui-popup-message [id='No']"));
        }

        public bool IsOkButtonDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id='w2ui-message0'].w2ui-popup-message [onclick^='w2popup.message']"));
        }

        public string GetMessageText()
        {
            return JSUtility.GetElementText("[id='w2ui-popup'] [id='w2ui-message0'] .w2ui-centered");
        }

        public void ClickPopupMessageYesButton()
        {
            Driver.FindElement(By.CssSelector("[id='w2ui-message0'].w2ui-popup-message [id='Yes']")).ClickEx();
        }

        public void ClickPopupMessageNoButton()
        {
            Driver.FindElement(By.CssSelector("[id='w2ui-message0'].w2ui-popup-message [id='No']")).ClickEx();
        }

        public void ClickPopupMessageOkButton()
        {
            Driver.FindElement(By.CssSelector("[id='w2ui-message0'].w2ui-popup-message [onclick^='w2popup.message']")).ClickEx();
        }

        #endregion Popup Message

        #endregion //Business methods
    }
}