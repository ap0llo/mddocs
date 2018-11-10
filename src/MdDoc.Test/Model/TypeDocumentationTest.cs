using MdDoc.Model;
using MdDoc.Test.TestData;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace MdDoc.Test.Model
{
    public class TypeDocumentationTest : TestBase
    {
        private readonly DocumentationContext m_Context;

        public TypeDocumentationTest()
        {
            m_Context = new DocumentationContext(m_Module, NullXmlDocProvider.Instance);
        }


        [Theory]
        [InlineData(typeof(TestClass_Type), TypeKind.Class)]
        [InlineData(typeof(TestStruct_Type), TypeKind.Struct)]
        [InlineData(typeof(TestInterface_Type), TypeKind.Interface)]
        [InlineData(typeof(TestEnum_Type), TypeKind.Enum)]
        public void Kind_returns_the_expected_value(Type type, TypeKind expectedKind)
        {
            // ARRANGE
            var typeDefinition = GetTypeDefinition(type);
            var sut = new TypeDocumentation(m_Context, typeDefinition);

            // ACT
            var actualKind = sut.Kind;

            // ASSERT
            Assert.Equal(expectedKind, actualKind);
        }


    }
}
