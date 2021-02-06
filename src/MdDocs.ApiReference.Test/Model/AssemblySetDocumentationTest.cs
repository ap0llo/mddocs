using System.Linq;
using Grynwald.MdDocs.ApiReference.Model;
using Grynwald.MdDocs.ApiReference.Model.XmlDocs;
using Grynwald.MdDocs.TestHelpers;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace Grynwald.MdDocs.ApiReference.Test.Model
{
    public class AssemblySetDocumentationTest : DynamicCompilationTestBase
    {
        [Theory]
        [InlineData("Assembly1", "Assembly1")]
        [InlineData("Assembly1", "assembly1")]
        [InlineData("ASSEMBLY1", "Assembly1")]
        public void FromAssemblyDefinitions_throws_InvalidAssemblySetException_if_set_contains_multiple_assemblies_with_the_same_name(string assemblyName1, string assemblyName2)
        {
            // ARRANGE
            var assembly1 = Compile("", assemblyName1);
            var assembly2 = Compile("", assemblyName2);

            // ACT 
            var ex = Record.Exception(() => AssemblySetDocumentation.FromAssemblyDefinitions(new[] { assembly1, assembly2 }));

            // ASSERT
            Assert.IsType<InvalidAssemblySetException>(ex);
            Assert.Contains("multiple assemblies named", ex.Message);
        }

        [Fact]
        public void Constructor_throws_InvalidAssemblySetException_if_a_type_exists_in_multiple_assemblies()
        {
            // ARRANGE
            var cs = @"
                namespace Namespace1
                {
                    public class Class1
                    {
                    }
                }   
            ";
            var assembly1 = Compile(cs, "Assembly1");
            var assembly2 = Compile(cs, "Assembly2");

            // ACT 
            var ex = Record.Exception(() => AssemblySetDocumentation.FromAssemblyDefinitions(new[] { assembly1, assembly2 }));

            // ASSERT
            Assert.IsType<InvalidAssemblySetException>(ex);
            Assert.Contains($"Type 'Namespace1.Class1' exists in multiple assemblies", ex.Message);
        }

        [Fact]
        public void Constructor_does_not_throw_InvalidAssemblySetException_if_a_type_exists_in_multiple_assemblies_but_is_not_public()
        {
            // ARRANGE
            var cs1 = @"
                namespace Namespace1
                {
                    public class Class1
                    {
                    }
                }   
            ";
            var cs2 = @"
                namespace Namespace1
                {
                    internal class Class1
                    {
                    }
                }   
            ";
            var assembly1 = Compile(cs1, "Assembly1");
            var assembly2 = Compile(cs2, "Assembly2");

            // ACT 
            var sut = AssemblySetDocumentation.FromAssemblyDefinitions(new[] { assembly1, assembly2 });

            // ASSERT
            Assert.Equal(2, sut.Assemblies.Count);
        }

        [Fact]
        public void Assemblies_returns_expected_assemblies()
        {
            // ARRANGE
            var assembly1 = Compile("", "Assembly1");
            var assembly2 = Compile("", "Assembly2");

            // ACT
            var sut = AssemblySetDocumentation.FromAssemblyDefinitions(new[] { assembly1, assembly2 });

            // ASSERT
            Assert.Equal(2, sut.Assemblies.Count);
            Assert.Contains(sut.Assemblies, x => x.Name == "Assembly1");
            Assert.Contains(sut.Assemblies, x => x.Name == "Assembly2");
        }

        [Fact]
        public void TryGetDocumentation_can_resolve_types_in_any_of_the_sets_assembly()
        {
            // ARRANGE
            var cs1 = @"
                namespace Namespace1
                {
                    public class Class1
                    {
                    }
                }   
            ";
            var cs2 = @"
                namespace Namespace1
                {
                    public class Class2
                    {
                    }
                }   
            ";
            var assembly1 = Compile(cs1, "Assembly1");
            var assembly2 = Compile(cs2, "Assembly2");

            var sut = AssemblySetDocumentation.FromAssemblyDefinitions(new[] { assembly1, assembly2 });

            // ACT
            var documentation1 = sut.TryGetDocumentation(new SimpleTypeId("Namespace1", "Class1"));
            var documentation2 = sut.TryGetDocumentation(new SimpleTypeId("Namespace1", "Class2"));

            // ASSERT
            Assert.NotNull(documentation1);
            var typeDocumentation1 = Assert.IsType<TypeDocumentation>(documentation1);
            Assert.Equal("Assembly1", typeDocumentation1.AssemblyName);

            Assert.NotNull(documentation2);
            var typeDocumentation2 = Assert.IsType<TypeDocumentation>(documentation2);
            Assert.Equal("Assembly2", typeDocumentation2.AssemblyName);
        }

        [Fact]
        public void TryGetDocumentation_returns_null_if_type_is_not_found()
        {
            // ARRANGE
            var cs1 = @"
                namespace Namespace1
                {
                    public class Class1
                    {
                    }
                }   
            ";
            var cs2 = @"
                namespace Namespace1
                {
                    public class Class2
                    {
                    }
                }   
            ";
            var assembly1 = Compile(cs1, "Assembly1");
            var assembly2 = Compile(cs2, "Assembly2");

            var sut = AssemblySetDocumentation.FromAssemblyDefinitions(new[] { assembly1, assembly2 });

            // ACT
            var documentation = sut.TryGetDocumentation(new SimpleTypeId("Namespace1", "Class3"));

            // ASSERT
            Assert.Null(documentation);
        }
    }
}
