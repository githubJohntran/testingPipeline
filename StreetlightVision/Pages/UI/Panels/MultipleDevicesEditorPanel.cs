using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using StreetlightVision.Extensions;
using StreetlightVision.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StreetlightVision.Pages.UI
{
    public class MultipleDevicesEditorPanel : NodeEditorPanelBase
    {
        #region Variables
        
        #endregion //Variables

        #region IWebElements        

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-devices'] .equipment-gl-editor-title-label")]
        private IWebElement panelTitle;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-devices'] .equipment-gl-list-item")]
        private IList<IWebElement> devicesList;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-devices-content-content-properties-tabs'] div.w2ui-tab")]
        private IList<IWebElement> tabList;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-devices-content-content-properties-tabs'] div.w2ui-tab.active")]
        private IWebElement activeTab;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-devices'] div.editor-group-header")]
        private IList<IWebElement> editorGroupHeaderList;

        #region Header toolbar buttons

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-devices-backButton']")]
        private IWebElement backButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-devices-buttons-toolbar_item_save'] .w2ui-button")]
        private IWebElement saveButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-devices-buttons-toolbar_item_delete'] .w2ui-button")]
        private IWebElement deleteButton;

        #endregion //Header toolbar buttons

        #region Identity tab

        #region Identity of the light point group

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-controllerStrId'] .slv-label.editor-label")]
        private IWebElement controllerIdLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-controllerStrId-field']")]
        private IWebElement controllerIdDropDown;

        #endregion //Identity of the light point group

        #region Control System group

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-modelFunctionId'] .slv-label.editor-label")]
        private IWebElement typeEquipmentLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-DimmingGroupName'] .slv-label.editor-label")]
        private IWebElement dimmingGroupLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-installStatus'] .slv-label.editor-label")]
        private IWebElement installStatusLabel;        

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-device-node-hwversion'] .slv-label.editor-label")]
        private IWebElement deviceHwVersionLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-device-node-swversion'] .slv-label.editor-label")]
        private IWebElement deviceSwVersionLabel;     

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-modelFunctionId-field']")]
        private IWebElement typeEquipmentDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-DimmingGroupName-field']")]
        private IWebElement dimmingGroupDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-MacAddress'] .editor-button.icon-barcode")]
        private IWebElement uniqueAddressScanButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-installStatus-field']")]
        private IWebElement installStatusDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-device-node-hwversion-field']")]
        private IWebElement deviceHwVersionInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-device-node-swversion-field']")]
        private IWebElement deviceSwVersionInput;        

        #endregion //Control System group        

        #endregion //Identity tab

        #region Inventory tab

        #region Location group

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-device-premise'] .slv-label.editor-label")]
        private IWebElement premiseLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-address'] .slv-label.editor-label")]
        private IWebElement address1Label;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-location-streetdescription'] .slv-label.editor-label")]
        private IWebElement address2Label;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-location-city'] .slv-label.editor-label")]
        private IWebElement cityLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-location-zipcode'] .slv-label.editor-label")]
        private IWebElement zipCodeLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-location-mapnumber'] .slv-label.editor-label")]
        private IWebElement mapNumberLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-location-locationtype'] .slv-label.editor-label")]
        private IWebElement locationTypeLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-device-premise-field']")]
        private IWebElement premiseInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-address-field']")]
        private IWebElement address1Input;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-location-streetdescription-field']")]
        private IWebElement address2Input;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-location-city-field']")]
        private IWebElement cityInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-location-zipcode-field']")]
        private IWebElement zipCodeInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-location-mapnumber-field']")]
        private IWebElement mapNumberInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-location-locationtype-field']")]
        private IWebElement locationTypeDropDown;        

        #endregion //Location group

        #region Customer group

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-client-accountnumber'] .slv-label.editor-label")]
        private IWebElement accountNumberLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-client-number'] .slv-label.editor-label")]
        private IWebElement customerNumberLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-client-name'] .slv-label.editor-label")]
        private IWebElement customerNameLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-client-accountnumber-field']")]
        private IWebElement accountNumberInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-client-number-field']")]
        private IWebElement customerNumberInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-client-name-field']")]
        private IWebElement customerNameInput;

        #endregion //Customer group

        #region Lamp group

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-brandId'] .slv-label.editor-label")]
        private IWebElement lampTypeLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-power'] .slv-label.editor-label")]
        private IWebElement lampWattageLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-powerCorrection'] .slv-label.editor-label")]
        private IWebElement fixedSavedPowerLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-lamp-installdate'] .slv-label.editor-label")]
        private IWebElement lampInstallDateLabel;      

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-brandId-field']")]
        private IWebElement lampTypeDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-brandId-field'] .editor-button.icon-edit-property")]
        private IWebElement lampTypeEditButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-power-field']")]
        private IWebElement lampWattageNumericInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-powerCorrection-field']")]
        private IWebElement fixedSavedPowerNumericInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-lamp-installdate-field']")]
        private IWebElement lampInstallDateInput;

        #endregion //Lamp group

        #region Driver or ballast group

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-ballast-type'] .slv-label.editor-label")]
        private IWebElement ballastTypeLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-ballast-dimmingtype'] .slv-label.editor-label")]
        private IWebElement dimmingInterfaceLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-ballast-brand'] .slv-label.editor-label")]
        private IWebElement ballastBrandLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-ballast-installation'] .slv-label.editor-label")]
        private IWebElement poleHeadInstallLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-ballast-type-field']")]
        private IWebElement ballastTypeInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-ballast-dimmingtype-field']")]
        private IWebElement dimmingInterfaceDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-ballast-brand-field']")]
        private IWebElement ballastBrandInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-ballast-installation-field']")]
        private IWebElement poleHeadInstallInput;

        #endregion //Driver or ballast group

        #region Luminaire group

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-luminaire-brand'] .slv-label.editor-label")]
        private IWebElement luminaireBrandLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-luminaire-type'] .slv-label.editor-label")]
        private IWebElement luminaireTypeLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-luminaire-model'] .slv-label.editor-label")]
        private IWebElement luminaireModelLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-luminaire-DistributionType'] .slv-label.editor-label")]
        private IWebElement lightDistributionLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-luminaire-orientation'] .slv-label.editor-label")]
        private IWebElement orientationLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-luminaire-colorcode'] .slv-label.editor-label")]
        private IWebElement colorCodeLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-luminaire-status'] .slv-label.editor-label")]
        private IWebElement statusLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-luminaire-installdate'] .slv-label.editor-label")]
        private IWebElement luminaireInstallDateLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-luminaire-brand-field']")]
        private IWebElement luminaireBrandInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-luminaire-type-field']")]
        private IWebElement luminaireTypeDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-luminaire-model-field']")]
        private IWebElement luminaireModelInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-luminaire-DistributionType-field']")]
        private IWebElement lightDistributionInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-luminaire-orientation-field']")]
        private IWebElement orientationDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-luminaire-colorcode-field']")]
        private IWebElement colorCodeInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-luminaire-status-field']")]
        private IWebElement statusInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-luminaire-installdate-field']")]
        private IWebElement luminaireInstallDateInput;

        #endregion //Luminaire group

        #region Bracket group

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-fixing-brand'] .slv-label.editor-label")]
        private IWebElement bracketBrandLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-fixing-model'] .slv-label.editor-label")]
        private IWebElement bracketModelLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-fixing-type'] .slv-label.editor-label")]
        private IWebElement bracketTypeLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-fixing-color'] .slv-label.editor-label")]
        private IWebElement bracketColorLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-fixing-brand-field']")]
        private IWebElement bracketBrandInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-fixing-model-field']")]
        private IWebElement bracketModelInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-fixing-type-field']")]
        private IWebElement bracketTypeInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-fixing-color-field']")]
        private IWebElement bracketColorInput;

        #endregion //Bracket group

        #region Pole or support group

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-pole-type'] .slv-label.editor-label")]
        private IWebElement poleTypeLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-pole-height'] .slv-label.editor-label")]
        private IWebElement poleHeightLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-pole-shape'] .slv-label.editor-label")]
        private IWebElement poleShapeLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-pole-material'] .slv-label.editor-label")]
        private IWebElement poleMaterialLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-pole-colorcode'] .slv-label.editor-label")]
        private IWebElement poleColorCodeLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-pole-status'] .slv-label.editor-label")]
        private IWebElement poleStatusLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-pole-groundtype'] .slv-label.editor-label")]
        private IWebElement typeGroundFixingLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-pole-installdate'] .slv-label.editor-label")]
        private IWebElement poleInstallDateLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-pole-type-field']")]
        private IWebElement poleTypeInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-pole-height-field']")]
        private IWebElement poleHeightInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-pole-shape-field']")]
        private IWebElement poleShapeInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-pole-material-field']")]
        private IWebElement poleMaterialInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-pole-colorcode-field']")]
        private IWebElement poleColorCodeInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-pole-status-field']")]
        private IWebElement poleStatusInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-pole-groundtype-field']")]
        private IWebElement typeGroundFixingInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-pole-installdate-field']")]
        private IWebElement poleInstallDateInput;

        #endregion //Pole or support group

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

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-network-supplyvoltage'] .slv-label.editor-label")]
        private IWebElement supplyVoltageLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-network-segmentnumber'] .slv-label.editor-label")]
        private IWebElement segmentLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-network-section'] .slv-label.editor-label")]
        private IWebElement sectionLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-network-highvoltagethreshold'] .slv-label.editor-label")]
        private IWebElement highVoltageThresholdLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-network-lowvoltagethreshold'] .slv-label.editor-label")]
        private IWebElement lowVoltageThresholdLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-providerId-field']")]
        private IWebElement energySupplierDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-providerId-field'] .icon-edit-property.editor-button")]
        private IWebElement energySupplierEditButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-network-type-field']")]
        private IWebElement networkTypeInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-network-supplyvoltage-field']")]
        private IWebElement supplyVoltageDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-network-segmentnumber-field']")]
        private IWebElement segmentNumericInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-network-section-field']")]
        private IWebElement sectionInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-network-highvoltagethreshold-field']")]
        private IWebElement highVoltageThresholdNumericInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-network-lowvoltagethreshold-field']")]
        private IWebElement lowVoltageThresholdNumericInput;

        #endregion //Network group

        #endregion //Electricity network tab

        #endregion //IWebElements

        #region Constructor

        public MultipleDevicesEditorPanel(IWebDriver driver, PageBase page) : base(driver, page)
        {
            PageFactory.InitElements(this, new RetryingElementLocator(driver, TimeSpan.FromMilliseconds(Settings.LocatorRetryingTimeout), TimeSpan.FromMilliseconds(Settings.LocatorPollingInterval)));

            WaitForPanelLoaded();
        }

        #endregion //Constructor

        #region Properties

        #endregion //Properties

        #region Basic methods

        #region Actions

        #region Header toolbar buttons

        /// <summary>
        /// Click 'Back' button
        /// </summary>
        public void ClickBackButton()
        {
            backButton.ClickEx();
        }

        /// <summary>
        /// Click 'Save' button
        /// </summary>
        public void ClickSaveButton()
        {
            saveButton.ClickEx();
        }

        /// <summary>
        /// Click 'Delete' button
        /// </summary>
        public void ClickDeleteButton()
        {
            deleteButton.ClickEx();
        }

        #endregion //Header toolbar buttons

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

        #endregion //Identity of the light point group

        #region Control System group

        /// <summary>
        /// Select an item of 'TypeEquipment' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectTypeEquipmentDropDown(string value)
        {
            typeEquipmentDropDown.Select(value);
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
        /// Click 'UniqueAddressScan' button
        /// </summary>
        public void ClickUniqueAddressScanButton()
        {
            uniqueAddressScanButton.ClickEx();
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
        /// Enter a value for 'DeviceHwVersion' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterDeviceHwVersionInput(string value)
        {
            deviceHwVersionInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'DeviceSwVersion' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterDeviceSwVersionInput(string value)
        {
            deviceSwVersionInput.Enter(value);
        }

        #endregion //Control System group        

        #endregion //Identity tab

        #region Inventory tab

        #region Location group

        /// <summary>
        /// Enter a value for 'Premise' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterPremiseInput(string value)
        {
            premiseInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'Address1' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterAddress1Input(string value)
        {
            address1Input.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'Address2' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterAddress2Input(string value)
        {
            address2Input.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'City' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterCityInput(string value)
        {
            cityInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'ZipCode' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterZipCodeInput(string value)
        {
            zipCodeInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'MapNumber' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterMapNumberInput(string value)
        {
            mapNumberInput.Enter(value);
        }

        /// <summary>
        /// Select an item of 'LocationType' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectLocationTypeDropDown(string value)
        {
            locationTypeDropDown.Select(value);
        }

        #endregion //Location group

        #region Customer group

        /// <summary>
        /// Enter a value for 'AccountNumber' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterAccountNumberInput(string value)
        {
            accountNumberInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'CustomerNumber' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterCustomerNumberInput(string value)
        {
            customerNumberInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'CustomerName' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterCustomerNameInput(string value)
        {
            customerNameInput.Enter(value);
        }

        #endregion //Customer group

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

        /// <summary>
        /// Enter a value for 'LampInstall' date input
        /// </summary>
        /// <param name="value"></param>
        public void EnterLampInstallDateInput(DateTime value)
        {
            lampInstallDateInput.Enter(value);
        }

        #endregion //Lamp group

        #region Driver or ballast group

        /// <summary>
        /// Enter a value for 'BallastType' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterBallastTypeInput(string value)
        {
            ballastTypeInput.Enter(value);
        }

        /// <summary>
        /// Select an item of 'DimmingInterface' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectDimmingInterfaceDropDown(string value)
        {
            dimmingInterfaceDropDown.Select(value);
        }

        /// <summary>
        /// Enter a value for 'BallastBrand' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterBallastBrandInput(string value)
        {
            ballastBrandInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'PoleHeadInstall' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterPoleHeadInstallInput(string value)
        {
            poleHeadInstallInput.Enter(value);
        }

        #endregion //Driver or ballast group

        #region Luminaire group

        /// <summary>
        /// Enter a value for 'LuminaireBrand' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterLuminaireBrandInput(string value)
        {
            luminaireBrandInput.Enter(value);
        }

        /// <summary>
        /// Select an item of 'LuminaireType' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectLuminaireTypeDropDown(string value)
        {
            luminaireTypeDropDown.Select(value);
        }

        /// <summary>
        /// Enter a value for 'LuminaireModel' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterLuminaireModelInput(string value)
        {
            luminaireModelInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'LightDistribution' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterLightDistributionInput(string value)
        {
            lightDistributionInput.Enter(value);
        }

        /// <summary>
        /// Select an item of 'Orientation' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectOrientationDropDown(string value)
        {
            orientationDropDown.Select(value);
        }

        /// <summary>
        /// Enter a value for 'ColorCode' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterColorCodeInput(string value)
        {
            colorCodeInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'Status' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterStatusInput(string value)
        {
            statusInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'LuminaireInstall' date input
        /// </summary>
        /// <param name="value"></param>
        public void EnterLuminaireInstallDateInput(DateTime value)
        {
            luminaireInstallDateInput.Enter(value);
        }

        #endregion //Luminaire group

        #region Bracket group

        /// <summary>
        /// Enter a value for 'BracketBrand' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterBracketBrandInput(string value)
        {
            bracketBrandInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'BracketModel' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterBracketModelInput(string value)
        {
            bracketModelInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'BracketType' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterBracketTypeInput(string value)
        {
            bracketTypeInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'BracketColor' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterBracketColorInput(string value)
        {
            bracketColorInput.Enter(value);
        }

        #endregion //Bracket group

        #region Pole or support group

        /// <summary>
        /// Enter a value for 'PoleType' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterPoleTypeInput(string value)
        {
            poleTypeInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'PoleHeight' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterPoleHeightInput(string value)
        {
            poleHeightInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'PoleShape' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterPoleShapeInput(string value)
        {
            poleShapeInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'PoleMaterial' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterPoleMaterialInput(string value)
        {
            poleMaterialInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'PoleColorCode' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterPoleColorCodeInput(string value)
        {
            poleColorCodeInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'PoleStatus' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterPoleStatusInput(string value)
        {
            poleStatusInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'TypeGroundFixing' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterTypeGroundFixingInput(string value)
        {
            typeGroundFixingInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'PoleInstall' date input
        /// </summary>
        /// <param name="value"></param>
        public void EnterPoleInstallDateInput(DateTime value)
        {
            poleInstallDateInput.Enter(value);
        }

        #endregion //Pole or support group

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
        /// Select an item of 'SupplyVoltage' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectSupplyVoltageDropDown(string value)
        {
            supplyVoltageDropDown.Select(value);
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

        /// <summary>
        /// Enter a value for 'HighVoltageThreshold' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterHighVoltageThresholdNumericInput(string value)
        {
            highVoltageThresholdNumericInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'LowVoltageThreshold' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterLowVoltageThresholdNumericInput(string value)
        {
            lowVoltageThresholdNumericInput.Enter(value);
        }

        #endregion //Network group

        #endregion //Electricity network tab

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
        /// Get 'ActiveTab' label text
        /// </summary>
        /// <returns></returns>
        public string GetActiveTabText()
        {
            return activeTab.Text;
        }

        #region Header toolbar buttons

        #endregion //Header toolbar buttons

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
        /// Get 'ControllerId' dropdown value
        /// </summary>
        /// <returns></returns>
        public string GetControllerIdValue()
        {
            return controllerIdDropDown.Text;
        }

        #endregion //Identity of the light point group

        #region Control System group

        /// <summary>
        /// Get 'TypeEquipment' label text
        /// </summary>
        /// <returns></returns>
        public string GetTypeEquipmentText()
        {
            return typeEquipmentLabel.Text;
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
        /// Get 'InstallStatus' label text
        /// </summary>
        /// <returns></returns>
        public string GetInstallStatusText()
        {
            return installStatusLabel.Text;
        }

        /// <summary>
        /// Get 'DeviceHwVersion' label text
        /// </summary>
        /// <returns></returns>
        public string GetDeviceHwVersionText()
        {
            return deviceHwVersionLabel.Text;
        }

        /// <summary>
        /// Get 'DeviceSwVersion' label text
        /// </summary>
        /// <returns></returns>
        public string GetDeviceSwVersionText()
        {
            return deviceSwVersionLabel.Text;
        }

        /// <summary>
        /// Get 'TypeEquipment' dropdown value
        /// </summary>
        /// <returns></returns>
        public string GetTypeEquipmentValue()
        {
            return typeEquipmentDropDown.Text;
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
        /// Get 'InstallStatus' dropdown value
        /// </summary>
        /// <returns></returns>
        public string GetInstallStatusValue()
        {
            return installStatusDropDown.Text;
        }

        /// <summary>
        /// Get 'DeviceHwVersion' input value
        /// </summary>
        /// <returns></returns>
        public string GetDeviceHwVersionValue()
        {
            return deviceHwVersionInput.Value();
        }

        /// <summary>
        /// Get 'DeviceSwVersion' input value
        /// </summary>
        /// <returns></returns>
        public string GetDeviceSwVersionValue()
        {
            return deviceSwVersionInput.Value();
        }

        #endregion //Control System group        

        #endregion //Identity tab

        #region Inventory tab

        #region Location group

        /// <summary>
        /// Get 'Premise' label text
        /// </summary>
        /// <returns></returns>
        public string GetPremiseText()
        {
            return premiseLabel.Text;
        }

        /// <summary>
        /// Get 'Address1' label text
        /// </summary>
        /// <returns></returns>
        public string GetAddress1Text()
        {
            return address1Label.Text;
        }

        /// <summary>
        /// Get 'Address2' label text
        /// </summary>
        /// <returns></returns>
        public string GetAddress2Text()
        {
            return address2Label.Text;
        }

        /// <summary>
        /// Get 'City' label text
        /// </summary>
        /// <returns></returns>
        public string GetCityText()
        {
            return cityLabel.Text;
        }

        /// <summary>
        /// Get 'ZipCode' label text
        /// </summary>
        /// <returns></returns>
        public string GetZipCodeText()
        {
            return zipCodeLabel.Text;
        }

        /// <summary>
        /// Get 'MapNumber' label text
        /// </summary>
        /// <returns></returns>
        public string GetMapNumberText()
        {
            return mapNumberLabel.Text;
        }

        /// <summary>
        /// Get 'LocationType' label text
        /// </summary>
        /// <returns></returns>
        public string GetLocationTypeText()
        {
            return locationTypeLabel.Text;
        }

        /// <summary>
        /// Get 'Premise' input value
        /// </summary>
        /// <returns></returns>
        public string GetPremiseValue()
        {
            return premiseInput.Value();
        }

        /// <summary>
        /// Get 'Address1' input value
        /// </summary>
        /// <returns></returns>
        public string GetAddress1Value()
        {
            return address1Input.Value();
        }

        /// <summary>
        /// Get 'Address2' input value
        /// </summary>
        /// <returns></returns>
        public string GetAddress2Value()
        {
            return address2Input.Value();
        }

        /// <summary>
        /// Get 'City' input value
        /// </summary>
        /// <returns></returns>
        public string GetCityValue()
        {
            return cityInput.Value();
        }

        /// <summary>
        /// Get 'ZipCode' input value
        /// </summary>
        /// <returns></returns>
        public string GetZipCodeValue()
        {
            return zipCodeInput.Value();
        }

        /// <summary>
        /// Get 'MapNumber' input value
        /// </summary>
        /// <returns></returns>
        public string GetMapNumberValue()
        {
            return mapNumberInput.Value();
        }

        /// <summary>
        /// Get 'LocationType' dropdown value
        /// </summary>
        /// <returns></returns>
        public string GetLocationTypeValue()
        {
            return locationTypeDropDown.Text;
        }

        #endregion //Location group

        #region Customer group

        /// <summary>
        /// Get 'AccountNumber' label text
        /// </summary>
        /// <returns></returns>
        public string GetAccountNumberText()
        {
            return accountNumberLabel.Text;
        }

        /// <summary>
        /// Get 'CustomerNumber' label text
        /// </summary>
        /// <returns></returns>
        public string GetCustomerNumberText()
        {
            return customerNumberLabel.Text;
        }

        /// <summary>
        /// Get 'CustomerName' label text
        /// </summary>
        /// <returns></returns>
        public string GetCustomerNameText()
        {
            return customerNameLabel.Text;
        }

        /// <summary>
        /// Get 'AccountNumber' input value
        /// </summary>
        /// <returns></returns>
        public string GetAccountNumberValue()
        {
            return accountNumberInput.Value();
        }

        /// <summary>
        /// Get 'CustomerNumber' input value
        /// </summary>
        /// <returns></returns>
        public string GetCustomerNumberValue()
        {
            return customerNumberInput.Value();
        }

        /// <summary>
        /// Get 'CustomerName' input value
        /// </summary>
        /// <returns></returns>
        public string GetCustomerNameValue()
        {
            return customerNameInput.Value();
        }

        #endregion //Customer group

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
        /// Get 'LampInstallDate' label text
        /// </summary>
        /// <returns></returns>
        public string GetLampInstallDateText()
        {
            return lampInstallDateLabel.Text;
        }

        /// <summary>
        /// Get 'LampType' dropdown value
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

        /// <summary>
        /// Get 'LampInstall' input value
        /// </summary>
        /// <returns></returns>
        public string GetLampInstallValue()
        {
            return lampInstallDateInput.Value();
        }

        #endregion //Lamp group

        #region Driver or ballast group

        /// <summary>
        /// Get 'BallastType' label text
        /// </summary>
        /// <returns></returns>
        public string GetBallastTypeText()
        {
            return ballastTypeLabel.Text;
        }

        /// <summary>
        /// Get 'DimmingInterface' label text
        /// </summary>
        /// <returns></returns>
        public string GetDimmingInterfaceText()
        {
            return dimmingInterfaceLabel.Text;
        }

        /// <summary>
        /// Get 'BallastBrand' label text
        /// </summary>
        /// <returns></returns>
        public string GetBallastBrandText()
        {
            return ballastBrandLabel.Text;
        }

        /// <summary>
        /// Get 'PoleHeadInstall' label text
        /// </summary>
        /// <returns></returns>
        public string GetPoleHeadInstallText()
        {
            return poleHeadInstallLabel.Text;
        }

        /// <summary>
        /// Get 'BallastType' input value
        /// </summary>
        /// <returns></returns>
        public string GetBallastTypeValue()
        {
            return ballastTypeInput.Value();
        }

        /// <summary>
        /// Get 'DimmingInterface' dropdown value
        /// </summary>
        /// <returns></returns>
        public string GetDimmingInterfaceValue()
        {
            return dimmingInterfaceDropDown.Text;
        }

        /// <summary>
        /// Get 'BallastBrand' input value
        /// </summary>
        /// <returns></returns>
        public string GetBallastBrandValue()
        {
            return ballastBrandInput.Value();
        }

        /// <summary>
        /// Get 'PoleHeadInstall' input value
        /// </summary>
        /// <returns></returns>
        public string GetPoleHeadInstallValue()
        {
            return poleHeadInstallInput.Value();
        }

        #endregion //Driver or ballast group

        #region Luminaire group

        /// <summary>
        /// Get 'LuminaireBrand' label text
        /// </summary>
        /// <returns></returns>
        public string GetLuminaireBrandText()
        {
            return luminaireBrandLabel.Text;
        }

        /// <summary>
        /// Get 'LuminaireType' label text
        /// </summary>
        /// <returns></returns>
        public string GetLuminaireTypeText()
        {
            return luminaireTypeLabel.Text;
        }

        /// <summary>
        /// Get 'LuminaireModel' label text
        /// </summary>
        /// <returns></returns>
        public string GetLuminaireModelText()
        {
            return luminaireModelLabel.Text;
        }

        /// <summary>
        /// Get 'LightDistribution' label text
        /// </summary>
        /// <returns></returns>
        public string GetLightDistributionText()
        {
            return lightDistributionLabel.Text;
        }

        /// <summary>
        /// Get 'Orientation' label text
        /// </summary>
        /// <returns></returns>
        public string GetOrientationText()
        {
            return orientationLabel.Text;
        }

        /// <summary>
        /// Get 'ColorCode' label text
        /// </summary>
        /// <returns></returns>
        public string GetColorCodeText()
        {
            return colorCodeLabel.Text;
        }

        /// <summary>
        /// Get 'Status' label text
        /// </summary>
        /// <returns></returns>
        public string GetStatusText()
        {
            return statusLabel.Text;
        }

        /// <summary>
        /// Get 'LuminaireInstallDate' label text
        /// </summary>
        /// <returns></returns>
        public string GetLuminaireInstallDateText()
        {
            return luminaireInstallDateLabel.Text;
        }

        /// <summary>
        /// Get 'LuminaireBrand' input value
        /// </summary>
        /// <returns></returns>
        public string GetLuminaireBrandValue()
        {
            return luminaireBrandInput.Value();
        }

        /// <summary>
        /// Get 'LuminaireType' dropdown value
        /// </summary>
        /// <returns></returns>
        public string GetLuminaireTypeValue()
        {
            return luminaireTypeDropDown.Text;
        }

        /// <summary>
        /// Get 'LuminaireModel' input value
        /// </summary>
        /// <returns></returns>
        public string GetLuminaireModelValue()
        {
            return luminaireModelInput.Value();
        }

        /// <summary>
        /// Get 'LightDistribution' input value
        /// </summary>
        /// <returns></returns>
        public string GetLightDistributionValue()
        {
            return lightDistributionInput.Value();
        }

        /// <summary>
        /// Get 'Orientation' dropdown value
        /// </summary>
        /// <returns></returns>
        public string GetOrientationValue()
        {
            return orientationDropDown.Text;
        }

        /// <summary>
        /// Get 'ColorCode' input value
        /// </summary>
        /// <returns></returns>
        public string GetColorCodeValue()
        {
            return colorCodeInput.Value();
        }

        /// <summary>
        /// Get 'Status' input value
        /// </summary>
        /// <returns></returns>
        public string GetStatusValue()
        {
            return statusInput.Value();
        }

        /// <summary>
        /// Get 'LuminaireInstall' input value
        /// </summary>
        /// <returns></returns>
        public string GetLuminaireInstallValue()
        {
            return luminaireInstallDateInput.Value();
        }

        #endregion //Luminaire group

        #region Bracket group

        /// <summary>
        /// Get 'BracketBrand' label text
        /// </summary>
        /// <returns></returns>
        public string GetBracketBrandText()
        {
            return bracketBrandLabel.Text;
        }

        /// <summary>
        /// Get 'BracketModel' label text
        /// </summary>
        /// <returns></returns>
        public string GetBracketModelText()
        {
            return bracketModelLabel.Text;
        }

        /// <summary>
        /// Get 'BracketType' label text
        /// </summary>
        /// <returns></returns>
        public string GetBracketTypeText()
        {
            return bracketTypeLabel.Text;
        }

        /// <summary>
        /// Get 'BracketColor' label text
        /// </summary>
        /// <returns></returns>
        public string GetBracketColorText()
        {
            return bracketColorLabel.Text;
        }

        /// <summary>
        /// Get 'BracketBrand' input value
        /// </summary>
        /// <returns></returns>
        public string GetBracketBrandValue()
        {
            return bracketBrandInput.Value();
        }

        /// <summary>
        /// Get 'BracketModel' input value
        /// </summary>
        /// <returns></returns>
        public string GetBracketModelValue()
        {
            return bracketModelInput.Value();
        }

        /// <summary>
        /// Get 'BracketType' input value
        /// </summary>
        /// <returns></returns>
        public string GetBracketTypeValue()
        {
            return bracketTypeInput.Value();
        }

        /// <summary>
        /// Get 'BracketColor' input value
        /// </summary>
        /// <returns></returns>
        public string GetBracketColorValue()
        {
            return bracketColorInput.Value();
        }

        #endregion //Bracket group

        #region Pole or support group

        /// <summary>
        /// Get 'PoleType' label text
        /// </summary>
        /// <returns></returns>
        public string GetPoleTypeText()
        {
            return poleTypeLabel.Text;
        }

        /// <summary>
        /// Get 'PoleHeight' label text
        /// </summary>
        /// <returns></returns>
        public string GetPoleHeightText()
        {
            return poleHeightLabel.Text;
        }

        /// <summary>
        /// Get 'PoleShape' label text
        /// </summary>
        /// <returns></returns>
        public string GetPoleShapeText()
        {
            return poleShapeLabel.Text;
        }

        /// <summary>
        /// Get 'PoleMaterial' label text
        /// </summary>
        /// <returns></returns>
        public string GetPoleMaterialText()
        {
            return poleMaterialLabel.Text;
        }

        /// <summary>
        /// Get 'PoleColorCode' label text
        /// </summary>
        /// <returns></returns>
        public string GetPoleColorCodeText()
        {
            return poleColorCodeLabel.Text;
        }

        /// <summary>
        /// Get 'PoleStatus' label text
        /// </summary>
        /// <returns></returns>
        public string GetPoleStatusText()
        {
            return poleStatusLabel.Text;
        }

        /// <summary>
        /// Get 'TypeGroundFixing' label text
        /// </summary>
        /// <returns></returns>
        public string GetTypeGroundFixingText()
        {
            return typeGroundFixingLabel.Text;
        }

        /// <summary>
        /// Get 'PoleInstallDate' label text
        /// </summary>
        /// <returns></returns>
        public string GetPoleInstallDateText()
        {
            return poleInstallDateLabel.Text;
        }

        /// <summary>
        /// Get 'PoleType' input value
        /// </summary>
        /// <returns></returns>
        public string GetPoleTypeValue()
        {
            return poleTypeInput.Value();
        }

        /// <summary>
        /// Get 'PoleHeight' input value
        /// </summary>
        /// <returns></returns>
        public string GetPoleHeightValue()
        {
            return poleHeightInput.Value();
        }

        /// <summary>
        /// Get 'PoleShape' input value
        /// </summary>
        /// <returns></returns>
        public string GetPoleShapeValue()
        {
            return poleShapeInput.Value();
        }

        /// <summary>
        /// Get 'PoleMaterial' input value
        /// </summary>
        /// <returns></returns>
        public string GetPoleMaterialValue()
        {
            return poleMaterialInput.Value();
        }

        /// <summary>
        /// Get 'PoleColorCode' input value
        /// </summary>
        /// <returns></returns>
        public string GetPoleColorCodeValue()
        {
            return poleColorCodeInput.Value();
        }

        /// <summary>
        /// Get 'PoleStatus' input value
        /// </summary>
        /// <returns></returns>
        public string GetPoleStatusValue()
        {
            return poleStatusInput.Value();
        }

        /// <summary>
        /// Get 'TypeGroundFixing' input value
        /// </summary>
        /// <returns></returns>
        public string GetTypeGroundFixingValue()
        {
            return typeGroundFixingInput.Value();
        }

        /// <summary>
        /// Get 'PoleInstall' input value
        /// </summary>
        /// <returns></returns>
        public string GetPoleInstallValue()
        {
            return poleInstallDateInput.Value();
        }

        #endregion //Pole or support group

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
        /// Get 'SupplyVoltage' label text
        /// </summary>
        /// <returns></returns>
        public string GetSupplyVoltageText()
        {
            return supplyVoltageLabel.Text;
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
        /// Get 'HighVoltageThreshold' label text
        /// </summary>
        /// <returns></returns>
        public string GetHighVoltageThresholdText()
        {
            return highVoltageThresholdLabel.Text;
        }

        /// <summary>
        /// Get 'LowVoltageThreshold' label text
        /// </summary>
        /// <returns></returns>
        public string GetLowVoltageThresholdText()
        {
            return lowVoltageThresholdLabel.Text;
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
        /// Get 'SupplyVoltage' dropdown value
        /// </summary>
        /// <returns></returns>
        public string GetSupplyVoltageValue()
        {
            return supplyVoltageDropDown.Text;
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

        /// <summary>
        /// Get 'HighVoltageThreshold' input value
        /// </summary>
        /// <returns></returns>
        public string GetHighVoltageThresholdValue()
        {
            return highVoltageThresholdNumericInput.Value();
        }

        /// <summary>
        /// Get 'LowVoltageThreshold' input value
        /// </summary>
        /// <returns></returns>
        public string GetLowVoltageThresholdValue()
        {
            return lowVoltageThresholdNumericInput.Value();
        }

        #endregion //Network group

        #endregion //Electricity network tab

        #endregion //Get methods

        #endregion //Basic methods

        #region Business methods        

        public bool IsBackButtonDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-devices-backButton']"));
        }

        public bool IsSaveButtonDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-devices-buttons-toolbar_item_save'] .w2ui-button"));
        }

        public bool IsDeleteButtonDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-devices-buttons-toolbar_item_delete'] .w2ui-button"));
        }

        public List<string> GetistOfEquipmentTypes()
        {
            return typeEquipmentDropDown.GetAllItems();
        }

        public List<string> GetListOfEnergySuppliers()
        {
            return energySupplierDropDown.GetAllItems();
        }

        /// <summary>
        /// Get all dimming groups items
        /// </summary>
        public List<string> GetListOfDimmingGroups()
        {
            return dimmingGroupDropDown.GetAllItems();
        }

        /// <summary>
        /// Get editor groups
        /// </summary>
        public List<string> GetListOfDevicesName()
        {
            return JSUtility.GetElementsText("[id$='editor-devices'] .equipment-gl-list-item .equipment-gl-list-item-label");
        }

        /// <summary>
        /// Get all tabs as text
        /// </summary>
        public List<string> GetListOfTabsName()
        {
            return JSUtility.GetElementsText("[id$='editor-devices-content-content-properties-tabs'] div.w2ui-tab");
        }

        /// <summary>
        /// Select a tab by its caption
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public void SelectTab(string name)
        {            
            Wait.ForElementsDisplayed(By.CssSelector("[id$='editor-devices-content-content-properties-tabs'] div.w2ui-tab"));
            Wait.ForElementText(By.CssSelector("[id$='editor-devices-content-content-properties-tabs'] div.w2ui-tab.active"));
            var tab = tabList.FirstOrDefault(p => p.Text.Contains(name));
            if (tab != null)
            {
                tab.ClickEx();
                WebDriverContext.Wait.Until(driver => activeTab.Text == name);
            }
        }

        /// <summary>
        /// Check if a tab is exisiting
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool IsTabExisting(string name)
        {
            return tabList.Any(p => p.Text.Contains(name));
        }

        /// <summary>
        /// Get caption of active tab
        /// </summary>
        /// <returns></returns>
        public string GetActiveTabCaption()
        {
            return activeTab.Text;
        }

        /// <summary>
        /// Expand all group of current active tab
        /// </summary>
        public void ExpandGroupsActiveTab()
        {
            var groups = Driver.FindElements(By.CssSelector("[id$='editor-devices'] .editor-tab[style*='display: block'] .editor-group-header"));
            foreach (var group in groups)
            {
                if (group.FindElements(By.CssSelector("div.icon-collapsed")).Count > 0)
                {
                    group.FindElement(By.CssSelector("div.icon-collapsed")).ClickByJS();
                }
            }
        }

        /// <summary>
        /// Get list of all property fields in active tab
        /// </summary>
        /// <param name="exceptNotGetItemsList"></param>
        /// <returns></returns>
        public List<string> GetListOfPropertyNameActiveTab(params string[] exceptNotGetItemsList)
        {
            var result = new List<string>();

            var properties = Driver.FindElements(By.CssSelector("[id$='editor-devices'] .editor-tab[style*='display: block'] .editor-property"));
            foreach (var property in properties)
            {
                var label = property.FindElement(By.CssSelector("div.editor-label:nth-child(1)"));
                var labelText = label.Text.Trim();
                result.Add(labelText);
            }
            return result;
        }

        /// <summary>
        /// Get editor groups
        /// </summary>
        public List<string> GetListOfGroupsName()
        {
            return JSUtility.GetElementsText("[id$='editor-devices'] .editor-group-header div[dir]");
        }

        public List<string> GetListOfGroupsNameActiveTab()
        {
            return JSUtility.GetElementsText("[id$='editor-devices'] .editor-tab[style*='display: block'] .editor-group-header div[dir]");
        }

        public bool IsControllerIdDropDownDisplayed()
        {
            return controllerIdDropDown.Displayed;
        }

        public bool IsTypeEquipmentDropDownDisplayed()
        {
            return typeEquipmentDropDown.Displayed;
        }

        public bool IsDimmingGroupDropDownDisplayed()
        {
            return dimmingGroupDropDown.Displayed;
        }
        
        public bool IsInstallStatusDropDownDisplayed()
        {
            return installStatusDropDown.Displayed;
        }

        public bool IsDeviceHwVersionInputDisplayed()
        {
            return deviceHwVersionInput.Displayed;
        }

        public bool IsDeviceSwVersionInputDisplayed()
        {
            return deviceSwVersionInput.Displayed;
        }

        public bool IsPremiseInputDisplayed()
        {
            return premiseInput.Displayed;
        }

        public bool IsAddress1InputDisplayed()
        {
            return address1Input.Displayed;
        }

        public bool IsAddress2InputDisplayed()
        {
            return address2Input.Displayed;
        }

        public bool IsCityInputDisplayed()
        {
            return cityInput.Displayed;
        }

        public bool IsZipCodeInputDisplayed()
        {
            return zipCodeInput.Displayed;
        }

        public bool IsMapNumberInputDisplayed()
        {
            return mapNumberInput.Displayed;
        }

        public bool IsLocationTypeDropDownDisplayed()
        {
            return locationTypeDropDown.Displayed;
        }
        
        public bool IsAccountNumberInputDisplayed()
        {
            return accountNumberInput.Displayed;
        }

        public bool IsCustomerNameInputDisplayed()
        {
            return customerNameInput.Displayed;
        }

        public bool IsCustomerNumberInputDisplayed()
        {
            return customerNumberInput.Displayed;
        }

        public bool IsLampTypeDropDownDisplayed()
        {
            return lampTypeDropDown.Displayed;
        }

        public bool IsLampWattageNumericInputDisplayed()
        {
            return lampWattageNumericInput.Displayed;
        }

        public bool IsFixedSavedPowerNumericInputDisplayed()
        {
            return fixedSavedPowerNumericInput.Displayed;
        }

        public bool IsLampInstallDateInputDisplayed()
        {
            return lampInstallDateInput.Displayed;
        }

        public bool IsBallastTypeInputDisplayed()
        {
            return ballastTypeInput.Displayed;
        }

        public bool IsDimmingInterfaceDropDownDisplayed()
        {
            return dimmingInterfaceDropDown.Displayed;
        }

        public bool IsBallastBrandInputDisplayed()
        {
            return ballastBrandInput.Displayed;
        }

        public bool IsPoleHeadInstallInputDisplayed()
        {
            return poleHeadInstallInput.Displayed;
        }

        public bool IsLuminaireBrandInputDisplayed()
        {
            return luminaireBrandInput.Displayed;
        }

        public bool IsLuminaireTypeDropDownDisplayed()
        {
            return luminaireTypeDropDown.Displayed;
        }

        public bool IsLuminaireModelInputDisplayed()
        {
            return luminaireModelInput.Displayed;
        }

        public bool IsLightDistributionInputDisplayed()
        {
            return lightDistributionInput.Displayed;
        }

        public bool IsOrientationDropDownDisplayed()
        {
            return orientationDropDown.Displayed;
        }

        public bool IsColorCodeInputDisplayed()
        {
            return colorCodeInput.Displayed;
        }

        public bool IsStatusInputDisplayed()
        {
            return statusInput.Displayed;
        }

        public bool IsLuminaireInstallDateInputDisplayed()
        {
            return luminaireInstallDateInput.Displayed;
        }

        public bool IsBracketBrandInputDisplayed()
        {
            return bracketBrandInput.Displayed;
        }

        public bool IsBracketModelInputDisplayed()
        {
            return bracketModelInput.Displayed;
        }

        public bool IsBracketTypeInputDisplayed()
        {
            return bracketTypeInput.Displayed;
        }

        public bool IsBracketColorInputDisplayed()
        {
            return bracketColorInput.Displayed;
        }

        public bool IsPoleTypeInputDisplayed()
        {
            return poleTypeInput.Displayed;
        }

        public bool IsPoleHeightInputDisplayed()
        {
            return poleHeightInput.Displayed;
        }

        public bool IsPoleShapeInputDisplayed()
        {
            return poleShapeInput.Displayed;
        }

        public bool IsPoleMaterialInputDisplayed()
        {
            return poleMaterialInput.Displayed;
        }

        public bool IsPoleColorCodeInputDisplayed()
        {
            return poleColorCodeInput.Displayed;
        }

        public bool IsPoleStatusInputDisplayed()
        {
            return poleStatusInput.Displayed;
        }

        public bool IsTypeGroundFixingInputDisplayed()
        {
            return typeGroundFixingInput.Displayed;
        }

        public bool IsPoleInstallDateInputDisplayed()
        {
            return poleInstallDateInput.Displayed;
        }

        public bool IsCommentInputDisplayed()
        {
            return commentInput.Displayed;
        }

        public bool IsEnergySupplierDropDownDisplayed()
        {
            return energySupplierDropDown.Displayed;
        }

        public bool IsNetworkTypeInputDisplayed()
        {
            return networkTypeInput.Displayed;
        }

        public bool IsSegmentNumericInputDisplayed()
        {
            return segmentNumericInput.Displayed;
        }

        public bool IsSectionInputDisplayed()
        {
            return sectionInput.Displayed;
        }

        public bool IsHighVoltageThresholdNumericInputDisplayed()
        {
            return highVoltageThresholdNumericInput.Displayed;
        }

        public bool IsLowVoltageThresholdNumericInputDisplayed()
        {
            return lowVoltageThresholdNumericInput.Displayed;
        }

        /// <summary>
        /// Click Cancel a Device
        /// </summary>
        public void ClickCancelDevice(string name)
        {
            var deviceList = Driver.FindElements(By.CssSelector("[id$='editor-devices'] .equipment-gl-list-item"));
            foreach (var device in deviceList)
            {
                var label = device.FindElement(By.CssSelector(".equipment-gl-list-item-label"));
                var labelText = label.Text.Trim();
                if(labelText.Equals(name))
                {
                    device.MoveTo();
                    var cancelButton = device.FindElement(By.CssSelector(".icon-cancel.equipment-gl-item-button"));
                    Wait.ForElementDisplayed(cancelButton);
                    cancelButton.ClickEx();
                    Wait.ForElementInvisible(By.CssSelector(".icon-cancel.equipment-gl-item-button"));
                    return;
                }
            }
        }

        /// <summary>
        /// Click Remove a Device
        /// </summary>
        public void ClickRemoveDevice(string name)
        {
            var deviceList = Driver.FindElements(By.CssSelector("[id$='editor-devices'] .equipment-gl-list-item"));
            foreach (var device in deviceList)
            {
                var label = device.FindElement(By.CssSelector(".equipment-gl-list-item-label"));
                var labelText = label.Text.Trim();
                if (labelText.Equals(name))
                {
                    device.MoveTo();
                    var removeButton = device.FindElement(By.CssSelector(".icon-delete.equipment-gl-item-button"));
                    Wait.ForElementDisplayed(removeButton);
                    removeButton.ClickEx();
                    return;
                }
            }
        }

        public string SelectRandomInstallStatusDropDown()
        {
            var currentValue = GetLampTypeValue();
            var listItems = installStatusDropDown.GetAllItems();
            listItems.Remove(currentValue);
            var value = listItems.PickRandom();
            installStatusDropDown.Select(value);

            return value;
        }

        public string SelectRandomLampTypeDropDown()
        {
            var currentValue = GetLampTypeValue();
            var listItems = lampTypeDropDown.GetAllItems();
            listItems.Remove(currentValue);
            var value = listItems.PickRandom();
            lampTypeDropDown.Select(value);

            return value;
        }

        #endregion //Business methods   

        public override void WaitForPanelLoaded()
        {
            Wait.ForElementText(panelTitle);
        }
    }
}
