using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using StreetlightVision.Extensions;
using StreetlightVision.Models;
using StreetlightVision.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace StreetlightVision.Pages.UI
{
    public class CabinetControllerWidgetPanel : DeviceWidgetPanelBase
    {
        #region Variables        

        #endregion //Variables

        #region IWebElements        
        

        #endregion //IWebElements

        #region Constructor

        public CabinetControllerWidgetPanel(IWebDriver driver, PageBase page) : base(driver, page)
        {
            PageFactory.InitElements(this, new RetryingElementLocator(driver, TimeSpan.FromMilliseconds(Settings.LocatorRetryingTimeout), TimeSpan.FromMilliseconds(Settings.LocatorPollingInterval)));

            WaitForPanelLoaded();
        }

        #endregion //Constructor

        #region Properties
        
        #endregion //Properties

        #region Basic methods

        #region Actions
        
        #endregion //Actions

        #region Get methods
        
        #endregion //Get methods

        #endregion //Basic methods

        #region Business methods        

        public void WaitForNameText(string name)
        {
            Wait.ForElementInvisible(By.CssSelector("[id$='widgetPanel'] [id$='loader-spin']"));
            Wait.ForElementText(By.CssSelector("[id$='widgetPanel'] .cabinetcontroller [id$='recto'] .device-name"), name);
        }

        public void WaitForInformationWidgetPanelDisplayed()
        {
            Wait.ForElementInvisible(By.CssSelector("[id$='widgetPanel'] [id$='recto']"));
            Wait.ForElementDisplayed(By.CssSelector("[id$='widgetPanel'] [id$='verso']"));
            Wait.ForMilliseconds(500);
        }

        public void WaitInformationWidgetPanelDisappeared()
        {
            Wait.ForElementDisplayed(By.CssSelector("[id$='widgetPanel'] [id$='recto']"));
            Wait.ForElementInvisible(By.CssSelector("[id$='widgetPanel'] [id$='verso']"));
            Wait.ForMilliseconds(500);
        }

        #endregion //Business methods

        public override void WaitForCommandCompleted()
        {
            Wait.ForProgressCompleted();
            Wait.ForSeconds(1);
        }

        public override void WaitForPanelLoaded()
        {
            Wait.ForElementInvisible(By.CssSelector("[id$='widgetPanel'] [id$='loader-spin']"));
        }
    }
}
