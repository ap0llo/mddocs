using MdDoc.Test.TestData;
using Mono.Cecil;
using Xunit;

namespace MdDoc.Test.Model
{
    public abstract class MemberDocumentationTest : TestBase
    {

        [Fact]
        public void TryGetDocumentation_returns_null_for_an_undocumented_type()
        {
            // ARRANGE
            var typeReference = GetTypeReference(typeof(TestClass_InternalType));
            var sut = GetMemberDocumentationInstance();

            // ACT
            var documentation = sut.TryGetDocumentation(typeReference);

            // ASSERT
            Assert.Null(documentation);
        }


        [Fact]
        public void TryGetDocumenation_returns_expected_documentation_item_for_an_documented_type()
        {
            // ARRANGE
            var typeReference = GetTypeReference(typeof(TestClass_Type));
            var sut = GetMemberDocumentationInstance();

            // ACT
            var documentation = sut.TryGetDocumentation(typeReference);

            // ASSERT
            Assert.NotNull(documentation);
            Assert.Equal(typeReference, (TypeReference)documentation.Definition);
        }



        protected abstract MdDoc.Model.MemberDocumentation GetMemberDocumentationInstance();

    }
}
