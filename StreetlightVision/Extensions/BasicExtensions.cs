using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;

namespace StreetlightVision.Extensions
{
    public static class BasicExtensions
    {
        /// <summary>
        /// Get an string list after split(RemoveEmptyEntries) with separator ('\r', '\n')
        /// </summary>
        /// <param name="input"></param>
        /// <returns>list of string</returns>
        public static List<string> SplitEx(this string input)
        {
            return input.SplitEx(new char[] { '\r', '\n' });
        }

        /// <summary>
        /// Get an string list after split(RemoveEmptyEntries) with separator(char[])
        /// </summary>
        /// <param name="input"></param>
        /// <param name="separator"></param>
        /// <returns>list of string</returns>
        public static List<string> SplitEx(this string input, char[] separator)
        {
            var list = input.Split(separator, StringSplitOptions.RemoveEmptyEntries).ToList();
            return list;
        }

        /// <summary>
        /// Get an string list after split(RemoveEmptyEntries) with a separator
        /// </summary>
        /// <param name="input"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static List<string> SplitEx(this string input, char separator)
        {
            var list = input.Split(new char[] { separator }, StringSplitOptions.RemoveEmptyEntries).ToList();
            return list;
        }

        /// <summary>
        /// Get an string list after split(RemoveEmptyEntries) with separator(string[])
        /// </summary>
        /// <param name="input"></param>
        /// <param name="separator"></param>
        /// <returns>list of string</returns>
        public static List<string> SplitEx(this string input, string[] separator)
        {
            var list = input.Split(separator, StringSplitOptions.RemoveEmptyEntries).ToList();
            return list;
        }

        /// <summary>
        /// Get an string list after split(RemoveEmptyEntries) with a separator
        /// </summary>
        /// <param name="input"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static List<string> SplitEx(this string input, string separator)
        {
            var list = input.Split(new string[] { separator }, StringSplitOptions.RemoveEmptyEntries).ToList();
            return list;
        }

        /// <summary>
        /// Get an string after split(RemoveEmptyEntries) with specific index and separator ('\r', '\n')
        /// </summary>
        /// <param name="input"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static string SplitAndGetAt(this string input, int index)
        {
            return input.SplitAndGetAt(new char[] { '\r', '\n' }, index);
        }

        /// <summary>
        /// Get an string after split(RemoveEmptyEntries) with specific index and separators
        /// </summary>
        /// <param name="input"></param>
        /// <param name="seseparator"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static string SplitAndGetAt(this string input, char[] separator, int index)
        {
            var list = input.Split(separator, StringSplitOptions.RemoveEmptyEntries);

            if (index < list.Length)
                return list.ElementAt(index).Trim();

            return string.Empty;
        }

        /// <summary>
        /// Get an string after split(RemoveEmptyEntries) with specific index and a separator
        /// </summary>
        /// <param name="input"></param>
        /// <param name="separator"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static string SplitAndGetAt(this string input, char separator, int index)
        {
            var list = input.Split(new char[] { separator }, StringSplitOptions.RemoveEmptyEntries);

            if (index < list.Length)
                return list.ElementAt(index).Trim();

            return string.Empty;
        }

        /// <summary>
        /// Get an string after split(RemoveEmptyEntries) with specific index and separators
        /// </summary>
        /// <param name="input"></param>
        /// <param name="seseparator"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static string SplitAndGetAt(this string input, string[] separator, int index)
        {
            var list = input.Split(separator, StringSplitOptions.RemoveEmptyEntries);

            if (index < list.Length)
                return list.ElementAt(index).Trim();

            return string.Empty;
        }

        /// <summary>
        ///  Get an string after split(RemoveEmptyEntries) with specific index and a separator
        /// </summary>
        /// <param name="input"></param>
        /// <param name="separator"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static string SplitAndGetAt(this string input, string separator, int index)
        {
            var list = input.Split(new string[] { separator }, StringSplitOptions.RemoveEmptyEntries);

            if (index < list.Length)
                return list.ElementAt(index).Trim();

            return string.Empty;
        }

        /// <summary>
        /// Get last deepest child of a specific path (e.g "Geozones\Nguyen Van Troi Geozone\1st Sub-GeoZone\New-UXBNTVR" is "New-UXBNTVR")
        /// </summary>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        public static string GetChildName(this string fullPath)
        {
            var nodes = fullPath.SplitEx(new string[] { @"\" });
            return nodes[nodes.Count - 1];
        }

        /// <summary>
        /// Get parent name of last child of a specific path (e.g "Geozones\Nguyen Van Troi Geozone\1st Sub-GeoZone\New-UXBNTVR" is "1st Sub-GeoZone")
        /// </summary>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        public static string GetParentName(this string fullPath)
        {
            var nodes = fullPath.SplitEx(new string[] { @"\" });
            var parentNode = nodes.Count >= 2 ? nodes[nodes.Count - 2] : Settings.RootGeozoneName;

            return parentNode;
        }

        /// <summary>
        /// Get Full of parent name (e.g Automation [GeoZones])
        /// </summary>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        public static string GetParentFullName(this string fullPath)
        {
            var nodes = fullPath.SplitEx(new string[] { @"\" });
            string parentNode;
            if (nodes.Count >= 3)
                parentNode = string.Format("{0} [{1}]", nodes[nodes.Count - 2], nodes[nodes.Count - 3]);
            else if (nodes.Count == 2)
                parentNode = string.Format("{0} [{1}]", nodes[0], Settings.RootGeozoneName);
            else
                parentNode = Settings.RootGeozoneName;

            return parentNode;
        }

        /// <summary>
        /// Get parent path of last child of a specific path (e.g "Geozones\Nguyen Van Troi Geozone\1st Sub-GeoZone\New-UXBNTVR" is "Geozones\Nguyen Van Troi Geozone\1st Sub-GeoZone")
        /// </summary>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        public static string GetParentPath(this string fullPath)
        {
            var nodes = fullPath.SplitEx(new string[] { @"\" });
            nodes.RemoveAt(nodes.Count - 1);
            var parentPath = nodes.Count >= 1 ? string.Join(@"\", nodes) : Settings.RootGeozoneName;

            return parentPath;
        }

        /// <summary>
        /// Get root name of a specific path  
        /// (e.g.1 "Geozones\Nguyen Van Troi Geozone\1st Sub-GeoZone\New-UXBNTVR" is "Geozones"
        /// ; e.g.2 "Nguyen Van Troi Geozone\1st Sub-GeoZone\New-UXBNTVR" is "Nguyen Van Troi Geozone")
        /// </summary>
        /// <remarks>
        ///
        /// </remarks>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        public static string GetRootName(this string fullPath)
        {
            var nodes = fullPath.SplitEx(new string[] { @"\" });
            var rootNode = nodes.Count >= 2 ? nodes[0] : Settings.RootGeozoneName;

            return rootNode;

        }

        /// <summary>
        /// Get timestamp of a date time (HHmmssFF)
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string Timestamp(this DateTime dt)
        {
            return dt.ToString("HHmmssFF");
        }

        /// <summary>
        /// Check if parent list includes child list
        /// </summary>
        /// <param name="parentList"></param>
        /// <param name="childList"></param>
        /// <returns></returns>
        public static bool CheckIfIncluded(this IEnumerable<string> parentList, IEnumerable<string> childList)
        {
            foreach (var child in childList)
            {
                if (!parentList.Contains(child))
                {
                    return false;
                }
            }
            return true;
        }

        public static bool IsIncreasing<T>(this IEnumerable<T> list) where T : IComparable
        {
            return list.Zip(list.Skip(1), (a, b) => a.CompareTo(b) <= 0).All(b => b);
        }

        public static bool IsDecreasing<T>(this IEnumerable<T> list) where T : IComparable
        {
            return list.Zip(list.Skip(1), (a, b) => a.CompareTo(b) >= 0).All(b => b);
        }

        public static bool IsIncreasing(this List<string> list)
        {
            var length = list.Count();
            var prev = list[0];
            for (var i = 1; i < length; i++)
            {
                var next = list[i];
                if (CompareString(prev, next) > 0) return false;
                prev = list[i];
            }

            return true;
        }

        public static bool IsDecreasing(this List<string> list)
        {
            var length = list.Count();
            var prev = list[0];
            for (var i = 1; i < length; i++)
            {
                var next = list[i];
                if (CompareString(prev, next) < 0) return false;
                prev = list[i];
            }

            return true;
        }

        private static int CompareString(string a, string b)
        {
            a = a.ToUpper();
            b = b.ToUpper();
            var length = 0;
            var lengthA = a.Length;
            var lengthB = b.Length;
            if (lengthA == 0 && lengthB == 0) return 0;
            if (lengthA == 0) return -1;
            if (lengthB == 0) return 1;

            if (lengthA < lengthB)
                length = lengthA;
            else
                length = lengthB;

            for (var i = 0; i < length; i++)
            {
                if (a[i].CompareTo(b[i]) == 0) continue;
                if (a[i].CompareTo(b[i]) < 0)
                    return -1;
                return 1;
            }
            return 0;
        }                

        /// <summary>
        /// Convert a string list of date to datetime list
        /// </summary>
        /// <param name="listDateStr"></param>
        /// <returns></returns>
        public static List<DateTime> ToDateList(this IEnumerable<string> listDateStr, string dateFormat)
        {
            List<DateTime> listDate;
            try
            {
                listDate = listDateStr.Select(d => DateTime.ParseExact(d, dateFormat, CultureInfo.InvariantCulture)).ToList();
            }
            catch
            {
                listDate = listDateStr.Select(d => DateTime.Parse(d, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal)).ToList();
            }

            return listDate;
        }

        /// <summary>
        /// Trim Start and Trim End with '\r', '\n', ' '
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string TrimEx(this string input)
        {
            return input.TrimStart('\r', '\n', ' ').TrimEnd('\r', '\n', ' ').Trim();
        }

        public static string GetKnownName(this Color color)
        {
            foreach (KnownColor kc in Enum.GetValues(typeof(KnownColor)))
            {
                Color known = Color.FromKnownColor(kc);
                if (color.ToArgb() == known.ToArgb())
                {
                    return known.Name;
                }
            }

            return string.Format("Unknown(#{0})", color.Name);
        }

        public static string ToHex(this Color color)
        {
            return "#" + color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");
        }

        public static bool DictionaryEqual<TKey, TValue>(this IDictionary<TKey, TValue> first, IDictionary<TKey, TValue> second)
        {
            return first.DictionaryEqual(second, null);
        }

        public static bool DictionaryEqual<TKey, TValue>(this IDictionary<TKey, TValue> first, IDictionary<TKey, TValue> second, IEqualityComparer<TValue> valueComparer)
        {
            if (first == second) return true;
            if ((first == null) || (second == null)) return false;
            if (first.Count != second.Count) return false;

            valueComparer = valueComparer ?? EqualityComparer<TValue>.Default;

            foreach (var kvp in first)
            {
                TValue secondValue;
                if (!second.TryGetValue(kvp.Key, out secondValue)) return false;
                if (!valueComparer.Equals(kvp.Value, secondValue)) return false;
            }
            return true;
        }

        public static string ToUpperFirstChar(this string input)
        {
            switch (input)
            {
                case null: throw new ArgumentNullException(nameof(input));
                case "": throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input));
                default: return input.First().ToString().ToUpper() + input.Substring(1);
            }
        }

        public static void AddRange<T, S>(this Dictionary<T, S> source, Dictionary<T, S> collection)
        {
            if (collection == null) return;

            foreach (var item in collection)
            {
                if (!source.ContainsKey(item.Key))
                {
                    source.Add(item.Key, item.Value);
                }
                else
                {
                    Assert.Warn("Duplicate key issue here !");
                }
            }
        }

        public static void Add<T, S>(this Dictionary<T, S> source, KeyValuePair<T, S> item)
        {
            if (!source.ContainsKey(item.Key))
            {
                source.Add(item.Key, item.Value);
            }
            else
            {
                Assert.Warn("Duplicate key issue here !");
            }
        }

        public static Dictionary<string, object> ToDicObj(this Dictionary<string, string> source)
        {
            return source.ToDictionary(pair => pair.Key, pair => (object)pair.Value);
        }

        public static Dictionary<string, string> ToDicStr(this Dictionary<string, object> source)
        {
            return source.ToDictionary(pair => pair.Key, pair => pair.Value.ToString());
        }
    }
}
