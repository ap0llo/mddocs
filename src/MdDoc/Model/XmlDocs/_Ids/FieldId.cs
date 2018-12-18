using System;

namespace MdDoc.Model.XmlDocs
{
    public sealed class FieldId : MemberId, IEquatable<FieldId>
    {
        public TypeId DefiningType { get; }

        public string Name { get; }


        public FieldId(TypeId definingType, string name)
        {
            if (definingType == null)
                throw new ArgumentNullException(nameof(definingType));

            if (String.IsNullOrEmpty(name))
                throw new ArgumentException("Value must not be empty", nameof(name));

            DefiningType = definingType;
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
