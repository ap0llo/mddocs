using System;
using Grynwald.MdDocs.ApiReference.Model;

namespace Grynwald.MdDocs.ApiReference.Test.Model
{
    public class ConstructorDocumentationTest : MemberDocumentationTest, IDisposable
    {
        protected override MemberDocumentation GetMemberDocumentationInstance(TypeDocumentation typeDocumentation)
        {
            return typeDocumentation.Constructors!;
        }
    }
}
