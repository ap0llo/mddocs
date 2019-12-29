#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;

namespace Grynwald.MdDocs.ApiReference.Model
{
    /// <summary>
    /// Identifies an unbound generic type.
    /// </summary>
    public sealed class GenericTypeId : TypeId, IEquatable<GenericTypeId>
    {
        private readonly IReadOnlyList<string> m_TypeParameterDisplayNames;

        /// <summary>
        /// Gets the number of type parameters in the generic type.
        /// </summary>
        public int Arity { get; }

        /// <inheritdoc />
        public override string DisplayName
        {
            get
            {
                if(!IsNestedType && Arity == 1 && Namespace.IsSystem && Name.Equals("Nullable"))
                {
                    return $"{m_TypeParameterDisplayNames.Single()}?";
                }

                var name = IsNestedType ? $"{DeclaringType.DisplayName}.{Name}" : Name;
                return $"{name}<{String.Join(", ", m_TypeParameterDisplayNames)}>";
            }
        }

        /// <inheritdoc />
        public override bool IsVoid => false;


        /// <summary>
        /// Initializes a new instance of <see cref="GenericTypeId"/>.
        /// </summary>
        /// <param name="namespace">The namespace the type is defined in.</param>
        /// <param name="name">The name of the type.</param>
        /// <param name="arity">The number of type parameter the type defines.</param>
        /// <param name="typeParameterDisplayNames">The display names of the type parameters (e.g. "TKey", "TValue"...)</param>
        public GenericTypeId(NamespaceId @namespace, string name, int arity, IReadOnlyList<string> typeParameterDisplayNames) : base(@namespace, name)
        {
            m_TypeParameterDisplayNames = typeParameterDisplayNames ?? throw new ArgumentNullException(nameof(typeParameterDisplayNames));
            Arity = arity;

            if (typeParameterDisplayNames?.Count != arity)
                throw new ArgumentException("The number of type parameter display names must match the type's arity", nameof(typeParameterDisplayNames));
        }

        /// <summary>
        /// Initializes a new instance of <see cref="GenericTypeId"/>.
        /// </summary>
        /// <param name="namespaceName">The namespace the type is defined in.</param>
        /// <param name="name">The name of the type.</param>
        /// <param name="arity">The number of type parameter the type defines.</param>
        public GenericTypeId(string namespaceName, string name, int arity) : this(new NamespaceId(namespaceName), name, arity)
        { }

        /// <summary>
        /// Initializes a new instance of <see cref="GenericTypeId"/>.
        /// </summary>
        /// <param name="namespace">The namespace the type is defined in.</param>
        /// <param name="name">The name of the type.</param>
        /// <param name="arity">The number of type parameter the type defines.</param>
        public GenericTypeId(NamespaceId @namespace, string name, int arity) : base(@namespace, name)
        {
            Arity = arity;
            m_TypeParameterDisplayNames = arity == 1
                ? new [] { "T" }
                : Enumerable.Range(1, arity).Select(x => "T" + x).ToArray();
        }

        /// <summary>
        /// Initializes a new instance of <see cref="GenericTypeId"/> for a nested type.
        /// </summary>
        /// <param name="declaringType">The type the nested type is defined in.</param>
        /// <param name="name">The name of the type.</param>
        /// <param name="arity">The number of type parameter the type defines.</param>
        /// <param name="typeParameterDisplayNames">The display names of the type parameters (e.g. "TKey", "TValue"...)</param>
        public GenericTypeId(TypeId declaringType, string name, int arity, IReadOnlyList<string> typeParameterDisplayNames) : base(declaringType, name)
        {
            m_TypeParameterDisplayNames = typeParameterDisplayNames ?? throw new ArgumentNullException(nameof(typeParameterDisplayNames));
            Arity = arity;

            if (typeParameterDisplayNames?.Count != arity)
                throw new ArgumentException("The number of type parameter display names must match the type's arity", nameof(typeParameterDisplayNames));
        }

        /// <summary>
        /// Initializes a new instance of <see cref="GenericTypeId"/> for a nested type.
        /// </summary>
        /// <param name="declaringType">The type the nested type is defined in.</param>
        /// <param name="name">The name of the type.</param>
        /// <param name="arity">The number of type parameter the type defines.</param>
        public GenericTypeId(TypeId declaringType, string name, int arity) : base(declaringType, name)
        {
            Arity = arity;
            m_TypeParameterDisplayNames = arity == 1
                ? new[] { "T" }
                : Enumerable.Range(1, arity).Select(x => "T" + x).ToArray();
        }



        /// <inheritdoc />
        public override bool Equals(object obj) => Equals(obj as GenericTypeId);

        /// <inheritdoc />
        public override bool Equals(TypeId other) => Equals(other as GenericTypeId);

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                var hash = base.GetHashCode();
                hash ^= Arity.GetHashCode();
                return hash;
            }
        }

        /// <summary>
        /// Determines whether this instance of <see cref="GenericTypeId"/> refers to the same type as <paramref name="other"/>.
        /// </summary>
        public bool Equals(GenericTypeId other)
        {
            if (ReferenceEquals(this, other))
                return true;

            if (other == null)
                return false;

            return base.Equals(other) &&
                   Arity == other.Arity;
        }
    }
}
