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
    public class InstallationAppTests : TestBase
    {
        #region Variables
        
        #endregion //Variables

        #region Contructors

        #endregion //Contructors

        #region Test Cases

        [Test, DynamicRetry]
        [Description("Installation App - SC-377 - UI of Installation app")]
        public void SC_377_01()
        {
            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.Installation);

            Step("1. Go to Installation app");
            Step("2. Verify Installation page is routed and loaded successfully");            
            var installationPage = desktopPage.GoToApp(App.Installation) as InstallationPage;
            
            Step("3. Press Follow Me button the the top right corner of the map");
            installationPage.Map.ClickFollowMeButton();            
            installationPage.WaitForHeaderMessageDisplayed();
            var headerMessage = installationPage.GetHeaderMessage();
            SLVHelper.AllowOnceLocation();

            Step("4. Verify a message \"Search your location...\" displays. Users is taken to the current location");
            VerifyEqual("4. Verify a message \"Search your location...\" displays. Users is taken to the current location", "Search your location...", headerMessage);
            installationPage.WaitForHeaderMessageDisappeared();

            Step("5. Press Earth/Plan map to change the type of map");
            var currentMapButtonIsEarth = installationPage.Map.IsEarthButtonDisplayed();
            var currentMapButtonIsPlan = installationPage.Map.IsPlanButtonDisplayed();
            installationPage.Map.ClickEarthOrPlanButton();

            Step("6. Verify If the current type of map is Earth, then it is changed to Plan. Otherwise, it is changed to Earth map.");
            if(currentMapButtonIsEarth)
                VerifyEqual("6. Verify the current type of map is Earth, then it is changed to Plan", true, installationPage.Map.IsPlanButtonDisplayed());
            if (currentMapButtonIsPlan)
                VerifyEqual("6. Verify If the current type of map is Plan, then it is changed to Earth", true, installationPage.Map.IsEarthButtonDisplayed());

            Step("7. Hover the Google Map button");
            installationPage.Map.MoveToChooseMapSourceButton();

            Step("8. Verify There is a pop-up displayed with 3 options: OpenStreetMap, Google Maps, Bing Maps");
            var expectedMapSources = new List<string> { "OpenStreetMap", "Google Maps", "Bing Maps" };
            var actualMapSources = installationPage.Map.GetListOfMapSourceItems();
            VerifyEqual("8. Verify There is a pop-up displayed with 3 options: OpenStreetMap, Google Maps, Bing Maps", expectedMapSources, actualMapSources);
            installationPage.AppBar.ClickHeaderBartop();

            Step("9. Select Bing Maps");
            installationPage.Map.ChooseMapSource(MapSource.BingMaps);

            Step("10. Verify The button is changed to Bing Map logo");
            VerifyEqual("10. Verify The button is changed to Bing Map logo", true, installationPage.Map.GetMapSourceIconUrl().Contains("bingmap.png"));

            Step("11. Select Street Maps");
            installationPage.Map.ChooseMapSource(MapSource.OpenStreetMap);

            Step("12. Verify The button is changed to Street Map logo");
            VerifyEqual("12. Verify The button is changed to Street Map logo", true, installationPage.Map.GetMapSourceIconUrl().Contains("openstreetmap.png"));

            Step("13. Select Google Maps");
            installationPage.Map.ChooseMapSource(MapSource.GoogleMaps);

            Step("14. Verify The button is changed to Google Map logo");
            VerifyEqual("14. Verify The button is changed to Google Map logo", true, installationPage.Map.GetMapSourceIconUrl().Contains("googlemap.png"));

            Step("15. Press Add Streetlight button");
            installationPage.Map.ClickAddStreetlightButton();
            installationPage.Map.WaitForRecorderDisplayed();            

            Step("16. Verify a red blinking rectangle appears in the map with the text: 'Position the new device on the map. Click here to cancel.'");
            var expectedRecordText = "Position the new device on the map. Click here to cancel.";
            VerifyEqual("16. Verify a red blinking rectangle appears in the map with the text: "+ expectedRecordText, expectedRecordText, installationPage.Map.GetRecorderText());

            Step("17. Press on the text");
            installationPage.Map.ClickRecorderCancelButton();
            installationPage.Map.WaitForRecorderDisappeared();

            Step("18. Verify the red blinking rectangle disappears");
            VerifyEqual("18. Verify the red blinking rectangle disappears", false, installationPage.Map.IsRecorderDisplayed());

            Step("19. Press Add Streetlight button, and drop the device in the map");
            installationPage.Map.ClickAddStreetlightButton();
            installationPage.Map.WaitForRecorderDisplayed();
            installationPage.Map.PositionNewDevice();
            installationPage.Map.WaitForRecorderDisappeared();
            installationPage.WaitForPreviousActionComplete();
            installationPage.WaitForPopupDialogDisplayed();

            Step("20. Verify the pop-up displays with");
            Step(" o Title: New Streetlight");
            Step(" o Text: About this light point");
            Step(" o Textbox with the label: Identifier *");
            Step(" o Dropdownlist box with label: Type of equipment *");
            Step(" o Dropdownlist box with label: Controller ID *");
            Step(" o Textbox with the label: Name *");
            Step(" o Textbox with the label: Address 1");
            Step(" o Textbox with the label: Address 2");
            Step(" o Textbox with the label: City");
            Step(" o Textbox with the label: Zip code");
            Step(" o Dropdownlist box with the label: Lamp is accessible");
            Step(" o Button: Next, Cancel");            
            VerifyEqual("20. Verify the pop-up displays", true, installationPage.IsPopupDialogDisplayed());
            VerifyEqual("20. Verify Title: New Streetlight", "New Streetlight", installationPage.InstallationPopupPanel.GetPanelTitleText());
            VerifyEqual("20. Verify Text: About this light point", "About this light point", installationPage.InstallationPopupPanel.GetBasicInfoFormCaptionText());
            VerifyEqual("20. Verify Textbox with the label: Identifier *", "Identifier *", installationPage.InstallationPopupPanel.GetIdentifierText());
            VerifyEqual("20. Verify Dropdownlist box with label: Type of equipment *", "Type of equipment *", installationPage.InstallationPopupPanel.GetTypeOfEquipmentText());
            VerifyEqual("20. Verify Dropdownlist box with label: Controller ID *", "Controller ID *", installationPage.InstallationPopupPanel.GetControllerIdText());
            VerifyEqual("20. Verify Textbox with the label: Name *", "Name *", installationPage.InstallationPopupPanel.GetNameText());
            VerifyEqual("20. Verify Textbox with the label: Address 1", "Address 1", installationPage.InstallationPopupPanel.GetAddress1Text());
            VerifyEqual("20. Verify Textbox with the label: Address 2", "Address 2", installationPage.InstallationPopupPanel.GetAddress2Text());
            VerifyEqual("20. Verify Textbox with the label: City", "City", installationPage.InstallationPopupPanel.GetCityText());
            VerifyEqual("20. Verify Textbox with the label: Zip code", "Zip code", installationPage.InstallationPopupPanel.GetZipcodeText());
            VerifyEqual("20. Verify Dropdownlist box with the label: Lamp is accessible", "Lamp is accessible", installationPage.InstallationPopupPanel.GetLampIsAccessibleText());
            VerifyEqual("20. Verify Button: Next is displayed", true, installationPage.InstallationPopupPanel.IsNextButtonVisible());
            VerifyEqual("20. Verify Button: Cancel is displayed", true, installationPage.InstallationPopupPanel.IsCancelButtonVisible());
            VerifyEqual("[SC-812] Verify Identifier field is read-only", true, !installationPage.InstallationPopupPanel.IsIdentifierEditable());

            Step("21. Press Cancel button");
            installationPage.InstallationPopupPanel.ClickCancelButton();
            installationPage.WaitForPopupDialogDisappeared();

            Step("22. Verify The pop-up is closed and the device is not installed");
            VerifyEqual("22. Verify the pop-up is closed", false, installationPage.IsPopupDialogDisplayed());

            Step("23. Press X icon on the Installation panel");
            installationPage.GeozoneTreeMainPanel.ClickCloseButton();
            installationPage.WaitForMainGeozoneTreeDisappeared();

            Step("24. Verify The Installation panel is closed");
            VerifyEqual("24. Verify The Installation panel is closed", false, installationPage.GeozoneTreeMainPanel.IsPanelVisible());

            Step("25. Press Geozones panel");
            installationPage.ClickShowButton();
            installationPage.WaitForMainGeozoneTreeDisplayed();

            Step("26. Verify The installation panel is opened.");
            VerifyEqual("26. Verify The Installation panel is opened", true, installationPage.GeozoneTreeMainPanel.IsPanelVisible());
        }

        [Test, DynamicRetry]
        [Description("Installation App - SC-377 - Install a new streetlight successfully from Installation app")]
        public void SC_377_02()
        {
            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var testData = GetTestDataOfSC_377_02();
            var controllerId = testData["ControllerId"];
            var controllerName = testData["ControllerName"];
            var geozone = SLVHelper.GenerateUniqueName("GZNSC37702");
            var streetlight = SLVHelper.GenerateUniqueName("STL");

            Step("-> Create data for testing");
            DeleteGeozones("GZNSC37702*");
            CreateNewGeozone(geozone);

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.Installation, App.EquipmentInventory);

            Step("1. Go to Installation app");
            Step("2. Verify Installation page is routed and loaded successfully");
            var installationPage = desktopPage.GoToApp(App.Installation) as InstallationPage;

            Step("3. Geozone Automation > Free Devices");
            installationPage.GeozoneTreeMainPanel.SelectNode(geozone);

            Step("4. Press Add Streetlight button and drop it on the map");
            installationPage.Map.ClickAddStreetlightButton();
            installationPage.Map.WaitForRecorderDisplayed();
            installationPage.Map.PositionNewDevice();
            installationPage.Map.WaitForRecorderDisappeared();
            installationPage.WaitForPreviousActionComplete();
            installationPage.WaitForPopupDialogDisplayed();
            var identifier = installationPage.InstallationPopupPanel.GetIdentifierValue();
            VerifyEqual("[SC-812] Verify Identifier field is read-only", true, !installationPage.InstallationPopupPanel.IsIdentifierEditable());

            Step("5. Press Next button");
            installationPage.InstallationPopupPanel.ClickNextButton();
            installationPage.Dialog.WaitForPopupMessageDisplayed();

            Step("6. Verify The warning message displays \"Please enter 'Type of equipment' property.\"");
            var expectedErrorMsg = "Please enter 'Type of equipment' property.";
            VerifyEqual(string.Format("6. Verify Error message is: '{0}''", expectedErrorMsg), expectedErrorMsg, installationPage.Dialog.GetMessageText());
            installationPage.Dialog.ClickOkButton();
            installationPage.Dialog.WaitForPopupMessageDisappeared();

            Step("7. Press OK, select Type of equipment and press Next");
            var typeOfEquipment = "ABEL-Vigilon A[Dimmable ballast]";
            installationPage.InstallationPopupPanel.SelectTypeOfEquipmentDropDown(typeOfEquipment);
            installationPage.InstallationPopupPanel.ClickNextButton();
            installationPage.Dialog.WaitForPopupMessageDisplayed();

            Step("8. Verify The warning message displays \"Please enter 'Controller ID' property.\"");
            expectedErrorMsg = "Please enter 'Controller ID' property.";
            VerifyEqual(string.Format("8. Verify Error message is: '{0}''", expectedErrorMsg), expectedErrorMsg, installationPage.Dialog.GetMessageText());
            installationPage.Dialog.ClickOkButton();
            installationPage.Dialog.WaitForPopupMessageDisappeared();
            
            Step("9. Press OK, select Controller ID, clear out Name value and press Next");           
            installationPage.InstallationPopupPanel.SelectControllerIdDropDown(controllerName);
            installationPage.InstallationPopupPanel.EnterNameInput("");
            installationPage.InstallationPopupPanel.ClickNextButton();
            installationPage.Dialog.WaitForPopupMessageDisplayed();

            Step("10. Verify The warning message displays \"Please enter 'Name' property.\"");
            expectedErrorMsg = "Please enter 'Name' property.";
            VerifyEqual(string.Format("12. Verify Error message is: '{0}''", expectedErrorMsg), expectedErrorMsg, installationPage.Dialog.GetMessageText());
            installationPage.Dialog.ClickOkButton();
            installationPage.Dialog.WaitForPopupMessageDisappeared();

            Step("11. Press OK, input Name and press Next");
            installationPage.InstallationPopupPanel.EnterNameInput(streetlight);
            installationPage.InstallationPopupPanel.ClickNextButton();
            installationPage.InstallationPopupPanel.WaitForScanQRCodeFormDisplayed();

            Step("12. Verify The pop-up changes to the new UI");
            Step(" o Title: There is a Left Arrow icon next to 'New Streetlight'");
            Step(" o Textbox with label: Unique address");
            Step(" o Textbox with lable: NIC serial number");
            VerifyEqual("12. Verify There is a Left Arrow icon next to 'New Streetlight", true, installationPage.InstallationPopupPanel.IsBackButtonVisible());
            VerifyEqual("12. Verify Textbox with the label: Unique address", "Unique address", installationPage.InstallationPopupPanel.GetUniqueAddressText());
            VerifyEqual("12. Verify Textbox with the label: NIC serial number", "NIC serial number", installationPage.InstallationPopupPanel.GetNicSerialNumberText());

            Step("13. Press Left Arrow icon");
            installationPage.InstallationPopupPanel.ClickBackButton();
            installationPage.InstallationPopupPanel.WaitForBasicInfoFormDisplayed();

            Step("14. Verify The pop-up changes to the previous UI with");
            Step(" o Title: New Streetlight");
            Step(" o Text: About this light point");
            Step(" o Textbox with the label: Identifier *. Value unchanged.");
            Step(" o Dropdownlist box with label: Type of equipment *. Value unchanged.");
            Step(" o Dropdownlist box with label: Controller ID *. Value unchanged.");
            Step(" o Textbox with the label: Name *. Value unchanged.");
            Step(" o Textbox with the label: Address 1");
            Step(" o Textbox with the label: Address 2");
            Step(" o Textbox with the label: City");
            Step(" o Textbox with the label: Zip code");
            Step(" o Dropdownlist box with the label: Lamp is accessible");
            Step(" o Button: Next, Cancel");
            VerifyEqual("14. Verify the pop-up displays", true, installationPage.IsPopupDialogDisplayed());
            VerifyEqual("14. Verify Title: New Streetlight", "New Streetlight", installationPage.InstallationPopupPanel.GetPanelTitleText());
            VerifyEqual("14. Verify Text: About this light point", "About this light point", installationPage.InstallationPopupPanel.GetBasicInfoFormCaptionText());
            VerifyEqual("14. Verify Textbox with the label: Identifier *", "Identifier *", installationPage.InstallationPopupPanel.GetIdentifierText());
            VerifyEqual("14. Verify Identifier Value is unchanged", identifier, installationPage.InstallationPopupPanel.GetIdentifierValue());
            VerifyEqual("14. Verify Dropdownlist box with label: Type of equipment *", "Type of equipment *", installationPage.InstallationPopupPanel.GetTypeOfEquipmentText());
            VerifyEqual("14. Verify Type of equipment Value is unchanged", typeOfEquipment, installationPage.InstallationPopupPanel.GetTypeOfEquipmentValue());
            VerifyEqual("14. Verify Dropdownlist box with label: Controller ID *", "Controller ID *", installationPage.InstallationPopupPanel.GetControllerIdText());
            VerifyEqual("14. Verify Controller ID Value is unchanged", controllerName, installationPage.InstallationPopupPanel.GetControllerIdValue());
            VerifyEqual("14. Verify Textbox with the label: Name *", "Name *", installationPage.InstallationPopupPanel.GetNameText());
            VerifyEqual("14. Verify Name Value is unchanged", streetlight, installationPage.InstallationPopupPanel.GetNameValue());
            VerifyEqual("14. Verify Textbox with the label: Address 1", "Address 1", installationPage.InstallationPopupPanel.GetAddress1Text());
            VerifyEqual("14. Verify Textbox with the label: Address 2", "Address 2", installationPage.InstallationPopupPanel.GetAddress2Text());
            VerifyEqual("14. Verify Textbox with the label: City", "City", installationPage.InstallationPopupPanel.GetCityText());
            VerifyEqual("14. Verify Textbox with the label: Zip code", "Zip code", installationPage.InstallationPopupPanel.GetZipcodeText());
            VerifyEqual("14. Verify Dropdownlist box with the label: Lamp is accessible", "Lamp is accessible", installationPage.InstallationPopupPanel.GetLampIsAccessibleText());
            VerifyEqual("14. Verify Button: Next is displayed", true, installationPage.InstallationPopupPanel.IsNextButtonVisible());
            VerifyEqual("14. Verify Button: Cancel is displayed", true, installationPage.InstallationPopupPanel.IsCancelButtonVisible());

            Step("15. Press Next and input the valid MAC address, then press Next button");
            var macAddress = SLVHelper.GenerateMACAddress();
            installationPage.InstallationPopupPanel.ClickNextButton();
            installationPage.InstallationPopupPanel.WaitForScanQRCodeFormDisplayed();
            installationPage.InstallationPopupPanel.EnterUniqueAddressInput(macAddress);
            installationPage.InstallationPopupPanel.ClickNextButton();
            installationPage.InstallationPopupPanel.WaitForLightComeOnFormDisplayed();

            Step("16. Verify The pop-up changes to the new UI");
            Step(" o Text: Did light come ON once node was installed ?");
            Step(" o Checkbox with lable: Lamp came on");
            VerifyEqual("16. Verify Text: Did light come ON once node was installed ?", "Did light come ON once node was installed ?", installationPage.InstallationPopupPanel.GetLightComeOnFormCaptionText());
            VerifyEqual("16. Verify Checkbox with lable: Lamp came on", "Lamp came on", installationPage.InstallationPopupPanel.GetLampCameOnText());

            Step("17. Press Left Arrow icon");
            installationPage.InstallationPopupPanel.ClickBackButton();
            installationPage.InstallationPopupPanel.WaitForScanQRCodeFormDisplayed();

            Step("18. Verify The pop-up changes to the previous UI with");
            Step(" o Textbox with label: Unique address. Value unchanged.");
            VerifyEqual("20. Verify Textbox with label: Unique address", "Unique address", installationPage.InstallationPopupPanel.GetUniqueAddressText());
            VerifyEqual("20. Verify Value of Unique address unchanged", macAddress, installationPage.InstallationPopupPanel.GetUniqueAddressValue());

            Step("19. Press Next and don't check the checkbox and press Next");
            installationPage.InstallationPopupPanel.ClickNextButton();
            installationPage.InstallationPopupPanel.WaitForLightComeOnFormDisplayed();
            installationPage.InstallationPopupPanel.ClickNextButton();
            installationPage.InstallationPopupPanel.WaitForCommentFormDisplayed();

            Step("20. Verify The pop-up changes to the new UI");
            Step(" o Text: Please add some comment");
            Step(" o Richtextbox with label: Comment");
            VerifyEqual("20. Verify Text: Please add some comment", "Please add some comment", installationPage.InstallationPopupPanel.GetCommentFormCaptionText());
            VerifyEqual("20. Verify Richtextbox with label: Comment", "Comment", installationPage.InstallationPopupPanel.GetCommentText());

            Step("21. Press Left Arrow icon");
            installationPage.InstallationPopupPanel.ClickBackButton();
            installationPage.InstallationPopupPanel.WaitForLightComeOnFormDisplayed();

            Step("22. Verify The pop-up changes to the previous UI with");
            Step(" o Checkbox with label: Lamp came on. Checkbox is unchecked.");
            VerifyEqual("22. Verify Checkbox with label: Lamp came on", "Lamp came on", installationPage.InstallationPopupPanel.GetLampCameOnText());
            VerifyEqual("22. Verify Checkbox is unchecked", false, installationPage.InstallationPopupPanel.GetLampCameOnValue());

            Step("23. Press Next and input some comment, then press Next");
            var comment = SLVHelper.GenerateString();
            installationPage.InstallationPopupPanel.ClickNextButton();
            installationPage.InstallationPopupPanel.WaitForCommentFormDisplayed();
            installationPage.InstallationPopupPanel.EnterCommentInput(comment);
            installationPage.InstallationPopupPanel.ClickNextButton();
            installationPage.InstallationPopupPanel.WaitForFinishFormDisplayed();

            Step("24. Verify The pop-up changes to the new UI");
            Step(" o Text: Thank you, you can go to the next light point.");
            Step(" o Button: Finish, Cancel");
            VerifyEqual("24. Verify Text: Thank you, you can go to the next light point", "Thank you, you can go to the next light point.", installationPage.InstallationPopupPanel.GetFinishMessageText());
            VerifyEqual("24. Verify Button: Finish is displayed", true, installationPage.InstallationPopupPanel.IsFinishButtonVisible());
            VerifyEqual("24. Verify Button: Cancel is displayed", true, installationPage.InstallationPopupPanel.IsCancelButtonVisible());

            Step("25. Press Left Arrow icon");
            installationPage.InstallationPopupPanel.ClickBackButton();
            installationPage.InstallationPopupPanel.WaitForCommentFormDisplayed();

            Step("26. Verify The pop-up changes to the previous UI with");
            Step(" o Richtextbox with label: Comment. Value unchanged.");
            VerifyEqual("26. Verify Richtextbox with label: Comment", "Comment", installationPage.InstallationPopupPanel.GetCommentText());
            VerifyEqual("26. Verify Value of Comment unchanged", comment, installationPage.InstallationPopupPanel.GetCommentValue());

            Step("27. Press Next, then press Finish");
            installationPage.InstallationPopupPanel.ClickNextButton();
            installationPage.InstallationPopupPanel.WaitForFinishFormDisplayed();
            installationPage.InstallationPopupPanel.ClickFinishButton();
            installationPage.WaitForPreviousActionComplete();
            installationPage.WaitForPopupDialogDisappeared();
           
            Step("28. Verify The new streetlight is added to the map with the green color (it means streetlight is installed successfully) and the white bulb icon on the grey circle (covered SC-1925)");
            VerifyBackgroundIcon(installationPage, streetlight, BgColor.Green);
            VerifyBulbIconIsWhite(installationPage, streetlight);

            try
            {
                DeleteDevice(controllerId, identifier);
                DeleteGeozone(geozone);
            }
            catch {}
        }

        [Test, DynamicRetry]
        [Description("Installation App - SC-377 - Install a new streetlight successfully from Installation app")]
        public void SC_377_03()
        {
            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - Install a new streetlight from Installation app");
            Step("**** Precondition ****\n");

            var testData = GetTestDataOfSC_377_03();
            var controllerId = testData["ControllerId"];
            var controllerName = testData["ControllerName"];
            var geozone = SLVHelper.GenerateUniqueName("GZNSC37703");
            var streetlight = SLVHelper.GenerateUniqueName("STL");

            Step("-> Create data for testing");
            DeleteGeozones("GZNSC37703*");
            CreateNewGeozone(geozone);

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.Installation, App.EquipmentInventory);
            var installationPage = desktopPage.GoToApp(App.Installation) as InstallationPage;
            installationPage.GeozoneTreeMainPanel.SelectNode(geozone);
            string identifier;
            var newStreetlightPath = installationPage.AddNewStreetlight(out identifier, streetlight, "ABEL-Vigilon A[Dimmable ballast]", controllerName);

            Step("1. Go to Installation app");
            Step("2. Verify Installation page is routed and loaded successfully");
            desktopPage = Browser.RefreshLoggedInCMS();
            installationPage = desktopPage.GoToApp(App.Installation) as InstallationPage;

            Step("3. Select the new recreated streetlight");
            installationPage.GeozoneTreeMainPanel.SelectNode(newStreetlightPath);
            installationPage.WaitForPreviousActionComplete();
            installationPage.WaitForPopupDialogDisplayed();
            VerifyEqual("[SC-812] Verify Identifier field is read-only", true, !installationPage.InstallationPopupPanel.IsIdentifierEditable());

            Step("4. Update all the fields with new values, then Press Next button");
            var newTypeOfEquipment = "SSN Cimcon Dim Photocell[Lamp #0]";
            var newAddress1 = SLVHelper.GenerateString();
            var newAddress2 = SLVHelper.GenerateString();
            var newCity = SLVHelper.GenerateString();
            var newZipcode = SLVHelper.GenerateStringInteger(99999);
            installationPage.InstallationPopupPanel.SelectTypeOfEquipmentDropDown(newTypeOfEquipment);
            installationPage.InstallationPopupPanel.EnterAddress1Input(newAddress1);
            installationPage.InstallationPopupPanel.EnterAddress2Input(newAddress2);
            installationPage.InstallationPopupPanel.EnterCityInput(newCity);
            installationPage.InstallationPopupPanel.EnterZipcodeInput(newZipcode);
            installationPage.InstallationPopupPanel.SelectLampIsAccessibleDropDown("No");
            installationPage.InstallationPopupPanel.ClickNextButton();
            installationPage.InstallationPopupPanel.WaitForReasonFormDisplayed();

            Step("5. Verify The pop-up changes to the previous UI with");
            Step(" o Text: Why can't it be installed ?");
            Step(" o Dropdownlist box with label: Reason");
            VerifyEqual("5. Verify Text: Why can't it be installed ?", "Why can't it be installed ?", installationPage.InstallationPopupPanel.GetReasonFormCaptionText());
            VerifyEqual("5. Verify Dropdownlist box with label: Reason", "Reason", installationPage.InstallationPopupPanel.GetReasonText());
            
            Step("6. Select Other for the reason and press Next");
            installationPage.InstallationPopupPanel.SelectReasonDropDown("OTHER");
            installationPage.InstallationPopupPanel.ClickNextButton();
            installationPage.InstallationPopupPanel.WaitForRunTestFormDisplayed();

            Step("7. Verify The pop-up changes to the new UI with");
            Step(" o Text: Test the streetlight");
            Step(" o RUN TEST button next to the streetlight image");
            VerifyEqual("7. Verify Text: Test the streetlight", "Test the streetlight", installationPage.InstallationPopupPanel.GetRunTestFormCaptionText());
            VerifyEqual("7. Verify RUN TEST button next to the streetlight image", true, installationPage.InstallationPopupPanel.IsRunTestButtonVisible());

            Step("8. Press Left Arrow icon");
            installationPage.InstallationPopupPanel.ClickBackButton();
            installationPage.InstallationPopupPanel.WaitForReasonFormDisplayed();

            Step("9. Verify The pop-up changes to the previous UI with");
            Step(" o Dropdownlist box with label: Reason. Value= Other.");
            VerifyEqual("9. Verify Dropdownlist box with label: Reason", "Reason", installationPage.InstallationPopupPanel.GetReasonText());
            VerifyEqual("9. Verify Value of Reason = OTHER", "OTHER", installationPage.InstallationPopupPanel.GetReasonValue());

            Step("10. Clear the reason Other, and input a new reason. E.g. No MAC address. And press Next");
            var newReason = "No Mac Address";
            installationPage.InstallationPopupPanel.SelectReasonDropDown(newReason);
            installationPage.InstallationPopupPanel.ClickNextButton();
            installationPage.InstallationPopupPanel.WaitForRunTestFormDisplayed();

            Step("11. Press RUN TEST button");
            installationPage.InstallationPopupPanel.ClickRunTestButton();
            installationPage.InstallationPopupPanel.WaitForRunTestCompleted();

            Step("12. Verify The error message displays with red text");
            VerifyEqual("12. Verify The error message displays with red text", true, installationPage.InstallationPopupPanel.IsRunTestErrorMessageDisplayed());

            Step("13. Press Next");            
            installationPage.InstallationPopupPanel.ClickNextButton();
            installationPage.InstallationPopupPanel.WaitForCommentFormDisplayed();
           
            Step("14. Verify The pop-up changes to the new UI");
            Step(" o Text: Please add some comment");
            Step(" o Richtextbox with label: Comment.");
            VerifyEqual("14. Verify Text: Please add some comment", "Please add some comment", installationPage.InstallationPopupPanel.GetCommentFormCaptionText());
            VerifyEqual("14. Verify Richtextbox with label: Comment", "Comment", installationPage.InstallationPopupPanel.GetCommentText());
            
            Step("15. Update the new comment and press Next");
            var newComment = SLVHelper.GenerateString();
            installationPage.InstallationPopupPanel.EnterCommentInput(newComment);
            installationPage.InstallationPopupPanel.ClickNextButton();
            installationPage.InstallationPopupPanel.WaitForFinishFormDisplayed();

            Step("16. Verify The pop-up changes to the new UI");
            Step(" o Text: Thank you, you can go to the next light point.");
            Step(" o Button: Finish, Cancel");
            VerifyEqual("16. Verify Text: Thank you, you can go to the next light point", "Thank you, you can go to the next light point.", installationPage.InstallationPopupPanel.GetFinishMessageText());
            VerifyEqual("16. Verify Button: Finish is displayed", true, installationPage.InstallationPopupPanel.IsFinishButtonVisible());
            VerifyEqual("16. Verify Button: Cancel is displayed", true, installationPage.InstallationPopupPanel.IsCancelButtonVisible());
            
            Step("17. Press Finish button");
            installationPage.InstallationPopupPanel.ClickFinishButton();
            installationPage.WaitForPreviousActionComplete();
            installationPage.WaitForPopupDialogDisappeared();

            Step("18. Verify The pop-up is closed");
            VerifyEqual("18. Verify The pop-up is closed", false, installationPage.IsPopupDialogDisplayed());

            Step("19. Select the streetlight again.");
            installationPage.GeozoneTreeMainPanel.SelectNode(streetlight);
            installationPage.WaitForPreviousActionComplete();
            installationPage.WaitForPopupDialogDisplayed();            

            Step("20. Verify all the new values are updated");
            VerifyEqual(string.Format("20. Verify Type of equipment is updated to {0}", newTypeOfEquipment), newTypeOfEquipment, installationPage.InstallationPopupPanel.GetTypeOfEquipmentValue());            
            VerifyEqual(string.Format("20. Verify Address 1 is updated to {0}", newAddress1), newAddress1, installationPage.InstallationPopupPanel.GetAddress1Value());
            VerifyEqual(string.Format("20. Verify Address 2 is updated to {0}", newAddress2), newAddress2, installationPage.InstallationPopupPanel.GetAddress2Value());
            VerifyEqual(string.Format("20. Verify City is updated to {0}", newCity), newCity, installationPage.InstallationPopupPanel.GetCityValue());
            VerifyEqual(string.Format("20. Verify Zip code is updated to {0}", newZipcode), newZipcode, installationPage.InstallationPopupPanel.GetZipcodeValue());

            Step("21. Press Next");
            installationPage.InstallationPopupPanel.ClickNextButton();
            installationPage.InstallationPopupPanel.WaitForReasonFormDisplayed();

            Step("22. Verify the new reason is updated");
            VerifyEqual(string.Format("22. Verify the new reason is updated to {0}", newReason), newReason, installationPage.InstallationPopupPanel.GetReasonValue());

            Step("23. Press Next until Comment screen appears");
            installationPage.InstallationPopupPanel.ClickNextButton();
            installationPage.InstallationPopupPanel.WaitForRunTestFormDisplayed();
            installationPage.InstallationPopupPanel.ClickNextButton();
            installationPage.InstallationPopupPanel.WaitForCommentFormDisplayed();

            Step("24. Verify the new comment is updated");
            VerifyEqual(string.Format("24. Verify the new comment is updated to {0}", newComment), newComment, installationPage.InstallationPopupPanel.GetCommentValue());

            Step("25. Press Finish");
            installationPage.InstallationPopupPanel.ClickNextButton();
            installationPage.InstallationPopupPanel.WaitForFinishFormDisplayed();
            installationPage.InstallationPopupPanel.ClickFinishButton();
            installationPage.WaitForPreviousActionComplete();
            installationPage.WaitForPopupDialogDisappeared();

            Step("26. Verify The pop-up is closed.");
            VerifyEqual("26. Verify The pop-up is closed", false, installationPage.IsPopupDialogDisplayed());
            
            try
            {
                DeleteDevice(controllerId, identifier);
                DeleteGeozone(geozone);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("Installation App -SC-377- Install a new streetlight with 'Lamp came on' option from Installation app")]
        public void SC_377_04()
        {
            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var testData = GetTestDataOfSC_377_04();
            var controllerId = testData["ControllerId"];
            var controllerName = testData["ControllerName"];
            var geozone = SLVHelper.GenerateUniqueName("GZNSC37704");
            var streetlight = SLVHelper.GenerateUniqueName("STL");

            Step("-> Create data for testing");
            DeleteGeozones("GZNSC37704*");
            CreateNewGeozone(geozone);

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.Installation, App.EquipmentInventory);
           
            Step("1. Go to Installation app");
            Step("2. Verify Installation page is routed and loaded successfully");
            var installationPage = desktopPage.GoToApp(App.Installation) as InstallationPage;

            Step("3. Geozone Automation > Free Devices");
            installationPage.GeozoneTreeMainPanel.SelectNode(geozone);            
            
            Step("4. Press Add Streetlight button and drop it on the map");
            installationPage.Map.ClickAddStreetlightButton();
            installationPage.Map.WaitForRecorderDisplayed();
            installationPage.Map.PositionNewDevice();
            installationPage.Map.WaitForRecorderDisappeared();
            installationPage.WaitForPreviousActionComplete();
            installationPage.WaitForPopupDialogDisplayed();           
            VerifyEqual("[SC-812] Verify Identifier field is read-only", true, !installationPage.InstallationPopupPanel.IsIdentifierEditable());

            Step("5. Input all the fields with valid values, then Press Next button");
            var typeOfEquipment = "ABEL-Vigilon A[Dimmable ballast]" ;          
            var address1 = SLVHelper.GenerateString();
            var address2 = SLVHelper.GenerateString();
            var city = SLVHelper.GenerateString();
            var zipcode = SLVHelper.GenerateStringInteger(99999);
            var identifier = installationPage.InstallationPopupPanel.GetIdentifierValue();
            installationPage.InstallationPopupPanel.SelectTypeOfEquipmentDropDown(typeOfEquipment);
            installationPage.InstallationPopupPanel.SelectControllerIdDropDown(controllerName);
            installationPage.InstallationPopupPanel.EnterNameInput(streetlight);
            installationPage.InstallationPopupPanel.EnterAddress1Input(address1);
            installationPage.InstallationPopupPanel.EnterAddress2Input(address2);
            installationPage.InstallationPopupPanel.EnterCityInput(city);
            installationPage.InstallationPopupPanel.EnterZipcodeInput(zipcode);
            installationPage.InstallationPopupPanel.ClickNextButton();
            installationPage.InstallationPopupPanel.WaitForScanQRCodeFormDisplayed();

            Step("6. Input the valid MAC address and press Next");
            var macAddress = SLVHelper.GenerateMACAddress();
            installationPage.InstallationPopupPanel.EnterUniqueAddressInput(macAddress);
            installationPage.InstallationPopupPanel.ClickNextButton();
            installationPage.InstallationPopupPanel.WaitForLightComeOnFormDisplayed();

            Step("7. Verify The pop-up changes to the new UI with");
            Step(" o Text: Did light come ON once node was installed ?");
            Step(" o Checkbox with lable: Lamp came on. Checkbox is unchecked.");
            VerifyEqual("7. Verify Text: Did light come ON once node was installed ?", "Did light come ON once node was installed ?", installationPage.InstallationPopupPanel.GetLightComeOnFormCaptionText());
            VerifyEqual("7. Verify Checkbox with lable: Lamp came on", "Lamp came on", installationPage.InstallationPopupPanel.GetLampCameOnText());

            Step("8. Check Lam come on checkbox, and Next button");
            installationPage.InstallationPopupPanel.TickLampCameOnCheckbox(true);
            installationPage.InstallationPopupPanel.ClickNextButton();
            installationPage.InstallationPopupPanel.WaitForMoreInfoFormDisplayed();

            Step("9. Verify The pop-up changes to the new UI with");
            Step(" o Text: About this light point");
            Step(" o Dropdowlist with label: Lamp Type");
            Step(" o Textbox with label: Lamp wattage (W)");
            Step(" o Dropdowlist with label: Luminaire type");
            Step(" o Dropdowlist with label: Luminaire model");
            Step(" o Dropdowlist with label: Light distribution");
            Step(" o Dropdowlist with label: Orientation");
            Step(" o Textbox with label: Bracket length");
            Step(" o Dropdowlist with label: Pole type");                        
            VerifyEqual("9. Verify Text: About this light point", "About this light point", installationPage.InstallationPopupPanel.GetMoreInfoFormCaptionText());            
            VerifyEqual("9. Verify Dropdowlist with label: Lamp Type", "Lamp Type", installationPage.InstallationPopupPanel.GetLampTypeText());            
            VerifyEqual("9. Verify Textbox with label: Lamp wattage (W)", "Lamp wattage (W)", installationPage.InstallationPopupPanel.GetLampWattageText());
            VerifyEqual("9. Verify Dropdowlist with label: Luminaire type", "Luminaire type", installationPage.InstallationPopupPanel.GetLuminaireTypeText());
            VerifyEqual("9. Verify Dropdowlist with label: Luminaire model", "Luminaire model", installationPage.InstallationPopupPanel.GetLuminaireModelText());
            VerifyEqual("9. Verify Dropdowlist with label: Light distribution", "Light distribution", installationPage.InstallationPopupPanel.GetLightDistributionText());
            VerifyEqual("9. Verify Dropdowlist with label: Orientation", "Orientation", installationPage.InstallationPopupPanel.GetOrientationText());
            VerifyEqual("9. Verify Textbox with label: Bracket length", "Bracket length", installationPage.InstallationPopupPanel.GetBracketLengthText());
            VerifyEqual("9. Verify Dropdowlist with label: Pole type", "Pole type", installationPage.InstallationPopupPanel.GetPoleTypeText());
            
            Step("10. Press Left Arrow icon");
            installationPage.InstallationPopupPanel.ClickBackButton();
            installationPage.InstallationPopupPanel.WaitForLightComeOnFormDisplayed();

            Step("11. Verify The pop-up changes to the previous UI");
            Step("o Checkbox with lable: Lamp came on. Checkbox is checked.");
            VerifyEqual("11. Verify Checkbox with label: Lamp came on", "Lamp came on", installationPage.InstallationPopupPanel.GetLampCameOnText());
            VerifyEqual("11. Verify Checkbox is checked", true, installationPage.InstallationPopupPanel.GetLampCameOnValue());

            Step("12. Press Next and input or select values for the UI, then press Next");
            installationPage.InstallationPopupPanel.ClickNextButton();
            installationPage.InstallationPopupPanel.WaitForMoreInfoFormDisplayed();
            var lampType = "HPS 70W Ferro";
            var lampWattage = SLVHelper.GenerateStringInteger(999);
            var luminaireType = SLVHelper.GenerateString();
            var luminaireModel = SLVHelper.GenerateString();
            var lightDistribution = SLVHelper.GenerateString();
            var orientation = SLVHelper.GenerateString();
            var bracketLength = SLVHelper.GenerateStringInteger(999);
            var poleType = SLVHelper.GenerateString();
            installationPage.InstallationPopupPanel.SelectLampTypeDropDown(lampType);
            installationPage.InstallationPopupPanel.EnterLampWattageNumericInput(lampWattage);
            installationPage.InstallationPopupPanel.SelectLuminaireTypeDropDown(luminaireType);
            installationPage.InstallationPopupPanel.SelectLuminaireModelDropDown(luminaireModel);
            installationPage.InstallationPopupPanel.SelectLightDistributionDropDown(lightDistribution);
            installationPage.InstallationPopupPanel.SelectOrientationDropDown(orientation);
            installationPage.InstallationPopupPanel.SelectBracketLengthDropDown(bracketLength);
            installationPage.InstallationPopupPanel.SelectPoleTypeDropDown(poleType);
            installationPage.InstallationPopupPanel.ClickNextButton();
            installationPage.InstallationPopupPanel.WaitForPhotoFormDisplayed();

            Step("13. Verify The pop-up changes to the new UI");
            Step(" o Text: Take a photo");
            Step(" o Button: photo icon");
            VerifyEqual("13. Verify Text: Take a photo", "Take a photo", installationPage.InstallationPopupPanel.GetTakePhotoFormCaptionText());
            VerifyEqual("13. Verify Button: photo icon displays", true, installationPage.InstallationPopupPanel.IsCameraButtonVisible());

            Step("14. Press Left Arrow icon");
            installationPage.InstallationPopupPanel.ClickBackButton();
            installationPage.InstallationPopupPanel.WaitForMoreInfoFormDisplayed();

            Step("15. Verify The pop-up changes to the previous UI and the values are unchanged");            
            VerifyEqual("15. Verify Text: About this light point", "About this light point", installationPage.InstallationPopupPanel.GetMoreInfoFormCaptionText());
            VerifyEqual("15. Verify Dropdowlist with label: Lamp Type", "Lamp Type", installationPage.InstallationPopupPanel.GetLampTypeText());
            VerifyEqual("15. Verify Lamp Type Value is unchanged", lampType, installationPage.InstallationPopupPanel.GetLampTypeValue());
            VerifyEqual("15. Verify Textbox with label: Lamp wattage (W)", "Lamp wattage (W)", installationPage.InstallationPopupPanel.GetLampWattageText());
            VerifyEqual("15. Verify Lamp wattage (W) Value is unchanged", lampWattage, installationPage.InstallationPopupPanel.GetLampWattageValue());
            VerifyEqual("15. Verify Dropdowlist with label: Luminaire type", "Luminaire type", installationPage.InstallationPopupPanel.GetLuminaireTypeText());
            VerifyEqual("15. Verify Luminaire type Value is unchanged", luminaireType, installationPage.InstallationPopupPanel.GetLuminaireTypeValue());
            VerifyEqual("15. Verify Dropdowlist with label: Luminaire model", "Luminaire model", installationPage.InstallationPopupPanel.GetLuminaireModelText());
            VerifyEqual("15. Verify Luminaire model Value is unchanged", luminaireModel, installationPage.InstallationPopupPanel.GetLuminaireModelValue());
            VerifyEqual("15. Verify Dropdowlist with label: Light distribution", "Light distribution", installationPage.InstallationPopupPanel.GetLightDistributionText());
            VerifyEqual("15. Verify Light distribution Value is unchanged", lightDistribution, installationPage.InstallationPopupPanel.GetLightDistributionValue());
            VerifyEqual("15. Verify Dropdowlist with label: Orientation", "Orientation", installationPage.InstallationPopupPanel.GetOrientationText());
            VerifyEqual("15. Verify Orientation Value is unchanged", orientation, installationPage.InstallationPopupPanel.GetOrientationValue());
            VerifyEqual("15. Verify Textbox with label: Bracket length", "Bracket length", installationPage.InstallationPopupPanel.GetBracketLengthText());
            VerifyEqual("15. Verify Bracket length Value is unchanged", bracketLength, installationPage.InstallationPopupPanel.GetBracketLengthValue());
            VerifyEqual("15. Verify Dropdowlist with label: Pole type", "Pole type", installationPage.InstallationPopupPanel.GetPoleTypeText());
            VerifyEqual("15. Verify Pole type Value is unchanged", poleType, installationPage.InstallationPopupPanel.GetPoleTypeValue());

            Step("16. Press Next 2 times, and input some comment, then press Next");
            installationPage.InstallationPopupPanel.ClickNextButton();
            installationPage.InstallationPopupPanel.WaitForPhotoFormDisplayed();
            installationPage.InstallationPopupPanel.ClickNextButton();
            installationPage.InstallationPopupPanel.WaitForCommentFormDisplayed();
            var comment = SLVHelper.GenerateString(9);
            installationPage.InstallationPopupPanel.EnterCommentInput(comment);
            installationPage.InstallationPopupPanel.ClickNextButton();
            installationPage.InstallationPopupPanel.WaitForFinishFormDisplayed();

            Step("17. Verify The pop-up changes to the new UI");
            Step(" o Text: Thank you, you can go to the next light point");
            Step(" o Button: Finish, Cancel");
            VerifyEqual("17. Verify Text: Thank you, you can go to the next light point", "Thank you, you can go to the next light point.", installationPage.InstallationPopupPanel.GetFinishMessageText());
            VerifyEqual("17. Verify Button: Finish is displayed", true, installationPage.InstallationPopupPanel.IsFinishButtonVisible());
            VerifyEqual("17. Verify Button: Cancel is displayed", true, installationPage.InstallationPopupPanel.IsCancelButtonVisible());

            Step("18. Press Finish");
            installationPage.InstallationPopupPanel.ClickFinishButton();
            installationPage.WaitForPreviousActionComplete();
            installationPage.WaitForPopupDialogDisappeared();

            Step("19. Verify The new streetlight is added to the map with the green color (it means streetlight is installed successfully)");
            VerifyBackgroundIcon(installationPage, streetlight, BgColor.Green);
            
            try
            {
                DeleteDevice(controllerId, identifier);
                DeleteGeozone(geozone);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("Installation App -SC-377- Install a new streetlight failed from Installation app")]
        public void SC_377_05()
        {
            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var testData = GetTestDataOfSC_377_05();
            var controllerId = testData["ControllerId"];
            var controllerName = testData["ControllerName"];
            var geozone = SLVHelper.GenerateUniqueName("GZNSC37705");
            var streetlight = SLVHelper.GenerateUniqueName("STL");

            Step("-> Create data for testing");
            DeleteGeozones("GZNSC37705*");
            CreateNewGeozone(geozone);

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.Installation, App.EquipmentInventory);

            Step("1. Go to Installation app");
            Step("2. Verify Installation page is routed and loaded successfully");
            var installationPage = desktopPage.GoToApp(App.Installation) as InstallationPage;

            Step("3. Geozone Automation > Free Devices");
            installationPage.GeozoneTreeMainPanel.SelectNode(geozone);

            Step("4. Press Add Streetlight button and drop it on the map");
            installationPage.Map.ClickAddStreetlightButton();
            installationPage.Map.WaitForRecorderDisplayed();
            installationPage.Map.PositionNewDevice();
            installationPage.Map.WaitForRecorderDisappeared();
            installationPage.WaitForPreviousActionComplete();
            installationPage.WaitForPopupDialogDisplayed();            
            VerifyEqual("[SC-812] Verify Identifier field is read-only", true, !installationPage.InstallationPopupPanel.IsIdentifierEditable());

            Step("5. Input all mandatory fields (fields with *) in the pop-up and other option fields (Address 1, Address 2, City, Zip code, Lamp is accessible) and press Next button");
            var typeOfEquipment = "ABEL-Vigilon A[Dimmable ballast]";           
            var address1 = SLVHelper.GenerateString();
            var address2 = SLVHelper.GenerateString();
            var city = SLVHelper.GenerateString();
            var zipcode = SLVHelper.GenerateStringInteger(99999);
            var identifier = installationPage.InstallationPopupPanel.GetIdentifierValue();
            installationPage.InstallationPopupPanel.SelectTypeOfEquipmentDropDown(typeOfEquipment);
            installationPage.InstallationPopupPanel.SelectControllerIdDropDown(controllerName);
            installationPage.InstallationPopupPanel.EnterNameInput(streetlight);
            installationPage.InstallationPopupPanel.EnterAddress1Input(address1);
            installationPage.InstallationPopupPanel.EnterAddress2Input(address2);
            installationPage.InstallationPopupPanel.EnterCityInput(city);
            installationPage.InstallationPopupPanel.EnterZipcodeInput(zipcode);
            installationPage.InstallationPopupPanel.SelectLampIsAccessibleDropDown("Yes");
            installationPage.InstallationPopupPanel.ClickNextButton();
            installationPage.InstallationPopupPanel.WaitForScanQRCodeFormDisplayed();

            Step("6. Don't input the MAC address, then press Next button");
            installationPage.InstallationPopupPanel.ClickNextButton();
            installationPage.InstallationPopupPanel.WaitForLightComeOnFormDisplayed();

            Step("7. Don't check the checkbox and press Next");
            installationPage.InstallationPopupPanel.TickLampCameOnCheckbox(false);
            installationPage.InstallationPopupPanel.ClickNextButton();
            installationPage.InstallationPopupPanel.WaitForCommentFormDisplayed();

            Step("8. Input some comment, then press Next");            
            var comment = SLVHelper.GenerateString(9);
            installationPage.InstallationPopupPanel.EnterCommentInput(comment);

            Step("9. Press Next, then press Finish");
            installationPage.InstallationPopupPanel.ClickNextButton();
            installationPage.InstallationPopupPanel.WaitForFinishFormDisplayed();            
            installationPage.InstallationPopupPanel.ClickFinishButton();
            installationPage.WaitForPreviousActionComplete();
            installationPage.WaitForPopupDialogDisappeared();

            Step("10. The new streetlight is added to the map with the orange color (it means streetlight is installed failed) and the white bulb icon on the grey circle (covered SC-1925)");
            VerifyBackgroundIcon(installationPage, streetlight, BgColor.Orange);
            VerifyBulbIconIsWhite(installationPage, streetlight);

            Step("11. Select the new recreated streetlight");
            installationPage.GeozoneTreeMainPanel.SelectNode(streetlight);
            installationPage.WaitForPreviousActionComplete();
            installationPage.WaitForPopupDialogDisplayed();

            Step("12. Verify The pop-up display with");
            Step(" o Title: Streetlight's name");
            Step(" o Text: About this light point");
            Step(" o Textbox with the label: Identifier *. Value saved.");
            Step(" o Dropdownlist box with label: Type of equipment *. Value saved.");
            Step(" o Dropdownlist box with label: Controller ID *. Value saved.");
            Step(" o Textbox with the label: Name *. Value saved.");
            Step(" o Textbox with the label: Address 1. Value saved.");
            Step(" o Textbox with the label: Address 2. Value saved.");
            Step(" o Textbox with the label: City. Value saved.");
            Step(" o Textbox with the label: Zip code. Value saved.");
            Step(" o Dropdownlist box with the label: Lamp is accessible. Value = 'Yes'");
            Step(" o Button: Next, Cancel");
            VerifyEqual("12. Verify the pop-up displays", true, installationPage.IsPopupDialogDisplayed());
            VerifyEqual("12. Verify Title: Streetlight's name", streetlight, installationPage.InstallationPopupPanel.GetPanelTitleText());
            VerifyEqual("12. Verify Text: About this light point", "About this light point", installationPage.InstallationPopupPanel.GetBasicInfoFormCaptionText());
            VerifyEqual("12. Verify Textbox with the label: Identifier *", "Identifier *", installationPage.InstallationPopupPanel.GetIdentifierText());
            VerifyEqual("12. Verify Identifier Value is saved", identifier, installationPage.InstallationPopupPanel.GetIdentifierValue());
            VerifyEqual("12. Verify Dropdownlist box with label: Type of equipment *", "Type of equipment *", installationPage.InstallationPopupPanel.GetTypeOfEquipmentText());
            VerifyEqual("12. Verify Type of equipment Value is saved", typeOfEquipment, installationPage.InstallationPopupPanel.GetTypeOfEquipmentValue());
            VerifyEqual("12. Verify Dropdownlist box with label: Controller ID *", "Controller ID *", installationPage.InstallationPopupPanel.GetControllerIdText());
            VerifyEqual("12. Verify Controller ID Value is saved", controllerName, installationPage.InstallationPopupPanel.GetControllerIdValue());
            VerifyEqual("12. Verify Textbox with the label: Name *", "Name *", installationPage.InstallationPopupPanel.GetNameText());
            VerifyEqual("12. Verify Name Value is saved", streetlight, installationPage.InstallationPopupPanel.GetNameValue());
            VerifyEqual("12. Verify Textbox with the label: Address 1", "Address 1", installationPage.InstallationPopupPanel.GetAddress1Text());
            VerifyEqual("12. Verify Address 1 Value is saved", address1, installationPage.InstallationPopupPanel.GetAddress1Value());
            VerifyEqual("12. Verify Textbox with the label: Address 2", "Address 2", installationPage.InstallationPopupPanel.GetAddress2Text());
            VerifyEqual("12. Verify Address 2 Value is saved", address2, installationPage.InstallationPopupPanel.GetAddress2Value());
            VerifyEqual("12. Verify Textbox with the label: City", "City", installationPage.InstallationPopupPanel.GetCityText());
            VerifyEqual("12. Verify City Value is saved", city, installationPage.InstallationPopupPanel.GetCityValue());
            VerifyEqual("12. Verify Textbox with the label: Zip code", "Zip code", installationPage.InstallationPopupPanel.GetZipcodeText());
            VerifyEqual("12. Verify Zip code Value is saved", zipcode, installationPage.InstallationPopupPanel.GetZipcodeValue());
            VerifyEqual("12. Verify Dropdownlist box with the label: Lamp is accessible", "Lamp is accessible", installationPage.InstallationPopupPanel.GetLampIsAccessibleText());
            VerifyEqual("12. Verify Lamp is accessible Value = 'Yes'", "Yes", installationPage.InstallationPopupPanel.GetLampIsAccessibleValue());
            VerifyEqual("12. Verify Button: Next is displayed", true, installationPage.InstallationPopupPanel.IsNextButtonVisible());
            VerifyEqual("12. Verify Button: Cancel is displayed", true, installationPage.InstallationPopupPanel.IsCancelButtonVisible());

            try
            {                
                DeleteDevice(controllerId, identifier);
                DeleteGeozone(geozone);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [NonParallelizable]
        [Description("Installation App -SC-377- The number and percent of devices installed or to be installed will be updated")]
        public void SC_377_06()
        {
            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - Take note the number and percent of 'installed' and 'to be installed' devices on the Installation app, and the total devices in system displayed on Equipment Inventory tile.");
            Step("**** Precondition ****\n");

            var testData = GetTestDataOfSC_377_06();
            var controllerId = testData["ControllerId"];
            var controllerName = testData["ControllerName"];
            var geozone = SLVHelper.GenerateUniqueName("GZNSC37706");
            var streetlight1 = SLVHelper.GenerateUniqueName("STL01");
            var streetlight2 = SLVHelper.GenerateUniqueName("STL02");

            Step("-> Create data for testing");
            DeleteGeozones("GZNSC37706*");
            CreateNewGeozone(geozone);            

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.Installation, App.EquipmentInventory);
            var notedInstalledDevicesCount = desktopPage.GetInstallationInstalledDevicesCount();
            var notedNoInstalledDevicesCount = desktopPage.GetInstallationNoInstalledDevicesCount();

            Step("1. Go to Installation app");
            Step("2. Verify Installation page is routed and loaded successfully");
            var installationPage = desktopPage.GoToApp(App.Installation) as InstallationPage;

            Step("3. Select a geozone and press Add Streetlight button on the top right corner");
            installationPage.GeozoneTreeMainPanel.SelectNode(geozone);

            Step("4. Input all the fields and press Next");
            Step("5. Input the Unique Address and press Next until finish.");
            string identifier1;
            var newStreetlightPath1 = installationPage.AddNewStreetlight(out identifier1, streetlight1, "ABEL-Vigilon A[Dimmable ballast]", controllerName, SLVHelper.GenerateMACAddress());

            Step("6. Verify The green streetlight displays on the map");
            VerifyBackgroundIcon(installationPage, streetlight1, BgColor.Green);

            Step("7. Refresh the page");
            desktopPage = Browser.RefreshLoggedInCMS();
            var newInstalledDevicesCount1 = desktopPage.GetInstallationInstalledDevicesCount();
            var newNoInstalledDevicesCount1 = desktopPage.GetInstallationNoInstalledDevicesCount();

            Step("8. Verify The number of 'installed' is increased 1 value in the Application tile");
            VerifyEqual("8. Verify The number of 'installed' is increased 1 value in the Application tile", notedInstalledDevicesCount + 1,  newInstalledDevicesCount1);

            Step("9. Go to Installation app");
            installationPage = desktopPage.GoToApp(App.Installation) as InstallationPage;

            Step("10. Select a geozone and press Add Streetlight button on the top right corner");
            installationPage.GeozoneTreeMainPanel.SelectNode(geozone);

            Step("11. Input all the fields and press Next");
            Step("12. Don't input the Unique Address and press Next until finish.");
            string identifier2;
            var newStreetlightPath2 = installationPage.AddNewStreetlight(out identifier2, streetlight2, "ABEL-Vigilon A[Dimmable ballast]", controllerName);

            Step("13. Verify The orange streetlight displays on the map");
            VerifyBackgroundIcon(installationPage, streetlight2, BgColor.Orange);

            Step("14. Refresh the page");
            desktopPage = Browser.RefreshLoggedInCMS();
            var totalDevicesCount = desktopPage.GetEquipmentInventoryDevicesCount();
            var newInstalledDevicesCount2 = desktopPage.GetInstallationInstalledDevicesCount();
            var newNoInstalledDevicesCount2 = desktopPage.GetInstallationNoInstalledDevicesCount();
            var newInstalledDevicesPercent2 = desktopPage.GetInstallationInstalledDevicesPercent();
            var newNoInstalledDevicesPercent2 = desktopPage.GetInstallationNoInstalledDevicesPercent();

            var percentInstalledDevices = ((decimal)newInstalledDevicesCount2 / totalDevicesCount) * 100;
            var percentInstalledDevicesStr = string.Format("{0:N1}%", percentInstalledDevices);
            var percentNoInstalledDevices = ((decimal)newNoInstalledDevicesCount2 / totalDevicesCount) * 100;
            var percentNoInstalledDevicesStr = string.Format("{0:N1}%", percentNoInstalledDevices);

            Step("19. Verify");
            Step(" o The number of 'To be installed' is increased 1 value in the Application tile");
            Step(" o The percent of 'installed' is equal to (100*[the quantity of installed devices] / Total devices in cms)");
            Step(" o The percent of 'To be installed' is equal to (100*[the quantity of 'to be installed devices'] / Total devices in cms)");
            VerifyEqual("[SC-2020] 19. Verify The number of 'To be installed' is increased 1 value in the Application tile", newNoInstalledDevicesCount1 + 1, newNoInstalledDevicesCount2);            
            VerifyEqual("19. Verify The percent of 'installed' is equal to (100*[the quantity of installed devices] / Total devices in cms)", percentInstalledDevicesStr, newInstalledDevicesPercent2);
            VerifyEqual("19. Verify The percent of 'To be installed' is equal to (100*[the quantity of 'to be installed devices'] / Total devices in cms)", percentNoInstalledDevicesStr, newNoInstalledDevicesPercent2);

            try
            {
                DeleteDevice(controllerId, identifier1);
                DeleteDevice(controllerId, identifier2);
                DeleteGeozone(geozone);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("SC-1890 - Installation - Status filter tab not visible")]
        public void SC_1890()
        {
            var dicData = new Dictionary<string, DeviceInstallStatus>();
            var geozone = SLVHelper.GenerateUniqueName("GZNSC1890");
            var controller = SLVHelper.GenerateUniqueName("CTRL");
            var streetlight1 = SLVHelper.GenerateUniqueName("STL01");
            var streetlight2 = SLVHelper.GenerateUniqueName("STL02");
            var streetlight3 = SLVHelper.GenerateUniqueName("STL03");
            var streetlight4 = SLVHelper.GenerateUniqueName("STL04");
            var streetlight5 = SLVHelper.GenerateUniqueName("STL05");
            var streetlight6 = SLVHelper.GenerateUniqueName("STL06");
            var streetlight7 = SLVHelper.GenerateUniqueName("STL07");
            var streetlight8 = SLVHelper.GenerateUniqueName("STL08");

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - Create a testing geozone and add 8 streetlights with each containing a type of Install Status:");
            Step("  + '-'");
            Step("  + To be verified");
            Step("  + Verified");
            Step("  + New");
            Step("  + Does not exist");
            Step("  + To be installed");
            Step("  + Installed");
            Step("  + Removed");
            Step("**** Precondition ****\n");

            Step("-> Create data for testing");
            DeleteGeozones("GZNSC1890*");
            var eventTime = Settings.GetServerTime();
            CreateNewGeozone(geozone, latMin: "4.63862", latMax: "4.64108", lngMin: "102.44929", lngMax: "102.45444");
            CreateNewController(controller, geozone);
            CreateNewDevice(DeviceType.Streetlight, streetlight1, controller, geozone, lat: SLVHelper.GenerateCoordinate("4.63922", "4.64066"), lng: SLVHelper.GenerateCoordinate("102.45081", "102.45265"));
            CreateNewDevice(DeviceType.Streetlight, streetlight2, controller, geozone, lat: SLVHelper.GenerateCoordinate("4.63922", "4.64066"), lng: SLVHelper.GenerateCoordinate("102.45081", "102.45265"));
            CreateNewDevice(DeviceType.Streetlight, streetlight3, controller, geozone, lat: SLVHelper.GenerateCoordinate("4.63922", "4.64066"), lng: SLVHelper.GenerateCoordinate("102.45081", "102.45265"));
            CreateNewDevice(DeviceType.Streetlight, streetlight4, controller, geozone, lat: SLVHelper.GenerateCoordinate("4.63922", "4.64066"), lng: SLVHelper.GenerateCoordinate("102.45081", "102.45265"));
            CreateNewDevice(DeviceType.Streetlight, streetlight5, controller, geozone, lat: SLVHelper.GenerateCoordinate("4.63922", "4.64066"), lng: SLVHelper.GenerateCoordinate("102.45081", "102.45265"));
            CreateNewDevice(DeviceType.Streetlight, streetlight6, controller, geozone, lat: SLVHelper.GenerateCoordinate("4.63922", "4.64066"), lng: SLVHelper.GenerateCoordinate("102.45081", "102.45265"));
            CreateNewDevice(DeviceType.Streetlight, streetlight7, controller, geozone, lat: SLVHelper.GenerateCoordinate("4.63922", "4.64066"), lng: SLVHelper.GenerateCoordinate("102.45081", "102.45265"));
            CreateNewDevice(DeviceType.Streetlight, streetlight8, controller, geozone, lat: SLVHelper.GenerateCoordinate("4.63922", "4.64066"), lng: SLVHelper.GenerateCoordinate("102.45081", "102.45265"));
            dicData.Add(streetlight1, DeviceInstallStatus.None);
            SetValueToDevice(controller, streetlight1, "installStatus", DeviceInstallStatus.None.Value, eventTime);
            dicData.Add(streetlight2, DeviceInstallStatus.ToBeVerified);
            SetValueToDevice(controller, streetlight2, "installStatus", DeviceInstallStatus.ToBeVerified.Value, eventTime);
            dicData.Add(streetlight3, DeviceInstallStatus.Verified);
            SetValueToDevice(controller, streetlight3, "installStatus", DeviceInstallStatus.Verified.Value, eventTime);
            dicData.Add(streetlight4, DeviceInstallStatus.New);
            SetValueToDevice(controller, streetlight4, "installStatus", DeviceInstallStatus.New.Value, eventTime);
            dicData.Add(streetlight5, DeviceInstallStatus.DoesNotExist);
            SetValueToDevice(controller, streetlight5, "installStatus", DeviceInstallStatus.DoesNotExist.Value, eventTime);
            dicData.Add(streetlight6, DeviceInstallStatus.ToBeInstalled);
            SetValueToDevice(controller, streetlight6, "installStatus", DeviceInstallStatus.ToBeInstalled.Value, eventTime);
            dicData.Add(streetlight7, DeviceInstallStatus.Installed);
            SetValueToDevice(controller, streetlight7, "installStatus", DeviceInstallStatus.Installed.Value, eventTime);
            dicData.Add(streetlight8, DeviceInstallStatus.Removed);
            SetValueToDevice(controller, streetlight8, "installStatus", DeviceInstallStatus.Removed.Value, eventTime);

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.Installation);

            Step("1. Go to Installation app");
            Step("2. Verify Installation page is routed and loaded successfully");
            var installationPage = desktopPage.GoToApp(App.Installation) as InstallationPage;

            Step("3. Select the testing geozone, then press Filter and select the Status tab");
            installationPage.GeozoneTreeMainPanel.SelectNode(geozone);
            Wait.ForSeconds(30); //Waiting for color of streetlights shown
            installationPage.GeozoneTreeMainPanel.ClickFilterButton();
            installationPage.GeozoneTreeMainPanel.WaitForFilterPanelDisplayed();
            installationPage.GeozoneTreeMainPanel.FiltersPanel.SelectTab("Status");

            Step("4. Verify The status tab displays with");
            Step(" o Checkbox columns");
            Step(" o Color column");
            Step(" o Name of the status column");
            var deviceStatusColorList = installationPage.GeozoneTreeMainPanel.FiltersPanel.GetListOfDeviceStatusColor();
            var deviceStatusNameList = installationPage.GeozoneTreeMainPanel.FiltersPanel.GetListOfDeviceStatusName();
            VerifyEqual("4. Verify The status tab displays with Checkbox columns", true, installationPage.GeozoneTreeMainPanel.FiltersPanel.GetDeviceStatusCheckboxRowCount() == 8);
            VerifyEqual("4. Verify The status tab displays with Color columns", true, deviceStatusColorList.Count == 8);
            VerifyEqual("4. Verify The status tab displays with Name of the status column", true, deviceStatusNameList.Count == 8);

            Step("5. Verify Each status has an unique color");
            Step(" o '-': Grey");
            Step(" o To be verified: Lighter Blue");
            Step(" o Verified: Blue");
            Step(" o New: Dark Grey");
            Step(" o Does not exist: Yellow");
            Step(" o To be installed: Orange");
            Step(" o Installed: Green");
            Step(" o Removed: Red");
            var dicDeviceStatus = installationPage.GeozoneTreeMainPanel.FiltersPanel.GetDictionaryOfDeviceStatus();
            VerifyEqual("5. Verify status '-' is Grey", DeviceInstallStatus.None.Color, dicDeviceStatus[DeviceInstallStatus.None.Value]);
            VerifyEqual("5. Verify status 'To be verified' is Grey", DeviceInstallStatus.ToBeVerified.Color, dicDeviceStatus[DeviceInstallStatus.ToBeVerified.Value]);
            VerifyEqual("5. Verify status 'Verified' is Lighter Blue", DeviceInstallStatus.Verified.Color, dicDeviceStatus[DeviceInstallStatus.Verified.Value]);
            VerifyEqual("5. Verify status 'New' is Dark Grey", DeviceInstallStatus.New.Color, dicDeviceStatus[DeviceInstallStatus.New.Value]);
            VerifyEqual("5. Verify status 'Does not exist' is Yellow", DeviceInstallStatus.DoesNotExist.Color, dicDeviceStatus[DeviceInstallStatus.DoesNotExist.Value]);
            VerifyEqual("5. Verify status 'To be installed' is Orange", DeviceInstallStatus.ToBeInstalled.Color, dicDeviceStatus[DeviceInstallStatus.ToBeInstalled.Value]);
            VerifyEqual("5. Verify status 'Installed' is Green", DeviceInstallStatus.Installed.Color, dicDeviceStatus[DeviceInstallStatus.Installed.Value]);
            VerifyEqual("5. Verify status 'Removed' is Red", DeviceInstallStatus.Removed.Color, dicDeviceStatus[DeviceInstallStatus.Removed.Value]);

            Step("6. Check only '-' status");
            installationPage.GeozoneTreeMainPanel.FiltersPanel.UncheckAllStatus();
            installationPage.GeozoneTreeMainPanel.FiltersPanel.CheckStatus(DeviceInstallStatus.None.Value);

            Step("7. Verify There is only 1 streetlight on the map. The streetlight is in Grey color");
            var devices = installationPage.Map.GetDevicesDisplayed();
            VerifyEqual("7. Verify There is only 1 streetlight on the map", 1, devices.Count);
            VerifyBackgroundIcon(installationPage, devices.FirstOrDefault(), BgColor.LightGray, false);

            Step("8. Check randomly 4 statuses more");
            installationPage.GeozoneTreeMainPanel.FiltersPanel.UncheckAllStatus();
            var statusList = DeviceInstallStatus.GetList();
            statusList.Remove(DeviceInstallStatus.None.Value);
            var rdmStatuses = statusList.PickRandom(4).ToArray();
            installationPage.GeozoneTreeMainPanel.FiltersPanel.CheckStatus(rdmStatuses);

            Step("9. Verify The map displays 4 streetlight matched selected statuses");
            Step(" o The 4 streetlights matched the colors of selected status on the map");
            dicDeviceStatus = installationPage.GeozoneTreeMainPanel.FiltersPanel.GetDictionaryOfDeviceStatus();
            devices = installationPage.Map.GetDevicesDisplayed();
            VerifyEqual("9. Verify The map displays 4 streetlight matched selected statuses", 4, devices.Count);
            devices.Remove(streetlight1);
            foreach (var device in devices)
            {
                Step(string.Format("-> Check color of {0}", device));
                var status = dicData[device];
                VerifyEqual(string.Format("9. Verify '{0}' status is {1} in map matched selected in Status tab", device, status.BgColor), status.Color, dicDeviceStatus[status.Value]);
                VerifyBackgroundIcon(installationPage, device, dicData[device].BgColor, false);
            }

            Step("10. Delected all selected streetlights");
            installationPage.GeozoneTreeMainPanel.FiltersPanel.UncheckAllStatus();

            Step("11. Verify The map is empty");
            devices = installationPage.Map.GetDevicesDisplayed();
            VerifyEqual("11. Verify The map is empty", 0, devices.Count);

            Step("12. Select all streetlights");
            installationPage.GeozoneTreeMainPanel.FiltersPanel.CheckAllStatus();

            Step("13. Verify All 8 streetlights display on the map");
            Step(" o The 8 streetlights matched the colors of selected status on the map");
            dicDeviceStatus = installationPage.GeozoneTreeMainPanel.FiltersPanel.GetDictionaryOfDeviceStatus();
            devices = installationPage.Map.GetDevicesDisplayed();
            VerifyEqual("13. Verify The map displays 8 streetlight matched selected statuses", 8, devices.Count);
            foreach (var device in devices)
            {
                Step(string.Format("-> Check color of {0}", device));
                var status = dicData[device];
                VerifyEqual(string.Format("13. Verify '{0}' status is {1} in map matched selected in Status tab", device, status.BgColor), status.Color, dicDeviceStatus[status.Value]);
                VerifyBackgroundIcon(installationPage, device, dicData[device].BgColor, false);
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

        /// <summary>
        /// Verify background icon of a streetlight with specific color
        /// </summary>
        /// <param name="page"></param>
        /// <param name="streetlightName"></param>
        /// <param name="color"></param>
        private void VerifyBackgroundIcon(InstallationPage page, string streetlightName, BgColor color, bool isDeviceFocus = true)
        {
            if (isDeviceFocus)
            {
                page.GeozoneTreeMainPanel.SelectNode(streetlightName);
                page.WaitForPreviousActionComplete();
                page.WaitForPopupDialogDisplayed();
                page.InstallationPopupPanel.ClickCancelButton();
                page.WaitForPopupDialogDisappeared();
            }

            var expectedImgBgBytes = ImageUtility.GetStreetlightBgIconBytes(color);
            var actualImgBgBytes = page.Map.GetStreetlightBgIconBytes(streetlightName);
            if (actualImgBgBytes.Length == 0 && color == BgColor.LightGray)
            {
                VerifyEqual(string.Format("Verify background icon of streetlight '{0}' is {1}", streetlightName, color.ToString()), true, true);
                return;
            }
            var result = ImageUtility.Compare(expectedImgBgBytes, actualImgBgBytes);
            var isTrue = result <= 160;
            VerifyEqual(string.Format("Verify background icon of streetlight '{0}' is {1}", streetlightName, color.ToString()), true, isTrue);
        }

        /// <summary>
        /// Verify Bulb icon of a streetlight is white
        /// </summary>
        /// <param name="page"></param>
        /// <param name="streetlightName"></param>
        /// <param name="color"></param>
        private void VerifyBulbIconIsWhite(InstallationPage page, string streetlightName)
        {
            var expectedImgBgBytes = DeviceType.Streetlight.GetIconBytes("white");
            var actualImgBgBytes = page.Map.GetStreetlightBulbIconBytes(streetlightName);

            var result = ImageUtility.Compare(expectedImgBgBytes, actualImgBgBytes);
            var isTrue = result == 0;
            if (!Browser.Name.Equals("Chrome")) isTrue = result < 100;
            VerifyEqual("[SC-1925] Verify icon of a streetlight is white on the grey circle", true, isTrue);
        }

        #endregion //Verify methods

        #region Input XML data        

        /// <summary>
        /// Get test data of SC_377_02
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfSC_377_02()
        {
            var testCaseName = "SC_377_02";
            var xmlUtility = new XmlUtility(Settings.INSTALLATION_TEST_DATA_FILE_PATH);
            var testData = new Dictionary<string, string>();
            
            var controllerInfo = xmlUtility.GetSingleNode(string.Format(Settings.INSTALLATION_XPATH_PREFIX, testCaseName, "Controller"));
            testData.Add("ControllerId", controllerInfo.GetAttrVal("id"));
            testData.Add("ControllerName", controllerInfo.GetAttrVal("name"));

            return testData;
        }

        /// <summary>
        /// Get test data of SC_377_03
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfSC_377_03()
        {
            var testCaseName = "SC_377_03";
            var xmlUtility = new XmlUtility(Settings.INSTALLATION_TEST_DATA_FILE_PATH);
            var testData = new Dictionary<string, string>();

            var controllerInfo = xmlUtility.GetSingleNode(string.Format(Settings.INSTALLATION_XPATH_PREFIX, testCaseName, "Controller"));
            testData.Add("ControllerId", controllerInfo.GetAttrVal("id"));
            testData.Add("ControllerName", controllerInfo.GetAttrVal("name"));

            return testData;
        }

        /// <summary>
        /// Get test data of SC_377_04
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfSC_377_04()
        {
            var testCaseName = "SC_377_04";
            var xmlUtility = new XmlUtility(Settings.INSTALLATION_TEST_DATA_FILE_PATH);
            var testData = new Dictionary<string, string>();
            
            var controllerInfo = xmlUtility.GetSingleNode(string.Format(Settings.INSTALLATION_XPATH_PREFIX, testCaseName, "Controller"));
            testData.Add("ControllerId", controllerInfo.GetAttrVal("id"));
            testData.Add("ControllerName", controllerInfo.GetAttrVal("name"));

            return testData;
        }

        /// <summary>
        /// Get test data of SC_377_05
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfSC_377_05()
        {
            var testCaseName = "SC_377_05";
            var xmlUtility = new XmlUtility(Settings.INSTALLATION_TEST_DATA_FILE_PATH);
            var testData = new Dictionary<string, string>();
            
            var controllerInfo = xmlUtility.GetSingleNode(string.Format(Settings.INSTALLATION_XPATH_PREFIX, testCaseName, "Controller"));
            testData.Add("ControllerId", controllerInfo.GetAttrVal("id"));
            testData.Add("ControllerName", controllerInfo.GetAttrVal("name"));

            return testData;
        }

        /// <summary>
        /// Get test data of SC_377_06
        /// <returns></returns>
        private Dictionary<string, string> GetTestDataOfSC_377_06()
        {
            var testCaseName = "SC_377_06";
            var xmlUtility = new XmlUtility(Settings.INSTALLATION_TEST_DATA_FILE_PATH);
            var testData = new Dictionary<string, string>();

            var controllerInfo = xmlUtility.GetSingleNode(string.Format(Settings.INSTALLATION_XPATH_PREFIX, testCaseName, "Controller"));
            testData.Add("ControllerId", controllerInfo.GetAttrVal("id"));
            testData.Add("ControllerName", controllerInfo.GetAttrVal("name"));

            return testData;
        }        

        #endregion //Input XML data       

        #endregion //Private methods
    }
}