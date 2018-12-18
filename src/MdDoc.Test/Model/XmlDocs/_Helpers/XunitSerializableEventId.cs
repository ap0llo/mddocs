using MdDoc.Model.XmlDocs;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit.Abstractions;

namespace MdDoc.Test.Model.XmlDocs
{
    class XunitSerializableEventId : IXunitSerializable
    {
        public EventId EventId { get; private set; }


        // parameterless constructor required by xunit
        public XunitSerializableEventId()
        { }

        public XunitSerializableEventId(EventId eventId)
        {
            EventId = eventId ?? throw new ArgumentNullException(nameof(EventId));
        }


        public void Deserialize(IXunitSerializationInfo info)
        {
            EventId = new EventId(
                definingType: info.GetValue<XunitSerializableTypeId>(nameof(EventId.DefiningType)),
                name: info.GetValue<string>(nameof(EventId.Name))
            );
        }

        public void Serialize(IXunitSerializationInfo info)
        {
            info.AddValue(nameof(EventId.DefiningType), new XunitSerializableTypeId(EventId.DefiningType));
            info.AddValue(nameof(EventId.Name), EventId.Name);            
        }

        public static implicit operator EventId(XunitSerializableEventId serializable) => serializable?.EventId;
    }
}
