using System;
using System.Collections.Generic;
using System.Linq;

namespace Grynwald.MdDocs.ApiReference.Model
{
    /// <summary>
    /// Identifies a constructed type (a generic type with type arguments).
    /// </summary>
    public sealed class GenericTypeInstanceId : TypeId, IEquatable<GenericTypeInstanceId>
    {
        /// <summary>
        /// Get type type's type arguments.
        /// </summary>
        public IReadOnlyList<TypeId> TypeArguments { get; }

        /// <inheritdoc />
        public override string DisplayName =>
            $"{Name}<{String.Join(", ", TypeArguments.Select(a => a.DisplayName))}>";

        /// <inheritdoc />
        public override bool IsVoid => false;


        /// <summary>
        /// Initializes a new instance of <see cref="GenericTypeInstanceId"/>.
        /// </summary>
        /// <param name="namespaceName">The namespace the type is defined in.</param>
        /// <param name="name">The type's name.</param>
        public GenericTypeInstanceId(string namespaceName, string name, IReadOnlyList<TypeId> typeArguments)
            : this(new NamespaceId(namespaceName), name, typeArguments)
        { }

        /// <param name="typeArguments">The type's type arguments.</param>/// <summary>
        /// Initializes a new instance of <see cref="GenericTypeInstanceId"/>.
        /// </summary>
        /// <param name="namespace">The namespace the type is defined in.</param>
        /// <param name="name">The type's name.</param>
        /// <param name="typeArguments">The type's type arguments.</param>
        public GenericTypeInstanceId(NamespaceId @namespace, string name, IReadOnlyList<TypeId> typeArguments) : base(@namespace, name)
        {
            TypeArguments = typeArguments ?? throw new ArgumentNullException(nameof(typeArguments));
        }


        /// <inheritdoc />
        public override bool Equals(TypeId other) => Equals(other as GenericTypeInstanceId);

        /// <inheritdoc />
        public override bool Equals(object obj) => Equals(obj as GenericTypeInstanceId);

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                var hash = base.GetHashCode();

                foreach (var argument in TypeArguments)
                {
                    hash ^= argument.GetHashCode();
                }

                return hash;
            }
        }

        /// <summary>
        /// Determines whether this instance of <see cref="GenericTypeId"/> refers to the same type as <paramref name="other"/>.
        /// </summary>
        public bool Equals(GenericTypeInstanceId other)
        {
            if (ReferenceEquals(this, other))
                return true;

            if (other == null)
                return false;

            return base.Equals(other) &&
                   TypeArguments.SequenceEqual(other.TypeArguments);
        }
    }
}
