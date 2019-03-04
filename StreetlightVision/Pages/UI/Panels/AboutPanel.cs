using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using StreetlightVision.Extensions;
using System;

namespace StreetlightVision.Pages.UI
{
    public class AboutPanel : PanelBase
    {
        #region Variables

        #endregion //Variables

        #region IWebElements

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-desktop-settings-backButton']")]
        private IWebElement backButton;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-desktop-settings-title'] .side-panel-title-label")]
        private IWebElement panelHeaderLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='desktop-settings-panel-about'] .about-logo.desktop-settings-about-logo")]
        private IWebElement logo;

        [FindsBy(How = How.CssSelector, Using = "[id='desktop-settings-panel-about'] .slv-title")]
        private IWebElement slvTitle;

        [FindsBy(How = How.CssSelector, Using = "[id='desktop-settings-panel-about'] .desktop-settings-label:nth-child(3)")]
        private IWebElement slvVersionLabel;
        
        [FindsBy(How = How.CssSelector, Using = "[id='desktop-settings-panel-about'] .desktop-settings-label:nth-child(4)")]
        private IWebElement slvBuildLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='desktop-settings-panel-about'] .desktop-settings-about-copyright")]
        private IWebElement slvCopyrightLabel;

        #endregion //IWebElements

        #region Constructor

        public AboutPanel(IWebDriver driver, PageBase page)
            : base(driver, page)
        {
            PageFactory.InitElements(this, new RetryingElementLocator(driver, TimeSpan.FromMilliseconds(Settings.LocatorRetryingTimeout), TimeSpan.FromMilliseconds(Settings.LocatorPollingInterval)));
            WaitForPanelLoaded();
        }

        #endregion //Constructor

        #region Basic methods

        #region Actions

        /// <summary>
        /// Click 'Back' button
        /// </summary>
        public void ClickBackButton()
        {
            backButton.ClickEx();
        }

        #endregion //Actions

        #region Get methods

        /// <summary>
        /// Get 'PanelHeader' label text
        /// </summary>
        /// <returns></returns>
        public string GetPanelHeaderText()
        {
            return panelHeaderLabel.Text;
        }

        /// <summary>
        /// Get 'SlvTitle' text
        /// </summary>
        /// <returns></returns>
        public string GetSlvTitleText()
        {
            return slvTitle.Text;
        }

        /// <summary>
        /// Get 'SlvVersion' label text
        /// </summary>
        /// <returns></returns>
        public string GetSlvVersionText()
        {
            return slvVersionLabel.Text;
        }

        /// <summary>
        /// Get 'SlvBuild' label text
        /// </summary>
        /// <returns></returns>
        public string GetSlvBuildText()
        {
            return slvBuildLabel.Text;
        }

        /// <summary>
        /// Get 'SlvCopyRight' label text
        /// </summary>
        /// <returns></returns>
        public string GetSlvCopyrightText()
        {
            return slvCopyrightLabel.Text;
        }

        #endregion //Get methods

        #endregion //Basic methods

        #region Business methods

        #endregion //Business methods

        public override void WaitForPanelLoaded()
        {
            Wait.ForElementStyle(backButton, "display: block");
            Wait.ForElementText(slvCopyrightLabel);
        }
    }
}
