using ImageMagick;
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
    public class DeviceWidgetPanelBase : PanelBase
    {
        #region Variables

        private Dictionary<RealtimeCommand, CommandModel> _commandsDict;
        private InformationWidgetPanel _informationWidgetPanel;

        #endregion //Variables

        #region IWebElements        

        #region Main form

        #region Header

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] [title='Refresh']")]
        private IWebElement refreshButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] [title='Automatic'], [id$='widgetPanel'] .controller-header button")]
        private IWebElement clockButton;
                
        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] .widget-header > span:last-child, [id$='widgetPanel'] .controller-header button > div:nth-child(2)")]
        private IWebElement clockLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] [title='Sunrise/Sunset times']")]
        private IWebElement sunriseSunsetButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] [title='Information'], [id$='widgetPanel'] [title='INFORMATION']")]
        private IWebElement informationButton;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] [title='Close']")]
        private IWebElement closeButton;

        #endregion //Header

        #region Footer Title

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] .recto-div .widget-bottom-title .device-image, [id$='widgetPanel'] [id$='recto'] [id$='title'] > img")]
        private IWebElement deviceIcon;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] .recto-div .widget-bottom-title .device-name, [id$='widgetPanel'] [id$='recto'] [id$='title'] > span")]
        private IWebElement deviceNameLabel;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] .recto-div .widget-layout-refresh-time, [id$='widgetPanel'] [id$='recto'] [id$='time']")]
        private IWebElement lastUpdateTimeLabel;

        #endregion //Footer Title

        #endregion //Main form

        #region Information form

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] .verso-div .widget-bottom-title img, [id$='widgetPanel'] [id$='verso'] [id$='title'] > img")]
        private IWebElement infomationDeviceIcon;

        [FindsBy(How = How.CssSelector, Using = "[id$='widgetPanel'] .verso-div .widget-bottom-title .device-name, [id$='widgetPanel'] [id$='verso'] [id$='title'] > span")]
        private IWebElement infomationDeviceNameLabel;

        #endregion //Information form

        #endregion //IWebElements

        #region Constructor

        public DeviceWidgetPanelBase(IWebDriver driver, PageBase page) : base(driver, page)
        {
            PageFactory.InitElements(this, new RetryingElementLocator(driver, TimeSpan.FromMilliseconds(Settings.LocatorRetryingTimeout), TimeSpan.FromMilliseconds(Settings.LocatorPollingInterval)));
            InitCommandsDictionary();
        }

        #endregion //Constructor

        #region Properties

        public Dictionary<RealtimeCommand, CommandModel> CommandsDict
        {
            get
            {
                return _commandsDict;
            }

            set
            {
                _commandsDict = value;
            }
        }

        public InformationWidgetPanel InformationWidgetPanel
        {
            get
            {
                if (_informationWidgetPanel == null)
                {
                    _informationWidgetPanel = new InformationWidgetPanel(this.Driver, this.Page);
                }

                return _informationWidgetPanel;
            }
        }

        #endregion //Properties

        #region Basic methods

        #region Actions

        #region Main form

        #region Header

        /// <summary>
        /// Click 'Refresh' button
        /// </summary>
        public void ClickRefreshButton()
        {
            refreshButton.ClickEx();
        }

        /// <summary>
        /// Click 'Clock' button
        /// </summary>
        public void ClickClockButton()
        {
            clockButton.ClickEx();
        }

        /// <summary>
        /// Click 'SunriseSunset' button
        /// </summary>
        public void ClickSunriseSunsetButton()
        {
            sunriseSunsetButton.ClickEx();
        }

        /// <summary>
        /// Click 'Information' button
        /// </summary>
        public void ClickInformationButton()
        {
            informationButton.ClickEx();
        }

        /// <summary>
        /// Click 'Close' button
        /// </summary>
        public void ClickCloseButton()
        {
            closeButton.ClickEx();
        }

        #endregion //Header

        #region Footer Title

        #endregion //Footer Title

        #endregion //Main form

        #region Information form

        #endregion //Information form

        #endregion //Actions

        #region Get methods

        #region Main form

        #region Header

        /// <summary>
        /// Get 'Clock' label text
        /// </summary>
        /// <returns></returns>
        public string GetClockText()
        {
            return clockLabel.Text;
        }

        #endregion //Header

        #region Footer Title

        /// <summary>
        /// Get 'DeviceIcon' input value
        /// </summary>
        /// <returns></returns>
        public string GetDeviceIconValue()
        {
            return deviceIcon.IconValue();
        }

        /// <summary>
        /// Get 'DeviceName' label text
        /// </summary>
        /// <returns></returns>
        public string GetDeviceNameText()
        {
            return deviceNameLabel.Text;
        }

        /// <summary>
        /// Get 'LastUpdateTime' label text
        /// </summary>
        /// <returns></returns>
        public string GetLastUpdateTimeText()
        {
            return lastUpdateTimeLabel.Text;
        }

        #endregion //Footer Title

        #endregion //Main form

        #region Information form

        /// <summary>
        /// Get 'InfomationDeviceIcon' input value
        /// </summary>
        /// <returns></returns>
        public string GetInfomationDeviceIconValue()
        {
            return infomationDeviceIcon.IconValue();
        }

        /// <summary>
        /// Get 'InfomationDeviceName' label text
        /// </summary>
        /// <returns></returns>
        public string GetInfomationDeviceNameText()
        {
            return infomationDeviceNameLabel.Text;
        }

        #endregion //Information form

        #endregion //Get methods

        #endregion //Basic methods

        #region Business methods

        private void InitCommandsDictionary()
        {
            CommandsDict = new Dictionary<RealtimeCommand, CommandModel>();

            CommandsDict.Add(RealtimeCommand.DimOff, new CommandModel { Value = 0, TriangleValue = 170, OffsetX = 0, OffsetY = 170 });
            CommandsDict.Add(RealtimeCommand.Dim10, new CommandModel { Value = 10, TriangleValue = 153, OffsetX = 0, OffsetY = 168 });
            CommandsDict.Add(RealtimeCommand.Dim20, new CommandModel { Value = 20, TriangleValue = 136, OffsetX = 0, OffsetY = 151 });
            CommandsDict.Add(RealtimeCommand.Dim30, new CommandModel { Value = 30, TriangleValue = 119, OffsetX = 0, OffsetY = 134 });
            CommandsDict.Add(RealtimeCommand.Dim40, new CommandModel { Value = 40, TriangleValue = 102, OffsetX = 0, OffsetY = 117 });
            CommandsDict.Add(RealtimeCommand.Dim50, new CommandModel { Value = 50, TriangleValue = 85, OffsetX = 0, OffsetY = 100 });
            CommandsDict.Add(RealtimeCommand.Dim60, new CommandModel { Value = 60, TriangleValue = 68, OffsetX = 0, OffsetY = 83 });
            CommandsDict.Add(RealtimeCommand.Dim70, new CommandModel { Value = 70, TriangleValue = 51, OffsetX = 0, OffsetY = 66 });
            CommandsDict.Add(RealtimeCommand.Dim80, new CommandModel { Value = 80, TriangleValue = 34, OffsetX = 0, OffsetY = 49 });
            CommandsDict.Add(RealtimeCommand.Dim90, new CommandModel { Value = 90, TriangleValue = 17, OffsetX = 0, OffsetY = 32 });
            CommandsDict.Add(RealtimeCommand.DimOn, new CommandModel { Value = 100, TriangleValue = 0, OffsetX = 0, OffsetY = 0 });
        }

        public RealtimeCommand GetCommandByText(string text)
        {
            return CommandsDict.FirstOrDefault(p => p.Value.Text.Equals(text)).Key;
        }

        public virtual void WaitForCommandCompleted()
        {

        }        

        public bool IsRefreshButtonVisible()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='widgetPanel'] [title='Refresh']"));
        }

        public bool IsClockButtonVisible()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='widgetPanel'] [title='Automatic'], [id$='widgetPanel'] [id$='wholeSegment'] button .icon-clock"));
        }

        public bool IsSunriseSunsetButtonVisible()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='widgetPanel'] [title='Sunrise/Sunset times']"));
        }

        public bool IsInformationButtonVisible()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='widgetPanel'] [title='Information'], [id$='widgetPanel'] [title='INFORMATION']"));
        }

        public bool IsCloseButtonVisible()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='widgetPanel'] [title='Close']"));
        }        

        public List<string> GetListOfCommandsText()
        {
            return JSUtility.GetElementsText("button[class$='command-button']:not(.icon-play)");
        }

        public virtual bool IsIndicatorCursorVisible()
        {
            return ElementUtility.IsDisplayed(By.CssSelector("[id$='widgetPanel'] [id$='jauge-cursor']"));
        }

        public byte[] GetDeviceIconBytes()
        {
            var url = deviceIcon.GetAttribute("src");

            return SLVHelper.DownloadFileData(url);
        }  

        public bool CheckIfDeviceIcon(DeviceType device)
        {
            var expectedIcon = new MagickImage(device.GetIconBytes());
            var actualBytes = GetDeviceIconBytes();
            if (actualBytes.Length == 0) return false;
            var actualIcon = new MagickImage(actualBytes);
            var result = expectedIcon.Compare(actualIcon, ErrorMetric.Absolute);

            return result == 0;
        }

        #endregion //Business methods

        public override void WaitForPanelLoaded()
        {
            Wait.ForElementInvisible(By.CssSelector("[id$='widgetPanel'] [id$='loader-spin']"));
        }

        public override void WaitForPreviousActionComplete()
        {
            base.WaitForPreviousActionComplete();
        }
    }
}
