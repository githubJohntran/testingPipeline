using NUnit.Framework;
using StreetlightVision.Extensions;
using StreetlightVision.Models;
using StreetlightVision.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;

namespace StreetlightVision.Tests
{
    public class TestBase
    {
        [OneTimeSetUp]
        public virtual void SetUpBeforeSuite()
        {
        }

        [SetUp]
        public virtual void SetUpBeforeEachTest()
        {
            WebDriverContext.NewDriverForCurrentTest();
        }

        [TearDown]
        public virtual void TearDownAfterEachTest()
        {
            WebDriverContext.QuitDriverOfCurrentTest();
        }

        [OneTimeTearDown]
        public virtual void TearDownAfterSuite()
        {
        }

        public void Info(string info)
        {
            if (Settings.StepLoggingEnabled)
            {
                Console.WriteLine("[INFO] " + info);
            }
        }

        public void Info(string info, params object[] arg)
        {
            if (Settings.StepLoggingEnabled)
            {
                Console.WriteLine("[INFO] " + info, arg);
            }
        }

        public void Step(string description)
        {
            if (Settings.StepLoggingEnabled)
            {
                Console.WriteLine(description);
            }
        }

        public void Step(string description, params object[] arg)
        {
            if (Settings.StepLoggingEnabled)
            {
                Console.WriteLine(description, arg);
            }
        }

        public static void Warning(string message)
        {
            Assert.Warn(string.Format("*** {0} ***", message));
        }

        public static void VerifyEqual(string message, object expected, object actual)
        {
            if (expected == null)
            {
                Warn.Unless(expected == actual, "\nFAILED: " + message + string.Format("\n  [EXPECTED]: {0}\n  [ACTUAL]: {1}", expected, actual));
            }
            else
            {
                Warn.Unless(expected.Equals(actual), "\nFAILED: " + message + string.Format("\n  [EXPECTED]: {0}\n  [ACTUAL]: {1}", expected, actual));
            }
        }

        public static void VerifyEqual(string message, DateTime expected, DateTime actual)
        {
            Warn.Unless(expected.Equals(actual), "\nFAILED: " + message + string.Format("\n  [EXPECTED]: {0}\n  [ACTUAL]: {1}", expected, actual));
        }

        public static void VerifyEqual(string message, string expected, string actual)
        {
            Warn.Unless(expected.Equals(actual), "\nFAILED: " + message + string.Format("\n  [EXPECTED]: {0}\n  [ACTUAL]: {1}", expected, actual));
        }

        public static void VerifyEqual(string message, Color expected, Color actual)
        {
            Warn.Unless(expected == actual, "\nFAILED: " + message + string.Format("\n  [EXPECTED]: {0}\n  [ACTUAL]: {1}", expected, actual));
        }

        public static void VerifyEqual(string message, List<string> expectedList, List<string> actualList, bool IsSequenceChecked = true)
        {
            Warn.Unless(expectedList.Count == actualList.Count, "\nFAILED:" + message + "(2 lists have different item count)" + string.Format("\n  EXPECTED: {0} [{1}]\n  ACTUAL: {2} [{3}]", expectedList.Count, string.Join(", ", expectedList), actualList.Count, string.Join(", ", actualList)));

            if (IsSequenceChecked)
            {
                if (expectedList.Count == actualList.Count)
                {
                    for (var i = 0; i < expectedList.Count; i++)
                    {
                        Warn.Unless(expectedList[i] == actualList[i], "\nFAILED: " + message + string.Format("\n  Item {0}: EXPECTED: {1}\n  ACTUAL: {2}", i, expectedList[i], actualList[i]));
                    }
                }
            }
            else
            {
                Warn.Unless(actualList.CheckIfIncluded(expectedList), "\nFAILED: " + message + string.Format("\n  EXPECTED: {0}\n  ACTUAL: {1}", string.Join(", ", expectedList), string.Join(", ", actualList)));
            }
        }

        public static void VerifyEqual(string message, Dictionary<string, string> expected, Dictionary<string, string> actual)
        {
            Warn.Unless(expected.Count == actual.Count, "\nFAILED:" + message + "(2 dictionaries have different item count)" + string.Format("\n  EXPECTED: {0} [{1}]\n  ACTUAL: {2} [{3}]", expected.Count, string.Join(", ", expected.Values.ToArray()), actual.Count, string.Join(", ", actual.Values.ToArray())));

            if (expected.Count == actual.Count)
            {
                foreach (var key in expected.Keys)
                {
                    Warn.Unless(actual.ContainsKey(key), "\nFAILED: " + message + string.Format("\n actual dictionary does not contain key {0}", key));
                    if (actual.ContainsKey(key))
                    {
                        VerifyEqual(message + string.Format(" [{0}]", key), expected[key], actual[key]);
                    }
                }
            }
        }

        public static void VerifyContain(string message, List<string> parentList, List<string> childList)
        {
            Warn.Unless(parentList.CheckIfIncluded(childList), "\nFAILED: " + message + string.Format("\n  EXPECTED: {0}\n  ACTUAL: {1}", string.Join(", ", childList), string.Join(", ", parentList)));
        }

        public static void VerifyEqual(string message, DataTable expectedDataTable, DataTable actualDataTable)
        {
            Warn.Unless(expectedDataTable.Rows.Count == actualDataTable.Rows.Count, "\nFAILED:" + message + "(2 tables have different item count)" + string.Format("\n  EXPECTED: {0}\n  ACTUAL: {1}", expectedDataTable.Rows.Count, actualDataTable.Rows.Count));

            Warn.Unless(expectedDataTable.Columns.Count == actualDataTable.Columns.Count, "\nFAILED:" + message + "(2 tables have different item count)" + string.Format("\n  EXPECTED: {0}\n  ACTUAL: {1}", expectedDataTable.Columns.Count, actualDataTable.Columns.Count));

            foreach (DataColumn column in expectedDataTable.Columns)
            {
                Warn.Unless(actualDataTable.Columns.Contains(column.ColumnName), "\nFAILED: " + string.Format("Actual table doesn't have column {0}", column.ColumnName));
            }

            for (var i = 0; i < expectedDataTable.Rows.Count; i++)
            {
                foreach (DataColumn column in expectedDataTable.Columns)
                {
                    if (!actualDataTable.Columns.Contains(column.ColumnName)) break;
                    Warn.Unless(expectedDataTable.Rows[i][column.ColumnName].Equals(actualDataTable.Rows[i][column.ColumnName]), "\nFAILED: " + string.Format("Value of row[{0}][{1}]\n  EXPECTED: {2}\n  ACTUAL: {3}", i + 1, column.ColumnName, expectedDataTable.Rows[i][column.ColumnName], actualDataTable.Rows[i][column.ColumnName]));
                }
            }
        }

        public static void VerifyTrue(string message, bool condition, object expected, object actual)
        {
            Warn.Unless(condition, "\nFAILED: " + message + string.Format("\n  EXPECTED: {0}\n  ACTUAL: {1}", expected, actual));
        }

        /// <summary>
        /// Send request to create new profile and user with current test method name
        /// </summary>
        /// <returns></returns>
        public UserModel CreateNewProfileAndUser(string language = "", string skin = "", string geozone = "")
        {
            if (string.IsNullOrEmpty(geozone)) geozone = Settings.RootGeozoneName;
            var userModel = new UserModel();
            var timetamp = DateTime.Now.Timestamp();
            var method = TestContext.CurrentContext.Test.MethodName.Replace("_", string.Empty);
            userModel.Profile = string.Format("{0}{1} Profile", method, timetamp);
            userModel.Username = string.Format("{0}{1}", method, timetamp);
            userModel.Password = Settings.Users["DefaultTest"].Password;
            userModel.Email = Settings.Users["DefaultTest"].Email;
            userModel.FullName = userModel.Username;
            if (!SLVHelper.SendRequestCreateProfile(userModel.Profile, geozone, language, skin))
            {
                Warning(string.Format("Cannot created Profile '{0}'", userModel.Profile));
                return null;
            }
            if (!SLVHelper.SendRequestCreateUser(userModel.Profile, userModel.Username, userModel.Password, userModel.Email))
            {
                Warning(string.Format("Cannot created Username '{0}'", userModel.Username));
                return null;
            }

            return userModel;
        }

        /// <summary>
        /// Send request to create a testing user
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public UserModel CreateNewTestingUser(string name)
        {
            var geozone = Settings.RootGeozoneName;
            var userModel = new UserModel();
            var timetamp = DateTime.Now.Timestamp();
            userModel.Profile = string.Format("{0}{1} Profile", name, timetamp);
            userModel.Username = string.Format("{0}{1}", name, timetamp);
            userModel.Password = Settings.Users["DefaultTest"].Password;
            userModel.Email = Settings.Users["DefaultTest"].Email;
            userModel.FullName = userModel.Username;
            if (!SLVHelper.SendRequestCreateProfile(userModel.Profile, geozone))
            {
                Warning(string.Format("Cannot created Profile '{0}'", userModel.Profile));
                return null;
            }
            if (!SLVHelper.SendRequestCreateUser(userModel.Profile, userModel.Username, userModel.Password, userModel.Email))
            {
                Warning(string.Format("Cannot created Username '{0}'", userModel.Username));
                return null;
            }

            return userModel;
        }

        /// <summary>
        /// Send request to create a new geozone
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parentName"></param>
        /// <param name="latMin"></param>
        /// <param name="latMax"></param>
        /// <param name="lngMin"></param>
        /// <param name="lngMax"></param>
        public void CreateNewGeozone(string name, string parentName = "", string latMin = "", string latMax = "", string lngMin = "", string lngMax = "")
        {
            if (string.IsNullOrEmpty(parentName)) parentName = Settings.RootGeozoneName;
            if (string.IsNullOrEmpty(latMin)) latMin = SLVHelper.GenerateLatitude(1, 49);
            if (string.IsNullOrEmpty(latMax)) latMax = SLVHelper.GenerateLatitude(50, 89);
            if (string.IsNullOrEmpty(lngMin)) lngMin = SLVHelper.GenerateLongitude(1, 89);
            if (string.IsNullOrEmpty(lngMax)) lngMax = SLVHelper.GenerateLongitude(90, 179);
            var id = SLVHelper.SendRequestCreateGeozone(name, parentName, latMin, latMax, lngMin, lngMax);
            if (id < 0) Warning(string.Format("Cannot create geozone '{0}'", name));
        }

        /// <summary>
        /// Send request to create a new device with default infos
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="controllerId"></param>
        /// <param name="parentGeozone"></param>
        /// <param name="typeOfEquipment"></param>
        /// <param name="lat"></param>
        /// <param name="lng"></param>
        public void CreateNewDevice(DeviceType type, string name, string controllerId, string parentGeozone, string typeOfEquipment = "", string lat = "", string lng = "")
        {
            if (string.IsNullOrEmpty(typeOfEquipment))
            {
                if (type == DeviceType.Streetlight) typeOfEquipment = "SSN Cimcon Dim Photocell[Lamp #0]";
                else if (type == DeviceType.Switch) typeOfEquipment = "ABEL-Vigilon A[Switch]";
                else if (type == DeviceType.ElectricalCounter) typeOfEquipment = "CIRWATT MINI[Counter]";
                else if (type == DeviceType.CabinetController) typeOfEquipment = "EDMI Mk10E[cabinetFunction6]";
            }
            if (string.IsNullOrEmpty(lat)) lat = SLVHelper.GenerateLatitude();
            if (string.IsNullOrEmpty(lng)) lng = SLVHelper.GenerateLongitude();
            var model = Settings.GetModelOfDevice(type, typeOfEquipment);
            var isCreated = SLVHelper.SendRequestCreateDevice(type.Category, name, name, model, controllerId, parentGeozone, lat, lng);
            if (!isCreated) Warning(string.Format("Cannot create device '{0}'(Model: {1} - Type Of Equipment: {2})", name, model, typeOfEquipment));
        }

        /// <summary>
        /// Send request to create a new controller with default infos
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parentGeozone"></param>
        /// <param name="controlTechnology"></param>
        /// <param name="lat"></param>
        /// <param name="lng"></param>
        /// <param name="host"></param>
        public void CreateNewController(string name, string parentGeozone, string controlTechnology = "Open South Bound XML Web API", string lat = "", string lng = "", string host = "localhost")
        {
            if (string.IsNullOrEmpty(lat)) lat = SLVHelper.GenerateLatitude();
            if (string.IsNullOrEmpty(lng)) lng = SLVHelper.GenerateLongitude();
            var firmware = Settings.ControllerControlTechnologies[controlTechnology];
            var isCreated = SLVHelper.SendRequestCreateController(name, name, firmware, parentGeozone, lat, lng, host);
            if (!isCreated) Warning(string.Format("Cannot create controller '{0}'(FirmwareVersion: {1} - Control Technology: {2})", name, firmware, controlTechnology));
        }

        /// <summary>
        /// Send request to create new control program
        /// </summary>
        /// <param name="name"></param>
        /// <param name="timeline">Either T_12_12 for noon-to-noon or T_00_24 for midnight-to-midnight</param>
        /// <param name="description"></param>
        /// <param name="sunbases">list of sun bases (e.g. event: 100#SUNSET#0)</param>
        /// <param name="events">list of events (e.g. event: 55#10:20:00)</param>  
        public long CreateNewControlProgram(string name, string timeline, string description, List<string> sunbases, List<string> events)
        {
            var controlProgramId = SLVHelper.SendRequestCreateControlProgram(name, timeline, SLVHelper.GenerateHexColor(), description);
            if (controlProgramId != -1)
            {
                foreach (var sun in sunbases)
                {
                    var level = int.Parse(sun.SplitAndGetAt(new string[] { "#" }, 0));
                    var sunType = sun.SplitAndGetAt(new string[] { "#" }, 1);
                    var startOffsetInSeconds = int.Parse(sun.SplitAndGetAt(new string[] { "#" }, 2));
                    SLVHelper.SendRequestAddSunBasedControlProgram(controlProgramId, level, sunType, startOffsetInSeconds);
                }

                foreach (var eventPoint in events)
                {
                    var level = int.Parse(eventPoint.SplitAndGetAt(new string[] { "#" }, 0));
                    var time = eventPoint.SplitAndGetAt(new string[] { "#" }, 1);
                    var hour = int.Parse(time.SplitAndGetAt(new string[] { ":" }, 0));
                    var minute = int.Parse(time.SplitAndGetAt(new string[] { ":" }, 1));
                    var second = int.Parse(time.SplitAndGetAt(new string[] { ":" }, 2));
                    SLVHelper.SendRequestAddEventControlProgram(controlProgramId, level, hour, minute, second);
                }
            }
            else
                Warning(string.Format("Cannot create control program '{0}'", name));

            return controlProgramId;
        }

        /// <summary>
        /// Send request to create a new control program with Advanced Mode template
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        public void CreateNewControlProgramAdvancedMode(string name, string description = "")
        {
            CreateNewControlProgram(name, "T_12_12", description, new List<string> { "100#SUNSET#0", "0#SUNRISE#0" }, new List<string> { "90#5:0:0", "77#23:0:0" });
        }

        /// <summary>
        /// Send request to create a new control program
        /// </summary>
        /// <param name="name"></param>
        /// <param name="timeline"></param>
        /// <param name="hexColor"></param>
        /// <param name="description"></param>
        /// <param name="template"></param>
        /// <returns></returns>
        public long CreateNewControlProgram(string name, string timeline, string hexColor, string description, string template = "")
        {
            return SLVHelper.SendRequestCreateControlProgram(name, timeline, hexColor, description, template);
        }

        /// <summary>
        /// Send request to add event to control program
        /// </summary>
        /// <param name="id"></param>
        /// <param name="level"></param>
        /// <param name="startHour"></param>
        /// <param name="startMinute"></param>
        /// <param name="startSecond"></param>
        /// <returns></returns>
        public bool AddEventControlProgram(long id, int level, int startHour, int startMinute, int startSecond)
        {
            return SLVHelper.SendRequestAddEventControlProgram(id, level, startHour, startMinute, startSecond);
        }

        /// <summary>
        /// Send request to associate control program to calendar yearly
        /// </summary>
        /// <param name="controlProgramId"></param>
        /// <param name="calendarId"></param>
        /// <param name="fromMonth"></param>
        /// <param name="fromDay"></param>
        /// <param name="toMonth"></param>
        /// <param name="toDay"></param>
        /// <returns></returns>
        public bool AssociateControlProgramToCalendarYearly(long controlProgramId, long calendarId, int fromMonth, int fromDay, int toMonth, int toDay)
        {
            return SLVHelper.SendRequestAssociateControlProgramToCalendarYearly(controlProgramId, calendarId, fromMonth, fromDay, toMonth, toDay);
        }

        /// <summary>
        /// Send request to associate control program to calendar monthly
        /// </summary>
        /// <param name="controlProgramId"></param>
        /// <param name="calendarId"></param>
        /// <param name="fromDay"></param>
        /// <param name="lastDay"></param>
        /// <returns></returns>
        public bool AssociateControlProgramToCalendarMonthly(long controlProgramId, long calendarId, int fromDay, int lastDay)
        {
            return SLVHelper.SendRequestAssociateControlProgramToCalendarMonthly(controlProgramId, calendarId, fromDay, lastDay);
        }

        /// <summary>
        /// Send request to ceate new calendar
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        public long CreateNewCalendar(string name, string description = "")
        {
            var nameEncode = HttpUtility.UrlEncode(name);
            var calendarId = SLVHelper.SendRequestCreateCalendar(nameEncode, description);
            if (calendarId == -1) Warning(string.Format("Cannot create calendar '{0}'", name));

            return calendarId;
        }

        /// <summary>
        /// Send request to create new lamp type
        /// </summary>
        /// <param name="name"></param>
        public string CreateNewLampType(string name)
        {
            var brandId = SLVHelper.SendRequestCreateLampType(name);
            if (string.IsNullOrEmpty(brandId)) Warning(string.Format("Cannot create lamp type '{0}'", name));

            return brandId;
        }

        /// <summary>
        /// Send request to create new energy supplier
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pollutionRate"></param>
        public void CreateNewEnergySupplier(string name, string pollutionRate)
        {
            var id = SLVHelper.SendRequestCreateEnergySupplier(name, pollutionRate);
            if (string.IsNullOrEmpty(id)) Warning(string.Format("Cannot create energy supplier '{0}'", name));
        }

        /// <summary>
        /// Send request to import a file
        /// </summary>
        /// <param name="filePath"></param>
        public void ImportFile(string filePath)
        {
            var importedSuccess = SLVHelper.SendRequestDeviceImportFile(filePath);
            if (!importedSuccess) Warning(string.Format("Cannot import file ({0})", filePath));
        }

        /// <summary>
        /// Create a CSV file with number of devices at a location
        /// </summary>
        /// <param name="count"></param>
        /// <param name="deviceType"></param>
        /// <param name="csvFilePath"></param>
        /// <param name="geozoneFullPath"></param>
        /// <param name="controllerId"></param>
        /// <param name="namePrefix"></param>
        /// <param name="typeOfEquipment"></param>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <param name="fileAppend"></param>
        /// <returns>list of devices name</returns>
        public List<string> CreateCsvDevicesSameLocation(int count, DeviceType deviceType, string csvFilePath, string geozoneFullPath, string controllerId, string namePrefix, string typeOfEquipment, string latitude, string longitude, bool fileAppend = false)
        {
            var devices = new List<string>();
            var model = Settings.GetModelOfDevice(deviceType, typeOfEquipment);
            var content = new List<string>();
            content.Add("categoryStrId;controllerStrId;geoZone path;idOnController;lat;lng;name;model;macAddress");
            for (int i = 1; i <= count; i++)
            {
                var device = string.Format("{0}{1:D2}", namePrefix, i);
                devices.Add(device);
                var csvLine = string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8}", deviceType.Category, controllerId, geozoneFullPath, device, latitude, longitude, device, model, SLVHelper.GenerateMACAddress());
                content.Add(csvLine);
            }

            if (fileAppend)
            {
                content.RemoveAt(0);
                if (!SLVHelper.UpdateFile(csvFilePath, content.ToArray()))
                    Warning(string.Format("Cannot update CSV file ({0}). Please check again", csvFilePath));
            }
            else
            {
                if (!SLVHelper.CreateNewFile(csvFilePath, content.ToArray()))
                    Warning(string.Format("Cannot create CSV file ({0}). Please check again", csvFilePath));
            }

            return devices;
        }

        /// <summary>
        /// Create a CSV file with number of devices at random location
        /// </summary>
        /// <param name="count"></param>
        /// <param name="deviceType"></param>
        /// <param name="csvFilePath"></param>
        /// <param name="geozoneFullPath"></param>
        /// <param name="controllerId"></param>
        /// <param name="namePrefix"></param>
        /// <param name="typeOfEquipment"></param>
        /// <param name="fileAppend"></param>
        /// <returns>list of devices name</returns>
        public List<string> CreateCsvDevices(int count, DeviceType deviceType, string csvFilePath, string geozoneFullPath, string controllerId, string namePrefix, string typeOfEquipment = "", bool fileAppend = false)
        {
            var devices = new List<string>();
            if (string.IsNullOrEmpty(typeOfEquipment))
            {
                if (deviceType == DeviceType.Streetlight) typeOfEquipment = "SSN Cimcon Dim Photocell[Lamp #0]";
                else if (deviceType == DeviceType.Switch) typeOfEquipment = "ABEL-Vigilon A[Switch]";
                else if (deviceType == DeviceType.ElectricalCounter) typeOfEquipment = "CIRWATT MINI[Counter]";
                else if (deviceType == DeviceType.CabinetController) typeOfEquipment = "EDMI Mk10E[cabinetFunction6]";
            }
            var model = Settings.GetModelOfDevice(deviceType, typeOfEquipment);
            var content = new List<string>();
            content.Add("categoryStrId;controllerStrId;geoZone path;idOnController;lat;lng;name;model;macAddress");
            for (int i = 1; i <= count; i++)
            {
                var device = string.Format("{0}{1:D2}", namePrefix, i);
                devices.Add(device);
                var csvLine = string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8}", deviceType.Category, controllerId, geozoneFullPath, device, SLVHelper.GenerateLatitude(), SLVHelper.GenerateLongitude(), device, model, SLVHelper.GenerateMACAddress());
                content.Add(csvLine);
            }

            if (fileAppend)
            {
                content.RemoveAt(0);
                if (!SLVHelper.UpdateFile(csvFilePath, content.ToArray()))
                    Warning(string.Format("Cannot update CSV file ({0}). Please check again", csvFilePath));
            }
            else
            {
                if (!SLVHelper.CreateNewFile(csvFilePath, content.ToArray()))
                    Warning(string.Format("Cannot create CSV file ({0}). Please check again", csvFilePath));
            }

            return devices;
        }

        /// <summary>
        /// Create a CSV file with devices list
        /// </summary>
        /// <param name="csvFilePath"></param>
        /// <param name="geozoneFullPath"></param>
        /// <param name="devices"></param>
        public void CreateCsvDevices(string csvFilePath, string geozoneFullPath, List<DeviceModel> devices)
        {
            var content = new List<string>();
            content.Add("categoryStrId;controllerStrId;geoZone path;idOnController;lat;lng;name;model;macAddress");

            foreach (var device in devices)
            {
                var model = Settings.GetModelOfDevice(device.Type, device.TypeOfEquipment);
                var csvLine = string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8}", device.Type.Category, device.Controller, geozoneFullPath, device.Id, device.Latitude, device.Longitude, device.Name, model, device.UniqueAddress);
                content.Add(csvLine);
            }

            if (!SLVHelper.CreateNewFile(csvFilePath, content.ToArray()))
                Warning(string.Format("Cannot create CSV file ({0}). Please check again", csvFilePath));
        }

        /// <summary>
        ///  Create a CSV file with devices has no location
        /// </summary>
        /// <param name="csvFilePath"></param>
        /// <param name="geozoneFullPath"></param>
        /// <param name="devices"></param>
        public void CreateCsvDevicesHasNoLocation(string csvFilePath, string geozoneFullPath, List<DeviceModel> devices)
        {
            var content = new List<string>();
            content.Add("categoryStrId;controllerStrId;geoZone path;idOnController;name;model;macAddress");

            foreach (var device in devices)
            {
                var model = Settings.GetModelOfDevice(device.Type, device.TypeOfEquipment);
                var csvLine = string.Format("{0};{1};{2};{3};{4};{5};{6}", device.Type.Category, device.Controller, geozoneFullPath, device.Id, device.Name, model, device.UniqueAddress);
                content.Add(csvLine);
            }

            if (!SLVHelper.CreateNewFile(csvFilePath, content.ToArray()))
                Warning(string.Format("Cannot create CSV file ({0}). Please check again", csvFilePath));
        }

        /// <summary>
        /// Create a CSV file with a device at a location
        /// </summary>
        /// <param name="deviceType"></param>
        /// <param name="csvFilePath"></param>
        /// <param name="geozoneFullPath"></param>
        /// <param name="controllerId"></param>
        /// <param name="deviceId"></param>
        /// <param name="typeOfEquipment"></param>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <param name="macAddress"></param>
        /// <param name="deviceName"></param>
        /// <param name="fileAppend"></param>
        public void CreateCsv(DeviceType deviceType, string csvFilePath, string geozoneFullPath, string controllerId, string deviceId, string typeOfEquipment, string latitude, string longitude, string macAddress = "", string deviceName = "", bool fileAppend = false)
        {
            var model = string.Empty;
            if (!string.IsNullOrEmpty(typeOfEquipment)) model = Settings.GetModelOfDevice(deviceType, typeOfEquipment);
            if (string.IsNullOrEmpty(macAddress)) macAddress = SLVHelper.GenerateMACAddress();
            if (string.IsNullOrEmpty(deviceName)) deviceName = deviceId;
            var content = new List<string>();
            if (string.IsNullOrEmpty(model))
            {
                content.Add("categoryStrId;controllerStrId;geoZone path;idOnController;lat;lng;name;macAddress");
                content.Add(string.Format("{0};{1};{2};{3};{4};{5};{6};{7}", deviceType.Category, controllerId, geozoneFullPath, deviceId, latitude, longitude, deviceName, macAddress));
            }
            else
            {
                content.Add("categoryStrId;controllerStrId;geoZone path;idOnController;lat;lng;name;model;macAddress");
                content.Add(string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8}", deviceType.Category, controllerId, geozoneFullPath, deviceId, latitude, longitude, deviceName, model, macAddress));
            }

            if (fileAppend)
            {
                content.RemoveAt(0);
                if (!SLVHelper.UpdateFile(csvFilePath, content.ToArray()))
                    Warning(string.Format("Cannot update CSV file ({0}). Please check again", csvFilePath));
            }
            else
            {
                if (!SLVHelper.CreateNewFile(csvFilePath, content.ToArray()))
                    Warning(string.Format("Cannot create CSV file ({0}). Please check again", csvFilePath));
            }
        }

        /// <summary>
        /// Create a CSV file with a device 
        /// </summary>
        /// <param name="csvFilePath"></param>
        /// <param name="geozoneFullPath"></param>
        /// <param name="device"></param>
        /// <param name="isLocation"></param>
        /// <param name="fileAppend"></param>
        public void CreateCsv(string csvFilePath, string geozoneFullPath, DeviceModel device, bool isLocation = true, bool fileAppend = false)
        {
            var model = string.Empty;
            var content = new List<string>();
            if (!string.IsNullOrEmpty(device.TypeOfEquipment)) model = Settings.GetModelOfDevice(device.Type, device.TypeOfEquipment);

            var headerLine = "categoryStrId;controllerStrId;geoZone path;idOnController;name;macAddress";
            var contentLine = string.Format("{0};{1};{2};{3};{4};{5}", device.Type.Category, device.Controller, geozoneFullPath, device.Id, device.Name, device.UniqueAddress);

            if (!string.IsNullOrEmpty(model))
            {
                headerLine += ";model";
                contentLine += ";" + model;
            }

            if (isLocation)
            {
                headerLine += ";lat;lng";
                contentLine += ";" + device.Latitude + ";" + device.Longitude;
            }
            content.Add(headerLine);
            content.Add(contentLine);

            if (fileAppend)
            {
                content.RemoveAt(0);
                if (!SLVHelper.UpdateFile(csvFilePath, content.ToArray()))
                    Warning(string.Format("Cannot update CSV file ({0}). Please check again", csvFilePath));
            }
            else
            {
                if (!SLVHelper.CreateNewFile(csvFilePath, content.ToArray()))
                    Warning(string.Format("Cannot create CSV file ({0}). Please check again", csvFilePath));
            }
        }

        /// <summary>
        /// Create a CSV file with a device with some non-required fields
        /// </summary>
        /// <param name="deviceType"></param>
        /// <param name="csvFilePath"></param>
        /// <param name="geozoneFullPath"></param>
        /// <param name="controllerId"></param>
        /// <param name="deviceId"></param>
        /// <param name="typeOfEquipment"></param>
        /// <param name="deviceName"></param>
        /// <param name="nonRequiredItems"></param>
        public void CreateCsv(DeviceType deviceType, string csvFilePath, string geozoneFullPath, string controllerId, string deviceId, string typeOfEquipment, string deviceName = "", List<string> nonRequiredItems = null)
        {
            var model = Settings.GetModelOfDevice(deviceType, typeOfEquipment);
            if (string.IsNullOrEmpty(deviceName)) deviceName = deviceId;
            var content = new List<string>();
            var headerLine = "categoryStrId;controllerStrId;geoZone path;idOnController;model;name";
            var contentLine = string.Format("{0};{1};{2};{3};{4};{5}", deviceType.Category, controllerId, geozoneFullPath, deviceId, model, deviceName);

            if (nonRequiredItems != null)
            {
                foreach (var item in nonRequiredItems)
                {
                    var field = item.SplitAndGetAt("#", 0);
                    var value = item.SplitAndGetAt("#", 1);
                    headerLine += ";" + field;
                    contentLine += ";" + value;
                }
            }
            content.Add(headerLine);
            content.Add(contentLine);

            if (!SLVHelper.CreateNewFile(csvFilePath, content.ToArray()))
                Warning(string.Format("Cannot create CSV file ({0}). Please check again", csvFilePath));
        }

        /// <summary>
        ///  Create a CSV file with a device with list of fields
        /// </summary>
        /// <param name="deviceType"></param>
        /// <param name="csvFilePath"></param>
        /// <param name="Items"></param>    
        public void CreateCsv(DeviceType deviceType, string csvFilePath, List<string> Items = null)
        {
            var content = new List<string>();
            var headerLine = "categoryStrId";
            var contentLine = deviceType.Category;

            if (Items != null)
            {
                foreach (var item in Items)
                {
                    var field = item.SplitAndGetAt("#", 0);
                    var value = item.SplitAndGetAt("#", 1);
                    headerLine += ";" + field;
                    contentLine += ";" + value;
                }
            }
            content.Add(headerLine);
            content.Add(contentLine);

            if (!SLVHelper.CreateNewFile(csvFilePath, content.ToArray()))
                Warning(string.Format("Cannot create CSV file ({0}). Please check again", csvFilePath));
        }

        /// <summary>
        /// Send request to get device by name
        /// </summary>
        /// <param name="name"></param>
        /// <param name="geozoneName"></param>
        /// <returns></returns>
        public DeviceModel GetDevice(string name, string geozoneName = "")
        {
            return SLVHelper.SendRequestGetDevice(name, geozoneName);
        }

        /// <summary>
        /// Send request to get device by controllerId
        /// </summary>
        /// <param name="controllerId"></param>
        /// <returns></returns>
        public Dictionary<string,string> GetDevicesByControllerId(string controllerId)
        {
            return SLVHelper.SendRequestGetDevicesByControllerId(controllerId);
        }

        /// <summary>
        /// Send request to get device by geozone name
        /// </summary>
        /// <param name="controllerId"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetDevicesByGeozone(string name)
        {
            return SLVHelper.SendRequestGetDevicesByGeozone(name);
        }

        /// <summary>
        /// Send request to delete a device
        /// </summary>
        /// <param name="controllerId"></param>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        public bool DeleteDevice(string controllerId, string deviceId)
        {
            return SLVHelper.SendRequestDeleteDevice(controllerId, deviceId);
        }

        /// <summary>
        /// Send request to delete a controller
        /// </summary>
        /// <param name="controllerId"></param>
        /// <returns></returns>
        public bool DeleteController(string controllerId)
        {
            return SLVHelper.SendRequestDeleteController(controllerId);
        }

        /// <summary>
        /// Send request to delete devices by geozone
        /// </summary>
        /// <param name="geozoneName"></param>
        /// <param name="devicePattern"></param>
        /// <returns></returns>
        public bool DeleteDevicesByGeozone(string geozoneName, string devicePattern = "")
        {
            return SLVHelper.SendRequestDeleteDevicesByGeozone(geozoneName, devicePattern);
        }

        /// <summary>
        /// Send request to delete devices by controller Id
        /// </summary>
        /// <param name="controllerId"></param>
        /// <returns></returns>
        public bool DeleteDevicesByControllerId(string controllerId)
        {
            return SLVHelper.SendRequestDeleteDevicesByControllerId(controllerId);
        }

        /// <summary>
        /// Send request to delete a geozone
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool DeleteGeozone(string name)
        {
            return SLVHelper.SendRequestDeleteGeozone(name);
        }

        /// <summary>
        /// Send request to delete a geozone
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteGeozone(int id)
        {
            return SLVHelper.SendRequestDeleteGeozone(id);
        }

        /// <summary>
        /// Send request to delete geozones match a pattern
        /// </summary>
        /// <param name="namePattern"></param>
        /// <returns></returns>
        public bool DeleteGeozones(string namePattern)
        {
            return SLVHelper.SendRequestDeleteGeozones(namePattern);
        }

        /// <summary>
        /// Send request to delete an user and profile
        /// </summary>
        /// <param name="namePattern"></param>
        /// <returns></returns>
        public bool DeleteUserAndProfile(UserModel model)
        {
            return SLVHelper.SendRequestDeleteUserAndProfile(model);
        }

        /// <summary>
        /// Send request to delete profile
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool DeleteProfile(string name)
        {
            return SLVHelper.SendRequestDeleteProfile(name);
        }

        /// <summary>
        /// Send request to delete profiles match a pattern
        /// </summary>
        /// <param name="namePattern"></param>
        /// <returns></returns>
        public bool DeleteProfiles(string namePattern)
        {
            return SLVHelper.SendRequestDeleteProfiles(namePattern);
        }

        /// <summary>
        /// Send request to delete a calendar
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool DeleteCalendar(string name)
        {
            return SLVHelper.SendRequestDeleteCalendar(name);
        }

        /// <summary>
        /// Send request to delete a calendar
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteCalendar(long id)
        {
            return SLVHelper.SendRequestDeleteCalendar(id);
        }

        /// <summary>
        /// Send request to delete calendars match a pattern
        /// </summary>
        /// <param name="namePattern"></param>
        /// <returns></returns>
        public bool DeleteCalendars(string namePattern)
        {
            return SLVHelper.SendRequestDeleteCalendars(namePattern);
        }

        /// <summary>
        /// Send request to delete a control program
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool DeleteControlProgram(string name)
        {
            return SLVHelper.SendRequestDeleteControlProgram(name);
        }

        /// <summary>
        /// Send request to delete a control program
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteControlProgram(long id)
        {
            return SLVHelper.SendRequestDeleteControlProgram(id);
        }

        /// <summary>
        /// Send request to delete control programs match a pattern
        /// </summary>
        /// <param name="namePattern"></param>
        /// <returns></returns>
        public bool DeleteControlPrograms(string namePattern)
        {
            return SLVHelper.SendRequestDeleteControlPrograms(namePattern);
        }

        /// <summary>
        /// Delete a lamp type
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool DeleteLampType(string name)
        {
            return SLVHelper.SendRequestDeleteLampType(name);
        }

        /// <summary>
        /// Send request to delete lamp type match a pattern
        /// </summary>
        /// <param name="namePattern"></param>
        /// <returns></returns>
        public bool DeleteLampTypes(string namePattern)
        {
            return SLVHelper.SendRequestDeleteLampTypes(namePattern);
        }

        /// <summary>
        /// Send request to delete a energy supplier
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool DeleteEnergySupplier(string name)
        {
            return SLVHelper.SendRequestDeleteEnergySupplier(name);
        }

        /// <summary>
        /// Send request to delete energy suppliers match a pattern
        /// </summary>
        /// <param name="namePattern"></param>
        /// <returns></returns>
        public bool DeleteEnergySuppliers(string namePattern)
        {
            return SLVHelper.SendRequestDeleteEnergySuppliers(namePattern);
        }

        /// <summary>
        /// Send request to set value to device
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
        public bool SetValueToDevice(string controllerId, string deviceId, string valueName, dynamic value, DateTime eventTime, string comment = "", string user = "", string password = "")
        {
            return SLVHelper.SendRequestSetDeviceValuesToDevice(controllerId, deviceId, valueName, value, eventTime, comment, user, password);
        }

        /// <summary>
        /// Send request to set value to controller
        /// </summary>
        /// <param name="controllerId"></param>
        /// <param name="valueName"></param>
        /// <param name="value"></param>
        /// <param name="eventTime"></param>
        /// <param name="isUpdateTime"></param>
        /// <returns></returns>
        public bool SetValueToController(string controllerId, string valueName, dynamic value, DateTime eventTime, bool isUpdateTime = false)
        {
            return SLVHelper.SendRequestSetDeviceValuesToController(controllerId, valueName, value, eventTime, isUpdateTime);
        }

        /// <summary>
        /// Send request to commissioning a controller
        /// </summary>
        /// <param name="controllerId"></param>
        /// <returns></returns>
        public bool CommissionController(string controllerId)
        {
            return SLVHelper.SendRequestCommissionController(controllerId);
        }

        /// <summary>
        ///  Send request to exit manual mode of device
        /// </summary>
        /// <param name="controllerId"></param>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        public bool ExitManualMode(string controllerId, string deviceId)
        {
            return SLVHelper.SendRequestExitManualMode(controllerId, deviceId);
        }

        /// <summary>
        ///  Send request to update energy supplier
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pollutionRate"></param>
        /// <returns></returns>
        public bool UpdateEnergySupplier(string name, string pollutionRate)
        {
            return SLVHelper.SendRequestUpdateEnergySupplier(name, pollutionRate);
        }

        /// <summary>
        /// Send request to create alarm
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="action"></param>
        /// <param name="properties"></param>
        /// <returns>alarmDefinitionId</returns>
        public string CreateAlarm(string name, AlarmType type, string action, params string[] properties)
        {
            return SLVHelper.SendRequestCreateAlarm(name, type, action, properties);
        }

        /// <summary>
        /// Send request to update alarm
        /// </summary>
        /// <param name="alarmDefinitionId"></param>
        /// <param name="name"></param>
        /// <param name="properties"></param>
        /// <returns></returns>
        public bool UpdateAlarm(string alarmDefinitionId, string name, params string[] properties)
        {
            return SLVHelper.SendRequestUpdateAlarm(alarmDefinitionId, name, properties);
        }

        /// <summary>
        /// Send request to delete an alarm
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool DeleteAlarm(string name)
        {
            return SLVHelper.SendRequestDeleteAlarmByName(name);
        }

        /// <summary>
        /// Send request to delete alarms match a pattern
        /// </summary>
        /// <param name="pattern">if pattern is not filled, it will delete all alarms</param>
        /// <returns></returns>
        public bool DeleteAlarms(string pattern = "")
        {
            return SLVHelper.SendRequestDeleteAlarms(pattern);
        }
    }
}
