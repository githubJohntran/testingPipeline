using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using SeleniumExtras.PageObjects;
using StreetlightVision.Extensions;
using StreetlightVision.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace StreetlightVision.Pages.UI
{
    public class ControlProgramItemsPopupPanel : PanelBase
    {
        #region Variables 
               
        private GridPanel _gridPanel;

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

        [FindsBy(How = How.CssSelector, Using = "[id='w2ui-popup'] [id$='toolbar_item_add'] > table")]
        private IWebElement addNewButton;

        [FindsBy(How = How.CssSelector, Using = "[id='w2ui-popup'] [id$='toolbar_item_delete'] > table")]
        private IWebElement deleteButton;

        [FindsBy(How = How.CssSelector, Using = "[id='w2ui-popup'] [id$='toolbar_item_sort'] > table")]
        private IWebElement sortButton;

        [FindsBy(How = How.CssSelector, Using = "[id='w2ui-popup'] tr[id*='grid_rec']")]
        private IList<IWebElement> controlProgramItemsList;

        [FindsBy(How = How.CssSelector, Using = "[id='w2ui-popup'] tr[id*='grid_rec'].w2ui-selected")]
        private IWebElement selectedControlProgramItem;

        #endregion //IWebElements

        #region Constructor

        public ControlProgramItemsPopupPanel(IWebDriver driver, PageBase page)
            : base(driver, page) 
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
        /// Click 'AddNew' button
        /// </summary>
        public void ClickAddNewButton()
        {
            addNewButton.ClickEx();
        }

        /// <summary>
        /// Click 'Delete' button
        /// </summary>
        public void ClickDeleteButton()
        {
            deleteButton.ClickEx();
        }

        /// <summary>
        /// Click 'Sort' button
        /// </summary>
        public void ClickSortButton()
        {
            sortButton.ClickEx();
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
        /// Get control program items count
        /// </summary>
        /// <returns></returns>
        public int GetItemsCount()
        {
            return controlProgramItemsList.Count;
        }

        public List<string> GetListOfShapesDiameter()
        {
            var shapes = Driver.FindElements(By.CssSelector("[id='w2ui-popup'] tr[id*='grid_rec'] [col='0'] .schedulermanager-schedule-commands-popup-period-dot path"));
            var result = shapes.Select(p => p.GetAttribute("d").ToString()).ToList();

            return result;
        }

        public List<byte[]> GetListOfTimelineIconBytes()
        {
            var iconRows = Driver.FindElements(By.CssSelector("[id='w2ui-popup'] tr[id*='grid_rec'] [col='1'] .schedulermanager-schedule-commands-popup-type-select img"));
            var result = new List<byte[]>();

            foreach (var img in iconRows)
            {
                var src = img.GetAttribute("src");
                src = src.Replace("data:image/png;base64,", string.Empty);
                result.Add(Convert.FromBase64String(src));
            }

            return result;
        }

        public List<string> GetListOfTime()
        {
            var result = new List<string>();
            var timeRows = Driver.FindElements(By.CssSelector("[id='w2ui-popup'] tr[id*='grid_rec'] [col='2']"));
            
            foreach (var item in timeRows)
            {
                var temp = new StringBuilder();
                if (item.FindElements(By.CssSelector(".schedulermanager-schedule-commands-popup-sign-select")).Any())
                {
                    var sign = item.FindElement(By.CssSelector(".schedulermanager-schedule-commands-popup-sign-select")).Text;
                    temp.Append(sign);
                    temp.Append(" ");
                }
                temp.Append(int.Parse(item.FindElement(By.CssSelector(".schedulermanager-schedule-commands-popup-time-hours-field input")).Value()));
                temp.Append(":");
                temp.Append(item.FindElement(By.CssSelector(".schedulermanager-schedule-commands-popup-time-minutes-field input")).Value());
                temp.Append(":");
                temp.Append(item.FindElement(By.CssSelector(".schedulermanager-schedule-commands-popup-time-seconds-field input")).Value());
                if (item.FindElements(By.CssSelector(".schedulermanager-schedule-commands-popup-ampm-select")).Any())
                {
                    var ampm = item.FindElement(By.CssSelector(".schedulermanager-schedule-commands-popup-ampm-select")).Text;
                    temp.Append(" ");
                    temp.Append(ampm);
                }
                result.Add(temp.ToString());
            }        

            return result;
        }

        public List<string> GetListOfLevel()
        {
            var result = new List<string>();
            var levels = Driver.FindElements(By.CssSelector("[id='w2ui-popup'] tr[id*='grid_rec'] [col='3'] .schedulermanager-schedule-commands-popup-level-field input"));

            foreach (var input in levels)
            {
                result.Add(input.Value());
            }

            return result;
        }

        /// <summary>
        /// Select a ControlProgramItem by index (this item will be highlight)
        /// </summary>
        /// <param name="index"></param>
        public void SelectItem(int index)
        {
            controlProgramItemsList[index].FindElement(By.CssSelector(".w2ui-grid-data[col='0']")).ClickEx();
            Wait.ForMilliseconds(200);
        }

        /// <summary>
        /// Double Click on ControlProgramItem by index
        /// </summary>
        /// <param name="index"></param>
        public void DoubleClickItem(int index)
        {
            var item = controlProgramItemsList[index];
            item.ClickEx();
            var col0 = item.FindElement(By.CssSelector("[col='0'] .schedulermanager-schedule-commands-popup-period-dot path"));
            col0.MoveTo();
            col0.DoubleClickByJS();
        }

        /// <summary>
        /// Select NewItem Sign DropDown
        /// </summary>
        /// <param name="value"></param>
        public void SelectNewItemSignDropDown(string value)
        {
            var newItem = controlProgramItemsList.LastOrDefault();
            var dropdown = newItem.FindElement(By.CssSelector("[col='2'] .schedulermanager-schedule-commands-popup-sign-select"));
            dropdown.Select(value, true);
        }

        /// <summary>
        /// Enter NewItem Hour Input
        /// </summary>
        /// <param name="value"></param>
        public void EnterNewItemHourInput(string value)
        {
            var newItem = controlProgramItemsList.LastOrDefault();
            var input = newItem.FindElement(By.CssSelector("[col='2'] .schedulermanager-schedule-commands-popup-time-hours-field input"));
            input.ClickEx();
            input.SendKeys(Keys.End);
            input.Enter(value);
        }

        /// <summary>
        /// Enter NewItem Minute Input
        /// </summary>
        /// <param name="value"></param>
        public void EnterNewItemMinuteInput(string value)
        {
            var newItem = controlProgramItemsList.LastOrDefault();
            var input = newItem.FindElement(By.CssSelector("[col='2'] .schedulermanager-schedule-commands-popup-time-minutes-field input"));
            input.ClickEx();
            input.SendKeys(Keys.End);
            input.Enter(value);
        }

        /// <summary>
        /// Enter NewItem Second Input
        /// </summary>
        /// <param name="value"></param>
        public void EnterNewItemSecondInput(string value)
        {
            var newItem = controlProgramItemsList.LastOrDefault();
            var input = newItem.FindElement(By.CssSelector("[col='2'] .schedulermanager-schedule-commands-popup-time-seconds-field input"));
            input.ClickEx();
            input.SendKeys(Keys.End);
            input.Enter(value);
        }

        /// <summary>
        /// Select NewItem AMPM DropDown
        /// </summary>
        /// <param name="value"></param>
        public void SelectNewItemAMPMDropDown(string value)
        {
            var newItem = controlProgramItemsList.LastOrDefault();
            var dropdown = newItem.FindElement(By.CssSelector("[col='2'].schedulermanager-schedule-commands-popup-ampm-select"));
            dropdown.Select(value, true);
        }

        /// <summary>
        /// Enter NewItem Level
        /// </summary>
        /// <param name="value"></param>
        public void EnterNewItemLevelInput(string value)
        {
            var newItem = controlProgramItemsList.LastOrDefault();
            var input = newItem.FindElement(By.CssSelector("[col='3'] .schedulermanager-schedule-commands-popup-level-field input"));
            input.ClickEx();
            input.SendKeys(Keys.End);
            input.Enter(value);
        }

        /// <summary>
        /// Select SelectedItem Sign DropDown
        /// </summary>
        /// <param name="value"></param>
        public void SelectSelectedItemSignDropDown(string value)
        {
            var dropdown = selectedControlProgramItem.FindElement(By.CssSelector("[col='2'] .schedulermanager-schedule-commands-popup-sign-select"));
            dropdown.Select(value, true);
        }

        /// <summary>
        /// Enter SelectedItem Hour Input
        /// </summary>
        /// <param name="value"></param>
        public void EnterSelectedItemHourInput(string value)
        {
            var input = selectedControlProgramItem.FindElement(By.CssSelector("[col='2'] .schedulermanager-schedule-commands-popup-time-hours-field input"));
            input.Clear();
            input.ClickEx();
            input.SendKeys(Keys.End);
            input.Enter(value);
        }

        /// <summary>
        /// Enter SelectedItem Minute Input
        /// </summary>
        /// <param name="value"></param>
        public void EnterSelectedItemMinuteInput(string value)
        {
            var input = selectedControlProgramItem.FindElement(By.CssSelector("[col='2'] .schedulermanager-schedule-commands-popup-time-minutes-field input"));
            input.Clear();
            input.ClickEx();
            input.SendKeys(Keys.End);
            input.Enter(value);
        }

        /// <summary>
        /// Enter SelectedItem Second Input
        /// </summary>
        /// <param name="value"></param>
        public void EnterSelectedItemSecondInput(string value)
        {
            var input = selectedControlProgramItem.FindElement(By.CssSelector("[col='2'] .schedulermanager-schedule-commands-popup-time-seconds-field input"));
            input.Clear();
            input.ClickEx();
            input.SendKeys(Keys.End);
            input.Enter(value);
        }

        /// <summary>
        /// Select SelectedItem AMPM DropDown
        /// </summary>
        /// <param name="value"></param>
        public void SelectSelectedItemAMPMDropDown(string value)
        {
            var dropdown = selectedControlProgramItem.FindElement(By.CssSelector("[col='2'].schedulermanager-schedule-commands-popup-ampm-select"));
            dropdown.Select(value, true);
        }

        /// <summary>
        /// Enter SelectedItem Level
        /// </summary>
        /// <param name="value"></param>
        public void EnterSelectedItemLevelInput(string value)
        {
            var input = selectedControlProgramItem.FindElement(By.CssSelector("[col='3'] .schedulermanager-schedule-commands-popup-level-field input"));
            input.ClickEx();
            input.SendKeys(Keys.End);
            input.Enter(value);
        }

        public bool IsAddButtonVisible()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id='w2ui-popup'] [id$='toolbar_item_add'] > table"));
        }

        public bool IsDeleteButtonVisible()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id='w2ui-popup'] [id$='toolbar_item_delete'] > table"));
        }        

        public bool IsSortButtonVisible()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id='w2ui-popup'] [id$='toolbar_item_sort'] > table"));
        }

        public bool IsCancelButtonVisible()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id='w2ui-popup'] [id$='btn-undo']"));
        }

        public bool IsSaveButtonVisible()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id='w2ui-popup'] [id$='btn-save']"));
        }

        public bool IsCircleShape(string diameter)
        {
            var expectedDiameter = "M 16 16m -5, 0 a 5,5 0 1,0 10 0 a 5,5 0 1,0 -10 0";
            expectedDiameter = expectedDiameter.Replace(" ", string.Empty).Replace(",", string.Empty);
            diameter = diameter.Replace(" ", string.Empty).Replace(",", string.Empty);

            return diameter.Equals(expectedDiameter);
        }

        public bool IsTriangleDownShape(string diameter)
        {
            var expectedDiameter = "M 10 11L 16 23L 22 11L 10 11L 16 23";
            expectedDiameter = expectedDiameter.Replace(" ", string.Empty).Replace(",", string.Empty);
            diameter = diameter.Replace(" ", string.Empty).Replace(",", string.Empty);

            return diameter.Equals(expectedDiameter);
        }

        public bool IsTriangleUpShape(string diameter)
        {
            var expectedDiameter = "M 22 21L 16 9L 10 21L 22 21L 16 9";
            expectedDiameter = expectedDiameter.Replace(" ", string.Empty).Replace(",", string.Empty);
            diameter = diameter.Replace(" ", string.Empty).Replace(",", string.Empty);

            return diameter.Equals(expectedDiameter);
        }

        public bool IsDiamondShape(string diameter)
        {
            var expectedDiameter = "M 16 10L 22 16L 16 22L 10 16L 16 10L 22 16";
            expectedDiameter = expectedDiameter.Replace(" ", string.Empty).Replace(",", string.Empty);
            diameter = diameter.Replace(" ", string.Empty).Replace(",", string.Empty);

            return diameter.Equals(expectedDiameter);
        }

        /// <summary>
        /// Check if a diameter is diamond or circle or triangle shape
        /// </summary>
        /// <param name="diameter"></param>
        /// <returns></returns>
        public bool IsShape(string diameter)
        {
            return IsCircleShape(diameter) || IsTriangleDownShape(diameter) || IsTriangleUpShape(diameter) || IsDiamondShape(diameter);
        }

        /// <summary>
        /// Check time of all items are displayed with Hour, Minute, Second and have value
        /// </summary>
        /// <returns></returns>
        public bool AreTimeRecordsDisplayed()
        {
            var result = new List<string>();
            var timeRows = Driver.FindElements(By.CssSelector("[id='w2ui-popup'] tr[id*='grid_rec'] [col='2']"));

            foreach (var item in timeRows)
            {
                var hourBy =  By.CssSelector(".schedulermanager-schedule-commands-popup-time-hours-field input") ;
                if (item.FindElements(hourBy).Any())
                {
                    var hourInput = item.FindElement(hourBy);
                    if (string.IsNullOrEmpty(hourInput.Value()))
                        return false;
                }
                else
                    return false;

                var minuteBy = By.CssSelector(".schedulermanager-schedule-commands-popup-time-minutes-field input");
                if (item.FindElements(minuteBy).Any())
                {
                    var minuteInput = item.FindElement(minuteBy);
                    if (string.IsNullOrEmpty(minuteInput.Value()))
                        return false;
                }
                else
                    return false;

                var secondBy = By.CssSelector(".schedulermanager-schedule-commands-popup-time-seconds-field input");
                if (item.FindElements(secondBy).Any())
                {
                    var secondInput = item.FindElement(secondBy);
                    if (string.IsNullOrEmpty(secondInput.Value()))
                        return false;
                }
                else
                    return false;
            }

            return true;
        }

        #endregion //Business methods
    }
}