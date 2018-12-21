using System.Linq;
using MdDoc.Test.TestData;

namespace MdDoc.Test.Model
{
    public class MethodOverloadDocumentationTest : OverloadDocumentationTest
    {
        protected override MdDoc.Model.OverloadDocumentation GetOverloadDocumentationInstance()
        {
            return GetTypeDocumentation(typeof(TestClass_Methods)).Methods.First().Overloads.First();
        }
    }
}
