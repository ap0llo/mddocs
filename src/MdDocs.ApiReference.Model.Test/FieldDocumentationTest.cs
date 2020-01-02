using System.Linq;

namespace Grynwald.MdDocs.ApiReference.Model.Test
{
    public class FieldDocumentationTest : MemberDocumentationTest
    {
        protected override MemberDocumentation GetMemberDocumentationInstance(TypeDocumentation typeDocumentation)
        {
            return typeDocumentation.Fields.Single();
        }
    }
}
