using MdDoc.Model;
using MdDoc.Test.TestData;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace MdDoc.Test.Model
{
    public abstract class OverloadDocumentationTest : TestBase
    {
        [Fact]
        public void TryGetDocumentation_returns_null_for_an_undocumented_type()
        {
            // ARRANGE
            var typeId = GetTypeId(typeof(TestClass_InternalType));
            var sut = GetOverloadDocumentationInstance();

            // ACT
            var documentation = sut.TryGetDocumentation(typeId);

            // ASSERT
            Assert.Null(documentation);
        }


        [Fact]
        public void TryGetDocumenation_returns_expected_documentation_item_for_an_documented_type()
        {
            // ARRANGE
            var typeId = GetTypeId(typeof(TestClass_Type));
            var sut = GetOverloadDocumentationInstance();

            // ACT
            var documentation = sut.TryGetDocumentation(typeId);

            // ASSERT
            Assert.NotNull(documentation);
            Assert.IsType<TypeDocumentation>(documentation);
            Assert.Equal(typeId, ((TypeDocumentation)documentation).TypeId);
        }


        protected abstract MdDoc.Model.OverloadDocumentation GetOverloadDocumentationInstance();
    }
}
