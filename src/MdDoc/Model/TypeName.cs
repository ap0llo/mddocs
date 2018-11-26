using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MdDoc.Model
{
    public sealed class TypeName
    {        
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

        private readonly TypeReference m_TypeReference;


        /// <summary>
        /// Gets the type's namespace
        /// </summary>
        public string Namespace => m_TypeReference.Namespace;

        /// <summary>
        /// Gets the full name of the type including type parameters and namespace
        /// </summary>
        public string FullName => $"{Namespace}.{Name}";

        /// <summary>
        /// Gets the name of the types including type parameters
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the base name of the type (the name without type parameters)
        /// </summary>
        public string BaseName { get; }

        /// <summary>
        /// Gets whether the type is an array
        /// </summary>
        public bool IsArray => ElementType != null;

        /// <summary>
        /// Gets the array element type if the type is an array (null otherwise)
        /// </summary>
        public TypeName ElementType { get; }

        /// <summary>
        /// Gets the type's generic type arguments. 
        /// For non-generic types returns an empty list
        /// </summary>
        public IReadOnlyList<TypeName> TypeArguments { get; } = Array.Empty<TypeName>();


        public TypeName(TypeReference typeReference)
        {
            m_TypeReference = typeReference ?? throw new ArgumentNullException(nameof(typeReference));

            BaseName = m_TypeReference.Name;
            if(m_TypeReference.IsArray)
            {
                ElementType = new TypeName(typeReference.GetElementType());
            }
            else if (typeReference is GenericInstanceType genericType && genericType.HasGenericArguments)
            {
                // The number of type parameters is appended to the type name after a '`'
                // Remove this suffix to determine the "base name" of the type
                BaseName = genericType.Name.Replace($"`{genericType.GenericArguments.Count}", "");

                TypeArguments = genericType.GenericArguments.Select(x => new TypeName(x)).ToArray();
            }

            Name = GetTypeName();            
        }


        public override string ToString() => Name;

        private string GetTypeName()
        {            
            if (s_BuiltInTypes.ContainsKey(m_TypeReference.FullName))
            {
                return s_BuiltInTypes[m_TypeReference.FullName];
            }
            else if (IsArray)
            {                
                return $"{ElementType}[]";
            }
            else if (TypeArguments.Count > 0)
            {
                var resultBuilder = new StringBuilder();

                resultBuilder.Append(BaseName);
                resultBuilder.Append("<");
                resultBuilder.AppendJoin(", ", TypeArguments);
                resultBuilder.Append(">");

                return resultBuilder.ToString();
            }
            else
            {
                return m_TypeReference.Name;
            }
        }
    }
}
