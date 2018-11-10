using System;
using Mono.Cecil;

namespace MdDoc.Model
{
    static class TypeDefinitionExtensions
    {
        public static TypeKind Kind(this TypeDefinition type)
        {
            if (type.IsEnum)
                return TypeKind.Enum;

            if (type.IsClass)
                return type.IsValueType ? TypeKind.Struct : TypeKind.Class;

            if (type.IsInterface)
                return TypeKind.Interface;

            
            throw new InvalidOperationException();
        }        
    }
}
