using NUnit.Framework;
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
    public class CommissionPopupPanel : PanelBase
    {
        #region Variables

        #endregion //Variables

        #region IWebElements       

        [FindsBy(How = How.CssSelector, Using = "[id='w2ui-popup'] .w2ui-msg-title")]
        private IWebElement panelTitle;

        [FindsBy(How = How.CssSelector, Using = "[id='w2ui-popup'] .w2ui-msg-close")]
        private IWebElement closeButton;

        [FindsBy(How = How.CssSelector, Using = "[id='w2ui-popup'] .popup-button.icon-cancel2")]
        private IWebElement cancelButton;

        [FindsBy(How = How.CssSelector, Using = "[id='w2ui-popup'] .popup-button.icon-commission")]
        private IWebElement commissionButton;

        [FindsBy(How = How.CssSelector, Using = "[id='w2ui-popup'] .popup-button.icon-stop")]
        private IWebElement abortButton;

        [FindsBy(How = How.CssSelector, Using = "[id^='commissioning-popup'].w2ui-grid")]
        private IWebElement gridContainer;

        [FindsBy(How = How.CssSelector, Using = "[id^='commissioning-popup'].w2ui-grid [id^='grid'][id*='records'] tr[id^='grid'][id*='rec']")]
        private IList<IWebElement> gridRecordsList;

        #endregion //IWebElements

        #region Constructor

        public CommissionPopupPanel(IWebDriver driver, PageBase page)
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
        /// Click 'Cancel' button
        /// </summary>
        public void ClickCancelButton()
        {
            cancelButton.ClickEx();
        }

        /// <summary>
        /// Click 'Commission' button
        /// </summary>
        public void ClickCommissionButton()
        {
            commissionButton.ClickEx();
        }

        /// <summary>
        /// Click 'Abort' button
        /// </summary>
        public void ClickAbortButton()
        {
            abortButton.ClickEx();
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
        
        public List<string> GetListOfControllers()
        {
            var dtGridData =  gridContainer.BuildDataTableFromGrid();
            var controllerList = dtGridData.AsEnumerable().Select(r => r.Field<string>("Controller")).ToList();

            return controllerList;
        }

        public List<string> GetListOfExpandInfomation()
        {
            return JSUtility.GetElementsText("[id='w2ui-popup'] [id$='grid_records'] [id*='grid_rec'].w2ui-expanded-row .w2ui-grid-data[col='0'] > div");
        }

        public List<string> GetListOfExpandInfomationIcon()
        {
            var results = new List<string>();
            var informationIconCols = Driver.FindElements(By.CssSelector("[id='w2ui-popup'] [id$='grid_records'] [id*='grid_rec'].w2ui-expanded-row .w2ui-grid-data[col='1'] > div"));
            foreach (var col in informationIconCols)
            {
                var icon = string.Empty;               
                var cssClass = col.GetAttribute("class");
                if (cssClass.Contains("icon-ok"))
                    icon = "Ok";
                if (cssClass.Contains("icon-warning"))
                    icon = "Warning";
                if (cssClass.Contains("icon-error"))
                    icon = "Error";
                if (cssClass.Contains("icon-info"))
                    icon = "Info";
                results.Add(icon);
            }

            return results;
        }

        /// <summary>
        /// Wait until data is displayed in grid
        /// </summary>
        public void WaitForGridContentDisplayed()
        {
            Wait.ForElementsDisplayed(By.CssSelector("[id='w2ui-popup'] [id$='grid_records'] [id*='grid_rec']"));
        }

        public bool IsExpandIconNextDevice(string name)
        {
            var gridRecords = Driver.FindElements(By.CssSelector("[id='w2ui-popup'] [id$='grid_records'] [id*='grid_rec']"));
            var controllerElement = gridRecords.FirstOrDefault(p => name.Equals(p.FindElement(By.CssSelector(".w2ui-grid-data[col='0'] > div[title]")).Text.Trim()));

            if (controllerElement != null)
            {
                var expandIcon = controllerElement.FindElement(By.CssSelector(".w2ui-col-expand > div"));
                if (expandIcon != null)
                {
                    var expanded = expandIcon.FindElement(By.CssSelector(".w2ui-expand-plus"));
                    return expanded != null;
                }                   
            }

            return false;
        }

        public bool IsCollapseIconNextDevice(string name)
        {
            var gridRecords = Driver.FindElements(By.CssSelector("[id='w2ui-popup'] [id$='grid_records'] [id*='grid_rec']"));
            var controllerElement = gridRecords.FirstOrDefault(p => name.Equals(p.FindElement(By.CssSelector(".w2ui-grid-data[col='0'] > div[title]")).Text.Trim()));

            if (controllerElement != null)
            {
                var expandIcon = controllerElement.FindElement(By.CssSelector(".w2ui-col-expand > div"));
                if (expandIcon != null)
                {
                    var expanded = expandIcon.FindElement(By.CssSelector(".w2ui-expand-minus"));
                    return expanded != null;
                }
            }

            return false;
        }

        public void ClickExpandIconNextDevice(string name)
        {
            var gridRecords = Driver.FindElements(By.CssSelector("[id='w2ui-popup'] [id$='grid_records'] [id*='grid_rec']"));
            var deviceElement = gridRecords.FirstOrDefault(p => name.Equals(p.FindElement(By.CssSelector(".w2ui-grid-data[col='0'] > div[title]")).Text.Trim()));

            if (deviceElement != null)
            {
                var expandIcon = deviceElement.FindElement(By.CssSelector(".w2ui-col-expand > div"));
                if (expandIcon != null)
                {                   
                    expandIcon.ClickEx();
                    var expanded = expandIcon.FindElement(By.CssSelector(".w2ui-expand"));
                    if (expanded.GetAttribute("class").Contains("w2ui-expand-minus"))
                        Wait.ForElementsDisplayed(By.CssSelector("[id='w2ui-popup'] [id$='grid_records'] [id*='grid_rec'].w2ui-expanded-row"));
                    else
                        Wait.ForElementsInvisible(By.CssSelector("[id='w2ui-popup'] [id$='grid_records'] [id*='grid_rec'].w2ui-expanded-row"));

                    Wait.ForSeconds(2);
                }
            }
        }

        /// <summary>
        /// Build data table from grid.
        /// </summary>
        /// <returns></returns>
        public DataTable BuildDataTableFromGrid()
        {
            var gridContainer = Driver.FindElement(By.CssSelector("[id='w2ui-popup'] [id$='grid_body'].w2ui-grid-body"));
            DataTable tblResult = gridContainer.BuildDataTableFromGrid();

            return tblResult;
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
        /// Get list of Progress Icon of grid
        /// </summary>
        /// <returns></returns>
        public List<string> GetListOfProgressIcon()
        {
            var results = new List<string>();
            var progressIconCols = Driver.FindElements(By.CssSelector("[id='w2ui-popup'] [id$='grid_body'].w2ui-grid-body td.w2ui-grid-data[col='2'] > div"));
            foreach (var col in progressIconCols)
            {
                var icon = string.Empty;
                if (col.FindElements(By.CssSelector("div")).Count == 1)
                {
                    var divIcon = col.FindElement(By.CssSelector("div"));
                    var cssClass = divIcon.GetAttribute("class");
                    if (cssClass.Contains("icon-ok"))
                        icon = "Ok";
                    else if (cssClass.Contains("icon-warning"))
                        icon = "Warning";
                    else if (cssClass.Contains("icon-error"))
                        icon = "Error";
                    else if (cssClass.Contains("icon-info"))
                        icon = "Info";
                    else if (cssClass.Contains("icomoon"))
                        icon = "Plane";
                }
                results.Add(icon);
            }            

            return results;
        }

        /// <summary>
        /// Get progress column of specific row text
        /// </summary>
        /// <param name="rowText"></param>
        /// <param name="value"></param>
        public string GetProgressIcon(string rowText)
        {
            var currentRec = gridRecordsList.FirstOrDefault(p => p.Text.Contains(rowText));
            if (currentRec != null)
            {
                var div = currentRec.FindElement(By.CssSelector("td.w2ui-grid-data[col='2'] > div"));              
                if (div.FindElements(By.CssSelector("div")).Count == 1)
                {
                    var divIcon = div.FindElement(By.CssSelector("div"));
                    var cssClass = divIcon.GetAttribute("class");
                    if (cssClass.Contains("icon-ok"))
                        return "Ok";
                    if (cssClass.Contains("icon-warning"))
                        return "Warning";
                    if (cssClass.Contains("icon-error"))
                        return "Error";
                    if (cssClass.Contains("icon-info"))
                        return "Info";
                    if (cssClass.Contains("icomoon"))
                        return "Plane";
                }

                return string.Empty;
            }
            else
                Assert.Warn(string.Format("Cannot find row with '{0}'", rowText));

            return string.Empty;
        }

        public bool IsCommissionButtonDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id='w2ui-popup'] .popup-button.icon-commission"));
        }

        public bool IsCancelButtonDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id='w2ui-popup'] .popup-button.icon-cancel2"));
        }

        #endregion //Business methods

        public override void WaitForPanelLoaded()
        {
            Wait.ForElementDisplayed(gridContainer);
        }
    }
}