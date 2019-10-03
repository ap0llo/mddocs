using System;
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
    }
}
