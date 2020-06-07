using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;

namespace Grynwald.MdDocs.Common
{
    public static class PropertyDefinitionExtensions
    {
        /// <summary>
        /// Gets a property's public custom attributes excluding attributes emitted by the C# compiler not relevant for the user.
        /// </summary>
        /// <returns>
        /// Returns all attributes except:
        /// <list type="bullet">        
        ///     <item>non-public Attribute types</item>
        /// </list>
        /// </returns>
        public static IEnumerable<CustomAttribute> GetCustomAttributes(this PropertyDefinition parameter)
        {
            return parameter.CustomAttributes
                .Where(attribute =>
                {
                    if (!attribute.AttributeType.Resolve().IsPublic)
                        return false;

                    return true;
                });
        }
    }
}
