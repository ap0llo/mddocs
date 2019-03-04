using System.Linq;
using Grynwald.MdDocs.ApiReference.Model;
using Grynwald.MdDocs.ApiReference.Test.TestData;

namespace Grynwald.MdDocs.ApiReference.Test.Model
{
    public class OperatorOverloadDocumentationTest : OverloadDocumentationTest
    {
        protected override OverloadDocumentation GetOverloadDocumentationInstance()
        {
            return GetTypeDocumentation(typeof(TestClass_Operators)).Operators.First().Overloads.First();
        }
    }
}
