using NUnit.Framework;
using StreetlightVision.Extensions;
using StreetlightVision.Models;
using StreetlightVision.Pages;
using StreetlightVision.Utilities;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;

namespace StreetlightVision.Tests.DataSetup
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    public class TestingDataCleaner : TestBase
    {
        #region Variables

        #endregion //Variables

        #region Contructors

        #endregion //Contructors        

        #region Test Cases

        [Test]
        [Description("Delete testing alarms in Alarm Manager")]
        [NonParallelizable]
        public void DeleteTestingAlarms()
        {
            SLVHelper.SendRequestDeleteAlarms();
        }

        [Test]
        [Description("Delete testing reports in Report Manager")]
        [NonParallelizable]
        public void DeleteTestingReports()
        {
            var loginPage = Browser.OpenCMS();
            var desktopPage = loginPage.LoginAsValidUser(Settings.Users["DefaultTest"].Username, Settings.Users["DefaultTest"].Password);
            desktopPage.InstallAppsIfNotExist(App.ReportManager);
            var reportManagerPage = desktopPage.GoToApp(App.ReportManager) as ReportManagerPage;

            var reports = reportManagerPage.GridPanel.GetListOfColumnData("Name");
            reports = reports.Where(p => Regex.IsMatch(p, Settings.DeletableNamePattern)).ToList();
            foreach (var report in reports)
            {
                reportManagerPage.DeleteReport(report, true);
            }
        }

        [Test, Order(3)]
        [Description("Delete all testing devices")]
        [NonParallelizable]
        public void DeleteTestingDevices()
        {
            var controlerId = "vietnamcontroller";
            var devices = SLVHelper.SendRequestGetDevicesByControllerId(controlerId);
            var tempDevices = devices.Where(p => Regex.IsMatch(p.Key, Settings.DeletableNamePattern) || p.Key.StartsWith("New-") || p.Value.StartsWith("New-"));
            foreach (var device in tempDevices)
            {
                SLVHelper.SendRequestDeleteDevice(controlerId, device.Key);
            }

            devices = SLVHelper.SendRequestGetDevicesByGeozone("Automation");
            tempDevices = devices.Where(p => Regex.IsMatch(p.Value, Settings.DeletableNamePattern));
            foreach (var device in tempDevices)
            {
                var arr = device.Key.SplitEx(new string[] { "#" });
                var controllerId = arr[1];
                var deviceId = arr[0];
                SLVHelper.SendRequestDeleteDevice(controllerId, deviceId);
            }            

            //Delete controllers
            var controllers = SLVHelper.SendRequestGetAllControllers();
            var tempControllers = controllers.Where(p => Regex.IsMatch(p.Key, Settings.DeletableNamePattern));
            foreach (var controller in tempControllers)
            {
                SLVHelper.SendRequestDeleteController(controller.Key);
            }
        }

        [Test, Order(1)]
        [Description("Delete testing profiles and users")]
        [NonParallelizable]
        public void DeleteTestingProfilesAndUsers()
        {
            var users = SLVHelper.SendRequestGetAllUsers();
            var tempUsers = users.Where(p => Regex.IsMatch(p.Key, "TS1.*Profile") || Regex.IsMatch(p.Key, @"(.*\d{8}.*)")).ToList();

            foreach (var user in tempUsers)
            {
                SLVHelper.SendRequestDeleteUserAndProfile(user.Key, user.Value);
            }
        }

        [Test, Order(4)]
        [Description("Delete testing control programs and calendars")]
        [NonParallelizable]
        public void DeleteTestingControlProgramsAndCalendars()
        {
            var calendars = SLVHelper.SendRequestGetCalendars();
            var tempCalendars = calendars.Where(p => Regex.IsMatch(p.Value, Settings.DeletableNamePattern) || Regex.IsMatch(p.Value, "New calendar.*") || Regex.IsMatch(p.Value, @"\b[a-zA-Z]{5}\b") || Regex.IsMatch(p.Value, @"\b[a-zA-Z]{6}\b"));
            foreach (var calendar in tempCalendars)
            {
                SLVHelper.SendRequestDeleteCalendar(calendar.Key);
            }

            var controlPrograms = SLVHelper.SendRequestGetControlPrograms();
            var tempControlPrograms = controlPrograms.Where(p => Regex.IsMatch(p.Value, Settings.DeletableNamePattern) || Regex.IsMatch(p.Value, "New control program.*"));
            foreach (var controlProgram in tempControlPrograms)
            {
                SLVHelper.SendRequestDeleteControlProgram(controlProgram.Key);
            }
        }

        [Test, Order(2)]
        [Description("Delete testing geozones")]
        [NonParallelizable]
        public void DeleteTestingGeozones()
        {
            var geozones = SLVHelper.SendRequestGetAllGeozones();
            var tempGeozones = geozones.Where(p => Regex.IsMatch(p.Value, Settings.DeletableNamePattern));
            foreach (var geozone in tempGeozones)
            {
                SLVHelper.SendRequestDeleteGeozone(geozone.Key);
            }
        }

        #endregion //Test Cases

        #region Private methods

        #endregion //Private methods
    }
}
