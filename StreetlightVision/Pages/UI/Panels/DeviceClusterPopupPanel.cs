using NUnit.Framework;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using StreetlightVision.Extensions;
using StreetlightVision.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace StreetlightVision.Pages.UI
{
    public class DeviceClusterPopupPanel : PanelBase
    {
        #region Variables
        
        #endregion //Variables

        #region IWebElements       

        [FindsBy(How = How.CssSelector, Using = ".slv-map-cluster-popup .header-text")]
        private IWebElement panelTitle;       

        [FindsBy(How = How.CssSelector, Using = ".slv-map-cluster-popup .close-button")]
        private IWebElement closeButton;

        [FindsBy(How = How.CssSelector, Using = ".slv-map-cluster-popup [id$='column-on-off'] > table")]
        private IWebElement shownHideColumnsButton;

        [FindsBy(How = How.CssSelector, Using = ".slv-map-cluster-popup [id$='toolbar_item_reposition'] > table")]
        private IWebElement repositionButton;

        [FindsBy(How = How.CssSelector, Using = "[id='w2ui-overlay'] .w2ui-col-on-off")]
        private IWebElement showHideColumnMenu;

        [FindsBy(How = How.CssSelector, Using = "[id='w2ui-overlay'] .w2ui-col-on-off table tr")]
        private IList<IWebElement> showHideColumnList;

        [FindsBy(How = How.CssSelector, Using = "[id='w2ui-overlay'] .w2ui-col-on-off label")]
        private IList<IWebElement> showHideColumnLabelList;

        [FindsBy(How = How.CssSelector, Using = "[id='w2ui-overlay'] .w2ui-col-on-off input")]
        private IList<IWebElement> showHideColumnCheckBoxList;        

        [FindsBy(How = How.CssSelector, Using = ".slv-map-cluster-popup")]
        private IWebElement popupPanel;

        [FindsBy(How = How.CssSelector, Using = ".slv-map-cluster-popup [id$='cluster-popup_body'].w2ui-grid-body")]
        private IWebElement gridContainer;

        [FindsBy(How = How.CssSelector, Using = ".slv-map-cluster-popup tr[id^='grid'][id*='rec']")]
        private IList<IWebElement> gridRecordsList;

        #endregion //IWebElements

        #region Constructor

        public DeviceClusterPopupPanel(IWebDriver driver, PageBase page)
            : base(driver, page)
        {
            PageFactory.InitElements(this, new RetryingElementLocator(driver, TimeSpan.FromMilliseconds(Settings.LocatorRetryingTimeout), TimeSpan.FromMilliseconds(Settings.LocatorPollingInterval)));

            WaitForPanelLoaded();
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
        /// Click 'ShownHideColumns' button
        /// </summary>
        public void ClickShownHideColumnsButton()
        {
            shownHideColumnsButton.ClickEx();
        }

        /// <summary>
        /// Click 'Reposition' button
        /// </summary>
        public void ClickRepositionButton()
        {
            repositionButton.ClickEx();
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

        #region Wait methods

        #endregion //Wait methods

        public bool IsCloseButtonDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector(".slv-map-cluster-popup .close-button"));
        }

        public bool IsShowHideColumnsButtonDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector(".slv-map-cluster-popup [id$='column-on-off'] > table"));
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
        /// Check if "Reposition" button is visible
        /// </summary>
        /// <returns></returns>
        public bool IsRepositionButtonVisible()
        {
            return Driver.FindElements(By.CssSelector(".slv-map-cluster-popup [id$='toolbar_item_reposition'] > table")).Count > 0;
        }

        /// <summary>
        /// Toggle on show hide columns menu
        /// </summary>
        public void DisplayShowHideColumnsMenu()
        {
            if (!IsShowHideColumnsMenuDisplayed())
            {
                shownHideColumnsButton.ClickEx();
            }
        }

        /// <summary>
        /// Toggle off show hide columns menu
        /// </summary>
        public void HideShowHideColumnsMenu()
        {
            if (IsShowHideColumnsMenuDisplayed())
            {
                shownHideColumnsButton.ClickEx();
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
                            labelForCheckbox.Click();
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
                            labelForCheckbox.Click();
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
                            labelForCheckbox.Click();
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
                            labelForCheckbox.Click();
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
        /// Get list of Icon Type of grid
        /// </summary>
        /// <returns></returns>
        public List<string> GetListOfIconType()
        {
            var results = new List<string>();
            var images = Driver.FindElements(By.CssSelector(".slv-map-cluster-popup tr[id^='grid'][id*='rec'] td.w2ui-grid-data[col='0'] img"));

            foreach (var img in images)
            {
                results.Add(img.GetAttribute("src"));
            }

            return results;
        }

        public string GetIconType(string rowText)
        {
            var currentRec = gridRecordsList.FirstOrDefault(p => p.Text.Contains(rowText));
            if (currentRec != null)
            {
                var img = currentRec.FindElement(By.CssSelector("td.w2ui-grid-data[col='0'] img"));
                return img.GetAttribute("src");
            }
            else
                Assert.Warn(string.Format("Cannot find row with '{0}'", rowText));

            return string.Empty;
        }

        /// <summary>
        /// Build Device Cluster data table from grid.
        /// </summary>
        /// <returns></returns>
        public DataTable BuildDataTable()
        {
            DataTable tblResult = gridContainer.BuildDataTableFromGrid();

            return tblResult;
        }

        /// <summary>
        /// Get data of a specific column of Device Cluster grid
        /// </summary>
        /// <returns></returns>
        public List<string> GetListOfColumnData(string columnName)
        {
            DataTable tblGrid = BuildDataTable();
            var result = new List<string>();

            if (tblGrid.Columns.Contains(columnName))
            {
                result = tblGrid.AsEnumerable().Select(r => r.Field<string>(columnName)).ToList();
            }

            return result;
        }

        /// <summary>
        /// Tick to select all rows checkbox in grid
        /// </summary>
        /// <returns></returns>
        public void TickAllRowsCheckbox(bool value)
        {
            var checkbox = Driver.FindElement(By.CssSelector(".slv-map-cluster-popup .w2ui-head .checkbox.w2ui-grid-slv-checkbox"));
            checkbox.Check(value);
        }

        public bool GetAllRowsCheckbox()
        {
            var checkbox = Driver.FindElement(By.CssSelector(".slv-map-cluster-popup .w2ui-head .checkbox.w2ui-grid-slv-checkbox"));
            return checkbox.CheckboxValue();
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
                var checkbox = currentRec.FindElement(By.CssSelector("div.checkbox.w2ui-grid-slv-checkbox"));
                checkbox.Check(value);
            }
            else
                Assert.Warn(string.Format("Cannot find row with '{0}'", rowText));
        }

        public void SelectRow(string rowText)
        {
            var currentRec = gridRecordsList.FirstOrDefault(p => p.Text.Contains(rowText));
            if (currentRec != null)
            {
                currentRec.ClickEx();
            }
            else
                Assert.Warn(string.Format("Cannot find row with '{0}'", rowText));
        }

        public void SelectRowWithShiftKey(string rowText)
        {
            var currentRec = gridRecordsList.FirstOrDefault(p => p.Text.Contains(rowText));
            if (currentRec != null)
            {
                currentRec.ClickAndHoldWithShiftKey();
            }
            else
                Assert.Warn(string.Format("Cannot find row with '{0}'", rowText));
        }

        public void SelectRowWithCtrlKey(string rowText)
        {
            var currentRec = gridRecordsList.FirstOrDefault(p => p.Text.Contains(rowText));
            if (currentRec != null)
            {
                currentRec.ClickAndHoldWithCtrlKey();
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
            var recordChecked = false;
            WebDriverContext.Wait.Until(driver =>
            {
                try
                {
                    var currentRec = gridRecordsList.FirstOrDefault(p => p.Text.Contains(rowText));
                    var checkbox = currentRec.FindElement(By.CssSelector("div.checkbox.w2ui-grid-slv-checkbox"));

                    recordChecked = checkbox.CheckboxValue();

                    return true;
                }
                catch (StaleElementReferenceException)
                {
                    return false;
                }
            });

            return recordChecked;
        }

        public bool AreCheckBoxGridChecked()
        {
            var allChecked = true;

            var checkboxes = Driver.FindElements(By.CssSelector(".slv-map-cluster-popup tr[id^='grid'][id*='rec'] .checkbox.w2ui-grid-slv-checkbox"));
            foreach (var checkbox in checkboxes)
            {
                if (checkbox.CheckboxValue() == false) allChecked = false;
            }

            return allChecked;
        }

        /// <summary>
        /// Check if a specific column has data
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool IsGridHasTextPresent(string columnName, string value)
        {
            var data = GetListOfColumnData(columnName);
            return data.Exists(p => p.TrimEx().Equals(value));
        }

        #endregion //Business methods

        public override void WaitForPanelLoaded()
        {
            Wait.ForElementDisplayed(popupPanel);
        }
    }
}