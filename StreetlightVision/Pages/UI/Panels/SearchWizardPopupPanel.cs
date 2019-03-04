using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using StreetlightVision.Extensions;
using StreetlightVision.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StreetlightVision.Pages.UI
{
    public class SearchWizardPopupPanel : PanelBase
    {
        #region Variables
        
        private const string _attributesCheckboxListCss = "[id='w2ui-popup'] div.slv-customreport-popup-wizard-page-columns-list-item input[type='checkbox']";
        private const string _attributesListCss = "[id='w2ui-popup'] div.slv-customreport-popup-wizard-page-columns-list-item";

        private GeozoneTreePopupPanel _geozoneTreePopupPanel;

        #endregion //Variables

        #region IWebElements       

        [FindsBy(How = How.CssSelector, Using = "[id='w2ui-popup'] div.slv-customreport-wizard-title-label")]
        private IWebElement panelTitle;

        [FindsBy(How = How.CssSelector, Using = "[id='w2ui-popup'] div.icon-settings.slv-customreport-wizard-title-icon")]
        private IWebElement wizardTitleIcon;

        [FindsBy(How = How.CssSelector, Using = "[id='w2ui-popup'] div.slv-customreport-wizard-title-close.icon-cancel")]
        private IWebElement closeButton;

        [FindsBy(How = How.CssSelector, Using = "[id='w2ui-popup'] div.icon-arrow-right.slv-customreport-wizard-title-back")]
        private IWebElement backButton;

        [FindsBy(How = How.CssSelector, Using = "[id='w2ui-popup'] button.slv-customreport-wizard-button-next")]
        private IWebElement nextButton;

        [FindsBy(How = How.CssSelector, Using = "[id='w2ui-popup'] button.slv-customreport-wizard-button-finish")]
        private IWebElement finishButton;

        [FindsBy(How = How.CssSelector, Using = "[id='w2ui-popup']")]
        private IWebElement popupPanel;

        #region Search Form

        [FindsBy(How = How.CssSelector, Using = "[id='w2ui-popup'] div.slv-customreport-popup-wizard-page-main-requests")]
        private IWebElement searchNameDropDown;

        [FindsBy(How = How.CssSelector, Using = "[id='w2ui-popup'] input.slv-customreport-popup-wizard-page-main-new-request")]
        private IWebElement newSearchNameInput;

        [FindsBy(How = How.CssSelector, Using = "[id='w2ui-popup'] button.slv-customreport-popup-wizard-page-main-button-add")]
        private IWebElement newAdvancedSearchButton;

        [FindsBy(How = How.CssSelector, Using = "[id='w2ui-popup'] button.slv-customreport-popup-wizard-page-main-button-select")]
        private IWebElement selectSavedSearchButton;

        #endregion //Search Form

        #region Select Geozone Form

        [FindsBy(How = How.CssSelector, Using = "[id='w2ui-popup'] div.slv-customreport-popup-wizard-page-geozone-label")]
        private IWebElement selectGeozoneCaptionLabel;

        #endregion //Select Geozone Form

        #region Select Attributes Form

        [FindsBy(How = How.CssSelector, Using = "[id='w2ui-popup'] div.slv-customreport-popup-wizard-page-columns-label")]
        private IWebElement selectAttributeCaptionLabel;

        #endregion //Select Attributes Form

        #region Select Filter Form

        [FindsBy(How = How.CssSelector, Using = "[id='w2ui-popup'] div.slv-customreport-popup-wizard-page-filters-label")]
        private IWebElement filterCaptionLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='w2ui-popup'] div.slv-customreport-popup-wizard-page-filters-list-item")]
        private IList<IWebElement> filtersList;

        [FindsBy(How = How.CssSelector, Using = "[id='w2ui-popup'] button.button.slv-customreport-popup-wizard-page-filters-list-item-button.icon-save")]
        private IWebElement filtersSaveButton;

        [FindsBy(How = How.CssSelector, Using = "[id='w2ui-popup'] button.slv-customreport-popup-wizard-page-filters-list-item-button.icon-save")]
        private IList<IWebElement> filtersSaveButtonList;

        [FindsBy(How = How.CssSelector, Using = "[id='w2ui-popup'] button.slv-customreport-popup-wizard-page-filters-list-item-button.icon-delete")]
        private IList<IWebElement> filtersDeleteButtonList;

        #endregion //Select Filter Form

        #region Finish Form

        [FindsBy(How = How.CssSelector, Using = "[id='w2ui-popup'] div.slv-customreport-popup-wizard-page-summary-label-1")]
        private IWebElement criteriaMessageLabel;

        [FindsBy(How = How.CssSelector, Using = "[id='w2ui-popup'] div.slv-customreport-popup-wizard-page-summary-label-2")]
        private IWebElement finishMessageLabel;

        #endregion //Finish Form

        #endregion //IWebElements

        #region Constructor

        public SearchWizardPopupPanel(IWebDriver driver, PageBase page)
            : base(driver, page)
        {
            PageFactory.InitElements(this, new RetryingElementLocator(driver, TimeSpan.FromMilliseconds(Settings.LocatorRetryingTimeout), TimeSpan.FromMilliseconds(Settings.LocatorPollingInterval)));

            WaitForPanelLoaded();
        }

        #endregion //Constructor

        #region Properties

        public GeozoneTreePopupPanel GeozoneTreePopupPanel
        {
            get
            {
                if (_geozoneTreePopupPanel == null)
                {
                    _geozoneTreePopupPanel = new GeozoneTreePopupPanel(this.Driver, this.Page);
                }

                return _geozoneTreePopupPanel;
            }
        }

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
        /// Click 'Back' button
        /// </summary>
        public void ClickBackButton()
        {
            backButton.ClickEx();
        }

        /// <summary>
        /// Click 'Next' button
        /// </summary>
        public void ClickNextButton()
        {
            nextButton.ClickEx();
        }

        /// <summary>
        /// Click 'Finish' button
        /// </summary>
        public void ClickFinishButton()
        {
            finishButton.ClickEx();
        }

        #region Search Form

        /// <summary>
        /// Select an item of 'SearchName' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectSearchNameDropDown(string value)
        {
            searchNameDropDown.Select(value);
        }

        /// <summary>
        /// Enter a value for 'NewSearhName' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterNewSearchNameInput(string value)
        {
            newSearchNameInput.Enter(value);
        }

        /// <summary>
        /// Click 'NewAdvancedSearch' button
        /// </summary>
        public void ClickNewAdvancedSearchButton()
        {
            newAdvancedSearchButton.ClickEx();
        }

        /// <summary>
        /// Click 'SelectSearch' button
        /// </summary>
        public void ClickSelectSavedSearchButton()
        {
            selectSavedSearchButton.ClickEx();
        }

        #endregion //Search Form

        #region Select Geozone Form

        #endregion //Select Geozone Form

        #region Select Attributes Form

        #endregion //Select Attributes Form

        #region Select Fitler Form

        /// <summary>
        /// Click 'FiltersSave' button
        /// </summary>
        public void ClickFiltersSaveButton()
        {
            filtersSaveButton.ClickEx();
        }

        #endregion //Select Fitler Form

        #region Finish Form

        #endregion //Finish Form

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
        /// Get 'WizardTitleIcon' input value
        /// </summary>
        /// <returns></returns>
        public string GetWizardTitleIconValue()
        {
            return wizardTitleIcon.IconValue();
        }

        #region Search Form

        /// <summary>
        /// Get 'SearchName' input value
        /// </summary>
        /// <returns></returns>
        public string GetSearchNameValue()
        {
            return searchNameDropDown.Text;
        }

        /// <summary>
        /// Get 'SavedSearh' placeholder value
        /// </summary>
        /// <returns></returns>
        public string GetSavedSearchPlaceHolderValue()
        {
            return searchNameDropDown.FindElement(By.CssSelector("a > span.select2-chosen")).Text;
        }

        /// <summary>
        /// Get 'NewSearhName' input value
        /// </summary>
        /// <returns></returns>
        public string GetNewSearchNameValue()
        {
            return newSearchNameInput.Value();
        }

        /// <summary>
        /// Get 'AddNewSearch' button text
        /// </summary>
        public string GetAddNewSearchButtonText()
        {
            return newAdvancedSearchButton.Text;
        }

        /// <summary>
        /// Click 'SelectSearch' button text
        /// </summary>
        public string GetSelectSearchButtonText()
        {
            return selectSavedSearchButton.Text;
        }

        /// <summary>
        /// Get all items of 'SavedSearch' dropdown 
        /// </summary>
        /// <returns></returns>
        public List<string> GetSavedSearchDropDownItems()
        {
            return searchNameDropDown.GetAllItems();
        }

        #endregion //Search Form

        #region Select Geozone Form

        /// <summary>
        /// Get 'SelectGeozoneCaption' label text
        /// </summary>
        /// <returns></returns>
        public string GetSelectGeozoneCaptionText()
        {
            return selectGeozoneCaptionLabel.Text;
        }

        #endregion //Select Geozone Form

        #region Select Attributes Form

        /// <summary>
        /// Get 'SelectAttributeCaption' label text
        /// </summary>
        /// <returns></returns>
        public string GetSelectAttributeCaptionText()
        {
            return selectAttributeCaptionLabel.Text;
        }

        #endregion //Select Attributes Form

        #region Select Fitler Form

        /// <summary>
        /// Get 'FilterCaption' label text
        /// </summary>
        /// <returns></returns>
        public string GetFilterCaptionText()
        {
            return filterCaptionLabel.Text;
        }

        #endregion //Select Fitler Form

        #region Finish Form

        /// <summary>
        /// Get 'CriteriaMessage' label text
        /// </summary>
        /// <returns></returns>
        public string GetCriteriaMessageText()
        {
            return criteriaMessageLabel.Text;
        }

        /// <summary>
        /// Get 'FinishMessage' label text
        /// </summary>
        /// <returns></returns>
        public string GetFinishMessageText()
        {
            return finishMessageLabel.Text;
        }

        #endregion //Finish Form

        #endregion //Get methods

        #endregion //Basic methods

        #region Business methods     

        #region Wait methods

        /// <summary>
        /// Wait for newSearchNameInput displayed
        /// </summary>
        public void WaitForNewSearchNameInputVisible()
        {
            Wait.ForElementDisplayed(newSearchNameInput);
        }

        /// <summary>
        /// Wait for searchNameDropDown displayed
        /// </summary>
        public void WaitForSavedSearhNameDropDownVisible()
        {
            Wait.ForElementDisplayed(searchNameDropDown);
        }

        /// <summary>
        /// Wait for device search action completely
        /// </summary>
        public void WaitForDeviceSearchCompleted()
        {
            Wait.ForElementStyle(By.CssSelector("[id^='slv-customreport-wizard-content-loader'].loader"), "display: none");
        }

        public void WaitForMainSearchFormDisplayed()
        {
            Wait.ForElementStyle(By.CssSelector("[id='w2ui-popup'] div.slv-customreport-wizard-content-pages > div:nth-child(1)"), "left: 0px");
        }

        public void WaitForGeozoneFormDisplayed()
        {
            Wait.ForElementStyle(By.CssSelector("[id='w2ui-popup'] div.slv-customreport-wizard-content-pages > div:nth-child(2)"), "left: 0px");
        }

        public void WaitForAttributeFormDisplayed()
        {
            Wait.ForElementStyle(By.CssSelector("[id='w2ui-popup'] div.slv-customreport-wizard-content-pages > div:nth-child(3)"), "left: 0px");
        }

        public void WaitForFilterFormDisplayed()
        {
            Wait.ForElementStyle(By.CssSelector("[id='w2ui-popup'] div.slv-customreport-wizard-content-pages > div:nth-child(4)"), "left: 0px");
        }

        public void WaitForFinishFormDisplayed()
        {
            Wait.ForElementStyle(By.CssSelector("[id='w2ui-popup'] div.slv-customreport-wizard-content-pages > div:nth-child(5)"), "left: 0px");
        }

        #endregion //Wait methods   

        /// <summary>
        /// Check all attributes in column list
        /// </summary>
        public void CheckAllAttributeList()
        {
            var checkboxes = Driver.FindElements(By.CssSelector(_attributesCheckboxListCss));
            foreach (var checkbox in checkboxes)
            {
                if (!checkbox.Selected)
                {
                    checkbox.ClickByJS();
                }
            }
        }

        /// <summary>
        /// Uncheck all attributes in column list
        /// </summary>
        public void UnCheckAllAttributeList()
        {
            var checkboxes = Driver.FindElements(By.CssSelector(_attributesCheckboxListCss));
            foreach (var checkbox in checkboxes)
            {
                if (checkbox.Selected)
                {
                    checkbox.ClickByJS();
                }
            }
        }

        /// <summary>
        /// Check if all attributes in column list selected
        /// </summary>
        public bool AreAllAttributeListChecked()
        {
            var checkboxes = Driver.FindElements(By.CssSelector(_attributesCheckboxListCss));
            foreach (var checkbox in checkboxes)
            {
                if (checkbox.Selected == false)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Check if all attributes in column list unselected
        /// </summary>
        public bool AreAllAttributeListUnchecked()
        {
            var checkboxes = Driver.FindElements(By.CssSelector(_attributesCheckboxListCss));
            foreach (var checkbox in checkboxes)
            {
                if (checkbox.Selected == true)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Check attributes in column list based on expected list
        /// </summary>
        public void CheckAttributeList(params string[] list)
        {
            var checkboxList = Driver.FindElements(By.CssSelector(_attributesListCss));
            foreach (var checkbox in checkboxList)
            {
                if (list.Any(a => string.Equals(a, checkbox.Text.Trim(), StringComparison.InvariantCultureIgnoreCase)))
                {
                    checkbox.ScrollToElementByJS();
                    checkbox.Check(true);
                }
            }
        }

        /// <summary>
        /// Uncheck attributes in column list based on expected list
        /// </summary>
        public void UncheckAttributeList(params string[] list)
        {
            var checkboxList = Driver.FindElements(By.CssSelector(_attributesListCss));
            foreach (var checkbox in checkboxList)
            {
                if (list.Any(a => string.Equals(a, checkbox.Text.Trim(), StringComparison.InvariantCultureIgnoreCase)))
                {
                    checkbox.ScrollToElementByJS();
                    checkbox.Check(false);
                }
            }
        }

        /// <summary>
        /// Get all attribute list
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllAttributeList()
        {
            return JSUtility.GetElementsText(_attributesListCss);
        }

        /// <summary>
        /// Get all attribute list
        /// </summary>
        /// <returns></returns>
        public List<string> GetCheckedAttributeList()
        {
            var result = new List<string>();
            var checkboxList = Driver.FindElements(By.CssSelector(_attributesListCss));
            foreach (var checkbox in checkboxList)
            {
                if (checkbox.CheckboxValue())
                    result.Add(checkbox.Text.Trim());
            }

            return result;
        }

        /// <summary>
        /// Get all attribute list
        /// </summary>
        /// <returns></returns>
        public List<string> GetUncheckedAttributeList()
        {
            var result = new List<string>();
            var checkboxList = Driver.FindElements(By.CssSelector(_attributesListCss));
            foreach (var checkbox in checkboxList)
            {
                if (checkbox.CheckboxValue() == false)
                    result.Add(checkbox.Text.Trim());
            }

            return result;
        }

        /// <summary>
        /// Get total number of filter options
        /// </summary>
        public int GetFiltersCount()
        {
            return filtersList.Count;
        }

        /// <summary>
        /// Get total number of filter's "Save" button
        /// </summary>
        /// <returns></returns>
        public int GetFilterSaveButtonsCount()
        {
            return filtersSaveButtonList.Count;
        }

        /// <summary>
        /// Check if 'Next' button is visible
        /// </summary>
        /// <returns></returns>
        public bool IsNextButtonVisible()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id='w2ui-popup'] button.slv-customreport-wizard-button-next"));
        }

        /// <summary>
        /// Check if 'Finish' button is visible
        /// </summary>
        /// <returns></returns>
        public bool IsFinishButtonVisible()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id='w2ui-popup'] button.slv-customreport-wizard-button-finish"));
        }

        /// <summary>
        /// Check if 'Back' button is visible
        /// </summary>
        /// <returns></returns>
        public bool IsBackButtonVisible()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id='w2ui-popup'] div.icon-arrow-right.slv-customreport-wizard-title-back"));
        }

        /// <summary>
        /// Check if panel is visible
        /// </summary>
        /// <returns></returns>
        public bool IsPanelVisible()
        {
            return Driver.FindElements(By.CssSelector("[id='w2ui-popup']")).Count > 0;
        }

        /// <summary>
        /// Check if attribute list menu is visible
        /// </summary>
        /// <returns></returns>
        public bool IsAttributeListMenuVisible()
        {
            return Driver.FindElements(By.CssSelector("[id='w2ui-popup'] div.slv-customreport-popup-wizard-page-columns-label")).Count > 0 
                && Driver.FindElements(By.CssSelector(_attributesListCss)).Count > 0;
        }

        /// <summary>
        /// Check if attribute list menu is visible
        /// </summary>
        /// <returns></returns>
        public bool IsFilterMenuVisible()
        {
            return Driver.FindElements(By.CssSelector("[id='w2ui-popup'] div.slv-customreport-popup-wizard-page-filters-label")).Count > 0 
                && filtersList.Count > 0;
        }

        /// <summary>
        /// Select filter name for first filter of list
        /// </summary>
        /// <param name="value"></param>
        public void SelectFirstFilterNameDropDown(string value)
        {
            var firstFilter = filtersList.FirstOrDefault();
            var dropdown = firstFilter.FindElement(By.CssSelector(".slv-customreport-popup-wizard-page-filters-list-item-meaning-selector"));
            dropdown.Select(value);
        }

        /// <summary>
        /// Select filter operator for first filter of list
        /// </summary>
        /// <param name="value"></param>
        public void SelectFirstFilterOperatorDropDown(string value)
        {
            var firstFilter = filtersList.FirstOrDefault();
            var dropdown = firstFilter.FindElement(By.CssSelector(".slv-customreport-popup-wizard-page-filters-list-item-operator-selector"));
            dropdown.Select(value, true);
        }

        /// <summary>
        /// Enter or Select filter value for first filter of list
        /// </summary>
        /// <param name="value"></param>
        public void EnterOrSelectFirstFilterValue(string value)
        {
            var firstFilter = filtersList.FirstOrDefault();
            var inputCss = By.CssSelector(".slv-customreport-popup-wizard-page-filters-list-item-value-selector > input");
            var dropdownCss = By.CssSelector(".slv-customreport-popup-wizard-page-filters-list-item-value-selector > .select2-container");
            if (firstFilter.FindElements(inputCss).Any())
            {
                var input = firstFilter.FindElement(inputCss);
                input.Enter(value);
            }
            else if (firstFilter.FindElements(dropdownCss).Any())
            {
                var dropdown = firstFilter.FindElement(dropdownCss);
                dropdown.Select(value, true);
            }

        }

        public void SelectFirstFilterMultipleValueDropDown(string value)
        {
            var firstFilter = filtersList.FirstOrDefault();
            var inputCss = By.CssSelector(".slv-customreport-popup-wizard-page-filters-list-item-value-selector > input");
            var dropdownCss = By.CssSelector(".slv-customreport-popup-wizard-page-filters-list-item-value-selector > .select2-container");
            var dropdown = firstFilter.FindElement(dropdownCss);
            dropdown.SelectMultiple(value);
        }

        public List<string> GetListOfFilterItemsName()
        {
            var firstFilter = filtersList.FirstOrDefault();
            var dropdown = firstFilter.FindElement(By.CssSelector(".slv-customreport-popup-wizard-page-filters-list-item-meaning-selector"));
            return dropdown.GetAllItems();
        }

        public bool IsSearchNameExisting(string value)
        {
            return searchNameDropDown.CheckIfItemExists(value);
        }

        /// <summary>
        /// Check if New advanced search button is displayed
        /// </summary>
        /// <returns></returns>
        public bool IsNewAdvancedSearchButtonDisplayed()
        {
            return newAdvancedSearchButton.Displayed;
        }

        public bool IsSelectSavedSearchButtonDisplayed()
        {
            return selectSavedSearchButton.Displayed;
        }

        public bool IsFirstFilterRowHasTrashIcon()
        {
            var firstFilter = filtersList.FirstOrDefault();
            return firstFilter.FindElements(By.CssSelector("button.icon-delete")).Any();
        }

        public bool IsLastFilterRowHasSaveIcon()
        {
            var lastFilter = filtersList.LastOrDefault();
            return lastFilter.FindElements(By.CssSelector("button.icon-save")).Any();
        }

        #endregion //Business methods

        public override void WaitForPanelLoaded()
        {
            Wait.ForElementStyle(popupPanel, "opacity: 1");
        }
    }
}