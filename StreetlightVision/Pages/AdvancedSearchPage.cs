using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using StreetlightVision.Extensions;
using StreetlightVision.Pages.UI;
using StreetlightVision.Utilities;
using System;
using System.Text.RegularExpressions;
using System.Threading;

namespace StreetlightVision.Pages
{
    public class AdvancedSearchPage : PageBase
    {
        #region Variables

        private SearchWizardPopupPanel _searchWizardPopupPanel;
        private GridPanel _gridPanel;
        private SwitcherOverlayPanel _switcherOverlayPanel;

        #endregion //Variables

        #region IWebElements

        #endregion //IWebElements

        #region Constructor

        public AdvancedSearchPage(IWebDriver driver)
            : base(driver)
        {
            PageFactory.InitElements(this, new RetryingElementLocator(driver, TimeSpan.FromMilliseconds(Settings.LocatorRetryingTimeout), TimeSpan.FromMilliseconds(Settings.LocatorPollingInterval)));

            WaitForPageReady();
        }

        #endregion //Constructor

        #region Properties

        public SearchWizardPopupPanel SearchWizardPopupPanel
        {
            get
            {
                if (_searchWizardPopupPanel == null)
                {
                    _searchWizardPopupPanel = new SearchWizardPopupPanel(this.Driver, this);
                }

                return _searchWizardPopupPanel;
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

        public SwitcherOverlayPanel SwitcherOverlayPanel
        {
            get
            {
                if (_switcherOverlayPanel == null)
                {
                    _switcherOverlayPanel = new SwitcherOverlayPanel(this.Driver, this);
                }

                return _switcherOverlayPanel;
            }
        }        

        #endregion //Properties

        #region Basic methods

        #region Actions

        #endregion //Actions

        #region Get methods

        public string GetHttp404TitleText()
        {
            var header = Driver.FindElement(By.CssSelector("h1"));
            return header.Text;
        }

        public string GetHttp404DescriptionText()
        {
            var description = Driver.FindElement(By.CssSelector("p:nth-child(5) > u"));
            return description.Text;
        }

        #endregion //Get methods

        #endregion //Basic methods

        #region Business methods

        /// <summary>
        /// Wait for SearchWizardPopupPanel displayed
        /// </summary>
        public void WaitForSearchWizardPopupPanelDisplayed()
        {
            Wait.ForElementDisplayed(By.CssSelector("[id='w2ui-popup']"));
            Wait.ForMilliseconds(500);
        }

        /// <summary>
        /// Wait for SearchWizardPopupPanel hidden
        /// </summary>
        public void WaitForSearchWizardPopupPanelDisappeared()
        {
            Wait.ForElementInvisible(By.CssSelector("[id='w2ui-popup']"));
            Wait.ForMilliseconds(500);
        }        

        /// <summary>
        /// Checking if Http 404 if is existing
        /// </summary>
        /// <returns></returns>
        public bool IsHttp404Existing()
        {
            Wait.ForSeconds(1);
            if (ElementUtility.IsDisplayed(By.CssSelector("h1")) && ElementUtility.IsDisplayed(By.CssSelector("p:nth-child(5) > u")))
            {
                var header = Driver.FindElement(By.CssSelector("h1"));
                if (header.Text.Equals("HTTP Status 404 -"))
                {
                    var description = Driver.FindElement(By.CssSelector("p:nth-child(5) > u"));
                    if (description.Text.Equals("The requested resource is not available."))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public void WaitForSwitcherOverlayPanelDisplayed()
        {
            Wait.ForElementDisplayed(By.CssSelector("div.slv-switcher"));
        }

        public void WaitForSwitcherOverlayPanelDisappeared()
        {
            Wait.ForElementInvisible(By.CssSelector("div.slv-switcher"));
        }

        /// <summary>
        /// Create a new search with name
        /// </summary>
        /// <param name="name"></param>
        /// <param name="geozone"></param>
        /// <param name="hasSearchDialogShown"></param>
        public void CreateNewSearch( string name, string geozone, bool hasSearchDialogShown = false)
        {
            if (!hasSearchDialogShown)
            {
                GridPanel.ClickEditButton();
                WaitForSearchWizardPopupPanelDisplayed();
            }
            SearchWizardPopupPanel.ClickNewAdvancedSearchButton();
            SearchWizardPopupPanel.WaitForNewSearchNameInputVisible();
            SearchWizardPopupPanel.EnterNewSearchNameInput(name);
            SearchWizardPopupPanel.ClickNextButton();
            SearchWizardPopupPanel.WaitForGeozoneFormDisplayed();
            SearchWizardPopupPanel.GeozoneTreePopupPanel.SelectNode(geozone);
            SearchWizardPopupPanel.ClickNextButton();
            SearchWizardPopupPanel.WaitForAttributeFormDisplayed();
            SearchWizardPopupPanel.ClickNextButton();
            SearchWizardPopupPanel.WaitForFilterFormDisplayed();
            SearchWizardPopupPanel.ClickNextButton();
            SearchWizardPopupPanel.WaitForFinishFormDisplayed();
            SearchWizardPopupPanel.ClickFinishButton();
            WaitForSearchWizardPopupPanelDisappeared();
            WaitForPreviousActionComplete();
            GridPanel.WaitForGridContentAvailable();
            GridPanel.SelectSelectOrAddSearchDropDown("---");
        }

        /// <summary>
        ///  Create a new search with name and a specific filter
        /// </summary>
        /// <param name="name"></param>
        /// <param name="geozone"></param>
        /// <param name="filterName"></param>
        /// <param name="filterOperator"></param>
        /// <param name="filterValue"></param>
        /// <param name="hasSearchDialogShown"></param>
        public void CreateNewSearch(string name, string geozone, string filterName, string filterOperator, string filterValue, bool hasSearchDialogShown = false)
        {
            if (!hasSearchDialogShown)
            {
                GridPanel.ClickEditButton();
                WaitForSearchWizardPopupPanelDisplayed();
            }
            SearchWizardPopupPanel.ClickNewAdvancedSearchButton();
            SearchWizardPopupPanel.WaitForNewSearchNameInputVisible();
            SearchWizardPopupPanel.EnterNewSearchNameInput(name);
            SearchWizardPopupPanel.ClickNextButton();
            SearchWizardPopupPanel.WaitForGeozoneFormDisplayed();
            SearchWizardPopupPanel.GeozoneTreePopupPanel.SelectNode(geozone);
            SearchWizardPopupPanel.ClickNextButton();
            SearchWizardPopupPanel.WaitForAttributeFormDisplayed();
            SearchWizardPopupPanel.ClickNextButton();
            SearchWizardPopupPanel.WaitForFilterFormDisplayed();
            SearchWizardPopupPanel.SelectFirstFilterNameDropDown(filterName);
            SearchWizardPopupPanel.SelectFirstFilterOperatorDropDown(filterOperator);
            SearchWizardPopupPanel.EnterOrSelectFirstFilterValue(filterValue);
            SearchWizardPopupPanel.ClickFiltersSaveButton();
            SearchWizardPopupPanel.ClickNextButton();
            SearchWizardPopupPanel.WaitForFinishFormDisplayed();
            SearchWizardPopupPanel.ClickFinishButton();
            WaitForSearchWizardPopupPanelDisappeared();
            WaitForPreviousActionComplete();
        }

        /// <summary>
        /// Create a new search with all attributes selected
        /// </summary>
        /// <param name="name"></param>
        /// <param name="geozone"></param>
        /// <returns>search result count</returns>
        public int CreateNewSearchWithAllAttributes(string name, string geozone)
        {
            SearchWizardPopupPanel.ClickNewAdvancedSearchButton();
            SearchWizardPopupPanel.WaitForNewSearchNameInputVisible();
            SearchWizardPopupPanel.EnterNewSearchNameInput(name);
            SearchWizardPopupPanel.ClickNextButton();
            SearchWizardPopupPanel.WaitForGeozoneFormDisplayed();
            SearchWizardPopupPanel.GeozoneTreePopupPanel.SelectNode(geozone);
            SearchWizardPopupPanel.ClickNextButton();
            SearchWizardPopupPanel.WaitForAttributeFormDisplayed();
            SearchWizardPopupPanel.CheckAllAttributeList();
            SearchWizardPopupPanel.ClickNextButton();
            SearchWizardPopupPanel.WaitForFilterFormDisplayed();
            SearchWizardPopupPanel.ClickNextButton();
            SearchWizardPopupPanel.WaitForFinishFormDisplayed();
            SearchWizardPopupPanel.WaitForDeviceSearchCompleted();
            var messageRegex = Regex.Match(string.Format("{0} {1}", SearchWizardPopupPanel.GetCriteriaMessageText(), SearchWizardPopupPanel.GetFinishMessageText()), @"(\d*) devices match your search criteria. Click on Finish to see the results");
            var searchDevicesCount = int.Parse(messageRegex.Groups[1].ToString());
            SearchWizardPopupPanel.ClickFinishButton();
            WaitForSearchWizardPopupPanelDisappeared();
            WaitForPreviousActionComplete();
            GridPanel.WaitForGridContentAvailable();

            return searchDevicesCount;
        }

        #endregion //Business methods

        protected override void WaitForPageReady()
        {
            base.WaitForPageReady();
        }
    }
}
