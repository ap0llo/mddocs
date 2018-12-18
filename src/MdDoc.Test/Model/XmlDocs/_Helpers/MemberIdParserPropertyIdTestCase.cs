using MdDoc.Model.XmlDocs;
using Xunit.Abstractions;

namespace MdDoc.Test.Model.XmlDocs
{
    public class MemberIdParserPropertyIdTestCase : IXunitSerializable
    {
        public string Input { get; private set; }

        public PropertyId ExpectedPropertyId { get; private set; }


        // parameterless constructor required by xunit
        public MemberIdParserPropertyIdTestCase()
        { }

        public MemberIdParserPropertyIdTestCase(string input, PropertyId expectedPropertyId)
        {
            Input = input;
            ExpectedPropertyId = expectedPropertyId;
        }


        public void Deserialize(IXunitSerializationInfo info)
        {
            Input = info.GetValue<string>(nameof(Input));
            ExpectedPropertyId = info.GetValue<XunitSerializablePropertyId>(nameof(ExpectedPropertyId));
        }

        public void Serialize(IXunitSerializationInfo info)
        {
            info.AddValue(nameof(Input), Input);
            info.AddValue(nameof(ExpectedPropertyId), new XunitSerializablePropertyId(ExpectedPropertyId));
        }

        public override string ToString() => Input;
    }
}
