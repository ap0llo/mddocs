using System;
using System.Collections.Generic;
using System.Linq;

namespace Grynwald.MdDocs.ApiReference.Model
{
    /// <summary>
    /// Identifies a property or an indexer.
    /// </summary>
    public sealed class PropertyId : TypeMemberId, IEquatable<PropertyId>
    {
        /// <summary>
        /// The types of the property's parameters.
        /// </summary>
        /// <remarks>
        /// Regular properties do not have any parameters and this list will be empty.
        /// However, indexers are implemented as properties and have parameters.
        /// </remarks>
        public IReadOnlyList<TypeId> Parameters { get; }


        /// <summary>
        /// Initializes a new instance of <see cref="PropertyId"/>.
        /// </summary>
        /// <param name="definingType">The type defining the property.</param>
        /// <param name="name">The name of the property.</param>
        public PropertyId(TypeId definingType, string name) : this(definingType, name, Array.Empty<TypeId>())
        { }

        /// <summary>
        /// Initializes a new instance of <see cref="PropertyId"/>.
        /// </summary>
        /// <param name="definingType">The type defining the property/indexer.</param>
        /// <param name="name">The name of the property/indexer.</param>
        /// <param name="parameters">The types of the indexer's parameters.</param>
        public PropertyId(TypeId definingType, string name, IReadOnlyList<TypeId> parameters) : base(definingType, name)
        {
            Parameters = parameters ?? throw new ArgumentNullException(nameof(parameters));
        }


        /// <inheritdoc />
        public override bool Equals(object? obj) => Equals(obj as PropertyId);

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                var hash = DefiningType.GetHashCode() * 397;
                hash ^= StringComparer.Ordinal.GetHashCode(Name);

                foreach (var parameter in Parameters)
                {
                    hash ^= parameter.GetHashCode();
                }

                return hash;
            }
        }

        /// <summary>
        /// Determines whether this instance of <see cref="PropertyId"/> refers to the same property of indexer as <paramref name="other"/>.
        /// </summary>
        public bool Equals(PropertyId? other)
        {
            if (ReferenceEquals(this, other))
                return true;

            if (other == null)
                return false;

            return DefiningType.Equals(other.DefiningType) &&
                   StringComparer.Ordinal.Equals(Name, other.Name) &&
                   Parameters.Count == other.Parameters.Count &&
                   Parameters.SequenceEqual(other.Parameters);
        }
    }
}
