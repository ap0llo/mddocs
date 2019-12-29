using System.Diagnostics.CodeAnalysis;
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
#if NETSTANDARD2_1 || NETCOREAPP3_0 || NETCOREAPP3_1
        public static bool TryGetAttributeValue(this XElement element, string name, [NotNullWhen(true)]out string? value)
#else
        public static bool TryGetAttributeValue(this XElement element, string name, out string? value)
#endif
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
