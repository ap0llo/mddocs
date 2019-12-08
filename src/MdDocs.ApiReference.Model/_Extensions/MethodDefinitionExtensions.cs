using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;

namespace Grynwald.MdDocs.ApiReference.Model
{
    internal static class MethodDefinitionExtensions
    {
        /// <summary>
        /// Gets a methods's custom attributes excluding attributes emitted by the C# compiler not relevant for the user.
        /// </summary>
        /// <returns>
        /// Returns all attributes except:
        /// <list type="bullet">
        ///     <item><c>ExtensionAttribute</c> </item>
        /// </list>
        /// </returns>
        public static IEnumerable<CustomAttribute> GetCustomAttributes(this MethodDefinition parameter)
        {
            return parameter.CustomAttributes
                .Where(attribute =>
                {
                    if (attribute.AttributeType.FullName == Constants.ExtensionAttributeFullName)
                        return false;

                    return true;
                });
        }
    }
}
