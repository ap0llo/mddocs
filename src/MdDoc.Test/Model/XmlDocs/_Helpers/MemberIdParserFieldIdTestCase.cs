using MdDoc.Model.XmlDocs;
using Xunit.Abstractions;

namespace MdDoc.Test.Model.XmlDocs
{
    public class MemberIdParserFieldIdTestCase : IXunitSerializable
    {
        public string Input { get; private set; }

        public FieldId ExpectedFieldId { get; private set; }


        // parameterless constructor required by xunit
        public MemberIdParserFieldIdTestCase()
        { }

        public MemberIdParserFieldIdTestCase(string input, FieldId expectedFieldId)
        {
            Input = input;
            ExpectedFieldId = expectedFieldId;
        }


        public void Deserialize(IXunitSerializationInfo info)
        {
            Input = info.GetValue<string>(nameof(Input));
            ExpectedFieldId = info.GetValue<XunitSerializableFieldId>(nameof(ExpectedFieldId));
        }

        public void Serialize(IXunitSerializationInfo info)
        {
            info.AddValue(nameof(Input), Input);
            info.AddValue(nameof(ExpectedFieldId), new XunitSerializableFieldId(ExpectedFieldId));
        }

        public override string ToString() => Input;
    }
}
