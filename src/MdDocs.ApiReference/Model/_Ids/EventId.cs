using System;

namespace Grynwald.MdDocs.ApiReference.Model
{
    public sealed class EventId : TypeMemberId, IEquatable<EventId>
    {
        public EventId(TypeId definingType, string name) : base(definingType, name)
        { }


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
