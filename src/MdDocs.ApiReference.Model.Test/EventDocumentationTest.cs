using System.Linq;

namespace Grynwald.MdDocs.ApiReference.Model.Test
{
    public class EventDocumentationTest : MemberDocumentationTest
    {
        protected override MemberDocumentation GetMemberDocumentationInstance(TypeDocumentation typeDocumentation)
        {
            return typeDocumentation.Events.First();
        }
    }
}
