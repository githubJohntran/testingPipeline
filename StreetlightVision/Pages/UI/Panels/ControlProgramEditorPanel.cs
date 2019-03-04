using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using StreetlightVision.Extensions;
using StreetlightVision.Models;
using StreetlightVision.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace StreetlightVision.Pages.UI
{
    public class ControlProgramEditorPanel : PanelBase
    {
        #region Variables

        #endregion //Variables

        #region IWebElements

        [FindsBy(How = How.CssSelector, Using = "[id$='main-panel'] > div:nth-child(1) .schedulermanager-editor-title")]
        private IWebElement panelTitle;

        [FindsBy(How = How.CssSelector, Using = "[id$='main-panel'] > div:nth-child(1) .schedulermanager-editor-scheduleitems-button")]
        private IWebElement controlProgramItemsButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='main-panel'] > div:nth-child(1) .schedulermanager-editor-save-button")]
        private IWebElement saveButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='main-panel'] > div:nth-child(1) .schedulermanager-schedules-editor-fields > .schedulermanager-schedules-editor-fields-row-name-and-color .w2ui-field label")]
        private IWebElement nameLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='main-panel'] > div:nth-child(1) .schedulermanager-schedules-editor-fields > .schedulermanager-schedules-editor-fields-row-name-and-color .w2ui-field input")]
        private IWebElement nameInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='main-panel'] > div:nth-child(1) .schedulermanager-schedules-editor-fields > .schedulermanager-schedules-editor-fields-row-name-and-color .slv-field label")]
        private IWebElement chartColorLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='main-panel'] > div:nth-child(1) .schedulermanager-schedules-editor-fields > .schedulermanager-schedules-editor-fields-row-name-and-color .slv-schedule-color-picker")]
        private IWebElement chartColorPicker;

        [FindsBy(How = How.CssSelector, Using = "[id$='main-panel'] > div:nth-child(1) .schedulermanager-schedules-editor-fields > .w2ui-field label")]
        private IWebElement descriptionLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='main-panel'] > div:nth-child(1) .schedulermanager-schedules-editor-fields > .w2ui-field textarea")]
        private IWebElement descriptionInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='main-panel'] > div:nth-child(1) .schedulermanager-schedules-editor-fields > .slv-field label")]
        private IWebElement templateLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='main-panel'] > div:nth-child(1) .schedulermanager-schedules-editor-fields > .slv-field div[id*='slv-schedulermanager-schedule-template']")]
        private IWebElement templateDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id$='main-panel'] > div:nth-child(1) .schedulermanager-schedules-editor-chart-panel svg.schedulermanager-schedules-chart")]
        private IWebElement chart;

        [FindsBy(How = How.CssSelector, Using = "[id$='main-panel'] > div:nth-child(1) .schedulermanager-schedules-editor-chart-panel svg.schedulermanager-schedules-chart g.sunbased-areas")]
        private IWebElement chartArea;

        [FindsBy(How = How.CssSelector, Using = "[id$='main-panel'] > div:nth-child(1) .schedulermanager-schedules-editor-chart-panel g.dots path.scheduler-editor-chart-dot")]
        private IList<IWebElement> chartDots;

        [FindsBy(How = How.CssSelector, Using = "[id$='main-panel'] > div:nth-child(1) .schedulermanager-schedules-editor-chart-panel g.dots path.scheduler-editor-chart-dot[style*='rgb(255, 255, 255)'], [id$='main-panel'] > div:nth-child(1) .schedulermanager-schedules-editor-chart-panel g.dots path.scheduler-editor-chart-dot[style*='#ffffff']")]
        private IList<IWebElement> chartVariationDots;

        #region Advanced mode

        [FindsBy(How = How.CssSelector, Using = "[id$='main-panel'] > div:nth-child(1) .schedulermanager-schedules-editor-fields .schedulermanager-schedules-editor-timeline-label")]
        private IWebElement timelineLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='main-panel'] > div:nth-child(1) .schedulermanager-schedules-editor-fields div[id*='slv-schedulermanager-schedule-timeline']")]
        private IWebElement timelineIconDropDown;

        #endregion //Advanced mode

        #region Always ON, Always OFF, Astro ON/OFF, Astro ON/OFF and fixed time events, Day fixed time events

        [FindsBy(How = How.CssSelector, Using = "[id$='main-panel'] > div:nth-child(1) .schedulermanager-schedules-editor-template-onoff-fields:nth-child(1) .schedulermanager-schedules-editor-template-on-fields .schedulermanager-schedules-editor-template-onoff-label")]
        private IWebElement switchOnLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='main-panel'] > div:nth-child(1) .schedulermanager-schedules-editor-template-onoff-fields:nth-child(1) .schedulermanager-schedules-editor-template-on-fields .schedulermanager-schedules-editor-template-onoff-time")]
        private IWebElement switchOnTimeInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='main-panel'] > div:nth-child(1) .schedulermanager-schedules-editor-template-onoff-fields:nth-child(1) .schedulermanager-schedules-editor-template-on-fields .schedulermanager-schedules-editor-template-onoff-minutes")]
        private IWebElement switchOnMinuteInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='main-panel'] > div:nth-child(1) .schedulermanager-schedules-editor-template-onoff-fields:nth-child(1) .schedulermanager-schedules-editor-template-on-fields .schedulermanager-schedules-editor-template-onoff-units")]
        private IWebElement switchOnUnitLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='main-panel'] > div:nth-child(1) .schedulermanager-schedules-editor-template-onoff-fields:nth-child(1) .schedulermanager-schedules-editor-template-on-fields .schedulermanager-schedules-editor-template-onoff-relation.select2-container")]
        private IWebElement switchOnRelationDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id$='main-panel'] > div:nth-child(1) .schedulermanager-schedules-editor-template-onoff-fields:nth-child(1) .schedulermanager-schedules-editor-template-on-fields .schedulermanager-schedules-editor-template-onoff-sunevent.select2-container")]
        private IWebElement switchOnSunEventDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id$='main-panel'] > div:nth-child(1) .schedulermanager-schedules-editor-template-onoff-fields:nth-child(1) .schedulermanager-schedules-editor-template-on-fields .schedulermanager-schedules-editor-template-onoff-level")]
        private IWebElement switchOnLevelInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='main-panel'] > div:nth-child(1) .schedulermanager-schedules-editor-template-onoff-fields:nth-child(3) .schedulermanager-schedules-editor-template-off-fields .schedulermanager-schedules-editor-template-onoff-label")]
        private IWebElement switchOffLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='main-panel'] > div:nth-child(1) .schedulermanager-schedules-editor-template-onoff-fields:nth-child(3) .schedulermanager-schedules-editor-template-off-fields .schedulermanager-schedules-editor-template-onoff-time")]
        private IWebElement switchOffTimeInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='main-panel'] > div:nth-child(1) .schedulermanager-schedules-editor-template-onoff-fields:nth-child(3) .schedulermanager-schedules-editor-template-off-fields .schedulermanager-schedules-editor-template-onoff-minutes")]
        private IWebElement switchOffMinuteInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='main-panel'] > div:nth-child(1) .schedulermanager-schedules-editor-template-onoff-fields:nth-child(3) .schedulermanager-schedules-editor-template-off-fields .schedulermanager-schedules-editor-template-onoff-units")]
        private IWebElement switchOffnUnitLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='main-panel'] > div:nth-child(1) .schedulermanager-schedules-editor-template-onoff-fields:nth-child(3) .schedulermanager-schedules-editor-template-off-fields .schedulermanager-schedules-editor-template-onoff-relation.select2-container")]
        private IWebElement switchOffRelationDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id$='main-panel'] > div:nth-child(1) .schedulermanager-schedules-editor-template-onoff-fields:nth-child(3) .schedulermanager-schedules-editor-template-off-fields .schedulermanager-schedules-editor-template-onoff-sunevent.select2-container")]
        private IWebElement switchOffSunEventDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id$='main-panel'] > div:nth-child(1) .schedulermanager-schedules-editor-template-onoff-fields:nth-child(3) .schedulermanager-schedules-editor-template-off-fields .schedulermanager-schedules-editor-template-onoff-level")]
        private IWebElement switchOffLevelInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='main-panel'] > div:nth-child(1) .schedulermanager-schedules-editor-template-variations .schedulermanager-schedules-editor-template-variations-label")]
        private IWebElement variationsLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='main-panel'] > div:nth-child(1) .schedulermanager-schedules-editor-template-variations .schedulermanager-schedules-editor-template-variations-add-button")]
        private IWebElement variationsAddButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='main-panel'] > div:nth-child(1) .schedulermanager-schedules-editor-template-variations-events .schedulermanager-schedules-editor-template-variation-time")]
        private IList<IWebElement> variationsEventsTimeInputList;

        [FindsBy(How = How.CssSelector, Using = "[id$='main-panel'] > div:nth-child(1) .schedulermanager-schedules-editor-template-variations-events .schedulermanager-schedules-editor-template-variation-level")]
        private IList<IWebElement> variationsEventsLevelInputList;

        [FindsBy(How = How.CssSelector, Using = "[id$='main-panel'] > div:nth-child(1) .schedulermanager-schedules-editor-template-variations-events i.fa-trash-o")]
        private IList<IWebElement> variationsEventsRemoveButtonsList;

        [FindsBy(How = How.CssSelector, Using = "[id$='main-panel'] > div:nth-child(1) .schedulermanager-schedules-editor-template-variations-events [data-index]")]
        private IList<IWebElement> variationsEventsList;

        [FindsBy(How = How.CssSelector, Using = "[id$='main-panel'] > div:nth-child(1) .schedulermanager-schedules-editor-template-onoff-fields:nth-child(4) .schedulermanager-schedules-editor-template-off-fields .schedulermanager-schedules-editor-template-onoff-label")]
        private IWebElement optionsLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='main-panel'] > div:nth-child(1) .schedulermanager-schedules-editor-template-onoff-fields:nth-child(4) .schedulermanager-schedules-editor-template-off-fields .schedulermanager-schedules-editor-template-fields .checkbox")]
        private IWebElement optionsPhotocellEnableCheckbox;

        [FindsBy(How = How.CssSelector, Using = "[id$='main-panel'] > div:nth-child(1) .schedulermanager-schedules-editor-template-onoff-fields:nth-child(4) .schedulermanager-schedules-editor-template-off-fields .schedulermanager-schedules-editor-template-fields .schedulermanager-editor-help-button")]
        private IWebElement optionsPhotocellEnableInformationButton;

        #endregion //Always ON, Always OFF, Astro ON/OFF, Astro ON/OFF and fixed time events, Day fixed time events

        #endregion //IWebElements

        #region Constructor

        public ControlProgramEditorPanel(IWebDriver driver, PageBase page) : base(driver, page)
        {
            PageFactory.InitElements(this, new RetryingElementLocator(driver, TimeSpan.FromMilliseconds(Settings.LocatorRetryingTimeout), TimeSpan.FromMilliseconds(Settings.LocatorPollingInterval)));
        }

        #endregion //Constructor

        #region Properties        

        #endregion //Properties

        #region Basic methods 

        #region Actions

        /// <summary>
        /// Click 'ControlProgramItems' button
        /// </summary>
        public void ClickControlProgramItemsButton()
        {
            controlProgramItemsButton.ClickEx();
        }

        /// <summary>
        /// Click 'Save' button
        /// </summary>
        public void ClickSaveButton()
        {
            saveButton.ClickEx();
        }

        /// <summary>
        /// Enter a value for 'Name' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterNameInput(string value)
        {
            nameInput.Enter(value);
        }

        /// <summary>
        /// Set a random color for 'Chart' color picker
        /// </summary>
        public void SetChartColorPicker()
        {
            chartColorPicker.SetColor();
        }

        /// <summary>
        /// Enter a value for 'Description' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterDescriptionInput(string value)
        {
            descriptionInput.Enter(value);
        }

        /// <summary>
        /// Select an item of 'Template' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectTemplateDropDown(string value)
        {
            templateDropDown.Select(value, true);
        }

        #region Advanced mode

        /// <summary>
        /// Select an item of 'TimelineIcon' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectTimelineIconDropDown(string value)
        {
            timelineIconDropDown.SelectIcon(value);
        }

        #endregion //Advanced mode

        #region Always ON, Always OFF, Astro ON/OFF, Astro ON/OFF and fixed time events, Day fixed time events

        /// <summary>
        /// Enter a value for 'SwitchOnTime' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterSwitchOnTimeInput(string value)
        {
            switchOnTimeInput.EnterTime(value);
        }

        /// <summary>
        /// Enter a value for 'SwitchOnMinute' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterSwitchOnMinuteInput(string value)
        {
            switchOnMinuteInput.Enter(value);
        }

        /// <summary>
        /// Select an item of 'SwitchOnRelation' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectSwitchOnRelationDropDown(string value)
        {
            switchOnRelationDropDown.Select(value, true);
        }

        /// <summary>
        /// Select an item of 'SwitchOnSunEvent' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectSwitchOnSunEventDropDown(string value)
        {
            switchOnSunEventDropDown.Select(value);
        }

        /// <summary>
        /// Enter a value for 'SwitchOnLevel' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterSwitchOnLevelInput(string value)
        {
            switchOnLevelInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'SwitchOffTime' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterSwitchOffTimeInput(string value)
        {
            switchOffTimeInput.EnterTime(value);
        }

        /// <summary>
        /// Enter a value for 'SwitchOffMinute' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterSwitchOffMinuteInput(string value)
        {
            switchOffMinuteInput.Enter(value);
        }

        /// <summary>
        /// Select an item of 'SwitchOffRelation' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectSwitchOffRelationDropDown(string value)
        {
            switchOffRelationDropDown.Select(value, true);
        }

        /// <summary>
        /// Select an item of 'SwitchOffSunEvent' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectSwitchOffSunEventDropDown(string value)
        {
            switchOffSunEventDropDown.Select(value);
        }

        /// <summary>
        /// Enter a value for 'SwitchOffLevel' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterSwitchOffLevelInput(string value)
        {
            switchOffLevelInput.Enter(value);
        }

        /// <summary>
        /// Click 'VariationsAdd' button
        /// </summary>
        public void ClickVariationsAddButton()
        {
            variationsAddButton.ClickEx();
        }

        /// <summary>
        /// Tick 'OptionsPhotocellEnable' checkbox
        /// </summary>
        /// <param name="value"></param>
        public void TickOptionsPhotocellEnableCheckbox(bool value)
        {
            optionsPhotocellEnableCheckbox.Check(value);
        }

        /// <summary>
        /// Click 'OptionsPhotocellEnableInformation' button
        /// </summary>
        public void ClickOptionsPhotocellEnableInformationButton()
        {
            optionsPhotocellEnableInformationButton.ClickEx();
        }

        #endregion //Always ON, Always OFF, Astro ON/OFF, Astro ON/OFF and fixed time events, Day fixed time events

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
        /// Get 'ChartColor' label text
        /// </summary>
        /// <returns></returns>
        public string GetChartColorText()
        {
            return chartColorLabel.Text;
        }

        /// <summary>
        /// Get 'Chart' color picker value
        /// </summary>
        /// <returns></returns>
        public Color GetChartColorValue()
        {
            return chartColorPicker.ColorValue();
        }

        /// <summary>
        /// Get 'Description' label text
        /// </summary>
        /// <returns></returns>
        public string GetDescriptionText()
        {
            return descriptionLabel.Text;
        }

        /// <summary>
        /// Get 'Description' input value
        /// </summary>
        /// <returns></returns>
        public string GetDescriptionValue()
        {
            return descriptionInput.Value();
        }

        /// <summary>
        /// Get 'Template' label text
        /// </summary>
        /// <returns></returns>
        public string GetTemplateText()
        {
            return templateLabel.Text;
        }

        /// <summary>
        /// Get 'Template' input value
        /// </summary>
        /// <returns></returns>
        public string GetTemplateValue()
        {
            return templateDropDown.Text;
        }

        #region Advanced mode

        /// <summary>
        /// Get 'Timeline' label text
        /// </summary>
        /// <returns></returns>
        public string GetTimelineText()
        {
            return timelineLabel.Text;
        }

        /// <summary>
        /// Get 'TimelineIcon' input value
        /// </summary>
        /// <returns></returns>
        public string GetTimelineIconValue()
        {
            return timelineIconDropDown.IconValue();
        }

        #endregion //Advanced mode

        #region Always ON, Always OFF, Astro ON/OFF, Astro ON/OFF and fixed time events, Day fixed time events

        /// <summary>
        /// Get 'SwitchOn' label text
        /// </summary>
        /// <returns></returns>
        public string GetSwitchOnText()
        {
            return switchOnLabel.Text;
        }

        /// <summary>
        /// Get 'SwitchOnTime' input value
        /// </summary>
        /// <returns></returns>
        public string GetSwitchOnTimeValue()
        {
            return switchOnTimeInput.TimeValue();
        }

        /// <summary>
        /// Get 'SwitchOnMinute' input value
        /// </summary>
        /// <returns></returns>
        public string GetSwitchOnMinuteValue()
        {
            return switchOnMinuteInput.Value();
        }

        /// <summary>
        /// Get 'SwitchOnUnit' label text
        /// </summary>
        /// <returns></returns>
        public string GetSwitchOnUnitText()
        {
            return switchOnUnitLabel.Text;
        }

        /// <summary>
        /// Get 'SwitchOnRelation' dropdown value
        /// </summary>
        /// <returns></returns>
        public string GetSwitchOnRelationValue()
        {
            return switchOnRelationDropDown.Text;
        }

        /// <summary>
        /// Get 'SwitchOnSunEvent' dropdown value
        /// </summary>
        /// <returns></returns>
        public string GetSwitchOnSunEventValue()
        {
            return switchOnSunEventDropDown.Text;
        }

        /// <summary>
        /// Get 'SwitchOnLevel' input value
        /// </summary>
        /// <returns></returns>
        public string GetSwitchOnLevelValue()
        {
            return switchOnLevelInput.Value();
        }

        /// <summary>
        /// Get 'SwitchOff' label text
        /// </summary>
        /// <returns></returns>
        public string GetSwitchOffText()
        {
            return switchOffLabel.Text;
        }

        /// <summary>
        /// Get 'SwitchOffTime' input value
        /// </summary>
        /// <returns></returns>
        public string GetSwitchOffTimeValue()
        {
            return switchOffTimeInput.TimeValue();
        }

        /// <summary>
        /// Get 'SwitchOffMinute' input value
        /// </summary>
        /// <returns></returns>
        public string GetSwitchOffMinuteValue()
        {
            return switchOffMinuteInput.Value();
        }

        /// <summary>
        /// Get 'SwitchOffnUnit' label text
        /// </summary>
        /// <returns></returns>
        public string GetSwitchOffnUnitText()
        {
            return switchOffnUnitLabel.Text;
        }

        /// <summary>
        /// Get 'SwitchOffRelation' dropdown value
        /// </summary>
        /// <returns></returns>
        public string GetSwitchOffRelationValue()
        {
            return switchOffRelationDropDown.Text;
        }

        /// <summary>
        /// Get 'SwitchOffSunEvent' dropdown value
        /// </summary>
        /// <returns></returns>
        public string GetSwitchOffSunEventValue()
        {
            return switchOffSunEventDropDown.Text;
        }

        /// <summary>
        /// Get 'SwitchOffLevel' input value
        /// </summary>
        /// <returns></returns>
        public string GetSwitchOffLevelValue()
        {
            return switchOffLevelInput.Value();
        }

        /// <summary>
        /// Get 'Variations' label text
        /// </summary>
        /// <returns></returns>
        public string GetVariationsText()
        {
            return variationsLabel.Text;
        }

        /// <summary>
        /// Get 'Options' label text
        /// </summary>
        /// <returns></returns>
        public string GetOptionsText()
        {
            return optionsLabel.Text;
        }

        /// <summary>
        /// Get 'OptionsPhotocellEnable' checkbox value
        /// </summary>
        /// <returns></returns>
        public bool GetOptionsPhotocellEnableValue()
        {
            return optionsPhotocellEnableCheckbox.CheckboxValue();
        }

        #endregion //Always ON, Always OFF, Astro ON/OFF, Astro ON/OFF and fixed time events, Day fixed time events

        #endregion //Get methods

        #endregion //Basic methods

        #region Business methods

        public void WaitForControlProgramNameDisplayed(string value)
        {
            Wait.ForElementValue(nameInput, value);
        }

        /// <summary>
        /// Get chart dots count
        /// </summary>
        /// <returns></returns>
        public int GetChartDotsCount()
        {
            return chartDots.Count;
        }

        public int GetChartVariationDotsCount()
        {
            return chartVariationDots.Count;
        }

        /// <summary>
        /// Get Diameter of a Chart Dot 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public string GetChartDotDiameter(int index)
        {
            return chartDots[index].GetAttribute("d");
        }

        public void DoubleClickRandomDot()
        {
            var curDot = chartDots.PickRandom();
            curDot.DoubleClick(false);
        }

        public void DoubleClickRandomVariationDot()
        {
            var curDot = chartVariationDots.PickRandom();
            curDot.DoubleClick(false);
        }

        public void DoubleClickRandomInsideChart()
        {
            var random = new Random();
            var x = chartArea.Size.Width / 2 - random.Next(-100, 100);
            var y = chartArea.Size.Height / 2 - random.Next(-20, 20);

            chartArea.DoubleClick(x, y, false);
            Wait.ForSeconds(1);
        }

        public void MoveToRandomInsideChart()
        {
            var random = new Random();
            var x = chartArea.Size.Width / 2 - random.Next(-100, 100);
            var y = chartArea.Size.Height / 2 - random.Next(-20, 20);

            chartArea.MoveTo(x, y);
        }

        public void WaitForChartTooltipDisplayed()
        {
            Wait.ForElementStyle(By.CssSelector("[id$='main-panel'] > div:nth-child(1) .schedulermanager-schedules-editor-chart-panel g.dots g:nth-child(2) text.tooltip"), "display: inline");            
        }
        
        public void MoveToSunsetDot()
        {
            var sunsetDot = chartDots.FirstOrDefault();
            sunsetDot.MoveTo();
            WaitForChartTooltipDisplayed();
        }

        public void MoveToSunriseDot()
        {
            var sunriseDot = chartDots.LastOrDefault();
            sunriseDot.MoveTo();
            WaitForChartTooltipDisplayed();
        }

        public void MoveToRandomVariationDot()
        {
            var random = new Random();
            var index = random.Next(0, chartVariationDots.Count - 1);
            var variationDot = chartVariationDots[index];
            variationDot.MoveTo();
            WaitForChartTooltipDisplayed();
        }

        public void MoveToVariationDot(int index)
        {
            if (chartVariationDots.Count < 1) return;
            var variationDot = chartVariationDots[index];
            variationDot.MoveTo();
            WaitForChartTooltipDisplayed();
        }

        public void MoveToChartDot(int index)
        {
            var dotCount = chartDots.Count;
            var variationDot = chartDots[index];
            variationDot.MoveTo();
            WaitForChartTooltipDisplayed();
        }

        public string GetDotTooltipTime()
        {
            var timeTooltip = Driver.FindElement(By.CssSelector("[id$='main-panel'] > div:nth-child(1) .schedulermanager-schedules-editor-chart-panel g.dots g:nth-child(2) text.tooltip:nth-child(2)"));
            return timeTooltip.Text;            
        }

        public string GetDotTooltipLevel()
        {
            var levelTooltip = Driver.FindElement(By.CssSelector("[id$='main-panel'] > div:nth-child(1) .schedulermanager-schedules-editor-chart-panel g.dots g:nth-child(2) text.tooltip:nth-child(5)"));
            return levelTooltip.Text;          
        }

        /// <summary>
        /// Get list of chart dots time and level with format(Time|Level, ex: 20:00|90%)
        /// </summary>
        /// <returns></returns>
        public List<string> GetListOfTimeAndLevelChartDots()
        {
            var result = new List<string>();
            foreach (var dot in chartDots)
            {
                dot.MoveTo();
                Wait.ForMilliseconds(500);
                result.Add(string.Format("{0}|{1}", GetDotTooltipTime(), GetDotTooltipLevel()));
            }

            return result;
        }

        public bool IsNameInputReadOnly()
        {
            return nameInput.IsReadOnly();
        }

        public bool IsDescriptionInputReadOnly()
        {
            return descriptionInput.IsReadOnly();
        }

        public bool IsChartColorPickerReadOnly()
        {
            return chartColorPicker.Displayed && Driver.FindElements(By.CssSelector("[id$='main-panel'] > div:nth-child(1) .schedulermanager-schedules-editor-fields > .schedulermanager-schedules-editor-fields-row-name-and-color .slv-schedule-color-picker.disabled")).Any();
        }

        public bool IsTemplateDropDownReadOnly()
        {
            return templateDropDown.IsReadOnly(false);
        }

        public bool IsTimelineDropDownReadOnly()
        {
            return timelineIconDropDown.Displayed && Driver.FindElements(By.CssSelector("[id$='main-panel'] > div:nth-child(1) div[id*='slv-schedulermanager-schedule-timeline'].slv-schedule-timeline-disabled")).Any();
        }

        public bool IsOptionsPhotocellEnableDisplayed()
        {
            return optionsPhotocellEnableCheckbox.Displayed && optionsPhotocellEnableCheckbox.Text.Trim().Equals("Daytime Photocell Override");
        }

        public bool IsOptionsHelperIconDisplayed()
        {
            return optionsPhotocellEnableInformationButton.Displayed;
        }

        #region Switch On

        public bool IsSwitchOnTimeInputReadOnly()
        {
            return switchOnTimeInput.IsReadOnly();
        }

        public bool IsSwitchOnLevelInputReadOnly()
        {
            return switchOnLevelInput.IsReadOnly();
        }

        public bool IsSwitchOnMinuteInputReadOnly()
        {
            return switchOnMinuteInput.IsReadOnly();
        }

        public bool IsSwitchOnRelationDropDownReadOnly()
        {
            return switchOnRelationDropDown.Displayed && Driver.FindElements(By.CssSelector("[id$='main-panel'] > div:nth-child(1) .schedulermanager-schedules-editor-template-on-fields .schedulermanager-schedules-editor-template-onoff-relation.select2-container-disabled")).Any();
        }

        public bool IsSwitchOnSunEventsDropDownReadOnly()
        {
            return switchOnSunEventDropDown.Displayed && Driver.FindElements(By.CssSelector("[id$='main-panel'] > div:nth-child(1) .schedulermanager-schedules-editor-template-on-fields .schedulermanager-schedules-editor-template-onoff-sunevent.select2-container-disabled")).Any();
        }

        #endregion //Switch On

        #region Switch Off

        public bool IsSwitchOffTimeInputReadOnly()
        {
            return switchOffTimeInput.IsReadOnly();
        }

        public bool IsSwitchOffLevelInputReadOnly()
        {
            return switchOffLevelInput.IsReadOnly();
        }

        public bool IsSwitchOffMinuteInputReadOnly()
        {
            return switchOffMinuteInput.IsReadOnly();
        }

        public bool IsSwitchOffRelationDropDownReadOnly()
        {
            return switchOffRelationDropDown.Displayed && Driver.FindElements(By.CssSelector("[id$='main-panel'] > div:nth-child(1) .schedulermanager-schedules-editor-template-off-fields .schedulermanager-schedules-editor-template-onoff-relation.select2-container-disabled")).Any();
        }

        public bool IsSwitchOffSunEventsDropDownReadOnly()
        {
            return switchOffSunEventDropDown.Displayed && Driver.FindElements(By.CssSelector("[id$='main-panel'] > div:nth-child(1) .schedulermanager-schedules-editor-template-off-fields .schedulermanager-schedules-editor-template-onoff-sunevent.select2-container-disabled")).Any();
        }

        #endregion //Switch Off

        #region Variations

        public bool AreVariationsTimeInputsReadOnly()
        {
            foreach (var input in variationsEventsTimeInputList)
            {
                if (!input.IsReadOnly())
                    return false;
            }
            return true;
        }

        public bool AreVariationsTimeInputsEditable()
        {
            foreach (var input in variationsEventsTimeInputList)
            {
                if (input.IsReadOnly())
                    return false;
            }
            return true;
        }

        public bool AreVariationsLevelInputsReadOnly()
        {
            foreach (var input in variationsEventsLevelInputList)
            {
                if (!input.IsReadOnly())
                    return false;
            }
            return true;
        }

        public bool AreVariationsLevelInputsEditable()
        {
            foreach (var input in variationsEventsLevelInputList)
            {
                if (input.IsReadOnly())
                    return false;
            }
            return true;
        }

        public int GetVariationsCount()
        {
            return variationsEventsList.Count;
        }

        public string GetFirstVariationTimeInputValue()
        {
            var input = variationsEventsTimeInputList.FirstOrDefault();
            return input.TimeValue();
        }
        public string GetLastVariationTimeInputValue()
        {
            var input = variationsEventsTimeInputList.LastOrDefault();
            return input.TimeValue();
        }

        public string GetFirstVariationLevelInputValue()
        {
            var input = variationsEventsLevelInputList.FirstOrDefault();
            return input.Value();
        }

        public string GetLastVariationLevelInputValue()
        {
            var input = variationsEventsLevelInputList.LastOrDefault();
            return input.Value();
        }

        public string GetVariationTimeInputValue(int index)
        {
            var input = variationsEventsTimeInputList[index];
            return input.TimeValue();
        }

        public string GetVariationLevelInputValue(int index)
        {
            var input = variationsEventsLevelInputList[index];
            return input.Value();
        }

        public void EnterFirstVariationTimeInput(string value)
        {
            var input = variationsEventsTimeInputList.FirstOrDefault();
            input.EnterTime(value);
        }

        public void EnterLastVariationTimeInput(string value)
        {
            var input = variationsEventsTimeInputList.LastOrDefault();
            input.EnterTime(value);
        }

        public void EnterFirstVariationLevelInput(string value)
        {
            var input = variationsEventsLevelInputList.FirstOrDefault();
            input.Enter(value);
        }

        public void EnterLastVariationLevelInput(string value)
        {
            var input = variationsEventsLevelInputList.LastOrDefault();
            input.Enter(value);
        }

        public void EnterVariationTimeInput(int index, string value)
        {
            var input = variationsEventsTimeInputList[index];
            input.EnterTime(value);
        }

        public void EnterVariationLevelInput(int index, string value)
        {
            var input = variationsEventsLevelInputList[index];
            input.Enter(value);
        }

        public void ClickFirstVariationRemoveButton()
        {
            var button = variationsEventsRemoveButtonsList.FirstOrDefault();
            button.ClickEx();
        }

        public void ClickLastVariationRemoveButton()
        {
            var button = variationsEventsRemoveButtonsList.LastOrDefault();
            button.ClickEx();
        }

        public void ClickVariationRemoveButton(int index)
        {
            var button = variationsEventsRemoveButtonsList[index];
            button.ClickEx();
        }

        /// <summary>
        /// Get list of variations time and level inputs with format(Time|Level, ex: 20:00|90%)
        /// </summary>
        /// <returns></returns>
        public List<string> GetListOfVariationsTimeAndLevelInput()
        {
            var variationsCount = GetVariationsCount();
            var result = new List<string>();

            for (int i = 0; i < variationsCount; i++)
            {
                result.Add(string.Format("{0}|{1}", GetVariationTimeInputValue(i), GetVariationLevelInputValue(i)));
            }

            return result;
        }

        #endregion //Variations

        public bool IsControlProgramItemsButtonVisible()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='main-panel'] > div:nth-child(1) .schedulermanager-editor-scheduleitems-button"));
        }

        public bool IsSaveButtonVisible()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='main-panel'] > div:nth-child(1) .schedulermanager-editor-save-button"));
        }

        public List<string> GetListOfTemplateItems()
        {
            return templateDropDown.GetAllItems();
        }

        public bool IsSwitchOnGroupVisible()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='main-panel'] > div:nth-child(1) .schedulermanager-schedules-editor-template-on-fields"));
        }

        public bool IsSwitchOffGroupVisible()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='main-panel'] > div:nth-child(1) .schedulermanager-schedules-editor-template-off-fields"));
        }

        public bool IsVariationsGroupVisible()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='main-panel'] > div:nth-child(1) .schedulermanager-schedules-editor-template-variations"));
        }

        public bool IsTimelineVisible()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='main-panel'] > div:nth-child(1) .schedulermanager-schedules-editor-fields div[id*='slv-schedulermanager-schedule-timeline']"));
        }

        public byte[] GetBytesOfMoonIcon()
        {
            var path = Settings.GetFullPath(string.Format(@"Resources\img\apps\{0}\moonIcon.png", App.SchedulingManager));
            return File.ReadAllBytes(path);
        }

        public byte[] GetBytesOfSunIcon()
        {
            var path = Settings.GetFullPath(string.Format(@"Resources\img\apps\{0}\sunIcon.png", App.SchedulingManager));
            return File.ReadAllBytes(path);
        }

        public byte[] GetBytesOfTimeIcon()
        {
            var path = Settings.GetFullPath(string.Format(@"Resources\img\apps\{0}\timeIcon.png", App.SchedulingManager));
            return File.ReadAllBytes(path);
        }

        public byte[] GetTimelineIconBytes()
        {
            var timelineIconBase64String = GetTimelineIconValue();
            var timelineIconBytes = Convert.FromBase64String(timelineIconBase64String);

            return timelineIconBytes;
        }

        public byte[] GetBytesOfChart()
        {
            return chart.TakeScreenshotAsBytes();
        }

        #endregion //Business methods

        public override void WaitForPanelLoaded()
        {
            Wait.ForSeconds(2);
        }
    }
}
