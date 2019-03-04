using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using StreetlightVision.Extensions;
using System;
using System.Linq;

namespace StreetlightVision.Pages.UI
{
    public class ControlProgramCommandTypePopupPanel : PanelBase
    {
        #region Variables 
               
        #endregion //Variables

        #region IWebElements       

        [FindsBy(How = How.CssSelector, Using = "[id='w2ui-popup'] .w2ui-msg-title")]
        private IWebElement panelTitle;

        [FindsBy(How = How.CssSelector, Using = "[id='w2ui-popup'] .w2ui-msg-close")]
        private IWebElement closeButton;

        [FindsBy(How = How.CssSelector, Using = "[id='w2ui-popup'] [id$='btn-undo']")]
        private IWebElement cancelButton;

        [FindsBy(How = How.CssSelector, Using = "[id='w2ui-popup'] [id$='btn-save']")]
        private IWebElement saveButton;

        [FindsBy(How = How.CssSelector, Using = "[id='w2ui-popup'] [id='slv-schedules-dot-editor-popup'] .select2-container:nth-child(1)")]
        private IWebElement typeDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id='w2ui-popup'] [id='slv-schedules-dot-editor-popup'] .select2-container:nth-child(3)")]
        private IWebElement sunEventIconDropDown;

        #endregion //IWebElements

        #region Constructor

        public ControlProgramCommandTypePopupPanel(IWebDriver driver, PageBase page)
            : base(driver, page) 
        {
            PageFactory.InitElements(this, new RetryingElementLocator(driver, TimeSpan.FromMilliseconds(Settings.LocatorRetryingTimeout), TimeSpan.FromMilliseconds(Settings.LocatorPollingInterval)));
        }

        #endregion //Constructor

        #region Properties        

        #endregion //Properties

        #region Basic methods  

        #region Actions

        /// <summary>
        /// Click 'Close' button
        /// </summary>
        public void ClickCloseButton()
        {
            closeButton.ClickEx();
        }

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
        /// Select an item of 'Type' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectTypeDropDown(string value)
        {
            typeDropDown.Select(value);
        }

        /// <summary>
        /// Select an item of 'SunEventIcon' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectSunEventIconDropDown(string value)
        {
            sunEventIconDropDown.SelectIcon(value);
        }

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
        /// Get 'Type' dropdown value
        /// </summary>
        /// <returns></returns>
        public string GetTypeValue()
        {
            return typeDropDown.Text;
        }

        /// <summary>
        /// Get 'SunEventIcon' dropdown value
        /// </summary>
        /// <returns></returns>
        public string GetSunEventIconValue()
        {
            return sunEventIconDropDown.IconValue();
        }

        #endregion //Get methods

        #endregion //Basic methods

        #region Business methods


        #endregion //Business methods
    }
}