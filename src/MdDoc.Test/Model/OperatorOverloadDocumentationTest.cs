using MdDoc.Test.TestData;
using System.Linq;

namespace MdDoc.Test.Model
{
    public class OperatorOverloadDocumentationTest : OverloadDocumentationTest
    {
        protected override MdDoc.Model.OverloadDocumentation GetOverloadDocumentationInstance()
        {
            return GetTypeDocumentation(typeof(TestClass_Operators)).Operators.First().Overloads.First();
        }

    }
}
