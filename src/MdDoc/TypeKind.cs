using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Text;

namespace MdDoc
{
    public enum TypeKind
    {        
        Class = 1,
        Struct = 2,
        Interface = 3,
        Enum = 4
    }

    public static class TypeDefinitionExtensions
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
    }
}
