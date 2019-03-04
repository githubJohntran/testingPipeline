using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace StreetlightVision.Utilities
{
    public class XmlUtility
    {
        private XmlDocument _xmlDocument;
        public XmlUtility()
        {
            _xmlDocument = new XmlDocument();
        }

        public XmlUtility(string filePath) : this()
        {
            Load(filePath);
        }

        /// <summary>
        /// Load a xml file
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="isFilePath"></param>
        public void Load(string filePath)
        {
            _xmlDocument.Load(filePath);
        }

        /// <summary>
        ///  Load a stream
        /// </summary>
        /// <param name="stream"></param>
        public void Load(Stream stream)
        {
            _xmlDocument.Load(stream);
        }

        /// <summary>
        /// Get a list nodes that matches specific xpath
        /// </summary>
        /// <param name="xPath"></param>
        /// <returns></returns>
        public List<XmlNode> GetNodes(string xPath)
        {
            List<XmlNode> result = null;
            var list = _xmlDocument.SelectNodes(xPath);

            if (list.Count > 0)
            {
                result = new List<XmlNode>();
                foreach (XmlNode node in list)
                {
                    result.Add(node);
                }
            }

            return result;
        }

        /// <summary>
        /// Get list child nodes of a node that matches specific xpath
        /// </summary>
        /// <param name="xPath"></param>
        /// <returns></returns>
        public List<XmlNode> GetChildNodes(string xPath)
        {
            List<XmlNode> result = null;
            var list = _xmlDocument.SelectSingleNode(xPath).ChildNodes;

            if (list.Count > 0)
            {
                result = new List<XmlNode>();
                foreach (XmlNode node in list)
                {
                    result.Add(node);
                }
            }

            return result;
        }

        /// <summary>
        /// Get a first single node that matches specific xpath
        /// </summary>
        /// <param name="xPath"></param>
        /// <returns></returns>
        public XmlNode GetSingleNode(string xPath)
        {
            return _xmlDocument.SelectSingleNode(xPath);
        }

        /// <summary>
        /// Get a node that matches specific xpath and inner text
        /// </summary>
        /// <param name="xPath"></param>
        /// <param name="innerText"></param>
        /// <returns></returns>
        public XmlNode GetNode(string xPath, string innerText)
        {
            var list = _xmlDocument.SelectNodes(xPath);

            foreach (XmlNode node in list)
            {
                if (node.InnerText.Trim().Equals(innerText))
                    return node;
            }

            return null;
        }

        /// <summary>
        /// Check a node is existing with specific xpath and inner text
        /// </summary>
        /// <param name="xPath"></param>
        /// <param name="innerText"></param>
        /// <returns></returns>
        public bool IsNodeExisted(string xPath, string innerText)
        {
            var list = _xmlDocument.SelectNodes(xPath);

            foreach (XmlNode node in list)
            {
                if (node.InnerText.Trim().Equals(innerText))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Get a list text of nodes that matches specific xpath
        /// </summary>
        /// <param name="xPath"></param>
        /// <returns></returns>
        public List<string> GetNodesText(string xPath)
        {
            List<string> result = new List<string>();
            var list = _xmlDocument.SelectNodes(xPath);

            foreach (XmlNode node in list)
            {
                result.Add(node.InnerText.Trim());
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
            List<string> result = new List<string>();
            var list = _xmlDocument.SelectSingleNode(xPath).ChildNodes;

            foreach (XmlNode node in list)
            {
                result.Add(node.InnerText.Trim());
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
            var node = _xmlDocument.SelectSingleNode(xPath);

            if (node != null)
                return node.InnerText.Trim();

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
            List<string> result = new List<string>();
            var list = _xmlDocument.SelectNodes(xPath);

            foreach (XmlNode node in list)
            {
                result.Add(node.Attributes[attributeName].ToString());
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
            var node = _xmlDocument.SelectSingleNode(xPath);

            return node.Attributes[attributeName].Value;
        }

        public bool IsInnerTextContains(string text)
        {
            return _xmlDocument.InnerText.IndexOf(text) >= 0;
        }
    }

    /// <summary>
    /// Defines extension methods for XmlUtility
    /// </summary>
    public static class XmlExtension
    {
        /// <summary>
        /// Get child node based on its tag name
        /// </summary>
        /// <param name="node"></param>
        /// <param name="tagName"></param>
        /// <returns></returns>
        public static XmlNode GetChildNode(this XmlNode node, string tagName)
        {
            return node[tagName];
        }

        /// <summary>
        /// Get text of child node based on its tag name
        /// </summary>
        /// <param name="node"></param>
        /// <param name="tagName"></param>
        /// <returns></returns>
        public static string GetChildNodeText(this XmlNode node, string tagName)
        {
            return node[tagName].InnerText.Trim();
        }

        /// <summary>
        ///  Get an attribute value of node
        /// </summary>
        /// <param name="node"></param>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        public static string GetAttrVal(this XmlNode node, string attributeName)
        {
            if(node.Attributes != null && node.Attributes[attributeName] != null)
                return node.Attributes[attributeName].Value.ToString();
            return null;
        }      
    }
}
