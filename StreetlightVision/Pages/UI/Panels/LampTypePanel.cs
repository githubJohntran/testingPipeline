using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using StreetlightVision.Extensions;
using StreetlightVision.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StreetlightVision.Pages.UI
{
    public class LampTypePanel : PanelBase
    {
        #region Variables

        #endregion //Variables

        #region IWebElements

        #region Header menu

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-device-brand-backButton']")]
        private IWebElement backButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-device-brand-title'] .equipment-gl-editor-title-label")]
        private IWebElement panelTitle;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-device-brand-buttons-toolbar_item_import'] > .w2ui-button")]
        private IWebElement importButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-device-brand-buttons-toolbar_item_add'] > .w2ui-button")]
        private IWebElement addButton;

        #endregion //Header menu

        #region Lamp types list menu

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-device-brand-content-layout-list'] .equipment-gl-list-item")]
        private IList<IWebElement> lampTypesList;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-device-brand-content-layout-list'] .equipment-gl-list-item.slv-item-selected")]
        private IWebElement selectedLampType;

        #endregion //Lamp types list menu

        #region Lamp types detail menu

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-device-brand--brand-title']")]
        private IWebElement propertiesLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-device-brand-brand-save-button']")]
        private IWebElement saveButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-name'] .slv-label.editor-label")]
        private IWebElement nameLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-LampType'] .slv-label.editor-label")]
        private IWebElement identifierLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-LampPower'] .slv-label.editor-label")]
        private IWebElement lampWattageLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-WarmUpTime'] .slv-label.editor-label")]
        private IWebElement warmupTimeLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-ILevel1min'] .slv-label.editor-label")]
        private IWebElement iLevel1MinLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-ILevel1max'] .slv-label.editor-label")]
        private IWebElement iLevel1MaxLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-ILevel2min'] .slv-label.editor-label")]
        private IWebElement iLevel2MinLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-ILevel2max'] .slv-label.editor-label")]
        private IWebElement iLevel2MaxLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-Ino'] .slv-label.editor-label")]
        private IWebElement iNoLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-PFmin'] .slv-label.editor-label")]
        private IWebElement pfMinLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-Vno'] .slv-label.editor-label")]
        private IWebElement vNoLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-Vmin'] .slv-label.editor-label")]
        private IWebElement vMinLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-Vmax'] .slv-label.editor-label")]
        private IWebElement vMaxLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-minOutputValue'] .slv-label.editor-label")]
        private IWebElement minOutputValueLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-ControlVoltMax'] .slv-label.editor-label")]
        private IWebElement controlVoltMaxLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-BHmax'] .slv-label.editor-label")]
        private IWebElement bhMaxLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-Pmax'] .slv-label.editor-label")]
        private IWebElement powerMaxLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-CLOBHLimit'] .slv-label.editor-label")]
        private IWebElement cloHoursIncrementLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-CLOValue'] .slv-label.editor-label")]
        private IWebElement cloInitialValueLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-Interface'] .slv-label.editor-label")]
        private IWebElement interfaceLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-name-field']")]
        private IWebElement nameInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-LampType-field']")]
        private IWebElement identifierInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-LampPower-field']")]
        private IWebElement lampWattageNumericInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-WarmUpTime-field']")]
        private IWebElement warmupTimeNumericInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-ILevel1min-field']")]
        private IWebElement iLevel1MinNumericInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-ILevel1max-field']")]
        private IWebElement iLevel1MaxNumericInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-ILevel2min-field']")]
        private IWebElement iLevel2MinNumericInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-ILevel2max-field']")]
        private IWebElement iLevel2MaxNumericInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-Ino-field']")]
        private IWebElement iNoNumericInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-PFmin-field']")]
        private IWebElement pfMinNumericInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-Vno-field']")]
        private IWebElement vNoNumericInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-Vmin-field']")]
        private IWebElement vMinNumericInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-Vmax-field']")]
        private IWebElement vMaxNumericInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-minOutputValue-field']")]
        private IWebElement minOutputValueNumericInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-ControlVoltMax-field']")]
        private IWebElement controlVoltMaxNumericInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-BHmax-field']")]
        private IWebElement bhMaxNumericInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-Pmax-field']")]
        private IWebElement powerMaxNumericInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-CLOBHLimit-field']")]
        private IWebElement cloHoursIncrementNumericInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-CLOValue-field']")]
        private IWebElement cloInitialValueNumericInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-Interface-field']")]
        private IWebElement interfaceDropDown;

        #endregion //Lamp types detail menu

        #endregion //IWebElements

        #region Constructor

        public LampTypePanel(IWebDriver driver, PageBase page)
            : base(driver, page)
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
        /// Click 'Import' button
        /// </summary>
        public void ClickImportButton()
        {
            importButton.ClickEx();
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
        /// Enter a value for 'Identifier' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterIdentifierInput(string value)
        {
            identifierInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'LampWattage' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterLampWattageNumericInput(string value)
        {
            lampWattageNumericInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'WarmupTime' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterWarmupTimeNumericInput(string value)
        {
            warmupTimeNumericInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'ILevel1Min' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterILevel1MinNumericInput(string value)
        {
            iLevel1MinNumericInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'ILevel1Max' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterILevel1MaxNumericInput(string value)
        {
            iLevel1MaxNumericInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'ILevel2Min' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterILevel2MinNumericInput(string value)
        {
            iLevel2MinNumericInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'ILevel2Max' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterILevel2MaxNumericInput(string value)
        {
            iLevel2MaxNumericInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'INo' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterINoNumericInput(string value)
        {
            iNoNumericInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'PfMin' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterPfMinNumericInput(string value)
        {
            pfMinNumericInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'VNo' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterVNoNumericInput(string value)
        {
            vNoNumericInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'VMin' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterVMinNumericInput(string value)
        {
            vMinNumericInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'VMax' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterVMaxNumericInput(string value)
        {
            vMaxNumericInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'MinOutputValue' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterMinOutputValueNumericInput(string value)
        {
            minOutputValueNumericInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'ControlVoltMax' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterControlVoltMaxNumericInput(string value)
        {
            controlVoltMaxNumericInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'BhMax' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterBhMaxNumericInput(string value)
        {
            bhMaxNumericInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'PowerMax' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterPowerMaxNumericInput(string value)
        {
            powerMaxNumericInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'CloHoursIncrement' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterCloHoursIncrementNumericInput(string value)
        {
            cloHoursIncrementNumericInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'CloInitialValue' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterCloInitialValueNumericInput(string value)
        {
            cloInitialValueNumericInput.Enter(value);
        }

        /// <summary>
        /// Select an item of 'Interface' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectInterfaceDropDown(string value)
        {
            interfaceDropDown.Select(value);
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
        /// Get 'Identifier' label text
        /// </summary>
        /// <returns></returns>
        public string GetIdentifierText()
        {
            return identifierLabel.Text;
        }

        /// <summary>
        /// Get 'LampWattage' label text
        /// </summary>
        /// <returns></returns>
        public string GetLampWattageText()
        {
            return lampWattageLabel.Text;
        }

        /// <summary>
        /// Get 'WarmupTime' label text
        /// </summary>
        /// <returns></returns>
        public string GetWarmupTimeText()
        {
            return warmupTimeLabel.Text;
        }

        /// <summary>
        /// Get 'ILevel1Min' label text
        /// </summary>
        /// <returns></returns>
        public string GetILevel1MinText()
        {
            return iLevel1MinLabel.Text;
        }

        /// <summary>
        /// Get 'ILevel1Max' label text
        /// </summary>
        /// <returns></returns>
        public string GetILevel1MaxText()
        {
            return iLevel1MaxLabel.Text;
        }

        /// <summary>
        /// Get 'ILevel2Min' label text
        /// </summary>
        /// <returns></returns>
        public string GetILevel2MinText()
        {
            return iLevel2MinLabel.Text;
        }

        /// <summary>
        /// Get 'ILevel2Max' label text
        /// </summary>
        /// <returns></returns>
        public string GetILevel2MaxText()
        {
            return iLevel2MaxLabel.Text;
        }

        /// <summary>
        /// Get 'INo' label text
        /// </summary>
        /// <returns></returns>
        public string GetINoText()
        {
            return iNoLabel.Text;
        }

        /// <summary>
        /// Get 'PfMin' label text
        /// </summary>
        /// <returns></returns>
        public string GetPfMinText()
        {
            return pfMinLabel.Text;
        }

        /// <summary>
        /// Get 'VNo' label text
        /// </summary>
        /// <returns></returns>
        public string GetVNoText()
        {
            return vNoLabel.Text;
        }

        /// <summary>
        /// Get 'VMin' label text
        /// </summary>
        /// <returns></returns>
        public string GetVMinText()
        {
            return vMinLabel.Text;
        }

        /// <summary>
        /// Get 'VMax' label text
        /// </summary>
        /// <returns></returns>
        public string GetVMaxText()
        {
            return vMaxLabel.Text;
        }

        /// <summary>
        /// Get 'MinOutputValue' label text
        /// </summary>
        /// <returns></returns>
        public string GetMinOutputValueText()
        {
            return minOutputValueLabel.Text;
        }

        /// <summary>
        /// Get 'ControlVoltMax' label text
        /// </summary>
        /// <returns></returns>
        public string GetControlVoltMaxText()
        {
            return controlVoltMaxLabel.Text;
        }

        /// <summary>
        /// Get 'BhMax' label text
        /// </summary>
        /// <returns></returns>
        public string GetBhMaxText()
        {
            return bhMaxLabel.Text;
        }

        /// <summary>
        /// Get 'PowerMax' label text
        /// </summary>
        /// <returns></returns>
        public string GetPowerMaxText()
        {
            return powerMaxLabel.Text;
        }

        /// <summary>
        /// Get 'CloHoursIncrement' label text
        /// </summary>
        /// <returns></returns>
        public string GetCloHoursIncrementText()
        {
            return cloHoursIncrementLabel.Text;
        }

        /// <summary>
        /// Get 'CloInitialValue' label text
        /// </summary>
        /// <returns></returns>
        public string GetCloInitialValueText()
        {
            return cloInitialValueLabel.Text;
        }

        /// <summary>
        /// Get 'Interface' label text
        /// </summary>
        /// <returns></returns>
        public string GetInterfaceText()
        {
            return interfaceLabel.Text;
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
        /// Get 'Identifier' input value
        /// </summary>
        /// <returns></returns>
        public string GetIdentifierValue()
        {
            return identifierInput.Value();
        }

        /// <summary>
        /// Get 'LampWattage' input value
        /// </summary>
        /// <returns></returns>
        public string GetLampWattageValue()
        {
            return lampWattageNumericInput.Value();
        }

        /// <summary>
        /// Get 'WarmupTime' input value
        /// </summary>
        /// <returns></returns>
        public string GetWarmupTimeValue()
        {
            return warmupTimeNumericInput.Value();
        }

        /// <summary>
        /// Get 'ILevel1Min' input value
        /// </summary>
        /// <returns></returns>
        public string GetILevel1MinValue()
        {
            return iLevel1MinNumericInput.Value();
        }

        /// <summary>
        /// Get 'ILevel1Max' input value
        /// </summary>
        /// <returns></returns>
        public string GetILevel1MaxValue()
        {
            return iLevel1MaxNumericInput.Value();
        }

        /// <summary>
        /// Get 'ILevel2Min' input value
        /// </summary>
        /// <returns></returns>
        public string GetILevel2MinValue()
        {
            return iLevel2MinNumericInput.Value();
        }

        /// <summary>
        /// Get 'ILevel2Max' input value
        /// </summary>
        /// <returns></returns>
        public string GetILevel2MaxValue()
        {
            return iLevel2MaxNumericInput.Value();
        }

        /// <summary>
        /// Get 'INo' input value
        /// </summary>
        /// <returns></returns>
        public string GetINoValue()
        {
            return iNoNumericInput.Value();
        }

        /// <summary>
        /// Get 'PfMin' input value
        /// </summary>
        /// <returns></returns>
        public string GetPfMinValue()
        {
            return pfMinNumericInput.Value();
        }

        /// <summary>
        /// Get 'VNo' input value
        /// </summary>
        /// <returns></returns>
        public string GetVNoValue()
        {
            return vNoNumericInput.Value();
        }

        /// <summary>
        /// Get 'VMin' input value
        /// </summary>
        /// <returns></returns>
        public string GetVMinValue()
        {
            return vMinNumericInput.Value();
        }

        /// <summary>
        /// Get 'VMax' input value
        /// </summary>
        /// <returns></returns>
        public string GetVMaxValue()
        {
            return vMaxNumericInput.Value();
        }

        /// <summary>
        /// Get 'MinOutputValue' input value
        /// </summary>
        /// <returns></returns>
        public string GetMinOutputValueValue()
        {
            return minOutputValueNumericInput.Value();
        }

        /// <summary>
        /// Get 'ControlVoltMax' input value
        /// </summary>
        /// <returns></returns>
        public string GetControlVoltMaxValue()
        {
            return controlVoltMaxNumericInput.Value();
        }

        /// <summary>
        /// Get 'BhMax' input value
        /// </summary>
        /// <returns></returns>
        public string GetBhMaxValue()
        {
            return bhMaxNumericInput.Value();
        }

        /// <summary>
        /// Get 'PowerMax' input value
        /// </summary>
        /// <returns></returns>
        public string GetPowerMaxValue()
        {
            return powerMaxNumericInput.Value();
        }

        /// <summary>
        /// Get 'CloHoursIncrement' input value
        /// </summary>
        /// <returns></returns>
        public string GetCloHoursIncrementValue()
        {
            return cloHoursIncrementNumericInput.Value();
        }

        /// <summary>
        /// Get 'CloInitialValue' input value
        /// </summary>
        /// <returns></returns>
        public string GetCloInitialValueValue()
        {
            return cloInitialValueNumericInput.Value();
        }

        /// <summary>
        /// Get 'Interface' input value
        /// </summary>
        /// <returns></returns>
        public string GetInterfaceValue()
        {
            return interfaceDropDown.Text;
        }

        #endregion //Lamp types detail menu

        #endregion //Get methods

        #endregion //Basic methods

        #region Business methods

        /// <summary>
        /// Get text of selected lamp type 
        /// </summary>
        /// <returns></returns>
        public string GetSelectedLampType()
        {
            return selectedLampType.Text;
        }

        public void ScrollToSelectedLampType()
        {
            selectedLampType.ScrollToElementByJS();
        }

        public void SelectLampType(string lampType)
        {
            var lampTypeElement = lampTypesList.FirstOrDefault(el => el.Text.Contains(lampType));

            lampTypeElement.ClickEx();
        }

        /// <summary>
        /// Get all lamp types in the list
        /// </summary>
        /// <returns></returns>
        public List<string> GetListOfLampTypes()
        {
            //var script = "function f() { let lst = []; let items = document.querySelectorAll(\"[id$='editor-device-brand-content-layout-list'] div.equipment-gl-list-item\"); for(let i = 0; i < items.length; i++) { lst.push(items[i].innerText) }; return lst; }";
            //var result = (IReadOnlyCollection<object>)WebDriverContext.JsExecutor.ExecuteScript(script + "return f();");

            //var lampTypeList = result.Cast<string>().ToList();

            //return lampTypeList;

            return JSUtility.GetElementsText("[id$='editor-device-brand-content-layout-list'] div.equipment-gl-list-item .equipment-gl-list-item-label");
        }

        public List<string> GetListOfInterface()
        {
            return interfaceDropDown.GetAllItems();
        }

        public int CountSelectedLampType()
        {
            return WebDriverContext.CurrentDriver.FindElements(By.CssSelector("[id$='editor-device-brand-content-layout-list'] .equipment-gl-list-item.slv-item-selected")).Count;
        }

        public void ClickRemoveSelectedLampTypeIcon()
        {
            selectedLampType.FindElement(By.CssSelector("div.icon-delete")).ClickEx();
        }

        public void WaitForFieldsReadyForEntering()
        {
            WebDriverContext.Wait.Until(driver => nameInput.Displayed);
        }

        public void WaitForPropertiesSectionDisplayed()
        {
            Wait.ForElementStyle(By.CssSelector("[id$='editor-device-brand--brand-panel']"), "display: block");
        }

        public void WaitForPropertiesSectionDisappeared()
        {
            Wait.ForElementStyle(By.CssSelector("[id$='editor-device-brand--brand-panel']"), "display: none");
        }

        public bool IsImportButtonDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-device-brand-buttons-toolbar_item_import'] > .w2ui-button"));
        }

        public bool IsAddButtonDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-device-brand-buttons-toolbar_item_add'] > .w2ui-button"));
        }

        public bool IsSaveButtonDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-device-brand-brand-save-button']"));
        }

        public bool IsPropertiesSectionDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-device-brand--brand-panel']"));
        }

        public bool IsSelectedLampTypeRemoveButtonDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-device-brand-content-layout-list'] .equipment-gl-list-item.slv-item-selected .icon-delete"));
        }

        public bool HasSelectedLampType()
        {
            return JSUtility.GetElementsText("[id$='editor-device-brand-content-layout-list'] .equipment-gl-list-item.slv-item-selected").Any();
        }

        public bool IsIdentifierInputReadOnly()
        {
            return identifierInput.IsReadOnly();
        }

        public bool IsInterfaceDropDownReadOnly()
        {
            return interfaceDropDown.IsReadOnly();
        }

        public bool AreInputsEditable()
        {
            var inputs = Driver.FindElements(By.CssSelector("[id$='editor-device-brand-content-layout-properties'] input[id^='editor-property'][id$='field']")).ToList();
            inputs.RemoveAll(p => p.GetAttribute("id").Equals("editor-property-LampType-field"));

            if (inputs.Count > 0)
            {
                foreach (var input in inputs)
                {
                    if (input.IsReadOnly())
                        return false;
                }
            }
            return true;
        }

        public void SelectRandomInterfaceDropDown()
        {
            var currentValue = GetInterfaceValue();
            var listItems = interfaceDropDown.GetAllItems();
            listItems.Remove(currentValue);
            interfaceDropDown.Select(listItems.PickRandom());
        }

        public void DeleteSelectedLampType()
        {            
            ClickRemoveSelectedLampTypeIcon();
            Page.WaitForPopupDialogDisplayed();
            Page.Dialog.ClickYesButton();
            Page.WaitForPreviousActionComplete();
        }

        public void CreateNewLampType(string name)
        {
            ClickAddButton();
            WaitForPropertiesSectionDisplayed();
            EnterNameInput(name);
            EnterIdentifierInput(SLVHelper.GenerateString(25));           
            EnterLampWattageNumericInput(SLVHelper.GenerateStringInteger(2000));
            EnterWarmupTimeNumericInput(SLVHelper.GenerateStringInteger(60));
            EnterILevel1MinNumericInput(SLVHelper.GenerateStringInteger(5000));
            EnterILevel1MaxNumericInput(SLVHelper.GenerateStringInteger(10000));
            EnterILevel2MinNumericInput(SLVHelper.GenerateStringInteger(5000));
            EnterILevel2MaxNumericInput(SLVHelper.GenerateStringInteger(10000));
            EnterINoNumericInput(SLVHelper.GenerateStringInteger(5000));
            EnterPfMinNumericInput(SLVHelper.GenerateStringDouble());
            EnterVNoNumericInput(SLVHelper.GenerateStringInteger(500));
            EnterVMinNumericInput(SLVHelper.GenerateStringInteger(500));
            EnterVMaxNumericInput(SLVHelper.GenerateStringInteger(500));
            EnterMinOutputValueNumericInput(SLVHelper.GenerateStringInteger(100));
            EnterControlVoltMaxNumericInput(SLVHelper.GenerateStringInteger(10));
            EnterBhMaxNumericInput(SLVHelper.GenerateStringInteger(200000));
            EnterPowerMaxNumericInput(SLVHelper.GenerateStringInteger(200));
            EnterCloHoursIncrementNumericInput(SLVHelper.GenerateStringInteger(10000));
            EnterCloInitialValueNumericInput(SLVHelper.GenerateStringInteger(100));
            SelectRandomInterfaceDropDown();
            ClickSaveButton();
            WaitForPreviousActionComplete();
        }

        #endregion //Business methods

        public override void WaitForPanelLoaded()
        {
            Wait.ForElementStyle(By.CssSelector("[id$='editor-device-brand']"), "left: 0px");
        }
    }
}
