using System.Linq;

namespace Grynwald.MdDocs.ApiReference.Model.Test
{
    public class OperatorOverloadDocumentationTest : OverloadDocumentationTest
    {
        protected override OverloadDocumentation GetOverloadDocumentationInstance(TypeDocumentation typeDocumentation)
        {
            return typeDocumentation.Operators.First().Overloads.First();
        }
    }
}
