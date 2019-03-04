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
    public class StreetlightWidgetPanel : DeviceWidgetPanelBase
    {
        #region Variables
       

        #endregion //Variables

        #region IWebElements        

        #region Main form

        #region Header

        #region Status Panel

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] [id$='status-panel'] [id$='status-tab']")]
        private IWebElement statusButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] [id$='status-panel'] [id$='status-tab'] > div")]
        private IWebElement statusLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] [id$='status-panel'] [id$='status-tab'] > img")]
        private IWebElement statusIcon;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] [id$='status-panel'] [id$='status-content'] > div:nth-child(1) > .slv-label")]
        private IWebElement statusLampFailureLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] [id$='status-panel'] [id$='status-content'] > div:nth-child(1) > img")]
        private IWebElement statusLampFailureIcon;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] [id$='status-panel'] [id$='status-content'] > div:nth-child(2) > .slv-label")]
        private IWebElement statusLostCommunicationLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] [id$='status-panel'] [id$='status-content'] > div:nth-child(2) > img")]
        private IWebElement statusLostCommunicationIcon;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] [id$='status-panel'] [id$='status-content'] > div:nth-child(3) > .slv-label")]
        private IWebElement statusNodeFailureLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] [id$='status-panel'] [id$='status-content'] > div:nth-child(3) > img")]
        private IWebElement statusNodeFailureIcon;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] [id$='status-panel'] [id$='status-content'] > div:nth-child(4) > .slv-label")]
        private IWebElement statusUnknownIdentifierLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] [id$='status-panel'] [id$='status-content'] > div:nth-child(4) > img")]
        private IWebElement statusUnknownIdentifierIcon;

        #endregion //Status Panel

        #endregion //Header

        #region Realtime        

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] .streetlight button[title='ON']")]
        private IWebElement commandOnButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] .streetlight button[title='90%']")]
        private IWebElement commandDim90Button;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] .streetlight button[title='80%']")]
        private IWebElement commandDim80Button;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] .streetlight button[title='70%']")]
        private IWebElement commandDim70Button;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] .streetlight button[title='60%']")]
        private IWebElement commandDim60Button;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] .streetlight button[title='50%']")]
        private IWebElement commandDim50Button;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] .streetlight button[title='40%']")]
        private IWebElement commandDim40Button;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] .streetlight button[title='30%']")]
        private IWebElement commandDim30Button;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] .streetlight button[title='20%']")]
        private IWebElement commandDim20Button;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] .streetlight button[title='10%']")]
        private IWebElement commandDim10Button;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] .streetlight button[title='OFF']")]
        private IWebElement commandOffButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] .streetlight [id$='jauge-indicator']")]
        private IWebElement realtimeJaugeIndicatorCanvas;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] .streetlight [id$='jauge-cursor']")]
        private IWebElement realtimeJaugeCursorCanvas;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] .streetlight [id$='jauge-indicator-value']")]
        private IWebElement realtimeFeedbackValueLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] .streetlight [id$='jauge-indicator-title']")]
        private IWebElement realtimeFeedbackLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] .streetlight [id$='jauge-consign-value']")]
        private IWebElement realtimeCommandValueLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] .streetlight [id$='jauge-consign-title']")]
        private IWebElement realtimeCommandLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] .streetlight [id$='jauge-consign-input']")]
        private IWebElement realtimeCommandInput;  

        #endregion //Realtime

        #region Metering

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] .streetlight .metering-panel-title")]
        private IWebElement meteringTitleLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] .streetlight div:nth-child(1) > div.streetlight-metering-value")]
        private IWebElement LampBurningHoursValueLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] .streetlight div:nth-child(2) > div.streetlight-metering-value")]
        private IWebElement lampEnergyValueLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] .streetlight div:nth-child(3) > div.streetlight-metering-value")]
        private IWebElement lampLevelCommandValueLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] .streetlight div:nth-child(4) > div.streetlight-metering-value")]
        private IWebElement lampLevelFeedbackValueLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] .streetlight div:nth-child(5) > div.streetlight-metering-value")]
        private IWebElement lampPowerValueLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] .streetlight div:nth-child(6) > div.streetlight-metering-value")]
        private IWebElement lampSwitchFeedbackValueLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] .streetlight div:nth-child(7) > div.streetlight-metering-value")]
        private IWebElement mainsCurrentValueLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] .streetlight div:nth-child(8) > div.streetlight-metering-value")]
        private IWebElement MainsVoltageValueLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] .streetlight div:nth-child(9) > div.streetlight-metering-value")]
        private IWebElement nodeFailureMessageValueLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] .streetlight div:nth-child(10) > div.streetlight-metering-value")]
        private IWebElement powerFactorValueLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] .streetlight div:nth-child(11) > div.streetlight-metering-value")]
        private IWebElement temperatureValueLabel;

        #endregion //Metering

        #endregion //Main form       

        #endregion //IWebElements

        #region Constructor

        public StreetlightWidgetPanel(IWebDriver driver, PageBase page) : base(driver, page)
        {
            PageFactory.InitElements(this, new RetryingElementLocator(driver, TimeSpan.FromMilliseconds(Settings.LocatorRetryingTimeout), TimeSpan.FromMilliseconds(Settings.LocatorPollingInterval)));
            WaitForPanelLoaded();
        }

        #endregion //Constructor

        #region Properties

        #endregion //Properties

        #region Basic methods

        #region Actions        

        #region Main form

        #region Header

        #region Status Panel

        /// <summary>
        /// Click 'Status' button
        /// </summary>
        public void ClickStatusButton()
        {
            statusButton.ClickEx();
        }

        #endregion //Status Panel

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
        /// <param name="pressEnterKey"></param>
        public void EnterRealtimeCommandInput(string value, bool pressEnterKey = false)
        {
            realtimeCommandInput.Enter(value, pressEnterKey: pressEnterKey);
        }

        /// <summary>
        /// Click to edit 'Command' value
        /// </summary>
        /// <param name="value"></param>
        public void ClickToEditCommandValue()
        {
            realtimeCommandValueLabel.ClickEx();
        }

        #endregion //Realtime

        #region Metering

        #endregion //Metering

        #endregion //Main form

        #region Information form

        #endregion //Information form

        #endregion //Actions

        #region Get methods

        #region Main form

        #region Header

        #region Status Panel

        /// <summary>
        /// Get 'Status' label text
        /// </summary>
        /// <returns></returns>
        public string GetStatusText()
        {
            return statusLabel.Text;
        }

        /// <summary>
        /// Get 'StatusIcon' input value
        /// </summary>
        /// <returns></returns>
        public string GetStatusIconValue()
        {
            return statusIcon.IconValue();
        }

        /// <summary>
        /// Get 'StatusLampFailure' label text
        /// </summary>
        /// <returns></returns>
        public string GetStatusLampFailureText()
        {
            return statusLampFailureLabel.Text;
        }

        /// <summary>
        /// Get 'StatusLampFailureIcon' input value
        /// </summary>
        /// <returns></returns>
        public string GetStatusLampFailureIconValue()
        {
            return statusLampFailureIcon.IconValue();
        }

        /// <summary>
        /// Get 'StatusLostCommunication' label text
        /// </summary>
        /// <returns></returns>
        public string GetStatusLostCommunicationText()
        {
            return statusLostCommunicationLabel.Text;
        }

        /// <summary>
        /// Get 'StatusLostCommunicationIcon' input value
        /// </summary>
        /// <returns></returns>
        public string GetStatusLostCommunicationIconValue()
        {
            return statusLostCommunicationIcon.IconValue();
        }

        /// <summary>
        /// Get 'StatusNodeFailure' label text
        /// </summary>
        /// <returns></returns>
        public string GetStatusNodeFailureText()
        {
            return statusNodeFailureLabel.Text;
        }

        /// <summary>
        /// Get 'StatusNodeFailureIcon' input value
        /// </summary>
        /// <returns></returns>
        public string GetStatusNodeFailureIconValue()
        {
            return statusNodeFailureIcon.IconValue();
        }

        /// <summary>
        /// Get 'StatusUnknownIdentifier' label text
        /// </summary>
        /// <returns></returns>
        public string GetStatusUnknownIdentifierText()
        {
            return statusUnknownIdentifierLabel.Text;
        }

        /// <summary>
        /// Get 'StatusUnknownIdentifierIcon' input value
        /// </summary>
        /// <returns></returns>
        public string GetStatusUnknownIdentifierIconValue()
        {
            return statusUnknownIdentifierIcon.IconValue();
        }

        #endregion //Status Panel

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

        /// <summary>
        /// Get 'JaugeCursor' style top value
        /// </summary>
        /// <returns></returns>
        public string GetJaugeCursorStyleTopValue()
        {
            return realtimeJaugeCursorCanvas.GetStyleAttr("top").Replace("px", string.Empty);
        }        

        #endregion //Realtime

        #region Metering

        /// <summary>
        /// Get 'MeteringTitle' label text
        /// </summary>
        /// <returns></returns>
        public string GetMeteringTitleText()
        {
            return meteringTitleLabel.Text;
        }        

        /// <summary>
        /// Get 'LampBurningHoursValue' label text
        /// </summary>
        /// <returns></returns>
        public string GetLampBurningHoursValueText()
        {
            return LampBurningHoursValueLabel.Text;
        }

        /// <summary>
        /// Get 'LampEnergyValue' label text
        /// </summary>
        /// <returns></returns>
        public string GetLampEnergyValueText()
        {
            return lampEnergyValueLabel.Text;
        }

        /// <summary>
        /// Get 'LampLevelCommandValue' label text
        /// </summary>
        /// <returns></returns>
        public string GetLampLevelCommandValueText()
        {
            return lampLevelCommandValueLabel.Text;
        }

        /// <summary>
        /// Get 'LampLevelFeedbackValue' label text
        /// </summary>
        /// <returns></returns>
        public string GetLampLevelFeedbackValueText()
        {
            return lampLevelFeedbackValueLabel.Text;
        }

        /// <summary>
        /// Get 'LampPowerValue' label text
        /// </summary>
        /// <returns></returns>
        public string GetLampPowerValueText()
        {
            return lampPowerValueLabel.Text;
        }

        /// <summary>
        /// Get 'LampSwitchFeedbackValue' label text
        /// </summary>
        /// <returns></returns>
        public string GetLampSwitchFeedbackValueText()
        {
            return lampSwitchFeedbackValueLabel.Text;
        }

        /// <summary>
        /// Get 'MainsCurrentValue' label text
        /// </summary>
        /// <returns></returns>
        public string GetMainsCurrentValueText()
        {
            return mainsCurrentValueLabel.Text;
        }

        /// <summary>
        /// Get 'MainsVoltageValue' label text
        /// </summary>
        /// <returns></returns>
        public string GetMainsVoltageValueText()
        {
            return MainsVoltageValueLabel.Text;
        }

        /// <summary>
        /// Get 'NodeFailureMessageValue' label text
        /// </summary>
        /// <returns></returns>
        public string GetNodeFailureMessageValueText()
        {
            return nodeFailureMessageValueLabel.Text;
        }

        /// <summary>
        /// Get 'PowerFactorValue' label text
        /// </summary>
        /// <returns></returns>
        public string GetPowerFactorValueText()
        {
            return powerFactorValueLabel.Text;
        }

        /// <summary>
        /// Get 'TemperatureValue' label text
        /// </summary>
        /// <returns></returns>
        public string GetTemperatureValueText()
        {
            return temperatureValueLabel.Text;
        }

        #endregion //Metering

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
            switch(command)
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
        /// Execute random dimming (except 10%, 20%, 30%)
        /// </summary>
        /// <returns></returns>
        public RealtimeCommand ExecuteRandomDimming()
        {
            var random = new Random();
            var commands = new Dictionary<RealtimeCommand, CommandModel>(CommandsDict);
            commands.Remove(RealtimeCommand.Dim10);
            commands.Remove(RealtimeCommand.Dim20);
            commands.Remove(RealtimeCommand.Dim30);
            var commandItem = commands.ElementAt(random.Next(0, commands.Count - 1));
            ExecuteCommand(commandItem.Key);

            return commandItem.Key;
        }

        /// <summary>
        /// Execute random dimming (except 10%, 20%, 30% and except commands in parameter)
        /// </summary>
        /// <returns></returns>
        public RealtimeCommand ExecuteRandomDimming(params RealtimeCommand[] exceptCommands)
        {
            var random = new Random();
            var commands = new Dictionary<RealtimeCommand, CommandModel>(CommandsDict);
            commands.Remove(RealtimeCommand.Dim10);
            commands.Remove(RealtimeCommand.Dim20);
            commands.Remove(RealtimeCommand.Dim30);
            foreach (var command in exceptCommands)
            {
                commands.Remove(command);
            }

            var commandItem = commands.ElementAt(random.Next(0, commands.Count - 1));
            ExecuteCommand(commandItem.Key);

            return commandItem.Key;
        }

        /// <summary>
        ///  Execute random dimming except ON, OFF by drag and drop cursor(10%, 20%, 30%, % 40%, 50%, 60%, 70%, 80%, 90%)
        /// </summary>
        public RealtimeCommand ExecuteRandomDimming10To90ByCursor()
        {
            var random = new Random();
            var commands = new Dictionary<RealtimeCommand, CommandModel>(CommandsDict);
            commands.Remove(RealtimeCommand.DimOn);
            commands.Remove(RealtimeCommand.DimOff);
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

        /// <summary>
        /// Check if Commit Command is editable
        /// </summary>
        /// <returns></returns>
        public bool IsCommitCommandEditable()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='widgetPanel'] .streetlight [id$='jauge-consign-input-container']"));
        }

        public override bool IsIndicatorCursorVisible()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='widgetPanel'] .streetlight [id$='jauge-cursor']"));
        }

        public bool IsStatusPanelShown()
        {
            var statusPanelTopValue = Driver.FindElement(By.CssSelector("[id$='widgetPanel'] [id$='status-panel']")).GetStyleAttr("top");
            return statusPanelTopValue.Equals("-2px");
        }

        public void WaitForStatusPanelDisplayed()
        {
            Wait.ForElementStyle(By.CssSelector("[id$='widgetPanel'] [id$='status-panel']"), "top: -2px");
        }

        public void WaitForStatusPanelDisappeared()
        {
            Wait.ForElementStyle(By.CssSelector("[id$='widgetPanel'] [id$='status-panel']"), "top: -240px");
        }

        public void WaitForNameText(string streetlightName)
        {
            Wait.ForElementInvisible(By.CssSelector("[id$='widgetPanel'] [id$='loader-spin']"));
            Wait.ForElementText(By.CssSelector("[id$='widgetPanel'] .recto-div .widget-bottom-title .device-name"), streetlightName);
        }

        public void WaitForLastUpdatedTimeText()
        {
            Wait.ForElementInvisible(By.CssSelector("[id$='widgetPanel'] [id$='loader'].loader"));
            Wait.ForElementText(By.CssSelector("[id$='widgetPanel'] .recto-div .widget-layout-refresh-time"));
        }       

        public int GetIndicatorCursorPositionValue()
        {
            var topValue = realtimeJaugeCursorCanvas.GetStyleAttr("top");
            int value = (int)float.Parse(topValue.Replace("px", string.Empty));
            return value;
        }

        public void WaitForInformationWidgetPanelDisplayed()
        {
            Wait.ForElementInvisible(By.CssSelector("[id$='widgetPanel'] .recto-div"));
            Wait.ForElementDisplayed(By.CssSelector("[id$='widgetPanel'] .verso-div"));
            Wait.ForMilliseconds(500);
        }

        public void WaitInformationWidgetPanelDisappeared()
        {
            Wait.ForElementDisplayed(By.CssSelector("[id$='widgetPanel'] .recto-div"));
            Wait.ForElementInvisible(By.CssSelector("[id$='widgetPanel'] .verso-div"));
            Wait.ForMilliseconds(500);
        }

        #endregion //Business methods

        public override void WaitForCommandCompleted()
        {
            Wait.ForProgressCompleted();
            Wait.ForSeconds(1);
        }

        public override void WaitForPanelLoaded()
        {
            base.WaitForPanelLoaded();
        }

        public override void WaitForPreviousActionComplete()
        {
            base.WaitForPreviousActionComplete();
        }
    }
}
