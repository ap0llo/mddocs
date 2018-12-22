using System.Linq;
using MdDoc.Model;
using MdDoc.Test.TestData;

namespace MdDoc.Test.Model
{
    public class PropertyDocumentationTest : MemberDocumentationTest
    {
        protected override MemberDocumentation GetMemberDocumentationInstance()
        {
            return GetTypeDocumentation(typeof(TestClass_Properties)).Properties.First();
        }       
    }
}
