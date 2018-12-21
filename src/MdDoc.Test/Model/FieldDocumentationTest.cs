using System.Linq;
using MdDoc.Model;
using MdDoc.Test.TestData;

namespace MdDoc.Test.Model
{
    public class FieldDocumentationTest : MemberDocumentationTest
    {
        protected override MemberDocumentation GetMemberDocumentationInstance()
        {
            return GetTypeDocumentation(typeof(TestClass_Fields)).Fields.First();
        }
    }
}
