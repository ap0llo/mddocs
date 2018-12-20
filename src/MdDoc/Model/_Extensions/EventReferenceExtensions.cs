using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Text;

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
