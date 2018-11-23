using MdDoc.Test.TestData;
using System.Linq;

namespace MdDoc.Test.Model
{
    public class EventDocumentationTest : MemberDocumentationTest
    {
        protected override MdDoc.Model.MemberDocumentation GetMemberDocumentationInstance()
        {
            return GetTypeDocumentation(typeof(TestClass_Events)).Events.First();
        }
    }
}
