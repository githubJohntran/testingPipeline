using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using StreetlightVision.Extensions;
using StreetlightVision.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StreetlightVision.Pages.UI
{
    public class CabinetControllerEditorPanel : DeviceEditorPanel
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

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-MacAddress'] .slv-label.editor-label")]
        private IWebElement uniqueAddressLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-install-date'] .slv-label.editor-label")]
        private IWebElement controllerInstallDateLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-installStatus'] .slv-label.editor-label")]
        private IWebElement installStatusLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-device-node-serialnumber'] .slv-label.editor-label")]
        private IWebElement serialNumberLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-device-node-hwversion'] .slv-label.editor-label")]
        private IWebElement deviceHwVersionLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-device-node-swversion'] .slv-label.editor-label")]
        private IWebElement deviceSwVersionLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$=editor-property-device-node-hwType'] .slv-label.editor-label")]
        private IWebElement deviceHwTypeLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-device-nic-serialnumber'] .slv-label.editor-label")]
        private IWebElement nicSerialNumberLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-device-nic-swversion'] .slv-label.editor-label")]
        private IWebElement nicSwVersionLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-device-nic-hwversion'] .slv-label.editor-label")]
        private IWebElement nicHwVersionLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-device-nic-hwModel'] .slv-label.editor-label")]
        private IWebElement nicHwModelLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-device-nic-currentmode'] .slv-label.editor-label")]
        private IWebElement nicCurrentModeLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-device-nic-fallbackmode'] .slv-label.editor-label")]
        private IWebElement nicFallbackModeLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-device-node-manufdate'] .slv-label.editor-label")]
        private IWebElement manufactoringDateLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-device-node-name'] .slv-label.editor-label")]
        private IWebElement deviceNameLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-device-node-manufacturer'] .slv-label.editor-label")]
        private IWebElement deviceManufacturerLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-ConfigStatus'] .slv-label.editor-label")]
        private IWebElement configStatusLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-ConfigStatusMessage'] .slv-label.editor-label")]
        private IWebElement configStatusMessageLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-CommunicationStatus'] .slv-label.editor-label")]
        private IWebElement communicationStatusLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-reference'] .slv-label.editor-label")]
        private IWebElement referenceLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-ElexonChargeCode'] .slv-label.editor-label")]
        private IWebElement elexonChargeCodeLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-device-uiqid'] .slv-label.editor-label")]
        private IWebElement utilityIdLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-device-meter-programid'] .slv-label.editor-label")]
        private IWebElement meterProgramIdLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-device-nic-catalog'] .slv-label.editor-label")]
        private IWebElement catalogNumberLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-defaultLightState'] .slv-label.editor-label")]
        private IWebElement defaultLightLevelLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-timeout'] .slv-label.editor-label")]
        private IWebElement timeoutLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-retries'] .slv-label.editor-label")]
        private IWebElement retriesLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-ssn-logsequence-event-last'] .slv-label.editor-label")]
        private IWebElement lastEventLogSequenceLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-ssn-requesttime-event-last'] .slv-label.editor-label")]
        private IWebElement lastEventRequestTimeLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-ssn-logsequence-meter-last'] .slv-label.editor-label")]
        private IWebElement lastMeterLogSequenceLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-ssn-requesttime-meter-last'] .slv-label.editor-label")]
        private IWebElement lastMeterRequestTimeLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-ssn-is-cpd'] .slv-label.editor-label")]
        private IWebElement isCpdLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-ssn-logsequence-imu-last'] .slv-label.editor-label")]
        private IWebElement lastImuLogSequenceLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-ssn-requesttime-imu-last'] .slv-label.editor-label")]
        private IWebElement lastImuRequestTimeLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-modelFunctionId-field']")]
        private IWebElement typeOfEquipmentDropDown;        

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-MacAddress-field']")]
        private IWebElement uniqueAddressInput;        

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-install-date-field']")]
        private IWebElement controllerInstallDateInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-installStatus-field']")]
        private IWebElement installStatusDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-device-node-serialnumber-field']")]
        private IWebElement serialNumberInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-device-node-hwversion-field']")]
        private IWebElement deviceHwVersionInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-device-node-swversion-field']")]
        private IWebElement deviceSwVersionInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-device-node-hwType-field']")]
        private IWebElement deviceHwTypeInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-device-nic-serialnumber-field']")]
        private IWebElement nicSerialNumberInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-device-nic-swversion-field']")]
        private IWebElement nicSwVersionInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-device-nic-hwversion-field']")]
        private IWebElement nicHwVersionInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-device-nic-hwModel-field']")]
        private IWebElement nicHwModelInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-device-nic-currentmode-field']")]
        private IWebElement nicCurrentModeInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-device-nic-fallbackmode-field']")]
        private IWebElement nicFallbackModeDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-device-node-manufdate-field']")]
        private IWebElement manufactoringDateInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-device-node-name-field']")]
        private IWebElement deviceNameInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-device-node-manufacturer-field']")]
        private IWebElement deviceManufacturerInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-ConfigStatus-field']")]
        private IWebElement configStatusInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-ConfigStatusMessage-field']")]
        private IWebElement configStatusMessageInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-CommunicationStatus-field']")]
        private IWebElement communicationStatusInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-reference-field']")]
        private IWebElement referenceInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-ElexonChargeCode-field']")]
        private IWebElement elexonChargeCodeInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-device-uiqid-field']")]
        private IWebElement utilityIdInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-device-meter-programid-field']")]
        private IWebElement meterProgramIdInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-device-nic-catalog-field']")]
        private IWebElement catalogNumberInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-defaultLightState-field']")]
        private IWebElement defaultLightLevelNumericInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-timeout-field']")]
        private IWebElement timeoutNumericInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-retries-field']")]
        private IWebElement retriesNumericInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-ssn-logsequence-event-last-field']")]
        private IWebElement lastEventLogSequenceInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-ssn-requesttime-event-last-field']")]
        private IWebElement lastEventRequestTimeInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-ssn-logsequence-meter-last-field']")]
        private IWebElement lastMeterLogSequenceInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-ssn-requesttime-meter-last-field']")]
        private IWebElement lastMeterRequestTimeInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-ssn-is-cpd-field']")]
        private IWebElement isCpdCheckbox;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-ssn-logsequence-imu-last-field']")]
        private IWebElement lastImuLogSequenceInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-ssn-requesttime-imu-last-field']")]
        private IWebElement lastImuRequestTimeInput;

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

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-location-utillocationid'] .slv-label.editor-label")]
        private IWebElement utilityLocationIdLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-DevicePictureFilePath'] .slv-label.editor-label")]
        private IWebElement pictureFilePathLabel;

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

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-location-utillocationid-field']")]
        private IWebElement utilityLocationIdInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-DevicePictureFilePath-field']")]
        private IWebElement pictureFilePathInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-DevicePictureFilePath'] .editor-button.icon-camera")]
        private IWebElement takePhotoButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-DevicePictureFilePath'] .editor-photo")]
        private IWebElement pictureImage;

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

        #endregion //Inventory tab

        #region I/O

        #region About the Cabinet Controller digital outputs

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-DimmingGroupName'] .slv-label.editor-label")]
        private IWebElement dimmingGroupLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-DimmingGroupName-field']")]
        private IWebElement dimmingGroupDropDown;

        #endregion //About the Cabinet Controller digital outputs

        #endregion //I/O


        #region Electricity network tab

        #region Network group

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-providerId'] .slv-label.editor-label")]
        private IWebElement energySupplierLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-network-type'] .slv-label.editor-label")]
        private IWebElement networkTypeLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-network-supplyvoltage'] .slv-label.editor-label")]
        private IWebElement supplyVoltageLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-network-cabinet'] .slv-label.editor-label")]
        private IWebElement cabinetLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-network-transformer'] .slv-label.editor-label")]
        private IWebElement transformerLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-network-feedernumber'] .slv-label.editor-label")]
        private IWebElement feederNumberLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-network-switch'] .slv-label.editor-label")]
        private IWebElement switchLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-network-substation'] .slv-label.editor-label")]
        private IWebElement substationLabel;

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

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-network-cabinet-field']")]
        private IWebElement cabinetInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-network-transformer-field']")]
        private IWebElement transformerInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-network-feedernumber-field']")]
        private IWebElement feederNumberInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-network-switch-field']")]
        private IWebElement switchInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-network-substation-field']")]
        private IWebElement substationInput;

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

        public CabinetControllerEditorPanel(IWebDriver driver, PageBase page) : base(driver, page)
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
        /// Enter a value for 'UniqueAddress' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterUniqueAddressInput(string value)
        {
            uniqueAddressInput.Enter(value);
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
        /// Enter a value for 'SerialNumber' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterSerialNumberInput(string value)
        {
            serialNumberInput.Enter(value);
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

        /// <summary>
        /// Enter a value for 'DeviceHwType' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterDeviceHwTypeInput(string value)
        {
            deviceHwTypeInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'NicSerialNumber' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterNicSerialNumberInput(string value)
        {
            nicSerialNumberInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'NicSwVersion' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterNicSwVersionInput(string value)
        {
            nicSwVersionInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'NicHwVersion' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterNicHwVersionInput(string value)
        {
            nicHwVersionInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'NicHwModel' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterNicHwModelInput(string value)
        {
            nicHwModelInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'NicCurrentMode' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterNicCurrentModeInput(string value)
        {
            nicCurrentModeInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'NicFallbackMode' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterNicFallbackModeDropDown(string value)
        {
            nicFallbackModeDropDown.Select(value);
        }

        /// <summary>
        /// Enter a value for 'ManufactoringDate' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterManufactoringDateInput(string value)
        {
            manufactoringDateInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'DeviceName' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterDeviceNameInput(string value)
        {
            deviceNameInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'DeviceManufacturer' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterDeviceManufacturerInput(string value)
        {
            deviceManufacturerInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'ConfigStatus' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterConfigStatusInput(string value)
        {
            configStatusInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'ConfigStatusMessage' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterConfigStatusMessageInput(string value)
        {
            configStatusMessageInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'CommunicationStatus' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterCommunicationStatusInput(string value)
        {
            communicationStatusInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'Reference' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterReferenceInput(string value)
        {
            referenceInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'ElexonChargeCode' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterElexonChargeCodeInput(string value)
        {
            elexonChargeCodeInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'UtilityId' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterUtilityIdInput(string value)
        {
            utilityIdInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'MeterProgramId' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterMeterProgramIdInput(string value)
        {
            meterProgramIdInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'CatalogNumber' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterCatalogNumberInput(string value)
        {
            catalogNumberInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'DefaultLightLevel' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterDefaultLightLevelNumericInput(string value)
        {
            defaultLightLevelNumericInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'Timeout' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterTimeoutNumericInput(string value)
        {
            timeoutNumericInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'Retries' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterRetriesNumericInput(string value)
        {
            retriesNumericInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'LastEventLogSequence' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterLastEventLogSequenceInput(string value)
        {
            lastEventLogSequenceInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'LastEventRequestTime' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterLastEventRequestTimeInput(string value)
        {
            lastEventRequestTimeInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'LastMeterLogSequence' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterLastMeterLogSequenceInput(string value)
        {
            lastMeterLogSequenceInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'LastMeterRequestTime' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterLastMeterRequestTimeInput(string value)
        {
            lastMeterRequestTimeInput.Enter(value);
        }

        /// <summary>
        /// Tick 'IsCpd' checkbox
        /// </summary>
        /// <param name="value"></param>
        public void TickIsCpdCheckbox(bool value)
        {
            isCpdCheckbox.Check(value);
        }

        /// <summary>
        /// Enter a value for 'LastImuLogSequence' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterLastImuLogSequenceInput(string value)
        {
            lastImuLogSequenceInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'LastImuRequestTime' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterLastImuRequestTimeInput(string value)
        {
            lastImuRequestTimeInput.Enter(value);
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

        /// <summary>
        /// Enter a value for 'UtilityLocationId' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterUtilityLocationIdInput(string value)
        {
            utilityLocationIdInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'PictureFilePath' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterPictureFilePathInput(string value)
        {
            pictureFilePathInput.Enter(value);
        }

        /// <summary>
        /// Click 'TakePhoto' button
        /// </summary>
        public void ClickTakePhotoButton()
        {
            takePhotoButton.ClickEx();
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

        #endregion //Inventory tab

        #region I/O

        #region About the Cabinet Controller digital outputs

        /// <summary>
        /// Select an item of 'DimmingGroup' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectDimmingGroupDropDown(string value)
        {
            dimmingGroupDropDown.Select(value);
        }

        #endregion //About the Cabinet Controller digital outputs

        #endregion //I/O

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
        /// Enter a value for 'Cabinet' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterCabinetInput(string value)
        {
            cabinetInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'Transformer' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterTransformerInput(string value)
        {
            transformerInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'FeederNumber' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterFeederNumberInput(string value)
        {
            feederNumberInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'Switch' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterSwitchInput(string value)
        {
            switchInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'Substation' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterSubstationInput(string value)
        {
            substationInput.Enter(value);
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
        /// Get 'SerialNumber' label text
        /// </summary>
        /// <returns></returns>
        public string GetSerialNumberText()
        {
            return serialNumberLabel.Text;
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
        /// Get 'DeviceHwType' label text
        /// </summary>
        /// <returns></returns>
        public string GetDeviceHwTypeText()
        {
            return deviceHwTypeLabel.Text;
        }

        /// <summary>
        /// Get 'NicSerialNumber' label text
        /// </summary>
        /// <returns></returns>
        public string GetNicSerialNumberText()
        {
            return nicSerialNumberLabel.Text;
        }

        /// <summary>
        /// Get 'NicSwVersion' label text
        /// </summary>
        /// <returns></returns>
        public string GetNicSwVersionText()
        {
            return nicSwVersionLabel.Text;
        }

        /// <summary>
        /// Get 'NicHwVersion' label text
        /// </summary>
        /// <returns></returns>
        public string GetNicHwVersionText()
        {
            return nicHwVersionLabel.Text;
        }

        /// <summary>
        /// Get 'NicHwModel' label text
        /// </summary>
        /// <returns></returns>
        public string GetNicHwModelText()
        {
            return nicHwModelLabel.Text;
        }

        /// <summary>
        /// Get 'NicCurrentMode' label text
        /// </summary>
        /// <returns></returns>
        public string GetNicCurrentModeText()
        {
            return nicCurrentModeLabel.Text;
        }

        /// <summary>
        /// Get 'NicFallbackMode' label text
        /// </summary>
        /// <returns></returns>
        public string GetNicFallbackModeText()
        {
            return nicFallbackModeLabel.Text;
        }

        /// <summary>
        /// Get 'ManufactoringDate' label text
        /// </summary>
        /// <returns></returns>
        public string GetManufactoringDateText()
        {
            return manufactoringDateLabel.Text;
        }

        /// <summary>
        /// Get 'DeviceName' label text
        /// </summary>
        /// <returns></returns>
        public string GetDeviceNameText()
        {
            return deviceNameLabel.Text;
        }

        /// <summary>
        /// Get 'DeviceManufacturer' label text
        /// </summary>
        /// <returns></returns>
        public string GetDeviceManufacturerText()
        {
            return deviceManufacturerLabel.Text;
        }

        /// <summary>
        /// Get 'ConfigStatus' label text
        /// </summary>
        /// <returns></returns>
        public string GetConfigStatusText()
        {
            return configStatusLabel.Text;
        }

        /// <summary>
        /// Get 'ConfigStatusMessage' label text
        /// </summary>
        /// <returns></returns>
        public string GetConfigStatusMessageText()
        {
            return configStatusMessageLabel.Text;
        }

        /// <summary>
        /// Get 'CommunicationStatus' label text
        /// </summary>
        /// <returns></returns>
        public string GetCommunicationStatusText()
        {
            return communicationStatusLabel.Text;
        }

        /// <summary>
        /// Get 'Reference' label text
        /// </summary>
        /// <returns></returns>
        public string GetReferenceText()
        {
            return referenceLabel.Text;
        }

        /// <summary>
        /// Get 'ElexonChargeCode' label text
        /// </summary>
        /// <returns></returns>
        public string GetElexonChargeCodeText()
        {
            return elexonChargeCodeLabel.Text;
        }

        /// <summary>
        /// Get 'UtilityId' label text
        /// </summary>
        /// <returns></returns>
        public string GetUtilityIdText()
        {
            return utilityIdLabel.Text;
        }

        /// <summary>
        /// Get 'MeterProgramId' label text
        /// </summary>
        /// <returns></returns>
        public string GetMeterProgramIdText()
        {
            return meterProgramIdLabel.Text;
        }

        /// <summary>
        /// Get 'CatalogNumber' label text
        /// </summary>
        /// <returns></returns>
        public string GetCatalogNumberText()
        {
            return catalogNumberLabel.Text;
        }

        /// <summary>
        /// Get 'DefaultLightLevel' label text
        /// </summary>
        /// <returns></returns>
        public string GetDefaultLightLevelText()
        {
            return defaultLightLevelLabel.Text;
        }

        /// <summary>
        /// Get 'Timeout' label text
        /// </summary>
        /// <returns></returns>
        public string GetTimeoutText()
        {
            return timeoutLabel.Text;
        }

        /// <summary>
        /// Get 'Retries' label text
        /// </summary>
        /// <returns></returns>
        public string GetRetriesText()
        {
            return retriesLabel.Text;
        }

        /// <summary>
        /// Get 'LastEventLogSequence' label text
        /// </summary>
        /// <returns></returns>
        public string GetLastEventLogSequenceText()
        {
            return lastEventLogSequenceLabel.Text;
        }

        /// <summary>
        /// Get 'LastEventRequestTime' label text
        /// </summary>
        /// <returns></returns>
        public string GetLastEventRequestTimeText()
        {
            return lastEventRequestTimeLabel.Text;
        }

        /// <summary>
        /// Get 'LastMeterLogSequence' label text
        /// </summary>
        /// <returns></returns>
        public string GetLastMeterLogSequenceText()
        {
            return lastMeterLogSequenceLabel.Text;
        }

        /// <summary>
        /// Get 'LastMeterRequestTime' label text
        /// </summary>
        /// <returns></returns>
        public string GetLastMeterRequestTimeText()
        {
            return lastMeterRequestTimeLabel.Text;
        }

        /// <summary>
        /// Get 'IsCpd' label text
        /// </summary>
        /// <returns></returns>
        public string GetIsCpdText()
        {
            return isCpdLabel.Text;
        }

        /// <summary>
        /// Get 'LastImuLogSequence' label text
        /// </summary>
        /// <returns></returns>
        public string GetLastImuLogSequenceText()
        {
            return lastImuLogSequenceLabel.Text;
        }

        /// <summary>
        /// Get 'LastImuRequestTime' label text
        /// </summary>
        /// <returns></returns>
        public string GetLastImuRequestTimeText()
        {
            return lastImuRequestTimeLabel.Text;
        }

        /// <summary>
        /// Get 'TypeOfEquipment' input value
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

        /// <summary>
        /// Get 'ControllerInstallDate' input value
        /// </summary>
        /// <returns></returns>
        public string GetControllerInstallDateValue()
        {
            return controllerInstallDateInput.Value();
        }

        /// <summary>
        /// Get 'InstallStatus' input value
        /// </summary>
        /// <returns></returns>
        public string GetInstallStatusValue()
        {
            return installStatusDropDown.Text;
        }

        /// <summary>
        /// Get 'SerialNumber' input value
        /// </summary>
        /// <returns></returns>
        public string GetSerialNumberValue()
        {
            return serialNumberInput.Value();
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

        /// <summary>
        /// Get 'DeviceHwType' input value
        /// </summary>
        /// <returns></returns>
        public string GetDeviceHwTypeValue()
        {
            return deviceHwTypeInput.Value();
        }

        /// <summary>
        /// Get 'NicSerialNumber' input value
        /// </summary>
        /// <returns></returns>
        public string GetNicSerialNumberValue()
        {
            return nicSerialNumberInput.Value();
        }

        /// <summary>
        /// Get 'NicSwVersion' input value
        /// </summary>
        /// <returns></returns>
        public string GetNicSwVersionValue()
        {
            return nicSwVersionInput.Value();
        }

        /// <summary>
        /// Get 'NicHwVersion' input value
        /// </summary>
        /// <returns></returns>
        public string GetNicHwVersionValue()
        {
            return nicHwVersionInput.Value();
        }

        /// <summary>
        /// Get 'NicHwModel' input value
        /// </summary>
        /// <returns></returns>
        public string GetNicHwModelValue()
        {
            return nicHwModelInput.Value();
        }

        /// <summary>
        /// Get 'NicCurrentMode' input value
        /// </summary>
        /// <returns></returns>
        public string GetNicCurrentModeValue()
        {
            return nicCurrentModeInput.Value();
        }

        /// <summary>
        /// Get 'NicFallbackMode' input value
        /// </summary>
        /// <returns></returns>
        public string GetNicFallbackModeValue()
        {
            return nicFallbackModeDropDown.Text;
        }

        /// <summary>
        /// Get 'ManufactoringDate' input value
        /// </summary>
        /// <returns></returns>
        public string GetManufactoringDateValue()
        {
            return manufactoringDateInput.Value();
        }

        /// <summary>
        /// Get 'DeviceName' input value
        /// </summary>
        /// <returns></returns>
        public string GetDeviceNameValue()
        {
            return deviceNameInput.Value();
        }

        /// <summary>
        /// Get 'DeviceManufactorer' input value
        /// </summary>
        /// <returns></returns>
        public string GetDeviceManufacturerValue()
        {
            return deviceManufacturerInput.Value();
        }

        /// <summary>
        /// Get 'ConfigStatus' input value
        /// </summary>
        /// <returns></returns>
        public string GetConfigStatusValue()
        {
            return configStatusInput.Value();
        }

        /// <summary>
        /// Get 'ConfigStatusMessage' input value
        /// </summary>
        /// <returns></returns>
        public string GetConfigStatusMessageValue()
        {
            return configStatusMessageInput.Value();
        }

        /// <summary>
        /// Get 'CommunicationStatus' input value
        /// </summary>
        /// <returns></returns>
        public string GetCommunicationStatusValue()
        {
            return communicationStatusInput.Value();
        }

        /// <summary>
        /// Get 'Reference' input value
        /// </summary>
        /// <returns></returns>
        public string GetReferenceValue()
        {
            return referenceInput.Value();
        }

        /// <summary>
        /// Get 'ElexonChargeCode' input value
        /// </summary>
        /// <returns></returns>
        public string GetElexonChargeCodeValue()
        {
            return elexonChargeCodeInput.Value();
        }

        /// <summary>
        /// Get 'UtilityId' input value
        /// </summary>
        /// <returns></returns>
        public string GetUtilityIdValue()
        {
            return utilityIdInput.Value();
        }

        /// <summary>
        /// Get 'MeterProgramId' input value
        /// </summary>
        /// <returns></returns>
        public string GetMeterProgramIdValue()
        {
            return meterProgramIdInput.Value();
        }

        /// <summary>
        /// Get 'CatalogNumber' input value
        /// </summary>
        /// <returns></returns>
        public string GetCatalogNumberValue()
        {
            return catalogNumberInput.Value();
        }

        /// <summary>
        /// Get 'DefaultLightLevel' input value
        /// </summary>
        /// <returns></returns>
        public string GetDefaultLightLevelValue()
        {
            return defaultLightLevelNumericInput.Value();
        }

        /// <summary>
        /// Get 'Timeout' input value
        /// </summary>
        /// <returns></returns>
        public string GetTimeoutValue()
        {
            return timeoutNumericInput.Value();
        }

        /// <summary>
        /// Get 'Retries' input value
        /// </summary>
        /// <returns></returns>
        public string GetRetriesValue()
        {
            return retriesNumericInput.Value();
        }

        /// <summary>
        /// Get 'LastEventLogSequence' input value
        /// </summary>
        /// <returns></returns>
        public string GetLastEventLogSequenceValue()
        {
            return lastEventLogSequenceInput.Value();
        }

        /// <summary>
        /// Get 'LastEventRequestTime' input value
        /// </summary>
        /// <returns></returns>
        public string GetLastEventRequestTimeValue()
        {
            return lastEventRequestTimeInput.Value();
        }

        /// <summary>
        /// Get 'LastMeterLogSequence' input value
        /// </summary>
        /// <returns></returns>
        public string GetLastMeterLogSequenceValue()
        {
            return lastMeterLogSequenceInput.Value();
        }

        /// <summary>
        /// Get 'LastMeterRequestTime' input value
        /// </summary>
        /// <returns></returns>
        public string GetLastMeterRequestTimeValue()
        {
            return lastMeterRequestTimeInput.Value();
        }

        /// <summary>
        /// Get 'IsCpd' input value
        /// </summary>
        /// <returns></returns>
        public bool GetIsCpdValue()
        {
            return isCpdCheckbox.CheckboxValue();
        }

        /// <summary>
        /// Get 'LastImuLogSequence' input value
        /// </summary>
        /// <returns></returns>
        public string GetLastImuLogSequenceValue()
        {
            return lastImuLogSequenceInput.Value();
        }

        /// <summary>
        /// Get 'LastImuRequestTime' input value
        /// </summary>
        /// <returns></returns>
        public string GetLastImuRequestTimeValue()
        {
            return lastImuRequestTimeInput.Value();
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
        /// Get 'UtilityLocationId' label text
        /// </summary>
        /// <returns></returns>
        public string GetUtilityLocationIdText()
        {
            return utilityLocationIdLabel.Text;
        }

        /// <summary>
        /// Get 'PictureFilePath' label text
        /// </summary>
        /// <returns></returns>
        public string GetPictureFilePathText()
        {
            return pictureFilePathLabel.Text;
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
        /// Get 'LocationType' input value
        /// </summary>
        /// <returns></returns>
        public string GetLocationTypeValue()
        {
            return locationTypeDropDown.Text;
        }

        /// <summary>
        /// Get 'UtilityLocationId' input value
        /// </summary>
        /// <returns></returns>
        public string GetUtilityLocationIdValue()
        {
            return utilityLocationIdInput.Value();
        }

        /// <summary>
        /// Get 'PictureFilePath' input value
        /// </summary>
        /// <returns></returns>
        public string GetPictureFilePathValue()
        {
            return pictureFilePathInput.Value();
        }

        /// <summary>
        /// Get 'PictureImage' input value
        /// </summary>
        /// <returns></returns>
        public string GetPictureImageValue()
        {
            return pictureImage.ImageValue();
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

        #endregion //Inventory tab

        #region I/O

        #region About the Cabinet Controller digital outputs

        /// <summary>
        /// Get 'DimmingGroup' label text
        /// </summary>
        /// <returns></returns>
        public string GetDimmingGroupText()
        {
            return dimmingGroupLabel.Text;
        }

        /// <summary>
        /// Get 'DimmingGroup' input value
        /// </summary>
        /// <returns></returns>
        public string GetDimmingGroupValue()
        {
            return dimmingGroupDropDown.Text;
        }

        #endregion //About the Cabinet Controller digital outputs

        #endregion //I/O

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
        /// Get 'Cabinet' label text
        /// </summary>
        /// <returns></returns>
        public string GetCabinetText()
        {
            return cabinetLabel.Text;
        }

        /// <summary>
        /// Get 'Transformer' label text
        /// </summary>
        /// <returns></returns>
        public string GetTransformerText()
        {
            return transformerLabel.Text;
        }

        /// <summary>
        /// Get 'FeederNumber' label text
        /// </summary>
        /// <returns></returns>
        public string GetFeederNumberText()
        {
            return feederNumberLabel.Text;
        }

        /// <summary>
        /// Get 'Switch' label text
        /// </summary>
        /// <returns></returns>
        public string GetSwitchText()
        {
            return switchLabel.Text;
        }

        /// <summary>
        /// Get 'Substation' label text
        /// </summary>
        /// <returns></returns>
        public string GetSubstationText()
        {
            return substationLabel.Text;
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
        /// Get 'Cabinet' input value
        /// </summary>
        /// <returns></returns>
        public string GetCabinetValue()
        {
            return cabinetInput.Value();
        }

        /// <summary>
        /// Get 'Transformer' input value
        /// </summary>
        /// <returns></returns>
        public string GetTransformerValue()
        {
            return transformerInput.Value();
        }

        /// <summary>
        /// Get 'FeederNumber' input value
        /// </summary>
        /// <returns></returns>
        public string GetFeederNumberValue()
        {
            return feederNumberInput.Value();
        }

        /// <summary>
        /// Get 'Switch' input value
        /// </summary>
        /// <returns></returns>
        public string GetSwitchValue()
        {
            return switchInput.Value();
        }

        /// <summary>
        /// Get 'Substation' input value
        /// </summary>
        /// <returns></returns>
        public string GetSubstationValue()
        {
            return substationInput.Value();
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

        public void SelectRandomEnergySupplierDropDown()
        {
            var currentValue = GetEnergySupplierValue();
            var listItems = energySupplierDropDown.GetAllItems();
            listItems.Remove(currentValue);
            energySupplierDropDown.Select(listItems.PickRandom());
        }

        public void SelectRandomSupplierVoltageDropDown()
        {
            var currentValue = GetSupplyVoltageValue();
            var listItems = supplyVoltageDropDown.GetAllItems();
            listItems.Remove(currentValue);
            supplyVoltageDropDown.Select(listItems.PickRandom());
        }

        /// <summary>
        /// Get all equipment types items
        /// </summary>
        /// <returns></returns>
        public List<string> GetListOfEquipmentTypes()
        {
            return typeOfEquipmentDropDown.GetAllItems();
        }

        #endregion //Business methods        

        public override void WaitForPanelLoaded()
        {
            Wait.ForElementStyle(By.CssSelector("[id$='equipmentgl-editor']"), string.Format("left: {0}px", WebDriverContext.JsExecutor.ExecuteScript("return screen.availWidth - 350 - 60")));
        }
    }
}
