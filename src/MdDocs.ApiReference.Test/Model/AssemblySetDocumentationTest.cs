using System.Linq;
using Grynwald.MdDocs.ApiReference.Model;
using Grynwald.MdDocs.TestHelpers;
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

        [Fact]
        public void Namespaces_includes_expected_namespaces()
        {
            // ARRANGE
            var cs1 = @"
	            using System;

                namespace Namespace1
                {
	                public class Class1
	                { }
                }

                namespace Namespace1.Namespace2
                {
	                public class Class2
	                { }
                }

                namespace Namespace3
                {
	                public class Class3
	                { }
                }           
            ";

            var cs2 = @"
	            using System;

                namespace Namespace1
                {
	                public class Class4
	                { }
                }

                namespace Namespace4
                {
	                public class Class4
	                { }
                }           
            ";

            var assembly1 = Compile(cs1, "Assembly1");
            var assembly2 = Compile(cs2, "Assembly2");
            using var sut = AssemblySetDocumentation.FromAssemblyDefinitions(assembly1, assembly2);

            var expectedNamespaces = new[]
            {
                new NamespaceId("Namespace1"),
                new NamespaceId("Namespace1.Namespace2"),
                new NamespaceId("Namespace3"),
                new NamespaceId("Namespace4")
            };

            // ACT
            var actualNamespaces = sut.Namespaces;

            // ASSERT
            Assert.Equal(expectedNamespaces.Length, actualNamespaces.Count);
            Assert.All(
                expectedNamespaces,
                expectedNamespace => Assert.Contains(actualNamespaces, x => x.NamespaceId.Equals(expectedNamespace))
            );

            var namespace1 = Assert.Single(actualNamespaces.Where(x => x.Name == "Namespace1"));
            Assert.Contains(namespace1.Types, t => t.TypeId.Name == "Class1");
            Assert.Contains(namespace1.Types, t => t.TypeId.Name == "Class4");
        }

        [Fact]
        public void Namespaces_does_include_namespaces_that_contain_only_internal_types()
        {
            // ARRANGE
            var cs1 = @"
	            using System;

                namespace Namespace1
                {
	                public class Class1
	                { }
                }

                namespace Namespace2
                {
	                internal class Class2
	                { }
                }           
            ";
            var cs2 = @"
	            using System;

                namespace Namespace1
                {
	                public class Class3
	                { }
                }

                namespace Namespace3
                {
	                internal class Class4
	                { }
                }           
            ";

            var assembly1 = Compile(cs1, "Assembly1");
            var assembly2 = Compile(cs2, "Assembly2");
            using var sut = AssemblySetDocumentation.FromAssemblyDefinitions(assembly1, assembly2);

            var expectedNamespaces = new[]
            {
                new NamespaceId("Namespace1")
            };

            // ACT
            var actualNamespaces = sut.Namespaces;

            // ASSERT
            Assert.Equal(expectedNamespaces.Length, actualNamespaces.Count);
            Assert.All(
                expectedNamespaces,
                expectedNamespace => Assert.Contains(actualNamespaces, x => x.NamespaceId.Equals(expectedNamespace))
            );
        }

        [Fact]
        public void TryGetDocumentation_returns_null_for_an_undocumented_type()
        {
            // ARRANGE
            var cs1 = @"
                namespace Namespace1.Namespace2
                {
                    internal class InternalClass1
                    { }
                }
            ";
            var cs2 = @"
                namespace Namespace1.Namespace2
                {
                    internal class InternalClass2
                    { }
                }
            ";

            using var assembly1 = Compile(cs1, "Assembly1");
            using var assembly2 = Compile(cs2, "Assembly2");

            var typeId1 = assembly1.MainModule.Types.Single(x => x.Name == "InternalClass1").ToTypeId();
            var typeId2 = assembly2.MainModule.Types.Single(x => x.Name == "InternalClass2").ToTypeId();

            using var sut = AssemblySetDocumentation.FromAssemblyDefinitions(assembly1, assembly2);

            // ACT
            var documentation1 = sut.TryGetDocumentation(typeId1);
            var documentation2 = sut.TryGetDocumentation(typeId2);

            // ASSERT
            Assert.Null(documentation1);
            Assert.Null(documentation2);
        }

        [Fact]
        public void TryGetDocumentation_returns_expected_documentation_item_for_an_documented_type()
        {
            // ARRANGE
            var cs1 = @"
                namespace Namespace1.Namespace2
                {
                    public class Class1
                    { }
                }
            ";
            var cs2 = @"
                namespace Namespace3
                {
                    public class Class2
                    { }
                }
            ";

            var assembly1 = Compile(cs1, "Assembly1");
            var assembly2 = Compile(cs2, "Assembly2");

            var typeId1 = assembly1.MainModule.Types.Single(x => x.Name == "Class1").ToTypeId();
            var typeId2 = assembly2.MainModule.Types.Single(x => x.Name == "Class2").ToTypeId();
            var namespaceId1 = new NamespaceId("Namespace1.Namespace2");
            var namespaceId2 = new NamespaceId("Namespace3");

            using var sut = AssemblySetDocumentation.FromAssemblyDefinitions(assembly1, assembly2);

            // ACT
            var documentation1 = sut.TryGetDocumentation(typeId1);
            var documentation2 = sut.TryGetDocumentation(typeId2);
            var documentation3 = sut.TryGetDocumentation(namespaceId1);
            var documentation4 = sut.TryGetDocumentation(namespaceId2);

            // ASSERT
            Assert.NotNull(documentation1);
            var typeDocumentation1 = Assert.IsType<TypeDocumentation>(documentation1);
            Assert.Equal(typeId1, typeDocumentation1.TypeId);

            Assert.NotNull(documentation2);
            var typeDocumentation2 = Assert.IsType<TypeDocumentation>(documentation2);
            Assert.Equal(typeId2, typeDocumentation2.TypeId);

            Assert.NotNull(documentation3);
            var namespaceDocumentation1 = Assert.IsType<NamespaceDocumentation>(documentation3);
            Assert.Equal(namespaceId1, namespaceDocumentation1.NamespaceId);

            Assert.NotNull(documentation4);
            var namespaceDocumentation2 = Assert.IsType<NamespaceDocumentation>(documentation4);
            Assert.Equal(namespaceId2, namespaceDocumentation2.NamespaceId);

        }

    }
}
