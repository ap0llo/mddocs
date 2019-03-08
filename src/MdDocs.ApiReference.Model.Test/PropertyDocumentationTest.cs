using System.Linq;
using Grynwald.MdDocs.ApiReference.Model;
using Grynwald.MdDocs.ApiReference.Test.TestData;

namespace Grynwald.MdDocs.ApiReference.Test.Model
{
    public class PropertyDocumentationTest : MemberDocumentationTest
    {
        protected override MemberDocumentation GetMemberDocumentationInstance()
        {
            return GetTypeDocumentation(typeof(TestClass_Properties)).Properties.First();
        }       
    }
}
