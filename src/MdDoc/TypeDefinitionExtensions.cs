using System;
using System.Collections.Generic;
using System.Linq;
using MdDoc.Model;
using Mono.Cecil;

namespace MdDoc
{
    static class TypeDefinitionExtensions
    {
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

        public static IEnumerable<MethodDefinition> GetDocumentedMethods(this TypeDefinition type, DocumentationContext context)
        {
            if (type.Kind() == TypeKind.Class || type.Kind() == TypeKind.Struct || type.Kind() == TypeKind.Interface)                
            {
                return type.Methods
                    .Where(m => !m.IsConstructor)
                    .Where(context.IsDocumentedItem);
            }
            else
            {
                return Enumerable.Empty<MethodDefinition>();
            }
        }




    }
}
