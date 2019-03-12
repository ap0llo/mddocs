using System;

namespace Grynwald.MdDocs.ApiReference.Model
{
    /// <summary>
    /// Identifies an event.
    /// </summary>
    public sealed class EventId : TypeMemberId, IEquatable<EventId>
    {
        /// <summary>
        /// Initializes a new instance of <see cref="EventId"/>.
        /// </summary>
        /// <param name="definingType">The type which defines the event.</param>
        /// <param name="name">The name of the event.</param>
        public EventId(TypeId definingType, string name) : base(definingType, name)
        { }


        /// <inheritdoc />
        public override bool Equals(object obj) => Equals(obj as EventId);

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
        /// Determines whether this instance of <see cref="EventId"/> refers to the same event as <paramref name="other"/>.
        /// </summary>
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
