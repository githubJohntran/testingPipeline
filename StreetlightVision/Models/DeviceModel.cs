namespace StreetlightVision.Models
{
    public class DeviceModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string Controller { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string TypeOfEquipment { get; set; }
        public string UniqueAddress { get; set; }
        public string DimmingGroup { get; set; }
        public string ControlTechnology { get; set; }        
        public string Cluster { get; set; }
        public string TimeZone { get; set; }
        public DeviceType Type { get; set; }
        public DeviceStatus Status { get; set; }

        public DeviceModel()
        { 
        }
        public DeviceModel(DeviceType type)
        {
            Type = type;
        }
    }

    public enum DeviceStatus
    {
        Working,
        NonWorking
    }
}
