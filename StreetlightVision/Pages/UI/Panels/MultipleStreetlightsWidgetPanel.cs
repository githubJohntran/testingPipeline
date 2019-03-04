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
    public class MultipleStreetlightsWidgetPanel: DeviceWidgetPanelBase
    {
        #region Variables
       

        #endregion //Variables

        #region IWebElements        

        #region Main form

        #region Header        

        #endregion //Header

        #region Realtime        

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] [id*='streetlights'][id$='commands'] button:first-child")]
        private IWebElement commandOnButton;  

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] [id*='streetlights'][id$='commands'] button:last-child")]
        private IWebElement commandOffButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] [id*='streetlights'][id$='jauge-indicator']")]
        private IWebElement realtimeJaugeIndicatorCanvas;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] [id*='streetlights'][id$='jauge-cursor']")]
        private IWebElement realtimeJaugeCursorCanvas;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] [id*='streetlights'][id$='jauge-indicator-value']")]
        private IWebElement realtimeFeedbackValueLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] [id*='streetlights'][id$='jauge-indicator-title']")]
        private IWebElement realtimeFeedbackLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] [id*='streetlights'][id$='jauge-consign-value']")]
        private IWebElement realtimeCommandValueLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] [id*='streetlights'][id$='jauge-consign-title']")]
        private IWebElement realtimeCommandLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] [id*='streetlights'][id$='jauge-consign-input']")]
        private IWebElement realtimeCommandInput;     

        #endregion //Realtime

        #endregion //Main form

        #endregion //IWebElements

        #region Constructor

        public MultipleStreetlightsWidgetPanel(IWebDriver driver, PageBase page) : base(driver, page)
        {
            PageFactory.InitElements(this, new RetryingElementLocator(driver, TimeSpan.FromMilliseconds(Settings.LocatorRetryingTimeout), TimeSpan.FromMilliseconds(Settings.LocatorPollingInterval)));
            WaitForPanelLoaded();
        }

        #endregion //Constructor

        #region Properties

        #endregion //Properties

        #region Basic methods   

        #region Actions

        /// <summary>
        /// Click 'CommandOn' button
        /// </summary>
        public void ClickCommandOnButton()
        {
            commandOnButton.ClickEx();
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

        #endregion //Actions

        #region Get methods

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
        /// Click to edit 'Command' value
        /// </summary>
        /// <param name="value"></param>
        public void ClickToEditCommandValue()
        {
            realtimeCommandValueLabel.ClickEx();
        }

        #endregion //Get methods

        #endregion //Basic methods

        #region Business methods        

        /// <summary>
        /// Select a tab with a specific name
        /// </summary>
        /// <param name="name"></param>
        public void SelectTab(string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get all item list of a specific tab name
        /// </summary>
        /// <param name="tabName"></param>
        /// <returns></returns>
        public string GetItemsList(string tabName)
        {
            throw new NotImplementedException();
        }

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
                case RealtimeCommand.DimOff:
                    ClickCommandOffButton();
                    break;
            }
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

        public int GetIndicatorCursorPositionValue()
        {
            var topValue = realtimeJaugeCursorCanvas.GetStyleAttr("top");
            int value = (int)float.Parse(topValue.Replace("px", string.Empty));
            return value;
        }

        /// <summary>
        /// Check if Commit Command is editable
        /// </summary>
        /// <returns></returns>
        public bool IsCommitCommandEditable()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='widgetPanel'] [id*='streetlights'][id$='jauge-consign-input-container']"));
        }

        public bool IsStatusIconQuestionDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='widgetPanel'] [id$='header'] .streetlight-header-left-button.icon-unknown"));
        }

        public bool IsStatusIconOkDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='widgetPanel'] [id$='header'] .streetlight-header-left-button.icon-ok"));
        }

        #endregion //Business methods

        public override void WaitForCommandCompleted()
        {
            Wait.ForProgressCompleted();
            Wait.ForSeconds(2);
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
