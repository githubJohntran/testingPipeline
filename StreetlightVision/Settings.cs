using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using NUnit.Framework.Internal.Commands;
using StreetlightVision.Models;
using StreetlightVision.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Xml;

namespace StreetlightVision
{
    public class Settings
    {
        public const string XML_FILE_PATH = @"Resources\data\xml\";
        public const string CSV_FILE_PATH = @"Resources\data\csv\";

        public static string TESTING_COMMON_DATA_FILE_PATH { get { return GetFullPath(XML_FILE_PATH + "TestingCommonData.xml"); } }
        public const string TESTING_COMMON_DATA_XPATH_PREFIX = @"/TestingData/{0}/{1}";

        public static string TESTING_USERS_FILE_PATH { get { return GetFullPath(XML_FILE_PATH + "TestingUsers.xml"); } }
        public const string TESTING_USERS_XPATH_PREFIX = @"/TestingUsers/{0}/{1}";

        public static string SMOKE_TEST_DATA_FILE_PATH { get { return GetFullPath(XML_FILE_PATH + "SMOKE.xml"); } }
        public const string SMOKE_XPATH_PREFIX = @"/SMOKE/{0}/{1}";

        public static string TC1_TEST_DATA_FILE_PATH { get { return GetFullPath(XML_FILE_PATH + "TC1.xml"); } }
        public const string TC1_XPATH_PREFIX = @"/TC1/{0}/{1}";

        public static string TC2_TEST_DATA_FILE_PATH { get { return GetFullPath(XML_FILE_PATH + "TC2.xml"); } }
        public const string TC2_XPATH_PREFIX = @"/TC2/{0}/{1}";

        public static string TC3_TEST_DATA_FILE_PATH { get { return GetFullPath(XML_FILE_PATH + "TC3.xml"); } }
        public const string TC3_XPATH_PREFIX = @"/TC3/{0}/{1}";

        public static string TC4_TEST_DATA_FILE_PATH { get { return GetFullPath(XML_FILE_PATH + "TC4.xml"); } }
        public const string TC4_XPATH_PREFIX = @"/TC4/{0}/{1}";

        public static string TC5_TEST_DATA_FILE_PATH { get { return GetFullPath(XML_FILE_PATH + "TC5.xml"); } }
        public const string TC5_XPATH_PREFIX = @"/TC5/{0}/{1}";

        public static string JIRA_COVERAGE_TEST_DATA_FILE_PATH { get { return GetFullPath(XML_FILE_PATH + "JiraCoverage.xml"); } }
        public const string JIRA_COVERAGE_XPATH_PREFIX = @"/JiraCoverage/{0}/{1}";

        #region Apps Test Data file

        public static string RM_TEST_DATA_FILE_PATH { get { return GetFullPath(XML_FILE_PATH + "ReportManager.xml"); } }
        public const string RM_XPATH_PREFIX = @"/ReportManager/{0}/{1}";

        public static string AS_TEST_DATA_FILE_PATH { get { return GetFullPath(XML_FILE_PATH + "AdvancedSearch.xml"); } }
        public const string AS_XPATH_PREFIX = @"/AdvancedSearch/{0}/{1}";

        public static string BO_TEST_DATA_FILE_PATH { get { return GetFullPath(XML_FILE_PATH + "BackOffice.xml"); } }
        public const string BO_XPATH_PREFIX = @"/BackOffice/{0}/{1}";

        public static string DEVICE_HISTORY_TEST_DATA_FILE_PATH { get { return GetFullPath(XML_FILE_PATH + "DeviceHistory.xml"); } }
        public const string DEVICE_HISTORY_XPATH_PREFIX = @"/DeviceHistory/{0}/{1}";

        public static string SM_TEST_DATA_FILE_PATH { get { return GetFullPath(XML_FILE_PATH + "SchedulingManager.xml"); } }
        public const string SM_XPATH_PREFIX = @"/SchedulingManager/{0}/{1}";

        public static string FT_TEST_DATA_FILE_PATH { get { return GetFullPath(XML_FILE_PATH + "FailureTracking.xml"); } }
        public const string FT_XPATH_PREFIX = @"/FailureTracking/{0}/{1}";

        public static string RTC_TEST_DATA_FILE_PATH { get { return GetFullPath(XML_FILE_PATH + "RealtimeControl.xml"); } }
        public const string RTC_XPATH_PREFIX = @"/RealtimeControl/{0}/{1}";

        public static string EI_TEST_DATA_FILE_PATH { get { return GetFullPath(XML_FILE_PATH + "EquipmentInventory.xml"); } }
        public const string EI_XPATH_PREFIX = @"/EquipmentInventory/{0}/{1}";

        public static string INSTALLATION_TEST_DATA_FILE_PATH { get { return GetFullPath(XML_FILE_PATH + "Installation.xml"); } }
        public const string INSTALLATION_XPATH_PREFIX = @"/Installation/{0}/{1}";

        public static string DATA_HISTORY_TEST_DATA_FILE_PATH { get { return GetFullPath(XML_FILE_PATH + "DataHistory.xml"); } }
        public const string DATA_HISTORY_XPATH_PREFIX = @"/DataHistory/{0}/{1}";

        public static string BC_TEST_DATA_FILE_PATH { get { return GetFullPath(XML_FILE_PATH + "BatchControl.xml"); } }
        public const string BC_XPATH_PREFIX = @"/BatchControl/{0}/{1}";

        public static string USER_TEST_DATA_FILE_PATH { get { return GetFullPath(XML_FILE_PATH + "User.xml"); } }
        public const string USER_XPATH_PREFIX = @"/User/{0}/{1}";

        public static string ALARM_TEST_DATA_FILE_PATH { get { return GetFullPath(XML_FILE_PATH + "Alarm.xml"); } }
        public const string ALARM_XPATH_PREFIX = @"/Alarm/{0}/{1}";

        #endregion //Apps Test Data file

        #region Widgets Test Data file

        public static string GFM_TEST_DATA_FILE_PATH { get { return GetFullPath(XML_FILE_PATH + "GeozoneFailuresMonitor.xml"); } }
        public const string GFM_XPATH_PREFIX = @"/GeozoneFailuresMonitor/{0}/{1}";

        public static string ENV_SENSOR_TEST_DATA_FILE_PATH { get { return GetFullPath(XML_FILE_PATH + "EnvironmentalSensor.xml"); } }
        public const string ENV_SENSOR_XPATH_PREFIX = @"/EnvironmentalSensor/{0}/{1}";

        #endregion //Apps Test Data file

        public const string DEFAULT_TIMEZONE = "Coordinated Universal Time [+00:00] [UTC]";

        /// <summary>
        /// Host name
        /// </summary>
        public static string HostName = ConfigurationManager.AppSettings.Get("HOST_NAME");

        /// <summary>
        /// SLV HTTP URL
        /// </summary>
        public static string SlvUrl = ConfigurationManager.AppSettings.Get("SLV_URL");

        /// <summary>
        /// Browser Name from App.config
        /// </summary>
        public static readonly string Browser = ConfigurationManager.AppSettings.Get("BROWSER");

        /// <summary>
        /// Web driver timeout value
        /// </summary>
        public static readonly int DriverWaitingTimeout = Convert.ToInt32(ConfigurationManager.AppSettings.Get("DRIVER_WAIT_TIMEOUT"));

        /// <summary>
        /// Retrying timeout value
        /// </summary>
        public static readonly int LocatorRetryingTimeout = Convert.ToInt32(ConfigurationManager.AppSettings.Get("LOCATOR_RETRYING_TIMEOUT"));

        /// <summary>
        /// Polling interval value
        /// </summary>
        public static readonly int LocatorPollingInterval = Convert.ToInt32(ConfigurationManager.AppSettings.Get("LOCATOR_POLLING_INTERVAL"));

        /// <summary>
        /// Alarm Time Wait for trigger/acknowlegde
        /// </summary>
        public static readonly int AlarmTimeWait = Convert.ToInt32(ConfigurationManager.AppSettings.Get("ALARM_TIME_WAIT"));

        /// <summary>
        /// Server Timezone name
        /// </summary>
        public static string ServerTimeZoneName = ConfigurationManager.AppSettings.Get("SERVER_TIMEZONE");

        /// <summary>
        /// Root geozone's name
        /// </summary>
        public static string RootGeozoneName = ConfigurationManager.AppSettings.Get("ROOT_GEOZONE_NAME");

        /// <summary>
        /// Step logging enabled
        /// </summary>
        public static bool StepLoggingEnabled = ConfigurationManager.AppSettings.Get("STEP_LOGGING_ENABLED") == "ON";

        /// <summary>
        /// Verify logging enabled
        /// </summary>
        public static bool VerifyLoggingEnabled = ConfigurationManager.AppSettings.Get("VERIFY_LOGGING_ENABLED") == "ON";

        /// <summary>
        /// Deletable item's name
        /// </summary>
        public static string DeletableNamePattern = ConfigurationManager.AppSettings.Get("DELETABLE_NAME_PATTERN");        
     
        /// <summary>
        /// Get Binary Path for FireFox Driver
        /// </summary>
        public static string FirefoxBinaryPath = ConfigurationManager.AppSettings.Get("FIREFOX_BINARY_PATH");

        private static Dictionary<string, UserModel> _dicUsers = null;
        public static Dictionary<string, UserModel> Users
        {
            get
            {
                if (_dicUsers == null)
                {
                    _dicUsers = new Dictionary<string, UserModel>();
                    var xmlUtility = new XmlUtility(TESTING_USERS_FILE_PATH);

                    foreach (var userNode in xmlUtility.GetNodes(@"/TestingUsers/User"))
                    {
                        var alias = userNode.GetAttrVal("alias");
                        var userName = userNode.GetAttrVal("userName");
                        var password = userNode.GetAttrVal("password");
                        var fullName = userNode.GetAttrVal("fullName");
                        var email = userNode.GetAttrVal("email");
                        var profile = userNode.GetAttrVal("profile");
                        var language = userNode.GetAttrVal("language");
                        var skin = userNode.GetAttrVal("skin");

                        if (!_dicUsers.ContainsKey(alias))
                        {
                            _dicUsers.Add(alias, new UserModel { Username = userName, Password = password, FullName = fullName, Email = email, Profile = profile, Language = language, Skin = skin });
                        }
                    }
                }

                return _dicUsers;
            }

        }

        private static TestDataModel _commonTestData = null;
        public static TestDataModel CommonTestData
        {
            get
            {
                if (_commonTestData == null)
                {
                    _commonTestData = new TestDataModel();
                    var xmlUtility = new XmlUtility(TESTING_COMMON_DATA_FILE_PATH);

                    foreach (var geozoneNode in xmlUtility.GetNodes(@"/TestingData/Geozone"))
                    {
                        var geozone = new GeozoneModel();
                        geozone.Name = geozoneNode.GetAttrVal("name");
                        geozone.Path = geozoneNode.GetAttrVal("path");

                        var workingStreetlights = geozoneNode.SelectNodes(@"Working/Streetlight");
                        var workingControllers = geozoneNode.SelectNodes(@"Working/Controller");
                        var nonWorkingStreetlights = geozoneNode.SelectNodes(@"NonWorking/Streetlight");
                        var nonWorkingControllers = geozoneNode.SelectNodes(@"NonWorking/Controller");

                        foreach (XmlNode node in workingStreetlights)
                        {
                            geozone.Devices.Add(InitStreetlight(node, DeviceStatus.Working));
                        }

                        foreach (XmlNode node in nonWorkingStreetlights)
                        {
                            geozone.Devices.Add(InitStreetlight(node, DeviceStatus.NonWorking));
                        }

                        foreach (XmlNode node in workingControllers)
                        {
                            geozone.Devices.Add(InitController(node, DeviceStatus.Working));
                        }

                        foreach (XmlNode node in nonWorkingControllers)
                        {
                            geozone.Devices.Add(InitController(node, DeviceStatus.NonWorking));
                        }

                        _commonTestData.Geozones.Add(geozone);
                    }
                }

                return _commonTestData;
            }

        }

        /// <summary>
        /// This key is to state the method to name a WebDriver instance of current 
        /// test
        /// </summary>
        public static string CurrentTestWebDriverKeyName
        {
            get
            {
                return TestContext.CurrentContext.Test.Name;
            }
        }

        /// <summary>
        /// Dictionary defines Olson TimeZone with Windows TimeZone
        /// </summary>
        public static Dictionary<string, string> OlsonWindowsTimeZones = new Dictionary<string, string>()
        {
            { "Africa/Bangui", "W. Central Africa Standard Time" },
            { "Africa/Cairo", "Egypt Standard Time" },
            { "Africa/Casablanca", "Morocco Standard Time" },
            { "Africa/Harare", "South Africa Standard Time" },
            { "Africa/Johannesburg", "South Africa Standard Time" },
            { "Africa/Lagos", "W. Central Africa Standard Time" },
            { "Africa/Monrovia", "Greenwich Standard Time" },
            { "Africa/Nairobi", "E. Africa Standard Time" },
            { "Africa/Windhoek", "Namibia Standard Time" },
            { "America/Anchorage", "Alaskan Standard Time" },
            { "America/Argentina/San_Juan", "Argentina Standard Time" },
            { "America/Asuncion", "Paraguay Standard Time" },
            { "America/Bahia", "Bahia Standard Time" },
            { "America/Bogota", "SA Pacific Standard Time" },
            { "America/Buenos_Aires", "Argentina Standard Time" },
            { "America/Caracas", "Venezuela Standard Time" },
            { "America/Cayenne", "SA Eastern Standard Time" },
            { "America/Chicago", "Central Standard Time" },
            { "America/Chihuahua", "Mountain Standard Time (Mexico)" },
            { "America/Cuiaba", "Central Brazilian Standard Time" },
            { "America/Denver", "Mountain Standard Time" },
            { "America/Fortaleza", "SA Eastern Standard Time" },
            { "America/Godthab", "Greenland Standard Time" },
            { "America/Guatemala", "Central America Standard Time" },
            { "America/Halifax", "Atlantic Standard Time" },
            { "America/Indianapolis", "US Eastern Standard Time" },
            { "America/Indiana/Indianapolis", "US Eastern Standard Time" },
            { "America/La_Paz", "SA Western Standard Time" },
            { "America/Los_Angeles", "Pacific Standard Time" },
            { "America/Mexico_City", "Mexico Standard Time" },
            { "America/Montevideo", "Montevideo Standard Time" },
            { "America/New_York", "Eastern Standard Time" },
            { "America/Noronha", "UTC-02" },
            { "America/Phoenix", "US Mountain Standard Time" },
            { "America/Regina", "Canada Central Standard Time" },
            { "America/Santa_Isabel", "Pacific Standard Time (Mexico)" },
            { "America/Santiago", "Pacific SA Standard Time" },
            { "America/Sao_Paulo", "E. South America Standard Time" },
            { "America/St_Johns", "Newfoundland Standard Time" },
            { "America/Tijuana", "Pacific Standard Time" },
            { "Antarctica/McMurdo", "New Zealand Standard Time" },
            { "Atlantic/South_Georgia", "UTC-02" },
            { "Asia/Almaty", "Central Asia Standard Time" },
            { "Asia/Amman", "Jordan Standard Time" },
            { "Asia/Baghdad", "Arabic Standard Time" },
            { "Asia/Baku", "Azerbaijan Standard Time" },
            { "Asia/Bangkok", "SE Asia Standard Time" },
            { "Asia/Beirut", "Middle East Standard Time" },
            { "Asia/Calcutta", "India Standard Time" },
            { "Asia/Colombo", "Sri Lanka Standard Time" },
            { "Asia/Damascus", "Syria Standard Time" },
            { "Asia/Dhaka", "Bangladesh Standard Time" },
            { "Asia/Dubai", "Arabian Standard Time" },
            { "Asia/Irkutsk", "North Asia East Standard Time" },
            { "Asia/Jerusalem", "Israel Standard Time" },
            { "Asia/Kabul", "Afghanistan Standard Time" },
            { "Asia/Kamchatka", "Kamchatka Standard Time" },
            { "Asia/Karachi", "Pakistan Standard Time" },
            { "Asia/Katmandu", "Nepal Standard Time" },
            { "Asia/Kolkata", "India Standard Time" },
            { "Asia/Krasnoyarsk", "North Asia Standard Time" },
            { "Asia/Kuala_Lumpur", "Singapore Standard Time" },
            { "Asia/Kuwait", "Arab Standard Time" },
            { "Asia/Magadan", "Magadan Standard Time" },
            { "Asia/Muscat", "Arabian Standard Time" },
            { "Asia/Novosibirsk", "N. Central Asia Standard Time" },
            { "Asia/Oral", "West Asia Standard Time" },
            { "Asia/Rangoon", "Myanmar Standard Time" },
            { "Asia/Riyadh", "Arab Standard Time" },
            { "Asia/Seoul", "Korea Standard Time" },
            { "Asia/Shanghai", "China Standard Time" },
            { "Asia/Singapore", "Singapore Standard Time" },
            { "Asia/Taipei", "Taipei Standard Time" },
            { "Asia/Tashkent", "West Asia Standard Time" },
            { "Asia/Tbilisi", "Georgian Standard Time" },
            { "Asia/Tehran", "Iran Standard Time" },
            { "Asia/Tokyo", "Tokyo Standard Time" },
            { "Asia/Ulaanbaatar", "Ulaanbaatar Standard Time" },
            { "Asia/Vladivostok", "Vladivostok Standard Time" },
            { "Asia/Yakutsk", "Yakutsk Standard Time" },
            { "Asia/Yekaterinburg", "Ekaterinburg Standard Time" },
            { "Asia/Yerevan", "Armenian Standard Time" },
            { "Atlantic/Azores", "Azores Standard Time" },
            { "Atlantic/Cape_Verde", "Cape Verde Standard Time" },
            { "Atlantic/Reykjavik", "Greenwich Standard Time" },
            { "Australia/Adelaide", "Cen. Australia Standard Time" },
            { "Australia/Brisbane", "E. Australia Standard Time" },
            { "Australia/Darwin", "AUS Central Standard Time" },
            { "Australia/Hobart", "Tasmania Standard Time" },
            { "Australia/Perth", "W. Australia Standard Time" },
            { "Australia/Sydney", "AUS Eastern Standard Time" },
            { "Etc/GMT", "UTC" },
            { "Etc/GMT+11", "UTC-11" },
            { "Etc/GMT+12", "Dateline Standard Time" },
            { "Etc/GMT+2", "UTC-02" },
            { "Etc/GMT-12", "UTC+12" },
            { "Europe/Amsterdam", "W. Europe Standard Time" },
            { "Europe/Athens", "GTB Standard Time" },
            { "Europe/Belgrade", "Central Europe Standard Time" },
            { "Europe/Berlin", "W. Europe Standard Time" },
            { "Europe/Brussels", "Romance Standard Time" },
            { "Europe/Budapest", "Central Europe Standard Time" },
            { "Europe/Dublin", "GMT Standard Time" },
            { "Europe/Helsinki", "FLE Standard Time" },
            { "Europe/Istanbul", "GTB Standard Time" },
            { "Europe/Kiev", "FLE Standard Time" },
            { "Europe/London", "GMT Standard Time" },
            { "Europe/Minsk", "E. Europe Standard Time" },
            { "Europe/Moscow", "Russian Standard Time" },
            { "Europe/Paris", "Romance Standard Time" },
            { "Europe/Sarajevo", "Central European Standard Time" },
            { "Europe/Warsaw", "Central European Standard Time" },
            { "Indian/Mauritius", "Mauritius Standard Time" },
            { "Pacific/Apia", "Samoa Standard Time" },
            { "Pacific/Auckland", "New Zealand Standard Time" },
            { "Pacific/Fiji", "Fiji Standard Time" },
            { "Pacific/Guadalcanal", "Central Pacific Standard Time" },
            { "Pacific/Guam", "West Pacific Standard Time" },
            { "Pacific/Honolulu", "Hawaiian Standard Time" },
            { "Pacific/Pago_Pago", "UTC-11" },
            { "Pacific/Port_Moresby", "West Pacific Standard Time" },
            { "Pacific/Tongatapu", "Tonga Standard Time" }
        };

        /// <summary>
        /// Dictionary for Streetlight: Key = 'Name of equipment type' in UI, Value = 'model' in csv
        /// </summary>
        private static Dictionary<string, string> _dicStreetlightEquipmentTypes = null;
        public static Dictionary<string, string> StreetlightEquipmentTypes
        {
            get
            {
                if (_dicStreetlightEquipmentTypes == null)
                {
                    _dicStreetlightEquipmentTypes = SLVHelper.SendRequestGetEquipmentTypes(DeviceType.Streetlight);
                }

                return _dicStreetlightEquipmentTypes;
            }
        }

        /// <summary>
        /// Dictionary for Switch: Key = 'Name of equipment type' in UI, Value = 'model' in csv
        /// </summary>
        private static Dictionary<string, string> _dicSwitchEquipmentTypes = null;
        public static Dictionary<string, string> SwitchEquipmentTypes
        {
            get
            {
                if (_dicSwitchEquipmentTypes == null)
                {
                    _dicSwitchEquipmentTypes = SLVHelper.SendRequestGetEquipmentTypes(DeviceType.Switch);
                }

                return _dicSwitchEquipmentTypes;
            }
        }

        /// <summary>
        /// Dictionary for Electrical Counter: Key = 'Name of equipment type' in UI, Value = 'model' in csv
        /// </summary>
        private static Dictionary<string, string> _dicElectricalCounterEquipmentTypes = null;
        public static Dictionary<string, string> ElectricalCounterEquipmentTypes
        {
            get
            {
                if (_dicElectricalCounterEquipmentTypes == null)
                {
                    _dicElectricalCounterEquipmentTypes = SLVHelper.SendRequestGetEquipmentTypes(DeviceType.ElectricalCounter);
                }

                return _dicElectricalCounterEquipmentTypes;
            }
        }

        /// <summary>
        /// Dictionary for Cabinet Controller: Key = 'Name of equipment type' in UI, Value = 'model' in csv
        /// </summary>
        private static Dictionary<string, string> _dicCabinetControllerEquipmentTypes = null;
        public static Dictionary<string, string> CabinetControllerEquipmentTypes
        {
            get
            {
                if (_dicCabinetControllerEquipmentTypes == null)
                {
                    _dicCabinetControllerEquipmentTypes = SLVHelper.SendRequestGetEquipmentTypes(DeviceType.CabinetController);
                }

                return _dicCabinetControllerEquipmentTypes;
            }
        }

        /// <summary>
        /// Dictionary for Cabinet Controller: Key = 'Name of control technology' in UI, Value = 'firmwareversion' in csv
        /// </summary>
        private static Dictionary<string, string> _dicControllerControlTechnologies = null;
        public static Dictionary<string, string> ControllerControlTechnologies
        {
            get
            {
                if (_dicControllerControlTechnologies == null)
                {
                    _dicControllerControlTechnologies = SLVHelper.SendRequestGetControlTechnologies();
                }

                return _dicControllerControlTechnologies;
            }
        }

        #region Public setting methods

        /// <summary>
        /// Get assembly path
        /// </summary>
        public static string AssemblyPath
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                path = path.Replace(";", ":");

                return Path.GetDirectoryName(path);
            }
        }

        /// <summary>
        /// Get Downloads folder path
        /// </summary>
        public static string DownloadsPath
        {
            get
            {
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
            }
        }

        /// <summary>
        /// Get current Date Time of SLV Server
        /// </summary>
        /// <returns></returns>
        public static DateTime GetServerTime()
        {
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(ServerTimeZoneName);
            return TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, timeZone);
        }        

        /// <summary>
        /// Get current TimeZone of SLV Server
        /// </summary>
        /// <returns></returns>
        public static int GetServerTimeZone()
        {
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(ServerTimeZoneName);
            return timeZone.BaseUtcOffset.Hours;
        }

        /// <summary>
        /// Get current Date Time of Local Machine
        /// </summary>
        /// <returns></returns>
        public static DateTime GetLocalTime()
        {
            return DateTime.Now;
        }

        /// <summary>
        /// Get current TimeZone of Local
        /// </summary>
        /// <returns></returns>
        public static int GetLocalTimeZone()
        {
            return TimeZoneInfo.Local.BaseUtcOffset.Hours;
        }

        /// <summary>
        /// Get current Date Time of a windows timezone
        /// </summary>
        /// <returns></returns>
        public static DateTime GetDateTimeByTimeZoneId(string id)
        {
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(id);
            return TimeZoneInfo.ConvertTime(DateTime.Now, timeZone);
        }

        /// <summary>
        /// Get current Date Time of an olson timezone (e.g. Europe/Paris)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static DateTime GetDateTimeByOlsonTimeZoneId(string id)
        {
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(ConvertOlsonTimeZoneToWindowsTimeZone(id));
            return TimeZoneInfo.ConvertTime(DateTime.Now, timeZone);
        }

        /// <summary>
        /// Get current date time of controler
        /// </summary>
        /// <param name="controlerId"></param>
        /// <returns></returns>
        public static DateTime GetCurrentControlerDateTime(string controlerId)
        { 
            return SLVHelper.SendRequestGetControllerDateTime(controlerId);
        }

        /// <summary>
        /// Converts an Olson time zone ID to a Windows time zone ID.
        /// </summary>
        /// <param name="olsonTimeZoneId">An Olson time zone ID. See http://unicode.org/repos/cldr-tmp/trunk/diff/supplemental/zone_tzid.html. </param>
        /// <returns>
        /// The TimeZoneInfo corresponding to the Olson time zone ID, 
        /// or null if you passed in an invalid Olson time zone ID.
        /// </returns>
        /// <remarks>
        /// See http://unicode.org/repos/cldr-tmp/trunk/diff/supplemental/zone_tzid.html
        /// </remarks>
        public static string ConvertOlsonTimeZoneToWindowsTimeZone(string olsonTimeZoneId)
        {
            try
            {
                return OlsonWindowsTimeZones[olsonTimeZoneId];
            }
            catch { }
            return string.Empty;
        }

        public static string ConvertWindowsTimeZoneToOlsonTimeZone(string windowsTimeZoneId)
        {
            try
            {
                return OlsonWindowsTimeZones.FirstOrDefault(p => p.Value.Equals(windowsTimeZoneId)).Key;
            }
            catch { }            
            return string.Empty;
        }


        /// <summary>
        /// Get all days name of a year
        /// </summary>
        /// <param name="year"></param>
        /// <param name="dw"></param>
        /// <returns></returns>
        public static List<DateTime> GetAllDaysOfWeek(int year, params DayOfWeek[] days)
        {
            List<DateTime> listOfDates = new List<DateTime>();

            foreach (var dw in days)
            {
                DateTime firstDayOfWeek = FindFirstDayOfWeek(year, dw);

                for (DateTime currentDateTime = firstDayOfWeek; currentDateTime.Year == year;
                currentDateTime = currentDateTime.AddDays(7))
                {
                    listOfDates.Add(currentDateTime);
                }
            }
            listOfDates.Sort();

            return listOfDates;
        }


        /// <summary>
        /// Get all day name of a year of specific date
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static List<DateTime> GetAllDaysOfWeek(DateTime date)
        {
            return GetAllDaysOfWeek(date.Year, date.DayOfWeek);
        }        

        /// <summary>
        /// Get first day name of a year
        /// </summary>
        /// <param name="year"></param>
        /// <param name="dw"></param>
        /// <returns></returns>
        public static DateTime FindFirstDayOfWeek(int year, DayOfWeek dw)
        {
            for (int i = 1; i <= 7; ++i)
            {
                DateTime currentDate = new DateTime(year, 1, i);
                if (currentDate.DayOfWeek == dw)
                {
                    return currentDate;
                }
            }
            throw new Exception("Impossible! Find First Day Of Week In Year.");
        }

        /// <summary>
        ///  Get first day name of a month of a year
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="dw"></param>
        /// <returns></returns>
        public static DateTime FindFirstDayOfWeek(int year, int month, DayOfWeek dw)
        {
            for (int i = 1; i <= 7; ++i)
            {
                DateTime currentDate = new DateTime(year, month, i);
                if (currentDate.DayOfWeek == dw)
                {
                    return currentDate;
                }
            }
            throw new Exception("Impossible! Find First Day Of Week In Year.");
        }

        /// <summary>
        /// Get first day name of a year
        /// </summary>
        /// <param name="year"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime FindFirstDayOfWeekInYear(int year, DateTime date)
        {
            return FindFirstDayOfWeek(year, date.DayOfWeek);
        }

        /// <summary>
        /// Get the last day of a month
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public static DateTime GetLastDayOfMonth(int year, int month)
        {
            var firstDayMonth = new DateTime(year, month, 1);

            return firstDayMonth.AddMonths(1).AddDays(-1);
        }

        /// <summary>
        /// Get all last day of month in a year
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static List<DateTime> GetAllLastDaysOfMonth(int year)
        {
            var result = new List<DateTime>();
            for (int i = 1; i <= 12; i++)
            {
                result.Add(GetLastDayOfMonth(year, i));
            }

            return result;
        }

        /// <summary>
        /// Get next weeday
        /// </summary>
        /// <param name="start"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        public static DateTime GetNextWeekday(DateTime start, DayOfWeek day)
        {
            // The (... + 7) % 7 ensures we end up with a value in the range [0, 6]
            int daysToAdd = ((int)day - (int)start.DayOfWeek + 7) % 7;
            return start.AddDays(daysToAdd);
        }

        public static DateTime GetNextWeekday(DateTime start, string dayOfWeekName)
        {
            var day = GetDayOfWeek(dayOfWeekName);
            // The (... + 7) % 7 ensures we end up with a value in the range [0, 6]
            int daysToAdd = ((int)day - (int)start.DayOfWeek + 7) % 7;
            return start.AddDays(daysToAdd);
        }

        /// <summary>
        /// Get Day Of Week by name string
        /// </summary>
        /// <param name="dayOfWeekName"></param>
        /// <returns></returns>
        public static DayOfWeek GetDayOfWeek(string dayOfWeekName)
        {
            if (Enum.IsDefined(typeof(DayOfWeek), dayOfWeekName))
            {
                return (DayOfWeek)Enum.Parse(typeof(DayOfWeek), dayOfWeekName, true);
            }
            throw new Exception(dayOfWeekName + " is invalid");
        }

        /// <summary>
        /// Check a date time matchs formats
        /// </summary>
        /// <param name="dateValue"></param>
        /// <param name="formats"></param>
        /// <returns></returns>
        public static bool CheckDateTimeMatchFormats(string dateValue, params string[] formats)
        {
            DateTime dt;
            if (!DateTime.TryParseExact(dateValue, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deviceType"></param>
        /// <param name="typeOfEquipment"></param>
        /// <returns></returns>
        public static string GetModelOfDevice(DeviceType deviceType, string typeOfEquipment)
        {
            if (deviceType == DeviceType.Streetlight)
                return StreetlightEquipmentTypes[typeOfEquipment];
            if (deviceType == DeviceType.Switch)
                return SwitchEquipmentTypes[typeOfEquipment];
            if (deviceType == DeviceType.ElectricalCounter)
                return ElectricalCounterEquipmentTypes[typeOfEquipment];
            if (deviceType == DeviceType.CabinetController)
                return CabinetControllerEquipmentTypes[typeOfEquipment];

            return string.Empty;
        }

        #endregion

        #region Private setting methods

        /// <summary>
        /// Get the full path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetFullPath(string path)
        {
            return Path.Combine(AssemblyPath, path);
        }

        private static DeviceModel InitStreetlight(XmlNode node, DeviceStatus status)
        {
            var device = new DeviceModel();
            device.Type = DeviceType.Streetlight;
            device.Status = status;
            device.Name = node.GetAttrVal("name");
            device.Id = node.GetAttrVal("id");
            device.Controller = node.GetAttrVal("controller");
            device.Latitude = node.GetAttrVal("latitude");
            device.Longitude = node.GetAttrVal("longitude");
            device.TypeOfEquipment = node.GetAttrVal("typeOfEquipment");
            device.UniqueAddress = node.GetAttrVal("uniqueAddress");
            device.DimmingGroup = node.GetAttrVal("dimmingGroup");
            device.Cluster = node.GetAttrVal("cluster");

            return device;
        }

        private static DeviceModel InitController(XmlNode node, DeviceStatus status)
        {
            var device = new DeviceModel();
            device.Type = DeviceType.Controller;
            device.Status = status;
            device.Name = node.GetAttrVal("name");
            device.Id = node.GetAttrVal("id");
            device.Latitude = node.GetAttrVal("latitude");
            device.Longitude = node.GetAttrVal("longitude");
            device.ControlTechnology = node.GetAttrVal("controlTechnology");
            device.UniqueAddress = node.GetAttrVal("uniqueAddress");
            device.TimeZone = node.GetAttrVal("timeZone");

            return device;
        }

        #endregion //Private methods
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class DynamicRetry : PropertyAttribute, IWrapSetUpTearDown
    {
        private const int DEFAULT_TRIES = 1;
        static Lazy<int> numberOfRetries = new Lazy<int>(() =>
        {
            int count = 0;
            return int.TryParse(ConfigurationManager.AppSettings.Get("RETRY_COUNT"), out count) ? count : DEFAULT_TRIES;
        });

        /// <summary>
        /// Construct a RepeatAttribute
        /// </summary>
        /// <param name="count">The number of times to run the test</param>
        public DynamicRetry() : base(numberOfRetries.Value)
        {
        }

        #region IWrapSetUpTearDown Members

        /// <summary>
        /// Wrap a command and return the result.
        /// </summary>
        /// <param name="command">The command to be wrapped</param>
        /// <returns>The wrapped command</returns>
        public TestCommand Wrap(TestCommand command)
        {
            return new CustomRetryCommand(command, numberOfRetries.Value);
        }

        #endregion

        #region Nested CustomRetry Class

        /// <summary>
        /// The test command for the RetryAttribute
        /// </summary>
        public class CustomRetryCommand : DelegatingTestCommand
        {
            private int _retryCount;

            /// <summary>
            /// Initializes a new instance of the <see cref="CustomRetryCommand"/> class.
            /// </summary>
            /// <param name="innerCommand">The inner command.</param>
            /// <param name="retryCount">The number of repetitions</param>
            public CustomRetryCommand(TestCommand innerCommand, int retryCount)
                : base(innerCommand)
            {
                _retryCount = retryCount;
            }

            /// <summary>
            /// Runs the test, saving a TestResult in the supplied TestExecutionContext.
            /// </summary>
            /// <param name="context">The context in which the test should run.</param>
            /// <returns>A TestResult</returns>
            public override TestResult Execute(TestExecutionContext context)
            {
                int count = _retryCount;

                while (count-- > 0)
                {
                    context.CurrentResult = innerCommand.Execute(context);
                    var results = context.CurrentResult.ResultState;

                    if (results != ResultState.Error
                        && results != ResultState.Failure
                        && results != ResultState.SetUpError
                        && results != ResultState.SetUpFailure
                        && results != ResultState.TearDownError
                        && results != ResultState.ChildFailure)
                    {                        
                        break;
                    }

                    // Clear result for retry
                    if (count > 0) context.CurrentResult = context.CurrentTest.MakeTestResult();
                }

                return context.CurrentResult;
            }
        }

        #endregion
    }
}
