using ImageMagick;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using StreetlightVision.Extensions;
using StreetlightVision.Models;
using StreetlightVision.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StreetlightVision.Pages.UI
{
    public class NewDevicePanel : PanelBase
    {
        #region Variables

        #endregion //Variables

        #region IWebElements

        #region New device Menu

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-createDevice-backButton']")]
        private IWebElement backButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-createDevice-title'] .equipment-gl-editor-title-label")]
        private IWebElement panelTitle;

        [FindsBy(How = How.CssSelector, Using = "[id$='geozone-createDevice-equipmenent-list'] .equipment-gl-list-item")]
        private IList<IWebElement> deviceList;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-createDevice-equipment-gl-building']")]
        private IWebElement buildingButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-createDevice-equipment-gl-building'] .equipment-list-item-icon")]
        private IWebElement buildingIcon;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-createDevice-equipment-gl-building'] .equipment-list-item-title")]
        private IWebElement buildingLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-createDevice-equipment-gl-cameraip']")]
        private IWebElement cameraIpButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-createDevice-equipment-gl-cameraip'] .equipment-list-item-icon")]
        private IWebElement cameraIpIcon;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-createDevice-equipment-gl-cameraip'] .equipment-list-item-title")]
        private IWebElement cameraIpLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-createDevice-equipment-gl-cityObject']")]
        private IWebElement cityObjectButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-createDevice-equipment-gl-cityObject'] .equipment-list-item-icon")]
        private IWebElement cityObjectIcon;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-createDevice-equipment-gl-cityObject'] .equipment-list-item-title")]
        private IWebElement cityObjectLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-createDevice-equipment-gl-electricalCounter']")]
        private IWebElement electricalCounterButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-createDevice-equipment-gl-electricalCounter'] .equipment-list-item-icon")]
        private IWebElement electricalCounterIcon;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-createDevice-equipment-gl-electricalCounter'] .equipment-list-item-title")]
        private IWebElement electricalCounterLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-createDevice-equipment-gl-envSensor']")]
        private IWebElement envSensorButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-createDevice-equipment-gl-envSensor'] .equipment-list-item-icon")]
        private IWebElement envSensorIcon;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-createDevice-equipment-gl-envSensor'] .equipment-list-item-title")]
        private IWebElement envSensorLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-createDevice-equipment-gl-input']")]
        private IWebElement inputButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-createDevice-equipment-gl-input'] .equipment-list-item-icon")]
        private IWebElement inputIcon;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-createDevice-equipment-gl-input'] .equipment-list-item-title")]
        private IWebElement inputLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-createDevice-equipment-gl-nature']")]
        private IWebElement natureButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-createDevice-equipment-gl-nature'] .equipment-list-item-icon")]
        private IWebElement natureIcon;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-createDevice-equipment-gl-nature'] .equipment-list-item-title")]
        private IWebElement natureLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-createDevice-equipment-gl-networkComponent']")]
        private IWebElement networkComponentButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-createDevice-equipment-gl-networkComponent'] .equipment-list-item-icon")]
        private IWebElement networkComponentIcon;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-createDevice-equipment-gl-networkComponent'] .equipment-list-item-title")]
        private IWebElement networkComponentLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-createDevice-equipment-gl-occupancySensor']")]
        private IWebElement occupancySensorButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-createDevice-equipment-gl-occupancySensor'] .equipment-list-item-icon")]
        private IWebElement occupancySensorIcon;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-createDevice-equipment-gl-occupancySensor'] .equipment-list-item-title")]
        private IWebElement occupancySensorLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-createDevice-equipment-gl-output']")]
        private IWebElement outputButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-createDevice-equipment-gl-output'] .equipment-list-item-icon")]
        private IWebElement outputIcon;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-createDevice-equipment-gl-output'] .equipment-list-item-title")]
        private IWebElement outputLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-createDevice-equipment-gl-parkingPlace']")]
        private IWebElement parkingPlaceButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-createDevice-equipment-gl-parkingPlace'] .equipment-list-item-icon")]
        private IWebElement parkingPlaceIcon;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-createDevice-equipment-gl-parkingPlace'] .equipment-list-item-title")]
        private IWebElement parkingPlaceLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-createDevice-equipment-gl-streetlight']")]
        private IWebElement streetlightButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-createDevice-equipment-gl-streetlight'] .equipment-list-item-icon")]
        private IWebElement streetlightIcon;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-createDevice-equipment-gl-streetlight'] .equipment-list-item-title")]
        private IWebElement streetlightLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-createDevice-equipment-gl-switch']")]
        private IWebElement switchDeviceButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-createDevice-equipment-gl-switch'] .equipment-list-item-icon")]
        private IWebElement switchDeviceIcon;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-createDevice-equipment-gl-switch'] .equipment-list-item-title")]
        private IWebElement switchDeviceLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-createDevice-equipment-gl-tank']")]
        private IWebElement tankButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-createDevice-equipment-gl-tank'] .equipment-list-item-icon")]
        private IWebElement tankIcon;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-createDevice-equipment-gl-tank'] .equipment-list-item-title")]
        private IWebElement tankLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-createDevice-equipment-gl-transportSignage']")]
        private IWebElement transportSignageButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-createDevice-equipment-gl-transportSignage'] .equipment-list-item-icon")]
        private IWebElement transportSignageIcon;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-createDevice-equipment-gl-transportSignage'] .equipment-list-item-title")]
        private IWebElement transportSignageLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-createDevice-equipment-gl-vehicle']")]
        private IWebElement vehicleButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-createDevice-equipment-gl-vehicle'] .equipment-list-item-icon")]
        private IWebElement vehicleIcon;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-createDevice-equipment-gl-vehicle'] .equipment-list-item-title")]
        private IWebElement vehicleLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-createDevice-equipment-gl-vehicleChargingStation']")]
        private IWebElement vehicleChargingStationButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-createDevice-equipment-gl-vehicleChargingStation'] .equipment-list-item-icon")]
        private IWebElement vehicleChargingStationIcon;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-createDevice-equipment-gl-vehicleChargingStation'] .equipment-list-item-title")]
        private IWebElement vehicleChargingStationLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-createDevice-equipment-gl-wasteContainer']")]
        private IWebElement wasteContainerButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-createDevice-equipment-gl-wasteContainer'] .equipment-list-item-icon")]
        private IWebElement wasteContainerIcon;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-createDevice-equipment-gl-wasteContainer'] .equipment-list-item-title")]
        private IWebElement wasteContainerLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-createDevice-equipment-gl-weatherStation']")]
        private IWebElement weatherStationButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-createDevice-equipment-gl-weatherStation'] .equipment-list-item-icon")]
        private IWebElement weatherStationIcon;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-createDevice-equipment-gl-weatherStation'] .equipment-list-item-title")]
        private IWebElement weatherStationLabel;

        #endregion //New device Menu

        #region New device Detail

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-createDevice-properties-category-icon']")]
        private IWebElement newDeviceIcon;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-createDevice-properties-category-label']")]
        private IWebElement newDeviceLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-createDevice-properties-name-label'] .label.equipment-gl-editor-label")]
        private IWebElement nameLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-createDevice-properties-name-field']")]
        private IWebElement nameInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-controllerStrId'] .slv-label.editor-label")]
        private IWebElement controllerIdLabel;

        [FindsBy(How = How.CssSelector, Using = "div[id$='editor-property-controllerStrId-field")]
        private IWebElement controllerIdDropDown;

        [FindsBy(How = How.CssSelector, Using = "input[id$='editor-property-controllerStrId-field")]
        private IWebElement controllerIdInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-idOnController'] .slv-label.editor-label")]
        private IWebElement identifierLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-idOnController-field")]
        private IWebElement identifierInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-controller-host'] .slv-label.editor-label")]
        private IWebElement newDeviceGatewayHostNameLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-controller-host-field")]
        private IWebElement newControllerGatewayHostNameInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-IPHost-field")]
        private IWebElement newDeviceGatewayHostNameInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-modelFunctionId'] .slv-label.editor-label")]
        private IWebElement typeEquipmentLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-modelFunctionId-field")]
        private IWebElement typeOfEquipmentDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-createDevice-properties-apply-button']")]
        private IWebElement positionDeviceButton;

        #endregion //New device Detail

        #endregion //IWebElements

        #region Constructor

        public NewDevicePanel(IWebDriver driver, PageBase page)
            : base(driver, page)
        {
            PageFactory.InitElements(this, new RetryingElementLocator(driver, TimeSpan.FromMilliseconds(Settings.LocatorRetryingTimeout), TimeSpan.FromMilliseconds(Settings.LocatorPollingInterval)));

            WaitForPanelLoaded();
        }

        #endregion //Constructor

        #region Basic methods

        #region Actions

        #region New device Menu

        /// <summary>
        /// Click 'Back' button
        /// </summary>
        public void ClickBackButton()
        {
            backButton.ClickEx();
        }

        /// <summary>
        /// Click 'Building' button
        /// </summary>
        public void ClickBuildingButton()
        {
            buildingButton.ClickEx();
        }

        /// <summary>
        /// Click 'CameraIp' button
        /// </summary>
        public void ClickCameraIpButton()
        {
            cameraIpButton.ClickEx();
        }

        /// <summary>
        /// Click 'CityObject' button
        /// </summary>
        public void ClickCityObjectButton()
        {
            cityObjectButton.ClickEx();
        }

        /// <summary>
        /// Click 'ElectricalCounter' button
        /// </summary>
        public void ClickElectricalCounterButton()
        {
            electricalCounterButton.ClickEx();
        }

        /// <summary>
        /// Click 'EnvSensor' button
        /// </summary>
        public void ClickEnvSensorButton()
        {
            envSensorButton.ClickEx();
        }

        /// <summary>
        /// Click 'Input' button
        /// </summary>
        public void ClickInputButton()
        {
            inputButton.ClickEx();
        }

        /// <summary>
        /// Click 'Nature' button
        /// </summary>
        public void ClickNatureButton()
        {
            natureButton.ClickEx();
        }

        /// <summary>
        /// Click 'NetworkComponent' button
        /// </summary>
        public void ClickNetworkComponentButton()
        {
            networkComponentButton.ClickEx();
        }

        /// <summary>
        /// Click 'OccupancySensor' button
        /// </summary>
        public void ClickOccupancySensorButton()
        {
            occupancySensorButton.ClickEx();
        }

        /// <summary>
        /// Click 'Output' button
        /// </summary>
        public void ClickOutputButton()
        {
            outputButton.ClickEx();
        }

        /// <summary>
        /// Click 'ParkingPlace' button
        /// </summary>
        public void ClickParkingPlaceButton()
        {
            parkingPlaceButton.ClickEx();
        }

        /// <summary>
        /// Click 'Streetlight' button
        /// </summary>
        public void ClickStreetlightButton()
        {
            streetlightButton.ClickEx();
        }

        /// <summary>
        /// Click 'SwitchDevice' button
        /// </summary>
        public void ClickSwitchDeviceButton()
        {
            switchDeviceButton.ClickEx();
        }

        /// <summary>
        /// Click 'Tank' button
        /// </summary>
        public void ClickTankButton()
        {
            tankButton.ClickEx();
        }

        /// <summary>
        /// Click 'TransportSignage' button
        /// </summary>
        public void ClickTransportSignageButton()
        {
            transportSignageButton.ClickEx();
        }

        /// <summary>
        /// Click 'Vehicle' button
        /// </summary>
        public void ClickVehicleButton()
        {
            vehicleButton.ClickEx();
        }

        /// <summary>
        /// Click 'VehicleChargingStation' button
        /// </summary>
        public void ClickVehicleChargingStationButton()
        {
            vehicleChargingStationButton.ClickEx();
        }

        /// <summary>
        /// Click 'WasteContainer' button
        /// </summary>
        public void ClickWasteContainerButton()
        {
            wasteContainerButton.ClickEx();
        }

        /// <summary>
        /// Click 'WeatherStation' button
        /// </summary>
        public void ClickWeatherStationButton()
        {
            weatherStationButton.ClickEx();
        }

        #endregion //New device Menu

        #region New device Detail

        /// <summary>
        /// Enter a value for 'Name' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterNameInput(string value)
        {
            nameInput.Enter(value);
        }

        /// <summary>
        /// Select an item of 'ControllerId' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectControllerIdDropDown(string value)
        {
            controllerIdDropDown.Select(value);
        }

        /// <summary>
        /// Enter a value for 'ControllerId' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterControllerIdInput(string value)
        {
            controllerIdInput.Enter(value);
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
        /// Enter a value for 'NewControllereGatewayHostName' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterNewControllerGatewayHostNameInput(string value)
        {
            newControllerGatewayHostNameInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'GatewayHostName' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterNewDeviceGatewayHostNameInput(string value)
        {
            newDeviceGatewayHostNameInput.Enter(value);
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
        /// Click 'PositionDevice' button
        /// </summary>
        public void ClickPositionDeviceButton()
        {
            positionDeviceButton.ClickEx();
        }

        #endregion //New device Detail

        #endregion //Actions

        #region Get methods

        #region New device Menu

        /// <summary>
        /// Get 'PanelTitle' text
        /// </summary>
        /// <returns></returns>
        public string GetPanelTitleText()
        {
            return panelTitle.Text;
        }

        /// <summary>
        /// Get 'BuildingIcon' input value
        /// </summary>
        /// <returns></returns>
        public string GetBuildingIconValue()
        {
            return buildingIcon.IconValue();
        }

        /// <summary>
        /// Get 'Building' label text
        /// </summary>
        /// <returns></returns>
        public string GetBuildingText()
        {
            return buildingLabel.Text;
        }

        /// <summary>
        /// Get 'CameraIpIcon' input value
        /// </summary>
        /// <returns></returns>
        public string GetCameraIpIconValue()
        {
            return cameraIpIcon.IconValue();
        }

        /// <summary>
        /// Get 'CameraIp' label text
        /// </summary>
        /// <returns></returns>
        public string GetCameraIpText()
        {
            return cameraIpLabel.Text;
        }

        /// <summary>
        /// Get 'CityObjectIcon' input value
        /// </summary>
        /// <returns></returns>
        public string GetCityObjectIconValue()
        {
            return cityObjectIcon.IconValue();
        }

        /// <summary>
        /// Get 'CityObject' label text
        /// </summary>
        /// <returns></returns>
        public string GetCityObjectText()
        {
            return cityObjectLabel.Text;
        }

        /// <summary>
        /// Get 'ElectricalCounterIcon' input value
        /// </summary>
        /// <returns></returns>
        public string GetElectricalCounterIconValue()
        {
            return electricalCounterIcon.IconValue();
        }

        /// <summary>
        /// Get 'ElectricalCounter' label text
        /// </summary>
        /// <returns></returns>
        public string GetElectricalCounterText()
        {
            return electricalCounterLabel.Text;
        }

        /// <summary>
        /// Get 'EnvSensorIcon' input value
        /// </summary>
        /// <returns></returns>
        public string GetEnvSensorIconValue()
        {
            return envSensorIcon.IconValue();
        }

        /// <summary>
        /// Get 'EnvSensor' label text
        /// </summary>
        /// <returns></returns>
        public string GetEnvSensorText()
        {
            return envSensorLabel.Text;
        }

        /// <summary>
        /// Get 'InputIcon' input value
        /// </summary>
        /// <returns></returns>
        public string GetInputIconValue()
        {
            return inputIcon.IconValue();
        }

        /// <summary>
        /// Get 'Input' label text
        /// </summary>
        /// <returns></returns>
        public string GetInputText()
        {
            return inputLabel.Text;
        }

        /// <summary>
        /// Get 'NatureIcon' input value
        /// </summary>
        /// <returns></returns>
        public string GetNatureIconValue()
        {
            return natureIcon.IconValue();
        }

        /// <summary>
        /// Get 'Nature' label text
        /// </summary>
        /// <returns></returns>
        public string GetNatureText()
        {
            return natureLabel.Text;
        }

        /// <summary>
        /// Get 'NetworkComponentIcon' input value
        /// </summary>
        /// <returns></returns>
        public string GetNetworkComponentIconValue()
        {
            return networkComponentIcon.IconValue();
        }

        /// <summary>
        /// Get 'NetworkComponent' label text
        /// </summary>
        /// <returns></returns>
        public string GetNetworkComponentText()
        {
            return networkComponentLabel.Text;
        }

        /// <summary>
        /// Get 'OccupancySensorIcon' input value
        /// </summary>
        /// <returns></returns>
        public string GetOccupancySensorIconValue()
        {
            return occupancySensorIcon.IconValue();
        }

        /// <summary>
        /// Get 'OccupancySensor' label text
        /// </summary>
        /// <returns></returns>
        public string GetOccupancySensorText()
        {
            return occupancySensorLabel.Text;
        }

        /// <summary>
        /// Get 'OutputIcon' input value
        /// </summary>
        /// <returns></returns>
        public string GetOutputIconValue()
        {
            return outputIcon.IconValue();
        }

        /// <summary>
        /// Get 'Output' label text
        /// </summary>
        /// <returns></returns>
        public string GetOutputText()
        {
            return outputLabel.Text;
        }

        /// <summary>
        /// Get 'ParkingPlaceIcon' input value
        /// </summary>
        /// <returns></returns>
        public string GetParkingPlaceIconValue()
        {
            return parkingPlaceIcon.IconValue();
        }

        /// <summary>
        /// Get 'ParkingPlace' label text
        /// </summary>
        /// <returns></returns>
        public string GetParkingPlaceText()
        {
            return parkingPlaceLabel.Text;
        }

        /// <summary>
        /// Get 'StreetlightIcon' input value
        /// </summary>
        /// <returns></returns>
        public string GetStreetlightIconValue()
        {
            return streetlightIcon.IconValue();
        }

        /// <summary>
        /// Get 'Streetlight' label text
        /// </summary>
        /// <returns></returns>
        public string GetStreetlightText()
        {
            return streetlightLabel.Text;
        }

        /// <summary>
        /// Get 'SwitchDeviceIcon' input value
        /// </summary>
        /// <returns></returns>
        public string GetSwitchDeviceIconValue()
        {
            return switchDeviceIcon.IconValue();
        }

        /// <summary>
        /// Get 'SwitchDevice' label text
        /// </summary>
        /// <returns></returns>
        public string GetSwitchDeviceText()
        {
            return switchDeviceLabel.Text;
        }

        /// <summary>
        /// Get 'TankIcon' input value
        /// </summary>
        /// <returns></returns>
        public string GetTankIconValue()
        {
            return tankIcon.IconValue();
        }

        /// <summary>
        /// Get 'Tank' label text
        /// </summary>
        /// <returns></returns>
        public string GetTankText()
        {
            return tankLabel.Text;
        }

        /// <summary>
        /// Get 'TransportSignageIcon' input value
        /// </summary>
        /// <returns></returns>
        public string GetTransportSignageIconValue()
        {
            return transportSignageIcon.IconValue();
        }

        /// <summary>
        /// Get 'TransportSignage' label text
        /// </summary>
        /// <returns></returns>
        public string GetTransportSignageText()
        {
            return transportSignageLabel.Text;
        }

        /// <summary>
        /// Get 'VehicleIcon' input value
        /// </summary>
        /// <returns></returns>
        public string GetVehicleIconValue()
        {
            return vehicleIcon.IconValue();
        }

        /// <summary>
        /// Get 'Vehicle' label text
        /// </summary>
        /// <returns></returns>
        public string GetVehicleText()
        {
            return vehicleLabel.Text;
        }

        /// <summary>
        /// Get 'VehicleChargingStationIcon' input value
        /// </summary>
        /// <returns></returns>
        public string GetVehicleChargingStationIconValue()
        {
            return vehicleChargingStationIcon.IconValue();
        }

        /// <summary>
        /// Get 'VehicleChargingStation' label text
        /// </summary>
        /// <returns></returns>
        public string GetVehicleChargingStationText()
        {
            return vehicleChargingStationLabel.Text;
        }

        /// <summary>
        /// Get 'WasteContainerIcon' input value
        /// </summary>
        /// <returns></returns>
        public string GetWasteContainerIconValue()
        {
            return wasteContainerIcon.IconValue();
        }

        /// <summary>
        /// Get 'WasteContainer' label text
        /// </summary>
        /// <returns></returns>
        public string GetWasteContainerText()
        {
            return wasteContainerLabel.Text;
        }

        /// <summary>
        /// Get 'WeatherStationIcon' input value
        /// </summary>
        /// <returns></returns>
        public string GetWeatherStationIconValue()
        {
            return weatherStationIcon.IconValue();
        }

        /// <summary>
        /// Get 'WeatherStation' label text
        /// </summary>
        /// <returns></returns>
        public string GetWeatherStationText()
        {
            return weatherStationLabel.Text;
        }

        #endregion //New device Menu

        #region New device Detail

        /// <summary>
        /// Get 'NewDeviceIcon' input value
        /// </summary>
        /// <returns></returns>
        public string GetNewDeviceIconValue()
        {
            return newDeviceIcon.IconValue();
        }

        /// <summary>
        /// Get 'NewDevice' label text
        /// </summary>
        /// <returns></returns>
        public string GetNewDeviceText()
        {
            return newDeviceLabel.Text;
        }

        /// <summary>
        /// Get 'Name' label text
        /// </summary>
        /// <returns></returns>
        public string GetNameText()
        {
            return nameLabel.Text;
        }

        /// <summary>
        /// Get 'Name' input value
        /// </summary>
        /// <returns></returns>
        public string GetNameValue()
        {
            return nameInput.Value();
        }

        /// <summary>
        /// Get 'ControllerId' label text
        /// </summary>
        /// <returns></returns>
        public string GetControllerIdText()
        {
            return controllerIdLabel.Text;
        }

        /// <summary>
        /// Get 'ControllerId' input value
        /// </summary>
        /// <returns></returns>
        public string GetNewDeviceControllerIdValue()
        {
            if(ElementUtility.IsDisplayed(By.CssSelector("div[id$='editor-property-controllerStrId-field")))            
                return controllerIdDropDown.Text;
            return controllerIdInput.Value();
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
        /// Get 'Identifier' input value
        /// </summary>
        /// <returns></returns>
        public string GetIdentifierValue()
        {
            return identifierInput.Value();
        }

        /// <summary>
        /// Get 'NewDeviceGatewayHostName' label text
        /// </summary>
        /// <returns></returns>
        public string GetNewDeviceGatewayHostNameText()
        {
            return newDeviceGatewayHostNameLabel.Text;
        }

        /// <summary>
        /// Get 'NewControllerGatewayHostName' input value
        /// </summary>
        /// <returns></returns>
        public string GetNewControllerGatewayHostNameValue()
        {
            return newControllerGatewayHostNameInput.Value();
        }

        /// <summary>
        /// Get 'NewDeviceGatewayHostName' input value
        /// </summary>
        /// <returns></returns>
        public string GetNewDeviceGatewayHostNameValue()
        {
            return newDeviceGatewayHostNameInput.Value();
        }

        /// <summary>
        /// Get 'TypeEquipment' label text
        /// </summary>
        /// <returns></returns>
        public string GetTypeEquipmentText()
        {
            return typeEquipmentLabel.Text;
        }

        /// <summary>
        /// Get 'NewDeviceTypeOfEquipment' input value
        /// </summary>
        /// <returns></returns>
        public string GetNewDeviceTypeOfEquipmentValue()
        {
            return typeOfEquipmentDropDown.Text;
        }

        #endregion //New device Detail

        #endregion //Get methods

        #endregion //Basic methods

        #region Business methods

        public void SelectDevice(DeviceType deviceType)
        {
            var device = deviceList.FirstOrDefault(p => p.Text.Equals(deviceType.Value));
            device.ClickEx();
        }

        public List<string> GetListOfDeviceTypes()
        {
            return deviceList.Select(p => p.Text).ToList();            
        }

        public byte[] GetNewDeviceIconBytes()
        {
            var backgroundImg = newDeviceIcon.GetStyleAttr("background-image");
            var url = backgroundImg.Replace("url(\"", string.Empty).Replace("\")", string.Empty);

            return SLVHelper.DownloadFileData(url);
        }

        public bool CheckIfDeviceIcon(DeviceType device)
        {
            var expectedIcon = new MagickImage(device.GetIconBytes());
            var actualIcon = new MagickImage(GetNewDeviceIconBytes());
            var result = expectedIcon.Compare(actualIcon, ErrorMetric.Absolute);

            return result == 0;
        }

        public string GetPositionDeviceButtonText()
        {
            return positionDeviceButton.Text;
        }

        public bool IsNameInputReadOnly()
        {
            return nameInput.IsReadOnly();
        }

        public bool IsControllerIdInputReadOnly()
        {
            return controllerIdInput.IsReadOnly();
        }

        public bool IsControllerIdInputDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("input[id$='editor-property-controllerStrId-field"));
        }
        
        public bool IsControllerIdDropDownReadOnly()
        {
            return controllerIdDropDown.IsReadOnly(false);
        }

        public bool IsControllerIdDropDownSelectable()
        {
            return controllerIdDropDown.GetAllItems().Any();
        }

        public bool IsGatewayHostNameInputReadOnly()
        {
            return newControllerGatewayHostNameInput.IsReadOnly();
        }

        public bool IsIdentifierInputReadOnly()
        {
            return identifierInput.IsReadOnly();
        }

        public bool IsTypeOfEquipmentDropDownReadOnly()
        {
            return typeOfEquipmentDropDown.IsReadOnly(false);
        }

        public bool IsTypeOfEquipmentDropDownSelectable()
        {
            return typeOfEquipmentDropDown.GetAllItems().Any();
        }      

        public bool IsPositionDeviceButtonDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-geozone-createDevice-properties-apply-button']"));
        }

        public void WaitForNewDevicePropertiesSectionDisplayed()
        {
            Wait.ForElementDisplayed(By.CssSelector("[id$='editor-geozone-createDevice-properties']"));
            Wait.ForElementStyle(By.CssSelector("[id$='editor-geozone-createDevice-properties']"), "left: 0px");
        }

        public void WaitForNewDevicePropertiesSectionDisappeared()
        {
            Wait.ForElementStyle(By.CssSelector("[id$='editor-geozone-createDevice-properties']"), "left: 350px");
        }

        public bool IsNewDevicePropertiesSectionDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-geozone-createDevice-properties']"));
        }

        public void SelectRandomTypeOfEquipmentDropDown()
        {
            var currentValue = GetNewDeviceTypeOfEquipmentValue();
            var listItems = typeOfEquipmentDropDown.GetAllItems();
            listItems.Remove(currentValue);
            typeOfEquipmentDropDown.Select(listItems.PickRandom());
        }

        public List<string> GetListOfTypeOfEquipment()
        {
            return typeOfEquipmentDropDown.GetAllItems();
        }

        /// <summary>
        /// Enter a value for 'HostName' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterHostNameInput(DeviceType deviceType, string value)
        {
            if (deviceType.Equals(DeviceType.Controller))
            {
                EnterNewControllerGatewayHostNameInput(value);
            }
            else
            {
                EnterNewDeviceGatewayHostNameInput(value);
            }
        }

        #endregion //Business methods

        public override void WaitForPanelLoaded()
        {
            Wait.ForElementStyle(By.CssSelector("[id$='editor-geozone-createDevice']"), "left: 0px");
        }
    }
}
