using NUnit.Framework;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using StreetlightVision.Extensions;
using StreetlightVision.Models;
using StreetlightVision.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StreetlightVision.Pages.UI
{
    public class DeviceEditorPanel : NodeEditorPanelBase
    {
        #region Variables

        private CommissionPanel _commissionPanel;
        private MacAddressPanel _macAddressPanel;
        private DuplicateEquipmentPanel _duplicateEquipmentPanel;
        private LampTypePanel _lampTypePanel;
        private EnergySupplierPanel _energySupplierPanel;

        #endregion //Variables

        #region IWebElements

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-device-content-properties-tabs'] div.w2ui-tab")]
        private IList<IWebElement> tabList;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-device-content-properties-tabs'] div.w2ui-tab.active")]
        private IWebElement activeTab;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-device'] div.editor-group-header")]
        private IList<IWebElement> editorGroupHeaderList;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-device'] [id$='editor-device-content-properties'] .editor-tab .editor-property")]
        private IList<IWebElement> proptertiesList;

        #region Header toolbar buttons

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-deviceequipmentDeviceButtons_item_cancel'] .w2ui-button")]
        private IWebElement backButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-deviceequipmentDeviceButtons_item_commission'] .w2ui-button")]
        private IWebElement commissionButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-deviceequipmentDeviceButtons_item_replaceLamp'] .w2ui-button")]
        private IWebElement replaceLampButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-deviceequipmentDeviceButtons_item_replaceNode'] .w2ui-button")]
        private IWebElement replaceNodeButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-deviceequipmentDeviceButtons_item_duplicate'] .w2ui-button")]
        private IWebElement duplicateButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-deviceequipmentDeviceButtons_item_save'] .w2ui-button")]
        private IWebElement saveButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-deviceequipmentDeviceButtons_item_delete'] .w2ui-button")]
        private IWebElement deleteButton;

        #endregion //Header toolbar buttons  

        #region Content Identity

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-device-content-category-icon']")]
        private IWebElement deviceIcon;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-device-content-category-label']")]
        private IWebElement deviceCaptionLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-device-content-name-label'] .slv-label.equipment-gl-editor-label")]
        private IWebElement nameLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-device-content-geozone-label'] .slv-label.equipment-gl-editor-label")]
        private IWebElement geozoneLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-device-content-lat-label']")]
        private IWebElement latitudeLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-device-content-lng-label']")]
        private IWebElement longitudeLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-device-content-name-field']")]
        private IWebElement nameInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-device-content-geozone-field']")]
        private IWebElement geozoneInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-device-content-lat-field']")]
        private IWebElement latitudeInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-device-content-lng-field']")]
        private IWebElement longitudeInput;

        #endregion //Content Identity      

        #endregion //IWebElements

        #region Constructor

        public DeviceEditorPanel(IWebDriver driver, PageBase page) : base(driver, page)
        {
            PageFactory.InitElements(this, new RetryingElementLocator(driver, TimeSpan.FromMilliseconds(Settings.LocatorRetryingTimeout), TimeSpan.FromMilliseconds(Settings.LocatorPollingInterval)));

            WaitForPanelLoaded();
        }

        #endregion //Constructor

        #region Properties

        public CommissionPanel CommissionPanel
        {
            get
            {
                if (_commissionPanel == null)
                {
                    _commissionPanel = new CommissionPanel(this.Driver, this.Page);
                }

                return _commissionPanel;
            }
        }

        public MacAddressPanel MacAddressPanel
        {
            get
            {
                if (_macAddressPanel == null)
                {
                    _macAddressPanel = new MacAddressPanel(this.Driver, this.Page);
                }

                return _macAddressPanel;
            }
        }


        public DuplicateEquipmentPanel DuplicateEquipmentPanel
        {
            get
            {
                if (_duplicateEquipmentPanel == null)
                {
                    _duplicateEquipmentPanel = new DuplicateEquipmentPanel(this.Driver, this.Page);
                }

                return _duplicateEquipmentPanel;
            }
        }

        public LampTypePanel LampTypePanel
        {
            get
            {
                if (_lampTypePanel == null)
                {
                    _lampTypePanel = new LampTypePanel(this.Driver, this.Page);
                }

                return _lampTypePanel;
            }
        }

        public EnergySupplierPanel EnergySupplierPanel
        {
            get
            {
                if (_energySupplierPanel == null)
                {
                    _energySupplierPanel = new EnergySupplierPanel(this.Driver, this.Page);
                }

                return _energySupplierPanel;
            }
        }

        #endregion //Properties

        #region Basic methods        

        #region Actions

        #region Header toolbar buttons

        /// <summary>
        /// Click 'Back' button
        /// </summary>
        public void ClickBackButton()
        {
            backButton.ClickEx();
        }

        /// <summary>
        /// Click 'Commission' button
        /// </summary>
        public void ClickCommissionButton()
        {
            commissionButton.ClickEx();
        }

        /// <summary>
        /// Click 'ReplaceLamp' button
        /// </summary>
        public void ClickReplaceLampButton()
        {
            replaceLampButton.ClickEx();
        }

        /// <summary>
        /// Click 'ReplaceNode' button
        /// </summary>
        public void ClickReplaceNodeButton()
        {
            replaceNodeButton.ClickEx();
        }

        /// <summary>
        /// Click 'Duplicate' button
        /// </summary>
        public void ClickDuplicateButton()
        {
            duplicateButton.ClickEx();
        }

        /// <summary>
        /// Click 'Save' button
        /// </summary>
        public void ClickSaveButton()
        {
            saveButton.ClickEx();
        }

        /// <summary>
        /// Click 'Delete' button
        /// </summary>
        public void ClickDeleteButton()
        {
            deleteButton.ClickEx();
        }

        #endregion //Header toolbar buttons

        #region Content Identity

        /// <summary>
        /// Enter a value for 'Name' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterNameInput(string value)
        {
            nameInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'Geozone' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterGeozoneInput(string value)
        {
            geozoneInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'Latitude' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterLatitudeInput(string value)
        {
            latitudeInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'Longitude' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterLongitudeInput(string value)
        {
            longitudeInput.Enter(value);
        }

        #endregion //Content Identity

        #endregion //Actions

        #region Get methods

        #region Header toolbar buttons

        #endregion //Header toolbar buttons

        #region Content Identity

        /// <summary>
        /// Get 'DeviceIcon' input value
        /// </summary>
        /// <returns></returns>
        public string GetDeviceIconValue()
        {
            return deviceIcon.IconValue();
        }

        /// <summary>
        /// Get 'DeviceCaption' label text
        /// </summary>
        /// <returns></returns>
        public string GetDeviceCaptionText()
        {
            return deviceCaptionLabel.Text;
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
        /// Get 'Geozone' label text
        /// </summary>
        /// <returns></returns>
        public string GetGeozoneText()
        {
            return geozoneLabel.Text;
        }

        /// <summary>
        /// Get 'Latitude' label text
        /// </summary>
        /// <returns></returns>
        public string GetLatitudeText()
        {
            return latitudeLabel.Text;
        }

        /// <summary>
        /// Get 'Longitude' label text
        /// </summary>
        /// <returns></returns>
        public string GetLongitudeText()
        {
            return longitudeLabel.Text;
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
        /// Get 'Geozone' input value
        /// </summary>
        /// <returns></returns>
        public string GetGeozoneValue()
        {
            return geozoneInput.Value();
        }

        /// <summary>
        /// Get 'Latitude' input value
        /// </summary>
        /// <returns></returns>
        public string GetLatitudeValue()
        {
            return latitudeInput.Value();
        }

        /// <summary>
        /// Get 'Longitude' input value
        /// </summary>
        /// <returns></returns>
        public string GetLongitudeValue()
        {
            return longitudeInput.Value();
        }

        #endregion //Content Identity

        #endregion //Get methods

        #endregion //Basic methods

        #region Business methods

        public void WaitForCommissionButtonDisplayed()
        {
            Wait.ForElementDisplayed(By.CssSelector("[id$='editor-deviceequipmentDeviceButtons_item_commission'] .w2ui-button"));
        }

        public void WaitForCommissionButtonDisappeared()
        {
            Wait.ForElementInvisible(By.CssSelector("[id$='editor-deviceequipmentDeviceButtons_item_commission'] .w2ui-button"));
        }

        public void WaitForCommissionPanelDisplayed()
        {
            Wait.ForElementDisplayed(By.CssSelector("[id$='editor-device-commission']"));
            Wait.ForElementStyle(By.CssSelector("[id$='editor-device-commission']"), "left: 0px");
        }

        public void WaitForCommissionPanelDisappeared()
        {
            Wait.ForElementInvisible(By.CssSelector("[id$='editor-device-commission']"));
            Wait.ForElementStyle(By.CssSelector("[id$='editor-device-commission']"), "left: 350px");
        }

        public void WaitForMacAddressPanelDisplayed()
        {
            Wait.ForElementDisplayed(By.CssSelector("[id$='editor-device-macaddress']"));
            Wait.ForElementStyle(By.CssSelector("[id$='editor-device-macaddress']"), "left: 0px");
        }

        public void WaitForMacAddressPanelDisappeared()
        {
            Wait.ForElementInvisible(By.CssSelector("[id$='editor-device-macaddress']"));
            Wait.ForElementStyle(By.CssSelector("[id$='editor-device-macaddress']"), "left: 350px");
        }

        public void WaitForDuplicateEquipmentPanelDisplayed()
        {
            Wait.ForElementDisplayed(By.CssSelector("[id$='editor-device-duplicate']"));
            Wait.ForElementStyle(By.CssSelector("[id$='editor-device-duplicate']"), "left: 0px");
        }

        public void WaitForDuplicateEquipmentPanelDisappeared()
        {
            Wait.ForElementInvisible(By.CssSelector("[id$='editor-device-duplicate']"));
            Wait.ForElementStyle(By.CssSelector("[id$='editor-device-duplicate']"), "left: 350px");
        }

        public void WaitForLampTypePanelDisplayed()
        {
            Wait.ForElementDisplayed(By.CssSelector("[id$='editor-device-brand']"));
            Wait.ForElementStyle(By.CssSelector("[id$='editor-device-brand']"), "left: 0px");
        }

        public void WaitForLampTypePanelDisappeared()
        {
            Wait.ForElementInvisible(By.CssSelector("[id$='editor-device-brand']"));
            Wait.ForElementStyle(By.CssSelector("[id$='editor-device-brand']"), "left: 350px");
        }

        public void WaitForEnergySupplierPanelDisplayed()
        {
            Wait.ForElementDisplayed(By.CssSelector("[id$='editor-device-provider']"));
            Wait.ForElementStyle(By.CssSelector("[id$='editor-device-provider']"), "left: 0px");
        }

        public void WaitForEnergySupplierPanelDisappeared()
        {
            Wait.ForElementInvisible(By.CssSelector("[id$='editor-device-provider']"));
            Wait.ForElementStyle(By.CssSelector("[id$='editor-device-provider']"), "left: 350px");
        }

        public void WaitForTitleHasText()
        {
            Wait.ForElementText(By.CssSelector("[id$='editor-device-content-category-label'].slv-title"));
        }

        public bool IsCommissionPanelDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-device-commission']"));
        }

        public bool IsMacAddressPanelDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-device-macaddress']"));
        }

        public bool IsDuplicateEquipmentPanelDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-device-duplicate']"));
        }

        public bool IsLampTypePanelDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-device-brand']"));
        }

        public bool IsEnergySupplierPanelDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-device-provider']"));
        }

        public bool IsBackButtonDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-deviceequipmentDeviceButtons_item_cancel'] .w2ui-button"));
        }

        public bool IsCommissionButtonDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-deviceequipmentDeviceButtons_item_commission'] .w2ui-button"));
        }

        public bool IsReplaceLampButtonDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-deviceequipmentDeviceButtons_item_replaceLamp'] .w2ui-button"));
        }

        public bool IsReplaceNodeButtonDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-deviceequipmentDeviceButtons_item_replaceNode'] .w2ui-button"));
        }

        public bool IsDuplicateButtonDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-deviceequipmentDeviceButtons_item_duplicate'] .w2ui-button"));
        }

        public bool IsSaveButtonDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-deviceequipmentDeviceButtons_item_save'] .w2ui-button"));
        }

        public bool IsDeleteButtonDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-deviceequipmentDeviceButtons_item_delete'] .w2ui-button"));
        }

        public bool IsNameInputReadOnly()
        {
            return nameInput.IsReadOnly();
        }

        public bool IsParentGeozoneInputReadOnly()
        {
            return geozoneInput.IsReadOnly();
        }

        public bool IsLatitudeInputReadOnly()
        {
            return latitudeInput.IsReadOnly();
        }

        public bool IsLongitudeInputReadOnly()
        {
            return longitudeInput.IsReadOnly();
        }

        public bool IsNameInputDisplayed()
        {
            return nameInput.Displayed;
        }

        public bool IsParentGeozoneInputDisplayed()
        {
            return geozoneInput.Displayed;
        }

        public bool IsLatitudeInputDisplayed()
        {
            return latitudeInput.Displayed;
        }

        public bool IsLongitudeInputDisplayed()
        {
            return longitudeInput.Displayed;
        }

        /// <summary>
        /// Check if panel is visible
        /// </summary>
        /// <returns></returns>
        public bool IsPanelVisible()
        {
            return backButton.Displayed;
        }

        public bool AreInputsReadOnly()
        {
            var inputs = Driver.FindElements(By.CssSelector("[id$='equipmentgl-editor-device-content-properties'] input[id^='editor-property'][id$='field']")).ToList();

            if (inputs.Count > 0)
            {
                foreach (var input in inputs)
                {
                    if (!input.IsReadOnly())
                        return false;
                }
            }
            return true;
        }

        public bool AreInputsEditable(NodeType type)
        {
            var inputs = Driver.FindElements(By.CssSelector("[id$='equipmentgl-editor-device-content-properties'] input[id^='editor-property'][id$='field']")).ToList();
            switch (type)
            {
                case NodeType.Controller:
                    inputs.RemoveAll(p => p.GetAttribute("id").Equals("editor-property-controllerStrId-field"));
                    break;
                case NodeType.Streetlight:
                    inputs.RemoveAll(p => p.GetAttribute("id").Equals("editor-property-idOnController-field"));
                    inputs.RemoveAll(p => p.GetAttribute("id").Equals("editor-property-ConfigStatus-field"));
                    inputs.RemoveAll(p => p.GetAttribute("id").Equals("editor-property-ConfigStatusMessage-field"));
                    inputs.RemoveAll(p => p.GetAttribute("id").Equals("editor-property-CommunicationStatus-field"));
                    break;
                case NodeType.Switch:
                    inputs.RemoveAll(p => p.GetAttribute("id").Equals("editor-property-idOnController-field"));
                    break;
                case NodeType.CabinetController:
                    inputs.RemoveAll(p => p.GetAttribute("id").Equals("editor-property-idOnController-field"));
                    break;
            }

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

        public bool AreDropDownsReadOnly()
        {
            var dropdownInputs = Driver.FindElements(By.CssSelector("[id$='equipmentgl-editor-device-content-properties'] div[id^='editor-property'].editor-select input.select2-focusser")).ToList();
            if (dropdownInputs.Count > 0)
            {
                foreach (var input in dropdownInputs)
                {
                    if (!input.IsReadOnly())
                        return false;
                }
            }
            return true;
        }

        public bool AreDropDownsEditable()
        {
            var dropdownInputs = Driver.FindElements(By.CssSelector("[id$='equipmentgl-editor-device-content-properties'] div[id^='editor-property'].editor-select input.select2-focusser")).ToList();
            if (dropdownInputs.Count > 0)
            {
                foreach (var input in dropdownInputs)
                {
                    if (input.IsReadOnly())
                        return false;
                }
            }
            return true;
        }

        public bool AreCheckboxesReadOnly()
        {
            var checkboxInputs = Driver.FindElements(By.CssSelector("[id$='equipmentgl-editor-device-content-properties'] div[id^='editor-property'].checkbox input")).ToList();
            if (checkboxInputs.Count > 0)
            {
                foreach (var input in checkboxInputs)
                {
                    if (!input.IsReadOnly())
                        return false;
                }
            }
            return true;
        }

        public bool AreCheckboxesEditable()
        {
            var checkboxInputs = Driver.FindElements(By.CssSelector("[id$='equipmentgl-editor-device-content-properties'] div[id^='editor-property'].checkbox input")).ToList();
            if (checkboxInputs.Count > 0)
            {
                foreach (var input in checkboxInputs)
                {
                    if (input.IsReadOnly())
                        return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Get all tabs as text
        /// </summary>
        public List<string> GetListOfTabsName()
        {
            return JSUtility.GetElementsText("[id$='editor-device-content-properties-tabs'] div.w2ui-tab");
        }

        /// <summary>
        /// Select a tab by its caption
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public void SelectTab(string name)
        {
            var tab = tabList.FirstOrDefault(p => p.Text.Contains(name));
            if (tab != null)
            {
                tab.ClickEx();
                WebDriverContext.Wait.Until(driver => JSUtility.GetElementText("[id$='editor-device-content-properties-tabs'] div.w2ui-tab.active") == name);
            }
        }

        /// <summary>
        /// Check if a tab is exisiting
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool IsTabExisting(string name)
        {
            return tabList.Any(p => p.Text.Contains(name));
        }

        /// <summary>
        /// Get caption of active tab
        /// </summary>
        /// <returns></returns>
        public string GetActiveTabCaption()
        {
            return activeTab.Text;
        }

        /// <summary>
        /// Get editor groups
        /// </summary>
        public List<string> GetListOfGroupsName()
        {
            return JSUtility.GetElementsText("[id$='editor-device'] .editor-group-header div[dir]");
        }

        public List<string> GetListOfGroupsNameActiveTab()
        {
            return JSUtility.GetElementsText("[id$='editor-device'] [id*='editor-device-content-properties-tab'].editor-tab[style*='display: block'] .editor-group-header div[dir]");
        }

        /// <summary>
        /// Expand an editor group by its caption
        /// </summary>
        /// <param name="groupName"></param>
        public void ExpandGroup(string groupName)
        {
            foreach (var editorGroupElement in editorGroupHeaderList)
            {
                if (editorGroupElement.FindElement(By.CssSelector("div:nth-child(2)")).Text.Contains(groupName))
                {
                    if (editorGroupElement.FindElements(By.CssSelector("div.icon-collapsed")).Count > 0)
                    {
                        editorGroupElement.FindElement(By.CssSelector("div.icon-collapsed")).ClickByJS();

                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Expand all group of current active tab
        /// </summary>
        public void ExpandGroupsActiveTab()
        {
            var groups = Driver.FindElements(By.CssSelector("[id$='editor-device'] [id*='editor-device-content-properties-tab'].editor-tab[style*='display: block'] .editor-group-header"));
            foreach (var group in groups)
            {
                if (group.FindElements(By.CssSelector("div.icon-collapsed")).Count > 0)
                {
                    group.FindElement(By.CssSelector("div.icon-collapsed")).ClickByJS();
                }
            }
        }

        /// <summary>
        /// Collapse an editor group by its caption
        /// </summary>
        /// <param name="groupName"></param>
        public void CollapseGroup(string groupName)
        {
            foreach (var editorGroupElement in editorGroupHeaderList)
            {
                if (editorGroupElement.FindElement(By.CssSelector("div:nth-child(2)")).Text.Contains(groupName))
                {
                    if (editorGroupElement.FindElements(By.CssSelector("div.icon-expanded")).Count > 0)
                    {
                        editorGroupElement.FindElement(By.CssSelector("div.icon-expanded")).ClickByJS();

                        break;
                    }
                }
            }
        }

        /// <summary>
        ///  Collapse all group of current active tab
        /// </summary>
        public void CollapseGroupsActiveTab()
        {
            var groups = Driver.FindElements(By.CssSelector("[id*='editor-device-content-properties-tab'].editor-tab[style*='display: block'] .editor-group-header"));
            foreach (var group in groups)
            {
                if (group.FindElements(By.CssSelector("div.icon-expanded")).Count > 0)
                {
                    group.FindElement(By.CssSelector("div.icon-expanded")).ClickByJS();
                }
            }
        }

        /// <summary>
        /// Check if an editor group is expanded
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public bool IsGroupExpanded(string groupName)
        {
            foreach (var editorGroupElement in editorGroupHeaderList)
            {
                if (editorGroupElement.FindElement(By.CssSelector("div:nth-child(2)")).Text.Contains(groupName))
                {
                    return editorGroupElement.FindElements(By.CssSelector("div.icon-expanded")).Count > 0;
                }
            }
            Assert.Warn(string.Format("Editor group with group {0} is not found", groupName));

            return false;
        }

        /// <summary>
        /// Check if an editor group is collapsed
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public bool IsGroupCollapsed(string groupName)
        {
            foreach (var editorGroupElement in editorGroupHeaderList)
            {
                if (editorGroupElement.FindElement(By.CssSelector("div:nth-child(2)")).Text.Contains(groupName))
                {
                    return editorGroupElement.FindElements(By.CssSelector("div.icon-collapsed")).Count > 0;
                }
            }
            Assert.Warn(string.Format("Editor group with group {0} is not found", groupName));

            return false;
        }

        /// <summary>
        /// Check if a group is exisiting
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool IsGroupExistingActiveTab(string name)
        {
            var groupNames = GetListOfGroupsNameActiveTab();
            return groupNames.Any(p => p.Equals(name));
        }

        /// <summary>
        /// Check if a group is exisiting
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool IsGroupExisting(string name)
        {
            var groupNames = GetListOfGroupsName();
            return groupNames.Any(p => p.Equals(name));
        }

        public Dictionary<string, string> GetEditablePropertiesValue()
        {
            var result = new Dictionary<string, string>();
            var tabs = GetListOfTabsName();

            foreach (var tab in tabs)
            {
                SelectTab(tab);
                ExpandGroupsActiveTab();

                var properties = Driver.FindElements(By.CssSelector("[id$='editor-device'] [id$='editor-device-content-properties'] .editor-tab[style*='display: block'] .editor-property"));
                foreach (var property in properties)
                {
                    var key = property.GetAttribute("id");
                    var editor = property.FindElement(By.CssSelector("[id^='editor-property'][id$='field']"));
                    if (!editor.IsReadOnly())
                    {
                        string value = string.Empty;
                        var ccsClass = editor.GetAttribute("class");
                        if (ccsClass.Contains("editor-select"))//dropdown
                        {
                            value = editor.Text;
                        }
                        else if (ccsClass.Contains("editor-field")) //input
                        {
                            value = editor.Value();
                        }
                        else if (ccsClass.Contains("checkbox")) //checkbox
                        {
                            Wait.ForMilliseconds(500);
                            value = editor.CheckboxValue().ToString();
                        }
                       
                        if (!result.ContainsKey(key))
                            result.Add(key, value);
                        else
                            Assert.Warn(string.Format("{0} has already been added into Dictionary", key));
                    }
                }
            }

            return result;
        }

        public Dictionary<string, string> GetReadOnlyPropertiesValue()
        {
            var result = new Dictionary<string, string>();
            var tabs = GetListOfTabsName();

            foreach (var tab in tabs)
            {
                SelectTab(tab);
                ExpandGroupsActiveTab();

                var properties = Driver.FindElements(By.CssSelector("[id$='editor-device'] [id$='editor-device-content-properties'] .editor-tab[style*='display: block'] .editor-property"));
                foreach (var property in properties)
                {
                    var label = property.FindElement(By.CssSelector("div.editor-label:nth-child(1)"));
                    var editor = property.FindElement(By.CssSelector("[id^='editor-property'][id$='field']"));
                    if (editor.IsReadOnly())
                    {
                        string value = string.Empty;
                        var ccsClass = editor.GetAttribute("class");
                        if (ccsClass.Contains("editor-select"))//dropdown
                        {
                            value = editor.Text;
                        }
                        else if (ccsClass.Contains("editor-field")) //input
                        {
                            value = editor.Value();
                        }
                        else if (ccsClass.Contains("checkbox")) //checkbox
                        {
                            Wait.ForMilliseconds(500);
                            value = editor.CheckboxValue().ToString();
                        }

                        result.Add(label.Text.Trim(), value);
                    }
                }
            }

            return result;
        }

        public void EnterEditablePropertiesValue()
        {
            var tabs = GetListOfTabsName();

            foreach (var tab in tabs)
            {
                SelectTab(tab);
                ExpandGroupsActiveTab();
                var properties = Driver.FindElements(By.CssSelector("[id$='editor-device'] [id$='editor-device-content-properties'] .editor-tab[style*='display: block'] .editor-property"));

                foreach (var property in properties)
                {
                    var label = property.FindElement(By.CssSelector("div.editor-label:nth-child(1)"));
                    var editor = property.FindElement(By.CssSelector("[id^='editor-property'][id$='field']"));
                    if (!editor.IsReadOnly())
                    {
                        string value = string.Empty;
                        var ccsClass = editor.GetAttribute("class");
                        if (ccsClass.Contains("editor-select"))//dropdown
                        {
                            value = editor.Text;
                            var items = editor.GetAllItems();
                            items.Remove(value);
                            items.RemoveAll(p => string.IsNullOrEmpty(p));
                            editor.Select(items.PickRandom());
                        }
                        else if (ccsClass.Contains("editor-field")) //input
                        {
                            editor.ClickEx();
                            if (property.FindElements(By.CssSelector(".w2ui-field-helper")).Any()) //numeric input
                            {
                                editor.Enter(SLVHelper.GenerateStringInteger(999));
                            }
                            else //text input
                            {
                                if (label.Text.ToLower().Contains("date"))
                                    editor.Enter(string.Format(@"{0}/{1}/{2}", SLVHelper.GenerateStringInteger(12), SLVHelper.GenerateStringInteger(28), SLVHelper.GenerateStringInteger(1900, DateTime.Now.AddYears(-1).Year)));
                                else if (label.Text.ToLower().Contains("unique address"))
                                    editor.Enter(SLVHelper.GenerateMACAddress());
                                else
                                    editor.Enter(SLVHelper.GenerateString(9));
                            }
                        }
                        else if (ccsClass.Contains("checkbox")) //checkbox
                        {
                            Wait.ForMilliseconds(500);
                            editor.Check(!editor.CheckboxValue());
                        }
                    }
                }
            }
        }

        public void EnterEditablePropertiesValue(params string[] exceptList)
        {
            var tabs = GetListOfTabsName();

            foreach (var tab in tabs)
            {
                SelectTab(tab);
                ExpandGroupsActiveTab();
                var properties = Driver.FindElements(By.CssSelector("[id$='editor-device'] [id$='editor-device-content-properties'] .editor-tab[style*='display: block'] .editor-property"));

                foreach (var property in properties)
                {
                    var label = property.FindElement(By.CssSelector("div.editor-label:nth-child(1)"));
                    var fieldName = label.Text.Trim();
                    if (!exceptList.Contains(fieldName))
                    {
                        var editor = property.FindElement(By.CssSelector("[id^='editor-property'][id$='field']"));
                        if (!editor.IsReadOnly())
                        {
                            string value = string.Empty;
                            var ccsClass = editor.GetAttribute("class");
                            if (ccsClass.Contains("editor-select"))//dropdown
                            {
                                value = editor.Text;
                                var items = new List<string>();
                                if (fieldName.Equals("Time zone"))
                                    items = SLVHelper.GenerateTimezone();
                                else
                                    items = editor.GetAllItems();
                                items.Remove(value);
                                items.RemoveAll(p => string.IsNullOrEmpty(p));
                                if (items.Any()) editor.Select(items.PickRandom());
                            }
                            else if (ccsClass.Contains("editor-field")) //input
                            {
                                editor.ClickEx();
                                if (property.FindElements(By.CssSelector(".w2ui-field-helper")).Any()) //numeric input
                                {
                                    editor.Enter(SLVHelper.GenerateStringInteger(999));
                                }
                                else //text input
                                {
                                    if (fieldName.ToLower().Contains("date"))                                        
                                        editor.Enter(string.Format(@"{0}-{1}-{2}", SLVHelper.GenerateStringInteger(1900, DateTime.Now.AddYears(-1).Year), SLVHelper.GenerateInteger(12).ToString("D2"), SLVHelper.GenerateInteger(28).ToString("D2")));
                                    else if (fieldName.ToLower().Contains("unique address"))
                                        editor.Enter(SLVHelper.GenerateMACAddress());
                                    else
                                        editor.Enter(SLVHelper.GenerateString(6));
                                }
                            }
                            else if (ccsClass.Contains("checkbox")) //checkbox
                            {
                                Wait.ForMilliseconds(500);
                                editor.Check(!editor.CheckboxValue());
                            }
                        }
                    }
                }
            }
        }

        public decimal GetLongitudeValueNumber()
        {
            return decimal.Parse(GetLongitudeValue().Replace(" °", string.Empty).Trim());
        }

        public decimal GetLatitudeValueNumber()
        {
            return decimal.Parse(GetLatitudeValue().Replace(" °", string.Empty).Trim());
        }

        #endregion //Business methods   

        public override void WaitForPanelLoaded()
        {
            Wait.ForSeconds(1);
            Wait.ForElementText(deviceCaptionLabel);
        }
    }
}
