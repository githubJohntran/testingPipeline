using NUnit.Framework;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using StreetlightVision.Extensions;
using StreetlightVision.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace StreetlightVision.Pages.UI
{
    public class BackOfficeDetailsPanel : PanelBase
    {
        #region Variables        

        #endregion //Variables

        #region IWebElements

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-container'] [id='slv-view-backoffice-editor-title']")]
        private IWebElement panelTitle;

        [FindsBy(How = How.CssSelector, Using = "[id='tb_backOfficeToolbar_item_save'] table.w2ui-button")]
        private IWebElement saveButton;

        #region General

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-General-timeoutAuthentication'] .backoffice-configuration-field-label")]
        private IWebElement generalTimeoutAuthenticationLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-General-timeoutAuthentication'] input.editor-field")]
        private IWebElement generalTimeoutAuthenticationNumericInput;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-General-timeoutAuthentication'] .slv-description")]
        private IWebElement generalTimeoutAuthenticationDescLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-General'] .backoffice-configuration-field-title:nth-child(2)")]
        private IWebElement generalDefaultReportConfigurationLabel;        

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-General-gridRowMaxCount'] .backoffice-configuration-field-label")]
        private IWebElement generalGridRowMaxCountLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-General-gridRowMaxCount'] input.editor-field")]
        private IWebElement generalGridRowMaxCountNumericInput;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-General-gridRowMaxCount'] .slv-description")]
        private IWebElement generalGridRowMaxCountDescLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-General-exportCsvSeparator'] .backoffice-configuration-field-label")]
        private IWebElement generalExportCsvSeparatorLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-General-exportCsvSeparator'] input.editor-field")]
        private IWebElement generalExportCsvSeparatorInput;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-General-exportCsvSeparator'] .slv-description")]
        private IWebElement generalExportCsvSeparatorDescLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-General'] .backoffice-configuration-field-title:nth-child(4)")]
        private IWebElement generalDefaultSearchConfigurationLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-General-searchAttributs'] .slv-description")]
        private IWebElement generalSearchAttributesDescLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-General-searchAttributs'] .backoffice-configuration-field-label")]
        private IWebElement generalSearchAttributesLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-General-searchAttributs'] input.editor-field")]
        private IWebElement generalSearchAttributesInput;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-General-searchAttributs'] button.icon-add")]
        private IWebElement generalSearchAttributesAddButton;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-General-searchAttributs'] .backoffice-configuration-field-list-item")]
        private IList<IWebElement> generalSearchAttributesList;

        #endregion //General

        #region Desktop

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-Desktop-appList'] .slv-description")]
        private IWebElement desktopApplicationsListDescLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-Desktop-appList'] .backoffice-configuration-field-label")]
        private IWebElement desktopApplicationsListLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-Desktop-appList'] .backoffice-configuration-field-list-item")]
        private IList<IWebElement> desktopApplicationsList;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-Desktop-appList'] .backoffice-configuration-field-list-item:not(.backoffice-configuration-field-list-item-disable)")]
        private IList<IWebElement> desktopAvailableApplicationsList;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-Desktop-appList'] .backoffice-configuration-field-list-item-disable")]
        private IList<IWebElement> desktopDisableApplicationsList;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-Desktop-widgetList'] .slv-description")]
        private IWebElement desktopWidgetsListDescLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-Desktop-widgetList'] .backoffice-configuration-field-label")]
        private IWebElement desktopWidgetsListLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-Desktop-widgetList'] .backoffice-configuration-field-list-item")]
        private IList<IWebElement> desktopWidgetsList;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-Desktop-widgetList'] .backoffice-configuration-field-list-item:not(.backoffice-configuration-field-list-item-disable)")]
        private IList<IWebElement> desktopAvailableWidgetsList;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-Desktop-widgetList'] .backoffice-configuration-field-list-item-disable")]
        private IList<IWebElement> desktopDisableWidgetsList;

        #endregion //Desktop

        #region Equipment Inventory       

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-EquipmentGL'] .backoffice-configuration-field-title:nth-child(1)")]
        private IWebElement equipmentTreeviewConfigurationLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-EquipmentGL-loadDevices'] .backoffice-configuration-field-label")]
        private IWebElement equipmentDisplayDevicesLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-EquipmentGL-loadDevices'] .checkbox")]
        private IWebElement equipmentDisplayDevicesCheckbox;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-EquipmentGL-loadDevices'] .slv-description")]
        private IWebElement equipmentDisplayDevicesDescLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-EquipmentGL-searchMapFilter'] .backoffice-configuration-field-label")]
        private IWebElement equipmentMapFilterLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-EquipmentGL-searchMapFilter'] .checkbox")]
        private IWebElement equipmentMapFilterCheckbox;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-EquipmentGL-searchMapFilter'] .slv-description")]
        private IWebElement equipmentMapFilterDescLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-EquipmentGL'] .backoffice-configuration-field-title:nth-child(5)")]
        private IWebElement equipmentEditorConfigurationLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-EquipmentGL-enableDeviceLocation'] .backoffice-configuration-field-label")]
        private IWebElement equipmentDeviceLocationLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-EquipmentGL-enableDeviceLocation'] .checkbox")]
        private IWebElement equipmentDeviceLocationCheckbox;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-EquipmentGL-enableDeviceLocation'] .slv-description")]
        private IWebElement equipmentDeviceLocationDescLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-EquipmentGL-enableGeozoneParent'] .backoffice-configuration-field-label")]
        private IWebElement equipmentParentGeozoneLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-EquipmentGL-enableGeozoneParent'] .checkbox")]
        private IWebElement equipmentParentGeozoneCheckbox;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-EquipmentGL-enableGeozoneParent'] .slv-description")]
        private IWebElement equipmentParentGeozoneDescLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-EquipmentGL'] .backoffice-configuration-field-title:nth-child(8)")]
        private IWebElement equipmentReportConfigurationLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-EquipmentGL-eventTimeVisible'] .backoffice-configuration-field-label")]
        private IWebElement equipmentEventTimeVisibleLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-EquipmentGL-eventTimeVisible'] .checkbox")]
        private IWebElement equipmentEventTimeVisibleCheckbox;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-EquipmentGL-eventTimeVisible'] .slv-description")]
        private IWebElement equipmentEventTimeVisibleDescLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-EquipmentGL-rowsPerPage'] .backoffice-configuration-field-label")]
        private IWebElement equipmentRowsPerPageLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-EquipmentGL-rowsPerPage'] input.editor-field")]
        private IWebElement equipmentRowsPerPageNumericInput;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-EquipmentGL-rowsPerPage'] .slv-description")]
        private IWebElement equipmentRowsPerPageDescLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-EquipmentGL-toolbarItems'] .slv-description")]
        private IWebElement equipmentToolbarItemsDescLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-EquipmentGL-toolbarItems'] .backoffice-configuration-field-label")]
        private IWebElement equipmentToolbarItemsLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-EquipmentGL-toolbarItems'] .backoffice-configuration-field-list-item")]
        private IList<IWebElement> equipmentToolbarItemsList;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-EquipmentGL-toolbarItems'] .backoffice-configuration-field-list-item:not(.backoffice-configuration-field-list-item-disable)")]
        private IList<IWebElement> equipmentAvailableToolbarItemsList;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-EquipmentGL-toolbarItems'] .backoffice-configuration-field-list-item-disable")]
        private IList<IWebElement> equipmentDisableToolbarItemsList;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-EquipmentGL-searchAttributs'] .slv-description")]
        private IWebElement equipmentSearchAttributesDescLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-EquipmentGL-searchAttributs'] .backoffice-configuration-field-label")]
        private IWebElement equipmentSearchAttributesLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-EquipmentGL-searchAttributs'] input.editor-field")]
        private IWebElement equipmentSearchAttributesInput;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-EquipmentGL-searchAttributs'] button.icon-add")]
        private IWebElement equipmentSearchAttributesAddButton;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-EquipmentGL-searchAttributs'] .backoffice-configuration-field-list-item")]
        private IList<IWebElement> equipmentSearchAttributesList;

        #endregion //Equipment Inventory

        #region Real-time Control     

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-RealTimeGL'] .backoffice-configuration-field-title:nth-child(1)")]
        private IWebElement realtimeTreeviewConfigurationLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-RealTimeGL-loadDevices'] .backoffice-configuration-field-label")]
        private IWebElement realtimeDisplayDevicesLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-RealTimeGL-loadDevices'] .checkbox")]
        private IWebElement realtimeDisplayDevicesCheckbox;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-RealTimeGL-loadDevices'] .slv-description")]
        private IWebElement realtimeDisplayDevicesDescLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-RealTimeGL-searchMapFilter'] .backoffice-configuration-field-label")]
        private IWebElement realtimeMapFilterLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-RealTimeGL-searchMapFilter'] .checkbox")]
        private IWebElement realtimeMapFilterCheckbox;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-RealTimeGL-searchMapFilter'] .slv-description")]
        private IWebElement realtimeMapFilterDescLabel;

        #endregion //Real-time Control

        #region Data history       

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-DataHistory'] .backoffice-configuration-field-title:nth-child(1)")]
        private IWebElement dataHistoryTreeviewConfigurationLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-DataHistory-loadDevices'] .backoffice-configuration-field-label")]
        private IWebElement dataHistoryDisplayDevicesLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-DataHistory-loadDevices'] .checkbox")]
        private IWebElement dataHistoryDisplayDevicesCheckbox;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-DataHistory-loadDevices'] .slv-description")]
        private IWebElement dataHistoryDisplayDevicesDescLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-DataHistory'] .backoffice-configuration-field-title:nth-child(3)")]
        private IWebElement dataHistoryReportConfigurationLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-DataHistory-eventTimeVisible'] .backoffice-configuration-field-label")]
        private IWebElement dataHistoryEventTimeVisibleLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-DataHistory-eventTimeVisible'] .checkbox")]
        private IWebElement dataHistoryEventTimeVisibleCheckbox;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-DataHistory-eventTimeVisible'] .slv-description")]
        private IWebElement dataHistoryEventTimeVisibleDescLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-DataHistory-rowsPerPage'] .backoffice-configuration-field-label")]
        private IWebElement dataHistoryRowsPerPageLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-DataHistory-rowsPerPage'] input.editor-field")]
        private IWebElement dataHistoryRowsPerPageNumericInput;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-DataHistory-rowsPerPage'] .slv-description")]
        private IWebElement dataHistoryRowsPerPageDescLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-DataHistory-toolbarItems'] .slv-description")]
        private IWebElement dataHistoryToolbarItemsDescLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-DataHistory-toolbarItems'] .backoffice-configuration-field-label")]
        private IWebElement dataHistoryToolbarItemsLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-DataHistory-toolbarItems'] .backoffice-configuration-field-list-item")]
        private IList<IWebElement> dataHistoryToolbarItemsList;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-DataHistory-toolbarItems'] .backoffice-configuration-field-list-item:not(.backoffice-configuration-field-list-item-disable)")]
        private IList<IWebElement> dataHistoryAvailableToolbarItemsList;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-DataHistory-toolbarItems'] .backoffice-configuration-field-list-item-disable")]
        private IList<IWebElement> dataHistoryDisableToolbarItemsList;

        #endregion //Data history

        #region Device History      

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-DeviceHistory'] .backoffice-configuration-field-title:nth-child(1)")]
        private IWebElement deviceHistoryTreeviewConfigurationLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-DeviceHistory-loadDevices'] .backoffice-configuration-field-label")]
        private IWebElement deviceHistoryDisplayDevicesLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-DeviceHistory-loadDevices'] .checkbox")]
        private IWebElement deviceHistoryDisplayDevicesCheckbox;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-DeviceHistory-loadDevices'] .slv-description")]
        private IWebElement deviceHistoryDisplayDevicesDescLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-DeviceHistory'] .backoffice-configuration-field-title:nth-child(3)")]
        private IWebElement deviceHistoryReportConfigurationLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-DeviceHistory-eventTimeVisible'] .backoffice-configuration-field-label")]
        private IWebElement deviceHistoryEventTimeVisibleLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-DeviceHistory-eventTimeVisible'] .checkbox")]
        private IWebElement deviceHistoryEventTimeVisibleCheckbox;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-DeviceHistory-eventTimeVisible'] .slv-description")]
        private IWebElement deviceHistoryEventTimeVisibleDescLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-DeviceHistory-rowsPerPage'] .backoffice-configuration-field-label")]
        private IWebElement deviceHistoryRowsPerPageLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-DeviceHistory-rowsPerPage'] input.editor-field")]
        private IWebElement deviceHistoryRowsPerPageNumericInput;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-DeviceHistory-rowsPerPage'] .slv-description")]
        private IWebElement deviceHistoryRowsPerPageDescLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-DeviceHistory-toolbarItems'] .slv-description")]
        private IWebElement deviceHistoryToolbarItemsDescLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-DeviceHistory-toolbarItems'] .backoffice-configuration-field-label")]
        private IWebElement deviceHistoryToolbarItemsLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-DeviceHistory-toolbarItems'] .backoffice-configuration-field-list-item")]
        private IList<IWebElement> deviceHistoryToolbarItemsList;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-DeviceHistory-toolbarItems'] .backoffice-configuration-field-list-item:not(.backoffice-configuration-field-list-item-disable)")]
        private IList<IWebElement> deviceHistoryAvailableToolbarItemsList;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-DeviceHistory-toolbarItems'] .backoffice-configuration-field-list-item-disable")]
        private IList<IWebElement> deviceHistoryDisableToolbarItemsList;

        #endregion //Device History

        #region Failure Analysis

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-Failure'] .backoffice-configuration-field-title:nth-child(1)")]
        private IWebElement failureAnalysisTreeviewConfigurationLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-Failure-loadDevices'] .backoffice-configuration-field-label")]
        private IWebElement failureAnalysisDisplayDevicesLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-Failure-loadDevices'] .checkbox")]
        private IWebElement failureAnalysisDisplayDevicesCheckbox;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-Failure-loadDevices'] .slv-description")]
        private IWebElement failureAnalysisDisplayDevicesDescLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-Failure-searchAttributs'] .slv-description")]
        private IWebElement failureAnalysisSearchAttributesDescLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-Failure-searchAttributs'] .backoffice-configuration-field-label")]
        private IWebElement failureAnalysisSearchAttributesLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-Failure-searchAttributs'] input.editor-field")]
        private IWebElement failureAnalysisSearchAttributesInput;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-Failure-searchAttributs'] button.icon-add")]
        private IWebElement failureAnalysisSearchAttributesAddButton;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-Failure-searchAttributs'] .backoffice-configuration-field-list-item")]
        private IList<IWebElement> failureAnalysisSearchAttributesList;

        #endregion //Failure Analysis

        #region Failure Tracking

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-FailureTrackingGL'] .backoffice-configuration-field-title:nth-child(1)")]
        private IWebElement failureTrackingTreeviewConfigurationLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-FailureTrackingGL-loadDevices'] .backoffice-configuration-field-label")]
        private IWebElement failureTrackingDisplayDevicesLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-FailureTrackingGL-loadDevices'] .checkbox")]
        private IWebElement failureTrackingDisplayDevicesCheckbox;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-FailureTrackingGL-loadDevices'] .slv-description")]
        private IWebElement failureTrackingDisplayDevicesDescLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-FailureTrackingGL-searchMapFilter'] .backoffice-configuration-field-label")]
        private IWebElement failureTrackingMapFilterLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-FailureTrackingGL-searchMapFilter'] .checkbox")]
        private IWebElement failureTrackingMapFilterCheckbox;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-FailureTrackingGL-searchMapFilter'] .slv-description")]
        private IWebElement failureTrackingMapFilterDescLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-FailureTrackingGL-searchAttributs'] .slv-description")]
        private IWebElement failureTrackingSearchAttributesDescLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-FailureTrackingGL-searchAttributs'] .backoffice-configuration-field-label")]
        private IWebElement failureTrackingSearchAttributesLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-FailureTrackingGL-searchAttributs'] input.editor-field")]
        private IWebElement failureTrackingSearchAttributesInput;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-FailureTrackingGL-searchAttributs'] button.icon-add")]
        private IWebElement failureTrackingSearchAttributesAddButton;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-FailureTrackingGL-searchAttributs'] .backoffice-configuration-field-list-item")]
        private IList<IWebElement> failureTrackingSearchAttributesList;

        #endregion //Failure Tracking

        #region Advanced Search

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-CustomReport'] .backoffice-configuration-field-title:nth-child(1)")]
        private IWebElement advancedSearchReportConfigurationLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-CustomReport-eventTimeVisible'] .backoffice-configuration-field-label")]
        private IWebElement advancedSearchEventTimeVisibleLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-CustomReport-eventTimeVisible'] .checkbox")]
        private IWebElement advancedSearchEventTimeVisibleCheckbox;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-CustomReport-eventTimeVisible'] .slv-description")]
        private IWebElement advancedSearchEventTimeVisibleDescLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-CustomReport-rowsPerPage'] .backoffice-configuration-field-label")]
        private IWebElement advancedSearchRowsPerPageLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-CustomReport-rowsPerPage'] input.editor-field")]
        private IWebElement advancedSearchRowsPerPageNumericInput;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-CustomReport-rowsPerPage'] .slv-description")]
        private IWebElement advancedSearchRowsPerPageDescLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-CustomReport-toolbarItems'] .slv-description")]
        private IWebElement advancedSearchToolbarItemsDescLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-CustomReport-toolbarItems'] .backoffice-configuration-field-label")]
        private IWebElement advancedSearchToolbarItemsLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-CustomReport-toolbarItems'] .backoffice-configuration-field-list-item")]
        private IList<IWebElement> advancedSearchToolbarItemsList;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-CustomReport-toolbarItems'] .backoffice-configuration-field-list-item:not(.backoffice-configuration-field-list-item-disable)")]
        private IList<IWebElement> advancedSearchAvailableToolbarItemsList;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-editor-editor-CustomReport-toolbarItems'] .backoffice-configuration-field-list-item-disable")]
        private IList<IWebElement> advancedSearchDisableToolbarItemsList;

        #endregion //Advanced Search

        #endregion //IWebElements

        #region Constructor

        public BackOfficeDetailsPanel(IWebDriver driver, PageBase page)
            : base(driver, page)
        {
            PageFactory.InitElements(this, new RetryingElementLocator(driver, TimeSpan.FromMilliseconds(Settings.LocatorRetryingTimeout), TimeSpan.FromMilliseconds(Settings.LocatorPollingInterval)));

            WaitForPanelLoaded();
        }

        #endregion //Constructor

        #region Properties        

        #endregion

        #region Basic methods  

        #region Actions

        /// <summary>
        /// Click 'Save' button
        /// </summary>
        public void ClickSaveButton()
        {
            saveButton.ClickEx();
        }

        #region General

        /// <summary>
        /// Enter a value for 'GeneralTimeoutAuthentication' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterGeneralTimeoutAuthenticationNumericInput(string value)
        {
            generalTimeoutAuthenticationNumericInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'GeneralGridRowMaxCount' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterGeneralGridRowMaxCountNumericInput(string value)
        {
            generalGridRowMaxCountNumericInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'GeneralExportCsvSeparator' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterGeneralExportCsvSeparatorInput(string value)
        {
            generalExportCsvSeparatorInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'GeneralSearchAttributs' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterGeneralSearchAttributsInput(string value)
        {
            generalSearchAttributesInput.Enter(value);
        }

        /// <summary>
        /// Click 'GeneralSearchAttributsAdd' button
        /// </summary>
        public void ClickGeneralSearchAttributsAddButton()
        {
            generalSearchAttributesAddButton.ClickEx();
        }

        #endregion //General

        #region Desktop
        
        #endregion //Desktop

        #region Equipment Inventory       

        /// <summary>
        /// Tick 'EquipmentDisplayDevices' checkbox
        /// </summary>
        /// <param name="value"></param>
        public void TickEquipmentDisplayDevicesCheckbox(bool value)
        {
            equipmentDisplayDevicesCheckbox.Check(value);
        }

        /// <summary>
        /// Tick 'EquipmentMapFilter' checkbox
        /// </summary>
        /// <param name="value"></param>
        public void TickEquipmentMapFilterCheckbox(bool value)
        {
            equipmentMapFilterCheckbox.Check(value);
        }

        /// <summary>
        /// Tick 'EquipmentDeviceLocation' checkbox
        /// </summary>
        /// <param name="value"></param>
        public void TickEquipmentDeviceLocationCheckbox(bool value)
        {
            equipmentDeviceLocationCheckbox.Check(value);
        }

        /// <summary>
        /// Tick 'EquipmentParentGeozone' checkbox
        /// </summary>
        /// <param name="value"></param>
        public void TickEquipmentParentGeozoneCheckbox(bool value)
        {
            equipmentParentGeozoneCheckbox.Check(value);
        }

        /// <summary>
        /// Tick 'EquipmentEventTimeVisible' checkbox
        /// </summary>
        /// <param name="value"></param>
        public void TickEquipmentEventTimeVisibleCheckbox(bool value)
        {
            equipmentEventTimeVisibleCheckbox.Check(value);
        }

        /// <summary>
        /// Enter a value for 'EquipmentRowsPerPage' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterEquipmentRowsPerPageNumericInput(string value)
        {
            equipmentRowsPerPageNumericInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'EquipmentSearchAttributes' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterEquipmentSearchAttributesInput(string value)
        {
            equipmentSearchAttributesInput.Enter(value);
        }

        /// <summary>
        /// Click 'EquipmentSearchAttributesAdd' button
        /// </summary>
        public void ClickEquipmentSearchAttributesAddButton()
        {
            equipmentSearchAttributesAddButton.ClickEx();
        }

        #endregion //Equipment Inventory

        #region Real-time Control     

        /// <summary>
        /// Tick 'RealtimeDisplayDevices' checkbox
        /// </summary>
        /// <param name="value"></param>
        public void TickRealtimeDisplayDevicesCheckbox(bool value)
        {
            realtimeDisplayDevicesCheckbox.Check(value);
        }

        /// <summary>
        /// Tick 'RealtimeMapFilter' checkbox
        /// </summary>
        /// <param name="value"></param>
        public void TickRealtimeMapFilterCheckbox(bool value)
        {
            realtimeMapFilterCheckbox.Check(value);
        }

        #endregion //Real-time Control

        #region Data history       

        /// <summary>
        /// Tick 'DataHistoryDisplayDevices' checkbox
        /// </summary>
        /// <param name="value"></param>
        public void TickDataHistoryDisplayDevicesCheckbox(bool value)
        {
            dataHistoryDisplayDevicesCheckbox.Check(value);
        }

        /// <summary>
        /// Tick 'DataHistoryEventTimeVisible' checkbox
        /// </summary>
        /// <param name="value"></param>
        public void TickDataHistoryEventTimeVisibleCheckbox(bool value)
        {
            dataHistoryEventTimeVisibleCheckbox.Check(value);
        }

        /// <summary>
        /// Enter a value for 'DataHistoryRowsPerPage' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterDataHistoryRowsPerPageNumericInput(string value)
        {
            dataHistoryRowsPerPageNumericInput.Enter(value);
        }

        #endregion //Data history

        #region Device History      

        /// <summary>
        /// Tick 'DeviceHistoryDisplayDevices' checkbox
        /// </summary>
        /// <param name="value"></param>
        public void TickDeviceHistoryDisplayDevicesCheckbox(bool value)
        {
            deviceHistoryDisplayDevicesCheckbox.Check(value);
        }

        /// <summary>
        /// Tick 'DeviceHistoryEventTimeVisible' checkbox
        /// </summary>
        /// <param name="value"></param>
        public void TickDeviceHistoryEventTimeVisibleCheckbox(bool value)
        {
            deviceHistoryEventTimeVisibleCheckbox.Check(value);
        }

        /// <summary>
        /// Enter a value for 'DeviceHistoryRowsPerPage' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterDeviceHistoryRowsPerPageNumericInput(string value)
        {
            deviceHistoryRowsPerPageNumericInput.Enter(value);
        }

        #endregion //Device History

        #region Failure Analysis

        /// <summary>
        /// Tick 'FailureAnalysisDisplayDevices' checkbox
        /// </summary>
        /// <param name="value"></param>
        public void TickFailureAnalysisDisplayDevicesCheckbox(bool value)
        {
            failureAnalysisDisplayDevicesCheckbox.Check(value);
        }
        
        /// <summary>
        /// Enter a value for 'FailureAnalysisSearchAttributs' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterFailureAnalysisSearchAttributsInput(string value)
        {
            failureAnalysisSearchAttributesInput.Enter(value);
        }

        /// <summary>
        /// Click 'FailureAnalysisSearchAttributesAdd' button
        /// </summary>
        public void ClickFailureAnalysisSearchAttributesAddButton()
        {
            failureAnalysisSearchAttributesAddButton.ClickEx();
        }

        #endregion //Failure Analysis

        #region Failure Tracking

        /// <summary>
        /// Tick 'FailureTrackingDisplayDevices' checkbox
        /// </summary>
        /// <param name="value"></param>
        public void TickFailureTrackingDisplayDevicesCheckbox(bool value)
        {
            failureTrackingDisplayDevicesCheckbox.Check(value);
        }

        /// <summary>
        /// Tick 'FailureTrackingMapFilter' checkbox
        /// </summary>
        /// <param name="value"></param>
        public void TickFailureTrackingMapFilterCheckbox(bool value)
        {
            failureTrackingMapFilterCheckbox.Check(value);
        }

        /// <summary>
        /// Enter a value for 'FailureTrackingSearchAttributs' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterFailureTrackingSearchAttributesInput(string value)
        {
            failureTrackingSearchAttributesInput.Enter(value);
        }

        /// <summary>
        /// Click 'FailureTrackingSearchAttributesAdd' button
        /// </summary>
        public void ClickFailureTrackingSearchAttributesAddButton()
        {
            failureTrackingSearchAttributesAddButton.ClickEx();
        }

        #endregion //Failure Tracking

        #region Advanced Search

        /// <summary>
        /// Tick 'AdvancedSearchEventTimeVisible' checkbox
        /// </summary>
        /// <param name="value"></param>
        public void TickAdvancedSearchEventTimeVisibleCheckbox(bool value)
        {
            advancedSearchEventTimeVisibleCheckbox.Check(value);
        }

        /// <summary>
        /// Enter a value for 'AdvancedSearchRowsPerPage' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterAdvancedSearchRowsPerPageNumericInput(string value)
        {
            advancedSearchRowsPerPageNumericInput.Enter(value);
        }

        #endregion //Advanced Search

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

        #region General

        /// <summary>
        /// Get 'GeneralTimeoutAuthentication' label text
        /// </summary>
        /// <returns></returns>
        public string GetGeneralTimeoutAuthenticationText()
        {
            return generalTimeoutAuthenticationLabel.Text;
        }

        /// <summary>
        /// Get 'GeneralTimeoutAuthentication' input value
        /// </summary>
        /// <returns></returns>
        public string GetGeneralTimeoutAuthenticationValue()
        {
            return generalTimeoutAuthenticationNumericInput.Value();
        }

        /// <summary>
        /// Get 'GeneralTimeoutAuthenticationDesc' label text
        /// </summary>
        /// <returns></returns>
        public string GetGeneralTimeoutAuthenticationDescText()
        {
            return generalTimeoutAuthenticationDescLabel.Text;
        }

        /// <summary>
        /// Get 'GeneralDefaultReportConfiguration' label text
        /// </summary>
        /// <returns></returns>
        public string GetGeneralDefaultReportConfigurationText()
        {
            return generalDefaultReportConfigurationLabel.Text;
        }

        /// <summary>
        /// Get 'GeneralGridRowMaxCount' label text
        /// </summary>
        /// <returns></returns>
        public string GetGeneralGridRowMaxCountText()
        {
            return generalGridRowMaxCountLabel.Text;
        }

        /// <summary>
        /// Get 'GeneralGridRowMaxCount' input value
        /// </summary>
        /// <returns></returns>
        public string GetGeneralGridRowMaxCountValue()
        {
            return generalGridRowMaxCountNumericInput.Value();
        }

        /// <summary>
        /// Get 'GeneralGridRowMaxCountDesc' label text
        /// </summary>
        /// <returns></returns>
        public string GetGeneralGridRowMaxCountDescText()
        {
            return generalGridRowMaxCountDescLabel.Text;
        }

        /// <summary>
        /// Get 'GeneralExportCsvSeparator' label text
        /// </summary>
        /// <returns></returns>
        public string GetGeneralExportCsvSeparatorText()
        {
            return generalExportCsvSeparatorLabel.Text;
        }

        /// <summary>
        /// Get 'GeneralExportCsvSeparator' input value
        /// </summary>
        /// <returns></returns>
        public string GetGeneralExportCsvSeparatorValue()
        {
            return generalExportCsvSeparatorInput.Value();
        }

        /// <summary>
        /// Get 'GeneralExportCsvSeparatorDesc' label text
        /// </summary>
        /// <returns></returns>
        public string GetGeneralExportCsvSeparatorDescText()
        {
            return generalExportCsvSeparatorDescLabel.Text;
        }

        /// <summary>
        /// Get 'GeneralDefaultSearchConfiguration' label text
        /// </summary>
        /// <returns></returns>
        public string GetGeneralDefaultSearchConfigurationText()
        {
            return generalDefaultSearchConfigurationLabel.Text;
        }

        /// <summary>
        /// Get 'GeneralSearchAttributesDesc' label text
        /// </summary>
        /// <returns></returns>
        public string GetGeneralSearchAttributesDescText()
        {
            return generalSearchAttributesDescLabel.Text;
        }

        /// <summary>
        /// Get 'GeneralSearchAttributes' label text
        /// </summary>
        /// <returns></returns>
        public string GetGeneralSearchAttributesText()
        {
            return generalSearchAttributesLabel.Text;
        }

        /// <summary>
        /// Get 'GeneralSearchAttributes' input value
        /// </summary>
        /// <returns></returns>
        public string GetGeneralSearchAttributesValue()
        {
            return generalSearchAttributesInput.Value();
        }

        #endregion //General

        #region Desktop

        /// <summary>
        /// Get 'DesktopApplicationsListDesc' label text
        /// </summary>
        /// <returns></returns>
        public string GetDesktopApplicationsListDescText()
        {
            return desktopApplicationsListDescLabel.Text;
        }

        /// <summary>
        /// Get 'DesktopApplicationsList' label text
        /// </summary>
        /// <returns></returns>
        public string GetDesktopApplicationsListText()
        {
            return desktopApplicationsListLabel.Text;
        }

        /// <summary>
        /// Get 'DesktopWidgetsListDesc' label text
        /// </summary>
        /// <returns></returns>
        public string GetDesktopWidgetsListDescText()
        {
            return desktopWidgetsListDescLabel.Text;
        }

        /// <summary>
        /// Get 'DesktopWidgetsList' label text
        /// </summary>
        /// <returns></returns>
        public string GetDesktopWidgetsListText()
        {
            return desktopWidgetsListLabel.Text;
        }        

        #endregion //Desktop

        #region Equipment Inventory       

        /// <summary>
        /// Get 'EquipmentTreeviewConfiguration' label text
        /// </summary>
        /// <returns></returns>
        public string GetEquipmentTreeviewConfigurationText()
        {
            return equipmentTreeviewConfigurationLabel.Text;
        }

        /// <summary>
        /// Get 'EquipmentDisplayDevices' label text
        /// </summary>
        /// <returns></returns>
        public string GetEquipmentDisplayDevicesText()
        {
            return equipmentDisplayDevicesLabel.Text;
        }

        /// <summary>
        /// Get 'EquipmentDisplayDevices' checkbox value
        /// </summary>
        /// <returns></returns>
        public bool GetEquipmentDisplayDevicesValue()
        {
            return equipmentDisplayDevicesCheckbox.CheckboxValue();
        }

        /// <summary>
        /// Get 'EquipmentDisplayDevicesDesc' label text
        /// </summary>
        /// <returns></returns>
        public string GetEquipmentDisplayDevicesDescText()
        {
            return equipmentDisplayDevicesDescLabel.Text;
        }

        /// <summary>
        /// Get 'EquipmentMapFilter' label text
        /// </summary>
        /// <returns></returns>
        public string GetEquipmentMapFilterText()
        {
            return equipmentMapFilterLabel.Text;
        }

        /// <summary>
        /// Get 'EquipmentMapFilter' checkbox value
        /// </summary>
        /// <returns></returns>
        public bool GetEquipmentMapFilterValue()
        {
            return equipmentMapFilterCheckbox.CheckboxValue();
        }

        /// <summary>
        /// Get 'EquipmentMapFilterDesc' label text
        /// </summary>
        /// <returns></returns>
        public string GetEquipmentMapFilterDescText()
        {
            return equipmentMapFilterDescLabel.Text;
        }

        /// <summary>
        /// Get 'EquipmentEditorConfiguration' label text
        /// </summary>
        /// <returns></returns>
        public string GetEquipmentEditorConfigurationText()
        {
            return equipmentEditorConfigurationLabel.Text;
        }

        /// <summary>
        /// Get 'EquipmentDeviceLocation' label text
        /// </summary>
        /// <returns></returns>
        public string GetEquipmentDeviceLocationText()
        {
            return equipmentDeviceLocationLabel.Text;
        }

        /// <summary>
        /// Get 'EquipmentDeviceLocation' checkbox value
        /// </summary>
        /// <returns></returns>
        public bool GetEquipmentDeviceLocationValue()
        {
            return equipmentDeviceLocationCheckbox.CheckboxValue();
        }

        /// <summary>
        /// Get 'EquipmentDeviceLocationDesc' label text
        /// </summary>
        /// <returns></returns>
        public string GetEquipmentDeviceLocationDescText()
        {
            return equipmentDeviceLocationDescLabel.Text;
        }

        /// <summary>
        /// Get 'EquipmentParentGeozone' label text
        /// </summary>
        /// <returns></returns>
        public string GetEquipmentParentGeozoneText()
        {
            return equipmentParentGeozoneLabel.Text;
        }

        /// <summary>
        /// Get 'EquipmentParentGeozone' checkbox value
        /// </summary>
        /// <returns></returns>
        public bool GetEquipmentParentGeozoneValue()
        {
            return equipmentParentGeozoneCheckbox.CheckboxValue();
        }

        /// <summary>
        /// Get 'EquipmentParentGeozoneDesc' label text
        /// </summary>
        /// <returns></returns>
        public string GetEquipmentParentGeozoneDescText()
        {
            return equipmentParentGeozoneDescLabel.Text;
        }

        /// <summary>
        /// Get 'EquipmentReportConfiguration' label text
        /// </summary>
        /// <returns></returns>
        public string GetEquipmentReportConfigurationText()
        {
            return equipmentReportConfigurationLabel.Text;
        }

        /// <summary>
        /// Get 'EquipmentEventTimeVisible' label text
        /// </summary>
        /// <returns></returns>
        public string GetEquipmentEventTimeVisibleText()
        {
            return equipmentEventTimeVisibleLabel.Text;
        }

        /// <summary>
        /// Get 'EquipmentEventTimeVisible' checkbox value
        /// </summary>
        /// <returns></returns>
        public bool GetEquipmentEventTimeVisibleValue()
        {
            return equipmentEventTimeVisibleCheckbox.CheckboxValue();
        }

        /// <summary>
        /// Get 'EquipmentEventTimeVisibleDesc' label text
        /// </summary>
        /// <returns></returns>
        public string GetEquipmentEventTimeVisibleDescText()
        {
            return equipmentEventTimeVisibleDescLabel.Text;
        }

        /// <summary>
        /// Get 'EquipmentRowsPerPage' label text
        /// </summary>
        /// <returns></returns>
        public string GetEquipmentRowsPerPageText()
        {
            return equipmentRowsPerPageLabel.Text;
        }

        /// <summary>
        /// Get 'EquipmentRowsPerPage' input value
        /// </summary>
        /// <returns></returns>
        public string GetEquipmentRowsPerPageValue()
        {
            return equipmentRowsPerPageNumericInput.Value();
        }

        /// <summary>
        /// Get 'EquipmentRowsPerPageDesc' label text
        /// </summary>
        /// <returns></returns>
        public string GetEquipmentRowsPerPageDescText()
        {
            return equipmentRowsPerPageDescLabel.Text;
        }

        /// <summary>
        /// Get 'EquipmentToolbarItemsDesc' label text
        /// </summary>
        /// <returns></returns>
        public string GetEquipmentToolbarItemsDescText()
        {
            return equipmentToolbarItemsDescLabel.Text;
        }

        /// <summary>
        /// Get 'EquipmentToolbarItems' label text
        /// </summary>
        /// <returns></returns>
        public string GetEquipmentToolbarItemsText()
        {
            return equipmentToolbarItemsLabel.Text;
        }

        /// <summary>
        /// Get 'EquipmentSearchAttributesDesc' label text
        /// </summary>
        /// <returns></returns>
        public string GetEquipmentSearchAttributesDescText()
        {
            return equipmentSearchAttributesDescLabel.Text;
        }

        /// <summary>
        /// Get 'EquipmentSearchAttributes' label text
        /// </summary>
        /// <returns></returns>
        public string GetEquipmentSearchAttributesText()
        {
            return equipmentSearchAttributesLabel.Text;
        }

        /// <summary>
        /// Get 'EquipmentSearchAttributes' input value
        /// </summary>
        /// <returns></returns>
        public string GetEquipmentSearchAttributesValue()
        {
            return equipmentSearchAttributesInput.Value();
        }

        #endregion //Equipment Inventory

        #region Real-time Control     

        /// <summary>
        /// Get 'RealtimeTreeviewConfiguration' label text
        /// </summary>
        /// <returns></returns>
        public string GetRealtimeTreeviewConfigurationText()
        {
            return realtimeTreeviewConfigurationLabel.Text;
        }

        /// <summary>
        /// Get 'RealtimeDisplayDevices' label text
        /// </summary>
        /// <returns></returns>
        public string GetRealtimeDisplayDevicesText()
        {
            return realtimeDisplayDevicesLabel.Text;
        }

        /// <summary>
        /// Get 'RealtimeDisplayDevices' checkbox value
        /// </summary>
        /// <returns></returns>
        public bool GetRealtimeDisplayDevicesValue()
        {
            return realtimeDisplayDevicesCheckbox.CheckboxValue();
        }

        /// <summary>
        /// Get 'RealtimeDisplayDevicesDesc' label text
        /// </summary>
        /// <returns></returns>
        public string GetRealtimeDisplayDevicesDescText()
        {
            return realtimeDisplayDevicesDescLabel.Text;
        }

        /// <summary>
        /// Get 'RealtimeMapFilter' label text
        /// </summary>
        /// <returns></returns>
        public string GetRealtimeMapFilterText()
        {
            return realtimeMapFilterLabel.Text;
        }

        /// <summary>
        /// Get 'RealtimeMapFilter' checkbox value
        /// </summary>
        /// <returns></returns>
        public bool GetRealtimeMapFilterValue()
        {
            return realtimeMapFilterCheckbox.CheckboxValue();
        }

        /// <summary>
        /// Get 'RealtimeMapFilterDesc' label text
        /// </summary>
        /// <returns></returns>
        public string GetRealtimeMapFilterDescText()
        {
            return realtimeMapFilterDescLabel.Text;
        }

        #endregion //Real-time Control

        #region Data history       

        /// <summary>
        /// Get 'DataHistoryTreeviewConfiguration' label text
        /// </summary>
        /// <returns></returns>
        public string GetDataHistoryTreeviewConfigurationText()
        {
            return dataHistoryTreeviewConfigurationLabel.Text;
        }

        /// <summary>
        /// Get 'DataHistoryDisplayDevices' label text
        /// </summary>
        /// <returns></returns>
        public string GetDataHistoryDisplayDevicesText()
        {
            return dataHistoryDisplayDevicesLabel.Text;
        }

        /// <summary>
        /// Get 'DataHistoryDisplayDevices' checkbox value
        /// </summary>
        /// <returns></returns>
        public bool GetDataHistoryDisplayDevicesValue()
        {
            return dataHistoryDisplayDevicesCheckbox.CheckboxValue();
        }

        /// <summary>
        /// Get 'DataHistoryDisplayDevicesDesc' label text
        /// </summary>
        /// <returns></returns>
        public string GetDataHistoryDisplayDevicesDescText()
        {
            return dataHistoryDisplayDevicesDescLabel.Text;
        }

        /// <summary>
        /// Get 'DataHistoryReportConfiguration' label text
        /// </summary>
        /// <returns></returns>
        public string GetDataHistoryReportConfigurationText()
        {
            return dataHistoryReportConfigurationLabel.Text;
        }

        /// <summary>
        /// Get 'DataHistoryEventTimeVisible' label text
        /// </summary>
        /// <returns></returns>
        public string GetDataHistoryEventTimeVisibleText()
        {
            return dataHistoryEventTimeVisibleLabel.Text;
        }

        /// <summary>
        /// Get 'DataHistoryEventTimeVisible' checkbox value
        /// </summary>
        /// <returns></returns>
        public bool GetDataHistoryEventTimeVisibleValue()
        {
            return dataHistoryEventTimeVisibleCheckbox.CheckboxValue();
        }

        /// <summary>
        /// Get 'DataHistoryEventTimeVisibleDesc' label text
        /// </summary>
        /// <returns></returns>
        public string GetDataHistoryEventTimeVisibleDescText()
        {
            return dataHistoryEventTimeVisibleDescLabel.Text;
        }

        /// <summary>
        /// Get 'DataHistoryRowsPerPage' label text
        /// </summary>
        /// <returns></returns>
        public string GetDataHistoryRowsPerPageText()
        {
            return dataHistoryRowsPerPageLabel.Text;
        }

        /// <summary>
        /// Get 'DataHistoryRowsPerPage' input value
        /// </summary>
        /// <returns></returns>
        public string GetDataHistoryRowsPerPageValue()
        {
            return dataHistoryRowsPerPageNumericInput.Value();
        }

        /// <summary>
        /// Get 'DataHistoryRowsPerPageDesc' label text
        /// </summary>
        /// <returns></returns>
        public string GetDataHistoryRowsPerPageDescText()
        {
            return dataHistoryRowsPerPageDescLabel.Text;
        }

        /// <summary>
        /// Get 'DataHistoryToolbarItemsDesc' label text
        /// </summary>
        /// <returns></returns>
        public string GetDataHistoryToolbarItemsDescText()
        {
            return dataHistoryToolbarItemsDescLabel.Text;
        }

        /// <summary>
        /// Get 'DataHistoryToolbarItems' label text
        /// </summary>
        /// <returns></returns>
        public string GetDataHistoryToolbarItemsText()
        {
            return dataHistoryToolbarItemsLabel.Text;
        }

        #endregion //Data history

        #region Device History      

        /// <summary>
        /// Get 'DeviceHistoryTreeviewConfiguration' label text
        /// </summary>
        /// <returns></returns>
        public string GetDeviceHistoryTreeviewConfigurationText()
        {
            return deviceHistoryTreeviewConfigurationLabel.Text;
        }

        /// <summary>
        /// Get 'DeviceHistoryDisplayDevices' label text
        /// </summary>
        /// <returns></returns>
        public string GetDeviceHistoryDisplayDevicesText()
        {
            return deviceHistoryDisplayDevicesLabel.Text;
        }

        /// <summary>
        /// Get 'DeviceHistoryDisplayDevices' checkbox value
        /// </summary>
        /// <returns></returns>
        public bool GetDeviceHistoryDisplayDevicesValue()
        {
            return deviceHistoryDisplayDevicesCheckbox.CheckboxValue();
        }

        /// <summary>
        /// Get 'DeviceHistoryDisplayDevicesDesc' label text
        /// </summary>
        /// <returns></returns>
        public string GetDeviceHistoryDisplayDevicesDescText()
        {
            return deviceHistoryDisplayDevicesDescLabel.Text;
        }        

        /// <summary>
        /// Get 'DeviceHistoryReportConfiguration' label text
        /// </summary>
        /// <returns></returns>
        public string GetDeviceHistoryReportConfigurationText()
        {
            return deviceHistoryReportConfigurationLabel.Text;
        }

        /// <summary>
        /// Get 'DeviceHistoryEventTimeVisible' label text
        /// </summary>
        /// <returns></returns>
        public string GetDeviceHistoryEventTimeVisibleText()
        {
            return deviceHistoryEventTimeVisibleLabel.Text;
        }

        /// <summary>
        /// Get 'DeviceHistoryEventTimeVisible' checkbox value
        /// </summary>
        /// <returns></returns>
        public bool GetDeviceHistoryEventTimeVisibleValue()
        {
            return deviceHistoryEventTimeVisibleCheckbox.CheckboxValue();
        }

        /// <summary>
        /// Get 'DeviceHistoryEventTimeVisibleDesc' label text
        /// </summary>
        /// <returns></returns>
        public string GetDeviceHistoryEventTimeVisibleDescText()
        {
            return deviceHistoryEventTimeVisibleDescLabel.Text;
        }

        /// <summary>
        /// Get 'DeviceHistoryRowsPerPage' label text
        /// </summary>
        /// <returns></returns>
        public string GetDeviceHistoryRowsPerPageText()
        {
            return deviceHistoryRowsPerPageLabel.Text;
        }

        /// <summary>
        /// Get 'DeviceHistoryRowsPerPage' input value
        /// </summary>
        /// <returns></returns>
        public string GetDeviceHistoryRowsPerPageValue()
        {
            return deviceHistoryRowsPerPageNumericInput.Value();
        }

        /// <summary>
        /// Get 'DeviceHistoryRowsPerPageDesc' label text
        /// </summary>
        /// <returns></returns>
        public string GetDeviceHistoryRowsPerPageDescText()
        {
            return deviceHistoryRowsPerPageDescLabel.Text;
        }

        /// <summary>
        /// Get 'DeviceHistoryToolbarItemsDesc' label text
        /// </summary>
        /// <returns></returns>
        public string GetDeviceHistoryToolbarItemsDescText()
        {
            return deviceHistoryToolbarItemsDescLabel.Text;
        }

        /// <summary>
        /// Get 'DeviceHistoryToolbarItems' label text
        /// </summary>
        /// <returns></returns>
        public string GetDeviceHistoryToolbarItemsText()
        {
            return deviceHistoryToolbarItemsLabel.Text;
        }

        #endregion //Device History

        #region Failure Analysis

        /// <summary>
        /// Get 'FailureAnalysisTreeviewConfiguration' label text
        /// </summary>
        /// <returns></returns>
        public string GetFailureAnalysisTreeviewConfigurationText()
        {
            return failureAnalysisTreeviewConfigurationLabel.Text;
        }

        /// <summary>
        /// Get 'FailureAnalysisDisplayDevices' label text
        /// </summary>
        /// <returns></returns>
        public string GetFailureAnalysisDisplayDevicesText()
        {
            return failureAnalysisDisplayDevicesLabel.Text;
        }

        /// <summary>
        /// Get 'FailureAnalysisDisplayDevices' checkbox value
        /// </summary>
        /// <returns></returns>
        public bool GetFailureAnalysisDisplayDevicesValue()
        {
            return failureAnalysisDisplayDevicesCheckbox.CheckboxValue();
        }

        /// <summary>
        /// Get 'FailureAnalysisDisplayDevicesDesc' label text
        /// </summary>
        /// <returns></returns>
        public string GetFailureAnalysisDisplayDevicesDescText()
        {
            return failureAnalysisDisplayDevicesDescLabel.Text;
        }        

        /// <summary>
        /// Get 'FailureAnalysisSearchAttributesDesc' label text
        /// </summary>
        /// <returns></returns>
        public string GetFailureAnalysisSearchAttributesDescText()
        {
            return failureAnalysisSearchAttributesDescLabel.Text;
        }

        /// <summary>
        /// Get 'FailureAnalysisSearchAttributes' label text
        /// </summary>
        /// <returns></returns>
        public string GetFailureAnalysisSearchAttributesText()
        {
            return failureAnalysisSearchAttributesLabel.Text;
        }

        /// <summary>
        /// Get 'FailureAnalysisSearchAttributes' input value
        /// </summary>
        /// <returns></returns>
        public string GetFailureAnalysisSearchAttributesValue()
        {
            return failureAnalysisSearchAttributesInput.Value();
        }

        #endregion //Failure Analysis

        #region Failure Tracking

        /// <summary>
        /// Get 'FailureTrackingTreeviewConfiguration' label text
        /// </summary>
        /// <returns></returns>
        public string GetFailureTrackingTreeviewConfigurationText()
        {
            return failureTrackingTreeviewConfigurationLabel.Text;
        }

        /// <summary>
        /// Get 'FailureTrackingDisplayDevices' label text
        /// </summary>
        /// <returns></returns>
        public string GetFailureTrackingDisplayDevicesText()
        {
            return failureTrackingDisplayDevicesLabel.Text;
        }

        /// <summary>
        /// Get 'FailureTrackingDisplayDevices' checkbox value
        /// </summary>
        /// <returns></returns>
        public bool GetFailureTrackingDisplayDevicesValue()
        {
            return failureTrackingDisplayDevicesCheckbox.CheckboxValue();
        }

        /// <summary>
        /// Get 'FailureTrackingDisplayDevicesDesc' label text
        /// </summary>
        /// <returns></returns>
        public string GetFailureTrackingDisplayDevicesDescText()
        {
            return failureTrackingDisplayDevicesDescLabel.Text;
        }

        /// <summary>
        /// Get 'FailureTrackingMapFilter' label text
        /// </summary>
        /// <returns></returns>
        public string GetFailureTrackingMapFilterText()
        {
            return failureTrackingMapFilterLabel.Text;
        }

        /// <summary>
        /// Get 'FailureTrackingMapFilter' checkbox value
        /// </summary>
        /// <returns></returns>
        public bool GetFailureTrackingMapFilterValue()
        {
            return failureTrackingMapFilterCheckbox.CheckboxValue();
        }

        /// <summary>
        /// Get 'FailureTrackingMapFilterDesc' label text
        /// </summary>
        /// <returns></returns>
        public string GetFailureTrackingMapFilterDescText()
        {
            return failureTrackingMapFilterDescLabel.Text;
        }

        /// <summary>
        /// Get 'FailureTrackingSearchAttributesDesc' label text
        /// </summary>
        /// <returns></returns>
        public string GetFailureTrackingSearchAttributesDescText()
        {
            return failureTrackingSearchAttributesDescLabel.Text;
        }

        /// <summary>
        /// Get 'FailureTrackingSearchAttributs' label text
        /// </summary>
        /// <returns></returns>
        public string GetFailureTrackingSearchAttributesText()
        {
            return failureTrackingSearchAttributesLabel.Text;
        }

        /// <summary>
        /// Get 'FailureTrackingSearchAttributes' input value
        /// </summary>
        /// <returns></returns>
        public string GetFailureTrackingSearchAttributesValue()
        {
            return failureTrackingSearchAttributesInput.Value();
        }

        #endregion //Failure Tracking

        #region Advanced Search

        /// <summary>
        /// Get 'AdvancedSearchReportConfiguration' label text
        /// </summary>
        /// <returns></returns>
        public string GetAdvancedSearchReportConfigurationText()
        {
            return advancedSearchReportConfigurationLabel.Text;
        }

        /// <summary>
        /// Get 'AdvancedSearchEventTimeVisible' label text
        /// </summary>
        /// <returns></returns>
        public string GetAdvancedSearchEventTimeVisibleText()
        {
            return advancedSearchEventTimeVisibleLabel.Text;
        }

        /// <summary>
        /// Get 'AdvancedSearchEventTimeVisible' checkbox value
        /// </summary>
        /// <returns></returns>
        public bool GetAdvancedSearchEventTimeVisibleValue()
        {
            return advancedSearchEventTimeVisibleCheckbox.CheckboxValue();
        }

        /// <summary>
        /// Get 'AdvancedSearchEventTimeVisibleDesc' label text
        /// </summary>
        /// <returns></returns>
        public string GetAdvancedSearchEventTimeVisibleDescText()
        {
            return advancedSearchEventTimeVisibleDescLabel.Text;
        }

        /// <summary>
        /// Get 'AdvancedSearchRowsPerPage' label text
        /// </summary>
        /// <returns></returns>
        public string GetAdvancedSearchRowsPerPageText()
        {
            return advancedSearchRowsPerPageLabel.Text;
        }

        /// <summary>
        /// Get 'AdvancedSearchRowsPerPage' input value
        /// </summary>
        /// <returns></returns>
        public string GetAdvancedSearchRowsPerPageValue()
        {
            return advancedSearchRowsPerPageNumericInput.Value();
        }

        /// <summary>
        /// Get 'AdvancedSearchRowsPerPageDesc' label text
        /// </summary>
        /// <returns></returns>
        public string GetAdvancedSearchRowsPerPageDescText()
        {
            return advancedSearchRowsPerPageDescLabel.Text;
        }

        /// <summary>
        /// Get 'AdvancedSearchToolbarItemsDesc' label text
        /// </summary>
        /// <returns></returns>
        public string GetAdvancedSearchToolbarItemsDescText()
        {
            return advancedSearchToolbarItemsDescLabel.Text;
        }

        /// <summary>
        /// Get 'AdvancedSearchToolbarItems' label text
        /// </summary>
        /// <returns></returns>
        public string GetAdvancedSearchToolbarItemsText()
        {
            return advancedSearchToolbarItemsLabel.Text;
        }

        #endregion //Advanced Search

        #endregion //Get methods

        #endregion //Basic methods

        #region Business methods

        public void WaitForTitleDisplayed(string name)
        {
            Wait.ForElementText(panelTitle, name);
        }

        #region General

        public bool IsGeneralTimeoutAuthenticationNumericInputEditable()
        {
            return !generalTimeoutAuthenticationNumericInput.IsReadOnly();
        }

        public bool IsGeneralGridRowMaxCountNumericInputEditable()
        {
            return !generalGridRowMaxCountNumericInput.IsReadOnly();
        }

        public bool IsGeneralExportCsvSeparatorInputEditable()
        {
            return !generalExportCsvSeparatorInput.IsReadOnly();
        }

        public List<string> GetGeneralSearchAttributsNameList()
        {
            return JSUtility.GetElementsText("[id='slv-view-backoffice-editor-editor-General-searchAttributs'] .backoffice-configuration-field-list-item-title");
        }

        /// <summary>
        /// Remove General attributes
        /// </summary>
        public void RemoveGeneralAttributes(params string[] attributes)
        {
            foreach (var attribute in attributes)
            {
                var item = generalSearchAttributesList.FirstOrDefault(p => p.Text.Equals(attribute));
                if (item != null)
                {
                    item.ScrollToElementByJS();
                    var removeBy = By.CssSelector("div.backoffice-configuration-field-list-item-button.icon-delete");
                    item.MoveTo();
                    var removeBtn = item.FindElement(removeBy);
                    Wait.ForElementStyle(removeBtn, "visibility: visible");                   
                    removeBtn.ClickEx();
                    Wait.ForMilliseconds(100);
                }
                else
                    Assert.Warn(string.Format("'{0}' does not exit", attribute));
            }
        }

        /// <summary>
        /// Add General attributes
        /// </summary>
        /// <param name="attributes"></param>
        public void AddGeneralAttributes(params string[] attributes)
        {
            foreach (var attribute in attributes)
            {
                EnterGeneralSearchAttributsInput(attribute);
                ClickGeneralSearchAttributsAddButton();
                Wait.ForMilliseconds(100);
            }
        }

        #endregion //General

        #region Desktop

        public List<string> GetDesktopAllApplicationsNameList()
        {
            return JSUtility.GetElementsText("[id='slv-view-backoffice-editor-editor-Desktop-appList'] .backoffice-configuration-field-list-item-title");
        }

        public List<string> GetDesktopAvailableApplicationsNameList()
        {
            return JSUtility.GetElementsText("[id='slv-view-backoffice-editor-editor-Desktop-appList'] .backoffice-configuration-field-list-item:not(.backoffice-configuration-field-list-item-disable) .backoffice-configuration-field-list-item-title");
        }

        public List<string> GetDesktopDisableApplicationsNameList()
        {
            return JSUtility.GetElementsText("[id='slv-view-backoffice-editor-editor-Desktop-appList'] .backoffice-configuration-field-list-item-disable .backoffice-configuration-field-list-item-title");
        }

        public List<string> GetDesktopAllWidgetsNameList()
        {
            return JSUtility.GetElementsText("[id='slv-view-backoffice-editor-editor-Desktop-widgetList'] .backoffice-configuration-field-list-item-title");
        }

        public List<string> GetDesktopAvailableWidgetsNameList()
        {
            return JSUtility.GetElementsText("[id='slv-view-backoffice-editor-editor-Desktop-widgetList'] .backoffice-configuration-field-list-item:not(.backoffice-configuration-field-list-item-disable) .backoffice-configuration-field-list-item-title");
        }

        public List<string> GetDesktopDisableWidgetsNameList()
        {
            return JSUtility.GetElementsText("[id='slv-view-backoffice-editor-editor-Desktop-widgetList'] .backoffice-configuration-field-list-item-disable .backoffice-configuration-field-list-item-title");
        }

        /// <summary>
        /// Check all apps
        /// </summary>
        public void CheckAllApps()
        {
            var apps = new List<IWebElement>(desktopApplicationsList);
            foreach (var app in apps)
            {
                var checkbox = app.FindElement(By.CssSelector(".checkbox"));
                checkbox.Check(true);
            }
        }

        /// <summary>
        /// Un-check all apps
        /// </summary>
        public void UncheckAllApps()
        {
            var apps = new List<IWebElement>(desktopApplicationsList);
            foreach (var app in apps)
            {
                var checkbox = app.FindElement(By.CssSelector(".checkbox"));
                checkbox.Check(false);
            }
        }        

        /// <summary>
        /// Un-check random apps with specific number count
        /// </summary>
        /// <param name="count"></param>
        public void UncheckRandomApps(int count)
        {
            var apps = new List<IWebElement>(desktopAvailableApplicationsList);
            if (apps.Count < count) count = apps.Count;
            var randomApps = apps.PickRandom(count);
            foreach (var app in randomApps)
            {
                var checkbox = app.FindElement(By.CssSelector(".checkbox"));
                checkbox.Check(false);
            }
        }

        /// <summary>
        /// check random apps with specific number count
        /// </summary>
        /// <param name="count"></param>
        public void CheckRandomApps(int count)
        {
            var apps = new List<IWebElement>(desktopDisableApplicationsList);
            if (apps.Count < count) count = apps.Count;
            var randomApps = apps.PickRandom(count);
            foreach (var app in randomApps)
            {
                var checkbox = app.FindElement(By.CssSelector(".checkbox"));
                checkbox.Check(true);
            }
        }

        /// <summary>
        /// Check apps
        /// </summary>
        public void CheckApps(params string[] appsName)
        {
            var apps = new List<IWebElement>(desktopApplicationsList);
            foreach (var app in apps)
            {
                if (appsName.Contains(app.Text))
                {
                    var checkbox = app.FindElement(By.CssSelector(".checkbox"));
                    checkbox.Check(true);
                }
            }
        }

        /// <summary>
        /// Check apps
        /// </summary>
        public void UncheckApps(params string[] appsName)
        {
            var apps = new List<IWebElement>(desktopApplicationsList);
            foreach (var app in apps)
            {
                if (appsName.Contains(app.Text))
                {
                    var checkbox = app.FindElement(By.CssSelector(".checkbox"));
                    checkbox.Check(false);
                }
            }
        }

        /// <summary>
        /// Check all widgets
        /// </summary>
        public void CheckAllWidgets()
        {
            var widgets = new List<IWebElement>(desktopWidgetsList);
            foreach (var widget in widgets)
            {
                var checkbox = widget.FindElement(By.CssSelector(".checkbox"));
                checkbox.Check(true);
            }
        }

        /// <summary>
        /// Un-check all widgets
        /// </summary>
        public void UncheckAllWidgets()
        {
            var widgets = new List<IWebElement>(desktopWidgetsList);
            foreach (var widget in widgets)
            {
                var checkbox = widget.FindElement(By.CssSelector(".checkbox"));
                checkbox.Check(false);
            }
        }

        /// <summary>
        /// Un-check random widgets with specific number count
        /// </summary>
        /// <param name="count"></param>
        public void UncheckRandomWidgets(int count)
        {
            var widgets = new List<IWebElement>(desktopAvailableWidgetsList);
            if (widgets.Count < count) count = widgets.Count;
            var randomWidget= widgets.PickRandom(count);
            foreach (var widget in randomWidget)
            {
                var checkbox = widget.FindElement(By.CssSelector(".checkbox"));
                checkbox.Check(false);
            }
        }

        /// <summary>
        /// Check random widgets with specific number count
        /// </summary>
        /// <param name="count"></param>
        public void CheckRandomWidgets(int count)
        {
            var widgets = new List<IWebElement>(desktopDisableWidgetsList);
            if (widgets.Count < count) count = widgets.Count;
            var randomWidget = widgets.PickRandom(count);
            foreach (var widget in randomWidget)
            {
                var checkbox = widget.FindElement(By.CssSelector(".checkbox"));
                checkbox.Check(true);
            }
        }

        /// <summary>
        /// Check widgets
        /// </summary>
        public void CheckWidgets(params string[] widgetsName)
        {
            var widgets = new List<IWebElement>(desktopWidgetsList);
            foreach (var widget in widgets)
            {
                if (widgetsName.Contains(widget.Text))
                {
                    var checkbox = widget.FindElement(By.CssSelector(".checkbox"));
                    checkbox.Check(true);
                }
            }
        }

        /// <summary>
        /// Check widgets
        /// </summary>
        public void UncheckWidgets(params string[] widgetsName)
        {
            var widgets = new List<IWebElement>(desktopWidgetsList);
            foreach (var widget in widgets)
            {
                if (widgetsName.Contains(widget.Text))
                {
                    var checkbox = widget.FindElement(By.CssSelector(".checkbox"));
                    checkbox.Check(false);
                }
            }
        }

        #endregion //Desktop

        #region Equipment Inventory

        public bool IsEquipmentDisplayDevicesCheckboxEditable()
        {
            return !equipmentDisplayDevicesCheckbox.FindElement(By.CssSelector("input")).IsReadOnly();
        }

        public bool IsEquipmentMapFilterCheckboxEditable()
        {
            return !equipmentMapFilterCheckbox.FindElement(By.CssSelector("input")).IsReadOnly();
        }

        public bool IsEquipmentDeviceLocationCheckboxEditable()
        {
            return !equipmentDeviceLocationCheckbox.FindElement(By.CssSelector("input")).IsReadOnly();
        }

        public bool IsEquipmentParentGeozoneCheckboxEditable()
        {
            return !equipmentParentGeozoneCheckbox.FindElement(By.CssSelector("input")).IsReadOnly();
        }

        public bool IsEquipmentEventTimeVisibleCheckboxEditable()
        {
            return !equipmentEventTimeVisibleCheckbox.FindElement(By.CssSelector("input")).IsReadOnly();
        }

        public bool IsEquipmentRowCountPerPageInputEditable()
        {
            return !equipmentRowsPerPageNumericInput.IsReadOnly();
        }

        public List<string> GetEquipmentToolbarItemsNameList()
        {
            return JSUtility.GetElementsText("[id='slv-view-backoffice-editor-editor-EquipmentGL-toolbarItems'] .backoffice-configuration-field-list-item-title");
        }

        public List<string> GetEquipmentAvailableToolbarItemsNameList()
        {
            return JSUtility.GetElementsText("[id='slv-view-backoffice-editor-editor-EquipmentGL-toolbarItems'] .backoffice-configuration-field-list-item:not(.backoffice-configuration-field-list-item-disable) .backoffice-configuration-field-list-item-title");
        }

        public List<string> GetEquipmentDisableToolbarItemsNameList()
        {
            return JSUtility.GetElementsText("[id='slv-view-backoffice-editor-editor-EquipmentGL-toolbarItems'] .backoffice-configuration-field-list-item-disable .backoffice-configuration-field-list-item-title");
        }

        /// <summary>
        /// check all ToolbarItem
        /// </summary>
        public void CheckEquipmentAllToolbarItems()
        {
            var apps = new List<IWebElement>(equipmentToolbarItemsList);
            foreach (var app in apps)
            {
                var checkbox = app.FindElement(By.CssSelector(".checkbox"));
                checkbox.Check(true);
            }
        }

        /// <summary>
        /// Un-check all ToolbarItem
        /// </summary>
        public void UncheckEquipmentAllToolbarItems()
        {
            var apps = new List<IWebElement>(equipmentToolbarItemsList);
            foreach (var app in apps)
            {
                var checkbox = app.FindElement(By.CssSelector(".checkbox"));
                checkbox.Check(false);
            }
        }

        /// <summary>
        /// Un-check random ToolbarItems with specific number count
        /// </summary>
        /// <param name="count"></param>
        public void UncheckEquipmentRandomToolbarItems(int count)
        {
            var items = new List<IWebElement>(equipmentAvailableToolbarItemsList);
            var randomItems = items.PickRandom(count);
            foreach (var app in randomItems)
            {
                var checkbox = app.FindElement(By.CssSelector(".checkbox"));
                checkbox.Check(false);
            }
        }

        /// <summary>
        /// check random ToolbarItems with specific number count
        /// </summary>
        /// <param name="count"></param>
        public void CheckEquipmentRandomToolbarItems(int count)
        {
            var items = new List<IWebElement>(equipmentDisableToolbarItemsList);
            var randomItems = items.PickRandom(count);
            foreach (var item in randomItems)
            {
                var checkbox = item.FindElement(By.CssSelector(".checkbox"));
                checkbox.Check(true);
            }
        }

        /// <summary>
        /// Check Toolbar Items
        /// </summary>
        public void CheckEquipmentToolbarItems(params string[] itemsName)
        {
            var items = new List<IWebElement>(equipmentToolbarItemsList);
            foreach (var item in items)
            {
                if (itemsName.Contains(item.Text))
                {
                    var checkbox = item.FindElement(By.CssSelector(".checkbox"));
                    checkbox.Check(true);
                }
            }
        }

        /// <summary>
        /// Uncheck Toolbar Items
        /// </summary>
        public void UncheckEquipmentToolbarItems(params string[] itemsName)
        {
            var items = new List<IWebElement>(equipmentToolbarItemsList);
            foreach (var item in items)
            {
                if (itemsName.Contains(item.Text))
                {
                    var checkbox = item.FindElement(By.CssSelector(".checkbox"));
                    checkbox.Check(false);
                }
            }
        }

        public List<string> GetEquipmentSearchAttributesNameList()
        {
            return JSUtility.GetElementsText("[id='slv-view-backoffice-editor-editor-EquipmentGL-searchAttributs'] .backoffice-configuration-field-list-item-title");
        }

        /// <summary>
        /// Remove Equipment attributes
        /// </summary>
        public void RemoveEquipmentAttributes(params string[] attributes)
        {
            foreach (var attribute in attributes)
            {
                var item = equipmentSearchAttributesList.FirstOrDefault(p => p.Text.Equals(attribute));
                if (item != null)
                {
                    item.ScrollToElementByJS();
                    var removeBy = By.CssSelector("div.backoffice-configuration-field-list-item-button.icon-delete");
                    item.MoveTo();
                    var removeBtn = item.FindElement(removeBy);
                    Wait.ForElementStyle(removeBtn, "visibility: visible");
                    removeBtn.ClickEx();
                    Wait.ForMilliseconds(100);
                }
                else
                    Assert.Warn(string.Format("'{0}' does not exit", attribute));
            }
        }

        /// <summary>
        /// Add Equipment attributes
        /// </summary>
        /// <param name="attributes"></param>
        public void AddEquipmentAttributes(params string[] attributes)
        {
            foreach (var attribute in attributes)
            {
                EnterEquipmentSearchAttributesInput(attribute);
                ClickEquipmentSearchAttributesAddButton();
                Wait.ForMilliseconds(100);
            }
        }

        #endregion //Equipment Inventory

        #region Real-time Control

        public bool IsRealtimeDisplayDevicesCheckboxEditable()
        {
            return !realtimeDisplayDevicesCheckbox.FindElement(By.CssSelector("input")).IsReadOnly();
        }

        public bool IsRealtimeMapFilterCheckboxEditable()
        {
            return !realtimeMapFilterCheckbox.FindElement(By.CssSelector("input")).IsReadOnly();
        }

        #endregion //Real-time Control

        #region Data History

        public bool IsDataHistoryDisplayDevicesCheckboxEditable()
        {
            return !dataHistoryDisplayDevicesCheckbox.FindElement(By.CssSelector("input")).IsReadOnly();
        }      

        public bool IsDataHistoryEventTimeVisibleCheckboxEditable()
        {
            return !dataHistoryEventTimeVisibleCheckbox.FindElement(By.CssSelector("input")).IsReadOnly();
        }

        public bool IsDataHistoryRowCountPerPageInputEditable()
        {
            return !dataHistoryRowsPerPageNumericInput.IsReadOnly();
        }

        public List<string> GetDataHistoryToolbarItemsNameList()
        {
            return JSUtility.GetElementsText("[id='slv-view-backoffice-editor-editor-DataHistory-toolbarItems'] .backoffice-configuration-field-list-item-title");
        }

        public List<string> GetDataHistoryAvailableToolbarItemsNameList()
        {
            return JSUtility.GetElementsText("[id='slv-view-backoffice-editor-editor-DataHistory-toolbarItems'] .backoffice-configuration-field-list-item:not(.backoffice-configuration-field-list-item-disable) .backoffice-configuration-field-list-item-title");
        }

        public List<string> GetDataHistoryDisableToolbarItemsNameList()
        {
            return JSUtility.GetElementsText("[id='slv-view-backoffice-editor-editor-DataHistory-toolbarItems'] .backoffice-configuration-field-list-item-disable .backoffice-configuration-field-list-item-title");
        }

        /// <summary>
        /// check all ToolbarItem
        /// </summary>
        public void CheckDataHistoryAllToolbarItems()
        {
            var apps = new List<IWebElement>(dataHistoryToolbarItemsList);
            foreach (var app in apps)
            {
                var checkbox = app.FindElement(By.CssSelector(".checkbox"));
                checkbox.Check(true);
            }
        }

        /// <summary>
        /// Un-check all ToolbarItem
        /// </summary>
        public void UncheckDataHistoryAllToolbarItems()
        {
            var apps = new List<IWebElement>(dataHistoryToolbarItemsList);
            foreach (var app in apps)
            {
                var checkbox = app.FindElement(By.CssSelector(".checkbox"));
                checkbox.Check(false);
            }
        }

        /// <summary>
        /// Un-check random ToolbarItems with specific number count
        /// </summary>
        /// <param name="count"></param>
        public void UncheckDataHistoryRandomToolbarItems(int count)
        {
            var items = new List<IWebElement>(dataHistoryAvailableToolbarItemsList);
            var randomItems = items.PickRandom(count);
            foreach (var app in randomItems)
            {
                var checkbox = app.FindElement(By.CssSelector(".checkbox"));
                checkbox.Check(false);
            }
        }

        /// <summary>
        /// check random ToolbarItems with specific number count
        /// </summary>
        /// <param name="count"></param>
        public void CheckDataHistoryRandomToolbarItems(int count)
        {
            var items = new List<IWebElement>(dataHistoryDisableToolbarItemsList);
            var randomItems = items.PickRandom(count);
            foreach (var item in randomItems)
            {
                var checkbox = item.FindElement(By.CssSelector(".checkbox"));
                checkbox.Check(true);
            }
        }

        /// <summary>
        /// Check Toolbar Items
        /// </summary>
        public void CheckDataHistoryToolbarItems(params string[] itemsName)
        {
            var items = new List<IWebElement>(dataHistoryToolbarItemsList);
            foreach (var item in items)
            {
                if (itemsName.Contains(item.Text))
                {
                    var checkbox = item.FindElement(By.CssSelector(".checkbox"));
                    checkbox.Check(true);
                }
            }
        }

        /// <summary>
        /// Uncheck Toolbar Items
        /// </summary>
        public void UncheckDataHistoryToolbarItems(params string[] itemsName)
        {
            var items = new List<IWebElement>(dataHistoryToolbarItemsList);
            foreach (var item in items)
            {
                if (itemsName.Contains(item.Text))
                {
                    var checkbox = item.FindElement(By.CssSelector(".checkbox"));
                    checkbox.Check(false);
                }
            }
        }

        #endregion //Data History

        #region Device History

        public bool IsDeviceHistoryDisplayDevicesCheckboxEditable()
        {
            return !deviceHistoryDisplayDevicesCheckbox.FindElement(By.CssSelector("input")).IsReadOnly();
        }        

        public bool IsDeviceHistoryEventTimeVisibleCheckboxEditable()
        {
            return !deviceHistoryEventTimeVisibleCheckbox.FindElement(By.CssSelector("input")).IsReadOnly();
        }

        public bool IsDeviceHistoryRowCountPerPageInputEditable()
        {
            return !deviceHistoryRowsPerPageNumericInput.IsReadOnly();
        }

        public List<string> GetDeviceHistoryToolbarItemsNameList()
        {
            return JSUtility.GetElementsText("[id='slv-view-backoffice-editor-editor-DeviceHistory-toolbarItems'] .backoffice-configuration-field-list-item-title");
        }

        public List<string> GetDeviceHistoryAvailableToolbarItemsNameList()
        {
            return JSUtility.GetElementsText("[id='slv-view-backoffice-editor-editor-DeviceHistory-toolbarItems'] .backoffice-configuration-field-list-item:not(.backoffice-configuration-field-list-item-disable) .backoffice-configuration-field-list-item-title");
        }

        public List<string> GetDeviceHistoryDisableToolbarItemsNameList()
        {
            return JSUtility.GetElementsText("[id='slv-view-backoffice-editor-editor-DeviceHistory-toolbarItems'] .backoffice-configuration-field-list-item-disable .backoffice-configuration-field-list-item-title");
        }

        /// <summary>
        /// check all ToolbarItem
        /// </summary>
        public void CheckDeviceHistoryAllToolbarItems()
        {
            var apps = new List<IWebElement>(deviceHistoryToolbarItemsList);
            foreach (var app in apps)
            {
                var checkbox = app.FindElement(By.CssSelector(".checkbox"));
                checkbox.Check(true);
            }
        }

        /// <summary>
        /// Un-check all ToolbarItem
        /// </summary>
        public void UncheckDeviceHistoryAllToolbarItems()
        {
            var apps = new List<IWebElement>(deviceHistoryToolbarItemsList);
            foreach (var app in apps)
            {
                var checkbox = app.FindElement(By.CssSelector(".checkbox"));
                checkbox.Check(false);
            }
        }

        /// <summary>
        /// Un-check random ToolbarItems with specific number count
        /// </summary>
        /// <param name="count"></param>
        public void UncheckDeviceHistoryRandomToolbarItems(int count)
        {
            var items = new List<IWebElement>(deviceHistoryAvailableToolbarItemsList);
            var randomItems = items.PickRandom(count);
            foreach (var app in randomItems)
            {
                var checkbox = app.FindElement(By.CssSelector(".checkbox"));
                checkbox.Check(false);
            }
        }

        /// <summary>
        /// check random ToolbarItems with specific number count
        /// </summary>
        /// <param name="count"></param>
        public void CheckDeviceHistoryRandomToolbarItems(int count)
        {
            var items = new List<IWebElement>(deviceHistoryDisableToolbarItemsList);
            var randomItems = items.PickRandom(count);
            foreach (var item in randomItems)
            {
                var checkbox = item.FindElement(By.CssSelector(".checkbox"));
                checkbox.Check(true);
            }
        }

        /// <summary>
        /// Check Toolbar Items
        /// </summary>
        public void CheckDeviceHistoryToolbarItems(params string[] itemsName)
        {
            var items = new List<IWebElement>(deviceHistoryToolbarItemsList);
            foreach (var item in items)
            {
                if (itemsName.Contains(item.Text))
                {
                    var checkbox = item.FindElement(By.CssSelector(".checkbox"));
                    checkbox.Check(true);
                }
            }
        }

        #endregion //Device History

        #region Failure Analysis

        public bool IsFailureAnalysisDisplayDevicesCheckboxEditable()
        {
            return !failureAnalysisDisplayDevicesCheckbox.FindElement(By.CssSelector("input")).IsReadOnly();
        }        

        public List<string> GetFailureAnalysisSearchAttributesNameList()
        {
            return JSUtility.GetElementsText("[id='slv-view-backoffice-editor-editor-Failure-searchAttributs'] .backoffice-configuration-field-list-item-title");
        }

        /// <summary>
        /// Remove Failure Analysis attributes
        /// </summary>
        public void RemoveFailureAnalysisAttributes(params string[] attributes)
        {
            foreach (var attribute in attributes)
            {
                var item = failureAnalysisSearchAttributesList.FirstOrDefault(p => p.Text.Equals(attribute));
                if (item != null)
                {
                    item.ScrollToElementByJS();
                    var removeBy = By.CssSelector("div.backoffice-configuration-field-list-item-button.icon-delete");
                    item.MoveTo();
                    var removeBtn = item.FindElement(removeBy);
                    Wait.ForElementStyle(removeBtn, "visibility: visible");
                    removeBtn.ClickEx();
                    Wait.ForMilliseconds(100);
                }
                else
                    Assert.Warn(string.Format("'{0}' does not exit", attribute));
            }
        }

        /// <summary>
        /// Add Failure Analysis attributes
        /// </summary>
        /// <param name="attributes"></param>
        public void AddFailureAnalysisAttributes(params string[] attributes)
        {
            foreach (var attribute in attributes)
            {
                EnterFailureAnalysisSearchAttributsInput(attribute);
                ClickFailureAnalysisSearchAttributesAddButton();
                Wait.ForMilliseconds(100);
            }
        }

        #endregion //Failure Analysis

        #region Failure Tracking

        public bool IsFailureTrackingDisplayDevicesCheckboxEditable()
        {
            return !failureTrackingDisplayDevicesCheckbox.FindElement(By.CssSelector("input")).IsReadOnly();
        }

        public bool IsFailureTrackingMapFilterCheckboxEditable()
        {
            return !failureTrackingMapFilterCheckbox.FindElement(By.CssSelector("input")).IsReadOnly();
        }

        public List<string> GetFailureTrackingSearchAttributesNameList()
        {
            return JSUtility.GetElementsText("[id='slv-view-backoffice-editor-editor-FailureTrackingGL-searchAttributs'] .backoffice-configuration-field-list-item-title");
        }

        /// <summary>
        /// Remove Failure Tracking attributes
        /// </summary>
        public void RemoveFailureTrackingAttributes(params string[] attributes)
        {
            foreach (var attribute in attributes)
            {
                var item = failureTrackingSearchAttributesList.FirstOrDefault(p => p.Text.Equals(attribute));
                if (item != null)
                {
                    item.ScrollToElementByJS();
                    var removeBy = By.CssSelector("div.backoffice-configuration-field-list-item-button.icon-delete");
                    item.MoveTo();
                    var removeBtn = item.FindElement(removeBy);
                    Wait.ForElementStyle(removeBtn, "visibility: visible");
                    removeBtn.ClickEx();
                    Wait.ForMilliseconds(100);
                }
                else
                    Assert.Warn(string.Format("'{0}' does not exit", attribute));
            }
        }

        /// <summary>
        /// Add Failure Tracking attributes
        /// </summary>
        /// <param name="attributes"></param>
        public void AddFailureTrackingAttributes(params string[] attributes)
        {
            foreach (var attribute in attributes)
            {
                EnterFailureTrackingSearchAttributesInput(attribute);
                ClickFailureTrackingSearchAttributesAddButton();
                Wait.ForMilliseconds(100);
            }
        }

        #endregion //Failure Tracking

        #region Advanced Search        

        public bool IsAdvancedSearchEventTimeVisibleCheckboxEditable()
        {
            return !advancedSearchEventTimeVisibleCheckbox.FindElement(By.CssSelector("input")).IsReadOnly();
        }

        public bool IsAdvancedSearchRowCountPerPageInputEditable()
        {
            return !advancedSearchRowsPerPageNumericInput.IsReadOnly();
        }

        public List<string> GetAdvancedSearchToolbarItemsNameList()
        {
            return JSUtility.GetElementsText("[id='slv-view-backoffice-editor-editor-CustomReport-toolbarItems'] .backoffice-configuration-field-list-item-title");
        }

        public List<string> GetAdvancedSearchAvailableToolbarItemsNameList()
        {
            return JSUtility.GetElementsText("[id='slv-view-backoffice-editor-editor-CustomReport-toolbarItems'] .backoffice-configuration-field-list-item:not(.backoffice-configuration-field-list-item-disable) .backoffice-configuration-field-list-item-title");
        }

        public List<string> GetAdvancedSearchDisableToolbarItemsNameList()
        {
            return JSUtility.GetElementsText("[id='slv-view-backoffice-editor-editor-CustomReport-toolbarItems'] .backoffice-configuration-field-list-item-disable .backoffice-configuration-field-list-item-title");
        }

        /// <summary>
        /// check all ToolbarItem
        /// </summary>
        public void CheckAdvancedSearchAllToolbarItems()
        {
            var apps = new List<IWebElement>(advancedSearchToolbarItemsList);
            foreach (var app in apps)
            {
                var checkbox = app.FindElement(By.CssSelector(".checkbox"));
                checkbox.Check(true);
            }
        }

        /// <summary>
        /// Un-check all ToolbarItem
        /// </summary>
        public void UncheckAdvancedSearchAllToolbarItems()
        {
            var apps = new List<IWebElement>(advancedSearchToolbarItemsList);
            foreach (var app in apps)
            {
                var checkbox = app.FindElement(By.CssSelector(".checkbox"));
                checkbox.Check(false);
            }
        }

        /// <summary>
        /// Un-check random ToolbarItems with specific number count
        /// </summary>
        /// <param name="count"></param>
        public void UncheckAdvancedSearchRandomToolbarItems(int count)
        {
            var items = new List<IWebElement>(advancedSearchAvailableToolbarItemsList);
            var randomItems = items.PickRandom(count);
            foreach (var app in randomItems)
            {
                var checkbox = app.FindElement(By.CssSelector(".checkbox"));
                checkbox.Check(false);
            }
        }

        /// <summary>
        /// check random ToolbarItems with specific number count
        /// </summary>
        /// <param name="count"></param>
        public void CheckAdvancedSearchRandomToolbarItems(int count)
        {
            var items = new List<IWebElement>(advancedSearchDisableToolbarItemsList);
            var randomItems = items.PickRandom(count);
            foreach (var item in randomItems)
            {
                var checkbox = item.FindElement(By.CssSelector(".checkbox"));
                checkbox.Check(true);
            }
        }

        /// <summary>
        /// Check Toolbar Items
        /// </summary>
        public void CheckAdvancedSearchToolbarItems(params string[] itemsName)
        {
            var items = new List<IWebElement>(advancedSearchToolbarItemsList);
            foreach (var item in items)
            {
                if (itemsName.Contains(item.Text))
                {
                    var checkbox = item.FindElement(By.CssSelector(".checkbox"));
                    checkbox.Check(true);
                }
            }
        }

        /// <summary>
        /// Uncheck Toolbar Items
        /// </summary>
        public void UncheckAdvancedSearchToolbarItems(params string[] itemsName)
        {
            var items = new List<IWebElement>(advancedSearchToolbarItemsList);
            foreach (var item in items)
            {
                if (itemsName.Contains(item.Text))
                {
                    var checkbox = item.FindElement(By.CssSelector(".checkbox"));
                    checkbox.Check(false);
                }
            }
        }

        #endregion //Advanced Search

        #endregion //Business methods

        public override void WaitForPanelLoaded()
        {

        }

        public override void WaitForPreviousActionComplete()
        {
            base.WaitForPreviousActionComplete();
        }
    }
}
