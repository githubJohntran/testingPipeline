using NUnit.Framework;
using StreetlightVision.Extensions;
using StreetlightVision.Models;
using StreetlightVision.Pages;
using StreetlightVision.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace StreetlightVision.Tests.Coverage.Apps
{
    [TestFixture]
    [NonParallelizable]
    public class SchedulingManagerAppTests : TestBase
    {
        #region Variables

        #endregion //Variables

        #region Contructors

        #endregion //Contructors        

        #region Test Cases

        [Test, DynamicRetry]
        [Description("SM_01 Control program - Basic operations - Basic ON-OFF template")]
        public void SM_01()
        {
            var expectedTemplates = GetTemplates();
            var template = "Astro ON/OFF";
            var random = new Random();

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.SchedulingManager);

            Step("1. Go to Scheduling Manager app");
            Step("2. Expected Scheduling Manager is routed and loaded successfully");
            var schedulingManagerPage = desktopPage.GoToApp(App.SchedulingManager) as SchedulingManagerPage;

            Step("3. Select 'Control program' tab in left panel");
            schedulingManagerPage.SchedulingManagerPanel.SelectTab("Control program");
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("4. Click Add new control program button");
            schedulingManagerPage.SchedulingManagerPanel.ClickAddControlProgramButton();
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("5. Expected");
            Step(" - A new item is orderedly inserted into Control program grid");
            Step("  + Color: a random color");
            Step("  + Name: pattern 'New control program \\d{1,}'");
            Step("  + Geozones: GeoZones");
            var newControlProgramNameInGrid = schedulingManagerPage.SchedulingManagerPanel.GetSelectedControlProgramName();
            var newControlProgramColorInGrid = schedulingManagerPage.SchedulingManagerPanel.GetSelectedControlProgramColor();
            var newControlProgramGeozoneInGrid = schedulingManagerPage.SchedulingManagerPanel.GetSelectedControlProgramGeozone();            
            VerifyEqual("5. Verify Color: a random color", true, newControlProgramColorInGrid != Color.Empty);
            VerifyEqual("5. Verify Name: pattern 'New control program \\d{1,}'", true, Regex.IsMatch(schedulingManagerPage.SchedulingManagerPanel.GetSelectedControlProgramName(), @"New control program \d{1,}"));
            VerifyEqual("5. Verify Geozones: GeoZones", "GeoZones", newControlProgramGeozoneInGrid);

            Step(" - Control program editor panel is refreshed for the new item:");
            Step("  + Name and color should be the same in grid");
            Step("  + Description: empty");
            var newControlProgramName = schedulingManagerPage.ControlProgramEditorPanel.GetNameValue();
            var newControlProgramColor = schedulingManagerPage.ControlProgramEditorPanel.GetChartColorValue();
            var newControlProgramDescription = schedulingManagerPage.ControlProgramEditorPanel.GetDescriptionValue();
            VerifyEqual("5. Verify Name should be the same in grid", newControlProgramNameInGrid, newControlProgramName);
            VerifyEqual("5. Verify Color should be the same in grid", newControlProgramColorInGrid, newControlProgramColor);
            VerifyEqual("5. Verify Description: empty", string.Empty, newControlProgramDescription);

            Step(" - Template dropdown list has items:");
            Step("  + Astro ON/OFF");
            Step("  + Astro ON/OFF and fixed time events");
            Step("  + Always ON");
            Step("  + Always OFF");
            Step("  + Day fixed time events");
            Step("  + Advanced mode");
            var templateItems = schedulingManagerPage.ControlProgramEditorPanel.GetListOfTemplateItems();
            VerifyEqual("5. Verify Template dropdown list has items as expected", expectedTemplates, templateItems, false);

            Step("6. Select template 'Astro ON/OFF'");
            schedulingManagerPage.ControlProgramEditorPanel.SelectTemplateDropDown(template);

            Step("7. Expected Details are updated according to the template:");
            Step(" - Control program table button is not displayed");
            VerifyEqual("7. Verify Control program table button is not displayed", false, schedulingManagerPage.ControlProgramEditorPanel.IsControlProgramItemsButtonVisible());

            Step(" - Switch ON:");
            Step("  + Minutes: numeric = 0");
            Step("  + After/Before field: dropdown list = 'After'");
            Step("  + Sunset: readonly dropdown list = 'Sunset'");
            Step("  + Dimming level: percentage numeric = '100%'");
            VerifyInitialSwithOnGroup(schedulingManagerPage);

            Step(" - Switch OFF:");
            Step("  + Minutes: numeric = 0");
            Step("  + After/Before field: dropdown list = 'After'");
            Step("  + Sunrise: readonly dropdown list = 'Sunrise'");
            Step("  + Dimming level: readonly input with default value = '0%'");
            VerifyInitialSwithOffGroup(schedulingManagerPage);

            Step(" - Variations: is not displayed");
            VerifyEqual("7. Verify Variations: is not displayed", false, schedulingManagerPage.ControlProgramEditorPanel.IsVariationsGroupVisible());

            Step(" - Timeline: is not displayed");
            VerifyEqual("7. Verify Timeline: is not displayed", false, schedulingManagerPage.ControlProgramEditorPanel.IsTimelineVisible());

            Step(" - Chart: sunset & sunrise points reflect dimming level. To verify this, hover the mouse on these points. When each point is hovered, 2 label appears: 1 is on the left with dimming level in percentage; 1 is at the bottom reflects the minutes, if After is being selected, the value is prefixed with '+' sign, if Before, prefixed with '-' in case Minutes is not 0; prefixed with '-' always in case Minutes is 0");
            schedulingManagerPage.ControlProgramEditorPanel.MoveToSunsetDot();
            VerifyChartTooltip(schedulingManagerPage, schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOnLevelValue(), GetTooltipTimeWithRelation(schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOnRelationValue(), int.Parse(schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOnMinuteValue())));
            schedulingManagerPage.ControlProgramEditorPanel.MoveToSunriseDot();
            VerifyChartTooltip(schedulingManagerPage, schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOffLevelValue(), GetTooltipTimeWithRelation(schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOffRelationValue(), int.Parse(schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOffMinuteValue())));

            Step("8. Change all editable fields");
            var newName = SLVHelper.GenerateUniqueName("CPSM11");
            var newColor = schedulingManagerPage.ControlProgramEditorPanel.GetChartColorValue();
            var newDescription = "Any description 1";
            var newSwitchOnMinute = random.Next(1, 59).ToString();
            var newSwitchOnRelation = "Before";
            var newSwitchOnLevel = random.Next(1, 99).ToString();
            var newSwitchOffMinute = random.Next(1, 59).ToString();
            var newSwitchOffRelation = "Before";
            EnterBasicValues(schedulingManagerPage, newName, newDescription);
            EnterSwitchOnGroupValues(schedulingManagerPage, switchOnMinute: newSwitchOnMinute, switchOnRelation: newSwitchOnRelation, switchOnLevel: newSwitchOnLevel);
            EnterSwitchOffGroupValues(schedulingManagerPage, switchOffMinute: newSwitchOffMinute, switchOffRelation: newSwitchOffRelation);

            Step("9. Expected The chart refreshes its displaying (see chart verifying in previous step)");
            schedulingManagerPage.ControlProgramEditorPanel.MoveToSunsetDot();
            VerifyChartTooltip(schedulingManagerPage, schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOnLevelValue(), GetTooltipTimeWithRelation(schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOnRelationValue(), int.Parse(schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOnMinuteValue())));
            schedulingManagerPage.ControlProgramEditorPanel.MoveToSunriseDot();
            VerifyChartTooltip(schedulingManagerPage, schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOffLevelValue(), GetTooltipTimeWithRelation(schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOffRelationValue(), int.Parse(schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOffMinuteValue())));

            Step("10. Change all editable fields again");
            newName = SLVHelper.GenerateUniqueName("CPSM12");
            newDescription = "Any description 2";
            newSwitchOnMinute = random.Next(1, 59).ToString();
            newSwitchOnRelation = "After";
            newSwitchOnLevel = random.Next(1, 99).ToString();
            newSwitchOffMinute = random.Next(1, 59).ToString();
            newSwitchOffRelation = "After";
            EnterBasicValues(schedulingManagerPage, newName, newDescription);
            EnterSwitchOnGroupValues(schedulingManagerPage, switchOnMinute: newSwitchOnMinute, switchOnRelation: newSwitchOnRelation, switchOnLevel: newSwitchOnLevel);
            EnterSwitchOffGroupValues(schedulingManagerPage, switchOffMinute: newSwitchOffMinute, switchOffRelation: newSwitchOffRelation);

            Step("11. Expected The chart refreshes its displaying (see chart verifying in previous step)");
            schedulingManagerPage.ControlProgramEditorPanel.MoveToSunsetDot();
            VerifyChartTooltip(schedulingManagerPage, schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOnLevelValue(), GetTooltipTimeWithRelation(schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOnRelationValue(), int.Parse(schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOnMinuteValue())));
            schedulingManagerPage.ControlProgramEditorPanel.MoveToSunriseDot();
            VerifyChartTooltip(schedulingManagerPage, schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOffLevelValue(), GetTooltipTimeWithRelation(schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOffRelationValue(), int.Parse(schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOffMinuteValue())));

            Step("12. Save the new control program");
            schedulingManagerPage.ControlProgramEditorPanel.ClickSaveButton();
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("13. Expected The template field becomes readonly");
            VerifyEqual("13. Verify The template field becomes readonly", true, schedulingManagerPage.ControlProgramEditorPanel.IsTemplateDropDownReadOnly());

            Step("14. Reload browser and go to Scheduling Manager again");
            desktopPage = Browser.RefreshLoggedInCMS();
            schedulingManagerPage = desktopPage.GoToApp(App.SchedulingManager) as SchedulingManagerPage;

            Step("15. Select 'Control program' tab");
            schedulingManagerPage.SchedulingManagerPanel.SelectTab("Control program");
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("16. Expected The newly-created control program is still present in the grid");
            VerifyEqual("[SC-567] 16. Verify The newly-created control program is still present in the grid", true, schedulingManagerPage.SchedulingManagerPanel.IsControlProgramPresentInGrid(newName));

            Step("17. Select it");
            schedulingManagerPage.SchedulingManagerPanel.SelectControlProgram(newName);
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("18. Expected");
            Step(" - Control program editor panel is refreshed for the new item: all values should be remained");
            VerifyBasicValues(schedulingManagerPage, newName, newDescription, template);
            VerifySwitchOnGroupValues(schedulingManagerPage, switchOnMinute: newSwitchOnMinute, switchOnRelation: newSwitchOnRelation, switchOnLevel: string.Format("{0}%", newSwitchOnLevel));
            VerifySwitchOffGroupValues(schedulingManagerPage, switchOffMinute: newSwitchOffMinute, switchOffRelation: newSwitchOffRelation);

            Step(" - Template field is readonly");
            VerifyEqual("Verify Template field is readonly", true, schedulingManagerPage.ControlProgramEditorPanel.IsTemplateDropDownReadOnly());
            Step(" - Verify the chart (see its verifying in previous step)");
            schedulingManagerPage.ControlProgramEditorPanel.MoveToSunsetDot();
            VerifyChartTooltip(schedulingManagerPage, schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOnLevelValue(), GetTooltipTimeWithRelation(schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOnRelationValue(), int.Parse(schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOnMinuteValue())));
            schedulingManagerPage.ControlProgramEditorPanel.MoveToSunriseDot();
            VerifyChartTooltip(schedulingManagerPage, schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOffLevelValue(), GetTooltipTimeWithRelation(schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOffRelationValue(), int.Parse(schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOffMinuteValue())));

            Step("19. Change all editable fields again then save");
            newName = SLVHelper.GenerateUniqueName("CPSM13");
            newDescription = "Any description 3";
            newSwitchOnMinute = random.Next(1, 59).ToString();
            newSwitchOnRelation = "Before";
            newSwitchOnLevel = random.Next(1, 99).ToString();
            newSwitchOffMinute = random.Next(1, 59).ToString();
            newSwitchOffRelation = "After";
            EnterBasicValues(schedulingManagerPage, newName, newDescription);
            EnterSwitchOnGroupValues(schedulingManagerPage, switchOnMinute: newSwitchOnMinute, switchOnRelation: newSwitchOnRelation, switchOnLevel: newSwitchOnLevel);
            EnterSwitchOffGroupValues(schedulingManagerPage, switchOffMinute: newSwitchOffMinute, switchOffRelation: newSwitchOffRelation);

            schedulingManagerPage.ControlProgramEditorPanel.ClickSaveButton();
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("20. Reload browser and go to Scheduling Manager again");
            desktopPage = Browser.RefreshLoggedInCMS();
            schedulingManagerPage = desktopPage.GoToApp(App.SchedulingManager) as SchedulingManagerPage;

            Step("21. Select 'Control program' tab");
            schedulingManagerPage.SchedulingManagerPanel.SelectTab("Control program");
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("22. Expected The newly-created control program is still present in the grid");
            VerifyEqual("[SC-567] 22. Verify The newly-created control program is still present in the grid", true, schedulingManagerPage.SchedulingManagerPanel.IsControlProgramPresentInGrid(newName));

            Step("23. Select it");
            schedulingManagerPage.SchedulingManagerPanel.SelectControlProgram(newName);
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("24. Expected");
            Step(" - Control program editor panel is refreshed for the new item: all values should be remained");
            VerifyBasicValues(schedulingManagerPage, newName, newDescription, template);
            VerifySwitchOnGroupValues(schedulingManagerPage, switchOnMinute: newSwitchOnMinute, switchOnRelation: newSwitchOnRelation, switchOnLevel: string.Format("{0}%", newSwitchOnLevel));
            VerifySwitchOffGroupValues(schedulingManagerPage, switchOffMinute: newSwitchOffMinute, switchOffRelation: newSwitchOffRelation);

            Step(" - Template field is readonly");
            VerifyEqual("Verify Template field is readonly", true, schedulingManagerPage.ControlProgramEditorPanel.IsTemplateDropDownReadOnly());
            Step(" - Verify the chart (see its verifying in previous step)");
            schedulingManagerPage.ControlProgramEditorPanel.MoveToSunsetDot();
            VerifyChartTooltip(schedulingManagerPage, schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOnLevelValue(), GetTooltipTimeWithRelation(schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOnRelationValue(), int.Parse(schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOnMinuteValue())));
            schedulingManagerPage.ControlProgramEditorPanel.MoveToSunriseDot();
            VerifyChartTooltip(schedulingManagerPage, schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOffLevelValue(), GetTooltipTimeWithRelation(schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOffRelationValue(), int.Parse(schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOffMinuteValue())));

            Step("25. Click Delete button from Control program grid");
            schedulingManagerPage.SchedulingManagerPanel.ClickDeleteControlProgramButton();

            Step("26. Expected A confirmation dialog appears with message 'Do you want to delete the control program '{{Control program name}}' ?'");
            schedulingManagerPage.WaitForPopupDialogDisplayed();
            var expectedMessage = string.Format("Do you want to delete the control program '{0}' ?", newName);
            VerifyEqual(string.Format("26. Verify A confirmation dialog appears with message '{0}'", expectedMessage), expectedMessage, schedulingManagerPage.Dialog.GetMessageText());

            Step("27. Click No");
            schedulingManagerPage.Dialog.ClickNoButton();

            Step("28. Expected The dialog disappears. The selected program is still present in the list. All values in Control program editor are still remained");
            schedulingManagerPage.WaitForPopupDialogDisappeared();
            VerifyEqual("28. Verify The selected program is still present in the list", newName, schedulingManagerPage.SchedulingManagerPanel.GetSelectedControlProgramName());
            VerifyBasicValues(schedulingManagerPage, newName, newDescription, template);
            VerifySwitchOnGroupValues(schedulingManagerPage, switchOnMinute: newSwitchOnMinute, switchOnRelation: newSwitchOnRelation, switchOnLevel: string.Format("{0}%", newSwitchOnLevel));
            VerifySwitchOffGroupValues(schedulingManagerPage, switchOffMinute: newSwitchOffMinute, switchOffRelation: newSwitchOffRelation);

            Step("29. Click Delete button from Control program grid again");
            schedulingManagerPage.SchedulingManagerPanel.ClickDeleteControlProgramButton();

            Step("30. Expected A confirmation dialog appears with message 'Do you want to delete the control program '{{Control program name}}' ?'");
            schedulingManagerPage.WaitForPopupDialogDisplayed();
            VerifyEqual(string.Format("30. Verify A confirmation dialog appears with message '{0}'", expectedMessage), expectedMessage, schedulingManagerPage.Dialog.GetMessageText());

            Step("31. Click Yes");
            schedulingManagerPage.Dialog.ClickYesButton();
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("32. Expected The dialog disappears. The selected program is no longer present in the list. All values in Control program editor are still remained");
            schedulingManagerPage.WaitForPopupDialogDisappeared();
            VerifyEqual("32. Verify The selected program is no longer present in the list", false, schedulingManagerPage.SchedulingManagerPanel.IsControlProgramPresentInGrid(newName));

            Step("33. Reload browser and go to Scheduling Manager again");
            desktopPage = Browser.RefreshLoggedInCMS();
            schedulingManagerPage = desktopPage.GoToApp(App.SchedulingManager) as SchedulingManagerPage;

            Step("34. Select 'Control program' tab");
            schedulingManagerPage.SchedulingManagerPanel.SelectTab("Control program");
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("35. Expected The selected program is no longer present in the list");
            VerifyEqual("35. Verify The selected program is no longer present in the list", false, schedulingManagerPage.SchedulingManagerPanel.IsControlProgramPresentInGrid(newName));
        }

        [Test, DynamicRetry]
        [Description("SM_02 Control program - Basic operations - Basic ON-OFF fixed time events template")]
        public void SM_02()
        {
            var expectedTemplates = GetTemplates();
            var template = "Astro ON/OFF and fixed time events";
            var random = new Random();

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.SchedulingManager);

            Step("1. Go to Scheduling Manager app");
            Step("2. Expected Scheduling Manager is routed and loaded successfully");
            var schedulingManagerPage = desktopPage.GoToApp(App.SchedulingManager) as SchedulingManagerPage;

            Step("3. Select 'Control program' tab in left panel");
            schedulingManagerPage.SchedulingManagerPanel.SelectTab("Control program");
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("4. Click Add new control program button");
            schedulingManagerPage.SchedulingManagerPanel.ClickAddControlProgramButton();
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("5. Expected");
            Step(" - A new item is orderedly inserted into Control program grid");
            Step("  + Color: a random color");
            Step("  + Name: pattern 'New control program \\d{1,}'");
            Step("  + Geozones: GeoZones");
            var newControlProgramNameInGrid = schedulingManagerPage.SchedulingManagerPanel.GetSelectedControlProgramName();
            var newControlProgramColorInGrid = schedulingManagerPage.SchedulingManagerPanel.GetSelectedControlProgramColor();
            var newControlProgramGeozoneInGrid = schedulingManagerPage.SchedulingManagerPanel.GetSelectedControlProgramGeozone();
            VerifyEqual("5. Verify Color: a random color", true, newControlProgramColorInGrid != Color.Empty);
            VerifyEqual("5. Verify Name: pattern 'New control program \\d{1,}'", true, Regex.IsMatch(schedulingManagerPage.SchedulingManagerPanel.GetSelectedControlProgramName(), @"New control program \d{1,}"));
            VerifyEqual("5. Verify Geozones: GeoZones", "GeoZones", newControlProgramGeozoneInGrid);

            Step(" - Control program editor panel is refreshed for the new item:");
            Step("  + Name and color should be the same in grid");
            Step("  + Description: empty");
            var newControlProgramName = schedulingManagerPage.ControlProgramEditorPanel.GetNameValue();
            var newControlProgramColor = schedulingManagerPage.ControlProgramEditorPanel.GetChartColorValue();
            var newControlProgramDescription = schedulingManagerPage.ControlProgramEditorPanel.GetDescriptionValue();
            VerifyEqual("5. Verify Name should be the same in grid", newControlProgramNameInGrid, newControlProgramName);
            VerifyEqual("5. Verify Color should be the same in grid", newControlProgramColorInGrid, newControlProgramColor);
            VerifyEqual("5. Verify Description: empty", string.Empty, newControlProgramDescription);

            Step(" - Template dropdown list has items:");
            Step("  + Astro ON/OFF");
            Step("  + Astro ON/OFF and fixed time events");
            Step("  + Always ON");
            Step("  + Always OFF");
            Step("  + Day fixed time events");
            Step("  + Advanced mode");
            var templateItems = schedulingManagerPage.ControlProgramEditorPanel.GetListOfTemplateItems();
            VerifyEqual("5. Verify Template dropdown list has items as expected", expectedTemplates, templateItems, false);

            Step("6. Select template 'Astro ON/OFF and fixed time events'");
            schedulingManagerPage.ControlProgramEditorPanel.SelectTemplateDropDown(template);

            Step("7. Expected Details are updated according to the template:");
            Step(" - Control program table button is not displayed");
            VerifyEqual("7. Verify Control program table button is not displayed", false, schedulingManagerPage.ControlProgramEditorPanel.IsControlProgramItemsButtonVisible());

            Step(" - Switch ON:");
            Step("  + Minutes: numeric = 0");
            Step("  + After/Before field: dropdown list = 'After'");
            Step("  + Sunset: readonly dropdown list = 'Sunset'");
            Step("  + Dimming level: percentage numeric = '100%'");
            VerifyInitialSwithOnGroup(schedulingManagerPage);

            Step(" - Switch OFF:");
            Step("  + Minutes: numeric = 0");
            Step("  + After/Before field: dropdown list = 'After'");
            Step("  + Sunrise: readonly dropdown list = 'Sunrise'");
            Step("  + Dimming level: readonly input with default value = '0%'");
            VerifyInitialSwithOffGroup(schedulingManagerPage);

            Step(" - Variations:");
            Step(" - 2 items by default: 1st field  is time picker (hh:mm), 2nd is dimming level in percentage, 3rd is Remove button:");
            Step("  + 1st item: 11:00 PM; 77%");
            Step("  + 2nd item: 05:00 AM; 90%");
            VerifyInitialVariationsGroup(schedulingManagerPage);

            Step(" - Timeline: is not displayed");
            VerifyEqual("7. Verify Timeline: is not displayed", false, schedulingManagerPage.ControlProgramEditorPanel.IsTimelineVisible());

            Step(" - Chart: sunset, sunrise & variation points reflect dimming level. To verify this, hover the mouse on these points. When each point is hovered, 2 label appears: 1 is on the left with dimming level in percentage; 1 is at the bottom reflects the minutes, if After is being selected, the value is prefixed with '+' sign, if Before, prefixed with '-' in case Minutes is not 0; prefixed with '-' always in case Minutes is 0; there is no either '+' or '-' for variation points");
            schedulingManagerPage.ControlProgramEditorPanel.MoveToSunsetDot();
            VerifyChartTooltip(schedulingManagerPage, schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOnLevelValue(), GetTooltipTimeWithRelation(schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOnRelationValue(), int.Parse(schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOnMinuteValue())));
            schedulingManagerPage.ControlProgramEditorPanel.MoveToSunriseDot();
            VerifyChartTooltip(schedulingManagerPage, schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOffLevelValue(), GetTooltipTimeWithRelation(schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOffRelationValue(), int.Parse(schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOffMinuteValue())));
            var indexVariation = random.Next(0, 1);
            schedulingManagerPage.ControlProgramEditorPanel.MoveToVariationDot(indexVariation);
            VerifyChartTooltip(schedulingManagerPage, schedulingManagerPage.ControlProgramEditorPanel.GetVariationLevelInputValue(indexVariation), GetTooltipTimeVariation(schedulingManagerPage.ControlProgramEditorPanel.GetVariationTimeInputValue(indexVariation)));

            Step("8. Change all editable fields (including adding more variations)");
            var newName = SLVHelper.GenerateUniqueName("CPSM21");
            var newColor = schedulingManagerPage.ControlProgramEditorPanel.GetChartColorValue();
            var newDescription = "Any description 1";
            var newSwitchOnMinute = random.Next(1, 59).ToString();
            var newSwitchOnRelation = "Before";
            var newSwitchOnLevel = random.Next(1, 99).ToString();
            var newSwitchOffMinute = random.Next(1, 59).ToString();
            var newSwitchOffRelation = "Before";
            var default1stVariationTime = string.Format("{0:d2}:00", random.Next(19, 23));
            var default1stVariationLevel = random.Next(1, 99).ToString();
            var default2ndVariationTime = string.Format("{0:d2}:00", random.Next(1, 3));
            var default2ndVariationLevel = random.Next(1, 99).ToString();
            var newVariationTime = string.Format("{0:d2}:15", random.Next(4, 6));
            var newVariationLevel = random.Next(1, 99).ToString();
            EnterBasicValues(schedulingManagerPage, newName, newDescription);
            EnterSwitchOnGroupValues(schedulingManagerPage, switchOnMinute: newSwitchOnMinute, switchOnRelation: newSwitchOnRelation, switchOnLevel: newSwitchOnLevel);
            EnterSwitchOffGroupValues(schedulingManagerPage, switchOffMinute: newSwitchOffMinute, switchOffRelation: newSwitchOffRelation);
            EnterVariationsGroupValues(schedulingManagerPage, 0, default1stVariationTime, default1stVariationLevel);
            EnterVariationsGroupValues(schedulingManagerPage, 1, default2ndVariationTime, default2ndVariationLevel);
            schedulingManagerPage.ControlProgramEditorPanel.ClickVariationsAddButton();
            EnterVariationsGroupValues(schedulingManagerPage, 2, newVariationTime, newVariationLevel);

            Step("9. Expected The chart refreshes its displaying (see chart verifying in previous step)");
            schedulingManagerPage.ControlProgramEditorPanel.MoveToSunsetDot();
            VerifyChartTooltip(schedulingManagerPage, schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOnLevelValue(), GetTooltipTimeWithRelation(schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOnRelationValue(), int.Parse(schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOnMinuteValue())));
            schedulingManagerPage.ControlProgramEditorPanel.MoveToSunriseDot();
            VerifyChartTooltip(schedulingManagerPage, schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOffLevelValue(), GetTooltipTimeWithRelation(schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOffRelationValue(), int.Parse(schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOffMinuteValue())));
            indexVariation = random.Next(0, 2);
            schedulingManagerPage.ControlProgramEditorPanel.MoveToVariationDot(indexVariation);
            VerifyChartTooltip(schedulingManagerPage, schedulingManagerPage.ControlProgramEditorPanel.GetVariationLevelInputValue(indexVariation), GetTooltipTimeVariation(schedulingManagerPage.ControlProgramEditorPanel.GetVariationTimeInputValue(indexVariation)));

            Step("10. Change all editable fields again (including removing more variations)");
            newName = SLVHelper.GenerateUniqueName("CPSM22");
            newDescription = "Any description 2";
            newSwitchOnMinute = random.Next(1, 59).ToString();
            newSwitchOnRelation = "After";
            newSwitchOnLevel = random.Next(1, 99).ToString();
            newSwitchOffMinute = random.Next(1, 59).ToString();
            newSwitchOffRelation = "After";
            default1stVariationTime = string.Format("{0:d2}:00", random.Next(21, 23));
            default1stVariationLevel = random.Next(1, 99).ToString();
            schedulingManagerPage.ControlProgramEditorPanel.ClickLastVariationRemoveButton();
            Step("--> Remove 2nd Variation for sort issue");
            schedulingManagerPage.ControlProgramEditorPanel.ClickLastVariationRemoveButton();
            EnterBasicValues(schedulingManagerPage, newName, newDescription);
            EnterSwitchOnGroupValues(schedulingManagerPage, switchOnMinute: newSwitchOnMinute, switchOnRelation: newSwitchOnRelation, switchOnLevel: newSwitchOnLevel);
            EnterSwitchOffGroupValues(schedulingManagerPage, switchOffMinute: newSwitchOffMinute, switchOffRelation: newSwitchOffRelation);
            EnterVariationsGroupValues(schedulingManagerPage, 0, default1stVariationTime, default1stVariationLevel);

            Step("11. Expected The chart refreshes its displaying (see chart verifying in previous step)");
            schedulingManagerPage.ControlProgramEditorPanel.MoveToSunsetDot();
            VerifyChartTooltip(schedulingManagerPage, schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOnLevelValue(), GetTooltipTimeWithRelation(schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOnRelationValue(), int.Parse(schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOnMinuteValue())));
            schedulingManagerPage.ControlProgramEditorPanel.MoveToSunriseDot();
            VerifyChartTooltip(schedulingManagerPage, schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOffLevelValue(), GetTooltipTimeWithRelation(schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOffRelationValue(), int.Parse(schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOffMinuteValue())));
            indexVariation = 0;
            schedulingManagerPage.ControlProgramEditorPanel.MoveToVariationDot(indexVariation);
            VerifyChartTooltip(schedulingManagerPage, schedulingManagerPage.ControlProgramEditorPanel.GetVariationLevelInputValue(indexVariation), GetTooltipTimeVariation(schedulingManagerPage.ControlProgramEditorPanel.GetVariationTimeInputValue(indexVariation)));

            Step("12. Save the new control program");
            schedulingManagerPage.ControlProgramEditorPanel.ClickSaveButton();
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("13. Expected The template field becomes readonly");
            VerifyEqual("13. Verify The template field becomes readonly", true, schedulingManagerPage.ControlProgramEditorPanel.IsTemplateDropDownReadOnly());

            Step("14. Reload browser and go to Scheduling Manager again");
            desktopPage = Browser.RefreshLoggedInCMS();
            schedulingManagerPage = desktopPage.GoToApp(App.SchedulingManager) as SchedulingManagerPage;

            Step("15. Select 'Control program' tab");
            schedulingManagerPage.SchedulingManagerPanel.SelectTab("Control program");
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("16. Expected The newly-created control program is still present in the grid");
            VerifyEqual("[SC-567] 16. Verify The newly-created control program is still present in the grid", true, schedulingManagerPage.SchedulingManagerPanel.IsControlProgramPresentInGrid(newName));

            Step("17. Select it");
            schedulingManagerPage.SchedulingManagerPanel.SelectControlProgram(newName);
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("18. Expected");
            Step(" - Control program editor panel is refreshed for the new item: all values should be remained");
            VerifyBasicValues(schedulingManagerPage, newName, newDescription, template);
            VerifySwitchOnGroupValues(schedulingManagerPage, switchOnMinute: newSwitchOnMinute, switchOnRelation: newSwitchOnRelation, switchOnLevel: string.Format("{0}%", newSwitchOnLevel));
            VerifySwitchOffGroupValues(schedulingManagerPage, switchOffMinute: newSwitchOffMinute, switchOffRelation: newSwitchOffRelation);
            VerifyVariationsGroupValues(schedulingManagerPage, 0, default1stVariationTime, default1stVariationLevel);
            Step(" - Template field is readonly");
            VerifyEqual("Verify Template field is readonly", true, schedulingManagerPage.ControlProgramEditorPanel.IsTemplateDropDownReadOnly());
            Step(" - Verify the chart (see its verifying in previous step)");
            schedulingManagerPage.ControlProgramEditorPanel.MoveToSunsetDot();
            VerifyChartTooltip(schedulingManagerPage, schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOnLevelValue(), GetTooltipTimeWithRelation(schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOnRelationValue(), int.Parse(schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOnMinuteValue())));
            schedulingManagerPage.ControlProgramEditorPanel.MoveToSunriseDot();
            VerifyChartTooltip(schedulingManagerPage, schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOffLevelValue(), GetTooltipTimeWithRelation(schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOffRelationValue(), int.Parse(schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOffMinuteValue())));
            indexVariation = 0;
            schedulingManagerPage.ControlProgramEditorPanel.MoveToVariationDot(indexVariation);
            VerifyChartTooltip(schedulingManagerPage, schedulingManagerPage.ControlProgramEditorPanel.GetVariationLevelInputValue(indexVariation), GetTooltipTimeVariation(schedulingManagerPage.ControlProgramEditorPanel.GetVariationTimeInputValue(indexVariation)));

            Step("19. Change all editable fields again then save");
            newName = SLVHelper.GenerateUniqueName("CPSM23");
            newDescription = "Any description 3";
            newSwitchOnMinute = random.Next(1, 59).ToString();
            newSwitchOnRelation = "Before";
            newSwitchOnLevel = random.Next(1, 99).ToString();
            newSwitchOffMinute = random.Next(1, 59).ToString();
            newSwitchOffRelation = "After";
            default1stVariationTime = string.Format("{0:d2}:00", random.Next(19, 23));
            default1stVariationLevel = random.Next(1, 99).ToString();
            EnterBasicValues(schedulingManagerPage, newName, newDescription);
            EnterSwitchOnGroupValues(schedulingManagerPage, switchOnMinute: newSwitchOnMinute, switchOnRelation: newSwitchOnRelation, switchOnLevel: newSwitchOnLevel);
            EnterSwitchOffGroupValues(schedulingManagerPage, switchOffMinute: newSwitchOffMinute, switchOffRelation: newSwitchOffRelation);
            EnterVariationsGroupValues(schedulingManagerPage, 0, default1stVariationTime, default1stVariationLevel);

            schedulingManagerPage.ControlProgramEditorPanel.ClickSaveButton();
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("20. Reload browser and go to Scheduling Manager again");
            desktopPage = Browser.RefreshLoggedInCMS();
            schedulingManagerPage = desktopPage.GoToApp(App.SchedulingManager) as SchedulingManagerPage;

            Step("21. Select 'Control program' tab");
            schedulingManagerPage.SchedulingManagerPanel.SelectTab("Control program");
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("22. Expected The newly-created control program is still present in the grid");
            VerifyEqual("[SC-567] 22. Verify The newly-created control program is still present in the grid", true, schedulingManagerPage.SchedulingManagerPanel.IsControlProgramPresentInGrid(newName));

            Step("23. Select it");
            schedulingManagerPage.SchedulingManagerPanel.SelectControlProgram(newName);
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("24. Expected");
            Step(" - Control program editor panel is refreshed for the new item: all values should be remained");
            VerifyBasicValues(schedulingManagerPage, newName, newDescription, template);
            VerifySwitchOnGroupValues(schedulingManagerPage, switchOnMinute: newSwitchOnMinute, switchOnRelation: newSwitchOnRelation, switchOnLevel: string.Format("{0}%", newSwitchOnLevel));
            VerifySwitchOffGroupValues(schedulingManagerPage, switchOffMinute: newSwitchOffMinute, switchOffRelation: newSwitchOffRelation);
            VerifyVariationsGroupValues(schedulingManagerPage, 0, default1stVariationTime, default1stVariationLevel);

            Step(" - Template field is readonly");
            VerifyEqual("Verify Template field is readonly", true, schedulingManagerPage.ControlProgramEditorPanel.IsTemplateDropDownReadOnly());
            Step(" - Verify the chart (see its verifying in previous step)");
            schedulingManagerPage.ControlProgramEditorPanel.MoveToSunsetDot();
            VerifyChartTooltip(schedulingManagerPage, schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOnLevelValue(), GetTooltipTimeWithRelation(schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOnRelationValue(), int.Parse(schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOnMinuteValue())));
            schedulingManagerPage.ControlProgramEditorPanel.MoveToSunriseDot();
            VerifyChartTooltip(schedulingManagerPage, schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOffLevelValue(), GetTooltipTimeWithRelation(schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOffRelationValue(), int.Parse(schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOffMinuteValue())));
            indexVariation = 0;
            schedulingManagerPage.ControlProgramEditorPanel.MoveToVariationDot(indexVariation);
            VerifyChartTooltip(schedulingManagerPage, schedulingManagerPage.ControlProgramEditorPanel.GetVariationLevelInputValue(indexVariation), GetTooltipTimeVariation(schedulingManagerPage.ControlProgramEditorPanel.GetVariationTimeInputValue(indexVariation)));

            Step("25. Click Delete button from Control program grid");
            schedulingManagerPage.SchedulingManagerPanel.ClickDeleteControlProgramButton();

            Step("26. Expected A confirmation dialog appears with message 'Do you want to delete the control program '{{Control program name}}' ?'");
            schedulingManagerPage.WaitForPopupDialogDisplayed();
            var expectedMessage = string.Format("Do you want to delete the control program '{0}' ?", newName);
            VerifyEqual(string.Format("26. Verify A confirmation dialog appears with message '{0}'", expectedMessage), expectedMessage, schedulingManagerPage.Dialog.GetMessageText());

            Step("27. Click No");
            schedulingManagerPage.Dialog.ClickNoButton();

            Step("28. Expected The dialog disappears. The selected program is still present in the list. All values in Control program editor are still remained");
            schedulingManagerPage.WaitForPopupDialogDisappeared();
            VerifyEqual("28. Verify The selected program is still present in the list", newName, schedulingManagerPage.SchedulingManagerPanel.GetSelectedControlProgramName());
            VerifyBasicValues(schedulingManagerPage, newName, newDescription, template);
            VerifySwitchOnGroupValues(schedulingManagerPage, switchOnMinute: newSwitchOnMinute, switchOnRelation: newSwitchOnRelation, switchOnLevel: string.Format("{0}%", newSwitchOnLevel));
            VerifySwitchOffGroupValues(schedulingManagerPage, switchOffMinute: newSwitchOffMinute, switchOffRelation: newSwitchOffRelation);
            VerifyVariationsGroupValues(schedulingManagerPage, 0, default1stVariationTime, default1stVariationLevel);

            Step("29. Click Delete button from Control program grid again");
            schedulingManagerPage.SchedulingManagerPanel.ClickDeleteControlProgramButton();

            Step("30. Expected A confirmation dialog appears with message 'Do you want to delete the control program '{{Control program name}}' ?'");
            schedulingManagerPage.WaitForPopupDialogDisplayed();
            VerifyEqual(string.Format("30. Verify A confirmation dialog appears with message '{0}'", expectedMessage), expectedMessage, schedulingManagerPage.Dialog.GetMessageText());

            Step("31. Click Yes");
            schedulingManagerPage.Dialog.ClickYesButton();
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("32. Expected The dialog disappears. The selected program is no longer present in the list. All values in Control program editor are still remained");
            schedulingManagerPage.WaitForPopupDialogDisappeared();
            VerifyEqual("32. Verify The selected program is no longer present in the list", false, schedulingManagerPage.SchedulingManagerPanel.IsControlProgramPresentInGrid(newName));

            Step("33. Reload browser and go to Scheduling Manager again");
            desktopPage = Browser.RefreshLoggedInCMS();
            schedulingManagerPage = desktopPage.GoToApp(App.SchedulingManager) as SchedulingManagerPage;

            Step("34. Select 'Control program' tab");
            schedulingManagerPage.SchedulingManagerPanel.SelectTab("Control program");
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("35. Expected The selected program is no longer present in the list");
            VerifyEqual("35. Verify The selected program is no longer present in the list", false, schedulingManagerPage.SchedulingManagerPanel.IsControlProgramPresentInGrid(newName));
        }

        [Test, DynamicRetry]
        [Description("SM_03 Control program - Basic operations - Always ON template")]
        public void SM_03()
        {
            var expectedTemplates = GetTemplates();
            var template = "Always ON";
            var random = new Random();

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.SchedulingManager);

            Step("1. Go to Scheduling Manager app");
            Step("2. Expected Scheduling Manager is routed and loaded successfully");
            var schedulingManagerPage = desktopPage.GoToApp(App.SchedulingManager) as SchedulingManagerPage;

            Step("3. Select 'Control program' tab in left panel");
            schedulingManagerPage.SchedulingManagerPanel.SelectTab("Control program");
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("4. Click Add new control program button");
            schedulingManagerPage.SchedulingManagerPanel.ClickAddControlProgramButton();
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("5. Expected");
            Step(" - A new item is orderedly inserted into Control program grid");
            Step("  + Color: a random color");
            Step("  + Name: pattern 'New control program \\d{1,}'");
            Step("  + Geozones: GeoZones");
            var newControlProgramNameInGrid = schedulingManagerPage.SchedulingManagerPanel.GetSelectedControlProgramName();
            var newControlProgramColorInGrid = schedulingManagerPage.SchedulingManagerPanel.GetSelectedControlProgramColor();
            var newControlProgramGeozoneInGrid = schedulingManagerPage.SchedulingManagerPanel.GetSelectedControlProgramGeozone();            
            VerifyEqual("5. Verify Color: a random color", true, newControlProgramColorInGrid != Color.Empty);
            VerifyEqual("5. Verify Name: pattern 'New control program \\d{1,}'", true, Regex.IsMatch(schedulingManagerPage.SchedulingManagerPanel.GetSelectedControlProgramName(), @"New control program \d{1,}"));
            VerifyEqual("5. Verify Geozones: GeoZones", "GeoZones", newControlProgramGeozoneInGrid);

            Step(" - Control program editor panel is refreshed for the new item:");
            Step("  + Name and color should be the same in grid");
            Step("  + Description: empty");
            var newControlProgramName = schedulingManagerPage.ControlProgramEditorPanel.GetNameValue();
            var newControlProgramColor = schedulingManagerPage.ControlProgramEditorPanel.GetChartColorValue();
            var newControlProgramDescription = schedulingManagerPage.ControlProgramEditorPanel.GetDescriptionValue();
            VerifyEqual("5. Verify Name should be the same in grid", newControlProgramNameInGrid, newControlProgramName);
            VerifyEqual("5. Verify Color should be the same in grid", newControlProgramColorInGrid, newControlProgramColor);
            VerifyEqual("5. Verify Description: empty", string.Empty, newControlProgramDescription);

            Step(" - Template dropdown list has items:");
            Step("  + Astro ON/OFF");
            Step("  + Astro ON/OFF and fixed time events");
            Step("  + Always ON");
            Step("  + Always OFF");
            Step("  + Day fixed time events");
            Step("  + Advanced mode");
            var templateItems = schedulingManagerPage.ControlProgramEditorPanel.GetListOfTemplateItems();
            VerifyEqual("5. Verify Template dropdown list has items as expected", expectedTemplates, templateItems, false);

            Step("6. Select template 'Always ON'");
            schedulingManagerPage.ControlProgramEditorPanel.SelectTemplateDropDown(template);

            Step("7. Expected Details are updated according to the template:");
            Step(" - Control program table button is not displayed");
            VerifyEqual("7. Verify Control program table button is not displayed", false, schedulingManagerPage.ControlProgramEditorPanel.IsControlProgramItemsButtonVisible());

            Step(" - Switch ON:");
            Step("  + Time = 09:00 AM (09:00)");
            Step("  + Dimming level: percentage numeric = '100%'");
            VerifyEqual("7. Verify Time is 09:00 AM (09:00)", true, schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOnTimeValue() == "09:00 AM" || schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOnTimeValue() == "09:00");
            VerifyEqual("7. Verify Dimming Level is 100%", "100%", schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOnLevelValue());

            Step(" - Switch OFF: is not displayed");
            VerifyEqual("7. Verify Switch OFF: is not displayed", false, schedulingManagerPage.ControlProgramEditorPanel.IsSwitchOffGroupVisible());

            Step(" - Variations: is not displayed");
            VerifyEqual("7. Verify Variations: is not displayed", false, schedulingManagerPage.ControlProgramEditorPanel.IsVariationsGroupVisible());

            Step(" - Timeline: is not displayed");
            VerifyEqual("7. Verify Timeline: is not displayed", false, schedulingManagerPage.ControlProgramEditorPanel.IsTimelineVisible());

            Step(" - Chart: 1 point reflect dimming level. To verify this, hover the mouse on this point, 2 label appears: 1 is on the left with dimming level in percentage; 1 is at the bottom reflects time,there is no either '+' or '-' for this point");
            schedulingManagerPage.ControlProgramEditorPanel.MoveToChartDot(0);
            VerifyChartTooltip(schedulingManagerPage, schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOnLevelValue(), GetTooltipTimeVariation(schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOnTimeValue()));

            Step("8. Change all editable fields");
            var newName = SLVHelper.GenerateUniqueName("CPSM31");
            var newColor = schedulingManagerPage.ControlProgramEditorPanel.GetChartColorValue();
            var newDescription = "Any description 1";
            var newSwitchOnTime = string.Format("{0:d2}:00", random.Next(6, 10));
            var newSwitchOnLevel = random.Next(1, 99).ToString();
            EnterBasicValues(schedulingManagerPage, newName, newDescription);
            EnterSwitchOnGroupValues(schedulingManagerPage, switchOnTime: newSwitchOnTime, switchOnLevel: newSwitchOnLevel);

            Step("9. Expected The chart refreshes its displaying (see chart verifying in previous step)");
            schedulingManagerPage.ControlProgramEditorPanel.MoveToChartDot(0);
            VerifyChartTooltip(schedulingManagerPage, schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOnLevelValue(), GetTooltipTimeVariation(schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOnTimeValue()));

            Step("10. Change all editable fields again");
            newName = SLVHelper.GenerateUniqueName("CPSM32");
            newDescription = "Any description 2";
            newSwitchOnTime = string.Format("{0:d2}:00", random.Next(6, 10));
            newSwitchOnLevel = random.Next(1, 99).ToString();
            EnterBasicValues(schedulingManagerPage, newName, newDescription);
            EnterSwitchOnGroupValues(schedulingManagerPage, switchOnTime: newSwitchOnTime, switchOnLevel: newSwitchOnLevel);

            Step("11. Expected The chart refreshes its displaying (see chart verifying in previous step)");
            schedulingManagerPage.ControlProgramEditorPanel.MoveToChartDot(0);
            VerifyChartTooltip(schedulingManagerPage, schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOnLevelValue(), GetTooltipTimeVariation(schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOnTimeValue()));

            Step("12. Save the new control program");
            schedulingManagerPage.ControlProgramEditorPanel.ClickSaveButton();
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("13. Expected The template field becomes readonly");
            VerifyEqual("13. Verify The template field becomes readonly", true, schedulingManagerPage.ControlProgramEditorPanel.IsTemplateDropDownReadOnly());

            Step("14. Reload browser and go to Scheduling Manager again");
            desktopPage = Browser.RefreshLoggedInCMS();
            schedulingManagerPage = desktopPage.GoToApp(App.SchedulingManager) as SchedulingManagerPage;

            Step("15. Select 'Control program' tab");
            schedulingManagerPage.SchedulingManagerPanel.SelectTab("Control program");
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("16. Expected The newly-created control program is still present in the grid");
            VerifyEqual("[SC-567] 16. Verify The newly-created control program is still present in the grid", true, schedulingManagerPage.SchedulingManagerPanel.IsControlProgramPresentInGrid(newName));

            Step("17. Select it");
            schedulingManagerPage.SchedulingManagerPanel.SelectControlProgram(newName);
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("18. Expected");
            Step(" - Control program editor panel is refreshed for the new item: all values should be remained");
            VerifyBasicValues(schedulingManagerPage, newName, newDescription, template);
            VerifySwitchOnGroupValues(schedulingManagerPage, switchOnTime: newSwitchOnTime, switchOnLevel: string.Format("{0}%", newSwitchOnLevel));

            Step(" - Template field is readonly");
            VerifyEqual("18. Verify Template field is readonly", true, schedulingManagerPage.ControlProgramEditorPanel.IsTemplateDropDownReadOnly());
            Step(" - Verify the chart (see its verifying in previous step)");
            schedulingManagerPage.ControlProgramEditorPanel.MoveToChartDot(0);
            VerifyChartTooltip(schedulingManagerPage, schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOnLevelValue(), GetTooltipTimeVariation(schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOnTimeValue()));

            Step("19. Change all editable fields again then save");
            newName = SLVHelper.GenerateUniqueName("CPSM33");
            newDescription = "Any description 3";
            newSwitchOnTime = string.Format("{0:d2}:00", random.Next(6, 10));
            newSwitchOnLevel = random.Next(1, 99).ToString();
            EnterBasicValues(schedulingManagerPage, newName, newDescription);
            EnterSwitchOnGroupValues(schedulingManagerPage, switchOnTime: newSwitchOnTime, switchOnLevel: newSwitchOnLevel);
            schedulingManagerPage.ControlProgramEditorPanel.ClickSaveButton();
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("20. Reload browser and go to Scheduling Manager again");
            desktopPage = Browser.RefreshLoggedInCMS();
            schedulingManagerPage = desktopPage.GoToApp(App.SchedulingManager) as SchedulingManagerPage;

            Step("21. Select 'Control program' tab");
            schedulingManagerPage.SchedulingManagerPanel.SelectTab("Control program");
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("22. Expected The newly-created control program is still present in the grid");
            VerifyEqual("[SC-567] 22. Verify The newly-created control program is still present in the grid", true, schedulingManagerPage.SchedulingManagerPanel.IsControlProgramPresentInGrid(newName));

            Step("23. Select it");
            schedulingManagerPage.SchedulingManagerPanel.SelectControlProgram(newName);
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("24. Expected");
            Step(" - Control program editor panel is refreshed for the new item: all values should be remained");
            VerifyBasicValues(schedulingManagerPage, newName, newDescription, template);
            VerifySwitchOnGroupValues(schedulingManagerPage, switchOnTime: newSwitchOnTime, switchOnLevel: string.Format("{0}%", newSwitchOnLevel));

            Step(" - Template field is readonly");
            VerifyEqual("24. Verify Template field is readonly", true, schedulingManagerPage.ControlProgramEditorPanel.IsTemplateDropDownReadOnly());
            Step(" - Verify the chart (see its verifying in previous step)");
            schedulingManagerPage.ControlProgramEditorPanel.MoveToChartDot(0);
            VerifyChartTooltip(schedulingManagerPage, schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOnLevelValue(), GetTooltipTimeVariation(schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOnTimeValue()));

            Step("25. Click Delete button from Control program grid");
            schedulingManagerPage.SchedulingManagerPanel.ClickDeleteControlProgramButton();

            Step("26. Expected A confirmation dialog appears with message 'Do you want to delete the control program '{{Control program name}}' ?'");
            schedulingManagerPage.WaitForPopupDialogDisplayed();
            var expectedMessage = string.Format("Do you want to delete the control program '{0}' ?", newName);
            VerifyEqual(string.Format("26. Verify A confirmation dialog appears with message '{0}'", expectedMessage), expectedMessage, schedulingManagerPage.Dialog.GetMessageText());

            Step("27. Click No");
            schedulingManagerPage.Dialog.ClickNoButton();

            Step("28. Expected The dialog disappears. The selected program is still present in the list. All values in Control program editor are still remained");
            schedulingManagerPage.WaitForPopupDialogDisappeared();
            VerifyEqual("28. Verify The selected program is still present in the list", newName, schedulingManagerPage.SchedulingManagerPanel.GetSelectedControlProgramName());
            VerifyBasicValues(schedulingManagerPage, newName, newDescription, template);
            VerifySwitchOnGroupValues(schedulingManagerPage, switchOnTime: newSwitchOnTime, switchOnLevel: string.Format("{0}%", newSwitchOnLevel));

            Step("29. Click Delete button from Control program grid again");
            schedulingManagerPage.SchedulingManagerPanel.ClickDeleteControlProgramButton();

            Step("30. Expected A confirmation dialog appears with message 'Do you want to delete the control program '{{Control program name}}' ?'");
            schedulingManagerPage.WaitForPopupDialogDisplayed();
            VerifyEqual(string.Format("30. Verify A confirmation dialog appears with message '{0}'", expectedMessage), expectedMessage, schedulingManagerPage.Dialog.GetMessageText());

            Step("31. Click Yes");
            schedulingManagerPage.Dialog.ClickYesButton();
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("32. Expected The dialog disappears. The selected program is no longer present in the list. All values in Control program editor are still remained");
            schedulingManagerPage.WaitForPopupDialogDisappeared();
            VerifyEqual("32. Verify The selected program is no longer present in the list", false, schedulingManagerPage.SchedulingManagerPanel.IsControlProgramPresentInGrid(newName));

            Step("33. Reload browser and go to Scheduling Manager again");
            desktopPage = Browser.RefreshLoggedInCMS();
            schedulingManagerPage = desktopPage.GoToApp(App.SchedulingManager) as SchedulingManagerPage;

            Step("34. Select 'Control program' tab");
            schedulingManagerPage.SchedulingManagerPanel.SelectTab("Control program");
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("35. Expected The selected program is no longer present in the list");
            VerifyEqual("35. Verify The selected program is no longer present in the list", false, schedulingManagerPage.SchedulingManagerPanel.IsControlProgramPresentInGrid(newName));
        }

        [Test, DynamicRetry]
        [Description("SM_04 Control program - Basic operations - Always OFF template")]
        public void SM_04()
        {
            var expectedTemplates = GetTemplates();
            var template = "Always OFF";
            var random = new Random();

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.SchedulingManager);

            Step("1. Go to Scheduling Manager app");
            Step("2. Expected Scheduling Manager is routed and loaded successfully");
            var schedulingManagerPage = desktopPage.GoToApp(App.SchedulingManager) as SchedulingManagerPage;

            Step("3. Select 'Control program' tab in left panel");
            schedulingManagerPage.SchedulingManagerPanel.SelectTab("Control program");
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("4. Click Add new control program button");
            schedulingManagerPage.SchedulingManagerPanel.ClickAddControlProgramButton();
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("5. Expected");
            Step(" - A new item is orderedly inserted into Control program grid");
            Step("  + Color: a random color");
            Step("  + Name: pattern 'New control program \\d{1,}'");
            Step("  + Geozones: GeoZones");
            var newControlProgramNameInGrid = schedulingManagerPage.SchedulingManagerPanel.GetSelectedControlProgramName();
            var newControlProgramColorInGrid = schedulingManagerPage.SchedulingManagerPanel.GetSelectedControlProgramColor();
            var newControlProgramGeozoneInGrid = schedulingManagerPage.SchedulingManagerPanel.GetSelectedControlProgramGeozone();            
            VerifyEqual("5. Verify Color: a random color", true, newControlProgramColorInGrid != Color.Empty);
            VerifyEqual("5. Verify Name: pattern 'New control program \\d{1,}'", true, Regex.IsMatch(schedulingManagerPage.SchedulingManagerPanel.GetSelectedControlProgramName(), @"New control program \d{1,}"));
            VerifyEqual("5. Verify Geozones: GeoZones", "GeoZones", newControlProgramGeozoneInGrid);

            Step(" - Control program editor panel is refreshed for the new item:");
            Step("  + Name and color should be the same in grid");
            Step("  + Description: empty");
            var newControlProgramName = schedulingManagerPage.ControlProgramEditorPanel.GetNameValue();
            var newControlProgramColor = schedulingManagerPage.ControlProgramEditorPanel.GetChartColorValue();
            var newControlProgramDescription = schedulingManagerPage.ControlProgramEditorPanel.GetDescriptionValue();
            VerifyEqual("5. Verify Name should be the same in grid", newControlProgramNameInGrid, newControlProgramName);
            VerifyEqual("5. Verify Color should be the same in grid", newControlProgramColorInGrid, newControlProgramColor);
            VerifyEqual("5. Verify Description: empty", string.Empty, newControlProgramDescription);

            Step(" - Template dropdown list has items:");
            Step("  + Astro ON/OFF");
            Step("  + Astro ON/OFF and fixed time events");
            Step("  + Always ON");
            Step("  + Always OFF");
            Step("  + Day fixed time events");
            Step("  + Advanced mode");
            var templateItems = schedulingManagerPage.ControlProgramEditorPanel.GetListOfTemplateItems();
            VerifyEqual("5. Verify Template dropdown list has items as expected", expectedTemplates, templateItems, false);

            Step("6. Select template 'Always OFF'");
            schedulingManagerPage.ControlProgramEditorPanel.SelectTemplateDropDown(template);

            Step("7. Expected Details are updated according to the template:");
            Step("  + 2 triangle points: at the buttom of the chart. The left is '12:00 AM'. The right is '12:00 PM'");
            Step("  + Hover the right point: '0%'and '12:00 PM' is highlight");
            Step("  + Hover the left point: '0%'and '12:00 AM' is highlight");
            VerifyEqual("7. Verify 2 triangle points: at the buttom of the chart", 2, schedulingManagerPage.ControlProgramEditorPanel.GetChartDotsCount());
            schedulingManagerPage.ControlProgramEditorPanel.MoveToChartDot(0);
            VerifyChartTooltip(schedulingManagerPage, "0%", "12:00:00 AM");
            schedulingManagerPage.ControlProgramEditorPanel.MoveToChartDot(1);
            VerifyChartTooltip(schedulingManagerPage, "0%", "12:00:00 PM");
            
            Step("8. Enter name, description and save the new control program");
            var newName = SLVHelper.GenerateUniqueName("CPSM4");
            var newDescription = "Any description 1";
            EnterBasicValues(schedulingManagerPage, newName, newDescription);
            schedulingManagerPage.ControlProgramEditorPanel.ClickSaveButton();
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("9. Expected The template field becomes readonly");
            VerifyEqual("9. Verify The template field becomes readonly", true, schedulingManagerPage.ControlProgramEditorPanel.IsTemplateDropDownReadOnly());

            Step("10. Reload browser and go to Scheduling Manager again");
            desktopPage = Browser.RefreshLoggedInCMS();
            schedulingManagerPage = desktopPage.GoToApp(App.SchedulingManager) as SchedulingManagerPage;

            Step("11. Select 'Control program' tab");
            schedulingManagerPage.SchedulingManagerPanel.SelectTab("Control program");
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("12. Expected The newly-created control program is still present in the grid");
            VerifyEqual("[SC-567] 12. Verify The newly-created control program is still present in the grid", true, schedulingManagerPage.SchedulingManagerPanel.IsControlProgramPresentInGrid(newName));

            Step("13. Select it");
            schedulingManagerPage.SchedulingManagerPanel.SelectControlProgram(newName);
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("14. Expected");
            Step(" - Control program editor panel is refreshed for the new item: all values should be remained");
            VerifyBasicValues(schedulingManagerPage, newName, newDescription, template);
            Step(" - Template field is readonly");
            VerifyEqual("Verify Template field is readonly", true, schedulingManagerPage.ControlProgramEditorPanel.IsTemplateDropDownReadOnly());
            Step(" - Verify the chart (see its verifying in previous step)");
            VerifyEqual("14. Verify 2 triangle points: at the buttom of the chart", 2, schedulingManagerPage.ControlProgramEditorPanel.GetChartDotsCount());
            schedulingManagerPage.ControlProgramEditorPanel.MoveToChartDot(0);
            VerifyChartTooltip(schedulingManagerPage, "0%", "12:00:00 AM");
            schedulingManagerPage.ControlProgramEditorPanel.MoveToChartDot(1);
            VerifyChartTooltip(schedulingManagerPage, "0%", "12:00:00 PM");
            
            Step("15. Reload browser and go to Scheduling Manager again");
            desktopPage = Browser.RefreshLoggedInCMS();
            schedulingManagerPage = desktopPage.GoToApp(App.SchedulingManager) as SchedulingManagerPage;

            Step("16. Select 'Control program' tab");
            schedulingManagerPage.SchedulingManagerPanel.SelectTab("Control program");
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("17. Expected The newly-created control program is still present in the grid");
            VerifyEqual("[SC-567] 17. Verify The newly-created control program is still present in the grid", true, schedulingManagerPage.SchedulingManagerPanel.IsControlProgramPresentInGrid(newName));

            Step("18. Select it");
            schedulingManagerPage.SchedulingManagerPanel.SelectControlProgram(newName);
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("19. Expected");
            Step(" - Control program editor panel is refreshed for the new item: all values should be remained");
            VerifyBasicValues(schedulingManagerPage, newName, newDescription, template);
            Step(" - Template field is readonly");
            VerifyEqual("Verify Template field is readonly", true, schedulingManagerPage.ControlProgramEditorPanel.IsTemplateDropDownReadOnly());
            Step(" - Verify the chart (see its verifying in previous step)");
            VerifyEqual("19. Verify 2 triangle points: at the buttom of the chart", 2, schedulingManagerPage.ControlProgramEditorPanel.GetChartDotsCount());
            schedulingManagerPage.ControlProgramEditorPanel.MoveToChartDot(0);
            VerifyChartTooltip(schedulingManagerPage, "0%", "12:00:00 AM");
            schedulingManagerPage.ControlProgramEditorPanel.MoveToChartDot(1);
            VerifyChartTooltip(schedulingManagerPage, "0%", "12:00:00 PM");

            Step("20. Click Delete button from Control program grid");
            schedulingManagerPage.SchedulingManagerPanel.ClickDeleteControlProgramButton();

            Step("21. Expected A confirmation dialog appears with message 'Do you want to delete the control program '{{Control program name}}' ?'");
            schedulingManagerPage.WaitForPopupDialogDisplayed();
            var expectedMessage = string.Format("Do you want to delete the control program '{0}' ?", newName);
            VerifyEqual(string.Format("21. Verify A confirmation dialog appears with message '{0}'", expectedMessage), expectedMessage, schedulingManagerPage.Dialog.GetMessageText());

            Step("22. Click No");
            schedulingManagerPage.Dialog.ClickNoButton();

            Step("23. Expected The dialog disappears. The selected program is still present in the list. All values in Control program editor are still remained");
            schedulingManagerPage.WaitForPopupDialogDisappeared();
            VerifyEqual("23. Verify The selected program is still present in the list", newName, schedulingManagerPage.SchedulingManagerPanel.GetSelectedControlProgramName());
            VerifyBasicValues(schedulingManagerPage, newName, newDescription, template);

            Step("24. Click Delete button from Control program grid again");
            schedulingManagerPage.SchedulingManagerPanel.ClickDeleteControlProgramButton();

            Step("25. Expected A confirmation dialog appears with message 'Do you want to delete the control program '{{Control program name}}' ?'");
            schedulingManagerPage.WaitForPopupDialogDisplayed();
            VerifyEqual(string.Format("25. Verify A confirmation dialog appears with message '{0}'", expectedMessage), expectedMessage, schedulingManagerPage.Dialog.GetMessageText());

            Step("26. Click Yes");
            schedulingManagerPage.Dialog.ClickYesButton();
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("27. Expected The dialog disappears. The selected program is no longer present in the list. All values in Control program editor are still remained");
            schedulingManagerPage.WaitForPopupDialogDisappeared();
            VerifyEqual("27. Verify The selected program is no longer present in the list", false, schedulingManagerPage.SchedulingManagerPanel.IsControlProgramPresentInGrid(newName));

            Step("28. Reload browser and go to Scheduling Manager again");
            desktopPage = Browser.RefreshLoggedInCMS();
            schedulingManagerPage = desktopPage.GoToApp(App.SchedulingManager) as SchedulingManagerPage;

            Step("29. Select 'Control program' tab");
            schedulingManagerPage.SchedulingManagerPanel.SelectTab("Control program");
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("30. Expected The selected program is no longer present in the list");
            VerifyEqual("30. Verify The selected program is no longer present in the list", false, schedulingManagerPage.SchedulingManagerPanel.IsControlProgramPresentInGrid(newName));
        }

        [Test, DynamicRetry]
        [Description("SM_05 Control program - Basic operations - Day fixed time events template")]
        public void SM_05()
        {
            Warning("--> Blocked by SC-564: Schedule \"Day fixed time events\" not saved properly when first removing an event");
            //var expectedTemplates = GetTemplates();
            //var template = "Day fixed time events";
            //var random = new Random();

            //Step("**** Precondition ****");
            //Step(" - User has logged in successfully");
            //Step("**** Precondition ****\n");

            //var loginPage = Browser.OpenSlvPage();
            //var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            //desktopPage.InstallAppsIfNotExist(App.SchedulingManager);

            //Step("1. Go to Scheduling Manager app");
            //Step("2. Expected Scheduling Manager is routed and loaded successfully");
            //var schedulingManagerPage = desktopPage.GoToApp(App.SchedulingManager) as SchedulingManagerPage;

            //Step("3. Select 'Control program' tab in left panel");
            //schedulingManagerPage.SchedulingManagerPanel.SelectTab("Control program");
            //schedulingManagerPage.WaitForPreviousActionComplete();

            //Step("4. Click Add new control program button");
            //schedulingManagerPage.SchedulingManagerPanel.ClickAddControlProgramButton();
            //schedulingManagerPage.WaitForPreviousActionComplete();

            //Step("5. Expected");
            //Step(" - A new item is orderedly inserted into Control program grid");
            //Step("  + Color: a random color");
            //Step("  + Name: pattern 'New control program \\d{1,}'");
            //Step("  + Geozones: GeoZones");
            //var newControlProgramNameInGrid = schedulingManagerPage.SchedulingManagerPanel.GetSelectedRowName();
            //var newControlProgramColorInGrid = schedulingManagerPage.SchedulingManagerPanel.GetSelectedRowColor();
            //var newControlProgramGeozoneInGrid = schedulingManagerPage.SchedulingManagerPanel.GetSelectedRowGeozone();            
            //VerifyEqual("Verify Color: a random color", true, newControlProgramColorInGrid != Color.Empty);
            //VerifyEqual("Verify Name: pattern 'New control program \\d{1,}'", true, Regex.IsMatch(schedulingManagerPage.SchedulingManagerPanel.GetSelectedRowName(), @"New control program \d{1,}"));
            //VerifyEqual("Verify Geozones: GeoZones", "GeoZones", newControlProgramGeozoneInGrid);

            //Step(" - Control program editor panel is refreshed for the new item:");
            //Step("  + Name and color should be the same in grid");
            //Step("  + Description: empty");
            //var newControlProgramName = schedulingManagerPage.ControlProgramEditorPanel.GetNameValue();
            //var newControlProgramColor = schedulingManagerPage.ControlProgramEditorPanel.GetChartColorValue();
            //var newControlProgramDescription = schedulingManagerPage.ControlProgramEditorPanel.GetDescriptionValue();
            //VerifyEqual("Verify Name should be the same in grid", newControlProgramNameInGrid, newControlProgramName);
            //VerifyEqual("Verify Color should be the same in grid", newControlProgramColorInGrid, newControlProgramColor);
            //VerifyEqual("Verify Description: empty", string.Empty, newControlProgramDescription);

            //Step(" - Template dropdown list has items:");
            //Step("  + Astro ON/OFF");
            //Step("  + Astro ON/OFF and fixed time events");
            //Step("  + Always ON");
            //Step("  + Always OFF");
            //Step("  + Day fixed time events");
            //Step("  + Advanced mode");
            //var templateItems = schedulingManagerPage.ControlProgramEditorPanel.GetListOfTemplateItems();
            //VerifyEqual("Verify Template dropdown list has items as expected", true, expectedTemplates.Count == templateItems.Count && expectedTemplates.CheckIfIncluded(templateItems));

            //Step("6. Select template 'Day fixed time events'");
            //schedulingManagerPage.ControlProgramEditorPanel.SelectTemplateDropDown(template);

            //Step("7. Expected Details are updated according to the template:");
            //Step(" - Control program table button is not displayed");
            //VerifyEqual("Verify Control program table button is not displayed", false, schedulingManagerPage.ControlProgramEditorPanel.IsControlProgramItemsButtonVisible());

            //Step(" - Switch ON: is not displayed");
            //VerifyEqual("Verify Switch ON: is not displayed", false, schedulingManagerPage.ControlProgramEditorPanel.IsSwitchOnGroupVisible());

            //Step(" - Switch OFF: is not displayed");
            //VerifyEqual("Verify Switch OFF: is not displayed", false, schedulingManagerPage.ControlProgramEditorPanel.IsSwitchOffGroupVisible());

            //Step(" - Variations:");
            //Step(" - 2 items by default: 1st field  is time picker (hh:mm), 2nd is dimming level in percentage, 3rd is Remove button:");
            //Step("  + 1st item: 09:00 AM; 100%");
            //Step("  + 2nd item: 09:30 AM; 0%");
            //VerifyEqual("Verify 2 items by default", 2, schedulingManagerPage.ControlProgramEditorPanel.GetVariationsCount());
            //VerifyEqual("Verify 1st item: Time is 09:00 AM (09:00)", true, schedulingManagerPage.ControlProgramEditorPanel.GetFirstVariationTimeInputValue() == "09:00 AM" || schedulingManagerPage.ControlProgramEditorPanel.GetFirstVariationTimeInputValue() == "09:00");
            //VerifyEqual("Verify 1st item: Level is 100%", "100%", schedulingManagerPage.ControlProgramEditorPanel.GetFirstVariationLevelInputValue());
            //VerifyEqual("Verify 2nd item: Time is 09:30 AM (09:30)", true, schedulingManagerPage.ControlProgramEditorPanel.GetLastVariationTimeInputValue() == "09:00 AM" || schedulingManagerPage.ControlProgramEditorPanel.GetLastVariationTimeInputValue() == "09:30");
            //VerifyEqual("Verify 2nd item: Level is 0%", "0%", schedulingManagerPage.ControlProgramEditorPanel.GetLastVariationLevelInputValue());

            //Step(" - Timeline: is not displayed");
            //VerifyEqual("Verify Timeline: is not displayed", false, schedulingManagerPage.ControlProgramEditorPanel.IsTimelineVisible());

            //Step(" - Chart: variation points reflect dimming level. To verify this, hover the mouse on these points. When each point is hovered, 2 label appears: 1 is on the left with dimming level in percentage; 1 is at the bottom reflects the minutes; there is no either '+' or '-' for variation points");
            //var indexVariation = random.Next(0, 1);
            //schedulingManagerPage.ControlProgramEditorPanel.MoveToVariationDot(indexVariation);
            //var expectedLevel = schedulingManagerPage.ControlProgramEditorPanel.GetVariationLevelInputValue(indexVariation);
            //var expectedTime = GetTooltipTimeVariation(schedulingManagerPage.ControlProgramEditorPanel.GetVariationTimeInputValue(indexVariation));
            //VerifyChartTooltip(schedulingManagerPage, expectedLevel, expectedTime);

            //Step("8. Change all editable fields (including adding more variations)");
            //var newName = SLVHelper.GenerateUniqueName("Name CP1");
            //var newColor = schedulingManagerPage.ControlProgramEditorPanel.GetChartColorValue();
            //var newDescription = "Any description 1";
            //var default1stVariationTime = string.Format("{0:d2}:00", random.Next(1, 8));
            //var default1stVariationLevel = random.Next(1, 99).ToString();
            //var default2ndVariationTime = string.Format("{0:d2}:00", random.Next(9, 15));
            //var default2ndVariationLevel = random.Next(1, 99).ToString();
            //var newVariationTime = string.Format("{0:d2}:15", random.Next(16, 23));
            //var newVariationLevel = random.Next(1, 99).ToString();
            //EnterBasicValues(schedulingManagerPage, newName, newDescription);
            //EnterVariationsGroupValues(schedulingManagerPage, 0, default1stVariationTime, default1stVariationLevel);
            //EnterVariationsGroupValues(schedulingManagerPage, 1, default2ndVariationTime, default2ndVariationLevel);
            //schedulingManagerPage.ControlProgramEditorPanel.ClickVariationsAddButton();
            //EnterVariationsGroupValues(schedulingManagerPage, 2, newVariationTime, newVariationLevel);

            //Step("9. Expected The chart refreshes its displaying (see chart verifying in previous step)");
            //for (int i = 0; i < 3; i++)
            //{
            //    indexVariation = i;
            //    schedulingManagerPage.ControlProgramEditorPanel.MoveToVariationDot(indexVariation);
            //    expectedLevel = schedulingManagerPage.ControlProgramEditorPanel.GetVariationLevelInputValue(indexVariation);
            //    expectedTime = GetTooltipTimeVariation(schedulingManagerPage.ControlProgramEditorPanel.GetVariationTimeInputValue(indexVariation));
            //    VerifyChartTooltip(schedulingManagerPage, expectedLevel, expectedTime);
            //}

            //Step("10. Change all editable fields again (including removing more variations)");
            //newName = SLVHelper.GenerateUniqueName("Name CP2");
            //newDescription = "Any description 2";
            //default1stVariationTime = string.Format("{0:d2}:00", random.Next(1, 12));
            //default1stVariationLevel = random.Next(1, 99).ToString();
            //schedulingManagerPage.ControlProgramEditorPanel.ClickLastVariationRemoveButton();
            //Step("--> Remove 2nd Variation for sort issue");
            //schedulingManagerPage.ControlProgramEditorPanel.ClickLastVariationRemoveButton();
            //EnterBasicValues(schedulingManagerPage, newName, newDescription);
            //EnterVariationsGroupValues(schedulingManagerPage, 0, default1stVariationTime, default1stVariationLevel);

            //Step("11. Expected The chart refreshes its displaying (see chart verifying in previous step)");
            //indexVariation = 0;
            //schedulingManagerPage.ControlProgramEditorPanel.MoveToVariationDot(indexVariation);
            //expectedLevel = schedulingManagerPage.ControlProgramEditorPanel.GetVariationLevelInputValue(indexVariation);
            //expectedTime = GetTooltipTimeVariation(schedulingManagerPage.ControlProgramEditorPanel.GetVariationTimeInputValue(indexVariation));
            //VerifyChartTooltip(schedulingManagerPage, expectedLevel, expectedTime);

            //Step("12. Save the new control program");
            //schedulingManagerPage.ControlProgramEditorPanel.ClickSaveButton();
            //schedulingManagerPage.WaitForPreviousActionComplete();

            //Step("13. Expected The template field becomes readonly");
            //VerifyEqual("Verify The template field becomes readonly", true, schedulingManagerPage.ControlProgramEditorPanel.IsTemplateDropDownReadOnly());

            //Step("14. Reload browser and go to Scheduling Manager again");
            //desktopPage = Browser.RefreshPage();
            //schedulingManagerPage = desktopPage.GoToApp(App.SchedulingManager) as SchedulingManagerPage;

            //Step("15. Select 'Control program' tab");
            //schedulingManagerPage.SchedulingManagerPanel.SelectTab("Control program");
            //schedulingManagerPage.WaitForPreviousActionComplete();

            //Step("16. Expected The newly-created control program is still present in the grid");
            //VerifyEqual("Verify The newly-created control program is still present in the grid", true, schedulingManagerPage.SchedulingManagerPanel.IsControlProgramPresentInGrid(newName));

            //Step("17. Select it");
            //schedulingManagerPage.SchedulingManagerPanel.SelectControlProgram(newName);
            //schedulingManagerPage.WaitForPreviousActionComplete();

            //Step("18. Expected");
            //Step(" - Control program editor panel is refreshed for the new item: all values should be remained");
            //VerifyBasicValues(schedulingManagerPage, newName, newDescription, template);            
            //VerifyVariationsGroupValues(schedulingManagerPage, 0, default1stVariationTime, default1stVariationLevel);
            //Step(" - Template field is readonly");
            //VerifyEqual("Verify Template field is readonly", true, schedulingManagerPage.ControlProgramEditorPanel.IsTemplateDropDownReadOnly());
            //Step(" - Verify the chart (see its verifying in previous step)");
            //indexVariation = 0;
            //schedulingManagerPage.ControlProgramEditorPanel.MoveToVariationDot(indexVariation);
            //expectedLevel = schedulingManagerPage.ControlProgramEditorPanel.GetVariationLevelInputValue(indexVariation);
            //expectedTime = GetTooltipTimeVariation(schedulingManagerPage.ControlProgramEditorPanel.GetVariationTimeInputValue(indexVariation));
            //VerifyChartTooltip(schedulingManagerPage, expectedLevel, expectedTime);

            //Step("19. Change all editable fields again then save");
            //newName = SLVHelper.GenerateUniqueName("Name CP3");
            //newDescription = "Any description 3";
            //default1stVariationTime = string.Format("{0:d2}:00", random.Next(19, 23));
            //default1stVariationLevel = random.Next(1, 99).ToString();
            //EnterBasicValues(schedulingManagerPage, newName, newDescription);
            //EnterVariationsGroupValues(schedulingManagerPage, 0, default1stVariationTime, default1stVariationLevel);
            //schedulingManagerPage.AppBar.ClickHeaderBartop();

            //schedulingManagerPage.ControlProgramEditorPanel.ClickSaveButton();
            //schedulingManagerPage.WaitForPreviousActionComplete();

            //Step("20. Reload browser and go to Scheduling Manager again");
            //desktopPage = Browser.RefreshPage();
            //schedulingManagerPage = desktopPage.GoToApp(App.SchedulingManager) as SchedulingManagerPage;

            //Step("21. Select 'Control program' tab");
            //schedulingManagerPage.SchedulingManagerPanel.SelectTab("Control program");
            //schedulingManagerPage.WaitForPreviousActionComplete();

            //Step("22. Expected The newly-created control program is still present in the grid");
            //VerifyEqual("Verify The newly-created control program is still present in the grid", true, schedulingManagerPage.SchedulingManagerPanel.IsControlProgramPresentInGrid(newName));

            //Step("23. Select it");
            //schedulingManagerPage.SchedulingManagerPanel.SelectControlProgram(newName);
            //schedulingManagerPage.WaitForPreviousActionComplete();

            //Step("24. Expected");
            //Step(" - Control program editor panel is refreshed for the new item: all values should be remained");
            //VerifyBasicValues(schedulingManagerPage, newName, newDescription, template);
            //VerifyVariationsGroupValues(schedulingManagerPage, 0, default1stVariationTime, default1stVariationLevel);            
            //Step(" - Template field is readonly");
            //VerifyEqual("Verify Template field is readonly", true, schedulingManagerPage.ControlProgramEditorPanel.IsTemplateDropDownReadOnly());
            //Step(" - Verify the chart (see its verifying in previous step)");
            //indexVariation = 0;
            //schedulingManagerPage.ControlProgramEditorPanel.MoveToVariationDot(indexVariation);
            //expectedLevel = schedulingManagerPage.ControlProgramEditorPanel.GetVariationLevelInputValue(indexVariation);
            //expectedTime = GetTooltipTimeVariation(schedulingManagerPage.ControlProgramEditorPanel.GetVariationTimeInputValue(indexVariation));
            //VerifyChartTooltip(schedulingManagerPage, expectedLevel, expectedTime);

            //Step("25. Click Delete button from Control program grid");
            //schedulingManagerPage.SchedulingManagerPanel.ClickDeleteControlProgramButton();

            //Step("26. Expected A confirmation dialog appears with message 'Do you want to delete the control program '{{Control program name}}' ?'");
            //schedulingManagerPage.WaitForPopupDialogDisplayed();
            //var expectedMessage = string.Format("Do you want to delete the control program '{0}' ?", newName);
            //VerifyEqual(string.Format("Verify A confirmation dialog appears with message '{0}'", expectedMessage), expectedMessage, schedulingManagerPage.Dialog.GetMessageText());

            //Step("27. Click No");
            //schedulingManagerPage.Dialog.ClickNoButton();

            //Step("28. Expected The dialog disappears. The selected program is still present in the list. All values in Control program editor are still remained");
            //schedulingManagerPage.WaitForPopupDialogDisappeared();
            //VerifyEqual("Verify The selected program is still present in the list", newName, schedulingManagerPage.SchedulingManagerPanel.GetSelectedRowName());
            //VerifyBasicValues(schedulingManagerPage, newName, newDescription, template);
            //VerifyVariationsGroupValues(schedulingManagerPage, 0, default1stVariationTime, default1stVariationLevel);

            //Step("29. Click Delete button from Control program grid again");
            //schedulingManagerPage.SchedulingManagerPanel.ClickDeleteControlProgramButton();

            //Step("30. Expected A confirmation dialog appears with message 'Do you want to delete the control program '{{Control program name}}' ?'");
            //schedulingManagerPage.WaitForPopupDialogDisplayed();
            //VerifyEqual(string.Format("Verify A confirmation dialog appears with message '{0}'", expectedMessage), expectedMessage, schedulingManagerPage.Dialog.GetMessageText());

            //Step("31. Click Yes");
            //schedulingManagerPage.Dialog.ClickYesButton();
            //schedulingManagerPage.WaitForPreviousActionComplete();

            //Step("32. Expected The dialog disappears. The selected program is no longer present in the list. All values in Control program editor are still remained");
            //schedulingManagerPage.WaitForPopupDialogDisappeared();
            //VerifyEqual("Verify The selected program is no longer present in the list", false, schedulingManagerPage.SchedulingManagerPanel.IsControlProgramPresentInGrid(newName));

            //Step("33. Reload browser and go to Scheduling Manager again");
            //desktopPage = Browser.RefreshPage();
            //schedulingManagerPage = desktopPage.GoToApp(App.SchedulingManager) as SchedulingManagerPage;

            //Step("34. Select 'Control program' tab");
            //schedulingManagerPage.SchedulingManagerPanel.SelectTab("Control program");
            //schedulingManagerPage.WaitForPreviousActionComplete();

            //Step("35. Expected The selected program is no longer present in the list");
            //VerifyEqual("Verify The selected program is no longer present in the list", false, schedulingManagerPage.SchedulingManagerPanel.IsControlProgramPresentInGrid(newName));
        }

        [Test, DynamicRetry]
        [Description("SM_06 Control program - Basic operations - Advanced mode template")]
        public void SM_06()
        {
            var expectedTemplates = GetTemplates();
            var template = "Advanced mode";
            var random = new Random();

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.SchedulingManager);

            Step("1. Go to Scheduling Manager app");
            Step("2. Expected Scheduling Manager is routed and loaded successfully");
            var schedulingManagerPage = desktopPage.GoToApp(App.SchedulingManager) as SchedulingManagerPage;

            Step("3. Select 'Control program' tab in left panel");
            schedulingManagerPage.SchedulingManagerPanel.SelectTab("Control program");
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("4. Click Add new control program button");
            schedulingManagerPage.SchedulingManagerPanel.ClickAddControlProgramButton();
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("5. Expected");
            Step(" - A new item is orderedly inserted into Control program grid");
            Step("  + Color: a random color");
            Step("  + Name: pattern 'New control program \\d{1,}'");
            Step("  + Geozones: GeoZones");
            var newControlProgramNameInGrid = schedulingManagerPage.SchedulingManagerPanel.GetSelectedControlProgramName();
            var newControlProgramColorInGrid = schedulingManagerPage.SchedulingManagerPanel.GetSelectedControlProgramColor();
            var newControlProgramGeozoneInGrid = schedulingManagerPage.SchedulingManagerPanel.GetSelectedControlProgramGeozone();            
            VerifyEqual("5. Verify Color: a random color", true, newControlProgramColorInGrid != Color.Empty);
            VerifyEqual("5. Verify Name: pattern 'New control program \\d{1,}'", true, Regex.IsMatch(schedulingManagerPage.SchedulingManagerPanel.GetSelectedControlProgramName(), @"New control program \d{1,}"));
            VerifyEqual("5. Verify Geozones: GeoZones", "GeoZones", newControlProgramGeozoneInGrid);

            Step(" - Control program editor panel is refreshed for the new item:");
            Step("  + Name and color should be the same in grid");
            Step("  + Description: empty");
            var newControlProgramName = schedulingManagerPage.ControlProgramEditorPanel.GetNameValue();
            var newControlProgramColor = schedulingManagerPage.ControlProgramEditorPanel.GetChartColorValue();
            var newControlProgramDescription = schedulingManagerPage.ControlProgramEditorPanel.GetDescriptionValue();
            VerifyEqual("7. Verify Name should be the same in grid", newControlProgramNameInGrid, newControlProgramName);
            VerifyEqual("7. Verify Color should be the same in grid", newControlProgramColorInGrid, newControlProgramColor);
            VerifyEqual("7. Verify Description: empty", string.Empty, newControlProgramDescription);

            Step(" - Template dropdown list has items:");
            Step("  + Astro ON/OFF");
            Step("  + Astro ON/OFF and fixed time events");
            Step("  + Always ON");
            Step("  + Always OFF");
            Step("  + Day fixed time events");
            Step("  + Advanced mode");
            var templateItems = schedulingManagerPage.ControlProgramEditorPanel.GetListOfTemplateItems();
            VerifyEqual("5. Verify Template dropdown list has items as expected", expectedTemplates, templateItems, false);

            Step("6. Select template 'Advanced mode'");
            schedulingManagerPage.ControlProgramEditorPanel.SelectTemplateDropDown(template);

            Step("7. Expected Details are updated according to the template:");
            Step(" - Control program table button is displayed");
            VerifyEqual("7. Verify Control program table button is displayed", true, schedulingManagerPage.ControlProgramEditorPanel.IsControlProgramItemsButtonVisible());

            Step(" - Switch ON: is not displayed");
            VerifyEqual("7. Verify Switch ON: is not displayed", false, schedulingManagerPage.ControlProgramEditorPanel.IsSwitchOnGroupVisible());

            Step(" - Switch OFF: is not displayed");
            VerifyEqual("7. Verify Switch OFF: is not displayed", false, schedulingManagerPage.ControlProgramEditorPanel.IsSwitchOffGroupVisible());

            Step(" - Variations: is not displayed");
            VerifyEqual("7. Verify Variations: is not displayed", false, schedulingManagerPage.ControlProgramEditorPanel.IsVariationsGroupVisible());

            Step(" - Timeline: is displayed = Moon icon");
            var expectedMoonBytes = schedulingManagerPage.ControlProgramEditorPanel.GetBytesOfMoonIcon();
            var actualTimelineIconBytes = schedulingManagerPage.ControlProgramEditorPanel.GetTimelineIconBytes();
            var result = ImageUtility.Compare(expectedMoonBytes, actualTimelineIconBytes);
            VerifyEqual("7. Verify Timeline: is displayed = Moon icon", true, result == 0);

            Step(" - Chart: variation points reflect dimming level. To verify this, hover the mouse on these points. When each point is hovered, 2 label appears: 1 is on the left with dimming level in percentage; 1 is at the bottom reflects the minutes; there is no either '+' or '-' for variation points");
            VerifyChartVariationsTooltip(schedulingManagerPage);

            Step("8. Change all editable fields");
            var newName = SLVHelper.GenerateUniqueName("CPSM61");
            var newColor = schedulingManagerPage.ControlProgramEditorPanel.GetChartColorValue();
            var newDescription = "Any description 1";
            EnterBasicValues(schedulingManagerPage, newName, newDescription);

            Step("9. Expected The chart refreshes its displaying (see chart verifying in previous step)");
            VerifyChartVariationsTooltip(schedulingManagerPage);

            Step("10. Double click inside the chart");
            var currentVariantionDotsCount = schedulingManagerPage.ControlProgramEditorPanel.GetChartVariationDotsCount();
            schedulingManagerPage.ControlProgramEditorPanel.DoubleClickRandomInsideChart();

            Step("12. Expected A new dot is displayed at the clicked position. There is a new correspondent entry in Variations section");
            var actualVariantionDotsCount = schedulingManagerPage.ControlProgramEditorPanel.GetChartVariationDotsCount();
            VerifyEqual("12. Verify A new dot is displayed at the clicked position", currentVariantionDotsCount + 1, actualVariantionDotsCount);

            Step("13. Change all editable fields again");
            newName = SLVHelper.GenerateUniqueName("CPSM62");
            newDescription = "Any description 2";
            EnterBasicValues(schedulingManagerPage, newName, newDescription);

            Step("14. Expected The chart refreshes its displaying (see chart verifying in previous step)");
            VerifyChartVariationsTooltip(schedulingManagerPage);

            Step("15. Save the new control program");
            schedulingManagerPage.ControlProgramEditorPanel.ClickSaveButton();
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("16. Expected The template field becomes readonly");
            VerifyEqual("16. Verify The template field becomes readonly", true, schedulingManagerPage.ControlProgramEditorPanel.IsTemplateDropDownReadOnly());

            Step("17. Reload browser and go to Scheduling Manager again");
            desktopPage = Browser.RefreshLoggedInCMS();
            schedulingManagerPage = desktopPage.GoToApp(App.SchedulingManager) as SchedulingManagerPage;

            Step("18. Select 'Control program' tab");
            schedulingManagerPage.SchedulingManagerPanel.SelectTab("Control program");
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("19. Expected The newly-created control program is still present in the grid");
            VerifyEqual("[SC-567] 19. Verify The newly-created control program is still present in the grid", true, schedulingManagerPage.SchedulingManagerPanel.IsControlProgramPresentInGrid(newName));

            Step("20. Select it");
            schedulingManagerPage.SchedulingManagerPanel.SelectControlProgram(newName);
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("21. Expected");
            Step(" - Control program editor panel is refreshed for the new item: all values should be remained");
            VerifyBasicValues(schedulingManagerPage, newName, newDescription, template);

            Step(" - Template field is readonly");
            VerifyEqual("21. Verify Template field is readonly", true, schedulingManagerPage.ControlProgramEditorPanel.IsTemplateDropDownReadOnly());
            Step(" - Verify the chart (see its verifying in previous step)");
            VerifyChartVariationsTooltip(schedulingManagerPage);

            Step("22. Change all editable fields again then save");
            newName = SLVHelper.GenerateUniqueName("CPSM63");
            newDescription = "Any description 3";
            EnterBasicValues(schedulingManagerPage, newName, newDescription);

            schedulingManagerPage.ControlProgramEditorPanel.ClickSaveButton();
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("23. Reload browser and go to Scheduling Manager again");
            desktopPage = Browser.RefreshLoggedInCMS();
            schedulingManagerPage = desktopPage.GoToApp(App.SchedulingManager) as SchedulingManagerPage;

            Step("24. Select 'Control program' tab");
            schedulingManagerPage.SchedulingManagerPanel.SelectTab("Control program");
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("25. Expected The newly-created control program is still present in the grid");
            VerifyEqual("[SC-567] 25. Verify The newly-created control program is still present in the grid", true, schedulingManagerPage.SchedulingManagerPanel.IsControlProgramPresentInGrid(newName));

            Step("26. Select it");
            schedulingManagerPage.SchedulingManagerPanel.SelectControlProgram(newName);
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("27. Expected");
            Step(" - Control program editor panel is refreshed for the new item: all values should be remained");
            VerifyBasicValues(schedulingManagerPage, newName, newDescription, template);
            Step(" - Template field is readonly");
            VerifyEqual("27. Verify Template field is readonly", true, schedulingManagerPage.ControlProgramEditorPanel.IsTemplateDropDownReadOnly());
            Step(" - Verify the chart (see its verifying in previous step)");
            VerifyChartVariationsTooltip(schedulingManagerPage);

            Step("28. Click Delete button from Control program grid");
            schedulingManagerPage.SchedulingManagerPanel.ClickDeleteControlProgramButton();

            Step("29. Expected A confirmation dialog appears with message 'Do you want to delete the control program '{{Control program name}}' ?'");
            schedulingManagerPage.WaitForPopupDialogDisplayed();
            var expectedMessage = string.Format("Do you want to delete the control program '{0}' ?", newName);
            VerifyEqual(string.Format("29. Verify A confirmation dialog appears with message '{0}'", expectedMessage), expectedMessage, schedulingManagerPage.Dialog.GetMessageText());

            Step("30. Click No");
            schedulingManagerPage.Dialog.ClickNoButton();

            Step("31. Expected The dialog disappears. The selected program is still present in the list. All values in Control program editor are still remained");
            schedulingManagerPage.WaitForPopupDialogDisappeared();
            VerifyEqual("31. Verify The selected program is still present in the list", newName, schedulingManagerPage.SchedulingManagerPanel.GetSelectedControlProgramName());
            VerifyBasicValues(schedulingManagerPage, newName, newDescription, template);

            Step("32. Click Delete button from Control program grid again");
            schedulingManagerPage.SchedulingManagerPanel.ClickDeleteControlProgramButton();

            Step("33. Expected A confirmation dialog appears with message 'Do you want to delete the control program '{{Control program name}}' ?'");
            schedulingManagerPage.WaitForPopupDialogDisplayed();
            VerifyEqual(string.Format("33. Verify A confirmation dialog appears with message '{0}'", expectedMessage), expectedMessage, schedulingManagerPage.Dialog.GetMessageText());

            Step("34. Click Yes");
            schedulingManagerPage.Dialog.ClickYesButton();
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("35. Expected The dialog disappears. The selected program is no longer present in the list. All values in Control program editor are still remained");
            schedulingManagerPage.WaitForPopupDialogDisappeared();
            VerifyEqual("35. Verify The selected program is no longer present in the list", false, schedulingManagerPage.SchedulingManagerPanel.IsControlProgramPresentInGrid(newName));

            Step("36. Reload browser and go to Scheduling Manager again");
            desktopPage = Browser.RefreshLoggedInCMS();
            schedulingManagerPage = desktopPage.GoToApp(App.SchedulingManager) as SchedulingManagerPage;

            Step("37. Select 'Control program' tab");
            schedulingManagerPage.SchedulingManagerPanel.SelectTab("Control program");
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("38. Expected The selected program is no longer present in the list");
            VerifyEqual("38. Verify The selected program is no longer present in the list", false, schedulingManagerPage.SchedulingManagerPanel.IsControlProgramPresentInGrid(newName));
        }

        [Test, DynamicRetry]
        [NonParallelizable]
        [Description("SM_07 Control program - Duplicate - Astro ON-OFF template")]
        public void SM_07()
        {
            var testData = GetTestDataOfTestSM07();
            var xmlTemplateName = testData["TemplateName"];

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.SchedulingManager);

            Step("1. Go to Scheduling Manager app");
            Step("2. Expected Scheduling Manager is routed and loaded successfully");
            var schedulingManagerPage = desktopPage.GoToApp(App.SchedulingManager) as SchedulingManagerPage;

            Step("3. Select 'Control program' tab in left panel");
            schedulingManagerPage.SchedulingManagerPanel.SelectTab("Control program");
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("4. Select a control whose template is 'Basic ON-OFF'");
            schedulingManagerPage.SchedulingManagerPanel.SelectControlProgram(xmlTemplateName);
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("5. Click Duplicate");
            schedulingManagerPage.SchedulingManagerPanel.ClickDuplicateControlProgramButton();
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("6. Expected A new item is added into control program grid");
            var name = schedulingManagerPage.ControlProgramEditorPanel.GetNameValue();
            VerifyEqual("6. Verify A new item is added into control program grid", true, schedulingManagerPage.SchedulingManagerPanel.IsControlProgramPresentInGrid(name));

            Step("7. Select another control");
            var listControlPrograms = schedulingManagerPage.SchedulingManagerPanel.GetListOfControlProgramName();
            listControlPrograms.Remove(xmlTemplateName);
            listControlPrograms.Remove(name);
            var randomControlProgram = listControlPrograms.PickRandom();
            schedulingManagerPage.SchedulingManagerPanel.SelectControlProgram(randomControlProgram);
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("8. Expected The new item which has been duplicated is no longer present in grid");
            Wait.ForSeconds(2);
            VerifyEqual(string.Format("8. Verify The new item which has been duplicated is no longer present in grid ({0})", name), false, schedulingManagerPage.SchedulingManagerPanel.IsControlProgramPresentInGrid(name));

            Step("9. Click Duplicate");
            schedulingManagerPage.SchedulingManagerPanel.ClickDuplicateControlProgramButton();
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("10. Expected A new item is added into control program grid");
            name = schedulingManagerPage.ControlProgramEditorPanel.GetNameValue();
            VerifyEqual("10. Verify A new item is added into control program grid", true, schedulingManagerPage.SchedulingManagerPanel.IsControlProgramPresentInGrid(name));

            Step("11. Reload browser and go to Scheduling Manager\\Control program again");
            desktopPage = Browser.RefreshLoggedInCMS();
            schedulingManagerPage = desktopPage.GoToApp(App.SchedulingManager) as SchedulingManagerPage;
            schedulingManagerPage.WaitForControlProgramDisplayed();

            Step("12. Expected The new item which has been duplicated is no longer present in grid");
            VerifyEqual("12. Verify The new item which has been duplicated is no longer present in grid", false, schedulingManagerPage.SchedulingManagerPanel.IsControlProgramPresentInGrid(name));

            Step("13. Select a control");
            schedulingManagerPage.SchedulingManagerPanel.SelectControlProgram(xmlTemplateName);
            schedulingManagerPage.WaitForPreviousActionComplete();
            var expectedColor = schedulingManagerPage.ControlProgramEditorPanel.GetChartColorValue();
            var expectedDescription = schedulingManagerPage.ControlProgramEditorPanel.GetDescriptionValue();
            var expectedTemplate = schedulingManagerPage.ControlProgramEditorPanel.GetTemplateValue();
            var expectedSwitchOnMinute = schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOnMinuteValue();
            var expectedSwitchOnRelation = schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOnRelationValue();
            var expectedSwitchOnLevel = schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOnLevelValue();
            var expectedSwitchOffMinute = schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOffMinuteValue();
            var expectedSwitchOffRelation = schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOffRelationValue();
            var expectedChartBytes = schedulingManagerPage.ControlProgramEditorPanel.GetBytesOfChart();

            Step("14. Click Duplicate");
            schedulingManagerPage.SchedulingManagerPanel.ClickDuplicateControlProgramButton();
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("15. Expected A new item is added into control program grid");
            name = schedulingManagerPage.ControlProgramEditorPanel.GetNameValue();
            VerifyEqual("15. Verify A new item is added into control program grid", true, schedulingManagerPage.SchedulingManagerPanel.IsControlProgramPresentInGrid(name));

            Step("16. Switch to an app then return");
            var usersPage = schedulingManagerPage.AppBar.SwitchTo(App.Users) as UsersPage;
            schedulingManagerPage = usersPage.AppBar.SwitchTo(App.SchedulingManager) as SchedulingManagerPage;
            schedulingManagerPage.SchedulingManagerPanel.SelectTab("Calendar");
            schedulingManagerPage.WaitForPreviousActionComplete();
            schedulingManagerPage.SchedulingManagerPanel.SelectTab("Control program");
            schedulingManagerPage.WaitForPreviousActionComplete();
            schedulingManagerPage.WaitForControlProgramDisplayed();            

            Step("17. Expected The new item which has been duplicated is still present in grid. Verify duplicated and duplicating has the same data on Color, Description, Template, Timeline and Dimming, Chart, Control Program items. Name of the new one has pattern 'New control program \\d{1,}'");
            VerifyEqual("[SC-1935] 17. Control Program Editor is displayed", true, schedulingManagerPage.IsControlProgramEditorDisplayed());
            VerifyEqual("17. Verify The new item which has been duplicated is still present in grid", true, schedulingManagerPage.SchedulingManagerPanel.IsControlProgramPresentInGrid(name));
            VerifyEqual("17. Verify Name of the new one has pattern 'New control program \\d{1,}'", true, Regex.IsMatch(name, @"New control program \d{1,}"));
            VerifyBasicValues(schedulingManagerPage, name, expectedDescription, expectedTemplate, expectedColor);
        }

        [Test, DynamicRetry]
        [NonParallelizable]
        [Description("SM_08 Control program - Duplicate - Astro ON-OFF and fixed time events template")]
        public void SM_08()
        {
            var testData = GetTestDataOfTestSM08();
            var xmlTemplateName = testData["TemplateName"];

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.SchedulingManager);

            Step("1. Go to Scheduling Manager app");
            Step("2. Expected Scheduling Manager is routed and loaded successfully");
            var schedulingManagerPage = desktopPage.GoToApp(App.SchedulingManager) as SchedulingManagerPage;

            Step("3. Select 'Control program' tab in left panel");
            schedulingManagerPage.SchedulingManagerPanel.SelectTab("Control program");
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("4. Select a control whose template is 'Basic ON-OFF + fixed time events'");
            schedulingManagerPage.SchedulingManagerPanel.SelectControlProgram(xmlTemplateName);
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("5. Click Duplicate");
            schedulingManagerPage.SchedulingManagerPanel.ClickDuplicateControlProgramButton();
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("6. Expected A new item is added into control program grid");
            var name = schedulingManagerPage.ControlProgramEditorPanel.GetNameValue();
            VerifyEqual("6. Verify A new item is added into control program grid", true, schedulingManagerPage.SchedulingManagerPanel.IsControlProgramPresentInGrid(name));

            Step("7. Select another control");
            var listControlPrograms = schedulingManagerPage.SchedulingManagerPanel.GetListOfControlProgramName();
            listControlPrograms.Remove(xmlTemplateName);
            listControlPrograms.Remove(name);
            var randomControlProgram = listControlPrograms.PickRandom();
            schedulingManagerPage.SchedulingManagerPanel.SelectControlProgram(randomControlProgram);
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("8. Expected The new item which has been duplicated is no longer present in grid");
            Wait.ForSeconds(2);
            VerifyEqual(string.Format("8. Verify The new item which has been duplicated is no longer present in grid ({0})", name), false, schedulingManagerPage.SchedulingManagerPanel.IsControlProgramPresentInGrid(name));

            Step("9. Click Duplicate");
            schedulingManagerPage.SchedulingManagerPanel.ClickDuplicateControlProgramButton();
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("10. Expected A new item is added into control program grid");
            name = schedulingManagerPage.ControlProgramEditorPanel.GetNameValue();
            VerifyEqual("10. Verify A new item is added into control program grid", true, schedulingManagerPage.SchedulingManagerPanel.IsControlProgramPresentInGrid(name));

            Step("11. Reload browser and go to Scheduling Manager\\Control program again");
            desktopPage = Browser.RefreshLoggedInCMS();
            schedulingManagerPage = desktopPage.GoToApp(App.SchedulingManager) as SchedulingManagerPage;
            schedulingManagerPage.WaitForControlProgramDisplayed();

            Step("12. Expected The new item which has been duplicated is no longer present in grid");
            VerifyEqual("12. Verify The new item which has been duplicated is no longer present in grid", false, schedulingManagerPage.SchedulingManagerPanel.IsControlProgramPresentInGrid(name));

            Step("13. Select a control");
            schedulingManagerPage.SchedulingManagerPanel.SelectControlProgram(xmlTemplateName);
            schedulingManagerPage.WaitForPreviousActionComplete();
            var expectedColor = schedulingManagerPage.ControlProgramEditorPanel.GetChartColorValue();
            var expectedDescription = schedulingManagerPage.ControlProgramEditorPanel.GetDescriptionValue();
            var expectedTemplate = schedulingManagerPage.ControlProgramEditorPanel.GetTemplateValue();
            var expectedSwitchOnMinute = schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOnMinuteValue();
            var expectedSwitchOnRelation = schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOnRelationValue();
            var expectedSwitchOnLevel = schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOnLevelValue();
            var expectedSwitchOffMinute = schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOffMinuteValue();
            var expectedSwitchOffRelation = schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOffRelationValue();
            var expectedVariationsTimeAndLevelList = schedulingManagerPage.ControlProgramEditorPanel.GetListOfVariationsTimeAndLevelInput();
            var expectedChartBytes = schedulingManagerPage.ControlProgramEditorPanel.GetBytesOfChart();

            Step("14. Click Duplicate");
            schedulingManagerPage.SchedulingManagerPanel.ClickDuplicateControlProgramButton();
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("15. Expected A new item is added into control program grid");
            name = schedulingManagerPage.ControlProgramEditorPanel.GetNameValue();
            VerifyEqual("15. Verify A new item is added into control program grid", true, schedulingManagerPage.SchedulingManagerPanel.IsControlProgramPresentInGrid(name));

            Step("16. Switch to an app then return");
            var usersPage = schedulingManagerPage.AppBar.SwitchTo(App.Users) as UsersPage;
            schedulingManagerPage = usersPage.AppBar.SwitchTo(App.SchedulingManager) as SchedulingManagerPage;
            schedulingManagerPage.SchedulingManagerPanel.SelectTab("Calendar");
            schedulingManagerPage.WaitForPreviousActionComplete();
            schedulingManagerPage.SchedulingManagerPanel.SelectTab("Control program");
            schedulingManagerPage.WaitForPreviousActionComplete();
            schedulingManagerPage.WaitForControlProgramDisplayed();

            Step("17. Expected The new item which has been duplicated is still present in grid. Verify duplicated and duplicating has the same data on Color, Description, Template, Timeline and Dimming, Chart, Control Program items. Name of the new one has pattern 'New control program \\d{1,}'");
            VerifyEqual("[SC-1935] 17. Control Program Editor is displayed", true, schedulingManagerPage.IsControlProgramEditorDisplayed());
            VerifyEqual("17. Verify The new item which has been duplicated is still present in grid", true, schedulingManagerPage.SchedulingManagerPanel.IsControlProgramPresentInGrid(name));
            VerifyEqual("17. Verify Name of the new one has pattern 'New control program \\d{1,}'", true, Regex.IsMatch(name, @"New control program \d{1,}"));
            VerifyBasicValues(schedulingManagerPage, name, expectedDescription, expectedTemplate, expectedColor);
        }

        [Test, DynamicRetry]
        [NonParallelizable]
        [Description("SM_09 Control program - Duplicate - Always ON template")]
        public void SM_09()
        {
            var testData = GetTestDataOfTestSM09();
            var xmlTemplateName = testData["TemplateName"];

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.SchedulingManager);

            Step("1. Go to Scheduling Manager app");
            Step("2. Expected Scheduling Manager is routed and loaded successfully");
            var schedulingManagerPage = desktopPage.GoToApp(App.SchedulingManager) as SchedulingManagerPage;

            Step("3. Select 'Control program' tab in left panel");
            schedulingManagerPage.SchedulingManagerPanel.SelectTab("Control program");
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("4. Select a control whose template is 'Always ON'");
            schedulingManagerPage.SchedulingManagerPanel.SelectControlProgram(xmlTemplateName);
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("5. Click Duplicate");
            schedulingManagerPage.SchedulingManagerPanel.ClickDuplicateControlProgramButton();
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("6. Expected A new item is added into control program grid");
            var name = schedulingManagerPage.ControlProgramEditorPanel.GetNameValue();
            VerifyEqual("6. Verify A new item is added into control program grid", true, schedulingManagerPage.SchedulingManagerPanel.IsControlProgramPresentInGrid(name));

            Step("7. Select another control");
            var listControlPrograms = schedulingManagerPage.SchedulingManagerPanel.GetListOfControlProgramName();
            listControlPrograms.Remove(xmlTemplateName);
            listControlPrograms.Remove(name);
            var randomControlProgram = listControlPrograms.PickRandom();
            schedulingManagerPage.SchedulingManagerPanel.SelectControlProgram(randomControlProgram);
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("8. Expected The new item which has been duplicated is no longer present in grid");
            Wait.ForSeconds(2);
            VerifyEqual(string.Format("8. Verify The new item which has been duplicated is no longer present in grid ({0})", name), false, schedulingManagerPage.SchedulingManagerPanel.IsControlProgramPresentInGrid(name));

            Step("9. Click Duplicate");
            schedulingManagerPage.SchedulingManagerPanel.ClickDuplicateControlProgramButton();
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("10. Expected A new item is added into control program grid");
            name = schedulingManagerPage.ControlProgramEditorPanel.GetNameValue();
            VerifyEqual("10. Verify A new item is added into control program grid", true, schedulingManagerPage.SchedulingManagerPanel.IsControlProgramPresentInGrid(name));

            Step("11. Reload browser and go to Scheduling Manager\\Control program again");
            desktopPage = Browser.RefreshLoggedInCMS();
            schedulingManagerPage = desktopPage.GoToApp(App.SchedulingManager) as SchedulingManagerPage;
            schedulingManagerPage.WaitForControlProgramDisplayed();

            Step("12. Expected The new item which has been duplicated is no longer present in grid");
            VerifyEqual("12. Verify The new item which has been duplicated is no longer present in grid", false, schedulingManagerPage.SchedulingManagerPanel.IsControlProgramPresentInGrid(name));

            Step("13. Select a control");
            schedulingManagerPage.SchedulingManagerPanel.SelectControlProgram(xmlTemplateName);
            schedulingManagerPage.WaitForPreviousActionComplete();
            var expectedColor = schedulingManagerPage.ControlProgramEditorPanel.GetChartColorValue();
            var expectedDescription = schedulingManagerPage.ControlProgramEditorPanel.GetDescriptionValue();
            var expectedTemplate = schedulingManagerPage.ControlProgramEditorPanel.GetTemplateValue();
            var expectedSwitchOnTime = schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOnTimeValue();
            var expectedSwitchOnLevel = schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOnLevelValue();
            var expectedChartBytes = schedulingManagerPage.ControlProgramEditorPanel.GetBytesOfChart();

            Step("14. Click Duplicate");
            schedulingManagerPage.SchedulingManagerPanel.ClickDuplicateControlProgramButton();
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("15. Expected A new item is added into control program grid");
            name = schedulingManagerPage.ControlProgramEditorPanel.GetNameValue();
            VerifyEqual("15. Verify A new item is added into control program grid", true, schedulingManagerPage.SchedulingManagerPanel.IsControlProgramPresentInGrid(name));

            Step("16. Switch to an app then return");
            var usersPage = schedulingManagerPage.AppBar.SwitchTo(App.Users) as UsersPage;
            schedulingManagerPage = usersPage.AppBar.SwitchTo(App.SchedulingManager) as SchedulingManagerPage;
            schedulingManagerPage.SchedulingManagerPanel.SelectTab("Calendar");
            schedulingManagerPage.WaitForPreviousActionComplete();
            schedulingManagerPage.SchedulingManagerPanel.SelectTab("Control program");
            schedulingManagerPage.WaitForPreviousActionComplete();
            schedulingManagerPage.WaitForControlProgramDisplayed();

            Step("17. Expected The new item which has been duplicated is still present in grid. Verify duplicated and duplicating has the same data on Color, Description, Template, Timeline and Dimming, Chart, Control Program items. Name of the new one has pattern 'New control program \\d{1,}'");
            VerifyEqual("[SC-1935] 17. Control Program Editor is displayed", true, schedulingManagerPage.IsControlProgramEditorDisplayed());
            VerifyEqual("17. Verify The new item which has been duplicated is still present in grid", true, schedulingManagerPage.SchedulingManagerPanel.IsControlProgramPresentInGrid(name));
            VerifyEqual("17. Verify Name of the new one has pattern 'New control program \\d{1,}'", true, Regex.IsMatch(name, @"New control program \d{1,}"));
            VerifyBasicValues(schedulingManagerPage, name, expectedDescription, expectedTemplate, expectedColor);
        }

        [Test, DynamicRetry]
        [NonParallelizable]
        [Description("SM_10 Control program - Duplicate - Always OFF template")]
        public void SM_10()
        {
            var testData = GetTestDataOfTestSM10();
            var xmlTemplateName = testData["TemplateName"];

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.SchedulingManager);

            Step("1. Go to Scheduling Manager app");
            Step("2. Expected Scheduling Manager is routed and loaded successfully");
            var schedulingManagerPage = desktopPage.GoToApp(App.SchedulingManager) as SchedulingManagerPage;

            Step("3. Select 'Control program' tab in left panel");
            schedulingManagerPage.SchedulingManagerPanel.SelectTab("Control program");
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("4. Select a control whose template is 'Always OFF'");
            schedulingManagerPage.SchedulingManagerPanel.SelectControlProgram(xmlTemplateName);
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("5. Click Duplicate");
            schedulingManagerPage.SchedulingManagerPanel.ClickDuplicateControlProgramButton();
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("6. Expected A new item is added into control program grid");
            var name = schedulingManagerPage.ControlProgramEditorPanel.GetNameValue();
            VerifyEqual("6. Verify A new item is added into control program grid", true, schedulingManagerPage.SchedulingManagerPanel.IsControlProgramPresentInGrid(name));

            Step("7. Select another control");
            var listControlPrograms = schedulingManagerPage.SchedulingManagerPanel.GetListOfControlProgramName();
            listControlPrograms.Remove(xmlTemplateName);
            listControlPrograms.Remove(name);
            var randomControlProgram = listControlPrograms.PickRandom();
            schedulingManagerPage.SchedulingManagerPanel.SelectControlProgram(randomControlProgram);
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("8. Expected The new item which has been duplicated is no longer present in grid");
            VerifyEqual(string.Format("8. Verify The new item which has been duplicated is no longer present in grid ({0})", name), false, schedulingManagerPage.SchedulingManagerPanel.IsControlProgramPresentInGrid(name));

            Step("9. Click Duplicate");
            schedulingManagerPage.SchedulingManagerPanel.ClickDuplicateControlProgramButton();
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("10. Expected A new item is added into control program grid");
            name = schedulingManagerPage.ControlProgramEditorPanel.GetNameValue();
            VerifyEqual(string.Format("10. Verify A new item is added into control program grid ({0})", name), true, schedulingManagerPage.SchedulingManagerPanel.IsControlProgramPresentInGrid(name));

            Step("11. Reload browser and go to Scheduling Manager\\Control program again");
            desktopPage = Browser.RefreshLoggedInCMS();
            schedulingManagerPage = desktopPage.GoToApp(App.SchedulingManager) as SchedulingManagerPage;
            schedulingManagerPage.WaitForControlProgramDisplayed();

            Step("12. Expected The new item which has been duplicated is no longer present in grid");
            VerifyEqual("12. Verify The new item which has been duplicated is no longer present in grid", false, schedulingManagerPage.SchedulingManagerPanel.IsControlProgramPresentInGrid(name));

            Step("13. Select a control");
            schedulingManagerPage.SchedulingManagerPanel.SelectControlProgram(xmlTemplateName);
            schedulingManagerPage.WaitForPreviousActionComplete();
            var expectedColor = schedulingManagerPage.ControlProgramEditorPanel.GetChartColorValue();
            var expectedDescription = schedulingManagerPage.ControlProgramEditorPanel.GetDescriptionValue();
            var expectedTemplate = schedulingManagerPage.ControlProgramEditorPanel.GetTemplateValue();

            Step("14. Click Duplicate");
            schedulingManagerPage.SchedulingManagerPanel.ClickDuplicateControlProgramButton();
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("15. Expected A new item is added into control program grid");
            name = schedulingManagerPage.ControlProgramEditorPanel.GetNameValue();
            VerifyEqual("15. Verify A new item is added into control program grid", true, schedulingManagerPage.SchedulingManagerPanel.IsControlProgramPresentInGrid(name));

            Step("16. Switch to an app then return");
            var usersPage = schedulingManagerPage.AppBar.SwitchTo(App.Users) as UsersPage;
            schedulingManagerPage = usersPage.AppBar.SwitchTo(App.SchedulingManager) as SchedulingManagerPage;
            schedulingManagerPage.SchedulingManagerPanel.SelectTab("Calendar");
            schedulingManagerPage.WaitForPreviousActionComplete();
            schedulingManagerPage.SchedulingManagerPanel.SelectTab("Control program");
            schedulingManagerPage.WaitForPreviousActionComplete();
            schedulingManagerPage.WaitForControlProgramDisplayed();

            Step(@"17. Expected The new item which has been duplicated is still present in grid. Verify duplicated and duplicating has the same data on Name, Description, Template, Chart, and color of Control Program items. Name of the new one has pattern 'New control program \d{1,}'");
            VerifyEqual("[SC-1935] 17. Control Program Editor is displayed", true, schedulingManagerPage.IsControlProgramEditorDisplayed());
            VerifyEqual("17. Verify The new item which has been duplicated is still present in grid", true, schedulingManagerPage.SchedulingManagerPanel.IsControlProgramPresentInGrid(name));
            VerifyEqual("17. Verify Name of the new one has pattern 'New control program \\d{1,}'", true, Regex.IsMatch(name, @"New control program \d{1,}"));
            VerifyBasicValues(schedulingManagerPage, name, expectedDescription, expectedTemplate, expectedColor);
        }

        [Test, DynamicRetry]
        [NonParallelizable]
        [Description("SM_11 Control program - Duplicate - Day fixed time events template")]
        public void SM_11()
        {
            var testData = GetTestDataOfTestSM11();
            var xmlTemplateName = testData["TemplateName"];

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.SchedulingManager);

            Step("1. Go to Scheduling Manager app");
            Step("2. Expected Scheduling Manager is routed and loaded successfully");
            var schedulingManagerPage = desktopPage.GoToApp(App.SchedulingManager) as SchedulingManagerPage;

            Step("3. Select 'Control program' tab in left panel");
            schedulingManagerPage.SchedulingManagerPanel.SelectTab("Control program");
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("4. Select a control whose template is 'Day fixed time events'");
            schedulingManagerPage.SchedulingManagerPanel.SelectControlProgram(xmlTemplateName);
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("5. Click Duplicate");
            schedulingManagerPage.SchedulingManagerPanel.ClickDuplicateControlProgramButton();
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("6. Expected A new item is added into control program grid");
            var name = schedulingManagerPage.ControlProgramEditorPanel.GetNameValue();
            VerifyEqual("6. Verify A new item is added into control program grid", true, schedulingManagerPage.SchedulingManagerPanel.IsControlProgramPresentInGrid(name));

            Step("7. Select another control");
            var listControlPrograms = schedulingManagerPage.SchedulingManagerPanel.GetListOfControlProgramName();
            listControlPrograms.Remove(xmlTemplateName);
            listControlPrograms.Remove(name);
            var randomControlProgram = listControlPrograms.PickRandom();
            schedulingManagerPage.SchedulingManagerPanel.SelectControlProgram(randomControlProgram);
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("8. Expected The new item which has been duplicated is no longer present in grid");
            Wait.ForSeconds(2);
            VerifyEqual(string.Format("8. Verify The new item which has been duplicated is no longer present in grid ({0})", name), false, schedulingManagerPage.SchedulingManagerPanel.IsControlProgramPresentInGrid(name));

            Step("9. Click Duplicate");
            schedulingManagerPage.SchedulingManagerPanel.ClickDuplicateControlProgramButton();
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("10. Expected A new item is added into control program grid");
            name = schedulingManagerPage.ControlProgramEditorPanel.GetNameValue();
            VerifyEqual("10. Verify A new item is added into control program grid", true, schedulingManagerPage.SchedulingManagerPanel.IsControlProgramPresentInGrid(name));

            Step("11. Reload browser and go to Scheduling Manager\\Control program again");
            desktopPage = Browser.RefreshLoggedInCMS();
            schedulingManagerPage = desktopPage.GoToApp(App.SchedulingManager) as SchedulingManagerPage;
            schedulingManagerPage.WaitForControlProgramDisplayed();

            Step("12. Expected The new item which has been duplicated is no longer present in grid");
            VerifyEqual("12. Verify The new item which has been duplicated is no longer present in grid", false, schedulingManagerPage.SchedulingManagerPanel.IsControlProgramPresentInGrid(name));

            Step("13. Select a control");
            schedulingManagerPage.SchedulingManagerPanel.SelectControlProgram(xmlTemplateName);
            schedulingManagerPage.WaitForPreviousActionComplete();
            var expectedColor = schedulingManagerPage.ControlProgramEditorPanel.GetChartColorValue();
            var expectedDescription = schedulingManagerPage.ControlProgramEditorPanel.GetDescriptionValue();
            var expectedTemplate = schedulingManagerPage.ControlProgramEditorPanel.GetTemplateValue();
            var expectedVariationsTimeAndLevelList = schedulingManagerPage.ControlProgramEditorPanel.GetListOfVariationsTimeAndLevelInput();
            var expectedChartBytes = schedulingManagerPage.ControlProgramEditorPanel.GetBytesOfChart();

            Step("14. Click Duplicate");
            schedulingManagerPage.SchedulingManagerPanel.ClickDuplicateControlProgramButton();
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("15. Expected A new item is added into control program grid");
            name = schedulingManagerPage.ControlProgramEditorPanel.GetNameValue();
            VerifyEqual("15. Verify A new item is added into control program grid", true, schedulingManagerPage.SchedulingManagerPanel.IsControlProgramPresentInGrid(name));

            Step("16. Switch to an app then return");
            var usersPage = schedulingManagerPage.AppBar.SwitchTo(App.Users) as UsersPage;
            schedulingManagerPage = usersPage.AppBar.SwitchTo(App.SchedulingManager) as SchedulingManagerPage;
            schedulingManagerPage.SchedulingManagerPanel.SelectTab("Calendar");
            schedulingManagerPage.WaitForPreviousActionComplete();
            schedulingManagerPage.SchedulingManagerPanel.SelectTab("Control program");
            schedulingManagerPage.WaitForPreviousActionComplete();
            schedulingManagerPage.WaitForControlProgramDisplayed();

            Step("17. Expected The new item which has been duplicated is still present in grid. Verify duplicated and duplicating has the same data on Color, Description, Template, Timeline and Dimming, Chart, Control Program items. Name of the new one has pattern 'New control program \\d{1,}'");
            VerifyEqual("[SC-1935] 17. Control Program Editor is displayed", true, schedulingManagerPage.IsControlProgramEditorDisplayed());
            VerifyEqual("17. Verify The new item which has been duplicated is still present in grid", true, schedulingManagerPage.SchedulingManagerPanel.IsControlProgramPresentInGrid(name));
            VerifyEqual("17. Verify Name of the new one has pattern 'New control program \\d{1,}'", true, Regex.IsMatch(name, @"New control program \d{1,}"));
            VerifyBasicValues(schedulingManagerPage, name, expectedDescription, expectedTemplate, expectedColor);
        }

        [Test, DynamicRetry]
        [NonParallelizable]
        [Description("SM_12 Control program - Duplicate - Advanced mode template")]
        public void SM_12()
        {
            var testData = GetTestDataOfTestSM12();
            var xmlTemplateName = testData["TemplateName"];

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.SchedulingManager);

            Step("1. Go to Scheduling Manager app");
            Step("2. Expected Scheduling Manager is routed and loaded successfully");
            var schedulingManagerPage = desktopPage.GoToApp(App.SchedulingManager) as SchedulingManagerPage;

            Step("3. Select 'Control program' tab in left panel");
            schedulingManagerPage.SchedulingManagerPanel.SelectTab("Control program");
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("4. Select a control whose template is 'Advanced mode'");
            schedulingManagerPage.SchedulingManagerPanel.SelectControlProgram(xmlTemplateName);
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("5. Click Duplicate");
            schedulingManagerPage.SchedulingManagerPanel.ClickDuplicateControlProgramButton();
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("6. Expected A new item is added into control program grid");
            var name = schedulingManagerPage.ControlProgramEditorPanel.GetNameValue();
            VerifyEqual("6. Verify A new item is added into control program grid", true, schedulingManagerPage.SchedulingManagerPanel.IsControlProgramPresentInGrid(name));

            Step("7. Select another control");
            var listControlPrograms = schedulingManagerPage.SchedulingManagerPanel.GetListOfControlProgramName();
            listControlPrograms.Remove(xmlTemplateName);
            listControlPrograms.Remove(name);
            var randomControlProgram = listControlPrograms.PickRandom();
            schedulingManagerPage.SchedulingManagerPanel.SelectControlProgram(randomControlProgram);
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("8. Expected The new item which has been duplicated is no longer present in grid");
            VerifyEqual(string.Format("8. Verify The new item which has been duplicated is no longer present in grid ({0})", name), false, schedulingManagerPage.SchedulingManagerPanel.IsControlProgramPresentInGrid(name));

            Step("9. Click Duplicate");
            schedulingManagerPage.SchedulingManagerPanel.ClickDuplicateControlProgramButton();
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("10. Expected A new item is added into control program grid");
            name = schedulingManagerPage.ControlProgramEditorPanel.GetNameValue();
            VerifyEqual("10. Verify A new item is added into control program grid", true, schedulingManagerPage.SchedulingManagerPanel.IsControlProgramPresentInGrid(name));

            Step("11. Reload browser and go to Scheduling Manager\\Control program again");
            desktopPage = Browser.RefreshLoggedInCMS();
            schedulingManagerPage = desktopPage.GoToApp(App.SchedulingManager) as SchedulingManagerPage;
            schedulingManagerPage.WaitForControlProgramDisplayed();

            Step("12. Expected The new item which has been duplicated is no longer present in grid");
            VerifyEqual("12. Verify The new item which has been duplicated is no longer present in grid", false, schedulingManagerPage.SchedulingManagerPanel.IsControlProgramPresentInGrid(name));

            Step("13. Select a control");
            schedulingManagerPage.SchedulingManagerPanel.SelectControlProgram(xmlTemplateName);
            schedulingManagerPage.WaitForPreviousActionComplete();
            var expectedColor = schedulingManagerPage.ControlProgramEditorPanel.GetChartColorValue();
            var expectedDescription = schedulingManagerPage.ControlProgramEditorPanel.GetDescriptionValue();
            var expectedTemplate = schedulingManagerPage.ControlProgramEditorPanel.GetTemplateValue();
            var expectedTimelineIconBytes = schedulingManagerPage.ControlProgramEditorPanel.GetTimelineIconBytes();

            Step("14. Click Duplicate");
            schedulingManagerPage.SchedulingManagerPanel.ClickDuplicateControlProgramButton();
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("15. Expected A new item is added into control program grid");
            name = schedulingManagerPage.ControlProgramEditorPanel.GetNameValue();
            VerifyEqual("15. Verify A new item is added into control program grid", true, schedulingManagerPage.SchedulingManagerPanel.IsControlProgramPresentInGrid(name));

            Step("16. Switch to an app then return");
            var usersPage = schedulingManagerPage.AppBar.SwitchTo(App.Users) as UsersPage;
            schedulingManagerPage = usersPage.AppBar.SwitchTo(App.SchedulingManager) as SchedulingManagerPage;
            schedulingManagerPage.SchedulingManagerPanel.SelectTab("Calendar");
            schedulingManagerPage.WaitForPreviousActionComplete();
            schedulingManagerPage.SchedulingManagerPanel.SelectTab("Control program");
            schedulingManagerPage.WaitForPreviousActionComplete();
            schedulingManagerPage.WaitForControlProgramDisplayed();

            Step("17. Expected The new item which has been duplicated is still present in grid. Verify duplicated and duplicating has the same data on Color, Description, Template, Timeline and Dimming, Chart, Control Program items. Name of the new one has pattern 'New control program \\d{1,}'");
            VerifyEqual("[SC-1935] 17. Control Program Editor is displayed", true, schedulingManagerPage.IsControlProgramEditorDisplayed());
            VerifyEqual("17. Verify The new item which has been duplicated is still present in grid", true, schedulingManagerPage.SchedulingManagerPanel.IsControlProgramPresentInGrid(name));
            VerifyEqual("17. Verify Name of the new one has pattern 'New control program \\d{1,}'", true, Regex.IsMatch(name, @"New control program \d{1,}"));
            VerifyBasicValues(schedulingManagerPage, name, expectedDescription, expectedTemplate, expectedColor);
            VerifyTimelineIcon(schedulingManagerPage, expectedTimelineIconBytes);
        }

        [Test, DynamicRetry]
        [Description("SM_13 Control program - Chart vs Control Program Items table")]
        public void SM_13()
        {
            var expectedTemplates = GetTemplates();
            var template = "Advanced mode";
            var random = new Random();

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.SchedulingManager);

            Step("1. Go to Scheduling Manager app");
            Step("2. Expected Scheduling Manager is routed and loaded successfully");
            var schedulingManagerPage = desktopPage.GoToApp(App.SchedulingManager) as SchedulingManagerPage;

            Step("3. Select 'Control program' tab in left panel");
            schedulingManagerPage.SchedulingManagerPanel.SelectTab("Control program");
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("4. Click Add new control program button");
            schedulingManagerPage.SchedulingManagerPanel.ClickAddControlProgramButton();
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("5. Expected");
            Step(" - A new item is orderedly inserted into Control program grid");
            Step("  + Color: a random color");
            Step("  + Name: pattern 'New control program \\d{1,}'");
            Step("  + Geozones: GeoZones");
            var newControlProgramNameInGrid = schedulingManagerPage.SchedulingManagerPanel.GetSelectedControlProgramName();
            var newControlProgramColorInGrid = schedulingManagerPage.SchedulingManagerPanel.GetSelectedControlProgramColor();
            var newControlProgramGeozoneInGrid = schedulingManagerPage.SchedulingManagerPanel.GetSelectedControlProgramGeozone();            
            VerifyEqual("5. Verify Color: a random color", true, newControlProgramColorInGrid != Color.Empty);
            VerifyEqual("5. Verify Name: pattern 'New control program \\d{1,}'", true, Regex.IsMatch(schedulingManagerPage.SchedulingManagerPanel.GetSelectedControlProgramName(), @"New control program \d{1,}"));
            VerifyEqual("5. Verify Geozones: GeoZones", "GeoZones", newControlProgramGeozoneInGrid);

            Step(" - Control program editor panel is refreshed for the new item:");
            Step("  + Name and color should be the same in grid");
            Step("  + Description: empty");
            var newControlProgramName = schedulingManagerPage.ControlProgramEditorPanel.GetNameValue();
            var newControlProgramColor = schedulingManagerPage.ControlProgramEditorPanel.GetChartColorValue();
            var newControlProgramDescription = schedulingManagerPage.ControlProgramEditorPanel.GetDescriptionValue();
            VerifyEqual("5. Verify Name should be the same in grid", newControlProgramNameInGrid, newControlProgramName);
            VerifyEqual("5. Verify Color should be the same in grid", newControlProgramColorInGrid, newControlProgramColor);
            VerifyEqual("5. Verify Description: empty", string.Empty, newControlProgramDescription);

            Step(" - Template dropdown list has items:");
            Step("  + Astro ON/OFF");
            Step("  + Astro ON/OFF and fixed time events");
            Step("  + Always ON");
            Step("  + Always OFF");
            Step("  + Day fixed time events");
            Step("  + Advanced mode");
            var templateItems = schedulingManagerPage.ControlProgramEditorPanel.GetListOfTemplateItems();
            VerifyEqual("5. Verify Template dropdown list has items as expected", expectedTemplates, templateItems, false);

            Step("6. Select template 'Advanced mode'");
            schedulingManagerPage.ControlProgramEditorPanel.SelectTemplateDropDown(template);

            Step("7. Expected Details are updated according to the template:");
            Step(" - Control program table button is displayed");
            VerifyEqual("7. Verify Control program table button is displayed", true, schedulingManagerPage.ControlProgramEditorPanel.IsControlProgramItemsButtonVisible());

            Step(" - Switch ON: is not displayed");
            VerifyEqual("7. Verify Switch ON: is not displayed", false, schedulingManagerPage.ControlProgramEditorPanel.IsSwitchOnGroupVisible());

            Step(" - Switch OFF: is not displayed");
            VerifyEqual("7. Verify Switch OFF: is not displayed", false, schedulingManagerPage.ControlProgramEditorPanel.IsSwitchOffGroupVisible());

            Step(" - Variations: is not displayed");
            VerifyEqual("7. Verify Variations: is not displayed", false, schedulingManagerPage.ControlProgramEditorPanel.IsVariationsGroupVisible());

            Step(" - Timeline: is displayed = Moon icon");
            var expectedMoonBytes = schedulingManagerPage.ControlProgramEditorPanel.GetBytesOfMoonIcon();
            var actualTimelineIconBytes = schedulingManagerPage.ControlProgramEditorPanel.GetTimelineIconBytes();
            var result = ImageUtility.Compare(expectedMoonBytes, actualTimelineIconBytes);
            VerifyEqual("7. Verify Timeline: is displayed = Moon icon", true, result == 0);

            Step(" - Chart: variation points reflect dimming level. To verify this, hover the mouse on these points. When each point is hovered, 2 label appears: 1 is on the left with dimming level in percentage; 1 is at the bottom reflects the minutes; there is no either '+' or '-' for variation points");
            VerifyChartVariationsTooltip(schedulingManagerPage);

            Step("8. Notes dots in chart (dimming level, time point)");
            var dotsTimeAndLevelList = schedulingManagerPage.ControlProgramEditorPanel.GetListOfTimeAndLevelChartDots();

            Step("9. Click Control program editor button");
            schedulingManagerPage.ControlProgramEditorPanel.ClickControlProgramItemsButton();
            schedulingManagerPage.WaitForPopupDialogDisplayed();

            Step("10. Expected Control program table panel appears. Verify dots vs rows in the table:");
            Step(" - Number of dots = number of rows");
            Step(" - Dimming levels");
            Step(" - Points are either diamond or circle or trianlge shape");
            VerifyControlProgramTable(schedulingManagerPage, dotsTimeAndLevelList);

            Step("12. Click Add item in table panel, update its time (not duplicated with existing ones) then hit Save button");
            schedulingManagerPage.ControlProgramItemsPopupPanel.ClickAddNewButton();
            schedulingManagerPage.ControlProgramItemsPopupPanel.EnterNewItemHourInput(random.Next(6, 11).ToString());
            schedulingManagerPage.ControlProgramItemsPopupPanel.EnterNewItemMinuteInput(random.Next(1, 59).ToString());
            schedulingManagerPage.ControlProgramItemsPopupPanel.EnterNewItemSecondInput(random.Next(1, 59).ToString());
            schedulingManagerPage.ControlProgramItemsPopupPanel.EnterNewItemLevelInput(random.Next(1, 99).ToString());
            schedulingManagerPage.ControlProgramItemsPopupPanel.ClickSaveButton();
            schedulingManagerPage.WaitForPopupDialogDisappeared();
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("13. Expected Control program table panel disappears. The chart refreshes. Verify dots vs rows (see previous expected step)");
            var newDotsTimeAndLevelList = schedulingManagerPage.ControlProgramEditorPanel.GetListOfTimeAndLevelChartDots();
            VerifyEqual("13. Verify new Dot was added", dotsTimeAndLevelList.Count + 1, newDotsTimeAndLevelList.Count);

            Step("14. Click Control program editor button");
            schedulingManagerPage.ControlProgramEditorPanel.ClickControlProgramItemsButton();
            schedulingManagerPage.WaitForPopupDialogDisplayed();

            Step("15. Expected Control program table panel appears. Verify dots vs rows in the table:");
            Step(" - Number of dots = number of rows");
            Step(" - Dimming levels");
            Step(" - Points are either diamond or circle or trianlge shape");
            VerifyControlProgramTable(schedulingManagerPage, newDotsTimeAndLevelList);

            Step("16. Expected Hour, Minute, Second values are displayed (to cover SC-693)");            
            VerifyEqual("[SC-693] 16. Verify Hour, Minute, Second values are displayed", true, schedulingManagerPage.ControlProgramItemsPopupPanel.AreTimeRecordsDisplayed());

            Step("17. Select a row and update its all values then hit Save button");
            schedulingManagerPage.ControlProgramItemsPopupPanel.SelectItem(random.Next(1, 3));
            schedulingManagerPage.ControlProgramItemsPopupPanel.EnterSelectedItemHourInput(random.Next(6, 11).ToString());
            schedulingManagerPage.ControlProgramItemsPopupPanel.EnterSelectedItemMinuteInput(random.Next(1, 59).ToString());
            schedulingManagerPage.ControlProgramItemsPopupPanel.EnterSelectedItemSecondInput(random.Next(1, 59).ToString());
            schedulingManagerPage.ControlProgramItemsPopupPanel.EnterSelectedItemLevelInput(random.Next(1, 99).ToString());
            schedulingManagerPage.ControlProgramItemsPopupPanel.ClickSaveButton();
            schedulingManagerPage.WaitForPopupDialogDisappeared();
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("18. Expected Control program table panel disappears. The chart refreshes. Verify dots vs rows (see previous expected step)");
            var updatedDotsTimeAndLevelList = schedulingManagerPage.ControlProgramEditorPanel.GetListOfTimeAndLevelChartDots();
            VerifyEqual("17. Verify no dots added or removed and all current dots are remained", newDotsTimeAndLevelList.Count, updatedDotsTimeAndLevelList.Count);

            Step("19. Click Control program editor button");
            schedulingManagerPage.ControlProgramEditorPanel.ClickControlProgramItemsButton();
            schedulingManagerPage.WaitForPopupDialogDisplayed();

            Step("20. Expected Control program table panel appears. Verify dots vs rows in the table:");
            Step(" - Number of dots = number of rows");
            Step(" - Dimming levels");
            Step(" - Points are either diamond or circle or trianlge shape");
            VerifyControlProgramTable(schedulingManagerPage, updatedDotsTimeAndLevelList);

            Step("21. Delete a row then hit Save button");
            schedulingManagerPage.ControlProgramItemsPopupPanel.SelectItem(random.Next(1, 3));
            schedulingManagerPage.ControlProgramItemsPopupPanel.ClickDeleteButton();
            schedulingManagerPage.WaitForPopupMessageDialogDisplayed();
            schedulingManagerPage.Dialog.ClickYesButton();
            schedulingManagerPage.WaitForPopupMessageDialogDisappeared();
            schedulingManagerPage.ControlProgramItemsPopupPanel.ClickSaveButton();
            schedulingManagerPage.WaitForPopupDialogDisappeared();
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("22. Expected Control program table panel disappears. The chart refreshes. Verify dots vs rows (see previous expected step)");
            var deletedDotsTimeAndLevelList = schedulingManagerPage.ControlProgramEditorPanel.GetListOfTimeAndLevelChartDots();
            VerifyEqual("Verify a Dot was deleted", updatedDotsTimeAndLevelList.Count - 1, deletedDotsTimeAndLevelList.Count);

            Step("23. Click Control program editor button");
            schedulingManagerPage.ControlProgramEditorPanel.ClickControlProgramItemsButton();
            schedulingManagerPage.WaitForPopupDialogDisplayed();

            Step("24. Expected Control program table panel appears. Verify dots vs rows in the table:");
            Step(" - Number of dots = number of rows");
            Step(" - Dimming levels");
            Step(" - Points are either diamond or circle or trianlge shape");
            VerifyControlProgramTable(schedulingManagerPage, deletedDotsTimeAndLevelList);

            Step("25. Hit Cancel button");
            schedulingManagerPage.ControlProgramItemsPopupPanel.ClickCancelButton();
            schedulingManagerPage.WaitForPopupDialogDisappeared();

            Step("26. Expected Control program table panel disappears. Verify no dots added or removed and all current dots are remained");
            dotsTimeAndLevelList = schedulingManagerPage.ControlProgramEditorPanel.GetListOfTimeAndLevelChartDots();
            VerifyEqual("Verify no dots added or removed and all current dots are remained", deletedDotsTimeAndLevelList.Count, dotsTimeAndLevelList.Count);

            Step("27. Double click on a random point inside the chart");
            schedulingManagerPage.ControlProgramEditorPanel.DoubleClickRandomInsideChart();
            schedulingManagerPage.ControlProgramEditorPanel.DoubleClickRandomInsideChart(); // for automation: hide a dot after double click.

            Step("28. Expected A new dot is added onto the chart");
            var currentChartDotsCount = schedulingManagerPage.ControlProgramEditorPanel.GetChartDotsCount();
            VerifyEqual("28. Verify A new dot is added onto the chart", dotsTimeAndLevelList.Count + 2, currentChartDotsCount);
            dotsTimeAndLevelList = schedulingManagerPage.ControlProgramEditorPanel.GetListOfTimeAndLevelChartDots();

            Step("29. Click Control program editor button");
            schedulingManagerPage.ControlProgramEditorPanel.ClickControlProgramItemsButton();
            schedulingManagerPage.WaitForPopupDialogDisplayed();

            Step("30. Expected Control program table panel appears. Verify dots vs rows in the table:");
            Step(" - Number of dots = number of rows");
            Step(" - Dimming levels");
            Step(" - Points are either diamond or circle or trianlge shape");
            VerifyControlProgramTable(schedulingManagerPage, dotsTimeAndLevelList);

            Step("31. Expected Hour, Minute, Second values are displayed (to cover SC-693)");            
            VerifyEqual("[SC-693] 16. Verify Hour, Minute, Second values are displayed", true, schedulingManagerPage.ControlProgramItemsPopupPanel.AreTimeRecordsDisplayed());

            Step("32. Hit Cancel button");
            schedulingManagerPage.ControlProgramItemsPopupPanel.ClickCancelButton();
            schedulingManagerPage.WaitForPopupDialogDisappeared();

            Step("33. Double-click on a point in chart. Update its value when a dialog for its values appears then hit Save button on that dialog");
            schedulingManagerPage.ControlProgramEditorPanel.DoubleClickRandomVariationDot();
            schedulingManagerPage.WaitForPopupDialogDisplayed();
            schedulingManagerPage.ControlProgramCommandTypePopupPanel.SelectTypeDropDown("Sun event");
            schedulingManagerPage.ControlProgramCommandTypePopupPanel.ClickSaveButton();
            schedulingManagerPage.WaitForPopupDialogDisappeared();
            schedulingManagerPage.WaitForPreviousActionComplete();
            dotsTimeAndLevelList = schedulingManagerPage.ControlProgramEditorPanel.GetListOfTimeAndLevelChartDots();

            Step("34. Click Control program editor button");
            schedulingManagerPage.ControlProgramEditorPanel.ClickControlProgramItemsButton();
            schedulingManagerPage.WaitForPopupDialogDisplayed();

            Step("35. Expected Control program table panel appears. Verify dots vs rows in the table:");
            Step(" - Number of dots = number of rows");
            Step(" - Dimming levels");
            Step(" - Points are either diamond or circle or trianlge shape");
            VerifyControlProgramTable(schedulingManagerPage, dotsTimeAndLevelList);

            Step("36. Expected Hour, Minute, Second values are displayed (to cover SC-693)");
            VerifyEqual("[SC-693] 16. Verify Hour, Minute, Second values are displayed", true, schedulingManagerPage.ControlProgramItemsPopupPanel.AreTimeRecordsDisplayed());

            try
            {
                //Remove new control program
                DeleteControlProgram(newControlProgramName);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("SM_14 Control program - Search")]
        public void SM_14()
        {
            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.SchedulingManager);

            Step("1. Go to Scheduling Manager app");
            Step("2. Expected Scheduling Manager is routed and loaded successfully");
            var schedulingManagerPage = desktopPage.GoToApp(App.SchedulingManager) as SchedulingManagerPage;

            Step("3. Select 'Control program' tab");
            schedulingManagerPage.SchedulingManagerPanel.SelectTab("Control program");
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("4. Enter in turn inexisting/existing name (partial and complete)");
            Step("5. Hit Enter or Search button (randomly)");
            Step("6. Expected Verify grid has no data in case of inexisting name only (partially or completely) matched name rows");
            Step("7. Select a found row");
            Step("8. Expected Control program editor loads data for the selected item (only need to verify name and color)");
            Step("--> Enter inexisting name");
            schedulingManagerPage.SchedulingManagerPanel.EnterSearchControlProgramInput(SLVHelper.GenerateUniqueName("Any"));
            schedulingManagerPage.SchedulingManagerPanel.ClickSearchControlProgramButton();
            Wait.ForSeconds(1);
            var controlProgramList = schedulingManagerPage.SchedulingManagerPanel.GetListOfControlProgramName();
            VerifyEqual("Verify grid has no data", 0, controlProgramList.Count);

            Step("--> Enter existing name (partially)");
            var search = "SLV-VN";
            schedulingManagerPage.SchedulingManagerPanel.ClickClearSearchControlProgramButton();
            schedulingManagerPage.SchedulingManagerPanel.EnterSearchControlProgramInput(search);
            schedulingManagerPage.SchedulingManagerPanel.ClickSearchControlProgramButton();
            Wait.ForSeconds(1);
            controlProgramList = schedulingManagerPage.SchedulingManagerPanel.GetListOfControlProgramName();
            VerifyEqual(string.Format("8. Verify grid has data matching '{0}'", search), true, controlProgramList.Count > 0 && controlProgramList.All(p => p.IndexOf(search) >= 0));

            var selectedName = controlProgramList.PickRandom();
            schedulingManagerPage.SchedulingManagerPanel.SelectControlProgram(selectedName);
            schedulingManagerPage.WaitForPreviousActionComplete();
            var selectedColor = schedulingManagerPage.SchedulingManagerPanel.GetSelectedControlProgramColor();
            VerifyEqual(string.Format("8. Verify Name is '{0}'", selectedName), selectedName, schedulingManagerPage.ControlProgramEditorPanel.GetNameValue());
            VerifyEqual(string.Format("8. Verify Color is '{0}'", selectedColor.GetKnownName()), selectedColor, schedulingManagerPage.ControlProgramEditorPanel.GetChartColorValue());

            Step("--> Enter existing name (completely)");
            search = "SLV-VN Advanced mode";
            schedulingManagerPage.SchedulingManagerPanel.ClickClearSearchControlProgramButton();
            schedulingManagerPage.SchedulingManagerPanel.EnterSearchControlProgramInput(search);
            schedulingManagerPage.SchedulingManagerPanel.ClickSearchControlProgramButton();
            Wait.ForSeconds(1);
            controlProgramList = schedulingManagerPage.SchedulingManagerPanel.GetListOfControlProgramName();
            VerifyEqual(string.Format("8. Verify grid has a row matching '{0}'", search), 1, controlProgramList.Count);

            schedulingManagerPage.SchedulingManagerPanel.SelectControlProgram(search);
            schedulingManagerPage.WaitForPreviousActionComplete();
            selectedColor = schedulingManagerPage.SchedulingManagerPanel.GetSelectedControlProgramColor();
            VerifyEqual(string.Format("8. Verify Name is '{0}'", search), search, schedulingManagerPage.ControlProgramEditorPanel.GetNameValue());
            VerifyEqual(string.Format("8. Verify Color is '{0}'", selectedColor.GetKnownName()), selectedColor, schedulingManagerPage.ControlProgramEditorPanel.GetChartColorValue());
        }

        [Test, DynamicRetry]
        [Description("SM_15 Calendar - Basic operations")]
        public void SM_15()
        {
            var geozone = SLVHelper.GenerateUniqueName("GZNSM15");
            var controller = SLVHelper.GenerateUniqueName("CTRL");
            var streetlight = SLVHelper.GenerateUniqueName("STL");
            var calendarName = SLVHelper.GenerateUniqueName("CSM15");

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            Step("-> Create data for testing");
            DeleteGeozones("GZNSM15*");
            CreateNewGeozone(geozone);
            CreateNewController(controller, geozone);
            CreateNewDevice(DeviceType.Streetlight, streetlight, controller, geozone);            

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.SchedulingManager);

            Step("1. Go to Scheduling Manager App");
            Step("2. Verify Scheduling Manager page is routed and loaded successfully");
            desktopPage = Browser.RefreshLoggedInCMS();
            var schedulingManagerPage = desktopPage.GoToApp(App.SchedulingManager) as SchedulingManagerPage;

            Step("3. Select 'Calendar' tab in left panel");
            schedulingManagerPage.SchedulingManagerPanel.SelectTab("Calendar");
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("4. Click Add new calendar button");
            schedulingManagerPage.SchedulingManagerPanel.ClickAddCalendarButton();
            schedulingManagerPage.WaitForPreviousActionComplete();
            var selectedCalendar = schedulingManagerPage.SchedulingManagerPanel.GetSelectedCalendarName();

            Step("5. Verify A new calendar is added in the the grid with");
            Step(" o Name: New calendar {1}");
            VerifyEqual("5. Verify A new calendar is added in the the grid with 'New calendar {1}'", true, Regex.IsMatch(selectedCalendar, @"New calendar \d{1,}"));

            Step("6. Verify The calendar editor is refreshed with");
            Step(" o The year: the current year");
            Step(" o Button: Calendar Item, Clear Calendar Item, Save");
            Step(" o Name: New calendar {1}");
            Step(" o Description is empty");
            Step(" o The text: 0 devices are using this calendar");
            VerifyEqual("6. Verify The year: the current year", Settings.GetServerTime().Year.ToString(), schedulingManagerPage.CalendarEditorPanel.GetCurrentYearText());           
            VerifyEqual("6. Verify Button: Calendar Item is displayed", true, schedulingManagerPage.CalendarEditorPanel.IsCalendarItemsButtonVisible());
            VerifyEqual("6. Verify Button: Clear Calendar Item is displayed", true, schedulingManagerPage.CalendarEditorPanel.IsClearButtonVisible());
            VerifyEqual("6. Verify Button: Save is displayed", true, schedulingManagerPage.CalendarEditorPanel.IsSaveButtonVisible());
            VerifyEqual("6. Name: New calendar {1}", true, Regex.IsMatch(selectedCalendar, @"New calendar \d{1,}"));
            VerifyEqual("6. Verify Description is empty", "", schedulingManagerPage.CalendarEditorPanel.GetDescriptionValue());
            VerifyEqual("6. Verify The text: 0 devices are using this calendar", "0 devices are using this calendar", schedulingManagerPage.CalendarEditorPanel.GetDevicesCountText());
            
            Step("7. Verify 12 months of the current year displays");
            var expectedMonthsName = new List<string> { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
            var actualMonthsName = schedulingManagerPage.CalendarEditorPanel.GetListOfCalendarMonthsName();
            VerifyEqual("7. Verify 12 months of the current year displays", expectedMonthsName, actualMonthsName);

            Step("8. Clear the value of Name textbox and press Save button");
            schedulingManagerPage.CalendarEditorPanel.ClearNameInput();            
            schedulingManagerPage.CalendarEditorPanel.ClickSaveButton();
            schedulingManagerPage.WaitForPopupDialogDisplayed();

            Step("9. Verify A notification pop-up displays");      
            Step(" o Text: Cannot create a calendar with an empty name");
            VerifyEqual("9. Verify A notification pop-up displays 'Cannot create a calendar with an empty name'", "Cannot create a calendar with an empty name", schedulingManagerPage.Dialog.GetMessageText());

            Step("10. Press OK button");
            schedulingManagerPage.Dialog.ClickOkButton();
            schedulingManagerPage.WaitForPopupDialogDisappeared();

            Step("11. Verify The pop-up is closed");
            VerifyEqual("11. Verify The pop-up is closed", true, !schedulingManagerPage.IsPopupDialogDisplayed());

            Step("12. Input the name.");
            schedulingManagerPage.CalendarEditorPanel.EnterNameInput(calendarName);
            Wait.ForSeconds(1);

            Step("13. Verify The name of calendar on the grid is updated to the new name");            
            VerifyEqual("13. Verify The name of calendar on the grid is updated to the new name", calendarName, schedulingManagerPage.SchedulingManagerPanel.GetSelectedCalendarName());

            Step("14. Press Save button");
            schedulingManagerPage.CalendarEditorPanel.ClickSaveButton();
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("15. Verify The calendar is saved successfully.");
            VerifyEqual("15. Verify The calendar is saved successfully", calendarName, schedulingManagerPage.SchedulingManagerPanel.GetSelectedCalendarName());
            VerifyEqual("15. Verify The calendar is saved successfully", calendarName, schedulingManagerPage.CalendarEditorPanel.GetNameValue());

            Step("16. Select the newly added calendar");
            schedulingManagerPage.SchedulingManagerPanel.SelectCalendar(calendarName);
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("17. Verify Calendar Editor panel is refreshed with");
            Step(" o The year: the current year");
            Step(" o Button: Calendar Item, Clear Calendar Item, Save");
            Step(" o Name: New inputted name");
            Step(" o Description is empty");
            Step(" o The text: 0 devices are using this calendar");
            VerifyEqual("17. Verify The year: the current year", Settings.GetServerTime().Year.ToString(), schedulingManagerPage.CalendarEditorPanel.GetCurrentYearText());
            VerifyEqual("17. Verify Button: Calendar Item is displayed", true, schedulingManagerPage.CalendarEditorPanel.IsCalendarItemsButtonVisible());
            VerifyEqual("17. Verify Button: Clear Calendar Item is displayed", true, schedulingManagerPage.CalendarEditorPanel.IsClearButtonVisible());
            VerifyEqual("17. Verify Button: Save is displayed", true, schedulingManagerPage.CalendarEditorPanel.IsSaveButtonVisible());
            VerifyEqual("17. Name: New inputted name", calendarName, schedulingManagerPage.CalendarEditorPanel.GetNameValue());
            VerifyEqual("17. Verify Description is empty", "", schedulingManagerPage.CalendarEditorPanel.GetDescriptionValue());
            VerifyEqual("17. Verify The text: 0 devices are using this calendar", "0 devices are using this calendar", schedulingManagerPage.CalendarEditorPanel.GetDevicesCountText());

            Step("18. Go to Equipment Inventory, select a streetlight in a geozone and update its dimming group to the newly created calendar");
            var equipmentInventoryPage = schedulingManagerPage.AppBar.SwitchTo(App.EquipmentInventory) as EquipmentInventoryPage;
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(geozone + @"\" + streetlight);
            equipmentInventoryPage.WaitForEditorPanelDisplayed();
            equipmentInventoryPage.StreetlightEditorPanel.SelectDimmingGroupDropDown(calendarName);
            equipmentInventoryPage.StreetlightEditorPanel.ClickSaveButton();
            equipmentInventoryPage.WaitForPreviousActionComplete();
            equipmentInventoryPage.WaitForEditorPanelDisappeared();

            Step("19. Go back to Schedule Manager and select the calendar again");
            schedulingManagerPage = equipmentInventoryPage.AppBar.SwitchTo(App.SchedulingManager) as SchedulingManagerPage;

            Step("20. Verify The text displays: '1 devices are using this calendar'. And the Calendar Icon next to the name in the grid is changed color to yellow");
            schedulingManagerPage.SchedulingManagerPanel.SelectCalendar(calendarName);
            schedulingManagerPage.WaitForPreviousActionComplete();
            VerifyEqual("20. Verify The text: 1 devices are using this calendar", "1 devices are using this calendar", schedulingManagerPage.CalendarEditorPanel.GetDevicesCountText());
            VerifyEqual("20. Verify Calendar Icon next to the name in the grid is changed color to 'used'", true, schedulingManagerPage.SchedulingManagerPanel.IsCalendarUsed());

            Step("21. Click Delete button");
            schedulingManagerPage.SchedulingManagerPanel.ClickDeleteCalendarButton();
            schedulingManagerPage.WaitForPopupDialogDisplayed();

            Step("22. Verify a notification pop-up displays");
            Step(" o Text: You cannot delete calendar currently used by devices.");
            VerifyEqual("28. Verify A confirmation pop-up displays: You cannot delete calendar currently used by devices.", "You cannot delete calendar currently used by devices.", schedulingManagerPage.Dialog.GetMessageText());

            Step("23. Press Ok");
            schedulingManagerPage.Dialog.ClickOkButton();
            schedulingManagerPage.WaitForPopupDialogDisappeared();

            Step("24. Verify The pop-up is closed and the calendar is still displayed in the grid");
            VerifyEqual("24. Verify The pop-up is closed", true, !schedulingManagerPage.IsPopupDialogDisplayed());
            VerifyEqual("24. Verify The calendar is still displayed in the grid", true, schedulingManagerPage.SchedulingManagerPanel.IsCalendarPresentInGrid(calendarName));

            Step("25. Go to Equipment Inventory and clear the dimming group of the previous streetlight, then go back to Schedule Manager and select the calendar again");
            equipmentInventoryPage = schedulingManagerPage.AppBar.SwitchTo(App.EquipmentInventory) as EquipmentInventoryPage;
            equipmentInventoryPage.WaitForEditorPanelDisplayed();
            equipmentInventoryPage.StreetlightEditorPanel.ClearDimmingGroupDropDown();
            equipmentInventoryPage.StreetlightEditorPanel.ClickSaveButton();
            equipmentInventoryPage.WaitForPreviousActionComplete();
            equipmentInventoryPage.WaitForEditorPanelDisappeared();

            schedulingManagerPage = equipmentInventoryPage.AppBar.SwitchTo(App.SchedulingManager) as SchedulingManagerPage;
            schedulingManagerPage.SchedulingManagerPanel.SelectCalendar(calendarName);
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("26. Verify The text displays: '0 devices are using this calendar'. And the Calendar Icon next to the name in the grid is changed color to brown");            
            VerifyEqual("26. Verify The text: 0 devices are using this calendar", "0 devices are using this calendar", schedulingManagerPage.CalendarEditorPanel.GetDevicesCountText());
            VerifyEqual("26. Verify Calendar Icon next to the name in the grid is changed color to 'not used'", false, schedulingManagerPage.SchedulingManagerPanel.IsCalendarUsed());

            Step("27. Click Delete button");
            schedulingManagerPage.SchedulingManagerPanel.ClickDeleteCalendarButton();
            schedulingManagerPage.WaitForPopupDialogDisplayed();

            Step("28. Verify The confirmation pop-up displays");
            Step(" o Text: Do you want to delete the calendar 'New inputted name' ?");
            var expectedDeleteMessage = string.Format("Do you want to delete the calendar '{0}' ?", calendarName);
            VerifyEqual("28. Verify A confirmation pop-up displays:" + expectedDeleteMessage, expectedDeleteMessage, schedulingManagerPage.Dialog.GetMessageText());

            Step("29. Select No button on the confirmation pop-up");
            schedulingManagerPage.Dialog.ClickNoButton();
            schedulingManagerPage.WaitForPopupDialogDisappeared();

            Step("30. Verify The calendar is still displayed in the grid");
            VerifyEqual("30. Verify The calendar is still displayed in the grid", true, schedulingManagerPage.SchedulingManagerPanel.IsCalendarPresentInGrid(calendarName));

            Step("31. Press Delete button again");
            schedulingManagerPage.SchedulingManagerPanel.ClickDeleteCalendarButton();
            schedulingManagerPage.WaitForPopupDialogDisplayed();
            schedulingManagerPage.Dialog.ClickYesButton();
            schedulingManagerPage.WaitForPreviousActionComplete();
            schedulingManagerPage.WaitForPopupDialogDisappeared();

            Step("32. Verify The calendar is deleted");
            VerifyEqual("32. Verify The calendar is deleted", true, !schedulingManagerPage.SchedulingManagerPanel.IsCalendarPresentInGrid(calendarName));
            
            try
            {
                DeleteGeozone(geozone);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("SM_16 Calendar - Duplicate")]
        public void SM_16()
        {
            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.SchedulingManager);

            Step("1. Go to Scheduling Manager app");
            Step("2. Expected Scheduling Manager is routed and loaded successfully");
            var schedulingManagerPage = desktopPage.GoToApp(App.SchedulingManager) as SchedulingManagerPage;

            Step("3. Select 'Calendar' tab in left panel");
            schedulingManagerPage.SchedulingManagerPanel.SelectTab("Calendar");
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("4. Select a calendar");
            var calendarList = schedulingManagerPage.SchedulingManagerPanel.GetListOfCalendarName();
            var randomCalendars = calendarList.PickRandom(2);
            schedulingManagerPage.SchedulingManagerPanel.SelectCalendar(randomCalendars[0]);
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("5. Click Duplicate");
            schedulingManagerPage.SchedulingManagerPanel.ClickDuplicateCalendarButton();
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("6. Expected A new item is added into control program grid");
            var newCalendarList = schedulingManagerPage.SchedulingManagerPanel.GetListOfCalendarName();
            VerifyEqual("6. Verify A new item is added into control program grid", calendarList.Count + 1, newCalendarList.Count);
            var selectedCalendar = schedulingManagerPage.SchedulingManagerPanel.GetSelectedCalendarName();

            Step("7. Select another calendar");
            schedulingManagerPage.SchedulingManagerPanel.SelectCalendar(randomCalendars[1]);
            schedulingManagerPage.WaitForPreviousActionComplete();
            calendarList = schedulingManagerPage.SchedulingManagerPanel.GetListOfCalendarName();
            var description = schedulingManagerPage.CalendarEditorPanel.GetDescriptionValue();
            var calendarDateDic = schedulingManagerPage.CalendarEditorPanel.GetCalendars();
            schedulingManagerPage.CalendarEditorPanel.ClickCalendarItemsButton();
            schedulingManagerPage.WaitForPopupDialogDisplayed();
            var calendarItemsList = schedulingManagerPage.CalendarEditorItemsPopupPanel.GetListOfItemsName();
            schedulingManagerPage.CalendarEditorItemsPopupPanel.ClickCancelButton();
            schedulingManagerPage.WaitForPopupDialogDisappeared();

            Step("8. Expected The new item which has been duplicated is no longer present in grid");
            calendarList = schedulingManagerPage.SchedulingManagerPanel.GetListOfCalendarName();
            VerifyEqual("8. Verify The new item which has been duplicated is no longer present in grid", false, schedulingManagerPage.SchedulingManagerPanel.IsCalendarPresentInGrid(selectedCalendar));

            Step("9. Click Duplicate");
            schedulingManagerPage.SchedulingManagerPanel.ClickDuplicateCalendarButton();
            schedulingManagerPage.WaitForPreviousActionComplete();
            selectedCalendar = schedulingManagerPage.SchedulingManagerPanel.GetSelectedCalendarName();

            Step("10. Expected A new item is added into control program grid. Verify duplicated and duplicating has the same data on Description, Calendars, Calendar items. Name of the new one has pattern 'New calendar \\d{1,}'. Devices used = 0");
            newCalendarList = schedulingManagerPage.SchedulingManagerPanel.GetListOfCalendarName();
            VerifyEqual("10. Verify A new item is added into control program grid", calendarList.Count + 1, newCalendarList.Count);
            var dupDevicesUsed = schedulingManagerPage.CalendarEditorPanel.GetDevicesUsed();
            var dupDescription = schedulingManagerPage.CalendarEditorPanel.GetDescriptionValue();
            var dupCalendarDateDic = schedulingManagerPage.CalendarEditorPanel.GetCalendars();
            schedulingManagerPage.CalendarEditorPanel.ClickCalendarItemsButton();
            schedulingManagerPage.WaitForPopupDialogDisplayed();
            var dupCalendarItemsList = schedulingManagerPage.CalendarEditorItemsPopupPanel.GetListOfItemsName();
            schedulingManagerPage.CalendarEditorItemsPopupPanel.ClickCancelButton();
            schedulingManagerPage.WaitForPopupDialogDisappeared();

            VerifyEqual("10. Verify duplicated and duplicating has the same data on Description", description, dupDescription);
            VerifyEqual("10. Verify duplicated and duplicating has the same data on Calendar items", calendarItemsList, dupCalendarItemsList, false);
            VerifyEqual("10. Verify duplicated and duplicating has the same data on Calendars", true, calendarDateDic.DictionaryEqual(dupCalendarDateDic));
            VerifyEqual("10. Verify Name of the new one has pattern 'New calendar \\d{1,}'", true, Regex.IsMatch(selectedCalendar, @"New calendar \d{1,}"));
            VerifyEqual("10. Verify Devices used = 0", 0, dupDevicesUsed);

            Step("11. Reload browser and go to Scheduling Manager\\Calendar again");
            desktopPage = Browser.RefreshLoggedInCMS();
            schedulingManagerPage = desktopPage.GoToApp(App.SchedulingManager) as SchedulingManagerPage;
            schedulingManagerPage.SchedulingManagerPanel.SelectTab("Calendar");
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("12. Expected The new item which has been duplicated is no longer present in grid");
            VerifyEqual("12. Verify The new item which has been duplicated is no longer present in grid", false, schedulingManagerPage.SchedulingManagerPanel.IsCalendarPresentInGrid(selectedCalendar));
        }

        [Test, DynamicRetry]
        [Description("SM_17 Calendar - Search")]
        public void SM_17()
        {
            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.SchedulingManager);

            Step("1. Go to Scheduling Manager app");
            Step("2. Expected Scheduling Manager is routed and loaded successfully");
            var schedulingManagerPage = desktopPage.GoToApp(App.SchedulingManager) as SchedulingManagerPage;

            Step("3. Select 'Calendar' tab");
            schedulingManagerPage.SchedulingManagerPanel.SelectTab("Calendar");
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("4. Enter in turn inexisting/existing name (partial and complete)");
            Step("5. Hit Enter or Search button (randomly)");
            Step("6. Expected Verify grid has no data in case of inexisting name only (partially or completely) matched name rows");
            Step("7. Select a found row");
            Step("8. Expected Calendar editor loads data for the selected item (only need to verify name)");
            Step("--> Enter inexisting name");
            schedulingManagerPage.SchedulingManagerPanel.EnterSearchCalendarInput(SLVHelper.GenerateUniqueName("Any"));
            schedulingManagerPage.SchedulingManagerPanel.ClickSearchCalendarButton();
            var controlProgramList = schedulingManagerPage.SchedulingManagerPanel.GetListOfCalendarName();
            VerifyEqual("Verify grid has no data", 0, controlProgramList.Count);

            Step("--> Enter existing name (partially)");
            var search = "SlvDemo";
            schedulingManagerPage.SchedulingManagerPanel.ClickClearSearchCalendarButton();
            schedulingManagerPage.SchedulingManagerPanel.EnterSearchCalendarInput(search);
            schedulingManagerPage.SchedulingManagerPanel.ClickSearchCalendarButton();
            controlProgramList = schedulingManagerPage.SchedulingManagerPanel.GetListOfCalendarName();
            VerifyEqual(string.Format("8. Verify grid has data matching '{0}'", search), true, controlProgramList.Count > 0 && controlProgramList.All(p => p.IndexOf(search) >= 0));

            var selectedName = controlProgramList.PickRandom();
            schedulingManagerPage.SchedulingManagerPanel.SelectCalendar(selectedName);
            schedulingManagerPage.WaitForPreviousActionComplete();
            VerifyEqual(string.Format("8. Verify Name is '{0}'", selectedName), selectedName, schedulingManagerPage.CalendarEditorPanel.GetNameValue());

            Step("--> Enter existing name (completely)");
            search = "SlvDemoGroup2";
            schedulingManagerPage.SchedulingManagerPanel.ClickClearSearchCalendarButton();
            schedulingManagerPage.SchedulingManagerPanel.EnterSearchCalendarInput(search);
            schedulingManagerPage.SchedulingManagerPanel.ClickSearchCalendarButton();
            controlProgramList = schedulingManagerPage.SchedulingManagerPanel.GetListOfCalendarName();
            VerifyEqual(string.Format("8. Verify grid has a row matching '{0}'", search), 1, controlProgramList.Count);

            schedulingManagerPage.SchedulingManagerPanel.SelectCalendar(search);
            schedulingManagerPage.WaitForPreviousActionComplete();
            VerifyEqual(string.Format("8. Verify Name is '{0}'", search), search, schedulingManagerPage.CalendarEditorPanel.GetNameValue());
        }

        [Test, DynamicRetry]
        [Description("SM_18_01 Calendar - Editor operations")]
        public void SM_18_01()
        {
            var calendarName = SLVHelper.GenerateUniqueName("SM1801");
            CreateNewCalendar(calendarName);

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - There is an existing calendar");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.SchedulingManager);

            Step("1. Go to Scheduling Manager app");
            Step("2. Expected Scheduling Manager is routed and loaded successfully");
            var schedulingManagerPage = desktopPage.GoToApp(App.SchedulingManager) as SchedulingManagerPage;

            Step("3. Select 'Calendar' tab in left panel");
            schedulingManagerPage.SchedulingManagerPanel.SelectTab("Calendar");

            Step("4. Select an existing calendar");
            schedulingManagerPage.SchedulingManagerPanel.SelectCalendar(calendarName);
            schedulingManagerPage.WaitForPreviousActionComplete();

            //Step("5. Clear the name of the calendar and press Save button");
            //schedulingManagerPage.CalendarEditorPanel.ClearNameInput();
            //schedulingManagerPage.CalendarEditorPanel.ClickSaveButton();
            //schedulingManagerPage.WaitForPreviousActionComplete();

            //Step("6. Verify currently we can save an empty name for calendar. As Jean-Marc confirmation. This should be a improvement but no ticket created yet. Marked this as a warning.");
            //var isNameEmpty = string.IsNullOrEmpty(schedulingManagerPage.SchedulingManagerPanel.GetSelectedCalendarName());
            //if (isNameEmpty)
            //{
            //    Warning("6. Verify currently we can save an empty name for calendar (As Jean-Marc confirmation. This should be a improvement but no ticket created yet. Marked this as a warning.)");
            //}

            Step("7. Update the name and description with the new value and press Save button");
            var updatedCalendarName = SLVHelper.GenerateUniqueName("CSM18-Upd1");
            var description = SLVHelper.GenerateUniqueName("Description1");
            schedulingManagerPage.CalendarEditorPanel.EnterNameInput(updatedCalendarName);
            schedulingManagerPage.CalendarEditorPanel.EnterDescriptionInput(description);
            schedulingManagerPage.CalendarEditorPanel.ClickSaveButton();
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("8. Verify The name and the description is updated. The calendar name on the grid is also updated.");
            VerifyEqual("8. Verify The name is updated", updatedCalendarName, schedulingManagerPage.CalendarEditorPanel.GetNameValue());
            VerifyEqual("8. Verify The description is updated", description, schedulingManagerPage.CalendarEditorPanel.GetDescriptionValue());
            VerifyEqual("8. Verify The calendar name on the grid is also updated", updatedCalendarName, schedulingManagerPage.SchedulingManagerPanel.GetSelectedCalendarName());

            Step("9. Click randomly on a day in a month");
            var randomDate = schedulingManagerPage.CalendarEditorPanel.ClickRandomCalendarDate();
            schedulingManagerPage.WaitForPopupDialogDisplayed();

            Step("10. Verify Control programs pop-up displays");
            var hasPopupDialogDisplayed = schedulingManagerPage.HasPopupDialogDisplayed();
            VerifyEqual("10. Verify Control programs pop-up displays", true, hasPopupDialogDisplayed);

            Step("11. Select a Control Programs in the list. Take note the color and the name of the control program");
            schedulingManagerPage.CalendarControlProgramsPopupPanel.SelectItem(schedulingManagerPage.CalendarControlProgramsPopupPanel.GetListOfItemsName().PickRandom());
            var notedColor = schedulingManagerPage.CalendarControlProgramsPopupPanel.GetSelectedItemColor();

            Step("12. Press Save button on the Control Programs pop-up");
            schedulingManagerPage.CalendarControlProgramsPopupPanel.ClickSaveButton();
            schedulingManagerPage.WaitForPopupDialogDisappeared();

            Step("13. Verify The background color of the date cell is changed to the color of the control program");
            var colorDate = schedulingManagerPage.CalendarEditorPanel.GetCalendarDateColor(randomDate);
            VerifyEqual("13. Verify The background color of the date cell is changed to the color of the control program", notedColor, colorDate);

            Step("14. Select another calendar in the grid, then select back the testing calendar again");
            schedulingManagerPage.SchedulingManagerPanel.SelectRandomCalendar();
            schedulingManagerPage.WaitForPreviousActionComplete();
            schedulingManagerPage.SchedulingManagerPanel.SelectCalendar(updatedCalendarName);
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("15. Verify The color of date cell is not saved all changes");
            var reselectColorDate = schedulingManagerPage.CalendarEditorPanel.GetCalendarDateColor(randomDate);
            VerifyTrue("15. Verify The color of date cell is not saved all changes", notedColor != reselectColorDate, notedColor, reselectColorDate);

            Step("16. Update name, description and set a control program for a date again");
            updatedCalendarName = SLVHelper.GenerateUniqueName("CSM18-Upd2");
            description = SLVHelper.GenerateUniqueName("Description2");
            schedulingManagerPage.CalendarEditorPanel.EnterNameInput(updatedCalendarName);
            schedulingManagerPage.CalendarEditorPanel.EnterDescriptionInput(description);
            randomDate = schedulingManagerPage.CalendarEditorPanel.ClickRandomCalendarDate();
            schedulingManagerPage.WaitForPopupDialogDisplayed();
            schedulingManagerPage.CalendarControlProgramsPopupPanel.SelectItem(schedulingManagerPage.CalendarControlProgramsPopupPanel.GetListOfItemsName().PickRandom());
            notedColor = schedulingManagerPage.CalendarControlProgramsPopupPanel.GetSelectedItemColor();
            schedulingManagerPage.CalendarControlProgramsPopupPanel.ClickSaveButton();
            schedulingManagerPage.WaitForPopupDialogDisappeared();

            Step("17. Press Save button on the Calendar Editor panel");
            schedulingManagerPage.CalendarEditorPanel.ClickSaveButton();
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("18. Select another calendar in the grid, then select back the testing calendar again");
            schedulingManagerPage.SchedulingManagerPanel.SelectRandomCalendar();
            schedulingManagerPage.WaitForPreviousActionComplete();
            schedulingManagerPage.SchedulingManagerPanel.SelectCalendar(updatedCalendarName);
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("19. Verify The name, description, and color of date cell are now changed correctly");
            VerifyEqual("19. Verify The name is changed correctly", updatedCalendarName, schedulingManagerPage.CalendarEditorPanel.GetNameValue());
            VerifyEqual("19. Verify The description is changed correctly", description, schedulingManagerPage.CalendarEditorPanel.GetDescriptionValue());
            VerifyEqual("19. Verify The color of date cell is changed correctly", notedColor, schedulingManagerPage.CalendarEditorPanel.GetCalendarDateColor(randomDate));

            Step("20. Press Eraser button");
            schedulingManagerPage.CalendarEditorPanel.ClickClearButton();
            schedulingManagerPage.WaitForPopupDialogDisplayed();

            Step("21. Verify a confirmation pop-up diplays with text: 'Do you want to clear all calendar items ?'");
            VerifyEqual("21. Verify a confirmation pop-up diplays with text: 'Do you want to clear all calendar items ?'", "Do you want to clear all calendar items ?", schedulingManagerPage.Dialog.GetMessageText());

            Step("22. Press No button");
            schedulingManagerPage.Dialog.ClickNoButton();
            schedulingManagerPage.WaitForPopupDialogDisappeared();

            Step("23. Verify The background color of the date cell is still not changed.");
            VerifyEqual("23. Verify The color of date cell is changed correctly", notedColor, schedulingManagerPage.CalendarEditorPanel.GetCalendarDateColor(randomDate));

            Step("24. Press Eraser button again and press Yes button");
            schedulingManagerPage.CalendarEditorPanel.ClickClearButton();
            schedulingManagerPage.WaitForPopupDialogDisplayed();
            schedulingManagerPage.Dialog.ClickYesButton();
            schedulingManagerPage.WaitForPopupDialogDisappeared();

            Step("25. Verify The background color of the date cell");
            VerifyEqual("25. Verify No background color of the date cell", Color.Empty, schedulingManagerPage.CalendarEditorPanel.GetCalendarDateColor(randomDate));

            Step("26. Press Save button on the Calendar Editor panel");
            schedulingManagerPage.CalendarEditorPanel.ClickSaveButton();
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("27. Select another calendar in the grid, then select back the testing calendar again");
            schedulingManagerPage.SchedulingManagerPanel.SelectRandomCalendar();
            schedulingManagerPage.WaitForPreviousActionComplete();
            schedulingManagerPage.SchedulingManagerPanel.SelectCalendar(updatedCalendarName);
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("28. Verify No background color of the date cell");
            VerifyEqual("28. Verify No background color of the date cell", Color.Empty, schedulingManagerPage.CalendarEditorPanel.GetCalendarDateColor(randomDate));

            try
            {
                Step("29. Delete the testing calendar after testcase is done.");
                DeleteCalendar(updatedCalendarName);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("SM_18_02 Calendar - Calendar editor - Control Program pop-up - Set a control program yearly for a day in a month.")]
        public void SM_18_02()
        {
            var calendarName = SLVHelper.GenerateUniqueName("CSM1802");
            CreateNewCalendar(calendarName);

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - There is an existing calendar");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.SchedulingManager);

            Step("1. Go to Scheduling Manager app");
            Step("2. Expected Scheduling Manager is routed and loaded successfully");
            var schedulingManagerPage = desktopPage.GoToApp(App.SchedulingManager) as SchedulingManagerPage;

            Step("3. Select 'Calendar' tab in left panel");
            schedulingManagerPage.SchedulingManagerPanel.SelectTab("Calendar");
            schedulingManagerPage.SchedulingManagerPanel.SelectCalendar(calendarName);
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("4. Press a day in a month");
            var randomDate = schedulingManagerPage.CalendarEditorPanel.ClickRandomCalendarDate();
            schedulingManagerPage.WaitForPopupDialogDisplayed();

            Step("5. Verify The Control Program pop-up displays with");
            Step(" o Title: Control programs");
            Step(" o All Fields Search textbox and Search Icon");
            Step(" o 4 options: None, Weekly, Monthly, Yearly");
            Step(" o Yearly is selected as default");
            Step(" o List of existing Control Programs with 3 columns: Color, Control Programs Name, Geozone");
            VerifyEqual("5. Verify Title: Control programs", "Control programs", schedulingManagerPage.CalendarControlProgramsPopupPanel.GetPanelTitleText().SplitAndGetAt(1));
            VerifyEqual("5. Verify All Fields Search textbox displays", true, schedulingManagerPage.CalendarControlProgramsPopupPanel.IsSearchInputVisible());            
            VerifyEqual("5. Verify 4 options: None, Weekly, Monthly, Yearly", new List<string> { "None", "Weekly", "Monthly", "Yearly" }, schedulingManagerPage.CalendarControlProgramsPopupPanel.GetListOfOptionsName());
            VerifyEqual("5. Verify Yearly is selected as default", "Yearly", schedulingManagerPage.CalendarControlProgramsPopupPanel.GetSelectedOptionName());
            VerifyEqual("5. Verify List of existing Control Programs with 3 columns: Color, Control Programs Name, Geozone", 3, schedulingManagerPage.CalendarControlProgramsPopupPanel.GetListOfColumnsCount());

            Step("6. Select a Control Program and press Save button");
            var controlProgramName = schedulingManagerPage.CalendarControlProgramsPopupPanel.GetListOfItemsName().PickRandom();
            schedulingManagerPage.CalendarControlProgramsPopupPanel.SelectItem(controlProgramName);
            var notedColor = schedulingManagerPage.CalendarControlProgramsPopupPanel.GetSelectedItemColor();
            schedulingManagerPage.CalendarControlProgramsPopupPanel.ClickSaveButton();
            schedulingManagerPage.WaitForPopupDialogDisappeared();

            Step("7. Verify The selected day in a month is set the color of the Control Program.");
            var colorDate = schedulingManagerPage.CalendarEditorPanel.GetCalendarDateColor(randomDate);
            VerifyEqual("7. Verify The selected day in a month is set the color of the Control Program", notedColor, colorDate);

            Step("8. Hover the day");
            schedulingManagerPage.CalendarEditorPanel.HoverCalendarDate(randomDate);

            Step("9. Verify the tooltip displays with 2 rows");
            Step(" o Clock Icon and the text: 'Each year the [full month] [date]'");
            Step(" o Color and Name of the Control Program");
            var date = DateTime.Parse(randomDate);
            var monthName = date.ToString("MMMM");
            var day = date.Day;
            var tooltipIcon = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateTitleIcon();
            var tooltipTitleText = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateTitleText();
            var tooltipControlProgramName = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateControlProgramName();
            var expectedTitle = string.Format("Each year the {0} {1}", monthName, day);
            var expectedTitleDayWith0 = string.Format("Each year the {0} {1}", monthName, day.ToString("D2"));
            VerifyEqual("9. Verify Clock Icon displayed", true, tooltipIcon.Contains("clock.png"));
            VerifyTrue("9. Verify the text: 'Each year the [full month] [date]'", expectedTitle == tooltipTitleText || expectedTitleDayWith0 == tooltipTitleText, expectedTitle + "/" + expectedTitleDayWith0, tooltipTitleText);     
            VerifyEqual("9. Verify Color and Name of the Control Program", controlProgramName, tooltipControlProgramName);
            
            Step("10. Press Next button to move to the next year");
            schedulingManagerPage.CalendarEditorPanel.ClickYearAfterButton();

            Step("11. Verify The day in a month in the next year is also set the color of the Control Program");
            var nextDate = string.Format(@"{0}/{1}/{2}", date.Month, date.Day, schedulingManagerPage.CalendarEditorPanel.GetCurrentYearText());
            colorDate = schedulingManagerPage.CalendarEditorPanel.GetCalendarDateColor(nextDate);
            VerifyEqual("11. Verify The selected day in a month is set the color of the Control Program", notedColor, colorDate);

            Step("12. Hover the day");
            schedulingManagerPage.CalendarEditorPanel.HoverCalendarDate(nextDate);
            tooltipIcon = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateTitleIcon();
            tooltipTitleText = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateTitleText();
            tooltipControlProgramName = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateControlProgramName();

            Step("13. Verify the tooltip displays with 2 rows");
            Step(" o Clock Icon and the text: 'Each year the [full month] [date]'");
            Step(" o Color and Name of the Control Program");
            VerifyEqual("13. Verify Clock Icon displayed", true, tooltipIcon.Contains("clock.png"));
            VerifyTrue("13. Verify the text: 'Each year the [full month] [date]'", expectedTitle == tooltipTitleText || expectedTitleDayWith0 == tooltipTitleText, expectedTitle + "/" + expectedTitleDayWith0, tooltipTitleText);
            VerifyEqual("13. Verify Color and Name of the Control Program", controlProgramName, tooltipControlProgramName);

            Step("14. Press Back button twice to move to the previous year");
            schedulingManagerPage.CalendarEditorPanel.ClickYearBeforeButton();
            schedulingManagerPage.CalendarEditorPanel.ClickYearBeforeButton();

            Step("15. Verify tooltip and color the same with next year");
            var previousDate = string.Format(@"{0}/{1}/{2}", date.Month, date.Day, schedulingManagerPage.CalendarEditorPanel.GetCurrentYearText());
            colorDate = schedulingManagerPage.CalendarEditorPanel.GetCalendarDateColor(previousDate);
            VerifyEqual("15. Verify The selected day in a month is set the color of the Control Program", notedColor, colorDate);

            schedulingManagerPage.CalendarEditorPanel.HoverCalendarDate(previousDate);
            tooltipIcon = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateTitleIcon();
            tooltipTitleText = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateTitleText();
            tooltipControlProgramName = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateControlProgramName();
            VerifyEqual("15. Verify Clock Icon displayed", true, tooltipIcon.Contains("clock.png"));
            VerifyTrue("15. Verify the text: 'Each year the [full month] [date]'", expectedTitle == tooltipTitleText || expectedTitleDayWith0 == tooltipTitleText, expectedTitle + "/" + expectedTitleDayWith0, tooltipTitleText);
            VerifyEqual("15. Verify Color and Name of the Control Program", controlProgramName, tooltipControlProgramName);

            try
            {
                DeleteCalendar(calendarName);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("SM_18_03 Calendar - Calendar editor - Control Program pop-up - Set a control program monthly for a day in a month.")]
        public void SM_18_03()
        {
            var calendarName = SLVHelper.GenerateUniqueName("CSM1803");
            CreateNewCalendar(calendarName);

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - There is an existing calendar");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.SchedulingManager);

            Step("1. Go to Scheduling Manager app");
            Step("2. Expected Scheduling Manager is routed and loaded successfully");
            var schedulingManagerPage = desktopPage.GoToApp(App.SchedulingManager) as SchedulingManagerPage;

            Step("3. Select 'Calendar' tab in left panel");
            schedulingManagerPage.SchedulingManagerPanel.SelectTab("Calendar");
            schedulingManagerPage.SchedulingManagerPanel.SelectCalendar(calendarName);
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("4. Press a day in a month");
            var randomDate = schedulingManagerPage.CalendarEditorPanel.ClickRandomCalendarDate();
            var date = DateTime.Parse(randomDate);
            var day = date.Day;
            schedulingManagerPage.WaitForPopupDialogDisplayed();

            Step("5. Verify The Control Program pop-up displays with");
            Step(" o Title: Control programs");
            Step(" o All Fields Search textbox and Search Icon");
            Step(" o 4 options: None, Weekly, Monthly, Yearly");
            Step(" o Yearly is selected as default");
            Step(" o List of existing Control Programs with 3 columns: Color, Control Programs Name, Geozone");
            VerifyEqual("5. Verify Title: Control programs", "Control programs", schedulingManagerPage.CalendarControlProgramsPopupPanel.GetPanelTitleText().SplitAndGetAt(1));
            VerifyEqual("5. Verify All Fields Search textbox displays", true, schedulingManagerPage.CalendarControlProgramsPopupPanel.IsSearchInputVisible());
            VerifyEqual("5. Verify 4 options: None, Weekly, Monthly, Yearly", new List<string> { "None", "Weekly", "Monthly", "Yearly" }, schedulingManagerPage.CalendarControlProgramsPopupPanel.GetListOfOptionsName());
            VerifyEqual("5. Verify Yearly is selected as default", "Yearly", schedulingManagerPage.CalendarControlProgramsPopupPanel.GetSelectedOptionName());
            VerifyEqual("5. Verify List of existing Control Programs with 3 columns: Color, Control Programs Name, Geozone", 3, schedulingManagerPage.CalendarControlProgramsPopupPanel.GetListOfColumnsCount());

            Step("6. Select the option Monthly");
            schedulingManagerPage.CalendarControlProgramsPopupPanel.ClickRadioMonthlyButton();

            Step("7. Verify Monthly option is selected");
            VerifyEqual("7. Verify Monthly option is selected", "Monthly", schedulingManagerPage.CalendarControlProgramsPopupPanel.GetSelectedOptionName());

            Step("8. Select a Control Program and press Save button");
            var controlProgramName = schedulingManagerPage.CalendarControlProgramsPopupPanel.GetListOfItemsName().PickRandom();
            schedulingManagerPage.CalendarControlProgramsPopupPanel.SelectItem(controlProgramName);
            var notedColor = schedulingManagerPage.CalendarControlProgramsPopupPanel.GetSelectedItemColor();
            schedulingManagerPage.CalendarControlProgramsPopupPanel.ClickSaveButton();
            schedulingManagerPage.WaitForPopupDialogDisappeared();

            Step("9. Verify The selected day of all months is set the color of the Control Program.");
            Step("10. Hover that day of all months");
            Step("11. Verify the tooltip displays with 2 rows");
            Step(" o Clock Icon and the text: 'Each month the [date]'");
            Step(" o Color and Name of the Control Program");
            var expectedTitle = string.Format("Each month the {0}", day);
            var expectedTitleDayWith0 = string.Format("Each month the {0}", day.ToString("D2"));
            for (int i = 1; i <= 12; i++)
            {
                var newDate = string.Format(@"{0}/{1}/{2}", i, date.Day, date.Year);
                Step("-> Verify " + newDate);
                var color = schedulingManagerPage.CalendarEditorPanel.GetCalendarDateColor(newDate);
                VerifyEqual(string.Format("9. Verify The '{0}' is set the color of the Control Program", newDate), notedColor, color);

                schedulingManagerPage.CalendarEditorPanel.HoverCalendarDate(newDate);
                var tooltipIcon = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateTitleIcon();
                var tooltipTitleText = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateTitleText();
                var tooltipControlProgramName = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateControlProgramName();                
                VerifyEqual("11. Verify Clock Icon displayed", true, tooltipIcon.Contains("clock.png"));
                VerifyTrue("11. Verify the text: 'Each month the [date]'", expectedTitle == tooltipTitleText || expectedTitleDayWith0 == tooltipTitleText, expectedTitle + "/" + expectedTitleDayWith0, tooltipTitleText);
                VerifyEqual("11. Verify Color and Name of the Control Program", controlProgramName, tooltipControlProgramName);
            }

            Step("12. Press Next button to move to the next year");
            schedulingManagerPage.CalendarEditorPanel.ClickYearAfterButton();

            Step("13. Verify The selected day of all months in the next year is also set the color of the Control Program");
            Step("14. Hover that day of all months");
            Step("15. Verify the tooltip displays with 2 rows");
            Step(" o Clock Icon and the text: 'Each month the [date]'");
            Step(" o Color and Name of the Control Program");
            for (int i = 1; i <= 12; i++)
            {
                var newDate = string.Format(@"{0}/{1}/{2}", i, date.Day, schedulingManagerPage.CalendarEditorPanel.GetCurrentYearText());
                Step("-> Verify " + newDate);
                var color = schedulingManagerPage.CalendarEditorPanel.GetCalendarDateColor(newDate);
                VerifyEqual(string.Format("11. Verify The '{0}' is set the color of the Control Program", newDate), notedColor, color);

                schedulingManagerPage.CalendarEditorPanel.HoverCalendarDate(newDate);
                var tooltipIcon = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateTitleIcon();
                var tooltipTitleText = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateTitleText();
                var tooltipControlProgramName = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateControlProgramName();
                VerifyEqual(string.Format("[SC-1074] 15. Verify the tooltip of '{0}' is displayed", newDate), true, schedulingManagerPage.CalendarEditorPanel.IsTooltipDateDisplayed());
                VerifyEqual("15. Verify Clock Icon displayed", true, tooltipIcon.Contains("clock.png"));
                VerifyTrue("15. Verify the text: 'Each month the [date]'", expectedTitle == tooltipTitleText || expectedTitleDayWith0 == tooltipTitleText, expectedTitle + "/" + expectedTitleDayWith0, tooltipTitleText);
                VerifyEqual("15. Verify Color and Name of the Control Program", controlProgramName, tooltipControlProgramName);
            }

            Step("16. Press Back button twice to move to the previous year");
            schedulingManagerPage.CalendarEditorPanel.ClickYearBeforeButton();
            schedulingManagerPage.CalendarEditorPanel.ClickYearBeforeButton();

            Step("17. Verify tooltip and color the same with next year");
            for (int i = 1; i <= 12; i++)
            {
                var newDate = string.Format(@"{0}/{1}/{2}", i, date.Day, schedulingManagerPage.CalendarEditorPanel.GetCurrentYearText());
                var color = schedulingManagerPage.CalendarEditorPanel.GetCalendarDateColor(newDate);
                VerifyEqual(string.Format("17. Verify The '{0}' is set the color of the Control Program", newDate), notedColor, color);

                schedulingManagerPage.CalendarEditorPanel.HoverCalendarDate(newDate);
                var tooltipIcon = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateTitleIcon();
                var tooltipTitleText = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateTitleText();
                var tooltipControlProgramName = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateControlProgramName();                
                VerifyEqual("17. Verify Clock Icon displayed", true, tooltipIcon.Contains("clock.png"));
                VerifyTrue("17. Verify the text: 'Each month the [date]'", expectedTitle == tooltipTitleText || expectedTitleDayWith0 == tooltipTitleText, expectedTitle + "/" + expectedTitleDayWith0, tooltipTitleText);
                VerifyEqual("17. Verify Color and Name of the Control Program", controlProgramName, tooltipControlProgramName);
            }

            try
            {
                DeleteCalendar(calendarName);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("SM_18_04 Calendar - Calendar editor - Control Program pop-up - Set a control program weekly for a day in a month.")]
        public void SM_18_04()
        {
            var calendarName = SLVHelper.GenerateUniqueName("CSM1804");
            CreateNewCalendar(calendarName);

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - There is an existing calendar");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.SchedulingManager);

            Step("1. Go to Scheduling Manager app");
            Step("2. Expected Scheduling Manager is routed and loaded successfully");
            var schedulingManagerPage = desktopPage.GoToApp(App.SchedulingManager) as SchedulingManagerPage;

            Step("3. Select 'Calendar' tab in left panel");
            schedulingManagerPage.SchedulingManagerPanel.SelectTab("Calendar");
            schedulingManagerPage.SchedulingManagerPanel.SelectCalendar(calendarName);
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("4. Press a day in a month");
            var randomDate = schedulingManagerPage.CalendarEditorPanel.ClickRandomCalendarDate();
            var date = DateTime.Parse(randomDate);
            var dayofWeek = date.DayOfWeek;
            var listDatesInYear = Settings.GetAllDaysOfWeek(date.Year, dayofWeek);
            schedulingManagerPage.WaitForPopupDialogDisplayed();

            Step("5. Verify The Control Program pop-up displays with");
            Step(" o Title: Control programs");
            Step(" o All Fields Search textbox and Search Icon");
            Step(" o 4 options: None, Weekly, Monthly, Yearly");
            Step(" o Yearly is selected as default");
            Step(" o List of existing Control Programs with 3 columns: Color, Control Programs Name, Geozone");
            VerifyEqual("5. Verify Title: Control programs", "Control programs", schedulingManagerPage.CalendarControlProgramsPopupPanel.GetPanelTitleText().SplitAndGetAt(1));
            VerifyEqual("5. Verify All Fields Search textbox displays", true, schedulingManagerPage.CalendarControlProgramsPopupPanel.IsSearchInputVisible());
            VerifyEqual("5. Verify 4 options: None, Weekly, Monthly, Yearly", new List<string> { "None", "Weekly", "Monthly", "Yearly" }, schedulingManagerPage.CalendarControlProgramsPopupPanel.GetListOfOptionsName());
            VerifyEqual("5. Verify Yearly is selected as default", "Yearly", schedulingManagerPage.CalendarControlProgramsPopupPanel.GetSelectedOptionName());
            VerifyEqual("5. Verify List of existing Control Programs with 3 columns: Color, Control Programs Name, Geozone", 3, schedulingManagerPage.CalendarControlProgramsPopupPanel.GetListOfColumnsCount());

            Step("6. Select the option Weekly");
            schedulingManagerPage.CalendarControlProgramsPopupPanel.ClickRadioWeeklyButton();

            Step("7. Verify Weekly option is selected");
            VerifyEqual("7. Verify Monthly option is selected", "Weekly", schedulingManagerPage.CalendarControlProgramsPopupPanel.GetSelectedOptionName());

            Step("8. Select a Control Program and press Save button");
            var controlProgramName = schedulingManagerPage.CalendarControlProgramsPopupPanel.GetListOfItemsName().PickRandom();
            schedulingManagerPage.CalendarControlProgramsPopupPanel.SelectItem(controlProgramName);
            var notedColor = schedulingManagerPage.CalendarControlProgramsPopupPanel.GetSelectedItemColor();
            schedulingManagerPage.CalendarControlProgramsPopupPanel.ClickSaveButton();
            schedulingManagerPage.WaitForPopupDialogDisappeared();

            Step("9. Verify The selected days of every week are set the color of the Control Program.");
            Step("10. Hover that day of all months");
            Step("11. Verify the tooltip displays with 2 rows");
            Step(" o Clock Icon and the text: 'Each [name of day]'");
            Step(" o Color and Name of the Control Program");
            var expectedTitle = string.Format("Each {0}", dayofWeek.ToString());
            foreach (var dt in listDatesInYear)
            {
                var newDate = string.Format(@"{0}/{1}/{2}", dt.Month, dt.Day, dt.Year);
                Step("-> Verify " + newDate);
                var color = schedulingManagerPage.CalendarEditorPanel.GetCalendarDateColor(newDate);
                VerifyEqual(string.Format("9. Verify The '{0}' is set the color of the Control Program", newDate), notedColor, color);

                schedulingManagerPage.CalendarEditorPanel.HoverCalendarDate(newDate);
                var tooltipIcon = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateTitleIcon();
                var tooltipTitleText = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateTitleText();
                var tooltipControlProgramName = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateControlProgramName();
                VerifyEqual("11. Verify Clock Icon displayed", true, tooltipIcon.Contains("clock.png"));
                VerifyEqual("11. Verify the text: 'Each [name of day]'", expectedTitle, tooltipTitleText);
                VerifyEqual("11. Verify Color and Name of the Control Program", controlProgramName, tooltipControlProgramName);
            }

            Step("12. Press Next button to move to the next year");
            schedulingManagerPage.CalendarEditorPanel.ClickYearAfterButton();

            Step("13. Verify The selected days of every week are set the color of the Control Program");
            Step("14. Hover that day of all months");
            Step("15. Verify the tooltip displays with 2 rows");
            Step(" o Clock Icon and the text: 'Each month the [date]'");
            Step(" o Color and Name of the Control Program");
            listDatesInYear = Settings.GetAllDaysOfWeek(int.Parse(schedulingManagerPage.CalendarEditorPanel.GetCurrentYearText()), dayofWeek);
            foreach (var dt in listDatesInYear)
            {
                var newDate = string.Format(@"{0}/{1}/{2}", dt.Month, dt.Day, dt.Year);
                Step("-> Verify " + newDate);
                var color = schedulingManagerPage.CalendarEditorPanel.GetCalendarDateColor(newDate);
                VerifyEqual(string.Format("11. Verify The '{0}' is set the color of the Control Program", newDate), notedColor, color);

                schedulingManagerPage.CalendarEditorPanel.HoverCalendarDate(newDate);
                var tooltipIcon = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateTitleIcon();
                var tooltipTitleText = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateTitleText();
                var tooltipControlProgramName = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateControlProgramName();
                VerifyEqual("15. Verify Clock Icon displayed", true, tooltipIcon.Contains("clock.png"));
                VerifyEqual("15. Verify the text: 'Each [name of day]'", expectedTitle, tooltipTitleText);
                VerifyEqual("15. Verify Color and Name of the Control Program", controlProgramName, tooltipControlProgramName);
            }

            Step("16. Press Back button twice to move to the previous year");
            schedulingManagerPage.CalendarEditorPanel.ClickYearBeforeButton();
            schedulingManagerPage.CalendarEditorPanel.ClickYearBeforeButton();

            Step("17. Verify tooltip and color the same with next year");
            listDatesInYear = Settings.GetAllDaysOfWeek(int.Parse(schedulingManagerPage.CalendarEditorPanel.GetCurrentYearText()), dayofWeek);
            foreach (var dt in listDatesInYear)
            {
                var newDate = string.Format(@"{0}/{1}/{2}", dt.Month, dt.Day, dt.Year);
                var color = schedulingManagerPage.CalendarEditorPanel.GetCalendarDateColor(newDate);
                VerifyEqual(string.Format("17. Verify The '{0}' is set the color of the Control Program", newDate), notedColor, color);

                schedulingManagerPage.CalendarEditorPanel.HoverCalendarDate(newDate);
                var tooltipIcon = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateTitleIcon();
                var tooltipTitleText = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateTitleText();
                var tooltipControlProgramName = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateControlProgramName();
                VerifyEqual("17. Verify Clock Icon displayed", true, tooltipIcon.Contains("clock.png"));
                VerifyEqual("17. Verify the text: 'Each [name of day]'", expectedTitle, tooltipTitleText);
                VerifyEqual("17. Verify Color and Name of the Control Program", controlProgramName, tooltipControlProgramName);
            }

            try
            {
                DeleteCalendar(calendarName);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("SM_18_05 Calendar - Calendar editor - Control Program pop-up - Set a control program for specific day in a month.")]
        public void SM_18_05()
        {
            var calendarName = SLVHelper.GenerateUniqueName("CSM1805");
            CreateNewCalendar(calendarName);

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - There is an existing calendar");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.SchedulingManager);

            Step("1. Go to Scheduling Manager app");
            Step("2. Expected Scheduling Manager is routed and loaded successfully");
            var schedulingManagerPage = desktopPage.GoToApp(App.SchedulingManager) as SchedulingManagerPage;

            Step("3. Select 'Calendar' tab in left panel");
            schedulingManagerPage.SchedulingManagerPanel.SelectTab("Calendar");
            schedulingManagerPage.SchedulingManagerPanel.SelectCalendar(calendarName);
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("4. Press a day in a month");
            var randomDate = schedulingManagerPage.CalendarEditorPanel.ClickRandomCalendarDate();
            var date = DateTime.Parse(randomDate);
            var dayofWeek = date.DayOfWeek;
            schedulingManagerPage.WaitForPopupDialogDisplayed();

            Step("5. Verify The Control Program pop-up displays with");
            Step(" o Title: Control programs");
            Step(" o All Fields Search textbox and Search Icon");
            Step(" o 4 options: None, Weekly, Monthly, Yearly");
            Step(" o Yearly is selected as default");
            Step(" o List of existing Control Programs with 3 columns: Color, Control Programs Name, Geozone");
            VerifyEqual("5. Verify Title: Control programs", "Control programs", schedulingManagerPage.CalendarControlProgramsPopupPanel.GetPanelTitleText().SplitAndGetAt(1));
            VerifyEqual("5. Verify All Fields Search textbox displays", true, schedulingManagerPage.CalendarControlProgramsPopupPanel.IsSearchInputVisible());
            VerifyEqual("5. Verify 4 options: None, Weekly, Monthly, Yearly", new List<string> { "None", "Weekly", "Monthly", "Yearly" }, schedulingManagerPage.CalendarControlProgramsPopupPanel.GetListOfOptionsName());
            VerifyEqual("5. Verify Yearly is selected as default", "Yearly", schedulingManagerPage.CalendarControlProgramsPopupPanel.GetSelectedOptionName());
            VerifyEqual("5. Verify List of existing Control Programs with 3 columns: Color, Control Programs Name, Geozone", 3, schedulingManagerPage.CalendarControlProgramsPopupPanel.GetListOfColumnsCount());

            Step("6. Select the option None");
            schedulingManagerPage.CalendarControlProgramsPopupPanel.ClickRadioNoneButton();

            Step("7. Verify None option is selected");
            VerifyEqual("7. Verify None option is selected", "None", schedulingManagerPage.CalendarControlProgramsPopupPanel.GetSelectedOptionName());

            Step("8. Select a Control Program and press Save button");
            var controlProgramName = schedulingManagerPage.CalendarControlProgramsPopupPanel.GetListOfItemsName().PickRandom();
            schedulingManagerPage.CalendarControlProgramsPopupPanel.SelectItem(controlProgramName);
            var notedColor = schedulingManagerPage.CalendarControlProgramsPopupPanel.GetSelectedItemColor();
            schedulingManagerPage.CalendarControlProgramsPopupPanel.ClickSaveButton();
            schedulingManagerPage.WaitForPopupDialogDisappeared();

            Step("9. Verify The selected day is set the color of the Control Program.");
            var colorDate = schedulingManagerPage.CalendarEditorPanel.GetCalendarDateColor(randomDate);
            VerifyEqual("9. Verify The selected day is set the color of the Control Program", notedColor, colorDate);

            Step("10. Hover the day");
            schedulingManagerPage.CalendarEditorPanel.HoverCalendarDate(randomDate);

            Step("11. Verify the tooltip displays with 2 rows");
            Step(" o Clock Icon and the text: 'The [name of day], [name of month] [date], [year]'. ex: The Friday, January 06, 2017'");
            Step(" o Color and Name of the Control Program");
            var expectedTitle = string.Format("The {0}, {1}", dayofWeek.ToString(), date.ToString("MMMM dd, yyyy"));
            var tooltipIcon = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateTitleIcon();
            var tooltipTitleText = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateTitleText();
            var tooltipControlProgramName = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateControlProgramName();
            VerifyEqual("11. Verify Clock Icon displayed", true, tooltipIcon.Contains("clock.png"));
            VerifyEqual("11. Verify the text: 'The [name of day], [name of month] [date], [year]'. ex: The Friday, January 06, 2017'", expectedTitle, tooltipTitleText);
            VerifyEqual("11. Verify Color and Name of the Control Program", controlProgramName, tooltipControlProgramName);

            Step("12. Press Next button to move to the next year");
            schedulingManagerPage.CalendarEditorPanel.ClickYearAfterButton();

            Step("13. Verify The same day of the month in the next year is NOT set the color of the Control Program");
            var nextDate = string.Format(@"{0}/{1}/{2}", date.Month, date.Day, schedulingManagerPage.CalendarEditorPanel.GetCurrentYearText());
            colorDate = schedulingManagerPage.CalendarEditorPanel.GetCalendarDateColor(nextDate);
            VerifyTrue("11. Verify The same day of the month in the next year is NOT set the color of the Control Program", notedColor != colorDate, notedColor, colorDate);

            Step("14. Hover the day");
            schedulingManagerPage.CalendarEditorPanel.HoverCalendarDate(nextDate);

            Step("15. Verify There is no tooltip displayed");
            VerifyEqual("15. Verify There is no tooltip displayed", true, !schedulingManagerPage.CalendarEditorPanel.IsTooltipDateDisplayed());

            Step("16. Press Back button twice to move to the previous year");
            schedulingManagerPage.CalendarEditorPanel.ClickYearBeforeButton();
            schedulingManagerPage.CalendarEditorPanel.ClickYearBeforeButton();

            Step("17. Verify tooltip and color are not set and displayed the same with next year");
            var previousDate = string.Format(@"{0}/{1}/{2}", date.Month, date.Day, schedulingManagerPage.CalendarEditorPanel.GetCurrentYearText());
            colorDate = schedulingManagerPage.CalendarEditorPanel.GetCalendarDateColor(previousDate);
            VerifyTrue("17. Verify The same day of the month in the next year is NOT set the color of the Control Program", notedColor != colorDate, notedColor, colorDate);

            schedulingManagerPage.CalendarEditorPanel.HoverCalendarDate(previousDate);
            VerifyEqual("17. Verify There is no tooltip displayed", true, !schedulingManagerPage.CalendarEditorPanel.IsTooltipDateDisplayed());
            
            try
            {
                DeleteCalendar(calendarName);
            }
            catch { }
        }

        [Test, DynamicRetry]        
        [Description("SM_18_06 Calendar - Calendar editor - Control Program pop-up - Set a control program yearly for multiple days in a month.")]
        [NonParallelizable]
        [Category("RunAlone")]
        public void SM_18_06()
        {
            var calendarName = SLVHelper.GenerateUniqueName("CSM1806");
            CreateNewCalendar(calendarName);

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - There is an existing calendar");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.SchedulingManager);

            Step("1. Go to Scheduling Manager app");
            Step("2. Expected Scheduling Manager is routed and loaded successfully");
            var schedulingManagerPage = desktopPage.GoToApp(App.SchedulingManager) as SchedulingManagerPage;

            Step("3. Select 'Calendar' tab in left panel");
            schedulingManagerPage.SchedulingManagerPanel.SelectTab("Calendar");
            schedulingManagerPage.SchedulingManagerPanel.SelectCalendar(calendarName);
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("4. Drag a mouse to select a whole week on a month");
            var weekDates = schedulingManagerPage.CalendarEditorPanel.DragAndDropRandomDaysInWeek(int.Parse(schedulingManagerPage.CalendarEditorPanel.GetCurrentYearText()));          
            schedulingManagerPage.WaitForPopupDialogDisplayed();
            var startDay = weekDates.First();
            var endDay = weekDates.Last();
            var expectedTitle = string.Format("Each year from {0} {1} to {2} {3}", startDay.ToString("MMMM"), startDay.Day.ToString("D2"), endDay.ToString("MMMM"), endDay.Day.ToString("D2"));

            Step("5. Verify The Control Program pop-up displays with");
            Step(" o Title: Control programs");
            Step(" o All Fields Search textbox and Search Icon");
            Step(" o 4 options: None, Weekly, Monthly, Yearly");
            Step(" o Yearly is selected as default");
            Step(" o List of existing Control Programs with 3 columns: Color, Control Programs Name, Geozone");
            VerifyEqual("5. Verify Title: Control programs", "Control programs", schedulingManagerPage.CalendarControlProgramsPopupPanel.GetPanelTitleText().SplitAndGetAt(1));
            VerifyEqual("5. Verify All Fields Search textbox displays", true, schedulingManagerPage.CalendarControlProgramsPopupPanel.IsSearchInputVisible());
            VerifyEqual("5. Verify 4 options: None, Weekly, Monthly, Yearly", new List<string> { "None", "Weekly", "Monthly", "Yearly" }, schedulingManagerPage.CalendarControlProgramsPopupPanel.GetListOfOptionsName());
            VerifyEqual("5. Verify Yearly is selected as default", "Yearly", schedulingManagerPage.CalendarControlProgramsPopupPanel.GetSelectedOptionName());
            VerifyEqual("5. Verify List of existing Control Programs with 3 columns: Color, Control Programs Name, Geozone", 3, schedulingManagerPage.CalendarControlProgramsPopupPanel.GetListOfColumnsCount());
            
            Step("6. Select a Control Program and press Save button");
            var controlProgramName = schedulingManagerPage.CalendarControlProgramsPopupPanel.GetListOfItemsName().PickRandom();
            schedulingManagerPage.CalendarControlProgramsPopupPanel.SelectItem(controlProgramName);
            var notedColor = schedulingManagerPage.CalendarControlProgramsPopupPanel.GetSelectedItemColor();
            schedulingManagerPage.CalendarControlProgramsPopupPanel.ClickSaveButton();
            schedulingManagerPage.WaitForPopupDialogDisappeared();

            Step("7. Verify The selected days are set the color of the Control Program.");
            foreach (var dt in weekDates)
            {
                var date = dt.ToString("M/d/yyyy");
                var color = schedulingManagerPage.CalendarEditorPanel.GetCalendarDateColor(date);
                VerifyEqual(string.Format("7. Verify The '{0}' is set the color of the Control Program", date), notedColor, color);
            }          

            Step("8. Hover one of those days randomly");
            var randomDate = weekDates.PickRandom();
            schedulingManagerPage.CalendarEditorPanel.HoverCalendarDate(randomDate.ToString("M/d/yyyy"));

            Step("9. Verify the tooltip displays with 2 rows");
            Step(" o Clock Icon and the text: 'Each year from [full month] [start date] to [full month] [end date]'. Ex: Each year from January 09 to January 15'");
            Step(" o Color and Name of the Control Program");
            var tooltipIcon = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateTitleIcon();
            var tooltipTitleText = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateTitleText();
            var tooltipControlProgramName = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateControlProgramName();
            VerifyEqual("9. Verify Clock Icon displayed", true, tooltipIcon.Contains("clock.png"));
            VerifyEqual("9. Verify the text: 'Each year from [full month] [start date] to [full month] [end date]'. Ex: Each year from January 09 to January 15'", expectedTitle, tooltipTitleText);
            VerifyEqual("9. Verify Color and Name of the Control Program", controlProgramName, tooltipControlProgramName);

            Step("10. Press Next button to move to the next year");
            schedulingManagerPage.CalendarEditorPanel.ClickYearAfterButton();

            Step("11. Verify The days from start date to end date in the same month of the next year are also set the color of the Control Program");
            foreach (var dt in weekDates)
            {
                var date = string.Format(@"{0}/{1}/{2}", dt.Month, dt.Day, schedulingManagerPage.CalendarEditorPanel.GetCurrentYearText());
                var color = schedulingManagerPage.CalendarEditorPanel.GetCalendarDateColor(date);
                VerifyEqual(string.Format("11. Verify The '{0}' is set the color of the Control Program", date), notedColor, color);
            }

            Step("12. Hover one of those days randomly");
            randomDate = weekDates.PickRandom();
            var nextDate = string.Format(@"{0}/{1}/{2}", randomDate.Month, randomDate.Day, schedulingManagerPage.CalendarEditorPanel.GetCurrentYearText());
            schedulingManagerPage.CalendarEditorPanel.HoverCalendarDate(nextDate);

            Step("13. Verify the tooltip displays with 2 rows");
            Step(" o Clock Icon and the text: 'Each year from [full month] [start date] to [full month] [end date]'. Ex: Each year from January 09 to January 15'");
            Step(" o Color and Name of the Control Program");
            tooltipIcon = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateTitleIcon();
            tooltipTitleText = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateTitleText();
            tooltipControlProgramName = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateControlProgramName();
            VerifyEqual("13. Verify Clock Icon displayed", true, tooltipIcon.Contains("clock.png"));
            VerifyEqual("13. Verify the text: 'Each year from [full month] [start date] to [full month] [end date]'. Ex: Each year from January 09 to January 15'", expectedTitle, tooltipTitleText);
            VerifyEqual("13. Verify Color and Name of the Control Program", controlProgramName, tooltipControlProgramName);

            Step("14. Press Back button twice to move to the previous year");
            schedulingManagerPage.CalendarEditorPanel.ClickYearBeforeButton();
            schedulingManagerPage.CalendarEditorPanel.ClickYearBeforeButton();

            Step("15. Verify tooltip and color the same with next year");
            foreach (var dt in weekDates)
            {
                var date = string.Format(@"{0}/{1}/{2}", dt.Month, dt.Day, schedulingManagerPage.CalendarEditorPanel.GetCurrentYearText());
                var color = schedulingManagerPage.CalendarEditorPanel.GetCalendarDateColor(date);
                VerifyEqual(string.Format("15. Verify The '{0}' is set the color of the Control Program", date), notedColor, color);
            }
            randomDate = weekDates.PickRandom();
            var previousDate = string.Format(@"{0}/{1}/{2}", randomDate.Month, randomDate.Day, schedulingManagerPage.CalendarEditorPanel.GetCurrentYearText());
            schedulingManagerPage.CalendarEditorPanel.HoverCalendarDate(previousDate);

            tooltipIcon = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateTitleIcon();
            tooltipTitleText = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateTitleText();
            tooltipControlProgramName = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateControlProgramName();
            VerifyEqual("15. Verify Clock Icon displayed", true, tooltipIcon.Contains("clock.png"));
            VerifyEqual("15. Verify the text: 'Each year from [full month] [start date] to [full month] [end date]'. Ex: Each year from January 09 to January 15'", expectedTitle, tooltipTitleText);
            VerifyEqual("15. Verify Color and Name of the Control Program", controlProgramName, tooltipControlProgramName);

            Step("16. Press Save button on Calendar editor to save changes");
            schedulingManagerPage.CalendarEditorPanel.ClickSaveButton();
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("17. Select another calendar, then select the testing calendar again");
            schedulingManagerPage.SchedulingManagerPanel.SelectRandomCalendar();
            schedulingManagerPage.WaitForPreviousActionComplete();
            schedulingManagerPage.SchedulingManagerPanel.SelectCalendar(calendarName);
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("18. Verify All changes for the current year, the next year and the previous year are saved correctly (Run all the verify points again)");
            foreach (var dt in weekDates)
            {
                var date = dt.ToString("M/d/yyyy");
                var color = schedulingManagerPage.CalendarEditorPanel.GetCalendarDateColor(date);
                VerifyEqual(string.Format("[#1285538] 18-7. Verify The '{0}' is set the color of the Control Program", date), notedColor, color);
            }
            randomDate = weekDates.PickRandom();
            schedulingManagerPage.CalendarEditorPanel.HoverCalendarDate(randomDate.ToString("M/d/yyyy"));
            if (!schedulingManagerPage.CalendarEditorPanel.IsTooltipDateDisplayed())
            {
                Warning("#1285538 Scheduling Manager - Changing calendars clears last changes");
                return;
            }

            tooltipIcon = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateTitleIcon();
            tooltipTitleText = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateTitleText();
            tooltipControlProgramName = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateControlProgramName();
            VerifyEqual("18-9. Verify Clock Icon displayed", true, tooltipIcon.Contains("clock.png"));
            VerifyEqual("18-9. Verify the text: 'Each year from [full month] [start date] to [full month] [end date]'. Ex: Each year from January 09 to January 15'", expectedTitle, tooltipTitleText);
            VerifyEqual("18-9. Verify Color and Name of the Control Program", controlProgramName, tooltipControlProgramName);

            schedulingManagerPage.CalendarEditorPanel.ClickYearAfterButton();
            foreach (var dt in weekDates)
            {
                var date = string.Format(@"{0}/{1}/{2}", dt.Month, dt.Day, schedulingManagerPage.CalendarEditorPanel.GetCurrentYearText());
                var color = schedulingManagerPage.CalendarEditorPanel.GetCalendarDateColor(date);
                VerifyEqual(string.Format("18-11. Verify The '{0}' is set the color of the Control Program", date), notedColor, color);
            }
            randomDate = weekDates.PickRandom();
            nextDate = string.Format(@"{0}/{1}/{2}", randomDate.Month, randomDate.Day, schedulingManagerPage.CalendarEditorPanel.GetCurrentYearText());
            schedulingManagerPage.CalendarEditorPanel.HoverCalendarDate(nextDate);
            tooltipIcon = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateTitleIcon();
            tooltipTitleText = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateTitleText();
            tooltipControlProgramName = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateControlProgramName();
            VerifyEqual("18-13. Verify Clock Icon displayed", true, tooltipIcon.Contains("clock.png"));
            VerifyEqual("18-13. Verify the text: 'Each year from [full month] [start date] to [full month] [end date]'. Ex: Each year from January 09 to January 15'", expectedTitle, tooltipTitleText);
            VerifyEqual("18-13. Verify Color and Name of the Control Program", controlProgramName, tooltipControlProgramName);

            schedulingManagerPage.CalendarEditorPanel.ClickYearBeforeButton();
            schedulingManagerPage.CalendarEditorPanel.ClickYearBeforeButton();
            foreach (var dt in weekDates)
            {
                var date = string.Format(@"{0}/{1}/{2}", dt.Month, dt.Day, schedulingManagerPage.CalendarEditorPanel.GetCurrentYearText());
                var color = schedulingManagerPage.CalendarEditorPanel.GetCalendarDateColor(date);
                VerifyEqual(string.Format("18-15. Verify The '{0}' is set the color of the Control Program", date), notedColor, color);
            }
            randomDate = weekDates.PickRandom();
            previousDate = string.Format(@"{0}/{1}/{2}", randomDate.Month, randomDate.Day, schedulingManagerPage.CalendarEditorPanel.GetCurrentYearText());
            schedulingManagerPage.CalendarEditorPanel.HoverCalendarDate(previousDate);
            tooltipIcon = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateTitleIcon();
            tooltipTitleText = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateTitleText();
            tooltipControlProgramName = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateControlProgramName();
            VerifyEqual("18-15. Verify Clock Icon displayed", true, tooltipIcon.Contains("clock.png"));
            VerifyEqual("18-15. Verify the text: 'Each year from [full month] [start date] to [full month] [end date]'. Ex: Each year from January 09 to January 15'", expectedTitle, tooltipTitleText);
            VerifyEqual("18-15. Verify Color and Name of the Control Program", controlProgramName, tooltipControlProgramName);

            try
            {
                DeleteCalendar(calendarName);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("SM_18_07 Calendar - Calendar editor - Control Program pop-up - Set a control program monthly for multiple days in a month.")]
        [NonParallelizable]
        [Category("RunAlone")]
        public void SM_18_07()
        {
            var months = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
            var calendarName = SLVHelper.GenerateUniqueName("CSM1807");
            CreateNewCalendar(calendarName);

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - There is an existing calendar");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.SchedulingManager);

            Step("1. Go to Scheduling Manager app");
            Step("2. Expected Scheduling Manager is routed and loaded successfully");
            var schedulingManagerPage = desktopPage.GoToApp(App.SchedulingManager) as SchedulingManagerPage;

            Step("3. Select 'Calendar' tab in left panel");
            schedulingManagerPage.SchedulingManagerPanel.SelectTab("Calendar");
            schedulingManagerPage.SchedulingManagerPanel.SelectCalendar(calendarName);
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("4. Drag a mouse to select a whole week on a month");
            var weekDates = schedulingManagerPage.CalendarEditorPanel.DragAndDropRandomDaysInWeek(int.Parse(schedulingManagerPage.CalendarEditorPanel.GetCurrentYearText()));
            schedulingManagerPage.WaitForPopupDialogDisplayed();
            var startDay = weekDates.First();
            var endDay = weekDates.Last();
            var expectedTitle = string.Format("Each month from {0} to {1}", startDay.Day, endDay.Day);

            Step("5. Verify The Control Program pop-up displays with");
            Step(" o Title: Control programs");
            Step(" o All Fields Search textbox and Search Icon");
            Step(" o 4 options: None, Weekly, Monthly, Yearly");
            Step(" o Yearly is selected as default");
            Step(" o List of existing Control Programs with 3 columns: Color, Control Programs Name, Geozone");
            VerifyEqual("5. Verify Title: Control programs", "Control programs", schedulingManagerPage.CalendarControlProgramsPopupPanel.GetPanelTitleText().SplitAndGetAt(1));
            VerifyEqual("5. Verify All Fields Search textbox displays", true, schedulingManagerPage.CalendarControlProgramsPopupPanel.IsSearchInputVisible());
            VerifyEqual("5. Verify 4 options: None, Weekly, Monthly, Yearly", new List<string> { "None", "Weekly", "Monthly", "Yearly" }, schedulingManagerPage.CalendarControlProgramsPopupPanel.GetListOfOptionsName());
            VerifyEqual("5. Verify Yearly is selected as default", "Yearly", schedulingManagerPage.CalendarControlProgramsPopupPanel.GetSelectedOptionName());
            VerifyEqual("5. Verify List of existing Control Programs with 3 columns: Color, Control Programs Name, Geozone", 3, schedulingManagerPage.CalendarControlProgramsPopupPanel.GetListOfColumnsCount());

            Step("6. Select the option Monthly");
            schedulingManagerPage.CalendarControlProgramsPopupPanel.ClickRadioMonthlyButton();

            Step("7. Verify Monthly option is selected");
            VerifyEqual("7. Verify Monthly option is selected", "Monthly", schedulingManagerPage.CalendarControlProgramsPopupPanel.GetSelectedOptionName());

            Step("8. Select a Control Program and press Save button");
            var controlProgramName = schedulingManagerPage.CalendarControlProgramsPopupPanel.GetListOfItemsName().PickRandom();
            schedulingManagerPage.CalendarControlProgramsPopupPanel.SelectItem(controlProgramName);
            var notedColor = schedulingManagerPage.CalendarControlProgramsPopupPanel.GetSelectedItemColor();
            schedulingManagerPage.CalendarControlProgramsPopupPanel.ClickSaveButton();
            schedulingManagerPage.WaitForPopupDialogDisappeared();

            Step("9. The selected days from start date to end date in 12 months are set the color of the Control Program.");
            for (int i = 1; i <= months.Count; i++)
            {
                foreach (var dt in weekDates)
                {
                    if (i == 2 && dt.Day > 28) continue;
                    var date = string.Format(@"{0}/{1}/{2}", i, dt.Day, dt.Year);
                    Step("-> Verify " + date);
                    var color = schedulingManagerPage.CalendarEditorPanel.GetCalendarDateColor(date);
                    VerifyEqual(string.Format("9. Verify The '{0}' is set the color of the Control Program", date), notedColor, color);
                }
            }

            Step("10. Hover one of those days in a random month");
            var randomDateWeek = weekDates.PickRandom();
            var randomMonth = months.PickRandom();
            var randomDate = string.Format(@"{0}/{1}/{2}", randomMonth, randomDateWeek.Day, randomDateWeek.Year);
            schedulingManagerPage.CalendarEditorPanel.HoverCalendarDate(randomDate);

            Step("11. Verify the tooltip displays with 2 rows");
            Step(" o Clock Icon and the text: 'Each month from [start date] to [end date]'. Ex: Each month from 16 to 22'");
            Step(" o Color and Name of the Control Program");
            var tooltipIcon = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateTitleIcon();
            var tooltipTitleText = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateTitleText();
            var tooltipControlProgramName = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateControlProgramName();
            VerifyEqual("11. Verify Clock Icon displayed", true, tooltipIcon.Contains("clock.png"));
            VerifyEqual("11. Verify the text: 'Each month from [start date] to [end date]'. Ex: Each month from 16 to 22'", expectedTitle, tooltipTitleText);
            VerifyEqual("11. Verify Color and Name of the Control Program", controlProgramName, tooltipControlProgramName);

            Step("12. Press Next button to move to the next year");
            schedulingManagerPage.CalendarEditorPanel.ClickYearAfterButton();

            Step("13. Verify The selected days from start date to end date in 12 months of the next year are also set the color of the Control Program");
            for (int i = 1; i <= months.Count; i++)
            {
                foreach (var dt in weekDates)
                {
                    if (i == 2 && dt.Day > 28) continue;
                    var date = string.Format(@"{0}/{1}/{2}", i, dt.Day, schedulingManagerPage.CalendarEditorPanel.GetCurrentYearText());
                    Step("-> Verify " + date);
                    var color = schedulingManagerPage.CalendarEditorPanel.GetCalendarDateColor(date);
                    VerifyEqual(string.Format("13. Verify The '{0}' is set the color of the Control Program", date), notedColor, color);
                }
            }

            Step("14. Hover one of those days in a random month");
            randomDateWeek = weekDates.PickRandom();
            randomMonth = months.PickRandom();           
            var nextDate = string.Format(@"{0}/{1}/{2}", randomMonth, randomDateWeek.Day, schedulingManagerPage.CalendarEditorPanel.GetCurrentYearText());
            schedulingManagerPage.CalendarEditorPanel.HoverCalendarDate(nextDate);

            Step("15. Verify the tooltip displays with 2 rows");
            Step(" o Clock Icon and the text: 'Each month from [start date] to [end date]'. Ex: Each month from 16 to 22'");
            Step(" o Color and Name of the Control Program");
            tooltipIcon = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateTitleIcon();
            tooltipTitleText = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateTitleText();
            tooltipControlProgramName = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateControlProgramName();
            VerifyEqual("15. Verify Clock Icon displayed", true, tooltipIcon.Contains("clock.png"));
            VerifyEqual("15. Verify the text: 'Each month from [start date] to [end date]'. Ex: Each month from 16 to 22'", expectedTitle, tooltipTitleText);
            VerifyEqual("15. Verify Color and Name of the Control Program", controlProgramName, tooltipControlProgramName);

            Step("16. Press Back button twice to move to the previous year");
            schedulingManagerPage.CalendarEditorPanel.ClickYearBeforeButton();
            schedulingManagerPage.CalendarEditorPanel.ClickYearBeforeButton();

            Step("17. Verify tooltip and color the same with next year");
            for (int i = 1; i <= months.Count; i++)
            {
                foreach (var dt in weekDates)
                {
                    if (i == 2 && dt.Day > 28) continue;
                    var date = string.Format(@"{0}/{1}/{2}", i, dt.Day, schedulingManagerPage.CalendarEditorPanel.GetCurrentYearText());
                    Step("-> Verify " + date);
                    var color = schedulingManagerPage.CalendarEditorPanel.GetCalendarDateColor(date);
                    VerifyEqual(string.Format("17. Verify The '{0}' is set the color of the Control Program", date), notedColor, color);
                }
            }
            randomDateWeek = weekDates.PickRandom();
            randomMonth = months.PickRandom();
            var previousDate = string.Format(@"{0}/{1}/{2}", randomMonth, randomDateWeek.Day, schedulingManagerPage.CalendarEditorPanel.GetCurrentYearText());
            schedulingManagerPage.CalendarEditorPanel.HoverCalendarDate(previousDate);

            tooltipIcon = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateTitleIcon();
            tooltipTitleText = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateTitleText();
            tooltipControlProgramName = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateControlProgramName();
            VerifyEqual("17. Verify Clock Icon displayed", true, tooltipIcon.Contains("clock.png"));
            VerifyEqual("17. Verify the text: 'Each month from [start date] to [end date]'. Ex: Each month from 16 to 22'", expectedTitle, tooltipTitleText);
            VerifyEqual("17. Verify Color and Name of the Control Program", controlProgramName, tooltipControlProgramName);

            Step("18. Press Save button on Calendar editor to save changes");
            schedulingManagerPage.CalendarEditorPanel.ClickSaveButton();
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("19. Select another calendar, then select the testing calendar again");
            schedulingManagerPage.SchedulingManagerPanel.SelectRandomCalendar();
            schedulingManagerPage.WaitForPreviousActionComplete();
            schedulingManagerPage.SchedulingManagerPanel.SelectCalendar(calendarName);
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("20. Verify All changes for the current year, the next year and the previous year are saved correctly (Run all the verify points again)");
            for (int i = 1; i <= months.Count; i++)
            {
                foreach (var dt in weekDates)
                {
                    if (i == 2 && dt.Day > 28) continue;
                    var date = string.Format(@"{0}/{1}/{2}", i, dt.Day, dt.Year);
                    Step("-> Verify " + date);
                    var color = schedulingManagerPage.CalendarEditorPanel.GetCalendarDateColor(date);
                    VerifyEqual(string.Format("[#1285538] 20-9. Verify The '{0}' is set the color of the Control Program", date), notedColor, color);
                }
            }
            randomDateWeek = weekDates.PickRandom();
            randomMonth = months.PickRandom();
            randomDate = string.Format(@"{0}/{1}/{2}", randomMonth, randomDateWeek.Day, randomDateWeek.Year);
            schedulingManagerPage.CalendarEditorPanel.HoverCalendarDate(randomDate);
            if (!schedulingManagerPage.CalendarEditorPanel.IsTooltipDateDisplayed())
            {
                Warning("#1285538 Scheduling Manager - Changing calendars clears last changes");
                return;
            }

            tooltipIcon = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateTitleIcon();
            tooltipTitleText = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateTitleText();
            tooltipControlProgramName = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateControlProgramName();
            VerifyEqual("20-11. Verify Clock Icon displayed", true, tooltipIcon.Contains("clock.png"));
            VerifyEqual("20-11. Verify the text: 'Each month from [start date] to [end date]'. Ex: Each month from 16 to 22'", expectedTitle, tooltipTitleText);
            VerifyEqual("20-11. Verify Color and Name of the Control Program", controlProgramName, tooltipControlProgramName);

            schedulingManagerPage.CalendarEditorPanel.ClickYearAfterButton();
            for (int i = 1; i <= months.Count; i++)
            {
                foreach (var dt in weekDates)
                {
                    if (i == 2 && dt.Day > 28) continue;
                    var date = string.Format(@"{0}/{1}/{2}", i, dt.Day, schedulingManagerPage.CalendarEditorPanel.GetCurrentYearText());
                    Step("-> Verify " + date);
                    var color = schedulingManagerPage.CalendarEditorPanel.GetCalendarDateColor(date);
                    VerifyEqual(string.Format("20-13. Verify The '{0}' is set the color of the Control Program", date), notedColor, color);
                }
            }
            randomDateWeek = weekDates.PickRandom();
            randomMonth = months.PickRandom();
            nextDate = string.Format(@"{0}/{1}/{2}", randomMonth, randomDateWeek.Day, schedulingManagerPage.CalendarEditorPanel.GetCurrentYearText());
            schedulingManagerPage.CalendarEditorPanel.HoverCalendarDate(nextDate);
            tooltipIcon = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateTitleIcon();
            tooltipTitleText = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateTitleText();
            tooltipControlProgramName = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateControlProgramName();
            VerifyEqual("20-15. Verify Clock Icon displayed", true, tooltipIcon.Contains("clock.png"));
            VerifyEqual("20-15. Verify the text: 'Each month from [start date] to [end date]'. Ex: Each month from 16 to 22'", expectedTitle, tooltipTitleText);
            VerifyEqual("20-15. Verify Color and Name of the Control Program", controlProgramName, tooltipControlProgramName);

            schedulingManagerPage.CalendarEditorPanel.ClickYearBeforeButton();
            schedulingManagerPage.CalendarEditorPanel.ClickYearBeforeButton();
            for (int i = 1; i <= months.Count; i++)
            {
                foreach (var dt in weekDates)
                {
                    if (i == 2 && dt.Day > 28) continue;
                    var date = string.Format(@"{0}/{1}/{2}", i, dt.Day, schedulingManagerPage.CalendarEditorPanel.GetCurrentYearText());
                    Step("-> Verify " + date);
                    var color = schedulingManagerPage.CalendarEditorPanel.GetCalendarDateColor(date);
                    VerifyEqual(string.Format("20-17. Verify The '{0}' is set the color of the Control Program", date), notedColor, color);
                }
            }
            randomDateWeek = weekDates.PickRandom();
            randomMonth = months.PickRandom();
            previousDate = string.Format(@"{0}/{1}/{2}", randomMonth, randomDateWeek.Day, schedulingManagerPage.CalendarEditorPanel.GetCurrentYearText());
            schedulingManagerPage.CalendarEditorPanel.HoverCalendarDate(previousDate);
            tooltipIcon = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateTitleIcon();
            tooltipTitleText = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateTitleText();
            tooltipControlProgramName = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateControlProgramName();
            VerifyEqual("20-17. Verify Clock Icon displayed", true, tooltipIcon.Contains("clock.png"));
            VerifyEqual("20-17. Verify the text: 'Each month from [start date] to [end date]'. Ex: Each month from 16 to 22'", expectedTitle, tooltipTitleText);
            VerifyEqual("20-17. Verify Color and Name of the Control Program", controlProgramName, tooltipControlProgramName);

            try
            {
                DeleteCalendar(calendarName);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("SM_18_08 Calendar - Calendar editor - Control Program pop-up - Set a control program weekly for multiple days in a month.")]
        [NonParallelizable]
        [Category("RunAlone")]
        public void SM_18_08()
        {
            var calendarName = SLVHelper.GenerateUniqueName("CSM1808");
            CreateNewCalendar(calendarName);

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - There is an existing calendar");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.SchedulingManager);

            Step("1. Go to Scheduling Manager app");
            Step("2. Expected Scheduling Manager is routed and loaded successfully");
            var schedulingManagerPage = desktopPage.GoToApp(App.SchedulingManager) as SchedulingManagerPage;

            Step("3. Select 'Calendar' tab in left panel");
            schedulingManagerPage.SchedulingManagerPanel.SelectTab("Calendar");
            schedulingManagerPage.SchedulingManagerPanel.SelectCalendar(calendarName);
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("4. Drag a mouse to select 3 days from Monday to Wednesday in a week of a month");
            var currentYear = int.Parse(schedulingManagerPage.CalendarEditorPanel.GetCurrentYearText());
            var weekDates = schedulingManagerPage.CalendarEditorPanel.DragAndDropRandomDaysInWeek(currentYear, 3);
            schedulingManagerPage.WaitForPopupDialogDisplayed();
            var listDatesInYear = new List<DateTime>();
            foreach (var dt in weekDates)
            {
                listDatesInYear.AddRange(Settings.GetAllDaysOfWeek(dt));
            }
            listDatesInYear.Sort();

            Step("5. Verify The Control Program pop-up displays with");
            Step(" o Title: Control programs");
            Step(" o All Fields Search textbox and Search Icon");
            Step(" o 4 options: None, Weekly, Monthly, Yearly");
            Step(" o Yearly is selected as default");
            Step(" o List of existing Control Programs with 3 columns: Color, Control Programs Name, Geozone");
            VerifyEqual("5. Verify Title: Control programs", "Control programs", schedulingManagerPage.CalendarControlProgramsPopupPanel.GetPanelTitleText().SplitAndGetAt(1));
            VerifyEqual("5. Verify All Fields Search textbox displays", true, schedulingManagerPage.CalendarControlProgramsPopupPanel.IsSearchInputVisible());
            VerifyEqual("5. Verify 4 options: None, Weekly, Monthly, Yearly", new List<string> { "None", "Weekly", "Monthly", "Yearly" }, schedulingManagerPage.CalendarControlProgramsPopupPanel.GetListOfOptionsName());
            VerifyEqual("5. Verify Yearly is selected as default", "Yearly", schedulingManagerPage.CalendarControlProgramsPopupPanel.GetSelectedOptionName());
            VerifyEqual("5. Verify List of existing Control Programs with 3 columns: Color, Control Programs Name, Geozone", 3, schedulingManagerPage.CalendarControlProgramsPopupPanel.GetListOfColumnsCount());

            Step("6. Select the option Weekly");
            schedulingManagerPage.CalendarControlProgramsPopupPanel.ClickRadioWeeklyButton();

            Step("7. Verify Weekly option is selected");
            VerifyEqual("7. Verify Monthly option is selected", "Weekly", schedulingManagerPage.CalendarControlProgramsPopupPanel.GetSelectedOptionName());

            Step("8. Select a Control Program and press Save button");
            var controlProgramName = schedulingManagerPage.CalendarControlProgramsPopupPanel.GetListOfItemsName().PickRandom();
            schedulingManagerPage.CalendarControlProgramsPopupPanel.SelectItem(controlProgramName);
            var notedColor = schedulingManagerPage.CalendarControlProgramsPopupPanel.GetSelectedItemColor();
            schedulingManagerPage.CalendarControlProgramsPopupPanel.ClickSaveButton();
            schedulingManagerPage.WaitForPopupDialogDisappeared();

            Step("9. Verify The selected days from Monday to Wednesday in 12 months are set the color of the Control Program.");         
            foreach (var dt in listDatesInYear)
            {                
                var newDate = string.Format(@"{0}/{1}/{2}", dt.Month, dt.Day, dt.Year);
                Step("-> Verify " + newDate);
                var color = schedulingManagerPage.CalendarEditorPanel.GetCalendarDateColor(newDate);
                VerifyEqual(string.Format("9. Verify The '{0}' is set the color of the Control Program", newDate), notedColor, color);              
            }

            Step("10. Hover one of Monday days in a random month");
            var listDates = Settings.GetAllDaysOfWeek(currentYear, DayOfWeek.Monday);
            schedulingManagerPage.CalendarEditorPanel.HoverCalendarDate(listDates.PickRandom().ToString("M/d/yyyy"));
            
            Step("11. Verify the tooltip displays with 2 rows");
            Step(" o Clock Icon and the text: 'Each Monday'.");
            Step(" o Color and Name of the Control Program");
            var tooltipIcon = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateTitleIcon();
            var tooltipTitleText = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateTitleText();
            var tooltipControlProgramName = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateControlProgramName();
            VerifyEqual("11. Verify Clock Icon displayed", true, tooltipIcon.Contains("clock.png"));
            VerifyEqual("11. Verify the text: 'Each Monday'", "Each Monday", tooltipTitleText);
            VerifyEqual("11. Verify Color and Name of the Control Program", controlProgramName, tooltipControlProgramName);

            Step("12. Hover one of Tuesday days in a random month");
            listDates = Settings.GetAllDaysOfWeek(currentYear, DayOfWeek.Tuesday);
            schedulingManagerPage.CalendarEditorPanel.HoverCalendarDate(listDates.PickRandom().ToString("M/d/yyyy"));
            
            Step("13. Verify the tooltip displays with 2 rows");
            Step(" o Clock Icon and the text: 'Each Monday'.");
            Step(" o Color and Name of the Control Program");
            tooltipIcon = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateTitleIcon();
            tooltipTitleText = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateTitleText();
            tooltipControlProgramName = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateControlProgramName();
            VerifyEqual("13. Verify Clock Icon displayed", true, tooltipIcon.Contains("clock.png"));
            VerifyEqual("13. Verify the text: 'Each Tuesday'", "Each Tuesday", tooltipTitleText);
            VerifyEqual("13. Verify Color and Name of the Control Program", controlProgramName, tooltipControlProgramName);

            Step("14. Hover one of Tuesday days in a random month");
            listDates = Settings.GetAllDaysOfWeek(currentYear, DayOfWeek.Wednesday);
            schedulingManagerPage.CalendarEditorPanel.HoverCalendarDate(listDates.PickRandom().ToString("M/d/yyyy"));

            Step("15. Verify the tooltip displays with 2 rows");
            Step(" o Clock Icon and the text: 'Each Monday'.");
            Step(" o Color and Name of the Control Program");
            tooltipIcon = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateTitleIcon();
            tooltipTitleText = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateTitleText();
            tooltipControlProgramName = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateControlProgramName();
            VerifyEqual("15. Verify Clock Icon displayed", true, tooltipIcon.Contains("clock.png"));
            VerifyEqual("15. Verify the text: 'Each Wednesday'", "Each Wednesday", tooltipTitleText);
            VerifyEqual("15. Verify Color and Name of the Control Program", controlProgramName, tooltipControlProgramName);

            Step("16. Press Next button to move to the next year");            
            schedulingManagerPage.CalendarEditorPanel.ClickYearAfterButton();
            currentYear = int.Parse(schedulingManagerPage.CalendarEditorPanel.GetCurrentYearText());
            listDatesInYear.Clear();
            foreach (var dt in weekDates)
            {
                listDatesInYear.AddRange(Settings.GetAllDaysOfWeek(currentYear, dt.DayOfWeek));
            }
            listDatesInYear.Sort();

            Step("17. Verify The selected days from Monday to Wednesday in 12 months of the next year are set the color of the Control Program.");     
            foreach (var dt in listDatesInYear)
            {
                var newDate = string.Format(@"{0}/{1}/{2}", dt.Month, dt.Day, dt.Year);
                Step("-> Verify " + newDate);
                var color = schedulingManagerPage.CalendarEditorPanel.GetCalendarDateColor(newDate);
                VerifyEqual(string.Format("17. Verify The '{0}' is set the color of the Control Program", newDate), notedColor, color);
            }

            Step("18. Hover one of Monday days in a random month");
            listDates = Settings.GetAllDaysOfWeek(currentYear, DayOfWeek.Monday);
            schedulingManagerPage.CalendarEditorPanel.HoverCalendarDate(listDates.PickRandom().ToString("M/d/yyyy"));

            Step("19. Verify the tooltip displays with 2 rows");
            Step(" o Clock Icon and the text: 'Each Monday'.");
            Step(" o Color and Name of the Control Program");
            tooltipIcon = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateTitleIcon();
            tooltipTitleText = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateTitleText();
            tooltipControlProgramName = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateControlProgramName();
            VerifyEqual("19. Verify Clock Icon displayed", true, tooltipIcon.Contains("clock.png"));
            VerifyEqual("19. Verify the text: 'Each Monday'", "Each Monday", tooltipTitleText);
            VerifyEqual("19. Verify Color and Name of the Control Program", controlProgramName, tooltipControlProgramName);

            Step("20. Hover one of Tuesday days in a random month");
            listDates = Settings.GetAllDaysOfWeek(currentYear, DayOfWeek.Tuesday);
            schedulingManagerPage.CalendarEditorPanel.HoverCalendarDate(listDates.PickRandom().ToString("M/d/yyyy"));

            Step("21. Verify the tooltip displays with 2 rows");
            Step(" o Clock Icon and the text: 'Each Monday'.");
            Step(" o Color and Name of the Control Program");
            tooltipIcon = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateTitleIcon();
            tooltipTitleText = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateTitleText();
            tooltipControlProgramName = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateControlProgramName();
            VerifyEqual("21. Verify Clock Icon displayed", true, tooltipIcon.Contains("clock.png"));
            VerifyEqual("21. Verify the text: 'Each Tuesday'", "Each Tuesday", tooltipTitleText);
            VerifyEqual("21. Verify Color and Name of the Control Program", controlProgramName, tooltipControlProgramName);

            Step("22. Hover one of Tuesday days in a random month");
            listDates = Settings.GetAllDaysOfWeek(currentYear, DayOfWeek.Wednesday);
            schedulingManagerPage.CalendarEditorPanel.HoverCalendarDate(listDates.PickRandom().ToString("M/d/yyyy"));

            Step("23. Verify the tooltip displays with 2 rows");
            Step(" o Clock Icon and the text: 'Each Monday'.");
            Step(" o Color and Name of the Control Program");
            tooltipIcon = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateTitleIcon();
            tooltipTitleText = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateTitleText();
            tooltipControlProgramName = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateControlProgramName();
            VerifyEqual("23. Verify Clock Icon displayed", true, tooltipIcon.Contains("clock.png"));
            VerifyEqual("23. Verify the text: 'Each Wednesday'", "Each Wednesday", tooltipTitleText);
            VerifyEqual("23. Verify Color and Name of the Control Program", controlProgramName, tooltipControlProgramName);
            
            Step("24. Press Back button twice to move to the previous year");
            schedulingManagerPage.CalendarEditorPanel.ClickYearBeforeButton();
            schedulingManagerPage.CalendarEditorPanel.ClickYearBeforeButton();
            currentYear = int.Parse(schedulingManagerPage.CalendarEditorPanel.GetCurrentYearText());
            listDatesInYear.Clear();
            foreach (var dt in weekDates)
            {
                listDatesInYear.AddRange(Settings.GetAllDaysOfWeek(currentYear, dt.DayOfWeek));
            }
            listDatesInYear.Sort();

            Step("25. Verify Verify tooltip and color the same with next year");
            foreach (var dt in listDatesInYear)
            {
                var newDate = string.Format(@"{0}/{1}/{2}", dt.Month, dt.Day, dt.Year);
                Step("-> Verify " + newDate);
                var color = schedulingManagerPage.CalendarEditorPanel.GetCalendarDateColor(newDate);
                VerifyEqual(string.Format("25. Verify The '{0}' is set the color of the Control Program", newDate), notedColor, color);
            }

            Step("-> Hover one of Monday days in a random month");
            listDates = Settings.GetAllDaysOfWeek(currentYear, DayOfWeek.Monday);
            schedulingManagerPage.CalendarEditorPanel.HoverCalendarDate(listDates.PickRandom().ToString("M/d/yyyy"));            
            tooltipIcon = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateTitleIcon();
            tooltipTitleText = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateTitleText();
            tooltipControlProgramName = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateControlProgramName();
            VerifyEqual("25. Verify Clock Icon displayed", true, tooltipIcon.Contains("clock.png"));
            VerifyEqual("25. Verify the text: 'Each Monday'", "Each Monday", tooltipTitleText);
            VerifyEqual("25. Verify Color and Name of the Control Program", controlProgramName, tooltipControlProgramName);

            Step("-> Hover one of Tuesday days in a random month");
            listDates = Settings.GetAllDaysOfWeek(currentYear, DayOfWeek.Tuesday);
            schedulingManagerPage.CalendarEditorPanel.HoverCalendarDate(listDates.PickRandom().ToString("M/d/yyyy"));
            tooltipIcon = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateTitleIcon();
            tooltipTitleText = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateTitleText();
            tooltipControlProgramName = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateControlProgramName();
            VerifyEqual("25. Verify Clock Icon displayed", true, tooltipIcon.Contains("clock.png"));
            VerifyEqual("25. Verify the text: 'Each Tuesday'", "Each Tuesday", tooltipTitleText);
            VerifyEqual("25. Verify Color and Name of the Control Program", controlProgramName, tooltipControlProgramName);

            Step("-> Hover one of Tuesday days in a random month");
            listDates = Settings.GetAllDaysOfWeek(currentYear, DayOfWeek.Wednesday);
            schedulingManagerPage.CalendarEditorPanel.HoverCalendarDate(listDates.PickRandom().ToString("M/d/yyyy"));
            tooltipIcon = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateTitleIcon();
            tooltipTitleText = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateTitleText();
            tooltipControlProgramName = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateControlProgramName();
            VerifyEqual("25. Verify Clock Icon displayed", true, tooltipIcon.Contains("clock.png"));
            VerifyEqual("25. Verify the text: 'Each Wednesday'", "Each Wednesday", tooltipTitleText);
            VerifyEqual("25. Verify Color and Name of the Control Program", controlProgramName, tooltipControlProgramName);
            
            Step("26. Press Save button on Calendar editor to save changes");
            schedulingManagerPage.CalendarEditorPanel.ClickSaveButton();
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("27. Select another calendar, then select the testing calendar again");
            schedulingManagerPage.SchedulingManagerPanel.SelectRandomCalendar();
            schedulingManagerPage.WaitForPreviousActionComplete();
            schedulingManagerPage.SchedulingManagerPanel.SelectCalendar(calendarName);
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("28. Verify All changes for the current year, the next year and the previous year are saved correctly (Run all the verify points again)");
            currentYear = int.Parse(schedulingManagerPage.CalendarEditorPanel.GetCurrentYearText());
            listDatesInYear.Clear();
            foreach (var dt in weekDates)
            {
                listDatesInYear.AddRange(Settings.GetAllDaysOfWeek(currentYear, dt.DayOfWeek));
            }
            listDatesInYear.Sort();
            foreach (var dt in listDatesInYear)
            {
                var newDate = string.Format(@"{0}/{1}/{2}", dt.Month, dt.Day, dt.Year);
                Step("-> Verify " + newDate);
                var color = schedulingManagerPage.CalendarEditorPanel.GetCalendarDateColor(newDate);
                VerifyEqual(string.Format("[#1285538] 28-9. Verify The '{0}' is set the color of the Control Program", newDate), notedColor, color);
            }

            Step("-> Hover one of Monday days in a random month");
            listDates = Settings.GetAllDaysOfWeek(currentYear, DayOfWeek.Monday);
            schedulingManagerPage.CalendarEditorPanel.HoverCalendarDate(listDates.PickRandom().ToString("M/d/yyyy"));
            if (!schedulingManagerPage.CalendarEditorPanel.IsTooltipDateDisplayed())
            {
                Warning("#1285538 Scheduling Manager - Changing calendars clears last changes");
                return;
            }

            tooltipIcon = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateTitleIcon();
            tooltipTitleText = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateTitleText();
            tooltipControlProgramName = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateControlProgramName();
            VerifyEqual("28-11. Verify Clock Icon displayed", true, tooltipIcon.Contains("clock.png"));
            VerifyEqual("28-11. Verify the text: 'Each Monday'", "Each Monday", tooltipTitleText);
            VerifyEqual("28-11. Verify Color and Name of the Control Program", controlProgramName, tooltipControlProgramName);

            Step("-> Hover one of Tuesday days in a random month");
            listDates = Settings.GetAllDaysOfWeek(currentYear, DayOfWeek.Tuesday);
            schedulingManagerPage.CalendarEditorPanel.HoverCalendarDate(listDates.PickRandom().ToString("M/d/yyyy"));
            tooltipIcon = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateTitleIcon();
            tooltipTitleText = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateTitleText();
            tooltipControlProgramName = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateControlProgramName();
            VerifyEqual("28-13. Verify Clock Icon displayed", true, tooltipIcon.Contains("clock.png"));
            VerifyEqual("28-13. Verify the text: 'Each Tuesday'", "Each Tuesday", tooltipTitleText);
            VerifyEqual("28-13. Verify Color and Name of the Control Program", controlProgramName, tooltipControlProgramName);

            Step("-> Hover one of Tuesday days in a random month");
            listDates = Settings.GetAllDaysOfWeek(currentYear, DayOfWeek.Wednesday);
            schedulingManagerPage.CalendarEditorPanel.HoverCalendarDate(listDates.PickRandom().ToString("M/d/yyyy"));
            tooltipIcon = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateTitleIcon();
            tooltipTitleText = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateTitleText();
            tooltipControlProgramName = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateControlProgramName();
            VerifyEqual("28-15. Verify Clock Icon displayed", true, tooltipIcon.Contains("clock.png"));
            VerifyEqual("28-15. Verify the text: 'Each Wednesday'", "Each Wednesday", tooltipTitleText);
            VerifyEqual("28-15. Verify Color and Name of the Control Program", controlProgramName, tooltipControlProgramName);

            Step("-> Press Next button to move to the next year");
            schedulingManagerPage.CalendarEditorPanel.ClickYearAfterButton();
            currentYear = int.Parse(schedulingManagerPage.CalendarEditorPanel.GetCurrentYearText());
            listDatesInYear.Clear();
            foreach (var dt in weekDates)
            {
                listDatesInYear.AddRange(Settings.GetAllDaysOfWeek(currentYear, dt.DayOfWeek));
            }
            listDatesInYear.Sort();
            
            foreach (var dt in listDatesInYear)
            {
                var newDate = string.Format(@"{0}/{1}/{2}", dt.Month, dt.Day, dt.Year);
                Step("-> Verify " + newDate);
                var color = schedulingManagerPage.CalendarEditorPanel.GetCalendarDateColor(newDate);
                VerifyEqual(string.Format("28-17. Verify The '{0}' is set the color of the Control Program", newDate), notedColor, color);
            }

            Step("-> Hover one of Monday days in a random month");
            listDates = Settings.GetAllDaysOfWeek(currentYear, DayOfWeek.Monday);
            schedulingManagerPage.CalendarEditorPanel.HoverCalendarDate(listDates.PickRandom().ToString("M/d/yyyy"));
            tooltipIcon = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateTitleIcon();
            tooltipTitleText = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateTitleText();
            tooltipControlProgramName = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateControlProgramName();
            VerifyEqual("28-19. Verify Clock Icon displayed", true, tooltipIcon.Contains("clock.png"));
            VerifyEqual("28-19. Verify the text: 'Each Monday'", "Each Monday", tooltipTitleText);
            VerifyEqual("28-19. Verify Color and Name of the Control Program", controlProgramName, tooltipControlProgramName);

            Step("-> Hover one of Tuesday days in a random month");
            listDates = Settings.GetAllDaysOfWeek(currentYear, DayOfWeek.Tuesday);
            schedulingManagerPage.CalendarEditorPanel.HoverCalendarDate(listDates.PickRandom().ToString("M/d/yyyy"));
            tooltipIcon = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateTitleIcon();
            tooltipTitleText = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateTitleText();
            tooltipControlProgramName = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateControlProgramName();
            VerifyEqual("28-21. Verify Clock Icon displayed", true, tooltipIcon.Contains("clock.png"));
            VerifyEqual("28-21. Verify the text: 'Each Tuesday'", "Each Tuesday", tooltipTitleText);
            VerifyEqual("28-21. Verify Color and Name of the Control Program", controlProgramName, tooltipControlProgramName);

            Step("-> Hover one of Tuesday days in a random month");
            listDates = Settings.GetAllDaysOfWeek(currentYear, DayOfWeek.Wednesday);
            schedulingManagerPage.CalendarEditorPanel.HoverCalendarDate(listDates.PickRandom().ToString("M/d/yyyy"));
            tooltipIcon = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateTitleIcon();
            tooltipTitleText = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateTitleText();
            tooltipControlProgramName = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateControlProgramName();
            VerifyEqual("28-23. Verify Clock Icon displayed", true, tooltipIcon.Contains("clock.png"));
            VerifyEqual("28-23. Verify the text: 'Each Wednesday'", "Each Wednesday", tooltipTitleText);
            VerifyEqual("28-23. Verify Color and Name of the Control Program", controlProgramName, tooltipControlProgramName);

            Step("-> Press Back button twice to move to the previous year");
            schedulingManagerPage.CalendarEditorPanel.ClickYearBeforeButton();
            schedulingManagerPage.CalendarEditorPanel.ClickYearBeforeButton();
            currentYear = int.Parse(schedulingManagerPage.CalendarEditorPanel.GetCurrentYearText());
            listDatesInYear.Clear();
            foreach (var dt in weekDates)
            {
                listDatesInYear.AddRange(Settings.GetAllDaysOfWeek(currentYear, dt.DayOfWeek));
            }
            listDatesInYear.Sort();
            
            foreach (var dt in listDatesInYear)
            {
                var newDate = string.Format(@"{0}/{1}/{2}", dt.Month, dt.Day, dt.Year);
                Step("-> Verify " + newDate);
                var color = schedulingManagerPage.CalendarEditorPanel.GetCalendarDateColor(newDate);
                VerifyEqual(string.Format("28-25. Verify The '{0}' is set the color of the Control Program", newDate), notedColor, color);
            }

            Step("-> Hover one of Monday days in a random month");
            listDates = Settings.GetAllDaysOfWeek(currentYear, DayOfWeek.Monday);
            schedulingManagerPage.CalendarEditorPanel.HoverCalendarDate(listDates.PickRandom().ToString("M/d/yyyy"));
            tooltipIcon = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateTitleIcon();
            tooltipTitleText = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateTitleText();
            tooltipControlProgramName = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateControlProgramName();
            VerifyEqual("28-25. Verify Clock Icon displayed", true, tooltipIcon.Contains("clock.png"));
            VerifyEqual("28-25. Verify the text: 'Each Monday'", "Each Monday", tooltipTitleText);
            VerifyEqual("28-25. Verify Color and Name of the Control Program", controlProgramName, tooltipControlProgramName);

            Step("-> Hover one of Tuesday days in a random month");
            listDates = Settings.GetAllDaysOfWeek(currentYear, DayOfWeek.Tuesday);
            schedulingManagerPage.CalendarEditorPanel.HoverCalendarDate(listDates.PickRandom().ToString("M/d/yyyy"));
            tooltipIcon = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateTitleIcon();
            tooltipTitleText = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateTitleText();
            tooltipControlProgramName = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateControlProgramName();
            VerifyEqual("28-25. Verify Clock Icon displayed", true, tooltipIcon.Contains("clock.png"));
            VerifyEqual("28-25. Verify the text: 'Each Tuesday'", "Each Tuesday", tooltipTitleText);
            VerifyEqual("28-25. Verify Color and Name of the Control Program", controlProgramName, tooltipControlProgramName);

            Step("-> Hover one of Tuesday days in a random month");
            listDates = Settings.GetAllDaysOfWeek(currentYear, DayOfWeek.Wednesday);
            schedulingManagerPage.CalendarEditorPanel.HoverCalendarDate(listDates.PickRandom().ToString("M/d/yyyy"));
            tooltipIcon = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateTitleIcon();
            tooltipTitleText = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateTitleText();
            tooltipControlProgramName = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateControlProgramName();
            VerifyEqual("28-25. Verify Clock Icon displayed", true, tooltipIcon.Contains("clock.png"));
            VerifyEqual("28-25. Verify the text: 'Each Wednesday'", "Each Wednesday", tooltipTitleText);
            VerifyEqual("28-25. Verify Color and Name of the Control Program", controlProgramName, tooltipControlProgramName);

            try
            {
                DeleteCalendar(calendarName);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("SM_18_09 Calendar - Calendar editor - Control Program pop-up - Set a control program for multiple specific days in a month.")]
        [NonParallelizable]
        [Category("RunAlone")]
        public void SM_18_09()
        {
            var calendarName = SLVHelper.GenerateUniqueName("CSM1809");
            CreateNewCalendar(calendarName);

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - There is an existing calendar");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.SchedulingManager);

            Step("1. Go to Scheduling Manager app");
            Step("2. Expected Scheduling Manager is routed and loaded successfully");
            var schedulingManagerPage = desktopPage.GoToApp(App.SchedulingManager) as SchedulingManagerPage;

            Step("3. Select 'Calendar' tab in left panel");
            schedulingManagerPage.SchedulingManagerPanel.SelectTab("Calendar");
            schedulingManagerPage.SchedulingManagerPanel.SelectCalendar(calendarName);
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("4. Drag a mouse to select a whole week on a month");
            var currentYear = int.Parse(schedulingManagerPage.CalendarEditorPanel.GetCurrentYearText());
            var randomMonth = new List<int> { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 }.PickRandom();
            var weekDates = schedulingManagerPage.CalendarEditorPanel.DragAndDropRandomDays(currentYear, randomMonth, DayOfWeek.Saturday, 3);
            schedulingManagerPage.WaitForPopupDialogDisplayed();
            var startDay = weekDates.First();
            var endDay = weekDates.Last();
            var expectedTitle = string.Format("From {0}, {1} to {2}, {3}", startDay.DayOfWeek, startDay.ToString("MMMM dd, yyyy"), endDay.DayOfWeek, endDay.ToString("MMMM dd, yyyy"));

            Step("5. Verify The Control Program pop-up displays with");
            Step(" o Title: Control programs");
            Step(" o All Fields Search textbox and Search Icon");
            Step(" o 3 options: None, Monthly, Yearly");
            Step(" o Yearly is selected as default");
            Step(" o List of existing Control Programs with 3 columns: Color, Control Programs Name, Geozone");
            VerifyEqual("5. Verify Title: Control programs", "Control programs", schedulingManagerPage.CalendarControlProgramsPopupPanel.GetPanelTitleText().SplitAndGetAt(1));
            VerifyEqual("5. Verify All Fields Search textbox displays", true, schedulingManagerPage.CalendarControlProgramsPopupPanel.IsSearchInputVisible());
            VerifyEqual("5. Verify 3 options: None, Monthly, Yearly", new List<string> { "None", "Monthly", "Yearly" }, schedulingManagerPage.CalendarControlProgramsPopupPanel.GetListOfOptionsName());
            VerifyEqual("5. Verify Yearly is selected as default", "Yearly", schedulingManagerPage.CalendarControlProgramsPopupPanel.GetSelectedOptionName());
            VerifyEqual("5. Verify List of existing Control Programs with 3 columns: Color, Control Programs Name, Geozone", 3, schedulingManagerPage.CalendarControlProgramsPopupPanel.GetListOfColumnsCount());

            Step("6. Select the option None");
            schedulingManagerPage.CalendarControlProgramsPopupPanel.ClickRadioNoneButton();

            Step("7. Verify None option is selected");
            VerifyEqual("7. Verify None option is selected", "None", schedulingManagerPage.CalendarControlProgramsPopupPanel.GetSelectedOptionName());

            Step("8. Select a Control Program and press Save button");
            var controlProgramName = schedulingManagerPage.CalendarControlProgramsPopupPanel.GetListOfItemsName().PickRandom();
            schedulingManagerPage.CalendarControlProgramsPopupPanel.SelectItem(controlProgramName);
            var notedColor = schedulingManagerPage.CalendarControlProgramsPopupPanel.GetSelectedItemColor();
            schedulingManagerPage.CalendarControlProgramsPopupPanel.ClickSaveButton();
            schedulingManagerPage.WaitForPopupDialogDisappeared();

            Step("9. Verify The selected days of the random month are set the color of the Control Program.");
            foreach (var dt in weekDates)
            {
                var newDate = string.Format(@"{0}/{1}/{2}", dt.Month, dt.Day, dt.Year);
                Step("-> Verify " + newDate);
                var date = dt.ToString("M/d/yyyy");
                var color = schedulingManagerPage.CalendarEditorPanel.GetCalendarDateColor(date);
                VerifyEqual(string.Format("9. Verify The '{0}' is set the color of the Control Program", date), notedColor, color);
            }

            Step("10. Hover one of those days");
            var randomDate = weekDates.PickRandom();
            schedulingManagerPage.CalendarEditorPanel.HoverCalendarDate(randomDate.ToString("M/d/yyyy"));

            Step("11. Verify the tooltip displays with 2 rows");
            Step(" o Clock Icon and the text: 'From [name of day], [name of month] [start date], [current year] to [name of day], [name of month] [end date]'. Ex: From Saturday, January 14, 2017 to Monday, January 16, 2017");
            Step(" o Color and Name of the Control Program");
            var tooltipIcon = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateTitleIcon();
            var tooltipTitleText = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateTitleText();
            var tooltipControlProgramName = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateControlProgramName();
            VerifyEqual("11. Verify Clock Icon displayed", true, tooltipIcon.Contains("clock.png"));
            VerifyEqual("11. Verify the text: 'From [name of day], [name of month] [start date], [current year] to [name of day], [name of month] [end date]'", expectedTitle, tooltipTitleText);
            VerifyEqual("11. Verify Color and Name of the Control Program", controlProgramName, tooltipControlProgramName);

            Step("12. Hover a same day of those days but in the previous or next month");
            Step("13. Verify The same day in the previous or next month is NOT set the color of the Control Program");

            Step("-> Hover a same day of those days in the previous month");
            foreach (var dt in weekDates)
            {
                var newDate = string.Format(@"{0}/{1}/{2}", randomMonth - 1, dt.Day, dt.Year);
                Step("-> Verify " + newDate);
                var color = schedulingManagerPage.CalendarEditorPanel.GetCalendarDateColor(newDate);
                VerifyTrue(string.Format("13. Verify The '{0}' is NOT set the color of the Control Program", newDate), notedColor != color, notedColor, color);
            }

            Step("-> Hover a same day of those days in the next month");
            foreach (var dt in weekDates)
            {
                var newDate = string.Format(@"{0}/{1}/{2}", randomMonth + 1, dt.Day, dt.Year);
                Step("-> Verify " + newDate);
                var color = schedulingManagerPage.CalendarEditorPanel.GetCalendarDateColor(newDate);
                VerifyTrue(string.Format("13. Verify The '{0}' is NOT set the color of the Control Program", newDate), notedColor != color, notedColor, color);
            }

            Step("14. Press Next button to move to the next year");
            schedulingManagerPage.CalendarEditorPanel.ClickYearAfterButton();
            currentYear = int.Parse(schedulingManagerPage.CalendarEditorPanel.GetCurrentYearText());

            Step("15. Verify The same day of the month in the next year is NOT set the color of the Control Program");
            foreach (var dt in weekDates)
            {
                var newDate = string.Format(@"{0}/{1}/{2}", randomMonth, dt.Day, currentYear);
                Step("-> Verify " + newDate);
                var color = schedulingManagerPage.CalendarEditorPanel.GetCalendarDateColor(newDate);
                VerifyTrue(string.Format("15. Verify The '{0}' is NOT set the color of the Control Program", newDate), notedColor != color, notedColor, color);
            }

            Step("16. Hover the day");
            randomDate = weekDates.PickRandom();
            var nextDate = string.Format(@"{0}/{1}/{2}", randomDate.Month, randomDate.Day, currentYear);
            schedulingManagerPage.CalendarEditorPanel.HoverCalendarDate(nextDate);

            Step("17. Verify There is no tooltip displayed");
            VerifyEqual("17. Verify There is no tooltip displayed", true, !schedulingManagerPage.CalendarEditorPanel.IsTooltipDateDisplayed());
            
            Step("18. Press Back button twice to move to the previous year");
            schedulingManagerPage.CalendarEditorPanel.ClickYearBeforeButton();
            schedulingManagerPage.CalendarEditorPanel.ClickYearBeforeButton();
            currentYear = int.Parse(schedulingManagerPage.CalendarEditorPanel.GetCurrentYearText());

            Step("19. Verify Verify tooltip and color are not set and displayed the same with next year");
            foreach (var dt in weekDates)
            {
                var newDate = string.Format(@"{0}/{1}/{2}", randomMonth, dt.Day, currentYear);
                Step("-> Verify " + newDate);
                var color = schedulingManagerPage.CalendarEditorPanel.GetCalendarDateColor(newDate);
                VerifyTrue(string.Format("19. Verify The '{0}' is NOT set the color of the Control Program", newDate), notedColor != color, notedColor, color);
            }

            Step("20. Press Save button on Calendar editor to save changes");
            schedulingManagerPage.CalendarEditorPanel.ClickSaveButton();
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("21. Select another calendar, then select the testing calendar again");
            schedulingManagerPage.SchedulingManagerPanel.SelectRandomCalendar();
            schedulingManagerPage.WaitForPreviousActionComplete();
            schedulingManagerPage.SchedulingManagerPanel.SelectCalendar(calendarName);
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("22. Verify All changes for the current year, the next year and the previous year are saved correctly (Run all the verify points again)");
            foreach (var dt in weekDates)
            {
                var newDate = string.Format(@"{0}/{1}/{2}", dt.Month, dt.Day, dt.Year);
                Step("-> Verify " + newDate);
                var date = dt.ToString("M/d/yyyy");
                var color = schedulingManagerPage.CalendarEditorPanel.GetCalendarDateColor(date);
                VerifyEqual(string.Format("[#1285538] 22-9. Verify The '{0}' is set the color of the Control Program", date), notedColor, color);
            }
            randomDate = weekDates.PickRandom();
            schedulingManagerPage.CalendarEditorPanel.HoverCalendarDate(randomDate.ToString("M/d/yyyy"));
            if (!schedulingManagerPage.CalendarEditorPanel.IsTooltipDateDisplayed())
            {
                Warning("#1285538 Scheduling Manager - Changing calendars clears last changes");
                return;
            }

            tooltipIcon = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateTitleIcon();
            tooltipTitleText = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateTitleText();
            tooltipControlProgramName = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateControlProgramName();
            VerifyEqual("22-11. Verify Clock Icon displayed", true, tooltipIcon.Contains("clock.png"));
            VerifyEqual("22-11. Verify the text: 'From [name of day], [name of month] [start date], [current year] to [name of day], [name of month] [end date]'", expectedTitle, tooltipTitleText);
            VerifyEqual("22-11. Verify Color and Name of the Control Program", controlProgramName, tooltipControlProgramName);            
            Step("-> Hover a same day of those days in the previous month");
            foreach (var dt in weekDates)
            {
                var newDate = string.Format(@"{0}/{1}/{2}", randomMonth - 1, dt.Day, dt.Year);
                Step("-> Verify " + newDate);
                var color = schedulingManagerPage.CalendarEditorPanel.GetCalendarDateColor(newDate);
                VerifyTrue(string.Format("22-13. Verify The '{0}' is NOT set the color of the Control Program", newDate), notedColor != color, notedColor, color);
            }
            Step("-> Hover a same day of those days in the next month");
            foreach (var dt in weekDates)
            {
                var newDate = string.Format(@"{0}/{1}/{2}", randomMonth + 1, dt.Day, dt.Year);
                Step("-> Verify " + newDate);
                var color = schedulingManagerPage.CalendarEditorPanel.GetCalendarDateColor(newDate);
                VerifyTrue(string.Format("22-13. Verify The '{0}' is NOT set the color of the Control Program", newDate), notedColor != color, notedColor, color);
            }
            
            schedulingManagerPage.CalendarEditorPanel.ClickYearAfterButton();
            currentYear = int.Parse(schedulingManagerPage.CalendarEditorPanel.GetCurrentYearText());
            foreach (var dt in weekDates)
            {
                var newDate = string.Format(@"{0}/{1}/{2}", randomMonth, dt.Day, currentYear);
                Step("-> Verify " + newDate);
                var color = schedulingManagerPage.CalendarEditorPanel.GetCalendarDateColor(newDate);
                VerifyTrue(string.Format("22-15. Verify The '{0}' is NOT set the color of the Control Program", newDate), notedColor != color, notedColor, color);
            }            
            randomDate = weekDates.PickRandom();
            nextDate = string.Format(@"{0}/{1}/{2}", randomDate.Month, randomDate.Day, currentYear);
            schedulingManagerPage.CalendarEditorPanel.HoverCalendarDate(nextDate);
            VerifyEqual("22-17. Verify There is no tooltip displayed", true, !schedulingManagerPage.CalendarEditorPanel.IsTooltipDateDisplayed());
            
            schedulingManagerPage.CalendarEditorPanel.ClickYearBeforeButton();
            schedulingManagerPage.CalendarEditorPanel.ClickYearBeforeButton();
            currentYear = int.Parse(schedulingManagerPage.CalendarEditorPanel.GetCurrentYearText());
            foreach (var dt in weekDates)
            {
                var newDate = string.Format(@"{0}/{1}/{2}", randomMonth, dt.Day, currentYear);
                Step("-> Verify " + newDate);
                var color = schedulingManagerPage.CalendarEditorPanel.GetCalendarDateColor(newDate);
                VerifyTrue(string.Format("22-19. Verify The '{0}' is NOT set the color of the Control Program", newDate), notedColor != color, notedColor, color);
            }

            try
            {
                DeleteCalendar(calendarName);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("SM_18_10 Calendar - Calendar editor - Control Program pop-up - Set a control program yearly for multiple days of back-to-back months.")]
        [NonParallelizable]
        [Category("RunAlone")]
        public void SM_18_10()
        {
            var calendarName = SLVHelper.GenerateUniqueName("CSM1810");
            CreateNewCalendar(calendarName);

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - There is an existing calendar");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.SchedulingManager);

            Step("1. Go to Scheduling Manager app");
            Step("2. Expected Scheduling Manager is routed and loaded successfully");
            var schedulingManagerPage = desktopPage.GoToApp(App.SchedulingManager) as SchedulingManagerPage;

            Step("3. Select 'Calendar' tab in left panel");
            schedulingManagerPage.SchedulingManagerPanel.SelectTab("Calendar");
            schedulingManagerPage.SchedulingManagerPanel.SelectCalendar(calendarName);
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("4. Drag a mouse to select a whole week on a month");
            var currentYear = int.Parse(schedulingManagerPage.CalendarEditorPanel.GetCurrentYearText());
            var randomMonth = new List<int> { 1, 2, 3, 5, 6, 7, 9, 10, 11 }.PickRandom();
            var weekDates = schedulingManagerPage.CalendarEditorPanel.DragAndDropLastDayMonthToFirstDayNextMonth(currentYear, randomMonth);
            schedulingManagerPage.WaitForPopupDialogDisplayed();
            var startDay = weekDates.First();
            var endDay = weekDates.Last();
            var expectedTitle = string.Format("Each year from {0} to {1}", startDay.ToString("MMMM dd"), endDay.ToString("MMMM dd"));

            Step("5. Verify The Control Program pop-up displays with");
            Step(" o Title: Control programs");
            Step(" o All Fields Search textbox and Search Icon");
            Step(" o 2 options: None, Yearly");
            Step(" o Yearly is selected as default");
            Step(" o List of existing Control Programs with 3 columns: Color, Control Programs Name, Geozone");
            VerifyEqual("5. Verify Title: Control programs", "Control programs", schedulingManagerPage.CalendarControlProgramsPopupPanel.GetPanelTitleText().SplitAndGetAt(1));
            VerifyEqual("5. Verify All Fields Search textbox displays", true, schedulingManagerPage.CalendarControlProgramsPopupPanel.IsSearchInputVisible());
            VerifyEqual("5. Verify 2 options: None, Yearly", new List<string> { "None", "Yearly" }, schedulingManagerPage.CalendarControlProgramsPopupPanel.GetListOfOptionsName());
            VerifyEqual("5. Verify Yearly is selected as default", "Yearly", schedulingManagerPage.CalendarControlProgramsPopupPanel.GetSelectedOptionName());
            VerifyEqual("5. Verify List of existing Control Programs with 3 columns: Color, Control Programs Name, Geozone", 3, schedulingManagerPage.CalendarControlProgramsPopupPanel.GetListOfColumnsCount());
            
            Step("6. Select a Control Program and press Save button");           
            var controlProgramName = schedulingManagerPage.CalendarControlProgramsPopupPanel.GetListOfItemsName().PickRandom();
            schedulingManagerPage.CalendarControlProgramsPopupPanel.SelectItem(controlProgramName);
            var notedColor = schedulingManagerPage.CalendarControlProgramsPopupPanel.GetSelectedItemColor();
            schedulingManagerPage.CalendarControlProgramsPopupPanel.ClickSaveButton();
            schedulingManagerPage.WaitForPopupDialogDisappeared();

            Step("7. Verify The selected days of the random month are set the color of the Control Program.");
            foreach (var dt in weekDates)
            {
                var newDate = string.Format(@"{0}/{1}/{2}", dt.Month, dt.Day, dt.Year);
                Step("-> Verify " + newDate);
                var date = dt.ToString("M/d/yyyy");
                var color = schedulingManagerPage.CalendarEditorPanel.GetCalendarDateColor(date);
                VerifyEqual(string.Format("7. Verify The '{0}' is set the color of the Control Program", date), notedColor, color);
            }

            Step("8. Hover a day");
            var randomDate = weekDates.PickRandom();
            schedulingManagerPage.CalendarEditorPanel.HoverCalendarDate(randomDate.ToString("M/d/yyyy"));

            Step("9. Verify the tooltip displays with 2 rows");
            Step(" o Clock Icon and the text: 'Each year from [name of first month] [start date] to [name of 2nd month] [end date]'. Ex: Each year from January 31 to February 01");
            Step(" o Color and Name of the Control Program");
            var tooltipIcon = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateTitleIcon();
            var tooltipTitleText = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateTitleText();
            var tooltipControlProgramName = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateControlProgramName();
            VerifyEqual("9. Verify Clock Icon displayed", true, tooltipIcon.Contains("clock.png"));
            VerifyEqual("9. Verify the text: 'Each year from [name of first month] [start date] to [name of 2nd month] [end date]'. Ex: Each year from January 31 to February 01'", expectedTitle, tooltipTitleText);
            VerifyEqual("9. Verify Color and Name of the Control Program", controlProgramName, tooltipControlProgramName);

            Step("10. Press Next button to move to the next year");
            schedulingManagerPage.CalendarEditorPanel.ClickYearAfterButton();
            currentYear = int.Parse(schedulingManagerPage.CalendarEditorPanel.GetCurrentYearText());

            Step("11. Verify The days in the same months in the next year is also set the color of the Control Program");
            foreach (var dt in weekDates)
            {
                var newDate = string.Format(@"{0}/{1}/{2}", dt.Month, dt.Day, currentYear);
                Step("-> Verify " + newDate);
                var color = schedulingManagerPage.CalendarEditorPanel.GetCalendarDateColor(newDate);
                VerifyEqual(string.Format("11. Verify The '{0}' is set the color of the Control Program", newDate), notedColor, color);
            }

            Step("12. Hover the day");
            randomDate = weekDates.PickRandom();
            var nextDate = string.Format(@"{0}/{1}/{2}", randomDate.Month, randomDate.Day, currentYear);
            schedulingManagerPage.CalendarEditorPanel.HoverCalendarDate(nextDate);

            Step("13. Verify the tooltip displays with 2 rows");
            Step(" o Clock Icon and the text: 'Each year from [name of first month] [start date] to [name of 2nd month] [end date]'. Ex: Each year from January 31 to February 01");
            Step(" o Color and Name of the Control Program");
            tooltipIcon = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateTitleIcon();
            tooltipTitleText = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateTitleText();
            tooltipControlProgramName = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateControlProgramName();
            VerifyEqual("13. Verify Clock Icon displayed", true, tooltipIcon.Contains("clock.png"));
            VerifyEqual("13. Verify the text: 'Each year from [name of first month] [start date] to [name of 2nd month] [end date]'. Ex: Each year from January 31 to February 01'", expectedTitle, tooltipTitleText);
            VerifyEqual("13. Verify Color and Name of the Control Program", controlProgramName, tooltipControlProgramName);

            Step("14. Press Back button twice to move to the previous year");
            schedulingManagerPage.CalendarEditorPanel.ClickYearBeforeButton();
            schedulingManagerPage.CalendarEditorPanel.ClickYearBeforeButton();
            currentYear = int.Parse(schedulingManagerPage.CalendarEditorPanel.GetCurrentYearText());

            Step("15. Verify tooltip and color the same with next year");
            foreach (var dt in weekDates)
            {
                var newDate = string.Format(@"{0}/{1}/{2}", dt.Month, dt.Day, currentYear);
                Step("-> Verify " + newDate);
                var color = schedulingManagerPage.CalendarEditorPanel.GetCalendarDateColor(newDate);
                VerifyEqual(string.Format("15. Verify The '{0}' is set the color of the Control Program", newDate), notedColor, color);
            }

            Step("16. Press Save button on Calendar editor to save changes");
            schedulingManagerPage.CalendarEditorPanel.ClickSaveButton();
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("17. Select another calendar, then select the testing calendar again");
            schedulingManagerPage.SchedulingManagerPanel.SelectRandomCalendar();
            schedulingManagerPage.WaitForPreviousActionComplete();
            schedulingManagerPage.SchedulingManagerPanel.SelectCalendar(calendarName);
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("18. Verify All changes for the current year, the next year and the previous year are saved correctly (Run all the verify points again)");
            randomDate = weekDates.PickRandom();
            schedulingManagerPage.CalendarEditorPanel.HoverCalendarDate(randomDate.ToString("M/d/yyyy"));
            if (!schedulingManagerPage.CalendarEditorPanel.IsTooltipDateDisplayed())
            {
                Warning("#1285538 Scheduling Manager - Changing calendars clears last changes");
                return;
            }

            tooltipIcon = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateTitleIcon();
            tooltipTitleText = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateTitleText();
            tooltipControlProgramName = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateControlProgramName();
            VerifyEqual("18-9. Verify Clock Icon displayed", true, tooltipIcon.Contains("clock.png"));
            VerifyEqual("18-9. Verify the text: 'Each year from [name of first month] [start date] to [name of 2nd month] [end date]'. Ex: Each year from January 31 to February 01'", expectedTitle, tooltipTitleText);
            VerifyEqual("18-9. Verify Color and Name of the Control Program", controlProgramName, tooltipControlProgramName);
            
            schedulingManagerPage.CalendarEditorPanel.ClickYearAfterButton();
            currentYear = int.Parse(schedulingManagerPage.CalendarEditorPanel.GetCurrentYearText());
            foreach (var dt in weekDates)
            {
                var newDate = string.Format(@"{0}/{1}/{2}", dt.Month, dt.Day, currentYear);
                Step("-> Verify " + newDate);
                var color = schedulingManagerPage.CalendarEditorPanel.GetCalendarDateColor(newDate);
                VerifyEqual(string.Format("18-11. Verify The '{0}' is set the color of the Control Program", newDate), notedColor, color);
            }
            randomDate = weekDates.PickRandom();
            nextDate = string.Format(@"{0}/{1}/{2}", randomDate.Month, randomDate.Day, currentYear);
            schedulingManagerPage.CalendarEditorPanel.HoverCalendarDate(nextDate);
            
            tooltipIcon = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateTitleIcon();
            tooltipTitleText = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateTitleText();
            tooltipControlProgramName = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateControlProgramName();
            VerifyEqual("18-13. Verify Clock Icon displayed", true, tooltipIcon.Contains("clock.png"));
            VerifyEqual("18-13. Verify the text: 'Each year from [name of first month] [start date] to [name of 2nd month] [end date]'. Ex: Each year from January 31 to February 01'", expectedTitle, tooltipTitleText);
            VerifyEqual("18-13. Verify Color and Name of the Control Program", controlProgramName, tooltipControlProgramName);
            
            schedulingManagerPage.CalendarEditorPanel.ClickYearBeforeButton();
            schedulingManagerPage.CalendarEditorPanel.ClickYearBeforeButton();
            currentYear = int.Parse(schedulingManagerPage.CalendarEditorPanel.GetCurrentYearText());
            foreach (var dt in weekDates)
            {
                var newDate = string.Format(@"{0}/{1}/{2}", dt.Month, dt.Day, currentYear);
                Step("-> Verify " + newDate);
                var color = schedulingManagerPage.CalendarEditorPanel.GetCalendarDateColor(newDate);
                VerifyEqual(string.Format("18-15. Verify The '{0}' is set the color of the Control Program", newDate), notedColor, color);
            }

            try
            {
                Step("19. Delete the testing calendar after testcase is done.");
                DeleteCalendar(calendarName);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("SM_18_11 Calendar - Calendar editor - Control Program pop-up - Set a control program for the last days of all months.")]
        [NonParallelizable]
        public void SM_18_11()
        {
            var calendarName = SLVHelper.GenerateUniqueName("CSM1811");
            CreateNewCalendar(calendarName);

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - There is an existing calendar");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.SchedulingManager);

            Step("1. Go to Scheduling Manager app");
            Step("2. Expected Scheduling Manager is routed and loaded successfully");            
            var schedulingManagerPage = desktopPage.GoToApp(App.SchedulingManager) as SchedulingManagerPage;

            Step("3. Select 'Calendar' tab in left panel");
            schedulingManagerPage.SchedulingManagerPanel.SelectTab("Calendar");
            schedulingManagerPage.SchedulingManagerPanel.SelectCalendar(calendarName);
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("4. Select the last day of a month");
            var expectedTitle = "Last day of month";
            var currentYear = int.Parse(schedulingManagerPage.CalendarEditorPanel.GetCurrentYearText());
            var lastDayofMonthDates = Settings.GetAllLastDaysOfMonth(currentYear);
            var randomLastDayOfMonth = lastDayofMonthDates.PickRandom();
            schedulingManagerPage.CalendarEditorPanel.ClickCalendarDate(randomLastDayOfMonth.ToString("M/d/yyyy"));
            schedulingManagerPage.WaitForPopupDialogDisplayed();
            
            Step("5. Verify The Control Program pop-up displays with");
            Step(" o Title: Control programs");
            Step(" o All Fields Search textbox and Search Icon");
            Step(" o 5 options: None, Last, Weekly, Monthly, Yearly");
            Step(" o Yearly is selected as default");
            Step(" o List of existing Control Programs with 3 columns: Color, Control Programs Name, Geozone");
            VerifyEqual("5. Verify Title: Control programs", "Control programs", schedulingManagerPage.CalendarControlProgramsPopupPanel.GetPanelTitleText().SplitAndGetAt(1));
            VerifyEqual("5. Verify All Fields Search textbox displays", true, schedulingManagerPage.CalendarControlProgramsPopupPanel.IsSearchInputVisible());
            VerifyEqual("5. Verify 5 options: None, Last, Weekly, Monthly, Yearly", new List<string> { "None", "Last", "Weekly", "Monthly", "Yearly" }, schedulingManagerPage.CalendarControlProgramsPopupPanel.GetListOfOptionsName());
            VerifyEqual("5. Verify Yearly is selected as default", "Yearly", schedulingManagerPage.CalendarControlProgramsPopupPanel.GetSelectedOptionName());
            VerifyEqual("5. Verify List of existing Control Programs with 3 columns: Color, Control Programs Name, Geozone", 3, schedulingManagerPage.CalendarControlProgramsPopupPanel.GetListOfColumnsCount());

            Step("6. Select the option Last");
            schedulingManagerPage.CalendarControlProgramsPopupPanel.ClickRadioLastButton();

            Step("7. Verify Last option is selected");
            VerifyEqual("7. Verify Last option is selected", "Last", schedulingManagerPage.CalendarControlProgramsPopupPanel.GetSelectedOptionName());
            
            Step("8. Select a Control Program and press Save button");
            var controlProgramName = schedulingManagerPage.CalendarControlProgramsPopupPanel.GetListOfItemsName().PickRandom();
            schedulingManagerPage.CalendarControlProgramsPopupPanel.SelectItem(controlProgramName);
            var notedColor = schedulingManagerPage.CalendarControlProgramsPopupPanel.GetSelectedItemColor();
            schedulingManagerPage.CalendarControlProgramsPopupPanel.ClickSaveButton();
            schedulingManagerPage.WaitForPopupDialogDisappeared();

            Step("9. Verify The last days of all months in the current year are set the color of the Control Program..");
            foreach (var dt in lastDayofMonthDates)
            {
                var date = dt.ToString("M/d/yyyy");
                Step("-> Verify " + date);
                var color = schedulingManagerPage.CalendarEditorPanel.GetCalendarDateColor(date);
                VerifyEqual(string.Format("9. Verify The '{0}' is set the color of the Control Program", date), notedColor, color);
            }

            Step("10. Hover randomly one of those days");
            var randomDate = lastDayofMonthDates.PickRandom();
            schedulingManagerPage.CalendarEditorPanel.HoverCalendarDate(randomDate.ToString("M/d/yyyy"));

            Step("11. Verify the tooltip displays with 2 rows");
            Step(" o Clock Icon and the text: 'Last day of month'");
            Step(" o Color and Name of the Control Program");
            var tooltipIcon = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateTitleIcon();
            var tooltipTitleText = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateTitleText();
            var tooltipControlProgramName = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateControlProgramName();
            VerifyEqual("11. Verify Clock Icon displayed", true, tooltipIcon.Contains("clock.png"));
            VerifyEqual("11. Verify the text: 'Last day of month", expectedTitle, tooltipTitleText);
            VerifyEqual("11. Verify Color and Name of the Control Program", controlProgramName, tooltipControlProgramName);

            Step("12. Press Next button to move to the next year");
            schedulingManagerPage.CalendarEditorPanel.ClickYearAfterButton();
            currentYear = int.Parse(schedulingManagerPage.CalendarEditorPanel.GetCurrentYearText());
            lastDayofMonthDates = Settings.GetAllLastDaysOfMonth(currentYear);

            Step("13. Verify The last days of all months in the next year are also set the color of the Control Program");            
            foreach (var dt in lastDayofMonthDates)
            {
                var newDate = string.Format(@"{0}/{1}/{2}", dt.Month, dt.Day, currentYear);
                Step("-> Verify " + newDate);
                var color = schedulingManagerPage.CalendarEditorPanel.GetCalendarDateColor(newDate);
                VerifyEqual(string.Format("13. Verify The '{0}' is set the color of the Control Program", newDate), notedColor, color);
            }

            Step("14. Hover the day");
            randomDate = lastDayofMonthDates.PickRandom();
            var nextDate = string.Format(@"{0}/{1}/{2}", randomDate.Month, randomDate.Day, currentYear);
            schedulingManagerPage.CalendarEditorPanel.HoverCalendarDate(nextDate);

            Step("15. Verify the tooltip displays with 2 rows");
            Step(" o Clock Icon and the text: 'Last day of month'");
            Step(" o Color and Name of the Control Program");
            tooltipIcon = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateTitleIcon();
            tooltipTitleText = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateTitleText();
            tooltipControlProgramName = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateControlProgramName();
            VerifyEqual("15. Verify Clock Icon displayed", true, tooltipIcon.Contains("clock.png"));
            VerifyEqual("15. Verify the text: 'Last day of month'", expectedTitle, tooltipTitleText);
            VerifyEqual("15. Verify Color and Name of the Control Program", controlProgramName, tooltipControlProgramName);

            Step("16. Press Back button twice to move to the previous year");
            schedulingManagerPage.CalendarEditorPanel.ClickYearBeforeButton();
            schedulingManagerPage.CalendarEditorPanel.ClickYearBeforeButton();
            currentYear = int.Parse(schedulingManagerPage.CalendarEditorPanel.GetCurrentYearText());
            lastDayofMonthDates = Settings.GetAllLastDaysOfMonth(currentYear);

            Step("17. Verify tooltip and color the same with next year");            
            foreach (var dt in lastDayofMonthDates)
            {
                var newDate = string.Format(@"{0}/{1}/{2}", dt.Month, dt.Day, currentYear);
                Step("-> Verify " + newDate);
                var color = schedulingManagerPage.CalendarEditorPanel.GetCalendarDateColor(newDate);
                VerifyEqual(string.Format("17. Verify The '{0}' is set the color of the Control Program", newDate), notedColor, color);
            }

            Step("18. Press Save button on Calendar editor to save changes");
            schedulingManagerPage.CalendarEditorPanel.ClickSaveButton();
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("19. Select another calendar, then select the testing calendar again");
            schedulingManagerPage.SchedulingManagerPanel.SelectRandomCalendar();
            schedulingManagerPage.WaitForPreviousActionComplete();
            schedulingManagerPage.SchedulingManagerPanel.SelectCalendar(calendarName);
            schedulingManagerPage.WaitForPreviousActionComplete();            

            Step("20. Verify All changes for the current year, the next year and the previous year are saved correctly (Run all the verify points again)");
            currentYear = int.Parse(schedulingManagerPage.CalendarEditorPanel.GetCurrentYearText());
            lastDayofMonthDates = Settings.GetAllLastDaysOfMonth(currentYear);
            randomDate = lastDayofMonthDates.PickRandom();
            schedulingManagerPage.CalendarEditorPanel.HoverCalendarDate(randomDate.ToString("M/d/yyyy"));
            if (!schedulingManagerPage.CalendarEditorPanel.IsTooltipDateDisplayed())
            {
                Warning("#1285538 Scheduling Manager - Changing calendars clears last changes");
                return;
            }

            tooltipIcon = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateTitleIcon();
            tooltipTitleText = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateTitleText();
            tooltipControlProgramName = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateControlProgramName();
            VerifyEqual("20-11. Verify Clock Icon displayed", true, tooltipIcon.Contains("clock.png"));
            VerifyEqual("20-11. Verify the text: 'Last day of month'", expectedTitle, tooltipTitleText);
            VerifyEqual("20-11. Verify Color and Name of the Control Program", controlProgramName, tooltipControlProgramName);

            schedulingManagerPage.CalendarEditorPanel.ClickYearAfterButton();
            currentYear = int.Parse(schedulingManagerPage.CalendarEditorPanel.GetCurrentYearText());
            lastDayofMonthDates = Settings.GetAllLastDaysOfMonth(currentYear);
            foreach (var dt in lastDayofMonthDates)
            {
                var newDate = string.Format(@"{0}/{1}/{2}", dt.Month, dt.Day, currentYear);
                Step("-> Verify " + newDate);
                var color = schedulingManagerPage.CalendarEditorPanel.GetCalendarDateColor(newDate);
                VerifyEqual(string.Format("20-13. Verify The '{0}' is set the color of the Control Program", newDate), notedColor, color);
            }
            randomDate = lastDayofMonthDates.PickRandom();
            nextDate = string.Format(@"{0}/{1}/{2}", randomDate.Month, randomDate.Day, currentYear);
            schedulingManagerPage.CalendarEditorPanel.HoverCalendarDate(nextDate);

            tooltipIcon = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateTitleIcon();
            tooltipTitleText = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateTitleText();
            tooltipControlProgramName = schedulingManagerPage.CalendarEditorPanel.GetTooltipDateControlProgramName();
            VerifyEqual("20-15. Verify Clock Icon displayed", true, tooltipIcon.Contains("clock.png"));
            VerifyEqual("20-15. Verify the text: 'Last day of month'", expectedTitle, tooltipTitleText);
            VerifyEqual("20-15. Verify Color and Name of the Control Program", controlProgramName, tooltipControlProgramName);

            schedulingManagerPage.CalendarEditorPanel.ClickYearBeforeButton();
            schedulingManagerPage.CalendarEditorPanel.ClickYearBeforeButton();
            currentYear = int.Parse(schedulingManagerPage.CalendarEditorPanel.GetCurrentYearText());
            lastDayofMonthDates = Settings.GetAllLastDaysOfMonth(currentYear);
            foreach (var dt in lastDayofMonthDates)
            {
                var newDate = string.Format(@"{0}/{1}/{2}", dt.Month, dt.Day, currentYear);
                Step("-> Verify " + newDate);
                var color = schedulingManagerPage.CalendarEditorPanel.GetCalendarDateColor(newDate);
                VerifyEqual(string.Format("20-17. Verify The '{0}' is set the color of the Control Program", newDate), notedColor, color);
            }

            try
            {
                Step("19. Delete the testing calendar after testcase is done.");
                DeleteCalendar(calendarName);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("SM_18_12 Calendar - Calendar editor - Calendar Items pop-up")]
        public void SM_18_12()
        {
            var calendarName = SLVHelper.GenerateUniqueName("CSM1812");
            CreateNewCalendar(calendarName);

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - There is an existing calendar");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.SchedulingManager);

            Step("1. Go to Scheduling Manager app");
            Step("2. Expected Scheduling Manager is routed and loaded successfully");
            var schedulingManagerPage = desktopPage.GoToApp(App.SchedulingManager) as SchedulingManagerPage;

            Step("3. Select 'Calendar' tab in left panel and select the testing calendar then press Calendar Items button");
            schedulingManagerPage.SchedulingManagerPanel.SelectTab("Calendar");
            schedulingManagerPage.SchedulingManagerPanel.SelectCalendar(calendarName);
            schedulingManagerPage.WaitForPreviousActionComplete();
            schedulingManagerPage.CalendarEditorPanel.ClickCalendarItemsButton();
            schedulingManagerPage.WaitForPopupDialogDisplayed();

            Step("4. Verify The Calendar Items pop-up displays with");
            Step(" o Title: Calendar items");
            Step(" o Recycle Bin icon, Up and Down icons");
            Step(" o The list of selected calendar with 2 columns: Color, the text");
            Step(" o 2 button icons: Cancel and Save");
            VerifyEqual("5. Verify Title: Calendar items", "Calendar items", schedulingManagerPage.CalendarEditorItemsPopupPanel.GetPanelTitleText().SplitAndGetAt(1));
            VerifyEqual("4. Verify Recycle Bin icon is displayed", true, schedulingManagerPage.CalendarEditorItemsPopupPanel.IsDeleteButtonVisible());
            VerifyEqual("4. Verify Up icons is displayed", true, schedulingManagerPage.CalendarEditorItemsPopupPanel.IsUpButtonVisible());
            VerifyEqual("4. Verify Down icons is displayed", true, schedulingManagerPage.CalendarEditorItemsPopupPanel.IsDownButtonVisible());
            VerifyEqual("4. Verify Save button is displayed", true, schedulingManagerPage.CalendarEditorItemsPopupPanel.IsSaveButtonVisible());
            VerifyEqual("4. Verify Cancel button is displayed", true, schedulingManagerPage.CalendarEditorItemsPopupPanel.IsCancelButtonVisible());            
            VerifyEqual("4. Verify The list of selected calendar with 2 columns: Color, the text", 2, schedulingManagerPage.CalendarEditorItemsPopupPanel.GetListOfColumnsCount());
           
            Step("5. Press Close button");
            schedulingManagerPage.CalendarEditorItemsPopupPanel.ClickCloseButton();
            schedulingManagerPage.WaitForPopupDialogDisappeared();

            Step("6. Verify The pop-up is closed");
            VerifyEqual("6. Verify The pop-up is closed", true, !schedulingManagerPage.IsPopupDialogDisplayed());

            Step("7. Add a control program yearly for a day in the calendar and press Calendar Items button");
            var randomDate = schedulingManagerPage.CalendarEditorPanel.ClickRandomCalendarDate();
            var date = DateTime.Parse(randomDate);
            schedulingManagerPage.WaitForPopupDialogDisplayed();
            var controlProgram = schedulingManagerPage.CalendarControlProgramsPopupPanel.SelectRandomItem();
            schedulingManagerPage.CalendarControlProgramsPopupPanel.ClickSaveButton();
            schedulingManagerPage.WaitForPopupDialogDisappeared();
            var expectedColor = schedulingManagerPage.CalendarEditorPanel.GetCalendarDateColor(randomDate);
            var expectedItemName = string.Format("Each year the {0} {1}", date.ToString("MMMM"), date.Day.ToString("D2"));

            schedulingManagerPage.CalendarEditorPanel.ClickCalendarItemsButton();
            schedulingManagerPage.WaitForPopupDialogDisplayed();

            Step("8. Verify The list of Calendar Items is filled with 1 row of the newly added control program with");
            Step(" o The color of the program control");
            Step(" o The text 'Each year the [name of month] [date]'");
          
            var listItems = schedulingManagerPage.CalendarEditorItemsPopupPanel.GetListOfItemsName();
            var actualItemName = listItems.FirstOrDefault();
            var actualDateColor = schedulingManagerPage.CalendarEditorItemsPopupPanel.GetColorOfItem(actualItemName);
            VerifyEqual("8. Verify The list of Calendar Items is filled with 1 row of the newly added control program", 1, listItems.Count);
            VerifyEqual("8. Verify The color of the program control", expectedColor, actualDateColor);
            VerifyEqual("8. Verify The text 'Each year the [name of month] [date]'", expectedItemName, actualItemName);

            Step("9. Select the row in the list");
            schedulingManagerPage.CalendarEditorItemsPopupPanel.SelectItem(expectedItemName);

            Step("10. Verify The background color of that row is changed");
            VerifyEqual("10. Verify The background color of that row is changed", true, schedulingManagerPage.CalendarEditorItemsPopupPanel.HasSelectedItem());
            
            Step("11. Press Delete button");
            schedulingManagerPage.CalendarEditorItemsPopupPanel.ClickDeleteButton();
            schedulingManagerPage.CalendarEditorItemsPopupPanel.WaitForPopupMessageDisplayed();

            Step("12. Verify The confirmation pop-up displays");
            Step(" o The text: Do you want to delete the selected calendar item ?");
            VerifyEqual("12. Verify The text: Do you want to delete the selected calendar item ?", "Do you want to delete the selected calendar item ?", schedulingManagerPage.CalendarEditorItemsPopupPanel.GetMessageText());

            Step("13. Press No button");
            schedulingManagerPage.CalendarEditorItemsPopupPanel.ClickPopupMessageNoButton();
            schedulingManagerPage.CalendarEditorItemsPopupPanel.WaitForPopupMessageDisappeared();

            Step("14. Verify The warning pop-up is closed and the row is still displayed on the list");
            VerifyEqual("14. Verify The warning pop-up is closed", true, !schedulingManagerPage.CalendarEditorItemsPopupPanel.IsPopupMessageDisplayed());
            VerifyEqual("14. Verify The row is still displayed on the list", true, schedulingManagerPage.CalendarEditorItemsPopupPanel.IsItemExisting(expectedItemName));

            Step("15. Press Delete button again and press Yes on the pop-up");
            schedulingManagerPage.CalendarEditorItemsPopupPanel.ClickDeleteButton();
            schedulingManagerPage.CalendarEditorItemsPopupPanel.WaitForPopupMessageDisplayed();
            schedulingManagerPage.CalendarEditorItemsPopupPanel.ClickPopupMessageYesButton();
            schedulingManagerPage.CalendarEditorItemsPopupPanel.WaitForPopupMessageDisappeared();

            Step("16. Verify The row is deleted");
            VerifyEqual("16. Verify The row is deleted", true, !schedulingManagerPage.CalendarEditorItemsPopupPanel.IsItemExisting(expectedItemName));

            Step("17. Press Cancel button on the Calendar Items");
            schedulingManagerPage.CalendarEditorItemsPopupPanel.ClickCancelButton();
            schedulingManagerPage.WaitForPopupDialogDisappeared();

            Step("18. Verify The selected date is still set the color of the control program");
            actualDateColor = schedulingManagerPage.CalendarEditorPanel.GetCalendarDateColor(randomDate);
            VerifyEqual("18. Verify The selected date is still set the color of the control program", expectedColor, actualDateColor);

            Step("19. Press Calendar Items button again");
            schedulingManagerPage.CalendarEditorPanel.ClickCalendarItemsButton();
            schedulingManagerPage.WaitForPopupDialogDisplayed();

            Step("20. Verify The row is still displayed on the list");
            VerifyEqual("20. Verify The row is still displayed on the list", true, schedulingManagerPage.CalendarEditorItemsPopupPanel.IsItemExisting(expectedItemName));

            Step("21. Select the row and press Delete button, and press Yes on the confirmation pop-up then press Save button");
            schedulingManagerPage.CalendarEditorItemsPopupPanel.SelectItem(expectedItemName);
            schedulingManagerPage.CalendarEditorItemsPopupPanel.ClickDeleteButton();
            schedulingManagerPage.CalendarEditorItemsPopupPanel.WaitForPopupMessageDisplayed();
            schedulingManagerPage.CalendarEditorItemsPopupPanel.ClickPopupMessageYesButton();
            schedulingManagerPage.CalendarEditorItemsPopupPanel.WaitForPopupMessageDisappeared();
            schedulingManagerPage.CalendarEditorItemsPopupPanel.ClickSaveButton();
            schedulingManagerPage.WaitForPopupDialogDisappeared();

            Step("22. Verify the selected date is removed the color of the control program");
            actualDateColor = schedulingManagerPage.CalendarEditorPanel.GetCalendarDateColor(randomDate);
            VerifyTrue("22. Verify the selected date is removed the color of the control program", expectedColor != actualDateColor, expectedColor, actualDateColor);
            
            Step("23. Press Calendar Items button again");
            schedulingManagerPage.CalendarEditorPanel.ClickCalendarItemsButton();
            schedulingManagerPage.WaitForPopupDialogDisplayed();

            Step("24. Verify the list is empty");
            listItems = schedulingManagerPage.CalendarEditorItemsPopupPanel.GetListOfItemsName();
            VerifyEqual("24. Verify The list of Calendar Items is filled with 1 row of the newly added control program", 0, listItems.Count);

            try
            {
                DeleteCalendar(calendarName);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("SM_18_13 Calendar - Calendar editor - Calendar Items pop-up - Set the many control programs on a date")]
        public void SM_18_13()
        {
            var calendarName = SLVHelper.GenerateUniqueName("CSM1813");
            CreateNewCalendar(calendarName);

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - There is an existing calendar");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.SchedulingManager);

            Step("1. Go to Scheduling Manager app");
            Step("2. Expected Scheduling Manager is routed and loaded successfully");            
            var schedulingManagerPage = desktopPage.GoToApp(App.SchedulingManager) as SchedulingManagerPage;

            Step("3. Select 'Calendar' tab in left panel and select the testing calendar");
            schedulingManagerPage.SchedulingManagerPanel.SelectTab("Calendar");
            schedulingManagerPage.SchedulingManagerPanel.SelectCalendar(calendarName);
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("4. Press the last day of a random month");
            int currentYear = int.Parse(schedulingManagerPage.CalendarEditorPanel.GetCurrentYearText());
            var listLastDayOfMonthDates = Settings.GetAllLastDaysOfMonth(currentYear);
            var randomLastDate = listLastDayOfMonthDates.PickRandom();

            Step("5. Add a control program yearly for that day.");
            schedulingManagerPage.CalendarEditorPanel.ClickCalendarDate(randomLastDate.ToString("M/d/yyyy"));
            schedulingManagerPage.WaitForPopupDialogDisplayed();
            var controlProgramYearly = schedulingManagerPage.CalendarControlProgramsPopupPanel.SelectRandomItem();
            schedulingManagerPage.CalendarControlProgramsPopupPanel.ClickSaveButton();
            schedulingManagerPage.WaitForPopupDialogDisappeared();
            var colorYearly = schedulingManagerPage.CalendarEditorPanel.GetCalendarDateColor(randomLastDate.ToString("M/d/yyyy"));
            var itemNameYear = string.Format("Each year the {0}", randomLastDate.ToString("MMMM dd"));

            Step("6. Press Calendar Items button");
            schedulingManagerPage.CalendarEditorPanel.ClickCalendarItemsButton();
            schedulingManagerPage.WaitForPopupDialogDisplayed();

            Step("7. Verify There is a row added to the list with");
            Step(" o The color of the selected control program");
            Step(" o The text 'Each year the [name of month] [date]'. Ex: Each year the March 31");
            var listItems = schedulingManagerPage.CalendarEditorItemsPopupPanel.GetListOfItemsName();
            var actualDateColor = schedulingManagerPage.CalendarEditorItemsPopupPanel.GetColorOfItem(itemNameYear);
            VerifyEqual("7. Verify There is a row added to the list", 1, listItems.Count);
            VerifyEqual("7. Verify The color of the selected control program", colorYearly, actualDateColor);
            VerifyEqual("7. Verify The text 'Each year the [name of month] [date]'. Ex: Each year the March 31", itemNameYear, listItems.FirstOrDefault());

            Step("8. Close the pop-up and press that day again");
            schedulingManagerPage.CalendarEditorItemsPopupPanel.ClickCloseButton();
            schedulingManagerPage.WaitForPopupDialogDisappeared();
            schedulingManagerPage.CalendarEditorPanel.ClickCalendarDate(randomLastDate.ToString("M/d/yyyy"));
            schedulingManagerPage.WaitForPopupDialogDisplayed();

            Step("9. Select 'Monthly' option and another control program, then press Save");
            schedulingManagerPage.CalendarControlProgramsPopupPanel.ClickRadioMonthlyButton();
            var controlProgramMonthly = schedulingManagerPage.CalendarControlProgramsPopupPanel.SelectRandomItem(controlProgramYearly);
            var colorMonthly = schedulingManagerPage.CalendarControlProgramsPopupPanel.GetSelectedItemColor();
            schedulingManagerPage.CalendarControlProgramsPopupPanel.ClickSaveButton();
            schedulingManagerPage.WaitForPopupDialogDisappeared();
            var itemNameMonthly = string.Format("Each month the {0}", randomLastDate.Day);

            Step("10. Verify The background color of the selected day is replaced by the color of the newly selected control program in all month");
            var listSelectedDatesMonthly = listLastDayOfMonthDates.Where(p => p.Day == randomLastDate.Day);
            foreach (var dt in listSelectedDatesMonthly)
            {
                var date = dt.ToString("M/d/yyyy");
                Step("-> Verify " + date);
                var color = schedulingManagerPage.CalendarEditorPanel.GetCalendarDateColor(date);
                VerifyEqual(string.Format("10. Verify The '{0}' is set the color of the Control Program", date), colorMonthly, color);
            }

            Step("11. Press Calendar Items button");
            schedulingManagerPage.CalendarEditorPanel.ClickCalendarItemsButton();
            schedulingManagerPage.WaitForPopupDialogDisplayed();

            Step("12. Verify There is another row added at the 1st position with");
            Step(" o The color of the newly selected control program");
            Step(" o The test 'Each month the [date]'. Ex: Each month the 31");
            listItems = schedulingManagerPage.CalendarEditorItemsPopupPanel.GetListOfItemsName();
            actualDateColor = schedulingManagerPage.CalendarEditorItemsPopupPanel.GetColorOfItem(itemNameMonthly);
            VerifyEqual("12. Verify There is a row added to the list", 2, listItems.Count);
            VerifyEqual("12. Verify The color of the newly selected control program", colorMonthly, actualDateColor);
            VerifyEqual("12. Verify The text 'Each month the [date]'. Ex: Each month the 31", true, schedulingManagerPage.CalendarEditorItemsPopupPanel.IsItemExisting(itemNameMonthly));
            VerifyEqual("12. Verify The text of row added at the 1st position: 'Each month the [date]'. Ex: Each month the 31", itemNameMonthly, listItems.FirstOrDefault());

            Step("13. Close the pop-up and press that day again");
            schedulingManagerPage.CalendarEditorItemsPopupPanel.ClickCloseButton();
            schedulingManagerPage.WaitForPopupDialogDisappeared();
            schedulingManagerPage.CalendarEditorPanel.ClickCalendarDate(randomLastDate.ToString("M/d/yyyy"));
            schedulingManagerPage.WaitForPopupDialogDisplayed();

            Step("14. Select 'Weekly' option and another control program, then press Save");
            schedulingManagerPage.CalendarControlProgramsPopupPanel.ClickRadioWeeklyButton();
            var controlProgramWeekly = schedulingManagerPage.CalendarControlProgramsPopupPanel.SelectRandomItem(controlProgramYearly, controlProgramMonthly);
            var colorWeekly = schedulingManagerPage.CalendarControlProgramsPopupPanel.GetSelectedItemColor();
            schedulingManagerPage.CalendarControlProgramsPopupPanel.ClickSaveButton();
            schedulingManagerPage.WaitForPopupDialogDisappeared();
            var itemNameWeekly = string.Format("Each {0}", randomLastDate.DayOfWeek);

            Step("15. Verify The background color of all the days which has the same name of date (ex: Friday) are replaced by the color of the newly selected control program.");
            var listSelectedDatesWeekly = Settings.GetAllDaysOfWeek(currentYear, randomLastDate.DayOfWeek);
            foreach (var dt in listSelectedDatesWeekly)
            {
                var date = dt.ToString("M/d/yyyy");
                Step("-> Verify " + date);
                var color = schedulingManagerPage.CalendarEditorPanel.GetCalendarDateColor(date);
                VerifyEqual(string.Format("15. Verify The '{0}' is set the color of the Control Program", date), colorWeekly, color);
            }

            Step("16. Press Calendar Items button");
            schedulingManagerPage.CalendarEditorPanel.ClickCalendarItemsButton();
            schedulingManagerPage.WaitForPopupDialogDisplayed();

            Step("17. Verify There is another row added at the 1st position with");
            Step(" o The color of the newly selected control program");
            Step(" o The test 'Each [name of date]'. Ex: Each Friday");
            listItems = schedulingManagerPage.CalendarEditorItemsPopupPanel.GetListOfItemsName();
            actualDateColor = schedulingManagerPage.CalendarEditorItemsPopupPanel.GetColorOfItem(itemNameWeekly);
            VerifyEqual("17. Verify There is a row added to the list", 3, listItems.Count);
            VerifyEqual("17. Verify The color of the newly selected control program", colorWeekly, actualDateColor);
            VerifyEqual("17. Verify The text of row added at the 1st position: 'Each [name of date]'. Ex: Each Friday", itemNameWeekly, listItems.FirstOrDefault());

            Step("18. Close the pop-up and press that day again");
            schedulingManagerPage.CalendarEditorItemsPopupPanel.ClickCloseButton();
            schedulingManagerPage.WaitForPopupDialogDisappeared();
            schedulingManagerPage.CalendarEditorPanel.ClickCalendarDate(randomLastDate.ToString("M/d/yyyy"));
            schedulingManagerPage.WaitForPopupDialogDisplayed();

            Step("19. Select 'Last' option and another control program, then press Save");
            schedulingManagerPage.CalendarControlProgramsPopupPanel.ClickRadioLastButton();
            var controlProgramLast = schedulingManagerPage.CalendarControlProgramsPopupPanel.SelectRandomItem(controlProgramYearly, controlProgramMonthly, controlProgramWeekly);
            var colorLast = schedulingManagerPage.CalendarControlProgramsPopupPanel.GetSelectedItemColor();
            schedulingManagerPage.CalendarControlProgramsPopupPanel.ClickSaveButton();
            schedulingManagerPage.WaitForPopupDialogDisappeared();
            var itemNameLast = "Last day of month";

            Step("20. Verify The background color of the last day of all months are replaced by the color of the newly selected control program.");
            foreach (var dt in listLastDayOfMonthDates)
            {
                var date = dt.ToString("M/d/yyyy");
                Step("-> Verify " + date);
                var color = schedulingManagerPage.CalendarEditorPanel.GetCalendarDateColor(date);
                VerifyEqual(string.Format("15. Verify The '{0}' is set the color of the Control Program", date), colorLast, color);
            }

            Step("21. Press Calendar Items button");
            schedulingManagerPage.CalendarEditorPanel.ClickCalendarItemsButton();
            schedulingManagerPage.WaitForPopupDialogDisplayed();

            Step("22. Verify There is another row added at the 1st position with");
            Step(" o The color of the newly selected control program");
            Step(" o The test 'Last day of month'");
            listItems = schedulingManagerPage.CalendarEditorItemsPopupPanel.GetListOfItemsName();
            actualDateColor = schedulingManagerPage.CalendarEditorItemsPopupPanel.GetColorOfItem(itemNameLast);
            VerifyEqual("22. Verify There is a row added to the list", 4, listItems.Count);
            VerifyEqual("22. Verify The color of the newly selected control program", colorLast, actualDateColor);
            VerifyEqual("22. Verify The text of row added at the 1st position: 'Last day of month'", itemNameLast, listItems.FirstOrDefault());
            
            Step("23. Close the pop-up and press that day again");
            schedulingManagerPage.CalendarEditorItemsPopupPanel.ClickCloseButton();
            schedulingManagerPage.WaitForPopupDialogDisappeared();
            schedulingManagerPage.CalendarEditorPanel.ClickCalendarDate(randomLastDate.ToString("M/d/yyyy"));
            schedulingManagerPage.WaitForPopupDialogDisplayed();

            Step("24. Select 'None' option and another control program, then press Save");
            schedulingManagerPage.CalendarControlProgramsPopupPanel.ClickRadioNoneButton();
            var controlProgramNone = schedulingManagerPage.CalendarControlProgramsPopupPanel.SelectRandomItem(controlProgramYearly, controlProgramMonthly, controlProgramWeekly, controlProgramLast);
            var colorNone = schedulingManagerPage.CalendarControlProgramsPopupPanel.GetSelectedItemColor();
            schedulingManagerPage.CalendarControlProgramsPopupPanel.ClickSaveButton();
            schedulingManagerPage.WaitForPopupDialogDisappeared();
            var itemNameNone = string.Format("The {0}, {1}", randomLastDate.DayOfWeek, randomLastDate.ToString("MMMM dd, yyyy"));

            Step("25. Verify The background color of the selected day is replaced by the color of the newly selected control program");
            actualDateColor = schedulingManagerPage.CalendarEditorPanel.GetCalendarDateColor(randomLastDate.ToString("M/d/yyyy"));
            VerifyEqual("25. Verify The background color of the selected day is replaced by the color of the newly selected control program", colorNone, actualDateColor);

            Step("26. Press Calendar Items button");
            schedulingManagerPage.CalendarEditorPanel.ClickCalendarItemsButton();
            schedulingManagerPage.WaitForPopupDialogDisplayed();

            Step("27. Verify There is another row added at the 1st position with");
            Step(" o The color of the newly selected control program");
            Step(" o The test 'The [name of day], [name of month] [date], [year]'. Ex: 'The Friday, Match 31, 2017'");
            listItems = schedulingManagerPage.CalendarEditorItemsPopupPanel.GetListOfItemsName();
            actualDateColor = schedulingManagerPage.CalendarEditorItemsPopupPanel.GetColorOfItem(itemNameNone);
            VerifyEqual("27. Verify There is a row added to the list", 5, listItems.Count);
            VerifyEqual("27. Verify The color of the newly selected control program", colorNone, actualDateColor);
            VerifyEqual("27. Verify The text of row added at the 1st position: 'The [name of day], [name of month] [date], [year]'. Ex: 'The Friday, Match 31, 2017'", itemNameNone, listItems.FirstOrDefault());

            try
            {
                DeleteCalendar(calendarName);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("SM_18_14 Calendar - Calendar editor - Calendar Items pop-up - Set the priority for control programs of a day")]
        public void SM_18_14()
        {
            var calendarName = SLVHelper.GenerateUniqueName("CSM1814");
            CreateNewCalendar(calendarName);

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - There is an existing calendar");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.SchedulingManager);

            Step("1. Go to Scheduling Manager app");
            Step("2. Expected Scheduling Manager is routed and loaded successfully");           
            var schedulingManagerPage = desktopPage.GoToApp(App.SchedulingManager) as SchedulingManagerPage;

            Step("3. Select 'Calendar' tab in left panel and select the testing calendar");
            schedulingManagerPage.SchedulingManagerPanel.SelectTab("Calendar");
            schedulingManagerPage.SchedulingManagerPanel.SelectCalendar(calendarName);
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("4. Press the last day of a random month");
            int currentYear = int.Parse(schedulingManagerPage.CalendarEditorPanel.GetCurrentYearText());
            var listLastDayOfMonthDates = Settings.GetAllLastDaysOfMonth(currentYear);
            var randomLastDate = listLastDayOfMonthDates.PickRandom();
            var dateStr = randomLastDate.ToString("M/d/yyyy");

            Step("5. Add a Yearly control program for that day.");
            schedulingManagerPage.CalendarEditorPanel.ClickCalendarDate(dateStr);
            schedulingManagerPage.WaitForPopupDialogDisplayed();
            var controlProgramYearly = schedulingManagerPage.CalendarControlProgramsPopupPanel.SelectRandomItem();
            schedulingManagerPage.CalendarControlProgramsPopupPanel.ClickSaveButton();
            schedulingManagerPage.WaitForPopupDialogDisappeared();
            var colorYearly = schedulingManagerPage.CalendarEditorPanel.GetCalendarDateColor(dateStr);
            var itemNameYearly = string.Format("Each year the {0}", randomLastDate.ToString("MMMM dd"));

            Step("6. Continue to add control programs Monthly/Weekly/Last/None for that day");
            schedulingManagerPage.CalendarEditorPanel.ClickCalendarDate(dateStr);
            schedulingManagerPage.WaitForPopupDialogDisplayed();
            schedulingManagerPage.CalendarControlProgramsPopupPanel.ClickRadioMonthlyButton();
            var controlProgramMonthly = schedulingManagerPage.CalendarControlProgramsPopupPanel.SelectRandomItem(controlProgramYearly);
            var colorMonthly = schedulingManagerPage.CalendarControlProgramsPopupPanel.GetSelectedItemColor();
            schedulingManagerPage.CalendarControlProgramsPopupPanel.ClickSaveButton();
            schedulingManagerPage.WaitForPopupDialogDisappeared();
            var itemNameMonthly = string.Format("Each month the {0}", randomLastDate.Day);            

            schedulingManagerPage.CalendarEditorPanel.ClickCalendarDate(dateStr);
            schedulingManagerPage.WaitForPopupDialogDisplayed();
            schedulingManagerPage.CalendarControlProgramsPopupPanel.ClickRadioWeeklyButton();
            var controlProgramWeekly = schedulingManagerPage.CalendarControlProgramsPopupPanel.SelectRandomItem(controlProgramYearly, controlProgramMonthly);
            var colorWeekly = schedulingManagerPage.CalendarControlProgramsPopupPanel.GetSelectedItemColor();
            schedulingManagerPage.CalendarControlProgramsPopupPanel.ClickSaveButton();
            schedulingManagerPage.WaitForPopupDialogDisappeared();
            var itemNameWeekly = string.Format("Each {0}", randomLastDate.DayOfWeek);
            
            schedulingManagerPage.CalendarEditorPanel.ClickCalendarDate(dateStr);
            schedulingManagerPage.WaitForPopupDialogDisplayed();
            schedulingManagerPage.CalendarControlProgramsPopupPanel.ClickRadioLastButton();
            var controlProgramLast = schedulingManagerPage.CalendarControlProgramsPopupPanel.SelectRandomItem(controlProgramYearly, controlProgramMonthly, controlProgramWeekly);
            var colorLast = schedulingManagerPage.CalendarControlProgramsPopupPanel.GetSelectedItemColor();
            schedulingManagerPage.CalendarControlProgramsPopupPanel.ClickSaveButton();
            schedulingManagerPage.WaitForPopupDialogDisappeared();
            var itemNameLast = "Last day of month";
            
            schedulingManagerPage.CalendarEditorPanel.ClickCalendarDate(dateStr);
            schedulingManagerPage.WaitForPopupDialogDisplayed();
            schedulingManagerPage.CalendarControlProgramsPopupPanel.ClickRadioNoneButton();
            var controlProgramNone = schedulingManagerPage.CalendarControlProgramsPopupPanel.SelectRandomItem(controlProgramYearly, controlProgramMonthly, controlProgramWeekly, controlProgramLast);
            var colorNone = schedulingManagerPage.CalendarControlProgramsPopupPanel.GetSelectedItemColor();
            schedulingManagerPage.CalendarControlProgramsPopupPanel.ClickSaveButton();
            schedulingManagerPage.WaitForPopupDialogDisappeared();
            var itemNameNone = string.Format("The {0}, {1}", randomLastDate.DayOfWeek, randomLastDate.ToString("MMMM dd, yyyy"));

            Step("7. Press Calendar Items button");
            schedulingManagerPage.CalendarEditorPanel.ClickCalendarItemsButton();
            schedulingManagerPage.WaitForPopupDialogDisplayed();

            Step("8. Verify The 5 rows are added to the list");
            var listItems = schedulingManagerPage.CalendarEditorItemsPopupPanel.GetListOfItemsName();
            VerifyEqual("8. Verify The 5 rows are added to the list", 5, listItems.Count);

            Step("9. Select the last row (Yearly control program), press Up button 4 times to move to the top");
            schedulingManagerPage.CalendarEditorItemsPopupPanel.SelectItem(listItems.Last());
            schedulingManagerPage.CalendarEditorItemsPopupPanel.ClickUpButton();
            schedulingManagerPage.CalendarEditorItemsPopupPanel.ClickUpButton();
            schedulingManagerPage.CalendarEditorItemsPopupPanel.ClickUpButton();
            schedulingManagerPage.CalendarEditorItemsPopupPanel.ClickUpButton();            
            
            Step("10. Verify That row is moved to the top");
            listItems = schedulingManagerPage.CalendarEditorItemsPopupPanel.GetListOfItemsName();
            VerifyEqual("10. Verify That row is moved to the top", 0, listItems.IndexOf(itemNameYearly));

            Step("11. Press Save button");
            schedulingManagerPage.CalendarEditorItemsPopupPanel.ClickSaveButton();
            schedulingManagerPage.WaitForPopupDialogDisappeared();

            Step("12. Verify The background color of the that day is replaced by the color of Yearly control program");
            var actualDateColor = schedulingManagerPage.CalendarEditorPanel.GetCalendarDateColor(dateStr);
            VerifyEqual("12. Verify The background color of the that day is replaced by the color of Yearly control program", colorYearly, actualDateColor);

            Step("13. Press Calendar Items button again");
            schedulingManagerPage.CalendarEditorPanel.ClickCalendarItemsButton();
            schedulingManagerPage.WaitForPopupDialogDisplayed();

            Step("14. Select the last row (Monthly control program), press Up button 4 times to move to the top");
            schedulingManagerPage.CalendarEditorItemsPopupPanel.SelectItem(listItems.Last());
            schedulingManagerPage.CalendarEditorItemsPopupPanel.ClickUpButton();
            schedulingManagerPage.CalendarEditorItemsPopupPanel.ClickUpButton();
            schedulingManagerPage.CalendarEditorItemsPopupPanel.ClickUpButton();
            schedulingManagerPage.CalendarEditorItemsPopupPanel.ClickUpButton();

            Step("15. Verify That row is moved to the top");
            listItems = schedulingManagerPage.CalendarEditorItemsPopupPanel.GetListOfItemsName();
            VerifyEqual("15. Verify That row is moved to the top", 0, listItems.IndexOf(itemNameMonthly));

            Step("16. Press Save button");
            schedulingManagerPage.CalendarEditorItemsPopupPanel.ClickSaveButton();
            schedulingManagerPage.WaitForPopupDialogDisappeared();

            Step("17. Verify The background color of the that day in each month is replaced by the color of Monthly control program");
            var listSelectedDatesMonthly = listLastDayOfMonthDates.Where(p => p.Day == randomLastDate.Day);
            foreach (var dt in listSelectedDatesMonthly)
            {
                var date = dt.ToString("M/d/yyyy");
                Step("-> Verify " + date);
                var color = schedulingManagerPage.CalendarEditorPanel.GetCalendarDateColor(date);
                VerifyEqual(string.Format("17. Verify The '{0}' is set the color of Monthly Control Program", date), colorMonthly, color);
            }

            Step("18. Press Calendar Items button again");
            schedulingManagerPage.CalendarEditorPanel.ClickCalendarItemsButton();
            schedulingManagerPage.WaitForPopupDialogDisplayed();

            Step("19. Select the 1st row, press Down button 4 times to move to the bottom");
            listItems = schedulingManagerPage.CalendarEditorItemsPopupPanel.GetListOfItemsName();
            schedulingManagerPage.CalendarEditorItemsPopupPanel.SelectItem(listItems.First());
            schedulingManagerPage.CalendarEditorItemsPopupPanel.ClickDownButton();
            schedulingManagerPage.CalendarEditorItemsPopupPanel.ClickDownButton();
            schedulingManagerPage.CalendarEditorItemsPopupPanel.ClickDownButton();
            schedulingManagerPage.CalendarEditorItemsPopupPanel.ClickDownButton();

            Step("20. Select the 2nd row, press Down button 4 times to move to the bottom");
            listItems = schedulingManagerPage.CalendarEditorItemsPopupPanel.GetListOfItemsName();
            schedulingManagerPage.CalendarEditorItemsPopupPanel.SelectItem(listItems.First());
            schedulingManagerPage.CalendarEditorItemsPopupPanel.ClickDownButton();
            schedulingManagerPage.CalendarEditorItemsPopupPanel.ClickDownButton();
            schedulingManagerPage.CalendarEditorItemsPopupPanel.ClickDownButton();
            schedulingManagerPage.CalendarEditorItemsPopupPanel.ClickDownButton();

            Step("21. Verify the 1st row is now None control program");
            listItems = schedulingManagerPage.CalendarEditorItemsPopupPanel.GetListOfItemsName();
            VerifyEqual("21. Verify the 1st row is now None control program", itemNameNone, listItems.First());

            Step("22. Press Save button");
            schedulingManagerPage.CalendarEditorItemsPopupPanel.ClickSaveButton();
            schedulingManagerPage.WaitForPopupDialogDisappeared();

            Step("23. Verify The background color of that day is replaced by the color of None control program");
            actualDateColor = schedulingManagerPage.CalendarEditorPanel.GetCalendarDateColor(dateStr);
            VerifyEqual("23. Verify The background color of that day is replaced by the color of None control program", colorNone, actualDateColor);

            try
            {
                DeleteCalendar(calendarName);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("SM_18_15 Calendar - Calendar editor - Calendar Items pop-up - Set the many control programs on mulitple days")]
        [NonParallelizable]
        [Category("RunAlone")]
        public void SM_18_15()
        {
            var calendarName = SLVHelper.GenerateUniqueName("CSM1815");
            CreateNewCalendar(calendarName);

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - There is an existing calendar");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.SchedulingManager);

            Step("1. Go to Scheduling Manager app");
            Step("2. Expected Scheduling Manager is routed and loaded successfully");
            var schedulingManagerPage = desktopPage.GoToApp(App.SchedulingManager) as SchedulingManagerPage;

            Step("3. Select 'Calendar' tab in left panel and select the testing calendar");
            schedulingManagerPage.SchedulingManagerPanel.SelectTab("Calendar");
            schedulingManagerPage.SchedulingManagerPanel.SelectCalendar(calendarName);
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("4. Press the 2 back-to-back days of a random month");
            int currentYear = int.Parse(schedulingManagerPage.CalendarEditorPanel.GetCurrentYearText());
            var listDates = schedulingManagerPage.CalendarEditorPanel.DragAndDropRandomDays(currentYear, DayOfWeek.Tuesday, 2);            
            schedulingManagerPage.WaitForPopupDialogDisplayed();
            var dateFrom = listDates.First();
            var dateTo = listDates.Last();
            var itemNameYearly = string.Format("Each year from {0} to {1}", dateFrom.ToString("MMMM dd"), dateTo.ToString("MMMM dd"));

            Step("5. Add a control program yearly for that 2 days.");            
            var controlProgramYearly = schedulingManagerPage.CalendarControlProgramsPopupPanel.SelectRandomItem();
            var colorYearly = schedulingManagerPage.CalendarControlProgramsPopupPanel.GetSelectedItemColor();
            schedulingManagerPage.CalendarControlProgramsPopupPanel.ClickSaveButton();
            schedulingManagerPage.WaitForPopupDialogDisappeared();
           
            Step("6. Press Calendar Items button");
            schedulingManagerPage.CalendarEditorPanel.ClickCalendarItemsButton();
            schedulingManagerPage.WaitForPopupDialogDisplayed();

            Step("7. Verify There is a row added to the list with");
            Step(" o The color of the selected control program");
            Step(" o The text 'Each year from [name of month] [start day] to [name of month] [end day]'. Ex: Each year from January 07 to January 08");
            var listItems = schedulingManagerPage.CalendarEditorItemsPopupPanel.GetListOfItemsName();
            var actualDateColor = schedulingManagerPage.CalendarEditorItemsPopupPanel.GetColorOfItem(itemNameYearly);
            VerifyEqual("7. Verify There is a row added to the list", 1, listItems.Count);
            VerifyEqual("7. Verify The color of the selected control program", colorYearly, actualDateColor);
            VerifyEqual("7. Verify The text 'Each year from [name of month] [start day] to [name of month] [end day]'. Ex: Each year from January 07 to January 08", itemNameYearly, listItems.First());

            Step("8. Close the pop-up and select that 2 days again");
            schedulingManagerPage.CalendarEditorItemsPopupPanel.ClickCloseButton();
            schedulingManagerPage.WaitForPopupDialogDisappeared();
            schedulingManagerPage.CalendarEditorPanel.DragAndDropFromDateToDate(dateFrom, dateTo);
            schedulingManagerPage.WaitForPopupDialogDisplayed();

            Step("9. Select 'Monthly' option and another control program, then press Save");
            schedulingManagerPage.CalendarControlProgramsPopupPanel.ClickRadioMonthlyButton();
            var controlProgramMonthly = schedulingManagerPage.CalendarControlProgramsPopupPanel.SelectRandomItem(controlProgramYearly);
            var colorMonthly = schedulingManagerPage.CalendarControlProgramsPopupPanel.GetSelectedItemColor();
            schedulingManagerPage.CalendarControlProgramsPopupPanel.ClickSaveButton();
            schedulingManagerPage.WaitForPopupDialogDisappeared();
            var itemNameMonthly = string.Format("Each month from {0} to {1}", dateFrom.Day, dateTo.Day);

            Step("10. Verify The background color of the selected days are replaced by the color of the newly selected control program in all months");
            for (int i = 1; i <= 12; i++)
            {
                var date = string.Format(@"{0}/{1}/{2}", i, dateFrom.Day, dateFrom.Year);
                Step("-> Verify " + date);
                var colorFrom = schedulingManagerPage.CalendarEditorPanel.GetCalendarDateColor(date);
                VerifyEqual(string.Format("10. Verify The '{0}' is set the color of Monthly Control Program", date), colorMonthly, colorFrom);
                date = string.Format(@"{0}/{1}/{2}", i, dateTo.Day, dateTo.Year);
                Step("-> Verify " + date);
                var colorTo = schedulingManagerPage.CalendarEditorPanel.GetCalendarDateColor(date);
                VerifyEqual(string.Format("10. Verify The '{0}' is set the color of Monthly Control Program", date), colorMonthly, colorTo);
            }

            Step("11. Press Calendar Items button");
            schedulingManagerPage.CalendarEditorPanel.ClickCalendarItemsButton();
            schedulingManagerPage.WaitForPopupDialogDisplayed();

            Step("12. Verify There is another row added at the 1st position with");
            Step(" o The color of the newly selected control program");
            Step(" o The test 'Each month from [start day] to [end day]'. Ex: Each month from 7 to 8");
            listItems = schedulingManagerPage.CalendarEditorItemsPopupPanel.GetListOfItemsName();
            actualDateColor = schedulingManagerPage.CalendarEditorItemsPopupPanel.GetColorOfItem(itemNameMonthly);
            VerifyEqual("12. Verify There is a row added to the list", 2, listItems.Count);
            VerifyEqual("12. Verify The color of the newly selected control program", colorMonthly, actualDateColor);
            VerifyEqual("12. Verify The text 'Each month from [start day] to [end day]'. Ex: Each month from 7 to 8", itemNameMonthly, listItems.First());
            
            Step("13. Close the pop-up and select that 2 days again");
            schedulingManagerPage.CalendarEditorItemsPopupPanel.ClickCloseButton();
            schedulingManagerPage.WaitForPopupDialogDisappeared();
            schedulingManagerPage.CalendarEditorPanel.DragAndDropFromDateToDate(dateFrom, dateTo);
            schedulingManagerPage.WaitForPopupDialogDisplayed();

            Step("14. Select 'Weekly' option and another control program, then press Save");
            schedulingManagerPage.CalendarControlProgramsPopupPanel.ClickRadioWeeklyButton();
            var controlProgramWeekly = schedulingManagerPage.CalendarControlProgramsPopupPanel.SelectRandomItem(controlProgramYearly, controlProgramMonthly);
            var colorWeekly = schedulingManagerPage.CalendarControlProgramsPopupPanel.GetSelectedItemColor();
            schedulingManagerPage.CalendarControlProgramsPopupPanel.ClickSaveButton();
            schedulingManagerPage.WaitForPopupDialogDisappeared();
            var itemNameWeeklyDateFrom = string.Format("Each {0}", dateFrom.DayOfWeek);
            var itemNameWeeklyDateTo= string.Format("Each {0}", dateTo.DayOfWeek);

            Step("15. Verify The background color of all the days which have the same name of date (ex: Friday, Saturday) are replaced by the color of the newly selected control program.");
            var listSelectedDatesWeekly = Settings.GetAllDaysOfWeek(currentYear, dateFrom.DayOfWeek, dateTo.DayOfWeek);
            foreach (var dt in listSelectedDatesWeekly)
            {
                var date = dt.ToString("M/d/yyyy");
                Step("-> Verify " + date);
                var color = schedulingManagerPage.CalendarEditorPanel.GetCalendarDateColor(date);
                VerifyEqual(string.Format("15. Verify The '{0}' is set the color of Weekly Control Program", date), colorWeekly, color);
            }

            Step("16. Press Calendar Items button");
            schedulingManagerPage.CalendarEditorPanel.ClickCalendarItemsButton();
            schedulingManagerPage.WaitForPopupDialogDisplayed();

            Step("17. Verify There are 2 rows added at the 1st and 2nd position with");
            Step(" o The color of the newly selected control program");
            Step(" o The test 'Each [name of date]'. Ex: Each Saturday");
            Step(" o The test 'Each [name of date]'. Ex: Each Friday");
            listItems = schedulingManagerPage.CalendarEditorItemsPopupPanel.GetListOfItemsName();
            actualDateColor = schedulingManagerPage.CalendarEditorItemsPopupPanel.GetColorOfItem(itemNameWeeklyDateFrom);
            VerifyEqual("17. Verify There is 2 rows added to the list", 4, listItems.Count);
            VerifyEqual("17. Verify The color of the newly selected control program", colorWeekly, actualDateColor);
            VerifyEqual("17. Verify The test 'Each [name of date]'. Ex: Each Saturday", itemNameWeeklyDateTo, listItems[0]);
            VerifyEqual("17. Verify The test 'Each [name of date]'. Ex: Each Friday", itemNameWeeklyDateFrom, listItems[1]);
            
            Step("18. Close the pop-up and select that 2 days again");
            schedulingManagerPage.CalendarEditorItemsPopupPanel.ClickCloseButton();
            schedulingManagerPage.WaitForPopupDialogDisappeared();
            schedulingManagerPage.CalendarEditorPanel.DragAndDropFromDateToDate(dateFrom, dateTo);
            schedulingManagerPage.WaitForPopupDialogDisplayed();

            Step("19. Select 'None' option and another control program, then press Save");
            schedulingManagerPage.CalendarControlProgramsPopupPanel.ClickRadioNoneButton();
            var controlProgramNone = schedulingManagerPage.CalendarControlProgramsPopupPanel.SelectRandomItem(controlProgramYearly, controlProgramMonthly, controlProgramWeekly);
            var colorNone = schedulingManagerPage.CalendarControlProgramsPopupPanel.GetSelectedItemColor();
            schedulingManagerPage.CalendarControlProgramsPopupPanel.ClickSaveButton();
            schedulingManagerPage.WaitForPopupDialogDisappeared();
            var itemNameNone = string.Format("From {0}, {1} to {2}, {3}", dateFrom.DayOfWeek, dateFrom.ToString("MMMM dd, yyyy"), dateTo.DayOfWeek, dateTo.ToString("MMMM dd, yyyy"));

            Step("20. Verify The background color of the selected 2 days are replaced by the color of the newly selected control program");
            var dateStr = dateFrom.ToString("M/d/yyyy");
            var colorDate = schedulingManagerPage.CalendarEditorPanel.GetCalendarDateColor(dateStr);
            VerifyEqual(string.Format("20. Verify The '{0}' is set the color of None Control Program", dateStr), colorNone, colorDate);
            dateStr = dateTo.ToString("M/d/yyyy");
            colorDate = schedulingManagerPage.CalendarEditorPanel.GetCalendarDateColor(dateStr);
            VerifyEqual(string.Format("20. Verify The '{0}' is set the color of None Control Program", dateStr), colorNone, colorDate);

            Step("21. Press Calendar Items button");
            schedulingManagerPage.CalendarEditorPanel.ClickCalendarItemsButton();
            schedulingManagerPage.WaitForPopupDialogDisplayed();

            Step("22. Verify There is another row added at the 1st position with");
            Step(" o The color of the newly selected control program");
            Step(" o The test 'From [name of day], [name of month] [start day], [year] to [name of day], [name of month] [end day], [year]'. Ex: 'From Saturday, January 07, 2017 to Sunday, January 08, 2017'");
            listItems = schedulingManagerPage.CalendarEditorItemsPopupPanel.GetListOfItemsName();
            actualDateColor = schedulingManagerPage.CalendarEditorItemsPopupPanel.GetColorOfItem(itemNameNone);
            VerifyEqual("22. Verify There is another row added to the list", 5, listItems.Count);
            VerifyEqual("22. Verify The color of the newly selected control program", colorNone, actualDateColor);            
            VerifyEqual("22. The test 'From [name of day], [name of month] [start day], [year] to [name of day], [name of month] [end day], [year]'. Ex: 'From Saturday, January 07, 2017 to Sunday, January 08, 2017'", itemNameNone, listItems.First());

            try
            {
                DeleteCalendar(calendarName);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("SM_19 Calendar - Created automatically for a device")]
        public void SM_19()
        {
            var geozone = SLVHelper.GenerateUniqueName("GZNSM19");
            var controller = SLVHelper.GenerateUniqueName("CTRL");
            var streetlight = SLVHelper.GenerateUniqueName("STL");
            var newDimmingGroupName = SLVHelper.GenerateUniqueName("CSM19");

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");         

            Step("-> Create data for testing");
            DeleteGeozones("GZNSM19*");
            CreateNewGeozone(geozone);
            CreateNewController(controller, geozone);

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.EquipmentInventory, App.SchedulingManager);

            Step("1. Go to Equipment Inventory app");
            Step("2. Expected Equipment Inventory is routed and loaded successfully");
            var equipmentInventoryPage = desktopPage.GoToApp(App.EquipmentInventory) as EquipmentInventoryPage;

            Step("3. Create a streetlight with new dimming group");            
            equipmentInventoryPage.GeozoneTreeMainPanel.SelectNode(geozone);
            equipmentInventoryPage.CreateDevice(DeviceType.Streetlight, streetlight, controller, streetlight, "SSN Cimcon Dim Photocell[Lamp #0]");

            Step("4. Expected The creating is a success");
            var actualName = equipmentInventoryPage.StreetlightEditorPanel.GetNameValue();
            VerifyEqual(string.Format("4. Verify '{0}' is created successfully", streetlight), streetlight, actualName);
            equipmentInventoryPage.StreetlightEditorPanel.SelectDimmingGroupDropDown(newDimmingGroupName);
            equipmentInventoryPage.StreetlightEditorPanel.ClickSaveButton();
            equipmentInventoryPage.WaitForPreviousActionComplete();
            equipmentInventoryPage.WaitForEditorPanelDisappeared();

            Step("5. Go to Scheduling Manager App");
            Step("6. Expected Scheduling Manager page is routed and loaded successfully");
            var schedulingManagerPage = equipmentInventoryPage.AppBar.SwitchTo(App.SchedulingManager) as SchedulingManagerPage;

            Step("7. Select 'Calendar' tab");
            schedulingManagerPage.SchedulingManagerPanel.SelectTab("Calendar");
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("8. Expected The new dimming group in step #3 is present in calendar grid");
            VerifyEqual("8. Verify The new dimming group in step #3 is present in calendar grid", true, schedulingManagerPage.SchedulingManagerPanel.IsCalendarPresentInGrid(newDimmingGroupName));

            Step("9. Select the dimming group in the grid");
            schedulingManagerPage.SchedulingManagerPanel.SelectCalendar(newDimmingGroupName);
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("10. Expected Verify Calendar editor panel loads data for the selected item:");
            Step(" - Name");
            Step(" - Description: looks like \"Created automatically for device 'lamp02@smartsims' when its variable 'DimmingGroupName' was set to value 'SLV_2140_14380117' at\" where lamp02 is device id and smartsims is controller id");
            Step(" - A label below description field has text '1 devices are using this calendar'");
            var descriptionPattern = string.Format("Created automatically for device '{0}@{1}' when its variable 'DimmingGroupName'.*", streetlight, controller);
            var name = schedulingManagerPage.CalendarEditorPanel.GetNameValue();
            var description = schedulingManagerPage.CalendarEditorPanel.GetDescriptionValue();
            var devicesUsedLabel = schedulingManagerPage.CalendarEditorPanel.GetDevicesCountText();
            VerifyEqual(string.Format("10. Verify Name is '{0}'", newDimmingGroupName), newDimmingGroupName, name);
            VerifyEqual("10. Verify Description matched pattern", true, Regex.IsMatch(description, descriptionPattern));
            VerifyEqual("10. Verify A label below description field has text '1 devices are using this calendar'", "1 devices are using this calendar", devicesUsedLabel);

            try
            {
                DeleteGeozone(geozone);
                DeleteCalendar(newDimmingGroupName);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("SM_20_01 - Control program - Create a Control Program with template: 'Astro ON/OFF' and Daytime Photocell Override")]
        public void SM_20_01()
        {
            var template = "Astro ON/OFF";
            var newName = SLVHelper.GenerateUniqueName("CPSM2001");
            var description = newName + "-Description";

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.SchedulingManager);

            Step("1. Go to Scheduling Manager app");
            Step("2. Expected Scheduling Manager is routed and loaded successfully");
            var schedulingManagerPage = desktopPage.GoToApp(App.SchedulingManager) as SchedulingManagerPage;

            Step("3. Select 'Control program' tab in left panel");
            schedulingManagerPage.SchedulingManagerPanel.SelectTab("Control program");
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("4. Press 'Add New' button to add a new Control Program");
            schedulingManagerPage.SchedulingManagerPanel.ClickAddControlProgramButton();
            schedulingManagerPage.WaitForPreviousActionComplete();
                       
            Step("5. Enter the name and description and select the template is 'Astro ON/OFF'");
            schedulingManagerPage.ControlProgramEditorPanel.EnterNameInput(newName);
            schedulingManagerPage.ControlProgramEditorPanel.EnterDescriptionInput(description);
            schedulingManagerPage.ControlProgramEditorPanel.SelectTemplateDropDown(template);

            Step("6. Verify There is a section with");
            Step(" o Title: Options");
            Step(" o A checkbox named 'Daytime Photocell Override'");
            Step(" o An icon I following after the checkbox");
            VerifyEqual("6. Verify There is a section with Title: Options", "Options", schedulingManagerPage.ControlProgramEditorPanel.GetOptionsText());
            VerifyEqual("6. Verify A checkbox named 'Daytime Photocell Override", true, schedulingManagerPage.ControlProgramEditorPanel.IsOptionsPhotocellEnableDisplayed());
            VerifyEqual("6. Verify An icon I following after the checkbox", true, schedulingManagerPage.ControlProgramEditorPanel.IsOptionsHelperIconDisplayed());

            Step("7. Press the I icon");
            schedulingManagerPage.ControlProgramEditorPanel.ClickOptionsPhotocellEnableInformationButton();
            schedulingManagerPage.WaitForPopupDialogDisplayed();

            Step("8. Verify A new pop-up displays with");
            Step(" o Title: Daytime Photocell Override Help");
            Step(" o Descirption:");
            Step("  + Daytime Photocell Override mode switches to photocell control when the lamp is scheduled to be off. This enables the lamp to be illuminated when dark conditions occur.");
            Step("  + When Daytime Photocell Override mode is active, lamp brightness is set to the last level specified in the variation section. If no variations are specified, lamp brightness is set to the brightness value in the Switch ON section.");
            Step("  + Photocell sensitivity can be specified on a per lamp basis using On Lux Level and Off Lux Level settings in Inventory Management.");            
            var expectedLine1 = "Daytime Photocell Override mode switches to photocell control when the lamp is scheduled to be off. This enables the lamp to be illuminated when dark conditions occur.";
            var expectedLine2 = "When Daytime Photocell Override mode is active, lamp brightness is set to the last level specified in the variation section. If no variations are specified, lamp brightness is set to the brightness value in the Switch ON section.";
            var expectedLine3 = "Photocell sensitivity can be specified on a per lamp basis using On Lux Level and Off Lux Level settings in Inventory Management.";
            var informationList = schedulingManagerPage.Dialog.GetListOfDaytimePhotocellInformation();
            VerifyEqual("8. Verify A new pop-up displays with Title: Daytime Photocell Override Help", "Daytime Photocell Override Help", schedulingManagerPage.Dialog.GetDialogTitleText());
            VerifyEqual("8. Verify Descirption Line 1 is " + expectedLine1, expectedLine1, informationList[0]);
            VerifyEqual("8. Verify Descirption Line 2 is " + expectedLine2, expectedLine2, informationList[1]);
            VerifyEqual("8. Verify Descirption Line 3 is " + expectedLine3, expectedLine3, informationList[2]);
            
            Step("9. Press X to close the pop-up");
            schedulingManagerPage.Dialog.ClickCloseButton();
            schedulingManagerPage.WaitForPopupDialogDisappeared();

            Step("10. Verify The pop-up is closed");
            VerifyEqual("10. Verify The pop-up is closed", false, schedulingManagerPage.IsPopupDialogDisplayed());

            Step("11. Input random values of minutes texboxes for Switch ON and Switch OFF sections");
            var switchOnMinute = SLVHelper.GenerateStringInteger(1, 59);
            var switchOnRelation = "Before";
            var switchOffMinute = SLVHelper.GenerateStringInteger(1, 59);
            var switchOffRelation = "Before";
            schedulingManagerPage.ControlProgramEditorPanel.EnterSwitchOnMinuteInput(switchOnMinute);
            schedulingManagerPage.ControlProgramEditorPanel.EnterSwitchOffMinuteInput(switchOffMinute);

            Step("12. Change the value Before to After and After to Before for Switch ON and Switch OFF sections");
            schedulingManagerPage.ControlProgramEditorPanel.SelectSwitchOnRelationDropDown(switchOnRelation);
            schedulingManagerPage.ControlProgramEditorPanel.SelectSwitchOffRelationDropDown(switchOffRelation);

            Step("13. Check the option 'Daytime Photocell Override'");
            schedulingManagerPage.ControlProgramEditorPanel.TickOptionsPhotocellEnableCheckbox(true);

            Step("14. Verify The option is checked");
            VerifyEqual("14. Verify The option is checked", true, schedulingManagerPage.ControlProgramEditorPanel.GetOptionsPhotocellEnableValue());

            Step("15. Press Save button, select another existing Control Program in the list, then select the newly created one");
            schedulingManagerPage.ControlProgramEditorPanel.ClickSaveButton();
            schedulingManagerPage.WaitForPreviousActionComplete();
            var listControlPrograms = schedulingManagerPage.SchedulingManagerPanel.GetListOfControlProgramName();
            listControlPrograms.Remove(newName);            
            var randomControlProgram = listControlPrograms.PickRandom();
            schedulingManagerPage.SchedulingManagerPanel.SelectControlProgram(randomControlProgram);
            schedulingManagerPage.WaitForPreviousActionComplete();
            schedulingManagerPage.SchedulingManagerPanel.SelectControlProgram(newName);
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("16. Verify The control program is created correctly.");
            Step(" o Name, Descirption, Template, Minutes are saved");
            Step(" o Before and After values are saved");
            Step(" o Daytime Photocell Override is checked.");
            VerifyEqual("16. Verify The control program Name is created correctly", newName, schedulingManagerPage.ControlProgramEditorPanel.GetNameValue());
            VerifyEqual("16. Verify The control program Descirption is created correctly", description, schedulingManagerPage.ControlProgramEditorPanel.GetDescriptionValue());
            VerifyEqual("16. Verify The control program Template is created correctly", template, schedulingManagerPage.ControlProgramEditorPanel.GetTemplateValue());
            VerifyEqual("16. Verify The control program Switch On Minutes is created correctly", switchOnMinute, schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOnMinuteValue());
            VerifyEqual("16. Verify The control program Switch On Relation is created correctly", switchOnRelation, schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOnRelationValue());
            VerifyEqual("16. Verify The control program Switch Off Minutes is created correctly", switchOffMinute, schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOffMinuteValue());
            VerifyEqual("16. Verify The control program Switch Off Relation is created correctly", switchOffRelation, schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOffRelationValue());
            VerifyEqual("16. Verify Daytime Photocell Override is checked", true, schedulingManagerPage.ControlProgramEditorPanel.GetOptionsPhotocellEnableValue());
            
            Step("17. Update the name, dscription and minutes, Before and After of Switch ON and Switch OFF value, and deselect the option 'Daytime Photocell Override' and press Save button.");
            var updatedName = newName + "-Updated1";
            var updatedDescription = description + "-Updated1";
            var updatedSwitchOnMinute = SLVHelper.GenerateStringInteger(1, 59);
            var updatedSwitchOnRelation = "After";
            var updatedSwitchOffMinute = SLVHelper.GenerateStringInteger(1, 59);
            var updatedSwitchOffRelation = "After";
            schedulingManagerPage.ControlProgramEditorPanel.EnterNameInput(updatedName);
            schedulingManagerPage.ControlProgramEditorPanel.EnterDescriptionInput(updatedDescription);
            schedulingManagerPage.ControlProgramEditorPanel.EnterSwitchOnMinuteInput(updatedSwitchOnMinute);
            schedulingManagerPage.ControlProgramEditorPanel.EnterSwitchOffMinuteInput(updatedSwitchOffMinute);
            schedulingManagerPage.ControlProgramEditorPanel.SelectSwitchOnRelationDropDown(updatedSwitchOnRelation);
            schedulingManagerPage.ControlProgramEditorPanel.SelectSwitchOffRelationDropDown(updatedSwitchOffRelation);
            schedulingManagerPage.ControlProgramEditorPanel.TickOptionsPhotocellEnableCheckbox(false);
            schedulingManagerPage.ControlProgramEditorPanel.ClickSaveButton();
            schedulingManagerPage.WaitForPreviousActionComplete();           

            Step("18. Select another existing Control Program in the list, then select the newly updated one");
            schedulingManagerPage.SchedulingManagerPanel.SelectControlProgram(randomControlProgram);
            schedulingManagerPage.WaitForPreviousActionComplete();
            schedulingManagerPage.SchedulingManagerPanel.SelectControlProgram(updatedName);
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("19. Verify The control program is updated correctly");
            Step(" o Name, Description and minutes are updated");
            Step(" o Before and After values are saved");
            Step(" o Daytime Photocell Override is unchecked.");
            VerifyEqual("19. Verify The control program Name is updated correctly", updatedName, schedulingManagerPage.ControlProgramEditorPanel.GetNameValue());
            VerifyEqual("19. Verify The control program Descirption is updated correctly", updatedDescription, schedulingManagerPage.ControlProgramEditorPanel.GetDescriptionValue());            
            VerifyEqual("19. Verify The control program Switch On Minutes is updated correctly", updatedSwitchOnMinute, schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOnMinuteValue());
            VerifyEqual("19. Verify The control program Switch On Relation is updated correctly", updatedSwitchOnRelation, schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOnRelationValue());
            VerifyEqual("19. Verify The control program Switch Off Minutes is updated correctly", updatedSwitchOffMinute, schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOffMinuteValue());
            VerifyEqual("19. Verify The control program Switch Off Relation is updated correctly", updatedSwitchOffRelation, schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOffRelationValue());
            VerifyEqual("19. Verify Daytime Photocell Override is unchecked", false, schedulingManagerPage.ControlProgramEditorPanel.GetOptionsPhotocellEnableValue());
            
            Step("20. Update the name, description and minutes, Before and After of Switch ON and Switch OFF value, and select the option 'Daytime Photocell Override' and press Save button.");
            updatedName = newName + "-Updated2";
            updatedDescription = description + "-Updated2";
            updatedSwitchOnMinute = SLVHelper.GenerateStringInteger(1, 59);
            updatedSwitchOnRelation = "Before";
            updatedSwitchOffMinute = SLVHelper.GenerateStringInteger(1, 59);
            updatedSwitchOffRelation = "Before";
            schedulingManagerPage.ControlProgramEditorPanel.EnterNameInput(updatedName);
            schedulingManagerPage.ControlProgramEditorPanel.EnterDescriptionInput(updatedDescription);
            schedulingManagerPage.ControlProgramEditorPanel.EnterSwitchOnMinuteInput(updatedSwitchOnMinute);
            schedulingManagerPage.ControlProgramEditorPanel.EnterSwitchOffMinuteInput(updatedSwitchOffMinute);
            schedulingManagerPage.ControlProgramEditorPanel.SelectSwitchOnRelationDropDown(updatedSwitchOnRelation);
            schedulingManagerPage.ControlProgramEditorPanel.SelectSwitchOffRelationDropDown(updatedSwitchOffRelation);
            schedulingManagerPage.ControlProgramEditorPanel.TickOptionsPhotocellEnableCheckbox(true);
            schedulingManagerPage.ControlProgramEditorPanel.ClickSaveButton();
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("21. select another existing Control Program in the list, then select the newly updated one");
            schedulingManagerPage.SchedulingManagerPanel.SelectControlProgram(randomControlProgram);
            schedulingManagerPage.WaitForPreviousActionComplete();
            schedulingManagerPage.SchedulingManagerPanel.SelectControlProgram(updatedName);
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("22. Verify The control program is updated correctly");
            Step(" o Name, Description and minutes are updated");
            Step(" o Before and After values are saved");
            Step(" o Daytime Photocell Override is checked");
            VerifyEqual("22. Verify The control program Name is updated correctly", updatedName, schedulingManagerPage.ControlProgramEditorPanel.GetNameValue());
            VerifyEqual("22. Verify The control program Descirption is updated correctly", updatedDescription, schedulingManagerPage.ControlProgramEditorPanel.GetDescriptionValue());
            VerifyEqual("22. Verify The control program Switch On Minutes is updated correctly", updatedSwitchOnMinute, schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOnMinuteValue());
            VerifyEqual("22. Verify The control program Switch On Relation is updated correctly", updatedSwitchOnRelation, schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOnRelationValue());
            VerifyEqual("22. Verify The control program Switch Off Minutes is updated correctly", updatedSwitchOffMinute, schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOffMinuteValue());
            VerifyEqual("22. Verify The control program Switch Off Relation is updated correctly", updatedSwitchOffRelation, schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOffRelationValue());
            VerifyEqual("22. Verify Daytime Photocell Override is checked", true, schedulingManagerPage.ControlProgramEditorPanel.GetOptionsPhotocellEnableValue());
            
            try
            {
                DeleteControlProgram(updatedName);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("SM_20_02 - Control program - Create a Control Program with template: 'Astro ON/OFF and fixed time events' and Daytime Photocell Override")]
        public void SM_20_02()
        {
            var template = "Astro ON/OFF and fixed time events";
            var newName = SLVHelper.GenerateUniqueName("CPSM2002");
            var description = newName + "-Description";

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.SchedulingManager);

            Step("1. Go to Scheduling Manager app");
            Step("2. Expected Scheduling Manager is routed and loaded successfully");
            var schedulingManagerPage = desktopPage.GoToApp(App.SchedulingManager) as SchedulingManagerPage;

            Step("3. Select 'Control program' tab in left panel");
            schedulingManagerPage.SchedulingManagerPanel.SelectTab("Control program");
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("4. Press 'Add New' button to add a new Control Program");
            schedulingManagerPage.SchedulingManagerPanel.ClickAddControlProgramButton();
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("5. Enter the name and description and select the template is 'Astro ON/OFF and fixed time events'");
            schedulingManagerPage.ControlProgramEditorPanel.EnterNameInput(newName);
            schedulingManagerPage.ControlProgramEditorPanel.EnterDescriptionInput(description);
            schedulingManagerPage.ControlProgramEditorPanel.SelectTemplateDropDown(template);

            Step("6. Verify There is a section with");
            Step(" o Title: Options");
            Step(" o A checkbox named 'Daytime Photocell Override'");
            Step(" o An icon I following after the checkbox");
            VerifyEqual("6. Verify There is a section with Title: Options", "Options", schedulingManagerPage.ControlProgramEditorPanel.GetOptionsText());
            VerifyEqual("6. Verify A checkbox named 'Daytime Photocell Override", true, schedulingManagerPage.ControlProgramEditorPanel.IsOptionsPhotocellEnableDisplayed());
            VerifyEqual("6. Verify An icon I following after the checkbox", true, schedulingManagerPage.ControlProgramEditorPanel.IsOptionsHelperIconDisplayed());

            Step("7. Press the I icon");
            schedulingManagerPage.ControlProgramEditorPanel.ClickOptionsPhotocellEnableInformationButton();
            schedulingManagerPage.WaitForPopupDialogDisplayed();

            Step("8. Verify A new pop-up displays with");
            Step(" o Title: Daytime Photocell Override Help");
            Step(" o Descirption:");
            Step("  + Daytime Photocell Override mode switches to photocell control when the lamp is scheduled to be off. This enables the lamp to be illuminated when dark conditions occur.");
            Step("  + When Daytime Photocell Override mode is active, lamp brightness is set to the last level specified in the variation section. If no variations are specified, lamp brightness is set to the brightness value in the Switch ON section.");
            Step("  + Photocell sensitivity can be specified on a per lamp basis using On Lux Level and Off Lux Level settings in Inventory Management.");
            var expectedLine1 = "Daytime Photocell Override mode switches to photocell control when the lamp is scheduled to be off. This enables the lamp to be illuminated when dark conditions occur.";
            var expectedLine2 = "When Daytime Photocell Override mode is active, lamp brightness is set to the last level specified in the variation section. If no variations are specified, lamp brightness is set to the brightness value in the Switch ON section.";
            var expectedLine3 = "Photocell sensitivity can be specified on a per lamp basis using On Lux Level and Off Lux Level settings in Inventory Management.";
            var informationList = schedulingManagerPage.Dialog.GetListOfDaytimePhotocellInformation();
            VerifyEqual("8. Verify A new pop-up displays with Title: Daytime Photocell Override Help", "Daytime Photocell Override Help", schedulingManagerPage.Dialog.GetDialogTitleText());
            VerifyEqual("8. Verify Descirption Line 1 is " + expectedLine1, expectedLine1, informationList[0]);
            VerifyEqual("8. Verify Descirption Line 2 is " + expectedLine2, expectedLine2, informationList[1]);
            VerifyEqual("8. Verify Descirption Line 3 is " + expectedLine3, expectedLine3, informationList[2]);

            Step("9. Press X to close the pop-up");
            schedulingManagerPage.Dialog.ClickCloseButton();
            schedulingManagerPage.WaitForPopupDialogDisappeared();

            Step("10. Verify The pop-up is closed");
            VerifyEqual("10. Verify The pop-up is closed", false, schedulingManagerPage.IsPopupDialogDisplayed());

            Step("11. Input random values of minutes texboxes for Switch ON and Switch OFF sections");
            var switchOnMinute = SLVHelper.GenerateStringInteger(1, 59);
            var switchOnRelation = "Before";
            var switchOffMinute = SLVHelper.GenerateStringInteger(1, 59);
            var switchOffRelation = "Before";
            var variationsTime1 = string.Format("{0:D2}:00", SLVHelper.GenerateInteger(1, 12));
            var variationsLevel1 = SLVHelper.GenerateStringInteger(1, 99);
            var variationsTime2 = string.Format("{0:D2}:00", SLVHelper.GenerateInteger(13, 23));
            var variationsLevel2 = SLVHelper.GenerateStringInteger(1, 99);

            schedulingManagerPage.ControlProgramEditorPanel.EnterSwitchOnMinuteInput(switchOnMinute);
            schedulingManagerPage.ControlProgramEditorPanel.EnterSwitchOffMinuteInput(switchOffMinute);

            Step("12. Change the value Before to After and After to Before for Switch ON and Switch OFF sections");
            schedulingManagerPage.ControlProgramEditorPanel.SelectSwitchOnRelationDropDown(switchOnRelation);
            schedulingManagerPage.ControlProgramEditorPanel.SelectSwitchOffRelationDropDown(switchOffRelation);

            Step("13. Change the values of Time and Percent in Variations section");
            schedulingManagerPage.ControlProgramEditorPanel.EnterVariationTimeInput(0, variationsTime1);
            schedulingManagerPage.ControlProgramEditorPanel.EnterVariationLevelInput(0, variationsLevel1);
            schedulingManagerPage.ControlProgramEditorPanel.EnterVariationTimeInput(1, variationsTime2);
            schedulingManagerPage.ControlProgramEditorPanel.EnterVariationLevelInput(1, variationsLevel2);

            Step("14. Check the option 'Daytime Photocell Override'");
            schedulingManagerPage.ControlProgramEditorPanel.TickOptionsPhotocellEnableCheckbox(true);
            var variationsList = schedulingManagerPage.ControlProgramEditorPanel.GetListOfVariationsTimeAndLevelInput();

            Step("15. Verify The option is checked");
            VerifyEqual("15. Verify The option is checked", true, schedulingManagerPage.ControlProgramEditorPanel.GetOptionsPhotocellEnableValue());

            Step("16. Press Save button, select another existing Control Program in the list, then select the newly created one");
            schedulingManagerPage.ControlProgramEditorPanel.ClickSaveButton();
            schedulingManagerPage.WaitForPreviousActionComplete();
            var listControlPrograms = schedulingManagerPage.SchedulingManagerPanel.GetListOfControlProgramName();
            listControlPrograms.Remove(newName);
            var randomControlProgram = listControlPrograms.PickRandom();
            schedulingManagerPage.SchedulingManagerPanel.SelectControlProgram(randomControlProgram);
            schedulingManagerPage.WaitForPreviousActionComplete();
            schedulingManagerPage.SchedulingManagerPanel.SelectControlProgram(newName);
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("17. Verify The control program is created correctly.");
            Step(" o Name, Descirption, Template, Minutes are saved");
            Step(" o Before and After values are saved");
            Step(" o Variation values are saved");
            Step(" o Daytime Photocell Override is checked.");
            VerifyEqual("17. Verify The control program Name is created correctly", newName, schedulingManagerPage.ControlProgramEditorPanel.GetNameValue());
            VerifyEqual("17. Verify The control program Descirption is created correctly", description, schedulingManagerPage.ControlProgramEditorPanel.GetDescriptionValue());
            VerifyEqual("17. Verify The control program Template is created correctly", template, schedulingManagerPage.ControlProgramEditorPanel.GetTemplateValue());
            VerifyEqual("17. Verify The control program Switch On Minutes is created correctly", switchOnMinute, schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOnMinuteValue());
            VerifyEqual("17. Verify The control program Switch On Relation is created correctly", switchOnRelation, schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOnRelationValue());
            VerifyEqual("17. Verify The control program Switch Off Minutes is created correctly", switchOffMinute, schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOffMinuteValue());
            VerifyEqual("17. Verify The control program Switch Off Relation is created correctly", switchOffRelation, schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOffRelationValue());            
            VerifyEqual("17. Verify The control program Variations Time and Level are created correctly", variationsList, schedulingManagerPage.ControlProgramEditorPanel.GetListOfVariationsTimeAndLevelInput(), false);
            VerifyEqual("17. Verify Daytime Photocell Override is checked", true, schedulingManagerPage.ControlProgramEditorPanel.GetOptionsPhotocellEnableValue());

            Step("18. Update the name, dscription and minutes, Before, After of Switch ON and Switch OFF value, and values of Variations section and deselect the option 'Daytime Photocell Override' and press Save button.");
            var updatedName = newName + "-Updated1";
            var updatedDescription = description + "-Updated1";
            var updatedSwitchOnMinute = SLVHelper.GenerateStringInteger(1, 59);
            var updatedSwitchOnRelation = "After";
            var updatedSwitchOffMinute = SLVHelper.GenerateStringInteger(1, 59);
            var updatedSwitchOffRelation = "After";
            var updatedVariationsTime1 = string.Format("{0:D2}:00", SLVHelper.GenerateInteger(1, 12));
            var updatedVariationsLevel1 = SLVHelper.GenerateStringInteger(1, 99);
            var updatedVariationsTime2 = string.Format("{0:D2}:00", SLVHelper.GenerateInteger(13, 23));
            var updatedVariationsLevel2 = SLVHelper.GenerateStringInteger(1, 99);

            schedulingManagerPage.ControlProgramEditorPanel.EnterNameInput(updatedName);
            schedulingManagerPage.ControlProgramEditorPanel.EnterDescriptionInput(updatedDescription);
            schedulingManagerPage.ControlProgramEditorPanel.EnterSwitchOnMinuteInput(updatedSwitchOnMinute);
            schedulingManagerPage.ControlProgramEditorPanel.EnterSwitchOffMinuteInput(updatedSwitchOffMinute);
            schedulingManagerPage.ControlProgramEditorPanel.SelectSwitchOnRelationDropDown(updatedSwitchOnRelation);
            schedulingManagerPage.ControlProgramEditorPanel.SelectSwitchOffRelationDropDown(updatedSwitchOffRelation);
            schedulingManagerPage.ControlProgramEditorPanel.EnterVariationTimeInput(0, updatedVariationsTime1);
            schedulingManagerPage.ControlProgramEditorPanel.EnterVariationLevelInput(0, updatedVariationsLevel1);
            schedulingManagerPage.ControlProgramEditorPanel.EnterVariationTimeInput(1, updatedVariationsTime2);
            schedulingManagerPage.ControlProgramEditorPanel.EnterVariationLevelInput(1, updatedVariationsLevel2);
            schedulingManagerPage.ControlProgramEditorPanel.TickOptionsPhotocellEnableCheckbox(false);
            var updatedVariationsList = schedulingManagerPage.ControlProgramEditorPanel.GetListOfVariationsTimeAndLevelInput();
            schedulingManagerPage.ControlProgramEditorPanel.ClickSaveButton();
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("19. Select another existing Control Program in the list, then select the newly updated one");
            schedulingManagerPage.SchedulingManagerPanel.SelectControlProgram(randomControlProgram);
            schedulingManagerPage.WaitForPreviousActionComplete();
            schedulingManagerPage.SchedulingManagerPanel.SelectControlProgram(updatedName);
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("20. Verify The control program is updated correctly");
            Step(" o Name, Description and minutes are updated");
            Step(" o Before and After values are saved");
            Step(" o Variation values are saved");
            Step(" o Daytime Photocell Override is unchecked.");
            VerifyEqual("20. Verify The control program Name is updated correctly", updatedName, schedulingManagerPage.ControlProgramEditorPanel.GetNameValue());
            VerifyEqual("20. Verify The control program Descirption is updated correctly", updatedDescription, schedulingManagerPage.ControlProgramEditorPanel.GetDescriptionValue());
            VerifyEqual("20. Verify The control program Switch On Minutes is updated correctly", updatedSwitchOnMinute, schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOnMinuteValue());
            VerifyEqual("20. Verify The control program Switch On Relation is updated correctly", updatedSwitchOnRelation, schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOnRelationValue());
            VerifyEqual("20. Verify The control program Switch Off Minutes is updated correctly", updatedSwitchOffMinute, schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOffMinuteValue());
            VerifyEqual("20. Verify The control program Switch Off Relation is updated correctly", updatedSwitchOffRelation, schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOffRelationValue());            
            VerifyEqual("20. Verify The control program Variations Time and Level are updated correctly", updatedVariationsList, schedulingManagerPage.ControlProgramEditorPanel.GetListOfVariationsTimeAndLevelInput(), false);
            VerifyEqual("20. Verify Daytime Photocell Override is unchecked", false, schedulingManagerPage.ControlProgramEditorPanel.GetOptionsPhotocellEnableValue());

            Step("21. Update the name, dscription and minutes, Before, After of Switch ON and Switch OFF value, and values of Variations section and select the option 'Daytime Photocell Override' and press Save button.");
            updatedName = newName + "-Updated2";
            updatedDescription = description + "-Updated2";
            updatedSwitchOnMinute = SLVHelper.GenerateStringInteger(1, 59);
            updatedSwitchOnRelation = "Before";
            updatedSwitchOffMinute = SLVHelper.GenerateStringInteger(1, 59);
            updatedSwitchOffRelation = "Before";
            updatedVariationsTime1 = string.Format("{0:D2}:00", SLVHelper.GenerateInteger(1, 12));
            updatedVariationsLevel1 = SLVHelper.GenerateStringInteger(1, 99);
            updatedVariationsTime2 = string.Format("{0:D2}:00", SLVHelper.GenerateInteger(13, 23));
            updatedVariationsLevel2 = SLVHelper.GenerateStringInteger(1, 99);
            schedulingManagerPage.ControlProgramEditorPanel.EnterNameInput(updatedName);
            schedulingManagerPage.ControlProgramEditorPanel.EnterDescriptionInput(updatedDescription);
            schedulingManagerPage.ControlProgramEditorPanel.EnterSwitchOnMinuteInput(updatedSwitchOnMinute);
            schedulingManagerPage.ControlProgramEditorPanel.EnterSwitchOffMinuteInput(updatedSwitchOffMinute);
            schedulingManagerPage.ControlProgramEditorPanel.SelectSwitchOnRelationDropDown(updatedSwitchOnRelation);
            schedulingManagerPage.ControlProgramEditorPanel.SelectSwitchOffRelationDropDown(updatedSwitchOffRelation);
            schedulingManagerPage.ControlProgramEditorPanel.EnterVariationTimeInput(0, updatedVariationsTime1);
            schedulingManagerPage.ControlProgramEditorPanel.EnterVariationLevelInput(0, updatedVariationsLevel1);
            schedulingManagerPage.ControlProgramEditorPanel.EnterVariationTimeInput(1, updatedVariationsTime2);
            schedulingManagerPage.ControlProgramEditorPanel.EnterVariationLevelInput(1, updatedVariationsLevel2);
            schedulingManagerPage.ControlProgramEditorPanel.TickOptionsPhotocellEnableCheckbox(true);
            updatedVariationsList = schedulingManagerPage.ControlProgramEditorPanel.GetListOfVariationsTimeAndLevelInput();
            schedulingManagerPage.ControlProgramEditorPanel.ClickSaveButton();
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("22. select another existing Control Program in the list, then select the newly updated one");
            schedulingManagerPage.SchedulingManagerPanel.SelectControlProgram(randomControlProgram);
            schedulingManagerPage.WaitForPreviousActionComplete();
            schedulingManagerPage.SchedulingManagerPanel.SelectControlProgram(updatedName);
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("23. Verify The control program is updated correctly");
            Step(" o Name, Description and minutes are updated");
            Step(" o Before and After values are saved");
            Step(" o Daytime Photocell Override is checked");
            VerifyEqual("23. Verify The control program Name is updated correctly", updatedName, schedulingManagerPage.ControlProgramEditorPanel.GetNameValue());
            VerifyEqual("23. Verify The control program Descirption is updated correctly", updatedDescription, schedulingManagerPage.ControlProgramEditorPanel.GetDescriptionValue());
            VerifyEqual("23. Verify The control program Switch On Minutes is updated correctly", updatedSwitchOnMinute, schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOnMinuteValue());
            VerifyEqual("23. Verify The control program Switch On Relation is updated correctly", updatedSwitchOnRelation, schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOnRelationValue());
            VerifyEqual("23. Verify The control program Switch Off Minutes is updated correctly", updatedSwitchOffMinute, schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOffMinuteValue());
            VerifyEqual("23. Verify The control program Switch Off Relation is updated correctly", updatedSwitchOffRelation, schedulingManagerPage.ControlProgramEditorPanel.GetSwitchOffRelationValue());            
            VerifyEqual("23. Verify The control program Variations Time and Level are updated correctly", updatedVariationsList, schedulingManagerPage.ControlProgramEditorPanel.GetListOfVariationsTimeAndLevelInput(), false);
            VerifyEqual("23. Verify Daytime Photocell Override is checked", true, schedulingManagerPage.ControlProgramEditorPanel.GetOptionsPhotocellEnableValue());

            try
            {                
                DeleteControlProgram(updatedName);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("SC-1287 - SC-1330 Scheduling Manager - Calendar Comm Failures tab is empty when there are more than 1000 failures")]
        public void SC_1287()
        {
            var testData = GetTestDataOfTestSC_1287();
            var calendar = testData["Calendar"];            
            var recordsPerPage = int.Parse(testData["RecordsPerPage"]);

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - Testing with preset calendar: CalFailure");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.SchedulingManager);
            var schedulingManagerPage = desktopPage.GoToApp(App.SchedulingManager) as SchedulingManagerPage;

            Step("1. Go to Scheduling Manager app and select the Failure tab and check only CalFailure calendar");
            schedulingManagerPage.SchedulingManagerPanel.SelectTab("Failures");
            schedulingManagerPage.WaitForPreviousActionComplete();
            schedulingManagerPage.SchedulingManagerPanel.TickAllCalendarFailuresCheckbox(false);
            schedulingManagerPage.GridPanel.WaitForDataReloaded();
            schedulingManagerPage.SchedulingManagerPanel.TickFailuresCalendarGridRecordCheckbox(calendar, true);
            schedulingManagerPage.GridPanel.WaitForDataReloaded();
            var totalFailures = schedulingManagerPage.SchedulingManagerPanel.GetFailuresDevicesCount(calendar);
            var pageNum = totalFailures / recordsPerPage + 1;

            Step("2. Verify In the paging area, there is:");
            Step(" - Text: 'Generated in # seconds'");
            Step(" - Text: '1-{total number per page} of {total number of failures} Records'.Ex: '1 - 200 of 249 Records'");
            var footerLeftText = schedulingManagerPage.GridPanel.GetFooterLeftText();
            var footerRightText = schedulingManagerPage.GridPanel.GetFooterRightText();
            var expectedFooterRightText = string.Format("1 - {0} of {1} Records", recordsPerPage, totalFailures);
            VerifyEqual("2. Verify In the paging area, Text: 'Generated in # seconds'", true, Regex.IsMatch(footerLeftText, @"Generated in (.*?) seconds"));            
            VerifyEqual("2. Verify In the paging area, Text: '1-{total number per page} of {total number of failures} Records", expectedFooterRightText, footerRightText);

            Step("3. Scroll to the last records of the list and select the last record");
            schedulingManagerPage.GridPanel.SelectFailuresGridLastRecords(1);

            Step("4. Verify User can scroll to see the list and the last record is checked.");
            var lastRecordCheckValue = schedulingManagerPage.GridPanel.GetFailuresGridLastRecordCheckboxValue();
            VerifyEqual("4. Verify User can scroll to see the list and the last record is checked", true, lastRecordCheckValue);

            Step("5. Press 'Next' button on the paging");
            schedulingManagerPage.GridPanel.ClickFooterPageNextButton();
            schedulingManagerPage.GridPanel.WaitForDataReloaded();

            Step("6. Verify The page is refreshed with");
            Step(" - Data of the 2nd page is displayed.");
            Step(" - The current page indicator is now 2.");
            Step(" - Text: '{the first record of page 2}-{total number per page} of {total number of failures} Records'");
            var pageIndex = schedulingManagerPage.GridPanel.GetFooterPageIndexValue();
            footerRightText = schedulingManagerPage.GridPanel.GetFooterRightText();
            expectedFooterRightText = string.Format("{0} - {1} of {1} Records", recordsPerPage + 1, totalFailures);
            VerifyEqual("6. Verify Data of the 2nd page is displayed and the current page indicator is now 2", "2", pageIndex);            
            VerifyEqual("6. Verify In the paging area, Text: '1-{total number per page} of {total number of failures} Records", expectedFooterRightText, footerRightText);

            Step("7. Scroll to the last records of the list and select 2 last records");
            schedulingManagerPage.GridPanel.SelectFailuresGridLastRecords(2);

            Step("8. Verify User can scroll to see the list and the 2 last records are checked");
            var lastRecordsCheckValue = schedulingManagerPage.GridPanel.GetFailuresGridLastRecordsCheckboxValue(2);
            VerifyEqual("8. Verify User can scroll to see the list and the 2 last records are checked", true, lastRecordsCheckValue.All(p => p == true));

            Step("9. Press 'First' button on the paging");
            schedulingManagerPage.GridPanel.ClickFooterPageFirstButton();
            schedulingManagerPage.GridPanel.WaitForDataReloaded();

            Step("10. Verify The page is refreshed with");
            Step(" - Data of the 1st page is displayed.");
            Step(" - The current page indicator is now 1.");
            Step(" - Text: '1-{total number per page} of {total number of failures} Records'");
            pageIndex = schedulingManagerPage.GridPanel.GetFooterPageIndexValue();
            footerRightText = schedulingManagerPage.GridPanel.GetFooterRightText();
            expectedFooterRightText = string.Format("1 - {0} of {1} Records", recordsPerPage, totalFailures);
            VerifyEqual("10. Verify Data of the 1st page is displayed and the current page indicator is now 1", "1", pageIndex);            
            VerifyEqual("10. Verify In the paging area, Text: '1-{total number per page} of {total number of failures} Records", expectedFooterRightText, footerRightText);
            
            Step("11. Press 'Last' button on the paging");
            schedulingManagerPage.GridPanel.ClickFooterPageLastButton();
            schedulingManagerPage.GridPanel.WaitForDataReloaded();

            Step("12. Verify The page is refreshed with");
            Step(" - Data of the last page is displayed.");
            Step(" - The current page indicator is now the last page.");
            Step(" - Text: '{the first record of the last page}-{total number of failures} of {total number of failures} Records'");            
            pageIndex = schedulingManagerPage.GridPanel.GetFooterPageIndexValue();
            footerRightText = schedulingManagerPage.GridPanel.GetFooterRightText();
            expectedFooterRightText = string.Format("{0} - {1} of {1} Records", (recordsPerPage * (pageNum - 1)) + 1, totalFailures);
            VerifyEqual("12. Verify Data of the last page is displayed and the current page indicator is now the last page", pageNum.ToString(), pageIndex);
            VerifyEqual("12. Verify In the paging area,  Text: '{the first record of the last page}-{total number per page} of {total number of failures} Records'", expectedFooterRightText, footerRightText);

            Step("13. Press 'Previous' button on the paging");
            schedulingManagerPage.GridPanel.ClickFooterPagePreviousButton();
            schedulingManagerPage.GridPanel.WaitForDataReloaded();

            Step("14. Verify The page is refreshed with");
            Step(" - Data of the previous page is displayed.");
            Step(" - The current page indicator is now the last page -1.");
            Step(" - Text: '{the first record of the previous page}-{total number per page} of {total number of failures} Records'");
            pageIndex = schedulingManagerPage.GridPanel.GetFooterPageIndexValue();
            footerRightText = schedulingManagerPage.GridPanel.GetFooterRightText();
            expectedFooterRightText = string.Format("{0} - {1} of {2} Records", (recordsPerPage * (pageNum - 2)) + 1, recordsPerPage, totalFailures);
            VerifyEqual("14. Verify Data of the previous page is displayed and the current page indicator is now the last page - 1", (pageNum - 1).ToString(), pageIndex);
            VerifyEqual("14. Verify In the paging area,  Text: '{the first record of the previous page}-{total number per page} of {total number of failures} Records'", expectedFooterRightText, footerRightText);

            Step("15. Put the invalid number (> page number) into the textbox in paging and press Enter");
            var invalidNumber = pageNum + 2;
            schedulingManagerPage.GridPanel.EnterFooterPageIndexInput(invalidNumber.ToString());
            schedulingManagerPage.GridPanel.WaitForDataReloaded();

            Step("16. Verify The page is refreshed with");
            Step(" - Data of the last page is displayed.");
            Step(" - The current page indicator is now the last page.");
            Step(" - Text: '{the first record of the last page}-{total number of failures} of {total number of failures} Records'");
            pageIndex = schedulingManagerPage.GridPanel.GetFooterPageIndexValue();
            footerRightText = schedulingManagerPage.GridPanel.GetFooterRightText();
            expectedFooterRightText = string.Format("{0} - {1} of {1} Records", (recordsPerPage * (pageNum - 1)) + 1, totalFailures);
            VerifyEqual("16. Verify Data of the last page is displayed and the current page indicator is now the last page", pageNum.ToString(), pageIndex);
            VerifyEqual("16. Verify In the paging area,  Text: '{the first record of the last page}-{total number per page} of {total number of failures} Records'", expectedFooterRightText, footerRightText);
            
            Step("17. Put the valid number into the textbox in paging and press Enter");
            var validNumber = 1;
            schedulingManagerPage.GridPanel.EnterFooterPageIndexInput(validNumber.ToString());
            schedulingManagerPage.GridPanel.WaitForDataReloaded();

            Step("18. Verify The page is refreshed with");
            Step(" - Data of the inputed page is displayed.");
            Step(" - The current page indicator is now the inputed number.");
            Step(" - Text: '{the first record of the inputed page}-{total number per page} of {total number of failures} Records'");
            pageIndex = schedulingManagerPage.GridPanel.GetFooterPageIndexValue();
            footerRightText = schedulingManagerPage.GridPanel.GetFooterRightText();
            expectedFooterRightText = string.Format("{0} - {1} of {2} Records", (recordsPerPage * (validNumber - 1)) + 1, recordsPerPage, totalFailures);
            VerifyEqual("18. Verify Data of the inputed page is displayed and the current page indicator is now the inputed number.", validNumber.ToString(), pageIndex);
            VerifyEqual("18. Verify In the paging area,  Text: '{the first record of the previous page}-{total number per page} of {total number of failures} Records'", expectedFooterRightText, footerRightText);

            Step("19. Unselect the testing calendar");
            schedulingManagerPage.SchedulingManagerPanel.TickFailuresCalendarGridRecordCheckbox(calendar, false);
            schedulingManagerPage.GridPanel.WaitForDataReloaded();

            Step("20. Verify The list is refreshed and empty");
            var dtGrid = schedulingManagerPage.GridPanel.BuildFailuresDataTable();
            VerifyEqual("20. Verify The list is refreshed and empty", true, dtGrid.Rows.Count == 0);
        }

        [Test, DynamicRetry]
        [Description("SM_21 SC-940 Commissionning a calendar without saving it first never completes")]
        public void SM_21()
        {
            var testData = GetTestDataOfTestSM21();
            var controllerId = testData["ControllerId"];
            var controllerName = testData["ControllerName"];
            var geozone = SLVHelper.GenerateUniqueName("GZNSM21");
            var streetlight1 = SLVHelper.GenerateUniqueName("STL01");
            var streetlight2 = SLVHelper.GenerateUniqueName("STL02");
            var calendarName1 = SLVHelper.GenerateUniqueName("CRM2101");
            var calendarName2 = SLVHelper.GenerateUniqueName("CRM2102");
            var typeOfEquipment = "ABEL-Vigilon A[Dimmable ballast]";

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step(" - Create a streelight using Controller: Smartsims commission and Calendar: SM-21-Calendar-01");
            Step(" - Create a streelight using Controller: Smartsims commission and Calendar: SM-21-Calendar-02");
            Step("**** Precondition ****\n");

            Step("-> Create data for testing");
            DeleteGeozones("GZNSM21*");
            CreateNewCalendar(calendarName1, string.Format("Automated calendar for {0}", streetlight1));
            CreateNewCalendar(calendarName2, string.Format("Automated calendar for {0}", streetlight2));
            CreateNewGeozone(geozone);
            CreateNewDevice(DeviceType.Streetlight, streetlight1, controllerId, geozone, typeOfEquipment);
            CreateNewDevice(DeviceType.Streetlight, streetlight2, controllerId, geozone, typeOfEquipment);            
            SetValueToDevice(controllerId, streetlight1, "DimmingGroupName", calendarName1, Settings.GetCurrentControlerDateTime(controllerId).AddMinutes(10));
            SetValueToDevice(controllerId, streetlight2, "DimmingGroupName", calendarName2, Settings.GetCurrentControlerDateTime(controllerId).AddMinutes(10));

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.SchedulingManager);

            Step("1. Go to Scheduling Manager app");
            Step("2. Expected Scheduling Manager page is routed and loaded successfully");
            var schedulingManagerPage = desktopPage.GoToApp(App.SchedulingManager) as SchedulingManagerPage;

            Step("3. Select Calendar tab and choose 'SM-21-Calendar-01'");
            schedulingManagerPage.SchedulingManagerPanel.SelectTab("Calendar");
            schedulingManagerPage.SchedulingManagerPanel.SelectCalendar(calendarName1);
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("4. Click on a day in a month and select a control program for it, then press Save button");
            var randomDate = schedulingManagerPage.CalendarEditorPanel.ClickRandomCalendarDate();
            schedulingManagerPage.WaitForPopupDialogDisplayed();
            var hasPopupDialogDisplayed = schedulingManagerPage.HasPopupDialogDisplayed();
            schedulingManagerPage.CalendarControlProgramsPopupPanel.SelectItem(schedulingManagerPage.CalendarControlProgramsPopupPanel.GetListOfItemsName().PickRandom());
            var notedColor = schedulingManagerPage.CalendarControlProgramsPopupPanel.GetSelectedItemColor();
            schedulingManagerPage.CalendarControlProgramsPopupPanel.ClickSaveButton();
            schedulingManagerPage.WaitForPopupDialogDisappeared();

            Step("5. Verify The day in that month is changed color of the selected control program");
            VerifyEqual("5. Verify The day in that month is changed color of the selected control program", notedColor, schedulingManagerPage.CalendarEditorPanel.GetCalendarDateColor(randomDate));
            
            Step("6. Click Commission button without saving any changes");
            schedulingManagerPage.SchedulingManagerPanel.ClickCommissioningCalendarButton();
            schedulingManagerPage.WaitForPopupDialogDisplayed();

            Step("7. Verify A comfirmation pop-up displays with");
            Step(" - Title: Confirmation");
            Step(" - Description: The control program has changed and will be saved. Do you want to continue ?");
            Step(" - Button: Yes, No");
            VerifyEqual("7. Verify Title: Confirmation", "Confirmation", schedulingManagerPage.Dialog.GetDialogTitleText());
            VerifyEqual("7. Verify Description: 'The control program has changed and will be saved. Do you want to continue ?'", "The control program has changed and will be saved. Do you want to continue ?", schedulingManagerPage.Dialog.GetMessageText());
            VerifyEqual("7. Verify Button: Yes displays", true, schedulingManagerPage.Dialog.IsYesButtonDisplayed());
            VerifyEqual("7. Verify Button: No displays", true, schedulingManagerPage.Dialog.IsNoButtonDisplayed());

            Step("8. Press 'Yes'");
            schedulingManagerPage.Dialog.ClickYesButton();
            schedulingManagerPage.WaitForPreviousActionComplete();
            schedulingManagerPage.WaitForPopupDialogDisplayed();

            Step("9. Expected The Commissioning pop-up displays with: Controller's Name");
            schedulingManagerPage.CommissionPopupPanel.WaitForGridContentDisplayed();
            var controllerList = schedulingManagerPage.CommissionPopupPanel.GetListOfControllers();
            VerifyTrue("9. Verify The Commissioning pop-up displays with: Controller's Name", controllerList.Contains(controllerName), controllerName, string.Join(",", controllerList));
            
            Step("10. Press Commission button and wait for the process completes.");
            schedulingManagerPage.CommissionPopupPanel.ClickCommissionButton();
            Wait.ForSeconds(5); //waiting for commissioning message popup 
            if (!schedulingManagerPage.Dialog.IsPopupMessageDisplayed())
            {
                Warning("[SC-940] - Commissionning a calendar without saving it first never completes");
                return;
            }

            Step("11. Expected The Successful pop-up displays with:");
            Step(" - Message: 'Calendar commissioning has been initiated. This process can take several minutes. You may check for calendar commissioning failures in the Failures tab.'");
            Step(" - Button: OK");
            VerifyEqual("11. Verify message is shown as expected", "Calendar commissioning has been initiated. This process can take several minutes. You may check for calendar commissioning failures in the Failures tab.", schedulingManagerPage.Dialog.GetMessageText());
            VerifyEqual("11. Button: OK displays", true, schedulingManagerPage.Dialog.IsOkButtonDisplayed());
            
            Step("12. Press OK button");
            schedulingManagerPage.Dialog.ClickOkButton();
            schedulingManagerPage.Dialog.WaitForPopupMessageDisappeared();

            Step("13. Expected The pop-up dissappears.");
            VerifyEqual("13. Verify The pop-up dissappears", false, schedulingManagerPage.Dialog.IsPopupMessageDisplayed());
                       
            Step("14. Close the Commissioning pop-up");
            schedulingManagerPage.CommissionPopupPanel.ClickCancelButton();
            schedulingManagerPage.WaitForPopupDialogDisappeared();

            Step("15. Click Commission button again on the same calendar");
            schedulingManagerPage.SchedulingManagerPanel.ClickCommissioningCalendarButton();
            schedulingManagerPage.WaitForPopupDialogDisplayed();

            Step("16. Expected The Commissioning pop-up displays with: Controller's Name");
            schedulingManagerPage.CommissionPopupPanel.WaitForGridContentDisplayed();
            controllerList = schedulingManagerPage.CommissionPopupPanel.GetListOfControllers();
            VerifyTrue("16. Verify The Commissioning pop-up displays with: Controller's Name", controllerList.Contains(controllerName), controllerName, string.Join(",", controllerList));
            
            Step("17. Press Commission button and wait for the process completes.");
            schedulingManagerPage.CommissionPopupPanel.ClickCommissionButton();
            schedulingManagerPage.Dialog.WaitForPopupMessageDisplayed();

            Step("18. Expected The Successful pop-up displays with:");
            Step(" - Message: 'Calendar commissioning has been initiated. This process can take several minutes. You may check for calendar commissioning failures in the Failures tab.'");
            Step(" - Button: OK");
            VerifyEqual("18. Verify message is shown as expected", "Calendar commissioning has been initiated. This process can take several minutes. You may check for calendar commissioning failures in the Failures tab.", schedulingManagerPage.Dialog.GetMessageText());
            VerifyEqual("18. Button: OK displays", true, schedulingManagerPage.Dialog.IsOkButtonDisplayed());

            Step("19. Press OK button");
            schedulingManagerPage.Dialog.ClickOkButton();
            schedulingManagerPage.Dialog.WaitForPopupMessageDisappeared();

            Step("20. Expected The pop-up dissappears.");
            VerifyEqual("20. Verify The pop-up dissappears", false, schedulingManagerPage.Dialog.IsPopupMessageDisplayed());
            schedulingManagerPage.CommissionPopupPanel.ClickCancelButton();
            schedulingManagerPage.WaitForPopupDialogDisappeared();

            Step("21. Select Calendar tab and choose 'SM-21-Calendar-02'");
            schedulingManagerPage.SchedulingManagerPanel.SelectCalendar(calendarName2);
            schedulingManagerPage.WaitForPreviousActionComplete();
            
            Step("22. Click Commission button without saving any changes");
            schedulingManagerPage.SchedulingManagerPanel.ClickCommissioningCalendarButton();
            schedulingManagerPage.WaitForPopupDialogDisplayed();

            Step("23. Expected The Commissioning pop-up displays with: Controller's Name");
            schedulingManagerPage.CommissionPopupPanel.WaitForGridContentDisplayed();
            controllerList = schedulingManagerPage.CommissionPopupPanel.GetListOfControllers();
            VerifyTrue("23. Verify The Commissioning pop-up displays with: Controller's Name", controllerList.Contains(controllerName), controllerName, string.Join(",", controllerList));
            
            Step("24. Press Commission button and wait for the process completes.");
            schedulingManagerPage.CommissionPopupPanel.ClickCommissionButton();
            schedulingManagerPage.Dialog.WaitForPopupMessageDisplayed();

            Step("25. Expected The Successful pop-up displays with:");
            Step(" - Message: 'Calendar commissioning has been initiated. This process can take several minutes. You may check for calendar commissioning failures in the Failures tab.'");
            Step(" - Button: OK");
            VerifyEqual("25. Verify message is shown as expected", "Calendar commissioning has been initiated. This process can take several minutes. You may check for calendar commissioning failures in the Failures tab.", schedulingManagerPage.Dialog.GetMessageText());
            VerifyEqual("25. Button: OK displays", true, schedulingManagerPage.Dialog.IsOkButtonDisplayed());
            
            Step("26. Press OK button");
            schedulingManagerPage.Dialog.ClickOkButton();
            schedulingManagerPage.Dialog.WaitForPopupMessageDisappeared();

            Step("27. Expected The pop-up dissappears.");
            VerifyEqual("27. Verify The pop-up dissappears", false, schedulingManagerPage.Dialog.IsPopupMessageDisplayed());

            try
            {
                //Remove testing data
                DeleteGeozone(geozone);
                DeleteCalendar(calendarName1);
                DeleteCalendar(calendarName2);
            }
            catch { }
        }

        [Test, DynamicRetry]
        [Description("SM_22 Set a new controll program for a calendar in Scheduling Manager")]
        public void SM_22()
        {
            var controlProgramName1 = SLVHelper.GenerateUniqueName("CPSM2201");
            var controlProgramName2 = SLVHelper.GenerateUniqueName("CPSM2202");
            var calendarName = SLVHelper.GenerateUniqueName("CSM22");            

            Step("**** Precondition ****");
            Step(" - User has logged in successfully");
            Step("**** Precondition ****\n");

            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.SchedulingManager);

            Step("1. Go to Scheduling Manager app");
            Step("2. Expected Scheduling Manager page is routed and loaded successfully");
            var schedulingManagerPage = desktopPage.GoToApp(App.SchedulingManager) as SchedulingManagerPage;
            
            Step("3. Press 'Add New' button on Control Program tab");
            schedulingManagerPage.SchedulingManagerPanel.SelectTab("Control program");
            schedulingManagerPage.SchedulingManagerPanel.ClickAddControlProgramButton();
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("4. Enter the Name, Description and select a temmplate for the control program, then press Save");
            schedulingManagerPage.ControlProgramEditorPanel.EnterNameInput(controlProgramName1);
            schedulingManagerPage.ControlProgramEditorPanel.EnterDescriptionInput(SLVHelper.GenerateUniqueName("Description1"));
            schedulingManagerPage.ControlProgramEditorPanel.SelectTemplateDropDown("Advanced mode");
            schedulingManagerPage.ControlProgramEditorPanel.ClickSaveButton();
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("5. Verify The new control program is in the Control Program list");
            var controlProgramList = schedulingManagerPage.SchedulingManagerPanel.GetListOfControlProgramName();
            VerifyEqual("5. Verify The new control program is in the Control Program list", true, controlProgramList.Contains(controlProgramName1));

            Step("6. Select Calendar tab and press 'Add New' button");
            schedulingManagerPage.SchedulingManagerPanel.SelectTab("Calendar");
            schedulingManagerPage.WaitForPreviousActionComplete();
            schedulingManagerPage.SchedulingManagerPanel.ClickAddCalendarButton();
            schedulingManagerPage.WaitForPreviousActionComplete();

            Step("7. Enter Name, Description for the Calendar, then select a day in a month randomly");
            schedulingManagerPage.CalendarEditorPanel.EnterNameInput(calendarName);
            schedulingManagerPage.CalendarEditorPanel.EnterDescriptionInput(SLVHelper.GenerateUniqueName("Any description"));
            schedulingManagerPage.CalendarEditorPanel.ClickSaveButton();
            schedulingManagerPage.WaitForPreviousActionComplete();
            var randomDate = schedulingManagerPage.CalendarEditorPanel.ClickRandomCalendarDate();
            schedulingManagerPage.WaitForPopupDialogDisplayed();       

            Step("8. Verify The Control Programs pop-up displays containing the testing Control Program");
            var controlProgramListPoup = schedulingManagerPage.CalendarControlProgramsPopupPanel.GetListOfItemsName();
            var isControlProgram1Existing = controlProgramListPoup.Contains(controlProgramName1);
            VerifyEqual(string.Format("[SC-1999] 8. Verify The Control Programs pop-up displays containing the testing Control Program '{0}'", controlProgramName1), true, isControlProgram1Existing);
            if (isControlProgram1Existing)
            {
                Step("9. Select the testing Control Program and press Save button on the pop-up");
                schedulingManagerPage.CalendarControlProgramsPopupPanel.SelectItem(controlProgramName1);
                var notedColor = schedulingManagerPage.CalendarControlProgramsPopupPanel.GetSelectedItemColor();
                schedulingManagerPage.CalendarControlProgramsPopupPanel.ClickSaveButton();
                schedulingManagerPage.WaitForPopupDialogDisappeared();

                Step("10. Verify The control program is set for the selected date");
                var colorDate = schedulingManagerPage.CalendarEditorPanel.GetCalendarDateColor(randomDate);
                VerifyEqual("10. Verify The control program is set for the selected date", notedColor, colorDate);

                Step("11. Select Control Program tab and Press 'Add New' button");
                schedulingManagerPage.SchedulingManagerPanel.SelectTab("Control program");
                schedulingManagerPage.WaitForPreviousActionComplete();
                schedulingManagerPage.SchedulingManagerPanel.ClickAddControlProgramButton();
                schedulingManagerPage.WaitForPreviousActionComplete();

                Step("12. Enter the Name, Description and select a temmplate for the control program, then press Save");
                schedulingManagerPage.ControlProgramEditorPanel.EnterNameInput(controlProgramName2);
                schedulingManagerPage.ControlProgramEditorPanel.EnterDescriptionInput(SLVHelper.GenerateUniqueName("Description2"));
                schedulingManagerPage.ControlProgramEditorPanel.SelectTemplateDropDown("Advanced mode");
                schedulingManagerPage.ControlProgramEditorPanel.ClickSaveButton();
                schedulingManagerPage.WaitForPreviousActionComplete();

                Step("13. Verify The new control program is in the Control Program list");
                controlProgramList = schedulingManagerPage.SchedulingManagerPanel.GetListOfControlProgramName();
                VerifyEqual("13. Verify The new control program is in the Control Program list", true, controlProgramList.Contains(controlProgramName2));

                Step("14. Select Calendar tab and choose the testing calendar");
                schedulingManagerPage.SchedulingManagerPanel.SelectTab("Calendar");
                schedulingManagerPage.WaitForPreviousActionComplete();
                schedulingManagerPage.SchedulingManagerPanel.SelectCalendar(calendarName);
                schedulingManagerPage.WaitForPreviousActionComplete();

                Step("15. Select a day in a month randomly");
                randomDate = schedulingManagerPage.CalendarEditorPanel.ClickRandomCalendarDate();
                schedulingManagerPage.WaitForPopupDialogDisplayed();

                Step("16. Verify The Control Programs pop-up displays containing the 2nd testing Control Program");
                controlProgramListPoup = schedulingManagerPage.CalendarControlProgramsPopupPanel.GetListOfItemsName();
                var isControlProgram2Existing = controlProgramListPoup.Contains(controlProgramName2);                
                VerifyEqual(string.Format("[SC-1999] 16. Verify The Control Programs pop-up displays containing the 2nd testing Control Program '{0}'", controlProgramName2), true, isControlProgram2Existing);
                if (isControlProgram2Existing)
                {
                    Step("17. Select the 2nd testing Control Program and press Save button on the pop-up");
                    schedulingManagerPage.CalendarControlProgramsPopupPanel.SelectItem(controlProgramName2);
                    notedColor = schedulingManagerPage.CalendarControlProgramsPopupPanel.GetSelectedItemColor();
                    schedulingManagerPage.CalendarControlProgramsPopupPanel.ClickSaveButton();
                    schedulingManagerPage.WaitForPopupDialogDisappeared();

                    Step("18. Verify The 2nd testing control program is set for the selected date");
                    colorDate = schedulingManagerPage.CalendarEditorPanel.GetCalendarDateColor(randomDate);
                    VerifyEqual("18. Verify The 2nd testing control program is set for the selected date", notedColor, colorDate);
                }
            }
            else
            {
                if(schedulingManagerPage.IsPopupDialogDisplayed())
                {
                    schedulingManagerPage.CalendarControlProgramsPopupPanel.ClickCancelButton();
                    schedulingManagerPage.WaitForPopupDialogDisappeared();
                }
            }

            try
            {
                DeleteCalendar(calendarName);
                DeleteControlProgram(controlProgramName1);
                DeleteControlProgram(controlProgramName2);
            }
            catch { }
        }

        #endregion //Test Cases

        #region Private methods        

        public string GetTooltipTimeWithRelation(string relation, int minute)
        {
            var sign = string.Empty;
            switch (relation)
            {
                case "After":
                    sign = "+";
                    break;
                case "Before":
                    sign = "-";
                    break;
            }
            if (minute == 0) sign = "-"; //always '-' when minute == 0

            return string.Format("{0} 00:{1}:00", sign, minute.ToString("d2"));
        }

        public string GetTooltipTimeVariation(string value)
        {
            var result = "";
            if (value.Contains("PM") || value.Contains("AM"))
            {
                var time = DateTime.ParseExact(value, "hh:mm tt", CultureInfo.CurrentCulture);
                var hour = time.Hour;
                hour = hour == 0 ? 12 : hour;
                hour = hour > 12 ? hour - 12 : hour;
                result = string.Format("{0}:{1:d2}:00 {2}", hour, time.Minute, time.ToString("tt", CultureInfo.InvariantCulture).ToUpper());
            }
            else
            {
                var time = DateTime.Parse(value);
                result = string.Format("{0}:{1:d2}:00", time.Hour, time.Minute);
            }

            return result;
        }

        public string GetTimeWithTT(string value)
        {
            var time = DateTime.ParseExact(value, "hh:mm", CultureInfo.CurrentCulture);
            var hour = time.Hour;
            hour = hour == 0 ? 12 : hour;
            hour = hour > 12 ? hour - 12 : hour;
            var result = string.Format("{0:d2}:{1:d2} {2}", hour, time.Minute, time.ToString("tt", CultureInfo.InvariantCulture).ToUpper());

            return result;
        }

        public List<string> GetTemplates()
        {
            var templates = new List<string>();
            templates.Add("Astro ON/OFF");
            templates.Add("Astro ON/OFF and fixed time events");
            templates.Add("Always ON");
            templates.Add("Always OFF");
            templates.Add("Day fixed time events");
            templates.Add("Advanced mode");
            return templates;
        }

        public void EnterBasicValues(SchedulingManagerPage page, string name, string description)
        {
            page.ControlProgramEditorPanel.EnterNameInput(name);
            page.ControlProgramEditorPanel.EnterDescriptionInput(description);
        }

        public void EnterSwitchOnGroupValues(SchedulingManagerPage page, string switchOnMinute = "", string switchOnRelation = "", string switchOnLevel = "", string switchOnTime = "")
        {
            if (!string.IsNullOrEmpty(switchOnMinute))
                page.ControlProgramEditorPanel.EnterSwitchOnMinuteInput(switchOnMinute);
            if (!string.IsNullOrEmpty(switchOnRelation))
                page.ControlProgramEditorPanel.SelectSwitchOnRelationDropDown(switchOnRelation);
            if (!string.IsNullOrEmpty(switchOnLevel))
                page.ControlProgramEditorPanel.EnterSwitchOnLevelInput(switchOnLevel);
            if (!string.IsNullOrEmpty(switchOnTime))
                page.ControlProgramEditorPanel.EnterSwitchOnTimeInput(switchOnTime);

            page.AppBar.ClickHeaderBartop();
        }

        public void EnterSwitchOffGroupValues(SchedulingManagerPage page, string switchOffMinute = "", string switchOffRelation = "", string switchOffTime = "")
        {
            if (!string.IsNullOrEmpty(switchOffMinute))
                page.ControlProgramEditorPanel.EnterSwitchOffMinuteInput(switchOffMinute);
            if (!string.IsNullOrEmpty(switchOffRelation))
                page.ControlProgramEditorPanel.SelectSwitchOffRelationDropDown(switchOffRelation);
            if (!string.IsNullOrEmpty(switchOffTime))
                page.ControlProgramEditorPanel.EnterSwitchOffTimeInput(switchOffTime);
            page.AppBar.ClickHeaderBartop();
        }

        public void EnterVariationsGroupValues(SchedulingManagerPage page, int variationIndex, string time, string level)
        {
            page.ControlProgramEditorPanel.EnterVariationTimeInput(variationIndex, time);
            page.ControlProgramEditorPanel.EnterVariationLevelInput(variationIndex, level);
            page.AppBar.ClickHeaderBartop();
        }

        #region Verify methods

        public void VerifyInitialSwithOnGroup(SchedulingManagerPage page)
        {
            VerifyEqual("Verify Minutes: numeric = 0", "0", page.ControlProgramEditorPanel.GetSwitchOnMinuteValue());
            VerifyEqual("Verify After/Before field: dropdown list = 'After'", "After", page.ControlProgramEditorPanel.GetSwitchOnRelationValue());
            VerifyEqual("Verify Sunset: readonly dropdown list = 'Sunset'", "Sunset", page.ControlProgramEditorPanel.GetSwitchOnSunEventValue());
            VerifyEqual("Verify Sunset: dropdown list is readonly", true, page.ControlProgramEditorPanel.IsSwitchOnSunEventsDropDownReadOnly());
            VerifyEqual("Verify Dimming level: percentage numeric = '100%'", "100%", page.ControlProgramEditorPanel.GetSwitchOnLevelValue());
        }

        public void VerifyInitialSwithOffGroup(SchedulingManagerPage page)
        {
            VerifyEqual("Verify Minutes: numeric = 0", "0", page.ControlProgramEditorPanel.GetSwitchOffMinuteValue());
            VerifyEqual("Verify After/Before field: dropdown list = 'After'", "After", page.ControlProgramEditorPanel.GetSwitchOffRelationValue());
            VerifyEqual("Verify Sunrise: readonly dropdown list = 'Sunrise'", "Sunrise", page.ControlProgramEditorPanel.GetSwitchOffSunEventValue());
            VerifyEqual("Verify Sunrise: dropdown list is readonly", true, page.ControlProgramEditorPanel.IsSwitchOffSunEventsDropDownReadOnly());
            VerifyEqual("Verify Dimming level: percentage numeric = '0%'", "0%", page.ControlProgramEditorPanel.GetSwitchOffLevelValue());
        }

        public void VerifyInitialVariationsGroup(SchedulingManagerPage page)
        {
            var variationsCount = page.ControlProgramEditorPanel.GetVariationsCount();
            VerifyEqual("Verify 2 items by default", 2, variationsCount);

            VerifyEqual("Verify 1st item: Time is 11:00 PM (23:00)", true, page.ControlProgramEditorPanel.GetFirstVariationTimeInputValue() == "11:00 PM" || page.ControlProgramEditorPanel.GetFirstVariationTimeInputValue() == "23:00");
            VerifyEqual("Verify 1st item: Level is 77%", "77%", page.ControlProgramEditorPanel.GetFirstVariationLevelInputValue());
            VerifyEqual("Verify 2nd item: Time is 05:00 AM (05:00)", true, page.ControlProgramEditorPanel.GetLastVariationTimeInputValue() == "05:00 AM" || page.ControlProgramEditorPanel.GetLastVariationTimeInputValue() == "05:00");
            VerifyEqual("Verify 2nd item: Level is 90%", "90%", page.ControlProgramEditorPanel.GetLastVariationLevelInputValue());
        }

        public void VerifyChartTooltip(SchedulingManagerPage page, string expectedLevel, string expectedTime)
        {
            var actualTime = page.ControlProgramEditorPanel.GetDotTooltipTime();
            var actualLevel = page.ControlProgramEditorPanel.GetDotTooltipLevel();
            VerifyEqual(string.Format("Verify The left with dimming level in percentage is '{0}'", expectedLevel), expectedLevel, actualLevel);
            VerifyEqual(string.Format("Verify The bottom reflects the minutes is '{0}'", expectedTime), expectedTime, actualTime);
        }

        public void VerifyChartVariationsTooltip(SchedulingManagerPage page)
        {
            var percentPattern = @"\d{1,}%";
            var timePattern = @"\d{1,}:\d{1,}:\d{1,}.*";
            var variationCount = page.ControlProgramEditorPanel.GetChartVariationDotsCount();
            for (int index = 0; index < variationCount; index++)
            {
                page.ControlProgramEditorPanel.MoveToVariationDot(index);
                var actualTime = page.ControlProgramEditorPanel.GetDotTooltipTime();
                var actualLevel = page.ControlProgramEditorPanel.GetDotTooltipLevel();
                VerifyEqual("Verify The left with dimming level in percentage (x%)", true, Regex.IsMatch(actualLevel, percentPattern));
                VerifyEqual("Verify The bottom reflects the time(xx:xx:xx AM/PM)", true, Regex.IsMatch(actualTime, timePattern));
                VerifyEqual("Verify There is no either '+' or '-'", true, actualTime.IndexOf('+') == -1 && actualTime.IndexOf('-') == -1);
            }
        }

        public void VerifyBasicValues(SchedulingManagerPage page, string name, string description, string template, Color? color = null)
        {
            VerifyEqual(string.Format("Verify Name is '{0}'", name), name, page.ControlProgramEditorPanel.GetNameValue());
            VerifyEqual(string.Format("Verify Description is '{0}'", description), description, page.ControlProgramEditorPanel.GetDescriptionValue());
            VerifyEqual(string.Format("Verify Template is '{0}'", template), template, page.ControlProgramEditorPanel.GetTemplateValue());
            if (color != null)
            {
                var c = (Color)color;
                VerifyEqual(string.Format("Verify Color is {0}", c.GetKnownName()), c, page.ControlProgramEditorPanel.GetChartColorValue());
            }
        }

        public void VerifySwitchOnGroupValues(SchedulingManagerPage page, string switchOnMinute = "", string switchOnRelation = "", string switchOnLevel = "", string switchOnTime = "")
        {
            if (!string.IsNullOrEmpty(switchOnMinute))
                VerifyEqual(string.Format("Verify Swith On - Minute is {0}", switchOnMinute), switchOnMinute, page.ControlProgramEditorPanel.GetSwitchOnMinuteValue());
            if (!string.IsNullOrEmpty(switchOnRelation))
                VerifyEqual(string.Format("Verify Swith On - Relation is {0}", switchOnRelation), switchOnRelation, page.ControlProgramEditorPanel.GetSwitchOnRelationValue());
            if (!string.IsNullOrEmpty(switchOnLevel))
                VerifyEqual(string.Format("Verify Swith On - Level is {0}", switchOnLevel), switchOnLevel, page.ControlProgramEditorPanel.GetSwitchOnLevelValue());
            if (!string.IsNullOrEmpty(switchOnTime))
            {
                var timeTT = GetTimeWithTT(switchOnTime);
                VerifyEqual(string.Format("Verify Swith On - Time is {0}({1})", switchOnTime, timeTT), true, page.ControlProgramEditorPanel.GetSwitchOnTimeValue() == switchOnTime || page.ControlProgramEditorPanel.GetSwitchOnTimeValue() == timeTT);
            }
        }

        public void VerifySwitchOffGroupValues(SchedulingManagerPage page, string switchOffMinute = "", string switchOffRelation = "", string switchOffTime = "")
        {
            if (!string.IsNullOrEmpty(switchOffMinute))
                VerifyEqual(string.Format("Verify Swith Off - Minute is {0}", switchOffMinute), switchOffMinute, page.ControlProgramEditorPanel.GetSwitchOffMinuteValue());
            if (!string.IsNullOrEmpty(switchOffRelation))
                VerifyEqual(string.Format("Verify Swith Off - Relation is {0}", switchOffRelation), switchOffRelation, page.ControlProgramEditorPanel.GetSwitchOffRelationValue());
            if (!string.IsNullOrEmpty(switchOffTime))
            {
                var timeTT = GetTimeWithTT(switchOffTime);
                VerifyEqual(string.Format("Verify Swith On - Time is {0}({1})", switchOffTime, timeTT), true, page.ControlProgramEditorPanel.GetSwitchOffTimeValue() == switchOffTime || page.ControlProgramEditorPanel.GetSwitchOffTimeValue() == timeTT);
            }
        }

        public void VerifyVariationsGroupValues(SchedulingManagerPage page, int variationIndex, string time, string level)
        {
            DateTime t1 = Convert.ToDateTime(time);
            DateTime t2 = Convert.ToDateTime(page.ControlProgramEditorPanel.GetVariationTimeInputValue(variationIndex));
            VerifyEqual(string.Format("Verify Variation {0} - Time is '{1}'", variationIndex + 1, time), t1, t2);
            VerifyEqual(string.Format("Verify Variation {0} - Level is '{1}%'", variationIndex + 1, level), string.Format("{0}%", level), page.ControlProgramEditorPanel.GetVariationLevelInputValue(variationIndex));
        }

        public void VerifyVariationsGroupValues(SchedulingManagerPage page, List<string> variationsList)
        {
            var actualVariationsList = page.ControlProgramEditorPanel.GetListOfVariationsTimeAndLevelInput();
            VerifyEqual("Verify Variations time and dimming level are the same", variationsList, actualVariationsList, false);
        }

        public void VerifyChart(SchedulingManagerPage page, byte[] chartBytes)
        {
            var actualChartBytes = page.ControlProgramEditorPanel.GetBytesOfChart();
            var result = ImageUtility.Compare(chartBytes, actualChartBytes);
            VerifyEqual("Verify Chart is the same as expected", true, result == 0);
        }

        public void VerifyTimelineIcon(SchedulingManagerPage page, byte[] timelineIconBytes)
        {
            var actualTimelineIconBytes = page.ControlProgramEditorPanel.GetTimelineIconBytes();
            var result = ImageUtility.Compare(timelineIconBytes, actualTimelineIconBytes);
            VerifyEqual("Verify Timeline icon is the same as expected", true, result == 0);
        }

        public void VerifyControlProgramTable(SchedulingManagerPage page, List<string> dotsTimeAndLevelList)
        {
            var dotsTimeList = dotsTimeAndLevelList.Select(p => p.Split('|')[0]).ToList();
            var dotsLevelList = dotsTimeAndLevelList.Select(p => p.Split('|')[1]).ToList();

            var programItemsCount = page.ControlProgramItemsPopupPanel.GetItemsCount();
            var shapesDiameterList = page.ControlProgramItemsPopupPanel.GetListOfShapesDiameter();
            var timeList = page.ControlProgramItemsPopupPanel.GetListOfTime();
            var levelList = page.ControlProgramItemsPopupPanel.GetListOfLevel();

            VerifyEqual("Verify Number of dots = number of rows", dotsTimeAndLevelList.Count, programItemsCount);
            VerifyEqual("Verify Dimming levels are the same", dotsLevelList, levelList, false);
            VerifyEqual("Verify Points are either diamond or circle or triangle shape", true, shapesDiameterList.All(p => page.ControlProgramItemsPopupPanel.IsShape(p)));
        }

        #endregion //Verify methods

        #region Input XML data

        private Dictionary<string, string> GetTestDataOfTestSM07()
        {
            var testCaseName = "SM07";
            var xmlUtility = new XmlUtility(Settings.SM_TEST_DATA_FILE_PATH);
            var testData = new Dictionary<string, string>();

            testData.Add("TemplateName", xmlUtility.GetSingleNodeText(string.Format(Settings.SM_XPATH_PREFIX, testCaseName, "TemplateName")));

            return testData;
        }

        private Dictionary<string, string> GetTestDataOfTestSM08()
        {
            var testCaseName = "SM08";
            var xmlUtility = new XmlUtility(Settings.SM_TEST_DATA_FILE_PATH);
            var testData = new Dictionary<string, string>();

            testData.Add("TemplateName", xmlUtility.GetSingleNodeText(string.Format(Settings.SM_XPATH_PREFIX, testCaseName, "TemplateName")));

            return testData;
        }

        private Dictionary<string, string> GetTestDataOfTestSM09()
        {
            var testCaseName = "SM09";
            var xmlUtility = new XmlUtility(Settings.SM_TEST_DATA_FILE_PATH);
            var testData = new Dictionary<string, string>();

            testData.Add("TemplateName", xmlUtility.GetSingleNodeText(string.Format(Settings.SM_XPATH_PREFIX, testCaseName, "TemplateName")));

            return testData;
        }

        private Dictionary<string, string> GetTestDataOfTestSM10()
        {
            var testCaseName = "SM10";
            var xmlUtility = new XmlUtility(Settings.SM_TEST_DATA_FILE_PATH);
            var testData = new Dictionary<string, string>();

            testData.Add("TemplateName", xmlUtility.GetSingleNodeText(string.Format(Settings.SM_XPATH_PREFIX, testCaseName, "TemplateName")));

            return testData;
        }

        private Dictionary<string, string> GetTestDataOfTestSM11()
        {
            var testCaseName = "SM11";
            var xmlUtility = new XmlUtility(Settings.SM_TEST_DATA_FILE_PATH);
            var testData = new Dictionary<string, string>();

            testData.Add("TemplateName", xmlUtility.GetSingleNodeText(string.Format(Settings.SM_XPATH_PREFIX, testCaseName, "TemplateName")));

            return testData;
        }

        private Dictionary<string, string> GetTestDataOfTestSM12()
        {
            var testCaseName = "SM12";
            var xmlUtility = new XmlUtility(Settings.SM_TEST_DATA_FILE_PATH);
            var testData = new Dictionary<string, string>();

            testData.Add("TemplateName", xmlUtility.GetSingleNodeText(string.Format(Settings.SM_XPATH_PREFIX, testCaseName, "TemplateName")));

            return testData;
        }
        
        private Dictionary<string, string> GetTestDataOfTestSC_1287()
        {
            var testCaseName = "SC_1287";
            var xmlUtility = new XmlUtility(Settings.SM_TEST_DATA_FILE_PATH);
            var testData = new Dictionary<string, string>();

            testData.Add("Calendar", xmlUtility.GetSingleNodeText(string.Format(Settings.SM_XPATH_PREFIX, testCaseName, "Calendar")));
            testData.Add("RecordsPerPage", xmlUtility.GetSingleNodeText(string.Format(Settings.SM_XPATH_PREFIX, testCaseName, "RecordsPerPage")));

            return testData;
        }

        private Dictionary<string, string> GetTestDataOfTestSM21()
        {
            var testCaseName = "SM21";
            var xmlUtility = new XmlUtility(Settings.SM_TEST_DATA_FILE_PATH);
            var testData = new Dictionary<string, string>();
            
            var controllerInfo = xmlUtility.GetSingleNode(string.Format(Settings.SM_XPATH_PREFIX, testCaseName, "Controller"));
            testData.Add("ControllerName", controllerInfo.GetAttrVal("name"));
            testData.Add("ControllerId", controllerInfo.GetAttrVal("id"));

            return testData;
        }

        #endregion //Input XML data

        #endregion //Private methods
    }
}
