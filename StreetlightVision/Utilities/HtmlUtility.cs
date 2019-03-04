using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;

namespace StreetlightVision.Utilities
{
    public class HtmlUtility
    {
        private HtmlDocument _htmlDocument;
        public HtmlUtility()
        {
            _htmlDocument = new HtmlDocument();
        }

        public HtmlUtility(string htmlSource, bool isFilePath = false) : this()
        {
            Load(htmlSource, isFilePath);
        }

        /// <summary>
        /// Load a html string or file
        /// </summary>
        /// <param name="htmlSource"></param>
        /// <param name="isFilePath"></param>
        public void Load(string htmlSource, bool isFilePath = false)
        {
            if (isFilePath)
            {
                _htmlDocument.Load(htmlSource);
            }
            else
            {
                //Fix html missing </tr>
                var fixedHtml = Regex.Replace(htmlSource, "<tr[^>]*>(?:(?!</?tr>|</tbody>|</table>).)*?(?=<tr[^>]*>|</tbody>|</table>)", "$&</tr>", RegexOptions.Singleline | RegexOptions.IgnoreCase);
                _htmlDocument.LoadHtml(fixedHtml);
            }
        }

        /// <summary>
        /// Get a list nodes that matches specific xpath
        /// </summary>
        /// <param name="xPath"></param>
        /// <returns></returns>
        public List<HtmlNode> GetNodes(string xPath)
        {
            return _htmlDocument.DocumentNode.SelectNodes(xPath).ToList();
        }

        /// <summary>
        /// Get a list childe nodes of a node that matches specific xpath
        /// </summary>
        /// <param name="xPath"></param>
        /// <returns></returns>
        public List<HtmlNode> GetChildNodes(string xPath)
        {
            var node = _htmlDocument.DocumentNode.SelectSingleNode(xPath);
            if (node != null)
            {
                var list = _htmlDocument.DocumentNode.SelectSingleNode(xPath).ChildNodes.ToList();
                return list.Where(p => !string.IsNullOrEmpty(p.InnerText.Replace(Environment.NewLine, string.Empty).Trim())).ToList();
            }

            return null;
        }

        /// <summary>
        /// Get a first single node that matches specific xpath
        /// </summary>
        /// <param name="xPath"></param>
        /// <returns></returns>
        public HtmlNode GetSingleNode(string xPath)
        {
            return _htmlDocument.DocumentNode.SelectSingleNode(xPath);
        }

        /// <summary>
        /// Get a node that matches specific xpath and inner text
        /// </summary>
        /// <param name="xPath"></param>
        /// <param name="innerText"></param>
        /// <returns></returns>
        public HtmlNode GetNode(string xPath, string innerText)
        {
            var list = _htmlDocument.DocumentNode.SelectNodes(xPath).ToList();

            return list.FirstOrDefault(p => innerText.Equals(p.InnerText.Replace(Environment.NewLine, string.Empty).Trim()));
        }

        /// <summary>
        /// Get a list text of nodes that matches specific xpath
        /// </summary>
        /// <param name="xPath"></param>
        /// <returns></returns>
        public List<string> GetNodesText(string xPath)
        {
            List<string> result = null;
            var list = _htmlDocument.DocumentNode.SelectNodes(xPath).ToList();

            if(list.Any())
            {
                result = list.Select(p => p.InnerText.Replace(Environment.NewLine, string.Empty).Trim()).ToList();
            }

            return result;
        }

        /// <summary>
        /// Get list text of child nodes of a node that matches specific xpath
        /// </summary>
        /// <param name="xPath"></param>
        /// <returns></returns>
        public List<string> GetChildNodesText(string xPath)
        {
            List<string> result = null;
            var node = _htmlDocument.DocumentNode.SelectSingleNode(xPath);
            if (node != null)
            {
                var list = _htmlDocument.DocumentNode.SelectSingleNode(xPath).ChildNodes.ToList();
                if (list.Any())
                {
                    result = list.Select(p => p.InnerText.Replace(Environment.NewLine, string.Empty).Trim()).ToList();
                    result = result.Where(p => !string.IsNullOrEmpty(p)).ToList();
                }
            }

            return result;
        }

        /// <summary>
        /// Get a first single node that matches specific xpath
        /// </summary>
        /// <param name="xPath"></param>
        /// <returns></returns>
        public string GetSingleNodeText(string xPath)
        {
            var node = _htmlDocument.DocumentNode.SelectSingleNode(xPath);

            if (node != null)
                return node.InnerText.Replace(Environment.NewLine, string.Empty).Trim();

            return null;
        }

        /// <summary>
        ///  Get an attribute value of nodes that matches specific xpath and name
        /// </summary>
        /// <param name="xPath"></param>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        public List<string> GetNodesAttrVal(string xPath, string attributeName)
        {
            List<string> result = null;
            var list = _htmlDocument.DocumentNode.SelectNodes(xPath).ToList();

            if (list.Any())
            {
                result = list.Select(p => p.GetAttributeValue(attributeName, string.Empty)).ToList();
            }

            return result;
        }

        /// <summary>
        ///  Get an attribute value of first single node that matches specific xpath and name
        /// </summary>
        /// <param name="xPath"></param>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        public string GetSingleNodeAttrVal(string xPath, string attributeName)
        {
            var node = _htmlDocument.DocumentNode.SelectSingleNode(xPath);

            return node.GetAttributeValue(attributeName, string.Empty);
        }

        /// <summary>
        /// Check a node is existing with specific xpath and inner text
        /// </summary>
        /// <param name="xPath"></param>
        /// <param name="innerText"></param>
        /// <returns></returns>
        public bool IsNodeExisted(string xPath, string innerText)
        {
            var list = _htmlDocument.DocumentNode.SelectNodes(xPath).ToList();
            return list.Any(p => innerText.Equals(p.InnerText.Replace(Environment.NewLine, string.Empty).Trim()));
        }

        /// <summary>
        /// Get a html table with specific xpath 
        /// </summary>
        /// <param name="xPath"></param>
        /// <returns></returns>
        public DataTable GetTable(string xPath)
        {
            xPath = string.Format("{0}/tr", xPath);
            var nodes = _htmlDocument.DocumentNode.SelectNodes(xPath);
            var table = new DataTable("HtmlTable");

            var headers = nodes[0]
                .Elements("th")
                .Select(th => th.InnerText.Replace(Environment.NewLine, string.Empty).Trim());
            foreach (var header in headers)
            {
                table.Columns.Add(header);
            }
            var rows = nodes.Skip(1).Select(tr => tr
                .Elements("td")
                .Select(td => td.InnerText.Replace(Environment.NewLine, string.Empty).Trim())
                .ToArray());
            foreach (var row in rows)
            {
                table.Rows.Add(row);
            }

            return table;
        }
    }
}
