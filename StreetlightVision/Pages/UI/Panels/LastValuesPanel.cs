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
    public class LastValuesPanel : PanelBase
    {
        #region Variables

        #endregion //Variables

        #region IWebElements

        [FindsBy(How = How.CssSelector, Using = "[id$='last-values-side-panel-backButton']")]
        private IWebElement backToolbarButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='last-values-side-panel-title'] .slv-icon.slv-meanings-lastvalues-sidepanel-title-icon")]
        private IWebElement deviceTypePanelIcon;

        [FindsBy(How = How.CssSelector, Using = "[id$='last-values-side-panel-title'] .slv-meanings-lastvalues-sidepanel-title-label")]
        private IWebElement selectedDeviceLabel;
        
        [FindsBy(How = How.CssSelector, Using = "[id$='last-values-side-panel-title'] .slv-icon.slv-meanings-lastvalues-sidepanel-title-icon")]
        private IWebElement selectedDeviceIcon;
                
        [FindsBy(How = How.CssSelector, Using = "[id$='last-values-side-panel-content-tabs'] tr > td > div")]
        private IList<IWebElement> tabsList;

        [FindsBy(How = How.CssSelector, Using = "[id$='last-values-side-panel-content-tabs'] tr > td > div.w2ui-tab.active")]
        private IWebElement activeTab;

        [FindsBy(How = How.CssSelector, Using = "[id$='last-values-side-panel-content-tab1'] .slv-meanings-lastvalues-sidepanel-list-item-label")]
        private IList<IWebElement> meteringsContentsLabelList;

        [FindsBy(How = How.CssSelector, Using = "[id$='last-values-side-panel-content-tab1'] .slv-meanings-lastvalues-sidepanel-list-item")]
        private IList<IWebElement> meteringsContentsList;

        [FindsBy(How = How.CssSelector, Using = "[id$='last-values-side-panel-content-tab1'] .slv-meanings-lastvalues-sidepanel-list-item.slv-item-over")]
        private IWebElement hoverMeteringsAttribute;

        [FindsBy(How = How.CssSelector, Using = "[id$='last-values-side-panel-content-tab2'] .slv-meanings-lastvalues-sidepanel-list-item-label")]
        private IList<IWebElement> failuresContentsLabelList;

        [FindsBy(How = How.CssSelector, Using = "[id$='last-values-side-panel-content-tab2'] .slv-meanings-lastvalues-sidepanel-list-item")]
        private IList<IWebElement> failuresContentsList;

        [FindsBy(How = How.CssSelector, Using = "[id$='last-values-side-panel-content-tab2'] .slv-meanings-lastvalues-sidepanel-list-item.slv-item-over")]
        private IWebElement hoverFailuresAttribute;

        [FindsBy(How = How.CssSelector, Using = "[id$='last-values-side-panel']")]
        private IWebElement lastValuesPanel;

        #endregion //IWebElements

        #region Constructor

        public LastValuesPanel(IWebDriver driver, PageBase page) : base(driver, page)
        {
            PageFactory.InitElements(this, new RetryingElementLocator(driver, TimeSpan.FromMilliseconds(Settings.LocatorRetryingTimeout), TimeSpan.FromMilliseconds(Settings.LocatorPollingInterval)));

            WaitForPanelLoaded();
        }

        #endregion //Constructor

        #region Basic methods

        #region Actions

        /// <summary>
        /// Click 'BackToolbar' button
        /// </summary>
        public void ClickBackToolbarButton()
        {
            backToolbarButton.ClickEx();
        }

        #endregion //Actions

        #region Get methods

        /// <summary>
        /// Get 'DeviceTypePanelIcon' input value
        /// </summary>
        /// <returns></returns>
        public string GetDeviceTypePanelIconValue()
        {
            return deviceTypePanelIcon.IconValue();
        }

        /// <summary>
        /// Get 'SelectedDevice' label text
        /// </summary>
        /// <returns></returns>
        public string GetSelectedDeviceText()
        {
            return selectedDeviceLabel.Text;
        }

        /// <summary>
        /// Get 'SelectedDeviceIcon' input value
        /// </summary>
        /// <returns></returns>
        public string GetSelectedDeviceIconValue()
        {
            return selectedDeviceIcon.IconValue();
        }

        /// <summary>
        /// Get 'ActiveTab' label text
        /// </summary>
        /// <returns></returns>
        public string GetActiveTabText()
        {
            return activeTab.Text;
        }

        /// <summary>
        /// Get list tabs text
        /// </summary>
        /// <returns></returns>
        public List<string> GetListOfTabs()
        {
            var tabs = tabsList.Select(t => t.Text).ToList();
            tabs.RemoveAll(p => string.IsNullOrEmpty(p));

            return tabs;
        }

        #endregion //Get methods

        #endregion //Basic methods

        #region Business methods

        /// <summary>
        /// Select a tab with a specific name
        /// </summary>
        /// <param name="name"></param>
        public void SelectTab(string name)
        {
            Wait.ForElementText(activeTab);
            var currentActiveTab = GetActiveTabText();
            if (!currentActiveTab.Equals(name))
            {
                var tab = tabsList.FirstOrDefault(p => p.Text.Equals(name));
                if (tab != null)
                {
                    tab.ClickEx();
                    WaitForPreviousActionComplete();
                    WebDriverContext.Wait.Until(driver => JSUtility.GetElementText("[id$='last-values-side-panel-content-tabs'] tr > td > div.w2ui-tab.active") == name);
                }
            }            
        }        

        /// <summary>
        /// Get all items' label of Meterings tab
        /// </summary>
        /// <param name="tabName"></param>
        /// <returns></returns>
        public List<string> GetMeteringsNameList()
        {
            var list = meteringsContentsLabelList.Select(e => e.Text.TrimEx()).ToList();
            list.RemoveAll(p => p.Equals("Node failure message"));
            list.RemoveAll(p => p.Contains("Node") && p.Contains("failure") && p.Contains("message"));

            return list;
        }

        /// <summary>
        /// Get all items' name has value of Failures tab
        /// </summary>
        /// <returns></returns>
        public List<string> GetMeteringsNameHasValueList()
        {
            var result = new List<string>();

            foreach (var item in meteringsContentsList)
            {
                if (!string.IsNullOrEmpty(item.GetAttribute("title")))
                    result.Add(item.FindElement(By.CssSelector(".slv-meanings-lastvalues-sidepanel-list-item-label")).Text.TrimEx());
            }

            return result;
        }        

        /// <summary>
        /// Get all items' name of Failures tab
        /// </summary>
        /// <param name="tabName"></param>
        /// <returns></returns>
        public List<string> GetFailuresNameList()
        {
            var list = failuresContentsLabelList.Select(e => e.Text.TrimEx()).ToList();
            list.RemoveAll(p => p.Equals("Node failure message"));

            return list;
        }

        /// <summary>
        /// Get all items' name has value of Failures tab
        /// </summary>
        /// <returns></returns>
        public List<string> GetFailuresNameHasValueList()
        {
            var result = new List<string>();

            foreach (var item in failuresContentsList)
            {
                if (!string.IsNullOrEmpty(item.GetAttribute("title")))
                    result.Add(item.FindElement(By.CssSelector(".slv-meanings-lastvalues-sidepanel-list-item-label")).Text.TrimEx());
            }

            return result;
        }

        /// <summary>
        /// Select a specific attribute of Meterings tab
        /// </summary>
        /// <param name="attribute"></param>
        public void SelectMeteringsAttribute(string attribute)
        {
            meteringsContentsLabelList.FirstOrDefault(a => a.Text.TrimEx().Equals(attribute, StringComparison.InvariantCultureIgnoreCase)).ClickEx(); 
        }

        /// <summary>
        /// Select a specific attribute of Failures tab
        /// </summary>
        /// <param name="attribute"></param>
        public void SelectFailuresAttribute(string attribute)
        {
            failuresContentsLabelList.FirstOrDefault(a => a.Text.TrimEx().Equals(attribute, StringComparison.InvariantCultureIgnoreCase)).ClickEx();
        }

        /// <summary>
        /// Check whether current panel displayed
        /// </summary>
        /// <returns></returns>
        public bool IsPanelDisplayed()
        {
            return lastValuesPanel.Displayed;
        }
        
        /// <summary>
        /// Get all Meterings' attributes which have value
        /// </summary>
        /// <returns></returns>
        public List<string> GetMeteringsAttributesWithValue()
        {
            return meteringsContentsList.Where(e => e.GetAttribute("title") != string.Empty).Select(e => e.Text.SplitAndGetAt(0)).ToList();
        }

        /// <summary>
        /// Get all Meterings' attributes which have time value with "d"
        /// </summary>
        /// <returns></returns>
        public List<string> GetMeteringsAttributesWithTimeValue()
        {
            return meteringsContentsList.Where(e => e.GetAttribute("title") != string.Empty && e.Text.SplitAndGetAt(2).Contains("d")).Select(e => e.Text.SplitAndGetAt(0)).ToList();
        }

        /// <summary>
        /// Get hover Meterings' attribute
        /// </summary>
        /// <returns></returns>
        public string GetMeteringsHoverAttribute()
        {
            return hoverMeteringsAttribute.Text.SplitAndGetAt(0);
        }

        /// <summary>
        /// Get Meterings' attribute tooltip
        /// </summary>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public string GetMeteringsTooltipAttribute(string attribute)
        {
            return meteringsContentsList.FirstOrDefault(e => e.Text.SplitAndGetAt(0) == attribute).GetAttribute("title");
        }

        /// <summary>
        /// Get Meterings's time value
        /// </summary>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public string GetMeteringsTimeAttribute(string attribute)
        {
            return meteringsContentsList.FirstOrDefault(e => e.Text.SplitAndGetAt(0) == attribute).Text.SplitAndGetAt(2);
        }

        /// <summary>
        /// Get Metering Value of attribute key (e.g. Lamp current with key=LampCurrent)
        /// </summary>
        /// <param name="attributeKey"></param>
        /// <returns></returns>
        public string GetMeteringValue(string attributeKey)
        {
            var css = string.Format("[id$='last-values-side-panel-content-tab1'] .slv-meanings-lastvalues-sidepanel-list-item [id$='last-values-side-panel-meaning-{0}'] .slv-meanings-lastvalues-sidepanel-list-item-value", attributeKey);

            return JSUtility.GetElementText(css);
        }

        /// <summary>
        /// Get Metering Time of attribute key (e.g. Lamp current with key=LampCurrent)
        /// </summary>
        /// <param name="attributeKey"></param>
        /// <returns></returns>
        public string GetMeteringTime(string attributeKey)
        {
            var css = string.Format("[id$='last-values-side-panel-content-tab1'] .slv-meanings-lastvalues-sidepanel-list-item [id$='last-values-side-panel-meaning-{0}'] .slv-meanings-lastvalues-sidepanel-list-item-time", attributeKey);

            return JSUtility.GetElementText(css);
        }

        /// <summary>
        /// Get Failure Icon of attribute key (e.g. Lamp current with key=LampCurrent)
        /// </summary>
        /// <param name="attributeKey"></param>
        /// <returns></returns>
        public FailureIcon GetFailureIcon(string attributeKey)
        {
            var css = string.Format("[id$='last-values-side-panel-content-tab2'] .slv-meanings-lastvalues-sidepanel-list-item [id$='last-values-side-panel-meaning-{0}']", attributeKey);
            var failureElement = Driver.FindElement(By.CssSelector(css));

            if (failureElement.FindElements(By.CssSelector(".icon-ok")).Any()) return FailureIcon.OK;
            if (failureElement.FindElements(By.CssSelector(".icon-warning")).Any()) return FailureIcon.Warning;
            if (failureElement.FindElements(By.CssSelector(".icon-error")).Any()) return FailureIcon.Error;

            return FailureIcon.None;
        }

        /// <summary>
        /// Get Failure Time of attribute key (e.g. Lamp current with key=LampCurrent)
        /// </summary>
        /// <param name="attributeKey"></param>
        /// <returns></returns>
        public string GetFailureTime(string attributeKey)
        {
            var css = string.Format("[id$='last-values-side-panel-content-tab2'] .slv-meanings-lastvalues-sidepanel-list-item [id$='last-values-side-panel-meaning-{0}'] .slv-meanings-lastvalues-sidepanel-list-item-time", attributeKey);

            return JSUtility.GetElementText(css);
        }

        /// <summary>
        /// Get all Failures' attributes which have value
        /// </summary>
        /// <returns></returns>
        public List<string> GetFailuresAttributesWithValue()
        {
            return failuresContentsList.Where(e => e.GetAttribute("title") != string.Empty).Select(e => e.Text.SplitAndGetAt(0)).ToList();
        }

        /// <summary>
        /// Get Failures' attribute tooltip
        /// </summary>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public string GetFailuresTooltipAttribute(string attribute)
        {
            return failuresContentsList.Where(e => e.Text.SplitAndGetAt(0) == attribute).Select(e => e.GetAttribute("title")).FirstOrDefault();
        }

        /// <summary>
        /// Get hover Failures' attribute
        /// </summary>
        /// <returns></returns>
        public string GetFailuresHoverAttribute()
        {
            return hoverFailuresAttribute.Text.SplitAndGetAt(0);
        }

        /// <summary>
        /// Hover an Meterings' attribute
        /// </summary>
        /// <param name="attribute"></param>
        public void MoveHoverMeteringsAttribute(string attribute)
        {
            var attrElement = meteringsContentsList.FirstOrDefault(e => e.Text.SplitAndGetAt(0) == attribute);
            attrElement.MoveTo();
        }

        /// <summary>
        /// Hover an Failures' attribute
        /// </summary>
        /// <param name="attribute"></param>
        public void MoveHoverFailuresAttribute(string attribute)
        {
            var attrElement = failuresContentsList.FirstOrDefault(e => e.Text.SplitAndGetAt(0) == attribute);
            attrElement.MoveTo();
        }

        #endregion //Business methods

        public void WaitForPanelClosed()
        {
            Wait.ForElementStyle(By.CssSelector("[id$='last-values-side-panel']"), "left: -350px");
        }

        public override void WaitForPanelLoaded()
        {
            Wait.ForElementsDisplayed(By.CssSelector("[id$='last-values-side-panel-metering-list'] .slv-meanings-lastvalues-sidepanel-list-item-label"));
        }
    }
}
