using System;
using System.Collections.Generic;

namespace Grynwald.MdDocs.ApiReference.Model
{
    /// <summary>
    /// Identifies a non-generic type.
    /// </summary>
    public sealed class SimpleTypeId : TypeId, IEquatable<SimpleTypeId>
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
            { "System.Void", "void" }
        };

        /// <summary>
        /// Gets the type's display name.
        /// </summary>
        /// <remarks>
        /// For built-in types like <c>System.String</c> this will return the C# name of the type, e.g. <c>string</c>.
        /// </remarks>
        public override string DisplayName
        {
            get
            {
                var name = IsNestedType ? $"{DeclaringType.DisplayName}.{Name}" : Name;

                var namespaceAndName = String.IsNullOrEmpty(Namespace.Name) ? name : $"{Namespace.Name}.{name}";
                return s_BuiltInTypes.TryGetValue(namespaceAndName, out var builtinName)
                    ? builtinName
                    : name;
            }
        }


        /// <summary>
        /// Gets whether this type id refers to <see cref="System.Void"/>
        /// </summary>
        public override bool IsVoid => !IsNestedType && Namespace.IsSystem && Name == "Void";

        /// <summary>
        /// Initializes a new instance of <see cref="SimpleTypeId"/>.
        /// </summary>
        /// <param name="namespaceName">The namespace the type is defined in.</param>
        /// <param name="name">The type's name.</param>
        public SimpleTypeId(string namespaceName, string name) : this(new NamespaceId(namespaceName), name)
        { }

        /// <summary>
        /// Initializes a new instance of <see cref="SimpleTypeId"/>.
        /// </summary>
        /// <param name="namespace">The namespace the type is defined in.</param>
        /// <param name="name">The type's name.</param>
        public SimpleTypeId(NamespaceId @namespace, string name) : base(@namespace, name)
        { }

        /// <summary>
        /// Initializes a new instance of <see cref="SimpleTypeId"/> for a nested type.
        /// </summary>
        /// <param name="declaringType">The type the nested type is defined in.</param>
        /// <param name="name">The type's name.</param>
        public SimpleTypeId(TypeId declaringType, string name) : base(declaringType, name)
        { }


        /// <inheritdoc />
        public override bool Equals(object obj) => Equals(obj as SimpleTypeId);

        /// <inheritdoc />
        public override bool Equals(TypeId other) => other is SimpleTypeId && base.Equals(other);

        /// <inheritdoc />
        public override int GetHashCode() => base.GetHashCode();

        /// <summary>
        /// Determines whether this instance of <see cref="SimpleTypeId"/> refers to the same type as <paramref name="other"/>.
        /// </summary>
        public bool Equals(SimpleTypeId other) => other != null && Equals((TypeId)other);
    }
}
