using System.Linq;

namespace Grynwald.MdDocs.ApiReference.Model.Test
{
    public class PropertyDocumentationTest : MemberDocumentationTest
    {
        protected override MemberDocumentation GetMemberDocumentationInstance(TypeDocumentation typeDocumentation)
        {
            return typeDocumentation.Properties.First();
        }
    }
}
