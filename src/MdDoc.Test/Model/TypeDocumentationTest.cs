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

        private TypeDocumentation GetTypeDocumentation(Type type)
        {
            var typeDefinition = GetTypeDefinition(type);
            var sut = new TypeDocumentation(m_Context, typeDefinition);
            return sut;
        }


        [Theory]
        [InlineData(typeof(TestClass_Type), TypeKind.Class)]
        [InlineData(typeof(TestStruct_Type), TypeKind.Struct)]
        [InlineData(typeof(TestInterface_Type), TypeKind.Interface)]
        [InlineData(typeof(TestEnum_Type), TypeKind.Enum)]
        public void Kind_returns_the_expected_value(Type type, TypeKind expectedKind)
        {
            // ARRANGE
            var sut = GetTypeDocumentation(type);

            // ACT
            var actualKind = sut.Kind;

            // ASSERT
            Assert.Equal(expectedKind, actualKind);
        }

        
        [Fact]
        public void Properties_returns_expected_properties_01()
        {
            // ARRANGE / ACT
            var sut = GetTypeDocumentation(typeof(TestClass_Properties));
            var properties = sut.Properties;

            // ASSERT
            Assert.Equal(4, properties.Count);
            Assert.Contains(properties, prop => prop.Definition.Name == "Property1");
            Assert.Contains(properties, prop => prop.Definition.Name == "Property2");
            Assert.Contains(properties, prop => prop.Definition.Name == "Item");
            Assert.DoesNotContain(properties, prop => prop.Definition.Name == "Property3");
            Assert.DoesNotContain(properties, prop => prop.Definition.Name == "Property4");
        }

        [Fact]
        public void Properties_returns_expected_properties_02()
        {
            // ARRANGE / ACT
            var sut = GetTypeDocumentation(typeof(TestInterface_Properties));
            var properties = sut.Properties;

            // ASSERT
            Assert.Equal(4, properties.Count);
            Assert.Contains(properties, prop => prop.Definition.Name == "Property1");
            Assert.Contains(properties, prop => prop.Definition.Name == "Property2");
            Assert.Contains(properties, prop => prop.Definition.Name == "Item");
        }
    

        [Fact]
        public void Properties_returns_expected_properties_03()
        {
            // ARRANGE / ACT
            var sut = GetTypeDocumentation(typeof(TestStruct_Properties));
            var properties = sut.Properties;

            // ASSERT
            Assert.Equal(4, properties.Count);
            Assert.Contains(properties, prop => prop.Definition.Name == "Property1");
            Assert.Contains(properties, prop => prop.Definition.Name == "Property2");
            Assert.Contains(properties, prop => prop.Definition.Name == "Item");
        }

    }
}
