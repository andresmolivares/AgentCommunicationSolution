using System.Xml.Linq;
using System.Xml.XPath;

namespace DDS.DCP.Extensions
{
    public static class XDocumentExtensions
    {
        /// <summary>
        /// Get the value from the element of a valid <see cref="XDocument"/> instance.
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="nodePath"></param>
        /// <returns></returns>
        public static string GetElementValueFromXPath(this XNode owner, string nodePath)
        {
            if (owner != null)
            {
                // Use specified XPath notation to find element
                XElement element = owner.XPathSelectElement(nodePath);
                return element != null ? element.Value : string.Empty;
            }
            return string.Empty;
        }
    }
}
