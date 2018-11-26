using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MdDoc.Model
{
    sealed class TypeNameFormatter
    {
        public static readonly TypeNameFormatter Instance = new TypeNameFormatter();

        private static readonly IReadOnlyDictionary<string, string> s_BuiltInTypes = new Dictionary<string, string>()
        {
            { "System.Boolean", "bool" },
            { "System.Byte", "byte" },
            { "System.SByte", "sbyte" },
            { "System.Char", "char" },
            { "System.Decimal", "decimal" },
            { "System.Double", "double" },
            { "System.Single", "float" },
            { "System.Int32", "int" },
            { "System.UInt32", "uint" },
            { "System.Int64", "long" },
            { "System.UInt64", "ulong" },
            { "System.Object", "object" },
            { "System.Int16", "short" },
            { "System.UInt16", "ushort" },
            { "System.String", "string" },

        };

        private TypeNameFormatter()
        {
        }


        public string GetTypeName(TypeReference reference)
        {
            if (s_BuiltInTypes.ContainsKey(reference.FullName))
            {
                return s_BuiltInTypes[reference.FullName];
            }
            else if (reference.IsArray)
            {
                var elementTypeName = GetTypeName(reference.GetElementType());
                return $"{elementTypeName}[]";
            }
            else if (reference is GenericInstanceType genericType && genericType.HasGenericArguments)
            {
                var resultBuilder = new StringBuilder();
                
                // The numer of type parameters is appended to the type name after a '`'
                // Remove this suffix from the type name as the parameters will be appended in C# syntax (< and >)
                var typeName = genericType.Name.Replace($"`{genericType.GenericArguments.Count}", "");
                resultBuilder.Append(typeName);


                resultBuilder.Append("<");
                resultBuilder.AppendJoin(", ", genericType.GenericArguments.Select(GetTypeName));
                resultBuilder.Append(">");

                return resultBuilder.ToString();

            }
            else
            {
                return reference.Name;
            }
        }
    }
}
