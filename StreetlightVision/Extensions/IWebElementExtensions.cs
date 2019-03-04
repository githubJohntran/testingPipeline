using ImageMagick;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using SeleniumExtras.WaitHelpers;
using StreetlightVision.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace StreetlightVision.Extensions
{
    public static class IWebElementExtensions
    {
        private const string JS_HELPER_FILE_PATH = @"Resources\js\jshelper.js";


        /// <summary>
        /// The correct datetime's format should be: (MMMM/dd/yyyy) or (MM/dd/yyyy)
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string CorrectDateTimeFormat(string format)
        {
            var correctFormat = new StringBuilder();
            foreach (var c in format)
            {
                if (c.Equals('m'))
                    correctFormat.Append(Char.ToUpperInvariant(c));
                else
                    correctFormat.Append(Char.ToLowerInvariant(c));
            }
            return correctFormat.ToString();
        }

        /// <summary>
        /// Find an element from javascript
        /// </summary>
        /// <param name="searchContext"></param>
        /// <returns></returns>
        public static IWebElement FindElementByJS(this ISearchContext searchContext, string cssSelector, string subFrameName = null)
        {
            DateTime endingTime = DateTime.Now.AddMilliseconds(Settings.DriverWaitingTimeout);

            while (DateTime.Now < endingTime)
            {
                try
                {
                    var fileString = File.ReadAllText(Path.Combine(Settings.AssemblyPath, JS_HELPER_FILE_PATH));

                    return (IWebElement)WebDriverContext.JsExecutor.ExecuteScript(fileString + "return getElementFromRoot(arguments[0], arguments[1], arguments[2]);", searchContext, cssSelector, subFrameName);
                }
                catch (Exception)
                {
                }

                Wait.ForMilliseconds(200);
            }

            throw new TimeoutException("While looking for the element by javascript using the CSS Selector " + cssSelector + ", the timeout was reached.");
        }

        /// <summary>
        /// Find elements by javascript
        /// </summary>
        /// <param name="searchContext"></param>
        /// <returns></returns>
        public static IList<IWebElement> FindElementsByJS(this ISearchContext searchContext, string cssSelector, string subFrameName = null)
        {
            DateTime endingTime = DateTime.Now.AddMilliseconds(Settings.DriverWaitingTimeout);

            while (DateTime.Now < endingTime)
            {
                try
                {
                    var fileString = File.ReadAllText(Path.Combine(Settings.AssemblyPath, JS_HELPER_FILE_PATH));

                    var results = (IList)WebDriverContext.JsExecutor.ExecuteScript(fileString + "return getElementsFromRoot(arguments[0], arguments[1], arguments[2]);", searchContext, cssSelector, subFrameName);

                    var elementList = new List<IWebElement>();

                    if (results.Count > 0)
                    {
                        elementList = ((IReadOnlyCollection<IWebElement>)results).ToList();
                    }

                    return elementList;
                }
                catch (Exception)
                {
                }

                Wait.ForMilliseconds(200);
            }

            throw new TimeoutException("While looking for the elements by javascript using the CSS Selector " + cssSelector + ", the timeout was reached.");
        }

        /// <summary>
        /// Get parent of an element
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static IWebElement GetParentElement(this IWebElement element)
        {
            return (IWebElement)WebDriverContext.JsExecutor.ExecuteScript("return arguments[0].parentNode", element);
        }

        /// <summary>
        /// Get previous sibling element of a element
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static IWebElement GetPreviousElement(this IWebElement element)
        {
            return (IWebElement)WebDriverContext.JsExecutor.ExecuteScript("return arguments[0].previousElementSibling", element);
        }

        /// <summary>
        /// Get next sibling element of a element
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static IWebElement GetNextElement(this IWebElement element)
        {
            return (IWebElement)WebDriverContext.JsExecutor.ExecuteScript("return arguments[0].nextElementSibling", element);
        }

        /// <summary>
        /// Click using javascript
        /// </summary>
        /// <param name="element"></param>
        public static void ClickByJS(this IWebElement element)
        {
            WebDriverContext.JsExecutor.ExecuteScript("arguments[0].click()", element);
        }

        /// <summary>
        /// Double click
        /// </summary>
        /// <param name="element"></param>
        public static void DoubleClick(this IWebElement element, bool is2click = true)
        {
            var action = new Actions(WebDriverContext.CurrentDriver);

            action.MoveToElement(element).Perform();
            if (is2click)
            {
                action.Click().Click().Perform();
            }
            else
            {
                element.DoubleClickByJS();
            }
        }

        /// <summary>
        ///  Double click by javascript
        /// </summary>
        /// <param name="element"></param>
        public static void DoubleClickByJS(this IWebElement element)
        {
            WebDriverContext.JsExecutor.ExecuteScript("var evt = document.createEvent('MouseEvents');" +
                   "evt.initMouseEvent('dblclick', true, true, window, 0, 0, 0, 0, 0, false, false, false, false, 0, null);" +
                   "arguments[0].dispatchEvent(evt);", element);
        }

        /// <summary>
        /// Double click at x,y
        /// </summary>
        /// <param name="element"></param>
        /// <param name="offsetX"></param>
        /// <param name="offsetY"></param>
        public static void DoubleClick(this IWebElement element, int offsetX, int offsetY, bool is2click = true)
        {
            var action = new Actions(WebDriverContext.CurrentDriver);
            action.MoveToElement(element, offsetX, offsetY).Perform();
            if (is2click)
            {
                action.Click().Click().Perform();
            }
            else
            {
                action.DoubleClick().Build().Perform();
            }
        }

        /// <summary>
        /// Scroll to a specific element
        /// </summary>
        /// <param name="element"></param>
        /// <param name="alignToTop"></param>
        public static void ScrollToElementByJS(this IWebElement element, bool alignToTop = false)
        {
            if (element == null)
                throw new Exception("Object is null");
            try
            {
                WebDriverContext.JsExecutor.ExecuteScript("arguments[0].scrollIntoView(arguments[1]);", element, alignToTop);
                Wait.ForMilliseconds(200);
            }
            catch
            {
                throw new Exception("Could not scroll to element");
            }
        }

        /// <summary>
        /// Drag And Drop to an offset X,Y by Javascript
        /// </summary>
        /// <param name="element"></param>
        /// <param name="offsetX"></param>
        /// <param name="offsetY"></param>
        public static void DragAndDropToOffsetByJS(this IWebElement element, int offsetX, int offsetY, bool includedLocation = true)
        {
            var actualOffsetX = includedLocation ? element.Location.X + offsetX : offsetX;
            var actualOffsetY = includedLocation ? element.Location.Y + offsetY : offsetY;
            WebDriverContext.JsExecutor.ExecuteScript("function simulate(f,c,d,e){var b,a=null;for(b in eventMatchers)if(eventMatchers[b].test(c)){a=b;break}if(!a)return!1;document.createEvent?(b=document.createEvent(a),a==\"HTMLEvents\"?b.initEvent(c,!0,!0):b.initMouseEvent(c,!0,!0,document.defaultView,0,d,e,d,e,!1,!1,!1,!1,0,null),f.dispatchEvent(b)):(a=document.createEventObject(),a.detail=0,a.screenX=d,a.screenY=e,a.clientX=d,a.clientY=e,a.ctrlKey=!1,a.altKey=!1,a.shiftKey=!1,a.metaKey=!1,a.button=1,f.fireEvent(\"on\"+c,a));return!0} var eventMatchers={HTMLEvents:/^(?:load|unload|abort|error|select|change|submit|reset|focus|blur|resize|scroll)$/,MouseEvents:/^(?:click|dblclick|mouse(?:down|up|over|move|out))$/}; " +
            "simulate(arguments[0],\"mousedown\",0,0); simulate(arguments[0],\"mousemove\",arguments[1],arguments[2]); simulate(arguments[0],\"mouseup\",arguments[1],arguments[2]); ",
            element, actualOffsetX, actualOffsetY);
        }

        /// <summary>
        /// Build data table from grid container. Data doesn't include total row
        /// </summary>
        /// <returns></returns>
        public static DataTable BuildDataTableFromGrid(this IWebElement gridContainer)
        {
            DataTable tblResult = new DataTable();
            var columnHeaders = (gridContainer.GetGridHeaders() as IEnumerable).Cast<string>().ToList();
            var noNameColumnCount = 0;

            foreach (var header in columnHeaders)
            {
                var headerText = header as string;
                headerText = headerText.TrimStart('\r', '\n', '\t', ' ').TrimEnd('\r', '\n', '\t', ' ');
                if (headerText.Trim() == string.Empty)
                {
                    headerText = string.Format("Empty Header {0}", ++noNameColumnCount);
                }

                if (headerText == "#")
                {
                    headerText = "Line #";
                }

                tblResult.Columns.Add(headerText);
            }

            /*
             * Add rows to table
             */

            var gridData = gridContainer.GetGridTable() as IEnumerable;

            foreach (object element in gridData)
            {
                var newRow = new List<string>();
                foreach (string cell in element as IEnumerable)
                {
                    var trimmedCell = cell.TrimStart('\r', '\n', '\t', ' ').TrimEnd('\r', '\n', '\t', ' ');
                    newRow.Add(trimmedCell);
                }

                if (columnHeaders.Count() < newRow.Count)
                {
                    while (newRow.Count >= 5)
                    {
                        newRow.RemoveAt(newRow.Count - 3);
                    }

                    newRow.RemoveAt(newRow.Count - 1);
                }

                tblResult.Rows.Add(newRow.ToArray());
            }

            return tblResult;
        }

        /// <summary>
        /// Build a data table from total row. Therefore, the data table contains only 1 row
        /// </summary>
        /// <returns></returns>
        public static DataTable BuildDataTableFromTotalRow(this IWebElement gridContainer)
        {
            string gridHeaderColumnsCss = "td.w2ui-head:not(.w2ui-head-last)";
            string gridDataCellsCss = "td.w2ui-col-number, td.w2ui-grid-data:not(.w2ui-grid-data-last)";

            DataTable tblResult = new DataTable();

            /*
             * Add columns to table
             */
            var columnHeaderElements = gridContainer.FindElements(By.CssSelector(gridHeaderColumnsCss));

            if (columnHeaderElements != null)
            {
                foreach (var columnHeaderElement in columnHeaderElements)
                {
                    var headerText = columnHeaderElement.Text;

                    if (headerText == "#")
                    {
                        headerText = "Line #";
                    }

                    tblResult.Columns.Add(headerText);
                }
            }

            /*
             * Add total row to table
             */

            var totalRow = gridContainer.GetParentElement().FindElementByJS(".w2ui-grid-summary:not([style*='display: none'])");

            if (totalRow != null)
            {
                var totalCellElements = totalRow.FindElementsByJS(gridDataCellsCss);

                if (totalCellElements != null)
                {
                    var rowData = new List<string>();
                    foreach (var totalCellElement in totalCellElements)
                    {
                        var cellValue = totalCellElement.Text;

                        rowData.Add(cellValue);
                    }

                    tblResult.Rows.Add(rowData.ToArray());
                }
            }

            return tblResult;
        }

        /// <summary>
        /// Enter value for an input element
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public static void Enter(this IWebElement element, string value, bool shouldClear = true, bool isAutocomplete = false, bool pressEnterKey = false)
        {
            if (shouldClear)
            {
                try  { element.Clear(); }
                catch { element.ClearByJS(); }
            }
            WebDriverContext.Wait.Until(ExpectedConditions.ElementToBeClickable(element));
            element.ClickEx();
            element.SendKeys(value);            
            if (isAutocomplete)
            {
                Wait.ForElementDisplayed(By.CssSelector("ul.ui-autocomplete.ui-menu.ui-widget"));
                var item = WebDriverContext.CurrentDriver.FindElements(By.CssSelector("ul.ui-autocomplete.ui-menu.ui-widget .ui-menu-item > div")).FirstOrDefault();
                if (item != null)
                {
                    item.ClickEx();
                    Wait.ForElementsInvisible(By.CssSelector("ul.ui-autocomplete.ui-menu.ui-widget"));
                }
            }
            if (pressEnterKey) element.SendKeys(Keys.Enter);
        }

        /// <summary>
        /// Enter value for an date input element
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public static void Enter(this IWebElement element, DateTime date)
        {
            try { element.Clear(); }
            catch { element.ClearByJS(); }
            var dateFormat = element.GetAttribute("placeholder");
            var dateFormated = date.ToString(CorrectDateTimeFormat(dateFormat));
            element.ClickEx();
            element.SendKeys(dateFormated);

            Wait.ForElementDisplayed(By.CssSelector(".w2ui-overlay"));
            Wait.ForSeconds(1);
            var calendarDates = WebDriverContext.CurrentDriver.FindElements(By.CssSelector(".w2ui-overlay .w2ui-date"));
            var curDate = calendarDates.FirstOrDefault(p => p.GetAttribute("date").Equals(dateFormated));
            if (curDate != null)
            {
                curDate.ClickEx();
                Wait.ForElementInvisible(By.CssSelector(".w2ui-overlay"));
            }
        }

        /// <summary>
        /// Enter a time value (format: HH:mm, ex: 5:00, 18:00)
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public static void EnterTime(this IWebElement element, string value)
        {
            var placeholder = element.GetAttribute("placeholder");
            if (placeholder.ToLower().Equals("hh:mi pm"))
            {
                var datetime = DateTime.ParseExact(value, "HH:mm", CultureInfo.CurrentCulture);
                value = datetime.ToString("hh:mm tt");
            }

            try { element.Clear(); }
            catch { element.ClearByJS(); }
            element.ClickEx();
            element.Enter(value);
        }

        public static byte[] TakeScreenshotAsBytes(this IWebElement element)
        {
            Screenshot screenShot = ((ITakesScreenshot)WebDriverContext.CurrentDriver).GetScreenshot();
            MagickImage elementImage = new MagickImage(screenShot.AsByteArray, new MagickReadSettings());

            elementImage.Crop(element.Location.X, element.Location.Y, element.Size.Width, element.Size.Height);
            var result = elementImage.ToByteArray();

            return result;
        }

        public static byte[] TakeScreenshotAsBytes(this IWebElement element, int cropLeft, int cropTop, int cropRight, int cropBottom)
        {
            Screenshot screenShot = ((ITakesScreenshot)WebDriverContext.CurrentDriver).GetScreenshot();
            MagickImage elementImage = new MagickImage(screenShot.AsByteArray, new MagickReadSettings());

            elementImage.Crop(element.Location.X + cropLeft, element.Location.Y + cropTop, element.Size.Width - (cropRight + cropLeft), element.Size.Height - (cropBottom + cropTop));
            var result = elementImage.ToByteArray();

            return result;
        }

        public static Bitmap TakeScreenshotAsBitmap(this IWebElement element)
        {
            Screenshot screenShot = ((ITakesScreenshot)WebDriverContext.CurrentDriver).GetScreenshot();
            MagickImage elementImage = new MagickImage(screenShot.AsByteArray, new MagickReadSettings());

            elementImage.Crop(element.Location.X, element.Location.Y, element.Size.Width, element.Size.Height);
            var result = elementImage.ToBitmap();

            return result;
        }

        public static Bitmap TakeScreenshotAsBitmap(this IWebElement element, int cropLeft, int cropTop, int cropRight, int cropBottom)
        {
            Screenshot screenShot = ((ITakesScreenshot)WebDriverContext.CurrentDriver).GetScreenshot();
            MagickImage elementImage = new MagickImage(screenShot.AsByteArray, new MagickReadSettings());

            elementImage.Crop(element.Location.X + cropLeft, element.Location.Y + cropTop, element.Size.Width - (cropRight + cropLeft), element.Size.Height - (cropBottom + cropTop));
            var result = elementImage.ToBitmap();

            return result;
        }

        /// <summary>
        /// Take Screenshot
        /// </summary>
        /// <param name="element"></param>
        /// <returns>name path file</returns>
        public static void TakeScreenshot(this IWebElement element, string outputNamePath)
        {
            Screenshot screenShot = ((ITakesScreenshot)WebDriverContext.CurrentDriver).GetScreenshot();
            MagickImage elementImage = new MagickImage(screenShot.AsByteArray, new MagickReadSettings());

            elementImage.Crop(element.Location.X, element.Location.Y, element.Size.Width, element.Size.Height);
            elementImage.Write(outputNamePath);
        }

        /// <summary>
        /// Take Screenshot
        /// </summary>
        /// <param name="element"></param>
        /// <returns>name path file</returns>
        public static void TakeScreenshot(this IWebElement element, string outputNamePath, int cropLeft, int cropTop, int cropRight, int cropBottom)
        {
            Screenshot screenShot = ((ITakesScreenshot)WebDriverContext.CurrentDriver).GetScreenshot();
            MagickImage elementImage = new MagickImage(screenShot.AsByteArray, new MagickReadSettings());

            elementImage.Crop(element.Location.X + cropLeft, element.Location.Y + cropTop, element.Size.Width - (cropRight + cropLeft), element.Size.Height - (cropBottom + cropTop));
            elementImage.Write(outputNamePath);
        }

        /// <summary>
        /// Select value for a combo box
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public static void Select(this IWebElement element, string value, bool isListBox = false)
        {
            if (element.Text.Equals(value)) return;

            IWebElement inputTextBox = null;
            element.ClickEx();
            Wait.ForElementDisplayed(By.CssSelector("[id='select2-drop']"));
            if (!isListBox)
            {
                inputTextBox = WebDriverContext.CurrentDriver.FindElement(By.CssSelector("[id='select2-drop'] input"));              
                try { inputTextBox.Clear(); }
                catch { inputTextBox.ClearByJS(); }
                inputTextBox.SendKeys(value);
            }
            
            Wait.ForElementsDisplayed(By.CssSelector("[id='select2-drop'] ul li.select2-result.select2-result-selectable"));
            var result = WebDriverContext.CurrentDriver.FindElements(By.CssSelector("[id='select2-drop'] ul li.select2-result.select2-result-selectable")).ToList();
            var item = result.FirstOrDefault(e => e.Text.Trim().Equals(value.Trim(), StringComparison.InvariantCultureIgnoreCase));
            if (item == null)
            {
                item = result.FirstOrDefault(e => e.Text.Trim().IndexOf(value.Trim(), StringComparison.InvariantCultureIgnoreCase) != -1);
            }
            if (item == null)
            {
                var firstWord = value.SplitAndGetAt(new char[] { ' ' }, 0);
                if (!isListBox)
                {
                    try { inputTextBox.Clear(); }
                    catch { inputTextBox.ClearByJS(); }
                    inputTextBox.SendKeys(firstWord);
                }
                item = result.FirstOrDefault(e => e.Text.Trim().IndexOf(firstWord.Trim(), StringComparison.InvariantCultureIgnoreCase) != -1);
            }
            if (item != null)
            {
                item.ScrollToElementByJS();
                item.ClickEx();
                Wait.ForElementInvisible(By.CssSelector("[id='select2-drop']"));
            }
            else
                throw new Exception(string.Format("There is no item with '{0}'", value));
        }

        public static void SelectMultiple(this IWebElement element, params string[] values)
        {
            foreach (var value in values)
            {
                var inputTextBox = element.FindElement(By.CssSelector("input"));
                inputTextBox.SendKeys(value);
                var result = WebDriverContext.CurrentDriver.FindElements(By.CssSelector("[id='select2-drop'] ul li.select2-result.select2-result-selectable")).ToList();
                var item = result.FirstOrDefault(e => e.Text.Equals(value, StringComparison.InvariantCultureIgnoreCase));
                if (item == null)
                {
                    item = result.FirstOrDefault(e => e.Text.IndexOf(value, StringComparison.InvariantCultureIgnoreCase) != -1);
                }
                if (item == null)
                {
                    var firstWord = value.SplitAndGetAt(new char[] { ' ' }, 0);
                    try { inputTextBox.Clear(); }
                    catch { inputTextBox.ClearByJS(); }
                    inputTextBox.SendKeys(firstWord);
                    result = WebDriverContext.CurrentDriver.FindElements(By.CssSelector("[id='select2-drop'] ul li.select2-result.select2-result-selectable")).ToList();
                    item = result.FirstOrDefault(e => e.Text.IndexOf(firstWord, StringComparison.InvariantCultureIgnoreCase) != -1);
                }
                if (item != null)
                {
                    item.ScrollToElementByJS();
                    item.ClickEx();
                    Wait.ForElementInvisible(By.CssSelector("[id='select2-drop']"));
                }
                else
                    throw new Exception(string.Format("There is no item with '{0}'", value));
            }           
        }

        /// <summary>
        /// Select an icon item in dropdown
        /// </summary>
        /// <param name="element"></param>
        /// <param name="src"></param>
        public static void SelectIcon(this IWebElement element, string src)
        {
            element.ClickEx();
            Wait.ForElementDisplayed(By.CssSelector("[id='select2-drop']"));

            Wait.ForElementsDisplayed(By.CssSelector("[id='select2-drop'] ul li.select2-result.select2-result-selectable"));
            var result = WebDriverContext.CurrentDriver.FindElements(By.CssSelector("[id='select2-drop'] ul li.select2-result.select2-result-selectable")).ToList();
            var item = result.FirstOrDefault(e => e.FindElement(By.CssSelector("img")).GetAttribute("src").Contains(src));
            if (item != null)
            {
                item.ClickEx();
                Wait.ForElementInvisible(By.CssSelector("[id='select2-drop']"));
            }
            else
                throw new Exception(string.Format("There is no item with this src '{0}'", src));
        }

        /// <summary>
        /// Check if an item is existing in a combo box
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public static bool CheckIfItemExists(this IWebElement element, string value, bool isListBox = false)
        {
            element.ClickEx();
            Wait.ForElementDisplayed(By.CssSelector("[id='select2-drop']"));
            if (!isListBox)
            {
                var inputTextBox = WebDriverContext.CurrentDriver.FindElement(By.CssSelector("[id='select2-drop'] input"));
                try { inputTextBox.Clear(); }
                catch { inputTextBox.ClearByJS(); }
                inputTextBox.SendKeys(value);
            }
            Wait.ForElementsDisplayed(By.CssSelector("[id='select2-drop'] ul li.select2-result.select2-result-selectable, [id='select2-drop'] ul li.select2-no-results"));
            var result = WebDriverContext.CurrentDriver.FindElements(By.CssSelector("[id='select2-drop'] ul li.select2-result.select2-result-selectable, [id='select2-drop'] ul li.select2-no-results")).ToList();
            var item = result.FirstOrDefault(e => e.Text.Equals(value, StringComparison.InvariantCultureIgnoreCase) || e.Text.IndexOf(value, StringComparison.InvariantCultureIgnoreCase) != -1);
            var action = new Actions(WebDriverContext.CurrentDriver);
            action.SendKeys(Keys.Escape).Build().Perform();
            Wait.ForElementInvisible(By.CssSelector("[id='select2-drop']"));
            if (item != null)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Get all items of a combo box
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static List<string> GetAllItems(this IWebElement element, bool isCloseResult = true)
        {
            var bySelectDrop = By.CssSelector("[id='select2-drop']");
            if (!ElementUtility.IsDisplayed(bySelectDrop))
            {
                element.ClickEx();
            }
            Wait.ForElementDisplayed(bySelectDrop);
            Wait.ForElementsDisplayed(By.CssSelector("[id='select2-drop'] ul li.select2-result.select2-result-selectable, [id='select2-drop'] ul li.select2-no-results"));            
            var items = JSUtility.GetElementsText("[id='select2-drop'] ul li.select2-result.select2-result-selectable, [id='select2-drop'] ul li.select2-no-results");
            if (isCloseResult)
            {
                var action = new Actions(WebDriverContext.CurrentDriver);
                action.SendKeys(Keys.Escape).Build().Perform();
                Wait.ForElementInvisible(bySelectDrop);
            }

            return items;
        }

        /// <summary>
        ///  Get all autocomplete items of a input
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static List<string> GetAllAutoCompleteItems(this IWebElement element)
        {
            Wait.ForSeconds(1);
            Wait.ForElementDisplayed(By.CssSelector("ul.ui-autocomplete.ui-menu"));            
            var items = JSUtility.GetElementsText("ul.ui-autocomplete.ui-menu .ui-menu-item div");
            var action = new Actions(WebDriverContext.CurrentDriver);
            action.SendKeys(Keys.Escape).Build().Perform();
            Wait.ForElementInvisible(By.CssSelector("ul.ui-autocomplete.ui-menu"));

            return items;
        }

        /// <summary>
        /// Check a checkbox
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public static void Check(this IWebElement element, bool value)
        {
            var inputCheckbox = element.FindElement(By.CssSelector("input[type='checkbox']"));
            if (inputCheckbox.Selected != value)
            {
                var checkboxLabel = element.FindElement(By.CssSelector("label"));
                checkboxLabel.ClickEx();
            }
        }

        /// <summary>
        /// Check if an element is read only
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static bool IsReadOnly(this IWebElement element, bool isInputElement = true)
        {
            if (!isInputElement)
            {
                var input = element.FindElement(By.CssSelector("input"));
                return input.GetAttribute("readonly") != null || input.GetAttribute("disabled") != null;
            }
            return element.GetAttribute("readonly") != null || element.GetAttribute("disabled") != null;
        }

        /// <summary>
        /// Get value of an input
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static string Value(this IWebElement element)
        {
            return element.GetAttribute("value").Trim();
        }

        /// <summary>
        /// Get value of an time input
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static string TimeValue(this IWebElement element)
        {
            var value = element.GetAttribute("value").ToUpper();
            var result = "";
            if (value.Contains("PM") || value.Contains("AM"))
            {
                var h = value.SplitAndGetAt(new char[] { ':' }, 0);
                DateTime time = DateTime.ParseExact(value, h.Length == 1 ? "h:mm tt" : "hh:mm tt", CultureInfo.CurrentCulture);
                var hour = time.Hour;
                hour = hour == 0 ? 12 : hour;
                hour = hour > 12 ? hour - 12 : hour;
                result = string.Format("{0:d2}:{1:d2} {2}", hour, time.Minute, time.ToString("tt", CultureInfo.InvariantCulture).ToUpper());
            }
            else
            {
                var time = DateTime.Parse(value);
                result = string.Format("{0:d2}:{1:d2}", time.Hour, time.Minute);
            }

            return result;
        }

        /// <summary>
        /// Get value of an icon
        /// </summary>
        /// <param name="element"></param>
        /// <returns>base64String or url</returns>
        public static string IconValue(this IWebElement element)
        {
            var src = element.GetAttribute("src");
            if (src == null)
            {
                var img = element.FindElement(By.CssSelector("img"));
                src = img.GetAttribute("src");
            }
            if (src.Contains("data:image/png;base64"))
                src = src.Replace("data:image/png;base64,", string.Empty);

            return src;
        }

        /// <summary>
        /// Get image url of an icon
        /// </summary>
        /// <param name="element"></param>
        /// <returns>base64String</returns>
        public static string ImageUrl(this IWebElement element)
        {
            var url = element.GetAttribute("style");

            return url;
        }

        /// <summary>
        /// Get value of an image
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static string ImageValue(this IWebElement element)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get value of a checkbox
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static bool CheckboxValue(this IWebElement element)
        {
            var inputCheckbox = element.FindElement(By.CssSelector("input[type='checkbox']"));

            return inputCheckbox.Selected;
        }

        /// <summary>
        /// Clear selected value of a dropdown control
        /// </summary>
        /// <param name="dropdownElement"></param>
        /// <param name="itemValue"></param>
        /// <returns></returns>
        public static void RemoveValue(this IWebElement dropdownElement, string itemValue)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Remove a selected value of a list dropdown control
        /// </summary>
        /// <param name="dropdownElement"></param>
        /// <param name="itemValue"></param>
        /// <returns></returns>
        public static void RemoveValueInList(this IWebElement dropdownElement, string itemValue)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns selected item of a list dropdown control
        /// </summary>
        /// <param name="dropdownElement"></param>
        /// <returns></returns>
        public static List<string> GetSelectedItems(this IWebElement dropdownElement)
        {
            var selectedItems = dropdownElement.FindElements(By.CssSelector(".editor-select-values .editor-select-values-item .editor-select-values-item-title"));
            var result = selectedItems.Select(p => p.Text.Trim()).ToList();

            return result;
        }

        /// <summary>
        /// Returns select all item of a list dropdown control
        /// </summary>
        /// <param name="dropdownElement"></param>
        /// <returns></returns>
        public static void SelectAllItems(this IWebElement dropdownElement)
        {
            var selectAllButton = dropdownElement.FindElement(By.CssSelector("button.icon-select-all"));
            selectAllButton.ClickEx();
            dropdownElement.ForChildElementDisplayed(By.CssSelector(".editor-select-values"));
        }
       
        /// <summary>
        /// Clear selected item in a dropdown
        /// </summary>
        /// <param name="dropdownElement"></param>
        public static void ClearSelectedItem(this IWebElement dropdownElement)
        {
            var clearButton = dropdownElement.FindElement(By.CssSelector("abbr.select2-search-choice-close"));
            if (clearButton != null)
                clearButton.ClickEx();
        }

        /// <summary>
        /// Returns select items of a list dropdown control
        /// </summary>
        /// <param name="dropdownElement"></param>
        /// <returns></returns>
        public static void SelectItems(this IWebElement dropdownElement, params string[] items)
        {
            foreach (var item in items)
            {
                dropdownElement.Select(item, true);
            }
        }

        /// <summary>
        /// Select random color for a color picker
        /// </summary>
        /// <param name="element"></param>
        public static void SetColor(this IWebElement element)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get value of an color picker
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static Color ColorValue(this IWebElement element)
        {
            var rgb = element.GetAttribute("value");
            if (rgb == null) return Color.Empty;
            rgb = rgb.Replace("rgb(", string.Empty).Replace(")", string.Empty);
            var splitString = rgb.Split(',');
            var splitInts = splitString.Select(item => int.Parse(item)).ToArray();
            var color = Color.FromArgb(splitInts[0], splitInts[1], splitInts[2]);

            return color;
        }

        /// <summary>
        /// Execute MoveTo and Click action using Actions class of Selenium
        /// </summary>
        /// <param name="element"></param>
        public static void MoveToAndClick(this IWebElement element)
        {
            var action = new Actions(WebDriverContext.CurrentDriver);
            try
            {                
                action.MoveToElement(element).Perform();
                action.Click().Perform();
            }
            catch (InvalidOperationException ex)
            {
                element.ScrollToElementByJS();
                action.MoveToElement(element).Perform();
                action.Click().Perform();
            }
        }

        /// <summary>
        /// Click and hold an element, then move to another position
        /// </summary>
        /// <param name="element"></param>
        /// <param name="toOffsetX"></param>
        /// <param name="toOffsetY"></param>
        /// <param name="waitSeconds"></param>
        public static void ClickHoldAndMoveTo(this IWebElement element, int toOffsetX, int toOffsetY, int waitSeconds)
        {
            var action = new Actions(WebDriverContext.CurrentDriver);
            action.ClickAndHold().Perform();
            Wait.ForSeconds(waitSeconds);
            element.MoveToAndClick(toOffsetX, toOffsetY);
        }

        /// <summary>
        /// Click and hold an element, then move to another position
        /// </summary>
        /// <param name="element"></param>
        /// <param name="fromOffsetX"></param>
        /// <param name="fromOffsetY"></param>
        /// <param name="toOffsetX"></param>
        /// <param name="toOffsetY"></param>
        public static void ClickHoldAndMoveTo(this IWebElement element, int fromOffsetX, int fromOffsetY, int toOffsetX, int toOffsetY, int waitSeconds)
        {
            var action = new Actions(WebDriverContext.CurrentDriver);
            action.MoveToElement(element, fromOffsetX, fromOffsetY).Perform();
            action.ClickAndHold().Perform();
            Wait.ForSeconds(waitSeconds);
            element.MoveToAndClick(toOffsetX, toOffsetY + 5);
        }

        /// <summary>
        /// Execute MoveTo OffsetX,OffsetY and Click action using Actions class of Selenium
        /// </summary>
        /// <param name="element"></param>
        /// <param name="offsetX"></param>
        /// <param name="offsetY"></param>
        public static void MoveToAndClick(this IWebElement element, int offsetX, int offsetY)
        {
            var delta = 0;
            if (Browser.Name.Equals("FF")) delta = 5;
            var action = new Actions(WebDriverContext.CurrentDriver);
            action.MoveToElement(element, offsetX, offsetY - delta).Click().Build().Perform();
        }

        /// <summary>
        /// Execute MoveTo OffsetX,OffsetY and Click + Shift key using Actions class of Selenium
        /// </summary>
        /// <param name="element"></param>
        /// <param name="offsetX"></param>
        /// <param name="offsetY"></param>
        public static void MoveToAndClickWithShiftKey(this IWebElement element, int offsetX, int offsetY)
        {
            var delta = 0;
            if (Browser.Name.Equals("FF")) delta = 5;
            var action = new Actions(WebDriverContext.CurrentDriver);
            action.MoveToElement(element, offsetX, offsetY - delta).KeyDown(Keys.Shift).Click().KeyUp(Keys.Shift).Build().Perform();
        }

        /// <summary>
        /// Execute Click + Shift key using Actions class of Selenium
        /// </summary>
        /// <param name="element"></param>
        public static void ClickAndHoldWithShiftKey(this IWebElement element)
        {
            var action = new Actions(WebDriverContext.CurrentDriver);
            action.KeyDown(Keys.Shift);
            element.ClickEx();
            action.KeyUp(Keys.Shift).Build().Perform();
        }

        /// <summary>
        /// Execute Click + Ctrl key using Actions class of Selenium
        /// </summary>
        /// <param name="element"></param>
        public static void ClickAndHoldWithCtrlKey(this IWebElement element)
        {
            var action = new Actions(WebDriverContext.CurrentDriver);
            action.KeyDown(Keys.LeftControl);
            element.ClickEx();
            action.KeyUp(Keys.LeftControl).Build().Perform();
        }

        /// <summary>
        ///  Execute MoveTo OffsetX,OffsetY and Click + Ctrl key using Actions class of Selenium
        /// </summary>
        /// <param name="element"></param>
        /// <param name="offsetX"></param>
        /// <param name="offsetY"></param>
        public static void MoveToAndClickWithCtrlKey(this IWebElement element, int offsetX, int offsetY)
        {
            var delta = 0;
            if (Browser.Name.Equals("FF")) delta = 5;
            var action = new Actions(WebDriverContext.CurrentDriver);
            action.MoveToElement(element, offsetX, offsetY - delta).KeyDown(Keys.LeftControl).Click().KeyUp(Keys.LeftControl).Build().Perform();
        }

        /// <summary>
        /// Execute MoveTo OffsetX,OffsetY and Click + s key using Actions class of Selenium
        /// </summary>
        /// <param name="element"></param>
        /// <param name="offsetX"></param>
        /// <param name="offsetY"></param>
        public static void MoveToAndClickWithSKey(this IWebElement element, int offsetX, int offsetY)
        {
            var delta = 0;
            if (Browser.Name.Equals("FF")) delta = 5;
            var action = new Actions(WebDriverContext.CurrentDriver);
            action.MoveToElement(element, offsetX, offsetY - delta).SendKeys("s").Click().SendKeys("s").Build().Perform();
        }

        /// <summary>
        /// Execute MoveTo action using Actions class of Selenium
        /// </summary>
        /// <param name="element"></param>
        public static void MoveTo(this IWebElement element)
        {
            var action = new Actions(WebDriverContext.CurrentDriver);
            action.MoveToElement(element).Perform();
        }

        /// <summary>
        /// Execute MoveTo a position offset using Actions class of Selenium
        /// </summary>
        /// <param name="element"></param>
        /// <param name="position"></param>
        public static void MoveTo(this IWebElement element, Point position)
        {
            element.MoveTo(position.X, position.Y);
        }

        /// <summary>
        /// Execute MoveTo OffsetX,OffsetY using Actions class of Selenium
        /// </summary>
        /// <param name="element"></param>
        /// <param name="offsetX"></param>
        /// <param name="offsetY"></param>
        public static void MoveTo(this IWebElement element, int offsetX, int offsetY)
        {
            var action = new Actions(WebDriverContext.CurrentDriver);
            action.MoveToElement(element, offsetX, offsetY).Perform();
        }

        /// <summary>
        /// Get a value of specific key in Style attribute
        /// </summary>
        /// <param name="element"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetStyleAttr(this IWebElement element, string key = "")
        {
            if (string.IsNullOrEmpty(key))
                return element.GetAttribute("style");

            var styles = element.GetAttribute("style").SplitEx(new char[] { ';' });
            foreach (var style in styles)
            {
                var keyValuePair = style.SplitEx(new string[] { ": " });
                if (keyValuePair[0].Trim().Equals(key))
                {
                    return keyValuePair[1].Trim();
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// Get url of background image css
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static string GetBackgroundImageUrl(this IWebElement element)
        {
            var bgCss = "background-image";
            if (Browser.Name.Equals("Chrome")) bgCss = "background";
            var backgroundImg = element.GetCssValue(bgCss);
            var regex = Regex.Match(backgroundImg, "http.*png");
            if (regex.Success)
                return regex.Groups[0].ToString();

            return string.Empty;
        }

        /// <summary>
        /// Get color of a specific style color of an element (ex: background-color)
        /// </summary>
        /// <param name="element"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static Color GetStyleColorAttr(this IWebElement element, string key = "")
        {
            var colorRGB = element.GetStyleAttr(key);
            if (string.IsNullOrEmpty(colorRGB))
                return Color.Empty;

            colorRGB = colorRGB.Replace("rgb(", string.Empty).Replace(")", string.Empty);
            var splitString = colorRGB.Split(',');
            var splitInts = splitString.Select(item => int.Parse(item)).ToArray();
            var color = Color.FromArgb(splitInts[0], splitInts[1], splitInts[2]);

            return color;
        }

        /// <summary>
        /// Set opacity of an element via its style
        /// </summary>
        /// <param name="element"></param>
        /// <param name="opacity"></param>
        public static void SetOpacity(this IWebElement element, string opacity)
        {
            WebDriverContext.JsExecutor.ExecuteScript("arguments[0].style.opacity=arguments[1]", element, opacity);

            if (Browser.Name.Equals("Chrome"))
            {
                Wait.ForMilliseconds(200);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <param name="childBy"></param>
        /// <returns></returns>
        public static bool ChildExists(this IWebElement element, By childBy)
        {
            try
            {
                var child = element.FindElement(childBy);
                if (child != null)
                {
                    return true;
                }
            }
            catch (NoSuchElementException)
            {
                return false;
            }

            return false;
        }

        public static void WaitForReady(this IWebElement element)
        {
            GenericOperation<bool>.Retry(() => element.CheckReady(), p => p == true, 15);
        }

        public static void WaitForReady(this IList<IWebElement> elements)
        {
            GenericOperation<bool>.Retry(() => elements.CheckReady(), p => p == true, 15);
        }

        private static bool CheckReady(this IWebElement element)
        {
            try
            {
                var attr = element.GetAttribute("class");
                return true;
            }
            catch (StaleElementReferenceException)
            {
                return false;
            }
        }

        private static bool CheckReady(this IList<IWebElement> elements)
        {
            try
            {
                foreach (var element in elements)
                {
                    var attr = element.GetAttribute("class");
                }
                return true;
            }
            catch (StaleElementReferenceException)
            {
                return false;
            }
        }

        /// <summary>
        /// Custom Click method for issue ElementClickInterceptedException
        /// </summary>
        /// <param name="element"></param>
        public static void ClickEx(this IWebElement element)
        {
            try
            {
                element.Click();
            }
            catch
            {
                try
                {
                    element.MoveToAndClick();
                }
                catch
                {
                    element.ClickByJS();
                }
            }
        }

        /// <summary>
        /// Clear input text by JavaScript
        /// </summary>
        /// <param name="element"></param>
        public static void ClearByJS(this IWebElement element)
        {
            WebDriverContext.JsExecutor.ExecuteScript("arguments[0].value=''", element);
        }

    }
}
