using Mono.Cecil;

namespace Grynwald.MdDocs.ApiReference.Model
{
    /// <summary>
    /// Extension methods for <see cref="EventReference"/>.
    /// </summary>
    internal static class EventReferenceExtensions
    {
        /// <summary>
        /// Gets the <see cref="MemberId"/> for the event.
        /// </summary>
        public static MemberId ToMemberId(this EventReference eventReference)
        {
            return new EventId(
                eventReference.DeclaringType.ToTypeId(),
                eventReference.Name
            );
        }
    }
}
