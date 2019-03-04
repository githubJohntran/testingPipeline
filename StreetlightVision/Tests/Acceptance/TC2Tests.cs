using NUnit.Framework;
using StreetlightVision.Extensions;
using StreetlightVision.Models;
using StreetlightVision.Pages;
using StreetlightVision.Utilities;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;

namespace StreetlightVision.Tests.Acceptance
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    public class TC2Tests : TestBase
    {
        #region Variables

        #endregion //Variables

        #region Contructors

        #endregion //Contructors

        #region Test Cases        

        [Test, DynamicRetry]
        [Description("TS 2.3.2 Equipment Inventory - Edit geozone")]
        public void TS2_03_02()
        {
            Assert.Pass("Covered by EI_02, EI_02_01");
        }

        [Test, DynamicRetry]
        [Description("TS 2.3.3 Equipment Inventory - Remove geozone")]
        public void TS2_03_03()
        {
            Assert.Pass("Covered by EI_03");
        }

        [Test, DynamicRetry]
        [Description("TS 2.3.4 Equipment Inventory - Move geozone to larger one")]
        public void TS2_03_04()
        {
            var testData = GetTestDataOfTestTS2_03_04();
            var parentGeozonePath = testData["ParentGeozone"];
            var destGeozonePath = testData["DestGeozone"];
            var parentGeozone = parentGeozonePath.GetChildName() ;
            var destGeozone = destGeozonePath.GetChildName();
            var dragdropGeozone = SLVHelper.GenerateUniqueName("GZNTS20304");
            var controller = SLVHelper.GenerateUniqueName("CTR");
            var streetlight = SLVHelper.GenerateUniqueName("STL");
            var switchDevice = SLVHelper.GenerateUniqueName("SW");
            var subGeozone = SLVHelper.GenerateUniqueName("SubGZN");            

            Step("-> Create data for testing");
            DeleteGeozones("GZNTS20304*");
            CreateNewGeozone(dragdropGeozone, parentGeozone);
            CreateNewGeozone(subGeozone, dragdropGeozone);
            CreateNewController(controller, dragdropGeozone);
            CreateNewDevice(DeviceType.Streetlight, streetlight, controller, dragdropGeozone);
            CreateNewDevice(DeviceType.Switch, switchDevice, controller, dragdropGeozone);

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.EquipmentInventory);
           
            Step("1. Go to Equipment Inventory app");
            Step("2. Expected Equipment Inventory page is routed and loaded successfully");
            var equipmentInventoryPage = desktopPage.GoToApp(App.EquipmentInventory) as EquipmentInventoryPage;

            Step("3. Select a geozone in the geozone tree with sub geozones");
            // Select source geozone first to get its child nodes
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(parentGeozonePath + @"\" + dragdropGeozone);
            var expectedListOfSourceGeozoneNodesAsText = equipmentInventoryPage.GeozoneTreeMainPanel.GetChildNodeNamesOfSelectedNode();
            // Now select the dest geozone
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(destGeozonePath);

            Step("4. Drag and drop it into its new parent geozone.");
            equipmentInventoryPage.GeozoneTreeMainPanel.MoveNodeToGeozone(dragdropGeozone, destGeozone);

            Step("5. Expected There is a popup asking \"Would you like to move {{geozone name}} geozone and all sub geoZones and equipments ?\"");
            VerifyEqual("5. Verify confirmation dialog appears", true, equipmentInventoryPage.Dialog.IsDialogVisible());
            VerifyEqual("5. Verify confirmation message", string.Format("Would you like to move {0} geozone and all sub geoZones and equipments ?", dragdropGeozone), equipmentInventoryPage.Dialog.GetMessageText());

            Step("6. Click \"No\"");
            equipmentInventoryPage.Dialog.ClickNoButton();
            equipmentInventoryPage.WaitForPopupDialogDisappeared();

            Step("7. Expected The popup goes away and geozone was not moved");
            var geozoneNotMovedYet = equipmentInventoryPage.GeozoneTreeMainPanel.GetChildNodeNamesOfSelectedNode(NodeType.GeoZone).Contains(dragdropGeozone);
            VerifyEqual("7. Verify the moved geozone has not been moved yet", false, geozoneNotMovedYet);
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(parentGeozonePath);
            geozoneNotMovedYet = equipmentInventoryPage.GeozoneTreeMainPanel.GetChildNodeNamesOfSelectedNode(NodeType.GeoZone).Contains(dragdropGeozone);
            VerifyEqual("7. Verify the moved geozone is still in parent", true, geozoneNotMovedYet);

            Step("8. Drag and drop it into its new parent geozone again and click \"Yes\" on the popup");
            equipmentInventoryPage.GeozoneTreeMainPanel.MoveNodeToGeozone(dragdropGeozone, destGeozone);
            equipmentInventoryPage.WaitForPopupDialogDisplayed();
            equipmentInventoryPage.Dialog.ClickYesButton();
            equipmentInventoryPage.WaitForPreviousActionComplete();

            Step("9. Expected Value of Parent field in Geozone editor of the moved device should be updated to the geozone where it has been moved to");
            var isParentGeozoneReadonly = equipmentInventoryPage.GeozoneEditorPanel.IsParentGeozoneInputReadOnly();
            if (isParentGeozoneReadonly)
            {
                var actualParentGeozoneName = equipmentInventoryPage.GeozoneEditorPanel.GetParentGeozoneValue();
                VerifyTrue(string.Format("9. Verify Value of Parent field should be updated to the geozone '{0}'", destGeozone), destGeozone.Equals(actualParentGeozoneName), destGeozone, actualParentGeozoneName);
            }
            else
            {
                var parentGeozoneFullName = string.Format(@"{0} [{1}]", destGeozone, destGeozonePath.GetParentName());
                var actualParentGeozoneFullname = equipmentInventoryPage.GeozoneEditorPanel.GetParentGeozoneValue();
                VerifyTrue(string.Format("[SC-32] 9. Verify Value of Parent field should be updated to the geozone '{0}'", parentGeozoneFullName), parentGeozoneFullName.Equals(actualParentGeozoneFullname), parentGeozoneFullName, actualParentGeozoneFullname);
            }

            Step("10. Expected The geozone is moved to the new geozone in the goezone tree, and so are the subgeozones");
            var geozoneNoLongerInPrevious = !equipmentInventoryPage.GeozoneTreeMainPanel.GetChildNodeNamesOfSelectedNode(NodeType.GeoZone).Contains(dragdropGeozone);
            VerifyEqual("10. Verify the moved geozone is no longer present in its old parent geozone",true, geozoneNoLongerInPrevious);

            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(destGeozonePath);
            var isSourceGeozonePresentInDestGeozone = equipmentInventoryPage.GeozoneTreeMainPanel.GetChildNodeNamesOfSelectedNode(NodeType.GeoZone).Contains(dragdropGeozone);
            VerifyEqual("10. Verify new geozone is now present in new parent geozone", true, isSourceGeozonePresentInDestGeozone);

            // Move to moved source geozone to check if its children are moved as well
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(dragdropGeozone);
            var actualListOfSourceGeozoneNodesAsText = equipmentInventoryPage.GeozoneTreeMainPanel.GetChildNodeNamesOfSelectedNode();
            VerifyEqual("10. Verify child node of moved geozone are also moved accordingly", expectedListOfSourceGeozoneNodesAsText, actualListOfSourceGeozoneNodesAsText);

            Step("11. Refresh browser then go to Equipment Inventory app again");
            desktopPage = Browser.RefreshLoggedInCMS();
            equipmentInventoryPage = desktopPage.GoToApp(App.EquipmentInventory) as EquipmentInventoryPage;

            Step("11. Expected The moved geozone and its subgeozones and devices are actually moved (under new parent geozone)");
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(destGeozonePath);

            isSourceGeozonePresentInDestGeozone = equipmentInventoryPage.GeozoneTreeMainPanel.GetChildNodeNamesOfSelectedNode(NodeType.GeoZone).Contains(dragdropGeozone);
            VerifyEqual("[SLV-3846, SC-32] 11. Verify new geozone is now present in new parent geozone", true, isSourceGeozonePresentInDestGeozone);

            // Move to moved source geozone to check if its children are moved as well
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(dragdropGeozone);
            actualListOfSourceGeozoneNodesAsText = equipmentInventoryPage.GeozoneTreeMainPanel.GetChildNodeNamesOfSelectedNode();
            VerifyEqual("11. Verify child node of moved geozone are also moved accordingly", expectedListOfSourceGeozoneNodesAsText, actualListOfSourceGeozoneNodesAsText);
            
            try
            {
                DeleteGeozone(dragdropGeozone);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("TS 2.4.1 Equipment Inventory - Move device to another geozone")]
        public void TS2_04_01()
        {
            var sourceGeozone = SLVHelper.GenerateUniqueName("GZNTS20401S");
            var destGeozone = SLVHelper.GenerateUniqueName("GZNTS20401D");
            var controller = SLVHelper.GenerateUniqueName("CLTS20401");
            var movedDevice = SLVHelper.GenerateUniqueName("MovedSTL");

            Step("-> Create data for testing");
            DeleteGeozones("GZNTS20401*");
            CreateNewGeozone(sourceGeozone);
            CreateNewGeozone(destGeozone);
            CreateNewController(controller, sourceGeozone);
            CreateNewDevice(DeviceType.Streetlight, movedDevice, controller, sourceGeozone);

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.EquipmentInventory);

            Step("1. Go to Equipment Inventory app");
            Step("2. Expected Equipment Inventory page is routed and loaded successfully");
            var equipmentInventoryPage = desktopPage.GoToApp(App.EquipmentInventory) as EquipmentInventoryPage;

            Step("3. Select a geozone (e.g. geozone A) and note its attitudes and longitudes");
            Step("4. Select another geozone (e.g. geozone B) and select one of its devices (e.g. device X). Note its attitudes and longitudes");
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(sourceGeozone);
            equipmentInventoryPage.WaitForEditorPanelDisplayed();
            var notedLongMin = equipmentInventoryPage.GeozoneEditorPanel.GetLongitudeMinimumValue();
            var notedLongMax = equipmentInventoryPage.GeozoneEditorPanel.GetLongitudeMaximumValue();
            var notedLatMin = equipmentInventoryPage.GeozoneEditorPanel.GetLatitudeMinimumValue();
            var notedLatMax = equipmentInventoryPage.GeozoneEditorPanel.GetLatitudeMaximumValue();

            Step("5. Drag and drop device X to geozone A");
            equipmentInventoryPage.GeozoneTreeMainPanel.MoveNodeToGeozone(movedDevice, destGeozone);

            Step("6. Expected There is a popup asking \"Would you like to move {{device name}} equipment ?\"");
            VerifyEqual("6. Verify confirmation dialog appears", true, equipmentInventoryPage.Dialog.IsDialogVisible());
            VerifyEqual("6. Verify confirmation message", string.Format("Would you like to move {0} equipment ?", movedDevice), equipmentInventoryPage.Dialog.GetMessageText());

            Step("7. Click \"No\"");
            equipmentInventoryPage.Dialog.ClickNoButton();
            equipmentInventoryPage.WaitForPopupDialogDisappeared();

            Step("8. Expected The popup goes away and device was not moved");
            var deviceNotMovedYet = equipmentInventoryPage.GeozoneTreeMainPanel.GetChildNodeNamesOfSelectedNode().Contains(movedDevice);
            VerifyEqual("8. Verify the moved device has not been moved yet", true, deviceNotMovedYet);

            Step("9. Drag and drop device X into geozone A again and click \"Yes\" on the popup");
            equipmentInventoryPage.GeozoneTreeMainPanel.MoveNodeToGeozone(movedDevice, destGeozone);
            equipmentInventoryPage.WaitForPopupDialogDisplayed();
            equipmentInventoryPage.Dialog.ClickYesButton();
            equipmentInventoryPage.WaitForPreviousActionComplete();
            equipmentInventoryPage.WaitForPopupDialogDisappeared();

            Step("10. Expected Value of Parent field in Streetlight editor of the moved device should be updated to the geozone where it has been moved to");
            var isParentGeozoneReadonly = equipmentInventoryPage.GeozoneEditorPanel.IsParentGeozoneInputReadOnly();
            if (isParentGeozoneReadonly)
            {
                var actualParentGeozoneName = equipmentInventoryPage.GeozoneEditorPanel.GetParentGeozoneValue();
                VerifyTrue(string.Format("10. Verify Value of Parent field should be updated to the geozone '{0}'", destGeozone), destGeozone.Equals(actualParentGeozoneName), destGeozone, actualParentGeozoneName);
            }

            Step("11. Expected The device is moved to geozone A (present in A and not present in B any longer)");
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(destGeozone);
            equipmentInventoryPage.WaitForEditorPanelDisplayed();
            var movedDevicePresentInNewGeozone = equipmentInventoryPage.GeozoneTreeMainPanel.GetChildNodeNamesOfSelectedNode().Contains(movedDevice);
            VerifyEqual("11. Verify the moved device has been moved to new geozone", true, movedDevicePresentInNewGeozone);

            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(sourceGeozone);
            equipmentInventoryPage.WaitForEditorPanelDisplayed();
            var movedDeviceNotPresentInOldGeozone = !equipmentInventoryPage.GeozoneTreeMainPanel.GetChildNodeNamesOfSelectedNode().Contains(movedDevice);
            VerifyEqual("11. Verify the moved device is no longer present in the old geozone", true, movedDeviceNotPresentInOldGeozone);

            Step("12. Select geozone A");
            Step("13. Expected Geozone A's boundary is unchanged if device X's location is inside A boundary. Otherwise, geozone A's boundary are updated accordingly to device X's location");            
            VerifyEqual("13. Verify Longitude Minimum is unchanged", notedLongMin, equipmentInventoryPage.GeozoneEditorPanel.GetLongitudeMinimumValue());
            VerifyEqual("13. Verify Longitude Maximum is unchanged", notedLongMax, equipmentInventoryPage.GeozoneEditorPanel.GetLongitudeMaximumValue());
            VerifyEqual("13. Verify Latitude Minimum is unchanged", notedLatMin, equipmentInventoryPage.GeozoneEditorPanel.GetLatitudeMinimumValue());
            VerifyEqual("13. Verify Latitude Maximum is unchanged", notedLatMax, equipmentInventoryPage.GeozoneEditorPanel.GetLatitudeMaximumValue());

            Step("14. Refresh browser and go to Equipment Inventory page again");
            desktopPage = Browser.RefreshLoggedInCMS();
            equipmentInventoryPage = desktopPage.GoToApp(App.EquipmentInventory) as EquipmentInventoryPage;

            Step("15. Expected Verify the device is actually moved to new geozone (present in A and not present in B any longer)");
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(destGeozone);
            equipmentInventoryPage.WaitForEditorPanelDisplayed();
            movedDevicePresentInNewGeozone = equipmentInventoryPage.GeozoneTreeMainPanel.GetChildNodeNamesOfSelectedNode().Contains(movedDevice);
            VerifyEqual("15. Verify the moved device has been moved to new geozone", true, movedDevicePresentInNewGeozone);

            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(sourceGeozone);
            equipmentInventoryPage.WaitForEditorPanelDisplayed();
            movedDeviceNotPresentInOldGeozone = !equipmentInventoryPage.GeozoneTreeMainPanel.GetChildNodeNamesOfSelectedNode().Contains(movedDevice);
            VerifyEqual("15. Verify the moved device is no longer present in the old geozone", true, movedDeviceNotPresentInOldGeozone);
            
            try
            {
                DeleteGeozone(sourceGeozone);
                DeleteGeozone(destGeozone);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("TS 2.4.2 Equipment Inventory - Move geozone to another one by changing its parent geozone")]
        public void TS2_04_02()
        {
            var testData = GetTestDataOfTestTS2_04_02();
            var parentGeozonePath = testData["ParentGeozone"];
            var sourceGeozone = SLVHelper.GenerateUniqueName("Source");
            var destGeozone = SLVHelper.GenerateUniqueName("Dest");
            var parentGeozoneName = parentGeozonePath.GetChildName();

            Step("-> Create data for testing");
            CreateNewGeozone(sourceGeozone, parentGeozoneName);
            CreateNewGeozone(destGeozone, parentGeozoneName);

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.EquipmentInventory);

            Step("1. Go to Equipment Inventory app");
            Step("2. Expected Equipment Inventory page is routed and loaded successfully");
            var equipmentInventoryPage = desktopPage.GoToApp(App.EquipmentInventory) as EquipmentInventoryPage;

            Step("3. Select a geozone from geozone tree");
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(parentGeozonePath + @"\" + sourceGeozone);
            equipmentInventoryPage.WaitForEditorPanelDisplayed();

            Step("4. Select a geozone from Parent field on the right widget and click Save");
            equipmentInventoryPage.GeozoneEditorPanel.SelectParentGeozoneDropDown(string.Format("{0} [{1}]", destGeozone, parentGeozoneName));
            equipmentInventoryPage.GeozoneEditorPanel.ClickSaveButton();
            equipmentInventoryPage.WaitForPreviousActionComplete();
            equipmentInventoryPage.WaitForEditorPanelDisappeared();

            Step("5. Expected The geozone is moved to the new geozone in the goezone tree, and so are the subgeozones");
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(parentGeozonePath);
            equipmentInventoryPage.WaitForEditorPanelDisplayed();

            var sourceGeozoneAlreadyMoved = !equipmentInventoryPage.GeozoneTreeMainPanel.GetChildNodeNamesOfSelectedNode().Contains(sourceGeozone);
            VerifyEqual("5. Verify the moved geozone has been moved to new geozone", true, sourceGeozoneAlreadyMoved);

            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(destGeozone);
            equipmentInventoryPage.WaitForEditorPanelDisplayed();

            var sourceGeozonePresentInNewGeozone = equipmentInventoryPage.GeozoneTreeMainPanel.GetChildNodeNamesOfSelectedNode().Contains(sourceGeozone);
            VerifyEqual("5. Verify the moved geozone is present in new geozone", true, sourceGeozonePresentInNewGeozone);

            Step("6. Refresh browser then go to Equipment Inventory app again");
            desktopPage = Browser.RefreshLoggedInCMS();
            equipmentInventoryPage = desktopPage.GoToApp(App.EquipmentInventory) as EquipmentInventoryPage;

            Step("7. Expected The moved geozone and its subgeozones and devices are actually moved (under new parent geozone)");
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(parentGeozonePath);
            equipmentInventoryPage.WaitForEditorPanelDisplayed();

            sourceGeozoneAlreadyMoved = !equipmentInventoryPage.GeozoneTreeMainPanel.GetChildNodeNamesOfSelectedNode().Contains(sourceGeozone);
            VerifyEqual("7. Verify the moved geozone has been moved to new geozone", true, sourceGeozoneAlreadyMoved);

            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(destGeozone);
            equipmentInventoryPage.WaitForEditorPanelDisplayed();

            sourceGeozonePresentInNewGeozone = equipmentInventoryPage.GeozoneTreeMainPanel.GetChildNodeNamesOfSelectedNode().Contains(sourceGeozone);
            VerifyEqual("[SLV-3846] 7. Verify the moved geozone is present in new geozone", true, sourceGeozonePresentInNewGeozone);
            
            try
            {
                //Delete geozones created
                DeleteGeozone(destGeozone);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("TS 2.5.1 Equipment Inventory - Create streetlight")]
        public void TS2_05_01()
        {
            Assert.Pass("Covered by EI_16, EI_16_01");
        }

        [Test, DynamicRetry]
        [Description("TS 2.5.2	Equipment Inventory - Edit streetlight")]
        public void TS2_05_02()
        {
            Assert.Pass("Covered by EI_17, EI_17_01, EI_17_02");
        }

        [Test, DynamicRetry]
        [Description("TS 2.5.3	Equipment Inventory - Remove streetlight")]
        public void TS2_05_03()
        {
            Assert.Pass("Covered by EI_18");
        }

        [Test, DynamicRetry]
        [Description("TS 2.7.1 Equipment Inventory - Duplicate device")]
        public void TS2_07_01()
        {
            Assert.Pass("Covered by EI_19");
        }        

        [Test, DynamicRetry]
        [Description("TS 2.12.1	Equipment Inventory - Search function")]
        public void TS2_12_01()
        {
            var testData = GetTestDataOfTestTS2_12_01();

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.EquipmentInventory);

            Step("1. Go to Equipment Inventory app");
            Step("2. Expected Equipment Inventory page is routed and loaded successfully");          
            var equipmentInventoryPage = desktopPage.GoToApp(App.EquipmentInventory) as EquipmentInventoryPage;

            Step("3. Perform search by complete/incomplete/inexisting Name, Unique address, Identifier");
            Step("4. Expected Search result widget displays matched devices with complete/incomplete values, displays \"No result\" with inexisting values");
            Step("5. Select a device found in Search result widget");
            Step("6. Expected The map highlights the selected device on the map. Device panel displays details of the selected device");
            foreach (var searchNode in testData["SearchNodeList"])
            {
                var searchAttribute = searchNode.GetAttrVal("attribute");
                var searchOperator = searchNode.GetAttrVal("operator");
                var inexistingValue = searchNode.GetAttrVal("inexistingValue");
                var completeValue = searchNode.GetAttrVal("completeValue");
                var incompleteValue = searchNode.GetAttrVal("incompleteValue");

                equipmentInventoryPage.GeozoneTreeMainPanel.ClickExpandSearchButton();
                equipmentInventoryPage.GeozoneTreeMainPanel.SelectAttributeDropDown(searchAttribute);
                equipmentInventoryPage.GeozoneTreeMainPanel.SelectOperatorDropDown(searchOperator);

                Step("Search with inexisting value");
                equipmentInventoryPage.GeozoneTreeMainPanel.EnterSearchTextInput(inexistingValue);
                equipmentInventoryPage.GeozoneTreeMainPanel.ClickSearchButton();
                equipmentInventoryPage.WaitForPreviousActionComplete();

                var noResultFound = equipmentInventoryPage.GeozoneTreeMainPanel.SearchResultsGeozonePanel.IsSearchResultMessageDisplayed();
                VerifyTrue("6.1 Verify no device found", noResultFound == true, true, noResultFound);

                Step("Search with complete value");
                equipmentInventoryPage.GeozoneTreeMainPanel.EnterSearchTextInput(completeValue);
                equipmentInventoryPage.GeozoneTreeMainPanel.ClickSearchButton();
                equipmentInventoryPage.WaitForPreviousActionComplete();

                var resultFound = equipmentInventoryPage.GeozoneTreeMainPanel.SearchResultsGeozonePanel.IsSearchResultContentFoundDisplayed();
                VerifyEqual("6.2 Verify 1 or many devices found", true, resultFound);

                Step("Select a found device");
                var selectedDevice = equipmentInventoryPage.GeozoneTreeMainPanel.SearchResultsGeozonePanel.SelectRandomFoundDevice().FirstOrDefault();
                equipmentInventoryPage.WaitForPreviousActionComplete();
                equipmentInventoryPage.Map.WaitForDevicesDisplayedOnGLMap();
                equipmentInventoryPage.WaitForDeviceEditorPanelDisplayed();
                Wait.ForGLMapStopFlying();

                var deviceModel = equipmentInventoryPage.Map.GetFirstSelectedDevice();
                VerifyEqual("6.3 Verify active device name on the map", selectedDevice, equipmentInventoryPage.Map.MoveAndGetDeviceNameGL(deviceModel.Longitude, deviceModel.Latitude));

                Step("Search with incomplete value");
                equipmentInventoryPage.GeozoneTreeMainPanel.EnterSearchTextInput(incompleteValue);
                equipmentInventoryPage.GeozoneTreeMainPanel.ClickSearchButton();
                equipmentInventoryPage.WaitForPreviousActionComplete();

                resultFound = equipmentInventoryPage.GeozoneTreeMainPanel.SearchResultsGeozonePanel.IsSearchResultContentFoundDisplayed();
                VerifyEqual("6.4 Verify 1 or many devices found", true, resultFound);

                Step("Select a found device");
                selectedDevice = equipmentInventoryPage.GeozoneTreeMainPanel.SearchResultsGeozonePanel.SelectRandomFoundDevice().FirstOrDefault();
                equipmentInventoryPage.WaitForPreviousActionComplete();
                equipmentInventoryPage.Map.WaitForDevicesDisplayedOnGLMap();
                equipmentInventoryPage.WaitForDeviceEditorPanelDisplayed();
                Wait.ForGLMapStopFlying();

                deviceModel = equipmentInventoryPage.Map.GetFirstSelectedDevice();
                VerifyEqual("6.5 Verify active device name on the map", selectedDevice, equipmentInventoryPage.Map.MoveAndGetDeviceNameGL(deviceModel.Longitude, deviceModel.Latitude));
            }
        }        

        [Test, DynamicRetry]
        [Description("TS 2.14.1 Equipment Inventory - Custom report")]
        public void TS2_14_01()
        {
            var testData = GetTestDataOfTestTS2_14_01();
            var geozone = testData["Geozone"].ToString();
            var streetlights = testData["Streetlights"] as List<DeviceModel>;
            var streetlightName = streetlights.PickRandom().Name;

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.EquipmentInventory);
            Step("1. Go to Equipment Inventory app");
            Step("2. Expected Equipment Inventory page is routed and loaded successfully");           
            var equipmentInventoryPage = desktopPage.GoToApp(App.EquipmentInventory) as EquipmentInventoryPage;

            Step("3. Select a geozone from geozone tree. The gezone has no sub-geozone");
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(geozone);

            Step("4. Click Custom report at the bottom of Device widget on the right side");
            equipmentInventoryPage.GeozoneEditorPanel.ClickCustomReportButton();
            equipmentInventoryPage.WaitForPreviousActionComplete();
            equipmentInventoryPage.GridPanel.WaitForGridContentAvailable();

            Step("5. Expected Main panel displays custom report for selected geozone. Each row is a device of the selected geozone");
            var geozoneNodeList = equipmentInventoryPage.GeozoneTreeMainPanel.GetChildNodeNamesOfSelectedNode().OrderBy(n => n).ToList();
            var gridNodeList = equipmentInventoryPage.GridPanel.GetListOfColumnData("Device").OrderBy(n => n).ToList();
            VerifyEqual("5. Verify nodes in selected geozone and grid", geozoneNodeList, gridNodeList);

            Step("6. Click a device in the grid");
            equipmentInventoryPage.GridPanel.ClickGridRecord(streetlightName);
            equipmentInventoryPage.WaitForPreviousActionComplete();
            equipmentInventoryPage.Map.WaitForDevicesDisplayedOnGLMap();
            equipmentInventoryPage.GeozoneTreeMainPanel.WaitForSelectedNodeText(streetlightName);

            Step("7. Expected The clicked device is highlighted in the geozone tree; Main panel goes away; The clicked device is highlighted in the geozone tree and on the map");
            var selectedNodeText = equipmentInventoryPage.GeozoneTreeMainPanel.GetSelectedNodeText();
            VerifyEqual("7. Verify the selected node in geozone", streetlightName, selectedNodeText);
            VerifyEqual("7. Verify grid panel goes away", false, equipmentInventoryPage.IsCustomReportPanelDisplayed());
            var deviceModel = equipmentInventoryPage.Map.GetFirstSelectedDevice();
            var activeDeviceName = equipmentInventoryPage.Map.MoveAndGetDeviceNameGL(deviceModel.Longitude, deviceModel.Latitude);
            VerifyEqual("7. Verify active device name on the map", streetlightName, activeDeviceName);
        }        

        [Test, DynamicRetry]
        [Description("TS 2.15.1 Equipment Inventory - Number of devices in a geozone")]
        public void TS2_15_01()
        {
            var geozone1 = SLVHelper.GenerateUniqueName("GZNTS2150101");
            var geozone2 = SLVHelper.GenerateUniqueName("GZNTS2150102");
            var controller = SLVHelper.GenerateUniqueName("CTRL");
            var typeOfEquipment = "SSN Cimcon Dim Photocell[Lamp #0]";
            var geozoneList = new List<string> { geozone1, geozone2 };

            var deviceList = new List<string>();
            var dicGeozoneNodeCount = new Dictionary<string, int>();

            Step("-> Create data for testing");
            DeleteGeozones("GZNTS215010*");
            CreateNewGeozone(geozone1);
            CreateNewGeozone(geozone2, geozone1);
            CreateNewController(controller, geozone2);

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.EquipmentInventory);

            Step("1. Go to Equipment Inventory app");
            Step("2. Expected Equipment Inventory page is routed and loaded successfully");           
            var equipmentInventoryPage = desktopPage.GoToApp(App.EquipmentInventory) as EquipmentInventoryPage;

            Step("3. Select a geozone and add devices into it");
            Step("4. **Expected** When a device is added, number of devices of the selected geozone and its higher level geozones are updated accordingly");
            foreach (var geozone in geozoneList)
            {
                equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(geozone);
                dicGeozoneNodeCount[geozone] = equipmentInventoryPage.GeozoneTreeMainPanel.GetSelectedNodeDevicesCount();
            }

            for (var i = 0; i < SLVHelper.GenerateInteger(5); i++)
            {
                var device = SLVHelper.GenerateUniqueName("STL");
                deviceList.Add(device);
                equipmentInventoryPage.CreateDevice(DeviceType.Streetlight, device, controller, device, typeOfEquipment);

                foreach (var geozone in geozoneList)
                {
                    equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(geozone);
                    var deviceCount = equipmentInventoryPage.GeozoneTreeMainPanel.GetSelectedNodeDevicesCount();
                    dicGeozoneNodeCount[geozone] = dicGeozoneNodeCount[geozone] + 1;
                    VerifyEqual("4. Verify device count increases 1", dicGeozoneNodeCount[geozone], deviceCount);
                }
            }

            try
            {
                DeleteGeozone(geozone1);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("TS 2.16.1 Equipment Inventory - Streetlight widget - All types")]
        public void TS2_16_01()
        {
            Assert.Pass("Covered by EI_26");
        }

        [Test, DynamicRetry]
        [Description("TS 2.16.2 Equipment Inventory - Streetlight widget - Telematics LCU")]
        public void TS2_16_02()
        {
            Assert.Pass("Covered by EI_26_01");
        }

        [Test, DynamicRetry]
        [Description("TS 2.16.3 Equipment Inventory - Streetlight widget - SSN Cimcon Dim Photocell")]
        public void TS2_16_03()
        {
            Assert.Pass("Covered by EI_26_02");
        }

        [Test, DynamicRetry]
        [Description("TS 2.17.1 Equipment Inventory - Controller widget - All types")]
        public void TS2_17_01()
        {
            Assert.Pass("Covered by EI_14");
        }

        [Test, DynamicRetry]
        [Description("TS 2.17.2 Equipment Inventory - Controller widget - iLON Segment Controller")]
        public void TS2_17_02()
        {
            Assert.Pass("Covered by EI_14_01");
        }

        [Test, DynamicRetry]
        [Description("TS 2.17.3 Equipment Inventory - Controller widget - Open South Bound XML Web API")]
        public void TS2_17_03()
        {
            Assert.Pass("Covered by EI_14_02");
        }

        [Test, DynamicRetry]
        [Description("TS 2.17.4 Equipment Inventory - Controller widget - Telematics Wireless")]
        public void TS2_17_04()
        {
            Assert.Pass("Covered by EI_14_03");
        }

        [Test, DynamicRetry]
        [Description("TS 2.17.5 Equipment Inventory - Controller widget - Silver Spring Networks")]
        public void TS2_17_05()
        {
            Assert.Pass("Covered by EI_14_04");
        }      

        [Test, DynamicRetry]
        [Description("TS 2.18.1 Equipment Inventory - Meter widget - Cirwatt Mini")]
        public void TS2_18_01()
        {
            Assert.Pass("Covered by EI_45_01");
        }

        [Test, DynamicRetry]
        [Description("TS 2.19.1 Equipment Inventory - Actions on lamp type")]
        public void TS2_19_01()
        {
            Assert.Pass("Covered by EI_23, EI_23_01, EI_23_02, EI_23_03");
        }

        [Test, DynamicRetry]
        [Description("TS 2.20.1 Equipment Inventory - Actions on energy supplier - Create")]
        public void TS2_20_01()
        {
            Assert.Pass("Covered by EI_24_01");
        }

        [Test, DynamicRetry]
        [Description("TS 2.20.2 Equipment Inventory - Actions on energy supplier - Edit")]
        public void TS2_20_02()
        {
            Assert.Pass("Covered by EI_24_02");
        }

        [Test, DynamicRetry]
        [Description("TS 2.20.3 Equipment Inventory - Actions on energy supplier - Remove")]
        public void TS2_20_03()
        {
            Assert.Pass("Covered by EI_24_03");
        }

        [Test, DynamicRetry]
        [Description("TS 2.20.4 Equipment Inventory - Actions on energy supplier - Select")]
        public void TS2_20_04()
        {
            Assert.Pass("Covered by EI_24");
        }

        [Test, DynamicRetry]
        [Description("TS 2.21.1 Inventory Verification - Verify streetlight with Yes")]
        [NonParallelizable]
        public void TS2_21_01()
        {
            var testData = GetTestDataOfTestTS2_21_01();
            var controllerId = testData["ControllerId"];
            var controllerName = testData["ControllerName"];
            var address1 = testData["Address1"];
            var address2 = testData["Address2"];
            var city = testData["City"];
            var zipcode = testData["Zipcode"];
            var isLampPositionCorrect = testData["IsLampPositionCorrect"];
            var geozone = SLVHelper.GenerateUniqueName("GZNTS22101");
            var streetlight = SLVHelper.GenerateUniqueName("STL");
            var fullStreetlightPath = string.Format(@"{0}\{1}", geozone, streetlight);

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            Step("-> Create data for testing");
            DeleteGeozones("GZNTS22101*");
            CreateNewGeozone(geozone, latMin: "12.24204", latMax: "12.27351", lngMin: "105.77142", lngMax: "105.83913");
            CreateNewDevice(DeviceType.Streetlight, streetlight, controllerId, geozone, lat: SLVHelper.GenerateCoordinate("12.25065", "12.26740"), lng: SLVHelper.GenerateCoordinate("105.80181", "105.81553"));

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.InventoryVerification);
            
            Step("1. Go to Inventory Verification app");
            Step("2. Expected Inventory Verification page is routed and loaded successfully");
            var inventoryVerificationPage = desktopPage.GoToApp(App.InventoryVerification) as InventoryVerificationPage;

            Step("3. Expand to left node of a geozone and select a light point");
            inventoryVerificationPage.GeozoneTreeMainPanel.SelectNode(fullStreetlightPath);

            Step("4. Expected A dialog with details are shown");
            inventoryVerificationPage.WaitForPopupDialogDisplayed();
            VerifyEqual(string.Format("Verify title {0} is correct", streetlight), streetlight, inventoryVerificationPage.VerificationPopupPanel.GetPanelTitleText());
            Step(" - Verify in the first page of the wizard, following fields are present: (SLV-1926)");
            Step("  + Controller ID *");
            Step("  + Identifier *");
            Step("  + Name *");
            Step("  + Address 1");
            Step("  + Address 2");
            Step("  + City");
            Step("  + Zip code");
            Step("  + Is lamp position correct?");
            Step(" - Verify all fields are editable");
            VerifySLV1926ForVerification(inventoryVerificationPage);

            Step("5. Enter some data into fields");
            inventoryVerificationPage.VerificationPopupPanel.EnterAddress1Input(address1);
            inventoryVerificationPage.VerificationPopupPanel.EnterAddress2Input(address2);
            inventoryVerificationPage.VerificationPopupPanel.EnterCityInput(city);
            inventoryVerificationPage.VerificationPopupPanel.EnterZipcodeInput(zipcode);

            Step("6. Select \"Yes\" in \"Is lamp position correct?\" field then click Next button");
            inventoryVerificationPage.VerificationPopupPanel.SelectIsLampPositionCorrectDropDown(isLampPositionCorrect);
            inventoryVerificationPage.VerificationPopupPanel.ClickNextButton();

            Step("7. Expected Next step is displayed");
            inventoryVerificationPage.VerificationPopupPanel.WaitForMoreInfoFormDisplayed();

            Step("8. Un/check and leave comment on fields then click Back icon of top-left corner");
            inventoryVerificationPage.VerificationPopupPanel.ClickBackButton();

            Step("9. Expected Changes on previous step are still kept");
            inventoryVerificationPage.VerificationPopupPanel.WaitForBasicInfoFormDisplayed();
            VerifyBasicInfoForm(inventoryVerificationPage, controllerName, streetlight, streetlight, address1, address2, city, zipcode, isLampPositionCorrect);

            Step("10. Click Next button");
            inventoryVerificationPage.VerificationPopupPanel.ClickNextButton();

            Step("11. Changes on next step are still kept");
            inventoryVerificationPage.VerificationPopupPanel.WaitForMoreInfoFormDisplayed();
            VerifyMoreInfoForm(inventoryVerificationPage, false, false, string.Empty);

            Step("12. Click Next button");
            inventoryVerificationPage.VerificationPopupPanel.ClickNextButton();

            Step("13. Next step is displayed to ask for taking photo");
            inventoryVerificationPage.VerificationPopupPanel.WaitForPhotoFormDisplayed();
            VerifyPhotoForm(inventoryVerificationPage);

            Step("14. Click Back icon");
            inventoryVerificationPage.VerificationPopupPanel.ClickBackButton();
            inventoryVerificationPage.VerificationPopupPanel.WaitForMoreInfoFormDisplayed();

            Step("15. Expected Changes on previous step are still kept");
            VerifyMoreInfoForm(inventoryVerificationPage, false, false, string.Empty);

            Step("16. Click Back icon again");
            inventoryVerificationPage.VerificationPopupPanel.ClickBackButton();

            Step("17. Expected Changes on previous step are still kept");
            inventoryVerificationPage.VerificationPopupPanel.WaitForBasicInfoFormDisplayed();
            VerifyBasicInfoForm(inventoryVerificationPage, controllerName, streetlight, streetlight, address1, address2, city, zipcode, isLampPositionCorrect);

            Step("18. Click Next button");
            inventoryVerificationPage.VerificationPopupPanel.ClickNextButton();

            Step("19. Expected Changes on next step are still kept");
            inventoryVerificationPage.VerificationPopupPanel.WaitForMoreInfoFormDisplayed();
            VerifyMoreInfoForm(inventoryVerificationPage, false, false, string.Empty);

            Step("21. Click Next button");
            Step("22. Expected to taking photo step");
            inventoryVerificationPage.VerificationPopupPanel.ClickNextButton();
            inventoryVerificationPage.VerificationPopupPanel.WaitForPhotoFormDisplayed();
            VerifyPhotoForm(inventoryVerificationPage);

            Step("23. Click Next button");
            inventoryVerificationPage.VerificationPopupPanel.ClickNextButton();

            Step("24. Expected to Run test step");
            inventoryVerificationPage.VerificationPopupPanel.WaitForRunTestFormDisplayed();
            VerifyRunTestForm(inventoryVerificationPage);

            Step("25. Click Next button");
            inventoryVerificationPage.VerificationPopupPanel.ClickNextButton();

            Step("26. Expected");
            Step(" o Finish step is displayed with message \"Thank you, you can go to the next light point.\"");
            Step(" o Finish button is available instead of Next button");
            Step(" o Back icon is still available");
            inventoryVerificationPage.VerificationPopupPanel.WaitForFinishFormDisplayed();
            VerifyFinishForm(inventoryVerificationPage, "Thank you, you can go to the next light point.");

            Step("27. Click Finish button");
            inventoryVerificationPage.VerificationPopupPanel.ClickFinishButton();

            Step("28. Expected No error displays, the dialog disappears. The device on the map is turned into blue (Verified status)");
            inventoryVerificationPage.WaitForPreviousActionComplete();
            inventoryVerificationPage.WaitForPopupDialogDisappeared();

            inventoryVerificationPage.Map.ZoomIn(ZoomLevel.Level_MAX);
            VerifyBackgroundIcon(inventoryVerificationPage, streetlight, BgColor.Blue);

            try
            {
                DeleteGeozone(geozone);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("TS 2.21.2 Inventory Verification - Verify streetlight with No")]
        [NonParallelizable]
        public void TS2_21_02()
        {
            var testData = GetTestDataOfTestTS2_21_02();
            var reason = testData["Reason"];
            var controllerId = testData["ControllerId"];
            var controllerName = testData["ControllerName"];
            var address1 = testData["Address1"];
            var address2 = testData["Address2"];
            var city = testData["City"];
            var zipcode = testData["Zipcode"];
            var isLampPositionCorrect = testData["IsLampPositionCorrect"];
            var geozone = SLVHelper.GenerateUniqueName("GZNTS22102");
            var streetlight = SLVHelper.GenerateUniqueName("STL");
            var fullStreetlightPath = string.Format(@"{0}\{1}", geozone, streetlight);

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");
                        
            Step("-> Create data for testing");
            DeleteGeozones("GZNTS22102*");
            CreateNewGeozone(geozone, latMin: "12.24204", latMax: "12.27351", lngMin: "105.77142", lngMax: "105.83913");
            CreateNewDevice(DeviceType.Streetlight, streetlight, controllerId, geozone, lat: SLVHelper.GenerateCoordinate("12.25065", "12.26740"), lng: SLVHelper.GenerateCoordinate("105.80181", "105.81553"));

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.InventoryVerification);            

            Step("1. Go to Inventory Verification app");
            Step("2. Expected Inventory Verification page is routed and loaded successfully");
            var inventoryVerificationPage = desktopPage.GoToApp(App.InventoryVerification) as InventoryVerificationPage;

            Step("3. Expand to left node of a geozone and select a light point");
            inventoryVerificationPage.GeozoneTreeMainPanel.SelectNode(fullStreetlightPath);

            Step("4. Expected A dialog with details are shown");
            inventoryVerificationPage.WaitForPopupDialogDisplayed();
            VerifyEqual(string.Format("Verify title {0} is correct", streetlight), streetlight, inventoryVerificationPage.VerificationPopupPanel.GetPanelTitleText());
            VerifySLV1926ForVerification(inventoryVerificationPage);

            Step("5. Enter some data into fields");
            inventoryVerificationPage.VerificationPopupPanel.EnterAddress1Input(address1);
            inventoryVerificationPage.VerificationPopupPanel.EnterAddress2Input(address2);
            inventoryVerificationPage.VerificationPopupPanel.EnterCityInput(city);
            inventoryVerificationPage.VerificationPopupPanel.EnterZipcodeInput(zipcode);

            Step("6. Select \"No\" in \"Is lamp position correct?\" field then click Next button");
            inventoryVerificationPage.VerificationPopupPanel.SelectIsLampPositionCorrectDropDown(isLampPositionCorrect);
            inventoryVerificationPage.VerificationPopupPanel.ClickNextButton();

            Step("7. Expected Next step is displayed");
            inventoryVerificationPage.VerificationPopupPanel.WaitForReasonFormDisplayed();

            Step("8. Select a reason then click Next button");
            if (!string.IsNullOrEmpty(reason))
                inventoryVerificationPage.VerificationPopupPanel.SelectReasonDropDown(reason);
            inventoryVerificationPage.VerificationPopupPanel.ClickNextButton();

            Step("9. Expected Even if reason is empty or not, Taking photo step is moved to");
            inventoryVerificationPage.VerificationPopupPanel.WaitForPhotoFormDisplayed();
            VerifyPhotoForm(inventoryVerificationPage);

            Step("10. Click Next button");
            inventoryVerificationPage.VerificationPopupPanel.ClickNextButton();

            Step("11. Expected to Run test step");
            inventoryVerificationPage.VerificationPopupPanel.WaitForRunTestFormDisplayed();
            VerifyRunTestForm(inventoryVerificationPage);

            Step("12. Click Next button");
            inventoryVerificationPage.VerificationPopupPanel.ClickNextButton();

            Step("13. Expected");
            Step(" o Finish step is displayed with message \"Thank you, you can go to the next light point.\"");
            Step(" o Finish button is available instead of Next button");
            Step(" o Back icon is still available");
            inventoryVerificationPage.VerificationPopupPanel.WaitForFinishFormDisplayed();
            VerifyFinishForm(inventoryVerificationPage, "Thank you, you can go to the next light point.");

            Step("14. Click Finish button");
            inventoryVerificationPage.VerificationPopupPanel.ClickFinishButton();

            Step("15. Expected No error displays, the dialog disappears. The device on the map is turned into Yellow (Does not exist status)");
            inventoryVerificationPage.WaitForPreviousActionComplete();
            inventoryVerificationPage.WaitForPopupDialogDisappeared();

            inventoryVerificationPage.Map.ZoomIn(ZoomLevel.Level_MAX);
            VerifyBackgroundIcon(inventoryVerificationPage, streetlight, BgColor.Yellow);
            
            try
            {
                DeleteGeozone(geozone);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("TTS 2.21.3 Inventory Verification - Verify streetlight with Can't verify")]
        [NonParallelizable]
        public void TS2_21_03()
        {
            var testData = GetTestDataOfTestTS2_21_03();
            var reason = testData["Reason"];
            var controllerId = testData["ControllerId"];
            var controllerName = testData["ControllerName"];
            var address1 = testData["Address1"];
            var address2 = testData["Address2"];
            var city = testData["City"];
            var zipcode = testData["Zipcode"];
            var isLampPositionCorrect = testData["IsLampPositionCorrect"];
            var geozone = SLVHelper.GenerateUniqueName("GZNTS22103");
            var streetlight = SLVHelper.GenerateUniqueName("STL");
            var fullStreetlightPath = string.Format(@"{0}\{1}", geozone, streetlight);

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            Step("-> Create data for testing");
            DeleteGeozones("GZNTS22103*");
            CreateNewGeozone(geozone, latMin: "12.24204", latMax: "12.27351", lngMin: "105.77142", lngMax: "105.83913");
            CreateNewDevice(DeviceType.Streetlight, streetlight, controllerId, geozone, lat: SLVHelper.GenerateCoordinate("12.25065", "12.26740"), lng: SLVHelper.GenerateCoordinate("105.80181", "105.81553"));

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.InventoryVerification);

            Step("1. Go to Inventory Verification app");
            Step("2. Expected Inventory Verification page is routed and loaded successfully");
            var inventoryVerificationPage = desktopPage.GoToApp(App.InventoryVerification) as InventoryVerificationPage;

            Step("3. Expand to left node of a geozone and select a light point");
            inventoryVerificationPage.GeozoneTreeMainPanel.SelectNode(fullStreetlightPath);

            Step("4. Expected A dialog with details are shown");
            inventoryVerificationPage.WaitForPopupDialogDisplayed();
            VerifyEqual(string.Format("Verify title {0} is correct", streetlight), streetlight, inventoryVerificationPage.VerificationPopupPanel.GetPanelTitleText());
            VerifySLV1926ForVerification(inventoryVerificationPage);

            Step("5. Enter some data into fields");
            inventoryVerificationPage.VerificationPopupPanel.EnterAddress1Input(address1);
            inventoryVerificationPage.VerificationPopupPanel.EnterAddress2Input(address2);
            inventoryVerificationPage.VerificationPopupPanel.EnterCityInput(city);
            inventoryVerificationPage.VerificationPopupPanel.EnterZipcodeInput(zipcode);

            Step("6. Select \"Can't verify\" in \"Is lamp position correct?\" field then click Next button");
            inventoryVerificationPage.VerificationPopupPanel.SelectIsLampPositionCorrectDropDown(isLampPositionCorrect);
            inventoryVerificationPage.VerificationPopupPanel.ClickNextButton();

            Step("7. Expected Next step is displayed");
            inventoryVerificationPage.VerificationPopupPanel.WaitForReasonFormDisplayed();

            Step("8. Select a reason then click Next button");
            if (!string.IsNullOrEmpty(reason))
                inventoryVerificationPage.VerificationPopupPanel.SelectReasonDropDown(reason);
            inventoryVerificationPage.VerificationPopupPanel.ClickNextButton();

            Step("9. Expected Even if reason is empty or not, Taking photo step is moved to");
            inventoryVerificationPage.VerificationPopupPanel.WaitForPhotoFormDisplayed();
            VerifyPhotoForm(inventoryVerificationPage);

            Step("10. Click Next button");
            inventoryVerificationPage.VerificationPopupPanel.ClickNextButton();

            Step("11. Expected to Run test step");
            inventoryVerificationPage.VerificationPopupPanel.WaitForRunTestFormDisplayed();
            VerifyRunTestForm(inventoryVerificationPage);

            Step("12. Click Next button");
            inventoryVerificationPage.VerificationPopupPanel.ClickNextButton();

            Step("13. Expected");
            Step(" o Finish step is displayed with message \"Thank you, you can go to the next light point.\"");
            Step(" o Finish button is available instead of Next button");
            Step(" o Back icon is still available");
            inventoryVerificationPage.VerificationPopupPanel.WaitForFinishFormDisplayed();
            VerifyFinishForm(inventoryVerificationPage, "Thank you, you can go to the next light point.");

            Step("14. Click Finish button");
            inventoryVerificationPage.VerificationPopupPanel.ClickFinishButton();

            Step("15. Expected No error displays, the dialog disappears. The device on the map is turned into Yellow (Does not exist status)");
            inventoryVerificationPage.WaitForPreviousActionComplete();
            inventoryVerificationPage.WaitForPopupDialogDisappeared();

            inventoryVerificationPage.Map.ZoomIn(ZoomLevel.Level_MAX);
            VerifyBackgroundIcon(inventoryVerificationPage, streetlight, BgColor.LightBlue);
            
            try
            {
                DeleteGeozone(geozone);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("TS 2.22.1 Installation - Install streetlight with Yes")]
        [NonParallelizable]
        public void TS2_22_01()
        {
            var testData = GetTestDataOfTestTS2_22_01();
            var inputControllerId = testData["ControllerId"];
            var inputControllerName = testData["ControllerName"];
            var inputTypeOfEquipment = testData["TypeOfEquipment"];
            var inputLampIsAccessible = testData["LampIsAccessible"];
            var inputUniqueAddress = testData["UniqueAddress"];
            var inputNicSerialNumber = testData["NicSerialNumber"];
            var inputLampCameOn = bool.Parse(testData["LampCameOn"]);            

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - Location service is enabled on browser");
            Step("**** Precondition ****\n");
            
            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.EquipmentInventory, App.Installation);

            Step("1. Go to Installation app");
            Step("2. Expected Installation page is routed and loaded successfully");
            var installationPage = desktopPage.GoToApp(App.Installation) as InstallationPage;
            var notedScaleLineText = installationPage.Map.GetMapScaleLineText();

            Step("3. Click Follow Me");
            installationPage.Map.ClickFollowMeButton();
            installationPage.WaitForHeaderMessageDisappeared();
            SLVHelper.AllowOnceLocation();
            var scaleLineText = installationPage.Map.GetMapScaleLineText();
            if (notedScaleLineText.Equals(scaleLineText))
            {
                Warning("[SC-1868] Installation - The Follow Me function does not work anymore");
                return;
            }

            Step("4. Expected SLV CMS find the location using Google Web Service then locates the found location to the map");
            installationPage.Map.WaitForLocationMarkerDisplayed();
            installationPage.Map.ZoomIn(ZoomLevel.Level_MAX);
            VerifyEqual("4. Verify User Location Marker is displayed", true, installationPage.Map.IsLocationMarkerPresent());

            Step("5. Click New Streetlight icon on top right and place it at a point on the map");
            installationPage.Map.ClickAddStreetlightButton();
            installationPage.Map.WaitForRecorderDisplayed();
            installationPage.Map.PositionNewDevice();
            installationPage.Map.WaitForRecorderDisappeared();

            Step("6. Expected Installation wizard appears with title \"New Streetlight\" and streetlight icon");
            Step(" + Identifier field is automatically filled based on streetlight's location on map");
            Step(" + \"Lamp is accessible\" is \"Yes\"");
            installationPage.WaitForPreviousActionComplete();
            installationPage.WaitForPopupDialogDisplayed();
            VerifyEqual("6. Verify title 'New Streetlight' is correct", "New Streetlight", installationPage.InstallationPopupPanel.GetPanelTitleText());
            var identifier = installationPage.InstallationPopupPanel.GetIdentifierValue();
            var streetlightName = installationPage.InstallationPopupPanel.GetNameValue();            
            var curLampIsAccessible = installationPage.InstallationPopupPanel.GetLampIsAccessibleValue();            
            VerifyEqual("6. Verify 'Lamp is accessible' is automatically filled with 'Yes'", "Yes", curLampIsAccessible);
            VerifySLV1926ForInstallation(installationPage);

            Step("7. Leave \"Type of equipment\" field empty then click Next");
            installationPage.InstallationPopupPanel.ClickNextButton();
            installationPage.Dialog.WaitForPopupMessageDisplayed();

            Step("8. Expected Error message \"Please enter 'Type of equipment' property.\" is shown");
            var expectedErrorMsg = "Please enter 'Type of equipment' property.";
            VerifyEqual(string.Format("8. Verify Error message is: '{0}''", expectedErrorMsg), expectedErrorMsg, installationPage.Dialog.GetMessageText());
            installationPage.Dialog.ClickOkButton();
            installationPage.Dialog.WaitForPopupMessageDisappeared();

            Step("9. Fill Identifier and \"Type of equipment\" fields, leave Controller ID field empty then click Next");
            installationPage.InstallationPopupPanel.SelectTypeOfEquipmentDropDown(inputTypeOfEquipment);
            installationPage.InstallationPopupPanel.ClickNextButton();
            installationPage.Dialog.WaitForPopupMessageDisplayed();

            Step("10. Expected Error message \"Please enter 'Controller ID' property.\" is shown");
            expectedErrorMsg = "Please enter 'Controller ID' property.";
            VerifyEqual(string.Format("10. Verify Error message is: '{0}''", expectedErrorMsg), expectedErrorMsg, installationPage.Dialog.GetMessageText());
            installationPage.Dialog.ClickOkButton();
            installationPage.Dialog.WaitForPopupMessageDisappeared();

            Step("11. Fill all required fields (Type of equipment, Controller ID) then click Next");
            installationPage.InstallationPopupPanel.SelectControllerIdDropDown(inputControllerName);
            installationPage.InstallationPopupPanel.ClickNextButton();

            Step("12. Expected Next screen appears to ask for Unique address and NIC serial number");
            installationPage.InstallationPopupPanel.WaitForScanQRCodeFormDisplayed();

            Step("13. Enter value into Unique address and NIC serial number field then click Next");
            installationPage.InstallationPopupPanel.EnterUniqueAddressInput(inputUniqueAddress);
            installationPage.InstallationPopupPanel.EnterNicSerialNumberInput(inputNicSerialNumber);
            installationPage.InstallationPopupPanel.ClickNextButton();

            Step("14. Check \"Lamp came on\" then click Next");
            installationPage.InstallationPopupPanel.WaitForLightComeOnFormDisplayed();
            installationPage.InstallationPopupPanel.TickLampCameOnCheckbox(inputLampCameOn);
            installationPage.InstallationPopupPanel.ClickNextButton();

            Step("15. Expected Next screen appears for device details");
            installationPage.InstallationPopupPanel.WaitForMoreInfoFormDisplayed();

            Step("16. Enter values into fields then click Next");
            installationPage.InstallationPopupPanel.SelectLampTypeDropDown("HPS 70W Ferro");
            installationPage.InstallationPopupPanel.EnterLampWattageNumericInput("10");
            installationPage.InstallationPopupPanel.SelectLuminaireTypeDropDown("Any");
            installationPage.InstallationPopupPanel.SelectLuminaireModelDropDown("Any");
            installationPage.InstallationPopupPanel.SelectLightDistributionDropDown("Any");
            installationPage.InstallationPopupPanel.SelectOrientationDropDown("Any");
            installationPage.InstallationPopupPanel.SelectBracketLengthDropDown("10");
            installationPage.InstallationPopupPanel.SelectPoleTypeDropDown("Any");
            installationPage.InstallationPopupPanel.ClickNextButton();

            Step("17. Expected Next screen appears to ask for a photo");
            installationPage.InstallationPopupPanel.WaitForPhotoFormDisplayed();
            VerifyPhotoForm(installationPage);

            //Run Test Form
            installationPage.InstallationPopupPanel.ClickNextButton();
            installationPage.InstallationPopupPanel.WaitForRunTestFormDisplayed();

            Step("18. Click Next");
            installationPage.InstallationPopupPanel.ClickNextButton();

            Step("19. Expected Next screen appears to ask for a comment");
            installationPage.InstallationPopupPanel.WaitForCommentFormDisplayed();

            Step("20. Enter a comment then click Next");
            installationPage.InstallationPopupPanel.EnterCommentInput("test comments");
            installationPage.InstallationPopupPanel.ClickNextButton();

            Step("21. Expected Finish screen appears");
            installationPage.InstallationPopupPanel.WaitForFinishFormDisplayed();
            VerifyFinishForm(installationPage, "Thank you, you can go to the next light point.");

            Step("22. Click Finish button");
            installationPage.InstallationPopupPanel.ClickFinishButton();
            installationPage.WaitForPreviousActionComplete();
            installationPage.WaitForPopupDialogDisappeared();

            Step("23. Expected No error displays. The device on the map is turned into green (Installed status)");                   
            VerifyBackgroundIcon(installationPage, streetlightName, BgColor.Green);

            Info("TS 2.23.1");
            Step("1. Click Add new streetlight and place it on the map");
            installationPage.Map.ClickAddStreetlightButton();
            installationPage.Map.WaitForRecorderDisplayed();
            installationPage.Map.PositionNewDevice();
            installationPage.Map.WaitForRecorderDisappeared();
            installationPage.WaitForPreviousActionComplete();
            installationPage.WaitForPopupDialogDisplayed();

            Step("2. Expected Type of equipment, Controller ID are copied from previously installed streetlight");
            var curTypeEquipment = installationPage.InstallationPopupPanel.GetTypeOfEquipmentValue();
            var curControllerId = installationPage.InstallationPopupPanel.GetControllerIdValue();
            VerifyEqual(string.Format("[2.23.1] 2. Verify Type of equipment is copied from previously installed streetlight and equal '{0}'", curTypeEquipment), inputTypeOfEquipment, curTypeEquipment);
            VerifyEqual(string.Format("[2.23.1] 2. Verify Controller ID is copied from previously installed streetlight '{0}'", curControllerId), inputControllerName, curControllerId);          

            try
            {
                DeleteDevice(inputControllerId, identifier);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("TS 2.22.2 Installation - Install streetlight with No")]
        [NonParallelizable]
        public void TS2_22_02()
        {
            var testData = GetTestDataOfTestTS2_22_02();
            var inputGeozone = testData["Geozone"].ToString();
            var inputLampIsAccessible = testData["LampIsAccessible"].ToString();
            var inputReason = testData["Reason"].ToString();
            var controller = testData["Controller"] as DeviceModel;
            var streetlights = testData["Streetlights"] as List<DeviceModel>;
            var streetlight = streetlights.PickRandom();
            var streetlightName = streetlight.Name;

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - Location service is enabled on browser");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.EquipmentInventory, App.Installation);

            Step("1. Go to Installation app");
            Step("2. Expected Installation page is routed and loaded successfully");
            var installationPage = desktopPage.GoToApp(App.Installation) as InstallationPage;

            Step("3. Click on a real Streetligh device");
            installationPage.GeozoneTreeMainPanel.SelectNode(inputGeozone + @"\" + streetlightName);

            Step("4. Expected A dialog with details are shown");
            installationPage.WaitForPopupDialogDisplayed();
            VerifyEqual(string.Format("4. Verify title '{0}' is correct", streetlightName), streetlightName, installationPage.InstallationPopupPanel.GetPanelTitleText());

            Step("5. \"Lamp is accessible\" is set to \"No\" then click Next");
            installationPage.InstallationPopupPanel.SelectLampIsAccessibleDropDown(inputLampIsAccessible);
            var currentIdentifier = installationPage.InstallationPopupPanel.GetIdentifierValue();
            var currentTypeEquipment = installationPage.InstallationPopupPanel.GetTypeOfEquipmentValue();
            var currentController = installationPage.InstallationPopupPanel.GetControllerIdValue();
            var currentAddress1 = installationPage.InstallationPopupPanel.GetAddress1Value();
            var currentAddress2 = installationPage.InstallationPopupPanel.GetAddress2Value();
            var currentCity = installationPage.InstallationPopupPanel.GetCityValue();
            var currentZipcode = installationPage.InstallationPopupPanel.GetZipcodeValue();
            var currentLampIsAccessible = installationPage.InstallationPopupPanel.GetLampIsAccessibleValue();
            installationPage.InstallationPopupPanel.ClickNextButton();

            Step("6. Expected Next step is displayed to ask for reason \"Why can't it be installed ?\"");
            installationPage.InstallationPopupPanel.WaitForReasonFormDisplayed();
            VerifyReasonForm(installationPage, "Why can't it be installed ?");

            Step("7. Select a reason then click Back icon of top-left corner");
            installationPage.InstallationPopupPanel.SelectReasonDropDown(inputReason);
            installationPage.InstallationPopupPanel.ClickBackButton();

            Step("8. Expected Changes on previous step are still kept");
            installationPage.InstallationPopupPanel.WaitForBasicInfoFormDisplayed();
            VerifyBasicInfoForm(installationPage, currentController, currentIdentifier, streetlightName, currentAddress1, currentAddress2, currentCity, currentZipcode, currentLampIsAccessible);

            Step("9. Click Next button");
            installationPage.InstallationPopupPanel.ClickNextButton();

            Step("10. Changes on next step are still kept");
            installationPage.InstallationPopupPanel.WaitForReasonFormDisplayed();
            VerifyEqual(string.Format("10. Verify Reason is '{0}'", inputReason), inputReason, installationPage.InstallationPopupPanel.GetReasonValue());

            Step("11. Click Next button");
            installationPage.InstallationPopupPanel.ClickNextButton();

            Step("12. Next step is displayed to test the street light");
            installationPage.InstallationPopupPanel.WaitForRunTestFormDisplayed();
            VerifyRunTestForm(installationPage);

            Step("13. Click RUN TEST button");
            installationPage.InstallationPopupPanel.ClickRunTestButton();
            installationPage.InstallationPopupPanel.WaitForRunTestCompleted();

            Step("14. Expected Test runs ok");
            var expectedTestInfo = "Successful test !";
            VerifyEqual(string.Format("14. Verify test information is {0}", expectedTestInfo), expectedTestInfo, installationPage.InstallationPopupPanel.GetInfoMessageText());

            Step("15. Next step is displayed to ask for a comment");
            installationPage.InstallationPopupPanel.ClickNextButton();
            installationPage.InstallationPopupPanel.WaitForCommentFormDisplayed();
            VerifyCommentForm(installationPage);

            Step("16. Click Next button");
            installationPage.InstallationPopupPanel.ClickNextButton();

            Step("17. Expected Finish screen appears");
            installationPage.InstallationPopupPanel.WaitForFinishFormDisplayed();
            VerifyFinishForm(installationPage, "Thank you, you can go to the next light point.");

            Step("18. Click Finish button");
            installationPage.InstallationPopupPanel.ClickFinishButton();
            installationPage.WaitForPreviousActionComplete();
            installationPage.WaitForPopupDialogDisappeared();

            Step("19. Expected No error displays. The device on the map is turned into orange (Installed status)");           
            VerifyBackgroundIcon(installationPage, streetlightName, BgColor.Orange);
            
            try
            {
                // Set back current device to normal status
                SetValueToDevice(controller.Id, streetlight.Id, "installStatus", "-", Settings.GetCurrentControlerDateTime(controller.Id).AddMinutes(10));
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("TS 2.22.3 Installation - Install streetlight with Can't verify")]
        [NonParallelizable]
        public void TS2_22_03()
        {
            var testData = GetTestDataOfTestTS2_22_03();
            var inputGeozone = testData["Geozone"].ToString();
            var inputLampIsAccessible = testData["LampIsAccessible"].ToString();
            var inputReason = testData["Reason"].ToString();
            var controller = testData["Controller"] as DeviceModel;
            var streetlights = testData["Streetlights"] as List<DeviceModel>;
            var streetlight = streetlights.PickRandom();
            var streetlightName = streetlight.Name;

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - Location service is enabled on browser");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.EquipmentInventory, App.Installation);

            Step("1. Go to Installation app");
            Step("2. Expected Installation page is routed and loaded successfully");
            var installationPage = desktopPage.GoToApp(App.Installation) as InstallationPage;

            Step("3. Click on a real Streetligh device");
            installationPage.GeozoneTreeMainPanel.SelectNode(inputGeozone + @"\" + streetlightName);

            Step("4. Expected A dialog with details are shown");
            installationPage.WaitForPopupDialogDisplayed();
            VerifyEqual(string.Format("4. Verify title '{0}' is correct", streetlightName), streetlightName, installationPage.InstallationPopupPanel.GetPanelTitleText());

            Step("5. \"Lamp is accessible\" is set to \"Can't verify\" then click Next");
            installationPage.InstallationPopupPanel.SelectLampIsAccessibleDropDown(inputLampIsAccessible);
            var currentIdentifier = installationPage.InstallationPopupPanel.GetIdentifierValue();
            var currentTypeEquipment = installationPage.InstallationPopupPanel.GetTypeOfEquipmentValue();
            var currentController = installationPage.InstallationPopupPanel.GetControllerIdValue();
            var currentAddress1 = installationPage.InstallationPopupPanel.GetAddress1Value();
            var currentAddress2 = installationPage.InstallationPopupPanel.GetAddress2Value();
            var currentCity = installationPage.InstallationPopupPanel.GetCityValue();
            var currentZipcode = installationPage.InstallationPopupPanel.GetZipcodeValue();
            var currentLampIsAccessible = installationPage.InstallationPopupPanel.GetLampIsAccessibleValue();
            installationPage.InstallationPopupPanel.ClickNextButton();

            Step("6. Expected Next step is displayed to ask for reason \"Why can't it be installed ?\"");
            installationPage.InstallationPopupPanel.WaitForReasonFormDisplayed();
            VerifyReasonForm(installationPage, "Why can't it be installed ?");

            Step("7. Select a reason then click Back icon of top-left corner");
            installationPage.InstallationPopupPanel.SelectReasonDropDown(inputReason);
            installationPage.InstallationPopupPanel.ClickBackButton();

            Step("8. Expected Changes on previous step are still kept");
            installationPage.InstallationPopupPanel.WaitForBasicInfoFormDisplayed();
            VerifyBasicInfoForm(installationPage, currentController, currentIdentifier, streetlightName, currentAddress1, currentAddress2, currentCity, currentZipcode, currentLampIsAccessible);

            Step("9. Click Next button");
            installationPage.InstallationPopupPanel.ClickNextButton();

            Step("10. Changes on next step are still kept");
            installationPage.InstallationPopupPanel.WaitForReasonFormDisplayed();
            VerifyEqual(string.Format("10. Verify Reason is '{0}'", inputReason), inputReason, installationPage.InstallationPopupPanel.GetReasonValue());

            Step("11. Click Next button");
            installationPage.InstallationPopupPanel.ClickNextButton();

            Step("12. Next step is displayed to test the street light");
            installationPage.InstallationPopupPanel.WaitForRunTestFormDisplayed();
            VerifyRunTestForm(installationPage);

            Step("13. Click RUN TEST button");
            installationPage.InstallationPopupPanel.ClickRunTestButton();
            installationPage.InstallationPopupPanel.WaitForRunTestCompleted();

            Step("14. Expected Test runs ok");
            var expectedTestInfo = "Successful test !";
            VerifyEqual(string.Format("14. Verify test information is {0}", expectedTestInfo), expectedTestInfo, installationPage.InstallationPopupPanel.GetRunTestMessage());

            Step("15. Next step is displayed to ask for a comment");
            installationPage.InstallationPopupPanel.ClickNextButton();
            installationPage.InstallationPopupPanel.WaitForCommentFormDisplayed();
            VerifyCommentForm(installationPage);

            Step("16. Click Next button");
            installationPage.InstallationPopupPanel.ClickNextButton();

            Step("17. Expected Finish screen appears");
            installationPage.InstallationPopupPanel.WaitForFinishFormDisplayed();
            VerifyFinishForm(installationPage, "Thank you, you can go to the next light point.");

            Step("18. Click Finish button");
            installationPage.InstallationPopupPanel.ClickFinishButton();
            installationPage.WaitForPreviousActionComplete();
            installationPage.WaitForPopupDialogDisappeared();

            Step("19. Expected No error displays. The device on the map is turned into orange (Installed status)");       
            VerifyBackgroundIcon(installationPage, streetlightName, BgColor.Orange);

            try
            {
                // Set back current device to normal status
                SetValueToDevice(controller.Id, streetlight.Id, "installStatus", "-", Settings.GetCurrentControlerDateTime(controller.Id).AddMinutes(10));
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("TS 2.23.1 Installation - Install second device")]
        public void TS2_23_01()
        {
            Assert.Pass("This test is included in TS 2.22.1 <-- see it for details");
        }

        [Test, DynamicRetry]
        [Description("TS 2.25.1 Advanced Search - create and export new searches")]
        [NonParallelizable]
        public void TS2_25_01()
        {
            //Read xml input test data
            var testData = GetTestDataOfTestTS2_25_01();
            var xmlGeoZones = testData["GeoZones"] as List<string>;
            var xmlSearchNamePrefix = testData["SearchName"].ToString();
            var xmlExportFilePattern = testData["ExportFilePattern"].ToString();

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.AdvancedSearch);

            Step("1. Go to AdvancedSearch app");
            Step("2. Expected AdvancedSearch page is routed and loaded successfully");
            var advancedSearchPage = desktopPage.GoToApp(App.AdvancedSearch) as AdvancedSearchPage;
            advancedSearchPage.WaitForSearchWizardPopupPanelDisplayed();

            var listSearch = new List<string>();
            var geoZoneCount = xmlGeoZones.Count;
            for (int i = 0; i < geoZoneCount; i++)
            {
                var geoZonePath = xmlGeoZones[i];
                var geoZoneName = geoZonePath.GetChildName();
                var searchName = SLVHelper.GenerateUniqueName(xmlSearchNamePrefix);
                listSearch.Add(searchName);
                Info(string.Format("-> {0}", geoZonePath));

                Step("3. Click \"New advanced search\"");
                advancedSearchPage.SearchWizardPopupPanel.ClickNewAdvancedSearchButton();

                Step("4. Expected Next step is moved with button \"Select a saved searched\" and Search icon. Next button is not present");
                advancedSearchPage.SearchWizardPopupPanel.WaitForNewSearchNameInputVisible();
                VerifyEqual("4. Verify Next button is invisible", false, advancedSearchPage.SearchWizardPopupPanel.IsNextButtonVisible());

                Step("5. Enter a new search name (e.g. \"Search 1\")");
                advancedSearchPage.SearchWizardPopupPanel.EnterNewSearchNameInput(searchName);

                Step("6. Expected Next button is present");
                VerifyEqual("6. Verify Next button is invisible", true, advancedSearchPage.SearchWizardPopupPanel.IsNextButtonVisible());
                advancedSearchPage.SearchWizardPopupPanel.ClickNextButton();
                advancedSearchPage.SearchWizardPopupPanel.WaitForGeozoneFormDisplayed();

                Step("7. Select a geozone then click Next");
                advancedSearchPage.SearchWizardPopupPanel.GeozoneTreePopupPanel.SelectNode(geoZonePath);
                var expectedDevices = advancedSearchPage.SearchWizardPopupPanel.GeozoneTreePopupPanel.GetSelectedNodeDevicesCount();
                advancedSearchPage.SearchWizardPopupPanel.ClickNextButton();
                advancedSearchPage.SearchWizardPopupPanel.WaitForAttributeFormDisplayed();

                Step("8. Expected Next step is moved with label \"Select the attributes to display in the search results\" and list of attributes panel");
                var expectedAttrLabel = "Select the attributes to display in the search results";
                VerifyEqual(string.Format("8. Verify Attribute columns label is '{0}'", expectedAttrLabel), expectedAttrLabel, advancedSearchPage.SearchWizardPopupPanel.GetSelectAttributeCaptionText());

                Step("9. Select attributes Lamp Type, Dimming Group Name and Install Status (others are left default) then click Next (Lamp Type may be not present because of SLV-1256)");
                var checkedList = new List<string>() { "Lamp Type", "Dimming group", "Install status" };
                advancedSearchPage.SearchWizardPopupPanel.CheckAttributeList(checkedList.ToArray());
                advancedSearchPage.SearchWizardPopupPanel.ClickNextButton();
                advancedSearchPage.SearchWizardPopupPanel.WaitForFilterFormDisplayed();

                Step("10. Expected Next step is moved with label \"Filter your results by adding search criteria\" and Filter panel");
                var expectedFiltersLabel = "Filter your results by adding search criteria";
                VerifyEqual(string.Format("10. Verify Filters label is '{0}'", expectedFiltersLabel), expectedFiltersLabel, advancedSearchPage.SearchWizardPopupPanel.GetFilterCaptionText());

                Step("11. Click Next");
                Step("12. Expected Next step is moved with label {{number of selected geozone's devices}} devices match your search criteria. Click on Finish to see the results");
                var expectedMessage = string.Format("{0} devices match your search criteria.", expectedDevices);
                advancedSearchPage.SearchWizardPopupPanel.ClickNextButton();
                advancedSearchPage.SearchWizardPopupPanel.WaitForFinishFormDisplayed();
                advancedSearchPage.SearchWizardPopupPanel.WaitForDeviceSearchCompleted();
                var actualMessage = advancedSearchPage.SearchWizardPopupPanel.GetCriteriaMessageText();
                VerifyEqual(string.Format("12. Verify search result message is: '{0}'", expectedMessage), expectedMessage, actualMessage);
                VerifyEqual("12. Verify Next button is invisible", false, advancedSearchPage.SearchWizardPopupPanel.IsNextButtonVisible());
                VerifyEqual("12. Verify Finish button is visible", true, advancedSearchPage.SearchWizardPopupPanel.IsFinishButtonVisible());

                Step("13. Click Finish");
                advancedSearchPage.SearchWizardPopupPanel.ClickFinishButton();
                advancedSearchPage.WaitForSearchWizardPopupPanelDisappeared();
                advancedSearchPage.WaitForPreviousActionComplete();
                advancedSearchPage.GridPanel.WaitForPanelLoaded();

                Step("14. Expected The popup disappears. A new search is created:");
                Step(" o The grid displays devices of selected geozone with selected attributes");
                Step(" o Search name appears and is selected in search list");
                var actualSearchName = advancedSearchPage.GridPanel.GetSelectOrAddSearchValue();
                VerifyEqual(string.Format("14. Verify Search Name '{0}' appears and is selected in search list", searchName), searchName, actualSearchName);
                var footerLeftText = advancedSearchPage.GridPanel.GetFooterLeftText();
                var match = new Regex(@"Generated in (.*?) seconds").Match(footerLeftText);
                Step("* Total time of search '{0}' is {1} seconds", geoZoneName, match.Groups[1].Value);

                Step("15. Click Export");
                SLVHelper.DeleteAllFilesByPattern(xmlExportFilePattern);
                advancedSearchPage.GridPanel.ClickGenerateCSVToolbarButton();
                advancedSearchPage.WaitForPreviousActionComplete();
                advancedSearchPage.GridPanel.ClickDownloadToolbarButton();
                SLVHelper.SaveDownloads();                                
                VerifyEqual("[SLV-2492] 15. Verify The requested resource is not available.", false, advancedSearchPage.IsHttp404Existing());

                Step("16. Expected A CSV file is downloaded. Its content reflects displayed data");
                var tblGrid = advancedSearchPage.GridPanel.BuildDataTableFromGrid();
                int recordsOfPage = 200;
                if (expectedDevices > recordsOfPage)
                {
                    var page = expectedDevices / recordsOfPage;
                    for (int j = 0; j < page; j++)
                    {
                        advancedSearchPage.GridPanel.ClickFooterPageNextButton();
                        advancedSearchPage.GridPanel.WaitForLeftFooterTextDisplayed();
                        var pageTable = advancedSearchPage.GridPanel.BuildDataTableFromGrid();
                        tblGrid.Merge(pageTable);
                    }
                }

                var tblCSV = SLVHelper.BuildDataTableFromLastDownloadedCSV(xmlExportFilePattern);
                var tblGridFormatted = tblGrid.Copy();
                var tblCSVFormatted = tblCSV.Copy();
                VerifyEqual("16. Verify Lamp Type exists in search grid", true, tblGrid.Columns.Contains("Lamp Type"));
                VerifyEqual("16. Verify Dimming Group Name exists in search grid", true, tblGrid.Columns.Contains("Dimming group"));
                VerifyEqual("16. Verify Install Status exists in search grid", true, tblGrid.Columns.Contains("Install status"));

                FormatAdvancedSearchDataTables(ref tblGridFormatted, ref tblCSVFormatted);
                VerifyEqual(string.Format("[{0}] 16. Verify exported CSV file reflects what is being shown in the grid", geoZoneName), tblGridFormatted, tblCSVFormatted);

                Step("17. Repeat other advanced searches with 3 other geozones");
                Step("18. Expected The same with previous search");
                Step("19. Remember to log total time of each complete search and export");
                advancedSearchPage.GridPanel.ClickEditButton();
                advancedSearchPage.WaitForSearchWizardPopupPanelDisplayed();
            }

            try
            {
                //Remove all search
                advancedSearchPage.SearchWizardPopupPanel.ClickCloseButton();
                advancedSearchPage.WaitForSearchWizardPopupPanelDisappeared();
                foreach (var search in listSearch)
                {
                    advancedSearchPage.GridPanel.DeleleRequest(search);
                }
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("SLV-2019 Review pagination on grid component")]
        public void SLV_2019()
        {            
            var testData = GetTestDataOfTestSLV_2019();
            var xmlRowsPerPage = testData["RowsPerPage"];
            var rootGeozone = Settings.RootGeozoneName;

            Step("**** Precondition ****");
            Step(" - User has logged in successfully A geozone with more than 200 devices");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenBackOfficeApp();
            var backOfficePage = loginPage.LoginAsValidUserToBackOffice(Settings.Users["admin"].Username, Settings.Users["admin"].Password);
            backOfficePage.BackOfficeOptionsPanel.SelectConfiguration(App.EquipmentInventory);
            var firstDeviceLocationValue = backOfficePage.BackOfficeDetailsPanel.GetEquipmentDeviceLocationValue();
            backOfficePage.BackOfficeDetailsPanel.EnterEquipmentRowsPerPageNumericInput(xmlRowsPerPage);
            backOfficePage.BackOfficeDetailsPanel.ClickSaveButton();
            backOfficePage.WaitForPreviousActionComplete();

            Step("1. Go to Equipment Inventory app");
            Step("2. Expected Equipment Inventory page is routed and loaded successfully");
            var desktopPage = Browser.NavigateToLoggedInCMS();
            desktopPage.InstallAppsIfNotExist(App.EquipmentInventory);
            var equipmentInventoryPage = desktopPage.GoToApp(App.EquipmentInventory) as EquipmentInventoryPage;

            Step("3. Select the root geozone");
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(rootGeozone);

            Step("4. Expected Geozone details panel appears");
            equipmentInventoryPage.WaitForEditorPanelDisplayed();

            Step("5. Click Custom report button at the bottom of the panel");
            equipmentInventoryPage.GeozoneEditorPanel.ClickCustomReportButton();
            equipmentInventoryPage.WaitForPreviousActionComplete();
            equipmentInventoryPage.WaitForCustomReportDisplayed();

            Step("6. Expected Custom report panel appears:");
            Step(" + 200 first devices are shown in the grid");
            Step(" + Page number is 1");
            equipmentInventoryPage.GridPanel.WaitForGridContentAvailable();
            var actualCustomReportTitle = equipmentInventoryPage.GridPanel.GetPanelTitleText();
            VerifyEqual(string.Format("6. Verify Custom report title is '{0}'", rootGeozone), rootGeozone, actualCustomReportTitle);

            var currentPage = equipmentInventoryPage.GridPanel.GetFooterPageIndexValue();
            var gridTable = equipmentInventoryPage.GridPanel.BuildDataTableFromGrid();
            VerifyEqual("6. Verify 200 first devices are shown in the grid", xmlRowsPerPage, gridTable.Rows.Count.ToString());
            VerifyEqual("6. Verify Page number is 1", "1", currentPage);

            Step("7. Click Next to go to Page 2");
            equipmentInventoryPage.GridPanel.ClickFooterPageNextButton();
            equipmentInventoryPage.GridPanel.WaitForLeftFooterTextDisplayed();

            Step("8. Expected Custom report panel appears:");
            Step(" + 200 first devices are shown in the grid");
            Step(" + Page number is 2");
            currentPage = equipmentInventoryPage.GridPanel.GetFooterPageIndexValue();
            gridTable = equipmentInventoryPage.GridPanel.BuildDataTableFromGrid();
            VerifyEqual("8. Verify 200 first devices are shown in the grid", xmlRowsPerPage, gridTable.Rows.Count.ToString());
            VerifyEqual("8. Verify Page number is 2", "2", currentPage);

            Step("9. Click Reload");
            Step("10. Expected Verify the page number is still 2 and 200 devices of page 2 is still shown(According to SLV-2019, after clicking Reload button, 200 devices of page 2 is still displayed but page number is changed to 1)");
            equipmentInventoryPage.GridPanel.ClickReloadDataToolbarButton();
            equipmentInventoryPage.GridPanel.WaitForLeftFooterTextDisplayed();
            currentPage = equipmentInventoryPage.GridPanel.GetFooterPageIndexValue();
            gridTable = equipmentInventoryPage.GridPanel.BuildDataTableFromGrid();
            VerifyTrue("10. Verify the page number is still 2 and 200 devices of page 2 is still shown", gridTable.Rows.Count.ToString() == xmlRowsPerPage && currentPage.Equals("2"), string.Format("Count:{0}, Page:{1}", xmlRowsPerPage, 2), string.Format("Count:{0}, Page:{1}", gridTable.Rows.Count, currentPage));
        }

        #endregion //Test Cases

        #region Private methods

        /// <summary>
        /// Make sure 2 compared tables proper formatted before compared
        /// </summary>
        /// <param name="gridDataTable"></param>
        /// <param name="csvDataTable"></param>
        private void FormatAdvancedSearchDataTables(ref DataTable gridDataTable, ref DataTable csvDataTable)
        {
            if (csvDataTable.Columns.Contains("id"))
            {
                csvDataTable.Columns.Remove("id");
            }

            //Ignore checking Lamp Type column because of data is different
            if (gridDataTable.Columns.Contains("Lamp Type"))
            {
                gridDataTable.Columns.Remove("Lamp Type");
            }

            //Ignore checking Lamp Type column because of data is different
            if (csvDataTable.Columns.Contains("brandId"))
            {
                csvDataTable.Columns.Remove("brandId");
            }

            foreach (DataColumn col in csvDataTable.Columns)
            {
                if (col.ColumnName == "name")
                {
                    csvDataTable.Columns["name"].ColumnName = "Device";
                }
                else if (col.ColumnName == "address")
                {
                    csvDataTable.Columns["address"].ColumnName = "Address 1";
                }
                else if (col.ColumnName == "MacAddress")
                {
                    csvDataTable.Columns["MacAddress"].ColumnName = "Unique address";
                }
                else if (col.ColumnName == "DimmingGroupName")
                {
                    csvDataTable.Columns["DimmingGroupName"].ColumnName = "Dimming group";
                }
                else if (col.ColumnName == "installStatus")
                {
                    csvDataTable.Columns["installStatus"].ColumnName = "Install status";
                }
                else if (col.ColumnName == "ConfigStatus")
                {
                    csvDataTable.Columns["ConfigStatus"].ColumnName = "Configuration status";
                }
            }

            foreach (DataColumn col in gridDataTable.Columns)
            {
                foreach (DataRow row in gridDataTable.Rows)
                {
                    if (row[col.ColumnName].ToString() == "---")
                        row[col.ColumnName] = string.Empty;
                }
            }
        }

        #region Verify methods

        #region Inventory Verification

        /// <summary>
        /// Verify SLV-1926
        /// </summary>
        private void VerifySLV1926ForVerification(InventoryVerificationPage page)
        {
            var expectedControllerID = "Controller ID *";
            var expectedIdentifier = "Identifier *";
            var expectedName = "Name *";
            var expectedAddress1 = "Address 1";
            var expectedAddress2 = "Address 2";
            var expectedCity = "City";
            var expectedZipCode = "Zip code";
            var expectedIsLampPositionCorrect = "Is lamp position correct ?";

            Step("-> Checking SLV-1926: Inventory Verification and Installation - Add Name field");
            //- Verify in the first page of the wizard, following fields are present: (SLV-1926)
            // + Controller ID * 
            // + Identifier * 
            // + Name * 
            // + Address 1 
            // + Address 2 
            // + City 
            // + Zip code 
            // + Is lamp position correct ? 

            var actualControllerID = page.VerificationPopupPanel.GetControllerIdText();
            var actualIdentifier = page.VerificationPopupPanel.GetIdentifierText();
            var actualName = page.VerificationPopupPanel.GetNameText();
            var actualAddress1 = page.VerificationPopupPanel.GetAddress1Text();
            var actualAddress2 = page.VerificationPopupPanel.GetAddress2Text();
            var actualCity = page.VerificationPopupPanel.GetCityText();
            var actualZipCode = page.VerificationPopupPanel.GetZipcodeText();
            var actualIsLampPositionCorrect = page.VerificationPopupPanel.GetIsLampPositionCorrectText();

            VerifyTrue("Verify 'Controller ID' is displayed", expectedControllerID.Equals(actualControllerID), "Displayed", "Not Display");
            VerifyTrue("Verify 'Identifier' is displayed", expectedIdentifier.Equals(actualIdentifier), "Displayed", "Not Display");
            VerifyTrue("Verify 'Name' is displayed", expectedName.Equals(actualName), "Displayed", "Not Display");
            VerifyTrue("Verify 'Address 1' is displayed", expectedAddress1.Equals(actualAddress1), "Displayed", "Not Display");
            VerifyTrue("Verify 'Address 2' is displayed", expectedAddress2.Equals(actualAddress2), "Displayed", "Not Display");
            VerifyTrue("Verify 'City' is displayed", expectedCity.Equals(actualCity), "Displayed", "Not Display");
            VerifyTrue("Verify 'Zip code' is displayed", expectedZipCode.Equals(actualZipCode), "Displayed", "Not Display");
            VerifyTrue("Verify 'Is lamp position correct ?' is displayed", expectedIsLampPositionCorrect.Equals(actualIsLampPositionCorrect), "Displayed", "Not Display");

            //- Verify all fields are editable
            VerifyTrue("Verify 'Controller ID' field should be editable", page.VerificationPopupPanel.IsControllerIdEditable(), "Should be editable", "Read-only");
            VerifyTrue("Verify 'Identifier' field should be editable", page.VerificationPopupPanel.IsIdentifierEditable(), "Should be editable", "Read-only");
            VerifyTrue("Verify 'Name' field should be editable", page.VerificationPopupPanel.IsNameEditable(), "Should be editable", "Read-only");
            VerifyTrue("Verify 'Address 1' field should be editable", page.VerificationPopupPanel.IsAddress1Editable(), "Should be editable", "Read-only");
            VerifyTrue("Verify 'Address 2' field should be editable", page.VerificationPopupPanel.IsAddress2Editable(), "Should be editable", "Read-only");
            VerifyTrue("Verify 'City' field should be editable", page.VerificationPopupPanel.IsCityEditable(), "Should be editable", "Read-only");
            VerifyTrue("Verify 'Zip code' field should be editable", page.VerificationPopupPanel.IsZipcodeEditable(), "Should be editable", "Read-only");
            VerifyTrue("Verify 'Is lamp position correct ?' field should be editable", page.VerificationPopupPanel.IsLampPositionCorrectEditable(), "Should be editable", "Read-only");
        }

        /// <summary>
        /// Verify basic infos form
        /// </summary>
        /// <param name="page"></param>
        /// <param name="expectedController"></param>
        /// <param name="expectedIdentifier"></param>
        /// <param name="expectedName"></param>
        /// <param name="expectedAddress1"></param>
        /// <param name="expectedAddress2"></param>
        /// <param name="expectedCity"></param>
        /// <param name="expectedZipcode"></param>
        /// <param name="expectedIsLampPositionCorrect"></param>
        private void VerifyBasicInfoForm(InventoryVerificationPage page, string expectedController, string expectedIdentifier, string expectedName, string expectedAddress1, string expectedAddress2, string expectedCity, string expectedZipcode, string expectedIsLampPositionCorrect)
        {
            var actualController = page.VerificationPopupPanel.GetControllerIdValue();
            var actualIdentifier = page.VerificationPopupPanel.GetIdentifierValue();
            var actualName = page.VerificationPopupPanel.GetNameValue();
            var actualAddress1 = page.VerificationPopupPanel.GetAddress1Value();
            var actualAddress2 = page.VerificationPopupPanel.GetAddress2Value();
            var actualCity = page.VerificationPopupPanel.GetCityValue();
            var actualZipcode = page.VerificationPopupPanel.GetZipcodeValue();
            var actualIsLampPositionCorrect = page.VerificationPopupPanel.GetIsLampPositionCorrectValue();

            VerifyEqual(string.Format("Verify expectedController is '{0}'", expectedController), expectedController, actualController);
            VerifyEqual(string.Format("Verify Identifier is '{0}'", expectedIdentifier), expectedIdentifier, actualIdentifier);
            VerifyEqual(string.Format("Verify Name is '{0}'", expectedName), expectedName, actualName);
            VerifyEqual(string.Format("Verify Address 1 is '{0}'", expectedAddress1), expectedAddress1, actualAddress1);
            VerifyEqual(string.Format("Verify Address 2 is '{0}'", expectedAddress2), expectedAddress2, actualAddress2);
            VerifyEqual(string.Format("Verify City is '{0}'", expectedCity), expectedCity, actualCity);
            VerifyEqual(string.Format("Verify Zipcode is '{0}'", expectedZipcode), expectedZipcode, actualZipcode);
            VerifyEqual(string.Format("Verify Is Lamp Position Correct is '{0}'", expectedIsLampPositionCorrect), expectedIsLampPositionCorrect, actualIsLampPositionCorrect);
        }

        /// <summary>
        /// Verify more infos form
        /// </summary>
        /// <param name="page"></param>
        /// <param name="expectedIsFunctionning"></param>
        /// <param name="expectedIsAcorn"></param>
        /// <param name="expectedComment"></param>
        private void VerifyMoreInfoForm(InventoryVerificationPage page, bool expectedIsFunctionning, bool expectedIsAcorn, string expectedComment)
        {
            var actualIsFunctionning = page.VerificationPopupPanel.GetIsLampFunctionningValue();
            var actualIsAcorn = page.VerificationPopupPanel.GetIsLampAcornValue();
            var actualComment = page.VerificationPopupPanel.GetCommentValue();

            VerifyEqual(string.Format("Verify Lamp Is Functionning is '{0}'", expectedIsFunctionning), expectedIsFunctionning, actualIsFunctionning);
            VerifyEqual(string.Format("Verify Lamp Is Acorn is '{0}'", expectedIsAcorn), expectedIsAcorn, actualIsAcorn);
            VerifyEqual(string.Format("Verify Comment is '{0}'", expectedComment), expectedComment, actualComment);
        }

        /// <summary>
        /// Verify photo form
        /// </summary>
        /// <param name="page"></param>
        private void VerifyPhotoForm(InventoryVerificationPage page)
        {
            string expectedSnapshotTitle = "Take a photo";
            var actualSnapshotTitle = page.VerificationPopupPanel.GetTakePhotoFormCaptionText();
            VerifyEqual(string.Format("Verify Snapshot Title is '{0}'", expectedSnapshotTitle), expectedSnapshotTitle, actualSnapshotTitle);
        }

        /// <summary>
        /// Verify run test form
        /// </summary>
        /// <param name="page"></param>
        private void VerifyRunTestForm(InventoryVerificationPage page)
        {
            string expectedTitle = "Test the streetlight";
            var actualTitle = page.VerificationPopupPanel.GetRunTestFormCaptionText();
            VerifyEqual(string.Format("Verify Run Test title is '{0}'", expectedTitle), expectedTitle, actualTitle);
        }

        /// <summary>
        /// Verify finish form
        /// </summary>
        /// <param name="page"></param>
        /// <param name="expecetedMessage"></param>
        private void VerifyFinishForm(InventoryVerificationPage page, string expecetedMessage)
        {
            var actualMessage = page.VerificationPopupPanel.GetFinishMessageText();
            VerifyEqual(string.Format("Verify Message is '{0}'", expecetedMessage), expecetedMessage, actualMessage);
            VerifyEqual("Verify Next button is invisible", false, page.VerificationPopupPanel.IsNextButtonVisible());
            VerifyEqual("Verify Finish button is visible", true, page.VerificationPopupPanel.IsFinishButtonVisible());
            VerifyEqual("Verify Back button is visible", true, page.VerificationPopupPanel.IsBackButtonVisible());
        }

        /// <summary>
        /// Verify background icon of a streetlight wih specific color
        /// </summary>
        /// <param name="page"></param>
        /// <param name="streetlightName"></param>
        /// <param name="color"></param>
        private void VerifyBackgroundIcon(InventoryVerificationPage page, string streetlightName, BgColor color)
        {
            var expectedImgBgBytes = ImageUtility.GetStreetlightBgIconBytes(color);
            var actualImgBgBytes = page.Map.GetStreetlightBgIconBytes(streetlightName);
            var result = ImageUtility.Compare(expectedImgBgBytes, actualImgBgBytes);
            var isTrue = result == 0;
            if (!Browser.Name.Equals("Chrome")) isTrue = result < 100;
            VerifyTrue(string.Format("Verify '{0}' on the map is turned into {1}", streetlightName, color.ToString()), isTrue, color.ToString(), string.Format("Not {0}", color.ToString()));
        }

        #endregion //Inventory Verification

        #region Installation

        /// <summary>
        /// Verify SLV-1926
        /// </summary>
        /// <param name="page"></param>
        private void VerifySLV1926ForInstallation(InstallationPage page)
        {
            var expectedControllerID = "Controller ID *";
            var expectedIdentifier = "Identifier *";
            var expectedName = "Name *";
            var expectedAddress1 = "Address 1";
            var expectedAddress2 = "Address 2";
            var expectedCity = "City";
            var expectedZipCode = "Zip code";
            var expectedLampIsAccessible = "Lamp is accessible";

            Step("-> Checking SLV-1926: Inventory Verification and Installation - Add Name field");
            //- Verify in the first page of the wizard, following fields are present: (SLV-1926)
            // + Controller ID * 
            // + Identifier * 
            // + Name * 
            // + Address 1 
            // + Address 2 
            // + City 
            // + Zip code 
            // + Lamp is accessible

            var actualControllerID = page.InstallationPopupPanel.GetControllerIdText();
            var actualIdentifier = page.InstallationPopupPanel.GetIdentifierText();
            var actualName = page.InstallationPopupPanel.GetNameText();
            var actualAddress1 = page.InstallationPopupPanel.GetAddress1Text();
            var actualAddress2 = page.InstallationPopupPanel.GetAddress2Text();
            var actualCity = page.InstallationPopupPanel.GetCityText();
            var actualZipCode = page.InstallationPopupPanel.GetZipcodeText();
            var actualLampIsAccessible = page.InstallationPopupPanel.GetLampIsAccessibleText();

            VerifyTrue("Verify 'Controller ID' is displayed", expectedControllerID.Equals(actualControllerID), "Displayed", "Not Display");
            VerifyTrue("Verify 'Identifier' is displayed", expectedIdentifier.Equals(actualIdentifier), "Displayed", "Not Display");
            VerifyTrue("Verify 'Name' is displayed", expectedName.Equals(actualName), "Displayed", "Not Display");
            VerifyTrue("Verify 'Address 1' is displayed", expectedAddress1.Equals(actualAddress1), "Displayed", "Not Display");
            VerifyTrue("Verify 'Address 2' is displayed", expectedAddress2.Equals(actualAddress2), "Displayed", "Not Display");
            VerifyTrue("Verify 'City' is displayed", expectedCity.Equals(actualCity), "Displayed", "Not Display");
            VerifyTrue("Verify 'Zip code' is displayed", expectedZipCode.Equals(actualZipCode), "Displayed", "Not Display");
            VerifyTrue("Verify 'Lamp is accessible' is displayed", expectedLampIsAccessible.Equals(actualLampIsAccessible), "Displayed", "Not Display");

            //Verify Identifier and Name's values are automatically and randomly generated with the same value and starts with "New-"
            var curIdentifier = page.InstallationPopupPanel.GetIdentifierValue();
            var curName = page.InstallationPopupPanel.GetNameValue();
            VerifyEqual("Verify Identifier and Name's values are automatically and randomly generated with the same value and starts with \"New-\"", true, curIdentifier.Equals(curName) && curIdentifier.Contains("New-"));

            //- Verify all fields are editable
            VerifyTrue("Verify 'Controller ID' field should be editable", page.InstallationPopupPanel.IsControllerIdEditable(), "Should be editable", "Read-only");
            //VerifyTrue("Verify 'Identifier' field should be editable", page.InstallationPopupPanel.IsIdentifierEditable(), "Should be editable", "Read-only");
            VerifyTrue("Verify 'Name' field should be editable", page.InstallationPopupPanel.IsNameEditable(), "Should be editable", "Read-only");
            VerifyTrue("Verify 'Address 1' field should be editable", page.InstallationPopupPanel.IsAddress1Editable(), "Should be editable", "Read-only");
            VerifyTrue("Verify 'Address 2' field should be editable", page.InstallationPopupPanel.IsAddress2Editable(), "Should be editable", "Read-only");
            VerifyTrue("Verify 'City' field should be editable", page.InstallationPopupPanel.IsCityEditable(), "Should be editable", "Read-only");
            VerifyTrue("Verify 'Zip code' field should be editable", page.InstallationPopupPanel.IsZipcodeEditable(), "Should be editable", "Read-only");
            VerifyTrue("Verify 'Lamp is accessible' field should be editable", page.InstallationPopupPanel.IsLampIsAccessibleEditable(), "Should be editable", "Read-only");
        }

        /// <summary>
        /// Verify basic infos form
        /// </summary>
        /// <param name="page"></param>
        /// <param name="expectedController"></param>
        /// <param name="expectedIdentifier"></param>
        /// <param name="expectedName"></param>
        /// <param name="expectedAddress1"></param>
        /// <param name="expectedAddress2"></param>
        /// <param name="expectedCity"></param>
        /// <param name="expectedZipcode"></param>
        /// <param name="expectedLampIsAccess"></param>
        private void VerifyBasicInfoForm(InstallationPage page, string expectedController, string expectedIdentifier, string expectedName, string expectedAddress1, string expectedAddress2, string expectedCity, string expectedZipcode, string expectedLampIsAccess)
        {
            var actualController = page.InstallationPopupPanel.GetControllerIdValue();
            var actualIdentifier = page.InstallationPopupPanel.GetIdentifierValue();
            var actualName = page.InstallationPopupPanel.GetNameValue();
            var actualAddress1 = page.InstallationPopupPanel.GetAddress1Value();
            var actualAddress2 = page.InstallationPopupPanel.GetAddress2Value();
            var actualCity = page.InstallationPopupPanel.GetCityValue();
            var actualZipcode = page.InstallationPopupPanel.GetZipcodeValue();
            var actualLampIsAccess = page.InstallationPopupPanel.GetLampIsAccessibleValue();

            VerifyEqual(string.Format("Verify expectedController is '{0}'", expectedController), expectedController, actualController);
            VerifyEqual(string.Format("Verify Identifier is '{0}'", expectedIdentifier), expectedIdentifier, actualIdentifier);
            VerifyEqual(string.Format("Verify Name is '{0}'", expectedName), expectedName, actualName);
            VerifyEqual(string.Format("Verify Address 1 is '{0}'", expectedAddress1), expectedAddress1, actualAddress1);
            VerifyEqual(string.Format("Verify Address 2 is '{0}'", expectedAddress2), expectedAddress2, actualAddress2);
            VerifyEqual(string.Format("Verify City is '{0}'", expectedCity), expectedCity, actualCity);
            VerifyEqual(string.Format("Verify Zipcode is '{0}'", expectedZipcode), expectedZipcode, actualZipcode);
            VerifyEqual(string.Format("Verify Is Lamp Position Correct is '{0}'", expectedLampIsAccess), expectedLampIsAccess, actualLampIsAccess);
        }

        /// <summary>
        /// Verify reason form
        /// </summary>
        /// <param name="page"></param>
        /// <param name="expecetedMessage"></param>
        private void VerifyReasonForm(InstallationPage page, string expecetedMessage)
        {
            var reasonTitle = page.InstallationPopupPanel.GetReasonFormCaptionText();
            VerifyEqual(string.Format("Verify Reason Title is '{0}'", expecetedMessage), expecetedMessage, reasonTitle);
        }

        /// <summary>
        /// Verify photo form
        /// </summary>
        /// <param name="page"></param>
        private void VerifyPhotoForm(InstallationPage page)
        {
            string expectedSnapshotTitle = "Take a photo";
            var actualSnapshotTitle = page.InstallationPopupPanel.GetTakePhotoFormCaptionText();
            VerifyEqual(string.Format("Verify Snapshot Title is '{0}'", expectedSnapshotTitle), expectedSnapshotTitle, actualSnapshotTitle);
        }

        /// <summary>
        /// Verify run test form
        /// </summary>
        /// <param name="page"></param>
        private void VerifyRunTestForm(InstallationPage page)
        {
            string expectedTitle = "Test the streetlight";
            var actualTitle = page.InstallationPopupPanel.GetRunTestFormCaptionText();
            VerifyEqual(string.Format("Verify Run Test title is '{0}'", expectedTitle), expectedTitle, actualTitle);
        }

        /// <summary>
        /// Verify comment form
        /// </summary>
        /// <param name="page"></param>
        private void VerifyCommentForm(InstallationPage page)
        {
            string expectedTitle = "Please add some comment";
            var actualTitle = page.InstallationPopupPanel.GetCommentFormCaptionText();
            VerifyEqual(string.Format("Verify Comment title is '{0}'", expectedTitle), expectedTitle, actualTitle);
        }

        /// <summary>
        /// Verify finish form
        /// </summary>
        /// <param name="page"></param>
        /// <param name="expecetedMessage"></param>
        private void VerifyFinishForm(InstallationPage page, string expecetedMessage)
        {
            var actualMessage = page.InstallationPopupPanel.GetFinishMessageText();
            VerifyEqual(string.Format("Verify Message is '{0}'", expecetedMessage), expecetedMessage, actualMessage);
            VerifyEqual("Verify Next button is invisible", false, page.InstallationPopupPanel.IsNextButtonVisible());
            VerifyEqual("Verify Finish button is visible", true, page.InstallationPopupPanel.IsFinishButtonVisible());
            VerifyEqual("Verify Back button is visible", true, page.InstallationPopupPanel.IsBackButtonVisible());
        }

        /// <summary>
        /// Verify background icon of a streetlight wih specific color
        /// </summary>
        /// <param name="page"></param>
        /// <param name="streetlightName"></param>
        /// <param name="color"></param>
        private void VerifyBackgroundIcon(InstallationPage page, string streetlightName, BgColor color)
        {
            page.GeozoneTreeMainPanel.SelectNode(streetlightName);
            page.WaitForPreviousActionComplete();
            page.WaitForPopupDialogDisplayed();
            page.InstallationPopupPanel.ClickCancelButton();
            page.WaitForPopupDialogDisappeared();

            var expectedImgBgBytes = ImageUtility.GetStreetlightBgIconBytes(color);
            var actualImgBgBytes = page.Map.GetStreetlightBgIconBytes(streetlightName);
            var result = ImageUtility.Compare(expectedImgBgBytes, actualImgBgBytes);
            var isTrue = result == 0;
            if (!Browser.Name.Equals("Chrome")) isTrue = result < 100;
            VerifyEqual(string.Format("Verify background icon of a streetlight is {0}", color.ToString()), true, isTrue);
        }

        #endregion //Installation

        #endregion //Verify methods

        #region XML Input data

        private Dictionary<string, object> GetCommonTestData()
        {
            var realtimeGeozone = Settings.CommonTestData[0];
            var testData = new Dictionary<string, object>();
            testData.Add("Geozone", realtimeGeozone.Path);
            var controller = realtimeGeozone.Devices.FirstOrDefault(p => p.Type == DeviceType.Controller && p.Status == DeviceStatus.Working);
            testData.Add("Controller", controller);
            var streetlights = realtimeGeozone.Devices.Where(p => p.Type == DeviceType.Streetlight && p.Status == DeviceStatus.Working && string.IsNullOrEmpty(p.Cluster)).ToList();
            testData.Add("Streetlights", streetlights);

            return testData;
        }        

        /// <summary>
        /// Read test data for test case TS2_03_04
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfTestTS2_03_04()
        {
            var testCaseName = "TS2_3_4";
            var xmlUtility = new XmlUtility(Settings.TC2_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, string>();

            testData.Add("ParentGeozone", xmlUtility.GetSingleNodeText(string.Format(Settings.TC2_XPATH_PREFIX, testCaseName, "ParentGeozone")));
            testData.Add("DestGeozone", xmlUtility.GetSingleNodeText(string.Format(Settings.TC2_XPATH_PREFIX, testCaseName, "DestGeozone")));

            return testData;
        }

        /// <summary>
        /// Read test data for test case TS2_04_02
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfTestTS2_04_02()
        {
            var testCaseName = "TS2_4_2";
            var xmlUtility = new XmlUtility(Settings.TC2_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, string>();
            testData.Add("ParentGeozone", xmlUtility.GetSingleNodeText(string.Format(Settings.TC2_XPATH_PREFIX, testCaseName, "ParentGeozone")));

            return testData;
        }

        /// <summary>
        /// Read test data for test case TS2_05_01
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfTestTS2_05_01()
        {
            var testCaseName = "TS2_5_1";
            var xmlUtility = new XmlUtility(Settings.TC2_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, string>();
            var streetlightNode = xmlUtility.GetSingleNode(string.Format(Settings.TC2_XPATH_PREFIX, testCaseName, "NewStreetlight"));
            testData.Add("name", streetlightNode.GetAttrVal("name"));
            testData.Add("geozone", streetlightNode.GetAttrVal("geozone"));
            testData.Add("latitude", streetlightNode.GetAttrVal("latitude"));
            testData.Add("longitude", streetlightNode.GetAttrVal("longitude"));

            var identityNode = streetlightNode.SelectSingleNode("Identity");
            testData.Add("controllerId", identityNode.GetAttrVal("controllerId"));
            testData.Add("identifier", identityNode.GetAttrVal("identifier"));
            testData.Add("typeOfEquipment", identityNode.GetAttrVal("typeOfEquipment"));
            testData.Add("dimmingGroup", identityNode.GetAttrVal("dimmingGroup"));
            testData.Add("uniqueAddress", identityNode.GetAttrVal("uniqueAddress"));
            testData.Add("controllerInstallDate", identityNode.GetAttrVal("controllerInstallDate"));
            testData.Add("installStatus", identityNode.GetAttrVal("installStatus"));
            testData.Add("serialNumber", identityNode.GetAttrVal("serialNumber"));
            testData.Add("deviceHWVersion", identityNode.GetAttrVal("deviceHWVersion"));
            testData.Add("deviceSWVersion", identityNode.GetAttrVal("deviceSWVersion"));
            testData.Add("nicSerialNumber", identityNode.GetAttrVal("nicSerialNumber"));
            testData.Add("nicSWVersion", identityNode.GetAttrVal("nicSWVersion"));
            testData.Add("nicHWVersion", identityNode.GetAttrVal("nicHWVersion"));
            testData.Add("nicCurrentMode", identityNode.GetAttrVal("nicCurrentMode"));
            testData.Add("nicFallbackMode", identityNode.GetAttrVal("nicFallbackMode"));
            testData.Add("manufacturingDate", identityNode.GetAttrVal("manufacturingDate"));
            testData.Add("reference", identityNode.GetAttrVal("reference"));
            testData.Add("elexonChargeCode", identityNode.GetAttrVal("elexonChargeCode"));
            testData.Add("utilityId", identityNode.GetAttrVal("utilityId"));
            testData.Add("timeout", identityNode.GetAttrVal("timeout"));
            testData.Add("retries", identityNode.GetAttrVal("retries"));

            var inventoryNode = streetlightNode.SelectSingleNode("Inventory");
            testData.Add("premise", inventoryNode.GetAttrVal("premise"));
            testData.Add("address1", inventoryNode.GetAttrVal("address1"));
            testData.Add("address2", inventoryNode.GetAttrVal("address2"));
            testData.Add("city", inventoryNode.GetAttrVal("city"));
            testData.Add("zipCode", inventoryNode.GetAttrVal("zipCode"));
            testData.Add("mapNumber", inventoryNode.GetAttrVal("mapNumber"));
            testData.Add("locationType", inventoryNode.GetAttrVal("locationType"));
            testData.Add("utilityLocationID", inventoryNode.GetAttrVal("utilityLocationID"));
            testData.Add("pictureFilePath", inventoryNode.GetAttrVal("pictureFilePath"));
            testData.Add("accountNumber", inventoryNode.GetAttrVal("accountNumber"));
            testData.Add("customerNumber", inventoryNode.GetAttrVal("customerNumber"));
            testData.Add("customerName", inventoryNode.GetAttrVal("customerName"));
            testData.Add("lampType", inventoryNode.GetAttrVal("lampType"));
            testData.Add("lampWattage", inventoryNode.GetAttrVal("lampWattage"));
            testData.Add("fixedSavedPower", inventoryNode.GetAttrVal("fixedSavedPower"));
            testData.Add("lampInstallDate", inventoryNode.GetAttrVal("lampInstallDate"));
            testData.Add("powerFactorThreshold", inventoryNode.GetAttrVal("powerFactorThreshold"));
            testData.Add("onLuxLevel", inventoryNode.GetAttrVal("onLuxLevel"));
            testData.Add("offLuxLevel", inventoryNode.GetAttrVal("offLuxLevel"));
            testData.Add("ballastType", inventoryNode.GetAttrVal("ballastType"));
            testData.Add("dimmingInterface", inventoryNode.GetAttrVal("dimmingInterface"));
            testData.Add("ballastBrand", inventoryNode.GetAttrVal("ballastBrand"));
            testData.Add("poleOrHeadInstall", inventoryNode.GetAttrVal("poleOrHeadInstall"));
            testData.Add("luminaireBrand", inventoryNode.GetAttrVal("luminaireBrand"));
            testData.Add("luminaireType", inventoryNode.GetAttrVal("luminaireType"));
            testData.Add("luminaireModel", inventoryNode.GetAttrVal("luminaireModel"));
            testData.Add("lightDistribution", inventoryNode.GetAttrVal("lightDistribution"));
            testData.Add("orientation", inventoryNode.GetAttrVal("orientation"));
            testData.Add("colorCode", inventoryNode.GetAttrVal("colorCode"));
            testData.Add("status", inventoryNode.GetAttrVal("status"));
            testData.Add("luminaireInstallDate", inventoryNode.GetAttrVal("luminaireInstallDate"));
            testData.Add("bracketBrand", inventoryNode.GetAttrVal("bracketBrand"));
            testData.Add("bracketModel", inventoryNode.GetAttrVal("bracketModel"));
            testData.Add("bracketType", inventoryNode.GetAttrVal("bracketType"));
            testData.Add("bracketColor", inventoryNode.GetAttrVal("bracketColor"));
            testData.Add("poleType", inventoryNode.GetAttrVal("poleType"));
            testData.Add("poleHeight", inventoryNode.GetAttrVal("poleHeight"));
            testData.Add("poleShape", inventoryNode.GetAttrVal("poleShape"));
            testData.Add("poleMaterial", inventoryNode.GetAttrVal("poleMaterial"));
            testData.Add("poleColorCode", inventoryNode.GetAttrVal("poleColorCode"));
            testData.Add("poleStatus", inventoryNode.GetAttrVal("poleStatus"));
            testData.Add("typeOfGroundFixing", inventoryNode.GetAttrVal("typeOfGroundFixing"));
            testData.Add("poleInstallDate", inventoryNode.GetAttrVal("poleInstallDate"));
            testData.Add("comment", inventoryNode.GetAttrVal("comment"));

            var electricalNetworkNode = streetlightNode.SelectSingleNode("ElectricalNetwork");
            testData.Add("energySupplier", electricalNetworkNode.GetAttrVal("energySupplier"));
            testData.Add("networkType", electricalNetworkNode.GetAttrVal("networkType"));
            testData.Add("supplyVoltage", electricalNetworkNode.GetAttrVal("supplyVoltage"));
            testData.Add("segment", electricalNetworkNode.GetAttrVal("segment"));
            testData.Add("section", electricalNetworkNode.GetAttrVal("section"));
            testData.Add("highVoltageThreshold", electricalNetworkNode.GetAttrVal("highVoltageThreshold"));
            testData.Add("lowVoltageThreshold", electricalNetworkNode.GetAttrVal("lowVoltageThreshold"));

            return testData;
        }

        /// <summary>
        /// Read test data for test case TS2_07_01
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfTestTS2_07_01()
        {
            var testCaseName = "TS2_7_1";
            var xmlUtility = new XmlUtility(Settings.TC2_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, string>();
            var streetlightNode = xmlUtility.GetSingleNode(string.Format(Settings.TC2_XPATH_PREFIX, testCaseName, "Streetlight"));
            testData.Add("geozone", streetlightNode.GetAttrVal("geozone"));
            testData.Add("name", streetlightNode.GetAttrVal("name"));

            return testData;
        }

        /// <summary>
        /// Read test data for test case TS2_12_01
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, List<XmlNode>> GetTestDataOfTestTS2_12_01()
        {
            var testCaseName = "TS2_12_1";
            var xmlUtility = new XmlUtility(Settings.TC2_TEST_DATA_FILE_PATH);
            var testData = new Dictionary<string, List<XmlNode>>();
            testData.Add("SearchNodeList", xmlUtility.GetNodes(string.Format(Settings.TC2_XPATH_PREFIX, testCaseName, "Search")));

            return testData;
        }

        /// <summary>
        /// Read test data for test case TS2_14_01
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, object> GetTestDataOfTestTS2_14_01()
        {
            return GetCommonTestData();
        }

        /// <summary>
        /// Read test data for test case TS2_16_01
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfTestTS2_16_01()
        {
            var testCaseName = "TS2_16_1";
            var xmlUtility = new XmlUtility(Settings.TC2_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, string>();
            var geozone = xmlUtility.GetSingleNodeText(string.Format(Settings.TC2_XPATH_PREFIX, testCaseName, "Geozone"));
            testData.Add("Geozone", geozone);

            return testData;
        }

        /// <summary>
        /// Read test data for test case TS2_16_02
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfTestTS2_16_02()
        {
            var testCaseName = "TS2_16_2";
            var xmlUtility = new XmlUtility(Settings.TC2_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, string>();
            var geozone = xmlUtility.GetSingleNodeText(string.Format(Settings.TC2_XPATH_PREFIX, testCaseName, "Geozone"));
            testData.Add("Geozone", geozone);

            return testData;
        }

        /// <summary>
        /// Read test data for test case TS2_16_03
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfTestTS2_16_03()
        {
            var testCaseName = "TS2_16_3";
            var xmlUtility = new XmlUtility(Settings.TC2_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, string>();
            var geozone = xmlUtility.GetSingleNodeText(string.Format(Settings.TC2_XPATH_PREFIX, testCaseName, "Geozone"));
            testData.Add("Geozone", geozone);

            return testData;
        }

        /// <summary>
        /// Read test data for test case TS2_17_01
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfTestTS2_17_01()
        {
            var testCaseName = "TS2_17_1";
            var xmlUtility = new XmlUtility(Settings.TC2_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, string>();
            var geozone = xmlUtility.GetSingleNodeText(string.Format(Settings.TC2_XPATH_PREFIX, testCaseName, "Geozone"));
            testData.Add("Geozone", geozone);

            return testData;
        }

        /// <summary>
        /// Read test data for test case TS2_17_02
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfTestTS2_17_02()
        {
            var testCaseName = "TS2_17_2";
            var xmlUtility = new XmlUtility(Settings.TC2_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, string>();
            var geozone = xmlUtility.GetSingleNodeText(string.Format(Settings.TC2_XPATH_PREFIX, testCaseName, "Geozone"));
            testData.Add("Geozone", geozone);

            return testData;
        }

        /// <summary>
        /// Read test data for test case TS2_17_03
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfTestTS2_17_03()
        {
            var testCaseName = "TS2_17_3";
            var xmlUtility = new XmlUtility(Settings.TC2_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, string>();
            var geozone = xmlUtility.GetSingleNodeText(string.Format(Settings.TC2_XPATH_PREFIX, testCaseName, "Geozone"));
            testData.Add("Geozone", geozone);

            return testData;
        }

        /// <summary>
        /// Read test data for test case TS2_17_04
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfTestTS2_17_04()
        {
            var testCaseName = "TS2_17_4";
            var xmlUtility = new XmlUtility(Settings.TC2_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, string>();
            var geozone = xmlUtility.GetSingleNodeText(string.Format(Settings.TC2_XPATH_PREFIX, testCaseName, "Geozone"));
            testData.Add("Geozone", geozone);

            return testData;
        }

        /// <summary>
        /// Read test data for test case TS2_17_05
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfTestTS2_17_05()
        {
            var testCaseName = "TS2_17_5";
            var xmlUtility = new XmlUtility(Settings.TC2_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, string>();
            var geozone = xmlUtility.GetSingleNodeText(string.Format(Settings.TC2_XPATH_PREFIX, testCaseName, "Geozone"));
            testData.Add("Geozone", geozone);

            return testData;
        }

        /// <summary>
        /// Read test data for test case TS2_18_01
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfTestTS2_18_01()
        {
            var testCaseName = "TS2_18_1";
            var xmlUtility = new XmlUtility(Settings.TC2_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, string>();
            var geozone = xmlUtility.GetSingleNodeText(string.Format(Settings.TC2_XPATH_PREFIX, testCaseName, "Geozone"));
            testData.Add("Geozone", geozone);

            return testData;
        }

        /// <summary>
        /// Read test data for test case TS2_19_01
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfTestTS2_19_01()
        {
            var testCaseName = "TS2_19_1";
            var xmlUtility = new XmlUtility(Settings.TC2_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, string>();
            var geozone = xmlUtility.GetSingleNodeText(string.Format(Settings.TC2_XPATH_PREFIX, testCaseName, "Geozone"));
            testData.Add("Geozone", geozone);

            return testData;
        }

        /// <summary>
        /// Read test data for test case TS2_20_01
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfTestTS2_20_01()
        {
            var testCaseName = "TS2_20_1";
            var xmlUtility = new XmlUtility(Settings.TC2_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, string>();
            var geozone = xmlUtility.GetSingleNodeText(string.Format(Settings.TC2_XPATH_PREFIX, testCaseName, "Geozone"));
            testData.Add("Geozone", geozone);

            return testData;
        }

        /// <summary>
        /// Read test data for test case TSTS2_21_1
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfTestTS2_21_01()
        {
            var testCaseName = "TS2_21_1";
            var xmlUtility = new XmlUtility(Settings.TC2_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, string>();
            var step1 = xmlUtility.GetSingleNode(string.Format(Settings.TC2_XPATH_PREFIX, testCaseName, "Step1"));
            testData.Add("ControllerId", step1.GetChildNodeText("ControllerId"));
            testData.Add("ControllerName", step1.GetChildNodeText("ControllerName"));
            testData.Add("Address1", step1.GetChildNodeText("Address1"));
            testData.Add("Address2", step1.GetChildNodeText("Address2"));
            testData.Add("City", step1.GetChildNodeText("City"));
            testData.Add("Zipcode", step1.GetChildNodeText("Zipcode"));
            testData.Add("IsLampPositionCorrect", step1.GetChildNodeText("IsLampPositionCorrect"));

            return testData;
        }

        /// <summary>
        /// Read test data for test case TS2_21_2
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfTestTS2_21_02()
        {
            var testCaseName = "TS2_21_2";
            var xmlUtility = new XmlUtility(Settings.TC2_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, string>();
            testData.Add("Reason", xmlUtility.GetSingleNodeText(string.Format(Settings.TC2_XPATH_PREFIX, testCaseName, "Reason")));
            var step1 = xmlUtility.GetSingleNode(string.Format(Settings.TC2_XPATH_PREFIX, testCaseName, "Step1"));
            testData.Add("ControllerId", step1.GetChildNodeText("ControllerId"));
            testData.Add("ControllerName", step1.GetChildNodeText("ControllerName"));
            testData.Add("Address1", step1.GetChildNodeText("Address1"));
            testData.Add("Address2", step1.GetChildNodeText("Address2"));
            testData.Add("City", step1.GetChildNodeText("City"));
            testData.Add("Zipcode", step1.GetChildNodeText("Zipcode"));
            testData.Add("IsLampPositionCorrect", step1.GetChildNodeText("IsLampPositionCorrect"));

            return testData;
        }

        /// <summary>
        /// Read test data for test case TS2_21_3
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfTestTS2_21_03()
        {
            var testCaseName = "TS2_21_3";
            var xmlUtility = new XmlUtility(Settings.TC2_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, string>();
            testData.Add("Reason", xmlUtility.GetSingleNodeText(string.Format(Settings.TC2_XPATH_PREFIX, testCaseName, "Reason")));
            var step1 = xmlUtility.GetSingleNode(string.Format(Settings.TC2_XPATH_PREFIX, testCaseName, "Step1"));
            testData.Add("ControllerId", step1.GetChildNodeText("ControllerId"));
            testData.Add("ControllerName", step1.GetChildNodeText("ControllerName"));
            testData.Add("Address1", step1.GetChildNodeText("Address1"));
            testData.Add("Address2", step1.GetChildNodeText("Address2"));
            testData.Add("City", step1.GetChildNodeText("City"));
            testData.Add("Zipcode", step1.GetChildNodeText("Zipcode"));
            testData.Add("IsLampPositionCorrect", step1.GetChildNodeText("IsLampPositionCorrect"));

            return testData;
        }

        /// <summary>
        /// Read test data for test case TS2_22_1
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfTestTS2_22_01()
        {
            var testCaseName = "TS2_22_1";
            var xmlUtility = new XmlUtility(Settings.TC2_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, string>();
            var step1 = xmlUtility.GetSingleNode(string.Format(Settings.TC2_XPATH_PREFIX, testCaseName, "Step1"));
            var step3 = xmlUtility.GetSingleNode(string.Format(Settings.TC2_XPATH_PREFIX, testCaseName, "Step3"));

            testData.Add("ControllerId", step1.GetChildNodeText("ControllerId"));
            testData.Add("ControllerName", step1.GetChildNodeText("ControllerName"));
            testData.Add("TypeOfEquipment", step1.GetChildNodeText("TypeOfEquipment"));
            testData.Add("LampIsAccessible", step1.GetChildNodeText("LampIsAccessible"));
            testData.Add("UniqueAddress", SLVHelper.GenerateMACAddress());
            testData.Add("NicSerialNumber", SLVHelper.GenerateMACAddress());
            testData.Add("LampCameOn", step3.GetChildNodeText("LampCameOn"));

            return testData;
        }

        /// <summary>
        /// Read test data for test case TS2_22_2
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, object> GetTestDataOfTestTS2_22_02()
        {
            var testCaseName = "TS2_22_2";
            var xmlUtility = new XmlUtility(Settings.TC2_TEST_DATA_FILE_PATH);

            var testData = GetCommonTestData();            
            var step1 = xmlUtility.GetSingleNode(string.Format(Settings.TC2_XPATH_PREFIX, testCaseName, "Step1"));
            var step2 = xmlUtility.GetSingleNode(string.Format(Settings.TC2_XPATH_PREFIX, testCaseName, "Step2"));            
            testData.Add("LampIsAccessible", step1.GetChildNodeText("LampIsAccessible"));
            testData.Add("Reason", step2.GetChildNodeText("Reason"));

            return testData;
        }

        /// <summary>
        /// Read test data for test case TS2_22_3
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, object> GetTestDataOfTestTS2_22_03()
        {
            var testCaseName = "TS2_22_3";
            var xmlUtility = new XmlUtility(Settings.TC2_TEST_DATA_FILE_PATH);

            var testData = GetCommonTestData();
            var step1 = xmlUtility.GetSingleNode(string.Format(Settings.TC2_XPATH_PREFIX, testCaseName, "Step1"));
            var step2 = xmlUtility.GetSingleNode(string.Format(Settings.TC2_XPATH_PREFIX, testCaseName, "Step2"));
            
            testData.Add("LampIsAccessible", step1.GetChildNodeText("LampIsAccessible"));
            testData.Add("Reason", step2.GetChildNodeText("Reason"));

            return testData;
        }

        /// <summary>
        /// Read test data for test case TS2_25_1
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, object> GetTestDataOfTestTS2_25_01()
        {
            var testCaseName = "TS2_25_1";
            var xmlUtility = new XmlUtility(Settings.TC2_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, object>();
            testData.Add("GeoZones", xmlUtility.GetChildNodesText(string.Format(Settings.TC2_XPATH_PREFIX, testCaseName, "GeoZones")));
            testData.Add("SearchName", xmlUtility.GetSingleNodeText(string.Format(Settings.TC2_XPATH_PREFIX, testCaseName, "SearchName")));
            testData.Add("ExportFilePattern", xmlUtility.GetSingleNodeText(string.Format(Settings.TC2_XPATH_PREFIX, testCaseName, "ExportFilePattern")));

            return testData;
        }

        /// <summary>
        /// Read test data for test case SLV_2019
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfTestSLV_2019()
        {
            var testCaseName = "SLV_2019";
            var xmlUtility = new XmlUtility(Settings.TC2_TEST_DATA_FILE_PATH);

            var testData = new Dictionary<string, string>();
            testData.Add("RowsPerPage", xmlUtility.GetSingleNodeText(string.Format(Settings.TC2_XPATH_PREFIX, testCaseName, "RowsPerPage")));

            return testData;
        }

        #endregion //XML Input data

        #endregion
    }
}
