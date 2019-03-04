using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using StreetlightVision.Extensions;

namespace StreetlightVision.Pages.UI
{
    public class PhotoPanel : PanelBase
    {
        #region Variables

        #endregion //Variables

        #region IWebElements

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-device-photo-backButton']")]
        private IWebElement backButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-device-photo-title'] .equipmentgl-editor-title-label")]
        private IWebElement panelTitle;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-device-photo-content'] > div.slv-snapshot-header > div")]
        private IWebElement takePhotoLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='editor-device-photo-content'] > div.slv-snapshot-header > button")]
        private IWebElement cameraButton;
       
        [FindsBy(How = How.CssSelector, Using = "[id$='editor-device-photo-content'] > div.slv-snapshot-viewbox")]
        private IWebElement viewboxImage;

        #endregion //IWebElements

        #region Constructor

        public PhotoPanel(IWebDriver driver, PageBase page)
            : base(driver, page)
        { }

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
        /// Click 'Camera' button
        /// </summary>
        public void ClickCameraButton()
        {
            cameraButton.ClickEx();
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

        /// <summary>
        /// Get 'TakePhoto' label text
        /// </summary>
        /// <returns></returns>
        public string GetTakePhotoText()
        {
            return takePhotoLabel.Text;
        }

        /// <summary>
        /// Get 'ViewboxImage' input value
        /// </summary>
        /// <returns></returns>
        public string GetViewboxImageValue()
        {
            return viewboxImage.ImageValue();
        }

        #endregion //Get methods

        #endregion //Basic methods

        #region Business methods

        #endregion //Business methods
    }
}
