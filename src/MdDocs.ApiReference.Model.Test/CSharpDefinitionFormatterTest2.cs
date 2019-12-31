using System.Linq;
using Xunit;

namespace Grynwald.MdDocs.ApiReference.Model.Test
{
    public class CSharpDefinitionFormatterTest2 : DynamicCompilationTestBase
    {
        [Fact]
        public void GetDefinition_does_not_include_internal_attributes()
        {
            // ARRANGE
            var cs = @"
                using System;

                internal class MyInternalAttribute : Attribute
                { }

                [MyInternalAttribute]
                public class MyClass
                {
                    [MyInternalAttribute]
                    public string Property1 { get; }

                    [MyInternalAttribute]
                    public string this[int index] => throw new NotImplementedException();

                    [MyInternalAttribute]
                    public int Field1;

                    [MyInternalAttribute]
                    public static event EventHandler Event1;

                    [MyInternalAttribute]
                    public void Method1([MyInternalAttribute]string parameter) => throw new NotImplementedException();
                }
            ";

            using var assembly = Compile(cs);

            var typeDefinition = assembly.MainModule.GetTypes().Single(x => x.Name == "MyClass");
            var propertyDefinition = typeDefinition.Properties.Single(x => !x.HasParameters);
            var indexerDefinition = typeDefinition.Properties.Single(x => x.HasParameters);
            var fieldDefinition = typeDefinition.Fields.Single(x => x.IsPublic);
            var eventDefinition = typeDefinition.Events.Single();
            var methodDefinition = typeDefinition.Methods.Single(x => x.Name == "Method1");

            // ACT / ASSERT
            Assert.Equal("public class MyClass", CSharpDefinitionFormatter.GetDefinition(typeDefinition));
            Assert.Equal("public string Property1 { get; }", CSharpDefinitionFormatter.GetDefinition(propertyDefinition));
            Assert.Equal("public string this[int index] { get; }", CSharpDefinitionFormatter.GetDefinition(indexerDefinition));
            Assert.Equal("public int Field1;", CSharpDefinitionFormatter.GetDefinition(fieldDefinition));
            Assert.Equal("public static event EventHandler Event1;", CSharpDefinitionFormatter.GetDefinition(eventDefinition));
            Assert.Equal("public void Method1(string parameter);", CSharpDefinitionFormatter.GetDefinition(methodDefinition));
        }

        [Fact]
        public void GetDefinition_returns_the_expected_definition_for_nested_types()
        {
            // ARRANGE
            var cs = @"

                public class Class1
                {
                    public class NestedClass1
                    {
                        public class NestedClass2
                        { }
                    }

                    public interface NestedInterface1
                    { }

                    public class NestedClass3<T>
                    {
                        public class NestedClass4
                        { }
                    }

                    public class NestedClass5<T1, T2>
                    {
                        public class NestedClass6<T3>
                        { }
                    }
                }
            ";

            using var assembly = Compile(cs);

            var class1 = assembly.MainModule.Types.Single(x => x.Name == "Class1");
            var nestedClass1 = class1.NestedTypes.Single(x => x.Name == "NestedClass1");
            var nestedClass2 = nestedClass1.NestedTypes.Single(x => x.Name == "NestedClass2");
            var nestedInterface1 = class1.NestedTypes.Single(x => x.Name == "NestedInterface1");
            var nestedClass3 = class1.NestedTypes.Single(x => x.Name == "NestedClass3`1");
            var nestedClass4 = nestedClass3.NestedTypes.Single(x => x.Name == "NestedClass4");
            var nestedClass5 = class1.NestedTypes.Single(x => x.Name == "NestedClass5`2");
            var nestedClass6 = nestedClass5.NestedTypes.Single(x => x.Name == "NestedClass6`1");


            // ACT / ASSERT
            Assert.Equal("public class Class1.NestedClass1", CSharpDefinitionFormatter.GetDefinition(nestedClass1));
            Assert.Equal("public class Class1.NestedClass1.NestedClass2", CSharpDefinitionFormatter.GetDefinition(nestedClass2));
            Assert.Equal("public interface Class1.NestedInterface1", CSharpDefinitionFormatter.GetDefinition(nestedInterface1));
            Assert.Equal("public class Class1.NestedClass3<T>", CSharpDefinitionFormatter.GetDefinition(nestedClass3));
            Assert.Equal("public class Class1.NestedClass3<T>.NestedClass4", CSharpDefinitionFormatter.GetDefinition(nestedClass4));
            Assert.Equal("public class Class1.NestedClass5<T1, T2>", CSharpDefinitionFormatter.GetDefinition(nestedClass5));
            Assert.Equal("public class Class1.NestedClass5<T1, T2>.NestedClass6<T3>", CSharpDefinitionFormatter.GetDefinition(nestedClass6));

        }

        [Fact]
        public void GetDefinition_returns_the_expected_definition_for_properties()
        {
            // ARRANGE
            var cs = @"
                using System;
                using System.IO;
                using System.Collections.Generic;

                public class SampleAttribute : Attribute
                {
                    public string Property1 { get; set; }

                    public SampleAttribute(int value)
                    { }
                }

                public class Class1
                {
                    public int Property1 { get; set; }

                    public byte Property2 { get; set; }

                    public string Property3 { get; }

                    public string Property4 { get; private set; }

                    public string Property5 { private get; set; }

                    public Stream Property6 { get; }

                    public IEnumerable<string> Property7 { get; }

                    public static IEnumerable<string> Property8 { get; }

                    [Sample(1)]
                    public static IEnumerable<string> Property9 { get; }
                }
            ";

            using var assembly = Compile(cs);

            var class1 = assembly.MainModule.Types.Single(x => x.Name == "Class1");
            var property1 = class1.Properties.Single(x => x.Name == "Property1");
            var property2 = class1.Properties.Single(x => x.Name == "Property2");
            var property3 = class1.Properties.Single(x => x.Name == "Property3");
            var property4 = class1.Properties.Single(x => x.Name == "Property4");
            var property5 = class1.Properties.Single(x => x.Name == "Property5");
            var property6 = class1.Properties.Single(x => x.Name == "Property6");
            var property7 = class1.Properties.Single(x => x.Name == "Property7");
            var property8 = class1.Properties.Single(x => x.Name == "Property8");
            var property9 = class1.Properties.Single(x => x.Name == "Property9");

            // ACT / ASSERT
            Assert.Equal("public int Property1 { get; set; }", CSharpDefinitionFormatter.GetDefinition(property1));
            Assert.Equal("public byte Property2 { get; set; }", CSharpDefinitionFormatter.GetDefinition(property2));
            Assert.Equal("public string Property3 { get; }", CSharpDefinitionFormatter.GetDefinition(property3));
            Assert.Equal("public string Property4 { get; }", CSharpDefinitionFormatter.GetDefinition(property4));
            Assert.Equal("public string Property5 { set; }", CSharpDefinitionFormatter.GetDefinition(property5));
            Assert.Equal("public Stream Property6 { get; }", CSharpDefinitionFormatter.GetDefinition(property6));
            Assert.Equal("public IEnumerable<string> Property7 { get; }", CSharpDefinitionFormatter.GetDefinition(property7));
            Assert.Equal("public static IEnumerable<string> Property8 { get; }", CSharpDefinitionFormatter.GetDefinition(property8));
            Assert.Equal(
                "[Sample(1)]\r\n" +
                "public static IEnumerable<string> Property9 { get; }",
                CSharpDefinitionFormatter.GetDefinition(property9)
            );
        }

        [Fact]
        public void GetDefinition_returns_the_expected_definition_for_fields()
        {
            // ARRANGE
            var cs = @"
                using System;
                using System.IO;
                using System.Collections.Generic;

                public class SampleAttribute : Attribute
                {
                    public string Property1 { get; set; }

                    public SampleAttribute(int value)
                    { }
                }

                public class Class1
                {
                    public string Field1;

                    public static string Field2;

                    public const string Field3 = """";

                    public static readonly int Field4;

                    [Sample(1)]
                    public static readonly int Field5;
                }
            ";

            using var assembly = Compile(cs);

            var class1 = assembly.MainModule.Types.Single(x => x.Name == "Class1");
            var field1 = class1.Fields.Single(x => x.Name == "Field1");
            var field2 = class1.Fields.Single(x => x.Name == "Field2");
            var field3 = class1.Fields.Single(x => x.Name == "Field3");
            var field4 = class1.Fields.Single(x => x.Name == "Field4");
            var field5 = class1.Fields.Single(x => x.Name == "Field5");

            // ACT / ASSERT
            Assert.Equal("public string Field1;", CSharpDefinitionFormatter.GetDefinition(field1));
            Assert.Equal("public static string Field2;", CSharpDefinitionFormatter.GetDefinition(field2));
            Assert.Equal("public const string Field3;", CSharpDefinitionFormatter.GetDefinition(field3));
            Assert.Equal("public static readonly int Field4;", CSharpDefinitionFormatter.GetDefinition(field4));
            Assert.Equal(
                "[Sample(1)]\r\n" +
                "public static readonly int Field5;",
                CSharpDefinitionFormatter.GetDefinition(field5));
        }

        [Fact]
        public void GetDefinition_returns_the_expected_definition_for_events()
        {
            // ARRANGE
            var cs = @"
                using System;

                public class SampleAttribute : Attribute
                {
                    public string Property1 { get; set; }

                    public SampleAttribute(int value)
                    { }
                }

                public class Class1
                {
                    public event EventHandler<EventArgs> Event1;

                    public static event EventHandler Event2;

                    [Sample(1)]
                    public static event EventHandler Event3;

                }
            ";

            using var assembly = Compile(cs);

            var class1 = assembly.MainModule.Types.Single(x => x.Name == "Class1");
            var event1 = class1.Events.Single(x => x.Name == "Event1");
            var event2 = class1.Events.Single(x => x.Name == "Event2");
            var event3 = class1.Events.Single(x => x.Name == "Event3");

            // ACT / ASSERT
            Assert.Equal("public event EventHandler<EventArgs> Event1;", CSharpDefinitionFormatter.GetDefinition(event1));
            Assert.Equal("public static event EventHandler Event2;", CSharpDefinitionFormatter.GetDefinition(event2));
            Assert.Equal(
                "[Sample(1)]\r\n" +
                "public static event EventHandler Event3;",
                CSharpDefinitionFormatter.GetDefinition(event3)
            );

        }

        [Fact]
        public void GetDefinition_returns_the_expected_definition_for_indexers()
        {
            // ARRANGE
            var cs = @"
                using System;
                using System.IO;

                public class Class1
                {
                    public int this[object parameter] { get { throw new NotImplementedException(); } }

                    public int this[object parameter1, Stream parameter2] { get { throw new NotImplementedException(); } }
                }
            ";

            using var assembly = Compile(cs);

            var class1 = assembly.MainModule.Types.Single(x => x.Name == "Class1");
            var indexer1 = class1.Properties.Single(p => p.Parameters.Count == 1);
            var indexer2 = class1.Properties.Single(p => p.Parameters.Count == 2);

            // ACT / ASSERT
            Assert.Equal("public int this[object parameter] { get; }", CSharpDefinitionFormatter.GetDefinition(indexer1));
            Assert.Equal("public int this[object parameter1, Stream parameter2] { get; }", CSharpDefinitionFormatter.GetDefinition(indexer2));

        }

        [Fact]
        public void GetDefinition_returns_the_expected_definition_for_extension_methods()
        {
            // ARRANGE
            // ARRANGE
            var cs = @"
                using System;
                using System.IO;

                public static class Class1
                {
                    public static void Method1(this string param) => throw new NotImplementedException();
                }
            ";

            using var assembly = Compile(cs);

            var class1 = assembly.MainModule.Types.Single(x => x.Name == "Class1");
            var method1 = class1.Methods.Single(x => x.Name == "Method1");

            // ACT / ASSERT
            Assert.Equal("public static void Method1(this string param);", CSharpDefinitionFormatter.GetDefinition(method1));
        }
    }
}
