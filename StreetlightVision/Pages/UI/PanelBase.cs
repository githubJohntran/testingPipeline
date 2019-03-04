using OpenQA.Selenium;
using StreetlightVision.Extensions;

namespace StreetlightVision.Pages.UI
{
    public abstract class PanelBase
    {
        #region Variables

        private IWebDriver _driver;
        private PageBase _page;

        #endregion //Variables

        #region IWebElements

        #endregion //IWebElements

        #region Constructor

        public PanelBase(IWebDriver driver, PageBase page)
        {
            this._driver = driver;
            this._page = page;
        }

        #endregion //Constructor

        #region Properties

        public IWebDriver Driver
        {
            get
            {
                return _driver;
            }
        }

        public PageBase Page
        {
            get { return _page; }
        }

        #endregion //Properties

        #region Basic methods

        #region Actions

        #endregion //Actions

        #region Get methods

        #endregion //Get methods

        #endregion //Basic methods

        #region Business methods

        #endregion //Business methods        

        public virtual void WaitForPanelLoaded()
        {
        }

        public virtual void WaitForPreviousActionComplete()
        {
            try
            {
                Wait.ForLoadingIconDisappeared();
                Wait.ForProgressCompleted();
            }
            catch (UnhandledAlertException)
            {
                Wait.ForSeconds(2);
            }
        }
    }
}
