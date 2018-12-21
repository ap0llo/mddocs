using System;

namespace MdDoc.Model
{
    public sealed class EventId : TypeMemberId, IEquatable<EventId>
    {
        public string Name { get; }


        public EventId(TypeId definingType, string name) : base(definingType)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentException("Value must not be empty", nameof(name));

            Name = name;
        }


        public override bool Equals(object obj) => Equals(obj as EventId);

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = DefiningType.GetHashCode() * 397;
                hash ^= StringComparer.Ordinal.GetHashCode(Name);
                return hash;
            }            
        }

        public bool Equals(EventId other)
        {
            if (ReferenceEquals(this, other))
                return true;

            return other != null &&
                DefiningType.Equals(other.DefiningType) &&
                StringComparer.Ordinal.Equals(Name, other.Name);
        }
    }
}
