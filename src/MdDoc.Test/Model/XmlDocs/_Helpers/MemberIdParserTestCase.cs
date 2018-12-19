using MdDoc.Model.XmlDocs;
using System;
using Xunit.Abstractions;

namespace MdDoc.Test.Model.XmlDocs
{
    public class MemberIdParserTestCase : IXunitSerializable
    {
        public string Input { get; private set; }

        public MemberId ExpectedMemberId { get; private set; }


        // parameterless constructor required by xunit
        public MemberIdParserTestCase()
        { }

        public MemberIdParserTestCase(string input, MemberId expectedMemberId)
        {
            Input = input;
            ExpectedMemberId = expectedMemberId;
        }


        public void Deserialize(IXunitSerializationInfo info)
        {
            Input = info.GetValue<string>(nameof(Input));
            var type = info.GetValue<string>("type");

            switch (type)
            {
                case nameof(TypeId):
                    ExpectedMemberId = info.GetValue<XunitSerializableTypeId>(nameof(ExpectedMemberId));
                    break;

                case nameof(MethodId):
                    ExpectedMemberId = info.GetValue<XunitSerializableMethodId>(nameof(ExpectedMemberId));
                    break;

                case nameof(PropertyId):
                    ExpectedMemberId = info.GetValue<XunitSerializablePropertyId>(nameof(ExpectedMemberId));
                    break;

                case nameof(FieldId):
                    ExpectedMemberId = info.GetValue<XunitSerializableFieldId>(nameof(ExpectedMemberId));
                    break;

                case nameof(EventId):
                    ExpectedMemberId = info.GetValue<XunitSerializableEventId>(nameof(ExpectedMemberId));
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        public void Serialize(IXunitSerializationInfo info)
        {
            info.AddValue(nameof(Input), Input);

            switch (ExpectedMemberId)
            {
                case TypeId typeId:
                    info.AddValue("type", nameof(TypeId));
                    info.AddValue(nameof(ExpectedMemberId), new XunitSerializableTypeId(typeId));
                    break;

                case MethodId methodId:
                    info.AddValue("type", nameof(MethodId));
                    info.AddValue(nameof(ExpectedMemberId), new XunitSerializableMethodId(methodId));
                    break;

                case PropertyId propertyId:
                    info.AddValue("type", nameof(PropertyId));
                    info.AddValue(nameof(ExpectedMemberId), new XunitSerializablePropertyId(propertyId));
                    break;

                case FieldId fieldId:
                    info.AddValue("type", nameof(FieldId));
                    info.AddValue(nameof(ExpectedMemberId), new XunitSerializableFieldId(fieldId));
                    break;

                case EventId eventId:
                    info.AddValue("type", nameof(EventId));
                    info.AddValue(nameof(ExpectedMemberId), new XunitSerializableEventId(eventId));
                    break;
                    
                default:
                    throw new NotImplementedException();
            }            
        }

        public override string ToString() => Input;
    }
}
