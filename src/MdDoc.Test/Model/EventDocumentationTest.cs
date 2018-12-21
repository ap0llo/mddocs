using System.Linq;
using MdDoc.Model;
using MdDoc.Test.TestData;

namespace MdDoc.Test.Model
{
    public class EventDocumentationTest : MemberDocumentationTest
    {
        protected override MemberDocumentation GetMemberDocumentationInstance()
        {
            return GetTypeDocumentation(typeof(TestClass_Events)).Events.First();
        }
    }
}
