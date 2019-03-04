using System;
using System.Collections.Generic;
using System.Linq;

namespace StreetlightVision.Models
{
    /// <summary>
    /// Define test data model
    /// </summary>
    public class TestDataModel
    {
        public List<GeozoneModel> Geozones { get; set; }

        public TestDataModel()
        {
            Geozones = new List<GeozoneModel>();
        }

        public GeozoneModel this[int i]
        {
            get
            {
                if(Geozones.Any())
                    return Geozones[i];

                throw new Exception("There is no geozone found");
            }
        }
    }
}
