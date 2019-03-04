using NUnit.Framework;
using StreetlightVision.Extensions;
using StreetlightVision.Models;
using StreetlightVision.Pages;
using StreetlightVision.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace StreetlightVision.Tests.Coverage.Apps
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    public class DeviceHistoryAppTests : TestBase
    {
        #region Variables

        private string _deviceHistoryExportedFilePattern = "*DeviceHistory-Export.csv";

        #endregion //Variables

        #region Contructors

        #endregion //Contructors

        #region Test Cases

        [Test, DynamicRetry]
        [Description("DE_01 Advanced search grid - Refresh")]
        public void DE_01()
        {
            var template = "Generated in (.*) seconds";
            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);

            Step("1. Go to Device History app");
            Step("2. Expected Device History page is routed and loaded successfully");
            desktopPage.InstallAppsIfNotExist(App.DeviceHistory);
            var deviceHistoryPage = desktopPage.GoToApp(App.DeviceHistory) as DeviceHistoryPage;

            Step("3. Wait for grid data loading complete then note the message 'Generated in {{duration}} seconds'");
            deviceHistoryPage.GridPanel.WaitForLeftFooterTextDisplayed();
            var leftFooterText1 = deviceHistoryPage.GridPanel.GetFooterLeftText();
            var leftFooter1Regex = Regex.Match(leftFooterText1, template);
            var notedDuration1 = leftFooter1Regex.Groups[1];
            VerifyEqual("3. Verify loaded time text as expected", true, leftFooter1Regex.Success);

            Step("4. Click Refresh");
            deviceHistoryPage.GridPanel.ClickReloadDataToolbarButton();
            deviceHistoryPage.WaitForPreviousActionComplete();
            deviceHistoryPage.GridPanel.WaitForLeftFooterTextDisplayed();

            Step("5. Expected Data is reloaded, verify {{duration}} in message 'Generated in {{duration}} message' has changed");
            var leftFooterText2 = deviceHistoryPage.GridPanel.GetFooterLeftText();
            var leftFooter2Regex = Regex.Match(leftFooterText2, template);
            var notedDuration2 = leftFooter2Regex.Groups[1];
            VerifyEqual("[SC-735] 5. Verify loaded time text", true, leftFooter2Regex.Success);
            VerifyTrue("5. Verify {{duration}} in message 'Generated in {{duration}} message' has changed", notedDuration1 != notedDuration2, notedDuration1, notedDuration2);
        }

        [Test, DynamicRetry]
        [Description("DE_02 Advanced search grid - Show hide columns")]
        public void DE_02()
        {
            var expectedColumnList = GetAvailableGridColumns();

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);

            Step("1. Go to Device History app");
            Step("2. Expected Device History page is routed and loaded successfully");
            desktopPage.InstallAppsIfNotExist(App.DeviceHistory);
            var deviceHistoryPage = desktopPage.GoToApp(App.DeviceHistory) as DeviceHistoryPage;

            Step("3. Select a geozone and wait for grid data is loaded");
            deviceHistoryPage.GeozoneTreeMainPanel.SelectNode(Settings.RootGeozoneName);
            deviceHistoryPage.GridPanel.WaitForLeftFooterTextDisplayed();

            Step("4. Click 'Show/Hide columns' button");
            var actualColumnList = deviceHistoryPage.GridPanel.GetAllColumnsInShowHideColumnsMenu();
            actualColumnList.Remove("Line #");
            expectedColumnList.Sort();
            actualColumnList.Sort();
            VerifyEqual("4. Verify available column list", expectedColumnList, actualColumnList);

            var tblGrid = deviceHistoryPage.GridPanel.BuildDataTableFromGrid();
            var checkedColumns = deviceHistoryPage.GridPanel.GetAllCheckedColumnsInShowHideColumnsMenu();
            // This column is not available in shown hide columns menu
            checkedColumns.Insert(0, "Device");
            var shownColumns = tblGrid.Columns.Cast<DataColumn>().Select(c => c.ColumnName).ToList();
            VerifyEqual("4. Verify checked and shown columns", checkedColumns, shownColumns);

            Step("5. Check 1 new column");
            Step("6. **Expected** Verify checked columns in list vs columns displayed in grid");
            deviceHistoryPage.GridPanel.CheckColumnsInShowHideColumnsMenu(actualColumnList.PickRandom());
            tblGrid = deviceHistoryPage.GridPanel.BuildDataTableFromGrid();
            checkedColumns = deviceHistoryPage.GridPanel.GetAllCheckedColumnsInShowHideColumnsMenu();
            // This column is not available in shown hide columns menu
            checkedColumns.Insert(0, "Device");
            shownColumns = tblGrid.Columns.Cast<DataColumn>().Select(c => c.ColumnName).ToList();
            VerifyEqual("6. Verify checked and shown columns", checkedColumns, shownColumns);

            Step("7. Uncheck a different column");
            Step("8. **Expected** Verify checked columns in list vs columns displayed in grid");
            deviceHistoryPage.GridPanel.CheckColumnsInShowHideColumnsMenu(actualColumnList.PickRandom());
            tblGrid = deviceHistoryPage.GridPanel.BuildDataTableFromGrid();
            checkedColumns = deviceHistoryPage.GridPanel.GetAllCheckedColumnsInShowHideColumnsMenu();
            // This column is not available in shown hide columns menu
            checkedColumns.Insert(0, "Device");
            shownColumns = tblGrid.Columns.Cast<DataColumn>().Select(c => c.ColumnName).ToList();
            VerifyEqual("8. Verify checked and shown columns", checkedColumns, shownColumns);

            Step("9. Uncheck all columns");
            Step("10. **Expected** Verify checked columns in list vs columns displayed in grid");
            deviceHistoryPage.GridPanel.UncheckAllColumnsInShowHideColumnsMenu();
            tblGrid = deviceHistoryPage.GridPanel.BuildDataTableFromGrid();
            checkedColumns = deviceHistoryPage.GridPanel.GetAllCheckedColumnsInShowHideColumnsMenu();
            // This column is not available in shown hide columns menu
            checkedColumns.Insert(0, "Device");
            shownColumns = tblGrid.Columns.Cast<DataColumn>().Select(c => c.ColumnName).ToList();
            VerifyEqual("10. Verify checked and shown columns", checkedColumns, shownColumns);

            Step("11. Check all columns");
            Step("12. **Expected** Verify checked columns in list vs columns displayed in grid");
            deviceHistoryPage.GridPanel.CheckAllColumnsInShowHideColumnsMenu();
            tblGrid = deviceHistoryPage.GridPanel.BuildDataTableFromGrid();
            checkedColumns = deviceHistoryPage.GridPanel.GetAllCheckedColumnsInShowHideColumnsMenu();
            // This column is not available in shown hide columns menu
            checkedColumns.Insert(0, "Device");
            checkedColumns.Remove("Line #");
            shownColumns = tblGrid.Columns.Cast<DataColumn>().Select(c => c.ColumnName).ToList();
            shownColumns.Remove("Line #");
            VerifyEqual("12. Verify checked and shown columns", checkedColumns, shownColumns);
        }

        [Test, DynamicRetry]
        [Description("DE_03 Advanced search grid - Advanced filter")]
        public void DE_03()
        {
            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);

            Step("1. Go to Device History app");
            Step("2. Expected Device History page is routed and loaded successfully");
            desktopPage.InstallAppsIfNotExist(App.DeviceHistory);
            var deviceHistoryPage = desktopPage.GoToApp(App.DeviceHistory) as DeviceHistoryPage;

            Step("3. Click Search icon button");
            var expectedSearchFields = deviceHistoryPage.GridPanel.GetAllColumnsInShowHideColumnsMenu();
            deviceHistoryPage.GridPanel.HideShowHideColumnsMenu();

            deviceHistoryPage.GridPanel.ClickSearchToolbarButton();
            deviceHistoryPage.GridPanel.WaitForAdvancedSearchPanelDisplayed();

            Step("4. Verify");
            Step(" o The attribute 'Device' will always appear first");
            Step(" o Attributes are listed in alphabetical order");
            var listSearchFields = deviceHistoryPage.GridPanel.GetListOfAdvancedSearchFields();
            var firstField = listSearchFields.First();
            listSearchFields.RemoveAt(0);
            var firstCharList = listSearchFields.Select(p => p[0].ToString()).ToList();
            VerifyEqual("4. Verify The attribute 'Device' will always appear first", "Device", firstField);
            VerifyEqual("4. Verify Attributes are listed in alphabetical order", true, firstCharList.IsIncreasing());

            Step("5. Expected Advanced search popup appears");
            Step(" o Verify list of search fields in this popup and in Show/Hide columns popup are equa");
            Step(" o Verify there is no section 'Current search criteria' and 'search conditions' displayed");
            expectedSearchFields.Remove("Line #");
            expectedSearchFields.Insert(0, "Device");
            var actualSearchFields = deviceHistoryPage.GridPanel.GetListOfAdvancedSearchFields();

            VerifyEqual("5. Advanced search popup appears", true, deviceHistoryPage.GridPanel.IsAdvancedSearchPopupDisplayed());
            VerifyEqual("5. - Verify list of search fields in this popup and in Show/Hide columns popup are equal", expectedSearchFields, actualSearchFields);
            VerifyEqual("5. - Verify there is no section 'Current search criteria' and 'search conditions' displayed", false, deviceHistoryPage.GridPanel.IsSearchCriteriaSectionDisplayed());

            // Temporarily enter device name only
            Step("6. Enter all edit fields then hit Manifier icon at the bottom");
            Step("7. Expected The advanced search panel disappears");
            var searchValue = "STL";
            deviceHistoryPage.GridPanel.EnterSearchCriteriaFor1stValueInputField("Device", searchValue);
            deviceHistoryPage.GridPanel.ClickAdvancedSearchSearchButton();
            VerifyEqual("7. Verify Advanced search popup appears", false, deviceHistoryPage.GridPanel.IsAdvancedSearchPopupDisplayed());

            Step("8. Click Search (Manifier icon) button again");
            deviceHistoryPage.GridPanel.WaitForSearchToolbarButtonEnabled();
            deviceHistoryPage.GridPanel.ClickSearchToolbarButton();
            deviceHistoryPage.GridPanel.WaitForAdvancedSearchPanelDisplayed();

            Step(@"9. Expected Section 'Current search criteria' and 'search conditions' is displayed: search condition text looks like '{ Field name 1} {operator 1} { value 1},...,{ Field name n} {operator n} { value n}'");
            VerifyEqual("9. Verify Section 'Current search criteria' and 'search conditions' is displayed", true, deviceHistoryPage.GridPanel.IsSearchCriteriaSectionDisplayed());
            VerifyEqual("9. Verify Search criteria text", string.Format("Current search criteria:Device contains {0}", searchValue), deviceHistoryPage.GridPanel.GetSearchCriteria());

            Step("10. Click Reset icon in advanced search panel");
            Step("11. Expected The advanced search panel disappears");
            deviceHistoryPage.GridPanel.ClickAdvancedSearchResetButton();
            VerifyEqual("11. Verify Advanced search popup appears", false, deviceHistoryPage.GridPanel.IsAdvancedSearchPopupDisplayed());

            Step("12. Click Search (Manifier icon) button again");
            Step("13. Expected Advanced search panel appears. There is no section 'Current search criteria' and 'search conditions' displayed");
            deviceHistoryPage.GridPanel.WaitForSearchToolbarButtonEnabled();
            deviceHistoryPage.GridPanel.ClickSearchToolbarButton();
            deviceHistoryPage.GridPanel.WaitForAdvancedSearchPanelDisplayed();
            VerifyEqual("13. Verify there is no section 'Current search criteria' and 'search conditions' displayed", false, deviceHistoryPage.GridPanel.IsSearchCriteriaSectionDisplayed());
        }

        [Test, DynamicRetry]
        [Description("DE_04 Advanced search grid - Equipment Inventory shortcut")]
        public void DE_04()
        {
            var geozone = SLVHelper.GenerateUniqueName("GZNDE04");
            var controller = SLVHelper.GenerateUniqueName("CTRL");
            var streetlight = SLVHelper.GenerateUniqueName("STL");

            Step("-> Create data for testing");
            DeleteGeozones("GZNDE04*");
            CreateNewGeozone(geozone);
            CreateNewController(controller, geozone);
            CreateNewDevice(DeviceType.Streetlight, streetlight, controller, geozone);

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);

            Step("1. Go to Device History app");
            Step("2. Expected Device History page is routed and loaded successfully");
            desktopPage.InstallAppsIfNotExist(App.DeviceHistory);
            var deviceHistoryPage = desktopPage.GoToApp(App.DeviceHistory) as DeviceHistoryPage;

            Step("3. Select a geozone to load grid data");
            deviceHistoryPage.GeozoneTreeMainPanel.SelectNode(geozone);

            Step("4. Double-click a device (row) in grid");
            var deviceList = deviceHistoryPage.GridPanel.GetListOfColumnData("Device");
            var pickedDevice = deviceList.PickRandom();
            deviceHistoryPage.GridPanel.DoubleClickGridRecord(pickedDevice);

            Step(@"5. **Expected** An overlay appears:
                       - Close button
                       - Device label: name of the selected device
                       - 4 shortcuts to apps: Equipment Inventory, Real-Time Control, Data History, Failure Tracking");
            var expectedShortcuts = new List<string>() { App.EquipmentInventory, App.RealTimeControl, App.DataHistory, App.FailureTracking };
            var actualShortcuts = deviceHistoryPage.SwitcherOverlayPanel.GetListOfAppsToSwitch();
            VerifyEqual("5. An overlay appears", true, deviceHistoryPage.SwitcherOverlayPanel.IsPanelDisplayed());
            VerifyEqual("5. Close button", true, deviceHistoryPage.SwitcherOverlayPanel.IsCloseButtonDisplayed());
            VerifyEqual("5. Device label", pickedDevice, deviceHistoryPage.SwitcherOverlayPanel.GetPanelHeaderText());
            VerifyEqual("5. Shortcuts", expectedShortcuts, actualShortcuts);

            Step("6. Click Close button");
            Step("7. **Expected** The overlay is closed");
            deviceHistoryPage.SwitcherOverlayPanel.ClickSwitcherCancelButton();
            VerifyEqual("7. An overlay appears", false, deviceHistoryPage.SwitcherOverlayPanel.IsPanelDisplayed());

            Step("8. Double-click a device (row) in grid");
            Step("9. **Expected** An overlay appears again");
            deviceHistoryPage.GridPanel.DoubleClickGridRecord(pickedDevice);
            VerifyEqual("9. An overlay appears", true, deviceHistoryPage.SwitcherOverlayPanel.IsPanelDisplayed());

            Step("10. Click Equipment Inventory");
            Step("11. **Expected** Equipment Inventory app is switched to. The selected device is being selected in geozone tree and map. Device details panel for that device appears");
            var equipmentInventoryPage = deviceHistoryPage.SwitcherOverlayPanel.SwitchToEquipmentInventoryApp();
            VerifyEqual("11. The selected device is being selected in geozone tree and map", pickedDevice, equipmentInventoryPage.GeozoneTreeMainPanel.GetSelectedNodeName());
            VerifyEqual("11. Device details panel for that device appears", true, equipmentInventoryPage.DeviceEditorPanel.IsDisplayed());
            VerifyEqual("11. Device details panel for that device appears", pickedDevice, equipmentInventoryPage.DeviceEditorPanel.GetNameValue());

            try
            {
                DeleteGeozone(geozone);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("DE_05 Advanced search grid - Real-time Control shortcut")]
        public void DE_05()
        {
            var geozone = SLVHelper.GenerateUniqueName("GZNDE05");
            var controller = SLVHelper.GenerateUniqueName("CTRL");
            var streetlight = SLVHelper.GenerateUniqueName("STL");

            Step("-> Create data for testing");
            DeleteGeozones("GZNDE05*");
            CreateNewGeozone(geozone);
            CreateNewController(controller, geozone);
            CreateNewDevice(DeviceType.Streetlight, streetlight, controller, geozone);

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);

            Step("1. Go to Device History app");
            Step("2. Expected Device History page is routed and loaded successfully");
            desktopPage.InstallAppsIfNotExist(App.DeviceHistory);
            var deviceHistoryPage = desktopPage.GoToApp(App.DeviceHistory) as DeviceHistoryPage;           

            Step("3. Select a geozone to load grid data");
            deviceHistoryPage.GeozoneTreeMainPanel.SelectNode(geozone);

            Step("4. Double-click a device (row) in grid");
            var deviceList = deviceHistoryPage.GridPanel.GetListOfColumnData("Device");
            var pickedDevice = deviceList.PickRandom();
            deviceHistoryPage.GridPanel.DoubleClickGridRecord(pickedDevice);

            Step(@"5. **Expected** An overlay appears:
                       - Close button
                       - Device label: name of the selected device
                       - 4 shortcuts to apps: Equipment Inventory, Real-Time Control, Data History, Failure Tracking");
            var expectedShortcuts = new List<string>() { App.EquipmentInventory, App.RealTimeControl, App.DataHistory, App.FailureTracking };
            var actualShortcuts = deviceHistoryPage.SwitcherOverlayPanel.GetListOfAppsToSwitch();
            VerifyEqual("5. Verify An overlay appears", true, deviceHistoryPage.SwitcherOverlayPanel.IsPanelDisplayed());
            VerifyEqual("5. Verify Close button", true, deviceHistoryPage.SwitcherOverlayPanel.IsCloseButtonDisplayed());
            VerifyEqual("5. Verify Device label", pickedDevice, deviceHistoryPage.SwitcherOverlayPanel.GetPanelHeaderText());
            VerifyEqual("5. Verify Shortcuts", expectedShortcuts, actualShortcuts);

            Step("6. Click Close button");
            Step("7. **Expected** The overlay is closed");
            deviceHistoryPage.SwitcherOverlayPanel.ClickSwitcherCancelButton();
            VerifyEqual("7. Verify An overlay appears", false, deviceHistoryPage.SwitcherOverlayPanel.IsPanelDisplayed());

            Step("8. Double-click a device (row) in grid");
            Step("9. **Expected** An overlay appears again");
            deviceHistoryPage.GridPanel.DoubleClickGridRecord(pickedDevice);
            VerifyEqual("9. Verify An overlay appears", true, deviceHistoryPage.SwitcherOverlayPanel.IsPanelDisplayed());

            Step("10. Click Real-time Control");
            Step("11. **Expected** Real-time app is switched to. The selected device is being selected in geozone tree and map. Device controller panel for that device appears");
            var realtimeControlPage = deviceHistoryPage.SwitcherOverlayPanel.SwitchToRealtimeControlApp();
            realtimeControlPage.WaitForStreetlightWidgetDisplayed(pickedDevice);
            VerifyEqual("11. Verify The selected device is being selected in geozone tree and map", pickedDevice, realtimeControlPage.GeozoneTreeMainPanel.GetSelectedNodeName());
            VerifyEqual("11. Verify Device controller panel for that device appears", pickedDevice, realtimeControlPage.DeviceWidgetPanel.GetDeviceNameText());

            try
            {
                DeleteGeozone(geozone);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("DE_6 Advanced search grid - Data history shortcut")]
        public void DE_06()
        {
            var geozone = SLVHelper.GenerateUniqueName("GZNDE06");
            var controller = SLVHelper.GenerateUniqueName("CTRL");
            var streetlight = SLVHelper.GenerateUniqueName("STL");

            Step("-> Create data for testing");
            DeleteGeozones("GZNDE06*");
            CreateNewGeozone(geozone);
            CreateNewController(controller, geozone);
            CreateNewDevice(DeviceType.Streetlight, streetlight, controller, geozone);

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);

            Step("1. Go to Device History app");
            Step("2. Expected Device History page is routed and loaded successfully");
            desktopPage.InstallAppsIfNotExist(App.DeviceHistory);
            var deviceHistoryPage = desktopPage.GoToApp(App.DeviceHistory) as DeviceHistoryPage;

            Step("3. Select a geozone to load grid data");
            deviceHistoryPage.GeozoneTreeMainPanel.SelectNode(geozone);

            Step("4. Double-click a device (row) in grid");
            var deviceList = deviceHistoryPage.GridPanel.GetListOfColumnData("Device");
            var pickedDevice = deviceList.PickRandom();
            deviceHistoryPage.GridPanel.DoubleClickGridRecord(pickedDevice);

            Step(@"5. **Expected** An overlay appears:
                       - Close button
                       - Device label: name of the selected device
                       - 4 shortcuts to apps: Equipment Inventory, Real-Time Control, Data History, Failure Tracking");
            var expectedShortcuts = new List<string>() { App.EquipmentInventory, App.RealTimeControl, App.DataHistory, App.FailureTracking };
            var actualShortcuts = deviceHistoryPage.SwitcherOverlayPanel.GetListOfAppsToSwitch();
            VerifyEqual("5. An overlay appears", true, deviceHistoryPage.SwitcherOverlayPanel.IsPanelDisplayed());
            VerifyEqual("5. Close button", true, deviceHistoryPage.SwitcherOverlayPanel.IsCloseButtonDisplayed());
            VerifyEqual("5. Device label", pickedDevice, deviceHistoryPage.SwitcherOverlayPanel.GetPanelHeaderText());
            VerifyEqual("5. Shortcuts", expectedShortcuts, actualShortcuts);

            Step("6. Click Close button");
            Step("7. **Expected** The overlay is closed");
            deviceHistoryPage.SwitcherOverlayPanel.ClickSwitcherCancelButton();
            VerifyEqual("7. An overlay appears", false, deviceHistoryPage.SwitcherOverlayPanel.IsPanelDisplayed());

            Step("8. Double-click a device (row) in grid");
            Step("9. **Expected** An overlay appears again");
            deviceHistoryPage.GridPanel.DoubleClickGridRecord(pickedDevice);
            VerifyEqual("9. An overlay appears", true, deviceHistoryPage.SwitcherOverlayPanel.IsPanelDisplayed());

            Step("10. Click Data History");
            Step("11. **Expected** Data History is switched to. The selected device is being selected in grid. Last values panel appears and displays data for the selected device.");
            var dataHistoryPage = deviceHistoryPage.SwitcherOverlayPanel.SwitchToDataHistoryApp();
            dataHistoryPage.WaitForLastValuePanelDisplayed();
            VerifyEqual("11. The selected device is being selected in last value panel", pickedDevice, dataHistoryPage.LastValuesPanel.GetSelectedDeviceText());

            try
            {
                DeleteGeozone(geozone);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("DE_07 Advanced search grid - Failure Tracking shortcut")]
        public void DE_07()
        {
            var geozone = SLVHelper.GenerateUniqueName("GZNDE07");
            var controller = SLVHelper.GenerateUniqueName("CTRL");
            var streetlight = SLVHelper.GenerateUniqueName("STL");

            Step("-> Create data for testing");
            DeleteGeozones("GZNDE07*");
            CreateNewGeozone(geozone);
            CreateNewController(controller, geozone);
            CreateNewDevice(DeviceType.Streetlight, streetlight, controller, geozone);

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);

            Step("1. Go to Device History app");
            Step("2. Expected Device History page is routed and loaded successfully");
            desktopPage.InstallAppsIfNotExist(App.DeviceHistory);
            var deviceHistoryPage = desktopPage.GoToApp(App.DeviceHistory) as DeviceHistoryPage;

            Step("3. Select a geozone to load grid data");
            deviceHistoryPage.GeozoneTreeMainPanel.SelectNode(geozone);

            Step("4. Double-click a device (row) in grid");
            var deviceList = deviceHistoryPage.GridPanel.GetListOfColumnData("Device");
            var pickedDevice = deviceList.PickRandom();
            deviceHistoryPage.GridPanel.DoubleClickGridRecord(pickedDevice);

            Step(@"5. **Expected** An overlay appears:
                       - Close button
                       - Device label: name of the selected device
                       - 4 shortcuts to apps: Equipment Inventory, Real-Time Control, Data History, Failure Tracking");
            var expectedShortcuts = new List<string>() { App.EquipmentInventory, App.RealTimeControl, App.DataHistory, App.FailureTracking };
            var actualShortcuts = deviceHistoryPage.SwitcherOverlayPanel.GetListOfAppsToSwitch();
            VerifyEqual("5. An overlay appears", true, deviceHistoryPage.SwitcherOverlayPanel.IsPanelDisplayed());
            VerifyEqual("5. Close button", true, deviceHistoryPage.SwitcherOverlayPanel.IsCloseButtonDisplayed());
            VerifyEqual("5. Device label", pickedDevice, deviceHistoryPage.SwitcherOverlayPanel.GetPanelHeaderText());
            VerifyEqual("5. Shortcuts", expectedShortcuts, actualShortcuts);

            Step("6. Click Close button");
            Step("7. **Expected** The overlay is closed");
            deviceHistoryPage.SwitcherOverlayPanel.ClickSwitcherCancelButton();
            VerifyEqual("7. An overlay appears", false, deviceHistoryPage.SwitcherOverlayPanel.IsPanelDisplayed());

            Step("8. Double-click a device (row) in grid");
            Step("9. **Expected** An overlay appears again");
            deviceHistoryPage.GridPanel.DoubleClickGridRecord(pickedDevice);
            VerifyEqual("9. An overlay appears", true, deviceHistoryPage.SwitcherOverlayPanel.IsPanelDisplayed());

            Step("10. Click Failure Tracking");
            Step("11. **Expected** Failure Tracking app is switched to. The selected device is being selected in geozone tree and map. Failure Tracking panel for that device appears");
            var failureTrackingPage = deviceHistoryPage.SwitcherOverlayPanel.SwitchToFailureTrackingApp();
            VerifyEqual("11. The selected device is being selected in geozone tree and map", pickedDevice, failureTrackingPage.GeozoneTreeMainPanel.GetSelectedNodeName());
            VerifyEqual("11. Failure Tracking panel for that device appears", true, failureTrackingPage.FailureTrackingDetailsPanel.IsPanelVisible());
            VerifyEqual("11. Failure Tracking panel for that device appears", pickedDevice, failureTrackingPage.FailureTrackingDetailsPanel.GetDeviceNameValueText());

            try
            {
                DeleteGeozone(geozone);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("DE_08 Device history panel - Select device from geozone tree")]
        public void DE_08()
        {
            var testData = GetTestDataOfTestDE_08();
            var deviceWithFullPath = testData["Device"];
            var deviceParts = deviceWithFullPath.Split(new char[] { '\\' });
            var deviceNameOnly = deviceParts[deviceParts.Length - 1];

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);

            Step("1. Go to Device History app");
            Step("2. Expected Device History page is routed and loaded successfully");
            desktopPage.InstallAppsIfNotExist(App.DeviceHistory);
            var deviceHistoryPage = desktopPage.GoToApp(App.DeviceHistory) as DeviceHistoryPage;

            Step("3. Select a device from geozone tree");
            deviceHistoryPage.GeozoneTreeMainPanel.SelectNode(deviceWithFullPath);

            Step(@"4. **Expected** Device history panel appears:
                       - Back button
                       - Title: name of selected device");
            deviceHistoryPage.WaitForDeviceHistoryPanelDisplayed();
            VerifyEqual("Device history panel appears", true, deviceHistoryPage.IsDeviceHistoryPanelDisplayed());
            VerifyEqual("Panel title = selected device", deviceNameOnly, deviceHistoryPage.DeviceHistoryPanel.GetDeviceNameTitleText());

            Step("5. Click Back button");
            Step("6. **Expected** The panel disappears");
            deviceHistoryPage.DeviceHistoryPanel.ClickBackButton();
            deviceHistoryPage.WaitForDeviceHistoryPanelDisappeared();
            VerifyEqual("6. Device history panel disappears", false, deviceHistoryPage.IsDeviceHistoryPanelDisplayed());

            Step("7. Select the device from tree again");
            Step("8. **Expected** The panel appears");
            deviceHistoryPage.GeozoneTreeMainPanel.SelectNode(deviceNameOnly);
            deviceHistoryPage.WaitForDeviceHistoryPanelDisplayed();
            VerifyEqual("[SLV-3086] 8. Device history panel appears after select the device again", true, deviceHistoryPage.IsDeviceHistoryPanelDisplayed());
        }

        [Test, DynamicRetry]
        [Description("DE_09 Device history panel - Select device from grid")]
        public void DE_09()
        {
            var testData = GetTestDataOfTestDE_09();
            var geozone = testData["Geozone"];
            
            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);

            Step("1. Go to Device History app");
            Step("2. Expected Device History page is routed and loaded successfully");
            desktopPage.InstallAppsIfNotExist(App.DeviceHistory);
            var deviceHistoryPage = desktopPage.GoToApp(App.DeviceHistory) as DeviceHistoryPage;

            Step("3. Select a geozone then a device of the selected geozone from grid");
            deviceHistoryPage.GeozoneTreeMainPanel.SelectNode(geozone);
            var deviceList = deviceHistoryPage.GridPanel.GetListOfColumnData("Device");
            var device = deviceList.PickRandom();
            deviceHistoryPage.GridPanel.ClickGridRecord(device);

            Step(@"4. **Expected** Device history panel appears:
                       - Back button
                       - Title: name of selected device");
            deviceHistoryPage.WaitForDeviceHistoryPanelDisplayed();
            VerifyEqual("4. Device history panel appears", true, deviceHistoryPage.IsDeviceHistoryPanelDisplayed());
            VerifyEqual("4. Panel title = selected device", device, deviceHistoryPage.DeviceHistoryPanel.GetDeviceNameTitleText());

            Step("5. Click Back button");
            Step("6. **Expected** The panel disappears");
            deviceHistoryPage.DeviceHistoryPanel.ClickBackButton();
            deviceHistoryPage.WaitForDeviceHistoryPanelDisappeared();
            VerifyEqual("6. Device history panel disappears", false, deviceHistoryPage.IsDeviceHistoryPanelDisplayed());

            Step("7. Select the device from grid again");
            Step("8. **Expected** The panel appears");
            deviceHistoryPage.GridPanel.ClickGridRecord(device);
            deviceHistoryPage.WaitForDeviceHistoryPanelDisplayed();
            VerifyEqual("8. Device history panel appears after select the device again", true, deviceHistoryPage.IsDeviceHistoryPanelDisplayed());
        }

        [Test, DynamicRetry]
        [Description("DE_10 Device history panel - Column filter")]
        public void DE_10()
        {
            var testData = GetTestDataOfTestDE_10();
            var geozone = testData["Geozone"].ToString();
            var streetlights = testData["Streetlights"] as List<DeviceModel>;

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);

            Step("1. Go to Device History app");
            Step("2. Expected Device History page is routed and loaded successfully");
            desktopPage.InstallAppsIfNotExist(App.DeviceHistory, App.EquipmentInventory);
            var deviceHistoryPage = desktopPage.GoToApp(App.DeviceHistory) as DeviceHistoryPage;

            Step("3. Select a device from geozone tree");
            deviceHistoryPage.GeozoneTreeMainPanel.SelectNode(geozone);
            var streetlight = streetlights.PickRandom();
            deviceHistoryPage.GeozoneTreeMainPanel.SelectNode(streetlight.Name);

            Step("4. Expected Device History panel appears");
            deviceHistoryPage.WaitForDeviceHistoryPanelDisplayed();
            VerifyEqual("4. Verify Device history panel appears", true, deviceHistoryPage.IsDeviceHistoryPanelDisplayed());

            Step("5. Press button Attributes filter");
            deviceHistoryPage.DeviceHistoryPanel.ClickFilterToolbarButton();
            deviceHistoryPage.DeviceHistoryPanel.WaitForAttributesFilterPanelDisplayed();

            Step("6. Verify the text changes to 'Device history' and these following options are checked as default:");
            Step(" o Dimming group");
            Step(" o Unique address");
            Step(" o Install status");
            Step(" o Configuration status");
            Step(" o Address 1");
            var expectedAttributes = new List<string> { "Dimming group", "Unique address", "Install status", "Configuration status", "Address 1" };
            var actualCheckedAttributes = deviceHistoryPage.DeviceHistoryPanel.GetListOfCheckedAttributes();
            VerifyEqual("6. Verify the text changes to 'Device history' and these following options are checked as default", expectedAttributes, actualCheckedAttributes, false);

            Step("7. Select randomly an option, and press button Device History");
            var uncheckedAttributes = deviceHistoryPage.DeviceHistoryPanel.GetListOfUncheckedAttributes();
            var randomAttribute = uncheckedAttributes.PickRandom();
            deviceHistoryPage.DeviceHistoryPanel.CheckFilterAttributes(randomAttribute);
            deviceHistoryPage.DeviceHistoryPanel.ClickFilterToolbarButton();
            deviceHistoryPage.DeviceHistoryPanel.WaitForAttributesFilterPanelDisappeared();

            Step("8. Verify The text changes to 'Attributes filter'");
            VerifyEqual("8. Verify The text changes to 'Attributes filter'", "Attributes filter", deviceHistoryPage.DeviceHistoryPanel.GetFilterTitleText());

            Step("9. Select another device in another geozone");
            var anotherStreetlight = streetlights.Where(p => !p.Equals(streetlight)).PickRandom();
            deviceHistoryPage.GeozoneTreeMainPanel.SelectNode(anotherStreetlight.Name);

            Step("10. Press button Attributes filter");
            deviceHistoryPage.DeviceHistoryPanel.ClickFilterToolbarButton();
            deviceHistoryPage.DeviceHistoryPanel.WaitForAttributesFilterPanelDisplayed();

            Step("11. Verify the text changes to 'Device history' and these following options are checked:");
            Step(" o Dimming group");
            Step(" o Unique address");
            Step(" o Install status");
            Step(" o Configuration status");
            Step(" o Address 1");
            Step(" o The new option selected");
            expectedAttributes.Add(randomAttribute);
            actualCheckedAttributes = deviceHistoryPage.DeviceHistoryPanel.GetListOfCheckedAttributes();
            VerifyEqual("11. Verify the text changes to 'Device history' and following options are checked", expectedAttributes, actualCheckedAttributes, false);

            Step("12. Press button Device History, and press Application Switcher then select Eqiupment Inventory app");
            deviceHistoryPage.DeviceHistoryPanel.ClickFilterToolbarButton();
            deviceHistoryPage.DeviceHistoryPanel.WaitForAttributesFilterPanelDisappeared();
            var equipmentInventory = deviceHistoryPage.AppBar.SwitchTo(App.EquipmentInventory) as EquipmentInventoryPage;

            Step("13. Press Application Switcher and select Device History app again");
            deviceHistoryPage = equipmentInventory.AppBar.SwitchTo(App.DeviceHistory) as DeviceHistoryPage;

            Step("14. Press button Attributes filter");
            deviceHistoryPage.DeviceHistoryPanel.ClickFilterToolbarButton();
            deviceHistoryPage.DeviceHistoryPanel.WaitForAttributesFilterPanelDisplayed();

            Step("15. Verify select options are unchanged.");
            actualCheckedAttributes = deviceHistoryPage.DeviceHistoryPanel.GetListOfCheckedAttributes();
            VerifyEqual("15. Verify the text changes to 'Device history' and following options are checked", expectedAttributes, actualCheckedAttributes, false);

            Step("16. Refresh the page, and go to Device History app, then select a device in a geozone");
            desktopPage = Browser.RefreshLoggedInCMS();
            deviceHistoryPage = desktopPage.GoToApp(App.DeviceHistory) as DeviceHistoryPage;
            deviceHistoryPage.GeozoneTreeMainPanel.SelectNode(geozone);
            streetlight = streetlights.PickRandom();
            deviceHistoryPage.GeozoneTreeMainPanel.SelectNode(streetlight.Name);

            Step("17. Press button Attributes filter");
            deviceHistoryPage.DeviceHistoryPanel.ClickFilterToolbarButton();
            deviceHistoryPage.DeviceHistoryPanel.WaitForAttributesFilterPanelDisplayed();

            Step("18. Verify the text changes to 'Device history' and these following options are checked as default:");
            Step(" o Dimming group");
            Step(" o Unique address");
            Step(" o Install status");
            Step(" o Configuration status");
            Step(" o Address 1");
            expectedAttributes.Remove(randomAttribute);
            actualCheckedAttributes = deviceHistoryPage.DeviceHistoryPanel.GetListOfCheckedAttributes();
            VerifyEqual("18. Verify the text changes to 'Device history' and following options are checked", expectedAttributes, actualCheckedAttributes, false);
        }

        [Test, DynamicRetry]
        [Description("DE_11 Device history panel - Duration filter")]
        public void DE_11()
        {
            var testData = GetTestDataOfTestDE_11();
            var geozone = testData["Geozone"].ToString();

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);

            Step("1. Go to Device History app");
            Step("2. Expected Device History page is routed and loaded successfully");
            desktopPage.InstallAppsIfNotExist(App.DeviceHistory);
            var deviceHistoryPage = desktopPage.GoToApp(App.DeviceHistory) as DeviceHistoryPage;

            Step("3. Select a device from grid");
            deviceHistoryPage.GeozoneTreeMainPanel.SelectNode(geozone);

            var deviceList = deviceHistoryPage.GridPanel.GetListOfColumnData("Device");
            deviceList = deviceList.Where(p => p.Contains("Telematics")).ToList();
            var device = deviceList.PickRandom();
            deviceHistoryPage.GridPanel.ClickGridRecord(device);
            deviceHistoryPage.WaitForDeviceHistoryPanelDisplayed();

            Step(@"4. **Expected** Device History panel appears");
            VerifyEqual("4. Verify Device history panel appears", true, deviceHistoryPage.IsDeviceHistoryPanelDisplayed());

            Step(@"5. **Expected** Verify Duration filter has following options:
                       - Last two weeks
                       - Last month
                       - Last two months
                       - Last three months
                       - Last six months");
            var expectedDurationList = new List<string>() { "Last two weeks", "Last month", "Last two months", "Last three months", "Last six months" };
            var actualDurationList = deviceHistoryPage.DeviceHistoryPanel.GetAllDurations();
            VerifyEqual("5. Verify duration filter options", expectedDurationList, actualDurationList);

            Step("7. Verify {{Now - time value in Time cell}} is within the currently-selected duration");      
            deviceHistoryPage.DeviceHistoryPanel.WaitForLoaderSpinDisappeared();
            if (deviceHistoryPage.DeviceHistoryPanel.IsNoDataMessageDisplayed())
            {
                Warning(string.Format("No data displayed for current selected duration '{0}'", deviceHistoryPage.DeviceHistoryPanel.GetDurationValue()));
            }
            else
            {
                var entryTimeList = deviceHistoryPage.DeviceHistoryPanel.GetListOfColumnData("Time");
                var selectedDuration = deviceHistoryPage.DeviceHistoryPanel.GetDurationValue();
                foreach (var entryTime in entryTimeList)
                {
                    VerifyTimeWithinDuration(selectedDuration, entryTime);
                }
            }

            foreach (var duration in expectedDurationList)
            {
                Step("8. Select in turn each option");
                Step("9. **Expected** The grid reloads data according to selected duration. Depending on a selected duration, grid may contains rows or no row. In cases of rows, verify {{Now - time value in Time cell}} is within the selected duration");
                deviceHistoryPage.DeviceHistoryPanel.SelectDurationDropDown(duration);
                deviceHistoryPage.DeviceHistoryPanel.WaitForLoaderSpinDisappeared();
                if (deviceHistoryPage.DeviceHistoryPanel.IsNoDataMessageDisplayed())
                {
                    Warning(string.Format("No data displayed for '{0}'", duration));
                }
                else
                {
                    var  entryTimeList = deviceHistoryPage.DeviceHistoryPanel.GetListOfColumnData("Time");
                    foreach (var entryTime in entryTimeList)
                    {
                        VerifyTimeWithinDuration(duration, entryTime);
                    }
                }
            }
        }

        [Test, DynamicRetry]
        [Description("DE_12 Device history panel - Refresh")]
        public void DE_12()
        {
            var geozone = SLVHelper.GenerateUniqueName("GZNDE12");
            var controller = SLVHelper.GenerateUniqueName("CTRL");
            var streetlight = SLVHelper.GenerateUniqueName("STL");
            var listInstallStatus = new List<string>() { "To be verified", "Verified", "New", "Does not exist", "To be installed", "Installed", "Removed" };

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - Create new streetlight for testing");
            Step("**** Precondition ****\n");

            Step("-> Create data for testing");
            DeleteGeozones("GZNDE12*");
            CreateNewGeozone(geozone);
            CreateNewController(controller, geozone);
            CreateNewDevice(DeviceType.Streetlight, streetlight, controller, geozone);

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.DeviceHistory);

            Step("1. Go to Device History app");
            Step("2. Expected Device History page is routed and loaded successfully");
            var deviceHistoryPage = desktopPage.GoToApp(App.DeviceHistory) as DeviceHistoryPage;

            Step("3. Select  device from geozone tree");
            deviceHistoryPage.GeozoneTreeMainPanel.SelectNode(geozone + @"\" + streetlight);

            Step("4. Expected Device History panel appears and there is no data in grid view");
            deviceHistoryPage.WaitForDeviceHistoryPanelDisplayed();
            var table = deviceHistoryPage.DeviceHistoryPanel.BuildDataTableFromGrid();
            VerifyEqual("4. Verify Device history panel appears", true, deviceHistoryPage.IsDeviceHistoryPanelDisplayed());
            VerifyEqual("4. Verify There is no data in grid view", 0, table.Rows.Count);

            Step("5. Send a command to update Install Status of the Streetlight");
            var eventDateTime = Settings.GetServerTime();
            var notedInstallStatus = listInstallStatus.PickRandom();
            var isSentInstallStatusSuccess = SetValueToDevice(controller, streetlight, "installStatus", notedInstallStatus, eventDateTime);

            Step("6. Verify sent command successfully");
            VerifyEqual("6. Verify sent command forInstall Status successfully", true, isSentInstallStatusSuccess);

            Step("7. Click Refresh in the panel");
            deviceHistoryPage.DeviceHistoryPanel.ClickRefreshToolbarButton();
            deviceHistoryPage.DeviceHistoryPanel.WaitForLoaderSpinDisappeared();

            Step("8. Verify Grid view has 1 new record 'Install Status' added with command value");
            table = deviceHistoryPage.DeviceHistoryPanel.BuildDataTableFromGrid();
            var installStatusRow = table.Select("Name = 'Install status'").FirstOrDefault();
            VerifyEqual("8. Verify Grid view has 1 new record added", 1, table.Rows.Count);
            if (installStatusRow != null)
            {
                var actualInstallStatus = installStatusRow["Value"].ToString();
                VerifyEqual("8. Verify Install status, is " + notedInstallStatus, notedInstallStatus, actualInstallStatus);
            }
            else
            {
                Warning("8. There is no row with Install status attribute");
            }

            try
            {
                Step("Delete the test data after testcase is done.");
                DeleteGeozone(geozone);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("DE_13 Device history panel - Basic filter in grid")]
        public void DE_13()
        {
            var testData = GetTestDataOfTestDE_13();
            var geozone = testData["Geozone"];

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);

            Step("1. Go to Device History app");
            Step("2. Expected Device History page is routed and loaded successfully");
            desktopPage.InstallAppsIfNotExist(App.DeviceHistory);
            var deviceHistoryPage = desktopPage.GoToApp(App.DeviceHistory) as DeviceHistoryPage;

            Step("3. Select a device from geozone tree");
            deviceHistoryPage.GeozoneTreeMainPanel.SelectNode(geozone);
            var deviceList = deviceHistoryPage.GeozoneTreeMainPanel.GetChildDeviceNamesOfSelectedNode();
            var pickedDevice = deviceList.Where(p => p.Contains("Telematics")).PickRandom();
            deviceHistoryPage.GeozoneTreeMainPanel.SelectNode(pickedDevice);

            Step("4. **Expected** Device History panel appears");
            deviceHistoryPage.WaitForDeviceHistoryPanelDisplayed();
            VerifyEqual("Device history panel appears", true, deviceHistoryPage.IsDeviceHistoryPanelDisplayed());

            Step("5. Enter partially or completely the matched time value in grid into Search text field then hit enter");
            Step("6. **Expected** Matched time rows are displayed");
            Step("7. Enter any not matched time value into Search field then hit enter");
            Step("8. **Expected** No row is displayed");
            Step("9. Repeat steps from #5 to #8 on other columns. Verify rows are displayed if matched and no row is displayed if unmatched");
            var tblGrid = deviceHistoryPage.DeviceHistoryPanel.BuildDataTableFromGrid();
            var columnList = tblGrid.Columns.Cast<DataColumn>().Select(c => c.ColumnName).ToList();
            foreach (var column in columnList) // repeat search on columns
            {
                // Matched search
                if (tblGrid.Rows.Count > 0)
                {
                    var searchText = tblGrid.Rows[0][column].ToString();
                    if (column == "Time")
                    {
                        searchText = searchText.SplitAndGetAt(new char[] { ' ' }, 0);
                    }
                    else
                    {
                        if (DateTime.Now.Day % 2 == 0)
                        {
                            searchText = searchText.Substring(new Random().Next(searchText.Length));
                        }
                    }
                    deviceHistoryPage.DeviceHistoryPanel.EnterSearchToolbarInput(searchText);
                    deviceHistoryPage.DeviceHistoryPanel.ClickSearchToolbarButton();
                    Wait.ForMilliseconds(200);// Sleeping for search result
                    var tblSearchResult = deviceHistoryPage.DeviceHistoryPanel.BuildDataTableFromGrid();
                    VerifyEqual("Verify search result found", true, tblSearchResult.Rows.Count > 0);
                }

                //Unmatched search
                deviceHistoryPage.DeviceHistoryPanel.EnterSearchToolbarInput(DateTime.Now.ToBinary().ToString());
                deviceHistoryPage.DeviceHistoryPanel.ClickSearchToolbarButton();
                Wait.ForMilliseconds(200);// Sleeping for search result
                VerifyEqual("Verify search result NOT found", true, deviceHistoryPage.DeviceHistoryPanel.BuildDataTableFromGrid().Rows.Count == 0);
            }
        }

        [Test, DynamicRetry]
        [Description("DE_14 Device history panel - Advanced filter in grid")]
        public void DE_14()
        {
            var testData = GetTestDataOfTestDE_14();
            var geozone = testData["Geozone"];
            
            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);

            Step("1. Go to Device History app");
            Step("2. Expected Device History page is routed and loaded successfully");
            desktopPage.InstallAppsIfNotExist(App.DeviceHistory);
            var deviceHistoryPage = desktopPage.GoToApp(App.DeviceHistory) as DeviceHistoryPage;

            Step("3. Select a device from geozone tree");
            deviceHistoryPage.GeozoneTreeMainPanel.SelectNode(geozone);
            var deviceList = deviceHistoryPage.GeozoneTreeMainPanel.GetChildDeviceNamesOfSelectedNode();
            var pickedDevice = deviceList.Where(p => p.Contains("Telematics")).PickRandom();
            deviceHistoryPage.GeozoneTreeMainPanel.SelectNode(pickedDevice);

            Step("4. **Expected** Device History panel appears");
            deviceHistoryPage.WaitForDeviceHistoryPanelDisplayed();
            VerifyEqual("Device history panel appears", true, deviceHistoryPage.IsDeviceHistoryPanelDisplayed());

            Step("5. Click Search icon");
            Step("6. **Expected** Advanced filter panel appears with fields Time, User, Name, Value");
            deviceHistoryPage.DeviceHistoryPanel.ClickSearchToolbarButton();
            deviceHistoryPage.DeviceHistoryPanel.WaitForAdvancedSearchPanelDisplayed();
            var expectedSearchFieldList = new List<string> { "Time", "User", "Name", "Value" };
            var actualSearchFieldList = deviceHistoryPage.DeviceHistoryPanel.GetListOfAdvancedSearchFields();
            VerifyEqual("Verify advanced search field list", expectedSearchFieldList, actualSearchFieldList);
            // Click to hide the advanced search popup
            deviceHistoryPage.DeviceHistoryPanel.ClickSearchToolbarButton();

            Step("7. Try to search with matched/unmatched condition in each single field and simple operator");
            Step(" - Time: '=' operator");
            Step(" - User: 'contains' operator");
            Step(" - Name: 'contains' operator");
            Step(" - Value: 'contains' operator");
            Step("8. **Expected** Rows are displayed if matched and no row is displayed if unmatched");
            var tblGrid = deviceHistoryPage.DeviceHistoryPanel.BuildDataTableFromGrid();
            if (tblGrid.Rows.Count > 0)
            {
                var userList = tblGrid.AsEnumerable().Select(r => r.Field<string>("User").Trim()).ToList();
                var nameList = tblGrid.AsEnumerable().Select(r => r.Field<string>("Name").Trim()).ToList();
                var valueList = tblGrid.AsEnumerable().Select(r => r.Field<string>("Value").Trim()).ToList();
                var searchedUserText = userList.Where(p => !string.IsNullOrEmpty(p)).PickRandom();
                var searchedNameText = nameList.Where(p => !string.IsNullOrEmpty(p)).PickRandom();
                var searchedValueText = valueList.Where(p => !string.IsNullOrEmpty(p)).PickRandom();
                var index = SLVHelper.GenerateInteger(tblGrid.Rows.Count);
                var pairNameText = nameList[index];
                var pairValueText = valueList[index];

                /*
                 User
                 */
                // User - Full match
                deviceHistoryPage.DeviceHistoryPanel.ClickSearchToolbarButton();
                deviceHistoryPage.DeviceHistoryPanel.WaitForAdvancedSearchPanelDisplayed();
                deviceHistoryPage.DeviceHistoryPanel.EnterSearchCriteriaForInputField("User", searchedUserText);
                deviceHistoryPage.DeviceHistoryPanel.ClickAdvancedSearchSearchButton();

                tblGrid = deviceHistoryPage.DeviceHistoryPanel.BuildDataTableFromGrid();
                VerifyEqual("8.1 Verify found result", true, tblGrid.Rows.Count > 0);

                // User - partial match
                deviceHistoryPage.DeviceHistoryPanel.ClickSearchToolbarButton();
                deviceHistoryPage.DeviceHistoryPanel.WaitForAdvancedSearchPanelDisplayed();
                deviceHistoryPage.DeviceHistoryPanel.EnterSearchCriteriaForInputField("User", searchedUserText.Substring(0, searchedUserText.Length / 2));
                deviceHistoryPage.DeviceHistoryPanel.ClickAdvancedSearchSearchButton();

                tblGrid = deviceHistoryPage.DeviceHistoryPanel.BuildDataTableFromGrid();
                VerifyEqual("8.2 Verify found result", true, tblGrid.Rows.Count > 0);

                // User - unmatch
                deviceHistoryPage.DeviceHistoryPanel.ClickSearchToolbarButton();
                deviceHistoryPage.DeviceHistoryPanel.WaitForAdvancedSearchPanelDisplayed();
                deviceHistoryPage.DeviceHistoryPanel.EnterSearchCriteriaForInputField("User", DateTime.Now.ToBinary().ToString());
                deviceHistoryPage.DeviceHistoryPanel.ClickAdvancedSearchSearchButton();

                tblGrid = deviceHistoryPage.DeviceHistoryPanel.BuildDataTableFromGrid();
                VerifyEqual("8.3 Verify found result", false, tblGrid.Rows.Count > 0);

                /*
                 Name
                 */
                // Name - Full match
                deviceHistoryPage.DeviceHistoryPanel.ClickSearchToolbarButton();
                deviceHistoryPage.DeviceHistoryPanel.WaitForAdvancedSearchPanelDisplayed();
                deviceHistoryPage.DeviceHistoryPanel.ClearInputFieldsInAdvancedSearchPopup();
                deviceHistoryPage.DeviceHistoryPanel.EnterSearchCriteriaForInputField("Name", searchedNameText);
                deviceHistoryPage.DeviceHistoryPanel.ClickAdvancedSearchSearchButton();

                tblGrid = deviceHistoryPage.DeviceHistoryPanel.BuildDataTableFromGrid();
                VerifyEqual("8.4 Verify found result", true, tblGrid.Rows.Count > 0);

                // Name - partial match
                deviceHistoryPage.DeviceHistoryPanel.ClickSearchToolbarButton();
                deviceHistoryPage.DeviceHistoryPanel.WaitForAdvancedSearchPanelDisplayed();
                deviceHistoryPage.DeviceHistoryPanel.EnterSearchCriteriaForInputField("Name", searchedNameText.Substring(0, searchedNameText.Length / 2));
                deviceHistoryPage.DeviceHistoryPanel.ClickAdvancedSearchSearchButton();

                tblGrid = deviceHistoryPage.DeviceHistoryPanel.BuildDataTableFromGrid();
                VerifyEqual("8.5 Verify found result", true, tblGrid.Rows.Count > 0);

                // Name - unmatch
                deviceHistoryPage.DeviceHistoryPanel.ClickSearchToolbarButton();
                deviceHistoryPage.DeviceHistoryPanel.WaitForAdvancedSearchPanelDisplayed();
                deviceHistoryPage.DeviceHistoryPanel.EnterSearchCriteriaForInputField("Name", DateTime.Now.ToBinary().ToString());
                deviceHistoryPage.DeviceHistoryPanel.ClickAdvancedSearchSearchButton();

                tblGrid = deviceHistoryPage.DeviceHistoryPanel.BuildDataTableFromGrid();
                VerifyEqual("8.6 Verify found result", false, tblGrid.Rows.Count > 0);

                /*
                 Value
                 */
                // Value - Full match
                deviceHistoryPage.DeviceHistoryPanel.ClickSearchToolbarButton();
                deviceHistoryPage.DeviceHistoryPanel.WaitForAdvancedSearchPanelDisplayed();
                deviceHistoryPage.DeviceHistoryPanel.ClearInputFieldsInAdvancedSearchPopup();
                deviceHistoryPage.DeviceHistoryPanel.EnterSearchCriteriaForInputField("Value", searchedValueText);
                deviceHistoryPage.DeviceHistoryPanel.ClickAdvancedSearchSearchButton();

                tblGrid = deviceHistoryPage.DeviceHistoryPanel.BuildDataTableFromGrid();
                VerifyEqual("8.7 Verify found result", true, tblGrid.Rows.Count > 0);

                // Value - partial match
                deviceHistoryPage.DeviceHistoryPanel.ClickSearchToolbarButton();
                deviceHistoryPage.DeviceHistoryPanel.WaitForAdvancedSearchPanelDisplayed();
                deviceHistoryPage.DeviceHistoryPanel.EnterSearchCriteriaForInputField("Value", searchedValueText.Substring(0, searchedValueText.Length / 2));
                deviceHistoryPage.DeviceHistoryPanel.ClickAdvancedSearchSearchButton();

                tblGrid = deviceHistoryPage.DeviceHistoryPanel.BuildDataTableFromGrid();
                VerifyEqual("8.8 Verify found result", true, tblGrid.Rows.Count > 0);

                // Value - unmatch
                deviceHistoryPage.DeviceHistoryPanel.ClickSearchToolbarButton();
                deviceHistoryPage.DeviceHistoryPanel.WaitForAdvancedSearchPanelDisplayed();
                deviceHistoryPage.DeviceHistoryPanel.EnterSearchCriteriaForInputField("Value", DateTime.Now.ToBinary().ToString());
                deviceHistoryPage.DeviceHistoryPanel.ClickAdvancedSearchSearchButton();

                tblGrid = deviceHistoryPage.DeviceHistoryPanel.BuildDataTableFromGrid();
                VerifyEqual("8.9 Verify found result", false, tblGrid.Rows.Count > 0);

                /*
                 Name & Value
                 */
                // Full match
                deviceHistoryPage.DeviceHistoryPanel.ClickSearchToolbarButton();
                deviceHistoryPage.DeviceHistoryPanel.WaitForAdvancedSearchPanelDisplayed();
                deviceHistoryPage.DeviceHistoryPanel.EnterSearchCriteriaForInputField("Name", pairNameText);
                deviceHistoryPage.DeviceHistoryPanel.EnterSearchCriteriaForInputField("Value", pairValueText);
                deviceHistoryPage.DeviceHistoryPanel.ClickAdvancedSearchSearchButton();

                tblGrid = deviceHistoryPage.DeviceHistoryPanel.BuildDataTableFromGrid();
                VerifyEqual("8.10 Verify found result", true, tblGrid.Rows.Count > 0);

                // Partial match
                deviceHistoryPage.DeviceHistoryPanel.ClickSearchToolbarButton();
                deviceHistoryPage.DeviceHistoryPanel.WaitForAdvancedSearchPanelDisplayed();
                deviceHistoryPage.DeviceHistoryPanel.EnterSearchCriteriaForInputField("Name", pairNameText.Substring(0, pairNameText.Length / 2));
                deviceHistoryPage.DeviceHistoryPanel.EnterSearchCriteriaForInputField("Value", pairValueText.Substring(0, pairValueText.Length / 2));
                deviceHistoryPage.DeviceHistoryPanel.ClickAdvancedSearchSearchButton();

                tblGrid = deviceHistoryPage.DeviceHistoryPanel.BuildDataTableFromGrid();
                VerifyEqual("8.11 Verify found result", true, tblGrid.Rows.Count > 0);

                // Unmatch
                deviceHistoryPage.DeviceHistoryPanel.ClickSearchToolbarButton();
                deviceHistoryPage.DeviceHistoryPanel.WaitForAdvancedSearchPanelDisplayed();
                deviceHistoryPage.DeviceHistoryPanel.EnterSearchCriteriaForInputField("Name", DateTime.Now.ToBinary().ToString());
                deviceHistoryPage.DeviceHistoryPanel.EnterSearchCriteriaForInputField("Value", DateTime.Now.ToBinary().ToString());
                deviceHistoryPage.DeviceHistoryPanel.ClickAdvancedSearchSearchButton();

                tblGrid = deviceHistoryPage.DeviceHistoryPanel.BuildDataTableFromGrid();
                VerifyEqual("8.12 Verify found result", false, tblGrid.Rows.Count > 0);
            }
        }

        [Test, DynamicRetry]
        [Description("DE_15 Device history panel - Export")]
        [NonParallelizable]
        public void DE_15()
        {
            var testData = GetTestDataOfTestDE_10();
            var geozone = testData["Geozone"].ToString();
            var streetlights = testData["Streetlights"] as List<DeviceModel>;

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);

            Step("1. Go to Device History app");
            Step("2. Expected Device History page is routed and loaded successfully");
            desktopPage.InstallAppsIfNotExist(App.DeviceHistory);
            var deviceHistoryPage = desktopPage.GoToApp(App.DeviceHistory) as DeviceHistoryPage;

            Step("3. Select a device from geozone tree");
            deviceHistoryPage.GeozoneTreeMainPanel.SelectNode(geozone);
            var pickedDevice = streetlights.PickRandom().Name;
            deviceHistoryPage.GeozoneTreeMainPanel.SelectNode(pickedDevice);

            Step("4. **Expected** Device History panel appears");
            deviceHistoryPage.WaitForDeviceHistoryPanelDisplayed();
            VerifyEqual("4. Verify Device history panel appears", true, deviceHistoryPage.IsDeviceHistoryPanelDisplayed());

            Step("5. Click Export icon");
            deviceHistoryPage.DeviceHistoryPanel.WaitForLoaderSpinDisappeared();
            if (deviceHistoryPage.DeviceHistoryPanel.IsNoDataMessageDisplayed())
            {
                Warning(string.Format("No data displayed for current selected duration '{0}'", deviceHistoryPage.DeviceHistoryPanel.GetDurationValue()));
            }
            else
            {
                SLVHelper.DeleteAllFilesByPattern(_deviceHistoryExportedFilePattern);
                deviceHistoryPage.DeviceHistoryPanel.ClickExportToolbarButton();
                SLVHelper.SaveDownloads();

                Step(@"6. **Expected** A csv is downloaded
                      - Its name's pattern: '\d{10}-DeviceHistory-Export.csv'
                      - Its content reflects grid's content");
                var tblGrid = deviceHistoryPage.DeviceHistoryPanel.BuildDataTableFromGrid();
                var tblCSV = SLVHelper.BuildDataTableFromLastDownloadedCSV(_deviceHistoryExportedFilePattern);
                foreach (DataRow rowCSV in tblCSV.Rows)
                {
                    var time = rowCSV["Time"].ToString();
                    rowCSV["Time"] = DateTime.Parse(time.Substring(0, 24), CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal);
                }
                VerifyEqual("6. Verify downloaded csv reflects grid's content", tblGrid, tblCSV);
            }
        }

        [Test, DynamicRetry]
        [Description("DE_16 Device history panel - Show duplicated logs")]
        public void DE_16()
        {
            var testData = GetTestDataOfTestDE_16();
            var geozone = testData["Geozone"];

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);

            Step("1. Go to Device History app");
            Step("2. Expected Device History page is routed and loaded successfully");
            desktopPage.InstallAppsIfNotExist(App.DeviceHistory);
            var deviceHistoryPage = desktopPage.GoToApp(App.DeviceHistory) as DeviceHistoryPage;

            Step("3. Select a device from geozone tree");
            deviceHistoryPage.GeozoneTreeMainPanel.SelectNode(geozone);
            var deviceList = deviceHistoryPage.GeozoneTreeMainPanel.GetChildDeviceNamesOfSelectedNode();
            var pickedDevice = deviceList.PickRandom();
            deviceHistoryPage.GeozoneTreeMainPanel.SelectNode(pickedDevice);

            Step("4. **Expected** Device History panel appears");
            deviceHistoryPage.WaitForDeviceHistoryPanelDisplayed();
            VerifyEqual("Device history panel appears", true, deviceHistoryPage.IsDeviceHistoryPanelDisplayed());

            Step("5. Toggle on Show duplicated logs if it is being toggled off and note entries");
            if (deviceHistoryPage.DeviceHistoryPanel.IsDuplicatedLogsToggleOn())
            {
                deviceHistoryPage.DeviceHistoryPanel.ClickDuplicatedLogsToolbarButton();
                VerifyEqual("5. Verify Show duplicated logs has been toggle off", false, deviceHistoryPage.DeviceHistoryPanel.IsDuplicatedLogsToggleOn());
            }
            Wait.ForMilliseconds(500); // wait for data refreshed
            var tblDuplicates = deviceHistoryPage.DeviceHistoryPanel.BuildDataTableFromGrid();

            Step("6. Toggle off Show duplicated logs");
            Step("7. **Expected** All unduplicated entries are remained displayed. Only 1 latest duplicated entry is remained displayed");
            deviceHistoryPage.DeviceHistoryPanel.ClickDuplicatedLogsToolbarButton();
            VerifyEqual("7. Verify Show duplicated logs has been toggle on", true, deviceHistoryPage.DeviceHistoryPanel.IsDuplicatedLogsToggleOn());
            Wait.ForMilliseconds(500); // wait for data refreshed
            var tblNoDuplicates = deviceHistoryPage.DeviceHistoryPanel.BuildDataTableFromGrid();

            // copy tables for handling
            var tblSortedNoDuplicates = SortDataTableByTime(tblNoDuplicates, "Time", "ASC");
            var tblSortedDuplicates = SortDataTableByTime(tblDuplicates, "Time", "ASC");
            var listSortedNoDuplicates = tblSortedNoDuplicates.AsEnumerable().Select(r => string.Format("{0}|{1}", r.Field<string>("Name"), r.Field<string>("Value"))).ToList();
            var listSortedDuplicates = tblSortedDuplicates.AsEnumerable().Select(r => string.Format("{0}|{1}", r.Field<string>("Name"), r.Field<string>("Value"))).ToList();

            var listSortedDuplicatesRemoved = RemoveDuplicate(listSortedDuplicates).ToList();
            VerifyEqual("7. Verify only 1 latest entry is remained displayed in grid, other duplicated logs are removed", listSortedNoDuplicates, listSortedDuplicatesRemoved, false);

            Step("8. Toggle on Show duplicated logs");
            Step("9. **Expected** Verify all entries are shown as in step #6");
            deviceHistoryPage.DeviceHistoryPanel.ClickDuplicatedLogsToolbarButton();
            VerifyEqual("9. Verify Show duplicated logs has been toggle off", false, deviceHistoryPage.DeviceHistoryPanel.IsDuplicatedLogsToggleOn());
            VerifyEqual("9. Verify entries before unchecked and after checked are the same", tblDuplicates, deviceHistoryPage.DeviceHistoryPanel.BuildDataTableFromGrid());
        }

        [Test, DynamicRetry]
        [Description("DE_17 Device history panel - Reflect changes from Equipment Inventory")]
        public void DE_17()
        {
            var geozone = SLVHelper.GenerateUniqueName("GZNDE17");
            var controller = SLVHelper.GenerateUniqueName("CTRL");
            var streetlight = SLVHelper.GenerateUniqueName("STL");
            var listAttributes = new List<string>() { "Unique address", "Dimming group", "Configuration status", "Install status", "Config status msg" };
            var listInstallStatus = new List<string>() { "To be verified", "Verified", "New", "Does not exist", "To be installed", "Installed", "Removed" };
            
            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - Create new streetlight for testing");
            Step("**** Precondition ****\n");

            Step("-> Create data for testing");
            DeleteGeozones("GZNDE17*");
            CreateNewGeozone(geozone);
            CreateNewController(controller, geozone);
            CreateNewDevice(DeviceType.Streetlight, streetlight, controller, geozone);

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);            
            desktopPage.InstallAppsIfNotExist(App.DeviceHistory);
            
            Step("1. Prepare the commands to update the values for 5 following attributes of the Streetlight with eventTime in the future:");
            Step(" o Dimming group - valueName=DimmingGroupName");
            Step(" o Install status - valueName=installStatus");
            Step(" o Unique address - valueName=MacAddress");
            Step(" o Configuration status - valueName=ConfigStatus");
            Step(" o Config status msg - valueName=ConfigStatusMessage");
            var eventDateTime = DateTime.Now.AddDays(2).Date;
            var notedDimmingGroupName = SLVHelper.GenerateString();
            var notedInstallStatus = listInstallStatus.PickRandom();
            var notedMacAddress = SLVHelper.GenerateMACAddress();
            var notedConfigStatus = SLVHelper.GenerateString();
            var notedConfigStatusMessage = SLVHelper.GenerateString(9);
            var isSentDimmingGroupNameSuccess = SetValueToDevice(controller, streetlight, "DimmingGroupName", notedDimmingGroupName, eventDateTime);
            var isSentInstallStatusSuccess = SetValueToDevice(controller, streetlight, "installStatus", notedInstallStatus, eventDateTime);
            var isSentMacAddressSuccess = SetValueToDevice(controller, streetlight, "MacAddress", notedMacAddress, eventDateTime);
            var isSentConfigStatusSuccess = SetValueToDevice(controller, streetlight, "ConfigStatus ", notedConfigStatus, eventDateTime);
            var isSentConfigStatusMessageSuccess = SetValueToDevice(controller, streetlight, "ConfigStatusMessage", notedConfigStatusMessage, eventDateTime);
            
            Step("2. Verify All commands are run successfully");
            VerifyEqual("2. Verify sent command for Dimming group successfully", true, isSentDimmingGroupNameSuccess);
            VerifyEqual("2. Verify sent command for Install status successfully", true, isSentInstallStatusSuccess);
            VerifyEqual("2. Verify sent command for Unique address successfully", true, isSentMacAddressSuccess);
            VerifyEqual("2. Verify sent command for Configuration status successfully", true, isSentConfigStatusSuccess);
            VerifyEqual("2. Verify sent command for Config status msg successfully", true, isSentConfigStatusMessageSuccess);

            Step("3. Go to Device History");
            Step("4. Verify Device History app is routed and loaded successfully");
            var deviceHistoryPage = desktopPage.GoToApp(App.DeviceHistory) as DeviceHistoryPage;

            Step("5. Select the new streetlight");
            deviceHistoryPage.GeozoneTreeMainPanel.SelectNode(geozone + @"\" + streetlight);
            deviceHistoryPage.WaitForDeviceHistoryPanelDisplayed();          

            Step("6. Press Attributes filter button on Device History Detail panel of the new streetlight");
            deviceHistoryPage.DeviceHistoryPanel.ClickFilterToolbarButton();
            deviceHistoryPage.DeviceHistoryPanel.WaitForAttributesFilterPanelDisplayed();

            Step("7. Select 5 attributes: Unique address, Dimming group, Configuration status, Install status, Config status msg");
            deviceHistoryPage.DeviceHistoryPanel.CheckFilterAttributes(listAttributes.ToArray());

            Step("8. Press Attributes filter button again and press Refresh button");
            deviceHistoryPage.DeviceHistoryPanel.ClickFilterToolbarButton();
            deviceHistoryPage.DeviceHistoryPanel.WaitForAttributesFilterPanelDisappeared();
            deviceHistoryPage.DeviceHistoryPanel.ClickRefreshToolbarButton();
            deviceHistoryPage.DeviceHistoryPanel.WaitForLoaderSpinDisappeared();

            Step("16. Expected 2 new records of Latitude and Longitude are displayed with values equal to their newly inputted values.");
            var dt = deviceHistoryPage.DeviceHistoryPanel.BuildDataTableFromGrid();
            var uniqueAddressRow = dt.Select("Name = 'Unique address'").FirstOrDefault();
            var dimmingGroupRow = dt.Select("Name = 'Dimming group'").FirstOrDefault();
            var configurationStatusRow = dt.Select("Name = 'Configuration status'").FirstOrDefault();
            var installStatusRow = dt.Select("Name = 'Install status'").FirstOrDefault();
            var configStatusMsgRow = dt.Select("Name = 'Config status msg'").FirstOrDefault();

            Step("9. Get the first 5 records of the grid and verify the following information");
            Step("10. Verify the values of 5 attributes: Unique address, Dimming group, Configuration status, Install status, Config status msg are equal to values set up in step 1.");
            if (uniqueAddressRow != null)
            {
                var actualUniqueAddress = uniqueAddressRow["Value"].ToString();
                VerifyEqual("10. Verify Unique address is " + notedMacAddress, notedMacAddress, actualUniqueAddress);
            }
            else
            {
                Warning("10. There is no row with Unique Address attribute");
            }

            if (dimmingGroupRow != null)
            {
                var actualDimmingGroup = dimmingGroupRow["Value"].ToString();
                VerifyEqual("10. Verify Unique address is " + notedDimmingGroupName, notedDimmingGroupName, actualDimmingGroup);
            }
            else
            {
                Warning("10. There is no row with Dimming Group attribute");
            }

            if (configurationStatusRow != null)
            {
                var actualConfigStatus = configurationStatusRow["Value"].ToString();
                VerifyEqual("10. Verify Configuration status is " + notedConfigStatus, notedConfigStatus, actualConfigStatus);
            }
            else
            {
                Warning("10. There is no row with Configuration status attribute");
            }

            if (installStatusRow != null)
            {
                var actualInstallStatus = installStatusRow["Value"].ToString();
                VerifyEqual("10. Verify Install status, is " + notedInstallStatus, notedInstallStatus, actualInstallStatus);
            }
            else
            {
                Warning("10. There is no row with Install status attribute");
            }

            if (configStatusMsgRow != null)
            {
                var actualConfigStatusMessage = configStatusMsgRow["Value"].ToString();
                VerifyEqual("10. Verify Config status msg is " + notedConfigStatusMessage, notedConfigStatusMessage, actualConfigStatusMessage);
            }
            else
            {
                Warning("10. There is no row with Config status msg attribute");
            }

            try
            {
                Step("11. Delete the test data after testcase is done.");
                DeleteGeozone(geozone);
            }
            catch { }
        }

        [Test, DynamicRetry]        
        [Description("DE_18 SC-1442 Wrong timezone adjustment applied to lat, long, 2d shift in Device History")]
        [NonParallelizable]
        public void DE_18()
        {
            var testData = GetTestDataOfTestDE_18();
            var timeZones = testData["OlsonTimeZones"] as List<string>;
            var timeZoneId = timeZones.PickRandom();
             var typeOfEquipment = "ABEL-Vigilon A[Dimmable ballast]";
            var csvFilePath = Settings.GetFullPath(Settings.CSV_FILE_PATH + "DE18.csv");
            var geozone = SLVHelper.GenerateUniqueName("GZNDE18");
            var controller = SLVHelper.GenerateUniqueName("CTRL");
            var streetlight1 = SLVHelper.GenerateUniqueName("STL01");
            var streetlight2 = SLVHelper.GenerateUniqueName("STL02");
            var fullGeozonePath = Settings.RootGeozoneName + @"/" + geozone;
            var importedStreetlightLatitude  = SLVHelper.GenerateCoordinate("9.27195", "9.27280");
            var importedStreetlightLongitude = SLVHelper.GenerateCoordinate("99.70760", "99.70880");
            var newStreelightLatitude = SLVHelper.GenerateCoordinate("9.27195", "9.27280");
            var newStreetlightLongitude = SLVHelper.GenerateCoordinate("99.70760", "99.70880");

            Step("-> Create data for testing");
            DeleteGeozones("GZNDE18*");
            CreateNewGeozone(geozone, latMin: "9.27195", latMax: "9.27356", lngMin: "99.70525", lngMax: "99.71040");
            CreateNewController(controller, geozone);
            SetValueToController(controller, "TimeZoneId", timeZoneId, Settings.GetServerTime());
            CreateNewDevice(DeviceType.Streetlight, streetlight1, controller, geozone, typeOfEquipment, newStreelightLatitude, newStreetlightLongitude);
            var notedTimezoneTime = Settings.GetCurrentControlerDateTime(controller);
            CreateCsv(DeviceType.Streetlight, csvFilePath, fullGeozonePath, controller, streetlight2, typeOfEquipment, importedStreetlightLatitude, importedStreetlightLongitude);
          
            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.DeviceHistory, App.EquipmentInventory);     

            Step("1. Go to Device History and select the testing geozone and the testing streetlight");
            var deviceHistoryPage = desktopPage.GoToApp(App.DeviceHistory) as DeviceHistoryPage;
            deviceHistoryPage.GeozoneTreeMainPanel.SelectNode(geozone + @"\" + streetlight1);
            deviceHistoryPage.WaitForDeviceHistoryPanelDisplayed();

            Step("2. Press 'Attribute filter' button and select Longitude, Latitude, 2D shift");
            var expectedAttributes = new List<string> { "Longitude", "Latitude", "2D shift" };
            deviceHistoryPage.DeviceHistoryPanel.ClickFilterToolbarButton();
            deviceHistoryPage.DeviceHistoryPanel.WaitForAttributesFilterPanelDisplayed();
            deviceHistoryPage.DeviceHistoryPanel.CheckFilterAttributes(expectedAttributes.ToArray());
            deviceHistoryPage.DeviceHistoryPanel.ClickFilterToolbarButton();
            deviceHistoryPage.DeviceHistoryPanel.WaitForAttributesFilterPanelDisappeared();

            Step("3. Verify The datetime of these 3 attributes is following the timezone of the controller");
            var listOfTime = deviceHistoryPage.DeviceHistoryPanel.GetListOfColumnData("Time");
            foreach (var time in listOfTime)
            {
                var entryTimeAsDateTime = Convert.ToDateTime(time);                
                var span = entryTimeAsDateTime.Subtract(notedTimezoneTime);
                VerifyTrue(string.Format("[{0}] 3. Verify The datetime of these 3 attributes is following the timezone of the controller", timeZoneId)
                    , Math.Abs(span.TotalMinutes) <= 5, notedTimezoneTime, entryTimeAsDateTime);
            }

            Step("4. Go to Equipment Inventory > the testing geozone and moving the testing streetlight for a long distance (Over 200m), then back to the Device History application, click Refresh button");
            var equipmentInventoryPage = deviceHistoryPage.AppBar.SwitchTo(App.EquipmentInventory) as EquipmentInventoryPage;
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(geozone);
            equipmentInventoryPage.Map.ClickHoldDeviceAndMoveTo(newStreetlightLongitude, newStreelightLatitude, SLVHelper.GenerateInteger(200, 250), SLVHelper.GenerateInteger(50, 100));
            equipmentInventoryPage.WaitForHeaderMessageDisplayed();
            var notedTimezoneTime1 = Settings.GetCurrentControlerDateTime(controller);
            equipmentInventoryPage.WaitForHeaderMessageDisappeared();
            deviceHistoryPage = equipmentInventoryPage.AppBar.SwitchTo(App.DeviceHistory) as DeviceHistoryPage;
            deviceHistoryPage.GeozoneTreeMainPanel.SelectNode(streetlight1);
            deviceHistoryPage.WaitForDeviceHistoryPanelDisplayed();
            deviceHistoryPage.DeviceHistoryPanel.ClickRefreshToolbarButton();
            deviceHistoryPage.DeviceHistoryPanel.WaitForLoaderSpinDisappeared();

            Step("5. Verify");
            Step(" o There are 3 new records of Longitude, Latitude, 2D shift in the grid");
            Step(" o The datetime of these 3 attributes is following the timezone of the controller (approximate the current datetime of testing timezone)");
            var listOfTime1 = deviceHistoryPage.DeviceHistoryPanel.GetListOfColumnData("Time");
            listOfTime1.RemoveAll(p => listOfTime.Contains(p));
            VerifyEqual("5. Verify There are 3 new records of Longitude, Latitude, 2D Shift in the grid", 3, listOfTime1.Count);
            foreach (var time in listOfTime1)
            {
                var entryTimeAsDateTime = Convert.ToDateTime(time);
                var span = entryTimeAsDateTime.Subtract(notedTimezoneTime1);
                VerifyTrue(string.Format("[{0}] 5. Verify The datetime of these 3 attributes is following the timezone of the controller (approximate the current datetime of testing timezone [+/-5 minutes]", timeZoneId)
                    , Math.Abs(span.TotalMinutes) <= 5 , notedTimezoneTime1, entryTimeAsDateTime);
            }

            Step("6. Prepare a csv file to import a new streetlight of the testing controller with Longitude, Latitude and import it using the API");
            ImportFile(csvFilePath);
            var notedTimezoneTime2 = Settings.GetCurrentControlerDateTime(controller);

            Step("7. Go to Device History and select the testing geozone and the newly imported streetlight");
            desktopPage = Browser.RefreshLoggedInCMS();
            deviceHistoryPage = desktopPage.GoToApp(App.DeviceHistory) as DeviceHistoryPage;
            deviceHistoryPage.GeozoneTreeMainPanel.SelectNode(geozone + @"\" + streetlight2);
            deviceHistoryPage.WaitForDeviceHistoryPanelDisplayed();

            Step("8. Verify The datetime of these 3 attributes is following the timezone of the controller");
            deviceHistoryPage.DeviceHistoryPanel.ClickFilterToolbarButton();
            deviceHistoryPage.DeviceHistoryPanel.WaitForAttributesFilterPanelDisplayed();
            deviceHistoryPage.DeviceHistoryPanel.CheckFilterAttributes(expectedAttributes.ToArray());
            deviceHistoryPage.DeviceHistoryPanel.ClickFilterToolbarButton();
            deviceHistoryPage.DeviceHistoryPanel.WaitForAttributesFilterPanelDisappeared();
            var listOfTime2 = deviceHistoryPage.DeviceHistoryPanel.GetListOfColumnData("Time");
            foreach (var time in listOfTime2)
            {
                var entryTimeAsDateTime = Convert.ToDateTime(time);
                var span = entryTimeAsDateTime.Subtract(notedTimezoneTime2);
                VerifyTrue(string.Format("[{0}] 8. Verify The datetime of these 3 attributes is following the timezone of the controller", timeZoneId)
                    , Math.Abs(span.TotalMinutes) <= 5, notedTimezoneTime2, entryTimeAsDateTime);
            }

            Step("9. Go to Equipment Inventory > the testing geozone and moving the newly imported streetlight for a long distance (Over 200m), then back to the Device History application, click Refresh button");
            equipmentInventoryPage = deviceHistoryPage.AppBar.SwitchTo(App.EquipmentInventory) as EquipmentInventoryPage;
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(geozone);
            equipmentInventoryPage.Map.ClickHoldDeviceAndMoveTo(importedStreetlightLongitude, importedStreetlightLatitude, SLVHelper.GenerateInteger(-250, -200), SLVHelper.GenerateInteger(50, 100));
            equipmentInventoryPage.WaitForHeaderMessageDisplayed();
            var notedTimezoneTime3 = Settings.GetCurrentControlerDateTime(controller);
            equipmentInventoryPage.WaitForHeaderMessageDisappeared();
            deviceHistoryPage = equipmentInventoryPage.AppBar.SwitchTo(App.DeviceHistory) as DeviceHistoryPage;
            deviceHistoryPage.GeozoneTreeMainPanel.SelectNode(streetlight2);
            deviceHistoryPage.WaitForDeviceHistoryPanelDisplayed();
            deviceHistoryPage.DeviceHistoryPanel.ClickRefreshToolbarButton();
            deviceHistoryPage.DeviceHistoryPanel.WaitForLoaderSpinDisappeared();

            Step("10. Verify");
            Step(" o There are 3 new records of Longitude, Latitude, 2D shift in the grid");
            Step(" o The datetime of these 3 attributes is following the timezone of the controller (approximate the current datetime of testing timezone)");
            var listOfTime3 = deviceHistoryPage.DeviceHistoryPanel.GetListOfColumnData("Time");
            listOfTime3.RemoveAll(p => listOfTime2.Contains(p));
            VerifyEqual("10. Verify There are 3 new records of Longitude, Latitude, 2D shift in the grid", 3, listOfTime3.Count);
            foreach (var time in listOfTime3)
            {
                var entryTimeAsDateTime = Convert.ToDateTime(time);
                var span = entryTimeAsDateTime.Subtract(notedTimezoneTime3);
                VerifyTrue(string.Format("[{0}] 10. Verify The datetime of these 3 attributes is following the timezone of the controller (approximate the current datetime of testing timezone [+/-5 minutes]", timeZoneId)
                    , Math.Abs(span.TotalMinutes) <= 5, notedTimezoneTime3, entryTimeAsDateTime);
            }
            
            try
            {
                DeleteGeozone(geozone);
            }
            catch { }
        }

        #endregion //Test Cases

        #region Private methods        

        #region Verify methods

        private static void VerifyTimeWithinDuration(string duration, string entryTime)
        {
            var entryTimeAsDateTime = Convert.ToDateTime(entryTime);
            var now = DateTime.Now;
            var days = now.Subtract(entryTimeAsDateTime).Days;
            var expectedDays = 0;
            if (duration == "Last two weeks")
            {
                expectedDays = 14;
            }
            else
            {
                if (duration == "Last month")
                {
                    expectedDays = 31;
                }
                else
                {
                    if (duration == "Last two months")
                    {
                        expectedDays = 62;
                    }
                    else
                    {
                        if (duration == "Last three months")
                        {
                            expectedDays = 93;
                        }
                        else
                        {
                            if (duration == "Last six months")
                            {
                                expectedDays = 186;
                            }
                        }
                    }
                }
            }

            VerifyEqual("[SC-573] Verify each time entry is within selected duration", true, expectedDays >= days);
        }

        #endregion //Verify methods

        #region Input XML data

        /// <summary>
        /// Get common data test
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, object> GetCommonTestData()
        {
            var realtimeGeozone = Settings.CommonTestData[0];
            var testData = new Dictionary<string, object>();
            testData.Add("Geozone", realtimeGeozone.Path);
            var streetlights = realtimeGeozone.Devices.Where(p => p.Type == DeviceType.Streetlight && p.Status == DeviceStatus.Working && string.IsNullOrEmpty(p.Cluster)).ToList();
            testData.Add("Streetlights", streetlights);

            return testData;
        }

        /// <summary>
        /// Get list of columns in show hide column menu
        /// </summary>
        /// <returns></returns>
        private List<string> GetAvailableGridColumns()
        {
            var testCaseName = "Common";
            var xmlUtility = new XmlUtility(Settings.DEVICE_HISTORY_TEST_DATA_FILE_PATH);
            var columnListAstring = xmlUtility.GetChildNodesText(string.Format(Settings.DEVICE_HISTORY_XPATH_PREFIX, testCaseName, "GridColumns"));

            return columnListAstring[0].SplitEx(new string[] { "," });
        }
        
        /// <summary>
        /// Get test data of DE_08
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfTestDE_08()
        {
            var testCaseName = "DE_08";
            var xmlUtility = new XmlUtility(Settings.DEVICE_HISTORY_TEST_DATA_FILE_PATH);
            var testData = new Dictionary<string, string>();

            testData.Add("Device", xmlUtility.GetSingleNodeText(string.Format(Settings.DEVICE_HISTORY_XPATH_PREFIX, testCaseName, "Device")));

            return testData;
        }

        /// <summary>
        /// Get test data of DE_09
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfTestDE_09()
        {
            var testCaseName = "DE_09";
            var xmlUtility = new XmlUtility(Settings.DEVICE_HISTORY_TEST_DATA_FILE_PATH);
            var testData = new Dictionary<string, string>();

            testData.Add("Geozone", xmlUtility.GetSingleNodeText(string.Format(Settings.DEVICE_HISTORY_XPATH_PREFIX, testCaseName, "Geozone")));

            return testData;
        }

        /// <summary>
        /// Get test data of DE_10
        /// <returns></returns>
        private Dictionary<string, object> GetTestDataOfTestDE_10()
        {
            return GetCommonTestData();
        }

        /// <summary>
        /// Get test data of DE_11
        /// <returns></returns>
        private Dictionary<string, object> GetTestDataOfTestDE_11()
        {
            return GetCommonTestData();
        }

        /// <summary>
        /// Get test data of DE_13
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfTestDE_13()
        {
            var testCaseName = "DE_13";
            var xmlUtility = new XmlUtility(Settings.DEVICE_HISTORY_TEST_DATA_FILE_PATH);
            var testData = new Dictionary<string, string>();

            testData.Add("Geozone", xmlUtility.GetSingleNodeText(string.Format(Settings.DEVICE_HISTORY_XPATH_PREFIX, testCaseName, "Geozone")));

            return testData;
        }

        /// <summary>
        /// Get test data of DE_14
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfTestDE_14()
        {
            var testCaseName = "DE_14";
            var xmlUtility = new XmlUtility(Settings.DEVICE_HISTORY_TEST_DATA_FILE_PATH);
            var testData = new Dictionary<string, string>();

            testData.Add("Geozone", xmlUtility.GetSingleNodeText(string.Format(Settings.DEVICE_HISTORY_XPATH_PREFIX, testCaseName, "Geozone")));

            return testData;
        }

        /// <summary>
        /// Get test data of DE_15
        /// <returns></returns>
        private Dictionary<string, object> GetTestDataOfTestDE_15()
        {
            return GetCommonTestData();
        }

        /// <summary>
        /// Get test data of DE_16
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfTestDE_16()
        {
            var testCaseName = "DE_16";
            var xmlUtility = new XmlUtility(Settings.DEVICE_HISTORY_TEST_DATA_FILE_PATH);
            var testData = new Dictionary<string, string>();

            testData.Add("Geozone", xmlUtility.GetSingleNodeText(string.Format(Settings.DEVICE_HISTORY_XPATH_PREFIX, testCaseName, "Geozone")));

            return testData;
        }

        /// <summary>
        /// Get test data of DE_18
        /// <returns></returns>
        private Dictionary<string, object> GetTestDataOfTestDE_18()
        {
            var testCaseName = "DE_18";
            var xmlUtility = new XmlUtility(Settings.DEVICE_HISTORY_TEST_DATA_FILE_PATH);
            var testData = new Dictionary<string, object>();            
            testData.Add("OlsonTimeZones", xmlUtility.GetChildNodesText(string.Format(Settings.DEVICE_HISTORY_XPATH_PREFIX, testCaseName, "OlsonTimeZones")));

            return testData;
        }

        #endregion //Input XML data

        /// <summary>
        /// Convert time string column to date time column then return sorted table according to specified order
        /// </summary>
        /// <param name="table"></param>
        /// <param name="timeColumnName"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        private DataTable SortDataTableByTime(DataTable table, string timeColumnName, string order)
        {
            var newCol = timeColumnName + "Col";
            var tbl = table.Copy();
            tbl.Columns.Add(timeColumnName + "Col", typeof(DateTime));

            foreach (DataRow row in tbl.Rows)
            {
                var time = row[timeColumnName].ToString();
                Console.WriteLine(time);
                row[newCol] = DateTime.Parse(time, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal);
            }

            var vw = tbl.DefaultView;
            vw.Sort = newCol + " " + order;

            return vw.ToTable();
        }

        /// <summary>
        /// Remove next duplicated items
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        /// <remarks>
        /// Two entries are considered duplicates when all of the following conditions are met
        ///- their name is identical,
        ///- their value is identical,
        ///- there is no other entry between them with the same name but a different value.
        ///For instance:
        ///a/ 2017-03-10 10:00:00 / Configuration status / SUCCESS
        ///b/ 2017-03-10 09:00:00 / Configuration status / SUCCESS
        ///c/ 2017-03-10 08:00:00 / Configuration status / Commissioning
        ///d/ 2017-03-10 07:00:00 / Configuration status / SUCCESS
        ///a and b are duplicates, but b and d are not duplicates!
        /// </remarks>
        private static IEnumerable<string> RemoveNextDuplicate(IList<string> collection)
        {
            if (collection.Count > 0) yield return collection[0];
            var temp = collection[0];
            for (int i = 1; i < collection.Count; i++)
            {
                if (temp == collection[i]) continue;
                yield return collection[i];
                temp = collection[i];
            }
        }

        /// <summary>
        /// Remove duplicated items
        /// </summary>
        private static IList<string> RemoveDuplicate(IList<string> telematics)
        {
            var list = new List<string>();

            if (telematics.Count > 0)
            {
                list.Add(telematics[0]);
            }

            for (int i = 1; i < telematics.Count; i++)
            {
                var telematic = list.LastOrDefault(x => x.Split('|')[0] == telematics[i].Split('|')[0]);
                if (telematic == null)
                {
                    list.Add(telematics[i]);
                }
                else
                {
                    if (telematic.Split('|')[1] != telematics[i].Split('|')[1])
                    {
                        list.Add(telematics[i]);
                    }
                }
            }

            return list;
        }

        #endregion //Private methods
    }
}