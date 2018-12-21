using Mono.Cecil;

namespace MdDoc.Model
{
    public static class EventReferenceExtensions
    {
        public static MemberId ToMemberId(this EventReference eventReference)
        {
            return new EventId(
                eventReference.DeclaringType.ToTypeId(),
                eventReference.Name
            );
        }
    }
}
