using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using StreetlightVision.Extensions;
using StreetlightVision.Models;
using StreetlightVision.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace StreetlightVision.Pages.UI
{
    public class ControllerWidgetPanel : DeviceWidgetPanelBase
    {
        #region Variables

        private InputsOutputsWidgetPanel _inputsOutputsWidgetPanel;

        #endregion //Variables

        #region IWebElements        

        #region Main form

        #region Header        

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] [id$='status-panel'] [id$='status-tab']")]
        private IWebElement inputsOutputsButton;

        #endregion //Header

        #region Realtime

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] .controller button[value='100']")]
        private IWebElement commandOnButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] .controller button[value='90']")]
        private IWebElement commandDim90Button;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] .controller button[value='80']")]
        private IWebElement commandDim80Button;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] .controller button[value='70']")]
        private IWebElement commandDim70Button;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] .controller button[value='60']")]
        private IWebElement commandDim60Button;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] .controller button[value='50']")]
        private IWebElement commandDim50Button;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] .controller button[value='40']")]
        private IWebElement commandDim40Button;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] .controller button[value='30']")]
        private IWebElement commandDim30Button;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] .controller button[value='20']")]
        private IWebElement commandDim20Button;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] .controller button[value='10']")]
        private IWebElement commandDim10Button;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] .controller button[value='0']")]
        private IWebElement commandOffButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] .controller [id$='jauge-indicator']")]
        private IWebElement realtimeJaugeIndicatorCanvas;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] .controller [id$='jauge-cursor']")]
        private IWebElement realtimeJaugeCursorCanvas;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] .controller [id$='jauge-indicator-value']")]
        private IWebElement realtimeFeedbackValueLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] .controller [id$='jauge-indicator-title']")]
        private IWebElement realtimeFeedbackLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] .controller [id$='jauge-consign-value']")]
        private IWebElement realtimeCommandValueLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] .controller [id$='jauge-consign-title']")]
        private IWebElement realtimeCommandLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] .controller [id$='jauge-consign-input']")]
        private IWebElement realtimeCommandInput;        

        #endregion //Realtime

        #region Data info

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] .controller [id$='recto'] [id$='systemTime'] > div:nth-child(1)")]
        private IWebElement systemTimeLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] .controller [id$='recto'] [id$='systemTime'] > div:nth-child(2)")]
        private IWebElement systemTimeValueLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] .controller [id$='recto'] [id$='gpsLocation'][style*='top: 80px;'] > div:nth-child(1)")]
        private IWebElement gpsPositionLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] .controller [id$='recto'] [id$='gpsLocation'][style*='top: 80px;'] > div:nth-child(2) > div:nth-child(1)")]
        private IWebElement gpsPositionLatitudeLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] .controller [id$='recto'] [id$='gpsLocation'][style*='top: 80px;'] > div:nth-child(2) > div:nth-child(2)")]
        private IWebElement gpsPositionLongitubeLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] .controller [id$='recto'] [id$='gpsLocation'][style*='top: 135px;'] > div:nth-child(1)")]
        private IWebElement gpsLastDataSentLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] .controller [id$='recto'] [id$='gpsLocation'][style*='top: 135px;'] > div:nth-child(2)")]
        private IWebElement gpsLastDataSentValueLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] .controller [id$='recto'] [id$='wholeSegment'] > div")]
        private IWebElement wholeSegmentLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] .controller [id$='recto'] [id$='wholeSegment'] > button")]
        private IWebElement wholeSegmentAutomaticButton;

        #endregion //Data info

        #endregion //Main form

        #endregion //IWebElements

        #region Constructor

        public ControllerWidgetPanel(IWebDriver driver, PageBase page) : base(driver, page)
        {
            PageFactory.InitElements(this, new RetryingElementLocator(driver, TimeSpan.FromMilliseconds(Settings.LocatorRetryingTimeout), TimeSpan.FromMilliseconds(Settings.LocatorPollingInterval)));

            WaitForPanelLoaded();
        }

        #endregion //Constructor

        #region Properties

        public InputsOutputsWidgetPanel InputsOutputsWidgetPanel
        {
            get
            {
                if (_inputsOutputsWidgetPanel == null)
                {
                    _inputsOutputsWidgetPanel = new InputsOutputsWidgetPanel(this.Driver, this.Page);
                }

                return _inputsOutputsWidgetPanel;
            }
        }

        #endregion //Properties

        #region Basic methods

        #region Actions

        #region Main form

        #region Header

        /// <summary>
        /// Click 'InputsOutputs' button
        /// </summary>
        public void ClickInputsOutputsButton()
        {
            inputsOutputsButton.ClickEx();
        }

        #endregion //Header

        #region Realtime

        /// <summary>
        /// Click 'CommandOn' button
        /// </summary>
        public void ClickCommandOnButton()
        {
            commandOnButton.ClickEx();
        }

        /// <summary>
        /// Click 'CommandDim90' button
        /// </summary>
        public void ClickCommandDim90Button()
        {
            commandDim90Button.ClickEx();
        }

        /// <summary>
        /// Click 'CommandDim80' button
        /// </summary>
        public void ClickCommandDim80Button()
        {
            commandDim80Button.ClickEx();
        }

        /// <summary>
        /// Click 'CommandDim70' button
        /// </summary>
        public void ClickCommandDim70Button()
        {
            commandDim70Button.ClickEx();
        }

        /// <summary>
        /// Click 'CommandDim60' button
        /// </summary>
        public void ClickCommandDim60Button()
        {
            commandDim60Button.ClickEx();
        }

        /// <summary>
        /// Click 'CommandDim50' button
        /// </summary>
        public void ClickCommandDim50Button()
        {
            commandDim50Button.ClickEx();
        }

        /// <summary>
        /// Click 'CommandDim40' button
        /// </summary>
        public void ClickCommandDim40Button()
        {
            commandDim40Button.ClickEx();
        }

        /// <summary>
        /// Click 'CommandDim30' button
        /// </summary>
        public void ClickCommandDim30Button()
        {
            commandDim30Button.ClickEx();
        }

        /// <summary>
        /// Click 'CommandDim20' button
        /// </summary>
        public void ClickCommandDim20Button()
        {
            commandDim20Button.ClickEx();
        }

        /// <summary>
        /// Click 'CommandDim10' button
        /// </summary>
        public void ClickCommandDim10Button()
        {
            commandDim10Button.ClickEx();
        }

        /// <summary>
        /// Click 'CommandOff' button
        /// </summary>
        public void ClickCommandOffButton()
        {
            commandOffButton.ClickEx();
        }

        /// <summary>
        /// Enter a value for 'RealtimeCommand' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterRealtimeCommandInput(string value)
        {
            realtimeCommandInput.Enter(value);
        }

        #endregion //Realtime

        #region Data info

        /// <summary>
        /// Click 'WholeSegmentAutomatic' button
        /// </summary>
        public void ClickWholeSegmentAutomaticButton()
        {
            wholeSegmentAutomaticButton.ClickEx();
        }

        #endregion //Data info

        #endregion //Main form        

        #endregion //Actions

        #region Get methods

        #region Main form

        #region Header        

        #endregion //Header

        #region Realtime

        /// <summary>
        /// Get 'RealtimeFeedbackValue' label text
        /// </summary>
        /// <returns></returns>
        public string GetRealtimeFeedbackValueText()
        {
            return realtimeFeedbackValueLabel.Text;
        }

        /// <summary>
        /// Get 'RealtimeFeedback' label text
        /// </summary>
        /// <returns></returns>
        public string GetRealtimeFeedbackText()
        {
            return realtimeFeedbackLabel.Text;
        }

        /// <summary>
        /// Get 'RealtimeCommandValue' label text
        /// </summary>
        /// <returns></returns>
        public string GetRealtimeCommandValueText()
        {
            return realtimeCommandValueLabel.Text;
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
        /// Get 'RealtimeCommand' input value
        /// </summary>
        /// <returns></returns>
        public string GetRealtimeCommandValue()
        {
            return realtimeCommandInput.Value();
        }

        #endregion //Realtime

        #region Data info

        /// <summary>
        /// Get 'SystemTime' label text
        /// </summary>
        /// <returns></returns>
        public string GetSystemTimeText()
        {
            return systemTimeLabel.Text;
        }

        /// <summary>
        /// Get 'SystemTimeValue' label text
        /// </summary>
        /// <returns></returns>
        public string GetSystemTimeValueText()
        {
            return systemTimeValueLabel.Text;
        }

        /// <summary>
        /// Get 'GpsPosition' label text
        /// </summary>
        /// <returns></returns>
        public string GetGpsPositionText()
        {
            return gpsPositionLabel.Text;
        }

        /// <summary>
        /// Get 'GpsPositionLatitude' label text
        /// </summary>
        /// <returns></returns>
        public string GetGpsPositionLatitudeText()
        {
            return gpsPositionLatitudeLabel.Text;
        }

        /// <summary>
        /// Get 'GpsPositionLongitube' label text
        /// </summary>
        /// <returns></returns>
        public string GetGpsPositionLongitubeText()
        {
            return gpsPositionLongitubeLabel.Text;
        }

        /// <summary>
        /// Get 'GpsLastDataSent' label text
        /// </summary>
        /// <returns></returns>
        public string GetGpsLastDataSentText()
        {
            return gpsLastDataSentLabel.Text;
        }

        /// <summary>
        /// Get 'GpsLastDataSentValue' label text
        /// </summary>
        /// <returns></returns>
        public string GetGpsLastDataSentValueText()
        {
            return gpsLastDataSentValueLabel.Text;
        }

        /// <summary>
        /// Get 'WholeSegment' label text
        /// </summary>
        /// <returns></returns>
        public string GetWholeSegmentText()
        {
            return wholeSegmentLabel.Text;
        }

        #endregion //Data info

        #endregion //Main form        

        #endregion //Get methods

        #endregion //Basic methods

        #region Business methods

        /// <summary>
        /// Execute a specific command
        /// </summary>
        /// <param name="command"></param>
        public void ExecuteCommand(RealtimeCommand command)
        {
            switch (command)
            {
                case RealtimeCommand.DimOn:
                    ClickCommandOnButton();
                    break;
                case RealtimeCommand.Dim90:
                    ClickCommandDim90Button();
                    break;
                case RealtimeCommand.Dim80:
                    ClickCommandDim80Button();
                    break;
                case RealtimeCommand.Dim70:
                    ClickCommandDim70Button();
                    break;
                case RealtimeCommand.Dim60:
                    ClickCommandDim60Button();
                    break;
                case RealtimeCommand.Dim50:
                    ClickCommandDim50Button();
                    break;
                case RealtimeCommand.Dim40:
                    ClickCommandDim40Button();
                    break;
                case RealtimeCommand.Dim30:
                    ClickCommandDim30Button();
                    break;
                case RealtimeCommand.Dim20:
                    ClickCommandDim20Button();
                    break;
                case RealtimeCommand.Dim10:
                    ClickCommandDim10Button();
                    break;
                case RealtimeCommand.DimOff:
                    ClickCommandOffButton();
                    break;
            }
        }

        /// <summary>
        /// Execute random dimming (expect 10%, 20%, 30%)
        /// </summary>
        /// <returns></returns>
        public RealtimeCommand ExecuteRandomDimming()
        {
            var random = new Random();
            var commands = new Dictionary<RealtimeCommand, CommandModel>(CommandsDict);
            commands.Remove(RealtimeCommand.Dim10);
            commands.Remove(RealtimeCommand.Dim20);
            commands.Remove(RealtimeCommand.Dim30);
            commands.Remove(RealtimeCommand.Dim90);
            var commandItem = commands.ElementAt(random.Next(0, commands.Count - 1));
            ExecuteCommand(commandItem.Key);

            return commandItem.Key;
        }

        /// <summary>
        ///  Execute random dimming except ON, OFF by drag and drop cursor(10%, 20%, 30%, % 40%, 50%, 60%, 70%, 80%)
        /// </summary>
        public RealtimeCommand ExecuteRandomDimming10To80ByCursor()
        {
            var random = new Random();
            var commands = new Dictionary<RealtimeCommand, CommandModel>(CommandsDict);
            commands.Remove(RealtimeCommand.DimOn);
            commands.Remove(RealtimeCommand.DimOff);
            commands.Remove(RealtimeCommand.Dim80);
            var commandItem = commands.ElementAt(random.Next(0, commands.Count - 1));
            var top = GetIndicatorCursorPositionValue();
            realtimeJaugeCursorCanvas.DragAndDropToOffsetByJS(commandItem.Value.OffsetX, commandItem.Value.OffsetY - top);

            return commandItem.Key;
        }        

        /// <summary>
        /// Execute random dimming by drag and drop cursor (any value except ON, OFF)
        /// </summary>
        public void ExecuteRandomDimmingByCursor()
        {
            var random = new Random();
            var topValue = realtimeJaugeCursorCanvas.GetStyleAttr("top");
            int value = (int)float.Parse(topValue.Replace("px", string.Empty));
            var offsetY = random.Next(0 - value, 170 - value);
            realtimeJaugeCursorCanvas.DragAndDropToOffsetByJS(0, offsetY);
        }       

        public void WaitForInputsOutputsPanelDisplayed()
        {
            Wait.ForElementStyle(By.CssSelector("[id$='widgetPanel'] [id$='status-panel']"), "top: -2px");
        }

        public void WaitForInputsOutputsPanelDisappeared()
        {
            Wait.ForElementStyle(By.CssSelector("[id$='widgetPanel'] [id$='status-panel']"), "top: -240px");
        }

        public void WaitForNameText(string controllerName)
        {
            Wait.ForElementInvisible(By.CssSelector("[id$='widgetPanel'] [id$='loader-spin']"));
            Wait.ForElementText(By.CssSelector("[id$='widgetPanel'] .controller [id$='recto'] .controller-title"), controllerName);
        }

        public void WaitForInformationWidgetPanelDisplayed()
        {
            Wait.ForElementInvisible(By.CssSelector("[id$='widgetPanel'] [id$='recto']"));
            Wait.ForElementDisplayed(By.CssSelector("[id$='widgetPanel'] [id$='verso']"));
            Wait.ForMilliseconds(500);
        }

        public void WaitInformationWidgetPanelDisappeared()
        {
            Wait.ForElementDisplayed(By.CssSelector("[id$='widgetPanel'] [id$='recto']"));
            Wait.ForElementInvisible(By.CssSelector("[id$='widgetPanel'] [id$='verso']"));
            Wait.ForMilliseconds(500);
        }

        public override bool IsIndicatorCursorVisible()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='widgetPanel'] .controller [id$='jauge-cursor']"));
        }

        public bool IsAutomationButtonVisible()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='widgetPanel'] [id*= 'wholeSegment'] > button"));
        }        

        public string GetLatitubeText()
        {
            var latitubeText = GetGpsPositionLatitudeText();
            return latitubeText.SplitAndGetAt(new char[] { ':' }, 0);
        }

        public string GetLatitubeValue()
        {
            var latitubeText = GetGpsPositionLatitudeText();
            return latitubeText.SplitAndGetAt(new char[] { ':' }, 1);
        }

        public string GetLongitubeText()
        {
            var longitubeText = GetGpsPositionLongitubeText();
            return longitubeText.SplitAndGetAt(new char[] { ':' }, 0);
        }

        public string GetLongitubeValue()
        {
            var longitubeText = GetGpsPositionLongitubeText();
            return longitubeText.SplitAndGetAt(new char[] { ':' }, 1);
        }

        public int GetIndicatorCursorPositionValue()
        {
            var topValue = realtimeJaugeCursorCanvas.GetStyleAttr("top");
            int value = (int)float.Parse(topValue.Replace("px", string.Empty));
            return value;
        }

        #endregion //Business methods

        public override void WaitForCommandCompleted()
        {
            Wait.ForProgressCompleted();
            Wait.ForSeconds(1);
        }

        public override void WaitForPanelLoaded()
        {
            Wait.ForElementInvisible(By.CssSelector("[id$='widgetPanel'] [id$='loader-spin']"));
        }
    }
}
