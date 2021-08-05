using System;
using System.Linq;
using Grynwald.MdDocs.ApiReference.Model;
using Grynwald.MdDocs.ApiReference.Model.XmlDocs;
using Grynwald.MdDocs.TestHelpers;
using Microsoft.Extensions.Logging.Abstractions;
using Mono.Cecil;
using Xunit;

namespace Grynwald.MdDocs.ApiReference.Test.Model
{
    public class TypeDocumentationTest : DynamicCompilationTestBase
    {
        private AssemblyDocumentation GetAssemblyDocumentation(AssemblyDefinition assemblyDefinition)
        {
            using var assemblySetDocumentation = AssemblySetDocumentation.FromAssemblyDefinitions(assemblyDefinition);
            return assemblySetDocumentation.Assemblies.Single();
        }

        [Theory]
        [InlineData("public class Class1", TypeKind.Class)]
        [InlineData("public struct Struct1", TypeKind.Struct)]
        [InlineData("public interface Interface1", TypeKind.Interface)]
        [InlineData("public enum Enum1", TypeKind.Enum)]
        // Ported to MonoCecilDocumentationLoaderTest
        public void Kind_returns_the_expected_value(string definition, TypeKind expectedKind)
        {
            // ARRANGE
            var cs = $@"
                using System;

                {definition}
                {{ }}
            ";

            using var assembly = Compile(cs);
            using var assemblyDocumentation = GetAssemblyDocumentation(assembly);

            var sut = assemblyDocumentation.Types.Single();

            // ACT
            var actualKind = sut.Kind;

            // ASSERT
            Assert.Equal(expectedKind, actualKind);
        }

        [Fact]
        public void AssemblyName_returns_the_expected_value()
        {
            // ARRANGE
            var cs = @"
                using System;

                public class Class1
                { }
            ";

            using var assembly = Compile(cs);
            using var assemblyDocumentation = GetAssemblyDocumentation(assembly);

            var sut = assemblyDocumentation.Types.Single();

            // Act / ASSERT
            Assert.Equal(assembly.Name.Name, sut.AssemblyName);
        }

        [Fact]
        // Ported to MonoCecilDocumentationLoaderTest
        public void Fields_returns_expected_fields_for_classes()
        {
            // ARRANGE 
            var cs = @"
	            using System;

	            public class Class1
	            {
                    public int Field1;
                }
            ";

            using var assembly = Compile(cs);
            using var assemblyDocumentation = GetAssemblyDocumentation(assembly);

            var sut = assemblyDocumentation.Types.Single();

            // ACT / ASSERT
            Assert.Single(sut.Fields);
            Assert.Contains(sut.Fields, field => field.Name == "Field1");
        }

        [Fact]
        // Ported to MonoCecilDocumentationLoaderTest
        public void Fields_returns_expected_fields_for_enums()
        {
            // ARRANGE
            var cs = @"
	            using System;

	            public enum Enum1
	            {
                    Value1,
                    Value2
                }
            ";

            using var assembly = Compile(cs);
            using var assemblyDocumentation = GetAssemblyDocumentation(assembly);

            var sut = assemblyDocumentation.Types.Single();

            // ACT / ASSERT
            Assert.Equal(2, sut.Fields.Count);
            Assert.Contains(sut.Fields, field => field.Name == "Value1");
            Assert.Contains(sut.Fields, field => field.Name == "Value2");
        }

        [Fact]
        // Ported to MonoCecilDocumentationLoaderTest
        public void Events_returns_expected_events_for_classes()
        {
            var cs = @"
	            using System;

	            public class Class1
	            {
                    public event EventHandler Event1;

                    public event EventHandler<EventArgs> Event2;

                    public event EventHandler Event3
                    {
                        add { throw new NotImplementedException(); }
                        remove { throw new NotImplementedException(); }
                    }
                }
            ";

            using var assembly = Compile(cs);
            using var assemblyDocumentation = GetAssemblyDocumentation(assembly);

            var sut = assemblyDocumentation.Types.Single();

            // ACT / ASSERT
            Assert.Equal(3, sut.Events.Count);
            Assert.Contains(sut.Events, e => e.Name == "Event1");
            Assert.Contains(sut.Events, e => e.Name == "Event2");
            Assert.Contains(sut.Events, e => e.Name == "Event3");
        }

        [Fact]
        // Ported to MonoCecilDocumentationLoaderTest
        public void Events_returns_expected_events_for_interfaces()
        {
            // ARRANGE
            var cs = @"
	            using System;

	            public interface Interface1
	            {
                    event EventHandler Event1;

                    event EventHandler<EventArgs> Event2;

                    event EventHandler Event3;
                }
            ";

            using var assembly = Compile(cs);
            using var assemblyDocumentation = GetAssemblyDocumentation(assembly);

            var sut = assemblyDocumentation.Types.Single();

            // ACT / ASSERT
            Assert.Equal(3, sut.Events.Count);
            Assert.Contains(sut.Events, e => e.Name == "Event1");
            Assert.Contains(sut.Events, e => e.Name == "Event2");
            Assert.Contains(sut.Events, e => e.Name == "Event3");
        }

        [Fact]
        // Ported to MonoCecilDocumentationLoaderTest
        public void Events_is_empty_for_enums()
        {
            // ARRANGE
            var cs = @"
	            using System;

	            public enum Enum1
	            {
                    Value1,
                    Value2
                }
            ";

            using var assembly = Compile(cs);
            using var assemblyDocumentation = GetAssemblyDocumentation(assembly);

            var sut = assemblyDocumentation.Types.Single();

            // ACT / ASSERT
            Assert.NotNull(sut.Events);
            Assert.Empty(sut.Events);
        }

        [Fact]
        public void Properties_returns_expected_properties_01()
        {
            // ARRANGE
            var cs = @"
	            using System;

	            public class Class1
	            {
                    public int Property1 { get; set; }

                    public int Property2 { get; }

                    internal int Property3 { get; }

                    private int Property4 { get; }

                    public int this[int foo] => throw new NotImplementedException();

                    public int this[int foo, double bar] => throw new NotImplementedException();
                }
            ";

            using var assembly = Compile(cs);
            using var assemblyDocumentation = GetAssemblyDocumentation(assembly);

            var sut = assemblyDocumentation.Types.Single();

            // ACT
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
            // ARRANGE
            var cs = @"
	            using System;

	            public interface Interface1
	            {
                    int Property1 { get; set; }

                    int Property2 { get; }

                    int this[int foo] { get; }

                    int this[int foo, double bar] { get; }
                }
            ";

            using var assembly = Compile(cs);
            using var assemblyDocumentation = GetAssemblyDocumentation(assembly);

            var sut = assemblyDocumentation.Types.Single();

            // ACT
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
            // ARRANGE
            var cs = @"
	            using System;

	            public struct Struct1
	            {
                    public int Property1 { get; set; }

                    public int Property2 { get; }

                    internal int Property3 { get; }

                    private int Property4 { get; }

                    public int this[int foo] => throw new NotImplementedException();

                    public int this[int foo, double bar] => throw new NotImplementedException();
                }
            ";

            using var assembly = Compile(cs);
            using var assemblyDocumentation = GetAssemblyDocumentation(assembly);

            var sut = assemblyDocumentation.Types.Single();

            // ARRANGE / ACT
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
            // ARRANGE
            var cs = @"
	            using System;

	            public class Class1
	            {
                    public int Property1 { get; set; }

                    public int Property2 { get; }

                    internal int Property3 { get; }

                    private int Property4 { get; }

                    public int this[int foo] => throw new NotImplementedException();

                    public int this[int foo, double bar] => throw new NotImplementedException();
                }
            ";

            using var assembly = Compile(cs);
            using var assemblyDocumentation = GetAssemblyDocumentation(assembly);

            var sut = assemblyDocumentation.Types.Single();

            // ACT
            var indexers = sut.Indexers;

            // ASSERT
            Assert.Equal(1, indexers.Count);
            Assert.Contains(indexers, indexer => indexer.Name == "Item" && indexer.Overloads.Count == 2);
        }

        [Fact]
        public void Indexers_returns_expected_properties_02()
        {
            // ARRANGE
            var cs = @"
	            using System;

	            public interface Interface1
	            {
                    int Property1 { get; set; }

                    int Property2 { get; }

                    int this[int foo] { get; }

                    int this[int foo, double bar] { get; }
                }
            ";

            using var assembly = Compile(cs);
            using var assemblyDocumentation = GetAssemblyDocumentation(assembly);

            var sut = assemblyDocumentation.Types.Single();

            // ARRANGE / ACT
            var indexers = sut.Indexers;

            // ASSERT
            Assert.Equal(1, indexers.Count);
            Assert.Contains(indexers, indexer => indexer.Name == "Item" && indexer.Overloads.Count == 2);
        }

        [Fact]
        public void Indexers_returns_expected_properties_03()
        {
            // ARRANGE
            var cs = @"
	            using System;

	            public struct Struct1
	            {
                    public int Property1 { get; set; }

                    public int Property2 { get; }

                    internal int Property3 { get; }

                    private int Property4 { get; }

                    public int this[int foo] => throw new NotImplementedException();

                    public int this[int foo, double bar] => throw new NotImplementedException();
                }
            ";

            using var assembly = Compile(cs);
            using var assemblyDocumentation = GetAssemblyDocumentation(assembly);

            var sut = assemblyDocumentation.Types.Single();

            // ARRANGE / ACT
            var indexers = sut.Indexers;

            // ASSERT
            Assert.Equal(1, indexers.Count);
            Assert.Contains(indexers, indexer => indexer.Name == "Item" && indexer.Overloads.Count == 2);
        }

        [Fact]
        public void Properties_is_empty_for_enums()
        {
            // ARRANGE
            var cs = @"
	            using System;

	            public enum Enum1
	            {
                    Value1,
                    Value2
                }
            ";

            using var assembly = Compile(cs);
            using var assemblyDocumentation = GetAssemblyDocumentation(assembly);

            var sut = assemblyDocumentation.Types.Single();

            // ACT / ASSERT
            Assert.NotNull(sut.Properties);
            Assert.Empty(sut.Properties);
        }

        [Fact]
        public void Methods_returns_the_expected_methods()
        {
            // ARRANGE
            var cs = @"
	            using System;

	            public class Class1
	            {
                    public void Method1() => throw new NotImplementedException();

                    public string Method2() => throw new NotImplementedException();

                    public string Method3(string parameter) => throw new NotImplementedException();

                    public string Method4<T1, T2>(T1 foo, T2 bar) => throw new NotImplementedException();
	            }
            ";

            using var assembly = Compile(cs);
            using var assemblyDocumentation = GetAssemblyDocumentation(assembly);

            var sut = assemblyDocumentation.Types.Single();

            // ACT / ASSERT
            Assert.Equal(4, sut.Methods.Count);
            Assert.All(sut.Methods, method => Assert.Single(method.Overloads));
            Assert.Contains(sut.Methods, m => m.Name == "Method1");
            Assert.Contains(sut.Methods, m => m.Name == "Method2");
            Assert.Contains(sut.Methods, m => m.Name == "Method3");
            Assert.Contains(sut.Methods, m => m.Name == "Method4");

        }

        [Fact]
        public void Methods_do_not_include_constructors()
        {
            // ARRANGE
            var cs = @"
	            using System;

	            public class Class1
	            {
	                public Class1()
                    {}

                    public Class1(string parameter)
                    {}
                }
            ";

            using var assembly = Compile(cs);
            using var assemblyDocumentation = GetAssemblyDocumentation(assembly);

            var sut = assemblyDocumentation.Types.Single();

            // ACT / ASSERT
            Assert.NotNull(sut.Methods);
            Assert.Empty(sut.Methods);
        }

        [Fact]
        public void Methods_include_overloads_with_generic_parameters()
        {
            // ARRANGE
            var cs = @"
	            using System;

	            public class Class1
	            {
                    public void Method1() { }

                    public void Method1(string foo) { }

                    public void Method1<T>(T foo) { }
                }
            ";

            using var assembly = Compile(cs);
            using var assemblyDocumentation = GetAssemblyDocumentation(assembly);

            var sut = assemblyDocumentation.Types.Single();

            // ACT / ASSERT
            var method = Assert.Single(sut.Methods);

            Assert.Equal("Method1", method.Name);
            Assert.Equal(3, method.Overloads.Count);
            Assert.Single(method.Overloads.Where(x => x.Definition.HasGenericParameters));
        }

        [Fact]
        public void Methods_does_not_include_operator_overloads()
        {
            // ARRANGE
            var cs = @"
	            using System;

	            public class Class1
	            {
                    public static Class1 operator +(Class1  other) => throw new NotImplementedException();
                }
            ";

            using var assembly = Compile(cs);
            using var assemblyDocumentation = GetAssemblyDocumentation(assembly);

            var sut = assemblyDocumentation.Types.Single();

            // ACT / ASSERT
            Assert.NotNull(sut.Methods);
            Assert.Empty(sut.Methods);
        }

        [Fact]
        public void Methods_does_not_include_property_getters_and_setters()
        {
            // ARRANGE
            var cs = @"
	            using System;

	            public struct Struct1
	            {
                    public int Property1 { get; set; }

                    public int Property2 { get; }

                    internal int Property3 { get; }

                    private int Property4 { get; }

                    public int this[int foo] => throw new NotImplementedException();

                    public int this[int foo, double bar] => throw new NotImplementedException();
                }
            ";

            using var assembly = Compile(cs);
            using var assemblyDocumentation = GetAssemblyDocumentation(assembly);

            var sut = assemblyDocumentation.Types.Single();

            // ACT / ASSERT
            Assert.NotNull(sut.Methods);
            Assert.Empty(sut.Methods);
        }

        [Fact]
        public void Methods_does_not_include_event_accessors()
        {
            // ARRANGE
            var cs = @"
	            using System;

	            public class Class1
	            {
                    public event EventHandler Event1;

                    public event EventHandler<EventArgs> Event2;

                    public event EventHandler Event3
                    {
                        add { throw new NotImplementedException(); }
                        remove { throw new NotImplementedException(); }
                    }
                }
            ";

            using var assembly = Compile(cs);
            using var assemblyDocumentation = GetAssemblyDocumentation(assembly);

            var sut = assemblyDocumentation.Types.Single();

            // ACT / ASSERT
            Assert.NotNull(sut.Methods);
            Assert.Empty(sut.Methods);
        }

        [Fact]
        public void Constructors_returns_the_expected_constructor_overloads_for_classes_01()
        {
            // ARRANGE
            var cs = @"
	            using System;

	            public class Class1
	            {
                }
            ";

            using var assembly = Compile(cs);
            using var assemblyDocumentation = GetAssemblyDocumentation(assembly);

            var sut = assemblyDocumentation.Types.Single();

            // ACT / ASSERT
            Assert.NotNull(sut.Constructors);
            Assert.NotNull(sut.Constructors!.Overloads);
            Assert.Single(sut.Constructors!.Overloads);
        }

        [Fact]
        public void Constructors_returns_the_expected_constructor_overloads_for_classes_02()
        {
            // ARRANGE
            var cs = @"
	            using System;

	            public class Class1
	            {
	                public Class1()
	                { }

	                public Class1(string parameter)
	                { }
                }
            ";

            using var assembly = Compile(cs);
            using var assemblyDocumentation = GetAssemblyDocumentation(assembly);

            var sut = assemblyDocumentation.Types.Single();

            // ACT / ASSERT
            Assert.NotNull(sut.Constructors);
            Assert.Equal(2, sut.Constructors!.Overloads.Count);
        }

        [Fact]
        public void Constructors_returns_the_expected_constructor_overloads_for_structs_01()
        {
            // ARRANGE
            var cs = @"
	            using System;

	            public struct Struct1
	            { }
            ";

            using var assembly = Compile(cs);
            using var assemblyDocumentation = GetAssemblyDocumentation(assembly);

            var sut = assemblyDocumentation.Types.Single();

            // ACT / ASSERT
            Assert.Null(sut.Constructors);
        }

        [Fact]
        public void Constructors_returns_the_expected_constructor_overloads_for_structs_02()
        {
            // ARRANGE
            var cs = @"
	            using System;

	            public struct Struct1
	            {
                    public Struct1(string foo)
                    {
                    }

                    public Struct1(int bar)
                    {
                    }
                }
            ";

            using var assembly = Compile(cs);
            using var assemblyDocumentation = GetAssemblyDocumentation(assembly);

            var sut = assemblyDocumentation.Types.Single();


            // ACT / ASSERT
            Assert.NotNull(sut.Constructors);
            Assert.Equal(2, sut.Constructors!.Overloads.Count);
        }

        [Fact]
        public void Constructors_returns_null_for_interfaces()
        {
            // ARRANGE
            var cs = @"
	            using System;

	            public interface Interface1
	            { }
            ";

            using var assembly = Compile(cs);
            using var assemblyDocumentation = GetAssemblyDocumentation(assembly);

            var sut = assemblyDocumentation.Types.Single();

            // ACT / ASSERT
            Assert.Null(sut.Constructors);
        }

        [Fact]
        public void Constructors_returns_null_for_enums()
        {
            // ARRANGE
            var cs = @"
	            using System;

	            public enum Enum1
	            {
	                Value1,
	                Value2
	            }
            ";

            using var assembly = Compile(cs);
            using var assemblyDocumentation = GetAssemblyDocumentation(assembly);

            var sut = assemblyDocumentation.Types.Single();

            // ACT / ASSERT
            Assert.Null(sut.Constructors);
        }


        [Fact]
        public void Operators_contains_expected_number_of_operator_overloads()
        {
            // ARRANGE
            var cs = @"
	            using System;

	            public class Class1
	            {
                    public static Class1 operator +(Class1 other) => throw new NotImplementedException();

                    public static Class1 operator +(Class1 left, Class1 right) => throw new NotImplementedException();

                    public static Class1 operator -(Class1 other) => throw new NotImplementedException();

                    public static Class1 operator -(Class1 left, Class1 right) => throw new NotImplementedException();

                    public static Class1 operator *(Class1 left, Class1 right) => throw new NotImplementedException();

                    public static Class1 operator /(Class1 left, Class1 right) => throw new NotImplementedException();

                    public static Class1 operator %(Class1 left, Class1 right) => throw new NotImplementedException();

                    public static Class1 operator &(Class1 left, Class1 right) => throw new NotImplementedException();

                    public static Class1 operator |(Class1 left, Class1 right) => throw new NotImplementedException();

                    public static Class1 operator ^(Class1 left, Class1 right) => throw new NotImplementedException();

                    public static Class1 operator <<(Class1 left, int count) => throw new NotImplementedException();

                    public static Class1 operator >>(Class1 left, int count) => throw new NotImplementedException();

                    public static Class1 operator !(Class1 other) => throw new NotImplementedException();

                    public static Class1 operator ~(Class1 other) => throw new NotImplementedException();

                    public static Class1 operator ++(Class1 other) => throw new NotImplementedException();

                    public static Class1 operator --(Class1 other) => throw new NotImplementedException();

                    public static bool operator true(Class1 other) => throw new NotImplementedException();

                    public static bool operator false(Class1 other) => throw new NotImplementedException();

                    public static Class1 operator ==(Class1 left, Class1 right) => throw new NotImplementedException();

                    public static Class1 operator !=(Class1 left, Class1 right) => throw new NotImplementedException();

                    public static Class1 operator <(Class1 left, Class1 right) => throw new NotImplementedException();

                    public static Class1 operator >(Class1 left, Class1 right) => throw new NotImplementedException();

                    public static Class1 operator <=(Class1 left, Class1 right) => throw new NotImplementedException();

                    public static Class1 operator >=(Class1 left, Class1 right) => throw new NotImplementedException();

                    public static implicit operator string(Class1 instance) => throw new NotImplementedException();
		
                    public static explicit operator int(Class1 instance) => throw new NotImplementedException();
                }
            ";

            using var assembly = Compile(cs);
            using var assemblyDocumentation = GetAssemblyDocumentation(assembly);

            var sut = assemblyDocumentation.Types.Single();

            // ACT / ASSERT
            Assert.NotNull(sut.Operators);
            Assert.Equal(26, sut.Operators.Count);
        }

        [Fact]
        public void Multiple_overoads_of_the_same_operator_are_gropued()
        {
            // ARRANGE
            var cs = @"
	            using System;

	            public class Class1
	            {
                    public static Class1 operator +(Class1 left, int right) => throw new NotImplementedException();

                    public static Class1 operator +(Class1 left, double right) => throw new NotImplementedException();
                }
            ";

            using var assembly = Compile(cs);
            using var assemblyDocumentation = GetAssemblyDocumentation(assembly);

            var sut = assemblyDocumentation.Types.Single();

            // ACT / ASSERT
            Assert.NotNull(sut.Operators);
            var operatorOverload = Assert.Single(sut.Operators);
            Assert.Equal(2, operatorOverload.Overloads.Count);
        }

        [Fact]
        public void InheritanceHierarchy_contains_the_expected_types_01()
        {
            // ARRANGE
            var cs = @"
	            using System;

	            public class Class1
	            { }
            ";

            using var assembly = Compile(cs);
            using var assemblyDocumentation = GetAssemblyDocumentation(assembly);

            var sut = assemblyDocumentation.Types.Single();

            var expectedSequence = new[]
            {
                new SimpleTypeId("System", "Object"),
                new SimpleTypeId("", "Class1")
            };

            // ACT / ASSERT
            Assert.NotNull(sut.InheritanceHierarchy);
            Assert.Equal(expectedSequence.Length, sut.InheritanceHierarchy.Count);
            Assert.True(expectedSequence.SequenceEqual(sut.InheritanceHierarchy));
        }

        [Fact]
        public void InheritanceHierarchy_contains_the_expected_types_02()
        {
            // ARRANGE
            var cs = @"
	            using System;

	            public struct Struct1
	            { }
            ";

            using var assembly = Compile(cs);
            using var assemblyDocumentation = GetAssemblyDocumentation(assembly);

            var sut = assemblyDocumentation.Types.Single();

            var expectedSequence = new[]
            {
                new SimpleTypeId("System", "Object"),
                new SimpleTypeId("System", "ValueType"),
                new SimpleTypeId("", "Struct1")
            };

            // ACT / ASSERT
            Assert.NotNull(sut.InheritanceHierarchy);
            Assert.Equal(expectedSequence.Length, sut.InheritanceHierarchy.Count);
            Assert.True(expectedSequence.SequenceEqual(sut.InheritanceHierarchy));
        }

        [Fact]
        public void InheritanceHierarchy_contains_the_expected_types_03()
        {
            // ARRANGE
            var cs = @"
	            using System;

	            public enum Enum1
	            {
	                Value1,
	                Value2,
                }
            ";

            using var assembly = Compile(cs);
            using var assemblyDocumentation = GetAssemblyDocumentation(assembly);

            var sut = assemblyDocumentation.Types.Single();

            var expectedSequence = new[]
            {
                new SimpleTypeId("System", "Object"),
                new SimpleTypeId("System", "ValueType"),
                new SimpleTypeId("System", "Enum"),
                new SimpleTypeId("", "Enum1")
            };


            // ACT / ASSERT
            Assert.NotNull(sut.InheritanceHierarchy);
            Assert.Equal(expectedSequence.Length, sut.InheritanceHierarchy.Count);
            Assert.True(expectedSequence.SequenceEqual(sut.InheritanceHierarchy));
        }

        [Fact]
        public void InheritanceHierarchy_is_empty_for_interfaces_01()
        {
            // ARRANGE
            var cs = @"
	            using System;

	            public interface Interface1
	            { }
            ";

            using var assembly = Compile(cs);
            using var assemblyDocumentation = GetAssemblyDocumentation(assembly);

            var sut = assemblyDocumentation.Types.Single();

            // ACT / ASSERT
            Assert.NotNull(sut.InheritanceHierarchy);
            Assert.Empty(sut.InheritanceHierarchy);
        }

        [Fact]
        public void InheritanceHierarchy_is_empty_for_interfaces_02()
        {
            // ARRANGE
            var cs = @"
	            using System;

	            public interface Interface1 : IDisposable
	            { }
            ";

            using var assembly = Compile(cs);
            using var assemblyDocumentation = GetAssemblyDocumentation(assembly);

            var sut = assemblyDocumentation.Types.Single();

            // ACT / ASSERT
            Assert.NotNull(sut.InheritanceHierarchy);
            Assert.Empty(sut.InheritanceHierarchy);
        }

        [Fact]
        public void Attributes_returns_the_expected_types()
        {
            // ARRANGE
            var cs = @"
	            using System;

                namespace Namespace1
                {
                    public class TestAttribute : Attribute
                    { }

                    [Obsolete]
                    [Test]
	                public class Class1
	                {
                        // defining an indexer makes the c# compiler emit a DefaultMember attribute
                        // this should be ignored when reading the type's attribute
                        public int this[int param] => throw new NotImplementedException();
                    }

                    [Obsolete]
                    [Test]
                    public static class Class2   
                    {
                        // defining an extension method makes the c# compiler emit a Extension attribute
                        // this should be ignored when reading the type's attribute
                        public static void Foo(this string str) => throw new NotImplementedException();
                    }

                    [Obsolete]
                    [Test]
                    public readonly struct Struct1
                    {
                        // the readonly modified makes the C# compiler emit a IsReadOnly attribute
                        // this should be ignored when reading the type's attribute
                    }

                    [Obsolete]
                    [Test]
	                public interface Interface1
	                {
                    }

                    [Obsolete]
                    [Test]
                    public enum Enum1
                    {
                        Value1,
                        Value2
                    }
                }
            ";

            using var assembly = Compile(cs);
            using var assemblyDocumentation = GetAssemblyDocumentation(assembly);

            var types = assemblyDocumentation.Types.Where(x => x.TypeId.Name != "TestAttribute");

            var expectedAttributes = new[]
            {
                new SimpleTypeId("System", "ObsoleteAttribute"),
                new SimpleTypeId("Namespace1", "TestAttribute")
            };

            // ASSERT
            Assert.All(types, type =>
            {
                Assert.NotNull(type.Attributes);
                Assert.Equal(expectedAttributes.Length, type.Attributes.Count);
                Assert.True(expectedAttributes.SequenceEqual(type.Attributes));
            });
        }

        [Fact]
        public void Attributes_does_not_include_internal_attributes()
        {
            // ARRANGE
            var cs = @"
	            using System;

                public class Test1Attribute : Attribute 
	            { }

                internal class Test2Attribute : Attribute 
	            { }

                [Test1, Test2]
	            public class Class1
	            { }
            ";

            using var assembly = Compile(cs);
            using var assemblyDocumentation = GetAssemblyDocumentation(assembly);

            var sut = assemblyDocumentation.Types.Single(x => x.TypeId.Name == "Class1");

            var expectedAttributes = new[]
            {
                new SimpleTypeId("", "Test1Attribute")
            };

            // ACT / ASSERT
            Assert.NotNull(sut.Attributes);
            Assert.Equal(expectedAttributes.Length, sut.Attributes.Count);
            Assert.True(expectedAttributes.SequenceEqual(sut.Attributes));
        }

        [Fact]
        public void TryGetDocumentation_returns_null_for_an_undocumented_type()
        {
            // ARRANGE
            var cs = @"
	            using System;

	            public class Class1
	            { }

                internal class Class2
	            { }
            ";

            using var assembly = Compile(cs);
            using var assemblyDocumentation = GetAssemblyDocumentation(assembly);

            var sut = assemblyDocumentation.Types.Single(x => x.TypeId.Name == "Class1");

            var typeId = new SimpleTypeId(NamespaceId.GlobalNamespace, "Class2");

            // ACT
            var documentation = sut.TryGetDocumentation(typeId);

            // ASSERT
            Assert.Null(documentation);
        }

        [Fact]
        public void TryGetDocumenation_returns_expected_documentation_item_for_an_documented_type()
        {
            // ARRANGE
            var cs = @"
	            using System;

	            public class Class1
	            { }

	            public class Class2
	            { }
            ";

            using var assembly = Compile(cs);
            using var assemblyDocumentation = GetAssemblyDocumentation(assembly);

            var sut = assemblyDocumentation.Types.Single(x => x.TypeId.Name == "Class1");

            // ARRANGE
            var typeId = new SimpleTypeId(NamespaceId.GlobalNamespace, "Class2");

            // ACT
            var documentation = sut.TryGetDocumentation(typeId);

            // ASSERT
            Assert.NotNull(documentation);
            var typeDocumentation = Assert.IsType<TypeDocumentation>(documentation);
            Assert.Equal(typeId, typeDocumentation.TypeId);
        }

        [Fact]
        public void ImplementedInterfaces_is_empty_for_interfaces_which_do_not_inherit_from_other_interfaces()
        {
            // ARRANGE
            var cs = @"
	            using System;

	            public interface Interface1
	            { }
            ";

            using var assembly = Compile(cs);
            using var assemblyDocumentation = GetAssemblyDocumentation(assembly);

            var sut = assemblyDocumentation.Types.Single();

            // ACT / ASSERT
            Assert.NotNull(sut.ImplementedInterfaces);
            Assert.Empty(sut.ImplementedInterfaces);
        }

        [Fact]
        public void ImplementedInterfaces_returns_the_expected_list_of_interfaces_for_interfaces()
        {
            // ARRANGE
            var cs = @"
	            using System;

	            public interface Interface1 : IDisposable
	            { }
            ";

            using var assembly = Compile(cs);
            using var assemblyDocumentation = GetAssemblyDocumentation(assembly);

            var sut = assemblyDocumentation.Types.Single();

            // ACT / ASSERT
            Assert.NotNull(sut.ImplementedInterfaces);
            Assert.Single(sut.ImplementedInterfaces);
            Assert.Contains(sut.ImplementedInterfaces, i => i.Equals(new SimpleTypeId("System", "IDisposable")));
        }

        [Fact]
        public void ImplementedInterfaces_is_empty_for_enums()
        {
            // ARRANGE
            var cs = @"
	            using System;

	            public enum Enum1
	            {
	                Value1, 
	                Value2
	            }
            ";

            using var assembly = Compile(cs);
            using var assemblyDocumentation = GetAssemblyDocumentation(assembly);

            var sut = assemblyDocumentation.Types.Single();

            // ACT / ASSERT
            Assert.NotNull(sut.ImplementedInterfaces);
            Assert.Empty(sut.ImplementedInterfaces);
        }

        [Fact]
        public void ImplementedInterfaces_returns_the_expected_list_of_interfaces_for_classes()
        {
            // ARRANGE
            var cs = @"
	            using System;

                public interface Interface1
	            { }

	            public class Class1 : Interface1, IDisposable
	            {
                    public void Dispose()
	                { }
                }
            ";

            using var assembly = Compile(cs);
            using var assemblyDocumentation = GetAssemblyDocumentation(assembly);

            var sut = assemblyDocumentation.Types.Single(x => x.TypeId.Name == "Class1");


            // ACT / ASSERT
            Assert.NotNull(sut.ImplementedInterfaces);
            Assert.Equal(2, sut.ImplementedInterfaces.Count);
            Assert.Contains(sut.ImplementedInterfaces, i => i.Equals(new SimpleTypeId(NamespaceId.GlobalNamespace, "Interface1")));
            Assert.Contains(sut.ImplementedInterfaces, i => i.Equals(new SimpleTypeId("System", "IDisposable")));
        }

        [Fact]
        public void ImplementedInterfaces_is_empty_for_classes_that_do_not_implement_interfaces()
        {
            // ARRANGE
            var cs = @"
	            using System;

	            public class Class1
	            { }
            ";

            using var assembly = Compile(cs);
            using var assemblyDocumentation = GetAssemblyDocumentation(assembly);

            var sut = assemblyDocumentation.Types.Single();

            // ACT / ASSERT
            Assert.NotNull(sut.ImplementedInterfaces);
            Assert.Empty(sut.ImplementedInterfaces);
        }

        [Fact]
        public void ImplementedInterfaces_returns_the_expected_list_of_interfaces_for_structs()
        {
            // ARRANGE
            var cs = @"
	            using System;

                public interface Interface1
	            { }

	            public struct Struct1 : Interface1, IDisposable
	            {
                    public void Dispose()
                    { }
                }
            ";

            using var assembly = Compile(cs);
            using var assemblyDocumentation = GetAssemblyDocumentation(assembly);

            var sut = assemblyDocumentation.Types.Single(x => x.TypeId.Name == "Struct1");

            // ACT / ASSERT
            Assert.NotNull(sut.ImplementedInterfaces);
            Assert.Equal(2, sut.ImplementedInterfaces.Count);
            Assert.Contains(sut.ImplementedInterfaces, i => i.Equals(new SimpleTypeId(NamespaceId.GlobalNamespace, "Interface1")));
            Assert.Contains(sut.ImplementedInterfaces, i => i.Equals(new SimpleTypeId("System", "IDisposable")));
        }

        [Fact]
        public void ImplementedInterfaces_is_empty_for_structs_that_do_not_implement_interfaces()
        {

            // ARRANGE
            var cs = @"
	            using System;

	            public struct Struct1
	            { }
            ";

            using var assembly = Compile(cs);
            using var assemblyDocumentation = GetAssemblyDocumentation(assembly);

            var sut = assemblyDocumentation.Types.Single();

            // ACT / ASSERT
            Assert.NotNull(sut.ImplementedInterfaces);
            Assert.Empty(sut.ImplementedInterfaces);
        }

        [Fact]
        public void TypeParameters_is_empty_for_non_generic_type()
        {
            // ARRANGE
            var cs = @"
	            using System;

	            public class Class1
	            { }
            ";

            using var assembly = Compile(cs);
            using var assemblyDocumentation = GetAssemblyDocumentation(assembly);

            var sut = assemblyDocumentation.Types.Single();

            // ACT / ASSERT
            Assert.NotNull(sut.TypeParameters);
            Assert.Empty(sut.TypeParameters);
        }

        [Fact]
        public void TypeParameters_returns_expected_parameters()
        {
            // ARRANGE
            var cs = @"
	            using System;

	            public class Class1<T1>
	            { }
            ";

            using var assembly = Compile(cs);
            using var assemblyDocumentation = GetAssemblyDocumentation(assembly);

            var sut = assemblyDocumentation.Types.Single();

            // ACT / ASSERT
            Assert.NotNull(sut.TypeParameters);
            Assert.Single(sut.TypeParameters);
            Assert.Contains(sut.TypeParameters, typeParam => typeParam.Name == "T1");
        }
    }
}
