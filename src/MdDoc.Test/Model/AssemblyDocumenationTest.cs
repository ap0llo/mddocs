using MdDoc.Test.TestData;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace MdDoc.Test.Model
{
    public class AssemblyDocumenationTest : TestBase
    {
        [Fact]
        public void TryGetDocumentation_returns_null_for_an_undocumented_type()
        {
            // ARRANGE
            var typeReference = GetTypeName(typeof(TestClass_InternalType));
            var sut = m_AssemblyDocumentation;

            // ACT
            var documentation = sut.TryGetDocumentation(typeReference);

            // ASSERT
            Assert.Null(documentation);            
        }


        [Fact]
        public void TryGetDocumenation_returns_expected_documentation_item_for_an_documented_type()
        {
            // ARRANGE
            var typeName = GetTypeName(typeof(TestClass_Type));
            var sut = m_AssemblyDocumentation;

            // ACT
            var documentation = sut.TryGetDocumentation(typeName);

            // ASSERT
            Assert.NotNull(documentation);
            Assert.Equal(typeName, documentation.Name);
        }

    }
}
