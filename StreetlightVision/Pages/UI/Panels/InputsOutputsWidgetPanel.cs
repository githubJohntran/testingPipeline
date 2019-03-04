using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using StreetlightVision.Extensions;
using System;

namespace StreetlightVision.Pages.UI
{
    public class InputsOutputsWidgetPanel : PanelBase
    {
        #region Variables

        #endregion //Variables

        #region IWebElements        

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] [id$='status-panel'] [id$='status-tab'] > div")]
        private IWebElement inputsOutputssLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] [id$='status-panel'] [id$='status-content'] [id$='inputs'] .controller-io-panel:nth-child(1) > div.slv-label")]
        private IWebElement input1Label;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] [id$='status-panel'] [id$='status-content'] [id$='inputs'] [id$='inputState1']")]
        private IWebElement input1State;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] [id$='status-panel'] [id$='status-content'] [id$='inputs'] .controller-io-panel:nth-child(2) > div.slv-label")]
        private IWebElement input2Label;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] [id$='status-panel'] [id$='status-content'] [id$='inputs'] [id$='inputState2']")]
        private IWebElement input2State;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] [id$='status-panel'] [id$='status-content'] [id$='outputs'] .controller-io-panel:nth-child(1) > div.slv-label")]
        private IWebElement output1Label;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] [id$='status-panel'] [id$='status-content'] [id$='outputs'] [id$='outputONButton1']")]
        private IWebElement output1OnButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] [id$='status-panel'] [id$='status-content'] [id$='outputs'] [id$='outputOFFButton1']")]
        private IWebElement output1OffButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] [id$='status-panel'] [id$='status-content'] [id$='outputs'] [id$='outputMode1']")]
        private IWebElement output1ModeLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] [id$='status-panel'] [id$='status-content'] [id$='outputs'] [id$='outputState1']")]
        private IWebElement output1State;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] [id$='status-panel'] [id$='status-content'] [id$='outputs'] .controller-io-panel:nth-child(2) > div.slv-label")]
        private IWebElement output2Label;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] [id$='status-panel'] [id$='status-content'] [id$='outputs'] [id$='outputState2']")]
        private IWebElement output2State;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] [id$='status-panel'] [id$='status-content'] [id$='outputs'] [id$='outputONButton2']")]
        private IWebElement output2OnButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] [id$='status-panel'] [id$='status-content'] [id$='outputs'] [id$='outputOFFButton2']")]
        private IWebElement output2OffButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] [id$='status-panel'] [id$='status-content'] [id$='outputs'] [id$='outputMode2']")]
        private IWebElement output2ModeLabel;

        #endregion //IWebElements

        #region Constructor

        public InputsOutputsWidgetPanel(IWebDriver driver, PageBase page)
            : base(driver, page)
        {
            PageFactory.InitElements(this, new RetryingElementLocator(driver, TimeSpan.FromMilliseconds(Settings.LocatorRetryingTimeout), TimeSpan.FromMilliseconds(Settings.LocatorPollingInterval)));
            WaitForPanelLoaded();
        }

        #endregion //Constructor

        #region Basic methods

        #region Actions

        /// <summary>
        /// Click 'Output1On' button
        /// </summary>
        public void ClickOutput1OnButton()
        {
            output1OnButton.ClickEx();
        }

        /// <summary>
        /// Click 'Output1Off' button
        /// </summary>
        public void ClickOutput1OffButton()
        {
            output1OffButton.ClickEx();
        }

        /// <summary>
        /// Click 'Output2On' button
        /// </summary>
        public void ClickOutput2OnButton()
        {
            output2OnButton.ClickEx();
        }

        /// <summary>
        /// Click 'Output2Off' button
        /// </summary>
        public void ClickOutput2OffButton()
        {
            output2OffButton.ClickEx();
        }

        #endregion //Actions

        #region Get methods

        /// <summary>
        /// Get 'InputsOutputss' label text
        /// </summary>
        /// <returns></returns>
        public string GetInputsOutputssText()
        {
            return inputsOutputssLabel.Text;
        }

        /// <summary>
        /// Get 'Input1' label text
        /// </summary>
        /// <returns></returns>
        public string GetInput1Text()
        {
            return input1Label.Text;
        }

        /// <summary>
        /// Get 'Input2' label text
        /// </summary>
        /// <returns></returns>
        public string GetInput2Text()
        {
            return input2Label.Text;
        }

        /// <summary>
        /// Get 'Output1' label text
        /// </summary>
        /// <returns></returns>
        public string GetOutput1Text()
        {
            return output1Label.Text;
        }

        /// <summary>
        /// Get 'Output1Mode' label text
        /// </summary>
        /// <returns></returns>
        public string GetOutput1ModeText()
        {
            return output1ModeLabel.Text;
        }

        /// <summary>
        /// Get 'Output2' label text
        /// </summary>
        /// <returns></returns>
        public string GetOutput2Text()
        {
            return output2Label.Text;
        }

        /// <summary>
        /// Get 'Output2Mode' label text
        /// </summary>
        /// <returns></returns>
        public string GetOutput2ModeText()
        {
            return output2ModeLabel.Text;
        }

        #endregion //Get methods

        #endregion //Basic methods

        #region Business methods
        
        public bool IsOutput1StatusGREEN()
        {
            return output1State.GetAttribute("class").Contains("controller-led-on");
        }

        public bool IsOutput1StatusGRAY()
        {
            return output1State.GetAttribute("class").Contains("controller-led-off");
        }

        public bool IsOutput2StatusGREEN()
        {
            return output2State.GetAttribute("class").Contains("controller-led-on");
        }

        public bool IsOutput2StatusGRAY()
        {
            return output2State.GetAttribute("class").Contains("controller-led-off");
        }

        #endregion //Business methods

    }
}
