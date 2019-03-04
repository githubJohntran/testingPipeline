using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using System;
using StreetlightVision.Pages.UI;
using System.Collections.Generic;
using System.Linq;
using StreetlightVision.Extensions;

namespace StreetlightVision.Pages
{
    public class BackOfficePage : PageBase
    {
        #region Variables

        private BackOfficeOptionsPanel _backOfficeOptionsPanel;      
        private BackOfficeDetailsPanel _backOfficeDetailsPanel;

        #endregion //Variables

        #region IWebElements        

        #endregion //IWebElements

        #region Constructor

        public BackOfficePage(IWebDriver driver)
            : base(driver)                                                                                                                                                                                                                                                                                                                                                                             
        {
            PageFactory.InitElements(this, new RetryingElementLocator(driver, TimeSpan.FromMilliseconds(Settings.LocatorRetryingTimeout), TimeSpan.FromMilliseconds(Settings.LocatorPollingInterval)));
           
            WaitForPageReady();
        }

        #endregion //Constructor

        #region Properties

        public BackOfficeOptionsPanel BackOfficeOptionsPanel
        {
            get
            {
                if (_backOfficeOptionsPanel == null)
                {
                    _backOfficeOptionsPanel = new BackOfficeOptionsPanel(this.Driver, this);
                }

                return _backOfficeOptionsPanel;
            }
        }        

        public BackOfficeDetailsPanel BackOfficeDetailsPanel
        {
            get
            {
                if (_backOfficeDetailsPanel == null)
                {
                    _backOfficeDetailsPanel = new BackOfficeDetailsPanel(this.Driver, this);
                }

                return _backOfficeDetailsPanel;
            }            
        }

        public readonly Dictionary<string, string>  AttributesDictionary = new Dictionary<string, string>()
        {
            { "name", "Name"},
            { "idOnController", "Identifier"},
            { "controllerStrId", "Controller ID"},
            { "categoryStrId", "Category"},
            { "MacAddress", "Unique address"},
            { "DimmingGroupName", "Dimming group"},
            { "installStatus", "Install status"},
            { "ConfigStatus", "Configuration status"}, //with special character "space"
            { "address", "Address 1"},
            { "location.streetdescription", "Address 2"},
            { "location.city", "City"},
            { "location.zipcode", "Zip code"},
            { "client.name", "Customer name"},
            { "client.number", "Customer number"},
            { "luminaire.model", "Luminaire model"},
            { "power", "Lamp wattage (W)"},            
            { "SoftwareVersion", "Software version"},
            { "comment", "Comment"}
        };

        #endregion //Properties

        #region Basic methods

        #region Actions        

        #endregion //Actions

        #region Get methods

        #endregion //Get methods

        #endregion //Basic methods

        #region Business methods   
        
        public List<string> GetAttributesKey()
        {
            return AttributesDictionary.Keys.ToList();
        }

        public List<string> GetAttributesName(params string[] attributesKey)
        {
            var result = new List<string>();
            foreach (var key in attributesKey)
            {
                result.Add(AttributesDictionary[key]);
            }

            return result;
        }

        #endregion //Business methods

        protected override void WaitForPageReady()
        {
            Wait.ForElementStyle(By.Id("slv_preloader"), "display: none");
            base.WaitForPageReady();
            BackOfficeOptionsPanel.WaitForPanelLoaded();
        }        
    }
}
