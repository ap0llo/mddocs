using System;
using System.Collections.Generic;
using Mono.Cecil;

namespace Grynwald.MdDocs.CommandLineHelp
{
    internal static class TypeDefinitionExtensions
    {
        /// <summary>
        /// Gets all the type's properties including the properties defined in base classes.
        /// </summary>
        public static IEnumerable<PropertyDefinition> GetAllProperties(this TypeDefinition type)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));

            var properties = new List<PropertyDefinition>();

            TypeDefinition? currentType = type;
            while (currentType != null)
            {
                properties.AddRange(currentType.Properties);
                currentType = currentType.BaseType?.Resolve();
            }

            return properties;
        }
    }
}
