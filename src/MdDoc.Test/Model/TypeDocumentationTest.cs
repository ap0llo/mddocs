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
        public void Fields_returns_expected_fields_for_classes()
        {
            // ARRANGE / ACT
            var sut = GetTypeDocumentation(typeof(TestClass_Fields));

            // ASSERT
            Assert.Single(sut.Fields);
            Assert.Contains(sut.Fields, field => field.Definition.Name == "Field1");
        }

        [Fact]
        public void Fields_returns_expected_fields_for_enums()
        {
            // ARRANGE / ACT
            var sut = GetTypeDocumentation(typeof(TestEnum_Type));

            // ASSERT
            Assert.Equal(2, sut.Fields.Count);
            Assert.Contains(sut.Fields, field => field.Definition.Name == "Value1");
            Assert.Contains(sut.Fields, field => field.Definition.Name == "Value2");
        }

        [Fact]
        public void Events_returns_expected_events_for_classes()
        {
            // ARRANGE / ACT
            var sut = GetTypeDocumentation(typeof(TestClass_Events));

            // ASSERT
            Assert.Equal(3, sut.Events.Count);
            Assert.Contains(sut.Events, e => e.Definition.Name == "Event1");
            Assert.Contains(sut.Events, e => e.Definition.Name == "Event2");
            Assert.Contains(sut.Events, e => e.Definition.Name == "Event3");
        }

        [Fact]
        public void Events_returns_expected_events_for_interfaces()
        {
            // ARRANGE / ACT
            var sut = GetTypeDocumentation(typeof(TestInterface_Events));

            // ASSERT
            Assert.Equal(3, sut.Events.Count);
            Assert.Contains(sut.Events, e => e.Definition.Name == "Event1");
            Assert.Contains(sut.Events, e => e.Definition.Name == "Event2");
            Assert.Contains(sut.Events, e => e.Definition.Name == "Event3");
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

        [Fact]
        public void Constructors_returns_the_expected_constructor_overloads_for_classes_01()
        {
            var sut = GetTypeDocumentation(typeof(TestClass_Methods));

            Assert.NotNull(sut.Constructors);
            Assert.Single(sut.Constructors.Overloads);
            Assert.Equal(".ctor", sut.Constructors.Name);
        }

        [Fact]
        public void Constructors_returns_the_expected_constructor_overloads_for_classes_02()
        {
            var sut = GetTypeDocumentation(typeof(TestClass_Constructors));

            Assert.NotNull(sut.Constructors);
            Assert.Equal(2, sut.Constructors.Overloads.Count);
            Assert.Equal(".ctor", sut.Constructors.Name);
        }

        [Fact]
        public void Constructors_returns_the_expected_constructor_overloads_for_structs_01()
        {
            var sut = GetTypeDocumentation(typeof(TestStruct_Type));
            Assert.Null(sut.Constructors);            
        }

        [Fact]
        public void Constructors_returns_the_expected_constructor_overloads_for_structs_02()
        {
            var sut = GetTypeDocumentation(typeof(TestStruct_Constructors));

            Assert.NotNull(sut.Constructors);
            Assert.Equal(2, sut.Constructors.Overloads.Count);
            Assert.Equal(".ctor", sut.Constructors.Name);
        }

        [Fact]
        public void Constructors_returns_null_for_interfaces()
        {
            var sut = GetTypeDocumentation(typeof(TestInterface_Type));
            Assert.Null(sut.Constructors);            
        }

        [Fact]
        public void Constructors_returns_null_for_enums()
        {
            var sut = GetTypeDocumentation(typeof(TestEnum_Type));
            Assert.Null(sut.Constructors);
        }
    }
}
