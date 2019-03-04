namespace StreetlightVision.Models
{
    /// <summary>
    /// data for specific command
    /// </summary>
    public class CommandModel
    {
        public int Value { get; set; }
        public string ValueText { get { return string.Format("{0}%", Value); } }
        public string Text
        {
            get
            {
                string text = string.Format("{0}%", Value);
                switch (Value)
                {
                    case 0:
                        text = "OFF";
                        break;
                    case 100:
                        text = "ON";
                        break;
                    default:
                        text = string.Format("{0}%", Value);
                        break;
                }
                return text;
            }
        }
        public int TriangleValue { get; set; }

        public int OffsetX { get; set; }
        public int OffsetY { get; set; }
    }
}
