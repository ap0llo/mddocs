#nullable disable

using System;

namespace Grynwald.MdDocs.ApiReference.Model
{
    /// <summary>
    /// Identifies a field.
    /// </summary>
    public sealed class FieldId : TypeMemberId, IEquatable<FieldId>
    {
        /// <summary>
        /// Initializes a new instance of <see cref="FieldId"/>.
        /// </summary>
        /// <param name="definingType">The type which defines the field.</param>
        /// <param name="name">The name of the field.</param>
        public FieldId(TypeId definingType, string name) : base(definingType, name)
        { }


        /// <inheritdoc />
        public override bool Equals(object obj) => Equals(obj as FieldId);

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                var hash = DefiningType.GetHashCode() * 397;
                hash ^= StringComparer.Ordinal.GetHashCode(Name);
                return hash;
            }
        }

        /// <summary>
        /// Determines whether this instance of <see cref="FieldId"/> refers to the same field as <paramref name="other"/>.
        /// </summary>
        public bool Equals(FieldId other)
        {
            if (ReferenceEquals(this, other))
                return true;

            return other != null &&
                   DefiningType.Equals(other.DefiningType) &&
                   StringComparer.Ordinal.Equals(Name, other.Name);
        }
    }
}
