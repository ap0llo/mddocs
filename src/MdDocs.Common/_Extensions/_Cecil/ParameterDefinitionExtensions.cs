using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;

namespace Grynwald.MdDocs.Common
{
    public static class ParameterDefinitionExtensions
    {
        /// <summary>
        /// Gets a parameter's custom attributes excluding attributes emitted by the C# compiler not relevant for the user.
        /// </summary>
        /// <returns>
        /// Returns all attributes except:
        /// <list type="bullet">
        ///     <item><c>IsReadOnlyAttribute</c> </item>
        ///     <item><c>ParamArrayAttribute</c> </item>
        ///     <item>non-public Attribute types</item>
        /// </list>
        /// </returns>
        public static IEnumerable<CustomAttribute> GetCustomAttributes(this ParameterDefinition parameter)
        {
            return parameter.CustomAttributes
                .Where(attribute =>
                {
                    if (attribute.AttributeType.FullName == SystemTypeNames.IsReadOnlyAttributeFullName)
                        return false;

                    if (attribute.AttributeType.FullName == SystemTypeNames.ParamArrayAttributeFullName)
                        return false;

                    if (!attribute.AttributeType.Resolve().IsPublic)
                        return false;

                    return true;
                });
        }
    }
}
