using Grynwald.MdDocs.ApiReference.Model;
using Grynwald.MdDocs.ApiReference.Test.TestData;

namespace Grynwald.MdDocs.ApiReference.Test.Model
{
    public class ConstructorDocumentationTest : MemberDocumentationTest
    {
        protected override MemberDocumentation GetMemberDocumentationInstance()
        {
            return GetTypeDocumentation(typeof(TestClass_Constructors)).Constructors!;
        }
    }
}
