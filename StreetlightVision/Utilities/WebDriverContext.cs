using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;

namespace StreetlightVision.Utilities
{
    public enum DriverType
    {
        IE,
        Chrome,
        Safari,
        FF,
        Edge
    }

    public class WebDriverContext
    {
        private static Dictionary<string, IWebDriver> drivers = new Dictionary<string, IWebDriver>();

        public static IWebDriver CurrentDriver
        {
            get
            {
                return drivers[Settings.CurrentTestWebDriverKeyName];
            }
        }
        
        public static WebDriverWait Wait { get; set; }

        public static IJavaScriptExecutor JsExecutor
        {
            get
            {
                return (IJavaScriptExecutor)CurrentDriver; 
            }
        }

        public static Dictionary<string, IWebDriver> WebDrivers
        {
            get
            {
                if (drivers == null)
                {
                    drivers = new Dictionary<string, IWebDriver>();
                }

                return drivers;
            }
        }

        public static void NewDriverForCurrentTest(string language = "en")
        {
            IWebDriver driver = CreateWebDriver(language);
            //IWebDriver driver = CreateBrowserStackRemoteWebDriver(language);
            //IWebDriver driver = CreateSauceLabsRemoteWebDriver(language);

            if (drivers.ContainsKey(Settings.CurrentTestWebDriverKeyName))
            {
                drivers[Settings.CurrentTestWebDriverKeyName] = driver;
            }
            else
            {
                drivers.Add(Settings.CurrentTestWebDriverKeyName, driver);
            }
        }

        public static void RenewCurrentDriverWithNewLanguage(string language = "en")
        {
            IWebDriver driver = CreateWebDriver(language);
            //IWebDriver driver = CreateBrowserStackRemoteWebDriver(language);
            //IWebDriver driver = CreateSauceLabsRemoteWebDriver(language);

            if (drivers.ContainsKey(Settings.CurrentTestWebDriverKeyName))
            {
                drivers[Settings.CurrentTestWebDriverKeyName].Quit();

                drivers[Settings.CurrentTestWebDriverKeyName] = driver;
            }
            else
            {
                drivers.Add(Settings.CurrentTestWebDriverKeyName, driver);
            }
        }

        public static void QuitDriverOfCurrentTest()
        {
            if (drivers.ContainsKey(Settings.CurrentTestWebDriverKeyName))
            {
                var currentDriver = drivers[Settings.CurrentTestWebDriverKeyName];

                if (currentDriver != null)
                {
                    currentDriver.Close();
                    currentDriver.Quit();
                    currentDriver.Dispose();
                    drivers.Remove(Settings.CurrentTestWebDriverKeyName);
                }
            }                          
        }

        private static IWebDriver CreateWebDriver(string language)
        {
            var browser = Settings.Browser;
            var type = (DriverType)Enum.Parse(typeof(DriverType), browser);
            var driverPath = Settings.AssemblyPath;
            IWebDriver driver = null;

            switch (type)
            {
                case DriverType.IE:
                    var ieDriverService = InternetExplorerDriverService.CreateDefaultService(driverPath);
                    var ieOptions = new InternetExplorerOptions();
                    ieOptions.IntroduceInstabilityByIgnoringProtectedModeSettings = true;
                    ieOptions.PageLoadStrategy = PageLoadStrategy.Eager;
                    ieOptions.UnhandledPromptBehavior = UnhandledPromptBehavior.Accept;
                    ieOptions.IgnoreZoomLevel = true;

                    driver = new InternetExplorerDriver(ieDriverService, ieOptions, TimeSpan.FromMilliseconds(Settings.DriverWaitingTimeout));

                    break;

                case DriverType.Chrome:
                    var chromeOptions = new ChromeOptions();
                    chromeOptions.AddArguments("--disable-extensions");
                    chromeOptions.AddArguments("--disable-infobars");
                    chromeOptions.AddArguments("--ignore-certificate-errors");
                    chromeOptions.AddArguments(string.Format("--lang={0}", language));
                    chromeOptions.AddUserProfilePreference("profile.default_content_setting_values.automatic_downloads", 1);

                    driver = new ChromeDriver(driverPath, chromeOptions, TimeSpan.FromMilliseconds(Settings.DriverWaitingTimeout));

                    break;

                case DriverType.FF:
                    var firefoxDriverService = FirefoxDriverService.CreateDefaultService(driverPath);
                    var firefoxOptions = new FirefoxOptions();
                    var firefoxProfile = new FirefoxProfile();
                    firefoxDriverService.FirefoxBinaryPath = Settings.FirefoxBinaryPath;
                    firefoxProfile.AcceptUntrustedCertificates = true;
                    firefoxProfile.SetPreference("browser.download.dir", Settings.DownloadsPath);
                    firefoxProfile.SetPreference("browser.download.folderList", 2);
                    firefoxProfile.SetPreference("browser.helperApps.neverAsk.saveToDisk", "application/octet-stream doc xls pdf txt csv");
                    firefoxProfile.SetPreference("pdfjs.disabled", true);
                    firefoxProfile.SetPreference("geo.enabled", true);
                    firefoxProfile.SetPreference("geo.provider.use_corelocation", true);
                    firefoxProfile.SetPreference("geo.prompt.testing", true);
                    firefoxProfile.SetPreference("geo.prompt.testing.allow", true);
                    firefoxOptions.AcceptInsecureCertificates = true;
                    firefoxOptions.Profile = firefoxProfile;

                    driver = new FirefoxDriver(firefoxDriverService, firefoxOptions, TimeSpan.FromMilliseconds(Settings.DriverWaitingTimeout));

                    break;

                case DriverType.Edge:
                    driver = new EdgeDriver();

                    break;

                default:
                    driver = new ChromeDriver();
                    break;
            }

            if (driver == null)
            {
                throw new InvalidOperationException(string.Format("{0} is an invalid or unsupported WebDriver. Please check BROWSER value in App.config", driver.ToString()));
            }
           
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(500);
            driver.Manage().Timeouts().AsynchronousJavaScript = TimeSpan.FromSeconds(10);
            driver.Manage().Window.Maximize();
            Wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(Settings.DriverWaitingTimeout));

            return driver;
        }

        private static IWebDriver CreateBrowserStackRemoteWebDriver(string language)
        {
            var browser = Settings.Browser;
            var type = (DriverType)Enum.Parse(typeof(DriverType), browser);
            var driverPath = Settings.AssemblyPath;
            IWebDriver driver = null;
            DesiredCapabilities capability = new DesiredCapabilities();           

            switch (type)
            {
                case DriverType.IE:
                    capability.SetCapability("browser", "IE");
                    capability.SetCapability("browser_version", "11.0");
                    capability.SetCapability("browserstack.ie.noFlash", "true");
                    capability.SetCapability("browserstack.ie.enablePopups", "true");
                    break;

                case DriverType.Chrome:                    
                    var chromeOptions = new ChromeOptions();
                    chromeOptions.AddArgument("--disable-plugins");
                    chromeOptions.AddArguments("--disable-extensions");
                    chromeOptions.AddArguments("--disable-infobars");
                    chromeOptions.AddArguments("--no-sandbox");
                    chromeOptions.AddArguments("--ignore-certificate-errors");
                    chromeOptions.AddArguments(string.Format("--lang={0}", language));

                    capability = (DesiredCapabilities)chromeOptions.ToCapabilities();
                    capability.SetCapability("browser", "Chrome");
                    break;

                case DriverType.FF:
                    capability.SetCapability("browser", "Firefox");
                    break;

                case DriverType.Edge:
                    capability.SetCapability("browser", "Edge");
                    break;

                default:
                    break;
            }

            capability.SetCapability("browserstack.user", "cuongbui7");
            capability.SetCapability("browserstack.key", "fE39pazafyVuu8xUp5cQ");
            capability.SetCapability("browserstack.debug", "true");
            capability.SetCapability("browserstack.local", "true"); //Must run Local Test
            driver = new RemoteWebDriver(new Uri("http://hub-cloud.browserstack.com/wd/hub/"), capability);

            if (driver == null)
            {
                throw new InvalidOperationException(string.Format("{0} is an invalid or unsupported WebDriver. Please check BROWSER value in App.config", driver.ToString()));
            }

            //driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(500);
            //driver.Manage().Timeouts().AsynchronousJavaScript = TimeSpan.FromSeconds(10);

            return driver;
        }

        private static IWebDriver CreateSauceLabsRemoteWebDriver(string language)
        {
            var browser = Settings.Browser;
            var type = (DriverType)Enum.Parse(typeof(DriverType), browser);
            var driverPath = Settings.AssemblyPath;
            IWebDriver driver = null;
            DesiredCapabilities capability = new DesiredCapabilities();

            switch (type)
            {
                case DriverType.IE:
                    capability.SetCapability(CapabilityType.BrowserName, "Internet Explorer");
                    capability.SetCapability(CapabilityType.Version, "11");
                    break;

                case DriverType.Chrome:
                    capability.SetCapability(CapabilityType.BrowserName, "Chrome");
                    break;

                case DriverType.FF:
                    capability.SetCapability(CapabilityType.BrowserName, "Firefox");
                    break;

                case DriverType.Edge:
                    capability.SetCapability(CapabilityType.BrowserName, "Microsoft Edge");
                    break;

                default:
                    break;
            }
            
            capability.SetCapability("username", "cbui");
            capability.SetCapability("accessKey", "a3ca1ec7-a06d-405f-9d80-286fee951178");
            capability.SetCapability("name", Settings.CurrentTestWebDriverKeyName);
            driver = new RemoteWebDriver(new Uri("http://ondemand.saucelabs.com/wd/hub"), capability, TimeSpan.FromMilliseconds(Settings.DriverWaitingTimeout));

            if (driver == null)
            {
                throw new InvalidOperationException(string.Format("{0} is an invalid or unsupported WebDriver. Please check BROWSER value in App.config", driver.ToString()));
            }

            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(500);
            driver.Manage().Timeouts().AsynchronousJavaScript = TimeSpan.FromSeconds(10);

            return driver;
        }

        /// <summary>
        /// Set timeout for current webdriver wait
        /// </summary>
        /// <param name="minute"></param>
        public static void SetTimeOut(int minute = 1)
        {
            int milliseconds = Settings.DriverWaitingTimeout * minute;
            Wait = new WebDriverWait(CurrentDriver, TimeSpan.FromMilliseconds(milliseconds));
        }
    }
}
