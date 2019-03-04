using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using StreetlightVision.Extensions;
using StreetlightVision.Pages.UI;
using StreetlightVision.Utilities;
using System;

namespace StreetlightVision.Pages
{
    public class InstallationPage : PageBase
    {
        #region Variables

        private GeozoneTreeMainPanel _geozoneTreeMainPanel;
        private InstallationPopupPanel _installationPopupPanel;
        private Map _map;

        #endregion //Variables

        #region IWebElements

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-desktop-main'] [id$='browser-show-button']")]
        private IWebElement showButton;

        #endregion //IWebElements

        #region Constructor

        public InstallationPage(IWebDriver driver)
            : base(driver)
        {
            PageFactory.InitElements(this, new RetryingElementLocator(driver, TimeSpan.FromMilliseconds(Settings.LocatorRetryingTimeout), TimeSpan.FromMilliseconds(Settings.LocatorPollingInterval)));
            WaitForPageReady();
        }

        #endregion //Constructor

        #region Properties

        public GeozoneTreeMainPanel GeozoneTreeMainPanel
        {
            get
            {
                if (_geozoneTreeMainPanel == null)
                {
                    _geozoneTreeMainPanel = new GeozoneTreeMainPanel(this.Driver, this);
                }

                return _geozoneTreeMainPanel;
            }
        }

        public InstallationPopupPanel InstallationPopupPanel
        {
            get
            {
                if (_installationPopupPanel == null)
                {
                    _installationPopupPanel = new InstallationPopupPanel(this.Driver, this);
                }

                return _installationPopupPanel;
            }
        }

        public Map Map
        {
            get
            {
                if (_map == null)
                {
                    _map = new Map(this.Driver, this);
                }

                return _map;
            }
        }

        #endregion //Properties

        #region Basic methods

        #region Actions

        /// <summary>
        /// Click 'Show' button
        /// </summary>
        public void ClickShowButton()
        {
            showButton.ClickEx();
        }

        #endregion //Actions

        #region Get methods

        #endregion //Get methods

        #endregion //Basic methods

        #region Business methods

        #region Wait methods

        public override void WaitForPopupDialogDisplayed()
        {
            Wait.ForElementDisplayed(By.CssSelector("[id='w2ui-popup']"));
            Wait.ForElementInvisible(By.CssSelector("[id$='wizard-loader-spin']"));
            Wait.ForMilliseconds(500);
        }

        public void WaitForMainGeozoneTreeDisplayed()
        {
            Wait.ForElementStyle(By.CssSelector("[id$='browser']"), "left: 20px");
        }

        public void WaitForMainGeozoneTreeDisappeared()
        {
            Wait.ForElementStyle(By.CssSelector("[id$='browser']"), "left: -330px");
        }

        public void OpenGeozoneTreeIfNotExpand()
        {
            var isShowButtonDisplayed = ElementUtility.IsDisplayed(By.CssSelector("[id='slv-view-desktop-main'] [id$='browser-show-button']"));
            if (isShowButtonDisplayed)
            {
                ClickShowButton();
                WaitForMainGeozoneTreeDisplayed();
            }
        }

        #endregion //Wait methods

        /// <summary>
        /// Add a new streetlight
        /// </summary>
        /// <param name="streetlightName"></param>
        /// <param name="typeOfEquipment"></param>
        /// <param name="controllerId"></param>
        /// <returns>streetlight path</returns>
        public string AddNewStreetlight(out string identifier, string streetlightName, string typeOfEquipment, string controllerId, string macAddess = "")
        {
            Map.ClickAddStreetlightButton();
            Map.WaitForRecorderDisplayed();
            Map.PositionNewDevice();
            Map.WaitForRecorderDisappeared();
            WaitForPreviousActionComplete();
            WaitForPopupDialogDisplayed();
            var geozonePath = GeozoneTreeMainPanel.GetSelectedNodeGeozonePath(false);
            identifier = InstallationPopupPanel.GetIdentifierValue();

            InstallationPopupPanel.SelectTypeOfEquipmentDropDown(typeOfEquipment);
            InstallationPopupPanel.SelectControllerIdDropDown(controllerId);
            InstallationPopupPanel.EnterNameInput(streetlightName);

            InstallationPopupPanel.ClickNextButton();
            InstallationPopupPanel.WaitForScanQRCodeFormDisplayed();
            if (!string.IsNullOrEmpty(macAddess)) InstallationPopupPanel.EnterUniqueAddressInput(macAddess);

            InstallationPopupPanel.ClickNextButton();
            InstallationPopupPanel.WaitForLightComeOnFormDisplayed();

            InstallationPopupPanel.ClickNextButton();
            InstallationPopupPanel.WaitForCommentFormDisplayed();

            InstallationPopupPanel.ClickNextButton();
            InstallationPopupPanel.WaitForFinishFormDisplayed();

            InstallationPopupPanel.ClickFinishButton();
            WaitForPreviousActionComplete();
            WaitForPopupDialogDisappeared();

            return geozonePath + @"\" + streetlightName;
        }

        #endregion //Business methods

        protected override void WaitForPageReady()
        {           
            base.WaitForPageReady();
            OpenGeozoneTreeIfNotExpand();
            GeozoneTreeMainPanel.WaitForPanelLoaded();
            Map.WaitForLoaded();
        }
    }
}
