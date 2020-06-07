using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;

namespace Grynwald.MdDocs.Common
{
    public static class TypeDefinitionExtensions
    {
        public static (string name, long value)[] GetEnumValues(this TypeDefinition type)
        {
            if (!type.IsEnum)
                throw new InvalidOperationException($"Type '{type.FullName}' is not a enum");

            return type.Fields
                .Where(f => f.IsPublic && !f.IsSpecialName)
                .Select(f => (f.Name, Convert.ToInt64(f.Constant)))
                .ToArray();
        }


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
