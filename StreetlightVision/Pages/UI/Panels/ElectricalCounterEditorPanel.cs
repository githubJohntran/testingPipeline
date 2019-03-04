using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using StreetlightVision.Extensions;
using StreetlightVision.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StreetlightVision.Pages.UI
{
    public class ElectricalCounterEditorPanel : DeviceEditorPanel
    {
        #region Variables

        #endregion //Variables

        #region IWebElements 

        #region Identity tab

        #region Identity of the energy meter

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-controllerStrId'] .slv-label.editor-label")]
        private IWebElement controllerIdLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-idOnController'] .slv-label.editor-label")]
        private IWebElement identifierLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-controllerStrId-field']")]
        private IWebElement controllerIdDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-idOnController-field']")]
        private IWebElement identifierInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-modelFunctionId'] .slv-label.editor-label")]
        private IWebElement typeOfEquipmentLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-MacAddress'] .slv-label.editor-label")]
        private IWebElement uniqueAddressLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-modelFunctionId-field']")]
        private IWebElement typeOfEquipmentDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-MacAddress-field']")]
        private IWebElement uniqueAddressInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-MacAddress'] .editor-button.icon-barcode")]
        private IWebElement uniqueAddressScanButton;

        #endregion //Identity of the energy meter

        #endregion //Identity tab

        #region Inventory tab

        #region Location group        

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-address'] .slv-label.editor-label")]
        private IWebElement address1Label;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-address-field']")]
        private IWebElement address1Input;

        #endregion //Location group

        #region About the energy meter

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-energymeter-type'] .slv-label.editor-label")]
        private IWebElement typeOfMeterLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-energymeter-type-field']")]
        private IWebElement typeOfMeterDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-install-date'] .slv-label.editor-label")]
        private IWebElement controllerInstallDateLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-install-date-field']")]
        private IWebElement controllerInstallDtInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-comment'] .slv-label.editor-label")]
        private IWebElement commentLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-comment-field']")]
        private IWebElement commentInput;

        #endregion //About the energy meter

        #endregion //Inventory tab

        #endregion //IWebElements

        #region Constructor

        public ElectricalCounterEditorPanel(IWebDriver driver, PageBase page) : base(driver, page)
        {
            PageFactory.InitElements(this, new RetryingElementLocator(driver, TimeSpan.FromMilliseconds(Settings.LocatorRetryingTimeout), TimeSpan.FromMilliseconds(Settings.LocatorPollingInterval)));

            WaitForPanelLoaded();
        }

        #endregion //Constructor

        #region Basic methods

        #region Actions        

        #region Identity tab

        #region Identity of the energy meter

        /// <summary>
        /// Select an item of 'ControllerId' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectControllerIdDropDown(string value)
        {
            controllerIdDropDown.Select(value);
        }

        /// <summary>
        /// Enter a value for 'Identifier' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterIdentifierInput(string value)
        {
            identifierInput.Enter(value);
        }

        /// <summary>
        /// Select an item of 'TypeOfEquipment' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectTypeOfEquipmentDropDown(string value)
        {
            typeOfEquipmentDropDown.Select(value);
        }

        /// <summary>
        /// Enter a value for 'UniqueAddress' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterUniqueAddressInput(string value)
        {
            uniqueAddressInput.Enter(value);
        }

        /// <summary>
        /// Click 'UniqueAddressScan' button
        /// </summary>
        public void ClickUniqueAddressScanButton()
        {
            uniqueAddressScanButton.ClickEx();
        }

        #endregion //Identity of the energy meter

        #endregion //Identity tab

        #region Inventory tab

        #region Location group        

        /// <summary>
        /// Enter a value for 'Address1' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterAddress1Input(string value)
        {
            address1Input.Enter(value);
        }

        #endregion //Location group

        #region About the energy meter

        /// <summary>
        /// Select a value for 'TypeOfMeter' dropdown
        /// </summary>
        /// <param name="value"></param>
        public void SelectTypeOfMeterDropDown(string value)
        {
            typeOfMeterDropDown.Select(value);
        }

        /// <summary>
        /// Enter a value for 'ControllerInstallDate' date input
        /// </summary>
        /// <param name="value"></param>
        public void EnterControllerInstallDateInput(string value)
        {
            controllerInstallDtInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'Comment' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterCommentInput(string value)
        {
            commentInput.Enter(value);
        }

        #endregion //About the energy meter

        #endregion //Inventory tab      

        #endregion //Actions

        #region Get methods

        #region Identity tab

        #region Identity of the energy meter

        /// <summary>
        /// Get 'ControllerId' label text
        /// </summary>
        /// <returns></returns>
        public string GetControllerIdText()
        {
            return controllerIdLabel.Text;
        }

        /// <summary>
        /// Get 'Identifier' label text
        /// </summary>
        /// <returns></returns>
        public string GetIdentifierText()
        {
            return identifierLabel.Text;
        }

        /// <summary>
        /// Get 'ControllerId' input value
        /// </summary>
        /// <returns></returns>
        public string GetControllerIdValue()
        {
            return controllerIdDropDown.Text;
        }

        /// <summary>
        /// Get all items of 'ControllerId' dropdown 
        /// </summary>
        /// <returns></returns>
        public List<string> GetControllersItems()
        {
            return controllerIdDropDown.GetAllItems();
        }

        /// <summary>
        /// Get 'Identifier' input value
        /// </summary>
        /// <returns></returns>
        public string GetIdentifierValue()
        {
            return identifierInput.Value();
        }

        /// <summary>
        /// Get 'TypeOfEquipment' label text
        /// </summary>
        /// <returns></returns>
        public string GetTypeOfEquipmentText()
        {
            return typeOfEquipmentLabel.Text;
        }

        /// <summary>
        /// Get 'UniqueAddress' label text
        /// </summary>
        /// <returns></returns>
        public string GetUniqueAddressText()
        {
            return uniqueAddressLabel.Text;
        }

        /// <summary>
        /// Get 'TypeOfEquipment' dropdown value
        /// </summary>
        /// <returns></returns>
        public string GetTypeOfEquipmentValue()
        {
            return typeOfEquipmentDropDown.Text;
        }

        /// <summary>
        /// Get 'UniqueAddress' input value
        /// </summary>
        /// <returns></returns>
        public string GetUniqueAddressValue()
        {
            return uniqueAddressInput.Value();
        }

        #endregion //Identity of the energy meter

        #endregion //Identity tab

        #region Inventory tab    

        #region Location group        

        /// <summary>
        /// Get 'Address1' label text
        /// </summary>
        /// <returns></returns>
        public string GetAddress1Text()
        {
            return address1Label.Text;
        }

        /// <summary>
        /// Get 'Address1' input value
        /// </summary>
        /// <returns></returns>
        public string GetAddress1Value()
        {
            return address1Input.Value();
        }

        #endregion //Location group    

        #region About the energy meter

        /// <summary>
        /// Get 'TypeOfMeter' label text
        /// </summary>
        /// <returns></returns>
        public string GetTypeOfMeterText()
        {
            return typeOfMeterLabel.Text;
        }

        /// <summary>
        /// Get 'TypeOfMeter' input value
        /// </summary>
        /// <returns></returns>
        public string GetTypeOfMeterValue()
        {
            return typeOfMeterDropDown.Text;
        }

        /// <summary>
        /// Get 'ControllerInstallDate' label text
        /// </summary>
        /// <returns></returns>
        public string GetControllerInstallDateText()
        {
            return controllerInstallDateLabel.Text;
        }

        /// <summary>
        /// Get 'ControllerInstallDate' input value
        /// </summary>
        /// <returns></returns>
        public string GetControllerInstallDateValue()
        {
            return controllerInstallDtInput.Value();
        }

        /// <summary>
        /// Get 'Comment' label text
        /// </summary>
        /// <returns></returns>
        public string GetCommentText()
        {
            return commentLabel.Text;
        }

        /// <summary>
        /// Get 'Comment' input value
        /// </summary>
        /// <returns></returns>
        public string GetCommentValue()
        {
            return commentInput.Value();
        }

        #endregion //About the energy meter

        #endregion //Inventory tab        

        #endregion //Get methods

        #endregion //Basic methods

        #region Business methods        

        public List<string> GetListOfEquipmentTypes()
        {
            return typeOfEquipmentDropDown.GetAllItems();
        }

        public bool IsIdentifierReadOnly()
        {
            return identifierInput.IsReadOnly();
        }

        public bool IsControllerIdDropDownDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-property-controllerStrId-field']"));
        }

        public bool IsIdentifierInputDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id='editor-property-idOnController-field']"));
        }

        public bool IsTypeOfEquipmentDropDownDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-property-modelFunctionId-field']"));
        }

        public bool IsUniqueAddressInputDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-property-MacAddress-field']"));
        }

        public bool IsTypeOfMeterDropDownDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-property-energymeter-type-field']"));
        }

        public bool IsControllerInstallDateInputDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-property-install-date-field"));
        }

        public bool IsCommentInputDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-property-comment-field']"));
        }

        public void SelectRandomControllerIdDropDown()
        {
            var currentValue = GetControllerIdValue();
            //var listItems = controllerIdDropDown.GetAllItems();
            var listItems = new List<string> { "Alarm Area Controller", "Flower Market", "iLON", "Vietnam Controller", "DevCtrl A" };
            listItems.Remove(currentValue);
            controllerIdDropDown.Select(listItems.PickRandom());
        }

        #endregion //Business methods        

        public override void WaitForPanelLoaded()
        {
            Wait.ForElementStyle(By.CssSelector("[id$='equipmentgl-editor']"), string.Format("left: {0}px", WebDriverContext.JsExecutor.ExecuteScript("return screen.availWidth - 350 - 60")));
        }
    }
}
