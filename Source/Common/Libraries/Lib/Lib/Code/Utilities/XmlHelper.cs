using System;
using System.Collections.Generic;
using System.Xml;


namespace Public.Common.Lib
{
    public class XmlHelper
    {
        #region Static

        public static XmlElement CreateElement(XmlDocument document, string nodeName, string nodeInnerText)
        {
            XmlElement output = document.CreateElement(nodeName);

            if (!String.IsNullOrEmpty(nodeInnerText))
            {
                output.InnerText = nodeInnerText;
            }

            return output;
        }

        public static XmlElement CreateElement(XmlNode nodeInDocument, string nodeName, string nodeInnerText)
        {
            XmlElement output = XmlHelper.CreateElement(nodeInDocument.OwnerDocument, nodeName, nodeInnerText);
            return output;
        }

        public static XmlElement CreateElement(XmlNode nodeInDocument, string nodeName)
        {
            XmlElement output = XmlHelper.CreateElement(nodeInDocument, nodeName, String.Empty);
            return output;
        }

        public static XmlElement CreateElement(XmlDocument document, string nodeName, string nodeInnerText, IEnumerable<Tuple<string, string>> attributes)
        {
            XmlElement output = XmlHelper.CreateElement(document, nodeName, nodeInnerText);

            foreach (Tuple<string, string> attribute in attributes)
            {
                XmlAttribute attributeXml = XmlHelper.CreateAttribute(document, attribute.Item1, attribute.Item2);
                output.Attributes.Append(attributeXml);
            }

            return output;
        }

        public static XmlElement CreateElement(XmlNode nodeInDocument, string nodeName, string nodeInnerText, IEnumerable<Tuple<string, string>> attributes)
        {
            XmlElement output = XmlHelper.CreateElement(nodeInDocument.OwnerDocument, nodeName, nodeInnerText, attributes);
            return output;
        }

        public static XmlElement CreateElement(XmlDocument document, string nodeName, IEnumerable<Tuple<string, string>> attributes)
        {
            XmlElement output = XmlHelper.CreateElement(document, nodeName, null, attributes);
            return output;
        }

        public static XmlElement CreateElement(XmlNode nodeInDocument, string nodeName, IEnumerable<Tuple<string, string>> attributes)
        {
            XmlElement output = XmlHelper.CreateElement(nodeInDocument.OwnerDocument, nodeName, null, attributes);
            return output;
        }

        public static XmlAttribute CreateAttribute(XmlDocument document, string name, string value)
        {
            XmlAttribute output = document.CreateAttribute(name);
            output.Value = value;

            return output;
        }

        public static XmlAttribute CreateAttribute(XmlNode nodeInDocument, string name, string value)
        {
            XmlAttribute output = XmlHelper.CreateAttribute(nodeInDocument.OwnerDocument, name, value);
            return output;
        }

        public static XmlElement AddChildElement(XmlNode parentNode, string nodeName, string nodeInnerText, XmlNode afterNode)
        {
            XmlElement child = XmlHelper.CreateElement(parentNode, nodeName, nodeInnerText);
            parentNode.InsertAfter(child, afterNode);

            return child;
        }

        public static XmlElement AddChildElement(XmlNode parentNode, string nodeName, XmlNode afterNode)
        {
            XmlElement child = parentNode.OwnerDocument.CreateElement(nodeName);
            parentNode.InsertAfter(child, afterNode);

            return child;
        }

        public static XmlElement AddChildElement(XmlNode parentNode, string nodeName, string nodeInnerText)
        {
            XmlElement child = XmlHelper.CreateElement(parentNode, nodeName, nodeInnerText);
            parentNode.AppendChild(child);

            return child;
        }

        public static XmlElement AddChildElement(XmlNode parentNode, string nodeName, string nodeInnerText, IEnumerable<Tuple<string, string>> attributes)
        {
            XmlElement child = XmlHelper.CreateElement(parentNode, nodeName, nodeInnerText, attributes);
            parentNode.AppendChild(child);

            return child;
        }

        public static XmlElement AddChildElement(XmlNode parentNode, string nodeName, IEnumerable<Tuple<string, string>> attributes)
        {
            XmlElement child = XmlHelper.CreateElement(parentNode, nodeName, null, attributes);
            parentNode.AppendChild(child);

            return child;
        }

        public static void FixXmlDocumentNamespaceForSave(XmlDocument document, string desiredNamespace)
        {
            document.FirstChild.NextSibling.Attributes.RemoveNamedItem(@"xmlns");
            document.DocumentElement.SetAttribute(@"xmlns", desiredNamespace);
        }

        #endregion
    }
}
