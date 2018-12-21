using System;

namespace MdDoc.Model
{
    public sealed class FieldId : TypeMemberId, IEquatable<FieldId>
    {
        public string Name { get; }


        public FieldId(TypeId definingType, string name) : base(definingType)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentException("Value must not be empty", nameof(name));

            Name = name;
        }

        public override bool Equals(object obj) => Equals(obj as FieldId);

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = DefiningType.GetHashCode() * 397;
                hash ^= StringComparer.Ordinal.GetHashCode(Name);
                return hash;
            }
        }

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
