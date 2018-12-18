using MdDoc.Model.XmlDocs;
using Xunit.Abstractions;

namespace MdDoc.Test.Model.XmlDocs
{
    public class MemberIdParserTypeIdTestCase : IXunitSerializable
    {
        public string Input { get; private set; }

        public TypeId ExpectedTypeId { get; private set; }


        // parameterless constructor required by xunit
        public MemberIdParserTypeIdTestCase()
        { }

        public MemberIdParserTypeIdTestCase(string input, TypeId expectedMethodId)
        {
            Input = input;
            ExpectedTypeId = expectedMethodId;
        }


        public void Deserialize(IXunitSerializationInfo info)
        {
            Input = info.GetValue<string>(nameof(Input));
            ExpectedTypeId = info.GetValue<XunitSerializableTypeId>(nameof(ExpectedTypeId));
        }

        public void Serialize(IXunitSerializationInfo info)
        {
            info.AddValue(nameof(Input), Input);
            info.AddValue(nameof(ExpectedTypeId), new XunitSerializableTypeId(ExpectedTypeId));
        }

        public override string ToString() => Input;
    }
}
