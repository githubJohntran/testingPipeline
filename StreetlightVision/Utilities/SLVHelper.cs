using AutoIt;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using StreetlightVision.Extensions;
using StreetlightVision.Models;
using StreetlightVision.Pages;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace StreetlightVision.Utilities
{
    public static class SLVHelper
    {
        #region Variables

        private static Random _random = new Random();

        #endregion //Variables

        #region Constants

        private const string SWITCH_APPS_FILE_PATH = @"Resources\img\apps\{0}\switchIcon.png";
        private const string APPS_DRAG_DROP_FILE_PATH = @"Resources\img\apps\{0}\dragdrop.png";
        private const string WIDGETS_DRAG_DROP_FILE_PATH = @"Resources\img\widgets\{0}\dragdrop.png";
        private const string IE_FILE_PATH = @"Resources\img\browsers\IE\{0}";
        private const string GET_ELEMENTS_FILE_HELPER_PATH = @"Resources\js\get_element_helper.js";
        private const string MAP_DEVICE_PATH = @"Resources\img\map_device";
        private const string SCREENSHOT_PATH = @"Resources\img\{0}_screenshots";

        #endregion //Constants        

        #region Public methods

        /// <summary>
        /// Delete all files by pattern
        /// </summary>
        /// <param name="pattern"></param>
        public static void DeleteAllFilesByPattern(string pattern)
        {
            foreach (var exportedFile in Directory.GetFiles(Settings.DownloadsPath, pattern))
            {
                File.Delete(exportedFile);
            }
        }

        /// <summary>
        /// Check a pattern file is existing
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static bool CheckFileExists(string pattern)
        {
            try
            {
                WebDriverContext.Wait.Until(driver => Directory.GetFiles(Settings.DownloadsPath, pattern).Length > 0);
                var files = Directory.GetFiles(Settings.DownloadsPath, pattern);

                return files.Length > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Get path of the first found file with pattern
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static string GetPathOfFileByPattern(string pattern)
        {
            try
            {
                WebDriverContext.Wait.Until(driver => Directory.GetFiles(Settings.DownloadsPath, pattern).Length > 0);

                var files = Directory.GetFiles(Settings.DownloadsPath, pattern);

                return files.Length > 0 ? files[0] : string.Empty;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Click the save button appeared when saving/exporting a file to local
        /// </summary>
        public static void SaveDownloads(bool isFirstSaved = true)
        {
            try
            {
                var browser = Settings.Browser;
                var type = (DriverType)Enum.Parse(typeof(DriverType), browser);
                string frame = string.Empty;
                string winSaveDialogTitle = string.Empty;

                switch (type)
                {
                    case DriverType.IE:
                        frame = "[Class:IEFrame]";
                        winSaveDialogTitle = "Internet Explorer";
                        Wait.ForSeconds(3);
                        AutoItX.Init();
                        AutoItX.WinActivate(frame);
                        Wait.ForSeconds(2);
                        AutoItX.ControlSend(frame, "", "[Class:DirectUIHWND; Instance:1]", "!{S DOWN}");
                        AutoItX.WinActivate(winSaveDialogTitle);
                        AutoItX.ControlClick(winSaveDialogTitle, "", "[Class:Button; Instance:2]");                       
                        Wait.ForSeconds(2);
                        break;

                    case DriverType.FF:                        
                        frame = "[Class:MozillaDialogClass]";
                        Wait.ForSeconds(3);
                        if (AutoItX.WinExists(frame) == 1)
                        {
                            Wait.ForSeconds(2);
                            AutoItX.Init();
                            AutoItX.WinActivate(frame);
                            if (isFirstSaved)
                            {
                                AutoItX.Send("{DOWN}");
                                Wait.ForSeconds(1);
                            }
                            AutoItX.Send("{ENTER}");
                            Wait.ForSeconds(2);
                        }
                        break;
                }       
            }
            catch (Exception ex)
            {
                throw new Exception("Error Save File", ex);
            }
        }

        /// <summary>
        /// Click Yes on Security Alert
        /// </summary>
        public static void AllowSecurityAlert()
        {
            if (Browser.Name.Equals("IE"))
            {
                try
                {
                    string frame = "[Class:IEFrame]";
                    string winSecurityAlertTitle = "Security Alert";

                    Wait.ForSeconds(3);
                    AutoItX.Init();
                    AutoItX.WinActivate(frame);
                    Wait.ForSeconds(2);
                    while (AutoItX.WinExists(winSecurityAlertTitle) == 1)
                    {
                        AutoItX.WinActivate(winSecurityAlertTitle);
                        Wait.ForSeconds(2);
                        AutoItX.ControlClick(winSecurityAlertTitle, "", "[Class:Button; Instance:1]");
                        Wait.ForSeconds(3);
                    }
                    AutoItX.WinActivate(frame);
                }
                catch (Exception ex) { throw new Exception("Error Click Yes on Security Alert", ex); }
            }
        }

        public static void AllowOnceLocation()
        {
            if (Browser.Name.Equals("IE"))
            {
                try
                {
                    string frame = "[Class:IEFrame]";

                    Wait.ForSeconds(3);
                    AutoItX.Init();
                    AutoItX.WinActivate(frame);
                    Wait.ForSeconds(2);
                    AutoItX.ControlClick(frame, "", "[Class:DirectUIHWND; Instance:1]", "left", 1, 715, 28);
                    Wait.ForSeconds(1);
                    AutoItX.Send("{ENTER}");
                    Wait.ForSeconds(2);
                }
                catch (Exception ex) { throw new Exception("Error Allow Once", ex); }
            }
        }

        /// <summary>
        /// Fill the file name into Open File dialog and hit Enter
        /// </summary>
        public static void OpenFileFromFileDialog(string fileName)
        {
            try
            {
                var browser = Settings.Browser;
                var type = (DriverType)Enum.Parse(typeof(DriverType), browser);
                string frame = string.Empty;
                string winUploadDialogTitle = string.Empty;

                switch (type)
                {
                    case DriverType.Chrome:
                        frame = "Chrome_WidgetWin_1";
                        winUploadDialogTitle = "Open";
                        break;
                    case DriverType.IE:
                        frame = "[Class:IEFrame]";
                        winUploadDialogTitle = "Choose File to Upload";
                        break;
                    case DriverType.FF:
                        frame = "[Class:MozillaWindowClass]";
                        winUploadDialogTitle = "File Upload";
                        break;
                }
                Wait.ForSeconds(3);
                AutoItX.Init();
                AutoItX.WinActivate(frame);
                AutoItX.WinWait(winUploadDialogTitle);
                AutoItX.WinActivate(winUploadDialogTitle);
                Wait.ForSeconds(2);
                AutoItX.Send(fileName);
                Wait.ForSeconds(3);
                AutoItX.ControlClick(winUploadDialogTitle, "", "Button1");
                Wait.ForSeconds(2);
            }
            catch (Exception ex)
            {
                throw new Exception("Error Upload File", ex);
            }
        }        

        /// <summary>
        /// Get data of latest exported CSV
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static DataTable BuildDataTableFromLastDownloadedCSV(string pattern)
        {
            DataTable tblCSV = CSVUtility.BuildDataTableFromCSV(GetPathOfFileByPattern(pattern));

            return tblCSV;
        }

        /// <summary>
        /// Get header line of latest exported CSV
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static string GetHeaderLineFromDownloadedCSV(string pattern)
        {
            var headerLine = CSVUtility.GetHeaderLineFromCSV(GetPathOfFileByPattern(pattern));

            return headerLine;
        }

        /// <summary>
        /// Generate a unique name for an item (Report, Alarm, Device, etc.) under a pattern
        /// so that a script can decide the named item may be deleted
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static string GenerateUniqueName(string prefix)
        {
            return prefix + DateTime.Now.ToBinary();
        }

        /// <summary>
        /// Generate a random string with specific length
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GenerateString(int length = 5)
        {
            const string chars = "AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz";
            if (length >= chars.Length) return chars;
            return new string(Enumerable.Repeat(chars, length).Select(s => s[_random.Next(s.Length)]).ToArray());
        }

        /// <summary>
        /// Generate a random special string with specific length
        /// </summary>
        /// <param name="length">Maximum length = 14</param>
        /// <returns></returns>
        public static string GenerateSpecialString(int length = 3)
        {
            const string chars = @"~!@#$%^&*()-+$";
            if (length > chars.Length) throw new Exception("Length must be less than 14 chars");
            return new string(Enumerable.Repeat(chars, length).Select(s => s[_random.Next(s.Length)]).ToArray());
        }
        
        /// <summary>
        /// Generate a random string mixed number with specific length
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GenerateStringMixedNumber(int length = 5)
        {
            const string chars = "AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz0123456789";
            if (length >= chars.Length) return chars;
            return new string(Enumerable.Repeat(chars, length).Select(s => s[_random.Next(s.Length)]).ToArray());
        }

        /// <summary>
        /// Generate a random MAC Address
        /// </summary>
        /// <returns></returns>
        public static string GenerateMACAddress()
        {
            var sBuilder = new StringBuilder();
            int number;
            byte b;
            for (int i = 0; i < 6; i++)
            {
                number = _random.Next(0, 255);
                b = Convert.ToByte(number);
                if (i == 0)
                {
                    b = SetBit(b, 6); //--> set locally administered
                    b = UnsetBit(b, 7); // --> set unicast 
                }
                sBuilder.Append(number.ToString("X2"));
            }
            return sBuilder.ToString().ToUpper();
        }

        private static byte SetBit(byte b, int BitNumber)
        {
            if (BitNumber < 8 && BitNumber > -1)
            {
                return (byte)(b | (byte)(0x01 << BitNumber));
            }
            else
            {
                throw new InvalidOperationException("Invalid BitNumber " + BitNumber.ToString() + "(BitNumber = (min)0 - (max)7)");
            }
        }

        private static byte UnsetBit(byte b, int BitNumber)
        {
            if (BitNumber < 8 && BitNumber > -1)
            {
                return (byte)(b | (byte)(0x00 << BitNumber));
            }
            else
            {
                throw new InvalidOperationException("Invalid BitNumber " + BitNumber.ToString() + " (BitNumber = (min)0 - (max)7)");
            }
        }

        /// <summary>
        /// Generate a random integer with specific maxValue
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static int GenerateInteger(int maxValue = 999)
        {
            return _random.Next(1, maxValue);
        }

        /// <summary>
        /// Generate a random integer with specific from minValue to  maxValue
        /// </summary>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        public static int GenerateInteger(int minValue = 1, int maxValue = 999)
        {
            return _random.Next(minValue, maxValue);
        }

        /// <summary>
        /// Generate a random string integer with specific maxValue
        /// </summary>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        public static string GenerateStringInteger(int maxValue = 999)
        {
            return GenerateInteger(maxValue).ToString();
        }

        /// <summary>
        /// Generate a random string integer with specific from minValue to  maxValue
        /// </summary>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        public static string GenerateStringInteger(int minValue = 1, int maxValue = 999)
        {
            return GenerateInteger(minValue, maxValue).ToString();
        }

        /// <summary>
        /// Generate a random double between 0.0 to 1.0
        /// </summary>
        /// <returns></returns>
        public static double GenerateDouble()
        {
            return _random.NextDouble();
        }

        /// <summary>
        /// Generate a random string double between 0.0 to 1.0
        /// </summary>
        /// <returns></returns>
        public static string GenerateStringDouble()
        {
            return GenerateDouble().ToString();
        }

        /// <summary>
        /// Generate a random string Decimal
        /// </summary>
        /// <param name="digits"></param>
        /// <param name="decimals"></param>
        /// <returns></returns>
        public static string GenerateStringDecimal(int digits, int decimals)
        {
            return GenerateDecimal(digits, decimals).ToString();
        }

        /// <summary>
        /// Generate a random Decimal
        /// </summary>
        /// <param name="digits"></param>
        /// <param name="decimals"></param>
        /// <returns></returns>
        public static decimal GenerateDecimal(int digits, int decimals)
        {
            var intValue = _random.Next((int)Math.Pow(10, digits + decimals - 1), (int)Math.Pow(10, digits + decimals) - 1);

            return (decimal)(intValue / Math.Pow(10, decimals));
        }

        /// <summary>
        /// Generate a list number from min to max with pading number (e.g padZero = 2, 00 01 02 03 ...)
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static List<string> GenerateListStringInterger(int min, int max, int padZero = 2)
        {
            string padValue = "D" + padZero;
            var result = new List<string>();

            for (int i = min; i <= max; i++)
            {
                result.Add(string.Format("{0:" + padValue + "}", i));
            }

            return result;
        }

        /// <summary>
        /// Generate a list time from min to max r (e.g 00:00, 10:00, 21:00)
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static List<string> GenerateListStringTime(int min, int max)
        {
            var result = new List<string>();

            for (int i = min; i <= max; i++)
            {
                result.Add(string.Format("{0:D2}:00", i));
            }

            return result;
        }
        
        public static List<string> GenerateTimezone()
        {
            var result = new List<string>();
            for (int i = 1; i <= 12; i++)
            {
                result.Add(string.Format("GMT-{0:D2}:00", i));
                result.Add(string.Format("GMT+{0:D2}:00", i));
            }

            return result;
        }

        /// <summary>
        /// Create a random hex color (e.g. #00123456)
        /// </summary>
        /// <returns></returns>
        public static string GenerateHexColor()
        {
            return String.Format("#{0:X2}{1:X6}", _random.Next(0x100), _random.Next(0x1000000));
        }

        /// <summary>
        /// Create a random coordinate (latitude/longitude) wih min value and max value
        /// Ex: min="48.26468", max="48.26670"
        /// </summary>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        public static string GenerateCoordinate(string minValue, string maxValue)
        {
            var minA = int.Parse(minValue.SplitAndGetAt(".", 0));
            var minB = int.Parse(minValue.SplitAndGetAt(".", 1));
            var maxA = int.Parse(maxValue.SplitAndGetAt(".", 0));
            var maxB = int.Parse(maxValue.SplitAndGetAt(".", 1));

            return string.Format("{0}.{1}", minA > maxA ? maxA : minA, GenerateStringInteger(minB, maxB));
        }

        /// <summary>
        ///  Create a random latitude
        /// </summary>
        /// <returns></returns>
        public static string GenerateLatitude(int min = 1, int max = 89)
        {
            return string.Format("{0}.{1}", GenerateStringInteger(min, max), GenerateStringInteger(10000, 30000));
        }

        /// <summary>
        ///  Create a random longitude
        /// </summary>
        /// <returns></returns>
        public static string GenerateLongitude(int min = 1, int max = 179)
        {
            return string.Format("{0}.{1}", GenerateStringInteger(min, max), GenerateStringInteger(10000, 30000));
        }
       
        public static bool IsServerFileExists(string url)
        {
            InitServicePointManager();
            var cookies = new CookieContainer();
            var token = GetToken(ref cookies);
            var request = (HttpWebRequest)WebRequest.Create(url);           
            request.Credentials = GetNetworkCredential();
            request.CookieContainer = cookies;
            request.Method = "HEAD";
            request.Headers.Add("X-CSRF-Token", token);
            Console.WriteLine("Send request: {0}", url);
            var exists = false;
            try
            {
                request.GetResponse();
                exists = true;
            }
            catch (WebException ex)
            {
                exists = false;
            }

            return exists;
        }

        public static byte[] DownloadFileData(string url)
        {
            InitServicePointManager();
            byte[] data;            
            var cookies = new CookieContainer();
            var token = GetToken(ref cookies);
            var request = (HttpWebRequest) WebRequest.Create(url);
            request.Credentials = GetNetworkCredential();
            request.CookieContainer = cookies;
            request.Method = "HEAD";
            request.Headers.Add("X-CSRF-Token", token);
            Console.WriteLine("Send request: {0}", url);
            var exists = false;
            try
            {
                request.GetResponse();
                exists = true;
            }
            catch
            {
                exists = false;
                Assert.Warn(string.Format("File '{0}' not found in server", url));
            }

            if (exists)
            {
                var client = new WebClient();
                data = client.DownloadData(url);

                return data;
            }

            return new byte[0];
        }

        /// <summary>
        /// Logout current user and login again with specific user and password
        /// </summary>
        /// <param name="page"></param>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static DesktopPage LogoutAndLogin(PageBase page, string user, string password)
        {
            page.AppBar.ClickSettingsButton();
            page.WaitForSettingsPanelDisplayed();
            page.SettingsPanel.ClickLogoutLink();
            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(user, password);

            return desktopPage;
        }

        /// <summary>
        /// Get apps display on Switcher
        /// </summary>
        /// <returns></returns>
        public static List<string> GetListOfSwitcherApps()
        {
            var result = new List<string>();
            var collection = (IReadOnlyCollection<object>)WebDriverContext.JsExecutor.ExecuteScript("return plugin.userContext.desktop.layout.header.headerInstance.appList");

            foreach (var item in collection)
            {
                var dic = (Dictionary<string, object>)item;
                result.Add(dic["name"].ToString());
            }

            return result;
        }

        /// <summary>
        /// Get current app displays on Switcher
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentAppSwitcher()
        {
            var app = WebDriverContext.JsExecutor.ExecuteScript("return plugin.userContext.desktop.layout.header.headerInstance.currview.app.name");
            
            return app.ToString();
        }

        /// <summary>
        /// Get apps/widgets name display on Desktop with sort index
        /// </summary>
        /// <returns></returns>
        public static List<string> GetListOfDesktopTilesName()
        {
            var result = new List<string>();
            var collection = (IReadOnlyCollection<object>)WebDriverContext.JsExecutor.ExecuteScript(@"var apps = plugin.userContext.desktop.applications;
                                                                                                      var result =[];
                                                                                                      for (var i = 0; i < apps.length; i++)
                                                                                                      {
                                                                                                        result.push(apps[i].app.name);
                                                                                                      }
                                                                                                      return result;");
            result = collection.Select(p => p.ToString()).ToList();

            return result;
        }

        /// <summary>
        /// Get apps with icon display on Switcher
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> GetListSwitcherAppsIcon()
        {
            var result = new Dictionary<string, string>();
            var collection = (IReadOnlyCollection<object>)WebDriverContext.JsExecutor.ExecuteScript("return plugin.userContext.desktop.layout.header.headerInstance.appList");

            foreach (var item in collection)
            {
                var dic = (Dictionary<string, object>)item;
                result.Add(dic["name"].ToString(), string.Format("{0}/groundcontrol/{1}", Settings.SlvUrl, dic["icon"].ToString()));
            }

            return result;
        }

        /// <summary>
        /// Covnert an App name from English to specific Language
        /// </summary>
        /// <param name="englishAppName"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        /// <remarks>This function can be replaced by API from SLV</remarks>
        public static string ConvertAppName(string englishAppName, string language)
        {
            switch(language)
            {
                case "English":
                    return englishAppName;
                case "French":                
                    switch(englishAppName)
                    {
                        case App.EquipmentInventory:
                            return "Équipements";
                        case App.RealTimeControl:
                            return "Contrôle Temps-Réel";
                        case App.FailureTracking:
                            return "Suivi de Panne";
                        case App.Users:
                            return "Utilisateurs";
                        case App.ReportManager:
                            return "Gestionnaire de rapports";
                        case App.BatchControl:
                            return "Commande de groupe";
                        case App.SchedulingManager:
                            return "Programmations horaires";
                    }
                    break;
            }
            return englishAppName;
        }

        public static string ConvertWeekdayNameToEnglish(string currentWeekdayName, string language)
        {
            switch (language)
            {
                case "French":
                    switch (currentWeekdayName)
                    {
                        case "Lundi":
                            return "Monday";
                        case "Mardi":
                            return "Tuesday";
                        case "Mercredi":
                            return "Wednesday";
                        case "Jeudi":
                            return "Thursday";
                        case "Vendredi":
                            return "Friday";
                        case "Samedi":
                            return "Saturday";
                        case "Dimanche":
                            return "Sunday";
                    }
                    break;
            }
            return currentWeekdayName;
        }

        /// <summary>
        /// Create a new file with name path and lines of content
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static bool CreateNewFile(string fileName, params string[] content)
        {
            try
            {
                var streamWriter = new StreamWriter(fileName, false);
                foreach (var line in content)
                {
                    streamWriter.WriteLine(line);
                }

                streamWriter.Flush();
                streamWriter.Close();
                streamWriter.Dispose();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Update a file with name path and lines of content
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static bool UpdateFile(string fileName, params string[] content)
        {
            try
            {
                var streamWriter = new StreamWriter(fileName, true);
                foreach (var line in content)
                {
                    streamWriter.WriteLine(line);
                }

                streamWriter.Flush();
                streamWriter.Close();
                streamWriter.Dispose();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        #region API methods

        #region Import CSV File

        /// <summary>
        /// Send importDevicesFromCsvFileAsync and getBatchResult to import a file and verify batchId returned
        /// </summary>
        /// <param name="fileNamePath"></param>
        /// <returns></returns>
        public static bool SendRequestDeviceImportFile(string fileNamePath)
        {
            var batchId = SendRequestDeviceImportFileAsync(fileNamePath).ToSync();

            return SendRequestGetBatchResult(batchId);
        }

        /// <summary>
        ///  Send importDevicesFromCsvFileAsync request to import a file
        /// </summary>
        /// <param name="fileNamePath"></param>
        /// <returns></returns>
        private static async Task<string> SendRequestDeviceImportFileAsync(string fileNamePath)
        {
            InitServicePointManager();
            var cookies = new CookieContainer();
            var token = GetToken(ref cookies);

            var url = GetFullUrl("/api/loggingmanagement/importDevicesFromCsvFileAsync");
            var request = (HttpWebRequest) WebRequest.Create(url);
            request.Credentials = GetNetworkCredential();
            request.CookieContainer = cookies;
            request.Method = "POST";
            request.Headers.Add("X-CSRF-Token", token);
            Console.WriteLine("Send request: {0}", url);
            var byteArray = File.ReadAllBytes(fileNamePath);
            using (Stream dataStream = await request.GetRequestStreamAsync())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Flush();
                dataStream.Close();
            }

            var response = (HttpWebResponse)await request.GetResponseAsync();
            using (var responseStream = response.GetResponseStream())
            {
                if (responseStream != null)
                {
                    var xmlUtility = new XmlUtility();
                    xmlUtility.Load(responseStream);

                    return xmlUtility.GetSingleNodeText("/SLVBatchResult/batchId");
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Send getBatchResult request to verify batchId is completed or not
        /// </summary>
        /// <param name="batchId"></param>
        /// <returns></returns>
        public static bool SendRequestGetBatchResult(string batchId)
        {
            var url = GetFullUrl(string.Format("/api/batch/getBatchResult?batchId={0}", batchId));
            var status = "ERROR";
            while (true)
            {
                Wait.ForSeconds(1);
                var response = SendRequest(url);
                using (var responseStream = response.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        var xmlUtility = new XmlUtility();
                        xmlUtility.Load(responseStream);
                        var batchRunning = bool.Parse(xmlUtility.GetSingleNodeText("/SLVBatchResult/batchRunning"));
                        status = xmlUtility.GetSingleNodeText("/SLVBatchResult/status");
                        if (!batchRunning) break;
                    }
                }
            }

            return status != "ERROR";
        }

        #endregion //Import CSV File

        #region Control Program

        /// <summary>
        /// Send getSchedules Request to get all control programs
        /// </summary>
        /// <returns>Dictionary<int:id, string:name> </returns>
        public static Dictionary<int, string> SendRequestGetControlPrograms()
        {
            var result = new Dictionary<int, string>();
            var url = GetFullUrl("/api/dimmingscheduler/getSchedules?category=STREETLIGHT_DIMMING");
            var response = SendRequest(url);
            if (response.StatusCode != HttpStatusCode.OK) return result;
            using (var responseStream = response.GetResponseStream())
            {
                if (responseStream != null)
                {
                    var xmlUtility = new XmlUtility();
                    xmlUtility.Load(responseStream);
                    var nodes = xmlUtility.GetNodes("/com.dotv.streetlightserver.api.data.SLVResult/value/com.slv.service.core.data.dto.SLVSchedule");
                    if (nodes == null) return result;
                    foreach (var node in nodes)
                    {
                        var idNode = node.SelectSingleNode("id");
                        var nameNode = node.SelectSingleNode("name");
                        if (idNode == null || nameNode == null) continue;
                        result.Add(int.Parse(idNode.InnerText.Trim()), nameNode.InnerText.Trim());
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Send createSchedule request to create new control program
        /// </summary>
        /// <param name="name">Control program name</param>
        /// <param name="timeline">Either T_12_12 for noon-to-noon or T_00_24 for midnight-to-midnight</param>
        /// <param name="hexColor">HTML code of the color used to represent the schedule on the user interface, including alpha channel (format: #xxxxxxxx)</param>
        /// <param name="description">description of the control program</param>
        /// <returns></returns>
        public static long SendRequestCreateControlProgram(string name, string timeline, string hexColor, string description, string template = "")
        {
            long value;
            var url = GetFullUrl(string.Format("/api/dimmingscheduler/createSchedule?name={0}&category=STREETLIGHT_DIMMING&timeline={1}&color={2}&description={3}&template={4}", name, timeline, hexColor.Replace("#", "%23"), description, template));
            var response = SendRequest(url);
            if (response.StatusCode != HttpStatusCode.OK) return -1;
            using (var responseStream = response.GetResponseStream())
            {
                if (responseStream != null)
                {
                    var xmlUtility = new XmlUtility();
                    xmlUtility.Load(responseStream);
                    var errorCodeNode = xmlUtility.GetSingleNode("/com.dotv.streetlightserver.api.data.SLVResult/errorCode");
                    var valueNode = xmlUtility.GetSingleNode("/com.dotv.streetlightserver.api.data.SLVResult/value");
                    if (errorCodeNode == null || valueNode == null) return -1;
                    if (errorCodeNode.InnerText.Equals("0") && long.TryParse(valueNode.InnerText, out value)) return value;
                }
            }

            return -1;
        }        

        /// <summary>
        /// Send addSunEventDayTimeScheduleEvent request to add a sun-based control program event
        /// </summary>
        /// <param name="id">Schedule ID (int)</param>
        /// <param name="level">Dimming level (0 - 100)</param>
        /// <param name="startSunEvent">Type of start event: SUNRISE or SUNSET</param>
        /// <param name="startOffsetInSeconds">Offset with the start event in seconds. A negative number indicates a number of seconds before the event, while a positive number indicates a number of seconds after the event</param>
        /// <returns></returns>
        public static bool SendRequestAddSunBasedControlProgram(long id, int level, string startSunEvent, int startOffsetInSeconds)
        {
            var url = GetFullUrl(string.Format("/api/dimmingscheduler/addSunEventDayTimeScheduleEvent?scheduleId={0}&level={1}&startSunEvent={2}&startOffsetInSeconds={3}", id, level, startSunEvent, startOffsetInSeconds));
            var response = SendRequest(url);

            if (response.StatusCode != HttpStatusCode.OK) return false;
            using (var responseStream = response.GetResponseStream())
            {
                if (responseStream != null)
                {
                    var xmlUtility = new XmlUtility();
                    xmlUtility.Load(responseStream);
                    var errorCodeNode = xmlUtility.GetSingleNode("/com.dotv.streetlightserver.api.data.SLVResult/errorCode");
                    if (errorCodeNode == null) return false;

                    return errorCodeNode.InnerText.Equals("0");                    
                }
            }

            return false;
        }

        /// <summary>
        /// Send addAbsoluteDayTimeScheduleEvent request to add an event to a control program 
        /// </summary>
        /// <param name="id">Schedule ID (int)</param>
        /// <param name="level">Dimming level (0 - 100)</param>
        /// <param name="startHour">Hour (0-23)</param>
        /// <param name="startMinute">Minutes (0-59)</param>
        /// <param name="startSecond">Seconds (0-59)</param>
        /// <returns></returns>
        public static bool SendRequestAddEventControlProgram(long id, int level, int startHour, int startMinute, int startSecond)
        {
            var url = GetFullUrl(string.Format("/api/dimmingscheduler/addAbsoluteDayTimeScheduleEvent?scheduleId={0}&level={1}&startHour={2}&startMinute={3}&startSecond={4}", id, level, startHour, startMinute, startSecond));
            var response = SendRequest(url);
            if (response.StatusCode != HttpStatusCode.OK) return false;
            using (var responseStream = response.GetResponseStream())
            {
                if (responseStream != null)
                {
                    var xmlUtility = new XmlUtility();
                    xmlUtility.Load(responseStream);
                    var errorCodeNode = xmlUtility.GetSingleNode("/com.dotv.streetlightserver.api.data.SLVResult/errorCode");
                    if (errorCodeNode == null) return false;

                    return errorCodeNode.InnerText.Equals("0");
                }
            }

            return false;
        }        

        /// <summary>
        ///  Send deleteSchedule request to delete a control program 
        /// </summary>
        /// <param name="id">Schedule ID (int)</param>
        /// <returns></returns>
        public static bool SendRequestDeleteControlProgram(long id)
        {
            var url = GetFullUrl(string.Format("/api/dimmingscheduler/deleteSchedule?scheduleId={0}", id));
            var response = SendRequest(url);
            if (response.StatusCode != HttpStatusCode.OK) return false;
            using (var responseStream = response.GetResponseStream())
            {
                if (responseStream != null)
                {
                    var xmlUtility = new XmlUtility();
                    xmlUtility.Load(responseStream);
                    var errorCodeNode = xmlUtility.GetSingleNode("/com.dotv.streetlightserver.api.data.SLVResult/errorCode");
                    if (errorCodeNode == null) return false;

                    return errorCodeNode.InnerText.Equals("0");
                }
            }

            return false;
        }

        /// <summary>
        /// Send deleteSchedule request to delete a control program 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool SendRequestDeleteControlProgram(string name)
        {
            var status = true;
            var controlPrograms = SendRequestGetControlPrograms();
            var deletedControlPrograms = controlPrograms.Where(p => p.Value.Equals(name)).Select(p => p.Key).ToList(); 
            foreach (var controlProgramId in deletedControlPrograms)
            {
                if (!SendRequestDeleteControlProgram(controlProgramId)) status = false;
            }

            return status;
        }

        /// <summary>
        /// Send deleteSchedule request to delete control programs matched a pattern
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static bool SendRequestDeleteControlPrograms(string pattern)
        {
            var status = true;
            var controlPrograms = SendRequestGetControlPrograms();
            var deletedControlPrograms = controlPrograms.Where(p => Regex.IsMatch(p.Value, pattern)).Select(p => p.Key).ToList();
            foreach (var controlProgramId in deletedControlPrograms)
            {
                if (!SendRequestDeleteControlProgram(controlProgramId)) status = false;
            }

            return status;
        }              

        #endregion //Control Program

        #region Calendar

        /// <summary>
        /// Send getSchedulers Request to get all calendars
        /// </summary>
        /// <returns>Dictionary<int:id, string:name> </returns>
        public static Dictionary<int, string> SendRequestGetCalendars()
        {
            var result = new Dictionary<int, string>();
            var url = GetFullUrl("/api/dimmingscheduler/getSchedulers?category=STREETLIGHT_DIMMING");
            var response = SendRequest(url);
            if (response.StatusCode != HttpStatusCode.OK) return result;
            using (var responseStream = response.GetResponseStream())
            {
                if (responseStream != null)
                {
                    var xmlUtility = new XmlUtility();
                    xmlUtility.Load(responseStream);
                    var nodes = xmlUtility.GetNodes("/com.dotv.streetlightserver.api.data.SLVResult/value/com.slv.service.core.data.dto.SLVScheduler");
                    if (nodes == null) return result;
                    foreach (var node in nodes)
                    {
                        var idNode = node.SelectSingleNode("id");
                        var nameNode = node.SelectSingleNode("name");
                        if (idNode == null || nameNode == null) continue;
                        result.Add(int.Parse(idNode.InnerText.Trim()), nameNode.InnerText.Trim());
                    }
                }
            }

            return result;
        }

        /// <summary>
        ///  Send createScheduler request to create new calendar
        /// </summary>
        /// <param name="name">Calendar name</param>
        /// <param name="description">description of calendar</param>
        /// <returns></returns>
        public static long SendRequestCreateCalendar(string name, string description = "")
        {
            long value;
            var url = GetFullUrl(string.Format("/api/dimmingscheduler/createScheduler?name={0}&category=STREETLIGHT_DIMMING&description={1}", name, description));
            var response = SendRequest(url);
            if (response.StatusCode != HttpStatusCode.OK) return -1;
            using (var responseStream = response.GetResponseStream())
            {
                if (responseStream != null)
                {
                    var xmlUtility = new XmlUtility();
                    xmlUtility.Load(responseStream);
                    var errorCodeNode = xmlUtility.GetSingleNode("/com.dotv.streetlightserver.api.data.SLVResult/errorCode");
                    var valueNode = xmlUtility.GetSingleNode("/com.dotv.streetlightserver.api.data.SLVResult/value");
                    if (errorCodeNode == null || valueNode == null) return -1;
                    if (errorCodeNode.InnerText.Equals("0") && long.TryParse(valueNode.InnerText, out value)) return value;
                }
            }

            return -1;
        }

        /// <summary>
        ///  Send deleteScheduler request to delete a calendar
        /// </summary>
        /// <param name="id">scheduler ID (int)</param>
        /// <returns></returns>
        public static bool SendRequestDeleteCalendar(long id)
        {
            var url = GetFullUrl(string.Format("/api/dimmingscheduler/deleteScheduler?schedulerId={0}", id));
            var response = SendRequest(url);
            if (response.StatusCode != HttpStatusCode.OK) return false;
            using (var responseStream = response.GetResponseStream())
            {
                if (responseStream != null)
                {
                    var xmlUtility = new XmlUtility();
                    xmlUtility.Load(responseStream);
                    var errorCodeNode = xmlUtility.GetSingleNode("/com.dotv.streetlightserver.api.data.SLVResult/errorCode");
                    if (errorCodeNode == null) return false;

                    return errorCodeNode.InnerText.Equals("0");
                }
            }

            return false;
        }

        /// <summary>
        /// Send deleteScheduler request to delete a calendar
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool SendRequestDeleteCalendar(string name)
        {
            var status = true;
            var calendars = SendRequestGetCalendars();
            var deletedCalendars = calendars.Where(p => p.Value.Equals(name)).Select(p => p.Key).ToList();
            foreach (var calendarId in deletedCalendars)
            {
                if(SendRequestDeleteCalendar(calendarId)) status = false;
            }

            return status;
        }

        /// <summary>
        /// Send deleteScheduler request to delete calendars matched a pattern
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static bool SendRequestDeleteCalendars(string pattern)
        {
            var status = true;
            var calendars = SendRequestGetCalendars();
            var deletedCalendars = calendars.Where(p => Regex.IsMatch(p.Value, pattern)).Select(p => p.Key).ToList();
            foreach (var calendarId in deletedCalendars)
            {
                if (SendRequestDeleteCalendar(calendarId)) status = false;
            }

            return status;
        }

        /// <summary>
        /// Send addEveryYearSchedulerItemAt request to associate control program to calendar yearly
        /// </summary>
        /// <param name="controlProgramId"></param>
        /// <param name="calendarId"></param>
        /// <param name="fromMonth"></param>
        /// <param name="fromDay"></param>
        /// <param name="toMonth"></param>
        /// <param name="toDay"></param>
        /// <returns></returns>
        public static bool SendRequestAssociateControlProgramToCalendarYearly(long controlProgramId, long calendarId, int fromMonth, int fromDay, int toMonth, int toDay)
        {
            var url = GetFullUrl(string.Format("/api/dimmingscheduler/addEveryYearSchedulerItemAt?scheduleId={0}&schedulerId={1}&fromMonth={2}&fromDayInMonth={3}&toMonth={4}&toDayInMonth={5}", controlProgramId, calendarId, fromMonth, fromDay, toMonth, toDay));
            var response = SendRequest(url);
            if (response.StatusCode != HttpStatusCode.OK) return false;
            using (var responseStream = response.GetResponseStream())
            {
                if (responseStream != null)
                {
                    var xmlUtility = new XmlUtility();
                    xmlUtility.Load(responseStream);
                    var errorCodeNode = xmlUtility.GetSingleNode("/com.dotv.streetlightserver.api.data.SLVResult/errorCode");
                    if (errorCodeNode == null) return false;

                    return errorCodeNode.InnerText.Equals("0");
                }
            }

            return false;
        }

        /// <summary>
        /// Send addEveryMonthSchedulerItemAt request to associate control program to calendar monthly
        /// </summary>
        /// <param name="controlProgramId"></param>
        /// <param name="calendarId"></param>
        /// <param name="fromDay"></param>
        /// <param name="lastDay"></param>
        /// <returns></returns>
        public static bool SendRequestAssociateControlProgramToCalendarMonthly(long controlProgramId, long calendarId, int fromDay, int lastDay)
        {
            var url = GetFullUrl(string.Format("/api/dimmingscheduler/addEveryMonthSchedulerItemAt?scheduleId={0}&schedulerId={1}&fromDayInMonth={2}&toDayInMonth={3}&lastDayInMonth=false", controlProgramId, calendarId, fromDay, lastDay));
            var response = SendRequest(url);
            if (response.StatusCode != HttpStatusCode.OK) return false;
            using (var responseStream = response.GetResponseStream())
            {
                if (responseStream != null)
                {
                    var xmlUtility = new XmlUtility();
                    xmlUtility.Load(responseStream);
                    var errorCodeNode = xmlUtility.GetSingleNode("/com.dotv.streetlightserver.api.data.SLVResult/errorCode");
                    if (errorCodeNode == null) return false;

                    return errorCodeNode.InnerText.Equals("0");
                }
            }

            return false;
        }

        /// <summary>
        /// Send commissionControllersSchedulers request to commission calendar with specific controller Id
        /// </summary>
        /// <param name="calendarId"></param>
        /// <param name="controllerId"></param>
        /// <returns></returns>
        public static bool SendRequestCommissionCalendar(long calendarId, string controllerId)
        {
            var url = GetFullUrl(string.Format("/api/dimmingscheduler/commissionControllersSchedulers?schedulerId={0}&controllerStrId={1}", calendarId, controllerId));
            var response = SendRequest(url);
            if (response.StatusCode != HttpStatusCode.OK) return false;
            using (var responseStream = response.GetResponseStream())
            {
                if (responseStream != null)
                {
                    var xmlUtility = new XmlUtility();
                    xmlUtility.Load(responseStream);
                    var batchId = xmlUtility.GetSingleNodeText("/SLVBatchResult/batchId");
                    if (batchId == null) return false;

                    return SendRequestGetBatchResult(batchId);
                }
            }

            return false;
        }       

        #endregion //Calendar

        #region Geozone

        /// <summary>
        /// Send getAllGeozones Request to get all devices of controller
        /// </summary>
        /// <returns>Dictionary<int:id, string:name> </returns>
        public static Dictionary<int, string> SendRequestGetAllGeozones()
        {
            var result = new Dictionary<int, string>();
            var url = GetFullUrl("/api/asset/getAllGeozones");
            var response = SendRequest(url);
            if (response.StatusCode != HttpStatusCode.OK) return result;
            using (var responseStream = response.GetResponseStream())
            {
                if (responseStream != null)
                {
                    var xmlUtility = new XmlUtility();
                    xmlUtility.Load(responseStream);
                    var nodes = xmlUtility.GetNodes("/SLVGeoZone-array/SLVGeoZone");
                    if (nodes == null) return result;
                    foreach (var node in nodes)
                    {
                        var idNode = node.SelectSingleNode("id");
                        var nameNode = node.SelectSingleNode("name");
                        if (idNode == null || nameNode == null) continue;
                        result.Add(int.Parse(idNode.InnerText.Trim()), nameNode.InnerText.Trim());
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Send searchGeozones request to get all devices of controller
        /// </summary>
        /// <param name="name"></param>
        /// <returns>geozoneId (-1 if geozone does not exist)</returns>
        public static int SendRequestGetGeozoneByName(string name)
        {
            var url = GetFullUrl(string.Format("/api/asset/searchGeozones?name={0}", name));
            var response = SendRequest(url);
            if (response.StatusCode != HttpStatusCode.OK) return -1;
            using (var responseStream = response.GetResponseStream())
            {
                if (responseStream != null)
                {
                    var xmlUtility = new XmlUtility();
                    xmlUtility.Load(responseStream);
                    var node = xmlUtility.GetSingleNode("/SLVGeoZone-array/SLVGeoZone");
                    if (node == null)
                    {
                        return -1;
                    }
                    return int.Parse(node.SelectSingleNode("id").InnerText.Trim());
                }
            }

            return -1;
        }

        /// <summary>
        /// Send createGeozone request to create a geozone
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parentId"></param>
        /// <param name="latMin"></param>
        /// <param name="latMax"></param>
        /// <param name="lngMin"></param>
        /// <param name="lngMax"></param>
        /// <returns>geozoneId (-1 if geozone created fail)</returns>
        public static int SendRequestCreateGeozone(string name, int parentId, string latMin, string latMax, string lngMin, string lngMax)
        {
            var url = GetFullUrl(string.Format("/api/assetmanagement/createGeozone?name={0}&parentId={1}&latMin={2}&latMax={3}&lngMin={4}&lngMax={5}", name, parentId, latMin, latMax, lngMin, lngMax));
            var response = SendRequest(url);
            if (response.StatusCode != HttpStatusCode.OK) return -1;
            using (var responseStream = response.GetResponseStream())
            {
                if (responseStream != null)
                {
                    var xmlUtility = new XmlUtility();
                    xmlUtility.Load(responseStream);

                    return int.Parse(xmlUtility.GetSingleNodeText("/SLVGeoZone/id"));
                }
            }

            return -1;
        }

        /// <summary>
        /// Send createGeozone request to create a geozone
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parentName"></param>
        /// <param name="latMin"></param>
        /// <param name="latMax"></param>
        /// <param name="lngMin"></param>
        /// <param name="lngMax"></param>
        /// <returns>geozoneId (-1 if geozone created fail)</returns>
        public static int SendRequestCreateGeozone(string name, string parentName, string latMin, string latMax, string lngMin, string lngMax)
        {
            var parentId = SendRequestGetGeozoneByName(parentName);
            if (parentId == -1) return -1;

            return SendRequestCreateGeozone(name, parentId, latMin, latMax, lngMin, lngMax);
        }

        /// <summary>
        ///  Send deleteGeozone request to delete a Geozone
        /// </summary>
        /// <param name="id">Geozone ID (int)</param>
        /// <returns></returns>
        public static bool SendRequestDeleteGeozone(int id)
        {
            var url = GetFullUrl(string.Format("/api/assetmanagement/deleteGeozone?geozoneId={0}&pullUpContent=false", id));
            var response = SendRequest(url);
            if (response.StatusCode != HttpStatusCode.OK) return false;
            using (var responseStream = response.GetResponseStream())
            {
                if (responseStream != null)
                {
                    var xmlUtility = new XmlUtility();
                    xmlUtility.Load(responseStream);
                    var stringNode = xmlUtility.GetSingleNode("/string");
                    if (stringNode == null) return false;

                    return stringNode.InnerText.Equals("OK");
                }
            }

            return false;
        }

        /// <summary>
        ///  Send deleteGeozone request to delete a Geozone
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool SendRequestDeleteGeozone(string name)
        {
            var goezoneId = SendRequestGetGeozoneByName(name);
            if (goezoneId == -1) return false;

            return SendRequestDeleteGeozone(goezoneId);
        }

        /// <summary>
        ///  Send deleteGeozone request to delete geozones match a pattern
        /// </summary>
        /// <param name="namePattern"></param>
        /// <returns></returns>
        public static bool SendRequestDeleteGeozones(string namePattern)
        {
            var status = true;
            var geozones = SendRequestGetAllGeozones();
            var deletedGeozones = geozones.Where(p => Regex.IsMatch(p.Value, namePattern));
            foreach (var geozone in deletedGeozones)
            {
                if (!SendRequestDeleteGeozone(geozone.Key)) status = false;
            }

            return status;
        }

        #endregion //Geozone

        #region Device

        /// <summary>
        /// Send getControllerDevices request to get all devices of controller
        /// </summary>
        /// <returns>Dictionary<string:identifer, string:name> </returns>
        public static Dictionary<string, string> SendRequestGetDevicesByControllerId(string controllerId)
        {
            var result = new Dictionary<string, string>();
            var url = GetFullUrl(string.Format("/api/asset/getControllerDevices?controllerStrId={0}", controllerId));
            var response = SendRequest(url);
            if (response.StatusCode != HttpStatusCode.OK) return result;
            using (var responseStream = response.GetResponseStream())
            {
                if (responseStream != null)
                {
                    var xmlUtility = new XmlUtility();
                    xmlUtility.Load(responseStream);
                    var nodes = xmlUtility.GetNodes("/SLVDevice-array/SLVDevice");
                    if (nodes == null) return result;
                    foreach (var node in nodes)
                    {
                        var deviceIdNode = node.SelectSingleNode("idOnController");
                        var nameNode = node.SelectSingleNode("name");
                        if (deviceIdNode == null || nameNode == null) continue;
                        result.Add(deviceIdNode.InnerText.Trim(), nameNode.InnerText.Trim());
                    }
                }
            }

            return result;
        }

        /// <summary>
        ///  Send getControllerDevices request to get devices of a category of controller
        /// </summary>
        /// <param name="controllerId"></param>
        /// <param name="deviceType"></param>
        /// <returns></returns>
        public static Dictionary<string, string> SendRequestGetDevicesByControllerId(string controllerId, DeviceType deviceType)
        {
            var result = new Dictionary<string, string>();
            var url = GetFullUrl(string.Format("/api/asset/getControllerDevices?controllerStrId={0}", controllerId));
            var response = SendRequest(url);
            if (response.StatusCode != HttpStatusCode.OK) return result;
            using (var responseStream = response.GetResponseStream())
            {
                if (responseStream != null)
                {
                    var xmlUtility = new XmlUtility();
                    xmlUtility.Load(responseStream);
                    var nodes = xmlUtility.GetNodes("/SLVDevice-array/SLVDevice");
                    if (nodes == null) return result;
                    foreach (var node in nodes)
                    {
                        var deviceIdNode = node.SelectSingleNode("idOnController");
                        var nameNode = node.SelectSingleNode("name");
                        var categoryNode = node.SelectSingleNode("categoryStrId");
                        if (deviceIdNode == null || nameNode == null || categoryNode == null) continue;
                        if (categoryNode.InnerText.Trim().Equals(deviceType.Category))
                        {
                            result.Add(deviceIdNode.InnerText.Trim(), nameNode.InnerText.Trim());
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Send getGeozoneDevices request to get all devices of geozone
        /// </summary>
        /// <param name="geozoneId"></param>
        /// <returns>Dictionary<string:identifer#controllerId, string:name> </returns>
        public static Dictionary<string, string> SendRequestGetDevicesByGeozone(int geozoneId)
        {
            var result = new Dictionary<string, string>();
            var url = GetFullUrl(string.Format("/api/asset/getGeozoneDevices?geozoneId={0}&recurse=true", geozoneId));
            var response = SendRequest(url);
            if (response.StatusCode != HttpStatusCode.OK) return result;
            using (var responseStream = response.GetResponseStream())
            {
                if (responseStream != null)
                {
                    var xmlUtility = new XmlUtility();
                    xmlUtility.Load(responseStream);
                    var nodes = xmlUtility.GetNodes("/SLVDevice-array/SLVDevice");
                    if (nodes == null) return result;
                    foreach (var node in nodes)
                    {
                        var deviceIdNode = node.SelectSingleNode("idOnController");
                        var controllerIdNode = node.SelectSingleNode("controllerStrId");
                        var nameNode = node.SelectSingleNode("name");
                        if (deviceIdNode == null || controllerIdNode == null || nameNode == null) continue;
                        result.Add(string.Format("{0}#{1}", deviceIdNode.InnerText.Trim(), controllerIdNode.InnerText.Trim()), nameNode.InnerText.Trim());
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Send getGeozoneDevices request to get all devices of geozone
        /// </summary>
        /// <param name="geozoneName"></param>
        /// <returns>Dictionary<string:identifer#controllerId, string:name> </returns>
        public static Dictionary<string, string> SendRequestGetDevicesByGeozone(string geozoneName)
        {
            var result = new Dictionary<string, string>();
            var geozoneId = SendRequestGetGeozoneByName(geozoneName);
            if (geozoneId == -1) return result;

            return SendRequestGetDevicesByGeozone(geozoneId);
        }


        /// <summary>
        ///  Send searchDevicesExt request to get a device
        /// </summary>
        /// <param name="name"></param>
        /// <param name="geozoneName">default root geozone</param>
        /// <returns></returns>
        public static DeviceModel SendRequestGetDevice(string name, string geozoneName = "")
        {
            var geozoneId = 1;            
            if(!string.IsNullOrEmpty(geozoneName))
            { 
                geozoneId = SendRequestGetGeozoneByName(geozoneName);
                if (geozoneId == -1) geozoneId = 1;
            }

            var url = GetFullUrl(string.Format("/api/asset/searchDevicesExt?geozoneId={0}&recurse=true&maxResults=1&attributeName=name&attributeOperator=eq&attributeValue={1}", geozoneId, name));
            var response = SendRequest(url);
            if (response.StatusCode != HttpStatusCode.OK) return null;
            using (var responseStream = response.GetResponseStream())
            {
                if (responseStream != null)
                {
                    var xmlUtility = new XmlUtility();
                    xmlUtility.Load(responseStream);
                    var node = xmlUtility.GetSingleNode("/com.dotv.streetlightserver.api.data.SLVResult/value/SLVDevice");
                    if (node == null) return null;
                    var device = new DeviceModel();
                    device.Status = DeviceStatus.NonWorking;
                    device.Id = node.GetChildNodeText("idOnController");
                    device.Name = node.GetChildNodeText("name");
                    device.Latitude = node.GetChildNodeText("lat");
                    device.Longitude = node.GetChildNodeText("lng");
                    
                    return device;
                }
            }

            return null;
        }

        /// <summary>
        /// Send createCategoryDevice request to create a device
        /// </summary>
        /// <param name="category"></param>
        /// <param name="deviceName"></param>
        /// <param name="deviceId"></param>
        /// <param name="equipmentType"></param>
        /// <param name="controllerId"></param>
        /// <param name="geozoneId"></param>
        /// <param name="lat"></param>
        /// <param name="lng"></param>
        /// <returns></returns>
        public static bool SendRequestCreateDevice(string category, string deviceName, string deviceId, string equipmentType, string controllerId, int geozoneId, string lat, string lng)
        {
            var url = GetFullUrl(string.Format("/api/assetmanagement/createCategoryDevice?userName={0}&categoryStrId={1}&controllerStrId={2}&idOnController={3}&geoZoneId={4}&lat={5}&lng={6}", deviceName, category, controllerId, deviceId, geozoneId, lat, lng));
            var response = SendRequest(url);
            if (response.StatusCode != HttpStatusCode.OK) return false;

            return SendRequestSetDeviceValuesToDevice(controllerId, deviceId, "model", equipmentType, Settings.GetServerTime());
        }

        /// <summary>
        /// Send createCategoryDevice request to create a device
        /// </summary>
        /// <param name="category"></param>
        /// <param name="deviceName"></param>
        /// <param name="deviceId"></param>
        /// <param name="model"></param>
        /// <param name="controllerId"></param>
        /// <param name="geozoneName"></param>
        /// <param name="lat"></param>
        /// <param name="lng"></param>
        /// <returns></returns>
        public static bool SendRequestCreateDevice(string category, string deviceName, string deviceId, string model, string controllerId, string geozoneName, string lat, string lng)
        {
            var geozoneId = SendRequestGetGeozoneByName(geozoneName);
            if (geozoneId == -1) return false;

            return SendRequestCreateDevice(category, deviceName, deviceId, model, controllerId, geozoneId, lat, lng);
        }

        /// <summary>
        /// Send SetDeviceValues request to update a device
        /// </summary>
        /// <param name="controllerId"></param>
        /// <param name="deviceId"></param>
        /// <param name="valueName"></param>
        /// <param name="value"></param>
        /// <param name="eventTime"></param>
        /// <param name="comment"></param>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static bool SendRequestSetDeviceValuesToDevice(string controllerId, string deviceId, string valueName, dynamic value, DateTime eventTime, string comment = "", string user = "", string password = "")
        {
            var requestUriTemplate = GetFullUrl("/api/loggingmanagement/setDeviceValues?controllerStrId={0}&idOnController={1}&valueName={2}&value={3}&doLog=true&eventTime={4}");
            if (!string.IsNullOrEmpty(comment)) requestUriTemplate = requestUriTemplate + "&comment=" + comment;
            var propertyToSend = valueName;
            dynamic valueToSend = value;
            if (valueToSend is bool)
            {
                valueToSend = (bool)valueToSend == true ? "true" : "false";
            }
            else
            {
                valueToSend = HttpUtility.UrlEncode(value as string);
            }
            var timeToSend = eventTime;
            var requestUrlToSend = string.Format(requestUriTemplate, controllerId, deviceId, propertyToSend, valueToSend, timeToSend.ToString("yyyy-MM-dd HH:mm:ss"));
            var response = SendRequest(requestUrlToSend, user, password);
            if (response.StatusCode != HttpStatusCode.OK) return false;
            using (var responseStream = response.GetResponseStream())
            {
                if (responseStream != null)
                {
                    var xmlUtility = new XmlUtility();
                    xmlUtility.Load(responseStream);
                    var errorCodeNode = xmlUtility.GetSingleNode("/com.dotv.streetlightserver.api.data.SLVResult/errorCode");
                    if (errorCodeNode == null) return false;

                    return errorCodeNode.InnerText.Equals("0");
                }
            }

            return false;
        }

        /// <summary>
        /// Send deleteDevice request to delete a device
        /// </summary>
        /// <param name="controllerId"></param>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        public static bool SendRequestDeleteDevice(string controllerId, string deviceId)
        {
            var url = GetFullUrl(string.Format("/api/assetmanagement/deleteDevice?controllerStrId={0}&idOnController={1}", controllerId, deviceId));
            var response = SendRequest(url);
            if (response.StatusCode != HttpStatusCode.OK) return false;
            using (var responseStream = response.GetResponseStream())
            {
                if (responseStream != null)
                {
                    var xmlUtility = new XmlUtility();
                    xmlUtility.Load(responseStream);
                    var stringNode = xmlUtility.GetSingleNode("/string");
                    if (stringNode == null) return false;

                    return stringNode.InnerText.Equals("OK");
                }
            }

            return false;
        }

        /// <summary>
        /// Send request to delete all devices of controller
        /// </summary>
        /// <param name="controllerId"></param>
        /// <returns></returns>
        public static bool SendRequestDeleteDevicesByControllerId(string controllerId)
        {
            var status = true;
            var devices = SendRequestGetDevicesByControllerId(controllerId);
            foreach (var device in devices)
            {
                if (!SendRequestDeleteDevice(controllerId, device.Key)) status = false;
            }

            return status;
        }

        /// <summary>
        /// Send request to delete all devices of geozone
        /// </summary>
        /// <param name="geozoneName"></param>
        /// <param name="devicePattern"></param>
        /// <returns></returns>
        public static bool SendRequestDeleteDevicesByGeozone(string geozoneName, string devicePattern = "")
        {
            var status = true;
            var devices = SendRequestGetDevicesByGeozone(geozoneName);
            if (!string.IsNullOrEmpty(devicePattern))
                devices = devices.Where(p => Regex.IsMatch(p.Value, devicePattern)).ToDictionary(p => p.Key, p => p.Value);
            foreach (var device in devices)
            {
                var arr = device.Key.SplitEx(new string[] { "#" });
                var controllerId = arr[1];
                var deviceId = arr[0];
                if (!SendRequestDeleteDevice(controllerId, deviceId)) status = false;
            }

            return status;
        }

        /// <summary>
        /// Send request to get equipment types of specific device type
        /// </summary>
        /// <param name="deviceType"></param>
        /// <returns>Dictionary: key=name, value=model</returns>
        public static Dictionary<string, string> SendRequestGetEquipmentTypes(DeviceType deviceType)
        {
            var result = new Dictionary<string, string>();
            var url = GetFullUrl(string.Format("/api/logging/getVirtualDeviceValueDescriptors?ser=json&propertyName=categoryStrId&propertyValue={0}&configFilePath=adminEquipmentsDeviceCardConfigs.xml", deviceType.Category));
            var response = SendRequest(url);
            if (response.StatusCode != HttpStatusCode.OK) return result;
            using (var responseStream = response.GetResponseStream())
            {
                if (responseStream != null)
                {
                    var responseJSON = new StreamReader(responseStream);
                    var token = JToken.Parse(responseJSON.ReadToEnd());
                    var keys = token.Children();
                    var modelFunctionId = keys.FirstOrDefault(p => p["labelKey"].ToString() == "db.meaning.modelfunctionid.label");
                    var inner = modelFunctionId["dataFormat"].Value<JObject>();
                    var entries = inner.Properties().Where(p => Regex.IsMatch(p.Name, @"item\[\d{1,}\].*")).Select(p => new KeyValuePair<string, string>(p.Name, p.Value.ToString())).ToDictionary(x => x.Key, x => x.Value);
                    var length = entries.Count / 2;
                    for (int i = 0; i < length; i++)
                    {
                        var key = entries[string.Format("item[{0}].label", i)];
                        var value = entries[string.Format("item[{0}].value", i)];
                        result.Add(key, value);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Send request to exit manual mode for a device
        /// </summary>
        /// <param name="controllerId"></param>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        public static bool SendRequestExitManualMode(string controllerId, string deviceId)
        {
            var url = GetFullUrl(string.Format("/api/dimming/exitManualModes?controllerStrId={0}&idOnController={1}", controllerId, deviceId));
            var response = SendRequest(url);
            if (response.StatusCode != HttpStatusCode.OK) return false;
            using (var responseStream = response.GetResponseStream())
            {
                if (responseStream != null)
                {
                    var xmlUtility = new XmlUtility();
                    xmlUtility.Load(responseStream);
                    var errorCodeNode = xmlUtility.GetSingleNode("/com.dotv.streetlightserver.api.data.SLVResult/errorCode");
                    if (errorCodeNode == null) return false;

                    return errorCodeNode.InnerText.Equals("0");
                }
            }

            return false;
        }

        #endregion //Device

        #region Controller

        /// <summary>
        /// Send getAllControllers request to get all controllers
        /// </summary>
        /// <returns>Dictionary<string:identifer, string:name> </returns>
        public static Dictionary<string, string> SendRequestGetAllControllers()
        {
            var result = new Dictionary<string, string>();
            var url = GetFullUrl("/api/asset/getAllControllers");
            var response = SendRequest(url);
            if (response.StatusCode != HttpStatusCode.OK) return result;
            using (var responseStream = response.GetResponseStream())
            {
                if (responseStream != null)
                {
                    var xmlUtility = new XmlUtility();
                    xmlUtility.Load(responseStream);
                    var nodes = xmlUtility.GetNodes("/SLVController-array/SLVController");
                    if (nodes == null) return result;
                    foreach (var node in nodes)
                    {
                        var strIdNode = node.SelectSingleNode("strId");
                        var nameNode = node.SelectSingleNode("name");
                        if (strIdNode == null || nameNode == null) continue;
                        result.Add(strIdNode.InnerText.Trim(), nameNode.InnerText.Trim());
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Send createCategoryDevice request to create a controller
        /// </summary>
        /// <param name="controllerName"></param>
        /// <param name="controllerId"></param>
        /// <param name="controlTechnology"></param>
        /// <param name="geozoneId"></param>
        /// <param name="lat"></param>
        /// <param name="lng"></param>
        /// <param name="host"></param>
        /// <returns></returns>
        public static bool SendRequestCreateController(string controllerName, string controllerId, string controlTechnology, int geozoneId, string lat, string lng, string host = "")
        {
            var url = GetFullUrl(string.Format("/api/assetmanagement/createCategoryDevice?userName={0}&categoryStrId=controllerdevice&controllerStrId={1}&idOnController=controllerdevice&geoZoneId={2}&lat={3}&lng={4}", controllerName, controllerId, geozoneId, lat, lng));
            var response = SendRequest(url);
            if (response.StatusCode != HttpStatusCode.OK) return false;

            var success = SendRequestSetDeviceValuesToController(controllerId, "controller.firmwareVersion", controlTechnology, Settings.GetServerTime());
            if (!success) return false;
            if (!string.IsNullOrEmpty(host))
                success = SendRequestSetDeviceValuesToController(controllerId, "controller.host", host, Settings.GetServerTime());

            return success;
        }

        /// <summary>
        /// Send createCategoryDevice request to create a controller
        /// </summary>
        /// <param name="controllerName"></param>
        /// <param name="controllerId"></param>
        /// <param name="controlTechnology"></param>
        /// <param name="geozoneName"></param>
        /// <param name="lat"></param>
        /// <param name="lng"></param>
        /// <param name="host"></param>
        /// <returns></returns>
        public static bool SendRequestCreateController(string controllerName, string controllerId, string controlTechnology, string geozoneName, string lat, string lng, string host = "")
        {
            var geozoneId = SendRequestGetGeozoneByName(geozoneName);
            if (geozoneId == -1) return false;

            return SendRequestCreateController(controllerName, controllerId, controlTechnology, geozoneId, lat, lng, host);
        }

        /// <summary>
        /// Send SetDeviceValues request to update a controller
        /// </summary>
        /// <param name="controllerId"></param>
        /// <param name="deviceId"></param>
        /// <param name="valueName"></param>
        /// <param name="value"></param>
        /// <param name="eventTime"></param>
        public static bool SendRequestSetDeviceValuesToController(string controllerId, string valueName, dynamic value, DateTime eventTime, bool isUpdateTime = false)
        {
            var requestUriTemplate = GetFullUrl("/api/loggingmanagement/setDeviceValues?controllerStrId={0}&idOnController=controllerdevice&valueName={1}&value={2}&doLog=true&eventTime={3}");
            if (isUpdateTime) requestUriTemplate = requestUriTemplate + "&updateControllerUpdateTime=true";
            var reversedRequestedUrls = string.Empty;
            var propertyToSend = valueName;
            dynamic valueToSend = value;
            if (valueToSend is bool)
            {
                valueToSend = (bool)valueToSend == true ? "true" : "false";
            }
            else
            {
                valueToSend = value as string;
            }
            var timeToSend = eventTime;
            var requestUrlToSend = string.Format(requestUriTemplate, controllerId, propertyToSend, valueToSend, timeToSend.ToString("yyyy-MM-dd HH:mm:ss"));
            var response = SendRequest(requestUrlToSend);
            if (response.StatusCode != HttpStatusCode.OK) return false;
            using (var responseStream = response.GetResponseStream())
            {
                if (responseStream != null)
                {
                    var xmlUtility = new XmlUtility();
                    xmlUtility.Load(responseStream);
                    var errorCodeNode = xmlUtility.GetSingleNode("/com.dotv.streetlightserver.api.data.SLVResult/errorCode");
                    if (errorCodeNode == null) return false;

                    return errorCodeNode.InnerText.Equals("0");
                }
            }

            return false;
        }

        /// <summary>
        ///  Send deleteDevice request to delete a controller
        /// </summary>
        /// <param name="controllerId"></param>
        /// <returns></returns>
        public static bool SendRequestDeleteController(string controllerId)
        {
            return SendRequestDeleteDevice(controllerId, "controllerdevice");
        }

        /// <summary>
        /// Send getSystemTimeStringInLocalTime to get datetime of a controller
        /// </summary>
        /// <param name="controllerId"></param>
        /// <returns></returns>
        public static DateTime SendRequestGetControllerDateTime(string controllerId)
        {
            var url = GetFullUrl(string.Format("/api/controller/getSystemTimeStringInLocalTime?controllerStrId={0}", controllerId));
            var response = SendRequest(url);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                Assert.Warn(string.Format("*** Cannot get current time of controller '{0}'", controllerId));
                return DateTime.MinValue;
            }

            using (var responseStream = response.GetResponseStream())
            {
                if (responseStream != null)
                {
                    var xmlUtility = new XmlUtility();
                    xmlUtility.Load(responseStream);
                    var timeNode = xmlUtility.GetSingleNode("/string");
                    if (timeNode == null) return DateTime.MinValue;

                    return DateTime.Parse(timeNode.InnerText.Trim());

                }
            }

            return DateTime.MinValue;
        }

        /// <summary>
        /// Send request to get control technologie of controller
        /// </summary>
        /// <returns>Dictionary: key=name, value=firmwareVersion</returns>
        public static Dictionary<string, string> SendRequestGetControlTechnologies()
        {
            var result = new Dictionary<string, string>();
            var url = GetFullUrl("/api/logging/getVirtualDeviceValueDescriptors?ser=json&propertyName=categoryStrId&propertyValue=controllerdevice&configFilePath=adminEquipmentsDeviceCardConfigs.xml");
            var response = SendRequest(url);
            if (response.StatusCode != HttpStatusCode.OK) return result;
            using (var responseStream = response.GetResponseStream())
            {
                if (responseStream != null)
                {
                    var responseJSON = new StreamReader(responseStream);
                    var token = JToken.Parse(responseJSON.ReadToEnd());
                    var keys = token.Children();
                    var firmwareVersion = keys.FirstOrDefault(p => p["labelKey"].ToString() == "db.meaning.controller.firmwareversion.label");
                    var inner = firmwareVersion["dataFormat"].Value<JObject>();
                    var entries = inner.Properties().Where(p => Regex.IsMatch(p.Name, @"item\[\d{1,}\]\.label") || Regex.IsMatch(p.Name, @"item\[\d{1,}\]\.value"))
                        .Where(p => !Regex.IsMatch(p.Name, @"item\[\d{1,}\]\.labelKey"))
                        .Select(p => new KeyValuePair<string, string>(p.Name, p.Value.ToString())).ToDictionary(x => x.Key, x => x.Value);

                    var length = entries.Count / 2;
                    for (int i = 0; i < length; i++)
                    {
                        var key = entries[string.Format("item[{0}].label", i)];
                        var value = entries[string.Format("item[{0}].value", i)];
                        result.Add(key, value);
                    }
                }
            }

            return result;
        }

        /// <summary>
        ///  Send commissionControllerAsync request to commission controller
        /// </summary>
        /// <param name="controllerId"></param>
        /// <returns></returns>
        public static bool SendRequestCommissionController(string controllerId)
        {
            var url = GetFullUrl(string.Format("/api/controllermanagement/commissionControllerAsync?controllerStrId={0}&flags=7", controllerId));
            var response = SendRequest(url);
            if (response.StatusCode != HttpStatusCode.OK) return false;
            using (var responseStream = response.GetResponseStream())
            {
                if (responseStream != null)
                {
                    var xmlUtility = new XmlUtility();
                    xmlUtility.Load(responseStream);
                    var batchId = xmlUtility.GetSingleNodeText("/SLVBatchResult/batchId");
                    if (batchId == null) return false;

                    return SendRequestGetBatchResult(batchId);
                }
            }

            return false;
        }

        #endregion //Controller

        #region Profile and User

        /// <summary>
        /// Send getAllUsers request to get all users
        /// </summary>
        /// <returns>Dictionary<string:user, string:user profile> </returns>
        public static Dictionary<string, string> SendRequestGetAllUsers()
        {
            var result = new Dictionary<string, string>();
            var url = GetFullUrl("/api/userprofile/getAllUsers");
            var response = SendRequest(url);
            if (response.StatusCode != HttpStatusCode.OK) return result;
            using (var responseStream = response.GetResponseStream())
            {
                if (responseStream != null)
                {
                    var xmlUtility = new XmlUtility();
                    xmlUtility.Load(responseStream);
                    var nodes = xmlUtility.GetNodes("/SLVUser-array/SLVUser");
                    if(nodes == null) return result;
                    foreach (var node in nodes)
                    {
                        var loginNode = node.SelectSingleNode("login");
                        var profileNameNode = node.SelectSingleNode("profilName");
                        if (loginNode == null || profileNameNode == null) continue;
                        result.Add(loginNode.InnerText.Trim(), profileNameNode.InnerText.Trim());
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Send createProfil request to create a profile
        /// </summary>
        /// <param name="name"></param>
        /// <param name="geozoneId"></param>
        /// <param name="language"></param>
        /// <param name="skin"></param>
        /// <returns></returns>
        public static bool SendRequestCreateProfile(string name, int geozoneId, string language = "", string skin = "")
        {
            var url = GetFullUrl(string.Format("/api/userprofile/createProfil?profilName={0}&geoZoneId={1}", name, geozoneId));
            var response = SendRequest(url);
            if (response.StatusCode != HttpStatusCode.OK) return false;
            using (var responseStream = response.GetResponseStream())
            {
                if (responseStream != null)
                {
                    var xmlUtility = new XmlUtility();
                    xmlUtility.Load(responseStream);
                    var nameNode = xmlUtility.GetSingleNode("/SLVProfil/name");
                    if (nameNode == null) return false;
                    if(!nameNode.InnerText.Equals(name)) return false;
                }
            }
            
            var sendUpdate = false;
            var urlUpdate = GetFullUrl(string.Format("/api/userprofile/updateProfilProperties?profilName={0}", name));
            if (!string.IsNullOrEmpty(language))
            {
                urlUpdate += string.Format("&property.locale={0}", language);
                sendUpdate = true;
            }
            if (!string.IsNullOrEmpty(skin))
            {
                urlUpdate += string.Format("&property.skin={0}", skin);
                sendUpdate = true;
            }
            if (sendUpdate)
            {
                response = SendRequest(urlUpdate);
                if (response.StatusCode != HttpStatusCode.OK) return false;
                using (var responseStream = response.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        var xmlUtility = new XmlUtility();
                        xmlUtility.Load(responseStream);
                        var stringNode = xmlUtility.GetSingleNode("/string");
                        if (stringNode == null) return false;

                        return stringNode.InnerText.Equals("OK");
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Send createProfil request to create a profile
        /// </summary>
        /// <param name="name"></param>
        /// <param name="geozoneName"></param>
        /// <param name="language"></param>
        /// <param name="skin"></param>
        /// <returns></returns>
        public static bool SendRequestCreateProfile(string name, string geozoneName, string language = "", string skin = "")
        {
            var geozoneId = SendRequestGetGeozoneByName(geozoneName);
            if (geozoneId == -1) return false;

            return SendRequestCreateProfile(name, geozoneId, language, skin);
        }

        /// <summary>
        ///  Send createUser request to create an user
        /// </summary>
        /// <param name="profile"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="email"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="phone"></param>
        /// <returns></returns>
        public static bool SendRequestCreateUser(string profile, string username, string password, string email = "", string firstName = "", string lastName = "", string phone = "")
        {
            var url = GetFullUrl(string.Format("/api/userprofile/createUser?profilName={0}&login={1}&password={2}&firstName={3}&lastName={4}&phone={5}", profile, username, password, firstName, lastName, phone));
            
            if (!string.IsNullOrEmpty(email))
            {
                url += string.Format("&email={0}", email);
            }
            else
            {
                url += string.Format("&email={0}@{1}.com", username, GenerateString(5));
            }
            var response = SendRequest(url);
            if (response.StatusCode != HttpStatusCode.OK) return false;
            using (var responseStream = response.GetResponseStream())
            {
                if (responseStream != null)
                {
                    var xmlUtility = new XmlUtility();
                    xmlUtility.Load(responseStream);
                    var loginNode = xmlUtility.GetSingleNode("/SLVUser/login");
                    if (loginNode == null) return false;

                    return loginNode.InnerText.Equals(username);
                }
            }

            return false;
        }

        /// <summary>
        /// Send deleteProfil request to delete an User profile
        /// </summary>
        /// <param name="profileName"></param>
        /// <returns></returns>
        public static bool SendRequestDeleteProfileHasNoUsers(string profileName)
        {
            var url = GetFullUrl(string.Format("/api/userprofile/deleteProfil?profilName={0}", profileName));
            var response = SendRequest(url);
            if (response.StatusCode != HttpStatusCode.OK) return false;
            using (var responseStream = response.GetResponseStream())
            {
                if (responseStream != null)
                {
                    var xmlUtility = new XmlUtility();
                    xmlUtility.Load(responseStream);
                    var errorCodeNode = xmlUtility.GetSingleNode("/com.dotv.streetlightserver.api.data.SLVResult/errorCode");
                    if (errorCodeNode == null) return false;

                    return errorCodeNode.InnerText.Equals("0");
                }
            }

            return false;
        }

        /// <summary>
        /// Send request to delete a profile has many users
        /// </summary>
        /// <param name="profileName"></param>
        /// <returns></returns>
        public static bool SendRequestDeleteProfile(string profileName)
        {
            var users = SendRequestGetAllUsers();
            var usersOfProfile = users.Where(p => p.Value.Equals(profileName)).Select(p => p.Key).ToList();
            foreach (var user in usersOfProfile)
            {
                SendRequestDeleteUser(user);
            }

            return SendRequestDeleteProfileHasNoUsers(profileName);
        }

        /// <summary>
        /// Send request to delete profiles match a pattern
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static bool SendRequestDeleteProfiles(string pattern)
        {
            var users = SendRequestGetAllUsers();
            var profiles = users.Where(p => Regex.IsMatch(p.Value, pattern)).Select(p => p.Value).ToList();
            foreach (var profile in profiles)
            {
                var usersOfProfile = users.Where(p => p.Value.Equals(profile)).Select(p => p.Key).ToList();
                foreach (var user in usersOfProfile)
                {
                    if (!SendRequestDeleteUser(user)) return false;
                }
                if (!SendRequestDeleteProfileHasNoUsers(profile)) return false;
            }

            return true;
        }

        /// <summary>
        /// Send deleteUser request to delete an User
        /// </summary>
        /// <param name="profileName"></param>
        /// <returns></returns>
        public static bool SendRequestDeleteUser(string username)
        {
            var url = GetFullUrl(string.Format("/api/userprofile/deleteUser?login={0}", username));
            var response = SendRequest(url);
            if (response.StatusCode != HttpStatusCode.OK) return false;
            using (var responseStream = response.GetResponseStream())
            {
                if (responseStream != null)
                {
                    var xmlUtility = new XmlUtility();
                    xmlUtility.Load(responseStream);
                    var errorCodeNode = xmlUtility.GetSingleNode("/com.dotv.streetlightserver.api.data.SLVResult/errorCode");
                    if (errorCodeNode == null) return false;

                    return errorCodeNode.InnerText.Equals("0");
                }
            }

            return false;
        }

        /// <summary>
        ///  Send request to delete a profile and user
        /// </summary>
        /// <param name="username"></param>
        /// <param name="profileName"></param>
        /// <returns></returns>
        public static bool SendRequestDeleteUserAndProfile(string username, string profileName)
        {
            if (SendRequestDeleteUser(username))
                return SendRequestDeleteProfileHasNoUsers(profileName);

            return false;
        }

        /// <summary>
        /// Send request to delete a profile and user
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        public static bool SendRequestDeleteUserAndProfile(UserModel userModel)
        {
            return SendRequestDeleteUserAndProfile(userModel.Username, userModel.Profile);
        }

        #endregion //Profile and User        

        #region Lamp Types

        /// <summary>
        /// Send getAllBrands Request to get all lamp types
        /// </summary>
        /// <returns>Dictionary<int:id, string:name> </returns>
        public static Dictionary<int, string> SendRequestGetLampTypes()
        {
            var result = new Dictionary<int, string>();
            var url = GetFullUrl("/api/asset/getAllBrands");
            var response = SendRequest(url);
            if (response.StatusCode != HttpStatusCode.OK) return result;
            using (var responseStream = response.GetResponseStream())
            {
                if (responseStream != null)
                {
                    var xmlUtility = new XmlUtility();
                    xmlUtility.Load(responseStream);
                    var nodes = xmlUtility.GetNodes("/SLVLabelledValue-array/SLVLabelledValue");
                    if (nodes == null) return result;
                    foreach (var node in nodes)
                    {
                        var idNode = node.SelectSingleNode("value");
                        var nameNode = node.SelectSingleNode("label");
                        if (idNode == null || nameNode == null) continue;
                        result.Add(int.Parse(idNode.InnerText.Trim()), nameNode.InnerText.Trim());
                    }
                }
            }

            return result;
        }

        /// <summary>
        ///  Send createBrand request to create new lamp type
        /// </summary>
        /// <param name="name">Brand name</param>
        /// <returns></returns>
        public static string SendRequestCreateLampType(string name)
        {
            var url = GetFullUrl(string.Format("/api/assetmanagement/createBrand?name={0}&description={0}", name));
            var response = SendRequest(url);
            if (response.StatusCode != HttpStatusCode.OK) return string.Empty;
            using (var responseStream = response.GetResponseStream())
            {
                if (responseStream != null)
                {
                    var xmlUtility = new XmlUtility();
                    xmlUtility.Load(responseStream);
                    var valueNode = xmlUtility.GetSingleNode("/SLVLabelledValue/value");
                    if (valueNode == null) return string.Empty;

                    return valueNode.InnerText.Trim();                    
                }
            }

            return string.Empty;
        }

        /// <summary>
        ///  Send deleteBrand request to delete a lamp type
        /// </summary>
        /// <param name="id">brandId (int)</param>
        /// <returns></returns>
        public static bool SendRequestDeleteLampType(int id)
        {
            var url = GetFullUrl(string.Format("/api/assetmanagement/deleteBrand?brandId={0}", id));
            var response = SendRequest(url);
            if (response.StatusCode != HttpStatusCode.OK) return false;
            using (var responseStream = response.GetResponseStream())
            {
                if (responseStream != null)
                {
                    var xmlUtility = new XmlUtility();
                    xmlUtility.Load(responseStream);
                    var errorCodeNode = xmlUtility.GetSingleNode("/com.dotv.streetlightserver.api.data.SLVResult/errorCode");
                    if (errorCodeNode == null) return false;

                    return errorCodeNode.InnerText.Equals("0");
                }
            }

            return false;
        }

        /// <summary>
        /// Send deleteBrand request to delete a lamp type
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool SendRequestDeleteLampType(string name)
        {
            var status = true;
            var brandes = SendRequestGetLampTypes();
            var deletedBrandes = brandes.Where(p => p.Value.Equals(name)).Select(p => p.Key).ToList();
            foreach (var brandId in deletedBrandes)
            {
                if(!SendRequestDeleteLampType(brandId)) status = false;
            }

            return status;
        }

        /// <summary>
        /// Send deleteBrand request to delete lamp types matched a pattern
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static bool SendRequestDeleteLampTypes(string pattern)
        {
            var status = true;
            var brandes = SendRequestGetLampTypes();
            var deletedBrandes = brandes.Where(p => Regex.IsMatch(p.Value, pattern)).Select(p => p.Key).ToList();
            foreach (var brandId in deletedBrandes)
            {
                if (!SendRequestDeleteLampType(brandId)) status = false;
            }

            return status;
        }

        #endregion //Lamp Types

        #region Energy Supplier

        /// <summary>
        /// Send getAllBrands Request to get all energy suppliers
        /// </summary>
        /// <returns>Dictionary<int:id, string:name> </returns>
        public static Dictionary<int, string> SendRequestGetEnergySuppliers()
        {
            var result = new Dictionary<int, string>();
            var url = GetFullUrl("/api/asset/getAllProviders");
            var response = SendRequest(url);
            if (response.StatusCode != HttpStatusCode.OK) return result;
            using (var responseStream = response.GetResponseStream())
            {
                if (responseStream != null)
                {
                    var xmlUtility = new XmlUtility();
                    xmlUtility.Load(responseStream);
                    var nodes = xmlUtility.GetNodes("/SLVProvider-array/SLVProvider");
                    if (nodes == null) return result;
                    foreach (var node in nodes)
                    {
                        var idNode = node.SelectSingleNode("id");
                        var nameNode = node.SelectSingleNode("name");
                        if (idNode == null || nameNode == null) continue;
                        result.Add(int.Parse(idNode.InnerText.Trim()), nameNode.InnerText.Trim());
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Send createProvider request to create new energy supplier
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pollutionRate"></param>
        /// <returns></returns>
        public static string SendRequestCreateEnergySupplier(string name, string pollutionRate)
        {
            var url = GetFullUrl(string.Format("/api/assetmanagement/createProvider?name={0}&pollutionRate={1}", name, pollutionRate));
            var response = SendRequest(url);
            if (response.StatusCode != HttpStatusCode.OK) return string.Empty;
            using (var responseStream = response.GetResponseStream())
            {
                if (responseStream != null)
                {
                    var xmlUtility = new XmlUtility();
                    xmlUtility.Load(responseStream);
                    var idNode = xmlUtility.GetSingleNode("/SLVProvider/id");
                    if (idNode == null) return string.Empty;

                    return idNode.InnerText.Trim();
                }
            }

            return string.Empty;
        }

        /// <summary>
        ///  Send updateProvider request to update an energy supplier
        /// </summary>
        /// <param name="id">Provider ID (int)</param>
        /// <returns></returns>
        public static bool SendRequestUpdateEnergySupplier(int id, string newName, string pollutionRate)
        {
            var url = GetFullUrl(string.Format("/api/assetmanagement/updateProvider?providerId={0}&newName={1}&pollutionRate={2}", id, newName, pollutionRate));
            var response = SendRequest(url);
            if (response.StatusCode != HttpStatusCode.OK) return false;
            using (var responseStream = response.GetResponseStream())
            {
                if (responseStream != null)
                {
                    var xmlUtility = new XmlUtility();
                    xmlUtility.Load(responseStream);
                    var idNode = xmlUtility.GetSingleNode("/SLVProvider/id");
                    if (idNode == null) return false;

                    return idNode.InnerText.Equals(id.ToString());
                }
            }

            return false;
        }

        /// <summary>
        /// Send updateProvider request to update an energy supplier
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pollutionRate"></param>
        /// <returns></returns>
        public static bool SendRequestUpdateEnergySupplier(string name, string pollutionRate)
        {
            var providers = SendRequestGetEnergySuppliers();
            var provider = providers.FirstOrDefault(p => p.Value.Equals(name));

            return SendRequestUpdateEnergySupplier(provider.Key, provider.Value, pollutionRate);
        }

        /// <summary>
        ///  Send deleteProvider request to delete an energy supplier
        /// </summary>
        /// <param name="id">Provider ID (int)</param>
        /// <returns></returns>
        public static bool SendRequestDeleteEnergySupplier(int id)
        {
            var url = GetFullUrl(string.Format("/api/assetmanagement/deleteProvider?id={0}", id));
            var response = SendRequest(url);
            if (response.StatusCode != HttpStatusCode.OK) return false;
            using (var responseStream = response.GetResponseStream())
            {
                if (responseStream != null)
                {
                    var xmlUtility = new XmlUtility();
                    xmlUtility.Load(responseStream);
                    var errorCodeNode = xmlUtility.GetSingleNode("/com.dotv.streetlightserver.api.data.SLVResult/errorCode");
                    if (errorCodeNode == null) return false;

                    return errorCodeNode.InnerText.Equals("0");
                }
            }

            return false;
        }

        /// <summary>
        /// Send deleteProvider request to delete an energy supplier
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool SendRequestDeleteEnergySupplier(string name)
        {
            var status = true;
            var providers = SendRequestGetEnergySuppliers();
            var deletedProviders = providers.Where(p => p.Value.Equals(name)).Select(p => p.Key).ToList();
            foreach (var providerId in deletedProviders)
            {
                if (!SendRequestDeleteEnergySupplier(providerId)) status = false;
            }

            return status;
        }

        /// <summary>
        /// Send deleteProvider request to delete energy suppliers matched a pattern
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static bool SendRequestDeleteEnergySuppliers(string pattern)
        {
            var status = true;
            var providers = SendRequestGetEnergySuppliers();
            var deletedProviders = providers.Where(p => Regex.IsMatch(p.Value, pattern)).Select(p => p.Key).ToList();
            foreach (var providerId in deletedProviders)
            {
                if (!SendRequestDeleteEnergySupplier(providerId)) status = false;
            }

            return status;
        }

        #endregion //Energy Supplier

        #region Alarm        

        /// <summary>
        /// Send request to get alarms with specific name pattern
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static Dictionary<string, string> SendRequestGetAlarms(string pattern = "")
        {
            var result = new Dictionary<string, string>();
            var url = GetFullUrl("/api/alarmmanagement/getAllAlarmDefinitions?propertyDescriptors=false");
            var response = SendRequest(url);
            if (response.StatusCode != HttpStatusCode.OK) return result;
            using (var responseStream = response.GetResponseStream())
            {
                if (responseStream != null)
                {
                    var xmlUtility = new XmlUtility();
                    xmlUtility.Load(responseStream);
                    var nodes = xmlUtility.GetNodes("/SLVAlarmDefinition-array/SLVAlarmDefinition");
                    if (nodes == null) return result;
                    foreach (var node in nodes)
                    {
                        var nameNode = node.SelectSingleNode("name");
                        var idNode = node.SelectSingleNode("id");
                        if (nameNode == null || idNode == null) continue;
                        result.Add(nameNode.InnerText.Trim(), idNode.InnerText.Trim());
                    }
                }
            }

            if(!string.IsNullOrEmpty(pattern))
            {
                result = result.Where(p => Regex.IsMatch(p.Key, pattern)).ToDictionary(k => k.Key, v => v.Value);
            }

            return result;
        }

        /// <summary>
        /// Send request to create alarm
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="action"></param>
        /// <param name="properties"></param>
        /// <returns>alarmDefinitionId</returns>
        public static string SendRequestCreateAlarm(string name, AlarmType type, string action, params string[] properties)
        {
            InitServicePointManager();
            var id = "";
            var strBuilder = new StringBuilder();
            var url = GetFullUrl(string.Format("/api/alarmmanagement/createSingleActionAlarmDefinition?alarmDefinitionName={0}&triggerConditionImplClassName={1}&alarmStateChangeActionImplClassName={2}", HttpUtility.UrlEncode(name), type.TriggerConditionImplClassName, action));            
            var response = SendRequest(url);
            if (response.StatusCode != HttpStatusCode.OK) return string.Empty;            
            using (var responseStream = response.GetResponseStream())
            {
                if (responseStream != null)
                {
                    var xmlUtility = new XmlUtility();
                    xmlUtility.Load(responseStream);
                    var idNode = xmlUtility.GetSingleNode("/SLVAlarmDefinition/id");
                    if (idNode == null) return string.Empty;
                    id = idNode.InnerText.Trim();
                }
            }
            if (string.IsNullOrEmpty(id)) return string.Empty;
            if (SendRequestUpdateAlarm(id, name, properties)) return id;

            return string.Empty;
        }

        /// <summary>
        /// Send request to update an alarm
        /// </summary>
        /// <param name="alarmDefinitionId">>e.g. SmartMeteringValueAlarmTriggersGenerator</param>
        /// <param name="name"></param>
        /// <param name="properties">e.g. triggerCondition.triggerValue|21000</param>
        /// <returns></returns>
        public static bool SendRequestUpdateAlarm(string alarmDefinitionId, string name, params string[] properties)
        {
            InitServicePointManager();
            var strBuilder = new StringBuilder();
            var url = GetFullUrl(string.Format("/api/alarmmanagement/updateAlarmDefinition?alarmDefinitionId={0}&alarmName={1}", HttpUtility.UrlEncode(alarmDefinitionId), HttpUtility.UrlEncode(name)));
            var strTemplate = "&propertyName={0}&propertyValue={1}";
            foreach (var property in properties)
            {
                var propertyName = property.SplitAndGetAt("|", 0);
                var propertyValue = property.SplitAndGetAt("|", 1);
                strBuilder.Append(string.Format(strTemplate, HttpUtility.UrlEncode(propertyName), HttpUtility.UrlEncode(propertyValue)));
            }
            var strPropertied = strBuilder.ToString();
            var response = SendRequest(url + strPropertied);
            if (response.StatusCode != HttpStatusCode.OK) return false;
            using (var responseStream = response.GetResponseStream())
            {
                if (responseStream != null)
                {
                    var xmlUtility = new XmlUtility();
                    xmlUtility.Load(responseStream);

                    return xmlUtility.IsInnerTextContains(name);
                }
            }

            return false;
        }

        /// <summary>
        /// Send request to delete an alarm by definitionId
        /// </summary>
        /// <param name="definitionId"></param>
        /// <returns></returns>
        public static bool SendRequestDeleteAlarmById(string definitionId)
        {
            InitServicePointManager();
            var strBuilder = new StringBuilder();
            var url = GetFullUrl(string.Format("/api/alarmmanagement/deleteAlarmDefinition?alarmDefinitionId={0}", HttpUtility.UrlEncode(definitionId)));        
            var response = SendRequest(url);
            if (response.StatusCode != HttpStatusCode.OK) return false;
            using (var responseStream = response.GetResponseStream())
            {
                if (responseStream != null)
                {
                    var xmlUtility = new XmlUtility();
                    xmlUtility.Load(responseStream);
                    var errorCodeNode = xmlUtility.GetSingleNode("/com.dotv.streetlightserver.api.data.SLVResult/errorCode");
                    if (errorCodeNode == null) return false;

                    return errorCodeNode.InnerText.Equals("0");
                }
            }

            return false;
        }

        /// <summary>
        /// Send request to delete an alarm by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool SendRequestDeleteAlarmByName(string name)
        {
            var alarms = SendRequestGetAlarms();
            if (alarms.ContainsKey(name))
            {
                return SendRequestDeleteAlarmById(alarms[name]);
            }      

            return false;
        }

        /// <summary>
        /// Send request to delete alarms with pattern
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static bool SendRequestDeleteAlarms(string pattern = "")
        {
            var status = true;
            var alarms = SendRequestGetAlarms(pattern);
            foreach (var alarm in alarms)
            {
                if (!SendRequestDeleteAlarmById(alarm.Value)) status = false;
            }

            return status;
        }

        #endregion //Alarm

        #endregion //API methods

        #region JavaScript methods

        /// <summary>
        /// Get current language code of SLV page
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentLanguageCode()
        {
            var languageCode = WebDriverContext.JsExecutor.ExecuteScript("return plugin.userContext.profile.locale");
            return languageCode.ToString();
        }

        /// <summary>
        ///  Get current skin of SLV page
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentSkin()
        {
            var skin = WebDriverContext.JsExecutor.ExecuteScript("return plugin.userContext.getSkin()");
            return skin.ToString();
        }

        #endregion //JavaScript methods

        #endregion //Public methods

        #region Private methods

        private static string GetToken(ref CookieContainer cookies, string user = "", string password = "")
        {
            var tokenRequest = (HttpWebRequest)WebRequest.Create(GetFullUrl("/api/userprofile/getCurrentUser"));
            tokenRequest.Credentials = GetNetworkCredential(user, password);
            tokenRequest.CookieContainer = cookies;
            tokenRequest.Headers.Add("X-CSRF-Token", "Fetch");
            tokenRequest.Headers.Add("X-Requested-By", "Sample");
            tokenRequest.Headers.Add("X-Requested-With", "XMLHttpRequest");
            tokenRequest.Headers.Add("ser", "json");
            var tokenResponse = (HttpWebResponse)tokenRequest.GetResponse();
            var token = tokenResponse.Headers["X-CSRF-Token"];

            return token;
        }

        private static HttpWebResponse SendRequest(string url, string user = "", string password = "")
        {
            InitServicePointManager();
            var cookies = new CookieContainer();            
            var token = GetToken(ref cookies, user, password);

            //main request
            var request = (HttpWebRequest) WebRequest.Create(url);
            request.Credentials = GetNetworkCredential(user, password);           
            request.CookieContainer = cookies;
            request.Headers.Add("X-CSRF-Token", token);
            Console.WriteLine("Send request: {0}", url);
            var finalResponse = (HttpWebResponse)request.GetResponse();

            return finalResponse;
        }

        private static void InitServicePointManager()
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }

        private static string GetFullUrl(string apiUrl)
        {
            var requestBaseUrl = Settings.SlvUrl.EndsWith("/") ? Settings.SlvUrl.TrimEnd('/') : Settings.SlvUrl;
            var url = requestBaseUrl + apiUrl;

            return url;
        }

        private static NetworkCredential GetNetworkCredential(string user = "", string password = "")
        {
            var strUser = string.IsNullOrEmpty(user) ? Settings.Users["DefaultTest"].Username : user;
            var strPassword = string.IsNullOrEmpty(password) ? Settings.Users["DefaultTest"].Password : password;
            return new NetworkCredential(strUser, strPassword);
        }

        #endregion //Private methods
    }
}
