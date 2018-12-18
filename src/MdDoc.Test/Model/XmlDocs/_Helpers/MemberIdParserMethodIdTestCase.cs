using MdDoc.Model.XmlDocs;
using Xunit.Abstractions;

namespace MdDoc.Test.Model.XmlDocs
{
    public class MemberIdParserMethodIdTestCase : IXunitSerializable
    {
        public string Input { get; private set; }

        public MethodId ExpectedMethodId { get; private set; }


        // parameterless constructor required by xunit
        public MemberIdParserMethodIdTestCase()
        { }

        public MemberIdParserMethodIdTestCase(string input, MethodId expectedMethodId)
        {
            Input = input;
            ExpectedMethodId = expectedMethodId;
        }


        public void Deserialize(IXunitSerializationInfo info)
        {
            Input = info.GetValue<string>(nameof(Input));
            ExpectedMethodId = info.GetValue<XunitSerializableMethodId>(nameof(ExpectedMethodId));
        }

        public void Serialize(IXunitSerializationInfo info)
        {
            info.AddValue(nameof(Input), Input);
            info.AddValue(nameof(ExpectedMethodId), new XunitSerializableMethodId(ExpectedMethodId));
        }

        public override string ToString() => Input;
    }
}
