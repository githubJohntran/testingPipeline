using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using StreetlightVision.Extensions;
using StreetlightVision.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StreetlightVision.Pages.UI
{
    public class AlarmEditorPanel : PanelBase
    {
        #region Variables

        #endregion //Variables

        #region IWebElements

        #region Header toolbar buttons

        [FindsBy(How = How.CssSelector, Using = "[id='tb_alarmmanagerReportButtons_item_cancel'] > table.w2ui-button")]
        private IWebElement cancelButton;

        [FindsBy(How = How.CssSelector, Using = "[id='tb_alarmmanagerReportButtons_item_save'] > table.w2ui-button")]
        private IWebElement saveButton;

        [FindsBy(How = How.CssSelector, Using = "[id='tb_alarmmanagerReportButtons_item_delete'] > table.w2ui-button")]
        private IWebElement deleteButton;

        #endregion //Header toolbar buttons

        #region Content Identity

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-content-name-label'] .slv-label")]
        private IWebElement nameLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-content-name-field']")]
        private IWebElement nameInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-content-trigger-label'] .slv-label")]
        private IWebElement typeLabel;

        [FindsBy(How = How.CssSelector, Using = "div[id$='editor-content-trigger-field']")]
        private IWebElement typeDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-content-action-label'] .slv-label")]
        private IWebElement actionLabel;

        [FindsBy(How = How.CssSelector, Using = "div[id$='editor-content-action-field']")]
        private IWebElement actionDropDown;

        #endregion //Content Identity

        #region General tab        

        #region Configuration group

        #endregion //Configuration group

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-alarmPriority'] .slv-label")]
        private IWebElement priorityLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-alarmPriority-field']")]
        private IWebElement priorityNumbericInput;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-newAlarmWhenAcknowledged'] .slv-label")]
        private IWebElement newAlarmIfRetriggerLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-newAlarmWhenAcknowledged-field']")]
        private IWebElement newAlarmIfRetriggerCheckbox;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-autoAcknowledge'] .slv-label")]
        private IWebElement autoAcknowledgeLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-autoAcknowledge-field']")]
        private IWebElement autoAcknowledgeCheckbox;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-disabled'] .slv-label")]
        private IWebElement disabledLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-disabled-field']")]
        private IWebElement disabledCheckbox;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-refreshRate'] .slv-label")]
        private IWebElement refreshRateLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-refreshRate-field']")]
        private IWebElement refreshRateDropDown;

        #endregion //General tab

        #region Trigger condition tab

        #region Configuration group

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-fullFilledMessageTemplate'] .slv-label")]
        private IWebElement messageLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-fullFilledMessageTemplate-field']")]
        private IWebElement messageInput;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-controllerIds'] .slv-label, [id='editor-property-triggerCondition-controllerStrIds'] .slv-label")]
        private IWebElement controllersLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-controllerIds-field']")]
        private IWebElement controllersDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-ioName'] .slv-label")]
        private IWebElement inputOutputLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-ioName-field']")]
        private IWebElement inputOutputDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-sunEventValue'] .slv-label")]
        private IWebElement sunEventLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-sunEventValue-field']")]
        private IWebElement sunEventDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-delay'] .slv-label")]
        private IWebElement delayLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-delay-field']")]
        private IWebElement delayNumbericInput;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-controllerStrIds-field'], [id='editor-property-triggerCondition-controllerIds-field']")]
        private IWebElement controllersListDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-controllerStrIds-field'] > button, [id='editor-property-triggerCondition-controllerIds-field'] > button")]
        private IWebElement controllersSelectAllButton;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-io1Name-field'] .slv-label")]
        private IWebElement inputOutput1Label;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-io1Name-field']")]
        private IWebElement inputOutput1DropDown;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-operator1Value'] .slv-label")]
        private IWebElement operator1Label;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-operator1Value-field']")]
        private IWebElement operator1DropDown;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-io1TestValueName'] .slv-label")]
        private IWebElement testValue1Label;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-operator1Value-field']")]
        private IWebElement testValue1DropDown;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-io2Name-field'] .slv-label")]
        private IWebElement inputOutput2Label;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-io2Name-field']")]
        private IWebElement inputOutput2DropDown;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-operator2Value'] .slv-label")]
        private IWebElement operator2Label;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-operator2Value-field']")]
        private IWebElement operator2DropDown;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-io2TestValueName'] .slv-label")]
        private IWebElement testValue2Label;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-operator2Value-field']")]
        private IWebElement testValue2DropDown;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-io3Name-field'] .slv-label")]
        private IWebElement inputOutput3Label;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-io3Name-field']")]
        private IWebElement inputOutput3DropDown;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-operator3Value'] .slv-label")]
        private IWebElement operator3Label;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-operator3Value-field']")]
        private IWebElement operator3DropDown;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-io3TestValueName'] .slv-label")]
        private IWebElement testValue3Label;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-operator3Value-field']")]
        private IWebElement testValue3DropDown;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-logicalOperatorValue'] .slv-label")]
        private IWebElement logicalOperatorLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-logicalOperatorValue-field']")]
        private IWebElement logicalOperatorDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-operatorValue'] .slv-label")]
        private IWebElement operator1_2Label;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-operatorValue-field']")]
        private IWebElement operator1_2DropDown;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-operator2Value'] .slv-label")]
        private IWebElement operator1_3Label;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-operator2Value-field']")]
        private IWebElement operator1_3DropDown;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-io1Name-field'] .slv-label")]
        private IWebElement firstIOLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-io1Name-field']")]
        private IWebElement firstIODropDown;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-io2Name-field'] .slv-label")]
        private IWebElement secondIOLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-io2Name-field']")]
        private IWebElement secondIODropDown;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-operatorValue'] .slv-label")]
        private IWebElement operatorLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-operatorValue-field']")]
        private IWebElement operatorDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-inputName'] .slv-label")]
        private IWebElement inputNameLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-inputName-field']")]
        private IWebElement inputNameDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-inputValue'] .slv-label")]
        private IWebElement inputValueLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-inputValue-field']")]
        private IWebElement inputValueDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-hoursDelay'] .slv-label")]
        private IWebElement hoursDelayLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-hoursDelay-field']")]
        private IWebElement hoursDelayNumbericInput;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-hoursCheckInterval'] .slv-label")]
        private IWebElement hoursCheckIntervalLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-hoursCheckInterval-field']")]
        private IWebElement checkHoursIntervalDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-deviceIds'] .slv-label, [id='editor-property-triggerCondition-deviceId'] .slv-label")]
        private IWebElement devicesLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-deviceIds-field']")]
        private IWebElement devicesListDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-meaningCategories'] .slv-label")]
        private IWebElement variablesTypeLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-meaningCategories-field']")]
        private IWebElement variablesTypeListDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-delayTriggeringValueInMinutes'] .slv-label")]
        private IWebElement criticalDelayLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-delayTriggeringValueInMinutes-field']")]
        private IWebElement criticalDelayDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-timeMode'] .slv-label")]
        private IWebElement timestampModeLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-timeMode-field']")]
        private IWebElement timestampModeDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-triggeredDevicesRatioLimitPercent'] .slv-label")]
        private IWebElement criticalRatioLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-triggeredDevicesRatioLimitPercent-field']")]
        private IWebElement criticalRatioNumericInput;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-deviceId-field']")]
        private IWebElement deviceDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-criticityLevel'] .slv-label")]
        private IWebElement criticityLevelLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-criticityLevel-field']")]
        private IWebElement criticityLevelDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-meteringStrId'] .slv-label")]
        private IWebElement meteringLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-meteringStrId-field']")]
        private IWebElement meteringDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-ignoreOperatorValue'] .slv-label")]
        private IWebElement ignoreOperatorLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-ignoreOperatorValue-field']")]
        private IWebElement ignoreOperatorDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-ignoreValue'] .slv-label")]
        private IWebElement ignoreValueLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-ignoreValue-field']")]
        private IWebElement ignoreValueNumericInput;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-analysisPeriod'] .slv-label")]
        private IWebElement analysisPeriodLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-analysisPeriod-field']")]
        private IWebElement analysisPeriodDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-analyticModeValue'] .slv-label")]
        private IWebElement analyticModeLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-analyticModeValue-field']")]
        private IWebElement analyticModeDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-percentageDifferenceTriggerValue'] .slv-label")]
        private IWebElement percentageDifferenceTriggerLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-percentageDifferenceTriggerValue-field']")]
        private IWebElement percentageDifferenceTriggerNumericInput;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-fromDaytimeStr'] .slv-label")]
        private IWebElement fromDaytimeLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-fromDaytimeStr-field']")]
        private IWebElement fromDaytimeInput;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-toDaytimeStr'] .slv-label")]
        private IWebElement toDaytimeLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-toDaytimeStr-field']")]
        private IWebElement toDaytimeInput;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-criticalFailureRatio'] .slv-label")]
        private IWebElement criticalFailureRatioLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-criticalFailureRatio-field']")]
        private IWebElement criticalFailureRatioNumericInput;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-triggeringMeaningsStrIds'] .slv-label")]
        private IWebElement failuresLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-triggeringMeaningsStrIds-field']")]
        private IWebElement failuresListDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-criticalCount'] .slv-label")]
        private IWebElement criticalCountLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-criticalCount-field']")]
        private IWebElement criticalCountNumericInput;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-radius'] .slv-label")]
        private IWebElement radiusLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-radius-field']")]
        private IWebElement radiusNumericInput;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-threshold'] .slv-label")]
        private IWebElement thresholdLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-threshold-field']")]
        private IWebElement thresholdNumericInput;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-alarmBundleIds'] .slv-label")]
        private IWebElement alarmsLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-alarmBundleIds-field']")]
        private IWebElement alarmsDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-logicalExpression'] .slv-label")]
        private IWebElement logicalExpressionLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-logicalExpression-field']")]
        private IWebElement logicalExpressionInput;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-triggerOperatorValue'] .slv-label")]
        private IWebElement triggeringOperatorLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-triggerOperatorValue-field']")]
        private IWebElement triggeringOperatorDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-triggerValue'] .slv-label")]
        private IWebElement triggeringValueLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-triggerValue-field']")]
        private IWebElement triggeringValueNumericInput;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-delay1'] .slv-label")]
        private IWebElement analysisTimeT1Label;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-delay1-field']")]
        private IWebElement analysisTimeT1DropDown;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-delay2'] .slv-label")]
        private IWebElement alarmTimeT1Label;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-delay2-field']")]
        private IWebElement alarmTimeT1DropDown;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-counterIds'] .slv-label")]
        private IWebElement metersLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-triggerCondition-counterIds-field']")]
        private IWebElement metersListDropDown;

        #endregion //Configuration group

        #endregion //Trigger condition tab

        #region Actions tab

        #region Notify by Email

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-content-properties-group-2-content']")]
        private IWebElement emailContainer;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-action0-from'] .slv-label")]
        private IWebElement fromLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-action0-from-field']")]
        private IWebElement fromInput;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-action0-to'] .slv-label")]
        private IWebElement toLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-action0-to-field']")]
        private IWebElement toListDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-action0-to-field'] > button")]
        private IWebElement toSelectAllButton;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-action0-subject'] .slv-label")]
        private IWebElement subjectLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-action0-subject-field']")]
        private IWebElement subjectInput;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-action0-message'] .slv-label")]
        private IWebElement emailMessageLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-action0-message-field']")]
        private IWebElement emailMessageInput;

        #endregion //Notify by Email

        #region Send HTML request

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-action0-httpUri'] .slv-label")]
        private IWebElement urlLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='#editor-property-action0-httpUri-field']")]
        private IWebElement urlInput;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-action0-httpUser'] .slv-label")]
        private IWebElement userLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='#editor-property-action0-httpUser-field']")]
        private IWebElement userInput;

        [FindsBy(How = How.CssSelector, Using = "[id='editor-property-action0-httpPassword'] .slv-label")]
        private IWebElement passwordLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='#editor-property-action0-httpPassword-field']")]
        private IWebElement passwordInput;

        #endregion //Send HTML request

        #region Send report

        [FindsBy(How = How.CssSelector, Using = "[id$='scheduledReportId'] .slv-label.editor-label")]
        private IWebElement reportLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-htmlFormat-field-checkbox']")]
        private IWebElement reportDropDown;

        #endregion //Send report

        #endregion //Actions tab

        [FindsBy(How = How.CssSelector, Using = "[id^='tabs'][id*='geozone-editor-content-properties-tab-'] > div")]
        private IList<IWebElement> tabsList;

        #endregion //IWebElements

        #region Constructor

        public AlarmEditorPanel(IWebDriver driver, PageBase page) : base(driver, page)
        {
            PageFactory.InitElements(this, new RetryingElementLocator(driver, TimeSpan.FromMilliseconds(Settings.LocatorRetryingTimeout), TimeSpan.FromMilliseconds(Settings.LocatorPollingInterval)));

            WaitForPanelLoaded();
        }

        #endregion //Constructor

        #region Basic methods      

        #region Actions

        #region Header toolbar buttons

        /// <summary>
        /// Click 'Cancel' button
        /// </summary>
        public void ClickCancelButton()
        {
            cancelButton.ClickEx();
        }

        /// <summary>
        /// Click 'Save' button
        /// </summary>
        public void ClickSaveButton()
        {
            saveButton.ClickEx();
        }

        /// <summary>
        /// Click 'Delete' button
        /// </summary>
        public void ClickDeleteButton()
        {
            deleteButton.ClickEx();
        }

        #endregion //Header toolbar buttons

        #region Content Identity

        /// <summary>
        /// Enter a value for 'Name' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterNameInput(string value)
        {
            nameInput.Enter(value);
        }

        /// <summary>
        /// Select an item of 'Type' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectTypeDropDown(string value)
        {
            typeDropDown.Select(value);
        }

        /// <summary>
        /// Select an item of 'Action' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectActionDropDown(string value)
        {
            actionDropDown.Select(value);
        }

        #endregion //Content Identity

        #region General tab        

        #region Configuration group

        #endregion //Configuration group

        /// <summary>
        /// Enter a value for 'PriorityNumberic' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterPriorityNumbericInput(string value)
        {
            priorityNumbericInput.Enter(value);
        }

        /// <summary>
        /// Tick 'NewAlarmIfRetrigger' checkbox
        /// </summary>
        /// <param name="value"></param>
        public void TickNewAlarmIfRetriggerCheckbox(bool value)
        {
            newAlarmIfRetriggerCheckbox.Check(value);
        }

        /// <summary>
        /// Tick 'AutoAcknowledge' checkbox
        /// </summary>
        /// <param name="value"></param>
        public void TickAutoAcknowledgeCheckbox(bool value)
        {
            autoAcknowledgeCheckbox.Check(value);
        }

        /// <summary>
        /// Tick 'Disabled' checkbox
        /// </summary>
        /// <param name="value"></param>
        public void TickDisabledCheckbox(bool value)
        {
            disabledCheckbox.Check(value);
        }

        /// <summary>
        /// Select an item of 'RefreshRate' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectRefreshRateDropDown(string value)
        {
            refreshRateDropDown.Select(value);
        }

        #endregion //General tab

        #region Trigger condition tab

        #region Configuration group

        /// <summary>
        /// Enter a value for 'Message' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterMessageInput(string value)
        {
            messageInput.Enter(value);
        }

        /// <summary>
        /// Select an item of 'Controllers' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectControllersDropDown(string value)
        {
            controllersDropDown.Select(value);
        }

        /// <summary>
        /// Select an item of 'InputOutput' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectInputOutputDropDown(string value)
        {
            inputOutputDropDown.Select(value);
        }

        /// <summary>
        /// Select an item of 'SunEvent' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectSunEventDropDown(string value)
        {
            sunEventDropDown.Select(value);
        }

        /// <summary>
        /// Enter a value for 'DelayNumberic' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterDelayNumbericInput(string value)
        {
            delayNumbericInput.Enter(value);
        }

        /// <summary>
        /// Select an item of 'Controllers' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectControllersListDropDown(string value)
        {
            controllersListDropDown.Select(value);
        }

        /// <summary>
        /// Remove a selected item of 'Controllers' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void RemoveControllersListDropDownValue(string value)
        {
            controllersListDropDown.RemoveValueInList(value);
        }

        /// <summary>
        /// Click 'ControllersSelectAll' button
        /// </summary>
        public void ClickControllersSelectAllButton()
        {
            controllersSelectAllButton.ClickEx();
        }

        /// <summary>
        /// Select an item of 'InputOutput1' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectInputOutput1DropDown(string value)
        {
            inputOutput1DropDown.Select(value);
        }

        /// <summary>
        /// Select an item of 'Operator1' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectOperator1DropDown(string value)
        {
            operator1DropDown.Select(value);
        }

        /// <summary>
        /// Select an item of 'TestValue1' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectTestValue1DropDown(string value)
        {
            testValue1DropDown.Select(value);
        }

        /// <summary>
        /// Select an item of 'InputOutput2' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectInputOutput2DropDown(string value)
        {
            inputOutput2DropDown.Select(value);
        }

        /// <summary>
        /// Select an item of 'Operator2' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectOperator2DropDown(string value)
        {
            operator2DropDown.Select(value);
        }

        /// <summary>
        /// Select an item of 'TestValue2' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectTestValue2DropDown(string value)
        {
            testValue2DropDown.Select(value);
        }

        /// <summary>
        /// Select an item of 'InputOutput3' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectInputOutput3DropDown(string value)
        {
            inputOutput3DropDown.Select(value);
        }

        /// <summary>
        /// Select an item of 'Operator3' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectOperator3DropDown(string value)
        {
            operator3DropDown.Select(value);
        }

        /// <summary>
        /// Select an item of 'TestValue3' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectTestValue3DropDown(string value)
        {
            testValue3DropDown.Select(value);
        }

        /// <summary>
        /// Select an item of 'LogicalOperator' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectLogicalOperatorDropDown(string value)
        {
            logicalOperatorDropDown.Select(value);
        }

        /// <summary>
        /// Select an item of 'Operator1_2' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectOperator1_2DropDown(string value)
        {
            operator1_2DropDown.Select(value);
        }

        /// <summary>
        /// Select an item of 'Operator1_3' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectOperator1_3DropDown(string value)
        {
            operator1_3DropDown.Select(value);
        }

        /// <summary>
        /// Select an item of 'FirstIO' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectFirstIODropDown(string value)
        {
            firstIODropDown.Select(value);
        }

        /// <summary>
        /// Select an item of 'SecondIO' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectSecondIODropDown(string value)
        {
            secondIODropDown.Select(value);
        }

        /// <summary>
        /// Select an item of 'Operator' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectOperatorDropDown(string value)
        {
            operatorDropDown.Select(value);
        }

        /// <summary>
        /// Select an item of 'InputName' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectInputNameDropDown(string value)
        {
            inputNameDropDown.Select(value);
        }

        /// <summary>
        /// Select an item of 'InputValue' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectInputValueDropDown(string value)
        {
            inputValueDropDown.Select(value);
        }

        /// <summary>
        /// Enter a value for 'HoursDelayNumberic' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterHoursDelayNumbericInput(string value)
        {
            hoursDelayNumbericInput.Enter(value);
        }

        /// <summary>
        /// Select an item of 'HoursCheckInterval' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectCheckHoursIntervalDropDown(string value)
        {
            checkHoursIntervalDropDown.Select(value);
        }

        /// <summary>
        /// Select an item of 'Devices' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectDevicesListDropDown(string value)
        {
            devicesListDropDown.Select(value);
        }

        /// <summary>
        /// Remove a selected item of 'Devices' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void RemoveDevicesListDropDownValue(string value)
        {
            devicesListDropDown.RemoveValueInList(value);
        }

        /// <summary>
        /// Select an item of 'VariablesType' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectVariablesTypeListDropDown(string value)
        {
            variablesTypeListDropDown.Select(value);
        }

        /// <summary>
        /// Remove a selected item of 'VariablesType' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void RemoveVariablesTypeListDropDownValue(string value)
        {
            variablesTypeListDropDown.RemoveValueInList(value);
        }

        /// <summary>
        /// Select an item of 'CriticalDelay' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectCriticalDelayDropDown(string value)
        {
            criticalDelayDropDown.Select(value);
        }

        /// <summary>
        /// Select an item of 'TimestampMode' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectTimestampModeDropDown(string value)
        {
            timestampModeDropDown.Select(value);
        }

        /// <summary>
        /// Enter a value for 'CriticalRatio' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterCriticalRatioNumericInput(string value)
        {
            criticalRatioNumericInput.Enter(value);
        }

        /// <summary>
        /// Select an item of 'Device' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectDeviceDropDown(string value)
        {
            deviceDropDown.Select(value);
        }

        /// <summary>
        /// Select an item of 'CriticityLevel' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectCriticityLevelDropDown(string value)
        {
            criticityLevelDropDown.Select(value);
        }

        /// <summary>
        /// Select an item of 'Metering' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectMeteringDropDown(string value)
        {
            meteringDropDown.Select(value);
        }

        /// <summary>
        /// Select an item of 'IgnoreOperator' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectIgnoreOperatorDropDown(string value)
        {
            ignoreOperatorDropDown.Select(value);
        }

        /// <summary>
        /// Enter a value for 'IgnoreValue' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterIgnoreValueNumericInput(string value)
        {
            ignoreValueNumericInput.Enter(value);
        }

        /// <summary>
        /// Select an item of 'AnalysisPeriod' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectAnalysisPeriodDropDown(string value)
        {
            analysisPeriodDropDown.Select(value);
        }

        /// <summary>
        /// Select an item of 'AnalyticMode' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectAnalyticModeDropDown(string value)
        {
            analyticModeDropDown.Select(value);
        }

        /// <summary>
        /// Enter a value for 'PercentageDifferenceTrigger' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterPercentageDifferenceTriggerNumericInput(string value)
        {
            percentageDifferenceTriggerNumericInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'FromDaytime' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterFromDaytimeInput(string value)
        {
            fromDaytimeInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'ToDaytime' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterToDaytimeInput(string value)
        {
            toDaytimeInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'CriticalFailureRatio' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterCriticalFailureRatioNumericInput(string value)
        {
            criticalFailureRatioNumericInput.Enter(value);
        }

        /// <summary>
        /// Select an item of 'Failures' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectFailuresListDropDown(string value)
        {
            failuresListDropDown.Select(value);
        }

        /// <summary>
        /// Remove a selected item of 'Failures' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void RemoveFailuresListDropDownValue(string value)
        {
            failuresListDropDown.RemoveValueInList(value);
        }

        /// <summary>
        /// Enter a value for 'CriticalCount' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterCriticalCountNumericInput(string value)
        {
            criticalCountNumericInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'Radius' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterRadiusNumericInput(string value)
        {
            radiusNumericInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'Threshold' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterThresholdNumericInput(string value)
        {
            thresholdNumericInput.Enter(value);
        }

        /// <summary>
        /// Select an item of 'Alarms' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectAlarmsDropDown(string value)
        {
            alarmsDropDown.Select(value);
        }

        /// <summary>
        /// Enter a value for 'LogicalExpression' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterLogicalExpressionInput(string value)
        {
            logicalExpressionInput.Enter(value);
        }

        /// <summary>
        /// Select an item of 'TriggeringOperator' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectTriggeringOperatorDropDown(string value)
        {
            triggeringOperatorDropDown.Select(value);
        }

        /// <summary>
        /// Enter a value for 'TriggeringValue' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterTriggeringValueNumericInput(string value)
        {
            triggeringValueNumericInput.Enter(value);
        }

        /// <summary>
        /// Select an item of 'AnalysisTimeT1' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectAnalysisTimeT1DropDown(string value)
        {
            analysisTimeT1DropDown.Select(value);
        }

        /// <summary>
        /// Select an item of 'AlarmTimeT1' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectAlarmTimeT1DropDown(string value)
        {
            alarmTimeT1DropDown.Select(value);
        }

        /// <summary>
        /// Select an item of 'Meters' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectMetersListDropDown(string value)
        {
            metersListDropDown.Select(value);
        }

        /// <summary>
        /// Remove a selected item of 'Meters' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void RemoveMetersListDropDownValue(string value)
        {
            metersListDropDown.RemoveValueInList(value);
        }

        #endregion //Configuration group

        #endregion //Trigger condition tab

        #region Actions tab

        #region Notify by Email

        /// <summary>
        /// Enter a value for 'From' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterFromInput(string value)
        {
            fromInput.Enter(value);
        }

        /// <summary>
        /// Select an item of 'To' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectToListDropDown(string value)
        {
            toListDropDown.Select(value);
        }

        /// <summary>
        /// Remove a selected item of 'To' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void RemoveToListDropDownValue(string value)
        {
            toListDropDown.RemoveValueInList(value);
        }

        /// <summary>
        /// Click 'ToSelectAll' button
        /// </summary>
        public void ClickToSelectAllButton()
        {
            toSelectAllButton.ClickEx();
        }

        /// <summary>
        /// Enter a value for 'Subject' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterSubjectInput(string value)
        {
            subjectInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'EmailMessage' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterEmailMessageInput(string value)
        {
            emailMessageInput.Enter(value);
        }

        #endregion //Notify by Email

        #region Send HTML request

        /// <summary>
        /// Enter a value for 'Url' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterUrlInput(string value)
        {
            urlInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'User' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterUserInput(string value)
        {
            userInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'Password' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterPasswordInput(string value)
        {
            passwordInput.Enter(value);
        }

        #endregion //Send HTML request

        #region Send report

        /// <summary>
        /// Select an item of 'Report' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectReportDropDown(string value)
        {
            reportDropDown.Select(value);
        }

        #endregion //Send report

        #endregion //Actions tab

        #endregion //Actions

        #region Get methods

        #region Header toolbar buttons

        #endregion //Header toolbar buttons

        #region Content Identity

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
        /// Get 'Type' label text
        /// </summary>
        /// <returns></returns>
        public string GetTypeText()
        {
            return typeLabel.Text;
        }

        /// <summary>
        /// Get 'Type' input value
        /// </summary>
        /// <returns></returns>
        public string GetTypeValue()
        {
            return typeDropDown.Text;
        }

        /// <summary>
        /// Get 'Action' label text
        /// </summary>
        /// <returns></returns>
        public string GetActionText()
        {
            return actionLabel.Text;
        }

        /// <summary>
        /// Get 'Action' input value
        /// </summary>
        /// <returns></returns>
        public string GetActionValue()
        {
            return actionDropDown.Text;
        }

        #endregion //Content Identity

        #region General tab        

        #region Configuration group

        #endregion //Configuration group

        /// <summary>
        /// Get 'Priority' label text
        /// </summary>
        /// <returns></returns>
        public string GetPriorityText()
        {
            return priorityLabel.Text;
        }

        /// <summary>
        /// Get 'PriorityNumberic' input value
        /// </summary>
        /// <returns></returns>
        public string GetPriorityNumbericValue()
        {
            return priorityNumbericInput.Value();
        }

        /// <summary>
        /// Get 'NewAlarmIfRetrigger' label text
        /// </summary>
        /// <returns></returns>
        public string GetNewAlarmIfRetriggerText()
        {
            return newAlarmIfRetriggerLabel.Text;
        }

        /// <summary>
        /// Get 'NewAlarmIfRetrigger' input value
        /// </summary>
        /// <returns></returns>
        public bool GetNewAlarmIfRetriggerValue()
        {
            return newAlarmIfRetriggerCheckbox.CheckboxValue();
        }

        /// <summary>
        /// Get 'AutoAcknowledge' label text
        /// </summary>
        /// <returns></returns>
        public string GetAutoAcknowledgeText()
        {
            return autoAcknowledgeLabel.Text;
        }

        /// <summary>
        /// Get 'AutoAcknowledge' input value
        /// </summary>
        /// <returns></returns>
        public bool GetAutoAcknowledgeValue()
        {
            return autoAcknowledgeCheckbox.CheckboxValue();
        }

        /// <summary>
        /// Get 'Disabled' label text
        /// </summary>
        /// <returns></returns>
        public string GetDisabledText()
        {
            return disabledLabel.Text;
        }

        /// <summary>
        /// Get 'Disabled' input value
        /// </summary>
        /// <returns></returns>
        public bool GetDisabledValue()
        {
            return disabledCheckbox.CheckboxValue();
        }

        /// <summary>
        /// Get 'RefreshRate' label text
        /// </summary>
        /// <returns></returns>
        public string GetRefreshRateText()
        {
            return refreshRateLabel.Text;
        }

        /// <summary>
        /// Get 'RefreshRate' input value
        /// </summary>
        /// <returns></returns>
        public string GetRefreshRateValue()
        {
            return refreshRateDropDown.Text;
        }

        #endregion //General tab

        #region Trigger condition tab

        #region Configuration group

        /// <summary>
        /// Get 'Message' label text
        /// </summary>
        /// <returns></returns>
        public string GetMessageText()
        {
            return messageLabel.Text;
        }

        /// <summary>
        /// Get 'Message' input value
        /// </summary>
        /// <returns></returns>
        public string GetMessageValue()
        {
            return messageInput.Value();
        }

        /// <summary>
        /// Get 'Controllers' label text
        /// </summary>
        /// <returns></returns>
        public string GetControllersText()
        {
            return controllersLabel.Text;
        }

        /// <summary>
        /// Get 'Controllers' input value
        /// </summary>
        /// <returns></returns>
        public string GetControllersValue()
        {
            return controllersDropDown.Text;
        }

        /// <summary>
        /// Get 'InputOutput' label text
        /// </summary>
        /// <returns></returns>
        public string GetInputOutputText()
        {
            return inputOutputLabel.Text;
        }

        /// <summary>
        /// Get 'InputOutput' input value
        /// </summary>
        /// <returns></returns>
        public string GetInputOutputValue()
        {
            return inputOutputDropDown.Text;
        }

        /// <summary>
        /// Get 'SunEvent' label text
        /// </summary>
        /// <returns></returns>
        public string GetSunEventText()
        {
            return sunEventLabel.Text;
        }

        /// <summary>
        /// Get 'SunEvent' input value
        /// </summary>
        /// <returns></returns>
        public string GetSunEventValue()
        {
            return sunEventDropDown.Text;
        }

        /// <summary>
        /// Get 'Delay' label text
        /// </summary>
        /// <returns></returns>
        public string GetDelayText()
        {
            return delayLabel.Text;
        }

        /// <summary>
        /// Get 'DelayNumberic' input value
        /// </summary>
        /// <returns></returns>
        public string GetDelayNumbericValue()
        {
            return delayNumbericInput.Value();
        }

        /// <summary>
        /// Get all selected values of 'Controllers'
        /// </summary>
        /// <returns></returns>
        public List<string> GetControllersValues()
        {
            return controllersListDropDown.GetSelectedItems();
        }

        /// <summary>
        /// Get 'InputOutput1' label text
        /// </summary>
        /// <returns></returns>
        public string GetInputOutput1Text()
        {
            return inputOutput1Label.Text;
        }

        /// <summary>
        /// Get 'InputOutput1' input value
        /// </summary>
        /// <returns></returns>
        public string GetInputOutput1Value()
        {
            return inputOutput1DropDown.Text;
        }

        /// <summary>
        /// Get 'Operator1' label text
        /// </summary>
        /// <returns></returns>
        public string GetOperator1Text()
        {
            return operator1Label.Text;
        }

        /// <summary>
        /// Get 'Operator1' input value
        /// </summary>
        /// <returns></returns>
        public string GetOperator1Value()
        {
            return operator1DropDown.Text;
        }

        /// <summary>
        /// Get 'TestValue1' label text
        /// </summary>
        /// <returns></returns>
        public string GetTestValue1Text()
        {
            return testValue1Label.Text;
        }

        /// <summary>
        /// Get 'TestValue1' input value
        /// </summary>
        /// <returns></returns>
        public string GetTestValue1Value()
        {
            return testValue1DropDown.Text;
        }

        /// <summary>
        /// Get 'InputOutput2' label text
        /// </summary>
        /// <returns></returns>
        public string GetInputOutput2Text()
        {
            return inputOutput2Label.Text;
        }

        /// <summary>
        /// Get 'InputOutput2' input value
        /// </summary>
        /// <returns></returns>
        public string GetInputOutput2Value()
        {
            return inputOutput2DropDown.Text;
        }

        /// <summary>
        /// Get 'Operator2' label text
        /// </summary>
        /// <returns></returns>
        public string GetOperator2Text()
        {
            return operator2Label.Text;
        }

        /// <summary>
        /// Get 'Operator2' input value
        /// </summary>
        /// <returns></returns>
        public string GetOperator2Value()
        {
            return operator2DropDown.Text;
        }

        /// <summary>
        /// Get 'TestValue2' label text
        /// </summary>
        /// <returns></returns>
        public string GetTestValue2Text()
        {
            return testValue2Label.Text;
        }

        /// <summary>
        /// Get 'TestValue2' input value
        /// </summary>
        /// <returns></returns>
        public string GetTestValue2Value()
        {
            return testValue2DropDown.Text;
        }

        /// <summary>
        /// Get 'InputOutput3' label text
        /// </summary>
        /// <returns></returns>
        public string GetInputOutput3Text()
        {
            return inputOutput3Label.Text;
        }

        /// <summary>
        /// Get 'InputOutput3' input value
        /// </summary>
        /// <returns></returns>
        public string GetInputOutput3Value()
        {
            return inputOutput3DropDown.Text;
        }

        /// <summary>
        /// Get 'Operator3' label text
        /// </summary>
        /// <returns></returns>
        public string GetOperator3Text()
        {
            return operator3Label.Text;
        }

        /// <summary>
        /// Get 'Operator3' input value
        /// </summary>
        /// <returns></returns>
        public string GetOperator3Value()
        {
            return operator3DropDown.Text;
        }

        /// <summary>
        /// Get 'TestValue3' label text
        /// </summary>
        /// <returns></returns>
        public string GetTestValue3Text()
        {
            return testValue3Label.Text;
        }

        /// <summary>
        /// Get 'TestValue3' input value
        /// </summary>
        /// <returns></returns>
        public string GetTestValue3Value()
        {
            return testValue3DropDown.Text;
        }

        /// <summary>
        /// Get 'LogicalOperator' label text
        /// </summary>
        /// <returns></returns>
        public string GetLogicalOperatorText()
        {
            return logicalOperatorLabel.Text;
        }

        /// <summary>
        /// Get 'LogicalOperator' input value
        /// </summary>
        /// <returns></returns>
        public string GetLogicalOperatorValue()
        {
            return logicalOperatorDropDown.Text;
        }

        /// <summary>
        /// Get 'Operator1_2' label text
        /// </summary>
        /// <returns></returns>
        public string GetOperator1_2Text()
        {
            return operator1_2Label.Text;
        }

        /// <summary>
        /// Get 'Operator1_2' input value
        /// </summary>
        /// <returns></returns>
        public string GetOperator1_2Value()
        {
            return operator1_2DropDown.Text;
        }

        /// <summary>
        /// Get 'Operator1_3' label text
        /// </summary>
        /// <returns></returns>
        public string GetOperator1_3Text()
        {
            return operator1_3Label.Text;
        }

        /// <summary>
        /// Get 'Operator1_3' input value
        /// </summary>
        /// <returns></returns>
        public string GetOperator1_3Value()
        {
            return operator1_3DropDown.Text;
        }

        /// <summary>
        /// Get 'FirstIO' label text
        /// </summary>
        /// <returns></returns>
        public string GetFirstIOText()
        {
            return firstIOLabel.Text;
        }

        /// <summary>
        /// Get 'FirstIO' dropdown value
        /// </summary>
        /// <returns></returns>
        public string GetFirstIOValue()
        {
            return firstIODropDown.Text;
        }

        /// <summary>
        /// Get 'SecondIO' label text
        /// </summary>
        /// <returns></returns>
        public string GetSecondIOText()
        {
            return secondIOLabel.Text;
        }

        /// <summary>
        /// Get 'SecondIO' dropdown value
        /// </summary>
        /// <returns></returns>
        public string GetSecondIOValue()
        {
            return secondIODropDown.Text;
        }

        /// <summary>
        /// Get 'Operator' label text
        /// </summary>
        /// <returns></returns>
        public string GetOperatorText()
        {
            return operatorLabel.Text;
        }

        /// <summary>
        /// Get 'Operator' input value
        /// </summary>
        /// <returns></returns>
        public string GetOperatorValue()
        {
            return operatorDropDown.Text;
        }

        /// <summary>
        /// Get 'InputName' label text
        /// </summary>
        /// <returns></returns>
        public string GetInputNameText()
        {
            return inputNameLabel.Text;
        }

        /// <summary>
        /// Get 'InputName' input value
        /// </summary>
        /// <returns></returns>
        public string GetInputNameValue()
        {
            return inputNameDropDown.Text;
        }

        /// <summary>
        /// Get 'InputValue' label text
        /// </summary>
        /// <returns></returns>
        public string GetInputValueText()
        {
            return inputValueLabel.Text;
        }

        /// <summary>
        /// Get 'InputValue' input value
        /// </summary>
        /// <returns></returns>
        public string GetInputValueValue()
        {
            return inputValueDropDown.Text;
        }

        /// <summary>
        /// Get 'HoursDelay' label text
        /// </summary>
        /// <returns></returns>
        public string GetHoursDelayText()
        {
            return hoursDelayLabel.Text;
        }

        /// <summary>
        /// Get 'HoursDelayNumberic' input value
        /// </summary>
        /// <returns></returns>
        public string GetHoursDelayNumbericValue()
        {
            return hoursDelayNumbericInput.Value();
        }

        /// <summary>
        /// Get 'HoursCheckInterval' label text
        /// </summary>
        /// <returns></returns>
        public string GetHoursCheckIntervalText()
        {
            return hoursCheckIntervalLabel.Text;
        }

        /// <summary>
        /// Get 'HoursCheckInterval' input value
        /// </summary>
        /// <returns></returns>
        public string GetCheckHoursIntervalValue()
        {
            return checkHoursIntervalDropDown.Text;
        }

        /// <summary>
        /// Get 'Devices' label text
        /// </summary>
        /// <returns></returns>
        public string GetDevicesText()
        {
            return devicesLabel.Text;
        }

        /// <summary>
        /// Get all selected values of 'Devices'
        /// </summary>
        /// <returns></returns>
        public List<string> GetDevicesValues()
        {
            return devicesListDropDown.GetSelectedItems();
        }

        /// <summary>
        /// Get 'VariablesType' label text
        /// </summary>
        /// <returns></returns>
        public string GetVariablesTypeText()
        {
            return variablesTypeLabel.Text;
        }

        /// <summary>
        /// Get all selected values of 'VariablesType'
        /// </summary>
        /// <returns></returns>
        public List<string> GetVariablesTypeValues()
        {
            return variablesTypeListDropDown.GetSelectedItems();
        }

        /// <summary>
        /// Get 'CriticalDelay' label text
        /// </summary>
        /// <returns></returns>
        public string GetCriticalDelayText()
        {
            return criticalDelayLabel.Text;
        }

        /// <summary>
        /// Get 'CriticalDelay' input value
        /// </summary>
        /// <returns></returns>
        public string GetCriticalDelayValue()
        {
            return criticalDelayDropDown.Text;
        }

        /// <summary>
        /// Get 'TimestampMode' label text
        /// </summary>
        /// <returns></returns>
        public string GetTimestampModeText()
        {
            return timestampModeLabel.Text;
        }

        /// <summary>
        /// Get 'TimestampMode' input value
        /// </summary>
        /// <returns></returns>
        public string GetTimestampModeValue()
        {
            return timestampModeDropDown.Text;
        }

        /// <summary>
        /// Get 'CriticalRatio' label text
        /// </summary>
        /// <returns></returns>
        public string GetCriticalRatioText()
        {
            return criticalRatioLabel.Text;
        }

        /// <summary>
        /// Get 'CriticalRatio' input value
        /// </summary>
        /// <returns></returns>
        public string GetCriticalRatioValue()
        {
            return criticalRatioNumericInput.Value();
        }

        /// <summary>
        /// Get 'Device' input value
        /// </summary>
        /// <returns></returns>
        public string GetDeviceValue()
        {
            return deviceDropDown.Text;
        }

        /// <summary>
        /// Get 'CriticityLevel' label text
        /// </summary>
        /// <returns></returns>
        public string GetCriticityLevelText()
        {
            return criticityLevelLabel.Text;
        }

        /// <summary>
        /// Get 'CriticityLevel' input value
        /// </summary>
        /// <returns></returns>
        public string GetCriticityLevelValue()
        {
            return criticityLevelDropDown.Text;
        }

        /// <summary>
        /// Get 'Metering' label text
        /// </summary>
        /// <returns></returns>
        public string GetMeteringText()
        {
            return meteringLabel.Text;
        }

        /// <summary>
        /// Get 'Metering' input value
        /// </summary>
        /// <returns></returns>
        public string GetMeteringValue()
        {
            return meteringDropDown.Text;
        }

        /// <summary>
        /// Get 'IgnoreOperator' label text
        /// </summary>
        /// <returns></returns>
        public string GetIgnoreOperatorText()
        {
            return ignoreOperatorLabel.Text;
        }

        /// <summary>
        /// Get 'IgnoreOperator' input value
        /// </summary>
        /// <returns></returns>
        public string GetIgnoreOperatorValue()
        {
            return ignoreOperatorDropDown.Text;
        }

        /// <summary>
        /// Get 'IgnoreValue' label text
        /// </summary>
        /// <returns></returns>
        public string GetIgnoreValueText()
        {
            return ignoreValueLabel.Text;
        }

        /// <summary>
        /// Get 'IgnoreValue' input value
        /// </summary>
        /// <returns></returns>
        public string GetIgnoreValueValue()
        {
            return ignoreValueNumericInput.Value();
        }

        /// <summary>
        /// Get 'AnalysisPeriod' label text
        /// </summary>
        /// <returns></returns>
        public string GetAnalysisPeriodText()
        {
            return analysisPeriodLabel.Text;
        }

        /// <summary>
        /// Get 'AnalysisPeriod' input value
        /// </summary>
        /// <returns></returns>
        public string GetAnalysisPeriodValue()
        {
            return analysisPeriodDropDown.Text;
        }

        /// <summary>
        /// Get 'AnalyticMode' label text
        /// </summary>
        /// <returns></returns>
        public string GetAnalyticModeText()
        {
            return analyticModeLabel.Text;
        }

        /// <summary>
        /// Get 'AnalyticMode' input value
        /// </summary>
        /// <returns></returns>
        public string GetAnalyticModeValue()
        {
            return analyticModeDropDown.Text;
        }

        /// <summary>
        /// Get 'PercentageDifferenceTrigger' label text
        /// </summary>
        /// <returns></returns>
        public string GetPercentageDifferenceTriggerText()
        {
            return percentageDifferenceTriggerLabel.Text;
        }

        /// <summary>
        /// Get 'PercentageDifferenceTrigger' input value
        /// </summary>
        /// <returns></returns>
        public string GetPercentageDifferenceTriggerValue()
        {
            return percentageDifferenceTriggerNumericInput.Value();
        }

        /// <summary>
        /// Get 'FromDaytime' label text
        /// </summary>
        /// <returns></returns>
        public string GetFromDaytimeText()
        {
            return fromDaytimeLabel.Text;
        }

        /// <summary>
        /// Get 'FromDaytime' input value
        /// </summary>
        /// <returns></returns>
        public string GetFromDaytimeValue()
        {
            return fromDaytimeInput.Value();
        }

        /// <summary>
        /// Get 'ToDaytime' label text
        /// </summary>
        /// <returns></returns>
        public string GetToDaytimeText()
        {
            return toDaytimeLabel.Text;
        }

        /// <summary>
        /// Get 'ToDaytime' input value
        /// </summary>
        /// <returns></returns>
        public string GetToDaytimeValue()
        {
            return toDaytimeInput.Value();
        }

        /// <summary>
        /// Get 'CriticalFailureRatio' label text
        /// </summary>
        /// <returns></returns>
        public string GetCriticalFailureRatioText()
        {
            return criticalFailureRatioLabel.Text;
        }

        /// <summary>
        /// Get 'CriticalFailureRatio' input value
        /// </summary>
        /// <returns></returns>
        public string GetCriticalFailureRatioValue()
        {
            return criticalFailureRatioNumericInput.Value();
        }

        /// <summary>
        /// Get 'Failures' label text
        /// </summary>
        /// <returns></returns>
        public string GetFailuresText()
        {
            return failuresLabel.Text;
        }

        /// <summary>
        /// Get all selected values of 'Failures'
        /// </summary>
        /// <returns></returns>
        public List<string> GetFailuresValues()
        {
            return failuresListDropDown.GetSelectedItems();
        }

        /// <summary>
        /// Get 'CriticalCount' label text
        /// </summary>
        /// <returns></returns>
        public string GetCriticalCountText()
        {
            return criticalCountLabel.Text;
        }

        /// <summary>
        /// Get 'CriticalCount' input value
        /// </summary>
        /// <returns></returns>
        public string GetCriticalCountValue()
        {
            return criticalCountNumericInput.Value();
        }

        /// <summary>
        /// Get 'Radius' label text
        /// </summary>
        /// <returns></returns>
        public string GetRadiusText()
        {
            return radiusLabel.Text;
        }

        /// <summary>
        /// Get 'Radius' input value
        /// </summary>
        /// <returns></returns>
        public string GetRadiusValue()
        {
            return radiusNumericInput.Value();
        }

        /// <summary>
        /// Get 'Threshold' label text
        /// </summary>
        /// <returns></returns>
        public string GetThresholdText()
        {
            return thresholdLabel.Text;
        }

        /// <summary>
        /// Get 'Threshold' input value
        /// </summary>
        /// <returns></returns>
        public string GetThresholdValue()
        {
            return thresholdNumericInput.Value();
        }

        /// <summary>
        /// Get 'Alarms' label text
        /// </summary>
        /// <returns></returns>
        public string GetAlarmsText()
        {
            return alarmsLabel.Text;
        }

        /// <summary>
        /// Get 'Alarms' input value
        /// </summary>
        /// <returns></returns>
        public string GetAlarmsValue()
        {
            return alarmsDropDown.Text;
        }

        /// <summary>
        /// Get 'LogicalExpression' label text
        /// </summary>
        /// <returns></returns>
        public string GetLogicalExpressionText()
        {
            return logicalExpressionLabel.Text;
        }

        /// <summary>
        /// Get 'LogicalExpression' input value
        /// </summary>
        /// <returns></returns>
        public string GetLogicalExpressionValue()
        {
            return logicalExpressionInput.Value();
        }

        /// <summary>
        /// Get 'TriggeringOperator' label text
        /// </summary>
        /// <returns></returns>
        public string GetTriggeringOperatorText()
        {
            return triggeringOperatorLabel.Text;
        }

        /// <summary>
        /// Get 'TriggeringOperator' input value
        /// </summary>
        /// <returns></returns>
        public string GetTriggeringOperatorValue()
        {
            return triggeringOperatorDropDown.Text;
        }

        /// <summary>
        /// Get 'TriggeringValue' label text
        /// </summary>
        /// <returns></returns>
        public string GetTriggeringValueText()
        {
            return triggeringValueLabel.Text;
        }

        /// <summary>
        /// Get 'TriggeringValue' input value
        /// </summary>
        /// <returns></returns>
        public string GetTriggeringValueValue()
        {
            return triggeringValueNumericInput.Value();
        }

        /// <summary>
        /// Get 'AnalysisTimeT1' label text
        /// </summary>
        /// <returns></returns>
        public string GetAnalysisTimeT1Text()
        {
            return analysisTimeT1Label.Text;
        }

        /// <summary>
        /// Get 'AnalysisTimeT1' input value
        /// </summary>
        /// <returns></returns>
        public string GetAnalysisTimeT1Value()
        {
            return analysisTimeT1DropDown.Text;
        }

        /// <summary>
        /// Get 'AlarmTimeT1' label text
        /// </summary>
        /// <returns></returns>
        public string GetAlarmTimeT1Text()
        {
            return alarmTimeT1Label.Text;
        }

        /// <summary>
        /// Get 'AlarmTimeT1' input value
        /// </summary>
        /// <returns></returns>
        public string GetAlarmTimeT1Value()
        {
            return alarmTimeT1DropDown.Text;
        }

        /// <summary>
        /// Get 'Meters' label text
        /// </summary>
        /// <returns></returns>
        public string GetMetersText()
        {
            return metersLabel.Text;
        }

        /// <summary>
        /// Get all selected values of 'Meters'
        /// </summary>
        /// <returns></returns>
        public List<string> GetMetersValues()
        {
            return metersListDropDown.GetSelectedItems();
        }

        #endregion //Configuration group

        #endregion //Trigger condition tab

        #region Actions tab

        #region Notify by Email

        /// <summary>
        /// Get 'From' label text
        /// </summary>
        /// <returns></returns>
        public string GetFromText()
        {
            return fromLabel.Text;
        }

        /// <summary>
        /// Get 'From' input value
        /// </summary>
        /// <returns></returns>
        public string GetFromValue()
        {
            return fromInput.Value();
        }

        /// <summary>
        /// Get 'To' label text
        /// </summary>
        /// <returns></returns>
        public string GetToText()
        {
            return toLabel.Text;
        }

        /// <summary>
        /// Get all selected values of 'To'
        /// </summary>
        /// <returns></returns>
        public List<string> GetToValues()
        {
            return toListDropDown.GetSelectedItems();
        }

        /// <summary>
        /// Get 'Subject' label text
        /// </summary>
        /// <returns></returns>
        public string GetSubjectText()
        {
            return subjectLabel.Text;
        }

        /// <summary>
        /// Get 'Subject' input value
        /// </summary>
        /// <returns></returns>
        public string GetSubjectValue()
        {
            return subjectInput.Value();
        }

        /// <summary>
        /// Get 'EmailMessage' label text
        /// </summary>
        /// <returns></returns>
        public string GetEmailMessageText()
        {
            return emailMessageLabel.Text;
        }

        /// <summary>
        /// Get 'EmailMessage' input value
        /// </summary>
        /// <returns></returns>
        public string GetEmailMessageValue()
        {
            return emailMessageInput.Value();
        }

        #endregion //Notify by Email

        #region Send HTML request

        /// <summary>
        /// Get 'Url' label text
        /// </summary>
        /// <returns></returns>
        public string GetUrlText()
        {
            return urlLabel.Text;
        }

        /// <summary>
        /// Get 'Url' input value
        /// </summary>
        /// <returns></returns>
        public string GetUrlValue()
        {
            return urlInput.Value();
        }

        /// <summary>
        /// Get 'User' label text
        /// </summary>
        /// <returns></returns>
        public string GetUserText()
        {
            return userLabel.Text;
        }

        /// <summary>
        /// Get 'User' input value
        /// </summary>
        /// <returns></returns>
        public string GetUserValue()
        {
            return userInput.Value();
        }

        /// <summary>
        /// Get 'Password' label text
        /// </summary>
        /// <returns></returns>
        public string GetPasswordText()
        {
            return passwordLabel.Text;
        }

        /// <summary>
        /// Get 'Password' input value
        /// </summary>
        /// <returns></returns>
        public string GetPasswordValue()
        {
            return passwordInput.Value();
        }

        #endregion //Send HTML request

        #region Send report

        /// <summary>
        /// Get 'Report' label text
        /// </summary>
        /// <returns></returns>
        public string GetReportText()
        {
            return reportLabel.Text;
        }

        /// <summary>
        /// Get 'Report' input value
        /// </summary>
        /// <returns></returns>
        public string GetReportValue()
        {
            return reportDropDown.Text;
        }

        #endregion //Send report

        #endregion //Actions tab

        #endregion //Get methods

        #endregion //Basic methods

        #region Business methods

        /// <summary>
        /// Check is panel is visible
        /// </summary>
        /// <returns></returns>
        public bool IsPanelVisible()
        {
            return deleteButton.Displayed;
        }

        /// <summary>
        /// Select a tab
        /// </summary>
        /// <param name="tabName"></param>
        public void SelectTab(string tabName)
        {
            var tab = tabsList.FirstOrDefault(t => t.Text.TrimEx().Equals(tabName, StringComparison.InvariantCultureIgnoreCase));
            if (tab != null)
            {
                tab.ClickEx();
                WebDriverContext.Wait.Until(driver => JSUtility.GetElementText("[id$='editor-content-properties-tabs'] div.w2ui-tab.active") == tabName);
            }
        }

        /// <summary>
        /// Get all 'RefreshRate' items
        /// </summary>
        /// <returns></returns>
        public List<string> GetRefreshRateItems()
        {
            return refreshRateDropDown.GetAllItems();
        }

        /// <summary>
        /// Get 'EmailMessage' input height
        /// </summary>
        /// <returns></returns>
        public int GetEmailMessageHeight()
        {
            var height = emailMessageInput.GetStyleAttr("height");
            return int.Parse(height.Replace("px", string.Empty));
        }

        /// <summary>
        /// Get 'Email' container height
        /// </summary>
        /// <returns></returns>
        public int GetEmailContainerHeight()
        {
            var height = emailContainer.GetStyleAttr("height");
            return int.Parse(height.Replace("px", string.Empty));
        }

        /// <summary>
        /// Clear selected item of 'IgnoreOperator' dropdown 
        /// </summary>
        public void ClearIgnoreOperatorDropDown()
        {
            ignoreOperatorDropDown.ClearSelectedItem();
        }

        #endregion //Business methods

        public override void WaitForPanelLoaded()
        {
            Wait.ForElementsDisplayed(By.CssSelector("[id$='editor-content-properties-tab-0']"));
        }
    }
}
