using System.Collections.Generic;

namespace StreetlightVision.Models
{
    public class GeozoneModel
    {
        public string Name { get; set; }
        public string Path { get; set; }

        public List<DeviceModel> Devices { get; set; }

        public GeozoneModel()
        {
            Devices = new List<DeviceModel>();
        }
    }
}
