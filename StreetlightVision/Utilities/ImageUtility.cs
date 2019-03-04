using ImageMagick;
using StreetlightVision.Models;
using System.IO;

namespace StreetlightVision.Utilities
{
    public static class ImageUtility
    {
        /// <summary>
        /// Compare 2 image bytes
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns>MeanErrorPerPixel</returns>
        public static double Compare(byte[] source, byte[] target)
        {
            var imageSource = new MagickImage(source);
            var imageTarget = new MagickImage(target);

            var result = imageSource.Compare(imageTarget);

            return result.MeanErrorPerPixel;
        }

        /// <summary>
        /// Compare 2 image with path file
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>

        public static double Compare(string source, string target)
        {
            var imageSource = new MagickImage(source);
            var imageTarget = new MagickImage(target);

            var result = imageSource.Compare(imageTarget);

            return result.MeanErrorPerPixel;
        }

        /// <summary>
        /// Compare 2 image with source byte[], target path file
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static double Compare(byte[] source, string target)
        {
            var imageSource = new MagickImage(source);
            var imageTarget = new MagickImage(target);

            var result = imageSource.Compare(imageTarget);

            return result.MeanErrorPerPixel;
        }

        /// <summary>
        /// Compare 2 image with source  path file, target byte[]
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static double Compare(string source, byte[] target)
        {
            var imageSource = new MagickImage(source);
            var imageTarget = new MagickImage(target);

            var result = imageSource.Compare(imageTarget);

            return result.MeanErrorPerPixel;
        }
        
        /// <summary>
        /// Get background file path of streetlight
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static string GetBackgroundStreetlight(BgColor color = BgColor.LightGray)
        {
            string iconFileName;

            switch (color)
            {
                case BgColor.LightGray:
                    iconFileName = "bg_light_gray.png";
                    break;

                case BgColor.Gray:
                    iconFileName = "bg_gray.png";
                    break;

                case BgColor.LightBlue:
                    iconFileName = "bg_light_blue.png";
                    break;

                case BgColor.Blue:
                    iconFileName = "bg_blue.png";
                    break;

                case BgColor.Yellow:
                    iconFileName = "bg_yellow.png";
                    break;

                case BgColor.Green:
                    iconFileName = "bg_green.png";
                    break;

                case BgColor.Orange:
                    iconFileName = "bg_orange.png";
                    break;

                case BgColor.Red:
                    iconFileName = "bg_red.png";
                    break;

                default:
                    iconFileName = "bg_light_gray.png";
                    break;
            }

            return Settings.GetFullPath(Path.Combine(@"Resources\img\map_device", iconFileName));
        }

        /// <summary>
        /// Get background image bytes of streetlight
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static byte[] GetStreetlightBgIconBytes(BgColor color = BgColor.LightGray)
        {
            return File.ReadAllBytes(GetBackgroundStreetlight(color));
        }
    }
}
