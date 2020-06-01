using System.Linq;
using Grynwald.MdDocs.ApiReference.Model;

namespace Grynwald.MdDocs.ApiReference.Test.Model
{
    public class PropertyDocumentationTest : MemberDocumentationTest
    {
        protected override MemberDocumentation GetMemberDocumentationInstance(TypeDocumentation typeDocumentation)
        {
            return typeDocumentation.Properties.First();
        }
    }
}
