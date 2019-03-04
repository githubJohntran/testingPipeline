using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using StreetlightVision.Extensions;
using StreetlightVision.Utilities;
using System;
using System.Collections.Generic;
using System.Threading;

namespace StreetlightVision.Pages.UI
{
    public class Dialog : PanelBase
    {
        #region Variables

        #endregion //Variables

        #region IWebElements

        [FindsBy(How = How.CssSelector, Using = "[id='w2ui-popup']")]
        private IWebElement dialogContainer;

        [FindsBy(How = How.CssSelector, Using = "[id='w2ui-popup'] .w2ui-msg-title")]
        private IWebElement dialogTitle;

        [FindsBy(How = How.CssSelector, Using = "[id='w2ui-popup'] .w2ui-msg-body .w2ui-centered, [id^='w2ui-message'].w2ui-popup-message div.w2ui-centered")]
        private IWebElement messageLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='w2ui-popup'] .w2ui-msg-buttons > button[onclick^='w2popup.close()'], [id^='w2ui-message'].w2ui-popup-message [onclick^='w2popup.message']")]
        private IWebElement okButton;

        [FindsBy(How = How.CssSelector, Using = "[id='w2ui-popup'] [id='Yes'], [id^='w2ui-message'].w2ui-popup-message [id='Yes']")]
        private IWebElement yesButton;

        [FindsBy(How = How.CssSelector, Using = "[id='w2ui-popup'] [id='No'], [id^='w2ui-message'].w2ui-popup-message [id='No']")]
        private IWebElement noButton;

        [FindsBy(How = How.CssSelector, Using = "[id='alarms_ack_button']")]
        private IWebElement sendButton;

        [FindsBy(How = How.CssSelector, Using = "[id='w2ui-popup'] .w2ui-msg-close")]
        private IWebElement closeButton;

        [FindsBy(How = How.CssSelector, Using = "[id='alarms_ack_msg']")]
        private IWebElement acknowledgeMessageInput;

        [FindsBy(How = How.CssSelector, Using = "[id='w2ui-popup'] [class='w2ui-box1'] [class='w2ui-msg-body'] [class='slv-realtimebatch-confirmation-body']")]
        private IWebElement dialogDescription;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-realtimebatch-confirmation-password']")]
        private IWebElement realTimeBatchPasswordInput;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-realtimebatch-confirmation-submit']")]
        private IWebElement confirmationSubmitButton;

        #endregion //IWebElements

        #region Constructor

        public Dialog(IWebDriver driver, PageBase page)
            : base(driver, page)
        {
            PageFactory.InitElements(this, new RetryingElementLocator(driver, TimeSpan.FromMilliseconds(Settings.LocatorRetryingTimeout), TimeSpan.FromMilliseconds(Settings.LocatorPollingInterval)));

            WaitForPanelLoaded();
        }

        #endregion //Constructor

        #region Basic methods

        #region Actions

        /// <summary>
        /// Click 'Ok' button
        /// </summary>
        public void ClickOkButton()
        {
            okButton.ClickEx();
        }

        /// <summary>
        /// Click 'Yes' button
        /// </summary>
        public void ClickYesButton()
        {
            yesButton.ClickEx();
        }

        /// <summary>
        /// Click 'No' button
        /// </summary>
        public void ClickNoButton()
        {
            noButton.ClickEx();
        }

        /// <summary>
        /// Click 'Send' button
        /// </summary>
        public void ClickSendButton()
        {
            sendButton.ClickEx();
        }

        /// <summary>
        /// Click 'Close' button
        /// </summary>
        public void ClickCloseButton()
        {
            closeButton.ClickEx();
        }

        /// <summary>
        /// Enter 'AcknowledgeMessageInput' input
        /// </summary>
        public void EnterAcknowledgeMessageInput(string value)
        {
            acknowledgeMessageInput.Enter(value);
        }

        /// <summary>
        /// Enter 'RealTimeBatchPasswordInput' input
        /// </summary>
        public void EnterRealTimeBatchPasswordInputInput(string value)
        {
            realTimeBatchPasswordInput.Enter(value);
        }

        /// <summary>
        /// Click 'ConfirmationSubmit' button
        /// </summary>
        public void ClickConfirmationSubmitButton()
        {
            confirmationSubmitButton.ClickEx();
        }

        #endregion //Actions

        #region Get methods

        /// <summary>
        /// Get 'DialogTitle' text
        /// </summary>
        /// <returns></returns>
        public string GetDialogTitleText()
        {
            if (dialogTitle.Text.SplitEx().Count > 1)
                return dialogTitle.Text.SplitAndGetAt(1);
            return dialogTitle.Text;
        }

        /// <summary>
        /// Get 'Message' label text
        /// </summary>
        /// <returns></returns>
        public string GetMessageText()
        {
            return messageLabel.Text;
        }

        /// <summary>
        /// Enter 'AcknowledgeMessageInput' input
        /// </summary>
        public string GetAcknowledgeMessageInputValue()
        {
            return acknowledgeMessageInput.Value();
        }

        /// <summary>
        /// Get 'Dialog description' text without line break
        /// </summary>
        public string GetDialogDescriptionText()
        {
            return dialogDescription.Text.Replace("\r\n", "");
        }

        /// <summary>
        /// Get 'Confirmation Submit Button' text
        /// </summary>
        public string GetConfirmationSubmitButtonText()
        {
            return confirmationSubmitButton.Text;
        }

        #endregion //Get methods

        #endregion //Basic methods

        #region Business methods

        /// <summary>
        /// Check if dialog is visible
        /// </summary>
        /// <returns></returns>
        public bool IsDialogVisible()
        {
            return WebDriverContext.CurrentDriver.FindElements(By.CssSelector("[id='w2ui-popup']")).Count > 0;
        }

        public bool IsPopupMessageDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id='w2ui-popup'] [id='w2ui-message0']"));
        }        

        /// <summary>
        /// Wait for dialog disappeared
        /// </summary>
        public void WaitForDialogDisappeared()
        {
            WebDriverContext.Wait.Until(driver => !IsDialogVisible());
        }

        /// <summary>
        /// Wait for poup message displayed
        /// </summary>
        public void WaitForPopupMessageDisplayed()
        {
            Wait.ForElementDisplayed(By.CssSelector("[id='w2ui-popup'] [id='w2ui-message0']"));
            Wait.ForMilliseconds(500);
        }

        /// <summary>
        /// Wait for poup message disappeared
        /// </summary>
        public void WaitForPopupMessageDisappeared()
        {
            Wait.ForElementInvisible(By.CssSelector("[id='w2ui-popup'] [id='w2ui-message0']"));
            Wait.ForMilliseconds(500);
        }

        public bool IsYesButtonDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id='w2ui-popup'] [id='Yes'], [id^='w2ui-message'].w2ui-popup-message [id='Yes']"));
        }

        public bool IsNoButtonDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id='w2ui-popup'] [id='No'], [id^='w2ui-message'].w2ui-popup-message [id='No']"));
        }

        public bool IsOkButtonDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id='w2ui-popup'] .w2ui-msg-buttons > button[onclick^='w2popup.close()'], [id^='w2ui-message'].w2ui-popup-message [onclick^='w2popup.message']"));
        }

        public List<string> GetListOfFailuresInformation()
        {
            return JSUtility.GetElementsText("[id='w2ui-popup'] .schedulermanager-failures-help-list > li");
        }

        public List<string> GetListOfDaytimePhotocellInformation()
        {
            return JSUtility.GetElementsText("[id='w2ui-popup'] .schedulermanager-help-popup li");
        }

        public bool IsAcknowledgeMessageInputDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id='alarms_ack_msg']"));
        }

        public bool IsRealTimeBatchPasswordInputDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id='slv-realtimebatch-confirmation-password']"));
        }

        #endregion //Business methods        

        public override void WaitForPanelLoaded()
        {
            Wait.ForElementStyle(dialogContainer, "opacity: 1");
            Wait.ForSeconds(2);
        }
    }
}
