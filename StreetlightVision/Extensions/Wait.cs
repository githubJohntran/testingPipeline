using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;
using StreetlightVision.Models;
using StreetlightVision.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;

namespace StreetlightVision.Extensions
{
    public static class Wait
    {
        #region General Wait For methods        

        /// <summary>
        /// Wait for element displayed
        /// </summary>
        public static void ForElementDisplayed(IWebElement element)
        {
            try
            {
                WebDriverContext.Wait.Until(driver => element.Displayed);
            }
            catch (UnhandledAlertException)
            {
                SLVHelper.AllowSecurityAlert();
            }
        }

        /// <summary>
        ///  Wait for element displayed
        /// </summary>
        /// <param name="element"></param>
        public static void ForElementDisplayed(By elementBy)
        {
            try
            {
                WebDriverContext.Wait.Until(ExpectedConditions.ElementIsVisible(elementBy));
            }
            catch (UnhandledAlertException)
            {
                SLVHelper.AllowSecurityAlert();
            }
        }

        /// <summary>
        /// Wait for elements displayed
        /// </summary>
        /// <param name="elements"></param>
        public static void ForElementsDisplayed(IList<IWebElement> elements)
        {
            try
            {
                WebDriverContext.Wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(new ReadOnlyCollection<IWebElement>(elements)));
            }
            catch (UnhandledAlertException)
            {
                SLVHelper.AllowSecurityAlert();
            }
        }

        /// <summary>
        /// Wait for elements displayed
        /// </summary>
        /// <param name="elementBy"></param>
        public static void ForElementsDisplayed(By elementBy)
        {
            try
            {
                WebDriverContext.Wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(elementBy));                
            }
            catch (UnhandledAlertException)
            {
                SLVHelper.AllowSecurityAlert();
            }
        }

        /// <summary>
        /// Wait for elements invisible
        /// </summary>
        /// <param name="elementBy"></param>
        public static void ForElementsInvisible(By elementBy)
        {
            try
            {
                WebDriverContext.Wait.Until(ExpectedConditions.InvisibilityOfElementLocated(elementBy));
            }
            catch (UnhandledAlertException)
            {
                SLVHelper.AllowSecurityAlert();
            }
        }

        /// <summary>
        ///  Wait for element exists
        /// </summary>
        /// <param name="element"></param>
        public static void ForElementExists(By elementBy)
        {
            try
            {
                WebDriverContext.Wait.Until(ExpectedConditions.ElementExists(elementBy));
            }
            catch (UnhandledAlertException)
            {
                SLVHelper.AllowSecurityAlert();
            }
        }

        /// <summary>
        /// Wait for element invisible
        /// </summary>
        public static void ForElementInvisible(By elementBy)
        {
            try
            {
                WebDriverContext.Wait.Until(ExpectedConditions.InvisibilityOfElementLocated(elementBy));
            }
            catch (UnhandledAlertException)
            {
                SLVHelper.AllowSecurityAlert();
            }
        }

        /// <summary>
        /// Wait for element has any text
        /// </summary>
        /// <param name="element"></param>
        public static void ForElementText(IWebElement element)
        {
            WebDriverContext.Wait.Until(driver =>
            {
                try
                {
                    return !string.IsNullOrEmpty(element.Text);
                }               
                catch (StaleElementReferenceException)
                {
                    return false;
                }
                catch (NoSuchElementException)
                {
                    return true;
                }
                catch (UnhandledAlertException)
                {
                    SLVHelper.AllowSecurityAlert();
                    return true;
                }
            });
        }

        public static void ForChildElementDisplayed(this IWebElement element, By childElementBy)
        {
            WebDriverContext.Wait.Until(driver =>
            {
                try
                {
                    return element.FindElements(childElementBy).Any();
                }
                catch (StaleElementReferenceException)
                {
                    return false;
                }
                catch (NoSuchElementException)
                {
                    return true;
                }
                catch (UnhandledAlertException)
                {
                    SLVHelper.AllowSecurityAlert();
                    return true;
                }
            });
        }

        /// <summary>
        /// Wait for element has any text
        /// </summary>
        public static void ForElementText(By elementBy)
        {
            WebDriverContext.Wait.Until(driver =>
            {
                try
                {
                    return !string.IsNullOrEmpty(driver.FindElement(elementBy).Text);
                }
                catch (StaleElementReferenceException)
                {
                    return false;
                }
                catch (NoSuchElementException)
                {
                    return true;
                }
                catch (UnhandledAlertException)
                {
                    SLVHelper.AllowSecurityAlert();
                    return true;
                }
            });
        }

        /// <summary>
        /// Wait for elements list has any text
        /// </summary>
        /// <param name="elements"></param>
        public static void ForElementsText(IList<IWebElement> elements)
        {
            WebDriverContext.Wait.Until(driver =>
            {
                try
                {
                    return elements.All(el => !string.IsNullOrEmpty(el.Text));
                }
                catch (StaleElementReferenceException)
                {
                    return false;
                }
                catch (NoSuchElementException)
                {
                    return true;
                }
                catch (UnhandledAlertException)
                {
                    SLVHelper.AllowSecurityAlert();
                    return true;
                }
            });
        }

        /// <summary>
        /// Wait for element has specific text
        /// </summary>
        public static void ForElementText(IWebElement element, string text)
        {
            try
            {
                WebDriverContext.Wait.Until(ExpectedConditions.TextToBePresentInElement(element, text));
            }
            catch (UnhandledAlertException)
            {
                SLVHelper.AllowSecurityAlert();
            }
        }

        /// <summary>
        /// Wait for element has specific text
        /// </summary>
        public static void ForElementText(By elementBy, string text)
        {
            try
            {
                WebDriverContext.Wait.Until(ExpectedConditions.TextToBePresentInElementLocated(elementBy, text));
            }
            catch (UnhandledAlertException)
            {
                SLVHelper.AllowSecurityAlert();
            }
        }

        /// <summary>
        /// Wait for element has value
        /// </summary>
        /// <param name="element"></param>
        public static void ForElementValue(IWebElement element)
        {
            WebDriverContext.Wait.Until(driver =>
            {
                try
                {
                    return !string.IsNullOrEmpty(element.Value());
                }
                catch (StaleElementReferenceException)
                {
                    return false;
                }
                catch (NoSuchElementException)
                {
                    return true;
                }
                catch (UnhandledAlertException)
                {
                    SLVHelper.AllowSecurityAlert();
                    return true;
                }
            });
        }

        /// <summary>
        /// Wait for element has value
        /// </summary>
        /// <param name="elementBy"></param>
        public static void ForElementValue(By elementBy)
        {
            WebDriverContext.Wait.Until(driver =>
            {
                try
                {
                    return !string.IsNullOrEmpty(driver.FindElement(elementBy).Value());
                }
                catch (StaleElementReferenceException)
                {
                    return false;
                }
                catch (NoSuchElementException)
                {
                    return true;
                }
                catch (UnhandledAlertException)
                {
                    SLVHelper.AllowSecurityAlert();
                    return true;
                }
            });
        }

        /// <summary>
        /// Wait for element has value
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public static void ForElementValue(IWebElement element, string value)
        {
            WebDriverContext.Wait.Until(driver =>
            {
                try
                {
                    return value.Equals(element.Value());
                }
                catch (StaleElementReferenceException)
                {
                    return false;
                }
                catch (NoSuchElementException)
                {
                    return true;
                }
                catch (UnhandledAlertException)
                {
                    SLVHelper.AllowSecurityAlert();
                    return true;
                }
            });
        }

        /// <summary>
        /// Wait for element has value
        /// </summary>
        /// <param name="elementBy"></param>
        /// <param name="value"></param>
        public static void ForElementValue(By elementBy, string value)
        {
            WebDriverContext.Wait.Until(driver =>
            {
                try
                {
                    return value.Equals(driver.FindElement(elementBy).Value());
                }
                catch (StaleElementReferenceException)
                {
                    return false;
                }
                catch (NoSuchElementException)
                {
                    return true;
                }
                catch (UnhandledAlertException)
                {
                    SLVHelper.AllowSecurityAlert();
                    return true;
                }
            });
        }

        /// <summary>
        ///  Wait for element has specific style (e.g. opacity: 1;)
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public static void ForElementStyle(IWebElement element, string value)
        {
            WebDriverContext.Wait.Until(driver =>
            {
                try
                {
                    return element.GetAttribute("style").Contains(value);
                }
                catch (StaleElementReferenceException)
                {
                    return false;
                }
                catch (UnhandledAlertException)
                {
                    SLVHelper.AllowSecurityAlert();
                    return true;
                }
            });
        }

        /// <summary>
        ///  Wait for element has specific style (e.g. opacity: 1;)
        /// </summary>
        /// <param name="elementBy"></param>
        /// <param name="value"></param>
        public static void ForElementStyle(By elementBy, string value)
        {
            WebDriverContext.Wait.Until(driver =>
            {
                try
                {
                    return driver.FindElement(elementBy).GetAttribute("style").Contains(value);
                }
                catch (StaleElementReferenceException)
                {
                    return false;
                }
                catch (UnhandledAlertException)
                {
                    SLVHelper.AllowSecurityAlert();
                    return true;
                }                
            });
        }

        /// <summary>
        /// Wait for minutes
        /// </summary>
        /// <param name="value"></param>
        public static void ForMinutes(int value)
        {
            Thread.Sleep(value * 60 * 1000);
        }

        /// <summary>
        /// Wait for seconds
        /// </summary>
        /// <param name="value"></param>
        public static void ForSeconds(int value)
        {
            Thread.Sleep(value * 1000);
        }

        /// <summary>
        /// Wait for Milliseconds
        /// </summary>
        /// <param name="value"></param>
        public static void ForMilliseconds(int value)
        {
            Thread.Sleep(value);
        }

        /// <summary>
        /// Wait for TimeSpan
        /// </summary>
        /// <param name="value"></param>
        public static void ForTimeSpan(TimeSpan value)
        {
            Thread.Sleep(value);
        }

        #endregion //General Wait For methods

        #region Extensive Wait For methods (Specific waits for each page)

        /// <summary>
        /// Wait for charts 
        /// </summary>
        public static void ForChartsCompletelyDrawn()
        {
            Wait.ForSeconds(2);
        }

        /// <summary>
        /// Wait for progress top bar completely
        /// </summary>
        public static void ForProgressCompleted()
        {
            WebDriverContext.Wait.Until(driver =>
            {
                try
                {
                    ForElementInvisible(By.CssSelector("[id='nprogress']"));
                    return true;
                }
                catch
                {
                    return false;
                }
            });
        }

        /// <summary>
        /// Wait for loading icon disappeared
        /// </summary>
        public static void ForLoadingIconDisappeared()
        {
            WebDriverContext.Wait.Until(driver =>
            {
                try
                {
                    ForElementInvisible(By.CssSelector("[id='slv-loader']"));
                    return true;
                }
                catch
                {
                    return false;
                }
            });
        }

        public static void ForGLMapStopFlying()
        {
            WebDriverContext.Wait.Until(driver =>
            {
                try
                {
                    var mapScaleText = string.Empty;
                    var mapGLScaleLabel = WebDriverContext.CurrentDriver.FindElement(By.CssSelector("div.mapboxgl-map div.mapboxgl-ctrl-scale"));

                    while (true)
                    {
                        mapScaleText = mapGLScaleLabel.Text;
                        ForMilliseconds(200);
                        if (mapScaleText == mapGLScaleLabel.Text)
                        {
                            mapScaleText = mapGLScaleLabel.Text;
                            ForMilliseconds(200);
                            if (mapScaleText == mapGLScaleLabel.Text)
                            {
                                return true;
                            }
                        }
                    }
                }
                catch (UnhandledAlertException)
                {
                    ForSeconds(2);
                    return true;
                }
            });
        }

        public static void ForGLMapZoomTo(ZoomGLLevel level)
        {
            var levelValue = (int)level;
            var meter = string.Format("{0} m", levelValue);
            var kilometer = string.Format("{0} km", levelValue / 1000);
            var expectedZoomText = levelValue < 1000 ? meter : kilometer;
            WebDriverContext.Wait.Until(driver =>
            {
                try
                {
                    var mapGLZoomText = WebDriverContext.CurrentDriver.FindElement(By.CssSelector("div.mapboxgl-map div.mapboxgl-ctrl-scale"));

                    while (true)
                    {
                        if (expectedZoomText == mapGLZoomText.Text.Trim())
                        {
                            return true;
                        }
                    }
                }
                catch (UnhandledAlertException)
                {
                    ForSeconds(1);
                    return true;
                }
            });
        }

        /// <summary>
        /// Wait for alarm trigger/auto acknowledged
        /// </summary>
        public static void ForAlarmTrigger()
        {
            ForSeconds(Settings.AlarmTimeWait);
        }

        #endregion //Extensive Wait For methods
    }
}
