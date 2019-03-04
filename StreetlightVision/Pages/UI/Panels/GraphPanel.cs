using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using StreetlightVision.Extensions;
using StreetlightVision.Utilities;

namespace StreetlightVision.Pages.UI
{
    public class GraphPanel : PanelBase
    {
        #region Variables

        #endregion //Variables

        #region IWebElements

        [FindsBy(How = How.CssSelector, Using = "[id$='window-meanings-historycharts-container-charts'] .slv-meanings-historychart")]
        private IList<IWebElement> historyChartsList;

        [FindsBy(How = How.CssSelector, Using = "[id*='window-meanings-historycharts-container'][id$='title'] .slv-label.slv-meanings-historychart-title-label")]
        private IList<IWebElement> historyChartLabelList;

        [FindsBy(How = How.CssSelector, Using = "[id$='window-meanings-historycharts-container-charts']")]
        private IWebElement historyChartContainer;

        [FindsBy(How = How.CssSelector, Using = "[id='desktop-header-bartop']")]
        private IWebElement fakeHstoryChartContainer;        

        #endregion //IWebElements

        #region Constructor

        public GraphPanel(IWebDriver driver, PageBase page) : base(driver, page)
        {
            PageFactory.InitElements(this, new RetryingElementLocator(driver, TimeSpan.FromMilliseconds(Settings.LocatorRetryingTimeout), TimeSpan.FromMilliseconds(Settings.LocatorPollingInterval)));

            WaitForPanelLoaded();
        }

        #endregion //Constructor

        #region Basic methods

        #region Actions

        /// <summary>
        /// Click Export button of a device's historychart
        /// </summary>
        /// <param name="device"></param>
        public void ClickExportFromGraph(string device)
        {
            foreach (var chart in historyChartsList)
            {
                var title = chart.FindElement(By.CssSelector(".slv-label.slv-meanings-historychart-title-label"));
                if (title.Text.Equals(device))
                {
                    var exportBtn = chart.FindElement(By.CssSelector("[id^='tb'][id$='toolbar_item_export'] table.w2ui-button"));
                    chart.ScrollToElementByJS();
                    exportBtn.ClickEx();
                    break;
                }
            }
        }

        /// <summary>
        /// Click Close button of a device's historychart
        /// </summary>
        /// <param name="device"></param>
        public void ClickCloseGraph(string device)
        {
            foreach (var chart in historyChartsList)
            {
                var title = chart.FindElement(By.CssSelector(".slv-label.slv-meanings-historychart-title-label"));
                if (title.Text.Equals(device))
                {
                    var closeBtn = chart.FindElement(By.CssSelector("[id^='tb'][id$='toolbar_item_close'] table.w2ui-button"));
                    closeBtn.ClickEx();
                    break;
                }
            }
        }
        
        public void CloseValueFromGraph(string device, string value)
        {
            foreach (var chart in historyChartsList)
            {
                var title = chart.FindElement(By.CssSelector(".slv-label.slv-meanings-historychart-title-label"));
                if (title.Text.Equals(device))
                {
                    var attributes = chart.FindElements(By.CssSelector("[id$='chart-legend'] .slv-meanings-historychart-chart-legend-line"));
                    var attribute = attributes.FirstOrDefault(p => p.Text.Trim().Equals(value));
                    if (attribute != null)
                    {
                        attribute.MoveTo();
                        var closeBtn = attribute.FindElement(By.CssSelector(".slv-meanings-historychart-chart-legend-button"));
                        closeBtn.ClickEx();
                        break;
                    }                    
                }
            }
        }

        #endregion //Actions

        #region Get methods
        
        /// <summary>
        /// Get selected devices from historychart
        /// </summary>
        /// <returns></returns>
        public List<string> GetSelectedDevices()
        {
            if (historyChartContainer == null || !historyChartContainer.Displayed)
                return null;
            return historyChartLabelList.Select(e => e.Text).ToList();
        }

        /// <summary>
        /// Get selected value of current device from historychart
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        public List<string> GetSelectedValues(string device)
        {
            if (historyChartContainer == null || !historyChartContainer.Displayed)
                return null;
            foreach (var chart in historyChartsList)
            {
                var title = chart.FindElement(By.CssSelector(".slv-label.slv-meanings-historychart-title-label"));
                if (title.Text.Equals(device))
                {
                    var values = chart.FindElements(By.CssSelector("[id$='chart-legend'] .slv-meanings-historychart-chart-legend-label"));
                    return values.Select(e => e.Text).ToList();
                }
            }
            return null;
        }

        /// <summary>
        /// Get all current opened graphs
        /// </summary>
        /// <returns></returns>
        public IList<IWebElement> GetGraphs()
        {
            if (historyChartContainer == null || !historyChartContainer.Displayed)
                return null;
            return historyChartsList;            
        }

        /// <summary>
        /// Get all current opened graphs
        /// </summary>
        /// <returns></returns>
        public IWebElement GetGraph(string device)
        {
            if (historyChartContainer == null || !historyChartContainer.Displayed)
                return null;

            foreach (var chart in historyChartsList)
            {
                var title = chart.FindElement(By.CssSelector(".slv-label.slv-meanings-historychart-title-label"));
                return chart;
            }
            return null;
        }

        #endregion //Get methods

        #endregion //Basic methods

        #region Business methods

        public byte[] SaveChartsAsBytes()
        {
            if (historyChartContainer == null || !historyChartContainer.Displayed)
                return fakeHstoryChartContainer.TakeScreenshotAsBytes();

            return historyChartContainer.TakeScreenshotAsBytes();
        }

        /// <summary>
        /// Check if panel is visible
        /// </summary>
        /// <returns></returns>
        public bool IsPanelVisible()
        {
            foreach (var label in historyChartLabelList)
            {
                if (!label.Displayed)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Scroll to expected chart
        /// </summary>
        /// <param name="device"></param>
        public void ScrollToChart(string device)
        {
            if (historyChartContainer == null || !historyChartContainer.Displayed)
                return;
            foreach (var chart in historyChartsList)
            {
                var title = chart.FindElement(By.CssSelector(".slv-label.slv-meanings-historychart-title-label"));
                if (title.Text.Equals(device))
                {
                    chart.ScrollToElementByJS();
                    return;
                }
            }
        }

        /// <summary>
        /// Close all graphs
        /// </summary>
        /// <param name="device"></param>
        public void CloseAllGraphs()
        {
            foreach (var chart in historyChartsList)
            {
                var closeBtn = chart.FindElement(By.CssSelector("[id^='tb'][id$='toolbar_item_close'] table.w2ui-button"));
                closeBtn.ClickEx();
                WaitForPreviousActionComplete();
            }
            Wait.ForElementStyle(By.CssSelector("[id$='historycharts-container_panel_main']"), "height: 0px");
            Wait.ForElementInvisible(By.CssSelector("[id$='historychart-toolbar-fromto']"));
        }

        public bool IsChartHasData(string device)
        {
            foreach (var chart in historyChartsList)
            {
                var title = chart.FindElement(By.CssSelector(".slv-label.slv-meanings-historychart-title-label"));
                if (title.Text.Equals(device))
                {
                    return chart.FindElements(By.CssSelector(".slv-meanings-historychart-chart-content svg path.path")).Any();
                }
            }

            return false;
        }

        public int GetChartGraphPointsCount(string device)
        {
            foreach (var chart in historyChartsList)
            {
                var title = chart.FindElement(By.CssSelector(".slv-label.slv-meanings-historychart-title-label"));
                if (title.Text.Equals(device))
                {
                    var pathD = chart.FindElement(By.CssSelector(".slv-meanings-historychart-chart-content svg path.path")).GetAttribute("d");
                    pathD = pathD.Replace("M0,", string.Empty).Replace("M 0 ", string.Empty);
                    var points = pathD.SplitEx(new string[] {"L"});

                    return points.Count / 2;
                }
            }

            return 0;
        }

        #endregion //Business methods

        public override void WaitForPanelLoaded()
        {
            Wait.ForElementsDisplayed(historyChartLabelList);

        }
    }
}
