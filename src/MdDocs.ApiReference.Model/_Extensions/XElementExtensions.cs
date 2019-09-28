using System.Xml.Linq;

namespace Grynwald.MdDocs.ApiReference.Model
{
    public static class XElementExtensions
    {
        /// <summary>
        /// Attempts to the value of the specified attribute.
        /// </summary>
        /// <param name="element"></param>
        /// <returns>Returns <c>true</c> if the attribute was found, otherwise <c>false</c></returns>
        public static bool TryGetAttributeValue(this XElement element, string name, out string value)
        {
            var attribute = element.Attribute(name);
            if (attribute != null)
            {
                value = attribute.Value;
                return true;
            }
            else
            {
                value = default;
                return false;
            }
        }
    }
}
