using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using StreetlightVision.Extensions;
using StreetlightVision.Utilities;
using System;

namespace StreetlightVision.Pages.UI
{
    public class NodeEditorPanelBase : PanelBase
    {
        #region Variables

        private ReplaceNodePanel _replaceNodePanel;

        #endregion //Variables

        #region IWebElements        

        #endregion //IWebElements

        #region Constructor

        public NodeEditorPanelBase(IWebDriver driver, PageBase page) : base(driver, page)
        {
            PageFactory.InitElements(this, new RetryingElementLocator(driver, TimeSpan.FromMilliseconds(Settings.LocatorRetryingTimeout), TimeSpan.FromMilliseconds(Settings.LocatorPollingInterval)));
        }

        #endregion //Constructor

        #region Properties

        public ReplaceNodePanel ReplaceNodePanel
        {
            get
            {
                if (_replaceNodePanel == null)
                {
                    _replaceNodePanel = new ReplaceNodePanel(this.Driver, this.Page);
                }

                return _replaceNodePanel;
            }
        }

        #endregion //Properties

        #region Business methods
        
        public virtual void WaitForReplaceNodePanelDisplayed()
        {
            Wait.ForElementDisplayed(By.CssSelector("[id$='editor-device-replaceNode']"));
            Wait.ForElementStyle(By.CssSelector("[id$='editor-device-replaceNode']"), "left: 0px");
        }

        public virtual void WaitForReplaceNodePanelDisappeared()
        {
            Wait.ForElementInvisible(By.CssSelector("[id$='editor-device-replaceNode']"));
            Wait.ForElementStyle(By.CssSelector("[id$='editor-device-replaceNode']"), "left: 350px");
        }

        public bool IsDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor'].slv-rounded-control"));
        }

        public bool IsReplaceNodePanelDisplayed()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='editor-device-replaceNode']"));
        }

        #endregion //Business methods
    }
}
