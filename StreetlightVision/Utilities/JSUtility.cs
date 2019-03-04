using OpenQA.Selenium;
using StreetlightVision.Extensions;
using StreetlightVision.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;

namespace StreetlightVision.Utilities
{
    public static class JSUtility
    {
        private const string JS_HELPER_FILE_PATH = @"Resources\js\jshelper.js";

        private static string _javaScriptEventSimulator = "" +
            /* Creates a drag event */
            "function createDragEvent(eventName, options)\r\n" +
            "{\r\n" +
            "   var event = document.createEvent(\"DragEvent\");\r\n" +
            "   var screenX = window.screenX + options.clientX;\r\n" +
            "   var screenY = window.screenY + options.clientY;\r\n" +
            "   var pageX = window.pageXOffset + options.clientX;\r\n" +
            "   var pageY = window.pageYOffset + options.clientY;\r\n" +
            "   var clientX = options.clientX;\r\n" +
            "   var clientY = options.clientY;\r\n" +
            "   var dataTransfer = {\r\n" +
            "       data: options.dragData == null ? {} : options.dragData,\r\n" +
            "       setData: function(eventName, val){\r\n" +
            "           if (typeof val === 'string') {\r\n" +
            "               this.data[eventName] = val;\r\n" +
            "           }\r\n" +
            "       },\r\n" +
            "       getData: function(eventName){\r\n" +
            "           return this.data[eventName];\r\n" +
            "       },\r\n" +
            "       clearData: function(){\r\n" +
            "           return this.data = {};\r\n" +
            "       },\r\n" +
            "       setDragImage: function(dragElement, x, y) {}\r\n" +
            "   };\r\n" +
            "   var eventInitialized=false;\r\n" +
            "   if (event != null && event.initDragEvent) {\r\n" +
            "       try {\r\n" +
            "           event.initDragEvent(eventName, true, true, window, 0, screenX, screenY, clientX, clientY, false, false, false, false, 0, null, dataTransfer);\r\n" +
            "           event.initialized=true;\r\n" +
            "       } catch(err) {\r\n" +
            "           // no-op\r\n" +
            "       }\r\n" +
            "   }\r\n" +
            "   if (!eventInitialized) {\r\n" +
            "       event = document.createEvent(\"CustomEvent\");\r\n" +
            "       event.initCustomEvent(eventName, true, true, null);\r\n" +
            "       event.view = window;\r\n" +
            "       event.detail = 0;\r\n" +
            "       event.screenX = screenX;\r\n" +
            "       event.screenY = screenY;\r\n" +
            "       event.pageX = pageX;\r\n" +
            "       event.pageY = pageY;\r\n" +
            "       event.clientX = clientX;\r\n" +
            "       event.clientY = clientY;\r\n" +
            "       event.ctrlKey = false;\r\n" +
            "       event.altKey = false;\r\n" +
            "       event.shiftKey = false;\r\n" +
            "       event.metaKey = false;\r\n" +
            "       event.button = 0;\r\n" +
            "       event.relatedTarget = null;\r\n" +
            "       event.dataTransfer = dataTransfer;\r\n" +
            "   }\r\n" +
            "   return event;\r\n" +
            "}\r\n" +

            /* Creates a mouse event */
            "function createMouseEvent(eventName, options)\r\n" +
            "{\r\n" +
            "   var event = document.createEvent(\"MouseEvent\");\r\n" +
            "   var screenX = window.screenX + options.clientX;\r\n" +
            "   var screenY = window.screenY + options.clientY;\r\n" +
            "   var pageX = window.pageXOffset + options.clientX;\r\n" +
            "   var pageY = window.pageYOffset + options.clientY;\r\n" +
            "   var clientX = options.clientX;\r\n" +
            "   var clientY = options.clientY;\r\n" +
            "   if (event != null && event.initMouseEvent) {\r\n" +
            "       event.initMouseEvent(eventName, true, true, window, 0, screenX, screenY, clientX, clientY, false, false, false, false, 0, null);\r\n" +
            "   } else {\r\n" +
            "       event = document.createEvent(\"CustomEvent\");\r\n" +
            "       event.initCustomEvent(eventName, true, true, null);\r\n" +
            "       event.view = window;\r\n" +
            "       event.detail = 0;\r\n" +
            "       event.screenX = screenX;\r\n" +
            "       event.screenY = screenY;\r\n" +
            "       event.pageX = pageX;\r\n" +
            "       event.pageY = pageY;\r\n" +
            "       event.clientX = clientX;\r\n" +
            "       event.clientY = clientY;\r\n" +
            "       event.ctrlKey = false;\r\n" +
            "       event.altKey = false;\r\n" +
            "       event.shiftKey = false;\r\n" +
            "       event.metaKey = false;\r\n" +
            "       event.button = 0;\r\n" +
            "       event.relatedTarget = null;\r\n" +
            "   }\r\n" +
            "   return event;\r\n" +
            "}\r\n" +

            /* Runs the events */
            "function dispatchEvent(webElement, eventName, event)\r\n" +
            "{\r\n" +
            "   if (webElement.dispatchEvent) {\r\n" +
            "       webElement.dispatchEvent(event);\r\n" +
            "   } else if (webElement.fireEvent) {\r\n" +
            "       webElement.fireEvent(\"on\"+eventName, event);\r\n" +
            "   }\r\n" +
            "}\r\n" +

            /* Simulates an individual event */
            "function simulateEventCall(element, eventName, dragStartEvent, options) {\r\n" +
            "   var event = null;\r\n" +
            "   if (eventName.indexOf(\"mouse\") > -1) {\r\n" +
            "       event = createMouseEvent(eventName, options);\r\n" +
            "   } else {\r\n" +
            "       event = createDragEvent(eventName, options);\r\n" +
            "   }\r\n" +
            "   if (dragStartEvent != null) {\r\n" +
            "       event.dataTransfer = dragStartEvent.dataTransfer;\r\n" +
            "   }\r\n" +
            "   dispatchEvent(element, eventName, event);\r\n" +
            "   return event;\r\n" +
            "}\r\n";

        /**
         * Simulates an individual events
         */
        private static string _simulateEvent = _javaScriptEventSimulator +
                "function simulateEvent(element, eventName, clientX, clientY, dragData) {\r\n" +
                "   return simulateEventCall(element, eventName, null, {clientX: clientX, clientY: clientY, dragData: dragData});\r\n" +
                "}\r\n" +

                "var event = simulateEvent(arguments[0], arguments[1], arguments[2], arguments[3], arguments[4]);\r\n" +
                "if (event.dataTransfer != null) {\r\n" +
                "   return event.dataTransfer.data;\r\n" +
                "}\r\n";

        /**
         * Simulates drag and drop
         */
        private static string _simulateHTML5DragAndDrop = _javaScriptEventSimulator +
                "function simulateHTML5DragAndDrop(dragFrom, dragTo, dragFromX, dragFromY, dragToX, dragToY) {\r\n" +
                "   var mouseDownEvent = simulateEventCall(dragFrom, \"mousedown\", null, {clientX: dragFromX, clientY: dragFromY});\r\n" +
                "   var dragStartEvent = simulateEventCall(dragFrom, \"dragstart\", null, {clientX: dragFromX, clientY: dragFromY});\r\n" +
                "   var dragEnterEvent = simulateEventCall(dragTo,   \"dragenter\", dragStartEvent, {clientX: dragToX, clientY: dragToY});\r\n" +
                "   var dragOverEvent  = simulateEventCall(dragTo,   \"dragover\",  dragStartEvent, {clientX: dragToX, clientY: dragToY});\r\n" +
                "   var dropEvent      = simulateEventCall(dragTo,   \"drop\",      dragStartEvent, {clientX: dragToX, clientY: dragToY});\r\n" +
                "   var dragEndEvent   = simulateEventCall(dragFrom, \"dragend\",   dragStartEvent, {clientX: dragToX, clientY: dragToY});\r\n" +
                "}\r\n" +
                "simulateHTML5DragAndDrop(arguments[0], arguments[1], arguments[2], arguments[3], arguments[4], arguments[5]);\r\n";

        public static object GetGridHeaders(this IWebElement gridContainer)
        {
            var script = @"
                function getGridHeaders(gridContainer) {
	                let columns = [];
                    let headerColumnsGrouped = gridContainer.querySelectorAll('[id$=columns] tr').length > 1;
	                let headerColumns = headerColumnsGrouped? gridContainer.querySelectorAll('tr:nth-child(2) td.w2ui-head div.w2ui-col-group') : gridContainer.querySelectorAll('td.w2ui-head:not(.w2ui-head-last)');

                    // if only one column, w2ui doesn't add the unreal last column
                    let numberOfColumnsToAdd = headerColumns.length > 1 && headerColumnsGrouped? headerColumns.length - 1 : headerColumns.length;

	                for(let i = 0; i < numberOfColumnsToAdd; i++) {
		                columns.push(headerColumns[i].innerText);
	                }

	                return columns;
                }
            ";

            return WebDriverContext.JsExecutor.ExecuteScript(script + "return getGridHeaders(arguments[0])", gridContainer);
        }

        /// <summary>
        /// Get all grid headers with duplicated column Name (ex: Timestamp, Value)
        /// </summary>
        /// <param name="gridContainer"></param>
        /// <returns></returns>
        public static object GetAllGridHeaders(this IWebElement gridContainer)
        {
            var script = @"
                function getGridHeaders(gridContainer) {
                 let columns = [];                
                 let headerColumns = gridContainer.querySelectorAll('td.w2ui-head:not(.w2ui-head-last)');

                 for(let i = 0; i < headerColumns.length; i++) {
                  columns.push(headerColumns[i].innerText);
                 }

                 return columns;
                }
            ";

            return WebDriverContext.JsExecutor.ExecuteScript(script + "return getGridHeaders(arguments[0])", gridContainer);
        }

        /// <summary>
        /// Get data records from data grid
        /// </summary>
        /// <param name="gridContainer"></param>
        /// <returns></returns>
        public static object GetGridTable(this IWebElement gridContainer)
        {
            DateTime endingTime = DateTime.Now.AddMilliseconds(Settings.DriverWaitingTimeout);

            while (DateTime.Now < endingTime)
            {
                try
                {
                    var fileString = File.ReadAllText(Path.Combine(Settings.AssemblyPath, JS_HELPER_FILE_PATH));
                    return WebDriverContext.JsExecutor.ExecuteScript(fileString + "return getGridTable(arguments[0]);", gridContainer);
                }
                catch (Exception)
                {
                }
                Wait.ForMilliseconds(200);
            }

            throw new TimeoutException("While getting data grid by javascript the timeout was reached.");
        }

        /// <summary>
        /// Get element text
        /// </summary>
        /// <param name="cssSelector"></param>
        /// <param name="subFrameId"></param>
        /// <returns></returns>
        public static string GetElementText(string cssSelector, string subFrameId = null)
        {
            var fileString = File.ReadAllText(Path.Combine(Settings.AssemblyPath, JS_HELPER_FILE_PATH));
            return (string)WebDriverContext.JsExecutor.ExecuteScript(fileString + "return getElementText(arguments[0], arguments[1]);", cssSelector, subFrameId);
        }

        /// <summary>
        /// Get elements text
        /// </summary>
        /// <param name="cssSelector"></param>
        /// <param name="subFrameId"></param>
        /// <returns></returns>
        public static List<string> GetElementsText(string cssSelector, string subFrameId = null)
        {
            var fileString = File.ReadAllText(Path.Combine(Settings.AssemblyPath, JS_HELPER_FILE_PATH));
            var results = (IList)WebDriverContext.JsExecutor.ExecuteScript(fileString + "return getElementsText(arguments[0], arguments[1]);", cssSelector, subFrameId);
            return results.OfType<string>().Select(p => p.TrimEx()).ToList();
        }

        /// <summary>
        /// Get an attribute value of element
        /// </summary>
        /// <param name="cssSelector"></param>
        /// <param name="attribute"></param>
        /// <param name="subFrameId"></param>
        /// <returns></returns>
        public static string GetElementAttributeValue(string cssSelector, string attribute, string subFrameId = null)
        {
            var fileString = File.ReadAllText(Path.Combine(Settings.AssemblyPath, JS_HELPER_FILE_PATH));
            return (string)WebDriverContext.JsExecutor.ExecuteScript(fileString + "return getElementAttributeValue(arguments[0], arguments[1], arguments[2]);", cssSelector, attribute, subFrameId);
        }

        /// <summary>
        /// Get list of attribute values of elements
        /// </summary>
        /// <param name="cssSelector"></param>
        /// <param name="attribute"></param>
        /// <param name="subFrameId"></param>
        /// <returns></returns>
        public static List<string> GetElementsAttributeValue(string cssSelector, string attribute, string subFrameId = null)
        {
            var fileString = File.ReadAllText(Path.Combine(Settings.AssemblyPath, JS_HELPER_FILE_PATH));
            var results = (IList)WebDriverContext.JsExecutor.ExecuteScript(fileString + "return getElementsAttributeValue(arguments[0], arguments[1], arguments[2]);", cssSelector, attribute, subFrameId);
            return results.OfType<string>().Select(p => p.TrimEx()).ToList();
        }

        /// <summary>
        /// Drag and drop using javascript
        /// </summary>
        /// <param name="srcElement"></param>
        /// <param name="dstElement"></param>
        public static void DragAndDropByJS(IWebElement srcElement, IWebElement dstElement)
        {
            var fileString = File.ReadAllText(Path.Combine(Settings.AssemblyPath, JS_HELPER_FILE_PATH));
            WebDriverContext.JsExecutor.ExecuteScript(fileString + "simulateDragDrop(arguments[0], arguments[1]);", srcElement, dstElement);
        }

        /// <summary>
        /// Check if the current element is invisible or not
        /// </summary>
        /// <param name="cssSelector"></param>
        /// <param name="subFrameId"></param>
        /// <returns></returns>
        public static bool IsElementInvisible(string cssSelector, string subFrameId = null)
        {
            var fileString = File.ReadAllText(Path.Combine(Settings.AssemblyPath, JS_HELPER_FILE_PATH));
            return (bool)WebDriverContext.JsExecutor.ExecuteScript(fileString + "return isElementInvisible(arguments[0], arguments[1]);", cssSelector, subFrameId);
        }

        /// <summary>
        /// Click on a specific element based on its CSS Selector by using external javascript file
        /// </summary>
        /// <param name="cssSelector"></param>
        /// <param name="subFrameId"></param>
        public static void ClickOnElement(string cssSelector, string subFrameId = null)
        {
            var fileString = File.ReadAllText(Path.Combine(Settings.AssemblyPath, JS_HELPER_FILE_PATH));
            WebDriverContext.JsExecutor.ExecuteScript(fileString + "clickOnElement(arguments[0], arguments[1])", cssSelector, subFrameId);
        }

        public static void ScrollToNode(string nodeName)
        {
            var cssNode = "\"[id$='browser-content'] div.w2ui-node, [id$='settings'][style*='display: block'] div.w2ui-node\"";
            var script = @"
               function scrollToNode(nodeText) {
                 let nodes = document.querySelectorAll(" + cssNode + @");
                 for(let i = 0; i < nodes.length; i++) {
                  if(nodes[i].innerText.indexOf(nodeText) != -1) {
                   nodes[i].scrollIntoView();
                   break;
                  }
                }
                }
            ";

            WebDriverContext.JsExecutor.ExecuteScript(script + "scrollToNode(arguments[0])", nodeName);
        }

        public static int GetX(Position pos, int width)
        {
            if (Position.TopLeft.Equals(pos) || Position.Left.Equals(pos) || Position.BottomLeft.Equals(pos))
            {
                return 1;
            }
            else if (Position.Top.Equals(pos) || Position.Center.Equals(pos) || Position.Bottom.Equals(pos))
            {
                return width / 2;
            }
            else if (Position.TopRight.Equals(pos) || Position.Right.Equals(pos) || Position.BottomRight.Equals(pos))
            {
                return width - 1;
            }
            else
            {
                return 0;
            }
        }

        static int GetY(Position pos, int height)
        {
            if (Position.TopLeft.Equals(pos) || Position.Top.Equals(pos) || Position.TopRight.Equals(pos))
            {
                return 1;
            }
            else if (Position.Left.Equals(pos) || Position.Center.Equals(pos) || Position.Right.Equals(pos))
            {
                return height / 2;
            }
            else if (Position.BottomLeft.Equals(pos) || Position.Bottom.Equals(pos) || Position.BottomRight.Equals(pos))
            {
                return height - 1;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Calls a drag event
        /// </summary>
        /// <param name="dragFrom"></param>
        /// <param name="eventName"></param>
        /// <param name="clientX"></param>
        /// <param name="clientY"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static Object SimulateEventHtml5(IWebElement dragFrom, String eventName, int clientX, int clientY, Object data)
        {
            return WebDriverContext.JsExecutor.ExecuteScript(_simulateEvent, dragFrom, eventName, clientX, clientY, data);
        }

        /// <summary>
        /// Calls a drag event
        /// </summary>
        /// <param name="dragFrom"></param>
        /// <param name="eventName"></param>
        /// <param name="mousePosition"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static Object SimulateEventHtml5(IWebElement dragFrom, String eventName, Position mousePosition, Object data)
        {
            Point fromLocation = dragFrom.Location;
            Size fromSize = dragFrom.Size;

            // Get Client X and Client Y locations
            int clientX = fromLocation.X + (fromSize == null ? 0 : GetX(mousePosition, fromSize.Width));
            int clientY = fromLocation.Y + (fromSize == null ? 0 : GetY(mousePosition, fromSize.Height));

            return SimulateEventHtml5(dragFrom, eventName, clientX, clientY, data);
        }

        /// <summary>
        /// Drags and drops a web element from source to target
        /// </summary>
        /// <param name="dragFrom"></param>
        /// <param name="dragTo"></param>
        /// <param name="dragFromX"></param>
        /// <param name="dragFromY"></param>
        /// <param name="dragToX"></param>
        /// <param name="dragToY"></param>
        public static void DragAndDropHtml5(IWebElement dragFrom, IWebElement dragTo, int dragFromX, int dragFromY, int dragToX, int dragToY)
        {
            WebDriverContext.JsExecutor.ExecuteScript(_simulateHTML5DragAndDrop, dragFrom, dragTo, dragFromX, dragFromY, dragToX, dragToY);
        }

        /// <summary>
        ///  Drags and drops a web element from source to target
        /// </summary>
        /// <param name="dragFrom"></param>
        /// <param name="dragTo"></param>
        /// <param name="dragFromPosition"></param>
        /// <param name="dragToPosition"></param>
        public static void DragAndDropHtml5(IWebElement dragFrom, IWebElement dragTo, Position dragFromPosition, Position dragToPosition)
        {
            Point fromLocation = dragFrom.Location;
            Point toLocation = dragTo.Location;
            Size fromSize = dragFrom.Size;
            Size toSize = dragTo.Size;

            // Get Client X and Client Y locations
            int dragFromX = fromLocation.X + (fromSize == null ? 0 : GetX(dragFromPosition, fromSize.Width));
            int dragFromY = fromLocation.Y + (fromSize == null ? 0 : GetY(dragFromPosition, fromSize.Height));
            int dragToX = toLocation.X + (toSize == null ? 0 : GetX(dragToPosition, toSize.Width));
            int dragToY = toLocation.Y + (toSize == null ? 0 : GetY(dragToPosition, toSize.Height));

            DragAndDropHtml5(dragFrom, dragTo, dragFromX, dragFromY, dragToX, dragToY);
        }
    }
}
