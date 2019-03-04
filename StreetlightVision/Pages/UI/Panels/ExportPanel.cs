using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using StreetlightVision.Extensions;
using StreetlightVision.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StreetlightVision.Pages.UI
{
    public class ExportPanel : PanelBase
    {
        #region Variables

        #endregion //Variables

        #region IWebElements

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-export-backButton']")]
        private IWebElement backButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-export'] .equipment-gl-editor-title-label")]
        private IWebElement panelTitle;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-export-nextButton']")]
        private IWebElement nextButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-export-saveButton']")]
        private IWebElement saveButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-export-downloadButton']")]
        private IWebElement downloadButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-export'] [id$='export-equipmenent-list'] .equipment-gl-list-item")]
        private IList<IWebElement> allDeviceTypeList;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-export'] [id$='export-equipmenent-list'] .equipment-gl-list-item:not(.equipment-gl-list-item-disable)")]
        private IList<IWebElement> checkedDeviceTypeList;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-export'] [id$='export-equipmenent-list'] .equipment-gl-list-item.equipment-gl-list-item-disable")]
        private IList<IWebElement> uncheckedDeviceTypeList;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-export'] [id$='export-property-list'] [id$='export-group-remoteControl'] .equipment-gl-list-item")]
        private IList<IWebElement> allRemoteControlPropertyList;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-export'] [id$='export-property-list'] [id$='export-group-remoteControl'] .equipment-gl-list-item:not(.equipment-gl-list-item-disable)")]
        private IList<IWebElement> checkedRemoteControlPropertyList;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-export'] [id$='export-property-list'] [id$='export-group-remoteControl'] .equipment-gl-list-item.equipment-gl-list-item-disable")]
        private IList<IWebElement> uncheckedRemoteControlPropertyList;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-export'] [id$='export-property-list'] [id$='export-group-others'] .equipment-gl-list-item")]
        private IList<IWebElement> allOthersPropertyList;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-export'] [id$='export-property-list'] [id$='export-group-others'] .equipment-gl-list-item:not(.equipment-gl-list-item-disable)")]
        private IList<IWebElement> checkedOthersPropertyList;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-export'] [id$='export-property-list'] [id$='export-group-others'] .equipment-gl-list-item.equipment-gl-list-item-disable")]
        private IList<IWebElement> uncheckedOthersPropertyList;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-geozone-export'] .equipment-gl-editor-group-header")]
        private IList<IWebElement> groupsList;

        #endregion //IWebElements

        #region Constructor

        public ExportPanel(IWebDriver driver, PageBase page)
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

        /// <summary>
        /// Click 'Next' button
        /// </summary>
        public void ClickNextButton()
        {
            nextButton.ClickEx();
        }

        /// <summary>
        /// Click 'Save' button
        /// </summary>
        public void ClickSaveButton()
        {
            saveButton.ClickEx();
        }

        /// <summary>
        /// Click 'Download' button
        /// </summary>
        public void ClickDownloadButton()
        {
            downloadButton.ClickEx();
        }

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

        #endregion //Get methods

        #endregion //Basic methods

        #region Business methods

        public List<string> GetListOfDeviceTypes()
        {
            return JSUtility.GetElementsText("[id$='editor-geozone-export'] [id$='export-equipmenent-list'] .equipment-gl-list-item .equipment-gl-list-item-title");
        }

        public List<string> GetListOfCheckedDeviceTypes()
        {
            return JSUtility.GetElementsText("[id$='editor-geozone-export'] [id$='export-equipmenent-list'] .equipment-gl-list-item:not(.equipment-gl-list-item-disable) .equipment-gl-list-item-title");
        }

        public List<string> GetListOfUncheckedDeviceTypes()
        {
            return JSUtility.GetElementsText("[id$='editor-geozone-export'] [id$='export-equipmenent-list'] .equipment-gl-list-item.equipment-gl-list-item-disable .equipment-gl-list-item-title");
        }

        public List<string> GetListOfProperties()
        {
            return JSUtility.GetElementsText("[id$='editor-geozone-export'] [id$='export-property-list'] .equipment-gl-list-item .equipment-gl-list-item-label");
        }

        public List<string> GetListOfCheckedProperties()
        {
            return JSUtility.GetElementsText("[id$='editor-geozone-export'] [id$='export-property-list'] .equipment-gl-list-item:not(.equipment-gl-list-item-disable) .equipment-gl-list-item-label");
        }

        public List<string> GetListOfUncheckedProperties()
        {
            return JSUtility.GetElementsText("[id$='editor-geozone-export'] [id$='export-property-list'] .equipment-gl-list-item.equipment-gl-list-item-disable .equipment-gl-list-item-label");
        }

        public List<string> GetListOfRemoteControlProperties()
        {
            return JSUtility.GetElementsText("[id$='editor-geozone-export'] [id$='export-property-list'] [id$='export-group-remoteControl'] .equipment-gl-list-item .equipment-gl-list-item-label");
        }

        public List<string> GetListOfCheckedRemoteControlProperties()
        {
            return JSUtility.GetElementsText("[id$='editor-geozone-export'] [id$='export-property-list'] [id$='export-group-remoteControl'] .equipment-gl-list-item:not(.equipment-gl-list-item-disable) .equipment-gl-list-item-label");
        }

        public List<string> GetListOfUncheckedRemoteControlProperties()
        {
            return JSUtility.GetElementsText("[id$='editor-geozone-export'] [id$='export-property-list'] [id$='export-group-remoteControl'] .equipment-gl-list-item.equipment-gl-list-item-disable .equipment-gl-list-item-label");
        }

        public List<string> GetListOfOthersProperties()
        {
            return JSUtility.GetElementsText("[id$='editor-geozone-export'] [id$='export-property-list'] [id$='export-group-others'] .equipment-gl-list-item .equipment-gl-list-item-label");
        }

        public List<string> GetListOfCheckedOthersProperties()
        {
            return JSUtility.GetElementsText("[id$='editor-geozone-export'] [id$='export-property-list'] [id$='export-group-others'] .equipment-gl-list-item:not(.equipment-gl-list-item-disable) .equipment-gl-list-item-label");
        }

        public List<string> GetListOfUncheckedOthersProperties()
        {
            return JSUtility.GetElementsText("[id$='editor-geozone-export'] [id$='export-property-list'] [id$='export-group-others'] .equipment-gl-list-item.equipment-gl-list-item-disable .equipment-gl-list-item-label");
        }

        #region Device Type

        /// <summary>
        /// check all DeviceType
        /// </summary>
        public void CheckAllDeviceTypes()
        {
            var typeList = new List<IWebElement>(allDeviceTypeList);
            foreach (var type in typeList)
            {
                var checkbox = type.FindElement(By.CssSelector(".checkbox"));
                checkbox.Check(true);
            }
        }

        /// <summary>
        /// Un-check all DeviceType
        /// </summary>
        public void UncheckAllDeviceTypes()
        {
            var typeList = new List<IWebElement>(allDeviceTypeList);
            foreach (var type in typeList)
            {
                var checkbox = type.FindElement(By.CssSelector(".checkbox"));
                checkbox.Check(false);
            }
        }

        /// <summary>
        /// Un-check random DeviceType with specific number count
        /// </summary>
        /// <param name="count"></param>
        public void UncheckRandomDeviceTypes(int count)
        {
            var types = new List<IWebElement>(checkedDeviceTypeList);
            var randomTypes = types.PickRandom(count);
            foreach (var type in randomTypes)
            {
                var checkbox = type.FindElement(By.CssSelector(".checkbox"));
                checkbox.Check(false);
            }
        }

        /// <summary>
        /// check random DeviceType with specific number count
        /// </summary>
        /// <param name="count"></param>
        public void CheckRandomDeviceTypes(int count)
        {
            var types = new List<IWebElement>(uncheckedDeviceTypeList);
            var randomTypes = types.PickRandom(count);
            foreach (var type in randomTypes)
            {
                var checkbox = type.FindElement(By.CssSelector(".checkbox"));
                checkbox.Check(true);
            }
        }

        /// <summary>
        /// Check Device Type
        /// </summary>
        public void CheckDeviceTypes(params string[] typesName)
        {
            var types = new List<IWebElement>(allDeviceTypeList);
            foreach (var type in types)
            {
                if (typesName.Contains(type.Text))
                {
                    var checkbox = type.FindElement(By.CssSelector(".checkbox"));
                    checkbox.Check(true);
                }
            }
        }

        /// <summary>
        /// Uncheck Device Type
        /// </summary>
        public void UncheckDeviceTypes(params string[] typesName)
        {
            var types = new List<IWebElement>(allDeviceTypeList);
            foreach (var type in types)
            {
                if (typesName.Contains(type.Text))
                {
                    var checkbox = type.FindElement(By.CssSelector(".checkbox"));
                    checkbox.Check(false);
                }
            }
        }

        #endregion //Device Type

        #region Remote Control

        /// <summary>
        /// check all RemoteControl property
        /// </summary>
        public void CheckAllRemoteControlProperties()
        {
            var properties = new List<IWebElement>(allRemoteControlPropertyList);
            foreach (var property in properties)
            {
                var checkbox = property.FindElement(By.CssSelector(".checkbox"));
                checkbox.Check(true);
            }
        }

        /// <summary>
        /// Un-check all RemoteControl property
        /// </summary>
        public void UncheckAllRemoteControlProperties()
        {
            var properties = new List<IWebElement>(allRemoteControlPropertyList);
            foreach (var property in properties)
            {
                var checkbox = property.FindElement(By.CssSelector(".checkbox"));
                checkbox.Check(false);
            }
        }

        /// <summary>
        /// Un-check random RemoteControl property with specific number count
        /// </summary>
        /// <param name="count"></param>
        public void UncheckRandomRemoteControlProperties(int count)
        {
            var properties = new List<IWebElement>(checkedRemoteControlPropertyList);
            var randomProperties = properties.PickRandom(count);
            foreach (var property in randomProperties)
            {
                var checkbox = property.FindElement(By.CssSelector(".checkbox"));
                checkbox.Check(false);
            }
        }

        /// <summary>
        /// check random RemoteControl property with specific number count
        /// </summary>
        /// <param name="count"></param>
        public void CheckRandomRemoteControlProperties(int count)
        {
            var properties = new List<IWebElement>(uncheckedRemoteControlPropertyList);
            var randomProperties = properties.PickRandom(count);
            foreach (var property in randomProperties)
            {
                var checkbox = property.FindElement(By.CssSelector(".checkbox"));
                checkbox.Check(true);
            }
        }

        /// <summary>
        /// Check RemoteControl property
        /// </summary>
        public void CheckRemoteControlProperties(params string[] propertiesName)
        {
            var properties = new List<IWebElement>(allRemoteControlPropertyList);
            foreach (var property in properties)
            {
                if (propertiesName.Contains(property.Text))
                {
                    var checkbox = property.FindElement(By.CssSelector(".checkbox"));
                    checkbox.Check(true);
                }
            }
        }

        /// <summary>
        /// Uncheck RemoteControl property
        /// </summary>
        public void UncheckRemoteControlProperties(params string[] propertiesName)
        {
            var properties = new List<IWebElement>(allRemoteControlPropertyList);
            foreach (var property in properties)
            {
                if (propertiesName.Contains(property.Text))
                {
                    var checkbox = property.FindElement(By.CssSelector(".checkbox"));
                    checkbox.Check(false);
                }
            }
        }

        #endregion //Remote Control

        #region Others

        /// <summary>
        /// check all Others property
        /// </summary>
        public void CheckAllOthersProperties()
        {
            var properties = new List<IWebElement>(allOthersPropertyList);
            foreach (var property in properties)
            {
                var checkbox = property.FindElement(By.CssSelector(".checkbox"));
                checkbox.Check(true);
            }
        }

        /// <summary>
        /// Un-check all Others property
        /// </summary>
        public void UncheckAllOthersProperties()
        {
            var properties = new List<IWebElement>(allOthersPropertyList);
            foreach (var property in properties)
            {
                var checkbox = property.FindElement(By.CssSelector(".checkbox"));
                checkbox.Check(false);
            }
        }

        /// <summary>
        /// Un-check random Others property with specific number count
        /// </summary>
        /// <param name="count"></param>
        public void UncheckRandomOthersProperties(int count)
        {
            var properties = new List<IWebElement>(checkedOthersPropertyList);
            var randomProperties = properties.PickRandom(count);
            foreach (var property in randomProperties)
            {
                var checkbox = property.FindElement(By.CssSelector(".checkbox"));
                checkbox.Check(false);
            }
        }

        /// <summary>
        /// check random Others property with specific number count
        /// </summary>
        /// <param name="count"></param>
        public void CheckRandomOthersProperties(int count)
        {
            var properties = new List<IWebElement>(uncheckedOthersPropertyList);
            var randomProperties = properties.PickRandom(count);
            foreach (var property in randomProperties)
            {
                var checkbox = property.FindElement(By.CssSelector(".checkbox"));
                checkbox.Check(true);
            }
        }

        /// <summary>
        /// Check Others property
        /// </summary>
        public void CheckOthersProperties(params string[] propertiesName)
        {
            var properties = new List<IWebElement>(allOthersPropertyList);
            foreach (var property in properties)
            {
                if (propertiesName.Contains(property.Text))
                {
                    var checkbox = property.FindElement(By.CssSelector(".checkbox"));
                    checkbox.Check(true);
                }
            }
        }

        /// <summary>
        /// Uncheck Others property
        /// </summary>
        public void UncheckOthersProperties(params string[] propertiesName)
        {
            var properties = new List<IWebElement>(allOthersPropertyList);
            foreach (var property in properties)
            {
                if (propertiesName.Contains(property.Text))
                {
                    var checkbox = property.FindElement(By.CssSelector(".checkbox"));
                    checkbox.Check(false);
                }
            }
        }

        #endregion //Others

        public List<string> GetListOfGroupsName()
        {
            return JSUtility.GetElementsText("[id$='editor-geozone-export'] .equipment-gl-editor-group-header div:nth-child(2)");            
        }

        public int GetExpandedGroupCount()
        {
            return Driver.FindElements(By.CssSelector("[id$='editor-geozone-export'] .equipment-gl-editor-group-header .icon-expanded")).Count;
        }

        public List<string> GetListOfExpandedGroupsName()
        {
            var resultList = new List<string>();

            foreach (var group in groupsList)
            {
                if (group.ChildExists(By.CssSelector(".icon-expanded")))
                    resultList.Add(group.Text.Trim());
            }

            return resultList;
        }

        public void WaitForPropertiesListPanelDisplayed()
        {
            Wait.ForElementDisplayed(By.CssSelector("[id$='editor-geozone-export-property-list']"));
            Wait.ForElementStyle(By.CssSelector("[id$='editor-geozone-export-property-list']"), "left: 0px");         
        }
        
        public void WaitForPropertiesListPanelDisappeared()
        {
            Wait.ForElementInvisible(By.CssSelector("[id$='editor-geozone-export-property-list']"));
            Wait.ForElementStyle(By.CssSelector("[id$='editor-geozone-export-property-list']"), "left: 350px");
        }

        public void WaitForSaveButtonDisplayed()
        {
            Wait.ForElementDisplayed(By.CssSelector("[id$='editor-geozone-export-saveButton']"));
        }

        public void WaitForDownloadButtonDisplayed()
        {
            Wait.ForElementDisplayed(By.CssSelector("[id$='editor-geozone-export-downloadButton']"));
        }

        public bool IsSaveButtonDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-geozone-export-saveButton']"));
        }

        public bool IsDownloadButtonDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-geozone-export-downloadButton']"));
        }

        #endregion //Business methods
    }
}
