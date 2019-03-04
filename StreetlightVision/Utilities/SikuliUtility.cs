using Sikuli4Net.sikuli_REST;
using Sikuli4Net.sikuli_UTIL;
using StreetlightVision.Extensions;
using StreetlightVision.Models;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace StreetlightVision.Utilities
{
    public static class SikuliUtility
    {
        /// <summary>
        /// Drag And Drop an app/widget to another one
        /// </summary>
        /// <param name="source"></param>
        /// <param name="dest"></param>
        /// <param name="isApp">if app is true, widget is false</param>
        public static void DragAndDrop(string source, string dest, bool isApp = true)
        {
            foreach (var process in Process.GetProcessesByName("javaw"))
            {
                process.Kill();
                Wait.ForSeconds(1);
            }

            APILauncher sikuli = new APILauncher(true);
            sikuli.Start();
            //Wait for Sikuli server started
            WebDriverContext.Wait.Until(driver => Process.GetProcessesByName("javaw").Length > 0);

            string path = @"Resources\img\apps\{0}\dragdrop.png";
            if (!isApp) path = @"Resources\img\widgets\{0}\dragdrop.png";

            var sourcePath = Settings.GetFullPath(string.Format(path, source));
            var destPath = Settings.GetFullPath(string.Format(path, dest));

            var sourcePattern = new Pattern(sourcePath);
            var destPattern = new Pattern(destPath);
            var screen = new Screen();
            screen.DragDrop(sourcePattern, destPattern);

            sikuli.Stop();
        }
    }
}
