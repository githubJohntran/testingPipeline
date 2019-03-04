using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using StreetlightVision.Extensions;
using StreetlightVision.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StreetlightVision.Pages.UI
{
    public class ControllerEditorPanel : DeviceEditorPanel
    {
        #region Variables

        #endregion //Variables

        #region IWebElements    

        #region Identity tab

        #region Identity of the controller group

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-controllerStrId'] .slv-label.editor-label")]
        private IWebElement controllerIdLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-controller-host'] .slv-label.editor-label")]
        private IWebElement gatewayHostNameLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-controller-firmwareVersion'] .slv-label.editor-label")]
        private IWebElement controlTechnologyLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-controller-model'] .slv-label.editor-label")]
        private IWebElement commMediaLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-SIMCardNumber'] .slv-label.editor-label")]
        private IWebElement simcardNumberLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-SIMPhoneNumber'] .slv-label.editor-label")]
        private IWebElement phoneNumberLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-HardwareRevision'] .slv-label.editor-label")]
        private IWebElement hardwareRevisionLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-SoftwareVersion'] .slv-label.editor-label")]
        private IWebElement softwareVersionLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-MacAddress'] .slv-label.editor-label")]
        private IWebElement uniqueAddressLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-XmlTechGetCommandMode'] .slv-label.editor-label")]
        private IWebElement realtimeCommandLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-XmlTechGzipRequestBody'] .slv-label.editor-label")]
        private IWebElement gzipPayloadLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-ControllerCacheMode'] .slv-label.editor-label")]
        private IWebElement controllerCacheModeLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-controllerStrId-field']")]
        private IWebElement controllerIdInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-controller-host-field']")]
        private IWebElement gatewayHostNameInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-controller-firmwareVersion-field']")]
        private IWebElement controlTechnologyDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-controller-model-field']")]
        private IWebElement commMediaDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-SIMCardNumber-field']")]
        private IWebElement simcardNumberInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-SIMPhoneNumber-field']")]
        private IWebElement phoneNumberInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-HardwareRevision-field']")]
        private IWebElement hardwareRevisionInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-SoftwareVersion-field']")]
        private IWebElement softwareVersionInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-MacAddress-field']")]
        private IWebElement uniqueAddressInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-MacAddress'] > button")]
        private IWebElement uniqueAddressScanButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-XmlTechGetCommandMode-field']")]
        private IWebElement realtimeCommandDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-XmlTechGzipRequestBody-field']")]
        private IWebElement gzipPayloadDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-ControllerCacheMode-field']")]
        private IWebElement controllerCacheModeDropDown;

        #endregion //Identity of the controller group

        #region Communication group

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-communicationNetworkId'] .slv-label.editor-label")]
        private IWebElement networkIdLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-serverMsgUrl-webapp'] .slv-label.editor-label")]
        private IWebElement serverWebappUrlLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-controller-auth-username'] .slv-label.editor-label")]
        private IWebElement usernameLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-controller-auth-password'] .slv-label.editor-label")]
        private IWebElement passwordLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-DevicesConfigMode'] .slv-label.editor-label")]
        private IWebElement commissionModeLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-servicePathConfiguration'] .slv-label.editor-label")]
        private IWebElement configPathLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-servicePathRealtime'] .slv-label.editor-label")]
        private IWebElement realtimePathLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-xmltech-group-dim-cde-support'] .slv-label.editor-label")]
        private IWebElement groupDimCommandLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-xmltech-all-dim-cde-support'] .slv-label.editor-label")]
        private IWebElement globalDimCommandLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-xmltech-maxdevicespercontroller'] .slv-label.editor-label")]
        private IWebElement maxDevicesLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-xmltech-consider-streetlight-as-lightpoint'] .slv-label.editor-label")]
        private IWebElement considerStreetLightsLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-xmltech-consider-replace-olc-command-as-update'] .slv-label.editor-label")]
        private IWebElement considerReplaceOlcLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-reportTime'] .slv-label.editor-label")]
        private IWebElement reportTimeLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-reportFrequency'] .slv-label.editor-label")]
        private IWebElement reportFrequencyLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-timeout'] .slv-label.editor-label")]
        private IWebElement timeoutLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-maxConnections'] .slv-label.editor-label")]
        private IWebElement maxConnectionsLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-minWaitBetweenCalls'] .slv-label.editor-label")]
        private IWebElement requestIntervalLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-waitAsDelay'] .slv-label.editor-label")]
        private IWebElement inclLatencyIntervalLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-sc-gateway'] .slv-label.editor-label")]
        private IWebElement gatewayLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-communicationNetworkId-field']")]
        private IWebElement networkIdInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-serverMsgUrl-webapp-field']")]
        private IWebElement serverWebAppUrlInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-controller-auth-username-field']")]
        private IWebElement usernameInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-controller-auth-password-field']")]
        private IWebElement passwordInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-DevicesConfigMode-field']")]
        private IWebElement commissionModeDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-servicePathConfiguration-field']")]
        private IWebElement configPathInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-servicePathRealtime-field']")]
        private IWebElement realtimePathInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-xmltech-group-dim-cde-support-field']")]
        private IWebElement groupDimCommandCheckbox;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-xmltech-all-dim-cde-support-field']")]
        private IWebElement globalDimCommandCheckbox;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-xmltech-maxdevicespercontroller-field']")]
        private IWebElement maxDevicesNumericInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-xmltech-consider-streetlight-as-lightpoint-field']")]
        private IWebElement considerStreetLightsCheckbox;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-xmltech-consider-replace-olc-command-as-update-field']")]
        private IWebElement considerReplaceOlcCheckbox;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-reportTime-field']")]
        private IWebElement reportTimeInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-reportFrequency-field']")]
        private IWebElement reportFrequencyNumericInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-timeout-field']")]
        private IWebElement timeoutNumericInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-maxConnections-field']")]
        private IWebElement maxConnectionsNumericInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-minWaitBetweenCalls-field']")]
        private IWebElement requestIntervalNumericInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-waitAsDelay-field']")]
        private IWebElement inclLatencyIntervalCheckbox;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-sc-gateway-field']")]
        private IWebElement gatewayInput;

        #endregion //Communication group

        #region Control System group

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-ssn-is-cpd'] .slv-label.editor-label")]
        private IWebElement isCpdLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-ssn-library-version'] .slv-label.editor-label")]
        private IWebElement libraryVersionLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-ssn-logsequence-imu-last'] .slv-label.editor-label")]
        private IWebElement lastImuLogSequenceLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-ssn-requesttime-imu-last'] .slv-label.editor-label")]
        private IWebElement lastImuRequestTimeLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-retries'] .slv-label.editor-label")]
        private IWebElement retriesLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-ssn-skip-delete-schedule-when-commissioning-calendars'] .slv-label.editor-label")]
        private IWebElement skipDeleteScheduleLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-ssn-is-cpd-field']")]
        private IWebElement isCpdCheckbox;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-ssn-library-version-field']")]
        private IWebElement libraryVersionInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-ssn-logsequence-imu-last-field']")]
        private IWebElement lastImuLogSequenceInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-ssn-requesttime-imu-last-field']")]
        private IWebElement lastImuRequestTimeInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-retries-field']")]
        private IWebElement retriesNumericInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-ssn-skip-delete-schedule-when-commissioning-calendars-field']")]
        private IWebElement skipDeleteScheduleCheckbox;

        #endregion //Control System group

        #region Auto-commissioning group

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-AutoComm-active'] .slv-label.editor-label")]
        private IWebElement autoCommActiveLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-AutoComm-referenceUnit'] .slv-label.editor-label")]
        private IWebElement referenceUnitAutoCommLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-AutoComm-maxUnits'] .slv-label.editor-label")]
        private IWebElement maxUnitsAutoCommLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-AutoComm-timeout'] .slv-label.editor-label")]
        private IWebElement timeoutAutoCommLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-AutoComm-username'] .slv-label.editor-label")]
        private IWebElement usernameAutoCommLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-AutoComm-password'] .slv-label.editor-label")]
        private IWebElement passwordAutoCommLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-AutoComm-active-field-checkbox']")]
        private IWebElement autoCommActiveCheckbox;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-AutoComm-referenceUnit-field']")]
        private IWebElement referenceUnitAutoCommInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-AutoComm-maxUnits-field']")]
        private IWebElement maxUnitsAutoCommNumericInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-AutoComm-timeout-field']")]
        private IWebElement timeoutAutoCommNumericInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-AutoComm-username-field']")]
        private IWebElement usernameAutoCommInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-AutoComm-password-field']")]
        private IWebElement passwordAutoCommInput;

        #endregion //Auto-commissioning group

        #endregion //Identity tab

        #region Inventory tab

        #region Location group

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

        #endregion //Location group

        #region About this cabinet group

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-cabinet-type'] .slv-label.editor-label")]
        private IWebElement typeOfCabinetLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-cabinetwattage'] .slv-label.editor-label")]
        private IWebElement cabinetWattageLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-cabinet-numberofsegment'] .slv-label.editor-label")]
        private IWebElement numberOfSegmentsLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-install-date'] .slv-label.editor-label")]
        private IWebElement controllerInstallDateLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-comment'] .slv-label.editor-label")]
        private IWebElement commentLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-cabinet-type-field']")]
        private IWebElement typeOfCabinetInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-cabinetwattage-field']")]
        private IWebElement cabinetWattageNumericInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-cabinet-numberofsegment-field']")]
        private IWebElement numberOfSegmentsNumericInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-install-date-field']")]
        private IWebElement controllerInstallDateInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-comment-field']")]
        private IWebElement commentInput;

        #endregion //About this cabinet group

        #endregion //Inventory tab

        #region Inputs and outputs tab

        #region About the Segment Controller's digital inputs group

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-DigitalInput1Info'] .slv-label.editor-label")]
        private IWebElement input1Label;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-DigitalInput2Info'] .slv-label.editor-label")]
        private IWebElement input2Label;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-InputModbus1Info'] .slv-label.editor-label")]
        private IWebElement digitalModbus1Label;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-InputModbus2Info'] .slv-label.editor-label")]
        private IWebElement digitalModbus2Label;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-InputModbus3Info'] .slv-label.editor-label")]
        private IWebElement digitalModbus3Label;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-InputModbus4Info'] .slv-label.editor-label")]
        private IWebElement digitalModbus4Label;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-InputModbus5Info'] .slv-label.editor-label")]
        private IWebElement digitalModbus5Label;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-InputModbus6Info'] .slv-label.editor-label")]
        private IWebElement digitalModbus6Label;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-InputModbus7Info'] .slv-label.editor-label")]
        private IWebElement digitalModbus7Label;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-InputModbus8Info'] .slv-label.editor-label")]
        private IWebElement digitalModbus8Label;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-InputModbus9Info'] .slv-label.editor-label")]
        private IWebElement digitalModbus9Label;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-InputModbus10Info'] .slv-label.editor-label")]
        private IWebElement digitalModbus10Label;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-InputModbus11Info'] .slv-label.editor-label")]
        private IWebElement digitalModbus11Label;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-InputModbus12Info'] .slv-label.editor-label")]
        private IWebElement digitalModbus12Label;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-DigitalInput1Info-field']")]
        private IWebElement input1Input;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-DigitalInput2Info-field']")]
        private IWebElement input2Input;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-InputModbus1Info-field']")]
        private IWebElement digitalModbus1Input;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-InputModbus2Info-field']")]
        private IWebElement digitalModbus2Input;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-InputModbus3Info-field']")]
        private IWebElement digitalModbus3Input;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-InputModbus4Info-field']")]
        private IWebElement digitalModbus4Input;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-InputModbus5Info-field']")]
        private IWebElement digitalModbus5Input;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-InputModbus6Info-field']")]
        private IWebElement digitalModbus6Input;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-InputModbus7Info-field']")]
        private IWebElement digitalModbus7Input;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-InputModbus8Info-field']")]
        private IWebElement digitalModbus8Input;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-InputModbus9Info-field']")]
        private IWebElement digitalModbus9Input;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-InputModbus10Info-field']")]
        private IWebElement digitalModbus10Input;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-InputModbus11Info-field']")]
        private IWebElement digitalModbus11Input;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-InputModbus12Info-field']")]
        private IWebElement digitalModbus12Input;

        #endregion //About the Segment Controller's digital inputs group

        #region About the Segment Controller's digital outputs group

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-MainsControlMode'] .slv-label.editor-label")]
        private IWebElement mainsControlModeLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-LightControlMode'] .slv-label.editor-label")]
        private IWebElement lightControlModeLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-DigitalOutput1Info'] .slv-label.editor-label")]
        private IWebElement output1LabelLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-DigitalOutput1Calendar'] .slv-label.editor-label")]
        private IWebElement output1CalendarLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-DigitalOutput2Info'] .slv-label.editor-label")]
        private IWebElement output2LabelLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-DigitalOutput2Calendar'] .slv-label.editor-label")]
        private IWebElement output2CalendarLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-MainsControlMode-field']")]
        private IWebElement mainsControlModeDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-LightControlMode-field']")]
        private IWebElement lightControlModeDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-DigitalOutput1Info-field']")]
        private IWebElement output1LabelInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-DigitalOutput1Calendar-field']")]
        private IWebElement output1CalendarInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-DigitalOutput2Info-field']")]
        private IWebElement output2LabelInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-DigitalOutput2Calendar-field']")]
        private IWebElement output2CalendarInput;

        #endregion //About the Segment Controller's digital outputs group

        #region Modbus Digital Inputs - Failure labels group

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-SegmentFailure1Info'] .slv-label.editor-label")]
        private IWebElement digitalModbus1FailureLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-SegmentFailure2Info'] .slv-label.editor-label")]
        private IWebElement digitalModbus2FailureLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-SegmentFailure3Info'] .slv-label.editor-label")]
        private IWebElement digitalModbus3FailureLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-SegmentFailure4Info'] .slv-label.editor-label")]
        private IWebElement digitalModbus4FailureLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-SegmentFailure5Info'] .slv-label.editor-label")]
        private IWebElement digitalModbus5FailureLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-SegmentFailure6Info'] .slv-label.editor-label")]
        private IWebElement digitalModbus6FailureLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-SegmentFailure7Info'] .slv-label.editor-label")]
        private IWebElement digitalModbus7FailureLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-SegmentFailure8Info'] .slv-label.editor-label")]
        private IWebElement digitalModbus8FailureLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-SegmentFailure9Info'] .slv-label.editor-label")]
        private IWebElement digitalModbus9FailureLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-SegmentFailure10Info'] .slv-label.editor-label")]
        private IWebElement digitalModbus10FailureLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-SegmentFailure11Info'] .slv-label.editor-label")]
        private IWebElement digitalModbus11FailureLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-SegmentFailure12Info'] .slv-label.editor-label")]
        private IWebElement digitalModbus12FailureLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-SegmentFailure1Info-field']")]
        private IWebElement digitalModbus1FailureInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-SegmentFailure2Info-field']")]
        private IWebElement digitalModbus2FailureInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-SegmentFailure3Info-field']")]
        private IWebElement digitalModbus3FailureInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-SegmentFailure4Info-field']")]
        private IWebElement digitalModbus4FailureInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-SegmentFailure5Info-field']")]
        private IWebElement digitalModbus5FailureInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-SegmentFailure6Info-field']")]
        private IWebElement digitalModbus6FailureInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-SegmentFailure7Info-field']")]
        private IWebElement digitalModbus7FailureInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-SegmentFailure8Info-field']")]
        private IWebElement digitalModbus8FailureInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-SegmentFailure9Info-field']")]
        private IWebElement digitalModbus9FailureInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-SegmentFailure10Info-field']")]
        private IWebElement digitalModbus10FailureInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-SegmentFailure11Info-field']")]
        private IWebElement digitalModbus11FailureInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-SegmentFailure12Info-field']")]
        private IWebElement digitalModbus12FailureInput;

        #endregion //Modbus Digital Inputs - Failure labels group

        #region Modbus analog inputs - Labels group

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-AnalogModbus1Info'] .slv-label.editor-label")]
        private IWebElement analogModbus1Label;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-AnalogModbus2Info'] .slv-label.editor-label")]
        private IWebElement analogModbus2Label;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-AnalogModbus3Info'] .slv-label.editor-label")]
        private IWebElement analogModbus3Label;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-AnalogModbus1Info-field']")]
        private IWebElement analogModbus1Input;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-AnalogModbus2Info-field']")]
        private IWebElement analogModbus2Input;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-AnalogModbus3Info-field']")]
        private IWebElement analogModbus3Input;

        #endregion //Modbus analog inputs - Labels group

        #endregion //Inputs and outputs tab

        #region Time tab        

        #region Time management group

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-TimeZoneId'] .slv-label.editor-label")]
        private IWebElement timezoneLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-TimeZoneId-field']")]
        private IWebElement timezoneDropDown;

        #endregion //Time management group        

        #endregion //Time tab

        #region System tab

        #region Resource usage group

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-resource-fpm-smartPoll-usage'] .slv-label.editor-label")]
        private IWebElement smartPollLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-resource-fpm-dataPointServer-usage'] .slv-label.editor-label")]
        private IWebElement slvDatapointServerLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-resource-fpm-smartPoll-usage-field-checkbox']")]
        private IWebElement smartPollCheckbox;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-resource-fpm-dataPointServer-usage-field-checkbox']")]
        private IWebElement slvDatapointServerCheckbox;

        #endregion //Resource usage group

        #endregion //System tab

        #endregion //IWebElements

        #region Constructor

        public ControllerEditorPanel(IWebDriver driver, PageBase page) : base(driver, page)
        {
            PageFactory.InitElements(this, new RetryingElementLocator(driver, TimeSpan.FromMilliseconds(Settings.LocatorRetryingTimeout), TimeSpan.FromMilliseconds(Settings.LocatorPollingInterval)));

            WaitForPanelLoaded();
        }

        #endregion //Constructor

        #region Basic methods        

        #region Actions        

        #region Identity tab

        #region Identity of the controller group

        /// <summary>
        /// Enter a value for 'ControllerId' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterControllerIdInput(string value)
        {
            controllerIdInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'GatewayHostName' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterGatewayHostNameInput(string value)
        {
            gatewayHostNameInput.Enter(value);
        }

        /// <summary>
        /// Select an item of 'ControlTechnology' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectControlTechnologyDropDown(string value)
        {
            controlTechnologyDropDown.Select(value);
        }

        /// <summary>
        /// Select an item of 'CommMedia' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectCommMediaDropDown(string value)
        {
            commMediaDropDown.Select(value);
        }

        /// <summary>
        /// Enter a value for 'SimcardNumber' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterSimcardNumberInput(string value)
        {
            simcardNumberInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'PhoneNumber' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterPhoneNumberInput(string value)
        {
            phoneNumberInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'HardwareRevision' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterHardwareRevisionInput(string value)
        {
            hardwareRevisionInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'SoftwareVersion' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterSoftwareVersionInput(string value)
        {
            softwareVersionInput.Enter(value);
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
        /// Select an item of 'RealtimeCommand' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectRealtimeCommandDropDown(string value)
        {
            realtimeCommandDropDown.Select(value);
        }

        /// <summary>
        /// Select an item of 'GzipPayload' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectGzipPayloadDropDown(string value)
        {
            gzipPayloadDropDown.Select(value);
        }

        /// <summary>
        /// Select an item of 'ControllerCacheMode' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectControllerCacheModeDropDown(string value)
        {
            controllerCacheModeDropDown.Select(value);
        }

        /// <summary>
        /// Click 'uniqueAddressScan' button
        /// </summary>
        public void ClickUniqueAddressScanButton()
        {
            uniqueAddressScanButton.ClickEx();
        }

        #endregion //Identity of the controller group

        #region Communication group

        /// <summary>
        /// Enter a value for 'NetworkId' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterNetworkIdInput(string value)
        {
            networkIdInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'ServerWebappUrl' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterServerWebAppUrlInput(string value)
        {
            serverWebAppUrlInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'Username' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterUsernameInput(string value)
        {
            usernameInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'Password' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterPasswordInput(string value)
        {
            passwordInput.Enter(value);
        }

        /// <summary>
        /// Select an item of 'CommissionMode' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectCommissionModeDropDown(string value)
        {
            commissionModeDropDown.Select(value);
        }

        /// <summary>
        /// Enter a value for 'ConfigPath' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterConfigPathInput(string value)
        {
            configPathInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'RealtimePath' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterRealtimePathInput(string value)
        {
            realtimePathInput.Enter(value);
        }

        /// <summary>
        /// Tick 'GroupDimCommand' checkbox
        /// </summary>
        /// <param name="value"></param>
        public void TickGroupDimCommandCheckbox(bool value)
        {
            groupDimCommandCheckbox.Check(value);
        }

        /// <summary>
        /// Tick 'GlobalDimCommand' checkbox
        /// </summary>
        /// <param name="value"></param>
        public void TickGlobalDimCommandCheckbox(bool value)
        {
            globalDimCommandCheckbox.Check(value);
        }

        /// <summary>
        /// Enter a value for 'MaxDevices' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterMaxDevicesNumericInput(string value)
        {
            maxDevicesNumericInput.Enter(value);
        }

        /// <summary>
        /// Tick 'ConsiderStreetLights' checkbox
        /// </summary>
        /// <param name="value"></param>
        public void TickConsiderStreetLightsCheckbox(bool value)
        {
            considerStreetLightsCheckbox.Check(value);
        }

        /// <summary>
        /// Tick 'ConsiderReplaceOlc' checkbox
        /// </summary>
        /// <param name="value"></param>
        public void TickConsiderReplaceOlcCheckbox(bool value)
        {
            considerReplaceOlcCheckbox.Check(value);
        }

        /// <summary>
        /// Enter a value for 'ReportTime' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterReportTimeInput(string value)
        {
            reportTimeInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'ReportFrequency' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterReportFrequencyNumericInput(string value)
        {
            reportFrequencyNumericInput.Enter(value);
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
        /// Enter a value for 'MaxConnections' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterMaxConnectionsNumericInput(string value)
        {
            maxConnectionsNumericInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'RequestInterval' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterRequestIntervalNumericInput(string value)
        {
            requestIntervalNumericInput.Enter(value);
        }

        /// <summary>
        /// Tick 'InclLatencyInterval' checkbox
        /// </summary>
        /// <param name="value"></param>
        public void TickInclLatencyIntervalCheckbox(bool value)
        {
            inclLatencyIntervalCheckbox.Check(value);
        }

        /// <summary>
        /// Enter a value for 'Gateway' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterGatewayInput(string value)
        {
            gatewayInput.Enter(value);
        }

        #endregion //Communication group

        #region Control System group

        /// <summary>
        /// Tick 'IsCpd' checkbox
        /// </summary>
        /// <param name="value"></param>
        public void TickIsCpdCheckbox(bool value)
        {
            isCpdCheckbox.Check(value);
        }

        /// <summary>
        /// Enter a value for 'LibraryVersion' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterLibraryVersionInput(string value)
        {
            libraryVersionInput.Enter(value);
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

        /// <summary>
        /// Enter a value for 'Retries' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterRetriesNumericInput(string value)
        {
            retriesNumericInput.Enter(value);
        }

        /// <summary>
        /// Tick 'SkipDeleteSchedule' checkbox
        /// </summary>
        /// <param name="value"></param>
        public void TickSkipDeleteScheduleCheckbox(bool value)
        {
            skipDeleteScheduleCheckbox.Check(value);
        }

        #endregion //Control System group

        #region Auto-commissioning group

        /// <summary>
        /// Tick 'AutoCommActive' checkbox
        /// </summary>
        /// <param name="value"></param>
        public void TickAutoCommActiveCheckbox(bool value)
        {
            autoCommActiveCheckbox.Check(value);
        }

        /// <summary>
        /// Enter a value for 'ReferenceUnitAutoComm' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterReferenceUnitAutoCommInput(string value)
        {
            referenceUnitAutoCommInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'MaxUnitsAutoComm' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterMaxUnitsAutoCommNumericInput(string value)
        {
            maxUnitsAutoCommNumericInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'TimeoutAutoComm' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterTimeoutAutoCommNumericInput(string value)
        {
            timeoutAutoCommNumericInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'UsernameAutoComm' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterUsernameAutoCommInput(string value)
        {
            usernameAutoCommInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'PasswordAutoComm' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterPasswordAutoCommInput(string value)
        {
            passwordAutoCommInput.Enter(value);
        }

        #endregion //Auto-commissioning group

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

        #endregion //Location group

        #region About this cabinet group

        /// <summary>
        /// Enter a value for 'TypeOfCabinet' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterTypeOfCabinetInput(string value)
        {
            typeOfCabinetInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'CabinetWattage' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterCabinetWattageNumericInput(string value)
        {
            cabinetWattageNumericInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'NumberOfSegments' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterNumberOfSegmentsNumericInput(string value)
        {
            numberOfSegmentsNumericInput.Enter(value);
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
        /// Enter a value for 'Comment' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterCommentInput(string value)
        {
            commentInput.Enter(value);
        }

        #endregion //About this cabinet group

        #endregion //Inventory tab

        #region Inputs and outputs tab

        #region About the Segment Controller's digital inputs group

        /// <summary>
        /// Enter a value for '1' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterInput1Input(string value)
        {
            input1Input.Enter(value);
        }

        /// <summary>
        /// Enter a value for '2' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterInput2Input(string value)
        {
            input2Input.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'DigitalModbus1' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterDigitalModbus1Input(string value)
        {
            digitalModbus1Input.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'DigitalModbus2' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterDigitalModbus2Input(string value)
        {
            digitalModbus2Input.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'DigitalModbus3' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterDigitalModbus3Input(string value)
        {
            digitalModbus3Input.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'DigitalModbus4' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterDigitalModbus4Input(string value)
        {
            digitalModbus4Input.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'DigitalModbus5' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterDigitalModbus5Input(string value)
        {
            digitalModbus5Input.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'DigitalModbus6' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterDigitalModbus6Input(string value)
        {
            digitalModbus6Input.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'DigitalModbus7' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterDigitalModbus7Input(string value)
        {
            digitalModbus7Input.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'DigitalModbus8' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterDigitalModbus8Input(string value)
        {
            digitalModbus8Input.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'DigitalModbus9' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterDigitalModbus9Input(string value)
        {
            digitalModbus9Input.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'DigitalModbus10' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterDigitalModbus10Input(string value)
        {
            digitalModbus10Input.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'DigitalModbus11' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterDigitalModbus11Input(string value)
        {
            digitalModbus11Input.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'DigitalModbus12' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterDigitalModbus12Input(string value)
        {
            digitalModbus12Input.Enter(value);
        }

        #endregion //About the Segment Controller's digital inputs group

        #region About the Segment Controller's digital outputs group

        /// <summary>
        /// Select an item of 'MainsControlMode' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectMainsControlModeDropDown(string value)
        {
            mainsControlModeDropDown.Select(value);
        }

        /// <summary>
        /// Select an item of 'LightControlMode' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectLightControlModeDropDown(string value)
        {
            lightControlModeDropDown.Select(value);
        }

        /// <summary>
        /// Enter a value for 'Output1Label' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterOutput1LabelInput(string value)
        {
            output1LabelInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'Output1Calendar' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterOutput1CalendarInput(string value)
        {
            output1CalendarInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'Output2Label' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterOutput2LabelInput(string value)
        {
            output2LabelInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'Output2Calendar' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterOutput2CalendarInput(string value)
        {
            output2CalendarInput.Enter(value);
        }

        #endregion //About the Segment Controller's digital outputs group

        #region Modbus Digital Inputs - Failure labels group

        /// <summary>
        /// Enter a value for 'DigitalModbus1Failure' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterDigitalModbus1FailureInput(string value)
        {
            digitalModbus1FailureInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'DigitalModbus2Failure' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterDigitalModbus2FailureInput(string value)
        {
            digitalModbus2FailureInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'DigitalModbus3Failure' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterDigitalModbus3FailureInput(string value)
        {
            digitalModbus3FailureInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'DigitalModbus4Failure' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterDigitalModbus4FailureInput(string value)
        {
            digitalModbus4FailureInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'DigitalModbus5Failure' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterDigitalModbus5FailureInput(string value)
        {
            digitalModbus5FailureInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'DigitalModbus6Failure' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterDigitalModbus6FailureInput(string value)
        {
            digitalModbus6FailureInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'DigitalModbus7Failure' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterDigitalModbus7FailureInput(string value)
        {
            digitalModbus7FailureInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'DigitalModbus8Failure' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterDigitalModbus8FailureInput(string value)
        {
            digitalModbus8FailureInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'DigitalModbus9Failure' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterDigitalModbus9FailureInput(string value)
        {
            digitalModbus9FailureInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'DigitalModbus10Failure' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterDigitalModbus10FailureInput(string value)
        {
            digitalModbus10FailureInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'DigitalModbus11Failure' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterDigitalModbus11FailureInput(string value)
        {
            digitalModbus11FailureInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'DigitalModbus12Failure' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterDigitalModbus12FailureInput(string value)
        {
            digitalModbus12FailureInput.Enter(value);
        }

        #endregion //Modbus Digital Inputs - Failure labels group

        #region Modbus analog inputs - Labels group

        /// <summary>
        /// Enter a value for 'AnalogModbus1' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterAnalogModbus1Input(string value)
        {
            analogModbus1Input.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'AnalogModbus2' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterAnalogModbus2Input(string value)
        {
            analogModbus2Input.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'AnalogModbus3' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterAnalogModbus3Input(string value)
        {
            analogModbus3Input.Enter(value);
        }

        #endregion //Modbus analog inputs - Labels group

        #endregion //Inputs and outputs tab

        #region Time tab        

        #region Time management group

        /// <summary>
        /// Select an item of 'Timezone' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectTimezoneDropDown(string value)
        {
            timezoneDropDown.Select(value);
        }

        #endregion //Time management group        

        #endregion //Time tab

        #region System tab

        #region Resource usage group

        /// <summary>
        /// Tick 'SmartPoll' checkbox
        /// </summary>
        /// <param name="value"></param>
        public void TickSmartPollCheckbox(bool value)
        {
            smartPollCheckbox.Check(value);
        }

        /// <summary>
        /// Tick 'SlvDatapointServer' checkbox
        /// </summary>
        /// <param name="value"></param>
        public void TickSlvDatapointServerCheckbox(bool value)
        {
            slvDatapointServerCheckbox.Check(value);
        }

        #endregion //Resource usage group

        #endregion //System tab

        #endregion //Actions

        #region Get methods        

        #region Identity tab

        #region Identity of the controller group

        /// <summary>
        /// Get 'ControllerId' label text
        /// </summary>
        /// <returns></returns>
        public string GetControllerIdText()
        {
            return controllerIdLabel.Text;
        }

        /// <summary>
        /// Get 'GatewayHostName' label text
        /// </summary>
        /// <returns></returns>
        public string GetGatewayHostNameText()
        {
            return gatewayHostNameLabel.Text;
        }

        /// <summary>
        /// Get 'ControlTechnology' label text
        /// </summary>
        /// <returns></returns>
        public string GetControlTechnologyText()
        {
            return controlTechnologyLabel.Text;
        }

        /// <summary>
        /// Get 'CommMedia' label text
        /// </summary>
        /// <returns></returns>
        public string GetCommMediaText()
        {
            return commMediaLabel.Text;
        }

        /// <summary>
        /// Get 'SimcardNumber' label text
        /// </summary>
        /// <returns></returns>
        public string GetSimcardNumberText()
        {
            return simcardNumberLabel.Text;
        }

        /// <summary>
        /// Get 'PhoneNumber' label text
        /// </summary>
        /// <returns></returns>
        public string GetPhoneNumberText()
        {
            return phoneNumberLabel.Text;
        }

        /// <summary>
        /// Get 'HardwareRevision' label text
        /// </summary>
        /// <returns></returns>
        public string GetHardwareRevisionText()
        {
            return hardwareRevisionLabel.Text;
        }

        /// <summary>
        /// Get 'SoftwareVersion' label text
        /// </summary>
        /// <returns></returns>
        public string GetSoftwareVersionText()
        {
            return softwareVersionLabel.Text;
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
        /// Get 'RealtimeCommand' label text
        /// </summary>
        /// <returns></returns>
        public string GetRealtimeCommandText()
        {
            return realtimeCommandLabel.Text;
        }

        /// <summary>
        /// Get 'GzipPayload' label text
        /// </summary>
        /// <returns></returns>
        public string GetGzipPayloadText()
        {
            return gzipPayloadLabel.Text;
        }

        /// <summary>
        /// Get 'ControllerCacheMode' label text
        /// </summary>
        /// <returns></returns>
        public string GetControllerCacheModeText()
        {
            return controllerCacheModeLabel.Text;
        }

        /// <summary>
        /// Get 'ControllerId' input value
        /// </summary>
        /// <returns></returns>
        public string GetControllerIdValue()
        {
            return controllerIdInput.Value();
        }

        /// <summary>
        /// Get 'GatewayHostName' input value
        /// </summary>
        /// <returns></returns>
        public string GetGatewayHostNameValue()
        {
            return gatewayHostNameInput.Value();
        }

        /// <summary>
        /// Get 'ControlTechnology' input value
        /// </summary>
        /// <returns></returns>
        public string GetControlTechnologyValue()
        {
            return controlTechnologyDropDown.Text;
        }

        /// <summary>
        /// Get 'CommMedia' input value
        /// </summary>
        /// <returns></returns>
        public string GetCommMediaValue()
        {
            return commMediaDropDown.Text;
        }

        /// <summary>
        /// Get 'SimcardNumber' input value
        /// </summary>
        /// <returns></returns>
        public string GetSimcardNumberValue()
        {
            return simcardNumberInput.Value();
        }

        /// <summary>
        /// Get 'PhoneNumber' input value
        /// </summary>
        /// <returns></returns>
        public string GetPhoneNumberValue()
        {
            return phoneNumberInput.Value();
        }

        /// <summary>
        /// Get 'HardwareRevision' input value
        /// </summary>
        /// <returns></returns>
        public string GetHardwareRevisionValue()
        {
            return hardwareRevisionInput.Value();
        }

        /// <summary>
        /// Get 'SoftwareVersion' input value
        /// </summary>
        /// <returns></returns>
        public string GetSoftwareVersionValue()
        {
            return softwareVersionInput.Value();
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
        /// Get 'RealtimeCommand' input value
        /// </summary>
        /// <returns></returns>
        public string GetRealtimeCommandValue()
        {
            return realtimeCommandDropDown.Text;
        }

        /// <summary>
        /// Get 'GzipPayload' input value
        /// </summary>
        /// <returns></returns>
        public string GetGzipPayloadValue()
        {
            return gzipPayloadDropDown.Text;
        }

        /// <summary>
        /// Get 'ControllerCacheMode' input value
        /// </summary>
        /// <returns></returns>
        public string GetControllerCacheModeValue()
        {
            return controllerCacheModeDropDown.Text;
        }

        #endregion //Identity of the controller group

        #region Communication group

        /// <summary>
        /// Get 'NetworkId' label text
        /// </summary>
        /// <returns></returns>
        public string GetNetworkIdText()
        {
            return networkIdLabel.Text;
        }

        /// <summary>
        /// Get 'ServerWebappUrl' label text
        /// </summary>
        /// <returns></returns>
        public string GetServerWebappUrlText()
        {
            return serverWebappUrlLabel.Text;
        }

        /// <summary>
        /// Get 'Username' label text
        /// </summary>
        /// <returns></returns>
        public string GetUsernameText()
        {
            return usernameLabel.Text;
        }

        /// <summary>
        /// Get 'Password' label text
        /// </summary>
        /// <returns></returns>
        public string GetPasswordText()
        {
            return passwordLabel.Text;
        }

        /// <summary>
        /// Get 'CommissionMode' label text
        /// </summary>
        /// <returns></returns>
        public string GetCommissionModeText()
        {
            return commissionModeLabel.Text;
        }

        /// <summary>
        /// Get 'ConfigPath' label text
        /// </summary>
        /// <returns></returns>
        public string GetConfigPathText()
        {
            return configPathLabel.Text;
        }

        /// <summary>
        /// Get 'RealtimePath' label text
        /// </summary>
        /// <returns></returns>
        public string GetRealtimePathText()
        {
            return realtimePathLabel.Text;
        }

        /// <summary>
        /// Get 'GroupDimCommand' label text
        /// </summary>
        /// <returns></returns>
        public string GetGroupDimCommandText()
        {
            return groupDimCommandLabel.Text;
        }

        /// <summary>
        /// Get 'GlobalDimCommand' label text
        /// </summary>
        /// <returns></returns>
        public string GetGlobalDimCommandText()
        {
            return globalDimCommandLabel.Text;
        }

        /// <summary>
        /// Get 'MaxDevices' label text
        /// </summary>
        /// <returns></returns>
        public string GetMaxDevicesText()
        {
            return maxDevicesLabel.Text;
        }

        /// <summary>
        /// Get 'ConsiderStreetLights' label text
        /// </summary>
        /// <returns></returns>
        public string GetConsiderStreetLightsText()
        {
            return considerStreetLightsLabel.Text;
        }

        /// <summary>
        /// Get 'ConsiderReplaceOlc' label text
        /// </summary>
        /// <returns></returns>
        public string GetConsiderReplaceOlcText()
        {
            return considerReplaceOlcLabel.Text;
        }

        /// <summary>
        /// Get 'ReportTime' label text
        /// </summary>
        /// <returns></returns>
        public string GetReportTimeText()
        {
            return reportTimeLabel.Text;
        }

        /// <summary>
        /// Get 'ReportFrequency' label text
        /// </summary>
        /// <returns></returns>
        public string GetReportFrequencyText()
        {
            return reportFrequencyLabel.Text;
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
        /// Get 'MaxConnections' label text
        /// </summary>
        /// <returns></returns>
        public string GetMaxConnectionsText()
        {
            return maxConnectionsLabel.Text;
        }

        /// <summary>
        /// Get 'RequestInterval' label text
        /// </summary>
        /// <returns></returns>
        public string GetRequestIntervalText()
        {
            return requestIntervalLabel.Text;
        }

        /// <summary>
        /// Get 'InclLatencyInterval' label text
        /// </summary>
        /// <returns></returns>
        public string GetInclLatencyIntervalText()
        {
            return inclLatencyIntervalLabel.Text;
        }

        /// <summary>
        /// Get 'Gateway' label text
        /// </summary>
        /// <returns></returns>
        public string GetGatewayText()
        {
            return gatewayLabel.Text;
        }

        /// <summary>
        /// Get 'NetworkId' input value
        /// </summary>
        /// <returns></returns>
        public string GetNetworkIdValue()
        {
            return networkIdInput.Value();
        }

        /// <summary>
        /// Get 'ServerWebappUrl' input value
        /// </summary>
        /// <returns></returns>
        public string GetServerWebAppUrlValue()
        {
            return serverWebAppUrlInput.Value();
        }

        /// <summary>
        /// Get 'Username' input value
        /// </summary>
        /// <returns></returns>
        public string GetUsernameValue()
        {
            return usernameInput.Value();
        }

        /// <summary>
        /// Get 'Password' input value
        /// </summary>
        /// <returns></returns>
        public string GetPasswordValue()
        {
            return passwordInput.Value();
        }

        /// <summary>
        /// Get 'CommissionMode' input value
        /// </summary>
        /// <returns></returns>
        public string GetCommissionModeValue()
        {
            return commissionModeDropDown.Text;
        }

        /// <summary>
        /// Get 'ConfigPath' input value
        /// </summary>
        /// <returns></returns>
        public string GetConfigPathValue()
        {
            return configPathInput.Value();
        }

        /// <summary>
        /// Get 'RealtimePath' input value
        /// </summary>
        /// <returns></returns>
        public string GetRealtimePathValue()
        {
            return realtimePathInput.Value();
        }

        /// <summary>
        /// Get 'GroupDimCommand' input value
        /// </summary>
        /// <returns></returns>
        public bool GetGroupDimCommandValue()
        {
            return groupDimCommandCheckbox.CheckboxValue();
        }

        /// <summary>
        /// Get 'GlobalDimCommand' input value
        /// </summary>
        /// <returns></returns>
        public bool GetGlobalDimCommandValue()
        {
            return globalDimCommandCheckbox.CheckboxValue();
        }

        /// <summary>
        /// Get 'MaxDevices' input value
        /// </summary>
        /// <returns></returns>
        public string GetMaxDevicesValue()
        {
            return maxDevicesNumericInput.Value();
        }

        /// <summary>
        /// Get 'ConsiderStreetLights' input value
        /// </summary>
        /// <returns></returns>
        public bool GetConsiderStreetLightsValue()
        {
            return considerStreetLightsCheckbox.CheckboxValue();
        }

        /// <summary>
        /// Get 'ConsiderReplaceOlc' input value
        /// </summary>
        /// <returns></returns>
        public bool GetConsiderReplaceOlcValue()
        {
            return considerReplaceOlcCheckbox.CheckboxValue();
        }

        /// <summary>
        /// Get 'ReportTime' input value
        /// </summary>
        /// <returns></returns>
        public string GetReportTimeValue()
        {
            return reportTimeInput.Value();
        }

        /// <summary>
        /// Get 'ReportFrequency' input value
        /// </summary>
        /// <returns></returns>
        public string GetReportFrequencyValue()
        {
            return reportFrequencyNumericInput.Value();
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
        /// Get 'MaxConnections' input value
        /// </summary>
        /// <returns></returns>
        public string GetMaxConnectionsValue()
        {
            return maxConnectionsNumericInput.Value();
        }

        /// <summary>
        /// Get 'RequestInterval' input value
        /// </summary>
        /// <returns></returns>
        public string GetRequestIntervalValue()
        {
            return requestIntervalNumericInput.Value();
        }

        /// <summary>
        /// Get 'InclLatencyInterval' input value
        /// </summary>
        /// <returns></returns>
        public bool GetInclLatencyIntervalValue()
        {
            return inclLatencyIntervalCheckbox.CheckboxValue();
        }

        /// <summary>
        /// Get 'Gateway' input value
        /// </summary>
        /// <returns></returns>
        public string GetGatewayValue()
        {
            return gatewayInput.Value();
        }

        #endregion //Communication group

        #region Control System group

        /// <summary>
        /// Get 'IsCpd' label text
        /// </summary>
        /// <returns></returns>
        public string GetIsCpdText()
        {
            return isCpdLabel.Text;
        }

        /// <summary>
        /// Get 'LibraryVersion' label text
        /// </summary>
        /// <returns></returns>
        public string GetLibraryVersionText()
        {
            return libraryVersionLabel.Text;
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
        /// Get 'Retries' label text
        /// </summary>
        /// <returns></returns>
        public string GetRetriesText()
        {
            return retriesLabel.Text;
        }

        /// <summary>
        /// Get 'SkipDeleteSchedule' label text
        /// </summary>
        /// <returns></returns>
        public string GetSkipDeleteScheduleText()
        {
            return skipDeleteScheduleLabel.Text;
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
        /// Get 'LibraryVersion' input value
        /// </summary>
        /// <returns></returns>
        public string GetLibraryVersionValue()
        {
            return libraryVersionInput.Value();
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

        /// <summary>
        /// Get 'Retries' input value
        /// </summary>
        /// <returns></returns>
        public string GetRetriesValue()
        {
            return retriesNumericInput.Value();
        }

        /// <summary>
        /// Get 'SkipDeleteSchedule' input value
        /// </summary>
        /// <returns></returns>
        public bool GetSkipDeleteScheduleValue()
        {
            return skipDeleteScheduleCheckbox.CheckboxValue();
        }

        #endregion //Control System group

        #region Auto-commissioning group

        /// <summary>
        /// Get 'AutoCommActive' label text
        /// </summary>
        /// <returns></returns>
        public string GetAutoCommActiveText()
        {
            return autoCommActiveLabel.Text;
        }

        /// <summary>
        /// Get 'ReferenceUnitAutoComm' label text
        /// </summary>
        /// <returns></returns>
        public string GetReferenceUnitAutoCommText()
        {
            return referenceUnitAutoCommLabel.Text;
        }

        /// <summary>
        /// Get 'MaxUnitsAutoComm' label text
        /// </summary>
        /// <returns></returns>
        public string GetMaxUnitsAutoCommText()
        {
            return maxUnitsAutoCommLabel.Text;
        }

        /// <summary>
        /// Get 'TimeoutAutoComm' label text
        /// </summary>
        /// <returns></returns>
        public string GetTimeoutAutoCommText()
        {
            return timeoutAutoCommLabel.Text;
        }

        /// <summary>
        /// Get 'UsernameAutoComm' label text
        /// </summary>
        /// <returns></returns>
        public string GetUsernameAutoCommText()
        {
            return usernameAutoCommLabel.Text;
        }

        /// <summary>
        /// Get 'PasswordAutoComm' label text
        /// </summary>
        /// <returns></returns>
        public string GetPasswordAutoCommText()
        {
            return passwordAutoCommLabel.Text;
        }

        /// <summary>
        /// Get 'AutoCommActive' input value
        /// </summary>
        /// <returns></returns>
        public bool GetAutoCommActiveValue()
        {
            return autoCommActiveCheckbox.Selected;
        }

        /// <summary>
        /// Get 'ReferenceUnitAutoComm' input value
        /// </summary>
        /// <returns></returns>
        public string GetReferenceUnitAutoCommValue()
        {
            return referenceUnitAutoCommInput.Value();
        }

        /// <summary>
        /// Get 'MaxUnitsAutoComm' input value
        /// </summary>
        /// <returns></returns>
        public string GetMaxUnitsAutoCommValue()
        {
            return maxUnitsAutoCommNumericInput.Value();
        }

        /// <summary>
        /// Get 'TimeoutAutoComm' input value
        /// </summary>
        /// <returns></returns>
        public string GetTimeoutAutoCommValue()
        {
            return timeoutAutoCommNumericInput.Value();
        }

        /// <summary>
        /// Get 'UsernameAutoComm' input value
        /// </summary>
        /// <returns></returns>
        public string GetUsernameAutoCommValue()
        {
            return usernameAutoCommInput.Value();
        }

        /// <summary>
        /// Get 'PasswordAutoComm' input value
        /// </summary>
        /// <returns></returns>
        public string GetPasswordAutoCommValue()
        {
            return passwordAutoCommInput.Value();
        }

        #endregion //Auto-commissioning group

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

        #endregion //Location group

        #region About this cabinet group

        /// <summary>
        /// Get 'TypeOfCabinet' label text
        /// </summary>
        /// <returns></returns>
        public string GetTypeOfCabinetText()
        {
            return typeOfCabinetLabel.Text;
        }

        /// <summary>
        /// Get 'CabinetWattage' label text
        /// </summary>
        /// <returns></returns>
        public string GetCabinetWattageText()
        {
            return cabinetWattageLabel.Text;
        }

        /// <summary>
        /// Get 'NumberOfSegments' label text
        /// </summary>
        /// <returns></returns>
        public string GetNumberOfSegmentsText()
        {
            return numberOfSegmentsLabel.Text;
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
        /// Get 'Comment' label text
        /// </summary>
        /// <returns></returns>
        public string GetCommentText()
        {
            return commentLabel.Text;
        }

        /// <summary>
        /// Get 'TypeOfCabinet' input value
        /// </summary>
        /// <returns></returns>
        public string GetTypeOfCabinetValue()
        {
            return typeOfCabinetInput.Value();
        }

        /// <summary>
        /// Get 'CabinetWattage' input value
        /// </summary>
        /// <returns></returns>
        public string GetCabinetWattageValue()
        {
            return cabinetWattageNumericInput.Value();
        }

        /// <summary>
        /// Get 'NumberOfSegments' input value
        /// </summary>
        /// <returns></returns>
        public string GetNumberOfSegmentsValue()
        {
            return numberOfSegmentsNumericInput.Value();
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
        /// Get 'Comment' input value
        /// </summary>
        /// <returns></returns>
        public string GetCommentValue()
        {
            return commentInput.Value();
        }

        #endregion //About this cabinet group

        #endregion //Inventory tab

        #region Inputs and outputs tab

        #region About the Segment Controller's digital inputs group

        /// <summary>
        /// Get 'Input1' label text
        /// </summary>
        /// <returns></returns>
        public string GetInput1Text()
        {
            return input1Label.Text;
        }

        /// <summary>
        /// Get 'Input2' label text
        /// </summary>
        /// <returns></returns>
        public string GetInput2Text()
        {
            return input2Label.Text;
        }

        /// <summary>
        /// Get 'DigitalModbus1' label text
        /// </summary>
        /// <returns></returns>
        public string GetDigitalModbus1Text()
        {
            return digitalModbus1Label.Text;
        }

        /// <summary>
        /// Get 'DigitalModbus2' label text
        /// </summary>
        /// <returns></returns>
        public string GetDigitalModbus2Text()
        {
            return digitalModbus2Label.Text;
        }

        /// <summary>
        /// Get 'DigitalModbus3' label text
        /// </summary>
        /// <returns></returns>
        public string GetDigitalModbus3Text()
        {
            return digitalModbus3Label.Text;
        }

        /// <summary>
        /// Get 'DigitalModbus4' label text
        /// </summary>
        /// <returns></returns>
        public string GetDigitalModbus4Text()
        {
            return digitalModbus4Label.Text;
        }

        /// <summary>
        /// Get 'DigitalModbus5' label text
        /// </summary>
        /// <returns></returns>
        public string GetDigitalModbus5Text()
        {
            return digitalModbus5Label.Text;
        }

        /// <summary>
        /// Get 'DigitalModbus6' label text
        /// </summary>
        /// <returns></returns>
        public string GetDigitalModbus6Text()
        {
            return digitalModbus6Label.Text;
        }

        /// <summary>
        /// Get 'DigitalModbus7' label text
        /// </summary>
        /// <returns></returns>
        public string GetDigitalModbus7Text()
        {
            return digitalModbus7Label.Text;
        }

        /// <summary>
        /// Get 'DigitalModbus8' label text
        /// </summary>
        /// <returns></returns>
        public string GetDigitalModbus8Text()
        {
            return digitalModbus8Label.Text;
        }

        /// <summary>
        /// Get 'DigitalModbus9' label text
        /// </summary>
        /// <returns></returns>
        public string GetDigitalModbus9Text()
        {
            return digitalModbus9Label.Text;
        }

        /// <summary>
        /// Get 'DigitalModbus10' label text
        /// </summary>
        /// <returns></returns>
        public string GetDigitalModbus10Text()
        {
            return digitalModbus10Label.Text;
        }

        /// <summary>
        /// Get 'DigitalModbus11' label text
        /// </summary>
        /// <returns></returns>
        public string GetDigitalModbus11Text()
        {
            return digitalModbus11Label.Text;
        }

        /// <summary>
        /// Get 'DigitalModbus12' label text
        /// </summary>
        /// <returns></returns>
        public string GetDigitalModbus12Text()
        {
            return digitalModbus12Label.Text;
        }

        /// <summary>
        /// Get '1' input value
        /// </summary>
        /// <returns></returns>
        public string Get1Value()
        {
            return input1Input.Value();
        }

        /// <summary>
        /// Get '2' input value
        /// </summary>
        /// <returns></returns>
        public string Get2Value()
        {
            return input2Input.Value();
        }

        /// <summary>
        /// Get 'DigitalModbus1' input value
        /// </summary>
        /// <returns></returns>
        public string GetDigitalModbus1Value()
        {
            return digitalModbus1Input.Value();
        }

        /// <summary>
        /// Get 'DigitalModbus2' input value
        /// </summary>
        /// <returns></returns>
        public string GetDigitalModbus2Value()
        {
            return digitalModbus2Input.Value();
        }

        /// <summary>
        /// Get 'DigitalModbus3' input value
        /// </summary>
        /// <returns></returns>
        public string GetDigitalModbus3Value()
        {
            return digitalModbus3Input.Value();
        }

        /// <summary>
        /// Get 'DigitalModbus4' input value
        /// </summary>
        /// <returns></returns>
        public string GetDigitalModbus4Value()
        {
            return digitalModbus4Input.Value();
        }

        /// <summary>
        /// Get 'DigitalModbus5' input value
        /// </summary>
        /// <returns></returns>
        public string GetDigitalModbus5Value()
        {
            return digitalModbus5Input.Value();
        }

        /// <summary>
        /// Get 'DigitalModbus6' input value
        /// </summary>
        /// <returns></returns>
        public string GetDigitalModbus6Value()
        {
            return digitalModbus6Input.Value();
        }

        /// <summary>
        /// Get 'DigitalModbus7' input value
        /// </summary>
        /// <returns></returns>
        public string GetDigitalModbus7Value()
        {
            return digitalModbus7Input.Value();
        }

        /// <summary>
        /// Get 'DigitalModbus8' input value
        /// </summary>
        /// <returns></returns>
        public string GetDigitalModbus8Value()
        {
            return digitalModbus8Input.Value();
        }

        /// <summary>
        /// Get 'DigitalModbus9' input value
        /// </summary>
        /// <returns></returns>
        public string GetDigitalModbus9Value()
        {
            return digitalModbus9Input.Value();
        }

        /// <summary>
        /// Get 'DigitalModbus10' input value
        /// </summary>
        /// <returns></returns>
        public string GetDigitalModbus10Value()
        {
            return digitalModbus10Input.Value();
        }

        /// <summary>
        /// Get 'DigitalModbus11' input value
        /// </summary>
        /// <returns></returns>
        public string GetDigitalModbus11Value()
        {
            return digitalModbus11Input.Value();
        }

        /// <summary>
        /// Get 'DigitalModbus12' input value
        /// </summary>
        /// <returns></returns>
        public string GetDigitalModbus12Value()
        {
            return digitalModbus12Input.Value();
        }

        #endregion //About the Segment Controller's digital inputs group

        #region About the Segment Controller's digital outputs group

        /// <summary>
        /// Get 'MainsControlMode' label text
        /// </summary>
        /// <returns></returns>
        public string GetMainsControlModeText()
        {
            return mainsControlModeLabel.Text;
        }

        /// <summary>
        /// Get 'LightControlMode' label text
        /// </summary>
        /// <returns></returns>
        public string GetLightControlModeText()
        {
            return lightControlModeLabel.Text;
        }

        /// <summary>
        /// Get 'Output1' label text
        /// </summary>
        /// <returns></returns>
        public string GetOutput1LabelText()
        {
            return output1LabelLabel.Text;
        }

        /// <summary>
        /// Get 'Output1Calendar' label text
        /// </summary>
        /// <returns></returns>
        public string GetOutput1CalendarText()
        {
            return output1CalendarLabel.Text;
        }

        /// <summary>
        /// Get 'Output2' label text
        /// </summary>
        /// <returns></returns>
        public string GetOutput2LabelText()
        {
            return output2LabelLabel.Text;
        }

        /// <summary>
        /// Get 'Output2Calendar' label text
        /// </summary>
        /// <returns></returns>
        public string GetOutput2CalendarText()
        {
            return output2CalendarLabel.Text;
        }

        /// <summary>
        /// Get 'MainsControlMode' input value
        /// </summary>
        /// <returns></returns>
        public string GetMainsControlModeValue()
        {
            return mainsControlModeDropDown.Text;
        }

        /// <summary>
        /// Get 'LightControlMode' input value
        /// </summary>
        /// <returns></returns>
        public string GetLightControlModeValue()
        {
            return lightControlModeDropDown.Text;
        }

        /// <summary>
        /// Get 'Output1Label' input value
        /// </summary>
        /// <returns></returns>
        public string GetOutput1LabelValue()
        {
            return output1LabelInput.Value();
        }

        /// <summary>
        /// Get 'Output1Calendar' input value
        /// </summary>
        /// <returns></returns>
        public string GetOutput1CalendarValue()
        {
            return output1CalendarInput.Value();
        }

        /// <summary>
        /// Get 'Output2Label' input value
        /// </summary>
        /// <returns></returns>
        public string GetOutput2LabelValue()
        {
            return output2LabelInput.Value();
        }

        /// <summary>
        /// Get 'Output2Calendar' input value
        /// </summary>
        /// <returns></returns>
        public string GetOutput2CalendarValue()
        {
            return output2CalendarInput.Value();
        }

        #endregion //About the Segment Controller's digital outputs group

        #region Modbus Digital Inputs - Failure labels group

        /// <summary>
        /// Get 'DigitalModbus1Failure' label text
        /// </summary>
        /// <returns></returns>
        public string GetDigitalModbus1FailureText()
        {
            return digitalModbus1FailureLabel.Text;
        }

        /// <summary>
        /// Get 'DigitalModbus2Failure' label text
        /// </summary>
        /// <returns></returns>
        public string GetDigitalModbus2FailureText()
        {
            return digitalModbus2FailureLabel.Text;
        }

        /// <summary>
        /// Get 'DigitalModbus3Failure' label text
        /// </summary>
        /// <returns></returns>
        public string GetDigitalModbus3FailureText()
        {
            return digitalModbus3FailureLabel.Text;
        }

        /// <summary>
        /// Get 'DigitalModbus4Failure' label text
        /// </summary>
        /// <returns></returns>
        public string GetDigitalModbus4FailureText()
        {
            return digitalModbus4FailureLabel.Text;
        }

        /// <summary>
        /// Get 'DigitalModbus5Failure' label text
        /// </summary>
        /// <returns></returns>
        public string GetDigitalModbus5FailureText()
        {
            return digitalModbus5FailureLabel.Text;
        }

        /// <summary>
        /// Get 'DigitalModbus6Failure' label text
        /// </summary>
        /// <returns></returns>
        public string GetDigitalModbus6FailureText()
        {
            return digitalModbus6FailureLabel.Text;
        }

        /// <summary>
        /// Get 'DigitalModbus7Failure' label text
        /// </summary>
        /// <returns></returns>
        public string GetDigitalModbus7FailureText()
        {
            return digitalModbus7FailureLabel.Text;
        }

        /// <summary>
        /// Get 'DigitalModbus8Failure' label text
        /// </summary>
        /// <returns></returns>
        public string GetDigitalModbus8FailureText()
        {
            return digitalModbus8FailureLabel.Text;
        }

        /// <summary>
        /// Get 'DigitalModbus9Failure' label text
        /// </summary>
        /// <returns></returns>
        public string GetDigitalModbus9FailureText()
        {
            return digitalModbus9FailureLabel.Text;
        }

        /// <summary>
        /// Get 'DigitalModbus10Failure' label text
        /// </summary>
        /// <returns></returns>
        public string GetDigitalModbus10FailureText()
        {
            return digitalModbus10FailureLabel.Text;
        }

        /// <summary>
        /// Get 'DigitalModbus11Failure' label text
        /// </summary>
        /// <returns></returns>
        public string GetDigitalModbus11FailureText()
        {
            return digitalModbus11FailureLabel.Text;
        }

        /// <summary>
        /// Get 'DigitalModbus12Failure' label text
        /// </summary>
        /// <returns></returns>
        public string GetDigitalModbus12FailureText()
        {
            return digitalModbus12FailureLabel.Text;
        }

        /// <summary>
        /// Get 'DigitalModbus1Failure' input value
        /// </summary>
        /// <returns></returns>
        public string GetDigitalModbus1FailureValue()
        {
            return digitalModbus1FailureInput.Value();
        }

        /// <summary>
        /// Get 'DigitalModbus2Failure' input value
        /// </summary>
        /// <returns></returns>
        public string GetDigitalModbus2FailureValue()
        {
            return digitalModbus2FailureInput.Value();
        }

        /// <summary>
        /// Get 'DigitalModbus3Failure' input value
        /// </summary>
        /// <returns></returns>
        public string GetDigitalModbus3FailureValue()
        {
            return digitalModbus3FailureInput.Value();
        }

        /// <summary>
        /// Get 'DigitalModbus4Failure' input value
        /// </summary>
        /// <returns></returns>
        public string GetDigitalModbus4FailureValue()
        {
            return digitalModbus4FailureInput.Value();
        }

        /// <summary>
        /// Get 'DigitalModbus5Failure' input value
        /// </summary>
        /// <returns></returns>
        public string GetDigitalModbus5FailureValue()
        {
            return digitalModbus5FailureInput.Value();
        }

        /// <summary>
        /// Get 'DigitalModbus6Failure' input value
        /// </summary>
        /// <returns></returns>
        public string GetDigitalModbus6FailureValue()
        {
            return digitalModbus6FailureInput.Value();
        }

        /// <summary>
        /// Get 'DigitalModbus7Failure' input value
        /// </summary>
        /// <returns></returns>
        public string GetDigitalModbus7FailureValue()
        {
            return digitalModbus7FailureInput.Value();
        }

        /// <summary>
        /// Get 'DigitalModbus8Failure' input value
        /// </summary>
        /// <returns></returns>
        public string GetDigitalModbus8FailureValue()
        {
            return digitalModbus8FailureInput.Value();
        }

        /// <summary>
        /// Get 'DigitalModbus9Failure' input value
        /// </summary>
        /// <returns></returns>
        public string GetDigitalModbus9FailureValue()
        {
            return digitalModbus9FailureInput.Value();
        }

        /// <summary>
        /// Get 'DigitalModbus10Failure' input value
        /// </summary>
        /// <returns></returns>
        public string GetDigitalModbus10FailureValue()
        {
            return digitalModbus10FailureInput.Value();
        }

        /// <summary>
        /// Get 'DigitalModbus11Failure' input value
        /// </summary>
        /// <returns></returns>
        public string GetDigitalModbus11FailureValue()
        {
            return digitalModbus11FailureInput.Value();
        }

        /// <summary>
        /// Get 'DigitalModbus12Failure' input value
        /// </summary>
        /// <returns></returns>
        public string GetDigitalModbus12FailureValue()
        {
            return digitalModbus12FailureInput.Value();
        }

        #endregion //Modbus Digital Inputs - Failure labels group

        #region Modbus analog inputs - Labels group

        /// <summary>
        /// Get 'AnalogModbus1' label text
        /// </summary>
        /// <returns></returns>
        public string GetAnalogModbus1Text()
        {
            return analogModbus1Label.Text;
        }

        /// <summary>
        /// Get 'AnalogModbus2' label text
        /// </summary>
        /// <returns></returns>
        public string GetAnalogModbus2Text()
        {
            return analogModbus2Label.Text;
        }

        /// <summary>
        /// Get 'AnalogModbus3' label text
        /// </summary>
        /// <returns></returns>
        public string GetAnalogModbus3Text()
        {
            return analogModbus3Label.Text;
        }

        /// <summary>
        /// Get 'AnalogModbus1' input value
        /// </summary>
        /// <returns></returns>
        public string GetAnalogModbus1Value()
        {
            return analogModbus1Input.Value();
        }

        /// <summary>
        /// Get 'AnalogModbus2' input value
        /// </summary>
        /// <returns></returns>
        public string GetAnalogModbus2Value()
        {
            return analogModbus2Input.Value();
        }

        /// <summary>
        /// Get 'AnalogModbus3' input value
        /// </summary>
        /// <returns></returns>
        public string GetAnalogModbus3Value()
        {
            return analogModbus3Input.Value();
        }

        #endregion //Modbus analog inputs - Labels group

        #endregion //Inputs and outputs tab

        #region Time tab        

        #region Time management group

        /// <summary>
        /// Get 'Timezone' label text
        /// </summary>
        /// <returns></returns>
        public string GetTimezoneText()
        {
            return timezoneLabel.Text;
        }

        /// <summary>
        /// Get 'Timezone' input value
        /// </summary>
        /// <returns></returns>
        public string GetTimezoneValue()
        {
            return timezoneDropDown.Text;
        }

        #endregion //Time management group        

        #endregion //Time tab

        #region System tab

        #region Resource usage group

        /// <summary>
        /// Get 'SmartPoll' label text
        /// </summary>
        /// <returns></returns>
        public string GetSmartPollText()
        {
            return smartPollLabel.Text;
        }

        /// <summary>
        /// Get 'SlvDatapointServer' label text
        /// </summary>
        /// <returns></returns>
        public string GetSlvDatapointServerText()
        {
            return slvDatapointServerLabel.Text;
        }

        /// <summary>
        /// Get 'SmartPoll' input value
        /// </summary>
        /// <returns></returns>
        public bool GetSmartPollValue()
        {
            return smartPollCheckbox.Selected;
        }

        /// <summary>
        /// Get 'SlvDatapointServer' input value
        /// </summary>
        /// <returns></returns>
        public bool GetSlvDatapointServerValue()
        {
            return slvDatapointServerCheckbox.Selected;
        }

        #endregion //Resource usage group

        #endregion //System tab

        #endregion //Get methods

        #endregion //Basic methods

        #region Business methods        

        public List<string> GetListOfControlTechnologies()
        {
            return controlTechnologyDropDown.GetAllItems();
        }

        public void SelectRandomCommMediaDropDown()
        {
            var currentValue = GetCommMediaValue();
            var listItems = commMediaDropDown.GetAllItems();
            listItems.Remove(currentValue);
            commMediaDropDown.Select(listItems.PickRandom());
        }

        public void SelectRandomRealtimeCommandDropDown()
        {
            var currentValue = GetRealtimeCommandValue();
            var listItems = realtimeCommandDropDown.GetAllItems();
            listItems.Remove(currentValue);
            realtimeCommandDropDown.Select(listItems.PickRandom());
        }

        public void SelectRandomGzipPayloadDropDown()
        {
            var currentValue = GetGzipPayloadValue();
            var listItems = gzipPayloadDropDown.GetAllItems();
            listItems.Remove(currentValue);
            gzipPayloadDropDown.Select(listItems.PickRandom());
        }

        public void SelectRandomControllerCacheModeDropDown()
        {
            var currentValue = GetControllerCacheModeValue();
            var listItems = controllerCacheModeDropDown.GetAllItems();
            listItems.Remove(currentValue);
            controllerCacheModeDropDown.Select(listItems.PickRandom());
        }

        public void SelectRandomCommissionModeDropDown()
        {
            var currentValue = GetCommissionModeValue();
            var listItems = commissionModeDropDown.GetAllItems();
            listItems.Remove(currentValue);
            commissionModeDropDown.Select(listItems.PickRandom());
        }

        #region Identity

        public bool IsControllerIdInputDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-property-controllerStrId-field']"));
        }

        public bool IsGatewayHostNameInputDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-property-controller-host-field']"));
        }

        public bool IsControlTechnologyDropDownDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-property-controller-firmwareVersion-field']"));
        }

        public bool IsRealtimeCommandDropDownDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-property-XmlTechGetCommandMode-field']"));
        }

        public bool IsGzipPayloadDropDownDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-property-XmlTechGzipRequestBody-field']"));
        }

        public bool IsControllerCacheModeDropDownDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-property-ControllerCacheMode-field']"));
        }

        public bool IsCommissionModeDropDownDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-property-DevicesConfigMode-field']"));
        }

        public bool IsRealtimePathInputDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-property-servicePathRealtime-field']"));
        }

        public bool IsConfigPathInputDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-property-servicePathConfiguration-field']"));
        }

        public bool IsGroupDimCommandCheckboxDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-property-xmltech-group-dim-cde-support-field']"));
        }
        public bool IsGlobalDimCommandCheckboxDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-property-xmltech-all-dim-cde-support-field']"));
        }

        public bool IsMaxDevicesInputDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-property-xmltech-maxdevicespercontroller-field']"));
        }

        public bool IsTimeoutInputDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-property-timeout-field']"));
        }

        public bool IsMaxConnectionsInputDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-property-maxConnections-field']"));
        }

        public bool IsIsCpdCheckboxDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-property-ssn-is-cpd-field']"));
        }

        public bool IsLibraryVersionInputDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-property-ssn-library-version-field']"));
        }

        public bool IsLastImuLogSequenceInputDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-property-ssn-logsequence-imu-last-field']"));
        }

        public bool IsLastImuRequestTimeInputDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-property-ssn-requesttime-imu-last-field']"));
        }

        public bool IsRetriesInputDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-property-retries-field']"));
        }

        public bool IsSkipDeleteScheduleCheckboxDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-property-ssn-skip-delete-schedule-when-commissioning-calendars-field']"));
        }

        public bool IsControllerIdInputReadOnly()
        {
            return controllerIdInput.IsReadOnly();
        }

        #endregion //Identity      

        #region Inventory

        public bool IsCabinetWattageInputDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-property-cabinetwattage-field']"));
        }

        #endregion //Inventory

        #region Inputs and outputs

        public bool IsMainsControlModeDropDownDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-property-MainsControlMode-field']"));
        }

        public bool IsLightControlModeDropDownDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-property-LightControlMode-field']"));
        }

        public bool IsOutput1LabelInputDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-property-DigitalOutput1Info-field']"));
        }

        public bool IsOutput1CalendarNameInputDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-property-DigitalOutput1Calendar-field']"));
        }

        public bool IsOutput2LabelInputDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-property-DigitalOutput2Info-field']"));
        }

        public bool IsOutput2CalendarNameInputDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-property-DigitalOutput2Calendar-field']"));
        }

        #endregion //Inputs and outputs

        #region Time

        public bool IsTimezoneInputDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-property-TimeZoneId-field']"));
        }

        #endregion //Time

        #endregion //Business methods   

        public override void WaitForPanelLoaded()
        {
            base.WaitForPanelLoaded();
        }
    }
}
