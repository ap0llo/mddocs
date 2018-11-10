using MdDoc.Model;
using MdDoc.Test.TestData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace MdDoc.Test.Model
{
    public class TypeDocumentationTest : TestBase
    {

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

        [Fact]
        public void Methods_returns_the_expected_methods()
        {
            // ARRANGE / ACT
            var sut = GetTypeDocumentation(typeof(TestClass_Methods));
            
            // ASSERT
            Assert.Equal(9, sut.Methods.Count);
            Assert.All(sut.Methods, method => Assert.Single(method.Overloads));
            Assert.Contains(sut.Methods, m => m.Name == "TestMethod1");
            Assert.Contains(sut.Methods, m => m.Name == "TestMethod2");
            Assert.Contains(sut.Methods, m => m.Name == "TestMethod3");
            Assert.Contains(sut.Methods, m => m.Name == "TestMethod4");
            Assert.Contains(sut.Methods, m => m.Name == "TestMethod5");
            Assert.Contains(sut.Methods, m => m.Name == "TestMethod6");
            Assert.Contains(sut.Methods, m => m.Name == "TestMethod7");
            Assert.Contains(sut.Methods, m => m.Name == "TestMethod8");
            Assert.Contains(sut.Methods, m => m.Name == "TestMethod9");
        }

        [Fact]
        public void Methods_do_not_include_constructors()
        {
            var sut = GetTypeDocumentation(typeof(TestClass_Constructors));
            Assert.NotNull(sut.Methods);            
            Assert.Empty(sut.Methods);            
        }
            
        [Fact]
        public void Methods_include_overloads_with_generic_parameters()
        {
            var sut = GetTypeDocumentation(typeof(TestClass_MethodOverloads));

            Assert.Single(sut.Methods);
            var method = sut.Methods.Single();

            Assert.Equal("TestMethod1", method.Name);
            Assert.Equal(3, method.Overloads.Count);
            Assert.Single(method.Overloads.Where(x => x.HasGenericParameters));
        }
    }
}
