using System;
using System.Collections.Generic;
using System.Linq;

using Mono.Cecil;

namespace MdDoc
{
    static class TypeDefinitionExtensions
    {
        public static TypeKind Kind(this TypeDefinition type)
        {
            if (type.IsEnum)
                return TypeKind.Enum;

            if (type.IsClass)
                return TypeKind.Class;

            if (type.IsInterface)
                return TypeKind.Interface;

            if (type.IsValueType)
                return TypeKind.Struct;

            throw new InvalidOperationException();
        }

        public static IEnumerable<MethodDefinition> GetDocumentedConstrutors(this TypeDefinition type, DocumentationContext context)
        {
            if (type.Kind() == TypeKind.Class || type.Kind() == TypeKind.Struct)
            {
                return type
                    .Methods
                    .Where(m => m.IsConstructor)
                    .Where(context.IsDocumentedItem);                
            }
            else
            {
                return Enumerable.Empty<MethodDefinition>();
            }
        }

    }
}
