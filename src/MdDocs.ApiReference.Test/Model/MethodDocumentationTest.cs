using System.Linq;
using Grynwald.MdDocs.ApiReference.Model;
using Grynwald.MdDocs.ApiReference.Model.XmlDocs;
using Grynwald.MdDocs.ApiReference.Test.TestData;
using Xunit;

namespace Grynwald.MdDocs.ApiReference.Test.Model
{
    public class MethodDocumentationTest : MemberDocumentationTest
    {
        [Fact]
        public void Name_returns_the_expected_value_for_generic_overloads()
        {
            var methodName = "TestMethod1";
            
            // get methods, use StartsWith() as generic overloads are suffixed with the numer
            // of type parameters
            var methodOverloads = GetTypeDefinition(typeof(TestClass_MethodOverloads))
                .Methods
                .Where(x => x.Name.StartsWith(methodName));

            var sut = new MethodDocumentation(GetTypeDocumentation(typeof(TestClass_MethodOverloads)), methodOverloads, NullXmlDocsProvider.Instance);

            Assert.Equal(methodName, sut.Name);
        }


        protected override MemberDocumentation GetMemberDocumentationInstance()
        {
            return GetTypeDocumentation(typeof(TestClass_Methods)).Methods.First();
        }
    }
}
