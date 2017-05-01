using System;
using System.IO;
using System.Xml;


namespace Public.Common.Lib.Extensions
{
    public static class XmlDocumentExtensions
    {
		/// <summary>
        /// Loads an XML document from a file, bypassing the namespace declarations, so that basic xpath queries without namespace prefixes can be used.
        /// </summary>
		public static void LoadNoNamespaces(this XmlDocument document, string filePath)
        {
            using (FileStream fStream = new FileStream(filePath, FileMode.Open))
            {
                XmlTextReader reader = new XmlTextReader(fStream);
                reader.Namespaces = false;

                document.Load(reader);
            }
        }
    }
}
