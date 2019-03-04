using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using StreetlightVision.Extensions;
using System;
using System.Collections.Generic;

namespace StreetlightVision.Pages.UI
{
    public class StatusWidgetPanel : PanelBase
    {
        #region Variables

        #endregion //Variables

        #region IWebElements        

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] [id$='status-panel'] [id$='status-tab'] > div")]
        private IWebElement inputsOutputssLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] [id$='status-panel'] [id$='status-tab'] > img")]
        private IWebElement statusIcon;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] [id$='status-panel'] [id$='status-tab'] > div")]
        private IWebElement statusLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] [id$='status-panel'] [id$='status-content'] > div")]
        private IList<IWebElement> statusItemList;

        #endregion //IWebElements

        #region Constructor

        public StatusWidgetPanel(IWebDriver driver, PageBase page)
            : base(driver, page)
        {
            PageFactory.InitElements(this, new RetryingElementLocator(driver, TimeSpan.FromMilliseconds(Settings.LocatorRetryingTimeout), TimeSpan.FromMilliseconds(Settings.LocatorPollingInterval)));
            WaitForPanelLoaded();
        }

        #endregion //Constructor

        #region Basic methods   

        #region Actions

        #endregion //Actions

        #region Get methods

        /// <summary>
        /// Get 'InputsOutputss' label text
        /// </summary>
        /// <returns></returns>
        public string GetInputsOutputssText()
        {
            return inputsOutputssLabel.Text;
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
        /// Get 'Status' label text
        /// </summary>
        /// <returns></returns>
        public string GetStatusText()
        {
            return statusLabel.Text;
        }

        #endregion //Get methods

        #endregion //Basic methods

        #region Business methods

        #endregion //Business methods

    }
}
