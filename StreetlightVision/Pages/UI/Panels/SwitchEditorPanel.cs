using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using StreetlightVision.Extensions;
using StreetlightVision.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StreetlightVision.Pages.UI
{
    public class SwitchEditorPanel : DeviceEditorPanel
    {
        #region Variables

        #endregion //Variables

        #region IWebElements 

        #region Identity tab

        #region Identity of the light point group

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-controllerStrId'] .slv-label.editor-label")]
        private IWebElement controllerIdLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-idOnController'] .slv-label.editor-label")]
        private IWebElement identifierLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-controllerStrId-field']")]
        private IWebElement controllerIdDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-idOnController-field']")]
        private IWebElement identifierInput;

        #endregion //Identity of the light point group

        #region Control System group

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-modelFunctionId'] .slv-label.editor-label")]
        private IWebElement typeOfEquipmentLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-DimmingGroupName'] .slv-label.editor-label")]
        private IWebElement dimmingGroupLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-MacAddress'] .slv-label.editor-label")]
        private IWebElement uniqueAddressLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-install-date'] .slv-label.editor-label")]
        private IWebElement controllerInstallDateLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-installStatus'] .slv-label.editor-label")]
        private IWebElement installStatusLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-Bandwidth'] .slv-label.editor-label")]
        private IWebElement bandwidthLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-CircuitIndex'] .slv-label.editor-label")]
        private IWebElement circuitIndexLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-ExternalPort'] .slv-label.editor-label")]
        private IWebElement externalPortLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-installStatus'] .slv-label.editor-label")]
        private IWebElement internalPortLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-modelFunctionId-field']")]
        private IWebElement typeOfEquipmentDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-DimmingGroupName-field']")]
        private IWebElement dimmingGroupDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-MacAddress-field']")]
        private IWebElement uniqueAddressInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-MacAddress'] .editor-button.icon-barcode")]
        private IWebElement uniqueAddressScanButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-install-date-field']")]
        private IWebElement controllerInstallDateInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-installStatus-field']")]
        private IWebElement installStatusDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-Bandwidth-field']")]
        private IWebElement bandwidthDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-CircuitIndex-field']")]
        private IWebElement circuitIndexNumericInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-ExternalPort-field']")]
        private IWebElement externalPortNumericInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-InternalPort-field']")]
        private IWebElement internalPortNumericInput;

        #endregion //Control System group

        #endregion //Identity tab

        #region Inventory tab

        #region Location group        

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-address'] .slv-label.editor-label")]
        private IWebElement address1Label;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-address-field']")]
        private IWebElement address1Input;

        #endregion //Location group

        #region Lamp group

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-brandId'] .slv-label.editor-label")]
        private IWebElement lampTypeLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-power'] .slv-label.editor-label")]
        private IWebElement lampWattageLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-powerCorrection'] .slv-label.editor-label")]
        private IWebElement fixedSavedPowerLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-brandId-field']")]
        private IWebElement lampTypeDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-brandId-field'] .editor-button.icon-edit-property")]
        private IWebElement lampTypeEditButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-power-field']")]
        private IWebElement lampWattageNumericInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-powerCorrection-field']")]
        private IWebElement fixedSavedPowerNumericInput;

        #endregion //Lamp group

        #region Comment group

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-comment'] .slv-label.editor-label")]
        private IWebElement commentLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-comment-field']")]
        private IWebElement commentInput;

        #endregion //Comment group

        #endregion //Inventory tab

        #region Electricity network tab

        #region Network group

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-providerId'] .slv-label.editor-label")]
        private IWebElement energySupplierLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-network-type'] .slv-label.editor-label")]
        private IWebElement networkTypeLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-network-segmentnumber'] .slv-label.editor-label")]
        private IWebElement segmentLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-network-section'] .slv-label.editor-label")]
        private IWebElement sectionLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-providerId-field']")]
        private IWebElement energySupplierDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-providerId-field'] .icon-edit-property.editor-button")]
        private IWebElement energySupplierEditButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-network-type-field']")]
        private IWebElement networkTypeInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-network-segmentnumber-field']")]
        private IWebElement segmentNumericInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-network-section-field']")]
        private IWebElement sectionInput;

        #endregion //Network group

        #endregion //Electricity network tab

        #endregion //IWebElements

        #region Constructor

        public SwitchEditorPanel(IWebDriver driver, PageBase page) : base(driver, page)
        {
            PageFactory.InitElements(this, new RetryingElementLocator(driver, TimeSpan.FromMilliseconds(Settings.LocatorRetryingTimeout), TimeSpan.FromMilliseconds(Settings.LocatorPollingInterval)));

            WaitForPanelLoaded();
        }

        #endregion //Constructor

        #region Basic methods

        #region Actions        

        #region Identity tab

        #region Identity of the light point group

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

        #endregion //Identity of the light point group

        #region Control System group

        /// <summary>
        /// Select an item of 'TypeOfEquipment' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectTypeOfEquipmentDropDown(string value)
        {
            typeOfEquipmentDropDown.Select(value);
        }

        /// <summary>
        /// Select an item of 'DimmingGroup' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectDimmingGroupDropDown(string value)
        {
            dimmingGroupDropDown.Select(value);
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

        /// <summary>
        /// Enter a value for 'ControllerInstallDate' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterControllerInstallDateInput(string value)
        {
            controllerInstallDateInput.Enter(value);
        }

        /// <summary>
        /// Select an item of 'InstallStatus' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectInstallStatusDropDown(string value)
        {
            installStatusDropDown.Select(value);
        }

        /// <summary>
        /// Select an item of 'Bandwidth' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectBandwidthDropDown(string value)
        {
            bandwidthDropDown.Select(value);
        }

        /// <summary>
        /// Enter a value for 'CircuitIndex' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterCircuitIndexNumericInput(string value)
        {
            circuitIndexNumericInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'ExternalPort' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterExternalPortNumericInput(string value)
        {
            externalPortNumericInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'InternalPort' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterInternalPortNumericInput(string value)
        {
            internalPortNumericInput.Enter(value);
        }

        #endregion //Control System group

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

        #region Lamp group

        /// <summary>
        /// Select an item of 'LampType' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectLampTypeDropDown(string value)
        {
            lampTypeDropDown.Select(value);
        }

        /// <summary>
        /// Click 'LampTypeEdit' button
        /// </summary>
        public void ClickLampTypeEditButton()
        {
            lampTypeEditButton.ClickEx();
        }

        /// <summary>
        /// Enter a value for 'LampWattage' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterLampWattageNumericInput(string value)
        {
            lampWattageNumericInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'FixedSavedPower' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterFixedSavedPowerNumericInput(string value)
        {
            fixedSavedPowerNumericInput.Enter(value);
        }

        #endregion //Lamp group        

        #region Comment group

        /// <summary>
        /// Enter a value for 'Comment' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterCommentInput(string value)
        {
            commentInput.Enter(value);
        }

        #endregion //Comment group

        #endregion //Inventory tab

        #region Electricity network tab

        #region Network group

        /// <summary>
        /// Select an item of 'EnergySupplier' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectEnergySupplierDropDown(string value)
        {
            energySupplierDropDown.Select(value);
        }

        /// <summary>
        /// Click 'EnergySupplierEdit' button
        /// </summary>
        public void ClickEnergySupplierEditButton()
        {
            energySupplierEditButton.ClickEx();
        }

        /// <summary>
        /// Enter a value for 'NetworkType' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterNetworkTypeInput(string value)
        {
            networkTypeInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'Segment' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterSegmentNumericInput(string value)
        {
            segmentNumericInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'Section' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterSectionInput(string value)
        {
            sectionInput.Enter(value);
        }

        #endregion //Network group

        #endregion //Electricity network tab

        #endregion //Actions

        #region Get methods

        #region Identity tab

        #region Identity of the light point group

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

        #endregion //Identity of the light point group

        #region Control System group

        /// <summary>
        /// Get 'TypeOfEquipment' label text
        /// </summary>
        /// <returns></returns>
        public string GetTypeOfEquipmentText()
        {
            return typeOfEquipmentLabel.Text;
        }

        /// <summary>
        /// Get 'DimmingGroup' label text
        /// </summary>
        /// <returns></returns>
        public string GetDimmingGroupText()
        {
            return dimmingGroupLabel.Text;
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
        /// Get 'ControllerInstallDate' label text
        /// </summary>
        /// <returns></returns>
        public string GetControllerInstallDateText()
        {
            return controllerInstallDateLabel.Text;
        }

        /// <summary>
        /// Get 'InstallStatus' label text
        /// </summary>
        /// <returns></returns>
        public string GetInstallStatusText()
        {
            return installStatusLabel.Text;
        }

        /// <summary>
        /// Get 'Bandwidth' label text
        /// </summary>
        /// <returns></returns>
        public string GetBandwidthText()
        {
            return bandwidthLabel.Text;
        }

        /// <summary>
        /// Get 'CircuitIndex' label text
        /// </summary>
        /// <returns></returns>
        public string GetCircuitIndexText()
        {
            return circuitIndexLabel.Text;
        }

        /// <summary>
        /// Get 'ExternalPort' label text
        /// </summary>
        /// <returns></returns>
        public string GetExternalPortText()
        {
            return externalPortLabel.Text;
        }

        /// <summary>
        /// Get 'InternalPort' label text
        /// </summary>
        /// <returns></returns>
        public string GetInternalPortText()
        {
            return internalPortLabel.Text;
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
        /// Get 'DimmingGroup' dropdown value
        /// </summary>
        /// <returns></returns>
        public string GetDimmingGroupValue()
        {
            return dimmingGroupDropDown.Text;
        }

        /// <summary>
        /// Get 'UniqueAddress' input value
        /// </summary>
        /// <returns></returns>
        public string GetUniqueAddressValue()
        {
            return uniqueAddressInput.Value();
        }

        /// <summary>
        /// Get 'ControllerInstallDate' input value
        /// </summary>
        /// <returns></returns>
        public string GetControllerInstallDateValue()
        {
            return controllerInstallDateInput.Value();
        }

        /// <summary>
        /// Get 'InstallStatus' dropdown value
        /// </summary>
        /// <returns></returns>
        public string GetInstallStatusValue()
        {
            return installStatusDropDown.Text;
        }

        /// <summary>
        /// Get 'Bandwidth' dropdown value
        /// </summary>
        /// <returns></returns>
        public string GetBandwidthValue()
        {
            return bandwidthDropDown.Text;
        }

        /// <summary>
        /// Get 'CircuitIndex' input value
        /// </summary>
        /// <returns></returns>
        public string GetCircuitIndexValue()
        {
            return circuitIndexNumericInput.Value();
        }

        /// <summary>
        /// Get 'ExternalPort' input value
        /// </summary>
        /// <returns></returns>
        public string GetExternalPortValue()
        {
            return externalPortNumericInput.Value();
        }

        /// <summary>
        /// Get 'InternalPort' input value
        /// </summary>
        /// <returns></returns>
        public string GetInternalPortValue()
        {
            return internalPortNumericInput.Value();
        }

        #endregion //Control System group

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

        #region Lamp group

        /// <summary>
        /// Get 'LampType' label text
        /// </summary>
        /// <returns></returns>
        public string GetLampTypeText()
        {
            return lampTypeLabel.Text;
        }

        /// <summary>
        /// Get 'LampWattage' label text
        /// </summary>
        /// <returns></returns>
        public string GetLampWattageText()
        {
            return lampWattageLabel.Text;
        }

        /// <summary>
        /// Get 'FixedSavedPower' label text
        /// </summary>
        /// <returns></returns>
        public string GetFixedSavedPowerText()
        {
            return fixedSavedPowerLabel.Text;
        }

        /// <summary>
        /// Get 'LampType' input value
        /// </summary>
        /// <returns></returns>
        public string GetLampTypeValue()
        {
            return lampTypeDropDown.Text;
        }

        /// <summary>
        /// Get 'LampWattage' input value
        /// </summary>
        /// <returns></returns>
        public string GetLampWattageValue()
        {
            return lampWattageNumericInput.Value();
        }

        /// <summary>
        /// Get 'FixedSavedPower' input value
        /// </summary>
        /// <returns></returns>
        public string GetFixedSavedPowerValue()
        {
            return fixedSavedPowerNumericInput.Value();
        }

        #endregion //Lamp group        

        #region Comment group

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

        #endregion //Comment group

        #endregion //Inventory tab

        #region Electricity network tab

        #region Network group

        /// <summary>
        /// Get 'EnergySupplier' label text
        /// </summary>
        /// <returns></returns>
        public string GetEnergySupplierText()
        {
            return energySupplierLabel.Text;
        }

        /// <summary>
        /// Get 'NetworkType' label text
        /// </summary>
        /// <returns></returns>
        public string GetNetworkTypeText()
        {
            return networkTypeLabel.Text;
        }

        /// <summary>
        /// Get 'Segment' label text
        /// </summary>
        /// <returns></returns>
        public string GetSegmentText()
        {
            return segmentLabel.Text;
        }

        /// <summary>
        /// Get 'Section' label text
        /// </summary>
        /// <returns></returns>
        public string GetSectionText()
        {
            return sectionLabel.Text;
        }

        /// <summary>
        /// Get 'EnergySupplier' dropdown value
        /// </summary>
        /// <returns></returns>
        public string GetEnergySupplierValue()
        {
            return energySupplierDropDown.Text;
        }

        /// <summary>
        /// Get 'NetworkType' input value
        /// </summary>
        /// <returns></returns>
        public string GetNetworkTypeValue()
        {
            return networkTypeInput.Value();
        }

        /// <summary>
        /// Get 'Segment' input value
        /// </summary>
        /// <returns></returns>
        public string GetSegmentValue()
        {
            return segmentNumericInput.Value();
        }

        /// <summary>
        /// Get 'Section' input value
        /// </summary>
        /// <returns></returns>
        public string GetSectionValue()
        {
            return sectionInput.Value();
        }

        #endregion //Network group

        #endregion //Electricity network tab

        #endregion //Get methods

        #endregion //Basic methods

        #region Business methods        

        public List<string> GetistOfEquipmentTypes()
        {
            return typeOfEquipmentDropDown.GetAllItems();
        }

        public List<string> GetListOfEnergySuppliers()
        {
            return energySupplierDropDown.GetAllItems();
        }      

        public bool IsIdentifierReadOnly()
        {
            return identifierInput.IsReadOnly();
        }

        public bool IsUniqueAddressReadOnly()
        {
            return uniqueAddressInput.IsReadOnly();
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

        public bool IsDimmingGroupDropDownDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-property-DimmingGroupName-field']"));
        }

        public bool IsUniqueAddressInputDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-property-MacAddress-field']"));
        }

        public bool IsControllerInstallDateInputDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-property-install-date-field']"));
        }

        public bool IsInstallStatusDropDownDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-property-installStatus-field']"));
        }

        public bool IsLampTypeDropDownDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-property-brandId-field']"));
        }

        public bool IsLampWattageInputDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-property-power-field']"));
        }

        public bool IsFixedSavedPowerInputDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-property-powerCorrection-field']"));
        }

        /// <summary>
        /// Get all dimming groups items
        /// </summary>
        public List<string> GetListOfDimmingGroups()
        {
            return dimmingGroupDropDown.GetAllItems();
        }

        public void SelectRandomControllerIdDropDown()
        {
            var currentValue = GetControllerIdValue();
            //var listItems = controllerIdDropDown.GetAllItems();
            var listItems = new List<string> { "Alarm Area Controller", "Flower Market", "iLON", "Vietnam Controller", "DevCtrl A" };
            listItems.Remove(currentValue);
            controllerIdDropDown.Select(listItems.PickRandom());
        }

        public void SelectRandomDimmingGroupDropDown()
        {
            var currentValue = GetDimmingGroupValue();
            var listItems = dimmingGroupDropDown.GetAllItems();
            listItems.Remove(currentValue);
            listItems.RemoveAll(p => p.Contains("SlvDemoGroup") || string.IsNullOrEmpty(p));
            dimmingGroupDropDown.Select(listItems.PickRandom());
        }

        public void SelectRandomInstallStatusDropDown()
        {
            var currentValue = GetInstallStatusValue();
            var listItems = installStatusDropDown.GetAllItems();
            listItems.Remove(currentValue);
            installStatusDropDown.Select(listItems.PickRandom());
        }

        public void SelectRandomLampTypeDropDown()
        {
            var currentValue = GetLampTypeValue();
            var listItems = lampTypeDropDown.GetAllItems().Where(p => p.Contains("Default")).ToList();
            listItems.Remove(currentValue);
            lampTypeDropDown.Select(listItems.PickRandom());
        }

        public void SelectRandomEnergySupplierDropDown()
        {
            var currentValue = GetEnergySupplierValue();
            var listItems = energySupplierDropDown.GetAllItems();
            listItems.Remove(currentValue);
            energySupplierDropDown.Select(listItems.PickRandom());
        }

        /// <summary>
        /// Get all equipment types items
        /// </summary>
        /// <returns></returns>
        public List<string> GetListOfEquipmentTypes()
        {
            return typeOfEquipmentDropDown.GetAllItems();
        }

        /// <summary>
        /// Get all lamp type items
        /// </summary>
        public List<string> GetListOfLampTypes()
        {
            return lampTypeDropDown.GetAllItems();
        }

        #endregion //Business methods        

        public override void WaitForPanelLoaded()
        {
            Wait.ForElementStyle(By.CssSelector("[id$='equipmentgl-editor']"), string.Format("left: {0}px", WebDriverContext.JsExecutor.ExecuteScript("return screen.availWidth - 350 - 60")));
        }
    }
}
