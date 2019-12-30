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

    }
}
