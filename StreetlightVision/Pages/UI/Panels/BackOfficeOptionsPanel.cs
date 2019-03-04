using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using StreetlightVision.Extensions;
using StreetlightVision.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StreetlightVision.Pages.UI
{
    public class BackOfficeOptionsPanel : PanelBase
    {
        #region Variables        

        #endregion //Variables

        #region IWebElements     

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-configuration'] .backoffice-configuration-title")]
        private IWebElement panelTitle;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-configuration'] .backoffice-configuration-item")]
        private IList<IWebElement> configurationItemsList;

        [FindsBy(How = How.CssSelector, Using = "[id='slv-view-backoffice-configuration'] .backoffice-configuration-item.slv-item-selected")]
        private IWebElement selectedConfigurationItem;

        #endregion //IWebElements

        #region Constructor

        public BackOfficeOptionsPanel(IWebDriver driver, PageBase page)
            : base(driver, page)
        {
            PageFactory.InitElements(this, new RetryingElementLocator(driver, TimeSpan.FromMilliseconds(Settings.LocatorRetryingTimeout), TimeSpan.FromMilliseconds(Settings.LocatorPollingInterval)));

            WaitForPanelLoaded();
        }

        #endregion //Constructor

        #region Properties        

        #endregion

        #region Basic methods     

        #region Action methods

        #endregion //Action methods   

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


        /// <summary>
        /// Select a configuration item with specific name
        /// </summary>
        /// <param name="name"></param>
        public void SelectConfiguration(string name)
        {
            var item = configurationItemsList.FirstOrDefault(p => p.Text.Equals(name));
            if (item != null)
            {
                item.ClickEx();
                Wait.ForElementText(By.CssSelector("[id='slv-view-backoffice-editor-title']"), name);
            }
            else
                throw new Exception(string.Format("'{0}' does not exist", name));
        }

        /// <summary>
        /// Get all configuration item name
        /// </summary>
        /// <returns></returns>
        public List<string> GetConfigurationNameList()
        {
            return JSUtility.GetElementsText("[id='slv-view-backoffice-configuration'] .backoffice-configuration-item-title");
        }

        /// <summary>
        /// Get selected configuration item name
        /// </summary>
        /// <returns></returns>
        public string GetSelectedConfigurationName()
        {
            return selectedConfigurationItem.Text;
        }

        #endregion //Business methods

        public override void WaitForPanelLoaded()
        {
            Wait.ForElementsText(configurationItemsList);
        }

        public override void WaitForPreviousActionComplete()
        {
            base.WaitForPreviousActionComplete();
        }
    }
}
