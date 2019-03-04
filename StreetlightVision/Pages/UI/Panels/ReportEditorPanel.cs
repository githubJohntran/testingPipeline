using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using StreetlightVision.Extensions;
using StreetlightVision.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StreetlightVision.Pages.UI
{
    public class ReportEditorPanel : PanelBase
    {
        #region Variables

        #endregion //Variables

        #region IWebElements

        #region Header toolbar buttons

        [FindsBy(How = How.CssSelector, Using = "[id='tb_reportmanagerReportButtons_item_cancel'] .w2ui-button")]
        private IWebElement cancelButton;

        [FindsBy(How = How.CssSelector, Using = "[id='tb_reportmanagerReportButtons_item_save'] .w2ui-button")]
        private IWebElement saveButton;

        [FindsBy(How = How.CssSelector, Using = "[id='tb_reportmanagerReportButtons_item_delete'] .w2ui-button")]
        private IWebElement deleteButton;

        #endregion //Header toolbar buttons

        #region Content Identity

        [FindsBy(How = How.CssSelector, Using = "[id$='geozone-editor-content-name-label'] .slv-label.reportmanager-editor-label")]
        private IWebElement nameLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='geozone-editor-content-type-label'] .slv-label.reportmanager-editor-label")]
        private IWebElement typeLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='geozone-editor-content-name-field']")]
        private IWebElement nameInput;

        [FindsBy(How = How.CssSelector, Using = "div[id$='geozone-editor-content-type-field']")]
        private IWebElement typeDropDown;

        #endregion //Content Identity

        #region Properties tab

        #region Configuration group

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-description'] .slv-label.editor-label")]
        private IWebElement descriptionLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-fromHourOfDay'] .slv-label.editor-label")]
        private IWebElement fromHourLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-fromMinute'] .slv-label.editor-label")]
        private IWebElement fromMinuteLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-toHourOfDay'] .slv-label.editor-label")]
        private IWebElement toHourLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-toMinute'] .slv-label.editor-label")]
        private IWebElement toMinuteLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-minutesPeriod'] .slv-label.editor-label")]
        private IWebElement periodicityMinutesLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-type1FailureMeaningStrIds'] .slv-label.editor-label")]
        private IWebElement lampFailuresLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-type1FailurePercentageGroupThreshold'] .slv-label.editor-label")]
        private IWebElement lampThresholdLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-type2FailureMeaningStrIds'] .slv-label.editor-label")]
        private IWebElement commFailuresLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-type2FailurePercentageGroupThreshold'] .slv-label.editor-label")]
        private IWebElement commThresholdLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-detail'] .slv-label.editor-label")]
        private IWebElement reportDetailsLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-devicesFilterMode'] .slv-label.editor-label")]
        private IWebElement filteringModeLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-criticalLifetimePercentageValue'] .slv-label.editor-label")]
        private IWebElement criticalLifetimeLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-daysCount'] .slv-label.editor-label")]
        private IWebElement numberOfDaysLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-downMeaningStrIds'] .slv-label.editor-label")]
        private IWebElement downMeaningsLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-contactType'] .slv-label.editor-label")]
        private IWebElement contactTypeLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-customerFullName'] .slv-label.editor-label")]
        private IWebElement customerFullnameLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-customerShortCode'] .slv-label.editor-label")]
        private IWebElement customerShortCodeLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-serviceCode'] .slv-label.editor-label")]
        private IWebElement serviceCodeLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-unitName'] .slv-label.editor-label")]
        private IWebElement unitNameLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-webCustomerId'] .slv-label.editor-label")]
        private IWebElement webCustomerIdLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-minDeltaTimeInMinutes'] .slv-label.editor-label")]
        private IWebElement minimumDeltaTimeLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-minDeltaLevel'] .slv-label.editor-label")]
        private IWebElement minimumDeltaLevelLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-maxRetry'] .slv-label.editor-label")]
        private IWebElement maxRetryLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-description-field']")]
        private IWebElement descriptionInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-fromHourOfDay-field']")]
        private IWebElement fromHourDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-fromMinute-field']")]
        private IWebElement fromMinuteDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-toHourOfDay-field']")]
        private IWebElement toHourDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-toMinute-field']")]
        private IWebElement toMinuteDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-minutesPeriod-field']")]
        private IWebElement periodicityMinutesDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-type1FailureMeaningStrIds-field']")]
        private IWebElement lampFailuresListDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-type1FailureMeaningStrIds-field'] > button")]
        private IWebElement lampFailuresSelectAllButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-type1FailurePercentageGroupThreshold-field']")]
        private IWebElement lampThresholdNumericInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-type2FailureMeaningStrIds-field']")]
        private IWebElement commFailuresListDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-type2FailureMeaningStrIds-field'] > button")]
        private IWebElement commFailuresSelectAllButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-type2FailurePercentageGroupThreshold-field']")]
        private IWebElement commThresholdNumericInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-detail-field']")]
        private IWebElement reportDetailsDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-devicesFilterMode-field']")]
        private IWebElement filteringModeDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-criticalLifetimePercentageValue-field']")]
        private IWebElement criticalLifetimeNumericInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-daysCount-field']")]
        private IWebElement numberOfDaysInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-downMeaningStrIds-field']")]
        private IWebElement downFailuresListDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-downMeaningStrIds-field'] > button")]
        private IWebElement downFailuresSelectAllButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-contactType-field']")]
        private IWebElement contactTypeInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-customerFullName-field']")]
        private IWebElement customerFullnameInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-customerShortCode-field']")]
        private IWebElement customerShortCodeInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-serviceCode-field']")]
        private IWebElement serviceCodeInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-unitName-field']")]
        private IWebElement unitNameInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-webCustomerId-field']")]
        private IWebElement webCustomerIdInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-minDeltaTimeInMinutes-field']")]
        private IWebElement minimumDeltaTimeNumericInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-minDeltaLevel-field']")]
        private IWebElement minimumDeltaLevelNumericInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-maxRetry-field']")]
        private IWebElement maxRetryNumericInput;

        #endregion //Configuration group

        #endregion //Properties tab

        #region Scheduler tab

        #region Configuration/BasicScheduler group

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-hourOfDay'] .slv-label.editor-label")]
        private IWebElement hourLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-minute'] .slv-label.editor-label")]
        private IWebElement minuteLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-periodStrId'] .slv-label.editor-label")]
        private IWebElement periodicityLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-hourOfDay-field']")]
        private IWebElement hourDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-minute-field']")]
        private IWebElement minuteDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-periodStrId-field']")]
        private IWebElement periodicityDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-schedulerTimeZoneId-field']")]
        private IWebElement timezoneDropDown;

        #endregion //Configuration/BasicScheduler group

        #endregion //Scheduler tab

        #region Export tab

        #region FTP group

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-ftpHost'] .slv-label.editor-label")]
        private IWebElement ftpHostLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-ftpSftpMode'] .slv-label.editor-label")]
        private IWebElement sftpModeLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-ftpUser'] .slv-label.editor-label")]
        private IWebElement ftpUserLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-ftpPassword'] .slv-label.editor-label")]
        private IWebElement ftpPasswordLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-ftpDirectory'] .slv-label.editor-label")]
        private IWebElement ftpDirectoryLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-ftpPassivMode'] .slv-label.editor-label")]
        private IWebElement ftpPassiveModeLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-ftpFileName'] .slv-label.editor-label")]
        private IWebElement ftpFilenameLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-ftpExportEnabled'] .slv-label.editor-label")]
        private IWebElement ftpEnabledLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-ftpFormat'] .slv-label.editor-label")]
        private IWebElement ftpFormatLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-ftpHost-field']")]
        private IWebElement ftpHostInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-ftpSftpMode-field']")]
        private IWebElement sftpModeCheckbox;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-ftpUser-field']")]
        private IWebElement ftpUserInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-ftpPassword-field']")]
        private IWebElement ftpPasswordInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-ftpDirectory-field']")]
        private IWebElement ftpDirectoryInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-ftpPassivMode-field']")]
        private IWebElement ftpPassiveModeCheckbox;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-ftpFileName-field']")]
        private IWebElement ftpFilenameInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-ftpExportEnabled-field']")]
        private IWebElement ftpEnabledCheckbox;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-ftpFormat-field']")]
        private IWebElement ftpFormatDropDown;

        #endregion //FTP group

        #region Mail group

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-mailExportEnabled'] .slv-label.editor-label")]
        private IWebElement mailEnabledLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-mailFormat'] .slv-label.editor-label")]
        private IWebElement mailFormatLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-reportInMailBody'] .slv-label.editor-label")]
        private IWebElement reportMailBodyLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-subject'] .slv-label.editor-label")]
        private IWebElement subjectLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-from'] .slv-label.editor-label")]
        private IWebElement fromLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-contacts'] .slv-label.editor-label")]
        private IWebElement contactsLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-mailExportEnabled-field']")]
        private IWebElement mailEnabledCheckbox;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-mailFormat-field']")]
        private IWebElement mailFormatDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-reportInMailBody-field']")]
        private IWebElement reportMailBodyCheckbox;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-subject-field']")]
        private IWebElement subjectInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-from-field']")]
        private IWebElement fromInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-contacts-field']")]
        private IWebElement contactsListDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-contacts-field'] > button")]
        private IWebElement contactsSelectAllButton;

        #endregion //Mail group

        #region Configuration group

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-htmlFormat'] .slv-label.editor-label")]
        private IWebElement htmlFormatLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-htmlFormat-field']")]
        private IWebElement htmlFormatCheckbox;

        #endregion //Configuration group

        #endregion //Export tab

        #region Report tab

        #region Configuration group

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-deviceCategoryStrIds'] .slv-label.editor-label")]
        private IWebElement deviceCategoriesLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-recurse'] .slv-label.editor-label")]
        private IWebElement recurseLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-valueNames'] .slv-label.editor-label")]
        private IWebElement valuesLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-meaningStrIds'] .slv-label.editor-label")]
        private IWebElement meaningsLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-lastValueMode'] .slv-label.editor-label")]
        private IWebElement lastValueOnlyLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-commandMeaningStrId'] .slv-label.editor-label")]
        private IWebElement commandLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-feedbackMeaningStrId'] .slv-label.editor-label")]
        private IWebElement feedbackLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-modeMeaningStrId'] .slv-label.editor-label")]
        private IWebElement modeMeaningLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-fromToMode'] .slv-label.editor-label")]
        private IWebElement fromToModeLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-fromLastHours'] .slv-label.editor-label")]
        private IWebElement fromLastHoursLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-fromHourOfDay'] .slv-label.editor-label")]
        private IWebElement fromLocalTimeLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-toHourOfDay'] .slv-label.editor-label")]
        private IWebElement toLocalTimeLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-commandValueStr'] .slv-label.editor-label")]
        private IWebElement commandValueLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-deviceCategoryStrIds-field']")]
        private IWebElement deviceCategoriesListDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-deviceCategoryStrIds-field'] > button")]
        private IWebElement deviceCategoriesSelectAllButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-recurse-field']")]
        private IWebElement recurseCheckbox;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-valueNames-field']")]
        private IWebElement valuesListDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-valueNames-field'] > button")]
        private IWebElement valuesSelectAllButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-meaningStrIds-field']")]
        private IWebElement meaningsListDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-meaningStrIds-field'] > button")]
        private IWebElement meaningsSelectAllButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-lastValueMode-field']")]
        private IWebElement lastValueOnlyCheckbox;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-commandMeaningStrId-field']")]
        private IWebElement commandDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-feedbackMeaningStrId-field']")]
        private IWebElement feedbackDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-modeMeaningStrId-field']")]
        private IWebElement modeMeaningDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-fromToMode-field']")]
        private IWebElement fromToModeDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-fromLastHours-field']")]
        private IWebElement fromLastHoursDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-fromHourOfDay-field']")]
        private IWebElement fromLocalTimeDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-toHourOfDay-field']")]
        private IWebElement toLocalTimeDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-commandValueStr-field']")]
        private IWebElement commandValueInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-fromDelay-field']")]
        private IWebElement periodDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-fromDayStr'] .slv-label.editor-label")]
        private IWebElement startDayLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-fromDayStr-field']")]
        private IWebElement startDayDateInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-fromTimeStr'] .slv-label.editor-label")]
        private IWebElement startTimeLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-fromTimeStr-field']")]
        private IWebElement startTimeInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-toDayStr'] .slv-label.editor-label")]
        private IWebElement endDayLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-toDayStr-field']")]
        private IWebElement endDayDateInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-toTimeStr'] .slv-label.editor-label")]
        private IWebElement endTimeLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-toTimeStr-field']")]
        private IWebElement endTimeInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-savedSearchId'] .slv-label.editor-label")]
        private IWebElement savedSearchLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-savedSearchId-field']")]
        private IWebElement savedSearchDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-addEventTimeColumns'] .slv-label.editor-label")]
        private IWebElement timestampColumnsLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-addEventTimeColumns-field']")]
        private IWebElement timestampColumnsCheckbox;

        #endregion //Configuration group

        #endregion //Report tab

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-content-properties-tabs'] div.w2ui-tab")]
        private IList<IWebElement> tabsList;

        [FindsBy(How = How.CssSelector, Using = "[id$='geozone-editor'] div.editor-group-header")]
        private IList<IWebElement> editorGroupHeaderList;

        #endregion //IWebElements

        #region Constructor

        public ReportEditorPanel(IWebDriver driver, PageBase page) : base(driver, page)
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

        #endregion //Content Identity

        #region Properties tab

        #region Configuration group

        /// <summary>
        /// Enter a value for 'Description' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterDescriptionInput(string value)
        {
            descriptionInput.Enter(value);
        }

        /// <summary>
        /// Select an item of 'FromHour' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectFromHourDropDown(string value)
        {
            fromHourDropDown.Select(value);
        }

        /// <summary>
        /// Select an item of 'FromMinute' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectFromMinuteDropDown(string value)
        {
            fromMinuteDropDown.Select(value);
        }

        /// <summary>
        /// Select an item of 'ToHour' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectToHourDropDown(string value)
        {
            toHourDropDown.Select(value);
        }

        /// <summary>
        /// Select an item of 'ToMinute' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectToMinuteDropDown(string value)
        {
            toMinuteDropDown.Select(value);
        }

        /// <summary>
        /// Select an item of 'PeriodicityMinutes' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectPeriodicityMinutesDropDown(string value)
        {
            periodicityMinutesDropDown.Select(value);
        }

        /// <summary>
        /// Select an item of 'LampFailures' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectLampFailuresListDropDown(string value)
        {
            lampFailuresListDropDown.Select(value);
        }

        /// <summary>
        /// Remove a selected item of 'LampFailures' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void RemoveLampFailuresListDropDownValue(string value)
        {
            lampFailuresListDropDown.RemoveValueInList(value);
        }

        /// <summary>
        /// Click 'LampFailuresSelectAll' button
        /// </summary>
        public void ClickLampFailuresSelectAllButton()
        {
            lampFailuresSelectAllButton.ClickEx();
        }

        /// <summary>
        /// Enter a value for 'LampThreshold' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterLampThresholdNumericInput(string value)
        {
            lampThresholdNumericInput.Enter(value);
        }

        /// <summary>
        /// Select an item of 'CommFailures' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectCommFailuresListDropDown(string value)
        {
            commFailuresListDropDown.Select(value);
        }

        /// <summary>
        /// Remove a selected item of 'CommFailures' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void RemoveCommFailuresListDropDownValue(string value)
        {
            commFailuresListDropDown.RemoveValueInList(value);
        }

        /// <summary>
        /// Click 'CommFailuresSelectAll' button
        /// </summary>
        public void ClickCommFailuresSelectAllButton()
        {
            commFailuresSelectAllButton.ClickEx();
        }

        /// <summary>
        /// Enter a value for 'CommThreshold' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterCommThresholdNumericInput(string value)
        {
            commThresholdNumericInput.Enter(value);
        }

        /// <summary>
        /// Select an item of 'ReportDetails' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectReportDetailsDropDown(string value)
        {
            reportDetailsDropDown.Select(value);
        }

        /// <summary>
        /// Select an item of 'FilteringMode' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectFilteringModeDropDown(string value)
        {
            filteringModeDropDown.Select(value);
        }

        /// <summary>
        /// Enter a value for 'CriticalLifetime' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterCriticalLifetimeNumericInput(string value)
        {
            criticalLifetimeNumericInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'NumberOfDays' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterNumberOfDaysInput(string value)
        {
            numberOfDaysInput.Enter(value);
        }

        /// <summary>
        /// Select an item of 'DownFailures' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectDownFailuresListDropDown(string value)
        {
            downFailuresListDropDown.Select(value);
        }

        /// <summary>
        /// Remove a selected item of 'DownFailures' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void RemoveDownFailuressListDropDownValue(string value)
        {
            downFailuresListDropDown.RemoveValueInList(value);
        }

        /// <summary>
        /// Click 'DownFailuresSelectAll' button
        /// </summary>
        public void ClickDownFailuresSelectAllButton()
        {
            downFailuresSelectAllButton.ClickEx();
        }

        /// <summary>
        /// Enter a value for 'ContactType' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterContactTypeInput(string value)
        {
            contactTypeInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'CustomerFullname' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterCustomerFullnameInput(string value)
        {
            customerFullnameInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'CustomerShortCode' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterCustomerShortCodeInput(string value)
        {
            customerShortCodeInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'ServiceCode' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterServiceCodeInput(string value)
        {
            serviceCodeInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'UnitName' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterUnitNameInput(string value)
        {
            unitNameInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'WebCustomerId' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterWebCustomerIdInput(string value)
        {
            webCustomerIdInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'MinimumDeltaTime' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterMinimumDeltaTimeNumericInput(string value)
        {
            minimumDeltaTimeNumericInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'MinimumDeltaLevel' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterMinimumDeltaLevelNumericInput(string value)
        {
            minimumDeltaLevelNumericInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'MaxRetry' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterMaxRetryNumericInput(string value)
        {
            maxRetryNumericInput.Enter(value);
        }

        #endregion //Configuration group

        #endregion //Properties tab

        #region Scheduler tab

        #region Configuration/BasicScheduler group

        /// <summary>
        /// Select an item of 'Hour' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectHourDropDown(string value)
        {
            hourDropDown.Select(value);
        }

        /// <summary>
        /// Select an item of 'Minute' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectMinuteDropDown(string value)
        {
            minuteDropDown.Select(value);
        }

        /// <summary>
        /// Select an item of 'Periodicity' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectPeriodicityDropDown(string value)
        {
            periodicityDropDown.Select(value);
        }        

        /// <summary>
        /// Select an item of 'Timezone' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectTimezoneDropDown(string value)
        {
            timezoneDropDown.Select(value);
        }

        #endregion //Configuration/BasicScheduler group

        #endregion //Scheduler tab

        #region Export tab

        #region FTP group

        /// <summary>
        /// Enter a value for 'FtpHost' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterFtpHostInput(string value)
        {
            ftpHostInput.Enter(value);
        }

        /// <summary>
        /// Tick 'SftpMode' checkbox
        /// </summary>
        /// <param name="value"></param>
        public void TickSftpModeCheckbox(bool value)
        {
            sftpModeCheckbox.Check(value);
        }

        /// <summary>
        /// Enter a value for 'FtpUser' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterFtpUserInput(string value)
        {
            ftpUserInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'FtpPassword' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterFtpPasswordInput(string value)
        {
            ftpPasswordInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'FtpDirectory' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterFtpDirectoryInput(string value)
        {
            ftpDirectoryInput.Enter(value);
        }

        /// <summary>
        /// Tick 'FtpPassiveMode' checkbox
        /// </summary>
        /// <param name="value"></param>
        public void TickFtpPassiveModeCheckbox(bool value)
        {
            ftpPassiveModeCheckbox.Check(value);
        }

        /// <summary>
        /// Enter a value for 'FtpFilename' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterFtpFilenameInput(string value)
        {
            ftpFilenameInput.Enter(value);
        }

        /// <summary>
        /// Tick 'FtpEnabled' checkbox
        /// </summary>
        /// <param name="value"></param>
        public void TickFtpEnabledCheckbox(bool value)
        {
            ftpEnabledCheckbox.Check(value);
        }

        /// <summary>
        /// Select an item of 'FtpFormat' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectFtpFormatDropDown(string value)
        {
            ftpFormatDropDown.Select(value);
        }

        #endregion //FTP group

        #region Mail group

        /// <summary>
        /// Tick 'MailEnabled' checkbox
        /// </summary>
        /// <param name="value"></param>
        public void TickMailEnabledCheckbox(bool value)
        {
            mailEnabledCheckbox.Check(value);
        }

        /// <summary>
        /// Select an item of 'MailFormat' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectMailFormatDropDown(string value)
        {
            mailFormatDropDown.Select(value);
        }

        /// <summary>
        /// Tick 'ReportMailBody' checkbox
        /// </summary>
        /// <param name="value"></param>
        public void TickReportMailBodyCheckbox(bool value)
        {
            reportMailBodyCheckbox.Check(value);
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
        /// Enter a value for 'From' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterFromInput(string value)
        {
            fromInput.Enter(value);
        }

        /// <summary>
        /// Select an item of 'Contacts' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectContactsListDropDown(string value)
        {
            contactsListDropDown.Select(value);
        }

        /// <summary>
        /// Remove a selected item of 'Contacts' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void RemoveContactsListDropDownValue(string value)
        {
            contactsListDropDown.RemoveValueInList(value);
        }

        /// <summary>
        /// Click 'ContactsSelectAll' button
        /// </summary>
        public void ClickContactsSelectAllButton()
        {
            contactsSelectAllButton.ClickEx();
        }

        #endregion //Mail group

        #region Configuration group

        /// <summary>
        /// Tick 'HtmlFormat' checkbox
        /// </summary>
        /// <param name="value"></param>
        public void TickHtmlFormatCheckbox(bool value)
        {
            htmlFormatCheckbox.Check(value);
        }

        #endregion //Configuration group

        #endregion //Export tab

        #region Report tab

        #region Configuration group

        /// <summary>
        /// Select an item of 'DeviceCategories' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectDeviceCategoriesListDropDown(string value)
        {
            deviceCategoriesListDropDown.Select(value);
        }

        /// <summary>
        /// Remove a selected item of 'DeviceCategories' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void RemoveDeviceCategoriesListDropDownValue(string value)
        {
            deviceCategoriesListDropDown.RemoveValueInList(value);
        }

        /// <summary>
        /// Click 'DeviceCategoriesSelectAll' button
        /// </summary>
        public void ClickDeviceCategoriesSelectAllButton()
        {
            deviceCategoriesSelectAllButton.ClickEx();
        }

        /// <summary>
        /// Tick 'Recurse' checkbox
        /// </summary>
        /// <param name="value"></param>
        public void TickRecurseCheckbox(bool value)
        {
            recurseCheckbox.Check(value);
        }

        /// <summary>
        /// Select an item of 'Values' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectValuesListDropDown(string value)
        {
            valuesListDropDown.Select(value);
        }

        /// <summary>
        /// Remove a selected item of 'Values' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void RemoveValuesListDropDownValue(string value)
        {
            valuesListDropDown.RemoveValueInList(value);
        }

        /// <summary>
        /// Click 'ValuesSelectAll' button
        /// </summary>
        public void ClickValuesSelectAllButton()
        {
            valuesSelectAllButton.ClickEx();
        }

        /// <summary>
        /// Select an item of 'Meanings' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectMeaningsListDropDown(string value)
        {
            meaningsListDropDown.Select(value);
        }

        /// <summary>
        /// Remove a selected item of 'Meanings' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void RemoveMeaningsListDropDownValue(string value)
        {
            meaningsListDropDown.RemoveValueInList(value);
        }

        /// <summary>
        /// Click 'MeaningsSelectAll' button
        /// </summary>
        public void ClickMeaningsSelectAllButton()
        {
            meaningsSelectAllButton.ClickEx();
        }

        /// <summary>
        /// Tick 'LastValueOnly' checkbox
        /// </summary>
        /// <param name="value"></param>
        public void TickLastValueOnlyCheckbox(bool value)
        {
            lastValueOnlyCheckbox.Check(value);
        }

        /// <summary>
        /// Select an item of 'Command' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectCommandDropDown(string value)
        {
            commandDropDown.Select(value);
        }

        /// <summary>
        /// Select an item of 'Feedback' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectFeedbackDropDown(string value)
        {
            feedbackDropDown.Select(value);
        }

        /// <summary>
        /// Select an item of 'ModeMeaning' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectModeMeaningDropDown(string value)
        {
            modeMeaningDropDown.Select(value);
        }

        /// <summary>
        /// Select an item of 'FromToMode' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectFromToModeDropDown(string value)
        {
            fromToModeDropDown.Select(value);
        }

        /// <summary>
        /// Select an item of 'FromLastHours' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectFromLastHoursDropDown(string value)
        {
            fromLastHoursDropDown.Select(value);
        }

        /// <summary>
        /// Select an item of 'FromLocalTime' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectFromLocalTimeDropDown(string value)
        {
            fromLocalTimeDropDown.Select(value);
        }

        /// <summary>
        /// Select an item of 'ToLocalTime' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectToLocalTimeDropDown(string value)
        {
            toLocalTimeDropDown.Select(value);
        }

        /// <summary>
        /// Enter a value for 'CommandValue' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterCommandValueInput(string value)
        {
            commandValueInput.Enter(value);
        }

        /// <summary>
        /// Select an item of 'Period' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectPeriodDropDown(string value)
        {
            periodDropDown.Select(value);
        }

        /// <summary>
        /// Enter a value for 'StartDay' date input
        /// </summary>
        /// <param name="value"></param>
        public void EnterStartDayDateInput(DateTime value)
        {
            startDayDateInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'StartTime' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterStartTimeInput(string value)
        {
            startTimeInput.EnterTime(value);
        }

        /// <summary>
        /// Enter a value for 'EndDay' date input
        /// </summary>
        /// <param name="value"></param>
        public void EnterEndDayDateInput(DateTime value)
        {
            endDayDateInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'EndTime' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterEndTimeInput(string value)
        {
            endTimeInput.EnterTime(value);
        }

        /// <summary>
        /// Select an item of 'SavedSearch' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectSavedSearchDropDown(string value)
        {
            savedSearchDropDown.Select(value);
        }

        /// <summary>
        /// Tick 'TimestampColumns' checkbox
        /// </summary>
        /// <param name="value"></param>
        public void TickTimestampColumnsCheckbox(bool value)
        {
            timestampColumnsCheckbox.Check(value);
        }

        #endregion //Configuration group

        #endregion //Report tab

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
        /// Get 'Type' label text
        /// </summary>
        /// <returns></returns>
        public string GetTypeText()
        {
            return typeLabel.Text;
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
        /// Get 'Type' input value
        /// </summary>
        /// <returns></returns>
        public string GetTypeValue()
        {
            return typeDropDown.Text;
        }

        #endregion //Content Identity

        #region Properties tab

        #region Configuration group

        /// <summary>
        /// Get 'Description' label text
        /// </summary>
        /// <returns></returns>
        public string GetDescriptionText()
        {
            return descriptionLabel.Text;
        }

        /// <summary>
        /// Get 'FromHour' label text
        /// </summary>
        /// <returns></returns>
        public string GetFromHourText()
        {
            return fromHourLabel.Text;
        }

        /// <summary>
        /// Get 'FromMinute' label text
        /// </summary>
        /// <returns></returns>
        public string GetFromMinuteText()
        {
            return fromMinuteLabel.Text;
        }

        /// <summary>
        /// Get 'ToHour' label text
        /// </summary>
        /// <returns></returns>
        public string GetToHourText()
        {
            return toHourLabel.Text;
        }

        /// <summary>
        /// Get 'ToMinute' label text
        /// </summary>
        /// <returns></returns>
        public string GetToMinuteText()
        {
            return toMinuteLabel.Text;
        }

        /// <summary>
        /// Get 'PeriodicityMinutes' label text
        /// </summary>
        /// <returns></returns>
        public string GetPeriodicityMinutesText()
        {
            return periodicityMinutesLabel.Text;
        }

        /// <summary>
        /// Get 'LampFailures' label text
        /// </summary>
        /// <returns></returns>
        public string GetLampFailuresText()
        {
            return lampFailuresLabel.Text;
        }

        /// <summary>
        /// Get 'LampThreshold' label text
        /// </summary>
        /// <returns></returns>
        public string GetLampThresholdText()
        {
            return lampThresholdLabel.Text;
        }

        /// <summary>
        /// Get 'CommFailures' label text
        /// </summary>
        /// <returns></returns>
        public string GetCommFailuresText()
        {
            return commFailuresLabel.Text;
        }

        /// <summary>
        /// Get 'CommThreshold' label text
        /// </summary>
        /// <returns></returns>
        public string GetCommThresholdText()
        {
            return commThresholdLabel.Text;
        }

        /// <summary>
        /// Get 'ReportDetails' label text
        /// </summary>
        /// <returns></returns>
        public string GetReportDetailsText()
        {
            return reportDetailsLabel.Text;
        }

        /// <summary>
        /// Get 'FilteringMode' label text
        /// </summary>
        /// <returns></returns>
        public string GetFilteringModeText()
        {
            return filteringModeLabel.Text;
        }

        /// <summary>
        /// Get 'CriticalLifetime' label text
        /// </summary>
        /// <returns></returns>
        public string GetCriticalLifetimeText()
        {
            return criticalLifetimeLabel.Text;
        }

        /// <summary>
        /// Get 'NumberOfDays' label text
        /// </summary>
        /// <returns></returns>
        public string GetNumberOfDaysText()
        {
            return numberOfDaysLabel.Text;
        }

        /// <summary>
        /// Get 'DownMeanings' label text
        /// </summary>
        /// <returns></returns>
        public string GetDownMeaningsText()
        {
            return downMeaningsLabel.Text;
        }

        /// <summary>
        /// Get 'ContactType' label text
        /// </summary>
        /// <returns></returns>
        public string GetContactTypeText()
        {
            return contactTypeLabel.Text;
        }

        /// <summary>
        /// Get 'CustomerFullname' label text
        /// </summary>
        /// <returns></returns>
        public string GetCustomerFullnameText()
        {
            return customerFullnameLabel.Text;
        }

        /// <summary>
        /// Get 'CustomerShortCode' label text
        /// </summary>
        /// <returns></returns>
        public string GetCustomerShortCodeText()
        {
            return customerShortCodeLabel.Text;
        }

        /// <summary>
        /// Get 'ServiceCode' label text
        /// </summary>
        /// <returns></returns>
        public string GetServiceCodeText()
        {
            return serviceCodeLabel.Text;
        }

        /// <summary>
        /// Get 'UnitName' label text
        /// </summary>
        /// <returns></returns>
        public string GetUnitNameText()
        {
            return unitNameLabel.Text;
        }

        /// <summary>
        /// Get 'WebCustomerId' label text
        /// </summary>
        /// <returns></returns>
        public string GetWebCustomerIdText()
        {
            return webCustomerIdLabel.Text;
        }

        /// <summary>
        /// Get 'MinimumDeltaTime' label text
        /// </summary>
        /// <returns></returns>
        public string GetMinimumDeltaTimeText()
        {
            return minimumDeltaTimeLabel.Text;
        }

        /// <summary>
        /// Get 'MinimumDeltaLevel' label text
        /// </summary>
        /// <returns></returns>
        public string GetMinimumDeltaLevelText()
        {
            return minimumDeltaLevelLabel.Text;
        }

        /// <summary>
        /// Get 'MaxRetry' label text
        /// </summary>
        /// <returns></returns>
        public string GetMaxRetryText()
        {
            return maxRetryLabel.Text;
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
        /// Get 'FromHour' input value
        /// </summary>
        /// <returns></returns>
        public string GetFromHourValue()
        {
            return fromHourDropDown.Text;
        }

        /// <summary>
        /// Get 'FromMinute' input value
        /// </summary>
        /// <returns></returns>
        public string GetFromMinuteValue()
        {
            return fromMinuteDropDown.Text;
        }

        /// <summary>
        /// Get 'ToHour' input value
        /// </summary>
        /// <returns></returns>
        public string GetToHourValue()
        {
            return toHourDropDown.Text;
        }

        /// <summary>
        /// Get 'ToMinute' input value
        /// </summary>
        /// <returns></returns>
        public string GetToMinuteValue()
        {
            return toMinuteDropDown.Text;
        }

        /// <summary>
        /// Get 'PeriodicityMinutes' input value
        /// </summary>
        /// <returns></returns>
        public string GetPeriodicityMinutesValue()
        {
            return periodicityMinutesDropDown.Text;
        }

        /// <summary>
        /// Get all selected values of 'LampFailures'
        /// </summary>
        /// <returns></returns>
        public List<string> GetLampFailuresValues()
        {
            return lampFailuresListDropDown.GetSelectedItems();
        }

        /// <summary>
        /// Get 'LampThreshold' input value
        /// </summary>
        /// <returns></returns>
        public string GetLampThresholdValue()
        {
            return lampThresholdNumericInput.Value();
        }

        /// <summary>
        /// Get all selected values of 'CommFailures'
        /// </summary>
        /// <returns></returns>
        public List<string> GetCommFailuresValues()
        {
            return commFailuresListDropDown.GetSelectedItems();
        }

        /// <summary>
        /// Get 'CommThreshold' input value
        /// </summary>
        /// <returns></returns>
        public string GetCommThresholdValue()
        {
            return commThresholdNumericInput.Value();
        }

        /// <summary>
        /// Get 'ReportDetails' input value
        /// </summary>
        /// <returns></returns>
        public string GetReportDetailsValue()
        {
            return reportDetailsDropDown.Text;
        }

        /// <summary>
        /// Get 'FilteringMode' input value
        /// </summary>
        /// <returns></returns>
        public string GetFilteringModeValue()
        {
            return filteringModeDropDown.Text;
        }

        /// <summary>
        /// Get 'CriticalLifetime' input value
        /// </summary>
        /// <returns></returns>
        public string GetCriticalLifetimeValue()
        {
            return criticalLifetimeNumericInput.Value();
        }

        /// <summary>
        /// Get 'NumberOfDays' input value
        /// </summary>
        /// <returns></returns>
        public string GetNumberOfDaysValue()
        {
            return numberOfDaysInput.Value();
        }

        /// <summary>
        /// Get all selected values of 'DownFailures'
        /// </summary>
        /// <returns></returns>
        public List<string> GetDownFailuresValues()
        {
            return downFailuresListDropDown.GetSelectedItems();
        }

        /// <summary>
        /// Get 'ContactType' input value
        /// </summary>
        /// <returns></returns>
        public string GetContactTypeValue()
        {
            return contactTypeInput.Value();
        }

        /// <summary>
        /// Get 'CustomerFullname' input value
        /// </summary>
        /// <returns></returns>
        public string GetCustomerFullnameValue()
        {
            return customerFullnameInput.Value();
        }

        /// <summary>
        /// Get 'CustomerShortCode' input value
        /// </summary>
        /// <returns></returns>
        public string GetCustomerShortCodeValue()
        {
            return customerShortCodeInput.Value();
        }

        /// <summary>
        /// Get 'ServiceCode' input value
        /// </summary>
        /// <returns></returns>
        public string GetServiceCodeValue()
        {
            return serviceCodeInput.Value();
        }

        /// <summary>
        /// Get 'UnitName' input value
        /// </summary>
        /// <returns></returns>
        public string GetUnitNameValue()
        {
            return unitNameInput.Value();
        }

        /// <summary>
        /// Get 'WebCustomerId' input value
        /// </summary>
        /// <returns></returns>
        public string GetWebCustomerIdValue()
        {
            return webCustomerIdInput.Value();
        }

        /// <summary>
        /// Get 'MinimumDeltaTime' input value
        /// </summary>
        /// <returns></returns>
        public string GetMinimumDeltaTimeValue()
        {
            return minimumDeltaTimeNumericInput.Value();
        }

        /// <summary>
        /// Get 'MinimumDeltaLevel' input value
        /// </summary>
        /// <returns></returns>
        public string GetMinimumDeltaLevelValue()
        {
            return minimumDeltaLevelNumericInput.Value();
        }

        /// <summary>
        /// Get 'MaxRetry' input value
        /// </summary>
        /// <returns></returns>
        public string GetMaxRetryValue()
        {
            return maxRetryNumericInput.Value();
        }

        #endregion //Configuration group

        #endregion //Properties tab

        #region Scheduler tab

        #region Configuration/BasicScheduler group

        /// <summary>
        /// Get 'Hour' label text
        /// </summary>
        /// <returns></returns>
        public string GetHourText()
        {
            return hourLabel.Text;
        }

        /// <summary>
        /// Get 'Minute' label text
        /// </summary>
        /// <returns></returns>
        public string GetMinuteText()
        {
            return minuteLabel.Text;
        }

        /// <summary>
        /// Get 'Periodicity' label text
        /// </summary>
        /// <returns></returns>
        public string GetPeriodicityText()
        {
            return periodicityLabel.Text;
        }

        /// <summary>
        /// Get 'Hour' input value
        /// </summary>
        /// <returns></returns>
        public string GetHourValue()
        {
            return hourDropDown.Text;
        }

        /// <summary>
        /// Get 'Minute' input value
        /// </summary>
        /// <returns></returns>
        public string GetMinuteValue()
        {
            return minuteDropDown.Text;
        }

        /// <summary>
        /// Get 'Periodicity' input value
        /// </summary>
        /// <returns></returns>
        public string GetPeriodicityValue()
        {
            return periodicityDropDown.Text;
        }        

        /// <summary>
        /// Get 'Timezone' dropdown value
        /// </summary>
        /// <returns></returns>
        public string GetTimezoneValue()
        {
            return timezoneDropDown.Text;
        }

        #endregion //Configuration/BasicScheduler group

        #endregion //Scheduler tab

        #region Export tab

        #region FTP group

        /// <summary>
        /// Get 'FtpHost' label text
        /// </summary>
        /// <returns></returns>
        public string GetFtpHostText()
        {
            return ftpHostLabel.Text;
        }

        /// <summary>
        /// Get 'SftpMode' label text
        /// </summary>
        /// <returns></returns>
        public string GetSftpModeText()
        {
            return sftpModeLabel.Text;
        }

        /// <summary>
        /// Get 'FtpUser' label text
        /// </summary>
        /// <returns></returns>
        public string GetFtpUserText()
        {
            return ftpUserLabel.Text;
        }

        /// <summary>
        /// Get 'FtpPassword' label text
        /// </summary>
        /// <returns></returns>
        public string GetFtpPasswordText()
        {
            return ftpPasswordLabel.Text;
        }

        /// <summary>
        /// Get 'FtpDirectory' label text
        /// </summary>
        /// <returns></returns>
        public string GetFtpDirectoryText()
        {
            return ftpDirectoryLabel.Text;
        }

        /// <summary>
        /// Get 'FtpPassiveMode' label text
        /// </summary>
        /// <returns></returns>
        public string GetFtpPassiveModeText()
        {
            return ftpPassiveModeLabel.Text;
        }

        /// <summary>
        /// Get 'FtpFilename' label text
        /// </summary>
        /// <returns></returns>
        public string GetFtpFilenameText()
        {
            return ftpFilenameLabel.Text;
        }

        /// <summary>
        /// Get 'FtpEnabled' label text
        /// </summary>
        /// <returns></returns>
        public string GetFtpEnabledText()
        {
            return ftpEnabledLabel.Text;
        }

        /// <summary>
        /// Get 'FtpFormat' label text
        /// </summary>
        /// <returns></returns>
        public string GetFtpFormatText()
        {
            return ftpFormatLabel.Text;
        }

        /// <summary>
        /// Get 'FtpHost' input value
        /// </summary>
        /// <returns></returns>
        public string GetFtpHostValue()
        {
            return ftpHostInput.Value();
        }

        /// <summary>
        /// Get 'SftpMode' input value
        /// </summary>
        /// <returns></returns>
        public bool GetSftpModeValue()
        {
            return sftpModeCheckbox.CheckboxValue();
        }

        /// <summary>
        /// Get 'FtpUser' input value
        /// </summary>
        /// <returns></returns>
        public string GetFtpUserValue()
        {
            return ftpUserInput.Value();
        }

        /// <summary>
        /// Get 'FtpPassword' input value
        /// </summary>
        /// <returns></returns>
        public string GetFtpPasswordValue()
        {
            return ftpPasswordInput.Value();
        }

        /// <summary>
        /// Get 'FtpDirectory' input value
        /// </summary>
        /// <returns></returns>
        public string GetFtpDirectoryValue()
        {
            return ftpDirectoryInput.Value();
        }

        /// <summary>
        /// Get 'FtpPassiveMode' input value
        /// </summary>
        /// <returns></returns>
        public bool GetFtpPassiveModeValue()
        {
            return ftpPassiveModeCheckbox.CheckboxValue();
        }

        /// <summary>
        /// Get 'FtpFilename' input value
        /// </summary>
        /// <returns></returns>
        public string GetFtpFilenameValue()
        {
            return ftpFilenameInput.Value();
        }

        /// <summary>
        /// Get 'FtpEnabled' input value
        /// </summary>
        /// <returns></returns>
        public bool GetFtpEnabledValue()
        {
            return ftpEnabledCheckbox.CheckboxValue();
        }

        /// <summary>
        /// Get 'FtpFormat' input value
        /// </summary>
        /// <returns></returns>
        public string GetFtpFormatValue()
        {
            return ftpFormatDropDown.Text;
        }

        #endregion //FTP group

        #region Mail group

        /// <summary>
        /// Get 'MailEnabled' label text
        /// </summary>
        /// <returns></returns>
        public string GetMailEnabledText()
        {
            return mailEnabledLabel.Text;
        }

        /// <summary>
        /// Get 'MailFormat' label text
        /// </summary>
        /// <returns></returns>
        public string GetMailFormatText()
        {
            return mailFormatLabel.Text;
        }

        /// <summary>
        /// Get 'ReportMailBody' label text
        /// </summary>
        /// <returns></returns>
        public string GetReportMailBodyText()
        {
            return reportMailBodyLabel.Text;
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
        /// Get 'From' label text
        /// </summary>
        /// <returns></returns>
        public string GetFromText()
        {
            return fromLabel.Text;
        }

        /// <summary>
        /// Get 'Contacts' label text
        /// </summary>
        /// <returns></returns>
        public string GetContactsText()
        {
            return contactsLabel.Text;
        }

        /// <summary>
        /// Get 'MailEnabled' input value
        /// </summary>
        /// <returns></returns>
        public bool GetMailEnabledValue()
        {
            return mailEnabledCheckbox.CheckboxValue();
        }

        /// <summary>
        /// Get 'MailFormat' input value
        /// </summary>
        /// <returns></returns>
        public string GetMailFormatValue()
        {
            return mailFormatDropDown.Text;
        }

        /// <summary>
        /// Get 'ReportMailBody' input value
        /// </summary>
        /// <returns></returns>
        public bool GetReportMailBodyValue()
        {
            return reportMailBodyCheckbox.CheckboxValue();
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
        /// Get 'From' input value
        /// </summary>
        /// <returns></returns>
        public string GetFromValue()
        {
            return fromInput.Value();
        }

        /// <summary>
        /// Get all selected values of 'Contacts'
        /// </summary>
        /// <returns></returns>
        public List<string> GetContactsValues()
        {
            return contactsListDropDown.GetSelectedItems();
        }

        #endregion //Mail group

        #region Configuration group

        /// <summary>
        /// Get 'HtmlFormat' label text
        /// </summary>
        /// <returns></returns>
        public string GetHtmlFormatText()
        {
            return htmlFormatLabel.Text;
        }

        /// <summary>
        /// Get 'HtmlFormat' input value
        /// </summary>
        /// <returns></returns>
        public bool GetHtmlFormatValue()
        {
            return htmlFormatCheckbox.CheckboxValue();
        }

        #endregion //Configuration group

        #endregion //Export tab

        #region Report tab

        #region Configuration group

        /// <summary>
        /// Get 'DeviceCategories' label text
        /// </summary>
        /// <returns></returns>
        public string GetDeviceCategoriesText()
        {
            return deviceCategoriesLabel.Text;
        }

        /// <summary>
        /// Get 'Recurse' label text
        /// </summary>
        /// <returns></returns>
        public string GetRecurseText()
        {
            return recurseLabel.Text;
        }

        /// <summary>
        /// Get 'Values' label text
        /// </summary>
        /// <returns></returns>
        public string GetValuesText()
        {
            return valuesLabel.Text;
        }

        /// <summary>
        /// Get 'Meanings' label text
        /// </summary>
        /// <returns></returns>
        public string GetMeaningsText()
        {
            return meaningsLabel.Text;
        }

        /// <summary>
        /// Get 'LastValueOnly' label text
        /// </summary>
        /// <returns></returns>
        public string GetLastValueOnlyText()
        {
            return lastValueOnlyLabel.Text;
        }

        /// <summary>
        /// Get 'Command' label text
        /// </summary>
        /// <returns></returns>
        public string GetCommandText()
        {
            return commandLabel.Text;
        }

        /// <summary>
        /// Get 'Feedback' label text
        /// </summary>
        /// <returns></returns>
        public string GetFeedbackText()
        {
            return feedbackLabel.Text;
        }

        /// <summary>
        /// Get 'ModeMeaning' label text
        /// </summary>
        /// <returns></returns>
        public string GetModeMeaningText()
        {
            return modeMeaningLabel.Text;
        }

        /// <summary>
        /// Get 'FromToMode' label text
        /// </summary>
        /// <returns></returns>
        public string GetFromToModeText()
        {
            return fromToModeLabel.Text;
        }

        /// <summary>
        /// Get 'FromLastHours' label text
        /// </summary>
        /// <returns></returns>
        public string GetFromLastHoursText()
        {
            return fromLastHoursLabel.Text;
        }

        /// <summary>
        /// Get 'FromLocalTime' label text
        /// </summary>
        /// <returns></returns>
        public string GetFromLocalTimeText()
        {
            return fromLocalTimeLabel.Text;
        }

        /// <summary>
        /// Get 'ToLocalTime' label text
        /// </summary>
        /// <returns></returns>
        public string GetToLocalTimeText()
        {
            return toLocalTimeLabel.Text;
        }

        /// <summary>
        /// Get 'CommandValue' label text
        /// </summary>
        /// <returns></returns>
        public string GetCommandValueText()
        {
            return commandValueLabel.Text;
        }

        /// <summary>
        /// Get all selected values of 'DeviceCategories'
        /// </summary>
        /// <returns></returns>
        public List<string> GetDeviceCategoriesValues()
        {
            return deviceCategoriesListDropDown.GetSelectedItems();
        }

        /// <summary>
        /// Get 'Recurse' input value
        /// </summary>
        /// <returns></returns>
        public bool GetRecurseValue()
        {
            return recurseCheckbox.CheckboxValue();
        }

        /// <summary>
        /// Get all selected values of 'Values'
        /// </summary>
        /// <returns></returns>
        public List<string> GetValuesValues()
        {
            return valuesListDropDown.GetSelectedItems();
        }

        /// <summary>
        /// Get all selected values of 'Meanings'
        /// </summary>
        /// <returns></returns>
        public List<string> GetMeaningsValues()
        {
            return meaningsListDropDown.GetSelectedItems();
        }

        /// <summary>
        /// Get 'LastValueOnly' input value
        /// </summary>
        /// <returns></returns>
        public bool GetLastValueOnlyValue()
        {
            return lastValueOnlyCheckbox.CheckboxValue();
        }

        /// <summary>
        /// Get 'Command' input value
        /// </summary>
        /// <returns></returns>
        public string GetCommandValue()
        {
            return commandDropDown.Text;
        }

        /// <summary>
        /// Get 'Feedback' input value
        /// </summary>
        /// <returns></returns>
        public string GetFeedbackValue()
        {
            return feedbackDropDown.Text;
        }

        /// <summary>
        /// Get 'ModeMeaning' input value
        /// </summary>
        /// <returns></returns>
        public string GetModeMeaningValue()
        {
            return modeMeaningDropDown.Text;
        }

        /// <summary>
        /// Get 'FromToMode' input value
        /// </summary>
        /// <returns></returns>
        public string GetFromToModeValue()
        {
            return fromToModeDropDown.Text;
        }

        /// <summary>
        /// Get 'FromLastHours' input value
        /// </summary>
        /// <returns></returns>
        public string GetFromLastHoursValue()
        {
            return fromLastHoursDropDown.Text;
        }

        /// <summary>
        /// Get 'FromLocalTime' input value
        /// </summary>
        /// <returns></returns>
        public string GetFromLocalTimeValue()
        {
            return fromLocalTimeDropDown.Text;
        }

        /// <summary>
        /// Get 'ToLocalTime' input value
        /// </summary>
        /// <returns></returns>
        public string GetToLocalTimeValue()
        {
            return toLocalTimeDropDown.Text;
        }

        /// <summary>
        /// Get 'CommandValue' input value
        /// </summary>
        /// <returns></returns>
        public string GetCommandValueValue()
        {
            return commandValueInput.Value();
        }

        /// <summary>
        /// Get 'StartDay' label text
        /// </summary>
        /// <returns></returns>
        public string GetStartDayText()
        {
            return startDayLabel.Text;
        }

        /// <summary>
        /// Get 'StartDay' input value
        /// </summary>
        /// <returns></returns>
        public string GetStartDayValue()
        {
            return startDayDateInput.Value();
        }

        /// <summary>
        /// Get 'StartTime' label text
        /// </summary>
        /// <returns></returns>
        public string GetStartTimeText()
        {
            return startTimeLabel.Text;
        }

        /// <summary>
        /// Get 'StartTime' input value
        /// </summary>
        /// <returns></returns>
        public string GetStartTimeValue()
        {
            return startTimeInput.TimeValue();
        }

        /// <summary>
        /// Get 'EndDay' label text
        /// </summary>
        /// <returns></returns>
        public string GetEndDayText()
        {
            return endDayLabel.Text;
        }

        /// <summary>
        /// Get 'EndDay' input value
        /// </summary>
        /// <returns></returns>
        public string GetEndDayValue()
        {
            return endDayDateInput.Value();
        }

        /// <summary>
        /// Get 'EndTime' label text
        /// </summary>
        /// <returns></returns>
        public string GetEndTimeText()
        {
            return endTimeLabel.Text;
        }

        /// <summary>
        /// Get 'EndTime' input value
        /// </summary>
        /// <returns></returns>
        public string GetEndTimeValue()
        {
            return endTimeInput.TimeValue();
        }

        /// <summary>
        /// Get 'SavedSearch' label text
        /// </summary>
        /// <returns></returns>
        public string GetSavedSearchText()
        {
            return savedSearchLabel.Text;
        }

        /// <summary>
        /// Get 'SavedSearch' dropdown value
        /// </summary>
        /// <returns></returns>
        public string GetSavedSearchValue()
        {
            return savedSearchDropDown.Text;
        }

        /// <summary>
        /// Get 'TimestampColumns' label text
        /// </summary>
        /// <returns></returns>
        public string GetTimestampColumnsText()
        {
            return timestampColumnsLabel.Text;
        }

        /// <summary>
        /// Get 'TimestampColumns' input value
        /// </summary>
        /// <returns></returns>
        public bool GetTimestampColumnsValue()
        {
            return timestampColumnsCheckbox.CheckboxValue();
        }

        #endregion //Configuration group

        #endregion //Report tab

        #endregion //Get methods

        #endregion //Basic methods

        #region Business methods

        /// <summary>
        /// Get all report type items
        /// </summary>
        public List<string> GetListOfReportTypes()
        {
            return typeDropDown.GetAllItems();
        }

        /// <summary>
        /// Get all Periodicity items
        /// </summary>
        public List<string> GetListOfPeriodicity()
        {
            return periodicityDropDown.GetAllItems();
        }

        /// <summary>
        /// Get all tabs as text
        /// </summary>
        public List<string> GetListOfTabsName()
        {
            return JSUtility.GetElementsText("[id$='editor-content-properties-tabs'] div.w2ui-tab");
        }

        /// <summary>
        /// Select a tab
        /// </summary>
        /// <param name="tabName"></param>
        public void SelectTab(string tabName)
        {
            var tab = tabsList.FirstOrDefault(t => t.Text.Equals(tabName, StringComparison.InvariantCultureIgnoreCase));
            if (tab != null)
            {
                tab.ClickEx();
                WebDriverContext.Wait.Until(driver => JSUtility.GetElementText("[id$='editor-content-properties-tabs'] div.w2ui-tab.active") == tabName);
            }
        }

        /// <summary>
        /// Get editor groups
        /// </summary>
        public List<string> GetListOfGroupsName()
        {
            return JSUtility.GetElementsText("[id$='geozone-editor'] .editor-group-header div[dir]");
        }

        public List<string> GetListOfGroupsNameActiveTab()
        {
            return JSUtility.GetElementsText("[id$='geozone-editor'] .editor-tab[style*='display: block'] .editor-group-header div[dir]");
        }

        /// <summary>
        /// Expand an editor group by its caption
        /// </summary>
        /// <param name="groupName"></param>
        public void ExpandGroup(string groupName)
        {
            foreach (var editorGroupElement in editorGroupHeaderList)
            {
                if (editorGroupElement.FindElement(By.CssSelector("div:nth-child(2)")).Text.Contains(groupName))
                {
                    if (editorGroupElement.FindElements(By.CssSelector("div.icon-collapsed")).Count > 0)
                    {
                        editorGroupElement.FindElement(By.CssSelector("div.icon-collapsed")).ClickByJS();

                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Expand all group of current active tab
        /// </summary>
        public void ExpandGroupsActiveTab()
        {
            var groups = Driver.FindElements(By.CssSelector("[id$='geozone-editor'] .editor-tab[style*='display: block'] .editor-group-header"));
            foreach (var group in groups)
            {
                if (group.FindElements(By.CssSelector("div.icon-collapsed")).Count > 0)
                {
                    group.FindElement(By.CssSelector("div.icon-collapsed")).ClickByJS();
                }
            }
        }

        public bool IsNameFieldDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='geozone-editor-content-name-label'] .slv-label.reportmanager-editor-label")) && ElementUtility.IsDisplayed(By.CssSelector("[id$='geozone-editor-content-name-field']"));
        }

        public bool IsTypeFieldDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='geozone-editor-content-type-label'] .slv-label.reportmanager-editor-label")) && ElementUtility.IsDisplayed(By.CssSelector("[id$='geozone-editor-content-type-field']"));
        }

        public bool IsNameInputReadOnly()
        {
            return nameInput.IsReadOnly();
        }

        public bool IsTypeDropDownReadOnly()
        {
            return Driver.FindElements(By.CssSelector("[id$='geozone-editor-content-type-field'].select2-container-disabled")).Any();
        }

        public Dictionary<string, object> GetPropertiesAndItems(params string[] exceptNotGetItemsList)
        {
            var tabs = GetListOfTabsName();
            var result = new Dictionary<string, object>();
            foreach (var tab in tabs)
            {
                SelectTab(tab);
                ExpandGroupsActiveTab();
                var properties = Driver.FindElements(By.CssSelector("[id$='geozone-editor'] .editor-tab[style*='display: block'] .editor-property"));

                foreach (var property in properties)
                {
                    var label = property.FindElement(By.CssSelector("div.editor-label:nth-child(1)"));
                    var labelText = label.Text.Trim();
                    var editor = property.FindElement(By.CssSelector("[id^='editor-property'][id$='field']"));
                    if (!editor.IsReadOnly())
                    {
                        string value = string.Empty;
                        var ccsClass = editor.GetAttribute("class");
                        if (ccsClass.Contains("editor-select"))//dropdown
                        {
                            if (!exceptNotGetItemsList.Contains(labelText))
                            {
                                var items = editor.GetAllItems();
                                result.Add(labelText, items);
                            }
                            else
                            {
                                result.Add(labelText, "dropdown");
                            }
                        }
                        else if (ccsClass.Contains("editor-field")) //input
                        {
                            result.Add(labelText, "input");
                        }
                        else if (ccsClass.Contains("checkbox")) //checkbox
                        {
                            result.Add(labelText, "checkbox");
                        }
                    }
                }
            }

            return result;
        }

        public Dictionary<string, object> GetPropertiesAndItemsActiveTab(params string[] exceptNotGetItemsList)
        {
            var result = new Dictionary<string, object>();

            var properties = Driver.FindElements(By.CssSelector("[id$='geozone-editor'] .editor-tab[style*='display: block'] .editor-property"));
            foreach (var property in properties)
            {
                var label = property.FindElement(By.CssSelector("div.editor-label:nth-child(1)"));
                var labelText = label.Text.Trim();
                var editor = property.FindElement(By.CssSelector("[id^='editor-property'][id$='field']"));
                if (!editor.IsReadOnly())
                {
                    string value = string.Empty;
                    var ccsClass = editor.GetAttribute("class");
                    if (ccsClass.Contains("editor-select"))//dropdown
                    {
                        if (!exceptNotGetItemsList.Contains(labelText))
                        {
                            var items = editor.GetAllItems();
                            result.Add(labelText, items);
                        }
                        else
                        {
                            result.Add(labelText, "dropdown");
                        }
                    }
                    else if (ccsClass.Contains("editor-field")) //input
                    {
                        result.Add(labelText, "input");
                    }
                    else if (ccsClass.Contains("checkbox")) //checkbox
                    {
                        result.Add(labelText, "checkbox");
                    }
                }
            }
            return result;
        }

        public Dictionary<string, string> GetPropertiesAndValues(params string[] exceptList)
        {
            var tabs = GetListOfTabsName();
            var result = new Dictionary<string, string>();

            foreach (var tab in tabs)
            {
                SelectTab(tab);
                ExpandGroupsActiveTab();

                var properties = Driver.FindElements(By.CssSelector("[id$='geozone-editor'] .editor-tab[style*='display: block'] .editor-property"));
                foreach (var property in properties)
                {
                    var label = property.FindElement(By.CssSelector("div.editor-label:nth-child(1)"));
                    var labelText = label.Text.Trim();
                    if (!exceptList.Contains(labelText))
                    {
                        var editor = property.FindElement(By.CssSelector("[id^='editor-property'][id$='field']"));
                        if (!editor.IsReadOnly())
                        {
                            string value = string.Empty;
                            var ccsClass = editor.GetAttribute("class");
                            if (ccsClass.Contains("editor-select"))//dropdown
                            {
                                var isListBox = editor.FindElements(By.CssSelector("button")).Count > 0;

                                if (!isListBox)
                                    value = editor.Text;
                                else
                                    value = editor.GetSelectedItems().FirstOrDefault();
                            }
                            else if (ccsClass.Contains("editor-field")) //input
                            {
                                value = editor.Value();
                            }
                            else if (ccsClass.Contains("checkbox")) //checkbox
                            {
                                Wait.ForMilliseconds(500);
                                value = editor.CheckboxValue().ToString();
                            }
                            result.Add(labelText, value);
                        }
                    }
                }
            }

            return result;
        }

        public void EnterEditablePropertiesValue(params string[] exceptList)
        {
            var tabs = GetListOfTabsName();

            foreach (var tab in tabs)
            {
                SelectTab(tab);
                ExpandGroupsActiveTab();
                var properties = Driver.FindElements(By.CssSelector("[id$='geozone-editor'] .editor-tab[style*='display: block'] .editor-property"));

                foreach (var property in properties)
                {
                    var label = property.FindElement(By.CssSelector("div.editor-label:nth-child(1)"));
                    var fieldName = label.Text.Trim();
                    if (!exceptList.Contains(label.Text.Trim()))
                    {
                        var editor = property.FindElement(By.CssSelector("[id^='editor-property'][id$='field']"));
                        if (!editor.IsReadOnly())
                        {
                            string value = string.Empty;
                            var ccsClass = editor.GetAttribute("class");
                            if (ccsClass.Contains("editor-select"))//dropdown
                            {
                                var isListBox = editor.FindElements(By.CssSelector("button")).Count > 0;
                                value = editor.Text;
                                var items = new List<string>();
                                if (fieldName.Equals("TimeZone"))
                                    items = SLVHelper.GenerateTimezone();
                                else
                                    items = editor.GetAllItems();
                                items.Remove(value);
                                if (items.Any()) editor.Select(items.PickRandom(), isListBox);
                            }
                            else if (ccsClass.Contains("editor-field")) //input
                            {
                                if (property.FindElements(By.CssSelector(".w2ui-field-helper")).Any()) //numeric input
                                {
                                    editor.Enter(SLVHelper.GenerateStringInteger(100));
                                }
                                else //text input
                                {
                                    if (label.Text.ToLower().Contains("date"))
                                    {
                                        editor.Enter(string.Format(@"{0}/{1}/{2}", SLVHelper.GenerateStringInteger(12), SLVHelper.GenerateStringInteger(28), SLVHelper.GenerateStringInteger(1900, DateTime.Now.AddYears(-1).Year)));
                                    }
                                    else if (label.Text.ToLower().Contains("time"))
                                    {
                                        editor.Enter(string.Format("{0}:{1}", SLVHelper.GenerateInteger(23).ToString("D2"), SLVHelper.GenerateInteger(59).ToString("D2")));
                                    }
                                    else
                                        editor.Enter(SLVHelper.GenerateString(9));
                                }
                            }
                            else if (ccsClass.Contains("checkbox")) //checkbox
                            {
                                Wait.ForMilliseconds(500);
                                editor.Check(!editor.CheckboxValue());
                            }
                        }
                    }
                }
            }
        }

        public void SelectAllLampFailureDropDownList()
        {
            lampFailuresListDropDown.SelectAllItems();
        }

        public void SelectRandomLampFailureDropDownList(int count = 2)
        {
            var items = lampFailuresListDropDown.GetAllItems();
            var currentItems = lampFailuresListDropDown.GetSelectedItems();
            items.RemoveAll(p => currentItems.Contains(p));
            var randomItems = items.PickRandom(count);
            lampFailuresListDropDown.SelectItems(randomItems.ToArray());
        }

        public void SelectAllCommFailureDropDownList()
        {
            commFailuresListDropDown.SelectAllItems();
        }

        public void SelectRandomCommFailureDropDownList(int count = 2)
        {
            var items = commFailuresListDropDown.GetAllItems();
            var currentItems = commFailuresListDropDown.GetSelectedItems();
            items.RemoveAll(p => currentItems.Contains(p));
            var randomItems = items.PickRandom(count);
            commFailuresListDropDown.SelectItems(randomItems.ToArray());
        }

        public void SelectAllContactsDropDownList()
        {
            contactsListDropDown.SelectAllItems();
        }

        public void SelectRandomContactsDropDownList(int count = 2)
        {
            var items = contactsListDropDown.GetAllItems();
            var currentItems = contactsListDropDown.GetSelectedItems();
            items.RemoveAll(p => currentItems.Contains(p));
            var randomItems = items.PickRandom(count);
            contactsListDropDown.SelectItems(randomItems.ToArray());
        }

        public void SelectAllDeviceCategoriesDropDownList()
        {
            deviceCategoriesListDropDown.SelectAllItems();
        }

        public void SelectRandomDeviceCategoriesDropDownList(int count = 2)
        {
            var items = deviceCategoriesListDropDown.GetAllItems();
            var currentItems = deviceCategoriesListDropDown.GetSelectedItems();
            items.RemoveAll(p => currentItems.Contains(p));
            var randomItems = items.PickRandom(count);
            deviceCategoriesListDropDown.SelectItems(randomItems.ToArray());
        }

        public void SelectAllValuesDropDownList()
        {
            valuesListDropDown.SelectAllItems();
        }

        public void SelectRandomValuesDropDownList(int count = 2)
        {
            var items = valuesListDropDown.GetAllItems();
            var currentItems = valuesListDropDown.GetSelectedItems();
            items.RemoveAll(p => currentItems.Contains(p));
            var randomItems = items.PickRandom(count);
            valuesListDropDown.SelectItems(randomItems.ToArray());
        }

        public void SelectAllMeaningsDropDownList()
        {
            meaningsListDropDown.SelectAllItems();
        }

        public void SelectRandomMeaningsDropDownList(int count = 2)
        {
            var items = meaningsListDropDown.GetAllItems();
            var currentItems = meaningsListDropDown.GetSelectedItems();
            items.RemoveAll(p => currentItems.Contains(p));
            var randomItems = items.PickRandom(count);
            meaningsListDropDown.SelectItems(randomItems.ToArray());
        }

        public void SelectAllDownFailuresDropDownList()
        {
            downFailuresListDropDown.SelectAllItems();
        }

        public void SelectRandomDownFailuresDropDownList(int count = 2)
        {
            var items = downFailuresListDropDown.GetAllItems();
            var currentItems = downFailuresListDropDown.GetSelectedItems();
            items.RemoveAll(p => currentItems.Contains(p));
            var randomItems = count > items.Count ? items.PickRandom(items.Count) : items.PickRandom(count);
            downFailuresListDropDown.SelectItems(randomItems.ToArray());
        }

        public void SelectAllDownMeaningsDropDownList()
        {
            SelectAllDownFailuresDropDownList();
        }

        public void SelectRandomDownMeaningsDropDownList(int count = 2)
        {
            SelectRandomDownFailuresDropDownList(count);
        }

        public List<string> GetDownMeaningsValues()
        {
            return GetDownFailuresValues();
        }

        public List<string> GetListOfDownMeaningsItems()
        {
            return downFailuresListDropDown.GetAllItems();
        }

        public bool AreFieldsEditable(params string[] exceptList)
        {
            var tabs = GetListOfTabsName();
            var result = new Dictionary<string, string>();

            var editable = true;

            foreach (var tab in tabs)
            {
                SelectTab(tab);
                ExpandGroupsActiveTab();

                var properties = Driver.FindElements(By.CssSelector("[id$='geozone-editor'] .editor-tab[style*='display: block'] .editor-property"));
                foreach (var property in properties)
                {
                    var label = property.FindElement(By.CssSelector("div.editor-label:nth-child(1)"));
                    if (!exceptList.Contains(label.Text.Trim()))
                    {
                        var editor = property.FindElement(By.CssSelector("[id^='editor-property'][id$='field']"));
                        if (!editor.IsReadOnly())
                        {
                            string value = string.Empty;
                            var ccsClass = editor.GetAttribute("class");
                            if (ccsClass.Contains("editor-select"))//dropdown
                            {
                                if (ccsClass.Contains("select2-container-disabled"))
                                    return false;
                            }
                            else if (ccsClass.Contains("editor-field")) //input
                            {
                                if (editor.IsReadOnly())
                                    return false;
                            }
                            else if (ccsClass.Contains("checkbox")) //checkbox
                            {
                                if (editor.FindElement(By.CssSelector("input")).IsReadOnly())
                                    return false;
                            }
                        }
                    }
                }
            }

            return editable;
        }


        /// <summary>
        /// Select random a timezone item
        /// </summary>
        public void SelectRandomTimeZoneDropDown()
        {
            var currentValue = GetTimezoneValue();
            var listItems = SLVHelper.GenerateTimezone();
            listItems.Remove(currentValue);
            timezoneDropDown.Select(listItems.PickRandom());
        }

        #endregion //Business methods        

        public override void WaitForPanelLoaded()
        {
            Wait.ForElementDisplayed(By.CssSelector("[id^='layout'][id$='geozone_panel_right']"));
            Wait.ForElementStyle(By.CssSelector("[id^='layout'][id$='geozone_panel_right']"), "display: block");
            Wait.ForElementText(nameLabel);
            Wait.ForElementText(typeLabel);
        }
    }
}
