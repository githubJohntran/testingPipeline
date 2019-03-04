using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using StreetlightVision.Pages.UI;
using System;
using StreetlightVision.Extensions;
using StreetlightVision.Utilities;

namespace StreetlightVision.Pages
{
    public class ReportManagerPage : PageBase
    {
        #region Variables

        private GeozoneTreeMainPanel _geozoneTreeMainPanel;
        private GridPanel _gridPanel;
        private ReportEditorPanel _reportEditorPanel;

        #endregion //Variables

        #region Constructor

        public ReportManagerPage(IWebDriver driver)
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

        public GridPanel GridPanel
        {
            get
            {
                if (_gridPanel == null)
                {
                    _gridPanel = new GridPanel(this.Driver, this);
                }

                return _gridPanel;
            }
        }

        public ReportEditorPanel ReportEditorPanel
        {
            get
            {
                if (_reportEditorPanel == null)
                {
                    _reportEditorPanel = new ReportEditorPanel(this.Driver, this);
                }

                return _reportEditorPanel;
            }
        }

        #endregion //Properties

        #region Business methods

        public void WaitForReportDetailsDisplayed()
        {
            Wait.ForElementDisplayed(By.CssSelector("[id$='geozone_panel_right']"));
            Wait.ForElementStyle(By.CssSelector("[id$='geozone_panel_right']"), "opacity: 1");
        }

        public void WaitForReportDetailsDisappeared()
        {
            Wait.ForElementInvisible(By.CssSelector("[id$='geozone_panel_right']"));
            Wait.ForElementStyle(By.CssSelector("[id$='geozone_panel_right']"), "opacity: 0");
        }

        public void WaitForGridPanelDisplayed()
        {
            Wait.ForElementStyle(By.CssSelector("[id$='panels_panel_main']"), "left: 380px");
        }

        public void WaitForAdvancedSearchPanelDisplayed()
        {
            Wait.ForElementDisplayed(By.CssSelector("[id$='reportmanager-geozone-grid']"));
        }

        public void WaitForAdvancedSearchPanelDisappeared()
        {
            Wait.ForElementInvisible(By.CssSelector("[id$='reportmanager-geozone-grid']"));
        }

        public bool IsAdvancedSearchPanelDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='reportmanager-geozone-grid']"));
        }

        /// <summary>
        /// Delete a selecting report in grid
        /// </summary>
        /// <param name="reportName"></param>
        /// <param name="confirmed"></param>
        public void DeleteReport(string reportName, bool confirmed = true)
        {
            GridPanel.ClickGridRecord(reportName);
            WaitForPreviousActionComplete();
            ReportEditorPanel.WaitForPanelLoaded();
            ReportEditorPanel.ClickDeleteButton();
            WaitForPopupDialogDisplayed();

            if (confirmed)
            {
                Dialog.ClickYesButton();
                WaitForPreviousActionComplete();
                if(IsPopupDialogDisplayed())
                {
                    var message = Dialog.GetMessageText();
                    if (message.ToUpper().Contains("ERROR"))
                    {
                        Dialog.ClickOkButton();
                    }
                }
            }
            else
            {
                Dialog.ClickNoButton();
            }
            WaitForPopupDialogDisappeared();
        }

        /// <summary>
        /// Delete current report selected
        /// </summary>
        public void DeleteCurrentReport()
        {
            ReportEditorPanel.ClickDeleteButton();
            WaitForPopupDialogDisplayed();
            
            Dialog.ClickYesButton();
            WaitForPreviousActionComplete();
            if (IsPopupDialogDisplayed())
            {
                var message = Dialog.GetMessageText();
                if (message.ToUpper().Contains("ERROR"))
                {
                    Dialog.ClickOkButton();
                }
            }
            WaitForPopupDialogDisappeared();
        }

        #endregion //Business methods

        protected override void WaitForPageReady()
        {
            base.WaitForPageReady();
            GeozoneTreeMainPanel.WaitForPanelLoaded();
            GridPanel.WaitForPanelLoaded();
        }

        public override void WaitForPopupDialogDisplayed()
        {
            Dialog.WaitForPanelLoaded();
            Wait.ForSeconds(3);
        }
    }
}
