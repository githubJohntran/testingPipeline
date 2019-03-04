using System;
using System.Collections.Generic;
using System.Drawing;

namespace StreetlightVision.Models
{
    public class GFMInformationModel
    {
        public string GeoZoneId { get; set; }
        public string GeoZoneName { get; set; }
        public int DevicesCount { get; set; }
        public int NonCriticalCount { get; set; }
        public int CriticalCount { get; set; }
        public decimal NonCriticalRatio { get; set; }
        public decimal CriticalRatio { get; set; }

        public GFMInformationModel()
        {
        }
    }    

    public class GFMOptionModel
    {
        public string Name { get; set; }
        public int Count { get; set; }
        public bool IsDisplayed { get; set; }
        public decimal Percentage { get; set; }       
        public int X { get; set; }
        public int Y { get; set; }
        public string ColorHex { get; set; }

        public string Percent
        {
            get
            {
                return string.Format("{0}%", Math.Round(Percentage, 2) * 100);
            }
        }

        public Color Color
        {
            get
            {
                return ColorTranslator.FromHtml(ColorHex);
            }
        }

        public GFMOptionModel()
        {
        }
    }
}
