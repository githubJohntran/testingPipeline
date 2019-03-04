using StreetlightVision.Utilities;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;

namespace StreetlightVision.Models
{
    /// <summary>
    /// Define list of apps on desktop
    /// </summary>
    public class App
    {
        public const string AdvancedSearch = "Advanced Search";
        public const string AlarmManager = "Alarm Manager";
        public const string Alarms = "Alarms";
        public const string BatchControl = "Batch Control";
        public const string ControlCenter = "Control Center";
        public const string Dashboard = "Dashboard";
        public const string DataHistory = "Data History";
        public const string DeviceHistory = "Device History";
        public const string DeviceLifetime = "Lifetime";
        public const string Energy = "Energy";
        public const string EquipmentInventory = "Equipment Inventory";
        public const string FailureAnalysis = "Failure Analysis";
        public const string FailureTracking = "Failure Tracking";
        public const string Installation = "Installation";
        public const string InventoryVerification = "Inventory Verification";
        public const string LogViewer = "Log Viewer";
        public const string MonthlyEnergySaving = "Monthly Energy Saving";
        public const string RealTimeControl = "Real-Time Control";
        public const string ReportManager = "Report Manager";
        public const string SchedulingManager = "Scheduling Manager";
        public const string Users = "Users";
        public const string WorkOrders = "Work Orders";
        public const string BackOffice = "Back Office";

        public static List<string> GetList()
        {
            var type = typeof(App);
            var appsName = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy).Where(fi => fi.IsLiteral && !fi.IsInitOnly).Select(p => p.GetValue(null).ToString()).ToList();

            return appsName;
        }
    }

    /// <summary>
    /// Define list of widgets on desktop
    /// </summary>
    public class Widget
    {
        public static readonly string Gateway = "Gateway";
        public static readonly string LuminaireController = "Luminaire Controller";
        public static readonly string CircutorCVMMeter = "Circutor CVM Meter";
        public static readonly string ElectricityMeter = "Electricity Meter";
        public static readonly string Camera = "Camera";
        public static readonly string Weather = "Weather";
        public static readonly string Clock = "Clock";
        public static readonly string VehicleChargingStation = "Vehicle Charging Station";
        public static readonly string GeozoneFailuresMonitor = "GeoZone Failures Monitor";
        public static readonly string PollutionSensorWidget = "Pollution Sensor";
        public static readonly string XCamMonitor = "XCam Monitor";
        public static readonly string SunriseSunsetTimes = "Sunrise Sunset Times";
        public static readonly string IoTEdgeRouterStatus = "IoT Edge Router Status";
        public static readonly string SpoonyWidget = "Spoony";
        public static readonly string SecurityWidget = "Security";
        public static readonly string EnvironmentalSensor = "Environmental Sensor";

        public static List<string> GetList()
        {
            var type = typeof(Widget);
            var widgetsName = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy).Where(fi => fi.IsInitOnly).Select(p => p.GetValue(null).ToString()).ToList();

            return widgetsName;
        }
    }

    /// <summary>
    /// Define list of Device Status
    /// </summary>
    public sealed class DeviceInstallStatus
    {
        private DeviceInstallStatus(string value, string iconFileName, Color color, BgColor bgColor)
        {
            Value = value;
            IconFileName = iconFileName;
            Color = color;
            BgColor = bgColor;
        }

        public string Value { get; set; }
        public string IconFileName { get; set; }
        public Color Color { get; set; }
        public BgColor BgColor { get; set; }
        public static DeviceInstallStatus None { get { return new DeviceInstallStatus("-", "bg_light_gray.png", Color.FromArgb(170, 170, 170), BgColor.LightGray); } }
        public static DeviceInstallStatus ToBeVerified { get { return new DeviceInstallStatus("To be verified", "bg_light_blue.png", Color.FromArgb(139, 179, 253), BgColor.LightBlue); } }
        public static DeviceInstallStatus Verified { get { return new DeviceInstallStatus("Verified", "bg_blue.png", Color.FromArgb(42, 116, 253), BgColor.Blue); } }
        public static DeviceInstallStatus New { get { return new DeviceInstallStatus("New", "bg_gray.png", Color.FromArgb(85, 85, 85), BgColor.Gray); } }
        public static DeviceInstallStatus DoesNotExist { get { return new DeviceInstallStatus("Does not exist", "bg_yellow.png", Color.FromArgb(255, 195, 12), BgColor.Yellow); } }
        public static DeviceInstallStatus ToBeInstalled { get { return new DeviceInstallStatus("To be installed", "bg_orange.png", Color.FromArgb(254, 111, 41), BgColor.Orange); } }
        public static DeviceInstallStatus Installed { get { return new DeviceInstallStatus("Installed", "bg_green.png", Color.FromArgb(87, 214, 2), BgColor.Green); } }
        public static DeviceInstallStatus Removed { get { return new DeviceInstallStatus("Removed", "bg_red.png", Color.FromArgb(253, 19, 1), BgColor.Red); } }
        
        public static List<string> GetList()
        {
            return new List<string> { "-", "To be verified", "Verified", "New", "Does not exist", "To be installed", "Installed", "Removed" };
        }

        public byte[] GetColorBytes()
        {
            var iconBytes = File.ReadAllBytes(Path.Combine(@"Resources\img\map_device", IconFileName));
            return iconBytes;
        }
    }

    /// <summary>
    /// Defines for device type
    /// </summary>
    public sealed class DeviceType
    {
        private DeviceType(string value, string category, bool hasControllerID, bool hasIdentifier, bool hasTypeOfEquipment, bool hasGatewayHostName = false)
        {
            Value = value;
            Category = category;
            HasControllerID = hasControllerID;
            HasIdentifier = hasIdentifier;
            HasTypeOfEquipment = hasTypeOfEquipment;
            HasGatewayHostName = hasGatewayHostName;
        }

        public string Value { get; set; }
        public string Category { get; set; }
        public bool HasControllerID { get; set; }
        public bool HasIdentifier { get; set; }
        public bool HasTypeOfEquipment { get; set; }
        public bool HasGatewayHostName { get; set; }
        public static DeviceType AudioPlayer { get { return new DeviceType("AUDIO PLAYER", "", false, true, false, true); } }
        public static DeviceType Building { get { return new DeviceType("BUILDING", "building", true, true, false); } }
        public static DeviceType CameraIp { get { return new DeviceType("CAMERA IP", "cameraip", false, true, false); } }
        public static DeviceType CityObject { get { return new DeviceType("CITY OBJECT", "cityObject", true, true, false); } }
        public static DeviceType Controller { get { return new DeviceType("CONTROLLER DEVICE", "controllerdevice", true, false, false, true); } }
        public static DeviceType ElectricalCounter { get { return new DeviceType("ELECTRICAL COUNTER", "electricalCounter", true, true, true); } }
        public static DeviceType EnvironmentalSensor { get { return new DeviceType("ENVIRONMENTAL SENSOR", "", true, true, true); } }
        public static DeviceType Inputs { get { return new DeviceType("INPUTS", "input", true, true, false); } }
        public static DeviceType Nature { get { return new DeviceType("NATURE", "nature", true, true, false); } }
        public static DeviceType NetworkComponent { get { return new DeviceType("NETWORK COMPONENT", "networkComponent", true, true, false); } }
        public static DeviceType OccupancySensor { get { return new DeviceType("OCCUPANCY SENSOR", "occupancySensor", true, true, true); } }
        public static DeviceType Ouputs { get { return new DeviceType("OUTPUTS", "output", true, true, false); } }
        public static DeviceType ParkingPlace { get { return new DeviceType("PARKING PLACE", "parkingPlace", false, true, false); } }
        public static DeviceType Streetlight { get { return new DeviceType("STREETLIGHT", "streetlight", true, true, true); } }
        public static DeviceType Switch { get { return new DeviceType("SWITCH DEVICE", "switch", true, true, true); } }
        public static DeviceType Tank { get { return new DeviceType("TANK", "tank", false, true, false); } }
        public static DeviceType TransportSignage { get { return new DeviceType("TRANSPORT SIGNAGE", "transportSignage", true, true, true); } }
        public static DeviceType Vehicle { get { return new DeviceType("VEHICLE", "vehicle", true, true, false); } }
        public static DeviceType VehicleChargingStation { get { return new DeviceType("VEHICLE CHARGING STATION", "vehicleChargingStation", true, true, false); } }
        public static DeviceType WasteContainer { get { return new DeviceType("WASTE CONTAINER", "wasteContainer", false, true, false); } }
        public static DeviceType WeatherStation { get { return new DeviceType("WEATHER STATION", "weatherStation", false, true, false); } }
        public static DeviceType CabinetController { get { return new DeviceType("CABINET CONTROLLER", "cabinetController", true, true, true); } }

        public override bool Equals(object obj)
        {
            return Value.Equals(((DeviceType)obj).Value);
        }

        public override string ToString()
        {
            return Value;
        }

        public static bool operator ==(DeviceType a, DeviceType b)
        {
            return a.Value == b.Value;
        }

        public static bool operator !=(DeviceType a, DeviceType b)
        {
            return a.Value != b.Value;
        }

        public static List<string> GetList()
        {
            return new List<string> { "AUDIO PLAYER", "BUILDING", "CAMERA IP", "CITY OBJECT", "CONTROLLER DEVICE", "ELECTRICAL COUNTER", "ENVIRONMENTAL SENSOR", "INPUTS", "NATURE", "NETWORK COMPONENT", "OCCUPANCY SENSOR", "OUTPUTS", "PARKING PLACE", "STREETLIGHT", "SWITCH DEVICE", "TANK", "TRANSPORT SIGNAGE", "VEHICLE", "VEHICLE CHARGING STATION", "WASTE CONTAINER", "WEATHER STATION", "CABINET CONTROLLER" };
        }

        public byte[] GetIconBytes()
        {
            var filePath = string.Format(@"Resources\img\devices\{0}.png", Value.ToLower());
            var iconBytes = File.ReadAllBytes(Settings.GetFullPath(filePath));
            return iconBytes;
        }

        public byte[] GetIconBytes(string color)
        {
            var filePath = string.Format(@"Resources\img\devices\{0} {1}.png", color, Value.ToLower());
            var iconBytes = File.ReadAllBytes(Settings.GetFullPath(filePath));
            return iconBytes;
        }

        public string GetIconPath()
        {
            return string.Format(@"Resources\img\devices\{0}.png", Value.ToLower());
        }

        public string GetIconFullPath()
        {
            return Settings.GetFullPath(GetIconPath());
        }       
    }

    /// <summary>
    /// Defines for Alarm type
    /// </summary>
    public sealed class AlarmType
    {
        private AlarmType(string name, string definitionId)
        {
            Name = name;
            DefinitionId = definitionId;
        }

        public string Name { get; set; }
        public string DefinitionId { get; set; }
        public string TriggerConditionImplClassName
        {
            get { return string.Format("com.slv.alarming.trigger.{0}", DefinitionId); }
        }

        public static AlarmType DeviceAlarmMultipleFailuresOnMultipleDevices { get { return new AlarmType("Device alarm: multiple failures on multiple devices", "MultiAlarmMultiDeviceFailureCondition"); } }
        public static AlarmType DeviceAlarmTooManyFailuresInAnArea { get { return new AlarmType("Device alarm: too many failures in an area", "DevicesInAreaTriggersGenerator"); } }
        public static AlarmType DeviceAlarmFailureRatioInAGroup { get { return new AlarmType("Device alarm: failure ratio in a group", "GroupFailureRatioCondition"); } }
        public static AlarmType DeviceAlarmDataAnalysisVsPreviousDay { get { return new AlarmType("Device alarm: data analysis vs previous day", "SmartMeteringDeviceAnalyticsAlarmTriggersGenerator"); } }
        public static AlarmType DeviceAlarmNoDataReceived { get { return new AlarmType("Device alarm: no data received", "DevicesUpdateTimeRatioTriggersGenerator"); } }
        public static AlarmType ControllerAlarmNoDataReceived { get { return new AlarmType("Controller alarm: no data received", "ControllerUpdateTimeAlarm"); } }
        public static AlarmType ControllerAlarmOnOffTimesVsPreviousDay { get { return new AlarmType("Controller alarm: ON/OFF times vs previous day", "SCSwitchingOnAndOffAlarm"); } }
        public static AlarmType ControllerAlarmLastKnownStateOfAnIO { get { return new AlarmType("Controller alarm: last known state of an I/O", "ControllerInputStateCondition"); } }
        public static AlarmType ControllerAlarmComparisonBetweenTwoIOs { get { return new AlarmType("Controller alarm: comparison between two I/Os", "SCIOStateComparisonAlarmTriggersGenerator"); } }
        public static AlarmType MeterAlarmComparisonToATrigger{ get { return new AlarmType("Meter alarm: comparison to a trigger", "SmartMeteringValueAlarmTriggersGenerator"); } }
        public static AlarmType MeterAlarmDataAnalysisVsPreviousDay { get { return new AlarmType("Meter alarm: data analysis vs previous day", "SmartMeteringAnalyticsAlarmTriggersGenerator"); } }
        public static AlarmType MeterAlarmDataAnalysisVsPreviousDayAtFixedTime { get { return new AlarmType("Meter alarm: data analysis vs previous day (at fixed time)", "SmartMeteringDayTimeAnalyticsAlarmTriggersGenerator"); } }
    }

    /// <summary>
    /// Defines for Alarm Action
    /// </summary>
    public class AlarmAction
    {
        public static readonly string NotifyByEmail = "com.slv.alarming.action.SendEMailAction";
        public static readonly string SendHttpRequest = "com.slv.alarming.action.HttpRequestAction";
        public static readonly string SendReport = "com.slv.alarming.action.TriggerScheduledReportAction";

        public static List<string> GetList()
        {
            var type = typeof(AlarmAction);
            var actionsName = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy).Where(fi => fi.IsInitOnly).Select(p => p.GetValue(null).ToString()).ToList();

            return actionsName;
        }
    }

    /// <summary>
    /// Define action enum of zoom level (from 1 - 19)
    /// </summary>
    public enum ZoomLevel
    {
        Level_MIN = 1,
        Level_02 = 2,
        Level_03 = 4,
        Level_04 = 8,
        Level_05 = 16,
        Level_06 = 32,
        Level_07 = 64,
        Level_08 = 128,
        Level_09 = 256,
        Level_10 = 512,
        Level_11 = 1024,
        Level_12 = 2048,
        Level_13 = 4096,
        Level_14 = 8192,
        Level_15 = 16384,
        Level_16 = 32768,
        Level_17 = 65536,
        Level_18 = 131072,
        Level_MAX = 262144,
    }

    /// <summary>
    /// Define action enum of zoom GL level (base meter of map, ex: 50m, 300km)
    /// </summary>
    public enum ZoomGLLevel
    {
        m10 = 10,
        m20 = 20,
        m30 = 30,
        m50 = 50,
        m100 = 100,
        m300 = 300,
        m500 = 500,
        km1 = 1000,
        km2 = 2000,
        km3 = 3000,
        km5 = 5000,
        km10 = 10000,
        km30 = 30000,
        km50 = 50000,
        km100 = 100000,
        km200 = 200000,
        km500 = 500000,
        km1000 = 1000000,
        km2000 = 2000000,
        km3000 = 3000000,
    }

    /// <summary>
    /// Define SLV Node Type enum
    /// </summary>
    public enum NodeType
    {
        All,
        GeoZone,
        AudioPlayer,
        Streetlight,
        Controller,
        Switch,
        CameraIp,
        Building,
        CityObject,
        ElectricalCounter,
        Input,
        Output,
        Nature,
        NetworkComponent,
        OccupancySensor,
        ParkingPlace,
        Tank,
        TransportSignage,
        Vehicle,
        VehicleChargingStation,
        WasteContainer,
        WeatherStation,
        EnvSensor,
        CabinetController,
        Unknown
    }

    /// <summary>
    /// Realtime Command enum
    /// </summary>
    public enum RealtimeCommand
    {
        DimOn,
        Dim90,
        Dim80,
        Dim70,
        Dim60,
        Dim50,
        Dim40,
        Dim30,
        Dim20,
        Dim10,
        DimOff
    }

    public enum BgColor
    {
        LightGray,
        Gray,
        LightBlue,
        Blue,
        Yellow,
        Green,
        Orange,
        Red,
    }

    public enum IE_PopupButton
    {
        Save,
        AllowOnce
    }

    public enum MapSource
    {
        OpenStreetMap,
        GoogleMaps,
        BingMaps
    }

    public enum FailureIcon
    {
        None,
        OK,
        Warning,
        Error
    }

    public enum Position
    {
        TopLeft,
        Top,
        TopRight,
        Left,
        Center,
        Right,
        BottomLeft,
        Bottom,
        BottomRight
    }
}
