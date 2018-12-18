using MdDoc.Model.XmlDocs;
using Xunit.Abstractions;

namespace MdDoc.Test.Model.XmlDocs
{
    public class MemberIdParserEventIdTestCase : IXunitSerializable
    {
        public string Input { get; private set; }

        public EventId ExpectedEventId { get; private set; }


        // parameterless constructor required by xunit
        public MemberIdParserEventIdTestCase()
        { } 

        public MemberIdParserEventIdTestCase(string input, EventId expectedEventId)
        {
            Input = input;
            ExpectedEventId = expectedEventId;
        }


        public void Deserialize(IXunitSerializationInfo info)
        {
            Input = info.GetValue<string>(nameof(Input));
            ExpectedEventId = info.GetValue<XunitSerializableEventId>(nameof(ExpectedEventId));
        }

        public void Serialize(IXunitSerializationInfo info)
        {
            info.AddValue(nameof(Input), Input);
            info.AddValue(nameof(ExpectedEventId), new XunitSerializableEventId(ExpectedEventId));
        }

        public override string ToString() => Input;
    }

}
