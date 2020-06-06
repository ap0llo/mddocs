using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;

namespace Grynwald.MdDocs.Common
{
    public static class FieldDefinitionExtensions
    {
        /// <summary>
        /// Gets a fields's public custom attributes excluding attributes emitted by the C# compiler not relevant for the user.
        /// </summary>
        /// <returns>
        /// Returns all attributes except:
        /// <list type="bullet">        
        ///     <item>non-public Attribute types</item>
        /// </list>
        /// </returns>
        public static IEnumerable<CustomAttribute> GetCustomAttributes(this FieldDefinition parameter)
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
