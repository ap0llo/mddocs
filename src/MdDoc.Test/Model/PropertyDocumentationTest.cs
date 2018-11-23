using MdDoc.Test.TestData;
using System.Linq;

namespace MdDoc.Test.Model
{
    public class PropertyDocumentationTest : MemberDocumentationTest
    {
        protected override MdDoc.Model.MemberDocumentation GetMemberDocumentationInstance()
        {
            return GetTypeDocumentation(typeof(TestClass_Properties)).Properties.First();
        }
    }
}
