using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.PageObjects;
using StreetlightVision.Extensions;
using StreetlightVision.Models;
using StreetlightVision.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace StreetlightVision.Pages.UI
{
    public class RealTimeBatchPanel : PanelBase
    {
        #region Variables

        #endregion //Variables

        #region IWebElements

        [FindsBy(How = How.CssSelector, Using = "[id='slv-realtimebatch-gridpanel']")]
        private IWebElement panel;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-realtimebatch-gridpanel'] .slv-realtimebatch-header")]
        private IWebElement panelTitle;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-realtimebatch-gridpanel'] .realtimebatch-mimic-w2ui-button.show")]
        private IWebElement showButton;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-realtimebatch-gridpanel'] .realtimebatch-mimic-w2ui-button.hide")]
        private IWebElement hideButton;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-realtimebatch-gridpanel'] .toolbar .checkbox")]
        private IWebElement autoZoomCheckbox;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-realtimebatch-gridpanel'] [id$='slv-realtimebatch-attribute']")]
        private IWebElement attributeDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-realtimebatch-gridpanel'] [id$='slv-realtimebatch-operator']")]
        private IWebElement operatorDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-realtimebatch-gridpanel'] [id$='slv-realtimebatch-search-field-container']")]
        private IWebElement valueFieldDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-realtimebatch-gridpanel'] [id='slv-realtimebatch-search-field-container'] input")]
        private IWebElement searchNameInput;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-realtimebatch-gridpanel'] div.w2ui-tab.active")]
        private IWebElement activeTab;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-realtimebatch-gridpanel'] [id='slv-realtimebatch-search-field-container'] span")]
        private IWebElement dropDownMenu;

        [FindsBy(How = How.CssSelector, Using = "[id='select2-drop'] input")]
        private IWebElement dropDownInput;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-realtimebatch-gridpanel'] [id='slv-realtimebatch-search-field-container'] [class='select2-chosen']")]
        private IWebElement dropDownInputField;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-realtimebatch-gridpanel'] [id='slv-realtimebatch-search-button']")]
        private IWebElement searchButton;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-realtimebatch-gridpanel'] div.w2ui-tab")]
        private IList<IWebElement> tabList;


        #region Streetlight

        [FindsBy(How = How.CssSelector, Using = "[id='grid_gridpanel-streetlight_body']")]
        private IWebElement streetlightGridBody;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-realtimebatch-gridpanel-streetlight'] [id$='item_w2ui-column-on-off'] > table")]
        private IWebElement streetlightShownHideColumnsButton;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-realtimebatch-gridpanel-streetlight'] input[id$='search_all']")]
        private IWebElement streetlightSearchInput;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-realtimebatch-gridpanel-streetlight'] [id$='searchClear'].w2ui-search-clear")]
        private IWebElement streetlightClearSearchButton;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-realtimebatch-gridpanel-streetlight'] [id$='toolbar_item_w2ui-search-advanced'] > table")]
        private IWebElement streetlightSearchButton;

        [FindsBy(How = How.CssSelector, Using = "[id='tb_gridpanel-streetlight_toolbar_item_export'] > table")]
        private IWebElement streetlightExportButton;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-realtimebatch-gridpanel-streetlight-refreshGroup']")]
        private IWebElement streetlightRefreshButton;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-realtimebatch-gridpanel-streetlight-dimming-off']")]
        private IWebElement streetlightDimmingOffButton;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-realtimebatch-gridpanel-streetlight-dimming-on']")]
        private IWebElement streetlightDimmingOnButton;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-realtimebatch-gridpanel-streetlight-back_to_auto']")]
        private IWebElement streetlightBackToAutoButton;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-realtimebatch-gridpanel-streetlight-dimming-input']")]
        private IWebElement streetlightDimmingRangeInput;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-realtimebatch-gridpanel-streetlight-dimming-value']")]
        private IWebElement streetlightDimmingRangeValueLabel;

        #endregion //Streetlight

        #region Controller

        [FindsBy(How = How.CssSelector, Using = "[id='grid_gridpanel-controllerdevice_body']")]
        private IWebElement controllerGridBody;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-realtimebatch-gridpanel-controllerdevice'] [id$='item_w2ui-column-on-off'] > table")]
        private IWebElement controllerShownHideColumnsButton;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-realtimebatch-gridpanel-controllerdevice'] input[id$='search_all']")]
        private IWebElement controllerSearchInput;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-realtimebatch-gridpanel-controllerdevice'] [id$='searchClear'].w2ui-search-clear")]
        private IWebElement controllerClearSearchButton;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-realtimebatch-gridpanel-controllerdevice'] [id$='toolbar_item_w2ui-search-advanced'] > table")]
        private IWebElement controllerSearchButton;

        [FindsBy(How = How.CssSelector, Using = "[id='tb_gridpanel-controllerdevice_toolbar_item_export'] > table")]
        private IWebElement controllerExportButton;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-realtimebatch-gridpanel-controller-refresh']")]
        private IWebElement controllerRefreshButton;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-realtimebatch-gridpanel-controller-send_data_logs']")]
        private IWebElement controllerSendDataLogsButton;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-realtimebatch-gridpanel-controller-sync_system_time']")]
        private IWebElement controllerSyncSystemTimeButton;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-realtimebatch-gridpanel-controller-output_index']")]
        private IWebElement controllerOutputDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-realtimebatch-gridpanel-controller-off']")]
        private IWebElement controllerOffButton;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-realtimebatch-gridpanel-controller-on']")]
        private IWebElement controllerOnButton;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-realtimebatch-gridpanel-controller-back_to_auto']")]
        private IWebElement controllerBackToAutoButton;

        #endregion //Controller

        #region Switch

        [FindsBy(How = How.CssSelector, Using = "[id='grid_gridpanel-switch_body']")]
        private IWebElement switchGridBody;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-realtimebatch-gridpanel-switch'] [id$='item_w2ui-column-on-off'] > table")]
        private IWebElement switchShownHideColumnsButton;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-realtimebatch-gridpanel-switch'] input[id$='search_all']")]
        private IWebElement switchSearchInput;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-realtimebatch-gridpanel-switch'] [id$='searchClear'].w2ui-search-clear")]
        private IWebElement switchClearSearchButton;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-realtimebatch-gridpanel-switch'] [id$='toolbar_item_w2ui-search-advanced'] > table")]
        private IWebElement switchSearchButton;

        [FindsBy(How = How.CssSelector, Using = "[id='tb_gridpanel-switch_toolbar_item_export'] > table")]
        private IWebElement switchExportButton;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-realtimebatch-gridpanel-switch-refreshGroup']")]
        private IWebElement switchRefreshButton;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-realtimebatch-gridpanel-switch-off']")]
        private IWebElement switchOffButton;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-realtimebatch-gridpanel-switch-on']")]
        private IWebElement switchOnButton;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-realtimebatch-gridpanel-switch-back_to_auto']")]
        private IWebElement switchBackToAutoButton;

        #endregion //Switch

        #region Electrical Counter

        [FindsBy(How = How.CssSelector, Using = "[id='grid_gridpanel-electricalCounter_body']")]
        private IWebElement electricalCounterGridBody;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-realtimebatch-gridpanel- electricalCounter'] [id$='item_w2ui-column-on-off'] > table")]
        private IWebElement electricalCounterShownHideColumnsButton;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-realtimebatch-gridpanel-electricalCounter'] input[id$='search_all']")]
        private IWebElement electricalCounterSearchInput;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-realtimebatch-gridpanel-electricalCounter'] [id$='searchClear'].w2ui-search-clear")]
        private IWebElement electricalCounterClearSearchButton;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-realtimebatch-gridpanel-electricalCounter'] [id$='toolbar_item_w2ui-search-advanced'] > table")]
        private IWebElement electricalCounterSearchButton;

        [FindsBy(How = How.CssSelector, Using = "[id='tb_gridpanel-streetlight_toolbar_item_export'] > table")]
        private IWebElement electricalCounterExportButton;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-realtimebatch-gridpanel-electricalCounter-refreshGroup']")]
        private IWebElement electricalCounterRefreshButton;

        #endregion //Electrical Counter

        #endregion //IWebElements

        #region Constructor

        public RealTimeBatchPanel(IWebDriver driver, PageBase page)
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
        /// Click 'Show' button
        /// </summary>
        public void ClickShowButton()
        {
            if (IsMaximizeIconDisplayed())
                showButton.ClickEx();
        }

        /// <summary>
        /// Click 'Hide' button
        /// </summary>
        public void ClickHideButton()
        {
            if (IsMinimizeIconDisplayed())
                hideButton.ClickEx();
        }

        /// <summary>
        /// Tick 'AutoZoom' checkbox
        /// </summary>
        /// <param name="value"></param>
        public void TickAutoZoomCheckbox(bool value)
        {
            autoZoomCheckbox.Check(value);
        }

        /// <summary>
        /// Select an item of 'Attribute' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectAttributeDropDown(string value)
        {
            attributeDropDown.Select(value);
        }

        /// <summary>
        /// Select an item of 'Operator' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectOperatorDropDown(string value)
        {
            operatorDropDown.Select(value);
        }

        /// <summary>
        /// Select an item of 'ValueField' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectValueFieldDropDown(string value)
        {
            valueFieldDropDown.Select(value);
        }

        /// <summary>
        /// Select an item of 'Drop down menu' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectValueDropDownMenu(string value)
        {
            ClickInsideDropDownMenu();
            dropDownInput.Select(value);

        }

        /// <summary>
        /// Enter a value for 'SearchName' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterSearchNameInput(string value)
        {
            searchNameInput.Enter(value);
        }

        /// <summary>
        /// Click 'Search' button
        /// </summary>
        public void ClickSearchButton()
        {
            searchButton.ClickEx();
        }


        #region Streetlight

        /// <summary>
        /// Click 'StreetlightShownHideColumns' button
        /// </summary>
        public void ClickStreetlightShownHideColumnsButton()
        {
            streetlightShownHideColumnsButton.ClickEx();
        }

        /// <summary>
        /// Enter a value for 'StreetlightSearch' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterStreetlightSearchInput(string value)
        {
            streetlightSearchInput.Enter(value);
        }

        /// <summary>
        /// Click 'StreetlightClearSearch' button
        /// </summary>
        public void ClickStreetlightClearSearchButton()
        {
            streetlightClearSearchButton.ClickEx();
        }

        /// <summary>
        /// Click 'StreetlightSearch' button
        /// </summary>
        public void ClickStreetlightSearchButton()
        {
            streetlightSearchButton.ClickEx();
        }

        /// <summary>
        /// Click 'StreetlightExport' button
        /// </summary>
        public void ClickStreetlightExportButton()
        {
            streetlightExportButton.ClickEx();
        }

        /// <summary>
        /// Click 'StreetlightRefreshGroup' button
        /// </summary>
        public void ClickStreetlightRefreshButton()
        {
            streetlightRefreshButton.ClickEx();
        }

        /// <summary>
        /// Click 'StreetlightDimmingOff' button
        /// </summary>
        public void ClickStreetlightDimmingOffButton()
        {
            streetlightDimmingOffButton.ClickEx();
        }

        /// <summary>
        /// Click 'StreetlightDimmingOn' button
        /// </summary>
        public void ClickStreetlightDimmingOnButton()
        {
            streetlightDimmingOnButton.ClickEx();
        }

        /// <summary>
        /// Click 'StreetlightBackToAuto' button
        /// </summary>
        public void ClickStreetlightBackToAutoButton()
        {
            streetlightBackToAutoButton.ClickEx();
        }

        /// <summary>
        /// Enter a value for 'StreetlightDimmingRange' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterStreetlightDimmingRangeInput(string value)
        {
            streetlightDimmingRangeInput.Enter(value);
        }

        /// <summary>
        /// Click on the UpArrow given times
        /// </summary>
        /// <param name="times"></param>
        public void ClickLampWattageUpArrow(int times)
        {
            var upArrowCss = "[id='slv-realtimebatch-search-field-container'] [class='w2ui-field-helper'] [class='w2ui-field-up']";
            var uparrow = Driver.FindElement(By.CssSelector(upArrowCss));
            for (int i = 1; i <= times; i++)
            {
                uparrow.ClickEx();
            }
        }

        /// <summary>
        /// Click on the DownArrow given times
        /// </summary>
        /// <param name="times"></param>
        public void ClickLampWattageDownArrow(int times)
        {
            var downArrowCss = "[id='slv-realtimebatch-search-field-container'] [class='w2ui-field-helper'] [class='w2ui-field-down']";
            var downArrow = Driver.FindElement(By.CssSelector(downArrowCss));
            for (int i = 1; i <= times; i++)
            {
                downArrow.ClickEx();
            }
        }

        #endregion //Streetlight

        #region Controller

        /// <summary>
        /// Click 'ControllerShownHideColumns' button
        /// </summary>
        public void ClickControllerShownHideColumnsButton()
        {
            controllerShownHideColumnsButton.ClickEx();
        }

        /// <summary>
        /// Enter a value for 'ControllerSearch' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterControllerSearchInput(string value)
        {
            controllerSearchInput.Enter(value);
        }

        /// <summary>
        /// Click 'ControllerClearSearch' button
        /// </summary>
        public void ClickControllerClearSearchButton()
        {
            controllerClearSearchButton.ClickEx();
        }

        /// <summary>
        /// Click 'ControllerSearch' button
        /// </summary>
        public void ClickControllerSearchButton()
        {
            controllerSearchButton.ClickEx();
        }

        /// <summary>
        /// Click 'ControllerExport' button
        /// </summary>
        public void ClickControllerExportButton()
        {
            controllerExportButton.ClickEx();
        }

        /// <summary>
        /// Click 'ControllerRefresh' button
        /// </summary>
        public void ClickControllerRefreshButton()
        {
            controllerRefreshButton.ClickEx();
        }

        /// <summary>
        /// Click 'ControllerSendDataLogs' button
        /// </summary>
        public void ClickControllerSendDataLogsButton()
        {
            controllerSendDataLogsButton.ClickEx();
        }

        /// <summary>
        /// Click 'ControllerSyncSystemTime' button
        /// </summary>
        public void ClickControllerSyncSystemTimeButton()
        {
            controllerSyncSystemTimeButton.ClickEx();
        }

        /// <summary>
        /// Select an item of 'ControllerOutput' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectControllerOutputDropDown(string value)
        {
            controllerOutputDropDown.Select(value, true);
        }

        /// <summary>
        /// Click 'ControllerOff' button
        /// </summary>
        public void ClickControllerOffButton()
        {
            controllerOffButton.ClickEx();
        }

        /// <summary>
        /// Click 'ControllerOn' button
        /// </summary>
        public void ClickControllerOnButton()
        {
            controllerOnButton.ClickEx();
        }

        /// <summary>
        /// Click 'ControllerBackToAuto' button
        /// </summary>
        public void ClickControllerBackToAutoButton()
        {
            controllerBackToAutoButton.ClickEx();
        }

        #endregion //Controller

        #region Switch

        /// <summary>
        /// Click 'SwitchShownHideColumns' button
        /// </summary>
        public void ClickSwitchShownHideColumnsButton()
        {
            switchShownHideColumnsButton.ClickEx();
        }

        /// <summary>
        /// Enter a value for 'SwitchSearch' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterSwitchSearchInput(string value)
        {
            switchSearchInput.Enter(value);
        }

        /// <summary>
        /// Click 'SwitchClearSearch' button
        /// </summary>
        public void ClickSwitchClearSearchButton()
        {
            switchClearSearchButton.ClickEx();
        }

        /// <summary>
        /// Click 'SwitchSearch' button
        /// </summary>
        public void ClickSwitchSearchButton()
        {
            switchSearchButton.ClickEx();
        }

        /// <summary>
        /// Click 'SwitchExport' button
        /// </summary>
        public void ClickSwitchExportButton()
        {
            switchExportButton.ClickEx();
        }

        /// <summary>
        /// Click 'SwitchRefresh' button
        /// </summary>
        public void ClickSwitchRefreshButton()
        {
            switchRefreshButton.ClickEx();
        }

        /// <summary>
        /// Click 'SwitchOff' button
        /// </summary>
        public void ClickSwitchOffButton()
        {
            switchOffButton.ClickEx();
        }

        /// <summary>
        /// Click 'SwitchOn' button
        /// </summary>
        public void ClickSwitchOnButton()
        {
            switchOnButton.ClickEx();
        }

        /// <summary>
        /// Click 'SwitchBackToAuto' button
        /// </summary>
        public void ClickSwitchBackToAutoButton()
        {
            switchBackToAutoButton.ClickEx();
        }

        #endregion //Switch

        #region Electrical Counter

        /// <summary>
        /// Click 'ElectricalCounterShownHideColumns' button
        /// </summary>
        public void ClickElectricalCounterShownHideColumnsButton()
        {
            electricalCounterShownHideColumnsButton.ClickEx();
        }

        /// <summary>
        /// Enter a value for 'ElectricalCounterSearch' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterElectricalCounterSearchInput(string value)
        {
            electricalCounterSearchInput.Enter(value);
        }

        /// <summary>
        /// Click 'ElectricalCounterClearSearch' button
        /// </summary>
        public void ClickElectricalCounterClearSearchButton()
        {
            electricalCounterClearSearchButton.ClickEx();
        }

        /// <summary>
        /// Click 'ElectricalCounterSearch' button
        /// </summary>
        public void ClickElectricalCounterSearchButton()
        {
            electricalCounterSearchButton.ClickEx();
        }

        /// <summary>
        /// Click 'ElectricalCounterExport' button
        /// </summary>
        public void ClickElectricalCounterExportButton()
        {
            electricalCounterExportButton.ClickEx();
        }

        /// <summary>
        /// Click 'ElectricalCounterRefresh' button
        /// </summary>
        public void ClickElectricalCounterRefreshButton()
        {
            electricalCounterRefreshButton.ClickEx();
        }

        #endregion //Electrical Counter

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
        /// Get 'AutoZoom' checkbox value
        /// </summary>
        /// <returns></returns>
        public bool GetAutoZoomValue()
        {
            return autoZoomCheckbox.CheckboxValue();
        }

        /// <summary>
        /// Get 'Attribute' dropdown value
        /// </summary>
        /// <returns></returns>
        public string GetAttributeValue()
        {
            return attributeDropDown.Text;
        }

        /// <summary>
        /// Get 'Operator' dropdown value
        /// </summary>
        /// <returns></returns>
        public string GetOperatorValue()
        {
            return operatorDropDown.Text;
        }

        /// <summary>
        /// Get 'ValueField' dropdown value
        /// </summary>
        /// <returns></returns>
        public string GetValueFieldValue()
        {
            return valueFieldDropDown.Text;
        }

        /// <summary>
        /// Get 'SearchName' input value
        /// </summary>
        /// <returns></returns>
        public string GetSearchNameValue()
        {
            return searchNameInput.Value();
        }

        /// <summary>
        /// Get the default value from 'SearchName' input
        /// </summary>
        /// <param name="value"></param>
        public string GetSearchNameInputDefaultValue()
        {
            return searchNameInput.GetAttribute("placeholder");
        }

        /// <summary>
        /// Get the default value from 'DropDownMenu'
        /// </summary>
        /// <returns></returns>
        public string GetDropDownMenuDefaultValue()
        {
            return dropDownMenu.Text;
        }

        #region Streetlight

        /// <summary>
        /// Get 'StreetlightSearch' input value
        /// </summary>
        /// <returns></returns>
        public string GetStreetlightSearchValue()
        {
            return streetlightSearchInput.Value();
        }

        /// <summary>
        /// Get 'StreetlightDimmingRange' input value
        /// </summary>
        /// <returns></returns>
        public string GetStreetlightDimmingRangeValue()
        {
            return streetlightDimmingRangeInput.Value();
        }

        /// <summary>
        /// Get 'StreetlightDimmingRangeValue' label text
        /// </summary>
        /// <returns></returns>
        public string GetStreetlightDimmingRangeValueText()
        {
            return streetlightDimmingRangeValueLabel.Text;
        }

        /// <summary>
        /// Get the number of operators in operatorDropDown
        /// </summary>
        /// <returns></returns>
        public int GetNumberOfOperators()
        {
            return operatorDropDown.GetAllItems().Count;
        }

        /// <summary>
        /// Get the listed devices from drop down menu
        /// </summary>
        /// <returns></returns>
        public List<string> GetListOfAllListedItemsFromDropDownMenu()
        {
            return dropDownMenu.GetAllItems();
        }

        /// <summary>
        /// Check the 'No matches found' is displayed
        /// </summary>
        /// <returns></returns>
        public bool NoMatchesFoundIsDisplayedInOperatorDropDown()
        {
            var allOperators = operatorDropDown.GetAllItems();
            var firstOfAllOperators = allOperators.FirstOrDefault();

            return firstOfAllOperators.Equals("No matches found");
        }


        public bool NoMatchesFoundIsDisplayedInDropDownMenu()
        {
            var allOperators = dropDownMenu.GetAllItems();
            var firstOfAllOperators = allOperators.FirstOrDefault();

            return firstOfAllOperators.Equals("No matches found");
        }

        /// <summary>
        /// Click inside the dropdown menu
        /// </summary>
        /// <returns></returns>
        public void ClickInsideDropDownMenu()
        {
            dropDownMenu.Click();
        }

        /// <summary>
        /// Enter value inside the dropdown input and press enter with Clear
        /// </summary>
        /// <returns></returns>
        public void EnterSearchValueDropDownInputWithClear(string value)
        {
            dropDownInput.Enter(value, true, false, true);
        }

        /// <summary>
        /// Enter value inside the dropdown input and press enter without Clear
        /// </summary>
        /// <returns></returns>
        public void EnterSearchValueDropDownInputWithoutClear(string value)
        {
            dropDownInput.Enter(value, false, false, true);
        }

        /// <summary>
        /// Get the innerText value from DropDownInputField
        /// </summary>
        /// <returns></returns>
        public string GetDropDownInputFieldText()
        {
            return dropDownInputField.GetAttribute("innerText");
        }

        #endregion //Streetlight

        #region Controller

        /// <summary>
        /// Get 'ControllerSearch' input value
        /// </summary>
        /// <returns></returns>
        public string GetControllerSearchValue()
        {
            return controllerSearchInput.Value();
        }

        /// <summary>
        /// Get 'ControllerOutput' dropdown value
        /// </summary>
        /// <returns></returns>
        public string GetControllerOutputValue()
        {
            return controllerOutputDropDown.Text;
        }

        #endregion //Controller

        #region Switch

        /// <summary>
        /// Get 'SwitchSearch' input value
        /// </summary>
        /// <returns></returns>
        public string GetSwitchSearchValue()
        {
            return switchSearchInput.Value();
        }

        #endregion //Switch

        #region Electrical Counter

        /// <summary>
        /// Get 'ElectricalCounterSearch' input value
        /// </summary>
        /// <returns></returns>
        public string GetElectricalCounterSearchValue()
        {
            return electricalCounterSearchInput.Value();
        }

        #endregion //Electrical Counter

        #endregion //Get methods

        #endregion //Basic methods

        #region Business methods     

        public void MinimizePanelByDragDrop()
        {
            var headerPanel = Driver.FindElement(By.CssSelector("[id='slv-realtimebatch-gridpanel'] .slv-top-panel"));
            headerPanel.DragAndDropToOffsetByJS(0, panel.Size.Height);
        }

        public string GetInstructionsMessage()
        {
            var byInstruction = By.CssSelector(".slv-realtimebatch-instructions");

            if (ElementUtility.IsDisplayed(byInstruction))
            {
                return Driver.FindElement(byInstruction).Text.Trim();
            }

            return string.Empty;

        }

        public bool IsMaximizeIconDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id='slv-realtimebatch-gridpanel'] .realtimebatch-mimic-w2ui-button.show"));
        }

        public bool IsMinimizeIconDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id='slv-realtimebatch-gridpanel'] .realtimebatch-mimic-w2ui-button.hide"));
        }

        public bool IsWidgetMaximized()
        {
            Wait.ForSeconds(2);// for effect maximized
            return IsMinimizeIconDisplayed();
        }

        public bool IsWidgetMinimized()
        {
            Wait.ForSeconds(2);// for effect minimized
            return IsMaximizeIconDisplayed();
        }

        public bool IsAttributeDropDownDisplayed()
        {
            return attributeDropDown.Displayed;
        }

        public bool IsOperaterDropDownDisplayed()
        {
            return operatorDropDown.Displayed;
        }

        public bool IsSearchNameInputDisplayed()
        {
            return searchNameInput.Displayed;
        }

        public bool IsSearchButtonDisplayed()
        {
            return searchButton.Displayed;
        }

        /// <summary>
        /// Select a tab by its caption
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public void SelectTab(string name)
        {
            Wait.ForElementsDisplayed(By.CssSelector("[id='slv-realtimebatch-gridpanel'] div.w2ui-tab"));
            Wait.ForElementText(By.CssSelector("[id='slv-realtimebatch-gridpanel'] div.w2ui-tab.active"));
            var tab = tabList.FirstOrDefault(p => p.Text.Contains(name));
            if (tab != null)
            {
                tab.ClickEx();
                WebDriverContext.Wait.Until(driver => activeTab.Text == name);
            }
        }

        public List<string> GetListOfTabsName()
        {
            return JSUtility.GetElementsText("[id='slv-realtimebatch-gridpanel'] .w2ui-tab");
        }

        public string GetActivedTabName()
        {
            return activeTab.Text;
        }

        public string GetTabName(DeviceType deviceType)
        {
            if (deviceType == DeviceType.Controller)
                return "Controller Device";
            else if (deviceType == DeviceType.Streetlight)
                return "StreetLight";
            else if (deviceType == DeviceType.Switch)
                return "Switch Device";
            else if (deviceType == DeviceType.ElectricalCounter)
                return "Electrical Counter";

            return string.Empty;
        }

        public List<string> GetListOfColumnDataName(DeviceType deviceType)
        {
            var result = new List<string>();

            if (deviceType == DeviceType.Controller)
                result = GetListOfControllerColumnData("Name");
            else if (deviceType == DeviceType.Streetlight)
                result = GetListOfStreetlightColumnData("Name");
            else if (deviceType == DeviceType.Switch)
                result = GetListOfSwitchColumnData("Name");
            else if (deviceType == DeviceType.ElectricalCounter)
                result = GetListOfElectricalCounterColumnData("Name");

            return result;
        }

        public void WaitForDataGridLoaded()
        {
            WaitForPreviousActionComplete();
        }

        public string SelectRandomValueFieldDropDown()
        {
            var currentValue = GetValueFieldValue();
            var listItems = valueFieldDropDown.GetAllItems();
            listItems.Remove(currentValue);
            listItems.RemoveAll(p => string.IsNullOrEmpty(p));
            var value = listItems.PickRandom();
            valueFieldDropDown.Select(value);

            return value;
        }

        #region Streetlight

        /// <summary>
        /// Build Streetlight data table from grid.
        /// </summary>
        /// <returns></returns>
        public DataTable BuildStreetlightDataTable()
        {
            DataTable tblResult = streetlightGridBody.BuildDataTableFromGrid();

            return tblResult;
        }

        /// <summary>
        /// Get data of a specific column of Streetlight grid
        /// </summary>
        /// <returns></returns>
        public List<string> GetListOfStreetlightColumnData(string columnName)
        {
            DataTable tblGrid = BuildStreetlightDataTable();
            var result = new List<string>();

            if (tblGrid.Columns.Contains(columnName))
            {
                result = tblGrid.AsEnumerable().Select(r => r.Field<string>(columnName)).ToList();
            }

            return result;
        }

        public void ClickStreetlightGridRecord(string text)
        {
            Wait.ForSeconds(1);
            var gridRecordsList = Driver.FindElements(By.CssSelector("[id='slv-realtimebatch-gridpanel-streetlight'] tr[id^='grid'][id*='rec']"));
            WebDriverContext.Wait.Until(driver =>
            {
                try
                {
                    var currentRec = gridRecordsList.FirstOrDefault(p => p.Text.Contains(text));
                    currentRec.ClickEx();

                    return true;
                }
                catch (StaleElementReferenceException)
                {
                    return false;
                }
            });
        }

        public List<string> GetListOfStreetlightColumnHeaders()
        {
            var results = new List<string>();
            var columnHeaders = streetlightGridBody.GetGridHeaders() as IEnumerable;

            foreach (var header in columnHeaders)
            {
                var headerText = header as string;
                headerText = headerText.TrimEx();
                if (!string.IsNullOrEmpty(headerText))
                    results.Add(headerText);
            }

            return results.Distinct().ToList();
        }

        public bool IsStreetlightRefreshButtonDisplayed()
        {
            return streetlightRefreshButton.Displayed;
        }

        public bool IsStreetlightDimmingOnButtonDisplayed()
        {
            return streetlightDimmingOnButton.Displayed;
        }

        public bool IsStreetlightDimmingOffButtonDisplayed()
        {
            return streetlightDimmingOffButton.Displayed;
        }

        public bool IsStreetlightBackToAutoButtonDisplayed()
        {
            return streetlightBackToAutoButton.Displayed;
        }

        public bool IsStreetlightDimmingBarDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id='slv-realtimebatch-gridpanel-streetlight'] .realtimebatch-devicesgrid-toolbar-inner"));
        }

        #endregion //Streetlight

        #region Controller

        /// <summary>
        /// Build Controller data table from grid.
        /// </summary>
        /// <returns></returns>
        public DataTable BuildControllerDataTable()
        {
            DataTable tblResult = controllerGridBody.BuildDataTableFromGrid();

            return tblResult;
        }

        /// <summary>
        /// Get data of a specific column of Controller grid
        /// </summary>
        /// <returns></returns>
        public List<string> GetListOfControllerColumnData(string columnName)
        {
            DataTable tblGrid = BuildControllerDataTable();
            var result = new List<string>();

            if (tblGrid.Columns.Contains(columnName))
            {
                result = tblGrid.AsEnumerable().Select(r => r.Field<string>(columnName)).ToList();
            }

            return result;
        }

        public void ClickControllerGridRecord(string text)
        {
            Wait.ForSeconds(1);
            var gridRecordsList = Driver.FindElements(By.CssSelector("[id='slv-realtimebatch-gridpanel-controllerdevice'] tr[id^='grid'][id*='rec']"));
            WebDriverContext.Wait.Until(driver =>
            {
                try
                {
                    var currentRec = gridRecordsList.FirstOrDefault(p => p.Text.Contains(text));
                    currentRec.ClickEx();

                    return true;
                }
                catch (StaleElementReferenceException)
                {
                    return false;
                }
            });
        }

        public List<string> GetListOfControllerColumnHeaders()
        {
            var results = new List<string>();
            var columnHeaders = controllerGridBody.GetGridHeaders() as IEnumerable;

            foreach (var header in columnHeaders)
            {
                var headerText = header as string;
                headerText = headerText.TrimEx();
                if (!string.IsNullOrEmpty(headerText))
                    results.Add(headerText);
            }

            return results.Distinct().ToList();
        }

        public List<string> GetListOfControllerOutputItems()
        {
            return JSUtility.GetElementsText("[id='slv-realtimebatch-gridpanel-controller-output_index'] option");
        }

        public bool IsControllerRefreshButtonDisplayed()
        {
            return controllerRefreshButton.Displayed;
        }

        public bool IsControllerSendDataLogsButtonDisplayed()
        {
            return controllerSendDataLogsButton.Displayed;
        }

        public bool IsControllerSyncSystemTimeButtonDisplayed()
        {
            return controllerSyncSystemTimeButton.Displayed;
        }

        public bool IsControllerOnButtonDisplayed()
        {
            return controllerOnButton.Displayed;
        }

        public bool IsControllerOffButtonDisplayed()
        {
            return controllerOffButton.Displayed;
        }

        public bool IsControllerBackToAutoButtonDisplayed()
        {
            return controllerBackToAutoButton.Displayed;
        }

        public bool IsControllerOutputDropDownDisplayed()
        {
            return controllerOutputDropDown.Displayed;
        }

        #endregion //Controller

        #region Switch

        /// <summary>
        /// Build Switch data table from grid.
        /// </summary>
        /// <returns></returns>
        public DataTable BuildSwitchDataTable()
        {
            DataTable tblResult = switchGridBody.BuildDataTableFromGrid();

            return tblResult;
        }

        /// <summary>
        /// Get data of a specific column of Switch grid
        /// </summary>
        /// <returns></returns>
        public List<string> GetListOfSwitchColumnData(string columnName)
        {
            DataTable tblGrid = BuildSwitchDataTable();
            var result = new List<string>();

            if (tblGrid.Columns.Contains(columnName))
            {
                result = tblGrid.AsEnumerable().Select(r => r.Field<string>(columnName)).ToList();
            }

            return result;
        }

        public void ClickSwitchGridRecord(string text)
        {
            Wait.ForSeconds(1);
            var gridRecordsList = Driver.FindElements(By.CssSelector("[id='slv-realtimebatch-gridpanel-switch'] tr[id^='grid'][id*='rec']"));
            WebDriverContext.Wait.Until(driver =>
            {
                try
                {
                    var currentRec = gridRecordsList.FirstOrDefault(p => p.Text.Contains(text));
                    currentRec.ClickEx();

                    return true;
                }
                catch (StaleElementReferenceException)
                {
                    return false;
                }
            });
        }

        public List<string> GetListOfSwitchColumnHeaders()
        {
            var results = new List<string>();
            var columnHeaders = switchGridBody.GetGridHeaders() as IEnumerable;

            foreach (var header in columnHeaders)
            {
                var headerText = header as string;
                headerText = headerText.TrimEx();
                if (!string.IsNullOrEmpty(headerText))
                    results.Add(headerText);
            }

            return results.Distinct().ToList();
        }

        public bool IsSwitchRefreshButtonDisplayed()
        {
            return switchRefreshButton.Displayed;
        }

        public bool IsSwitchOnButtonDisplayed()
        {
            return switchOnButton.Displayed;
        }

        public bool IsSwitchOffButtonDisplayed()
        {
            return switchOffButton.Displayed;
        }

        public bool IsSwitchBackToAutoButtonDisplayed()
        {
            return switchBackToAutoButton.Displayed;
        }

        #endregion //Switch

        #region Electrical Counter

        /// <summary>
        /// Build Electrical Counter data table from grid.
        /// </summary>
        /// <returns></returns>
        public DataTable BuildElectricalCounterDataTable()
        {
            DataTable tblResult = electricalCounterGridBody.BuildDataTableFromGrid();

            return tblResult;
        }

        /// <summary>
        /// Get data of a specific column of Electrical Counter grid
        /// </summary>
        /// <returns></returns>
        public List<string> GetListOfElectricalCounterColumnData(string columnName)
        {
            DataTable tblGrid = BuildElectricalCounterDataTable();
            var result = new List<string>();

            if (tblGrid.Columns.Contains(columnName))
            {
                result = tblGrid.AsEnumerable().Select(r => r.Field<string>(columnName)).ToList();
            }

            return result;
        }

        public void ClickElectricalCounterGridRecord(string text)
        {
            Wait.ForSeconds(1);
            var gridRecordsList = Driver.FindElements(By.CssSelector("[id='slv-realtimebatch-gridpanel-electricalCounter'] tr[id^='grid'][id*='rec']"));
            WebDriverContext.Wait.Until(driver =>
            {
                try
                {
                    var currentRec = gridRecordsList.FirstOrDefault(p => p.Text.Contains(text));
                    currentRec.ClickEx();

                    return true;
                }
                catch (StaleElementReferenceException)
                {
                    return false;
                }
            });
        }

        public List<string> GetListOfElectricalCounterColumnHeaders()
        {
            var results = new List<string>();
            var columnHeaders = electricalCounterGridBody.GetGridHeaders() as IEnumerable;

            foreach (var header in columnHeaders)
            {
                var headerText = header as string;
                headerText = headerText.TrimEx();
                if (!string.IsNullOrEmpty(headerText))
                    results.Add(headerText);
            }

            return results.Distinct().ToList();
        }

        public bool IsElectricalCounterRefreshButtonDisplayed()
        {
            return electricalCounterRefreshButton.Displayed;
        }

        #endregion //Switch

        #endregion //Business methods

        public override void WaitForPanelLoaded()
        {

        }
    }
}