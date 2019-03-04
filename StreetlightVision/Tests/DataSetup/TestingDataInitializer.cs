using NUnit.Framework;
using StreetlightVision.Extensions;
using StreetlightVision.Models;
using StreetlightVision.Utilities;
using System;
using System.Linq;

namespace StreetlightVision.Tests.DataSetup
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    public class TestingDataInitializer : TestBase
    {
        #region Variables

        #endregion //Variables
        
        #region Contructors

        #endregion //Contructors        

        #region Test Cases

        [Test, DynamicRetry]
        [Description("Init Back Office default values for testing")]
        public void InitDefaultValues()
        {
            var loginPage = Browser.OpenBackOfficeApp();
            var backOfficePage = loginPage.LoginAsValidUserToBackOffice(Settings.Users["admin"].Username, Settings.Users["admin"].Password);
            var defaultAttributes = backOfficePage.GetAttributesKey();

            #region General           

            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration("General");
            backOfficePage.BackOfficeDetailsPanel.EnterGeneralTimeoutAuthenticationNumericInput("120");
            backOfficePage.BackOfficeDetailsPanel.EnterGeneralExportCsvSeparatorInput(";");
            backOfficePage.BackOfficeDetailsPanel.ClickSaveButton();
            backOfficePage.WaitForPreviousActionComplete();

            #endregion //General

            #region Desktop

            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration("Desktop");

            //Applications List
            backOfficePage.BackOfficeDetailsPanel.CheckApps(App.EquipmentInventory, App.RealTimeControl, App.FailureTracking, App.Users, App.AlarmManager, App.Alarms, App.AdvancedSearch, App.DeviceHistory, App.SchedulingManager);

            //Widgets List
            backOfficePage.BackOfficeDetailsPanel.CheckAllWidgets();
            backOfficePage.BackOfficeDetailsPanel.UncheckWidgets(Widget.Weather);
            
            backOfficePage.BackOfficeDetailsPanel.ClickSaveButton();
            backOfficePage.WaitForPreviousActionComplete();

            #endregion //Desktop

            #region Equipment Inventory

            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.EquipmentInventory);

            //Treeview configuration
            backOfficePage.BackOfficeDetailsPanel.TickEquipmentDisplayDevicesCheckbox(true);
            backOfficePage.BackOfficeDetailsPanel.TickEquipmentMapFilterCheckbox(true);
            //Editor configuration
            backOfficePage.BackOfficeDetailsPanel.TickEquipmentDeviceLocationCheckbox(false);
            backOfficePage.BackOfficeDetailsPanel.TickEquipmentParentGeozoneCheckbox(true);
            //Report configuration
            backOfficePage.BackOfficeDetailsPanel.TickEquipmentEventTimeVisibleCheckbox(false);
            backOfficePage.BackOfficeDetailsPanel.EnterEquipmentRowsPerPageNumericInput("200");
            backOfficePage.BackOfficeDetailsPanel.CheckEquipmentToolbarItems("Timestamp", "Export");
            backOfficePage.BackOfficeDetailsPanel.UncheckEquipmentToolbarItems("Maximum Date");

            backOfficePage.BackOfficeDetailsPanel.ClickSaveButton();
            backOfficePage.WaitForPreviousActionComplete();

            #endregion //Equipment Inventory

            #region Real-time Control

            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.RealTimeControl);

            //Treeview configuration
            backOfficePage.BackOfficeDetailsPanel.TickRealtimeDisplayDevicesCheckbox(true);
            backOfficePage.BackOfficeDetailsPanel.TickRealtimeMapFilterCheckbox(true);
            //Map configuration
            //Update Max refresh and zoom level later

            backOfficePage.BackOfficeDetailsPanel.ClickSaveButton();
            backOfficePage.WaitForPreviousActionComplete();

            #endregion //Real-time Control

            #region Data History

            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.DataHistory);

            //Treeview configuration
            backOfficePage.BackOfficeDetailsPanel.TickDataHistoryDisplayDevicesCheckbox(true);
            //Report configuration
            backOfficePage.BackOfficeDetailsPanel.TickDataHistoryEventTimeVisibleCheckbox(false);
            backOfficePage.BackOfficeDetailsPanel.EnterDataHistoryRowsPerPageNumericInput("200");
            backOfficePage.BackOfficeDetailsPanel.CheckDataHistoryToolbarItems("Timestamp", "Export");
            backOfficePage.BackOfficeDetailsPanel.UncheckDataHistoryToolbarItems("Maximum Date");

            backOfficePage.BackOfficeDetailsPanel.ClickSaveButton();
            backOfficePage.WaitForPreviousActionComplete();

            #endregion //Data History

            #region Device History

            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.DeviceHistory);

            //Treeview configuration
            backOfficePage.BackOfficeDetailsPanel.TickDeviceHistoryDisplayDevicesCheckbox(true);
            //Report configuration
            backOfficePage.BackOfficeDetailsPanel.TickDeviceHistoryEventTimeVisibleCheckbox(false);
            backOfficePage.BackOfficeDetailsPanel.EnterDeviceHistoryRowsPerPageNumericInput("200");

            backOfficePage.BackOfficeDetailsPanel.ClickSaveButton();
            backOfficePage.WaitForPreviousActionComplete();

            #endregion //Device History

            #region Failure Analysis

            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.FailureAnalysis);
            backOfficePage.BackOfficeDetailsPanel.TickFailureAnalysisDisplayDevicesCheckbox(true);
            backOfficePage.BackOfficeDetailsPanel.ClickSaveButton();
            backOfficePage.WaitForPreviousActionComplete();

            #endregion //Failure Analysis

            #region Failure Tracking

            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.FailureTracking);
            backOfficePage.BackOfficeDetailsPanel.TickFailureTrackingDisplayDevicesCheckbox(true);
            backOfficePage.BackOfficeDetailsPanel.TickFailureTrackingMapFilterCheckbox(true);
            backOfficePage.BackOfficeDetailsPanel.ClickSaveButton();
            backOfficePage.WaitForPreviousActionComplete();

            #endregion //Failure Tracking

            #region Advanced Search

            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.AdvancedSearch);

            //Report configuration
            backOfficePage.BackOfficeDetailsPanel.TickAdvancedSearchEventTimeVisibleCheckbox(false);
            backOfficePage.BackOfficeDetailsPanel.EnterAdvancedSearchRowsPerPageNumericInput("200");
            backOfficePage.BackOfficeDetailsPanel.CheckAdvancedSearchToolbarItems("Timestamp", "Export");
            backOfficePage.BackOfficeDetailsPanel.UncheckAdvancedSearchToolbarItems("Maximum Date");

            backOfficePage.BackOfficeDetailsPanel.ClickSaveButton();
            backOfficePage.WaitForPreviousActionComplete();

            #endregion //Advanced Search
        }

        [Test, DynamicRetry]
        [Description("Init more users for Report Manager testing")]
        public void InitTestingUsers()
        {
            CreateNewTestingUser("UserRM1");
            CreateNewTestingUser("UserRM2");
        }

        #endregion //Test Cases

        #region Private methods       

        #endregion //Private methods
    }
}
