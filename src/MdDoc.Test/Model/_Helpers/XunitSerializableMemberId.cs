using System;
using Xunit.Abstractions;
using MdDoc.Model;

namespace MdDoc.Test.Model.XmlDocs
{
    public class XunitSerializableMemberId : IXunitSerializable
    {
        public MemberId MemberId { get; private set; }


        // parameterless constructor required by xunit
        public XunitSerializableMemberId()
        { }

        public XunitSerializableMemberId(MemberId memberId)
        {            
            MemberId = memberId;
        }


        public void Deserialize(IXunitSerializationInfo info)
        { 
            var type = info.GetValue<string>("type");

            switch (type)
            {
                case nameof(TypeId):
                    MemberId = info.GetValue<XunitSerializableTypeId>(nameof(MemberId));
                    break;

                case nameof(MethodId):
                    MemberId = info.GetValue<XunitSerializableMethodId>(nameof(MemberId));
                    break;

                case nameof(PropertyId):
                    MemberId = info.GetValue<XunitSerializablePropertyId>(nameof(MemberId));
                    break;

                case nameof(FieldId):
                    MemberId = info.GetValue<XunitSerializableFieldId>(nameof(MemberId));
                    break;

                case nameof(EventId):
                    MemberId = info.GetValue<XunitSerializableEventId>(nameof(MemberId));
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        public void Serialize(IXunitSerializationInfo info)
        {
            switch (MemberId)
            {
                case TypeId typeId:
                    info.AddValue("type", nameof(TypeId));
                    info.AddValue(nameof(MemberId), new XunitSerializableTypeId(typeId));
                    break;

                case MethodId methodId:
                    info.AddValue("type", nameof(MethodId));
                    info.AddValue(nameof(MemberId), new XunitSerializableMethodId(methodId));
                    break;

                case PropertyId propertyId:
                    info.AddValue("type", nameof(PropertyId));
                    info.AddValue(nameof(MemberId), new XunitSerializablePropertyId(propertyId));
                    break;

                case FieldId fieldId:
                    info.AddValue("type", nameof(FieldId));
                    info.AddValue(nameof(MemberId), new XunitSerializableFieldId(fieldId));
                    break;

                case EventId eventId:
                    info.AddValue("type", nameof(EventId));
                    info.AddValue(nameof(MemberId), new XunitSerializableEventId(eventId));
                    break;
                    
                default:
                    throw new NotImplementedException();
            }            
        }

        public static implicit operator MemberId(XunitSerializableMemberId serializable) => serializable?.MemberId;
    }
}
