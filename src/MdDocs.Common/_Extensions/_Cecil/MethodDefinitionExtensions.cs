using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;

namespace Grynwald.MdDocs.Common
{
    public static class MethodDefinitionExtensions
    {
        /// <summary>
        /// Gets a methods's public custom attributes excluding attributes emitted by the C# compiler not relevant for the user.
        /// </summary>
        /// <returns>
        /// Returns all attributes except:
        /// <list type="bullet">
        ///     <item><c>ExtensionAttribute</c> </item>
        ///     <item>non-public Attribute types</item>
        /// </list>
        /// </returns>
        public static IEnumerable<CustomAttribute> GetCustomAttributes(this MethodDefinition parameter)
        {
            return parameter.CustomAttributes
                .Where(attribute =>
                {
                    if (attribute.AttributeType.FullName == SystemTypeNames.ExtensionAttributeFullName)
                        return false;

                    if (!attribute.AttributeType.Resolve().IsPublic)
                        return false;

                    return true;
                });
        }
    }
}
