using System;
using System.Collections.Generic;
using System.Linq;

namespace Grynwald.MdDocs.ApiReference.Model
{
    /// <summary>
    /// Identifies a method.
    /// </summary>
    public sealed class MethodId : TypeMemberId, IEquatable<MethodId>
    {
        /// <summary>
        /// Gets the method's number of type parameters.
        /// </summary>
        public int Arity { get; }

        /// <summary>
        /// Gets the types of the method's parameters.
        /// </summary>
        public IReadOnlyList<TypeId> Parameters { get; }

        /// <summary>
        /// Gets the method's return type.
        /// </summary>
        /// <remarks>
        /// The return type is only relevant if the method is an overload of the <c>implicit</c> or <c>explicit</c> operators.
        /// For all other methods, the return type is not part of a method's identifying properties.
        /// </remarks>
        public TypeId ReturnType { get; }


        /// <summary>
        /// Initializes a new instance of <see cref="MethodId"/>.
        /// </summary>
        /// <param name="definingType">The type defining the method.</param>
        /// <param name="name">The name of the method.</param>
        public MethodId(TypeId definingType, string name) : this(definingType, name, 0, Array.Empty<TypeId>(), null)
        { }

        /// <summary>
        /// Initializes a new instance of <see cref="MethodId"/>.
        /// </summary>
        /// <param name="definingType">The type defining the method.</param>
        /// <param name="name">The name of the method.</param>
        /// <param name="parameters">The types of the method's parameters.</param>
        public MethodId(TypeId definingType, string name, IReadOnlyList<TypeId> parameters) : this(definingType, name, 0, parameters, null)
        { }

        /// <summary>
        /// Initializes a new instance of <see cref="MethodId"/>.
        /// </summary>
        /// <param name="definingType">The type defining the method.</param>
        /// <param name="name">The name of the method.</param>
        /// <param name="arity">The method's number of type parameters.</param>
        /// <param name="parameters">The types of the method's parameters.</param>
        public MethodId(TypeId definingType, string name, int arity, IReadOnlyList<TypeId> parameters) : this(definingType, name, arity, parameters, null)
        { }

        /// <summary>
        /// Initializes a new instance of <see cref="MethodId"/>.
        /// </summary>
        /// <param name="definingType">The type defining the method.</param>
        /// <param name="name">The name of the method.</param>
        /// <param name="arity">The method's number of type parameters.</param>
        /// <param name="parameters">The types of the method's parameters.</param>
        /// <param name="returnType">
        /// The methods return type.
        /// The return type is only relevant if the method is an overload of the <c>implicit</c> or <c>explicit</c> operators.
        /// For all other methods, the return type is not part of a method's identifying properties.
        /// </param>
        public MethodId(TypeId definingType, string name, int arity, IReadOnlyList<TypeId> parameters, TypeId returnType) : base(definingType, name)
        {
            Arity = arity;
            Parameters = parameters ?? throw new ArgumentNullException(nameof(parameters));
            ReturnType = returnType;
        }


        /// <inheritdoc />
        public override bool Equals(object obj) => Equals(obj as MethodId);

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                var hash = DefiningType.GetHashCode() * 397;
                hash ^= StringComparer.Ordinal.GetHashCode(Name);
                hash ^= Arity.GetHashCode();

                foreach (var parameter in Parameters)
                {
                    hash ^= parameter.GetHashCode();
                }

                if (ReturnType != null)
                    hash ^= ReturnType.GetHashCode();

                return hash;
            }
        }

        /// <summary>
        /// Determines whether this instance of <see cref="MethodId"/> refers to the same method as <paramref name="other"/>.
        /// </summary>
        public bool Equals(MethodId other)
        {
            if (ReferenceEquals(this, other))
                return true;

            if (other == null)
                return false;

            return DefiningType.Equals(other.DefiningType) &&
                   StringComparer.Ordinal.Equals(Name, other.Name) &&
                   Arity == other.Arity &&
                   Parameters.SequenceEqual(other.Parameters) &&
                   (
                       (ReturnType == null && other.ReturnType == null) ||
                       (ReturnType != null && ReturnType.Equals(other.ReturnType))
                   );
        }
    }
}
