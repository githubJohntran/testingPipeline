using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using StreetlightVision.Extensions;
using StreetlightVision.Utilities;

namespace StreetlightVision.Pages.UI
{
    public class StorePanel : PanelBase
    {
        #region Variables

        #endregion //Variables

        #region IWebElements

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-desktop-settings-backButton']")]
        private IWebElement backButton;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-desktop-settings-title'] .side-panel-title-label")]
        private IWebElement panelHeaderLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='desktop-settings-panel-store-button-install']")]
        private IWebElement installButton;

        [FindsBy(How = How.CssSelector, Using = "[id='tabs_desktop-settings-panel-store-tabs_tab_desktop-settings-panel-store-tab-0'] .w2ui-tab")]
        private IWebElement applicationsTab;

        [FindsBy(How = How.CssSelector, Using = "[id='tabs_desktop-settings-panel-store-tabs_tab_desktop-settings-panel-store-tab-1'] .w2ui-tab")]
        private IWebElement widgetsTab;

        [FindsBy(How = How.CssSelector, Using = "[id='desktop-settings-panel-store-descriptor-panel'] > div:nth-child(1) > div.tile.tile-background > img")]
        private IWebElement appOrWidgetIcon;

        [FindsBy(How = How.CssSelector, Using = "[id='desktop-settings-panel-store-descriptor-panel'] > div:nth-child(2)")]
        private IWebElement appOrWidgetStars;

        [FindsBy(How = How.CssSelector, Using = "[id='desktop-settings-panel-store-descriptor-panel'] > div:nth-child(1) > div:nth-child(2) > div:nth-child(1)")]
        private IWebElement appOrWidgetNameLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='desktop-settings-panel-store-descriptor-panel'] > div:nth-child(1) > div:nth-child(2) > div.slv-label")]
        private IWebElement appOrWidgetVerificationLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='desktop-settings-panel-store-descriptor-panel'] > div:nth-child(1) > div:nth-child(2) > div:nth-child(3) > div:nth-child(2)")]
        private IWebElement appOrWidgetSizeLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='desktop-settings-panel-store-descriptor-panel'] > div:nth-child(3) > div:nth-child(2)")]
        private IWebElement appOrWidgetEditorLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='desktop-settings-panel-store-descriptor-panel'] > div:nth-child(4) > div:nth-child(2)")]
        private IWebElement appOrWidgetLastUpdateLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='desktop-settings-panel-store-descriptor-panel'] > div:nth-child(5) > div:nth-child(2)")]
        private IWebElement appOrWidgetVersionLabel;

        [FindsBy(How = How.CssSelector, Using = "div[id$='desktop-settings-panel-store-tab-0'] [dir='auto']")]
        private IList<IWebElement> appNamesList;

        [FindsBy(How = How.CssSelector, Using = "div[id$='desktop-settings-panel-store-tab-1'] [dir='auto']")]
        private IList<IWebElement> widgetNamesList;

        [FindsBy(How = How.CssSelector, Using = "[id='desktop-settings-panel-store']")]
        private IWebElement storePanel;

        #endregion //IWebElements

        #region Constructor

        public StorePanel(IWebDriver driver, PageBase page)
            : base(driver, page)
        {
            PageFactory.InitElements(this, new RetryingElementLocator(driver, TimeSpan.FromMilliseconds(Settings.LocatorRetryingTimeout), TimeSpan.FromMilliseconds(Settings.LocatorPollingInterval)));
            WaitForPanelLoaded();
        }

        #endregion //Constructor

        #region Basic methods        

        #region Actions

        public void SelectApp(string appName)
        {
            // Hack: Scroll to last item before getting store apps otherwise we can not get list of store app properly
            var lastApp = appNamesList.LastOrDefault();
            lastApp.ScrollToElementByJS();

            var app = appNamesList.FirstOrDefault(a => a.Text.Equals(appName, StringComparison.InvariantCultureIgnoreCase));
            app.ScrollToElementByJS();
            app.ClickEx();       
        }

        public void SelectWidget(string widgetName)
        {
            // Hack: Scroll to last item before getting store apps otherwise we can not get list of store widget properly
            var lastWidget = widgetNamesList.LastOrDefault();
            lastWidget.ScrollToElementByJS();

            var widget = widgetNamesList.FirstOrDefault(a => a.Text.Equals(widgetName, StringComparison.InvariantCultureIgnoreCase));
            widget.ScrollToElementByJS();
            widget.ClickEx();
        }

        public void ClickHideButton()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Click Applications Tab
        /// </summary>
        public void ClickApplicationsTab()
        {
            applicationsTab.ClickEx();
        }

        /// <summary>
        /// Click Widgets Tab
        /// </summary>
        public void ClickWidgetsTab()
        {
            widgetsTab.ClickEx();
        }

        /// <summary>
        /// Click 'Back' button
        /// </summary>
        public void ClickBackButton()
        {
            backButton.ClickEx();
        }

        /// <summary>
        /// Click 'Install' button
        /// </summary>
        public void ClickInstallButton()
        {
            installButton.ClickEx();
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
        /// Get 'ApplicationsTab' label text
        /// </summary>
        /// <returns></returns>
        public string GetApplicationsTabText()
        {
            return applicationsTab.Text;
        }

        /// <summary>
        /// Get 'WidgetsTab' label text
        /// </summary>
        /// <returns></returns>
        public string GetWidgetsTabText()
        {
            return widgetsTab.Text;
        }

        /// <summary>
        /// Get 'AppOrWidgetIcon' input value
        /// </summary>
        /// <returns></returns>
        public string GetAppOrWidgetIconValue()
        {
            return appOrWidgetIcon.IconValue();
        }

        /// <summary>
        /// Get 'AppOrWidgetName' label text
        /// </summary>
        /// <returns></returns>
        public string GetAppOrWidgetNameText()
        {
            return appOrWidgetNameLabel.Text;
        }

        /// <summary>
        /// Get 'AppOrWidgetVerification' label text
        /// </summary>
        /// <returns></returns>
        public string GetAppOrWidgetVerificationText()
        {
            return appOrWidgetVerificationLabel.Text;
        }

        /// <summary>
        /// Get 'AppOrWidgetSize' label text
        /// </summary>
        /// <returns></returns>
        public string GetAppOrWidgetSizeText()
        {
            return appOrWidgetSizeLabel.Text;
        }

        /// <summary>
        /// Get 'AppOrWidgetEditor' label text
        /// </summary>
        /// <returns></returns>
        public string GetAppOrWidgetEditorText()
        {
            return appOrWidgetEditorLabel.Text;
        }

        /// <summary>
        /// Get 'AppOrWidgetLastUpdate' label text
        /// </summary>
        /// <returns></returns>
        public string GetAppOrWidgetLastUpdateText()
        {
            return appOrWidgetLastUpdateLabel.Text;
        }

        /// <summary>
        /// Get 'AppOrWidgetVersion' label text
        /// </summary>
        /// <returns></returns>
        public string GetAppOrWidgetVersionText()
        {
            return appOrWidgetVersionLabel.Text;
        }

        #endregion //Get methods

        #endregion //Basic methods

        #region Business methods

        /// <summary>
        /// Install a specific app
        /// </summary>
        /// <param name="name"></param>
        public void InstallApp(string name)
        {
            SelectApp(name);
            Wait.ForElementDisplayed(installButton);
            ClickInstallButton();
        }

        /// <summary>
        /// Install a specific widget
        /// </summary>
        /// <param name="name"></param>
        public void InstallWidget(string name)
        {
            SelectWidget(name);
            Wait.ForElementDisplayed(installButton);
            ClickInstallButton();
        }

        /// <summary>
        /// Get all disabled apps name
        /// </summary>
        /// <returns></returns>
        public List<string> GetDisabledAppsName()
        {
            return JSUtility.GetElementsText("div[id$='desktop-settings-panel-store-tab-0'] .desktop-store-item[style*='opacity: 0.5'] [dir]");
        }

        /// <summary>
        /// Get all apps name
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllAppsName()
        {
            return JSUtility.GetElementsText("div[id$='desktop-settings-panel-store-tab-0'] .desktop-store-item [dir]");
        }

        public void WaitForLastAppDisplayed()
        {
            Wait.ForElementText(appNamesList.LastOrDefault());
        }

        public void WaitForLastWidgetDisplayed()
        {
            Wait.ForElementText(widgetNamesList.LastOrDefault());
        }

        #endregion //Business methods

        public override void WaitForPanelLoaded()
        {
            Wait.ForElementStyle(backButton, "display: block");
            
        }
    }
}
