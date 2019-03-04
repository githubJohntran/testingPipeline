using OpenQA.Selenium;
using StreetlightVision.Extensions;
using SeleniumExtras.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using StreetlightVision.Utilities;

namespace StreetlightVision.Pages.UI
{
    public class EnergySupplierPanel : PanelBase
    {
        #region Variables

        #endregion //Variables

        #region IWebElements

        #region Header menu

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-device-provider-backButton']")]
        private IWebElement backButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-device-provider-title'] .equipment-gl-editor-title-label")]
        private IWebElement panelTitle;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-device-provider-buttons-toolbar_item_add'] table.w2ui-button")]
        private IWebElement addButton;

        #endregion //Header menu

        #region Energy provider list menu

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-device-provider-content-layout-list'] div.equipment-gl-list-item")]
        private IList<IWebElement> energySupplierList;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-device-provider-content-layout-list'] div.equipment-gl-list-item.slv-item-selected")]
        private IWebElement selectedEnergySupplier;

        #endregion //Lamp types list menu

        #region Energy provider detail menu

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-device-provider--provider-title']")]
        private IWebElement propertiesLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-device-provider-provider-save-button']")]
        private IWebElement saveButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-name'] .slv-label.editor-label")]
        private IWebElement nameLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-kgCo2byKwH'] .slv-label.editor-label")]
        private IWebElement kgCo2byKwhLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-name-field']")]
        private IWebElement nameInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-kgCo2byKwH-field']")]
        private IWebElement kgCo2byKwhNumericInput;


        #endregion //Lamp types detail menu

        #endregion //IWebElements

        #region Constructor

        public EnergySupplierPanel(IWebDriver driver, PageBase page) : base(driver, page)
        {
            PageFactory.InitElements(this, new RetryingElementLocator(driver, TimeSpan.FromMilliseconds(Settings.LocatorRetryingTimeout), TimeSpan.FromMilliseconds(Settings.LocatorPollingInterval)));

            WaitForPanelLoaded();
        }

        #endregion //Constructor

        #region Basic methods

        #region Actions

        #region Header menu

        /// <summary>
        /// Click 'Back' button
        /// </summary>
        public void ClickBackButton()
        {
            backButton.ClickEx();
        }

        /// <summary>
        /// Click 'Add' button
        /// </summary>
        public void ClickAddButton()
        {
            addButton.ClickEx();
        }

        #endregion //Header menu

        #region Lamp types list menu

        #endregion //Lamp types list menu

        #region Lamp types detail menu

        /// <summary>
        /// Click 'Save' button
        /// </summary>
        public void ClickSaveButton()
        {
            saveButton.ClickEx();
        }

        /// <summary>
        /// Enter a value for 'Name' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterNameInput(string value)
        {
            nameInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'KgCo2byKwh' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterKgCo2byKwhNumericInput(string value)
        {
            kgCo2byKwhNumericInput.Enter(value);
        }

        #endregion //Lamp types detail menu

        #endregion //Actions

        #region Get methods

        #region Header menu

        /// <summary>
        /// Get 'PanelTitle' text
        /// </summary>
        /// <returns></returns>
        public string GetPanelTitleText()
        {
            return panelTitle.Text;
        }

        #endregion //Header menu

        #region Lamp types list menu

        #endregion //Lamp types list menu

        #region Lamp types detail menu

        /// <summary>
        /// Get 'Properties' label text
        /// </summary>
        /// <returns></returns>
        public string GetPropertiesText()
        {
            return propertiesLabel.Text;
        }

        /// <summary>
        /// Get 'Name' label text
        /// </summary>
        /// <returns></returns>
        public string GetNameText()
        {
            return nameLabel.Text;
        }

        /// <summary>
        /// Get 'KgCo2byKwh' label text
        /// </summary>
        /// <returns></returns>
        public string GetKgCo2byKwhText()
        {
            return kgCo2byKwhLabel.Text;
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
        /// Get 'KgCo2byKwh' input value
        /// </summary>
        /// <returns></returns>
        public string GetKgCo2byKwhValue()
        {
            return kgCo2byKwhNumericInput.Value();
        }

        #endregion //Lamp types detail menu

        #endregion //Get methods

        #endregion //Basic methods

        #region Business methods

        public void WaitForPropertiesSectionDisplayed()
        {
            Wait.ForElementStyle(By.CssSelector("[id$='editor-device-provider--provider-panel']"), "display: block");
        }

        public void WaitForPropertiesSectionDisappeared()
        {
            Wait.ForElementStyle(By.CssSelector("[id$='editor-device-provider--provider-panel']"), "display: none");
        }

        public bool IsNameInputEditable()
        {
            return !nameInput.IsReadOnly();
        }

        public bool IsKgCo2byKwhNumericInputEditable()
        {
            return !kgCo2byKwhNumericInput.IsReadOnly();
        }

        public bool IsAddButtonDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-device-provider-buttons-toolbar_item_add'] > .w2ui-button"));
        }

        public bool IsSaveButtonDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-device-provider-provider-save-button']"));
        }

        public bool IsPropertiesSectionDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-device-provider--provider-panel']"));
        }

        public bool IsSelectedEnergySupplierRemoveButtonDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-device-provider-content-layout-list'] .equipment-gl-list-item.slv-item-selected .icon-delete"));
        }

        public bool HasSelectedEnergySupplier()
        {
            return JSUtility.GetElementsText("[id$='editor-device-provider-content-layout-list'] .equipment-gl-list-item.slv-item-selected").Any();
        }

        public List<string> GetListOfEnergySuppliers()
        {
            return JSUtility.GetElementsText("[id$='editor-device-provider-content-layout-list'] div.equipment-gl-list-item .equipment-gl-list-item-label");
        }

        public string GetSelectedEnergySupplier()
        {
            return selectedEnergySupplier.Text;
        }

        public void ScrollToSelectedEnergySupplier()
        {
            selectedEnergySupplier.ScrollToElementByJS();
        }

        public void ClickRemoveSelectedEnergySupplierIcon()
        {
            selectedEnergySupplier.FindElement(By.CssSelector("div.icon-delete")).ClickEx();
        }

        public void ClickDeleteEnergySupplier(string supplier)
        {
            var supplierElement = energySupplierList.FirstOrDefault(el => el.Text.Contains(supplier));
            supplierElement.ClickEx();

            var removeButton = supplierElement.FindElement(By.CssSelector("div.equipment-gl-item-button"));
            removeButton.ClickEx();
        }

        public void SelectEnergySupplier(string supplier)
        {
            var supplierElement = energySupplierList.FirstOrDefault(el => el.Text.Contains(supplier));

            supplierElement.ClickEx();
        }

        public void DeleteSelectedEnergySupplier()
        {
            ClickRemoveSelectedEnergySupplierIcon();
            Page.WaitForPopupDialogDisplayed();
            Page.Dialog.ClickYesButton();
            Page.WaitForPreviousActionComplete();
        }

        public void CreateNewEnergySupplier(string name)
        {
            ClickAddButton();
            WaitForPropertiesSectionDisplayed();
            EnterNameInput(name);
            EnterKgCo2byKwhNumericInput(SLVHelper.GenerateStringInteger(99999));
            ClickSaveButton();
            WaitForPreviousActionComplete();
        }

        #endregion //Business methods

        public override void WaitForPanelLoaded()
        {
            Wait.ForElementStyle(By.CssSelector("[id$='equipmentgl-editor-device-provider']"), "left: 0px");
        }
    }
}
