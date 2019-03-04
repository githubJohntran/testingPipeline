using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;
using StreetlightVision.Extensions;
using StreetlightVision.Pages;
using System.Linq;

namespace StreetlightVision.Utilities
{
    public class Browser
    {
        private const string MAIN_FRAME_NAME = "mainFrame";

        /// <summary>
        /// Name of Browser: Chrome, IE, FF
        /// </summary>
        public static string Name
        {
            get
            {
                return Settings.Browser;
            }
        }

        /// <summary>
        /// Navigate To StreetlightVision page
        /// </summary>
        /// <returns>LoginPage instance</returns>
        public static LoginPage OpenCMS()
        {
            return OpenSlvUrl(Settings.SlvUrl);
        }

        /// <summary>
        ///  Navigate to CMS page (already logged in)
        /// </summary>
        /// <returns></returns>
        public static DesktopPage NavigateToLoggedInCMS()
        {
            WebDriverContext.CurrentDriver.Navigate().GoToUrl(Settings.SlvUrl);
            if (Settings.SlvUrl.StartsWith("https") && Name.Equals("IE"))
            {
                IgnoreIfNotSecure();
            }

            WebDriverContext.CurrentDriver.SwitchTo().DefaultContent();
            WebDriverContext.Wait.Until(ExpectedConditions.FrameToBeAvailableAndSwitchToIt(MAIN_FRAME_NAME));

            return new DesktopPage(WebDriverContext.CurrentDriver);
        }

        /// <summary>
        /// Navigate to Back Office App (not login yet)
        /// </summary>
        /// <returns></returns>
        public static LoginPage OpenBackOfficeApp()
        {
            var url = Settings.SlvUrl.EndsWith("/") ? Settings.SlvUrl.TrimEnd('/') : Settings.SlvUrl;
            url = string.Format("{0}/groundcontrol/?app=BackOffice", url);

            WebDriverContext.CurrentDriver.Navigate().GoToUrl(url);
            if (url.StartsWith("https") && Name.Equals("IE"))
            {
                IgnoreIfNotSecure();
            }

            return new LoginPage(WebDriverContext.CurrentDriver);
        }

        /// <summary>
        ///  Navigate to Back Office App (already loggedin)
        /// </summary>
        /// <returns></returns>
        public static BackOfficePage NavigateToLoggedInBackOfficeApp()
        {
            var url = Settings.SlvUrl.EndsWith("/") ? Settings.SlvUrl.TrimEnd('/') : Settings.SlvUrl;
            url = string.Format("{0}/groundcontrol/?app=BackOffice", url);
            WebDriverContext.CurrentDriver.Navigate().GoToUrl(url);

            return new BackOfficePage(WebDriverContext.CurrentDriver);
        }

        /// <summary>
        /// Refresh StreetlightVision page
        /// </summary>
        /// <returns></returns>
        public static DesktopPage RefreshLoggedInCMS()
        {
            WebDriverContext.CurrentDriver.Navigate().Refresh();
            WebDriverContext.CurrentDriver.SwitchTo().DefaultContent();
            WebDriverContext.Wait.Until(ExpectedConditions.FrameToBeAvailableAndSwitchToIt(MAIN_FRAME_NAME));

            return new DesktopPage(WebDriverContext.CurrentDriver);
        }

        /// <summary>
        /// Refresh to Back Office page
        /// </summary>
        /// <returns></returns>
        public static BackOfficePage RefreshBackOfficePage()
        {
            WebDriverContext.CurrentDriver.Navigate().Refresh();

            return new BackOfficePage(WebDriverContext.CurrentDriver);
        }

        /// <summary>
        /// Refresh to Login page
        /// </summary>
        /// <returns></returns>
        public static LoginPage RefreshLoginPage()
        {
            WebDriverContext.CurrentDriver.Navigate().Refresh();
            WebDriverContext.CurrentDriver.SwitchTo().DefaultContent();
            WebDriverContext.Wait.Until(ExpectedConditions.FrameToBeAvailableAndSwitchToIt(MAIN_FRAME_NAME));

            return new LoginPage(WebDriverContext.CurrentDriver);
        }

        /// <summary>
        /// Get current window handle of driver
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentWindowHandles()
        {
            return WebDriverContext.CurrentDriver.CurrentWindowHandle;
        }

        /// <summary>
        /// Open new tab/window
        /// </summary>
        /// <returns>new Tab/Window Handles</returns>
        public static string OpenNewTab()
        {
            // execute some JavaScript to open a new window
            WebDriverContext.JsExecutor.ExecuteScript("window.open();");
            Wait.ForSeconds(2); //Wait for new window/tab to init completely.
            // save a reference to our new tab's window handle, this would be the last entry in the WindowHandles collection
            var newTabWindowHandles = WebDriverContext.CurrentDriver.WindowHandles[WebDriverContext.CurrentDriver.WindowHandles.Count - 1];
            // switch our WebDriver to the new tab's window handle
            WebDriverContext.CurrentDriver.SwitchTo().Window(newTabWindowHandles);
            WebDriverContext.CurrentDriver.Manage().Window.Maximize();

            return newTabWindowHandles;
        }        

        /// <summary>
        /// Switch to specific windowHandle
        /// </summary>
        /// <param name="windownHandle"></param>
        public static void SwitchTo(string windownHandle)
        {
            WebDriverContext.CurrentDriver.SwitchTo().Window(windownHandle);
            WebDriverContext.CurrentDriver.SwitchTo().DefaultContent();
            WebDriverContext.CurrentDriver.SwitchTo().Frame(MAIN_FRAME_NAME);
        }

        /// <summary>
        /// Open SLV URL
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private static LoginPage OpenSlvUrl(string url)
        {
            WebDriverContext.CurrentDriver.Navigate().GoToUrl(url);
            if (url.StartsWith("https") && Name.Equals("IE"))
            {
                IgnoreIfNotSecure();
            }

            try
            {
                WebDriverContext.CurrentDriver.SwitchTo().DefaultContent();
                WebDriverContext.CurrentDriver.SwitchTo().Frame(MAIN_FRAME_NAME);
                if(Name.Equals("IE")) WebDriverContext.Wait.Until(ExpectedConditions.FrameToBeAvailableAndSwitchToIt(MAIN_FRAME_NAME));
            }
            catch (NoSuchFrameException ex)
            {
                WebDriverContext.CurrentDriver.Navigate().Refresh();
                WebDriverContext.Wait.Until(ExpectedConditions.FrameToBeAvailableAndSwitchToIt(MAIN_FRAME_NAME));
            }

            return new LoginPage(WebDriverContext.CurrentDriver);
        }

        private static void IgnoreIfNotSecure()
        {
            if (WebDriverContext.CurrentDriver.FindElements(By.CssSelector("[id='moreInfoContainer']")).Any())
            {
                WebDriverContext.CurrentDriver.FindElement(By.CssSelector("[id='moreInfoContainer'] a")).ClickByJS();
                WebDriverContext.CurrentDriver.Navigate().GoToUrl("javascript:document.getElementById('overridelink').click()");
            }
        }        
    }
}
