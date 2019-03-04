using StreetlightVision.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace StreetlightVision.Utilities
{
    public class CSVUtility
    {
        /// <summary>
        /// Build DataTable from CSV file downloaded from SLV apps
        /// </summary>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        public static DataTable BuildDataTableFromCSV(string fullPath, params string[] ignoredColumns)
        {
            var separator = ';';
            var fileLines = File.ReadAllLines(fullPath);
            var headerLine = fileLines[0];
            if (!headerLine.Contains(separator.ToString())) separator = ',';
            var columnHeaders = headerLine.Contains(separator.ToString()) ? headerLine.Split(separator) : new string[] { };

            var tblResult = new DataTable();
            foreach (var header in columnHeaders)
            {
                tblResult.Columns.Add(header);
            }

            for (var i = 1; i < fileLines.Length; i++)
            {
                var cells = fileLines[i].Contains(separator.ToString()) ? fileLines[i].Split(separator) : new string[] { };
                tblResult.Rows.Add(cells);
            }

            foreach (var ignoredColumn in ignoredColumns)
            {
                tblResult.Columns.Remove(ignoredColumn);
            }

            return tblResult;
        }

        /// <summary>
        /// Get header line from CSV file downloaded from SLV apps
        /// </summary>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        public static string GetHeaderLineFromCSV(string fullPath)
        {
            var fileLines = File.ReadAllLines(fullPath);

            if(fileLines.Length > 0)
                return fileLines[0];

            throw new Exception("CSV file is empty");
        }
    }
}
