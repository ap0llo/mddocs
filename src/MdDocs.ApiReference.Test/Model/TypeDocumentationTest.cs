using System;
using System.Linq;
using Grynwald.MdDocs.ApiReference.Model;
using Grynwald.MdDocs.ApiReference.Test.TestData;
using Xunit;

namespace Grynwald.MdDocs.ApiReference.Test.Model
{
    public class TypeDocumentationTest : TestBase
    {
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
        public void AssemblyName_returns_the_expected_value()
        {
            // ARRANGE / ACT
            var typeDocumentation = GetTypeDocumentation(typeof(TestClass_Type));

            // ASSERT
            Assert.Equal(typeof(TestClass_Type).Assembly.GetName().Name, typeDocumentation.AssemblyName);
        }

        [Fact]
        public void Fields_returns_expected_fields_for_classes()
        {
            // ARRANGE / ACT
            var sut = GetTypeDocumentation(typeof(TestClass_Fields));

            // ASSERT
            Assert.Single(sut.Fields);
            Assert.Contains(sut.Fields, field => field.Name == "Field1");
        }

        [Fact]
        public void Fields_returns_expected_fields_for_enums()
        {
            // ARRANGE / ACT
            var sut = GetTypeDocumentation(typeof(TestEnum_Type));

            // ASSERT
            Assert.Equal(2, sut.Fields.Count);
            Assert.Contains(sut.Fields, field => field.Name == "Value1");
            Assert.Contains(sut.Fields, field => field.Name == "Value2");
        }

        [Fact]
        public void Events_returns_expected_events_for_classes()
        {
            // ARRANGE / ACT
            var sut = GetTypeDocumentation(typeof(TestClass_Events));

            // ASSERT
            Assert.Equal(3, sut.Events.Count);
            Assert.Contains(sut.Events, e => e.Name == "Event1");
            Assert.Contains(sut.Events, e => e.Name == "Event2");
            Assert.Contains(sut.Events, e => e.Name == "Event3");
        }

        [Fact]
        public void Events_returns_expected_events_for_interfaces()
        {
            // ARRANGE / ACT
            var sut = GetTypeDocumentation(typeof(TestInterface_Events));

            // ASSERT
            Assert.Equal(3, sut.Events.Count);
            Assert.Contains(sut.Events, e => e.Name == "Event1");
            Assert.Contains(sut.Events, e => e.Name == "Event2");
            Assert.Contains(sut.Events, e => e.Name == "Event3");
        }

        [Fact]
        public void Events_is_empty_for_enums()
        {
            // ARRANGE / ACT
            var sut = GetTypeDocumentation(typeof(TestEnum_Type));

            // ASSERT
            Assert.NotNull(sut.Events);
            Assert.Empty(sut.Events);            
        }

        [Fact]
        public void Properties_returns_expected_properties_01()
        {
            // ARRANGE / ACT
            var sut = GetTypeDocumentation(typeof(TestClass_Properties));
            var properties = sut.Properties;

            // ASSERT
            Assert.Equal(2, properties.Count);
            Assert.Contains(properties, prop => prop.Name == "Property1");
            Assert.Contains(properties, prop => prop.Name == "Property2");
            Assert.DoesNotContain(properties, prop => prop.Name == "Item");
            Assert.DoesNotContain(properties, prop => prop.Name == "Property3");
            Assert.DoesNotContain(properties, prop => prop.Name == "Property4");
        }
        
        [Fact]
        public void Properties_returns_expected_properties_02()
        {
            // ARRANGE / ACT
            var sut = GetTypeDocumentation(typeof(TestInterface_Properties));
            var properties = sut.Properties;

            // ASSERT
            Assert.Equal(2, properties.Count);
            Assert.Contains(properties, prop => prop.Name == "Property1");
            Assert.Contains(properties, prop => prop.Name == "Property2");
            Assert.DoesNotContain(properties, prop => prop.Name == "Item");
        }    

        [Fact]
        public void Properties_returns_expected_properties_03()
        {
            // ARRANGE / ACT
            var sut = GetTypeDocumentation(typeof(TestStruct_Properties));
            var properties = sut.Properties;

            // ASSERT
            Assert.Equal(2, properties.Count);
            Assert.Contains(properties, prop => prop.Name == "Property1");
            Assert.Contains(properties, prop => prop.Name == "Property2");
            Assert.DoesNotContain(properties, prop => prop.Name == "Item");
        }


        [Fact]
        public void Indexers_returns_expected_properties_01()
        {
            // ARRANGE / ACT
            var sut = GetTypeDocumentation(typeof(TestClass_Properties));
            var indexers = sut.Indexers;

            // ASSERT
            Assert.Equal(1, indexers.Count);
            Assert.Contains(indexers, indexer => indexer.Name == "Item" && indexer.Overloads.Count == 2);            
        }

        [Fact]
        public void Indexers_returns_expected_properties_02()
        {
            // ARRANGE / ACT
            var sut = GetTypeDocumentation(typeof(TestInterface_Properties));
            var indexers = sut.Indexers;

            // ASSERT
            Assert.Equal(1, indexers.Count);
            Assert.Contains(indexers, indexer => indexer.Name == "Item" && indexer.Overloads.Count == 2);
        }

        [Fact]
        public void Indexers_returns_expected_properties_03()
        {
            // ARRANGE / ACT
            var sut = GetTypeDocumentation(typeof(TestStruct_Properties));
            var indexers = sut.Indexers;

            // ASSERT
            Assert.Equal(1, indexers.Count);
            Assert.Contains(indexers, indexer => indexer.Name == "Item" && indexer.Overloads.Count == 2);
        }

        [Fact]
        public void Properties_is_empty_for_enums()
        {
            // ARRANGE / ACT
            var sut = GetTypeDocumentation(typeof(TestEnum_Type));

            // ASSERT
            Assert.NotNull(sut.Properties);
            Assert.Empty(sut.Properties);
        }

        [Fact]
        public void Methods_returns_the_expected_methods()
        {
            // ARRANGE / ACT
            var sut = GetTypeDocumentation(typeof(TestClass_Methods));
            
            // ASSERT
            Assert.Equal(12, sut.Methods.Count);
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
            Assert.Contains(sut.Methods, m => m.Name == "TestMethod10");
            Assert.Contains(sut.Methods, m => m.Name == "TestMethod11");
            Assert.Contains(sut.Methods, m => m.Name == "TestMethod12");
        }

        [Fact]
        public void Methods_do_not_include_constructors()
        {
            // ARRANGE / ACT        
            var sut = GetTypeDocumentation(typeof(TestClass_Constructors));

            // ASSERT
            Assert.NotNull(sut.Methods);            
            Assert.Empty(sut.Methods);            
        }
            
        [Fact]
        public void Methods_include_overloads_with_generic_parameters()
        {
            // ARRANGE / ACT
            var sut = GetTypeDocumentation(typeof(TestClass_MethodOverloads));

            // ASSERT
            Assert.Single(sut.Methods);
            var method = sut.Methods.Single();

            Assert.Equal("TestMethod1", method.Name);
            Assert.Equal(3, method.Overloads.Count);
            Assert.Single(method.Overloads.Where(x => x.Definition.HasGenericParameters));
        }

        [Fact]
        public void Methods_does_not_include_operator_overloads()
        {
            // ARRANGE / ACT
            var sut = GetTypeDocumentation(typeof(TestClass_Operators));

            // ASSERT
            Assert.NotNull(sut.Methods);
            Assert.Empty(sut.Methods);
        }

        [Fact]
        public void Methods_does_not_include_property_getters_and_setters()
        {
            // ARRANGE / ACT
            var sut = GetTypeDocumentation(typeof(TestClass_Properties));

            // ASSERT
            Assert.NotNull(sut.Methods);
            Assert.Empty(sut.Methods);
        }

        [Fact]
        public void Methods_does_not_include_event_accessors()
        {
            // ARRANGE / ACT
            var sut = GetTypeDocumentation(typeof(TestClass_Events));

            // ASSERT
            Assert.NotNull(sut.Methods);
            Assert.Empty(sut.Methods);
        }

        [Fact]
        public void Constructors_returns_the_expected_constructor_overloads_for_classes_01()
        {
            // ARRANGE / ACT
            var sut = GetTypeDocumentation(typeof(TestClass_Methods));

            // ASSERT
            Assert.NotNull(sut.Constructors);
            Assert.Single(sut.Constructors.Overloads);
            Assert.Equal(".ctor", sut.Constructors.Name);
        }

        [Fact]
        public void Constructors_returns_the_expected_constructor_overloads_for_classes_02()
        {
            // ARRANGE / ACT
            var sut = GetTypeDocumentation(typeof(TestClass_Constructors));

            // ASSERT
            Assert.NotNull(sut.Constructors);
            Assert.Equal(2, sut.Constructors.Overloads.Count);
            Assert.Equal(".ctor", sut.Constructors.Name);
        }

        [Fact]
        public void Constructors_returns_the_expected_constructor_overloads_for_structs_01()
        {
            // ARRANGE / ACT
            var sut = GetTypeDocumentation(typeof(TestStruct_Type));

            // ASSERT
            Assert.Null(sut.Constructors);            
        }

        [Fact]
        public void Constructors_returns_the_expected_constructor_overloads_for_structs_02()
        {
            // ARRANGE / ACT
            var sut = GetTypeDocumentation(typeof(TestStruct_Constructors));

            // ASSERT
            Assert.NotNull(sut.Constructors);
            Assert.Equal(2, sut.Constructors.Overloads.Count);
            Assert.Equal(".ctor", sut.Constructors.Name);
        }

        [Fact]
        public void Constructors_returns_null_for_interfaces()
        {
            // ARRANGE / ACT
            var sut = GetTypeDocumentation(typeof(TestInterface_Type));

            // ASSERT
            Assert.Null(sut.Constructors);            
        }

        [Fact]
        public void Constructors_returns_null_for_enums()
        {
            // ARRANGE / ACT
            var sut = GetTypeDocumentation(typeof(TestEnum_Type));

            // ASSERT
            Assert.Null(sut.Constructors);
        }


        [Fact]
        public void Operators_contains_expected_number_of_operator_overloads()
        {
            // ARRANGE / ACT
            var sut = GetTypeDocumentation(typeof(TestClass_Operators));

            // ASSERT
            Assert.NotNull(sut.Operators);
            Assert.Equal(26, sut.Operators.Count);
        }

        [Fact]
        public void Multiple_overoads_of_the_same_operator_are_gropued()
        {
            // ARRANGE / ACT
            var sut = GetTypeDocumentation(typeof(TestClass_MultipleOperatorOverloads));

            // ASSERT
            Assert.NotNull(sut.Operators);
            Assert.Equal(1, sut.Operators.Count);
        }

        [Fact]
        public void InheritanceHierarchy_contains_the_expected_types_01()
        {
            // ARRANGE
            var expectedSequence = new[]
            {
                new SimpleTypeId("System", "Object"),
                new SimpleTypeId("MdDoc.Test.TestData", "TestClass_Type")
            };

            // ACT
            var sut = GetTypeDocumentation(typeof(TestClass_Type));

            // ASSERT
            Assert.NotNull(sut.InheritanceHierarchy);
            Assert.Equal(expectedSequence.Length, sut.InheritanceHierarchy.Count);
            Assert.True(expectedSequence.SequenceEqual(sut.InheritanceHierarchy));            
        }

        [Fact]
        public void InheritanceHierarchy_contains_the_expected_types_02()
        {
            // ARRANGE
            var expectedSequence = new[]
            {
                new SimpleTypeId("System", "Object"),
                new SimpleTypeId("System", "ValueType"),
                new SimpleTypeId("MdDoc.Test.TestData", "TestStruct_Type")
            };

            // ACT
            var sut = GetTypeDocumentation(typeof(TestStruct_Type));

            // ASSERT
            Assert.NotNull(sut.InheritanceHierarchy);
            Assert.Equal(expectedSequence.Length, sut.InheritanceHierarchy.Count);
            Assert.True(expectedSequence.SequenceEqual(sut.InheritanceHierarchy));
        }


        [Fact]
        public void InheritanceHierarchy_contains_the_expected_types_03()
        {
            // ARRANGE
            var expectedSequence = new[]
            {
                new SimpleTypeId("System", "Object"),
                new SimpleTypeId("System", "ValueType"),
                new SimpleTypeId("System", "Enum"),
                new SimpleTypeId("MdDoc.Test.TestData", "TestEnum_Type")
            };

            // ACT
            var sut = GetTypeDocumentation(typeof(TestEnum_Type));

            // ASSERT
            Assert.NotNull(sut.InheritanceHierarchy);
            Assert.Equal(expectedSequence.Length, sut.InheritanceHierarchy.Count);       
            Assert.True(expectedSequence.SequenceEqual(sut.InheritanceHierarchy));
        }

        [Fact]
        public void InheritanceHierarchy_Is_empty_for_interfaces_01()
        {
            // ARRANGE / ACT
            var sut = GetTypeDocumentation(typeof(TestInterface_Type));

            // ASSERT
            Assert.NotNull(sut.InheritanceHierarchy);
            Assert.Empty(sut.InheritanceHierarchy);
        }
        
        [Fact]
        public void InheritanceHierarchy_Is_empty_for_interfaces_02()
        {
            // ARRANGE / ACT
            var sut = GetTypeDocumentation(typeof(TestInterface_Inheritance));

            // ASSERT
            Assert.NotNull(sut.InheritanceHierarchy);
            Assert.Empty(sut.InheritanceHierarchy);
        }

        [Theory]
#pragma warning disable CS0618 // Type or member is obsolete
        [InlineData(nameof(TestClass_Attributes))]
        [InlineData(nameof(TestClass_Attributes_ExtensionMethods))]
        [InlineData(nameof(TestStruct_Attributes))]
        [InlineData(nameof(TestInterface_Attributes))]
        [InlineData(nameof(TestEnum_Attributes))]
#pragma warning restore CS0618 // Type or member is obsolete
        public void Attributes_returns_the_expected_types(string typeName)
        {
            // ARRANGE
            var expectedAttributes = new[]
            {
                new SimpleTypeId("System", "ObsoleteAttribute"),
                new SimpleTypeId("MdDoc.Test.TestData", "TestAttribute")
            };

            // ACT
            var sut = GetTypeDocumentation(typeName);

            // ASSERT
            Assert.NotNull(sut.Attributes);
            Assert.Equal(expectedAttributes.Length, sut.Attributes.Count);
            Assert.True(expectedAttributes.SequenceEqual(sut.Attributes));
        }

        [Fact]
        public void TryGetDocumentation_returns_null_for_an_undocumented_type()
        {
            // ARRANGE
            var typeId = GetTypeId(typeof(TestClass_InternalType));
            var sut = GetTypeDocumentation(typeof(TestClass_Type));
            
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
            var sut = GetTypeDocumentation(typeof(TestClass_Type));

            // ACT
            var documentation = sut.TryGetDocumentation(typeId);

            // ASSERT
            Assert.NotNull(documentation);
            Assert.IsType<TypeDocumentation>(documentation);
            Assert.Equal(typeId, ((TypeDocumentation)documentation).TypeId);
        }

        [Fact]
        public void ImplementedInterfaces_is_empty_for_interfaces_which_do_not_inherit_from_other_interfaces()
        {
            // ARRANGE / ACT        
            var sut = GetTypeDocumentation(typeof(TestInterface_Type));

            // ASSERT
            Assert.NotNull(sut.ImplementedInterfaces);
            Assert.Empty(sut.ImplementedInterfaces);            
        }

        [Fact]
        public void ImplementedInterfaces_returns_the_expected_list_of_interfaces_for_interfaces()
        {
            // ARRANGE / ACT      
            var sut = GetTypeDocumentation(typeof(TestInterface_Inheritance));

            // ASSERT
            Assert.NotNull(sut.ImplementedInterfaces);
            Assert.Single(sut.ImplementedInterfaces);
            Assert.Contains(sut.ImplementedInterfaces, i => i.Equals(new SimpleTypeId("System", "IDisposable")));
        }

        [Fact]
        public void ImplementedInterfaces_is_empty_for_enums()
        {
            // ARRANGE / ACT        
            var sut = GetTypeDocumentation(typeof(TestEnum_Type));

            // ASSERT
            Assert.NotNull(sut.ImplementedInterfaces);
            Assert.Empty(sut.ImplementedInterfaces);
        }

        [Fact]
        public void ImplementedInterfaces_returns_the_expected_list_of_interfaces_for_classes()
        {
            // ARRANGE / ACT
            var sut = GetTypeDocumentation(typeof(TestClass_InterfaceImplementation));

            // ASSERT
            Assert.NotNull(sut.ImplementedInterfaces);
            Assert.Equal(2, sut.ImplementedInterfaces.Count);
            Assert.Contains(sut.ImplementedInterfaces, i => i.Equals(new SimpleTypeId("MdDoc.Test.TestData", "TestInterface_Type"))); 
            Assert.Contains(sut.ImplementedInterfaces, i => i.Equals(new SimpleTypeId("System", "IDisposable")));
        }

        [Fact]
        public void ImplementedInterfaces_is_empty_for_classes_that_do_not_implement_interfaces()
        {
            // ARRANGE / ACT
            var sut = GetTypeDocumentation(typeof(TestClass_Type));

            // ASSERT
            Assert.NotNull(sut.ImplementedInterfaces);
            Assert.Empty(sut.ImplementedInterfaces);           
        }

        [Fact]
        public void ImplementedInterfaces_returns_the_expected_list_of_interfaces_for_structs()
        {
            // ARRANGE / ACT
            var sut = GetTypeDocumentation(typeof(TestStruct_InterfaceImplementation));

            // ASSERT
            Assert.NotNull(sut.ImplementedInterfaces);
            Assert.Equal(2, sut.ImplementedInterfaces.Count);
            Assert.Contains(sut.ImplementedInterfaces, i => i.Equals(new SimpleTypeId("MdDoc.Test.TestData", "TestInterface_Type")));
            Assert.Contains(sut.ImplementedInterfaces, i => i.Equals(new SimpleTypeId("System", "IDisposable")));
        }

        [Fact]
        public void ImplementedInterfaces_is_empty_for_structs_that_do_not_implement_interfaces()
        {
            // ARRANGE / ACT
            var sut = GetTypeDocumentation(typeof(TestStruct_Type));

            // ASSERT
            Assert.NotNull(sut.ImplementedInterfaces);
            Assert.Empty(sut.ImplementedInterfaces);
        }

        [Fact]
        public void TypeParameters_is_empty_for_non_generic_type()
        {
            // ARRANGE´/ ACT
            var sut = GetTypeDocumentation(typeof(TestClass_Type));

            // ASSERT
            Assert.NotNull(sut.TypeParameters);
            Assert.Empty(sut.TypeParameters);
        }

        [Fact]
        public void TypeParameters_returns_expected_parameters()
        {
            // ARRANGE´/ ACT
            var sut = GetTypeDocumentation(typeof(TestClass_GenericType<>));

            // ASSERT
            Assert.NotNull(sut.TypeParameters);
            Assert.Single(sut.TypeParameters);
            Assert.Contains(sut.TypeParameters, typeParam => typeParam.Name == "T1");
        }
    }
}
