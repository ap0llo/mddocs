using System.Linq;
using Grynwald.MdDocs.ApiReference.Model;

namespace Grynwald.MdDocs.ApiReference.Test.Model
{
    public class OperatorOverloadDocumentationTest : OverloadDocumentationTest
    {
        protected override OverloadDocumentation GetOverloadDocumentationInstance(TypeDocumentation typeDocumentation)
        {
            return typeDocumentation.Operators.First().Overloads.First();
        }
    }
}
