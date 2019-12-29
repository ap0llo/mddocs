#nullable disable

using System;

namespace Grynwald.MdDocs.ApiReference.Model
{
    /// <summary>
    /// Identifies a type.
    /// </summary>
    public abstract class TypeId : MemberId, IEquatable<TypeId>
    {
        /// <summary>
        /// Gets the namespace the type is defined in.
        /// </summary>
        public NamespaceId Namespace { get; }

        /// <summary>
        /// Gets the type's name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the types display name (the name as it should be shown to the user).
        /// </summary>
        public abstract string DisplayName { get; }

        /// <summary>
        /// Determines whether this type is the void type (<see cref="System.Void" />).
        /// </summary>
        public abstract bool IsVoid { get; }

        /// <summary>
        /// Gets whether the type is a nested type
        /// </summary>
        public bool IsNestedType => DeclaringType != null;

        /// <summary>
        /// Gets the type's parent type if the type is a nested type
        /// </summary>
        public TypeId DeclaringType { get; }


        /// <summary>
        /// Initializes a new instance of <see cref="TypeId"/>.
        /// </summary>
        /// <param name="namespace">The namespace the type is defined in.</param>
        /// <param name="name">The type's name.</param>
        // private protected constructor => prevent implementation outside of this assembly
        private protected TypeId(NamespaceId @namespace, string name)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentException("Value must not be null or empty", nameof(name));

            Namespace = @namespace ?? throw new ArgumentNullException(nameof(@namespace));
            Name = name;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="TypeId"/> for a nested type.
        /// </summary>
        /// <param name="declaringType">The type that defines the nested type</param>
        /// <param name="name">The type's name.</param>
        // private protected constructor => prevent implementation outside of this assembly
        private protected TypeId(TypeId declaringType, string name)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentException("Value must not be null or empty", nameof(name));

            DeclaringType = declaringType ?? throw new ArgumentNullException(nameof(declaringType));
            Namespace = declaringType.Namespace;
            Name = name;
        }


        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                var hash = Namespace.GetHashCode() * 397;
                hash ^= StringComparer.Ordinal.GetHashCode(Name);
                return hash;
            }
        }

        /// <summary>
        /// Determines whether this instance of <see cref="TypeId" /> refers to the same type as <paramref name="other"/>.
        /// </summary>
        public virtual bool Equals(TypeId other)
        {
            if (ReferenceEquals(this, other))
                return true;

            if (other == null)
                return false;

            if (IsNestedType)
            {
                return DeclaringType.Equals(other.DeclaringType) &&
                       StringComparer.Ordinal.Equals(Name, other.Name);
            }
            else
            {
                return Namespace.Equals(other.Namespace) &&
                       StringComparer.Ordinal.Equals(Name, other.Name);
            }
        }
    }
}
