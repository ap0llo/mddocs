using MdDoc.Test.TestData;
using System.Linq;

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
