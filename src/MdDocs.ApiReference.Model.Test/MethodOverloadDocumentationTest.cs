using System.Linq;
using Grynwald.MdDocs.ApiReference.Model;
using Grynwald.MdDocs.ApiReference.Test.TestData;
using Xunit;

namespace Grynwald.MdDocs.ApiReference.Test.Model
{
    public class MethodOverloadDocumentationTest : OverloadDocumentationTest
    {
        protected override OverloadDocumentation GetOverloadDocumentationInstance()
        {
            return GetTypeDocumentation(typeof(TestClass_Methods)).Methods.First().Overloads.First();
        }


        [Fact]
        public void TypeParametes_is_empty_for_non_generic_method_01()
        {
            // ARRANGE / ACT
            var sut = GetTypeDocumentation(typeof(TestClass_GenericType<>))
                .Methods
                .Single(m => m.Name == "TestMethod1")
                .Overloads
                .Single();

            // ASSERT
            Assert.NotNull(sut.TypeParameters);
            Assert.Empty(sut.TypeParameters);
        }

        [Fact]
        public void TypeParametes_is_empty_for_non_generic_method_02()
        {
            // ARRANGE / ACT
            var sut = GetTypeDocumentation(typeof(TestClass_Methods))
                .Methods
                .Single(m => m.Name == "TestMethod1")
                .Overloads
                .Single();

            // ASSERT
            Assert.NotNull(sut.TypeParameters);
            Assert.Empty(sut.TypeParameters);
        }


        [Fact]
        public void TypeParametes_returns_expected_parameters_for_generic_methods()
        {
            // ARRANGE / ACT
            var sut = GetTypeDocumentation(typeof(TestClass_Methods))
                .Methods
                .Single(m => m.Name == "TestMethod8")
                .Overloads
                .Single();

            // ASSERT
            Assert.NotNull(sut.TypeParameters);
            Assert.Equal(2, sut.TypeParameters.Count);
            Assert.Contains(sut.TypeParameters, x => x.Name == "T1");
            Assert.Contains(sut.TypeParameters, x => x.Name == "T2");
        }
    }
}
