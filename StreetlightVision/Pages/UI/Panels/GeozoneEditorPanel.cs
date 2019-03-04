using NUnit.Framework;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using StreetlightVision.Extensions;
using StreetlightVision.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StreetlightVision.Pages.UI
{
    public class GeozoneEditorPanel : NodeEditorPanelBase
    {
        #region Variables

        private ImportPanel _importPanel;
        private ExportPanel _exportPanel;
        private NewDevicePanel _newDevicePanel;
        private ReplaceNodesPanel _replaceNodesPanel;
        private ImportCommissionPanel _importCommissionPanel;

        #endregion //Variables

        #region IWebElements

        #region Header toolbar buttons

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-buttons-toolbar_item_cancel'] .w2ui-button")]
        private IWebElement backButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-buttons-toolbar_item_add'] .w2ui-button")]
        private IWebElement addButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='w2ui-overlay'] tr[index='0']")]
        private IWebElement addGeozoneMenuItem;

        [FindsBy(How = How.CssSelector, Using = "[id$='w2ui-overlay'] tr[index='1']")]
        private IWebElement addDeviceMenuItem;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-buttons-toolbar_item_save'] .w2ui-button")]
        private IWebElement saveButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-buttons-toolbar_item_delete'] .w2ui-button")]
        private IWebElement deleteButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-buttons-toolbar_item_more'] .w2ui-button")]
        private IWebElement moreButton;

        [FindsBy(How = How.CssSelector, Using = "[id='w2ui-overlay'] tr[index='0']")]
        private IWebElement importMenuItem;

        [FindsBy(How = How.CssSelector, Using = "[id='w2ui-overlay'] tr[index='1']")]
        private IWebElement exportMenuItem;

        [FindsBy(How = How.CssSelector, Using = "[id='w2ui-overlay'] tr[index='2']")]
        private IWebElement replaceNodesMenuItem;

        [FindsBy(How = How.CssSelector, Using = "[id='w2ui-overlay'] tr .menu-text")]
        private IList<IWebElement> overlayMenuItemsList;

        #endregion //Header toolbar buttons

        #region Content Identity

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-content-name'] .slv-label.equipment-gl-editor-label")]
        private IWebElement nameLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-content-parent'] .slv-label.equipment-gl-editor-label")]
        private IWebElement parentGeozoneLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-content-name-field']")]
        private IWebElement nameInput;

        [FindsBy(How = How.CssSelector, Using = "div[id$='editor-geozone-content-parent-field']")]
        private IWebElement parentGeozoneDropDown;

        [FindsBy(How = How.CssSelector, Using = "input[id$='editor-geozone-content-parent-field']")]
        private IWebElement parentGeozoneTextBoxInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-content-updatebounds-button']")]
        private IWebElement udpateBoundsButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-content-latmin-label']")]
        private IWebElement latitudeMinimumLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-content-latmax-label']")]
        private IWebElement latitudeMaximumLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-content-lngmin-label']")]
        private IWebElement longitudeMinimumLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-content-lngmax-label']")]
        private IWebElement longitudeMaximumLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-content-latmin-field']")]
        private IWebElement latitudeMinimumInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-content-latmax-field']")]
        private IWebElement latitudeMaximumInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-content-lngmin-field']")]
        private IWebElement longitudeMinimumInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-content-lngmax-field']")]
        private IWebElement longitudeMaximumInput;

        #endregion //Content Identity

        #region Properties tab

        //Virtual Energy Consumption

        [FindsBy(How = How.CssSelector, Using = "[id*='editor-geozone-content-content-properties-group'] > div[dir]")]
        private IWebElement virtualEnergyConsumptionCaption;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-virtual-energy-consumption-jan'] .slv-label.editor-label")]
        private IWebElement januaryLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-virtual-energy-consumption-feb'] .slv-label.editor-label")]
        private IWebElement februaryLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-virtual-energy-consumption-mar'] .slv-label.editor-label")]
        private IWebElement marchLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-virtual-energy-consumption-apr'] .slv-label.editor-label")]
        private IWebElement aprilLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-virtual-energy-consumption-may'] .slv-label.editor-label")]
        private IWebElement mayLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-virtual-energy-consumption-jun'] .slv-label.editor-label")]
        private IWebElement juneLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-virtual-energy-consumption-jul'] .slv-label.editor-label")]
        private IWebElement julyLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-virtual-energy-consumption-aug'] .slv-label.editor-label")]
        private IWebElement augustLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-virtual-energy-consumption-sep'] .slv-label.editor-label")]
        private IWebElement septemberLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-virtual-energy-consumption-oct'] .slv-label.editor-label")]
        private IWebElement octoberLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-virtual-energy-consumption-nov'] .slv-label.editor-label")]
        private IWebElement novemberLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-virtual-energy-consumption-dec'] .slv-label.editor-label")]
        private IWebElement decemberLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-virtual-energy-consumption-jan-field']")]
        private IWebElement januaryNumericInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-virtual-energy-consumption-feb-field']")]
        private IWebElement februaryNumericInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-virtual-energy-consumption-mar-field']")]
        private IWebElement marchNumericInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-virtual-energy-consumption-apr-field']")]
        private IWebElement aprilNumericInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-virtual-energy-consumption-may-field']")]
        private IWebElement mayNumericInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-virtual-energy-consumption-jun-field']")]
        private IWebElement juneNumericInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-virtual-energy-consumption-jul-field']")]
        private IWebElement julyNumericInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-virtual-energy-consumption-aug-field']")]
        private IWebElement augustNumericInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-virtual-energy-consumption-sep-field']")]
        private IWebElement septemberNumericInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-virtual-energy-consumption-oct-field']")]
        private IWebElement octoberNumericInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-virtual-energy-consumption-nov-field']")]
        private IWebElement novemberNumericInput;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-property-virtual-energy-consumption-dec-field']")]
        private IWebElement decemberNumericInput;

        [FindsBy(How = How.CssSelector, Using = "[id^='editor-property-virtual-energy-consumption'].editor-property input")]
        private IList<IWebElement> virtualEnergyConsumptionInputsList;

        #endregion //Properties tab

        #region Footer toolbar buttons

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-footer-toolbar_item_customreport'] .w2ui-button")]
        private IWebElement customReportButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-footer-toolbar_item_customreport'] .w2ui-tb-caption")]
        private IWebElement customReportCaptionLabel;

        #endregion //Footer toolbar buttons

        #region Add Geozone

        #endregion //Add Geozone

        #endregion //IWebElements

        #region Constructor

        public GeozoneEditorPanel(IWebDriver driver, PageBase page) : base(driver, page)
        {
            PageFactory.InitElements(this, new RetryingElementLocator(driver, TimeSpan.FromMilliseconds(Settings.LocatorRetryingTimeout), TimeSpan.FromMilliseconds(Settings.LocatorPollingInterval)));

            WaitForPanelLoaded();
        }

        #endregion //Constructor

        #region Properties

        public ImportPanel ImportPanel
        {
            get
            {
                if (_importPanel == null)
                {
                    _importPanel = new ImportPanel(this.Driver, this.Page);
                }

                return _importPanel;
            }
        }

        public ExportPanel ExportPanel
        {
            get
            {
                if (_exportPanel == null)
                {
                    _exportPanel = new ExportPanel(this.Driver, this.Page);
                }

                return _exportPanel;
            }
        }

        public NewDevicePanel NewDevicePanel
        {
            get
            {
                if (_newDevicePanel == null)
                {
                    _newDevicePanel = new NewDevicePanel(this.Driver, this.Page);
                }

                return _newDevicePanel;
            }
        }

        public ReplaceNodesPanel ReplaceNodesPanel
        {
            get
            {
                if (_replaceNodesPanel == null)
                {
                    _replaceNodesPanel = new ReplaceNodesPanel(this.Driver, this.Page);
                }

                return _replaceNodesPanel;
            }
        }

        public ImportCommissionPanel ImportCommissionPanel
        {
            get
            {
                if (_importCommissionPanel == null)
                {
                    _importCommissionPanel = new ImportCommissionPanel(this.Driver, this.Page);
                }

                return _importCommissionPanel;
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
        /// Click 'Add' button
        /// </summary>
        public void ClickAddButton()
        {
            addButton.ClickEx();
            Wait.ForElementsDisplayed(By.CssSelector("[id='w2ui-overlay']"));
            Wait.ForMilliseconds(500);
        }

        /// <summary>
        /// Click 'AddGeozone' menu item
        /// </summary>
        public void ClickAddGeozoneMenuItem()
        {
            addGeozoneMenuItem.ClickEx();
        }

        /// <summary>
        /// Click 'AddDevice' menu item
        /// </summary>
        public void ClickAddDeviceMenuItem()
        {
            addDeviceMenuItem.ClickEx();
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

        /// <summary>
        /// Click 'More' button
        /// </summary>
        public void ClickMoreButton()
        {
            moreButton.ClickEx();
            Wait.ForElementsDisplayed(By.CssSelector("[id='w2ui-overlay']"));
            Wait.ForMilliseconds(500);
        }

        /// <summary>
        /// Click 'Import' menu item
        /// </summary>
        public void ClickImportMenuItem()
        {
            importMenuItem.ClickEx();
        }

        /// <summary>
        /// Click 'Export' menu item
        /// </summary>
        public void ClickExportMenuItem()
        {
            exportMenuItem.ClickEx();
        }

        /// <summary>
        /// Click 'ReplaceNode' menu item
        /// </summary>
        public void ClickReplaceNodesMenuItem()
        {
            replaceNodesMenuItem.ClickEx();
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
        /// Select an item of 'ParentGeozone' dropdown 
        /// </summary>
        /// <param name="value"></param>
        public void SelectParentGeozoneDropDown(string value)
        {
            parentGeozoneDropDown.Select(value);
        }

        /// <summary>
        /// Click 'UdpateBounds' button
        /// </summary>
        public void ClickUpdateBoundsButton()
        {
            udpateBoundsButton.ClickEx();
        }

        /// <summary>
        /// Enter a value for 'LatitudeMinimum' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterLatitudeMinimumInput(string value)
        {
            latitudeMinimumInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'LatitudeMaximum' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterLatitudeMaximumInput(string value)
        {
            latitudeMaximumInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'LongitudeMinimum' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterLongitudeMinimumInput(string value)
        {
            longitudeMinimumInput.Enter(value);
        }

        /// <summary>
        /// Enter a value for 'LongitudeMaximum' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterLongitudeMaximumInput(string value)
        {
            longitudeMaximumInput.Enter(value);
        }

        #endregion //Content Identity

        #region Properties tab

        /// <summary>
        /// Enter a value for 'January' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterJanuaryNumericInput(string value, bool shouldClear = true)
        {
            januaryNumericInput.Enter(value, shouldClear);
        }

        /// <summary>
        /// Enter a value for 'February' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterFebruaryNumericInput(string value, bool shouldClear = true)
        {
            februaryNumericInput.Enter(value, shouldClear);
        }

        /// <summary>
        /// Enter a value for 'March' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterMarchNumericInput(string value, bool shouldClear = true)
        {
            marchNumericInput.Enter(value, shouldClear);
        }

        /// <summary>
        /// Enter a value for 'April' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterAprilNumericInput(string value, bool shouldClear = true)
        {
            aprilNumericInput.Enter(value, shouldClear);
        }

        /// <summary>
        /// Enter a value for 'May' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterMayNumericInput(string value, bool shouldClear = true)
        {
            mayNumericInput.Enter(value, shouldClear);
        }

        /// <summary>
        /// Enter a value for 'June' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterJuneNumericInput(string value, bool shouldClear = true)
        {
            juneNumericInput.Enter(value, shouldClear);
        }

        /// <summary>
        /// Enter a value for 'July' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterJulyNumericInput(string value, bool shouldClear = true)
        {
            julyNumericInput.Enter(value, shouldClear);
        }

        /// <summary>
        /// Enter a value for 'August' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterAugustNumericInput(string value, bool shouldClear = true)
        {
            augustNumericInput.Enter(value, shouldClear);
        }

        /// <summary>
        /// Enter a value for 'September' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterSeptemberNumericInput(string value, bool shouldClear = true)
        {
            septemberNumericInput.Enter(value, shouldClear);
        }

        /// <summary>
        /// Enter a value for 'October' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterOctoberNumericInput(string value, bool shouldClear = true)
        {
            octoberNumericInput.Enter(value, shouldClear);
        }

        /// <summary>
        /// Enter a value for 'November' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterNovemberNumericInput(string value, bool shouldClear = true)
        {
            novemberNumericInput.Enter(value, shouldClear);
        }

        /// <summary>
        /// Enter a value for 'December' input
        /// </summary>
        /// <param name="value"></param>
        public void EnterDecemberNumericInput(string value, bool shouldClear = true)
        {
            decemberNumericInput.Enter(value, shouldClear);
        }

        #endregion //Properties tab

        #region Footer toolbar buttons

        /// <summary>
        /// Click 'CustomReport' button
        /// </summary>
        public void ClickCustomReportButton()
        {
            customReportButton.ClickEx();
        }

        #endregion //Footer toolbar buttons

        #region Add Geozone

        #endregion //Add Geozone

        #endregion //Actions

        #region Get methods

        #region Header toolbar buttons

        #endregion //Header toolbar buttons

        #region Content Identity

        /// <summary>
        /// Get 'Name' label text
        /// </summary>
        /// <returns></returns>
        public string GetNameText()
        {
            return nameLabel.Text;
        }

        /// <summary>
        /// Get 'ParentGeozone' label text
        /// </summary>
        /// <returns></returns>
        public string GetParentGeozoneText()
        {
            return parentGeozoneLabel.Text;
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
        /// Get 'ParentGeozone' input value
        /// </summary>
        /// <returns></returns>
        public string GetParentGeozoneValue()
        {
            return parentGeozoneDropDown.Text;
        }

        /// <summary>
        /// Get 'ParentGeozoneTextBox' input value
        /// </summary>
        /// <returns></returns>
        public string GetParentGeozoneTextBoxValue()
        {
            return parentGeozoneTextBoxInput.Value();
        }

        /// <summary>
        /// Get 'LatitudeMinimum' label text
        /// </summary>
        /// <returns></returns>
        public string GetLatitudeMinimumText()
        {
            return latitudeMinimumLabel.Text;
        }

        /// <summary>
        /// Get 'LatitudeMaximum' label text
        /// </summary>
        /// <returns></returns>
        public string GetLatitudeMaximumText()
        {
            return latitudeMaximumLabel.Text;
        }

        /// <summary>
        /// Get 'LongitudeMinimum' label text
        /// </summary>
        /// <returns></returns>
        public string GetLongitudeMinimumText()
        {
            return longitudeMinimumLabel.Text;
        }

        /// <summary>
        /// Get 'LongitudeMaximum' label text
        /// </summary>
        /// <returns></returns>
        public string GetLongitudeMaximumText()
        {
            return longitudeMaximumLabel.Text;
        }

        /// <summary>
        /// Get 'LatitudeMinimum' input value
        /// </summary>
        /// <returns></returns>
        public string GetLatitudeMinimumValue()
        {
            return latitudeMinimumInput.Value();
        }

        /// <summary>
        /// Get 'LatitudeMaximum' input value
        /// </summary>
        /// <returns></returns>
        public string GetLatitudeMaximumValue()
        {
            return latitudeMaximumInput.Value();
        }

        /// <summary>
        /// Get 'LongitudeMinimum' input value
        /// </summary>
        /// <returns></returns>
        public string GetLongitudeMinimumValue()
        {
            return longitudeMinimumInput.Value();
        }

        /// <summary>
        /// Get 'LongitudeMaximum' input value
        /// </summary>
        /// <returns></returns>
        public string GetLongitudeMaximumValue()
        {
            return longitudeMaximumInput.Value();
        }

        #endregion //Content Identity

        #region Properties tab

        /// <summary>
        /// Get 'January' label text
        /// </summary>
        /// <returns></returns>
        public string GetJanuaryText()
        {
            return januaryLabel.Text;
        }

        /// <summary>
        /// Get 'February' label text
        /// </summary>
        /// <returns></returns>
        public string GetFebruaryText()
        {
            return februaryLabel.Text;
        }

        /// <summary>
        /// Get 'March' label text
        /// </summary>
        /// <returns></returns>
        public string GetMarchText()
        {
            return marchLabel.Text;
        }

        /// <summary>
        /// Get 'April' label text
        /// </summary>
        /// <returns></returns>
        public string GetAprilText()
        {
            return aprilLabel.Text;
        }

        /// <summary>
        /// Get 'May' label text
        /// </summary>
        /// <returns></returns>
        public string GetMayText()
        {
            return mayLabel.Text;
        }

        /// <summary>
        /// Get 'June' label text
        /// </summary>
        /// <returns></returns>
        public string GetJuneText()
        {
            return juneLabel.Text;
        }

        /// <summary>
        /// Get 'July' label text
        /// </summary>
        /// <returns></returns>
        public string GetJulyText()
        {
            return julyLabel.Text;
        }

        /// <summary>
        /// Get 'August' label text
        /// </summary>
        /// <returns></returns>
        public string GetAugustText()
        {
            return augustLabel.Text;
        }

        /// <summary>
        /// Get 'September' label text
        /// </summary>
        /// <returns></returns>
        public string GetSeptemberText()
        {
            return septemberLabel.Text;
        }

        /// <summary>
        /// Get 'October' label text
        /// </summary>
        /// <returns></returns>
        public string GetOctoberText()
        {
            return octoberLabel.Text;
        }

        /// <summary>
        /// Get 'November' label text
        /// </summary>
        /// <returns></returns>
        public string GetNovemberText()
        {
            return novemberLabel.Text;
        }

        /// <summary>
        /// Get 'December' label text
        /// </summary>
        /// <returns></returns>
        public string GetDecemberText()
        {
            return decemberLabel.Text;
        }

        /// <summary>
        /// Get 'January' input value
        /// </summary>
        /// <returns></returns>
        public string GetJanuaryValue()
        {
            return januaryNumericInput.Value();
        }

        /// <summary>
        /// Get 'February' input value
        /// </summary>
        /// <returns></returns>
        public string GetFebruaryValue()
        {
            return februaryNumericInput.Value();
        }

        /// <summary>
        /// Get 'March' input value
        /// </summary>
        /// <returns></returns>
        public string GetMarchValue()
        {
            return marchNumericInput.Value();
        }

        /// <summary>
        /// Get 'April' input value
        /// </summary>
        /// <returns></returns>
        public string GetAprilValue()
        {
            return aprilNumericInput.Value();
        }

        /// <summary>
        /// Get 'May' input value
        /// </summary>
        /// <returns></returns>
        public string GetMayValue()
        {
            return mayNumericInput.Value();
        }

        /// <summary>
        /// Get 'June' input value
        /// </summary>
        /// <returns></returns>
        public string GetJuneValue()
        {
            return juneNumericInput.Value();
        }

        /// <summary>
        /// Get 'July' input value
        /// </summary>
        /// <returns></returns>
        public string GetJulyValue()
        {
            return julyNumericInput.Value();
        }

        /// <summary>
        /// Get 'August' input value
        /// </summary>
        /// <returns></returns>
        public string GetAugustValue()
        {
            return augustNumericInput.Value();
        }

        /// <summary>
        /// Get 'September' input value
        /// </summary>
        /// <returns></returns>
        public string GetSeptemberValue()
        {
            return septemberNumericInput.Value();
        }

        /// <summary>
        /// Get 'October' input value
        /// </summary>
        /// <returns></returns>
        public string GetOctoberValue()
        {
            return octoberNumericInput.Value();
        }

        /// <summary>
        /// Get 'November' input value
        /// </summary>
        /// <returns></returns>
        public string GetNovemberValue()
        {
            return novemberNumericInput.Value();
        }

        /// <summary>
        /// Get 'December' input value
        /// </summary>
        /// <returns></returns>
        public string GetDecemberValue()
        {
            return decemberNumericInput.Value();
        }

        #endregion //Properties tab

        #region Footer toolbar buttons

        /// <summary>
        /// Get 'CustomReportCaption' label text
        /// </summary>
        /// <returns></returns>
        public string GetCustomReportCaptionText()
        {
            return customReportCaptionLabel.Text;
        }

        #endregion //Footer toolbar buttons

        #region Add Geozone

        #endregion //Add Geozone

        #endregion //Get methods

        #endregion //Basic methods

        #region Business methods

        public void WaitForNewDevicePanelDisplayed()
        {
            Wait.ForElementDisplayed(By.CssSelector("[id$='editor-geozone-createDevice']"));
            Wait.ForElementStyle(By.CssSelector("[id$='editor-geozone-createDevice']"), "left: 0px");
        }

        public void WaitForNewDevicePanelDisappeared()
        {
            Wait.ForElementStyle(By.CssSelector("[id$='editor-geozone-createDevice']"), "left: 350px");
        }

        public void WaitForExportPanelDisplayed()
        {
            Wait.ForElementDisplayed(By.CssSelector("[id$='editor-geozone-export']"));
            Wait.ForElementStyle(By.CssSelector("[id$='editor-geozone-export']"), "left: 0px");
        }

        public void WaitForExportPanelDisappeared()
        {            
            if (Browser.Name.Equals("IE"))
            {
                Wait.ForSeconds(2);
            }
            else
            {
                Wait.ForElementStyle(By.CssSelector("[id$='editor-geozone-export']"), "left: 350px");
            }
        }

        public void WaitForImportPanelDisplayed()
        {
            Wait.ForElementDisplayed(By.CssSelector("[id$='editor-geozone-import']"));
            Wait.ForElementStyle(By.CssSelector("[id$='editor-geozone-import']"), "left: 0px");
        }

        public void WaitForImportPanelDisappeared()
        {
            Wait.ForElementStyle(By.CssSelector("[id$='editor-geozone-import']"), "left: 350px");
        }

        public bool IsImportPanelDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-geozone-import']"));
        }

        public bool IsImportCommissionPanelDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-geozone-import-commission']"));
        }

        public void WaitForReplaceNodesPanelDisplayed()
        {
            Wait.ForElementDisplayed(By.CssSelector("[id$='editor-geozone-replace-nodes']"));
            Wait.ForElementStyle(By.CssSelector("[id$='editor-geozone-replace-nodes']"), "left: 0px");
        }

        public void WaitForReplaceNodesPanelDisappeared()
        {
            Wait.ForElementStyle(By.CssSelector("[id$='editor-geozone-replace-nodes']"), "left: 350px");
        }

        public bool IsReplaceNodesPanelDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-geozone-replace-nodes']")); ;
        }

        /// <summary>
        /// Check if panel is visible
        /// </summary>
        /// <returns></returns>
        public bool IsPanelVisible()
        {
            return backButton.Displayed;
        }

        public bool IsVirtualEnergyConsumptionVisible()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id*='editor-geozone-content-content-properties-group'] > div[dir]"));
        }

        public bool IsGroupExpanded(string groupName)
        {
            var groupsList = Driver.FindElements(By.CssSelector("[id*='editor-geozone-content-content-properties-group'].editor-group-header"));
            var group = groupsList.FirstOrDefault(p => p.Text.Trim().Equals(groupName));

            if (group == null)
            {
                Assert.Warn(string.Format("Group '{0}' does not exist", groupName));
                return false;
            }

            return group.ChildExists(By.CssSelector(".icon-expanded"));
        }

        public bool IsCustomReportButtonVisible()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-geozone-footer-toolbar_item_customreport'] .w2ui-tb-caption"));
        }

        public bool IsParentGeozoneDropDownVisible()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("div[id$='editor-geozone-content-parent-field']"));
        }
        public bool IsBackButtonDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-geozone-buttons-toolbar_item_cancel'] .w2ui-button"));
        }

        public bool IsSaveButtonDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-geozone-buttons-toolbar_item_save'] .w2ui-button"));
        }

        public bool IsDeleteButtonDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-geozone-buttons-toolbar_item_delete'] .w2ui-button"));
        }

        public bool IsAddNewMenuDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-geozone-buttons-toolbar_item_add'] .w2ui-button"));
        }

        public bool IsMoreMenuDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-geozone-buttons-toolbar_item_more'] .w2ui-button"));
        }

        public bool IsUpdateBoundsButtonDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-geozone-content-updatebounds-button']"));
        }

        public bool IsNameInputReadOnly()
        {
            return nameInput.IsReadOnly();
        }

        public bool IsParentGeozoneDropDownReadOnly()
        {
            return parentGeozoneDropDown.IsReadOnly();
        }

        public bool IsParentGeozoneInputVisible()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("input[id$='editor-geozone-content-parent-field']"));
        }

        public bool IsParentGeozoneInputReadOnly()
        {
            return parentGeozoneTextBoxInput.IsReadOnly();
        }

        public bool IsLatMinInputReadOnly()
        {
            return latitudeMinimumInput.IsReadOnly();
        }

        public bool IsLatMaxInputReadOnly()
        {
            return latitudeMaximumInput.IsReadOnly();
        }

        public bool IsLongMinInputReadOnly()
        {
            return longitudeMinimumInput.IsReadOnly();
        }

        public bool IsLongMaxInputReadOnly()
        {
            return longitudeMaximumInput.IsReadOnly();
        }

        public bool AreVirtualEnergyConsumptionInputsReadOnly()
        {
            if (Driver.FindElements(By.CssSelector("[id^='editor-property-virtual-energy-consumption'].editor-property input")).Count > 0)
            {
                foreach (var input in virtualEnergyConsumptionInputsList)
                {
                    if (!input.IsReadOnly())
                        return false;
                }
            }
            return true;
        }

        public bool AreVirtualEnergyConsumptionInputsEditable()
        {
            if (Driver.FindElements(By.CssSelector("[id^='editor-property-virtual-energy-consumption'].editor-property input")).Count > 0)
            {
                foreach (var input in virtualEnergyConsumptionInputsList)
                {
                    if (input.IsReadOnly())
                        return false;
                }
            }
            return true;
        }

        public bool IsJanNumericUpDownInput()
        {
            var isInputDisplayed = ElementUtility.IsDisplayed(By.CssSelector("[id='editor-property-virtual-energy-consumption-jan'] input"));
            var isDownDisplayed = ElementUtility.IsDisplayed(By.CssSelector("[id='editor-property-virtual-energy-consumption-jan'] .arrow-down"));
            var isUpDisplayed = ElementUtility.IsDisplayed(By.CssSelector("[id='editor-property-virtual-energy-consumption-jan'] .arrow-up"));

            return isInputDisplayed && isDownDisplayed && isUpDisplayed;
        }

        public bool IsFebNumericUpDownInput()
        {
            var isInputDisplayed = ElementUtility.IsDisplayed(By.CssSelector("[id='editor-property-virtual-energy-consumption-feb'] input"));
            var isDownDisplayed = ElementUtility.IsDisplayed(By.CssSelector("[id='editor-property-virtual-energy-consumption-feb'] .arrow-down"));
            var isUpDisplayed = ElementUtility.IsDisplayed(By.CssSelector("[id='editor-property-virtual-energy-consumption-feb'] .arrow-up"));

            return isInputDisplayed && isDownDisplayed && isUpDisplayed;
        }

        public bool IsMarNumericUpDownInput()
        {
            var isInputDisplayed = ElementUtility.IsDisplayed(By.CssSelector("[id='editor-property-virtual-energy-consumption-mar'] input"));
            var isDownDisplayed = ElementUtility.IsDisplayed(By.CssSelector("[id='editor-property-virtual-energy-consumption-mar'] .arrow-down"));
            var isUpDisplayed = ElementUtility.IsDisplayed(By.CssSelector("[id='editor-property-virtual-energy-consumption-mar'] .arrow-up"));

            return isInputDisplayed && isDownDisplayed && isUpDisplayed;
        }

        public bool IsAprNumericUpDownInput()
        {
            var isInputDisplayed = ElementUtility.IsDisplayed(By.CssSelector("[id='editor-property-virtual-energy-consumption-apr'] input"));
            var isDownDisplayed = ElementUtility.IsDisplayed(By.CssSelector("[id='editor-property-virtual-energy-consumption-apr'] .arrow-down"));
            var isUpDisplayed = ElementUtility.IsDisplayed(By.CssSelector("[id='editor-property-virtual-energy-consumption-apr'] .arrow-up"));

            return isInputDisplayed && isDownDisplayed && isUpDisplayed;
        }

        public bool IsMayNumericUpDownInput()
        {
            var isInputDisplayed = ElementUtility.IsDisplayed(By.CssSelector("[id='editor-property-virtual-energy-consumption-may'] input"));
            var isDownDisplayed = ElementUtility.IsDisplayed(By.CssSelector("[id='editor-property-virtual-energy-consumption-may'] .arrow-down"));
            var isUpDisplayed = ElementUtility.IsDisplayed(By.CssSelector("[id='editor-property-virtual-energy-consumption-may'] .arrow-up"));

            return isInputDisplayed && isDownDisplayed && isUpDisplayed;
        }

        public bool IsJunNumericUpDownInput()
        {
            var isInputDisplayed = ElementUtility.IsDisplayed(By.CssSelector("[id='editor-property-virtual-energy-consumption-jun'] input"));
            var isDownDisplayed = ElementUtility.IsDisplayed(By.CssSelector("[id='editor-property-virtual-energy-consumption-jun'] .arrow-down"));
            var isUpDisplayed = ElementUtility.IsDisplayed(By.CssSelector("[id='editor-property-virtual-energy-consumption-jun'] .arrow-up"));

            return isInputDisplayed && isDownDisplayed && isUpDisplayed;
        }

        public bool IsJulNumericUpDownInput()
        {
            var isInputDisplayed = ElementUtility.IsDisplayed(By.CssSelector("[id='editor-property-virtual-energy-consumption-jul'] input"));
            var isDownDisplayed = ElementUtility.IsDisplayed(By.CssSelector("[id='editor-property-virtual-energy-consumption-jul'] .arrow-down"));
            var isUpDisplayed = ElementUtility.IsDisplayed(By.CssSelector("[id='editor-property-virtual-energy-consumption-jul'] .arrow-up"));

            return isInputDisplayed && isDownDisplayed && isUpDisplayed;
        }

        public bool IsAugNumericUpDownInput()
        {
            var isInputDisplayed = ElementUtility.IsDisplayed(By.CssSelector("[id='editor-property-virtual-energy-consumption-aug'] input"));
            var isDownDisplayed = ElementUtility.IsDisplayed(By.CssSelector("[id='editor-property-virtual-energy-consumption-aug'] .arrow-down"));
            var isUpDisplayed = ElementUtility.IsDisplayed(By.CssSelector("[id='editor-property-virtual-energy-consumption-aug'] .arrow-up"));

            return isInputDisplayed && isDownDisplayed && isUpDisplayed;
        }

        public bool IsSepNumericUpDownInput()
        {
            var isInputDisplayed = ElementUtility.IsDisplayed(By.CssSelector("[id='editor-property-virtual-energy-consumption-sep'] input"));
            var isDownDisplayed = ElementUtility.IsDisplayed(By.CssSelector("[id='editor-property-virtual-energy-consumption-sep'] .arrow-down"));
            var isUpDisplayed = ElementUtility.IsDisplayed(By.CssSelector("[id='editor-property-virtual-energy-consumption-sep'] .arrow-up"));

            return isInputDisplayed && isDownDisplayed && isUpDisplayed;
        }

        public bool IsOctNumericUpDownInput()
        {
            var isInputDisplayed = ElementUtility.IsDisplayed(By.CssSelector("[id='editor-property-virtual-energy-consumption-oct'] input"));
            var isDownDisplayed = ElementUtility.IsDisplayed(By.CssSelector("[id='editor-property-virtual-energy-consumption-oct'] .arrow-down"));
            var isUpDisplayed = ElementUtility.IsDisplayed(By.CssSelector("[id='editor-property-virtual-energy-consumption-oct'] .arrow-up"));

            return isInputDisplayed && isDownDisplayed && isUpDisplayed;
        }

        public bool IsNovNumericUpDownInput()
        {
            var isInputDisplayed = ElementUtility.IsDisplayed(By.CssSelector("[id='editor-property-virtual-energy-consumption-nov'] input"));
            var isDownDisplayed = ElementUtility.IsDisplayed(By.CssSelector("[id='editor-property-virtual-energy-consumption-nov'] .arrow-down"));
            var isUpDisplayed = ElementUtility.IsDisplayed(By.CssSelector("[id='editor-property-virtual-energy-consumption-nov'] .arrow-up"));

            return isInputDisplayed && isDownDisplayed && isUpDisplayed;
        }

        public bool IsDecNumericUpDownInput()
        {
            var isInputDisplayed = ElementUtility.IsDisplayed(By.CssSelector("[id='editor-property-virtual-energy-consumption-dec'] input"));
            var isDownDisplayed = ElementUtility.IsDisplayed(By.CssSelector("[id='editor-property-virtual-energy-consumption-dec'] .arrow-down"));
            var isUpDisplayed = ElementUtility.IsDisplayed(By.CssSelector("[id='editor-property-virtual-energy-consumption-dec'] .arrow-up"));

            return isInputDisplayed && isDownDisplayed && isUpDisplayed;
        }

        public bool IsNewDevicePanelDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-geozone-createDevice']"));
        }

        public bool IsExportPanelDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-geozone-export']"));
        }

        public int GetVirtualEnergyConsumptionMonthsCount()
        {
            return JSUtility.GetElementsText("div[id^='editor-property-virtual-energy-consumption']").Count;
        }

        /// <summary>
        /// Get all items of More Menu
        /// </summary>
        /// <returns></returns>
        public List<string> GetListOfMoreMenuItems()
        {
            ClickMoreButton();
            var result = overlayMenuItemsList.Select(p => p.Text).ToList();
            moreButton.ClickEx();

            return result;
        }

        /// <summary>
        /// Get all items of More Menu
        /// </summary>
        /// <returns></returns>
        public List<string> GetListOfAddMenuItems()
        {
            ClickAddButton();
            var result = overlayMenuItemsList.Select(p => p.Text).ToList();
            addButton.ClickEx();

            return result;
        }

        /// <summary>
        /// Get all tabs name
        /// </summary>
        /// <returns></returns>
        public List<string> GetListOfTabsName()
        {
            return JSUtility.GetElementsText("[id$='editor-geozone-content-content-properties-tabs'] div.w2ui-tab");
        }

        /// <summary>
        /// Get all groups name
        /// </summary>
        /// <returns></returns>
        public List<string> GetListOfGroupsName()
        {
            return JSUtility.GetElementsText("[id*='editor-geozone-content-content-properties-group'] > div[dir]");
        }

        public void ClearNameInput()
        {
            nameInput.Clear();
        }

        public string GetUdpateBoundsButtonText()
        {
            return udpateBoundsButton.Text;
        }

        public void WaitForImportCommissionPanelDisplayed()
        {
            Wait.ForElementDisplayed(By.CssSelector("[id$='editor-geozone-import-commission']"));
            Wait.ForElementStyle(By.CssSelector("[id$='editor-geozone-import-commission']"), "left: 0px");
        }

        public void WaitForImportCommissionPanelDisappeared()
        {
            Wait.ForElementInvisible(By.CssSelector("[id$='editor-geozone-import-commission']"));
            Wait.ForElementStyle(By.CssSelector("[id$='editor-geozone-import-commission']"), "left: 350px");
        }

        #endregion //Business methods

        public override void WaitForPanelLoaded()
        {
            Wait.ForElementStyle(By.CssSelector("[id$='equipmentgl-editor']"), string.Format("left: {0}px", WebDriverContext.JsExecutor.ExecuteScript("return screen.availWidth - 350 - 60")));
        }

        public override void WaitForPreviousActionComplete()
        {
            base.WaitForPreviousActionComplete();
        }
    }
}