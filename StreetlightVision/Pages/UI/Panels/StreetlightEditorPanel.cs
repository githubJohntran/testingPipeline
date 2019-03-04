using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using StreetlightVision.Extensions;
using StreetlightVision.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StreetlightVision.Pages.UI
{
    public class StreetlightEditorPanel : DeviceEditorPanel
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

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-luminaire-cabinet-name'] .slv-label.editor-label")]
        private IWebElement cabinetControllerLabel;

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

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-DimmingGroupName-field']")]
        private IWebElement dimmingGroupDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-luminaire-cabinet-name-field']")]
        private IWebElement cabinetControllerDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-MacAddress-field']")]
        private IWebElement uniqueAddressInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-MacAddress'] .editor-button.icon-barcode")]
        private IWebElement uniqueAddressScanButton;

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

        #region Dynamic Lighting group

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-DynamicControl-active'] .slv-label.editor-label")]
        private IWebElement activateLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-DynamicControl-startdelay'] .slv-label.editor-label")]
        private IWebElement startDelayLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-DynamicControl-txdelay'] .slv-label.editor-label")]
        private IWebElement transmissionDelayLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-DynamicControl-rampuptime'] .slv-label.editor-label")]
        private IWebElement lowToHighDelayLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-DynamicControl-lightcommand'] .slv-label.editor-label")]
        private IWebElement highLightLevelLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-DynamicControl-uptime'] .slv-label.editor-label")]
        private IWebElement maintainDelayLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-DynamicControl-dimdowntime'] .slv-label.editor-label")]
        private IWebElement highToLowDelayLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-DynamicControl-edge'] .slv-label.editor-label")]
        private IWebElement edgeLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-DynamicControl-edgemode'] .slv-label.editor-label")]
        private IWebElement edgeModeLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-DynamicControl-destination'] .slv-label.editor-label")]
        private IWebElement sensorGroupLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-DynamicControl-sensortype'] .slv-label.editor-label")]
        private IWebElement sensorTypeLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-DynamicControl-sensoractivetime'] .slv-label.editor-label")]
        private IWebElement sensorActivePeriodLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-DynamicControl-active-field']")]
        private IWebElement activateCheckbox;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-DynamicControl-startdelay-field']")]
        private IWebElement startDelayNumericInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-DynamicControl-txdelay-field']")]
        private IWebElement transmissionDelayNumericInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-DynamicControl-rampuptime-field']")]
        private IWebElement lowToHighDelayNumericInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-DynamicControl-lightcommand-field']")]
        private IWebElement highLightLevelNumericInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-DynamicControl-uptime-field']")]
        private IWebElement maintainDelayNumericInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-DynamicControl-dimdowntime-field']")]
        private IWebElement highToLowDelayNumericInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-DynamicControl-edge-field']")]
        private IWebElement edgeDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-DynamicControl-edgemode-field']")]
        private IWebElement edgeModeDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-DynamicControl-destination-field']")]
        private IWebElement sensorGroupInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-DynamicControl-sensortype-field']")]
        private IWebElement sensorTypeDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-DynamicControl-sensoractivetime-field']")]
        private IWebElement sensorActivePeriodDropDown;

        #endregion //Dynamic Lighting group

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

        #region Lamp group

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-brandId'] .slv-label.editor-label")]
        private IWebElement lampTypeLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-power'] .slv-label.editor-label")]
        private IWebElement lampWattageLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-powerCorrection'] .slv-label.editor-label")]
        private IWebElement fixedSavedPowerLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-lamp-installdate'] .slv-label.editor-label")]
        private IWebElement lampInstallDateLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-PowerFactorThreshold'] .slv-label.editor-label")]
        private IWebElement powerFactorThresholdLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-OnLuxLevel'] .slv-label.editor-label")]
        private IWebElement onLuxLevelLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-OffLuxLevel'] .slv-label.editor-label")]
        private IWebElement offLuxLevelLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-device-photocell-lampfailurethreshold'] .slv-label.editor-label")]
        private IWebElement lampFailureThresholdLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-device-photocell-lampwarmuptime'] .slv-label.editor-label")]
        private IWebElement lampWarmupTimeLabel;

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

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-PowerFactorThreshold-field']")]
        private IWebElement powerFactorThresholdNumericInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-OnLuxLevel-field']")]
        private IWebElement onLuxLevelNumericInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-OffLuxLevel-field']")]
        private IWebElement offLuxLevelNumericInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-device-photocell-lampfailurethreshold-field']")]
        private IWebElement lampFailureThresholdNumericInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-device-photocell-lampwarmuptime-field']")]
        private IWebElement lampWarmupTimeNumericInput;

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

        public StreetlightEditorPanel(IWebDriver driver, PageBase page) : base(driver, page)
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
        /// Select an item of 'CabinetController' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectCabinetControllerDropDown(string value)
        {
            cabinetControllerDropDown.Select(value);
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

        #region Dynamic Lighting group

        /// <summary>
        /// Tick 'Activate' checkbox
        /// </summary>
        /// <param name="value"></param>
        public void TickActivateCheckbox(bool value)
        {
            activateCheckbox.Check(value);
        }

        /// <summary>
        /// Enter a value for 'StartDelay' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterStartDelayNumericInput(string value)
        {
            startDelayNumericInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'TransmissionDelay' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterTransmissionDelayNumericInput(string value)
        {
            transmissionDelayNumericInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'LowToHighDelay' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterLowToHighDelayNumericInput(string value)
        {
            lowToHighDelayNumericInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'HighLightLevel' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterHighLightLevelNumericInput(string value)
        {
            highLightLevelNumericInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'MaintainDelay' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterMaintainDelayNumericInput(string value)
        {
            maintainDelayNumericInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'HighToLowDelay' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterHighToLowDelayNumericInput(string value)
        {
            highToLowDelayNumericInput.Enter(value);
        }

        /// <summary>
        /// Select an item of 'Edge' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectEdgeDropDown(string value)
        {
            edgeDropDown.Select(value);
        }

        /// <summary>
        /// Select an item of 'EdgeMode' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectEdgeModeDropDown(string value)
        {
            edgeModeDropDown.Select(value);
        }

        /// <summary>
        /// Enter a value for 'SensorGroup' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterSensorGroupInput(string value)
        {
            sensorGroupInput.Enter(value);
        }

        /// <summary>
        /// Select an item of 'SensorType' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectSensorTypeDropDown(string value)
        {
            sensorTypeDropDown.Select(value);
        }

        /// <summary>
        /// Select an item of 'SensorActivePeriod' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectSensorActivePeriodDropDown(string value)
        {
            sensorActivePeriodDropDown.Select(value);
        }

        #endregion //Dynamic Lighting group

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
        /// Enter a value for 'LampInstallDate' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterLampInstallDateInput(string value)
        {
            lampInstallDateInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'PowerFactorThreshold' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterPowerFactorThresholdNumericInput(string value)
        {
            powerFactorThresholdNumericInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'OnLuxLevel' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterOnLuxLevelNumericInput(string value)
        {
            onLuxLevelNumericInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'OffLuxLevel' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterOffLuxLevelNumericInput(string value)
        {
            offLuxLevelNumericInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'LampFailureThreshold' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterLampFailureThresholdNumericInput(string value)
        {
            lampFailureThresholdNumericInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'LampWarmupTime' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterLampWarmupTimeNumericInput(string value)
        {
            lampWarmupTimeNumericInput.Enter(value);
        }

        #endregion //Lamp group

        #region Driver or ballast group

        /// <summary>
        /// Enter an item of 'BallastType' input 
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
        /// Enter a value for 'LuminaireInstallDate' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterLuminaireInstallDateInput(string value)
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
        /// Enter a value for 'PoleInstallDate' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterPoleInstallDateInput(string value)
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
        /// Get 'DimmingGroup' label text
        /// </summary>
        /// <returns></returns>
        public string GetDimmingGroupText()
        {
            return dimmingGroupLabel.Text;
        }

        /// <summary>
        /// Get 'CabinetController' label text
        /// </summary>
        /// <returns></returns>
        public string GetCabinetControllerText()
        {
            return cabinetControllerLabel.Text;
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
        /// Get 'DimmingGroup' input value
        /// </summary>
        /// <returns></returns>
        public string GetDimmingGroupValue()
        {
            return dimmingGroupDropDown.Text;
        }

        /// <summary>
        /// Get 'CabinetController' input value
        /// </summary>
        /// <returns></returns>
        public string GetCabinetControllerValue()
        {
            return cabinetControllerDropDown.Text;
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

        #region Dynamic Lighting group

        /// <summary>
        /// Get 'Activate' label text
        /// </summary>
        /// <returns></returns>
        public string GetActivateText()
        {
            return activateLabel.Text;
        }

        /// <summary>
        /// Get 'StartDelay' label text
        /// </summary>
        /// <returns></returns>
        public string GetStartDelayText()
        {
            return startDelayLabel.Text;
        }

        /// <summary>
        /// Get 'TransmissionDelay' label text
        /// </summary>
        /// <returns></returns>
        public string GetTransmissionDelayText()
        {
            return transmissionDelayLabel.Text;
        }

        /// <summary>
        /// Get 'LowToHighDelay' label text
        /// </summary>
        /// <returns></returns>
        public string GetLowToHighDelayText()
        {
            return lowToHighDelayLabel.Text;
        }

        /// <summary>
        /// Get 'HighLightLevel' label text
        /// </summary>
        /// <returns></returns>
        public string GetHighLightLevelText()
        {
            return highLightLevelLabel.Text;
        }

        /// <summary>
        /// Get 'MaintainDelay' label text
        /// </summary>
        /// <returns></returns>
        public string GetMaintainDelayText()
        {
            return maintainDelayLabel.Text;
        }

        /// <summary>
        /// Get 'HighToLowDelay' label text
        /// </summary>
        /// <returns></returns>
        public string GetHighToLowDelayText()
        {
            return highToLowDelayLabel.Text;
        }

        /// <summary>
        /// Get 'Edge' label text
        /// </summary>
        /// <returns></returns>
        public string GetEdgeText()
        {
            return edgeLabel.Text;
        }

        /// <summary>
        /// Get 'EdgeMode' label text
        /// </summary>
        /// <returns></returns>
        public string GetEdgeModeText()
        {
            return edgeModeLabel.Text;
        }

        /// <summary>
        /// Get 'SensorGroup' label text
        /// </summary>
        /// <returns></returns>
        public string GetSensorGroupText()
        {
            return sensorGroupLabel.Text;
        }

        /// <summary>
        /// Get 'SensorType' label text
        /// </summary>
        /// <returns></returns>
        public string GetSensorTypeText()
        {
            return sensorTypeLabel.Text;
        }

        /// <summary>
        /// Get 'SensorActivePeriod' label text
        /// </summary>
        /// <returns></returns>
        public string GetSensorActivePeriodText()
        {
            return sensorActivePeriodLabel.Text;
        }

        /// <summary>
        /// Get 'Activate' input value
        /// </summary>
        /// <returns></returns>
        public bool GetActivateValue()
        {
            return activateCheckbox.CheckboxValue();
        }

        /// <summary>
        /// Get 'StartDelay' input value
        /// </summary>
        /// <returns></returns>
        public string GetStartDelayValue()
        {
            return startDelayNumericInput.Value();
        }

        /// <summary>
        /// Get 'TransmissionDelay' input value
        /// </summary>
        /// <returns></returns>
        public string GetTransmissionDelayValue()
        {
            return transmissionDelayNumericInput.Value();
        }

        /// <summary>
        /// Get 'LowToHighDelay' input value
        /// </summary>
        /// <returns></returns>
        public string GetLowToHighDelayValue()
        {
            return lowToHighDelayNumericInput.Value();
        }

        /// <summary>
        /// Get 'HighLightLevel' input value
        /// </summary>
        /// <returns></returns>
        public string GetHighLightLevelValue()
        {
            return highLightLevelNumericInput.Value();
        }

        /// <summary>
        /// Get 'MaintainDelay' input value
        /// </summary>
        /// <returns></returns>
        public string GetMaintainDelayValue()
        {
            return maintainDelayNumericInput.Value();
        }

        /// <summary>
        /// Get 'HighToLowDelay' input value
        /// </summary>
        /// <returns></returns>
        public string GetHighToLowDelayValue()
        {
            return highToLowDelayNumericInput.Value();
        }

        /// <summary>
        /// Get 'Edge' input value
        /// </summary>
        /// <returns></returns>
        public string GetEdgeValue()
        {
            return edgeDropDown.Text;
        }

        /// <summary>
        /// Get 'EdgeMode' input value
        /// </summary>
        /// <returns></returns>
        public string GetEdgeModeValue()
        {
            return edgeModeDropDown.Text;
        }

        /// <summary>
        /// Get 'SensorGroup' input value
        /// </summary>
        /// <returns></returns>
        public string GetSensorGroupValue()
        {
            return sensorGroupInput.Value();
        }

        /// <summary>
        /// Get 'SensorType' input value
        /// </summary>
        /// <returns></returns>
        public string GetSensorTypeValue()
        {
            return sensorTypeDropDown.Text;
        }

        /// <summary>
        /// Get 'SensorActivePeriod' input value
        /// </summary>
        /// <returns></returns>
        public string GetSensorActivePeriodValue()
        {
            return sensorActivePeriodDropDown.Text;
        }

        #endregion //Dynamic Lighting group

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
        /// Get 'PowerFactorThreshold' label text
        /// </summary>
        /// <returns></returns>
        public string GetPowerFactorThresholdText()
        {
            return powerFactorThresholdLabel.Text;
        }

        /// <summary>
        /// Get 'OnLuxLevel' label text
        /// </summary>
        /// <returns></returns>
        public string GetOnLuxLevelText()
        {
            return onLuxLevelLabel.Text;
        }

        /// <summary>
        /// Get 'OffLuxLevel' label text
        /// </summary>
        /// <returns></returns>
        public string GetOffLuxLevelText()
        {
            return offLuxLevelLabel.Text;
        }

        /// <summary>
        /// Get 'LampFailureThreshold' label text
        /// </summary>
        /// <returns></returns>
        public string GetLampFailureThresholdText()
        {
            return lampFailureThresholdLabel.Text;
        }

        /// <summary>
        /// Get 'LampWarmupTime' label text
        /// </summary>
        /// <returns></returns>
        public string GetLampWarmupTimeText()
        {
            return lampWarmupTimeLabel.Text;
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

        /// <summary>
        /// Get 'LampInstallDate' input value
        /// </summary>
        /// <returns></returns>
        public string GetLampInstallDateValue()
        {
            return lampInstallDateInput.Value();
        }

        /// <summary>
        /// Get 'PowerFactorThreshold' input value
        /// </summary>
        /// <returns></returns>
        public string GetPowerFactorThresholdValue()
        {
            return powerFactorThresholdNumericInput.Value();
        }

        /// <summary>
        /// Get 'OnLuxLevel' input value
        /// </summary>
        /// <returns></returns>
        public string GetOnLuxLevelValue()
        {
            return onLuxLevelNumericInput.Value();
        }

        /// <summary>
        /// Get 'OffLuxLevel' input value
        /// </summary>
        /// <returns></returns>
        public string GetOffLuxLevelValue()
        {
            return offLuxLevelNumericInput.Value();
        }

        /// <summary>
        /// Get 'LampFailureThreshold' input value
        /// </summary>
        /// <returns></returns>
        public string GetLampFailureThresholdValue()
        {
            return lampFailureThresholdNumericInput.Value();
        }

        /// <summary>
        /// Get 'LampWarmupTime' input value
        /// </summary>
        /// <returns></returns>
        public string GetLampWarmupTimeValue()
        {
            return lampWarmupTimeNumericInput.Value();
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
        /// Get 'DimmingInterface' input value
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
        /// Get 'LuminaireType' input value
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
        /// Get 'Orientation' input value
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
        /// Get 'LuminaireInstallDate' input value
        /// </summary>
        /// <returns></returns>
        public string GetLuminaireInstallDateValue()
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
        /// Get 'PoleInstallDate' input value
        /// </summary>
        /// <returns></returns>
        public string GetPoleInstallDateValue()
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

        #region Is

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

        public bool IsCabinetControllerDropDownDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-property-luminaire-cabinet-name-field']"));
        }

        public bool IsUniqueAddressInputDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-property-MacAddress-field']"));
        }

        public bool IsInstallStatusDropDownDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-property-installStatus-field']"));
        }

        public bool IsConfigurationStatusInputDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-property-ConfigStatus-field']"));
        }

        public bool IsConfigStatusMsgInputDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-property-ConfigStatusMessage-field']"));
        }

        public bool IsCommunicationStatusInputDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-property-CommunicationStatus-field']"));
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

        public bool IsReferenceInputDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-property-reference-field']"));
        }

        public bool IsElexonChargeCodeInputDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-property-ElexonChargeCode-field']"));
        }

        public bool IsDefaultLightLevelInputDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-property-defaultLightState-field']"));
        }

        public bool IsTimeoutNumericInputDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-property-timeout-field']"));
        }

        public bool IsRetriesNumericInputDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-property-retries-field']"));
        }

        public bool IsLastEventLogSequenceInputDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-property-ssn-logsequence-event-last-field']"));
        }

        public bool IsLastEventRequestTimeInputDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-property-ssn-requesttime-event-last-field']"));
        }

        public bool IsLastMeterLogSequenceInputDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-property-ssn-logsequence-meter-last-field']"));
        }

        public bool IsLastMeterRequestTimeInputDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-property-ssn-requesttime-meter-last-field']"));
        }

        public bool IsIsCpdCheckboxDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-property-ssn-is-cpd-field']"));
        }

        public bool IsLastImuLogSequenceInputDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-property-ssn-logsequence-imu-last-field']"));
        }

        public bool IsLastImuRequestTimeInputDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-property-ssn-requesttime-imu-last-field']"));
        }

        public bool IsLampFailureThresholdNumericInputDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-property-device-photocell-lampfailurethreshold-field']"));
        }

        public bool IsLampWarmupTimeNumericInputDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-property-device-photocell-lampwarmuptime-field']"));
        }

        public bool IsLampFailureThresholdExisted()
        {
            return Driver.FindElements(By.CssSelector("[id$='editor-property-device-photocell-lampfailurethreshold']")).Any();
        }

        public bool IsLampWarmupTimeExisted()
        {
            return Driver.FindElements(By.CssSelector("[id$='editor-property-device-photocell-lampwarmuptime']")).Any();
        }

        #endregion //Is

        public List<string> GetListOfEquipmentTypes()
        {
            return typeOfEquipmentDropDown.GetAllItems();
        }

        /// <summary>
        /// Get all dimming groups items
        /// </summary>
        public List<string> GetListOfDimmingGroups()
        {
            return dimmingGroupDropDown.GetAllItems();
        }

        /// <summary>
        /// Get all cabinet controllers
        /// </summary>
        public List<string> GetListOfCabinetControllers()
        {
            return cabinetControllerDropDown.GetAllItems();
        }

        /// <summary>
        /// Get all lamp type items
        /// </summary>
        public List<string> GetListOfLampTypes()
        {
            return lampTypeDropDown.GetAllItems();
        }

        public List<string> GetListOfEnergySuppliers()
        {
            return energySupplierDropDown.GetAllItems();
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

        public void SelectRandomCabinetControllerDropDown()
        {
            var currentValue = GetDimmingGroupValue();
            var listItems = cabinetControllerDropDown.GetAllItems();
            listItems.Remove(currentValue);
            cabinetControllerDropDown.Select(listItems.PickRandom());
        }

        public void SelectRandomInstallStatusDropDown()
        {
            var currentValue = GetInstallStatusValue();
            var listItems = installStatusDropDown.GetAllItems();
            listItems.Remove(currentValue);
            installStatusDropDown.Select(listItems.PickRandom());
        }

        public void SelectRandomLocationTypeDropDown()
        {
            var currentValue = GetLocationTypeValue();
            var listItems = locationTypeDropDown.GetAllItems();
            listItems.Remove(currentValue);
            locationTypeDropDown.Select(listItems.PickRandom());
        }

        public void SelectRandomLampTypeDropDown()
        {
            var currentValue = GetLampTypeValue();
            var listItems = lampTypeDropDown.GetAllItems().Where(p => p.Contains("Default")).ToList();
            listItems.Remove(currentValue);
            lampTypeDropDown.Select(listItems.PickRandom());
        }

        public void SelectRandomDimmingInterfaceDropDown()
        {
            var currentValue = GetDimmingInterfaceValue();
            var listItems = dimmingInterfaceDropDown.GetAllItems();
            listItems.Remove(currentValue);
            listItems.RemoveAll(p => p.Contains("SlvDemoGroup") || string.IsNullOrEmpty(p));
            dimmingInterfaceDropDown.Select(listItems.PickRandom());
        }

        public void SelectRandomEnergySupplierDropDown()
        {
            var currentValue = GetEnergySupplierValue();
            var listItems = energySupplierDropDown.GetAllItems();
            listItems.Remove(currentValue);
            energySupplierDropDown.Select(listItems.PickRandom());
        }

        public void SelectRandomSupplyVoltageDropDown()
        {
            var currentValue = GetSupplyVoltageValue();
            var listItems = supplyVoltageDropDown.GetAllItems();
            listItems.Remove(currentValue);
            supplyVoltageDropDown.Select(listItems.PickRandom());
        }

        public void SelectRandomNicFallbackModeDropDown()
        {
            var currentValue = GetNicFallbackModeValue();
            var listItems = nicFallbackModeDropDown.GetAllItems();
            listItems.Remove(currentValue);
            nicFallbackModeDropDown.Select(listItems.PickRandom());
        }

        /// <summary>
        /// Clear selected item of 'DimmingGroup' dropdown 
        /// </summary>
        public void ClearDimmingGroupDropDown()
        {
            dimmingGroupDropDown.ClearSelectedItem();
        }

        /// <summary>
        /// Clear selected item of 'CabinetController' dropdown 
        /// </summary>
        public void ClearCabinetControllerDropDown()
        {
            cabinetControllerDropDown.ClearSelectedItem();
        }

        public void ClearUniqueAddressInput()
        {
            uniqueAddressInput.Clear();
        }

        #endregion //Business methods        

        public override void WaitForPanelLoaded()
        {
            Wait.ForElementStyle(By.CssSelector("[id$='equipmentgl-editor']"), string.Format("left: {0}px", WebDriverContext.JsExecutor.ExecuteScript("return screen.availWidth - 350 - 60")));
        }
    }
}
