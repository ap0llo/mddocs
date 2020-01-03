using System;

namespace Grynwald.MdDocs.ApiReference.Model.Test
{
    public class ConstructorDocumentationTest : MemberDocumentationTest, IDisposable
    {
        protected override MemberDocumentation GetMemberDocumentationInstance(TypeDocumentation typeDocumentation)
        {
            return typeDocumentation.Constructors!;
        }
    }
}
