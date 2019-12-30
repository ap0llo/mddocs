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

            // ACT
            Assert.Equal("public class MyClass", CSharpDefinitionFormatter.GetDefinition(typeDefinition));
            Assert.Equal("public string Property1 { get; }", CSharpDefinitionFormatter.GetDefinition(propertyDefinition));
            Assert.Equal("public string this[int index] { get; }", CSharpDefinitionFormatter.GetDefinition(indexerDefinition));
            Assert.Equal("public int Field1;", CSharpDefinitionFormatter.GetDefinition(fieldDefinition));
            Assert.Equal("public static event EventHandler Event1;", CSharpDefinitionFormatter.GetDefinition(eventDefinition));
            Assert.Equal("public void Method1(string parameter);", CSharpDefinitionFormatter.GetDefinition(methodDefinition));
        }
    }
}
